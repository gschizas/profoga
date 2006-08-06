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
//UPGRADE_TODO: The type 'javax.vecmath.Point3f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Point3f = javax.vecmath.Point3f;
//UPGRADE_TODO: The type 'javax.vecmath.Point3i' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Point3i = javax.vecmath.Point3i;
namespace org.jmol.viewer
{
	
	class BbcageRenderer:ShapeRenderer
	{
		public BbcageRenderer()
		{
			InitBlock();
		}
		private void  InitBlock()
		{
			for (int i = 8; --i >= 0; )
				screens[i] = new Point3i();
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'screens '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Point3i[] screens = new Point3i[8];
		
		internal override void  render()
		{
			Bbcage bbcage = (Bbcage) shape;
			short mad = bbcage.mad;
			if (mad == 0)
				return ;
			short colix = bbcage.colix;
			if (colix == 0)
				colix = Graphics3D.OLIVE;
			render(viewer, g3d, mad, colix, frame.bboxVertices, screens);
		}
		
		internal static void  render(Viewer viewer, Graphics3D g3d, short mad, short colix, Point3f[] vertices, Point3i[] screens)
		{
			int zSum = 0;
			for (int i = 8; --i >= 0; )
			{
				viewer.transformPoint(vertices[i], screens[i]);
				zSum += screens[i].z;
			}
			int widthPixels = mad;
			if (mad >= 20)
			{
				widthPixels = viewer.scaleToScreen(zSum / 8, mad);
			}
			for (int i = 0; i < 24; i += 2)
			{
				if (mad < 0)
					g3d.drawDottedLine(colix, screens[Bbcage.edges[i]], screens[Bbcage.edges[i + 1]]);
				else
					g3d.fillCylinder(colix, Graphics3D.ENDCAPS_SPHERICAL, widthPixels, screens[Bbcage.edges[i]], screens[Bbcage.edges[i + 1]]);
			}
		}
	}
}