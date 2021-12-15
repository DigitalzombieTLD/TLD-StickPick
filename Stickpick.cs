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
    public class StickPick_Main : MelonMod
    {
		public static GameObject playerObject;
		public static int layerMask = 0;
		public static int stoneCounter = 0, stickCounter = 0, customCounter = 0;
		public static List<string> customItems = new List<string>();

		public override void OnApplicationStart()
        {
			layerMask |= 1 << 17; // gear layer		
			StickPick.Settings.OnLoad();

			loadCustomItemList();			
		}

		public override void OnSceneWasLoaded(int buildIndex, string sceneName)
		{
			
		}

		public override void OnUpdate()
		{
			if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.buttonPickSticks)) 
			{				
				RaycastHit[] sphereTargethit;
				sphereTargethit = Physics.SphereCastAll(GameManager.GetVpFPSPlayer().transform.position, Settings.options.pickupRadius, GameManager.GetVpFPSPlayer().transform.TransformDirection(Vector3.down), GameManager.GetVpFPSPlayer().Controller.m_Controller.height * 0.8f, layerMask);
			
				foreach (RaycastHit foo in sphereTargethit)
				{
					GearItem foundItem = foo.transform.gameObject.GetComponent<GearItem>();

					if(foundItem && foundItem.enabled)
					{				
						if (foundItem.m_GearName.Contains("GEAR_Stick") && !foundItem.m_InPlayerInventory && (Settings.options.pickupChoice == 0 || Settings.options.pickupChoice == 2))
						{
							GameAudioManager.PlaySound(foundItem.m_PutBackAudio, InterfaceManager.GetSoundEmitter());
							GameManager.GetPlayerManagerComponent().ProcessPickupItemInteraction(foundItem, false, true);
							stickCounter++;
						}

						if (foundItem.m_GearName.Contains("GEAR_Stone") && !foundItem.m_InPlayerInventory && (Settings.options.pickupChoice == 1 || Settings.options.pickupChoice == 2))
						{
							GameAudioManager.PlaySound(foundItem.m_PutBackAudio, InterfaceManager.GetSoundEmitter());
							GameManager.GetPlayerManagerComponent().ProcessPickupItemInteraction(foundItem, false, true);
							stoneCounter++;
						}

						if (customItems.Contains(foundItem.m_GearName) && !foundItem.m_InPlayerInventory && Settings.options.pickUpAdditionalItems == true)
						{
							GameAudioManager.PlaySound(foundItem.m_PutBackAudio, InterfaceManager.GetSoundEmitter());
							GameManager.GetPlayerManagerComponent().ProcessPickupItemInteraction(foundItem, false, true);
							customCounter++;
						}
					}
				}

				int calorieCost = 0;
				int itemCount = stoneCounter + stoneCounter + customCounter;

				if( itemCount!=0 && Settings.options.calorieCost != 0)
				{
					calorieCost = calorieCost * Settings.options.calorieCost;
					GameManager.GetHungerComponent().RemoveReserveCalories(calorieCost);
				}						
					
				MelonLogger.Msg("Picked up " + stickCounter + " sticks and " + stoneCounter + " stones in radius of " + Settings.options.pickupRadius + " meter and spent " + calorieCost + " calories.");
				
				if(Settings.options.pickUpAdditionalItems)
				{
					MelonLogger.Msg("Oh and " + customCounter + " items from your custom list also!");
				}			

				customCounter = 0;
				stoneCounter = 0;
				stickCounter = 0;				
			}
		}

		public static void loadCustomItemList()
		{
			customItems.Clear();

			if (File.Exists("Mods\\StickPickCustomList.txt") && Settings.options.pickUpAdditionalItems)
			{
				using (StreamReader sr = File.OpenText("Mods\\StickPickCustomList.txt"))
				{
					while (!sr.EndOfStream)
					{
						customItems.Add(sr.ReadLine());
					}
					sr.Close();
				}
				
				MelonLogger.Msg("Loaded custom list with " + customItems.Count + " items");
			}
		}
	}
}
