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
//UPGRADE_TODO: The type 'javax.vecmath.Point3i' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Point3i = javax.vecmath.Point3i;
using Graphics3D = org.jmol.g3d.Graphics3D;
namespace org.jmol.viewer
{
	
	class UccageRenderer:ShapeRenderer
	{
		public UccageRenderer()
		{
			InitBlock();
		}
		private void  InitBlock()
		{
			for (int i = 8; --i >= 0; )
				screens[i] = new Point3i();
		}
		
		internal SupportClass.TextNumberFormat nf;
		internal sbyte fid;
		
		internal override void  initRenderer()
		{
			nf = SupportClass.TextNumberFormat.getTextNumberInstance();
			fid = g3d.getFontFid("Monospaced", 12);
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'screens '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Point3i[] screens = new Point3i[8];
		
		internal override void  render()
		{
			Uccage uccage = (Uccage) shape;
			short mad = uccage.mad;
			if (mad == 0 || !uccage.hasUnitcell)
				return ;
			short colix = uccage.colix;
			if (colix == 0)
				colix = Graphics3D.OLIVE;
			BbcageRenderer.render(viewer, g3d, mad, colix, frame.unitcellVertices, screens);
			/*
			render(viewer, g3d, mad, bbox.colix, bbox.bboxVertices, bboxScreens);
			
			Point3i[] screens = frameRenderer.getTempScreens(8);
			for (int i = 8; --i >= 0; )
			viewer.transformPoint(uccage.vertices[i], screens[i]);
			short colix = uccage.colix;
			for (int i = 0; i < 24; i += 2) {
			Point3i screenA = screens[Bbox.edges[i]];
			Point3i screenB = screens[Bbox.edges[i+1]];
			if (i < 6) {
			g3d.drawLine(colix, screenA, screenB);
			} else {
			g3d.drawDottedLine(colix, screenA, screenB);
			}
			}
			*/
			
			g3d.setFont(fid);
			nf.setMaximumFractionDigits(3);
			nf.setMinimumFractionDigits(3);
			g3d.drawString("a=" + nf.FormatDouble(uccage.a) + "\u00C5", colix, 5, 15, 0);
			g3d.drawString("b=" + nf.FormatDouble(uccage.b) + "\u00C5", colix, 5, 30, 0);
			g3d.drawString("c=" + nf.FormatDouble(uccage.c) + "\u00C5", colix, 5, 45, 0);
			nf.setMaximumFractionDigits(1);
			g3d.drawString("\u03B1=" + nf.FormatDouble(uccage.alpha) + "\u00B0", colix, 5, 60, 0);
			g3d.drawString("\u03B2=" + nf.FormatDouble(uccage.beta) + "\u00B0", colix, 5, 75, 0);
			g3d.drawString("\u03B3=" + nf.FormatDouble(uccage.gamma) + "\u00B0", colix, 5, 90, 0);
		}
	}
}