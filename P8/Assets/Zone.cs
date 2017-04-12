using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Zone : IEquatable<Zone> {
	private int latitude_index;
	private int longitude_index;

	Zone(int latitude_index, int longitude_index){
		this.latitude_index = latitude_index;
		this.longitude_index = longitude_index;
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

	public override bool Equals(Zone z){
		
		return z.getLatitudeIndex == this.getLatitudeIndex && z.getLongitudeIndex == this.getLongitudeIndex;

	}
}
