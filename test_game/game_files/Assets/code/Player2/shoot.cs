using UnityEngine;
using System.Collections;

public class shoot : MonoBehaviour {
	public GameObject bullet;
	public float timer;
	public Transform shootpos;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (Input.GetButton("Fire1") && timer > 0.1) {
            timer = 0;
            Instantiate(bullet, shootpos.position, transform.rotation);
        }
	}
}
