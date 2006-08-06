/* $RCSfile$
* $Author: hansonr $
* $Date: 2006-04-05 20:59:08 +0200 (mer., 05 avr. 2006) $
* $Revision: 4915 $
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
	
	class SpartanSmolReader:AtomSetCollectionReader
	{
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'logging '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal bool logging = false;
		
		internal override AtomSetCollection readAtomSetCollection(System.IO.StreamReader reader)
		{
			
			
			atomSetCollection = new AtomSetCollection("spartan .smol");
			
			try
			{
				discardLinesUntilStartsWith(reader, "BEGINARCHIVE");
				if (discardLinesUntilContains(reader, "GEOMETRY") != null)
					readAtoms(reader);
				if (discardLinesUntilContains(reader, "BEGINPROPARC") != null)
					readProperties(reader);
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
			//no need to discard after GEOMETRY
			//discardLines(reader, 2);
			System.String line;
			int atomNum;
			logger.log("Reading BEGINARCHIVE GEOMETERY atom records...");
			while ((line = reader.ReadLine()) != null && (atomNum = parseInt(line, 0, 5)) > 0)
			{
				//logger.log("atom: " + line);
				/*
				was for OUTPUT section  
				String elementSymbol = parseToken(line, 10, 12);
				float x = parseFloat(line, 17, 30);
				float y = parseFloat(line, 31, 43);
				float z = parseFloat(line, 44, 58);
				*/
				float x = parseFloat(line, 6, 19);
				float y = parseFloat(line, 20, 33);
				float z = parseFloat(line, 34, 47);
				Atom atom = atomSetCollection.addNewAtom();
				atom.elementSymbol = getElementSymbol(atomNum);
				atom.x = x * ANGSTROMS_PER_BOHR;
				atom.y = y * ANGSTROMS_PER_BOHR;
				atom.z = z * ANGSTROMS_PER_BOHR;
			}
			atomSetCollection.setAtomSetName("Geometry");
		}
		
		internal virtual void  readProperties(System.IO.StreamReader reader)
		{
			System.String line;
			logger.log("Reading PROPARC properties records...");
			while ((line = reader.ReadLine()) != null && (line.Length < 10 || !line.Substring(0, (10) - (0)).Equals("ENDPROPARC")))
			{
				if (line.Length >= 4 && line.Substring(0, (4) - (0)).Equals("PROP"))
					readProperty(reader, line);
				if (line.Length >= 7 && line.Substring(0, (7) - (0)).Equals("VIBFREQ"))
					readVibFreqs(reader);
			}
		}
		
		internal virtual void  readProperty(System.IO.StreamReader reader, System.String line)
		{
			System.String[] tokens = getTokens(line);
			if (tokens.Length == 0)
				return ;
			//logger.log("reading property line:" + line);
			bool isString = (tokens[1].Equals("STRING"));
			System.String keyName = tokens[2];
			System.Object value_Renamed = new System.Object();
			System.Collections.ArrayList vector = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			if (tokens[3].Equals("="))
			{
				if (isString)
				{
					value_Renamed = getString(line, tokens[4].Substring(0, (1) - (0)));
				}
				else
				{
					value_Renamed = (float) parseFloat(tokens[4]);
				}
			}
			else if (tokens[tokens.Length - 1].Equals("BEGIN"))
			{
				int nValues = parseInt(tokens[tokens.Length - 2]);
				if (nValues == 0)
					nValues = 1;
				bool isArray = (tokens.Length == 6);
				System.Collections.ArrayList atomInfo = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
				while ((line = reader.ReadLine()) != null && !line.Substring(0, (3) - (0)).Equals("END"))
				{
					if (isString)
					{
						value_Renamed = getString(line, "\"");
						vector.Add(value_Renamed);
					}
					else
					{
						System.String[] tokens2 = getTokens(line);
						for (int i = 0; i < tokens2.Length; i++)
						{
							if (isArray)
							{
								atomInfo.Add((float) parseFloat(tokens2[i]));
								if ((i + 1) % nValues == 0)
								{
									vector.Add(atomInfo);
									atomInfo = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
								}
							}
							else
							{
								value_Renamed = (float) parseFloat(tokens2[i]);
								vector.Add(value_Renamed);
							}
						}
					}
				}
				value_Renamed = null;
			}
			else
			{
				logger.log(" Skipping property line " + line);
			}
			//logger.log(keyName + " = " + value + " ; " + vector);
			if (value_Renamed != null)
				atomSetCollection.setAtomSetCollectionAuxiliaryInfo(keyName, value_Renamed);
			if (vector.Count != 0)
				atomSetCollection.setAtomSetCollectionAuxiliaryInfo(keyName, vector);
		}
		
		//logger.log("reading property line:" + line);
		
		internal virtual void  readVibFreqs(System.IO.StreamReader reader)
		{
			System.String line = reader.ReadLine();
			System.String label = "";
			int frequencyCount = parseInt(line);
			System.Collections.ArrayList vibrations = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			System.Collections.ArrayList freqs = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			logger.log("reading VIBFREQ vibration records: frequencyCount = " + frequencyCount);
			for (int i = 0; i < frequencyCount; ++i)
			{
				atomSetCollection.cloneLastAtomSet();
				line = reader.ReadLine();
				System.Collections.Hashtable info = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
				float freq = parseFloat(line);
				info["freq"] = (float) freq;
				if (line.Length > 15 && !(label = line.Substring(15, (line.Length) - (15))).Equals("???"))
					info["label"] = label;
				freqs.Add(info);
				atomSetCollection.setAtomSetName(label + " " + freq + " cm^-1");
				atomSetCollection.setAtomSetProperty(SmarterJmolAdapter.PATH_KEY, "Frequencies");
			}
			// System.out.print(freqs);
			atomSetCollection.setAtomSetCollectionAuxiliaryInfo("VibFreqs", freqs);
			int atomCount = atomSetCollection.FirstAtomSetAtomCount;
			Atom[] atoms = atomSetCollection.atoms;
			System.Collections.ArrayList vib = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			System.Collections.ArrayList vibatom = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			int ifreq = 0;
			int iatom = atomCount; // add vibrations starting at second atomset
			int nValues = 3;
			float[] atomInfo = new float[3];
			while ((line = reader.ReadLine()) != null)
			{
				System.String[] tokens2 = getTokens(line);
				for (int i = 0; i < tokens2.Length; i++)
				{
					float f = parseFloat(tokens2[i]);
					atomInfo[i % nValues] = f;
					vibatom.Add((float) f);
					if ((i + 1) % nValues == 0)
					{
						if (logging)
							logger.log(ifreq + " atom " + iatom + "/" + atomCount + " vectors: " + atomInfo[0] + " " + atomInfo[1] + " " + atomInfo[2]);
						atoms[iatom].addVibrationVector(atomInfo[0], atomInfo[1], atomInfo[2]);
						vib.Add(vibatom);
						vibatom = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
						++iatom;
					}
				}
				if (iatom % atomCount == 0)
				{
					vibrations.Add(vib);
					vib = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
					if (++ifreq == frequencyCount)
						break; ///loop exit
				}
			}
			atomSetCollection.setAtomSetCollectionAuxiliaryInfo("vibration", vibrations);
		}
	}
}