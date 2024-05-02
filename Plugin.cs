using BepInEx;
using HarmonyLib;
using MTM101BaldAPI.Registers;
using PixelInternalAPI.Classes;
using PixelInternalAPI.Components;
using PixelInternalAPI.Extensions;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace PixelInternalAPI
{
	[BepInDependency("mtm101.rulerp.bbplus.baldidevapi", BepInDependency.DependencyFlags.HardDependency)]
	[BepInPlugin(ModInfo.PLUGIN_GUID, ModInfo.PLUGIN_NAME, ModInfo.PLUGIN_VERSION)]
	internal class BasePlugin : BaseUnityPlugin
	{
		private void Awake()
		{
			Harmony h = new(ModInfo.PLUGIN_GUID);
			h.PatchAll();

			LoadingEvents.RegisterOnAssetsLoaded(() =>
			{
				// ****** Soda machine *******
				GenericExtensions.FindResourceObjects<SodaMachine>().Do(x => {

					var comp = x.gameObject.AddComponent<SodaMachineCustomComponent>();
					comp.requiredItems.Add(Items.Quarter);
					_machines.Add(comp);
				});
				ObjectCreationExtensions.prefab = GenericExtensions.FindResourceObject<SodaMachine>();

				// ******* bill boards ********
				// Sprite Billboard object
				var baseSprite = new GameObject("SpriteBillBoard").AddComponent<SpriteRenderer>();
				baseSprite.material = GenericExtensions.FindResourceObjectByName<Material>("SpriteStandard_Billboard");
				baseSprite.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
				baseSprite.receiveShadows = false;

				baseSprite.gameObject.layer = LayerStorage.billboardLayer;
				DontDestroyOnLoad(baseSprite.gameObject);
				ObjectCreationExtensions._billboardprefab = baseSprite;


				// Sprite Non-Billboard object
				baseSprite = new GameObject("SpriteNoBillBoard").AddComponent<SpriteRenderer>();
				baseSprite.material = GenericExtensions.FindResourceObjectByName<Material>("SpriteStandard_NoBillboard");
				baseSprite.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
				baseSprite.receiveShadows = false;

				baseSprite.gameObject.layer = LayerStorage.billboardLayer;
				DontDestroyOnLoad(baseSprite.gameObject);
				ObjectCreationExtensions._nonbillboardprefab = baseSprite;

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

			// Put the necessary callbacks for prefabs
			ResourceManager.AddGeneratorStartCallback((_) => ResourceManager._prefabs.ForEach(x => x.SetActive(true)));
			ResourceManager.AddPostGenCallback((_) => ResourceManager._prefabs.ForEach(x => x.SetActive(false)));
		}

		internal static List<SodaMachineCustomComponent> _machines = [];

		readonly static FieldInfo sodaMachineItems = AccessTools.Field(typeof(SodaMachine), "potentialItems");
		readonly static FieldInfo _mysteryroom_items = AccessTools.Field(typeof(MysteryRoom), "items");
	}

	static class ModInfo
	{
		internal const string PLUGIN_GUID = "pixelguy.pixelmodding.baldiplus.pixelinternalapi";

		internal const string PLUGIN_NAME = "Pixel\'s Internal API";

		internal const string PLUGIN_VERSION = "1.0.5";
	}
}
