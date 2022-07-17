using System;
using MelonLoader;
using HarmonyLib;
using UnityEngine;
using System.Reflection;
using System.Xml.XPath;
using System.Globalization;
using UnhollowerRuntimeLib;
using ModSettings;
using System.Collections;
using System.IO;
using System.Collections.Generic;

namespace StickPick
{
	[HarmonyPatch(typeof(PlayerManager), "InteractiveObjectsProcessInteraction")]
	public class ExecuteInteractActionCardGame
	{
		public static bool Prefix(ref PlayerManager __instance)
		{
			GearItem gearitem = __instance.m_InteractiveObjectUnderCrosshair.GetComponent<GearItem>();
			if (Settings.options.skipInspect && __instance.m_InteractiveObjectUnderCrosshair != null)
			{
				if(Settings.options.skipInspectAll)
				{
					if(__instance.m_InteractiveObjectUnderCrosshair.name.Contains("GEAR_")&&!__instance.m_InteractiveObjectUnderCrosshair.name.Contains("CardGame")&& !gearitem.IsAttachedToPlacePoint())
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