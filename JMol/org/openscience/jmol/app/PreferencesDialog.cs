/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-04-02 16:06:20 +0200 (dim., 02 avr. 2006) $
* $Revision: 4871 $
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
using GT = org.jmol.i18n.GT;
namespace org.openscience.jmol.app
{
	
	[Serializable]
	public class PreferencesDialog:System.Windows.Forms.Form
	{
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassChangeListener' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassChangeListener
		{
			public AnonymousClassChangeListener(PreferencesDialog enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(PreferencesDialog enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private PreferencesDialog enclosingInstance;
			public PreferencesDialog Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			public virtual void  stateChanged(System.Object event_sender, System.EventArgs e)
			{
				
				System.Windows.Forms.TrackBar source = (System.Windows.Forms.TrackBar) event_sender;
				Enclosing_Instance.percentVdwAtom = source.Value;
				Enclosing_Instance.viewer.PercentVdwAtom = Enclosing_Instance.percentVdwAtom;
				Enclosing_Instance.currentProperties[(System.String) "percentVdwAtom"] = (System.String) ("" + Enclosing_Instance.percentVdwAtom);
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassActionListener' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassActionListener
		{
			public AnonymousClassActionListener(PreferencesDialog enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(PreferencesDialog enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private PreferencesDialog enclosingInstance;
			public PreferencesDialog Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			public virtual void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				Enclosing_Instance.viewer.AutoBond = true;
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassActionListener1' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassActionListener1
		{
			public AnonymousClassActionListener1(PreferencesDialog enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(PreferencesDialog enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private PreferencesDialog enclosingInstance;
			public PreferencesDialog Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			public virtual void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				Enclosing_Instance.viewer.AutoBond = false;
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassChangeListener1' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassChangeListener1
		{
			public AnonymousClassChangeListener1(PreferencesDialog enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(PreferencesDialog enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private PreferencesDialog enclosingInstance;
			public PreferencesDialog Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			public virtual void  stateChanged(System.Object event_sender, System.EventArgs e)
			{
				
				System.Windows.Forms.TrackBar source = (System.Windows.Forms.TrackBar) event_sender;
				Enclosing_Instance.marBond = (short) source.Value;
				Enclosing_Instance.viewer.MarBond = Enclosing_Instance.marBond;
				Enclosing_Instance.currentProperties[(System.String) "marBond"] = (System.String) ("" + Enclosing_Instance.marBond);
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassItemListener' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		internal class AnonymousClassItemListener
		{
			public AnonymousClassItemListener(PreferencesDialog enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(PreferencesDialog enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private PreferencesDialog enclosingInstance;
			public PreferencesDialog Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			//Component c;
			//AbstractButton b;
			
			public virtual void  itemStateChanged(System.Object event_sender, System.EventArgs e)
			{
				if (event_sender is System.Windows.Forms.MenuItem)
					((System.Windows.Forms.MenuItem) event_sender).Checked = !((System.Windows.Forms.MenuItem) event_sender).Checked;
				
				System.Windows.Forms.CheckBox cb = (System.Windows.Forms.CheckBox) event_sender;
				System.String key = Enclosing_Instance.guimap.getKey(cb);
				bool isSelected = cb.Checked;
				System.String strSelected = isSelected?"true":"false";
				if (key.Equals("Prefs.showHydrogens"))
				{
					Enclosing_Instance.showHydrogens = isSelected;
					Enclosing_Instance.viewer.ShowHydrogens = Enclosing_Instance.showHydrogens;
					Enclosing_Instance.currentProperties[(System.String) "showHydrogens"] = (System.String) strSelected;
				}
				else if (key.Equals("Prefs.showMeasurements"))
				{
					Enclosing_Instance.showMeasurements = isSelected;
					Enclosing_Instance.viewer.ShowMeasurements = Enclosing_Instance.showMeasurements;
					Enclosing_Instance.currentProperties[(System.String) "showMeasurements"] = (System.String) strSelected;
				}
				else if (key.Equals("Prefs.perspectiveDepth"))
				{
					Enclosing_Instance.perspectiveDepth = isSelected;
					Enclosing_Instance.viewer.PerspectiveDepth = Enclosing_Instance.perspectiveDepth;
					Enclosing_Instance.currentProperties[(System.String) "perspectiveDepth"] = (System.String) strSelected;
				}
				else if (key.Equals("Prefs.showAxes"))
				{
					Enclosing_Instance.showAxes = isSelected;
					Enclosing_Instance.viewer.ShowAxes = isSelected;
					Enclosing_Instance.currentProperties[(System.String) "showAxes"] = (System.String) strSelected;
				}
				else if (key.Equals("Prefs.showBoundingBox"))
				{
					Enclosing_Instance.showBoundingBox = isSelected;
					Enclosing_Instance.viewer.ShowBbcage = isSelected;
					Enclosing_Instance.currentProperties[(System.String) "showBoundingBox"] = (System.String) strSelected;
				}
				else if (key.Equals("Prefs.axesOrientationRasmol"))
				{
					Enclosing_Instance.axesOrientationRasmol = isSelected;
					Enclosing_Instance.viewer.AxesOrientationRasmol = isSelected;
					Enclosing_Instance.currentProperties[(System.String) "axesOrientationRasmol"] = (System.String) strSelected;
				}
				else if (key.Equals("Prefs.openFilePreview"))
				{
					Enclosing_Instance.openFilePreview = isSelected;
					Enclosing_Instance.currentProperties[(System.String) "openFilePreview"] = (System.String) strSelected;
				}
			}
		}
		private void  InitBlock()
		{
			prefsAction = new PrefsAction(this);
			checkBoxListener = new AnonymousClassItemListener(this);
		}
		virtual public SupportClass.ActionSupport[] Actions
		{
			get
			{
				SupportClass.ActionSupport[] defaultActions = new SupportClass.ActionSupport[]{prefsAction};
				return defaultActions;
			}
			
		}
		
		private bool autoBond;
		internal bool showHydrogens;
		internal bool showMeasurements;
		internal bool perspectiveDepth;
		internal bool showAxes;
		internal bool showBoundingBox;
		internal bool axesOrientationRasmol;
		internal bool openFilePreview;
		internal short marBond;
		internal int percentVdwAtom;
		internal System.Windows.Forms.Button bButton, pButton, tButton, eButton, vButton;
		private System.Windows.Forms.RadioButton abYes, abNo;
		private System.Windows.Forms.TrackBar vdwPercentSlider;
		private System.Windows.Forms.TrackBar bwSlider;
		private System.Windows.Forms.CheckBox cH, cM;
		private System.Windows.Forms.CheckBox cbPerspectiveDepth;
		private System.Windows.Forms.CheckBox cbShowAxes, cbShowBoundingBox;
		private System.Windows.Forms.CheckBox cbAxesOrientationRasmol;
		private System.Windows.Forms.CheckBox cbOpenFilePreview;
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		private System.Collections.Specialized.NameValueCollection originalSystemProperties;
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		private System.Collections.Specialized.NameValueCollection jmolDefaultProperties;
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		internal System.Collections.Specialized.NameValueCollection currentProperties;
		
		// The actions:
		
		//UPGRADE_NOTE: The initialization of  'prefsAction' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private PrefsAction prefsAction;
		private System.Collections.Hashtable commands;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'jmolDefaults'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] jmolDefaults = new System.String[]{"jmolDefaults", "true", "showHydrogens", "true", "showMeasurements", "true", "perspectiveDepth", "true", "showAxes", "false", "showBoundingBox", "false", "axesOrientationRasmol", "false", "openFilePreview", "true", "percentVdwAtom", "20", "autoBond", "true", "marBond", "150"};
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'rasmolOverrides'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] rasmolOverrides = new System.String[]{"jmolDefaults", "false", "percentVdwAtom", "0", "marBond", "1", "axesOrientationRasmol", "true"};
		
		internal JmolViewer viewer;
		internal GuiMap guimap;
		
		public PreferencesDialog(System.Windows.Forms.Form f, GuiMap guimap, JmolViewer viewer):base()
		{
			InitBlock();
			//UPGRADE_TODO: Constructor 'javax.swing.JDialog.JDialog' was converted to 'SupportClass.DialogSupport.SetDialog' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJDialogJDialog_javaawtFrame_boolean'"
			SupportClass.DialogSupport.SetDialog(this, f);
			this.guimap = guimap;
			this.viewer = viewer;
			
			initializeProperties();
			
			this.Text = GT._("Preferences");
			
			initVariables();
			commands = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
			SupportClass.ActionSupport[] actions = Actions;
			for (int i = 0; i < actions.Length; i++)
			{
				SupportClass.ActionSupport a = actions[i];
				//UPGRADE_ISSUE: Method 'javax.swing.Action.getValue' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingActiongetValue_javalangString'"
				//UPGRADE_ISSUE: Field 'javax.swing.Action.NAME' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingActionNAME_f'"
				commands[a.getValue(Action.NAME)] = a;
			}
			System.Windows.Forms.Panel container = new System.Windows.Forms.Panel();
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtBorderLayout'"
			/*
			container.setLayout(new BorderLayout());*/
			
			System.Windows.Forms.TabControl tabs = new System.Windows.Forms.TabControl();
			System.Windows.Forms.Panel disp = buildDispPanel();
			System.Windows.Forms.Panel atoms = buildAtomsPanel();
			System.Windows.Forms.Panel bonds = buildBondPanel();
			//    JPanel vibrate = buildVibratePanel();
			SupportClass.TabControlSupport.AddTab(tabs, GT._("Display"), disp);
			SupportClass.TabControlSupport.AddTab(tabs, GT._("Atoms"), atoms);
			SupportClass.TabControlSupport.AddTab(tabs, GT._("Bonds"), bonds);
			
			System.Windows.Forms.Panel buttonPanel = new System.Windows.Forms.Panel();
			//UPGRADE_TODO: Constructor 'java.awt.FlowLayout.FlowLayout' was converted to 'System.Object[]' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtFlowLayoutFlowLayout_int'"
			buttonPanel.Tag = new System.Object[]{(int) System.Drawing.ContentAlignment.TopRight, 5, 5};
			buttonPanel.Layout += new System.Windows.Forms.LayoutEventHandler(SupportClass.FlowLayoutResize);
			
			jmolDefaultsButton = SupportClass.ButtonSupport.CreateStandardButton(GT._("Jmol Defaults"));
			jmolDefaultsButton.Click += new System.EventHandler(this.actionPerformed);
			SupportClass.CommandManager.CheckCommand(jmolDefaultsButton);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			buttonPanel.Controls.Add(jmolDefaultsButton);
			
			rasmolDefaultsButton = SupportClass.ButtonSupport.CreateStandardButton(GT._("RasMol Defaults"));
			rasmolDefaultsButton.Click += new System.EventHandler(this.actionPerformed);
			SupportClass.CommandManager.CheckCommand(rasmolDefaultsButton);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			buttonPanel.Controls.Add(rasmolDefaultsButton);
			
			cancelButton = SupportClass.ButtonSupport.CreateStandardButton(GT._("Cancel"));
			cancelButton.Click += new System.EventHandler(this.actionPerformed);
			SupportClass.CommandManager.CheckCommand(cancelButton);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			buttonPanel.Controls.Add(cancelButton);
			
			applyButton = SupportClass.ButtonSupport.CreateStandardButton(GT._("Apply"));
			applyButton.Click += new System.EventHandler(this.actionPerformed);
			SupportClass.CommandManager.CheckCommand(applyButton);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			buttonPanel.Controls.Add(applyButton);
			
			okButton = SupportClass.ButtonSupport.CreateStandardButton(GT._("OK"));
			okButton.Click += new System.EventHandler(this.actionPerformed);
			SupportClass.CommandManager.CheckCommand(okButton);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			buttonPanel.Controls.Add(okButton);
			//UPGRADE_TODO: Method 'javax.swing.JRootPane.setDefaultButton' was converted to 'System.Windows.Forms.Form.AcceptButton' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJRootPanesetDefaultButton_javaxswingJButton'"
			okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.AcceptButton = okButton;
			
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			container.Controls.Add(tabs);
			tabs.Dock = System.Windows.Forms.DockStyle.Fill;
			tabs.BringToFront();
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			container.Controls.Add(buttonPanel);
			buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			buttonPanel.SendToBack();
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			((System.Windows.Forms.ContainerControl) this).Controls.Add(container);
			
			updateComponents();
			
			//UPGRADE_ISSUE: Method 'java.awt.Window.pack' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtWindowpack'"
			pack();
			centerDialog();
		}
		
		public virtual System.Windows.Forms.Panel buildDispPanel()
		{
			
			System.Windows.Forms.Panel disp = new System.Windows.Forms.Panel();
			//UPGRADE_ISSUE: Class 'java.awt.GridBagLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagLayout'"
			//UPGRADE_ISSUE: Constructor 'java.awt.GridBagLayout.GridBagLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagLayout'"
			GridBagLayout gridbag = new GridBagLayout();
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			disp.setLayout(gridbag);
			//UPGRADE_ISSUE: Class 'java.awt.GridBagConstraints' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			GridBagConstraints constraints;
			
			System.Windows.Forms.Panel showPanel = new System.Windows.Forms.Panel();
			//UPGRADE_TODO: Constructor 'java.awt.GridLayout.GridLayout' was converted to 'System.Drawing.Rectangle.Rectangle' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGridLayoutGridLayout_int_int'"
			//UPGRADE_TODO: Class 'java.awt.GridLayout' was converted to 'System.Drawing.Rectangle' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGridLayout'"
			showPanel.Tag = new System.Drawing.Rectangle(1, 3, 0, 0);
			showPanel.Layout += new System.Windows.Forms.LayoutEventHandler(SupportClass.GridLayoutResize);
			//UPGRADE_TODO: Method 'javax.swing.JComponent.setBorder' was converted to 'System.Windows.Forms.ControlPaint.DrawBorder3D' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJComponentsetBorder_javaxswingborderBorder'"
			//UPGRADE_ISSUE: Constructor 'javax.swing.border.TitledBorder.TitledBorder' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingborderTitledBorder'"
			System.Windows.Forms.ControlPaint.DrawBorder3D(showPanel.CreateGraphics(), 0, 0, showPanel.Width, showPanel.Height, new TitledBorder(GT._("Show All")));
			cH = guimap.newJCheckBox("Prefs.showHydrogens", viewer.ShowHydrogens);
			cH.CheckedChanged += new System.EventHandler(checkBoxListener.itemStateChanged);
			cM = guimap.newJCheckBox("Prefs.showMeasurements", viewer.ShowMeasurements);
			cM.CheckedChanged += new System.EventHandler(checkBoxListener.itemStateChanged);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			showPanel.Controls.Add(cH);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			showPanel.Controls.Add(cM);
			
			//UPGRADE_ISSUE: Constructor 'java.awt.GridBagConstraints.GridBagConstraints' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			constraints = new GridBagConstraints();
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridwidth' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.REMAINDER' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			constraints.gridwidth = GridBagConstraints.REMAINDER;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.fill' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.HORIZONTAL' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			constraints.fill = GridBagConstraints.HORIZONTAL;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.weightx' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			constraints.weightx = 1.0;
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			disp.Controls.Add(showPanel);
			showPanel.Dock = new System.Windows.Forms.DockStyle();
			showPanel.BringToFront();
			
			System.Windows.Forms.Panel fooPanel = new System.Windows.Forms.Panel();
			//UPGRADE_TODO: Method 'javax.swing.JComponent.setBorder' was converted to 'System.Windows.Forms.ControlPaint.DrawBorder3D' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJComponentsetBorder_javaxswingborderBorder'"
			//UPGRADE_ISSUE: Constructor 'javax.swing.border.TitledBorder.TitledBorder' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingborderTitledBorder'"
			System.Windows.Forms.ControlPaint.DrawBorder3D(fooPanel.CreateGraphics(), 0, 0, fooPanel.Width, fooPanel.Height, new TitledBorder(""));
			//UPGRADE_TODO: Constructor 'java.awt.GridLayout.GridLayout' was converted to 'System.Drawing.Rectangle.Rectangle' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGridLayoutGridLayout_int_int'"
			//UPGRADE_TODO: Class 'java.awt.GridLayout' was converted to 'System.Drawing.Rectangle' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGridLayout'"
			fooPanel.Tag = new System.Drawing.Rectangle(2, 1, 0, 0);
			fooPanel.Layout += new System.Windows.Forms.LayoutEventHandler(SupportClass.GridLayoutResize);
			
			cbPerspectiveDepth = guimap.newJCheckBox("Prefs.perspectiveDepth", viewer.PerspectiveDepth);
			cbPerspectiveDepth.CheckedChanged += new System.EventHandler(checkBoxListener.itemStateChanged);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			fooPanel.Controls.Add(cbPerspectiveDepth);
			
			cbShowAxes = guimap.newJCheckBox("Prefs.showAxes", viewer.ShowAxes);
			cbShowAxes.CheckedChanged += new System.EventHandler(checkBoxListener.itemStateChanged);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			fooPanel.Controls.Add(cbShowAxes);
			
			cbShowBoundingBox = guimap.newJCheckBox("Prefs.showBoundingBox", viewer.ShowBbcage);
			cbShowBoundingBox.CheckedChanged += new System.EventHandler(checkBoxListener.itemStateChanged);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			fooPanel.Controls.Add(cbShowBoundingBox);
			
			//UPGRADE_ISSUE: Constructor 'java.awt.GridBagConstraints.GridBagConstraints' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			constraints = new GridBagConstraints();
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridwidth' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.REMAINDER' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			constraints.gridwidth = GridBagConstraints.REMAINDER;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.fill' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.HORIZONTAL' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			constraints.fill = GridBagConstraints.HORIZONTAL;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.weightx' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			constraints.weightx = 1.0;
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			disp.Controls.Add(fooPanel);
			fooPanel.Dock = new System.Windows.Forms.DockStyle();
			fooPanel.BringToFront();
			
			System.Windows.Forms.Panel axesPanel = new System.Windows.Forms.Panel();
			//UPGRADE_TODO: Method 'javax.swing.JComponent.setBorder' was converted to 'System.Windows.Forms.ControlPaint.DrawBorder3D' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJComponentsetBorder_javaxswingborderBorder'"
			//UPGRADE_ISSUE: Constructor 'javax.swing.border.TitledBorder.TitledBorder' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingborderTitledBorder'"
			System.Windows.Forms.ControlPaint.DrawBorder3D(axesPanel.CreateGraphics(), 0, 0, axesPanel.Width, axesPanel.Height, new TitledBorder(""));
			//UPGRADE_TODO: Constructor 'java.awt.GridLayout.GridLayout' was converted to 'System.Drawing.Rectangle.Rectangle' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGridLayoutGridLayout_int_int'"
			//UPGRADE_TODO: Class 'java.awt.GridLayout' was converted to 'System.Drawing.Rectangle' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGridLayout'"
			axesPanel.Tag = new System.Drawing.Rectangle(1, 1, 0, 0);
			axesPanel.Layout += new System.Windows.Forms.LayoutEventHandler(SupportClass.GridLayoutResize);
			
			cbAxesOrientationRasmol = guimap.newJCheckBox("Prefs.axesOrientationRasmol", viewer.AxesOrientationRasmol);
			cbAxesOrientationRasmol.CheckedChanged += new System.EventHandler(checkBoxListener.itemStateChanged);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			axesPanel.Controls.Add(cbAxesOrientationRasmol);
			
			//UPGRADE_ISSUE: Constructor 'java.awt.GridBagConstraints.GridBagConstraints' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			constraints = new GridBagConstraints();
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridwidth' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.REMAINDER' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			constraints.gridwidth = GridBagConstraints.REMAINDER;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.fill' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.HORIZONTAL' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			constraints.fill = GridBagConstraints.HORIZONTAL;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.weightx' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			constraints.weightx = 1.0;
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			disp.Controls.Add(axesPanel);
			axesPanel.Dock = new System.Windows.Forms.DockStyle();
			axesPanel.BringToFront();
			
			System.Windows.Forms.Panel otherPanel = new System.Windows.Forms.Panel();
			//UPGRADE_TODO: Method 'javax.swing.JComponent.setBorder' was converted to 'System.Windows.Forms.ControlPaint.DrawBorder3D' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJComponentsetBorder_javaxswingborderBorder'"
			//UPGRADE_ISSUE: Constructor 'javax.swing.border.TitledBorder.TitledBorder' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingborderTitledBorder'"
			System.Windows.Forms.ControlPaint.DrawBorder3D(otherPanel.CreateGraphics(), 0, 0, otherPanel.Width, otherPanel.Height, new TitledBorder(""));
			//UPGRADE_TODO: Constructor 'java.awt.GridLayout.GridLayout' was converted to 'System.Drawing.Rectangle.Rectangle' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGridLayoutGridLayout_int_int'"
			//UPGRADE_TODO: Class 'java.awt.GridLayout' was converted to 'System.Drawing.Rectangle' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGridLayout'"
			otherPanel.Tag = new System.Drawing.Rectangle(1, 1, 0, 0);
			otherPanel.Layout += new System.Windows.Forms.LayoutEventHandler(SupportClass.GridLayoutResize);
			
			cbOpenFilePreview = guimap.newJCheckBox("Prefs.openFilePreview", openFilePreview);
			cbOpenFilePreview.CheckedChanged += new System.EventHandler(checkBoxListener.itemStateChanged);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			otherPanel.Controls.Add(cbOpenFilePreview);
			
			//UPGRADE_ISSUE: Constructor 'java.awt.GridBagConstraints.GridBagConstraints' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			constraints = new GridBagConstraints();
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridwidth' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.REMAINDER' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			constraints.gridwidth = GridBagConstraints.REMAINDER;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.fill' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.HORIZONTAL' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			constraints.fill = GridBagConstraints.HORIZONTAL;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.weightx' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			constraints.weightx = 1.0;
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			disp.Controls.Add(otherPanel);
			otherPanel.Dock = new System.Windows.Forms.DockStyle();
			otherPanel.BringToFront();
			
			
			System.Windows.Forms.Label filler = new System.Windows.Forms.Label();
			//UPGRADE_ISSUE: Constructor 'java.awt.GridBagConstraints.GridBagConstraints' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			constraints = new GridBagConstraints();
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridwidth' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.REMAINDER' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			constraints.gridwidth = GridBagConstraints.REMAINDER;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridheight' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.REMAINDER' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			constraints.gridheight = GridBagConstraints.REMAINDER;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.fill' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.BOTH' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			constraints.fill = GridBagConstraints.BOTH;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.weightx' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			constraints.weightx = 1.0;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.weighty' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			constraints.weighty = 1.0;
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			disp.Controls.Add(filler);
			filler.Dock = new System.Windows.Forms.DockStyle();
			filler.BringToFront();
			
			return disp;
		}
		
		public virtual System.Windows.Forms.Panel buildAtomsPanel()
		{
			
			//UPGRADE_ISSUE: Constructor 'java.awt.GridBagLayout.GridBagLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagLayout'"
			new GridBagLayout();
			//UPGRADE_TODO: Constructor 'javax.swing.JPanel.JPanel' was converted to 'System.Windows.Forms.Panel.Panel' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJPanelJPanel_javaawtLayoutManager'"
			System.Windows.Forms.Panel atomPanel = new System.Windows.Forms.Panel();
			//UPGRADE_ISSUE: Class 'java.awt.GridBagConstraints' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			GridBagConstraints constraints;
			
			System.Windows.Forms.Panel sfPanel = new System.Windows.Forms.Panel();
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtBorderLayout'"
			/*
			sfPanel.setLayout(new BorderLayout());*/
			//UPGRADE_TODO: Method 'javax.swing.JComponent.setBorder' was converted to 'System.Windows.Forms.ControlPaint.DrawBorder3D' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJComponentsetBorder_javaxswingborderBorder'"
			//UPGRADE_ISSUE: Constructor 'javax.swing.border.TitledBorder.TitledBorder' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingborderTitledBorder'"
			System.Windows.Forms.ControlPaint.DrawBorder3D(sfPanel.CreateGraphics(), 0, 0, sfPanel.Width, sfPanel.Height, new TitledBorder(GT._("Default atom size")));
			System.Windows.Forms.Label temp_label;
			//UPGRADE_TODO: The equivalent in .NET for field 'javax.swing.SwingConstants.CENTER' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			temp_label = new System.Windows.Forms.Label();
			temp_label.Text = GT._("(percentage of vanDerWaals radius)");
			temp_label.ImageAlign = (System.Drawing.ContentAlignment) System.Windows.Forms.HorizontalAlignment.Center;
			temp_label.TextAlign = (System.Drawing.ContentAlignment) System.Windows.Forms.HorizontalAlignment.Center;
			System.Windows.Forms.Label sfLabel = temp_label;
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			sfPanel.Controls.Add(sfLabel);
			sfLabel.Dock = System.Windows.Forms.DockStyle.Top;
			sfLabel.SendToBack();
			//UPGRADE_TODO: The equivalent in .NET for field 'javax.swing.SwingConstants.HORIZONTAL' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			vdwPercentSlider = SupportClass.TrackBarSupport.CreateTrackBar(System.Windows.Forms.Orientation.Horizontal, 0, 100, viewer.PercentVdwAtom);
			//UPGRADE_ISSUE: Method 'javax.swing.JComponent.putClientProperty' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJComponentputClientProperty_javalangObject_javalangObject'"
			vdwPercentSlider.putClientProperty("JSlider.isFilled", true);
			vdwPercentSlider.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
			//UPGRADE_ISSUE: Method 'javax.swing.JSlider.setMajorTickSpacing' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJSlidersetMajorTickSpacing_int'"
			vdwPercentSlider.setMajorTickSpacing(20);
			vdwPercentSlider.TickFrequency = 10;
			//UPGRADE_ISSUE: Method 'javax.swing.JSlider.setPaintLabels' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJSlidersetPaintLabels_boolean'"
			vdwPercentSlider.setPaintLabels(true);
			vdwPercentSlider.ValueChanged += new System.EventHandler(new AnonymousClassChangeListener(this).stateChanged);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			sfPanel.Controls.Add(vdwPercentSlider);
			vdwPercentSlider.Dock = System.Windows.Forms.DockStyle.Fill;
			vdwPercentSlider.BringToFront();
			//UPGRADE_ISSUE: Constructor 'java.awt.GridBagConstraints.GridBagConstraints' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			constraints = new GridBagConstraints();
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridwidth' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.REMAINDER' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			constraints.gridwidth = GridBagConstraints.REMAINDER;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.fill' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.HORIZONTAL' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			constraints.fill = GridBagConstraints.HORIZONTAL;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.weightx' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			constraints.weightx = 1.0;
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			atomPanel.Controls.Add(sfPanel);
			sfPanel.Dock = new System.Windows.Forms.DockStyle();
			sfPanel.BringToFront();
			
			
			System.Windows.Forms.Label filler = new System.Windows.Forms.Label();
			//UPGRADE_ISSUE: Constructor 'java.awt.GridBagConstraints.GridBagConstraints' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			constraints = new GridBagConstraints();
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridwidth' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.REMAINDER' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			constraints.gridwidth = GridBagConstraints.REMAINDER;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridheight' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.REMAINDER' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			constraints.gridheight = GridBagConstraints.REMAINDER;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.fill' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.BOTH' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			constraints.fill = GridBagConstraints.BOTH;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.weightx' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			constraints.weightx = 1.0;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.weighty' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			constraints.weighty = 1.0;
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			atomPanel.Controls.Add(filler);
			filler.Dock = new System.Windows.Forms.DockStyle();
			filler.BringToFront();
			
			return atomPanel;
		}
		
		public virtual System.Windows.Forms.Panel buildBondPanel()
		{
			
			System.Windows.Forms.Panel bondPanel = new System.Windows.Forms.Panel();
			//UPGRADE_ISSUE: Class 'java.awt.GridBagLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagLayout'"
			//UPGRADE_ISSUE: Constructor 'java.awt.GridBagLayout.GridBagLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagLayout'"
			GridBagLayout gridbag = new GridBagLayout();
			//UPGRADE_ISSUE: Class 'java.awt.GridBagConstraints' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			//UPGRADE_ISSUE: Constructor 'java.awt.GridBagConstraints.GridBagConstraints' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			GridBagConstraints c = new GridBagConstraints();
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			bondPanel.setLayout(gridbag);
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.fill' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.BOTH' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			c.fill = GridBagConstraints.BOTH;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.weightx' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			c.weightx = 1.0;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.weighty' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			c.weighty = 1.0;
			
			System.Windows.Forms.Panel autobondPanel = new System.Windows.Forms.Panel();
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'javax.swing.BoxLayout.BoxLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBoxLayout'"
			//UPGRADE_ISSUE: Field 'javax.swing.BoxLayout.Y_AXIS' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBoxLayout'"
			autobondPanel.setLayout(new BoxLayout(autobondPanel, BoxLayout.Y_AXIS));
			//UPGRADE_TODO: Method 'javax.swing.JComponent.setBorder' was converted to 'System.Windows.Forms.ControlPaint.DrawBorder3D' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJComponentsetBorder_javaxswingborderBorder'"
			//UPGRADE_ISSUE: Constructor 'javax.swing.border.TitledBorder.TitledBorder' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingborderTitledBorder'"
			System.Windows.Forms.ControlPaint.DrawBorder3D(autobondPanel.CreateGraphics(), 0, 0, autobondPanel.Width, autobondPanel.Height, new TitledBorder(GT._("Compute Bonds")));
			//UPGRADE_TODO: Class 'javax.swing.ButtonGroup' was converted to 'System.Collections.ArrayList' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
			System.Collections.ArrayList abGroup = new System.Collections.ArrayList();
			System.Windows.Forms.RadioButton temp_radiobutton;
			temp_radiobutton = new System.Windows.Forms.RadioButton();
			temp_radiobutton.Text = GT._("Automatically");
			abYes = temp_radiobutton;
			System.Windows.Forms.RadioButton temp_radiobutton2;
			temp_radiobutton2 = new System.Windows.Forms.RadioButton();
			temp_radiobutton2.Text = GT._("Don't Compute Bonds");
			abNo = temp_radiobutton2;
			abGroup.Add((System.Object) abYes);
			abGroup.Add((System.Object) abNo);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			autobondPanel.Controls.Add(abYes);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			autobondPanel.Controls.Add(abNo);
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createVerticalGlue' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			System.Windows.Forms.Control temp_Control;
			temp_Control = Box.createVerticalGlue();
			autobondPanel.Controls.Add(temp_Control);
			abYes.Checked = viewer.AutoBond;
			abYes.CheckedChanged += new System.EventHandler(new AnonymousClassActionListener(this).actionPerformed);
			SupportClass.CommandManager.CheckCommand(abYes);
			
			abNo.CheckedChanged += new System.EventHandler(new AnonymousClassActionListener1(this).actionPerformed);
			SupportClass.CommandManager.CheckCommand(abNo);
			
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridwidth' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.REMAINDER' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			c.gridwidth = GridBagConstraints.REMAINDER;
			//UPGRADE_ISSUE: Method 'java.awt.GridBagLayout.setConstraints' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagLayout'"
			gridbag.setConstraints(autobondPanel, c);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			bondPanel.Controls.Add(autobondPanel);
			
			System.Windows.Forms.Panel bwPanel = new System.Windows.Forms.Panel();
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtBorderLayout'"
			/*
			bwPanel.setLayout(new BorderLayout());*/
			//UPGRADE_TODO: Method 'javax.swing.JComponent.setBorder' was converted to 'System.Windows.Forms.ControlPaint.DrawBorder3D' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJComponentsetBorder_javaxswingborderBorder'"
			//UPGRADE_ISSUE: Constructor 'javax.swing.border.TitledBorder.TitledBorder' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingborderTitledBorder'"
			System.Windows.Forms.ControlPaint.DrawBorder3D(bwPanel.CreateGraphics(), 0, 0, bwPanel.Width, bwPanel.Height, new TitledBorder(GT._("Default Bond Radius")));
			System.Windows.Forms.Label temp_label;
			//UPGRADE_TODO: The equivalent in .NET for field 'javax.swing.SwingConstants.CENTER' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			temp_label = new System.Windows.Forms.Label();
			temp_label.Text = GT._("(Angstroms)");
			temp_label.ImageAlign = (System.Drawing.ContentAlignment) System.Windows.Forms.HorizontalAlignment.Center;
			temp_label.TextAlign = (System.Drawing.ContentAlignment) System.Windows.Forms.HorizontalAlignment.Center;
			System.Windows.Forms.Label bwLabel = temp_label;
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			bwPanel.Controls.Add(bwLabel);
			bwLabel.Dock = System.Windows.Forms.DockStyle.Top;
			bwLabel.SendToBack();
			
			bwSlider = SupportClass.TrackBarSupport.CreateTrackBar(0, 250, viewer.MadBond / 2);
			//UPGRADE_ISSUE: Method 'javax.swing.JComponent.putClientProperty' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJComponentputClientProperty_javalangObject_javalangObject'"
			bwSlider.putClientProperty("JSlider.isFilled", true);
			bwSlider.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
			//UPGRADE_ISSUE: Method 'javax.swing.JSlider.setMajorTickSpacing' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJSlidersetMajorTickSpacing_int'"
			bwSlider.setMajorTickSpacing(50);
			bwSlider.TickFrequency = 25;
			//UPGRADE_ISSUE: Method 'javax.swing.JSlider.setPaintLabels' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJSlidersetPaintLabels_boolean'"
			bwSlider.setPaintLabels(true);
			for (int i = 0; i <= 250; i += 50)
			{
				System.String label = "" + (1000 + i);
				label = "0." + label.Substring(1);
				//UPGRADE_ISSUE: Method 'javax.swing.JSlider.getLabelTable' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJSlidergetLabelTable'"
				System.Windows.Forms.Label temp_label2;
				//UPGRADE_TODO: The equivalent in .NET for field 'javax.swing.SwingConstants.CENTER' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				temp_label2 = new System.Windows.Forms.Label();
				temp_label2.Text = label;
				temp_label2.ImageAlign = (System.Drawing.ContentAlignment) System.Windows.Forms.HorizontalAlignment.Center;
				temp_label2.TextAlign = (System.Drawing.ContentAlignment) System.Windows.Forms.HorizontalAlignment.Center;
				bwSlider.getLabelTable()[(System.Int32) i] = temp_label2;
				//UPGRADE_ISSUE: Method 'javax.swing.JSlider.setLabelTable' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJSlidersetLabelTable_javautilDictionary'"
				//UPGRADE_ISSUE: Method 'javax.swing.JSlider.getLabelTable' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJSlidergetLabelTable'"
				bwSlider.setLabelTable(bwSlider.getLabelTable());
			}
			bwSlider.ValueChanged += new System.EventHandler(new AnonymousClassChangeListener1(this).stateChanged);
			
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			bwPanel.Controls.Add(bwSlider);
			bwSlider.Dock = System.Windows.Forms.DockStyle.Bottom;
			bwSlider.SendToBack();
			
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.weightx' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			c.weightx = 0.0;
			//UPGRADE_ISSUE: Method 'java.awt.GridBagLayout.setConstraints' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagLayout'"
			gridbag.setConstraints(bwPanel, c);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			bondPanel.Controls.Add(bwPanel);
			
			return bondPanel;
		}
		
		protected internal virtual void  centerDialog()
		{
			
			//UPGRADE_ISSUE: Method 'java.awt.Window.getToolkit' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtWindowgetToolkit'"
			this.getToolkit();
			System.Drawing.Size screenSize = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size;
			System.Drawing.Size size = this.Size;
			screenSize.Height = screenSize.Height / 2;
			screenSize.Width = screenSize.Width / 2;
			size.Height = size.Height / 2;
			size.Width = size.Width / 2;
			int y = screenSize.Height - size.Height;
			int x = screenSize.Width - size.Width;
			//UPGRADE_TODO: Method 'java.awt.Component.setLocation' was converted to 'System.Windows.Forms.Control.Location' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetLocation_int_int'"
			this.Location = new System.Drawing.Point(x, y);
		}
		
		public virtual void  ok()
		{
			save();
			Dispose();
		}
		
		public virtual void  cancel()
		{
			updateComponents();
			Dispose();
		}
		
		private void  updateComponents()
		{
			// Display panel
			cH.Checked = viewer.ShowHydrogens;
			cM.Checked = viewer.ShowMeasurements;
			
			cbPerspectiveDepth.Checked = viewer.PerspectiveDepth;
			cbShowAxes.Checked = viewer.ShowAxes;
			cbShowBoundingBox.Checked = viewer.ShowBbcage;
			
			cbAxesOrientationRasmol.Checked = viewer.AxesOrientationRasmol;
			
			cbOpenFilePreview.Checked = openFilePreview;
			
			// Atom panel controls: 
			vdwPercentSlider.Value = viewer.PercentVdwAtom;
			
			// Bond panel controls:
			abYes.Checked = viewer.AutoBond;
			bwSlider.Value = viewer.MadBond / 2;
		}
		
		private void  save()
		{
			try
			{
				//UPGRADE_TODO: Constructor 'java.io.FileOutputStream.FileOutputStream' was converted to 'System.IO.FileStream.FileStream' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileOutputStreamFileOutputStream_javaioFile'"
				System.IO.FileStream fileOutputStream = new System.IO.FileStream(Jmol.UserPropsFile.FullName, System.IO.FileMode.Create);
				//UPGRADE_ISSUE: Method 'java.util.Properties.store' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilPropertiesstore_javaioOutputStream_javalangString'"
				currentProperties.store(fileOutputStream, "Jmol");
				fileOutputStream.Close();
			}
			catch (System.Exception e)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				System.Console.Out.WriteLine("Error saving preferences" + e);
			}
			viewer.refresh();
		}
		
		internal virtual void  initializeProperties()
		{
			//UPGRADE_TODO: Method 'java.lang.System.getProperties' was converted to 'SupportClass.GetProperties' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangSystemgetProperties'"
			originalSystemProperties = SupportClass.GetProperties();
			//UPGRADE_TODO: Format of property file may need to be changed. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1089'"
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			jmolDefaultProperties = new System.Collections.Specialized.NameValueCollection(originalSystemProperties);
			for (int i = jmolDefaults.Length; (i -= 2) >= 0; )
				jmolDefaultProperties[(System.String) jmolDefaults[i]] = (System.String) jmolDefaults[i + 1];
			//UPGRADE_TODO: Format of property file may need to be changed. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1089'"
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			currentProperties = new System.Collections.Specialized.NameValueCollection(jmolDefaultProperties);
			try
			{
				//UPGRADE_TODO: Constructor 'java.io.FileInputStream.FileInputStream' was converted to 'System.IO.FileStream.FileStream' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileInputStreamFileInputStream_javaioFile'"
				System.IO.FileStream fis2 = new System.IO.FileStream(Jmol.UserPropsFile.FullName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
				new System.IO.BufferedStream(fis2, 1024);
				//UPGRADE_TODO: Method 'java.util.Properties.load' was converted to 'System.Collections.Specialized.NameValueCollection' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiesload_javaioInputStream'"
				currentProperties = new System.Collections.Specialized.NameValueCollection(System.Configuration.ConfigurationSettings.AppSettings);
				fis2.Close();
			}
			catch (System.Exception e2)
			{
			}
			//UPGRADE_ISSUE: Method 'java.lang.System.setProperties' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangSystem'"
			System_Renamed.setProperties(currentProperties);
		}
		
		internal virtual void  resetDefaults(System.String[] overrides)
		{
			//UPGRADE_TODO: Format of property file may need to be changed. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1089'"
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			currentProperties = new System.Collections.Specialized.NameValueCollection(jmolDefaultProperties);
			//UPGRADE_ISSUE: Method 'java.lang.System.setProperties' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangSystem'"
			System_Renamed.setProperties(currentProperties);
			if (overrides != null)
			{
				for (int i = overrides.Length; (i -= 2) >= 0; )
					currentProperties[(System.String) overrides[i]] = (System.String) overrides[i + 1];
			}
			initVariables();
			viewer.refresh();
			updateComponents();
		}
		
		internal virtual void  initVariables()
		{
			
			//UPGRADE_ISSUE: Method 'java.lang.Boolean.getBoolean' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangBooleangetBoolean_javalangString'"
			autoBond = Boolean.getBoolean("autoBond");
			//UPGRADE_ISSUE: Method 'java.lang.Boolean.getBoolean' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangBooleangetBoolean_javalangString'"
			showHydrogens = Boolean.getBoolean("showHydrogens");
			//showVectors = Boolean.getBoolean("showVectors");
			//UPGRADE_ISSUE: Method 'java.lang.Boolean.getBoolean' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangBooleangetBoolean_javalangString'"
			showMeasurements = Boolean.getBoolean("showMeasurements");
			//UPGRADE_ISSUE: Method 'java.lang.Boolean.getBoolean' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangBooleangetBoolean_javalangString'"
			perspectiveDepth = Boolean.getBoolean("perspectiveDepth");
			//UPGRADE_ISSUE: Method 'java.lang.Boolean.getBoolean' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangBooleangetBoolean_javalangString'"
			showAxes = Boolean.getBoolean("showAxes");
			//UPGRADE_ISSUE: Method 'java.lang.Boolean.getBoolean' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangBooleangetBoolean_javalangString'"
			showBoundingBox = Boolean.getBoolean("showBoundingBox");
			//UPGRADE_ISSUE: Method 'java.lang.Boolean.getBoolean' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangBooleangetBoolean_javalangString'"
			axesOrientationRasmol = Boolean.getBoolean("axesOrientationRasmol");
			//UPGRADE_NOTE: Exceptions thrown by the equivalent in .NET of method 'java.lang.Boolean.valueOf' may be different. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1099'"
			//UPGRADE_ISSUE: Method 'java.lang.System.getProperty' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangSystem'"
			openFilePreview = System.Boolean.Parse(System_Renamed.getProperty("openFilePreview", "true"));
			
			marBond = System.Int16.Parse(currentProperties.Get("marBond"));
			percentVdwAtom = System.Int32.Parse(currentProperties.Get("percentVdwAtom"));
			
			//UPGRADE_ISSUE: Method 'java.lang.Boolean.getBoolean' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangBooleangetBoolean_javalangString'"
			if (Boolean.getBoolean("jmolDefaults"))
				viewer.setJmolDefaults();
			else
				viewer.setRasmolDefaults();
			
			viewer.PercentVdwAtom = percentVdwAtom;
			viewer.MarBond = marBond;
			viewer.AutoBond = autoBond;
			viewer.ShowHydrogens = showHydrogens;
			viewer.ShowMeasurements = showMeasurements;
			viewer.PerspectiveDepth = perspectiveDepth;
			viewer.ShowAxes = showAxes;
			viewer.ShowBbcage = showBoundingBox;
			viewer.AxesOrientationRasmol = axesOrientationRasmol;
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'PrefsAction' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class PrefsAction:SupportClass.ActionSupport
		{
			private void  InitBlock(PreferencesDialog enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private PreferencesDialog enclosingInstance;
			public PreferencesDialog Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			//UPGRADE_TODO: Constructor 'javax.swing.AbstractAction.AbstractAction' was converted to 'ActionSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingAbstractActionAbstractAction_javalangString'"
			public PrefsAction(PreferencesDialog enclosingInstance):base("prefs")
			{
				InitBlock(enclosingInstance);
				//UPGRADE_ISSUE: Method 'javax.swing.AbstractAction.setEnabled' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractActionsetEnabled_boolean'"
				this.setEnabled(true);
			}
			
			public override void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				//UPGRADE_TODO: Method 'java.awt.Dialog.show' was converted to 'System.Windows.Forms.Form.ShowDialog' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtDialogshow'"
				Enclosing_Instance.ShowDialog();
			}
		}
		
		protected internal virtual SupportClass.ActionSupport getAction(System.String cmd)
		{
			return (SupportClass.ActionSupport) commands[cmd];
		}
		
		//UPGRADE_TODO: Interface 'java.awt.event.ItemListener' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
		//UPGRADE_NOTE: The initialization of  'checkBoxListener' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		internal ItemListener checkBoxListener;
		
		private System.Windows.Forms.Button applyButton;
		private System.Windows.Forms.Button jmolDefaultsButton;
		private System.Windows.Forms.Button rasmolDefaultsButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button okButton;
		
		public virtual void  actionPerformed(System.Object event_sender, System.EventArgs event_Renamed)
		{
			if (event_sender == applyButton)
			{
				save();
			}
			else if (event_sender == jmolDefaultsButton)
			{
				resetDefaults(null);
			}
			else if (event_sender == rasmolDefaultsButton)
			{
				resetDefaults(rasmolOverrides);
			}
			else if (event_sender == cancelButton)
			{
				cancel();
			}
			else if (event_sender == okButton)
			{
				ok();
			}
		}
	}
}