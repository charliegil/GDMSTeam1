using UnityEngine;
using UnityEngine.SceneManagement; 

public class PlayAgainManager : MonoBehaviour
{
    public void PlayAgain()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
