using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;

    private static SoundManager instance;
    private const string VolumeKey = "musicVolume";

    void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start() {
    PlayerPrefs.DeleteKey(VolumeKey);

    volumeSlider.value = 0.5f;  // Ensure slider starts at 50%
    
    if (!PlayerPrefs.HasKey(VolumeKey))
    {
        PlayerPrefs.SetFloat(VolumeKey, 0.5f);
        PlayerPrefs.Save(); // Immediately write the default to disk
    }
    Load();
}


    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Save();
    }

    private void Load()
    {
        float volumeValue = PlayerPrefs.GetFloat(VolumeKey);
        AudioListener.volume = volumeValue;

        if (volumeSlider != null)
        {
            volumeSlider.value = volumeValue;
        }
    }

    private void Save()
    {
        PlayerPrefs.SetFloat(VolumeKey, volumeSlider.value);
    }

    void OnEnable()
{
    volumeSlider = GameObject.Find("YourSliderGameObjectName").GetComponent<Slider>();
    Load(); 
}

}