using UnityEngine;

public class credits : MonoBehaviour
{
    public Camera cam;
    public float speed =1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if(cam== null) cam = Camera.main;
    }

    // Update is called once per frame.
    private void Update()
    {
        if(cam== null) cam = Camera.main;
        if(Mathf.Abs(cam.transform.position.y)> 21.5) UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        cam.transform.position+= new Vector3(0,-speed*Time.deltaTime,0);
    }
    
}
