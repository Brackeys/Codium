//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

// From: http://www.codeproject.com/Articles/35978/Reading-Image-Headers-to-Get-Width-and-Height
// Modified to adapt to Unity3D (changed Size to Vector2)using System;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace MaterialUI
{
	public static class ImageHelper
	{
		private const string errorMessage = "Could not recognise image format.";

		private static Dictionary<byte[], Func<BinaryReader, Vector2>> m_ImageFormatDecoders = new Dictionary<byte[], Func<BinaryReader, Vector2>>()
	    {
	        { new byte[]{ 0x42, 0x4D }, DecodeBitmap},
	        { new byte[]{ 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 }, DecodeGif },
	        { new byte[]{ 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 }, DecodeGif },
	        { new byte[]{ 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }, DecodePng },
	        { new byte[]{ 0xff, 0xd8 }, DecodeJfif },
	    };

	    /// <summary>
	    /// Gets the dimensions of an image.
	    /// </summary>
	    /// <param name="path">The path of the image to get the dimensions of.</param>
	    /// <returns>The dimensions of the specified image.</returns>
	    /// <exception cref="ArgumentException">The image was of an unrecognised format.</exception>
		public static Vector2 GetDimensions(string path)
	    {
		    try
		    {
				using (BinaryReader binaryReader = new BinaryReader(File.OpenRead(path)))
				{
					try
					{
						return GetDimensions(binaryReader);
					}
					catch (ArgumentException e)
					{
						if (e.Message.StartsWith(errorMessage))
						{
							throw new ArgumentException(errorMessage, "path", e);
						}
						else
						{
							throw e;
						}
					}
				}
		    }
		    catch (Exception)
		    {
		    	// If it failed, we open the full file...
		    	Texture2D texture = new Texture2D(2, 2, TextureFormat.R16, false);
				texture.LoadImage(File.ReadAllBytes(path));

				Vector2 size = new Vector2(texture.width, texture.height);
				GameObject.Destroy(texture);

		    	return size;
		    }
	    }

	    /// <summary>
	    /// Gets the dimensions of an image.
	    /// </summary>
	    /// <param name="path">The path of the image to get the dimensions of.</param>
	    /// <returns>The dimensions of the specified image.</returns>
	    /// <exception cref="ArgumentException">The image was of an unrecognised format.</exception>    
		public static Vector2 GetDimensions(BinaryReader binaryReader)
	    {
	        int maxMagicBytesLength = m_ImageFormatDecoders.Keys.OrderByDescending(x => x.Length).First().Length;

	        byte[] magicBytes = new byte[maxMagicBytesLength];

	        for (int i = 0; i < maxMagicBytesLength; i += 1)
	        {
	            magicBytes[i] = binaryReader.ReadByte();

	            foreach(var kvPair in m_ImageFormatDecoders)
	            {
	                if (magicBytes.StartsWith(kvPair.Key))
	                {
	                    return kvPair.Value(binaryReader);
	                }
	            }
	        }

	        throw new ArgumentException(errorMessage, "binaryReader");
	    }

	    private static bool StartsWith(this byte[] thisBytes, byte[] thatBytes)
	    {
	        for(int i = 0; i < thatBytes.Length; i+= 1)
	        {
	            if (thisBytes[i] != thatBytes[i])
	            {
	                return false;
	            }
	        }
	        return true;
	    }

	    private static short ReadLittleEndianInt16(this BinaryReader binaryReader)
	    {
	        byte[] bytes = new byte[sizeof(short)];
	        for (int i = 0; i < sizeof(short); i += 1)
	        {
	            bytes[sizeof(short) - 1 - i] = binaryReader.ReadByte();
	        }
	        return BitConverter.ToInt16(bytes, 0);
	    }

	    private static int ReadLittleEndianInt32(this BinaryReader binaryReader)
	    {
	        byte[] bytes = new byte[sizeof(int)];
	        for (int i = 0; i < sizeof(int); i += 1)
	        {
	            bytes[sizeof(int) - 1 - i] = binaryReader.ReadByte();
	        }
	        return BitConverter.ToInt32(bytes, 0);
	    }

		private static Vector2 DecodeBitmap(BinaryReader binaryReader)
	    {
	        binaryReader.ReadBytes(16);
	        int width = binaryReader.ReadInt32();
	        int height = binaryReader.ReadInt32();
			return new Vector2(width, height);
	    }

		private static Vector2 DecodeGif(BinaryReader binaryReader)
	    {
	        int width = binaryReader.ReadInt16();
	        int height = binaryReader.ReadInt16();
			return new Vector2(width, height);
	    }

		private static Vector2 DecodePng(BinaryReader binaryReader)
	    {
	        binaryReader.ReadBytes(8);
	        int width = binaryReader.ReadLittleEndianInt32();
	        int height = binaryReader.ReadLittleEndianInt32();
			return new Vector2(width, height);
	    }

		private static Vector2 DecodeJfif(BinaryReader binaryReader)
	    {
	        while (binaryReader.ReadByte() == 0xff)
	        {
	            byte marker = binaryReader.ReadByte();
	            short chunkLength = binaryReader.ReadLittleEndianInt16();

	            if (marker == 0xc0)
	            {
	                binaryReader.ReadByte();

	                int height = binaryReader.ReadLittleEndianInt16();
	                int width = binaryReader.ReadLittleEndianInt16();
					return new Vector2(width, height);
	            }

				if (chunkLength < 0)
			    {
					ushort uchunkLength = (ushort)chunkLength;
					binaryReader.ReadBytes(uchunkLength - 2);
			    }
			    else
			    {
					binaryReader.ReadBytes(chunkLength - 2);
			    }
	        }

			throw new ArgumentException(errorMessage);
	    }
	}
}
