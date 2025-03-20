using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))] 
public class EnergyBeam : MonoBehaviour
{
    public Transform target; // Locked-on enemy
    public int pointCount = 20; // More points for smoother curves
    public float waveAmplitude = 0.5f; // Strength of beam distortion
    public float waveFrequency = 5f; // Speed of wave oscillation
    public float beamWidth = 0.2f; // Thickness of beam
    public Material beamMaterial; // Glowing material

    private LineRenderer lineRenderer;
    private List<Vector3> points = new List<Vector3>();

    
    private void Awake() {
        if (!TryGetComponent(out LineRenderer _)) {
            gameObject.AddComponent<LineRenderer>();
        }
        lineRenderer = GetComponent<LineRenderer>();
    }
    private void Start()
    {
        lineRenderer.positionCount = pointCount;
        lineRenderer.material = beamMaterial;
        lineRenderer.startWidth = beamWidth;
        lineRenderer.endWidth = beamWidth;

        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
        //lineRenderer.useWorldSpace = false;
        lineRenderer.sortingOrder = 3;
    }

    private void Update()
    {
        if (target == null) {
            lineRenderer.enabled = false;
            return;
        }

        lineRenderer.enabled = true;

        // Start and end points
        Vector3 startPoint = transform.position;
        Vector3 endPoint = target.position;
        points.Clear();

        // Beam direction
        Vector3 beamDirection = (endPoint - startPoint).normalized;
        Vector3 perpendicular = new Vector3(-beamDirection.y, beamDirection.x, 0); // Perpendicular in 2D

        // Add first (fixed) point
        points.Add(startPoint);

        // Generate middle points with sine wave distortion
        for (int i = 1; i < pointCount - 1; i++) // Skip first and last points
        {
            float t = (float)i / (pointCount - 1); // Normalized position (0 to 1)
            Vector3 position = Vector3.Lerp(startPoint, endPoint, t);

            // Tapering function (strongest distortion in the center, none at edges)
            float taper = Mathf.Sin(t * Mathf.PI); 

            // Apply sine wave distortion only to middle points
            float waveOffset = Mathf.Sin(Time.time * waveFrequency + i * 0.5f) * waveAmplitude * taper;
            Vector3 offset = perpendicular * waveOffset;

            points.Add(position + offset);
        }

        // Add last (fixed) point
        points.Add(endPoint);

        // Apply points to LineRenderer
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}