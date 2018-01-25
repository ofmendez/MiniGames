using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartnerMG2 : MonoBehaviour {

	int partnerNumber;
	Text numHolder;
	public static int partnerRadio = 45;
	float xMin;
	float xMax;
	float yMin;
	float yMax;

	
	public float GetLimit(int _l){
		if(Math.Abs(xMin) < 0.1f)
			CalcLimits();
		switch (_l) {
		    case 1:
				return xMin ;
		      break;
		    case 2:
				return xMax ;
		      break;		    
	        case 3:
				return yMin ;
		      break;
     		case 4:
				return yMax ;
		      break;
		}
		return 0;

	}

	void CalcLimits(){
		xMin = transform.parent.GetComponent<RectTransform>().anchorMin.x * Screen.width  + partnerRadio ;
		xMax = transform.parent.GetComponent<RectTransform>().anchorMax.x * Screen.width  - partnerRadio ;
		yMin = transform.parent.GetComponent<RectTransform>().anchorMin.y * Screen.height + partnerRadio ;
		yMax = transform.parent.GetComponent<RectTransform>().anchorMax.y * Screen.height - partnerRadio ;
	}


	private void OnTriggerEnter2D(Collider2D other)	{
		Debug.Log("tocado");
		if ( MiniGame2.main.IsPartnerToCatch(partnerNumber))
		{
			Debug.Log("MUEREEEEEE");
			Destroy(this.gameObject);
		}
	}




	public void SetNumber(int _p1){
		partnerNumber = _p1;
		if(! numHolder)
			numHolder = GetComponentInChildren<Text>();
		numHolder.text = ""+_p1;
	}
	
}
