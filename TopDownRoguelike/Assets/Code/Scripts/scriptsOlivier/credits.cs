using UnityEngine;

public class credits : MonoBehaviour
{
    public Camera cam;
    public float speed =1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(cam== null) cam = Camera.main;
    }

    // Update is called once per frame.
    void Update()
    {
        if((cam.transform.position + new Vector3(0,0,10)).magnitude< 23)
        cam.transform.position+= new Vector3(0,-speed*Time.deltaTime,0);
    }
}
