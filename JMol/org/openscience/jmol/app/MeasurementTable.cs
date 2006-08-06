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
using org.jmol.api;
using GT = org.jmol.i18n.GT;
namespace org.openscience.jmol.app
{
	
	[Serializable]
	public class MeasurementTable:System.Windows.Forms.Form
	{
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassListSelectionListener' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassListSelectionListener
		{
			public AnonymousClassListSelectionListener(MeasurementTable enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(MeasurementTable enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private MeasurementTable enclosingInstance;
			public MeasurementTable Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public virtual void  valueChanged(System.Object event_sender, System.EventArgs e)
			{
				//UPGRADE_ISSUE: Method 'javax.swing.event.ListSelectionEvent.getValueIsAdjusting' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingeventListSelectionEventgetValueIsAdjusting'"
				if (e.getValueIsAdjusting())
					return ;
				//UPGRADE_TODO: Interface 'javax.swing.ListSelectionModel' was converted to 'SupportClass.ListSelectionModelSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
				//UPGRADE_TODO: The method 'javax.swing.event.ListSelectionEvent.getSource' needs to be in a event handling method in order to be properly converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1171'"
				SupportClass.ListSelectionModelSupport lsm = (SupportClass.ListSelectionModelSupport) e.getSource();
				if (lsm.SelectedItems.Count.Equals(0))
				{
					Enclosing_Instance.selectedMeasurementRow = - 1;
					Enclosing_Instance.deleteButton.Enabled = false;
				}
				else
				{
					Enclosing_Instance.selectedMeasurementRow = lsm.GetMinSelectionIndex();
					Enclosing_Instance.deleteButton.Enabled = true;
				}
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassActionListener' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassActionListener
		{
			public AnonymousClassActionListener(MeasurementTable enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(MeasurementTable enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private MeasurementTable enclosingInstance;
			public MeasurementTable Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public virtual void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				Enclosing_Instance.viewer.deleteMeasurement(Enclosing_Instance.selectedMeasurementRow);
				Enclosing_Instance.updateMeasurementTableData();
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassActionListener1' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassActionListener1
		{
			public AnonymousClassActionListener1(MeasurementTable enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(MeasurementTable enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private MeasurementTable enclosingInstance;
			public MeasurementTable Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public virtual void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				Enclosing_Instance.viewer.clearMeasurements();
				Enclosing_Instance.updateMeasurementTableData();
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassActionListener2' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassActionListener2
		{
			public AnonymousClassActionListener2(MeasurementTable enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(MeasurementTable enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private MeasurementTable enclosingInstance;
			public MeasurementTable Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public virtual void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				Enclosing_Instance.close();
			}
		}
		
		internal JmolViewer viewer;
		//UPGRADE_TODO: Class 'javax.swing.JTable' was converted to 'System.Windows.Forms.DataGrid' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
		private System.Windows.Forms.DataGrid measurementTable;
		private MeasurementTableModel measurementTableModel;
		//UPGRADE_TODO: Interface 'javax.swing.ListSelectionModel' was converted to 'SupportClass.ListSelectionModelSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
		private SupportClass.ListSelectionModelSupport measurementSelection;
		internal int selectedMeasurementRow = - 1;
		internal System.Windows.Forms.Button deleteButton;
		private System.Windows.Forms.Button deleteAllButton;
		
		/// <summary> Constructor
		/// 
		/// </summary>
		/// <param name="parentFrame">the parent frame
		/// </param>
		/// <param name="viewer">the JmolViewer in which the animation will take place (?)
		/// </param>
		public MeasurementTable(JmolViewer viewer, System.Windows.Forms.Form parentFrame):base()
		{
			//UPGRADE_TODO: Constructor 'javax.swing.JDialog.JDialog' was converted to 'SupportClass.DialogSupport.SetDialog' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJDialogJDialog_javaawtFrame_javalangString_boolean'"
			SupportClass.DialogSupport.SetDialog(this, parentFrame, GT._("Measurements..."));
			this.viewer = viewer;
			
			System.Windows.Forms.Panel container = new System.Windows.Forms.Panel();
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtBorderLayout'"
			/*
			container.setLayout(new BorderLayout());*/
			
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			System.Windows.Forms.Control temp_Control;
			temp_Control = constructMeasurementTable();
			container.Controls.Add(temp_Control);
			temp_Control.Dock = System.Windows.Forms.DockStyle.Fill;
			temp_Control.BringToFront();
			
			System.Windows.Forms.Panel foo = new System.Windows.Forms.Panel();
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtBorderLayout'"
			/*
			foo.setLayout(new BorderLayout());*/
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			System.Windows.Forms.Control temp_Control2;
			temp_Control2 = constructMeasurementButtonPanel();
			foo.Controls.Add(temp_Control2);
			temp_Control2.Dock = System.Windows.Forms.DockStyle.Left;
			temp_Control2.BringToFront();
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			System.Windows.Forms.Control temp_Control3;
			temp_Control3 = constructDismissButtonPanel();
			foo.Controls.Add(temp_Control3);
			temp_Control3.Dock = System.Windows.Forms.DockStyle.Right;
			temp_Control3.BringToFront();
			
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			container.Controls.Add(foo);
			foo.Dock = System.Windows.Forms.DockStyle.Bottom;
			foo.SendToBack();
			
			//UPGRADE_NOTE: Some methods of the 'java.awt.event.WindowListener' class are not used in the .NET Framework. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1308'"
			Closing += new System.ComponentModel.CancelEventHandler(new MeasurementListWindowListener(this).windowClosing);
			
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			((System.Windows.Forms.ContainerControl) this).Controls.Add(container);
			//UPGRADE_ISSUE: Method 'java.awt.Window.pack' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtWindowpack'"
			pack();
			centerDialog();
		}
		
		internal virtual System.Windows.Forms.Control constructMeasurementTable()
		{
			measurementTableModel = new MeasurementTableModel(this);
			System.Windows.Forms.DataGrid temp_DataGrid;
			temp_DataGrid = new System.Windows.Forms.DataGrid();
			temp_DataGrid.DataSource = measurementTableModel;
			measurementTable = temp_DataGrid;
			
			//UPGRADE_ISSUE: Method 'javax.swing.JTable.setPreferredScrollableViewportSize' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJTablesetPreferredScrollableViewportSize_javaawtDimension'"
			measurementTable.setPreferredScrollableViewportSize(new System.Drawing.Size(300, 100));
			
			//UPGRADE_ISSUE: Method 'javax.swing.table.TableColumn.setPreferredWidth' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtableTableColumnsetPreferredWidth_int'"
			//UPGRADE_TODO: The equivalent in .NET for method 'javax.swing.table.TableColumnModel.getColumn' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			((System.Data.DataTable) measurementTable.DataSource).Columns[0].setPreferredWidth(50);
			for (int i = 5; --i > 0; )
			{
				//UPGRADE_ISSUE: Method 'javax.swing.table.TableColumn.setPreferredWidth' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtableTableColumnsetPreferredWidth_int'"
				//UPGRADE_TODO: The equivalent in .NET for method 'javax.swing.table.TableColumnModel.getColumn' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				((System.Data.DataTable) measurementTable.DataSource).Columns[i].setPreferredWidth(15);
			}
			
			//UPGRADE_ISSUE: Method 'javax.swing.JTable.setSelectionMode' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJTablesetSelectionMode_int'"
			//UPGRADE_TODO: The equivalent in .NET for field 'javax.swing.ListSelectionModel.SINGLE_SELECTION' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			measurementTable.setSelectionMode((int) System.Windows.Forms.SelectionMode.One);
			//UPGRADE_ISSUE: Method 'javax.swing.JTable.setRowSelectionAllowed' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJTablesetRowSelectionAllowed_boolean'"
			measurementTable.setRowSelectionAllowed(true);
			//UPGRADE_ISSUE: Method 'javax.swing.JTable.setColumnSelectionAllowed' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJTablesetColumnSelectionAllowed_boolean'"
			measurementTable.setColumnSelectionAllowed(false);
			//UPGRADE_ISSUE: Method 'javax.swing.JTable.getSelectionModel' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJTablegetSelectionModel'"
			measurementSelection = measurementTable.getSelectionModel();
			//UPGRADE_ISSUE: Method 'javax.swing.ListSelectionModel.addListSelectionListener' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingListSelectionModeladdListSelectionListener_javaxswingeventListSelectionListener'"
			measurementSelection.addListSelectionListener(new AnonymousClassListSelectionListener(this));
			
			//UPGRADE_TODO: Constructor 'javax.swing.JScrollPane.JScrollPane' was converted to 'System.Windows.Forms.ScrollableControl.ScrollableControl' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJScrollPaneJScrollPane_javaawtComponent'"
			System.Windows.Forms.ScrollableControl temp_scrollablecontrol;
			temp_scrollablecontrol = new System.Windows.Forms.ScrollableControl();
			temp_scrollablecontrol.AutoScroll = true;
			temp_scrollablecontrol.Controls.Add(measurementTable);
			return temp_scrollablecontrol;
		}
		
		internal virtual System.Windows.Forms.Control constructMeasurementButtonPanel()
		{
			System.Windows.Forms.Panel measurementButtonPanel = new System.Windows.Forms.Panel();
			//UPGRADE_TODO: Constructor 'java.awt.FlowLayout.FlowLayout' was converted to 'System.Object[]' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtFlowLayoutFlowLayout_int'"
			measurementButtonPanel.Tag = new System.Object[]{(int) System.Drawing.ContentAlignment.TopLeft, 5, 5};
			measurementButtonPanel.Layout += new System.Windows.Forms.LayoutEventHandler(SupportClass.FlowLayoutResize);
			
			deleteButton = SupportClass.ButtonSupport.CreateStandardButton(GT._("Delete"));
			deleteButton.Click += new System.EventHandler(new AnonymousClassActionListener(this).actionPerformed);
			SupportClass.CommandManager.CheckCommand(deleteButton);
			deleteButton.Enabled = false;
			
			deleteAllButton = SupportClass.ButtonSupport.CreateStandardButton(GT._("DeleteAll"));
			deleteAllButton.Click += new System.EventHandler(new AnonymousClassActionListener1(this).actionPerformed);
			SupportClass.CommandManager.CheckCommand(deleteAllButton);
			deleteAllButton.Enabled = false;
			
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			measurementButtonPanel.Controls.Add(deleteAllButton);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			measurementButtonPanel.Controls.Add(deleteButton);
			return measurementButtonPanel;
		}
		
		internal virtual System.Windows.Forms.Control constructDismissButtonPanel()
		{
			System.Windows.Forms.Panel dismissButtonPanel = new System.Windows.Forms.Panel();
			//UPGRADE_TODO: Constructor 'java.awt.FlowLayout.FlowLayout' was converted to 'System.Object[]' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtFlowLayoutFlowLayout_int'"
			dismissButtonPanel.Tag = new System.Object[]{(int) System.Drawing.ContentAlignment.TopRight, 5, 5};
			dismissButtonPanel.Layout += new System.Windows.Forms.LayoutEventHandler(SupportClass.FlowLayoutResize);
			
			System.Windows.Forms.Button dismissButton = SupportClass.ButtonSupport.CreateStandardButton(GT._("Dismiss"));
			dismissButton.Click += new System.EventHandler(new AnonymousClassActionListener2(this).actionPerformed);
			SupportClass.CommandManager.CheckCommand(dismissButton);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			dismissButtonPanel.Controls.Add(dismissButton);
			//UPGRADE_TODO: Method 'javax.swing.JRootPane.setDefaultButton' was converted to 'System.Windows.Forms.Form.AcceptButton' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJRootPanesetDefaultButton_javaxswingJButton'"
			dismissButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.AcceptButton = dismissButton;
			return dismissButtonPanel;
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
		
		public virtual void  close()
		{
			//UPGRADE_TODO: Method 'java.awt.Component.setVisible' was converted to 'System.Windows.Forms.Control.Visible' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetVisible_boolean'"
			//UPGRADE_TODO: 'System.Windows.Forms.Application.Run' must be called to start a main form. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1135'"
			this.Visible = false;
		}
		
		public virtual void  activate()
		{
			updateMeasurementTableData();
			//UPGRADE_TODO: Method 'java.awt.Dialog.show' was converted to 'System.Windows.Forms.Form.ShowDialog' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtDialogshow'"
			ShowDialog();
		}
		
		internal virtual void  updateMeasurementTableData()
		{
			deleteAllButton.Enabled = viewer.MeasurementCount > 0;
			//UPGRADE_ISSUE: Method 'javax.swing.table.AbstractTableModel.fireTableDataChanged' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtableAbstractTableModelfireTableDataChanged'"
			measurementTableModel.fireTableDataChanged();
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'MeasurementListWindowListener' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		internal class MeasurementListWindowListener
		{
			public MeasurementListWindowListener(MeasurementTable enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(MeasurementTable enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private MeasurementTable enclosingInstance;
			public MeasurementTable Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			public void  windowClosing(System.Object event_sender, System.ComponentModel.CancelEventArgs e)
			{
				e.Cancel = true;
				Enclosing_Instance.close();
			}
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'stringClass '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal System.Type stringClass = "".GetType();
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'MeasurementTableModel' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		//UPGRADE_TODO: Class 'javax.swing.table.AbstractTableModel' was converted to 'System.Data.DataTable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
		[Serializable]
		internal class MeasurementTableModel:System.Data.DataTable
		{
			public MeasurementTableModel(MeasurementTable enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(MeasurementTable enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
				measurementHeaders = new System.String[]{GT._("Value"), "a", "b", "c", "d"};
			}
			private MeasurementTable enclosingInstance;
			public MeasurementTable Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			//UPGRADE_NOTE: Final was removed from the declaration of 'measurementHeaders '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
			//UPGRADE_NOTE: The initialization of  'measurementHeaders' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
			internal System.String[] measurementHeaders;
			
			//UPGRADE_NOTE: The equivalent of method 'javax.swing.table.AbstractTableModel.getColumnName' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
			public System.String getColumnName(int col)
			{
				return measurementHeaders[col];
			}
			//UPGRADE_NOTE: The equivalent of method 'javax.swing.table.AbstractTableModel.getRowCount' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
			public int getRowCount()
			{
				return Enclosing_Instance.viewer.MeasurementCount;
			}
			//UPGRADE_NOTE: The equivalent of method 'javax.swing.table.AbstractTableModel.getColumnCount' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
			public int getColumnCount()
			{
				return 5;
			}
			
			//UPGRADE_NOTE: The equivalent of method 'javax.swing.table.AbstractTableModel.getColumnClass' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
			public System.Type getColumnClass(int col)
			{
				return Enclosing_Instance.stringClass;
			}
			//UPGRADE_NOTE: The equivalent of method 'javax.swing.table.AbstractTableModel.getValueAt' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
			public System.Object getValueAt(int row, int col)
			{
				if (col == 0)
					return Enclosing_Instance.viewer.getMeasurementStringValue(row);
				int[] countPlusIndices = Enclosing_Instance.viewer.getMeasurementCountPlusIndices(row);
				if (col >= countPlusIndices.Length)
					return null;
				int atomIndex = countPlusIndices[col];
				return ("" + Enclosing_Instance.viewer.getAtomNumber(atomIndex) + " " + Enclosing_Instance.viewer.getAtomName(atomIndex));
			}
			
			//UPGRADE_NOTE: The equivalent of method 'javax.swing.table.AbstractTableModel.isCellEditable' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
			public bool isCellEditable(int row, int col)
			{
				return false;
			}
		}
		
		public virtual void  updateTables()
		{
			updateMeasurementTableData();
		}
	}
}