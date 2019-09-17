using UnityEngine;
using System.Collections;

public class SampleMusicPlay : MonoBehaviour 
{
	public void PlayMusic1()
	{
		AudioManager.Instance.PlayMusic ("Music01");                  // Play Music with key "Music01"
	}

	public void PlayMusic2()
	{
		AudioManager.Instance.PlayMusic ("Music02");                 // Play Music with key "Music02"
	}

	public void PlayMusic3()
	{
		AudioManager.Instance.PlayMusic ("Music03");                // Play Music with key "Music03"
	}
}
