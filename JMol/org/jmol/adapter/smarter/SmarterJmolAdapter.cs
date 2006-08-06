/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-04-03 03:17:20 +0200 (lun., 03 avr. 2006) $
* $Revision: 4885 $
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
	
	public class SmarterJmolAdapter:JmolAdapter
	{
		
		public SmarterJmolAdapter(Logger logger):base("SmarterJmolAdapter", logger)
		{
		}
		
		/* **************************************************************
		* the file related methods
		* **************************************************************/
		
		public const System.String PATH_KEY = ".PATH";
		//UPGRADE_NOTE: Final was removed from the declaration of 'PATH_SEPARATOR '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_TODO: Method 'java.lang.System.getProperty' was converted to 'System.IO.Path.PathSeparator.ToString' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangSystemgetProperty_javalangString'"
		public static readonly System.String PATH_SEPARATOR = System.IO.Path.PathSeparator.ToString();
		
		
		
		public override void  finish(System.Object clientFile)
		{
			((AtomSetCollection) clientFile).finish();
		}
		
		public override System.Object openBufferedReader(System.String name, System.IO.StreamReader bufferedReader)
		{
			try
			{
				System.Object atomSetCollectionOrErrorMessage = Resolver.resolve(name, bufferedReader, logger);
				if (atomSetCollectionOrErrorMessage is System.String)
					return atomSetCollectionOrErrorMessage;
				if (atomSetCollectionOrErrorMessage is AtomSetCollection)
				{
					AtomSetCollection atomSetCollection = (AtomSetCollection) atomSetCollectionOrErrorMessage;
					if (atomSetCollection.errorMessage != null)
						return atomSetCollection.errorMessage;
					return atomSetCollection;
				}
				return "unknown reader error";
			}
			catch (System.Exception e)
			{
				SupportClass.WriteStackTrace(e, Console.Error);
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				return "" + e;
			}
		}
		
		public override System.Object openBufferedReaders(System.String[] name, System.IO.StreamReader[] bufferedReader)
		{
			int size = System.Math.Min(name.Length, bufferedReader.Length);
			AtomSetCollection[] atomSetCollections = new AtomSetCollection[size];
			for (int i = 0; i < size; i++)
			{
				try
				{
					System.Object atomSetCollectionOrErrorMessage = Resolver.resolve(name[i], bufferedReader[i], logger);
					if (atomSetCollectionOrErrorMessage is System.String)
						return atomSetCollectionOrErrorMessage;
					if (atomSetCollectionOrErrorMessage is AtomSetCollection)
					{
						atomSetCollections[i] = (AtomSetCollection) atomSetCollectionOrErrorMessage;
						if (atomSetCollections[i].errorMessage != null)
							return atomSetCollections[i].errorMessage;
					}
					else
					{
						return "unknown reader error";
					}
				}
				catch (System.Exception e)
				{
					SupportClass.WriteStackTrace(e, Console.Error);
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					return "" + e;
				}
			}
			AtomSetCollection result = new AtomSetCollection(atomSetCollections);
			if (result.errorMessage != null)
			{
				return result.errorMessage;
			}
			return result;
		}
		
		public override System.Object openDOMReader(System.Object DOMNode)
		{
			try
			{
				System.Object atomSetCollectionOrErrorMessage = Resolver.DOMResolve(DOMNode, logger);
				if (atomSetCollectionOrErrorMessage is System.String)
					return atomSetCollectionOrErrorMessage;
				if (atomSetCollectionOrErrorMessage is AtomSetCollection)
				{
					AtomSetCollection atomSetCollection = (AtomSetCollection) atomSetCollectionOrErrorMessage;
					if (atomSetCollection.errorMessage != null)
						return atomSetCollection.errorMessage;
					return atomSetCollection;
				}
				return "unknown DOM reader error";
			}
			catch (System.Exception e)
			{
				SupportClass.WriteStackTrace(e, Console.Error);
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				return "" + e;
			}
		}
		
		public override System.String getFileTypeName(System.Object clientFile)
		{
			return ((AtomSetCollection) clientFile).fileTypeName;
		}
		
		public override System.String getAtomSetCollectionName(System.Object clientFile)
		{
			return ((AtomSetCollection) clientFile).collectionName;
		}
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		public override System.Collections.Specialized.NameValueCollection getAtomSetCollectionProperties(System.Object clientFile)
		{
			return ((AtomSetCollection) clientFile).atomSetCollectionProperties;
		}
		
		public virtual System.Collections.Hashtable getAtomSetCollectionAuxiliaryInfo(System.Object clientFile)
		{
			return ((AtomSetCollection) clientFile).atomSetCollectionAuxiliaryInfo;
		}
		
		public override int getAtomSetCount(System.Object clientFile)
		{
			return ((AtomSetCollection) clientFile).atomSetCount;
		}
		
		public override int getAtomSetNumber(System.Object clientFile, int atomSetIndex)
		{
			return ((AtomSetCollection) clientFile).getAtomSetNumber(atomSetIndex);
		}
		
		public override System.String getAtomSetName(System.Object clientFile, int atomSetIndex)
		{
			return ((AtomSetCollection) clientFile).getAtomSetName(atomSetIndex);
		}
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		public override System.Collections.Specialized.NameValueCollection getAtomSetProperties(System.Object clientFile, int atomSetIndex)
		{
			return ((AtomSetCollection) clientFile).getAtomSetProperties(atomSetIndex);
		}
		
		public virtual System.Collections.Hashtable getAtomSetAuxiliaryInfo(System.Object clientFile, int atomSetIndex)
		{
			return ((AtomSetCollection) clientFile).getAtomSetAuxiliaryInfo(atomSetIndex);
		}
		
		
		/* **************************************************************
		* The frame related methods
		* **************************************************************/
		
		public override int getEstimatedAtomCount(System.Object clientFile)
		{
			return ((AtomSetCollection) clientFile).atomCount;
		}
		
		public override bool coordinatesAreFractional(System.Object clientFile)
		{
			return ((AtomSetCollection) clientFile).coordinatesAreFractional;
		}
		
		public override float[] getNotionalUnitcell(System.Object clientFile)
		{
			return ((AtomSetCollection) clientFile).notionalUnitcell;
		}
		
		public override float[] getPdbScaleMatrix(System.Object clientFile)
		{
			return ((AtomSetCollection) clientFile).pdbScaleMatrix;
		}
		
		public override float[] getPdbScaleTranslate(System.Object clientFile)
		{
			return ((AtomSetCollection) clientFile).pdbScaleTranslate;
		}
		
		/*
		// not redefined for the smarterJmolAdapter, but we probably 
		// should do something similar like that. This would required
		// us to add a Properties to the Atom, I guess...
		public String getClientAtomStringProperty(Object clientAtom,
		String propertyName) {
		return null;
		
		"Property" is not the right class for this; numeric data are involved. RMH 
		}*/
		
		////////////////////////////////////////////////////////////////
		
		public override JmolAdapter.AtomIterator getAtomIterator(System.Object clientFile)
		{
			return new AtomIterator(this, (AtomSetCollection) clientFile);
		}
		
		public override JmolAdapter.BondIterator getBondIterator(System.Object clientFile)
		{
			return new BondIterator(this, (AtomSetCollection) clientFile);
		}
		
		public override JmolAdapter.StructureIterator getStructureIterator(System.Object clientFile)
		{
			AtomSetCollection atomSetCollection = (AtomSetCollection) clientFile;
			return atomSetCollection.structureCount == 0?null:new StructureIterator(this, atomSetCollection);
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AtomIterator' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		/* **************************************************************
		* the frame iterators
		* **************************************************************/
		new internal class AtomIterator:JmolAdapter.AtomIterator
		{
			private void  InitBlock(SmarterJmolAdapter enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private SmarterJmolAdapter enclosingInstance;
			virtual public int AtomSetIndex
			{
				get
				{
					return atom.atomSetIndex;
				}
				
			}
			virtual public System.Object UniqueID
			{
				get
				{
					return atom;
				}
				
			}
			virtual public System.String ElementSymbol
			{
				get
				{
					if (atom.elementSymbol != null)
						return atom.elementSymbol;
					return atom.ElementSymbol;
				}
				
			}
			virtual public int ElementNumber
			{
				get
				{
					return atom.elementNumber;
				}
				
			}
			virtual public System.String AtomName
			{
				get
				{
					return atom.atomName;
				}
				
			}
			virtual public int FormalCharge
			{
				get
				{
					return atom.formalCharge;
				}
				
			}
			virtual public float PartialCharge
			{
				get
				{
					return atom.partialCharge;
				}
				
			}
			virtual public float X
			{
				get
				{
					return atom.x;
				}
				
			}
			virtual public float Y
			{
				get
				{
					return atom.y;
				}
				
			}
			virtual public float Z
			{
				get
				{
					return atom.z;
				}
				
			}
			virtual public float VectorX
			{
				get
				{
					return atom.vectorX;
				}
				
			}
			virtual public float VectorY
			{
				get
				{
					return atom.vectorY;
				}
				
			}
			virtual public float VectorZ
			{
				get
				{
					return atom.vectorZ;
				}
				
			}
			virtual public float Bfactor
			{
				get
				{
					return atom.bfactor;
				}
				
			}
			virtual public int Occupancy
			{
				get
				{
					return atom.occupancy;
				}
				
			}
			virtual public bool IsHetero
			{
				get
				{
					return atom.isHetero;
				}
				
			}
			virtual public int AtomSerial
			{
				get
				{
					return atom.atomSerial;
				}
				
			}
			virtual public char ChainID
			{
				get
				{
					return org.jmol.api.JmolAdapter.canonizeChainID(atom.chainID);
				}
				
			}
			virtual public char AlternateLocationID
			{
				get
				{
					return org.jmol.api.JmolAdapter.canonizeAlternateLocationID(atom.alternateLocationID);
				}
				
			}
			virtual public System.String Group3
			{
				get
				{
					return atom.group3;
				}
				
			}
			virtual public int SequenceNumber
			{
				get
				{
					return atom.sequenceNumber;
				}
				
			}
			virtual public char InsertionCode
			{
				get
				{
					return org.jmol.api.JmolAdapter.canonizeInsertionCode(atom.insertionCode);
				}
				
			}
			public SmarterJmolAdapter Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			internal AtomSetCollection atomSetCollection;
			internal int iatom;
			internal Atom atom;
			
			internal AtomIterator(SmarterJmolAdapter enclosingInstance, AtomSetCollection atomSetCollection)
			{
				InitBlock(enclosingInstance);
				this.atomSetCollection = atomSetCollection;
				iatom = 0;
			}
			public virtual bool hasNext()
			{
				if (iatom == atomSetCollection.atomCount)
					return false;
				atom = atomSetCollection.atoms[iatom++];
				return true;
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'BondIterator' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		new internal class BondIterator:JmolAdapter.BondIterator
		{
			private void  InitBlock(SmarterJmolAdapter enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private SmarterJmolAdapter enclosingInstance;
			virtual public System.Object AtomUniqueID1
			{
				get
				{
					return atoms[bond.atomIndex1];
				}
				
			}
			virtual public System.Object AtomUniqueID2
			{
				get
				{
					return atoms[bond.atomIndex2];
				}
				
			}
			virtual public int EncodedOrder
			{
				get
				{
					return bond.order;
				}
				
			}
			public SmarterJmolAdapter Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			internal AtomSetCollection atomSetCollection;
			internal Atom[] atoms;
			internal Bond[] bonds;
			internal int ibond;
			internal Bond bond;
			
			internal BondIterator(SmarterJmolAdapter enclosingInstance, AtomSetCollection atomSetCollection)
			{
				InitBlock(enclosingInstance);
				this.atomSetCollection = atomSetCollection;
				atoms = atomSetCollection.atoms;
				bonds = atomSetCollection.bonds;
				ibond = 0;
			}
			public virtual bool hasNext()
			{
				if (ibond == atomSetCollection.bondCount)
					return false;
				bond = bonds[ibond++];
				return true;
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'StructureIterator' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		new public class StructureIterator:JmolAdapter.StructureIterator
		{
			private void  InitBlock(SmarterJmolAdapter enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private SmarterJmolAdapter enclosingInstance;
			virtual public System.String StructureType
			{
				get
				{
					return structure.structureType;
				}
				
			}
			virtual public char StartChainID
			{
				get
				{
					return org.jmol.api.JmolAdapter.canonizeChainID(structure.startChainID);
				}
				
			}
			virtual public int StartSequenceNumber
			{
				get
				{
					return structure.startSequenceNumber;
				}
				
			}
			virtual public char StartInsertionCode
			{
				get
				{
					return org.jmol.api.JmolAdapter.canonizeInsertionCode(structure.startInsertionCode);
				}
				
			}
			virtual public char EndChainID
			{
				get
				{
					return org.jmol.api.JmolAdapter.canonizeChainID(structure.endChainID);
				}
				
			}
			virtual public int EndSequenceNumber
			{
				get
				{
					return structure.endSequenceNumber;
				}
				
			}
			virtual public char EndInsertionCode
			{
				get
				{
					return structure.endInsertionCode;
				}
				
			}
			public SmarterJmolAdapter Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			internal int structureCount;
			internal Structure[] structures;
			internal Structure structure;
			internal int istructure;
			
			internal StructureIterator(SmarterJmolAdapter enclosingInstance, AtomSetCollection atomSetCollection)
			{
				InitBlock(enclosingInstance);
				structureCount = atomSetCollection.structureCount;
				structures = atomSetCollection.structures;
				istructure = 0;
			}
			
			public virtual bool hasNext()
			{
				if (istructure == structureCount)
					return false;
				structure = structures[istructure++];
				return true;
			}
		}
	}
}