/* $RCSfile$
* $Author: nicove $
* $Date: 2006-04-14 19:51:43 +0200 (ven., 14 avr. 2006) $
* $Revision: 4975 $
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
	
	/// <summary> CIF file reader for CIF and mmCIF files.
	/// 
	/// <p>
	/// <a href='http://www.iucr.org/iucr-top/cif/'>
	/// http://www.iucr.org/iucr-top/cif/
	/// </a>
	/// 
	/// </summary>
	/// <author>  Miguel <miguel@jmol.org>
	/// </author>
	class CifReader:AtomSetCollectionReader
	{
		public CifReader()
		{
			InitBlock();
		}
		private void  InitBlock()
		{
			tokenizer = new RidiculousFileFormatTokenizer();
		}
		
		internal float[] notionalUnitcell;
		
		internal System.IO.StreamReader reader;
		internal System.String line;
		//UPGRADE_NOTE: The initialization of  'tokenizer' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		internal RidiculousFileFormatTokenizer tokenizer;
		internal System.Collections.IDictionary strandsMap;
		
		internal override void  initialize()
		{
			notionalUnitcell = new float[6];
			for (int i = 6; --i >= 0; )
				notionalUnitcell[i] = System.Single.NaN;
		}
		
		internal override AtomSetCollection readAtomSetCollection(System.IO.StreamReader reader)
		{
			this.reader = reader;
			atomSetCollection = new AtomSetCollection("cif");
			strandsMap = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
			
			// this loop is a little tricky
			// the CIF format seems to generate lots of problems for parsers
			// the top of this loop should be ready to process the current line
			// pay careful attention to the 'break' and 'continue' sequence
			// or you will get stuck in an infinite loop
			line = reader.ReadLine();
			while (line != null)
			{
				if (line.StartsWith("loop_"))
				{
					processLoopBlock();
					// there is already an unprocessed line in the firing chamber
					continue;
				}
				else if (line.StartsWith("data_"))
				{
					processDataParameter();
				}
				else if (line.StartsWith("_cell_") || line.StartsWith("_cell."))
				{
					processCellParameter();
				}
				else if (line.StartsWith("_symmetry_space_group_name_H-M"))
				{
					processSymmetrySpaceGroupNameHM();
				}
				else if (line.StartsWith("_struct_asym"))
				{
					processStructAsymBlock();
				}
				line = reader.ReadLine();
			}
			checkUnitcell();
			logger.log("Done reading... Found these strands:");
			System.Collections.IEnumerator strands = strandsMap.Values.GetEnumerator();
			//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
			while (strands.MoveNext())
			{
				//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
				Strand strand = (Strand) strands.Current;
				logger.log(strand.ToString());
			}
			return atomSetCollection;
		}
		
		
		internal static bool isMatch(System.String str1, System.String str2)
		{
			int cch = str1.Length;
			if (str2.Length != cch)
				return false;
			for (int i = cch; --i >= 0; )
			{
				char ch1 = str1[i];
				char ch2 = str2[i];
				if (ch1 == ch2)
					continue;
				if ((ch1 == '_' || ch1 == '.') && (ch2 == '_' || ch2 == '.'))
					continue;
				if (ch1 <= 'Z' && ch1 >= 'A')
					ch1 += 'a' - 'A';
				else if (ch2 <= 'Z' && ch2 >= 'A')
					ch2 += 'a' - 'A';
				if (ch1 != ch2)
					return false;
			}
			return true;
		}
		
		
		internal virtual void  processDataParameter()
		{
			System.String collectionName = line.Substring(5).Trim();
			if (collectionName.Length > 0)
				atomSetCollection.collectionName = collectionName;
		}
		
		internal virtual void  processSymmetrySpaceGroupNameHM()
		{
			atomSetCollection.spaceGroup = line.Substring(29).Trim();
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'cellParamNames'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] cellParamNames = new System.String[]{"_cell_length_a", "_cell_length_b", "_cell_length_c", "_cell_angle_alpha", "_cell_angle_beta", "_cell_angle_gamma"};
		
		internal virtual void  processCellParameter()
		{
			//    logger.log("processCellParameter() line:" + line);
			System.String cellParameter = parseToken(line);
			for (int i = cellParamNames.Length; --i >= 0; )
				if (isMatch(cellParameter, cellParamNames[i]))
				{
					notionalUnitcell[i] = parseFloat(line, ichNextParse);
					//        logger.log("value=" + notionalUnitcell[i]);
					return ;
				}
			//    logger.log("NOT");
		}
		
		internal virtual void  checkUnitcell()
		{
			for (int i = 6; --i >= 0; )
			{
				if (System.Single.IsNaN(notionalUnitcell[i]))
					return ;
			}
			atomSetCollection.notionalUnitcell = notionalUnitcell;
		}
		
		private void  processLoopBlock()
		{
			//    logger.log("processLoopBlock()-------------------------");
			line = reader.ReadLine().Trim();
			//    logger.log("trimmed line:" + line);
			if (line.StartsWith("_atom_site"))
			{
				processAtomSiteLoopBlock();
				return ;
			}
			if (line.StartsWith("_struct_asym"))
			{
				processStructAsymBlock();
				return ;
			}
			if (line.StartsWith("_geom_bond"))
			{
				processGeomBondLoopBlock();
				return ;
			}
			if (line.StartsWith("_struct_conf") && !line.StartsWith("_struct_conf_type"))
			{
				processStructConfLoopBlock();
				return ;
			}
			if (line.StartsWith("_pdbx_poly_seq_scheme"))
			{
				processPolySeqSchemeLoopBlock();
				return ;
			}
			if (line.StartsWith("_pdbx_nonpoly_scheme"))
			{
				processNonPolySchemeLoopBlock();
				return ;
			}
			if (line.StartsWith("_struct_sheet_range"))
			{
				processStructSheetRangeLoopBlock();
				return ;
			}
			//    logger.log("Skipping loop block:" + line);
			skipLoopHeaders();
			skipLoopData();
		}
		
		private void  skipLoopHeaders()
		{
			// skip everything that begins with _
			while (line != null && (line = line.Trim()).Length > 0 && line[0] == '_')
			{
				line = reader.ReadLine();
			}
		}
		
		private void  skipLoopData()
		{
			// skip everything until empty line, or comment line
			// or start of a new loop_ or data_
			char ch;
			while (line != null && (line = line.Trim()).Length > 0 && (ch = line[0]) != '_' && ch != '#' && !line.StartsWith("loop_") && !line.StartsWith("data_"))
			{
				//      logger.log("skipLoopData just discarded:" + line);
				line = reader.ReadLine();
			}
		}
		
		////////////////////////////////////////////////////////////////
		// atom data
		////////////////////////////////////////////////////////////////
		
		internal const sbyte NONE = 0;
		internal const sbyte TYPE_SYMBOL = 1;
		internal const sbyte LABEL = 2;
		internal const sbyte FRACT_X = 3;
		internal const sbyte FRACT_Y = 4;
		internal const sbyte FRACT_Z = 5;
		internal const sbyte CARTN_X = 6;
		internal const sbyte CARTN_Y = 7;
		internal const sbyte CARTN_Z = 8;
		internal const sbyte OCCUPANCY = 9;
		internal const sbyte B_ISO = 10;
		internal const sbyte COMP_ID = 11;
		internal const sbyte ASYM_ID = 12;
		internal const sbyte SEQ_ID = 13;
		internal const sbyte INS_CODE = 14;
		internal const sbyte ALT_ID = 15;
		internal const sbyte GROUP_PDB = 16;
		internal const sbyte SITE_ID = 17;
		internal const sbyte MODEL_NO = 18;
		internal const sbyte ATOM_PROPERTY_MAX = 19;
		
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'atomFields'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] atomFields = new System.String[]{"_atom_site_type_symbol", "_atom_site_label", "_atom_site_label_atom_id", "_atom_site_fract_x", "_atom_site_fract_y", "_atom_site_fract_z", "_atom_site.Cartn_x", "_atom_site.Cartn_y", "_atom_site.Cartn_z", "_atom_site_occupancy", "_atom_site.b_iso_or_equiv", "_atom_site.label_comp_id", "_atom_site.label_asym_id", "_atom_site.label_seq_id", "_atom_site.pdbx_PDB_ins_code", "_atom_site.label_alt_id", "_atom_site.group_PDB", "_atom_site.id", "_atom_site.pdbx_PDB_model_num"};
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'atomFieldMap '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly sbyte[] atomFieldMap = new sbyte[]{TYPE_SYMBOL, LABEL, LABEL, FRACT_X, FRACT_Y, FRACT_Z, CARTN_X, CARTN_Y, CARTN_Z, OCCUPANCY, B_ISO, COMP_ID, ASYM_ID, SEQ_ID, INS_CODE, ALT_ID, GROUP_PDB, SITE_ID, MODEL_NO};
		
		internal virtual void  processAtomSiteLoopBlock()
		{
			//    logger.log("processAtomSiteLoopBlock()-------------------------");
			int currentModelNO = - 1;
			int missingSequenceNumber = 0;
			int atomSerial = 0;
			int[] fieldTypes = new int[100]; // should be enough
			bool[] atomPropertyReferenced = new bool[ATOM_PROPERTY_MAX];
			int fieldCount = parseLoopParameters(atomFields, atomFieldMap, fieldTypes, atomPropertyReferenced);
			// now that headers are parsed, check to see if we want
			// cartesian or fractional coordinates;
			if (atomPropertyReferenced[CARTN_X])
			{
				for (int i = FRACT_X; i < FRACT_Z; ++i)
					disableField(fieldCount, fieldTypes, i);
			}
			else if (atomPropertyReferenced[FRACT_X])
			{
				atomSetCollection.coordinatesAreFractional = true;
				for (int i = CARTN_X; i < CARTN_Z; ++i)
					disableField(fieldCount, fieldTypes, i);
			}
			else
			{
				// it is a different kind of _atom_site loop block
				//      logger.log("?que? no atom coordinates found");
				skipLoopData();
				return ;
			}
			
			for (; line != null; line = reader.ReadLine())
			{
				int lineLength = line.Length;
				if (lineLength == 0)
					break;
				char chFirst = line[0];
				if (chFirst == '#' || chFirst == '_' || line.StartsWith("loop_") || line.StartsWith("data_"))
					break;
				if (chFirst == ' ')
				{
					int i;
					for (i = lineLength; --i >= 0 && line[i] == ' '; )
					{
					}
					if (i < 0)
						break;
				}
				//      logger.log("line:" + line);
				//      logger.log("of length = " + line.length());
				if (line.Length == 1)
					logger.log("char value is " + (chFirst + 0));
				tokenizer.String = line;
				//      logger.log("reading an atom");
				Atom atom = new Atom();
				for (int i = 0; i < fieldCount; ++i)
				{
					if (!tokenizer.hasMoreTokens())
						tokenizer.String = reader.ReadLine();
					System.String field = tokenizer.nextToken();
					if (field == null)
						logger.log("field == null!");
					switch (fieldTypes[i])
					{
						
						case NONE: 
							break;
						
						case TYPE_SYMBOL: 
							System.String elementSymbol;
							if (field.Length < 2)
							{
								elementSymbol = field;
							}
							else
							{
								char ch0 = field[0];
								char ch1 = System.Char.ToLower(field[1]);
								if (Atom.isValidElementSymbol(ch0, ch1))
									elementSymbol = "" + ch0 + ch1;
								else
									elementSymbol = "" + ch0;
							}
							atom.elementSymbol = elementSymbol;
							break;
						
						case LABEL: 
							atom.atomName = field;
							break;
						
						case CARTN_X: 
						case FRACT_X: 
							atom.x = parseFloat(field);
							break;
						
						case CARTN_Y: 
						case FRACT_Y: 
							atom.y = parseFloat(field);
							break;
						
						case CARTN_Z: 
						case FRACT_Z: 
							atom.z = parseFloat(field);
							break;
						
						case OCCUPANCY: 
							float floatOccupancy = parseFloat(field);
							if (!System.Single.IsNaN(floatOccupancy))
							{
								//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
								atom.occupancy = (int) (floatOccupancy * 100);
							}
							break;
						
						case B_ISO: 
							atom.bfactor = parseFloat(field);
							break;
						
						case COMP_ID: 
							atom.group3 = field;
							break;
						
						case ASYM_ID: 
							if (field.Length > 1)
								logger.log("Don't know how to deal with chains more than 1 char", field);
							char firstChar = field[0];
							if (firstChar != '?' && firstChar != '.')
							{
								System.String chainID = "" + firstChar;
								if (strandsMap.Contains(chainID))
								{
									Strand strand = (Strand) strandsMap[chainID];
									// OK, here is the if/else construct that Wayne send me
									if (strand.isBlank)
									{
										atom.chainID = '0'; // no author provided ID
									}
									else if (strand.authorID != null)
									{
										// not pretty, but let's just assume the string only has one char
										atom.chainID = strand.authorID[0];
									}
									else
									{
										atom.chainID = '0';
									}
								}
								else
								{
									atom.chainID = firstChar;
								}
							}
							break;
						
						case SEQ_ID: 
							atom.sequenceNumber = parseInt(field);
							if (atom.sequenceNumber == System.Int32.MinValue)
							{
								logger.log("Warning! mmCIF ERROR: Missing SEQ_ID in mmCIF file for #" + atomSerial + " group3=" + atom.group3);
								atom.sequenceNumber = --missingSequenceNumber;
							}
							/*
							* 1d66.cif is missing this information, causing Jmol to 
							* improperly assign "CD" to HOH as group3 in HETATM records.
							*  
							*  interestingly, this fix allows for 
							*  
							*  select -3
							*  
							*  but I wouldn't publicize that. 
							*  
							* -- Bob Hanson  206/04/14
							* 
							*/
							break;
						
						case INS_CODE: 
							char insCode = field[0];
							if (insCode != '?' && insCode != '.')
								atom.chainID = insCode;
							break;
						
						case ALT_ID: 
							char alternateLocationID = field[0];
							if (alternateLocationID != '?' && alternateLocationID != '.')
								atom.alternateLocationID = alternateLocationID;
							break;
						
						case GROUP_PDB: 
							if ("HETATM".Equals(field))
								atom.isHetero = true;
							break;
						
						case SITE_ID: 
							//atom.atomSerial = parseInt(field);
							/*
							* I considered the above, but then decided there might be
							* a reason we aren't assigning a serial number for atoms in
							* a CIF file, maybe to do with the fact that in CIF files we
							* are using mapped atom names, whereas in PDB files we are not
							* Egon? 
							* 
							* So this assignment for now is just for internal purposes.
							* 
							* -- Bob Hanson
							* 
							*/
							atomSerial = parseInt(field);
							break;
						
						case MODEL_NO: 
							int modelNO = parseInt(field);
							if (modelNO != currentModelNO)
							{
								atomSetCollection.newAtomSet();
								currentModelNO = modelNO;
							}
							break;
						}
				}
				if (System.Single.IsNaN(atom.x) || System.Single.IsNaN(atom.y) || System.Single.IsNaN(atom.z))
					logger.log("atom " + atom.atomName + " has invalid/unknown coordinates");
				else
					atomSetCollection.addAtomWithMappedName(atom);
			}
		}
		
		internal virtual void  disableField(int fieldCount, int[] fieldTypes, int fieldIndex)
		{
			for (int i = fieldCount; --i >= 0; )
				if (fieldTypes[i] == fieldIndex)
					fieldTypes[i] = 0;
		}
		
		internal virtual int parseLoopParameters(System.String[] fields, sbyte[] fieldMap, int[] fieldTypes, bool[] propertyReferenced)
		{
			int fieldCount = 0;
			//UPGRADE_NOTE: Label 'outer_loop' was moved. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1014'"
			for (; line != null && (line = line.Trim()).Length > 0 && line[0] == '_'; ++fieldCount, line = reader.ReadLine())
			{
				for (int i = fields.Length; --i >= 0; )
					if (isMatch(line, fields[i]))
					{
						int iproperty = fieldMap[i];
						propertyReferenced[iproperty] = true;
						fieldTypes[fieldCount] = iproperty;
						//UPGRADE_NOTE: Labeled continue statement was changed to a goto statement. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1015'"
						goto outer_loop;
					}
				//UPGRADE_NOTE: Label 'outer_loop' was moved. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1014'"
outer_loop: ;
			}
			//    logger.log("parseLoopParameters sees fieldCount="+ fieldCount);
			return fieldCount;
		}
		
		internal virtual System.Object[] parseAndProcessStructParameters(System.String[] fields, sbyte[] fieldMap, int[] fieldTypes, bool[] propertyReferenced)
		{
			int fieldCount = 0;
			Strand struct_Renamed = new Strand();
			for (; line != null && (line = line.Trim()).Length > 0 && line[0] == '_'; ++fieldCount, line = reader.ReadLine())
			{
				logger.log("Processing line: ", line);
				for (int i = fields.Length; --i >= 0; )
				{
					tokenizer.String = line;
					System.String key = tokenizer.nextToken();
					// take this valid mmCIF into account:
					// _struct_asym.id A 
					System.String value_Renamed = null;
					if (isMatch(key, fields[i]))
					{
						if (tokenizer.hasMoreTokens())
						{
							value_Renamed = tokenizer.nextToken();
							if (fields[i].Equals(structAsymNames[STRUCT_ASYM_ID - 1]))
								struct_Renamed.chainID = value_Renamed;
							if (fields[i].Equals(structAsymNames[STRUCT_BLANK_CHAIN_ID - 1]))
								struct_Renamed.isBlank = "Y".Equals(value_Renamed)?true:false;
							logger.log("struct", struct_Renamed);
						}
						else
						{
							struct_Renamed = null;
						}
						int iproperty = fieldMap[i];
						propertyReferenced[iproperty] = true;
						fieldTypes[fieldCount] = iproperty;
						i = - 1;
					}
				}
			}
			logger.log("parseLoopParameters sees fieldCount=" + fieldCount);
			logger.log("parsed struct -> " + struct_Renamed);
			System.Object[] results = new System.Object[2];
			results[0] = (System.Int32) fieldCount;
			results[1] = struct_Renamed;
			return results;
		}
		
		////////////////////////////////////////////////////////////////
		// bond data
		////////////////////////////////////////////////////////////////
		
		internal const sbyte GEOM_BOND_ATOM_SITE_LABEL_1 = 1;
		internal const sbyte GEOM_BOND_ATOM_SITE_LABEL_2 = 2;
		internal const sbyte GEOM_BOND_SITE_SYMMETRY_2 = 3;
		//  final static byte GEOM_BOND_DISTANCE          = 4;
		
		internal const sbyte GEOM_BOND_PROPERTY_MAX = 4;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'geomBondFields'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] geomBondFields = new System.String[]{"_geom_bond_atom_site_label_1", "_geom_bond_atom_site_label_2", "_geom_bond_site_symmetry_2"};
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'geomBondFieldMap '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly sbyte[] geomBondFieldMap = new sbyte[]{GEOM_BOND_ATOM_SITE_LABEL_1, GEOM_BOND_ATOM_SITE_LABEL_2, GEOM_BOND_SITE_SYMMETRY_2};
		
		internal virtual void  processGeomBondLoopBlock()
		{
			int[] fieldTypes = new int[100]; // should be enough
			bool[] propertyReferenced = new bool[GEOM_BOND_PROPERTY_MAX];
			int fieldCount = parseLoopParameters(geomBondFields, geomBondFieldMap, fieldTypes, propertyReferenced);
			for (int i = GEOM_BOND_PROPERTY_MAX; --i > 0; )
			// only > 0, not >= 0
				if (!propertyReferenced[i])
				{
					logger.log("?que? missing _geom_bond property:" + i);
					skipLoopData();
					return ;
				}
			
			for (; line != null && (line = line.Trim()).Length > 0 && line[0] != '#' && line[0] != '_' && !line.StartsWith("loop_") && !line.StartsWith("data_"); line = reader.ReadLine())
			{
				tokenizer.String = line;
				int atomIndex1 = - 1;
				int atomIndex2 = - 1;
				System.String symmetry = null;
				for (int i = 0; i < fieldCount; ++i)
				{
					if (!tokenizer.hasMoreTokens())
						tokenizer.String = reader.ReadLine();
					System.String field = tokenizer.nextToken();
					switch (fieldTypes[i])
					{
						
						case NONE: 
							break;
						
						case GEOM_BOND_ATOM_SITE_LABEL_1: 
							atomIndex1 = atomSetCollection.getAtomNameIndex(field);
							break;
						
						case GEOM_BOND_ATOM_SITE_LABEL_2: 
							atomIndex2 = atomSetCollection.getAtomNameIndex(field);
							break;
						
						case GEOM_BOND_SITE_SYMMETRY_2: 
							if (field[0] != '.')
								symmetry = field;
							break;
						}
				}
				if (atomIndex1 >= 0 && atomIndex2 >= 0)
				{
					// miguel 2004 11 19
					// for now, do not deal with symmetry
					if (symmetry == null)
					{
						Bond bond = new Bond();
						bond.atomIndex1 = atomIndex1;
						bond.atomIndex2 = atomIndex2;
						atomSetCollection.addBond(bond);
					}
				}
			}
		}
		
		////////////////////////////////////////////////////////////////
		// helix and turn structure data
		////////////////////////////////////////////////////////////////
		
		internal const sbyte CONF_TYPE_ID = 1;
		internal const sbyte BEG_ASYM_ID = 2;
		internal const sbyte BEG_SEQ_ID = 3;
		internal const sbyte BEG_INS_CODE = 4;
		internal const sbyte END_ASYM_ID = 5;
		internal const sbyte END_SEQ_ID = 6;
		internal const sbyte END_INS_CODE = 7;
		internal const sbyte STRUCT_CONF_PROPERTY_MAX = 8;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'structConfFields'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] structConfFields = new System.String[]{"_struct_conf.conf_type_id", "_struct_conf.beg_auth_asym_id", "_struct_conf.beg_auth_seq_id", "_struct_conf.pdbx_beg_PDB_ins_code", "_struct_conf.end_auth_asym_id", "_struct_conf.end_auth_seq_id", "_struct_conf.pdbx_end_PDB_ins_code"};
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'structConfFieldMap '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly sbyte[] structConfFieldMap = new sbyte[]{CONF_TYPE_ID, BEG_ASYM_ID, BEG_SEQ_ID, BEG_INS_CODE, END_ASYM_ID, END_SEQ_ID, END_INS_CODE};
		
		internal virtual void  processStructConfLoopBlock()
		{
			int[] fieldTypes = new int[100]; // should be enough
			bool[] propertyReferenced = new bool[STRUCT_CONF_PROPERTY_MAX];
			int fieldCount = parseLoopParameters(structConfFields, structConfFieldMap, fieldTypes, propertyReferenced);
			for (int i = STRUCT_CONF_PROPERTY_MAX; --i > 0; )
			// only > 0, not >= 0
				if (!propertyReferenced[i])
				{
					logger.log("?que? missing _struct_conf property:" + i);
					skipLoopData();
					return ;
				}
			
			for (; line != null && (line = line.Trim()).Length > 0 && line[0] != '#'; line = reader.ReadLine())
			{
				tokenizer.String = line;
				Structure structure = new Structure();
				
				for (int i = 0; i < fieldCount; ++i)
				{
					if (!tokenizer.hasMoreTokens())
						tokenizer.String = reader.ReadLine();
					System.String field = tokenizer.nextToken();
					char firstChar = field[0];
					switch (fieldTypes[i])
					{
						
						case NONE: 
							break;
						
						case CONF_TYPE_ID: 
							if (field.StartsWith("HELX"))
								structure.structureType = "helix";
							else if (field.StartsWith("TURN"))
								structure.structureType = "turn";
							else
								structure.structureType = "none";
							break;
						
						case BEG_ASYM_ID: 
							structure.startChainID = (firstChar == '.' || firstChar == '?')?' ':firstChar;
							break;
						
						case BEG_SEQ_ID: 
							structure.startSequenceNumber = parseInt(field);
							break;
						
						case BEG_INS_CODE: 
							structure.startInsertionCode = (firstChar == '.' || firstChar == '?')?' ':firstChar;
							break;
						
						case END_ASYM_ID: 
							structure.endChainID = (firstChar == '.' || firstChar == '?')?' ':firstChar;
							break;
						
						case END_SEQ_ID: 
							structure.endSequenceNumber = parseInt(field);
							break;
						
						case END_INS_CODE: 
							structure.endInsertionCode = (firstChar == '.' || firstChar == '?')?' ':firstChar;
							break;
						}
				}
				atomSetCollection.addStructure(structure);
			}
		}
		
		////////////////////////////////////////////////////////////////
		// sheet structure data
		////////////////////////////////////////////////////////////////
		
		// note that the conf_id is not used
		internal const sbyte STRUCT_SHEET_RANGE_PROPERTY_MAX = 8;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'structSheetRangeFields'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] structSheetRangeFields = new System.String[]{"_struct_sheet_range.beg_label_asym_id", "_struct_sheet_range.beg_label_seq_id", "_struct_sheet_range.pdbx_beg_PDB_ins_code", "_struct_sheet_range.end_label_asym_id", "_struct_sheet_range.end_label_seq_id", "_struct_sheet_range.pdbx_end_PDB_ins_code"};
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'structSheetRangeFieldMap '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly sbyte[] structSheetRangeFieldMap = new sbyte[]{BEG_ASYM_ID, BEG_SEQ_ID, BEG_INS_CODE, END_ASYM_ID, END_SEQ_ID, END_INS_CODE};
		
		internal virtual void  processStructSheetRangeLoopBlock()
		{
			int[] fieldTypes = new int[100]; // should be enough
			bool[] propertyReferenced = new bool[STRUCT_SHEET_RANGE_PROPERTY_MAX];
			int fieldCount = parseLoopParameters(structSheetRangeFields, structSheetRangeFieldMap, fieldTypes, propertyReferenced);
			for (int i = STRUCT_SHEET_RANGE_PROPERTY_MAX; --i > 1; )
				if (!propertyReferenced[i])
				{
					logger.log("?que? missing _struct_conf property:" + i);
					skipLoopData();
					return ;
				}
			
			for (; line != null && (line = line.Trim()).Length > 0 && line[0] != '#'; line = reader.ReadLine())
			{
				tokenizer.String = line;
				Structure structure = new Structure();
				structure.structureType = "sheet";
				
				for (int i = 0; i < fieldCount; ++i)
				{
					if (!tokenizer.hasMoreTokens())
						tokenizer.String = reader.ReadLine();
					System.String field = tokenizer.nextToken();
					char firstChar = field[0];
					switch (fieldTypes[i])
					{
						
						case BEG_ASYM_ID: 
							structure.startChainID = (firstChar == '.' || firstChar == '?')?' ':firstChar;
							break;
						
						case BEG_SEQ_ID: 
							structure.startSequenceNumber = parseInt(field);
							break;
						
						case BEG_INS_CODE: 
							structure.startInsertionCode = (firstChar == '.' || firstChar == '?')?' ':firstChar;
							break;
						
						case END_ASYM_ID: 
							structure.endChainID = (firstChar == '.' || firstChar == '?')?' ':firstChar;
							break;
						
						case END_SEQ_ID: 
							structure.endSequenceNumber = parseInt(field);
							break;
						
						case END_INS_CODE: 
							structure.endInsertionCode = (firstChar == '.' || firstChar == '?')?' ':firstChar;
							break;
						}
				}
				atomSetCollection.addStructure(structure);
			}
		}
		
		////////////////////////////////////////////////////////////////
		// strand data
		////////////////////////////////////////////////////////////////
		
		internal const sbyte STRUCT_ASYM_ID = 1;
		internal const sbyte STRUCT_BLANK_CHAIN_ID = 2;
		internal const sbyte STRUCT_PROPERTY_MAX = 3;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'structAsymNames'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] structAsymNames = new System.String[]{"_struct_asym.id", "_struct_asym.pdbx_blank_PDB_chainid_flag"};
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'structAsymMap '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly sbyte[] structAsymMap = new sbyte[]{STRUCT_ASYM_ID, STRUCT_BLANK_CHAIN_ID};
		
		private void  addOrUpdateStrandInfo(Strand strand)
		{
			logger.log("Found only one chain", strand.chainID);
			if (strandsMap.Contains(strand.chainID))
			{
				Strand prevStrand = (Strand) strandsMap[strand.chainID];
				if (strand.authorID != null)
					prevStrand.authorID = strand.authorID;
				//UPGRADE_TODO: The 'System.Boolean' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
				if (prevStrand.isBlank != null)
					prevStrand.isBlank = strand.isBlank;
			}
			else
			{
				strandsMap[strand.chainID] = strand;
			}
		}
		
		private void  processStructAsymBlock()
		{
			//    logger.log("processAtomSiteLoopBlock()-------------------------");
			int[] fieldTypes = new int[10]; // should be enough
			bool[] structPropertyReferenced = new bool[STRUCT_PROPERTY_MAX];
			System.Object[] results = parseAndProcessStructParameters(structAsymNames, structAsymMap, fieldTypes, structPropertyReferenced);
			int fieldCount = ((System.Int32) results[0]);
			Strand struct_Renamed = ((Strand) results[1]);
			// now that headers are parsed, check to see if we want
			// cartesian or fractional coordinates;
			if (struct_Renamed != null)
			{
				/* Then this syntax was found:
				* _struct_asym.id                            A 
				* _struct_asym.pdbx_blank_PDB_chainid_flag   Y 
				* _struct_asym.pdbx_modified                 N 
				* _struct_asym.entity_id                     1 
				* _struct_asym.details                       ? 
				* #
				*/
				addOrUpdateStrandInfo(struct_Renamed);
			}
			else
			{
				/* Then the 'normal' _loop format was found. */
				logger.log("Found multiple chains... parsing them now");
				for (; line != null; line = reader.ReadLine())
				{
					int lineLength = line.Length;
					if (lineLength == 0)
						break;
					char chFirst = line[0];
					if (chFirst == '#' || chFirst == '_' || line.StartsWith("loop_") || line.StartsWith("data_"))
						break;
					if (chFirst == ' ')
					{
						int i;
						for (i = lineLength; --i >= 0 && line[i] == ' '; )
						{
						}
						if (i < 0)
							break;
					}
					//      logger.log("line:" + line);
					//      logger.log("of length = " + line.length());
					if (line.Length == 1)
						logger.log("char value is " + (chFirst + 0));
					tokenizer.String = line;
					//      logger.log("reading an atom");
					struct_Renamed = new Strand();
					for (int i = 0; i < fieldCount; ++i)
					{
						if (!tokenizer.hasMoreTokens())
							tokenizer.String = reader.ReadLine();
						System.String field = tokenizer.nextToken();
						if (field == null)
							logger.log("field == null!");
						switch (fieldTypes[i])
						{
							
							case STRUCT_ASYM_ID: 
								struct_Renamed.chainID = field;
								break;
							
							case STRUCT_BLANK_CHAIN_ID: 
								struct_Renamed.isBlank = "Y".Equals(field)?true:false;
								break;
							}
					}
					addOrUpdateStrandInfo(struct_Renamed);
				}
			}
		}
		
		////////////////////////////////////////////////////////////////
		// poly sequence scheme data
		////////////////////////////////////////////////////////////////
		
		internal const sbyte POLYSEQ_ASYM_ID = 1;
		internal const sbyte POLYSEQ_PDB_STRAND_ID = 2;
		internal const sbyte POLYSEQ_PROPERTY_MAX = 3;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'polySeqFields'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] polySeqFields = new System.String[]{"_pdbx_poly_seq_scheme.asym_id", "_pdbx_poly_seq_scheme.pdb_strand_id"};
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'polySeqMap '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly sbyte[] polySeqMap = new sbyte[]{POLYSEQ_ASYM_ID, POLYSEQ_PDB_STRAND_ID};
		
		private void  processPolySeqSchemeLoopBlock()
		{
			int[] fieldTypes = new int[100]; // should be enough
			bool[] propertyReferenced = new bool[POLYSEQ_PROPERTY_MAX];
			int fieldCount = parseLoopParameters(polySeqFields, polySeqMap, fieldTypes, propertyReferenced);
			for (int i = POLYSEQ_PROPERTY_MAX; --i > 1; )
				if (!propertyReferenced[i])
				{
					logger.log("?que? missing _pdbx_poly_seq_scheme property:" + i);
					skipLoopData();
					return ;
				}
			
			System.String lastChainID = "XXXXX";
			for (; line != null && (line = line.Trim()).Length > 0 && line[0] != '#'; line = reader.ReadLine())
			{
				tokenizer.String = line;
				Strand strand = new Strand();
				for (int i = 0; i < fieldCount; ++i)
				{
					if (!tokenizer.hasMoreTokens())
						tokenizer.String = reader.ReadLine();
					System.String field = tokenizer.nextToken();
					switch (fieldTypes[i])
					{
						
						case POLYSEQ_ASYM_ID: 
							strand.chainID = field;
							break;
						
						case POLYSEQ_PDB_STRAND_ID: 
							if (field[0] == '?')
							{
								strand.authorID = null;
							}
							else
							{
								strand.authorID = field;
							}
							break;
						}
				}
				// OK, we're only interested in the chain IDs
				// hence we only parse the first AA in a chain
				if (lastChainID.Equals(strand.chainID))
				{
					// skip
				}
				else
				{
					addOrUpdateStrandInfo(strand);
					lastChainID = strand.chainID;
				}
			}
			logger.log("Done reading _loop Poly Seq Scheme");
			System.Collections.IEnumerator strands = strandsMap.Values.GetEnumerator();
			//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
			while (strands.MoveNext())
			{
				//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
				Strand strand = (Strand) strands.Current;
				logger.log(strand.ToString());
			}
		}
		
		////////////////////////////////////////////////////////////////
		// non poly scheme data
		////////////////////////////////////////////////////////////////
		
		internal const sbyte NONPOLY_ASYM_ID = 1;
		internal const sbyte NONPOLY_PDB_STRAND_ID = 2;
		internal const sbyte NONPOLY_PROPERTY_MAX = 3;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'nonPolyFields'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] nonPolyFields = new System.String[]{"_pdbx_nonpoly_scheme.asym_id", "_pdbx_nonpoly_scheme.pdb_strand_id"};
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'nonPolyMap '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly sbyte[] nonPolyMap = new sbyte[]{POLYSEQ_ASYM_ID, POLYSEQ_PDB_STRAND_ID};
		
		private void  processNonPolySchemeLoopBlock()
		{
			int[] fieldTypes = new int[100]; // should be enough
			bool[] propertyReferenced = new bool[NONPOLY_PROPERTY_MAX];
			int fieldCount = parseLoopParameters(nonPolyFields, polySeqMap, fieldTypes, propertyReferenced);
			for (int i = NONPOLY_PROPERTY_MAX; --i > 1; )
				if (!propertyReferenced[i])
				{
					logger.log("?que? missing _pdbx_nonpoly_scheme property:" + i);
					skipLoopData();
					return ;
				}
			
			System.String lastChainID = "XXXXX";
			for (; line != null && (line = line.Trim()).Length > 0 && line[0] != '#'; line = reader.ReadLine())
			{
				tokenizer.String = line;
				Strand strand = new Strand();
				for (int i = 0; i < fieldCount; ++i)
				{
					if (!tokenizer.hasMoreTokens())
						tokenizer.String = reader.ReadLine();
					System.String field = tokenizer.nextToken();
					switch (fieldTypes[i])
					{
						
						case NONPOLY_ASYM_ID: 
							strand.chainID = field;
							break;
						
						case NONPOLY_PDB_STRAND_ID: 
							if (field[0] == '?')
							{
								strand.authorID = null;
							}
							else
							{
								strand.authorID = field;
							}
							break;
						}
				}
				// OK, we're only interested in the chain IDs
				// hence we only parse the first AA in a chain
				if (lastChainID.Equals(strand.chainID))
				{
					// skip
				}
				else
				{
					addOrUpdateStrandInfo(strand);
					lastChainID = strand.chainID;
				}
			}
			logger.log("Done reading _loop NonPoly Scheme");
			System.Collections.IEnumerator strands = strandsMap.Values.GetEnumerator();
			//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
			while (strands.MoveNext())
			{
				//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
				Strand strand = (Strand) strands.Current;
				logger.log(strand.ToString());
			}
		}
		
		
		////////////////////////////////////////////////////////////////
		// special tokenizer class
		////////////////////////////////////////////////////////////////
		
		/// <summary> A special tokenizer class for dealing with quoted strings in CIF files.
		/// <p>
		/// regarding the treatment of single quotes vs. primes in
		/// cif file, PMR wrote:
		/// </p>
		/// <p>
		/// * There is a formal grammar for CIF
		/// (see http://www.iucr.org/iucr-top/cif/index.html)
		/// which confirms this. The textual explanation is
		/// <p />
		/// <p>
		/// 14. Matching single or double quote characters (' or ") may
		/// be used to bound a string representing a non-simple data value
		/// provided the string does not extend over more than one line.
		/// <p />
		/// <p>
		/// 15. Because data values are invariably separated from other
		/// tokens in the file by white space, such a quote-delimited
		/// character string may contain instances of the character used
		/// to delimit the string provided they are not followed by white
		/// space. For example, the data item
		/// <code>
		/// _example  'a dog's life'
		/// </code>
		/// is legal; the data value is a dog's life.
		/// </p>
		/// <p>
		/// [PMR - the terminating character(s) are quote+whitespace.
		/// That would mean that:
		/// <code>
		/// _example 'Jones' life'
		/// </code>
		/// would be an error
		/// </p>
		/// <p>
		/// The CIF format was developed in that late 1980's under the aegis of the
		/// International Union of Crystallography (I am a consultant to the COMCIFs 
		/// committee). It was ratified by the Union and there have been several 
		/// workshops. mmCIF is an extension of CIF which includes a relational 
		/// structure. The formal publications are:
		/// </p>
		/// <p>
		/// Hall, S. R. (1991). "The STAR File: A New Format for Electronic Data 
		/// Transfer and Archiving", J. Chem. Inform. Comp. Sci., 31, 326-333.
		/// Hall, S. R., Allen, F. H. and Brown, I. D. (1991). "The Crystallographic
		/// Information File (CIF): A New Standard Archive File for Crystallography",
		/// Acta Cryst., A47, 655-685.
		/// Hall, S.R. & Spadaccini, N. (1994). "The STAR File: Detailed 
		/// Specifications," J. Chem. Info. Comp. Sci., 34, 505-508.
		/// </p>
		/// </summary>
		
		internal class RidiculousFileFormatTokenizer
		{
			virtual internal System.String String
			{
				set
				{
					if (value == null)
						value = "";
					this.str = value;
					cch = value.Length;
					ich = 0;
				}
				
			}
			internal System.String str;
			internal int ich;
			internal int cch;
			
			internal virtual bool hasMoreTokens()
			{
				char ch;
				while (ich < cch && ((ch = str[ich]) == ' ' || ch == '\t'))
					++ich;
				return ich < cch;
			}
			
			/* assume that hasMoreTokens() has been called and that
			* ich is pointing at a non-white character
			*/
			internal virtual System.String nextToken()
			{
				if (ich == cch)
					return null;
				int ichStart = ich;
				char ch = str[ichStart];
				if (ch != '\'' && ch != '"')
				{
					while (ich < cch && (ch = str[ich]) != ' ' && ch != '\t')
						++ich;
					return str.Substring(ichStart, (ich) - (ichStart));
				}
				char chOpeningQuote = ch;
				bool previousCharacterWasQuote = false;
				while (++ich < cch)
				{
					ch = str[ich];
					if (previousCharacterWasQuote && (ch == ' ' || ch == '\t'))
						break;
					previousCharacterWasQuote = (ch == chOpeningQuote);
				}
				if (ich == cch)
				{
					if (previousCharacterWasQuote)
					// close quote was last char of string
						return str.Substring(ichStart + 1, (ich - 1) - (ichStart + 1));
					// reached the end of the string without finding closing '
					return str.Substring(ichStart, (ich) - (ichStart));
				}
				++ich; // throw away the last white character
				return str.Substring(ichStart + 1, (ich - 2) - (ichStart + 1));
			}
		}
		static CifReader()
		{
			{
				if (atomFieldMap.Length != atomFields.Length)
					atomFields[100] = "explode";
			}
			{
				if (structAsymMap.Length != structAsymNames.Length)
					structAsymNames[100] = "explode";
			}
			{
				if (polySeqMap.Length != polySeqFields.Length)
					polySeqFields[100] = "explode";
			}
			{
				if (nonPolyMap.Length != nonPolyFields.Length)
					nonPolyFields[100] = "explode";
			}
		}
	}
}