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
using JmolAdapter = org.jmol.api.JmolAdapter;
namespace org.jmol.viewer
{
	
	
	sealed public class FrameExportJmolAdapter:JmolAdapter
	{
		
		internal Viewer viewer;
		internal Frame frame;
		
		internal FrameExportJmolAdapter(Viewer viewer, Frame frame):base("FrameExportJmolAdapter", null)
		{
			this.viewer = viewer;
			this.frame = frame;
		}
		
		public override System.String getAtomSetCollectionName(System.Object clientFile)
		{
			return viewer.ModelSetName;
		}
		
		public override int getEstimatedAtomCount(System.Object clientFile)
		{
			return frame.atomCount;
		}
		
		public override float[] getNotionalUnitcell(System.Object clientFile)
		{
			return frame.notionalUnitcell;
		}
		
		public override JmolAdapter.AtomIterator getAtomIterator(System.Object clientFile)
		{
			return new AtomIterator(this);
		}
		
		public override JmolAdapter.BondIterator getBondIterator(System.Object clientFile)
		{
			return new BondIterator(this);
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AtomIterator' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		new internal class AtomIterator:JmolAdapter.AtomIterator
		{
			public AtomIterator(FrameExportJmolAdapter enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(FrameExportJmolAdapter enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private FrameExportJmolAdapter enclosingInstance;
			public System.Object UniqueID
			{
				get
				{
					return (System.Int32) iatom;
				}
				
			}
			public int ElementNumber
			{
				get
				{
					return atom.elementNumber;
				}
				
			}
			public System.String ElementSymbol
			{
				get
				{
					return atom.ElementSymbol;
				}
				
			}
			public int FormalCharge
			{
				get
				{
					return atom.FormalCharge;
				}
				
			}
			public float PartialCharge
			{
				get
				{
					return atom.PartialCharge;
				}
				
			}
			public float X
			{
				get
				{
					return atom.AtomX;
				}
				
			}
			public float Y
			{
				get
				{
					return atom.AtomY;
				}
				
			}
			public float Z
			{
				get
				{
					return atom.AtomZ;
				}
				
			}
			public FrameExportJmolAdapter Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			internal int iatom;
			internal Atom atom;
			
			public bool hasNext()
			{
				if (iatom == Enclosing_Instance.frame.atomCount)
					return false;
				atom = Enclosing_Instance.frame.atoms[iatom++];
				return true;
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'BondIterator' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		new internal class BondIterator:JmolAdapter.BondIterator
		{
			public BondIterator(FrameExportJmolAdapter enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(FrameExportJmolAdapter enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private FrameExportJmolAdapter enclosingInstance;
			public System.Object AtomUniqueID1
			{
				get
				{
					return (System.Int32) bond.atom1.atomIndex;
				}
				
			}
			public System.Object AtomUniqueID2
			{
				get
				{
					return (System.Int32) bond.atom2.atomIndex;
				}
				
			}
			public int EncodedOrder
			{
				get
				{
					return bond.order;
				}
				
			}
			public FrameExportJmolAdapter Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			internal int ibond;
			internal Bond bond;
			
			public bool hasNext()
			{
				if (ibond >= Enclosing_Instance.frame.bondCount)
					return false;
				bond = Enclosing_Instance.frame.bonds[ibond++];
				return true;
			}
		}
	}
}