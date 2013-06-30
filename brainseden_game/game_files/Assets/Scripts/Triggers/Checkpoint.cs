using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {
	
	public int Index;
	public GameObject SpawnPos;
	// Use this for initialization
	
	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player" )
		{
			Debug.Log("Checkpoint reached");
			GameObject.FindGameObjectWithTag("Manager").GetComponent<Checkpoints>().CheckpointReached(Index);
		}
	}
}
