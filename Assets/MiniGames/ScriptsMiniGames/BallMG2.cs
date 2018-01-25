using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMG2 : MonoBehaviour {

	public HealthBar healthBar;

	Vector2 downPosition;
	Vector2 originPosition;
	int ballRadio = 24;
	float xMin;
	float xMax;
	float yMin;
	float yMax;
	float newX;
	float newY;
	float timeImmunity = 1f;
	bool isImmune;

	void Start(){
		xMin = transform.parent.GetComponent<RectTransform>().anchorMin.x * Screen.width  + ballRadio ;
		xMax = transform.parent.GetComponent<RectTransform>().anchorMax.x * Screen.width  - ballRadio ;
		yMin = transform.parent.GetComponent<RectTransform>().anchorMin.y * Screen.height + ballRadio ;
		yMax = transform.parent.GetComponent<RectTransform>().anchorMax.y * Screen.height - ballRadio ;
		isImmune = false;
	}

	private void OnTriggerEnter2D(Collider2D other)	{
		if(!isImmune && other.CompareTag("Enemy") )
			StartCoroutine(DamageAndTemporaryImmune());
	}

	private IEnumerator DamageAndTemporaryImmune(){
		isImmune = true;
		healthBar.TakeDamage(34);
		yield return new WaitForSeconds(timeImmunity);
		isImmune = false;
	}

	void Update () {
		if(MiniGame2.main.isPlaying){
		    if(Input.GetMouseButtonDown(0)) {
				downPosition   = (Vector2)Input.mousePosition;
				originPosition = (Vector2)transform.position;
		    }

		    if(Input.GetMouseButton(0)) {
				transform.position = originPosition + ((Vector2)Input.mousePosition - downPosition)*2.8f;
			    newX = Mathf.Clamp(transform.position.x, xMin,  xMax );
				newY = Mathf.Clamp(transform.position.y, yMin , yMax );
				transform.position = new Vector3(newX,newY,transform.position.z);
		    }
		}

	}


}
