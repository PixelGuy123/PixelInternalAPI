using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using MTM101BaldAPI;
using MTM101BaldAPI.Registers;
using PixelInternalAPI.Classes;
using PixelInternalAPI.Components;
using PixelInternalAPI.Extensions;
using System.Collections;
using System.Collections.Generic;
using PixelInternalAPI.Misc;
using TMPro;
#if DEBUG
using MTM101BaldAPI.PlusExtensions;
using MTM101BaldAPI.Components;
using System.Reflection.Emit;
#endif

using UnityEngine;

namespace PixelInternalAPI
{
	[BepInDependency("mtm101.rulerp.bbplus.baldidevapi", BepInDependency.DependencyFlags.HardDependency)]
	[BepInPlugin(ModInfo.PLUGIN_GUID, ModInfo.PLUGIN_NAME, ModInfo.PLUGIN_VERSION)]
	internal class BasePlugin : BaseUnityPlugin
	{
		private void Awake()
		{
			GlobalLogger = Logger;

			Harmony h = new(ModInfo.PLUGIN_GUID);
			h.PatchAll();

			LoadingEvents.RegisterOnAssetsLoaded(Info, GetBaseAssets(), false);

			LoadingEvents.RegisterOnAssetsLoaded(Info, AddAssetsInGame(), true);

			ResourceManager.AddPostGenCallback((x) =>
			{
				foreach (var machine in FindObjectsOfType<SodaMachine>())
				{
					var comp = machine.GetComponent<SodaMachineCustomComponent>();
					if (comp && !_machines.Contains(comp)) // If the machine's component wasn't found in the register, this wasn't made using the api standard methods, so should be deleted
						Destroy(comp);
				}
			});
		}

		internal static List<SodaMachineCustomComponent> _machines = [];

		//readonly static FieldInfo sodaMachineItems = AccessTools.Field(typeof(SodaMachine), "potentialItems");
		//readonly static FieldInfo _mysteryroom_items = AccessTools.Field(typeof(MysteryRoom), "items");
		internal static ManualLogSource GlobalLogger;

		// *************** Asset Load IEnumerators ***************
		IEnumerator GetBaseAssets()
		{
			yield return 3;
			yield return "Grabbing soda machines for creation";
			// ****** Soda machine *******
			GenericExtensions.FindResourceObjects<SodaMachine>().Do(x => {

				var comp = x.gameObject.AddComponent<SodaMachineCustomComponent>();
				comp.requiredItems.Add(Items.Quarter);
				_machines.Add(comp);
			});
			ObjectCreationExtensions.prefab = GenericExtensions.FindResourceObject<SodaMachine>().DuplicatePrefab();

			yield return "Getting sprite billboard prefabs";

			// ******* bill boards ********
			// Sprite Billboard object
			var baseSprite = new GameObject("SpriteBillBoard").AddComponent<SpriteRenderer>();
			baseSprite.material = GenericExtensions.FindResourceObjectByName<Material>("SpriteStandard_Billboard");
			baseSprite.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			baseSprite.receiveShadows = false;

			baseSprite.gameObject.layer = LayerStorage.billboardLayer;
			baseSprite.gameObject.ConvertToPrefab(true);
			ObjectCreationExtensions._billboardprefab = baseSprite;


			// Sprite Non-Billboard object
			baseSprite = new GameObject("SpriteNoBillBoard").AddComponent<SpriteRenderer>();
			baseSprite.material = GenericExtensions.FindResourceObjectByName<Material>("SpriteStandard_NoBillboard");
			baseSprite.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			baseSprite.receiveShadows = false;

			baseSprite.gameObject.layer = LayerStorage.billboardLayer;
			baseSprite.gameObject.ConvertToPrefab(true);
			ObjectCreationExtensions._nonbillboardprefab = baseSprite;

			// ******** Popups ********
			yield return "Creating in-game popups";

			var canvas = ObjectCreationExtensions.CreateCanvas();
			canvas.gameObject.ConvertToPrefab(true);
			canvas.name = "PopupCanvas";

			var popup = ObjectCreationExtensions.CreateImage(canvas, TextureExtensions.CreateSolidTexture(360, 360, Color.white).AddTextureOutline(Color.black, 3), false);
			popup.name = "PopupImage";

			var titleText = ObjectCreationExtensions.CreateTextMeshProUGUI(Color.black);
			titleText.alignment = TextAlignmentOptions.Center;
			titleText.fontWeight = FontWeight.Bold;
			titleText.fontSizeMax = 9;
			titleText.fontSize = 9;
			titleText.fontSizeMin = 5;
			titleText.enableAutoSizing = true;
			titleText.characterSpacing = 5;
			titleText.enableWordWrapping = true;

			titleText.transform.SetParent(popup.transform);
			titleText.transform.localPosition = Vector3.up * 39.98f;
			titleText.rectTransform.sizeDelta = new(100f, 50f);
			titleText.name = "PopupTitle";

			var descText = ObjectCreationExtensions.CreateTextMeshProUGUI(Color.black);
			descText.horizontalAlignment = HorizontalAlignmentOptions.Center;
			descText.verticalAlignment = VerticalAlignmentOptions.Top;

			descText.transform.SetParent(popup.transform);
			descText.transform.localPosition = Vector3.down * 119.6f;
			descText.name = "PopupDescription";

			descText.fontSize = 7;
			descText.fontSizeMax = 7;
			descText.fontSizeMin = 4;
			descText.characterSpacing = 3;
			descText.enableWordWrapping = true;
			descText.rectTransform.sizeDelta = new(100f, 300f);


			var popupComp = canvas.gameObject.AddComponent<Popup>();
			popupComp.render = canvas;
			popupComp.desc = descText;
			popupComp.title = titleText;
			popupComp.image = popup;

			ResourceManager.man = new GameObject("PopupManager").AddComponent<PopupManager>();
			DontDestroyOnLoad(ResourceManager.man.gameObject);

			ResourceManager.man.popupPrefab = popupComp;

			yield break;
		}

		IEnumerator AddAssetsInGame()
		{
			yield return 2;
			yield return "Adding item table to crazy vending machines";
			GenericExtensions.FindResourceObjects<SodaMachine>().DoIf(x => x.potentialItems.Length > 1, x => x.potentialItems = x.potentialItems.AddRangeToArray([.. ResourceManager._vendingMachineItems])); // if > 1, then it must be a crazy machine


			yield return "Adding item table to mystery rooms";
			GenericExtensions.FindResourceObjects<MysteryRoom>().Do(x => x.items = x.items.AddRangeToArray([.. ResourceManager._mysteryItems]));

			yield break;
		}
	}


#if DEBUG

	[HarmonyPatch(typeof(HappyBaldi), "SpawnWait", MethodType.Enumerator)]
	internal static class QuickCheatBox
	{
		[HarmonyTranspiler]
		internal static IEnumerable<CodeInstruction> Zero(IEnumerable<CodeInstruction> instructions) =>
			new CodeMatcher(instructions)
			.MatchForward(false, new CodeMatch(OpCodes.Ldc_I4_S, name:"9"))
			.Set(OpCodes.Ldc_I4_0, null)
			.InstructionEnumeration();
	}
	[HarmonyPatch(typeof(Baldi), "CaughtPlayer")]
	internal static class QuickBaldiNoDeath
	{
		[HarmonyPrefix]
		internal static bool NoDeath() => false;
	}
	[HarmonyPatch(typeof(BaseGameManager), "Initialize")]
	internal static class AlwaysFullMap
	{
		private static void Prefix(BaseGameManager __instance)
		{
			__instance.CompleteMapOnReady();
		}
	}
	[HarmonyPatch(typeof(PlayerMovement))]
	internal class Fast
	{

		[HarmonyPatch("Start")]
		[HarmonyPostfix]
		private static void PointsYeah(PlayerMovement __instance) =>
			Singleton<CoreGameManager>.Instance.AddPoints(9999999, __instance.pm.playerNumber, true);

		[HarmonyPatch("Update")]
		[HarmonyPostfix]
		private static void GottaGoFAST(PlayerMovement __instance)
		{
			var comp = __instance.pm.GetMovementStatModifier();
			if (Input.GetKeyDown(KeyCode.K))
			{
				if (!hasAdded)
				{
					comp.AddModifier("walkSpeed", wmod);
					comp.AddModifier("runSpeed", rmod);
				}
				else
				{
					comp.RemoveModifier(wmod);
					comp.RemoveModifier(rmod);
				}
				hasAdded = !hasAdded;
			}
		}

		readonly static ValueModifier wmod = new(3);
		readonly static ValueModifier rmod = new(3);

		static bool hasAdded = false;

	}
#endif
	static class ModInfo
	{
		internal const string PLUGIN_GUID = "pixelguy.pixelmodding.baldiplus.pixelinternalapi";

		internal const string PLUGIN_NAME = "Pixel\'s Internal API";

		internal const string PLUGIN_VERSION = "1.2.4";
	}
}
