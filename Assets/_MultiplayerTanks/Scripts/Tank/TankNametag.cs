﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TankNametag : TankBase
{
	public bool localNametag = false;
	private Nametag m_NametagObject;

	private void Start ()
	{
		if ( photonView.isMine == true && localNametag == false )
		{
			return;
		}

		m_NametagObject = Instantiate (Resources.Load<Nametag> ("Nametag"), transform.position, Quaternion.identity);
		m_NametagObject.Prime (transform);
		RefreshTag ();
	}

	private void OnDestroy ()
	{
		if (m_NametagObject != null)
		{
			Destroy (m_NametagObject.gameObject);
		}
	}

	public void SetVisible (bool visible)
	{
		if (visible)
		{
			Show ();
		}
		else
		{
			Hide ();
		}
	}

	public void Show ()
	{
		RefreshTag ();
		m_NametagObject?.SetVisible (true);
	}

	public void Hide ()
	{
		m_NametagObject?.SetVisible (false);
	}

	public void SetName (string name)
	{
		m_NametagObject?.SetName (name);
	}

	public void SetColor (Color color)
	{
		m_NametagObject?.SetColor (color);
	}

	public void RefreshTag ()
	{
		if ( Tank.Owner != null )
		{
			SetName (Tank.OwnerAlias);
			SetColor (Tank.Team.Color);
		}
	}
}
