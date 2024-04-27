

using UnityEngine;

namespace PixelInternalAPI.Components
{
	/// <summary>
	/// Basically an <see cref="ITM_Acceptable"/> but isn't wasted after being used.
	/// </summary>
	public class ITM_AcceptableNoUse : Item // Copy paste, but always return false to be a persistent item
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="pm"></param>
		/// <returns></returns>
		public override bool Use(PlayerManager pm)
		{
			if (Physics.Raycast(pm.transform.position, Singleton<CoreGameManager>.Instance.GetCamera(pm.playerNumber).transform.forward, out var hit, pm.pc.reach, pm.pc.ClickLayers))
			{
				foreach (IItemAcceptor itemAcceptor in hit.transform.GetComponents<IItemAcceptor>())
				{
					if (itemAcceptor != null && itemAcceptor.ItemFits(item))
					{
						itemAcceptor.InsertItem(pm, pm.ec);
						Destroy(gameObject);
						if (audUse != null)
						{
							Singleton<CoreGameManager>.Instance.audMan.PlaySingle(audUse);
						}
						return false;
					}
				}
			}
			Destroy(gameObject);
			return false;
		}

		/// <summary>
		/// The <see cref="Items"/> enum it should represent.
		/// </summary>
		[SerializeField]
		public Items item;

		/// <summary>
		/// The noise it does when used (optional).
		/// </summary>
		[SerializeField]
		public SoundObject audUse = null;

	}
}
