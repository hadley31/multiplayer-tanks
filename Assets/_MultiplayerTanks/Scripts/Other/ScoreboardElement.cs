using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreboardElement : MonoBehaviour
{
	public TextMeshProUGUI playerNameText;
	public TextMeshProUGUI teamNameText;
	public TextMeshProUGUI killsText;
	public TextMeshProUGUI deathsText;
	public TextMeshProUGUI scoreText;
	public TextMeshProUGUI pingText;

	private CanvasGroup m_Group;

	public Tank Tank
	{
		get;
		private set;
	}

	private void Awake ()
	{
		m_Group = GetComponent<CanvasGroup> ();
	}

	public void Prime (Tank tank)
	{
		if (tank == null)
		{
			return;
		}

		this.Tank = tank;

		Refresh ();
	}

	public void Refresh ()
	{
		if ( Tank == null || Tank.IsPlayer == false )
		{
			return;
		}

		// if the tank is dead, we want the element to be a little transparent
		m_Group.alpha = Tank.IsAlive ? 1 : 0.3f;

		playerNameText.text = Tank.OwnerAlias;

		teamNameText.color = Tank.Team.Color;
		teamNameText.text = Tank.Team.Name;


		killsText.text = Tank.Owner.GetKills ().ToString ();
		deathsText.text = Tank.Owner.GetDeaths ().ToString ();
		scoreText.text = Tank.Owner.GetScore ().ToString ();

		// get the ping of the player
		int ping = Tank.Owner.GetPing ();

		// set the ping color to an interactive value
		pingText.color = GetPingColor (ping);
		pingText.text = ping.ToString ();
	}

	private Color GetPingColor (int ping)
	{
		if ( ping < 80 )
		{
			return Color.white;
		}
		if (ping < 140)
		{
			return Color.yellow;
		}
		return Color.red;
	}
}
