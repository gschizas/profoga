/* $RCSfile$
* $Author: egonw $
* $Date: 2005-11-10 16:52:44 +0100 (jeu., 10 nov. 2005) $
* $Revision: 4255 $
*
* Copyright (C) 2003-2005  The Jmol Development Team
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
	
	/// <summary> Implements a queue for a bash-like command history.
	/// 
	/// 
	/// </summary>
	/// <author>   Agust\u00ED S\u00E1nchez Furrasola
	/// </author>
	/// <version>  $Revision: 4255 $ 2003-07-28
	/// 
	/// </version>
	sealed class CommandHistory
	{
		/// <summary> Retrieves the following command from the bottom of the list, updates
		/// list position.
		/// </summary>
		/// <returns> the String value of a command
		/// </returns>
		internal System.String CommandUp
		{
			get
			{
				if (commandList.Count > 0)
					pos--;
				return Command;
			}
			
		}
		/// <summary> Retrieves the following command from the top of the list, updates 
		/// list position.
		/// </summary>
		/// <returns> the String value of a command
		/// </returns>
		internal System.String CommandDown
		{
			get
			{
				if (commandList.Count > 0)
					pos++;
				return Command;
			}
			
		}
		/// <summary> Calculates the command to return.
		/// 
		/// </summary>
		/// <returns> the String value of a command
		/// </returns>
		private System.String Command
		{
			get
			{
				if (pos == 0)
				{
					return "";
				}
				int size = commandList.Count;
				if (size > 0)
				{
					if (pos == (size + 1))
					{
						return ""; // just beyond last one: ""
					}
					else if (pos > size)
					{
						pos = 1; // roll around to first command
					}
					else if (pos < 0)
					{
						pos = size; // roll around to last command
					}
					return (System.String) commandList[pos - 1];
				}
				return "";
			}
			
		}
		/// <summary> Resets maximum size of command queue. Cuts off extra commands.
		/// 
		/// </summary>
		/// <param name="maxSize">maximum size for the command queue
		/// </param>
		internal int MaxSize
		{
			set
			{
				this.maxSize = value;
				
				while (value < commandList.Count)
				{
					commandList.RemoveAt(0);
				}
			}
			
		}
		//UPGRADE_TODO: Class 'java.util.LinkedList' was converted to 'System.Collections.ArrayList' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilLinkedList'"
		private System.Collections.ArrayList commandList = new System.Collections.ArrayList();
		
		private int maxSize;
		
		private int pos = 0;
		
		/// <summary> Creates a new instance.
		/// 
		/// </summary>
		/// <param name="maxSize">maximum size for the command queue
		/// </param>
		internal CommandHistory(int maxSize)
		{
			this.maxSize = maxSize;
		}
		
		/// <summary> Adds a new command to the bottom of the list, resets
		/// list position.
		/// </summary>
		/// <param name="command">the String value of a command
		/// </param>
		internal void  addCommand(System.String command)
		{
			//      System.out.println(command);
			
			pos = 0;
			
			commandList.Insert(commandList.Count, command);
			
			if (commandList.Count > maxSize)
			{
				commandList.RemoveAt(0);
			}
		}
		
		/// <summary> Resets instance.
		/// 
		/// </summary>
		/// <param name="maxSize">maximum size for the command queue
		/// </param>
		internal void  reset(int maxSize)
		{
			this.maxSize = maxSize;
			//UPGRADE_TODO: Class 'java.util.LinkedList' was converted to 'System.Collections.ArrayList' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilLinkedList'"
			commandList = new System.Collections.ArrayList();
		}
		
		[STAThread]
		public static void  Main(System.String[] args)
		{
			CommandHistory h = new CommandHistory(4);
			
			h.addCommand("a");
			h.addCommand("b");
			h.addCommand("c");
			h.addCommand("d");
			
			System.Console.Out.WriteLine(h.CommandUp);
			System.Console.Out.WriteLine(h.CommandUp);
			System.Console.Out.WriteLine(h.CommandUp);
			System.Console.Out.WriteLine(h.CommandUp);
			System.Console.Out.WriteLine(h.CommandUp);
			System.Console.Out.WriteLine(h.CommandUp);
			
			System.Console.Out.WriteLine("******");
			System.Console.Out.WriteLine(h.CommandDown);
			System.Console.Out.WriteLine(h.CommandDown);
			System.Console.Out.WriteLine(h.CommandDown);
			System.Console.Out.WriteLine(h.CommandDown);
			System.Console.Out.WriteLine(h.CommandDown);
			System.Console.Out.WriteLine(h.CommandDown);
			System.Console.Out.WriteLine(h.CommandDown);
			
			
			h.MaxSize = 2;
			
			
			System.Console.Out.WriteLine(h.CommandUp);
			System.Console.Out.WriteLine(h.CommandUp);
			System.Console.Out.WriteLine(h.CommandUp);
			System.Console.Out.WriteLine(h.CommandUp);
			System.Console.Out.WriteLine(h.CommandUp);
			System.Console.Out.WriteLine(h.CommandUp);
			
			System.Console.Out.WriteLine("******");
			System.Console.Out.WriteLine(h.CommandDown);
			System.Console.Out.WriteLine(h.CommandDown);
			System.Console.Out.WriteLine(h.CommandDown);
			System.Console.Out.WriteLine(h.CommandDown);
			System.Console.Out.WriteLine(h.CommandDown);
			System.Console.Out.WriteLine(h.CommandDown);
			System.Console.Out.WriteLine(h.CommandDown);
		}
	}
}