using HarmonyLib;
using MelonLoader;

namespace StickPick
{
	[HarmonyPatch(typeof(PlayerManager), "InteractiveObjectsProcessInteraction")]
	public class ExecuteInteractActionCardGame
	{
		public static bool Prefix(ref PlayerManager __instance)
		{
			GearItem gearitem = new GearItem();
			if (__instance.m_InteractiveObjectUnderCrosshair != null) { gearitem = __instance.m_InteractiveObjectUnderCrosshair.GetComponent<GearItem>(); }
			bool allowedtoskip = gearitem != null && !gearitem.IsAttachedToPlacePoint() && gearitem.m_NarrativeCollectibleItem == null && (gearitem.m_Bed == null);
			if (Settings.options.SkipInspectChoice != 0 && __instance.m_InteractiveObjectUnderCrosshair != null)
			{
				if (Settings.options.SkipInspectChoice == 1)
				{
					if (__instance.m_InteractiveObjectUnderCrosshair.name.Contains("GEAR_") && !__instance.m_InteractiveObjectUnderCrosshair.name.Contains("CardGame") && gearitem != null && allowedtoskip)
					{
						GameManager.GetPlayerManagerComponent().ProcessPickupItemInteraction(gearitem, false, false);
						return false;
					}
				}
				else
				{
					if(__instance.m_InteractiveObjectUnderCrosshair.name.Contains("GEAR_Stick")|| __instance.m_InteractiveObjectUnderCrosshair.name.Contains("GEAR_Stone"))
					{
						GameManager.GetPlayerManagerComponent().ProcessPickupItemInteraction(gearitem, false, false);
						return false;
					}
				}
			}

			// if object is not the cardgame             
			return true;
		}
	}
}