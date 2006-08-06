/* $RCSfile$
* $Author: egonw $
* $Date: 2005-11-10 16:52:44 +0100 (jeu., 10 nov. 2005) $
* $Revision: 4255 $
*
* Copyright (C) 2002-2005  The Jmol Development Team
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
namespace org.jmol.viewer
{
	
	class Pmesh:MeshCollection
	{
		
		internal override void  setProperty(System.String propertyName, System.Object value_Renamed, System.Collections.BitArray bs)
		{
			if ((System.Object) "bufferedreader" == (System.Object) propertyName)
			{
				System.IO.StreamReader br = (System.IO.StreamReader) value_Renamed;
				if (currentMesh == null)
					allocMesh(null);
				currentMesh.clear();
				readPmesh(br);
				currentMesh.initialize();
				currentMesh.visible = true;
				return ;
			}
			base.setProperty(propertyName, value_Renamed, bs);
		}
		
		/*
		* vertexCount
		* x.xx y.yy z.zz {vertices}
		* polygonCount
		*
		*/
		
		internal virtual void  readPmesh(System.IO.StreamReader br)
		{
			//    System.out.println("Pmesh.readPmesh(" + br + ")");
			try
			{
				readVertexCount(br);
				//      System.out.println("vertexCount=" + currentMesh.vertexCount);
				readVertices(br);
				//      System.out.println("vertices read");
				readPolygonCount(br);
				//      System.out.println("polygonCount=" + currentMesh.polygonCount);
				readPolygonIndexes(br);
				//      System.out.println("polygonIndexes read");
			}
			catch (System.Exception e)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				System.Console.Out.WriteLine("Pmesh.readPmesh exception:" + e);
			}
		}
		
		internal virtual void  readVertexCount(System.IO.StreamReader br)
		{
			currentMesh.VertexCount = parseInt(br.ReadLine());
		}
		
		internal virtual void  readVertices(System.IO.StreamReader br)
		{
			if (currentMesh.vertexCount > 0)
			{
				for (int i = 0; i < currentMesh.vertexCount; ++i)
				{
					System.String line = br.ReadLine();
					float x = parseFloat(line);
					float y = parseFloat(line, ichNextParse);
					float z = parseFloat(line, ichNextParse);
					currentMesh.vertices[i] = new Point3f(x, y, z);
				}
			}
		}
		
		internal virtual void  readPolygonCount(System.IO.StreamReader br)
		{
			currentMesh.PolygonCount = parseInt(br.ReadLine());
		}
		
		internal virtual void  readPolygonIndexes(System.IO.StreamReader br)
		{
			if (currentMesh.polygonCount > 0)
			{
				for (int i = 0; i < currentMesh.polygonCount; ++i)
					currentMesh.polygonIndexes[i] = readPolygon(br);
			}
		}
		
		internal virtual int[] readPolygon(System.IO.StreamReader br)
		{
			int vertexIndexCount = parseInt(br.ReadLine());
			if (vertexIndexCount < 4)
				return null;
			int vertexCount = vertexIndexCount - 1;
			int[] vertices = new int[vertexCount];
			for (int i = 0; i < vertexCount; ++i)
				vertices[i] = parseInt(br.ReadLine());
			int extraVertex = parseInt(br.ReadLine());
			if (extraVertex != vertices[0])
			{
				System.Console.Out.WriteLine("?Que? polygon is not complete");
				throw new System.NullReferenceException();
			}
			return vertices;
		}
	}
}