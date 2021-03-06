﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class TankVisuals : TankBase
{
	[SerializeField]
	private Color m_Color = Color.black;

	public ColorUnityEvent onColorChanged;
	public UnityEvent onShow;
	public UnityEvent onHide;

	public Color Color
	{
		get { return m_Color; }
	}

	public bool IsVisible
	{
		get;
		private set;
	}

	public Renderer TopVisual
	{
		get;
		private set;
	}

	public Renderer BottomVisual
	{
		get;
		private set;
	}

	public Renderer BarrelVisual
	{
		get;
		private set;
	}

	private void Awake ()
	{
		TopVisual = transform.Find ("Top/Visual").GetComponent<Renderer> ();
		BottomVisual = transform.Find ("Base/Visual").GetComponent<Renderer> ();
		BarrelVisual = transform.Find ("Top/Barrel").GetComponent<Renderer> ();

		SetColor (m_Color);
	}

	public void SetVisible (bool value)
	{
		if ( IsVisible = value )
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
		TopVisual.enabled = true;
		BottomVisual.enabled = true;
		BarrelVisual.enabled = true;

		onShow.Invoke ();
	}

	public void Hide ()
	{
		TopVisual.enabled = false;
		BottomVisual.enabled = false;
		BarrelVisual.enabled = false;

		onHide.Invoke ();
	}


	public void SetColorRPC (Color color)
	{
		if (Tank.IsLocal == false && PhotonNetwork.isMasterClient == false)
		{
			return;
		}

		photonView.RPC ("SetColor", PhotonTargets.AllBuffered, color.ToVector ());
	}

	public void SetColor (Color color)
	{
		TopVisual.material.color = color;
		BottomVisual.material.color = color;

		if (m_Color == color)
		{
			return;
		}

		this.m_Color = color;

		onColorChanged.Invoke (m_Color);
	}

	[PunRPC]
	private void SetColor (Vector3 value)
	{
		SetColor (value.ToColor ());
	}

	public void RevertToTeamColor ()
	{
		if (PhotonNetwork.inRoom)
		{
			SetColorRPC (Tank.Team.Color);
		}
	}
}
