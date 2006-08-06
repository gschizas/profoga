/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-03-27 17:13:59 +0200 (lun., 27 mars 2006) $
* $Revision: 4771 $
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
namespace org.jmol.smiles
{
	
	/// <summary> Parses a SMILES String to create a <code>SmilesMolecule</code>.
	/// The SMILES specification has been found at the
	/// <a href="http://www.daylight.com/smiles/">SMILES Home Page</a>. <br>
	/// 
	/// Currently this parser supports only parts of the SMILES specification. <br>
	/// 
	/// An example on how to use it:
	/// <pre><code>
	/// try {
	/// SmilesParser sp = new SmilesParser();
	/// SmilesMolecule sm = sp.parseSmiles("CC(C)C(=O)O");
	/// // Use the resulting molecule 
	/// } catch (InvalidSmilesException e) {
	/// // Exception management
	/// }
	/// </code></pre>
	/// 
	/// </summary>
	/// <seealso cref="<a href="http://www.daylight.com/smiles/">SMILES Home Page</a>">
	/// </seealso>
	public class SmilesParser
	{
		
		private SmilesBond[] ringBonds;
		
		/// <summary> Constructs a <code>SmilesParser</code>.</summary>
		public SmilesParser()
		{
			ringBonds = null;
		}
		
		/// <summary> Parses a SMILES String
		/// 
		/// </summary>
		/// <param name="smiles">SMILES String
		/// </param>
		/// <returns> Molecule corresponding to <code>smiles</code>
		/// </returns>
		/// <throws>  InvalidSmilesException </throws>
		public virtual SmilesMolecule parseSmiles(System.String smiles)
		{
			if (smiles == null)
			{
				throw new InvalidSmilesException("SMILES expressions must not be null");
			}
			
			// First pass
			SmilesMolecule molecule = new SmilesMolecule();
			parseSmiles(molecule, smiles, null);
			
			// Implicit hydrogren creation
			for (int i = 0; i < molecule.AtomsCount; i++)
			{
				SmilesAtom atom = molecule.getAtom(i);
				atom.createMissingHydrogen(molecule);
			}
			
			// Check for rings
			if (ringBonds != null)
			{
				for (int i = 0; i < ringBonds.Length; i++)
				{
					if (ringBonds[i] != null)
					{
						throw new InvalidSmilesException("Open ring");
					}
				}
			}
			
			return molecule;
		}
		
		/// <summary> Parses a part of a SMILES String
		/// 
		/// </summary>
		/// <param name="molecule">Resulting molecule 
		/// </param>
		/// <param name="smiles">SMILES String
		/// </param>
		/// <param name="currentAtom">Current atom
		/// </param>
		/// <throws>  InvalidSmilesException </throws>
		private void  parseSmiles(SmilesMolecule molecule, System.String smiles, SmilesAtom currentAtom)
		{
			if ((smiles == null) || (smiles.Length == 0))
			{
				return ;
			}
			
			// Branching
			int index = 0;
			char firstChar = smiles[index];
			if (firstChar == '(')
			{
				index++;
				int currentIndex = index;
				int parenthesisCount = 1;
				while ((currentIndex < smiles.Length) && (parenthesisCount > 0))
				{
					switch (smiles[currentIndex])
					{
						
						case '(': 
							parenthesisCount++;
							break;
						
						case ')': 
							parenthesisCount--;
							break;
						}
					currentIndex++;
				}
				if (parenthesisCount != 0)
				{
					throw new InvalidSmilesException("Unbalanced parenthesis");
				}
				System.String subSmiles = smiles.Substring(index, (currentIndex - 1) - (index));
				parseSmiles(molecule, subSmiles, currentAtom);
				index = currentIndex;
				if (index >= smiles.Length)
				{
					throw new InvalidSmilesException("Pattern must not end with ')'");
				}
			}
			
			// Bonds
			firstChar = smiles[index];
			int bondType = SmilesBond.getBondTypeFromCode(firstChar);
			if (bondType != SmilesBond.TYPE_UNKOWN)
			{
				if (currentAtom == null)
				{
					throw new InvalidSmilesException("Bond without a previous atom");
				}
				index++;
			}
			
			// Atom
			firstChar = smiles[index];
			if ((firstChar >= '0') && (firstChar <= '9'))
			{
				// Ring
				System.String subSmiles = smiles.Substring(index, (index + 1) - (index));
				parseRing(molecule, subSmiles, currentAtom, bondType);
				index++;
			}
			else if (firstChar == '%')
			{
				// Ring
				index++;
				if ((smiles[index] < 0) || (smiles[index] > 9))
				{
					throw new InvalidSmilesException("Ring number must follow the % sign");
				}
				int currentIndex = index;
				while ((currentIndex < smiles.Length) && (smiles[currentIndex] >= '0') && (smiles[currentIndex] <= '9'))
				{
					currentIndex++;
				}
				System.String subSmiles = smiles.Substring(index, (currentIndex) - (index));
				parseRing(molecule, subSmiles, currentAtom, bondType);
				index = currentIndex;
			}
			else if (firstChar == '[')
			{
				// Atom definition
				index++;
				int currentIndex = index;
				while ((currentIndex < smiles.Length) && (smiles[currentIndex] != ']'))
				{
					currentIndex++;
				}
				if (currentIndex >= smiles.Length)
				{
					throw new InvalidSmilesException("Unmatched [");
				}
				System.String subSmiles = smiles.Substring(index, (currentIndex) - (index));
				currentAtom = parseAtom(molecule, subSmiles, currentAtom, bondType, true);
				index = currentIndex + 1;
			}
			else if (((firstChar >= 'a') && (firstChar <= 'z')) || ((firstChar >= 'A') && (firstChar <= 'Z')) || (firstChar == '*'))
			{
				// Atom definition
				int size = 1;
				if (index + 1 < smiles.Length)
				{
					char secondChar = smiles[index + 1];
					if ((firstChar >= 'A') && (firstChar <= 'Z') && (secondChar >= 'a') && (secondChar <= 'z'))
					{
						size = 2;
					}
				}
				System.String subSmiles = smiles.Substring(index, (index + size) - (index));
				currentAtom = parseAtom(molecule, subSmiles, currentAtom, bondType, false);
				index += size;
			}
			
			// Next part of the SMILES String
			if (index == 0)
			{
				throw new InvalidSmilesException("Unexpected character: " + smiles[0]);
			}
			if (index < smiles.Length)
			{
				System.String subSmiles = smiles.Substring(index);
				parseSmiles(molecule, subSmiles, currentAtom);
			}
		}
		
		/// <summary> Parses an atom definition
		/// 
		/// </summary>
		/// <param name="molecule">Resulting molecule 
		/// </param>
		/// <param name="smiles">SMILES String
		/// </param>
		/// <param name="currentAtom">Current atom
		/// </param>
		/// <param name="bondType">Bond type
		/// </param>
		/// <param name="complete">Indicates if is a complete definition (between [])
		/// </param>
		/// <returns> New atom
		/// </returns>
		/// <throws>  InvalidSmilesException </throws>
		private SmilesAtom parseAtom(SmilesMolecule molecule, System.String smiles, SmilesAtom currentAtom, int bondType, bool complete)
		{
			if ((smiles == null) || (smiles.Length == 0))
			{
				throw new InvalidSmilesException("Empty atom definition");
			}
			
			// Atomic mass
			int index = 0;
			char firstChar = smiles[index];
			int atomicMass = System.Int32.MinValue;
			if ((firstChar >= '0') && (firstChar <= '9'))
			{
				int currentIndex = index;
				while ((currentIndex < smiles.Length) && (smiles[currentIndex] >= '0') && (smiles[currentIndex] <= '9'))
				{
					currentIndex++;
				}
				System.String sub = smiles.Substring(index, (currentIndex) - (index));
				try
				{
					atomicMass = System.Int32.Parse(sub);
				}
				catch (System.FormatException e)
				{
					throw new InvalidSmilesException("Non numeric atomic mass");
				}
				index = currentIndex;
			}
			
			// Symbol
			if (index >= smiles.Length)
			{
				throw new InvalidSmilesException("Missing atom symbol");
			}
			firstChar = smiles[index];
			if (((firstChar < 'a') || (firstChar > 'z')) && ((firstChar < 'A') || (firstChar > 'Z')) && (firstChar != '*'))
			{
				throw new InvalidSmilesException("Unexpected atom symbol");
			}
			int size = 1;
			if (index + 1 < smiles.Length)
			{
				char secondChar = smiles[index + 1];
				if ((firstChar >= 'A') && (firstChar <= 'Z') && (secondChar >= 'a') && (secondChar <= 'z'))
				{
					size = 2;
				}
			}
			System.String atomSymbol = smiles.Substring(index, (index + size) - (index));
			index += size;
			
			// Chirality
			System.String chiralClass = null;
			int chiralOrder = System.Int32.MinValue;
			if (index < smiles.Length)
			{
				firstChar = smiles[index];
				if (firstChar == '@')
				{
					index++;
					if (index < smiles.Length)
					{
						firstChar = smiles[index];
						if (firstChar == '@')
						{
							index++;
							chiralClass = SmilesAtom.DEFAULT_CHIRALITY;
							chiralOrder = 2;
						}
						else if ((firstChar >= 'A') && (firstChar <= 'Z') && (firstChar != 'H'))
						{
							if (index + 1 < smiles.Length)
							{
								char secondChar = smiles[index];
								if ((secondChar >= 'A') && (secondChar <= 'Z'))
								{
									chiralClass = smiles.Substring(index, (index + 2) - (index));
									index += 2;
									int currentIndex = index;
									while ((currentIndex < smiles.Length) && (smiles[currentIndex] >= '0') && (smiles[currentIndex] <= '9'))
									{
										currentIndex++;
									}
									if (currentIndex > index)
									{
										System.String sub = smiles.Substring(index, (currentIndex) - (index));
										try
										{
											chiralOrder = System.Int32.Parse(sub);
										}
										catch (System.FormatException e)
										{
											throw new InvalidSmilesException("Non numeric chiral order");
										}
									}
									else
									{
										chiralOrder = 1;
									}
									index = currentIndex;
								}
							}
						}
						else
						{
							chiralClass = SmilesAtom.DEFAULT_CHIRALITY;
							chiralOrder = 1;
						}
					}
					else
					{
						chiralClass = SmilesAtom.DEFAULT_CHIRALITY;
						chiralOrder = 1;
					}
				}
			}
			
			// Hydrogen count
			int hydrogenCount = System.Int32.MinValue;
			if (index < smiles.Length)
			{
				firstChar = smiles[index];
				if (firstChar == 'H')
				{
					index++;
					int currentIndex = index;
					while ((currentIndex < smiles.Length) && (smiles[currentIndex] >= '0') && (smiles[currentIndex] <= '9'))
					{
						currentIndex++;
					}
					if (currentIndex > index)
					{
						System.String sub = smiles.Substring(index, (currentIndex) - (index));
						try
						{
							hydrogenCount = System.Int32.Parse(sub);
						}
						catch (System.FormatException e)
						{
							throw new InvalidSmilesException("Non numeric hydrogen count");
						}
					}
					else
					{
						hydrogenCount = 1;
					}
					index = currentIndex;
				}
			}
			if ((hydrogenCount == System.Int32.MinValue) && (complete))
			{
				hydrogenCount = 0;
			}
			
			// Charge
			int charge = 0;
			if (index < smiles.Length)
			{
				firstChar = smiles[index];
				if ((firstChar == '+') || (firstChar == '-'))
				{
					int count = 1;
					index++;
					if (index < smiles.Length)
					{
						char nextChar = smiles[index];
						if ((nextChar >= '0') && (nextChar <= '9'))
						{
							int currentIndex = index;
							while ((currentIndex < smiles.Length) && (smiles[currentIndex] >= '0') && (smiles[currentIndex] <= '9'))
							{
								currentIndex++;
							}
							System.String sub = smiles.Substring(index, (currentIndex) - (index));
							try
							{
								count = System.Int32.Parse(sub);
							}
							catch (System.FormatException e)
							{
								throw new InvalidSmilesException("Non numeric charge");
							}
							index = currentIndex;
						}
						else
						{
							int currentIndex = index;
							while ((currentIndex < smiles.Length) && (smiles[currentIndex] == firstChar))
							{
								currentIndex++;
								count++;
							}
							index = currentIndex;
						}
					}
					if (firstChar == '+')
					{
						charge = count;
					}
					else
					{
						charge = - count;
					}
				}
			}
			
			// Final check
			if (index < smiles.Length)
			{
				throw new InvalidSmilesException("Unexpected characters after atom definition: " + smiles.Substring(index));
			}
			
			// Create atom
			if (bondType == SmilesBond.TYPE_UNKOWN)
			{
				bondType = SmilesBond.TYPE_SINGLE;
			}
			SmilesAtom newAtom = molecule.createAtom();
			if ((currentAtom != null) && (bondType != SmilesBond.TYPE_NONE))
			{
				molecule.createBond(currentAtom, newAtom, bondType);
			}
			newAtom.Symbol = atomSymbol;
			newAtom.AtomicMass = atomicMass;
			newAtom.Charge = charge;
			newAtom.ChiralClass = chiralClass;
			newAtom.ChiralOrder = chiralOrder;
			newAtom.HydrogenCount = hydrogenCount;
			return newAtom;
		}
		
		/// <summary> Parses a ring definition
		/// 
		/// </summary>
		/// <param name="molecule">Resulting molecule 
		/// </param>
		/// <param name="smiles">SMILES String
		/// </param>
		/// <param name="currentAtom">Current atom
		/// </param>
		/// <param name="bondType">Bond type
		/// </param>
		/// <throws>  InvalidSmilesException </throws>
		private void  parseRing(SmilesMolecule molecule, System.String smiles, SmilesAtom currentAtom, int bondType)
		{
			// Extracting ring number
			int ringNum = 0;
			try
			{
				ringNum = System.Int32.Parse(smiles);
			}
			catch (System.FormatException e)
			{
				throw new InvalidSmilesException("Non numeric ring identifier");
			}
			
			// Checking rings buffer is big enough
			if (ringBonds == null)
			{
				ringBonds = new SmilesBond[10];
				for (int i = 0; i < ringBonds.Length; i++)
				{
					ringBonds[i] = null;
				}
			}
			if (ringNum >= ringBonds.Length)
			{
				SmilesBond[] tmp = new SmilesBond[ringNum + 1];
				for (int i = 0; i < ringBonds.Length; i++)
				{
					tmp[i] = ringBonds[i];
				}
				for (int i = ringBonds.Length; i < tmp.Length; i++)
				{
					tmp[i] = null;
				}
			}
			
			// Ring management
			if (ringBonds[ringNum] == null)
			{
				ringBonds[ringNum] = molecule.createBond(currentAtom, null, bondType);
			}
			else
			{
				if (bondType == SmilesBond.TYPE_UNKOWN)
				{
					bondType = ringBonds[ringNum].BondType;
					if (bondType == SmilesBond.TYPE_UNKOWN)
					{
						bondType = SmilesBond.TYPE_SINGLE;
					}
				}
				else
				{
					if ((ringBonds[ringNum].BondType != SmilesBond.TYPE_UNKOWN) && (ringBonds[ringNum].BondType != bondType))
					{
						throw new InvalidSmilesException("Incoherent bond type for ring");
					}
				}
				ringBonds[ringNum].BondType = bondType;
				ringBonds[ringNum].Atom2 = currentAtom;
				currentAtom.addBond(ringBonds[ringNum]);
				ringBonds[ringNum] = null;
			}
		}
	}
}