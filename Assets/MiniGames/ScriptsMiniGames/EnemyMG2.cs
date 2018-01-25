using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyMG2 : MonoBehaviour {

	Vector2 downPosition;
	Vector2 originPosition;
	int enemyRadio = 40;
	float speed = 90;
	Vector2 dir;
	float newX;
	float newY;

	float xMin;
	float xMax;
	float yMin;
	float yMax;
	
	void Start(){
		dir = Quaternion.Euler(0, 0, Random.Range(0.0f,360.0f)) * Vector2.up;
		xMin = transform.parent.GetComponent<RectTransform>().anchorMin.x * Screen.width  + enemyRadio ;
		xMax = transform.parent.GetComponent<RectTransform>().anchorMax.x * Screen.width  - enemyRadio ;
		yMin = transform.parent.GetComponent<RectTransform>().anchorMin.y * Screen.height + enemyRadio ;
		yMax = transform.parent.GetComponent<RectTransform>().anchorMax.y * Screen.height - enemyRadio ;
	}


	void Update () {
		if(MiniGame2.main.isPlaying){

		    transform.position += (Vector3)dir * speed * Time.deltaTime ;

			if (transform.position.x <= xMin)
				dir = Vector2.Reflect(dir,Vector2.right);
			if (transform.position.x >= xMax)
				dir = Vector2.Reflect(dir,Vector2.left);
			if (transform.position.y <= yMin)
				dir = Vector2.Reflect(dir,Vector2.up);
			if (transform.position.y >= yMax)
				dir = Vector2.Reflect(dir,Vector2.down);
		}

	}
}
