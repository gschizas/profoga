/* $RCSfile$
* $Author: egonw $
* $Date: 2006-01-11 15:38:05 +0100 (mer., 11 janv. 2006) $
* $Revision: 4395 $
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

/// <summary> This class only exists so that people can declare
/// JmolApplet in applet tags without having to give a full package
/// specification
/// 
/// see org.jmol.applet.JmolApplet
/// 
/// </summary>
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using JmolAppletInterface = org.jmol.api.JmolAppletInterface;
namespace JMol
{
	
	[Serializable]
	public class JmolApplet:org.jmol.appletwrapper.AppletWrapper, JmolAppletInterface
	{
		private void  InitBlock()
		{
			this.Load += new System.EventHandler(this.JmolApplet_StartEventHandler);
			this.Disposed += new System.EventHandler(this.JmolApplet_StopEventHandler);
		}
		
		public JmolApplet():base("org.jmol.applet.Jmol", "jmol75x29x8.gif", "Loading Jmol applet ...", 3, preloadClasses)
		{
			InitBlock();
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'preloadClasses'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private static readonly System.String[] preloadClasses = new System.String[]{"javax.vecmath.Point3f+", "org.jmol.g3d.Graphics3D", "org.jmol.adapter.smarter.SmarterJmolAdapter", "org.jmol.popup.JmolPopup", "javax.vecmath.Vector3f+", ".Matrix3f+", ".Point3i+", "org.jmol.g3d.Sphere3D", ".Line3D", ".Cylinder3D", ".Colix", ".Shade3D", "org.jmol.adapter.smarter.Atom", ".Bond", ".AtomSetCollection", ".AtomSetCollectionReader", ".Resolver"};
		
		public virtual void  script(System.String script)
		{
			if (wrappedApplet != null)
				((JmolAppletInterface) wrappedApplet).script(script);
		}
		
		public virtual void  loadInline(System.String strModel)
		{
			if (wrappedApplet != null)
				((JmolAppletInterface) wrappedApplet).loadInline(strModel);
		}
		
		public virtual void  loadNodeId(System.String nodeId)
		{
			if (wrappedApplet != null)
				((JmolAppletInterface) wrappedApplet).loadNodeId(nodeId);
		}
		
		//UPGRADE_TODO: Class 'netscape.javascript.JSObject' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
		public virtual void  loadDOMNode(JSObject DOMNode)
		{
			if (wrappedApplet != null)
				((JmolAppletInterface) wrappedApplet).loadDOMNode(DOMNode);
		}
		
		//UPGRADE_TODO: Class 'netscape.javascript.JSObject' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
		public virtual void  scriptButton(JSObject buttonWindow, System.String buttonName, System.String script, System.String buttonCallback)
		{
			if (wrappedApplet != null)
				((JmolAppletInterface) wrappedApplet).scriptButton(buttonWindow, buttonName, script, buttonCallback);
		}
		public System.String[][] GetParameterInfo()
		{
			return null;
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
		private void  JmolApplet_StartEventHandler(System.Object sender, System.EventArgs e)
		{
			init();
			start();
		}
		private void  JmolApplet_StopEventHandler(System.Object sender, System.EventArgs e)
		{
			stop();
		}
		public override String getParameter(System.String paramName)
		{
			return null;
		}
	}
}