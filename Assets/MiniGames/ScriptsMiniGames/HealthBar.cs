using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {

	public  RectTransform healthBar;

	public float currentHealth ;

	int maxHealth = 100;


	void Start () {
		currentHealth = maxHealth;
	}
	
	void Update () {
	    healthBar.sizeDelta = new Vector2(-healthBar.parent.GetComponent<RectTransform>().rect.width*(1-currentHealth/100f),healthBar.sizeDelta.y);
	}

	public void TakeDamage(int amount) {
	    currentHealth -= amount;
	    if (currentHealth <= 0) {
	        currentHealth = 0;
	        MiniGame2.main.GameOver();
	    }
	}
}
