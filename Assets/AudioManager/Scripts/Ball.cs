using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour 
{
	void OnCollisionEnter2D (Collision2D coll)
	{
		AudioManager.instance.PlaySound ("Bounce");                            // Play A Single Sound with key "Bounce"
	}
}
