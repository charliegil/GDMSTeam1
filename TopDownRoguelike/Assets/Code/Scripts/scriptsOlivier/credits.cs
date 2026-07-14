using System.Collections;
using UnityEngine;

public class credits : MonoBehaviour
{
    public Camera cam;
    public float speed = 1;

    public float waitAtFinish = 3;

    public GameObject whenToStop;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if (cam == null) cam = Camera.main;
    }
    IEnumerator waiting()
    {
        yield return new WaitForSeconds(waitAtFinish);
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    // Update is called once per frame.
    private void Update()
    {
        if (cam == null) cam = Camera.main;
        if (Mathf.Abs(cam.transform.position.y) > whenToStop.transform.position.y)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
            speed = 0;
            StartCoroutine("waiting");

        }


        cam.transform.position += new Vector3(0, -speed * Time.deltaTime, 0);
    }

    

}
