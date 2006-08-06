/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-03-27 23:34:39 +0200 (lun., 27 mars 2006) $
* $Revision: 4791 $
*
* Copyright (C) 2003-2006  Miguel, Jmol Development, www.jmol.org
*
* Contact: miguel@jmol.org
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
	
	class Strands:Mps
	{
		
		/*==============================================================*
		* M. Carson and C.E. Bugg (1986)
		* Algorithm for Ribbon Models of Proteins. J.Mol.Graphics 4:121-122.
		* http://sgce.cbse.uab.edu/carson/papers/ribbons86/ribbons86.html
		*==============================================================*/
		
		internal int strandCount = 5;
		
		internal override Mps.Mpspolymer allocateMpspolymer(Polymer polymer)
		{
			return new Schain(this, polymer);
		}
		
		internal override void  setProperty(System.String propertyName, System.Object value_Renamed, System.Collections.BitArray bs)
		{
			initialize();
			if ((System.Object) "strandCount" == (System.Object) propertyName)
			{
				if (value_Renamed is System.Int32)
				{
					int count = ((System.Int32) value_Renamed);
					if (count < 0)
						count = 0;
					else if (count > 20)
						count = 20;
					strandCount = count;
					return ;
				}
			}
			base.setProperty(propertyName, value_Renamed, bs);
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'Schain' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		internal class Schain:Mps.Mpspolymer
		{
			private void  InitBlock(Strands enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Strands enclosingInstance;
			public Strands Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			internal Schain(Strands enclosingInstance, Polymer polymer):base(polymer, - 2, 3000, 800, 5000)
			{
				InitBlock(enclosingInstance);
			}
		}
	}
}