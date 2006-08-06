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
//UPGRADE_TODO: The type 'java.awt.event_Renamed.MouseWheelEvent' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using MouseWheelEvent = java.awt.event_Renamed.MouseWheelEvent;
//UPGRADE_TODO: The type 'java.awt.event_Renamed.MouseWheelListener' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using MouseWheelListener = java.awt.event_Renamed.MouseWheelListener;
namespace org.jmol.viewer
{
	
	class MouseManager14:MouseManager11, MouseWheelListener
	{
		
		internal MouseManager14(System.Windows.Forms.Control component, Viewer viewer):base(component, viewer)
		{
			component.addMouseWheelListener(this);
		}
		
		public virtual void  mouseWheelMoved(MouseWheelEvent e)
		{
			mouseWheel(e.getWhen(), e.getWheelRotation(), e.getModifiers());
		}
	}
}