/* $RCSfile$
* $Author: egonw $
* $Date: 2005-11-10 16:52:44 +0100 (jeu., 10 nov. 2005) $
* $Revision: 4255 $
*
* Copyright (C) 2002-2005  The Jmol Development Team
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
//UPGRADE_TODO: The type 'javax.vecmath.Point3i' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Point3i = javax.vecmath.Point3i;
//UPGRADE_TODO: The type 'javax.vecmath.Vector3f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Vector3f = javax.vecmath.Vector3f;
namespace org.jmol.viewer
{
	
	
	class PruebaRenderer:ShapeRenderer
	{
		
		private const int level = 1;
		
		internal override void  render()
		{
			Prueba prueba = (Prueba) shape;
			short colix = prueba.colix;
			
			int vertexCount = g3d.getGeodesicVertexCount(level);
			//Vector3f[] vectors = g3d.getGeodesicVertexVectors();
			Vector3f[] tvs = g3d.TransformedVertexVectors;
			Point3i[] screens = viewer.allocTempScreens(vertexCount);
			short[] geodesicFaceVertexes = g3d.getGeodesicFaceVertexes(level);
			int geodesicFaceCount = g3d.getGeodesicFaceCount(level);
			
			calcScreens(vertexCount, tvs, screens);
			
			for (int i = geodesicFaceCount, j = 0; --i >= 0; )
			{
				short vA = geodesicFaceVertexes[j++];
				short vB = geodesicFaceVertexes[j++];
				short vC = geodesicFaceVertexes[j++];
				g3d.fillTriangle(colix, screens[vA], vA, screens[vB], vB, screens[vC], vC);
			}
			viewer.freeTempScreens(screens);
		}
		
		internal virtual void  calcScreens(int count, Vector3f[] tvs, Point3i[] screens)
		{
			float scaledRadius = viewer.scaleToScreen(1000, 1f);
			for (int i = count; --i >= 0; )
			{
				Vector3f tv = tvs[i];
				Point3i screen = screens[i];
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				screen.x = 150 + (int) (scaledRadius * tv.x);
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				screen.y = 150 - (int) (scaledRadius * tv.y); // y inverted on screen!
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				screen.z = 1000 - (int) (scaledRadius * tv.z); // smaller z comes to me
			}
		}
	}
}