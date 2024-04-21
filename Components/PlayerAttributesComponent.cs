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

		readonly public List<StaminaModifier> StaminaMods = [];

		readonly public List<SpeedModifier> SpeedMods = [];

		void Update()
		{
			float wspeed = walkSpeed, rspeed = runSpeed;

			for (int i = 0; i < SpeedMods.Count; i++)
			{
				if (!SpeedMods[i].Adder)
				{
					wspeed *= SpeedMods[i].WalkSpeed;
					rspeed *= SpeedMods[i].RunSpeed;
				}
				else
				{
					wspeed += SpeedMods[i].WalkSpeed;
					rspeed += SpeedMods[i].RunSpeed;
				}
			}

			plm.walkSpeed = wspeed;
			plm.runSpeed = rspeed;

			float sRise = staminaRise, sDrop = staminaDrop, sMax = staminaMax;

			for (int i = 0; i < StaminaMods.Count; i++)
			{
				sRise *= StaminaMods[i].StaminaRiseMod;
				sDrop *= StaminaMods[i].StaminaDropMod;
				sMax *= StaminaMods[i].StaminaMaxMod;
			}

			plm.staminaDrop = sDrop;
			plm.staminaMax = sMax;
			plm.staminaRise = sRise;

		}

		PlayerMovement plm;

		float walkSpeed, runSpeed, staminaRise, staminaDrop, staminaMax;
	}
}
