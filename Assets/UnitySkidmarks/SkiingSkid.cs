using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkiingSkid : MonoBehaviour {
	// INSPECTOR SETTINGS

	[SerializeField]
	Rigidbody rb;
	[SerializeField]
	Skidmarks skidmarksController;

	// END INSPECTOR SETTINGS

	WheelCollider wheelCollider;
	WheelHit wheelHitInfo;

	private Collider collider;
	
	
	
	int lastSkid = -1; // Array index for the skidmarks controller. Index of last skidmark piece this wheel used
	float lastFixedUpdateTime;

	// #### UNITY INTERNAL METHODS ####

	protected void Awake() {
		wheelCollider = GetComponent<WheelCollider>();
		lastFixedUpdateTime = Time.time;
	}

	protected void FixedUpdate() {
		lastFixedUpdateTime = Time.time;
	}

	protected void LateUpdate() {
		
		// lastSkid = skidmarksController.AddSkidMark(transform.position, transform.up, Color.black, lastSkid);
		
		Ray ray = new Ray(transform.position+new Vector3(-0.12f,1, 0), -transform.up * 100);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100)) {
			
			// Account for further movement since the last FixedUpdate
			Vector3 skidPoint = hit.point;// + (rb.velocity * (Time.time - lastFixedUpdateTime));
			lastSkid = skidmarksController.AddSkidMark(skidPoint, hit.normal, Color.white, lastSkid);
		}
		else {
			lastSkid = -1;
		}
	}

	// #### PUBLIC METHODS ####

	// #### PROTECTED/PRIVATE METHODS ####


}
