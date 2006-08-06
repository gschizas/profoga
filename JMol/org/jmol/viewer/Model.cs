/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-04-02 21:10:27 +0200 (dim., 02 avr. 2006) $
* $Revision: 4881 $
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
	
	sealed class Model
	{
		internal int ChainCount
		{
			get
			{
				return chainCount;
			}
			
		}
		internal int PolymerCount
		{
			get
			{
				return polymerCount;
			}
			
		}
		internal int GroupCount
		{
			get
			{
				int groupCount = 0;
				for (int i = chainCount; --i >= 0; )
					groupCount += chains[i].GroupCount;
				return groupCount;
			}
			
		}
		
		internal Mmset mmset;
		internal int modelIndex;
		internal System.String modelTag;
		
		private int chainCount = 0;
		private Chain[] chains = new Chain[8];
		
		private int polymerCount = 0;
		private Polymer[] polymers = new Polymer[8];
		
		
		internal Model(Mmset mmset, int modelIndex, System.String modelTag)
		{
			this.mmset = mmset;
			this.modelIndex = modelIndex;
			this.modelTag = modelTag;
		}
		
		internal void  freeze()
		{
			//    System.out.println("Mmset.freeze() chainCount=" + chainCount);
			chains = (Chain[]) Util.setLength(chains, chainCount);
			for (int i = 0; i < chainCount; ++i)
				chains[i].freeze();
			polymers = (Polymer[]) Util.setLength(polymers, polymerCount);
		}
		
		internal void  addSecondaryStructure(sbyte type, char startChainID, int startSeqcode, char endChainID, int endSeqcode)
		{
			for (int i = polymerCount; --i >= 0; )
			{
				Polymer polymer = polymers[i];
				polymer.addSecondaryStructure(type, startChainID, startSeqcode, endChainID, endSeqcode);
			}
		}
		
		internal void  calculateStructures()
		{
			//    System.out.println("Model.calculateStructures");
			for (int i = polymerCount; --i >= 0; )
				polymers[i].calculateStructures();
		}
		
		internal void  calcSelectedGroupsCount(System.Collections.BitArray bsSelected)
		{
			for (int i = chainCount; --i >= 0; )
				chains[i].calcSelectedGroupsCount(bsSelected);
		}
		
		internal void  calcSelectedMonomersCount(System.Collections.BitArray bsSelected)
		{
			for (int i = polymerCount; --i >= 0; )
				polymers[i].calcSelectedMonomersCount(bsSelected);
		}
		
		internal void  selectSeqcodeRange(int seqcodeA, int seqcodeB, System.Collections.BitArray bs)
		{
			for (int i = chainCount; --i >= 0; )
				chains[i].selectSeqcodeRange(seqcodeA, seqcodeB, bs);
		}
		
		internal Chain getChain(char chainID)
		{
			for (int i = chainCount; --i >= 0; )
			{
				Chain chain = chains[i];
				if (chain.chainID == chainID)
					return chain;
			}
			return null;
		}
		
		internal Chain getOrAllocateChain(char chainID)
		{
			//    System.out.println("chainID=" + chainID + " -> " + (chainID + 0));
			Chain chain = getChain(chainID);
			if (chain != null)
				return chain;
			if (chainCount == chains.Length)
				chains = (Chain[]) Util.doubleLength(chains);
			return chains[chainCount++] = new Chain(mmset.frame, this, chainID);
		}
		
		internal void  addPolymer(Polymer polymer)
		{
			if (polymerCount == polymers.Length)
				polymers = (Polymer[]) Util.doubleLength(polymers);
			polymers[polymerCount++] = polymer;
		}
		
		internal Polymer getPolymer(int polymerIndex)
		{
			return polymers[polymerIndex];
		}
		
		internal void  calcHydrogenBonds(System.Collections.BitArray bsA, System.Collections.BitArray bsB)
		{
			for (int i = polymerCount; --i >= 0; )
				polymers[i].calcHydrogenBonds(bsA, bsB);
		}
	}
}