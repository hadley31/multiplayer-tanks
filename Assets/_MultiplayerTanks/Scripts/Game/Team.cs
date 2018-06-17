﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Team
{
	public string Name
	{
		get;
	}

	public int Number
	{
		get;
	}

	public Color Color
	{
		get;
	}

	public Team (string name, int number, Color color)
	{
		this.Name = name;
		this.Number = number;
		this.Color = color;
	}


	public override bool Equals (object obj)
	{
		if (obj is Team)
		{
			Team other = (Team) obj;

			return this.Number == other.Number;
		}
		return false;
	}

	public override int GetHashCode ()
	{
		return base.GetHashCode ();
	}

	public override string ToString ()
	{
		return $"Team [Number: '{Number}'; Name: '{Name}'; Color: {Color.ToString ()}]";
	}

	public static bool operator == (Team a, Team b)
	{
		return a.Number == b.Number;
	}

	public static bool operator != (Team a, Team b)
	{
		return a.Number != b.Number;
	}
}
