using UnityEngine;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour 
{
	public float SwipeMultiplier = 1.0f;
	public float MinSwipeDistance = 150.0f;
	public float MaxSwipeDistance = 300.0f;
	public float LiftThrowDistance = 600.0f;
	public float HangHeight = 1.0f;
	public GameObject OtherPlayer;
	public bool IsGrabbingAnchor = false;
	public float NormalSwipeForce = 2.0f;
	public float ReducedSwipeForce = 1.0f;
	
	private bool _unitSelected = false;
	private Vector3 _AnchorPos = new Vector3(0,0,0);
	private Vector3 _startMousePos = new Vector3(0,0,0);
	private Vector3 _endMousePos = new Vector3(0,0,0);
	private Rigidbody _selectedUnit;
	private Transform _Anchor;
	private float _distanceToGround;
	
	public event EventHandler AnchorGrabbed;

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
		
		// Make the player dangle when his velocity is low and the other player is attached to an anchor point
		if(OtherPlayer.GetComponent<PlayerController>().IsGrabbingAnchor && !IsGrabbingAnchor && !IsGrounded() && rigidbody.velocity.magnitude < 2.0f 
			&& OtherPlayer.GetComponent<PlayerController>().transform.position.y > transform.position.y
			&& Mathf.Abs(OtherPlayer.transform.position.x - transform.position.x) < 1)
		{
			// Play animation
			GetComponentInChildren<PlayerAnimation>().PlayAnimation(CharacterAnimationType.Dangling, true);
		}
		
		// Make the player dangle when his velocity is high and the other player is attached to an anchor point
		if(OtherPlayer.GetComponent<PlayerController>().IsGrabbingAnchor && !IsGrabbingAnchor && !IsGrounded() && rigidbody.velocity.magnitude > 1.0f 
			&& OtherPlayer.GetComponent<PlayerController>().transform.position.y > transform.position.y)
		{
			// Play animation
			GetComponentInChildren<PlayerAnimation>().PlayAnimation(CharacterAnimationType.Swinging, true);
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
			
			Debug.Log("Swipe magnitude: " + swipeDistance);
			
			// Added because script used to execute twice
			if(IsGrabbingAnchor || IsGrounded() || rigidbody.velocity.magnitude < 5.0f)
			{
				swipeDistance*=NormalSwipeForce;
			}
			else
			{
				swipeDistance*=ReducedSwipeForce;
			}
			
			_selectedUnit.AddForce(swipeDistance);
			
			//Gravity && kinematic
			_Anchor = null;
			rigidbody.useGravity = true;
			rigidbody.isKinematic = false;
			
			// Enable gravity when clicked on unit, disable lerp effect
			_selectedUnit.rigidbody.useGravity = true;
			IsGrabbingAnchor = false;
		}
		
		// If the player is grabbing an anchor lerp his position
		if(IsGrabbingAnchor)
		{
			transform.position = Vector3.Lerp(transform.position, _AnchorPos - new Vector3(0,HangHeight,0), 0.1f * Time.deltaTime * 60);
		}
		
		if(rigidbody.velocity.magnitude > 0.1f)
		Debug.Log("Velocity: " + rigidbody.velocity.magnitude);
	}
	
	public void LaunchCharacter(Vector3 startPostion, Vector3 endPostion)
	{
		// Play animation
		GetComponentInChildren<PlayerAnimation>().PlayAnimation(CharacterAnimationType.JumpStart, false);
		
		// Set kinematic to false
		rigidbody.isKinematic = false;
		
		_endMousePos = Input.mousePosition;
		_unitSelected = false;
		
		// Calculate swipe distance and multiply with modifier
		Vector3 swipeDistance = endPostion - startPostion;
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
		
		// Added because script used to execute twice
		if(IsGrabbingAnchor || IsGrounded() || rigidbody.velocity.magnitude < 5.0f)
		{
			swipeDistance*=NormalSwipeForce;
		}
		else
		{
			swipeDistance*=ReducedSwipeForce;
		}
		
		rigidbody.AddForce(swipeDistance);
		
		//Gravity && kinematic
		_Anchor = null;
		rigidbody.useGravity = true;
		rigidbody.isKinematic = false;
		
		// Enable gravity when clicked on unit, disable lerp effect
		IsGrabbingAnchor = false;
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
	
	// Called when the character let's go of the anchor
	public void ReleaseAnchor(Transform anchor)
	{	
	}
	
	public void GrabAnchor(Transform anchor)
	{
		if(IsGrabbingAnchor)return;
		
		if(AnchorGrabbed != null)
		{
			AnchorGrabbed(this,new EventArgs());
		}
		
		rigidbody.useGravity = false;
		rigidbody.velocity = Vector3.zero;
		rigidbody.isKinematic = true;
		// Play animation
		GetComponentInChildren<PlayerAnimation>().PlayAnimation(CharacterAnimationType.JumpEnd, false);
		
		// Play animation
		GetComponentInChildren<PlayerAnimation>().PlayAnimation(CharacterAnimationType.Holdon, false);
		
		IsGrabbingAnchor = true;
		_Anchor = anchor;
		_AnchorPos = anchor.position;
	}
}
