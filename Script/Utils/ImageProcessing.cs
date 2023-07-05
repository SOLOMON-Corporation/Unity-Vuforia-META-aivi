using UnityEngine;
using System.IO;
using System;
using Vuforia;
using Image = Vuforia.Image;
using OpenCvSharp;
using System.Linq;
using System.Collections.Generic;

public static class ImageProcessing
{
    public static string savePath = "Assets/SavedImages/";
    public static string fileName = "CameraImage.png";
    private static Texture2D screenShot;
    private static RenderTexture renderTexture;
    // public static RawImage RawImageCrop;
    public static void SaveImageToAssets(byte[] bytes)
    {
        // Debug.Log("SaveImageToAssets");
        string timestamp = "1";//DateTime.UtcNow.ToString("ss.fff");
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        File.WriteAllBytes(savePath + timestamp + fileName, bytes);
    }
    static Vector2 convert3Dto2D(Vector3 CenterPt3d, Vector2 FocalLength, Vector2 PrincipalPoint)
    {
        return new Vector2(CenterPt3d.x * FocalLength.x / CenterPt3d.z + PrincipalPoint.x, CenterPt3d.y * FocalLength.y / CenterPt3d.z + PrincipalPoint.y);
    }

    public static Texture2D crop_image(Texture2D copyTexture)
    {
        GameObject CameraObj = GameObject.Find("ARCamera");
        GameObject ImageTargetObj = GameObject.Find("ImageTarget");
        Transform CameraObjTran = CameraObj.transform;
        Transform ImageTargetObjTran = ImageTargetObj.transform;
        Matrix4x4 CamPose = CameraObjTran.localToWorldMatrix;
        Matrix4x4 ImageTargetPose = ImageTargetObjTran.localToWorldMatrix;
        Matrix4x4 ImageTarget2Cam = CamPose.inverse * ImageTargetPose;//Unity matrix multiply is left-hand: ImageTargetPose * CamPose.inverse; 
        if (ImageTarget2Cam == Matrix4x4.identity)
            return null; //camera and image target are at the same place

        //PROJECTION MODEL
        Vector2 PrincipalPoint = VuforiaBehaviour.Instance.CameraDevice.GetCameraIntrinsics().PrincipalPoint;
        Vector2 FocalLength = VuforiaBehaviour.Instance.CameraDevice.GetCameraIntrinsics().FocalLength;
        // Vector3 CenterPt3d = ImageTarget2Cam.GetPosition();// + StationStageIndex.stagePosition;// Transform to game object position, not right on the image target
        // Vector2 CenterPt2d = convert3Dto2D(CenterPt3d, FocalLength, PrincipalPoint);//new Vector2(CenterPt3d.x*FocalLength.x/ CenterPt3d.z+PrincipalPoint.x, CenterPt3d.y * FocalLength.y / CenterPt3d.z + PrincipalPoint.y);
        //Get four corners in 2D
        var dim_x = StationStageIndex.stageMeshRenderBoundSize[0]*(1+ StationStageIndex.marginXdata);//add margin
        var dim_y = StationStageIndex.stageMeshRenderBoundSize[2]*(1+ StationStageIndex.marginYdata);
        Vector3 Corner1Pt3d = (ImageTarget2Cam * Matrix4x4.Translate(new Vector3(- dim_x/ 2, 0, -dim_y / 2))).GetPosition()+ StationStageIndex.stagePosition;
        Vector3 Corner2Pt3d = (ImageTarget2Cam * Matrix4x4.Translate(new Vector3(-dim_x / 2, 0, dim_y / 2))).GetPosition()+ StationStageIndex.stagePosition;
        Vector3 Corner3Pt3d = (ImageTarget2Cam * Matrix4x4.Translate(new Vector3(dim_x / 2, 0, dim_y / 2))).GetPosition() + StationStageIndex.stagePosition;
        Vector3 Corner4Pt3d = (ImageTarget2Cam * Matrix4x4.Translate(new Vector3(dim_x / 2, 0, -dim_y / 2))).GetPosition() + StationStageIndex.stagePosition;
        Vector2 Corner1Pt2d = convert3Dto2D(Corner1Pt3d, FocalLength, PrincipalPoint);
        Vector2 Corner2Pt2d = convert3Dto2D(Corner2Pt3d, FocalLength, PrincipalPoint);
        Vector2 Corner3Pt2d = convert3Dto2D(Corner3Pt3d, FocalLength, PrincipalPoint);
        Vector2 Corner4Pt2d = convert3Dto2D(Corner4Pt3d, FocalLength, PrincipalPoint);
        // Debug.Log("0329 imageprocessing.cs " + Corner1Pt3d + Corner2Pt3d+ Corner3Pt3d+ Corner4Pt3d);

        //convert to opencv
        Mat edges = new Mat();
        Mat imgMat = new Mat(copyTexture.height, copyTexture.width, MatType.CV_8UC4);
        imgMat = OpenCvSharp.Unity.TextureToMat(copyTexture);
        Point2d[] obj = new Point2d[4];
        Point2d[] scene = new Point2d[4];
        obj[0] = new OpenCvSharp.Point(Corner1Pt2d.x, Corner1Pt2d.y);
        obj[1] = new OpenCvSharp.Point(Corner2Pt2d.x, Corner2Pt2d.y);
        obj[2] = new OpenCvSharp.Point(Corner3Pt2d.x, Corner3Pt2d.y);
        obj[3] = new OpenCvSharp.Point(Corner4Pt2d.x, Corner4Pt2d.y);

        // Resize to pixel 
        int convertRatio;
        float xRatio = (float)StationStageIndex.metaAPIimageResize[0]/dim_x;
        float yRatio = (float)StationStageIndex.metaAPIimageResize[1]/dim_y;
        if (StationStageIndex.metaAPIimageResize[0]/dim_x > StationStageIndex.metaAPIimageResize[1]/dim_y){
            convertRatio = (int)yRatio;
        }
        else{
            convertRatio = (int)xRatio;
        }

        int w = (int)(dim_x *StationStageIndex.foreground2backgroundRatio*convertRatio);
        int h = (int)(dim_y * StationStageIndex.foreground2backgroundRatio*convertRatio);
        // Debug.LogError("0329 Debug dimx dimy =" + w + h);
        // Debug.LogError("Corner2Pt2d=" + Corner2Pt2d.ToString());
        scene[0] = new OpenCvSharp.Point(w - 1, h - 1);
        scene[1] = new OpenCvSharp.Point(w - 1, 0);
        scene[2] = new OpenCvSharp.Point(0, 0);
        scene[3] = new OpenCvSharp.Point(0, h - 1);
        Mat H = Cv2.FindHomography(obj, scene);
        Mat outputMat = new Mat(h, w, MatType.CV_8UC4);
         
        Cv2.WarpPerspective(imgMat, outputMat, H, outputMat.Size() );//new Size(w, h)
        Texture2D outputTexture = new Texture2D(outputMat.Cols, outputMat.Rows, TextureFormat.RGBA32, false);

        //Utils.matToTexture2D(outputMat, outputTexture);
        outputTexture = OpenCvSharp.Unity.MatToTexture(outputMat);
        //save_image(outputTexture);
        return outputTexture;

        // RawImageCrop.texture = outputTexture;
        // RawImageCrop.material.mainTexture = outputTexture;
    }
    public static UnityEngine.Rect ConvertLTRB2XYWH(int[] ltrb){
        return new UnityEngine.Rect((int)((ltrb[0]+ ltrb[2])/2), (int)((ltrb[1]+ ltrb[3])/2),(int)((ltrb[2]- ltrb[0])),(int)((ltrb[3]- ltrb[1])));
    }
    // public static Texture2D AddBackgroundToImage(Texture2D texture)
    public static Texture2D AddBackgroundToImage(Texture2D foregroundImage)
    {
        // Create a new texture with the desired size and background color
        // Color[] backgroundColors = Enumerable.Repeat(Color.black, 640 * 480).ToArray();
        Texture2D backgroundImage = new Texture2D(640, 480);
        // backgroundImage.SetPixels(backgroundColors);
        // backgroundImage.Apply();

        // Calculate the position where the foreground image should be placed
        int posX = (backgroundImage.width - foregroundImage.width) / 2;
        int posY = (backgroundImage.height - foregroundImage.height) / 2;

        // Copy the foreground image onto the background image at the calculated position
        Color[] foregroundColors = foregroundImage.GetPixels();
        backgroundImage.SetPixels(posX, posY, foregroundImage.width, foregroundImage.height, foregroundColors);

        // Apply the changes to the texture
        backgroundImage.Apply();

        // Return the result
        return backgroundImage;
    }
    public static void TakeScreenshot()
    {
        // Set the camera's target texture to the render texture
        Camera.main.targetTexture = renderTexture;
        // Render the scene
        Camera.main.Render();
        // Read the pixels from the render texture
        RenderTexture.active = renderTexture;
        screenShot.ReadPixels(new UnityEngine.Rect(0, 0, Screen.width, Screen.height), 0, 0);
        // Reset the target texture and active render texture
        // Camera.main.targetTexture = null;
        // RenderTexture.active = null;
        // Apply the pixels to the texture
        screenShot.Apply();
        // Save the screenshot to file
        System.IO.File.WriteAllBytes("screenshot.png", screenShot.EncodeToPNG());
        // DisplayImageOnUI(screenShot);
        // return screenShot;
    }

    public static void OnlineCamera(){
        Camera.main.targetTexture = null;
        RenderTexture.active = null;
    }

    public static Texture2D ResizeTexture(Texture2D texture2D, int targetX, int targetY)
    {
        RenderTexture rt=new RenderTexture(targetX, targetY,24);
        RenderTexture.active = rt;
        Graphics.Blit(texture2D,rt);
        Texture2D result=new Texture2D(targetX,targetY);
        result.ReadPixels(new UnityEngine.Rect(0,0,targetX,targetY),0,0);
        result.Apply();
        return result;
    }

}
