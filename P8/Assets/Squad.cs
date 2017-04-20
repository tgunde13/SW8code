using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad {
	private int size;
	private string name;
	private GameObject sprite;
	private int max_health;
	private Vector2 pos;
	private string key;

	public Squad(int size, string name, int max_health, double latitude, double longitude, string key){
		this.size = size;
		this.name = name;
		this.max_health = max_health;
		this.pos = new Vector2 ((float)latitude, (float)longitude);
		this.key = key;
	}

	public string getKey(){
		return key;
	}

	public void setSprite(GameObject sprite){
		this.sprite = sprite;
	}

	public string getName(){
		return name;
	}

	public Vector2 getPos(){
		return pos;
	}

	public int getMaxHealth(){
		return max_health;
	}


}
