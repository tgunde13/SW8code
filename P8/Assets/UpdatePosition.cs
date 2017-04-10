using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpdatePosition : MonoBehaviour {
	
	public DialogPanel error;
	public int frames_before_update = 300;
	public Mapbox.MeshGeneration.MapController mapController;

	int frames_past = 0;
	float current_latitude;
	float current_longitude;

	// Use this for initialization
	void Start () {
		getLocation ();
		getNewMap ();
	}

	void Update () {
		frames_past++;
		//What to do after a given number of frames
		if (frames_past == frames_before_update){
			//New maps are added here if needed
			getLocation();
			getNewMap ();
			frames_past = 0;
		}
	}

	//Sends a request to the mapcontroller with a new location
	void getNewMap(){
		Vector2 pos = new Vector2(current_latitude, current_longitude);
		mapController.Request (pos, 18);
	}

	//Gets the current location from the device
	IEnumerator getLocation(){
		Debug.Log ("GetLocation ½: location block reached");
		//Making sure location is enabled
		if (!Input.location.isEnabledByUser) {
			Debug.Log ("GetLocation 1: Location not enabled");
			yield break;
		}

		//Starting the location service
		Input.location.Start();

		//Wait until service initializes
		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0){
			//Debug.Log ("GetLocation 0: Location not yet retrived waited for {0} secounds", (maxWait + 20));
			yield return new WaitForSeconds(1);
			maxWait--;
		}

		// Service didn't initialize in 20 seconds
		if (maxWait < 1){
			Debug.Log ("GetLocation 2: Location service didn't start");
			error.show("Location service didn't start");
			yield break;
		}

		// Connection has failed
		if (Input.location.status == LocationServiceStatus.Failed) {
			Debug.Log ("GetLocation 3: Unable to determine device location");
			error.show ("Unable to determine device location");
			yield break;
		} else {
			// Access granted and location value could be retrieved
			current_latitude = Input.location.lastData.latitude;
			current_longitude = Input.location.lastData.longitude;

			yield return new WaitForSeconds (3);
			Debug.Log ("Refreshing location");
			Input.location.Stop ();
		}
	}
}
