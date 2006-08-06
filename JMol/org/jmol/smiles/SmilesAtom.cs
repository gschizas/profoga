/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-03-27 17:24:13 +0200 (lun., 27 mars 2006) $
* $Revision: 4772 $
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
	
	/// <summary> This class represents an atom in a <code>SmilesMolecule</code>.</summary>
	public class SmilesAtom
	{
		/// <summary> Returns the atom number of the atom.
		/// 
		/// </summary>
		/// <returns> Atom number.
		/// </returns>
		virtual public int Number
		{
			get
			{
				return number;
			}
			
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Returns the symbol of the atom.
		/// 
		/// </summary>
		/// <returns> Atom symbol.
		/// </returns>
		/// <summary> Sets the symbol of the atm.
		/// 
		/// </summary>
		/// <param name="symbol">Atom symbol.
		/// </param>
		virtual public System.String Symbol
		{
			get
			{
				return symbol;
			}
			
			set
			{
				this.symbol = (value != null)?String.Intern(value):null;
			}
			
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Returns the atomic mass of the atom.
		/// 
		/// </summary>
		/// <returns> Atomic mass.
		/// </returns>
		/// <summary> Sets the atomic mass of the atom.
		/// 
		/// </summary>
		/// <param name="mass">Atomic mass.
		/// </param>
		virtual public int AtomicMass
		{
			get
			{
				return atomicMass;
			}
			
			set
			{
				this.atomicMass = value;
			}
			
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Returns the charge of the atom.
		/// 
		/// </summary>
		/// <returns> Charge.
		/// </returns>
		/// <summary> Sets the charge of the atom.
		/// 
		/// </summary>
		/// <param name="charge">Charge.
		/// </param>
		virtual public int Charge
		{
			get
			{
				return charge;
			}
			
			set
			{
				this.charge = value;
			}
			
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Returns the number of a matching atom in a molecule.
		/// This value is temporary, it is used during the pattern matching algorithm.
		/// 
		/// </summary>
		/// <returns> matching atom.
		/// </returns>
		/// <summary> Sets the number of a matching atom in a molecule.
		/// This value is temporary, it is used during the pattern matching algorithm.
		/// 
		/// </summary>
		/// <param name="atom">Temporary: number of a matching atom in a molecule.
		/// </param>
		virtual public int MatchingAtom
		{
			get
			{
				return matchingAtom;
			}
			
			set
			{
				this.matchingAtom = value;
			}
			
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Returns the chiral class of the atom.
		/// (see <code>CHIRALITY_...</code> constants)
		/// 
		/// </summary>
		/// <returns> Chiral class.
		/// </returns>
		/// <summary> Sets the chiral class of the atom.
		/// (see <code>CHIRALITY_...</code> constants)
		/// 
		/// </summary>
		/// <param name="chiralClass">Chiral class.
		/// </param>
		virtual public System.String ChiralClass
		{
			get
			{
				return chiralClass;
			}
			
			set
			{
				this.chiralClass = (value != null)?String.Intern(value):null;
			}
			
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Returns the chiral order of the atom.
		/// 
		/// </summary>
		/// <returns> Chiral order.
		/// </returns>
		/// <summary> Sets the chiral order of the atom.
		/// 
		/// </summary>
		/// <param name="chiralOrder">Chiral order.
		/// </param>
		virtual public int ChiralOrder
		{
			get
			{
				return chiralOrder;
			}
			
			set
			{
				this.chiralOrder = value;
			}
			
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Returns the number of hydrogen atoms bonded with this atom.
		/// 
		/// </summary>
		/// <returns> Number of hydrogen atoms.
		/// </returns>
		/// <summary> Sets the number of hydrogen atoms bonded with this atom.
		/// 
		/// </summary>
		/// <param name="count">Number of hydrogen atoms.
		/// </param>
		virtual public int HydrogenCount
		{
			get
			{
				return hydrogenCount;
			}
			
			set
			{
				this.hydrogenCount = value;
			}
			
		}
		/// <summary> Returns the number of bonds of this atom.
		/// 
		/// </summary>
		/// <returns> Number of bonds.
		/// </returns>
		virtual public int BondsCount
		{
			get
			{
				return bondsCount;
			}
			
		}
		
		private int number;
		private System.String symbol;
		private int atomicMass;
		private int charge;
		private int hydrogenCount;
		private int matchingAtom;
		private System.String chiralClass;
		private int chiralOrder;
		
		private SmilesBond[] bonds;
		private int bondsCount;
		
		private const int INITIAL_BONDS = 4;
		
		/// <summary> Constant used for default chirality.</summary>
		public const System.String DEFAULT_CHIRALITY = "";
		/// <summary> Constant used for Allene chirality.</summary>
		public const System.String CHIRALITY_ALLENE = "AL";
		/// <summary> Constant used for Octahedral chirality.</summary>
		public const System.String CHIRALITY_OCTAHEDRAL = "OH";
		/// <summary> Constant used for Square Planar chirality.</summary>
		public const System.String CHIRALITY_SQUARE_PLANAR = "SP";
		/// <summary> Constant used for Tetrahedral chirality.</summary>
		public const System.String CHIRALITY_TETRAHEDRAL = "TH";
		/// <summary> Constant used for Trigonal Bipyramidal chirality.</summary>
		public const System.String CHIRALITY_TRIGONAL_BIPYRAMIDAL = "TB";
		
		/// <summary> Constructs a <code>SmilesAtom</code>.
		/// 
		/// </summary>
		/// <param name="number">Atom number in the molecule. 
		/// </param>
		public SmilesAtom(int number)
		{
			this.number = number;
			this.symbol = null;
			this.atomicMass = System.Int32.MinValue;
			this.charge = 0;
			this.hydrogenCount = System.Int32.MinValue;
			this.matchingAtom = - 1;
			this.chiralClass = null;
			this.chiralOrder = System.Int32.MinValue;
			bonds = new SmilesBond[INITIAL_BONDS];
			bondsCount = 0;
		}
		
		/// <summary> Creates missing hydrogen atoms in a <code>SmilesMolecule</code>.
		/// 
		/// </summary>
		/// <param name="molecule">Molecule containing the atom.
		/// </param>
		public virtual void  createMissingHydrogen(SmilesMolecule molecule)
		{
			// Determing max count
			int count = 0;
			if (hydrogenCount == System.Int32.MinValue)
			{
				if (symbol != null)
				{
					if ((System.Object) symbol == (System.Object) "B")
					{
						count = 3;
					}
					else if ((System.Object) symbol == (System.Object) "Br")
					{
						count = 1;
					}
					else if ((System.Object) symbol == (System.Object) "C")
					{
						count = 4;
					}
					else if ((System.Object) symbol == (System.Object) "Cl")
					{
						count = 1;
					}
					else if ((System.Object) symbol == (System.Object) "F")
					{
						count = 1;
					}
					else if ((System.Object) symbol == (System.Object) "I")
					{
						count = 1;
					}
					else if ((System.Object) symbol == (System.Object) "N")
					{
						count = 3;
					}
					else if ((System.Object) symbol == (System.Object) "O")
					{
						count = 2;
					}
					else if ((System.Object) symbol == (System.Object) "P")
					{
						count = 3;
					}
					else if ((System.Object) symbol == (System.Object) "S")
					{
						count = 2;
					}
				}
				for (int i = 0; i < bondsCount; i++)
				{
					SmilesBond bond = bonds[i];
					switch (bond.BondType)
					{
						
						case SmilesBond.TYPE_SINGLE: 
						case SmilesBond.TYPE_DIRECTIONAL_1: 
						case SmilesBond.TYPE_DIRECTIONAL_2: 
							count -= 1;
							break;
						
						case SmilesBond.TYPE_DOUBLE: 
							count -= 2;
							break;
						
						case SmilesBond.TYPE_TRIPLE: 
							count -= 3;
							break;
						}
				}
			}
			else
			{
				count = hydrogenCount;
			}
			
			// Adding hydrogens
			for (int i = 0; i < count; i++)
			{
				SmilesAtom hydrogen = molecule.createAtom();
				molecule.createBond(this, hydrogen, SmilesBond.TYPE_SINGLE);
				hydrogen.Symbol = "H";
			}
		}
		
		/// <summary> Returns the bond at index <code>number</code>.
		/// 
		/// </summary>
		/// <param name="number">Bond number.
		/// </param>
		/// <returns> Bond.
		/// </returns>
		public virtual SmilesBond getBond(int number)
		{
			if ((number >= 0) && (number < bondsCount))
			{
				return bonds[number];
			}
			return null;
		}
		
		/// <summary> Add a bond to the atom.
		/// 
		/// </summary>
		/// <param name="bond">Bond to add.
		/// </param>
		public virtual void  addBond(SmilesBond bond)
		{
			if (bondsCount >= bonds.Length)
			{
				SmilesBond[] tmp = new SmilesBond[bonds.Length * 2];
				Array.Copy(bonds, 0, tmp, 0, bonds.Length);
				bonds = tmp;
			}
			bonds[bondsCount] = bond;
			bondsCount++;
		}
	}
}