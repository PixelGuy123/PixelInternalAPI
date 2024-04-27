using PixelInternalAPI.Classes;
using System.Collections.Generic;
using UnityEngine;

namespace PixelInternalAPI.Components
{
	/// <summary>
	/// A component added to NPCs to modify some aspects from them.
	/// </summary>
	public class NPCAttributesContainer : MonoBehaviour
	{
		// ******** Looker stuff **********
		float originalDistance = 0f;
		Looker looker;

		internal List<BaseModifier> lookermods = [];
		/// <summary>
		/// Checks if the <paramref name="modifier"/> exists in the looker mods.
		/// </summary>
		/// <param name="modifier"></param>
		/// <returns>true if <paramref name="modifier"/> exists inside the looker mods, otherwise false.</returns>
		public bool HasLookerMod(BaseModifier modifier) => lookermods.Contains(modifier);

		/// <summary>
		/// Adds a <paramref name="mod"/> to the looker mods.
		/// </summary>
		/// <param name="mod"></param>
		public void AddLookerMod(BaseModifier mod)
		{
			lookermods.Add(mod);
			UpdateLooker();
		}
		/// <summary>
		/// Removes a <paramref name="mod"/> to the looker mods.
		/// </summary>
		/// <param name="mod"></param>
		public void RemoveLookerMod(BaseModifier mod)
		{
			lookermods.Remove(mod);
			UpdateLooker();
		}

		void UpdateLooker()
		{
			float val = originalDistance;
			lookermods.ForEach(x => val *= x.Mod);
			looker.distance = Mathf.Max(0f, val);
		}

		private void Awake()
		{
			var l = GetComponent<Looker>();
			if (l)
			{
				looker = l;
				originalDistance = l.distance;
			}
			else
				Debug.LogWarning($"The object: {name} doesn\'t have a Looker component");
		}
	}
}
