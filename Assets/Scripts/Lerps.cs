using UnityEngine;

public class Lerps
{
    public static void LinearLerp(Transform toLerp, Transform posA, Transform posB, float interpolateAmount)
    {
        float interpolateAmt = (interpolateAmount + Time.deltaTime) % 1f;

        toLerp.position = Vector2.Lerp(posA.position, posB.position, interpolateAmt);
    }

    public static void CubicLerp(Transform toLerp, Transform posA, Transform posB, Transform posC, float interpolateAmount)
    {
        float interpolateAmt = (interpolateAmount + Time.deltaTime) % 1f;
        Vector2 pointAB = Vector2.Lerp(posA.position, posB.position, interpolateAmt);
        Vector2 pointBC = Vector2.Lerp(posB.position, posC.position, interpolateAmt);

        toLerp.position = Vector2.Lerp(pointAB, pointBC, interpolateAmt);
    }

    public static void QuadraticLerp(Transform toLerp, Transform posA, Transform posB, Transform posC, Transform posD, float interpolateAmount)
    {
        float interpolateAmt = (interpolateAmount + Time.deltaTime) % 1f;

        Vector2 pointAB = Vector2.Lerp(posA.position, posB.position, interpolateAmt);
        Vector2 pointBC = Vector2.Lerp(posB.position, posC.position, interpolateAmt);
        Vector2 pointCD = Vector2.Lerp(posC.position, posD.position, interpolateAmt);

        Vector2 pointAB_BC = Vector2.Lerp(pointAB, pointBC, interpolateAmt);
        Vector2 pointBC_CD = Vector2.Lerp(pointBC, pointCD, interpolateAmt);

        toLerp.position = Vector2.Lerp(pointAB_BC, pointBC_CD, interpolateAmt);
    }
}
