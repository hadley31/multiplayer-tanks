using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserSetting<T>
{
	public string Name
	{
		get;
	}

	public object Value
	{
		get;
		set;
	}

	public object DefaultValue
	{
		get;
	}

	public string PlayerPrefKey
	{
		get;
	}

	public UserSetting (string name, string key, object defaultValue)
	{
		this.Name = name;
		this.PlayerPrefKey = key;
		this.DefaultValue = defaultValue;
	}

	public string GetString ()
	{
		return PlayerPrefs.GetString (PlayerPrefKey);
	}

	public void SaveString ()
	{
	//	PlayerPrefs.SetString (PlayerPrefKey, );
	}
}
