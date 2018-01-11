using UnityEngine;
using System.Collections;

public class Singleton<Instance> : MonoBehaviour where Instance:Singleton<Instance> {

	public static Instance main;
	public bool isPersistant;

	public virtual void MyAwake(){}

	public virtual void Awake()
	{
		if (isPersistant) {
			if (!main) {
				main = this as Instance;
			} else {
				DestroyObject (gameObject);
			}

			DontDestroyOnLoad (gameObject);
		} else {
			main = this as Instance;
		}

		StartCoroutine (WaitAwake ());
	}

	IEnumerator WaitAwake()
	{
		yield return 0;

		MyAwake ();
	}
}
