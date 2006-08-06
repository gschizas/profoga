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
	
	class SpartanReader:AtomSetCollectionReader
	{
		
		internal override AtomSetCollection readAtomSetCollection(System.IO.StreamReader reader)
		{
			
			atomSetCollection = new AtomSetCollection("spartan");
			
			try
			{
				if (discardLinesUntilContains(reader, "Cartesian Coordinates (Ang") != null)
					readAtoms(reader);
				if (discardLinesUntilContains(reader, "Vibrational Frequencies") != null)
					readFrequencies(reader);
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
		
		internal virtual void  readAtoms(System.IO.StreamReader reader)
		{
			discardLinesUntilBlank(reader);
			System.String line;
			while ((line = reader.ReadLine()) != null && (parseInt(line, 0, 3)) > 0)
			{
				System.String elementSymbol = parseToken(line, 4, 6);
				System.String atomName = parseToken(line, 7, 13);
				float x = parseFloat(line, 17, 30);
				float y = parseFloat(line, 31, 44);
				float z = parseFloat(line, 45, 58);
				Atom atom = atomSetCollection.addNewAtom();
				atom.elementSymbol = elementSymbol;
				atom.atomName = atomName;
				atom.x = x;
				atom.y = y;
				atom.z = z;
			}
		}
		
		internal virtual void  readFrequencies(System.IO.StreamReader reader)
		{
			int totalFrequencyCount = 0;
			
			while (true)
			{
				System.String line = discardLinesUntilNonBlank(reader);
				int lineBaseFreqCount = totalFrequencyCount;
				//      System.out.println("lineBaseFreqCount=" + lineBaseFreqCount);
				ichNextParse = 16;
				int lineFreqCount;
				for (lineFreqCount = 0; lineFreqCount < 3; ++lineFreqCount)
				{
					float frequency = parseFloat(line, ichNextParse);
					//        System.out.println("frequency=" + frequency);
					if (System.Single.IsNaN(frequency))
						break; //////////////// loop exit is here
					++totalFrequencyCount;
					if (totalFrequencyCount > 1)
						atomSetCollection.cloneFirstAtomSet();
				}
				if (lineFreqCount == 0)
					return ;
				Atom[] atoms = atomSetCollection.atoms;
				discardLines(reader, 2);
				int firstAtomSetAtomCount = atomSetCollection.FirstAtomSetAtomCount;
				for (int i = 0; i < firstAtomSetAtomCount; ++i)
				{
					line = reader.ReadLine();
					for (int j = 0; j < lineFreqCount; ++j)
					{
						int ichCoords = j * 23 + 10;
						float x = parseFloat(line, ichCoords, ichCoords + 7);
						float y = parseFloat(line, ichCoords + 7, ichCoords + 14);
						float z = parseFloat(line, ichCoords + 14, ichCoords + 21);
						int atomIndex = (lineBaseFreqCount + j) * firstAtomSetAtomCount + i;
						Atom atom = atoms[atomIndex];
						atom.vectorX = x;
						atom.vectorY = y;
						atom.vectorZ = z;
						//          System.out.println("x=" + x + " y=" + y + " z=" + z);
					}
				}
			}
		}
	}
}