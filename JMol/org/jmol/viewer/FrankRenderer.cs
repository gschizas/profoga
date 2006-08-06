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
namespace org.jmol.viewer
{
	
	class FrankRenderer:ShapeRenderer
	{
		
		internal override void  render()
		{
			Frank frank = (Frank) shape;
			short mad = frank.mad;
			if (mad == 0)
				return ;
			frank.calcMetrics();
			
			if (frank.font3d == null)
				System.Console.Out.WriteLine("que? frank.font3d = null?");
			
			g3d.drawString(Frank.frankString, frank.font3d, frank.colix, frank.bgcolix, g3d.RenderWidth - frank.frankWidth - Frank.frankMargin, g3d.RenderHeight - frank.frankDescent - Frank.frankMargin, 0);
		}
	}
}