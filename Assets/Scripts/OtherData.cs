using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherData : MonoBehaviour
{
	int _value = 0;
	public int Value
	{
		get
		{
			return _value;
		}
		set
		{
			_value = value;
		}
	}
	Color _color = new Color(1f, 1f, 1f, 1f);
	public Color Color
	{
		get
		{
			return _color;
		}
		set
		{
			_color = value;
		}
	}

	public void AddValue()
	{
		_value += 1;

	}
}
