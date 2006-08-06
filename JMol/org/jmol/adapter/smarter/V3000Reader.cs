/* $RCSfile$
* $Author: hansonr $
* $Date: 2006-03-15 08:52:29 -0500 (Wed, 15 Mar 2006) $
* $Revision: 4614 $
*
* Copyright (C) 2006  Miguel, Jmol Development, www.jmol.org
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
	
	/// <summary> A reader for MDL V3000 files
	/// <p>
	/// <a href='http://www.mdli.com/downloads/public/ctfile/ctfile.jsp'>
	/// http://www.mdli.com/downloads/public/ctfile/ctfile.jsp
	/// </a>
	/// <p>
	/// </summary>
	class V3000Reader:AtomSetCollectionReader
	{
		
		internal int headerAtomCount;
		internal int headerBondCount;
		
		internal override AtomSetCollection readAtomSetCollection(System.IO.StreamReader reader)
		{
			atomSetCollection = new AtomSetCollection("v3000");
			bool startNewAtomSet = false;
			/*
			remove code for processing more than one molecular model in
			a .sdf file as multiple models.
			we are just going to read the first model
			String line;
			while (true) {
			line = processCtab(reader, startNewAtomSet);
			if (line == null)
			break;
			if (line.equals("$$$$"))
			startNewAtomSet = true;
			}
			*/
			processCtab(reader, startNewAtomSet);
			return atomSetCollection;
		}
		
		internal virtual System.String processCtab(System.IO.StreamReader reader, bool startNewAtomSet)
		{
			System.String line;
			line = reader.ReadLine();
			while (line != null && !line.Equals("$$$$") && !line.StartsWith("M  END"))
			{
				if (line.StartsWith("M  V30 BEGIN ATOM"))
				{
					line = processAtomBlock(reader);
					continue;
				}
				if (line.StartsWith("M  V30 BEGIN BOND"))
				{
					line = processBondBlock(reader);
					continue;
				}
				if (line.StartsWith("M  V30 BEGIN CTAB"))
				{
					if (startNewAtomSet)
						atomSetCollection.newAtomSet();
				}
				else if (line.StartsWith("M  V30 COUNTS"))
				{
					processCounts(line);
				}
				line = reader.ReadLine();
			}
			return line;
		}
		
		internal virtual void  processCounts(System.String line)
		{
			headerAtomCount = parseInt(line, 13);
			headerBondCount = parseInt(line, ichNextParse);
		}
		
		internal virtual System.String processAtomBlock(System.IO.StreamReader reader)
		{
			for (int i = headerAtomCount; --i >= 0; )
			{
				System.String line = readLineWithContinuation(reader);
				if (line == null || (!line.StartsWith("M  V30 ")))
					throw new System.Exception("unrecognized atom");
				Atom atom = new Atom();
				atom.atomSerial = parseInt(line, 7);
				atom.elementSymbol = parseToken(line, ichNextParse);
				atom.x = parseFloat(line, ichNextParse);
				atom.y = parseFloat(line, ichNextParse);
				atom.z = parseFloat(line, ichNextParse);
				parseInt(line, ichNextParse); // discard aamap
				while (true)
				{
					System.String option = parseToken(line, ichNextParse);
					if (option == null)
						break;
					if (option.StartsWith("CHG="))
						atom.formalCharge = parseInt(option, 4);
				}
				atomSetCollection.addAtomWithMappedSerialNumber(atom);
			}
			System.String line2 = reader.ReadLine();
			if (line2 == null || !line2.StartsWith("M  V30 END ATOM"))
				throw new System.Exception("M  V30 END ATOM not found");
			return line2;
		}
		
		internal virtual System.String processBondBlock(System.IO.StreamReader reader)
		{
			for (int i = headerBondCount; --i >= 0; )
			{
				System.String line = readLineWithContinuation(reader);
				if (line == null || (!line.StartsWith("M  V30 ")))
					throw new System.Exception("unrecognized bond");
				/*int bondSerial = */ parseInt(line, 7); // currently unused
				int order = parseInt(line, ichNextParse);
				int atomSerial1 = parseInt(line, ichNextParse);
				int atomSerial2 = parseInt(line, ichNextParse);
				atomSetCollection.addNewBondWithMappedSerialNumbers(atomSerial1, atomSerial2, order);
			}
			System.String line2 = reader.ReadLine();
			if (line2 == null || !line2.StartsWith("M  V30 END BOND"))
				throw new System.Exception("M  V30 END BOND not found");
			return line2;
		}
		
		internal virtual System.String readLineWithContinuation(System.IO.StreamReader reader)
		{
			System.String line = reader.ReadLine();
			if (line != null && line.Length > 7)
			{
				while (line[line.Length - 1] == '-')
				{
					System.String line2 = reader.ReadLine();
					if (line2 == null || !line.StartsWith("M  V30 "))
						throw new System.Exception("Invalid line continuation");
					line += line2.Substring(7);
				}
			}
			return line;
		}
	}
}