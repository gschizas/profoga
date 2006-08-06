/* $RCSfile$
* $Author: nicove $
* $Date: 2006-04-04 20:28:06 +0200 (mar., 04 avr. 2006) $
* $Revision: 4907 $
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
	
	/// <summary> Jaguar reader tested for the two samples files in CVS. Both
	/// these files were created with Jaguar version 4.0, release 20.
	/// </summary>
	class JaguarReader:AtomSetCollectionReader
	{
		
		internal override AtomSetCollection readAtomSetCollection(System.IO.StreamReader reader)
		{
			
			atomSetCollection = new AtomSetCollection("jaguar");
			
			try
			{
				System.String line;
				while ((line = reader.ReadLine()) != null)
				{
					if (line.StartsWith("  final geometry:"))
					{
						readAtoms(reader);
					}
					else if (line.StartsWith("  harmonic frequencies in"))
					{
						readFrequencies(reader);
						break;
					}
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
		
		internal virtual void  readAtoms(System.IO.StreamReader reader)
		{
			// we only take the last set of atoms before the frequencies
			atomSetCollection.discardPreviousAtoms();
			// start parsing the atoms
			discardLines(reader, 2);
			System.String line;
			while ((line = reader.ReadLine()) != null && line.Length >= 60 && line[2] != ' ')
			{
				System.String atomName = parseToken(line, 2, 7);
				float x = parseFloat(line, 8, 24);
				float y = parseFloat(line, 26, 42);
				float z = parseFloat(line, 44, 60);
				if (System.Single.IsNaN(x) || System.Single.IsNaN(y) || System.Single.IsNaN(z))
					return ;
				int len = atomName.Length;
				if (len < 2)
					return ;
				System.String elementSymbol;
				char ch2 = atomName[1];
				if (ch2 >= 'a' && ch2 <= 'z')
					elementSymbol = atomName.Substring(0, (2) - (0));
				else
					elementSymbol = atomName.Substring(0, (1) - (0));
				Atom atom = atomSetCollection.addNewAtom();
				atom.elementSymbol = elementSymbol;
				atom.atomName = atomName;
				atom.x = x; atom.y = y; atom.z = z;
			}
		}
		
		/* A block without symmetry, looks like:
		
		harmonic frequencies in cm**-1, IR intensities in km/mol, and normal modes:
		
		frequencies  1350.52  1354.79  1354.91  1574.28  1577.58  3047.10  3165.57
		intensities    14.07    13.95    13.92     0.00     0.00     0.00    25.19
		C1   X     0.00280 -0.11431  0.01076 -0.00008 -0.00001 -0.00028 -0.00406
		C1   Y    -0.00528  0.01062  0.11423 -0.00015 -0.00001 -0.00038  0.00850
		C1   Z     0.11479  0.00330  0.00502 -0.00006  0.00000  0.00007 -0.08748
		
		With symmetry:
		
		harmonic frequencies in cm**-1, IR intensities in km/mol, and normal modes:
		
		frequencies  1352.05  1352.11  1352.16  1574.91  1574.92  3046.33  3164.52
		symmetries   B3       B1       B3       A        A        A        B1      
		intensities    14.01    14.00    14.00     0.00     0.00     0.00    25.06
		C1   X     0.08399 -0.00233 -0.07841  0.00000  0.00000  0.00000 -0.01133
		C1   Y     0.06983 -0.05009  0.07631 -0.00001  0.00000  0.00000 -0.00283
		C1   Z     0.03571  0.10341  0.03519  0.00001  0.00000  0.00001 -0.08724
		*/
		internal int atomCount;
		
		internal virtual void  readFrequencies(System.IO.StreamReader reader)
		{
			atomCount = atomSetCollection.FirstAtomSetAtomCount;
			int modelNumber = 1;
			System.String line;
			while ((line = reader.ReadLine()) != null && !line.StartsWith("  frequencies "))
			{
			}
			if (line == null)
				return ;
			// determine number of freqs on this line (starting with "frequencies")
			do 
			{
				logger.log("Freqs found: " + line);
				int freqCount = new SupportClass.Tokenizer(line).Count - 1;
				logger.log("  #freqs= " + freqCount);
				while ((line = reader.ReadLine()) != null && !line.StartsWith("  intensities "))
				{
				}
				for (int atomCenterNumber = 0; atomCenterNumber < atomCount; atomCenterNumber++)
				{
					// this assumes that the atoms are given in the same order as their
					// atomic coordinates, and disregards the label which is should use
					line = reader.ReadLine();
					logger.log("Read X line for atom: " + line);
					SupportClass.Tokenizer tokenizerX = new SupportClass.Tokenizer(line);
					tokenizerX.NextToken(); tokenizerX.NextToken(); // disregard label and X/Y/Z
					SupportClass.Tokenizer tokenizerY = new SupportClass.Tokenizer(reader.ReadLine());
					tokenizerY.NextToken(); tokenizerY.NextToken();
					SupportClass.Tokenizer tokenizerZ = new SupportClass.Tokenizer(reader.ReadLine());
					tokenizerZ.NextToken(); tokenizerZ.NextToken();
					for (int j = 0; j < freqCount; j++)
					{
						float x = parseFloat(tokenizerX.NextToken());
						float y = parseFloat(tokenizerY.NextToken());
						float z = parseFloat(tokenizerZ.NextToken());
						recordAtomVector(modelNumber + j, atomCenterNumber, x, y, z);
					}
				}
				discardLines(reader, 1);
				modelNumber += freqCount;
			}
			while ((line = reader.ReadLine()) != null && (line.StartsWith("  frequencies ")));
		}
		
		internal virtual void  recordAtomVector(int modelNumber, int atomCenterNumber, float x, float y, float z)
		{
			if (System.Single.IsNaN(x) || System.Single.IsNaN(y) || System.Single.IsNaN(z))
				return ; // line is too short -- no data found
			if (atomCenterNumber <= 0 || atomCenterNumber > atomCount)
				return ;
			if (atomCenterNumber == 1)
			{
				if (modelNumber > 1)
					atomSetCollection.cloneFirstAtomSet();
			}
			Atom atom = atomSetCollection.atoms[(modelNumber - 1) * atomCount + atomCenterNumber - 1];
			atom.vectorX = x;
			atom.vectorY = y;
			atom.vectorZ = z;
		}
		
		internal override void  discardLines(System.IO.StreamReader reader, int nLines)
		{
			for (int i = nLines; --i >= 0; )
				reader.ReadLine();
		}
	}
}