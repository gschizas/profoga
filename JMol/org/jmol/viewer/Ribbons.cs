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
namespace org.jmol.viewer
{
	
	class Ribbons:Mps
	{
		
		internal override Mps.Mpspolymer allocateMpspolymer(Polymer polymer)
		{
			return new Schain(this, polymer);
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'Schain' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		internal class Schain:Mps.Mpspolymer
		{
			private void  InitBlock(Ribbons enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Ribbons enclosingInstance;
			public Ribbons Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			internal Schain(Ribbons enclosingInstance, Polymer polymer):base(polymer, - 2, 3000, 800, 5000)
			{
				InitBlock(enclosingInstance);
			}
		}
	}
}