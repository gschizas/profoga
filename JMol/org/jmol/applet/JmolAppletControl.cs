/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-03-28 02:43:37 +0200 (mar., 28 mars 2006) $
* $Revision: 4796 $
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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Graphics3D = org.jmol.g3d.Graphics3D;
namespace org.jmol.applet
{
	
	//UPGRADE_TODO: Class 'java.applet.Applet' was converted to 'System.Windows.Forms.UserControl' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaappletApplet'"
	[Serializable]
	public class JmolAppletControl:System.Windows.Forms.UserControl
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
		public bool isActiveVar = true;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'parameterInfo'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private static readonly System.String[][] parameterInfo = new System.String[][]{new System.String[]{"foo", "bar,baz,biz", "the description"}};
		
		public System.String GetUserControlInfo()
		{
			return "JmolAppletControl ... see jmol.sourceforge.net";
		}
		
		public System.String[][] GetParameterInfo()
		{
			return parameterInfo;
		}
		
		private const int typeChimePush = 0;
		private const int typeChimeToggle = 1;
		private const int typeChimeRadio = 2;
		private const int typeButton = 3;
		private const int typeCheckbox = 4;
		private const int typeImmediate = 5;
		
		// put these in lower case
		//UPGRADE_NOTE: Final was removed from the declaration of 'typeNames'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private static readonly System.String[] typeNames = new System.String[]{"chimepush", "chimetoggle", "chimeradio", "button", "checkbox", "immediate"};
		
		internal System.String myName;
		internal bool mayScript;
		internal JmolAppletRegistry appletRegistry;
		//UPGRADE_ISSUE: Interface 'java.applet.AppletContext' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaappletAppletContext'"
		internal AppletContext context;
		internal System.String targetName;
		internal System.String typeName;
		internal int type;
		internal int width;
		internal int height;
		internal System.Drawing.Color colorBackground
		{
			get
			{
				return colorBackground_Renamed;
			}
			
			set
			{
				colorBackground_Renamed = value;
			}
			
		}
		internal System.Drawing.Color colorBackground_Renamed;
		internal System.Drawing.Color colorForeground
		{
			get
			{
				return colorForeground_Renamed;
			}
			
			set
			{
				colorForeground_Renamed = value;
			}
			
		}
		internal System.Drawing.Color colorForeground_Renamed;
		internal System.String script;
		internal System.String label;
		internal System.String altScript;
		internal System.String buttonCallback;
		
		internal System.String groupName;
		internal bool toggleState;
		
		internal System.Windows.Forms.Button awtButton;
		internal System.Windows.Forms.CheckBox awtCheckbox;
		internal System.Windows.Forms.Control myControl;
		
		private System.String getParam(System.String paramName)
		{
			System.String value_Renamed = getParameter(paramName);
			if (value_Renamed != null)
			{
				value_Renamed = value_Renamed.Trim();
				if (value_Renamed.Length == 0)
					value_Renamed = null;
			}
			return value_Renamed;
		}
		
		private System.String getParamLowerCase(System.String paramName)
		{
			System.String value_Renamed = getParameter(paramName);
			if (value_Renamed != null)
			{
				value_Renamed = value_Renamed.Trim().ToLower();
				if (value_Renamed.Length == 0)
					value_Renamed = null;
			}
			return value_Renamed;
		}
		
		//UPGRADE_TODO: Commented code was moved to the 'InitializeComponent' method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1228'"
		public void  init()
		{
			InitializeComponent();
			//UPGRADE_ISSUE: Method 'java.applet.Applet.getAppletContext' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaappletAppletgetAppletContext'"
			context = getAppletContext();
			myName = getParam("name");
			// note that this needs to be getParameter, not getParam
			// getParameter returns either null or the empty string
			mayScript = getParameter("mayscript") != null;
			appletRegistry = new JmolAppletRegistry(myName, mayScript, this);
			
			targetName = getParam("target");
			typeName = getParamLowerCase("type");
			for (type = typeNames.Length; --type >= 0 && !(typeNames[type].Equals(typeName)); )
			{
			}
			groupName = getParamLowerCase("group");
			System.String buttonState = getParamLowerCase("state");
			toggleState = (buttonState != null && (buttonState.Equals("on") || buttonState.Equals("true") || buttonState.Equals("pushed") || buttonState.Equals("checked") || buttonState.Equals("1")));
			label = getParameter("label"); // don't trim white space from a label
			script = getParam("script");
			altScript = getParam("altScript");
			try
			{
				width = System.Int32.Parse(getParam("width"));
				height = System.Int32.Parse(getParam("height"));
			}
			catch (System.FormatException e)
			{
			}
			System.String colorName;
			colorName = getParam("bgcolor");
			int argbBg = Graphics3D.getArgbFromString(colorName);
			//UPGRADE_TODO: Constructor 'java.awt.Color.Color' was converted to 'System.Drawing.Color.FromArgb' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtColorColor_int'"
			BackColor = argbBg == 0?System.Drawing.Color.White:System.Drawing.Color.FromArgb(argbBg);
			
			colorName = getParam("fgcolor");
			int argbFg = Graphics3D.getArgbFromString(colorName);
			//UPGRADE_TODO: Constructor 'java.awt.Color.Color' was converted to 'System.Drawing.Color.FromArgb' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtColorColor_int'"
			ForeColor = argbFg == 0?System.Drawing.Color.Black:System.Drawing.Color.FromArgb(argbFg);
			
			buttonCallback = getParam("buttoncallback");
			
			
			/*
			//UPGRADE_TODO: Constructor 'java.awt.GridLayout.GridLayout' was converted to 'System.Drawing.Rectangle.Rectangle' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGridLayoutGridLayout_int_int'"
			//UPGRADE_TODO: Class 'java.awt.GridLayout' was converted to 'System.Drawing.Rectangle' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGridLayout'"
			this.Tag = new System.Drawing.Rectangle(1, 1, 0, 0);
			Layout += new System.Windows.Forms.LayoutEventHandler(this.JmolAppletControl_setLayout);*/
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			System.Windows.Forms.Control temp_Control;
			temp_Control = allocateControl();
			Controls.Add(temp_Control);
			logWarnings();
			if (type == typeImmediate)
				runScript();
		}
		
		//UPGRADE_NOTE: The equivalent of method 'java.awt.Component.action' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		//UPGRADE_ISSUE: Class 'java.awt.Event' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
		public bool action(Event e, System.Object what)
		{
			switch (type)
			{
				
				case typeChimeToggle: 
					toggleState = !toggleState;
					awtButton.Text = toggleState?"X":"";
					// fall into;
					goto case typeImmediate;
				
				case typeImmediate: 
				// this is here to facilitate debuggin
				case typeChimePush: 
				case typeButton: 
					runScript();
					break;
				
				case typeChimeRadio: 
					if (!toggleState)
					{
						notifyRadioPeers();
						toggleState = true;
						awtButton.Text = "X";
						runScript();
					}
					break;
				
				case typeCheckbox: 
					if (toggleState != awtCheckbox.Checked)
					{
						if (!toggleState && groupName != null)
							notifyRadioPeers();
						toggleState = !toggleState;
						runScript();
					}
					break;
				}
			return true;
		}
		
		private void  logWarnings()
		{
			if (targetName == null)
				System.Console.Out.WriteLine(typeName + " with no target?");
			if (type == - 1)
				System.Console.Out.WriteLine("unrecognized control type:" + typeName);
			if (type == typeChimeRadio && groupName == null)
				System.Console.Out.WriteLine("chimeRadio with no group name?");
			if (script == null)
				System.Console.Out.WriteLine("control with no script?");
			if (type == typeChimeToggle && altScript == null)
				System.Console.Out.WriteLine("chimeToggle with no altScript?");
		}
		
		private System.Windows.Forms.Control allocateControl()
		{
			switch (type)
			{
				
				case typeChimePush: 
					label = "X";
					// fall into;
					goto case typeButton;
				
				case typeButton: 
					toggleState = true; // so that 'script' will run instead of 'altscript'
					System.Windows.Forms.Button temp_Button;
					temp_Button = new System.Windows.Forms.Button();
					temp_Button.Text = label;
					return awtButton = temp_Button;
				
				case typeChimeToggle: 
				case typeChimeRadio: 
					System.Windows.Forms.Button temp_Button2;
					temp_Button2 = new System.Windows.Forms.Button();
					temp_Button2.Text = toggleState?"X":"";
					return awtButton = temp_Button2;
				
				case typeCheckbox: 
					return awtCheckbox = SupportClass.CheckBoxSupport.CreateCheckBox(label, toggleState);
				
				case typeImmediate: 
					toggleState = true;
					System.Windows.Forms.Button temp_Button3;
					temp_Button3 = new System.Windows.Forms.Button();
					temp_Button3.Text = "immediate";
					return awtButton = temp_Button3;
				}
			System.Windows.Forms.Button temp_Button4;
			temp_Button4 = new System.Windows.Forms.Button();
			temp_Button4.Text = "?";
			return temp_Button4;
		}
		
		private void  notifyRadio(System.String radioGroupName)
		{
			if ((type != typeChimeRadio && type != typeCheckbox) || radioGroupName == null || !radioGroupName.Equals(groupName))
				return ;
			if (toggleState)
			{
				toggleState = false;
				if (type == typeChimeRadio)
					awtButton.Text = "";
				else
					awtCheckbox.Checked = false;
				runScript();
			}
		}
		
		private void  notifyRadioPeers()
		{
			//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
			for (System.Collections.IEnumerator e = appletRegistry.applets(); e.MoveNext(); )
			{
				//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
				System.Object peer = e.Current;
				if (!(peer is JmolAppletControl))
					continue;
				JmolAppletControl controlPeer = (JmolAppletControl) peer;
				controlPeer.notifyRadio(groupName);
			}
		}
		
		private void  runScript()
		{
			System.String scriptToRun = (toggleState?script:altScript);
			if (scriptToRun == null)
				return ;
			if (targetName == null)
			{
				System.Console.Out.WriteLine(typeName + " with name" + myName + " has no target?");
				return ;
			}
			appletRegistry.scriptCallback(targetName, scriptToRun, buttonCallback);
		}
		public void  ResizeControl(System.Drawing.Size p)
		{
			this.Width = p.Width;
			this.Height = p.Height;
		}
		public void  ResizeControl(int p2, int p3)
		{
			this.Width = p2;
			this.Height = p3;
		}
		public System.String  TempDocumentBaseVar = "";
		public virtual System.Uri DocumentBase
		{
			get
			{
				if (TempDocumentBaseVar == "")
					return new System.Uri("http://127.0.0.1");
				else
					return new System.Uri(TempDocumentBaseVar);
			}
			
		}
		public System.Drawing.Image getImage(System.Uri p4)
		{
			Bitmap TemporalyBitmap = new Bitmap(p4.AbsolutePath);
			return (Image) TemporalyBitmap;
		}
		public System.Drawing.Image getImage(System.Uri p5, System.String p6)
		{
			Bitmap TemporalyBitmap = new Bitmap(p5.AbsolutePath + p6);
			return (Image) TemporalyBitmap;
		}
		public virtual System.Boolean isActive()
		{
			return isActiveVar;
		}
		public virtual void  start()
		{
			isActiveVar = true;
		}
		public virtual void  stop()
		{
			isActiveVar = false;
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
		public virtual String getParameter(System.String paramName)
		{
			return null;
		}
		#region Windows Form Designer generated code
		private void  InitializeComponent()
		{
			this.SuspendLayout();
			this.BackColor = Color.LightGray;
			//UPGRADE_TODO: Constructor 'java.awt.GridLayout.GridLayout' was converted to 'System.Drawing.Rectangle.Rectangle' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGridLayoutGridLayout_int_int'"
			//UPGRADE_TODO: Class 'java.awt.GridLayout' was converted to 'System.Drawing.Rectangle' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGridLayout'"
			this.Tag = new System.Drawing.Rectangle(1, 1, 0, 0);
			Layout += new System.Windows.Forms.LayoutEventHandler(this.JmolAppletControl_setLayout);
			this.ResumeLayout(false);
		}
		#endregion
		public void  JmolAppletControl_setLayout(System.Object event_sender, System.Windows.Forms.LayoutEventArgs e)
		{
			SupportClass.GridLayoutResize(event_sender, e);
		}
	}
}