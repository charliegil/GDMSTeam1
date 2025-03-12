using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created.
    public GameObject PauseUI;

    public GameObject SkillTreeUI;

    public GameObject Canva;

    [SerializeField] private KeyCode keyToPause = KeyCode.M;

    private bool isPausing;

    public void Start()
    {
        PauseUI.SetActive(false);
        //Canva.SetActive(false);
        SkillTreeUI.SetActive(false);
    }

    public void pause()
    {
       if(isPausing){
            PauseUI.SetActive(false);
            //Canva.SetActive(false);
            SkillTreeUI.SetActive(false);
            isPausing = false;
            Time.timeScale = 1;
            Debug.Log("resuming");
       }
       else{
        Debug.Log("pausing");
        PauseUI.SetActive(true);
        SkillTreeUI.SetActive(false);
        Debug.Log(PauseUI.name);
        //Canva.SetActive(true);
        isPausing = true;
        Time.timeScale = 0;
       }
    }
    public void Update()
    {
        if(Input.GetKeyDown(keyToPause)){
            pause();
            
        }
    }


    public void quit()
    {
        Application.Quit();
    }
    public void settings()
    {

    }
    public void goMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
    public void showSkillTree(){
        SkillTreeUI.SetActive(true);
        PauseUI.SetActive(false);
        Debug.Log("show skill tree");
    }
    public void closeSkillTree(){
        SkillTreeUI.SetActive(false);
        PauseUI.SetActive(true);
        Debug.Log("close skill tree");
    }
    
}
