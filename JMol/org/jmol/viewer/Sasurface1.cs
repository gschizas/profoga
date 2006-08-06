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
using IntInt2ObjHash = org.jmol.util.IntInt2ObjHash;
using Graphics3D = org.jmol.g3d.Graphics3D;
//UPGRADE_TODO: The package 'javax.vecmath' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using javax.vecmath;
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
	/// surface of the molecule. In this way, a smooth surface is generated ...n
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
	
	class Sasurface1
	{
		private void  InitBlock()
		{
			GEODESIC_CALC_LEVEL = Sasurface.MAX_GEODESIC_RENDERING_LEVEL;
			MAX_FULL_TORUS_STEP_COUNT = Sasurface.MAX_FULL_TORUS_STEP_COUNT;
			OUTER_TORUS_STEP_COUNT = Sasurface.OUTER_TORUS_STEP_COUNT;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			TARGET_INNER_TORUS_STEP_ANGLE = (float) (2 * System.Math.PI / (MAX_FULL_TORUS_STEP_COUNT - 1));
			geodesicRenderingLevel = GEODESIC_CALC_LEVEL;
			outerRadials = new Vector3f[OUTER_TORUS_STEP_COUNT];
			for (int i = outerRadials.length; --i >= 0; )
				outerRadials[i] = new Vector3f();
		}
		
		internal System.String surfaceID;
		internal Graphics3D g3d;
		internal Viewer viewer;
		internal short colix;
		internal Frame frame;
		
		internal short mad; // this is really just a true/false flag ... 0 vs non-zero
		
		internal bool hide;
		
		//UPGRADE_NOTE: The initialization of  'GEODESIC_CALC_LEVEL' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private int GEODESIC_CALC_LEVEL;
		//UPGRADE_NOTE: The initialization of  'MAX_FULL_TORUS_STEP_COUNT' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		internal int MAX_FULL_TORUS_STEP_COUNT;
		//UPGRADE_NOTE: The initialization of  'OUTER_TORUS_STEP_COUNT' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		internal int OUTER_TORUS_STEP_COUNT;
		
		//UPGRADE_NOTE: The initialization of  'TARGET_INNER_TORUS_STEP_ANGLE' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		internal float TARGET_INNER_TORUS_STEP_ANGLE;
		
		//UPGRADE_NOTE: The initialization of  'geodesicRenderingLevel' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		internal int geodesicRenderingLevel;
		
		internal int surfaceConvexMax; // the Max == the highest atomIndex with surface + 1
		internal int[][] convexVertexMaps;
		internal int[][] convexFaceMaps;
		internal short[] colixesConvex;
		internal int geodesicVertexCount;
		
		internal int cavityCount;
		internal SasCavity[] cavities;
		internal int torusCount;
		internal Torus[] toruses;
		
		private IntInt2ObjHash htToruses;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'gem '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal SasGem gem;
		//UPGRADE_NOTE: Final was removed from the declaration of 'neighborFinder '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private SasNeighborFinder neighborFinder;
		
		//private final static boolean LOG = false;
		
		////////////////////////////////////////////////////////////////
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'pointT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Point3f pointT = new Point3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'zeroPointT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Point3f zeroPointT = new Point3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'centerPointT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Point3f centerPointT = new Point3f();
		//private final Point3f centerPointAT = new Point3f();
		//private final Point3f centerPointBT = new Point3f();
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'PI '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
		private static readonly float PI = (float) System.Math.PI;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'torusCavityAngleVectorT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f torusCavityAngleVectorT = new Vector3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'matrixT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Matrix3f matrixT = new Matrix3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'aaT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal AxisAngle4f aaT = new AxisAngle4f();
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'vectorX '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly Vector3f vectorX = new Vector3f(1, 0, 0);
		//UPGRADE_NOTE: Final was removed from the declaration of 'vectorY '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly Vector3f vectorY = new Vector3f(0, 1, 0);
		//UPGRADE_NOTE: Final was removed from the declaration of 'vectorZ '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly Vector3f vectorZ = new Vector3f(0, 0, 1);
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'unitRadialVectorT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f unitRadialVectorT = new Vector3f();
		// 90 degrees, although everything is in radians
		//UPGRADE_NOTE: Final was removed from the declaration of 'radialVector90T '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f radialVector90T = new Vector3f();
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'vectorT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f vectorT = new Vector3f();
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'outerRadials '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_NOTE: The initialization of  'outerRadials' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		internal Vector3f[] outerRadials;
		
		/*
		* radius and diameter of the probe. 0 == no probe
		*/
		internal float radiusP, diameterP;
		
		////////////////////////////////////////////////////////////////
		
		internal Sasurface1(System.String surfaceID, Viewer viewer, Graphics3D g3d, short colix, System.Collections.BitArray bs)
		{
			InitBlock();
			this.surfaceID = surfaceID;
			this.viewer = viewer;
			this.g3d = g3d;
			this.colix = colix;
			
			frame = viewer.Frame;
			gem = new SasGem(viewer, g3d, frame, GEODESIC_CALC_LEVEL);
			neighborFinder = new SasNeighborFinder(frame, this, g3d);
			geodesicVertexCount = g3d.getGeodesicVertexCount(GEODESIC_CALC_LEVEL);
			generate(bs);
		}
		
		internal virtual void  clearAll()
		{
			surfaceConvexMax = 0;
			convexVertexMaps = null;
			convexFaceMaps = null;
			torusCount = 0;
			toruses = null;
			cavityCount = 0;
			cavities = null;
			htToruses = null;
			radiusP = viewer.CurrentSolventProbeRadius;
			diameterP = 2 * radiusP;
			neighborFinder.ProbeRadius = radiusP;
		}
		
		internal virtual void  generate(System.Collections.BitArray bsSelected)
		{
			viewer.SolventOn = true;
			clearAll();
			int atomCount = frame.atomCount;
			convexVertexMaps = new int[atomCount][];
			convexFaceMaps = new int[atomCount][];
			colixesConvex = new short[atomCount];
			
			htToruses = new IntInt2ObjHash();
			// now, calculate surface for selected atoms
			long timeBegin = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
			int surfaceAtomCount = 0;
			for (int i = 0; i < atomCount; ++i)
			{
				// make this loop count up
				if (bsSelected.Get(i))
				{
					++surfaceAtomCount;
					neighborFinder.findAbuttingNeighbors(i, bsSelected);
				}
			}
			
			for (int i = torusCount; --i >= 0; )
			{
				Torus torus = toruses[i];
				torus.checkCavityCorrectness0();
				torus.checkCavityCorrectness1();
				torus.electReferenceCavity();
				torus.calcVectors();
				torus.calcCavityAnglesAndSort();
				torus.checkCavityCorrectness2();
				torus.buildTorusSegments();
				torus.calcPointCounts();
				torus.calcNormixes();
				
				torus.clipVertexMaps();
			}
			
			for (int i = torusCount; --i >= 0; )
			{
				Torus torus = toruses[i];
				torus.stitchWithGeodesics();
			}
			
			/*
			for (int i = 0; i < torusCount; ++i) {
			toruses[i].dumpTorusSegmentStuff();
			}
			*/
			
			for (int i = atomCount; --i >= 0; )
			{
				int[] vertexMap = convexVertexMaps[i];
				if (vertexMap != null)
				{
					convexFaceMaps[i] = gem.calcFaceBitmap(vertexMap);
					// temp hack
					convexVertexMaps[i] = gem.calcFaceVertexBitmap(convexFaceMaps[i]);
				}
			}
			
			long timeElapsed = (System.DateTime.Now.Ticks - 621355968000000000) / 10000 - timeBegin;
			System.Console.Out.WriteLine("surface atom count=" + surfaceAtomCount);
			System.Console.Out.WriteLine("Surface construction time = " + timeElapsed + " ms");
			htToruses = null;
			// update this count to slightly speed up surfaceRenderer
			int i2;
			for (i2 = atomCount; --i2 >= 0 && convexVertexMaps[i2] == null; )
			{
			}
			surfaceConvexMax = i2 + 1;
		}
		
		internal virtual void  setSize(int size, System.Collections.BitArray bsSelected)
		{
			System.Console.Out.WriteLine("Who is calling me?");
			throw new System.NullReferenceException();
		}
		
		internal virtual void  setProperty(System.String propertyName, System.Object value_Renamed, System.Collections.BitArray bs)
		{
			int atomCount = frame.atomCount;
			Atom[] atoms = frame.atoms;
			if ((System.Object) "color" == (System.Object) propertyName)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				System.Console.Out.WriteLine("I am surfaceID:" + surfaceID + " Surface.setProperty(color," + value_Renamed + ")");
				setProperty("colorConvex", value_Renamed, bs);
				setProperty("colorConcave", value_Renamed, bs);
				setProperty("colorSaddle", value_Renamed, bs);
			}
			if ((System.Object) "translucency" == (System.Object) propertyName)
			{
				setProperty("translucencyConvex", value_Renamed, bs);
				setProperty("translucencyConcave", value_Renamed, bs);
				setProperty("translucencySaddle", value_Renamed, bs);
			}
			if ((System.Object) "colorConvex" == (System.Object) propertyName)
			{
				short colix = Graphics3D.getColix(value_Renamed);
				for (int i = atomCount; --i >= 0; )
					if (bs.Get(i))
						colixesConvex[i] = (colix != Graphics3D.UNRECOGNIZED)?colix:viewer.getColixAtomPalette(atoms[i], (System.String) value_Renamed);
				return ;
			}
			if ((System.Object) "translucencyConvex" == (System.Object) propertyName)
			{
				bool isTranslucent = ((System.Object) "translucent" == value_Renamed);
				for (int i = atomCount; --i >= 0; )
					if (bs.Get(i))
						colixesConvex[i] = Graphics3D.setTranslucent(colixesConvex[i], isTranslucent);
				return ;
			}
			if ((System.Object) "colorSaddle" == (System.Object) propertyName)
			{
				short colix = Graphics3D.getColix(value_Renamed);
				for (int i = torusCount; --i >= 0; )
				{
					Torus torus = toruses[i];
					if (bs.Get(torus.ixA))
						torus.colixA = (colix != Graphics3D.UNRECOGNIZED)?colix:viewer.getColixAtomPalette(atoms[torus.ixA], (System.String) value_Renamed);
					if (bs.Get(torus.ixB))
						torus.colixB = (colix != Graphics3D.UNRECOGNIZED)?colix:viewer.getColixAtomPalette(atoms[torus.ixB], (System.String) value_Renamed);
				}
				return ;
			}
			if ((System.Object) "translucencySaddle" == (System.Object) propertyName)
			{
				bool isTranslucent = ((System.Object) "translucent" == value_Renamed);
				for (int i = torusCount; --i >= 0; )
				{
					Torus torus = toruses[i];
					if (bs.Get(torus.ixA))
						torus.colixA = Graphics3D.setTranslucent(torus.colixA, isTranslucent);
					if (bs.Get(torus.ixB))
						torus.colixB = Graphics3D.setTranslucent(torus.colixB, isTranslucent);
				}
				return ;
			}
			if ((System.Object) "colorConcave" == (System.Object) propertyName)
			{
				/*
				short colix = Graphics3D.getColix(value);
				for (int i = cavityCount; --i >= 0; ) {
				Cavity cavity = cavities[i];
				if (bs.get(cavity.ixI))
				cavity.colixI = 
				(colix != Graphics3D.UNRECOGNIZED)
				? colix
				: viewer.getColixAtomPalette(atoms[cavity.ixI], (String)value);
				if (bs.get(cavity.ixJ))
				cavity.colixJ = 
				(colix != Graphics3D.UNRECOGNIZED)
				? colix
				: viewer.getColixAtomPalette(atoms[cavity.ixJ], (String)value);
				if (bs.get(cavity.ixK))
				cavity.colixK = 
				(colix != Graphics3D.UNRECOGNIZED)
				? colix
				: viewer.getColixAtomPalette(atoms[cavity.ixK], (String)value);
				}
				*/
				return ;
			}
			if ((System.Object) "translucencyConcave" == (System.Object) propertyName)
			{
				/*
				boolean isTranslucent = ("translucent" == value);
				for (int i = cavityCount; --i >= 0; ) {
				Cavity cavity = cavities[i];
				if (bs.get(cavity.ixI))
				cavity.colixI = Graphics3D.setTranslucent(cavity.colixI,
				isTranslucent);
				if (bs.get(cavity.ixJ))
				cavity.colixJ = Graphics3D.setTranslucent(cavity.colixJ,
				isTranslucent);
				if (bs.get(cavity.ixK))
				cavity.colixK = Graphics3D.setTranslucent(cavity.colixK,
				isTranslucent);
				}
				*/
				return ;
			}
			
			if ((System.Object) "off" == (System.Object) propertyName)
			{
				hide = true;
				return ;
			}
			
			if ((System.Object) "on" == (System.Object) propertyName)
			{
				hide = false;
				return ;
			}
		}
		
		internal virtual void  calcVectors0and90(Point3f planeCenter, Vector3f axisVector, Point3f planeZeroPoint, Vector3f vector0, Vector3f vector90)
		{
			vector0.sub(planeZeroPoint, planeCenter);
			aaT.set_Renamed(axisVector, PI / 2);
			matrixT.set_Renamed(aaT);
			matrixT.transform(vector0, vector90);
		}
		
		////////////////////////////////////////////////////////////////
		
		internal virtual void  calcClippingPlaneCenter(Point3f axisPoint, Vector3f axisUnitVector, Point3f planePoint, Point3f planeCenterPoint)
		{
			vectorT.sub(axisPoint, planePoint);
			float distance = axisUnitVector.dot(vectorT);
			planeCenterPoint.scaleAdd(- distance, axisUnitVector, axisPoint);
		}
		
		internal Point3f[] convexEdgePoints;
		// what is the max size of this thing?
		internal short[] edgeVertexes;
		
		internal virtual void  allocateConvexVertexBitmap(int atomIndex)
		{
			if (convexVertexMaps[atomIndex] == null)
				convexVertexMaps[atomIndex] = Bmp.allocateSetAllBits(geodesicVertexCount);
		}
		
		internal virtual Torus createTorus(int indexI, int indexJ, Point3f torusCenterIJ, float torusRadius, bool fullTorus)
		{
			if (torusRadius < radiusP)
				return null;
			if (indexI >= indexJ)
				throw new System.NullReferenceException();
			if (htToruses.get_Renamed(indexI, indexJ) != null)
				throw new System.NullReferenceException();
			allocateConvexVertexBitmap(indexI);
			allocateConvexVertexBitmap(indexJ);
			Torus torus = new Torus(indexI, indexJ, torusCenterIJ, torusRadius, fullTorus);
			htToruses.put(indexI, indexJ, torus);
			saveTorus(torus);
			return torus;
		}
		
		internal virtual void  saveTorus(Torus torus)
		{
			if (toruses == null)
				toruses = new Torus[128];
			else if (torusCount == toruses.Length)
				toruses = (Torus[]) Util.doubleLength(toruses);
			toruses[torusCount++] = torus;
		}
		
		internal virtual Torus getTorus(int atomIndexA, int atomIndexB)
		{
			if (atomIndexA >= atomIndexB)
				throw new System.NullReferenceException();
			return (Torus) htToruses.get_Renamed(atomIndexA, atomIndexB);
		}
		
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
		
		internal virtual void  addCavity(int indexI, int indexJ, int indexK, SasCavity cavity)
		{
			if (cavities == null)
				cavities = new SasCavity[32];
			else if (cavityCount == cavities.Length)
				cavities = (SasCavity[]) Util.doubleLength(cavities);
			cavities[cavityCount++] = cavity;
			
			allocateConvexVertexBitmap(indexI);
			allocateConvexVertexBitmap(indexJ);
			allocateConvexVertexBitmap(indexK);
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'Torus' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		//UPGRADE_NOTE: The access modifier for this class or class field has been changed in order to prevent compilation errors due to the visibility level. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1296'"
		internal class Torus
		{
			private void  InitBlock(Sasurface1 enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Sasurface1 enclosingInstance;
			public Sasurface1 Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			//UPGRADE_NOTE: Final was removed from the declaration of 'ixA '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
			//UPGRADE_NOTE: Final was removed from the declaration of 'ixB '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
			internal int ixA;
			internal int ixB;
			//UPGRADE_NOTE: Final was removed from the declaration of 'center '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
			internal Point3f center;
			//UPGRADE_NOTE: Final was removed from the declaration of 'radius '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
			internal float radius;
			
			//UPGRADE_NOTE: Final was removed from the declaration of 'radialVector '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
			internal Vector3f radialVector = new Vector3f();
			//UPGRADE_NOTE: Final was removed from the declaration of 'axisUnitVector '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
			internal Vector3f axisUnitVector = new Vector3f();
			//UPGRADE_NOTE: Final was removed from the declaration of 'tangentVector '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
			internal Vector3f tangentVector = new Vector3f();
			//UPGRADE_NOTE: Final was removed from the declaration of 'outerRadial '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
			internal Vector3f outerRadial = new Vector3f();
			internal float outerAngle;
			internal short colixA, colixB;
			
			internal sbyte outerPointCount;
			internal sbyte segmentStripCount;
			internal short totalPointCount;
			
			internal short[] normixes;
			
			internal short[] connectAConvex;
			
			internal short[] seamA;
			internal short[] seamB;
			
			//UPGRADE_NOTE: Final was removed from the declaration of 'fullTorus '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
			internal bool fullTorus;
			internal short torusCavityCount;
			internal TorusCavity[] torusCavities;
			
			internal Torus(Sasurface1 enclosingInstance, int indexA, int indexB, Point3f center, float radius, bool fullTorus)
			{
				InitBlock(enclosingInstance);
				this.ixA = indexA;
				this.ixB = indexB;
				this.center = new Point3f(center);
				this.radius = radius;
				this.fullTorus = fullTorus;
			}
			
			internal virtual void  dumpTorusSegmentStuff()
			{
				for (int i = 0; i < torusSegmentCount; ++i)
				{
					torusSegments[i].dumpStuff();
				}
			}
			
			internal virtual void  electReferenceCavity()
			{
				if (fullTorus)
					return ;
				if (torusCavities[0].rightHanded)
					return ;
				for (int i = torusCavityCount; --i > 0; )
				{
					TorusCavity torusCavity = torusCavities[i];
					if (torusCavity.rightHanded)
					{
						torusCavities[i] = torusCavities[0];
						torusCavities[0] = torusCavity;
						break;
					}
				}
				if (!torusCavities[0].rightHanded)
					throw new System.NullReferenceException();
			}
			
			internal virtual void  calcVectors()
			{
				Point3f centerA = Enclosing_Instance.frame.atoms[ixA].point3f;
				Point3f centerB = Enclosing_Instance.frame.atoms[ixB].point3f;
				axisUnitVector.sub(centerB, centerA);
				axisUnitVector.normalize();
				
				Point3f referenceProbePoint = null;
				if (torusCavities != null)
				{
					referenceProbePoint = torusCavities[0].cavity.probeCenter;
				}
				else
				{
					// it is a full torus, so it does not really matter where
					// we put it;
					if (axisUnitVector.x == 0)
						Enclosing_Instance.unitRadialVectorT.set_Renamed(org.jmol.viewer.Sasurface1.vectorX);
					else if (axisUnitVector.y == 0)
						Enclosing_Instance.unitRadialVectorT.set_Renamed(org.jmol.viewer.Sasurface1.vectorY);
					else if (axisUnitVector.z == 0)
						Enclosing_Instance.unitRadialVectorT.set_Renamed(org.jmol.viewer.Sasurface1.vectorZ);
					else
					{
						Enclosing_Instance.unitRadialVectorT.set_Renamed(- axisUnitVector.y, axisUnitVector.x, 0);
						Enclosing_Instance.unitRadialVectorT.normalize();
					}
					referenceProbePoint = Enclosing_Instance.pointT;
					Enclosing_Instance.pointT.scaleAdd(radius, Enclosing_Instance.unitRadialVectorT, center);
				}
				
				calcVectors0and90(center, axisUnitVector, referenceProbePoint, radialVector, Enclosing_Instance.radialVector90T);
				
				tangentVector.cross(axisUnitVector, radialVector);
				tangentVector.normalize();
				
				outerRadial.sub(centerA, referenceProbePoint);
				outerRadial.normalize();
				outerRadial.scale(Enclosing_Instance.radiusP);
				
				Enclosing_Instance.vectorT.sub(centerB, referenceProbePoint);
				outerAngle = outerRadial.angle(Enclosing_Instance.vectorT);
			}
			
			internal virtual void  calcPointCounts()
			{
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				int c = (int) (Enclosing_Instance.OUTER_TORUS_STEP_COUNT * outerAngle / System.Math.PI);
				c = (c + 1) & 0xFE;
				if (c > Enclosing_Instance.OUTER_TORUS_STEP_COUNT)
					c = Enclosing_Instance.OUTER_TORUS_STEP_COUNT;
				else if (c == 0)
					c = 2;
				
				int t = 0;
				for (int i = torusSegmentCount; --i >= 0; )
					t += torusSegments[i].stepCount;
				//      System.out.println("segmentStripCount t=" + t);
				segmentStripCount = (sbyte) t;
				outerPointCount = (sbyte) c;
				totalPointCount = (short) (t * c);
				if (totalPointCount == 0)
				{
					System.Console.Out.WriteLine("?Que? why is this a torus?");
					System.Console.Out.WriteLine("calcPointCounts: " + " outerAngle=" + outerAngle + " segmentStripCount=" + segmentStripCount + " outerPointCount=" + outerPointCount + " totalPointCount=" + totalPointCount);
					for (int i = 0; i < torusSegmentCount; ++i)
					{
						TorusSegment ts = torusSegments[i];
						System.Console.Out.WriteLine("  torusSegment[" + i + "] : " + " .startAngle=" + ts.startAngle + " .stepAngle=" + ts.stepAngle + " .stepCount=" + ts.stepCount);
					}
					throw new System.NullReferenceException();
				}
			}
			
			internal virtual void  transformOuterRadials()
			{
				float stepAngle1 = (outerPointCount <= 1)?0:outerAngle / (outerPointCount - 1);
				Enclosing_Instance.aaT.set_Renamed(tangentVector, stepAngle1 * outerPointCount);
				for (int i = outerPointCount; --i > 0; )
				{
					Enclosing_Instance.aaT.angle -= stepAngle1;
					Enclosing_Instance.matrixT.set_Renamed(Enclosing_Instance.aaT);
					Enclosing_Instance.matrixT.transform(outerRadial, Enclosing_Instance.outerRadials[i]);
				}
				Enclosing_Instance.outerRadials[0].set_Renamed(outerRadial);
			}
			
			internal virtual void  addCavity(SasCavity cavity, bool rightHanded)
			{
				if (fullTorus)
					throw new System.NullReferenceException();
				if (torusCavities == null)
					torusCavities = new TorusCavity[2];
				else if (torusCavityCount == torusCavities.Length)
					torusCavities = (TorusCavity[]) Util.doubleLength(torusCavities);
				torusCavities[torusCavityCount] = new TorusCavity(enclosingInstance, cavity, rightHanded);
				++torusCavityCount;
			}
			
			internal virtual void  checkCavityCorrectness0()
			{
				if (fullTorus ^ (torusCavityCount == 0))
					throw new System.NullReferenceException();
			}
			
			internal virtual void  checkCavityCorrectness1()
			{
				if ((torusCavityCount & 1) != 0)
					throw new System.NullReferenceException();
				int rightCount = 0;
				for (int i = torusCavityCount; --i >= 0; )
					if (torusCavities[i].rightHanded)
						++rightCount;
				if (rightCount != torusCavityCount / 2)
					throw new System.NullReferenceException();
			}
			
			internal virtual void  calcCavityAnglesAndSort()
			{
				if (fullTorus)
					return ;
				// because of previous election, torusCavities[0] has angle 0;
				for (int i = torusCavityCount; --i > 0; )
					torusCavities[i].calcAngle(center, radialVector, Enclosing_Instance.radialVector90T);
				sortTorusCavitiesByAngle();
			}
			
			internal virtual void  sortTorusCavitiesByAngle()
			{
				// no need to sort entry #0, whose angle (by definition) is zero
				for (int i = torusCavityCount; --i >= 2; )
				{
					TorusCavity champion = torusCavities[i];
					for (int j = i; --j > 0; )
					{
						TorusCavity challenger = torusCavities[j];
						if (challenger.angle > champion.angle)
						{
							torusCavities[j] = champion;
							torusCavities[i] = champion = challenger;
						}
					}
				}
			}
			
			internal virtual void  checkCavityCorrectness2()
			{
				if (fullTorus)
					return ;
				if ((torusCavityCount & 1) != 0)
				// ensure even number
					throw new System.NullReferenceException();
				if (torusCavities[0].angle != 0)
					throw new System.NullReferenceException();
				for (int i = torusCavityCount; --i > 0; )
				{
					if (torusCavities[i].angle <= torusCavities[i - 1].angle && i != torusCavityCount - 1)
					{
						//System.out.println("oops! <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
						//for (int j = 0; j < torusCavityCount; ++j) {
						//System.out.println("cavity:" + j + " " +
						//                   torusCavities[j].angle + " " +
						//                   torusCavities[j].rightHanded);
						//}
						throw new System.NullReferenceException();
					}
					if (((i & 1) == 0) ^ torusCavities[i].rightHanded)
						throw new System.NullReferenceException();
				}
			}
			
			internal virtual void  buildTorusSegments()
			{
				if (torusCavityCount == 0)
				{
					addTorusSegment(new TorusSegment(this));
				}
				else
				{
					for (int i = 0; i < torusCavityCount; i += 2)
						addTorusSegment(new TorusSegment(this, torusCavities[i], torusCavities[i + 1]));
				}
			}
			
			internal int torusSegmentCount;
			internal TorusSegment[] torusSegments;
			
			internal virtual void  addTorusSegment(TorusSegment torusSegment)
			{
				if (torusSegments == null)
					torusSegments = new TorusSegment[4];
				if (torusSegmentCount == torusSegments.Length)
					torusSegments = (TorusSegment[]) Util.doubleLength(torusSegments);
				torusSegments[torusSegmentCount++] = torusSegment;
			}
			
			internal virtual void  calcNormixes()
			{
				transformOuterRadials();
				short[] normixes = this.normixes = new short[totalPointCount];
				int ix = 0;
				for (int i = 0; i < torusSegmentCount; ++i)
					ix = torusSegments[i].calcNormixes(normixes, ix);
			}
			
			internal virtual void  calcPoints(Point3f[] points)
			{
				//System.out.println("Sasurface1.Torus.calcPoints " +
				//                 " torusSegmentCount=" + torusSegmentCount);
				int indexStart = 0;
				transformOuterRadials();
				for (int i = 0; i < torusSegmentCount; ++i)
					indexStart = torusSegments[i].calcPoints(points, indexStart);
			}
			
			internal virtual void  calcScreens(Point3f[] points, Point3i[] screens)
			{
				for (int i = totalPointCount; --i >= 0; )
					Enclosing_Instance.viewer.transformPoint(points[i], screens[i]);
			}
			
			internal virtual int getTorusAndGeodesicIndexes(SasCavity cavity, bool isEdgeA)
			{
				for (int i = torusCavityCount; --i >= 0; )
				{
					if (torusCavities[i].cavity == cavity)
					{
						return torusSegments[i / 2].getTorusAndGeodesicIndexes((i & 1) == 0, isEdgeA);
					}
				}
				throw new System.NullReferenceException();
			}
			
			//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'TorusSegment' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
			internal class TorusSegment
			{
				private void  InitBlock(Torus enclosingInstance)
				{
					this.enclosingInstance = enclosingInstance;
				}
				private Torus enclosingInstance;
				public Torus Enclosing_Instance
				{
					get
					{
						return enclosingInstance;
					}
					
				}
				//UPGRADE_NOTE: Final was removed from the declaration of 'startCavity '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
				internal TorusCavity startCavity;
				//UPGRADE_NOTE: Final was removed from the declaration of 'endCavity '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
				internal TorusCavity endCavity;
				internal float startAngle;
				internal float stepAngle;
				internal int stepCount; // # of vertexes, which is 1 more than the # of strips
				
				internal TorusSegment(Torus enclosingInstance)
				{
					InitBlock(enclosingInstance);
					// for a full torus
					startCavity = endCavity = null;
					this.startAngle = 0;
					this.stepAngle = Enclosing_Instance.Enclosing_Instance.TARGET_INNER_TORUS_STEP_ANGLE;
					this.stepCount = Enclosing_Instance.Enclosing_Instance.MAX_FULL_TORUS_STEP_COUNT;
					/*
					System.out.println("FullTorus\n" +
					" startAngle=" + startAngle +
					" stepAngle=" + stepAngle +
					" stepCount=" + stepCount +
					" totalSegmentAngle=" + (stepAngle*(stepCount-1)));
					*/
				}
				
				internal TorusSegment(Torus enclosingInstance, TorusCavity startCavity, TorusCavity endCavity)
				{
					InitBlock(enclosingInstance);
					this.startCavity = startCavity;
					this.endCavity = endCavity;
					this.startAngle = startCavity.angle;
					float totalSegmentAngle = endCavity.angle - startAngle;
					/*
					System.out.println(" startAngle=" + startAngle +
					" endAngle=" + endAngle +
					" totalSegmentAngle=" + totalSegmentAngle);
					*/
					if (totalSegmentAngle < 0)
						totalSegmentAngle += 2 * org.jmol.viewer.Sasurface1.PI;
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					stepCount = (int) (totalSegmentAngle / Enclosing_Instance.Enclosing_Instance.TARGET_INNER_TORUS_STEP_ANGLE);
					stepAngle = totalSegmentAngle / stepCount;
					++stepCount; // one more strip than pieces of the segment
				}
				
				internal virtual void  dumpStuff()
				{
					System.Console.Out.Write(" start ixA=" + Enclosing_Instance.ixA + " ixB=" + Enclosing_Instance.ixB);
					startCavity.dumpStuff();
					System.Console.Out.Write("   end ixA=" + Enclosing_Instance.ixA + " ixB=" + Enclosing_Instance.ixB);
					endCavity.dumpStuff();
				}
				
				internal virtual int calcPoints(Point3f[] points, int ixPoint)
				{
					Enclosing_Instance.Enclosing_Instance.aaT.set_Renamed(Enclosing_Instance.axisUnitVector, startAngle);
					for (int i = stepCount; --i >= 0; Enclosing_Instance.Enclosing_Instance.aaT.angle += stepAngle)
					{
						Enclosing_Instance.Enclosing_Instance.matrixT.set_Renamed(Enclosing_Instance.Enclosing_Instance.aaT);
						Enclosing_Instance.Enclosing_Instance.matrixT.transform(Enclosing_Instance.radialVector, Enclosing_Instance.Enclosing_Instance.pointT);
						Enclosing_Instance.Enclosing_Instance.pointT.add(Enclosing_Instance.center);
						for (int j = 0; j < Enclosing_Instance.outerPointCount; ++j, ++ixPoint)
						{
							Enclosing_Instance.Enclosing_Instance.matrixT.transform(Enclosing_Instance.Enclosing_Instance.outerRadials[j], Enclosing_Instance.Enclosing_Instance.vectorT);
							points[ixPoint].add(Enclosing_Instance.Enclosing_Instance.pointT, Enclosing_Instance.Enclosing_Instance.vectorT);
							//System.out.println("  calcPoints[" + ixPoint + "]=" +
							//                 points[ixPoint]);
						}
					}
					return ixPoint;
				}
				
				internal virtual int calcNormixes(short[] normixes, int ix)
				{
					Enclosing_Instance.Enclosing_Instance.aaT.set_Renamed(Enclosing_Instance.axisUnitVector, startAngle);
					for (int i = stepCount; --i >= 0; Enclosing_Instance.Enclosing_Instance.aaT.angle += stepAngle)
					{
						Enclosing_Instance.Enclosing_Instance.matrixT.set_Renamed(Enclosing_Instance.Enclosing_Instance.aaT);
						for (int j = 0; j < Enclosing_Instance.outerPointCount; ++j, ++ix)
						{
							Enclosing_Instance.Enclosing_Instance.matrixT.transform(Enclosing_Instance.Enclosing_Instance.outerRadials[j], Enclosing_Instance.Enclosing_Instance.vectorT);
							normixes[ix] = Enclosing_Instance.Enclosing_Instance.g3d.get2SidedNormix(Enclosing_Instance.Enclosing_Instance.vectorT);
						}
					}
					return ix;
				}
				
				internal virtual void  calcEdgePoints(Point3f[] edgePoints, bool edgeA)
				{
					int outerRadialIndex;
					if (edgeA)
					{
						Enclosing_Instance.transformOuterRadials();
						outerRadialIndex = 0;
					}
					else
					{
						outerRadialIndex = Enclosing_Instance.outerPointCount - 1;
					}
					Enclosing_Instance.Enclosing_Instance.aaT.set_Renamed(Enclosing_Instance.axisUnitVector, startAngle);
					for (int i = 0; i < stepCount; Enclosing_Instance.Enclosing_Instance.aaT.angle += stepAngle, ++i)
					{
						Enclosing_Instance.Enclosing_Instance.matrixT.set_Renamed(Enclosing_Instance.Enclosing_Instance.aaT);
						Enclosing_Instance.Enclosing_Instance.matrixT.transform(Enclosing_Instance.radialVector, Enclosing_Instance.Enclosing_Instance.pointT);
						Enclosing_Instance.Enclosing_Instance.pointT.add(Enclosing_Instance.center);
						Enclosing_Instance.Enclosing_Instance.matrixT.transform(Enclosing_Instance.Enclosing_Instance.outerRadials[outerRadialIndex], Enclosing_Instance.Enclosing_Instance.vectorT);
						edgePoints[i].add(Enclosing_Instance.Enclosing_Instance.pointT, Enclosing_Instance.Enclosing_Instance.vectorT);
					}
				}
				
				internal virtual void  stitchWithSortedProjectedVertexes(bool isEdgeA, bool dump)
				{
					if (dump)
					{
						System.Console.Out.WriteLine("stitchWithSortedProjectedVertexes(isEdgeA " + isEdgeA + ")");
						System.Console.Out.WriteLine("startCavity.angle=" + startCavity.angle + " endCavity.angle=" + endCavity.angle);
						System.Console.Out.WriteLine("startAngle=" + startAngle + " stepAngle=" + stepAngle + " stepCount=" + stepCount);
						System.Console.Out.WriteLine("totalArc=" + (stepAngle * stepCount));
						System.Console.Out.WriteLine("totalArc2=" + (stepAngle * (stepCount - 1)));
					}
					Enclosing_Instance.Enclosing_Instance.gem.stitchWithTorusSegment(getSegmentStartingVertex(isEdgeA), Enclosing_Instance.outerPointCount, startAngle, stepAngle, stepCount, dump);
					if (startCavity != null)
					{
						if (isEdgeA)
						{
							startCavity.geodesicVertexA = Enclosing_Instance.Enclosing_Instance.gem.firstStitchedGeodesicVertex;
							endCavity.geodesicVertexA = Enclosing_Instance.Enclosing_Instance.gem.lastStitchedGeodesicVertex;
						}
						else
						{
							startCavity.geodesicVertexB = Enclosing_Instance.Enclosing_Instance.gem.firstStitchedGeodesicVertex;
							endCavity.geodesicVertexB = Enclosing_Instance.Enclosing_Instance.gem.lastStitchedGeodesicVertex;
						}
					}
				}
				
				internal virtual short getSegmentStartingVertex(bool isEdgeA)
				{
					int totalStepCount = 0;
					for (int i = 0; i < Enclosing_Instance.torusSegmentCount; ++i)
					{
						TorusSegment segment = Enclosing_Instance.torusSegments[i];
						if (segment == this)
						{
							int startingVertex = totalStepCount * Enclosing_Instance.outerPointCount;
							if (!isEdgeA)
							{
								/*
								System.out.println(" -------- I am not edge A! ---------");
								System.out.println("   startingVertex=" + startingVertex +
								"   outerPointCount=" + outerPointCount);
								*/
								startingVertex += Enclosing_Instance.outerPointCount - 1;
								/*
								System.out.println("   after startingVertex=" + startingVertex);
								*/
							}
							return (short) startingVertex;
						}
						totalStepCount += segment.stepCount;
					}
					System.Console.Out.WriteLine("torus segment not found in torus");
					throw new System.NullReferenceException();
				}
				
				internal virtual int getTorusAndGeodesicIndexes(bool isBeginning, bool isEdgeA)
				{
					short torusVertex = getSegmentStartingVertex(isEdgeA);
					if (!isBeginning)
						torusVertex = (short) (torusVertex + (stepCount - 1) * Enclosing_Instance.outerPointCount);
					short geodesicVertex = 0;
					return (torusVertex << 16) | geodesicVertex;
				}
			}
			
			internal virtual void  clipVertexMaps()
			{
				clipVertexMap(true);
				clipVertexMap(false);
			}
			
			internal virtual void  clipVertexMap(bool isEdgeA)
			{
				int ix = isEdgeA?ixA:ixB;
				Atom atom = Enclosing_Instance.frame.atoms[ix];
				calcZeroPoint(isEdgeA, Enclosing_Instance.zeroPointT);
				Enclosing_Instance.gem.clipGeodesic(isEdgeA, atom.point3f, atom.VanderwaalsRadiusFloat, Enclosing_Instance.zeroPointT, axisUnitVector, Enclosing_Instance.convexVertexMaps[ix]);
			}
			
			internal virtual void  calcZeroPoint(bool edgeA, Point3f zeroPoint)
			{
				Vector3f t;
				if (edgeA)
				{
					t = outerRadial;
				}
				else
				{
					Enclosing_Instance.aaT.set_Renamed(tangentVector, outerAngle);
					Enclosing_Instance.matrixT.set_Renamed(Enclosing_Instance.aaT);
					Enclosing_Instance.matrixT.transform(outerRadial, Enclosing_Instance.vectorT);
					t = Enclosing_Instance.vectorT;
				}
				zeroPoint.add(center, radialVector);
				zeroPoint.add(t);
			}
			
			internal virtual void  calcZeroAndCenterPoints(bool edgeA, Point3f atomCenter, Point3f zeroPoint, Point3f centerPoint)
			{
				calcZeroPoint(edgeA, zeroPoint);
				calcClippingPlaneCenter(atomCenter, axisUnitVector, zeroPoint, centerPoint);
			}
			
			internal virtual void  calcClippingPlaneCenterPoints(Point3f centerPointA, Point3f centerPointB)
			{
				calcZeroPoint(true, Enclosing_Instance.zeroPointT);
				Point3f centerA = Enclosing_Instance.frame.atoms[ixA].point3f;
				calcClippingPlaneCenter(centerA, axisUnitVector, Enclosing_Instance.zeroPointT, centerPointA);
				
				calcZeroPoint(false, Enclosing_Instance.zeroPointT);
				Point3f centerB = Enclosing_Instance.frame.atoms[ixB].point3f;
				calcClippingPlaneCenter(centerB, axisUnitVector, Enclosing_Instance.zeroPointT, centerPointB);
			}
			
			internal virtual void  stitchWithGeodesics()
			{
				//      System.out.println("torus.stitchWithGeodesics()");
				stitchWithGeodesic(true);
				stitchWithGeodesic(false);
				/*
				if (ixA == 0 && ixB == 1) {
				System.out.println("seam ixA=" + ixA + " ixB=" + ixB);
				System.out.println(" seamA:");
				if (seamA != null)
				gem.decodeSeam(seamA);
				System.out.println(" seamB:");
				if (seamA != null)
				gem.decodeSeam(seamB);
				}
				*/
			}
			
			internal virtual void  stitchWithGeodesic(bool isEdgeA)
			{
				int ix = isEdgeA?ixA:ixB;
				Atom atom = Enclosing_Instance.frame.atoms[ix];
				float atomRadius = atom.VanderwaalsRadiusFloat;
				Point3f atomCenter = atom.point3f;
				Enclosing_Instance.gem.reset();
				calcZeroAndCenterPoints(isEdgeA, atomCenter, Enclosing_Instance.zeroPointT, Enclosing_Instance.centerPointT);
				bool dump = false;
				//      dump = (ixA == 0 && ixB == 1);
				if (Enclosing_Instance.gem.projectAndSortGeodesicPoints(isEdgeA, atomCenter, atomRadius, Enclosing_Instance.centerPointT, axisUnitVector, Enclosing_Instance.zeroPointT, fullTorus, Enclosing_Instance.convexVertexMaps[ix], dump))
					stitchSegmentsWithSortedProjectedVertexes(isEdgeA);
			}
			
			internal virtual void  stitchSegmentsWithSortedProjectedVertexes(bool isEdgeA)
			{
				bool dump = false;
				//      dump = (ixA == 0 && ixB == 1);
				for (int i = torusSegmentCount; --i >= 0; )
					torusSegments[i].stitchWithSortedProjectedVertexes(isEdgeA, dump);
				short[] seam = Enclosing_Instance.gem.createSeam();
				if (isEdgeA)
					seamA = seam;
				else
					seamB = seam;
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'TorusCavity' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		//UPGRADE_NOTE: The access modifier for this class or class field has been changed in order to prevent compilation errors due to the visibility level. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1296'"
		internal class TorusCavity
		{
			private void  InitBlock(Sasurface1 enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Sasurface1 enclosingInstance;
			public Sasurface1 Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			//UPGRADE_NOTE: Final was removed from the declaration of 'cavity '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
			internal SasCavity cavity;
			//UPGRADE_NOTE: Final was removed from the declaration of 'rightHanded '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
			internal bool rightHanded;
			internal float angle = 0;
			internal short geodesicVertexA = - 1;
			internal short geodesicVertexB = - 1;
			
			internal TorusCavity(Sasurface1 enclosingInstance, SasCavity cavity, bool rightHanded)
			{
				InitBlock(enclosingInstance);
				this.cavity = cavity;
				this.rightHanded = rightHanded;
			}
			
			internal virtual void  dumpStuff()
			{
				System.Console.Out.WriteLine(" geodesicVertexA=" + geodesicVertexA + " geodesicVertexB=" + geodesicVertexB);
			}
			
			internal virtual void  calcAngle(Point3f center, Vector3f radialVector, Vector3f radialVector90)
			{
				Enclosing_Instance.torusCavityAngleVectorT.sub(cavity.probeCenter, center);
				angle = Enclosing_Instance.torusCavityAngleVectorT.angle(radialVector);
				float angleCavity90 = Enclosing_Instance.torusCavityAngleVectorT.angle(radialVector90);
				if (angleCavity90 <= org.jmol.viewer.Sasurface1.PI / 2)
					return ;
				angle = (2 * org.jmol.viewer.Sasurface1.PI) - angle;
			}
		}
	}
}