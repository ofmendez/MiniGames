using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MiniGame2 : MonoBehaviour {

	public GameObject boardPrefab;
	public GameObject boardHolder;
	public GameObject winScreen;
	public GameObject looseScreen;



	void Start (){
		ResetGame(1);
	}
	
	
 	public void LaunchLevel(Text ns){
		int n; int.TryParse(ns.text, out n);
		if (n>2 && n <9)
			ResetGame(n);
 	}

	private void ResetGame(int i)	{
		
	}

 

}
