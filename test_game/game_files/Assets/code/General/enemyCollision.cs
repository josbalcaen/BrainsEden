using UnityEngine;
using System.Collections;



public class enemyCollision : MonoBehaviour {
	
	private float timer = 0;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer > 0.5f)
		{
			transform.renderer.material.SetColor("_Color", Color.white);
		}
	}
	void OnCollisionEnter()
	{
		//Debug.Log("player collision entered.");
		//todo visual feedback
		transform.renderer.material.SetColor("_Color", Color.red);
		timer = 0;
	}
	
}
