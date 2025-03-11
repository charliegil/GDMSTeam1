using UnityEngine;

public class draggiingCamera : MonoBehaviour
{
    public Camera camera;

    public float sens = 1;

    private Vector3 oldPosition = Vector3.zero;
    private bool isDragging = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start(){
        if(camera == null) camera = Camera.main;
    }

    // Update is called once per frame.
    private void Update()
    {
        if(Input.GetMouseButton(0)){

            if(isDragging){
                Vector3 direction = (Input.mousePosition- oldPosition);
                camera.transform.position-= direction * Time.deltaTime*sens;
            }

            oldPosition = Input.mousePosition;
            isDragging = true;
        }
        else{
            oldPosition = Vector3.zero;
            isDragging = false;
        }


    }
}
