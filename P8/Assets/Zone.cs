using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Firebase.Database;

public class Zone {
	private int latitudeIndex;
	private int longitudeIndex;
	private DatabaseReference minionRef;
	private List<Squad> squads = new List<Squad>();
	private SpriteController spriteController;

	public Zone(int latitudeIndex, int longitudeIndex, SpriteController spriteController){
		this.latitudeIndex = latitudeIndex;
		this.longitudeIndex = longitudeIndex;
		this.spriteController = spriteController;
		this.minionRef = null;
	}

	public Zone(int latitudeIndex, int longitudeIndex, SpriteController spriteController, DatabaseReference minionRef){
		this.latitudeIndex = latitudeIndex;
		this.longitudeIndex = longitudeIndex;
		this.spriteController = spriteController;
		this.minionRef = minionRef;
		this.minionRef.ChildAdded += HandleChildAdded;
		this.minionRef.ChildRemoved += HandleChildRemoved;
	}

	~Zone(){
		this.minionRef.ChildAdded -= HandleChildAdded;
		this.minionRef.ChildRemoved -= HandleChildRemoved;
		foreach (Squad s in squads) {
			spriteController.RemoveSprite (s.GetKey());
		}
	}

	/// <summary>
	/// Event handler for child added.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="args">Arguments.</param>
	void HandleChildAdded(object sender, ChildChangedEventArgs args){
		long maxHealth = (long)args.Snapshot.Child ("health").GetValue (false);
		double latitude = (double)args.Snapshot.Child ("lat").GetValue (false);
		double longitude = (double)args.Snapshot.Child ("lon").GetValue (false);
		string key = (string)args.Snapshot.Key;
		string name = (string)args.Snapshot.Child ("name").GetValue (false);
		long level = (long)args.Snapshot.Child ("level").GetValue (false);
		long power = (long)args.Snapshot.Child ("power").GetValue (false);
		long speed = (long)args.Snapshot.Child ("speed").GetValue (false);
		long size = (long)args.Snapshot.Child ("size").GetValue (false);

		Squad s = new Squad ((int) level, (int) size, name, (int) maxHealth, (int) power, (int) speed, latitude, longitude, key);
		squads.Add(s);
		spriteController.AddSprite(s);
	}


	/// <summary>
	/// Event handler for child removed.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="args">Arguments.</param>
	void HandleChildRemoved(object sender, ChildChangedEventArgs args){
		string key = (string) args.Snapshot.Key;

		spriteController.RemoveSprite (key);

		foreach (Squad s in squads) {
			if (s.GetKey() == key) {
				squads.Remove (s);
				break;
			}
		}
	}

	/// <summary>
	/// Returns the latitude index of the zone.	
	/// </summary>
	/// <returns>The latitude index.</returns>
	public int GetLatitudeIndex(){
		return latitudeIndex;
	}

	/// <summary>
	/// Returns the longitude index of the zone.
	/// </summary>
	/// <returns>The longitude index.</returns>
	public int GetLongitudeIndex(){
		return longitudeIndex;
	}

	/// <summary>
	/// Sets the minion reference.
	/// </summary>
	/// <param name="minion_ref">Minion reference.</param>
	public void SetMinionRef(DatabaseReference minion_ref){
		this.minionRef = minion_ref;
		this.minionRef.ChildAdded += HandleChildAdded;
	}

	/// <summary>
	/// Gets the minion reference.
	/// </summary>
	/// <returns>The minion reference.</returns>
	public DatabaseReference GetMinionRef(){
		return minionRef;
	}

	/// <summary>
	/// Determines whether the specified <see cref="Zone"/> is equal to the current <see cref="Zone"/>, based on the latitude and longitude index.
	/// </summary>
	/// <param name="z">The <see cref="Zone"/> to compare with the current <see cref="Zone"/>.</param>
	/// <returns><c>true</c> if the specified <see cref="Zone"/> is equal to the current <see cref="Zone"/>; otherwise, <c>false</c>.</returns>
	public override bool Equals(object z){
		Zone zone = z as Zone;
		return (zone.GetLatitudeIndex() == this.GetLatitudeIndex()) && (zone.GetLongitudeIndex() == this.GetLongitudeIndex());
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
		return String.Format ("Latitude index: {0}, Longitude index: {1}", GetLatitudeIndex(), GetLongitudeIndex());
	}
}
