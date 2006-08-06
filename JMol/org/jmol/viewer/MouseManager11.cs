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
	
	class MouseManager11:MouseManager
	{
		static private System.Int32 state35;
		private static void  mouseDown(System.Object event_sender, System.Windows.Forms.MouseEventArgs e)
		{
			state35 = ((int) e.Button | (int) System.Windows.Forms.Control.ModifierKeys);
		}
		
		internal MouseManager11(System.Windows.Forms.Control component, Viewer viewer):base(component, viewer)
		{
			component.MouseDown += new System.Windows.Forms.MouseEventHandler(org.jmol.viewer.MouseManager11.mouseDown);
			component.Click += new System.EventHandler(this.mouseClicked);
			component.MouseEnter += new System.EventHandler(this.mouseEntered);
			component.MouseLeave += new System.EventHandler(this.mouseExited);
			component.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousePressed);
			component.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseReleased);
			component.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mouseMoved);
		}
		
		//UPGRADE_ISSUE: Class 'java.awt.Event' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
		internal override bool handleOldJvm10Event(Event e)
		{
			return false;
		}
		
		public virtual void  mouseClicked(System.Object event_sender, System.EventArgs e)
		{
			//UPGRADE_ISSUE: Method 'java.awt.event.InputEvent.getWhen' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventInputEvent'"
			//UPGRADE_ISSUE: Method 'java.awt.event.MouseEvent.getX' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventMouseEventgetX'"
			//UPGRADE_ISSUE: Method 'java.awt.event.MouseEvent.getY' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventMouseEventgetY'"
			//UPGRADE_NOTE: The 'java.awt.event.InputEvent.getModifiers' method simulation might not work for some controls. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1284'"
			//UPGRADE_ISSUE: Method 'java.awt.event.MouseEvent.getClickCount' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventMouseEventgetClickCount'"
			mouseClicked(e.getWhen(), e.getX(), e.getY(), state35, e.getClickCount());
		}
		
		public virtual void  mouseEntered(System.Object event_sender, System.EventArgs e)
		{
			//UPGRADE_ISSUE: Method 'java.awt.event.InputEvent.getWhen' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventInputEvent'"
			//UPGRADE_ISSUE: Method 'java.awt.event.MouseEvent.getX' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventMouseEventgetX'"
			//UPGRADE_ISSUE: Method 'java.awt.event.MouseEvent.getY' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventMouseEventgetY'"
			mouseEntered(e.getWhen(), e.getX(), e.getY());
		}
		
		public virtual void  mouseExited(System.Object event_sender, System.EventArgs e)
		{
			//UPGRADE_ISSUE: Method 'java.awt.event.InputEvent.getWhen' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventInputEvent'"
			//UPGRADE_ISSUE: Method 'java.awt.event.MouseEvent.getX' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventMouseEventgetX'"
			//UPGRADE_ISSUE: Method 'java.awt.event.MouseEvent.getY' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventMouseEventgetY'"
			mouseExited(e.getWhen(), e.getX(), e.getY());
		}
		
		public virtual void  mousePressed(System.Object event_sender, System.Windows.Forms.MouseEventArgs e)
		{
			//UPGRADE_ISSUE: Method 'java.awt.event.InputEvent.getWhen' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventInputEvent'"
			//UPGRADE_NOTE: The 'java.awt.event.InputEvent.getModifiers' method simulation might not work for some controls. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1284'"
			//UPGRADE_TODO: Method 'java.awt.event.MouseEvent.isPopupTrigger' was converted to 'System.Windows.Forms.MouseButtons.Right' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawteventMouseEventisPopupTrigger'"
			mousePressed(e.getWhen(), e.X, e.Y, state35, e.Button == System.Windows.Forms.MouseButtons.Right);
		}
		
		public virtual void  mouseReleased(System.Object event_sender, System.Windows.Forms.MouseEventArgs e)
		{
			//UPGRADE_ISSUE: Method 'java.awt.event.InputEvent.getWhen' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventInputEvent'"
			//UPGRADE_NOTE: The 'java.awt.event.InputEvent.getModifiers' method simulation might not work for some controls. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1284'"
			mouseReleased(e.getWhen(), e.X, e.Y, state35);
		}
		
		public virtual void  mouseDragged(System.Object event_sender, System.Windows.Forms.MouseEventArgs e)
		{
			//UPGRADE_NOTE: The 'java.awt.event.InputEvent.getModifiers' method simulation might not work for some controls. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1284'"
			int modifiers = state35;
			/****************************************************************
			* Netscape 4.* Win32 has a problem with mouseDragged
			* if you left-drag then none of the modifiers are selected
			* we will try to fix that here
			****************************************************************/
			if ((modifiers & LEFT_MIDDLE_RIGHT) == 0)
				modifiers |= LEFT;
			/****************************************************************/
			//UPGRADE_ISSUE: Method 'java.awt.event.InputEvent.getWhen' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventInputEvent'"
			mouseDragged(e.getWhen(), e.X, e.Y, modifiers);
		}
		
		public virtual void  mouseMoved(System.Object event_sender, System.Windows.Forms.MouseEventArgs e)
		{
			//UPGRADE_ISSUE: Method 'java.awt.event.InputEvent.getWhen' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventInputEvent'"
			//UPGRADE_NOTE: The 'java.awt.event.InputEvent.getModifiers' method simulation might not work for some controls. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1284'"
			mouseMoved(e.getWhen(), e.X, e.Y, state35);
		}
	}
}