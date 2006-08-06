/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-03-21 20:08:47 -0500 (Tue, 21 Mar 2006) $
* $Revision: 4678 $
*
* Copyright (C) 2003-2005  Miguel, Jmol Development, www.jmol.org
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
using Int2IntHash = org.jmol.util.Int2IntHash;
namespace org.jmol.g3d
{
	
	/// <summary><p>
	/// Implements a color index model using a colix as a
	/// <strong>COLor IndeX</strong>.
	/// </p>
	/// <p>
	/// A colix is a color index represented as a short int.
	/// </p>
	/// <p>
	/// The value 0 is considered a null value ... for no color. In Jmol this
	/// generally means that the value is inherited from some other object.
	/// </p>
	/// <p>
	/// The value 1 is used to indicate TRANSLUCENT, but with the color
	/// coming from the parent. The value 2 indicates OPAQUE, but with the
	/// color coming from the parent.
	/// </p>
	/// 
	/// </summary>
	/// <author>  Miguel, miguel@jmol.org
	/// </author>
	sealed class Colix
	{
		
		
		//UPGRADE_NOTE: The initialization of  'colixMax' was moved to static method 'org.jmol.g3d.Colix'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private static int colixMax;
		private static int[] argbs = new int[128];
		private static int[] argbsGreyscale;
		private static int[][] ashades = new int[128][];
		private static int[][] ashadesGreyscale;
		//UPGRADE_NOTE: Final was removed from the declaration of 'colixHash '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private static readonly Int2IntHash colixHash = new Int2IntHash();
		
		internal static short getColix(int argb)
		{
			if (argb == 0)
				return 0;
			int translucentMask = 0;
			if ((argb & unchecked((int) 0xFF000000)) != 0xFF000000)
			{
				if ((argb & unchecked((int) 0xFF000000)) == 0)
				{
					System.Console.Out.WriteLine("zero alpha channel + non-zero rgb not supported");
					throw new System.IndexOutOfRangeException();
				}
				argb |= unchecked((int) 0xFF000000);
				translucentMask = Graphics3D.TRANSLUCENT_MASK;
			}
			int c = colixHash.get_Renamed(argb);
			if (c > 0)
			{
				return (short) (c | translucentMask);
			}
			return (short) (allocateColix(argb) | translucentMask);
		}
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'allocateColix'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		private static int allocateColix(int argb)
		{
			lock (typeof(org.jmol.g3d.Colix))
			{
				// double-check to make sure that someone else did not allocate
				// something of the same color while we were waiting for the lock
				if ((argb & unchecked((int) 0xFF000000)) != 0xFF000000)
					throw new System.IndexOutOfRangeException();
				for (int i = colixMax; --i >= Graphics3D.SPECIAL_COLIX_MAX; )
					if (argb == argbs[i])
						return (short) i;
				if (colixMax == argbs.Length)
				{
					int oldSize = argbs.Length;
					int newSize = oldSize * 2;
					int[] t0 = new int[newSize];
					Array.Copy(argbs, 0, t0, 0, oldSize);
					argbs = t0;
					
					if (argbsGreyscale != null)
					{
						t0 = new int[newSize];
						Array.Copy(argbsGreyscale, 0, t0, 0, oldSize);
						argbsGreyscale = t0;
					}
					
					int[][] t2 = new int[newSize][];
					Array.Copy(ashades, 0, t2, 0, oldSize);
					ashades = t2;
					
					if (ashadesGreyscale != null)
					{
						t2 = new int[newSize][];
						Array.Copy(ashadesGreyscale, 0, t2, 0, oldSize);
						ashadesGreyscale = t2;
					}
				}
				argbs[colixMax] = argb;
				if (argbsGreyscale != null)
					argbsGreyscale[colixMax] = Graphics3D.calcGreyscaleRgbFromRgb(argb);
				colixHash.put(argb, colixMax);
				return colixMax++;
			}
		}
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'calcArgbsGreyscale'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		private static void  calcArgbsGreyscale()
		{
			lock (typeof(org.jmol.g3d.Colix))
			{
				if (argbsGreyscale == null)
				{
					argbsGreyscale = new int[argbs.Length];
					for (int i = argbsGreyscale.Length; --i >= 0; )
						argbsGreyscale[i] = Graphics3D.calcGreyscaleRgbFromRgb(argbs[i]);
				}
			}
		}
		
		internal static int getArgb(short colix)
		{
			return argbs[colix & Graphics3D.OPAQUE_MASK];
		}
		
		internal static int getArgbGreyscale(short colix)
		{
			if (argbsGreyscale == null)
				calcArgbsGreyscale();
			return argbsGreyscale[colix & Graphics3D.OPAQUE_MASK];
		}
		
		internal static bool isTranslucent(short colix)
		{
			return (colix & Graphics3D.TRANSLUCENT_MASK) != 0;
		}
		
		internal static int[] getShades(short colix)
		{
			colix &= Graphics3D.OPAQUE_MASK;
			int[] shades = ashades[colix];
			if (shades == null)
				shades = ashades[colix] = Shade3D.getShades(argbs[colix], false);
			return shades;
		}
		
		internal static int[] getShadesGreyscale(short colix)
		{
			colix &= Graphics3D.OPAQUE_MASK;
			if (ashadesGreyscale == null)
				ashadesGreyscale = new int[ashades.Length][];
			int[] shadesGreyscale = ashadesGreyscale[colix];
			if (shadesGreyscale == null)
				shadesGreyscale = ashadesGreyscale[colix] = Shade3D.getShades(argbs[colix], true);
			return shadesGreyscale;
		}
		
		internal static void  flushShades()
		{
			for (int i = colixMax; --i >= 0; )
				ashades[i] = null;
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'hashMix2 '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly Int2IntHash hashMix2 = new Int2IntHash(32);
		
		internal static short getColixMix(short colixA, short colixB)
		{
			if (colixA == colixB)
				return colixA;
			if (colixA <= 0)
				return colixB;
			if (colixB <= 0)
				return colixA;
			int translucentMask = colixA & colixB & Graphics3D.TRANSLUCENT_MASK;
			colixA &= ~ Graphics3D.TRANSLUCENT_MASK;
			colixB &= ~ Graphics3D.TRANSLUCENT_MASK;
			int mixId = ((colixA < colixB)?((colixA << 16) | colixB):((colixB << 16) | colixA));
			int mixed = hashMix2.get_Renamed(mixId);
			if (mixed == System.Int32.MinValue)
			{
				int argbA = argbs[colixA];
				int argbB = argbs[colixB];
				int r = (((argbA & 0x00FF0000) + (argbB & 0x00FF0000)) >> 1) & 0x00FF0000;
				int g = (((argbA & 0x0000FF00) + (argbB & 0x0000FF00)) >> 1) & 0x0000FF00;
				int b = (((argbA & 0x000000FF) + (argbB & 0x000000FF)) >> 1);
				int argbMixed = unchecked((int) 0xFF000000) | r | g | b;
				mixed = getColix(argbMixed);
				hashMix2.put(mixId, mixed);
			}
			return (short) (mixed | translucentMask);
		}
		static Colix()
		{
			colixMax = Graphics3D.SPECIAL_COLIX_MAX;
		}
	}
}