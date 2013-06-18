using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{  
  public float speed = 12;

  // START - Use this for initialization
  void Start () {}

  // UPDATE is called once per frame
  void Update () {
    // Moves player left, right, up, down. No collision.
    float x = Input.GetAxis("Horizontal") * Time.smoothDeltaTime * speed;
    float y = Input.GetAxis("Vertical") * Time.smoothDeltaTime * speed;
    transform.Translate(x,0,y,Space.Self);
  }
}