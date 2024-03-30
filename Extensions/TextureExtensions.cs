﻿using UnityEngine;

namespace PixelInternalAPI.Extensions
{
	public static class TextureExtensions
	{
		public static void OverlayTexture(this Texture2D t, Texture2D t2)
		{
			if (t.width != t2.width || t.height != t2.height)
				throw new System.ArgumentException($"Texture width or height don\'t match second texture ({t.width},{t.height} not equal to {t2.width},{t2.height})");

			Color[] colors = t.GetPixels();
			Color[] colors2 = t2.GetPixels();

			for (int i = 0; i < colors.Length; i++)
			{
				if (colors2[i].a < 1f)
					continue; // Only solid colors allowed

				colors[i] = colors2[i];
			}

			t.SetPixels(colors);
			t.Apply();
		}

		public static Texture2D MakeReadableTexture(this Texture2D source) // Credits to whoever replied the question in this post: https://stackoverflow.com/questions/44733841/how-to-make-texture2d-readable-via-script
		{
			RenderTexture renderTex = RenderTexture.GetTemporary(
						source.width,
						source.height,
						0,
						RenderTextureFormat.ARGB32,
						RenderTextureReadWrite.sRGB);

			Graphics.Blit(source, renderTex);
			RenderTexture previous = RenderTexture.active;
			RenderTexture.active = renderTex;
			Texture2D readableText = new(source.width, source.height);
			readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
			readableText.Apply();
			RenderTexture.active = previous;
			RenderTexture.ReleaseTemporary(renderTex);
			return readableText;
		}

		public static Texture2D CreateSolidTexture(int width, int height, Color solidColor)
		{
			var texture = new Texture2D(width, height);
			var colors = new Color[width * height];
			for (int i = 0; i < colors.Length; i++)
				colors[i] = solidColor;
			texture.SetPixels(colors);
			texture.Apply();
			return texture;
		}

		public static Cubemap CubemapFromTexture2D(Texture2D texture)
		{
			int cubemapWidth = texture.height / 6;
			Cubemap cubemap = new(cubemapWidth, TextureFormat.ARGB32, false);
			cubemap.SetPixels(texture.GetPixels(0, 0 * cubemapWidth, cubemapWidth, cubemapWidth), CubemapFace.NegativeZ);
			cubemap.SetPixels(texture.GetPixels(0, 1 * cubemapWidth, cubemapWidth, cubemapWidth), CubemapFace.PositiveZ);
			cubemap.SetPixels(texture.GetPixels(0, 2 * cubemapWidth, cubemapWidth, cubemapWidth), CubemapFace.PositiveY);
			cubemap.SetPixels(texture.GetPixels(0, 3 * cubemapWidth, cubemapWidth, cubemapWidth), CubemapFace.NegativeY);
			cubemap.SetPixels(texture.GetPixels(0, 4 * cubemapWidth, cubemapWidth, cubemapWidth), CubemapFace.NegativeX);
			cubemap.SetPixels(texture.GetPixels(0, 5 * cubemapWidth, cubemapWidth, cubemapWidth), CubemapFace.PositiveX);
			cubemap.Apply();
			return cubemap;
		}

		public static Texture2D GenerateTextureAtlas(Texture2D ceil, Texture2D wall, Texture2D floor)
		{
			var textureAtlas = new Texture2D(512, 512, TextureFormat.RGBA32, false)
			{
				filterMode = FilterMode.Point
			};
			var array = MaterialModifier.GetColorsForTileTexture(floor, 256);
			textureAtlas.SetPixels(0, 0, 256, 256, array);
			array = MaterialModifier.GetColorsForTileTexture(wall, 256);
			textureAtlas.SetPixels(256, 256, 256, 256, array);
			array = MaterialModifier.GetColorsForTileTexture(ceil, 256);
			textureAtlas.SetPixels(0, 256, 256, 256, array);
			textureAtlas.Apply();
			return textureAtlas;
		}
	}
}
