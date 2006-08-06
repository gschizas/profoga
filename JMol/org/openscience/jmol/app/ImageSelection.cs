/* $RCSfile$
* $Author$
* $Date$
* $Revision$
*
* Copyright (C) 2005  The Jmol Development Team
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
*  Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA
*  02110-1301, USA.
*/
using System;
namespace org.openscience.jmol.app
{
	
	/// <summary> This class is used to transfer an {@link Image} into the clipboard.
	/// 
	/// </summary>
	/// <author>  Nicolas Vervelle
	/// </author>
	public class ImageSelection : System.Windows.Forms.IDataObject
	{
		/// <summary> Transers <code>image</code> into the clipboard.
		/// 
		/// </summary>
		/// <param name="image">Image to transfer into the clipboard.
		/// </param>
		public static System.Drawing.Image Clipboard
		{
			set
			{
				ImageSelection sel = new ImageSelection(value);
				System.Windows.Forms.Clipboard.SetDataObject(sel);
			}
			
		}
		
		/// <summary> The image to transfer into the clipboard.</summary>
		private System.Drawing.Image image;
		
		/// <summary> Constructs a <code>ImageSelection</code>.
		/// 
		/// </summary>
		/// <param name="image">The real Image.
		/// </param>
		public ImageSelection(System.Drawing.Image image)
		{
			this.image = image;
		}
		
		/* (non-Javadoc)
		* @see java.awt.datatransfer.Transferable#getTransferDataFlavors()
		*/
		//UPGRADE_ISSUE: The equivalent in .NET for method 'java.awt.datatransfer.Transferable.getTransferDataFlavors' returns a different type. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1224'"
		public  virtual System.String[] GetFormats()
		{
			return new System.Windows.Forms.DataFormats.Format[]{DataFlavor.imageFlavor};
		}
		
		/* (non-Javadoc)
		* @see java.awt.datatransfer.Transferable#isDataFlavorSupported(java.awt.datatransfer.DataFlavor)
		*/
		//UPGRADE_NOTE: The equivalent of method 'java.awt.datatransfer.Transferable.isDataFlavorSupported' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		public virtual bool isDataFlavorSupported(System.Windows.Forms.DataFormats.Format flavor)
		{
			return DataFlavor.imageFlavor.equals(flavor);
		}
		
		/* (non-Javadoc)
		* @see java.awt.datatransfer.Transferable#getTransferData(java.awt.datatransfer.DataFlavor)
		*/
		//UPGRADE_NOTE: The equivalent of method 'java.awt.datatransfer.Transferable.getTransferData' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		public virtual System.Object getTransferData(System.Windows.Forms.DataFormats.Format flavor)
		{
			if (!DataFlavor.imageFlavor.equals(flavor))
			{
				throw new System.Exception();
			}
			return image;
		}
		//UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1232'"
		virtual public System.Object GetData(System.String format, System.Boolean autoConvert)
		{
			return null;
		}
		//UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1232'"
		virtual public System.Object GetData(System.String format)
		{
			return null;
		}
		//UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1232'"
		virtual public System.Object GetData(System.Type format)
		{
			return null;
		}
		//UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1232'"
		virtual public void  SetData(System.String format, System.Boolean autoConvert, System.Object data)
		{
		}
		//UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1232'"
		virtual public void  SetData(System.String format, System.Object data)
		{
		}
		//UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1232'"
		virtual public void  SetData(System.Type format, System.Object data)
		{
		}
		//UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1232'"
		virtual public void  SetData(System.Object data)
		{
		}
		//UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1232'"
		virtual public System.Boolean GetDataPresent(System.String format, System.Boolean autoConvert)
		{
			return false;
		}
		//UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1232'"
		virtual public System.Boolean GetDataPresent(System.String format)
		{
			return false;
		}
		//UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1232'"
		virtual public System.Boolean GetDataPresent(System.Type format)
		{
			return false;
		}
		//UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1232'"
		virtual public System.String[] GetFormats(System.Boolean autoConvert)
		{
			return null;
		}
	}
}