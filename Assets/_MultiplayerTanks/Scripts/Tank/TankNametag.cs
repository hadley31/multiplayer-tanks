using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TankNametag : TankBase
{
	public Tank tank;
	public bool localNametag;
	private TextMeshPro m_Text;

	private void Awake ()
	{
		m_Text = GetComponentInChildren<TextMeshPro> ();
	}

	private void OnEnable ()
	{
		if (tank.photonView.isMine && !localNametag)
		{
			this.gameObject.SetActive(false);
			return;
		}

		UpdateName ();
	}

	private void LateUpdate ()
	{
		if (tank == null)
		{
			return;
		}
		
		m_Text.transform.rotation = Camera.main.transform.rotation;
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
		SetName (tank.OwnerAlias);
	}
}
