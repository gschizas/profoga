/* $RCSfile$
* $Author: migueljmol $
* $Date: 2005-11-26 00:12:40 +0100 (sam., 26 nov. 2005) $
* $Revision: 4276 $
*
* Copyright (C) 2003-2005  Miguel, Jmol Development, www.jmol.org
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
//UPGRADE_TODO: The type 'javax.vecmath.Point3f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Point3f = javax.vecmath.Point3f;
namespace org.jmol.viewer
{
	
	class AlphaMonomer:Monomer
	{
		override internal ProteinStructure Structure
		{
			set
			{
				this.proteinStructure = value;
			}
			
		}
		override internal ProteinStructure ProteinStructure
		{
			get
			{
				return proteinStructure;
			}
			
		}
		override internal sbyte ProteinStructureType
		{
			get
			{
				return proteinStructure == null?0:proteinStructure.type;
			}
			
		}
		override internal bool Helix
		{
			get
			{
				return proteinStructure != null && proteinStructure.type == JmolConstants.PROTEIN_STRUCTURE_HELIX;
			}
			
		}
		override internal bool HelixOrSheet
		{
			get
			{
				return proteinStructure != null && proteinStructure.type >= JmolConstants.PROTEIN_STRUCTURE_SHEET;
			}
			
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'alphaOffsets'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly sbyte[] alphaOffsets = new sbyte[]{0};
		
		internal static Monomer validateAndAllocate(Chain chain, System.String group3, int seqcode, int firstIndex, int lastIndex, int[] specialAtomIndexes, Atom[] atoms)
		{
			if (firstIndex != lastIndex || specialAtomIndexes[JmolConstants.ATOMID_ALPHA_CARBON] != firstIndex)
				return null;
			return new AlphaMonomer(chain, group3, seqcode, firstIndex, lastIndex, alphaOffsets);
		}
		
		////////////////////////////////////////////////////////////////
		
		internal AlphaMonomer(Chain chain, System.String group3, int seqcode, int firstAtomIndex, int lastAtomIndex, sbyte[] offsets):base(chain, group3, seqcode, firstAtomIndex, lastAtomIndex, offsets)
		{
		}
		
		internal virtual bool isAlphaMonomer()
		{
			return true;
		}
		
		internal ProteinStructure proteinStructure;
		
		internal override Atom getAtom(sbyte specialAtomID)
		{
			return (specialAtomID == JmolConstants.ATOMID_ALPHA_CARBON?LeadAtom:null);
		}
		
		internal override Point3f getAtomPoint(sbyte specialAtomID)
		{
			return (specialAtomID == JmolConstants.ATOMID_ALPHA_CARBON?LeadAtomPoint:null);
		}
		
		internal override bool isConnectedAfter(Monomer possiblyPreviousMonomer)
		{
			if (possiblyPreviousMonomer == null)
				return true;
			if (!(possiblyPreviousMonomer is AlphaMonomer))
				return false;
			float distance = LeadAtomPoint.distance(possiblyPreviousMonomer.LeadAtomPoint);
			// jan reichert in email to miguel on 10 May 2004 said 4.2 looked good
			return distance <= 4.2f;
		}
	}
}