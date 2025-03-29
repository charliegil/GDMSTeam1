using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.InputSystem.Controls;
using System.Linq;
using System.Collections;

public class AudioManager : MonoBehaviour, IEventListener
{
    public static AudioManager instance;
    public AudioSource audioSourcePrefab;

    public AudioSource battleTheme;

    public AudioSource BossTheme;

    public AudioSource lowHealthSound;
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
        lowHealthSound.mute  =true;
        PlaySound("PlayerDeath");
        StartCoroutine(FadeOutAudio(battleTheme,3,0.2f));
    }

    public void EnterMenu(bool enter){
        if(enter)battleTheme.volume/=2;
        else{
            battleTheme.volume*=2;
        }
    }
    // public void ActivateBossTheme(bool activate){
    //     if(activate){
    //         Debug.Log("should active BossTheme");
    //         StartCoroutine(FadeOutAudio(battleTheme,4,0.2f));
    //         //battleTheme.volume = 0;
    //         BossTheme.volume = 5f;
    //         battleTheme.volume = 0f;//volumeBattle;
    //     }
    //     else{
    //         StartCoroutine(FadeOutAudio(BossTheme,4,0));
    //         Debug.Log("should desactivate BossTheme");
    //         battleTheme.volume = 5f;
    //         BossTheme.volume = 0f;
    //         //battleTheme.volume = volumeBattle;
    //     }
    // }
    public void ActivateBossTheme(bool activate) {
    if (activate) {
        Debug.Log("should activate BossTheme");

        StartCoroutine(FadeOutAudio(battleTheme, 4, 0)); // Fade out battle theme
        if (!BossTheme.isPlaying) BossTheme.Play();      // Ensure boss theme starts
        StartCoroutine(FadeOutAudio(BossTheme, 4, 0.5f)); // Fade in boss theme
    } 
    else {
        Debug.Log("should deactivate BossTheme");

        StartCoroutine(FadeOutAudio(BossTheme, 4, 0)); // Fade out boss theme
        if (!battleTheme.isPlaying) battleTheme.Play(); // Ensure battle theme starts
        StartCoroutine(FadeOutAudio(battleTheme, 4, 0.5f)); // Fade in battle theme
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
