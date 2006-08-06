/* $RCSfile$
* $Author: egonw $
* $Date: 2006-03-18 15:59:33 -0600 (Sat, 18 Mar 2006) $
* $Revision: 4652 $
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
	
	/// <summary> CSF file reader based on CIF idea -- fluid property fields.
	/// 
	/// note that, like CIF, the order of fields is totally unpredictable
	/// in addition, ID numbers are not sequential, requiring atomNames
	/// 
	/// first crack at this 2006/04/13
	/// 
	/// </summary>
	/// <author>  hansonr <hansonr@stolaf.edu>
	/// </author>
	class CsfReader:AtomSetCollectionReader
	{
		
		internal System.IO.StreamReader reader;
		internal System.String line;
		internal int nAtoms = 0;
		
		internal override AtomSetCollection readAtomSetCollection(System.IO.StreamReader reader)
		{
			this.reader = reader;
			atomSetCollection = new AtomSetCollection("csf");
			
			line = reader.ReadLine();
			while (line != null)
			{
				if (line.StartsWith("object_class"))
				{
					processObjectClass();
					// there is already an unprocessed line in the firing chamber
					continue;
				}
				line = reader.ReadLine();
			}
			return atomSetCollection;
		}
		private void  processObjectClass()
		{
			if (line.Equals("object_class connector"))
			{
				processConnectorObject();
				return ;
			}
			if (line.Equals("object_class atom"))
			{
				processAtomObject();
				return ;
			}
			if (line.Equals("object_class bond"))
			{
				processBondObject();
				return ;
			}
			if (line.Equals("object_class vibrational_level"))
			{
				processVibrationObject();
				return ;
			}
			line = reader.ReadLine();
		}
		
		internal virtual void  skipTo(System.String startsWith)
		{
			while ((line = reader.ReadLine()) != null && line.IndexOf(startsWith) != 0)
			{
			}
		}
		
		internal virtual int parseLineParameters(System.String[] fields, sbyte[] fieldMap, int[] fieldTypes, bool[] propertyReferenced)
		{
			System.String[] tokens = getTokens(line);
			System.String field;
			int fieldCount = - 1;
			for (int ipt = tokens.Length; --ipt >= 0; )
			{
				field = tokens[ipt];
				for (int i = fields.Length; --i >= 0; )
					if (field.Equals(fields[i]))
					{
						int iproperty = fieldMap[i];
						propertyReferenced[iproperty] = true;
						fieldTypes[ipt] = iproperty;
						if (fieldCount == - 1)
							fieldCount = ipt + 1;
						break;
					}
			}
			return fieldCount;
		}
		
		////////////////////////////////////////////////////////////////
		// connector data
		////////////////////////////////////////////////////////////////
		
		internal const sbyte conID = 1;
		internal const sbyte objCls1 = 2;
		internal const sbyte objID1 = 3;
		internal const sbyte objCls2 = 4;
		internal const sbyte objID2 = 5;
		
		internal const sbyte CONNECTOR_PROPERTY_MAX = 6;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'connectorFields'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] connectorFields = new System.String[]{"ID", "objCls1", "objID1", "objCls2", "objID2"};
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'connectorFieldMap '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly sbyte[] connectorFieldMap = new sbyte[]{conID, objCls1, objID1, objCls2, objID2};
		
		internal System.Collections.Hashtable connectors = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
		
		internal virtual void  processConnectorObject()
		{
			int[] fieldTypes = new int[100]; // should be enough
			bool[] propertyReferenced = new bool[CONNECTOR_PROPERTY_MAX];
			skipTo("ID");
			int fieldCount = parseLineParameters(connectorFields, connectorFieldMap, fieldTypes, propertyReferenced);
			//UPGRADE_NOTE: Label 'out' was moved. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1014'"
			for (; (line = reader.ReadLine()) != null; )
			{
				if (line.StartsWith("property_flags:"))
					break;
				System.String thisAtomID = null;
				System.String thisBondID = null;
				System.String[] tokens = getTokens(line);
				for (int i = 0; i < fieldCount; ++i)
				{
					System.String field = tokens[i];
					switch (fieldTypes[i])
					{
						
						case NONE: 
						case conID: 
							break;
						
						case objCls1: 
							if (!field.Equals("atom"))
							{
								//UPGRADE_NOTE: Labeled continue statement was changed to a goto statement. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1015'"
								goto out;
							}
							break;
						
						case objCls2: 
							if (!field.Equals("bond"))
							{
								//UPGRADE_NOTE: Labeled continue statement was changed to a goto statement. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1015'"
								goto out;
							}
							break;
						
						case objID1: 
							thisAtomID = "Atom" + field;
							break;
						
						case objID2: 
							thisBondID = "Bond" + field;
							break;
						
						default: 
							break;
						
					}
				}
				if (thisAtomID != null && thisBondID != null)
				{
					if (connectors.ContainsKey(thisBondID))
					{
						System.String[] connect = (System.String[]) connectors[thisBondID];
						connect[1] = thisAtomID;
						connectors[thisBondID] = connect;
					}
					else
					{
						System.String[] connect = new System.String[2];
						connect[0] = thisAtomID;
						connectors[thisBondID] = connect;
					}
				}
				//UPGRADE_NOTE: Label 'out' was moved. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1014'"
out: ;
			}
		}
		
		////////////////////////////////////////////////////////////////
		// atom data
		////////////////////////////////////////////////////////////////
		
		internal const sbyte NONE = 0;
		internal const sbyte atomID = 1;
		internal const sbyte sym = 2;
		internal const sbyte anum = 3;
		internal const sbyte chrg = 4;
		internal const sbyte xyz_coordinates = 5;
		internal const sbyte ATOM_PROPERTY_MAX = 6;
		
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'atomFields'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] atomFields = new System.String[]{"ID", "sym", "anum", "chrg", "xyz_coordinates"};
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'atomFieldMap '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly sbyte[] atomFieldMap = new sbyte[]{atomID, sym, anum, chrg, xyz_coordinates};
		
		internal virtual void  processAtomObject()
		{
			nAtoms = 0;
			int[] fieldTypes = new int[100]; // should be enough
			bool[] atomPropertyReferenced = new bool[ATOM_PROPERTY_MAX];
			skipTo("ID");
			int fieldCount = parseLineParameters(atomFields, atomFieldMap, fieldTypes, atomPropertyReferenced);
			for (; (line = reader.ReadLine()) != null; )
			{
				if (line.StartsWith("property_flags:"))
					break;
				System.String[] tokens = getTokens(line);
				Atom atom = new Atom();
				for (int i = 0; i < fieldCount; i++)
				{
					System.String field = tokens[i];
					if (field == null)
						System.Console.Out.WriteLine("field == null!");
					switch (fieldTypes[i])
					{
						
						case NONE: 
							break;
						
						case atomID: 
							atom.atomName = "Atom" + field;
							break;
						
						case sym: 
							atom.elementSymbol = field;
							break;
						
						case anum: 
							break;
						
						case xyz_coordinates: 
							atom.x = parseFloat(field);
							field = tokens[i + 1];
							atom.y = parseFloat(field);
							field = tokens[i + 2];
							atom.z = parseFloat(field);
							break;
						}
				}
				if (System.Single.IsNaN(atom.x) || System.Single.IsNaN(atom.y) || System.Single.IsNaN(atom.z))
					logger.log("atom " + atom.atomName + " has invalid/unknown coordinates");
				else
				{
					nAtoms++;
					atomSetCollection.addAtomWithMappedName(atom);
				}
			}
		}
		
		////////////////////////////////////////////////////////////////
		// bond order data
		////////////////////////////////////////////////////////////////
		
		internal const sbyte bondID = 1;
		internal const sbyte bondType = 2;
		internal const sbyte BOND_PROPERTY_MAX = 3;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'bondFields'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] bondFields = new System.String[]{"ID", "type"};
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'bondFieldMap '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly sbyte[] bondFieldMap = new sbyte[]{bondID, bondType};
		
		internal int nBonds = 0;
		
		internal virtual void  processBondObject()
		{
			int[] fieldTypes = new int[100]; // should be enough
			bool[] propertyReferenced = new bool[BOND_PROPERTY_MAX];
			skipTo("ID");
			int fieldCount = parseLineParameters(bondFields, bondFieldMap, fieldTypes, propertyReferenced);
			for (; (line = reader.ReadLine()) != null; )
			{
				if (line.StartsWith("property_flags:"))
					break;
				System.String thisBondID = null;
				System.String[] tokens = getTokens(line);
				for (int i = 0; i < fieldCount; ++i)
				{
					System.String field = tokens[i];
					switch (fieldTypes[i])
					{
						
						case NONE: 
							break;
						
						case bondID: 
							thisBondID = "Bond" + field;
							break;
						
						case bondType: 
							int order = 1;
							if (field.Equals("single"))
								order = 1;
							else if (field.Equals("double"))
								order = 2;
							else if (field.Equals("triple"))
								order = 3;
							else
								System.Console.Out.WriteLine("unknown CSF bond order: " + field);
							System.String[] connect = (System.String[]) connectors[thisBondID];
							Bond bond = new Bond();
							bond.atomIndex1 = atomSetCollection.getAtomNameIndex(connect[0]);
							bond.atomIndex2 = atomSetCollection.getAtomNameIndex(connect[1]);
							bond.order = order;
							atomSetCollection.addBond(bond);
							nBonds++;
							break;
						}
				}
			}
		}
		internal const sbyte vibID = 1;
		internal const sbyte normalMode = 2;
		internal const sbyte vibEnergy = 3;
		internal const sbyte transitionDipole = 4;
		internal const sbyte lineWidth = 5;
		internal const sbyte VIB_PROPERTY_MAX = 6;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'vibFields'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] vibFields = new System.String[]{"ID", "normalMode", "Energy", "transitionDipole"};
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'vibFieldMap '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly sbyte[] vibFieldMap = new sbyte[]{vibID, normalMode, vibEnergy, transitionDipole};
		
		internal virtual void  processVibrationObject()
		{
			int[] fieldTypes = new int[100]; // should be enough
			bool[] propertyReferenced = new bool[VIB_PROPERTY_MAX];
			skipTo("ID normalMode"); //a bit risky -- could miss it
			int thisvibID = - 1;
			float[] vibXYZ = new float[3];
			int iatom = atomSetCollection.FirstAtomSetAtomCount;
			int xyzpt = 0;
			Atom[] atoms = atomSetCollection.atoms;
out1: 
			for (; (line = reader.ReadLine()) != null; )
			{
				if (line.StartsWith("property_flags:"))
					break;
				System.String[] tokens = getTokens(line);
				System.Console.Out.WriteLine(tokens.Length + "LINE " + line);
				if (parseInt(tokens[0]) != thisvibID)
				{
					System.Console.Out.WriteLine("cloning" + thisvibID + " " + tokens[0]);
					thisvibID = parseInt(tokens[0]);
					atomSetCollection.cloneFirstAtomSetWithBonds(nBonds);
				}
				for (int i = 1; i < tokens.Length; ++i)
				{
					vibXYZ[xyzpt++] = parseFloat(tokens[i]);
					if (xyzpt == 3)
					{
						atoms[iatom].addVibrationVector(vibXYZ[0], vibXYZ[1], vibXYZ[2]);
						iatom++;
						xyzpt = 0;
					}
				}
			}
			
			skipTo("ID"); //second part
			int fieldCount = parseLineParameters(vibFields, vibFieldMap, fieldTypes, propertyReferenced);
			for (; (line = reader.ReadLine()) != null; )
			{
				if (line.StartsWith("property_flags:"))
					break;
				System.String[] tokens = getTokens(line);
				int thisvib = - 1;
				for (int i = 0; i < fieldCount; ++i)
				{
					System.String field = tokens[i];
					switch (fieldTypes[i])
					{
						
						case NONE: 
							break;
						
						case vibID: 
							thisvib = parseInt(field);
							break;
						
						case vibEnergy: 
							atomSetCollection.setAtomSetName(field + " cm^-1", thisvib);
							atomSetCollection.setAtomSetProperty(SmarterJmolAdapter.PATH_KEY, "Frequencies");
							break;
						}
				}
			}
		}
		static CsfReader()
		{
			{
				if (atomFieldMap.Length != atomFields.Length)
					atomFields[100] = "explode";
			}
		}
	}
}