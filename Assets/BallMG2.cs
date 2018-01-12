using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMG2 : MonoBehaviour {

	public  Transform target;
	float speed = 6f;
	Vector2 downPosition;
	Vector2 targetPos;
	// Use this for initialization
	void Start () {
    	targetPos = transform.position;
	}
	
	void Update () {
	    if(Input.GetMouseButtonDown(0)) {
			downPosition = (Vector2)Input.mousePosition;
	    }

	    if(Input.GetMouseButton(0)) {
			transform.position = (Vector2)transform.position + ((Vector2)Input.mousePosition - downPosition);

	    }

		if (Input.GetMouseButtonUp (0) ) {
	    }

	}
}
