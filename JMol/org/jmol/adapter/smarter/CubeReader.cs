/* $RCSfile$
* $Author: egonw $
* $Date: 2005-11-10 16:52:44 +0100 (jeu., 10 nov. 2005) $
* $Revision: 4255 $
*
* Copyright (C) 2005  Miguel, Jmol Development, www.jmol.org
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
	
	/// <summary> Gaussian cube file format
	/// 
	/// http://www.cup.uni-muenchen.de/oc/zipse/lv18099/orb_MOLDEN.html
	/// this is good because it is source code
	/// http://ftp.ccl.net/cca/software/SOURCES/C/scarecrow/gcube2plt.c
	/// 
	/// http://www.nersc.gov/nusers/resources/software/apps/chemistry/gaussian/g98/00000430.htm
	/// this contains some erroneous info
	/// http://astronomy.swin.edu.au/~pbourke/geomformats/cube/
	/// Miguel 2005 07 04
	/// BUT, the files that I have do not comply with this format
	/// because they have a negative atom count and an extra line
	/// We will assume that there was a file format change, denoted by
	/// the negative atom count.
	/// 
	/// seems that distances are in Bohrs
	/// 
	/// Miguel 2005 07 17
	/// first two URLs above explain that a negative atom count means
	/// that it is molecular orbital (MO) data
	/// with MO data, the extra line contains the number
	/// of orbitals and the orbital number
	/// we only support # of orbitals == 1
	/// if # of orbitals were > 1 then there would be multiple data
	/// points in each cell
	/// </summary>
	
	class CubeReader:AtomSetCollectionReader
	{
		
		internal System.IO.StreamReader br;
		internal bool negativeAtomCount;
		internal int atomCount;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'voxelCounts '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal int[] voxelCounts = new int[3];
		//UPGRADE_NOTE: Final was removed from the declaration of 'origin '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal float[] origin = new float[3];
		//UPGRADE_NOTE: Final was removed from the declaration of 'voxelVectors '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal float[][] voxelVectors = new float[3][];
		internal float[][][] voxelData;
		
		internal override AtomSetCollection readAtomSetCollection(System.IO.StreamReader br)
		{
			
			this.br = br;
			atomSetCollection = new AtomSetCollection("cube");
			try
			{
				atomSetCollection.newAtomSet();
				readTitleLines();
				readAtomCountAndOrigin();
				readVoxelVectors();
				readAtoms();
				/*
				volumetric data is no longer read here
				readExtraLine();
				readVoxelData();
				atomSetCollection.volumetricOrigin = origin;
				atomSetCollection.volumetricSurfaceVectors = voxelVectors;
				atomSetCollection.volumetricSurfaceData = voxelData;
				*/
			}
			catch (System.Exception ex)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				atomSetCollection.errorMessage = "Could not read file:" + ex;
			}
			return atomSetCollection;
		}
		
		internal virtual void  readTitleLines()
		{
			System.String title;
			title = br.ReadLine().Trim() + " - ";
			title += br.ReadLine().Trim();
			atomSetCollection.setAtomSetName(title);
		}
		
		internal virtual void  readAtomCountAndOrigin()
		{
			System.String line = br.ReadLine();
			atomCount = parseInt(line);
			origin[0] = parseFloat(line, ichNextParse);
			origin[1] = parseFloat(line, ichNextParse);
			origin[2] = parseFloat(line, ichNextParse);
			if (atomCount < 0)
			{
				atomCount = - atomCount;
				negativeAtomCount = true;
			}
		}
		
		internal virtual void  readVoxelVectors()
		{
			readVoxelVector(0);
			readVoxelVector(1);
			readVoxelVector(2);
		}
		
		internal virtual void  readVoxelVector(int voxelVectorIndex)
		{
			System.String line = br.ReadLine();
			float[] voxelVector = new float[3];
			voxelVectors[voxelVectorIndex] = voxelVector;
			voxelCounts[voxelVectorIndex] = parseInt(line);
			voxelVector[0] = parseFloat(line, ichNextParse);
			voxelVector[1] = parseFloat(line, ichNextParse);
			voxelVector[2] = parseFloat(line, ichNextParse);
		}
		
		internal virtual void  readAtoms()
		{
			for (int i = 0; i < atomCount; ++i)
			{
				System.String line = br.ReadLine();
				Atom atom = atomSetCollection.addNewAtom();
				atom.elementNumber = (sbyte) parseInt(line);
				atom.partialCharge = parseFloat(line, ichNextParse);
				atom.x = parseFloat(line, ichNextParse) * ANGSTROMS_PER_BOHR;
				atom.y = parseFloat(line, ichNextParse) * ANGSTROMS_PER_BOHR;
				atom.z = parseFloat(line, ichNextParse) * ANGSTROMS_PER_BOHR;
			}
		}
		
		internal virtual void  readExtraLine()
		{
			if (negativeAtomCount)
				br.ReadLine();
		}
		
		/*
		void readVoxelData() throws Exception {
		System.out.println("entering readVoxelData");
		String line = "";
		ichNextParse = 0;
		int voxelCountX = voxelCounts[0];
		int voxelCountY = voxelCounts[1];
		int voxelCountZ = voxelCounts[2];
		voxelData = new float[voxelCountX][][];
		for (int x = 0; x < voxelCountX; ++x) {
		float[][] plane = new float[voxelCountY][];
		voxelData[x] = plane;
		for (int y = 0; y < voxelCountY; ++y) {
		float[] strip = new float[voxelCountZ];
		plane[y] = strip;
		for (int z = 0; z < voxelCountZ; ++z) {
		float voxelValue = parseFloat(line, ichNextParse);
		if (Float.isNaN(voxelValue)) {
		line = br.readLine();
		if (line == null || Float.isNaN(voxelValue = parseFloat(line))) {
		System.out.println("end of file in CubeReader?");
		throw new NullPointerException();
		}
		}
		strip[z] = voxelValue;
		}
		}
		}
		System.out.println("Successfully read " + voxelCountX +
		" x " + voxelCountY +
		" x " + voxelCountZ + " voxels");
		}
		*/
	}
}