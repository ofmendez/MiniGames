using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum EState {_ShowEnemies, _ShowBallAndTs, _DrawingPath, _DonePath }

public class MiniGame1 : Singleton<MiniGame1> {
	
	public GameObject boardPrefab;
	public GameObject boardHolder;
	EState actualState;
	List<int> enemies = new List<int>(); 
	Dictionary<int, List<int>> graph = new Dictionary<int, List<int>>();
	List<Node> nodes = new List<Node>();
	GameObject tempBoard;
	int side;
	Node ball;
	Node target_origin;
	float timeToShowEnemies = 0.5f;
	// public Toggle tSelected;

 	void Start(){
 		ResetGame(4);
 	}

 	void Update() {
        if (Input.touchCount > 0 && actualState == EState._DrawingPath ) {
			
        }
    }

	// SETUP
	public void ResetGame(int n){
		actualState = EState._ShowEnemies;
		side = n;
		InstantiateElements();
		SetNamesAndAxis();
		SetEnemies();
		MakeGraph();
		StartCoroutine(DisplayEnemies());
		DisplayLinks();
	}

	void InstantiateElements(){
		tempBoard = Instantiate(boardPrefab, boardHolder.transform, false);
		tempBoard.GetComponent<Contain>().CreateNodes(side);
		nodes = tempBoard.GetComponentsInChildren<Node>().ToList();
	}




	public void SetNamesAndAxis(){
		int x = 0;
		int y = 0;
		foreach (Node node in nodes){
			node.SetAxis( x, y++, side*x +y);
			if (y % side != 0) continue;
			y = 0;
			x++;
		}
	}
	
	private void SetShowBallAndTsState()	{
		actualState = EState._ShowBallAndTs;
		CleanSprites(true);
		do{ SetBallAndtarget_origin();}
		while (!IsConnected(ball.id, target_origin.id));
		DisplayBallAndtarget_origin();		
	}

	public void SetEnemies(){
	    for (int i=0; i< side-1; i++) { // NO asignar màs que  n-1 enemigos
	        int number;
	        do number = Random.Range(1, side*side+1);
	        while (enemies.Contains(number));
		    enemies.Add(number);
	    }
	}

	void SetBallAndtarget_origin(){	    
		int number=1;
		do number = Random.Range(1, side*side+1);
		while (enemies.Contains(number) || (number%side != 0));
		target_origin = nodes [number-1];
		do number = Random.Range(1, side*side+1);
		while (enemies.Contains(number) || number%side !=1 || number == target_origin.id);
		ball =  nodes [number-1];
	}


// CONTROL SPRITES
	IEnumerator DisplayEnemies(){
		foreach(Node node in nodes){
			if(enemies.Contains(node.id))
				node.PutSpriteEnemy();
		}
		yield return new WaitForSeconds(timeToShowEnemies);
		SetShowBallAndTsState();
	}
	
	private void DisplayBallAndtarget_origin()	{
		ball.PutSpriteBall();
		target_origin.PutSpriteTarget();
		// for (int i = 0; i < side*side; i++){
		// 	Debug.Log("Esta pendejada: " +i+ " , "+ nodes[i].id);
		// }
	}
	
	private void CleanSprites(bool cleanBallAndTarget){
		foreach(Node node in nodes){
			if (!cleanBallAndTarget && (node == ball || node == target_origin)) continue;
			node.PutSpriteDefault();
		}
	}

// AUXILIARES
	
	private void MakeGraph(){
		foreach (Node node in nodes){
			graph[node.id] = new List<int>();
			if (enemies.Contains(node.id)) continue;
			foreach (int n in node.GetNeighbours(side, enemies)){
				graph[node.id].Add(n);				
			}                                                                          
		}
	}
	
	int DFS(int node, Dictionary<int, string> color){
		color[node] = "gray";
		int total_marked = 1;
		foreach(int neighbor in graph[node]){
	        if (color[neighbor] == "white")
	            total_marked += DFS( neighbor, color);
		}
		color[node] = "black";
	    return total_marked;
	}

    bool IsConnected(int _p1,int _p2){
		Dictionary<int, string> color = new Dictionary<int, string>();
		foreach (int node in graph.Keys ) {
			color[node] = "white";
		}
		DFS(_p1,color);	
    	return color[_p2] == "black";
    }

	private void DisplayLinks(){
		foreach (Node node in nodes){
			string t = "("+node.id+")";	
			node.PutText(t);
			if (enemies.Contains(node.id)) continue;
			foreach (int n in graph[node.id]){
				t += ", " + n;
			}   
			node.PutText(t);
		}
	}

	// private void ShowThisStage(int[,] distance){
	// 	List<Node> nodes = GetComponentsInChildren<Node>(true).ToList();
	// 	int i =0,j = 0;
	// 	foreach (Node node in nodes){
	// 		node.SetValue(""+distance[j,i]);
	// 		i++;
	// 		if ((i % side) == 0){
	// 			i = 0;
	// 			j++;
	// 		}
	// 		i= (i%side) ==0 ? 0: i;
	// 	}
	// }


	// public void ApplyAndBeginProcess(){
		// buttonsSums.SetActive(false);
		// buttonsPlus_minus.SetActive(false);
		// buttonsAdvance.SetActive(true);
		// buttonBackSteps.SetActive(true);
		// GetInitValues();
		// buttonsAdvance.GetComponentsInChildren<Button>()[0].interactable = false;
		// buttonsAdvance.GetComponentsInChildren<Button>()[1].interactable = true;
		// ShowFloydUntil(0);
	// }

	// public void NextFloyd(){
	// 	if (actualStage < side){
	// 		ShowFloydUntil(actualStage+1);			
	// 	}
	// 	// buttonsAdvance.GetComponentsInChildren<Button>()[0].interactable = (actualStage>0);
	// 	// buttonsAdvance.GetComponentsInChildren<Button>()[1].interactable =  (actualStage<side);
	// }

	// public void PreviousFloyd(){
	// 	if (actualStage > 0){
	// 		ShowFloydUntil(actualStage-1);			
	// 	}
	// 	// buttonsAdvance.GetComponentsInChildren<Button>()[0].interactable = (actualStage>0);
	// 	// buttonsAdvance.GetComponentsInChildren<Button>()[1].interactable =  (actualStage<side);
	// }
	
	// void ShowFloydUntil(int i){
	// 	actualStage = i;
	// 	//title.text = "D (" + i + ")";
	// 	//PaintWhiteSquares();
	// 	ExecuteFloydUntil(i);
	// 	//PaintStepSquares(i-1);
	// }


	// void ExecuteFloydUntil(int target_origin){
	// 	int[,] distance = new int[side, side];

	// 	for (int i = 0; i < side; ++i)
	// 	for (int j = 0; j < side; ++j)
	// 		distance[i, j] = grafo[i, j];

	// 	for (int k = 0; k < target_origin; ++k){
	// 		for (int i = 0; i < side; ++i){
	// 			for (int j = 0; j < side; ++j){
	// 				if (distance[i, k] + distance[k, j] < distance[i, j]){
	// 					distance[i, j] = distance[i, k] + distance[k, j];
	// 					if (distance[i, k] == 999999 || distance[k, j] == 999999)
	// 						distance[i, j] = 999999;
	// 				}
	// 			}
	// 		}
	// 	}
	// 	ShowThisStage(distance);
	// }

		// void GetInitValues(){
	// 	List<Node> nodes = GetComponentsInChildren<Node>(true).ToList();
	// 	int i =0 ,j= 0;		
	// 	foreach (Node node in nodes){
	// 		grafo[j,i] = node.GetValue();
	// 		i++;
	// 		if ((i % side) == 0){
	// 			i = 0;
	// 			j++;
	// 		}
	// 		i= (i%side) ==0 ? 0: i;
	// 	}
	// }
	

	// void InitializeValues(){
		
	// 	int j = 0;
	// 	List<Toggle> toggles = GetComponentsInChildren<Toggle>().ToList();
	// 	foreach (Toggle toggle in toggles){
	// 		if (j%(side+1) >0 && j>side){
	// 			toggle.GetComponentInChildren<Text>().color = Color.black;
	// 			toggle.GetComponentInChildren<Node>().SetValue( j%(side+2) ==0?"0":"INF");
	// 		}
	// 		j++;
	// 	}
	// }
	
	//AUX NODES
/*
	public void ShowPath(){
		PaintWhiteSquares();
		int[,] distance = new int[side, side];
		char[,] path = new char[side, side];
		char c = 'A';
		for (int i = 0; i < side; ++i){
			for (int j = 0; j < side; ++j){
				path[i, j] = c;
				distance[i, j] = grafo[i, j];
				c++;
			}
			c = 'A';
		}
		c = 'A';
		for (int k = 0; k < side; ++k){
			for (int i = 0; i < side; ++i){
				for (int j = 0; j < side; ++j){
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
			if ((i % side) == 0){
				i = 0;
				j++;
			}
			i= (i%side) ==0 ? 0: i;
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
		containVertical.DestroyChilds(side);
	}

	
	void PrintGraph(){
		string toPrint = "";
		for (int i = 0; i < side; i++) {
			for (int j = 0; j < side; j++) {
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
			if ((i % side) == 0){
				i = 0;
				j++;
			}
			i= (i%side) ==0 ? 0: i;
		}
	}
*/

}
