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
	
	private bool _Dangling = false;
	private float _DanglingTimer = 0f;
	private Vector3 _prevVelocity;
	private bool _Swinging = false;
	private PlayerAnimation _AnimPlayer;
	
	private bool _tension = false;

	// Use this for initialization
	void Start () 
	{
		_distanceToGround = collider.bounds.extents.y;
		_AnimPlayer = GetComponentInChildren<PlayerAnimation>();
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
		
		bool grounded = IsGrounded();
		// Check if the player is standing on the ground
		if(grounded && !IsGrabbingAnchor && !_tension)
		{
			// Play animation
			_AnimPlayer.PlayAnimation(CharacterAnimationType.Standing, true,0.5f);
			_Swinging = false;
		}
		
		// Make the player dangle when his velocity is low and the other player is attached to an anchor point
//		else if(OtherPlayer.GetComponent<PlayerController>().IsGrabbingAnchor && !IsGrabbingAnchor && !IsGrounded() && rigidbody.velocity.magnitude < 3.0f 
//			&& OtherPlayer.GetComponent<PlayerController>().transform.position.y > transform.position.y
//			&& Mathf.Abs(OtherPlayer.transform.position.x - transform.position.x) < 1)
//		{
//			// Play animation
//			_AnimPlayer.PlayAnimation(CharacterAnimationType.Dangling, true, 0.5f);
//		}
//		
//		// Make the player dangle when his velocity is high and the other player is attached to an anchor point
//		if(OtherPlayer.GetComponent<PlayerController>().IsGrabbingAnchor && !IsGrabbingAnchor && !IsGrounded() && rigidbody.velocity.magnitude > 3.0f 
//			&& OtherPlayer.GetComponent<PlayerController>().transform.position.y > transform.position.y)
//		{
//			
//			// Play animation
//			_AnimPlayer.PlayAnimation(CharacterAnimationType.Swinging, true, 0.5f);
//		}
		
		// Make the character Jump
		if(!IsGrabbingAnchor && !grounded && !_unitSelected)
		{
			if(rigidbody.velocity.y > -2f && transform.position.y > OtherPlayer.transform.position.y)
			{
				// Play animation
				_AnimPlayer.PlayAnimation(CharacterAnimationType.JumpLoop, true, 0.5f);
				_Swinging = false;
			}
			else 
			{
//				//Calculate change in velocity
//				Vector3 currentvel = rigidbody.velocity;
//				
//				if(_prevVelocity.y < 0 && Mathf.Abs(_prevVelocity.y / _prevVelocity.x)> 1)
//				{
////					if(_prevVelocity.magnitude - currentvel.magnitude > 5f)
////					{
////						_Dangling = true;
////						_DanglingTimer = 2f;
////						
//////						_AnimPlayer.PlayAnimation(CharacterAnimationType.Dangling,true,0.5f);
////					}
//				}
//				
//				_prevVelocity = currentvel;
//				
//				if(_Dangling)
//				{
//					_DanglingTimer -= Time.deltaTime;
//					if(_DanglingTimer <=0)
//					{
//						_Dangling = false;
//						_AnimPlayer.PlayAnimation(CharacterAnimationType.Swinging,true,0.5f);
//					}
//				}
//				else
//				{
				if(!_Swinging)
					_AnimPlayer.PlayAnimation(CharacterAnimationType.JumpEnd,false);
				
				_Swinging = true;
				if(rigidbody.velocity.x < 0)
				{
					
					_AnimPlayer.PlayAnimation(CharacterAnimationType.Swinging_Right,true,0.4f);
				}
				else
				{
					_AnimPlayer.PlayAnimation(CharacterAnimationType.Swinging_Left,true,0.4f);
				}
//				}
			}
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
						_Swinging = false;
						_tension = true;
						if(grounded)
						{
							_AnimPlayer.PlayAnimation(CharacterAnimationType.GroundTension,false);
						}
						else
						{
							// Play animation
							_AnimPlayer.PlayAnimation(CharacterAnimationType.TensionJump, false);
						}
						_startMousePos = Input.mousePosition;
					}
				}
			}
		}
		
		// Launch selected player
		if(Input.GetButtonUp("Fire1") && _unitSelected)
		{
			
			// Play animation
			_AnimPlayer.PlayAnimation(CharacterAnimationType.JumpStart, false);
			
			// Set kinematic to false
			_selectedUnit.rigidbody.isKinematic = false;
			
			_endMousePos = Input.mousePosition;
			_unitSelected = false;
			_tension = false;
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
		
		Debug.Log(rigidbody.velocity.magnitude);
//		if(rigidbody.velocity.magnitude > 0.1f)
//		Debug.Log("Velocity: " + rigidbody.velocity);
		
		
	}
	
	public void LaunchCharacter(Vector3 startPostion, Vector3 endPostion)
	{
		// Play animation
		_Swinging = false;
		_AnimPlayer.PlayAnimation(CharacterAnimationType.JumpStart, false);
		
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
		_AnimPlayer.PlayAnimation(CharacterAnimationType.JumpEnd, true);
		
		// Play animation
		_AnimPlayer.PlayAnimation(CharacterAnimationType.Holdon, false,0.5f);
		
		IsGrabbingAnchor = true;
		_Anchor = anchor;
		_AnchorPos = anchor.position;
	}
}
