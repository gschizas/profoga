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
*  Lesser General License for more details.
*
*  You should have received a copy of the GNU Lesser General Public
*  License along with this library; if not, write to the Free Software
*  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*/
using System;
using Graphics3D = org.jmol.g3d.Graphics3D;
namespace org.jmol.viewer
{
	
	class Balls:Shape
	{
		internal override void  setSize(int size, System.Collections.BitArray bsSelected)
		{
			short mad = (short) size;
			Atom[] atoms = frame.atoms;
			for (int i = frame.atomCount; --i >= 0; )
				if (bsSelected.Get(i))
					atoms[i].MadAtom = mad;
		}
		
		internal override void  setProperty(System.String propertyName, System.Object value_Renamed, System.Collections.BitArray bs)
		{
			int atomCount = frame.atomCount;
			Atom[] atoms = frame.atoms;
			if ((System.Object) "color" == (System.Object) propertyName)
			{
				short colix = Graphics3D.getColix(value_Renamed);
				for (int i = atomCount; --i >= 0; )
					if (bs.Get(i))
					{
						Atom atom = atoms[i];
						atom.ColixAtom = (colix != Graphics3D.UNRECOGNIZED)?colix:viewer.getColixAtomPalette(atom, (System.String) value_Renamed);
					}
				return ;
			}
			if ((System.Object) "translucency" == (System.Object) propertyName)
			{
				for (int i = atomCount; --i >= 0; )
					if (bs.Get(i))
						atoms[i].Translucent = value_Renamed == (System.Object) "translucent";
				return ;
			}
		}
		
		internal const int minimumPixelSelectionRadius = 6;
		
		/*
		* This algorithm assumes that atoms are circles at the z-depth
		* of their center point. Therefore, it probably has some flaws
		* around the edges when dealing with intersecting spheres that
		* are at approximately the same z-depth.
		* But it is much easier to deal with than trying to actually
		* calculate which atom was clicked
		*
		* A more general algorithm of recording which object drew
		* which pixel would be very expensive and not worth the trouble
		*/
		internal override void  findNearestAtomIndex(int x, int y, Closest closest)
		{
			if (frame.atomCount == 0)
				return ;
			Atom champion = null;
			//int championIndex = -1;
			for (int i = frame.atomCount; --i >= 0; )
			{
				Atom contender = frame.atoms[i];
				if (contender.isCursorOnTopOfVisibleAtom(x, y, minimumPixelSelectionRadius, champion))
				{
					champion = contender;
					//championIndex = i;
				}
			}
			closest.atom = champion;
		}
	}
}