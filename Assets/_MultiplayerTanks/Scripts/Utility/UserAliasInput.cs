using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UserAliasInput : MonoBehaviour
{
	const string User_Alias_Pref_Key = "ua";

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
			PhotonNetwork.playerName = "BiplaneDuck";
		}
		else if ( alias.ToUpper () == "BIPLANEDUCK" )
		{
			PhotonNetwork.playerName = "Asshat";
		}
		else
		{
			PhotonNetwork.playerName = alias;
		}

		m_Alias = alias;

		SaveAlias ();
	}

	private string LoadAlias ()
	{
		return PlayerPrefs.GetString (User_Alias_Pref_Key, "Player" + Random.Range (10000, 99999));
	}

	private void SaveAlias ()
	{
		PlayerPrefs.SetString (User_Alias_Pref_Key, m_Alias);
		PlayerPrefs.Save ();
	}
}
