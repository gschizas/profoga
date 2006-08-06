/* $RCSfile$
* $Author: egonw $
* $Date: 2005-11-10 16:52:44 +0100 (jeu., 10 nov. 2005) $
* $Revision: 4255 $
*
* Copyright (C) 2004-2005  The Jmol Development Team
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
using JmolAdapter = org.jmol.api.JmolAdapter;
namespace org.jmol.adapter.smarter
{
	
	/// <summary> Support for .hin, HyperChem's native file format.
	/// http://www.hyper.com
	/// <p />
	/// Record format is:
	/// <code>
	/// atom 1 - C ** - -0.06040 0.00000 0.00000 0.00000 3 2 a 6 a 38 s
	/// ...
	/// atom 67 - H ** - 0.17710 -7.10260 -3.74840 2.24660 1 34 s
	/// endmol 1
	/// </code>
	/// interesting fields are partialCharge, x, y, z, bondCount<br />
	/// bonds are atom number and s/d/t/a for single/double/triple/aromatic
	/// </summary>
	class HinReader:AtomSetCollectionReader
	{
		
		internal override AtomSetCollection readAtomSetCollection(System.IO.StreamReader reader)
		{
			
			atomSetCollection = new AtomSetCollection("hin");
			
			readAtoms(reader);
			if (errorMessage != null)
				atomSetCollection.errorMessage = errorMessage;
			else if (atomSetCollection.atomCount == 0)
				atomSetCollection.errorMessage = "No atoms in file";
			return atomSetCollection;
		}
		
		internal System.String errorMessage;
		
		internal int atomIndex;
		internal int baseAtomIndex;
		internal System.String[] tokens;
		
		internal const int MAX_TOKENS = 40; // should be plenty
		
		internal virtual void  readAtoms(System.IO.StreamReader reader)
		{
			
			tokens = new System.String[MAX_TOKENS];
			errorMessage = null;
			
			System.String line;
			while (errorMessage == null && (line = reader.ReadLine()) != null)
			{
				if (line.Length == 0 || line[0] == ';')
				// comment
					continue;
				if (line.StartsWith("mol "))
				// we have reached the start of a molecule
					processMol(line);
				else if (line.StartsWith("atom "))
					processAtom(line);
				else if (line.StartsWith("endmol "))
					processEndmol(line);
			}
			tokens = null;
		}
		
		internal virtual void  processMol(System.String line)
		{
			atomSetCollection.newAtomSet();
			System.String molName = getMolName(line);
			atomSetCollection.setAtomSetName(molName);
			atomIndex = 0;
			baseAtomIndex = atomSetCollection.atomCount;
		}
		
		internal virtual System.String getMolName(System.String line)
		{
			parseToken(line);
			parseToken(line, ichNextParse);
			return parseToken(line, ichNextParse);
		}
		
		internal virtual void  processAtom(System.String line)
		{
			
			int fileAtomNumber = parseInt(line, 5);
			if (fileAtomNumber - 1 != atomIndex)
			{
				errorMessage = "bad atom number sequence ... expected:" + (atomIndex + 1) + " found:" + fileAtomNumber;
				return ;
			}
			
			Atom atom = atomSetCollection.addNewAtom();
			parseToken(line, ichNextParse); // discard
			atom.elementSymbol = parseToken(line, ichNextParse);
			parseToken(line, ichNextParse); // discard
			parseToken(line, ichNextParse); // discard
			atom.partialCharge = parseFloat(line, ichNextParse);
			atom.x = parseFloat(line, ichNextParse);
			atom.y = parseFloat(line, ichNextParse);
			atom.z = parseFloat(line, ichNextParse);
			
			int bondCount = parseInt(line, ichNextParse);
			for (int i = 0; i < bondCount; ++i)
			{
				int otherAtomNumber = parseInt(line, ichNextParse);
				System.String bondTypeToken = parseToken(line, ichNextParse);
				if (otherAtomNumber > atomIndex)
					continue;
				int bondOrder;
				switch (bondTypeToken[0])
				{
					
					case 's': 
						bondOrder = 1;
						break;
					
					case 'd': 
						bondOrder = 2;
						break;
					
					case 't': 
						bondOrder = 3;
						break;
					
					case 'a': 
						bondOrder = JmolAdapter.ORDER_AROMATIC;
						break;
					
					default: 
						errorMessage = "unrecognized bond type:" + bondTypeToken + " atom #" + fileAtomNumber;
						return ;
					
				}
				atomSetCollection.addNewBond(baseAtomIndex + atomIndex, baseAtomIndex + otherAtomNumber - 1, bondOrder);
			}
			++atomIndex;
		}
		
		internal virtual void  processEndmol(System.String line)
		{
		}
	}
}