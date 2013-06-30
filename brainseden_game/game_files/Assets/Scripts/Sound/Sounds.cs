using UnityEngine;
using System.Collections.Generic;

public enum SoundEffectType
{
	SoundEffect1,
	SoundEffect2
}
public enum LevelType
{
	Morning,
	Noon,
	Evening,
	Menu
}
public class Sounds : MonoBehaviour {
	
	public float MusicVolume = 0.9f;
	public float SoundEffectsVolume = 0.5f;
	
	
	public AudioClip SoundEffect1;
	public AudioClip SoundEffect2;
	
	
	public AudioClip BackgroundNoon;
	public AudioClip BackgroundMorning;
	public AudioClip BackgroundEvening;
	public AudioClip BackgroundMenu;
	
	public static AudioClip sBackgroundNoon;
	public static AudioClip sBackgroundMorning;
	public static AudioClip sBackgroundEvening;
	public static AudioClip sBackgroundMenu;
	
	public static AudioSource _Audio1;
	public static AudioSource _Audio2;
	public static AudioSource _SoundEffectsSource;
	
	private static int CurrentAudio = 0;
	private static float DesiredVolume1 = 1f;
	private static float DesiredVolume2 = 0f;
	private float currentVolume1 = 1f;
	private float currentVolume2 = 0f;
	
//	private static int currentSoundEffectIndex;
	
	public static Dictionary<SoundEffectType,AudioClip[]> SoundEffectsDict = new Dictionary<SoundEffectType, AudioClip[]>();
	
	// Use this for initialization
	void Start () 
	{
		AudioSource[] sources = Camera.mainCamera.GetComponents<AudioSource>();
		_Audio1 = sources[0];
		_Audio2 = sources[1];
		_Audio1.loop = true;
		_Audio2.loop = true;
		
		_SoundEffectsSource = sources[2];
		_SoundEffectsSource.loop = false;
		_SoundEffectsSource.volume = SoundEffectsVolume;

		CurrentAudio = 0;
		
		_Audio1.volume = currentVolume1;
		_Audio2.volume = currentVolume2;
		
		sBackgroundNoon = BackgroundNoon;
		sBackgroundMorning = BackgroundMorning;
		sBackgroundEvening = BackgroundEvening;
		sBackgroundMenu = BackgroundMenu;
		
		SoundEffectsDict.Clear();
		SoundEffectsDict.Add(SoundEffectType.SoundEffect1,new AudioClip[]{SoundEffect1});
		SoundEffectsDict.Add(SoundEffectType.SoundEffect2,new AudioClip[]{SoundEffect2});
	
		PlayLevelBackground(LevelType.Morning);
		
	}
	

	public static void PlaySoundEffect(SoundEffectType type)
	{
		AudioClip[] clips = SoundEffectsDict[type];
		int rand = Random.Range(0,clips.Length);
		
		_SoundEffectsSource.PlayOneShot(clips[rand]);
	}
	public static void PlayLevelBackground(LevelType level)
	{
		++CurrentAudio;
		CurrentAudio%=2;
		AudioSource nextAudio; 
		
		if(CurrentAudio == 0)
		{
			nextAudio = _Audio1;
			DesiredVolume1 = 0.75f;
			DesiredVolume2 = 0f;
		}
		else
		{
			nextAudio = _Audio2;
			DesiredVolume1 = 0f;
			DesiredVolume2 = 0.75f;
		}
		
		if(nextAudio == null) return;
		switch(level)
		{
		case LevelType.Morning:
			nextAudio.clip = sBackgroundMorning;
			break;
		case LevelType.Noon:
			nextAudio.clip = sBackgroundNoon;
			break;
		case LevelType.Evening:
			nextAudio.clip = sBackgroundEvening;
			break;
		case LevelType.Menu:
			nextAudio.clip = sBackgroundMenu;
			break;
		}
		nextAudio.Play();
		
	}
	// Update is called once per frame
	void Update () 
	{
	
		
		float lerpVal = Mathf.Clamp01(0.05f*Time.deltaTime*60f);
		
		currentVolume1 = Mathf.Lerp(currentVolume1,DesiredVolume1,lerpVal);
		currentVolume2 = Mathf.Lerp(currentVolume2,DesiredVolume2,lerpVal);
		
		_Audio1.volume = currentVolume1;
		_Audio2.volume = currentVolume2;
	}
}
