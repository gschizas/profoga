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
using org.jmol.api;
namespace org.jmol.applet
{
	
	class Jvm12
	{
		virtual internal System.Drawing.Size Size
		{
			get
			{
				dimSize = dimSize.IsEmpty?new System.Drawing.Size(0, 0):awtComponent.Size;
				return awtComponent.Size;
			}
			
		}
		
		internal System.Windows.Forms.Control awtComponent;
		internal Console console;
		internal JmolViewer viewer;
		
		internal Jvm12(System.Windows.Forms.Control awtComponent, JmolViewer viewer)
		{
			this.awtComponent = awtComponent;
			this.viewer = viewer;
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'rectClip '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal System.Drawing.Rectangle rectClip = new System.Drawing.Rectangle();
		//UPGRADE_NOTE: Final was removed from the declaration of 'dimSize '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal System.Drawing.Size dimSize = new System.Drawing.Size(0, 0);
		internal virtual System.Drawing.Rectangle getClipBounds(System.Drawing.Graphics g)
		{
			//UPGRADE_TODO: The equivalent in .NET for method 'java.awt.Graphics.getClipBounds' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			rectClip = System.Drawing.Rectangle.Truncate(g.ClipBounds);
			return rectClip;
		}
		
		internal virtual void  showConsole(bool showConsole)
		{
			System.Console.Out.WriteLine("Jvm12.showConsole(" + showConsole + ")");
			if (!showConsole)
			{
				if (console != null)
				{
					console.Visible = false;
					console = null;
				}
				return ;
			}
			if (console == null)
				console = new Console(awtComponent, viewer, this);
			console.Visible = true;
		}
		
		internal virtual void  consoleMessage(System.String message)
		{
			if (console != null)
				console.output(message);
		}
	}
}