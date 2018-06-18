using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using TMPro;

public class UserAliasInput : MonoBehaviour
{
	private const string BIPLANE_REGEX = "BIPLANEDUCK";

	private TMP_InputField m_Input;
	private string m_Alias;

	private void Awake ()
	{
		m_Input = GetComponent<TMP_InputField> ();
	}

	private void OnEnable ()
	{
		SetAlias(LoadAlias ());

		m_Input.text = m_Alias;
	}

	private void OnDisable ()
	{
		SaveAlias ();
	}

	public void SetAlias (string alias)
	{
		if ( string.IsNullOrWhiteSpace (alias) || alias.Length > 20 )
		{
			m_Input.text = m_Alias;
			return;
		}
		else if ( alias == "AsDfGhJkL;" )
		{
			Player.LocalName = "BiplaneDuck";
		}
		else if ( Regex.IsMatch (alias.ToUpper (), BIPLANE_REGEX) )
		{
			Player.LocalName = "Asshat";
		}
		else
		{
			Player.LocalName = alias;
		}

		m_Alias = alias;

		SaveAlias ();
	}

	private string LoadAlias ()
	{
		return PlayerPrefs.GetString (PlayerPrefsKeys.User_Alias_Pref_Key, GetCurrentName ());
	}

	private void SaveAlias ()
	{
		PlayerPrefs.SetString (PlayerPrefsKeys.User_Alias_Pref_Key, m_Alias);
		PlayerPrefs.Save ();
	}

	private string GetCurrentName ()
	{
		return string.IsNullOrWhiteSpace (Player.LocalName) ? GetRandomName () : Player.LocalName;
	}

	private string GetRandomName ()
	{
		return "Player" + Random.Range (10000, 99999);
	}
}
