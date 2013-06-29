using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	
	public Transform Player1;
	public Transform Player2;
	public float CamDist;
	
	private Vector3 _DesiredTargetPos;
	private Vector3 _CurrentTargetPos;
	
	// Use this for initialization
	void Start ()
	{
		CalcDesiredPos();
		_CurrentTargetPos = _DesiredTargetPos;
	}
	void CalcLookat()
	{
		Vector3 camPos = _CurrentTargetPos - Vector3.forward*CamDist;
		transform.position = camPos;
		transform.LookAt(_CurrentTargetPos);
	}
	void CalcDesiredPos()
	{
		_DesiredTargetPos = (Player1.transform.position + Player2.transform.position)*.5f;
		
	}
	// Update is called once per frame
	void Update () 
	{
		CalcDesiredPos();
		
		float lerpVal = Mathf.Clamp01(0.1f*Time.deltaTime*60f);
		_CurrentTargetPos = Vector3.Lerp(_CurrentTargetPos,_DesiredTargetPos,lerpVal);
		CalcLookat();
	}
}
