/* $RCSfile$
* $Author: egonw $
* $Date: 2005-11-10 16:52:44 +0100 (jeu., 10 nov. 2005) $
* $Revision: 4255 $
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
namespace org.openscience.jmol.app
{
	
	[Serializable]
	public class FileTyper:System.Windows.Forms.Panel
	{
		private void  InitBlock()
		{
			choices = new System.String[]{JmolResourceHandler.getStringX("FileTyper.XYZ"), JmolResourceHandler.getStringX("FileTyper.PDB"), JmolResourceHandler.getStringX("FileTyper.CML")};
			fileType = choices[defaultTypeIndex];
		}
		/// <returns> The file type which contains the user's choice
		/// </returns>
		virtual public System.String Type
		{
			get
			{
				return fileType;
			}
			
		}
		private bool UseFileExtension
		{
			set
			{
				useFileExtension = value;
				fileTypeLabel.Enabled = !useFileExtension;
				fileTypeComboBox.Enabled = !useFileExtension;
			}
			
		}
		
		private System.Windows.Forms.CheckBox useFileExtensionCheckBox;
		private System.Windows.Forms.Label fileTypeLabel;
		private System.Windows.Forms.ComboBox fileTypeComboBox;
		private bool useFileExtension = true;
		
		//UPGRADE_NOTE: The initialization of  'choices' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private System.String[] choices;
		
		// Default is the first one:
		private int defaultTypeIndex = 0;
		//UPGRADE_NOTE: The initialization of  'fileType' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private System.String fileType;
		
		/// <summary> A simple panel with a combo box for allowing the user to choose
		/// the input file type.
		/// </summary>
		public FileTyper()
		{
			InitBlock();
			
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtBorderLayout'"
			/*
			setLayout(new BorderLayout());*/
			
			System.Windows.Forms.Panel fileTypePanel = new System.Windows.Forms.Panel();
			//UPGRADE_TODO: Method 'javax.swing.JComponent.setBorder' was converted to 'System.Windows.Forms.ControlPaint.DrawBorder3D' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJComponentsetBorder_javaxswingborderBorder'"
			//UPGRADE_ISSUE: Constructor 'javax.swing.border.EmptyBorder.EmptyBorder' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingborderEmptyBorderEmptyBorder_int_int_int_int'"
			System.Windows.Forms.ControlPaint.DrawBorder3D(fileTypePanel.CreateGraphics(), 0, 0, fileTypePanel.Width, fileTypePanel.Height, new EmptyBorder(5, 5, 5, 5));
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'java.awt.GridBagLayout.GridBagLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagLayout'"
			fileTypePanel.setLayout(new GridBagLayout());
			
			System.Windows.Forms.Label fillerLabel = new System.Windows.Forms.Label();
			//UPGRADE_ISSUE: Class 'java.awt.GridBagConstraints' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			//UPGRADE_ISSUE: Constructor 'java.awt.GridBagConstraints.GridBagConstraints' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			GridBagConstraints gridBagConstraints = new GridBagConstraints();
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridwidth' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.REMAINDER' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			gridBagConstraints.gridwidth = GridBagConstraints.REMAINDER;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.fill' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.BOTH' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			gridBagConstraints.fill = GridBagConstraints.BOTH;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.weightx' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			gridBagConstraints.weightx = 1.0;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.weighty' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			gridBagConstraints.weighty = 1.0;
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			fileTypePanel.Controls.Add(fillerLabel);
			fillerLabel.Dock = new System.Windows.Forms.DockStyle();
			fillerLabel.BringToFront();
			
			
			useFileExtensionCheckBox = SupportClass.CheckBoxSupport.CreateCheckBox(JmolResourceHandler.getStringX("FileTyper.useFileExtensionCheckBox"), useFileExtension);
			useFileExtensionCheckBox.CheckedChanged += new System.EventHandler(this.itemStateChanged);
			System.String mnemonic = JmolResourceHandler.getStringX("FileTyper.useFileExtensionMnemonic");
			if ((mnemonic != null) && (mnemonic.Length > 0))
			{
				System.Int32 tempInt;
				tempInt = useFileExtensionCheckBox.Text.ToLower().IndexOf(char.ToLower(mnemonic[0]));
				useFileExtensionCheckBox.Text = tempInt >= 0?useFileExtensionCheckBox.Text.Insert(tempInt, "&"):useFileExtensionCheckBox.Text;
			}
			//UPGRADE_ISSUE: Constructor 'java.awt.GridBagConstraints.GridBagConstraints' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			gridBagConstraints = new GridBagConstraints();
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridwidth' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.REMAINDER' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			gridBagConstraints.gridwidth = GridBagConstraints.REMAINDER;
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			fileTypePanel.Controls.Add(useFileExtensionCheckBox);
			useFileExtensionCheckBox.Dock = new System.Windows.Forms.DockStyle();
			useFileExtensionCheckBox.BringToFront();
			
			//UPGRADE_ISSUE: Constructor 'java.awt.GridBagConstraints.GridBagConstraints' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			gridBagConstraints = new GridBagConstraints();
			System.Windows.Forms.Label temp_label;
			temp_label = new System.Windows.Forms.Label();
			temp_label.Text = JmolResourceHandler.getStringX("FileTyper.fileTypeLabel");
			fileTypeLabel = temp_label;
			fileTypeLabel.ForeColor = System.Drawing.Color.Black;
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			fileTypePanel.Controls.Add(fileTypeLabel);
			fileTypeLabel.Dock = new System.Windows.Forms.DockStyle();
			fileTypeLabel.BringToFront();
			fileTypeComboBox = SupportClass.ComboBoxSupport.CreateComboBox(choices);
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridwidth' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.REMAINDER' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			gridBagConstraints.gridwidth = GridBagConstraints.REMAINDER;
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			fileTypePanel.Controls.Add(fileTypeComboBox);
			fileTypeComboBox.Dock = new System.Windows.Forms.DockStyle();
			fileTypeComboBox.BringToFront();
			fileTypeComboBox.SelectedIndex = defaultTypeIndex;
			fileTypeComboBox.SelectedValueChanged += new System.EventHandler(this.itemStateChanged);
			
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			Controls.Add(fileTypePanel);
			fileTypePanel.Dock = System.Windows.Forms.DockStyle.Fill;
			fileTypePanel.BringToFront();
			
			UseFileExtension = useFileExtension;
		}
		
		public virtual void  itemStateChanged(System.Object event_sender, System.EventArgs event_Renamed)
		{
			if (event_sender is System.Windows.Forms.MenuItem)
				((System.Windows.Forms.MenuItem) event_sender).Checked = !((System.Windows.Forms.MenuItem) event_sender).Checked;
			
			if (event_sender == useFileExtensionCheckBox)
			{
				//UPGRADE_ISSUE: Method 'java.awt.event.ItemEvent.getStateChange' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventItemEventgetStateChange'"
				//UPGRADE_ISSUE: Field 'java.awt.event.ItemEvent.DESELECTED' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventItemEventDESELECTED_f'"
				if (event_Renamed.getStateChange() == ItemEvent.DESELECTED)
				{
					UseFileExtension = false;
				}
				else
				{
					UseFileExtension = true;
				}
			}
			else if (event_sender == fileTypeComboBox)
			{
				fileType = ((System.String) fileTypeComboBox.SelectedItem);
			}
		}
		
		public virtual void  propertyChange(System.Object event_sender, SupportClass.PropertyChangingEventArgs event_Renamed)
		{
			
			System.String property = event_Renamed.PropertyName;
			if (useFileExtension)
			{
				if (property.Equals("SelectedFilesChangedProperty"))
				{
					System.IO.FileInfo file = (System.IO.FileInfo) event_Renamed.NewValue;
					System.String fileName = file.ToString().ToLower();
					if (fileName.EndsWith("xyz"))
					{
						fileTypeComboBox.SelectedIndex = 0;
					}
					else if (fileName.EndsWith("pdb"))
					{
						fileTypeComboBox.SelectedIndex = 1;
					}
					else if (fileName.EndsWith("cml"))
					{
						fileTypeComboBox.SelectedIndex = 2;
					}
					else
					{
						fileTypeComboBox.SelectedIndex = 0;
					}
				}
			}
		}
	}
}