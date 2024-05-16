using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using PixelInternalAPI.Extensions;

namespace PixelInternalAPI.Misc
{
	internal class Popup : MonoBehaviour
	{
		[SerializeField]
		internal TextMeshProUGUI title;
		[SerializeField]
		internal TextMeshProUGUI desc;
		[SerializeField]
		internal Canvas render;
		[SerializeField]
		internal Image image;
	}

	internal struct PopupInfo(string title, string message, bool isLocalized = true)
	{
		public string Title = title;
		string _message = message;
		readonly bool _isLocalized = isLocalized;

		public string Message { readonly get => _isLocalized ? Singleton<LocalizationManager>.Instance.GetLocalizedText(_message) : _message; set => _message = value; }
	}

	internal class PopupManager : MonoBehaviour
	{
		[SerializeField]
		internal Popup popupPrefab;

		internal Queue<PopupInfo> messagesToQueue = [];

		internal void QueuePopup(BepInEx.PluginInfo plug, string message, bool isLocalized) =>
			messagesToQueue.Enqueue(new(plug.Metadata.Name, message, isLocalized));
		

		void Update()
		{
			if (popupActive || messagesToQueue.Count == 0) return;

			popupActive = true;
			StartCoroutine(PopupAnimation(messagesToQueue.Dequeue()));
		}

		IEnumerator PopupAnimation(PopupInfo info)
		{
			var popup = Instantiate(popupPrefab);
			popup.render.worldCamera = Singleton<GlobalCam>.Instance.cam;
			popup.title.text = info.Title;
			popup.desc.text = info.Message;
			DontDestroyOnLoad(popup.gameObject);

			yield return null;

			if (popup.desc.textInfo.lineCount >= 10)
				popup.desc.fontSize = Mathf.Max(popup.desc.fontSizeMin, GenericExtensions.LinearEquation(popup.desc.textInfo.lineCount, -0.5f, 11.5f));
			
			

			var pop = popup.image;
			pop.transform.localPosition = new(238.83f, -239.29f); // Popup appearance at y -133.76f

			Vector3 pos = pop.transform.localPosition;
			Vector3 target = new(238.83f, -133.76f);
			float time = 0f;

			while (true)
			{
				time += 3f * Time.unscaledDeltaTime;
				if (time > 1f)
					break;

				pop.transform.localPosition = Vector3.Lerp(pos, target, time);
				yield return null;
			}

			pop.transform.localPosition = target;

			time = popup.desc.textInfo.wordCount / 5f;
			while (time > 0f)
			{
				time -= Time.unscaledDeltaTime;
				yield return null;
			}

			pos = target;
			target = new(238.83f, -239.29f);
			time = 0f;

			while (true)
			{
				time += 3f * Time.unscaledDeltaTime;
				if (time > 1f)
					break;

				pop.transform.localPosition = Vector3.Lerp(pos, target, time);
				yield return null;
			}


			Destroy(popup.gameObject);
			popupActive = false;

			yield break;
		}

		bool popupActive = false;
	}
}
