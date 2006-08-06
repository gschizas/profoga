/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-03-28 15:08:27 +0200 (mar., 28 mars 2006) $
* $Revision: 4831 $
*
* Copyright (C) 2000-2005  The Jmol Development Team
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
//import org.jmol.adapter.cdk.CdkJmolAdapter;
using SmarterJmolAdapter = org.jmol.adapter.smarter.SmarterJmolAdapter;
using JmolPopup = org.jmol.popup.JmolPopup;
using GT = org.jmol.i18n.GT;
//import org.openscience.cdk.applications.plugin.CDKPluginManager;
//UPGRADE_TODO: The type 'Acme.JPM.Encoders.PpmEncoder' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using PpmEncoder = Acme.JPM.Encoders.PpmEncoder;
//UPGRADE_TODO: The type 'com.lowagie.text.Document' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Document = com.lowagie.text.Document;
//UPGRADE_TODO: The type 'com.lowagie.text.DocumentException' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using DocumentException = com.lowagie.text.DocumentException;
//UPGRADE_TODO: The type 'com.lowagie.text.pdf.PdfContentByte' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using PdfContentByte = com.lowagie.text.pdf.PdfContentByte;
//UPGRADE_TODO: The type 'com.lowagie.text.pdf.PdfTemplate' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using PdfTemplate = com.lowagie.text.pdf.PdfTemplate;
//UPGRADE_TODO: The type 'com.lowagie.text.pdf.PdfWriter' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using PdfWriter = com.lowagie.text.pdf.PdfWriter;
using JpegEncoder = com.obrador.JpegEncoder;
//UPGRADE_TODO: The type 'org.apache.commons.cli.Options' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Options = org.apache.commons.cli.Options;
//UPGRADE_TODO: The type 'org.apache.commons.cli.CommandLine' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using CommandLine = org.apache.commons.cli.CommandLine;
//UPGRADE_TODO: The type 'org.apache.commons.cli.CommandLineParser' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using CommandLineParser = org.apache.commons.cli.CommandLineParser;
//UPGRADE_TODO: The type 'org.apache.commons.cli.PosixParser' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using PosixParser = org.apache.commons.cli.PosixParser;
//UPGRADE_TODO: The type 'org.apache.commons.cli.OptionBuilder' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using OptionBuilder = org.apache.commons.cli.OptionBuilder;
//UPGRADE_TODO: The type 'org.apache.commons.cli.ParseException' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using ParseException = org.apache.commons.cli.ParseException;
//UPGRADE_TODO: The type 'org.apache.commons.cli.HelpFormatter' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using HelpFormatter = org.apache.commons.cli.HelpFormatter;
namespace org.openscience.jmol.app
{
	
	[Serializable]
	public class Jmol:System.Windows.Forms.Panel
	{
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassPropertyChangeListener' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassPropertyChangeListener
		{
			public AnonymousClassPropertyChangeListener(System.Windows.Forms.Form f, Jmol enclosingInstance)
			{
				InitBlock(f, enclosingInstance);
			}
			private void  InitBlock(System.Windows.Forms.Form f, Jmol enclosingInstance)
			{
				this.f = f;
				this.enclosingInstance = enclosingInstance;
			}
			//UPGRADE_NOTE: Final variable f was copied into class AnonymousClassPropertyChangeListener. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1023'"
			private System.Windows.Forms.Form f;
			private Jmol enclosingInstance;
			public Jmol Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			public virtual void  propertyChange(System.Object event_sender, SupportClass.PropertyChangingEventArgs evt)
			{
				System.Console.Out.WriteLine("Drop triggered...");
				//UPGRADE_ISSUE: Member 'java.awt.Cursor.getPredefinedCursor' was converted to 'System.Windows.Forms.Cursor' which cannot be assigned to an int. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1086'"
				//UPGRADE_ISSUE: Member 'java.awt.Cursor.WAIT_CURSOR' was converted to 'System.Windows.Forms.Cursors.WaitCursor' which cannot be assigned to an int. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1086'"
				f.Cursor = System.Windows.Forms.Cursors.WaitCursor;
				
				if (evt.PropertyName.Equals(FileDropper.FD_PROPERTY_FILENAME))
				{
					//UPGRADE_NOTE: Final was removed from the declaration of 'filename '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					System.String filename = evt.NewValue.ToString();
					
					Enclosing_Instance.viewer.openFile(filename);
					System.String generatedAux13 = Enclosing_Instance.viewer.OpenFileError;
				}
				else if (evt.PropertyName.Equals(FileDropper.FD_PROPERTY_INLINE))
				{
					//UPGRADE_NOTE: Final was removed from the declaration of 'inline '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					System.String inline = evt.NewValue.ToString();
					Enclosing_Instance.viewer.openStringInline(inline);
				}
				
				//UPGRADE_ISSUE: Member 'java.awt.Cursor.getDefaultCursor' was converted to 'System.Windows.Forms.Cursors.Default' which cannot be assigned to an int. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1086'"
				f.Cursor = System.Windows.Forms.Cursors.Default;
			}
		}
		private void  InitBlock()
		{
			//UPGRADE_ISSUE: Constructor 'java.beans.PropertyChangeSupport.PropertyChangeSupport' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javabeansPropertyChangeSupport'"
			pcs = new PropertyChangeSupport(this);
			exportAction = new ExportAction(this);
			povrayAction = new PovrayAction(this);
			pdfAction = new PdfAction(this);
			printAction = new PrintAction(this);
			copyImageAction = new CopyImageAction(this);
			viewMeasurementTableAction = new ViewMeasurementTableAction(this);
			defaultActions = new SupportClass.ActionSupport[]{new NewAction(this), new NewwinAction(this), new OpenAction(this), new OpenUrlAction(this), printAction, exportAction, new CloseAction(this), new ExitAction(this), copyImageAction, new AboutAction(this), new WhatsNewAction(this), new UguideAction(this), new ConsoleAction(this), new RecentFilesAction(this), povrayAction, pdfAction, new ScriptWindowAction(this), new AtomSetChooserAction(this), viewMeasurementTableAction};
		}
		[System.ComponentModel.Browsable(true)]
		public  event SupportClass.PropertyChangeEventHandler PropertyChange;
		public static HistoryFile HistoryFile
		{
			get
			{
				return historyFile;
			}
			
		}
		/// <returns> A list of Actions that is understood by the upper level
		/// application
		/// </returns>
		virtual public SupportClass.ActionSupport[] Actions
		{
			get
			{
				
				System.Collections.ArrayList actions = new System.Collections.ArrayList();
				//UPGRADE_TODO: Method 'java.util.Arrays.asList' was converted to 'System.Collections.ArrayList' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilArraysasList_javalangObject[]'"
				actions.AddRange(new System.Collections.ArrayList(defaultActions));
				//UPGRADE_TODO: Method 'java.util.Arrays.asList' was converted to 'System.Collections.ArrayList' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilArraysasList_javalangObject[]'"
				actions.AddRange(new System.Collections.ArrayList(display.Actions));
				//UPGRADE_TODO: Method 'java.util.Arrays.asList' was converted to 'System.Collections.ArrayList' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilArraysasList_javalangObject[]'"
				actions.AddRange(new System.Collections.ArrayList(preferencesDialog.Actions));
				return (SupportClass.ActionSupport[]) SupportClass.ICollectionSupport.ToArray(actions, new SupportClass.ActionSupport[0]);
			}
			
		}
		/// <returns> The hosting frame, for the file-chooser dialog.
		/// </returns>
		//UPGRADE_TODO: Class 'java.awt.Frame' was converted to 'System.Windows.Forms.Form' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtFrame'"
		virtual protected internal System.Windows.Forms.Form Frame
		{
			get
			{
				
				for (System.Windows.Forms.Control p = Parent; p != null; p = p.Parent)
				{
					//UPGRADE_TODO: Class 'java.awt.Frame' was converted to 'System.Windows.Forms.Form' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtFrame'"
					if (p is System.Windows.Forms.Form)
					{
						//UPGRADE_TODO: Class 'java.awt.Frame' was converted to 'System.Windows.Forms.Form' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtFrame'"
						return (System.Windows.Forms.Form) p;
					}
				}
				return null;
			}
			
		}
		/// <summary> Returns a new File referenced by the property 'user.dir', or null
		/// if the property is not defined.
		/// 
		/// </summary>
		/// <returns>  a File to the user directory
		/// </returns>
		public static System.IO.FileInfo UserDirectory
		{
			get
			{
				//UPGRADE_TODO: Method 'java.lang.System.getProperty' was converted to 'System.Environment.CurrentDirectory' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangSystemgetProperty_javalangString'"
				if (System.Environment.CurrentDirectory == null)
				{
					return null;
				}
				//UPGRADE_TODO: Method 'java.lang.System.getProperty' was converted to 'System.Environment.CurrentDirectory' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangSystemgetProperty_javalangString'"
				return new System.IO.FileInfo(System.Environment.CurrentDirectory);
			}
			
		}
		
		/// <summary> The data model.</summary>
		
		public JmolViewer viewer;
		
		internal DisplayPanel display;
		internal StatusBar status;
		private PreferencesDialog preferencesDialog;
		internal MeasurementTable measurementTable;
		internal RecentFilesDialog recentFiles;
		//private JMenu recentFilesMenu;
		public ScriptWindow scriptWindow;
		public AtomSetChooser atomSetChooser;
		private ExecuteScriptAction executeScriptAction;
		protected internal System.Windows.Forms.Form frame;
		protected internal static System.IO.FileInfo currentDir;
		internal FileChooser openChooser;
		private System.Windows.Forms.FileDialog saveChooser;
		private FileTyper fileTyper;
		internal System.Windows.Forms.FileDialog exportChooser;
		internal JmolPopup jmolpopup;
		// private CDKPluginManager pluginManager;
		
		private GuiMap guimap = new GuiMap();
		
		private static int numWindows = 0;
		private static System.Drawing.Size screenSize
		{
			get
			{
				return screenSize_Renamed;
			}
			
			set
			{
				screenSize_Renamed = value;
			}
			
		}
		private static System.Drawing.Size screenSize_Renamed = System.Drawing.Size.Empty;
		internal int startupWidth, startupHeight;
		
		//UPGRADE_ISSUE: Class 'java.beans.PropertyChangeSupport' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javabeansPropertyChangeSupport'"
		//UPGRADE_NOTE: The initialization of  'pcs' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		internal PropertyChangeSupport pcs;
		
		// Window names for the history file
		private const System.String JMOL_WINDOW_NAME = "Jmol";
		private const System.String CONSOLE_WINDOW_NAME = "Console";
		private const System.String SCRIPT_WINDOW_NAME = "Script";
		private const System.String FILE_OPEN_WINDOW_NAME = "FileOpen";
		
		/// <summary> The current file.</summary>
		internal System.IO.FileInfo currentFile;
		
		/// <summary> Button group for toggle buttons in the toolbar.</summary>
		internal static System.Windows.Forms.ButtonBase buttonRotate = null;
		//UPGRADE_TODO: Class 'javax.swing.ButtonGroup' was converted to 'System.Collections.ArrayList' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
		internal static System.Collections.ArrayList toolbarButtonGroup = new System.Collections.ArrayList();
		
		internal static System.IO.FileInfo UserPropsFile;
		internal static HistoryFile historyFile;
		
		internal Splash splash;
		
		internal static System.Windows.Forms.Form consoleframe;
		
		internal Jmol(Splash splash, System.Windows.Forms.Form frame, Jmol parent, int startupWidth, int startupHeight):base()
		{
			InitBlock();
			this.frame = frame;
			this.startupWidth = startupWidth;
			this.startupHeight = startupHeight;
			numWindows++;
			
			frame.Text = "Jmol";
			frame.BackColor = System.Drawing.Color.LightGray;
			//UPGRADE_TODO: Method 'javax.swing.JFrame.getContentPane' was converted to 'System.Windows.Forms.Form' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJFramegetContentPane'"
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtBorderLayout'"
			/*
			((System.Windows.Forms.ContainerControl) frame).setLayout(new BorderLayout());*/
			
			this.splash = splash;
			
			//UPGRADE_TODO: Method 'javax.swing.JComponent.setBorder' was converted to 'System.Windows.Forms.ControlPaint.DrawBorder3D' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJComponentsetBorder_javaxswingborderBorder'"
			System.Windows.Forms.ControlPaint.DrawBorder3D(CreateGraphics(), 0, 0, Width, Height, System.Windows.Forms.Border3DStyle.Etched);
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtBorderLayout'"
			/*
			setLayout(new BorderLayout());*/
			
			status = (StatusBar) createStatusBar();
			say(GT._("Initializing 3D display..."));
			//
			display = new DisplayPanel(status, guimap);
			JmolAdapter modelAdapter;
			//UPGRADE_ISSUE: Method 'java.lang.System.getProperty' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangSystem'"
			System.String adapter = System_Renamed.getProperty("model");
			if (adapter == null || adapter.Length == 0)
				adapter = "smarter";
			if (adapter.Equals("smarter"))
			{
				System.Console.Out.WriteLine("using Smarter Model Adapter");
				modelAdapter = new SmarterJmolAdapter(null);
			}
			else if (adapter.Equals("cdk"))
			{
				System.Console.Out.WriteLine("the CDK Model Adapter is currently no longer supported. Check out http://bioclipse.net/. -- using Smarter");
				// modelAdapter = new CdkJmolAdapter(null);
				modelAdapter = new SmarterJmolAdapter(null);
			}
			else
			{
				System.Console.Out.WriteLine("unrecognized model adapter:" + adapter + " -- using Smarter");
				modelAdapter = new SmarterJmolAdapter(null);
			}
			
			viewer = JmolViewer.allocateViewer(display, modelAdapter);
			display.Viewer = viewer;
			
			say(GT._("Initializing Preferences..."));
			preferencesDialog = new PreferencesDialog(frame, guimap, viewer);
			say(GT._("Initializing Recent Files..."));
			recentFiles = new RecentFilesDialog(frame);
			say(GT._("Initializing Script Window..."));
			scriptWindow = new ScriptWindow(viewer, frame);
			say(GT._("Initializing AtomSetChooser Window..."));
			atomSetChooser = new AtomSetChooser(viewer, frame);
			
			viewer.JmolStatusListener = new MyJmolStatusListener(this);
			
			say(GT._("Initializing Measurements..."));
			measurementTable = new MeasurementTable(viewer, frame);
			
			// Setup Plugin system
			// say(GT._("Loading plugins..."));
			// pluginManager = new CDKPluginManager(
			//     System.getProperty("user.home") + System.getProperty("file.separator")
			//     + ".jmol", new JmolEditBus(viewer)
			// );
			// pluginManager.loadPlugin("org.openscience.cdkplugin.dirbrowser.DirBrowserPlugin");
			// pluginManager.loadPlugin("org.openscience.cdkplugin.dirbrowser.DadmlBrowserPlugin");
			// pluginManager.loadPlugins(
			//     System.getProperty("user.home") + System.getProperty("file.separator")
			//     + ".jmol/plugins"
			// );
			// feature to allow for globally installed plugins
			// if (System.getProperty("plugin.dir") != null) {
			//     pluginManager.loadPlugins(System.getProperty("plugin.dir"));
			// }
			
			// install the command table
			say(GT._("Building Command Hooks..."));
			commands = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
			SupportClass.ActionSupport[] actions = Actions;
			for (int i = 0; i < actions.Length; i++)
			{
				SupportClass.ActionSupport a = actions[i];
				//UPGRADE_ISSUE: Method 'javax.swing.Action.getValue' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingActiongetValue_javalangString'"
				//UPGRADE_ISSUE: Field 'javax.swing.Action.NAME' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingActionNAME_f'"
				commands[a.getValue(Action.NAME)] = a;
			}
			
			menuItems = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
			say(GT._("Building Menubar..."));
			executeScriptAction = new ExecuteScriptAction(this);
			menubar = createMenubar();
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javalangString_javaawtComponent'"
			Controls.Add(menubar);
			
			System.Windows.Forms.Panel panel = new System.Windows.Forms.Panel();
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtBorderLayout'"
			/*
			panel.setLayout(new BorderLayout());*/
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javalangString_javaawtComponent'"
			System.Windows.Forms.Control temp_Control;
			temp_Control = createToolbar();
			panel.Controls.Add(temp_Control);
			
			System.Windows.Forms.Panel ip = new System.Windows.Forms.Panel();
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtBorderLayout'"
			/*
			ip.setLayout(new BorderLayout());*/
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javalangString_javaawtComponent'"
			ip.Controls.Add(display);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javalangString_javaawtComponent'"
			panel.Controls.Add(ip);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javalangString_javaawtComponent'"
			Controls.Add(panel);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javalangString_javaawtComponent'"
			Controls.Add(status);
			
			say(GT._("Starting display..."));
			display.start();
			
			say(GT._("Setting up File Choosers..."));
			openChooser = new FileChooser();
			//UPGRADE_TODO: Method 'javax.swing.JFileChooser.setCurrentDirectory' was converted to 'System.Windows.Forms.OpenFileDialog.InitialDirectory' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJFileChoosersetCurrentDirectory_javaioFile'"
			openChooser.InitialDirectory = currentDir.DirectoryName;
			//UPGRADE_ISSUE: Method 'java.lang.System.getProperty' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangSystem'"
			System.String previewProperty = System_Renamed.getProperty("openFilePreview", "true");
			//UPGRADE_NOTE: Exceptions thrown by the equivalent in .NET of method 'java.lang.Boolean.valueOf' may be different. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1099'"
			if (System.Boolean.Parse(previewProperty))
			{
				new FilePreview(openChooser, modelAdapter);
			}
			//UPGRADE_TODO: Constructor may need to be changed depending on function performed by the 'System.Windows.Forms.FileDialog' object. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1270'"
			saveChooser = new System.Windows.Forms.OpenFileDialog();
			fileTyper = new FileTyper();
			//UPGRADE_ISSUE: Method 'javax.swing.JComponent.addPropertyChangeListener' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJComponentaddPropertyChangeListener_javabeansPropertyChangeListener'"
			saveChooser.addPropertyChangeListener(fileTyper);
			//UPGRADE_ISSUE: Method 'javax.swing.JFileChooser.setAccessory' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJFileChoosersetAccessory_javaxswingJComponent'"
			saveChooser.setAccessory(fileTyper);
			//UPGRADE_TODO: Method 'javax.swing.JFileChooser.setCurrentDirectory' was converted to 'System.Windows.Forms.OpenFileDialog.InitialDirectory' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJFileChoosersetCurrentDirectory_javaioFile'"
			saveChooser.InitialDirectory = currentDir.DirectoryName;
			//UPGRADE_TODO: Constructor may need to be changed depending on function performed by the 'System.Windows.Forms.FileDialog' object. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1270'"
			exportChooser = new System.Windows.Forms.OpenFileDialog();
			//UPGRADE_TODO: Method 'javax.swing.JFileChooser.setCurrentDirectory' was converted to 'System.Windows.Forms.OpenFileDialog.InitialDirectory' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJFileChoosersetCurrentDirectory_javaioFile'"
			exportChooser.InitialDirectory = currentDir.DirectoryName;
			
			//UPGRADE_ISSUE: Method 'java.beans.PropertyChangeSupport.addPropertyChangeListener' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javabeansPropertyChangeSupport'"
			pcs.addPropertyChangeListener(chemFileProperty, exportAction);
			//UPGRADE_ISSUE: Method 'java.beans.PropertyChangeSupport.addPropertyChangeListener' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javabeansPropertyChangeSupport'"
			pcs.addPropertyChangeListener(chemFileProperty, povrayAction);
			//UPGRADE_ISSUE: Method 'java.beans.PropertyChangeSupport.addPropertyChangeListener' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javabeansPropertyChangeSupport'"
			pcs.addPropertyChangeListener(chemFileProperty, pdfAction);
			//UPGRADE_ISSUE: Method 'java.beans.PropertyChangeSupport.addPropertyChangeListener' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javabeansPropertyChangeSupport'"
			pcs.addPropertyChangeListener(chemFileProperty, printAction);
			//UPGRADE_ISSUE: Method 'java.beans.PropertyChangeSupport.addPropertyChangeListener' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javabeansPropertyChangeSupport'"
			pcs.addPropertyChangeListener(chemFileProperty, viewMeasurementTableAction);
			//UPGRADE_ISSUE: Method 'java.beans.PropertyChangeSupport.addPropertyChangeListener' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javabeansPropertyChangeSupport'"
			pcs.addPropertyChangeListener(chemFileProperty, atomSetChooser);
			
			jmolpopup = JmolPopup.newJmolPopup(viewer);
			
			// prevent new Jmol from covering old Jmol
			if (parent != null)
			{
				System.Windows.Forms.Control temp_Control2;
				//UPGRADE_NOTE: Exceptions thrown by the equivalent in .NET of method 'java.awt.Component.getLocationOnScreen' may be different. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1099'"
				temp_Control2 = parent.frame;
				System.Drawing.Point location = temp_Control2.PointToScreen(temp_Control2.Location);
				int maxX = screenSize.Width - 50;
				int maxY = screenSize.Height - 50;
				
				location.X += 40;
				location.Y += 40;
				if ((location.X > maxX) || (location.Y > maxY))
				{
					//UPGRADE_TODO: Method 'java.awt.Point.setLocation' was converted to 'System.Drawing.Point.Point' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
					location = new System.Drawing.Point((System.Int32) 0, (System.Int32) 0);
				}
				//UPGRADE_TODO: Method 'java.awt.Component.setLocation' was converted to 'System.Windows.Forms.Control.Location' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetLocation_javaawtPoint'"
				frame.Location = location;
			}
			
			//UPGRADE_TODO: Method 'javax.swing.JFrame.getContentPane' was converted to 'System.Windows.Forms.Form' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJFramegetContentPane'"
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javalangString_javaawtComponent'"
			((System.Windows.Forms.ContainerControl) frame).Controls.Add(this);
			//UPGRADE_NOTE: Some methods of the 'java.awt.event.WindowListener' class are not used in the .NET Framework. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1308'"
			frame.Closing += new System.ComponentModel.CancelEventHandler(new Jmol.AppCloser(this).windowClosing);
			//UPGRADE_ISSUE: Method 'java.awt.Window.pack' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtWindowpack'"
			frame.pack();
			//UPGRADE_TODO: Method 'java.awt.Component.setSize' was converted to 'System.Windows.Forms.Control.Size' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetSize_int_int'"
			frame.Size = new System.Drawing.Size(startupWidth, startupHeight);
			//UPGRADE_TODO: Class 'javax.swing.ImageIcon' was converted to 'System.Drawing.Image' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingImageIcon'"
			System.Drawing.Image jmolIcon = JmolResourceHandler.getIconX("icon");
			System.Drawing.Image iconImage = jmolIcon;
			frame.Icon = System.Drawing.Icon.FromHandle(((System.Drawing.Bitmap) iconImage).GetHicon());
			
			// Repositionning windows
			historyFile.repositionWindow(SCRIPT_WINDOW_NAME, scriptWindow);
			
			say(GT._("Setting up Drag-and-Drop..."));
			FileDropper dropper = new FileDropper();
			//UPGRADE_NOTE: Final was removed from the declaration of 'f '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
			System.Windows.Forms.Form f = frame;
			dropper.PropertyChange += new SupportClass.PropertyChangeEventHandler(new AnonymousClassPropertyChangeListener(f, this).propertyChange);
			
			//UPGRADE_ISSUE: Method 'java.awt.Component.setDropTarget' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtComponentsetDropTarget_javaawtdndDropTarget'"
			this.AllowDrop = true;
			this.DragEnter += new System.Windows.Forms.DragEventHandler(dropper.dragEnter_renamed);
			this.DragOver += new System.Windows.Forms.DragEventHandler(dropper.dragOver_renamed);
			this.DragLeave += new System.EventHandler(dropper.dragExit_renamed);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(dropper.drop_renamed);
			//UPGRADE_ISSUE: Constructor 'java.awt.dnd.DropTarget.DropTarget' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtdndDropTarget'"
			this.setDropTarget(new DropTarget(this, dropper));
			this.Enabled = true;
			
			say(GT._("Launching main frame..."));
		}
		
		public static Jmol getJmol(System.Windows.Forms.Form frame, int startupWidth, int startupHeight)
		{
			//UPGRADE_TODO: Class 'javax.swing.ImageIcon' was converted to 'System.Drawing.Image' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingImageIcon'"
			System.Drawing.Image splash_image = JmolResourceHandler.getIconX("splash");
			System.Console.Out.WriteLine("splash_image=" + splash_image);
			Splash splash = new Splash(frame, splash_image);
			splash.Cursor = System.Windows.Forms.Cursors.WaitCursor;
			splash.showStatus(GT._("Creating main window..."));
			splash.showStatus(GT._("Initializing Swing..."));
			try
			{
				//UPGRADE_TODO: Method 'javax.swing.UIManager.setLookAndFeel' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
				//UPGRADE_TODO: Method 'javax.swing.UIManager.getCrossPlatformLookAndFeelClassName' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
				UIManager.setLookAndFeel(UIManager.getCrossPlatformLookAndFeelClassName());
			}
			catch (System.Exception exc)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				System.Console.Error.WriteLine("Error loading L&F: " + exc);
			}
			
			//UPGRADE_ISSUE: Method 'java.awt.Toolkit.getDefaultToolkit' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtToolkit'"
			Toolkit.getDefaultToolkit();
			screenSize = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size;
			
			splash.showStatus(GT._("Initializing Jmol..."));
			
			// cache the current directory to speed up Jmol window creation
			currentDir = UserDirectory;
			
			Jmol window = new Jmol(splash, frame, null, startupWidth, startupHeight);
			//UPGRADE_TODO: 'System.Windows.Forms.Application.Run' must be called to start a main form. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1135'"
			frame.Show();
			return window;
		}
		
		[STAThread]
		public static void  Main(System.String[] args)
		{
			
			Jmol jmol = null;
			
			System.String modelFilename = null;
			System.String scriptFilename = null;
			
			Options options = new Options();
			options.addOption("h", "help", false, GT._("give this help page"));
			
			OptionBuilder.withLongOpt("script");
			OptionBuilder.withDescription("script to run");
			OptionBuilder.withValueSeparator('=');
			OptionBuilder.hasArg();
			options.addOption(OptionBuilder.create("s"));
			
			OptionBuilder.withArgName(GT._("property=value"));
			OptionBuilder.hasArg();
			OptionBuilder.withValueSeparator();
			OptionBuilder.withDescription(GT._("supported options are given below"));
			options.addOption(OptionBuilder.create("D"));
			
			OptionBuilder.withLongOpt("geometry");
			OptionBuilder.withDescription(GT._("window size 500x500"));
			OptionBuilder.withValueSeparator();
			OptionBuilder.hasArg();
			options.addOption(OptionBuilder.create("g"));
			
			CommandLine line = null;
			try
			{
				CommandLineParser parser = new PosixParser();
				line = parser.parse(options, args);
			}
			catch (ParseException exception)
			{
				System.Console.Error.WriteLine("Unexpected exception: " + exception.toString());
			}
			
			if (line.hasOption("h"))
			{
				HelpFormatter formatter = new HelpFormatter();
				formatter.printHelp("Jmol", options);
				
				// now report on the -D options
				System.Console.Out.WriteLine();
				System.Console.Out.WriteLine(GT._("The -D options are as follows (defaults in parathesis):"));
				System.Console.Out.WriteLine("  cdk.debugging=[true|false] (false)");
				System.Console.Out.WriteLine("  cdk.debug.stdout=[true|false] (false)");
				System.Console.Out.WriteLine("  display.speed=[fps|ms] (ms)");
				System.Console.Out.WriteLine("  JmolConsole=[true|false] (true)");
				System.Console.Out.WriteLine("  plugin.dir (unset)");
				System.Console.Out.WriteLine("  user.language=[DE|EN|ES|FR|NL|PL] (EN)");
				
				System.Environment.Exit(0);
			}
			
			try
			{
				//UPGRADE_ISSUE: Method 'java.lang.System.getProperty' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangSystem'"
				System.String vers = System_Renamed.getProperty("java.version");
				if (String.CompareOrdinal(vers, "1.1.2") < 0)
				{
					System.Console.Out.WriteLine("!!!WARNING: Swing components require a " + "1.1.2 or higher version VM!!!");
				}
				
				int startupWidth = 0, startupHeight = 0;
				System.Drawing.Size size = historyFile.getWindowSize(JMOL_WINDOW_NAME);
				if (!size.IsEmpty)
				{
					startupWidth = size.Width;
					startupHeight = size.Height;
				}
				if (line.hasOption("g"))
				{
					System.String geometry = line.getOptionValue("g");
					int indexX = geometry.IndexOf('x');
					if (indexX > 0)
					{
						startupWidth = parseInt(geometry.Substring(0, (indexX) - (0)));
						startupHeight = parseInt(geometry.Substring(indexX + 1));
					}
				}
				if (startupWidth <= 0 || startupHeight <= 0)
				{
					startupWidth = 500;
					startupHeight = 550;
				}
				
				System.Windows.Forms.Form jmolFrame = new System.Windows.Forms.Form();
				System.Drawing.Point jmolPosition = historyFile.getWindowPosition(JMOL_WINDOW_NAME);
				if (!jmolPosition.IsEmpty)
				{
					//UPGRADE_TODO: Method 'java.awt.Component.setLocation' was converted to 'System.Windows.Forms.Control.Location' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetLocation_javaawtPoint'"
					jmolFrame.Location = jmolPosition;
				}
				jmol = getJmol(jmolFrame, startupWidth, startupHeight);
				
				// Process command line arguments
				args = line.getArgs();
				if (args.Length > 0)
				{
					modelFilename = args[0];
				}
				if (line.hasOption("s"))
				{
					scriptFilename = line.getOptionValue("s");
				}
				
				// Open a file if one is given as an argument
				if (modelFilename != null)
				{
					jmol.viewer.openFile(modelFilename);
					System.String generatedAux11 = jmol.viewer.OpenFileError;
				}
				
				// Oke, by now it is time to execute the script
				if (scriptFilename != null)
				{
					System.Console.Out.WriteLine("Executing script: " + scriptFilename);
					jmol.splash.showStatus(GT._("Executing script..."));
					jmol.viewer.evalFile(scriptFilename);
				}
			}
			//UPGRADE_NOTE: Exception 'java.lang.Throwable' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
			catch (System.Exception t)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				System.Console.Out.WriteLine("uncaught exception: " + t);
				SupportClass.WriteStackTrace(t, Console.Error);
			}
			
			System.Drawing.Point location = jmol.frame.Location;
			System.Drawing.Size size2 = jmol.frame.Size;
			
			// Adding console frame to grab System.out & System.err
			consoleframe = SupportClass.FormSupport.CreateForm(GT._("Jmol Console"));
			//UPGRADE_TODO: The equivalent in .NET for method 'java.awt.Frame.getIconImage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			consoleframe.Icon = System.Drawing.Icon.FromHandle(((System.Drawing.Bitmap) jmol.frame.Icon).GetHicon());
			try
			{
				ConsoleTextArea consoleTextArea = new ConsoleTextArea();
				//UPGRADE_NOTE: If the given Font Name does not exist, a default Font instance is created. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1075'"
				consoleTextArea.Font = new System.Drawing.Font("monospaced", 12);
				//UPGRADE_TODO: Constructor 'javax.swing.JScrollPane.JScrollPane' was converted to 'System.Windows.Forms.ScrollableControl.ScrollableControl' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJScrollPaneJScrollPane_javaawtComponent'"
				System.Windows.Forms.ScrollableControl temp_scrollablecontrol2;
				temp_scrollablecontrol2 = new System.Windows.Forms.ScrollableControl();
				temp_scrollablecontrol2.AutoScroll = true;
				temp_scrollablecontrol2.Controls.Add(consoleTextArea);
				//UPGRADE_TODO: Method 'javax.swing.JFrame.getContentPane' was converted to 'System.Windows.Forms.Form' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJFramegetContentPane'"
				//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
				System.Windows.Forms.Control temp_Control;
				temp_Control = temp_scrollablecontrol2;
				((System.Windows.Forms.ContainerControl) consoleframe).Controls.Add(temp_Control);
				temp_Control.Dock = System.Windows.Forms.DockStyle.Fill;
				temp_Control.BringToFront();
			}
			catch (System.IO.IOException e)
			{
				System.Windows.Forms.TextBox temp_TextBox;
				temp_TextBox = new System.Windows.Forms.TextBox();
				temp_TextBox.Multiline = true;
				temp_TextBox.WordWrap = false;
				temp_TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
				System.Windows.Forms.TextBox errorTextArea = temp_TextBox;
				//UPGRADE_NOTE: If the given Font Name does not exist, a default Font instance is created. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1075'"
				errorTextArea.Font = new System.Drawing.Font("monospaced", 12);
				//UPGRADE_TODO: Constructor 'javax.swing.JScrollPane.JScrollPane' was converted to 'System.Windows.Forms.ScrollableControl.ScrollableControl' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJScrollPaneJScrollPane_javaawtComponent'"
				System.Windows.Forms.ScrollableControl temp_scrollablecontrol4;
				temp_scrollablecontrol4 = new System.Windows.Forms.ScrollableControl();
				temp_scrollablecontrol4.AutoScroll = true;
				temp_scrollablecontrol4.Controls.Add(errorTextArea);
				//UPGRADE_TODO: Method 'javax.swing.JFrame.getContentPane' was converted to 'System.Windows.Forms.Form' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJFramegetContentPane'"
				//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
				System.Windows.Forms.Control temp_Control2;
				temp_Control2 = temp_scrollablecontrol4;
				((System.Windows.Forms.ContainerControl) consoleframe).Controls.Add(temp_Control2);
				temp_Control2.Dock = System.Windows.Forms.DockStyle.Fill;
				temp_Control2.BringToFront();
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				errorTextArea.AppendText(GT._("Could not create ConsoleTextArea: ") + e);
			}
			
			System.Drawing.Size consoleSize = historyFile.getWindowSize(CONSOLE_WINDOW_NAME);
			System.Drawing.Point consolePosition = historyFile.getWindowPosition(CONSOLE_WINDOW_NAME);
			if ((!consoleSize.IsEmpty) && (!consolePosition.IsEmpty))
			{
				consoleframe.SetBounds(consolePosition.X, consolePosition.Y, consoleSize.Width, consoleSize.Height);
			}
			else
			{
				consoleframe.SetBounds(location.X, location.Y + size2.Height, size2.Width, 200);
			}
			
			System.Boolean consoleVisible = historyFile.getWindowVisibility(CONSOLE_WINDOW_NAME);
			//UPGRADE_TODO: The 'System.Boolean' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
			if ((consoleVisible != null) && (consoleVisible.Equals(true)))
			{
				//UPGRADE_TODO: 'System.Windows.Forms.Application.Run' must be called to start a main form. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1135'"
				consoleframe.Show();
			}
		}
		
		internal static int parseInt(System.String str)
		{
			try
			{
				return System.Int32.Parse(str);
			}
			catch (System.FormatException nfe)
			{
				return System.Int32.MinValue;
			}
		}
		
		private void  say(System.String message)
		{
			if (splash == null)
			{
				System.Console.Out.WriteLine(message);
			}
			else
			{
				splash.showStatus(message);
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AppCloser' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		/// <summary> To shutdown when run as an application.  This is a
		/// fairly lame implementation.   A more self-respecting
		/// implementation would at least check to see if a save
		/// was needed.
		/// </summary>
		protected internal sealed class AppCloser
		{
			public AppCloser(Jmol enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(Jmol enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Jmol enclosingInstance;
			public Jmol Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			public void  windowClosing(System.Object event_sender, System.ComponentModel.CancelEventArgs e)
			{
				e.Cancel = true;
				Enclosing_Instance.doClose();
			}
		}
		
		internal virtual void  doClose()
		{
			// Save window positions and status in the history
			if (historyFile != null)
			{
				historyFile.addWindowInfo(JMOL_WINDOW_NAME, this.frame);
				historyFile.addWindowInfo(CONSOLE_WINDOW_NAME, consoleframe);
				historyFile.addWindowInfo(SCRIPT_WINDOW_NAME, scriptWindow);
			}
			
			// Close Jmol
			numWindows--;
			if (numWindows <= 1)
			{
				System.Console.Out.WriteLine(GT._("Closing Jmol..."));
				// pluginManager.closePlugins();
				System.Environment.Exit(0);
			}
			else
			{
				this.frame.Dispose();
			}
		}
		
		/// <summary> This is the hook through which all menu items are
		/// created.  It registers the result with the menuitem
		/// hashtable so that it can be fetched with getMenuItem().
		/// </summary>
		/// <param name="cmd">
		/// </param>
		/// <returns> Menu item created
		/// </returns>
		/// <seealso cref="getMenuItem">
		/// </seealso>
		protected internal virtual System.Windows.Forms.MenuItem createMenuItem(System.String cmd)
		{
			
			System.Windows.Forms.MenuItem mi;
			if (cmd.EndsWith("Check"))
			{
				mi = guimap.newJCheckBoxMenuItem(cmd, false);
			}
			else
			{
				mi = guimap.newJMenuItem(cmd);
			}
			
			//UPGRADE_TODO: Class 'javax.swing.ImageIcon' was converted to 'System.Drawing.Image' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingImageIcon'"
			System.Drawing.Image f = JmolResourceHandler.getIconX(cmd + "Image");
			if (f != null)
			{
				//UPGRADE_ISSUE: Method 'javax.swing.AbstractButton.setHorizontalTextPosition' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000'"
				//UPGRADE_TODO: The equivalent in .NET for field 'javax.swing.SwingConstants.RIGHT' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				mi.setHorizontalTextPosition((int) System.Windows.Forms.HorizontalAlignment.Right);
				//UPGRADE_ISSUE: Method 'javax.swing.AbstractButton.setIcon' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractButtonsetIcon_javaxswingIcon'"
				mi.setIcon(f);
			}
			
			if (cmd.EndsWith("Script"))
			{
				SupportClass.CommandManager.SetCommand(mi, JmolResourceHandler.getStringX(cmd));
				mi.Click += new System.EventHandler(executeScriptAction.actionPerformed);
				SupportClass.CommandManager.CheckCommand(mi);
			}
			else
			{
				SupportClass.CommandManager.SetCommand(mi, cmd);
				SupportClass.ActionSupport a = getAction(cmd);
				if (a != null)
				{
					mi.Click += new System.EventHandler(a.actionPerformed);
					SupportClass.CommandManager.CheckCommand(mi);
					//UPGRADE_ISSUE: Method 'javax.swing.Action.addPropertyChangeListener' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingActionaddPropertyChangeListener_javabeansPropertyChangeListener'"
					a.addPropertyChangeListener(new ActionChangedListener(this, mi));
					//UPGRADE_ISSUE: Method 'javax.swing.Action.isEnabled' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingActionisEnabled'"
					mi.Enabled = a.isEnabled();
				}
				else
				{
					mi.Enabled = false;
				}
			}
			menuItems[cmd] = mi;
			return mi;
		}
		
		/// <summary> Fetch the menu item that was created for the given
		/// command.
		/// </summary>
		/// <param name="cmd"> Name of the action.
		/// </param>
		/// <returns> item created for the given command or null
		/// if one wasn't created.
		/// </returns>
		protected internal virtual System.Windows.Forms.MenuItem getMenuItem(System.String cmd)
		{
			return (System.Windows.Forms.MenuItem) menuItems[cmd];
		}
		
		/// <summary> Fetch the action that was created for the given
		/// command.
		/// </summary>
		/// <param name="cmd"> Name of the action.
		/// </param>
		/// <returns> The action
		/// </returns>
		protected internal virtual SupportClass.ActionSupport getAction(System.String cmd)
		{
			return (SupportClass.ActionSupport) commands[cmd];
		}
		
		/// <summary> Create the toolbar.  By default this reads the
		/// resource file for the definition of the toolbars.
		/// </summary>
		/// <returns> The toolbar
		/// </returns>
		private System.Windows.Forms.Control createToolbar()
		{
			
			System.Windows.Forms.ToolBar temp_ToolBar;
			System.Windows.Forms.ImageList temp_ImageList;
			temp_ImageList = new System.Windows.Forms.ImageList();
			temp_ToolBar = new System.Windows.Forms.ToolBar();
			temp_ToolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(SupportClass.ToolBarButtonClicked);
			temp_ToolBar.ImageList = temp_ImageList;
			toolbar = temp_ToolBar;
			System.String[] tool1Keys = tokenize(JmolResourceHandler.getStringX("toolbar"));
			for (int i = 0; i < tool1Keys.Length; i++)
			{
				if (tool1Keys[i].Equals("-"))
				{
					System.Windows.Forms.ToolBarButton separator;
					separator = new System.Windows.Forms.ToolBarButton();
					separator.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
					toolbar.Buttons.Add(separator);
				}
				else
				{
					System.Windows.Forms.Button temp_Button;
					temp_Button = createTool(tool1Keys[i]);
					System.Windows.Forms.ToolBarButton temp_ToolBarButton;
					//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
					toolbar.Controls.Add(createTool(tool1Keys[i]));
				}
			}
			
			//Action handler implementation would go here.
			System.Windows.Forms.Button temp_Button2;
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createHorizontalGlue' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			temp_Button2 = Box.createHorizontalGlue();
			System.Windows.Forms.ToolBarButton temp_ToolBarButton2;
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			toolbar.Controls.Add(Box.createHorizontalGlue());
			
			return toolbar;
		}
		
		/// <summary> Hook through which every toolbar item is created.</summary>
		/// <param name="key">
		/// </param>
		/// <returns> Toolbar item
		/// </returns>
		protected internal virtual System.Windows.Forms.Control createTool(System.String key)
		{
			return createToolbarButton(key);
		}
		
		/// <summary> Create a button to go inside of the toolbar.  By default this
		/// will load an image resource.  The image filename is relative to
		/// the classpath (including the '.' directory if its a part of the
		/// classpath), and may either be in a JAR file or a separate file.
		/// 
		/// </summary>
		/// <param name="key">The key in the resource file to serve as the basis
		/// of lookups.
		/// </param>
		/// <returns> Button
		/// </returns>
		protected internal virtual System.Windows.Forms.ButtonBase createToolbarButton(System.String key)
		{
			
			//UPGRADE_TODO: Class 'javax.swing.ImageIcon' was converted to 'System.Drawing.Image' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingImageIcon'"
			System.Drawing.Image ii = JmolResourceHandler.getIconX(key + "Image");
			System.Windows.Forms.ButtonBase b = SupportClass.ButtonSupport.CreateStandardButton(ii);
			System.String isToggleString = JmolResourceHandler.getStringX(key + "Toggle");
			if (isToggleString != null)
			{
				//UPGRADE_NOTE: Exceptions thrown by the equivalent in .NET of method 'java.lang.Boolean.valueOf' may be different. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1099'"
				bool isToggle = System.Boolean.Parse(isToggleString);
				if (isToggle)
				{
					System.Windows.Forms.CheckBox temp_checkbox;
					temp_checkbox = new System.Windows.Forms.CheckBox();
					temp_checkbox.Appearance = System.Windows.Forms.Appearance.Button;
					temp_checkbox.Image = ii;
					b = temp_checkbox;
					if (key.Equals("rotate"))
						buttonRotate = b;
					toolbarButtonGroup.Add((System.Object) b);
					System.String isSelectedString = JmolResourceHandler.getStringX(key + "ToggleSelected");
					if (isSelectedString != null)
					{
						//UPGRADE_NOTE: Exceptions thrown by the equivalent in .NET of method 'java.lang.Boolean.valueOf' may be different. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1099'"
						bool isSelected = System.Boolean.Parse(isSelectedString);
						//UPGRADE_ISSUE: Method 'javax.swing.AbstractButton.setSelected' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractButtonsetSelected_boolean'"
						b.setSelected(isSelected);
					}
				}
			}
			//UPGRADE_ISSUE: Method 'javax.swing.JComponent.setRequestFocusEnabled' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJComponentsetRequestFocusEnabled_boolean'"
			b.setRequestFocusEnabled(false);
			//UPGRADE_ISSUE: Method 'javax.swing.AbstractButton.setMargin' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractButtonsetMargin_javaawtInsets'"
			b.setMargin(new System.Int32[]{1, 1, 1, 1});
			
			SupportClass.ActionSupport a = null;
			System.String actionCommand = null;
			if (key.EndsWith("Script"))
			{
				actionCommand = JmolResourceHandler.getStringX(key);
				a = executeScriptAction;
			}
			else
			{
				actionCommand = key;
				a = getAction(key);
			}
			if (a != null)
			{
				SupportClass.CommandManager.SetCommand(b, actionCommand);
				SupportClass.CommandManager.CheckCommand(b);
				//UPGRADE_ISSUE: Method 'javax.swing.Action.addPropertyChangeListener' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingActionaddPropertyChangeListener_javabeansPropertyChangeListener'"
				a.addPropertyChangeListener(new ActionChangedListener(this, b));
				//UPGRADE_ISSUE: Method 'javax.swing.Action.isEnabled' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingActionisEnabled'"
				b.Enabled = a.isEnabled();
			}
			else
			{
				b.Enabled = false;
			}
			
			System.String tip = guimap.getLabel(key + "Tip");
			if (tip != null)
			{
				SupportClass.ToolTipSupport.setToolTipText(b, tip);
			}
			
			return b;
		}
		
		public static void  setRotateButton()
		{
			if (buttonRotate != null)
			{
				//UPGRADE_ISSUE: Method 'javax.swing.AbstractButton.setSelected' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractButtonsetSelected_boolean'"
				buttonRotate.setSelected(true);
			}
		}
		
		/// <summary> Take the given string and chop it up into a series
		/// of strings on whitespace boundries.  This is useful
		/// for trying to get an array of strings out of the
		/// resource file.
		/// </summary>
		/// <param name="input">String to chop
		/// </param>
		/// <returns> Strings chopped on whitespace boundries
		/// </returns>
		protected internal virtual System.String[] tokenize(System.String input)
		{
			
			System.Collections.ArrayList v = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			SupportClass.Tokenizer t = new SupportClass.Tokenizer(input);
			System.String[] cmd;
			
			while (t.HasMoreTokens())
			{
				v.Add(t.NextToken());
			}
			cmd = new System.String[v.Count];
			for (int i = 0; i < cmd.Length; i++)
			{
				cmd[i] = ((System.String) v[i]);
			}
			
			return cmd;
		}
		
		protected internal virtual System.Windows.Forms.Control createStatusBar()
		{
			return new StatusBar();
		}
		
		/// <summary> Create the menubar for the app.  By default this pulls the
		/// definition of the menu from the associated resource file.
		/// </summary>
		/// <returns> Menubar
		/// </returns>
		protected internal virtual System.Windows.Forms.MainMenu createMenubar()
		{
			System.Windows.Forms.MainMenu mb = new System.Windows.Forms.MainMenu();
			addNormalMenuBar(mb);
			// The Macros Menu
			addMacrosMenuBar(mb);
			// The Plugin Menu
			// if (pluginManager != null) {
			//     mb.add(pluginManager.getMenu());
			// }
			// The Help menu, right aligned
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createHorizontalGlue' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			System.Windows.Forms.Control temp_Control;
			temp_Control = Box.createHorizontalGlue();
			mb.Controls.Add(temp_Control);
			addHelpMenuBar(mb);
			return mb;
		}
		
		protected internal virtual void  addMacrosMenuBar(System.Windows.Forms.MainMenu menuBar)
		{
			// ok, here needs to be added the funny stuff
			System.Windows.Forms.MenuItem macroMenu = new System.Windows.Forms.MenuItem(GT._("Macros"));
			//UPGRADE_TODO: Method 'java.lang.System.getProperty' was converted to 'System.Environment.GetEnvironmentVariable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangSystemgetProperty_javalangString'"
			//UPGRADE_TODO: Method 'java.lang.System.getProperty' was converted to 'System.IO.Path.DirectorySeparatorChar.ToString' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangSystemgetProperty_javalangString'"
			System.IO.FileInfo macroDir = new System.IO.FileInfo(System.Environment.GetEnvironmentVariable("userprofile") + System.IO.Path.DirectorySeparatorChar.ToString() + ".jmol" + System.IO.Path.DirectorySeparatorChar.ToString() + "macros");
			System.Console.Out.WriteLine("User macros dir: " + macroDir);
			bool tmpBool;
			if (System.IO.File.Exists(macroDir.FullName))
				tmpBool = true;
			else
				tmpBool = System.IO.Directory.Exists(macroDir.FullName);
			System.Console.Out.WriteLine("       exists: " + tmpBool);
			System.Console.Out.WriteLine("  isDirectory: " + System.IO.Directory.Exists(macroDir.FullName));
			bool tmpBool2;
			if (System.IO.File.Exists(macroDir.FullName))
				tmpBool2 = true;
			else
				tmpBool2 = System.IO.Directory.Exists(macroDir.FullName);
			if (tmpBool2 && System.IO.Directory.Exists(macroDir.FullName))
			{
				System.IO.FileInfo[] macros = SupportClass.FileSupport.GetFiles(macroDir);
				for (int i = 0; i < macros.Length; i++)
				{
					// loop over these files and load them
					System.String macroName = macros[i].Name;
					if (macroName.EndsWith(".macro"))
					{
						System.Console.Out.WriteLine("Possible macro found: " + macroName);
						try
						{
							//UPGRADE_TODO: Constructor 'java.io.FileInputStream.FileInputStream' was converted to 'System.IO.FileStream.FileStream' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileInputStreamFileInputStream_javaioFile'"
							System.IO.FileStream macro = new System.IO.FileStream(macros[i].FullName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
							//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
							//UPGRADE_TODO: Format of property file may need to be changed. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1089'"
							System.Collections.Specialized.NameValueCollection macroProps = new System.Collections.Specialized.NameValueCollection();
							//UPGRADE_TODO: Method 'java.util.Properties.load' was converted to 'System.Collections.Specialized.NameValueCollection' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiesload_javaioInputStream'"
							macroProps = new System.Collections.Specialized.NameValueCollection(System.Configuration.ConfigurationSettings.AppSettings);
							System.String macroTitle = macroProps.Get("Title");
							System.String macroScript = macroProps.Get("Script");
							System.Windows.Forms.MenuItem mi = new System.Windows.Forms.MenuItem(macroTitle);
							SupportClass.CommandManager.SetCommand(mi, macroScript);
							mi.Click += new System.EventHandler(executeScriptAction.actionPerformed);
							SupportClass.CommandManager.CheckCommand(mi);
							macroMenu.MenuItems.Add(mi);
						}
						catch (System.IO.IOException exception)
						{
							System.Console.Error.WriteLine("Could not load macro file: ");
							//UPGRADE_TODO: Method 'java.io.PrintStream.println' was converted to 'System.Console.Error.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintStreamprintln_javalangObject'"
							//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
							System.Console.Error.WriteLine(exception);
						}
					}
				}
			}
			menuBar.MenuItems.Add(macroMenu);
		}
		
		protected internal virtual void  addNormalMenuBar(System.Windows.Forms.MainMenu menuBar)
		{
			System.String[] menuKeys = tokenize(JmolResourceHandler.getStringX("menubar"));
			for (int i = 0; i < menuKeys.Length; i++)
			{
				if (menuKeys[i].Equals("-"))
				{
					//UPGRADE_ISSUE: Method 'javax.swing.Box.createHorizontalGlue' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
					//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
					System.Windows.Forms.Control temp_Control;
					temp_Control = Box.createHorizontalGlue();
					menuBar.Controls.Add(temp_Control);
				}
				else
				{
					System.Windows.Forms.MenuItem m = createMenu(menuKeys[i]);
					if (m != null)
						menuBar.MenuItems.Add(m);
				}
			}
		}
		
		protected internal virtual void  addHelpMenuBar(System.Windows.Forms.MainMenu menuBar)
		{
			System.String menuKey = "help";
			System.Windows.Forms.MenuItem m = createMenu(menuKey);
			if (m != null)
			{
				menuBar.MenuItems.Add(m);
			}
		}
		
		/// <summary> Create a menu for the app.  By default this pulls the
		/// definition of the menu from the associated resource file.
		/// </summary>
		/// <param name="key">
		/// </param>
		/// <returns> Menu created
		/// </returns>
		protected internal virtual System.Windows.Forms.MenuItem createMenu(System.String key)
		{
			
			// Get list of items from resource file:
			System.String[] itemKeys = tokenize(JmolResourceHandler.getStringX(key));
			
			// Get label associated with this menu:
			System.Windows.Forms.MenuItem menu = guimap.newJMenu(key);
			
			// Loop over the items in this menu:
			for (int i = 0; i < itemKeys.Length; i++)
			{
				
				System.String item = itemKeys[i];
				if (item.Equals("-"))
				{
					menu.MenuItems.Add(new System.Windows.Forms.MenuItem("-"));
					continue;
				}
				if (item.EndsWith("Menu"))
				{
					System.Windows.Forms.MenuItem pm;
					if ("recentFilesMenu".Equals(item))
					{
						/*recentFilesMenu = */ pm = createMenu(item);
					}
					else
					{
						pm = createMenu(item);
					}
					menu.MenuItems.Add(pm);
					continue;
				}
				System.Windows.Forms.MenuItem mi = createMenuItem(item);
				menu.MenuItems.Add(mi);
			}
			//UPGRADE_NOTE: Some methods of the 'javax.swing.event.MenuListener' class are not used in the .NET Framework. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1308'"
			menu.Select += new System.EventHandler(display.MenuListener.menuSelected);
			return menu;
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'ActionChangedListener' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class ActionChangedListener
		{
			private void  InitBlock(Jmol enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Jmol enclosingInstance;
			public Jmol Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			internal System.Windows.Forms.ButtonBase button;
			
			internal ActionChangedListener(Jmol enclosingInstance, System.Windows.Forms.ButtonBase button):base()
			{
				InitBlock(enclosingInstance);
				this.button = button;
			}
			
			public virtual void  propertyChange(System.Object event_sender, SupportClass.PropertyChangingEventArgs e)
			{
				
				System.String propertyName = e.PropertyName;
				//UPGRADE_ISSUE: Field 'javax.swing.Action.NAME' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingActionNAME_f'"
				if (e.PropertyName.Equals(Action.NAME))
				{
					System.String text = (System.String) e.NewValue;
					if (button.Text != null)
					{
						button.Text = text;
					}
				}
				else if (propertyName.Equals("enabled"))
				{
					System.Boolean enabledState = (System.Boolean) e.NewValue;
					button.Enabled = enabledState;
				}
			}
		}
		
		private System.Collections.Hashtable commands;
		private System.Collections.Hashtable menuItems;
		private System.Windows.Forms.MainMenu menubar;
		private System.Windows.Forms.ToolBar toolbar;
		
		
		private const System.String newwinAction = "newwin";
		private const System.String openAction = "open";
		private const System.String openurlAction = "openurl";
		private const System.String newAction = "new";
		//private static final String saveasAction = "saveas";
		private const System.String exportActionProperty = "export";
		private const System.String closeAction = "close";
		private const System.String exitAction = "exit";
		private const System.String aboutAction = "about";
		//private static final String vibAction = "vibrate";
		private const System.String whatsnewAction = "whatsnew";
		private const System.String uguideAction = "uguide";
		private const System.String printActionProperty = "print";
		private const System.String recentFilesAction = "recentFiles";
		private const System.String povrayActionProperty = "povray";
		private const System.String pdfActionProperty = "pdf";
		private const System.String scriptAction = "script";
		private const System.String atomsetchooserAction = "atomsetchooser";
		private const System.String copyImageActionProperty = "copyImage";
		
		
		// --- action implementations -----------------------------------
		
		//UPGRADE_NOTE: The initialization of  'exportAction' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private ExportAction exportAction;
		//UPGRADE_NOTE: The initialization of  'povrayAction' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private PovrayAction povrayAction;
		//UPGRADE_NOTE: The initialization of  'pdfAction' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private PdfAction pdfAction;
		//UPGRADE_NOTE: The initialization of  'printAction' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private PrintAction printAction;
		//UPGRADE_NOTE: The initialization of  'copyImageAction' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private CopyImageAction copyImageAction;
		//UPGRADE_NOTE: The initialization of  'viewMeasurementTableAction' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private ViewMeasurementTableAction viewMeasurementTableAction;
		
		
		/// <summary> Actions defined by the Jmol class</summary>
		//UPGRADE_NOTE: The initialization of  'defaultActions' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private SupportClass.ActionSupport[] defaultActions;
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'CloseAction' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class CloseAction:SupportClass.ActionSupport
		{
			private void  InitBlock(Jmol enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Jmol enclosingInstance;
			public Jmol Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			//UPGRADE_TODO: Constructor 'javax.swing.AbstractAction.AbstractAction' was converted to 'ActionSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingAbstractActionAbstractAction_javalangString'"
			internal CloseAction(Jmol enclosingInstance):base(org.openscience.jmol.app.Jmol.closeAction)
			{
				InitBlock(enclosingInstance);
			}
			
			public override void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				Enclosing_Instance.frame.Hide();
				Enclosing_Instance.doClose();
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'ConsoleAction' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class ConsoleAction:SupportClass.ActionSupport
		{
			private void  InitBlock(Jmol enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Jmol enclosingInstance;
			public Jmol Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			//UPGRADE_TODO: Constructor 'javax.swing.AbstractAction.AbstractAction' was converted to 'ActionSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingAbstractActionAbstractAction_javalangString'"
			public ConsoleAction(Jmol enclosingInstance):base("console")
			{
				InitBlock(enclosingInstance);
			}
			
			public override void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				//UPGRADE_TODO: 'System.Windows.Forms.Application.Run' must be called to start a main form. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1135'"
				org.openscience.jmol.app.Jmol.consoleframe.Show();
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AboutAction' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class AboutAction:SupportClass.ActionSupport
		{
			private void  InitBlock(Jmol enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Jmol enclosingInstance;
			public Jmol Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			//UPGRADE_TODO: Constructor 'javax.swing.AbstractAction.AbstractAction' was converted to 'ActionSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingAbstractActionAbstractAction_javalangString'"
			public AboutAction(Jmol enclosingInstance):base(org.openscience.jmol.app.Jmol.aboutAction)
			{
				InitBlock(enclosingInstance);
			}
			
			public override void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				AboutDialog ad = new AboutDialog(Enclosing_Instance.frame);
				//UPGRADE_TODO: Method 'java.awt.Dialog.show' was converted to 'System.Windows.Forms.Form.ShowDialog' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtDialogshow'"
				ad.ShowDialog();
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'WhatsNewAction' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class WhatsNewAction:SupportClass.ActionSupport
		{
			private void  InitBlock(Jmol enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Jmol enclosingInstance;
			public Jmol Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			//UPGRADE_TODO: Constructor 'javax.swing.AbstractAction.AbstractAction' was converted to 'ActionSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingAbstractActionAbstractAction_javalangString'"
			public WhatsNewAction(Jmol enclosingInstance):base(org.openscience.jmol.app.Jmol.whatsnewAction)
			{
				InitBlock(enclosingInstance);
			}
			
			public override void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				WhatsNewDialog wnd = new WhatsNewDialog(Enclosing_Instance.frame);
				//UPGRADE_TODO: Method 'java.awt.Dialog.show' was converted to 'System.Windows.Forms.Form.ShowDialog' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtDialogshow'"
				wnd.ShowDialog();
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'NewwinAction' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class NewwinAction:SupportClass.ActionSupport
		{
			private void  InitBlock(Jmol enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Jmol enclosingInstance;
			public Jmol Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			//UPGRADE_TODO: Constructor 'javax.swing.AbstractAction.AbstractAction' was converted to 'ActionSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingAbstractActionAbstractAction_javalangString'"
			internal NewwinAction(Jmol enclosingInstance):base(org.openscience.jmol.app.Jmol.newwinAction)
			{
				InitBlock(enclosingInstance);
			}
			
			public override void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				System.Windows.Forms.Form newFrame = new System.Windows.Forms.Form();
				new Jmol(null, newFrame, Enclosing_Instance, Enclosing_Instance.startupWidth, Enclosing_Instance.startupHeight);
				//UPGRADE_TODO: 'System.Windows.Forms.Application.Run' must be called to start a main form. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1135'"
				newFrame.Show();
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'UguideAction' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class UguideAction:SupportClass.ActionSupport
		{
			private void  InitBlock(Jmol enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Jmol enclosingInstance;
			public Jmol Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			//UPGRADE_TODO: Constructor 'javax.swing.AbstractAction.AbstractAction' was converted to 'ActionSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingAbstractActionAbstractAction_javalangString'"
			public UguideAction(Jmol enclosingInstance):base(org.openscience.jmol.app.Jmol.uguideAction)
			{
				InitBlock(enclosingInstance);
			}
			
			public override void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				HelpDialog hd = new HelpDialog(Enclosing_Instance.frame);
				//UPGRADE_TODO: Method 'java.awt.Dialog.show' was converted to 'System.Windows.Forms.Form.ShowDialog' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtDialogshow'"
				hd.ShowDialog();
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'CopyImageAction' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		/// <summary> An Action to copy the current image into the clipboard. </summary>
		[Serializable]
		internal class CopyImageAction:SupportClass.ActionSupport
		{
			private void  InitBlock(Jmol enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Jmol enclosingInstance;
			public Jmol Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			//UPGRADE_TODO: Constructor 'javax.swing.AbstractAction.AbstractAction' was converted to 'ActionSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingAbstractActionAbstractAction_javalangString'"
			public CopyImageAction(Jmol enclosingInstance):base(org.openscience.jmol.app.Jmol.copyImageActionProperty)
			{
				InitBlock(enclosingInstance);
			}
			
			public override void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				ImageSelection.Clipboard = Enclosing_Instance.viewer.ScreenImage;
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'PrintAction' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class PrintAction:MoleculeDependentAction
		{
			private void  InitBlock(Jmol enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Jmol enclosingInstance;
			public Jmol Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			public PrintAction(Jmol enclosingInstance):base(org.openscience.jmol.app.Jmol.printActionProperty)
			{
				InitBlock(enclosingInstance);
			}
			
			public override void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				Enclosing_Instance.print();
			}
		}
		
		/// <summary> added print command, so that it can be used by RasmolScriptHandler
		/// 
		/// </summary>
		public virtual void  print()
		{
			
			System.Drawing.Printing.PrintDocument job = new System.Drawing.Printing.PrintDocument();
			//UPGRADE_ISSUE: Method 'java.awt.print.PrinterJob.setPrintable' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtprintPrinterJobsetPrintable_javaawtprintPrintable'"
			job.setPrintable(display);
			if (SupportClass.PrintDialogSupport(job))
			{
				try
				{
					job.Print();
				}
				//UPGRADE_NOTE: Exception 'java.awt.PrinterException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
				catch (System.Exception e)
				{
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					System.Console.Out.WriteLine("" + e);
				}
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'OpenAction' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class OpenAction:NewAction
		{
			private void  InitBlock(Jmol enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Jmol enclosingInstance;
			public new Jmol Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			internal OpenAction(Jmol enclosingInstance):base(enclosingInstance, org.openscience.jmol.app.Jmol.openAction)
			{
				InitBlock(enclosingInstance);
			}
			
			public override void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				
				Enclosing_Instance.openChooser.DialogSize = org.openscience.jmol.app.Jmol.historyFile.getWindowSize(org.openscience.jmol.app.Jmol.FILE_OPEN_WINDOW_NAME);
				Enclosing_Instance.openChooser.DialogLocation = org.openscience.jmol.app.Jmol.historyFile.getWindowPosition(org.openscience.jmol.app.Jmol.FILE_OPEN_WINDOW_NAME);
				//UPGRADE_TODO: The equivalent in .NET for method 'javax.swing.JFileChooser.showOpenDialog' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				int retval = (int) Enclosing_Instance.openChooser.ShowDialog(Enclosing_Instance);
				if (retval == 0)
				{
					System.IO.FileInfo file = new System.IO.FileInfo(Enclosing_Instance.openChooser.FileName);
					Enclosing_Instance.viewer.evalStringQuiet("load " + file.FullName);
					return ;
				}
				org.openscience.jmol.app.Jmol.historyFile.addWindowInfo(org.openscience.jmol.app.Jmol.FILE_OPEN_WINDOW_NAME, Enclosing_Instance.openChooser.Dialog);
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'OpenUrlAction' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class OpenUrlAction:NewAction
		{
			private void  InitBlock(Jmol enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Jmol enclosingInstance;
			public new Jmol Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			internal System.String title;
			internal System.String prompt;
			
			internal OpenUrlAction(Jmol enclosingInstance):base(enclosingInstance, org.openscience.jmol.app.Jmol.openurlAction)
			{
				InitBlock(enclosingInstance);
				title = GT._("Open URL");
				prompt = GT._("Enter URL of molecular model");
			}
			
			public override void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				//UPGRADE_ISSUE: Method 'javax.swing.JOptionPane.showInputDialog' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJOptionPane'"
				System.String url = JOptionPane.showInputDialog(Enclosing_Instance.frame, prompt, title, (int) System.Windows.Forms.MessageBoxIcon.None);
				if (url != null)
				{
					if (url.IndexOf("://") == - 1)
						url = "http://" + url;
					Enclosing_Instance.viewer.openFile(url);
					System.String generatedAux2 = Enclosing_Instance.viewer.OpenFileError;
				}
				return ;
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'NewAction' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class NewAction:SupportClass.ActionSupport
		{
			private void  InitBlock(Jmol enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Jmol enclosingInstance;
			public Jmol Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			//UPGRADE_TODO: Constructor 'javax.swing.AbstractAction.AbstractAction' was converted to 'ActionSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingAbstractActionAbstractAction_javalangString'"
			internal NewAction(Jmol enclosingInstance):base(org.openscience.jmol.app.Jmol.newAction)
			{
				InitBlock(enclosingInstance);
			}
			
			//UPGRADE_TODO: Constructor 'javax.swing.AbstractAction.AbstractAction' was converted to 'ActionSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingAbstractActionAbstractAction_javalangString'"
			internal NewAction(Jmol enclosingInstance, System.String nm):base(nm)
			{
				InitBlock(enclosingInstance);
			}
			
			public override void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				Enclosing_Instance.Invalidate();
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'ExitAction' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		/// <summary> Really lame implementation of an exit command</summary>
		[Serializable]
		internal class ExitAction:SupportClass.ActionSupport
		{
			private void  InitBlock(Jmol enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Jmol enclosingInstance;
			public Jmol Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			//UPGRADE_TODO: Constructor 'javax.swing.AbstractAction.AbstractAction' was converted to 'ActionSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingAbstractActionAbstractAction_javalangString'"
			internal ExitAction(Jmol enclosingInstance):base(org.openscience.jmol.app.Jmol.exitAction)
			{
				InitBlock(enclosingInstance);
			}
			
			public override void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				Enclosing_Instance.doClose();
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'ExportAction' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class ExportAction:MoleculeDependentAction
		{
			private void  InitBlock(Jmol enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Jmol enclosingInstance;
			public Jmol Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			internal ExportAction(Jmol enclosingInstance):base(org.openscience.jmol.app.Jmol.exportActionProperty)
			{
				InitBlock(enclosingInstance);
			}
			
			public override void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				
				ImageTyper it = new ImageTyper(Enclosing_Instance.exportChooser);
				
				//UPGRADE_ISSUE: Method 'javax.swing.JFileChooser.setAccessory' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJFileChoosersetAccessory_javaxswingJComponent'"
				Enclosing_Instance.exportChooser.setAccessory(it);
				
				//UPGRADE_TODO: The equivalent in .NET for method 'javax.swing.JFileChooser.showSaveDialog' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				int retval = (int) Enclosing_Instance.exportChooser.ShowDialog(Enclosing_Instance);
				if (retval == 0)
				{
					System.IO.FileInfo file = new System.IO.FileInfo(Enclosing_Instance.exportChooser.FileName);
					
					System.Console.Out.WriteLine("file chosen=" + file);
					if (file != null)
					{
						try
						{
							System.Drawing.Image eImage = Enclosing_Instance.viewer.ScreenImage;
							//UPGRADE_TODO: Constructor 'java.io.FileOutputStream.FileOutputStream' was converted to 'System.IO.FileStream.FileStream' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileOutputStreamFileOutputStream_javaioFile'"
							System.IO.FileStream os = new System.IO.FileStream(file.FullName, System.IO.FileMode.Create);
							
							if (it.Type.Equals("JPEG"))
							{
								int quality = it.Quality;
								JpegEncoder jc = new JpegEncoder(eImage, quality, os);
								jc.Compress();
							}
							else if (it.Type.Equals("PPM"))
							{
								PpmEncoder pc = new PpmEncoder(eImage, os);
								pc.encode();
							}
							else if (it.Type.Equals("PNG"))
							{
								PngEncoder png = new PngEncoder(eImage);
								sbyte[] pngbytes = png.pngEncode();
								SupportClass.WriteOutput(os, pngbytes);
							}
							else
							{
								
								// Do nothing
							}
							
							os.Flush();
							os.Close();
						}
						catch (System.IO.IOException exc)
						{
							Enclosing_Instance.status.setStatus(1, GT._("IO Exception:"));
							//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
							Enclosing_Instance.status.setStatus(2, exc.ToString());
							//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
							System.Console.Out.WriteLine(exc.ToString());
						}
						Enclosing_Instance.viewer.releaseScreenImage();
						return ;
					}
				}
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'RecentFilesAction' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class RecentFilesAction:SupportClass.ActionSupport
		{
			private void  InitBlock(Jmol enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Jmol enclosingInstance;
			public Jmol Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			//UPGRADE_TODO: Constructor 'javax.swing.AbstractAction.AbstractAction' was converted to 'ActionSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingAbstractActionAbstractAction_javalangString'"
			public RecentFilesAction(Jmol enclosingInstance):base(org.openscience.jmol.app.Jmol.recentFilesAction)
			{
				InitBlock(enclosingInstance);
			}
			
			public override void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				
				//UPGRADE_TODO: Method 'java.awt.Dialog.show' was converted to 'System.Windows.Forms.Form.ShowDialog' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtDialogshow'"
				Enclosing_Instance.recentFiles.ShowDialog();
				System.String selection = Enclosing_Instance.recentFiles.File;
				if (selection != null)
				{
					Enclosing_Instance.viewer.openFile(selection);
					System.String generatedAux = Enclosing_Instance.viewer.OpenFileError;
				}
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'ScriptWindowAction' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class ScriptWindowAction:SupportClass.ActionSupport
		{
			private void  InitBlock(Jmol enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Jmol enclosingInstance;
			public Jmol Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			//UPGRADE_TODO: Constructor 'javax.swing.AbstractAction.AbstractAction' was converted to 'ActionSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingAbstractActionAbstractAction_javalangString'"
			public ScriptWindowAction(Jmol enclosingInstance):base(org.openscience.jmol.app.Jmol.scriptAction)
			{
				InitBlock(enclosingInstance);
			}
			
			public override void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				//UPGRADE_TODO: Method 'java.awt.Dialog.show' was converted to 'System.Windows.Forms.Form.ShowDialog' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtDialogshow'"
				Enclosing_Instance.scriptWindow.ShowDialog();
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AtomSetChooserAction' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class AtomSetChooserAction:SupportClass.ActionSupport
		{
			private void  InitBlock(Jmol enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Jmol enclosingInstance;
			public Jmol Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			//UPGRADE_TODO: Constructor 'javax.swing.AbstractAction.AbstractAction' was converted to 'ActionSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingAbstractActionAbstractAction_javalangString'"
			public AtomSetChooserAction(Jmol enclosingInstance):base(org.openscience.jmol.app.Jmol.atomsetchooserAction)
			{
				InitBlock(enclosingInstance);
			}
			
			public override void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				//UPGRADE_TODO: 'System.Windows.Forms.Application.Run' must be called to start a main form. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1135'"
				Enclosing_Instance.atomSetChooser.Show();
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'PovrayAction' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class PovrayAction:MoleculeDependentAction
		{
			private void  InitBlock(Jmol enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Jmol enclosingInstance;
			public Jmol Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			public PovrayAction(Jmol enclosingInstance):base(org.openscience.jmol.app.Jmol.povrayActionProperty)
			{
				InitBlock(enclosingInstance);
			}
			
			public override void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				
				if (Enclosing_Instance.currentFile != null)
				{
					Enclosing_Instance.currentFile.Name.Substring(0, (Enclosing_Instance.currentFile.Name.LastIndexOf(".")) - (0));
				}
				new PovrayDialog(Enclosing_Instance.frame, Enclosing_Instance.viewer);
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'PdfAction' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class PdfAction:MoleculeDependentAction
		{
			private void  InitBlock(Jmol enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Jmol enclosingInstance;
			public Jmol Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			public PdfAction(Jmol enclosingInstance):base(org.openscience.jmol.app.Jmol.pdfActionProperty)
			{
				InitBlock(enclosingInstance);
			}
			
			public override void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				
				//UPGRADE_ISSUE: Method 'javax.swing.JFileChooser.setAccessory' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJFileChoosersetAccessory_javaxswingJComponent'"
				Enclosing_Instance.exportChooser.setAccessory(null);
				
				//UPGRADE_TODO: The equivalent in .NET for method 'javax.swing.JFileChooser.showSaveDialog' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				int retval = (int) Enclosing_Instance.exportChooser.ShowDialog(Enclosing_Instance);
				if (retval == 0)
				{
					System.IO.FileInfo file = new System.IO.FileInfo(Enclosing_Instance.exportChooser.FileName);
					
					if (file != null)
					{
						Document document = new Document();
						
						try
						{
							//UPGRADE_TODO: Constructor 'java.io.FileOutputStream.FileOutputStream' was converted to 'System.IO.FileStream.FileStream' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileOutputStreamFileOutputStream_javaioFile'"
							PdfWriter writer = PdfWriter.getInstance(document, new System.IO.FileStream(file.FullName, System.IO.FileMode.Create));
							
							document.open();
							
							int w = Enclosing_Instance.display.Width;
							int h = Enclosing_Instance.display.Height;
							PdfContentByte cb = writer.getDirectContent();
							PdfTemplate tp = cb.createTemplate(w, h);
							System.Drawing.Graphics g2 = tp.createGraphics(w, h);
							//UPGRADE_TODO: Method 'java.awt.Graphics2D.setStroke' was converted to 'System.Drawing.Pen' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2DsetStroke_javaawtStroke'"
							//UPGRADE_TODO: Constructor 'java.awt.BasicStroke.BasicStroke' was converted to 'System.Drawing.Pen' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtBasicStrokeBasicStroke_float'"
							SupportClass.GraphicsManager.manager.SetPen(g2, new System.Drawing.Pen(System.Drawing.Brushes.Black, 0.1f));
							tp.setWidth(w);
							tp.setHeight(h);
							
							//UPGRADE_ISSUE: Method 'javax.swing.JComponent.print' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJComponentprint_javaawtGraphics'"
							Enclosing_Instance.display.print(g2);
							g2.Dispose();
							cb.addTemplate(tp, 72, 720 - h);
						}
						catch (DocumentException de)
						{
							//UPGRADE_TODO: Method 'java.io.PrintStream.println' was converted to 'System.Console.Error.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintStreamprintln_javalangObject'"
							System.Console.Error.WriteLine(de.getMessage());
						}
						catch (System.IO.IOException ioe)
						{
							//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
							System.Console.Error.WriteLine(ioe.Message);
						}
						
						document.close();
					}
				}
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'ViewMeasurementTableAction' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class ViewMeasurementTableAction:MoleculeDependentAction
		{
			private void  InitBlock(Jmol enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Jmol enclosingInstance;
			public Jmol Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			public ViewMeasurementTableAction(Jmol enclosingInstance):base("viewMeasurementTable")
			{
				InitBlock(enclosingInstance);
			}
			
			public override void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				Enclosing_Instance.measurementTable.activate();
			}
		}
		
		public const System.String chemFileProperty = "chemFile";
		
		//UPGRADE_NOTE: The access modifier for this class or class field has been changed in order to prevent compilation errors due to the visibility level. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1296'"
		[Serializable]
		abstract internal class MoleculeDependentAction:SupportClass.ActionSupport
		{
			
			//UPGRADE_TODO: Constructor 'javax.swing.AbstractAction.AbstractAction' was converted to 'ActionSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingAbstractActionAbstractAction_javalangString'"
			public MoleculeDependentAction(System.String name):base(name)
			{
				//UPGRADE_ISSUE: Method 'javax.swing.AbstractAction.setEnabled' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractActionsetEnabled_boolean'"
				setEnabled(false);
			}
			
			public virtual void  propertyChange(System.Object event_sender, SupportClass.PropertyChangingEventArgs event_Renamed)
			{
				
				if (event_Renamed.PropertyName.Equals(org.openscience.jmol.app.Jmol.chemFileProperty))
				{
					if (event_Renamed.NewValue != null)
					{
						//UPGRADE_ISSUE: Method 'javax.swing.AbstractAction.setEnabled' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractActionsetEnabled_boolean'"
						setEnabled(true);
					}
					else
					{
						//UPGRADE_ISSUE: Method 'javax.swing.AbstractAction.setEnabled' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractActionsetEnabled_boolean'"
						setEnabled(false);
					}
				}
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'MyJmolStatusListener' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		internal class MyJmolStatusListener : JmolStatusListener
		{
			public MyJmolStatusListener(Jmol enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(Jmol enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Jmol enclosingInstance;
			virtual public System.String StatusMessage
			{
				set
				{
					System.Console.Out.WriteLine("setStatusMessage:" + value);
				}
				
			}
			public Jmol Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public virtual void  notifyFileLoaded(System.String fullPathName, System.String fileName, System.String modelName, System.Object clientFile, System.String errorMsg)
			{
				if (errorMsg != null)
				{
					SupportClass.OptionPaneSupport.ShowMessageDialog(null, fullPathName + '\n' + errorMsg, GT._("File not loaded"), (int) System.Windows.Forms.MessageBoxIcon.Error);
					return ;
				}
				Enclosing_Instance.jmolpopup.updateComputedMenus();
				if (fullPathName == null)
				{
					// a 'clear/zap' operation
					return ;
				}
				System.String title = "Jmol";
				if (modelName != null && fileName != null)
					title = fileName + " - " + modelName;
				else if (fileName != null)
					title = fileName;
				else if (modelName != null)
					title = modelName;
				Enclosing_Instance.frame.Text = title;
				Enclosing_Instance.recentFiles.notifyFileOpen(fullPathName);
				SupportClass.PropertyChangingEventArgs me = new SupportClass.PropertyChangingEventArgs(org.openscience.jmol.app.Jmol.chemFileProperty, null, clientFile);
				if (PropertyChange != null)
					PropertyChange(this, me);
			}
			
			public virtual void  notifyFrameChanged(int frameNo)
			{
				// don't do anything
			}
			
			public virtual void  scriptEcho(System.String strEcho)
			{
				if (Enclosing_Instance.scriptWindow != null)
					Enclosing_Instance.scriptWindow.scriptEcho(strEcho);
			}
			
			public virtual void  scriptStatus(System.String strStatus)
			{
				if (Enclosing_Instance.scriptWindow != null)
					Enclosing_Instance.scriptWindow.scriptStatus(strStatus);
			}
			
			public virtual void  notifyScriptTermination(System.String strStatus, int msWalltime)
			{
				if (Enclosing_Instance.scriptWindow != null)
					Enclosing_Instance.scriptWindow.notifyScriptTermination(strStatus, msWalltime);
			}
			
			public virtual void  handlePopupMenu(int x, int y)
			{
				Enclosing_Instance.jmolpopup.show(x, y);
			}
			
			public virtual void  notifyMeasurementsChanged()
			{
				Enclosing_Instance.measurementTable.updateTables();
			}
			
			public virtual void  notifyAtomPicked(int atomIndex, System.String strInfo)
			{
				if (Enclosing_Instance.scriptWindow != null)
				{
					Enclosing_Instance.scriptWindow.scriptStatus(strInfo);
					Enclosing_Instance.scriptWindow.scriptStatus("\n");
				}
			}
			
			public virtual void  showUrl(System.String url)
			{
			}
			
			public virtual void  showConsole(bool showConsole)
			{
				if (showConsole)
				{
					//UPGRADE_TODO: Method 'java.awt.Dialog.show' was converted to 'System.Windows.Forms.Form.ShowDialog' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtDialogshow'"
					Enclosing_Instance.scriptWindow.ShowDialog();
				}
				else
					Enclosing_Instance.scriptWindow.Hide();
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'ExecuteScriptAction' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class ExecuteScriptAction:SupportClass.ActionSupport
		{
			private void  InitBlock(Jmol enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Jmol enclosingInstance;
			public Jmol Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			//UPGRADE_TODO: Constructor 'javax.swing.AbstractAction.AbstractAction' was converted to 'ActionSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingAbstractActionAbstractAction_javalangString'"
			public ExecuteScriptAction(Jmol enclosingInstance):base("executeScriptAction")
			{
				InitBlock(enclosingInstance);
			}
			
			public override void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				Enclosing_Instance.viewer.evalStringQuiet(SupportClass.CommandManager.GetCommand(event_sender));
			}
		}
		static Jmol()
		{
			{
				//UPGRADE_ISSUE: Method 'java.lang.System.getProperty' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangSystem'"
				if (System_Renamed.getProperty("javawebstart.version") != null)
				{
					
					// If the property is found, Jmol is running with Java Web Start. To fix
					// bug 4621090, the security manager is set to null.
					//UPGRADE_ISSUE: Method 'java.lang.System.setSecurityManager' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangSystem'"
					System_Renamed.setSecurityManager(null);
				}
				//UPGRADE_TODO: Method 'java.lang.System.getProperty' was converted to 'System.Environment.GetEnvironmentVariable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangSystemgetProperty_javalangString'"
				if (System.Environment.GetEnvironmentVariable("userprofile") == null)
				{
					System.Console.Error.WriteLine(GT._("Error starting Jmol: the property 'user.home' is not defined."));
					System.Environment.Exit(1);
				}
				//UPGRADE_TODO: Method 'java.lang.System.getProperty' was converted to 'System.Environment.GetEnvironmentVariable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangSystemgetProperty_javalangString'"
				System.IO.FileInfo ujmoldir = new System.IO.FileInfo(new System.IO.FileInfo(System.Environment.GetEnvironmentVariable("userprofile")).FullName + "\\" + ".jmol");
				//UPGRADE_TODO: Method 'java.io.File.mkdirs' was converted to 'System.IO.Directory.CreateDirectory' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFilemkdirs'"
				System.IO.Directory.CreateDirectory(ujmoldir.FullName);
				UserPropsFile = new System.IO.FileInfo(ujmoldir.FullName + "\\" + "properties");
				historyFile = new HistoryFile(new System.IO.FileInfo(ujmoldir.FullName + "\\" + "history"), "Jmol's persistent values");
			}
		}
	}
}