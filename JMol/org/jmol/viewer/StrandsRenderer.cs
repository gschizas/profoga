/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-03-27 23:34:39 +0200 (lun., 27 mars 2006) $
* $Revision: 4791 $
*
* Copyright (C) 2003-2006  Miguel, Jmol Development, www.jmol.org
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
//UPGRADE_TODO: The type 'javax.vecmath.Point3f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Point3f = javax.vecmath.Point3f;
//UPGRADE_TODO: The type 'javax.vecmath.Vector3f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Vector3f = javax.vecmath.Vector3f;
//UPGRADE_TODO: The type 'javax.vecmath.Point3i' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Point3i = javax.vecmath.Point3i;
namespace org.jmol.viewer
{
	
	class StrandsRenderer:MpsRenderer
	{
		
		internal Point3f pointT = new Point3f();
		
		internal virtual Point3i[] calcScreens(Point3f[] centers, Vector3f[] vectors, short[] mads, float offsetFraction)
		{
			Point3i[] screens = viewer.allocTempScreens(centers.length);
			if (offsetFraction == 0)
			{
				for (int i = centers.length; --i >= 0; )
					viewer.transformPoint(centers[i], screens[i]);
			}
			else
			{
				offsetFraction /= 1000;
				for (int i = centers.length; --i >= 0; )
				{
					pointT.set_Renamed(vectors[i]);
					float scale = mads[i] * offsetFraction;
					pointT.scaleAdd(scale, centers[i]);
					viewer.transformPoint(pointT, screens[i]);
				}
			}
			return screens;
		}
		
		internal int strandCount;
		internal float strandSeparation;
		internal float baseOffset;
		
		internal bool isNucleicPolymer;
		
		internal override void  renderMpspolymer(Mps.Mpspolymer mpspolymer)
		{
			Strands.Schain schain = (Strands.Schain) mpspolymer;
			Strands strands = (Strands) shape;
			
			strandCount = strands.strandCount;
			strandSeparation = (strandCount <= 1)?0:1f / (strandCount - 1);
			baseOffset = ((strandCount & 1) == 0)?strandSeparation / 2:strandSeparation;
			
			if (schain.wingVectors != null)
			{
				isNucleicPolymer = schain.polymer is NucleicPolymer;
				render1Chain(schain.monomerCount, schain.monomers, schain.leadMidpoints, schain.wingVectors, schain.mads, schain.colixes);
			}
		}
		
		
		internal virtual void  render1Chain(int monomerCount, Monomer[] monomers, Point3f[] centers, Vector3f[] vectors, short[] mads, short[] colixes)
		{
			if (vectors == null)
				return ;
			Point3i[] screens;
			for (int i = strandCount >> 1; --i >= 0; )
			{
				float f = (i * strandSeparation) + baseOffset;
				screens = calcScreens(centers, vectors, mads, f);
				render1Strand(monomerCount, monomers, mads, colixes, screens);
				viewer.freeTempScreens(screens);
				screens = calcScreens(centers, vectors, mads, - f);
				render1Strand(monomerCount, monomers, mads, colixes, screens);
				viewer.freeTempScreens(screens);
			}
			if ((strandCount & 1) != 0)
			{
				screens = calcScreens(centers, vectors, mads, 0f);
				render1Strand(monomerCount, monomers, mads, colixes, screens);
				viewer.freeTempScreens(screens);
			}
		}
		
		internal virtual void  render1Strand(int monomerCount, Monomer[] monomers, short[] mads, short[] colixes, Point3i[] screens)
		{
			for (int i = monomerCount; --i >= 0; )
				if (mads[i] > 0)
					render1StrandSegment(monomerCount, monomers[i], colixes[i], mads, screens, i);
		}
		
		
		internal virtual void  render1StrandSegment(int monomerCount, Monomer monomer, short colix, short[] mads, Point3i[] screens, int i)
		{
			int iLast = monomerCount;
			int iPrev = i - 1;
			if (iPrev < 0)
				iPrev = 0;
			int iNext = i + 1;
			if (iNext > iLast)
				iNext = iLast;
			int iNext2 = i + 2;
			if (iNext2 > iLast)
				iNext2 = iLast;
			colix = Graphics3D.inheritColix(colix, monomer.LeadAtom.colixAtom);
			g3d.drawHermite(colix, isNucleicPolymer?4:7, screens[iPrev], screens[i], screens[iNext], screens[iNext2]);
		}
	}
}