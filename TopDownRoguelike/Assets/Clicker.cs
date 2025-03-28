using UnityEngine;

public class Clicker : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created.
    

    public void play()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
    
    
}

