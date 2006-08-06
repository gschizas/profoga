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
	
	/// <summary> The history file contains data from previous uses of Jmol.
	/// 
	/// </summary>
	/// <author>  Bradley A. Smith (bradley@baysmith.com)
	/// </author>
	//UPGRADE_NOTE: The access modifier for this class or class field has been changed in order to prevent compilation errors due to the visibility level. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1296'"
	public class HistoryFile
	{
		/// <returns> The properties stored in the history file.
		/// </returns>
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		virtual internal System.Collections.Specialized.NameValueCollection Properties
		{
			get
			{
				//UPGRADE_TODO: Format of property file may need to be changed. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1089'"
				//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
				return new System.Collections.Specialized.NameValueCollection(properties);
			}
			
		}
		
		/// <summary> The data stored in the history file.</summary>
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		//UPGRADE_TODO: Format of property file may need to be changed. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1089'"
		private System.Collections.Specialized.NameValueCollection properties = new System.Collections.Specialized.NameValueCollection();
		
		/// <summary> The location of the history file.</summary>
		internal System.IO.FileInfo file;
		
		/// <summary> The information written to the header of the history file.</summary>
		internal System.String header;
		
		/// <summary> Creates a history file.
		/// 
		/// </summary>
		/// <param name="file">the location of the file.
		/// </param>
		/// <param name="header">information written to the header of the file.
		/// </param>
		internal HistoryFile(System.IO.FileInfo file, System.String header)
		{
			this.file = file;
			this.header = header;
			load();
		}
		
		/// <summary> Adds the given properties to the history. If a property existed previously,
		/// it will be replaced.
		/// 
		/// </summary>
		/// <param name="properties">the properties to add.
		/// </param>
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		internal virtual void  addProperties(System.Collections.Specialized.NameValueCollection properties)
		{
			
			System.Collections.IEnumerator keys = properties.Keys.GetEnumerator();
			bool modified = false;
			//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
			while (keys.MoveNext())
			{
				//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
				System.String key = (System.String) keys.Current;
				System.String value_Renamed = properties.Get(key);
				modified |= addProperty(key, value_Renamed);
			}
			save();
		}
		
		/// <summary> Get the value of a property
		/// 
		/// </summary>
		/// <param name="key">Key of the property to find
		/// </param>
		/// <param name="defaultValue">Default value to use if the property is not found
		/// </param>
		/// <returns> The value of the property
		/// </returns>
		internal virtual System.String getProperty(System.String key, System.String defaultValue)
		{
			return properties[key] == null?defaultValue:properties[key];
		}
		
		/// <summary> Adds the given property to the history. If it existed previously,
		/// it will be replaced.
		/// 
		/// </summary>
		/// <param name="key">Key of the property to add
		/// </param>
		/// <param name="value">Value of the property
		/// </param>
		/// <returns> true if the property is modified
		/// </returns>
		private bool addProperty(System.String key, System.String value_Renamed)
		{
			bool modified = false;
			System.Object tempObject;
			//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
			tempObject = properties[key];
			properties[key] = value_Renamed;
			System.Object oldValue = tempObject;
			if (!value_Renamed.Equals(oldValue))
			{
				modified = true;
			}
			return modified;
		}
		
		/// <param name="name">Window name
		/// </param>
		/// <returns> Position of the window stored in the history file
		/// </returns>
		internal virtual System.Drawing.Point getWindowPosition(System.String name)
		{
			System.Drawing.Point result = System.Drawing.Point.Empty;
			if (name != null)
			{
				try
				{
					System.String x = getProperty("Jmol.window." + name + ".x", null);
					System.String y = getProperty("Jmol.window." + name + ".y", null);
					if ((x != null) && (y != null))
					{
						int posX = System.Int32.Parse(x);
						int posY = System.Int32.Parse(y);
						result = new System.Drawing.Point(posX, posY);
					}
				}
				catch (System.Exception e)
				{
					//Just return a null result
				}
			}
			return result;
		}
		
		/// <param name="name">Window name
		/// </param>
		/// <returns> Size of the window stored in the history file
		/// </returns>
		internal virtual System.Drawing.Size getWindowSize(System.String name)
		{
			System.Drawing.Size result = System.Drawing.Size.Empty;
			if (name != null)
			{
				try
				{
					System.String w = getProperty("Jmol.window." + name + ".w", null);
					System.String h = getProperty("Jmol.window." + name + ".h", null);
					if ((w != null) && (h != null))
					{
						int dimW = System.Int32.Parse(w);
						int dimH = System.Int32.Parse(h);
						result = new System.Drawing.Size(dimW, dimH);
					}
				}
				catch (System.Exception e)
				{
					//Just return a null result
				}
			}
			return result;
		}
		
		/// <param name="name">Window name
		/// </param>
		/// <returns> Visibility of the window stored in the history file
		/// </returns>
		internal virtual System.Boolean getWindowVisibility(System.String name)
		{
			//UPGRADE_TODO: The 'System.Boolean' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
			System.Boolean result = null;
			if (name != null)
			{
				try
				{
					System.String v = getProperty("Jmol.window." + name + ".visible", null);
					if (v != null)
					{
						//UPGRADE_NOTE: Exceptions thrown by the equivalent in .NET of method 'java.lang.Boolean.valueOf' may be different. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1099'"
						result = System.Boolean.Parse(v);
					}
				}
				catch (System.Exception e)
				{
					//Just return a null result
				}
			}
			return result;
		}
		
		/// <summary> Adds the window positon to the history.
		/// If it existed previously, it will be replaced.
		/// 
		/// </summary>
		/// <param name="name">Window name
		/// </param>
		/// <param name="position">Window position
		/// </param>
		/// <returns> Tells if the properties are modified
		/// </returns>
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		private bool addWindowPosition(System.String name, ref System.Drawing.Point position)
		{
			bool modified = false;
			if (name != null)
			{
				if (!position.IsEmpty)
				{
					modified |= addProperty("Jmol.window." + name + ".x", "" + position.X);
					modified |= addProperty("Jmol.window." + name + ".y", "" + position.Y);
				}
			}
			return modified;
		}
		
		/// <summary> Adds the window size to the history.
		/// If it existed previously, it will be replaced.
		/// 
		/// </summary>
		/// <param name="name">Window name
		/// </param>
		/// <param name="size">Window size
		/// </param>
		/// <returns> Tells if the properties are modified
		/// </returns>
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		private bool addWindowSize(System.String name, ref System.Drawing.Size size)
		{
			bool modified = false;
			if (name != null)
			{
				if (!size.IsEmpty)
				{
					modified |= addProperty("Jmol.window." + name + ".w", "" + size.Width);
					modified |= addProperty("Jmol.window." + name + ".h", "" + size.Height);
				}
			}
			return modified;
		}
		
		/// <summary> Adds the window visibility to the history.
		/// If it existed previously, it will be replaced.
		/// 
		/// </summary>
		/// <param name="name">Window name
		/// </param>
		/// <param name="visible">Window visibilite
		/// </param>
		/// <returns> Tells if the properties are modified
		/// </returns>
		private bool addWindowVisibility(System.String name, bool visible)
		{
			bool modified = false;
			if (name != null)
			{
				modified |= addProperty("Jmol.window." + name + ".visible", "" + visible);
			}
			return modified;
		}
		
		/// <summary> Adds the window informations to the history.
		/// If it existed previously, it will be replaced.
		/// 
		/// </summary>
		/// <param name="name">Window name
		/// </param>
		/// <param name="window">Window
		/// </param>
		internal virtual void  addWindowInfo(System.String name, System.Windows.Forms.Control window)
		{
			if (window != null)
			{
				bool modified = false;
				System.Drawing.Point tempAux = window.Location;
				//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
				modified |= addWindowPosition(name, ref tempAux);
				System.Drawing.Size tempAux2 = window.Size;
				//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
				modified |= addWindowSize(name, ref tempAux2);
				//UPGRADE_TODO: Method 'java.awt.Component.isVisible' was converted to 'System.Windows.Forms.Control.Visible' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentisVisible'"
				modified |= addWindowVisibility(name, window.Visible);
				if (modified)
				{
					save();
				}
			}
		}
		
		/// <summary> Uses the informations in the history to reposition the window.
		/// 
		/// </summary>
		/// <param name="name">Window name
		/// </param>
		/// <param name="window">Window
		/// </param>
		internal virtual void  repositionWindow(System.String name, System.Windows.Forms.Control window)
		{
			if (window != null)
			{
				System.Drawing.Point position = getWindowPosition(name);
				System.Drawing.Size size = getWindowSize(name);
				System.Boolean visible = getWindowVisibility(name);
				if (!position.IsEmpty)
				{
					//UPGRADE_TODO: Method 'java.awt.Component.setLocation' was converted to 'System.Windows.Forms.Control.Location' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetLocation_javaawtPoint'"
					window.Location = position;
				}
				if (!size.IsEmpty)
				{
					//UPGRADE_TODO: Method 'java.awt.Component.setSize' was converted to 'System.Windows.Forms.Control.Size' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetSize_javaawtDimension'"
					window.Size = size;
				}
				//UPGRADE_TODO: The 'System.Boolean' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
				if ((visible != null) && (visible.Equals(true)))
				{
					//UPGRADE_TODO: 'System.Windows.Forms.Application.Run' must be called to start a main form. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1135'"
					window.Visible = true;
				}
			}
		}
		
		/// <summary> Loads properties from the history file.</summary>
		private void  load()
		{
			
			try
			{
				//UPGRADE_TODO: Constructor 'java.io.FileInputStream.FileInputStream' was converted to 'System.IO.FileStream.FileStream' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileInputStreamFileInputStream_javaioFile'"
				System.IO.FileStream input = new System.IO.FileStream(file.FullName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
				//UPGRADE_TODO: Method 'java.util.Properties.load' was converted to 'System.Collections.Specialized.NameValueCollection' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiesload_javaioInputStream'"
				properties = new System.Collections.Specialized.NameValueCollection(System.Configuration.ConfigurationSettings.AppSettings);
				input.Close();
			}
			catch (System.IO.IOException ex)
			{
				// System.err.println("Error loading history: " + ex);
			}
		}
		
		/// <summary> Saves properties to the history file.</summary>
		private void  save()
		{
			
			try
			{
				//UPGRADE_TODO: Constructor 'java.io.FileOutputStream.FileOutputStream' was converted to 'System.IO.FileStream.FileStream' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileOutputStreamFileOutputStream_javaioFile'"
				System.IO.FileStream output = new System.IO.FileStream(file.FullName, System.IO.FileMode.Create);
				//UPGRADE_ISSUE: Method 'java.util.Properties.store' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilPropertiesstore_javaioOutputStream_javalangString'"
				properties.store(output, header);
				output.Close();
			}
			catch (System.IO.IOException ex)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				System.Console.Error.WriteLine("Error saving history: " + ex);
			}
		}
	}
}