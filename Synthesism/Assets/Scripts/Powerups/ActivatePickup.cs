using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatePickup : MonoBehaviour {

	public float heightOffset;
	public float spikeSpeed;
	
	public Transform spikePosition;
	public GameObject spike;

	// Use this for initialization
	public void FireSpike(Vector3 dir) {
		GameObject moveSpike = Instantiate(spike);
		moveSpike.transform.position = (spikePosition.position);
		moveSpike.transform.rotation = spikePosition.rotation;
		moveSpike.GetComponent<Rigidbody>().AddForce(spikePosition.transform.forward * spikeSpeed);
	}
}
