using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour 
{
	public static AudioManager instance;

	public Sprite sfxOn;
	public Sprite sfxOff;
	public Image sfxButton;
	public Slider sfxVolSlider;

	public Sprite musicOn;
	public Sprite musicOff;
	public Image musicButton;
	public Slider musicVolSlider;

	public float delayInCrossfading = 0.3f;

    public List<MusicTrack> tracks = new List<MusicTrack>();
    public List<Sound> sounds = new List<Sound>();

	private bool sfxMute;
	private bool musicMute;
    private AudioSource music;
    private AudioSource sfx;

    private Sound GetSoundByName(string sName) => sounds.Find(x => x.name == sName);

    private static readonly List<string> mixBuffer = new List<string>();
    private const float mixBufferClearDelay = 0.05f;

    internal string currentTrack;

	public float MusicVolume => !PlayerPrefs.HasKey("Music Volume") ? 1f : PlayerPrefs.GetFloat("Music Volume");

    public float SfxVolume => !PlayerPrefs.HasKey("SFX Volume") ? 1f : PlayerPrefs.GetFloat("SFX Volume");

    private void Awake() 
	{
		instance = this;

		// Configuring Audio Source For Playing Music And SFX
		music = gameObject.AddComponent<AudioSource> ();
		music.loop = true;
		sfx = gameObject.AddComponent<AudioSource> ();

		sfxMute = false;

		musicMute = false;

		// Check If Sfx Volume Is Not 0
		if (Math.Abs(SfxVolume) > 0.05f) 
		{
			// Set The Saved Value Of SFX Volume
			sfxVolSlider.value = SfxVolume;
			sfx.volume = SfxVolume;
		}
		// Set The Values To 0
		else
		{
			sfxVolSlider.value = 0;
			sfx.volume = 0;
		}

		// Check If Music Volume Is Not 0
		if (Math.Abs(MusicVolume) > 0.05f) 
		{
			// Set The Saved Value Of Music Volume
			musicVolSlider.value = MusicVolume;
			music.volume = MusicVolume;
		}
		// Set The Values To 0
		else
		{
			musicVolSlider.value = 0;
			music.volume = 0;
		}


		// Checks If The sfxMute Is True Or Not
		if (PlayerPrefs.GetInt ("sfxMute") == 1)
		{
			SfxToggle ();
		}
		// Checks If The musicMute Is True Or Not
		if (PlayerPrefs.GetInt ("musicMute") == 1)
		{
			MusicToggle ();
		}

		StartCoroutine(MixBufferRoutine());
    }

	// Responsible for limiting the frequency of playing sounds
    private IEnumerator MixBufferRoutine()
    {
        float time = 0;

        while (true)
        {
            time += Time.unscaledDeltaTime;
            yield return 0;
            if (time >= mixBufferClearDelay)
            {
                mixBuffer.Clear();
                time = 0;
            }
        }
    }

    // Play a music track with Cross fading
    public void PlayMusic(string trackName) 
	{
        if (trackName != "")
            currentTrack = trackName;
		AudioClip to = null;
		foreach (MusicTrack track in tracks)
				if (track.name == trackName)
					to = track.track;

		StartCoroutine(CrossFade(to));
	}

	// Cross fading - Smooth Transition When Track Is Switched
    private IEnumerator CrossFade(AudioClip to) 
	{
		if (music.clip != null) 
		{
			while (delayInCrossfading > 0) 
			{
				music.volume = delayInCrossfading * MusicVolume;
				delayInCrossfading -= Time.unscaledDeltaTime;
				yield return 0;
			}
		}
		music.clip = to;
		if (to == null)
		{
			music.Stop();
			yield break;
		}
		delayInCrossfading = 0;

        if (!music.isPlaying)
            music.Play();
		
        while (delayInCrossfading < 1f) 
		{
			music.volume = delayInCrossfading * MusicVolume;
			delayInCrossfading += Time.unscaledDeltaTime;
			yield return 0;
		}
		music.volume = MusicVolume;
	}

	public void StopSound()
	{
		sfx.Stop ();
	}

	// Sfx Button On/Off
	public void SfxToggle()
    {
        sfxMute = !sfxMute;
        sfx.mute = sfxMute;

		sfxButton.sprite = !sfxMute ? sfxOn : sfxOff;

		PlayerPrefs.SetInt ("sfxMute", Utils.BoolToBinary(sfxMute));
        PlayerPrefs.Save();
    }

	// Music Button On/Off
	public void MusicToggle()
    {
        musicMute = !musicMute;
        music.mute = musicMute;

		musicButton.sprite = !musicMute ? musicOn : musicOff;

        PlayerPrefs.SetInt("musicMute", Utils.BoolToBinary(musicMute));
        PlayerPrefs.Save();
    }

	// A single sound effect
    public void PlaySound(string clip)
    {
        Sound sound = GetSoundByName(clip);

        if (sound != null && !mixBuffer.Contains(clip))
        {
            if (sound.clips.Count == 0)
                return;
            mixBuffer.Add(clip);
            sfx.PlayOneShot(sound.clips
                .GetRandom()); // Randomly Play Sound Each Time Through The Array Of clip
        }
    }

    // Changing Sfx Vol Using Slider
	public void SfxSlider()
	{
		float vol = sfxVolSlider.value;
		sfx.volume = vol;
		// Sets And Save The Value When User Use Slider
		PlayerPrefs.SetFloat ("SFX Volume", vol);
		PlayerPrefs.Save ();
	}

	// Changing Music Vol Using Slider
	public void MusicSlider()
	{
		float vol = musicVolSlider.value;
		music.volume = vol;
		// Sets And Save The Value When User Use Slider
		PlayerPrefs.SetFloat ("Music Volume", vol);
		PlayerPrefs.Save ();
	}

    [Serializable]
    public class MusicTrack 
	{
        public string name;
        public AudioClip track;
    }

    [Serializable]
    public class Sound 
	{
        public string name;
        public List<AudioClip> clips = new List<AudioClip>();
    }
}