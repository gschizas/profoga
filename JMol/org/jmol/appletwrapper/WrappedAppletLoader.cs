/* $RCSfile$
* $Author: egonw $
* $Date: 2005-11-10 16:52:44 +0100 (jeu., 10 nov. 2005) $
* $Revision: 4255 $
*
* Copyright (C) 2004-2005  The Jmol Development Team
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
namespace org.jmol.appletwrapper
{
	
	class WrappedAppletLoader:SupportClass.ThreadClass
	{
		
		internal AppletWrapper appletWrapper;
		internal System.String wrappedAppletClassName;
		
		private const int minimumLoadSeconds = 0;
		
		internal WrappedAppletLoader(AppletWrapper appletWrapper, System.String wrappedAppletClassName)
		{
			this.appletWrapper = appletWrapper;
			this.wrappedAppletClassName = wrappedAppletClassName;
		}
		
		override public void  Run()
		{
			long startTime = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
			System.Console.Out.WriteLine("WrappedAppletLoader.run(" + wrappedAppletClassName + ")");
			TickerThread tickerThread = new TickerThread(appletWrapper);
			tickerThread.Start();
			WrappedApplet wrappedApplet = null;
			try
			{
				//UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
				System.Type wrappedAppletClass = System.Type.GetType(wrappedAppletClassName);
				wrappedApplet = (WrappedApplet) System.Activator.CreateInstance(wrappedAppletClass);
				wrappedApplet.AppletWrapper = appletWrapper;
				wrappedApplet.init();
			}
			catch (System.Exception e)
			{
				System.Console.Out.WriteLine("Could not instantiate wrappedApplet class" + wrappedAppletClassName);
				SupportClass.WriteStackTrace(e, Console.Error);
			}
			long loadTimeSeconds = ((System.DateTime.Now.Ticks - 621355968000000000) / 10000 - startTime + 500) / 1000;
			System.Console.Out.WriteLine(wrappedAppletClassName + " load time = " + loadTimeSeconds + " seconds");
			if (minimumLoadSeconds != 0)
			{
				// optimizer should eliminate all this code
				long minimumEndTime = startTime + 1000 * minimumLoadSeconds;
				int sleepTime = (int) (minimumEndTime - (System.DateTime.Now.Ticks - 621355968000000000) / 10000);
				if (sleepTime > 0)
				{
					System.Console.Out.WriteLine("artificial minimum load time engaged");
					try
					{
						//UPGRADE_TODO: Method 'java.lang.Thread.sleep' was converted to 'System.Threading.Thread.Sleep' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangThreadsleep_long'"
						System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64) 10000 * sleepTime));
					}
					catch (System.Threading.ThreadInterruptedException ie)
					{
					}
				}
			}
			tickerThread.keepRunning = false;
			tickerThread.Interrupt();
			appletWrapper.wrappedApplet = wrappedApplet;
			//UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
			appletWrapper.Refresh();
		}
	}
	
	class TickerThread:SupportClass.ThreadClass
	{
		internal AppletWrapper appletWrapper;
		internal bool keepRunning = true;
		
		internal TickerThread(AppletWrapper appletWrapper)
		{
			this.appletWrapper = appletWrapper;
		}
		
		override public void  Run()
		{
			do 
			{
				try
				{
					//UPGRADE_TODO: Method 'java.lang.Thread.sleep' was converted to 'System.Threading.Thread.Sleep' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangThreadsleep_long'"
					System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64) 10000 * 999));
				}
				catch (System.Threading.ThreadInterruptedException ie)
				{
					break;
				}
				appletWrapper.repaintClock();
			}
			while (keepRunning);
		}
	}
}