/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-03-27 17:05:25 +0200 (lun., 27 mars 2006) $
* $Revision: 4770 $
*
* Copyright (C) 2005 Miguel, Jmol Development
*
* Contact: miguel@jmol.org,jmol-developers@lists.sourceforge.net
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

/*
* miguel 2005 07 17
*
*  System and method for the display of surface structures
*  contained within the interior region of a solid body
* United States Patent Number 4,710,876
* Granted: Dec 1, 1987
* Inventors:  Cline; Harvey E. (Schenectady, NY);
*             Lorensen; William E. (Ballston Lake, NY)
* Assignee: General Electric Company (Schenectady, NY)
* Appl. No.: 741390
* Filed: June 5, 1985
*
*
* Patents issuing prior to June 8, 1995 can last up to 17
* years from the date of issuance.
*
* Dec 1 1987 + 17 yrs = Dec 1 2004
*/
using System;
using org.jmol.g3d;
//UPGRADE_TODO: The type 'javax.vecmath.Point3f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Point3f = javax.vecmath.Point3f;
//UPGRADE_TODO: The type 'javax.vecmath.Point3i' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Point3i = javax.vecmath.Point3i;
//UPGRADE_TODO: The type 'javax.vecmath.Vector3f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Vector3f = javax.vecmath.Vector3f;
//UPGRADE_TODO: The type 'javax.vecmath.Matrix3f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Matrix3f = javax.vecmath.Matrix3f;
namespace org.jmol.viewer
{
	
	class Isosurface:MeshCollection
	{
		public Isosurface()
		{
			InitBlock();
		}
		private void  InitBlock()
		{
			ANGSTROMS_PER_BOHR = JmolConstants.ANGSTROMS_PER_BOHR;
			for (int i = 3; --i >= 0; )
				volumetricVectors[i] = new Vector3f();
			for (int i = 3; --i >= 0; )
				unitVolumetricVectors[i] = new Vector3f();
			for (int i = 12; --i >= 0; )
				surfacePoints[i] = new Point3f();
		}
		virtual internal short DefaultColix
		{
			get
			{
				int argb;
				if (cutoff >= 0)
				{
					indexColorPositive = (indexColorPositive % JmolConstants.argbsIsosurfacePositive.Length);
					argb = JmolConstants.argbsIsosurfacePositive[indexColorPositive++];
				}
				else
				{
					indexColorNegative = (indexColorNegative % JmolConstants.argbsIsosurfaceNegative.Length);
					argb = JmolConstants.argbsIsosurfaceNegative[indexColorNegative++];
				}
				return Graphics3D.getColix(argb);
			}
			
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'ANGSTROMS_PER_BOHR '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_NOTE: The initialization of  'ANGSTROMS_PER_BOHR' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		internal float ANGSTROMS_PER_BOHR;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'volumetricOrigin '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Point3f volumetricOrigin = new Point3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'volumetricVectors '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f[] volumetricVectors = new Vector3f[3];
		//UPGRADE_NOTE: Final was removed from the declaration of 'unitVolumetricVectors '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f[] unitVolumetricVectors = new Vector3f[3];
		//UPGRADE_NOTE: Final was removed from the declaration of 'volumetricVectorLengths '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal float[] volumetricVectorLengths = new float[3];
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'voxelCounts '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal int[] voxelCounts = new int[3];
		//UPGRADE_NOTE: Final was removed from the declaration of 'volumetricMatrix '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Matrix3f volumetricMatrix = new Matrix3f();
		internal float[][][] voxelData;
		
		internal int edgePointCount = 0;
		internal Point3f[] edgePoints;
		
		internal float cutoff = 0.02f;
		internal bool rangeDefined = false;
		internal float minRange, maxRange;
		
		internal override void  setProperty(System.String propertyName, System.Object value_Renamed, System.Collections.BitArray bs)
		{
			if ((System.Object) "bufferedreader" == (System.Object) propertyName)
			{
				System.IO.StreamReader br = (System.IO.StreamReader) value_Renamed;
				if (currentMesh == null)
					allocMesh(null);
				currentMesh.clear();
				readVolumetricHeader(br);
				calcVolumetricMatrix();
				readVolumetricData(br);
				calcVoxelVertexVectors();
				constructTessellatedSurface();
				currentMesh.colix = DefaultColix;
				currentMesh.initialize();
				currentMesh.checkForDuplicatePoints(.001f);
				currentMesh.visible = true;
				discardTempData();
				return ;
			}
			if ((System.Object) "cutoff" == (System.Object) propertyName)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				cutoff = (float) ((System.Single) value_Renamed);
				return ;
			}
			if ((System.Object) "rangeMin" == (System.Object) propertyName)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				minRange = (float) ((System.Single) value_Renamed);
				return ;
			}
			if ((System.Object) "rangeMax" == (System.Object) propertyName)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				maxRange = (float) ((System.Single) value_Renamed);
				rangeDefined = true;
				return ;
			}
			if ((System.Object) "removeRange" == (System.Object) propertyName)
			{
				rangeDefined = false;
				return ;
			}
			if ((System.Object) "colorreader" == (System.Object) propertyName)
			{
				System.IO.StreamReader br = (System.IO.StreamReader) value_Renamed;
				System.Console.Out.WriteLine("colorreader seen!");
				readVolumetricHeader(br);
				calcVolumetricMatrix();
				readVolumetricData(br);
				if (!rangeDefined)
				{
					minRange = getMinMappedValue();
					maxRange = getMaxMappedValue();
				}
				System.Console.Out.WriteLine(" minRange=" + minRange + " maxRange=" + maxRange);
				applyColorScale(minRange, maxRange, "roygb");
				discardTempData();
				return ;
			}
			base.setProperty(propertyName, value_Renamed, bs);
		}
		
		internal virtual void  calcVolumetricMatrix()
		{
			for (int i = 3; --i >= 0; )
				volumetricMatrix.setColumn(i, volumetricVectors[i]);
		}
		
		internal virtual void  discardTempData()
		{
			edgePointCount = 0;
			edgePoints = null;
			voxelData = null;
		}
		
		/*
		System.out.println("setProperty(" + propertyName + "," + value + ")");
		if ("load" == propertyName) {
		volumetricOrigin = new Point3f((float[])((Object[])value)[0]);
		float[][] vvectors = (float[][])((Object[])value)[1];
		for (int i = 3; --i >= 0; ) {
		volumetricVectors[i] = new Vector3f(vvectors[i]);
		}
		volumetricData = (float[][][])((Object[])value)[2];
		
		calcVoxelVertexVectors();
		constructTessellatedSurface();
		mesh.initialize();
		return;
		}
		*/
		
		////////////////////////////////////////////////////////////////
		// default color stuff
		internal int indexColorPositive;
		internal int indexColorNegative;
		////////////////////////////////////////////////////////////////
		
		////////////////////////////////////////////////////////////////
		// file reading stuff
		////////////////////////////////////////////////////////////////
		
		internal virtual void  readVolumetricHeader(System.IO.StreamReader br)
		{
			try
			{
				readTitleLines(br);
				readAtomCountAndOrigin(br);
				readVoxelVectors(br);
				readAtoms(br);
				readExtraLine(br);
			}
			catch (System.Exception e)
			{
				SupportClass.WriteStackTrace(e, Console.Error);
				throw new System.NullReferenceException();
			}
		}
		
		internal virtual void  readVolumetricData(System.IO.StreamReader br)
		{
			try
			{
				readVoxelData(br);
			}
			catch (System.Exception e)
			{
				SupportClass.WriteStackTrace(e, Console.Error);
				throw new System.NullReferenceException();
			}
		}
		
		internal virtual void  readTitleLines(System.IO.StreamReader br)
		{
			System.String title;
			title = br.ReadLine().Trim() + " - ";
			title += br.ReadLine().Trim();
		}
		
		internal int atomCount;
		internal bool negativeAtomCount;
		
		internal virtual void  readAtomCountAndOrigin(System.IO.StreamReader br)
		{
			System.String line = br.ReadLine();
			atomCount = parseInt(line);
			volumetricOrigin.x = parseFloat(line, ichNextParse);
			volumetricOrigin.y = parseFloat(line, ichNextParse);
			volumetricOrigin.z = parseFloat(line, ichNextParse);
			volumetricOrigin.scale(ANGSTROMS_PER_BOHR);
			if (atomCount < 0)
			{
				atomCount = - atomCount;
				negativeAtomCount = true;
			}
		}
		
		internal virtual void  readVoxelVectors(System.IO.StreamReader br)
		{
			for (int i = 0; i < 3; ++i)
				readVoxelVector(br, i);
		}
		
		internal virtual void  readVoxelVector(System.IO.StreamReader br, int voxelVectorIndex)
		{
			System.String line = br.ReadLine();
			Vector3f voxelVector = volumetricVectors[voxelVectorIndex];
			voxelCounts[voxelVectorIndex] = parseInt(line);
			voxelVector.x = parseFloat(line, ichNextParse);
			voxelVector.y = parseFloat(line, ichNextParse);
			voxelVector.z = parseFloat(line, ichNextParse);
			voxelVector.scale(ANGSTROMS_PER_BOHR);
			volumetricVectorLengths[voxelVectorIndex] = voxelVector.length();
			unitVolumetricVectors[voxelVectorIndex].normalize(voxelVector);
		}
		
		internal virtual void  readAtoms(System.IO.StreamReader br)
		{
			for (int i = 0; i < atomCount; ++i)
			{
				/*String line = */ br.ReadLine();
				/*
				Atom atom = atomSetCollection.addNewAtom();
				atom.elementNumber = (byte)parseInt(line);
				atom.partialCharge = parseFloat(line, ichNextParse);
				atom.x = parseFloat(line, ichNextParse) * ANGSTROMS_PER_BOHR;
				atom.y = parseFloat(line, ichNextParse) * ANGSTROMS_PER_BOHR;
				atom.z = parseFloat(line, ichNextParse) * ANGSTROMS_PER_BOHR;
				*/
			}
		}
		
		internal virtual void  readExtraLine(System.IO.StreamReader br)
		{
			if (negativeAtomCount)
				br.ReadLine();
		}
		
		internal virtual void  readVoxelData(System.IO.StreamReader br)
		{
			System.Console.Out.WriteLine("entering readVoxelData");
			System.String line = "";
			ichNextParse = 0;
			int voxelCountX = voxelCounts[0];
			int voxelCountY = voxelCounts[1];
			int voxelCountZ = voxelCounts[2];
			voxelData = new float[voxelCountX][][];
			for (int x = 0; x < voxelCountX; ++x)
			{
				float[][] plane = new float[voxelCountY][];
				voxelData[x] = plane;
				for (int y = 0; y < voxelCountY; ++y)
				{
					float[] strip = new float[voxelCountZ];
					plane[y] = strip;
					for (int z = 0; z < voxelCountZ; ++z)
					{
						float voxelValue = parseFloat(line, ichNextParse);
						if (System.Single.IsNaN(voxelValue))
						{
							line = br.ReadLine();
							if (line == null || System.Single.IsNaN(voxelValue = parseFloat(line)))
							{
								System.Console.Out.WriteLine("end of file in CubeReader?");
								throw new System.NullReferenceException();
							}
						}
						strip[z] = voxelValue;
					}
				}
			}
			System.Console.Out.WriteLine("Successfully read " + voxelCountX + " x " + voxelCountY + " x " + voxelCountZ + " voxels");
		}
		
		////////////////////////////////////////////////////////////////
		// marching cube stuff
		////////////////////////////////////////////////////////////////
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'vertexValues '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal float[] vertexValues = new float[8];
		//UPGRADE_NOTE: Final was removed from the declaration of 'surfacePoints '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Point3f[] surfacePoints = new Point3f[12];
		//UPGRADE_NOTE: Final was removed from the declaration of 'surfacePointIndexes '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal int[] surfacePointIndexes = new int[12];
		
		internal int voxelCountX, voxelCountY, voxelCountZ;
		
		internal virtual void  constructTessellatedSurface()
		{
			voxelCountX = voxelData.Length - 1;
			voxelCountY = voxelData[0].Length - 1;
			voxelCountZ = voxelData[0][0].Length - 1;
			
			int[][] isoPointIndexes = new int[voxelCountY * voxelCountZ][];
			for (int i2 = 0; i2 < voxelCountY * voxelCountZ; i2++)
			{
				isoPointIndexes[i2] = new int[12];
			}
			for (int i = voxelCountY * voxelCountZ; --i >= 0; )
				isoPointIndexes[i] = new int[12];
			
			/*
			for (int x = 0; x < voxelCountX; ++x)
			for (int y = 0; y < voxelCountY; ++y)
			for (int z = 0; z < voxelCountZ; ++z)
			System.out.println("" + x + "," + y + "," + z + " = " +
			voxelData[x][y][z]);
			*/
			
			int insideCount = 0, outsideCount = 0, surfaceCount = 0;
			for (int x = voxelCountX; --x >= 0; )
			{
				for (int y = voxelCountY; --y >= 0; )
				{
					for (int z = voxelCountZ; --z >= 0; )
					{
						int insideMask = 0;
						for (int i = 8; --i >= 0; )
						{
							Point3i offset = cubeVertexOffsets[i];
							float vertexValue = voxelData[x + offset.x][y + offset.y][z + offset.z];
							vertexValues[i] = vertexValue;
							if ((cutoff > 0 && vertexValue >= cutoff) || (cutoff < 0 && vertexValue <= cutoff))
								insideMask |= 1 << i;
						}
						
						/*
						for (int i = 0; i < 8; ++i )
						System.out.println("vertexValues[" + i + "]=" +
						vertexValues[i]);
						System.out.println("insideMask=" + Integer.toHexString(insideMask));
						*/
						
						if (insideMask == 0)
						{
							++outsideCount;
							continue;
						}
						if (insideMask == 0xFF)
						{
							++insideCount;
							continue;
						}
						++surfaceCount;
						calcVoxelOrigin(x, y, z);
						int[] voxelPointIndexes = propogateNeighborPointIndexes(x, y, z, isoPointIndexes);
						processOneVoxel(insideMask, cutoff, voxelPointIndexes);
					}
				}
			}
			System.Console.Out.WriteLine("volumetric=" + voxelCountX + "," + voxelCountY + "," + voxelCountZ + "," + " total=" + (voxelCountX * voxelCountY * voxelCountZ) + "\n" + " insideCount=" + insideCount + " outsideCount=" + outsideCount + " surfaceCount=" + surfaceCount + " total=" + (insideCount + outsideCount + surfaceCount));
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'nullNeighbor '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal int[] nullNeighbor = new int[]{- 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1};
		
		internal virtual int[] propogateNeighborPointIndexes(int x, int y, int z, int[][] isoPointIndexes)
		{
			int cellIndex = y * voxelCountZ + z;
			int[] voxelPointIndexes = isoPointIndexes[cellIndex];
			
			bool noXNeighbor = (x == voxelCountX - 1);
			// the x neighbor is myself from my last pass through here
			if (noXNeighbor)
			{
				voxelPointIndexes[1] = - 1;
				voxelPointIndexes[9] = - 1;
				voxelPointIndexes[5] = - 1;
				voxelPointIndexes[10] = - 1;
			}
			else
			{
				voxelPointIndexes[1] = voxelPointIndexes[3];
				voxelPointIndexes[9] = voxelPointIndexes[8];
				voxelPointIndexes[5] = voxelPointIndexes[7];
				voxelPointIndexes[10] = voxelPointIndexes[11];
			}
			
			//from the y neighbor pick up the top
			bool noYNeighbor = (y == voxelCountY - 1);
			int[] yNeighbor = noYNeighbor?nullNeighbor:isoPointIndexes[cellIndex + voxelCountZ];
			
			voxelPointIndexes[6] = yNeighbor[2];
			voxelPointIndexes[7] = yNeighbor[3];
			voxelPointIndexes[4] = yNeighbor[0];
			if (noXNeighbor)
				voxelPointIndexes[5] = yNeighbor[1];
			
			// from my z neighbor
			bool noZNeighbor = (z == voxelCountZ - 1);
			int[] zNeighbor = noZNeighbor?nullNeighbor:isoPointIndexes[cellIndex + 1];
			
			voxelPointIndexes[2] = zNeighbor[0];
			voxelPointIndexes[11] = zNeighbor[8];
			if (noYNeighbor)
				voxelPointIndexes[6] = zNeighbor[4];
			if (noXNeighbor)
				voxelPointIndexes[10] = zNeighbor[9];
			
			// these must always be calculated
			voxelPointIndexes[0] = - 1;
			voxelPointIndexes[3] = - 1;
			voxelPointIndexes[8] = - 1;
			
			return voxelPointIndexes;
		}
		
		internal virtual void  dump(int[] pointIndexes)
		{
			for (int i = 0; i < 12; ++i)
				System.Console.Out.WriteLine(" " + i + ":" + pointIndexes[i]);
		}
		
		internal virtual void  processOneVoxel(int insideMask, float cutoff, int[] voxelPointIndexes)
		{
			int edgeMask = edgeMaskTable[insideMask];
			for (int iEdge = 12; --iEdge >= 0; )
			{
				if ((edgeMask & (1 << iEdge)) == 0)
					continue;
				if (voxelPointIndexes[iEdge] >= 0)
					continue; // propogated from neighbor
				int vertexA = edgeVertexes[2 * iEdge];
				int vertexB = edgeVertexes[2 * iEdge + 1];
				float valueA = vertexValues[vertexA];
				float valueB = vertexValues[vertexB];
				calcVertexPoints(vertexA, vertexB);
				addEdgePoint(pointA);
				addEdgePoint(pointB);
				calcSurfacePoint(cutoff, valueA, valueB, surfacePoints[iEdge]);
				voxelPointIndexes[iEdge] = currentMesh.addVertexCopy(surfacePoints[iEdge]);
			}
			
			sbyte[] triangles = triangleTable[insideMask];
			for (int i = triangles.Length; (i -= 3) >= 0; )
				currentMesh.addTriangle(voxelPointIndexes[triangles[i]], voxelPointIndexes[triangles[i + 1]], voxelPointIndexes[triangles[i + 2]]);
		}
		
		internal virtual void  calcSurfacePoint(float cutoff, float valueA, float valueB, Point3f surfacePoint)
		{
			float diff = valueB - valueA;
			float fraction = (cutoff - valueA) / diff;
			if (System.Single.IsNaN(fraction) || fraction < 0 || fraction > 1)
			{
				System.Console.Out.WriteLine("fraction=" + fraction + " cutoff=" + cutoff + " A:" + valueA + " B:" + valueB);
				throw new System.IndexOutOfRangeException();
			}
			
			edgeVector.sub(pointB, pointA);
			surfacePoint.scaleAdd(fraction, edgeVector, pointA);
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'voxelOrigin '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Point3f voxelOrigin = new Point3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'voxelT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Point3f voxelT = new Point3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'pointA '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Point3f pointA = new Point3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'pointB '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Point3f pointB = new Point3f();
		// edgeVector should be a table lookup based upon edge number
		// vectors should be derived from the volumetric vectors in the file
		//UPGRADE_NOTE: Final was removed from the declaration of 'edgeVector '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f edgeVector = new Vector3f();
		
		internal virtual void  calcVertexPoints(int vertexA, int vertexB)
		{
			pointA.add(voxelOrigin, voxelVertexVectors[vertexA]);
			pointB.add(voxelOrigin, voxelVertexVectors[vertexB]);
			/*
			System.out.println("calcVertexPoints(" + vertexA + "," + vertexB + ")\n" +
			" pointA=" + pointA +
			" pointB=" + pointB);
			*/
		}
		
		internal virtual void  calcVoxelOrigin(int x, int y, int z)
		{
			voxelOrigin.scaleAdd(x, volumetricVectors[0], volumetricOrigin);
			voxelOrigin.scaleAdd(y, volumetricVectors[1], voxelOrigin);
			voxelOrigin.scaleAdd(z, volumetricVectors[2], voxelOrigin);
			/*
			System.out.println("voxelOrigin=" + voxelOrigin);
			*/
		}
		
		internal virtual void  addEdgePoint(Point3f point)
		{
			if (edgePoints == null)
				edgePoints = new Point3f[256];
			else if (edgePointCount == edgePoints.length)
				edgePoints = (Point3f[]) Util.doubleLength(edgePoints);
			edgePoints[edgePointCount++] = new Point3f(point);
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'cubeVertexOffsets '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly Point3i[] cubeVertexOffsets = new Point3i[]{new Point3i(0, 0, 0), new Point3i(1, 0, 0), new Point3i(1, 0, 1), new Point3i(0, 0, 1), new Point3i(0, 1, 0), new Point3i(1, 1, 0), new Point3i(1, 1, 1), new Point3i(0, 1, 1)};
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'cubeVertexVectors '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly Vector3f[] cubeVertexVectors = new Vector3f[]{new Vector3f(0, 0, 0), new Vector3f(1, 0, 0), new Vector3f(1, 0, 1), new Vector3f(0, 0, 1), new Vector3f(0, 1, 0), new Vector3f(1, 1, 0), new Vector3f(1, 1, 1), new Vector3f(0, 1, 1)};
		
		internal static Vector3f[] voxelVertexVectors = new Vector3f[8];
		
		internal virtual void  calcVoxelVertexVectors()
		{
			for (int i = 8; --i >= 0; )
				voxelVertexVectors[i] = calcVoxelVertexVector(cubeVertexVectors[i]);
			for (int i = 0; i < 8; ++i)
			{
				System.Console.Out.WriteLine("voxelVertexVectors[" + i + "]=" + voxelVertexVectors[i]);
			}
		}
		
		internal virtual Vector3f calcVoxelVertexVector(Vector3f cubeVectors)
		{
			Vector3f v = new Vector3f();
			volumetricMatrix.transform(cubeVectors, v);
			return v;
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'edgeVertexes'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly sbyte[] edgeVertexes = new sbyte[]{0, 1, 1, 2, 2, 3, 3, 0, 4, 5, 5, 6, 6, 7, 7, 4, 0, 4, 1, 5, 2, 6, 3, 7};
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'edgeMaskTable'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly short[] edgeMaskTable = new short[]{(short) (0x0000), (short) (0x0109), (short) (0x0203), (short) (0x030A), (short) (0x0406), (short) (0x050F), (short) (0x0605), (short) (0x070C), (short) (0x080C), (short) (0x0905), (short) (0x0A0F), (short) (0x0B06), (short) (0x0C0A), (short) (0x0D03), (short) (0x0E09), (short) (0x0F00), (short) (0x0190), (short) (0x0099), (short) (0x0393), (short) (0x029A), (short) (0x0596), (short) (0x049F), (short) (0x0795), (short) (0x069C), (short) (0x099C), (short) (0x0895), (short) (0x0B9F), (short) (0x0A96), (short) (0x0D9A), (short) (0x0C93), (short) (0x0F99), (short) (0x0E90), (short) (0x0230), (short) (0x0339), (short) (0x0033), (short) (0x013A), (short) (0x0636), (short) (0x073F), (short) (0x0435), (short) (0x053C), (short) (0x0A3C), (short) (0x0B35), (short) (0x083F), (short) (0x0936), (short) (0x0E3A), (short) (0x0F33), (short) (0x0C39), (short) (0x0D30), (short) (0x03A0), (short) (0x02A9), (short) (0x01A3), (short) (0x00AA), (short) (0x07A6), (short) (0x06AF), (short) (0x05A5), (short) (0x04AC), (short) (0x0BAC), (short) (0x0AA5), (short) (0x09AF), (short) (0x08A6), (short) (0x0FAA), (short) (0x0EA3), (short) (0x0DA9), (short) (0x0CA0), (short) (0x0460), (short) (0x0569), (short) (0x0663), (short) (0x076A), (short) (0x0066), (short) (0x016F), (short) (0x0265), (short) (0x036C), (short) (0x0C6C), (short) (0x0D65), (short) (0x0E6F), (short) (0x0F66), (short) (0x086A), (short) (0x0963), (short) (0x0A69), (short) (0x0B60), (short) (0x05F0), (short) (0x04F9), (short) (0x07F3), (short) (0x06FA), (short) (0x01F6), (short) (0x00FF), (short) (0x03F5), (short) (0x02FC), (short) (0x0DFC), (short) (0x0CF5), (short) (0x0FFF), (short) (0x0EF6), (short) (0x09FA), (short) (0x08F3), (short) (0x0BF9), (short) (0x0AF0), (short) (0x0650), (short) (0x0759), (short) (0x0453), (short) (0x055A), (short) (0x0256), (short) (0x035F), (short) (0x0055), (short) (0x015C), (short) (0x0E5C), (short) (0x0F55), (short) (0x0C5F), (short) (0x0D56), (short) (0x0A5A), (short) (
			0x0B53), (short) (0x0859), (short) (0x0950), (short) (0x07C0), (short) (0x06C9), (short) (0x05C3), (short) (0x04CA), (short) (0x03C6), (short) (0x02CF), (short) (0x01C5), (short) (0x00CC), (short) (0x0FCC), (short) (0x0EC5), (short) (0x0DCF), (short) (0x0CC6), (short) (0x0BCA), (short) (0x0AC3), (short) (0x09C9), (short) (0x08C0), (short) (0x08C0), (short) (0x09C9), (short) (0x0AC3), (short) (0x0BCA), (short) (0x0CC6), (short) (0x0DCF), (short) (0x0EC5), (short) (0x0FCC), (short) (0x00CC), (short) (0x01C5), (short) (0x02CF), (short) (0x03C6), (short) (0x04CA), (short) (0x05C3), (short) (0x06C9), (short) (0x07C0), (short) (0x0950), (short) (0x0859), (short) (0x0B53), (short) (0x0A5A), (short) (0x0D56), (short) (0x0C5F), (short) (0x0F55), (short) (0x0E5C), (short) (0x015C), (short) (0x0055), (short) (0x035F), (short) (0x0256), (short) (0x055A), (short) (0x0453), (short) (0x0759), (short) (0x0650), (short) (0x0AF0), (short) (0x0BF9), (short) (0x08F3), (short) (0x09FA), (short) (0x0EF6), (short) (0x0FFF), (short) (0x0CF5), (short) (0x0DFC), (short) (0x02FC), (short) (0x03F5), (short) (0x00FF), (short) (0x01F6), (short) (0x06FA), (short) (0x07F3), (short) (0x04F9), (short) (0x05F0), (short) (0x0B60), (short) (0x0A69), (short) (0x0963), (short) (0x086A), (short) (0x0F66), (short) (0x0E6F), (short) (0x0D65), (short) (0x0C6C), (short) (0x036C), (short) (0x0265), (short) (0x016F), (short) (0x0066), (short) (0x076A), (short) (0x0663), (short) (0x0569), (short) (0x0460), (short) (0x0CA0), (short) (0x0DA9), (short) (0x0EA3), (short) (0x0FAA), (short) (0x08A6), (short) (0x09AF), (short) (0x0AA5), (short) (0x0BAC), (short) (0x04AC), (short) (0x05A5), (short) (0x06AF), (short) (0x07A6), (short) (0x00AA), (short) (0x01A3), (short) (0x02A9), (short) (0x03A0), (short) (0x0D30), (short) (0x0C39), (short) (0x0F33), (short) (0x0E3A), (short) (0x0936), (short) (0x083F), (short) (0x0B35), (short) (0x0A3C), (short) (0x053C), (short) (0x0435), (short) (0x073F), (short) (0x0636), (short) (0x013A), (short) (0x0033), (short
			) (0x0339), (short) (0x0230), (short) (0x0E90), (short) (0x0F99), (short) (0x0C93), (short) (0x0D9A), (short) (0x0A96), (short) (0x0B9F), (short) (0x0895), (short) (0x099C), (short) (0x069C), (short) (0x0795), (short) (0x049F), (short) (0x0596), (short) (0x029A), (short) (0x0393), (short) (0x0099), (short) (0x0190), (short) (0x0F00), (short) (0x0E09), (short) (0x0D03), (short) (0x0C0A), (short) (0x0B06), (short) (0x0A0F), (short) (0x0905), (short) (0x080C), (short) (0x070C), (short) (0x0605), (short) (0x050F), (short) (0x0406), (short) (0x030A), (short) (0x0203), (short) (0x0109), (short) (0x0000)};
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'triangleTable'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly sbyte[][] triangleTable = new sbyte[][]{null, new sbyte[]{0, 8, 3}, new sbyte[]{0, 1, 9}, new sbyte[]{1, 8, 3, 9, 8, 1}, new sbyte[]{1, 2, 10}, new sbyte[]{0, 8, 3, 1, 2, 10}, new sbyte[]{9, 2, 10, 0, 2, 9}, new sbyte[]{2, 8, 3, 2, 10, 8, 10, 9, 8}, new sbyte[]{3, 11, 2}, new sbyte[]{0, 11, 2, 8, 11, 0}, new sbyte[]{1, 9, 0, 2, 3, 11}, new sbyte[]{1, 11, 2, 1, 9, 11, 9, 8, 11}, new sbyte[]{3, 10, 1, 11, 10, 3}, new sbyte[]{0, 10, 1, 0, 8, 10, 8, 11, 10}, new sbyte[]{3, 9, 0, 3, 11, 9, 11, 10, 9}, new sbyte[]{9, 8, 10, 10, 8, 11}, new sbyte[]{4, 7, 8}, new sbyte[]{4, 3, 0, 7, 3, 4}, new sbyte[]{0, 1, 9, 8, 4, 7}, new sbyte[]{4, 1, 9, 4, 7, 1, 7, 3, 1}, new sbyte[]{1, 2, 10, 8, 4, 7}, new sbyte[]{3, 4, 7, 3, 0, 4, 1, 2, 10}, new sbyte[]{9, 2, 10, 9, 0, 2, 8, 4, 7}, new sbyte[]{2, 10, 9, 2, 9, 7, 2, 7, 3, 7, 9, 4}, new sbyte[]{8, 4, 7, 3, 11, 2}, new sbyte[]{11, 4, 7, 11, 2, 4, 2, 0, 4}, new sbyte[]{9, 0, 1, 8, 4, 7, 2, 3, 11}, new sbyte[]{4, 7, 11, 9, 4, 11, 9, 11, 2, 9, 2, 1}, new sbyte[]{3, 10, 1, 3, 11, 10, 7, 8, 4}, new sbyte[]{1, 11, 10, 1, 4, 11, 1, 0, 4, 7, 11, 4}, new sbyte[]{4, 7, 8, 9, 0, 11, 9, 11, 10, 11, 0, 3}, new sbyte[]{4, 7, 11, 4, 11, 9, 9, 11, 10}, new sbyte[]{9, 5, 4}, new sbyte[]{9, 5, 4, 0, 8, 3}, new sbyte[]{0, 5, 4, 1, 5, 0}, new sbyte[]{8, 5, 4, 8, 3, 5, 3, 1, 5}, new sbyte[]{1, 2, 10, 9, 5, 4}, new sbyte[]{3, 0, 8, 1, 2, 10, 4, 9, 5}, new sbyte[]{5, 2, 10, 5, 4, 2, 4, 0, 2}, new sbyte[]{2, 10, 5, 3, 2, 5, 3, 5, 4, 3, 4, 8}, new sbyte[]{9, 5, 4, 2, 3, 11}, new sbyte[]{0, 11, 2, 0, 8, 11, 4, 9, 5}, new sbyte[]{0, 5, 4, 0, 1, 5, 2, 3, 11}, new sbyte[]{2, 1, 5, 2, 5, 8, 2, 8, 11, 4, 8, 5}, new sbyte[]{10, 3, 11, 10, 1, 3, 9, 5, 4}, new sbyte[]{4, 9, 5, 0, 8, 1, 8, 10, 1, 8, 11, 10}, new sbyte[]{5, 4, 0, 5, 0, 11, 5, 11, 10, 11, 0, 3}, new sbyte[]{5, 4, 8, 5, 8, 10, 10, 8, 11}, new sbyte[]{9, 7, 8, 5, 7, 9}, new sbyte[]{9, 3, 0, 9, 5, 3, 5, 7, 3}, new sbyte[]{0, 7, 8, 0, 1, 7, 1, 5, 7}, new sbyte[]{1, 5, 3, 3, 5, 7}, new sbyte[]{9, 7, 8, 9, 5, 7, 10, 1, 2}, 
			new sbyte[]{10, 1, 2, 9, 5, 0, 5, 3, 0, 5, 7, 3}, new sbyte[]{8, 0, 2, 8, 2, 5, 8, 5, 7, 10, 5, 2}, new sbyte[]{2, 10, 5, 2, 5, 3, 3, 5, 7}, new sbyte[]{7, 9, 5, 7, 8, 9, 3, 11, 2}, new sbyte[]{9, 5, 7, 9, 7, 2, 9, 2, 0, 2, 7, 11}, new sbyte[]{2, 3, 11, 0, 1, 8, 1, 7, 8, 1, 5, 7}, new sbyte[]{11, 2, 1, 11, 1, 7, 7, 1, 5}, new sbyte[]{9, 5, 8, 8, 5, 7, 10, 1, 3, 10, 3, 11}, new sbyte[]{5, 7, 0, 5, 0, 9, 7, 11, 0, 1, 0, 10, 11, 10, 0}, new sbyte[]{11, 10, 0, 11, 0, 3, 10, 5, 0, 8, 0, 7, 5, 7, 0}, new sbyte[]{11, 10, 5, 7, 11, 5}, new sbyte[]{10, 6, 5}, new sbyte[]{0, 8, 3, 5, 10, 6}, new sbyte[]{9, 0, 1, 5, 10, 6}, new sbyte[]{1, 8, 3, 1, 9, 8, 5, 10, 6}, new sbyte[]{1, 6, 5, 2, 6, 1}, new sbyte[]{1, 6, 5, 1, 2, 6, 3, 0, 8}, new sbyte[]{9, 6, 5, 9, 0, 6, 0, 2, 6}, new sbyte[]{5, 9, 8, 5, 8, 2, 5, 2, 6, 3, 2, 8}, new sbyte[]{2, 3, 11, 10, 6, 5}, new sbyte[]{11, 0, 8, 11, 2, 0, 10, 6, 5}, new sbyte[]{0, 1, 9, 2, 3, 11, 5, 10, 6}, new sbyte[]{5, 10, 6, 1, 9, 2, 9, 11, 2, 9, 8, 11}, new sbyte[]{6, 3, 11, 6, 5, 3, 5, 1, 3}, new sbyte[]{0, 8, 11, 0, 11, 5, 0, 5, 1, 5, 11, 6}, new sbyte[]{3, 11, 6, 0, 3, 6, 0, 6, 5, 0, 5, 9}, new sbyte[]{6, 5, 9, 6, 9, 11, 11, 9, 8}, new sbyte[]{5, 10, 6, 4, 7, 8}, new sbyte[]{4, 3, 0, 4, 7, 3, 6, 5, 10}, new sbyte[]{1, 9, 0, 5, 10, 6, 8, 4, 7}, new sbyte[]{10, 6, 5, 1, 9, 7, 1, 7, 3, 7, 9, 4}, new sbyte[]{6, 1, 2, 6, 5, 1, 4, 7, 8}, new sbyte[]{1, 2, 5, 5, 2, 6, 3, 0, 4, 3, 4, 7}, new sbyte[]{8, 4, 7, 9, 0, 5, 0, 6, 5, 0, 2, 6}, new sbyte[]{7, 3, 9, 7, 9, 4, 3, 2, 9, 5, 9, 6, 2, 6, 9}, new sbyte[]{3, 11, 2, 7, 8, 4, 10, 6, 5}, new sbyte[]{5, 10, 6, 4, 7, 2, 4, 2, 0, 2, 7, 11}, new sbyte[]{0, 1, 9, 4, 7, 8, 2, 3, 11, 5, 10, 6}, new sbyte[]{9, 2, 1, 9, 11, 2, 9, 4, 11, 7, 11, 4, 5, 10, 6}, new sbyte[]{8, 4, 7, 3, 11, 5, 3, 5, 1, 5, 11, 6}, new sbyte[]{5, 1, 11, 5, 11, 6, 1, 0, 11, 7, 11, 4, 0, 4, 11}, new sbyte[]{0, 5, 9, 0, 6, 5, 0, 3, 6, 11, 6, 3, 8, 4, 7}, new sbyte[]{6, 5, 9, 6, 9, 11, 4, 7, 9, 7, 11, 9}, new sbyte[]{10, 4, 9, 6, 4, 10}, new sbyte[]{4, 10, 6, 4, 9, 10
			, 0, 8, 3}, new sbyte[]{10, 0, 1, 10, 6, 0, 6, 4, 0}, new sbyte[]{8, 3, 1, 8, 1, 6, 8, 6, 4, 6, 1, 10}, new sbyte[]{1, 4, 9, 1, 2, 4, 2, 6, 4}, new sbyte[]{3, 0, 8, 1, 2, 9, 2, 4, 9, 2, 6, 4}, new sbyte[]{0, 2, 4, 4, 2, 6}, new sbyte[]{8, 3, 2, 8, 2, 4, 4, 2, 6}, new sbyte[]{10, 4, 9, 10, 6, 4, 11, 2, 3}, new sbyte[]{0, 8, 2, 2, 8, 11, 4, 9, 10, 4, 10, 6}, new sbyte[]{3, 11, 2, 0, 1, 6, 0, 6, 4, 6, 1, 10}, new sbyte[]{6, 4, 1, 6, 1, 10, 4, 8, 1, 2, 1, 11, 8, 11, 1}, new sbyte[]{9, 6, 4, 9, 3, 6, 9, 1, 3, 11, 6, 3}, new sbyte[]{8, 11, 1, 8, 1, 0, 11, 6, 1, 9, 1, 4, 6, 4, 1}, new sbyte[]{3, 11, 6, 3, 6, 0, 0, 6, 4}, new sbyte[]{6, 4, 8, 11, 6, 8}, new sbyte[]{7, 10, 6, 7, 8, 10, 8, 9, 10}, new sbyte[]{0, 7, 3, 0, 10, 7, 0, 9, 10, 6, 7, 10}, new sbyte[]{10, 6, 7, 1, 10, 7, 1, 7, 8, 1, 8, 0}, new sbyte[]{10, 6, 7, 10, 7, 1, 1, 7, 3}, new sbyte[]{1, 2, 6, 1, 6, 8, 1, 8, 9, 8, 6, 7}, new sbyte[]{2, 6, 9, 2, 9, 1, 6, 7, 9, 0, 9, 3, 7, 3, 9}, new sbyte[]{7, 8, 0, 7, 0, 6, 6, 0, 2}, new sbyte[]{7, 3, 2, 6, 7, 2}, new sbyte[]{2, 3, 11, 10, 6, 8, 10, 8, 9, 8, 6, 7}, new sbyte[]{2, 0, 7, 2, 7, 11, 0, 9, 7, 6, 7, 10, 9, 10, 7}, new sbyte[]{1, 8, 0, 1, 7, 8, 1, 10, 7, 6, 7, 10, 2, 3, 11}, new sbyte[]{11, 2, 1, 11, 1, 7, 10, 6, 1, 6, 7, 1}, new sbyte[]{8, 9, 6, 8, 6, 7, 9, 1, 6, 11, 6, 3, 1, 3, 6}, new sbyte[]{0, 9, 1, 11, 6, 7}, new sbyte[]{7, 8, 0, 7, 0, 6, 3, 11, 0, 11, 6, 0}, new sbyte[]{7, 11, 6}, new sbyte[]{7, 6, 11}, new sbyte[]{3, 0, 8, 11, 7, 6}, new sbyte[]{0, 1, 9, 11, 7, 6}, new sbyte[]{8, 1, 9, 8, 3, 1, 11, 7, 6}, new sbyte[]{10, 1, 2, 6, 11, 7}, new sbyte[]{1, 2, 10, 3, 0, 8, 6, 11, 7}, new sbyte[]{2, 9, 0, 2, 10, 9, 6, 11, 7}, new sbyte[]{6, 11, 7, 2, 10, 3, 10, 8, 3, 10, 9, 8}, new sbyte[]{7, 2, 3, 6, 2, 7}, new sbyte[]{7, 0, 8, 7, 6, 0, 6, 2, 0}, new sbyte[]{2, 7, 6, 2, 3, 7, 0, 1, 9}, new sbyte[]{1, 6, 2, 1, 8, 6, 1, 9, 8, 8, 7, 6}, new sbyte[]{10, 7, 6, 10, 1, 7, 1, 3, 7}, new sbyte[]{10, 7, 6, 1, 7, 10, 1, 8, 7, 1, 0, 8}, new sbyte[]{0, 3, 7, 0, 7, 10, 0, 10, 9, 6, 10, 7}, new sbyte[]{7, 6
			, 10, 7, 10, 8, 8, 10, 9}, new sbyte[]{6, 8, 4, 11, 8, 6}, new sbyte[]{3, 6, 11, 3, 0, 6, 0, 4, 6}, new sbyte[]{8, 6, 11, 8, 4, 6, 9, 0, 1}, new sbyte[]{9, 4, 6, 9, 6, 3, 9, 3, 1, 11, 3, 6}, new sbyte[]{6, 8, 4, 6, 11, 8, 2, 10, 1}, new sbyte[]{1, 2, 10, 3, 0, 11, 0, 6, 11, 0, 4, 6}, new sbyte[]{4, 11, 8, 4, 6, 11, 0, 2, 9, 2, 10, 9}, new sbyte[]{10, 9, 3, 10, 3, 2, 9, 4, 3, 11, 3, 6, 4, 6, 3}, new sbyte[]{8, 2, 3, 8, 4, 2, 4, 6, 2}, new sbyte[]{0, 4, 2, 4, 6, 2}, new sbyte[]{1, 9, 0, 2, 3, 4, 2, 4, 6, 4, 3, 8}, new sbyte[]{1, 9, 4, 1, 4, 2, 2, 4, 6}, new sbyte[]{8, 1, 3, 8, 6, 1, 8, 4, 6, 6, 10, 1}, new sbyte[]{10, 1, 0, 10, 0, 6, 6, 0, 4}, new sbyte[]{4, 6, 3, 4, 3, 8, 6, 10, 3, 0, 3, 9, 10, 9, 3}, new sbyte[]{10, 9, 4, 6, 10, 4}, new sbyte[]{4, 9, 5, 7, 6, 11}, new sbyte[]{0, 8, 3, 4, 9, 5, 11, 7, 6}, new sbyte[]{5, 0, 1, 5, 4, 0, 7, 6, 11}, new sbyte[]{11, 7, 6, 8, 3, 4, 3, 5, 4, 3, 1, 5}, new sbyte[]{9, 5, 4, 10, 1, 2, 7, 6, 11}, new sbyte[]{6, 11, 7, 1, 2, 10, 0, 8, 3, 4, 9, 5}, new sbyte[]{7, 6, 11, 5, 4, 10, 4, 2, 10, 4, 0, 2}, new sbyte[]{3, 4, 8, 3, 5, 4, 3, 2, 5, 10, 5, 2, 11, 7, 6}, new sbyte[]{7, 2, 3, 7, 6, 2, 5, 4, 9}, new sbyte[]{9, 5, 4, 0, 8, 6, 0, 6, 2, 6, 8, 7}, new sbyte[]{3, 6, 2, 3, 7, 6, 1, 5, 0, 5, 4, 0}, new sbyte[]{6, 2, 8, 6, 8, 7, 2, 1, 8, 4, 8, 5, 1, 5, 8}, new sbyte[]{9, 5, 4, 10, 1, 6, 1, 7, 6, 1, 3, 7}, new sbyte[]{1, 6, 10, 1, 7, 6, 1, 0, 7, 8, 7, 0, 9, 5, 4}, new sbyte[]{4, 0, 10, 4, 10, 5, 0, 3, 10, 6, 10, 7, 3, 7, 10}, new sbyte[]{7, 6, 10, 7, 10, 8, 5, 4, 10, 4, 8, 10}, new sbyte[]{6, 9, 5, 6, 11, 9, 11, 8, 9}, new sbyte[]{3, 6, 11, 0, 6, 3, 0, 5, 6, 0, 9, 5}, new sbyte[]{0, 11, 8, 0, 5, 11, 0, 1, 5, 5, 6, 11}, new sbyte[]{6, 11, 3, 6, 3, 5, 5, 3, 1}, new sbyte[]{1, 2, 10, 9, 5, 11, 9, 11, 8, 11, 5, 6}, new sbyte[]{0, 11, 3, 0, 6, 11, 0, 9, 6, 5, 6, 9, 1, 2, 10}, new sbyte[]{11, 8, 5, 11, 5, 6, 8, 0, 5, 10, 5, 2, 0, 2, 5}, new sbyte[]{6, 11, 3, 6, 3, 5, 2, 10, 3, 10, 5, 3}, new sbyte[]{5, 8, 9, 5, 2, 8, 5, 6, 2, 3, 8, 2}, new sbyte[]{9, 5, 6, 9, 6, 0, 0, 6, 2
			}, new sbyte[]{1, 5, 8, 1, 8, 0, 5, 6, 8, 3, 8, 2, 6, 2, 8}, new sbyte[]{1, 5, 6, 2, 1, 6}, new sbyte[]{1, 3, 6, 1, 6, 10, 3, 8, 6, 5, 6, 9, 8, 9, 6}, new sbyte[]{10, 1, 0, 10, 0, 6, 9, 5, 0, 5, 6, 0}, new sbyte[]{0, 3, 8, 5, 6, 10}, new sbyte[]{10, 5, 6}, new sbyte[]{11, 5, 10, 7, 5, 11}, new sbyte[]{11, 5, 10, 11, 7, 5, 8, 3, 0}, new sbyte[]{5, 11, 7, 5, 10, 11, 1, 9, 0}, new sbyte[]{10, 7, 5, 10, 11, 7, 9, 8, 1, 8, 3, 1}, new sbyte[]{11, 1, 2, 11, 7, 1, 7, 5, 1}, new sbyte[]{0, 8, 3, 1, 2, 7, 1, 7, 5, 7, 2, 11}, new sbyte[]{9, 7, 5, 9, 2, 7, 9, 0, 2, 2, 11, 7}, new sbyte[]{7, 5, 2, 7, 2, 11, 5, 9, 2, 3, 2, 8, 9, 8, 2}, new sbyte[]{2, 5, 10, 2, 3, 5, 3, 7, 5}, new sbyte[]{8, 2, 0, 8, 5, 2, 8, 7, 5, 10, 2, 5}, new sbyte[]{9, 0, 1, 5, 10, 3, 5, 3, 7, 3, 10, 2}, new sbyte[]{9, 8, 2, 9, 2, 1, 8, 7, 2, 10, 2, 5, 7, 5, 2}, new sbyte[]{1, 3, 5, 3, 7, 5}, new sbyte[]{0, 8, 7, 0, 7, 1, 1, 7, 5}, new sbyte[]{9, 0, 3, 9, 3, 5, 5, 3, 7}, new sbyte[]{9, 8, 7, 5, 9, 7}, new sbyte[]{5, 8, 4, 5, 10, 8, 10, 11, 8}, new sbyte[]{5, 0, 4, 5, 11, 0, 5, 10, 11, 11, 3, 0}, new sbyte[]{0, 1, 9, 8, 4, 10, 8, 10, 11, 10, 4, 5}, new sbyte[]{10, 11, 4, 10, 4, 5, 11, 3, 4, 9, 4, 1, 3, 1, 4}, new sbyte[]{2, 5, 1, 2, 8, 5, 2, 11, 8, 4, 5, 8}, new sbyte[]{0, 4, 11, 0, 11, 3, 4, 5, 11, 2, 11, 1, 5, 1, 11}, new sbyte[]{0, 2, 5, 0, 5, 9, 2, 11, 5, 4, 5, 8, 11, 8, 5}, new sbyte[]{9, 4, 5, 2, 11, 3}, new sbyte[]{2, 5, 10, 3, 5, 2, 3, 4, 5, 3, 8, 4}, new sbyte[]{5, 10, 2, 5, 2, 4, 4, 2, 0}, new sbyte[]{3, 10, 2, 3, 5, 10, 3, 8, 5, 4, 5, 8, 0, 1, 9}, new sbyte[]{5, 10, 2, 5, 2, 4, 1, 9, 2, 9, 4, 2}, new sbyte[]{8, 4, 5, 8, 5, 3, 3, 5, 1}, new sbyte[]{0, 4, 5, 1, 0, 5}, new sbyte[]{8, 4, 5, 8, 5, 3, 9, 0, 5, 0, 3, 5}, new sbyte[]{9, 4, 5}, new sbyte[]{4, 11, 7, 4, 9, 11, 9, 10, 11}, new sbyte[]{0, 8, 3, 4, 9, 7, 9, 11, 7, 9, 10, 11}, new sbyte[]{1, 10, 11, 1, 11, 4, 1, 4, 0, 7, 4, 11}, new sbyte[]{3, 1, 4, 3, 4, 8, 1, 10, 4, 7, 4, 11, 10, 11, 4}, new sbyte[]{4, 11, 7, 9, 11, 4, 9, 2, 11, 9, 1, 2}, new sbyte[]{9, 7, 4, 9, 11, 7, 9, 1
			, 11, 2, 11, 1, 0, 8, 3}, new sbyte[]{11, 7, 4, 11, 4, 2, 2, 4, 0}, new sbyte[]{11, 7, 4, 11, 4, 2, 8, 3, 4, 3, 2, 4}, new sbyte[]{2, 9, 10, 2, 7, 9, 2, 3, 7, 7, 4, 9}, new sbyte[]{9, 10, 7, 9, 7, 4, 10, 2, 7, 8, 7, 0, 2, 0, 7}, new sbyte[]{3, 7, 10, 3, 10, 2, 7, 4, 10, 1, 10, 0, 4, 0, 10}, new sbyte[]{1, 10, 2, 8, 7, 4}, new sbyte[]{4, 9, 1, 4, 1, 7, 7, 1, 3}, new sbyte[]{4, 9, 1, 4, 1, 7, 0, 8, 1, 8, 7, 1}, new sbyte[]{4, 0, 3, 7, 4, 3}, new sbyte[]{4, 8, 7}, new sbyte[]{9, 10, 8, 10, 11, 8}, new sbyte[]{3, 0, 9, 3, 9, 11, 11, 9, 10}, new sbyte[]{0, 1, 10, 0, 10, 8, 8, 10, 11}, new sbyte[]{3, 1, 10, 11, 3, 10}, new sbyte[]{1, 2, 11, 1, 11, 9, 9, 11, 8}, new sbyte[]{3, 0, 9, 3, 9, 11, 1, 2, 9, 2, 11, 9}, new sbyte[]{0, 2, 11, 8, 0, 11}, new sbyte[]{3, 2, 11}, new sbyte[]{2, 3, 8, 2, 8, 10, 10, 8, 9}, new sbyte[]{9, 10, 2, 0, 9, 2}, new sbyte[]{2, 3, 8, 2, 8, 10, 0, 1, 8, 1, 10, 8}, new sbyte[]{1, 10, 2}, new sbyte[]{1, 3, 8, 9, 1, 8}, new sbyte[]{0, 9, 1}, new sbyte[]{0, 3, 8}, null};
		
		////////////////////////////////////////////////////////////////
		// color scale stuff
		////////////////////////////////////////////////////////////////
		
		internal virtual void  applyColorScale(float min, float max, System.String scaleName)
		{
			if (currentMesh != null)
				applyColorScale(currentMesh, min, max, scaleName);
			else
			{
				for (int i = meshCount; --i >= 0; )
					applyColorScale(meshes[i], min, max, scaleName);
			}
		}
		
		internal virtual float getMinMappedValue()
		{
			if (currentMesh != null)
				return getMinMappedValue(currentMesh);
			float min = System.Single.MaxValue;
			for (int i = meshCount; --i >= 0; )
			{
				float challenger = getMinMappedValue(meshes[i]);
				if (challenger < min)
					min = challenger;
			}
			return min;
		}
		
		internal virtual float getMaxMappedValue()
		{
			if (currentMesh != null)
				return getMaxMappedValue(currentMesh);
			//UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Float.MIN_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			float max = System.Single.Epsilon;
			for (int i = meshCount; --i >= 0; )
			{
				float challenger = getMaxMappedValue(meshes[i]);
				if (challenger > max)
					max = challenger;
			}
			return max;
		}
		
		internal virtual void  applyColorScale(Mesh mesh, float min, float max, System.String scaleName)
		{
			int vertexCount = mesh.vertexCount;
			Point3f[] vertexes = mesh.vertices;
			short[] colixes = mesh.vertexColixes;
			if (colixes == null)
				mesh.vertexColixes = colixes = new short[vertexCount];
			for (int i = vertexCount; --i >= 0; )
			{
				float value_Renamed = lookupInterpolatedVoxelValue(vertexes[i]);
				colixes[i] = viewer.getColixFromPalette(value_Renamed, min, max, scaleName);
			}
		}
		
		internal virtual float getMinMappedValue(Mesh mesh)
		{
			int vertexCount = mesh.vertexCount;
			Point3f[] vertexes = mesh.vertices;
			float min = System.Single.MaxValue;
			for (int i = vertexCount; --i >= 0; )
			{
				float challenger = lookupInterpolatedVoxelValue(vertexes[i]);
				if (challenger < min)
					min = challenger;
			}
			return min;
		}
		
		internal virtual float getMaxMappedValue(Mesh mesh)
		{
			int vertexCount = mesh.vertexCount;
			Point3f[] vertexes = mesh.vertices;
			//UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Float.MIN_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			float max = System.Single.Epsilon;
			for (int i = vertexCount; --i >= 0; )
			{
				float challenger = lookupInterpolatedVoxelValue(vertexes[i]);
				if (challenger > max)
					max = challenger;
			}
			return max;
		}
		
		internal int i;
		//UPGRADE_NOTE: Final was removed from the declaration of 'pointVector '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f pointVector = new Vector3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'cubeLocation '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Point3f cubeLocation = new Point3f();
		internal virtual float lookupInterpolatedVoxelValue(Point3f point)
		{
			pointVector.sub(point, volumetricOrigin);
			float x = scaleByVoxelVector(pointVector, 0);
			float y = scaleByVoxelVector(pointVector, 1);
			float z = scaleByVoxelVector(pointVector, 2);
			return getInterpolatedValue(x, y, z);
		}
		
		internal virtual float scaleByVoxelVector(Vector3f vector, int voxelVectorIndex)
		{
			return (vector.dot(unitVolumetricVectors[voxelVectorIndex]) / volumetricVectorLengths[voxelVectorIndex]);
		}
		
		internal virtual int indexDown(float value_Renamed, int voxelVectorIndex)
		{
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			int floor = (int) value_Renamed;
			float delta = value_Renamed - floor;
			if (delta > 0.9f)
				++floor;
			int lastValue = voxelCounts[voxelVectorIndex] - 1;
			if (floor > lastValue)
				floor = lastValue;
			return floor;
		}
		
		internal virtual int indexUp(float value_Renamed, int voxelVectorIndex)
		{
			if (value_Renamed < 0)
				return 0;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			int ceil = ((int) value_Renamed) + 1;
			float delta = ceil - value_Renamed;
			if (delta > 0.9f)
				--ceil;
			int lastValue = voxelCounts[voxelVectorIndex] - 1;
			if (ceil > lastValue)
				ceil = lastValue;
			return ceil;
		}
		
		internal virtual float getInterpolatedValue(float x, float y, float z)
		{
			int xDown = indexDown(x, 0);
			int xUp = indexUp(x, 0);
			int yDown = indexDown(y, 1);
			int yUp = indexUp(y, 1);
			int zDown = indexDown(z, 2);
			int zUp = indexUp(z, 2);
			
			float valueDown = voxelData[xDown][yDown][zDown];
			float valueUp = voxelData[xUp][yUp][zUp];
			float valueDelta = valueUp - valueDown;
			float delta;
			int differentMask;
			differentMask = ((((xUp == xDown)?0:1) << 0) | (((yUp == yDown)?0:1) << 1) | (((zUp == zDown)?0:1) << 2));
			switch (differentMask)
			{
				
				case 0: 
					return valueDown;
				
				case 1: 
					delta = x - xDown;
					break;
				
				case 2: 
					delta = y - yDown;
					break;
				
				case 4: 
					delta = z - zDown;
					break;
				
				default: 
					// I don't feel like dealing with all the cases
					// just stick it in the middle
					delta = 0.5f;
					break;
				
			}
			return valueDown + delta * valueDelta;
		}
	}
}