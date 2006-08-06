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
	public class HelpDialog:System.Windows.Forms.Form
	{
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassJScrollPane' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		//UPGRADE_TODO: Class 'javax.swing.JScrollPane' was converted to 'System.Windows.Forms.ScrollableControl' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
		[Serializable]
		private class AnonymousClassJScrollPane:System.Windows.Forms.ScrollableControl
		{
			public AnonymousClassJScrollPane(HelpDialog enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(HelpDialog enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private HelpDialog enclosingInstance;
			public HelpDialog Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			//UPGRADE_NOTE: The equivalent of method 'javax.swing.JComponent.getPreferredSize' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
			public System.Drawing.Size getPreferredSize()
			{
				return new System.Drawing.Size(300, 300);
			}
			
			//UPGRADE_NOTE: The equivalent of method 'javax.swing.JComponent.getAlignmentX' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
			public float getAlignmentX()
			{
				//UPGRADE_ISSUE: Field 'java.awt.Component.LEFT_ALIGNMENT' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtComponentLEFT_ALIGNMENT_f'"
				return LEFT_ALIGNMENT;
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassActionListener' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassActionListener
		{
			public AnonymousClassActionListener(HelpDialog enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(HelpDialog enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private HelpDialog enclosingInstance;
			public HelpDialog Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			public virtual void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				Enclosing_Instance.OKPressed();
			}
		}
		
		//UPGRADE_TODO: Class 'javax.swing.JEditorPane' was converted to 'System.Windows.Forms.RichTextBox' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
		internal System.Windows.Forms.RichTextBox html;
		
		public HelpDialog(System.Windows.Forms.Form fr):this(fr, null)
		{
		}
		
		/// <summary> If url is null, then the default help url is taken.</summary>
		/// <param name="fr">
		/// </param>
		/// <param name="url">
		/// </param>
		public HelpDialog(System.Windows.Forms.Form fr, System.Uri url):base()
		{
			//UPGRADE_TODO: Constructor 'javax.swing.JDialog.JDialog' was converted to 'SupportClass.DialogSupport.SetDialog' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJDialogJDialog_javaawtFrame_javalangString_boolean'"
			SupportClass.DialogSupport.SetDialog(this, fr, GT._("Jmol Help"));
			
			try
			{
				System.Uri helpURL = url;
				if (url == null)
				{
					//UPGRADE_ISSUE: Method 'java.lang.ClassLoader.getResource' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassLoader'"
					//UPGRADE_ISSUE: Method 'java.lang.Class.getClassLoader' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassgetClassLoader'"
					helpURL = this.GetType().getClassLoader().getResource(JmolResourceHandler.getStringX("Help.helpURL"));
				}
				if (helpURL != null)
				{
					//UPGRADE_TODO: Constructor 'javax.swing.JEditorPane.JEditorPane' was converted to 'System.Windows.Forms.RichTextBox' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJEditorPaneJEditorPane_javanetURL'"
					//UPGRADE_TODO: Class 'javax.swing.JEditorPane' was converted to 'System.Windows.Forms.RichTextBox' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
					html = new System.Windows.Forms.RichTextBox();
				}
				else
				{
					System.Windows.Forms.RichTextBox temp_richtextbox;
					//UPGRADE_TODO: Constructor 'javax.swing.JEditorPane.JEditorPane' was converted to 'System.Windows.Forms.RichTextBox.LoadFile' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJEditorPaneJEditorPane_javalangString_javalangString'"
					temp_richtextbox = new System.Windows.Forms.RichTextBox();
					temp_richtextbox.Text = GT._("Unable to find url \"{0}\".", new System.Object[]{JmolResourceHandler.getStringX("Help.helpURL")});;
					html = temp_richtextbox;
				}
				html.ReadOnly = !false;
				//UPGRADE_NOTE: Some methods of the 'javax.swing.event.HyperlinkListener' class are not used in the .NET Framework. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1308'"
				html.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.hyperlinkUpdate);
			}
			catch (System.UriFormatException e)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				System.Console.Out.WriteLine("Malformed URL: " + e);
			}
			catch (System.IO.IOException e)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				System.Console.Out.WriteLine("IOException: " + e);
			}
			//UPGRADE_TODO: Class 'javax.swing.JScrollPane' was converted to 'System.Windows.Forms.ScrollableControl' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
			System.Windows.Forms.ScrollableControl scroller = new AnonymousClassJScrollPane(this);
			//UPGRADE_ISSUE: Method 'javax.swing.JScrollPane.getViewport' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJScrollPanegetViewport'"
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			scroller.getViewport().Controls.Add(html);
			
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtBorderLayout'"
			new BorderLayout();
			//UPGRADE_TODO: Constructor 'javax.swing.JPanel.JPanel' was converted to 'System.Windows.Forms.Panel.Panel' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJPanelJPanel_javaawtLayoutManager'"
			System.Windows.Forms.Panel htmlWrapper = new System.Windows.Forms.Panel();
			//UPGRADE_ISSUE: Method 'javax.swing.JComponent.setAlignmentX' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJComponentsetAlignmentX_float'"
			//UPGRADE_ISSUE: Field 'java.awt.Component.LEFT_ALIGNMENT' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtComponentLEFT_ALIGNMENT_f'"
			htmlWrapper.setAlignmentX(LEFT_ALIGNMENT);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			htmlWrapper.Controls.Add(scroller);
			scroller.Dock = System.Windows.Forms.DockStyle.Fill;
			scroller.BringToFront();
			
			System.Windows.Forms.Panel buttonPanel = new System.Windows.Forms.Panel();
			//UPGRADE_TODO: Constructor 'java.awt.FlowLayout.FlowLayout' was converted to 'System.Object[]' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtFlowLayoutFlowLayout_int'"
			buttonPanel.Tag = new System.Object[]{(int) System.Drawing.ContentAlignment.TopRight, 5, 5};
			buttonPanel.Layout += new System.Windows.Forms.LayoutEventHandler(SupportClass.FlowLayoutResize);
			System.Windows.Forms.Button ok = SupportClass.ButtonSupport.CreateStandardButton(GT._("OK"));
			ok.Click += new System.EventHandler(new AnonymousClassActionListener(this).actionPerformed);
			SupportClass.CommandManager.CheckCommand(ok);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			buttonPanel.Controls.Add(ok);
			//UPGRADE_TODO: Method 'javax.swing.JRootPane.setDefaultButton' was converted to 'System.Windows.Forms.Form.AcceptButton' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJRootPanesetDefaultButton_javaxswingJButton'"
			ok.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.AcceptButton = ok;
			
			System.Windows.Forms.Panel container = new System.Windows.Forms.Panel();
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtBorderLayout'"
			/*
			container.setLayout(new BorderLayout());*/
			
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			container.Controls.Add(htmlWrapper);
			htmlWrapper.Dock = System.Windows.Forms.DockStyle.Fill;
			htmlWrapper.BringToFront();
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			container.Controls.Add(buttonPanel);
			buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			buttonPanel.SendToBack();
			
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			((System.Windows.Forms.ContainerControl) this).Controls.Add(container);
			//UPGRADE_ISSUE: Method 'java.awt.Window.pack' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtWindowpack'"
			pack();
			centerDialog();
		}
		
		public virtual void  hyperlinkUpdate(System.Object event_sender, System.Windows.Forms.LinkClickedEventArgs e)
		{
			//UPGRADE_ISSUE: Method 'javax.swing.event.HyperlinkEvent.getEventType' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingeventHyperlinkEventgetEventType'"
			//UPGRADE_ISSUE: Field 'javax.swing.event.HyperlinkEvent.EventType.ACTIVATED' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingeventHyperlinkEventEventType'"
			if (e.getEventType() == HyperlinkEvent.EventType.ACTIVATED)
			{
				linkActivated(new System.Uri(e.LinkText));
			}
		}
		
		/// <summary> Follows the reference in an
		/// link.  The given url is the requested reference.
		/// By default this calls <a href="#setPage">setPage</a>,
		/// and if an exception is thrown the original previous
		/// document is restored and a beep sounded.  If an
		/// attempt was made to follow a link, but it represented
		/// a malformed url, this method will be called with a
		/// null argument.
		/// 
		/// </summary>
		/// <param name="u">the URL to follow
		/// </param>
		protected internal virtual void  linkActivated(System.Uri u)
		{
			System.Windows.Forms.Cursor temp_Cursor;
			temp_Cursor = new System.Windows.Forms.Cursor(new System.IntPtr(1));
			temp_Cursor = html.Cursor;
			System.Windows.Forms.Cursor c = temp_Cursor;
			//UPGRADE_ISSUE: Member 'java.awt.Cursor.getPredefinedCursor' was converted to 'System.Windows.Forms.Cursor' which cannot be assigned to an int. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1086'"
			//UPGRADE_ISSUE: Member 'java.awt.Cursor.WAIT_CURSOR' was converted to 'System.Windows.Forms.Cursors.WaitCursor' which cannot be assigned to an int. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1086'"
			System.Windows.Forms.Cursor waitCursor = System.Windows.Forms.Cursors.WaitCursor;
			html.Cursor = waitCursor;
			//UPGRADE_ISSUE: Method 'javax.swing.SwingUtilities.invokeLater' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingSwingUtilities'"
			SwingUtilities.invokeLater(new PageLoader(this, u, c));
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'PageLoader' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		/// <summary> temporary class that loads synchronously (although later than
		/// the request so that a cursor change can be done).
		/// </summary>
		internal class PageLoader : IThreadRunnable
		{
			private void  InitBlock(HelpDialog enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private HelpDialog enclosingInstance;
			public HelpDialog Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			internal PageLoader(HelpDialog enclosingInstance, System.Uri u, System.Windows.Forms.Cursor c)
			{
				InitBlock(enclosingInstance);
				url = u;
				cursor = c;
			}
			
			public virtual void  Run()
			{
				
				if (url == null)
				{
					
					// restore the original cursor
					Enclosing_Instance.html.Cursor = cursor;
					
					// remove this hack when automatic validation is
					// activated.
					System.Windows.Forms.Control parent = Enclosing_Instance.html.Parent;
					//UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
					parent.Refresh();
				}
				else
				{
					//UPGRADE_ISSUE: Interface 'javax.swing.text.Document' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextDocument'"
					//UPGRADE_TODO: Method 'javax.swing.text.JTextComponent.getDocument' was converted to 'System.Windows.Forms.TextBoxBase.Text' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingtextJTextComponentgetDocument'"
					Document doc = (System.String) Enclosing_Instance.html.Text;
					try
					{
						//UPGRADE_ISSUE: Method 'javax.swing.JEditorPane.setPage' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJEditorPanesetPage_javanetURL'"
						Enclosing_Instance.html.setPage(url);
					}
					catch (System.IO.IOException ioe)
					{
						//UPGRADE_ISSUE: Method 'javax.swing.text.JTextComponent.setDocument' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextJTextComponentsetDocument_javaxswingtextDocument'"
						Enclosing_Instance.html.setDocument(doc);
						//UPGRADE_ISSUE: Method 'java.awt.Window.getToolkit' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtWindowgetToolkit'"
						Enclosing_Instance.getToolkit();
						//UPGRADE_TODO: Method 'java.awt.Toolkit.beep' was converted to 'System.Console.Write' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
						System.Console.Write("\a");
					}
					finally
					{
						
						// schedule the cursor to revert after the paint
						// has happended.
						url = null;
						//UPGRADE_ISSUE: Method 'javax.swing.SwingUtilities.invokeLater' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingSwingUtilities'"
						SwingUtilities.invokeLater(this);
					}
				}
			}
			
			internal System.Uri url;
			internal System.Windows.Forms.Cursor cursor;
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
		
		public virtual void  OKPressed()
		{
			//UPGRADE_TODO: Method 'java.awt.Component.setVisible' was converted to 'System.Windows.Forms.Control.Visible' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetVisible_boolean'"
			//UPGRADE_TODO: 'System.Windows.Forms.Application.Run' must be called to start a main form. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1135'"
			this.Visible = false;
		}
	}
}