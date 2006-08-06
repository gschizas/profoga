/* $RCSfile$
* $Author: egonw $
* $Date: 2006-02-05 09:05:42 -0500 (Sun, 05 Feb 2006) $
* $Revision: 4453 $
*
* Copyright (C) 2003-2005  Miguel, Jmol Development, www.jmol.org
*
* Contact: miguel@jmol.org
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
namespace org.jmol.g3d
{
	
	/// <summary><p>
	/// Implementation of Platform3D when using AWT on 1.1 JVMs.
	/// </p>
	/// <p>
	/// Uses the AWT imaging routines to convert an int[] of ARGB values
	/// into an Image by implementing the ImageProducer interface.
	/// </p>
	/// <p>
	/// This is used by MSFT Internet Explorer with the MSFT JVM,
	/// and Netscape 4.* on both Win32 and MacOS 9.
	/// </p>
	/// 
	/// </summary>
	/// <author>  Miguel, miguel@jmol.org
	/// </author>
	sealed class Awt3D:Platform3D, System.Drawing.Image
	{
		
		internal System.Windows.Forms.Control component;
		
		internal System.Drawing.Color colorModelRGB;
		//UPGRADE_ISSUE: Interface 'java.awt.image.ImageConsumer' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtimageImageConsumer'"
		internal ImageConsumer ic;
		
		internal Awt3D(System.Windows.Forms.Control component)
		{
			this.component = component;
			//UPGRADE_ISSUE: Method 'java.awt.Toolkit.getColorModel' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtToolkit'"
			//UPGRADE_ISSUE: Method 'java.awt.Toolkit.getDefaultToolkit' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtToolkit'"
			colorModelRGB = Toolkit.getDefaultToolkit().getColorModel();
		}
		
		internal override System.Drawing.Image allocateImage()
		{
			//UPGRADE_ISSUE: Method 'java.awt.Component.createImage' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtComponentcreateImage_javaawtimageImageProducer'"
			return component.createImage(this);
		}
		
		internal override void  notifyEndOfRendering()
		{
			if (this.ic != null)
				startProduction(ic);
		}
		
		internal override System.Drawing.Image allocateOffscreenImage(int width, int height)
		{
			//    System.out.println("allocateOffscreenImage(" + width + "," + height + ")");
			System.Drawing.Image img = new System.Drawing.Bitmap(width, height);
			//    System.out.println("img=" + img);
			return img;
		}
		
		internal override System.Drawing.Graphics getGraphics(System.Drawing.Image image)
		{
			return System.Drawing.Graphics.FromImage(image);
		}
		
		//UPGRADE_NOTE: The equivalent of method 'java.awt.image.ImageProducer.addConsumer' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		//UPGRADE_ISSUE: Interface 'java.awt.image.ImageConsumer' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtimageImageConsumer'"
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'addConsumer'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public void  addConsumer(ImageConsumer ic)
		{
			lock (this)
			{
				startProduction(ic);
			}
		}
		
		//UPGRADE_NOTE: The equivalent of method 'java.awt.image.ImageProducer.isConsumer' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		//UPGRADE_ISSUE: Interface 'java.awt.image.ImageConsumer' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtimageImageConsumer'"
		public bool isConsumer(ImageConsumer ic)
		{
			return (this.ic == ic);
		}
		
		//UPGRADE_NOTE: The equivalent of method 'java.awt.image.ImageProducer.removeConsumer' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		//UPGRADE_ISSUE: Interface 'java.awt.image.ImageConsumer' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtimageImageConsumer'"
		public void  removeConsumer(ImageConsumer ic)
		{
			if (this.ic == ic)
				this.ic = null;
		}
		
		//UPGRADE_NOTE: The equivalent of method 'java.awt.image.ImageProducer.requestTopDownLeftRightResend' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		//UPGRADE_ISSUE: Interface 'java.awt.image.ImageConsumer' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtimageImageConsumer'"
		public void  requestTopDownLeftRightResend(ImageConsumer ic)
		{
		}
		
		//UPGRADE_NOTE: The equivalent of method 'java.awt.image.ImageProducer.startProduction' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		//UPGRADE_ISSUE: Interface 'java.awt.image.ImageConsumer' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtimageImageConsumer'"
		public void  startProduction(ImageConsumer ic)
		{
			if (this.ic != ic)
			{
				this.ic = ic;
				//UPGRADE_ISSUE: Method 'java.awt.image.ImageConsumer.setDimensions' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtimageImageConsumer'"
				ic.setDimensions(windowWidth, windowHeight);
				//UPGRADE_ISSUE: Method 'java.awt.image.ImageConsumer.setHints' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtimageImageConsumer'"
				//UPGRADE_ISSUE: Field 'java.awt.image.ImageConsumer.TOPDOWNLEFTRIGHT' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtimageImageConsumer'"
				//UPGRADE_ISSUE: Field 'java.awt.image.ImageConsumer.COMPLETESCANLINES' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtimageImageConsumer'"
				//UPGRADE_ISSUE: Field 'java.awt.image.ImageConsumer.SINGLEPASS' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtimageImageConsumer'"
				ic.setHints(ImageConsumer.TOPDOWNLEFTRIGHT | ImageConsumer.COMPLETESCANLINES | ImageConsumer.SINGLEPASS);
			}
			//UPGRADE_ISSUE: Method 'java.awt.image.ImageConsumer.setPixels' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtimageImageConsumer'"
			ic.setPixels(0, 0, windowWidth, windowHeight, colorModelRGB, pBuffer, 0, windowWidth);
			//UPGRADE_ISSUE: Method 'java.awt.image.ImageConsumer.imageComplete' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtimageImageConsumer'"
			//UPGRADE_ISSUE: Field 'java.awt.image.ImageConsumer.SINGLEFRAMEDONE' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtimageImageConsumer'"
			ic.imageComplete(ImageConsumer.SINGLEFRAMEDONE);
		}
	}
}