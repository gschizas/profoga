/* $RCSfile$
* $Author: hansonr $
* $Date: 2006-04-11 15:04:41 +0200 (mar., 11 avr. 2006) $
* $Revision: 4954 $
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
using Graphics3D = org.jmol.g3d.Graphics3D;
//UPGRADE_TODO: The type 'javax.vecmath.Point3i' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Point3i = javax.vecmath.Point3i;
namespace org.jmol.viewer
{
	
	
	class PolyhedraRenderer:ShapeRenderer
	{
		
		internal override void  render()
		{
			Polyhedra polyhedra = (Polyhedra) shape;
			Polyhedra.Polyhedron[] polyhedrons = polyhedra.polyhedrons;
			for (int i = polyhedra.polyhedronCount; --i >= 0; )
				render1(polyhedrons[i]);
		}
		
		internal virtual void  render1(Polyhedra.Polyhedron p)
		{
			int displayModelIndex = this.displayModelIndex;
			if (displayModelIndex >= 0 && p.centralAtom.modelIndex != displayModelIndex)
				return ;
			if (!p.visible)
				return ;
			short colix = Graphics3D.inheritColix(p.polyhedronColix, p.centralAtom.colixAtom);
			if (p.collapsed)
				renderCollapsed(p, colix);
			else
				renderFlat(p, colix);
		}
		
		internal virtual void  renderFlat(Polyhedra.Polyhedron p, short colix)
		{
			Atom[] vertexAtoms = p.vertexAtoms;
			sbyte[] planes = p.planes;
			short[] normixes = p.normixes;
			int drawEdges = p.edges;
			
			for (int i = 0, j = 0; j < planes.Length; )
				drawFace(colix, drawEdges, normixes[i++], vertexAtoms[planes[j++]], vertexAtoms[planes[j++]], vertexAtoms[planes[j++]]);
			for (int i = 0, j = 0; j < planes.Length; )
				fillFace(colix, normixes[i++], vertexAtoms[planes[j++]], vertexAtoms[planes[j++]], vertexAtoms[planes[j++]]);
		}
		
		internal virtual void  drawFace(short colix, int drawEdges, short normix, Atom atomA, Atom atomB, Atom atomC)
		{
			if (drawEdges == Polyhedra.EDGES_ALL || (drawEdges == Polyhedra.EDGES_FRONT && g3d.isDirectedTowardsCamera(normix)))
			{
				g3d.drawCylinderTriangle(Graphics3D.getOpaqueColix(colix), atomA.screenX, atomA.screenY, atomA.screenZ, atomB.screenX, atomB.screenY, atomB.screenZ, atomC.screenX, atomC.screenY, atomC.screenZ, 3);
			}
		}
		
		internal virtual void  fillFace(short colix, short normix, Atom atomA, Atom atomB, Atom atomC)
		{
			g3d.fillTriangle(colix, normix, atomA.screenX, atomA.screenY, atomA.screenZ, atomB.screenX, atomB.screenY, atomB.screenZ, atomC.screenX, atomC.screenY, atomC.screenZ);
		}
		
		internal virtual void  renderCollapsed(Polyhedra.Polyhedron p, short colix)
		{
			Point3i[] screens = viewer.allocTempScreens(p.faceCount);
			viewer.transformPoints(p.collapsedCenters, screens);
			
			Atom[] vertexAtoms = p.vertexAtoms;
			sbyte[] planes = p.planes;
			short[] normixes = p.collapsedNormixes;
			int drawEdges = p.edges;
			
			for (int i = p.faceCount; --i >= 0; )
				renderCollapsedFace(colix, drawEdges, screens[i], vertexAtoms[planes[3 * i + 0]], vertexAtoms[planes[3 * i + 1]], vertexAtoms[planes[3 * i + 2]], normixes[3 * i + 0], normixes[3 * i + 1], normixes[3 * i + 2]);
			viewer.freeTempScreens(screens);
		}
		
		internal virtual void  renderCollapsedFace(short colix, int drawEdges, Point3i center, Atom atomA, Atom atomB, Atom atomC, short normixA, short normixB, short normixC)
		{
			drawCollapsed(colix, drawEdges, normixA, center, atomB, atomC);
			drawCollapsed(colix, drawEdges, normixB, center, atomA, atomC);
			drawCollapsed(colix, drawEdges, normixC, center, atomA, atomB);
			fillCollapsed(colix, normixA, center, atomB, atomC);
			fillCollapsed(colix, normixB, center, atomA, atomC);
			fillCollapsed(colix, normixC, center, atomA, atomB);
		}
		
		internal virtual void  drawCollapsed(short colix, int drawEdges, short normix, Point3i collapsed, Atom atomB, Atom atomC)
		{
			if (drawEdges == Polyhedra.EDGES_ALL || (drawEdges == Polyhedra.EDGES_FRONT && g3d.isDirectedTowardsCamera(normix)))
			{
				g3d.drawCylinderTriangle(Graphics3D.getOpaqueColix(colix), collapsed.x, collapsed.y, collapsed.z, atomB.screenX, atomB.screenY, atomB.screenZ, atomC.screenX, atomC.screenY, atomC.screenZ, 3);
			}
		}
		
		internal virtual void  fillCollapsed(short colix, short normix, Point3i collapsed, Atom atomB, Atom atomC)
		{
			g3d.fillTriangle(colix, normix, collapsed.x, collapsed.y, collapsed.z, atomB.screenX, atomB.screenY, atomB.screenZ, atomC.screenX, atomC.screenY, atomC.screenZ);
		}
	}
}