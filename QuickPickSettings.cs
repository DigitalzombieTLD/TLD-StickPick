using System.IO;
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

		[Name("Quick Pickup")]
		[Description("Skip the inspection menu on picking up an item.")]
		public bool SkipMenu = false;

		[Name("Quick Pickup Carcasses")]
		[Description("Will put rabbits directly into inventory instead of being able to harvest on the ground.")]
		public bool SkipBun = false;

		[Name("Quick Pickup Notes")]
		[Description("Put notes straight into inventory without a chance to read them first.")]
		public bool SkipNotes = false;


		[Section("AoE Pickup")]

		[Name("Enable AoE Pickup")]
		[Description("Allows for picking up items in a range around you.")]
		public bool Allow_AoE = false;

		[Name("Only Outdoors")]
		[Description("Allows for picking up items in a range around you.")]
		public bool AoE_Out = false;

		[Name("Enable on Click")]
		[Description("Will AoE pickup when you hold left click.)")]
		public bool AllowClick = false;

		[Name("Enable on Key Press")]
		[Description("Will AoE pickup when you push a key.)")]
		public bool AllowKey = false;

		[Name("Keybind")]
		[Description("Pick up items in radius around you")]
		public UnityEngine.KeyCode KeyBind = KeyCode.LeftAlt;

		[Name("Pickup radius")]
		[Description("Choose the radius (sphere) around the player which gets searched for objects")]
		[Slider(0f, 15f)]
		public float pickupRadius = 2.0f;

		[Name("Pickup calorie cost")]
		[Description("Calorie cost per item picked up around you.")]
		[Slider(0, 25)]
		public int calorieCost = 2;

		[Section("AoE Item Filtering")]

		[Name("Items to Pick")]
		[Description("Choose which items around you(AoE) will be picked up.")]
		[Choice("All", "Only Sticks", "Custom List")]
		public int pickupChoice = 0;

		[Name("Ignore Pelts")]
		[Description("Ignore pelts regardless of other settings.")]
		public bool IgnorePelts = false;

		[Name("Override Quick Pickup for \"Items to Pick\"")]
		[Description("Items that you have allowed AoE for will also allow quick pickup.")]
		public bool PickOverride = false;

		[Name("Quick Pickup Dropped Items")]
		[Description("Items you have dropped will be picked up regardless of filters.")]
		public bool PickDrop = false;

		[Name("Custom List Type")]
		[Description("Choose whether the item list(in mods folder) acts as a blacklist or whitelist.")]
		[Choice("Whitelist", "Blacklist")]
		public int ListType = 0;

		protected override void OnConfirm()
        {
            base.OnConfirm();
		}

		protected override void OnChange(FieldInfo field, object oldValue, object newValue)
		{
			if (field.Name == nameof(EnableMod) ||
				field.Name == nameof(SkipMenu) ||
				field.Name == nameof(SkipBun) ||
				field.Name == nameof(SkipNotes) ||
				field.Name == nameof(Allow_AoE) ||
				field.Name == nameof(AoE_Out) ||
				field.Name == nameof(AllowClick) ||
				field.Name == nameof(AllowKey) ||
				field.Name == nameof(KeyBind) ||
				field.Name == nameof(pickupRadius) ||
				field.Name == nameof(calorieCost) ||
				field.Name == nameof(pickupChoice) ||
				field.Name == nameof(IgnorePelts) ||
				field.Name == nameof(PickOverride) ||
				field.Name == nameof(ListType) ||
				field.Name == nameof(PickDrop))
			{
				RefreshFields();
			}
		}
		public void RefreshFields()
		{
			SetFieldVisible(nameof(SkipMenu), Settings.options.EnableMod);
			SetFieldVisible(nameof(Allow_AoE), Settings.options.EnableMod);

			SetFieldVisible(nameof(SkipBun), Settings.options.EnableMod && Settings.options.SkipMenu);
			SetFieldVisible(nameof(SkipNotes), Settings.options.EnableMod && Settings.options.SkipMenu);

			SetFieldVisible(nameof(AoE_Out), Settings.options.EnableMod && Settings.options.Allow_AoE);
			SetFieldVisible(nameof(AllowClick), Settings.options.EnableMod && Settings.options.Allow_AoE);
			SetFieldVisible(nameof(AllowKey), Settings.options.EnableMod && Settings.options.Allow_AoE);
			SetFieldVisible(nameof(KeyBind), Settings.options.EnableMod && Settings.options.Allow_AoE && Settings.options.AllowKey);
			SetFieldVisible(nameof(pickupRadius), Settings.options.EnableMod && Settings.options.Allow_AoE);
			SetFieldVisible(nameof(calorieCost), Settings.options.EnableMod && Settings.options.Allow_AoE);
			SetFieldVisible(nameof(pickupChoice), Settings.options.EnableMod &&Settings.options.Allow_AoE);
			SetFieldVisible(nameof(IgnorePelts), Settings.options.EnableMod && Settings.options.Allow_AoE);
			SetFieldVisible(nameof(PickOverride), Settings.options.EnableMod && Settings.options.Allow_AoE && !Settings.options.SkipMenu);
			SetFieldVisible(nameof(ListType), Settings.options.EnableMod && Settings.options.Allow_AoE && pickupChoice == 2);
			SetFieldVisible(nameof(PickDrop), Settings.options.EnableMod && Settings.options.Allow_AoE && pickupChoice != 0);
		}
	}



    internal static class Settings
    {
        public static QuickPickSettingsMain options;

        public static void OnLoad()
        {
            options = new QuickPickSettingsMain();
            options.AddToModSettings("QuickPick");
			QuickPick_Main.loadCustomItemList();
			options.RefreshFields();
		}
    }
}
