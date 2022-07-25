﻿using System.IO;
using System.Reflection;
using UnityEngine;
using ModSettings;

namespace QuickPick
{
    internal class QuickPickSettingsMain : JsonModSettings
    {
		[Name("Enable")]
		[Description("Enables the Mod.")]
		public bool EnableMod = false;


		[Section("Skip Item Inspection Menu")]

		[Name("Quick Pickup")]
		[Description("Skip the inspection menu on picking up an item.")]
		public bool SkipMenu = false;

		[Name("Skip Inspection on Notes")]
		[Description("Put notes straight into inventory without a chance to read them first.")]
		public bool SkipNotes = false;

		[Section("AoE Pickup")]

		[Name("Enable AoE Pickup")]
		[Description("Allows for picking up items in a range around you.")]
		public bool Allow_AoE = false;

		[Name("Pickup radius")]
		[Description("Choose the radius (sphere) around the player which gets searched for objects")]
		[Slider(0f, 15f)]
		public float pickupRadius = 2.0f;

		[Name("Pickup calorie cost")]
		[Description("Calorie cost per item picked up around you.")]
		[Slider(0, 25)]
		public int calorieCost = 2;

		[Name("AoE Item Filter")]
		[Description("Choose which items around you will be picked up.")]
		[Choice("All", "Only Sticks", "Custom List")]
		public int pickupChoice = 0;



		protected override void OnConfirm()
        {
            base.OnConfirm();
			QuickPick_Main.loadCustomItemList();
			
		}

		protected override void OnChange(FieldInfo field, object oldValue, object newValue)
		{
			if (field.Name == nameof(EnableMod) ||
				field.Name == nameof(SkipMenu) ||
				field.Name == nameof(SkipNotes) ||
				field.Name == nameof(Allow_AoE) ||
				field.Name == nameof(pickupRadius) ||
				field.Name == nameof(calorieCost) ||
				field.Name == nameof(pickupChoice))
			{
				RefreshFields();
			}
		}
		public void RefreshFields()
		{
			SetFieldVisible(nameof(SkipMenu), Settings.options.EnableMod);
			SetFieldVisible(nameof(Allow_AoE), Settings.options.EnableMod);

			SetFieldVisible(nameof(SkipNotes), Settings.options.EnableMod && Settings.options.SkipMenu);

			SetFieldVisible(nameof(pickupRadius), Settings.options.EnableMod && Settings.options.Allow_AoE);
			SetFieldVisible(nameof(calorieCost), Settings.options.EnableMod && Settings.options.Allow_AoE);
			SetFieldVisible(nameof(pickupChoice), Settings.options.EnableMod &&Settings.options.Allow_AoE);
		}
	}



    internal static class Settings
    {
        public static QuickPickSettingsMain options;

        public static void OnLoad()
        {
            options = new QuickPickSettingsMain();
            options.AddToModSettings("QuickPick Settings");
			QuickPick_Main.loadCustomItemList();
			options.RefreshFields();
		}
    }
}
