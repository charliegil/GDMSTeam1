using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void play(){
        Time.timeScale = 1;
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
    public void goMenu(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
    public void restart(){ // sw
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
