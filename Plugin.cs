using BepInEx;
using HarmonyLib;
using MTM101BaldAPI.Registers;
using PixelInternalAPI.Extensions;
using System.Collections.Generic;
using System.Reflection;

namespace PixelInternalAPI
{
	[BepInPlugin(ModInfo.PLUGIN_GUID, ModInfo.PLUGIN_NAME, ModInfo.PLUGIN_VERSION)]
	internal class BasePlugin : BaseUnityPlugin
	{
		private void Awake()
		{
			Harmony h = new(ModInfo.PLUGIN_GUID);
			h.PatchAll();

			LoadingEvents.RegisterOnAssetsLoaded(() => // Make items get into random vending machines
			{
				GenericExtensions.FindResourceObjects<SodaMachine>().DoIf(x => ((WeightedItemObject[])sodaMachineItems.GetValue(x)).Length > 1, x =>
				{
					var weighteds = (WeightedItemObject[])sodaMachineItems.GetValue(x);
					weighteds = weighteds.AddRangeToArray([.. ResourceManager._vendingMachineItems]);
					sodaMachineItems.SetValue(x, weighteds);

				}); // if > 1, then it must be a crazy machine
			}, true);
		}

		readonly static FieldInfo sodaMachineItems = AccessTools.Field(typeof(SodaMachine), "potentialItems");
	}

	static class ModInfo
	{
		public const string PLUGIN_GUID = "pixelguy.pixelmodding.baldiplus.pixelinternalapi";

		public const string PLUGIN_NAME = "Pixel\'s Internal API";

		public const string PLUGIN_VERSION = "1.0.4";
	}

	public static class ResourceManager
	{
		public static void AddWeightedItemToCrazyMachine(WeightedItemObject item) =>
			_vendingMachineItems.Add(item);

		static internal List<WeightedItemObject> _vendingMachineItems = [];
	}
}
