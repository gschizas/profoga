/* $RCSfile$
* $Author: migueljmol $
* $Date: 2005-11-27 22:39:31 +0100 (dim., 27 nov. 2005) $
* $Revision: 4285 $
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
using JmolAdapter = org.jmol.api.JmolAdapter;
using JmolViewer = org.jmol.api.JmolViewer;
using GT = org.jmol.i18n.GT;
namespace org.openscience.jmol.app
{
	
	/// <summary> File previsualisation before opening</summary>
	[Serializable]
	public class FilePreview:System.Windows.Forms.Panel
	{
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassActionListener' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassActionListener
		{
			public AnonymousClassActionListener(FilePreview enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(FilePreview enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private FilePreview enclosingInstance;
			public FilePreview Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public virtual void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				if (Enclosing_Instance.active.Checked)
				{
					Enclosing_Instance.updatePreview(new System.IO.FileInfo(Enclosing_Instance.chooser.FileName));
				}
				else
				{
					Enclosing_Instance.updatePreview(null);
				}
			}
		}
		
		internal System.Windows.Forms.CheckBox active = null;
		internal System.Windows.Forms.FileDialog chooser = null;
		private JmolPanel display = null;
		
		/// <summary> Constructor</summary>
		/// <param name="fileChooser">File chooser
		/// </param>
		/// <param name="modelAdapter">Model adapter
		/// </param>
		public FilePreview(System.Windows.Forms.FileDialog fileChooser, JmolAdapter modelAdapter):base()
		{
			chooser = fileChooser;
			
			// Create a box to do the layout
			//UPGRADE_ISSUE: Class 'javax.swing.Box' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createVerticalBox' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			Box box = Box.createVerticalBox();
			
			// Add a checkbox to activate / deactivate preview
			active = SupportClass.CheckBoxSupport.CreateCheckBox(GT._("Preview"), false);
			active.Click += new System.EventHandler(new AnonymousClassActionListener(this).actionPerformed);
			SupportClass.CommandManager.CheckCommand(active);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			box.Controls.Add(active);
			
			// Add a preview area
			display = new JmolPanel(modelAdapter);
			//UPGRADE_TODO: Method 'javax.swing.JComponent.setPreferredSize' was converted to 'System.Windows.Forms.Control.Size' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
			display.Size = new System.Drawing.Size(80, 80);
			//UPGRADE_ISSUE: Method 'javax.swing.JComponent.setMinimumSize' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJComponentsetMinimumSize_javaawtDimension'"
			display.setMinimumSize(new System.Drawing.Size(50, 50));
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			box.Controls.Add(display);
			
			// Add the preview to the File Chooser
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			Controls.Add(box);
			//UPGRADE_ISSUE: Method 'javax.swing.JFileChooser.setAccessory' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJFileChoosersetAccessory_javaxswingJComponent'"
			fileChooser.setAccessory(this);
			//UPGRADE_ISSUE: Method 'javax.swing.JComponent.addPropertyChangeListener' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJComponentaddPropertyChangeListener_javabeansPropertyChangeListener'"
			fileChooser.addPropertyChangeListener(this);
		}
		
		/* (non-Javadoc)
		* @see java.beans.PropertyChangeListener#propertyChange(java.beans.PropertyChangeEvent)
		*/
		public virtual void  propertyChange(System.Object event_sender, SupportClass.PropertyChangingEventArgs evt)
		{
			if (active.Checked)
			{
				System.String prop = evt.PropertyName;
				if ("SelectedFilesChangedProperty".Equals(prop))
				{
					updatePreview((System.IO.FileInfo) evt.NewValue);
				}
			}
		}
		
		/// <summary> Update preview
		/// 
		/// </summary>
		/// <param name="file">File selected
		/// </param>
		internal virtual void  updatePreview(System.IO.FileInfo file)
		{
			if (file != null)
			{
				display.Viewer.evalStringQuiet("load " + file.FullName);
				//UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
				display.Refresh();
			}
			else
			{
				display.Viewer.evalStringQuiet("zap");
			}
		}
	}
	
	[Serializable]
	class JmolPanel:System.Windows.Forms.Panel
	{
		virtual public JmolViewer Viewer
		{
			get
			{
				return viewer;
			}
			
		}
		internal JmolViewer viewer;
		
		internal JmolPanel(JmolAdapter modelAdapter)
		{
			viewer = JmolViewer.allocateViewer(this, modelAdapter);
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'currentSize '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal System.Drawing.Size currentSize
		{
			get
			{
				return currentSize_Renamed;
			}
			
			set
			{
				currentSize_Renamed = value;
			}
			
		}
		internal System.Drawing.Size currentSize_Renamed = new System.Drawing.Size(0, 0);
		
		protected override void  OnPaint(System.Windows.Forms.PaintEventArgs g_EventArg)
		{
			System.Drawing.Graphics g = null;
			if (g_EventArg != null)
				g = g_EventArg.Graphics;
			//UPGRADE_TODO: Method 'javax.swing.JComponent.getSize' was converted to 'System.Drawing.Size' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJComponentgetSize_javaawtDimension'"
			currentSize = Size;
			viewer.ScreenDimension = Size;
			System.Drawing.Rectangle rectClip = new System.Drawing.Rectangle();
			rectClip = System.Drawing.Rectangle.Truncate(g.ClipBounds);
			System.Drawing.Rectangle generatedAux = rectClip;
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
			viewer.renderScreenImage(g, ref currentSize, ref rectClip);
		}
	}
}