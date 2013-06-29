using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	public float SwipeMultiplier = 1.0f;
	public float MinSwipeDistance = 150.0f;
	public float MaxSwipeDistance = 300.0f;
	public float LiftThrowDistance = 600.0f;
	public GameObject OtherPlayer;
	
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
		// Select player
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
		
		// Launch selected player
		if(Input.GetButtonUp("Fire1") && _unitSelected)
		{
			// Set kinematic to false
			_selectedUnit.rigidbody.isKinematic = false;
			
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
			
			// Apply hardcoded fixes
			HardcodedFixes(ref swipeDistance);
			
			// Lift-throw ability
			if(IsPositionValid() && OtherPlayer.rigidbody.velocity == Vector3.zero)
			{
				swipeDistance.Normalize();
				swipeDistance *= LiftThrowDistance;
			}
			
			Debug.Log("Swipe magnitude: " + swipeDistance);
			
			_selectedUnit.AddForce(swipeDistance);
			
			// Enable gravity when clicked on unit
			_selectedUnit.rigidbody.useGravity = true;
		}
	}
	
	private bool IsPositionValid()
	{
		return OtherPlayer.transform.position.y > transform.position.y && Mathf.Abs(OtherPlayer.transform.position.x - transform.position.x) < 2;
	}
	
	// These values are propably not correct, if you notice some weird jumps occur this is where you need to look!
	private void HardcodedFixes(ref Vector3 swipeDistance)
	{
		// HARDCODED: Increase the Y value if it's too low for large distances
		if(swipeDistance.y > 0 && swipeDistance.y < 350 && Mathf.Abs(swipeDistance.x) > 300)
		{
			swipeDistance.y = 350;
		}
		
		// HARDCODED: Increase the Y value if it's too low for large distances
		else if(swipeDistance.y > 0 && swipeDistance.y < 450 && Mathf.Abs(swipeDistance.x) > 400)
		{
			swipeDistance.y = 450;
		}
		
		// HARDCODED: Increase the Y value if it's too low for large distances
		else if(swipeDistance.y < -50 && Mathf.Abs(swipeDistance.x) > 300)
		{
			swipeDistance.y = 150;
		}
		
		// HARDCODED: Increase the Y value if it's too low for high jumps
		else if(swipeDistance.y > 400)
		{
			swipeDistance.y += 75;
		}
	}
}
