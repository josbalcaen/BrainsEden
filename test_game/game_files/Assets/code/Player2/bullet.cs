using UnityEngine;
using System.Collections;

public class bullet : MonoBehaviour {
	private float timer;
	// Use this for initialization
	void Start () {
		//give start speed
		transform.rigidbody.AddForce(transform.rotation.eulerAngles*10);
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer > 1)
		{
			DestroyObject(gameObject);
		}
	}
}
