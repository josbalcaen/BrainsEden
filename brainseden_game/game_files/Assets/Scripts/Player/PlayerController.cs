using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	public float SwipeMultiplier = 1.0f;
	public float MinSwipeDistance = 150.0f;
	public float MaxSwipeDistance = 300.0f;
	public float LiftThrowDistance = 600.0f;
	public float HangHeight = 1.0f;
	public GameObject OtherPlayer;
	public bool IsGrabbingAnchor = false;
	
	private bool _unitSelected = false;
	private Vector3 _AnchorPos = new Vector3(0,0,0);
	private Vector3 _startMousePos = new Vector3(0,0,0);
	private Vector3 _endMousePos = new Vector3(0,0,0);
	private Rigidbody _selectedUnit;
	private Transform Anchor;
	private float _distanceToGround;

	// Use this for initialization
	void Start () 
	{
		_distanceToGround = collider.bounds.extents.y;
	}
	
	bool IsGrounded()
	{
		Ray ray = new Ray(transform.position, -Vector3.up);
		RaycastHit hit = new RaycastHit();
  		
		if(Physics.Raycast(ray, out hit, _distanceToGround + 0.1f))
		{
			if(hit.transform.gameObject != null && hit.transform.gameObject.tag == "Level")
			{
				return true;
			}
		}
		
		return false;
	}
	
	// Update is called once per frame
	void Update () 
	{	
		// Check if the player is standing on the ground
		if(IsGrounded() && IsGrabbingAnchor == false)
		{
			// Play animation
			GetComponentInChildren<PlayerAnimation>().PlayAnimation(CharacterAnimationType.Standing, false);
		}
		
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
					// Check if the selected unit is me!
					_unitSelected = _selectedUnit.transform.gameObject == gameObject;
					
					if(_unitSelected)
					{
						// Play animation
						GetComponentInChildren<PlayerAnimation>().PlayAnimation(CharacterAnimationType.TensionJump, false);
						_startMousePos = Input.mousePosition;
					}
				}
			}
		}
		
		// Launch selected player
		if(Input.GetButtonUp("Fire1") && _unitSelected)
		{
			// Play animation
			GetComponentInChildren<PlayerAnimation>().PlayAnimation(CharacterAnimationType.JumpStart, false);
			
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
			
			// Added because script used to execute twice
			swipeDistance*=1.5f;
			
			_selectedUnit.AddForce(swipeDistance);
			
			// Enable gravity when clicked on unit, disable lerp effect
			_selectedUnit.rigidbody.useGravity = true;
			IsGrabbingAnchor = false;
		}
		
		// If the player is grabbing an anchor lerp his position
		if(IsGrabbingAnchor)
		{
			transform.position = Vector3.Lerp(transform.position, _AnchorPos - new Vector3(0,HangHeight,0), 0.1f * Time.deltaTime * 60);
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
	public void ReleaseAnchor(Transform anchor)
	{
		if(anchor != Anchor)return;
		//Gravity && kinematic
		rigidbody.useGravity = true;
		rigidbody.isKinematic = false;
		
	}
	public void GrabAnchor(Transform anchor)
	{
		
		if(IsGrabbingAnchor)return;
		
		rigidbody.useGravity = false;
		rigidbody.velocity = Vector3.zero;
		rigidbody.isKinematic = true;
		// Play animation
		GetComponentInChildren<PlayerAnimation>().PlayAnimation(CharacterAnimationType.JumpEnd, false);
		
		// Play animation
		GetComponentInChildren<PlayerAnimation>().PlayAnimation(CharacterAnimationType.Holdon, false);
		
		IsGrabbingAnchor = true;
		Anchor = anchor;
		_AnchorPos = anchor.position;
	}
}
