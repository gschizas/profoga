/* $RCSfile$
* $Author: egonw $
* $Date: 2005-11-10 16:52:44 +0100 (jeu., 10 nov. 2005) $
* $Revision: 4255 $
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
	
	/// <summary> Molecule created from a SMILES String</summary>
	public class SmilesMolecule
	{
		virtual public int AtomsCount
		{
			get
			{
				return atomsCount;
			}
			
		}
		virtual public int BondsCount
		{
			get
			{
				return bondsCount;
			}
			
		}
		
		private SmilesAtom[] atoms;
		private int atomsCount;
		private SmilesBond[] bonds;
		private int bondsCount;
		
		private const int INITIAL_ATOMS = 16;
		private const int INITIAL_BONDS = 16;
		
		/// <summary> SmilesMolecule constructor  </summary>
		public SmilesMolecule()
		{
			atoms = new SmilesAtom[INITIAL_ATOMS];
			atomsCount = 0;
			bonds = new SmilesBond[INITIAL_BONDS];
			bondsCount = 0;
		}
		
		/* ============================================================= */
		/*                             Atoms                             */
		/* ============================================================= */
		
		public virtual SmilesAtom createAtom()
		{
			if (atomsCount >= atoms.Length)
			{
				SmilesAtom[] tmp = new SmilesAtom[atoms.Length * 2];
				Array.Copy(atoms, 0, tmp, 0, atoms.Length);
				atoms = tmp;
			}
			SmilesAtom atom = new SmilesAtom(atomsCount);
			atoms[atomsCount] = atom;
			atomsCount++;
			return atom;
		}
		
		public virtual SmilesAtom getAtom(int number)
		{
			if ((number >= 0) && (number < atomsCount))
			{
				return atoms[number];
			}
			return null;
		}
		
		/* ============================================================= */
		/*                             Bonds                             */
		/* ============================================================= */
		
		public virtual SmilesBond createBond(SmilesAtom atom1, SmilesAtom atom2, int bondType)
		{
			if (bondsCount >= bonds.Length)
			{
				SmilesBond[] tmp = new SmilesBond[bonds.Length * 2];
				Array.Copy(bonds, 0, tmp, 0, bonds.Length);
				bonds = tmp;
			}
			SmilesBond bond = new SmilesBond(atom1, atom2, bondType);
			bonds[bondsCount] = bond;
			bondsCount++;
			if (atom1 != null)
			{
				atom1.addBond(bond);
			}
			if (atom2 != null)
			{
				atom2.addBond(bond);
			}
			return bond;
		}
		
		public virtual SmilesBond getBond(int number)
		{
			if ((number >= 0) && (number < bondsCount))
			{
				return bonds[number];
			}
			return null;
		}
	}
}