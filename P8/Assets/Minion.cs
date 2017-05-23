using System;

public class Minion{
	private string key;
	private int health;
	private int level;
	private string name;
	private int power;
	private int speed;
	private string type;
	private int xp;

	public Minion (string key, int health, int level, string name, int power, int speed, string type, int xp){
		this.key = key;
		this.health = health;
		this.level = level;
		this.name = name;
		this.power = power;
		this.speed = speed;
		this.type = type;
		this.xp = xp;
	}

	public string GetKey(){
		return key;
	}

	public int GetHealth(){
		return health;
	}

	public void SetHealth(int health){
		this.health = health;
	}

	public int GetLevel(){
		return level;
	}

	public string GetName(){
		return name;
	}

	public int GetPower(){
		return power;
	}

	public void SetPower(int power){
		this.power = power;
	}

	public int GetSpeed(){
		return speed;
	}

	public void SetSpeed(int speed){
		this.speed = speed;
	}

	public string GetType(){
		return type;
	}

	public int GetXp(){
		return xp;
	}

	public override string ToString ()
	{
		return string.Format ("Health: {0} \nLevel: {1} \nName: {2} \nPower: {3} \nSpeed: {4} \nType: {5} \nXP: {6}",
			health, level, name, power, speed, type, xp);
	}
}