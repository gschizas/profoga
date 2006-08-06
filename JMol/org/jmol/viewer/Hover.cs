/* $RCSfile$
* $Author: egonw $
* $Date: 2005-11-10 16:52:44 +0100 (jeu., 10 nov. 2005) $
* $Revision: 4255 $
*
* Copyright (C) 2002-2005  The Jmol Development Team
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
using org.jmol.g3d;
namespace org.jmol.viewer
{
	
	class Hover:Shape
	{
		
		private const System.String FONTFACE = "SansSerif";
		private const System.String FONTSTYLE = "Plain";
		private const int FONTSIZE = 12;
		
		internal int atomIndex = - 1;
		internal Font3D font3d;
		internal System.String labelFormat = "%U";
		internal short colixBackground;
		internal short colixForeground;
		
		internal override void  initShape()
		{
			font3d = g3d.getFont3D(FONTFACE, FONTSTYLE, FONTSIZE);
			colixBackground = Graphics3D.getColix("#FFFFC3"); // 255, 255, 195
			colixForeground = Graphics3D.BLACK;
		}
		
		internal override void  setProperty(System.String propertyName, System.Object value_Renamed, System.Collections.BitArray bsSelected)
		{
			if ((System.Object) "target" == (System.Object) propertyName)
			{
				if (value_Renamed == null)
					atomIndex = - 1;
				else
					atomIndex = ((System.Int32) value_Renamed);
				return ;
			}
			
			if ((System.Object) "color" == (System.Object) propertyName)
			{
				//      System.out.println("hover color changed");
				colixForeground = Graphics3D.getColix(value_Renamed);
				return ;
			}
			
			// translucency not implemented
			
			if ((System.Object) "bgcolor" == (System.Object) propertyName)
			{
				//      System.out.println("hover bgcolor changed");
				colixBackground = Graphics3D.getColix(value_Renamed);
				return ;
			}
			
			if ((System.Object) "font" == (System.Object) propertyName)
			{
				font3d = (Font3D) value_Renamed;
				return ;
			}
			
			if ((System.Object) "label" == (System.Object) propertyName)
			{
				labelFormat = ((System.String) value_Renamed);
				if (labelFormat != null && labelFormat.Length == 0)
					labelFormat = null;
				return ;
			}
		}
	}
}