using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MiniGame2 :  Singleton<MiniGame2>  {

	public GameObject boardPrefab;
	public GameObject boardHolder;
	public GameObject winScreen;
	public GameObject looseScreen;
	public PartnerMG2 partnerPrefab;
	public Transform ball;
	List<PartnerMG2> partners = new List<PartnerMG2>();
	int lastCaught;
	int numberOfPartners = 7;
	public bool isPlaying;


	void Start (){
		ResetGame(1);		
	}
	
	
 	public void LaunchLevel(Text ns){
		int n; int.TryParse(ns.text, out n);
		if (n>2 && n <9)
			ResetGame(n);
 	}



	void ResetGame(int i)	{
		LoadPartners();//Deben haberse calculado xMin,xMax,yMin..... para poder llamar este método	
		lastCaught = 0;
		winScreen.SetActive(false);
		looseScreen.SetActive(false);
		isPlaying = true;

	}

	private void LoadPartners(){
		int limit = 10;
		for (int i = 1; i <= numberOfPartners; i++){
			Vector2 newParnerPos;
			bool inFreeArea;
			do{			
				newParnerPos = new Vector2(Random.Range(partnerPrefab.GetLimit(1),partnerPrefab.GetLimit(2)), Random.Range(partnerPrefab.GetLimit(3),partnerPrefab.GetLimit(4)));
				inFreeArea = true;
				foreach (PartnerMG2 p in partners){
					if (Vector2.Distance(p.transform.position, newParnerPos) <= PartnerMG2.partnerRadio*2)
						inFreeArea = false;

				}
				if(Vector2.Distance(ball.position, newParnerPos) <= PartnerMG2.partnerRadio*2)
					inFreeArea = false;
			}while (!inFreeArea);
						
			PartnerMG2 partner = Instantiate(partnerPrefab, partnerPrefab.gameObject.transform.parent.transform, false);
			partner.gameObject.SetActive(true);
			partner.transform.position =  newParnerPos;
			partner.SetNumber(i);
			partners.Add(partner);
		}
	}


	public bool IsPartnerToCatch(int partnerNumber){
		bool isToCatch = false;
		if (lastCaught+1 == partnerNumber){
			lastCaught++;
			isToCatch = true;
		}
		if(lastCaught == numberOfPartners){
			winScreen.SetActive(true);
			isPlaying = false;

		}
		return isToCatch;
	}
	
	public void GameOver(){
		looseScreen.SetActive(true);
		isPlaying = false;
		
	}
}
