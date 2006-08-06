/* $RCSfile$
* $Author: egonw $
* $Date: 2005-11-10 10:52:44 -0500 (Thu, 10 Nov 2005) $
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
//UPGRADE_TODO: The type 'javax.vecmath.Vector3f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Vector3f = javax.vecmath.Vector3f;
namespace org.jmol.g3d
{
	
	/// <summary> Constructs a canonical geodesic sphere of unit radius.
	/// <p>
	/// The Normix3D code quantizes arbitrary vectors to the vectors
	/// of this unit sphere. normix values are then used for
	/// high performance surface normal lighting
	/// <p>
	/// The vertices of the geodesic sphere can be used for constructing
	/// vanderWaals and Connolly dot surfaces.
	/// <p>
	/// One geodesic sphere is constructed. It is a unit sphere
	/// with radius of 1.0
	/// <p>
	/// Many times a sphere is constructed with lines of latitude and
	/// longitude. With this type of rendering, the atom has north and
	/// south poles. And the faces are not regularly shaped ... at the
	/// poles they are triangles but elsewhere they are quadrilaterals.
	/// <p>
	/// A geodesic sphere is more appropriate for this type
	/// of application. The geodesic sphere does not have poles and 
	/// looks the same in all orientations ... as a sphere should. All
	/// faces are equilateral triangles.
	/// <p>
	/// The geodesic sphere is constructed by starting with an icosohedron, 
	/// a platonic solid with 12 vertices and 20 equilateral triangles
	/// for faces. The internal call to the private
	/// method <code>quadruple</code> will
	/// split each triangular face into 4 faces by creating a new vertex
	/// at the midpoint of each edge. These midpoints are still in the
	/// plane, so they are then 'pushed out' to the surface of the
	/// enclosing sphere by normalizing their length back to 1.0
	/// <p>
	/// The sequence of vertex counts is 12, 42, 162, 642, 2562.
	/// These are identified by 'levels', that run from 0 through 4;
	/// The vertices
	/// are stored so that when spheres are small they can choose to display
	/// only the first n bits where n is one of the above vertex counts.
	/// <code>
	/// Faces + Vertices = Edges + 2
	/// Faces: 20, 80, 320, 1280, 5120, 20480
	/// start with 20 faces ... at each level multiply by 4
	/// Edges: 30, 120, 480, 1920, 7680, 30720
	/// start with 30 edges ... also multipy by 4 ... strange, but true
	/// Vertices: 12, 42, 162, 642, 2562, 10242
	/// start with 12 vertices and 30 edges.
	/// when you subdivide, each edge contributes one vertex
	/// 12 + 30 = 42 vertices at the next level
	/// 80 faces + 42 vertices - 2 = 120 edges at the next level
	/// </code>
	/// <p>
	/// The vertices of the 'one true canonical sphere' are rotated to the
	/// current molecular rotation at the beginning of the repaint cycle.
	/// That way,
	/// individual atoms only need to scale the unit vector to the vdw
	/// radius for that atom.
	/// <p>
	/// 
	/// Notes 27 Sep 2005 </br>
	/// If I were to switch the representation to staring with
	/// a tetrahedron instead of an icosohedron we would get:
	/// <code>
	/// Faces: 4, 16, 64, 256, 1024
	/// Edges: 6, 24, 96, 384, 1536
	/// Vertices: 4, 10, 34, 130, 514
	/// </code>
	/// If I switched to face-centered normixes then I could efficiently
	/// Regardless, I think that face-centered normixes would also reduce
	/// ambiguity and would speed up the normal to normix process.
	/// 
	/// I could also start with an octahedron that placed one triangle
	/// in each 3D cartesian octant. That would push to 512 faces instead
	/// of 256 faces, leaving me with shorts. But, it would be easier to quantize
	/// at the first level because it would be based upon sign. And perhaps
	/// it would be easier to take advantage of symmetry in the process of
	/// converting from normal to normix.
	/// </summary>
	
	class Geodesic3D
	{
		internal static Vector3f[] VertexVectors
		{
			get
			{
				return vertexVectors;
			}
			
		}
		
		internal Graphics3D g3d;
		
		private const bool DUMP = false;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'halfRoot5 '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
		internal static readonly float halfRoot5 = (float) (0.5 * System.Math.Sqrt(5));
		//UPGRADE_NOTE: Final was removed from the declaration of 'oneFifth '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
		internal static readonly float oneFifth = 2 * (float) System.Math.PI / 5;
		//UPGRADE_NOTE: Final was removed from the declaration of 'oneTenth '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly float oneTenth = oneFifth / 2;
		
		// miguel 2005 01 11
		// within the context of this code, the term 'vertex' is used
		// to refer to a short which is an index into the tables
		// of vertex information.
		//UPGRADE_NOTE: Final was removed from the declaration of 'faceVertexesIcosahedron'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly short[] faceVertexesIcosahedron = new short[]{0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 5, 0, 5, 1, 1, 6, 2, 2, 7, 3, 3, 8, 4, 4, 9, 5, 5, 10, 1, 6, 1, 10, 7, 2, 6, 8, 3, 7, 9, 4, 8, 10, 5, 9, 11, 6, 10, 11, 7, 6, 11, 8, 7, 11, 9, 8, 11, 10, 9};
		
		// every vertex has 6 neighbors ... except at the beginning of the world
		//UPGRADE_NOTE: Final was removed from the declaration of 'neighborVertexesIcosahedron'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly short[] neighborVertexesIcosahedron = new short[]{1, 2, 3, 4, 5, - 1, 0, 5, 10, 6, 2, - 1, 0, 1, 6, 7, 3, - 1, 0, 2, 7, 8, 4, - 1, 0, 3, 8, 9, 5, - 1, 0, 4, 9, 10, 1, - 1, 1, 10, 11, 7, 2, - 1, 2, 6, 11, 8, 3, - 1, 3, 7, 11, 9, 4, - 1, 4, 8, 11, 10, 5, - 1, 5, 9, 11, 6, 1, - 1, 6, 7, 8, 9, 10, - 1};
		
		/// <summary> 5 levels, 0 through 4</summary>
		internal const int maxLevel = 4;
		internal static short[] vertexCounts;
		internal static short[][] neighborVertexesArrays;
		internal static short[][] faceVertexesArrays;
		internal static Vector3f[] vertexVectors;
		
		internal Geodesic3D(Graphics3D g3d)
		{
			this.g3d = g3d;
			if (vertexCounts == null)
				initialize();
		}
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'initialize'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		private void  initialize()
		{
			lock (this)
			{
				if (vertexCounts != null)
					return ;
				vertexCounts = new short[maxLevel];
				neighborVertexesArrays = new short[maxLevel][];
				faceVertexesArrays = new short[maxLevel][];
				
				vertexVectors = new Vector3f[12];
				vertexVectors[0] = new Vector3f(0, 0, halfRoot5);
				for (int i = 0; i < 5; ++i)
				{
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					vertexVectors[i + 1] = new Vector3f((float) System.Math.Cos(i * oneFifth), (float) System.Math.Sin(i * oneFifth), 0.5f);
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					vertexVectors[i + 6] = new Vector3f((float) System.Math.Cos(i * oneFifth + oneTenth), (float) System.Math.Sin(i * oneFifth + oneTenth), - 0.5f);
				}
				vertexVectors[11] = new Vector3f(0, 0, - halfRoot5);
				for (int i = 12; --i >= 0; )
					vertexVectors[i].normalize();
				faceVertexesArrays[0] = faceVertexesIcosahedron;
				neighborVertexesArrays[0] = neighborVertexesIcosahedron;
				vertexCounts[0] = 12;
				
				for (int i = 0; i < maxLevel - 1; ++i)
					quadruple(i);
				
				if (DUMP)
				{
					for (int i = 0; i < maxLevel; ++i)
					{
						System.Console.Out.WriteLine("geodesic level " + i + " vertexCount= " + getVertexCount(i) + " faceCount=" + getFaceCount(i) + " edgeCount=" + getEdgeCount(i));
					}
				}
			}
		}
		
		internal static int getVertexCount(int level)
		{
			return vertexCounts[level];
		}
		
		internal static int getFaceCount(int level)
		{
			return faceVertexesArrays[level].Length / 3;
		}
		
		internal static int getEdgeCount(int level)
		{
			return getVertexCount(level) + getFaceCount(level) - 2;
		}
		
		internal static short[] getNeighborVertexes(int level)
		{
			return neighborVertexesArrays[level];
		}
		
		internal static short[] getFaceVertexes(int level)
		{
			return faceVertexesArrays[level];
		}
		
		private static short vertexNext;
		private static System.Collections.Hashtable htVertex;
		
		private const bool VALIDATE = true;
		
		private static void  quadruple(int level)
		{
			if (DUMP)
				System.Console.Out.WriteLine("quadruple(" + level + ")");
			htVertex = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
			int oldVertexCount = vertexVectors.length;
			short[] oldFaceVertexes = faceVertexesArrays[level];
			int oldFaceVertexesLength = oldFaceVertexes.Length;
			int oldFaceCount = oldFaceVertexesLength / 3;
			int oldEdgesCount = oldVertexCount + oldFaceCount - 2;
			int newVertexCount = oldVertexCount + oldEdgesCount;
			int newFaceCount = 4 * oldFaceCount;
			Vector3f[] newVertexVectors = new Vector3f[newVertexCount];
			Array.Copy(vertexVectors, 0, newVertexVectors, 0, oldVertexCount);
			vertexVectors = newVertexVectors;
			
			short[] newFacesVertexes = new short[3 * newFaceCount];
			faceVertexesArrays[level + 1] = newFacesVertexes;
			short[] neighborVertexes = new short[6 * newVertexCount];
			neighborVertexesArrays[level + 1] = neighborVertexes;
			for (int i = neighborVertexes.Length; --i >= 0; )
				neighborVertexes[i] = - 1;
			
			vertexCounts[level + 1] = (short) newVertexCount;
			
			if (DUMP)
				System.Console.Out.WriteLine("oldVertexCount=" + oldVertexCount + " newVertexCount=" + newVertexCount + " oldFaceCount=" + oldFaceCount + " newFaceCount=" + newFaceCount);
			
			vertexNext = (short) oldVertexCount;
			
			int iFaceNew = 0;
			for (int i = 0; i < oldFaceVertexesLength; )
			{
				short iA = oldFaceVertexes[i++];
				short iB = oldFaceVertexes[i++];
				short iC = oldFaceVertexes[i++];
				short iAB = getVertex(iA, iB);
				short iBC = getVertex(iB, iC);
				short iCA = getVertex(iC, iA);
				
				newFacesVertexes[iFaceNew++] = iA;
				newFacesVertexes[iFaceNew++] = iAB;
				newFacesVertexes[iFaceNew++] = iCA;
				
				newFacesVertexes[iFaceNew++] = iB;
				newFacesVertexes[iFaceNew++] = iBC;
				newFacesVertexes[iFaceNew++] = iAB;
				
				newFacesVertexes[iFaceNew++] = iC;
				newFacesVertexes[iFaceNew++] = iCA;
				newFacesVertexes[iFaceNew++] = iBC;
				
				newFacesVertexes[iFaceNew++] = iCA;
				newFacesVertexes[iFaceNew++] = iAB;
				newFacesVertexes[iFaceNew++] = iBC;
				
				addNeighboringVertexes(neighborVertexes, iAB, iA);
				addNeighboringVertexes(neighborVertexes, iAB, iCA);
				addNeighboringVertexes(neighborVertexes, iAB, iBC);
				addNeighboringVertexes(neighborVertexes, iAB, iB);
				
				addNeighboringVertexes(neighborVertexes, iBC, iB);
				addNeighboringVertexes(neighborVertexes, iBC, iCA);
				addNeighboringVertexes(neighborVertexes, iBC, iC);
				
				addNeighboringVertexes(neighborVertexes, iCA, iC);
				addNeighboringVertexes(neighborVertexes, iCA, iA);
			}
			if (VALIDATE)
			{
				int vertexCount = vertexVectors.length;
				if (iFaceNew != newFacesVertexes.Length)
					throw new System.NullReferenceException();
				if (vertexNext != newVertexCount)
					throw new System.NullReferenceException();
				for (int i = 0; i < 12; ++i)
				{
					for (int j = 0; j < 5; ++j)
					{
						int neighbor = neighborVertexes[i * 6 + j];
						if (neighbor < 0)
							throw new System.NullReferenceException();
						if (neighbor >= vertexCount)
							throw new System.NullReferenceException();
						if (neighborVertexes[i * 6 + 5] != - 1)
							throw new System.NullReferenceException();
					}
				}
				for (int i = 12 * 6; i < neighborVertexes.Length; ++i)
				{
					int neighbor = neighborVertexes[i];
					if (neighbor < 0)
						throw new System.NullReferenceException();
					if (neighbor >= vertexCount)
						throw new System.NullReferenceException();
				}
				for (int i = 0; i < newVertexCount; ++i)
				{
					int neighborCount = 0;
					for (int j = neighborVertexes.Length; --j >= 0; )
						if (neighborVertexes[j] == i)
							++neighborCount;
					if ((i < 12 && neighborCount != 5) || (i >= 12 && neighborCount != 6))
						throw new System.NullReferenceException();
					int faceCount = 0;
					for (int j = newFacesVertexes.Length; --j >= 0; )
						if (newFacesVertexes[j] == i)
							++faceCount;
					if ((i < 12 && faceCount != 5) || (i >= 12 && faceCount != 6))
						throw new System.NullReferenceException();
				}
			}
			htVertex = null;
		}
		
		private static void  addNeighboringVertexes(short[] neighborVertexes, short v1, short v2)
		{
			for (int i = v1 * 6, iMax = i + 6; i < iMax; ++i)
			{
				if (neighborVertexes[i] == v2)
					return ;
				if (neighborVertexes[i] < 0)
				{
					neighborVertexes[i] = v2;
					for (int j = v2 * 6, jMax = j + 6; j < jMax; ++j)
					{
						if (neighborVertexes[j] == v1)
							return ;
						if (neighborVertexes[j] < 0)
						{
							neighborVertexes[j] = v1;
							return ;
						}
					}
				}
			}
			throw new System.NullReferenceException();
		}
		
		/*
		short getNeighborVertex(int level, short vertex, int neighborIndex) {
		short[] neighborVertexes = neighborVertexesArrays[level];
		int offset = vertex * 6 + neighborIndex;
		return neighborVertexes[offset];
		}
		*/
		
		private static short getVertex(short v1, short v2)
		{
			if (v1 > v2)
			{
				short t = v1;
				v1 = v2;
				v2 = t;
			}
			System.Int32 hashKey = (System.Int32) ((v1 << 16) + v2);
			System.Int16 iv = (System.Int16) htVertex[hashKey];
			//UPGRADE_TODO: The 'System.Int16' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
			if (iv != null)
				return (short) iv;
			Vector3f newVertexVector = new Vector3f(vertexVectors[v1]);
			vertexVectors[vertexNext] = newVertexVector;
			newVertexVector.add(vertexVectors[v2]);
			newVertexVector.scale(0.5f);
			newVertexVector.normalize();
			htVertex[hashKey] = (short) vertexNext;
			return vertexNext++;
		}
		
		internal static bool isNeighborVertex(short vertex1, short vertex2, int level)
		{
			short[] neighborVertexes = neighborVertexesArrays[level];
			int offset1 = vertex1 * 6;
			for (int i = offset1 + (vertex1 < 12?5:6); --i >= offset1; )
			{
				if (neighborVertexes[i] == vertex2)
					return true;
			}
			return false;
		}
	}
}