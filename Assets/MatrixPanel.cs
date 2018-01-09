using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MatrixPanel : MonoBehaviour {
	
//	public Contain containHorizontal;
	public Contain containVertical;
	public int nNodes;
	public Toggle tSelected;

	int[,] grafo ;
	// public GameObject buttonsSums;
	// public GameObject buttonsAdvance;
	// public GameObject buttonsPlus_minus;
	// public GameObject buttonBackSteps;
	public int actualStage;
	public Text title;
	

	public void ApplyAndBeginProcess(){
		// buttonsSums.SetActive(false);
		// buttonsPlus_minus.SetActive(false);
		// buttonsAdvance.SetActive(true);
		// buttonBackSteps.SetActive(true);
		GetInitValues();
		// buttonsAdvance.GetComponentsInChildren<Button>()[0].interactable = false;
		// buttonsAdvance.GetComponentsInChildren<Button>()[1].interactable = true;
		ShowFloydUntil(0);
	}

	public void NextFloyd(){
		if (actualStage < nNodes){
			ShowFloydUntil(actualStage+1);			
		}
		// buttonsAdvance.GetComponentsInChildren<Button>()[0].interactable = (actualStage>0);
		// buttonsAdvance.GetComponentsInChildren<Button>()[1].interactable =  (actualStage<nNodes);
	}

	public void PreviousFloyd(){
		if (actualStage > 0){
			ShowFloydUntil(actualStage-1);			
		}
		// buttonsAdvance.GetComponentsInChildren<Button>()[0].interactable = (actualStage>0);
		// buttonsAdvance.GetComponentsInChildren<Button>()[1].interactable =  (actualStage<nNodes);
	}
	
	void ShowFloydUntil(int i){
		actualStage = i;
		title.text = "D (" + i + ")";
		PaintWhiteSquares();
		ExecuteFloydUntil(i);
		PaintStepSquares(i-1);
	}


	void ExecuteFloydUntil(int target){
		int[,] distance = new int[nNodes, nNodes];

		for (int i = 0; i < nNodes; ++i)
		for (int j = 0; j < nNodes; ++j)
			distance[i, j] = grafo[i, j];

		for (int k = 0; k < target; ++k){
			for (int i = 0; i < nNodes; ++i){
				for (int j = 0; j < nNodes; ++j){
					if (distance[i, k] + distance[k, j] < distance[i, j]){
						distance[i, j] = distance[i, k] + distance[k, j];
						if (distance[i, k] == 999999 || distance[k, j] == 999999)
							distance[i, j] = 999999;
					}
				}
			}
		}
		ShowThisStage(distance);
	}

	private void ShowThisStage(int[,] distance){
		List<Node> nodes = GetComponentsInChildren<Node>(true).ToList();
		int i =0,j = 0;
		foreach (Node node in nodes){
			node.SetValue(""+distance[j,i]);
			i++;
			if ((i % nNodes) == 0){
				i = 0;
				j++;
			}
			i= (i%nNodes) ==0 ? 0: i;
		}
	}


	void GetInitValues(){
		List<Node> nodes = GetComponentsInChildren<Node>(true).ToList();
		int i =0 ,j= 0;		
		foreach (Node node in nodes){
			grafo[j,i] = node.GetValue();
			i++;
			if ((i % nNodes) == 0){
				i = 0;
				j++;
			}
			i= (i%nNodes) ==0 ? 0: i;
		}
	}
	

	// SETUP

	public void RestartSetup(){
		// buttonsSums.SetActive(true);
		// buttonsPlus_minus.SetActive(true);
		actualStage = 0;
		// buttonsAdvance.SetActive(false);
		// buttonBackSteps.SetActive(false);
		ShowThisStage(grafo);
		PaintWhiteSquares();
	}

	public void Setup(int n){
		// buttonsSums.SetActive(true);
		// buttonsPlus_minus.SetActive(true);
		actualStage = 0;
		// buttonsAdvance.SetActive(false);
		// buttonBackSteps.SetActive(false);
		grafo = new int[n,n];
		nNodes = n;
		containVertical.CreateNodes(nNodes);
		SetTitles();
		InitializeValues();
		PaintWhiteSquares();
		tSelected = GetComponentsInChildren<Node>()[nNodes+3].GetComponent<Toggle>();
		tSelected.isOn = true;
	}
	
	public void SetTitles(){
		List<Toggle> toggles = GetComponentsInChildren<Toggle>().ToList();
		
		char i='A';
		int j = 0;
		foreach (Toggle toggle in toggles){
			toggle.interactable = j != 0 && toggle.interactable;
			if ( (j <= nNodes && j>0) || ( j%(nNodes+1) ==0 && j>nNodes) ){
				toggle.GetComponentInChildren<Node>().SetValue( "" + i);
				toggle.interactable = false;
				i = (j==nNodes)? 'A': (char)(i+1);
			}
			j++;
		}
		foreach (Toggle toggle in toggles){
			if (!toggle.interactable){
				Destroy(toggle.GetComponent<Node>());				
			} 
		}
	}

	void InitializeValues(){
		
		int j = 0;
		List<Toggle> toggles = GetComponentsInChildren<Toggle>().ToList();
		foreach (Toggle toggle in toggles){
			if (j%(nNodes+1) >0 && j>nNodes){
				toggle.GetComponentInChildren<Text>().color = Color.black;
				toggle.GetComponentInChildren<Node>().SetValue( j%(nNodes+2) ==0?"0":"INF");
			}
			j++;
		}
	}
	
	//AUX NODES

	public void ShowPath(){
		PaintWhiteSquares();
		int[,] distance = new int[nNodes, nNodes];
		char[,] path = new char[nNodes, nNodes];
		char c = 'A';
		for (int i = 0; i < nNodes; ++i){
			for (int j = 0; j < nNodes; ++j){
				path[i, j] = c;
				distance[i, j] = grafo[i, j];
				c++;
			}
			c = 'A';
		}
		c = 'A';
		for (int k = 0; k < nNodes; ++k){
			for (int i = 0; i < nNodes; ++i){
				for (int j = 0; j < nNodes; ++j){
					if (distance[i, k] + distance[k, j] < distance[i, j]){
						distance[i, j] = distance[i, k] + distance[k, j];
						path[i, j] = (char)(c+k);
						if (distance[i, k] == 999999 || distance[k, j] == 999999)
							distance[i, j] = 999999;
					}
				}
			}
		}
		ShowThisPath(path);
		
	}

	public void ShowSteps(){
		ShowFloydUntil(0);
	}

	private void ShowThisPath(char[,] path){
		List<Node> nodes = GetComponentsInChildren<Node>(true).ToList();
		int i =0,j = 0;
		foreach (Node node in nodes){
			node.SetValue(  i!=j ?  (""+path[j,i])  : "--" );
			i++;
			if ((i % nNodes) == 0){
				i = 0;
				j++;
			}
			i= (i%nNodes) ==0 ? 0: i;
		}
	}

	public void SelectToggle (Toggle _t){
		if (_t.isOn){
			tSelected = _t;
			List<Toggle> toggles = GetComponentsInChildren<Toggle>().ToList();
			foreach (Toggle toggle in toggles){
				toggle.gameObject.SetActive(false);
				toggle.isOn = false;
				toggle.gameObject.SetActive(true);				
			}			
			tSelected.gameObject.SetActive(false);
			tSelected.isOn = true;
			tSelected.gameObject.SetActive(true);
		}
	}
	
	public void AddValueToNode(int v){
		Node node = tSelected.GetComponent<Node>();
		node.AddValue(v);

	}

	public void SetValueToNode(int v){
		Node node = tSelected.GetComponent<Node>();
		node.SetValue(""+v);
	}
	
	public void DestroyChilds(){
		containVertical.DestroyChilds(nNodes);
	}

	
	void PrintGraph(){
		string toPrint = "";
		for (int i = 0; i < nNodes; i++) {
			for (int j = 0; j < nNodes; j++) {
				toPrint += (string.Format("{0} ", grafo[i, j]));
			}
			toPrint +=("\n");
		}
		Debug.Log(toPrint);
	}

	void PaintWhiteSquares(){
		List<Node> nodes = GetComponentsInChildren<Node>(true).ToList();
		int i =0 ,j= 0;		
		foreach (Node node in nodes){
			node.GetComponentInChildren<Image>().color = Color.white;
		}
	}

	void PaintStepSquares(int d){

		List<Node> nodes = GetComponentsInChildren<Node>(true).ToList();
		int i =0 ,j= 0;		
		foreach (Node node in nodes){
			if (i == d || j == d){
				node.GetComponentInChildren<Image>().color = Color.yellow;
			}		
			i++;
			if ((i % nNodes) == 0){
				i = 0;
				j++;
			}
			i= (i%nNodes) ==0 ? 0: i;
		}
	}


}
