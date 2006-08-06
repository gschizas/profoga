/* $RCSfile$
* $Author: egonw $
* $Date: 2005-11-10 10:52:44 -0500 (Thu, 10 Nov 2005) $
* $Revision: 4255 $
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
namespace org.jmol.g3d
{
	
	
	/// <summary> renders triangles
	/// <p>
	/// currently only renders flat triangles
	/// <p>
	/// will probably need performance tuning
	/// 
	/// </summary>
	/// <author>  Miguel, miguel@jmol.org
	/// </author>
	class Triangle3D
	{
		private void  InitBlock()
		{
			rgb16sW = new Rgb16[DEFAULT];
			rgb16sE = new Rgb16[DEFAULT];
			for (int i = DEFAULT; --i >= 0; )
			{
				rgb16sW[i] = new Rgb16();
				rgb16sE[i] = new Rgb16();
			}
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'g3d '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Graphics3D g3d;
		//UPGRADE_NOTE: Final was removed from the declaration of 'line3d '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Line3D line3d;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'ax '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal int[] ax = new int[3];
		//UPGRADE_NOTE: Final was removed from the declaration of 'ay '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal int[] ay = new int[3];
		//UPGRADE_NOTE: Final was removed from the declaration of 'az '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal int[] az = new int[3];
		
		internal Rgb16[] rgb16sGouraud;
		
		internal Triangle3D(Graphics3D g3d)
		{
			InitBlock();
			this.g3d = g3d;
			this.line3d = g3d.line3d;
			rgb16sGouraud = new Rgb16[3];
			for (int i = 3; --i >= 0; )
				rgb16sGouraud[i] = new Rgb16();
		}
		
		/*===============================================================
		* 2004 05 12 - mth
		* I have been working hard to get the triangles to render
		* correctly when lines are drawn only once.
		* the rules were :
		* a pixel gets drawn when
		* 1. it is to the left of a line
		* 2. it is under a horizontal line
		*
		* this generally worked OK, but failed on small skinny triangles
		* careful reading of Michael Abrash's book
		* Graphics Programming Black Book
		* Chapter 38, The Polygon Primeval, page 714
		* it says:
		*   Narrow wedges and one-pixel-wide polygons will show up spottily
		* I do not understand why this is the case
		* so, the triangle drawing now paints overlapping edges by one pixel
		*
		*==============================================================*/
		
		
		internal virtual void  fillTriangle(bool useGouraud)
		{
			int cc0 = line3d.clipCode(ax[0], ay[0], az[0]);
			int cc1 = line3d.clipCode(ax[1], ay[1], az[1]);
			int cc2 = line3d.clipCode(ax[2], ay[2], az[2]);
			bool isClipped = (cc0 | cc1 | cc2) != 0;
			if (isClipped)
			{
				if ((cc0 & cc1 & cc2) != 0)
				{
					// all three corners are being clipped on the same dimension
					return ;
				}
				if ((line3d.visibilityCheck(ax[0], ay[0], az[0], ax[1], ay[1], az[1]) == Line3D.VISIBILITY_OFFSCREEN) && (line3d.visibilityCheck(ax[1], ay[1], az[1], ax[2], ay[2], az[2]) == Line3D.VISIBILITY_OFFSCREEN) && (line3d.visibilityCheck(ax[0], ay[0], az[0], ax[2], ay[2], az[2]) == Line3D.VISIBILITY_OFFSCREEN))
				{
					// this is not technically correct
					// none of the edges are on-screen, but the corners could
					// be on opposite sides of the screen, so that the screen
					// is in the interior of the triangle
					// we are not going to worry about that case
					return ;
				}
			}
			int iMinY = 0;
			if (ay[1] < ay[0])
				iMinY = 1;
			if (ay[2] < ay[iMinY])
				iMinY = 2;
			int iMidY = (iMinY + 1) % 3;
			int iMaxY = (iMinY + 2) % 3;
			if (ay[iMidY] > ay[iMaxY])
			{
				int t = iMidY; iMidY = iMaxY; iMaxY = t;
			}
			
			/*
			System.out.println("----fillTriangle\n" +
			" iMinY=" + iMinY + " iMidY=" + iMidY +
			" iMaxY=" + iMaxY + "\n" +
			"  minY="+ax[iMinY]+","+ay[iMinY]+","+az[iMinY]+"\n" +
			"  midY="+ax[iMidY]+","+ay[iMidY]+","+az[iMidY]+"\n" +
			"  maxY="+ax[iMaxY]+","+ay[iMaxY]+","+az[iMaxY]+"\n");
			*/
			int yMin = ay[iMinY];
			int yMid = ay[iMidY];
			int yMax = ay[iMaxY];
			int nLines = yMax - yMin + 1;
			if (nLines > axW.Length)
				reallocRasterArrays(nLines);
			
			Rgb16[] gouraudW = useGouraud?rgb16sW:null;
			Rgb16[] gouraudE = useGouraud?rgb16sE:null;
			
			int dyMidMin = yMid - yMin;
			if (dyMidMin == 0)
			{
				// flat top
				if (ax[iMidY] < ax[iMinY])
				{
					int t = iMidY; iMidY = iMinY; iMinY = t;
				}
				generateRaster(nLines, iMinY, iMaxY, axW, azW, 0, gouraudW);
				generateRaster(nLines, iMidY, iMaxY, axE, azE, 0, gouraudE);
			}
			else if (yMid == yMax)
			{
				// flat bottom
				if (ax[iMaxY] < ax[iMidY])
				{
					int t = iMidY; iMidY = iMaxY; iMaxY = t;
				}
				generateRaster(nLines, iMinY, iMidY, axW, azW, 0, gouraudW);
				generateRaster(nLines, iMinY, iMaxY, axE, azE, 0, gouraudE);
			}
			else
			{
				int dxMaxMin = ax[iMaxY] - ax[iMinY];
				//int dzMaxMin = az[iMaxY] - az[iMinY];
				int roundFactor;
				roundFactor = nLines / 2;
				if (dxMaxMin < 0)
					roundFactor = - roundFactor;
				int axSplit = ax[iMinY] + (dxMaxMin * dyMidMin + roundFactor) / nLines;
				if (axSplit < ax[iMidY])
				{
					generateRaster(nLines, iMinY, iMaxY, axW, azW, 0, gouraudW);
					generateRaster(dyMidMin, iMinY, iMidY, axE, azE, 0, gouraudE);
					generateRaster(nLines - dyMidMin, iMidY, iMaxY, axE, azE, dyMidMin, gouraudE);
				}
				else
				{
					generateRaster(dyMidMin, iMinY, iMidY, axW, azW, 0, gouraudW);
					generateRaster(nLines - dyMidMin, iMidY, iMaxY, axW, azW, dyMidMin, gouraudW);
					generateRaster(nLines, iMinY, iMaxY, axE, azE, 0, gouraudE);
				}
			}
			fillRaster(yMin, nLines, useGouraud, isClipped);
		}
		
		private const int DEFAULT = 64;
		
		internal int[] axW = new int[DEFAULT], azW = new int[DEFAULT];
		internal int[] axE = new int[DEFAULT], azE = new int[DEFAULT];
		internal Rgb16[] rgb16sW, rgb16sE;
		
		internal virtual void  reallocRasterArrays(int n)
		{
			n = (n + 31) & ~ 31;
			axW = new int[n];
			azW = new int[n];
			axE = new int[n];
			azE = new int[n];
			rgb16sW = reallocRgb16s(rgb16sW, n);
			rgb16sE = reallocRgb16s(rgb16sE, n);
		}
		
		internal virtual Rgb16[] reallocRgb16s(Rgb16[] rgb16s, int n)
		{
			Rgb16[] t = new Rgb16[n];
			Array.Copy(rgb16s, 0, t, 0, rgb16s.Length);
			for (int i = rgb16s.Length; i < n; ++i)
				t[i] = new Rgb16();
			return t;
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'rgb16t1 '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Rgb16 rgb16t1 = new Rgb16();
		//UPGRADE_NOTE: Final was removed from the declaration of 'rgb16t2 '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Rgb16 rgb16t2 = new Rgb16();
		
		private const bool VERIFY = true;
		
		internal virtual void  generateRaster(int dy, int iN, int iS, int[] axRaster, int[] azRaster, int iRaster, Rgb16[] gouraud)
		{
			/*
			System.out.println("generateRaster\n" +
			"N="+ax[iN]+","+ay[iN]+","+az[iN]+"\n" +
			"S="+ax[iS]+","+ay[iS]+","+az[iS]+"\n");
			*/
			int xN = ax[iN], zN = az[iN];
			int xS = ax[iS], zS = az[iS];
			int dx = xS - xN, dz = zS - zN;
			int xCurrent = xN;
			int xIncrement, width, errorTerm;
			if (dx >= 0)
			{
				xIncrement = 1;
				width = dx;
				errorTerm = 0;
			}
			else
			{
				xIncrement = - 1;
				width = - dx;
				errorTerm = - dy + 1;
			}
			
			/*
			System.out.println("xN=" + xN + " xS=" + xS + " dy=" + dy + " dz=" + dz);
			*/
			int zCurrentScaled = (zN << 10) + (1 << 9);
			int roundingFactor;
			roundingFactor = dy / 2;
			if (dz < 0)
				roundingFactor = - roundingFactor;
			int zIncrementScaled = ((dz << 10) + roundingFactor) / dy;
			
			int xMajorIncrement;
			int xMajorError;
			if (dy >= width)
			{
				xMajorIncrement = 0;
				xMajorError = width;
			}
			else
			{
				xMajorIncrement = dx / dy;
				xMajorError = width % dy;
			}
			for (int y = 0, i = iRaster; y < dy; zCurrentScaled += zIncrementScaled, ++i, ++y)
			{
				axRaster[i] = xCurrent;
				azRaster[i] = zCurrentScaled >> 10;
				//      System.out.println("z=" + azRaster[y]);
				xCurrent += xMajorIncrement;
				errorTerm += xMajorError;
				if (errorTerm > 0)
				{
					xCurrent += xIncrement;
					errorTerm -= dy;
				}
			}
			if (gouraud != null)
			{
				Rgb16 rgb16Base = rgb16t1;
				rgb16Base.set_Renamed(rgb16sGouraud[iN]);
				Rgb16 rgb16Increment = rgb16t2;
				rgb16Increment.diffDiv(rgb16sGouraud[iS], rgb16Base, dy);
				for (int i = iRaster, iMax = iRaster + dy; i < iMax; ++i)
					gouraud[i].setAndIncrement(rgb16Base, rgb16Increment);
				Rgb16 north = rgb16sGouraud[iN];
				Rgb16 generated = gouraud[iRaster];
				if (VERIFY)
				{
					if (north.Argb != generated.Argb)
					{
						System.Console.Out.WriteLine("north=" + north + "\ngenerated=" + generated);
						throw new System.NullReferenceException();
					}
					/*
					if (rgb16Base.getArgb() != rgb16sGouraud[iS].getArgb())
					throw new NullPointerException();
					*/
				}
			}
		}
		
		internal static int bar;
		
		internal virtual void  fillRaster(int y, int numLines, bool useGouraud, bool isClipped)
		{
			//    System.out.println("fillRaster("+y+","+numLines+","+paintFirstLine);
			int i = 0;
			++bar;
			if (y < 0)
			{
				numLines += y;
				i -= y;
				y = 0;
			}
			if (y + numLines > g3d.height)
				numLines = g3d.height - y;
			if (isClipped)
			{
				for (; --numLines >= 0; ++y, ++i)
				{
					int xW = axW[i];
					int pixelCount = axE[i] - xW + 1;
					if (pixelCount > 0)
						g3d.plotPixelsClipped(pixelCount, xW, y, azW[i], azE[i], useGouraud?rgb16sW[i]:null, useGouraud?rgb16sE[i]:null);
				}
			}
			else
			{
				for (; --numLines >= 0; ++y, ++i)
				{
					int xW = axW[i];
					int pixelCount = axE[i] - xW + 1;
					// miguel 2005 01 13
					// not sure exactly why we are getting pixel counts of 0 here
					// it means that the east/west lines are crossing by 1
					// something must be going wrong with the scaled addition
					if (pixelCount > 0)
						g3d.plotPixelsUnclipped(pixelCount, xW, y, azW[i], azE[i], useGouraud?rgb16sW[i]:null, useGouraud?rgb16sE[i]:null);
				}
			}
		}
		
		internal virtual void  setGouraud(int rgbA, int rgbB, int rgbC)
		{
			rgb16sGouraud[0].set_Renamed(rgbA);
			rgb16sGouraud[1].set_Renamed(rgbB);
			rgb16sGouraud[2].set_Renamed(rgbC);
		}
	}
}