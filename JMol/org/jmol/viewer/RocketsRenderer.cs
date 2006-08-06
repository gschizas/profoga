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
using org.jmol.g3d;
//UPGRADE_TODO: The type 'javax.vecmath.Point3f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Point3f = javax.vecmath.Point3f;
//UPGRADE_TODO: The type 'javax.vecmath.Point3i' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Point3i = javax.vecmath.Point3i;
//UPGRADE_TODO: The type 'javax.vecmath.Vector3f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Vector3f = javax.vecmath.Vector3f;
namespace org.jmol.viewer
{
	
	class RocketsRenderer:MpsRenderer
	{
		public RocketsRenderer()
		{
			InitBlock();
		}
		private void  InitBlock()
		{
			for (int i = 8; --i >= 0; )
			{
				screenCorners[i] = new Point3f();
				corners[i] = new Point3f();
			}
		}
		virtual internal Point3i[] Screens
		{
			get
			{
				int count = monomerCount + 1;
				Point3i[] screens = viewer.allocTempScreens(count);
				for (int i = count; --i >= 0; )
				{
					viewer.transformPoint(cordMidPoints[i], screens[i]);
					//      g3d.fillSphereCentered(Colix.CYAN, 15, screens[i]);
				}
				return screens;
			}
			
		}
		
		internal Point3i s0 = new Point3i();
		internal Point3i s1 = new Point3i();
		internal Point3i s2 = new Point3i();
		internal Point3i s3 = new Point3i();
		internal int diameterBeg, diameterMid, diameterEnd;
		
		internal Rockets cartoon;
		
		internal override void  renderMpspolymer(Mps.Mpspolymer mpspolymer)
		{
			Rockets.Cchain cchain = (Rockets.Cchain) mpspolymer;
			render1Chain(cchain.polymer, cchain.mads, cchain.colixes);
		}
		
		internal virtual void  render1Chain(Polymer polymer, short[] mads, short[] colixes)
		{
			if (!(polymer is AminoPolymer))
				return ;
			initializeChain((AminoPolymer) polymer);
			clearPending();
			for (int i = 0; i < monomerCount; ++i)
			{
				if (mads[i] == 0)
					continue;
				Monomer monomer = monomers[i];
				short colix = Graphics3D.inheritColix(colixes[i], monomer.LeadAtom.colixAtom);
				if (monomer.HelixOrSheet)
				{
					//        System.out.println("renderSpecialSegment[" + i + "]");
					renderSpecialSegment(monomer, colix, mads[i]);
				}
				else
				{
					//        System.out.println("renderRopeSegment[" + i + "]");
					renderRopeSegment(colix, mads, i, monomerCount, monomers, screens, isSpecials);
				}
			}
			renderPending();
			viewer.freeTempScreens(screens);
			viewer.freeTempPoints(cordMidPoints);
			viewer.freeTempBooleans(isSpecials);
		}
		
		internal int monomerCount;
		internal Monomer[] monomers;
		internal Point3i[] screens;
		internal bool[] isSpecials;
		internal Point3f[] cordMidPoints;
		
		internal virtual void  initializeChain(AminoPolymer aminopolymer)
		{
			monomers = aminopolymer.monomers;
			monomerCount = aminopolymer.monomerCount;
			isSpecials = calcIsSpecials(monomerCount, monomers);
			cordMidPoints = calcRopeMidPoints(aminopolymer);
			screens = Screens;
		}
		
		internal virtual Point3f[] calcRopeMidPoints(AminoPolymer aminopolymer)
		{
			int midPointCount = monomerCount + 1;
			Point3f[] cordMidPoints = viewer.allocTempPoints(midPointCount);
			//Monomer residuePrev = null;
			ProteinStructure proteinstructurePrev = null;
			Point3f point;
			for (int i = 0; i < monomerCount; ++i)
			{
				point = cordMidPoints[i];
				Monomer residue = monomers[i];
				if (isSpecials[i])
				{
					ProteinStructure proteinstructure = residue.ProteinStructure;
					point.set_Renamed(i - 1 != proteinstructure.MonomerIndex?proteinstructure.AxisStartPoint:proteinstructure.AxisEndPoint);
					
					//        if (i != structure.getStartResidueIndex()) {
					//          point.add(structure.getAxisEndPoint());
					//          point.scale(0.5f);
					//        }
					//residuePrev = residue;
					proteinstructurePrev = proteinstructure;
				}
				else
				{
					if (proteinstructurePrev != null)
						point.set_Renamed(proteinstructurePrev.AxisEndPoint);
					else
						aminopolymer.getLeadMidPoint(i, point);
					//residuePrev = null;
					proteinstructurePrev = null;
				}
			}
			point = cordMidPoints[monomerCount];
			if (proteinstructurePrev != null)
				point.set_Renamed(proteinstructurePrev.AxisEndPoint);
			else
				aminopolymer.getLeadMidPoint(monomerCount, point);
			return cordMidPoints;
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'screenA '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Point3i screenA = new Point3i();
		internal Point3i screenB = new Point3i();
		internal Point3i screenC = new Point3i();
		
		internal virtual void  renderSpecialSegment(Monomer monomer, short colix, short mad)
		{
			ProteinStructure proteinstructure = monomer.ProteinStructure;
			if (tPending)
			{
				if (proteinstructure == proteinstructurePending && mad == madPending && colix == colixPending && proteinstructure.getIndex(monomer) == endIndexPending + 1)
				{
					++endIndexPending;
					return ;
				}
				renderPending();
			}
			proteinstructurePending = proteinstructure;
			startIndexPending = endIndexPending = proteinstructure.getIndex(monomer);
			colixPending = colix;
			madPending = mad;
			tPending = true;
		}
		
		internal bool tPending;
		internal ProteinStructure proteinstructurePending;
		internal int startIndexPending;
		internal int endIndexPending;
		internal short madPending;
		internal short colixPending;
		internal int[] shadesPending;
		
		internal virtual void  clearPending()
		{
			tPending = false;
		}
		
		internal virtual void  renderPending()
		{
			if (tPending)
			{
				Point3f[] segments = proteinstructurePending.Segments;
				bool tEnd = (endIndexPending == proteinstructurePending.MonomerCount - 1);
				
				/*
				System.out.println("structurePending.getPolymerCount()=" +
				structurePending.getPolymerCount());
				System.out.println("segments.length=" + segments.length);
				System.out.println(" startIndexPending=" + startIndexPending +
				" endIndexPending=" + endIndexPending);
				System.out.println("tEnd=" + tEnd);
				*/
				if (proteinstructurePending is Helix)
					renderPendingHelix(segments[startIndexPending], segments[endIndexPending], segments[endIndexPending + 1], tEnd);
				else if (proteinstructurePending is Sheet)
					renderPendingSheet(segments[startIndexPending], segments[endIndexPending], segments[endIndexPending + 1], tEnd);
				else
					System.Console.Out.WriteLine("?Que? neither helix nor sheet");
				tPending = false;
			}
		}
		
		internal virtual void  renderPendingHelix(Point3f pointStart, Point3f pointBeforeEnd, Point3f pointEnd, bool tEnd)
		{
			viewer.transformPoint(pointStart, screenA);
			viewer.transformPoint(pointEnd, screenB);
			if (tEnd)
			{
				viewer.transformPoint(pointBeforeEnd, screenC);
				int capDiameter = viewer.scaleToScreen(screenC.z, madPending + (madPending >> 2));
				g3d.fillCone(colixPending, Graphics3D.ENDCAPS_FLAT, capDiameter, screenC, screenB);
				if (startIndexPending == endIndexPending)
					return ;
				Point3i t = screenB;
				screenB = screenC;
				screenC = t;
			}
			int zMid = (screenA.z + screenB.z) / 2;
			int diameter = viewer.scaleToScreen(zMid, madPending);
			g3d.fillCylinder(colixPending, Graphics3D.ENDCAPS_FLAT, diameter, screenA, screenB);
		}
		
		internal virtual void  renderPendingSheet(Point3f pointStart, Point3f pointBeforeEnd, Point3f pointEnd, bool tEnd)
		{
			shadesPending = g3d.getShades(colixPending);
			if (tEnd)
			{
				drawArrowHeadBox(pointBeforeEnd, pointEnd);
				drawBox(pointStart, pointBeforeEnd);
			}
			else
			{
				drawBox(pointStart, pointEnd);
			}
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'pointTipOffset '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Point3f pointTipOffset = new Point3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'pointArrow2 '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Point3f pointArrow2 = new Point3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'vectorNormal '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f vectorNormal = new Vector3f();
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'scaledWidthVector '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f scaledWidthVector = new Vector3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'scaledHeightVector '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f scaledHeightVector = new Vector3f();
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'arrowHeadFaces'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly sbyte[] arrowHeadFaces = new sbyte[]{0, 1, 3, 2, 0, 4, 5, 2, 1, 4, 5, 3};
		
		internal virtual void  drawArrowHeadBox(Point3f base_Renamed, Point3f tip)
		{
			Sheet sheet = (Sheet) proteinstructurePending;
			float scale = madPending / 1000f;
			scaledWidthVector.set_Renamed(sheet.WidthUnitVector);
			scaledWidthVector.scale(scale * 1.25f);
			scaledHeightVector.set_Renamed(sheet.HeightUnitVector);
			scaledHeightVector.scale(scale / 3);
			pointCorner.add(scaledWidthVector, scaledHeightVector);
			pointCorner.scaleAdd(- 0.5f, base_Renamed);
			pointTipOffset.set_Renamed(scaledHeightVector);
			pointTipOffset.scaleAdd(- 0.5f, tip);
			buildArrowHeadBox(pointCorner, scaledWidthVector, scaledHeightVector, pointTipOffset);
			g3d.fillTriangle(colixPending, screenCorners[0], screenCorners[1], screenCorners[4]);
			g3d.fillTriangle(colixPending, screenCorners[2], screenCorners[3], screenCorners[5]);
			for (int i = 0; i < 12; i += 4)
			{
				int i0 = arrowHeadFaces[i];
				int i1 = arrowHeadFaces[i + 1];
				int i2 = arrowHeadFaces[i + 2];
				int i3 = arrowHeadFaces[i + 3];
				g3d.fillQuadrilateral(colixPending, screenCorners[i0], screenCorners[i1], screenCorners[i2], screenCorners[i3]);
			}
			/*
			Sheet sheet = (Sheet)structurePending;
			Vector3f widthUnitVector = sheet.getWidthUnitVector();
			Vector3f heightUnitVector = sheet.getHeightUnitVector();
			float widthScale = (madPending + madPending >> 2) / 2 / 1000f;
			
			pointArrow1.set(widthUnitVector);
			pointArrow1.scaleAdd(-widthScale, base);
			viewer.transformPoint(pointArrow1, screenA);
			
			pointArrow2.set(widthUnitVector);
			pointArrow2.scaleAdd(widthScale, base);
			viewer.transformPoint(pointArrow2, screenB);
			
			viewer.transformPoint(tip, screenC);
			
			viewer.transformVector(heightUnitVector, vectorNormal);
			
			g3d.drawfillTriangle(colixPending, vectorNormal,
			screenA, screenB, screenC);
			*/
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'lengthVector '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f lengthVector = new Vector3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'pointCorner '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Point3f pointCorner = new Point3f();
		
		internal virtual void  drawBox(Point3f pointA, Point3f pointB)
		{
			Sheet sheet = (Sheet) proteinstructurePending;
			float scale = madPending / 1000f;
			scaledWidthVector.set_Renamed(sheet.WidthUnitVector);
			scaledWidthVector.scale(scale);
			scaledHeightVector.set_Renamed(sheet.HeightUnitVector);
			scaledHeightVector.scale(scale / 4);
			pointCorner.add(scaledWidthVector, scaledHeightVector);
			pointCorner.scaleAdd(- 0.5f, pointA);
			lengthVector.sub(pointB, pointA);
			buildBox(pointCorner, scaledWidthVector, scaledHeightVector, lengthVector);
			for (int i = 0; i < 6; ++i)
			{
				int i0 = boxFaces[i * 4];
				int i1 = boxFaces[i * 4 + 1];
				int i2 = boxFaces[i * 4 + 2];
				int i3 = boxFaces[i * 4 + 3];
				g3d.fillQuadrilateral(colixPending, screenCorners[i0], screenCorners[i1], screenCorners[i2], screenCorners[i3]);
			}
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'boxFaces'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly sbyte[] boxFaces = new sbyte[]{0, 1, 3, 2, 0, 2, 6, 4, 0, 4, 5, 1, 7, 5, 4, 6, 7, 6, 2, 3, 7, 3, 1, 5};
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'corners '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Point3f[] corners = new Point3f[8];
		//UPGRADE_NOTE: Final was removed from the declaration of 'screenCorners '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Point3f[] screenCorners = new Point3f[8];
		
		internal virtual void  buildBox(Point3f pointCorner, Vector3f scaledWidthVector, Vector3f scaledHeightVector, Vector3f lengthVector)
		{
			for (int i = 8; --i >= 0; )
			{
				Point3f corner = corners[i];
				corner.set_Renamed(pointCorner);
				if ((i & 1) != 0)
					corner.add(scaledWidthVector);
				if ((i & 2) != 0)
					corner.add(scaledHeightVector);
				if ((i & 4) != 0)
					corner.add(lengthVector);
				viewer.transformPoint(corner, screenCorners[i]);
			}
		}
		
		internal virtual void  buildArrowHeadBox(Point3f pointCorner, Vector3f scaledWidthVector, Vector3f scaledHeightVector, Point3f pointTip)
		{
			for (int i = 4; --i >= 0; )
			{
				Point3f corner = corners[i];
				corner.set_Renamed(pointCorner);
				if ((i & 1) != 0)
					corner.add(scaledWidthVector);
				if ((i & 2) != 0)
					corner.add(scaledHeightVector);
				viewer.transformPoint(corner, screenCorners[i]);
			}
			corners[4].set_Renamed(pointTip);
			viewer.transformPoint(pointTip, screenCorners[4]);
			corners[5].add(pointTip, scaledHeightVector);
			viewer.transformPoint(corners[5], screenCorners[5]);
		}
	}
}