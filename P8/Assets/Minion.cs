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

	public string getKey(){
		return key;
	}

	public int getHealth(){
		return health;
	}

	public void setHealth(int health){
		this.health = health;
	}

	public int getLevel(){
		return level;
	}

	public string getName(){
		return name;
	}

	public int getPower(){
		return power;
	}

	public void setPower(int power){
		this.power = power;
	}

	public int getSpeed(){
		return speed;
	}

	public void setSpeed(int speed){
		this.speed = speed;
	}

	public string getType(){
		return type;
	}

	public int getXp(){
		return xp;
	}

	public override string ToString ()
	{
		return string.Format ("Health: {0} \nLevel: {1} \nName: {2} \nPower: {3} \nSpeed: {4} \nType: {5} \nXP: {6}",
			health, level, name, power, speed, type, xp);
	}
}