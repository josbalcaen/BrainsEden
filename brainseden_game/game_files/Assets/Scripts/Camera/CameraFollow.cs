using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	
	public Transform Player1;
	public Transform Player2;
	public float CamDist;
	public float CamHeightOffset;
	public float Acceleration;
	
	private Vector3 _DesiredTargetPos;
	private Vector3 _CurrentTargetPos;
	
	private bool _PlayerDied = false;
	public float Damping = 0.4f;
	
	private Vector3 _velocity;
	// Use this for initialization
	void Start ()
	{
		CalcDesiredPos();
		_CurrentTargetPos = _DesiredTargetPos;
	}
	void CalcLookat()
	{
		
		Vector3 camPos = _CurrentTargetPos - Vector3.forward*CamDist + Vector3.up*CamHeightOffset;
		transform.position = camPos;
		transform.LookAt(_CurrentTargetPos);
	}
	void CalcDesiredPos()
	{
		_DesiredTargetPos = (Player1.transform.position + Player2.transform.position)*.5f;
		
	}
	public void PlayerRespawned()
	{
		_PlayerDied = false;
	}
	public void PlayerDied()
	{
		_PlayerDied = true;
	}
	// Update is called once per frame
	void Update () 
	{
		if(!_PlayerDied)
		{
			CalcDesiredPos();
		}
		Vector3 toTargetPos = _DesiredTargetPos - _CurrentTargetPos;
		float dist = toTargetPos.magnitude;
		
		_velocity += toTargetPos*dist*Acceleration*Time.deltaTime;
		_velocity *= Damping;
		_CurrentTargetPos += _velocity*Time.deltaTime;
//		_CurrentTargetPos = Vector3.Lerp(_CurrentTargetPos,_DesiredTargetPos,lerpVal);
		CalcLookat();
	}
}
