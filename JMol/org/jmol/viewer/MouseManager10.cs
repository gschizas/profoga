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
	
	class MouseManager10:MouseManager
	{
		
		internal MouseManager10(System.Windows.Forms.Control component, Viewer viewer):base(component, viewer)
		{
		}
		
		private int applyLeftMouse(int modifiers)
		{
			// if neither BUTTON2 or BUTTON3 then it must be BUTTON1
			return ((modifiers & MIDDLE_RIGHT) == 0)?(modifiers | LEFT):modifiers;
		}
		
		internal int xWhenPressed, yWhenPressed, modifiersWhenPressed10;
		
		//UPGRADE_ISSUE: Class 'java.awt.Event' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
		internal override bool handleOldJvm10Event(Event e)
		{
			//UPGRADE_ISSUE: Field 'java.awt.Event.x' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
			//UPGRADE_ISSUE: Field 'java.awt.Event.y' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
			//UPGRADE_ISSUE: Field 'java.awt.Event.modifiers' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
			int x = e.x, y = e.y, modifiers = e.modifiers;
			//UPGRADE_ISSUE: Field 'java.awt.Event.when' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
			long time = e.when;
			modifiers = applyLeftMouse(modifiers);
			//UPGRADE_ISSUE: Field 'java.awt.Event.id' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
			switch (e.id)
			{
				
				//UPGRADE_ISSUE: Field 'java.awt.Event.MOUSE_DOWN' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
				case Event.MOUSE_DOWN: 
					xWhenPressed = x; yWhenPressed = y; modifiersWhenPressed10 = modifiers;
					mousePressed(time, x, y, modifiers, false);
					break;
				
				//UPGRADE_ISSUE: Field 'java.awt.Event.MOUSE_DRAG' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
				case Event.MOUSE_DRAG: 
					mouseDragged(time, x, y, modifiers);
					break;
				
				//UPGRADE_ISSUE: Field 'java.awt.Event.MOUSE_ENTER' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
				case Event.MOUSE_ENTER: 
					mouseEntered(time, x, y);
					break;
				
				//UPGRADE_ISSUE: Field 'java.awt.Event.MOUSE_EXIT' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
				case Event.MOUSE_EXIT: 
					mouseExited(time, x, y);
					break;
				
				//UPGRADE_ISSUE: Field 'java.awt.Event.MOUSE_MOVE' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
				case Event.MOUSE_MOVE: 
					mouseMoved(time, x, y, modifiers);
					break;
				
				//UPGRADE_ISSUE: Field 'java.awt.Event.MOUSE_UP' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
				case Event.MOUSE_UP: 
					mouseReleased(time, x, y, modifiers);
					// simulate a mouseClicked event for us
					if (x == xWhenPressed && y == yWhenPressed && modifiers == modifiersWhenPressed10)
					{
						// the underlying code will turn this into dbl clicks for us
						mouseClicked(time, x, y, modifiers, 1);
					}
					break;
				
				default: 
					return false;
				
			}
			return true;
		}
	}
}