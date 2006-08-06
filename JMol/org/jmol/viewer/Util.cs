/* $RCSfile$
* $Author: egonw $
* $Date: 2005-11-10 16:52:44 +0100 (jeu., 10 nov. 2005) $
* $Revision: 4255 $
*
* Copyright (C) 2003-2005  The Jmol Development Team
*
* Contact: jmol-developers@lists.sf.net
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
namespace org.jmol.viewer
{
	
	sealed class Util
	{
		
		internal static System.Object ensureLength(System.Object array, int minimumLength)
		{
			if (array != null && ((System.Array) array).Length >= minimumLength)
				return array;
			return setLength(array, minimumLength);
		}
		
		internal static System.String[] ensureLength(System.String[] array, int minimumLength)
		{
			if (array != null && array.Length >= minimumLength)
				return array;
			return setLength(array, minimumLength);
		}
		
		internal static float[] ensureLength(float[] array, int minimumLength)
		{
			if (array != null && array.Length >= minimumLength)
				return array;
			return setLength(array, minimumLength);
		}
		
		internal static int[] ensureLength(int[] array, int minimumLength)
		{
			if (array != null && array.Length >= minimumLength)
				return array;
			return setLength(array, minimumLength);
		}
		
		internal static short[] ensureLength(short[] array, int minimumLength)
		{
			if (array != null && array.Length >= minimumLength)
				return array;
			return setLength(array, minimumLength);
		}
		
		internal static sbyte[] ensureLength(sbyte[] array, int minimumLength)
		{
			if (array != null && array.Length >= minimumLength)
				return array;
			return setLength(array, minimumLength);
		}
		
		internal static System.Object doubleLength(System.Object array)
		{
			return setLength(array, (array == null?16:2 * ((System.Array) array).Length));
		}
		
		internal static System.String[] doubleLength(System.String[] array)
		{
			return setLength(array, (array == null?16:2 * array.Length));
		}
		
		internal static float[] doubleLength(float[] array)
		{
			return setLength(array, (array == null?16:2 * array.Length));
		}
		
		internal static int[] doubleLength(int[] array)
		{
			return setLength(array, (array == null?16:2 * array.Length));
		}
		
		internal static short[] doubleLength(short[] array)
		{
			return setLength(array, (array == null?16:2 * array.Length));
		}
		
		internal static sbyte[] doubleLength(sbyte[] array)
		{
			return setLength(array, (array == null?16:2 * array.Length));
		}
		
		internal static bool[] doubleLength(bool[] array)
		{
			return setLength(array, (array == null?16:2 * array.Length));
		}
		
		internal static System.Object setLength(System.Object array, int newLength)
		{
			System.Object t = System.Array.CreateInstance(array.GetType().GetElementType(), newLength);
			int oldLength = ((System.Array) array).Length;
			Array.Copy(array, 0, t, 0, oldLength < newLength?oldLength:newLength);
			return t;
		}
		
		internal static System.String[] setLength(System.String[] array, int newLength)
		{
			System.String[] t = new System.String[newLength];
			if (array != null)
			{
				int oldLength = array.Length;
				Array.Copy(array, 0, t, 0, oldLength < newLength?oldLength:newLength);
			}
			return t;
		}
		
		internal static float[] setLength(float[] array, int newLength)
		{
			float[] t = new float[newLength];
			if (array != null)
			{
				int oldLength = array.Length;
				Array.Copy(array, 0, t, 0, oldLength < newLength?oldLength:newLength);
			}
			return t;
		}
		
		internal static int[] setLength(int[] array, int newLength)
		{
			int[] t = new int[newLength];
			if (array != null)
			{
				int oldLength = array.Length;
				Array.Copy(array, 0, t, 0, oldLength < newLength?oldLength:newLength);
			}
			return t;
		}
		
		internal static short[] setLength(short[] array, int newLength)
		{
			short[] t = new short[newLength];
			if (array != null)
			{
				int oldLength = array.Length;
				Array.Copy(array, 0, t, 0, oldLength < newLength?oldLength:newLength);
			}
			return t;
		}
		
		internal static sbyte[] setLength(sbyte[] array, int newLength)
		{
			sbyte[] t = new sbyte[newLength];
			if (array != null)
			{
				int oldLength = array.Length;
				Array.Copy(array, 0, t, 0, oldLength < newLength?oldLength:newLength);
			}
			return t;
		}
		
		internal static bool[] setLength(bool[] array, int newLength)
		{
			bool[] t = new bool[newLength];
			if (array != null)
			{
				int oldLength = array.Length;
				Array.Copy(array, 0, t, 0, oldLength < newLength?oldLength:newLength);
			}
			return t;
		}
		
		internal static void  swap(short[] array, int indexA, int indexB)
		{
			short t = array[indexA];
			array[indexA] = array[indexB];
			array[indexB] = t;
		}
		internal static void  swap(int[] array, int indexA, int indexB)
		{
			int t = array[indexA];
			array[indexA] = array[indexB];
			array[indexB] = t;
		}
		internal static void  swap(float[] array, int indexA, int indexB)
		{
			float t = array[indexA];
			array[indexA] = array[indexB];
			array[indexB] = t;
		}
	}
}