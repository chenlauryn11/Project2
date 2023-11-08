using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundControl : MonoBehaviour
{
    //Initializes the sliders to control volume
    [SerializeField] Slider musicSlider, sfxSlider;

    //Initializes the audio sources
    AudioSource musicAudio, sfxAudio;

    void Awake()
    {
        //Gets the music slider
        Slider b = musicSlider.GetComponent<Slider>();

        //Gets the sound effects slider
        Slider s = sfxSlider.GetComponent<Slider>();

        //Get the audio source on GameMangager 
        musicAudio = GameObject.Find("GameManager").GetComponent<AudioSource>();

        //Get the audio source on the player
        sfxAudio = GameObject.Find("Slime").GetComponent<AudioSource>();
    }

    //Start is called before the first frame update
    void Start()
    {
        musicAudio.volume = 1;
        sfxAudio.volume = 1;

        musicSlider.value = musicAudio.volume;
        sfxSlider.value = sfxAudio.volume;
    }

    //Change volume of music using value of musicSlider
    public void changeMusicVol()
    {
        musicAudio.volume = musicSlider.value;
    }

    //Change volume of sound effects using value of sfxSlider
    public void changeSFXVol()
    {
        sfxAudio.volume = sfxSlider.value;
    }

    //Mute both music and sound effects
    public void mute()
    {
        musicAudio.volume = 0;
        sfxAudio.volume = 0;
    }
}