using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		Destroy(gameObject);
		if(other.gameObject.tag=="Player") {
			other.gameObject.SetActive(false);
		}
	}
}
