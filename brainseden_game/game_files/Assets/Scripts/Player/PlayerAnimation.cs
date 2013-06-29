using UnityEngine;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class Animations
{
	public string Start = "";
	public string End = "";
	public string[] Loop;
}

public enum CharacterAnimationType
{
	Idle,
	Jumping,
	Swinging,
	Falling,
	Hanging
}

public class AnimationEventArgs: EventArgs
{
	public CharacterAnimationType AnimState{get; private set;}
	public AnimationEventArgs ( CharacterAnimationType type)
	{
		AnimState= type;
	}
}

public class PlayerAnimation : MonoBehaviour {
	
	private enum AnimState
	{
		Start,
		End,
		Loop,
	}
	
	public event EventHandler<AnimationEventArgs> AnimationFinished;
	public event EventHandler<AnimationEventArgs> AnimationStarted;
	
	private	static Dictionary<CharacterAnimationType,Animations> _AnimDict = new Dictionary<CharacterAnimationType, Animations>();
	private bool _Initialized = false;
//	private bool playOnce = true;
//	private float speedMultiplier = 1.0f;
	private CharacterAnimationType _CurrentAnimationType;
	
	private bool _Loop = false;
	private string _LastAnim = "";
	private AnimState _CurrentState;
	
	void Initialize()
	{
		if(_AnimDict.Count == 0)
		{
			//Idle
			_AnimDict.Add(CharacterAnimationType.Idle,new Animations()
			{
				Loop = new String[]{"AegirIdle"}
			});
			//Jump
			_AnimDict.Add(CharacterAnimationType.Jumping,new Animations()
			{
				Start = "AegirFalling",
				Loop = new String[]{"AegirJumping"}
			});
			
		}
		_Initialized = true;
	}
	
	// Use this for initialization
	void Start () 
	{	
		animation["AegirFalling"].speed = 0.0001f;
		animation["AegirIdle"].speed = 0.0001f;
		if(_Initialized)return;
		Initialize();
		PlayAnimation(CharacterAnimationType.Jumping,true,1.0f);
	
	}
	
	
	
	
	public void PlayAnimation(CharacterAnimationType type, bool loop = true, float crossfade = 0f)
	{
	
		_Loop = loop;
		if(!_Initialized)
		{
			Initialize();
		}
		_CurrentAnimationType = type;
		
		//Play the animation
		Animations data = _AnimDict[type];
		_LastAnim = data.Start;
		_CurrentState = AnimState.Start;
		
		if(_LastAnim == ""  )
		{
			if(data.Loop.Length >0)
			{
				_LastAnim = data.Loop[0];
				_CurrentState = AnimState.Loop;
			}
		}
		//Debug.Log("Playing Anim " + _LastAnim);
		if(crossfade == 0f)
		{
			animation.Play(_LastAnim);
			
		}
		else
		{
			animation.CrossFade(_LastAnim,crossfade);
			_CurrentState = AnimState.Loop;
		}
	}
	
	public void EndAnimation(float crossfade = 0f)
	{
		_CurrentState = AnimState.End;
		if(_AnimDict[_CurrentAnimationType].End != "")
		{
			if(crossfade == 0)
			{
				animation.Play(_AnimDict[_CurrentAnimationType].End);
				//Play end animation
			}
			else
			{
				animation.CrossFade(_AnimDict[_CurrentAnimationType].End,0.2f);
				//Set state to end
			}
			_LastAnim = _AnimDict[_CurrentAnimationType].End;
		}
		else
		{
			OnAnimationFinished(_CurrentAnimationType);
		}
	}
	
	

	// Update is called once per frame
	
	void Update () 
	{
		if(!animation.isPlaying)
		{
			switch (_CurrentState)
			{
			case AnimState.Start:
				//Play random loop animation
				string[] animations = _AnimDict[_CurrentAnimationType].Loop;
				if(animations.Length > 0)
				{
					_LastAnim = animations[Random.Range(0,animations.Length)];
					animation.Play(_LastAnim);
					_CurrentState = AnimState.Loop;
					
					if(AnimationStarted != null)AnimationStarted(this, new AnimationEventArgs(_CurrentAnimationType));
				}
				else if(_AnimDict[_CurrentAnimationType].End != "")
				{
					_CurrentState = AnimState.End;
					_LastAnim = _AnimDict[_CurrentAnimationType].End;
					animation.Play(_LastAnim);
				}
				else
					OnAnimationFinished(_CurrentAnimationType);
				break;
			case AnimState.Loop:
				
				if(_Loop)
				{
					//Play random loop animation
					string[] anims = _AnimDict[_CurrentAnimationType].Loop;
					if(anims.Length > 0)
					{
						_LastAnim = anims[Random.Range(0,anims.Length)];
						animation.Play(_LastAnim);
					}
				}
				else if(_AnimDict[_CurrentAnimationType].End != "")
				{
					_CurrentState = AnimState.End;
					_LastAnim = _AnimDict[_CurrentAnimationType].End;
					animation.Play(_LastAnim);
				}
				else
				{
					OnAnimationFinished(_CurrentAnimationType);	
				}
				
				break;
			case AnimState.End:
					OnAnimationFinished(_CurrentAnimationType);
				break;
			}
		}
		

	}
	

		
	public void SetWalkSpeedMultiplier(float speed)
	{
		animation["Walk"].speed = 2*speed;
	}
	
	void OnAnimationFinished(CharacterAnimationType state)
	{
		if(AnimationFinished != null) AnimationFinished(this, new AnimationEventArgs(state));
	}
}