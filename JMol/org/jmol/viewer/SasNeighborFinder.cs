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
using Graphics3D = org.jmol.g3d.Graphics3D;
//UPGRADE_TODO: The package 'javax.vecmath' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using javax.vecmath;
namespace org.jmol.viewer
{
	
	class SasNeighborFinder
	{
		virtual internal float ProbeRadius
		{
			set
			{
				if (value <= 0)
					throw new System.NullReferenceException();
				this.radiusP = value;
				diameterP = 2 * value;
			}
			
		}
		virtual internal int AtomI
		{
			set
			{
				if (LOG)
					System.Console.Out.WriteLine("setAtomI:" + value);
				this.indexI = value;
				atomI = atoms[value];
				centerI = atomI.point3f;
				radiusI = atomI.VanderwaalsRadiusFloat;
				radiiIP = radiusI + radiusP;
				radiiIP2 = radiiIP * radiiIP;
			}
			
		}
		virtual internal int NeighborJ
		{
			set
			{
				indexJ = neighborIndexes[value];
				if (LOG)
					System.Console.Out.WriteLine(" setNeighborJ:" + indexJ);
				atomJ = neighborAtoms[value];
				radiusJ = atomJ.VanderwaalsRadiusFloat;
				radiiJP = neighborPlusProbeRadii[value];
				radiiJP2 = neighborPlusProbeRadii2[value];
				centerJ = neighborCenters[value];
				distanceIJ2 = centerJ.distanceSquared(centerI);
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				distanceIJ = (float) System.Math.Sqrt(distanceIJ2);
			}
			
		}
		virtual internal int NeighborK
		{
			set
			{
				indexK = neighborIndexes[value];
				if (LOG)
					System.Console.Out.WriteLine("  setNeighborK:" + indexK);
				atomK = neighborAtoms[value];
				radiusK = atomK.VanderwaalsRadiusFloat;
				radiiKP = neighborPlusProbeRadii[value];
				radiiKP2 = neighborPlusProbeRadii2[value];
				centerK = neighborCenters[value];
				distanceIK2 = centerK.distanceSquared(centerI);
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				distanceIK = (float) System.Math.Sqrt(distanceIK2);
				distanceJK2 = centerK.distanceSquared(centerJ);
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				distanceJK = (float) System.Math.Sqrt(distanceJK2);
			}
			
		}
		//UPGRADE_NOTE: Final was removed from the declaration of 'frame '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Frame frame;
		//UPGRADE_NOTE: Final was removed from the declaration of 'sas1 '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Sasurface1 sas1;
		//UPGRADE_NOTE: Final was removed from the declaration of 'g3d '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Graphics3D g3d;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'atoms '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Atom[] atoms;
		private const bool LOG = false;
		
		internal SasNeighborFinder(Frame frame, Sasurface1 sas1, Graphics3D g3d)
		{
			this.sas1 = sas1;
			this.frame = frame;
			this.g3d = g3d;
			atoms = frame.atoms;
			maxVanderwaalsRadius = frame.MaxVanderwaalsRadius;
		}
		
		internal float radiusP, diameterP;
		internal float maxVanderwaalsRadius;
		
		internal virtual void  findAbuttingNeighbors(int atomIndex, System.Collections.BitArray bsSelected)
		{
			AtomI = atomIndex;
			getNeighbors(bsSelected);
			sortNeighborIndexes();
			calcCavitiesI();
		}
		
		internal int indexI, indexJ, indexK;
		internal Atom atomI, atomJ, atomK;
		internal Point3f centerI, centerJ, centerK;
		
		internal float radiusI, radiusJ, radiusK;
		internal float radiiIP, radiiJP, radiiKP;
		internal float radiiIP2, radiiJP2, radiiKP2;
		internal float distanceIJ, distanceIK, distanceJK;
		internal float distanceIJ2, distanceIK2, distanceJK2;
		
		internal int neighborCount;
		internal Atom[] neighborAtoms = new Atom[16];
		internal int[] neighborIndexes = new int[16];
		internal Point3f[] neighborCenters = new Point3f[16];
		internal float[] neighborPlusProbeRadii = new float[16];
		internal float[] neighborPlusProbeRadii2 = new float[16];
		internal int[] sortedNeighborIndexes = new int[16];
		
		internal virtual void  getNeighbors(System.Collections.BitArray bsSelected)
		{
			if (LOG)
				System.Console.Out.WriteLine("Surface.getNeighbors radiusI=" + radiusI + " diameterP=" + diameterP + " maxVdw=" + maxVanderwaalsRadius);
			neighborCount = 0;
			AtomIterator iter = frame.getWithinModelIterator(atomI, radiusI + diameterP + maxVanderwaalsRadius);
			while (iter.hasNext())
			{
				Atom neighbor = iter.next();
				if (neighbor == atomI)
					continue;
				// only consider selected neighbors
				if (!bsSelected.Get(neighbor.atomIndex))
					continue;
				float neighborRadius = neighbor.VanderwaalsRadiusFloat;
				if (centerI.distance(neighbor.point3f) > radiusI + radiusP + radiusP + neighborRadius)
					continue;
				if (neighborCount == neighborAtoms.Length)
				{
					neighborAtoms = (Atom[]) Util.doubleLength(neighborAtoms);
					neighborIndexes = Util.doubleLength(neighborIndexes);
					neighborCenters = (Point3f[]) Util.doubleLength(neighborCenters);
					neighborPlusProbeRadii = Util.doubleLength(neighborPlusProbeRadii);
					neighborPlusProbeRadii2 = Util.doubleLength(neighborPlusProbeRadii2);
				}
				neighborAtoms[neighborCount] = neighbor;
				neighborCenters[neighborCount] = neighbor.point3f;
				neighborIndexes[neighborCount] = neighbor.atomIndex;
				float radii = neighborRadius + radiusP;
				neighborPlusProbeRadii[neighborCount] = radii;
				neighborPlusProbeRadii2[neighborCount] = radii * radii;
				++neighborCount;
			}
			if (LOG)
			{
				System.Console.Out.WriteLine("neighborsFound=" + neighborCount);
				System.Console.Out.WriteLine("radiusI=" + radiusI + " maxVdwRadius=" + maxVanderwaalsRadius + " distMax=" + (radiusI + maxVanderwaalsRadius));
			}
		}
		
		internal virtual void  sortNeighborIndexes()
		{
			sortedNeighborIndexes = Util.ensureLength(sortedNeighborIndexes, neighborCount);
			for (int i = neighborCount; --i >= 0; )
				sortedNeighborIndexes[i] = i;
			for (int i = neighborCount; --i >= 0; )
				for (int j = i; --j >= 0; )
					if (neighborIndexes[sortedNeighborIndexes[i]] > neighborIndexes[sortedNeighborIndexes[j]])
					{
						int t = sortedNeighborIndexes[i];
						sortedNeighborIndexes[i] = sortedNeighborIndexes[j];
						sortedNeighborIndexes[j] = t;
					}
		}
		
		////////////////////////////////////////////////////////////////
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'vectorIJ '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private Vector3f vectorIJ = new Vector3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'vectorIK '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private Vector3f vectorIK = new Vector3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'normalIJK '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private Vector3f normalIJK = new Vector3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'torusCenterIJ '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private Point3f torusCenterIJ = new Point3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'torusCenterIK '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private Point3f torusCenterIK = new Point3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'torusCenterJK '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private Point3f torusCenterJK = new Point3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'probeBaseIJK '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private Point3f probeBaseIJK = new Point3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'probeCenter '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private Point3f probeCenter = new Point3f();
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'vectorPI '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private Vector3f vectorPI = new Vector3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'vectorPJ '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private Vector3f vectorPJ = new Vector3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'vectorPK '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private Vector3f vectorPK = new Vector3f();
		
		internal virtual void  calcCavitiesI()
		{
			if (radiusP == 0)
				return ;
			for (int iJ = neighborCount; --iJ >= 0; )
			{
				int sortedIndexJ = sortedNeighborIndexes[iJ];
				if (neighborIndexes[sortedIndexJ] <= indexI)
					continue;
				NeighborJ = sortedIndexJ;
				// deal with corrupt files that have duplicate atoms
				if (distanceIJ < 0.2)
					continue;
				// deal with one atom contained inside another
				// as with a hydrogen in 1D68.pdb
				if (radiusI + distanceIJ < radiusJ || radiusJ + distanceIJ < radiusI)
				{
					System.Console.Out.WriteLine("embedded atom:" + indexI + "<->" + indexJ);
					continue;
				}
				vectorIJ.sub(centerJ, centerI);
				calcTorusCenter(centerI, radiiIP2, centerJ, radiiJP2, distanceIJ2, torusCenterIJ);
				for (int iK = neighborCount; --iK >= 0; )
				{
					int sortedIndexK = sortedNeighborIndexes[iK];
					if (neighborIndexes[sortedIndexK] <= indexJ)
						continue;
					NeighborK = sortedIndexK;
					// deal with corrupt files that have duplicate atoms
					if (distanceIK < 0.1 || distanceJK < 0.1)
						continue;
					if (distanceJK >= radiiJP + radiiKP)
						continue;
					getCavitiesIJK();
				}
				checkFullTorusIJ();
			}
			// check for an isolated atom with no neighbors
			if (neighborCount == 0)
				sas1.allocateConvexVertexBitmap(indexI);
		}
		
		internal virtual void  calcTorusCenter(Point3f centerA, float radiiAP2, Point3f centerB, float radiiBP2, float distanceAB2, Point3f torusCenter)
		{
			/*
			System.out.println("calcTorusCenter(" + centerA + "," + radiiAP2 + "," +
			centerB + "," + radiiBP2 + "," +
			distanceAB2 + "," + ")");
			*/
			torusCenter.sub(centerB, centerA);
			torusCenter.scale((radiiAP2 - radiiBP2) / distanceAB2);
			torusCenter.add(centerA);
			torusCenter.add(centerB);
			torusCenter.scale(0.5f);
			/*
			System.out.println("torusCenter=" + torusCenter);
			*/
		}
		
		internal virtual bool checkProbeNotIJ(Point3f probeCenter)
		{
			for (int i = neighborCount; --i >= 0; )
			{
				int neighborIndex = neighborIndexes[i];
				if (neighborIndex == indexI || neighborIndex == indexJ)
					continue;
				if (probeCenter.distanceSquared(neighborCenters[i]) < neighborPlusProbeRadii2[i])
					return false;
			}
			return true;
		}
		
		internal virtual bool checkProbeAgainstNeighborsIJK(Point3f probeCenter)
		{
			for (int i = neighborCount; --i >= 0; )
			{
				int neighborIndex = neighborIndexes[i];
				if (neighborIndex == indexI || neighborIndex == indexJ || neighborIndex == indexK)
					continue;
				if (probeCenter.distanceSquared(neighborCenters[i]) < neighborPlusProbeRadii2[i])
					return false;
			}
			return true;
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'vectorT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f vectorT = new Vector3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'v2v3 '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f v2v3 = new Vector3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'v3v1 '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f v3v1 = new Vector3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'v1v2 '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f v1v2 = new Vector3f();
		
		internal virtual bool intersectPlanes(Vector3f v1, Point3f p1, Vector3f v2, Point3f p2, Vector3f v3, Point3f p3, Point3f intersection)
		{
			v2v3.cross(v2, v3);
			if (Float.isNaN(v2v3.x))
				return false;
			v3v1.cross(v3, v1);
			if (Float.isNaN(v3v1.x))
				return false;
			v1v2.cross(v1, v2);
			if (Float.isNaN(v1v2.x))
				return false;
			float denominator = v1.dot(v2v3);
			if (denominator == 0)
				return false;
			vectorT.set_Renamed(p1);
			intersection.scale(v1.dot(vectorT), v2v3);
			vectorT.set_Renamed(p2);
			intersection.scaleAdd(v2.dot(vectorT), v3v1, intersection);
			vectorT.set_Renamed(p3);
			intersection.scaleAdd(v3.dot(vectorT), v1v2, intersection);
			intersection.scale(1 / denominator);
			if (Float.isNaN(intersection.x))
				return false;
			return true;
		}
		
		internal virtual float calcProbeHeightIJK(Point3f probeBaseIJK)
		{
			float hypotenuse2 = radiiIP2;
			vectorT.sub(probeBaseIJK, centerI);
			float baseLength2 = vectorT.lengthSquared();
			float height2 = hypotenuse2 - baseLength2;
			if (height2 <= 0)
				return 0;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			return (float) System.Math.Sqrt(height2);
		}
		
		internal virtual void  getCavitiesIJK()
		{
			if (LOG)
				System.Console.Out.WriteLine("getCavitiesIJK:" + indexI + "," + indexJ + "," + indexK);
			vectorIK.sub(centerK, centerI);
			normalIJK.cross(vectorIJ, vectorIK);
			if (Float.isNaN(normalIJK.x))
				return ;
			normalIJK.normalize();
			calcTorusCenter(centerI, radiiIP2, centerK, radiiKP2, distanceIK2, torusCenterIK);
			if (!intersectPlanes(vectorIJ, torusCenterIJ, vectorIK, torusCenterIK, normalIJK, centerI, probeBaseIJK))
				return ;
			float probeHeight = calcProbeHeightIJK(probeBaseIJK);
			if (probeHeight <= 0)
				return ;
			Sasurface1.Torus torusIJ = null, torusIK = null, torusJK = null;
			for (int i = - 1; i <= 1; i += 2)
			{
				probeCenter.scaleAdd(i * probeHeight, normalIJK, probeBaseIJK);
				if (checkProbeAgainstNeighborsIJK(probeCenter))
				{
					SasCavity cavity = new SasCavity(centerI, centerJ, centerK, probeCenter, radiusP, probeBaseIJK, vectorPI, vectorPJ, vectorPK, vectorT, g3d);
					sas1.addCavity(indexI, indexJ, indexK, cavity);
					bool rightHanded = (i == 1);
					if (LOG)
						System.Console.Out.WriteLine(" indexI=" + indexI + " indexJ=" + indexJ + " indexK=" + indexK);
					if (torusIJ == null && (torusIJ = sas1.getTorus(indexI, indexJ)) == null)
						torusIJ = sas1.createTorus(indexI, indexJ, torusCenterIJ, calcTorusRadius(radiusI, radiusJ, distanceIJ2), false);
					if (torusIJ != null)
						torusIJ.addCavity(cavity, rightHanded);
					
					if (torusIK == null && (torusIK = sas1.getTorus(indexI, indexK)) == null)
						torusIK = sas1.createTorus(indexI, indexK, torusCenterIK, calcTorusRadius(radiusI, radiusK, distanceIK2), false);
					if (torusIK != null)
						torusIK.addCavity(cavity, !rightHanded);
					
					if (torusJK == null && (torusJK = sas1.getTorus(indexJ, indexK)) == null)
					{
						calcTorusCenter(centerJ, radiiJP2, centerK, radiiKP2, distanceJK2, torusCenterJK);
						torusJK = sas1.createTorus(indexJ, indexK, torusCenterJK, calcTorusRadius(radiusJ, radiusK, distanceJK2), false);
					}
					if (torusJK != null)
						torusJK.addCavity(cavity, rightHanded);
				}
			}
		}
		
		// check for a full torus with no cavities between I & J
		internal virtual float calcTorusRadius(float radiusA, float radiusB, float distanceAB2)
		{
			float t1 = radiusA + radiusB + diameterP;
			float t2 = t1 * t1 - distanceAB2;
			float diff = radiusA - radiusB;
			float t3 = distanceAB2 - diff * diff;
			if (t2 <= 0 || t3 <= 0 || distanceAB2 == 0)
			{
				System.Console.Out.WriteLine("calcTorusRadius\n" + " radiusA=" + radiusA + " radiusB=" + radiusB + " distanceAB2=" + distanceAB2);
				System.Console.Out.WriteLine("distanceAB=" + System.Math.Sqrt(distanceAB2) + " t1=" + t1 + " t2=" + t2 + " diff=" + diff + " t3=" + t3);
				throw new System.NullReferenceException();
			}
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			return (float) (0.5 * System.Math.Sqrt(t2) * System.Math.Sqrt(t3) / System.Math.Sqrt(distanceAB2));
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'unitRadialVectorT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f unitRadialVectorT = new Vector3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'vectorZ '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly Vector3f vectorZ = new Vector3f(0, 0, 1);
		//UPGRADE_NOTE: Final was removed from the declaration of 'pointT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Point3f pointT = new Point3f();
		
		internal virtual void  checkFullTorusIJ()
		{
			if (sas1.getTorus(indexI, indexJ) == null)
			{
				if (vectorIJ.z == 0)
					unitRadialVectorT.set_Renamed(vectorZ);
				else
				{
					unitRadialVectorT.set_Renamed(- vectorIJ.y, vectorIJ.x, 0);
					unitRadialVectorT.normalize();
				}
				float torusRadiusIJ = calcTorusRadius(radiusI, radiusJ, distanceIJ2);
				if (torusRadiusIJ > radiusP)
				{
					pointT.scaleAdd(torusRadiusIJ, unitRadialVectorT, torusCenterIJ);
					if (checkProbeNotIJ(pointT))
						sas1.createTorus(indexI, indexJ, torusCenterIJ, torusRadiusIJ, true);
				}
			}
		}
	}
}