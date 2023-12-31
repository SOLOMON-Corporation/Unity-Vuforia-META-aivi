using UnityEngine;
using Vuforia;
public class BarcodeCollider : MonoBehaviour
{
    // Start is called before the first frame update
    BarcodeBehaviour mBarcodeBehaviour;
    MeshCollider mMeshCollider;

    void OnDisable(){
        mBarcodeBehaviour.OnBarcodeOutlineChanged -= OnBarcodeOutlineChanged;
    }
    void OnEnable(){
        mBarcodeBehaviour = GetComponent<BarcodeBehaviour>();
        mBarcodeBehaviour.OnBarcodeOutlineChanged += OnBarcodeOutlineChanged;
    }
    void OnBarcodeOutlineChanged(Vector3[] vertices)
    {
        UpdateMeshCollider(vertices);
    }
    void UpdateMeshCollider(Vector3[] vertices)
    {        
        if (!mMeshCollider)
        {
            mMeshCollider = gameObject.AddComponent<MeshCollider>();
            mMeshCollider.cookingOptions = MeshColliderCookingOptions.None;
        }
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = new int []{ 0, 1, 2, 0, 2, 3 }; // Creates 2 triangles
        mMeshCollider.sharedMesh = mesh;
    }
}
