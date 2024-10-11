using UnityEngine;

namespace PixelInternalAPI.Components
{
	internal class PlayerClickDisabler : MonoBehaviour
	{
		internal void AttachToClicker(PlayerClick clicker)
		{
			this.clicker = clicker;
			originalReach = clicker.reach;
		}
		
		internal void DisableClick(bool disable)
		{
			if (disable)
				disables++;
			else
				disables--;

			if (disables < 0)
				disables = 0;

			UpdateClickState();
		}

		void UpdateClickState()
		{
			if (clicker)
				clicker.reach = IsDisabled ? -1 : originalReach;
		}

		[SerializeField]
		PlayerClick clicker;

		[SerializeField]
		float originalReach = 0; // No safe implementation because who the heck would want to override the reach like that

		int disables = 0;
		internal bool IsDisabled => disables > 0;
	}
}
