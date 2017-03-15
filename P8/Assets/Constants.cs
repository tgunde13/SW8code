using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants {
	// Pattern from https://www.codeproject.com/kb/recipes/emailregexvalidator.aspx
	public const string MatchEmailPattern = 
		@"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
     + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
     + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
     + @"([a-zA-Z0-9]+[\w-]+\.)+[a-zA-Z]{1}[a-zA-Z0-9-]{1,23})$";
	public const int MinimumPasswordLength = 6;

	//Scenes
	public const string LoginSceneName = "Login";
	public const string MapSceneName = "Map";

	//Firebase path
	public const string FirebaseUrl = "https://league-of-locations-26618308.firebaseio.com";

	//Facebook permission strings
	public const string Profile ="public_profile"; 
	public const string Email ="email";
	public const string Friends = "user_friends";

	//JSON file strings
	public const string LocaleDaDk = "da-DK";
	public const string LocaleEnGb = "en-GB";
}
