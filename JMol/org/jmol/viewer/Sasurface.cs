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
using Graphics3D = org.jmol.g3d.Graphics3D;
namespace org.jmol.viewer
{
	
	class Sasurface:Shape
	{
		
		internal static int MAX_GEODESIC_RENDERING_LEVEL = 2;
		
		// note that when there is a full torus
		// the 0 point will be repeated as 2*PI
		internal static int MAX_FULL_TORUS_STEP_COUNT = 15;
		
		// note that the outer torus is at most 180 degrees
		// so this step count is over 180 degrees, not 360
		internal static int OUTER_TORUS_STEP_COUNT = 9;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'MAX_TORUS_POINTS '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int MAX_TORUS_POINTS = MAX_FULL_TORUS_STEP_COUNT * OUTER_TORUS_STEP_COUNT;
		
		
		internal int surfaceCount;
		internal Sasurface1[] surfaces = new Sasurface1[4];
		
		internal Sasurface1 currentSurface;
		
		internal override void  initShape()
		{
		}
		
		internal override void  setSize(int size, System.Collections.BitArray bsSelected)
		{
			dumpState("setSize:" + size);
			if (currentSurface != null)
				currentSurface.setSize(size, bsSelected);
			else
			{
				for (int i = surfaceCount; --i >= 0; )
					surfaces[i].setSize(size, bsSelected);
			}
		}
		
		internal override void  setProperty(System.String propertyName, System.Object value_Renamed, System.Collections.BitArray bs)
		{
			//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			System.Console.Out.WriteLine("Sasurface.setProperty(" + propertyName + "," + value_Renamed + ")");
			//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			dumpState("setProperty:" + propertyName + ":" + value_Renamed);
			if ((System.Object) "surfaceID" == (System.Object) propertyName)
			{
				System.String surfaceID = (System.String) value_Renamed;
				System.Console.Out.WriteLine("surfaceID=" + surfaceID);
				if (surfaceID == null)
				{
					currentSurface = null;
					System.Console.Out.WriteLine("----> null surfaceID");
					return ;
				}
				for (int i = surfaceCount; --i >= 0; )
				{
					currentSurface = surfaces[i];
					if (surfaceID.Equals(currentSurface.surfaceID))
					{
						System.Console.Out.WriteLine("surfaceID is set to" + surfaceID);
						return ;
					}
				}
				allocSurface(surfaceID, bs);
				dumpState("done");
				return ;
			}
			
			if ((System.Object) "delete" == (System.Object) propertyName)
			{
				System.Console.Out.WriteLine("delete && surfaceCount=" + surfaceCount);
				if (currentSurface != null)
				{
					System.Console.Out.WriteLine("deleting currentSurface:" + currentSurface.surfaceID);
					int iCurrent;
					for (iCurrent = surfaceCount; surfaces[--iCurrent] != currentSurface; )
					{
					}
					for (int j = iCurrent + 1; j < surfaceCount; ++j)
						surfaces[j - 1] = surfaces[j];
					surfaces[--surfaceCount] = null;
					currentSurface = null;
				}
				else
				{
					System.Console.Out.WriteLine("deleting all surfaces");
					for (int i = surfaceCount; --i >= 0; )
						surfaces[i] = null;
					surfaceCount = 0;
				}
				return ;
			}
			
			if (currentSurface != null)
				currentSurface.setProperty(propertyName, value_Renamed, bs);
			else
				for (int i = surfaceCount; --i >= 0; )
					surfaces[i].setProperty(propertyName, value_Renamed, bs);
		}
		
		internal virtual void  allocSurface(System.String surfaceID, System.Collections.BitArray bs)
		{
			System.Console.Out.WriteLine("allocSurface(" + surfaceID + ")");
			surfaces = (Sasurface1[]) Util.ensureLength(surfaces, surfaceCount + 1);
			currentSurface = surfaces[surfaceCount++] = new Sasurface1(surfaceID, viewer, g3d, Graphics3D.YELLOW, bs);
		}
		
		internal virtual void  dumpState(System.String msg)
		{
			System.Console.Out.WriteLine(">>>>>>>>>>>>>>>>>>>>> " + msg);
			System.Console.Out.WriteLine("surfaceCount=" + surfaceCount);
			System.Console.Out.WriteLine("currentSurface=" + (currentSurface == null?"NULL":currentSurface.surfaceID));
		}
	}
}