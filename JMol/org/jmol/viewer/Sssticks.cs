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
using Graphics3D = org.jmol.g3d.Graphics3D;
namespace org.jmol.viewer
{
	
	class Sssticks:Sticks
	{
		
		internal override void  setSize(int size, System.Collections.BitArray bsSelected)
		{
			short mad = (short) size;
			setMadBond(mad, JmolConstants.BOND_SULFUR_MASK, bsSelected);
		}
		
		internal override void  setProperty(System.String propertyName, System.Object value_Renamed, System.Collections.BitArray bsSelected)
		{
			if ((System.Object) "color" == (System.Object) propertyName)
			{
				short colix = Graphics3D.getColix(value_Renamed);
				setColixBond(colix, ((colix != Graphics3D.UNRECOGNIZED)?null:(System.String) value_Renamed), JmolConstants.BOND_SULFUR_MASK, bsSelected);
				return ;
			}
			if ((System.Object) "translucency" == (System.Object) propertyName)
			{
				setTranslucencyBond(value_Renamed == (System.Object) "translucent", JmolConstants.BOND_SULFUR_MASK, bsSelected);
				return ;
			}
			/*
			perhaps broken because of elminiation of colorScheme
			if ("colorScheme" == propertyName) {
			if (value instanceof String) {
			if ("cpk" == (String)value) {
			setColixBond((short)0, JmolConstants.BOND_SULFUR_MASK, bsSelected);
			return;
			}
			}
			return;
			}
			*/
		}
	}
}