/* $RCSfile$
* $Author: hansonr $
* $Date: 2006-04-14 00:06:39 +0200 (ven., 14 avr. 2006) $
* $Revision: 4966 $
*
* Copyright (C) 2003-2005  The Jmol Development Team
*
* Contact: jmol-developers@lists.sf.net
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
using Graphics3D = org.jmol.g3d.Graphics3D;
using Tuple = org.jmol.bspt.Tuple;
//UPGRADE_TODO: The type 'javax.vecmath.Point3f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Point3f = javax.vecmath.Point3f;
//UPGRADE_TODO: The type 'javax.vecmath.Vector3f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Vector3f = javax.vecmath.Vector3f;
//UPGRADE_TODO: The type 'javax.vecmath.Point3i' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Point3i = javax.vecmath.Point3i;
namespace org.jmol.viewer
{
	
	sealed class Atom : Tuple
	{
		internal Group Group
		{
			get
			{
				return group;
			}
			
			set
			{
				this.group = value;
			}
			
		}
		internal short MadAtom
		{
			/*
			* What is a MAR?
			*  - just a term that I made up
			*  - an abbreviation for Milli Angstrom Radius
			* that is:
			*  - a *radius* of either a bond or an atom
			*  - in *millis*, or thousandths of an *angstrom*
			*  - stored as a short
			*
			* However! In the case of an atom radius, if the parameter
			* gets passed in as a negative number, then that number
			* represents a percentage of the vdw radius of that atom.
			* This is converted to a normal MAR as soon as possible
			*
			* (I know almost everyone hates bytes & shorts, but I like them ...
			*  gives me some tiny level of type-checking ...
			*  a rudimentary form of enumerations/user-defined primitive types)
			*/
			
			
			set
			{
				if (this.madAtom == JmolConstants.MAR_DELETED)
					return ;
				this.madAtom = convertEncodedMad(value);
			}
			
		}
		internal int RasMolRadius
		{
			get
			{
				if (madAtom == JmolConstants.MAR_DELETED)
					return 0;
				return madAtom / (4 * 2);
			}
			
		}
		internal int CovalentBondCount
		{
			get
			{
				if (bonds == null)
					return 0;
				int n = 0;
				for (int i = bonds.Length; --i >= 0; )
					if ((bonds[i].order & JmolConstants.BOND_COVALENT_MASK) != 0)
						++n;
				return n;
			}
			
		}
		internal int HbondCount
		{
			get
			{
				if (bonds == null)
					return 0;
				int n = 0;
				for (int i = bonds.Length; --i >= 0; )
					if ((bonds[i].order & JmolConstants.BOND_HYDROGEN_MASK) != 0)
						++n;
				return n;
			}
			
		}
		internal Bond[] Bonds
		{
			get
			{
				return bonds;
			}
			
		}
		internal short ColixAtom
		{
			set
			{
				if (value == 0)
					value = group.chain.frame.viewer.getColixAtomPalette(this, "cpk");
				this.colixAtom = value;
			}
			
		}
		internal bool Translucent
		{
			set
			{
				colixAtom = Graphics3D.setTranslucent(colixAtom, value);
			}
			
		}
		internal Vector3f VibrationVector
		{
			get
			{
				Vector3f[] vibrationVectors = group.chain.frame.vibrationVectors;
				return vibrationVectors == null?null:vibrationVectors[atomIndex];
			}
			
		}
		internal System.String Label
		{
			set
			{
				group.chain.frame.setLabel(value, atomIndex);
			}
			
		}
		internal sbyte ElementNumber
		{
			get
			{
				return elementNumber;
			}
			
		}
		internal System.String ElementSymbol
		{
			get
			{
				return JmolConstants.elementSymbols[elementNumber];
			}
			
		}
		internal System.String AtomNameOrNull
		{
			get
			{
				System.String[] atomNames = group.chain.frame.atomNames;
				return atomNames == null?null:atomNames[atomIndex];
			}
			
		}
		internal System.String AtomName
		{
			get
			{
				System.String atomName = AtomNameOrNull;
				return (atomName != null?atomName:JmolConstants.elementSymbols[elementNumber]);
			}
			
		}
		internal System.String PdbAtomName4
		{
			get
			{
				System.String atomName = AtomNameOrNull;
				return atomName != null?atomName:"";
			}
			
		}
		internal System.String Group1
		{
			get
			{
				if (group == null)
					return null;
				return group.Group1;
			}
			
		}
		internal int Seqcode
		{
			get
			{
				if (group == null)
					return - 1;
				return group.seqcode;
			}
			
		}
		internal int Resno
		{
			get
			{
				if (group == null)
					return - 1;
				return group.Resno;
			}
			
		}
		internal int AtomNumber
		{
			get
			{
				int[] atomSerials = group.chain.frame.atomSerials;
				if (atomSerials != null)
					return atomSerials[atomIndex];
				if ((System.Object) group.chain.frame.modelSetTypeName == (System.Object) "xyz" && group.chain.frame.viewer.ZeroBasedXyzRasmol)
					return atomIndex;
				return atomIndex + 1;
			}
			
		}
		internal bool Hetero
		{
			get
			{
				return (formalChargeAndFlags & IS_HETERO_FLAG) != 0;
			}
			
		}
		internal int FormalCharge
		{
			get
			{
				return formalChargeAndFlags >> 3;
			}
			
		}
		internal bool Visible
		{
			get
			{
				return (formalChargeAndFlags & VISIBLE_FLAG) != 0;
			}
			
		}
		internal float PartialCharge
		{
			get
			{
				float[] partialCharges = group.chain.frame.partialCharges;
				return partialCharges == null?0:partialCharges[atomIndex];
			}
			
		}
		internal Point3f Point3f
		{
			get
			{
				return point3f;
			}
			
		}
		internal float AtomX
		{
			get
			{
				return point3f.x;
			}
			
		}
		internal float AtomY
		{
			get
			{
				return point3f.y;
			}
			
		}
		internal float AtomZ
		{
			get
			{
				return point3f.z;
			}
			
		}
		internal short VanderwaalsMar
		{
			get
			{
				return JmolConstants.vanderwaalsMars[elementNumber];
			}
			
		}
		internal float VanderwaalsRadiusFloat
		{
			get
			{
				return JmolConstants.vanderwaalsMars[elementNumber] / 1000f;
			}
			
		}
		internal short BondingMar
		{
			get
			{
				return JmolConstants.getBondingMar(elementNumber, formalChargeAndFlags >> 3);
			}
			
		}
		internal float BondingRadiusFloat
		{
			get
			{
				return BondingMar / 1000f;
			}
			
		}
		internal int CurrentBondCount
		{
			get
			{
				return bonds == null?0:bonds.Length;
				/*
				int currentBondCount = 0;
				for (int i = (bonds == null ? 0 : bonds.length); --i >= 0; )
				currentBondCount += bonds[i].order & JmolConstants.BOND_COVALENT;
				return currentBondCount;
				*/
			}
			
		}
		internal short Colix
		{
			get
			{
				return colixAtom;
			}
			
		}
		internal int Argb
		{
			get
			{
				return group.chain.frame.viewer.getColixArgb(colixAtom);
			}
			
		}
		internal float Radius
		{
			get
			{
				if (madAtom == JmolConstants.MAR_DELETED)
					return 0;
				return madAtom / (1000f * 2);
			}
			
		}
		internal char ChainID
		{
			get
			{
				return group.chain.chainID;
			}
			
		}
		internal int Occupancy
		{
			// a percentage value in the range 0-100
			
			get
			{
				sbyte[] occupancies = group.chain.frame.occupancies;
				return occupancies == null?100:occupancies[atomIndex];
			}
			
		}
		internal int Bfactor100
		{
			// This is called bfactor100 because it is stored as an integer
			// 100 times the bfactor(temperature) value
			
			get
			{
				short[] bfactor100s = group.chain.frame.bfactor100s;
				if (bfactor100s == null)
					return 0;
				return bfactor100s[atomIndex];
			}
			
		}
		internal int PolymerLength
		{
			get
			{
				return group.PolymerLength;
			}
			
		}
		internal int PolymerIndex
		{
			get
			{
				return group.PolymerIndex;
			}
			
		}
		internal int SelectedGroupCountWithinChain
		{
			get
			{
				return group.chain.SelectedGroupCount;
			}
			
		}
		internal int SelectedGroupIndexWithinChain
		{
			get
			{
				return group.chain.getSelectedGroupIndex(group);
			}
			
		}
		internal int SelectedMonomerCountWithinPolymer
		{
			get
			{
				if (group is Monomer)
				{
					return ((Monomer) group).polymer.selectedMonomerCount;
				}
				return 0;
			}
			
		}
		internal int SelectedMonomerIndexWithinPolymer
		{
			get
			{
				if (group is Monomer)
				{
					Monomer monomer = (Monomer) group;
					return monomer.polymer.getSelectedMonomerIndex(monomer);
				}
				return - 1;
			}
			
		}
		internal int AtomIndex
		{
			get
			{
				return atomIndex;
			}
			
		}
		internal Chain Chain
		{
			get
			{
				return group.chain;
			}
			
		}
		internal Model Model
		{
			get
			{
				return group.chain.model;
			}
			
		}
		internal int ModelIndex
		{
			get
			{
				return modelIndex;
			}
			
		}
		internal bool Deleted
		{
			get
			{
				return madAtom == JmolConstants.MAR_DELETED;
			}
			
		}
		internal sbyte ProteinStructureType
		{
			get
			{
				return group.ProteinStructureType;
			}
			
		}
		internal short GroupID
		{
			get
			{
				return group.groupID;
			}
			
		}
		internal System.String SeqcodeString
		{
			get
			{
				return group.getSeqcodeString();
			}
			
		}
		internal System.String ModelTag
		{
			get
			{
				return group.chain.model.modelTag;
			}
			
		}
		internal int ModelTagNumber
		{
			get
			{
				try
				{
					return System.Int32.Parse(group.chain.model.modelTag);
				}
				catch (System.FormatException nfe)
				{
					return modelIndex + 1;
				}
			}
			
		}
		internal sbyte SpecialAtomID
		{
			get
			{
				sbyte[] specialAtomIDs = group.chain.frame.specialAtomIDs;
				return specialAtomIDs == null?0:specialAtomIDs[atomIndex];
			}
			
		}
		internal System.String Info
		{
			get
			{
				return Identity;
			}
			
		}
		internal System.String Identity
		{
			get
			{
				System.Text.StringBuilder info = new System.Text.StringBuilder();
				System.String group3 = getGroup3();
				System.String seqcodeString = SeqcodeString;
				char chainID = ChainID;
				if (group3 != null && group3.Length > 0)
				{
					info.Append("[");
					info.Append(group3);
					info.Append("]");
				}
				if (seqcodeString != null)
					info.Append(seqcodeString);
				if (chainID != 0 && chainID != ' ')
				{
					info.Append(":");
					info.Append(chainID);
				}
				System.String atomName = AtomNameOrNull;
				if (atomName != null)
				{
					if (info.Length > 0)
						info.Append(".");
					info.Append(atomName);
				}
				if (info.Length == 0)
				{
					info.Append(ElementSymbol);
					info.Append(" ");
					info.Append(AtomNumber);
				}
				if (group.chain.frame.ModelCount > 1)
				{
					info.Append("/");
					info.Append(ModelTagNumber);
				}
				info.Append(" #");
				info.Append(AtomNumber);
				return "" + info;
			}
			
		}
		internal int ScreenX
		{
			////////////////////////////////////////////////////////////////
			
			get
			{
				return screenX;
			}
			
		}
		internal int ScreenY
		{
			get
			{
				return screenY;
			}
			
		}
		internal int ScreenZ
		{
			get
			{
				return screenZ;
			}
			
		}
		internal int ScreenD
		{
			get
			{
				return screenDiameter;
			}
			
		}
		internal bool Protein
		{
			////////////////////////////////////////////////////////////////
			
			
			get
			{
				return group.Protein;
			}
			
		}
		internal bool Nucleic
		{
			get
			{
				return group.Nucleic;
			}
			
		}
		internal bool Dna
		{
			get
			{
				return group.Dna;
			}
			
		}
		internal bool Rna
		{
			get
			{
				return group.Rna;
			}
			
		}
		internal bool Purine
		{
			get
			{
				return group.Purine;
			}
			
		}
		internal bool Pyrimidine
		{
			get
			{
				return group.Pyrimidine;
			}
			
		}
		internal System.Collections.Hashtable PublicProperties
		{
			////////////////////////////////////////////////////////////////
			
			
			get
			{
				System.Collections.Hashtable ht = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
				ht["element"] = ElementSymbol;
				ht["x"] = new Double(point3f.x);
				ht["y"] = new Double(point3f.y);
				ht["z"] = new Double(point3f.z);
				ht["atomIndex"] = (System.Int32) atomIndex;
				ht["modelIndex"] = (System.Int32) modelIndex;
				ht["argb"] = (System.Int32) Argb;
				ht["radius"] = (double) Radius;
				ht["atomNumber"] = (System.Int32) AtomNumber;
				return ht;
			}
			
		}
		
		internal const sbyte VISIBLE_FLAG = (sbyte) (0x01);
		internal const sbyte VIBRATION_VECTOR_FLAG = (sbyte) (0x02);
		internal const sbyte IS_HETERO_FLAG = (sbyte) (0x04);
		
		internal Group group;
		internal int atomIndex;
		internal Point3f point3f;
		internal int screenX;
		internal int screenY;
		internal int screenZ;
		internal short screenDiameter;
		internal short modelIndex; // we want this here for the BallsRenderer
		internal sbyte elementNumber;
		internal sbyte formalChargeAndFlags;
		internal sbyte alternateLocationID;
		internal short madAtom;
		internal short colixAtom;
		internal Bond[] bonds;
		
		internal Atom(Viewer viewer, Frame frame, int modelIndex, int atomIndex, sbyte elementNumber, System.String atomName, int formalCharge, float partialCharge, int occupancy, float bfactor, float x, float y, float z, bool isHetero, int atomSerial, char chainID, float vibrationX, float vibrationY, float vibrationZ, char alternateLocationID, System.Object clientAtomReference)
		{
			this.modelIndex = (short) modelIndex;
			this.atomIndex = atomIndex;
			this.elementNumber = elementNumber;
			this.formalChargeAndFlags = (sbyte) (formalCharge << 3);
			this.colixAtom = viewer.getColixAtom(this);
			this.alternateLocationID = (sbyte) alternateLocationID;
			MadAtom = viewer.MadAtom;
			this.point3f = new Point3f(x, y, z);
			if (isHetero)
				formalChargeAndFlags |= IS_HETERO_FLAG;
			
			if (atomName != null)
			{
				if (frame.atomNames == null)
					frame.atomNames = new System.String[frame.atoms.Length];
				frame.atomNames[atomIndex] = String.Intern(atomName);
			}
			
			sbyte specialAtomID = lookupSpecialAtomID(atomName);
			if (specialAtomID != 0)
			{
				if (frame.specialAtomIDs == null)
					frame.specialAtomIDs = new sbyte[frame.atoms.Length];
				frame.specialAtomIDs[atomIndex] = specialAtomID;
			}
			
			if (occupancy < 0)
				occupancy = 0;
			else if (occupancy > 100)
				occupancy = 100;
			if (occupancy != 100)
			{
				if (frame.occupancies == null)
					frame.occupancies = new sbyte[frame.atoms.Length];
				frame.occupancies[atomIndex] = (sbyte) occupancy;
			}
			
			if (atomSerial != System.Int32.MinValue)
			{
				if (frame.atomSerials == null)
					frame.atomSerials = new int[frame.atoms.Length];
				frame.atomSerials[atomIndex] = atomSerial;
			}
			
			if (!System.Single.IsNaN(partialCharge))
			{
				if (frame.partialCharges == null)
					frame.partialCharges = new float[frame.atoms.Length];
				frame.partialCharges[atomIndex] = partialCharge;
			}
			
			if (!System.Single.IsNaN(bfactor) && bfactor != 0)
			{
				if (frame.bfactor100s == null)
					frame.bfactor100s = new short[frame.atoms.Length];
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				frame.bfactor100s[atomIndex] = (short) (bfactor * 100);
			}
			
			if (!System.Single.IsNaN(vibrationX) && !System.Single.IsNaN(vibrationY) && !System.Single.IsNaN(vibrationZ))
			{
				if (frame.vibrationVectors == null)
					frame.vibrationVectors = new Vector3f[frame.atoms.Length];
				frame.vibrationVectors[atomIndex] = new Vector3f(vibrationX, vibrationY, vibrationZ);
				formalChargeAndFlags |= VIBRATION_VECTOR_FLAG;
			}
			if (clientAtomReference != null)
			{
				if (frame.clientAtomReferences == null)
					frame.clientAtomReferences = new System.Object[frame.atoms.Length];
				frame.clientAtomReferences[atomIndex] = clientAtomReference;
			}
		}
		
		internal bool isBonded(Atom atomOther)
		{
			return getBond(atomOther) != null;
		}
		
		/// <summary> Returns the count of connections to atoms found in the
		/// specified BitSet. Bond order is not considered. Hydrogen bonds
		/// are considered valid connections and are included in the count.
		/// <p>
		/// If the bs parameter is null then the total count of
		/// connections is returned;
		/// 
		/// </summary>
		/// <param name="bs">the bitset of atom indexes to be considered
		/// </param>
		/// <returns>   the count
		/// </returns>
		internal int getConnectedCount(System.Collections.BitArray bs)
		{
			int connectedCount = 0;
			if (bonds != null)
			{
				for (int i = bonds.Length; --i >= 0; )
				{
					Bond bond = bonds[i];
					Atom otherAtom = (bond.atom1 != this)?bond.atom1:bond.atom2;
					if (bs == null || bs.Get(otherAtom.atomIndex))
						++connectedCount;
				}
			}
			return connectedCount;
		}
		
		internal Bond getBond(Atom atomOther)
		{
			if (bonds != null)
				for (int i = bonds.Length; --i >= 0; )
				{
					Bond bond = bonds[i];
					if ((bond.atom1 == atomOther) || (bond.atom2 == atomOther))
						return bond;
				}
			return null;
		}
		
		internal Bond bondMutually(Atom atomOther, short order, Frame frame)
		{
			if (isBonded(atomOther))
				return null;
			Bond bond = new Bond(this, atomOther, order, frame);
			addBond(bond, frame);
			atomOther.addBond(bond, frame);
			return bond;
		}
		
		private void  addBond(Bond bond, Frame frame)
		{
			if (bonds == null)
			{
				bonds = new Bond[1];
				bonds[0] = bond;
			}
			else
			{
				bonds = frame.addToBonds(bond, bonds);
			}
		}
		
		internal void  deleteBondedAtom(Atom atomToDelete)
		{
			if (bonds == null)
				return ;
			for (int i = bonds.Length; --i >= 0; )
			{
				Bond bond = bonds[i];
				Atom atomBonded = (bond.atom1 != this)?bond.atom1:bond.atom2;
				if (atomBonded == atomToDelete)
				{
					deleteBond(i);
					return ;
				}
			}
		}
		
		internal void  deleteAllBonds()
		{
			if (bonds == null)
				return ;
			for (int i = bonds.Length; --i >= 0; )
				group.chain.frame.deleteBond(bonds[i]);
			if (bonds != null)
			{
				System.Console.Out.WriteLine("bond delete error");
				throw new System.NullReferenceException();
			}
		}
		
		internal void  deleteBond(Bond bond)
		{
			for (int i = bonds.Length; --i >= 0; )
				if (bonds[i] == bond)
				{
					deleteBond(i);
					return ;
				}
		}
		
		internal void  deleteBond(int i)
		{
			int newLength = bonds.Length - 1;
			if (newLength == 0)
			{
				bonds = null;
				return ;
			}
			Bond[] bondsNew = new Bond[newLength];
			int j = 0;
			for (; j < i; ++j)
				bondsNew[j] = bonds[j];
			for (; j < newLength; ++j)
				bondsNew[j] = bonds[j + 1];
			bonds = bondsNew;
		}
		
		internal void  clearBonds()
		{
			bonds = null;
		}
		
		internal int getBondedAtomIndex(int bondIndex)
		{
			Bond bond = bonds[bondIndex];
			return (((bond.atom1 == this)?bond.atom2:bond.atom1).atomIndex & 0xFFFF);
		}
		
		internal short convertEncodedMad(int size)
		{
			if (size == - 1000)
			{
				// temperature
				int diameter = Bfactor100 * 10 * 2;
				if (diameter > 4000)
					diameter = 4000;
				size = diameter;
			}
			else if (size == - 1001)
			// ionic
				size = (BondingMar * 2);
			else if (size < 0)
			{
				size = - size;
				if (size > 200)
					size = 200;
				size = (size * VanderwaalsMar / 50);
			}
			return (short) size;
		}
		
		// miguel 2006 03 25
		// not sure what we should do here
		// current implementation of g3d uses a short for the zbuffer coordinate
		// we could consider turning that into an int, but that would have
		// significant implications
		//
		// actually, I think that it might work out just fine. we should use
		// an int in this world, but let g3d deal with the problem of
		// something having a depth that is more than 32K ... in the same
		// sense that g3d will clip if something is not on the screen
		
		//  final static int MIN_Z = 100;
		//  final static int MAX_Z = 32766;
		
		internal void  transform(Viewer viewer)
		{
			if (madAtom == JmolConstants.MAR_DELETED)
				return ;
			Point3i screen;
			Vector3f[] vibrationVectors;
			if ((formalChargeAndFlags & VIBRATION_VECTOR_FLAG) == 0 || (vibrationVectors = group.chain.frame.vibrationVectors) == null)
				screen = viewer.transformPoint(point3f);
			else
				screen = viewer.transformPoint(point3f, vibrationVectors[atomIndex]);
			screenX = screen.x;
			screenY = screen.y;
			screenZ = screen.z;
			screenDiameter = viewer.scaleToScreen(screenZ, madAtom);
		}
		
		internal System.String getGroup3()
		{
			return group.getGroup3();
		}
		
		internal bool isGroup3(System.String group3)
		{
			return group.isGroup3(group3);
		}
		
		internal bool isGroup3Match(System.String strWildcard)
		{
			return group.isGroup3Match(strWildcard);
		}
		
		internal bool isAtomNameMatch(System.String strPattern)
		{
			System.String atomName = AtomNameOrNull;
			int cchAtomName = atomName == null?0:atomName.Length;
			int cchPattern = strPattern.Length;
			int ich;
			for (ich = 0; ich < cchPattern; ++ich)
			{
				char charWild = System.Char.ToUpper(strPattern[ich]);
				if (charWild == '?')
					continue;
				if (ich >= cchAtomName || charWild != System.Char.ToUpper(atomName[ich]))
					return false;
			}
			return ich >= cchAtomName;
		}
		
		internal bool isAlternateLocationMatch(System.String strPattern)
		{
			if (strPattern == null)
				return true;
			if (strPattern.Length != 1)
				return false;
			return alternateLocationID == strPattern[0];
		}
		
		public float getDimensionValue(int dimension)
		{
			return (dimension == 0?point3f.x:(dimension == 1?point3f.y:point3f.z));
		}
		
		// find the longest bond to discard
		// but return null if atomChallenger is longer than any
		// established bonds
		// note that this algorithm works when maximum valence == 0
		internal Bond getLongestBondToDiscard(Atom atomChallenger)
		{
			float dist2Longest = point3f.distanceSquared(atomChallenger.point3f);
			Bond bondLongest = null;
			for (int i = bonds.Length; --i >= 0; )
			{
				Bond bond = bonds[i];
				Atom atomOther = bond.atom1 != this?bond.atom1:bond.atom2;
				float dist2 = point3f.distanceSquared(atomOther.point3f);
				if (dist2 > dist2Longest)
				{
					bondLongest = bond;
					dist2Longest = dist2;
				}
			}
			//    System.out.println("atom at " + point3f + " suggests discard of " +
			//                       bondLongest + " dist2=" + dist2Longest);
			return bondLongest;
		}
		
		internal System.String getClientAtomStringProperty(System.String propertyName)
		{
			System.Object[] clientAtomReferences = group.chain.frame.clientAtomReferences;
			return ((clientAtomReferences == null || clientAtomReferences.Length <= atomIndex)?null:(group.chain.frame.viewer.getClientAtomStringProperty(clientAtomReferences[atomIndex], propertyName)));
		}
		
		internal void  markDeleted()
		{
			deleteAllBonds();
			madAtom = JmolConstants.MAR_DELETED;
		}
		
		internal void  demoteSpecialAtomImposter()
		{
			group.chain.frame.specialAtomIDs[atomIndex] = 0;
		}
		
		/* ***************************************************************
		* disabled until I figure out how to generate pretty names
		* without breaking inorganic compounds
		
		// this requires a 4 letter name, in PDB format
		// only here for transition purposes
		static String calcPrettyName(String name) {
		if (name.length() < 4)
		return name;
		char chBranch = name.charAt(3);
		char chRemote = name.charAt(2);
		switch (chRemote) {
		case 'A':
		chRemote = '\u03B1';
		break;
		case 'B':
		chRemote = '\u03B2';
		break;
		case 'C':
		case 'G':
		chRemote = '\u03B3';
		break;
		case 'D':
		chRemote = '\u03B4';
		break;
		case 'E':
		chRemote = '\u03B5';
		break;
		case 'Z':
		chRemote = '\u03B6';
		break;
		case 'H':
		chRemote = '\u03B7';
		}
		String pretty = name.substring(0, 2).trim();
		if (chBranch != ' ')
		pretty += "" + chRemote + chBranch;
		else
		pretty += chRemote;
		return pretty;
		}
		*/
		
		private static System.Collections.Hashtable htAtom = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
		
		internal static System.String generateStarredAtomName(System.String primedAtomName)
		{
			int primeIndex = primedAtomName.IndexOf('\'');
			if (primeIndex < 0)
				return null;
			return primedAtomName.Replace('\'', '*');
		}
		
		internal static System.String generatePrimeAtomName(System.String starredAtomName)
		{
			int starIndex = starredAtomName.IndexOf('*');
			if (starIndex < 0)
				return starredAtomName;
			return starredAtomName.Replace('*', '\'');
		}
		
		internal sbyte lookupSpecialAtomID(System.String atomName)
		{
			if (atomName != null)
			{
				atomName = generatePrimeAtomName(atomName);
				System.Int32 boxedAtomID = (System.Int32) htAtom[atomName];
				//UPGRADE_TODO: The 'System.Int32' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
				if (boxedAtomID != null)
					return (sbyte) (boxedAtomID);
			}
			return 0;
		}
		
		internal System.String formatLabel(System.String strFormat)
		{
			if (strFormat == null || strFormat.Equals(""))
				return null;
			System.String strLabel = "";
			//int cch = strFormat.length();
			int ich, ichPercent;
			//UPGRADE_WARNING: Method 'java.lang.String.indexOf' was converted to 'System.String.IndexOf' which may throw an exception. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1101'"
			for (ich = 0; (ichPercent = strFormat.IndexOf('%', ich)) != - 1; )
			{
				if (ich != ichPercent)
					strLabel += strFormat.Substring(ich, (ichPercent) - (ich));
				ich = ichPercent + 1;
				try
				{
					System.String strT = "";
					float floatT = 0;
					bool floatIsSet = false;
					bool alignLeft = false;
					if (strFormat[ich] == '-')
					{
						alignLeft = true;
						++ich;
					}
					bool zeroPad = false;
					if (strFormat[ich] == '0')
					{
						zeroPad = true;
						++ich;
					}
					char ch;
					int width = 0;
					while ((ch = strFormat[ich]) >= '0' && (ch <= '9'))
					{
						width = (10 * width) + (ch - '0');
						++ich;
					}
					int precision = - 1;
					if (strFormat[ich] == '.')
					{
						++ich;
						if ((ch = strFormat[ich]) >= '0' && (ch <= '9'))
						{
							precision = ch - '0';
							++ich;
						}
					}
					switch (ch = strFormat[ich++])
					{
						
						case 'i': 
							strT = "" + AtomNumber;
							break;
						
						case 'a': 
							strT = AtomName;
							break;
						
						case 'e': 
							strT = JmolConstants.elementSymbols[elementNumber];
							break;
						
						case 'x': 
							floatT = point3f.x;
							floatIsSet = true;
							break;
						
						case 'y': 
							floatT = point3f.y;
							floatIsSet = true;
							break;
						
						case 'z': 
							floatT = point3f.z;
							floatIsSet = true;
							break;
						
						case 'X': 
							strT = "" + atomIndex;
							break;
						
						case 'C': 
							int formalCharge = FormalCharge;
							if (formalCharge > 0)
								strT = "" + formalCharge + "+";
							else if (formalCharge < 0)
								strT = "" + (- formalCharge) + "-";
							else
								strT = "0";
							break;
						
						case 'P': 
							floatT = PartialCharge;
							floatIsSet = true;
							break;
						
						case 'V': 
							floatT = VanderwaalsRadiusFloat;
							floatIsSet = true;
							break;
						
						case 'I': 
							floatT = BondingRadiusFloat;
							floatIsSet = true;
							break;
						
						case 'b': 
						// these two are the same
						case 't': 
							floatT = Bfactor100 / 100f;
							floatIsSet = true;
							break;
						
						case 'q': 
							strT = "" + Occupancy;
							break;
						
						case 'c': 
						// these two are the same
						case 's': 
							strT = "" + ChainID;
							break;
						
						case 'L': 
							strT = "" + PolymerLength;
							break;
						
						case 'M': 
							strT = "/" + ModelTagNumber;
							break;
						
						case 'm': 
							strT = Group1;
							break;
						
						case 'n': 
							strT = getGroup3();
							break;
						
						case 'r': 
							strT = SeqcodeString;
							break;
						
						case 'U': 
							strT = Identity;
							break;
						
						case '%': 
							strT = "%";
							break;
						
						case '{':  // client property name
							//UPGRADE_WARNING: Method 'java.lang.String.indexOf' was converted to 'System.String.IndexOf' which may throw an exception. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1101'"
							int ichCloseBracket = strFormat.IndexOf('}', ich);
							if (ichCloseBracket > ich)
							{
								// also picks up -1 when no '}' is found
								System.String propertyName = strFormat.Substring(ich, (ichCloseBracket) - (ich));
								System.String value_Renamed = getClientAtomStringProperty(propertyName);
								if (value_Renamed != null)
									strT = value_Renamed;
								ich = ichCloseBracket + 1;
								break;
							}
							// malformed will fall into
							goto default;
						
						default: 
							strT = "%" + ch;
							break;
						
					}
					if (floatIsSet)
					{
						strLabel += format(floatT, width, precision, alignLeft, zeroPad);
					}
					else
					{
						strLabel += format(strT, width, precision, alignLeft, zeroPad);
					}
				}
				catch (System.IndexOutOfRangeException ioobe)
				{
					ich = ichPercent;
					break;
				}
			}
			strLabel += strFormat.Substring(ich);
			if (strLabel.Length == 0)
				return null;
			return String.Intern(strLabel);
		}
		
		internal System.String format(float value_Renamed, int width, int precision, bool alignLeft, bool zeroPad)
		{
			return format(group.chain.frame.viewer.formatDecimal(value_Renamed, precision), width, 0, alignLeft, zeroPad);
		}
		
		internal static System.String format(System.String value_Renamed, int width, int precision, bool alignLeft, bool zeroPad)
		{
			if (value_Renamed == null)
				return "";
			if (precision > value_Renamed.Length)
				value_Renamed = value_Renamed.Substring(0, (precision) - (0));
			int padLength = width - value_Renamed.Length;
			if (padLength <= 0)
				return value_Renamed;
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			if (alignLeft)
				sb.Append(value_Renamed);
			for (int i = padLength; --i >= 0; )
				sb.Append((!alignLeft && zeroPad)?'0':' ');
			if (!alignLeft)
				sb.Append(value_Renamed);
			return "" + sb;
		}
		
		internal bool isCursorOnTopOfVisibleAtom(int xCursor, int yCursor, int minRadius, Atom competitor)
		{
			return (((formalChargeAndFlags & VISIBLE_FLAG) != 0) && isCursorOnTop(xCursor, yCursor, minRadius, competitor));
		}
		
		internal bool isCursorOnTop(int xCursor, int yCursor, int minRadius, Atom competitor)
		{
			int r = screenDiameter / 2;
			if (r < minRadius)
				r = minRadius;
			int r2 = r * r;
			int dx = screenX - xCursor;
			int dx2 = dx * dx;
			if (dx2 > r2)
				return false;
			int dy = screenY - yCursor;
			int dy2 = dy * dy;
			int dz2 = r2 - (dx2 + dy2);
			if (dz2 < 0)
				return false;
			if (competitor == null)
				return true;
			int z = screenZ;
			int zCompetitor = competitor.screenZ;
			int rCompetitor = competitor.screenDiameter / 2;
			if (z < zCompetitor - rCompetitor)
				return true;
			int dxCompetitor = competitor.screenX - xCursor;
			int dx2Competitor = dxCompetitor * dxCompetitor;
			int dyCompetitor = competitor.screenY - yCursor;
			int dy2Competitor = dyCompetitor * dyCompetitor;
			int r2Competitor = rCompetitor * rCompetitor;
			int dz2Competitor = r2Competitor - (dx2Competitor + dy2Competitor);
			return (z - System.Math.Sqrt(dz2) < zCompetitor - System.Math.Sqrt(dz2Competitor));
		}
		static Atom()
		{
			{
				for (int i = JmolConstants.specialAtomNames.Length; --i >= 0; )
				{
					System.String specialAtomName = JmolConstants.specialAtomNames[i];
					if (specialAtomName != null)
					{
						System.Int32 boxedI = (System.Int32) i;
						htAtom[specialAtomName] = boxedI;
					}
				}
			}
		}
	}
}