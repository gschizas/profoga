/* $RCSfile$
* $Author: egonw $
* $Date: 2005-11-10 16:52:44 +0100 (jeu., 10 nov. 2005) $
* $Revision: 4255 $
*
* Copyright (C) 2003-2005  Miguel, Jmol Development, www.jmol.org
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
	
	
	class RibbonsRenderer:MpsRenderer
	{
		// not current for Mcp class
		
		internal Ribbons strands;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'pointT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
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
					if (Float.isNaN(pointT.x))
					{
						System.Console.Out.WriteLine(" vectors[" + i + "]=" + vectors[i] + " centers[" + i + "]=" + centers[i] + " mads[" + i + "]=" + mads[i] + " scale=" + scale + " --> " + pointT);
					}
					viewer.transformPoint(pointT, screens[i]);
				}
			}
			return screens;
		}
		
		internal bool isNucleic;
		internal bool ribbonBorder = false;
		
		internal override void  renderMpspolymer(Mps.Mpspolymer mpspolymer)
		{
			Ribbons.Schain strandsChain = (Ribbons.Schain) mpspolymer;
			if (strandsChain.wingVectors != null)
			{
				// note that we are not treating a PhosphorusPolymer
				// as nucleic because we are not calculating the wing
				// vector correctly.
				// if/when we do that then this test will become
				// isNucleic = strandsChain.polymer.isNucleic();
				ribbonBorder = viewer.RibbonBorder;
				isNucleic = strandsChain.polymer is NucleicPolymer;
				render1Chain(strandsChain.monomerCount, strandsChain.monomers, strandsChain.leadMidpoints, strandsChain.wingVectors, strandsChain.mads, strandsChain.colixes);
			}
		}
		
		
		internal virtual void  render1Chain(int monomerCount, Monomer[] monomers, Point3f[] centers, Vector3f[] vectors, short[] mads, short[] colixes)
		{
			Point3i[] ribbonTopScreens;
			Point3i[] ribbonBottomScreens;
			
			ribbonTopScreens = calcScreens(centers, vectors, mads, isNucleic?1f:0.5f);
			ribbonBottomScreens = calcScreens(centers, vectors, mads, isNucleic?0f:- 0.5f);
			render2Strand(monomerCount, monomers, mads, colixes, ribbonTopScreens, ribbonBottomScreens);
			viewer.freeTempScreens(ribbonTopScreens);
			viewer.freeTempScreens(ribbonBottomScreens);
		}
		
		internal virtual void  render2Strand(int monomerCount, Monomer[] monomers, short[] mads, short[] colixes, Point3i[] ribbonTopScreens, Point3i[] ribbonBottomScreens)
		{
			for (int i = monomerCount; --i >= 0; )
				if (mads[i] > 0)
					render2StrandSegment(monomerCount, monomers[i], colixes[i], mads, ribbonTopScreens, ribbonBottomScreens, i);
		}
		
		internal virtual void  render2StrandSegment(int monomerCount, Monomer monomer, short colix, short[] mads, Point3i[] ribbonTopScreens, Point3i[] ribbonBottomScreens, int i)
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
			
			g3d.drawHermite(true, ribbonBorder, colix, isNucleic?4:7, ribbonTopScreens[iPrev], ribbonTopScreens[i], ribbonTopScreens[iNext], ribbonTopScreens[iNext2], ribbonBottomScreens[iPrev], ribbonBottomScreens[i], ribbonBottomScreens[iNext], ribbonBottomScreens[iNext2]);
		}
	}
}