using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// Preload signing alias.
/// Some code is from http://answers.unity3d.com/questions/757834/publishing-settings-keystore-password-not-saving.html
/// </summary>
[InitializeOnLoad]
public class PreloadSigningAlias {
	static PreloadSigningAlias () {
		PlayerSettings.Android.keystorePass = "jRcz9vp4";
		PlayerSettings.Android.keyaliasName = "key1";
		PlayerSettings.Android.keyaliasPass = "sRAvMweY";
	}
}