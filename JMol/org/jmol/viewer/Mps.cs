/* $RCSfile$
* $Author: egonw $
* $Date: 2005-11-10 16:52:44 +0100 (jeu., 10 nov. 2005) $
* $Revision: 4255 $
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
//UPGRADE_TODO: The type 'javax.vecmath.Point3f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Point3f = javax.vecmath.Point3f;
//UPGRADE_TODO: The type 'javax.vecmath.Vector3f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Vector3f = javax.vecmath.Vector3f;
using Graphics3D = org.jmol.g3d.Graphics3D;
namespace org.jmol.viewer
{
	
	/// <summary>*************************************************************
	/// Mps stands for Model-Chain-Polymer-Shape
	/// **************************************************************
	/// </summary>
	abstract class Mps:Shape
	{
		virtual internal int MpsmodelCount
		{
			get
			{
				return mpsmodels.Length;
			}
			
		}
		
		internal Mmset mmset;
		
		internal Mpsmodel[] mpsmodels;
		
		internal override void  initShape()
		{
			mmset = frame.mmset;
		}
		
		internal override void  setSize(int size, System.Collections.BitArray bsSelected)
		{
			short mad = (short) size;
			initialize();
			for (int m = mpsmodels.Length; --m >= 0; )
				mpsmodels[m].setMad(mad, bsSelected);
		}
		
		internal override void  setProperty(System.String propertyName, System.Object value_Renamed, System.Collections.BitArray bs)
		{
			initialize();
			if ((System.Object) "color" == (System.Object) propertyName)
			{
				System.String palette = null;
				short colix = Graphics3D.getColix(value_Renamed);
				if (colix == Graphics3D.UNRECOGNIZED)
					palette = ((System.String) value_Renamed);
				for (int m = mpsmodels.Length; --m >= 0; )
					mpsmodels[m].setColix(colix, palette, bs);
				return ;
			}
			if ((System.Object) "translucency" == (System.Object) propertyName)
			{
				bool isTranslucent = ((System.Object) "translucent" == value_Renamed);
				for (int m = mpsmodels.Length; --m >= 0; )
					mpsmodels[m].setTranslucent(isTranslucent, bs);
			}
		}
		
		internal abstract Mpspolymer allocateMpspolymer(Polymer polymer);
		
		internal virtual void  initialize()
		{
			if (mpsmodels == null)
			{
				int modelCount = mmset == null?0:mmset.ModelCount;
				Model[] models = mmset.Models;
				mpsmodels = new Mpsmodel[modelCount];
				for (int i = modelCount; --i >= 0; )
					mpsmodels[i] = new Mpsmodel(this, models[i]);
			}
		}
		
		internal virtual Mpsmodel getMpsmodel(int i)
		{
			return mpsmodels[i];
		}
		
		internal override void  findNearestAtomIndex(int xMouse, int yMouse, Closest closest)
		{
			for (int i = mpsmodels.Length; --i >= 0; )
				mpsmodels[i].findNearestAtomIndex(xMouse, yMouse, closest);
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'Mpsmodel' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		internal class Mpsmodel
		{
			private void  InitBlock(Mps enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Mps enclosingInstance;
			virtual internal int MpspolymerCount
			{
				get
				{
					return mpspolymers.Length;
				}
				
			}
			public Mps Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			internal Mpspolymer[] mpspolymers;
			internal int modelIndex;
			
			internal Mpsmodel(Mps enclosingInstance, Model model)
			{
				InitBlock(enclosingInstance);
				mpspolymers = new Mpspolymer[model.PolymerCount];
				this.modelIndex = model.modelIndex;
				for (int i = mpspolymers.Length; --i >= 0; )
					mpspolymers[i] = Enclosing_Instance.allocateMpspolymer(model.getPolymer(i));
			}
			
			internal virtual void  setMad(short mad, System.Collections.BitArray bsSelected)
			{
				for (int i = mpspolymers.Length; --i >= 0; )
				{
					Mpspolymer polymer = mpspolymers[i];
					if (polymer.monomerCount > 0)
						polymer.setMad(mad, bsSelected);
				}
			}
			
			internal virtual void  setColix(short colix, System.String palette, System.Collections.BitArray bsSelected)
			{
				for (int i = mpspolymers.Length; --i >= 0; )
				{
					Mpspolymer polymer = mpspolymers[i];
					if (polymer.monomerCount > 0)
						polymer.setColix(colix, palette, bsSelected);
				}
			}
			
			internal virtual void  setTranslucent(bool isTranslucent, System.Collections.BitArray bsSelected)
			{
				for (int i = mpspolymers.Length; --i >= 0; )
				{
					Mpspolymer polymer = mpspolymers[i];
					if (polymer.monomerCount > 0)
						polymer.setTranslucent(isTranslucent, bsSelected);
				}
			}
			
			internal virtual Mpspolymer getMpspolymer(int i)
			{
				return mpspolymers[i];
			}
			
			internal virtual void  findNearestAtomIndex(int xMouse, int yMouse, Closest closest)
			{
				for (int i = mpspolymers.Length; --i >= 0; )
					mpspolymers[i].findNearestAtomIndex(xMouse, yMouse, closest);
			}
		}
		
		internal abstract class Mpspolymer
		{
			internal Polymer polymer;
			internal short madOn;
			internal short madHelixSheet;
			internal short madTurnRandom;
			internal short madDnaRna;
			
			internal int monomerCount;
			internal Monomer[] monomers;
			internal short[] colixes;
			internal short[] mads;
			
			internal Point3f[] leadMidpoints;
			internal Vector3f[] wingVectors;
			
			internal Mpspolymer(Polymer polymer, int madOn, int madHelixSheet, int madTurnRandom, int madDnaRna)
			{
				this.polymer = polymer;
				this.madOn = (short) madOn;
				this.madHelixSheet = (short) madHelixSheet;
				this.madTurnRandom = (short) madTurnRandom;
				this.madDnaRna = (short) madDnaRna;
				
				// FIXME
				// I don't think that polymer can ever be null for this thing
				// so stop checking for null and see if it explodes
				monomerCount = polymer == null?0:polymer.monomerCount;
				if (monomerCount > 0)
				{
					colixes = new short[monomerCount];
					mads = new short[monomerCount + 1];
					monomers = polymer.monomers;
					
					leadMidpoints = polymer.LeadMidpoints;
					wingVectors = polymer.WingVectors;
				}
			}
			
			internal virtual short getMadSpecial(short mad, int groupIndex)
			{
				switch (mad)
				{
					
					case - 1:  // trace on
						if (madOn >= 0)
							return madOn;
						if (madOn != - 2)
						{
							System.Console.Out.WriteLine("not supported?");
							return 0;
						}
						// fall into;
						goto case - 2;
					
					case - 2:  // trace structure
						switch (monomers[groupIndex].ProteinStructureType)
						{
							
							case JmolConstants.PROTEIN_STRUCTURE_SHEET: 
							case JmolConstants.PROTEIN_STRUCTURE_HELIX: 
								return madHelixSheet;
							
							case JmolConstants.PROTEIN_STRUCTURE_DNA: 
							case JmolConstants.PROTEIN_STRUCTURE_RNA: 
								return madDnaRna;
							
							default: 
								return madTurnRandom;
							
						}
						goto case - 3;
					
					case - 3:  // trace temperature
						{
							if (!hasBfactorRange)
								calcBfactorRange();
							Atom atom = monomers[groupIndex].LeadAtom;
							int bfactor100 = atom.Bfactor100; // scaled by 1000
							int scaled = bfactor100 - bfactorMin;
							if (range == 0)
								return (short) 0;
							float percentile = scaled / floatRange;
							if (percentile < 0 || percentile > 1)
								System.Console.Out.WriteLine("Que ha ocurrido? " + percentile);
							//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
							return (short) ((1750 * percentile) + 250);
						}
					
					case - 4:  // trace displacement
						{
							Atom atom = monomers[groupIndex].LeadAtom;
							return (short) (2 * calcMeanPositionalDisplacement(atom.Bfactor100));
						}
					}
				System.Console.Out.WriteLine("unrecognized Mps.getSpecial(" + mad + ")");
				return 0;
			}
			
			internal bool hasBfactorRange = false;
			internal int bfactorMin, bfactorMax;
			internal int range;
			internal float floatRange;
			
			internal virtual void  calcBfactorRange()
			{
				bfactorMin = bfactorMax = monomers[0].LeadAtom.Bfactor100;
				for (int i = monomerCount; --i > 0; )
				{
					int bfactor = monomers[i].LeadAtom.Bfactor100;
					if (bfactor < bfactorMin)
						bfactorMin = bfactor;
					else if (bfactor > bfactorMax)
						bfactorMax = bfactor;
				}
				range = bfactorMax - bfactorMin;
				floatRange = range;
				System.Console.Out.WriteLine("bfactor range=" + range);
				hasBfactorRange = true;
			}
			
			internal virtual void  setMad(short mad, System.Collections.BitArray bsSelected)
			{
				int[] atomIndices = polymer.LeadAtomIndices;
				for (int i = monomerCount; --i >= 0; )
				{
					if (bsSelected.Get(atomIndices[i]))
						mads[i] = mad >= 0?mad:getMadSpecial(mad, i);
				}
				if (monomerCount > 1)
					mads[monomerCount] = mads[monomerCount - 1];
			}
			
			internal virtual void  setColix(short colix, System.String palette, System.Collections.BitArray bsSelected)
			{
				int[] atomIndices = polymer.LeadAtomIndices;
				for (int i = monomerCount; --i >= 0; )
				{
					int atomIndex = atomIndices[i];
					if (bsSelected.Get(atomIndex))
						colixes[i] = ((colix != Graphics3D.UNRECOGNIZED)?colix:Enclosing_Instance.viewer.getColixAtomPalette(Enclosing_Instance.frame.getAtomAt(atomIndex), palette));
				}
			}
			
			internal virtual void  setTranslucent(bool isTranslucent, System.Collections.BitArray bsSelected)
			{
				int[] atomIndices = polymer.LeadAtomIndices;
				for (int i = monomerCount; --i >= 0; )
				{
					int atomIndex = atomIndices[i];
					if (bsSelected.Get(atomIndex))
						colixes[i] = Graphics3D.setTranslucent(colixes[i], isTranslucent);
				}
			}
			
			//UPGRADE_NOTE: Final was removed from the declaration of 'eightPiSquared100 '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
			private static readonly double eightPiSquared100 = 8 * System.Math.PI * System.Math.PI * 100;
			/// <summary> Calculates the mean positional displacement in milliAngstroms.
			/// <p>
			/// <a href='http://www.rcsb.org/pdb/lists/pdb-l/200303/000609.html'>
			/// http://www.rcsb.org/pdb/lists/pdb-l/200303/000609.html
			/// </a>
			/// <code>
			/// > -----Original Message-----
			/// > From: pdb-l-admin@sdsc.edu [mailto:pdb-l-admin@sdsc.edu] On 
			/// > Behalf Of Philipp Heuser
			/// > Sent: Thursday, March 27, 2003 6:05 AM
			/// > To: pdb-l@sdsc.edu
			/// > Subject: pdb-l: temperature factor; occupancy
			/// > 
			/// > 
			/// > Hi all!
			/// > 
			/// > Does anyone know where to find proper definitions for the 
			/// > temperature factors 
			/// > and the values for occupancy?
			/// > 
			/// > Alright I do know, that the atoms with high temperature 
			/// > factors are more 
			/// > disordered than others, but what does a temperature factor of 
			/// > a specific 
			/// > value mean exactly.
			/// > 
			/// > 
			/// > Thanks in advance!
			/// > 
			/// > Philipp
			/// > 
			/// pdb-l: temperature factor; occupancy
			/// Bernhard Rupp br@llnl.gov
			/// Thu, 27 Mar 2003 08:01:29 -0800
			/// 
			/// * Previous message: pdb-l: temperature factor; occupancy
			/// * Next message: pdb-l: Structural alignment?
			/// * Messages sorted by: [ date ] [ thread ] [ subject ] [ author ]
			/// 
			/// Isotropic B is defined as 8*pi**2<u**2>.
			/// 
			/// Meaning: eight pi squared =79
			/// 
			/// so B=79*mean square displacement (from rest position) of the atom.
			/// 
			/// as u is in Angstrom, B must be in Angstrom squared.
			/// 
			/// example: B=79A**2
			/// 
			/// thus, u=sqrt([79/79]) = 1 A mean positional displacement for atom.
			/// 
			/// 
			/// See also 
			/// 
			/// http://www-structure.llnl.gov/Xray/comp/comp_scat_fac.htm#Atomic
			/// 
			/// for more examples.
			/// 
			/// BR
			/// </code>
			/// 
			/// </summary>
			/// <param name="bFactor100">
			/// </param>
			/// <returns> ?
			/// </returns>
			internal virtual short calcMeanPositionalDisplacement(int bFactor100)
			{
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				return (short) (System.Math.Sqrt(bFactor100 / eightPiSquared100) * 1000);
			}
			
			internal virtual void  findNearestAtomIndex(int xMouse, int yMouse, Closest closest)
			{
				polymer.findNearestAtomIndex(xMouse, yMouse, closest, mads);
			}
		}
	}
}