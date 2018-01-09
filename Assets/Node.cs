using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour{
	public string value;
	public Text mText;
	
	public void SetValue(string _s){
		mText.text = (_s == "999999")? "INF" :_s;
		value = mText.text;
	}


	public int GetValue(){	
		return  value == "INF"? 999999 : Convert.ToInt32(value);
	}

	public void AddValue(int i){
		int actual = GetValue();
		int result = actual == 999999 ? i: actual+i;
		SetValue(""+result);
	}
}
