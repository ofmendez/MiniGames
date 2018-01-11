using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.Build.AssetBundle;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour{
	public int ownX;
	public int ownY;
	public int id;
	public Text mText;
	public Image mImg;
	public  Sprite def;
	public  Sprite ball;
	public  Sprite enemy;
	public  Sprite target;
	public  Sprite pressed;
	private bool imBallOrTarget;
	
	public void SetAxis(int _x, int _y, int _id){
		ownX = _x;
		ownY = _y;
		id = _id;
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
		mImg.sprite = def;
	}
	public void PutSpriteBall(){
		mImg.sprite = ball;
	}
	public void PutSpriteEnemy(){
		mImg.sprite = enemy;
	}
	public void PutSpriteTarget(){
		mImg.sprite = target;
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

	
	public void PrintName(){	
//		Debug.Log("TOCA: "+mText.text);
	}

	public void PutText( string _t){
		mText.text = _t;
	}
	
	// public int GetValue(){	
		// return  value == "INF"? 999999 : Convert.ToInt32(value);
	// }

	public void AddValue(int i){
		// int actual = GetValue();
		// int result = actual == 999999 ? i: actual+i;
		// SetValue(""+result);
	}
}
