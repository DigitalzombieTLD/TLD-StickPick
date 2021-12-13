using System.IO;
using System.Reflection;
using UnityEngine;
using ModSettings;

namespace StickPick
{
    internal class StickPickSettingsMain : JsonModSettings
    {    
        [Section("Keybind")]

		[Name("Pick up sticks")]
		[Description("pickSticks")]
		public UnityEngine.KeyCode buttonPickSticks = KeyCode.LeftAlt;

		[Section("Other settings")]

		[Name("What to pick")]
		[Description("Choose between stick, stones or sticks & stones")]
		[Choice("Sticks", "Stones", "Sticks and stones")]
		public int pickupChoice = 0;

		[Name("Pickup radius")]
		[Description("Choose the radius (sphere) around the player which gets searched for objects")]
		[Slider(0f, 8f)]
		public float pickupRadius = 2.0f;

		[Name("Pickup calorie cost")]
		[Description("Calorie penalty for picking up a single stick/stone (multiplied if more than one items gets picked up)")]
		[Slider(0, 25)]
		public int calorieCost = 10;

		protected override void OnConfirm()
        {
            base.OnConfirm();
        }
    }

    internal static class Settings
    {
        public static StickPickSettingsMain options;

        public static void OnLoad()
        {
            options = new StickPickSettingsMain();
            options.AddToModSettings("StickPick Settings");
        }
    }
}
