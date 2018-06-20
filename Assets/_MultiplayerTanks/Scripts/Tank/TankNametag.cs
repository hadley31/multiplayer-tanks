using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TankNametag : TankBase
{
	public bool localNametag;
	private TextMeshPro m_Text;

	private void Awake ()
	{
		m_Text = GetComponentInChildren<TextMeshPro> ();
	}

	private void OnEnable ()
	{
		if (Tank.photonView.isMine && !localNametag)
		{
			this.enabled = false;
			return;
		}

		UpdateName ();
	}

	private void LateUpdate ()
	{
		if (Tank == null)
		{
			return;
		}
	}

	public void SetName (string name)
	{
		m_Text.text = name;
	}

	public void SetColor (Color color)
	{
		m_Text.color = color;
	}

	public void UpdateName ()
	{
		SetName (Tank.OwnerAlias);
	}
}
