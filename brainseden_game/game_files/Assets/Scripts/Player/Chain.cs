using UnityEngine;
using System.Collections;

public class Chain : MonoBehaviour {
	
	
	public GameObject Player1Anchor;
	public GameObject Player2Anchor;
	public static float MaximumDistance;
	public int NumObjects;
	public float MaxDistance;
	private GameObject[] ChainLinks;
	public GameObject ChainLinkPrefab;
	// Use this for initialization
	void Start () 
	{
		Physics.gravity = Vector3.down*20f;
		
		//set the limit of the joint to this distance
		SoftJointLimit limit = Player1Anchor.transform.parent.GetComponent<ConfigurableJoint>().linearLimit;
		limit.limit = MaxDistance;
		Player1Anchor.transform.parent.GetComponent<ConfigurableJoint>().linearLimit = limit;
		
		MaximumDistance = MaxDistance*0.5f;
//		MaximumDistance = MaxDistance;
		
//		Player1Anchor.transform.parent.GetComponent<ConfigurableJoint>().linearLimit.limit = MaxDistance;
		if(NumObjects %2 == 0)++NumObjects;
		ChainLinks = new GameObject[NumObjects];
		
		float distStep = MaximumDistance / NumObjects;
		
		
		
		for(int i= 0; i < NumObjects; ++i)
		{
			ChainLinks[i] = (GameObject)Instantiate(ChainLinkPrefab,Player1Anchor.transform.position + Vector3.right*distStep*i,Quaternion.identity);
			if(i == 0)
			{
				ChainLinks[i].GetComponent<CharacterJoint>().anchor = -(Vector3.right*distStep);
				ChainLinks[i].GetComponent<CharacterJoint>().connectedBody = Player1Anchor.rigidbody;
				
			}
			
			
			
//			else if (i == NumObjects - 1)
//			{
//				ChainLinks[i].GetComponent<HingeJoint>().connectedBody = Player2Anchor.rigidbody;
//				HingeJoint joint = (HingeJoint)ChainLinks[i].AddComponent(typeof(HingeJoint));
//				joint.connectedBody = Player2Anchor.rigidbody;
//			}
			else
			{
				ChainLinks[i].GetComponent<CharacterJoint>().anchor = -(Vector3.right*distStep);
				ChainLinks[i].GetComponent<CharacterJoint>().connectedBody = ChainLinks[i-1].rigidbody;
			}
			
			
			
		}
		Player2Anchor.transform.parent.position = Player1Anchor.transform.position + Vector3.right*MaximumDistance;
		Player2Anchor.GetComponent<CharacterJoint>().connectedBody = ChainLinks[NumObjects-1].rigidbody;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
