/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-03-31 02:27:49 +0200 (ven., 31 mars 2006) $
* $Revision: 4858 $
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
using Graphics3D = org.jmol.g3d.Graphics3D;
namespace org.jmol.viewer
{
	
	class Bond
	{
		virtual internal bool Covalent
		{
			get
			{
				return (order & JmolConstants.BOND_COVALENT_MASK) != 0;
			}
			
		}
		virtual internal bool Stereo
		{
			get
			{
				return (order & JmolConstants.BOND_STEREO_MASK) != 0;
			}
			
		}
		virtual internal bool Aromatic
		{
			get
			{
				return (order & JmolConstants.BOND_AROMATIC_MASK) != 0;
			}
			
		}
		virtual internal short Mad
		{
			set
			{
				this.mad = value;
			}
			
		}
		virtual internal short Colix
		{
			set
			{
				this.colix = value;
			}
			
		}
		virtual internal bool Translucent
		{
			set
			{
				colix = Graphics3D.setTranslucent(colix, value);
			}
			
		}
		virtual internal short Order
		{
			get
			{
				return order;
			}
			
			set
			{
				this.order = value;
			}
			
		}
		virtual internal Atom Atom1
		{
			get
			{
				return atom1;
			}
			
		}
		virtual internal Atom Atom2
		{
			get
			{
				return atom2;
			}
			
		}
		virtual internal float Radius
		{
			get
			{
				return mad / 2000f;
			}
			
		}
		virtual internal System.String OrderName
		{
			get
			{
				switch (order)
				{
					
					case 1: 
						return "single";
					
					case 2: 
						return "double";
					
					case 3: 
						return "triple";
					
					case 4: 
						return "aromatic";
					}
				if ((order & JmolConstants.BOND_HYDROGEN_MASK) != 0)
					return "hbond";
				return "unknown";
			}
			
		}
		virtual internal short Colix1
		{
			get
			{
				return Graphics3D.inheritColix(colix, atom1.colixAtom);
			}
			
		}
		virtual internal int Argb1
		{
			get
			{
				return atom1.group.chain.frame.viewer.getColixArgb(Colix1);
			}
			
		}
		virtual internal short Colix2
		{
			get
			{
				return Graphics3D.inheritColix(colix, atom2.colixAtom);
			}
			
		}
		virtual internal int Argb2
		{
			get
			{
				return atom1.group.chain.frame.viewer.getColixArgb(Colix2);
			}
			
		}
		virtual internal System.Collections.Hashtable PublicProperties
		{
			////////////////////////////////////////////////////////////////
			
			
			get
			{
				System.Collections.Hashtable ht = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
				ht["atomIndexA"] = (System.Int32) atom1.atomIndex;
				ht["atomIndexB"] = (System.Int32) atom2.atomIndex;
				ht["argbA"] = (System.Int32) Argb1;
				ht["argbB"] = (System.Int32) Argb2;
				ht["order"] = OrderName;
				ht["radius"] = (double) Radius;
				ht["modelIndex"] = (System.Int32) atom1.modelIndex;
				ht["xA"] = new Double(atom1.point3f.x);
				ht["yA"] = new Double(atom1.point3f.y);
				ht["zA"] = new Double(atom1.point3f.z);
				ht["xB"] = new Double(atom2.point3f.x);
				ht["yB"] = new Double(atom2.point3f.y);
				ht["zB"] = new Double(atom2.point3f.z);
				return ht;
			}
			
		}
		
		internal Atom atom1;
		internal Atom atom2;
		internal short order;
		internal short mad;
		internal short colix;
		
		internal Bond(Atom atom1, Atom atom2, short order, short mad, short colix)
		{
			if (atom1 == null)
				throw new System.NullReferenceException();
			if (atom2 == null)
				throw new System.NullReferenceException();
			this.atom1 = atom1;
			this.atom2 = atom2;
			if (atom1.elementNumber == 16 && atom2.elementNumber == 16)
				order |= JmolConstants.BOND_SULFUR_MASK;
			if (order == JmolConstants.BOND_AROMATIC_MASK)
				order = JmolConstants.BOND_AROMATIC;
			this.order = order;
			this.mad = mad;
			this.colix = colix;
		}
		
		internal Bond(Atom atom1, Atom atom2, short order, Frame frame):this(atom1, atom2, order, ((order & JmolConstants.BOND_HYDROGEN_MASK) != 0?1:frame.viewer.MadBond), (short) 0)
		{
		}
		
		internal virtual void  deleteAtomReferences()
		{
			if (atom1 != null)
				atom1.deleteBond(this);
			if (atom2 != null)
				atom2.deleteBond(this);
			atom1 = atom2 = null;
		}
	}
}