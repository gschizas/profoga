/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-04-03 03:17:20 +0200 (lun., 03 avr. 2006) $
* $Revision: 4885 $
*
* Copyright (C) 2004-2005  The Jmol Development Team
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
namespace org.jmol.adapter.smarter
{
	
	/// <summary> Reader for Gaussian 94/98/03 output files.
	/// 
	/// 
	/// </summary>
	class GaussianReader:AtomSetCollectionReader
	{
		/// <summary> Interpret the Energy= line for non SCF type energy output
		/// 
		/// </summary>
		/// <param name="line">The input line containing Energy=
		/// </param>
		private System.String Energy
		{
			set
			{
				System.String[] tokens = getTokens(value);
				energyKey = "Energy";
				energyString = tokens[1];
				atomSetCollection.setAtomSetNames("Energy = " + tokens[1], equivalentAtomSets);
			}
			
		}
		
		/// <summary> Word index of atomic number in line with atom coordinates in an
		/// orientation block.
		/// </summary>
		private const int STD_ORIENTATION_ATOMIC_NUMBER_OFFSET = 1;
		/// <summary> Word index of the first X vector of the first frequency in the
		/// frequency output.
		/// </summary>
		private const int FREQ_FIRST_VECTOR_OFFSET = 2;
		
		/// <summary> The default offset for the coordinate output is that for G98 or G03.
		/// If it turns out to be a G94 file, this will be reset.
		/// </summary>
		private int firstCoordinateOffset = 3;
		
		/// <summary>Calculated energy with units (if possible). </summary>
		private System.String energyString = "";
		/// <summary> Type of energy calculated, e.g., E(RB+HF-PW91).</summary>
		private System.String energyKey = "";
		
		/// <summary>The number of the calculation being interpreted. </summary>
		private int calculationNumber = 1;
		
		/// <summary>The scan point, where -1 denotes no scan information. </summary>
		private int scanPoint = - 1;
		
		/// <summary> The number of equivalent atom sets.
		/// <p>Needed to associate identical properties to multiple atomsets
		/// </summary>
		private int equivalentAtomSets = 0;
		
		/// <summary> Reads a Collection of AtomSets from a BufferedReader.
		/// 
		/// <p>New AtomSets are generated when an <code>Input</code>,
		/// <code>Standard</code> or <code>Z-Matrix</code> orientation is read.
		/// The occurence of these orientations seems to depend on (in pseudo-code):
		/// <code>
		/// <br>&nbsp;if (opt=z-matrix) Z-Matrix; else Input;
		/// <br>&nbsp;if (!NoSymmetry) Standard;
		/// </code>
		/// <br>Which means that if <code>NoSymmetry</code> is used with a z-matrix
		/// optimization, no other orientation besides <code>Z-Matrix</code> will be
		/// present.
		/// This is important because <code>Z-Matrix</code> may have dummy atoms while
		/// the analysis of the calculation results will not, i.e., the
		/// <code>Center Numbers</code> in the z-matrix orientation may be different
		/// from those in the population analysis!
		/// 
		/// <p>Single point or frequency calculations always have an
		/// <code>Input</code> orientation. If symmetry is used a
		/// <code>Standard</code> will be present too.
		/// 
		/// </summary>
		/// <param name="reader">BufferedReader associated with the Gaussian output text.
		/// </param>
		/// <returns> The AtomSetCollection representing the interpreted Gaussian text.
		/// </returns>
		/// <throws>  Exception If an error occurs </throws>
		/// <summary> 
		/// </summary>
		internal override AtomSetCollection readAtomSetCollection(System.IO.StreamReader reader)
		{
			
			atomSetCollection = new AtomSetCollection("gaussian");
			
			try
			{
				System.String line;
				int lineNum = 0;
				
				while ((line = reader.ReadLine()) != null)
				{
					if (line.StartsWith(" Step number"))
					{
						equivalentAtomSets = 0;
						// check for scan point information
						int scanPointIndex = line.IndexOf("scan point");
						if (scanPointIndex > 0)
						{
							scanPoint = parseInt(line, scanPointIndex + 10);
						}
						else
						{
							scanPoint = - 1; // no scan point information
						}
					}
					else if (line.IndexOf("-- Stationary point found") > 0)
					{
						// stationary point, if have scanPoint: need to increment now...
						// to get the initial geometry for the next scan point in the proper
						// place
						if (scanPoint >= 0)
							scanPoint++;
					}
					else if (line.IndexOf("Input orientation:") >= 0 || line.IndexOf("Z-Matrix orientation:") >= 0)
					{
						++equivalentAtomSets;
						readAtoms(reader, line);
					}
					else if (line.IndexOf("Standard orientation:") >= 0)
					{
						++equivalentAtomSets;
						readAtoms(reader, line);
					}
					else if (line.StartsWith(" Energy="))
					{
						Energy = line;
					}
					else if (line.StartsWith(" SCF Done:"))
					{
						readSCFDone(reader, line);
					}
					else if (line.StartsWith(" Harmonic frequencies"))
					{
						readFrequencies(reader);
					}
					else if (line.StartsWith(" Total atomic charges:") || line.StartsWith(" Mulliken atomic charges:"))
					{
						// NB this only works for the Standard or Input orientation of
						// the molecule since it does not list the values for the
						// dummy atoms in the z-matrix
						readPartialCharges(reader);
					}
					else if (line.StartsWith(" Normal termination of Gaussian"))
					{
						++calculationNumber;
					}
					else if (lineNum < 25)
					{
						if (line.IndexOf("This is part of the Gaussian 94(TM) system") >= 0 || line.StartsWith(" Gaussian 94:"))
						{
							firstCoordinateOffset = 2;
						}
					}
					lineNum++;
				}
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
		
		/// <summary> Interprets the SCF Done: section.
		/// 
		/// <p>The energyKey and energyString will be set for further AtomSets that have
		/// the same molecular geometry (e.g., frequencies).
		/// The energy, convergence, -V/T and S**2 values will be set as properties
		/// for the atomSet.
		/// 
		/// </summary>
		/// <param name="reader">BufferedReader associated with the Gaussian output text.
		/// </param>
		/// <param name="line">The input line containing SCF Done:.
		/// </param>
		/// <throws>  Exception If an error occurs </throws>
		/// <summary> 
		/// </summary>
		private void  readSCFDone(System.IO.StreamReader reader, System.String line)
		{
			System.String[] tokens = getTokens(line, 11);
			energyKey = tokens[0];
			energyString = tokens[2] + " " + tokens[3];
			// now set the names for the last equivalentAtomSets
			atomSetCollection.setAtomSetNames(energyKey + " = " + energyString, equivalentAtomSets);
			// also set the properties for them
			atomSetCollection.setAtomSetProperties(energyKey, energyString, equivalentAtomSets);
			tokens = getTokens(reader.ReadLine());
			atomSetCollection.setAtomSetProperties(tokens[0], tokens[2], equivalentAtomSets);
			atomSetCollection.setAtomSetProperties(tokens[3], tokens[5], equivalentAtomSets);
			tokens = getTokens(reader.ReadLine());
			atomSetCollection.setAtomSetProperties(tokens[0], tokens[2], equivalentAtomSets);
		}
		
		/* GAUSSIAN STRUCTURAL INFORMATION THAT IS EXPECTED
		NB I currently use the firstCoordinateOffset value to determine where
		X starts, I could use the number of tokens - 3, and read the last 3...
		*/
		
		// GAUSSIAN 04 format
		/*                 Standard orientation:
		----------------------------------------------------------
		Center     Atomic              Coordinates (Angstroms)
		Number     Number             X           Y           Z
		----------------------------------------------------------
		1          6           0.000000    0.000000    1.043880
		##SNIP##    
		---------------------------------------------------------------------
		*/
		
		// GAUSSIAN 98 and 03 format
		/*                    Standard orientation:                         
		---------------------------------------------------------------------
		Center     Atomic     Atomic              Coordinates (Angstroms)
		Number     Number      Type              X           Y           Z
		---------------------------------------------------------------------
		1          6             0        0.852764   -0.020119    0.050711
		##SNIP##
		---------------------------------------------------------------------
		*/
		
		private void  readAtoms(System.IO.StreamReader reader, System.String line)
		{
			atomSetCollection.newAtomSet();
			atomSetCollection.setAtomSetName(""); // start with an empty name
			System.String path = getTokens(line)[0]; // path = type of orientation
			discardLines(reader, 4);
			System.String[] tokens;
			while ((line = reader.ReadLine()) != null && !line.StartsWith(" --"))
			{
				tokens = getTokens(line); // get the tokens in the line
				Atom atom = atomSetCollection.addNewAtom();
				atom.elementNumber = (sbyte) parseInt(tokens[STD_ORIENTATION_ATOMIC_NUMBER_OFFSET]);
				if (atom.elementNumber < 0)
					atom.elementNumber = 0; // dummy atoms have -1 -> 0
				int offset = firstCoordinateOffset;
				atom.x = parseFloat(tokens[offset]);
				atom.y = parseFloat(tokens[++offset]);
				atom.z = parseFloat(tokens[++offset]);
			}
			atomSetCollection.setAtomSetProperty(SmarterJmolAdapter.PATH_KEY, "Calculation " + calculationNumber + (scanPoint >= 0?(SmarterJmolAdapter.PATH_SEPARATOR + "Scan Point " + scanPoint):"") + SmarterJmolAdapter.PATH_SEPARATOR + path);
			// always make sure that I have a name for the atomset
			// mostly needed if no "SCF Done" line follows the structure (e.g., last one
			// in a scan...)
			atomSetCollection.setAtomSetName("Last read atomset.");
		}
		
		/* SAMPLE FREQUENCY OUTPUT */
		/*
		Harmonic frequencies (cm**-1), IR intensities (KM/Mole), Raman scattering
		activities (A**4/AMU), depolarization ratios for plane and unpolarized
		incident light, reduced masses (AMU), force constants (mDyne/A),
		and normal coordinates:
		1                      2                      3
		A1                     B2                     B1
		Frequencies --    64.6809                64.9485               203.8241
		Red. masses --     8.0904                 2.2567                 1.0164
		Frc consts  --     0.0199                 0.0056                 0.0249
		IR Inten    --     1.4343                 1.4384                15.8823
		Atom AN      X      Y      Z        X      Y      Z        X      Y      Z
		1   6     0.00   0.00   0.48     0.00  -0.05   0.23     0.01   0.00   0.00
		2   6     0.00   0.00   0.48     0.00  -0.05  -0.23     0.01   0.00   0.00
		3   1     0.00   0.00   0.49     0.00  -0.05   0.63     0.03   0.00   0.00
		4   1     0.00   0.00   0.49     0.00  -0.05  -0.63     0.03   0.00   0.00
		5   1     0.00   0.00  -0.16     0.00  -0.31   0.00    -1.00   0.00   0.00
		6  35     0.00   0.00  -0.16     0.00   0.02   0.00     0.01   0.00   0.00
		##SNIP##
		10                     11                     12
		A1                     B2                     A1
		Frequencies --  2521.0940              3410.1755              3512.0957
		Red. masses --     1.0211                 1.0848                 1.2333
		Frc consts  --     3.8238                 7.4328                 8.9632
		IR Inten    --   264.5877               109.0525                 0.0637
		Atom AN      X      Y      Z        X      Y      Z        X      Y      Z
		1   6     0.00   0.00   0.00     0.00   0.06   0.00     0.00  -0.10   0.00
		2   6     0.00   0.00   0.00     0.00   0.06   0.00     0.00   0.10   0.00
		3   1     0.00   0.01   0.00     0.00  -0.70   0.01     0.00   0.70  -0.01
		4   1     0.00  -0.01   0.00     0.00  -0.70  -0.01     0.00  -0.70  -0.01
		5   1     0.00   0.00   1.00     0.00   0.00   0.00     0.00   0.00   0.00
		6  35     0.00   0.00  -0.01     0.00   0.00   0.00     0.00   0.00   0.00
		
		-------------------
		- Thermochemistry -
		-------------------
		*/
		
		/// <summary> Interprets the Harmonic frequencies section.
		/// 
		/// <p>The vectors are added to a clone of the last read AtomSet.
		/// Only the Frequencies, reduced masses, force constants and IR intensities
		/// are set as properties for each of the frequency type AtomSet generated.
		/// 
		/// </summary>
		/// <param name="reader">BufferedReader associated with the Gaussian output text.
		/// </param>
		/// <throws>  Exception If no frequences were encountered </throws>
		/// <throws>  IOException If an I/O error occurs </throws>
		/// <summary> 
		/// </summary>
		private void  readFrequencies(System.IO.StreamReader reader)
		{
			System.String line;
			System.String[] tokens; System.String[] symmetries; System.String[] frequencies;
			System.String[] red_masses; System.String[] frc_consts; System.String[] intensities;
			
			while ((line = reader.ReadLine()) != null && line.IndexOf(":") < 0)
			{
			}
			if (line == null)
				throw (new System.Exception("No frequencies encountered"));
			
			// G98 ends the frequencies with a line with a space (03 an empty line)
			// so I decided to read till the line is too short
			while ((line = reader.ReadLine()) != null && line.Length > 15)
			{
				// we now have the line with the vibration numbers in them, but don't need it
				symmetries = getTokens(reader.ReadLine()); // read symmetry labels
				// TODO I should really read all the properties of the vibrations listed
				// and not limit myself to only IR type ones..
				frequencies = getTokens(discardLinesUntilStartsWith(reader, " Frequencies"), 15);
				red_masses = getTokens(discardLinesUntilStartsWith(reader, " Red. masses"), 15);
				frc_consts = getTokens(discardLinesUntilStartsWith(reader, " Frc consts"), 15);
				intensities = getTokens(discardLinesUntilStartsWith(reader, " IR Inten"), 15);
				
				int frequencyCount = frequencies.Length;
				
				for (int i = 0; i < frequencyCount; ++i)
				{
					atomSetCollection.cloneLastAtomSet();
					atomSetCollection.setAtomSetName(symmetries[i] + " " + frequencies[i] + " cm**-1");
					// set the properties
					atomSetCollection.setAtomSetProperty(energyKey, energyString);
					atomSetCollection.setAtomSetProperty("Frequency", frequencies[i] + " cm**-1");
					atomSetCollection.setAtomSetProperty("Reduced Mass", red_masses[i] + " AMU");
					atomSetCollection.setAtomSetProperty("Force Constant", frc_consts[i] + " mDyne/A");
					atomSetCollection.setAtomSetProperty("IR Intensity", intensities[i] + " KM/Mole");
					atomSetCollection.setAtomSetProperty(SmarterJmolAdapter.PATH_KEY, "Calculation " + calculationNumber + SmarterJmolAdapter.PATH_SEPARATOR + "Frequencies");
				}
				
				int atomCount = atomSetCollection.LastAtomSetAtomCount;
				int firstModelAtom = atomSetCollection.atomCount - frequencyCount * atomCount;
				
				// position to start reading the displacement vectors
				discardLinesUntilStartsWith(reader, " Atom AN");
				
				// read the displacement vectors for every atom and frequency
				float x, y, z;
				for (int i = 0; i < atomCount; ++i)
				{
					tokens = getTokens(reader.ReadLine());
					int atomCenterNumber = parseInt(tokens[0]);
					for (int j = 0, offset = FREQ_FIRST_VECTOR_OFFSET; j < frequencyCount; ++j)
					{
						int atomOffset = firstModelAtom + j * atomCount + atomCenterNumber - 1;
						Atom atom = atomSetCollection.atoms[atomOffset];
						x = parseFloat(tokens[offset++]);
						y = parseFloat(tokens[offset++]);
						z = parseFloat(tokens[offset++]);
						atom.addVibrationVector(x, y, z);
					}
				}
			}
		}
		
		/* SAMPLE Mulliken Charges OUTPUT from G98 */
		/*
		Mulliken atomic charges:
		1
		1  C   -0.238024
		2  C   -0.238024
		###SNIP###
		6  Br  -0.080946
		Sum of Mulliken charges=   0.00000
		*/
		
		/// <summary> Reads partial charges and assigns them only to the last atom set. </summary>
		/// <param name="reader">The reader from which to read the charges
		/// </param>
		/// <throws>  Exception When an I/O error or discardlines error occurs </throws>
		// TODO this really should set the charges for the last nOrientations read
		// being careful about the dummy atoms...
		internal virtual void  readPartialCharges(System.IO.StreamReader reader)
		{
			discardLines(reader, 1);
			for (int i = atomSetCollection.LastAtomSetAtomIndex; i < atomSetCollection.atomCount; ++i)
			{
				// first skip over the dummy atoms
				while (atomSetCollection.atoms[i].elementNumber == 0)
					++i;
				// assign the partial charge
				atomSetCollection.atoms[i].partialCharge = parseFloat(getTokens(reader.ReadLine())[2]);
			}
		}
	}
}