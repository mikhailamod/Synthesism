using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatePickup : MonoBehaviour {

	public float heightOffset;
	
	public Transform spikePosition;
	public GameObject spike;
	public ParticleSystem smoke;

	// Use this for initialization
	public void FireSpike(Vector3 dir) {
		GameObject moveSpike = Instantiate(spike);
		moveSpike.transform.position = (spikePosition.position);
		moveSpike.transform.rotation = spikePosition.rotation;
	}
}
