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

namespace QuickPick
{
	public class QuickPick_Main : MelonMod
	{
		public static GameObject playerObject;
		public static int layerMask = 0;
		public static int stoneCounter = 0, stickCounter = 0, customCounter = 0;
		public static List<string> CustomList = new List<string>();



		bool pickingup()
		{
			if ( NoMenuOpen() && NoClickHold() && InterfaceManager.m_Instance != null)
			{
				return InputManager.GetFirePressed(InputManager.m_CurrentContext);
			}
						
			return false;
		}
		
		public static bool NoClickHold()
        {
			if (GameManager.Instance() == null) return false;
			if (GameManager.GetPlayerManagerComponent() == null) return false;
			return !GameManager.GetPlayerManagerComponent().IsClickHoldActive();
		}
		bool NoMenuOpen() { return InterfaceManager.m_Instance != null && !InterfaceManager.m_Instance.AnyOverlayPanelEnabled(); }


		float pickupradius() 
		{
			if (Settings.options.EnableMod && Settings.options.Allow_AoE) return Settings.options.pickupRadius;
			else return .001f;
		}	

		bool ValidFromList(GearItem item)
        {
			MelonLogger.Msg("is item null?: " + item == null);
			if(item == null) return false;
			MelonLogger.Msg("item: " + item.name);
			MelonLogger.Msg("is customlist null or empty?: " + (CustomList == null || CustomList.Count == 0));
			if (CustomList == null || CustomList.Count == 0) return true;
			bool result = CustomList.Contains(item.m_GearName);
			MelonLogger.Msg("list type: " + Settings.options.ListType.ToString());
			MelonLogger.Msg("item on list?: " + result);
			if (Settings.options.ListType == 0) return result;
			return !result;
		}

		bool Dropped(GearItem item)
        {
			return (Settings.options.PickDrop && item.m_BeenInPlayerInventory);
        }
		
		void pickupitem(GearItem foundItem)
        {
			if(foundItem != null)
            {
				bool allowedtopick = false;
				if (Settings.options.pickupChoice == 0 || Dropped(foundItem) || (Settings.options.pickupChoice == 1 && foundItem.name.Contains("GEAR_Stick")) || ((Settings.options.pickupChoice == 2 && ValidFromList(foundItem))))
				{
					allowedtopick = true;
				}
				if (foundItem && foundItem.enabled)
				{
					if (allowedtopick && !foundItem.m_InPlayerInventory)
					{
						GameAudioManager.PlaySound(foundItem.m_PutBackAudio, InterfaceManager.GetSoundEmitter());
						GameManager.GetPlayerManagerComponent().ProcessPickupItemInteraction(foundItem, false, true);
						itemcount++;
					}
				}
			}
		}
		
		GearItem gearitem = new GearItem();
		int itemcount;
		public override void OnUpdate()
		{
			if (pickingup())
			{
				RaycastHit[] sphereTargethit;
				sphereTargethit = Physics.SphereCastAll(GameManager.GetVpFPSPlayer().transform.position, pickupradius(), GameManager.GetVpFPSPlayer().transform.TransformDirection(Vector3.down), GameManager.GetVpFPSPlayer().Controller.m_Controller.height * 0.8f, layerMask);

				foreach (RaycastHit foo in sphereTargethit)
				{
					GearItem founditem = foo.transform.gameObject.GetComponent<GearItem>();
					pickupitem(founditem);
				}

				int calorieCost = 0;
				if (itemcount != 0 && Settings.options.calorieCost != 0)
				{
					calorieCost = itemcount * Settings.options.calorieCost;
					GameManager.GetHungerComponent().RemoveReserveCalories(calorieCost);
				}

			}

		}

		public override void OnApplicationStart()
		{
			layerMask |= 1 << 17; // gear layer		
			QuickPick.Settings.OnLoad();
			loadCustomItemList();
		}
		public static void loadCustomItemList()
		{
			CustomList.Clear();
			if (!File.Exists("Mods\\QuickPickCustomList.txt"))
            {
				if (File.Exists("Mods\\StickPickCustomList.txt")) File.Move("Mods\\StickPickCustomList.txt", "Mods\\QuickPickCustomList.txt");
				else File.Create("Mods\\QuickPickCustomList.txt");
			}
				
			{
				using (StreamReader sr = File.OpenText("Mods\\QuickPickCustomList.txt"))
				{
					while (!sr.EndOfStream)
					{
						CustomList.Add(sr.ReadLine());
					}
					sr.Close();
				}

			}

			if (File.Exists("Mods\\QuickPickCustomList.txt"))
			{
				using (StreamReader sr = File.OpenText("Mods\\QuickPickCustomList.txt"))
				{
					while (!sr.EndOfStream)
					{
						CustomList.Add(sr.ReadLine());
					}
					sr.Close();
				}
				
			}
		}
	}
}
