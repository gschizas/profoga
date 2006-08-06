using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
namespace JMol
{
	/* $RCSfile$
	* $Author: egonw $
	* $Date: 2006-01-11 15:38:05 +0100 (mer., 11 janv. 2006) $
	* $Revision: 4395 $
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
	
	[Serializable]
	public class JmolAppletControl:org.jmol.applet.JmolAppletControl
	{
		public JmolAppletControl()
		{
			InitBlock();
		}
		private void  InitBlock()
		{
			this.Load += new System.EventHandler(this.JmolAppletControl_StartEventHandler);
			this.Disposed += new System.EventHandler(this.JmolAppletControl_StopEventHandler);
		}
		public new System.String  TempDocumentBaseVar = "";
		public override System.Uri DocumentBase
		{
			get
			{
				if (TempDocumentBaseVar == "")
					return new System.Uri("http://127.0.0.1");
				else
					return new System.Uri(TempDocumentBaseVar);
			}
			
		}
		private void  JmolAppletControl_StartEventHandler(System.Object sender, System.EventArgs e)
		{
			init();
			start();
		}
		private void  JmolAppletControl_StopEventHandler(System.Object sender, System.EventArgs e)
		{
			stop();
		}
		public override String getParameter(System.String paramName)
		{
			return null;
		}
	}
}