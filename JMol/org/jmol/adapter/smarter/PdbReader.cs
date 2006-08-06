/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-02-02 15:45:15 +0100 $
* $Revision: 4434 $
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
	
	class PdbReader:AtomSetCollectionReader
	{
		internal System.String line;
		internal int lineLength;
		// index into atoms array + 1
		// so that 0 can be used for the null value
		internal int[] serialMap = new int[512];
		
		internal bool isNMRdata;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'htFormul '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal System.Collections.Hashtable htFormul = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
		
		internal System.String currentGroup3;
		internal System.Collections.Hashtable htElementsInCurrentGroup;
		
		internal override AtomSetCollection readAtomSetCollection(System.IO.StreamReader reader)
		{
			
			atomSetCollection = new AtomSetCollection("pdb");
			
			atomSetCollection.pdbStructureRecords = new System.String[32];
			initialize();
			while ((line = reader.ReadLine()) != null)
			{
				lineLength = line.Length;
				if (line.StartsWith("ATOM  ") || line.StartsWith("HETATM"))
				{
					atom();
					continue;
				}
				if (line.StartsWith("CONECT"))
				{
					conect();
					continue;
				}
				if (line.StartsWith("HELIX ") || line.StartsWith("SHEET ") || line.StartsWith("TURN  "))
				{
					structure();
					continue;
				}
				if (line.StartsWith("MODEL "))
				{
					model();
					continue;
				}
				if (line.StartsWith("CRYST1"))
				{
					cryst1();
					continue;
				}
				if (line.StartsWith("SCALE1"))
				{
					scale1();
					continue;
				}
				if (line.StartsWith("SCALE2"))
				{
					scale2();
					continue;
				}
				if (line.StartsWith("SCALE3"))
				{
					scale3();
					continue;
				}
				if (line.StartsWith("EXPDTA"))
				{
					expdta();
					continue;
				}
				if (line.StartsWith("FORMUL"))
				{
					formul();
					continue;
				}
				if (line.StartsWith("HEADER") && lineLength >= 66)
				{
					atomSetCollection.CollectionName = line.Substring(62, (66) - (62));
					continue;
				}
			}
			serialMap = null;
			if (isNMRdata)
				atomSetCollection.notionalUnitcell = atomSetCollection.pdbScaleMatrix = atomSetCollection.pdbScaleTranslate = null;
			return atomSetCollection;
		}
		
		internal override void  initialize()
		{
			htFormul.Clear();
			currentGroup3 = null;
		}
		
		internal virtual void  atom()
		{
			bool isHetero = line.StartsWith("HETATM");
			try
			{
				// for now, we are only taking alternate location 'A'
				char charAlternateLocation = line[16];
				//      if (charAlternateLocation != ' ' && charAlternateLocation != 'A')
				//        return;
				////////////////////////////////////////////////////////////////
				// get the group so that we can check the formul
				int serial = parseInt(line, 6, 11);
				char chainID = line[21];
				int sequenceNumber = parseInt(line, 22, 26);
				char insertionCode = line[26];
				System.String group3 = parseToken(line, 17, 20);
				if (group3 == null)
				{
					currentGroup3 = group3;
					htElementsInCurrentGroup = null;
				}
				else if (!group3.Equals(currentGroup3))
				{
					currentGroup3 = group3;
					htElementsInCurrentGroup = (System.Collections.Hashtable) htFormul[group3];
				}
				
				////////////////////////////////////////////////////////////////
				// extract elementSymbol
				System.String elementSymbol = deduceElementSymbol();
				
				/****************************************************************
				* atomName
				****************************************************************/
				System.String rawAtomName = line.Substring(12, (16) - (12));
				// confusion|concern about the effect this will have on
				// atom expressions
				// but we have to do it to support mmCIF
				System.String atomName = rawAtomName.Trim();
				/****************************************************************
				* calculate the charge from cols 79 & 80 (1-based)
				* 2+, 3-, etc
				****************************************************************/
				int charge = 0;
				if (lineLength >= 80)
				{
					char chMagnitude = line[78];
					char chSign = line[79];
					if (chSign >= '0' && chSign <= '7')
					{
						char chT = chSign;
						chSign = chMagnitude;
						chMagnitude = chT;
					}
					if ((chSign == '+' || chSign == '-' || chSign == ' ') && chMagnitude >= '0' && chMagnitude <= '7')
					{
						charge = chMagnitude - '0';
						if (chSign == '-')
							charge = - charge;
					}
				}
				
				/****************************************************************
				* read the bfactor from cols 61-66 (1-based)
				****************************************************************/
				float bfactor = parseFloat(line, 60, 66);
				
				/****************************************************************
				* read the occupancy from cols 55-60 (1-based)
				* should be in the range 0.00 - 1.00
				****************************************************************/
				int occupancy = 100;
				float floatOccupancy = parseFloat(line, 54, 60);
				if (floatOccupancy != System.Single.NaN)
				{
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					occupancy = (int) (floatOccupancy * 100);
				}
				
				/****************************************************************/
				
				/****************************************************************
				* coordinates
				****************************************************************/
				float x = parseFloat(line, 30, 38);
				float y = parseFloat(line, 38, 46);
				float z = parseFloat(line, 46, 54);
				/****************************************************************/
				if (serial >= serialMap.Length)
					serialMap = setLength(serialMap, serial + 500);
				Atom atom = new Atom();
				atom.elementSymbol = elementSymbol;
				atom.atomName = atomName;
				if (charAlternateLocation != ' ')
					atom.alternateLocationID = charAlternateLocation;
				atom.formalCharge = charge;
				atom.occupancy = occupancy;
				atom.bfactor = bfactor;
				atom.x = x; atom.y = y; atom.z = z;
				atom.isHetero = isHetero;
				atom.chainID = chainID;
				atom.atomSerial = serial;
				atom.group3 = currentGroup3;
				atom.sequenceNumber = sequenceNumber;
				atom.insertionCode = JmolAdapter.canonizeInsertionCode(insertionCode);
				
				atomSetCollection.addAtom(atom);
				// note that values are +1 in this serial map
				serialMap[serial] = atomSetCollection.atomCount;
			}
			catch (System.FormatException e)
			{
				logger.log("bad record", "" + line);
			}
		}
		
		internal virtual System.String deduceElementSymbol()
		{
			if (lineLength >= 78)
			{
				char ch76 = line[76];
				char ch77 = line[77];
				if (ch76 == ' ' && Atom.isValidElementSymbol(ch77))
					return "" + ch77;
				if (Atom.isValidElementSymbolNoCaseSecondChar(ch76, ch77))
					return "" + ch76 + ch77;
			}
			char ch12 = line[12];
			char ch13 = line[13];
			if ((htElementsInCurrentGroup == null || htElementsInCurrentGroup[line.Substring(12, (14) - (12))] != null) && Atom.isValidElementSymbolNoCaseSecondChar(ch12, ch13))
				return "" + ch12 + ch13;
			if ((htElementsInCurrentGroup == null || htElementsInCurrentGroup["" + ch13] != null) && Atom.isValidElementSymbol(ch13))
				return "" + ch13;
			if ((htElementsInCurrentGroup == null || htElementsInCurrentGroup["" + ch12] != null) && Atom.isValidElementSymbol(ch12))
				return "" + ch12;
			return "Xx";
		}
		
		internal virtual void  conect()
		{
			int sourceSerial = - 1;
			int sourceIndex = - 1;
			try
			{
				sourceSerial = parseInt(line, 6, 11);
				sourceIndex = serialMap[sourceSerial] - 1;
				if (sourceIndex < 0)
					return ;
				// use this for HBONDS
				for (int i = 0; i < 9; i += (i == 5?2:1))
				{
					//      for (int i = 0; i < 4; i += (i == 5 ? 2 : 1)) {
					int targetSerial = getTargetSerial(i);
					if (targetSerial < 0)
						continue;
					int targetIndex = serialMap[targetSerial] - 1;
					if (targetIndex < 0)
						continue;
					if (atomSetCollection.bondCount > 0)
					{
						Bond bond = atomSetCollection.bonds[atomSetCollection.bondCount - 1];
						if (i < 4 && bond.atomIndex1 == sourceIndex && bond.atomIndex2 == targetIndex)
						{
							++bond.order;
							continue;
						}
					}
					//        if (i >= 4)
					//          logger.log("hbond:" + sourceIndex + "->" + targetIndex);
					atomSetCollection.addBond(new Bond(sourceIndex, targetIndex, i < 4?1:JmolAdapter.ORDER_HBOND));
				}
			}
			catch (System.Exception e)
			{
			}
		}
		
		internal virtual int getTargetSerial(int i)
		{
			int offset = i * 5 + 11;
			int offsetEnd = offset + 5;
			if (offsetEnd <= lineLength)
				return parseInt(line, offset, offsetEnd);
			return System.Int32.MinValue;
		}
		
		internal virtual void  structure()
		{
			System.String structureType = "none";
			int startChainIDIndex;
			int startIndex;
			int endChainIDIndex;
			int endIndex;
			if (line.StartsWith("HELIX "))
			{
				structureType = "helix";
				startChainIDIndex = 19;
				startIndex = 21;
				endChainIDIndex = 31;
				endIndex = 33;
			}
			else if (line.StartsWith("SHEET "))
			{
				structureType = "sheet";
				startChainIDIndex = 21;
				startIndex = 22;
				endChainIDIndex = 32;
				endIndex = 33;
			}
			else if (line.StartsWith("TURN  "))
			{
				structureType = "turn";
				startChainIDIndex = 19;
				startIndex = 20;
				endChainIDIndex = 30;
				endIndex = 31;
			}
			else
				return ;
			
			if (lineLength < endIndex + 4)
				return ;
			
			char startChainID = line[startChainIDIndex];
			int startSequenceNumber = parseInt(line, startIndex, startIndex + 4);
			char startInsertionCode = line[startIndex + 4];
			char endChainID = line[endChainIDIndex];
			int endSequenceNumber = parseInt(line, endIndex, endIndex + 4);
			// some files are chopped to remove trailing whitespace
			char endInsertionCode = ' ';
			if (lineLength > endIndex + 4)
				endInsertionCode = line[endIndex + 4];
			
			// this should probably call Structure.validateAndAllocate
			// in order to check validity of parameters
			Structure structure = new Structure(structureType, startChainID, startSequenceNumber, startInsertionCode, endChainID, endSequenceNumber, endInsertionCode);
			atomSetCollection.addStructure(structure);
		}
		
		internal virtual void  model()
		{
			/****************************************************************
			* mth 2004 02 28
			* note that the pdb spec says:
			* COLUMNS       DATA TYPE      FIELD         DEFINITION
			* ----------------------------------------------------------------------
			*  1 -  6       Record name    "MODEL "
			* 11 - 14       Integer        serial        Model serial number.
			*
			* but I received a file with the serial
			* number right after the word MODEL :-(
			****************************************************************/
			try
			{
				int startModelColumn = 6; // should be 10 0-based
				int endModelColumn = 14;
				if (endModelColumn > lineLength)
					endModelColumn = lineLength;
				int modelNumber = parseInt(line, startModelColumn, endModelColumn);
				atomSetCollection.newAtomSet();
				atomSetCollection.setAtomSetNumber(modelNumber);
			}
			catch (System.FormatException e)
			{
			}
		}
		
		internal virtual void  cryst1()
		{
			try
			{
				float a = getFloat(6, 9);
				float b = getFloat(15, 9);
				float c = getFloat(24, 9);
				float alpha = getFloat(33, 7);
				float beta = getFloat(40, 7);
				float gamma = getFloat(47, 7);
				float[] notionalUnitcell = atomSetCollection.notionalUnitcell = new float[6];
				notionalUnitcell[0] = a;
				notionalUnitcell[1] = b;
				notionalUnitcell[2] = c;
				notionalUnitcell[3] = alpha;
				notionalUnitcell[4] = beta;
				notionalUnitcell[5] = gamma;
			}
			catch (System.Exception e)
			{
			}
		}
		
		internal virtual float getFloat(int ich, int cch)
		{
			return parseFloat(line, ich, ich + cch);
		}
		
		internal virtual void  scale(int n)
		{
			atomSetCollection.pdbScaleMatrix[n * 3 + 0] = getFloat(10, 10);
			atomSetCollection.pdbScaleMatrix[n * 3 + 1] = getFloat(20, 10);
			atomSetCollection.pdbScaleMatrix[n * 3 + 2] = getFloat(30, 10);
			float translation = getFloat(45, 10);
			if (translation != 0)
			{
				if (atomSetCollection.pdbScaleTranslate == null)
					atomSetCollection.pdbScaleTranslate = new float[3];
				atomSetCollection.pdbScaleTranslate[n] = translation;
			}
		}
		
		internal virtual void  scale1()
		{
			try
			{
				atomSetCollection.pdbScaleMatrix = new float[9];
				scale(0);
			}
			catch (System.Exception e)
			{
				atomSetCollection.pdbScaleMatrix = null;
				logger.log("scale1 died:" + 3);
			}
		}
		
		internal virtual void  scale2()
		{
			try
			{
				scale(1);
			}
			catch (System.Exception e)
			{
				atomSetCollection.pdbScaleMatrix = null;
				logger.log("scale2 died");
			}
		}
		
		internal virtual void  scale3()
		{
			try
			{
				scale(2);
			}
			catch (System.Exception e)
			{
				atomSetCollection.pdbScaleMatrix = null;
				logger.log("scale3 died");
			}
		}
		
		internal virtual void  expdta()
		{
			System.String technique = parseTrimmed(line, 10).ToLower();
			if (String.Compare(technique, 0, "nmr", 0, 3, true) == 0)
				isNMRdata = true;
		}
		
		internal virtual void  formul()
		{
			System.String groupName = parseToken(line, 12, 15);
			System.String formula = parseTrimmed(line, 19, 70);
			int ichLeftParen = formula.IndexOf('(');
			if (ichLeftParen >= 0)
			{
				int ichRightParen = formula.IndexOf(')');
				if (ichRightParen < 0 || ichLeftParen >= ichRightParen || ichLeftParen + 1 == ichRightParen)
				// pick up () case in 1SOM.pdb
					return ; // invalid formula;
				formula = parseTrimmed(formula, ichLeftParen + 1, ichRightParen);
			}
			System.Collections.Hashtable htElementsInGroup = (System.Collections.Hashtable) htFormul[groupName];
			if (htElementsInGroup == null)
				htFormul[groupName] = htElementsInGroup = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
			// now, look for atom names in the formula
			ichNextParse = 0;
			System.String elementWithCount;
			while ((elementWithCount = parseToken(formula, ichNextParse)) != null)
			{
				if (elementWithCount.Length < 2)
					continue;
				char chFirst = elementWithCount[0];
				char chSecond = elementWithCount[1];
				if (Atom.isValidElementSymbolNoCaseSecondChar(chFirst, chSecond))
					htElementsInGroup["" + chFirst + chSecond] = true;
				else if (Atom.isValidElementSymbol(chFirst))
					htElementsInGroup["" + chFirst] = true;
			}
		}
	}
}