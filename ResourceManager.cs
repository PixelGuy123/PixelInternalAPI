using System.Collections.Generic;
using UnityEngine;

namespace PixelInternalAPI
{
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
		/// <summary>
		/// Adds the <paramref name="item"/> to all the vending machines (created with this api) to accept the <paramref name="item"/> as a quarter.
		/// </summary>
		/// <param name="item"></param>
		public static void AddQuaterTypeItemToVendingMachine(this Items item) => BasePlugin._machines.ForEach(x => x.requiredItems.Add(item));

		static internal List<WeightedItemObject> _vendingMachineItems = [];
		static internal List<WeightedItemObject> _mysteryItems = [];

		// ******************************* Prefabs **********************************

		/// <summary>
		/// Adds the <paramref name="obj"/> as a "prefab", in other words, an object that will be enabled in the generator and disabled afterwards.
		/// <para>This is useful for objects that are builded through <see cref="ObjectBuilder"/> for example.</para>
		/// </summary>
		/// <param name="obj"></param>
		public static GameObject AddAsGeneratorPrefab(this GameObject obj)
		{
			_prefabs.Add(obj);
			return obj; // builder pattern lol
		}

		static internal List<GameObject> _prefabs = [];

		// ************************************************ Callbacks *************************************************

		/// <summary>
		/// Adds a <paramref name="call"/> that's triggered after a level is generated/loaded.
		/// </summary>
		/// <param name="call"></param>
		public static void AddPostGenCallback(System.Action<BaseGameManager> call) =>
			OnPostGen += (BaseGameManager) => call(BaseGameManager); // Workaround with lambda (I seriously don't wanna make those delegates public lmao)
		/// <summary>
		/// Adds a <paramref name="call"/> that's triggered when the level is reloaded (for either, going to the next floor or reloading it).
		/// </summary>
		/// <param name="call"></param>
		public static void AddReloadLevelCallback(System.Action<BaseGameManager> call) =>
			OnNextLevel += (BaseGameManager) => call(BaseGameManager);
		/// <summary>
		/// Adds a <paramref name="call"/> that's triggered when a level begins the generation/load process.
		/// </summary>
		/// <param name="call"></param>
		public static void AddGeneratorStartCallback(System.Action<GameInitializer> call) =>
			OnGameStart += (ini) => call(ini); // Workaround with lambda (I seriously don't wanna make those delegates public lmao)


		internal delegate void PostGen(BaseGameManager sender);
		internal static event PostGen OnPostGen;
		internal static event PostGen OnNextLevel;
		internal static void RaisePostGen(BaseGameManager sender) =>
			OnPostGen?.Invoke(sender);
		internal static void RaiseNextLevel(BaseGameManager sender) =>
			OnNextLevel?.Invoke(sender);

		internal delegate void GameInitialization(GameInitializer initializer);
		internal static event GameInitialization OnGameStart;
		internal static void RaiseGameStart(GameInitializer ga) =>
			OnGameStart?.Invoke(ga);

		
	}
}
