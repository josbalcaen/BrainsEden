using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {
	
	public bool Player2;
	private float _force = 400f;
	public Transform OtherPlayer;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if(!Player2)
		{
			if(Input.GetKey(KeyCode.Space))
			{
				rigidbody.AddForce(Vector3.up*_force);
			}
		}
		else
		{
			if(Input.GetKey(KeyCode.UpArrow))
			{
				rigidbody.AddForce(Vector3.up*_force);
			}
		}
		
		//get distance to other player
		Vector3 toOther = OtherPlayer.transform.position - transform.position;
		float dist = toOther.magnitude;
		if(dist > Chain.MaximumDistance)
		{
			//calculate forces
			//based on velocity
			//project on toOther
			Vector3 velocityNormal = Vector3.Project(rigidbody.velocity,toOther);
			//acceleration = v/t
			Vector3 accel = velocityNormal/Time.deltaTime;
			
			//force = m*a
			Vector3 force = rigidbody.mass*accel;
			rigidbody.AddForce(force);
		}
	}
}
