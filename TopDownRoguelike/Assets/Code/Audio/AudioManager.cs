using UnityEngine;
using System.Collections.Generic;

using System.Linq;
using System.Collections;

public class AudioManager : MonoBehaviour, IEventListener
{
    public static AudioManager instance;
    public AudioSource audioSourcePrefab;

    public AudioSource battleTheme;

    public AudioSource BossTheme;

    public AudioSource lowHealthSound;

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
        }
        else Destroy(gameObject);
        subscribe();
        
        volumeBoss = BossTheme.volume;
        volumeBattle = battleTheme.volume;
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
        StartCoroutine(FadeOutAudio(BossTheme,3,0));

    }

    public void EnterMenu(bool enter){
        if(enter){
            battleTheme.volume/=2;
            BossTheme.volume/=2;
        }
        else{
            battleTheme.volume*=2;
            BossTheme.volume*=2;
        }
    }
    public void ActivateBossTheme(bool activate){
        if(activate){
            BossTheme.Play();
            StartCoroutine(FadeOutAudio(BossTheme,4,0.2f));
            currentThemeSound = false;
            StartCoroutine(FadeOutAudio(battleTheme,4,0));
            Debug.Log("activate");
        }
        else{
            StartCoroutine(FadeOutAudio(BossTheme,4,0));
            battleTheme.Play();
            StartCoroutine(FadeOutAudio(battleTheme,4,0.2f));
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
