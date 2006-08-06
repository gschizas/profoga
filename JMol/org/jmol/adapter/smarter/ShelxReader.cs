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
	
	/// <summary> A reader for SHELX output (RES) files. It does not read all information.
	/// The list of fields that is read: TITL, REM, END, CELL, SPGR, SFAC
	/// In addition atoms are read.
	/// 
	/// <p>A reader for SHELX files. It currently supports SHELXL.
	/// 
	/// <p>The SHELXL format is described on the net:
	/// <a href="http://www.msg.ucsf.edu/local/programs/shelxl/ch_07.html"
	/// http://www.msg.ucsf.edu/local/programs/shelxl/ch_07.html</a>.
	/// 
	/// </summary>
	
	class ShelxReader:AtomSetCollectionReader
	{
		
		internal bool endReached;
		internal System.String[] sfacElementSymbols;
		
		internal override AtomSetCollection readAtomSetCollection(System.IO.StreamReader reader)
		{
			atomSetCollection = new AtomSetCollection("shelx");
			atomSetCollection.coordinatesAreFractional = true;
			
			System.String line;
			int lineLength;
			//UPGRADE_NOTE: Label 'readLine_loop' was moved. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1014'"
			while ((line = reader.ReadLine()) != null)
			{
				lineLength = line.Length;
				// '=' as last char of line means continue on next line
				while (lineLength > 0 && line[lineLength - 1] == '=')
					line = line.Substring(0, (lineLength - 1) - (0)) + reader.ReadLine();
				if (lineLength < 4)
				{
					if (lineLength == 3 && "END".ToUpper().Equals(line.ToUpper()))
						break;
					continue;
				}
				// FIXME -- should we call toUpperCase(Locale.US) ?
				// although I really don't think it is necessary
				System.String command = line.Substring(0, (4) - (0)).ToUpper();
				for (int i = unsupportedRecordTypes.Length; --i >= 0; )
					if (command.Equals(unsupportedRecordTypes[i]))
					{
						//UPGRADE_NOTE: Labeled continue statement was changed to a goto statement. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1015'"
						goto readLine_loop;
					}
				for (int i = supportedRecordTypes.Length; --i >= 0; )
					if (command.Equals(supportedRecordTypes[i]))
					{
						processSupportedRecord(i, line);
						if (endReached)
						{
							//UPGRADE_NOTE: Labeled break statement was changed to a goto statement. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1012'"
							goto readLine_loop_brk;
						}
						//UPGRADE_NOTE: Labeled continue statement was changed to a goto statement. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1015'"
						goto readLine_loop;
					}
				assumeAtomRecord(line);
				//UPGRADE_NOTE: Label 'readLine_loop' was moved. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1014'"
readLine_loop: ;
			}
			//UPGRADE_NOTE: Label 'readLine_loop_brk' was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1011'"
readLine_loop_brk: ;
			
			return atomSetCollection;
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'supportedRecordTypes'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] supportedRecordTypes = new System.String[]{"TITL", "CELL", "SPGR", "END ", "SFAC"};
		
		internal virtual void  processSupportedRecord(int recordIndex, System.String line)
		{
			switch (recordIndex)
			{
				
				case 0:  // TITL
					atomSetCollection.collectionName = parseTrimmed(line, 4);
					break;
				
				case 1:  // CELL
					cell(line);
					break;
				
				case 2:  // SPGR
					atomSetCollection.spaceGroup = parseTrimmed(line, 4);
					break;
				
				case 3:  // END
					endReached = true;
					break;
				
				case 4:  // SFAC
					parseSfacRecord(line);
					break;
				}
		}
		
		internal virtual void  cell(System.String line)
		{
			/* example:
			* CELL  1.54184   23.56421  7.13203 18.68928  90.0000 109.3799  90.0000
			* CELL   1.54184   7.11174  21.71704  30.95857  90.000  90.000  90.000
			*/
			float wavelength = parseFloat(line, 4);
			float[] notionalUnitcell = new float[6];
			for (int i = 0; i < 6; ++i)
				notionalUnitcell[i] = parseFloat(line, ichNextParse);
			atomSetCollection.wavelength = wavelength;
			atomSetCollection.notionalUnitcell = notionalUnitcell;
		}
		
		internal virtual void  parseSfacRecord(System.String line)
		{
			// an SFAC record is one of two cases
			// a simple SFAC record contains element names
			// a general SFAC record contains coefficients for a single element
			System.String[] sfacTokens = getTokens(line, 4);
			bool allElementSymbols = true;
			for (int i = sfacTokens.Length; allElementSymbols && --i >= 0; )
			{
				System.String token = sfacTokens[i];
				allElementSymbols = Atom.isValidElementSymbolNoCaseSecondChar(token);
			}
			if (allElementSymbols)
				parseSfacElementSymbols(sfacTokens);
			else
				parseSfacCoefficients(sfacTokens);
		}
		
		internal virtual void  parseSfacElementSymbols(System.String[] sfacTokens)
		{
			if (sfacElementSymbols == null)
			{
				sfacElementSymbols = sfacTokens;
			}
			else
			{
				int oldCount = sfacElementSymbols.Length;
				int tokenCount = sfacTokens.Length;
				sfacElementSymbols = setLength(sfacElementSymbols, oldCount + tokenCount);
				for (int i = tokenCount; --i >= 0; )
					sfacElementSymbols[oldCount + tokenCount] = sfacTokens[i];
			}
		}
		
		internal virtual void  parseSfacCoefficients(System.String[] sfacTokens)
		{
			float a1 = parseFloat(sfacTokens[1]);
			float a2 = parseFloat(sfacTokens[3]);
			float a3 = parseFloat(sfacTokens[5]);
			float a4 = parseFloat(sfacTokens[7]);
			float c = parseFloat(sfacTokens[9]);
			// element # is these floats rounded to nearest int
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			int z = (int) (a1 + a2 + a3 + a4 + c + 0.5f);
			System.String elementSymbol = getElementSymbol(z);
			int oldCount = 0;
			if (sfacElementSymbols == null)
			{
				sfacElementSymbols = new System.String[1];
			}
			else
			{
				oldCount = sfacElementSymbols.Length;
				sfacElementSymbols = setLength(sfacElementSymbols, oldCount + 1);
				sfacElementSymbols[oldCount] = elementSymbol;
			}
			sfacElementSymbols[oldCount] = elementSymbol;
		}
		
		internal virtual void  assumeAtomRecord(System.String line)
		{
			try
			{
				//    System.out.println("Assumed to contain an atom: " + line);
				// this line gives an atom, because all lines not starting with
				// a SHELX command is an atom
				System.String atomName = parseToken(line);
				int scatterFactor = parseInt(line, ichNextParse);
				float a = parseFloat(line, ichNextParse);
				float b = parseFloat(line, ichNextParse);
				float c = parseFloat(line, ichNextParse);
				// skip the rest
				
				Atom atom = atomSetCollection.addNewAtom();
				atom.atomName = atomName;
				if (sfacElementSymbols != null)
				{
					int elementIndex = scatterFactor - 1;
					if (elementIndex >= 0 && elementIndex < sfacElementSymbols.Length)
						atom.elementSymbol = sfacElementSymbols[elementIndex];
				}
				atom.x = a;
				atom.y = b;
				atom.z = c;
			}
			catch (System.Exception ex)
			{
				logger.log("Exception", ex, line);
			}
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'unsupportedRecordTypes'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] unsupportedRecordTypes = new System.String[]{"ZERR", "LATT", "SYMM", "DISP", "UNIT", "LAUE", "REM ", "MORE", "TIME", "HKLF", "OMIT", "SHEL", "BASF", "TWIN", "EXTI", "SWAT", "HOPE", "MERG", "SPEC", "RESI", "MOVE", "ANIS", "AFIX", "HFIX", "FRAG", "FEND", "EXYZ", "EXTI", "EADP", "EQIV", "CONN", "PART", "BIND", "FREE", "DFIX", "DANG", "BUMP", "SAME", "SADI", "CHIV", "FLAT", "DELU", "SIMU", "DEFS", "ISOR", "NCSY", "SUMP", "L.S.", "CGLS", "BLOC", "DAMP", "STIR", "WGHT", "FVAR", "BOND", "CONF", "MPLA", "RTAB", "HTAB", "LIST", "ACTA", "SIZE", "TEMP", "WPDB", "FMAP", "GRID", "PLAN", "MOLE", "    "};
	}
}