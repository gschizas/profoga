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
//UPGRADE_TODO: The package 'javax.vecmath' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using javax.vecmath;
namespace org.jmol.viewer
{
	
	class TempManager
	{
		
		internal Viewer viewer;
		
		internal TempManager(Viewer viewer)
		{
			this.viewer = viewer;
		}
		
		internal static int findBestFit(int size, short[] lengths)
		{
			int iFit = - 1;
			int fitLength = System.Int32.MaxValue;
			
			for (int i = lengths.Length; --i >= 0; )
			{
				int freeLength = lengths[i];
				if (freeLength >= size && freeLength < fitLength)
				{
					fitLength = freeLength;
					iFit = i;
				}
			}
			if (iFit >= 0)
				lengths[iFit] = 0;
			return iFit;
		}
		
		internal static int findShorter(int size, short[] lengths)
		{
			for (int i = lengths.Length; --i >= 0; )
				if (lengths[i] == 0)
				{
					lengths[i] = (short) size;
					return i;
				}
			int iShortest = 0;
			int shortest = lengths[0];
			for (int i = lengths.Length; --i > 0; )
				if (lengths[i] < shortest)
				{
					shortest = lengths[i];
					iShortest = i;
				}
			if (shortest < size)
			{
				lengths[iShortest] = (short) size;
				return iShortest;
			}
			return - 1;
		}
		
		////////////////////////////////////////////////////////////////
		// temp Points
		////////////////////////////////////////////////////////////////
		internal const int freePointsSize = 6;
		//UPGRADE_NOTE: Final was removed from the declaration of 'lengthsFreePoints '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal short[] lengthsFreePoints = new short[freePointsSize];
		//UPGRADE_NOTE: Final was removed from the declaration of 'freePoints '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Point3f[][] freePoints = new Point3f[freePointsSize][];
		
		internal virtual Point3f[] allocTempPoints(int size)
		{
			Point3f[] tempPoints;
			int iFit = findBestFit(size, lengthsFreePoints);
			if (iFit > 0)
			{
				tempPoints = freePoints[iFit];
			}
			else
			{
				tempPoints = new Point3f[size];
				for (int i = size; --i >= 0; )
					tempPoints[i] = new Point3f();
			}
			return tempPoints;
		}
		
		internal virtual void  freeTempPoints(Point3f[] tempPoints)
		{
			int iFree = findShorter(tempPoints.length, lengthsFreePoints);
			if (iFree >= 0)
				freePoints[iFree] = tempPoints;
		}
		
		////////////////////////////////////////////////////////////////
		// temp Screens
		////////////////////////////////////////////////////////////////
		internal const int freeScreensSize = 6;
		//UPGRADE_NOTE: Final was removed from the declaration of 'lengthsFreeScreens '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal short[] lengthsFreeScreens = new short[freeScreensSize];
		//UPGRADE_NOTE: Final was removed from the declaration of 'freeScreens '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Point3i[][] freeScreens = new Point3i[freeScreensSize][];
		
		internal virtual Point3i[] allocTempScreens(int size)
		{
			Point3i[] tempScreens;
			int iFit = findBestFit(size, lengthsFreeScreens);
			if (iFit > 0)
			{
				tempScreens = freeScreens[iFit];
			}
			else
			{
				tempScreens = new Point3i[size];
				for (int i = size; --i >= 0; )
					tempScreens[i] = new Point3i();
			}
			return tempScreens;
		}
		
		internal virtual void  freeTempScreens(Point3i[] tempScreens)
		{
			int iFree = findShorter(tempScreens.length, lengthsFreeScreens);
			if (iFree >= 0)
				freeScreens[iFree] = tempScreens;
		}
		
		////////////////////////////////////////////////////////////////
		// temp booleans
		////////////////////////////////////////////////////////////////
		internal const int freeBooleansSize = 2;
		//UPGRADE_NOTE: Final was removed from the declaration of 'lengthsFreeBooleans '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal short[] lengthsFreeBooleans = new short[freeBooleansSize];
		//UPGRADE_NOTE: Final was removed from the declaration of 'freeBooleans '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal bool[][] freeBooleans = new bool[freeBooleansSize][];
		
		internal virtual bool[] allocTempBooleans(int size)
		{
			bool[] tempBooleans;
			int iFit = findBestFit(size, lengthsFreeBooleans);
			if (iFit > 0)
			{
				tempBooleans = freeBooleans[iFit];
			}
			else
			{
				tempBooleans = new bool[size];
			}
			return tempBooleans;
		}
		
		internal virtual void  freeTempBooleans(bool[] tempBooleans)
		{
			int iFree = findShorter(tempBooleans.Length, lengthsFreeBooleans);
			if (iFree >= 0)
				freeBooleans[iFree] = tempBooleans;
		}
	}
}