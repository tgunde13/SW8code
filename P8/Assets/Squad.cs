using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad {
	private int size;
	private string name;
	private int max_health;
	private Vector2 pos;
	private string key;
	private int power;
	private int speed;
	private int level;

	public Squad(int level, int size, string name, int max_health, int power, int speed, double latitude, double longitude, string key){
		this.level = level;
		this.size = size;
		this.name = name;
		this.max_health = max_health;
		this.pos = new Vector2 ((float)latitude, (float)longitude);
		this.key = key;
		this.power = power;
		this.speed = speed;
	}

	public string GetKey(){
		return key;
	}

	public string GetName(){
		return name;
	}

	public Vector2 GetPos(){
		return pos;
	}

	public int GetMaxHealth(){
		return max_health;
	}

	public int GetPower(){
		return power;
	}

	public int GetSpeed(){
		return speed;
	}

	public int GetLevel(){
		return level;
	}

	public int GetSize(){
		return size;
	}

}
