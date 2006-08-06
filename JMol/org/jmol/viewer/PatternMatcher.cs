/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-03-27 17:27:39 +0200 (lun., 27 mars 2006) $
* $Revision: 4773 $
*
* Copyright (C) 2005  The Jmol Development Team
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
using InvalidSmilesException = org.jmol.smiles.InvalidSmilesException;
using SmilesAtom = org.jmol.smiles.SmilesAtom;
using SmilesBond = org.jmol.smiles.SmilesBond;
using SmilesMolecule = org.jmol.smiles.SmilesMolecule;
using SmilesParser = org.jmol.smiles.SmilesParser;
namespace org.jmol.viewer
{
	
	/// <summary> A class to match a SMILES pattern with a Jmol molecule.
	/// <p>
	/// The SMILES specification can been found at the
	/// <a href="http://www.daylight.com/smiles/">SMILES Home Page</a>.
	/// <p>
	/// An example on how to use it:
	/// <pre><code>
	/// PatternMatcher matcher = new PatternMatcher(jmolViewer);
	/// try {
	/// BitSet bitSet = matcher.getSubstructureSet(smilesString);
	/// // Use bitSet...
	/// } catch (InvalidSmilesException e) {
	/// // Exception management
	/// }
	/// </code></pre>
	/// 
	/// </summary>
	/// <author>  Nicolas Vervelle
	/// </author>
	/// <seealso cref="org.jmol.smiles.SmilesMolecule">
	/// </seealso>
	class PatternMatcher
	{
		
		private int atomCount = 0;
		private Frame frame = null;
		
		/// <summary> Constructs a <code>PatternMatcher</code>.
		/// 
		/// </summary>
		/// <param name="viewer">Jmol viewer.
		/// </param>
		public PatternMatcher(Viewer viewer)
		{
			this.frame = viewer.Frame;
			this.atomCount = viewer.AtomCount;
		}
		
		/// <summary> Returns a vector of bits indicating which atoms match the pattern.
		/// 
		/// </summary>
		/// <param name="smiles">SMILES pattern.
		/// </param>
		/// <returns> BitSet Array indicating which atoms match the pattern.
		/// </returns>
		/// <throws>  InvalidSmilesException Raised if <code>smiles</code> is not a valid SMILES pattern. </throws>
		public virtual System.Collections.BitArray getSubstructureSet(System.String smiles)
		{
			SmilesParser parser = new SmilesParser();
			SmilesMolecule pattern = parser.parseSmiles(smiles);
			return getSubstructureSet(pattern);
		}
		
		/// <summary> Returns a vector of bits indicating which atoms match the pattern.
		/// 
		/// </summary>
		/// <param name="pattern">SMILES pattern.
		/// </param>
		/// <returns> BitSet Array indicating which atoms match the pattern.
		/// </returns>
		public virtual System.Collections.BitArray getSubstructureSet(SmilesMolecule pattern)
		{
			System.Collections.BitArray bsSubstructure = new System.Collections.BitArray(64);
			searchMatch(bsSubstructure, pattern, 0);
			return bsSubstructure;
		}
		
		/// <summary> Recursively search matches.
		/// 
		/// </summary>
		/// <param name="bs">Resulting BitSet (each atom in a structure is set to 1).
		/// </param>
		/// <param name="pattern">SMILES pattern.
		/// </param>
		/// <param name="atomNum">Current atom of the pattern.
		/// </param>
		private void  searchMatch(System.Collections.BitArray bs, SmilesMolecule pattern, int atomNum)
		{
			//System.out.println("Begin match:" + atomNum);
			SmilesAtom patternAtom = pattern.getAtom(atomNum);
			for (int i = 0; i < patternAtom.BondsCount; i++)
			{
				SmilesBond patternBond = patternAtom.getBond(i);
				if (patternBond.Atom2 == patternAtom)
				{
					int matchingAtom = patternBond.Atom1.MatchingAtom;
					Atom atom = frame.getAtomAt(matchingAtom);
					Bond[] bonds = atom.Bonds;
					if (bonds != null)
					{
						for (int j = 0; j < bonds.Length; j++)
						{
							if (bonds[j].Atom1.atomIndex == matchingAtom)
							{
								searchMatch(bs, pattern, patternAtom, atomNum, bonds[j].Atom2.atomIndex);
							}
							if (bonds[j].Atom2.atomIndex == matchingAtom)
							{
								searchMatch(bs, pattern, patternAtom, atomNum, bonds[j].Atom1.atomIndex);
							}
						}
					}
					return ;
				}
			}
			for (int i = 0; i < atomCount; i++)
			{
				searchMatch(bs, pattern, patternAtom, atomNum, i);
			}
			//System.out.println("End match:" + atomNum);
		}
		
		/// <summary> Recursively search matches.
		/// 
		/// </summary>
		/// <param name="bs">Resulting BitSet (each atom in a structure is set to 1).
		/// </param>
		/// <param name="pattern">SMILES pattern.
		/// </param>
		/// <param name="patternAtom">Atom of the pattern that is currently tested.
		/// </param>
		/// <param name="atomNum">Current atom of the pattern.
		/// </param>
		/// <param name="i">Atom number of the atom that is currently tested to match <code>patternAtom</code>.
		/// </param>
		private void  searchMatch(System.Collections.BitArray bs, SmilesMolecule pattern, SmilesAtom patternAtom, int atomNum, int i)
		{
			// Check that an atom is not used twice
			for (int j = 0; j < atomNum; j++)
			{
				SmilesAtom previousAtom = pattern.getAtom(j);
				if (previousAtom.MatchingAtom == i)
				{
					return ;
				}
			}
			
			bool canMatch = true;
			Atom atom = frame.getAtomAt(i);
			
			// Check symbol
			if (((System.Object) patternAtom.Symbol != (System.Object) "*") && ((System.Object) patternAtom.Symbol != (System.Object) atom.ElementSymbol))
			{
				canMatch = false;
			}
			// Check atomic mass : NO because Jmol doesn't know about atomic mass
			// Check charge
			if (patternAtom.Charge != atom.FormalCharge)
			{
				canMatch = false;
			}
			
			// Check bonds
			for (int j = 0; j < patternAtom.BondsCount; j++)
			{
				SmilesBond patternBond = patternAtom.getBond(j);
				// Check only if the current atom is the second atom of the bond
				if (patternBond.Atom2 == patternAtom)
				{
					int matchingAtom = patternBond.Atom1.MatchingAtom;
					Bond[] bonds = atom.Bonds;
					bool bondFound = false;
					for (int k = 0; k < bonds.Length; k++)
					{
						if ((bonds[k].Atom1.atomIndex == matchingAtom) || (bonds[k].Atom2.atomIndex == matchingAtom))
						{
							switch (patternBond.BondType)
							{
								
								case SmilesBond.TYPE_AROMATIC: 
									if ((bonds[k].Order & JmolConstants.BOND_AROMATIC_MASK) != 0)
									{
										bondFound = true;
									}
									break;
								
								case SmilesBond.TYPE_DOUBLE: 
									if ((bonds[k].Order & JmolConstants.BOND_COVALENT_DOUBLE) != 0)
									{
										bondFound = true;
									}
									break;
								
								case SmilesBond.TYPE_SINGLE: 
								case SmilesBond.TYPE_DIRECTIONAL_1: 
								case SmilesBond.TYPE_DIRECTIONAL_2: 
									if ((bonds[k].Order & JmolConstants.BOND_COVALENT_SINGLE) != 0)
									{
										bondFound = true;
									}
									break;
								
								case SmilesBond.TYPE_TRIPLE: 
									if ((bonds[k].Order & JmolConstants.BOND_COVALENT_TRIPLE) != 0)
									{
										bondFound = true;
									}
									break;
								
								case SmilesBond.TYPE_UNKOWN: 
									bondFound = true;
									break;
								}
						}
					}
					if (!bondFound)
					{
						canMatch = false;
					}
				}
			}
			
			// Finish matching
			if (canMatch)
			{
				patternAtom.MatchingAtom = i;
				if (atomNum + 1 < pattern.AtomsCount)
				{
					searchMatch(bs, pattern, atomNum + 1);
				}
				else
				{
					for (int k = 0; k < pattern.AtomsCount; k++)
					{
						SmilesAtom matching = pattern.getAtom(k);
						SupportClass.BitArraySupport.Set(bs, matching.MatchingAtom);
					}
				}
				patternAtom.MatchingAtom = - 1;
			}
		}
	}
}