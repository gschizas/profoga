/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-03-28 14:30:10 +0200 (mar., 28 mars 2006) $
* $Revision: 4828 $
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
namespace org.jmol.viewer
{
	
	class BallsRenderer:ShapeRenderer
	{
		
		internal int minX, maxX, minY, maxY;
		internal bool showHydrogens;
		internal short colixSelection;
		
		internal override void  render()
		{
			minX = rectClip.X;
			maxX = minX + rectClip.Width;
			minY = rectClip.Y;
			maxY = minY + rectClip.Height;
			
			colixSelection = viewer.ColixSelection;
			showHydrogens = viewer.ShowHydrogens;
			
			Atom[] atoms = frame.atoms;
			int displayModelIndex = this.displayModelIndex;
			if (displayModelIndex < 0)
			{
				for (int i = frame.atomCount; --i >= 0; )
				{
					Atom atom = atoms[i];
					atom.transform(viewer);
					render(atom);
				}
			}
			else
			{
				for (int i = frame.atomCount; --i >= 0; )
				{
					Atom atom = atoms[i];
					if (atom.modelIndex != displayModelIndex)
					{
						atom.formalChargeAndFlags &= ~ Atom.VISIBLE_FLAG;
						continue;
					}
					atom.transform(viewer);
					render(atom);
				}
			}
		}
		
		internal virtual void  render(Atom atom)
		{
			if (!showHydrogens && atom.elementNumber == 1)
				return ;
			int diameter = atom.screenDiameter;
			bool hasHalo = viewer.hasSelectionHalo(atom.atomIndex);
			if (diameter == 0 && !hasHalo)
			{
				atom.formalChargeAndFlags &= ~ Atom.VISIBLE_FLAG;
				return ;
			}
			// mth 2004 04 02 ... hmmm ... I don't like this here ... looks ugly
			atom.formalChargeAndFlags |= Atom.VISIBLE_FLAG;
			
			g3d.fillSphereCentered(atom.colixAtom, atom.screenDiameter, atom.screenX, atom.screenY, atom.screenZ);
			
			if (hasHalo)
			{
				int halowidth = diameter / 4;
				if (halowidth < 4)
					halowidth = 4;
				if (halowidth > 10)
					halowidth = 10;
				int haloDiameter = diameter + 2 * halowidth;
				g3d.fillScreenedCircleCentered(colixSelection, haloDiameter, atom.screenX, atom.screenY, atom.screenZ);
			}
		}
	}
}