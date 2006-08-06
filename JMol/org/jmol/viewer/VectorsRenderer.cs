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
*  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*/
using System;
using org.jmol.g3d;
//UPGRADE_TODO: The package 'javax.vecmath' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using javax.vecmath;
namespace org.jmol.viewer
{
	
	class VectorsRenderer:ShapeRenderer
	{
		
		internal override void  render()
		{
			if (!frame.hasVibrationVectors_Renamed_Field)
				return ;
			Atom[] atoms = frame.atoms;
			Vectors vectors = (Vectors) shape;
			short[] mads = vectors.mads;
			if (mads == null)
				return ;
			short[] colixes = vectors.colixes;
			int displayModelIndex = this.displayModelIndex;
			for (int i = frame.atomCount; --i >= 0; )
			{
				Atom atom = atoms[i];
				if (mads[i] == 0 || (displayModelIndex >= 0 && atom.modelIndex != displayModelIndex))
					continue;
				Vector3f vibrationVector = atom.VibrationVector;
				if (vibrationVector == null)
					continue;
				if (transform(mads[i], atom, vibrationVector))
					renderVector(colixes[i], atom);
			}
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'pointVectorEnd '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Point3f pointVectorEnd = new Point3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'pointArrowHead '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Point3f pointArrowHead = new Point3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'screenVectorEnd '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Point3i screenVectorEnd = new Point3i();
		//UPGRADE_NOTE: Final was removed from the declaration of 'screenArrowHead '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Point3i screenArrowHead = new Point3i();
		//UPGRADE_NOTE: Final was removed from the declaration of 'vibrationVectorScaled '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f vibrationVectorScaled = new Vector3f();
		internal int diameter;
		internal float headWidthAngstroms;
		internal int headWidthPixels;
		
		internal const float arrowHeadBase = 0.8f;
		
		internal virtual bool transform(short mad, Atom atom, Vector3f vibrationVector)
		{
			if (atom.madAtom == JmolConstants.MAR_DELETED)
				return false;
			
			// to have the vectors stay in the the same spot
			/*
			float vectorScale = viewer.getVectorScale();
			pointVectorEnd.scaleAdd(vectorScale, atom.vibrationVector, atom.point3f);
			viewer.transformPoint(pointVectorEnd, screenVectorEnd);
			diameter = (mad <= 20)
			? mad
			: viewer.scaleToScreen(screenVectorEnd.z, mad);
			pointArrowHead.scaleAdd(vectorScale * arrowHeadBase,
			atom.vibrationVector, atom.point3f);
			viewer.transformPoint(pointArrowHead, screenArrowHead);
			headWidthPixels = diameter * 3 / 2;
			if (headWidthPixels < diameter + 2)
			headWidthPixels = diameter + 2;
			return true;
			*/
			
			// to have the vectors move when vibration is turned on
			float vectorScale = viewer.getVectorScale();
			pointVectorEnd.scaleAdd(vectorScale, vibrationVector, atom.point3f);
			viewer.transformPoint(pointVectorEnd, vibrationVector, screenVectorEnd);
			diameter = (mad <= 20)?mad:viewer.scaleToScreen(screenVectorEnd.z, mad);
			pointArrowHead.scaleAdd(vectorScale * arrowHeadBase, vibrationVector, atom.point3f);
			viewer.transformPoint(pointArrowHead, vibrationVector, screenArrowHead);
			headWidthPixels = diameter * 3 / 2;
			if (headWidthPixels < diameter + 2)
				headWidthPixels = diameter + 2;
			return true;
		}
		
		internal virtual void  renderVector(short colix, Atom atom)
		{
			colix = Graphics3D.inheritColix(colix, atom.colixAtom);
			g3d.fillCylinder(colix, Graphics3D.ENDCAPS_OPEN, diameter, atom.ScreenX, atom.ScreenY, atom.ScreenZ, screenArrowHead.x, screenArrowHead.y, screenArrowHead.z);
			g3d.fillCone(colix, Graphics3D.ENDCAPS_NONE, headWidthPixels, screenArrowHead, screenVectorEnd);
		}
	}
}