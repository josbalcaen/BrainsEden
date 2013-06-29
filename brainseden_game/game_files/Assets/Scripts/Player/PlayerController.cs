using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	private bool _unitSelected;
	private Vector3 _startMousePos = new Vector3(0,0,0);
	private Vector3 _endMousePos = new Vector3(0,0,0);
	private Rigidbody _selectedUnit;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Select object
		if (Input.GetButtonDown ("Fire1")) 
		{
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			
			RaycastHit hit = new RaycastHit();
						
			if (Physics.Raycast (ray, out hit)) 
			{
				// A unit has been selected
				if (hit.rigidbody != null)
				{
					_selectedUnit = hit.rigidbody;
					_unitSelected = true;
					_startMousePos = Input.mousePosition;
				}
			}
		}
		
		// Launch selected unit
		if(Input.GetButtonUp("Fire1") && _unitSelected)
		{
			_endMousePos = Input.mousePosition;
			_unitSelected = false;
			Vector3 swipeDistance = _endMousePos - _startMousePos;
			
			Debug.Log("Swipe magnitude: " + swipeDistance.magnitude);
			
			_selectedUnit.AddForce(swipeDistance);
		}
	}
}
