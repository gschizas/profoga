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
using Graphics3D = org.jmol.g3d.Graphics3D;
//UPGRADE_TODO: The package 'javax.vecmath' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using javax.vecmath;
namespace org.jmol.viewer
{
	
	class DotsRenderer:ShapeRenderer
	{
		virtual internal int TorusIncrement
		{
			get
			{
				if (scalePixelsPerAngstrom <= 5)
					return 16;
				if (scalePixelsPerAngstrom <= 10)
					return 8;
				if (scalePixelsPerAngstrom <= 20)
					return 4;
				if (scalePixelsPerAngstrom <= 40)
					return 2;
				return 1;
			}
			
		}
		virtual internal int TorusOuterDotCount
		{
			get
			{
				int dotCount = 8;
				if (scalePixelsPerAngstrom > 5)
				{
					dotCount = 16;
					if (scalePixelsPerAngstrom > 10)
					{
						dotCount = 32;
						if (scalePixelsPerAngstrom > 20)
						{
							dotCount = 64;
						}
					}
				}
				return dotCount;
			}
			
		}
		
		internal bool perspectiveDepth;
		internal int scalePixelsPerAngstrom;
		internal bool bondSelectionModeOr;
		
		internal Geodesic geodesic;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'mapNull '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_NOTE: The initialization of  'mapNull' was moved to static method 'org.jmol.viewer.DotsRenderer'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		internal static readonly int[] mapNull;
		
		internal override void  initRenderer()
		{
			
			this.geodesic = new Geodesic(this); // 12 vertices
			geodesic.quadruple(); // 12 * 4 - 6 = 42 vertices
			geodesic.quadruple(); // 42 * 4 - 6 = 162 vertices
			geodesic.quadruple(); // 162 * 4 - 6 = 642 vertices
			//    geodesic.quadruple(); // 642 * 4 - 6 = 2562 vertices
		}
		
		internal override void  render()
		{
			perspectiveDepth = viewer.PerspectiveDepth;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			scalePixelsPerAngstrom = (int) viewer.ScalePixelsPerAngstrom;
			bondSelectionModeOr = viewer.BondSelectionModeOr;
			
			
			geodesic.transform();
			Dots dots = (Dots) shape;
			if (dots == null)
				return ;
			Atom[] atoms = frame.atoms;
			int[][] dotsConvexMaps = dots.dotsConvexMaps;
			short[] colixesConvex = dots.colixesConvex;
			int displayModelIndex = this.displayModelIndex;
			for (int i = dots.dotsConvexMax; --i >= 0; )
			{
				int[] map = dotsConvexMaps[i];
				if (map != null && map != mapNull)
				{
					Atom atom = atoms[i];
					if (displayModelIndex < 0 || displayModelIndex == atom.modelIndex)
						renderConvex(dots, atom, colixesConvex[i], map);
				}
			}
			Dots.Torus[] tori = dots.tori;
			for (int i = dots.torusCount; --i >= 0; )
			{
				Dots.Torus torus = tori[i];
				if (displayModelIndex < 0 || displayModelIndex == atoms[torus.ixI].modelIndex)
					renderTorus(torus, atoms, colixesConvex, dotsConvexMaps);
			}
			Dots.Cavity[] cavities = dots.cavities;
			if (false)
			{
				System.Console.Out.WriteLine("concave surface rendering currently disabled");
				return ;
			}
			for (int i = dots.cavityCount; --i >= 0; )
			{
				Dots.Cavity cavity = cavities[i];
				if (displayModelIndex < 0 || displayModelIndex == atoms[cavity.ixI].modelIndex)
					renderCavity(cavities[i], atoms, colixesConvex, dotsConvexMaps);
			}
		}
		
		internal virtual void  renderConvex(Dots dots, Atom atom, short colix, int[] visibilityMap)
		{
			geodesic.calcScreenPoints(visibilityMap, dots.getAppropriateRadius(atom), atom.ScreenX, atom.ScreenY, atom.ScreenZ);
			if (geodesic.screenCoordinateCount > 0)
				g3d.plotPoints(Graphics3D.inheritColix(colix, atom.colixAtom), geodesic.screenCoordinateCount, geodesic.screenCoordinates);
		}
		
		internal Point3f pointT = new Point3f();
		internal Point3f pointT1 = new Point3f();
		internal Matrix3f matrixT = new Matrix3f();
		internal Matrix3f matrixT1 = new Matrix3f();
		internal Matrix3f matrixRot = new Matrix3f();
		internal AxisAngle4f aaT = new AxisAngle4f();
		internal AxisAngle4f aaT1 = new AxisAngle4f();
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'torusStepAngle '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
		internal static readonly float torusStepAngle = 2 * (float) System.Math.PI / 64;
		
		internal virtual void  renderTorus(Dots.Torus torus, Atom[] atoms, short[] colixes, int[][] dotsConvexMaps)
		{
			if (dotsConvexMaps[torus.ixI] != null)
				renderTorusHalf(torus, getColix(torus.colixI, colixes, atoms, torus.ixI), false);
			if (dotsConvexMaps[torus.ixJ] != null)
				renderTorusHalf(torus, getColix(torus.colixJ, colixes, atoms, torus.ixJ), true);
		}
		
		internal virtual short getColix(short colix, short[] colixes, Atom[] atoms, int index)
		{
			return Graphics3D.inheritColix(colix, atoms[index].colixAtom);
		}
		
		internal virtual void  renderTorusHalf(Dots.Torus torus, short colix, bool renderJHalf)
		{
			g3d.setColix(colix);
			long probeMap = torus.probeMap;
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			int torusDotCount1 = (int) (TorusOuterDotCount * torus.outerAngle / (2 * System.Math.PI));
			float stepAngle1 = torus.outerAngle / torusDotCount1;
			if (renderJHalf)
				stepAngle1 = - stepAngle1;
			aaT1.set_Renamed(torus.tangentVector, 0);
			
			aaT.set_Renamed(torus.axisVector, 0);
			int step = TorusIncrement;
			for (int i = 0; probeMap != 0; i += step, probeMap <<= step)
			{
				if (probeMap >= 0)
					continue;
				aaT.angle = i * torusStepAngle;
				matrixT.set_Renamed(aaT);
				matrixT.transform(torus.radialVector, pointT);
				pointT.add(torus.center);
				
				for (int j = torusDotCount1; --j >= 0; )
				{
					aaT1.angle = j * stepAngle1;
					matrixT1.set_Renamed(aaT1);
					matrixT1.transform(torus.outerRadial, pointT1);
					matrixT.transform(pointT1);
					pointT1.add(pointT);
					g3d.drawPixel(viewer.transformPoint(pointT1));
				}
			}
		}
		
		/// <summary> So, I need some help with this.
		/// I cannot think of a good way to render this cavity.
		/// The shapes are spherical triangle, but are very irregular.
		/// In the center of aromatic rings there are 2-4 ... which looks ugly
		/// So, if you have an idea how to render this, please let me know.
		/// </summary>
		
		internal static sbyte nearI = (sbyte) SupportClass.Identity((1 << 0));
		internal static sbyte nearJ = (sbyte) SupportClass.Identity((1 << 1));
		internal static sbyte nearK = (sbyte) SupportClass.Identity((1 << 2));
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'nearAssociations '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly sbyte[] nearAssociations = new sbyte[]{nearI | nearJ | nearK, nearI, nearJ, nearK, nearI | nearJ, nearJ | nearK, nearK | nearI, nearI, nearJ, nearJ, nearK, nearK, nearI, nearI, nearJ, nearK, nearI | nearJ, nearJ | nearK, nearK | nearI, nearI, nearJ, nearJ, nearK, nearK, nearI};
		
		internal virtual void  renderCavity(Dots.Cavity cavity, Atom[] atoms, short[] colixes, int[][] dotsConvexMaps)
		{
			Point3f[] points = cavity.points;
			if (dotsConvexMaps[cavity.ixI] != null)
			{
				g3d.setColix(getColix(cavity.colixI, colixes, atoms, cavity.ixI));
				renderCavityThird(points, 0);
			}
			if (dotsConvexMaps[cavity.ixJ] != null)
			{
				g3d.setColix(getColix(cavity.colixJ, colixes, atoms, cavity.ixJ));
				renderCavityThird(points, 1);
			}
			if (dotsConvexMaps[cavity.ixK] != null)
			{
				g3d.setColix(getColix(cavity.colixK, colixes, atoms, cavity.ixK));
				renderCavityThird(points, 2);
			}
		}
		
		internal virtual void  renderCavityThird(Point3f[] points, int which)
		{
			Point3i screen;
			for (int i = points.length; --i >= 0; )
			{
				if ((nearAssociations[i] & (1 << which)) != 0)
				{
					screen = viewer.transformPoint(points[i]);
					g3d.drawPixel(screen);
				}
			}
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'halfRoot5 '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
		internal static readonly float halfRoot5 = (float) (0.5 * System.Math.Sqrt(5));
		//UPGRADE_NOTE: Final was removed from the declaration of 'oneFifth '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
		internal static readonly float oneFifth = 2 * (float) System.Math.PI / 5;
		//UPGRADE_NOTE: Final was removed from the declaration of 'oneTenth '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly float oneTenth = oneFifth / 2;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'faceIndicesInitial'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly short[] faceIndicesInitial = new short[]{0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 5, 0, 5, 1, 1, 6, 2, 2, 7, 3, 3, 8, 4, 4, 9, 5, 5, 10, 1, 6, 1, 10, 7, 2, 6, 8, 3, 7, 9, 4, 8, 10, 5, 9, 11, 6, 10, 11, 7, 6, 11, 8, 7, 11, 9, 8, 11, 10, 9};
		
		/// <summary>*************************************************************
		/// This code constructs a geodesic sphere which is used to
		/// represent the vanderWaals and Connolly dot surfaces
		/// One geodesic sphere is constructed. It is a unit sphere
		/// with radius of 1.0 <p>
		/// Many times a sphere is constructed with lines of latitude and
		/// longitude. With this type of rendering, the atom has north and
		/// south poles. And the faces are not regularly shaped ... at the
		/// poles they are triangles but elsewhere they are quadrilaterals. <p>
		/// I think that a geodesic sphere is more appropriate for this type
		/// of application. The geodesic sphere does not have poles and 
		/// looks the same in all orientations ... as a sphere should. All
		/// faces are equilateral triangles. <p>
		/// The geodesic sphere is constructed by starting with an icosohedron, 
		/// a platonic solid with 12 vertices and 20 equilateral triangles
		/// for faces. The call to the method <code>quadruple</code> will
		/// split each triangular face into 4 faces by creating a new vertex
		/// at the midpoint of each edge. These midpoints are still in the
		/// plane, so they are then 'pushed out' to the surface of the
		/// enclosing sphere by normalizing their length back to 1.0<p>
		/// Individual atoms construct bitmaps to determine which dots are
		/// visible and which are obscured. Each bit corresponds to a single
		/// dot.<p>
		/// The sequence of vertex counts is 12, 42, 162, 642. The vertices
		/// are stored so that when atoms are small they can choose to display
		/// only the first n bits where n is one of the above vertex counts.<p>
		/// The vertices of the 'one true sphere' are rotated to the current
		/// molecular rotation at the beginning of the repaint cycle. That way,
		/// individual atoms only need to scale the unit vector to the vdw
		/// radius for that atom. <p>
		/// (If necessary, this on-the-fly scaling could be eliminated by
		/// storing multiple geodesic spheres ... one per vdw radius. But
		/// I suspect there are bigger performance problems with the saddle
		/// and convex connolly surfaces.)<p>
		/// I experimented with rendering the dots with light shading. However
		/// I found that it was much harder to look at. The dots in the front
		/// are lighter, but on a white background they are harder to see. The
		/// end result is that I tended to focus on the back side of the sphere
		/// of dots ... which made rotations very strange. So I turned off
		/// shading of dot surfaces.
		/// **************************************************************
		/// </summary>
		
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'Geodesic' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		internal class Geodesic
		{
			private void  InitBlock(DotsRenderer enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private DotsRenderer enclosingInstance;
			public DotsRenderer Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			internal Vector3f[] vertices;
			internal Vector3f[] verticesTransformed;
			//    byte[] intensitiesTransformed;
			internal int screenCoordinateCount;
			internal int[] screenCoordinates;
			//    byte[] intensities;
			internal short[] faceIndices;
			
			internal Geodesic(DotsRenderer enclosingInstance)
			{
				InitBlock(enclosingInstance);
				vertices = new Vector3f[12];
				vertices[0] = new Vector3f(0, 0, org.jmol.viewer.DotsRenderer.halfRoot5);
				for (int i = 0; i < 5; ++i)
				{
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					vertices[i + 1] = new Vector3f((float) System.Math.Cos(i * org.jmol.viewer.DotsRenderer.oneFifth), (float) System.Math.Sin(i * org.jmol.viewer.DotsRenderer.oneFifth), 0.5f);
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					vertices[i + 6] = new Vector3f((float) System.Math.Cos(i * org.jmol.viewer.DotsRenderer.oneFifth + org.jmol.viewer.DotsRenderer.oneTenth), (float) System.Math.Sin(i * org.jmol.viewer.DotsRenderer.oneFifth + org.jmol.viewer.DotsRenderer.oneTenth), - 0.5f);
				}
				vertices[11] = new Vector3f(0, 0, - org.jmol.viewer.DotsRenderer.halfRoot5);
				for (int i = 12; --i >= 0; )
					vertices[i].normalize();
				faceIndices = org.jmol.viewer.DotsRenderer.faceIndicesInitial;
				verticesTransformed = new Vector3f[12];
				for (int i = 12; --i >= 0; )
					verticesTransformed[i] = new Vector3f();
				screenCoordinates = new int[3 * 12];
				//      intensities = new byte[12];
				//      intensitiesTransformed = new byte[12];
			}
			
			internal virtual void  transform()
			{
				for (int i = vertices.length; --i >= 0; )
				{
					Vector3f t = verticesTransformed[i];
					Enclosing_Instance.viewer.transformVector(vertices[i], t);
					//        intensitiesTransformed[i] =
					//          Shade3D.calcIntensity((float)t.x, (float)t.y, (float)t.z);
				}
			}
			
			internal virtual void  calcScreenPoints(int[] visibilityMap, float radius, int x, int y, int z)
			{
				int dotCount = 12;
				if (Enclosing_Instance.scalePixelsPerAngstrom > 5)
				{
					dotCount = 42;
					if (Enclosing_Instance.scalePixelsPerAngstrom > 10)
					{
						dotCount = 162;
						if (Enclosing_Instance.scalePixelsPerAngstrom > 20)
						{
							dotCount = 642;
							//		  if (scalePixelsPerAngstrom > 32)
							//		      dotCount = 2562;
						}
					}
				}
				
				float scaledRadius = Enclosing_Instance.viewer.scaleToPerspective(z, radius);
				int icoordinates = 0;
				//      int iintensities = 0;
				int iDot = visibilityMap.Length << 5;
				screenCoordinateCount = 0;
				if (iDot > dotCount)
					iDot = dotCount;
				while (--iDot >= 0)
				{
					if (!org.jmol.viewer.DotsRenderer.getBit(visibilityMap, iDot))
						continue;
					//        intensities[iintensities++] = intensitiesTransformed[iDot];
					Vector3f vertex = verticesTransformed[iDot];
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					screenCoordinates[icoordinates++] = x + (int) ((scaledRadius * vertex.x) + (vertex.x < 0?- 0.5:0.5));
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					screenCoordinates[icoordinates++] = y + (int) ((scaledRadius * vertex.y) + (vertex.y < 0?- 0.5:0.5));
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					screenCoordinates[icoordinates++] = z + (int) ((scaledRadius * vertex.z) + (vertex.z < 0?- 0.5:0.5));
					++screenCoordinateCount;
				}
			}
			
			internal short iVertexNew;
			internal System.Collections.Hashtable htVertex;
			
			internal virtual void  quadruple()
			{
				htVertex = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
				int nVerticesOld = vertices.length;
				short[] faceIndicesOld = faceIndices;
				int nFaceIndicesOld = faceIndicesOld.Length;
				int nEdgesOld = nVerticesOld + nFaceIndicesOld / 3 - 2;
				int nVerticesNew = nVerticesOld + nEdgesOld;
				Vector3f[] verticesNew = new Vector3f[nVerticesNew];
				Array.Copy(vertices, 0, verticesNew, 0, nVerticesOld);
				vertices = verticesNew;
				verticesTransformed = new Vector3f[nVerticesNew];
				for (int i = nVerticesNew; --i >= 0; )
					verticesTransformed[i] = new Vector3f();
				screenCoordinates = new int[3 * nVerticesNew];
				//      intensitiesTransformed = new byte[nVerticesNew];
				//      intensities
				
				short[] faceIndicesNew = new short[4 * nFaceIndicesOld];
				faceIndices = faceIndicesNew;
				iVertexNew = (short) nVerticesOld;
				
				int iFaceNew = 0;
				for (int i = 0; i < nFaceIndicesOld; )
				{
					short iA = faceIndicesOld[i++];
					short iB = faceIndicesOld[i++];
					short iC = faceIndicesOld[i++];
					short iAB = getVertex(iA, iB);
					short iBC = getVertex(iB, iC);
					short iCA = getVertex(iC, iA);
					
					faceIndicesNew[iFaceNew++] = iA;
					faceIndicesNew[iFaceNew++] = iAB;
					faceIndicesNew[iFaceNew++] = iCA;
					
					faceIndicesNew[iFaceNew++] = iB;
					faceIndicesNew[iFaceNew++] = iBC;
					faceIndicesNew[iFaceNew++] = iAB;
					
					faceIndicesNew[iFaceNew++] = iC;
					faceIndicesNew[iFaceNew++] = iCA;
					faceIndicesNew[iFaceNew++] = iBC;
					
					faceIndicesNew[iFaceNew++] = iCA;
					faceIndicesNew[iFaceNew++] = iAB;
					faceIndicesNew[iFaceNew++] = iBC;
				}
				if (iFaceNew != faceIndicesNew.Length)
				{
					System.Console.Out.WriteLine("que?");
					throw new System.NullReferenceException();
				}
				if (iVertexNew != nVerticesNew)
				{
					System.Console.Out.WriteLine("huh? " + " iVertexNew=" + iVertexNew + "nVerticesNew=" + nVerticesNew);
					throw new System.NullReferenceException();
				}
				htVertex = null;
				//      bitmap = allocateBitmap(nVerticesNew);
			}
			
			private short getVertex(short i1, short i2)
			{
				if (i1 > i2)
				{
					short t = i1;
					i1 = i2;
					i2 = t;
				}
				System.Int32 hashKey = (System.Int32) ((i1 << 16) + i2);
				System.Int16 iv = (System.Int16) htVertex[hashKey];
				//UPGRADE_TODO: The 'System.Int16' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
				if (iv != null)
					return (short) iv;
				Vector3f vertexNew = new Vector3f(vertices[i1]);
				vertexNew.add(vertices[i2]);
				vertexNew.scale(0.5f);
				vertexNew.normalize();
				htVertex[hashKey] = (short) iVertexNew;
				vertices[iVertexNew] = vertexNew;
				return iVertexNew++;
			}
		}
		/*
		private final static int[] allocateBitmap(int count) {
		return new int[(count + 31) >> 5];
		}
		
		private final static void setBit(int[] bitmap, int i) {
		bitmap[(i >> 5)] |= 1 << (~i & 31);
		}
		
		private final static void clearBit(int[] bitmap, int i) {
		bitmap[(i >> 5)] &= ~(1 << (~i & 31));
		}
		*/
		internal static bool getBit(int[] bitmap, int i)
		{
			return (bitmap[(i >> 5)] << (i & 31)) < 0;
		}
		/*
		private final static void setAllBits(int[] bitmap, int count) {
		int i = count >> 5;
		if ((count & 31) != 0)
		bitmap[i] = 0x80000000 >> (count - 1);
		while (--i >= 0)
		bitmap[i] = -1;
		}
		
		private final static void clearBitmap(int[] bitmap) {
		for (int i = bitmap.length; --i >= 0; )
		bitmap[i] = 0;
		}
		*/
		static DotsRenderer()
		{
			mapNull = Dots.mapNull;
		}
	}
}