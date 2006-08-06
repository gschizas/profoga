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
	public class ConsoleTextArea:System.Windows.Forms.TextBox
	{
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassRunnable' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassRunnable : IThreadRunnable
		{
			public AnonymousClassRunnable(System.IO.StreamReader br, ConsoleTextArea enclosingInstance)
			{
				InitBlock(br, enclosingInstance);
			}
			private void  InitBlock(System.IO.StreamReader br, ConsoleTextArea enclosingInstance)
			{
				this.br = br;
				this.enclosingInstance = enclosingInstance;
			}
			//UPGRADE_NOTE: Final variable br was copied into class AnonymousClassRunnable. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1023'"
			private System.IO.StreamReader br;
			private ConsoleTextArea enclosingInstance;
			public ConsoleTextArea Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			public virtual void  Run()
			{
				
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				try
				{
					System.String s;
					//UPGRADE_ISSUE: Interface 'javax.swing.text.Document' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextDocument'"
					//UPGRADE_TODO: Method 'javax.swing.text.JTextComponent.getDocument' was converted to 'System.Windows.Forms.TextBoxBase.Text' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingtextJTextComponentgetDocument'"
					Document doc = (System.String) Enclosing_Instance.Text;
					s = br.ReadLine();
					while (s != null)
					{
						bool caretAtEnd = false;
						//UPGRADE_ISSUE: Method 'javax.swing.text.Document.getLength' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextDocument'"
						caretAtEnd = (Enclosing_Instance.SelectionStart == doc.getLength());
						sb.Length = 0;
						Enclosing_Instance.AppendText(sb.Append(s).Append('\n').ToString());
						if (caretAtEnd)
						{
							//UPGRADE_ISSUE: Method 'javax.swing.text.Document.getLength' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingtextDocument'"
							Enclosing_Instance.SelectionStart = doc.getLength();
						}
						s = br.ReadLine();
					}
				}
				catch (System.IO.IOException e)
				{
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					SupportClass.OptionPaneSupport.ShowMessageDialog(null, GT._("Error reading from BufferedReader: {0}", new System.Object[]{e.Message}));
					System.Environment.Exit(1);
				}
			}
		}
		
		public ConsoleTextArea(System.IO.Stream[] inStreams)
		{
			this.Multiline = true;
			this.WordWrap = false;
			this.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			for (int i = 0; i < inStreams.Length; ++i)
			{
				startConsoleReaderThread(inStreams[i]);
			}
		} // ConsoleTextArea()
		
		
		public ConsoleTextArea()
		{
			this.Multiline = true;
			this.WordWrap = false;
			this.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			
			//UPGRADE_NOTE: Final was removed from the declaration of 'ls '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
			LoopedStreams ls = new LoopedStreams();
			
			//UPGRADE_ISSUE: Method 'java.lang.System.getProperty' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangSystem'"
			System.String redirect = System_Renamed.getProperty("JmolConsole");
			if (redirect == null || redirect.Equals("true"))
			{
				// Redirect System.out & System.err.
				
				//UPGRADE_ISSUE: Class hierarchy differences between 'java.io.PrintStream' and 'System.IO.StreamWriter' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
				System.IO.StreamWriter ps = new System.IO.StreamWriter(ls.OutputStream);
				//UPGRADE_TODO: Method 'java.lang.System.setOut' was converted to 'System.Console.SetOut' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangSystemsetOut_javaioPrintStream'"
				System.Console.SetOut(ps);
				//UPGRADE_TODO: Method 'java.lang.System.setErr' was converted to 'System.Console.SetError' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangSystemsetErr_javaioPrintStream'"
				System.Console.SetError(ps);
			}
			
			startConsoleReaderThread(ls.InputStream);
		} // ConsoleTextArea()
		
		
		private void  startConsoleReaderThread(System.IO.Stream inStream)
		{
			
			//UPGRADE_NOTE: Final was removed from the declaration of 'br '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
			//UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
			//UPGRADE_WARNING: At least one expression was used more than once in the target code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1181'"
			System.IO.StreamReader br = new System.IO.StreamReader(new System.IO.StreamReader(inStream, System.Text.Encoding.Default).BaseStream, new System.IO.StreamReader(inStream, System.Text.Encoding.Default).CurrentEncoding);
			new SupportClass.ThreadClass(new System.Threading.ThreadStart(new AnonymousClassRunnable(br, this).Run)).Start();
		}
	}
}