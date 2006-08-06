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
	
	class ClassPreloader:SupportClass.ThreadClass
	{
		
		internal AppletWrapper appletWrapper;
		
		internal ClassPreloader(AppletWrapper appletWrapper)
		{
			this.appletWrapper = appletWrapper;
		}
		
		override public void  Run()
		{
			System.String className;
			//UPGRADE_TODO: The differences in the type  of parameters for method 'java.lang.Thread.setPriority'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
			Priority = (System.Threading.ThreadPriority) ((System.Int32) Priority - 1);
			while ((className = appletWrapper.NextPreloadClassName) != null)
			{
				//      System.out.println("preloading " + className);
				try
				{
					int lastCharIndex = className.Length - 1;
					bool constructOne = className[lastCharIndex] == '+';
					if (constructOne)
						className = className.Substring(0, (lastCharIndex) - (0));
					//UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
					System.Type preloadClass = System.Type.GetType(className);
					if (constructOne)
					{
						//UPGRADE_TODO: Method 'java.lang.Class.newInstance' was converted to 'System.Activator.CreateInstance' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangClassnewInstance'"
						System.Activator.CreateInstance(preloadClass);
					}
					//        System.out.println("finished preloading " + className);
				}
				catch (System.Exception e)
				{
					System.Console.Out.WriteLine("error preloading " + className);
					SupportClass.WriteStackTrace(e, Console.Error);
				}
			}
		}
	}
}