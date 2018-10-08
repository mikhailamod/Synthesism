using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatePickup : MonoBehaviour {

	public float heightOffset;
	public float spikeSpeed;
	
	public Transform spikePosition;
	public Transform spillPosition;
	
	public GameObject spike;
	public GameObject oil;

	// Use this for initialization
	public void FireSpike(Vector3 dir) {
		GameObject moveSpike = Instantiate(spike);
		moveSpike.transform.position = (spikePosition.position);
		moveSpike.transform.rotation = spikePosition.rotation;
		moveSpike.GetComponent<Rigidbody>().AddForce(spikePosition.transform.forward * spikeSpeed);
	}

	public void SpillOil(Vector3 dir) {
		GameObject moveSpill = Instantiate(oil);
		moveSpill.transform.position = (spillPosition.position);
	}

	//initiates slip procedure
    public void StartSlip() {
        StartCoroutine(GetComponent<PlayerCarController>().carMovementProperties.slip());
    }
}
