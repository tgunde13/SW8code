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

	string getKey(){
		return key;
	}

	int getHealth(){
		return health;
	}

	int getLevel(){
		return level;
	}

	string getName(){
		return name;
	}

	int getPower(){
		return power;
	}

	int getSpeed(){
		return speed;
	}

	string getType(){
		return type;
	}

	int getXp(){
		return xp;
	}

	public override string ToString ()
	{
		return string.Format ("Key: {0} \n Health: {1} \n Level: {2} \n Name: {3} \n Power: {4} \n Speed: {5} \n Type: {6} \n XP: {7}",
			key, health, level, name, power, speed, type, xp);
	}
}