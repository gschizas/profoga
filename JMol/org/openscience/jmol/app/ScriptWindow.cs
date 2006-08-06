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
	public sealed class ScriptWindow:System.Windows.Forms.Form, EnterListener
	{
		
		private ConsoleTextPane console;
		private System.Windows.Forms.Button closeButton;
		private System.Windows.Forms.Button runButton;
		private System.Windows.Forms.Button haltButton;
		private System.Windows.Forms.Button clearButton;
		private System.Windows.Forms.Button helpButton;
		internal JmolViewer viewer;
		
		public ScriptWindow(JmolViewer viewer, System.Windows.Forms.Form frame):base()
		{
			//UPGRADE_TODO: Constructor 'javax.swing.JDialog.JDialog' was converted to 'SupportClass.DialogSupport.SetDialog' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJDialogJDialog_javaawtFrame_javalangString_boolean'"
			SupportClass.DialogSupport.SetDialog(this, frame, GT._("Rasmol Scripts"));
			this.viewer = viewer;
			layoutWindow(((System.Windows.Forms.ContainerControl) this));
			//UPGRADE_TODO: Method 'java.awt.Component.setSize' was converted to 'System.Windows.Forms.Control.Size' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetSize_int_int'"
			Size = new System.Drawing.Size(500, 400);
			//UPGRADE_TODO: Method 'javax.swing.JDialog.setLocationRelativeTo' was converted to 'System.Windows.Forms.FormStartPosition.CenterParent' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJDialogsetLocationRelativeTo_javaawtComponent'"
			StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		}
		
		internal void  layoutWindow(System.Windows.Forms.Control container)
		{
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtBorderLayout'"
			/*
			container.setLayout(new BorderLayout());*/
			
			console = new ConsoleTextPane(this);
			
			
			console.setPrompt();
			//UPGRADE_TODO: Constructor 'javax.swing.JScrollPane.JScrollPane' was converted to 'System.Windows.Forms.ScrollableControl.ScrollableControl' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJScrollPaneJScrollPane_javaawtComponent'"
			System.Windows.Forms.ScrollableControl temp_scrollablecontrol2;
			temp_scrollablecontrol2 = new System.Windows.Forms.ScrollableControl();
			temp_scrollablecontrol2.AutoScroll = true;
			temp_scrollablecontrol2.Controls.Add(console);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			System.Windows.Forms.Control temp_Control;
			temp_Control = temp_scrollablecontrol2;
			container.Controls.Add(temp_Control);
			temp_Control.Dock = System.Windows.Forms.DockStyle.Fill;
			temp_Control.BringToFront();
			
			System.Windows.Forms.Panel buttonPanel = new System.Windows.Forms.Panel();
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			container.Controls.Add(buttonPanel);
			buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			buttonPanel.SendToBack();
			
			closeButton = SupportClass.ButtonSupport.CreateStandardButton(GT._("Close"));
			closeButton.Click += new System.EventHandler(this.actionPerformed);
			SupportClass.CommandManager.CheckCommand(closeButton);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			buttonPanel.Controls.Add(closeButton);
			
			runButton = SupportClass.ButtonSupport.CreateStandardButton(GT._("Run"));
			runButton.Click += new System.EventHandler(this.actionPerformed);
			SupportClass.CommandManager.CheckCommand(runButton);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			buttonPanel.Controls.Add(runButton);
			
			haltButton = SupportClass.ButtonSupport.CreateStandardButton(GT._("Halt"));
			haltButton.Click += new System.EventHandler(this.actionPerformed);
			SupportClass.CommandManager.CheckCommand(haltButton);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			buttonPanel.Controls.Add(haltButton);
			haltButton.Enabled = false;
			
			clearButton = SupportClass.ButtonSupport.CreateStandardButton(GT._("Clear"));
			clearButton.Click += new System.EventHandler(this.actionPerformed);
			SupportClass.CommandManager.CheckCommand(clearButton);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			buttonPanel.Controls.Add(clearButton);
			
			helpButton = SupportClass.ButtonSupport.CreateStandardButton(GT._("Help"));
			helpButton.Click += new System.EventHandler(this.actionPerformed);
			SupportClass.CommandManager.CheckCommand(helpButton);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			buttonPanel.Controls.Add(helpButton);
		}
		
		public void  scriptEcho(System.String strEcho)
		{
			if (strEcho != null)
			{
				console.outputEcho(strEcho);
			}
		}
		
		public void  scriptStatus(System.String strStatus)
		{
			if (strStatus != null)
			{
				console.outputStatus(strStatus);
			}
		}
		
		public void  notifyScriptTermination(System.String strMsg, int msWalltime)
		{
			if (strMsg != null)
			{
				console.outputError(strMsg);
			}
			runButton.Enabled = true;
			haltButton.Enabled = false;
		}
		
		public void  enterPressed()
		{
			//UPGRADE_ISSUE: Method 'javax.swing.AbstractButton.doClick' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractButtondoClick_int'"
			runButton.doClick(100);
			//    executeCommand();
		}
		
		internal void  executeCommand()
		{
			System.String strCommand = console.CommandString.Trim();
			console.appendNewline();
			console.setPrompt();
			if (strCommand.Length > 0)
			{
				System.String strErrorMessage = viewer.evalString(strCommand);
				if (strErrorMessage != null)
					console.outputError(strErrorMessage);
				else
				{
					runButton.Enabled = false;
					haltButton.Enabled = true;
				}
			}
			console.Focus();
		}
		
		public void  actionPerformed(System.Object event_sender, System.EventArgs e)
		{
			System.Object source = event_sender;
			if (source == closeButton)
			{
				Hide();
			}
			else if (source == runButton)
			{
				executeCommand();
			}
			else if (source == clearButton)
			{
				System.Console.Out.WriteLine("clearing content of script window.");
				console.clearContent();
			}
			else if (source == haltButton)
			{
				System.Console.Out.WriteLine("calling viewer.haltScriptExecution();");
				viewer.haltScriptExecution();
			}
			else if (source == helpButton)
			{
				//UPGRADE_ISSUE: Method 'java.lang.ClassLoader.getResource' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassLoader'"
				//UPGRADE_ISSUE: Method 'java.lang.Class.getClassLoader' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassgetClassLoader'"
				System.Uri url = this.GetType().getClassLoader().getResource("org/openscience/jmol/Data/guide/ch04.html");
				HelpDialog hd = new HelpDialog(null, url);
				//UPGRADE_TODO: Method 'java.awt.Dialog.show' was converted to 'System.Windows.Forms.Form.ShowDialog' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtDialogshow'"
				hd.ShowDialog();
			}
			console.Focus(); // always grab the focus (e.g., after clear)
		}
	}
	
	//UPGRADE_ISSUE: Class hierarchy differences between 'javax.swing.JEditorPane' and 'System.Windows.Forms.RichTextBox' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
	[Serializable]
	class ConsoleTextPane:System.Windows.Forms.RichTextBox
	{
		virtual public System.String CommandString
		{
			get
			{
				System.String cmd = consoleDoc.CommandString;
				commandHistory.addCommand(cmd);
				return cmd;
			}
			
		}
		
		private CommandHistory commandHistory = new CommandHistory(20);
		
		internal ConsoleDocument consoleDoc;
		internal EnterListener enterListener;
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'javax.swing.JEditorPane' and 'System.Windows.Forms.RichTextBox' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		internal ConsoleTextPane(EnterListener enterListener):base()
		{
			//UPGRADE_TODO: Method 'javax.swing.text.JTextComponent.getDocument' was converted to 'System.Windows.Forms.TextBoxBase.Text' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingtextJTextComponentgetDocument'"
			consoleDoc = (ConsoleDocument) Text;
			consoleDoc.ConsoleTextPane = this;
			this.enterListener = enterListener;
		}
		
		public virtual void  setPrompt()
		{
			consoleDoc.setPrompt();
		}
		
		public virtual void  appendNewline()
		{
			consoleDoc.appendNewline();
		}
		
		public virtual void  outputError(System.String strError)
		{
			consoleDoc.outputError(strError);
		}
		
		public virtual void  outputErrorForeground(System.String strError)
		{
			consoleDoc.outputErrorForeground(strError);
		}
		
		public virtual void  outputEcho(System.String strEcho)
		{
			consoleDoc.outputEcho(strEcho);
		}
		
		public virtual void  outputStatus(System.String strStatus)
		{
			consoleDoc.outputStatus(strStatus);
		}
		
		public virtual void  enterPressed()
		{
			if (enterListener != null)
				enterListener.enterPressed();
		}
		
		public virtual void  clearContent()
		{
			consoleDoc.clearContent();
		}
		
		
		
		/* (non-Javadoc)
		* @see java.awt.Component#processKeyEvent(java.awt.event.KeyEvent)
		*/
		
		/// <summary> Custom key event processing for command history implementation.
		/// 
		/// Captures key up and key down strokes to call command history
		/// and redefines the same events with control down to allow
		/// caret vertical shift.
		/// 
		/// </summary>
		/// <seealso cref="java.awt.Component.processKeyEvent(java.awt.event.KeyEvent)">
		/// </seealso>
		//UPGRADE_NOTE: The equivalent of method 'javax.swing.JEditorPane.processKeyEvent' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		protected internal void  processKeyEvent(System.Windows.Forms.KeyEventArgs ke)
		{
			// Id Control key is down, captures events does command
			// history recall and inhibits caret vertical shift.
			//UPGRADE_ISSUE: Method 'java.awt.event.KeyEvent.getKeyCode' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventKeyEventgetKeyCode'"
			//UPGRADE_ISSUE: Method 'java.awt.AWTEvent.getID' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtAWTEventgetID'"
			//UPGRADE_ISSUE: Field 'java.awt.event.KeyEvent.KEY_PRESSED' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventKeyEventKEY_PRESSED_f'"
			if (ke.getKeyCode() == (int) System.Windows.Forms.Keys.Up && ke.getID() == KeyEvent.KEY_PRESSED && !(System.Windows.Forms.Control.ModifierKeys == System.Windows.Forms.Keys.Control))
			{
				recallCommand(true);
			}
			else
			{
				//UPGRADE_ISSUE: Method 'java.awt.event.KeyEvent.getKeyCode' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventKeyEventgetKeyCode'"
				//UPGRADE_ISSUE: Method 'java.awt.AWTEvent.getID' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtAWTEventgetID'"
				//UPGRADE_ISSUE: Field 'java.awt.event.KeyEvent.KEY_PRESSED' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventKeyEventKEY_PRESSED_f'"
				if (ke.getKeyCode() == (int) System.Windows.Forms.Keys.Down && ke.getID() == KeyEvent.KEY_PRESSED && !(System.Windows.Forms.Control.ModifierKeys == System.Windows.Forms.Keys.Control))
				{
					recallCommand(false);
				}
				// If Control key is down, redefines the event as if it 
				// where a key up or key down stroke without modifiers.  
				// This allows to move the caret up and down
				// with no command history recall.
				else
				{
					//UPGRADE_ISSUE: Method 'java.awt.event.KeyEvent.getKeyCode' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventKeyEventgetKeyCode'"
					//UPGRADE_ISSUE: Method 'java.awt.AWTEvent.getID' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtAWTEventgetID'"
					//UPGRADE_ISSUE: Field 'java.awt.event.KeyEvent.KEY_PRESSED' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventKeyEventKEY_PRESSED_f'"
					if ((ke.getKeyCode() == (int) System.Windows.Forms.Keys.Down || ke.getKeyCode() == (int) System.Windows.Forms.Keys.Up) && ke.getID() == KeyEvent.KEY_PRESSED && (System.Windows.Forms.Control.ModifierKeys == System.Windows.Forms.Keys.Control))
					{
						//UPGRADE_ISSUE: Method 'javax.swing.JEditorPane.processKeyEvent' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJEditorPaneprocessKeyEvent_javaawteventKeyEvent'"
						//UPGRADE_TODO: The method 'java.awt.event.KeyEvent.getSource' needs to be in a event handling method in order to be properly converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1171'"
						//UPGRADE_ISSUE: Method 'java.awt.AWTEvent.getID' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtAWTEventgetID'"
						//UPGRADE_ISSUE: Method 'java.awt.event.InputEvent.getWhen' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventInputEvent'"
						//UPGRADE_ISSUE: Method 'java.awt.event.KeyEvent.getKeyCode' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventKeyEventgetKeyCode'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.awt.event.KeyEvent.getKeyChar' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						//UPGRADE_ISSUE: Method 'java.awt.event.KeyEvent.getKeyChar' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventKeyEventgetKeyChar'"
						base.processKeyEvent(new KeyEvent((System.Windows.Forms.Control) ke.getSource(), ke.getID(), ke.getWhen(), 0, ke.getKeyCode(), ke.getKeyChar(), ke.getKeyLocation()));
					}
					// Standard processing for other events.
					else
					{
						//UPGRADE_ISSUE: Method 'javax.swing.JEditorPane.processKeyEvent' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJEditorPaneprocessKeyEvent_javaawteventKeyEvent'"
						base.processKeyEvent(ke);
					}
				}
			}
		}
		
		/// <summary> Recall command histoy.
		/// 
		/// </summary>
		/// <param name="up">- history up or down
		/// </param>
		private void  recallCommand(bool up)
		{
			System.String cmd = up?commandHistory.CommandUp:commandHistory.CommandDown;
			
			try
			{
				consoleDoc.replaceCommand(cmd);
			}
			//UPGRADE_TODO: Class 'javax.swing.text.BadLocationException' was converted to 'System.Exception' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
			catch (System.Exception e)
			{
				SupportClass.WriteStackTrace(e, Console.Error);
			}
		}
	}
	
	//UPGRADE_ISSUE: Class 'javax.swing.text.DefaultStyledDocument' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextDefaultStyledDocument'"
	[Serializable]
	class ConsoleDocument:DefaultStyledDocument
	{
		virtual internal ConsoleTextPane ConsoleTextPane
		{
			set
			{
				this.consoleTextPane = value;
			}
			
		}
		virtual internal System.String CommandString
		{
			get
			{
				System.String strCommand = "";
				try
				{
					//UPGRADE_ISSUE: Method 'javax.swing.text.Position.getOffset' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextPosition'"
					int cmdStart = positionAfterPrompt.getOffset();
					// skip unnecessary leading spaces in the command.
					//UPGRADE_ISSUE: Method 'javax.swing.text.AbstractDocument.getText' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextAbstractDocument'"
					//UPGRADE_ISSUE: Method 'javax.swing.text.AbstractDocument.getLength' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextAbstractDocument'"
					strCommand = getText(cmdStart, getLength() - cmdStart).Trim();
				}
				//UPGRADE_TODO: Class 'javax.swing.text.BadLocationException' was converted to 'System.Exception' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
				catch (System.Exception e)
				{
				}
				return strCommand;
			}
			
		}
		
		internal ConsoleTextPane consoleTextPane;
		
		internal System.Collections.Hashtable attError;
		internal System.Collections.Hashtable attEcho;
		internal System.Collections.Hashtable attPrompt;
		internal System.Collections.Hashtable attUserInput;
		internal System.Collections.Hashtable attStatus;
		
		//UPGRADE_ISSUE: Constructor 'javax.swing.text.DefaultStyledDocument.DefaultStyledDocument' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextDefaultStyledDocument'"
		internal ConsoleDocument():base()
		{
			
			attError = new System.Collections.Hashtable();
			//UPGRADE_ISSUE: Method 'javax.swing.text.StyleConstants.setForeground' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextStyleConstants'"
			StyleConstants.setForeground(attError, System.Drawing.Color.Red);
			
			attPrompt = new System.Collections.Hashtable();
			//UPGRADE_ISSUE: Method 'javax.swing.text.StyleConstants.setForeground' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextStyleConstants'"
			StyleConstants.setForeground(attPrompt, System.Drawing.Color.Magenta);
			
			attUserInput = new System.Collections.Hashtable();
			//UPGRADE_ISSUE: Method 'javax.swing.text.StyleConstants.setForeground' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextStyleConstants'"
			StyleConstants.setForeground(attUserInput, System.Drawing.Color.Black);
			
			attEcho = new System.Collections.Hashtable();
			//UPGRADE_ISSUE: Method 'javax.swing.text.StyleConstants.setForeground' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextStyleConstants'"
			StyleConstants.setForeground(attEcho, System.Drawing.Color.Blue);
			//UPGRADE_ISSUE: Method 'javax.swing.text.StyleConstants.setBold' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextStyleConstants'"
			StyleConstants.setBold(attEcho, true);
			
			attStatus = new System.Collections.Hashtable();
			//UPGRADE_ISSUE: Method 'javax.swing.text.StyleConstants.setForeground' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextStyleConstants'"
			StyleConstants.setForeground(attStatus, System.Drawing.Color.Black);
			//UPGRADE_ISSUE: Method 'javax.swing.text.StyleConstants.setItalic' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextStyleConstants'"
			StyleConstants.setItalic(attStatus, true);
		}
		
		//UPGRADE_ISSUE: Interface 'javax.swing.text.Position' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextPosition'"
		internal Position positionBeforePrompt; // starts at 0, so first time isn't tracked (at least on Mac OS X)
		//UPGRADE_ISSUE: Interface 'javax.swing.text.Position' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextPosition'"
		internal Position positionAfterPrompt; // immediately after $, so this will track
		internal int offsetAfterPrompt; // only still needed for the insertString override and replaceCommand
		
		/// <summary> Removes all content of the script window, and add a new prompt.</summary>
		internal virtual void  clearContent()
		{
			try
			{
				//UPGRADE_ISSUE: Method 'javax.swing.text.AbstractDocument.remove' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextAbstractDocument'"
				//UPGRADE_ISSUE: Method 'javax.swing.text.AbstractDocument.getLength' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextAbstractDocument'"
				base.remove(0, getLength());
			}
			//UPGRADE_TODO: Class 'javax.swing.text.BadLocationException' was converted to 'System.Exception' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
			catch (System.Exception exception)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				System.Console.Out.WriteLine("Could not clear script window content: " + exception.Message);
			}
			setPrompt();
		}
		
		internal virtual void  setPrompt()
		{
			try
			{
				//UPGRADE_ISSUE: Method 'javax.swing.text.AbstractDocument.insertString' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextAbstractDocument'"
				//UPGRADE_ISSUE: Method 'javax.swing.text.AbstractDocument.getLength' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextAbstractDocument'"
				base.insertString(getLength(), "$ ", attPrompt);
				//UPGRADE_ISSUE: Method 'javax.swing.text.AbstractDocument.getLength' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextAbstractDocument'"
				offsetAfterPrompt = getLength();
				//UPGRADE_ISSUE: Method 'javax.swing.text.AbstractDocument.createPosition' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextAbstractDocument'"
				positionBeforePrompt = createPosition(offsetAfterPrompt - 2);
				// after prompt should be immediately after $ otherwise tracks the end
				// of the line (and no command will be found) at least on Mac OS X it did.
				//UPGRADE_ISSUE: Method 'javax.swing.text.AbstractDocument.createPosition' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextAbstractDocument'"
				positionAfterPrompt = createPosition(offsetAfterPrompt - 1);
				consoleTextPane.SelectionStart = offsetAfterPrompt;
			}
			//UPGRADE_TODO: Class 'javax.swing.text.BadLocationException' was converted to 'System.Exception' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
			catch (System.Exception e)
			{
			}
		}
		
		// it looks like the positionBeforePrompt does not track when it started out as 0
		// and a insertString at location 0 occurs. It may be better to track the
		// position after the prompt in stead
		internal virtual void  outputBeforePrompt(System.String str, System.Collections.Hashtable attribute)
		{
			try
			{
				//UPGRADE_ISSUE: Interface 'javax.swing.text.Position' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextPosition'"
				//UPGRADE_ISSUE: Method 'javax.swing.text.AbstractDocument.createPosition' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextAbstractDocument'"
				Position caretPosition = createPosition(consoleTextPane.SelectionStart);
				//UPGRADE_ISSUE: Method 'javax.swing.text.AbstractDocument.insertString' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextAbstractDocument'"
				//UPGRADE_ISSUE: Method 'javax.swing.text.Position.getOffset' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextPosition'"
				base.insertString(positionBeforePrompt.getOffset(), str + "\n", attribute);
				// keep the offsetAfterPrompt in sync
				//UPGRADE_ISSUE: Method 'javax.swing.text.Position.getOffset' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextPosition'"
				offsetAfterPrompt = positionBeforePrompt.getOffset() + 2;
				//UPGRADE_ISSUE: Method 'javax.swing.text.Position.getOffset' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextPosition'"
				consoleTextPane.SelectionStart = caretPosition.getOffset();
			}
			//UPGRADE_TODO: Class 'javax.swing.text.BadLocationException' was converted to 'System.Exception' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
			catch (System.Exception e)
			{
			}
		}
		
		internal virtual void  outputError(System.String strError)
		{
			outputBeforePrompt(strError, attError);
		}
		
		internal virtual void  outputErrorForeground(System.String strError)
		{
			try
			{
				//UPGRADE_ISSUE: Method 'javax.swing.text.AbstractDocument.insertString' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextAbstractDocument'"
				//UPGRADE_ISSUE: Method 'javax.swing.text.AbstractDocument.getLength' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextAbstractDocument'"
				base.insertString(getLength(), strError + "\n", attError);
				//UPGRADE_ISSUE: Method 'javax.swing.text.AbstractDocument.getLength' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextAbstractDocument'"
				consoleTextPane.SelectionStart = getLength();
			}
			//UPGRADE_TODO: Class 'javax.swing.text.BadLocationException' was converted to 'System.Exception' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
			catch (System.Exception e)
			{
			}
		}
		
		internal virtual void  outputEcho(System.String strEcho)
		{
			outputBeforePrompt(strEcho, attEcho);
		}
		
		internal virtual void  outputStatus(System.String strStatus)
		{
			outputBeforePrompt(strStatus, attStatus);
		}
		
		internal virtual void  appendNewline()
		{
			try
			{
				//UPGRADE_ISSUE: Method 'javax.swing.text.AbstractDocument.insertString' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextAbstractDocument'"
				//UPGRADE_ISSUE: Method 'javax.swing.text.AbstractDocument.getLength' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextAbstractDocument'"
				base.insertString(getLength(), "\n", attUserInput);
				//UPGRADE_ISSUE: Method 'javax.swing.text.AbstractDocument.getLength' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextAbstractDocument'"
				consoleTextPane.SelectionStart = getLength();
			}
			//UPGRADE_TODO: Class 'javax.swing.text.BadLocationException' was converted to 'System.Exception' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
			catch (System.Exception e)
			{
			}
		}
		
		// override the insertString to make sure everything typed ends up at the end
		// or in the 'command line' using the proper font, and the newline is processed.
		public virtual void  insertString(int offs, System.String str, System.Collections.IDictionary a)
		{
			//    System.out.println("insertString("+offs+","+str+",attr)");
			int ichNewline = str.IndexOf('\n');
			if (ichNewline > 0)
				str = str.Substring(0, (ichNewline) - (0));
			if (ichNewline != 0)
			{
				if (offs < offsetAfterPrompt)
				{
					//UPGRADE_ISSUE: Method 'javax.swing.text.AbstractDocument.getLength' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextAbstractDocument'"
					offs = getLength();
				}
				//UPGRADE_ISSUE: Method 'javax.swing.text.AbstractDocument.insertString' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextAbstractDocument'"
				base.insertString(offs, str, attUserInput);
				consoleTextPane.SelectionStart = offs + str.Length;
			}
			if (ichNewline >= 0)
			{
				consoleTextPane.enterPressed();
			}
		}
		
		public virtual void  remove(int offs, int len)
		{
			//    System.out.println("remove("+offs+","+len+")");
			if (offs < offsetAfterPrompt)
			{
				len -= (offsetAfterPrompt - offs);
				if (len <= 0)
					return ;
				offs = offsetAfterPrompt;
			}
			//UPGRADE_ISSUE: Method 'javax.swing.text.AbstractDocument.remove' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextAbstractDocument'"
			base.remove(offs, len);
			//    consoleTextPane.setCaretPosition(offs);
		}
		
		public virtual void  replace(int offs, int length, System.String str, System.Collections.IDictionary attrs)
		{
			//    System.out.println("replace("+offs+","+length+","+str+",attr)");
			if (offs < offsetAfterPrompt)
			{
				if (offs + length < offsetAfterPrompt)
				{
					//UPGRADE_ISSUE: Method 'javax.swing.text.AbstractDocument.getLength' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextAbstractDocument'"
					offs = getLength();
					length = 0;
				}
				else
				{
					length -= (offsetAfterPrompt - offs);
					offs = offsetAfterPrompt;
				}
			}
			base.replace(offs, length, str, attUserInput);
			//    consoleTextPane.setCaretPosition(offs + str.length());
		}
		
		/// <summary> Replaces current command on script.
		/// 
		/// </summary>
		/// <param name="newCommand">new command value
		/// 
		/// </param>
		/// <throws>  BadLocationException </throws>
		internal virtual void  replaceCommand(System.String newCommand)
		{
			//UPGRADE_ISSUE: Method 'javax.swing.text.AbstractDocument.getLength' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextAbstractDocument'"
			replace(offsetAfterPrompt, getLength() - offsetAfterPrompt, newCommand, attUserInput);
		}
	}
	
	internal interface EnterListener
	{
		void  enterPressed();
	}
}