/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-03-27 23:44:15 +0200 (lun., 27 mars 2006) $
* $Revision: 4792 $
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
//UPGRADE_TODO: The type 'javax.vecmath.Point3f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Point3f = javax.vecmath.Point3f;
namespace org.jmol.viewer
{
	
	class Uccage:SelectionIndependentShape
	{
		
		internal bool hasUnitcell;
		internal float a, b, c, alpha, beta, gamma;
		internal Point3f[] vertices;
		
		internal override void  initShape()
		{
			
			float[] notionalUnitcell = frame.notionalUnitcell;
			hasUnitcell = notionalUnitcell != null;
			if (hasUnitcell)
			{
				a = notionalUnitcell[0];
				b = notionalUnitcell[1];
				c = notionalUnitcell[2];
				alpha = notionalUnitcell[3];
				beta = notionalUnitcell[4];
				gamma = notionalUnitcell[5];
			}
		}
	}
}