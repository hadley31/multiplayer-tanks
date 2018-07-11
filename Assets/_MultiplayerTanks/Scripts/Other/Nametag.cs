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

	public string Name
	{
		get { return m_Text.text; }
		set { m_Text.text = value; }
	}

	public Color Color
	{
		get { return m_Text.color; }
		set { m_Text.color = value; }
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
		this.Name = name;
	}

	public void SetColor (Color color)
	{
		this.Color = color;
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
