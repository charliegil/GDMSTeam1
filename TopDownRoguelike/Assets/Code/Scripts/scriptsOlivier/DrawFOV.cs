using UnityEngine;


public class DrawFOV : MonoBehaviour{

    private LineRenderer FOVLines;


    
    public void drawLines(int range , int angle){
        // Start of the FOV renderer
        float lineWidth = 0.08f;
        FOVLines = GetComponent<LineRenderer>();
        if (FOVLines == null) FOVLines = gameObject.AddComponent<LineRenderer>();
        FOVLines.startWidth = lineWidth;
        FOVLines.endWidth = lineWidth;
        FOVLines.useWorldSpace = false;
        FOVLines.sortingLayerName = "Default";  
        FOVLines.sortingOrder = 10;

        Vector3 start = transform.position;
        Vector3 dir1 = RotateVector(Vector3.right, angle/2);
        Vector3 dir2 = RotateVector(Vector3.right, -angle/2);

        Vector3[] arcPoints = GenerateArc(start, start + dir1, start + dir2, 20,range);
        FOVLines.positionCount = 4 + arcPoints.Length; 
        
        FOVLines.SetPosition(0, start);
        FOVLines.SetPosition(1, start + dir2 * range);
        FOVLines.SetPosition(2, start);
        FOVLines.SetPosition(3, start + dir1 * range);
        
        for (int i = 0; i < arcPoints.Length; i++)
        {
            FOVLines.SetPosition(i + 4, arcPoints[i]);
        }
        // End of the FOV renderer
    }

    private Vector3 RotateVector(Vector3 v, float degrees){
        float rad = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        return new Vector3(v.x*cos-v.y*sin, v.x*sin + v.y*cos);
    }

        Vector3[] GenerateArc(Vector3 center, Vector3 pointA, Vector3 pointB, int resolution , int viewDistance){
        float radius = (pointA-center).magnitude;
        float startAngle = 0;
        float endAngle = Vector3.SignedAngle((pointA-center), (pointB - center), transform.forward);
        

        float step = (endAngle -startAngle)/ resolution;
        float angle = startAngle;
        Vector3[] arcPoints = new Vector3[resolution];
        for(int i = 0; i < resolution; i++){
            angle += step;
            if(startAngle > endAngle)Debug.Log("there is a problem with circle generation");
            arcPoints[i] = viewDistance * (RotateVector((pointA) , angle)); 
        }

        return arcPoints;
    }
    


}