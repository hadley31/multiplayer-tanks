using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TankNametag : MonoBehaviour
{
	public Tank tank;
	public float followSpeed = 30;
	private TextMeshPro m_Text;

	private void Awake ()
	{
		m_Text = GetComponentInChildren<TextMeshPro> ();
	}

	private void OnEnable ()
	{
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

	public void UpdateName ()
	{
		SetName (tank.OwnerAlias);
	}
}
