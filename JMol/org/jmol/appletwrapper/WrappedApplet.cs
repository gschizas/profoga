/* $RCSfile$
* $Author: egonw $
* $Date: 2005-11-10 16:52:44 +0100 (jeu., 10 nov. 2005) $
* $Revision: 4255 $
*
* Copyright (C) 2004-2005  The Jmol Development Team
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
namespace org.jmol.appletwrapper
{
	
	public interface WrappedApplet
	{
		AppletWrapper AppletWrapper
		{
			set;
			
		}
		System.String AppletInfo
		{
			get;
			
		}
		void  init();
		void  update(System.Drawing.Graphics g);
		void  paint(System.Drawing.Graphics g);
		//UPGRADE_ISSUE: Class 'java.awt.Event' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
		bool handleEvent(Event e);
		
		//UPGRADE_TODO: Class 'netscape.javascript.JSObject' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
		void  scriptButton(JSObject buttonWindow, System.String buttonName, System.String script, System.String buttonCallback);
		void  script(System.String script);
		void  loadInline(System.String strModel);
		void  loadNodeId(System.String nodeId);
		//UPGRADE_TODO: Class 'netscape.javascript.JSObject' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
		void  loadDOMNode(JSObject DOMNode);
	}
}