using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowController : MonoBehaviour {

	public float followSpeed;
	public float lookSpeed;
	public Transform targetObject;
	public Vector3 offset;

	public void LookAtTarget()
	{
		Vector3 lookDirection = targetObject.position - transform.position;
		Quaternion rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
		transform.rotation = Quaternion.Lerp(transform.rotation, rotation, lookSpeed * Time.deltaTime);
	}

	public void MoveToTarget()
	{
		Vector3 targetPosition = targetObject.position + 
			targetObject.forward * offset.z + 
			targetObject.right * offset.x + 
			targetObject.up * offset.y;
			transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
	}

	private void FixedUpdate()
	{
		LookAtTarget();
		MoveToTarget();
	}

}
