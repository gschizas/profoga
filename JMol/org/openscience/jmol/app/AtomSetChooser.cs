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
//UPGRADE_TODO: The type 'javax.vecmath.Point3f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Point3f = javax.vecmath.Point3f;
using GT = org.jmol.i18n.GT;
namespace org.openscience.jmol.app
{
	
	/// <summary> A JFrame that allows for choosing an Atomset to view.
	/// 
	/// </summary>
	/// <author>  Ren&eacute; Kanters, University of Richmond
	/// </author>
	[Serializable]
	public class AtomSetChooser:System.Windows.Forms.Form, IThreadRunnable
	{
		/// <summary> Sets the indexes to the atomSetIndex values of each leaf of the node.</summary>
		/// <param name="node">The node whose leaf's atomSetIndex values should be used
		/// </param>
		virtual protected internal System.Windows.Forms.TreeNode Indexes
		{
			set
			{
				//UPGRADE_ISSUE: Method 'javax.swing.tree.DefaultMutableTreeNode.getLeafCount' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtreeDefaultMutableTreeNodegetLeafCount'"
				int atomSetCount = value.getLeafCount();
				indexes = new int[atomSetCount];
				//UPGRADE_ISSUE: Method 'javax.swing.tree.DefaultMutableTreeNode.depthFirstEnumeration' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtreeDefaultMutableTreeNodedepthFirstEnumeration'"
				System.Collections.IEnumerator e = value.depthFirstEnumeration();
				int idx = 0;
				//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
				while (e.MoveNext())
				{
					//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
					value = (System.Windows.Forms.TreeNode) e.Current;
					if (value.GetNodeCount(false) == 0)
						indexes[idx++] = ((AtomSet) value).AtomSetIndex;
				}
				// now update the selectSlider (may trigger a valueChanged event...)
				selectSlider.Enabled = atomSetCount > 0;
				selectSlider.Maximum = atomSetCount - 1;
			}
			
		}
		
		private SupportClass.ThreadClass animThread = null;
		
		private System.Windows.Forms.TextBox propertiesTextArea;
		//UPGRADE_TODO: Class 'javax.swing.JTree' was converted to 'System.Windows.Forms.TreeView' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
		private System.Windows.Forms.TreeView tree;
		//UPGRADE_TODO: Class 'javax.swing.tree.DefaultTreeModel' was converted to 'System.Windows.Forms.TreeNode' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
		private System.Windows.Forms.TreeNode treeModel;
		private JmolViewer viewer;
		private System.Windows.Forms.CheckBox repeatCheckBox;
		private System.Windows.Forms.TrackBar selectSlider;
		private System.Windows.Forms.Label infoLabel;
		private System.Windows.Forms.TrackBar fpsSlider;
		private System.Windows.Forms.TrackBar amplitudeSlider;
		private System.Windows.Forms.TrackBar periodSlider;
		private System.Windows.Forms.TrackBar scaleSlider;
		private System.Windows.Forms.TrackBar radiusSlider;
		
		private System.Windows.Forms.FileDialog saveChooser;
		
		
		// Strings for the commands of the buttons and the determination
		// of the tooltips and images associated with them
		internal const System.String REWIND = "rewind";
		internal const System.String PREVIOUS = "prev";
		internal const System.String PLAY = "play";
		internal const System.String PAUSE = "pause";
		internal const System.String NEXT = "next";
		internal const System.String FF = "ff";
		internal const System.String SAVE = "save";
		
		/// <summary> String for prefix/resource identifier for the collection area.
		/// This value is used in the Jmol properties files.
		/// </summary>
		internal const System.String COLLECTION = "collection";
		/// <summary> String for prefix/resource identifier for the vector area.
		/// This value is used in the Jmol properties files.
		/// </summary>
		internal const System.String VECTOR = "vector";
		
		
		/// <summary> Sequence of atom set indexes in current tree selection for a branch,
		/// or siblings for a leaf.
		/// </summary>
		private int[] indexes;
		private int currentIndex = - 1;
		
		/// <summary> Maximum value for the fps slider.</summary>
		private const int FPS_MAX = 30;
		/// <summary> Precision of the vibration scale slider</summary>
		private const float AMPLITUDE_PRECISION = 0.01f;
		/// <summary> Maximum value for vibration scale. Should be in preferences?</summary>
		private const float AMPLITUDE_MAX = 1;
		/// <summary> Initial value of vibration scale. Should be in preferences?</summary>
		private const float AMPLITUDE_VALUE = 0.5f;
		
		/// <summary> Precision of the vibration period slider in seconds.</summary>
		private const float PERIOD_PRECISION = 0.001f;
		/// <summary> Maximum value for the vibration period in seconds. Should be in preferences?</summary>
		private const float PERIOD_MAX = 1; // in seconds
		/// <summary> Initial value for the vibration period in seconds. Should be in preferences?</summary>
		private const float PERIOD_VALUE = 0.5f;
		
		/// <summary> Maximum value for vector radius.</summary>
		private const int RADIUS_MAX = 19;
		/// <summary> Initial value of vector radius. Should be in preferences?</summary>
		private const int RADIUS_VALUE = 3;
		
		/// <summary> Precision of the vector scale slider</summary>
		private const float SCALE_PRECISION = 0.01f;
		/// <summary> Maximum value for vector scale. Should be in preferences?</summary>
		private const float SCALE_MAX = 2.0f;
		/// <summary> Initial value of vector scale. Should be in preferences?</summary>
		private const float SCALE_VALUE = 1.0f;
		
		
		
		public AtomSetChooser(JmolViewer viewer, System.Windows.Forms.Form frame):base()
		{
			this.Text = GT._("AtomSetChooser");
			this.viewer = viewer;
			//UPGRADE_TODO: Constructor may need to be changed depending on function performed by the 'System.Windows.Forms.FileDialog' object. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1270'"
			saveChooser = new System.Windows.Forms.OpenFileDialog();
			
			// initialize the treeModel
			//UPGRADE_TODO: Constructor 'javax.swing.tree.DefaultTreeModel.DefaultTreeModel' was converted to 'System.Windows.Forms.TreeNode' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingtreeDefaultTreeModelDefaultTreeModel_javaxswingtreeTreeNode'"
			treeModel = new System.Windows.Forms.TreeNode(GT._("No AtomSets").ToString());
			
			//UPGRADE_TODO: Method 'javax.swing.JFrame.getContentPane' was converted to 'System.Windows.Forms.Form' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJFramegetContentPane'"
			layoutWindow(((System.Windows.Forms.ContainerControl) this));
			//UPGRADE_ISSUE: Method 'java.awt.Window.pack' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtWindowpack'"
			pack();
			setLocationRelativeTo(frame);
		}
		
		private void  layoutWindow(System.Windows.Forms.Control container)
		{
			
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtBorderLayout'"
			/*
			container.setLayout(new BorderLayout());*/
			
			//////////////////////////////////////////////////////////
			// The tree and properties panel
			// as a split pane in the center of the container
			//////////////////////////////////////////////////////////
			System.Windows.Forms.Panel treePanel = new System.Windows.Forms.Panel();
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtBorderLayout'"
			/*
			treePanel.setLayout(new BorderLayout());*/
			tree = SupportClass.TreeSupport.CreateTreeView(treeModel);
			//UPGRADE_ISSUE: Method 'javax.swing.JTree.setVisibleRowCount' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJTreesetVisibleRowCount_int'"
			tree.setVisibleRowCount(5);
			// only allow single selection (may want to change this later?)
			//UPGRADE_ISSUE: Method 'javax.swing.tree.TreeSelectionModel.setSelectionMode' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtreeTreeSelectionModel'"
			//UPGRADE_ISSUE: Method 'javax.swing.JTree.getSelectionModel' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJTreegetSelectionModel'"
			//UPGRADE_ISSUE: Field 'javax.swing.tree.TreeSelectionModel.SINGLE_TREE_SELECTION' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtreeTreeSelectionModel'"
			tree.getSelectionModel().setSelectionMode(TreeSelectionModel.SINGLE_TREE_SELECTION);
			tree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.valueChanged);
			tree.Enabled = false;
			//UPGRADE_TODO: Constructor 'javax.swing.JScrollPane.JScrollPane' was converted to 'System.Windows.Forms.ScrollableControl.ScrollableControl' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJScrollPaneJScrollPane_javaawtComponent'"
			System.Windows.Forms.ScrollableControl temp_scrollablecontrol2;
			temp_scrollablecontrol2 = new System.Windows.Forms.ScrollableControl();
			temp_scrollablecontrol2.AutoScroll = true;
			temp_scrollablecontrol2.Controls.Add(tree);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			System.Windows.Forms.Control temp_Control;
			temp_Control = temp_scrollablecontrol2;
			treePanel.Controls.Add(temp_Control);
			temp_Control.Dock = System.Windows.Forms.DockStyle.Fill;
			temp_Control.BringToFront();
			// the panel for the properties
			System.Windows.Forms.Panel propertiesPanel = new System.Windows.Forms.Panel();
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtBorderLayout'"
			/*
			propertiesPanel.setLayout(new BorderLayout());*/
			//UPGRADE_TODO: Method 'javax.swing.JComponent.setBorder' was converted to 'System.Windows.Forms.ControlPaint.DrawBorder3D' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJComponentsetBorder_javaxswingborderBorder'"
			//UPGRADE_ISSUE: Constructor 'javax.swing.border.TitledBorder.TitledBorder' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingborderTitledBorder'"
			System.Windows.Forms.ControlPaint.DrawBorder3D(propertiesPanel.CreateGraphics(), 0, 0, propertiesPanel.Width, propertiesPanel.Height, new TitledBorder(GT._("Properties")));
			System.Windows.Forms.TextBox temp_TextBox;
			temp_TextBox = new System.Windows.Forms.TextBox();
			temp_TextBox.Multiline = true;
			temp_TextBox.WordWrap = false;
			temp_TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			propertiesTextArea = temp_TextBox;
			propertiesTextArea.ReadOnly = !false;
			//UPGRADE_TODO: Constructor 'javax.swing.JScrollPane.JScrollPane' was converted to 'System.Windows.Forms.ScrollableControl.ScrollableControl' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJScrollPaneJScrollPane_javaawtComponent'"
			System.Windows.Forms.ScrollableControl temp_scrollablecontrol4;
			temp_scrollablecontrol4 = new System.Windows.Forms.ScrollableControl();
			temp_scrollablecontrol4.AutoScroll = true;
			temp_scrollablecontrol4.Controls.Add(propertiesTextArea);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			System.Windows.Forms.Control temp_Control2;
			temp_Control2 = temp_scrollablecontrol4;
			propertiesPanel.Controls.Add(temp_Control2);
			temp_Control2.Dock = System.Windows.Forms.DockStyle.Fill;
			temp_Control2.BringToFront();
			
			// create the split pane with the treePanel and propertiesPanel
			System.Windows.Forms.Panel astPanel = new System.Windows.Forms.Panel();
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtBorderLayout'"
			/*
			astPanel.setLayout(new BorderLayout());*/
			//UPGRADE_TODO: Method 'javax.swing.JComponent.setBorder' was converted to 'System.Windows.Forms.ControlPaint.DrawBorder3D' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJComponentsetBorder_javaxswingborderBorder'"
			//UPGRADE_ISSUE: Constructor 'javax.swing.border.TitledBorder.TitledBorder' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingborderTitledBorder'"
			System.Windows.Forms.ControlPaint.DrawBorder3D(astPanel.CreateGraphics(), 0, 0, astPanel.Width, astPanel.Height, new TitledBorder(GT._("Atom Set Collection")));
			
			//UPGRADE_TODO: Class 'javax.swing.JSplitPane' was converted to 'SupportClass.SplitterPanelSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
			SupportClass.SplitterPanelSupport splitPane = new SupportClass.SplitterPanelSupport((int) System.Windows.Forms.Orientation.Vertical, treePanel, propertiesPanel);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			astPanel.Controls.Add(splitPane);
			splitPane.Dock = System.Windows.Forms.DockStyle.Fill;
			splitPane.BringToFront();
			//UPGRADE_ISSUE: Method 'javax.swing.JSplitPane.setResizeWeight' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJSplitPanesetResizeWeight_double'"
			splitPane.setResizeWeight(1.0);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			container.Controls.Add(astPanel);
			astPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			astPanel.BringToFront();
			
			//////////////////////////////////////////////////////////
			// The Controller area is south of the container
			//////////////////////////////////////////////////////////
			System.Windows.Forms.Panel controllerPanel = new System.Windows.Forms.Panel();
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'javax.swing.BoxLayout.BoxLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBoxLayout'"
			//UPGRADE_ISSUE: Field 'javax.swing.BoxLayout.Y_AXIS' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBoxLayout'"
			controllerPanel.setLayout(new BoxLayout(controllerPanel, BoxLayout.Y_AXIS));
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			container.Controls.Add(controllerPanel);
			controllerPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			controllerPanel.SendToBack();
			
			//////////////////////////////////////////////////////////
			// The collection chooser/controller/feedback area
			//////////////////////////////////////////////////////////
			System.Windows.Forms.Panel collectionPanel = new System.Windows.Forms.Panel();
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'javax.swing.BoxLayout.BoxLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBoxLayout'"
			//UPGRADE_ISSUE: Field 'javax.swing.BoxLayout.Y_AXIS' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBoxLayout'"
			collectionPanel.setLayout(new BoxLayout(collectionPanel, BoxLayout.Y_AXIS));
			//UPGRADE_TODO: Method 'javax.swing.JComponent.setBorder' was converted to 'System.Windows.Forms.ControlPaint.DrawBorder3D' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJComponentsetBorder_javaxswingborderBorder'"
			//UPGRADE_ISSUE: Constructor 'javax.swing.border.TitledBorder.TitledBorder' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingborderTitledBorder'"
			System.Windows.Forms.ControlPaint.DrawBorder3D(collectionPanel.CreateGraphics(), 0, 0, collectionPanel.Width, collectionPanel.Height, new TitledBorder(GT._("Collection")));
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			controllerPanel.Controls.Add(collectionPanel);
			// info area
			System.Windows.Forms.Panel infoPanel = new System.Windows.Forms.Panel();
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtBorderLayout'"
			/*
			infoPanel.setLayout(new BorderLayout());*/
			//UPGRADE_TODO: Method 'javax.swing.JComponent.setBorder' was converted to 'System.Windows.Forms.ControlPaint.DrawBorder3D' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJComponentsetBorder_javaxswingborderBorder'"
			//UPGRADE_ISSUE: Constructor 'javax.swing.border.TitledBorder.TitledBorder' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingborderTitledBorder'"
			System.Windows.Forms.ControlPaint.DrawBorder3D(infoPanel.CreateGraphics(), 0, 0, infoPanel.Width, infoPanel.Height, new TitledBorder(GT._("Info")));
			System.Windows.Forms.Label temp_label;
			temp_label = new System.Windows.Forms.Label();
			temp_label.Text = " ";
			infoLabel = temp_label;
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			infoPanel.Controls.Add(infoLabel);
			infoLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
			infoLabel.SendToBack();
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			collectionPanel.Controls.Add(infoPanel);
			// select slider area
			System.Windows.Forms.Panel cpsPanel = new System.Windows.Forms.Panel();
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtBorderLayout'"
			/*
			cpsPanel.setLayout(new BorderLayout());*/
			//UPGRADE_TODO: Method 'javax.swing.JComponent.setBorder' was converted to 'System.Windows.Forms.ControlPaint.DrawBorder3D' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJComponentsetBorder_javaxswingborderBorder'"
			//UPGRADE_ISSUE: Constructor 'javax.swing.border.TitledBorder.TitledBorder' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingborderTitledBorder'"
			System.Windows.Forms.ControlPaint.DrawBorder3D(cpsPanel.CreateGraphics(), 0, 0, cpsPanel.Width, cpsPanel.Height, new TitledBorder(GT._("Select")));
			selectSlider = SupportClass.TrackBarSupport.CreateTrackBar(0, 0, 0);
			selectSlider.ValueChanged += new System.EventHandler(this.stateChanged);
			//UPGRADE_ISSUE: Method 'javax.swing.JSlider.setMajorTickSpacing' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJSlidersetMajorTickSpacing_int'"
			selectSlider.setMajorTickSpacing(5);
			selectSlider.TickFrequency = 1;
			selectSlider.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
			//UPGRADE_ISSUE: Method 'javax.swing.JSlider.setSnapToTicks' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJSlidersetSnapToTicks_boolean'"
			selectSlider.setSnapToTicks(true);
			selectSlider.Enabled = false;
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			cpsPanel.Controls.Add(selectSlider);
			selectSlider.Dock = System.Windows.Forms.DockStyle.Bottom;
			selectSlider.SendToBack();
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			collectionPanel.Controls.Add(cpsPanel);
			// panel with controller and fps
			System.Windows.Forms.Panel row = new System.Windows.Forms.Panel();
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			collectionPanel.Controls.Add(row);
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'javax.swing.BoxLayout.BoxLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBoxLayout'"
			//UPGRADE_ISSUE: Field 'javax.swing.BoxLayout.X_AXIS' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBoxLayout'"
			row.setLayout(new BoxLayout(row, BoxLayout.X_AXIS));
			// repeat check box to be added to the controller
			repeatCheckBox = SupportClass.CheckBoxSupport.CreateCheckBox(GT._("Repeat"), false);
			System.Windows.Forms.Panel vcrpanel = createVCRController(COLLECTION);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			vcrpanel.Controls.Add(repeatCheckBox); // put the repeat text box in the vcr control
			// VCR-like play controller
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			row.Controls.Add(vcrpanel);
			// fps slider
			System.Windows.Forms.Panel fpsPanel = new System.Windows.Forms.Panel();
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			row.Controls.Add(fpsPanel);
			int fps = viewer.AnimationFps;
			if (fps > FPS_MAX)
				fps = FPS_MAX;
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtBorderLayout'"
			/*
			fpsPanel.setLayout(new BorderLayout());*/
			//UPGRADE_TODO: Method 'javax.swing.JComponent.setBorder' was converted to 'System.Windows.Forms.ControlPaint.DrawBorder3D' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJComponentsetBorder_javaxswingborderBorder'"
			//UPGRADE_ISSUE: Constructor 'javax.swing.border.TitledBorder.TitledBorder' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingborderTitledBorder'"
			System.Windows.Forms.ControlPaint.DrawBorder3D(fpsPanel.CreateGraphics(), 0, 0, fpsPanel.Width, fpsPanel.Height, new TitledBorder(GT._("FPS")));
			fpsSlider = SupportClass.TrackBarSupport.CreateTrackBar(0, FPS_MAX, fps);
			//UPGRADE_ISSUE: Method 'javax.swing.JSlider.setMajorTickSpacing' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJSlidersetMajorTickSpacing_int'"
			fpsSlider.setMajorTickSpacing(5);
			fpsSlider.TickFrequency = 1;
			fpsSlider.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
			//UPGRADE_ISSUE: Method 'javax.swing.JSlider.setSnapToTicks' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJSlidersetSnapToTicks_boolean'"
			fpsSlider.setSnapToTicks(true);
			fpsSlider.ValueChanged += new System.EventHandler(this.stateChanged);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			fpsPanel.Controls.Add(fpsSlider);
			fpsSlider.Dock = System.Windows.Forms.DockStyle.Bottom;
			fpsSlider.SendToBack();
			
			//////////////////////////////////////////////////////////
			// The vector panel
			//////////////////////////////////////////////////////////
			System.Windows.Forms.Panel vectorPanel = new System.Windows.Forms.Panel();
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			controllerPanel.Controls.Add(vectorPanel);
			// fill out the contents of the vectorPanel
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'javax.swing.BoxLayout.BoxLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBoxLayout'"
			//UPGRADE_ISSUE: Field 'javax.swing.BoxLayout.Y_AXIS' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBoxLayout'"
			vectorPanel.setLayout(new BoxLayout(vectorPanel, BoxLayout.Y_AXIS));
			//UPGRADE_TODO: Method 'javax.swing.JComponent.setBorder' was converted to 'System.Windows.Forms.ControlPaint.DrawBorder3D' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJComponentsetBorder_javaxswingborderBorder'"
			//UPGRADE_ISSUE: Constructor 'javax.swing.border.TitledBorder.TitledBorder' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingborderTitledBorder'"
			System.Windows.Forms.ControlPaint.DrawBorder3D(vectorPanel.CreateGraphics(), 0, 0, vectorPanel.Width, vectorPanel.Height, new TitledBorder(GT._("Vector")));
			// the first row in the vectoPanel: radius and scale of the vector
			System.Windows.Forms.Panel row1 = new System.Windows.Forms.Panel();
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'javax.swing.BoxLayout.BoxLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBoxLayout'"
			//UPGRADE_ISSUE: Field 'javax.swing.BoxLayout.X_AXIS' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBoxLayout'"
			row1.setLayout(new BoxLayout(row1, BoxLayout.X_AXIS));
			// controller for the vector representation
			System.Windows.Forms.Panel radiusPanel = new System.Windows.Forms.Panel();
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtBorderLayout'"
			/*
			radiusPanel.setLayout(new BorderLayout());*/
			//UPGRADE_TODO: Method 'javax.swing.JComponent.setBorder' was converted to 'System.Windows.Forms.ControlPaint.DrawBorder3D' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJComponentsetBorder_javaxswingborderBorder'"
			//UPGRADE_ISSUE: Constructor 'javax.swing.border.TitledBorder.TitledBorder' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingborderTitledBorder'"
			System.Windows.Forms.ControlPaint.DrawBorder3D(radiusPanel.CreateGraphics(), 0, 0, radiusPanel.Width, radiusPanel.Height, new TitledBorder(GT._("Radius")));
			radiusSlider = SupportClass.TrackBarSupport.CreateTrackBar(0, RADIUS_MAX, RADIUS_VALUE);
			//UPGRADE_ISSUE: Method 'javax.swing.JSlider.setMajorTickSpacing' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJSlidersetMajorTickSpacing_int'"
			radiusSlider.setMajorTickSpacing(5);
			radiusSlider.TickFrequency = 1;
			radiusSlider.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
			//UPGRADE_ISSUE: Method 'javax.swing.JSlider.setSnapToTicks' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJSlidersetSnapToTicks_boolean'"
			radiusSlider.setSnapToTicks(true);
			radiusSlider.ValueChanged += new System.EventHandler(this.stateChanged);
			viewer.evalStringQuiet("vector " + RADIUS_VALUE);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			radiusPanel.Controls.Add(radiusSlider);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			row1.Controls.Add(radiusPanel);
			// controller for the vector scale
			System.Windows.Forms.Panel scalePanel = new System.Windows.Forms.Panel();
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtBorderLayout'"
			/*
			scalePanel.setLayout(new BorderLayout());*/
			//UPGRADE_TODO: Method 'javax.swing.JComponent.setBorder' was converted to 'System.Windows.Forms.ControlPaint.DrawBorder3D' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJComponentsetBorder_javaxswingborderBorder'"
			//UPGRADE_ISSUE: Constructor 'javax.swing.border.TitledBorder.TitledBorder' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingborderTitledBorder'"
			System.Windows.Forms.ControlPaint.DrawBorder3D(scalePanel.CreateGraphics(), 0, 0, scalePanel.Width, scalePanel.Height, new TitledBorder(GT._("Scale")));
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			scaleSlider = SupportClass.TrackBarSupport.CreateTrackBar(0, (int) (SCALE_MAX / SCALE_PRECISION), (int) (SCALE_VALUE / SCALE_PRECISION));
			scaleSlider.ValueChanged += new System.EventHandler(this.stateChanged);
			viewer.setVectorScale(SCALE_VALUE);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			scalePanel.Controls.Add(scaleSlider);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			row1.Controls.Add(scalePanel);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			vectorPanel.Controls.Add(row1);
			// the second row: amplitude and period of the vibration animation
			System.Windows.Forms.Panel row2 = new System.Windows.Forms.Panel();
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'javax.swing.BoxLayout.BoxLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBoxLayout'"
			//UPGRADE_ISSUE: Field 'javax.swing.BoxLayout.X_AXIS' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBoxLayout'"
			row2.setLayout(new BoxLayout(row2, BoxLayout.X_AXIS));
			// controller for vibrationScale = amplitude
			System.Windows.Forms.Panel amplitudePanel = new System.Windows.Forms.Panel();
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtBorderLayout'"
			/*
			amplitudePanel.setLayout(new BorderLayout());*/
			//UPGRADE_TODO: Method 'javax.swing.JComponent.setBorder' was converted to 'System.Windows.Forms.ControlPaint.DrawBorder3D' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJComponentsetBorder_javaxswingborderBorder'"
			//UPGRADE_ISSUE: Constructor 'javax.swing.border.TitledBorder.TitledBorder' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingborderTitledBorder'"
			System.Windows.Forms.ControlPaint.DrawBorder3D(amplitudePanel.CreateGraphics(), 0, 0, amplitudePanel.Width, amplitudePanel.Height, new TitledBorder(GT._("Amplitude")));
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			amplitudeSlider = SupportClass.TrackBarSupport.CreateTrackBar(0, (int) (AMPLITUDE_MAX / AMPLITUDE_PRECISION), (int) (AMPLITUDE_VALUE / AMPLITUDE_PRECISION));
			viewer.setVibrationScale(AMPLITUDE_VALUE);
			amplitudeSlider.ValueChanged += new System.EventHandler(this.stateChanged);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			amplitudePanel.Controls.Add(amplitudeSlider);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			row2.Controls.Add(amplitudePanel);
			// controller for the vibrationPeriod
			System.Windows.Forms.Panel periodPanel = new System.Windows.Forms.Panel();
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtBorderLayout'"
			/*
			periodPanel.setLayout(new BorderLayout());*/
			//UPGRADE_TODO: Method 'javax.swing.JComponent.setBorder' was converted to 'System.Windows.Forms.ControlPaint.DrawBorder3D' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJComponentsetBorder_javaxswingborderBorder'"
			//UPGRADE_ISSUE: Constructor 'javax.swing.border.TitledBorder.TitledBorder' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingborderTitledBorder'"
			System.Windows.Forms.ControlPaint.DrawBorder3D(periodPanel.CreateGraphics(), 0, 0, periodPanel.Width, periodPanel.Height, new TitledBorder(GT._("Period")));
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			periodSlider = SupportClass.TrackBarSupport.CreateTrackBar(0, (int) (PERIOD_MAX / PERIOD_PRECISION), (int) (PERIOD_VALUE / PERIOD_PRECISION));
			viewer.VibrationPeriod = PERIOD_VALUE;
			periodSlider.ValueChanged += new System.EventHandler(this.stateChanged);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			periodPanel.Controls.Add(periodSlider);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			row2.Controls.Add(periodPanel);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			vectorPanel.Controls.Add(row2);
			// finally the controller at the bottom
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			System.Windows.Forms.Control temp_Control3;
			temp_Control3 = createVCRController(VECTOR);
			vectorPanel.Controls.Add(temp_Control3);
		}
		
		/// <summary> Creates a VCR type set of controller inside a JPanel.
		/// 
		/// <p>Uses the JmolResourceHandler to get the label for the panel,
		/// the images for the buttons, and the tooltips. The button names are 
		/// <code>rewind</code>, <code>prev</code>, <code>play</code>, <code>pause</code>,
		/// <code>next</code>, and <code>ff</code>.
		/// <p>The handler for the buttons should determine from the getActionCommand
		/// which button in which section triggered the actionEvent, which is identified
		/// by <code>{section}.{name}</code>.
		/// </summary>
		/// <param name="section">String of the section that the controller belongs to.
		/// </param>
		/// <returns> The JPanel
		/// </returns>
		private System.Windows.Forms.Panel createVCRController(System.String section)
		{
			System.Windows.Forms.Panel controlPanel = new System.Windows.Forms.Panel();
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'javax.swing.BoxLayout.BoxLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBoxLayout'"
			//UPGRADE_ISSUE: Field 'javax.swing.BoxLayout.X_AXIS' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBoxLayout'"
			controlPanel.setLayout(new BoxLayout(controlPanel, BoxLayout.X_AXIS));
			//UPGRADE_TODO: Method 'javax.swing.JComponent.setBorder' was converted to 'System.Windows.Forms.ControlPaint.DrawBorder3D' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJComponentsetBorder_javaxswingborderBorder'"
			//UPGRADE_ISSUE: Constructor 'javax.swing.border.TitledBorder.TitledBorder' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingborderTitledBorder'"
			System.Windows.Forms.ControlPaint.DrawBorder3D(controlPanel.CreateGraphics(), 0, 0, controlPanel.Width, controlPanel.Height, new TitledBorder(GT._("Controller")));
			System.Int32[] inset = new System.Int32[]{1, 1, 1, 1};
			// take out the save functionality until the XYZ file can properly be created
			//    String buttons[] = {REWIND,PREVIOUS,PLAY,PAUSE,NEXT,FF,SAVE};
			System.String[] buttons = new System.String[]{REWIND, PREVIOUS, PLAY, PAUSE, NEXT, FF};
			System.String insert = null;
			if (section.Equals(COLLECTION))
			{
				insert = GT._("atom set");
			}
			else if (section.Equals(VECTOR))
			{
				insert = GT._("vector");
			}
			System.String[] tooltips = new System.String[]{GT._("Go to first {0} in the collection", new System.Object[]{insert}), GT._("Go to previous {0} in the collection", new System.Object[]{insert}), GT._("Play the whole collection of {0}'s", new System.Object[]{insert}), GT._("Pause playing"), GT._("Go to next {0} in the collection", new System.Object[]{insert}), GT._("Jump to last {0} in the collection", new System.Object[]{insert})};
			for (int i = buttons.Length, idx = 0; --i >= 0; idx++)
			{
				System.String action = buttons[idx];
				// the icon and tool tip come from 
				System.Windows.Forms.Button btn = SupportClass.ButtonSupport.CreateStandardButton(JmolResourceHandler.getIconX("AtomSetChooser." + action + "Image"));
				SupportClass.ToolTipSupport.setToolTipText(btn, tooltips[idx]);
				//UPGRADE_ISSUE: Method 'javax.swing.AbstractButton.setMargin' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractButtonsetMargin_javaawtInsets'"
				btn.setMargin(inset);
				SupportClass.CommandManager.SetCommand(btn, section + "." + action);
				btn.Click += new System.EventHandler(this.actionPerformed);
				SupportClass.CommandManager.CheckCommand(btn);
				//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
				controlPanel.Controls.Add(btn);
			}
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createHorizontalGlue' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			System.Windows.Forms.Control temp_Control;
			temp_Control = Box.createHorizontalGlue();
			controlPanel.Controls.Add(temp_Control);
			return controlPanel;
		}
		
		public virtual void  valueChanged(System.Object event_sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			System.Windows.Forms.TreeNode node = (System.Windows.Forms.TreeNode) tree.SelectedNode;
			if (node == null)
			{
				return ;
			}
			try
			{
				int index = 0; // default for branch selection
				if (node.GetNodeCount(false) == 0)
				{
					System.Windows.Forms.TreeNode parent = (System.Windows.Forms.TreeNode) node.Parent;
					Indexes = parent; // the indexes are based what is in the parent
					index = parent.Nodes.IndexOf(node); // find out which index I had there
				}
				else
				{
					// selected branch
					Indexes = node;
				}
				showAtomSetIndex(index, true);
			}
			catch (System.Exception exception)
			{
				//     exception.printStackTrace();
			}
		}
		
		/// <summary> Show an atom set from the indexes array</summary>
		/// <param name="index">The index in the index array
		/// </param>
		/// <param name="bSetSelectSlider">If true, updates the selectSlider
		/// </param>
		protected internal virtual void  showAtomSetIndex(int index, bool bSetSelectSlider)
		{
			if (bSetSelectSlider)
			{
				selectSlider.Value = index; // slider calls back to really set the frame
				return ;
			}
			try
			{
				currentIndex = index;
				int atomSetIndex = indexes[index];
				//    viewer.setDisplayModelIndex(atomSetIndex);  // does not update
				viewer.evalStringQuiet("frame " + viewer.getModelNumber(atomSetIndex));
				infoLabel.Text = viewer.getModelName(atomSetIndex);
				showProperties(viewer.getModelProperties(atomSetIndex));
			}
			catch (System.Exception e)
			{
				// if this fails, ignore it.
			}
		}
		
		public virtual void  actionPerformed(System.Object event_sender, System.EventArgs e)
		{
			System.String cmd = SupportClass.CommandManager.GetCommand(event_sender);
			System.String[] parts = cmd.split("\\.");
			try
			{
				if (parts.Length == 2)
				{
					System.String section = parts[0];
					cmd = parts[1];
					if (COLLECTION.Equals(section))
					{
						if (REWIND.Equals(cmd))
						{
							animThread = null;
							showAtomSetIndex(0, true);
						}
						else if (PREVIOUS.Equals(cmd))
						{
							showAtomSetIndex(currentIndex - 1, true);
						}
						else if (PLAY.Equals(cmd))
						{
							if (animThread == null)
							{
								animThread = new SupportClass.ThreadClass(new System.Threading.ThreadStart(this.Run), "Animation");
								animThread.Start();
							}
						}
						else if (PAUSE.Equals(cmd))
						{
							animThread = null;
						}
						else if (NEXT.Equals(cmd))
						{
							showAtomSetIndex(currentIndex + 1, true);
						}
						else if (FF.Equals(cmd))
						{
							animThread = null;
							showAtomSetIndex(indexes.Length - 1, true);
						}
						else if (SAVE.Equals(cmd))
						{
							saveXYZCollection();
						}
					}
					else if (VECTOR.Equals(section))
					{
						if (REWIND.Equals(cmd))
						{
							findFrequency(0, 1);
						}
						else if (PREVIOUS.Equals(cmd))
						{
							findFrequency(currentIndex - 1, - 1);
						}
						else if (PLAY.Equals(cmd))
						{
							viewer.evalStringQuiet("vibration on");
						}
						else if (PAUSE.Equals(cmd))
						{
							viewer.evalStringQuiet("vibration off");
						}
						else if (NEXT.Equals(cmd))
						{
							findFrequency(currentIndex + 1, 1);
						}
						else if (FF.Equals(cmd))
						{
							findFrequency(indexes.Length - 1, - 1);
						}
						else if (SAVE.Equals(cmd))
						{
							System.Console.Out.WriteLine("Not implemented");
							// since I can not get to the vectors, I can't output this one (yet)
							// saveXYZVector();
						}
					}
				}
			}
			catch (System.Exception exception)
			{
				// exceptions during indexes array access: ignore it
			}
		}
		
		/// <summary> Saves the currently active collection as a multistep XYZ file. </summary>
		public virtual void  saveXYZCollection()
		{
			int nidx = indexes.Length;
			if (nidx == 0)
			{
				System.Console.Out.WriteLine("No collection selected.");
				return ;
			}
			
			//UPGRADE_TODO: The equivalent in .NET for method 'javax.swing.JFileChooser.showSaveDialog' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			int retval = (int) saveChooser.ShowDialog(this);
			if (retval == 0)
			{
				System.IO.FileInfo file = new System.IO.FileInfo(saveChooser.FileName);
				System.String fname = file.FullName;
				try
				{
					//UPGRADE_TODO: Constructor 'java.io.FileOutputStream.FileOutputStream' was converted to 'System.IO.FileStream.FileStream' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileOutputStreamFileOutputStream_javalangString'"
					System.IO.StreamWriter f = new System.IO.StreamWriter(new System.IO.FileStream(fname, System.IO.FileMode.Create), System.Text.Encoding.Default);
					for (int idx = 0; idx < nidx; idx++)
					{
						int modelIndex = indexes[idx];
						System.Text.StringBuilder str = new System.Text.StringBuilder(viewer.getModelName(modelIndex) + "\n");
						int natoms = 0;
						for (int i = 0; i < viewer.AtomCount; i++)
						{
							if (viewer.getAtomModelIndex(i) == modelIndex)
							{
								natoms++;
								Point3f p = viewer.getAtomPoint3f(i);
								// should really be getElementSymbol(i) in stead
								str.Append(viewer.getAtomName(i) + "\t");
								str.Append(p.x + "\t" + p.y + "\t" + p.z + "\n");
								// not sure how to get the vibration vector and charge here...
							}
						}
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_int'"
						f.WriteLine(natoms);
						f.Write(str);
					}
					//UPGRADE_NOTE: Exceptions thrown by the equivalent in .NET of method 'java.io.PrintWriter.close' may be different. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1099'"
					f.Close();
				}
				catch (System.IO.FileNotFoundException e)
				{
					// e.printStackTrace();
				}
			}
		}
		
		/// <summary> Have the viewer show a particular frame with frequencies
		/// if it can be found.
		/// </summary>
		/// <param name="index">Starting index where to start looking for frequencies
		/// </param>
		/// <param name="increment">Increment value for how to go through the list
		/// </param>
		public virtual void  findFrequency(int index, int increment)
		{
			int maxIndex = indexes.Length;
			bool foundFrequency = false;
			
			// search till get to either end of found a frequency
			while (index >= 0 && index < maxIndex && !(foundFrequency = viewer.modelHasVibrationVectors(indexes[index])))
			{
				index += increment;
			}
			
			if (foundFrequency)
			{
				showAtomSetIndex(index, true);
			}
		}
		
		public virtual void  stateChanged(System.Object event_sender, System.EventArgs e)
		{
			System.Object src = event_sender;
			int value_Renamed = ((System.Windows.Forms.TrackBar) src).Value;
			if (src == selectSlider)
			{
				showAtomSetIndex(value_Renamed, false);
			}
			else if (src == fpsSlider)
			{
				if (value_Renamed == 0)
					fpsSlider.Value = 1;
				// make sure I never set it to 0...
				else
					viewer.AnimationFps = value_Renamed;
			}
			else if (src == radiusSlider)
			{
				if (value_Renamed == 0)
					radiusSlider.Value = 1;
				// make sure I never set it to 0..
				else
					viewer.evalStringQuiet("vector " + value_Renamed);
			}
			else if (src == scaleSlider)
			{
				viewer.evalStringQuiet("vector scale " + value_Renamed * SCALE_PRECISION);
			}
			else if (src == amplitudeSlider)
			{
				viewer.setVibrationScale(value_Renamed * AMPLITUDE_PRECISION);
			}
			else if (src == periodSlider)
			{
				viewer.VibrationPeriod = value_Renamed * PERIOD_PRECISION;
			}
		}
		
		/// <summary> Shows the properties in the propertiesPane of the
		/// AtomSetChooser window
		/// </summary>
		/// <param name="properties">Properties to be shown.
		/// </param>
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		protected internal virtual void  showProperties(System.Collections.Specialized.NameValueCollection properties)
		{
			bool needLF = false;
			//UPGRADE_TODO: Method 'javax.swing.text.JTextComponent.setText' was converted to 'System.Windows.Forms.TextBoxBase.Text' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingtextJTextComponentsetText_javalangString'"
			propertiesTextArea.Text = "";
			if (properties != null)
			{
				System.Collections.IEnumerator e = properties.Keys.GetEnumerator();
				//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
				while (e.MoveNext())
				{
					//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
					System.String propertyName = (System.String) e.Current;
					if (propertyName.StartsWith("."))
						continue; // skip the 'hidden' ones
					propertiesTextArea.AppendText((needLF?"\n ":" ") + propertyName + "=" + properties.Get(propertyName));
					needLF = true;
				}
			}
		}
		
		/// <summary> Creates the treeModel of the AtomSets available in the JmolViewer</summary>
		private void  createTreeModel()
		{
			System.String key = null;
			System.String separator = null;
			System.Windows.Forms.TreeNode root = new System.Windows.Forms.TreeNode(viewer.ModelSetName.ToString());
			
			// first determine whether we have a PATH_KEY in the modelSetProperties
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			System.Collections.Specialized.NameValueCollection modelSetProperties = viewer.ModelSetProperties;
			if (modelSetProperties != null)
			{
				key = modelSetProperties.Get("PATH_KEY");
				separator = modelSetProperties.Get("PATH_SEPARATOR");
			}
			if (key == null || separator == null)
			{
				// make a flat hierarchy if no key or separator are known
				for (int atomSetIndex = 0, count = viewer.ModelCount; atomSetIndex < count; ++atomSetIndex)
				{
					root.Nodes.Add(new AtomSet(this, atomSetIndex, viewer.getModelName(atomSetIndex)));
				}
			}
			else
			{
				for (int atomSetIndex = 0, count = viewer.ModelCount; atomSetIndex < count; ++atomSetIndex)
				{
					System.Windows.Forms.TreeNode current = root;
					System.String path = viewer.getModelProperty(atomSetIndex, key);
					// if the path is not null we need to find out where to add a leaf
					if (path != null)
					{
						System.Windows.Forms.TreeNode child = null;
						System.String[] folders = path.split(separator);
						for (int i = 0, nFolders = folders.Length; --nFolders >= 0; i++)
						{
							bool found = false; // folder is initially not found
							System.String lookForFolder = folders[i];
							for (int childIndex = current.Nodes.Count; --childIndex >= 0; )
							{
								child = (System.Windows.Forms.TreeNode) current.Nodes[childIndex];
								found = lookForFolder.Equals(child.ToString());
								if (found)
									break;
							}
							if (found)
							{
								current = child; // follow the found folder
							}
							else
							{
								// the 'folder' was not found: we need to add it
								System.Windows.Forms.TreeNode newFolder = new System.Windows.Forms.TreeNode(lookForFolder.ToString());
								current.Nodes.Add(newFolder);
								current = newFolder; // follow the new folder
							}
						}
					}
					// current is the folder where the AtomSet is to be added
					current.Nodes.Add(new AtomSet(this, atomSetIndex, viewer.getModelName(atomSetIndex)));
				}
			}
			//UPGRADE_TODO: Method 'javax.swing.tree.DefaultTreeModel.setRoot' was converted to 'System.Windows.Forms.TreeNode.TreeView.Nodes' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingtreeDefaultTreeModelsetRoot_javaxswingtreeTreeNode'"
			treeModel.TreeView.Nodes[0] = root;
			//UPGRADE_TODO: Method 'javax.swing.tree.DefaultTreeModel.reload' was converted to 'System.Windows.Forms.TreeNode.TreeView.Refresh()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingtreeDefaultTreeModelreload'"
			treeModel.TreeView.Refresh();
			
			// en/dis able the tree based on whether the root has children
			tree.Enabled = root.Nodes.Count > 0;
			// disable the slider and set it up so that we don't have anything selected..
			indexes = null;
			currentIndex = - 1;
			selectSlider.Enabled = false;
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AtomSet' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		/// <summary> Objects in the AtomSetChooser tree</summary>
		[Serializable]
		private class AtomSet:System.Windows.Forms.TreeNode
		{
			private void  InitBlock(AtomSetChooser enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private AtomSetChooser enclosingInstance;
			virtual public int AtomSetIndex
			{
				get
				{
					return atomSetIndex;
				}
				
			}
			public AtomSetChooser Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			/// <summary> The index of that AtomSet</summary>
			private int atomSetIndex;
			/// <summary> The name of the AtomSet</summary>
			private System.String atomSetName;
			
			public AtomSet(AtomSetChooser enclosingInstance, int atomSetIndex, System.String atomSetName)
			{
				InitBlock(enclosingInstance);
				this.atomSetIndex = atomSetIndex;
				this.atomSetName = atomSetName;
			}
			
			public override System.String ToString()
			{
				return atomSetName;
			}
		}
		
		////////////////////////////////////////////////////////////////
		// PropertyChangeListener to receive notification that
		// the underlying AtomSetCollection has changed
		////////////////////////////////////////////////////////////////
		
		public virtual void  propertyChange(System.Object event_sender, SupportClass.PropertyChangingEventArgs propertyChangeEvent)
		{
			System.String eventName = propertyChangeEvent.PropertyName;
			if (eventName.Equals(Jmol.chemFileProperty))
			{
				createTreeModel(); // all I need to do is to recreate the tree model
			}
		}
		
		/* (non-Javadoc)
		* @see java.lang.Runnable#run()
		*/
		public virtual void  Run()
		{
			SupportClass.ThreadClass myThread = SupportClass.ThreadClass.Current();
			//UPGRADE_TODO: The differences in the type  of parameters for method 'java.lang.Thread.setPriority'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
			myThread.Priority = (System.Threading.ThreadPriority) System.Threading.ThreadPriority.Lowest;
			while (animThread == myThread)
			{
				// since user can change the tree selection, I need to treat
				// all variables as volatile.
				if (currentIndex < 0)
				{
					animThread = null; // kill thread if I don't have a proper index
				}
				else
				{
					++currentIndex;
					if (currentIndex == indexes.Length)
					{
						if (repeatCheckBox.Checked)
							currentIndex = 0;
						// repeat at 0
						else
						{
							currentIndex--; // went 1 too far, step back
							animThread = null; // stop the animation thread
						}
					}
					showAtomSetIndex(currentIndex, true); // update the view
					try
					{
						// sleep for the amount of time required for the fps setting
						// NB the viewer's fps setting is never 0, so I could
						// set it directly, but just in case this behavior changes later...
						int fps = viewer.AnimationFps;
						//UPGRADE_TODO: Method 'java.lang.Thread.sleep' was converted to 'System.Threading.Thread.Sleep' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangThreadsleep_long'"
						//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
						System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64) 10000 * (int) (1000.0 / (fps == 0?1:fps))));
					}
					catch (System.Threading.ThreadInterruptedException e)
					{
						SupportClass.WriteStackTrace(e, Console.Error); // show what went wrong
					}
				}
			}
		}
	}
}