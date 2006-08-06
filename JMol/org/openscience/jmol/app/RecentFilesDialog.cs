/* $RCSfile$
* $Author: migueljmol $
* $Date: 2005-11-27 22:39:31 +0100 (dim., 27 nov. 2005) $
* $Revision: 4285 $
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
using GT = org.jmol.i18n.GT;
namespace org.openscience.jmol.app
{
	
	/// <summary> Manages a list of recently opened files.
	/// 
	/// </summary>
	/// <author>  Bradley A. Smith (bradley@baysmith.com)
	/// </author>
	[Serializable]
	class RecentFilesDialog:System.Windows.Forms.Form
	{
		static private System.Int32 state99;
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassMouseAdapter' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassMouseAdapter
		{
			public AnonymousClassMouseAdapter(RecentFilesDialog enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(RecentFilesDialog enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private RecentFilesDialog enclosingInstance;
			public RecentFilesDialog Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public void  mouseClicked(System.Object event_sender, System.EventArgs e)
			{
				//UPGRADE_ISSUE: Method 'java.awt.event.MouseEvent.getClickCount' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventMouseEventgetClickCount'"
				if (e.getClickCount() == 2)
				{
					//UPGRADE_ISSUE: Method 'java.awt.event.MouseEvent.getPoint' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventMouseEventgetPoint'"
					int dblClickIndex = Enclosing_Instance.fileList.IndexFromPoint(e.getPoint());
					if (dblClickIndex >= 0 && dblClickIndex < Enclosing_Instance.files.Length && Enclosing_Instance.files[dblClickIndex] != null)
					{
						Enclosing_Instance.selectedFileName = Enclosing_Instance.files[dblClickIndex];
						Enclosing_Instance.close();
					}
				}
			}
		}
		private static void  mouseDown(System.Object event_sender, System.Windows.Forms.MouseEventArgs e)
		{
			state99 = ((int) e.Button | (int) System.Windows.Forms.Control.ModifierKeys);
		}
		/// <returns> String The name of the file picked or null if the action was aborted.
		/// 
		/// </returns>
		virtual public System.String File
		{
			get
			{
				return selectedFileName;
			}
			
		}
		
		internal System.String selectedFileName = null;
		private const int MAX_FILES = 10;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		internal System.String[] files = new System.String[MAX_FILES];
		internal System.Windows.Forms.ListBox fileList;
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		internal System.Collections.Specialized.NameValueCollection props;
		
		/// <summary>Creates a hidden recent files dialog</summary>
		/// <param name="boss">
		/// </param>
		//UPGRADE_TODO: Class 'java.awt.Frame' was converted to 'System.Windows.Forms.Form' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtFrame'"
		public RecentFilesDialog(System.Windows.Forms.Form boss):base()
		{
			//UPGRADE_TODO: Constructor 'javax.swing.JDialog.JDialog' was converted to 'SupportClass.DialogSupport.SetDialog' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJDialogJDialog_javaawtFrame_javalangString_boolean'"
			SupportClass.DialogSupport.SetDialog(this, boss, GT._("Recent Files"));
			//UPGRADE_TODO: Format of property file may need to be changed. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1089'"
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			props = new System.Collections.Specialized.NameValueCollection();
			getFiles();
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtBorderLayout'"
			/*
			((System.Windows.Forms.ContainerControl) this).setLayout(new java.awt.BorderLayout());*/
			System.Windows.Forms.Panel buttonPanel = new System.Windows.Forms.Panel();
			okButton = SupportClass.ButtonSupport.CreateStandardButton(GT._("Open"));
			okButton.Click += new System.EventHandler(this.actionPerformed);
			SupportClass.CommandManager.CheckCommand(okButton);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			buttonPanel.Controls.Add(okButton);
			cancelButton = SupportClass.ButtonSupport.CreateStandardButton(GT._("Cancel"));
			cancelButton.Click += new System.EventHandler(this.actionPerformed);
			SupportClass.CommandManager.CheckCommand(cancelButton);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			buttonPanel.Controls.Add(cancelButton);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javalangString_javaawtComponent'"
			((System.Windows.Forms.ContainerControl) this).Controls.Add(buttonPanel);
			
			//UPGRADE_TODO: Constructor 'javax.swing.JList.JList' was converted to 'System.Windows.Forms.ListBox.ListBox' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJListJList_javalangObject[]'"
			System.Windows.Forms.ListBox temp_ListBox;
			temp_ListBox = new System.Windows.Forms.ListBox();
			temp_ListBox.Items.AddRange(files);
			temp_ListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			fileList = temp_ListBox;
			//UPGRADE_TODO: Method 'javax.swing.JList.setSelectedIndex' was converted to 'System.Windows.Forms.ListBox.SelectedIndex' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJListsetSelectedIndex_int'"
			fileList.SelectedIndex = 0;
			//UPGRADE_TODO: The equivalent in .NET for field 'javax.swing.ListSelectionModel.SINGLE_SELECTION' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			fileList.SelectionMode = (System.Windows.Forms.SelectionMode) System.Windows.Forms.SelectionMode.One;
			
			//UPGRADE_TODO: Interface 'java.awt.event.MouseListener' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
			MouseListener dblClickListener = new AnonymousClassMouseAdapter(this);
			fileList.MouseDown += new System.Windows.Forms.MouseEventHandler(org.openscience.jmol.app.RecentFilesDialog.mouseDown);
			fileList.Click += new System.EventHandler(dblClickListener.mouseClicked);
			fileList.MouseEnter += new System.EventHandler(dblClickListener.mouseEntered);
			fileList.MouseLeave += new System.EventHandler(dblClickListener.mouseExited);
			fileList.MouseDown += new System.Windows.Forms.MouseEventHandler(dblClickListener.mousePressed);
			fileList.MouseUp += new System.Windows.Forms.MouseEventHandler(dblClickListener.mouseReleased);
			
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javalangString_javaawtComponent'"
			((System.Windows.Forms.ContainerControl) this).Controls.Add(fileList);
			//    System.out.println("I am setting my location relative to:" + boss);
			//UPGRADE_TODO: Method 'java.awt.Component.setLocation' was converted to 'System.Windows.Forms.Control.Location' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetLocation_int_int'"
			Location = new System.Drawing.Point(100, 100);
			//UPGRADE_ISSUE: Method 'java.awt.Window.pack' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtWindowpack'"
			pack();
		}
		
		private void  getFiles()
		{
			
			props = Jmol.HistoryFile.Properties;
			for (int i = 0; i < MAX_FILES; i++)
			{
				files[i] = props.Get("recentFilesFile" + i);
			}
		}
		
		/// <summary> Adds this file to the history. If already present,
		/// this file is premoted to the top position.
		/// </summary>
		/// <param name="name">Name of the file
		/// </param>
		public virtual void  addFile(System.String name)
		{
			
			int currentPosition = - 1;
			
			//Find if file is already present
			for (int i = 0; i < MAX_FILES; i++)
			{
				if ((files[i] != null) && files[i].Equals(name))
				{
					currentPosition = i;
				}
			}
			
			//No change so cope out
			if (currentPosition == 0)
			{
				return ;
			}
			
			//present so shift files below current position up one,
			//removing current position
			if (currentPosition > 0)
			{
				for (int i = currentPosition; i < MAX_FILES - 1; i++)
				{
					files[i] = files[i + 1];
				}
			}
			
			// Shift everything down one
			for (int j = MAX_FILES - 2; j >= 0; j--)
			{
				files[j + 1] = files[j];
			}
			
			//Insert file at head of list
			files[0] = name;
			fileList.Items.Clear();
			fileList.Items.AddRange(new System.Collections.ArrayList(files).ToArray());
			//UPGRADE_TODO: Method 'javax.swing.JList.setSelectedIndex' was converted to 'System.Windows.Forms.ListBox.SelectedIndex' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJListsetSelectedIndex_int'"
			fileList.SelectedIndex = 0;
			//UPGRADE_ISSUE: Method 'java.awt.Window.pack' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtWindowpack'"
			pack();
			saveList();
		}
		
		/// <summary>Saves the list to the history file. Called automaticaly when files are added *</summary>
		public virtual void  saveList()
		{
			
			for (int i = 0; i < 10; i++)
			{
				if (files[i] != null)
				{
					//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
					props["recentFilesFile" + i] = files[i];
				}
			}
			
			Jmol.HistoryFile.addProperties(props);
		}
		
		public virtual void  windowClosing(System.Object event_sender, System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel = true;
			cancel();
			close();
		}
		
		internal virtual void  cancel()
		{
			selectedFileName = null;
		}
		
		internal virtual void  close()
		{
			Hide();
		}
		
		public virtual void  actionPerformed(System.Object event_sender, System.EventArgs e)
		{
			
			if (event_sender == okButton)
			{
				int fileIndex = fileList.SelectedIndex;
				if (fileIndex < files.Length)
				{
					selectedFileName = files[fileIndex];
					close();
				}
			}
			else if (event_sender == cancelButton)
			{
				cancel();
				close();
			}
		}
		
		public virtual void  windowClosed(System.Object event_sender, System.EventArgs e)
		{
		}
		
		public virtual void  windowOpened(System.Object event_sender, System.EventArgs e)
		{
		}
		
		public virtual void  windowIconified(System.Object event_sender, System.EventArgs e)
		{
		}
		
		public virtual void  windowDeiconified(System.Object event_sender, System.EventArgs e)
		{
		}
		
		public virtual void  windowActivated(System.Object event_sender, System.EventArgs e)
		{
		}
		
		public virtual void  windowDeactivated(System.Object event_sender, System.EventArgs e)
		{
		}
		
		public virtual void  notifyFileOpen(System.String fullPathName)
		{
			if (fullPathName != null)
				addFile(fullPathName);
		}
	}
}