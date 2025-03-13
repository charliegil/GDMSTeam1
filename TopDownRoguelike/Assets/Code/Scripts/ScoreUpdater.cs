using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public class ScoreManager : MonoBehaviour
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
}

