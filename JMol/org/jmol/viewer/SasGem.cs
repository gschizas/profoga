/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-03-28 02:59:10 +0200 (mar., 28 mars 2006) $
* $Revision: 4799 $
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
using Bmp = org.jmol.util.Bmp;
using Graphics3D = org.jmol.g3d.Graphics3D;
//UPGRADE_TODO: The package 'javax.vecmath' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using javax.vecmath;
namespace org.jmol.viewer
{
	
	/// <summary> The SasGem is the Solvent Accessible Surface Geodesic Edge Machine.</summary>
	
	class SasGem
	{
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'g3d '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Graphics3D g3d;
		//UPGRADE_NOTE: Final was removed from the declaration of 'viewer '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Viewer viewer;
		//UPGRADE_NOTE: Final was removed from the declaration of 'frame '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Frame frame;
		//UPGRADE_NOTE: Final was removed from the declaration of 'geodesicLevel '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal int geodesicLevel;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'fplIdeal '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal SasFlattenedPointList fplIdeal;
		//UPGRADE_NOTE: Final was removed from the declaration of 'fplActual '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal SasFlattenedPointList fplActual;
		//UPGRADE_NOTE: Final was removed from the declaration of 'fplVisibleIdeal '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal SasFlattenedPointList fplVisibleIdeal;
		//UPGRADE_NOTE: Final was removed from the declaration of 'fplTorusSegment '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal SasFlattenedPointList fplTorusSegment;
		//UPGRADE_NOTE: Final was removed from the declaration of 'fplForStitching '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal SasFlattenedPointList fplForStitching;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'geodesicVertexVectors '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f[] geodesicVertexVectors;
		//UPGRADE_NOTE: Final was removed from the declaration of 'geodesicVertexCount '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal int geodesicVertexCount;
		//UPGRADE_NOTE: Final was removed from the declaration of 'geodesicFaceCount '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal int geodesicFaceCount;
		//UPGRADE_NOTE: Final was removed from the declaration of 'geodesicFaceVertexes '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal short[] geodesicFaceVertexes;
		//UPGRADE_NOTE: Final was removed from the declaration of 'geodesicNeighborVertexes '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal short[] geodesicNeighborVertexes;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'centerVectorT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f centerVectorT = new Vector3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'vertexPointT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Point3f vertexPointT = new Point3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'vertexVectorT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f vertexVectorT = new Vector3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'projectedPointT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Point3f projectedPointT = new Point3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'projectedVectorT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f projectedVectorT = new Vector3f();
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'bmpNotClippedT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal int[] bmpNotClippedT;
		//UPGRADE_NOTE: Final was removed from the declaration of 'bmpClippedT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal int[] bmpClippedT;
		//UPGRADE_NOTE: Final was removed from the declaration of 'idealEdgeMapT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal int[] idealEdgeMapT;
		//UPGRADE_NOTE: Final was removed from the declaration of 'actualEdgeMapT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal int[] actualEdgeMapT;
		//UPGRADE_NOTE: Final was removed from the declaration of 'visibleIdealEdgeMapT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal int[] visibleIdealEdgeMapT;
		//UPGRADE_NOTE: Final was removed from the declaration of 'faceMapT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal int[] faceMapT;
		//UPGRADE_NOTE: Final was removed from the declaration of 'vertexMapT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal int[] vertexMapT;
		
		internal short firstStitchedGeodesicVertex;
		internal short lastStitchedGeodesicVertex;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'PI '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
		private static readonly float PI = (float) System.Math.PI;
		//UPGRADE_NOTE: Final was removed from the declaration of 'MAX_FULL_TORUS_STEP_COUNT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_NOTE: The initialization of  'MAX_FULL_TORUS_STEP_COUNT' was moved to static method 'org.jmol.viewer.SasGem'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private static readonly int MAX_FULL_TORUS_STEP_COUNT;
		
		internal SasGem(Viewer viewer, Graphics3D g3d, Frame frame, int geodesicLevel)
		{
			this.g3d = g3d;
			this.viewer = viewer;
			this.frame = frame;
			this.geodesicLevel = geodesicLevel;
			
			geodesicVertexVectors = g3d.GeodesicVertexVectors;
			geodesicVertexCount = g3d.getGeodesicVertexCount(geodesicLevel);
			geodesicFaceCount = g3d.getGeodesicFaceCount(geodesicLevel);
			geodesicFaceVertexes = g3d.getGeodesicFaceVertexes(geodesicLevel);
			geodesicNeighborVertexes = g3d.getGeodesicNeighborVertexes(geodesicLevel);
			
			bmpNotClippedT = Bmp.allocateBitmap(geodesicVertexCount);
			bmpClippedT = Bmp.allocateBitmap(geodesicVertexCount);
			idealEdgeMapT = Bmp.allocateBitmap(geodesicVertexCount);
			actualEdgeMapT = Bmp.allocateBitmap(geodesicVertexCount);
			visibleIdealEdgeMapT = Bmp.allocateBitmap(geodesicVertexCount);
			faceMapT = Bmp.allocateBitmap(geodesicFaceCount);
			vertexMapT = Bmp.allocateBitmap(geodesicFaceCount);
			
			fplIdeal = new SasFlattenedPointList(g3d, geodesicLevel);
			fplActual = new SasFlattenedPointList(g3d, geodesicLevel);
			fplVisibleIdeal = new SasFlattenedPointList(g3d, geodesicLevel);
			fplTorusSegment = new SasFlattenedPointList(g3d, geodesicLevel);
			fplForStitching = new SasFlattenedPointList(g3d, geodesicLevel);
		}
		
		internal virtual void  reset()
		{
			stitchesCount = 0;
		}
		
		
		internal virtual bool findIdealEdge(bool isEdgeA, Point3f geodesicCenter, float geodesicRadius, Point3f planeCenter, Vector3f planeUnitNormal, int[] idealEdgeMap)
		{
			Bmp.clearBitmap(bmpNotClippedT);
			Bmp.clearBitmap(idealEdgeMap);
			int unclippedCount = 0;
			for (int i = geodesicVertexCount; --i >= 0; )
			{
				vertexPointT.scaleAdd(geodesicRadius, geodesicVertexVectors[i], geodesicCenter);
				vertexVectorT.sub(vertexPointT, planeCenter);
				float dot = vertexVectorT.dot(planeUnitNormal);
				if (isEdgeA)
					dot = - dot;
				if (dot >= 0)
				{
					++unclippedCount;
					Bmp.setBit(bmpNotClippedT, i);
				}
			}
			if (unclippedCount == 0)
				return false; // everything is clipped ... inside another atom
			if (unclippedCount == geodesicVertexCount)
			{
				findClippedFaceVertexes(isEdgeA, geodesicCenter, geodesicRadius, planeCenter, planeUnitNormal, idealEdgeMap);
				return false;
			}
			for (int v = - 1; (v = Bmp.nextSetBit(bmpNotClippedT, v + 1)) >= 0; )
			{
				int neighborsOffset = v * 6;
				for (int j = (v < Graphics3D.GEODESIC_START_VERTEX_COUNT)?Graphics3D.GEODESIC_START_NEIGHBOR_COUNT:6; --j >= 0; )
				{
					int neighbor = geodesicNeighborVertexes[neighborsOffset + j];
					if (!Bmp.getBit(bmpNotClippedT, neighbor))
					{
						Bmp.setBit(idealEdgeMap, v);
						break;
					}
				}
			}
			return true;
		}
		
		internal virtual bool findIdealInsideEdge(bool isEdgeA, Point3f geodesicCenter, float geodesicRadius, Point3f planeCenter, Vector3f planeUnitNormal, int[] idealInsideEdgeMap)
		{
			Bmp.clearBitmap(bmpClippedT);
			Bmp.clearBitmap(idealInsideEdgeMap);
			int clippedCount = 0;
			for (int i = geodesicVertexCount; --i >= 0; )
			{
				vertexPointT.scaleAdd(geodesicRadius, geodesicVertexVectors[i], geodesicCenter);
				vertexVectorT.sub(vertexPointT, planeCenter);
				float dot = vertexVectorT.dot(planeUnitNormal);
				if (isEdgeA)
					dot = - dot;
				if (dot <= 0)
				{
					++clippedCount;
					Bmp.setBit(bmpClippedT, i);
				}
			}
			if (clippedCount == 0)
				return false; // nothing is clipped ... 
			if (clippedCount == geodesicVertexCount)
				return false; // everything is clipped ... buried inside another atom
			for (int v = - 1; (v = Bmp.nextSetBit(bmpClippedT, v + 1)) >= 0; )
			{
				int neighborsOffset = v * 6;
				for (int j = (v < 12)?5:6; --j >= 0; )
				{
					int neighbor = geodesicNeighborVertexes[neighborsOffset + j];
					if (!Bmp.getBit(bmpClippedT, neighbor))
					{
						Bmp.setBit(idealInsideEdgeMap, v);
						break;
					}
				}
			}
			return true;
		}
		
		internal virtual bool findActualEdge(int[] visibleVertexMap, int[] actualEdgeMap)
		{
			int edgeVertexCount = 0;
			Bmp.clearBitmap(actualEdgeMap);
			for (int v = - 1; (v = Bmp.nextSetBit(visibleVertexMap, v + 1)) >= 0; )
			{
				int neighborsOffset = v * 6;
				for (int j = ((v < Graphics3D.GEODESIC_START_VERTEX_COUNT)?Graphics3D.GEODESIC_START_NEIGHBOR_COUNT:6); --j >= 0; )
				{
					int neighbor = geodesicNeighborVertexes[neighborsOffset + j];
					if (!Bmp.getBit(visibleVertexMap, neighbor))
					{
						Bmp.setBit(actualEdgeMap, v);
						++edgeVertexCount;
						break;
					}
				}
			}
			return edgeVertexCount > 0;
		}
		
		internal virtual short findClosestVertex(short normix, int[] edgeVertexMap)
		{
			// note that there is some other code in Normix3D.java that
			// does the same thing. See which algorithm works better.
			int champion = - 1;
			float championAngle = PI;
			Vector3f vector = geodesicVertexVectors[normix];
			for (int v = - 1; (v = Bmp.nextSetBit(edgeVertexMap, v + 1)) >= 0; )
			{
				float angle = vector.angle(geodesicVertexVectors[v]);
				if (angle < championAngle)
				{
					championAngle = angle;
					champion = v;
				}
			}
			return (short) champion;
		}
		
		/*
		* only a small piece of the sphere is clipped.
		* That is, none of the vertexes themselves are clipped,
		* but only a piece that is in within a face.
		* So, find the three vertexes that define the face in
		* which the clipped portion of the sphere exists.
		*
		* not working well ... cases are more pathological
		* just disable this.
		*/
		internal virtual void  findClippedFaceVertexes(bool isEdgeA, Point3f geodesicCenter, float radius, Point3f planeCenter, Vector3f planeUnitNormal, int[] edgeVertexMap)
		{
			return ; // works poorly;
			/*
			// isEdgeA is not accounted for in this old code below
			float goldLength, silverLength, bronzeLength;
			goldLength = silverLength = bronzeLength = Float.MAX_VALUE;
			short goldVertex, silverVertex, bronzeVertex;
			goldVertex = silverVertex = bronzeVertex = -1;
			for (int i = geodesicVertexCount; --i >= 0; ) {
			vertexPointT.scaleAdd(radius, geodesicVertexVectors[i],
			geodesicCenter);
			vertexVectorT.sub(vertexPointT, planeCenter);
			float challengerLength = vertexVectorT.length();
			if (challengerLength < goldLength) {
			bronzeLength = challengerLength;
			bronzeVertex = (short)i;
			}
			if (challengerLength < silverLength) {
			bronzeLength = silverLength;
			bronzeVertex = silverVertex;
			silverLength = challengerLength;
			silverVertex = (short)i;
			}
			if (challengerLength < goldLength) {
			silverLength = goldLength;
			silverVertex = goldVertex;
			goldLength = challengerLength;
			goldVertex = (short)i;
			}
			}
			// now, confirm that the 3 closest vertexes are actually neighbors
			// that form a face;
			if (! g3d.isNeighborVertex(goldVertex, silverVertex,
			geodesicLevel) ||
			! g3d.isNeighborVertex(goldVertex, bronzeVertex,
			geodesicLevel) ||
			! g3d.isNeighborVertex(silverVertex, bronzeVertex,
			geodesicLevel)) {
			System.out.println("Strange condition 0xFACE");
			return;
			}
			Bmp.setBit(edgeVertexMap, goldVertex);
			Bmp.setBit(edgeVertexMap, silverVertex);
			Bmp.setBit(edgeVertexMap, bronzeVertex);
			*/
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'aaT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal AxisAngle4f aaT = new AxisAngle4f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'matrixT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Matrix3f matrixT = new Matrix3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'vectorT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f vectorT = new Vector3f();
		
		internal virtual void  calcVectors0and90(Point3f planeCenter, Vector3f axisVector, Point3f planeZeroPoint, Vector3f vector0, Vector3f vector90)
		{
			vector0.sub(planeZeroPoint, planeCenter);
			aaT.set_Renamed(axisVector, PI / 2);
			matrixT.set_Renamed(aaT);
			matrixT.transform(vector0, vector90);
		}
		
		internal int stitchesCount;
		internal short[] stitches = new short[64];
		
		internal float[] segmentVertexAngles = new float[MAX_FULL_TORUS_STEP_COUNT];
		internal short[] segmentVertexes = new short[MAX_FULL_TORUS_STEP_COUNT];
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'vector0T '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f vector0T = new Vector3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'vector90T '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f vector90T = new Vector3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'planeCenterT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Point3f planeCenterT = new Point3f();
		
		internal virtual bool projectAndSortGeodesicPoints(bool isEdgeA, Point3f geodesicCenter, float geodesicRadius, Point3f planeCenter, Vector3f axisUnitVector, Point3f planeZeroPoint, bool fullTorus, int[] convexVertexMap, bool dump)
		{
			if (!findActualEdge(convexVertexMap, actualEdgeMapT))
				return false;
			if (!findIdealEdge(isEdgeA, geodesicCenter, geodesicRadius, planeCenter, axisUnitVector, idealEdgeMapT))
				return false;
			
			Bmp.and(visibleIdealEdgeMapT, idealEdgeMapT, actualEdgeMapT);
			
			calcVectors0and90(planeCenter, axisUnitVector, planeZeroPoint, vector0T, vector90T);
			
			fplActual.setGeodesicEdge(geodesicCenter, geodesicRadius, planeCenter, axisUnitVector, planeZeroPoint, fullTorus, vector0T, vector90T, geodesicVertexVectors, actualEdgeMapT);
			
			fplIdeal.setGeodesicEdge(geodesicCenter, geodesicRadius, planeCenter, axisUnitVector, planeZeroPoint, fullTorus, vector0T, vector90T, geodesicVertexVectors, idealEdgeMapT);
			
			fplVisibleIdeal.setGeodesicEdge(geodesicCenter, geodesicRadius, planeCenter, axisUnitVector, planeZeroPoint, fullTorus, vector0T, vector90T, geodesicVertexVectors, visibleIdealEdgeMapT);
			if (dump)
			{
				System.Console.Out.WriteLine("++++++++++++++++ projectAndSort isEdgeA:" + isEdgeA);
				System.Console.Out.WriteLine("fplActual=");
				fplActual.dump();
				System.Console.Out.WriteLine("fplIdeal=");
				fplIdeal.dump();
				System.Console.Out.WriteLine("fplVisibleIdeal=");
				fplVisibleIdeal.dump();
				System.Console.Out.WriteLine("---------------- projectAndSort");
			}
			return true;
		}
		
		internal virtual void  calcClippingPlaneCenter(Point3f axisPoint, Vector3f axisUnitVector, Point3f planePoint, Point3f planeCenterPoint)
		{
			vectorT.sub(axisPoint, planePoint);
			float distance = axisUnitVector.dot(vectorT);
			planeCenterPoint.scaleAdd(- distance, axisUnitVector, axisPoint);
		}
		
		internal static float calcAngleInThePlane(Vector3f radialVector0, Vector3f radialVector90, Vector3f vectorInQuestion)
		{
			float angle = radialVector0.angle(vectorInQuestion);
			float angle90 = radialVector90.angle(vectorInQuestion);
			if (angle90 > PI / 2)
				angle = 2 * PI - angle;
			return angle;
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'vectorBA '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f vectorBA = new Vector3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'vectorBC '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f vectorBC = new Vector3f();
		
		internal virtual float angleABC(float xA, float yA, float xB, float yB, float xC, float yC)
		{
			double vxAB = xA - xB;
			double vyAB = yA - yB;
			double vxBC = xC - xB;
			double vyBC = yC - yB;
			double dot = vxAB * vxBC + vyAB * vyBC;
			double lenAB = System.Math.Sqrt(vxAB * vxAB + vyAB * vyAB);
			double lenBC = System.Math.Sqrt(vxBC * vxBC + vyBC * vyBC);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			float angle = (float) System.Math.Acos(dot / (lenAB * lenBC));
			return angle;
		}
		
		internal virtual float angleABCRight(float xA, float xB, float xC, float yC)
		{
			double vxAB = xA - xB;
			double vxBC = xC - xB;
			double vyBC = yC;
			double dot = vxAB * vxBC;
			double lenAB = System.Math.Abs(vxAB);
			double lenBC = System.Math.Sqrt(vxBC * vxBC + vyBC * vyBC);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			return (float) System.Math.Acos(dot / (lenAB * lenBC));
		}
		
		internal virtual float angleABCLeft(float xA, float xB, float yB, float xC, float yC)
		{
			double vxAB = xA - xB;
			double vyAB = 0 - yB;
			double vxBC = xC - xB;
			double vyBC = yC - yB;
			double dot = vxAB * vxBC + vyAB * vyBC;
			double lenAB = System.Math.Sqrt(vxAB * vxAB + vyAB * vyAB);
			double lenBC = System.Math.Sqrt(vxBC * vxBC + vyBC * vyBC);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			return (float) System.Math.Acos(dot / (lenAB * lenBC));
		}
		
		internal virtual void  clipGeodesic(bool isEdgeA, Point3f geodesicCenter, float radius, Point3f planePoint, Vector3f axisUnitVector, int[] geodesicVertexMap)
		{
			centerVectorT.sub(geodesicCenter, planePoint);
			float dotCenter = centerVectorT.dot(axisUnitVector);
			if (isEdgeA)
				dotCenter = - dotCenter;
			if (dotCenter >= radius)
			// all points are visible
				return ;
			if (dotCenter < - radius)
			{
				// all points are clipped
				Bmp.clearBitmap(geodesicVertexMap);
				return ;
			}
			for (int i = - 1; (i = Bmp.nextSetBit(geodesicVertexMap, i + 1)) >= 0; )
			{
				vertexPointT.scaleAdd(radius, geodesicVertexVectors[i], geodesicCenter);
				vertexVectorT.sub(vertexPointT, planePoint);
				float dot = vertexVectorT.dot(axisUnitVector);
				if (isEdgeA)
					dot = - dot;
				if (dot < 0)
					Bmp.clearBit(geodesicVertexMap, i);
			}
		}
		
		internal virtual int[] calcFaceBitmap(int[] vertexMap)
		{
			Bmp.clearBitmap(faceMapT);
			for (int i = geodesicFaceCount, j = 3 * (i - 1); --i >= 0; j -= 3)
			{
				if (Bmp.getBit(vertexMap, geodesicFaceVertexes[j]) || Bmp.getBit(vertexMap, geodesicFaceVertexes[j + 1]) || Bmp.getBit(vertexMap, geodesicFaceVertexes[j + 2]))
					Bmp.setBit(faceMapT, i);
			}
			return Bmp.allocMinimalCopy(faceMapT);
		}
		
		internal virtual int[] calcFaceVertexBitmap(int[] faceMap)
		{
			Bmp.clearBitmap(vertexMapT);
			for (int i = - 1; (i = Bmp.nextSetBit(faceMap, i + 1)) >= 0; )
			{
				int j = i * 3;
				Bmp.setBit(vertexMapT, geodesicFaceVertexes[j]);
				Bmp.setBit(vertexMapT, geodesicFaceVertexes[j + 1]);
				Bmp.setBit(vertexMapT, geodesicFaceVertexes[j + 2]);
			}
			return Bmp.allocMinimalCopy(vertexMapT);
		}
		
		internal virtual void  stitchWithTorusSegment(short startingVertex, short vertexIncrement, float startingAngle, float angleIncrement, int stepCount, bool dump)
		{
			float endingAngle = startingAngle + (angleIncrement * (stepCount - 1));
			fplForStitching.buildForStitching(startingAngle, endingAngle, fplIdeal, fplActual, fplVisibleIdeal, dump);
			fplTorusSegment.generateTorusSegment(startingVertex, vertexIncrement, startingAngle, angleIncrement, stepCount);
			stitchEm(fplTorusSegment, fplForStitching, dump);
			if (dump)
			{
				dumpStitches();
			}
		}
		
		internal virtual void  stitchEm(SasFlattenedPointList torusFpl, SasFlattenedPointList geodesicFpl, bool dump)
		{
			if (geodesicFpl.count == 0)
			{
				firstStitchedGeodesicVertex = lastStitchedGeodesicVertex = - 1;
				return ;
			}
			int tLast = torusFpl.count - 1;
			int gLast = geodesicFpl.count - 1;
			firstStitchedGeodesicVertex = geodesicFpl.vertexes[0];
			oneStitch(torusFpl.vertexes[0], firstStitchedGeodesicVertex);
			int t = 0;
			int g = 0;
			while (t < tLast && g < gLast)
			{
				float d1 = geodesicFpl.angles[g + 1] - torusFpl.angles[t];
				float d2 = torusFpl.angles[t + 1] - geodesicFpl.angles[g];
				if (d1 < d2)
					++g;
				else
					++t;
				/*
				float angleT =
				angleABC(torusFpl.angles[t], 0,
				torusFpl.angles[t + 1], 0,
				geodesicFpl.angles[g], geodesicFpl.distances[g]);
				System.out.println("angleT=" + angleT + " : " +
				torusFpl.vertexes[t] + "->" +
				torusFpl.vertexes[t + 1] + "->" +
				"(" + geodesicFpl.vertexes[g] + ")");
				float angleG =
				angleABC(torusFpl.angles[t], 0,
				geodesicFpl.angles[g+1], geodesicFpl.distances[g+1],
				geodesicFpl.angles[g], geodesicFpl.distances[g]);
				System.out.println("angleG=" + angleG + " : " +
				torusFpl.vertexes[t] + "->" +
				"(" + geodesicFpl.vertexes[g + 1] + ")->" +
				"(" + geodesicFpl.vertexes[g] + ")");
				if (angleT > angleG)
				++t;
				else
				++g;
				*/
				oneStitch(torusFpl.vertexes[t], geodesicFpl.vertexes[g]);
			}
			while (t < tLast || g < gLast)
			{
				if (t < tLast)
					++t;
				else
					++g;
				oneStitch(torusFpl.vertexes[t], geodesicFpl.vertexes[g]);
			}
			lastStitchedGeodesicVertex = geodesicFpl.vertexes[gLast];
		}
		
		internal virtual void  oneStitch(short torusVertex, short geodesicVertex)
		{
			//System.out.println("oneStitch("+torusVertex+","+geodesicVertex+")");
			if (stitchesCount + 1 >= stitches.Length)
				stitches = Util.doubleLength(stitches);
			stitches[stitchesCount] = torusVertex;
			stitches[stitchesCount + 1] = geodesicVertex;
			stitchesCount += 2;
		}
		
		internal virtual void  dumpStitches()
		{
			System.Console.Out.WriteLine("    >> stitches stitchesCount=" + stitchesCount);
			for (int i = 0; i < stitchesCount; i += 2)
			{
				System.Console.Out.WriteLine("    " + stitches[i] + "->(" + stitches[i + 1] + ")");
			}
		}
		
		////////////////////////////////////////////////////////////////
		// seam stuff
		////////////////////////////////////////////////////////////////
		
		internal virtual short[] createSeam()
		{
			return createSeam(stitchesCount, stitches);
		}
		
		internal int seamCount;
		internal short[] seam = new short[64];
		
		internal virtual short[] createSeam(int stitchCount, short[] stitches)
		{
			seamCount = 0;
			short lastTorusVertex = - 1;
			short lastGeodesicVertex = - 1;
			for (int i = 0; i < stitchCount; i += 2)
			{
				short torusVertex = stitches[i];
				short geodesicVertex = stitches[i + 1];
				if (torusVertex != lastTorusVertex)
				{
					if (geodesicVertex != lastGeodesicVertex)
					{
						if (seamCount > 0)
							addToSeam(System.Int16.MinValue);
						addToSeam(torusVertex);
						addToSeam((short) ~ geodesicVertex);
					}
					else
					{
						addToSeam(torusVertex);
					}
				}
				else
				{
					addToSeam((short) ~ geodesicVertex);
				}
				lastTorusVertex = torusVertex;
				lastGeodesicVertex = geodesicVertex;
			}
			short[] newSeam = new short[seamCount];
			for (int i = newSeam.Length; --i >= 0; )
				newSeam[i] = seam[i];
			//    dumpSeam(stitchCount, stitches, seam);
			//    decodeSeam(seam);
			return newSeam;
		}
		
		internal virtual void  addToSeam(short vertex)
		{
			if (seamCount == seam.Length)
				seam = Util.doubleLength(seam);
			seam[seamCount++] = vertex;
		}
		
		internal virtual void  dumpSeam(int stitchCount, short[] stitches, short[] seam)
		{
			System.Console.Out.WriteLine("dumpSeam:");
			for (int i = 0; i < stitchCount; i += 2)
				System.Console.Out.WriteLine("  " + stitches[i] + "->" + stitches[i + 1]);
			System.Console.Out.WriteLine(" --");
			for (int i = 0; i < seam.Length; ++i)
			{
				short v = seam[i];
				System.Console.Out.Write("  " + v + " ");
				if (v == System.Int16.MinValue)
					System.Console.Out.WriteLine(" -- break");
				else if (v < 0)
					System.Console.Out.WriteLine("(" + ~ v + ")");
				else
					System.Console.Out.WriteLine("");
			}
		}
		
		internal virtual void  decodeSeam(short[] seam)
		{
			System.Console.Out.WriteLine("-----\ndecodeSeam\n-----");
			bool breakSeam = true;
			int lastTorusVertex = - 1;
			int lastGeodesicVertex = - 1;
			for (int i = 0; i < seam.Length; ++i)
			{
				if (breakSeam)
				{
					lastTorusVertex = seam[i++];
					lastGeodesicVertex = ~ seam[i];
					System.Console.Out.WriteLine("--break--");
					breakSeam = false;
					continue;
				}
				int v = seam[i];
				if (v > 0)
				{
					System.Console.Out.WriteLine(" " + lastTorusVertex + " -> " + v + " -> " + "(" + lastGeodesicVertex + ")");
					lastTorusVertex = v;
				}
				else
				{
					v = ~ v;
					System.Console.Out.WriteLine(" " + lastTorusVertex + " -> " + "(" + v + ") -> " + "(" + lastGeodesicVertex + ")");
					lastGeodesicVertex = v;
				}
			}
		}
		static SasGem()
		{
			MAX_FULL_TORUS_STEP_COUNT = Sasurface.MAX_FULL_TORUS_STEP_COUNT;
		}
	}
}