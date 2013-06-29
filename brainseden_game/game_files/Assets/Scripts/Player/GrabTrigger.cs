using UnityEngine;
using System.Collections;

public class GrabTrigger : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	// Disable gravity and velocity when entered a grabpoint trigger
	void OnTriggerEnter(Collider other) 
	{
		if(other.name == "Player 1" || other.name == "Player 2")
		{
			//other.transform.position = Vector3.Lerp(other.transform.position, transform.position, 0.25f);
			other.rigidbody.useGravity = false;
			other.rigidbody.velocity = Vector3.zero;
		}
    }
	
	// Enable gravity again when out of grabpoint trigger zone
	void OnTriggerExit(Collider other) 
	{
		if(other.name == "Player 1" || other.name == "Player 2")
		{
			other.rigidbody.useGravity = true;
		}
    }
}
