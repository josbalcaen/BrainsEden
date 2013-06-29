using UnityEngine;
using System.Collections;

public class Chain : MonoBehaviour {
	
	public Material mat;
	public GameObject Player1Anchor;
	public GameObject Player2Anchor;
	public static float MaximumDistance;
	public int NumObjects;
	public float MaxDistance;
	private GameObject[] ChainLinks;
	public GameObject ChainLinkPrefab;
	public float Radius;
	private Mesh _Mesh;
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
		GenerateMesh();
	}
	
//	void GenerateMesh2()
//	{
//		Vector3[] vertices = new Vector3[(numSides+1)*NumObjects];
//		Vector2[] UVs = new Vector2[(numSides+1)*NumObjects];
//		int[] indices = new int[(NumObjects-1)*numSides* 6];
//		
//		int NR_OF_STACKS = 5;
//		for(int k = 0; k < NumObjects; ++k)
//		{
//			Quaternion rot = ChainLinks[k].transform.rotation;
////			Quaternion rot = Quaternion.identity;
////			Vector3 localPos = Vector3.right*20*i;
//			
//			Vector3 localPos = ChainLinks[k].transform.position;
//			
//			Vector3 dir = rot * Vector3.right;
//			Vector3 up = Vector3.Cross(dir,new Vector3(0,0,1));
//			
//			for(int i =0; i < NR_OF_STACKS+1; ++i) //last of ring = first of ring
//			{
//				float angle = (float)Mathf.PI / i;
//				
//				
//				D3DXVECTOR3 offset;
//				D3DXMATRIX rotMat;
//				
//				D3DXMatrixRotationAxis(&rotMat,&dir,angle);
//				D3DXVec3TransformCoord(&offset,&tangent,&rotMat);
//				//Calculate circle around pos, around direction;
//	
//				m_VecVertices.push_back(VertexPosTex(midPos + offset,D3DXVECTOR2(k*(1.0f/NR_OF_RINGS),i*(1.0f/NR_OF_STACKS))));
//			}
//		}
//		
//		int NR_OF_RINGS = NumObjects;
//		for(int k = 0; k < NR_OF_RINGS;++k)
//		{
//			
//			D3DXVECTOR3 midPos = D3DXVECTOR3(static_cast<float>(k),0,0);
//			D3DXVECTOR3 dir = D3DXVECTOR3(1,0,0);
//			D3DXVECTOR3 tangent = D3DXVECTOR3(0,0,1);
//			for(int i =0; i < NR_OF_STACKS+1; ++i) //last of ring = first of ring
//			{
//				float angle = (float)M_PI / i;
//	
//				D3DXVECTOR3 offset;
//				D3DXMATRIX rotMat;
//				
//				D3DXMatrixRotationAxis(&rotMat,&dir,angle);
//				D3DXVec3TransformCoord(&offset,&tangent,&rotMat);
//				//Calculate circle around pos, around direction;
//	
//				m_VecVertices.push_back(VertexPosTex(midPos + offset,D3DXVECTOR2(k*(1.0f/NR_OF_RINGS),i*(1.0f/NR_OF_STACKS))));
//			}
//		}
//		for(int i =0; i < NR_OF_RINGS-1; ++i)
//		{
//			for(int j= 0; j < NR_OF_STACKS; ++j)
//			{
//				int ringCount = NR_OF_STACKS+1; //index buffer: first of ring is same as last (double)
//				m_VecIndices.push_back(j +i*ringCount);
//				m_VecIndices.push_back(j+ringCount  + i*ringCount);
//				m_VecIndices.push_back((j+1) +ringCount  +i*ringCount);
//	
//				m_VecIndices.push_back(j +i*ringCount);
//				m_VecIndices.push_back((j+1)+ringCount +i*ringCount);
//				m_VecIndices.push_back((j+1) +i*ringCount);
//			}
//		}
//	}
//	
	void GenerateMesh()
	{
		float radius = Radius;
		
		
		int numSides = 5;
				
		Vector3[] vertices = new Vector3[numSides*NumObjects];
		Vector2[] UVs = new Vector2[numSides*NumObjects];
		int[] indices = new int[(NumObjects-1)*numSides* 6];
		//iterate over objects
		for(int i= 0; i < NumObjects; ++i)
		{
			Quaternion rot = ChainLinks[i].transform.rotation;
//			Quaternion rot = Quaternion.identity;
//			Vector3 localPos = Vector3.right*20*i;
			
			Vector3 localPos = ChainLinks[i].transform.position;
			
			Vector3 dir = rot * Vector3.left;
			Vector3 up = Vector3.Cross(dir,new Vector3(0,0,1));

			//iterate over sides
			float angleStep = -360f/(float)numSides;
			for(int j = 0; j < numSides; ++j)
			{
				float angle = 240f+ j*angleStep;
				Vector3 offset = Quaternion.AngleAxis(angle,dir)*up*radius;
				
				vertices[j+numSides*i] = localPos + offset;
				UVs[j+numSides*i] = new Vector2((float)j/(float)numSides,i);
			}
			
		}
		
		//Indices
		for(int i= 0; i < (NumObjects-1)*numSides* 6; ++i)
		{
			//triangle 1:
			int quadNum = i/6; // = start vertex
			int ringNum = quadNum/numSides;
			int sideNum = quadNum%numSides;
			switch(i%6)
			{
			case 3:
			case 0:
				
				indices[i] = quadNum;
				break;
			case 1:
				indices[i] = ringNum*numSides + (quadNum+1)%numSides;
				break;
			case 4:
			case 2:
				indices[i] = (ringNum+1)*numSides + (quadNum+1)%numSides;
				break;
				

			case 5:
				indices[i] = (ringNum+1)*numSides + quadNum%numSides;
				break;
			}
			
			
			
		}
		
		
		_Mesh = new Mesh();
		_Mesh.vertices = vertices;
		_Mesh.SetTriangles(indices,0);
		_Mesh.uv = UVs;
		_Mesh.RecalculateNormals();
		GetComponent<MeshFilter>().mesh = _Mesh;
		
	}
	
	
//	void GenerateMesh()
//	{
//		
//		    float size = 1;
//       Vector3[] vertexList = new Vector3[]{
//          new Vector3(-size, -size, -size),
//          new Vector3(-size,  size, -size),
//          new Vector3( size,  size, -size),
//          new Vector3( size, -size, -size),
//          new Vector3( size, -size,  size),
//          new Vector3( size,  size,  size),
//          new Vector3(-size,  size,  size),
//          new Vector3(-size, -size,  size)
//		};
//       
//        int[] faceList = new int[]{
//          0, 1, 3, //   1: face arrière
//          0, 2, 3,
//          3, 2, 5, //   2: face droite
//          3, 5, 4,
//          5, 2, 1, //   3: face dessue
//          5, 1, 6,
//          3, 4, 7, //   4: face dessous
//          3, 7, 0,
//          0, 7, 6, //   5: face gauche
//          0, 6, 1,
//          4, 5, 6, //   6: face avant
//          4, 6, 7
//		};
//         
//         Vector2[] textureCoordinate= new Vector2[]
//		{
//        new Vector2(0.0f,  0.0f),
//        new Vector2(1.0f,  0.0f),
//        new Vector2(0.0f,  1.0f),
//        new Vector2(1.0f,  0.0f)
//		};
//       
//     
//       Mesh mesh = new Mesh();
//       mesh.vertices = vertexList;
//       mesh.triangles = faceList;
//       
//       mesh.uv = textureCoordinate;
//       mesh.RecalculateNormals();
//       //mesh.transform.position = Vector3(0, 0.5, 0)
//       //mesh.position = Vector3(0, 0.5, 0)
//       GetComponent<MeshFilter>().mesh = mesh;  
//       
//     
//    }
     
   
		
		
		
	
	void Update () {
	
		GenerateMesh();
	}
}
