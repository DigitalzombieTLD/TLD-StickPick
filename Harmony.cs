using HarmonyLib;
using MelonLoader;

namespace QuickPick
{

	[HarmonyPatch(typeof(PlayerManager), "InteractiveObjectsProcessInteraction")]
	public class ExecuteInteractActionCardGame
	{
		public static bool Prefix(ref PlayerManager __instance)
		{
			if (__instance.m_InteractiveObjectUnderCrosshair != null)
			{
				GearItem gearitem = __instance.m_InteractiveObjectUnderCrosshair.GetComponent<GearItem>();
				if(gearitem != null)
                {
					bool NotonCookingSlot = !gearitem.IsAttachedToPlacePoint();
					bool suppressedbynote = (!Settings.options.SkipNotes && gearitem.m_NarrativeCollectibleItem != null);
					bool allowedtoskip = !QuickPick_Main.IsLitItem(gearitem) && NotonCookingSlot && (gearitem.m_Bed == null) && !suppressedbynote && !QuickPick_Main.IsCarcass(gearitem);
					if (Settings.options.EnableMod && allowedtoskip && QuickPick_Main.SkipAllowedorOverridden(gearitem))
					{
						if (Settings.options.pickupChoice == 1)
						{
							if (gearitem.name.Contains("GEAR_Stick"))
							{
								GameManager.GetPlayerManagerComponent().ProcessPickupItemInteraction(gearitem, false, false);
								return false;
							}
						}
						if (gearitem.name.Contains("GEAR_") && !__instance.m_InteractiveObjectUnderCrosshair.name.Contains("CardGame") && gearitem != null && allowedtoskip)
						{
							GameManager.GetPlayerManagerComponent().ProcessPickupItemInteraction(gearitem, false, false);
							return false;
						}
					}
				}

			}

			// if object is not the cardgame             
			return true;
		}
	}


}