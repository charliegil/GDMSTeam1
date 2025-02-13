using UnityEngine;

public class PlayerAttackLineRenderer : MonoBehaviour
{
    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
    }

    public void DrawAttackLine(Vector3 start, Vector3 end) {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        lineRenderer.enabled = true;
    }

    public void ClearLine() {
        lineRenderer.enabled = false;
    }
}
