using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Firebase.Database;

public class Zone {
	private int latitude_index;
	private int longitude_index;
	private DatabaseReference minion_ref;
	private List<Squad> squads = new List<Squad>();
	private SpriteController sprite_controller;

	public Zone(int latitude_index, int longitude_index, SpriteController sprite_controller){
		this.latitude_index = latitude_index;
		this.longitude_index = longitude_index;
		this.sprite_controller = sprite_controller;
		this.minion_ref = null;
	}

	public Zone(int latitude_index, int longitude_index, SpriteController sprite_controller, DatabaseReference minion_ref){
		this.latitude_index = latitude_index;
		this.longitude_index = longitude_index;
		this.sprite_controller = sprite_controller;
		this.minion_ref = minion_ref;
		this.minion_ref.ChildAdded += handleChildAdded;
	}

	void handleChildAdded(object sender, ChildChangedEventArgs args){
		long maxHealth = (long)args.Snapshot.Child ("maxHealth").GetValue (false);
		Squad s = new Squad (1, "Change me", (int) maxHealth, ((double) args.Snapshot.Child ("lat").GetValue (false)), ((double) args.Snapshot.Child ("lon").GetValue (false)));
		squads.Add(s);
		sprite_controller.addSprite(s);
	}

	/// <summary>
	/// Returns the latitude index of the zone.	
	/// </summary>
	/// <returns>The latitude index.</returns>
	public int getLatitudeIndex(){
		return latitude_index;
	}

	/// <summary>
	/// Returns the longitude index of the zone.
	/// </summary>
	/// <returns>The longitude index.</returns>
	public int getLongitudeIndex(){
		return longitude_index;
	}

	/// <summary>
	/// Sets the minion reference.
	/// </summary>
	/// <param name="minion_ref">Minion reference.</param>
	public void setMinionRef(DatabaseReference minion_ref){
		this.minion_ref = minion_ref;
		this.minion_ref.ChildAdded += handleChildAdded;
	}

	/// <summary>
	/// Gets the minion reference.
	/// </summary>
	/// <returns>The minion reference.</returns>
	public DatabaseReference getMinionRef(){
		return minion_ref;
	}

	public void addSquad(Squad squad){
		squads.Add (squad);
	}

	public override bool Equals(object z){
		Zone zone = z as Zone;
		return (zone.getLatitudeIndex() == this.getLatitudeIndex()) && (zone.getLongitudeIndex() == this.getLongitudeIndex());
	}

	public override int GetHashCode()
	{
		return (base.GetHashCode ());
	}

	public override String ToString(){
		return String.Format ("Latitude index: {0}, Longitude index: {1}", getLatitudeIndex(), getLongitudeIndex());
	}
}
