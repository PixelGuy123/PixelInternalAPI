using PixelInternalAPI.Classes;
using System.Collections.Generic;
using UnityEngine;

namespace PixelInternalAPI.Components
{
	public class PlayerAttributesComponent : MonoBehaviour
	{ 
		void Awake()
		{
			plm = GetComponent<PlayerMovement>();
			walkSpeed = plm.walkSpeed;
			runSpeed = plm.runSpeed;
			staminaDrop = plm.staminaDrop;
			staminaMax = plm.staminaMax;
			staminaRise = plm.staminaRise;
		}

		private readonly Dictionary<string, int> attributes = [];
		public void AddAttribute(string s) 
		{
			if (attributes.ContainsKey(s))
				attributes[s]++;
			else
				attributes.Add(s, 1);
		}
		public void RemoveAttribute(string s)
		{
			if (attributes.ContainsKey(s))
			{
				int num = --attributes[s];
				if (num <= 0)
					attributes.Remove(s);
			}
		}
		public bool HasAttribute(string s) => attributes.ContainsKey(s);

		readonly private List<StaminaModifier> staminaMods = [];
		public void AddStaminaMod(StaminaModifier mod)
		{
			staminaMods.Add(mod);
			UpdateMovement();
		}

		public void RemoveStaminaMod(StaminaModifier mod)
		{
			staminaMods.Remove(mod);
			UpdateMovement();
		}

		readonly private List<BaseModifier> speedMods = [];

		public void AddSpeedMod(BaseModifier mod)
		{
			speedMods.Add(mod);
			UpdateMovement();
		}

		public void RemoveSpeedMod(BaseModifier mod)
		{
			speedMods.Remove(mod);
			UpdateMovement();
		}

		void UpdateMovement()
		{
			float wspeed = walkSpeed, rspeed = runSpeed;

			for (int i = 0; i < speedMods.Count; i++)
			{
				wspeed *= speedMods[i].Mod;
				rspeed *= speedMods[i].Mod;
			}

			plm.walkSpeed = wspeed;
			plm.runSpeed = rspeed;

			float sRise = staminaRise, sDrop = staminaDrop, sMax = staminaMax;

			for (int i = 0; i < staminaMods.Count; i++)
			{
				sRise *= staminaMods[i].StaminaRiseMod;
				sDrop *= staminaMods[i].StaminaDropMod;
				sMax *= staminaMods[i].StaminaMaxMod;
			}

			plm.staminaDrop = sDrop;
			plm.staminaMax = sMax;
			plm.staminaRise = sRise;

		}

		PlayerMovement plm;

		float walkSpeed, runSpeed, staminaRise, staminaDrop, staminaMax;
	}
}
