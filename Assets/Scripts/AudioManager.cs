using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;
    
    void Awake()
    {
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);

        foreach(Sound s in sounds){
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.time = s.time;
        }
    }

    
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null){return;}

        /*
        if(!s.isPlaying){
            s.source.Play();
            s.isPlaying = true;
        }
        */
        s.source.Play();
    }

    public IEnumerator PlayCoroutine(string name){
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null){yield return null;}
        s.isPlaying = true;
        yield return new WaitUntil(() => s.source.time == s.time);
        if(s.isPlaying){
            s.source.Play();
        }else{
            s.source.Stop();
        }
    }

    public void Stop(string name){
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null){return;}
        s.source.Stop();
        s.isPlaying = false;
    }
}
