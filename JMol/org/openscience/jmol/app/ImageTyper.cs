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
	
	[Serializable]
	public class ImageTyper:System.Windows.Forms.Panel
	{
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassItemListener' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassItemListener
		{
			public AnonymousClassItemListener(ImageTyper enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(ImageTyper enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private ImageTyper enclosingInstance;
			public ImageTyper Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			public virtual void  itemStateChanged(System.Object event_sender, System.EventArgs e)
			{
				if (event_sender is System.Windows.Forms.MenuItem)
					((System.Windows.Forms.MenuItem) event_sender).Checked = !((System.Windows.Forms.MenuItem) event_sender).Checked;
				
				System.Windows.Forms.ComboBox source = (System.Windows.Forms.ComboBox) event_sender;
				Enclosing_Instance.result = ((System.String) source.SelectedItem);
				if (Enclosing_Instance.result.Equals("JPEG"))
				{
					Enclosing_Instance.qSlider.Enabled = true;
				}
				else
				{
					Enclosing_Instance.qSlider.Enabled = false;
				}
			}
		}
		private void  InitBlock()
		{
			result = Choices[def];
		}
		/// <returns> The file type which contains the user's choice
		/// </returns>
		virtual public System.String Type
		{
			get
			{
				return result;
			}
			
		}
		/// <returns> The quality (on a scale from 0 to 10) of the JPEG
		/// image that is to be generated.  Returns -1 if choice was not JPEG.
		/// </returns>
		virtual public int Quality
		{
			get
			{
				return qSlider.Value;
			}
			
		}
		
		private System.String[] Choices = new System.String[]{"JPEG", "PNG", "PPM"};
		private int def = 0;
		//UPGRADE_NOTE: The initialization of  'result' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		internal System.String result;
		internal System.Windows.Forms.TrackBar qSlider;
		private System.Windows.Forms.ComboBox cb;
		
		/// <summary> A simple panel with a combo box for allowing the user to choose
		/// the input file type.
		/// 
		/// </summary>
		/// <param name="fc">the file chooser
		/// </param>
		public ImageTyper(System.Windows.Forms.FileDialog fc)
		{
			InitBlock();
			
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtBorderLayout'"
			/*
			setLayout(new BorderLayout());*/
			
			System.Windows.Forms.Panel cbPanel = new System.Windows.Forms.Panel();
			//UPGRADE_TODO: Constructor 'java.awt.FlowLayout.FlowLayout' was converted to 'System.Object[]' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtFlowLayoutFlowLayout'"
			cbPanel.Tag = new System.Object[]{(int) System.Drawing.ContentAlignment.TopCenter, 5, 5};
			cbPanel.Layout += new System.Windows.Forms.LayoutEventHandler(SupportClass.FlowLayoutResize);
			//UPGRADE_TODO: Method 'javax.swing.JComponent.setBorder' was converted to 'System.Windows.Forms.ControlPaint.DrawBorder3D' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJComponentsetBorder_javaxswingborderBorder'"
			//UPGRADE_ISSUE: Constructor 'javax.swing.border.TitledBorder.TitledBorder' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingborderTitledBorder'"
			System.Windows.Forms.ControlPaint.DrawBorder3D(cbPanel.CreateGraphics(), 0, 0, cbPanel.Width, cbPanel.Height, new TitledBorder(GT._("Image Type")));
			cb = new System.Windows.Forms.ComboBox();
			for (int i = 0; i < Choices.Length; i++)
			{
				cb.Items.Add(Choices[i]);
			}
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			cbPanel.Controls.Add(cb);
			cb.SelectedIndex = def;
			cb.SelectedValueChanged += new System.EventHandler(new AnonymousClassItemListener(this).itemStateChanged);
			
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			Controls.Add(cbPanel);
			cbPanel.Dock = System.Windows.Forms.DockStyle.Top;
			cbPanel.SendToBack();
			
			System.Windows.Forms.Panel qPanel = new System.Windows.Forms.Panel();
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtBorderLayout'"
			/*
			qPanel.setLayout(new BorderLayout());*/
			//UPGRADE_TODO: Method 'javax.swing.JComponent.setBorder' was converted to 'System.Windows.Forms.ControlPaint.DrawBorder3D' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJComponentsetBorder_javaxswingborderBorder'"
			//UPGRADE_ISSUE: Constructor 'javax.swing.border.TitledBorder.TitledBorder' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingborderTitledBorder'"
			System.Windows.Forms.ControlPaint.DrawBorder3D(qPanel.CreateGraphics(), 0, 0, qPanel.Width, qPanel.Height, new TitledBorder(GT._("JPEG Quality")));
			//UPGRADE_TODO: The equivalent in .NET for field 'javax.swing.SwingConstants.HORIZONTAL' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			qSlider = SupportClass.TrackBarSupport.CreateTrackBar(System.Windows.Forms.Orientation.Horizontal, 50, 100, 90);
			//UPGRADE_ISSUE: Method 'javax.swing.JComponent.putClientProperty' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJComponentputClientProperty_javalangObject_javalangObject'"
			qSlider.putClientProperty("JSlider.isFilled", true);
			qSlider.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
			//UPGRADE_ISSUE: Method 'javax.swing.JSlider.setMajorTickSpacing' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJSlidersetMajorTickSpacing_int'"
			qSlider.setMajorTickSpacing(10);
			//UPGRADE_ISSUE: Method 'javax.swing.JSlider.setPaintLabels' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJSlidersetPaintLabels_boolean'"
			qSlider.setPaintLabels(true);
			
			qSlider.Enabled = true;
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			qPanel.Controls.Add(qSlider);
			qSlider.Dock = System.Windows.Forms.DockStyle.Bottom;
			qSlider.SendToBack();
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			Controls.Add(qPanel);
			qPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			qPanel.SendToBack();
		}
	}
}