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

    private AudioSource battleTheme;
    private AudioSource bossTheme;
    private AudioSource lowHealthSound;


    public Coroutine battleCoroutine = null;
    public Coroutine BossThemeCoroutine=  null;
    private List<AudioSource> audioSources = new List<AudioSource>();
    public bool currentThemeSound = true;

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

        }
        else Destroy(gameObject);
        subscribe();
        
        battleTheme.loop = true;
        bossTheme.loop = true;
        lowHealthSound.loop = true;
        battleTheme.Play();
        StartCoroutine(FadeOutAudio(battleTheme,10,musicVolume));


        
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

    public void PlaySound(Sound clip)
    {
        AudioSource source = GetAvailableSource();
        source.volume = clip.volume;
        source.PlayOneShot(clip.clip);
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
            StartCoroutine(FadeOutAudio(bossTheme,4,musicVolume));
            currentThemeSound = false;
            StartCoroutine(FadeOutAudio(battleTheme,4,0));
            Debug.Log("activate");
        }
        else{
            StartCoroutine(FadeOutAudio(bossTheme,4,0));
            battleTheme.Play();
            StartCoroutine(FadeOutAudio(battleTheme,4,musicVolume));
            Debug.Log("deactivate");
        }
        currentThemeSound = !activate;
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
        audioSource.volume = Mathf.Lerp(startVolume, endVolume, elapsedTime / fadeDuration);
        yield return null;
    }

    //audioSource.Stop();
    audioSource.volume = endVolume; 
        //audioSource.mute = true;
    }
}
