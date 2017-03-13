using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityIndicatorHandler {

	public static void start() {
		#if UNITY_IPHONE
			Handheld.SetActivityIndicatorStyle(iOS.ActivityIndicatorStyle.Gray);
		#elif UNITY_ANDROID
		Handheld.SetActivityIndicatorStyle (AndroidActivityIndicatorStyle.Large);
			Handheld.
		#endif

		Handheld.StartActivityIndicator();
	}

	public static void stop() {
		Handheld.StopActivityIndicator ();
	}
}
