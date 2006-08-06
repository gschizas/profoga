/* $RCSfile$
* $Author: nicove $
* $Date: 2006-03-28 20:43:52 +0200 (mar., 28 mars 2006) $
* $Revision: 4836 $
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
//UPGRADE_TODO: The package 'javax.vecmath' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using javax.vecmath;
using Graphics3D = org.jmol.g3d.Graphics3D;
namespace org.jmol.viewer
{
	
	/// <summary>*************************************************************
	/// The Dots and DotsRenderer classes implement vanderWaals and Connolly
	/// dot surfaces. <p>
	/// The vanderWaals surface is defined by the vanderWaals radius of each
	/// atom. The surface of the atom is 'peppered' with dots. Each dot is
	/// tested to see if it falls within the vanderWaals radius of any of
	/// its neighbors. If so, then the dot is not displayed. <p>
	/// See DotsRenderer.Geodesic for more discussion of the implementation. <p>
	/// The Connolly surface is defined by rolling a probe sphere over the
	/// surface of the molecule. In this way, a smooth surface is generated ...
	/// one that does not have crevices between atoms. Three types of shapes
	/// are generated: convex, saddle, and concave. <p>
	/// The 'probe' is a sphere. A sphere of 1.2 angstroms representing HOH
	/// is commonly used. <p>
	/// Convex shapes are generated on the exterior surfaces of exposed atoms.
	/// They are points on the sphere which are exposed. In these areas of
	/// the molecule they look just like the vanderWaals dot surface. <p>
	/// The saddles are generated between pairs of atoms. Imagine an O2
	/// molecule. The probe sphere is rolled around the two oxygen spheres so
	/// that it stays in contact with both spheres. The probe carves out a
	/// torus (donut). The portion of the torus between the two points of
	/// contact with the oxygen spheres is a saddle. <p>
	/// The concave shapes are defined by triples of atoms. Imagine three
	/// atom spheres in a close triangle. The probe sphere will sit (nicely)
	/// in the little cavity formed by the three spheres. In fact, there are
	/// two cavities, one on each side of the triangle. The probe sphere makes
	/// one point of contact with each of the three atoms. The shape of the
	/// cavity is the spherical triangle on the surface of the probe sphere
	/// determined by these three contact points. <p>
	/// For each of these three surface shapes, the dots are painted only
	/// when the probe sphere does not interfere with any of the neighboring
	/// atoms. <p>
	/// See the following scripting commands:<br>
	/// set solvent on/off (on defaults to 1.2 angstroms) <br>
	/// set solvent 1.5 (choose another probe size) <br>
	/// dots on/off <br>
	/// color dots [color] <br>
	/// color dotsConvex [color] <br>
	/// color dotsSaddle [color] <br>
	/// color dotsConcave [color] <br>
	/// 
	/// The reference article for this implementation is: <br>
	/// Analytical Molecular Surface Calculation, Michael L. Connolly,
	/// Journal of Applied Crystalography, (1983) 15, 548-558 <p>
	/// 
	/// **************************************************************
	/// </summary>
	
	class Dots:Shape
	{
		virtual internal int AtomI
		{
			set
			{
				this.indexI = value;
				atomI = frame.atoms[value];
				centerI = atomI.point3f;
				radiusI = getAppropriateRadius(atomI);
				radiiIP2 = radiusI + radiusP;
				radiiIP2 *= radiiIP2;
			}
			
		}
		virtual internal int NeighborJ
		{
			set
			{
				indexJ = neighborIndices[value];
				atomJ = neighbors[value];
				radiusJ = getAppropriateRadius(atomJ);
				radiiJP2 = neighborPlusProbeRadii2[value];
				centerJ = neighborCenters[value];
				distanceIJ2 = centerI.distanceSquared(centerJ);
			}
			
		}
		virtual internal int NeighborK
		{
			set
			{
				indexK = neighborIndices[value];
				centerK = neighborCenters[value];
				atomK = neighbors[value];
				radiusK = getAppropriateRadius(atomK);
				radiiKP2 = neighborPlusProbeRadii2[value];
			}
			
		}
		
		internal DotsRenderer dotsRenderer;
		
		internal short mad; // this is really just a true/false flag ... 0 vs non-zero
		
		internal int dotsConvexMax; // the Max == the highest atomIndex with dots + 1
		internal int[][] dotsConvexMaps;
		internal short[] colixesConvex;
		internal Vector3f[] geodesicVertices;
		internal int geodesicCount;
		internal int[] geodesicMap;
		//UPGRADE_NOTE: Final was removed from the declaration of 'mapNull '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int[] mapNull = new int[0];
		
		internal int cavityCount;
		internal Cavity[] cavities;
		internal int torusCount;
		internal Torus[] tori;
		
		internal System.Collections.Hashtable htTori;
		
		internal bool useVanderwaalsRadius;
		
		internal int indexI, indexJ, indexK;
		internal Atom atomI, atomJ, atomK;
		internal Point3f centerI, centerJ, centerK;
		internal float radiusI, radiusJ, radiusK;
		internal float radiusP, diameterP;
		internal float radiiIP2, radiiJP2, radiiKP2;
		internal float distanceIJ2;
		internal Torus torusIJ, torusIK;
		//UPGRADE_NOTE: Final was removed from the declaration of 'baseIJK '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Point3f baseIJK = new Point3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'probeIJK '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Point3f probeIJK = new Point3f();
		internal float heightIJK;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'pointT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Point3f pointT = new Point3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'pointT1 '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Point3f pointT1 = new Point3f();
		
		internal override void  initShape()
		{
			dotsRenderer = (DotsRenderer) frame.getRenderer(JmolConstants.SHAPE_DOTS);
			geodesicVertices = dotsRenderer.geodesic.vertices;
			geodesicCount = geodesicVertices.length;
			geodesicMap = allocateBitmap(geodesicCount);
		}
		
		internal override void  setSize(int size, System.Collections.BitArray bsSelected)
		{
			// miguel 9 Sep 2005
			// this is a hack ...
			// if mad == 0 then turn it off
			// if mad > 0 then use vdw radius
			// if mad < 0 then use ionic radius
			short mad = (short) size;
			this.mad = mad;
			bool newUseVanderwaalsRadius = (mad > 0);
			
			if (radiusP != viewer.CurrentSolventProbeRadius || (newUseVanderwaalsRadius ^ useVanderwaalsRadius))
			{
				dotsConvexMax = 0;
				dotsConvexMaps = null;
				torusCount = 0;
				htTori = null;
				tori = null;
				cavityCount = 0;
				cavities = null;
				radiusP = viewer.CurrentSolventProbeRadius;
				diameterP = 2 * radiusP;
			}
			int atomCount = frame.atomCount;
			// always delete old surfaces for selected atoms
			if (dotsConvexMaps != null)
			{
				for (int i = atomCount; --i >= 0; )
					if (bsSelected.Get(i))
						dotsConvexMaps[i] = null;
				deleteUnnecessaryTori();
				deleteUnnecessaryCavities();
			}
			// now, calculate surface for selected atoms
			if (mad != 0)
			{
				useVanderwaalsRadius = (mad > 0);
				if (dotsConvexMaps == null)
				{
					dotsConvexMaps = new int[atomCount][];
					colixesConvex = new short[atomCount];
				}
				for (int i = atomCount; --i >= 0; )
					if (bsSelected.Get(i))
					{
						AtomI = i;
						getNeighbors(bsSelected);
						calcConvexMap();
						calcTori();
						calcCavities();
					}
			}
			if (dotsConvexMaps == null)
				dotsConvexMax = 0;
			else
			{
				// update this count to speed up dotsRenderer
				int i;
				for (i = atomCount; --i >= 0 && dotsConvexMaps[i] == null; )
				{
				}
				dotsConvexMax = i + 1;
			}
		}
		
		internal override void  setProperty(System.String propertyName, System.Object value_Renamed, System.Collections.BitArray bs)
		{
			int atomCount = frame.atomCount;
			Atom[] atoms = frame.atoms;
			if ((System.Object) "color" == (System.Object) propertyName)
			{
				System.Console.Out.WriteLine("Dots.setProperty('color')");
				setProperty("colorConvex", value_Renamed, bs);
				setProperty("colorConcave", value_Renamed, bs);
				setProperty("colorSaddle", value_Renamed, bs);
			}
			// no translucency for dots
			if ((System.Object) "colorConvex" == (System.Object) propertyName)
			{
				if (colixesConvex == null)
					return ;
				System.Console.Out.WriteLine("Dots.setProperty('colorConvex')");
				short colix = Graphics3D.getColix(value_Renamed);
				for (int i = atomCount; --i >= 0; )
					if (bs.Get(i))
						colixesConvex[i] = ((colix != Graphics3D.UNRECOGNIZED)?colix:viewer.getColixAtomPalette(atoms[i], (System.String) value_Renamed));
				return ;
			}
			if ((System.Object) "colorSaddle" == (System.Object) propertyName)
			{
				short colix = Graphics3D.getColix(value_Renamed);
				for (int i = torusCount; --i >= 0; )
				{
					Torus torus = tori[i];
					if (bs.Get(torus.ixI))
						torus.colixI = ((colix != Graphics3D.UNRECOGNIZED)?colix:viewer.getColixAtomPalette(atoms[torus.ixI], (System.String) value_Renamed));
					if (bs.Get(torus.ixJ))
						torus.colixJ = ((colix != Graphics3D.UNRECOGNIZED)?colix:viewer.getColixAtomPalette(atoms[torus.ixJ], (System.String) value_Renamed));
				}
				return ;
			}
			if ((System.Object) "colorConcave" == (System.Object) propertyName)
			{
				short colix = Graphics3D.getColix(value_Renamed);
				for (int i = cavityCount; --i >= 0; )
				{
					Cavity cavity = cavities[i];
					if (bs.Get(cavity.ixI))
						cavity.colixI = ((colix != Graphics3D.UNRECOGNIZED)?colix:viewer.getColixAtomPalette(atoms[cavity.ixI], (System.String) value_Renamed));
					if (bs.Get(cavity.ixJ))
						cavity.colixJ = ((colix != Graphics3D.UNRECOGNIZED)?colix:viewer.getColixAtomPalette(atoms[cavity.ixJ], (System.String) value_Renamed));
					if (bs.Get(cavity.ixK))
						cavity.colixK = ((colix != Graphics3D.UNRECOGNIZED)?colix:viewer.getColixAtomPalette(atoms[cavity.ixK], (System.String) value_Renamed));
				}
				return ;
			}
		}
		
		internal virtual float getAppropriateRadius(Atom atom)
		{
			return (useVanderwaalsRadius?atom.VanderwaalsRadiusFloat:atom.BondingRadiusFloat);
		}
		
		internal virtual void  calcConvexMap()
		{
			calcConvexBits();
			int indexLast;
			for (indexLast = geodesicMap.Length; --indexLast >= 0 && geodesicMap[indexLast] == 0; )
			{
			}
			int[] map = mapNull;
			if (indexLast >= 0)
			{
				int count = indexLast + 1;
				map = new int[count];
				Array.Copy(geodesicMap, 0, map, 0, count);
			}
			dotsConvexMaps[indexI] = map;
		}
		
		internal virtual void  calcConvexBits()
		{
			setAllBits(geodesicMap, geodesicCount);
			if (neighborCount == 0)
				return ;
			float combinedRadii = radiusI + radiusP;
			int iLastUsed = 0;
			for (int iDot = geodesicCount; --iDot >= 0; )
			{
				pointT.set_Renamed(geodesicVertices[iDot]);
				pointT.scaleAdd(combinedRadii, centerI);
				int iStart = iLastUsed;
				do 
				{
					if (pointT.distanceSquared(neighborCenters[iLastUsed]) < neighborPlusProbeRadii2[iLastUsed])
					{
						clearBit(geodesicMap, iDot);
						break;
					}
					iLastUsed = (iLastUsed + 1) % neighborCount;
				}
				while (iLastUsed != iStart);
			}
		}
		
		// I have no idea what this number should be
		internal int neighborCount;
		internal Atom[] neighbors = new Atom[16];
		internal int[] neighborIndices = new int[16];
		internal Point3f[] neighborCenters = new Point3f[16];
		internal float[] neighborPlusProbeRadii2 = new float[16];
		
		internal virtual void  getNeighbors(System.Collections.BitArray bsSelected)
		{
			/*
			System.out.println("Dots.getNeighbors radiusI=" + radiusI +
			" diameterP=" + diameterP +
			" maxVdw=" + frame.getMaxVanderwaalsRadius());
			*/
			AtomIterator iter = frame.getWithinModelIterator(atomI, radiusI + diameterP + frame.MaxVanderwaalsRadius);
			neighborCount = 0;
			while (iter.hasNext())
			{
				Atom neighbor = iter.next();
				if (neighbor == atomI)
					continue;
				// only consider selected neighbors
				if (!bsSelected.Get(neighbor.atomIndex))
					continue;
				float neighborRadius = getAppropriateRadius(neighbor);
				if (centerI.distance(neighbor.point3f) > radiusI + radiusP + radiusP + neighborRadius)
					continue;
				if (neighborCount == neighbors.Length)
				{
					neighbors = (Atom[]) Util.doubleLength(neighbors);
					neighborIndices = Util.doubleLength(neighborIndices);
					neighborCenters = (Point3f[]) Util.doubleLength(neighborCenters);
					neighborPlusProbeRadii2 = Util.doubleLength(neighborPlusProbeRadii2);
				}
				neighbors[neighborCount] = neighbor;
				neighborCenters[neighborCount] = neighbor.point3f;
				neighborIndices[neighborCount] = neighbor.atomIndex;
				float neighborPlusProbeRadii = neighborRadius + radiusP;
				neighborPlusProbeRadii2[neighborCount] = neighborPlusProbeRadii * neighborPlusProbeRadii;
				++neighborCount;
			}
			/*
			System.out.println("neighborsFound=" + neighborCount);
			System.out.println("myVdwRadius=" + myVdwRadius +
			" maxVdwRadius=" + maxVdwRadius +
			" distMax=" + (myVdwRadius + maxVdwRadius));
			Point3f me = atom.getPoint3f();
			for (int i = 0; i < neighborCount; ++i) {
			System.out.println(" dist=" +
			me.distance(neighbors[i].getPoint3f()));
			}
			*/
		}
		
		internal virtual void  calcTori()
		{
			if (radiusP == 0)
				return ;
			if (htTori == null)
			{
				torusCount = 0;
				tori = new Torus[32];
				htTori = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
			}
			for (int iJ = neighborCount; --iJ >= 0; )
			{
				if (indexI >= neighborIndices[iJ])
					continue;
				NeighborJ = iJ;
				torusIJ = getTorus(atomI, atomJ);
				if (torusIJ == null)
					continue;
				torusIJ.calcProbeMap();
				if (torusIJ.probeMap == 0)
					continue;
				if (torusCount == tori.Length)
					tori = (Torus[]) Util.doubleLength(tori);
				tori[torusCount++] = torusIJ;
			}
		}
		
		internal virtual void  deleteUnnecessaryTori()
		{
			bool torusDeleted = false;
			for (int i = torusCount; --i >= 0; )
			{
				Torus torus = tori[i];
				if (dotsConvexMaps[torus.ixI] == null && dotsConvexMaps[torus.ixJ] == null)
				{
					torusDeleted = true;
					tori[i] = null;
				}
			}
			if (torusDeleted)
			{
				int iDestination = 0;
				for (int iSource = 0; iSource < torusCount; ++iSource)
				{
					if (tori[iSource] != null)
						tori[iDestination++] = tori[iSource];
				}
				for (int i = torusCount; --i >= iDestination; )
					tori[i] = null;
				torusCount = iDestination;
			}
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'matrixT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Matrix3f matrixT = new Matrix3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'matrixT1 '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Matrix3f matrixT1 = new Matrix3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'aaT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal AxisAngle4f aaT = new AxisAngle4f();
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'vectorT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f vectorT = new Vector3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'vectorT1 '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f vectorT1 = new Vector3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'vectorZ '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f vectorZ = new Vector3f(0, 0, 1);
		//UPGRADE_NOTE: Final was removed from the declaration of 'vectorX '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f vectorX = new Vector3f(1, 0, 0);
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'pointTorusP '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Point3f pointTorusP = new Point3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'vectorPI '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f vectorPI = new Vector3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'vectorPJ '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f vectorPJ = new Vector3f();
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'Torus' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		//UPGRADE_NOTE: The access modifier for this class or class field has been changed in order to prevent compilation errors due to the visibility level. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1296'"
		internal class Torus
		{
			private void  InitBlock(Dots enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Dots enclosingInstance;
			public Dots Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			internal int ixI, ixJ;
			internal Point3f center;
			internal float radius;
			internal Vector3f axisVector;
			internal Vector3f radialVector;
			internal Vector3f unitRadialVector;
			internal Vector3f tangentVector;
			internal Vector3f outerRadial;
			internal float outerAngle;
			internal long probeMap;
			internal AxisAngle4f aaRotate;
			internal short colixI, colixJ;
			
			internal Torus(Dots enclosingInstance, Point3f centerA, int indexA, Point3f centerB, int indexB, Point3f center, float radius)
			{
				InitBlock(enclosingInstance);
				this.ixI = indexA;
				this.ixJ = indexB;
				this.center = center;
				this.radius = radius;
				
				axisVector = new Vector3f();
				axisVector.sub(centerB, centerA);
				
				if (axisVector.x == 0)
					unitRadialVector = new Vector3f(1, 0, 0);
				else if (axisVector.y == 0)
					unitRadialVector = new Vector3f(0, 1, 0);
				else if (axisVector.z == 0)
					unitRadialVector = new Vector3f(0, 0, 1);
				else
				{
					unitRadialVector = new Vector3f(- axisVector.y, axisVector.x, 0);
					unitRadialVector.normalize();
				}
				radialVector = new Vector3f(unitRadialVector);
				radialVector.scale(radius);
				
				tangentVector = new Vector3f();
				tangentVector.cross(radialVector, axisVector);
				tangentVector.normalize();
				
				Enclosing_Instance.pointTorusP.add(center, radialVector);
				
				Enclosing_Instance.vectorPI.sub(centerA, Enclosing_Instance.pointTorusP);
				Enclosing_Instance.vectorPI.normalize();
				Enclosing_Instance.vectorPI.scale(Enclosing_Instance.radiusP);
				
				Enclosing_Instance.vectorPJ.sub(centerB, Enclosing_Instance.pointTorusP);
				Enclosing_Instance.vectorPJ.normalize();
				Enclosing_Instance.vectorPJ.scale(Enclosing_Instance.radiusP);
				
				outerRadial = new Vector3f();
				outerRadial.add(Enclosing_Instance.vectorPI, Enclosing_Instance.vectorPJ);
				outerRadial.normalize();
				outerRadial.scale(Enclosing_Instance.radiusP);
				
				outerAngle = Enclosing_Instance.vectorPJ.angle(Enclosing_Instance.vectorPI) / 2;
				
				float angle = Enclosing_Instance.vectorZ.angle(axisVector);
				if (angle == 0)
				{
					Enclosing_Instance.matrixT.setIdentity();
				}
				else
				{
					Enclosing_Instance.vectorT.cross(Enclosing_Instance.vectorZ, axisVector);
					Enclosing_Instance.aaT.set_Renamed(Enclosing_Instance.vectorT, angle);
					Enclosing_Instance.matrixT.set_Renamed(Enclosing_Instance.aaT);
				}
				
				Enclosing_Instance.matrixT.transform(unitRadialVector, Enclosing_Instance.vectorT);
				angle = Enclosing_Instance.vectorX.angle(Enclosing_Instance.vectorT);
				if (angle != 0)
				{
					Enclosing_Instance.vectorT.cross(Enclosing_Instance.vectorX, Enclosing_Instance.vectorT);
					Enclosing_Instance.aaT.set_Renamed(Enclosing_Instance.vectorT, angle);
					Enclosing_Instance.matrixT1.set_Renamed(Enclosing_Instance.aaT);
					Enclosing_Instance.matrixT.mul(Enclosing_Instance.matrixT1);
				}
				
				aaRotate = new AxisAngle4f();
				aaRotate.set_Renamed(Enclosing_Instance.matrixT);
			}
			
			internal virtual void  calcProbeMap()
			{
				long probeMap = ~ 0;
				
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				float stepAngle = 2 * (float) System.Math.PI / 64;
				Enclosing_Instance.aaT.set_Renamed(axisVector, 0);
				int iLastNeighbor = 0;
				for (int a = 64; --a >= 0; )
				{
					Enclosing_Instance.aaT.angle = a * stepAngle;
					Enclosing_Instance.matrixT.set_Renamed(Enclosing_Instance.aaT);
					Enclosing_Instance.matrixT.transform(radialVector, Enclosing_Instance.pointT);
					Enclosing_Instance.pointT.add(center);
					int iStart = iLastNeighbor;
					do 
					{
						if (Enclosing_Instance.neighbors[iLastNeighbor].atomIndex != ixJ)
						{
							if (Enclosing_Instance.pointT.distanceSquared(Enclosing_Instance.neighborCenters[iLastNeighbor]) < Enclosing_Instance.neighborPlusProbeRadii2[iLastNeighbor])
							{
								probeMap &= ~ (1L << (63 - a));
								break;
							}
						}
						iLastNeighbor = (iLastNeighbor + 1) % Enclosing_Instance.neighborCount;
					}
					while (iLastNeighbor != iStart);
				}
				this.probeMap = probeMap;
			}
		}
		
		internal virtual Torus getTorus(Atom atomI, Atom atomJ)
		{
			int indexI = atomI.atomIndex;
			int indexJ = atomJ.atomIndex;
			// indexI < indexJ is tested previously in calcTorus
			if (indexI >= indexJ)
				throw new System.NullReferenceException();
			System.Int64 key = (long) (((long) indexI << 32) + indexJ);
			System.Object value_Renamed = htTori[key];
			if (value_Renamed != null)
			{
				if (value_Renamed is Torus)
				{
					Torus torus = (Torus) value_Renamed;
					return torus;
				}
				return null;
			}
			float radius = calcTorusRadius();
			if (radius == 0)
			{
				htTori[key] = false;
				return null;
			}
			Point3f center = calcTorusCenter();
			Torus torus2 = new Torus(centerI, indexI, centerJ, indexJ, center, radius);
			htTori[key] = torus2;
			return torus2;
		}
		
		internal virtual Point3f calcTorusCenter()
		{
			Point3f torusCenter = new Point3f();
			torusCenter.sub(centerJ, centerI);
			torusCenter.scale((radiiIP2 - radiiJP2) / distanceIJ2);
			torusCenter.add(centerI);
			torusCenter.add(centerJ);
			torusCenter.scale(0.5f);
			/*
			System.out.println("calcTorusCenter i=" + atomI.point3f.x + "," +
			atomI.point3f.y + "," + atomI.point3f.z + "  j=" +
			atomJ.point3f.x + "," + atomJ.point3f.y + "," +
			atomJ.point3f.z + "  center=" +
			torusCenter.x + "," + torusCenter.y + "," +
			torusCenter.z);
			*/
			return torusCenter;
		}
		
		internal virtual float calcTorusRadius()
		{
			float t1 = radiusI + radiusJ + diameterP;
			float t2 = t1 * t1 - distanceIJ2;
			float diff = radiusI - radiusJ;
			float t3 = distanceIJ2 - diff * diff;
			if (t2 <= 0 || t3 <= 0 || distanceIJ2 == 0)
				return 0;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			return (float) (0.5 * System.Math.Sqrt(t2) * System.Math.Sqrt(t3) / System.Math.Sqrt(distanceIJ2));
		}
		
		
		internal virtual void  calcCavities()
		{
			if (radiusP == 0)
				return ;
			if (cavities == null)
			{
				cavities = new Cavity[16];
				cavityCount = 0;
			}
			for (int iJ = neighborCount; --iJ >= 0; )
			{
				if (indexI >= neighborIndices[iJ])
					continue;
				NeighborJ = iJ;
				for (int iK = neighborCount; --iK >= 0; )
				{
					if (indexJ >= neighborIndices[iK])
						continue;
					NeighborK = iK;
					float distanceJK2 = centerJ.distanceSquared(centerK);
					if (distanceJK2 >= radiiJP2 + radiiKP2)
						continue;
					getCavitiesIJK();
				}
			}
		}
		
		internal virtual void  deleteUnnecessaryCavities()
		{
			bool cavityDeleted = false;
			for (int i = cavityCount; --i >= 0; )
			{
				Cavity cavity = cavities[i];
				if (dotsConvexMaps[cavity.ixI] == null && dotsConvexMaps[cavity.ixJ] == null && dotsConvexMaps[cavity.ixK] == null)
				{
					cavityDeleted = true;
					cavities[i] = null;
				}
			}
			if (cavityDeleted)
			{
				int iDestination = 0;
				for (int iSource = 0; iSource < cavityCount; ++iSource)
				{
					if (cavities[iSource] != null)
						cavities[iDestination++] = cavities[iSource];
				}
				for (int i = cavityCount; --i >= iDestination; )
					cavities[i] = null;
				cavityCount = iDestination;
			}
		}
		
		internal virtual void  getCavitiesIJK()
		{
			torusIJ = getTorus(atomI, atomJ);
			torusIK = getTorus(atomI, atomK);
			if (torusIJ == null || torusIK == null)
			{
				System.Console.Out.WriteLine("null torus found?");
				return ;
			}
			uIJK.cross(torusIJ.axisVector, torusIK.axisVector);
			uIJK.normalize();
			if (!calcBaseIJK() || !calcHeightIJK())
				return ;
			probeIJK.scaleAdd(heightIJK, uIJK, baseIJK);
			if (checkProbeIJK())
				addCavity(new Cavity(this));
			probeIJK.scaleAdd(- heightIJK, uIJK, baseIJK);
			if (checkProbeIJK())
				addCavity(new Cavity(this));
		}
		
		internal virtual bool checkProbeIJK()
		{
			for (int i = neighborCount; --i >= 0; )
			{
				int neighborIndex = neighborIndices[i];
				if (neighborIndex == indexI || neighborIndex == indexJ || neighborIndex == indexK)
					continue;
				if (probeIJK.distanceSquared(neighborCenters[i]) < neighborPlusProbeRadii2[i])
					return false;
			}
			return true;
		}
		
		internal virtual void  addCavity(Cavity cavity)
		{
			if (cavityCount == cavities.Length)
				cavities = (Cavity[]) Util.doubleLength(cavities);
			cavities[cavityCount++] = cavity;
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'uIJK '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f uIJK = new Vector3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'v2v3 '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f v2v3 = new Vector3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'v3v1 '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f v3v1 = new Vector3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'v1v2 '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f v1v2 = new Vector3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'p1 '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f p1 = new Vector3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'p2 '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f p2 = new Vector3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'p3 '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f p3 = new Vector3f();
		
		// plus use vectorPI and vectorPJ from above;
		//UPGRADE_NOTE: Final was removed from the declaration of 'vectorPK '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f vectorPK = new Vector3f();
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'gcSplits'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly sbyte[] gcSplits = new sbyte[]{1, 2, 4, 2, 3, 5, 3, 1, 6, 1, 4, 7, 2, 4, 8, 2, 5, 9, 3, 5, 10, 3, 6, 11, 1, 6, 12};
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'Cavity' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		//UPGRADE_NOTE: The access modifier for this class or class field has been changed in order to prevent compilation errors due to the visibility level. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1296'"
		internal class Cavity
		{
			private void  InitBlock(Dots enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Dots enclosingInstance;
			public Dots Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			//UPGRADE_NOTE: Final was removed from the declaration of 'ixI '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
			//UPGRADE_NOTE: Final was removed from the declaration of 'ixJ '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
			//UPGRADE_NOTE: Final was removed from the declaration of 'ixK '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
			internal int ixI;
			internal int ixJ;
			internal int ixK;
			//UPGRADE_NOTE: Final was removed from the declaration of 'points '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
			internal Point3f[] points;
			internal short colixI, colixJ, colixK;
			
			internal Cavity(Dots enclosingInstance)
			{
				InitBlock(enclosingInstance);
				ixI = Enclosing_Instance.indexI; ixJ = Enclosing_Instance.indexJ; ixK = Enclosing_Instance.indexK;
				
				points = new Point3f[25];
				for (int i = 25; --i >= 0; )
					points[i] = new Point3f();
				
				Enclosing_Instance.vectorPI.sub(Enclosing_Instance.centerI, Enclosing_Instance.probeIJK);
				Enclosing_Instance.vectorPI.normalize();
				points[1].scaleAdd(Enclosing_Instance.radiusP, Enclosing_Instance.vectorPI, Enclosing_Instance.probeIJK);
				
				Enclosing_Instance.vectorPJ.sub(Enclosing_Instance.centerJ, Enclosing_Instance.probeIJK);
				Enclosing_Instance.vectorPJ.normalize();
				points[2].scaleAdd(Enclosing_Instance.radiusP, Enclosing_Instance.vectorPJ, Enclosing_Instance.probeIJK);
				
				Enclosing_Instance.vectorPK.sub(Enclosing_Instance.centerK, Enclosing_Instance.probeIJK);
				Enclosing_Instance.vectorPK.normalize();
				points[3].scaleAdd(Enclosing_Instance.radiusP, Enclosing_Instance.vectorPK, Enclosing_Instance.probeIJK);
				
				Enclosing_Instance.vectorT.add(Enclosing_Instance.vectorPI, Enclosing_Instance.vectorPJ);
				Enclosing_Instance.vectorT.add(Enclosing_Instance.vectorPK);
				Enclosing_Instance.vectorT.normalize();
				points[0].scaleAdd(Enclosing_Instance.radiusP, Enclosing_Instance.vectorT, Enclosing_Instance.probeIJK);
				
				for (int i = 0; i < org.jmol.viewer.Dots.gcSplits.Length; i += 3)
					splitGreatCircle(org.jmol.viewer.Dots.gcSplits[i], org.jmol.viewer.Dots.gcSplits[i + 1], org.jmol.viewer.Dots.gcSplits[i + 2]);
				for (int i = 13; i < 25; ++i)
					splitGreatCircle(0, i - 12, i);
			}
			
			internal virtual void  splitGreatCircle(int indexA, int indexB, int indexMiddle)
			{
				Enclosing_Instance.vectorT.sub(points[indexA], Enclosing_Instance.probeIJK);
				Enclosing_Instance.vectorT1.sub(points[indexB], Enclosing_Instance.probeIJK);
				Enclosing_Instance.vectorT.add(Enclosing_Instance.vectorT1);
				Enclosing_Instance.vectorT.normalize();
				points[indexMiddle].scaleAdd(Enclosing_Instance.radiusP, Enclosing_Instance.vectorT, Enclosing_Instance.probeIJK);
			}
		}
		
		
		/*==============================================================*
		* All that it is trying to do is calculate the base point between
		* the two probes. This is the intersection of three planes:
		* the plane defined by atoms IJK, the bisecting plane of torusIJ,
		* and the bisecting plane of torusIK. <p>
		* I could not understand the algorithm that is described
		* in the Connolly article... seemed too complicated ... :-(
		* This algorithm takes finds the intersection of three planes,
		* where each plane is defined by a normal + a point on the plane
		*==============================================================*/
		internal virtual bool calcBaseIJK()
		{
			Vector3f v1 = torusIJ.axisVector;
			p1.set_Renamed(torusIJ.center);
			Vector3f v2 = torusIK.axisVector;
			p2.set_Renamed(torusIK.center);
			Vector3f v3 = uIJK;
			p3.set_Renamed(centerI);
			v2v3.cross(v2, v3);
			v3v1.cross(v3, v1);
			v1v2.cross(v1, v2);
			float denominator = v1.dot(v2v3);
			if (denominator == 0)
				return false;
			baseIJK.scale(v1.dot(p1), v2v3);
			baseIJK.scaleAdd(v2.dot(p2), v3v1, baseIJK);
			baseIJK.scaleAdd(v3.dot(p3), v1v2, baseIJK);
			baseIJK.scale(1 / denominator);
			return true;
		}
		
		internal virtual bool calcHeightIJK()
		{
			float hypotenuse2 = radiiIP2;
			vectorT.sub(baseIJK, centerI);
			float baseLength2 = vectorT.lengthSquared();
			float height2 = hypotenuse2 - baseLength2;
			if (height2 <= 0)
				return false;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			heightIJK = (float) System.Math.Sqrt(height2);
			return true;
		}
		
		internal static int[] allocateBitmap(int count)
		{
			return new int[(count + 31) >> 5];
		}
		
		internal static void  setBit(int[] bitmap, int i)
		{
			bitmap[(i >> 5)] |= 1 << (~ i & 31);
		}
		
		internal static void  clearBit(int[] bitmap, int i)
		{
			bitmap[(i >> 5)] &= ~ (1 << (~ i & 31));
		}
		
		internal static bool getBit(int[] bitmap, int i)
		{
			return (bitmap[(i >> 5)] << (i & 31)) < 0;
		}
		
		internal static void  setAllBits(int[] bitmap, int count)
		{
			int i = count >> 5;
			if ((count & 31) != 0)
				bitmap[i] = 0x80000000 >> (count - 1);
			while (--i >= 0)
				bitmap[i] = - 1;
		}
		
		internal static void  clearBitmap(int[] bitmap)
		{
			for (int i = bitmap.Length; --i >= 0; )
				bitmap[i] = 0;
		}
	}
}