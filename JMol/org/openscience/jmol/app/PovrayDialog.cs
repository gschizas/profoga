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
//UPGRADE_TODO: The type 'javax.swing.JFormattedTextField' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using JFormattedTextField = javax.swing.JFormattedTextField;
namespace org.openscience.jmol.app
{
	
	/// <summary> A dialog for controling the creation of a povray input file from a
	/// Chemframe and a display. The actual leg work of writing the file
	/// out is done by PovrayWriter.java.
	/// <p>Borrows code from org.openscience.jmol.Vibrate (Thanks!).
	/// </summary>
	/// <author>  Thomas James Grey (tjg1@ch.ic.ac.uk)
	/// </author>
	/// <author>  Matthew A. Meineke (mmeineke@nd.edu)
	/// </author>
	[Serializable]
	public class PovrayDialog:System.Windows.Forms.Form
	{
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassActionListener' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassActionListener
		{
			public AnonymousClassActionListener(PovrayDialog enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(PovrayDialog enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private PovrayDialog enclosingInstance;
			public PovrayDialog Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public virtual void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				Enclosing_Instance.updateScreen();
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassInputVerifier' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		//UPGRADE_ISSUE: Class 'javax.swing.InputVerifier' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingInputVerifier'"
		private class AnonymousClassInputVerifier:InputVerifier
		{
			public AnonymousClassInputVerifier(PovrayDialog enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(PovrayDialog enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private PovrayDialog enclosingInstance;
			public PovrayDialog Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public bool verify(System.Windows.Forms.Control component)
			{
				Enclosing_Instance.updateScreen();
				return true;
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassItemListener' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassItemListener
		{
			public AnonymousClassItemListener(PovrayDialog enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(PovrayDialog enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private PovrayDialog enclosingInstance;
			public PovrayDialog Enclosing_Instance
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
				Enclosing_Instance.updateScreen();
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassActionListener1' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassActionListener1
		{
			public AnonymousClassActionListener1(PovrayDialog enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(PovrayDialog enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private PovrayDialog enclosingInstance;
			public PovrayDialog Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			public virtual void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				Enclosing_Instance.showSavePathDialog();
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassItemListener1' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassItemListener1
		{
			public AnonymousClassItemListener1(PovrayDialog enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(PovrayDialog enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private PovrayDialog enclosingInstance;
			public PovrayDialog Enclosing_Instance
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
				Enclosing_Instance.imageSizeChanged();
				Enclosing_Instance.updateCommandLine();
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassPropertyChangeListener' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassPropertyChangeListener
		{
			public AnonymousClassPropertyChangeListener(PovrayDialog enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(PovrayDialog enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private PovrayDialog enclosingInstance;
			public PovrayDialog Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public virtual void  propertyChange(System.Object event_sender, SupportClass.PropertyChangingEventArgs e)
			{
				Enclosing_Instance.imageSizeChanged();
				Enclosing_Instance.updateCommandLine();
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassPropertyChangeListener1' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassPropertyChangeListener1
		{
			public AnonymousClassPropertyChangeListener1(PovrayDialog enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(PovrayDialog enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private PovrayDialog enclosingInstance;
			public PovrayDialog Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public virtual void  propertyChange(System.Object event_sender, SupportClass.PropertyChangingEventArgs e)
			{
				Enclosing_Instance.imageSizeChanged();
				Enclosing_Instance.updateCommandLine();
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassItemListener2' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassItemListener2
		{
			public AnonymousClassItemListener2(PovrayDialog enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(PovrayDialog enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private PovrayDialog enclosingInstance;
			public PovrayDialog Enclosing_Instance
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
				Enclosing_Instance.imageSizeChanged();
				Enclosing_Instance.updateCommandLine();
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassActionListener2' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassActionListener2
		{
			public AnonymousClassActionListener2(PovrayDialog enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(PovrayDialog enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private PovrayDialog enclosingInstance;
			public PovrayDialog Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public virtual void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				Enclosing_Instance.imageSizeChanged();
				Enclosing_Instance.updateCommandLine();
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassItemListener3' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassItemListener3
		{
			public AnonymousClassItemListener3(PovrayDialog enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(PovrayDialog enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private PovrayDialog enclosingInstance;
			public PovrayDialog Enclosing_Instance
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
				Enclosing_Instance.outputFormatChanged();
				Enclosing_Instance.updateCommandLine();
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassActionListener3' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassActionListener3
		{
			public AnonymousClassActionListener3(PovrayDialog enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(PovrayDialog enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private PovrayDialog enclosingInstance;
			public PovrayDialog Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public virtual void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				Enclosing_Instance.outputFormatChanged();
				Enclosing_Instance.updateCommandLine();
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassItemListener4' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassItemListener4
		{
			public AnonymousClassItemListener4(PovrayDialog enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(PovrayDialog enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private PovrayDialog enclosingInstance;
			public PovrayDialog Enclosing_Instance
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
				Enclosing_Instance.updateCommandLine();
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassItemListener5' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassItemListener5
		{
			public AnonymousClassItemListener5(PovrayDialog enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(PovrayDialog enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private PovrayDialog enclosingInstance;
			public PovrayDialog Enclosing_Instance
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
				Enclosing_Instance.mosaicPreviewChanged();
				Enclosing_Instance.updateCommandLine();
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassActionListener4' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassActionListener4
		{
			public AnonymousClassActionListener4(PovrayDialog enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(PovrayDialog enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private PovrayDialog enclosingInstance;
			public PovrayDialog Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public virtual void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				Enclosing_Instance.mosaicPreviewChanged();
				Enclosing_Instance.updateCommandLine();
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassActionListener5' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassActionListener5
		{
			public AnonymousClassActionListener5(PovrayDialog enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(PovrayDialog enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private PovrayDialog enclosingInstance;
			public PovrayDialog Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public virtual void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				Enclosing_Instance.mosaicPreviewChanged();
				Enclosing_Instance.updateCommandLine();
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassActionListener6' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassActionListener6
		{
			public AnonymousClassActionListener6(PovrayDialog enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(PovrayDialog enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private PovrayDialog enclosingInstance;
			public PovrayDialog Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			public virtual void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				Enclosing_Instance.showPovrayPathDialog();
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassActionListener7' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassActionListener7
		{
			public AnonymousClassActionListener7(PovrayDialog enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(PovrayDialog enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private PovrayDialog enclosingInstance;
			public PovrayDialog Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			public virtual void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				Enclosing_Instance.goPressed();
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassActionListener8' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassActionListener8
		{
			public AnonymousClassActionListener8(PovrayDialog enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(PovrayDialog enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private PovrayDialog enclosingInstance;
			public PovrayDialog Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			public virtual void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				Enclosing_Instance.cancelPressed();
			}
		}
		/// <returns> Command line split into arguments
		/// </returns>
		private System.String[] CommandLineArgs
		{
			get
			{
				
				//Parsing command line
				System.String commandLine = commandLineField.Text;
				System.Collections.ArrayList vector = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
				int begin = 0;
				int end = 0;
				int doubleQuoteCount = 0;
				while (end < commandLine.Length)
				{
					if (commandLine[end] == '\"')
					{
						doubleQuoteCount++;
					}
					if ((System.Char.GetUnicodeCategory(commandLine[end]) == System.Globalization.UnicodeCategory.SpaceSeparator))
					{
						while ((begin < end) && ((System.Char.GetUnicodeCategory(commandLine[begin]) == System.Globalization.UnicodeCategory.SpaceSeparator)))
						{
							begin++;
						}
						if (end > begin + 1)
						{
							if (doubleQuoteCount % 2 == 0)
							{
								vector.Add(commandLine.Substring(begin, (end) - (begin)));
								begin = end;
							}
						}
					}
					end++;
				}
				while ((begin < end) && ((System.Char.GetUnicodeCategory(commandLine[begin]) == System.Globalization.UnicodeCategory.SpaceSeparator)))
				{
					begin++;
				}
				if (end > begin + 1)
				{
					vector.Add(commandLine.Substring(begin, (end) - (begin)));
				}
				
				//Construct result
				System.String[] args = new System.String[vector.Count];
				for (int pos = 0; pos < vector.Count; pos++)
				{
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					args[pos] = vector[pos].ToString();
					if ((args[pos][0] == '\"') && (args[pos][args[pos].Length - 1] == '\"'))
					{
						args[pos] = args[pos].Substring(1, (args[pos].Length - 1) - (1));
					}
				}
				return args;
			}
			
		}
		
		private JmolViewer viewer;
		
		protected internal System.Windows.Forms.Button povrayPathButton;
		protected internal System.Windows.Forms.TextBox commandLineField;
		protected internal System.Windows.Forms.Button goButton;
		protected internal System.Windows.Forms.TextBox saveField;
		protected internal System.Windows.Forms.TextBox savePathLabel;
		private int outputWidth = - 1;
		private int outputHeight = - 1;
		protected internal System.Windows.Forms.TextBox povrayPathLabel;
		
		protected internal System.Windows.Forms.CheckBox runPovCheck;
		protected internal System.Windows.Forms.CheckBox useIniCheck;
		protected internal System.Windows.Forms.CheckBox allFramesCheck;
		protected internal System.Windows.Forms.CheckBox antiAliasCheck;
		protected internal System.Windows.Forms.CheckBox displayWhileRenderingCheck;
		
		private System.Windows.Forms.CheckBox imageSizeCheck;
		private System.Windows.Forms.Label imageSizeWidth;
		private JFormattedTextField imageSizeTextWidth;
		private System.Windows.Forms.Label imageSizeHeight;
		private JFormattedTextField imageSizeTextHeight;
		private System.Windows.Forms.CheckBox imageSizeRatioBox;
		private System.Windows.Forms.ComboBox imageSizeRatioCombo;
		
		private System.Windows.Forms.CheckBox outputFormatCheck;
		private System.Windows.Forms.ComboBox outputFormatCombo;
		
		private System.Windows.Forms.CheckBox outputAlphaCheck;
		
		private System.Windows.Forms.CheckBox mosaicPreviewCheck;
		private System.Windows.Forms.Label mosaicPreviewStart;
		private System.Windows.Forms.ComboBox mosaicPreviewComboStart;
		private System.Windows.Forms.Label mosaicPreviewEnd;
		private System.Windows.Forms.ComboBox mosaicPreviewComboEnd;
		
		// Event management
		//UPGRADE_TODO: Interface 'java.awt.event.ActionListener' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
		private ActionListener updateActionListener = null;
		//UPGRADE_ISSUE: Class 'javax.swing.InputVerifier' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingInputVerifier'"
		private InputVerifier updateInputVerifier = null;
		//UPGRADE_TODO: Interface 'java.awt.event.ItemListener' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
		private ItemListener updateItemListener = null;
		
		
		/// <summary> Creates a dialog for getting info related to output frames in
		/// povray format.
		/// </summary>
		/// <param name="f">The frame assosiated with the dialog
		/// </param>
		/// <param name="viewer">The interacting display we are reproducing (source of view angle info etc)
		/// </param>
		public PovrayDialog(System.Windows.Forms.Form f, JmolViewer viewer):base()
		{
			//UPGRADE_TODO: Constructor 'javax.swing.JDialog.JDialog' was converted to 'SupportClass.DialogSupport.SetDialog' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJDialogJDialog_javaawtFrame_javalangString_boolean'"
			SupportClass.DialogSupport.SetDialog(this, f, GT._("Render in POV-Ray"));
			this.viewer = viewer;
			
			//
			System.String text = null;
			
			//Take the height and width settings from the JFrame
			int screenWidth = viewer.ScreenWidth;
			int screenHeight = viewer.ScreenHeight;
			setImageDimensions(screenWidth, screenHeight);
			
			// Event management
			updateActionListener = new AnonymousClassActionListener(this);
			updateInputVerifier = new AnonymousClassInputVerifier(this);
			updateItemListener = new AnonymousClassItemListener(this);
			
			//Box: Window
			//UPGRADE_ISSUE: Class 'javax.swing.Box' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createVerticalBox' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			Box windowBox = Box.createVerticalBox();
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			((System.Windows.Forms.ContainerControl) this).Controls.Add(windowBox);
			
			//Box: Main
			//UPGRADE_ISSUE: Class 'javax.swing.Box' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createVerticalBox' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			Box mainBox = Box.createVerticalBox();
			
			//GUI for save name selection
			//UPGRADE_ISSUE: Class 'javax.swing.Box' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createVerticalBox' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			Box justSavingBox = Box.createVerticalBox();
			text = GT._("Jmol-to-POV-Ray Conversion");
			//UPGRADE_ISSUE: Constructor 'javax.swing.border.TitledBorder.TitledBorder' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingborderTitledBorder'"
			justSavingBox.setBorder(new TitledBorder(text));
			
			//UPGRADE_ISSUE: Class 'javax.swing.Box' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createHorizontalBox' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			Box saveBox = Box.createHorizontalBox();
			text = GT._("Filename Stem");
			//UPGRADE_ISSUE: Constructor 'javax.swing.border.TitledBorder.TitledBorder' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingborderTitledBorder'"
			saveBox.setBorder(new TitledBorder(text));
			text = GT._("Single frame: eg 'caffine' -> 'caffine.pov'; Multiple frame: eg 'caffine' -> 'caffine_1.pov', 'caffine_2.pov'");
			saveBox.setToolTipText(text);
			System.Windows.Forms.TextBox temp_text_box;
			//UPGRADE_TODO: Constructor 'javax.swing.JTextField.JTextField' was converted to 'System.Windows.Forms.TextBox' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJTextFieldJTextField_javalangString_int'"
			temp_text_box = new System.Windows.Forms.TextBox();
			temp_text_box.Text = "jmol";
			saveField = temp_text_box;
			//UPGRADE_TODO: Method 'javax.swing.JTextField.addActionListener' was converted to 'System.Windows.Forms.TextBox.KeyPress' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJTextFieldaddActionListener_javaawteventActionListener'"
			saveField.KeyPress += new System.Windows.Forms.KeyPressEventHandler(updateActionListener.actionPerformed);
			//UPGRADE_ISSUE: Method 'javax.swing.JComponent.setInputVerifier' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJComponentsetInputVerifier_javaxswingInputVerifier'"
			saveField.setInputVerifier(updateInputVerifier);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			saveBox.Controls.Add(saveField);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			justSavingBox.Controls.Add(saveBox);
			
			//GUI for save path selection
			//UPGRADE_ISSUE: Class 'javax.swing.Box' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createHorizontalBox' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			Box savePathBox = Box.createHorizontalBox();
			text = GT._("Working Directory");
			//UPGRADE_ISSUE: Constructor 'javax.swing.border.TitledBorder.TitledBorder' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingborderTitledBorder'"
			savePathBox.setBorder(new TitledBorder(text));
			text = GT._("Where the .pov files will be saved");
			savePathBox.setToolTipText(text);
			System.Windows.Forms.TextBox temp_text_box2;
			temp_text_box2 = new System.Windows.Forms.TextBox();
			temp_text_box2.Text = "";
			savePathLabel = temp_text_box2;
			savePathLabel.ReadOnly = !false;
			//UPGRADE_TODO: Method 'javax.swing.JComponent.setBorder' was converted to 'System.Windows.Forms.ControlPaint.DrawBorder3D' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJComponentsetBorder_javaxswingborderBorder'"
			System.Windows.Forms.ControlPaint.DrawBorder3D(savePathLabel.CreateGraphics(), 0, 0, savePathLabel.Width, savePathLabel.Height, System.Windows.Forms.Border3DStyle.Flat);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			savePathBox.Controls.Add(savePathLabel);
			text = GT._("Select");
			System.Windows.Forms.Button savePathButton = SupportClass.ButtonSupport.CreateStandardButton(text);
			savePathButton.Click += new System.EventHandler(new AnonymousClassActionListener1(this).actionPerformed);
			SupportClass.CommandManager.CheckCommand(savePathButton);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			savePathBox.Controls.Add(savePathButton);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			justSavingBox.Controls.Add(savePathBox);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			mainBox.Controls.Add(justSavingBox);
			
			//GUI for povray options
			//UPGRADE_ISSUE: Class 'javax.swing.Box' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createVerticalBox' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			Box povOptionsBox = Box.createVerticalBox();
			text = GT._("POV-Ray Runtime Options");
			//UPGRADE_ISSUE: Constructor 'javax.swing.border.TitledBorder.TitledBorder' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingborderTitledBorder'"
			povOptionsBox.setBorder(new TitledBorder(text));
			
			// Run povray option
			//UPGRADE_ISSUE: Class 'javax.swing.Box' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createHorizontalBox' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			Box runPovBox = Box.createHorizontalBox();
			text = GT._("Run POV-Ray directly");
			runPovCheck = SupportClass.CheckBoxSupport.CreateCheckBox(text, true);
			text = GT._("Launch povray from within Jmol");
			SupportClass.ToolTipSupport.setToolTipText(runPovCheck, text);
			runPovCheck.CheckedChanged += new System.EventHandler(updateItemListener.itemStateChanged);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			runPovBox.Controls.Add(runPovCheck);
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createGlue' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			System.Windows.Forms.Control temp_Control;
			temp_Control = Box.createGlue();
			runPovBox.Controls.Add(temp_Control);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			povOptionsBox.Controls.Add(runPovBox);
			
			// Use Ini option
			//UPGRADE_ISSUE: Class 'javax.swing.Box' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createHorizontalBox' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			Box useIniBox = Box.createHorizontalBox();
			text = GT._("Use .ini file");
			useIniCheck = SupportClass.CheckBoxSupport.CreateCheckBox(text, true);
			text = GT._("Save options in a .ini file");
			SupportClass.ToolTipSupport.setToolTipText(useIniCheck, text);
			useIniCheck.CheckedChanged += new System.EventHandler(updateItemListener.itemStateChanged);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			useIniBox.Controls.Add(useIniCheck);
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createGlue' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			System.Windows.Forms.Control temp_Control2;
			temp_Control2 = Box.createGlue();
			useIniBox.Controls.Add(temp_Control2);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			povOptionsBox.Controls.Add(useIniBox);
			
			// Render all frames options
			//UPGRADE_ISSUE: Class 'javax.swing.Box' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createHorizontalBox' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			Box allFramesBox = Box.createHorizontalBox();
			text = GT._("Render all frames");
			allFramesCheck = SupportClass.CheckBoxSupport.CreateCheckBox(text, false);
			text = GT._("Render each model (not only the currently displayed one)");
			SupportClass.ToolTipSupport.setToolTipText(allFramesCheck, text);
			allFramesCheck.CheckedChanged += new System.EventHandler(updateItemListener.itemStateChanged);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			allFramesBox.Controls.Add(allFramesCheck);
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createGlue' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			System.Windows.Forms.Control temp_Control3;
			temp_Control3 = Box.createGlue();
			allFramesBox.Controls.Add(temp_Control3);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			povOptionsBox.Controls.Add(allFramesBox);
			
			// Antialias option
			//UPGRADE_ISSUE: Class 'javax.swing.Box' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createHorizontalBox' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			Box antiAliasBox = Box.createHorizontalBox();
			text = GT._("Turn on POV-Ray anti-aliasing");
			antiAliasCheck = SupportClass.CheckBoxSupport.CreateCheckBox(text, true);
			text = GT._("Use povray's slower but higher quality anti-aliasing mode");
			SupportClass.ToolTipSupport.setToolTipText(antiAliasCheck, text);
			antiAliasCheck.CheckedChanged += new System.EventHandler(updateItemListener.itemStateChanged);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			antiAliasBox.Controls.Add(antiAliasCheck);
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createGlue' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			System.Windows.Forms.Control temp_Control4;
			temp_Control4 = Box.createGlue();
			antiAliasBox.Controls.Add(temp_Control4);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			povOptionsBox.Controls.Add(antiAliasBox);
			
			// Display when rendering option
			//UPGRADE_ISSUE: Class 'javax.swing.Box' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createHorizontalBox' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			Box displayBox = Box.createHorizontalBox();
			text = GT._("Display While Rendering");
			displayWhileRenderingCheck = SupportClass.CheckBoxSupport.CreateCheckBox(text, true);
			text = GT._("Should povray attempt to display while rendering?");
			SupportClass.ToolTipSupport.setToolTipText(displayWhileRenderingCheck, text);
			displayWhileRenderingCheck.CheckedChanged += new System.EventHandler(updateItemListener.itemStateChanged);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			displayBox.Controls.Add(displayWhileRenderingCheck);
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createGlue' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			System.Windows.Forms.Control temp_Control5;
			temp_Control5 = Box.createGlue();
			displayBox.Controls.Add(temp_Control5);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			povOptionsBox.Controls.Add(displayBox);
			
			// Image size option
			//UPGRADE_ISSUE: Class 'javax.swing.Box' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createHorizontalBox' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			Box imageBox = Box.createHorizontalBox();
			text = GT._("Image size");
			imageSizeCheck = SupportClass.CheckBoxSupport.CreateCheckBox(text, false);
			text = GT._("Image size");
			SupportClass.ToolTipSupport.setToolTipText(imageSizeCheck, text);
			imageSizeCheck.CheckedChanged += new System.EventHandler(new AnonymousClassItemListener1(this).itemStateChanged);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			imageBox.Controls.Add(imageSizeCheck);
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createHorizontalStrut' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			System.Windows.Forms.Control temp_Control6;
			temp_Control6 = Box.createHorizontalStrut(10);
			imageBox.Controls.Add(temp_Control6);
			//UPGRADE_ISSUE: Class 'javax.swing.Box' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createVerticalBox' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			Box imageSizeDetailBox = Box.createVerticalBox();
			//UPGRADE_ISSUE: Class 'javax.swing.Box' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createHorizontalBox' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			Box imageSizeXYBox = Box.createHorizontalBox();
			text = GT._("Width : ");
			System.Windows.Forms.Label temp_label;
			temp_label = new System.Windows.Forms.Label();
			temp_label.Text = text;
			imageSizeWidth = temp_label;
			text = GT._("Image width");
			SupportClass.ToolTipSupport.setToolTipText(imageSizeWidth, text);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			imageSizeXYBox.Controls.Add(imageSizeWidth);
			imageSizeTextWidth = new JFormattedTextField();
			imageSizeTextWidth.setValue((System.Int32) outputWidth);
			imageSizeTextWidth.addPropertyChangeListener("value", new AnonymousClassPropertyChangeListener(this));
			imageSizeXYBox.add(imageSizeTextWidth);
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createHorizontalStrut' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			System.Windows.Forms.Control temp_Control7;
			temp_Control7 = Box.createHorizontalStrut(10);
			imageSizeXYBox.Controls.Add(temp_Control7);
			text = GT._("Height : ");
			System.Windows.Forms.Label temp_label2;
			temp_label2 = new System.Windows.Forms.Label();
			temp_label2.Text = text;
			imageSizeHeight = temp_label2;
			text = GT._("Image height");
			SupportClass.ToolTipSupport.setToolTipText(imageSizeHeight, text);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			imageSizeXYBox.Controls.Add(imageSizeHeight);
			imageSizeTextHeight = new JFormattedTextField();
			imageSizeTextHeight.setValue((System.Int32) outputHeight);
			imageSizeTextHeight.addPropertyChangeListener("value", new AnonymousClassPropertyChangeListener1(this));
			imageSizeXYBox.add(imageSizeTextHeight);
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createGlue' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			System.Windows.Forms.Control temp_Control8;
			temp_Control8 = Box.createGlue();
			imageSizeXYBox.Controls.Add(temp_Control8);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			imageSizeDetailBox.Controls.Add(imageSizeXYBox);
			//UPGRADE_ISSUE: Class 'javax.swing.Box' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createHorizontalBox' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			Box imageSizeBox = Box.createHorizontalBox();
			text = GT._("Fixed ratio : ");
			imageSizeRatioBox = SupportClass.CheckBoxSupport.CreateCheckBox(text, true);
			text = GT._("Use a fixed ratio for width:height");
			SupportClass.ToolTipSupport.setToolTipText(imageSizeRatioBox, text);
			imageSizeRatioBox.CheckedChanged += new System.EventHandler(new AnonymousClassItemListener2(this).itemStateChanged);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			imageSizeBox.Controls.Add(imageSizeRatioBox);
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createHorizontalStrut' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			System.Windows.Forms.Control temp_Control9;
			temp_Control9 = Box.createHorizontalStrut(10);
			imageSizeBox.Controls.Add(temp_Control9);
			imageSizeRatioCombo = new System.Windows.Forms.ComboBox();
			text = GT._("User defined");
			imageSizeRatioCombo.Items.Add(text);
			text = GT._("Keep ratio of Jmol window");
			imageSizeRatioCombo.Items.Add(text);
			text = "4:3";
			imageSizeRatioCombo.Items.Add(text);
			text = "16:9";
			imageSizeRatioCombo.Items.Add(text);
			imageSizeRatioCombo.SelectedIndex = 1;
			imageSizeRatioCombo.SelectedValueChanged += new System.EventHandler(new AnonymousClassActionListener2(this).actionPerformed);
			SupportClass.CommandManager.CheckCommand(imageSizeRatioCombo);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			imageSizeBox.Controls.Add(imageSizeRatioCombo);
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createGlue' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			System.Windows.Forms.Control temp_Control10;
			temp_Control10 = Box.createGlue();
			imageSizeBox.Controls.Add(temp_Control10);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			imageSizeDetailBox.Controls.Add(imageSizeBox);
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createGlue' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			System.Windows.Forms.Control temp_Control11;
			temp_Control11 = Box.createGlue();
			imageSizeDetailBox.Controls.Add(temp_Control11);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			imageBox.Controls.Add(imageSizeDetailBox);
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createGlue' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			System.Windows.Forms.Control temp_Control12;
			temp_Control12 = Box.createGlue();
			imageBox.Controls.Add(temp_Control12);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			povOptionsBox.Controls.Add(imageBox);
			imageSizeChanged();
			
			// Output format option
			//UPGRADE_ISSUE: Class 'javax.swing.Box' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createHorizontalBox' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			Box outputBox = Box.createHorizontalBox();
			text = GT._("Output format : ");
			outputFormatCheck = SupportClass.CheckBoxSupport.CreateCheckBox(text, false);
			text = GT._("Select the file format of the output file");
			SupportClass.ToolTipSupport.setToolTipText(outputFormatCheck, text);
			outputFormatCheck.CheckedChanged += new System.EventHandler(new AnonymousClassItemListener3(this).itemStateChanged);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			outputBox.Controls.Add(outputFormatCheck);
			outputFormatCombo = new System.Windows.Forms.ComboBox();
			text = GT._("C - Compressed Targa-24");
			outputFormatCombo.Items.Add(text);
			text = GT._("N - PNG");
			outputFormatCombo.Items.Add(text);
			text = GT._("P - PPM");
			outputFormatCombo.Items.Add(text);
			text = GT._("T - Uncompressed Targa-24");
			outputFormatCombo.Items.Add(text);
			outputFormatCombo.SelectedIndex = 3;
			outputFormatCombo.SelectedValueChanged += new System.EventHandler(new AnonymousClassActionListener3(this).actionPerformed);
			SupportClass.CommandManager.CheckCommand(outputFormatCombo);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			outputBox.Controls.Add(outputFormatCombo);
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createGlue' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			System.Windows.Forms.Control temp_Control13;
			temp_Control13 = Box.createGlue();
			outputBox.Controls.Add(temp_Control13);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			povOptionsBox.Controls.Add(outputBox);
			outputFormatChanged();
			
			// Alpha option
			//UPGRADE_ISSUE: Class 'javax.swing.Box' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createHorizontalBox' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			Box alphaBox = Box.createHorizontalBox();
			text = GT._("Alpha transparency");
			outputAlphaCheck = SupportClass.CheckBoxSupport.CreateCheckBox(text, false);
			text = GT._("Output Alpha transparency data");
			SupportClass.ToolTipSupport.setToolTipText(outputAlphaCheck, text);
			outputAlphaCheck.CheckedChanged += new System.EventHandler(new AnonymousClassItemListener4(this).itemStateChanged);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			alphaBox.Controls.Add(outputAlphaCheck);
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createGlue' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			System.Windows.Forms.Control temp_Control14;
			temp_Control14 = Box.createGlue();
			alphaBox.Controls.Add(temp_Control14);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			povOptionsBox.Controls.Add(alphaBox);
			
			// Mosaic preview option
			//UPGRADE_ISSUE: Class 'javax.swing.Box' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createHorizontalBox' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			Box mosaicBox = Box.createHorizontalBox();
			text = GT._("Mosaic preview");
			mosaicPreviewCheck = SupportClass.CheckBoxSupport.CreateCheckBox(text, false);
			text = GT._("Render the image in several passes");
			SupportClass.ToolTipSupport.setToolTipText(mosaicPreviewCheck, text);
			mosaicPreviewCheck.CheckedChanged += new System.EventHandler(new AnonymousClassItemListener5(this).itemStateChanged);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			mosaicBox.Controls.Add(mosaicPreviewCheck);
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createHorizontalStrut' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			System.Windows.Forms.Control temp_Control15;
			temp_Control15 = Box.createHorizontalStrut(10);
			mosaicBox.Controls.Add(temp_Control15);
			text = GT._("Start size : ");
			System.Windows.Forms.Label temp_label3;
			temp_label3 = new System.Windows.Forms.Label();
			temp_label3.Text = text;
			mosaicPreviewStart = temp_label3;
			text = GT._("Inital size of the tiles");
			SupportClass.ToolTipSupport.setToolTipText(mosaicPreviewStart, text);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			mosaicBox.Controls.Add(mosaicPreviewStart);
			mosaicPreviewComboStart = new System.Windows.Forms.ComboBox();
			for (int power = 0; power < 8; power++)
			{
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				mosaicPreviewComboStart.Items.Add(System.Convert.ToString((int) System.Math.Pow(2, power)));
			}
			mosaicPreviewComboStart.SelectedIndex = 3;
			mosaicPreviewComboStart.SelectedValueChanged += new System.EventHandler(new AnonymousClassActionListener4(this).actionPerformed);
			SupportClass.CommandManager.CheckCommand(mosaicPreviewComboStart);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			mosaicBox.Controls.Add(mosaicPreviewComboStart);
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createHorizontalStrut' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			System.Windows.Forms.Control temp_Control16;
			temp_Control16 = Box.createHorizontalStrut(10);
			mosaicBox.Controls.Add(temp_Control16);
			text = GT._("End size : ");
			System.Windows.Forms.Label temp_label4;
			temp_label4 = new System.Windows.Forms.Label();
			temp_label4.Text = text;
			mosaicPreviewEnd = temp_label4;
			text = GT._("Final size of the tiles");
			SupportClass.ToolTipSupport.setToolTipText(mosaicPreviewEnd, text);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			mosaicBox.Controls.Add(mosaicPreviewEnd);
			mosaicPreviewComboEnd = new System.Windows.Forms.ComboBox();
			for (int power = 0; power < 8; power++)
			{
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				mosaicPreviewComboEnd.Items.Add(System.Convert.ToString((int) System.Math.Pow(2, power)));
			}
			mosaicPreviewComboEnd.SelectedIndex = 0;
			mosaicPreviewComboEnd.SelectedValueChanged += new System.EventHandler(new AnonymousClassActionListener5(this).actionPerformed);
			SupportClass.CommandManager.CheckCommand(mosaicPreviewComboEnd);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			mosaicBox.Controls.Add(mosaicPreviewComboEnd);
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createGlue' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			System.Windows.Forms.Control temp_Control17;
			temp_Control17 = Box.createGlue();
			mosaicBox.Controls.Add(temp_Control17);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			povOptionsBox.Controls.Add(mosaicBox);
			mosaicPreviewChanged();
			
			//GUI for povray path selection
			//UPGRADE_ISSUE: Class 'javax.swing.Box' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createHorizontalBox' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			Box povrayPathBox = Box.createHorizontalBox();
			text = GT._("POV-Ray Executable Location");
			//UPGRADE_ISSUE: Constructor 'javax.swing.border.TitledBorder.TitledBorder' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingborderTitledBorder'"
			povrayPathBox.setBorder(new TitledBorder(text));
			text = GT._("Location of the povray Executable");
			povrayPathBox.setToolTipText(text);
			System.Windows.Forms.TextBox temp_text_box3;
			temp_text_box3 = new System.Windows.Forms.TextBox();
			temp_text_box3.Text = "";
			povrayPathLabel = temp_text_box3;
			povrayPathLabel.ReadOnly = !false;
			//UPGRADE_TODO: Method 'javax.swing.JComponent.setBorder' was converted to 'System.Windows.Forms.ControlPaint.DrawBorder3D' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJComponentsetBorder_javaxswingborderBorder'"
			System.Windows.Forms.ControlPaint.DrawBorder3D(povrayPathLabel.CreateGraphics(), 0, 0, povrayPathLabel.Width, povrayPathLabel.Height, System.Windows.Forms.Border3DStyle.Flat);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			povrayPathBox.Controls.Add(povrayPathLabel);
			text = GT._("Select");
			povrayPathButton = SupportClass.ButtonSupport.CreateStandardButton(text);
			povrayPathButton.Click += new System.EventHandler(new AnonymousClassActionListener6(this).actionPerformed);
			SupportClass.CommandManager.CheckCommand(povrayPathButton);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			povrayPathBox.Controls.Add(povrayPathButton);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			povOptionsBox.Controls.Add(povrayPathBox);
			
			//GUI for command selection
			//UPGRADE_ISSUE: Class 'javax.swing.Box' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createVerticalBox' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			Box commandLineBox = Box.createVerticalBox();
			text = GT._("Command Line to Execute");
			//UPGRADE_ISSUE: Constructor 'javax.swing.border.TitledBorder.TitledBorder' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingborderTitledBorder'"
			commandLineBox.setBorder(new TitledBorder(text));
			text = GT._("The actual command which will be executed");
			commandLineBox.setToolTipText(text);
			//UPGRADE_TODO: Constructor 'javax.swing.JTextField.JTextField' was converted to 'System.Windows.Forms.TextBox' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJTextFieldJTextField_int'"
			commandLineField = new System.Windows.Forms.TextBox();
			text = GT._("The actual command which will be executed");
			SupportClass.ToolTipSupport.setToolTipText(commandLineField, text);
			//UPGRADE_TODO: Method 'javax.swing.JTextField.addActionListener' was converted to 'System.Windows.Forms.TextBox.KeyPress' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJTextFieldaddActionListener_javaawteventActionListener'"
			commandLineField.KeyPress += new System.Windows.Forms.KeyPressEventHandler(updateActionListener.actionPerformed);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			commandLineBox.Controls.Add(commandLineField);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			povOptionsBox.Controls.Add(commandLineBox);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			mainBox.Controls.Add(povOptionsBox);
			
			//GUI for panel with go, cancel and stop (etc) buttons
			//UPGRADE_ISSUE: Class 'javax.swing.Box' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createHorizontalBox' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			Box buttonBox = Box.createHorizontalBox();
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createGlue' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			System.Windows.Forms.Control temp_Control18;
			temp_Control18 = Box.createGlue();
			buttonBox.Controls.Add(temp_Control18);
			text = GT._("Go!");
			goButton = SupportClass.ButtonSupport.CreateStandardButton(text);
			text = GT._("Save file and possibly launch povray");
			SupportClass.ToolTipSupport.setToolTipText(goButton, text);
			goButton.Click += new System.EventHandler(new AnonymousClassActionListener7(this).actionPerformed);
			SupportClass.CommandManager.CheckCommand(goButton);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			buttonBox.Controls.Add(goButton);
			text = GT._("Cancel");
			System.Windows.Forms.Button cancelButton = SupportClass.ButtonSupport.CreateStandardButton(text);
			text = GT._("Cancel this dialog without saving");
			SupportClass.ToolTipSupport.setToolTipText(cancelButton, text);
			cancelButton.Click += new System.EventHandler(new AnonymousClassActionListener8(this).actionPerformed);
			SupportClass.CommandManager.CheckCommand(cancelButton);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			buttonBox.Controls.Add(cancelButton);
			
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			windowBox.Controls.Add(mainBox);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			windowBox.Controls.Add(buttonBox);
			
			getPathHistory();
			updateScreen();
			//UPGRADE_ISSUE: Method 'java.awt.Window.pack' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtWindowpack'"
			pack();
			centerDialog();
			//UPGRADE_TODO: Method 'java.awt.Component.setVisible' was converted to 'System.Windows.Forms.Control.Visible' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetVisible_boolean'"
			//UPGRADE_TODO: 'System.Windows.Forms.Application.Run' must be called to start a main form. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1135'"
			Visible = true;
		}
		
		/// <summary>  Sets the output image dimensions. Setting either to &lt;= 0 will
		/// remove the height and width specification from the commandline- the
		/// resulting behaviour depends on povray!
		/// </summary>
		/// <param name="imageWidth">The width of the image.
		/// </param>
		/// <param name="imageHeight">The height of the image.
		/// </param>
		public virtual void  setImageDimensions(int imageWidth, int imageHeight)
		{
			outputWidth = imageWidth;
			outputHeight = imageHeight;
			updateCommandLine();
		}
		
		/// <summary> Save or else launch povray- ie do our thang!</summary>
		internal virtual void  goPressed()
		{
			
			// File theFile = new.getSelectedFile();
			System.String basename = saveField.Text;
			System.String filename = basename + ".pov";
			System.String savePath = savePathLabel.Text;
			System.IO.FileInfo theFile = new System.IO.FileInfo(savePath + "\\" + filename);
			if (theFile != null)
			{
				try
				{
					//UPGRADE_TODO: Constructor 'java.io.FileOutputStream.FileOutputStream' was converted to 'System.IO.FileStream.FileStream' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileOutputStreamFileOutputStream_javaioFile'"
					System.IO.FileStream os = new System.IO.FileStream(theFile.FullName, System.IO.FileMode.Create);
					
					bool allFrames = false;
					if (allFramesCheck != null)
					{
						allFrames = allFramesCheck.Checked;
					}
					int width = outputWidth;
					int height = outputHeight;
					if ((imageSizeCheck != null) && (imageSizeCheck.Checked))
					{
						height = Integer.parseInt(imageSizeTextHeight.getText());
						width = Integer.parseInt(imageSizeTextWidth.getText());
					}
					PovraySaver povs = new PovraySaver(viewer, os, allFrames, width, height);
					povs.writeFile();
				}
				catch (System.IO.FileNotFoundException fnf)
				{
					System.Console.Out.WriteLine("Povray Dialog FileNotFoundException:" + theFile);
					return ;
				}
			}
			
			// Create INI file if needed
			bool useIniFile = useIniCheck.Checked;
			if (useIniFile)
			{
				filename = basename + ".ini";
				theFile = new System.IO.FileInfo(savePath + "\\" + filename);
				try
				{
					//UPGRADE_TODO: Class 'java.io.FileWriter' was converted to 'System.IO.StreamWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileWriter'"
					//UPGRADE_TODO: Constructor 'java.io.FileWriter.FileWriter' was converted to 'System.IO.StreamWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileWriterFileWriter_javaioFile'"
					System.IO.StreamWriter os = new System.IO.StreamWriter(theFile.FullName, false, System.Text.Encoding.Default);
					saveIni(os);
					os.Close();
				}
				catch (System.IO.FileNotFoundException fnf)
				{
					System.Console.Out.WriteLine("Povray Dialog FileNotFoundException:" + theFile);
					return ;
				}
				catch (System.IO.IOException ioe)
				{
					System.Console.Out.WriteLine("Povray Dialog IOException:" + theFile);
					return ;
				}
			}
			
			// Run Povray if needed
			bool callPovray = runPovCheck.Checked;
			if (callPovray)
			{
				System.String[] commandLineArgs = null;
				if (useIniFile)
				{
					if (!savePath.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
					{
						savePath += System.IO.Path.DirectorySeparatorChar.ToString();
					}
					commandLineArgs = new System.String[]{povrayPathLabel.Text, savePath + filename};
				}
				else
				{
					commandLineArgs = CommandLineArgs;
				}
				try
				{
					SupportClass.ExecSupport(commandLineArgs);
				}
				catch (System.IO.IOException e)
				{
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					System.Console.Out.WriteLine("Caught IOException in povray exec: " + e);
					System.Console.Out.WriteLine("CmdLine:");
					for (int i = 0; i < commandLineArgs.Length; i++)
					{
						System.Console.Out.WriteLine("  <" + commandLineArgs[i] + ">");
					}
				}
			}
			//UPGRADE_TODO: Method 'java.awt.Component.setVisible' was converted to 'System.Windows.Forms.Control.Visible' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetVisible_boolean'"
			//UPGRADE_TODO: 'System.Windows.Forms.Application.Run' must be called to start a main form. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1135'"
			Visible = false;
			saveHistory();
			Dispose();
		}
		
		/// <summary> Responds to cancel being press- or equivalent eg window closed.</summary>
		internal virtual void  cancelPressed()
		{
			//UPGRADE_TODO: Method 'java.awt.Component.setVisible' was converted to 'System.Windows.Forms.Control.Visible' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetVisible_boolean'"
			//UPGRADE_TODO: 'System.Windows.Forms.Application.Run' must be called to start a main form. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1135'"
			Visible = false;
			Dispose();
		}
		
		/// <summary> Show a file selector when the savePath button is pressed.</summary>
		internal virtual void  showSavePathDialog()
		{
			
			//UPGRADE_TODO: Constructor may need to be changed depending on function performed by the 'System.Windows.Forms.FileDialog' object. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1270'"
			System.Windows.Forms.FileDialog myChooser = new System.Windows.Forms.OpenFileDialog();
			//UPGRADE_ISSUE: Method 'javax.swing.JFileChooser.setFileSelectionMode' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJFileChoosersetFileSelectionMode_int'"
			myChooser.setFileSelectionMode(1);
			GT._("Select");
			//UPGRADE_TODO: The equivalent in .NET for method 'javax.swing.JFileChooser.showDialog' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			//UPGRADE_TODO: Method 'javax.swing.JFileChooser.showDialog' was converted to 'System.Windows.Forms.OpenFileDialog.ShowDialog' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJFileChoosershowDialog_javaawtComponent_javalangString'"
			int button = (int) myChooser.ShowDialog(this);
			//UPGRADE_TODO: The equivalent in .NET for field 'javax.swing.JFileChooser.APPROVE_OPTION' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			if (button == (int) System.Windows.Forms.DialogResult.OK)
			{
				System.IO.FileInfo newFile = new System.IO.FileInfo(myChooser.FileName);
				System.String savePath;
				if (System.IO.Directory.Exists(newFile.FullName))
				{
					savePath = newFile.ToString();
				}
				else
				{
					savePath = newFile.DirectoryName.ToString();
				}
				//UPGRADE_TODO: Method 'javax.swing.text.JTextComponent.setText' was converted to 'System.Windows.Forms.TextBoxBase.Text' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingtextJTextComponentsetText_javalangString'"
				savePathLabel.Text = savePath;
				updateCommandLine();
				//UPGRADE_ISSUE: Method 'java.awt.Window.pack' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtWindowpack'"
				pack();
			}
		}
		
		/// <summary> Show a file selector when the savePath button is pressed.</summary>
		internal virtual void  showPovrayPathDialog()
		{
			
			//UPGRADE_TODO: Constructor may need to be changed depending on function performed by the 'System.Windows.Forms.FileDialog' object. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1270'"
			System.Windows.Forms.FileDialog myChooser = new System.Windows.Forms.OpenFileDialog();
			GT._("Select");
			//UPGRADE_TODO: The equivalent in .NET for method 'javax.swing.JFileChooser.showDialog' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			//UPGRADE_TODO: Method 'javax.swing.JFileChooser.showDialog' was converted to 'System.Windows.Forms.OpenFileDialog.ShowDialog' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJFileChoosershowDialog_javaawtComponent_javalangString'"
			int button = (int) myChooser.ShowDialog(this);
			//UPGRADE_TODO: The equivalent in .NET for field 'javax.swing.JFileChooser.APPROVE_OPTION' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			if (button == (int) System.Windows.Forms.DialogResult.OK)
			{
				System.IO.FileInfo newFile = new System.IO.FileInfo(myChooser.FileName);
				//UPGRADE_TODO: Method 'javax.swing.text.JTextComponent.setText' was converted to 'System.Windows.Forms.TextBoxBase.Text' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingtextJTextComponentsetText_javalangString'"
				povrayPathLabel.Text = newFile.ToString();
				updateCommandLine();
				//UPGRADE_ISSUE: Method 'java.awt.Window.pack' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtWindowpack'"
				pack();
			}
		}
		
		/// <summary> Called when the ImageSize check box is modified </summary>
		internal virtual void  imageSizeChanged()
		{
			if (imageSizeCheck != null)
			{
				bool selected = imageSizeCheck.Checked;
				bool enabled = runPovCheck.Checked || useIniCheck.Checked;
				bool ratioSelected = false;
				imageSizeCheck.Enabled = enabled;
				if (imageSizeRatioBox != null)
				{
					ratioSelected = imageSizeRatioBox.Checked;
					imageSizeRatioBox.Enabled = selected && enabled;
				}
				if (imageSizeWidth != null)
				{
					imageSizeWidth.Enabled = selected && enabled;
				}
				if (imageSizeTextWidth != null)
				{
					imageSizeTextWidth.setEnabled(selected && enabled);
				}
				if (imageSizeHeight != null)
				{
					imageSizeHeight.Enabled = selected && !ratioSelected && enabled;
				}
				if (imageSizeTextHeight != null)
				{
					imageSizeTextHeight.setEnabled(selected && !ratioSelected && enabled);
				}
				if (imageSizeRatioCombo != null)
				{
					imageSizeRatioCombo.Enabled = selected && ratioSelected && enabled;
					if ((imageSizeTextWidth != null) && (imageSizeTextHeight != null))
					{
						int width = Integer.parseInt(imageSizeTextWidth.getValue().toString());
						int height;
						switch (imageSizeRatioCombo.SelectedIndex)
						{
							
							case 0:  // Free
								break;
							
							case 1:  // Jmol
								//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
								height = (int) (((double) width) * outputHeight / outputWidth);
								imageSizeTextHeight.setValue((System.Int32) height);
								break;
							
							case 2:  // 4/3
								//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
								height = (int) (((double) width) * 3 / 4);
								imageSizeTextHeight.setValue((System.Int32) height);
								break;
							
							case 3:  // 16/9
								//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
								height = (int) (((double) width) * 9 / 16);
								imageSizeTextHeight.setValue((System.Int32) height);
								break;
							}
					}
				}
			}
		}
		
		/// <summary> Called when the OutputFormat check box is modified </summary>
		internal virtual void  outputFormatChanged()
		{
			if (outputFormatCheck != null)
			{
				bool selected = outputFormatCheck.Checked;
				bool enabled = runPovCheck.Checked || useIniCheck.Checked;
				outputFormatCheck.Enabled = enabled;
				if (outputFormatCombo != null)
				{
					outputFormatCombo.Enabled = selected && enabled;
				}
			}
		}
		
		/// <summary> Called when the MosaicPreview check box is modified </summary>
		internal virtual void  mosaicPreviewChanged()
		{
			if (mosaicPreviewCheck != null)
			{
				bool selected = mosaicPreviewCheck.Checked;
				bool enabled = runPovCheck.Checked || useIniCheck.Checked;
				mosaicPreviewCheck.Enabled = enabled;
				if (mosaicPreviewStart != null)
				{
					mosaicPreviewStart.Enabled = selected && enabled;
				}
				if (mosaicPreviewComboStart != null)
				{
					mosaicPreviewComboStart.Enabled = selected && enabled;
				}
				if (mosaicPreviewEnd != null)
				{
					mosaicPreviewEnd.Enabled = selected && enabled;
				}
				if (mosaicPreviewComboEnd != null)
				{
					mosaicPreviewComboEnd.Enabled = selected && enabled;
				}
			}
		}
		
		/// <summary> Update screen informations</summary>
		protected internal virtual void  updateScreen()
		{
			
			// Call povray ?
			bool callPovray = false;
			if (runPovCheck != null)
			{
				callPovray = runPovCheck.Checked;
			}
			System.String text = null;
			if (callPovray)
			{
				text = GT._("Go!");
			}
			else
			{
				text = GT._("Save");
			}
			if (goButton != null)
			{
				goButton.Text = text;
			}
			
			// Use INI ?
			bool useIni = false;
			if (useIniCheck != null)
			{
				useIni = useIniCheck.Checked;
			}
			
			// Update state
			if (antiAliasCheck != null)
			{
				antiAliasCheck.Enabled = callPovray || useIni;
			}
			if (povrayPathButton != null)
			{
				povrayPathButton.Enabled = callPovray || useIni;
			}
			if (displayWhileRenderingCheck != null)
			{
				displayWhileRenderingCheck.Enabled = callPovray || useIni;
			}
			if (antiAliasCheck != null)
			{
				antiAliasCheck.Enabled = callPovray || useIni;
			}
			if (commandLineField != null)
			{
				commandLineField.Enabled = callPovray && !useIni;
			}
			
			// Various update
			imageSizeChanged();
			outputFormatChanged();
			mosaicPreviewChanged();
			
			// Update command line
			updateCommandLine();
		}
		
		/// <summary> Generates a commandline from the options set for povray path
		/// etc and sets in the textField.
		/// </summary>
		protected internal virtual void  updateCommandLine()
		{
			
			// Check fields
			System.String basename = null;
			if (saveField != null)
			{
				basename = saveField.Text;
			}
			System.String savePath = null;
			if (savePathLabel != null)
			{
				savePath = savePathLabel.Text;
			}
			System.String povrayPath = null;
			if (povrayPathLabel != null)
			{
				povrayPath = povrayPathLabel.Text;
			}
			if ((savePath == null) || (povrayPath == null) || (basename == null))
			{
				if (commandLineField != null)
				{
					//UPGRADE_TODO: Method 'javax.swing.text.JTextComponent.setText' was converted to 'System.Windows.Forms.TextBoxBase.Text' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingtextJTextComponentsetText_javalangString'"
					commandLineField.Text = GT._("null component string");
				}
				return ;
			}
			
			//Append a file separator to the savePath is necessary
			if (!savePath.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
			{
				savePath += System.IO.Path.DirectorySeparatorChar.ToString();
			}
			
			System.String commandLine = doubleQuoteIfContainsSpace(povrayPath) + " +I" + simpleQuoteIfContainsSpace(savePath + basename + ".pov");
			
			// Output format options
			System.String outputExtension = ".tga";
			System.String outputFileType = " +FT";
			if ((outputFormatCheck != null) && (outputFormatCheck.Checked))
			{
				switch (outputFormatCombo.SelectedIndex)
				{
					
					case 0:  // Compressed TARGA
						outputFileType = " +FC";
						break;
					
					case 1:  // PNG
						outputExtension = ".png";
						outputFileType = " +FN";
						break;
					
					case 2:  // PPM
						outputExtension = ".ppm";
						outputFileType = " +FP";
						break;
					
					default:  // Uncompressed TARGA
						break;
					
				}
			}
			commandLine += (" +O" + simpleQuoteIfContainsSpace(savePath + basename + outputExtension) + outputFileType);
			
			// Output alpha options
			if ((outputAlphaCheck != null) && (outputAlphaCheck.Checked))
			{
				commandLine += " +UA";
			}
			
			// Image size options
			if ((imageSizeCheck != null) && (imageSizeCheck.Checked))
			{
				commandLine += (" +H" + imageSizeTextHeight.getValue() + " +W" + imageSizeTextWidth.getValue());
			}
			else
			{
				if ((outputWidth > 0) && (outputHeight > 0))
				{
					commandLine += (" +H" + outputHeight + " +W" + outputWidth);
				}
			}
			
			// Anti Alias
			if ((antiAliasCheck != null) && (antiAliasCheck.Checked))
			{
				commandLine += " +A0.1";
			}
			
			// Display while rendering
			if ((displayWhileRenderingCheck != null) && (displayWhileRenderingCheck.Checked))
			{
				commandLine += " +D +P";
			}
			
			// Animation options
			if ((allFramesCheck != null) && (allFramesCheck.Checked))
			{
				commandLine += " +KFI1";
				commandLine += (" +KFF" + viewer.ModelCount);
				commandLine += " +KI1";
				commandLine += (" +KF" + viewer.ModelCount);
			}
			
			// Mosaic preview options
			if ((mosaicPreviewCheck != null) && (mosaicPreviewCheck.Checked))
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				commandLine += (" +SP" + mosaicPreviewComboStart.SelectedItem + " +EP" + mosaicPreviewComboEnd.SelectedItem);
			}
			
			commandLine += " -V"; // turn off verbose messages ... although it is still rather verbose
			
			if (commandLineField != null)
			{
				//UPGRADE_TODO: Method 'javax.swing.text.JTextComponent.setText' was converted to 'System.Windows.Forms.TextBoxBase.Text' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingtextJTextComponentsetText_javalangString'"
				commandLineField.Text = commandLine;
			}
		}
		
		/// <summary> Save INI file
		/// 
		/// </summary>
		/// <param name="os">Output stream
		/// </param>
		/// <throws>  IOException </throws>
		//UPGRADE_TODO: Class 'java.io.FileWriter' was converted to 'System.IO.StreamWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileWriter'"
		private void  saveIni(System.IO.StreamWriter os)
		{
			if (os == null)
			{
				return ;
			}
			
			// Save path
			System.String savePath = savePathLabel.Text;
			if (!savePath.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
			{
				savePath += System.IO.Path.DirectorySeparatorChar.ToString();
			}
			System.String basename = saveField.Text;
			
			// Input file
			os.Write("Input_File_Name=" + savePath + basename + ".pov\n");
			
			// Output format options
			System.String outputExtension = ".tga";
			System.String outputFileType = "T";
			if ((outputFormatCheck != null) && (outputFormatCheck.Checked))
			{
				switch (outputFormatCombo.SelectedIndex)
				{
					
					case 0:  // Compressed TARGA
						outputFileType = "C";
						break;
					
					case 1:  // PNG
						outputExtension = ".png";
						outputFileType = "N";
						break;
					
					case 2:  // PPM
						outputExtension = ".ppm";
						outputFileType = "P";
						break;
					
					default:  // Uncompressed TARGA
						break;
					
				}
			}
			os.Write("Output_to_File=true\n");
			os.Write("Output_File_Type=" + outputFileType + "\n");
			os.Write("Output_File_Name=" + savePath + basename + outputExtension + "\n");
			
			// Output alpha options
			if ((outputAlphaCheck != null) && (outputAlphaCheck.Checked))
			{
				os.Write("Output_Alpha=true\n");
			}
			
			// Image size options
			if ((imageSizeCheck != null) && (imageSizeCheck.Checked))
			{
				os.Write("Height=" + imageSizeTextHeight.getValue() + "\n");
				os.Write("Width=" + imageSizeTextWidth.getValue() + "\n");
			}
			else
			{
				if ((outputWidth > 0) && (outputHeight > 0))
				{
					os.Write("Height=" + outputHeight + "\n");
					os.Write("Width=" + outputWidth + "\n");
				}
			}
			
			// Anti Alias
			if ((antiAliasCheck != null) && (antiAliasCheck.Checked))
			{
				os.Write("Antialias=true\n");
				os.Write("Antialias_Threshold=0.1\n");
			}
			
			// Display while rendering
			if ((displayWhileRenderingCheck != null) && (displayWhileRenderingCheck.Checked))
			{
				os.Write("Display=true\n");
				os.Write("Pause_When_Done=true\n");
			}
			
			// Animation options
			if ((allFramesCheck != null) && (allFramesCheck.Checked))
			{
				os.Write("Initial_Frame=1\n");
				os.Write("Final_Frame=" + viewer.ModelCount + "\n");
				os.Write("Initial_Clock=1\n");
				os.Write("Final_Clock=" + viewer.ModelCount + "\n");
			}
			
			// Mosaic preview options
			if ((mosaicPreviewCheck != null) && (mosaicPreviewCheck.Checked))
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				os.Write("Preview_Start_Size=" + mosaicPreviewComboStart.SelectedItem + "\n");
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				os.Write("Preview_End_Size=" + mosaicPreviewComboEnd.SelectedItem + "\n");
			}
			
			os.Write("Verbose=false\n");
		}
		
		/// <summary> Centers the dialog on the screen.</summary>
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
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'PovrayWindowListener' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		/// <summary> Listener for responding to dialog window events.</summary>
		internal class PovrayWindowListener
		{
			public PovrayWindowListener(PovrayDialog enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(PovrayDialog enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private PovrayDialog enclosingInstance;
			public PovrayDialog Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			/// <summary> Closes the dialog when window closing event occurs.</summary>
			/// <param name="e">Event
			/// </param>
			public void  windowClosing(System.Object event_sender, System.ComponentModel.CancelEventArgs e)
			{
				e.Cancel = true;
				Enclosing_Instance.cancelPressed();
				Enclosing_Instance.Visible = false;
				Enclosing_Instance.Dispose();
			}
		}
		
		/// <summary> Just recovers the path settings from last session.</summary>
		private void  getPathHistory()
		{
			
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			System.Collections.Specialized.NameValueCollection props = Jmol.HistoryFile.Properties;
			if (povrayPathLabel != null)
			{
				//UPGRADE_TODO: Method 'java.lang.System.getProperty' was converted to 'System.Environment.GetEnvironmentVariable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangSystemgetProperty_javalangString'"
				System.String povrayPath = props["povrayPath"] == null?System.Environment.GetEnvironmentVariable("userprofile"):props["povrayPath"];
				if (povrayPath != null)
				{
					//UPGRADE_TODO: Method 'javax.swing.text.JTextComponent.setText' was converted to 'System.Windows.Forms.TextBoxBase.Text' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingtextJTextComponentsetText_javalangString'"
					povrayPathLabel.Text = povrayPath;
				}
			}
			if (savePathLabel != null)
			{
				//UPGRADE_TODO: Method 'java.lang.System.getProperty' was converted to 'System.Environment.GetEnvironmentVariable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangSystemgetProperty_javalangString'"
				System.String savePath = props["povraySavePath"] == null?System.Environment.GetEnvironmentVariable("userprofile"):props["povraySavePath"];
				if (savePath != null)
				{
					//UPGRADE_TODO: Method 'javax.swing.text.JTextComponent.setText' was converted to 'System.Windows.Forms.TextBoxBase.Text' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingtextJTextComponentsetText_javalangString'"
					savePathLabel.Text = savePath;
				}
			}
		}
		
		/// <summary> Just saves the path settings from this session.</summary>
		private void  saveHistory()
		{
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			//UPGRADE_TODO: Format of property file may need to be changed. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1089'"
			System.Collections.Specialized.NameValueCollection props = new System.Collections.Specialized.NameValueCollection();
			//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
			props["povrayPath"] = povrayPathLabel.Text;
			//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
			props["povraySavePath"] = savePathLabel.Text;
			Jmol.HistoryFile.addProperties(props);
		}
		
		internal virtual System.String doubleQuoteIfContainsSpace(System.String str)
		{
			for (int i = str.Length; --i >= 0; )
				if (str[i] == ' ')
					return "\"" + str + "\"";
			return str;
		}
		
		internal virtual System.String simpleQuoteIfContainsSpace(System.String str)
		{
			for (int i = str.Length; --i >= 0; )
				if (str[i] == ' ')
					return "\'" + str + "\'";
			return str;
		}
	}
}