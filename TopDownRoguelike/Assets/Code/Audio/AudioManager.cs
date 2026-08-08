using UnityEngine;
using System.Collections.Generic;

using System.Linq;
using System.Collections;

public class AudioManager : MonoBehaviour, IEventListener
{
    
    [Range(0f, 1f)] public float musicVolume= 0.2f;

   

    public static AudioManager instance;
    public AudioSource audioSourcePrefab;

    public AudioClip battleThemeMusic;

    public AudioClip BossThemeMusic;

    public AudioClip lowHealthSoundMusic;

    public AudioClip MainMenuThemeMusic;

    private AudioSource battleTheme;
    private AudioSource bossTheme;
    
    private AudioSource MainMenuTheme;
    private AudioSource lowHealthSound;


    public Coroutine battleCoroutine = null;
    public Coroutine BossThemeCoroutine=  null;
    private List<AudioSource> audioSources = new List<AudioSource>();


    float volumeBoss;
    float volumeBattle;

    [SerializeField] private List<Sound> sounds  = new List<Sound>();

    private void Awake()
    {
        if (instance == null){ 
            instance = this;
            DontDestroyOnLoad(instance);  
            DontDestroyOnLoad(gameObject);
            
            GameObject battle = new GameObject("BattleTheme");    
            battle.transform.SetParent(gameObject.transform);
            battleTheme = battle.AddComponent<AudioSource>();
            battleTheme.clip = battleThemeMusic;
            battleTheme.volume = 0;
            DontDestroyOnLoad(battle);
           
            GameObject boss = new GameObject("BossTheme");    
            boss.transform.SetParent(gameObject.transform);
            bossTheme = boss.AddComponent<AudioSource>();
            bossTheme.clip = BossThemeMusic;
            bossTheme.volume = 0;
            DontDestroyOnLoad(boss);

            GameObject lowHealth = new GameObject("LowHealthClip");    
            lowHealth.transform.SetParent(gameObject.transform);
            lowHealthSound = lowHealth.AddComponent<AudioSource>();
            lowHealthSound.clip = lowHealthSoundMusic;
            lowHealthSound.volume = 0;
            DontDestroyOnLoad(lowHealth);

            GameObject mainMenu = new GameObject("MainMenuTheme");    
            mainMenu.transform.SetParent(gameObject.transform);
            MainMenuTheme = mainMenu.AddComponent<AudioSource>();
            MainMenuTheme.clip = MainMenuThemeMusic;
            MainMenuTheme.volume = 0;
            DontDestroyOnLoad(mainMenu);


            battleTheme.loop = true;
            bossTheme.loop = true;
            lowHealthSound.loop = true;
            MainMenuTheme.loop = true;

            battleTheme.playOnAwake = false;
            bossTheme.playOnAwake = false;
            lowHealthSound.playOnAwake = false;
            MainMenuTheme.playOnAwake = false;
            
            subscribe();

            string name = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            Debug.Log(name + " is the current scene");
            
            if(name.Equals("main_finalVersion")){
                EnterNewScene(true);
            }
            else{
                EnterNewScene(false);
            }

        }
        else Destroy(gameObject);
        


        
    }

/// <summary>
/// entering a new scene
/// </summary>
/// <param name="scene"> true for the game scene, false for main menu scene</param>
    public void EnterNewScene(bool scene){
        if(scene){ // stop main menu musci and start battle theme with fade out
            MainMenuTheme.Stop();
            battleTheme.Play();
            battleTheme.volume = musicVolume;
            //StartCoroutine(FadeOutAudio(battleTheme,3,musicVolume));
        }
        else{
            battleTheme.Stop();
            bossTheme.Stop();
            lowHealthSound.Stop();
            MainMenuTheme.Play();
            MainMenuTheme.volume = musicVolume;
            //StartCoroutine(FadeOutAudio(MainMenuTheme,3,musicVolume));
        }
    }

    private void OnDisable()
    {
        unsubscribe();
    }

    private AudioSource GetAvailableSource()
    {
        foreach (AudioSource source in audioSources){
            
            if (source != null && !source.isPlaying) return source; 
        }
        AudioSource Source = Instantiate(audioSourcePrefab, transform);
        DontDestroyOnLoad(Source);
        audioSources.Add(Source);
        return Source;
    }

    
    public void PlaySound(string clipName){
        //if(clipName.Equals("PlayerDeath")) battleTheme.mute = true;
        Sound clip = sounds.FirstOrDefault(s => s.name.Contains(clipName));
        if(clip!=null){
            AudioSource source = GetAvailableSource();
            source.volume = clip.volume;
            source.PlayOneShot(clip.clip);
        }
    }

    public void OnPlayerDeath(){
        //battleTheme.mute = true;
        lowHealthSound.mute  = true;
        PlaySound("PlayerDeath");
        PlaySound("BossBell");
        StartCoroutine(FadeOutAudio(battleTheme,3,0));
        StartCoroutine(FadeOutAudio(bossTheme,3,0));

    }

    public void EnterMenu(bool enter){
        if(enter){
            battleTheme.volume/=2;
            bossTheme.volume/=2;
        }
        else{
            battleTheme.volume*=2;
            bossTheme.volume*=2;
        }
    }
    public void ActivateBossTheme(bool activate){
        if(activate){
            bossTheme.Play();
            //StartCoroutine(FadeOutAudio(bossTheme,2,musicVolume));
            bossTheme.volume = musicVolume;
            StartCoroutine(FadeOutAudio(battleTheme,2,0));
            Debug.Log("activate");
        }
        else{
            StartCoroutine(FadeOutAudio(bossTheme,2,0));
            battleTheme.Play();
            battleTheme.volume = musicVolume;
            //StartCoroutine(FadeOutAudio(battleTheme,2,musicVolume));
            
            Debug.Log("deactivate");
        }
    
    }


    public void subscribe()
    {
        EventManager.OnPlayerDied +=OnPlayerDeath;
    }

    public void unsubscribe()
    {
        EventManager.OnPlayerDied -= OnPlayerDeath;
    }

    public void setLowHealthVolume(float volume){
        lowHealthSound.volume = volume;
    }
    public void playLowHealthSound(){
        if(!lowHealthSound.isPlaying) lowHealthSound.Play();
    }


    public IEnumerator FadeOutAudio(AudioSource audioSource, float fadeDuration , float endVolume){

    float startVolume = audioSource.volume;
    float elapsedTime = 0f;

    while (elapsedTime < fadeDuration)
    {
        elapsedTime += Time.unscaledDeltaTime;
        float percentage = elapsedTime / fadeDuration;
        
       //Debug.Log("the volume is "  + volume + " the fade duration " + fadeDuration);
        audioSource.volume = Mathf.Lerp(startVolume, endVolume,percentage);
        //audioSource.volume = volume;
  
        yield return null;
    }
    
    audioSource.volume = endVolume; 
    
    }
}
