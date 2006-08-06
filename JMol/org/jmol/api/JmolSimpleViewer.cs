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
using Viewer = org.jmol.viewer.Viewer;
namespace org.jmol.api
{
	
	/// <summary> This is the high-level API for the JmolViewer for simple access.
	/// 
	/// </summary>
	
	abstract public class JmolSimpleViewer
	{
		abstract public System.String OpenFileError{get;}
		
		static public JmolSimpleViewer allocateSimpleViewer(System.Windows.Forms.Control awtComponent, JmolAdapter jmolAdapter)
		{
			return Viewer.allocateViewer(awtComponent, jmolAdapter);
		}
		
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		abstract public void  renderScreenImage(System.Drawing.Graphics g, ref System.Drawing.Size size, ref System.Drawing.Rectangle clip);
		
		abstract public System.String evalFile(System.String strFilename);
		abstract public System.String evalString(System.String strScript);
		
		abstract public void  openStringInline(System.String strModel);
		abstract public void  openDOM(System.Object DOMNode);
		abstract public void  openFile(System.String name);
	}
}