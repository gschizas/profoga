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
	
	class GamessReader:AtomSetCollectionReader
	{
		
		internal const float angstromsPerBohr = 0.529177f;
		
		internal override AtomSetCollection readAtomSetCollection(System.IO.StreamReader reader)
		{
			
			atomSetCollection = new AtomSetCollection("gamess");
			
			try
			{
				discardLinesUntilContains(reader, "COORDINATES (BOHR)");
				readAtomsInBohrCoordinates(reader);
				discardLinesUntilContains(reader, "FREQUENCIES IN CM");
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
		
		internal virtual void  readAtomsInBohrCoordinates(System.IO.StreamReader reader)
		{
			reader.ReadLine(); // discard one line
			System.String line;
			System.String atomName;
			atomSetCollection.newAtomSet();
			while ((line = reader.ReadLine()) != null && (atomName = parseToken(line, 1, 6)) != null)
			{
				float x = parseFloat(line, 17, 37);
				float y = parseFloat(line, 37, 57);
				float z = parseFloat(line, 57, 77);
				if (System.Single.IsNaN(x) || System.Single.IsNaN(y) || System.Single.IsNaN(z))
					break;
				Atom atom = atomSetCollection.addNewAtom();
				atom.atomName = atomName;
				atom.x = x * angstromsPerBohr;
				atom.y = y * angstromsPerBohr;
				atom.z = z * angstromsPerBohr;
			}
		}
		
		internal virtual void  readFrequencies(System.IO.StreamReader reader)
		{
			int totalFrequencyCount = 0;
			int atomCountInFirstModel = atomSetCollection.atomCount;
			float[] xComponents = new float[5];
			float[] yComponents = new float[5];
			float[] zComponents = new float[5];
			
			System.String line = discardLinesUntilContains(reader, "FREQUENCY:");
			while (line != null && line.IndexOf("FREQUENCY:") >= 0)
			{
				int lineBaseFreqCount = totalFrequencyCount;
				ichNextParse = 17;
				int lineFreqCount;
				for (lineFreqCount = 0; lineFreqCount < 5; ++lineFreqCount)
				{
					float frequency = parseFloat(line, ichNextParse);
					//        System.out.println("frequency=" + frequency);
					if (System.Single.IsNaN(frequency))
						break;
					++totalFrequencyCount;
					if (totalFrequencyCount > 1)
						atomSetCollection.cloneFirstAtomSet();
				}
				Atom[] atoms = atomSetCollection.atoms;
				discardLinesUntilBlank(reader);
				for (int i = 0; i < atomCountInFirstModel; ++i)
				{
					readComponents(reader.ReadLine(), lineFreqCount, xComponents);
					readComponents(reader.ReadLine(), lineFreqCount, yComponents);
					readComponents(reader.ReadLine(), lineFreqCount, zComponents);
					for (int j = 0; j < lineFreqCount; ++j)
					{
						int atomIndex = (lineBaseFreqCount + j) * atomCountInFirstModel + i;
						Atom atom = atoms[atomIndex];
						atom.vectorX = xComponents[j];
						atom.vectorY = yComponents[j];
						atom.vectorZ = zComponents[j];
					}
				}
				discardLines(reader, 12);
				line = reader.ReadLine();
			}
		}
		
		internal virtual void  readComponents(System.String line, int count, float[] components)
		{
			for (int i = 0, start = 20; i < count; ++i, start += 12)
				components[i] = parseFloat(line, start, start + 12);
		}
	}
}