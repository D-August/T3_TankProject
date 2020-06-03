using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_AudioCon : MonoBehaviour
{
    // Singleton
    public static Scr_AudioCon ac = null;

    [Header("AudioSources")]
    public List<AudioSource> asl = new List<AudioSource>();

    [Header("Muted or not")]
    public bool muted = false;

    // Awake, Start & Update
    private void Awake()
    {
        if (ac == null) ac = this;

    }
    void Start()
    {

    }
    void Update()
    {
        AudioSourcesController();

    }

    // Check if the AudioSource is being used, if not, remove
    public void AudioSourcesController()
    {
        try
        {
            foreach (AudioSource a in asl)
            {
                if (!a.isPlaying)
                {
                    try
                    {
                        asl.Remove(a);
                        Destroy(a);
                    }
                    catch { }
                }
            }
        }
        catch { }
    }

    // Play Sounds (Manual)
    public void PlaySound(AudioClip a_clip)
    {
        PlaySound(a_clip, 1f);
    }
    public void PlaySound(AudioClip a_clip, float volume)
    {
        PlaySound(a_clip, volume, false);
    }
    public void PlaySound(AudioClip a_clip, float volume, bool loop)
    {
        PlaySound(a_clip, volume, loop, gameObject);
    }
    public void PlaySound(AudioClip a_clip, float volume, bool loop, GameObject parent)
    {
        if (!muted)
        {
            AudioSource temp_as = parent.AddComponent<AudioSource>();
            temp_as.playOnAwake = false;
            temp_as.clip = a_clip;
            temp_as.volume = volume;
            temp_as.loop = loop;

            asl.Add(temp_as);

            temp_as.Play();
        }
    }

    // Check if Audio is Playing
    public bool AudioIsPlaying( string c_name )
    {
        try { return asl[ IndexInASL(c_name) ].isPlaying; } catch { }
        return false;
    }

    // Get Audio in ASL (Audio Source List)
    public int IndexInASL( string c_name )
    {
        int index = 0;

        for(index = 0; index < asl.Count; index++)
        {
            if (asl[index].clip.name == c_name) return index;
        }

        return index;
    }

    // Ger Position
    public int GetClipPosition(List<AudioClip> ac_list, string filename)
    {
        int i = 0;
        foreach (AudioClip ac in ac_list)
        {
            if (ac.name == filename)
            {
                return i;
            }
            else i++;
        }
        return 99;
    }
}