using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	public float SwipeMultiplier = 1.5f;
	public float MinSwipeDistance = 300.0f;
	public float MaxSwipeDistance = 600.0f;
	
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
			
			// Calculate swipe distance and multiply with modifier
			Vector3 swipeDistance = _endMousePos - _startMousePos;
			swipeDistance *= SwipeMultiplier;
			
			// Force the swipe distance to min and max value
			if(swipeDistance.magnitude < MinSwipeDistance)
			{
				swipeDistance.Normalize();
				swipeDistance *= MinSwipeDistance;
			}
			else if(swipeDistance.magnitude > MaxSwipeDistance)
			{
				swipeDistance.Normalize();
				swipeDistance *= MaxSwipeDistance;
			}
			
			Debug.Log("Swipe magnitude: " + swipeDistance.magnitude);
			
			_selectedUnit.AddForce(swipeDistance);
		}
	}
}
