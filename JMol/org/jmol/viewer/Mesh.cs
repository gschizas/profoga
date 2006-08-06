/* $RCSfile$
* $Author: egonw $
* $Date: 2005-11-10 16:52:44 +0100 (jeu., 10 nov. 2005) $
* $Revision: 4255 $
*
* Copyright (C) 2005  Miguel, The Jmol Development Team
*
* Contact: miguel@jmol.org, jmol-developers@lists.sf.net
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
using org.jmol.g3d;
//UPGRADE_TODO: The type 'javax.vecmath.Point3f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Point3f = javax.vecmath.Point3f;
//UPGRADE_TODO: The type 'javax.vecmath.Vector3f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Vector3f = javax.vecmath.Vector3f;
namespace org.jmol.viewer
{
	
	class Mesh
	{
		virtual internal bool Translucent
		{
			set
			{
				colix = Graphics3D.setTranslucent(colix, value);
				if (vertexColixes != null)
					for (int i = vertexCount; --i >= 0; )
						vertexColixes[i] = Graphics3D.setTranslucent(vertexColixes[i], value);
			}
			
		}
		virtual internal int VertexCount
		{
			set
			{
				this.vertexCount = value;
				vertices = new Point3f[value];
			}
			
		}
		virtual internal int PolygonCount
		{
			set
			{
				this.polygonCount = value;
				polygonIndexes = new int[value][];
			}
			
		}
		virtual internal short Colix
		{
			set
			{
				this.colix = value;
			}
			
		}
		internal Viewer viewer;
		
		internal System.String meshID;
		internal bool visible = true;
		internal short colix;
		internal short[] vertexColixes;
		internal Graphics3D g3d;
		
		internal int vertexCount;
		internal Point3f[] vertices;
		internal short[] normixes;
		internal int polygonCount;
		internal int[][] polygonIndexes;
		
		internal bool showPoints = false;
		internal bool drawTriangles = false;
		internal bool fillTriangles = true;
		
		internal Mesh(Viewer viewer, System.String meshID, Graphics3D g3d, short colix)
		{
			this.viewer = viewer;
			this.meshID = meshID;
			this.g3d = g3d;
			this.colix = colix;
		}
		
		
		internal virtual void  clear()
		{
			vertexCount = polygonCount = 0;
			vertices = null;
			polygonIndexes = null;
		}
		
		internal virtual void  initialize()
		{
			Vector3f[] vectorSums = new Vector3f[vertexCount];
			for (int i = vertexCount; --i >= 0; )
				vectorSums[i] = new Vector3f();
			sumVertexNormals(vectorSums);
			normixes = new short[vertexCount];
			for (int i = vertexCount; --i >= 0; )
			{
				normixes[i] = g3d.get2SidedNormix(vectorSums[i]);
				/*
				System.out.println("vectorSums[" + i + "]=" + vectorSums[i] +
				" -> normix:" + normixes[i]);
				*/
			}
		}
		
		internal virtual void  allocVertexColixes()
		{
			if (vertexColixes == null)
			{
				vertexColixes = new short[vertexCount];
				for (int i = vertexCount; --i >= 0; )
					vertexColixes[i] = colix;
			}
		}
		
		internal virtual void  sumVertexNormals(Vector3f[] vectorSums)
		{
			//UPGRADE_NOTE: Final was removed from the declaration of 'vNormalizedNormal '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
			Vector3f vNormalizedNormal = new Vector3f();
			
			for (int i = polygonCount; --i >= 0; )
			{
				int[] pi = polygonIndexes[i];
				g3d.calcNormalizedNormal(vertices[pi[0]], vertices[pi[1]], vertices[pi[2]], vNormalizedNormal);
				for (int j = pi.Length; --j >= 0; )
				{
					int k = pi[j];
					vectorSums[k].add(vNormalizedNormal);
				}
			}
		}
		
		internal virtual int addVertexCopy(Point3f vertex)
		{
			if (vertexCount == 0)
				vertices = new Point3f[256];
			else if (vertexCount == vertices.length)
				vertices = (Point3f[]) Util.doubleLength(vertices);
			vertices[vertexCount] = new Point3f(vertex);
			return vertexCount++;
		}
		
		internal virtual void  addTriangle(int vertexA, int vertexB, int vertexC)
		{
			if (polygonCount == 0)
				polygonIndexes = new int[256][];
			else if (polygonCount == polygonIndexes.Length)
				polygonIndexes = (int[][]) Util.doubleLength(polygonIndexes);
			int[] polygon = new int[]{vertexA, vertexB, vertexC};
			polygonIndexes[polygonCount++] = polygon;
		}
		
		internal virtual void  checkForDuplicatePoints(float cutoff)
		{
			float cutoff2 = cutoff * cutoff;
			for (int i = vertexCount; --i >= 0; )
				for (int j = i; --j >= 0; )
				{
					float dist2 = vertices[i].distanceSquared(vertices[j]);
					if (dist2 < cutoff2)
					{
						System.Console.Out.WriteLine("Mesh.checkForDuplicates " + vertices[i] + "<->" + vertices[j] + " : " + System.Math.Sqrt(dist2));
					}
				}
		}
	}
}