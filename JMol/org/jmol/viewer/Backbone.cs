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
*  Lesser General License for more details.
*
*  You should have received a copy of the GNU Lesser General Public
*  License along with this library; if not, write to the Free Software
*  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*/
using System;
namespace org.jmol.viewer
{
	
	class Backbone:Mps
	{
		
		internal override Mps.Mpspolymer allocateMpspolymer(Polymer polymer)
		{
			return new Bbpolymer(this, polymer);
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'Bbpolymer' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		//UPGRADE_NOTE: The access modifier for this class or class field has been changed in order to prevent compilation errors due to the visibility level. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1296'"
		internal class Bbpolymer:Mps.Mpspolymer
		{
			private void  InitBlock(Backbone enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Backbone enclosingInstance;
			public Backbone Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			internal Bbpolymer(Backbone enclosingInstance, Polymer polymer):base(polymer, 1, 1500, 500, 2000)
			{
				InitBlock(enclosingInstance);
			}
			
			internal override void  setMad(short mad, System.Collections.BitArray bsSelected)
			{
				bool bondSelectionModeOr = Enclosing_Instance.viewer.BondSelectionModeOr;
				int[] atomIndices = polymer.LeadAtomIndices;
				// note that i is initialized to monomerCount - 1
				// in order to skip the last atom
				// but it is picked up within the loop by looking at i+1
				for (int i = monomerCount - 1; --i >= 0; )
				{
					if ((bsSelected.Get(atomIndices[i]) && bsSelected.Get(atomIndices[i + 1])) || (bondSelectionModeOr && (bsSelected.Get(atomIndices[i]) || bsSelected.Get(atomIndices[i + 1]))))
						mads[i] = mad;
				}
			}
		}
	}
}