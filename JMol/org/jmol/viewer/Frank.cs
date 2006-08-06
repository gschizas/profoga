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
using org.jmol.g3d;
namespace org.jmol.viewer
{
	
	class Frank:SelectionIndependentShape
	{
		
		internal const System.String frankString = "Jmol";
		internal const System.String defaultFontName = "SansSerif";
		internal const System.String defaultFontStyle = "Bold";
		//UPGRADE_NOTE: Final was removed from the declaration of 'defaultFontColix '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_NOTE: The initialization of  'defaultFontColix' was moved to static method 'org.jmol.viewer.Frank'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		internal static readonly short defaultFontColix;
		internal const int defaultFontSize = 16;
		internal const int frankMargin = 4;
		
		internal Font3D currentMetricsFont3d;
		internal int frankWidth;
		internal int frankAscent;
		internal int frankDescent;
		
		
		internal override void  initShape()
		{
			colix = defaultFontColix;
			font3d = g3d.getFont3D(defaultFontName, defaultFontStyle, defaultFontSize);
		}
		
		internal override bool wasClicked(int x, int y)
		{
			int width = g3d.RenderWidth;
			int height = g3d.RenderHeight;
			if (g3d.fullSceneAntialiasRendering())
			{
				x *= 2;
				y *= 2;
			}
			return (width > 0 && height > 0 && x > width - frankWidth - frankMargin && y > height - frankAscent - frankMargin);
		}
		
		internal virtual void  calcMetrics()
		{
			if (font3d != currentMetricsFont3d)
			{
				currentMetricsFont3d = font3d;
				System.Drawing.Font fm = font3d.fontMetrics;
				//UPGRADE_ISSUE: Method 'java.awt.FontMetrics.stringWidth' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtFontMetricsstringWidth_javalangString'"
				frankWidth = fm.stringWidth(frankString);
				//UPGRADE_TODO: The equivalent in .NET for method 'java.awt.FontMetrics.getDescent' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				frankDescent = SupportClass.GetDescent(fm);
				//UPGRADE_TODO: The equivalent in .NET for method 'java.awt.FontMetrics.getAscent' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				frankAscent = SupportClass.GetAscent(fm);
			}
		}
		static Frank()
		{
			defaultFontColix = Graphics3D.GRAY;
		}
	}
}