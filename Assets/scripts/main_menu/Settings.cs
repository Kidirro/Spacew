using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    public Image BloomButton;
    public Image ChromaticButton;
    public Image MusicButton;
    public Image SoundButton;
    public PostProcessVolume postProcessing;
    public Sprite[] ButtonState;
    public Sprite[] SoundState;

    public AudioMixer Mixer;

    void Start()
    {
        OnEnable();
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        if (postProcessing)
        {
            Bloom bloom;
            ChromaticAberration chromaticAberration;
            postProcessing.profile.TryGetSettings(out bloom);
            postProcessing.profile.TryGetSettings(out chromaticAberration);
            if (bloom != null) bloom.active = PlayerPrefs.GetInt("Bloom") == 1;
            if (chromaticAberration != null) chromaticAberration.active = PlayerPrefs.GetInt("ChromaticAberration") == 1;
            BloomButton.sprite = ButtonState[bloom.active ? 1 : 0];
            ChromaticButton.sprite = ButtonState[chromaticAberration.active ? 1 : 0];
            PlayerPrefs.Save();

        }

        if (Mixer)
        {
            Mixer.SetFloat("Volume", PlayerPrefs.GetInt("MusicOn") ==1? 0: -80);
            GameManager.Using_sound = PlayerPrefs.GetInt("SoundOn") == 1;
            MusicButton.sprite = SoundState[PlayerPrefs.GetInt("MusicOn") == 1 ? 1 : 0];
            SoundButton.sprite = SoundState[GameManager.Using_sound ? 1 : 0];
            PlayerPrefs.Save();
        }

    }


    public void toggleBloom()
    {
        if (postProcessing != null)
        {
            Bloom bloom;
            postProcessing.profile.TryGetSettings(out bloom);
            if (bloom != null)
            {
                bloom.active = !bloom.active;
                PlayerPrefs.SetInt("Bloom", bloom.active ? 1 : 0);
                BloomButton.sprite = ButtonState[bloom.active ? 1 : 0];
                PlayerPrefs.Save();
            }
        }
    }

    public void toggleChromatic()
    {
        if (postProcessing != null)
        {
            ChromaticAberration chromatic;
            postProcessing.profile.TryGetSettings(out chromatic);
            if (chromatic != null)
            {
                chromatic.active = !chromatic.active;
                PlayerPrefs.SetInt("ChromaticAberration", chromatic.active ? 1 : 0);
                ChromaticButton.sprite = ButtonState[chromatic.active ? 1 : 0];
                PlayerPrefs.Save();

            }
        }
    }

    public void toggleMusic()
    {
        if (Mixer)
        {
            PlayerPrefs.SetInt("MusicOn", PlayerPrefs.GetInt("MusicOn") == 1 ? 0 : 1);
            Mixer.SetFloat("Volume", PlayerPrefs.GetInt("MusicOn") == 1 ? 0 : -80);
            MusicButton.sprite = SoundState[PlayerPrefs.GetInt("MusicOn") == 1 ? 1 : 0];
            PlayerPrefs.Save();
        }
    }

    public void toggleSound()
    {
        if (Mixer)
        {
            PlayerPrefs.SetInt("SoundOn", PlayerPrefs.GetInt("SoundOn") == 1 ? 0 : 1);
            SoundButton.sprite = SoundState[PlayerPrefs.GetInt("SoundOn") == 1 ? 1 : 0];
            GameManager.Using_sound = PlayerPrefs.GetInt("SoundOn") == 1;
            PlayerPrefs.Save();
        }
    }

    public void Exit()
    {
        gameObject.SetActive(false);
    }
}
