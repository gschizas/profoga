/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-03-21 21:09:01 -0500 (Tue, 21 Mar 2006) $
* $Revision: 4679 $
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
	/// Implementation of Platform3D when using Swing on JVMs >= 1.2
	/// </p>
	/// <p>
	/// Uses the BufferedImage classe to turn an int[] into an
	/// Image that can be drawn.
	/// </p>
	/// <p>
	/// This is used by everything except
	/// MSFT Internet Explorer with the MSFT JVM,
	/// and Netscape 4.* on both Win32 and MacOS 9.
	/// </p>
	/// 
	/// </summary>
	/// <author>  Miguel, miguel@jmol.org
	/// </author>
	
	sealed class Swing3D:Platform3D
	{
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'rgbColorModel '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.Drawing.Color rgbColorModel = SupportClass.ColorSupport.GetRGBDefault();
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'sampleModelBitMasks'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int[] sampleModelBitMasks = new int[]{0x00FF0000, 0x0000FF00, 0x000000FF, unchecked((int) 0xFF000000)};
		
		internal override System.Drawing.Image allocateImage()
		{
			//UPGRADE_TODO: Class 'java.awt.image.SinglePixelPackedSampleModel' was converted to 'System.Drawing.Bitmap' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtimageSinglePixelPackedSampleModel'"
			System.Drawing.Bitmap sppsm = new System.Drawing.Bitmap(windowWidth, windowHeight);
			System.Object[] tempObjectArray;
			//UPGRADE_TODO: Constructor 'java.awt.image.DataBufferInt.DataBufferInt' was converted to 'System.Collections.ArrayList.ArrayList' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtimageDataBufferIntDataBufferInt_int[]_int'"
			tempObjectArray = new System.Object[windowSize];
			System.Array.Copy(pBuffer, tempObjectArray, tempObjectArray.Length);
			System.Collections.ArrayList dbi = new System.Collections.ArrayList(tempObjectArray);
			//UPGRADE_TODO: Class 'java.awt.image.WritableRaster' was converted to 'System.Drawing.Bitmap' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtimageWritableRaster'"
			System.IO.MemoryStream tempDataBuffer;
			tempDataBuffer = new System.IO.MemoryStream();
			sppsm.Save(tempDataBuffer, System.Drawing.Imaging.ImageFormat.Bmp);
			//UPGRADE_TODO: Method 'java.awt.image.Raster.createWritableRaster' was converted to 'System.Drawing.Bitmap' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtimageRastercreateWritableRaster_javaawtimageSampleModel_javaawtimageDataBuffer_javaawtPoint'"
			System.Drawing.Bitmap wr = new System.Drawing.Bitmap(tempDataBuffer);
			//UPGRADE_ISSUE: Constructor 'java.awt.image.BufferedImage.BufferedImage' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtimageBufferedImageBufferedImage_javaawtimageColorModel_javaawtimageWritableRaster_boolean_javautilHashtable'"
			System.Drawing.Bitmap bi = new BufferedImage(rgbColorModel, wr, false, null);
			return bi;
		}
		
		internal override System.Drawing.Image allocateOffscreenImage(int width, int height)
		{
			return new System.Drawing.Bitmap(width, height, (System.Drawing.Imaging.PixelFormat) System.Drawing.Imaging.PixelFormat.Format32bppRgb);
		}
		
		internal override System.Drawing.Graphics getGraphics(System.Drawing.Image image)
		{
			System.Drawing.Bitmap bi = (System.Drawing.Bitmap) image;
			System.Drawing.Graphics g2d = System.Drawing.Graphics.FromImage(bi);
			// miguel 20041122
			// we need to turn off text antialiasing on OSX when
			// running in a web browser
			//UPGRADE_ISSUE: Method 'java.awt.Graphics2D.setRenderingHint' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGraphics2DsetRenderingHint_javaawtRenderingHintsKey_javalangObject'"
			//UPGRADE_ISSUE: Field 'java.awt.RenderingHints.KEY_TEXT_ANTIALIASING' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtRenderingHintsKEY_TEXT_ANTIALIASING_f'"
			//UPGRADE_ISSUE: Field 'java.awt.RenderingHints.VALUE_TEXT_ANTIALIAS_OFF' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtRenderingHintsVALUE_TEXT_ANTIALIAS_OFF_f'"
			g2d.setRenderingHint(RenderingHints.KEY_TEXT_ANTIALIASING, RenderingHints.VALUE_TEXT_ANTIALIAS_OFF);
			// I don't know if we need these or not, but cannot hurt to have them
			//UPGRADE_ISSUE: Method 'java.awt.Graphics2D.setRenderingHint' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGraphics2DsetRenderingHint_javaawtRenderingHintsKey_javalangObject'"
			//UPGRADE_ISSUE: Field 'java.awt.RenderingHints.KEY_ANTIALIASING' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtRenderingHintsKEY_ANTIALIASING_f'"
			//UPGRADE_ISSUE: Field 'java.awt.RenderingHints.VALUE_ANTIALIAS_OFF' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtRenderingHintsVALUE_ANTIALIAS_OFF_f'"
			g2d.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_OFF);
			//UPGRADE_ISSUE: Method 'java.awt.Graphics2D.setRenderingHint' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGraphics2DsetRenderingHint_javaawtRenderingHintsKey_javalangObject'"
			//UPGRADE_ISSUE: Field 'java.awt.RenderingHints.KEY_RENDERING' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtRenderingHintsKEY_RENDERING_f'"
			g2d.setRenderingHint(RenderingHints.KEY_RENDERING, (System.Object) System.Drawing.Drawing2D.CompositingQuality.HighSpeed);
			return g2d;
		}
	}
}