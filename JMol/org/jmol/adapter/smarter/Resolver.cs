/* $RCSfile$
* $Author: hansonr $
* $Date: 2006-04-14 17:13:21 +0200 (ven., 14 avr. 2006) $
* $Revision: 4970 $
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
	
	class Resolver
	{
		
		internal static System.Object resolve(System.String name, System.IO.StreamReader bufferedReader, JmolAdapter.Logger logger)
		{
			AtomSetCollectionReader atomSetCollectionReader;
			System.String atomSetCollectionReaderName = determineAtomSetCollectionReader(bufferedReader, logger);
			logger.log("The Resolver thinks", atomSetCollectionReaderName);
			System.String className = "org.jmol.adapter.smarter." + atomSetCollectionReaderName + "Reader";
			
			if (atomSetCollectionReaderName == null)
				return "unrecognized file format";
			
			try
			{
				//UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
				System.Type atomSetCollectionReaderClass = System.Type.GetType(className);
				atomSetCollectionReader = (AtomSetCollectionReader) System.Activator.CreateInstance(atomSetCollectionReaderClass);
			}
			catch (System.Exception e)
			{
				System.String err = "Could not instantiate:" + className;
				logger.log(err);
				return err;
			}
			
			atomSetCollectionReader.Logger = logger;
			atomSetCollectionReader.initialize();
			
			AtomSetCollection atomSetCollection = atomSetCollectionReader.readAtomSetCollection(bufferedReader);
			atomSetCollection.freeze();
			if (atomSetCollection.errorMessage != null)
				return atomSetCollection.errorMessage;
			if (atomSetCollection.atomCount == 0)
				return "No atoms in file";
			return atomSetCollection;
		}
		
		internal static System.Object DOMResolve(System.Object DOMNode, JmolAdapter.Logger logger)
		{
			AtomSetCollectionReader atomSetCollectionReader;
			System.String atomSetCollectionReaderName = "Cml";
			logger.log("DOM Resolver thinks", atomSetCollectionReaderName);
			System.String className = "org.jmol.adapter.smarter." + atomSetCollectionReaderName + "Reader";
			
			try
			{
				//UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
				System.Type atomSetCollectionReaderClass = System.Type.GetType(className);
				atomSetCollectionReader = (AtomSetCollectionReader) System.Activator.CreateInstance(atomSetCollectionReaderClass);
			}
			catch (System.Exception e)
			{
				System.String err = "Could not instantiate:" + className;
				logger.log(err);
				return err;
			}
			
			atomSetCollectionReader.Logger = logger;
			atomSetCollectionReader.initialize();
			
			AtomSetCollection atomSetCollection = atomSetCollectionReader.readAtomSetCollectionFromDOM(DOMNode);
			atomSetCollection.freeze();
			if (atomSetCollection.errorMessage != null)
				return atomSetCollection.errorMessage;
			if (atomSetCollection.atomCount == 0)
				return "No atoms in file";
			return atomSetCollection;
		}
		
		internal static System.String determineAtomSetCollectionReader(System.IO.StreamReader bufferedReader, JmolAdapter.Logger logger)
		{
			System.String[] lines = new System.String[16];
			LimitedLineReader llr = new LimitedLineReader(bufferedReader, 16384);
			for (int i = 0; i < lines.Length; ++i)
				lines[i] = llr.readLineWithNewline();
			if (checkV3000(lines))
				return "V3000";
			if (checkMol(lines))
				return "Mol";
			if (checkXyz(lines))
				return "Xyz";
			if (checkFoldingXyz(lines))
				return "FoldingXyz";
			if (checkCube(lines))
				return "Cube";
			// run these loops forward ... easier for people to understand
			for (int i = 0; i < startsWithRecords.Length; ++i)
			{
				System.String[] recordTags = startsWithRecords[i];
				for (int j = 0; j < recordTags.Length; ++j)
				{
					System.String recordTag = recordTags[j];
					for (int k = 0; k < lines.Length; ++k)
					{
						if (lines[k].StartsWith(recordTag))
							return startsWithFormats[i];
					}
				}
			}
			for (int i = 0; i < containsRecords.Length; ++i)
			{
				System.String[] recordTags = containsRecords[i];
				for (int j = 0; j < recordTags.Length; ++j)
				{
					System.String recordTag = recordTags[j];
					for (int k = 0; k < lines.Length; ++k)
					{
						if (lines[k].IndexOf(recordTag) != - 1)
							return containsFormats[i];
					}
				}
			}
			
			if (lines[1] == null || lines[1].Trim().Length == 0)
				return "Jme"; // this is really quite broken :-)
			return null;
		}
		
		////////////////////////////////////////////////////////////////
		// file types that need special treatment
		////////////////////////////////////////////////////////////////
		
		internal static bool checkV3000(System.String[] lines)
		{
			if (lines[3].Length >= 6)
			{
				System.String line4trimmed = lines[3].Trim();
				if (line4trimmed.EndsWith("V3000"))
					return true;
			}
			return false;
		}
		
		internal static bool checkMol(System.String[] lines)
		{
			if (lines[3].Length >= 6)
			{
				System.String line4trimmed = lines[3].Trim();
				if (line4trimmed.EndsWith("V2000") || line4trimmed.EndsWith("v2000"))
					return true;
				try
				{
					System.Int32.Parse(lines[3].Substring(0, (3) - (0)).Trim());
					System.Int32.Parse(lines[3].Substring(3, (6) - (3)).Trim());
					return true;
				}
				catch (System.FormatException nfe)
				{
				}
			}
			return false;
		}
		
		internal static bool checkXyz(System.String[] lines)
		{
			try
			{
				System.Int32.Parse(lines[0].Trim());
				return true;
			}
			catch (System.FormatException nfe)
			{
			}
			return false;
		}
		
		internal static bool checkFoldingXyz(System.String[] lines)
		{
			try
			{
				SupportClass.Tokenizer tokens = new SupportClass.Tokenizer(lines[0].Trim(), " \t");
				if ((tokens != null) && (tokens.Count >= 2))
				{
					System.Int32.Parse(tokens.NextToken().Trim());
					return true;
				}
			}
			catch (System.FormatException nfe)
			{
			}
			return false;
		}
		
		internal static bool checkCube(System.String[] lines)
		{
			try
			{
				SupportClass.Tokenizer tokens2 = new SupportClass.Tokenizer(lines[2]);
				if (tokens2 == null || tokens2.Count != 4)
					return false;
				System.Int32.Parse(tokens2.NextToken());
				for (int i = 3; --i >= 0; )
				{
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					System.Single.Parse(tokens2.NextToken());
				}
				SupportClass.Tokenizer tokens3 = new SupportClass.Tokenizer(lines[3]);
				if (tokens3 == null || tokens3.Count != 4)
					return false;
				System.Int32.Parse(tokens3.NextToken());
				for (int i = 3; --i >= 0; )
				{
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					if ((float) (System.Single.Parse(tokens3.NextToken())) < 0)
						return false;
				}
				return true;
			}
			catch (System.FormatException nfe)
			{
			}
			return false;
		}
		
		////////////////////////////////////////////////////////////////
		// these test lines that startWith one of these strings
		////////////////////////////////////////////////////////////////
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'pdbRecords'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] pdbRecords = new System.String[]{"HEADER", "OBSLTE", "TITLE ", "CAVEAT", "COMPND", "SOURCE", "KEYWDS", "EXPDTA", "AUTHOR", "REVDAT", "SPRSDE", "JRNL  ", "REMARK", "DBREF ", "SEQADV", "SEQRES", "MODRES", "HELIX ", "SHEET ", "TURN  ", "CRYST1", "ORIGX1", "ORIGX2", "ORIGX3", "SCALE1", "SCALE2", "SCALE3", "ATOM  ", "HETATM", "MODEL "};
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'shelxRecords'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] shelxRecords = new System.String[]{"TITL ", "ZERR ", "LATT ", "SYMM ", "CELL "};
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'cifRecords'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] cifRecords = new System.String[]{"data_", "_publ"};
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'ghemicalMMRecords'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] ghemicalMMRecords = new System.String[]{"!Header mm1gp", "!Header gpr"};
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'jaguarRecords'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] jaguarRecords = new System.String[]{"  |  Jaguar version"};
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'hinRecords'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] hinRecords = new System.String[]{"mol "};
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'mdlRecords'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] mdlRecords = new System.String[]{"$MDL "};
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'nwchemRecords'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] nwchemRecords = new System.String[]{" argument  1"};
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'spartanSmolRecords'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] spartanSmolRecords = new System.String[]{"INPUT="};
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'csfRecords'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] csfRecords = new System.String[]{"local_transform"};
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'startsWithRecords '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[][] startsWithRecords = new System.String[][]{pdbRecords, shelxRecords, cifRecords, ghemicalMMRecords, jaguarRecords, hinRecords, mdlRecords, nwchemRecords, spartanSmolRecords, csfRecords};
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'startsWithFormats'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] startsWithFormats = new System.String[]{"Pdb", "Shelx", "Cif", "GhemicalMM", "Jaguar", "Hin", "Mol", "NWChem", "SpartanSmol", "Csf"};
		
		////////////////////////////////////////////////////////////////
		// contains formats
		////////////////////////////////////////////////////////////////
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'molproRecords'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] molproRecords = new System.String[]{"http://www.molpro.net/schema/molpro"};
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'cmlRecords'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] cmlRecords = new System.String[]{"<?xml", "<atom", "<molecule", "<reaction", "<cml", "<bond", ".dtd\"", "<list>", "<entry", "<identifier", "http://www.xml-cml.org/schema/cml2/core"};
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'gaussianRecords'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] gaussianRecords = new System.String[]{"Entering Gaussian System", "Entering Link 1", "1998 Gaussian, Inc."};
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'mopacRecords'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] mopacRecords = new System.String[]{"MOPAC 93 (c) Fujitsu", "MOPAC2002 (c) Fujitsu", "MOPAC FOR LINUX (PUBLIC DOMAIN VERSION)"};
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'qchemRecords'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] qchemRecords = new System.String[]{"Welcome to Q-Chem", "A Quantum Leap Into The Future Of Chemistry"};
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'gamessRecords'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] gamessRecords = new System.String[]{"GAMESS"};
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'spartanRecords'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] spartanRecords = new System.String[]{"Spartan"};
		
		// It's important that molpro gets looked for before cml otherwise molpro files will probably parse as cml
		//UPGRADE_NOTE: Final was removed from the declaration of 'containsRecords '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[][] containsRecords = new System.String[][]{molproRecords, cmlRecords, gaussianRecords, mopacRecords, qchemRecords, gamessRecords, spartanRecords};
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'containsFormats'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] containsFormats = new System.String[]{"Molpro", "Cml", "Gaussian", "Mopac", "Qchem", "Gamess", "Spartan"};
	}
	
	class LimitedLineReader
	{
		internal int readLimit;
		internal char[] buf;
		internal int cchBuf;
		internal int ichCurrent;
		
		internal LimitedLineReader(System.IO.StreamReader bufferedReader, int readLimit)
		{
			this.readLimit = readLimit;
			//UPGRADE_ISSUE: Method 'java.io.BufferedReader.mark' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaioBufferedReadermark_int'"
			bufferedReader.mark(readLimit);
			buf = new char[readLimit];
			//UPGRADE_TODO: Method 'java.io.Reader.read' was converted to 'System.IO.StreamReader.Read' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioReaderread_char[]'"
			cchBuf = bufferedReader.Read((System.Char[]) buf, 0, buf.Length);
			ichCurrent = 0;
			//UPGRADE_ISSUE: Method 'java.io.BufferedReader.reset' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaioBufferedReaderreset'"
			bufferedReader.reset();
		}
		
		internal virtual System.String readLineWithNewline()
		{
			// mth 2004 10 17
			// for now, I am going to put in a hack here
			// we have some CIF files with many lines of '#' comments
			// I believe that for all formats we can flush if the first
			// char of the line is a #
			// if this becomes a problem then we will need to adjust
			while (ichCurrent < cchBuf)
			{
				int ichBeginningOfLine = ichCurrent;
				char ch = (char) (0);
				while (ichCurrent < cchBuf && (ch = buf[ichCurrent++]) != '\r' && ch != '\n')
				{
				}
				if (ch == '\r' && ichCurrent < cchBuf && buf[ichCurrent] == '\n')
					++ichCurrent;
				int cchLine = ichCurrent - ichBeginningOfLine;
				if (buf[ichBeginningOfLine] == '#')
				// flush comment lines;
					continue;
				System.Text.StringBuilder sb = new System.Text.StringBuilder(cchLine);
				sb.Append(buf, ichBeginningOfLine, cchLine);
				return "" + sb;
			}
			// miguel 2005 01 26
			// for now, just return the empty string.
			// it will only affect the Resolver code
			// it will be easier to handle because then everyone does not
			// need to check for the null pointer
			//
			// If it becomes a problem, then change this to null and modify
			// all the code above to make sure that it tests for null before
			// attempting to invoke methods on the strings. 
			return "";
		}
	}
}