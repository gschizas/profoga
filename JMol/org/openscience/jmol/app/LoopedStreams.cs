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
	
	public class LoopedStreams
	{
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassByteArrayOutputStream' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		internal class AnonymousClassByteArrayOutputStream:System.IO.MemoryStream
		{
			public AnonymousClassByteArrayOutputStream(LoopedStreams enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(LoopedStreams enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private LoopedStreams enclosingInstance;
			public LoopedStreams Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			public override void  Close()
			{
				
				Enclosing_Instance.keepRunning = false;
				try
				{
					base.Close();
					Enclosing_Instance.pipedOS.Close();
				}
				catch (System.IO.IOException e)
				{
					
					// Do something to log the error -- perhaps invoke a 
					// Runnable to log the error. For now we simply exit.
					System.Environment.Exit(1);
				}
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassPipedInputStream' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		internal class AnonymousClassPipedInputStream:System.IO.StreamReader
		{
			public AnonymousClassPipedInputStream(LoopedStreams enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(LoopedStreams enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private LoopedStreams enclosingInstance;
			public LoopedStreams Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			public override void  Close()
			{
				
				Enclosing_Instance.keepRunning = false;
				try
				{
					base.Close();
				}
				catch (System.IO.IOException e)
				{
					
					// Do something to log the error -- perhaps invoke a 
					// Runnable to log the error. For now we simply exit.
					System.Environment.Exit(1);
				}
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassRunnable' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassRunnable : IThreadRunnable
		{
			public AnonymousClassRunnable(LoopedStreams enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(LoopedStreams enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private LoopedStreams enclosingInstance;
			public LoopedStreams Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			public virtual void  Run()
			{
				
				while (Enclosing_Instance.keepRunning)
				{
					
					// Check for bytes in the stream.
					if (Enclosing_Instance.byteArrayOS.Length > 0)
					{
						sbyte[] buffer = null;
						lock (Enclosing_Instance.byteArrayOS)
						{
							buffer = SupportClass.ToSByteArray(Enclosing_Instance.byteArrayOS.ToArray());
							//UPGRADE_ISSUE: Method 'java.io.ByteArrayOutputStream.reset' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaioByteArrayOutputStreamreset'"
							Enclosing_Instance.byteArrayOS.reset(); // Clear the buffer.
						}
						try
						{
							
							// Send the extracted data to
							// the PipedOutputStream.
							//UPGRADE_ISSUE: Method 'java.io.PipedOutputStream.write' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaioPipedOutputStreamwrite_byte[]_int_int'"
							Enclosing_Instance.pipedOS.write(buffer, 0, buffer.Length);
						}
						catch (System.IO.IOException e)
						{
							
							// Do something to log the error -- perhaps 
							// invoke a Runnable. For now we simply exit.
							System.Environment.Exit(1);
						}
					}
					else
					{
						// No data available, go to sleep.
						try
						{
							
							// Check the ByteArrayOutputStream every
							// 1 second for new data.
							//UPGRADE_TODO: Method 'java.lang.Thread.sleep' was converted to 'System.Threading.Thread.Sleep' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangThreadsleep_long'"
							System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64) 10000 * 1000));
						}
						catch (System.Threading.ThreadInterruptedException e)
						{
						}
					}
				}
			}
		}
		private void  InitBlock()
		{
			byteArrayOS = new AnonymousClassByteArrayOutputStream(this);
			pipedIS = new AnonymousClassPipedInputStream(this);
		}
		virtual public System.IO.Stream InputStream
		{
			get
			{
				return pipedIS;
			}
			// getInputStream()
			
		}
		virtual public System.IO.Stream OutputStream
		{
			get
			{
				return byteArrayOS;
			}
			// getOutputStream()
			
		}
		
		//UPGRADE_ISSUE: Constructor 'java.io.PipedOutputStream.PipedOutputStream' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaioPipedOutputStreamPipedOutputStream'"
		internal System.IO.StreamWriter pipedOS = new PipedOutputStream();
		internal bool keepRunning = true;
		//UPGRADE_NOTE: The initialization of  'byteArrayOS' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		internal System.IO.MemoryStream byteArrayOS;
		
		//UPGRADE_NOTE: The initialization of  'pipedIS' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private System.IO.StreamReader pipedIS;
		
		public LoopedStreams()
		{
			InitBlock();
			//UPGRADE_ISSUE: Method 'java.io.PipedOutputStream.connect' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaioPipedOutputStreamconnect_javaioPipedInputStream'"
			pipedOS.connect(pipedIS);
			startByteArrayReaderThread();
		} // LoopedStreams()
		
		private void  startByteArrayReaderThread()
		{
			
			new SupportClass.ThreadClass(new System.Threading.ThreadStart(new AnonymousClassRunnable(this).Run)).Start();
		} // startByteArrayReaderThread()
	} // LoopedStreams
}