using HarmonyLib;
using MelonLoader;

namespace QuickPick
{
	/*
	[HarmonyPatch(typeof(PlayerManager), "InteractiveObjectsProcessInteraction")]
	public class ExecuteInteractActionCardGame
	{
		
		public static bool Prefix(ref PlayerManager __instance)
		{
			if(Settings.options.EnableMod && Settings.options.SkipMenu)
            {
				if(QuickPick_Main.NoClickHold())
                {
					GearItem gearitem = new GearItem();
					if (__instance.m_InteractiveObjectUnderCrosshair != null) { gearitem = __instance.m_InteractiveObjectUnderCrosshair.GetComponent<GearItem>(); }
					bool allowedtoskip = gearitem != null && !gearitem.IsAttachedToPlacePoint() && !Settings.options.SkipNotes && gearitem.m_NarrativeCollectibleItem == null && gearitem.m_Bed == null;
					if (__instance.m_InteractiveObjectUnderCrosshair != null)
					{
						if ((Settings.options.pickupChoice == 0) || (Settings.options.pickupChoice == 1 && gearitem.name.Contains("GEAR_Stick")) || !Settings.options.Allow_AoE)
						{
							if (allowedtoskip && __instance.m_InteractiveObjectUnderCrosshair.name.Contains("GEAR_") && !gearitem.name.Contains("CardGame"))
							{
								GameManager.GetPlayerManagerComponent().ProcessPickupItemInteraction(gearitem, false, false);
								return false;
							}
						}
					}

					// if object is not the cardgame
				}
			}
			return true;
		}
		
		
	}
	*/
	[HarmonyPatch(typeof(PlayerManager), "InteractiveObjectsProcessInteraction")]
	public class ExecuteInteractActionCardGame
	{
		public static bool Prefix(ref PlayerManager __instance)
		{
			GearItem gearitem = new GearItem();
			if (__instance.m_InteractiveObjectUnderCrosshair != null) { gearitem = __instance.m_InteractiveObjectUnderCrosshair.GetComponent<GearItem>(); }
			bool NotonCookingSlot = gearitem != null && !gearitem.IsAttachedToPlacePoint();
			bool allowedtoskip = NotonCookingSlot && (gearitem.m_Bed == null) && !Settings.options.SkipNotes && gearitem.m_NarrativeCollectibleItem == null;
			if (Settings.options.EnableMod && QuickPick_Main.AllowSkip(gearitem) && __instance.m_InteractiveObjectUnderCrosshair != null)
			{

				if (Settings.options.pickupChoice == 1)
				{
					if (__instance.m_InteractiveObjectUnderCrosshair.name.Contains("GEAR_Stick"))
					{
						GameManager.GetPlayerManagerComponent().ProcessPickupItemInteraction(gearitem, false, false);
						return false;
					}
				}
				if (__instance.m_InteractiveObjectUnderCrosshair.name.Contains("GEAR_") && !__instance.m_InteractiveObjectUnderCrosshair.name.Contains("CardGame") && gearitem != null && allowedtoskip)
					{
						GameManager.GetPlayerManagerComponent().ProcessPickupItemInteraction(gearitem, false, false);
						return false;
					}
				
			}

			// if object is not the cardgame             
			return true;
		}
	}


}