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
	
	class HoverRenderer:ShapeRenderer
	{
		
		internal override void  render()
		{
			Hover hover = (Hover) shape;
			if (hover.atomIndex == - 1 || hover.labelFormat == null)
				return ;
			Atom atom = frame.getAtomAt(hover.atomIndex);
			/*
			System.out.println("hover on atom:" + hover.atomIndex + " @ " +
			atom.getScreenX() + "," + atom.getScreenY());
			*/
			System.String msg = atom.formatLabel(hover.labelFormat);
			Font3D font3d = hover.font3d;
			System.Drawing.Font fontMetrics = font3d.fontMetrics;
			//UPGRADE_TODO: The equivalent in .NET for method 'java.awt.FontMetrics.getAscent' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			int ascent = SupportClass.GetAscent(fontMetrics);
			//UPGRADE_TODO: The equivalent in .NET for method 'java.awt.FontMetrics.getDescent' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			int descent = SupportClass.GetDescent(fontMetrics);
			int msgHeight = ascent + descent;
			//UPGRADE_ISSUE: Method 'java.awt.FontMetrics.stringWidth' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtFontMetricsstringWidth_javalangString'"
			int msgWidth = fontMetrics.stringWidth(msg);
			short colixBackground = hover.colixBackground;
			short colixForeground = hover.colixForeground;
			int windowWidth = g3d.WindowWidth;
			int windowHeight = g3d.WindowHeight;
			int width = msgWidth + 8;
			int height = msgHeight + 8;
			int x = atom.ScreenX + 4;
			if (x + width > windowWidth)
				x = windowWidth - width;
			if (x < 0)
				x = 0;
			int y = atom.ScreenY - height - 4;
			if (y + height > windowHeight)
				y = windowHeight - height;
			if (y < 0)
				y = 0;
			
			int msgX = x + 4;
			int msgYBaseline = y + 4 + ascent;
			if (colixBackground != 0)
			{
				g3d.fillRect(colixBackground, x, y, 2, width, height);
				g3d.drawRectNoSlab(colixForeground, x + 1, y + 1, 1, width - 2, height - 2);
			}
			g3d.drawStringNoSlab(msg, font3d, colixForeground, (short) 0, msgX, msgYBaseline, 0);
		}
	}
}