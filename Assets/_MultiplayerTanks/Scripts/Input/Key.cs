using System;
using UnityEngine;


public enum KeyPressType
{
	None,
	Down,
	Up,
	Held
}

public struct Key : IEquatable<Key>
{
	public static readonly Key None = new Key ("NO_KEY", KeyCode.Joystick8Button9);

	public readonly string Name;
	public readonly KeyCode KeyCode;

	public bool IsPressed
	{
		get { return Input.GetKey (this.KeyCode); }
	}

	public Key (string name, KeyCode keyCode)
	{
		this.Name = name;
		this.KeyCode = keyCode;
	}

	public KeyPressType GetPressType ()
	{
		if ( Input.GetKeyDown (this.KeyCode) )
			return KeyPressType.Down;

		if ( Input.GetKey (this.KeyCode) )
			return KeyPressType.Held;

		if ( Input.GetKeyUp (this.KeyCode) )
			return KeyPressType.Up;

		return KeyPressType.None;
	}

	public bool IsPressType (KeyPressType type)
	{
		return GetPressType () == type;
	}

	public override int GetHashCode ()
	{
		return base.GetHashCode ();
	}

	public override bool Equals (object obj)
	{
		if ( obj is Key )
		{
			return ( (Key) obj ).Name == this.Name;
		}
		return false;
	}

	public override string ToString ()
	{
		return string.Format ("Key[Name: {0}, KeyCode:{1}]", this.Name, this.KeyCode);
	}

	public bool Equals (Key other)
	{
		return this.Name == other.Name;
	}

	public static int operator + (Key a, Key b)
	{
		return (a.IsPressed ? 1 : 0) + (b.IsPressed ? 1 : 0);
	}

	public static int operator - (Key a, Key b)
	{
		return ( a.IsPressed ? 1 : 0 ) - ( b.IsPressed ? 1 : 0 );
	}

	public static bool operator == (Key a, Key b)
	{
		return a.Name == b.Name;
	}

	public static bool operator != (Key a, Key b)
	{
		return a.Name != b.Name;
	}
}
