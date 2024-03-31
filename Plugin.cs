using BepInEx;
using HarmonyLib;

namespace PixelInternalAPI
{
	[BepInPlugin(ModInfo.PLUGIN_GUID, ModInfo.PLUGIN_NAME, ModInfo.PLUGIN_VERSION)]
	internal class BasePlugin : BaseUnityPlugin
	{
		private void Awake()
		{
			Harmony h = new(ModInfo.PLUGIN_GUID);
			h.PatchAll();
		}
	}

	static class ModInfo
	{
		public const string PLUGIN_GUID = "pixelguy.pixelmodding.baldiplus.pixelinternalapi";

		public const string PLUGIN_NAME = "Pixel\'s Internal API";

		public const string PLUGIN_VERSION = "1.0.1";
	}
}
