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
namespace org.jmol.adapter.smarter
{
	
	class AtomSetCollection
	{
		virtual internal int FirstAtomSetAtomCount
		{
			get
			{
				return atomSetAtomCounts[0];
			}
			
		}
		virtual internal int LastAtomSetAtomCount
		{
			get
			{
				return atomSetAtomCounts[currentAtomSetIndex];
			}
			
		}
		virtual internal int LastAtomSetAtomIndex
		{
			get
			{
				//    System.out.println("atomSetCount=" + atomSetCount);
				return atomCount - atomSetAtomCounts[currentAtomSetIndex];
			}
			
		}
		virtual internal System.String CollectionName
		{
			set
			{
				if (value != null)
				{
					value = value.Trim();
					if (value.Length > 0)
						this.collectionName = value;
				}
			}
			
		}
		internal System.String fileTypeName;
		internal System.String collectionName;
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		//UPGRADE_TODO: Format of property file may need to be changed. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1089'"
		internal System.Collections.Specialized.NameValueCollection atomSetCollectionProperties = new System.Collections.Specialized.NameValueCollection();
		internal System.Collections.Hashtable atomSetCollectionAuxiliaryInfo = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'notionalUnitcellTags'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] notionalUnitcellTags = new System.String[]{"a", "b", "c", "alpha", "beta", "gamma"};
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'dictRefUnitcellTags'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] dictRefUnitcellTags = new System.String[]{"cif:_cell_length_a", "cif:_cell_length_b", "cif:cell_length_c", "cif:_cell_length_alpha", "cif:_cell_length_beta", "cif:_cell_length_gamma"};
		
		internal int atomCount;
		internal Atom[] atoms = new Atom[256];
		internal int bondCount;
		internal Bond[] bonds = new Bond[256];
		internal int structureCount;
		internal Structure[] structures = new Structure[16];
		
		internal int atomSetCount;
		internal int currentAtomSetIndex = - 1;
		internal int[] atomSetNumbers = new int[16];
		internal System.String[] atomSetNames = new System.String[16];
		internal int[] atomSetAtomCounts = new int[16];
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		internal System.Collections.Specialized.NameValueCollection[] atomSetProperties = new System.Collections.Specialized.NameValueCollection[16];
		internal System.Collections.Hashtable[] atomSetAuxiliaryInfo = new System.Collections.Hashtable[16];
		
		internal System.String errorMessage;
		
		internal System.String spaceGroup;
		internal float wavelength = System.Single.NaN;
		internal bool coordinatesAreFractional;
		internal float[] notionalUnitcell;
		internal float[] pdbScaleMatrix;
		internal float[] pdbScaleTranslate;
		
		internal System.String[] pdbStructureRecords;
		
		internal AtomSetCollection(System.String fileTypeName)
		{
			this.fileTypeName = fileTypeName;
			// set the default PATH properties as defined in the SmarterJmolAdapter
			atomSetCollectionProperties[(System.String) "PATH_KEY"] = (System.String) SmarterJmolAdapter.PATH_KEY;
			atomSetCollectionProperties[(System.String) "PATH_SEPARATOR"] = (System.String) SmarterJmolAdapter.PATH_SEPARATOR;
		}
		
		/// <summary> Creates an AtomSetCollection based on an array of AtomSetCollection
		/// 
		/// </summary>
		/// <param name="array">Array of AtomSetCollection
		/// </param>
		internal AtomSetCollection(AtomSetCollection[] array):this("Array")
		{
			for (int i = 0; i < array.Length; i++)
			{
				appendAtomSetCollection(array[i]);
			}
		}
		
		/// <summary> Appends an AtomSetCollection
		/// 
		/// </summary>
		/// <param name="collection">AtomSetCollection to append
		/// </param>
		protected internal virtual void  appendAtomSetCollection(AtomSetCollection collection)
		{
			// Initialisations
			int existingAtomsCount = atomCount;
			
			// Clone each AtomSet
			int clonedAtoms = 0;
			for (int atomSetNum = 0; atomSetNum < collection.atomSetCount; atomSetNum++)
			{
				newAtomSet();
				setAtomSetName(collection.getAtomSetName(atomSetNum));
				//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
				System.Collections.Specialized.NameValueCollection properties = collection.getAtomSetProperties(atomSetNum);
				if (properties != null)
				{
					System.Collections.IEnumerator props = properties.Keys.GetEnumerator();
					//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
					while ((props != null) && (props.MoveNext()))
					{
						//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
						System.String key = (System.String) props.Current;
						setAtomSetProperty(key, properties.Get(key));
					}
				}
				for (int atomNum = 0; atomNum < collection.atomSetAtomCounts[atomSetNum]; atomNum++)
				{
					newCloneAtom(collection.atoms[clonedAtoms]);
					clonedAtoms++;
				}
			}
			
			// Clone bonds
			for (int bondNum = 0; bondNum < collection.bondCount; bondNum++)
			{
				Bond bond = collection.bonds[bondNum];
				addNewBond(bond.atomIndex1 + existingAtomsCount, bond.atomIndex2 + existingAtomsCount, bond.order);
			}
		}
		
		~AtomSetCollection()
		{
			//    System.out.println("Model.finalize() called");
			try
			{
				//UPGRADE_NOTE: Call to 'super.finalize()' was removed. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1124'"
			}
			//UPGRADE_NOTE: Exception 'java.lang.Throwable' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
			catch (System.Exception t)
			{
			}
		}
		
		internal virtual void  finish()
		{
			atoms = null;
			bonds = null;
			notionalUnitcell = pdbScaleMatrix = pdbScaleTranslate = null;
			pdbStructureRecords = null;
		}
		
		internal virtual void  freeze()
		{
			System.Console.Out.WriteLine("AtomSetCollection.freeze; atomCount = " + atomCount);
			if (hasAlternateLocations())
				hackAlternateLocationDamage();
		}
		
		internal virtual void  discardPreviousAtoms()
		{
			for (int i = atomCount; --i >= 0; )
				atoms[i] = null;
			atomCount = 0;
			atomSymbolicMap.Clear();
			atomSetCount = 0;
			currentAtomSetIndex = - 1;
			for (int i = atomSetNumbers.Length; --i >= 0; )
			{
				atomSetNumbers[i] = atomSetAtomCounts[i] = 0;
				atomSetNames[i] = null;
			}
		}
		
		internal virtual Atom newCloneAtom(Atom atom)
		{
			//    System.out.println("newCloneAtom()");
			Atom clone = atom.cloneAtom();
			addAtom(clone);
			return clone;
		}
		
		// FIX ME This should really also clone the other things pertaining
		// to an atomSet, like the bonds (which probably should be remade...)
		// but also the atomSetProperties and atomSetName...
		internal virtual void  cloneFirstAtomSet()
		{
			newAtomSet();
			for (int i = 0, firstCount = atomSetAtomCounts[0]; i < firstCount; ++i)
				newCloneAtom(atoms[i]);
		}
		
		internal virtual void  cloneFirstAtomSetWithBonds(int nBonds)
		{
			/*
			*  see note above; same deal here? This is for CsfReader, where
			*  there are bonds indicated in the file, but we still need to 
			*  clone in the case of vibrational vectors. 
			*
			*  Bob Hanson 2006/4/14
			*  
			*/
			
			cloneFirstAtomSet();
			int firstCount = atomSetAtomCounts[0];
			for (int bondNum = 0; bondNum < nBonds; bondNum++)
			{
				Bond bond = bonds[bondCount - nBonds];
				addNewBond(bond.atomIndex1 + firstCount, bond.atomIndex2 + firstCount, bond.order);
			}
		}
		
		internal virtual void  cloneLastAtomSet()
		{
			//    System.out.println("cloneLastAtomSet");
			//    System.out.println("b4 atomCount=" + atomCount);
			//    System.out.println("atomSetCount=" + atomSetCount);
			//    System.out.println("atomSetAtomCount=" +
			//                       atomSetAtomCounts[currentAtomSetIndex]);
			int count = LastAtomSetAtomCount;
			int atomIndex = LastAtomSetAtomIndex;
			newAtomSet();
			for (; --count >= 0; ++atomIndex)
				newCloneAtom(atoms[atomIndex]);
			//    System.out.println("after atomCount=" + atomCount);
		}
		
		internal virtual Atom addNewAtom()
		{
			Atom atom = new Atom();
			addAtom(atom);
			return atom;
		}
		
		internal virtual void  addAtom(Atom atom)
		{
			if (atomCount == atoms.Length)
				atoms = (Atom[]) AtomSetCollectionReader.doubleLength(atoms);
			atoms[atomCount++] = atom;
			if (atomSetCount == 0)
			{
				atomSetCount = 1;
				currentAtomSetIndex = 0;
				atomSetNumbers[0] = 1;
			}
			atom.atomSetIndex = currentAtomSetIndex;
			++atomSetAtomCounts[currentAtomSetIndex];
			/*
			System.out.println("addAtom ... after" +
			"\natomCount=" + atomCount +
			"\natomSetCount=" + atomSetCount +
			"\natomSetAtomCounts[" + (currentAtomSetIndex) + "]=" +
			atomSetAtomCounts[atomSetIndex]);
			*/
		}
		
		internal virtual void  addAtomWithMappedName(Atom atom)
		{
			addAtom(atom);
			mapMostRecentAtomName();
		}
		
		internal virtual void  addAtomWithMappedSerialNumber(Atom atom)
		{
			addAtom(atom);
			mapMostRecentAtomSerialNumber();
		}
		
		internal virtual Bond addNewBond(int atomIndex1, int atomIndex2)
		{
			return addNewBond(atomIndex1, atomIndex2, 1);
		}
		
		internal virtual Bond addNewBond(System.String atomName1, System.String atomName2)
		{
			return addNewBond(atomName1, atomName2, 1);
		}
		
		internal virtual Bond addNewBond(int atomIndex1, int atomIndex2, int order)
		{
			if (atomIndex1 < 0 || atomIndex1 >= atomCount || atomIndex2 < 0 || atomIndex2 >= atomCount)
				return null;
			Bond bond = new Bond(atomIndex1, atomIndex2, order);
			addBond(bond);
			return bond;
		}
		
		internal virtual Bond addNewBond(System.String atomName1, System.String atomName2, int order)
		{
			return addNewBond(getAtomNameIndex(atomName1), getAtomNameIndex(atomName2), order);
		}
		
		internal virtual Bond addNewBondWithMappedSerialNumbers(int atomSerial1, int atomSerial2, int order)
		{
			return addNewBond(getAtomSerialNumberIndex(atomSerial1), getAtomSerialNumberIndex(atomSerial2), order);
		}
		
		internal virtual void  addBond(Bond bond)
		{
			/*
			System.out.println("I see a bond:" + bond.atomIndex1 + "-" +
			bond.atomIndex2 + ":" + bond.order);
			*/
			if (bond.atomIndex1 < 0 || bond.atomIndex2 < 0 || bond.order <= 0)
			{
				/*
				System.out.println(">>>>>>BAD BOND:" + bond.atomIndex1 + "-" +
				bond.atomIndex2 + ":" + bond.order);
				*/
				return ;
			}
			if (bondCount == bonds.Length)
				bonds = (Bond[]) AtomSetCollectionReader.setLength(bonds, bondCount + 1024);
			bonds[bondCount++] = bond;
		}
		
		internal virtual void  addStructure(Structure structure)
		{
			if (structureCount == structures.Length)
				structures = (Structure[]) AtomSetCollectionReader.setLength(structures, structureCount + 32);
			structures[structureCount++] = structure;
		}
		
		internal System.Collections.Hashtable atomSymbolicMap = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
		
		internal virtual void  mapMostRecentAtomName()
		{
			if (atomCount > 0)
			{
				int index = atomCount - 1;
				System.String atomName = atoms[index].atomName;
				if (atomName != null)
					atomSymbolicMap[atomName] = (System.Int32) index;
			}
		}
		
		internal virtual void  mapMostRecentAtomSerialNumber()
		{
			if (atomCount > 0)
			{
				int index = atomCount - 1;
				int atomSerial = atoms[index].atomSerial;
				if (atomSerial != System.Int32.MinValue)
					atomSymbolicMap[(System.Int32) atomSerial] = (System.Int32) index;
			}
		}
		
		internal virtual void  mapAtomName(System.String atomName, int atomIndex)
		{
			atomSymbolicMap[atomName] = (System.Int32) atomIndex;
		}
		
		internal virtual int getAtomNameIndex(System.String atomName)
		{
			int index = - 1;
			System.Object value_Renamed = atomSymbolicMap[atomName];
			if (value_Renamed != null)
				index = ((System.Int32) value_Renamed);
			return index;
		}
		
		internal virtual int getAtomSerialNumberIndex(int serialNumber)
		{
			int index = - 1;
			System.Object value_Renamed = atomSymbolicMap[(System.Int32) serialNumber];
			if (value_Renamed != null)
				index = ((System.Int32) value_Renamed);
			return index;
		}
		
		/// <summary> Sets a property for the AtomSetCollection</summary>
		/// <param name="key">The poperty key.
		/// </param>
		/// <param name="value">The property value.
		/// </param>
		internal virtual void  setAtomSetCollectionProperty(System.String key, System.String value_Renamed)
		{
			atomSetCollectionProperties[(System.String) key] = (System.String) value_Renamed;
		}
		
		internal virtual System.String getAtomSetCollectionProperty(System.String key)
		{
			return (System.String) atomSetCollectionProperties[(System.String) key];
		}
		
		internal virtual void  setAtomSetCollectionAuxiliaryInfo(System.String key, System.Object value_Renamed)
		{
			atomSetCollectionAuxiliaryInfo[key] = value_Renamed;
		}
		
		internal virtual System.Object getAtomSetCollectionAuxiliaryInfo(System.String key)
		{
			return atomSetCollectionAuxiliaryInfo[key];
		}
		
		////////////////////////////////////////////////////////////////
		// atomSet stuff
		////////////////////////////////////////////////////////////////
		
		internal virtual void  newAtomSet()
		{
			currentAtomSetIndex = atomSetCount++;
			if (atomSetCount > atomSetNumbers.Length)
			{
				atomSetNumbers = AtomSetCollectionReader.doubleLength(atomSetNumbers);
				atomSetNames = AtomSetCollectionReader.doubleLength(atomSetNames);
				atomSetAtomCounts = AtomSetCollectionReader.doubleLength(atomSetAtomCounts);
				//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
				atomSetProperties = (System.Collections.Specialized.NameValueCollection[]) AtomSetCollectionReader.doubleLength(atomSetProperties);
				atomSetAuxiliaryInfo = (System.Collections.Hashtable[]) AtomSetCollectionReader.doubleLength(atomSetAuxiliaryInfo);
			}
			atomSetNumbers[currentAtomSetIndex] = atomSetCount;
			// miguel 2006 03 22
			// added this clearing of the atomSymbolicMap to support V3000
			// seems that it should have been here all along, but apparently
			// noone else needed it
			atomSymbolicMap.Clear();
		}
		
		/// <summary> Sets the name for the current AtomSet
		/// 
		/// </summary>
		/// <param name="atomSetName">The name to be associated with the current AtomSet
		/// </param>
		internal virtual void  setAtomSetName(System.String atomSetName)
		{
			atomSetNames[currentAtomSetIndex] = atomSetName;
		}
		
		/// <summary> Sets the name for an AtomSet
		/// 
		/// </summary>
		/// <param name="atomSetName">The number to be associated with the AtomSet
		/// </param>
		/// <param name="atomSetIndex">The index of the AtomSet that needs the association
		/// </param>
		internal virtual void  setAtomSetName(System.String atomSetName, int atomSetIndex)
		{
			atomSetNames[atomSetIndex] = atomSetName;
		}
		
		/// <summary> Sets the atom set names of the last n atomSets</summary>
		/// <param name="atomSetName">The name
		/// </param>
		/// <param name="n">The number of last AtomSets that need these set
		/// </param>
		internal virtual void  setAtomSetNames(System.String atomSetName, int n)
		{
			for (int idx = currentAtomSetIndex; --n >= 0; --idx)
				setAtomSetName(atomSetName, idx);
		}
		
		/// <summary> Sets the number for the current AtomSet
		/// 
		/// </summary>
		/// <param name="atomSetNumber">The number for the current AtomSet.
		/// </param>
		internal virtual void  setAtomSetNumber(int atomSetNumber)
		{
			atomSetNumbers[currentAtomSetIndex] = atomSetNumber;
		}
		
		/// <summary> Sets a property for the AtomSet
		/// 
		/// </summary>
		/// <param name="key">The key for the property
		/// </param>
		/// <param name="value">The value to be associated with the key
		/// </param>
		internal virtual void  setAtomSetProperty(System.String key, System.String value_Renamed)
		{
			setAtomSetProperty(key, value_Renamed, currentAtomSetIndex);
		}
		
		
		/// <summary> Sets auxiliary information for the AtomSet
		/// 
		/// </summary>
		/// <param name="key">The key for the property
		/// </param>
		/// <param name="value">The value to be associated with the key
		/// </param>
		internal virtual void  setAtomSetAuxiliaryInfo(System.String key, System.Object value_Renamed)
		{
			setAtomSetAuxiliaryInfo(key, value_Renamed, currentAtomSetIndex);
		}
		
		/// <summary> Sets the a property for the an AtomSet
		/// 
		/// </summary>
		/// <param name="key">The key for the property
		/// </param>
		/// <param name="value">The value for the property
		/// </param>
		/// <param name="atomSetIndex">The index of the AtomSet to get the property
		/// </param>
		internal virtual void  setAtomSetProperty(System.String key, System.String value_Renamed, int atomSetIndex)
		{
			// lazy instantiation of the Properties object
			if (atomSetProperties[atomSetIndex] == null)
			{
				//UPGRADE_TODO: Format of property file may need to be changed. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1089'"
				//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
				atomSetProperties[atomSetIndex] = new System.Collections.Specialized.NameValueCollection();
			}
			atomSetProperties[atomSetIndex][(System.String) key] = (System.String) value_Renamed;
		}
		
		/// <summary> Sets auxiliary information for the an AtomSet
		/// 
		/// </summary>
		/// <param name="key">The key for the property
		/// </param>
		/// <param name="value">The value for the property
		/// </param>
		/// <param name="atomSetIndex">The index of the AtomSet to get the property
		/// </param>
		internal virtual void  setAtomSetAuxiliaryInfo(System.String key, System.Object value_Renamed, int atomSetIndex)
		{
			// lazy instantiation of the Properties object
			if (atomSetAuxiliaryInfo[atomSetIndex] == null)
				atomSetAuxiliaryInfo[atomSetIndex] = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
			atomSetAuxiliaryInfo[atomSetIndex][key] = value_Renamed;
		}
		
		/// <summary> Sets the same properties for the last n atomSets.</summary>
		/// <param name="key">The key for the property
		/// </param>
		/// <param name="value">The value of the property
		/// </param>
		/// <param name="n">The number of last AtomSets that need these set
		/// </param>
		internal virtual void  setAtomSetProperties(System.String key, System.String value_Renamed, int n)
		{
			for (int idx = currentAtomSetIndex; --n >= 0; --idx)
			{
				setAtomSetProperty(key, value_Renamed, idx);
			}
		}
		
		
		/// <summary> Clones the properties of the last atom set and associates it
		/// with the current atom set. 
		/// </summary>
		internal virtual void  cloneLastAtomSetProperties()
		{
			cloneAtomSetProperties(currentAtomSetIndex - 1);
		}
		
		/// <summary> Clones the properties of an atom set and associated it with the
		/// current atom set.
		/// </summary>
		/// <param name="index">The index of the atom set whose properties are to be cloned.
		/// </param>
		internal virtual void  cloneAtomSetProperties(int index)
		{
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			atomSetProperties[currentAtomSetIndex] = (System.Collections.Specialized.NameValueCollection) atomSetProperties[index].Clone();
		}
		/*
		// currently not needed because we take the atomSetCount directly
		int getAtomSetCount() {
		return atomSetCount;
		}*/
		
		internal virtual int getAtomSetNumber(int atomSetIndex)
		{
			return atomSetNumbers[atomSetIndex];
		}
		
		internal virtual System.String getAtomSetName(int atomSetIndex)
		{
			return atomSetNames[atomSetIndex];
		}
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		internal virtual System.Collections.Specialized.NameValueCollection getAtomSetProperties(int atomSetIndex)
		{
			return atomSetProperties[atomSetIndex];
		}
		
		internal virtual System.Collections.Hashtable getAtomSetAuxiliaryInfo(int atomSetIndex)
		{
			return atomSetAuxiliaryInfo[atomSetIndex];
		}
		
		////////////////////////////////////////////////////////////////
		// special support for alternate locations
		////////////////////////////////////////////////////////////////
		
		internal virtual bool hasAlternateLocations()
		{
			for (int i = atomCount; --i >= 0; )
				if (atoms[i].alternateLocationID != '\x0000')
					return true;
			return false;
		}
		
		internal virtual void  hackAlternateLocationDamage()
		{
			System.Console.Out.WriteLine("hacking alternate location damage");
		}
	}
}