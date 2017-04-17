using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Internet connection helper.
/// Code is taken from http://answers.unity3d.com/questions/567497/how-to-100-check-internet-availability.html
/// </summary>
public class InternetConnectionHelper {
	/// <summary>
	/// Checks the internet connection.
	/// </summary>
	/// <returns>A co-routine that can be used to check if device is connected to the internet.
	/// The action returns true if and only if the device has internet connection</returns>
	/// <param name="action">Action to check for internet connction with.</param>
	public static IEnumerator CheckInternetConnection(Action<bool> action){
		WWW www = new WWW(Constants.InternetConnectionCheckWebsite);
		yield return www;
		if (www.error != null) {
			action (false);
		} else {
			action (true);
		}
	} 
}
