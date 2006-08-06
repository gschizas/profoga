/* $RCSfile$
* $Author: egonw $
* $Date: 2005-11-10 16:52:44 +0100 (jeu., 10 nov. 2005) $
* $Revision: 4255 $
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
	
	class SasCavity
	{
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'probeCenter '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Point3f probeCenter;
		//UPGRADE_NOTE: Final was removed from the declaration of 'pointBottom '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Point3f pointBottom = new Point3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'normixBottom '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal short normixBottom;
		
		// probeCenter is the center of the probe
		// probeBase is the midpoint between this cavity
		// and its mirror image on the other side
		internal SasCavity(Point3f centerI, Point3f centerJ, Point3f centerK, Point3f probeCenter, float radiusP, Point3f probeBase, Vector3f vectorPI, Vector3f vectorPJ, Vector3f vectorPK, Vector3f vectorT, Graphics3D g3d)
		{
			this.probeCenter = new Point3f(probeCenter);
			
			vectorPI.sub(centerI, probeCenter);
			vectorPI.normalize();
			vectorPI.scale(radiusP);
			
			vectorPJ.sub(centerJ, probeCenter);
			vectorPJ.normalize();
			vectorPJ.scale(radiusP);
			
			vectorPK.sub(centerK, probeCenter);
			vectorPK.normalize();
			vectorPK.scale(radiusP);
			
			// the bottomPoint;
			
			vectorT.add(vectorPI, vectorPJ);
			vectorT.add(vectorPK);
			vectorT.normalize();
			pointBottom.scaleAdd(radiusP, vectorT, probeCenter);
			
			normixBottom = g3d.getInverseNormix(vectorT);
		}
	}
}