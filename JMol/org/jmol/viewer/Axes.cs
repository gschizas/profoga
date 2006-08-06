/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-03-27 23:44:15 +0200 (lun., 27 mars 2006) $
* $Revision: 4792 $
*
* Copyright (C) 2002-2006  Miguel, Jmol Development, www.jmol.org
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
//UPGRADE_TODO: The type 'javax.vecmath.Point3f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Point3f = javax.vecmath.Point3f;
//UPGRADE_TODO: The type 'javax.vecmath.Vector3f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Vector3f = javax.vecmath.Vector3f;
namespace org.jmol.viewer
{
	
	class Axes:SelectionIndependentShape
	{
		public Axes()
		{
			InitBlock();
		}
		private void  InitBlock()
		{
			for (int i = 6; --i >= 0; )
				axisPoints[i] = new Point3f();
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'unitAxisPoints '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly Point3f[] unitAxisPoints = new Point3f[]{new Point3f(1, 0, 0), new Point3f(0, 1, 0), new Point3f(0, 0, 1), new Point3f(- 1, 0, 0), new Point3f(0, - 1, 0), new Point3f(0, 0, - 1)};
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'originPoint '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Point3f originPoint = new Point3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'axisPoints '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Point3f[] axisPoints = new Point3f[6];
		
		internal override void  initShape()
		{
			originPoint.set_Renamed(viewer.BoundBoxCenter);
			Vector3f corner = viewer.BoundBoxCornerVector;
			for (int i = 6; --i >= 0; )
			{
				Point3f axisPoint = axisPoints[i];
				axisPoint.set_Renamed(unitAxisPoints[i]);
				// we have just set the axisPoint to be a unit on a single axis
				// therefore only one of these values (x, y, or z) will be nonzero
				// it will have value 1 or -1
				axisPoint.x *= corner.x;
				axisPoint.y *= corner.y;
				axisPoint.z *= corner.z;
				axisPoint.add(originPoint);
				
				font3d = g3d.getFont3D(JmolConstants.AXES_DEFAULT_FONTSIZE);
			}
		}
	}
}