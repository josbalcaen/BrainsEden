using UnityEngine;
using System.Collections;

public class GrabTrigger : MonoBehaviour 
{
	public int _numAttachedPlayers = 0;
	public bool _isActive = true;
	
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
		if((other.name == "Player 1" || other.name == "Player 2"))
		{
			++_numAttachedPlayers;
			//_isActive = false;
			if(_numAttachedPlayers == 1 )
			{
				_isActive = false;
				//other.transform.position = Vector3.Lerp(other.transform.position, transform.position, 0.25f);
				other.rigidbody.useGravity = false;
				other.rigidbody.velocity = Vector3.zero;
				other.rigidbody.isKinematic = true;
			}
			
		}
    }
	
	// Enable gravity again when out of grabpoint trigger zone
	void OnTriggerExit(Collider other) 
	{
		if((other.name == "Player 1" || other.name == "Player 2"))
		{
			--_numAttachedPlayers;
			if(_numAttachedPlayers == 0)
			{
				_isActive = true;
				//_isActive = true;
				other.rigidbody.useGravity = true;
				other.rigidbody.isKinematic = false;
			}
		}
    }
}
