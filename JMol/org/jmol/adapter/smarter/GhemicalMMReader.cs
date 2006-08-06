/* $RCSfile$
* $Author: egonw $
* $Date: 2005-11-10 16:52:44 +0100 (jeu., 10 nov. 2005) $
* $Revision: 4255 $
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
namespace org.jmol.adapter.smarter
{
	
	/// <summary> Reads Ghemical (<a href="http://www.uku.fi/~thassine/ghemical/">
	/// http://www.uku.fi/~thassine/ghemical</a>)
	/// molecular mechanics (*.mm1gp) files.
	/// <code>
	/// !Header mm1gp 100
	/// !Info 1
	/// !Atoms 6
	/// 0 6 
	/// 1 6 
	/// 2 1 
	/// 3 1 
	/// 4 1 
	/// 5 1 
	/// !Bonds 5
	/// 1 0 D 
	/// 2 0 S 
	/// 3 0 S 
	/// 4 1 S 
	/// 5 1 S 
	/// !Coord
	/// 0 0.06677 -0.00197151 4.968e-07 
	/// 1 -0.0667699 0.00197154 -5.19252e-07 
	/// 2 0.118917 -0.097636 2.03406e-06 
	/// 3 0.124471 0.0904495 -4.84021e-07 
	/// 4 -0.118917 0.0976359 -2.04017e-06 
	/// 5 -0.124471 -0.0904493 5.12591e-07 
	/// !Charges
	/// 0 -0.2
	/// 1 -0.2
	/// 2 0.1
	/// 3 0.1
	/// 4 0.1
	/// 5 0.1
	/// !End
	/// </code>
	/// 
	/// </summary>
	/// <author>  Egon Willighagen <egonw@sci.kun.nl>
	/// </author>
	class GhemicalMMReader:AtomSetCollectionReader
	{
		
		internal override AtomSetCollection readAtomSetCollection(System.IO.StreamReader input)
		{
			
			atomSetCollection = new AtomSetCollection("ghemicalMM");
			
			System.String line;
			while ((line = input.ReadLine()) != null)
			{
				if (line.StartsWith("!Header"))
					processHeader(line);
				else if (line.StartsWith("!Info"))
					processInfo(line);
				else if (line.StartsWith("!Atoms"))
					processAtoms(input, line);
				else if (line.StartsWith("!Bonds"))
					processBonds(input, line);
				else if (line.StartsWith("!Coord"))
					processCoord(input, line);
				else if (line.StartsWith("!Charges"))
					processCharges(input, line);
				else if (line.StartsWith("!End"))
				{
					return atomSetCollection;
				}
			}
			atomSetCollection.errorMessage = "unexpected end of file";
			return atomSetCollection;
		}
		
		internal virtual void  processHeader(System.String line)
		{
		}
		
		internal virtual void  processInfo(System.String line)
		{
		}
		
		internal virtual void  processAtoms(System.IO.StreamReader input, System.String line)
		{
			int atomCount = parseInt(line, 6);
			//System.out.println("atomCount=" + atomCount);
			for (int i = 0; i < atomCount; ++i)
			{
				if (atomSetCollection.atomCount != i)
					throw new System.Exception("GhemicalMMReader error #1");
				line = input.ReadLine();
				int atomIndex = parseInt(line);
				if (atomIndex != i)
					throw new System.Exception("bad atom index in !Atoms" + "expected: " + i + " saw:" + atomIndex);
				int elementNumber = parseInt(line, ichNextParse);
				Atom atom = atomSetCollection.addNewAtom();
				atom.elementNumber = (sbyte) elementNumber;
			}
		}
		
		internal virtual void  processBonds(System.IO.StreamReader input, System.String line)
		{
			int bondCount = parseInt(line, 6);
			for (int i = 0; i < bondCount; ++i)
			{
				line = input.ReadLine();
				int atomIndex1 = parseInt(line);
				int atomIndex2 = parseInt(line, ichNextParse);
				System.String orderCode = parseToken(line, ichNextParse);
				int order = 0;
				switch (orderCode[0])
				{
					
					case 'C':  // Conjugated (aromatic)
						++order; // our code for aromatic is 4;
						goto case 'T';
					
					case 'T': 
						++order;
						goto case 'D';
					
					case 'D': 
						++order;
						goto case 'S';
					
					case 'S': 
					default: 
						++order;
						break;
					}
				atomSetCollection.addNewBond(atomIndex1, atomIndex2, order);
			}
		}
		
		internal virtual void  processCoord(System.IO.StreamReader input, System.String line)
		{
			for (int i = 0; i < atomSetCollection.atomCount; ++i)
			{
				line = input.ReadLine();
				int atomIndex = parseInt(line);
				if (atomIndex != i)
					throw new System.Exception("bad atom index in !Coord" + "expected: " + i + " saw:" + atomIndex);
				Atom atom = atomSetCollection.atoms[i];
				atom.x = parseFloat(line, ichNextParse) * 10;
				atom.y = parseFloat(line, ichNextParse) * 10;
				atom.z = parseFloat(line, ichNextParse) * 10;
			}
		}
		
		internal virtual void  processCharges(System.IO.StreamReader input, System.String line)
		{
			for (int i = 0; i < atomSetCollection.atomCount; ++i)
			{
				line = input.ReadLine();
				int atomIndex = parseInt(line);
				if (atomIndex != i)
					throw new System.Exception("bad atom index in !Charges" + "expected: " + i + " saw:" + atomIndex);
				Atom atom = atomSetCollection.atoms[i];
				atom.partialCharge = parseFloat(line, ichNextParse);
			}
		}
	}
}