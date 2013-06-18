var target : Transform;
var smoothTime = 0.5;
var xOffset : float = 1.0;
var yOffset : float = 1.0;

private var thisTransform : Transform;
private var velocity : Vector2;


function Start()
{
       thisTransform = transform;
}

function LateUpdate()
{
		if (Mathf.Abs((thisTransform.position - target.position).magnitude) < 25 )
		{
	       thisTransform.position.x = Mathf.Lerp( thisTransform.position.x, target.position.x + xOffset, Time.deltaTime * smoothTime);
	
	       thisTransform.position.z = Mathf.Lerp( thisTransform.position.z, target.position.z + yOffset, Time.deltaTime * smoothTime);
			thisTransform.LookAt(target);
		}
}