/* $RCSfile$
* $Author: hansonr $
* $Date: 2006-04-05 04:50:43 +0200 (mer., 05 avr. 2006) $
* $Revision: 4913 $
*
* Copyright (C) 2004-2006  The Jmol Development Team
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
//UPGRADE_TODO: The type 'javax.vecmath.Vector3f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Vector3f = javax.vecmath.Vector3f;
using Graphics3D = org.jmol.g3d.Graphics3D;
namespace org.jmol.viewer
{
	
	class Polyhedra:SelectionIndependentShape
	{
		public Polyhedra()
		{
			InitBlock();
		}
		private void  InitBlock()
		{
			polyhedrons = new Polyhedron[32];
			for (int i = collapsedCentersT.length; --i >= 0; )
				collapsedCentersT[i] = new Point3f();
		}
		
		internal const float DEFAULT_CENTRAL_ANGLE_MAX = 145f / 180f * 3.1415926f;
		internal const float DEFAULT_FACE_NORMAL_MAX = 30f / 180f * 3.1415926f;
		internal const float DEFAULT_FACE_CENTER_OFFSET = 0.25f;
		internal const int EDGES_NONE = 0;
		internal const int EDGES_ALL = 1;
		internal const int EDGES_FRONT = 2;
		
		// Bob, please set these to reasonable values
		internal const int MINIMUM_ACCEPTABLE_VERTEX_COUNT = 3;
		internal const int MAXIMUM_ACCEPTABLE_VERTEX_COUNT = 20;
		internal const int FACE_COUNT_MAX = 85;
		
		internal const bool debugging = false;
		
		internal int polyhedronCount;
		//UPGRADE_NOTE: The initialization of  'polyhedrons' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		internal Polyhedron[] polyhedrons;
		internal float radius;
		internal int acceptableVertexCountCount = 0;
		// assume that 8 is enough ... if you need more just make this array bigger
		internal int[] acceptableVertexCounts = new int[8];
		internal float faceCenterOffset;
		internal float centralAngleMax;
		internal float faceNormalMax;
		internal int drawEdges;
		
		internal bool isCollapsed;
		
		internal System.Collections.BitArray bsCenters;
		internal System.Collections.BitArray bsVertices;
		
		internal override void  initShape()
		{
		}
		
		internal override void  setProperty(System.String propertyName, System.Object value_Renamed, System.Collections.BitArray bs)
		{
			//System.out.println(propertyName + " "+ value);
			if ((System.Object) "init" == (System.Object) propertyName)
			{
				faceCenterOffset = DEFAULT_FACE_CENTER_OFFSET;
				centralAngleMax = DEFAULT_CENTRAL_ANGLE_MAX;
				faceNormalMax = DEFAULT_FACE_NORMAL_MAX;
				radius = 0.0f;
				acceptableVertexCountCount = 0;
				bsCenters = bsVertices = null;
				isCollapsed = false;
				drawEdges = EDGES_NONE;
				return ;
			}
			if ((System.Object) "radius" == (System.Object) propertyName)
			{
				if (value_Renamed is System.Single)
				{
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					radius = (float) ((System.Single) value_Renamed);
				}
				else
					invalidPropertyType(propertyName, value_Renamed, "Float");
				return ;
			}
			if ((System.Object) "bonds" == (System.Object) propertyName)
			{
				radius = 0; // radius == 0 is the flag for using bonds
				return ;
			}
			if ((System.Object) "vertexCount" == (System.Object) propertyName)
			{
				if (value_Renamed is System.Int32)
				{
					if (acceptableVertexCountCount < acceptableVertexCounts.Length)
					{
						int vertexCount = ((System.Int32) value_Renamed);
						if (vertexCount >= MINIMUM_ACCEPTABLE_VERTEX_COUNT && vertexCount <= MAXIMUM_ACCEPTABLE_VERTEX_COUNT)
							acceptableVertexCounts[acceptableVertexCountCount++] = vertexCount;
					}
				}
				else
				{
					invalidPropertyType(propertyName, value_Renamed, "Integer");
				}
				return ;
			}
			if ((System.Object) "potentialCenterSet" == (System.Object) propertyName)
			{
				if (value_Renamed is System.Collections.BitArray)
					bsCenters = (System.Collections.BitArray) value_Renamed;
				else
					invalidPropertyType(propertyName, value_Renamed, "BitSet");
				return ;
			}
			if ((System.Object) "potentialVertexSet" == (System.Object) propertyName)
			{
				if (value_Renamed is System.Collections.BitArray)
					bsVertices = (System.Collections.BitArray) value_Renamed;
				else
					invalidPropertyType(propertyName, value_Renamed, "BitSet");
				return ;
			}
			if ((System.Object) "faceCenterOffset" == (System.Object) propertyName)
			{
				if (value_Renamed is System.Single)
				{
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					faceCenterOffset = (float) ((System.Single) value_Renamed);
				}
				else
					invalidPropertyType(propertyName, value_Renamed, "Float");
				return ;
			}
			if ((System.Object) "centerAngleMax" == (System.Object) propertyName)
			{
				if (value_Renamed is System.Single)
				{
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					centralAngleMax = ((float) ((System.Single) value_Renamed)) / 180f * 3.1415926f;
				}
				else
					invalidPropertyType(propertyName, value_Renamed, "Float");
				return ;
			}
			if ((System.Object) "faceNormalMax" == (System.Object) propertyName)
			{
				if (value_Renamed is System.Single)
				{
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					faceNormalMax = ((float) ((System.Single) value_Renamed)) / 180f * 3.1415926f;
				}
				else
					invalidPropertyType(propertyName, value_Renamed, "Float");
				return ;
			}
			if ((System.Object) "generate" == (System.Object) propertyName)
			{
				if (bsCenters == null)
					bsCenters = bs;
				buildPolyhedra();
				return ;
			}
			if ((System.Object) "collapsed" == (System.Object) propertyName)
			{
				if (bsCenters == null)
					bsCenters = bs;
				isCollapsed = value_Renamed == (System.Object) true;
				setCollapsed(isCollapsed, bsCenters);
				return ;
			}
			if ((System.Object) "delete" == (System.Object) propertyName)
			{
				if (bsCenters == null)
					bsCenters = bs;
				deletePolyhedra(bsCenters);
				return ;
			}
			if ((System.Object) "on" == (System.Object) propertyName)
			{
				if (bsCenters == null)
					bsCenters = bs;
				setVisible(true, bsCenters);
				return ;
			}
			if ((System.Object) "off" == (System.Object) propertyName)
			{
				if (bsCenters == null)
					bsCenters = bs;
				setVisible(false, bsCenters);
				return ;
			}
			if ((System.Object) "noedges" == (System.Object) propertyName)
			{
				drawEdges = EDGES_NONE;
				if (bsCenters == null)
					bsCenters = bs;
				setEdges(drawEdges, bsCenters);
				return ;
			}
			if ((System.Object) "edges" == (System.Object) propertyName)
			{
				drawEdges = EDGES_ALL;
				if (bsCenters == null)
					bsCenters = bs;
				setEdges(drawEdges, bsCenters);
				return ;
			}
			if ((System.Object) "frontedges" == (System.Object) propertyName)
			{
				drawEdges = EDGES_FRONT;
				if (bsCenters == null)
					bsCenters = bs;
				setEdges(drawEdges, bsCenters);
				return ;
			}
			if ((System.Object) "color" == (System.Object) propertyName)
			{
				// remember that this comes from 'color' command, so bsCenters is not set
				colix = Graphics3D.getColix(value_Renamed);
				setColix(colix, ((colix != Graphics3D.UNRECOGNIZED)?null:(System.String) value_Renamed), bs);
				return ;
			}
			if ((System.Object) "translucency" == (System.Object) propertyName)
			{
				// remember that this comes from 'color' command, so use bs not bsCenters
				setTranslucent((System.Object) "translucent" == value_Renamed, bs);
				return ;
			}
			base.setProperty(propertyName, value_Renamed, bs);
		}
		
		internal virtual void  deletePolyhedra(System.Collections.BitArray bs)
		{
			int newCount = 0;
			for (int i = 0; i < polyhedronCount; ++i)
			{
				Polyhedron p = polyhedrons[i];
				if (!bs.Get(p.centralAtom.atomIndex))
					polyhedrons[newCount++] = p;
			}
			for (int i = newCount; i < polyhedronCount; ++i)
				polyhedrons[i] = null;
			polyhedronCount = newCount;
		}
		
		internal virtual void  setVisible(bool visible, System.Collections.BitArray bs)
		{
			for (int i = polyhedronCount; --i >= 0; )
			{
				Polyhedron p = polyhedrons[i];
				if (p == null)
					continue;
				if (bs.Get(p.centralAtom.atomIndex))
					p.visible = visible;
			}
		}
		
		internal virtual void  setEdges(int edges, System.Collections.BitArray bs)
		{
			for (int i = polyhedronCount; --i >= 0; )
			{
				Polyhedron p = polyhedrons[i];
				if (p == null)
					continue;
				if (bs.Get(p.centralAtom.atomIndex))
					p.edges = edges;
			}
		}
		
		internal virtual void  setCollapsed(bool isCollapsed, System.Collections.BitArray bs)
		{
			for (int i = polyhedronCount; --i >= 0; )
			{
				Polyhedron p = polyhedrons[i];
				if (p == null)
					continue;
				if (bs.Get(p.centralAtom.atomIndex))
					p.collapsed = isCollapsed;
			}
		}
		
		internal virtual void  setColix(short colix, System.String palette, System.Collections.BitArray bs)
		{
			for (int i = polyhedronCount; --i >= 0; )
			{
				Polyhedron p = polyhedrons[i];
				if (p == null)
					continue;
				int atomIndex = p.centralAtom.atomIndex;
				if (bs.Get(atomIndex))
					p.polyhedronColix = ((colix != Graphics3D.UNRECOGNIZED)?colix:viewer.getColixAtomPalette(frame.getAtomAt(atomIndex), palette));
			}
		}
		
		internal virtual void  setTranslucent(bool isTranslucent, System.Collections.BitArray bs)
		{
			for (int i = polyhedronCount; --i >= 0; )
			{
				Polyhedron p = polyhedrons[i];
				if (p == null)
					continue;
				if (bs.Get(p.centralAtom.atomIndex))
					p.polyhedronColix = Graphics3D.setTranslucent(p.polyhedronColix, isTranslucent);
			}
		}
		
		internal virtual void  savePolyhedron(Polyhedron p)
		{
			// overwrite similar polyhedrons
			for (int i = polyhedronCount; --i >= 0; )
			{
				if (p.isSimilarEnoughToDelete(polyhedrons[i]))
				{
					polyhedrons[i] = p;
					return ;
				}
			}
			if (polyhedronCount == polyhedrons.Length)
				polyhedrons = (Polyhedron[]) Util.doubleLength(polyhedrons);
			polyhedrons[polyhedronCount++] = p;
		}
		
		internal virtual void  buildPolyhedra()
		{
			for (int i = frame.atomCount; --i >= 0; )
			{
				if (bsCenters.Get(i))
				{
					Polyhedron p = constructPolyhedron(i);
					if (p != null)
						savePolyhedron(p);
				}
			}
		}
		
		internal int potentialVertexCount;
		internal Atom[] potentialVertexAtoms = new Atom[MAXIMUM_ACCEPTABLE_VERTEX_COUNT];
		
		internal virtual Polyhedron constructPolyhedron(int atomIndex)
		{
			Atom atom = frame.getAtomAt(atomIndex);
			if (radius > 0)
				identifyPotentialRadiusVertices(atom);
			else
				identifyPotentialBondsVertices(atom);
			if (acceptableVertexCountCount == 0)
				return validatePolyhedronNew(atom, potentialVertexCount, potentialVertexAtoms);
			if (potentialVertexCount >= MINIMUM_ACCEPTABLE_VERTEX_COUNT)
			{
				for (int i = acceptableVertexCountCount; --i >= 0; )
				{
					if (potentialVertexCount == acceptableVertexCounts[i])
					{
						return validatePolyhedronNew(atom, potentialVertexCount, potentialVertexAtoms);
					}
				}
			}
			return null;
		}
		
		internal virtual void  identifyPotentialBondsVertices(Atom atom)
		{
			potentialVertexCount = 0;
			Bond[] bonds = atom.bonds;
			if (bonds == null)
				return ;
			for (int i = bonds.Length; --i >= 0; )
			{
				Bond bond = bonds[i];
				Atom otherAtom = bond.atom1 == atom?bond.atom2:bond.atom1;
				if (bsVertices != null && !bsVertices.Get(otherAtom.atomIndex))
					continue;
				if (potentialVertexCount == potentialVertexAtoms.Length)
					break;
				potentialVertexAtoms[potentialVertexCount++] = otherAtom;
			}
		}
		
		internal virtual void  identifyPotentialRadiusVertices(Atom atom)
		{
			potentialVertexCount = 0;
			AtomIterator withinIterator = frame.getWithinModelIterator(atom, radius);
			while (withinIterator.hasNext())
			{
				Atom otherAtom = withinIterator.next();
				if (otherAtom == atom || bsVertices != null && !bsVertices.Get(otherAtom.atomIndex))
					continue;
				if (potentialVertexCount == potentialVertexAtoms.Length)
					break;
				potentialVertexAtoms[potentialVertexCount++] = otherAtom;
			}
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'normalT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private Vector3f normalT = new Vector3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'normixesT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal short[] normixesT = new short[FACE_COUNT_MAX];
		//UPGRADE_NOTE: Final was removed from the declaration of 'planesT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal sbyte[] planesT = new sbyte[3 * FACE_COUNT_MAX];
		//UPGRADE_NOTE: Final was removed from the declaration of 'collapsedNormixesT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal short[] collapsedNormixesT = new short[3 * FACE_COUNT_MAX];
		//UPGRADE_NOTE: Final was removed from the declaration of 'collapsedCentersT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Point3f[] collapsedCentersT = new Point3f[FACE_COUNT_MAX];
		//UPGRADE_NOTE: Final was removed from the declaration of 'centerVectors '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f[] centerVectors = new Vector3f[3 * FACE_COUNT_MAX];
		//UPGRADE_NOTE: Final was removed from the declaration of 'centerSum '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private Vector3f centerSum = new Vector3f();
		
		internal virtual Polyhedron validatePolyhedronNew(Atom centralAtom, int vertexCount, Atom[] otherAtoms)
		{
			int faceCount = 0;
			Point3f centralAtomPoint = centralAtom.point3f;
			for (int i = vertexCount; --i >= 0; )
			{
				centerVectors[i] = new Vector3f(otherAtoms[i].point3f);
				centerVectors[i].sub(centralAtomPoint);
			}
			
			// simply define a face to be when all three central angles 
			// are < centralAngleMax
			// collapsed trick is that introduce a "simple" atom
			// near the center but not quite the center, so that our planes on
			// either side of the facet don't overlap. We step out maxFactor * normal
			
			// also needed: consideration for faces involving more than three atoms
			for (int i = 0; i < vertexCount - 2; i++)
			{
				for (int j = i + 1; j < vertexCount - 1; j++)
				{
					if (centerVectors[i].angle(centerVectors[j]) > centralAngleMax)
						continue;
					for (int k = j + 1; k < vertexCount; k++)
					{
						if (centerVectors[i].angle(centerVectors[k]) > centralAngleMax || centerVectors[j].angle(centerVectors[k]) > centralAngleMax)
							continue;
						Point3f pointI = otherAtoms[i].point3f;
						Point3f pointJ = otherAtoms[j].point3f;
						Point3f pointK = otherAtoms[k].point3f;
						getNormalFromCenter(centralAtomPoint, pointI, pointJ, pointK, false, normalT);
						
						centerSum.add(centerVectors[i], centerVectors[j]);
						centerSum.add(centerVectors[k]);
						if (debugging)
						{
							System.Console.Out.WriteLine("excluding? " + otherAtoms[i].Info + otherAtoms[j].Info + otherAtoms[k].Info);
							System.Console.Out.WriteLine("excluding? " + normalT + "\n" + centerSum + "\n" + (centerSum.angle(normalT) / 3.1415926f * 180f));
						}
						if (centerSum.angle(normalT) > faceNormalMax)
						{
							if (debugging)
								System.Console.Out.WriteLine("yes");
							continue;
						}
						if (debugging)
							System.Console.Out.WriteLine("no -- passes");
						
						planesT[3 * faceCount + 0] = (sbyte) i;
						planesT[3 * faceCount + 1] = (sbyte) j;
						planesT[3 * faceCount + 2] = (sbyte) k;
						normixesT[faceCount] = g3d.getNormix(normalT);
						
						// calculate collapsed faces too
						Point3f collapsedCenter = collapsedCentersT[faceCount];
						collapsedCenter.scaleAdd(faceCenterOffset, normalT, centralAtomPoint);
						getNormalFromCenter(pointI, collapsedCenter, pointJ, pointK, true, normalT);
						collapsedNormixesT[3 * faceCount + 0] = g3d.getNormix(normalT);
						
						getNormalFromCenter(pointJ, pointI, collapsedCenter, pointK, true, normalT);
						collapsedNormixesT[3 * faceCount + 1] = g3d.getNormix(normalT);
						
						getNormalFromCenter(pointK, pointI, pointJ, collapsedCenter, true, normalT);
						collapsedNormixesT[3 * faceCount + 2] = g3d.getNormix(normalT);
						
						if (++faceCount == FACE_COUNT_MAX)
						{
							//UPGRADE_NOTE: Labeled break statement was changed to a goto statement. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1012'"
							goto out_brk;
						}
					}
				}
			}
			//UPGRADE_NOTE: Label 'out_brk' was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1011'"
out_brk: ;
			
			if (faceCount < 1)
				return null;
			return new Polyhedron(centralAtom, vertexCount, otherAtoms, faceCount, normixesT, planesT, collapsedCentersT, collapsedNormixesT);
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'ptT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private Point3f ptT = new Point3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'ptT2 '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private Point3f ptT2 = new Point3f();
		// note: this shared vector3f is returned and used by callers
		
		internal virtual void  getNormalFromCenter(Point3f ptCenter, Point3f ptA, Point3f ptB, Point3f ptC, bool isCollapsed, Vector3f normal)
		{
			//but which way is it? add N to A and see who is closer to Center, A or N. 
			g3d.calcNormalizedNormal(ptA, ptB, ptC, normal); //still need normal
			ptT.add(ptA, ptB);
			ptT.add(ptC);
			ptT.scale(1 / 3f);
			ptT2.set_Renamed(normal);
			ptT2.scale(0.1f);
			ptT2.add(ptT);
			//              A      C
			//                \   /
			//                 \ / 
			//                  x pT is center of ABC; ptT2 is offset a bit from that
			//                  |    either closer to x (ok if not opaque) or further
			//                  |    from x (ok if opaque)
			//                  B
			// in the case of facet ABx, the "center" is really the OTHER point, C.
			if (!isCollapsed && ptCenter.distance(ptT2) < ptCenter.distance(ptT) || isCollapsed && ptCenter.distance(ptT) < ptCenter.distance(ptT2))
				normal.scale(- 1f);
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'Polyhedron' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		//UPGRADE_NOTE: The access modifier for this class or class field has been changed in order to prevent compilation errors due to the visibility level. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1296'"
		internal class Polyhedron
		{
			private void  InitBlock(Polyhedra enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Polyhedra enclosingInstance;
			public Polyhedra Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			//UPGRADE_NOTE: Final was removed from the declaration of 'centralAtom '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
			internal Atom centralAtom;
			//UPGRADE_NOTE: Final was removed from the declaration of 'vertexCount '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
			internal int vertexCount;
			//UPGRADE_NOTE: Final was removed from the declaration of 'vertexAtoms '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
			internal Atom[] vertexAtoms;
			//UPGRADE_NOTE: Final was removed from the declaration of 'faceCount '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
			internal int faceCount;
			//UPGRADE_NOTE: Final was removed from the declaration of 'normixes '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
			internal short[] normixes;
			//UPGRADE_NOTE: Final was removed from the declaration of 'planes '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
			internal sbyte[] planes;
			//UPGRADE_NOTE: Final was removed from the declaration of 'collapsedCenters '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
			internal Point3f[] collapsedCenters;
			//UPGRADE_NOTE: Final was removed from the declaration of 'collapsedNormixes '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
			internal short[] collapsedNormixes;
			internal bool visible;
			internal short polyhedronColix;
			internal bool collapsed;
			internal int edges;
			
			internal Polyhedron(Polyhedra enclosingInstance, Atom centralAtom, int vertexCount, Atom[] vertexAtoms, int faceCount, short[] normixes, sbyte[] planes, Point3f[] collapsedCenters, short[] collapsedNormixes)
			{
				InitBlock(enclosingInstance);
				System.Console.Out.WriteLine("new Polyhedron vertexCount = " + vertexCount + ";" + " faceCount = " + faceCount);
				this.centralAtom = centralAtom;
				this.vertexCount = vertexCount;
				this.vertexAtoms = new Atom[vertexCount];
				for (int i = vertexCount; --i >= 0; )
					this.vertexAtoms[i] = vertexAtoms[i];
				
				this.faceCount = faceCount;
				this.normixes = new short[faceCount];
				this.collapsedCenters = new Point3f[faceCount];
				for (int i = faceCount; --i >= 0; )
				{
					this.normixes[i] = normixes[i];
					this.collapsedCenters[i] = new Point3f(collapsedCenters[i]);
				}
				
				this.planes = new sbyte[faceCount * 3];
				this.collapsedNormixes = new short[faceCount * 3];
				for (int i = faceCount * 3; --i >= 0; )
				{
					this.planes[i] = planes[i];
					this.collapsedNormixes[i] = collapsedNormixes[i];
				}
				
				this.visible = true;
				this.polyhedronColix = 0; // always create with default of 'inherit'
				this.collapsed = Enclosing_Instance.isCollapsed;
				this.edges = Enclosing_Instance.drawEdges;
			}
			
			internal virtual bool isSimilarEnoughToDelete(Polyhedron p)
			{
				return centralAtom == p.centralAtom && faceCount == p.faceCount;
			}
		}
	}
}