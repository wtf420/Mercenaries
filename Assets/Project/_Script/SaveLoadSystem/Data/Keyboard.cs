using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]

public class Keyboard
{
	#region Fields & Properties

	public Dictionary<KeyboardHandler, string> Keyboards;

	#endregion

	#region Methods 

	public Keyboard()
	{
		Keyboards = new Dictionary<KeyboardHandler, string>();
		Keyboards.Add(KeyboardHandler.MoveForward, "W");
		Keyboards.Add(KeyboardHandler.MoveBack, "S");
		Keyboards.Add(KeyboardHandler.MoveLeft, "A");
		Keyboards.Add(KeyboardHandler.MoveRight, "D");
		Keyboards.Add(KeyboardHandler.Dash, "Space");
		Keyboards.Add(KeyboardHandler.Weapon1, "Alpha1");
		Keyboards.Add(KeyboardHandler.Weapon2, "Alpha2");
	}

	#endregion
}

