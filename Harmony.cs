using System;
using MelonLoader;
using Harmony;
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
			if(Settings.options.skipInspect && __instance.m_InteractiveObjectUnderCrosshair != null)
			{
				if(Settings.options.skipInspectAll)
				{
					if(__instance.m_InteractiveObjectUnderCrosshair.name.Contains("GEAR_")&&!__instance.m_InteractiveObjectUnderCrosshair.name.Contains("CardGame"))
					{
						GameManager.GetPlayerManagerComponent().ProcessPickupItemInteraction(__instance.m_InteractiveObjectUnderCrosshair.GetComponent<GearItem>(), false, false);
						return false;
					}
				}
				else
				{
					if(__instance.m_InteractiveObjectUnderCrosshair.name.Contains("GEAR_Stick")|| __instance.m_InteractiveObjectUnderCrosshair.name.Contains("GEAR_Stone"))
					{
						GameManager.GetPlayerManagerComponent().ProcessPickupItemInteraction(__instance.m_InteractiveObjectUnderCrosshair.GetComponent<GearItem>(), false, false);
						return false;
					}
				}
			}

			// if object is not the cardgame             
			return true;
		}
	}
}