using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	
	private LevelType _currentLevelType;

	
	// Use this for initialization
	void Start () {
//		Sounds.PlayLevelBackground(LevelType.Menu);
	}
	
	
	public void SetLevelType(LevelType type)
	{
		if(_currentLevelType != type)
		{
			_currentLevelType = type;
			Sounds.PlayLevelBackground(type);
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
