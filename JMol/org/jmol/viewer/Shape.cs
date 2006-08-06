/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-04-03 21:06:33 +0200 (lun., 03 avr. 2006) $
* $Revision: 4893 $
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
using org.jmol.g3d;
//UPGRADE_TODO: The type 'javax.vecmath.Point3f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Point3f = javax.vecmath.Point3f;
namespace org.jmol.viewer
{
	
	abstract class Shape
	{
		
		internal Viewer viewer;
		internal Frame frame;
		internal Graphics3D g3d;
		
		internal void  setViewerG3dFrame(Viewer viewer, Graphics3D g3d, Frame frame)
		{
			this.viewer = viewer;
			this.g3d = g3d;
			this.frame = frame;
			initShape();
		}
		
		internal virtual void  initShape()
		{
		}
		
		internal virtual void  setSize(int size, System.Collections.BitArray bsSelected)
		{
		}
		
		internal virtual void  setProperty(System.String propertyName, System.Object value_Renamed, System.Collections.BitArray bsSelected)
		{
			//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			System.Console.Out.WriteLine("unassigned property:" + propertyName + ":" + value_Renamed);
		}
		
		internal virtual void  invalidPropertyType(System.String propertyName, System.Object value_Renamed, System.String expectedMessage)
		{
			//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			System.Console.Out.WriteLine("invalid property type for " + propertyName + "\n" + " expected:" + expectedMessage + " received:" + value_Renamed);
		}
		
		internal virtual System.Object getProperty(System.String property, int index)
		{
			return null;
		}
		
		internal virtual bool wasClicked(int x, int y)
		{
			return false;
		}
		
		internal virtual void  findNearestAtomIndex(int xMouse, int yMouse, Closest closest)
		{
		}
		
		internal virtual void  checkBoundsMinMax(Point3f pointMin, Point3f pointMax)
		{
		}
	}
}