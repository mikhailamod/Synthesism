using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostController : MonoBehaviour {
	void OnTriggerEnter(Collider other) {
		Debug.Log("Collided with boost!");
		Destroy(this.gameObject);
		other.gameObject.GetComponent<PlayerCarController>().boost();
	}
}
