using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour 
{
	void OnCollisionEnter2D (Collision2D coll)
	{
		AudioManager.PlaySound ("Bounce");                            // Play A Single Sound with key "Bounce"
	}
}
