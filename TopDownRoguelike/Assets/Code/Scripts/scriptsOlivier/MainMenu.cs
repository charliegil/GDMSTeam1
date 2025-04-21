using UnityEngine;

public class MainMenu : MonoBehaviour
{
    
    public Texture2D cursorMenus;

    public void Start()
    {
        //Cursor.SetCursor(cursorMenus,Vector2.zero, CursorMode.Auto);
    }
    public void play()
    {
        Time.timeScale = 1;
        AudioManager.instance.EnterNewScene(true);
        UnityEngine.SceneManagement.SceneManager.LoadScene("main_finalVersion");
        
    }
    public void load()
    {
        Debug.Log("test");
    }

    


    public void quit()
    {
    #if UNITY_WEBGL
        UnityEngine.SceneManagement.SceneManager.LoadScene("WebGLQuit");
    #else
        Application.Quit();
    #endif
    }
    public void settings()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("OptionsScene");
    }
    public void tutorial(){
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Tutorial");
    }

    public void credits(){
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Credits");
    }


    public void goMenu()
    {
        AudioManager.instance.EnterNewScene(false);
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
    public void restart()
    { 
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
    public void showSkillTree(){

    }
    
}
