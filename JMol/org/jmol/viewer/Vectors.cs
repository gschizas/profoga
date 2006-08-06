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
	
	class Vectors:Shape
	{
		
		internal System.String[] strings;
		internal short[] mads;
		internal short[] colixes;
		
		internal override void  initShape()
		{
			if (frame.hasVibrationVectors_Renamed_Field)
			{
				mads = new short[frame.atomCount];
				colixes = new short[frame.atomCount];
			}
		}
		
		internal override void  setSize(int size, System.Collections.BitArray bsSelected)
		{
			if (frame.hasVibrationVectors_Renamed_Field)
			{
				short mad = (short) size;
				//Atom[] atoms = frame.atoms;
				for (int i = frame.atomCount; --i >= 0; )
					if (bsSelected.Get(i))
						mads[i] = mad;
			}
		}
		
		internal override void  setProperty(System.String propertyName, System.Object value_Renamed, System.Collections.BitArray bsSelected)
		{
			if (frame.hasVibrationVectors_Renamed_Field)
			{
				//Atom[] atoms = frame.atoms;
				if ((System.Object) "color" == (System.Object) propertyName)
				{
					short colix = Graphics3D.getColix(value_Renamed);
					for (int i = frame.atomCount; --i >= 0; )
						if (bsSelected.Get(i))
							colixes[i] = colix;
				}
			}
		}
	}
}