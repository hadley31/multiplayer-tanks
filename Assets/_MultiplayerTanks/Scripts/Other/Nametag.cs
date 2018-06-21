using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Nametag : MonoBehaviour
{
	private Transform m_AssociatedTransform;

	private TextMeshPro m_Text;

	public bool IsVisible
	{
		get { return gameObject.GetActive (); }
	}

	private void Awake ()
	{
		m_Text = GetComponentInChildren<TextMeshPro> ();
	}

	public void Prime (Transform go)
	{
		m_AssociatedTransform = go;
	}

	public void SetName (string name)
	{
		m_Text.text = name;
	}

	public void SetColor (Color color)
	{
		m_Text.color = color;
	}

	public void SetVisible (bool visible)
	{
		gameObject.SetActive (visible);
	}

	private void LateUpdate ()
	{
		if ( m_AssociatedTransform == null )
		{
			return;
		}

		transform.position = m_AssociatedTransform.position;
	}
}
