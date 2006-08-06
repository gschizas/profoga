/* $RCSfile$
* $Author: egonw $
* $Date: 2005-11-10 16:52:44 +0100 (jeu., 10 nov. 2005) $
* $Revision: 4255 $
*
* Copyright (C) 2004-2005  The Jmol Development Team
*
* Contact: jmol-developers@lists.sf.net, www.jmol.org
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
namespace org.jmol.applet
{
	
	class Console
	{
		private void  InitBlock()
		{
			input = new ShiftEnterTextArea(this);
			//UPGRADE_TODO: Method 'javax.swing.text.JTextComponent.getDocument' was converted to 'System.Windows.Forms.TextBoxBase.Text' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingtextJTextComponentgetDocument'"
			outputDocument = (System.String) output_Renamed_Field.Text;
		}
		virtual internal bool Visible
		{
			set
			{
				System.Console.Out.WriteLine("Console.setVisible(" + value + ")");
				//UPGRADE_TODO: Method 'java.awt.Component.setVisible' was converted to 'System.Windows.Forms.Control.Visible' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetVisible_boolean'"
				//UPGRADE_TODO: 'System.Windows.Forms.Application.Run' must be called to start a main form. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1135'"
				jf.Visible = value;
				//UPGRADE_TODO: Method 'javax.swing.JComponent.requestFocus' was converted to 'System.Windows.Forms.Control.Focus' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJComponentrequestFocus'"
				input.Focus();
			}
			
		}
		//UPGRADE_NOTE: Final was removed from the declaration of 'input '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_NOTE: The initialization of  'input' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		internal System.Windows.Forms.TextBox input;
		//UPGRADE_NOTE: Final was removed from the declaration of 'output '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_ISSUE: Class hierarchy differences between 'javax.swing.JEditorPane' and 'System.Windows.Forms.RichTextBox' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		internal System.Windows.Forms.RichTextBox output_Renamed_Field = new System.Windows.Forms.RichTextBox();
		//UPGRADE_NOTE: Final was removed from the declaration of 'outputDocument '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_ISSUE: Interface 'javax.swing.text.Document' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextDocument'"
		//UPGRADE_NOTE: The initialization of  'outputDocument' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		internal Document outputDocument;
		//UPGRADE_NOTE: Final was removed from the declaration of 'jf '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal System.Windows.Forms.Form jf = SupportClass.FormSupport.CreateForm("Jmol Console");
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'runButton '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal System.Windows.Forms.Button runButton = SupportClass.ButtonSupport.CreateStandardButton("Execute");
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'attributesCommand '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal System.Collections.Hashtable attributesCommand = new System.Collections.Hashtable();
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'viewer '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal JmolViewer viewer;
		//UPGRADE_NOTE: Final was removed from the declaration of 'jvm12 '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Jvm12 jvm12;
		
		internal Console(System.Windows.Forms.Control componentParent, JmolViewer viewer, Jvm12 jvm12)
		{
			InitBlock();
			this.viewer = viewer;
			this.jvm12 = jvm12;
			
			System.Console.Out.WriteLine("Console constructor");
			
			setupInput();
			setupOutput();
			
			//UPGRADE_TODO: Class 'javax.swing.JScrollPane' was converted to 'System.Windows.Forms.ScrollableControl' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
			//UPGRADE_TODO: Constructor 'javax.swing.JScrollPane.JScrollPane' was converted to 'System.Windows.Forms.ScrollableControl.ScrollableControl' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJScrollPaneJScrollPane_javaawtComponent'"
			System.Windows.Forms.ScrollableControl temp_scrollablecontrol;
			temp_scrollablecontrol = new System.Windows.Forms.ScrollableControl();
			temp_scrollablecontrol.AutoScroll = true;
			temp_scrollablecontrol.Controls.Add(input);
			System.Windows.Forms.ScrollableControl jscrollInput = temp_scrollablecontrol;
			//UPGRADE_ISSUE: Method 'javax.swing.JComponent.setMinimumSize' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJComponentsetMinimumSize_javaawtDimension'"
			jscrollInput.setMinimumSize(new System.Drawing.Size(2, 25));
			
			//UPGRADE_TODO: Class 'javax.swing.JScrollPane' was converted to 'System.Windows.Forms.ScrollableControl' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
			//UPGRADE_TODO: Constructor 'javax.swing.JScrollPane.JScrollPane' was converted to 'System.Windows.Forms.ScrollableControl.ScrollableControl' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJScrollPaneJScrollPane_javaawtComponent'"
			System.Windows.Forms.ScrollableControl temp_scrollablecontrol2;
			temp_scrollablecontrol2 = new System.Windows.Forms.ScrollableControl();
			temp_scrollablecontrol2.AutoScroll = true;
			temp_scrollablecontrol2.Controls.Add(output_Renamed_Field);
			System.Windows.Forms.ScrollableControl jscrollOutput = temp_scrollablecontrol2;
			//UPGRADE_ISSUE: Method 'javax.swing.JComponent.setMinimumSize' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJComponentsetMinimumSize_javaawtDimension'"
			jscrollOutput.setMinimumSize(new System.Drawing.Size(2, 25));
			//UPGRADE_TODO: Method 'javax.swing.JFrame.getContentPane' was converted to 'System.Windows.Forms.Form' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJFramegetContentPane'"
			System.Windows.Forms.Control c = ((System.Windows.Forms.ContainerControl) jf);
			
			//UPGRADE_TODO: Class 'javax.swing.JSplitPane' was converted to 'SupportClass.SplitterPanelSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
			SupportClass.SplitterPanelSupport jsp = new SupportClass.SplitterPanelSupport((int) System.Windows.Forms.Orientation.Vertical, jscrollOutput, jscrollInput);
			//UPGRADE_ISSUE: Method 'javax.swing.JSplitPane.setResizeWeight' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJSplitPanesetResizeWeight_double'"
			jsp.setResizeWeight(.9);
			
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtBorderLayout'"
			/*
			c.setLayout(new BorderLayout());*/
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			c.Controls.Add(jsp);
			jsp.Dock = System.Windows.Forms.DockStyle.Fill;
			jsp.BringToFront();
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			c.Controls.Add(runButton);
			runButton.Dock = System.Windows.Forms.DockStyle.Bottom;
			runButton.SendToBack();
			
			runButton.Click += new System.EventHandler(this.actionPerformed);
			SupportClass.CommandManager.CheckCommand(runButton);
			
			//UPGRADE_TODO: Method 'java.awt.Component.setSize' was converted to 'System.Windows.Forms.Control.Size' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetSize_int_int'"
			jf.Size = new System.Drawing.Size(400, 400);
			//UPGRADE_NOTE: Some methods of the 'java.awt.event.WindowListener' class are not used in the .NET Framework. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1308'"
			jf.Activated += new System.EventHandler(this.windowActivated);
			jf.Closed += new System.EventHandler(this.windowClosed);
			jf.Closing += new System.ComponentModel.CancelEventHandler(this.windowClosing);
			jf.Deactivate += new System.EventHandler(this.windowDeactivated);
			jf.Load += new System.EventHandler(this.windowOpened);
		}
		
		internal virtual void  setupInput()
		{
			input.WordWrap = true;
			input.WordWrap = true;
			
			//UPGRADE_ISSUE: Interface 'javax.swing.text.Keymap' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextKeymap'"
			//UPGRADE_ISSUE: Method 'javax.swing.text.JTextComponent.getKeymap' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextJTextComponentgetKeymap'"
			Keymap map = input.getKeymap();
			//    KeyStroke shiftCR = KeyStroke.getKeyStroke(KeyEvent.VK_ENTER,
			//                                               InputEvent.SHIFT_MASK);
			System.Windows.Forms.KeyEventArgs shiftA = new System.Windows.Forms.KeyEventArgs((System.Windows.Forms.Keys) ((int) System.Windows.Forms.Keys.A | (int) System.Windows.Forms.Keys.Shift));
			//UPGRADE_ISSUE: Method 'javax.swing.text.Keymap.removeKeyStrokeBinding' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextKeymap'"
			map.removeKeyStrokeBinding(shiftA);
		}
		
		internal virtual void  setupOutput()
		{
			output_Renamed_Field.ReadOnly = !false;
			//    output.setLineWrap(true);
			//    output.setWrapStyleWord(true);
			//UPGRADE_ISSUE: Method 'javax.swing.text.StyleConstants.setBold' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextStyleConstants'"
			StyleConstants.setBold(attributesCommand, true);
		}
		
		internal virtual void  output(System.String message)
		{
			output(message, null);
		}
		
		internal virtual void  output(System.String message, System.Collections.IDictionary att)
		{
			if (message[message.Length - 1] != '\n')
				message += "\n";
			try
			{
				//UPGRADE_ISSUE: Method 'javax.swing.text.Document.insertString' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextDocument'"
				//UPGRADE_ISSUE: Method 'javax.swing.text.Document.getLength' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextDocument'"
				outputDocument.insertString(outputDocument.getLength(), message, att);
			}
			//UPGRADE_TODO: Class 'javax.swing.text.BadLocationException' was converted to 'System.Exception' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
			catch (System.Exception ble)
			{
			}
			//UPGRADE_ISSUE: Method 'javax.swing.text.Document.getLength' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextDocument'"
			output_Renamed_Field.SelectionStart = outputDocument.getLength();
		}
		
		public virtual void  actionPerformed(System.Object event_sender, System.EventArgs e)
		{
			System.Object source = event_sender;
			if (source == runButton)
			{
				execute();
			}
		}
		
		internal virtual void  execute()
		{
			System.String strCommand = input.Text;
			//UPGRADE_TODO: Method 'javax.swing.text.JTextComponent.setText' was converted to 'System.Windows.Forms.TextBoxBase.Text' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingtextJTextComponentsetText_javalangString'"
			input.Text = null;
			output(strCommand, attributesCommand);
			System.String strErrorMessage = viewer.evalString(strCommand);
			if (strErrorMessage != null)
				output(strErrorMessage);
			//UPGRADE_TODO: Method 'javax.swing.JComponent.requestFocus' was converted to 'System.Windows.Forms.Control.Focus' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJComponentrequestFocus'"
			input.Focus();
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'ShiftEnterTextArea' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class ShiftEnterTextArea:System.Windows.Forms.TextBox
		{
			public ShiftEnterTextArea(Console enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(Console enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Console enclosingInstance;
			public Console Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			//UPGRADE_NOTE: The equivalent of method 'javax.swing.JComponent.processComponentKeyEvent' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
			public void  processComponentKeyEvent(System.Windows.Forms.KeyEventArgs ke)
			{
				//UPGRADE_ISSUE: Method 'java.awt.AWTEvent.getID' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtAWTEventgetID'"
				switch (ke.getID())
				{
					
					//UPGRADE_ISSUE: Field 'java.awt.event.KeyEvent.KEY_PRESSED' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventKeyEventKEY_PRESSED_f'"
					case KeyEvent.KEY_PRESSED: 
						//UPGRADE_ISSUE: Method 'java.awt.event.KeyEvent.getKeyCode' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventKeyEventgetKeyCode'"
						if (ke.getKeyCode() == (int) System.Windows.Forms.Keys.Enter && (System.Windows.Forms.Control.ModifierKeys == System.Windows.Forms.Keys.Shift))
						{
							Enclosing_Instance.execute();
							return ;
						}
						break;
					
					//UPGRADE_ISSUE: Field 'java.awt.event.KeyEvent.KEY_RELEASED' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventKeyEventKEY_RELEASED_f'"
					case KeyEvent.KEY_RELEASED: 
						//UPGRADE_ISSUE: Method 'java.awt.event.KeyEvent.getKeyCode' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventKeyEventgetKeyCode'"
						if (ke.getKeyCode() == (int) System.Windows.Forms.Keys.Enter && (System.Windows.Forms.Control.ModifierKeys == System.Windows.Forms.Keys.Shift))
							return ;
						break;
					}
				//UPGRADE_ISSUE: Method 'javax.swing.JComponent.processComponentKeyEvent' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJComponentprocessComponentKeyEvent_javaawteventKeyEvent'"
				base.processComponentKeyEvent(ke);
			}
		}
		
		////////////////////////////////////////////////////////////////
		// window listener stuff to close when the window closes
		////////////////////////////////////////////////////////////////
		
		public virtual void  windowActivated(System.Object event_sender, System.EventArgs we)
		{
		}
		
		public virtual void  windowClosed(System.Object event_sender, System.EventArgs we)
		{
			jvm12.console = null;
		}
		
		public virtual void  windowClosing(System.Object event_sender, System.ComponentModel.CancelEventArgs we)
		{
			we.Cancel = true;
			jvm12.console = null;
		}
		
		public virtual void  windowDeactivated(System.Object event_sender, System.EventArgs we)
		{
		}
		
		public virtual void  windowDeiconified(System.Object event_sender, System.EventArgs we)
		{
		}
		
		public virtual void  windowIconified(System.Object event_sender, System.EventArgs we)
		{
		}
		
		public virtual void  windowOpened(System.Object event_sender, System.EventArgs we)
		{
		}
	}
}