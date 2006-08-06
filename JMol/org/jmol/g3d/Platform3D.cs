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
	
	/// <summary><p>
	/// Specifies the API to an underlying int[] buffer of ARGB values that
	/// can be converted into an Image object and a short[] for z-buffer depth.
	/// </p>
	/// 
	/// </summary>
	/// <author>  Miguel, miguel@jmol.org
	/// </author>
	abstract class Platform3D
	{
		virtual internal int Background
		{
			set
			{
				if (this.argbBackground != value)
				{
					this.argbBackground = value;
					if (useClearingThread)
						clearingThread.notifyBackgroundChange(value);
				}
			}
			
		}
		
		internal int windowWidth, windowHeight, windowSize;
		internal int bufferWidth, bufferHeight, bufferSize;
		
		internal System.Drawing.Image imagePixelBuffer;
		internal int[] pBuffer;
		internal int[] zBuffer;
		internal int argbBackground;
		
		internal int widthOffscreen, heightOffscreen;
		internal System.Drawing.Image imageOffscreen;
		internal System.Drawing.Graphics gOffscreen;
		
		internal const bool forcePlatformAWT = false;
		internal const bool desireClearingThread = false;
		internal bool useClearingThread = true;
		
		internal ClearingThread clearingThread;
		
		internal static Platform3D createInstance(System.Windows.Forms.Control awtComponent)
		{
			//UPGRADE_ISSUE: Method 'java.lang.System.getProperty' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangSystem'"
			bool jvm12orGreater = String.CompareOrdinal(System_Renamed.getProperty("java.version"), "1.2") >= 0;
			bool useSwing = jvm12orGreater && !forcePlatformAWT;
			Platform3D platform = (useSwing?allocateSwing3D():new Awt3D(awtComponent));
			platform.initialize(desireClearingThread & useSwing);
			return platform;
		}
		
		private static Platform3D allocateSwing3D()
		{
			// this method is necessary in order to prevent Swing-related
			// classes from getting touched on the MacOS9 platform
			// otherwise the Mac crashes *badly* when the classes are not found
			return new Swing3D();
		}
		
		internal void  initialize(bool useClearingThread)
		{
			this.useClearingThread = useClearingThread;
			if (useClearingThread)
			{
				System.Console.Out.WriteLine("using ClearingThread");
				clearingThread = new ClearingThread(this);
				clearingThread.Start();
			}
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'ZBUFFER_BACKGROUND '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int ZBUFFER_BACKGROUND = System.Int32.MaxValue;
		
		internal abstract System.Drawing.Image allocateImage();
		
		internal virtual void  allocateBuffers(int width, int height, bool tFsaa4)
		{
			windowWidth = width;
			windowHeight = height;
			windowSize = width * height;
			if (tFsaa4)
			{
				bufferWidth = width * 2;
				bufferHeight = height * 2;
			}
			else
			{
				bufferWidth = width;
				bufferHeight = height;
			}
			bufferSize = bufferWidth * bufferHeight;
			zBuffer = new int[bufferSize];
			pBuffer = new int[bufferSize];
			imagePixelBuffer = allocateImage();
			/*
			System.out.println("  width:" + width + " bufferWidth=" + bufferWidth +
			"\nheight:" + height + " bufferHeight=" + bufferHeight);
			*/
		}
		
		internal virtual void  releaseBuffers()
		{
			windowWidth = windowHeight = bufferWidth = bufferHeight = bufferSize = - 1;
			if (imagePixelBuffer != null)
			{
				//UPGRADE_TODO: Method 'java.awt.Image.flush' was converted to 'System.Drawing.Image.Dispose' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
				imagePixelBuffer.Dispose();
				imagePixelBuffer = null;
			}
			pBuffer = null;
			zBuffer = null;
		}
		
		internal virtual bool hasContent()
		{
			for (int i = bufferSize; --i >= 0; )
				if (zBuffer[i] != ZBUFFER_BACKGROUND)
					return true;
			return false;
		}
		
		internal virtual void  clearScreenBuffer(int argbBackground)
		{
			for (int i = bufferSize; --i >= 0; )
			{
				zBuffer[i] = ZBUFFER_BACKGROUND;
				pBuffer[i] = argbBackground;
			}
		}
		
		internal void  obtainScreenBuffer()
		{
			if (useClearingThread)
			{
				clearingThread.obtainBufferForClient();
			}
			else
			{
				clearScreenBuffer(argbBackground);
			}
		}
		
		internal void  clearScreenBufferThreaded()
		{
			if (useClearingThread)
				clearingThread.releaseBufferForClearing();
		}
		
		internal virtual void  notifyEndOfRendering()
		{
		}
		
		internal abstract System.Drawing.Image allocateOffscreenImage(int width, int height);
		internal abstract System.Drawing.Graphics getGraphics(System.Drawing.Image imageOffscreen);
		
		internal virtual void  checkOffscreenSize(int width, int height)
		{
			if (width <= widthOffscreen && height <= heightOffscreen)
				return ;
			if (imageOffscreen != null)
			{
				gOffscreen.Dispose();
				//UPGRADE_TODO: Method 'java.awt.Image.flush' was converted to 'System.Drawing.Image.Dispose' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
				imageOffscreen.Dispose();
			}
			if (width > widthOffscreen)
				widthOffscreen = (width + 63) & ~ 63;
			if (height > heightOffscreen)
				heightOffscreen = (height + 15) & ~ 15;
			imageOffscreen = allocateOffscreenImage(widthOffscreen, heightOffscreen);
			gOffscreen = getGraphics(imageOffscreen);
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'ClearingThread' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		internal class ClearingThread:SupportClass.ThreadClass, IThreadRunnable
		{
			public ClearingThread(Platform3D enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(Platform3D enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Platform3D enclosingInstance;
			public Platform3D Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			
			internal bool bufferHasBeenCleared = false;
			internal bool clientHasBuffer = false;
			
			//UPGRADE_NOTE: Synchronized keyword was removed from method 'notifyBackgroundChange'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
			internal virtual void  notifyBackgroundChange(int argbBackground)
			{
				lock (this)
				{
					//      System.out.println("notifyBackgroundChange");
					bufferHasBeenCleared = false;
					System.Threading.Monitor.Pulse(this);
					// for now do nothing
				}
			}
			
			//UPGRADE_NOTE: Synchronized keyword was removed from method 'obtainBufferForClient'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
			internal virtual void  obtainBufferForClient()
			{
				lock (this)
				{
					//      System.out.println("obtainBufferForClient()");
					while (!bufferHasBeenCleared)
						try
						{
							System.Threading.Monitor.Wait(this);
						}
						catch (System.Threading.ThreadInterruptedException ie)
						{
						}
					clientHasBuffer = true;
				}
			}
			
			//UPGRADE_NOTE: Synchronized keyword was removed from method 'releaseBufferForClearing'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
			internal virtual void  releaseBufferForClearing()
			{
				lock (this)
				{
					//      System.out.println("releaseBufferForClearing()");
					clientHasBuffer = false;
					bufferHasBeenCleared = false;
					System.Threading.Monitor.Pulse(this);
				}
			}
			
			//UPGRADE_NOTE: Synchronized keyword was removed from method 'waitForClientRelease'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
			internal virtual void  waitForClientRelease()
			{
				lock (this)
				{
					//      System.out.println("waitForClientRelease()");
					while (clientHasBuffer || bufferHasBeenCleared)
						try
						{
							System.Threading.Monitor.Wait(this);
						}
						catch (System.Threading.ThreadInterruptedException ie)
						{
						}
				}
			}
			
			//UPGRADE_NOTE: Synchronized keyword was removed from method 'notifyBufferReady'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
			internal virtual void  notifyBufferReady()
			{
				lock (this)
				{
					//      System.out.println("notifyBufferReady()");
					bufferHasBeenCleared = true;
					System.Threading.Monitor.Pulse(this);
				}
			}
			
			override public void  Run()
			{
				/*
				System.out.println("running clearing thread:" +
				Thread.currentThread().getPriority());
				*/
				while (true)
				{
					waitForClientRelease();
					int bg;
					do 
					{
						bg = Enclosing_Instance.argbBackground;
						Enclosing_Instance.clearScreenBuffer(bg);
					}
					while (bg != Enclosing_Instance.argbBackground); // color changed underneath us
					notifyBufferReady();
				}
			}
		}
	}
}