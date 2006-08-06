/* $RCSfile$
* $Author: hansonr $
* $Date: 2006-03-12 22:37:51 -0500 (Sun, 12 Mar 2006) $
* $Revision: 4586 $
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
//UPGRADE_TODO: The type 'javax.vecmath.Vector3f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Vector3f = javax.vecmath.Vector3f;
//UPGRADE_TODO: The type 'javax.vecmath.Matrix3f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Matrix3f = javax.vecmath.Matrix3f;
namespace org.jmol.g3d
{
	
	/// <summary> Provides quantization of normalized vectors so that shading for
	/// lighting calculations can be handled by a simple index lookup
	/// <p>
	/// A 'normix' is a normal index, represented as a short
	/// 
	/// </summary>
	/// <author>  Miguel, miguel@jmol.org
	/// </author>
	class Normix3D
	{
		virtual internal Matrix3f RotationMatrix
		{
			set
			{
				this.rotationMatrix.set_Renamed(value);
				for (int i = normixCount; --i >= 0; )
				{
					Vector3f tv = transformedVectors[i];
					value.transform(Geodesic3D.vertexVectors[i], tv);
					float x = tv.x;
					float y = - tv.y;
					float z = tv.z;
					/*
					enable this code in order to allow
					lighting of the inside of surfaces.
					but they probably should not be specular
					and light source should be from another position ... like a headlamp
					
					if (z < 0) {
					x = -x;
					y = -y;
					z = -z;
					}
					*/
					sbyte intensity = Shade3D.calcIntensityNormalized(x, y, z);
					intensities[i] = intensity;
					if (z >= 0)
						intensities2Sided[i] = intensity;
					else
						intensities2Sided[i] = Shade3D.calcIntensityNormalized(- x, - y, - z);
				}
			}
			
		}
		virtual internal Vector3f[] TransformedVectors
		{
			get
			{
				return transformedVectors;
			}
			
		}
		
		internal const int NORMIX_GEODESIC_LEVEL = 3;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'g3d '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Graphics3D g3d;
		//UPGRADE_NOTE: Final was removed from the declaration of 'transformedVectors '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f[] transformedVectors;
		//UPGRADE_NOTE: Final was removed from the declaration of 'intensities '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal sbyte[] intensities;
		//UPGRADE_NOTE: Final was removed from the declaration of 'intensities2Sided '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal sbyte[] intensities2Sided;
		//UPGRADE_NOTE: Final was removed from the declaration of 'normixCount '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal int normixCount;
		//UPGRADE_NOTE: Final was removed from the declaration of 'vertexCounts'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int[] vertexCounts = new int[]{12, 42, 162, 642, 2562};
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'faceNormixesArrays '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly short[][] faceNormixesArrays = new short[NORMIX_GEODESIC_LEVEL + 1][];
		
		private const bool TIMINGS = false;
		private const bool DEBUG_WITH_SEQUENTIAL_SEARCH = true;
		
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'rotationMatrix '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private Matrix3f rotationMatrix = new Matrix3f();
		
		internal Normix3D(Graphics3D g3d)
		{
			// 12, 42, 162, 642, 2562
			this.g3d = g3d;
			normixCount = Geodesic3D.getVertexCount(NORMIX_GEODESIC_LEVEL);
			intensities = new sbyte[normixCount];
			intensities2Sided = new sbyte[normixCount];
			transformedVectors = new Vector3f[normixCount];
			for (int i = normixCount; --i >= 0; )
				transformedVectors[i] = new Vector3f();
			
			if (TIMINGS)
			{
				System.Console.Out.WriteLine("begin timings!");
				for (int i = 0; i < normixCount; ++i)
				{
					short normix = getNormix(Geodesic3D.vertexVectors[i]);
					if (normix != i)
						System.Console.Out.WriteLine("" + i + " -> " + normix);
				}
				short[] neighborVertexes = Geodesic3D.neighborVertexesArrays[NORMIX_GEODESIC_LEVEL];
				
				System.Random rand = new System.Random();
				Vector3f vFoo = new Vector3f();
				Vector3f vBar = new Vector3f();
				Vector3f vSum = new Vector3f();
				
				int runCount = 100000;
				long timeBegin, runTime;
				
				timeBegin = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
				for (int i = 0; i < runCount; ++i)
				{
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					short foo = (short) (rand.NextDouble() * normixCount);
					int offsetNeighbor;
					short bar;
					do 
					{
						//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
						offsetNeighbor = foo * 6 + (int) (rand.NextDouble() * 6);
						bar = neighborVertexes[offsetNeighbor];
					}
					while (bar == - 1);
					vFoo.set_Renamed(Geodesic3D.vertexVectors[foo]);
					vFoo.scale((float) rand.NextDouble());
					vBar.set_Renamed(Geodesic3D.vertexVectors[bar]);
					vBar.scale((float) rand.NextDouble());
					vSum.add(vFoo, vBar);
					vSum.normalize();
				}
				runTime = (System.DateTime.Now.Ticks - 621355968000000000) / 10000 - timeBegin;
				System.Console.Out.WriteLine("base runtime for " + runCount + " -> " + runTime + " ms");
				
				timeBegin = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
				for (int i = 0; i < runCount; ++i)
				{
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					short foo = (short) (rand.NextDouble() * normixCount);
					int offsetNeighbor;
					short bar;
					do 
					{
						//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
						offsetNeighbor = foo * 6 + (int) (rand.NextDouble() * 6);
						bar = neighborVertexes[offsetNeighbor];
					}
					while (bar == - 1);
					vFoo.set_Renamed(Geodesic3D.vertexVectors[foo]);
					vFoo.scale((float) rand.NextDouble());
					vBar.set_Renamed(Geodesic3D.vertexVectors[bar]);
					vBar.scale((float) rand.NextDouble());
					vSum.add(vFoo, vBar);
					short sum = getNormix(vSum);
					if (sum != foo && sum != bar)
					{
						System.Console.Out.WriteLine("foo:" + foo + " -> " + Geodesic3D.vertexVectors[foo] + "\n" + "bar:" + bar + " -> " + Geodesic3D.vertexVectors[bar] + "\n" + "sum:" + sum + " -> " + Geodesic3D.vertexVectors[sum] + "\n" + "foo.dist=" + dist2(vSum, Geodesic3D.vertexVectors[foo]) + "\n" + "bar.dist=" + dist2(vSum, Geodesic3D.vertexVectors[bar]) + "\n" + "sum.dist=" + dist2(vSum, Geodesic3D.vertexVectors[sum]) + "\n" + "\nvSum:" + vSum + "\n");
						throw new System.NullReferenceException();
					}
					short sum2 = getNormix(vSum);
					if (sum != sum2)
					{
						System.Console.Out.WriteLine("normalized not the same answer?");
						throw new System.NullReferenceException();
					}
				}
				runTime = (System.DateTime.Now.Ticks - 621355968000000000) / 10000 - timeBegin;
				System.Console.Out.WriteLine("normix2 runtime for " + runCount + " -> " + runTime + " ms");
			}
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'bsNull '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal System.Collections.BitArray bsNull = new System.Collections.BitArray(64);
		//UPGRADE_NOTE: Final was removed from the declaration of 'bsConsidered '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal System.Collections.BitArray bsConsidered = new System.Collections.BitArray(64);
		
		internal virtual short getNormix(Vector3f v)
		{
			return getNormix(v.x, v.y, v.z, NORMIX_GEODESIC_LEVEL);
		}
		
		internal virtual Vector3f getVector(short normix)
		{
			return Geodesic3D.vertexVectors[normix];
		}
		
		internal virtual short getNormix(double x, double y, double z, int geodesicLevel)
		{
			short champion;
			double t;
			if (z >= 0)
			{
				champion = 0;
				t = z - 1;
			}
			else
			{
				champion = 11;
				t = z - (- 1);
			}
			//UPGRADE_NOTE: In .NET BitArrays must be of the same size to allow the 'System.Collections.BitArray.And' operation. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1083'"
			bsConsidered.And(bsNull);
			SupportClass.BitArraySupport.Set(bsConsidered, champion);
			double championDist2 = x * x + y * y + t * t;
			for (int lvl = 0; lvl <= geodesicLevel; ++lvl)
			{
				short[] neighborVertexes = Geodesic3D.neighborVertexesArrays[lvl];
				for (int offsetNeighbors = 6 * champion, i = offsetNeighbors + (champion < 12?5:6); --i >= offsetNeighbors; )
				{
					short challenger = neighborVertexes[i];
					if (bsConsidered.Get(challenger))
						continue;
					SupportClass.BitArraySupport.Set(bsConsidered, challenger);
					//        System.out.println("challenger=" + challenger);
					Vector3f v = Geodesic3D.vertexVectors[challenger];
					double d;
					// d = dist2(v, x, y, z);
					//        System.out.println("challenger d2=" + (d*d));
					d = v.x - x;
					double d2 = d * d;
					if (d2 >= championDist2)
						continue;
					d = v.y - y;
					d2 += d * d;
					if (d2 >= championDist2)
						continue;
					d = v.z - z;
					d2 += d * d;
					if (d2 >= championDist2)
						continue;
					champion = challenger;
					championDist2 = d2;
				}
			}
			
			if (DEBUG_WITH_SEQUENTIAL_SEARCH)
			{
				int champSeq = 0;
				double champSeqD2 = dist2(Geodesic3D.vertexVectors[champSeq], x, y, z);
				for (int k = vertexCounts[geodesicLevel]; --k > 0; )
				{
					double challengerD2 = dist2(Geodesic3D.vertexVectors[k], x, y, z);
					if (challengerD2 < champSeqD2)
					{
						champSeq = k;
						champSeqD2 = challengerD2;
					}
				}
				if (champion != champSeq)
				{
					if (champSeqD2 + .01 < championDist2)
					{
						System.Console.Out.WriteLine("?que? getNormix is messed up?");
						bool considered = bsConsidered.Get(champSeq);
						System.Console.Out.WriteLine("Was the sequential winner considered? " + considered);
						System.Console.Out.WriteLine("champion " + champion + " @ " + championDist2 + " sequential champ " + champSeq + " @ " + champSeqD2 + "\n");
						return (short) champSeq;
					}
				}
			}
			return champion;
		}
		
		internal short[] inverseNormixes;
		
		internal virtual void  calculateInverseNormixes()
		{
			inverseNormixes = new short[normixCount];
			for (int n = normixCount; --n >= 0; )
			{
				Vector3f v = Geodesic3D.vertexVectors[n];
				inverseNormixes[n] = getNormix(- v.x, - v.y, - v.z, NORMIX_GEODESIC_LEVEL);
			}
			// validate that everyone's inverse is themselves
			for (int n = normixCount; --n >= 0; )
				if (inverseNormixes[inverseNormixes[n]] != n)
					throw new System.NullReferenceException();
		}
		
		internal virtual sbyte getIntensity(short normix)
		{
			if (normix < 0)
				return intensities2Sided[~ normix];
			return intensities[normix];
		}
		
		internal virtual short[] getFaceNormixes(int level)
		{
			short[] faceNormixes = faceNormixesArrays[level];
			if (faceNormixes != null)
				return faceNormixes;
			return calcFaceNormixes(level);
		}
		
		internal static double dist2(Vector3f v1, Vector3f v2)
		{
			double dx = v1.x - v2.x;
			double dy = v1.y - v2.y;
			double dz = v1.z - v2.z;
			return dx * dx + dy * dy + dz * dz;
		}
		
		internal static double dist2(Vector3f v1, double x, double y, double z)
		{
			double dx = v1.x - x;
			double dy = v1.y - y;
			double dz = v1.z - z;
			return dx * dx + dy * dy + dz * dz;
		}
		
		private const bool DEBUG_FACE_VECTORS = false;
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'calcFaceNormixes'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		private short[] calcFaceNormixes(int level)
		{
			lock (this)
			{
				//    System.out.println("calcFaceNormixes(" + level + ")");
				short[] faceNormixes = faceNormixesArrays[level];
				if (faceNormixes != null)
					return faceNormixes;
				Vector3f t = new Vector3f();
				short[] faceVertexes = Geodesic3D.faceVertexesArrays[level];
				int j = faceVertexes.Length;
				int faceCount = j / 3;
				faceNormixes = new short[faceCount];
				for (int i = faceCount; --i >= 0; )
				{
					Vector3f vA = Geodesic3D.vertexVectors[faceVertexes[--j]];
					Vector3f vB = Geodesic3D.vertexVectors[faceVertexes[--j]];
					Vector3f vC = Geodesic3D.vertexVectors[faceVertexes[--j]];
					t.add(vA, vB);
					t.add(vC);
					short normix = getNormix(t);
					faceNormixes[i] = normix;
					if (DEBUG_FACE_VECTORS)
					{
						Vector3f vN = Geodesic3D.vertexVectors[normix];
						
						double d2At = dist2(t, vA);
						double d2Bt = dist2(t, vB);
						double d2Ct = dist2(t, vC);
						double d2Nt = dist2(t, vN);
						if (d2At < d2Nt || d2Bt < d2Nt || d2Ct < d2Nt)
						{
							System.Console.Out.WriteLine(" d2At =" + d2At + " d2Bt =" + d2Bt + " d2Ct =" + d2Ct + " d2Nt =" + d2Nt);
						}
					}
					
					/*
					double champD = dist2(Geodesic3D.vertexVectors[normix], t);
					int champ = normix;
					for (int k = normixCount; --k >= 0; ) {
					double d = dist2(Geodesic3D.vertexVectors[k], t);
					if (d < champD) {
					champ = k;
					champD = d;
					}
					}
					if (champ != normix) {
					System.out.println("normix " + normix + " @ " +
					dist2(Geodesic3D.vertexVectors[normix], t) +
					"\n" +
					"champ " + champ + " @ " +
					dist2(Geodesic3D.vertexVectors[champ], t) +
					"\n");
					}
					*/
				}
				faceNormixesArrays[level] = faceNormixes;
				return faceNormixes;
			}
		}
		
		internal virtual bool isDirectedTowardsCamera(short normix)
		{
			// normix < 0 means a double sided normix, so always visible
			return (normix < 0) || (transformedVectors[normix].z > 0);
		}
		
		internal virtual short getVisibleNormix(double x, double y, double z, int[] visibilityBitmap, int level)
		{
			int minMapped = Bmp.getMinMappedBit(visibilityBitmap);
			//    System.out.println("minMapped =" + minMapped);
			if (minMapped < 0)
				return - 1;
			int maxMapped = Bmp.getMaxMappedBit(visibilityBitmap);
			int maxVisible = Geodesic3D.vertexCounts[level];
			int max = maxMapped < maxVisible?maxMapped:maxVisible;
			Vector3f v;
			double d;
			double championDist2;
			int champion = minMapped;
			v = Geodesic3D.vertexVectors[champion];
			d = x - v.x;
			championDist2 = d * d;
			d = y - v.y;
			championDist2 += d * d;
			d = z - v.z;
			championDist2 += d * d;
			
			for (int challenger = minMapped + 1; challenger < max; ++challenger)
			{
				if (!Bmp.getBit(visibilityBitmap, challenger))
					continue;
				double challengerDist2 = dist2(Geodesic3D.vertexVectors[challenger], x, y, z);
				if (challengerDist2 < championDist2)
				{
					champion = challenger;
					championDist2 = challengerDist2;
				}
			}
			//    System.out.println("visible champion=" + champion);
			if (!Bmp.getBit(visibilityBitmap, champion))
				throw new System.IndexOutOfRangeException();
			return (short) champion;
		}
	}
}