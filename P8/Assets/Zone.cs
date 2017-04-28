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
		this.minion_ref.ChildRemoved += handleChildRemoved;
	}

	~Zone(){
		this.minion_ref.ChildAdded -= handleChildAdded;
		this.minion_ref.ChildRemoved -= handleChildRemoved;
		foreach (Squad s in squads) {
			sprite_controller.removeSprite (s.getKey());
		}
	}

	/// <summary>
	/// Event handler for child added.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="args">Arguments.</param>
	void handleChildAdded(object sender, ChildChangedEventArgs args){
		long maxHealth = (long)args.Snapshot.Child ("health").GetValue (false);
		double latitude = (double)args.Snapshot.Child ("lat").GetValue (false);
		double longitude = (double)args.Snapshot.Child ("lon").GetValue (false);
		string key = (string)args.Snapshot.Key;
		string name = (string)args.Snapshot.Child ("name").GetValue (false);
		long level = (long)args.Snapshot.Child ("level").GetValue (false);
		long power = (long)args.Snapshot.Child ("power").GetValue (false);
		long speed = (long)args.Snapshot.Child ("speed").GetValue (false);

		Squad s = new Squad ((int) level, 1, name, (int) maxHealth, (int) power, (int) speed, latitude, longitude, key);
		squads.Add(s);
		sprite_controller.addSprite(s);
	}


	/// <summary>
	/// Event handler for child removed.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="args">Arguments.</param>
	void handleChildRemoved(object sender, ChildChangedEventArgs args){
		string key = (string) args.Snapshot.Key;

		sprite_controller.removeSprite (key);

		foreach (Squad s in squads) {
			if (s.getKey() == key) {
				squads.Remove (s);
				break;
			}
		}
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

	/// <summary>
	/// Determines whether the specified <see cref="Zone"/> is equal to the current <see cref="Zone"/>, based on the latitude and longitude index.
	/// </summary>
	/// <param name="z">The <see cref="Zone"/> to compare with the current <see cref="Zone"/>.</param>
	/// <returns><c>true</c> if the specified <see cref="Zone"/> is equal to the current <see cref="Zone"/>; otherwise, <c>false</c>.</returns>
	public override bool Equals(object z){
		Zone zone = z as Zone;
		return (zone.getLatitudeIndex() == this.getLatitudeIndex()) && (zone.getLongitudeIndex() == this.getLongitudeIndex());
	}

	public override int GetHashCode()
	{
		return (base.GetHashCode ());
	}

	/// <summary>
	/// Returns a string that represents the current object, by latitude and longitude index.
	/// </summary>
	/// <returns>A string that represents the current object.</returns>
	/// <filterpriority>2</filterpriority>
	public override String ToString(){
		return String.Format ("Latitude index: {0}, Longitude index: {1}", getLatitudeIndex(), getLongitudeIndex());
	}
}
