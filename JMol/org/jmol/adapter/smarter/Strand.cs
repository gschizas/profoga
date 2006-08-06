/* $RCSfile$
* $Author: egonw $
* $Date: 2005-11-10 16:52:44 +0100 (Thu, 10 Nov 2005) $
* $Revision: 4255 $
*
* Copyright (C) 2006  Jmol Development, www.jmol.org
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
namespace org.jmol.adapter.smarter
{
	
	class Strand
	{
		
		internal System.String chainID; // aka asym_id in PDB/mmCIF
		internal System.String authorID;
		internal System.Boolean isBlank;
		
		internal Strand()
		{
			chainID = null;
			authorID = null;
			isBlank = true;
		}
		
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		internal Strand(System.String chainID, System.String authorID, ref System.Boolean isBlank)
		{
			this.chainID = chainID;
			this.authorID = authorID;
			this.isBlank = isBlank;
		}
		
		public override System.String ToString()
		{
			//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Boolean.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			return "Strand " + chainID + ", authorID=" + authorID + ", isBlank=" + isBlank.ToString();
		}
	}
}