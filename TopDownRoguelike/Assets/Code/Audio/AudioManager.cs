using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.InputSystem.Controls;
using System.Linq;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource audioSourcePrefab;
    private List<AudioSource> audioSources = new List<AudioSource>();

    [SerializeField] private List<Sound> sounds  = new List<Sound>();

    private void Awake()
    {
        if (instance == null){ 
            instance = this;
            DontDestroyOnLoad(gameObject);    
        }
        else Destroy(gameObject);
    }

    private AudioSource GetAvailableSource()
    {
        foreach (AudioSource source in audioSources){
            if (!source.isPlaying) return source; 
        }
        AudioSource Source = Instantiate(audioSourcePrefab, transform);
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
        Sound clip = sounds.FirstOrDefault(s => s.name.Contains(clipName));
        if(clip!=null){
            AudioSource source = GetAvailableSource();
            source.volume = clip.volume;
            source.PlayOneShot(clip.clip);
        }
    }
}
