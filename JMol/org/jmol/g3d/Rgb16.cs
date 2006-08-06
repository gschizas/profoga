/* $RCSfile$
* $Author: egonw $
* $Date: 2005-11-10 10:52:44 -0500 (Thu, 10 Nov 2005) $
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
namespace org.jmol.g3d
{
	
	sealed class Rgb16
	{
		internal int Argb
		{
			get
			{
				return ((0xFF << 24) | ((rScaled << 8) & 0xFF0000) | (gScaled & 0xFF00) | (bScaled >> 8));
			}
			
		}
		internal int rScaled;
		internal int gScaled;
		internal int bScaled;
		
		internal Rgb16()
		{
		}
		
		internal Rgb16(int argb)
		{
			set_Renamed(argb);
		}
		
		internal void  set_Renamed(int argb)
		{
			rScaled = ((argb >> 8) & 0xFF00) | 0x80;
			gScaled = ((argb) & 0xFF00) | 0x80;
			bScaled = ((argb << 8) & 0xFF00) | 0x80;
		}
		
		internal void  set_Renamed(Rgb16 other)
		{
			rScaled = other.rScaled;
			gScaled = other.gScaled;
			bScaled = other.bScaled;
		}
		
		internal void  diffDiv(Rgb16 rgb16A, Rgb16 rgb16B, int divisor)
		{
			rScaled = (rgb16A.rScaled - rgb16B.rScaled) / divisor;
			gScaled = (rgb16A.gScaled - rgb16B.gScaled) / divisor;
			bScaled = (rgb16A.bScaled - rgb16B.bScaled) / divisor;
		}
		
		internal void  add(Rgb16 other)
		{
			rScaled += other.rScaled;
			gScaled += other.gScaled;
			bScaled += other.bScaled;
		}
		
		internal void  add(Rgb16 base_Renamed, Rgb16 other)
		{
			rScaled = base_Renamed.rScaled + other.rScaled;
			gScaled = base_Renamed.gScaled + other.gScaled;
			bScaled = base_Renamed.bScaled + other.bScaled;
		}
		
		internal void  setAndIncrement(Rgb16 base_Renamed, Rgb16 other)
		{
			rScaled = base_Renamed.rScaled;
			base_Renamed.rScaled += other.rScaled;
			gScaled = base_Renamed.gScaled;
			base_Renamed.gScaled += other.gScaled;
			bScaled = base_Renamed.bScaled;
			base_Renamed.bScaled += other.bScaled;
		}
		
		public override System.String ToString()
		{
			return "Rgb16(" + rScaled + "," + gScaled + "," + bScaled + " -> " + ((rScaled >> 8) & 0xFF) + "," + ((gScaled >> 8) & 0xFF) + "," + ((bScaled >> 8) & 0xFF) + ")";
		}
	}
}