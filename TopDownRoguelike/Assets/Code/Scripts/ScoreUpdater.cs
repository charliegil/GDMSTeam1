using System.Net.Sockets;
using TMPro;
using UnityEngine;


public class ScoreManager : MonoBehaviour , IEventListener
{
    public static ScoreManager Instance;

    public int score = 0;

    public TextMeshProUGUI ScoreText;
    

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        } else {
            Destroy(gameObject);
        }
    }

    public void AddScore(int points) {
        score += points;
        UpdateScoreUI();
    }

    public void UpdateScoreUI() {
        if(ScoreText != null)
            ScoreText.text = "Score: " + score;
    }
    public void OnplayerDied(){
        int max = PlayerPrefs.GetInt("HighScore", 0);
        if(max > score) return;
        
        PlayerPrefs.SetInt("HighScore", score);
        PlayerPrefs.Save();
    }

    public void subscribe()
    {
        EventManager.OnPlayerDied += OnplayerDied;
    }

    public void unsubscribe()
    {
        EventManager.OnPlayerDied -= OnplayerDied;
    }
}

