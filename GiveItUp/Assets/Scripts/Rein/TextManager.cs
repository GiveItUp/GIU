using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;
 
/// <summary>
/// TextManager V2
/// Inspired by http://forum.unity3d.com/threads/35617-TextManager-Localization-Script
/// 
/// Reads text files in the 'Assets\Resources' directory into a dictionary. The text file 
/// consists of one line that has the key name, a space and the actual text to display.
/// 
/// Example:
/// 
/// Assume you have a text file called English.txt in Assets\Resources\Languages
/// The file looks like this:
/// 
/// hello Hello and welcome!
/// goodbye Goodbye and thanks for playing!
/// 
/// In Unity code you have to set the Language property with the English.txt asset once:
/// TextManager.LoadResource = "Languages\English";
/// 
/// Then you can retrieve text by calling:
/// TextManager.GetText("hello");
/// This will return a string containing "Hello and welcome!"
/// </summary>
public class TextManager : ScriptableObject
{
		private static readonly IDictionary<string, string> TextTable = new Dictionary<string, string> ();
		private static TextAsset _asset;
		private static bool initialised;

		public static TextAsset Language {
				set {
						_asset = value;
						//Debug.Log (_asset);
						if (_asset == null) {
								Debug.LogError (String.Format ("No valid asset file."));
						} else {
								LoadLanguage (_asset);
						}
				}
		}
 
		public  static void Init ()
		{
			//	Debug.Log (Application.systemLanguage.ToString ());
				String language = Application.systemLanguage.ToString ();
				if (language != "English" && language != "French" && language != "German" && language != "Spanish" && language != "Portuguese" && language != "Chinese")
						language = "English";
				LoadResource = language;
		}
		/// <summary>
		/// Load a asset by its AssetName.
		/// </summary>
		/// <remarks>
		/// The text file must be located within 'Resources' subfolder in Unity ('Assets\Resources' in Visual Studio).
		/// </remarks>
		public static String LoadResource {
				set {
						Language = (TextAsset)Resources.Load (value, typeof(TextAsset));
				}
		}
 
		private TextManager ()
		{
		}
 
		private static void LoadLanguage (TextAsset asset)
		{
				TextTable.Clear ();
				//Debug.Log ("Language reading");
				int lineNumber = 1;
		try{
				using (StringReader reader = new StringReader(asset.text)) {
						string line;
						while ((line = reader.ReadLine()) != null) {
								string[] lineElements = line.Trim ().Split ('#');
								if (lineElements.Length > 1) {
										string key = lineElements [0];
										string val = lineElements [1];
										if (key != null && val != null) {
												//string loweredKey = key.ToLower();
												if (TextTable.ContainsKey (key)) {
														Debug.LogWarning (string.Format (
								"Duplicate key '{1}' in language file '{0}' detected.", TextTable [key], key));
												} else {
														TextTable.Add (key, val);
														//Debug.Log(key);
												}
										}
								} else {
										Debug.LogWarning (string.Format (
                         "Format error in language file '{0}' on line {1}. Each line must be of format: key value", 
                          asset.text, lineNumber));
								}
 
								lineNumber++;
						}
				}
		}catch(Exception ex)
		{
			Debug.Log(ex.Message);
		}
		//Debug.Log ("Language reading end");
		}
 
		public static string Get (string key)
		{
				if (TextTable.Count == 0) {
						Init ();
				}
				//string loweredKey = key.ToLower();
				//Debug.Log (key);
				string result = string.Format ("Couldn't find key '{0}' in dictionary.", key);

				key = key.Replace ("\n", "_");
				//Debug.Log (key.Length);
				if (TextTable.ContainsKey (key)) {
						result = TextTable [key];
				} else {
						Debug.LogError (result);
				}
//		Debug.Log(result.Contains("_"));
				result = result.Replace ("_", "\n");
				result = result.Replace ("%%", "#");
//		Debug.Log(result);
				return result;
		}
}