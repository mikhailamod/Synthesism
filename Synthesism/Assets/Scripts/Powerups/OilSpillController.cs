using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilSpillController : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag=="Player") {
			other.gameObject.GetComponent<ActivatePickup>().StartSlip();
			GameObject parent = gameObject.transform.parent.gameObject;
			Destroy(parent);
		}
	}
}
