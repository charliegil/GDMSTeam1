using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created.
    
    public Texture2D cursorMenus;

    public void Start()
    {
        Cursor.SetCursor(cursorMenus,Vector2.zero, CursorMode.Auto);
    }
    public void play()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("EnemiesOlivier");
        
    }
    public void load()
    {
        Debug.Log("test");
    }

    


    public void quit()
    {
        Application.Quit();
    }
    public void settings()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("OptionsScene");
    }

    public void credits(){
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Credits");
    }


    public void goMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
    public void restart()
    { // sw
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
    public void showSkillTree(){

    }
    
}
