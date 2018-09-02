using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostController : MonoBehaviour {

	public float speed = 10f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		RotateCube();

	}

	void RotateCube() {
		transform.Rotate(Vector3.up, speed * Time.deltaTime);
		transform.Rotate(Vector3.left, speed * Time.deltaTime);
	}

	void OnTriggerEnter(Collider other) {
		Destroy(this.gameObject);
		other.gameObject.GetComponent<PlayerCarController>().boost();
	}
}
