/* $RCSfile$
* $Author: migueljmol $
* $Date: 2005-11-28 16:48:37 +0100 (lun., 28 nov. 2005) $
* $Revision: 4288 $
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
	
	class Echo:Shape
	{
		
		private const int LEFT = 0;
		private const int CENTER = 1;
		private const int RIGHT = 2;
		
		private const int TOP = 0;
		private const int BOTTOM = 1;
		private const int MIDDLE = 2;
		
		private const System.String FONTFACE = "Serif";
		private const int FONTSIZE = 20;
		//UPGRADE_NOTE: Final was removed from the declaration of 'COLOR '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_NOTE: The initialization of  'COLOR' was moved to static method 'org.jmol.viewer.Echo'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private static readonly short COLOR;
		
		internal Text topText;
		internal Text middleText;
		internal Text bottomText;
		internal Text currentText;
		
		internal override void  initShape()
		{
		}
		
		internal override void  setProperty(System.String propertyName, System.Object value_Renamed, System.Collections.BitArray bsSelected)
		{
			/*
			System.out.println("Echo.setProperty(" + propertyName + "," + value + ")");
			*/
			
			if ((System.Object) "color" == (System.Object) propertyName)
			{
				if (currentText != null)
					currentText.colix = Graphics3D.getColix(value_Renamed);
				return ;
			}
			
			if ((System.Object) "translucency" == (System.Object) propertyName)
			{
				System.Console.Out.WriteLine("translucent echo not implemented");
			}
			
			if ((System.Object) "bgcolor" == (System.Object) propertyName)
			{
				if (currentText != null)
					currentText.bgcolix = value_Renamed == null?(short) 0:Graphics3D.getColix(value_Renamed);
				return ;
			}
			
			if ((System.Object) "font" == (System.Object) propertyName)
			{
				if (currentText != null)
				{
					currentText.font3d = (Font3D) value_Renamed;
					currentText.recalc();
				}
				return ;
			}
			
			if ((System.Object) "echo" == (System.Object) propertyName)
			{
				if (currentText != null)
				{
					currentText.text = ((System.String) value_Renamed);
					currentText.recalc();
				}
			}
			
			if ((System.Object) "off" == (System.Object) propertyName)
			{
				currentText = null;
				if (topText != null)
					topText.text = null;
				if (middleText != null)
					middleText.text = null;
				if (bottomText != null)
					bottomText.text = null;
			}
			
			if ((System.Object) "target" == (System.Object) propertyName)
			{
				System.String target = String.Intern(((System.String) value_Renamed));
				if ((System.Object) "top" == (System.Object) target)
				{
					if (topText == null)
						topText = new Text(this, TOP, CENTER, g3d.getFont3D(FONTFACE, FONTSIZE), COLOR);
					currentText = topText;
					return ;
				}
				
				if ((System.Object) "middle" == (System.Object) target)
				{
					if (middleText == null)
						middleText = new Text(this, MIDDLE, CENTER, g3d.getFont3D(FONTFACE, FONTSIZE), COLOR);
					currentText = middleText;
					return ;
				}
				
				if ((System.Object) "bottom" == (System.Object) target)
				{
					if (bottomText == null)
						bottomText = new Text(this, BOTTOM, LEFT, g3d.getFont3D(FONTFACE, FONTSIZE), COLOR);
					currentText = bottomText;
					return ;
				}
				
				if ((System.Object) "none" == (System.Object) target)
				{
					currentText = null;
					return ;
				}
				System.Console.Out.WriteLine("unrecognized target:" + target);
				return ;
			}
			
			if ((System.Object) "align" == (System.Object) propertyName)
			{
				if (currentText == null)
					return ;
				System.String align = String.Intern(((System.String) value_Renamed));
				if ((System.Object) "left" == (System.Object) align)
				{
					currentText.align = LEFT;
					return ;
				}
				
				if ((System.Object) "center" == (System.Object) align)
				{
					currentText.align = CENTER;
					return ;
				}
				
				if ((System.Object) "right" == (System.Object) align)
				{
					currentText.align = RIGHT;
					return ;
				}
				System.Console.Out.WriteLine("unrecognized align:" + align);
				return ;
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'Text' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		internal class Text
		{
			private void  InitBlock(Echo enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Echo enclosingInstance;
			public Echo Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			internal System.String text;
			internal int align;
			internal int valign;
			internal Font3D font3d;
			internal short colix;
			internal short bgcolix;
			
			internal int width;
			internal int ascent;
			internal int descent;
			
			internal Text(Echo enclosingInstance, int valign, int align, Font3D font3d, short colix)
			{
				InitBlock(enclosingInstance);
				this.align = align;
				this.valign = valign;
				this.font3d = font3d;
				this.colix = colix;
			}
			
			internal virtual void  recalc()
			{
				if (text == null || text.Length == 0)
				{
					text = null;
					return ;
				}
				System.Drawing.Font fm = font3d.fontMetrics;
				//UPGRADE_ISSUE: Method 'java.awt.FontMetrics.stringWidth' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtFontMetricsstringWidth_javalangString'"
				width = fm.stringWidth(text);
				//UPGRADE_TODO: The equivalent in .NET for method 'java.awt.FontMetrics.getDescent' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				descent = SupportClass.GetDescent(fm);
				//UPGRADE_TODO: The equivalent in .NET for method 'java.awt.FontMetrics.getAscent' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				ascent = SupportClass.GetAscent(fm);
			}
			
			internal virtual void  render(Graphics3D g3d)
			{
				if (text == null)
					return ;
				int x = g3d.RenderWidth - width - 1;
				if (align == org.jmol.viewer.Echo.CENTER)
					x /= 2;
				else if (align == org.jmol.viewer.Echo.LEFT)
					x = 0;
				
				int y;
				if (valign == org.jmol.viewer.Echo.TOP)
					y = ascent;
				else if (valign == org.jmol.viewer.Echo.MIDDLE)
				// baseline is at the middle
					y = g3d.RenderHeight / 2;
				else
					y = g3d.RenderHeight - descent - 1;
				
				g3d.drawStringNoSlab(text, font3d, colix, (short) 0, x, y, 0);
			}
		}
		static Echo()
		{
			COLOR = Graphics3D.RED;
		}
	}
}