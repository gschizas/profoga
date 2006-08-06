/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-03-27 18:30:39 +0200 (lun., 27 mars 2006) $
* $Revision: 4782 $
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
namespace org.jmol.viewer
{
	
	class Group
	{
		virtual internal System.String Group1
		{
			get
			{
				if (groupID >= JmolConstants.predefinedGroup1Names.Length)
					return "?";
				return JmolConstants.predefinedGroup1Names[groupID];
			}
			
		}
		virtual internal char ChainID
		{
			get
			{
				return chain.chainID;
			}
			
		}
		virtual internal int PolymerLength
		{
			get
			{
				return 0;
			}
			
		}
		virtual internal int PolymerIndex
		{
			get
			{
				return - 1;
			}
			
		}
		virtual internal sbyte ProteinStructureType
		{
			get
			{
				return JmolConstants.PROTEIN_STRUCTURE_NONE;
			}
			
		}
		virtual internal bool Protein
		{
			get
			{
				return false;
			}
			
		}
		virtual internal bool Nucleic
		{
			get
			{
				return false;
			}
			
		}
		virtual internal bool Dna
		{
			get
			{
				return false;
			}
			
		}
		virtual internal bool Rna
		{
			get
			{
				return false;
			}
			
		}
		virtual internal bool Purine
		{
			get
			{
				return false;
			}
			
		}
		virtual internal bool Pyrimidine
		{
			get
			{
				return false;
			}
			
		}
		virtual internal int Resno
		{
			////////////////////////////////////////////////////////////////
			// seqcode stuff
			////////////////////////////////////////////////////////////////
			
			
			get
			{
				return seqcode >> 8;
			}
			
		}
		virtual internal bool Hetero
		{
			get
			{
				// just look at the first atom of the group
				return chain.frame.atoms[firstAtomIndex].Hetero;
			}
			
		}
		
		internal Chain chain;
		internal int seqcode;
		internal short groupID;
		internal int firstAtomIndex = - 1;
		internal int lastAtomIndex;
		
		
		internal Group(Chain chain, System.String group3, int seqcode, int firstAtomIndex, int lastAtomIndex)
		{
			this.chain = chain;
			this.seqcode = seqcode;
			
			if (group3 == null)
				group3 = "";
			this.groupID = getGroupID(group3);
			this.firstAtomIndex = firstAtomIndex;
			this.lastAtomIndex = lastAtomIndex;
		}
		
		internal bool isGroup3(System.String group3)
		{
			return group3Names[groupID].ToUpper().Equals(group3.ToUpper());
		}
		
		internal System.String getGroup3()
		{
			return group3Names[groupID];
		}
		
		internal static System.String getGroup3(short groupID)
		{
			return group3Names[groupID];
		}
		
		internal short getGroupID()
		{
			return groupID;
		}
		
		internal bool isGroup3Match(System.String strWildcard)
		{
			int cchWildcard = strWildcard.Length;
			int ichWildcard = 0;
			System.String group3 = group3Names[groupID];
			int cchGroup3 = group3.Length;
			if (cchWildcard < cchGroup3)
				return false;
			while (cchWildcard > cchGroup3)
			{
				// wildcard is too long
				// so strip '?' from the beginning and the end, if possible
				if (strWildcard[ichWildcard] == '?')
				{
					++ichWildcard;
				}
				else if (strWildcard[ichWildcard + cchWildcard - 1] != '?')
				{
					return false;
				}
				--cchWildcard;
			}
			for (int i = cchGroup3; --i >= 0; )
			{
				char charWild = strWildcard[ichWildcard + i];
				if (charWild == '?')
					continue;
				if (charWild != group3[i])
					return false;
			}
			return true;
		}
		
		////////////////////////////////////////////////////////////////
		// static stuff for group ids
		////////////////////////////////////////////////////////////////
		
		private static System.Collections.Hashtable htGroup = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
		
		internal static System.String[] group3Names = new System.String[128];
		internal static short group3NameCount = 0;
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'addGroup3Name'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		internal static short addGroup3Name(System.String group3)
		{
			lock (typeof(org.jmol.viewer.Group))
			{
				if (group3NameCount == group3Names.Length)
					group3Names = Util.doubleLength(group3Names);
				short groupID = group3NameCount++;
				group3Names[groupID] = group3;
				htGroup[group3] = (short) groupID;
				return groupID;
			}
		}
		
		internal static short getGroupID(System.String group3)
		{
			if (group3 == null)
				return - 1;
			short groupID = lookupGroupID(group3);
			return (groupID != - 1)?groupID:addGroup3Name(group3);
		}
		
		internal static short lookupGroupID(System.String group3)
		{
			if (group3 != null)
			{
				System.Int16 boxedGroupID = (System.Int16) htGroup[group3];
				//UPGRADE_TODO: The 'System.Int16' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
				if (boxedGroupID != null)
					return (short) boxedGroupID;
			}
			return - 1;
		}
		
		internal int getSeqcode()
		{
			return seqcode;
		}
		
		internal System.String getSeqcodeString()
		{
			return getSeqcodeString(seqcode);
		}
		
		internal static int getSeqcode(int sequenceNumber, char insertionCode)
		{
			if (sequenceNumber == System.Int32.MinValue)
				return sequenceNumber;
			if (!((insertionCode >= 'A' && insertionCode <= 'Z') || (insertionCode >= 'a' && insertionCode <= 'z') || (insertionCode >= '0' && insertionCode <= '9')))
			{
				if (insertionCode != ' ' && insertionCode != '\x0000')
					System.Console.Out.WriteLine("unrecognized insertionCode:" + insertionCode);
				insertionCode = '\x0000';
			}
			return (sequenceNumber << 8) + insertionCode;
		}
		
		internal static System.String getSeqcodeString(int seqcode)
		{
			if (seqcode == System.Int32.MinValue)
				return null;
			return (seqcode & 0xFF) == 0?"" + (seqcode >> 8):"" + (seqcode >> 8) + '^' + (char) (seqcode & 0xFF);
		}
		
		internal void  selectAtoms(System.Collections.BitArray bs)
		{
			for (int i = firstAtomIndex; i <= lastAtomIndex; ++i)
				SupportClass.BitArraySupport.Set(bs, i);
		}
		
		internal virtual bool isSelected(System.Collections.BitArray bs)
		{
			for (int i = firstAtomIndex; i <= lastAtomIndex; ++i)
				if (bs.Get(i))
					return true;
			return false;
		}
		
		public override System.String ToString()
		{
			return "[" + getGroup3() + "-" + getSeqcodeString() + "]";
		}
		static Group()
		{
			{
				for (int i = 0; i < JmolConstants.predefinedGroup3Names.Length; ++i)
				{
					addGroup3Name(JmolConstants.predefinedGroup3Names[i]);
				}
			}
		}
	}
}