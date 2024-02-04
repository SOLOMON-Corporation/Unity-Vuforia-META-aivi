using UnityEngine;

public static class GeometryUtils
{
    // Start is called before the first frame update
    public static Vector3 CalculateNormal(Vector3 positionA, Vector3 positionB, Vector3 positionC)
    {
        Vector3 sideAB = positionB - positionA;
        Vector3 sideAC = positionC - positionA;
        Vector3 normal = Vector3.Cross(sideAB, sideAC).normalized;
        return normal;
    }
}
