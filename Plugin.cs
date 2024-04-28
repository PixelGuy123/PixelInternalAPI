using BepInEx;
using HarmonyLib;
using MTM101BaldAPI.Registers;
using PixelInternalAPI.Components;
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

			LoadingEvents.RegisterOnAssetsLoaded(() =>
			{
				GenericExtensions.FindResourceObjects<SodaMachine>().Do(x => {

					var comp = x.gameObject.AddComponent<SodaMachineCustomComponent>();
					comp.requiredItems.Add(Items.Quarter);

				});
				ObjectCreationExtensions.prefab = GenericExtensions.FindResourceObject<SodaMachine>();
			}, false);

			LoadingEvents.RegisterOnAssetsLoaded(() => // Make items get into random vending machines
			{
				GenericExtensions.FindResourceObjects<SodaMachine>().DoIf(x => ((WeightedItemObject[])sodaMachineItems.GetValue(x)).Length > 1, x =>
				{
					var weighteds = (WeightedItemObject[])sodaMachineItems.GetValue(x);
					weighteds = weighteds.AddRangeToArray([.. ResourceManager._vendingMachineItems]);
					sodaMachineItems.SetValue(x, weighteds);

				}); // if > 1, then it must be a crazy machine
				GenericExtensions.FindResourceObjects<MysteryRoom>().Do(x => // adding mystery items to... mystery room
				{
					WeightedItemObject[] items = (WeightedItemObject[])_mysteryroom_items.GetValue(x);
					items = items.AddRangeToArray([.. ResourceManager._mysteryItems]);
					_mysteryroom_items.SetValue(x, items);
				});
			}, true);
		}

		readonly static FieldInfo sodaMachineItems = AccessTools.Field(typeof(SodaMachine), "potentialItems");
		readonly static FieldInfo _mysteryroom_items = AccessTools.Field(typeof(MysteryRoom), "items");
	}

	static class ModInfo
	{
		internal const string PLUGIN_GUID = "pixelguy.pixelmodding.baldiplus.pixelinternalapi";

		internal const string PLUGIN_NAME = "Pixel\'s Internal API";

		internal const string PLUGIN_VERSION = "1.0.4";
	}
	/// <summary>
	/// A basic class that store some info that can be useful for the game.
	/// </summary>
	public static class ResourceManager // Resources stuff I guess? Will be stored here
	{
		/// <summary>
		/// Adds an weighted item to the crazy vending machine loot table. Note: This must be called at RegisterOnAssetsLoaded from the LoadingEvents, non-post).
		/// </summary>
		/// <param name="item"></param>
		public static void AddWeightedItemToCrazyMachine(WeightedItemObject item) => // yay
			_vendingMachineItems.Add(item);

		/// <summary>
		/// Adds an weighted item to the <see cref="MysteryRoom"/> event loot table. Note: This must be called at RegisterOnAssetsLoaded from the LoadingEvents, non-post).
		/// </summary>
		/// <param name="item"></param>
		public static void AddMysteryItem(WeightedItemObject item) =>
			_mysteryItems.Add(item);

		static internal List<WeightedItemObject> _vendingMachineItems = [];
		static internal List<WeightedItemObject> _mysteryItems = [];
	}
}
