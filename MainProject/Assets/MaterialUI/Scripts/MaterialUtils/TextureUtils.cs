//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using UnityEngine;
using System.IO;

namespace MaterialUI
{
	public static class TextureUtils
	{
		public enum CropAnchor
		{
			LEFT,
			RIGHT,
			TOP,
			BOTTOM,
			CENTER,
		}
		
		public static Texture2D LoadCroppedFromFile(string filePath, int width, int height)
		{
			return LoadCroppedFromFile(filePath, width, height, TextureUtils.CropAnchor.CENTER);
		}
		
		public static Texture2D LoadCroppedFromFile(string filePath, int width, int height, CropAnchor anchor)
		{
			Texture2D thumbnail = TextureUtils.LoadFixedHeightFromFile(filePath, height, TextureFormat.R16);
			TextureUtils.Crop(thumbnail, width, height, anchor);
			return thumbnail;
		}
		
		// Load a texture with the specified height and calculating width by respecting the aspect ratio
		public static Texture2D LoadFixedHeightFromFile(string filePath, int fixedHeight, TextureFormat textureFormat = TextureFormat.R16)
		{
			Vector2 imageSize = ImageHelper.GetDimensions(filePath);
			float width = fixedHeight;
			if (imageSize.y != 0)
			{
				width = imageSize.x * fixedHeight / imageSize.y;
			}
			
			return LoadFromFile(filePath, (int)width, fixedHeight, textureFormat);
		}
		
		public static Texture2D LoadFromFile(string filePath, int width, int height, TextureFormat textureFormat = TextureFormat.R16)
		{
			// Load texture in memory
			byte[] fileData = File.ReadAllBytes(filePath);
			
			// Create texture for Unity and load it
			Texture2D texture = new Texture2D(2, 2, textureFormat, false);
			texture.LoadImage(fileData);
			
			// Resize the texture
			Resize(texture, width, height);
			
			return texture;
		}
		
		public static void Resize(Texture2D texture, int width, int height)
		{
			Color[] pixelArray = new Color[width * height];
			float incX = (1.0f / (float)width);
			float incY = (1.0f / (float)height); 
			
			for (int px = 0; px < pixelArray.Length; px++)
			{ 
				pixelArray[px] = texture.GetPixelBilinear(incX * ((float)px % width), incY * ((float)Mathf.Floor(px / width))); 
			}
			
			texture.Resize(width, height);
			texture.SetPixels(pixelArray, 0); 
			texture.Apply();
		}
		
		public static void Crop(Texture2D texture, int width, int height, CropAnchor anchor = CropAnchor.CENTER)
		{
			int resultPosX = 0;
			int resultPosY = 0;
			int resultWidth = 0;
			int resultHeight = 0;
			
			float ratioWidth = (float)width/(float)texture.width;
			float ratioHeight = (float)height/(float)texture.height;
			
			if (ratioHeight < ratioWidth)
			{
				resultWidth = width;
				resultHeight = (int)(texture.height * ratioWidth);
				
				switch(anchor)
				{
				case CropAnchor.TOP:
					resultPosY = (int)(resultHeight - height);
					break;
				case CropAnchor.BOTTOM:
					resultPosY = 0;
					break;
				default:
					resultPosY = (int)((resultHeight - height) * 0.5f);
					break;
				}
			}
			else
			{
				resultWidth  = (int)(texture.width * ratioHeight);
				resultHeight = height;
				
				switch(anchor)
				{
				case CropAnchor.LEFT:
					resultPosX = 0;
					break;
				case CropAnchor.RIGHT:
					resultPosX = (int)(resultWidth - width);
					break;
				default:
					resultPosX = (int)((resultWidth - width) * 0.5f);
					break;
				}
			}
			
			Resize(texture, resultWidth, resultHeight);
			Color[] pixelArray = texture.GetPixels(resultPosX, resultPosY, width, height);
			
			texture.Resize(width, height);
			texture.SetPixels(pixelArray, 0); 
			texture.Apply();
		}
	}
}
