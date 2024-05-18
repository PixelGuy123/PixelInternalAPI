using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace PixelInternalAPI.Extensions
{
	/// <summary>
	/// Various extension methods for textures
	/// </summary>
	public static class TextureExtensions
	{
		/// <summary>
		/// Puts a secondary texture (<paramref name="t2"/>) over the first texture (<paramref name="t"/>)
		/// </summary>
		/// <param name="t"></param>
		/// <param name="t2"></param>
		/// <exception cref="System.ArgumentException"></exception>
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
		/// <summary>
		/// Take a unreadable <see cref="Texture2D"/> and converts into a readable one (editable one).
		/// </summary>
		/// <param name="source"></param>
		/// <returns>A readable <see cref="Texture2D"/></returns>
		public static Texture2D MakeReadableTexture(this Texture2D source) // copypaste from texture pack now lol
		{
			RenderTexture dummyTex = RenderTexture.GetTemporary(source.width, source.height, 24);
			Texture2D toDump = source;

			toDump = new(toDump.width, toDump.height, toDump.format, toDump.mipmapCount > 1)
			{
				name = source.name
			};
			Graphics.Blit(source, dummyTex);
			toDump.filterMode = source.filterMode;
			dummyTex.filterMode = source.filterMode;

			toDump.ReadPixels(new Rect(0, 0, dummyTex.width, dummyTex.height), 0, 0);
			toDump.Apply();
			RenderTexture.ReleaseTemporary(dummyTex);

			return toDump;
		}
		/// <summary>
		/// Creates a <see cref="Texture2D"/> with a <paramref name="solidColor"/> defined.
		/// </summary>
		/// <param name="width">The width of the texture.</param>
		/// <param name="height">The height of the texture.</param>
		/// <param name="solidColor">The fill up color for the texture.</param>
		/// <returns><see cref="Texture2D"/> with a <paramref name="solidColor"/> defined.</returns>
		/// <exception cref="System.ArgumentException"></exception>
		public static Texture2D CreateSolidTexture(int width, int height, Color solidColor)
		{
			if (width <= 0 || height <= 0)
				throw new System.ArgumentException($"width or height is below 1 ({width},{height})");

			var texture = new Texture2D(width, height);
			var colors = new Color[width * height];
			for (int i = 0; i < colors.Length; i++)
				colors[i] = solidColor;
			texture.SetPixels(colors);
			texture.Apply();
			return texture;
		}
		/// <summary>
		/// Adds an outline to the <paramref name="tex"/> with a outline width of 1.
		/// </summary>
		/// <param name="tex">The target texture.</param>
		/// <param name="outlineColor">The color of the outline.</param>
		/// <returns>The same texture but with an outline.</returns>
		/// <exception cref="System.ArgumentException"></exception>
		public static Texture2D AddTextureOutline(this Texture2D tex, Color outlineColor) =>
			tex.AddTextureOutline(outlineColor, 1);
		/// <summary>
		/// Adds an outline to the <paramref name="tex"/>.
		/// </summary>
		/// <param name="tex">The target texture.</param>
		/// <param name="outlineColor">The color of the outline.</param>
		/// <param name="outlineWidth">The outline's width</param>
		/// <returns>The same texture but with an outline.</returns>
		/// <exception cref="System.ArgumentException"></exception>
		public static Texture2D AddTextureOutline(this Texture2D tex, Color outlineColor, int outlineWidth)
		{
			if (outlineWidth <= 0)
				throw new System.ArgumentException("The outline width is below or equal to 0");
			if (outlineWidth > tex.width || outlineWidth > tex.height)
				throw new System.ArgumentException($"The outline width is higher than the texture\'s width or height (Texture Width: {tex.width}; Texture Height: {tex.height}; Outline Width: {outlineWidth})");
			var colors = tex.GetPixels();
			for (int i = 0; i < outlineWidth; i++)
			{
				// right
				int x = 0;
				for (; x < tex.width; x++)
					colors[x + (i * tex.width)] = outlineColor;
				// up (right)
				for (; x < colors.Length; x += tex.width)
					colors[x - i] = outlineColor;
				
				// left (up)
				for (x = colors.Length - 1; x > colors.Length - tex.width; x--)
					colors[x - (i * tex.width)] = outlineColor;
				

				// down (left + up)
				for (; x > 0; x -= tex.width)
					colors[x + i] = outlineColor;
			}
			tex.SetPixels(colors);
			tex.Apply();
			return tex;
		}
		/// <summary>
		/// Creates a Cubemap from <paramref name="texture"/>
		/// </summary>
		/// <param name="texture"></param>
		/// <returns>Cubemap from <paramref name="texture"/></returns>
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
		/// <summary>
		/// Generates the default texture atlas BB+ uses for rooms.
		/// </summary>
		/// <param name="ceil"></param>
		/// <param name="wall"></param>
		/// <param name="floor"></param>
		/// <returns>default texture atlas BB+ uses for rooms.</returns>
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
