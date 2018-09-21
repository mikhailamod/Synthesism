using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostController : MonoBehaviour {

	public float boostSpeed;

	void OnTriggerEnter(Collider other) {
		other.gameObject.GetComponent<Rigidbody>().AddForce(other.transform.forward * boostSpeed, ForceMode.Impulse);
	}


}
