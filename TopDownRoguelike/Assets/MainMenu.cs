using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void play(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("Movement");
    }
    public void load(){
        Debug.Log("test");
    }

    public void quit(){
        Application.Quit();
    }
    public void settings(){
        
    }
}
