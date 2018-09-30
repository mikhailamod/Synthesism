using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour {

	public float rotationSpeed;
	public bool isSpike;

	void OnTriggerEnter(Collider other) {
		Debug.Log("Pickup!");

		//rm cube
		gameObject.SetActive(false);
		//need to reposition then set active(true)

		//generate number between 0,1
		isSpike = Random.value>0.5;

		//true for testing
		
		if(true) {
			other.gameObject.GetComponent<PlayerCarController>().carMovementProperties.hasSpike = true;
		}
		else {
			other.gameObject.GetComponent<PlayerCarController>().carMovementProperties.hasOil = true;
		}
	}

	void Update() {
		//rotate cube
		transform.Rotate(Vector3.left * Time.deltaTime * rotationSpeed);
		transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed);
	}
}
