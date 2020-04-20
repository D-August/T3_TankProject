using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Sound : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip aclip;
    public AudioSource asauce;

    // Awake, Start & Update
    void Awake()
    {
        
    }
    void Start()
    {
        
    }
    void Update()
    {
        
    }

    public void ReproduceAudio(AudioClip aclip, AudioSource asauce, float volume, string mode)
    {
        if (this.asauce.clip != aclip)
        {
            this.asauce = asauce;
            this.aclip = aclip;

            asauce.volume = volume;
            if (mode == "loop") asauce.loop = true;
            asauce.clip = aclip;
            asauce.Play();
        }
        else
        {
            if (!this.asauce.isPlaying)
            {
                asauce.volume = volume;
                if (mode == "loop") asauce.loop = true;
                asauce.Play();
            }
        }
        
    }
    public void ReproduceAudio ( AudioClip aclip, AudioSource asauce, float volume, string mode, int times)
    {
        if (this.asauce.clip != aclip)
        {
            this.asauce = asauce;
            this.aclip = aclip;

            asauce.volume = volume;
            if (mode == "loop") asauce.loop = true;
            asauce.clip = aclip;
            asauce.Play();
        }
        else
        {
            if (!this.asauce.isPlaying)
            {
                asauce.volume = volume;
                if (mode == "loop") asauce.loop = true;
                asauce.Play();
            }
        }
    }

}
