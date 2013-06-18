var target : Transform;
var smoothTime = 0.3;
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

       thisTransform.position.x = Mathf.Lerp( thisTransform.position.x, target.position.x + xOffset, Time.deltaTime * smoothTime);

       thisTransform.position.z = Mathf.Lerp( thisTransform.position.z, target.position.z + yOffset, Time.deltaTime * smoothTime);
		thisTransform.LookAt(target);
}