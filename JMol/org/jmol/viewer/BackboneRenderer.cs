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
using org.jmol.g3d;
namespace org.jmol.viewer
{
	
	class BackboneRenderer:MpsRenderer
	{
		
		internal override void  renderMpspolymer(Mps.Mpspolymer mpspolymer)
		{
			renderBackboneChain((Backbone.Bbpolymer) mpspolymer);
		}
		
		internal virtual void  renderBackboneChain(Backbone.Bbpolymer bbpolymer)
		{
			render1Chain(bbpolymer.monomerCount, bbpolymer.polymer.LeadAtomIndices, bbpolymer.mads, bbpolymer.colixes);
		}
		
		internal virtual void  render1Chain(int monomerCount, int[] atomIndices, short[] mads, short[] colixes)
		{
			for (int i = monomerCount - 1; --i >= 0; )
			{
				if (mads[i] == 0)
					continue;
				
				Atom atomA = frame.getAtomAt(atomIndices[i]);
				Atom atomB = frame.getAtomAt(atomIndices[i + 1]);
				atomA.formalChargeAndFlags |= Atom.VISIBLE_FLAG;
				atomB.formalChargeAndFlags |= Atom.VISIBLE_FLAG;
				int xA = atomA.ScreenX, yA = atomA.ScreenY, zA = atomA.ScreenZ;
				int xB = atomB.ScreenX, yB = atomB.ScreenY, zB = atomB.ScreenZ;
				short colixA = Graphics3D.inheritColix(colixes[i], atomA.colixAtom);
				short colixB = Graphics3D.inheritColix(colixes[i + 1], atomB.colixAtom);
				if (mads[i] < 0)
				{
					g3d.drawLine(colixA, colixB, xA, yA, zA, xB, yB, zB);
				}
				else
				{
					int width = viewer.scaleToScreen((zA + zB) / 2, mads[i]);
					g3d.fillCylinder(colixA, colixB, Graphics3D.ENDCAPS_SPHERICAL, width, xA, yA, zA, xB, yB, zB);
				}
			}
		}
	}
}