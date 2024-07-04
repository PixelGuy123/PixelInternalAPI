using HarmonyLib;
using UnityEngine;

namespace PixelInternalAPI.Misc.VanillaFixes
{
    [HarmonyPatch(typeof(ItemManager))]
    internal class ItemManagerPatch
    {
        [HarmonyPatch("AddItem", [typeof(ItemObject)])]
        [HarmonyPatch("SetItem")]
        [HarmonyPrefix]
        static bool IsInstantUse(ItemObject item, PlayerManager ___pm)
        {
            if (!item.addToInventory)
            {
                Object.Instantiate(item.item).Use(___pm);
                return false;
            }
            return true;
        }
    }
}
