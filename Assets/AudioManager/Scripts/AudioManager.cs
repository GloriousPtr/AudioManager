using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour 
{
	public static AudioManager Instance;

	public Sprite SfxOn;
	public Sprite SfxOff;
	public Transform SfxButton;
	public Transform SfxVolSlider;

	public Sprite MusicOn;
	public Sprite MusicOff;
	public Transform MusicButton;
	public Transform MusicVolSlider;

	public float delayInCrossfading = 0.3f;

	public bool IsSoundOn;
	public bool SfxMute;
	public bool IsMusicOn;
	public bool MusicMute;

    private AudioSource music;
    private AudioSource sfx;

    public List<MusicTrack> tracks = new List<MusicTrack>();
    public List<Sound> sounds = new List<Sound>();
    private Sound GetSoundByName(string sName){    return sounds.Find(x => x.name == sName);    }


    private static readonly List<string> mixBuffer = new List<string>();
    private const float mixBufferClearDelay = 0.05f;

    public bool mute;
    public bool quiet_mode;
    
    internal string currentTrack;

	public float musicVolume 
	{
		get
		{
			if (!PlayerPrefs.HasKey("Music Volume"))
				return 1f;
			else
			    return PlayerPrefs.GetFloat("Music Volume");
		}
	}

	public float sfxVolume 
	{
		get
		{
			if (!PlayerPrefs.HasKey("SFX Volume"))
				return 1f;
			else
				return PlayerPrefs.GetFloat("SFX Volume");
		}
	}

    void Awake() 
	{
		Instance = this;

		// Configuring Audio Source For Playing Music And SFX
		music = gameObject.AddComponent<AudioSource> ();
		music.loop = true;
		sfx = gameObject.AddComponent<AudioSource> ();

		SfxMute = false;
		IsSoundOn = true;

		MusicMute = false;
		IsMusicOn = true;


		// Check If Sfx Volume Is Not 0
		if (Math.Abs(sfxVolume) > 0.05f) 
		{
			// Set The Saved Value Of SFX Volume
			SfxVolSlider.GetComponent<Slider> ().value = sfxVolume;
			sfx.volume = sfxVolume;
		}
		// Set The Values To 0
		else
		{
			SfxVolSlider.GetComponent<Slider> ().value = 0;
			sfx.volume = 0;
		}

		// Check If Music Volume Is Not 0
		if (Math.Abs(musicVolume) > 0.05f) 
		{
			// Set The Saved Value Of Music Volume
			MusicVolSlider.GetComponent<Slider> ().value = musicVolume;
			music.volume = musicVolume;
		}
		// Set The Values To 0
		else
		{
			MusicVolSlider.GetComponent<Slider> ().value = 0;
			music.volume = 0;
		}


		// Checks If The SfxMute Is True Or Not
		if (PlayerPrefs.GetInt ("SfxMute") == 1)
		{
			SfxToggle ();
		}
		// Checks If The MusicMute Is True Or Not
		if (PlayerPrefs.GetInt ("MusicMute") == 1)
		{
			MusicToggle ();
		}

		StartCoroutine(MixBufferRoutine());
    }

	// Responsible for limiting the frequency of playing sounds
	private IEnumerator MixBufferRoutine() {
		float time = 0;

		while (true) {
			time += Time.unscaledDeltaTime;
			yield return 0;
			if (time >= mixBufferClearDelay) {
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
		StartCoroutine(Instance.CrossFade(to));
	}

	// Cross fading - Smooth Transition When Track Is Switched
    private IEnumerator CrossFade(AudioClip to) 
	{
		if (music.clip != null) 
		{
			while (delayInCrossfading > 0) 
			{
				music.volume = delayInCrossfading * musicVolume;
				delayInCrossfading -= Time.unscaledDeltaTime;
				yield return 0;
			}
		}
		music.clip = to;
		if (to == null || mute)
		{
			music.Stop();
			yield break;
		}
		delayInCrossfading = 0;

        if (!music.isPlaying)
            music.Play();
		
        while (delayInCrossfading < 1f) 
		{
			music.volume = delayInCrossfading * musicVolume;
			delayInCrossfading += Time.unscaledDeltaTime;
			yield return 0;
		}
		music.volume = musicVolume;
	}

	public void StopSound()
	{
		sfx.Stop ();
	}

	// Sfx Button On/Off
	public void SfxToggle()
    {
		if (SfxMute) 
		{
			SfxMute = false;
			IsSoundOn = true;
			SfxButton.GetComponentInChildren<Image> ().sprite = SfxOn;
			PlayerPrefs.SetInt ("SfxMute", 0);
			PlayerPrefs.Save ();
		}
		else if (!SfxMute) 
		{
			SfxMute = true;
			IsSoundOn = false;
			SfxButton.GetComponentInChildren<Image> ().sprite = SfxOff;
			PlayerPrefs.SetInt ("SfxMute", 1);
			PlayerPrefs.Save ();
		}
	}

	// Music Button On/Off
	public void MusicToggle()
	{
		if (MusicMute) 
		{
			MusicMute = false;
			GetComponent<AudioSource>().mute = false;
			IsMusicOn = true;
			MusicButton.GetComponentInChildren<Image> ().sprite = MusicOn;
			PlayerPrefs.SetInt ("MusicMute", 0);
			PlayerPrefs.Save ();
		}
		else if (!MusicMute)
		{
			MusicMute = true;
			GetComponent<AudioSource>().mute = true;
			IsMusicOn = false;
			MusicButton.GetComponentInChildren<Image> ().sprite = MusicOff;
			PlayerPrefs.SetInt ("MusicMute", 1);
			PlayerPrefs.Save ();
		}
	}

	// A single sound effect
	public static void PlaySound(string clip) 
	{
		if (Instance.IsSoundOn) {
			Sound sound = Instance.GetSoundByName (clip);

			if (sound != null && !mixBuffer.Contains (clip)) {
				if (sound.clips.Count == 0)
					return;
				mixBuffer.Add (clip);
				Instance.sfx.PlayOneShot (sound.clips.GetRandom ());             // Randomly Play Sound Each Time Through The Array Of clip
			}
		}
	}

	// Changing Sfx Vol Using Slider
	public void SfxSlider()
	{
		float vol = SfxVolSlider.GetComponent<Slider>().value;
		sfx.volume = vol;
		// Sets And Save The Value When User Use Slider
		PlayerPrefs.SetFloat ("SFX Volume", vol);
		PlayerPrefs.Save ();
	}

	// Changing Music Vol Using Slider
	public void MusicSlider()
	{
		float vol = MusicVolSlider.GetComponent<Slider> ().value;
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