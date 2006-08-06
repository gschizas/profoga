/* $RCSfile$
* $Author: egonw $
* $Date: 2005-11-10 16:52:44 +0100 (jeu., 10 nov. 2005) $
* $Revision: 4255 $
*
* Copyright (C) 2005  Miguel, Jmol Development, www.jmol.org
*
* Contact: miguel@jmol.org
*
*  This library is free software; you can redistribute it and/or
*  modify it under the terms of the GNU Lesser General Public
*  License as published by the Free Software Foundation; either
*  version 2.1 of the License, or (at your option) any later version.
*
*  This library is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
*  Lesser General Public License for more details.
*
*  You should have received a copy of the GNU Lesser General Public
*  License along with this library; if not, write to the Free Software
*  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*/
using System;
namespace org.jmol.util
{
	
	/// <summary>*************************************************************
	/// The Bmp class provides support for BitMap Pool objects
	/// and static support for BitMaP operations on int[]
	/// **************************************************************
	/// </summary>
	
	public sealed class Bmp
	{
		
		private const bool debugDoubleCheck = true;
		
		public static int[] allocateBitmap(int count)
		{
			return new int[(count + 31) >> 5];
		}
		
		public static int[] allocateSetAllBits(int count)
		{
			int i = (count + 31) >> 5;
			int[] bitmap = new int[i];
			int fractionalBitCount = count & 31;
			if (fractionalBitCount != 0)
			{
				bitmap[--i] = SupportClass.URShift((0x80000000 >> (fractionalBitCount - 1)), (32 - fractionalBitCount));
			}
			while (--i >= 0)
				bitmap[i] = unchecked((int) 0xFFFFFFFF);
			return bitmap;
		}
		
		public static int[] growBitmap(int[] bitmap, int count)
		{
			if (count < 0)
				throw new System.IndexOutOfRangeException();
			if (bitmap == null)
				return (count == 0)?null:allocateBitmap(count);
			int minLength = (count + 31) >> 5;
			if (bitmap.Length >= minLength)
				return bitmap;
			int[] newBitmap = new int[minLength];
			for (int i = bitmap.Length; --i >= 0; )
				newBitmap[i] = bitmap[i];
			return newBitmap;
		}
		
		public static void  setBit(int[] bitmap, int i)
		{
			int index = i >> 5;
			bitmap[index] |= 1 << (i & 31);
		}
		
		public static int[] setBitGrow(int[] bitmap, int i)
		{
			bitmap = growBitmap(bitmap, i + 1);
			if (bitmap != null)
				bitmap[i >> 5] |= 1 << (i & 31);
			return bitmap;
		}
		
		public static void  clearBit(int[] bitmap, int i)
		{
			int index = i >> 5;
			if (index < bitmap.Length)
				bitmap[(i >> 5)] &= ~ (1 << (i & 31));
		}
		
		public static bool getBit(int[] bitmap, int i)
		{
			int index = i >> 5;
			if (index >= bitmap.Length)
				return false;
			return (bitmap[index] & (1 << (i & 31))) != 0;
		}
		
		public static void  setAllBits(int[] bitmap, int count)
		{
			int i = (count + 31) >> 5;
			if (bitmap.Length != i)
			{
				if (bitmap.Length < i)
					throw new System.IndexOutOfRangeException();
				// zero out all the bits above
				for (int j = bitmap.Length; --j >= i; )
					bitmap[j] = 0;
			}
			int fractionalBitCount = count & 31;
			if (fractionalBitCount != 0)
			{
				bitmap[--i] = SupportClass.URShift((0x80000000 >> (fractionalBitCount - 1)), (32 - fractionalBitCount));
			}
			while (--i >= 0)
				bitmap[i] = unchecked((int) 0xFFFFFFFF);
		}
		
		public static void  orInto(int[] bmDestination, int[] bmSource)
		{
			int minLength = (bmDestination.Length < bmSource.Length?bmDestination.Length:bmSource.Length);
			for (int i = minLength; --i >= 0; )
				bmDestination[i] |= bmSource[i];
		}
		
		public static int[] allocMinimalCopy(int[] bitmap)
		{
			int indexLast;
			for (indexLast = bitmap.Length; --indexLast >= 0 && bitmap[indexLast] == 0; )
			{
			}
			int[] map = null;
			if (indexLast >= 0)
			{
				int count = indexLast + 1;
				map = new int[count];
				for (int j = count; --j >= 0; )
					map[j] = bitmap[j];
			}
			return map;
		}
		
		public static int countBits(int map)
		{
			map -= SupportClass.URShift((map & unchecked((int) 0xAAAAAAAA)), 1);
			map = (map & 0x33333333) + ((SupportClass.URShift(map, 2)) & 0x33333333);
			map = (map + (SupportClass.URShift(map, 4))) & 0x0F0F0F0F;
			map += SupportClass.URShift(map, 8);
			map += SupportClass.URShift(map, 16);
			return map & 0xFF;
		}
		
		public static int countBits(int[] bitmap)
		{
			int count = 0;
			for (int i = bitmap.Length; --i >= 0; )
			{
				int bits = bitmap[i];
				if (bits != 0)
					count += countBits(bits);
			}
			return count;
		}
		
		public static void  clearBitmap(int[] bitmap)
		{
			for (int i = bitmap.Length; --i >= 0; )
				bitmap[i] = 0;
		}
		
		public static int getMaxMappedBit(int[] bitmap)
		{
			if (bitmap == null)
				return 0;
			int answer1 = 0;
			if (debugDoubleCheck)
			{
				for (answer1 = bitmap.Length * 32; --answer1 >= 0 && !getBit(bitmap, answer1); )
				{
				}
				++answer1;
			}
			
			int maxMapped = bitmap.Length << 5;
			int map = 0;
			int i;
			for (i = bitmap.Length; --i >= 0 && (map = bitmap[i]) == 0; )
				maxMapped -= 32;
			if (i >= 0)
			{
				if ((map & unchecked((int) 0xFFFF0000)) == 0)
				{
					map <<= 16;
					maxMapped -= 16;
				}
				if ((map & unchecked((int) 0xFF000000)) == 0)
				{
					map <<= 8;
					maxMapped -= 8;
				}
				if ((map & unchecked((int) 0xF0000000)) == 0)
				{
					map <<= 4;
					maxMapped -= 4;
				}
				if ((map & unchecked((int) 0xC0000000)) == 0)
				{
					map <<= 2;
					maxMapped -= 2;
				}
				if (map >= 0)
					maxMapped -= 1;
			}
			if (debugDoubleCheck)
			{
				if (answer1 != maxMapped)
				{
					System.Console.Out.WriteLine("answer1=" + answer1 + " maxMapped=" + maxMapped);
					System.Console.Out.WriteLine("bitmap.length=" + bitmap.Length);
					for (int j = 0; j < bitmap.Length; ++j)
						System.Console.Out.WriteLine("bitmap[" + j + "]=" + System.Convert.ToString(bitmap[j], 2));
					throw new System.NullReferenceException();
				}
			}
			return maxMapped;
		}
		
		public static int getMinMappedBit(int[] bitmap)
		{
			if (bitmap == null)
				return - 1;
			int mapLength = bitmap.Length;
			int maxMapped = mapLength << 5;
			int answer1 = 0;
			if (debugDoubleCheck)
			{
				for (; answer1 < maxMapped && !getBit(bitmap, answer1); ++answer1)
				{
				}
				if (answer1 == maxMapped)
					answer1 = - 1;
			}
			int map = 0;
			int minMapped = 0;
			int i;
			for (i = 0; i < mapLength && (map = bitmap[i]) == 0; ++i)
				minMapped += 32;
			if (i == mapLength)
			{
				minMapped = - 1;
			}
			else
			{
				if ((map & 0x0000FFFF) == 0)
				{
					map >>= 16;
					minMapped += 16;
				}
				if ((map & 0x000000FF) == 0)
				{
					map >>= 8;
					minMapped += 8;
				}
				if ((map & 0x0000000F) == 0)
				{
					map >>= 4;
					minMapped += 4;
				}
				if ((map & 0x00000003) == 0)
				{
					map >>= 2;
					minMapped += 2;
				}
				if ((map & 0x00000001) == 0)
					minMapped += 1;
			}
			if (debugDoubleCheck)
			{
				if (answer1 != minMapped)
				{
					System.Console.Out.WriteLine("answer1=" + answer1 + " minMapped=" + minMapped);
					System.Console.Out.WriteLine("bitmap.length=" + bitmap.Length);
					for (int j = 0; j < bitmap.Length; ++j)
						System.Console.Out.WriteLine("bitmap[" + j + "]=" + System.Convert.ToString(bitmap[j], 2));
					throw new System.NullReferenceException();
				}
			}
			return minMapped;
		}
		
		public static int nextSetBit(int[] bitmap, int iStart)
		{
			if (bitmap == null)
				return - 1;
			int mapLength = bitmap.Length;
			if (iStart >= mapLength << 5)
				return - 1;
			int bitmapIndex = iStart >> 5;
			int bitIndexWithinWord = iStart & 31;
			int map = bitmap[bitmapIndex] & (0x80000000 >> (31 - bitIndexWithinWord));
			if ((map & (1 << bitIndexWithinWord)) != 0)
				return iStart;
			while (map == 0)
			{
				if (++bitmapIndex == mapLength)
					return - 1;
				map = bitmap[bitmapIndex];
			}
			bitIndexWithinWord = 0;
			if ((map & 0x0000FFFF) == 0)
			{
				bitIndexWithinWord += 16;
				map >>= 16;
			}
			if ((map & 0x000000FF) == 0)
			{
				bitIndexWithinWord += 8;
				map >>= 8;
			}
			if ((map & 0x0000000F) == 0)
			{
				bitIndexWithinWord += 4;
				map >>= 4;
			}
			if ((map & 0x00000003) == 0)
			{
				bitIndexWithinWord += 2;
				map >>= 2;
			}
			if ((map & 0x00000001) == 0)
				++bitIndexWithinWord;
			return (bitmapIndex << 5) + bitIndexWithinWord;
		}
		
		public static bool and(int[] target, int[] other)
		{
			int bits = 0;
			if (target != null && other != null)
			{
				int len = (target.Length <= other.Length)?target.Length:other.Length;
				for (int i = len; --i >= 0; )
					bits |= (target[i] &= other[i]);
			}
			return bits != 0;
		}
		
		public static bool and(int[] target, int[] bmpA, int[] bmpB)
		{
			int len = System.Math.Min(bmpA == null?0:bmpA.Length, bmpB == null?0:bmpB.Length);
			for (int i = target.Length; --i >= len; )
				target[i] = 0;
			int bits = 0;
			for (int i = len; --i >= 0; )
				bits |= (target[i] = bmpA[i] & bmpB[i]);
			return bits != 0;
		}
	}
}