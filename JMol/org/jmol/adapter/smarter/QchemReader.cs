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
namespace org.jmol.adapter.smarter
{
	/// <summary> A reader for Q-Chem 2.1
	/// Q-Chem  is a quantum chemistry program developed
	/// by Q-Chem, Inc. (http://www.q-chem.com/)
	/// 
	/// <p> Molecular coordinates and normal coordinates of
	/// vibrations are read. 
	/// 
	/// <p> This reader was developed from a single
	/// output file, and therefore, is not guaranteed to
	/// properly read all Q-chem output. If you have problems,
	/// please contact the author of this code, not the developers
	/// of Q-chem.
	/// 
	/// <p> This is a hacked version of Miguel's GaussianReader
	/// 
	/// </summary>
	/// <author>  Steven E. Wheeler (swheele2@ccqc.uga.edu)
	/// </author>
	/// <version>  1.0
	/// </version>
	
	class QchemReader:AtomSetCollectionReader
	{
		
		internal override AtomSetCollection readAtomSetCollection(System.IO.StreamReader reader)
		{
			
			atomSetCollection = new AtomSetCollection("qchem");
			
			try
			{
				System.String line;
				int lineNum = 0;
				while ((line = reader.ReadLine()) != null)
				{
					if (line.IndexOf("Standard Nuclear Orientation") >= 0)
					{
						readAtoms(reader);
					}
					else if (line.IndexOf("VIBRATIONAL FREQUENCIES") >= 0)
					{
						readFrequencies(reader);
						break;
					}
					else if (line.IndexOf("Mulliken Net Atomic Charges") >= 0)
					{
						readPartialCharges(reader);
					}
					++lineNum;
				}
			}
			catch (System.Exception ex)
			{
				SupportClass.WriteStackTrace(ex, Console.Error);
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				atomSetCollection.errorMessage = "Could not read file:" + ex;
				return atomSetCollection;
			}
			if (atomSetCollection.atomCount == 0)
			{
				atomSetCollection.errorMessage = "No atoms in file";
			}
			return atomSetCollection;
		}
		
		/* Q-chem 2.1 format:
		Standard Nuclear Orientation (Angstroms)
		I     Atom         X            Y            Z
		----------------------------------------------------
		1      H       0.000000     0.000000     4.756791*/
		
		// offset of coordinates within 'Standard Nuclear Orientation:'
		internal int coordinateBase = 16;
		// number of lines to skip after 'Frequencies:' to get to the vectors
		internal int frequencyLineSkipCount = 4;
		
		internal int atomCount;
		
		internal virtual void  readAtoms(System.IO.StreamReader reader)
		{
			// we only take the last set of atoms before the frequencies
			atomSetCollection.discardPreviousAtoms();
			atomCount = 0;
			discardLines(reader, 2);
			System.String line;
			while ((line = reader.ReadLine()) != null && !line.StartsWith(" --"))
			{
				/*String centerNumber = */ parseToken(line, 0, 5);
				System.String aname = parseToken(line, 6, 12);
				if (aname.IndexOf("X") == 1)
				{
					// skip dummy atoms
					continue;
				}
				
				//q-chem specific offsets
				float x = parseFloat(line, coordinateBase, coordinateBase + 13);
				float y = parseFloat(line, coordinateBase + 13, coordinateBase + 26);
				float z = parseFloat(line, coordinateBase + 26, coordinateBase + 39);
				if (System.Single.IsNaN(x) || System.Single.IsNaN(y) || System.Single.IsNaN(z))
					continue;
				Atom atom = atomSetCollection.addNewAtom();
				atom.elementSymbol = aname;
				atom.x = x; atom.y = y; atom.z = z;
				++atomCount;
			}
		}
		
		internal virtual void  readFrequencies(System.IO.StreamReader reader)
		{
			int modelNumber = 1;
			System.String line;
			while ((line = reader.ReadLine()) != null && !line.StartsWith(" Frequency:"))
			{
			}
			if (line == null)
				return ;
			do 
			{
				// FIXME  We'll want to read in the frequency of the vibration
				// at some point
				discardLines(reader, frequencyLineSkipCount);
				for (int i = 0; i < atomCount; ++i)
				{
					line = reader.ReadLine();
					for (int j = 0, col = 12; j < 3; ++j, col += 23)
					{
						float x = parseFloat(line, col, col + 5);
						float y = parseFloat(line, col + 7, col + 12);
						float z = parseFloat(line, col + 14, col + 19);
						
						recordAtomVector(modelNumber + j, i + 1, x, y, z);
					}
				}
				discardLines(reader, 1);
				modelNumber += 3;
			}
			while ((line = reader.ReadLine()) != null && line.StartsWith(" Frequency:"));
		}
		
		internal virtual void  recordAtomVector(int modelNumber, int atomCenterNumber, float x, float y, float z)
		{
			if (System.Single.IsNaN(x) || System.Single.IsNaN(y) || System.Single.IsNaN(z))
				return ; // no data found
			if (atomCenterNumber <= 0 || atomCenterNumber > atomCount)
				return ;
			if (atomCenterNumber == 1 && modelNumber > 1)
				atomSetCollection.cloneFirstAtomSet();
			
			Atom atom = atomSetCollection.atoms[(modelNumber - 1) * atomCount + atomCenterNumber - 1];
			atom.vectorX = x;
			atom.vectorY = y;
			atom.vectorZ = z;
		}
		
		internal virtual void  readPartialCharges(System.IO.StreamReader reader)
		{
			discardLines(reader, 3);
			System.String line;
			for (int i = 0; i < atomCount && (line = reader.ReadLine()) != null; ++i)
				atomSetCollection.atoms[i].partialCharge = parseFloat(line, 29, 38);
		}
	}
}