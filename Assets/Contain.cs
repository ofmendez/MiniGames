using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.UI;

public class Contain : MonoBehaviour {

	
	public GameObject childPrefab;
	public bool isLeave;
	public bool value;
	private LinkedList<GameObject> childs = new LinkedList<GameObject>();
	public int nNode =0;

	public void CreateNodes(int n){
		if (!isLeave){
			childPrefab.GetComponent<Contain>().CreateNodes(n);
		}
		for (int i = 0; i < n; i++){
			GameObject child = Instantiate(childPrefab, this.gameObject.transform, true);
			childs.AddLast(child);
		}

	}

	public void DestroyChilds(int n){
		if (!isLeave){
			childPrefab.GetComponent<Contain>().DestroyChilds(n);
		}
		for (int i = 0; i < n; i++){
			GameObject o = childs.Last();
			childs.RemoveLast();
			Destroy(o);
		}

	}




}
