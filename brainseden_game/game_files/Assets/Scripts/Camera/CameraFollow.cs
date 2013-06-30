using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	
	public Transform Player1;
	public Transform Player2;
	public float CamDist;
	public float CamDistFar;
	public float CamHeightOffset;
	
	private Vector3 _DesiredTargetPos;
	private Vector3 _CurrentTargetPos;
	
	private bool _PlayerDied = false;
	public float Damping = 0.4f;
	
	private float _CurrentCamDist = 0;
	private float _DesiredCamDist = 0;

	// Use this for initialization
	void Start ()
	{
		_CurrentCamDist = _DesiredCamDist = CamDist;
		CalcDesiredPos();
		_CurrentTargetPos = _DesiredTargetPos;
	}
	void CalcLookat()
	{
		
		Vector3 camPos = _CurrentTargetPos - Vector3.forward*_CurrentCamDist + Vector3.up*CamHeightOffset;
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
	
		float lerpVal = Time.deltaTime*0.1f*60f;
		
		
		
		
		if(Input.GetKeyDown(KeyCode.Space))
		{
			_DesiredCamDist = CamDistFar;
		}
		if(Input.GetKeyUp(KeyCode.Space))
		{
			_DesiredCamDist = CamDist;
		}
		_CurrentTargetPos = Vector3.Lerp(_CurrentTargetPos,_DesiredTargetPos,lerpVal);
		_CurrentCamDist = Mathf.Lerp(_CurrentCamDist,_DesiredCamDist,lerpVal);
		
		CalcLookat();
		
		
		
	}
}
