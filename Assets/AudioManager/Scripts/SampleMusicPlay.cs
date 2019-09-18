using UnityEngine;
using System.Collections;

public class SampleMusicPlay : MonoBehaviour 
{
	public void PlayMusic1()
	{
		AudioManager.instance.PlayMusic ("Music01");                  // Play Music with key "Music01"
	}

	public void PlayMusic2()
	{
		AudioManager.instance.PlayMusic ("Music02");                 // Play Music with key "Music02"
	}

	public void PlayMusic3()
	{
		AudioManager.instance.PlayMusic ("Music03");                // Play Music with key "Music03"
	}
}
