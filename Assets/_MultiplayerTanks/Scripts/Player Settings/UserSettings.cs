using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class UserSettings : MonoBehaviour
{
	const string Settings_File_Name = "user_settings.dat";

	public static readonly string Settings_Path = Path.Combine (Application.persistentDataPath, Settings_File_Name);

	public static void Save ()
	{
		using ( FileStream stream = File.Open (Settings_Path, FileMode.OpenOrCreate) )
		{
			BinaryFormatter bf = new BinaryFormatter ();

			bf.Serialize (stream, null);
		}
	}

	public static void Load ()
	{
		if (File.Exists (Settings_Path) == false)
		{
			return;
		}

		using ( FileStream stream = File.Open (Settings_Path, FileMode.OpenOrCreate) )
		{
			BinaryFormatter bf = new BinaryFormatter ();

			bf.Serialize (stream, null);
		}
	}
}
