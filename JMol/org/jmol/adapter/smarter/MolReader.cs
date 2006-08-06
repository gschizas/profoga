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
using JmolAdapter = org.jmol.api.JmolAdapter;
namespace org.jmol.adapter.smarter
{
	
	/// <summary> A reader for MDLI mol and sdf files.
	/// <p>
	/// <a href='http://www.mdli.com/downloads/public/ctfile/ctfile.jsp'>
	/// http://www.mdli.com/downloads/public/ctfile/ctfile.jsp
	/// </a>
	/// <p>
	/// </summary>
	class MolReader:AtomSetCollectionReader
	{
		
		internal override AtomSetCollection readAtomSetCollection(System.IO.StreamReader reader)
		{
			atomSetCollection = new AtomSetCollection("mol");
			System.String firstLine = reader.ReadLine();
			if (firstLine.StartsWith("$MDL"))
			{
				processRgHeader(reader, firstLine);
				//String line;
				while (!reader.ReadLine().StartsWith("$CTAB"))
				{
				}
				processCtab(reader);
			}
			else
			{
				processMolSdHeader(reader, firstLine);
				processCtab(reader);
			}
			return atomSetCollection;
		}
		
		internal virtual void  processMolSdHeader(System.IO.StreamReader reader, System.String firstLine)
		{
			atomSetCollection.CollectionName = firstLine;
			reader.ReadLine();
			reader.ReadLine();
		}
		
		internal virtual void  processRgHeader(System.IO.StreamReader reader, System.String firstLine)
		{
			System.String line;
			while ((line = reader.ReadLine()) != null && !line.StartsWith("$HDR"))
			{
			}
			if (line == null)
			{
				System.Console.Out.WriteLine("$HDR not found in MDL RG file");
				return ;
			}
			processMolSdHeader(reader, reader.ReadLine());
		}
		
		internal virtual void  processCtab(System.IO.StreamReader reader)
		{
			System.String countLine = reader.ReadLine();
			int atomCount = parseInt(countLine, 0, 3);
			int bondCount = parseInt(countLine, 3, 6);
			readAtoms(reader, atomCount);
			readBonds(reader, bondCount);
		}
		
		internal virtual void  readAtoms(System.IO.StreamReader reader, int atomCount)
		{
			for (int i = 0; i < atomCount; ++i)
			{
				System.String line = reader.ReadLine();
				System.String elementSymbol = "";
				if (line.Length > 34)
				{
					elementSymbol = String.Intern(line.Substring(31, (34) - (31)).Trim());
				}
				else
				{
					// deal with older Mol format where nothing after the symbol is used
					elementSymbol = String.Intern(line.Substring(31).Trim());
				}
				float x = parseFloat(line, 0, 10);
				float y = parseFloat(line, 10, 20);
				float z = parseFloat(line, 20, 30);
				int charge = 0;
				if (line.Length >= 39)
				{
					int chargeCode = parseInt(line, 36, 39);
					if (chargeCode >= 1 && chargeCode <= 7)
						charge = 4 - chargeCode;
				}
				Atom atom = atomSetCollection.addNewAtom();
				atom.elementSymbol = elementSymbol;
				atom.formalCharge = charge;
				atom.x = x; atom.y = y; atom.z = z;
			}
		}
		
		internal virtual void  readBonds(System.IO.StreamReader reader, int bondCount)
		{
			for (int i = 0; i < bondCount; ++i)
			{
				System.String line = reader.ReadLine();
				int atomIndex1 = parseInt(line, 0, 3);
				int atomIndex2 = parseInt(line, 3, 6);
				int order = parseInt(line, 6, 9);
				if (order == 4)
					order = JmolAdapter.ORDER_AROMATIC;
				atomSetCollection.addBond(new Bond(atomIndex1 - 1, atomIndex2 - 1, order));
			}
		}
	}
}