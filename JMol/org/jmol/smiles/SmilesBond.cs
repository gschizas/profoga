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
	
	/// <summary> Bond in a SmilesMolecule</summary>
	public class SmilesBond
	{
		virtual public SmilesAtom Atom1
		{
			get
			{
				return atom1;
			}
			
			set
			{
				this.atom1 = value;
			}
			
		}
		virtual public SmilesAtom Atom2
		{
			get
			{
				return atom2;
			}
			
			set
			{
				this.atom2 = value;
			}
			
		}
		virtual public int BondType
		{
			get
			{
				return bondType;
			}
			
			set
			{
				this.bondType = value;
			}
			
		}
		
		// Bond orders
		public const int TYPE_UNKOWN = - 1;
		public const int TYPE_NONE = 0;
		public const int TYPE_SINGLE = 1;
		public const int TYPE_DOUBLE = 2;
		public const int TYPE_TRIPLE = 3;
		public const int TYPE_AROMATIC = 4;
		public const int TYPE_DIRECTIONAL_1 = 5;
		public const int TYPE_DIRECTIONAL_2 = 6;
		
		// Bond expressions
		public const char CODE_NONE = '.';
		public const char CODE_SINGLE = '-';
		public const char CODE_DOUBLE = '=';
		public const char CODE_TRIPLE = '#';
		public const char CODE_AROMATIC = ':';
		public const char CODE_DIRECTIONAL_1 = '/';
		public const char CODE_DIRECTIONAL_2 = '\\';
		
		private SmilesAtom atom1;
		private SmilesAtom atom2;
		private int bondType;
		
		/// <summary> SmilesBond constructor
		/// 
		/// </summary>
		/// <param name="atom1">First atom
		/// </param>
		/// <param name="atom2">Second atom
		/// </param>
		/// <param name="bondType">Bond type
		/// </param>
		public SmilesBond(SmilesAtom atom1, SmilesAtom atom2, int bondType)
		{
			this.atom1 = atom1;
			this.atom2 = atom2;
			this.bondType = bondType;
		}
		
		/// <param name="code">Bond code
		/// </param>
		/// <returns> Bond type
		/// </returns>
		public static int getBondTypeFromCode(char code)
		{
			switch (code)
			{
				
				case CODE_NONE: 
					return TYPE_NONE;
				
				case CODE_SINGLE: 
					return TYPE_SINGLE;
				
				case CODE_DOUBLE: 
					return TYPE_DOUBLE;
				
				case CODE_TRIPLE: 
					return TYPE_TRIPLE;
				
				case CODE_AROMATIC: 
					return TYPE_AROMATIC;
				
				case CODE_DIRECTIONAL_1: 
					return TYPE_DIRECTIONAL_1;
				
				case CODE_DIRECTIONAL_2: 
					return TYPE_DIRECTIONAL_2;
				}
			return TYPE_UNKOWN;
		}
	}
}