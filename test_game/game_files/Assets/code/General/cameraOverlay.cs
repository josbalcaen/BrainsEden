using UnityEngine;
using System.Collections;

public class cameraOverlay : MonoBehaviour {
	public Transform player;
	public Transform player2;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Debug.DrawLine(player.position, player2.position, Color.blue);
	}

}
