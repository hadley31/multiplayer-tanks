using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlanderMenu : MonoBehaviour
{
	public static readonly List<HighlanderMenu> menus = new List<HighlanderMenu> ();

	public int tier;

	public void OnEnable ()
	{
		menus.Add (this);
	}

	public void OnDisable ()
	{
		menus.Remove (this);
	}

	public bool IsOpen
	{
		get { return menus.Contains (this); }
	}

	public void Open ()
	{
		gameObject.SetActive (true);
		
		
		foreach (HighlanderMenu menu in menus)
		{
			if (menu.tier <= this.tier && menu != this)
			{
				menu.Close ();
			}
		}
	}

	public void Close ()
	{
		gameObject.SetActive (false);
	}

	public void Toggle ()
	{
		if (IsOpen)
		{

		}
	}
}
