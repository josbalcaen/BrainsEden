using UnityEngine;
using System.Collections.Generic;

public class Tutorial : MonoBehaviour {
	
	public GameObject Player;
	public bool Enabled = false;
	public List<Transform> Anchors = new List<Transform>();
	int currentAnchorIndex = 0;
	
	public float WaitInterval = 1f;
	
	private float _waitTimer;
	// Use this for initialization
	void Start () {
	
	}
	
	public void Play()
	{
		Enabled = true;
	}
	
	void AnchorGrabbed(object sender, EventArgs e)
	{
		//start timer
		_waitTimer = WaitInterval;
		
	}
	// Update is called once per frame
	void Update () 
	{
		if(!Enabled)return;
		
		if(Player.GetComponent<PlayerController>().IsGrabbingAnchor)
		{
			if(_waitTimer > 0)
			{
				_waitTimer -= Time.deltaTime;
				if(_waitTimer <=0)
				{
					//Jump to next part
					
					
					++currentAnchorIndex;
					if(currentAnchorIndex >= Anchors.Count) Enabled = false;
					
				}
			}
		}
		
		
	}
}
