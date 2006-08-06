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
//UPGRADE_TODO: The type 'javax.vecmath.Point3f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Point3f = javax.vecmath.Point3f;
//UPGRADE_TODO: The type 'javax.vecmath.Point3i' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Point3i = javax.vecmath.Point3i;
namespace org.jmol.viewer
{
	
	class TraceRenderer:MpsRenderer
	{
		
		internal bool isNucleicPolymer;
		
		internal override void  renderMpspolymer(Mps.Mpspolymer mpspolymer)
		{
			Trace.Tchain tchain = (Trace.Tchain) mpspolymer;
			isNucleicPolymer = tchain.polymer is NucleicPolymer;
			monomerCount = tchain.monomerCount;
			if (monomerCount == 0)
				return ;
			monomers = tchain.monomers;
			leadMidpoints = tchain.leadMidpoints;
			leadMidpointScreens = calcScreenLeadMidpoints(monomerCount, leadMidpoints);
			render1Chain(tchain.mads, tchain.colixes);
			viewer.freeTempScreens(leadMidpointScreens);
		}
		
		internal int monomerCount;
		
		internal Monomer[] monomers;
		internal Point3i[] leadMidpointScreens;
		internal Point3f[] leadMidpoints;
		
		internal virtual void  render1Chain(short[] mads, short[] colixes)
		{
			for (int i = monomerCount; --i >= 0; )
			{
				if (mads[i] == 0)
					continue;
				short colix = Graphics3D.inheritColix(colixes[i], monomers[i].LeadAtom.colixAtom);
				renderRopeSegment(colix, mads, i, monomerCount, monomers, leadMidpointScreens, null);
			}
		}
	}
}