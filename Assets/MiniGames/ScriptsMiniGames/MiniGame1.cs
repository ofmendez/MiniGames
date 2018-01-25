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
	public GameObject winScreen;
	public GameObject looseScreen;
	public bool onlyBeginWithBall;
	Dictionary<int, List<int>> graph = new Dictionary<int, List<int>>();
	GameObject tempBoard;
	EState actualState;
	List<int> touchedNodes = new List<int>(); 
	List<int> enemies = new List<int>(); 
	List<Node> nodes = new List<Node>();
	int side;
	Node ball;
	Node target_origin;
	Node lastTouched;
	float timeToShowEnemies = 2.5f;
	string firstTouched;

 	public void LaunchLevel(Text ns){ // Será innecesario, llamar directamente a ResetGame ( ->lados del cuadrado <-)
		int n; int.TryParse(ns.text, out n);
		if (n>2 && n <9)
			ResetGame(n);
 	}

 	void Update() {
		if (Input.GetMouseButtonUp (0) && actualState == EState._DrawingPath) 
	        DisplayBallAndtarget_origin();
    }


    public void EvalTouchedNode(int touchedId){
    	if(actualState == EState._ShowBallAndTs && StartCorrectly(touchedId) ){
    		BeginDrawPath(touchedId);
    	}
    	if(actualState == EState._DrawingPath && CanBePressed(touchedId)  ){
    		PressNewNode(touchedId);
    	}
    }


    void PressNewNode(int touchedId){
    	if(EndCondition(touchedId)){
    		EvalWinOrLoose(touchedId);
    	}
		nodes[touchedId-1].PutSpritePressed();
		lastTouched = nodes[touchedId-1];
	    touchedNodes.Add(lastTouched.id);
    }


    void BeginDrawPath(int touchedId){
		actualState = EState._DrawingPath;
    	firstTouched = nodes[touchedId-1] == ball ? "ball" :"target_origin";
		nodes[touchedId-1].PutSpritePressed();
		lastTouched = nodes[touchedId-1];
	    touchedNodes.Add(lastTouched.id);
    }

    void EvalWinOrLoose(int touchedId){
    	if (touchedNodes.Intersect(enemies).Any())
    		LooseGame();
	    else
	    	WinGame();
	    actualState = EState._DonePath;
	    DisplayEnemies();
    }

	void WinGame(){
		winScreen.SetActive(true);   ///GANO EL JUEGO		
	}

	void LooseGame(){
    	looseScreen.SetActive(true); ///PERDIO EL JUEGO				
	}


	// SETUP
	public void ResetGame(int n){
		ClearElements();
		actualState = EState._ShowEnemies;		
		side = n;
		InstantiateElements();
		SetNamesAndAxis();
		SetEnemies();
		MakeGraph();
		StartCoroutine(DisplayEnemiesForAWhile());
		// DisplayLinks();
	}

	public void ClearElements(){
		winScreen.SetActive(false);
		looseScreen.SetActive(false);
		if(tempBoard) Destroy(tempBoard);
		enemies.Clear();
		touchedNodes.Clear();
		nodes.Clear();
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
	
	private void SetBallAndTarget()	{
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
	IEnumerator DisplayEnemiesForAWhile(){
		DisplayEnemies();
		yield return new WaitForSeconds(timeToShowEnemies);
		SetBallAndTarget();
	}

	void DisplayEnemies(){
		bool muustShowIntersected =  false;
		foreach(Node node in nodes){
			muustShowIntersected = (actualState == EState._DonePath && touchedNodes.Contains(node.id));
			if(enemies.Contains(node.id))
				node.PutSpriteEnemy(muustShowIntersected);
		}
	}
	
	private void DisplayBallAndtarget_origin()	{
		actualState = EState._ShowBallAndTs;
		touchedNodes.Clear();
		CleanSprites(true);
		ball.PutSpriteBall();
		target_origin.PutSpriteTarget();
		firstTouched = "";
	}
	
	private void CleanSprites(bool cleanBallAndTarget){
		foreach(Node node in nodes){
			if (!cleanBallAndTarget && (node == ball || node == target_origin)) continue;
			node.PutSpriteDefault();
		}
	}

// AUXILIARES
    bool  StartCorrectly(int touchedId){
    	return (onlyBeginWithBall && touchedId == ball.id) || (!onlyBeginWithBall && (touchedId == ball.id || touchedId == target_origin.id));
    }
    bool EndCondition(int touchedId){
    	return (firstTouched == "ball" && nodes[touchedId-1] == target_origin) || (firstTouched == "target_origin" && nodes[touchedId-1] == ball);
    }
    bool CanBePressed(int touchedId){
    	return lastTouched.GetNeighbours(side, new List<int>() ).Contains(touchedId) && ! nodes[touchedId-1].IsPressed();
    }

	private void MakeGraph(){
		foreach (Node node in nodes){
			graph[node.id] = new List<int>();
			if (enemies.Contains(node.id)) continue;
			foreach (int n in node.GetNeighbours(side, enemies)){
				graph[node.id].Add(n);				
			}                                                                          
		}
	}
	
	void DFS(int node, Dictionary<int, string> color){
		color[node] = "gray";
		foreach(int neighbor in graph[node]){
	        if (color[neighbor] == "white")
	            DFS( neighbor, color);
		}
		color[node] = "black";
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
}
