/* $RCSfile$
* $Author: migueljmol $
* $Date: 2005-11-26 00:12:40 +0100 (sam., 26 nov. 2005) $
* $Revision: 4276 $
*
* Copyright (C) 2002-2005  Miguel, Jmol Development, www.jmol.org
*
* Contact: miguel@jmol.org
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
using JmolAppletInterface = org.jmol.api.JmolAppletInterface;
namespace org.jmol.applet
{
	
	public class JmolAppletRegistry
	{
		//UPGRADE_TODO: Class 'netscape.javascript.JSObject' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
		virtual internal JSObject JsoWindow
		{
			get
			{
				//UPGRADE_TODO: Class 'netscape.javascript.JSObject' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
				JSObject jsoWindow = null;
				if (mayScript)
				{
					try
					{
						//UPGRADE_TODO: Method 'netscape.javascript.JSObject.getWindow' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
						jsoWindow = JSObject.getWindow(applet);
					}
					catch (System.Exception e)
					{
						System.Console.Out.WriteLine("exception trying to get jsoWindow");
					}
				}
				else
				{
					System.Console.Out.WriteLine("mayScript not specified for:" + name);
				}
				return jsoWindow;
			}
			
		}
		//UPGRADE_TODO: Class 'netscape.javascript.JSObject' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
		virtual internal JSObject JsoTop
		{
			get
			{
				//UPGRADE_TODO: Class 'netscape.javascript.JSObject' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
				JSObject jsoTop = null;
				//UPGRADE_TODO: Class 'netscape.javascript.JSObject' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
				JSObject jsoWindow = JsoWindow;
				if (jsoWindow != null)
				{
					try
					{
						//UPGRADE_TODO: Method 'netscape.javascript.JSObject.getMember' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
						//UPGRADE_TODO: Class 'netscape.javascript.JSObject' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
						jsoTop = (JSObject) jsoWindow.getMember("top");
					}
					catch (System.Exception e)
					{
						System.Console.Out.WriteLine("exception trying to get window.top");
					}
				}
				return jsoTop;
			}
			
		}
		
		internal System.String name;
		internal bool mayScript;
		//UPGRADE_TODO: Class 'java.applet.Applet' was converted to 'System.Windows.Forms.UserControl' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaappletApplet'"
		internal System.Windows.Forms.UserControl applet;
		//UPGRADE_ISSUE: Interface 'java.applet.AppletContext' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaappletAppletContext'"
		internal AppletContext appletContext;
		internal System.String strJavaVendor, strJavaVersion, strOSName;
		
		//UPGRADE_TODO: Class 'java.applet.Applet' was converted to 'System.Windows.Forms.UserControl' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaappletApplet'"
		public JmolAppletRegistry(System.String name, bool mayScript, System.Windows.Forms.UserControl applet)
		{
			if (name == null || name.Length == 0)
				name = null;
			this.name = name;
			this.mayScript = mayScript;
			this.applet = applet;
			//UPGRADE_ISSUE: Method 'java.applet.Applet.getAppletContext' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaappletAppletgetAppletContext'"
			this.appletContext = applet.getAppletContext();
			//UPGRADE_ISSUE: Method 'java.lang.System.getProperty' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangSystem'"
			strJavaVendor = System_Renamed.getProperty("java.vendor");
			//UPGRADE_ISSUE: Method 'java.lang.System.getProperty' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangSystem'"
			strJavaVersion = System_Renamed.getProperty("java.version");
			//UPGRADE_TODO: Method 'java.lang.System.getProperty' was converted to 'System.Environment.GetEnvironmentVariable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangSystemgetProperty_javalangString'"
			strOSName = System.Environment.GetEnvironmentVariable("OS");
			/*
			if (mayScript) {
			try {
			jsoWindow = JSObject.getWindow(applet);
			System.out.println("JmolAppletRegistry: jsoWindow=" + jsoWindow);
			} catch (Exception e) {
			System.out.println("exception trying to get jsoWindow");
			}
			}
			*/
			checkIn(name, applet);
		}
		
		public virtual System.Collections.IEnumerator applets()
		{
			return htRegistry.Values.GetEnumerator();
		}
		
		private static System.Collections.Hashtable htRegistry = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
		
		//UPGRADE_TODO: Class 'java.applet.Applet' was converted to 'System.Windows.Forms.UserControl' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaappletApplet'"
		internal virtual void  checkIn(System.String name, System.Windows.Forms.UserControl applet)
		{
			System.Console.Out.WriteLine("AppletRegistry.checkIn(" + name + ")");
			if (name != null)
				htRegistry[name] = applet;
		}
		
		public virtual void  script(System.String targetName, System.String script)
		{
			scriptCallback(targetName, script, null);
		}
		
		public virtual void  scriptCallback(System.String targetName, System.String script, System.String callbackJavaScript)
		{
			if (targetName == null || targetName.Length == 0)
			{
				System.Console.Out.WriteLine("no targetName specified");
				return ;
			}
			if (tryDirect(targetName, script, callbackJavaScript))
				return ;
			/*
			if (tryJavaScript(targetName, script, callbackJavaScript))
			return;
			*/
			System.Console.Out.WriteLine("unable to find target:" + targetName);
		}
		
		private bool tryDirect(System.String targetName, System.String script, System.String callbackJavaScript)
		{
			System.Console.Out.WriteLine("tryDirect trying appletContext");
			//UPGRADE_ISSUE: Method 'java.applet.AppletContext.getApplet' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaappletAppletContext'"
			System.Object target = appletContext.getApplet(targetName);
			if (target == null)
			{
				System.Console.Out.WriteLine("... trying registry");
				target = htRegistry[targetName];
			}
			if (target == null)
			{
				System.Console.Out.WriteLine("tryDirect failed to find applet:" + targetName);
				return false;
			}
			if (!(target is JmolAppletInterface))
			{
				System.Console.Out.WriteLine("target " + targetName + " is not a JmolApplet");
				return true;
			}
			JmolAppletInterface targetJmolApplet = (JmolAppletInterface) target;
			targetJmolApplet.scriptButton((callbackJavaScript == null?null:JsoWindow), name, script, callbackJavaScript);
			return true;
		}
		
		/*
		private boolean tryJavaScript(String targetName, String script,
		String callbackJavaScript) {
		if (mayScript) {
		JSObject jsoTop = getJsoTop();
		if (jsoTop != null) {
		try {
		jsoTop.eval(functionRunJmolAppletScript);
		jsoTop.call("runJmolAppletScript",
		new Object[] { targetName, getJsoWindow(), name,
		script, callbackJavaScript });
		return true;
		} catch (Exception e) {
		System.out.println("exception calling JavaScript");
		}
		}
		}
		return false;
		}
		
		final static String functionRunJmolAppletScript=
		// w = win, n = name, t = target, s = script
		"function runJmolAppletScript(t,w,n,s,b){" +
		" function getApplet(w,t){" +
		"  var a;" +
		"  if(w.document.applets!=undefined){" +
		"   a=w.document.applets[t];" +
		"   if (a!=undefined) return a;" +
		"  }" +
		"  var f=w.frames;" +
		"  if(f!=undefined){" +
		"   for(var i=f.length;--i>=0;){" +
		"     a=getApplet(f[i],t);" +
		"     if(a!=undefined) return a;" +
		"   }" +
		"  }" +
		"  return undefined;" +
		" }" +
		" var a=getApplet(w.top,t);" +
		" if (a==undefined){" +
		"  alert('cannot find JmolApplet:' + t);" +
		"  return;" +
		" }" +
		" a.scriptButton(w,n,s,b);" +
		"}\n";
		*/
	}
}