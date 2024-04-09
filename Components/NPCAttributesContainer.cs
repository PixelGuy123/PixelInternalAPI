using PixelInternalAPI.Classes;
using System.Collections.Generic;
using UnityEngine;

namespace PixelInternalAPI.Components
{
	public class NPCAttributesContainer : MonoBehaviour
	{
		// ******** Looker stuff **********
		float originalDistance = 0f;
		Looker looker;

		internal List<BaseModifier> lookermods = [];

		public bool HasLookerMod(BaseModifier modifier) => lookermods.Contains(modifier);

		public void AddLookerMod(BaseModifier mod)
		{
			lookermods.Add(mod);
			UpdateLooker();
		}

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
