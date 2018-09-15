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
			Player.LocalAlias = "BiplaneDuck";
		}
		else if ( Regex.IsMatch (alias.ToUpper (), BIPLANE_REGEX) )
		{
			Player.LocalAlias = "Asshat";
		}
		else
		{
			Player.LocalAlias = alias;
		}

		m_Alias = alias;

		SaveAlias ();
	}

	private string LoadAlias ()
	{
		return PlayerPrefs.GetString (PlayerPrefsKeys.User_Alias, GetCurrentName ());
	}

	private void SaveAlias ()
	{
		PlayerPrefs.SetString (PlayerPrefsKeys.User_Alias, m_Alias);
		PlayerPrefs.Save ();
	}

	private string GetCurrentName ()
	{
		return string.IsNullOrWhiteSpace (Player.LocalAlias) ? GetRandomName () : Player.LocalAlias;
	}

	private string GetRandomName ()
	{
		return "Player" + Random.Range (10000, 99999);
	}
}
