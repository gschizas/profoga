/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-03-25 12:26:43 -0500 (Sat, 25 Mar 2006) $
* $Revision: 4698 $
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
	
	/// <summary> implementation for text rendering
	/// <p>
	/// uses java fonts by rendering into an offscreen buffer.
	/// strings are rasterized and stored as a bitmap in an int[].
	/// <p>
	/// needs work
	/// 
	/// 
	/// </summary>
	/// <author>  Miguel, miguel@jmol.org
	/// </author>
	class Text3D
	{
		/*
		we have a few problems here
		a message is probably going to vary in size with z depth
		a message is probably going to be repeated by more than one atom
		fonts?
		just one?
		a restricted number?
		any font?
		if we want to support more than one then a fontindex is probably
		called for in order to prevent a hashtable lookup
		color - can be applied by the painter
		rep
		array of booleans - uncompressed
		array of bits - uncompressed - i like this
		some type of run-length, using bytes
		*/
		internal System.Windows.Forms.Control component;
		internal int height; // this height is just ascent + descent ... no reason for leading
		internal int ascent;
		internal int width;
		internal int size;
		internal int[] bitmap;
		
		internal Text3D(System.String text, Font3D font3d, Platform3D platform)
		{
			calcMetrics(text, font3d);
			platform.checkOffscreenSize(width, height);
			renderOffscreen(text, font3d, platform);
			rasterize(platform);
		}
		
		/*
		static int widthBuffer;
		static int heightBuffer;
		static Image img;
		static Graphics g;
		
		void checkImageBufferSize(Component component, int width, int height) {
		boolean realloc = false;
		int widthT = widthBuffer;
		int heightT = heightBuffer;
		if (width > widthT) {
		widthT = (width + 63) & ~63;
		realloc = true;
		}
		if (height > heightT) {
		heightT = (height + 7) & ~7;
		realloc = true;
		}
		if (realloc) {
		if (g != null)
		g.dispose();
		img = component.createImage(widthT, heightT);
		widthBuffer = widthT;
		heightBuffer = heightT;
		g = img.getGraphics();
		}
		}
		
		*/
		
		internal virtual void  calcMetrics(System.String text, Font3D font3d)
		{
			System.Drawing.Font fontMetrics = font3d.fontMetrics;
			//UPGRADE_TODO: The equivalent in .NET for method 'java.awt.FontMetrics.getAscent' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			ascent = SupportClass.GetAscent(fontMetrics);
			//UPGRADE_TODO: The equivalent in .NET for method 'java.awt.FontMetrics.getDescent' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			height = ascent + SupportClass.GetDescent(fontMetrics);
			//UPGRADE_ISSUE: Method 'java.awt.FontMetrics.stringWidth' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtFontMetricsstringWidth_javalangString'"
			width = fontMetrics.stringWidth(text);
			size = width * height;
		}
		
		internal virtual void  renderOffscreen(System.String text, Font3D font3d, Platform3D platform)
		{
			System.Drawing.Graphics g = platform.gOffscreen;
			SupportClass.GraphicsManager.manager.SetColor(g, System.Drawing.Color.Black);
			g.FillRectangle(SupportClass.GraphicsManager.manager.GetPaint(g), 0, 0, width, height);
			SupportClass.GraphicsManager.manager.SetColor(g, System.Drawing.Color.White);
			SupportClass.GraphicsManager.manager.SetFont(g, font3d.font);
			//UPGRADE_TODO: Method 'java.awt.Graphics.drawString' was converted to 'System.Drawing.Graphics.DrawString' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphicsdrawString_javalangString_int_int'"
			g.DrawString(text, SupportClass.GraphicsManager.manager.GetFont(g), SupportClass.GraphicsManager.manager.GetBrush(g), 0, ascent - SupportClass.GraphicsManager.manager.GetFont(g).GetHeight());
		}
		
		internal virtual void  rasterize(Platform3D platform)
		{
			SupportClass.PixelCapturer pixelGrabber = new SupportClass.PixelCapturer(platform.imageOffscreen, 0, 0, width, height, true);
			try
			{
				pixelGrabber.CapturePixels();
			}
			catch (System.Threading.ThreadInterruptedException e)
			{
				System.Console.Out.WriteLine("Que? 7748");
			}
			int[] pixels = (int[]) pixelGrabber.Pixels;
			
			int bitmapSize = (size + 31) >> 5;
			bitmap = new int[bitmapSize];
			
			int offset, shifter;
			for (offset = shifter = 0; offset < size; ++offset, shifter <<= 1)
			{
				if ((pixels[offset] & 0x00FFFFFF) != 0)
					shifter |= 1;
				if ((offset & 31) == 31)
					bitmap[offset >> 5] = shifter;
			}
			if ((offset & 31) != 0)
			{
				shifter <<= 31 - (offset & 31);
				bitmap[offset >> 5] = shifter;
			}
			
			if (false)
			{
				// error checking
				// shifter error checking
				bool[] bits = new bool[size];
				for (int i = 0; i < size; ++i)
					bits[i] = (pixels[i] & 0x00FFFFFF) != 0;
				//
				for (offset = 0; offset < size; ++offset, shifter <<= 1)
				{
					if ((offset & 31) == 0)
						shifter = bitmap[offset >> 5];
					if (shifter < 0)
					{
						if (!bits[offset])
						{
							System.Console.Out.WriteLine("false positive @" + offset);
							System.Console.Out.WriteLine("size = " + size);
						}
					}
					else
					{
						if (bits[offset])
						{
							System.Console.Out.WriteLine("false negative @" + offset);
							System.Console.Out.WriteLine("size = " + size);
						}
					}
				}
				// error checking
			}
		}
		
		internal static System.Collections.Hashtable htFont3d = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
		
		// FIXME mth
		// we have a synchronization issue/race condition  here with multiple
		// so only one Text3D can be generated at a time
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'getText3D'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		internal static Text3D getText3D(System.String text, Font3D font3d, Platform3D platform)
		{
			lock (typeof(org.jmol.g3d.Text3D))
			{
				
				System.Collections.Hashtable htForThisFont = (System.Collections.Hashtable) htFont3d[font3d];
				if (htForThisFont != null)
				{
					Text3D text3d = (Text3D) htForThisFont[text];
					if (text3d != null)
						return text3d;
				}
				else
				{
					htForThisFont = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
					htFont3d[font3d] = htForThisFont;
				}
				Text3D text3d2 = new Text3D(text, font3d, platform);
				htForThisFont[text] = text3d2;
				return text3d2;
			}
		}
		
		internal static void  plot(int x, int y, int z, int argb, int argbBackground, System.String text, Font3D font3d, Graphics3D g3d)
		{
			Text3D text3d = getText3D(text, font3d, g3d.platform);
			int[] bitmap = text3d.bitmap;
			int textWidth = text3d.width;
			int textHeight = text3d.height;
			if (x + textWidth < 0 || x > g3d.width || y + textHeight < 0 || y > g3d.height)
				return ;
			if (x < 0 || x + textWidth > g3d.width || y < 0 || y + textHeight > g3d.height)
				plotClipped(x, y, z, argb, argbBackground, g3d, textWidth, textHeight, bitmap);
			else
				plotUnclipped(x, y, z, argb, argbBackground, g3d, textWidth, textHeight, bitmap);
		}
		
		internal static void  plotUnclipped(int x, int y, int z, int argb, int argbBackground, Graphics3D g3d, int textWidth, int textHeight, int[] bitmap)
		{
			int offset = 0;
			int shiftregister = 0;
			int i = 0, j = 0;
			int[] zbuf = g3d.zbuf;
			int[] pbuf = g3d.pbuf;
			int screenWidth = g3d.width;
			int pbufOffset = y * screenWidth + x;
			while (i < textHeight)
			{
				while (j < textWidth)
				{
					if ((offset & 31) == 0)
						shiftregister = bitmap[offset >> 5];
					if (shiftregister == 0 && argbBackground == 0)
					{
						int skip = 32 - (offset & 31);
						j += skip;
						offset += skip;
						pbufOffset += skip;
					}
					else
					{
						if (shiftregister < 0 || argbBackground != 0)
						{
							if (z < zbuf[pbufOffset])
							{
								zbuf[pbufOffset] = z;
								pbuf[pbufOffset] = shiftregister < 0?argb:argbBackground;
							}
						}
						shiftregister <<= 1;
						++offset;
						++j;
						++pbufOffset;
					}
				}
				while (j >= textWidth)
				{
					++i;
					j -= textWidth;
					pbufOffset += (screenWidth - textWidth);
				}
			}
		}
		
		internal static void  plotClipped(int x, int y, int z, int argb, int argbBackground, Graphics3D g3d, int textWidth, int textHeight, int[] bitmap)
		{
			int offset = 0;
			int shiftregister = 0;
			int i = 0, j = 0;
			while (i < textHeight)
			{
				while (j < textWidth)
				{
					if ((offset & 31) == 0)
						shiftregister = bitmap[offset >> 5];
					if (shiftregister == 0 && argbBackground == 0)
					{
						int skip = 32 - (offset & 31);
						j += skip;
						offset += skip;
					}
					else
					{
						if (shiftregister < 0 || argbBackground != 0)
							g3d.plotPixelClippedNoSlab(shiftregister < 0?argb:argbBackground, x + j, y + i, z);
						shiftregister <<= 1;
						++offset;
						++j;
					}
				}
				while (j >= textWidth)
				{
					++i;
					j -= textWidth;
				}
			}
		}
	}
}