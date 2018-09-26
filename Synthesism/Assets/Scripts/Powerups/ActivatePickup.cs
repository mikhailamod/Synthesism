using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatePickup : MonoBehaviour {

	public float heightOffset;
	public GameObject spike;

	// Use this for initialization
	public void FireSpike(Vector3 dir) {
		Instantiate(spike);
	}
}
