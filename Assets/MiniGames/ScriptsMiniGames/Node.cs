using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.Build.AssetBundle;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour{
	int ownX;
	int ownY;
	public int id;
	public Text mText;
	public Image mImg;
	public  Sprite def;
	public  Sprite ball;
	public  Sprite enemy;
	public  Sprite target;
	public  Sprite press;
	public  Sprite intersected;
	private bool imBallOrTarget;
	bool pressed;
	Shadow shadow;
	
	public void SetAxis(int _x, int _y, int _id){
		ownX = _x;
		ownY = _y;
		id = _id;
		shadow = GetComponentInChildren<Shadow>();
		// mText.text = ownX+" , "+ownY+", id:  "+id;
	}

	bool HasUpNode(){
		return (ownX != 0);
	}
	
	bool HasDownNode(int side){
		return (ownX != side-1);
	}

	bool HasLeftNode(){
		return (ownY != 0);
	}
	
	bool HasRigthNode(int side){
		return (ownY != side-1);
	}

	public void PutSpriteDefault(){
		mImg.color =  Color.white;
		shadow.enabled = false;
		mImg.sprite = def;
		pressed = false;
	}
	
	public void PutSpriteBall(){
		mImg.sprite = ball;
		pressed = false;
	}
	
	public void PutSpriteEnemy(bool intersect){
		mImg.sprite = intersect ? intersected: enemy;
		pressed = false;
	}
	
	public void PutSpriteTarget(){
		mImg.sprite = target;
		pressed = false;
	}
	
	public void PutSpritePressed(){
		shadow.enabled = true;
		mImg.color = new Color(1f,0.4f,0f,1f);
		mImg.sprite = def;
		pressed = true;
	}

	public bool IsPressed(){
		return pressed;
	}

	public List<int> GetNeighbours(int side, List<int> enemies){
		List<int> result = new List<int>();
		if (HasUpNode()			&& ! enemies.Contains (id-side) 	)
			result.Add(id-side);
		if (HasDownNode(side)	&& ! enemies.Contains (id+side)  )
			result.Add(id+side);
		if (HasRigthNode(side)		&& ! enemies.Contains (id+1)		)
			result.Add(id+1);
		if (HasLeftNode()	&& ! enemies.Contains (id-1) 	)
			result.Add(id-1);
		return result;

	}	
	
	public void Touch(){	
		MiniGame1.main.EvalTouchedNode(id);
	}

	public void PutText( string _t){
		mText.text = _t;
	}
	
}
