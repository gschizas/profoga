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
using Graphics3D = org.jmol.g3d.Graphics3D;
//UPGRADE_TODO: The type 'javax.vecmath.Point3i' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Point3i = javax.vecmath.Point3i;
namespace org.jmol.viewer
{
	
	class AxesRenderer:ShapeRenderer
	{
		public AxesRenderer()
		{
			InitBlock();
		}
		private void  InitBlock()
		{
			for (int i = 6; --i >= 0; )
				axisScreens[i] = new Point3i();
		}
		
		internal System.String[] axisLabels = new System.String[]{"+X", "+Y", "+Z", null, null, null};
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'axisScreens '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Point3i[] axisScreens = new Point3i[6];
		//UPGRADE_NOTE: Final was removed from the declaration of 'originScreen '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Point3i originScreen = new Point3i();
		
		internal override void  render()
		{
			Axes axes = (Axes) shape;
			short mad = axes.mad;
			if (mad == 0)
				return ;
			
			viewer.transformPoint(axes.originPoint, originScreen);
			for (int i = 6; --i >= 0; )
				viewer.transformPoint(axes.axisPoints[i], axisScreens[i]);
			
			int widthPixels = mad;
			if (mad >= 20)
				widthPixels = viewer.scaleToScreen(originScreen.z, mad);
			short colix = axes.colix;
			if (colix == 0)
				colix = Graphics3D.OLIVE;
			for (int i = 6; --i >= 0; )
			{
				if (mad < 0)
					g3d.drawDottedLine(colix, originScreen, axisScreens[i]);
				else
					g3d.fillCylinder(colix, Graphics3D.ENDCAPS_FLAT, widthPixels, originScreen, axisScreens[i]);
				System.String label = axisLabels[i];
				if (label != null)
					frameRenderer.renderStringOutside(label, colix, axes.font3d, axisScreens[i], g3d);
			}
		}
	}
}