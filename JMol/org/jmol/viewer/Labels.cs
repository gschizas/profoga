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
using org.jmol.g3d;
namespace org.jmol.viewer
{
	
	class Labels:Shape
	{
		
		internal System.String[] strings;
		internal short[] colixes;
		internal short[] bgcolixes;
		internal sbyte[] fids;
		internal short[] offsets;
		
		internal Font3D defaultFont3D;
		
		internal override void  initShape()
		{
			defaultFont3D = g3d.getFont3D(JmolConstants.DEFAULT_FONTFACE, JmolConstants.DEFAULT_FONTSTYLE, JmolConstants.LABEL_DEFAULT_FONTSIZE);
		}
		
		internal override void  setProperty(System.String propertyName, System.Object value_Renamed, System.Collections.BitArray bsSelected)
		{
			Atom[] atoms = frame.atoms;
			if ((System.Object) "color" == (System.Object) propertyName)
			{
				System.String palette = null;
				short colix = Graphics3D.getColix(value_Renamed);
				if (colix == Graphics3D.UNRECOGNIZED)
					palette = ((System.String) value_Renamed);
				for (int i = frame.atomCount; --i >= 0; )
					if (bsSelected.Get(i))
					{
						if (colixes == null || i >= colixes.Length)
						{
							if (colix == 0)
								continue;
							colixes = Util.ensureLength(colixes, i + 1);
						}
						colixes[i] = ((colix != Graphics3D.UNRECOGNIZED)?colix:viewer.getColixAtomPalette(frame.getAtomAt(i), palette));
					}
			}
			// no translucency
			if ((System.Object) "bgcolor" == (System.Object) propertyName)
			{
				short bgcolix = Graphics3D.getColix(value_Renamed);
				for (int i = frame.atomCount; --i >= 0; )
					if (bsSelected.Get(i))
					{
						//Atom atom = atoms[i];
						if (bgcolixes == null || i >= bgcolixes.Length)
						{
							if (bgcolix == 0)
								continue;
							bgcolixes = Util.ensureLength(bgcolixes, i + 1);
						}
						bgcolixes[i] = bgcolix;
					}
			}
			
			if ((System.Object) "label" == (System.Object) propertyName)
			{
				System.String strLabel = (System.String) value_Renamed;
				for (int i = frame.atomCount; --i >= 0; )
					if (bsSelected.Get(i))
					{
						Atom atom = atoms[i];
						System.String label = atom.formatLabel(strLabel);
						if (strings == null || i >= strings.Length)
						{
							if (label == null)
								continue;
							strings = Util.ensureLength(strings, i + 1);
						}
						strings[i] = label;
					}
				return ;
			}
			
			if ((System.Object) "fontsize" == (System.Object) propertyName)
			{
				int fontsize = ((System.Int32) value_Renamed);
				if (fontsize == JmolConstants.LABEL_DEFAULT_FONTSIZE)
				{
					fids = null;
					return ;
				}
				sbyte fid = g3d.getFontFid(fontsize);
				fids = Util.ensureLength(fids, frame.atomCount);
				for (int i = frame.atomCount; --i >= 0; )
					fids[i] = fid;
				return ;
			}
			
			if ((System.Object) "font" == (System.Object) propertyName)
			{
				sbyte fid = ((Font3D) value_Renamed).fid;
				for (int i = frame.atomCount; --i >= 0; )
					if (bsSelected.Get(i))
					{
						if (fids == null || i >= fids.Length)
						{
							if (fid == defaultFont3D.fid)
								continue;
							fids = Util.ensureLength(fids, i + 1);
						}
						fids[i] = fid;
					}
				return ;
			}
			
			if ((System.Object) "offset" == (System.Object) propertyName)
			{
				int offset = ((System.Int32) value_Renamed);
				if (offset == 0)
					offset = System.Int16.MinValue;
				else if (offset == ((JmolConstants.LABEL_DEFAULT_X_OFFSET << 8) | JmolConstants.LABEL_DEFAULT_Y_OFFSET))
					offset = 0;
				for (int i = frame.atomCount; --i >= 0; )
					if (bsSelected.Get(i))
					{
						if (offsets == null || i >= offsets.Length)
						{
							if (offset == 0)
								continue;
							offsets = Util.ensureLength(offsets, i + 1);
						}
						offsets[i] = (short) offset;
					}
				return ;
			}
			
			if ((System.Object) "pickingLabel" == (System.Object) propertyName)
			{
				// toggle
				int atomIndex = ((System.Int32) value_Renamed);
				if (strings != null && strings.Length > atomIndex && strings[atomIndex] != null)
				{
					strings[atomIndex] = null;
				}
				else
				{
					System.String strLabel;
					if (viewer.ModelCount > 1)
						strLabel = "[%n]%r:%c.%a/%M";
					else if (viewer.ChainCount > 1)
						strLabel = "[%n]%r:%c.%a";
					else if (viewer.GroupCount <= 1)
						strLabel = "%e%i";
					else
						strLabel = "[%n]%r.%a";
					Atom atom = atoms[atomIndex];
					strings = Util.ensureLength(strings, atomIndex + 1);
					strings[atomIndex] = atom.formatLabel(strLabel);
				}
				return ;
			}
		}
	}
}