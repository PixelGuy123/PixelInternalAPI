using UnityEngine;

namespace PixelInternalAPI.Components
{
	/// <summary>
	/// A component to make the object rotate just like a billboard does.
	/// </summary>
    public class BillboardRotator : MonoBehaviour
    {
        private void LateUpdate()
        {
			if (mainCam != null)
				transform.forward = !invertFace ? mainCam.transform.forward : -mainCam.transform.forward;
			else if (Camera.main != null)
				mainCam = Camera.main;

        }
        Camera mainCam;

		/// <summary>
		/// When set to true, the object will rotate the opposite direction to the player.
		/// </summary>
		[SerializeField]
		public bool invertFace = false;
    }
}
