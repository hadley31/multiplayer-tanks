using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldSaveValue : MonoBehaviour
{
	public InputField inputField;
	public string saveName;

	protected void OnEnable ()
	{
		LoadString ();
		inputField.onEndEdit.AddListener (SaveString);
	}

	protected void OnDisable ()
	{
		inputField.onEndEdit.RemoveListener (SaveString);
	}

	public void SaveString (string s)
	{
		PlayerPrefs.SetString (saveName, s);
		PlayerPrefs.Save ();
	}

	public void LoadString ()
	{
		inputField.text = PlayerPrefs.GetString (saveName);
	}
}
