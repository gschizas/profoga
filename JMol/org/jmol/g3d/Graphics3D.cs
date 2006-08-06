/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-03-25 13:27:02 -0500 (Sat, 25 Mar 2006) $
* $Revision: 4700 $
*
* Copyright (C) 2003-2006  Miguel, Jmol Development, www.jmol.org
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
//UPGRADE_TODO: The type 'javax.vecmath.Point3i' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Point3i = javax.vecmath.Point3i;
//UPGRADE_TODO: The type 'javax.vecmath.Point3f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Point3f = javax.vecmath.Point3f;
//UPGRADE_TODO: The type 'javax.vecmath.Vector3f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Vector3f = javax.vecmath.Vector3f;
//UPGRADE_TODO: The type 'javax.vecmath.Matrix3f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Matrix3f = javax.vecmath.Matrix3f;
namespace org.jmol.g3d
{
	
	/// <summary> Provides high-level graphics primitives for 3D visualization.
	/// <p>
	/// A pure software implementation of a 3D graphics engine.
	/// No hardware required.
	/// Depending upon what you are rendering ... some people say it
	/// is <i>pretty fast</i>.
	/// 
	/// </summary>
	/// <author>  Miguel, miguel@jmol.org
	/// </author>
	
	sealed public class Graphics3D
	{
		/// <summary> gets g3d width
		/// 
		/// </summary>
		/// <returns> width pixel count;
		/// </returns>
		public int WindowWidth
		{
			get
			{
				return width;
			}
			
		}
		/// <summary> gets g3d height
		/// 
		/// </summary>
		/// <returns> height pixel count
		/// </returns>
		public int WindowHeight
		{
			get
			{
				return height;
			}
			
		}
		public int RenderWidth
		{
			get
			{
				return width;
			}
			
		}
		public int RenderHeight
		{
			get
			{
				return height;
			}
			
		}
		/// <summary> sets background color to the specified argb value
		/// 
		/// </summary>
		/// <param name="argb">an argb value with alpha channel
		/// </param>
		public int BackgroundArgb
		{
			set
			{
				platform.Background = value;
			}
			
		}
		/// <summary> controls greyscale rendering</summary>
		/// <param name="greyscaleMode">Flag for greyscale rendering
		/// </param>
		public bool GreyscaleMode
		{
			set
			{
				this.inGreyscaleMode = value;
			}
			
		}
		public int Intensity
		{
			set
			{
				// only adjusting intensity, but colix & isTranslucent stay the same
				argbCurrent = argbNoisyUp = argbNoisyDn = shadesCurrent[value];
			}
			
		}
		public int FontOfSize
		{
			set
			{
				font3dCurrent = getFont3D(value);
			}
			
		}
		public Font3D Font3DCurrent
		{
			get
			{
				return font3dCurrent;
			}
			
		}
		public sbyte FontFidCurrent
		{
			get
			{
				return font3dCurrent.fid;
			}
			
		}
		public System.Drawing.Font FontMetrics
		{
			get
			{
				return font3dCurrent.fontMetrics;
			}
			
		}
		public System.Drawing.Image ScreenImage
		{
			get
			{
				return platform.imagePixelBuffer;
			}
			
		}
		public bool Specular
		{
			get
			{
				return Shade3D.Specular;
			}
			
			set
			{
				Shade3D.Specular = value;
			}
			
		}
		public int SpecularPower
		{
			set
			{
				Shade3D.SpecularPower = value;
			}
			
		}
		public int AmbientPercent
		{
			set
			{
				Shade3D.AmbientPercent = value;
			}
			
		}
		public int DiffusePercent
		{
			set
			{
				Shade3D.DiffusePercent = value;
			}
			
		}
		public int SpecularPercent
		{
			set
			{
				Shade3D.SpecularPercent = value;
			}
			
		}
		public float LightsourceZ
		{
			set
			{
				Shade3D.LightsourceZ = value;
			}
			
		}
		public Vector3f[] GeodesicVertexVectors
		{
			get
			{
				return Geodesic3D.VertexVectors;
			}
			
		}
		public Vector3f[] TransformedVertexVectors
		{
			get
			{
				return normix3d.TransformedVectors;
			}
			
		}
		
		internal Platform3D platform;
		internal Line3D line3d;
		internal Circle3D circle3d;
		internal Sphere3D sphere3d;
		internal Triangle3D triangle3d;
		internal Cylinder3D cylinder3d;
		internal Hermite3D hermite3d;
		internal Geodesic3D geodesic3d;
		internal Normix3D normix3d;
		
		public const int HIGHEST_GEODESIC_LEVEL = 3;
		
		internal bool isFullSceneAntialiasingEnabled;
		internal bool antialiasThisFrame;
		
		internal bool inGreyscaleMode;
		internal sbyte[] anaglyphChannelBytes;
		
		internal bool tPaintingInProgress;
		
		internal int windowWidth, windowHeight;
		internal int width, height;
		internal int slab, depth;
		internal int xLast, yLast;
		internal int[] pbuf;
		internal int[] zbuf;
		
		internal int clipX;
		internal int clipY;
		internal int clipWidth;
		internal int clipHeight;
		
		internal short colixCurrent;
		internal int[] shadesCurrent;
		internal int argbCurrent;
		internal bool isTranslucent;
		internal int argbNoisyUp, argbNoisyDn;
		
		internal Font3D font3dCurrent;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'ZBUFFER_BACKGROUND '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_NOTE: The initialization of  'ZBUFFER_BACKGROUND' was moved to static method 'org.jmol.g3d.Graphics3D'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		internal static readonly int ZBUFFER_BACKGROUND;
		
		/// <summary> Allocates a g3d object
		/// 
		/// </summary>
		/// <param name="awtComponent">the java.awt.Component where the image will be drawn
		/// </param>
		public Graphics3D(System.Windows.Forms.Control awtComponent)
		{
			platform = Platform3D.createInstance(awtComponent);
			//    Font3D.initialize(platform);
			this.line3d = new Line3D(this);
			this.circle3d = new Circle3D(this);
			this.sphere3d = new Sphere3D(this);
			this.triangle3d = new Triangle3D(this);
			this.cylinder3d = new Cylinder3D(this);
			this.hermite3d = new Hermite3D(this);
			this.geodesic3d = new Geodesic3D(this);
			this.normix3d = new Normix3D(this);
			//    setFontOfSize(13);
		}
		
		/// <summary> Sets the window size. This will be smaller than the
		/// rendering size if FullSceneAntialiasing is enabled
		/// 
		/// </summary>
		/// <param name="windowWidth">Window width
		/// </param>
		/// <param name="windowHeight">Window height
		/// </param>
		/// <param name="enableFullSceneAntialiasing">currently not in production
		/// </param>
		public void  setWindowSize(int windowWidth, int windowHeight, bool enableFullSceneAntialiasing)
		{
			if (this.windowWidth == windowWidth && this.windowHeight == windowHeight && enableFullSceneAntialiasing == isFullSceneAntialiasingEnabled)
				return ;
			this.windowWidth = windowWidth;
			this.windowHeight = windowHeight;
			isFullSceneAntialiasingEnabled = enableFullSceneAntialiasing;
			width = - 1; height = - 1;
			pbuf = null;
			zbuf = null;
			platform.releaseBuffers();
		}
		
		/// <summary> is full scene / oversampling antialiasing in effect
		/// 
		/// </summary>
		/// <returns> the answer
		/// </returns>
		public bool fullSceneAntialiasRendering()
		{
			return false;
		}
		
		/// <summary> Return a greyscale rgb value 0-FF using NTSC color luminance algorithm
		/// <p>
		/// the alpha component is set to 0xFF. If you want a value in the
		/// range 0-255 then & the result with 0xFF;
		/// 
		/// </summary>
		/// <param name="rgb">the rgb value
		/// </param>
		/// <returns> a grayscale value in the range 0 - 255 decimal
		/// </returns>
		public static int calcGreyscaleRgbFromRgb(int rgb)
		{
			int grey = ((2989 * ((rgb >> 16) & 0xFF)) + (5870 * ((rgb >> 8) & 0xFF)) + (1140 * (rgb & 0xFF)) + 5000) / 10000;
			int greyRgb = (grey << 16) | (grey << 8) | grey | unchecked((int) 0xFF000000);
			return greyRgb;
		}
		
		/// <summary> clipping from the front and the back
		/// <p>
		/// the plane is defined as a percentage from the back of the image
		/// to the front
		/// <p>
		/// For slab values:
		/// <ul>
		/// <li>100 means 100% is shown
		/// <li>75 means the back 75% is shown
		/// <li>50 means the back half is shown
		/// <li>0 means that nothing is shown
		/// </ul>
		/// <p>
		/// for depth values:
		/// <ul>
		/// <li>0 means 100% is shown
		/// <li>25 means the back 25% is <i>not</i> shown
		/// <li>50 means the back half is <i>not</i> shown
		/// <li>100 means that nothing is shown
		/// </ul>
		/// <p>
		/// </summary>
		/// <param name="slabValue">front clipping percentage [0,100]
		/// </param>
		/// <param name="depthValue">rear clipping percentage [0,100]
		/// </param>
		public void  setSlabAndDepthValues(int slabValue, int depthValue)
		{
			slab = slabValue < 0?0:(slabValue > ZBUFFER_BACKGROUND?ZBUFFER_BACKGROUND:slabValue);
			depth = depthValue < 0?0:(depthValue > ZBUFFER_BACKGROUND?ZBUFFER_BACKGROUND:depthValue);
		}
		
		/// <summary> used internally when oversampling is enabled</summary>
		private void  downSampleFullSceneAntialiasing()
		{
			int[] pbuf1 = pbuf;
			int[] pbuf4 = pbuf;
			int width4 = width;
			int offset1 = 0;
			int offset4 = 0;
			for (int i = windowHeight; --i >= 0; )
			{
				for (int j = windowWidth; --j >= 0; )
				{
					int argb;
					argb = (pbuf4[offset4] >> 2) & 0x3F3F3F3F;
					argb += ((pbuf4[offset4 + width4] >> 2) & 0x3F3F3F3F);
					++offset4;
					argb += ((pbuf4[offset4] >> 2) & 0x3F3F3F3F);
					argb += ((pbuf4[offset4 + width4] >> 2) & 0x3F3F3F3F);
					argb += ((argb & unchecked((int) 0xC0C0C0C0)) >> 6);
					argb |= unchecked((int) 0xFF000000);
					pbuf1[offset1] = argb;
					++offset1;
					++offset4;
				}
				offset4 += width4;
			}
		}
		
		public bool hasContent()
		{
			return platform.hasContent();
		}
		
		/// <summary> sets current color from colix color index</summary>
		/// <param name="colix">the color index
		/// </param>
		public void  setColix(short colix)
		{
			colixCurrent = colix;
			shadesCurrent = getShades(colix);
			argbCurrent = argbNoisyUp = argbNoisyDn = getColixArgb(colix);
			isTranslucent = (colix & TRANSLUCENT_MASK) != 0;
		}
		
		public void  setColixIntensity(short colix, int intensity)
		{
			colixCurrent = colix;
			shadesCurrent = getShades(colix);
			argbCurrent = argbNoisyUp = argbNoisyDn = shadesCurrent[intensity];
			isTranslucent = (colix & TRANSLUCENT_MASK) != 0;
		}
		
		internal void  setColorNoisy(short colix, int intensity)
		{
			colixCurrent = colix;
			int[] shades = getShades(colix);
			argbCurrent = shades[intensity];
			argbNoisyUp = shades[intensity < shadeLast?intensity + 1:shadeLast];
			argbNoisyDn = shades[intensity > 0?intensity - 1:0];
			isTranslucent = (colix & TRANSLUCENT_MASK) != 0;
		}
		
		internal int[] imageBuf = new int[0];
		
		/// <summary> draws a circle of the specified color at the specified location
		/// 
		/// </summary>
		/// <param name="colix">the color index
		/// </param>
		/// <param name="diameter">pixel diameter
		/// </param>
		/// <param name="x">center x
		/// </param>
		/// <param name="y">center y
		/// </param>
		/// <param name="z">center z
		/// </param>
		public void  drawCircleCentered(short colix, int diameter, int x, int y, int z)
		{
			if (z < slab || z > depth)
				return ;
			int r = (diameter + 1) / 2;
			setColix(colix);
			if ((x >= r && x + r < width) && (y >= r && y + r < height))
			{
				switch (diameter)
				{
					
					case 2: 
						plotPixelUnclipped(x, y - 1, z);
						plotPixelUnclipped(x - 1, y - 1, z);
						plotPixelUnclipped(x - 1, y, z);
						goto case 1;
					
					case 1: 
						plotPixelUnclipped(x, y, z);
						goto case 0;
					
					case 0: 
						break;
					
					default: 
						circle3d.plotCircleCenteredUnclipped(x, y, z, diameter);
						break;
					
				}
			}
			else
			{
				switch (diameter)
				{
					
					case 2: 
						plotPixelClipped(x, y - 1, z);
						plotPixelClipped(x - 1, y - 1, z);
						plotPixelClipped(x - 1, y, z);
						goto case 1;
					
					case 1: 
						plotPixelClipped(x, y, z);
						goto case 0;
					
					case 0: 
						break;
					
					default: 
						circle3d.plotCircleCenteredClipped(x, y, z, diameter);
						break;
					
				}
			}
		}
		
		/// <summary> draws a screened circle ... every other dot is turned on
		/// 
		/// </summary>
		/// <param name="colixFill">the color index
		/// </param>
		/// <param name="diameter">the pixel diameter
		/// </param>
		/// <param name="x">center x
		/// </param>
		/// <param name="y">center y
		/// </param>
		/// <param name="z">center z
		/// </param>
		public void  fillScreenedCircleCentered(short colixFill, int diameter, int x, int y, int z)
		{
			if (diameter == 0 || z < slab || z > depth)
				return ;
			int r = (diameter + 1) / 2;
			setColix(colixFill);
			isTranslucent = true;
			if (x >= r && x + r < width && y >= r && y + r < height)
			{
				circle3d.plotFilledCircleCenteredUnclipped(x, y, z, diameter);
				isTranslucent = false;
				circle3d.plotCircleCenteredUnclipped(x, y, z, diameter);
			}
			else
			{
				circle3d.plotFilledCircleCenteredClipped(x, y, z, diameter);
				isTranslucent = false;
				circle3d.plotCircleCenteredClipped(x, y, z, diameter);
			}
		}
		
		/// <summary> fills a solid circle
		/// 
		/// </summary>
		/// <param name="colixFill">the color index
		/// </param>
		/// <param name="diameter">the pixel diameter
		/// </param>
		/// <param name="x">center x
		/// </param>
		/// <param name="y">center y
		/// </param>
		/// <param name="z">center z
		/// </param>
		public void  fillCircleCentered(short colixFill, int diameter, int x, int y, int z)
		{
			if (diameter == 0 || z < slab || z > depth)
				return ;
			int r = (diameter + 1) / 2;
			setColix(colixFill);
			if (x >= r && x + r < width && y >= r && y + r < height)
			{
				circle3d.plotFilledCircleCenteredUnclipped(x, y, z, diameter);
			}
			else
			{
				circle3d.plotFilledCircleCenteredClipped(x, y, z, diameter);
			}
		}
		
		
		/// <summary> fills a solid sphere
		/// 
		/// </summary>
		/// <param name="colix">the color index
		/// </param>
		/// <param name="diameter">pixel count
		/// </param>
		/// <param name="x">center x
		/// </param>
		/// <param name="y">center y
		/// </param>
		/// <param name="z">center z
		/// </param>
		public void  fillSphereCentered(short colix, int diameter, int x, int y, int z)
		{
			if (diameter <= 1)
			{
				plotPixelClipped(colix, x, y, z);
			}
			else
			{
				sphere3d.render(getShades(colix), ((colix & TRANSLUCENT_MASK) != 0), diameter, x, y, z);
			}
		}
		
		/// <summary> fills a solid sphere
		/// 
		/// </summary>
		/// <param name="colix">the color index
		/// </param>
		/// <param name="diameter">pixel count
		/// </param>
		/// <param name="center">javax.vecmath.Point3i defining the center
		/// </param>
		public void  fillSphereCentered(short colix, int diameter, Point3i center)
		{
			fillSphereCentered(colix, diameter, center.x, center.y, center.z);
		}
		
		/// <summary> fills a solid sphere
		/// 
		/// </summary>
		/// <param name="colix">the color index
		/// </param>
		/// <param name="diameter">pixel count
		/// </param>
		/// <param name="center">a javax.vecmath.Point3f ... floats are casted to ints
		/// </param>
		public void  fillSphereCentered(short colix, int diameter, Point3f center)
		{
			fillSphereCentered(colix, diameter, (int) center.x, (int) center.y, (int) center.z);
		}
		
		/// <summary> draws a rectangle
		/// 
		/// </summary>
		/// <param name="colix">the color index
		/// </param>
		/// <param name="x">upper left x
		/// </param>
		/// <param name="y">upper left y
		/// </param>
		/// <param name="z">upper left z
		/// </param>
		/// <param name="width">pixel count
		/// </param>
		/// <param name="height">pixel count
		/// </param>
		public void  drawRect(short colix, int x, int y, int z, int width, int height)
		{
			setColix(colix);
			int xRight = x + width - 1;
			line3d.drawHLine(argbCurrent, isTranslucent, x, y, z, width - 1, true);
			int yBottom = y + height - 1;
			line3d.drawVLine(argbCurrent, isTranslucent, x, y, z, height - 1, true);
			line3d.drawVLine(argbCurrent, isTranslucent, xRight, y, z, height - 1, true);
			line3d.drawHLine(argbCurrent, isTranslucent, x, yBottom, z, width, true);
		}
		
		/// <summary> draws a rectangle while ignoring slab/depth clipping
		/// <p>
		/// could be useful for UI work
		/// 
		/// </summary>
		/// <param name="colix">the color index
		/// </param>
		/// <param name="x">upper left x
		/// </param>
		/// <param name="y">upper left y
		/// </param>
		/// <param name="z">upper left z
		/// </param>
		/// <param name="width">pixel count
		/// </param>
		/// <param name="height">pixel count
		/// </param>
		public void  drawRectNoSlab(short colix, int x, int y, int z, int width, int height)
		{
			setColix(colix);
			int xRight = x + width - 1;
			line3d.drawHLine(argbCurrent, isTranslucent, x, y, z, width - 1, false);
			int yBottom = y + height - 1;
			line3d.drawVLine(argbCurrent, isTranslucent, x, y, z, height - 1, false);
			line3d.drawVLine(argbCurrent, isTranslucent, xRight, y, z, height - 1, false);
			line3d.drawHLine(argbCurrent, isTranslucent, x, yBottom, z, width, false);
		}
		
		/// <summary> draws the specified string in the current font.
		/// no line wrapping
		/// 
		/// </summary>
		/// <param name="str">the string
		/// </param>
		/// <param name="colix">the color index
		/// </param>
		/// <param name="xBaseline">baseline x
		/// </param>
		/// <param name="yBaseline">baseline y
		/// </param>
		/// <param name="z">baseline z
		/// </param>
		public void  drawString(System.String str, short colix, int xBaseline, int yBaseline, int z)
		{
			drawString(str, font3dCurrent, colix, (short) 0, xBaseline, yBaseline, z);
		}
		
		/// <summary> draws the specified string in the current font.
		/// no line wrapping
		/// 
		/// </summary>
		/// <param name="str">the String
		/// </param>
		/// <param name="font3d">the Font3D
		/// </param>
		/// <param name="colix">the color index
		/// </param>
		/// <param name="xBaseline">baseline x
		/// </param>
		/// <param name="yBaseline">baseline y
		/// </param>
		/// <param name="z">baseline z
		/// </param>
		public void  drawString(System.String str, Font3D font3d, short colix, int xBaseline, int yBaseline, int z)
		{
			drawString(str, font3d, colix, (short) 0, xBaseline, yBaseline, z);
		}
		
		/// <summary> draws the specified string in the current font.
		/// no line wrapping
		/// 
		/// </summary>
		/// <param name="str">the String
		/// </param>
		/// <param name="font3d">the Font3D
		/// </param>
		/// <param name="colix">the color index
		/// </param>
		/// <param name="bgcolix">the color index of the background
		/// </param>
		/// <param name="xBaseline">baseline x
		/// </param>
		/// <param name="yBaseline">baseline y
		/// </param>
		/// <param name="z">baseline z
		/// </param>
		public void  drawString(System.String str, Font3D font3d, short colix, short bgcolix, int xBaseline, int yBaseline, int z)
		{
			//    System.out.println("Graphics3D.drawString(" + str + "," + font3d +
			//                       ", ...)");
			
			font3dCurrent = font3d;
			setColix(colix);
			if (z < slab || z > depth)
				return ;
			//    System.out.println("ready to call");
			//UPGRADE_TODO: The equivalent in .NET for method 'java.awt.FontMetrics.getAscent' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			Text3D.plot(xBaseline, yBaseline - SupportClass.GetAscent(font3d.fontMetrics), z, argbCurrent, getColixArgb(bgcolix), str, font3dCurrent, this);
			//    System.out.println("done");
		}
		
		public void  drawStringNoSlab(System.String str, Font3D font3d, short colix, short bgcolix, int xBaseline, int yBaseline, int z)
		{
			font3dCurrent = font3d;
			setColix(colix);
			//UPGRADE_TODO: The equivalent in .NET for method 'java.awt.FontMetrics.getAscent' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			Text3D.plot(xBaseline, yBaseline - SupportClass.GetAscent(font3d.fontMetrics), z, argbCurrent, getColixArgb(bgcolix), str, font3dCurrent, this);
		}
		
		public void  setFont(sbyte fid)
		{
			font3dCurrent = Font3D.getFont3D(fid);
		}
		
		public void  setFont(Font3D font3d)
		{
			font3dCurrent = font3d;
		}
		
		internal bool currentlyRendering;
		
		private void  setRectClip(int x, int y, int width, int height)
		{
			if (x < 0)
				x = 0;
			if (y < 0)
				y = 0;
			if (x + width > windowWidth)
				width = windowWidth - x;
			if (y + height > windowHeight)
				height = windowHeight - y;
			clipX = x;
			clipY = y;
			clipWidth = width;
			clipHeight = height;
			if (antialiasThisFrame)
			{
				clipX *= 2;
				clipY *= 2;
				clipWidth *= 2;
				clipHeight *= 2;
			}
		}
		
		
		// 3D specific routines
		public void  beginRendering(int clipX, int clipY, int clipWidth, int clipHeight, Matrix3f rotationMatrix, bool antialiasThisFrame)
		{
			if (currentlyRendering)
				endRendering();
			normix3d.setRotationMatrix(rotationMatrix);
			antialiasThisFrame &= isFullSceneAntialiasingEnabled;
			this.antialiasThisFrame = antialiasThisFrame;
			currentlyRendering = true;
			if (pbuf == null)
			{
				platform.allocateBuffers(windowWidth, windowHeight, isFullSceneAntialiasingEnabled);
				pbuf = platform.pBuffer;
				zbuf = platform.zBuffer;
				width = windowWidth;
				xLast = width - 1;
				height = windowHeight;
				yLast = height - 1;
			}
			width = windowWidth;
			height = windowHeight;
			if (antialiasThisFrame)
			{
				width *= 2;
				height *= 2;
			}
			xLast = width - 1;
			yLast = height - 1;
			setRectClip(clipX, clipY, clipWidth, clipHeight);
			platform.obtainScreenBuffer();
		}
		
		public void  endRendering()
		{
			if (currentlyRendering)
			{
				if (antialiasThisFrame)
					downSampleFullSceneAntialiasing();
				platform.notifyEndOfRendering();
				currentlyRendering = false;
			}
		}
		
		public void  snapshotAnaglyphChannelBytes()
		{
			if (currentlyRendering)
				throw new System.NullReferenceException();
			if (anaglyphChannelBytes == null || anaglyphChannelBytes.Length != pbuf.Length)
				anaglyphChannelBytes = new sbyte[pbuf.Length];
			for (int i = pbuf.Length; --i >= 0; )
				anaglyphChannelBytes[i] = (sbyte) pbuf[i];
		}
		
		public void  applyBlueOrGreenAnaglyph(bool blueChannel)
		{
			int shiftCount = blueChannel?0:8;
			for (int i = pbuf.Length; --i >= 0; )
				pbuf[i] = ((pbuf[i] & unchecked((int) 0xFFFF0000)) | ((anaglyphChannelBytes[i] & 0x000000FF) << shiftCount));
		}
		
		public void  applyCyanAnaglyph()
		{
			for (int i = pbuf.Length; --i >= 0; )
			{
				int blueAndGreen = anaglyphChannelBytes[i] & 0x000000FF;
				int cyan = (blueAndGreen << 8) | blueAndGreen;
				pbuf[i] = pbuf[i] & unchecked((int) 0xFFFF0000) | cyan;
			}
		}
		
		public void  releaseScreenImage()
		{
			platform.clearScreenBufferThreaded();
		}
		
		public void  drawDashedLine(short colix, int run, int rise, int x1, int y1, int z1, int x2, int y2, int z2)
		{
			int argb = getColixArgb(colix);
			line3d.drawDashedLine(argb, isTranslucent, argb, isTranslucent, run, rise, x1, y1, z1, x2, y2, z2);
		}
		
		public void  drawDottedLine(short colix, int x1, int y1, int z1, int x2, int y2, int z2)
		{
			int argb = getColixArgb(colix);
			line3d.drawDashedLine(argb, isTranslucent, argb, isTranslucent, 2, 1, x1, y1, z1, x2, y2, z2);
		}
		
		public void  drawDashedLine(short colix1, short colix2, int run, int rise, int x1, int y1, int z1, int x2, int y2, int z2)
		{
			
			line3d.drawDashedLine(getColixArgb(colix1), isColixTranslucent(colix1), getColixArgb(colix2), isColixTranslucent(colix2), run, rise, x1, y1, z1, x2, y2, z2);
		}
		
		
		public void  drawLine(Point3i pointA, Point3i pointB)
		{
			line3d.drawLine(argbCurrent, isTranslucent, argbCurrent, isTranslucent, pointA.x, pointA.y, pointA.z, pointB.x, pointB.y, pointB.z);
		}
		
		public void  drawLine(short colix, Point3i pointA, Point3i pointB)
		{
			setColix(colix);
			line3d.drawLine(argbCurrent, isTranslucent, argbCurrent, isTranslucent, pointA.x, pointA.y, pointA.z, pointB.x, pointB.y, pointB.z);
		}
		
		public void  drawDottedLine(short colix, Point3i pointA, Point3i pointB)
		{
			drawDashedLine(colix, 2, 1, pointA, pointB);
		}
		
		public void  drawDashedLine(short colix, int run, int rise, Point3i pointA, Point3i pointB)
		{
			setColix(colix);
			line3d.drawDashedLine(argbCurrent, isTranslucent, argbCurrent, isTranslucent, run, rise, pointA.x, pointA.y, pointA.z, pointB.x, pointB.y, pointB.z);
		}
		
		public void  drawDashedLine(int run, int rise, int x1, int y1, int z1, int x2, int y2, int z2)
		{
			line3d.drawDashedLine(argbCurrent, isTranslucent, argbCurrent, isTranslucent, run, rise, x1, y1, z1, x2, y2, z2);
		}
		
		public void  drawLine(int x1, int y1, int z1, int x2, int y2, int z2)
		{
			line3d.drawLine(argbCurrent, isTranslucent, argbCurrent, isTranslucent, x1, y1, z1, x2, y2, z2);
		}
		
		public void  drawLine(short colix, int x1, int y1, int z1, int x2, int y2, int z2)
		{
			setColix(colix);
			line3d.drawLine(argbCurrent, isTranslucent, argbCurrent, isTranslucent, x1, y1, z1, x2, y2, z2);
		}
		
		public void  drawLine(short colix1, short colix2, int x1, int y1, int z1, int x2, int y2, int z2)
		{
			line3d.drawLine(getColixArgb(colix1), isColixTranslucent(colix1), getColixArgb(colix2), isColixTranslucent(colix2), x1, y1, z1, x2, y2, z2);
		}
		
		public void  drawPolygon4(int[] ax, int[] ay, int[] az)
		{
			drawLine(ax[0], ay[0], az[0], ax[3], ay[3], az[3]);
			for (int i = 3; --i >= 0; )
				drawLine(ax[i], ay[i], az[i], ax[i + 1], ay[i + 1], az[i + 1]);
		}
		
		public void  fillQuadrilateral(short colix, Point3f screenA, Point3f screenB, Point3f screenC, Point3f screenD)
		{
			/*
			System.out.println("fillQuad----------------");
			System.out.println("screenA="+ screenA +
			"\nscreenB=" + screenB +
			"\nscreenC=" + screenC +
			"\nscreenD=" + screenD);
			*/
			setColorNoisy(colix, calcIntensityScreen(screenA, screenB, screenC));
			fillTriangle(screenA, screenB, screenC);
			fillTriangle(screenA, screenC, screenD);
		}
		
		public void  fillTriangle(short colix, Point3i screenA, short normixA, Point3i screenB, short normixB, Point3i screenC, short normixC)
		{
			int[] t;
			t = triangle3d.ax;
			t[0] = screenA.x; t[1] = screenB.x; t[2] = screenC.x;
			t = triangle3d.ay;
			t[0] = screenA.y; t[1] = screenB.y; t[2] = screenC.y;
			t = triangle3d.az;
			t[0] = screenA.z; t[1] = screenB.z; t[2] = screenC.z;
			
			if (normixA == normixB && normixA == normixC)
			{
				setColorNoisy(colix, normix3d.getIntensity(normixA));
				triangle3d.fillTriangle(false);
			}
			else
			{
				setColix(colix);
				triangle3d.setGouraud(shadesCurrent[normix3d.getIntensity(normixA)], shadesCurrent[normix3d.getIntensity(normixB)], shadesCurrent[normix3d.getIntensity(normixC)]);
				triangle3d.fillTriangle(true);
			}
		}
		
		public void  fillTriangle(Point3i screenA, short colixA, short normixA, Point3i screenB, short colixB, short normixB, Point3i screenC, short colixC, short normixC)
		{
			int[] t;
			t = triangle3d.ax;
			t[0] = screenA.x; t[1] = screenB.x; t[2] = screenC.x;
			t = triangle3d.ay;
			t[0] = screenA.y; t[1] = screenB.y; t[2] = screenC.y;
			t = triangle3d.az;
			t[0] = screenA.z; t[1] = screenB.z; t[2] = screenC.z;
			
			if (normixA == normixB && normixA == normixC && colixA == colixB && colixA == colixC)
			{
				setColorNoisy(colixA, normix3d.getIntensity(normixA));
				triangle3d.fillTriangle(false);
			}
			else
			{
				triangle3d.setGouraud(getShades(colixA)[normix3d.getIntensity(normixA)], getShades(colixB)[normix3d.getIntensity(normixB)], getShades(colixC)[normix3d.getIntensity(normixC)]);
				int translucentCount = 0;
				if (isColixTranslucent(colixA))
					++translucentCount;
				if (isColixTranslucent(colixB))
					++translucentCount;
				if (isColixTranslucent(colixC))
					++translucentCount;
				isTranslucent = translucentCount >= 2;
				triangle3d.fillTriangle(true);
			}
		}
		
		public void  fillTriangle(short colix, Point3i screenA, Point3i screenB, Point3i screenC)
		{
			calcSurfaceShade(colix, screenA, screenB, screenC);
			int[] t;
			t = triangle3d.ax;
			t[0] = screenA.x; t[1] = screenB.x; t[2] = screenC.x;
			t = triangle3d.ay;
			t[0] = screenA.y; t[1] = screenB.y; t[2] = screenC.y;
			t = triangle3d.az;
			t[0] = screenA.z; t[1] = screenB.z; t[2] = screenC.z;
			
			triangle3d.fillTriangle(false);
		}
		
		public void  fillTriangle(short colix, short normix, int xScreenA, int yScreenA, int zScreenA, int xScreenB, int yScreenB, int zScreenB, int xScreenC, int yScreenC, int zScreenC)
		{
			setColorNoisy(colix, normix3d.getIntensity(normix));
			int[] t;
			t = triangle3d.ax;
			t[0] = xScreenA; t[1] = xScreenB; t[2] = xScreenC;
			t = triangle3d.ay;
			t[0] = yScreenA; t[1] = yScreenB; t[2] = yScreenC;
			t = triangle3d.az;
			t[0] = zScreenA; t[1] = zScreenB; t[2] = zScreenC;
			triangle3d.fillTriangle(false);
		}
		
		public void  fillTriangle(short colix, Point3f screenA, Point3f screenB, Point3f screenC)
		{
			setColorNoisy(colix, calcIntensityScreen(screenA, screenB, screenC));
			fillTriangle(screenA, screenB, screenC);
		}
		
		public void  fillQuadrilateral(short colix, Point3i screenA, Point3i screenB, Point3i screenC, Point3i screenD)
		{
			fillTriangle(colix, screenA, screenB, screenC);
			fillTriangle(colix, screenA, screenC, screenD);
		}
		
		public void  fillQuadrilateral(short colix, Point3i screenA, short normixA, Point3i screenB, short normixB, Point3i screenC, short normixC, Point3i screenD, short normixD)
		{
			fillTriangle(colix, screenA, normixA, screenB, normixB, screenC, normixC);
			fillTriangle(colix, screenA, normixA, screenC, normixC, screenD, normixD);
		}
		
		public void  fillQuadrilateral(Point3i screenA, short colixA, short normixA, Point3i screenB, short colixB, short normixB, Point3i screenC, short colixC, short normixC, Point3i screenD, short colixD, short normixD)
		{
			fillTriangle(screenA, colixA, normixA, screenB, colixB, normixB, screenC, colixC, normixC);
			fillTriangle(screenA, colixA, normixA, screenC, colixC, normixC, screenD, colixD, normixD);
		}
		
		public void  fillTriangle(Point3i screenA, Point3i screenB, Point3i screenC)
		{
			
			int[] t;
			t = triangle3d.ax;
			t[0] = screenA.x; t[1] = screenB.x; t[2] = screenC.x;
			t = triangle3d.ay;
			t[0] = screenA.y; t[1] = screenB.y; t[2] = screenC.y;
			t = triangle3d.az;
			t[0] = screenA.z; t[1] = screenB.z; t[2] = screenC.z;
			
			triangle3d.fillTriangle(false);
		}
		
		public void  fillTriangle(Point3f screenA, Point3f screenB, Point3f screenC)
		{
			int[] t;
			t = triangle3d.ax;
			t[0] = (int) screenA.x; t[1] = (int) screenB.x; t[2] = (int) screenC.x;
			t = triangle3d.ay;
			t[0] = (int) screenA.y; t[1] = (int) screenB.y; t[2] = (int) screenC.y;
			t = triangle3d.az;
			t[0] = (int) screenA.z; t[1] = (int) screenB.z; t[2] = (int) screenC.z;
			
			triangle3d.fillTriangle(false);
		}
		
		internal int intensity = 0;
		
		internal void  diff(Vector3f v, Point3i s1, Point3i s2)
		{
			v.x = s1.x - s2.x;
			v.y = s1.y - s2.y;
			v.z = s1.z - s2.z;
		}
		
		public void  calcSurfaceShade(short colix, Point3i screenA, Point3i screenB, Point3i screenC)
		{
			diff(vectorAB, screenB, screenA);
			diff(vectorAC, screenC, screenA);
			vectorNormal.cross(vectorAB, vectorAC);
			int intensity = vectorNormal.z >= 0?calcIntensity(- vectorNormal.x, - vectorNormal.y, vectorNormal.z):calcIntensity(vectorNormal.x, vectorNormal.y, - vectorNormal.z);
			if (intensity > intensitySpecularSurfaceLimit)
				intensity = intensitySpecularSurfaceLimit;
			setColorNoisy(colix, intensity);
		}
		
		public void  drawfillTriangle(short colix, int xA, int yA, int zA, int xB, int yB, int zB, int xC, int yC, int zC)
		{
			setColix(colix);
			int argb = argbCurrent;
			line3d.drawLine(argb, isTranslucent, argb, isTranslucent, xA, yA, zA, xB, yB, zB);
			line3d.drawLine(argb, isTranslucent, argb, isTranslucent, xA, yA, zA, xC, yC, zC);
			line3d.drawLine(argb, isTranslucent, argb, isTranslucent, xB, yB, zB, xC, yC, zC);
			int[] t;
			t = triangle3d.ax;
			t[0] = xA; t[1] = xB; t[2] = xC;
			t = triangle3d.ay;
			t[0] = yA; t[1] = yB; t[2] = yC;
			t = triangle3d.az;
			t[0] = zA; t[1] = zB; t[2] = zC;
			
			triangle3d.fillTriangle(false);
		}
		
		public void  fillTriangle(short colix, bool translucent, int xA, int yA, int zA, int xB, int yB, int zB, int xC, int yC, int zC)
		{
			/*
			System.out.println("fillTriangle:" + xA + "," + yA + "," + zA + "->" +
			xB + "," + yB + "," + zB + "->" +
			xC + "," + yC + "," + zC);
			*/
			setColix(colix);
			int[] t;
			t = triangle3d.ax;
			t[0] = xA; t[1] = xB; t[2] = xC;
			t = triangle3d.ay;
			t[0] = yA; t[1] = yB; t[2] = yC;
			t = triangle3d.az;
			t[0] = zA; t[1] = zB; t[2] = zC;
			
			triangle3d.fillTriangle(false);
		}
		
		/*
		final Point3i warrenA = new Point3i();
		final Point3i warrenB = new Point3i();
		final Point3i warrenC = new Point3i();
		
		public void fillTriangleWarren(int argb,
		int xA, int yA, int zA,
		int xB, int yB, int zB,
		int xC, int yC, int zC) {
		warrenA.x = xA; warrenA.y = yA; warrenA.z = zA;
		warrenB.x = xB; warrenB.y = yB; warrenB.z = zB;
		warrenC.x = xC; warrenC.y = yC; warrenC.z = zC;
		
		short colix = getColix(argb);
		
		calcSurfaceShade(colix, false, warrenA, warrenB, warrenC);
		
		int[] t;
		t = triangle3d.ax;
		t[0] = xA; t[1] = xB; t[2] = xC;
		t = triangle3d.ay;
		t[0] = yA; t[1] = yB; t[2] = yC;
		t = triangle3d.az;
		t[0] = zA; t[1] = zB; t[2] = zC;
		
		triangle3d.fillTriangle(false, false);
		}
		*/
		
		public void  drawTriangle(short colix, int xA, int yA, int zA, int xB, int yB, int zB, int xC, int yC, int zC)
		{
			/*
			System.out.println("drawTriangle:" + xA + "," + yA + "," + zA + "->" +
			xB + "," + yB + "," + zB + "->" +
			xC + "," + yC + "," + zC);
			*/
			setColix(colix);
			drawLine(xA, yA, zA, xB, yB, zB);
			drawLine(xA, yA, zA, xC, yC, zC);
			drawLine(xB, yB, zB, xC, yC, zC);
		}
		
		public void  drawCylinderTriangle(short colix, int xA, int yA, int zA, int xB, int yB, int zB, int xC, int yC, int zC, int diameter)
		{
			fillCylinder(colix, colix, Graphics3D.ENDCAPS_SPHERICAL, diameter, xA, yA, zA, xB, yB, zB);
			fillCylinder(colix, colix, Graphics3D.ENDCAPS_SPHERICAL, diameter, xA, yA, zA, xC, yC, zC);
			fillCylinder(colix, colix, Graphics3D.ENDCAPS_SPHERICAL, diameter, xB, yB, zB, xC, yC, zC);
		}
		
		public void  drawTriangle(short colix, Point3i screenA, Point3i screenB, Point3i screenC)
		{
			drawTriangle(colix, screenA.x, screenA.y, screenA.z, screenB.x, screenB.y, screenB.z, screenC.x, screenC.y, screenC.z);
		}
		
		public void  drawQuadrilateral(short colix, Point3i screenA, Point3i screenB, Point3i screenC, Point3i screenD)
		{
			setColix(colix);
			drawLine(screenA, screenB);
			drawLine(screenB, screenC);
			drawLine(screenC, screenD);
			drawLine(screenD, screenA);
		}
		
		public const sbyte ENDCAPS_NONE = 0;
		public const sbyte ENDCAPS_OPEN = 1;
		public const sbyte ENDCAPS_FLAT = 2;
		public const sbyte ENDCAPS_SPHERICAL = 3;
		
		public void  fillCylinder(short colixA, short colixB, sbyte endcaps, int diameter, int xA, int yA, int zA, int xB, int yB, int zB)
		{
			cylinder3d.render(colixA, colixB, endcaps, diameter, xA, yA, zA, xB, yB, zB);
		}
		
		public void  fillCylinder(short colix, sbyte endcaps, int diameter, int xA, int yA, int zA, int xB, int yB, int zB)
		{
			cylinder3d.render(colix, colix, endcaps, diameter, xA, yA, zA, xB, yB, zB);
		}
		
		public void  fillCylinder(short colix, sbyte endcaps, int diameter, Point3i screenA, Point3i screenB)
		{
			cylinder3d.render(colix, colix, endcaps, diameter, screenA.x, screenA.y, screenA.z, screenB.x, screenB.y, screenB.z);
		}
		
		public void  fillCone(short colix, sbyte endcap, int diameter, int xBase, int yBase, int zBase, int xTip, int yTip, int zTip)
		{
			cylinder3d.renderCone(colix, endcap, diameter, xBase, yBase, zBase, xTip, yTip, zTip);
		}
		
		public void  fillCone(short colix, sbyte endcap, int diameter, Point3i screenBase, Point3i screenTip)
		{
			cylinder3d.renderCone(colix, endcap, diameter, screenBase.x, screenBase.y, screenBase.z, screenTip.x, screenTip.y, screenTip.z);
		}
		
		public void  fillHermite(short colix, int tension, int diameterBeg, int diameterMid, int diameterEnd, Point3i s0, Point3i s1, Point3i s2, Point3i s3)
		{
			hermite3d.render(true, colix, tension, diameterBeg, diameterMid, diameterEnd, s0, s1, s2, s3);
		}
		
		public void  drawHermite(short colix, int tension, Point3i s0, Point3i s1, Point3i s2, Point3i s3)
		{
			hermite3d.render(false, colix, tension, 0, 0, 0, s0, s1, s2, s3);
		}
		
		public void  drawHermite(bool fill, bool border, short colix, int tension, Point3i s0, Point3i s1, Point3i s2, Point3i s3, Point3i s4, Point3i s5, Point3i s6, Point3i s7)
		{
			hermite3d.render2(fill, border, colix, tension, s0, s1, s2, s3, s4, s5, s6, s7);
		}
		
		public void  fillRect(short colix, int x, int y, int z, int widthFill, int heightFill)
		{
			setColix(colix);
			if (x < 0)
			{
				widthFill += x;
				if (widthFill <= 0)
					return ;
				x = 0;
			}
			if (x + widthFill > width)
			{
				widthFill = width - x;
				if (widthFill == 0)
					return ;
			}
			if (y < 0)
			{
				heightFill += y;
				if (heightFill <= 0)
					return ;
				y = 0;
			}
			if (y + heightFill > height)
				heightFill = height - y;
			while (--heightFill >= 0)
				plotPixelsUnclipped(widthFill, x, y++, z);
		}
		
		public void  drawPixel(Point3i point)
		{
			plotPixelClipped(point);
		}
		
		public void  drawPixel(int x, int y, int z)
		{
			plotPixelClipped(x, y, z);
		}
		
		public void  drawPixel(Point3i point, int normix)
		{
			plotPixelClipped(shadesCurrent[normix3d.intensities[normix]], point.x, point.y, point.z);
		}
		
		/* ***************************************************************
		* the plotting routines
		* ***************************************************************/
		
		
		internal void  plotPixelClipped(int x, int y, int z)
		{
			if (x < 0 || x >= width || y < 0 || y >= height || z < slab || z > depth)
				return ;
			int offset = y * width + x;
			if (z < zbuf[offset])
			{
				zbuf[offset] = z;
				pbuf[offset] = argbCurrent;
			}
		}
		
		internal void  plotPixelClipped(Point3i screen)
		{
			int x = screen.x;
			if (x < 0 || x >= width)
				return ;
			int y = screen.y;
			if (y < 0 || y >= height)
				return ;
			int z = screen.z;
			if (z < slab || z > depth)
				return ;
			int offset = y * width + x;
			if (z < zbuf[offset])
			{
				zbuf[offset] = z;
				pbuf[offset] = argbCurrent;
			}
		}
		
		internal void  plotPixelClipped(int argb, int x, int y, int z)
		{
			if (x < 0 || x >= width || y < 0 || y >= height || z < slab || z > depth)
				return ;
			int offset = y * width + x;
			if (z < zbuf[offset])
			{
				zbuf[offset] = z;
				pbuf[offset] = argb;
			}
		}
		
		internal void  plotPixelClippedNoSlab(int argb, int x, int y, int z)
		{
			if (x < 0 || x >= width || y < 0 || y >= height)
				return ;
			int offset = y * width + x;
			if (z < zbuf[offset])
			{
				zbuf[offset] = z;
				pbuf[offset] = argb;
			}
		}
		
		internal void  plotPixelClipped(int argb, bool isTranslucent, int x, int y, int z)
		{
			if (x < 0 || x >= width || y < 0 || y >= height || z < slab || z > depth)
				return ;
			if (isTranslucent && ((x ^ y) & 1) != 0)
				return ;
			int offset = y * width + x;
			if (z < zbuf[offset])
			{
				zbuf[offset] = z;
				pbuf[offset] = argb;
			}
		}
		
		internal void  plotPixelClipped(short colix, int x, int y, int z)
		{
			if (x < 0 || x >= width || y < 0 || y >= height || z < slab || z > depth)
				return ;
			int offset = y * width + x;
			if (z < zbuf[offset])
			{
				zbuf[offset] = z;
				pbuf[offset] = getColixArgb(colix);
			}
		}
		
		internal void  plotPixelUnclipped(int x, int y, int z)
		{
			int offset = y * width + x;
			if (z < zbuf[offset])
			{
				zbuf[offset] = z;
				pbuf[offset] = argbCurrent;
			}
		}
		
		internal void  plotPixelUnclipped(int argb, int x, int y, int z)
		{
			int offset = y * width + x;
			if (z < zbuf[offset])
			{
				zbuf[offset] = z;
				pbuf[offset] = argb;
			}
		}
		
		internal void  plotPixelsClipped(int count, int x, int y, int z)
		{
			if (y < 0 || y >= height || x >= width || z < slab || z > depth)
				return ;
			if (x < 0)
			{
				count += x; // x is negative, so this is subtracting -x
				x = 0;
			}
			if (count + x > width)
				count = width - x;
			if (count <= 0)
				return ;
			int offsetPbuf = y * width + x;
			int offsetMax = offsetPbuf + count;
			int step = 1;
			if (isTranslucent)
			{
				step = 2;
				if (((x ^ y) & 1) != 0)
					++offsetPbuf;
			}
			while (offsetPbuf < offsetMax)
			{
				if (z < zbuf[offsetPbuf])
				{
					zbuf[offsetPbuf] = z;
					pbuf[offsetPbuf] = argbCurrent;
				}
				offsetPbuf += step;
			}
		}
		
		internal void  plotPixelsClipped(int count, int x, int y, int zAtLeft, int zPastRight, Rgb16 rgb16Left, Rgb16 rgb16Right)
		{
			//    System.out.print("plotPixelsClipped z values:");
			/*
			System.out.println("plotPixelsClipped count=" + count + "x,y,z=" +
			x + "," + y + "," + zAtLeft + " -> " + zPastRight);
			*/
			if (count <= 0 || y < 0 || y >= height || x >= width || (zAtLeft < slab && zPastRight < slab) || (zAtLeft > depth && zPastRight > depth))
				return ;
			int seed = (x << 16) + (y << 1) ^ 0x33333333;
			// scale the z coordinates;
			int zScaled = (zAtLeft << 10) + (1 << 9);
			int dz = zPastRight - zAtLeft;
			int roundFactor = count / 2;
			int zIncrementScaled = ((dz << 10) + (dz >= 0?roundFactor:- roundFactor)) / count;
			if (x < 0)
			{
				x = - x;
				zScaled += zIncrementScaled * x;
				count -= x;
				if (count <= 0)
					return ;
				x = 0;
			}
			if (count + x > width)
				count = width - x;
			// when screening 0,0 should be turned ON
			// the first time through this will get flipped to true
			bool flipflop = ((x ^ y) & 1) != 0;
			int offsetPbuf = y * width + x;
			if (rgb16Left == null)
			{
				while (--count >= 0)
				{
					if (!isTranslucent || (flipflop = !flipflop))
					{
						int z = zScaled >> 10;
						if (z >= slab && z <= depth && z < zbuf[offsetPbuf])
						{
							zbuf[offsetPbuf] = z;
							seed = ((seed << 16) + (seed << 1) + seed) & 0x7FFFFFFF;
							int bits = (seed >> 16) & 0x07;
							pbuf[offsetPbuf] = (bits == 0?argbNoisyDn:(bits == 1?argbNoisyUp:argbCurrent));
						}
					}
					++offsetPbuf;
					zScaled += zIncrementScaled;
				}
			}
			else
			{
				int rScaled = rgb16Left.rScaled << 8;
				int rIncrement = ((rgb16Right.rScaled - rgb16Left.rScaled) << 8) / count;
				int gScaled = rgb16Left.gScaled;
				int gIncrement = (rgb16Right.gScaled - gScaled) / count;
				int bScaled = rgb16Left.bScaled;
				int bIncrement = (rgb16Right.bScaled - bScaled) / count;
				while (--count >= 0)
				{
					if (!isTranslucent || (flipflop = !flipflop))
					{
						int z = zScaled >> 10;
						if (z >= slab && z <= depth && z < zbuf[offsetPbuf])
						{
							zbuf[offsetPbuf] = z;
							pbuf[offsetPbuf] = (unchecked((int) 0xFF000000) | (rScaled & 0xFF0000) | (gScaled & 0xFF00) | ((bScaled >> 8) & 0xFF));
						}
					}
					++offsetPbuf;
					zScaled += zIncrementScaled;
					rScaled += rIncrement;
					gScaled += gIncrement;
					bScaled += bIncrement;
				}
			}
		}
		
		internal const bool ENABLE_GOURAUD_STATS = true;
		internal static int totalGouraud;
		internal static int shortCircuitGouraud;
		
		internal void  plotPixelsUnclipped(int count, int x, int y, int zAtLeft, int zPastRight, Rgb16 rgb16Left, Rgb16 rgb16Right)
		{
			if (count <= 0)
				return ;
			int seed = (x << 16) + (y << 1) ^ 0x33333333;
			// scale the z coordinates;
			int zScaled = (zAtLeft << 10) + (1 << 9);
			int dz = zPastRight - zAtLeft;
			int roundFactor = count / 2;
			int zIncrementScaled = ((dz << 10) + (dz >= 0?roundFactor:- roundFactor)) / count;
			int offsetPbuf = y * width + x;
			if (rgb16Left == null)
			{
				if (!isTranslucent)
				{
					while (--count >= 0)
					{
						int z = zScaled >> 10;
						if (z < zbuf[offsetPbuf])
						{
							zbuf[offsetPbuf] = z;
							seed = ((seed << 16) + (seed << 1) + seed) & 0x7FFFFFFF;
							int bits = (seed >> 16) & 0x07;
							pbuf[offsetPbuf] = (bits == 0?argbNoisyDn:(bits == 1?argbNoisyUp:argbCurrent));
						}
						++offsetPbuf;
						zScaled += zIncrementScaled;
					}
				}
				else
				{
					bool flipflop = ((x ^ y) & 1) != 0;
					while (--count >= 0)
					{
						flipflop = !flipflop;
						if (flipflop)
						{
							int z = zScaled >> 10;
							if (z < zbuf[offsetPbuf])
							{
								zbuf[offsetPbuf] = z;
								seed = ((seed << 16) + (seed << 1) + seed) & 0x7FFFFFFF;
								int bits = (seed >> 16) & 0x07;
								pbuf[offsetPbuf] = (bits == 0?argbNoisyDn:(bits == 1?argbNoisyUp:argbCurrent));
							}
						}
						++offsetPbuf;
						zScaled += zIncrementScaled;
					}
				}
			}
			else
			{
				bool flipflop = ((x ^ y) & 1) != 0;
				if (ENABLE_GOURAUD_STATS)
				{
					++totalGouraud;
					int i = count;
					int j = offsetPbuf;
					int zMin = zAtLeft < zPastRight?zAtLeft:zPastRight;
					
					if (!isTranslucent)
					{
						for (; zbuf[j] < zMin; ++j)
							if (--i == 0)
							{
								if ((++shortCircuitGouraud % 100000) == 0)
									System.Console.Out.WriteLine("totalGouraud=" + totalGouraud + " shortCircuitGouraud=" + shortCircuitGouraud + " %=" + (100.0 * shortCircuitGouraud / totalGouraud));
								return ;
							}
					}
					else
					{
						if (flipflop)
						{
							++j;
							if (--i == 0)
								return ;
						}
						for (; zbuf[j] < zMin; j += 2)
						{
							i -= 2;
							if (i <= 0)
							{
								if ((++shortCircuitGouraud % 100000) == 0)
									System.Console.Out.WriteLine("totalGouraud=" + totalGouraud + " shortCircuitGouraud=" + shortCircuitGouraud + " %=" + (100.0 * shortCircuitGouraud / totalGouraud));
								return ;
							}
						}
					}
				}
				
				int rScaled = rgb16Left.rScaled << 8;
				int rIncrement = ((rgb16Right.rScaled - rgb16Left.rScaled) << 8) / count;
				int gScaled = rgb16Left.gScaled;
				int gIncrement = (rgb16Right.gScaled - gScaled) / count;
				int bScaled = rgb16Left.bScaled;
				int bIncrement = (rgb16Right.bScaled - bScaled) / count;
				while (--count >= 0)
				{
					if (!isTranslucent || (flipflop = !flipflop))
					{
						int z = zScaled >> 10;
						if (z < zbuf[offsetPbuf])
						{
							zbuf[offsetPbuf] = z;
							pbuf[offsetPbuf] = (unchecked((int) 0xFF000000) | (rScaled & 0xFF0000) | (gScaled & 0xFF00) | ((bScaled >> 8) & 0xFF));
						}
					}
					++offsetPbuf;
					zScaled += zIncrementScaled;
					rScaled += rIncrement;
					gScaled += gIncrement;
					bScaled += bIncrement;
				}
			}
		}
		
		internal void  plotPixelsUnclipped(int count, int x, int y, int z)
		{
			int offsetPbuf = y * width + x;
			if (!isTranslucent)
			{
				while (--count >= 0)
				{
					if (z < zbuf[offsetPbuf])
					{
						zbuf[offsetPbuf] = z;
						pbuf[offsetPbuf] = argbCurrent;
					}
					++offsetPbuf;
				}
			}
			else
			{
				int offsetMax = offsetPbuf + count;
				if (((x ^ y) & 1) != 0)
					if (++offsetPbuf == offsetMax)
						return ;
				do 
				{
					if (z < zbuf[offsetPbuf])
					{
						zbuf[offsetPbuf] = z;
						pbuf[offsetPbuf] = argbCurrent;
					}
					offsetPbuf += 2;
				}
				while (offsetPbuf < offsetMax);
			}
		}
		
		internal void  plotPixelsClipped(int[] pixels, int offset, int count, int x, int y, int z)
		{
			if (y < 0 || y >= height || x >= width || z < slab || z > depth)
				return ;
			if (x < 0)
			{
				count += x; // x is negative, so this is subtracting -x
				if (count < 0)
					return ;
				offset -= x; // and this is adding -x
				x = 0;
			}
			if (count + x > width)
				count = width - x;
			int offsetPbuf = y * width + x;
			while (--count >= 0)
			{
				int pixel = pixels[offset++];
				int alpha = pixel & unchecked((int) 0xFF000000);
				if (alpha >= 0x80000000)
				{
					if (z < zbuf[offsetPbuf])
					{
						zbuf[offsetPbuf] = z;
						pbuf[offsetPbuf] = pixel;
					}
				}
				++offsetPbuf;
			}
		}
		
		internal void  plotPixelsUnclipped(int[] pixels, int offset, int count, int x, int y, int z)
		{
			int offsetPbuf = y * width + x;
			while (--count >= 0)
			{
				int pixel = pixels[offset++];
				int alpha = pixel & unchecked((int) 0xFF000000);
				if ((alpha & unchecked((int) 0x80000000)) != 0)
				{
					if (z < zbuf[offsetPbuf])
					{
						zbuf[offsetPbuf] = z;
						pbuf[offsetPbuf] = pixel;
					}
				}
				++offsetPbuf;
			}
		}
		
		internal void  plotLineDelta(int[] shades1, bool isTranslucent1, int[] shades2, bool isTranslucent2, int fp8Intensity, int x, int y, int z, int dx, int dy, int dz)
		{
			if (x < 0 || x >= width || x + dx < 0 || x + dx >= width || y < 0 || y >= height || y + dy < 0 || y + dy >= height || z < slab || z + dz < slab || z > depth || z + dz > depth)
				line3d.plotLineDeltaClipped(shades1, isTranslucent1, shades2, isTranslucent2, fp8Intensity, x, y, z, dx, dy, dz);
			else
				line3d.plotLineDeltaUnclipped(shades1, isTranslucent1, shades2, isTranslucent2, fp8Intensity, x, y, z, dx, dy, dz);
		}
		
		internal void  plotLineDelta(short colixA, short colixB, int x, int y, int z, int dx, int dy, int dz)
		{
			if (x < 0 || x >= width || x + dx < 0 || x + dx >= width || y < 0 || y >= height || y + dy < 0 || y + dy >= height || z < slab || z + dz < slab || z > depth || z + dz > depth)
				line3d.plotLineDeltaClipped(getColixArgb(colixA), isColixTranslucent(colixA), getColixArgb(colixB), isColixTranslucent(colixB), x, y, z, dx, dy, dz);
			else
				line3d.plotLineDeltaUnclipped(getColixArgb(colixA), isColixTranslucent(colixA), getColixArgb(colixB), isColixTranslucent(colixB), x, y, z, dx, dy, dz);
		}
		
		internal void  plotLineDelta(int argb1, bool isTranslucent1, int argb2, bool isTranslucent2, int x, int y, int z, int dx, int dy, int dz)
		{
			if (x < 0 || x >= width || x + dx < 0 || x + dx >= width || y < 0 || y >= height || y + dy < 0 || y + dy >= height || z < slab || z + dz < slab || z > depth || z + dz > depth)
				line3d.plotLineDeltaClipped(argb1, isTranslucent1, argb2, isTranslucent2, x, y, z, dx, dy, dz);
			else
				line3d.plotLineDeltaUnclipped(argb1, isTranslucent1, argb2, isTranslucent2, x, y, z, dx, dy, dz);
		}
		
		public void  plotPoints(short colix, int count, int[] coordinates)
		{
			setColix(colix);
			int argb = argbCurrent;
			for (int i = count * 3; i > 0; )
			{
				int z = coordinates[--i];
				int y = coordinates[--i];
				int x = coordinates[--i];
				if (x < 0 || x >= width || y < 0 || y >= height || z < slab || z > depth)
					continue;
				int offset = y * width + x;
				if (z < zbuf[offset])
				{
					zbuf[offset] = z;
					pbuf[offset] = argb;
				}
			}
		}
		
		public void  plotPoints(int count, short colix, sbyte[] intensities, int[] coordinates)
		{
			int[] shades = getShades(colix);
			for (int i = count * 3, j = count - 1; i > 0; --j)
			{
				int z = coordinates[--i];
				int y = coordinates[--i];
				int x = coordinates[--i];
				if (x < 0 || x >= width || y < 0 || y >= height || z < slab || z > depth)
					continue;
				int offset = y * width + x;
				if (z < zbuf[offset])
				{
					zbuf[offset] = z;
					//        pbuf[offset] = getColixArgb(colix);
					pbuf[offset] = shades[intensities[j]];
				}
			}
		}
		
		internal void  averageOffsetArgb(int offset, int argb)
		{
			pbuf[offset] = ((((pbuf[offset] >> 1) & 0x007F7F7F) + ((argb >> 1) & unchecked((int) 0xFF7F7F7F))) | (argb & unchecked((int) 0xFF010101)));
		}
		
		/* entries 0 through 3 are reserved and are special
		TRANSLUCENT and OPAQUE are used to inherit
		the underlying color, but change the translucency
		
		Note that colors are not actually translucent. Rather,
		they are 'screened' where every-other pixel is turned
		on. 
		*/
		internal const short TRANSLUCENT_MASK = (short) (0x4000);
		//UPGRADE_NOTE: Final was removed from the declaration of 'OPAQUE_MASK '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly short OPAQUE_MASK = ~ TRANSLUCENT_MASK;
		internal static short CHANGABLE_MASK = -32768; // negative
		internal const short UNMASK_CHANGABLE_TRANSLUCENT = (short) (0x3FFF);
		
		public const short NULL_COLIX = 0;
		public const short TRANSLUCENT = 1;
		public const short OPAQUE = 2;
		public const short UNRECOGNIZED = 3;
		public const short SPECIAL_COLIX_MAX = 4;
		
		public const short BLACK = 4;
		public const short ORANGE = 5;
		public const short PINK = 6;
		public const short BLUE = 7;
		public const short WHITE = 8;
		public const short CYAN = 9;
		public const short RED = 10;
		public const short GREEN = 11;
		public const short GRAY = 12;
		public const short SILVER = 13;
		public const short LIME = 14;
		public const short MAROON = 15;
		public const short NAVY = 16;
		public const short OLIVE = 17;
		public const short PURPLE = 18;
		public const short TEAL = 19;
		public const short MAGENTA = 20;
		public const short YELLOW = 21;
		public const short HOTPINK = 22;
		public const short GOLD = 23;
		
		internal static int[] predefinedArgbs = new int[]{unchecked((int) 0xFF000000), unchecked((int) 0xFFFFA500), unchecked((int) 0xFFFFC0CB), unchecked((int) 0xFF0000FF), unchecked((int) 0xFFFFFFFF), unchecked((int) 0xFF00FFFF), unchecked((int) 0xFFFF0000), unchecked((int) 0xFF008000), unchecked((int) 0xFF808080), unchecked((int) 0xFFC0C0C0), unchecked((int) 0xFF00FF00), unchecked((int) 0xFF800000), unchecked((int) 0xFF000080), unchecked((int) 0xFF808000), unchecked((int) 0xFF800080), unchecked((int) 0xFF008080), unchecked((int) 0xFFFF00FF), unchecked((int) 0xFFFFFF00), unchecked((int) 0xFFFF69B4), unchecked((int) 0xFFFFD700)};
		
		public int getColixArgb(short colix)
		{
			if (colix < 0)
				colix = changableColixMap[colix & UNMASK_CHANGABLE_TRANSLUCENT];
			if (!inGreyscaleMode)
				return Colix.getArgb(colix);
			return Colix.getArgbGreyscale(colix);
		}
		
		public int[] getShades(short colix)
		{
			if (colix < 0)
				colix = changableColixMap[colix & UNMASK_CHANGABLE_TRANSLUCENT];
			if (!inGreyscaleMode)
				return Colix.getShades(colix);
			return Colix.getShadesGreyscale(colix);
		}
		
		public static short getChangableColixIndex(short colix)
		{
			if (colix >= 0)
				return - 1;
			return (short) (colix & UNMASK_CHANGABLE_TRANSLUCENT);
		}
		
		public static bool isColixTranslucent(short colix)
		{
			return (colix & TRANSLUCENT_MASK) != 0;
		}
		
		public static short getTranslucentColix(short colix, bool translucent)
		{
			return (short) (translucent?(colix | TRANSLUCENT_MASK):(colix & OPAQUE_MASK));
		}
		
		public static short getTranslucentColix(short colix)
		{
			return (short) (colix | TRANSLUCENT_MASK);
		}
		
		public static short getOpaqueColix(short colix)
		{
			return (short) (colix & OPAQUE_MASK);
		}
		
		public static short getColix(int argb)
		{
			return Colix.getColix(argb);
		}
		
		public short getColixMix(short colixA, short colixB)
		{
			return Colix.getColixMix(colixA >= 0?colixA:changableColixMap[colixA & UNMASK_CHANGABLE_TRANSLUCENT], colixB >= 0?colixB:changableColixMap[colixB & UNMASK_CHANGABLE_TRANSLUCENT]);
		}
		
		public static short setTranslucent(short colix, bool isTranslucent)
		{
			if (isTranslucent)
			{
				if (colix >= 0 && colix < SPECIAL_COLIX_MAX)
					return TRANSLUCENT;
				return (short) (colix | TRANSLUCENT_MASK);
			}
			if (colix >= 0 && colix < SPECIAL_COLIX_MAX)
				return OPAQUE;
			return (short) (colix & OPAQUE_MASK);
		}
		
		public static short getColix(System.String colorName)
		{
			int argb = getArgbFromString(colorName);
			if (argb != 0)
				return getColix(argb);
			if ("none".ToUpper().Equals(colorName.ToUpper()))
				return 0;
			if ("translucent".ToUpper().Equals(colorName.ToUpper()))
				return TRANSLUCENT;
			if ("opaque".ToUpper().Equals(colorName.ToUpper()))
				return OPAQUE;
			return UNRECOGNIZED;
		}
		
		public static short getColix(System.Object obj)
		{
			if (obj == null)
				return 0;
			if (obj is System.Int32)
				return getColix(((System.Int32) obj));
			if (obj is System.String)
				return getColix((System.String) obj);
			//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			System.Console.Out.WriteLine("?? getColix(" + obj + ")");
			return HOTPINK;
		}
		
		public static short inheritColix(short myColix, short parentColix)
		{
			switch (myColix)
			{
				
				case 0: 
					return parentColix;
				
				case TRANSLUCENT: 
					return (short) (parentColix | TRANSLUCENT_MASK);
				
				case OPAQUE: 
					return (short) (parentColix & OPAQUE_MASK);
				
				default: 
					return myColix;
				
			}
		}
		
		public static short inheritColix(short myColix, short parentColix, short grandParentColix)
		{
			if (myColix >= SPECIAL_COLIX_MAX)
				return myColix;
			parentColix = inheritColix(parentColix, grandParentColix);
			if (myColix == 0)
				return parentColix;
			return inheritColix(myColix, parentColix);
		}
		
		public System.String getHexColorFromIndex(short colix)
		{
			int argb = getColixArgb(colix);
			if (argb == 0)
				return null;
			System.String r = System.Convert.ToString((argb >> 16) & 0xFF, 16);
			System.String g = System.Convert.ToString((argb >> 8) & 0xFF, 16);
			System.String b = System.Convert.ToString(argb & 0xFF, 16);
			return "#" + r + g + b;
		}
		
		/// <summary>*************************************************************
		/// changable colixes
		/// give me a short ID and a color, and I will give you a colix
		/// later, you can reassign the color if you want
		/// **************************************************************
		/// </summary>
		
		internal short[] changableColixMap = new short[16];
		
		public short getChangableColix(short id, int argb)
		{
			if (id >= changableColixMap.Length)
			{
				short[] t = new short[id + 16];
				Array.Copy(changableColixMap, 0, t, 0, changableColixMap.Length);
				changableColixMap = t;
			}
			if (changableColixMap[id] == 0)
				changableColixMap[id] = getColix(argb);
			return (short) (id | CHANGABLE_MASK);
		}
		
		public void  changeColixArgb(short id, int argb)
		{
			if (id < changableColixMap.Length && changableColixMap[id] != 0)
				changableColixMap[id] = getColix(argb);
		}
		
		public void  flushShadesAndImageCaches()
		{
			Colix.flushShades();
			Sphere3D.flushImageCache();
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'shadeMax '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_NOTE: The initialization of  'shadeMax' was moved to static method 'org.jmol.g3d.Graphics3D'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		public static readonly sbyte shadeMax;
		//UPGRADE_NOTE: Final was removed from the declaration of 'shadeLast '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_NOTE: The initialization of  'shadeLast' was moved to static method 'org.jmol.g3d.Graphics3D'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		public static readonly sbyte shadeLast;
		//UPGRADE_NOTE: Final was removed from the declaration of 'shadeNormal '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_NOTE: The initialization of  'shadeNormal' was moved to static method 'org.jmol.g3d.Graphics3D'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		public static readonly sbyte shadeNormal;
		//UPGRADE_NOTE: Final was removed from the declaration of 'intensitySpecularSurfaceLimit '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_NOTE: The initialization of  'intensitySpecularSurfaceLimit' was moved to static method 'org.jmol.g3d.Graphics3D'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		public static readonly sbyte intensitySpecularSurfaceLimit;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'vectorAB '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private Vector3f vectorAB = new Vector3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'vectorAC '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private Vector3f vectorAC = new Vector3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'vectorNormal '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private Vector3f vectorNormal = new Vector3f();
		
		// these points are in screen coordinates
		public int calcIntensityScreen(Point3f screenA, Point3f screenB, Point3f screenC)
		{
			vectorAB.sub(screenB, screenA);
			vectorAC.sub(screenC, screenA);
			vectorNormal.cross(vectorAB, vectorAC);
			return (vectorNormal.z >= 0?Shade3D.calcIntensity(- vectorNormal.x, - vectorNormal.y, vectorNormal.z):Shade3D.calcIntensity(vectorNormal.x, vectorNormal.y, - vectorNormal.z));
		}
		
		
		static public int calcIntensity(float x, float y, float z)
		{
			return Shade3D.calcIntensity(x, y, z);
		}
		
		/* ***************************************************************
		* fontID stuff
		* a fontID is a byte that contains the size + the face + the style
		* ***************************************************************/
		
		public Font3D getFont3D(int fontSize)
		{
			return Font3D.getFont3D(Font3D.FONT_FACE_SANS, Font3D.FONT_STYLE_PLAIN, fontSize, platform);
		}
		
		public Font3D getFont3D(System.String fontFace, int fontSize)
		{
			return Font3D.getFont3D(Font3D.getFontFaceID(fontFace), Font3D.FONT_STYLE_PLAIN, fontSize, platform);
		}
		
		// {"Plain", "Bold", "Italic", "BoldItalic"};
		public Font3D getFont3D(System.String fontFace, System.String fontStyle, int fontSize)
		{
			return Font3D.getFont3D(Font3D.getFontFaceID(fontFace), Font3D.getFontStyleID(fontStyle), fontSize, platform);
		}
		
		public sbyte getFontFid(int fontSize)
		{
			return getFont3D(fontSize).fid;
		}
		
		public sbyte getFontFid(System.String fontFace, int fontSize)
		{
			return getFont3D(fontFace, fontSize).fid;
		}
		
		public sbyte getFontFid(System.String fontFace, System.String fontStyle, int fontSize)
		{
			return getFont3D(fontFace, fontStyle, fontSize).fid;
		}
		
		// 140 JavaScript color names
		// includes 16 official HTML 4.0 color names & values
		// plus a few extra rasmol names
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'colorNames'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly System.String[] colorNames = new System.String[]{"aliceblue", "antiquewhite", "aqua", "aquamarine", "azure", "beige", "bisque", "black", "blanchedalmond", "blue", "blueviolet", "brown", "burlywood", "cadetblue", "chartreuse", "chocolate", "coral", "cornflowerblue", "cornsilk", "crimson", "cyan", "darkblue", "darkcyan", "darkgoldenrod", "darkgray", "darkgreen", "darkkhaki", "darkmagenta", "darkolivegreen", "darkorange", "darkorchid", "darkred", "darksalmon", "darkseagreen", "darkslateblue", "darkslategray", "darkturquoise", "darkviolet", "deeppink", "deepskyblue", "dimgray", "dodgerblue", "firebrick", "floralwhite", "forestgreen", "fuchsia", "gainsboro", "ghostwhite", "gold", "goldenrod", "gray", "green", "greenyellow", "honeydew", "hotpink", "indianred", "indigo", "ivory", "khaki", "lavender", "lavenderblush", "lawngreen", "lemonchiffon", "lightblue", "lightcoral", "lightcyan", "lightgoldenrodyellow", "lightgreen", "lightgrey", "lightpink", "lightsalmon", "lightseagreen", "lightskyblue", "lightslategray", "lightsteelblue", "lightyellow", "lime", "limegreen", "linen", "magenta", "maroon", "mediumaquamarine", "mediumblue", "mediumorchid", "mediumpurple", "mediumseagreen", "mediumslateblue", "mediumspringgreen", "mediumturquoise", "mediumvioletred", "midnightblue", "mintcream", "mistyrose", "moccasin", "navajowhite", "navy", "oldlace", "olive", "olivedrab", "orange", "orangered", "orchid", "palegoldenrod", "palegreen", "paleturquoise", "palevioletred", "papayawhip", "peachpuff", "peru", "pink", "plum", "powderblue", "purple", "red", "rosybrown", "royalblue", "saddlebrown", "salmon", "sandybrown", "seagreen", "seashell", "sienna", "silver", "skyblue", "slateblue", "slategray", "snow", "springgreen", "steelblue", "tan", "teal", "thistle", "tomato", "turquoise", "violet", "wheat", "white", "whitesmoke", "yellow", "yellowgreen", "bluetint", "greenblue", "greentint", "grey", "pinktint", "redorange", "yellowtint", "pecyan", "pepurple", "pegreen", "peblue", "peviolet", "pebrown", "pepink"
			, "peyellow", "pedarkgreen", "peorange", "pelightblue", "pedarkcyan", "pedarkgray", "pewhite"};
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'colorArgbs'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly int[] colorArgbs = new int[]{unchecked((int) 0xFFF0F8FF), unchecked((int) 0xFFFAEBD7), unchecked((int) 0xFF00FFFF), unchecked((int) 0xFF7FFFD4), unchecked((int) 0xFFF0FFFF), unchecked((int) 0xFFF5F5DC), unchecked((int) 0xFFFFE4C4), unchecked((int) 0xFF000000), unchecked((int) 0xFFFFEBCD), unchecked((int) 0xFF0000FF), unchecked((int) 0xFF8A2BE2), unchecked((int) 0xFFA52A2A), unchecked((int) 0xFFDEB887), unchecked((int) 0xFF5F9EA0), unchecked((int) 0xFF7FFF00), unchecked((int) 0xFFD2691E), unchecked((int) 0xFFFF7F50), unchecked((int) 0xFF6495ED), unchecked((int) 0xFFFFF8DC), unchecked((int) 0xFFDC143C), unchecked((int) 0xFF00FFFF), unchecked((int) 0xFF00008B), unchecked((int) 0xFF008B8B), unchecked((int) 0xFFB8860B), unchecked((int) 0xFFA9A9A9), unchecked((int) 0xFF006400), unchecked((int) 0xFFBDB76B), unchecked((int) 0xFF8B008B), unchecked((int) 0xFF556B2F), unchecked((int) 0xFFFF8C00), unchecked((int) 0xFF9932CC), unchecked((int) 0xFF8B0000), unchecked((int) 0xFFE9967A), unchecked((int) 0xFF8FBC8F), unchecked((int) 0xFF483D8B), unchecked((int) 0xFF2F4F4F), unchecked((int) 0xFF00CED1), unchecked((int) 0xFF9400D3), unchecked((int) 0xFFFF1493), unchecked((int) 0xFF00BFFF), unchecked((int) 0xFF696969), unchecked((int) 0xFF1E90FF), unchecked((int) 0xFFB22222), unchecked((int) 0xFFFFFAF0), unchecked((int) 0xFF228B22), unchecked((int) 0xFFFF00FF), unchecked((int) 0xFFDCDCDC), unchecked((int) 0xFFF8F8FF), unchecked((int) 0xFFFFD700), unchecked((int) 0xFFDAA520), unchecked((int) 0xFF808080), unchecked((int) 0xFF008000), unchecked((int) 0xFFADFF2F), unchecked((int) 0xFFF0FFF0), unchecked((int) 0xFFFF69B4), unchecked((int) 0xFFCD5C5C), unchecked((int) 0xFF4B0082), unchecked((int) 0xFFFFFFF0), unchecked((int) 0xFFF0E68C), unchecked((int) 0xFFE6E6FA), unchecked((int) 0xFFFFF0F5), unchecked((int) 0xFF7CFC00), unchecked((int) 0xFFFFFACD), unchecked((int) 0xFFADD8E6), unchecked((int) 0xFFF08080), unchecked((int) 0xFFE0FFFF), unchecked((int) 0xFFFAFAD2), unchecked((int) 0xFF90EE90), unchecked((
			int) 0xFFD3D3D3), unchecked((int) 0xFFFFB6C1), unchecked((int) 0xFFFFA07A), unchecked((int) 0xFF20B2AA), unchecked((int) 0xFF87CEFA), unchecked((int) 0xFF778899), unchecked((int) 0xFFB0C4DE), unchecked((int) 0xFFFFFFE0), unchecked((int) 0xFF00FF00), unchecked((int) 0xFF32CD32), unchecked((int) 0xFFFAF0E6), unchecked((int) 0xFFFF00FF), unchecked((int) 0xFF800000), unchecked((int) 0xFF66CDAA), unchecked((int) 0xFF0000CD), unchecked((int) 0xFFBA55D3), unchecked((int) 0xFF9370DB), unchecked((int) 0xFF3CB371), unchecked((int) 0xFF7B68EE), unchecked((int) 0xFF00FA9A), unchecked((int) 0xFF48D1CC), unchecked((int) 0xFFC71585), unchecked((int) 0xFF191970), unchecked((int) 0xFFF5FFFA), unchecked((int) 0xFFFFE4E1), unchecked((int) 0xFFFFE4B5), unchecked((int) 0xFFFFDEAD), unchecked((int) 0xFF000080), unchecked((int) 0xFFFDF5E6), unchecked((int) 0xFF808000), unchecked((int) 0xFF6B8E23), unchecked((int) 0xFFFFA500), unchecked((int) 0xFFFF4500), unchecked((int) 0xFFDA70D6), unchecked((int) 0xFFEEE8AA), unchecked((int) 0xFF98FB98), unchecked((int) 0xFFAFEEEE), unchecked((int) 0xFFDB7093), unchecked((int) 0xFFFFEFD5), unchecked((int) 0xFFFFDAB9), unchecked((int) 0xFFCD853F), unchecked((int) 0xFFFFC0CB), unchecked((int) 0xFFDDA0DD), unchecked((int) 0xFFB0E0E6), unchecked((int) 0xFF800080), unchecked((int) 0xFFFF0000), unchecked((int) 0xFFBC8F8F), unchecked((int) 0xFF4169E1), unchecked((int) 0xFF8B4513), unchecked((int) 0xFFFA8072), unchecked((int) 0xFFF4A460), unchecked((int) 0xFF2E8B57), unchecked((int) 0xFFFFF5EE), unchecked((int) 0xFFA0522D), unchecked((int) 0xFFC0C0C0), unchecked((int) 0xFF87CEEB), unchecked((int) 0xFF6A5ACD), unchecked((int) 0xFF708090), unchecked((int) 0xFFFFFAFA), unchecked((int) 0xFF00FF7F), unchecked((int) 0xFF4682B4), unchecked((int) 0xFFD2B48C), unchecked((int) 0xFF008080), unchecked((int) 0xFFD8BFD8), unchecked((int) 0xFFFF6347), unchecked((int) 0xFF40E0D0), unchecked((int) 0xFFEE82EE), unchecked((int) 0xFFF5DEB3), unchecked((int) 0xFFFFFFFF), unchecked((int) 0xFFF5F5F5), unchecked((
			int) 0xFFFFFF00), unchecked((int) 0xFF9ACD32), unchecked((int) 0xFFAFD7FF), unchecked((int) 0xFF2E8B57), unchecked((int) 0xFF98FFB3), unchecked((int) 0xFF808080), unchecked((int) 0xFFFFABBB), unchecked((int) 0xFFFF4500), unchecked((int) 0xFFF6F675), unchecked((int) 0xFF00ffff), unchecked((int) 0xFFd020ff), unchecked((int) 0xFF00ff00), unchecked((int) 0xFF6060ff), unchecked((int) 0xFFff80c0), unchecked((int) 0xFFa42028), unchecked((int) 0xFFffd8d8), unchecked((int) 0xFFffff00), unchecked((int) 0xFF00c000), unchecked((int) 0xFFffb000), unchecked((int) 0xFFb0b0ff), unchecked((int) 0xFF00a0a0), unchecked((int) 0xFF606060), unchecked((int) 0xFFffffff)};
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'mapJavaScriptColors '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private static readonly System.Collections.Hashtable mapJavaScriptColors = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
		
		public static int getArgbFromString(System.String strColor)
		{
			/*
			System.out.println("ColorManager.getArgb!FromString(" + strColor + ")");
			*/
			if (strColor != null)
			{
				if (strColor.Length == 7 && strColor[0] == '#')
				{
					try
					{
						//UPGRADE_TODO: Method 'java.lang.Integer.parseInt' was converted to 'System.Convert.ToInt32' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
						int red = System.Convert.ToInt32(strColor.Substring(1, (3) - (1)), 16);
						//UPGRADE_TODO: Method 'java.lang.Integer.parseInt' was converted to 'System.Convert.ToInt32' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
						int grn = System.Convert.ToInt32(strColor.Substring(3, (5) - (3)), 16);
						//UPGRADE_TODO: Method 'java.lang.Integer.parseInt' was converted to 'System.Convert.ToInt32' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
						int blu = System.Convert.ToInt32(strColor.Substring(5, (7) - (5)), 16);
						return (unchecked((int) 0xFF000000) | (red & 0xFF) << 16 | (grn & 0xFF) << 8 | (blu & 0xFF));
					}
					catch (System.FormatException e)
					{
					}
				}
				else
				{
					System.Int32 boxedArgb = (System.Int32) mapJavaScriptColors[strColor.ToLower()];
					//UPGRADE_TODO: The 'System.Int32' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
					if (boxedArgb != null)
						return boxedArgb;
				}
			}
			return 0;
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'vAB '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f vAB = new Vector3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'vAC '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f vAC = new Vector3f();
		
		public void  calcNormalizedNormal(Point3f pointA, Point3f pointB, Point3f pointC, Vector3f vNormNorm)
		{
			vAB.sub(pointB, pointA);
			vAC.sub(pointC, pointA);
			vNormNorm.cross(vAB, vAC);
			vNormNorm.normalize();
		}
		
		public void  calcXYNormalToLine(Point3f pointA, Point3f pointB, Vector3f vNormNorm)
		{
			// vector in xy plane perpendicular to a line between two points RMH
			Vector3f axis = new Vector3f(pointA);
			axis.sub(pointB);
			float phi = axis.angle(new Vector3f(0, 1, 0));
			if (phi == 0)
			{
				vNormNorm.set_Renamed(1, 0, 0);
			}
			else
			{
				vNormNorm.cross(axis, new Vector3f(0, 1, 0));
				vNormNorm.normalize();
			}
		}
		
		public void  calcAveragePoint(Point3f pointA, Point3f pointB, Point3f pointC)
		{
			Vector3f v = new Vector3f(pointB);
			v.sub(pointA);
			v.scale(1 / 2f);
			pointC.set_Renamed(pointA);
			pointC.add(v);
		}
		
		public short getNormix(Vector3f vector)
		{
			return normix3d.getNormix(vector.x, vector.y, vector.z, Normix3D.NORMIX_GEODESIC_LEVEL);
		}
		
		public short getNormix(Vector3f vector, int geodesicLevel)
		{
			return normix3d.getNormix(vector.x, vector.y, vector.z, geodesicLevel);
		}
		
		public short getInverseNormix(Vector3f vector)
		{
			return normix3d.getNormix(- vector.x, - vector.y, - vector.z, Normix3D.NORMIX_GEODESIC_LEVEL);
		}
		
		public short getInverseNormix(short normix)
		{
			if (normix3d.inverseNormixes != null)
				return normix3d.inverseNormixes[normix];
			normix3d.calculateInverseNormixes();
			return normix3d.inverseNormixes[normix];
		}
		
		public short get2SidedNormix(Vector3f vector)
		{
			return (short) ~ normix3d.getNormix(vector.x, vector.y, vector.z, Normix3D.NORMIX_GEODESIC_LEVEL);
		}
		
		public bool isDirectedTowardsCamera(short normix)
		{
			return normix3d.isDirectedTowardsCamera(normix);
		}
		
		public short getClosestVisibleGeodesicVertexIndex(Vector3f vector, int[] visibilityBitmap, int level)
		{
			return normix3d.getVisibleNormix(vector.x, vector.y, vector.z, visibilityBitmap, level);
		}
		
		public bool isNeighborVertex(short vertex1, short vertex2, int level)
		{
			return Geodesic3D.isNeighborVertex(vertex1, vertex2, level);
		}
		
		public int getGeodesicVertexCount(int level)
		{
			return Geodesic3D.getVertexCount(level);
		}
		
		public Vector3f getNormixVector(short normix)
		{
			return normix3d.getVector(normix);
		}
		
		public int getGeodesicFaceCount(int level)
		{
			return Geodesic3D.getFaceCount(level);
		}
		
		public short[] getGeodesicFaceVertexes(int level)
		{
			return Geodesic3D.getFaceVertexes(level);
		}
		
		public short[] getGeodesicFaceNormixes(int level)
		{
			return normix3d.getFaceNormixes(level);
		}
		
		public const int GEODESIC_START_VERTEX_COUNT = 12;
		public const int GEODESIC_START_NEIGHBOR_COUNT = 5;
		
		public short[] getGeodesicNeighborVertexes(int level)
		{
			return Geodesic3D.getNeighborVertexes(level);
		}
		static Graphics3D()
		{
			ZBUFFER_BACKGROUND = Platform3D.ZBUFFER_BACKGROUND;
			{
				for (int i = 0; i < predefinedArgbs.Length; ++i)
					if (Colix.getColix(predefinedArgbs[i]) != i + SPECIAL_COLIX_MAX)
						throw new System.NullReferenceException();
			}
			shadeMax = Shade3D.shadeMax;
			shadeLast = Shade3D.shadeMax - 1;
			shadeNormal = Shade3D.shadeNormal;
			intensitySpecularSurfaceLimit = Shade3D.intensitySpecularSurfaceLimit;
			{
				for (int i = colorNames.Length; --i >= 0; )
					mapJavaScriptColors[colorNames[i]] = (System.Int32) colorArgbs[i];
			}
		}
	}
}