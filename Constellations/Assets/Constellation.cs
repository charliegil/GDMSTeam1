using UnityEngine;
using System.Collections.Generic;

public class OrionConstellation : MonoBehaviour
{
    private const float DistanceScale = 0.005f;
    private const float RadiusScale = 0.002f;
    private List<GameObject> stars = new List<GameObject>(); // Store stars

    void Start()
    {
        CreateStar("Betelgeuse", 5, 55, 10, 7, 24, 24, 642, 887, 50f);
        CreateStar("Rigel", 5, 14, 32, -8, 12, 6, 860, 78, 100f);
        CreateStar("Bellatrix", 5, 25, 7, 6, 20, 59, 250, 5.75f, 30f);
        CreateStar("Mintaka", 5, 32, 0, -0, 17, 56, 1200, 16, 40f);
        CreateStar("Alnilam", 5, 36, 12, -1, 12, 7, 2000, 32, 70f);
        CreateStar("Alnitak", 5, 40, 45, -1, 56, 34, 1260, 20, 60f);
        CreateStar("Saiph", 5, 47, 45, -9, 40, 10, 650, 22, 45f);

        AdjustCamera();
    }

    void CreateStar(string name, float raH, float raM, float raS, float decD, float decM, float decS, float distance, float radius, float brightness)
    {
        Vector3 position = ConvertToCartesian(raH, raM, raS, decD, decM, decS, distance);
        GameObject star = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        star.name = name;
        star.transform.position = position;
        star.transform.localScale = Vector3.one * (radius * RadiusScale);
        SetBrightMaterial(star, Color.white, brightness);
        stars.Add(star); // Store star in list
    }

    void AdjustCamera()
    {
        Vector3 center = Vector3.zero;
        foreach (var star in stars) center += star.transform.position;
        center /= stars.Count;

        float maxDistance = 0f;
        foreach (var star in stars)
        {
            float distance = Vector3.Distance(center, star.transform.position);
            if (distance > maxDistance) maxDistance = distance;
        }

        float cameraDistance = maxDistance * 2.5f;
        Camera.main.transform.position = center + new Vector3(0, 0, -cameraDistance);
        Camera.main.transform.LookAt(center);
    }

    Vector3 ConvertToCartesian(float raH, float raM, float raS, float decD, float decM, float decS, float distance)
    {
        float raDeg = (raH * 15) + (raM * 0.25f) + (raS * (15f / 3600f));
        float raRad = raDeg * Mathf.Deg2Rad;
        float decDeg = decD + (decM / 60f) + (decS / 3600f);
        float decRad = decDeg * Mathf.Deg2Rad;

        float x = distance * Mathf.Cos(decRad) * Mathf.Cos(raRad);
        float y = distance * Mathf.Cos(decRad) * Mathf.Sin(raRad);
        float z = distance * Mathf.Sin(decRad);

        return new Vector3(x, y, z) * DistanceScale;
    }

    void SetBrightMaterial(GameObject star, Color color, float intensity)
    {
        Renderer renderer = star.GetComponent<Renderer>();
        Material material = new Material(Shader.Find("Standard"));
        material.EnableKeyword("_EMISSION");
        material.SetColor("_EmissionColor", color * intensity);
        renderer.material = material;
    }
}
