using System.Collections.Generic;
using PixelInternalAPI.Misc;

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
		/// Adds the <paramref name="item"/> to be able of unlocking doors (just like keys).
		/// </summary>
		/// <param name="item"></param>
		public static void AddKeyTypeItem(this ItemObject item) => _unlockableItems.Add(item.itemType);


		readonly static internal List<WeightedItemObject> _vendingMachineItems = [];
		readonly static internal List<WeightedItemObject> _mysteryItems = [];
		readonly static internal HashSet<Items> _unlockableItems = [Items.DetentionKey];

		// ************************************************ Callbacks *************************************************

		/// <summary>
		/// Registers a callback after a level is generated/loaded.
		/// </summary>
		/// <param name="call">The action that is going to be taken when the callback is triggered</param>
		public static void AddPostGenCallback(System.Action<BaseGameManager> call) =>
			OnPostGen += (BaseGameManager) => call(BaseGameManager); // Workaround with lambda (I seriously don't wanna make those delegates public lmao)
		/// <summary>
		/// Registers a callback before the level starts generating/loading.
		/// </summary>
		/// <param name="call">The action that is going to be taken when the callback is triggered</param>
		public static void AddGenStartCallback(System.Action<GameInitializer, LevelBuilder, BaseGameManager, SceneObject> call) =>
		OnGeneratorStart += (a, b, c, d) => call(a, b, c, d); // Workaround with lambda (I seriously don't wanna make those delegates public lmao)
		/// <summary>
		/// Registers a callback when the level is reloaded (for either, going to the next floor or reloading it).
		/// </summary>
		/// <param name="call">The action that is going to be taken when the callback is triggered</param>
		public static void AddReloadLevelCallback(System.Action<BaseGameManager, bool> call) =>
			OnNextLevel += (BaseGameManager, next) => call(BaseGameManager, next);


		internal delegate void PostGen(BaseGameManager sender);
		internal static event PostGen OnPostGen;
		internal static void RaisePostGen(BaseGameManager sender)
		{
			BasePlugin.GlobalLogger.LogInfo("Invoking Post Gen");
			OnPostGen?.Invoke(sender);
		}
		internal delegate void GenStart(GameInitializer sender, LevelBuilder lb, BaseGameManager gm, SceneObject sceneObj);
		internal static event GenStart OnGeneratorStart;
		internal static void RaiseGenStart(GameInitializer sender, LevelBuilder lb, BaseGameManager gm, SceneObject sceneObj)
		{
			BasePlugin.GlobalLogger.LogInfo("Invoking Generator Start");
			OnGeneratorStart?.Invoke(sender, lb, gm, sceneObj);
		}

		internal delegate void ReloadLevel(BaseGameManager sender, bool goingNextLevel);
		internal static event ReloadLevel OnNextLevel;
		internal static void RaiseNextLevel(BaseGameManager sender, bool nextlevel)
		{
			BasePlugin.GlobalLogger.LogInfo($"Invoking Next Level (Next Level: {nextlevel})");
			OnNextLevel?.Invoke(sender, nextlevel);
		}

		// ********************************** Popups **************************************
		/// <summary>
		/// Raises a popup in the right bottom of the screen.
		/// </summary>
		/// <param name="info">The plugin that is calling this.</param>
		/// <param name="message">The message to be raised.</param>
		public static void RaisePopup(BepInEx.PluginInfo info, string message) =>
			man.QueuePopup(info, message, false);
		
		/// <summary>
		/// Raises a popup in the right bottom of the screen.
		/// </summary>
		/// <param name="info">The plugin that is calling this.</param>
		/// <param name="messageKey">The localized message to be raised</param>
		public static void RaiseLocalizedPopup(BepInEx.PluginInfo info, string messageKey) =>
			man.QueuePopup(info, messageKey, true);
		

		internal static PopupManager man;
	}
}
