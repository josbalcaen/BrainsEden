using UnityEngine;
using System.Collections.Generic;

public class Checkpoints : MonoBehaviour 
{
	public List<GameObject> CheckPointsList = new List<GameObject>();
	
	private int _highestCheckpoint = 0;
	public void Start()
	{
		for(int i = 0; i < CheckPointsList.Count; ++i)
		{
			CheckPointsList[i].GetComponent<Checkpoint>().Index = i;
		}
	}
	
	public void CheckpointReached(int index)
	{
		_highestCheckpoint = Mathf.Max(index,_highestCheckpoint);
	}
	
	public Vector3 GetSpawnPos()
	{
		return CheckPointsList[_highestCheckpoint].GetComponent<Checkpoint>().SpawnPos.transform.position;
	}

}
