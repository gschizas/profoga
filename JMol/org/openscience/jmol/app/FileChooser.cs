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
namespace org.openscience.jmol.app
{
	
	/// <summary> JFileChooser with possibility to fix size and location</summary>
	//UPGRADE_TODO: Parent class 'javax.swing.JFileChooser' was replaced with 'System.Windows.Forms.FileDialog'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1077'"
	[Serializable]
	public class FileChooser:System.Windows.Forms.FileDialog
	{
		/// <param name="p">Location of the JDialog
		/// </param>
		virtual public System.Drawing.Point DialogLocation
		{
			set
			{
				dialogLocation = value;
			}
			
		}
		/// <param name="d">Size of the JDialog
		/// </param>
		virtual public System.Drawing.Size DialogSize
		{
			set
			{
				dialogSize = value;
			}
			
		}
		/// <returns> Dialog containing the JFileChooser
		/// </returns>
		virtual public System.Windows.Forms.Form Dialog
		{
			get
			{
				return dialog;
			}
			
		}
		
		private System.Drawing.Point dialogLocation
		{
			get
			{
				return dialogLocation_Renamed;
			}
			
			set
			{
				dialogLocation_Renamed = value;
			}
			
		}
		private System.Drawing.Point dialogLocation_Renamed = System.Drawing.Point.Empty;
		private System.Drawing.Size dialogSize
		{
			get
			{
				return dialogSize_Renamed;
			}
			
			set
			{
				dialogSize_Renamed = value;
			}
			
		}
		private System.Drawing.Size dialogSize_Renamed = System.Drawing.Size.Empty;
		private System.Windows.Forms.Form dialog = null;
		
		/* (non-Javadoc)
		* @see javax.swing.JFileChooser#createDialog(java.awt.Component)
		*/
		protected internal virtual System.Windows.Forms.Form createDialog(System.Windows.Forms.Control parent)
		{
			dialog = base.createDialog(parent);
			if (dialog != null)
			{
				if (!dialogLocation.IsEmpty)
				{
					//UPGRADE_TODO: Method 'java.awt.Component.setLocation' was converted to 'System.Windows.Forms.Control.Location' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetLocation_javaawtPoint'"
					dialog.Location = dialogLocation;
				}
				if (!dialogSize.IsEmpty)
				{
					//UPGRADE_TODO: Method 'java.awt.Component.setSize' was converted to 'System.Windows.Forms.Control.Size' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetSize_javaawtDimension'"
					dialog.Size = dialogSize;
				}
			}
			return dialog;
		}
		//UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1232'"
		override System.Boolean RunFileDialog(System.Windows.Forms.NativeMethods.OPENFILENAME_I ofn)
		{
			return false;
		}
		//UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1232'"
		public override void  Reset()
		{
		}
		//UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1232'"
		protected override System.Boolean RunDialog(System.IntPtr hwndOwner)
		{
			return false;
		}
	}
}