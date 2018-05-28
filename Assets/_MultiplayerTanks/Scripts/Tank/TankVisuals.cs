using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TankVisuals : TankBase
{
	[SerializeField]
	private Color m_Color = Color.black;

	public Color Color
	{
		get { return m_Color; }
	}

	public bool IsVisable
	{
		get { return Tank.IsAlive; }
	}

	private void Awake ()
	{
		SetColor (m_Color);
	}

	public void Show ()
	{

	}

	public void Hide ()
	{

	}

	public void SetColor (Color color)
	{
		this.m_Color = color;

		transform.Find ("Bottom").Find ("Visual").GetComponent<Renderer> ().material.color = m_Color;
		transform.Find ("Top").Find ("Visual").GetComponent<Renderer> ().material.color = m_Color;
	}

	public void UpdateColor ()
	{

	}

	public void SetColor (int team)
	{

	}
}
