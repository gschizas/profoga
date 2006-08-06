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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
namespace org.jmol.appletwrapper
{
	
	//UPGRADE_TODO: Class 'java.applet.Applet' was converted to 'System.Windows.Forms.UserControl' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaappletApplet'"
	[Serializable]
	public class AppletWrapper:System.Windows.Forms.UserControl
	{
		private void  InitBlock()
		{
			this.Load += new System.EventHandler(this.AppletWrapper_StartEventHandler);
			this.Disposed += new System.EventHandler(this.AppletWrapper_StopEventHandler);
		}
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'getNextPreloadClassName'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		virtual internal System.String NextPreloadClassName
		{
			get
			{
				lock (this)
				{
					if (preloadClassNames == null || preloadClassIndex == preloadClassNames.Length)
						return null;
					System.String className = preloadClassNames[preloadClassIndex++];
					if (className[0] == '.')
					{
						int lastDot = previousClassName.LastIndexOf('.');
						System.String previousPackageName = previousClassName.Substring(0, (lastDot) - (0));
						className = previousPackageName + className;
					}
					return previousClassName = className;
				}
			}
			
		}
		public bool isActiveVar = true;
		
		private System.String wrappedAppletClassName;
		private System.String preloadImageName;
		private System.String preloadTextMessage;
		private int preloadThreadCount;
		private System.String[] preloadClassNames;
		
		private int preloadClassIndex;
		private System.String previousClassName;
		
		private bool needToCompleteInitialization;
		
		private bool preloadImageReadyForDisplay;
		private bool preloadImagePainted;
		private System.Drawing.Image preloadImage;
		//private int preloadImageHeight;
		//UPGRADE_ISSUE: Class 'java.awt.MediaTracker' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtMediaTracker'"
		private MediaTracker mediaTracker;
		
		private System.Drawing.Color bgcolor
		{
			get
			{
				return bgcolor_Renamed;
			}
			
			set
			{
				bgcolor_Renamed = value;
			}
			
		}
		private System.Drawing.Color bgcolor_Renamed;
		private System.Drawing.Color textColor
		{
			get
			{
				return textColor_Renamed;
			}
			
			set
			{
				textColor_Renamed = value;
			}
			
		}
		private System.Drawing.Color textColor_Renamed;
		
		public WrappedApplet wrappedApplet;
		
		private long startTime;
		private int clockX;
		private int clockBaseline;
		private int clockWidth;
		
		
		private static int MINIMUM_ELAPSED_SECONDS = 1;
		
		private static System.String fontFace = "sansserif";
		private static int fontSizeDivisor = 18;
		private int fontSize;
		private System.Drawing.Font font;
		private System.Drawing.Font fontMetrics;
		private int fontAscent;
		//private int fontDescent;
		private int fontHeight;
		
		
		public AppletWrapper(System.String wrappedAppletClassName, System.String preloadImageName, System.String preloadTextMessage, int preloadThreadCount, System.String[] preloadClassNames)
		{
			InitBlock();
			this.wrappedAppletClassName = wrappedAppletClassName;
			this.preloadImageName = preloadImageName;
			this.preloadTextMessage = preloadTextMessage;
			this.preloadThreadCount = preloadThreadCount;
			this.preloadClassNames = preloadClassNames;
			needToCompleteInitialization = true;
		}
		
		public System.String GetUserControlInfo()
		{
			return (wrappedApplet != null?wrappedApplet.AppletInfo:null);
		}
		
		public void  init()
		{
			InitializeComponent();
			startTime = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
			new WrappedAppletLoader(this, wrappedAppletClassName).Start();
			for (int i = preloadThreadCount; --i >= 0; )
				new ClassPreloader(this).Start();
		}
		
		//UPGRADE_NOTE: The equivalent of method 'java.awt.Container.update' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		public void  update(System.Drawing.Graphics g)
		{
			if (wrappedApplet != null)
			{
				mediaTracker = null;
				preloadImage = null;
				fontMetrics = null;
				
				wrappedApplet.update(g);
				return ;
			}
			System.Drawing.Size dim = Size; // deprecated, but use it for old JVMs
			
			if (needToCompleteInitialization)
			{
				//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
				completeInitialization(g, ref dim);
			}
			
			SupportClass.GraphicsManager.manager.SetColor(g, bgcolor);
			g.FillRectangle(SupportClass.GraphicsManager.manager.GetPaint(g), 0, 0, dim.Width, dim.Height);
			SupportClass.GraphicsManager.manager.SetColor(g, textColor);
			
			int imageBottom = 0;
			
			if (!preloadImageReadyForDisplay && mediaTracker != null)
			{
				//UPGRADE_ISSUE: Method 'java.awt.MediaTracker.checkID' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtMediaTracker'"
				preloadImageReadyForDisplay = mediaTracker.checkID(0, true);
			}
			
			if (preloadImageReadyForDisplay)
			{
				int imageHeight = preloadImage.Height;
				if (imageHeight > 0)
				{
					if (10 + imageHeight + fontHeight <= dim.Height)
					{
						//UPGRADE_WARNING: Method 'java.awt.Graphics.drawImage' was converted to 'System.Drawing.Graphics.drawImage' which may throw an exception. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1101'"
						g.DrawImage(preloadImage, 10, 10);
						preloadImagePainted = true;
						imageBottom = 10 + imageHeight;
					}
				}
			}
			
			long elapsedTime = ((System.DateTime.Now.Ticks - 621355968000000000) / 10000 - startTime) / 1000;
			if (elapsedTime >= MINIMUM_ELAPSED_SECONDS)
			{
				int messageBaseline = imageBottom + fontAscent;
				if (messageBaseline < dim.Height / 2)
					messageBaseline = dim.Height / 2;
				else if (messageBaseline >= dim.Height)
					messageBaseline = dim.Height - 1;
				SupportClass.GraphicsManager.manager.SetFont(g, font);
				//UPGRADE_TODO: Method 'java.awt.Graphics.drawString' was converted to 'System.Drawing.Graphics.DrawString' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphicsdrawString_javalangString_int_int'"
				g.DrawString(preloadTextMessage, SupportClass.GraphicsManager.manager.GetFont(g), SupportClass.GraphicsManager.manager.GetBrush(g), 10, messageBaseline - SupportClass.GraphicsManager.manager.GetFont(g).GetHeight());
				
				System.String clockText = "" + elapsedTime + " seconds";
				//UPGRADE_ISSUE: Method 'java.awt.FontMetrics.stringWidth' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtFontMetricsstringWidth_javalangString'"
				clockWidth = fontMetrics.stringWidth(clockText);
				clockX = dim.Width - clockWidth - 5;
				if (clockX < 0)
					clockX = 0;
				clockBaseline = dim.Height - 5;
				//UPGRADE_TODO: Method 'java.awt.Graphics.drawString' was converted to 'System.Drawing.Graphics.DrawString' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphicsdrawString_javalangString_int_int'"
				g.DrawString(clockText, SupportClass.GraphicsManager.manager.GetFont(g), SupportClass.GraphicsManager.manager.GetBrush(g), clockX, clockBaseline - SupportClass.GraphicsManager.manager.GetFont(g).GetHeight());
			}
		}
		
		protected override void  OnPaint(System.Windows.Forms.PaintEventArgs g_EventArg)
		{
			System.Drawing.Graphics g = null;
			if (g_EventArg != null)
				g = g_EventArg.Graphics;
			if (wrappedApplet != null)
			{
				wrappedApplet.paint(g);
				return ;
			}
			update(g);
		}
		
		internal virtual void  repaintClock()
		{
			if (!preloadImagePainted || clockBaseline == 0)
			{
				//UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
				Refresh();
			}
			else
			{
				//UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint_int_int_int_int'"
				Refresh();
			}
		}
		
		
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		private void  completeInitialization(System.Drawing.Graphics g, ref System.Drawing.Size dim)
		{
			needToCompleteInitialization = false;
			if (preloadImageName != null)
			{
				try
				{
					System.Console.Out.WriteLine("loadImage:" + preloadImageName);
					//UPGRADE_ISSUE: Method 'java.lang.ClassLoader.getResource' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassLoader'"
					//UPGRADE_ISSUE: Method 'java.lang.Class.getClassLoader' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassgetClassLoader'"
					System.Uri urlImage = GetType().getClassLoader().getResource(preloadImageName);
					System.Console.Out.WriteLine("urlImage=" + urlImage);
					if (urlImage != null)
					{
						//UPGRADE_ISSUE: Method 'java.awt.Toolkit.getImage' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtToolkit'"
						//UPGRADE_ISSUE: Method 'java.awt.Toolkit.getDefaultToolkit' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtToolkit'"
						preloadImage = Toolkit.getDefaultToolkit().getImage(urlImage);
						System.Console.Out.WriteLine("successfully loaded " + preloadImageName);
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						System.Console.Out.WriteLine("preloadImage=" + preloadImage);
						//UPGRADE_ISSUE: Constructor 'java.awt.MediaTracker.MediaTracker' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtMediaTracker'"
						mediaTracker = new MediaTracker(this);
						//UPGRADE_ISSUE: Method 'java.awt.MediaTracker.addImage' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtMediaTracker'"
						mediaTracker.addImage(preloadImage, 0);
						//UPGRADE_ISSUE: Method 'java.awt.MediaTracker.checkID' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtMediaTracker'"
						mediaTracker.checkID(0, true);
					}
				}
				catch (System.Exception e)
				{
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					System.Console.Out.WriteLine("getImage failed: " + e);
				}
			}
			System.String bgcolorName = getParameter("boxbgcolor");
			if (bgcolorName == null)
				bgcolorName = getParameter("bgcolor");
			bgcolor = getColorFromName(bgcolorName);
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
			textColor = getContrastingBlackOrWhite(ref bgcolor);
			
			fontSize = dim.Height / fontSizeDivisor;
			if (fontSize < 7)
				fontSize = 7;
			if (fontSize > 30)
				fontSize = 30;
			
			while (true)
			{
				//UPGRADE_NOTE: If the given Font Name does not exist, a default Font instance is created. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1075'"
				//UPGRADE_TODO: Field 'java.awt.Font.PLAIN' was converted to 'System.Drawing.FontStyle.Regular' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtFontPLAIN_f'"
				font = new System.Drawing.Font(fontFace, fontSize, (System.Drawing.FontStyle) System.Drawing.FontStyle.Regular);
				fontMetrics = font;
				//UPGRADE_ISSUE: Method 'java.awt.FontMetrics.stringWidth' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtFontMetricsstringWidth_javalangString'"
				if (fontMetrics.stringWidth(preloadTextMessage) + 10 < dim.Width)
					break;
				if (fontSize < 8)
					break;
				fontSize -= 2;
			}
			//UPGRADE_TODO: The equivalent in .NET for method 'java.awt.FontMetrics.getHeight' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			fontHeight = (int) fontMetrics.GetHeight();
			//UPGRADE_TODO: The equivalent in .NET for method 'java.awt.FontMetrics.getAscent' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			fontAscent = SupportClass.GetAscent(fontMetrics);
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'colorNames'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private static readonly System.String[] colorNames = new System.String[]{"aqua", "black", "blue", "fuchsia", "gray", "green", "lime", "maroon", "navy", "olive", "purple", "red", "silver", "teal", "white", "yellow"};
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'colors '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private static readonly System.Drawing.Color[] colors = new System.Drawing.Color[]{System.Drawing.Color.Cyan, System.Drawing.Color.Black, System.Drawing.Color.Blue, System.Drawing.Color.Magenta, System.Drawing.Color.Gray, System.Drawing.Color.FromArgb(0, 128, 0), System.Drawing.Color.Green, System.Drawing.Color.FromArgb(128, 0, 0), System.Drawing.Color.FromArgb(0, 0, 128), System.Drawing.Color.FromArgb(128, 128, 0), System.Drawing.Color.FromArgb(128, 0, 128), System.Drawing.Color.Red, System.Drawing.Color.LightGray, System.Drawing.Color.FromArgb(0, 128, 128), System.Drawing.Color.White, System.Drawing.Color.Yellow};
		
		
		private System.Drawing.Color getColorFromName(System.String strColor)
		{
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
						return System.Drawing.Color.FromArgb(red, grn, blu);
					}
					catch (System.FormatException e)
					{
					}
				}
				else
				{
					strColor = String.Intern(strColor.ToLower());
					for (int i = colorNames.Length; --i >= 0; )
						if ((System.Object) strColor == (System.Object) colorNames[i])
							return colors[i];
				}
			}
			return System.Drawing.Color.Black;
		}
		
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		private System.Drawing.Color getContrastingBlackOrWhite(ref System.Drawing.Color color)
		{
			// return a grayscale value 0-FF using NTSC color luminance algorithm
			//UPGRADE_TODO: The equivalent in .NET for method 'java.awt.Color.getRGB' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			int argb = color.ToArgb();
			int grayscale = ((2989 * (argb >> 16) & 0xFF) + (5870 * (argb >> 8) & 0xFF) + (1140 * (argb & 0xFF)) + 500) / 1000;
			return grayscale < 128?System.Drawing.Color.White:System.Drawing.Color.Black;
		}
		
		//UPGRADE_NOTE: The equivalent of method 'java.awt.Component.handleEvent' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		//UPGRADE_ISSUE: Class 'java.awt.Event' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
		public bool handleEvent(Event e)
		{
			if (wrappedApplet != null)
				return wrappedApplet.handleEvent(e);
			return false;
		}
		public void  ResizeControl(System.Drawing.Size p)
		{
			this.Width = p.Width;
			this.Height = p.Height;
		}
		public void  ResizeControl(int p2, int p3)
		{
			this.Width = p2;
			this.Height = p3;
		}
		public System.String[][] GetParameterInfo()
		{
			return null;
		}
		public System.String  TempDocumentBaseVar = "";
		public virtual System.Uri DocumentBase
		{
			get
			{
				if (TempDocumentBaseVar == "")
					return new System.Uri("http://127.0.0.1");
				else
					return new System.Uri(TempDocumentBaseVar);
			}
			
		}
		public System.Drawing.Image getImage(System.Uri p4)
		{
			Bitmap TemporalyBitmap = new Bitmap(p4.AbsolutePath);
			return (Image) TemporalyBitmap;
		}
		public System.Drawing.Image getImage(System.Uri p5, System.String p6)
		{
			Bitmap TemporalyBitmap = new Bitmap(p5.AbsolutePath + p6);
			return (Image) TemporalyBitmap;
		}
		public virtual System.Boolean isActive()
		{
			return isActiveVar;
		}
		public virtual void  start()
		{
			isActiveVar = true;
		}
		public virtual void  stop()
		{
			isActiveVar = false;
		}
		private void  AppletWrapper_StartEventHandler(System.Object sender, System.EventArgs e)
		{
			init();
			start();
		}
		private void  AppletWrapper_StopEventHandler(System.Object sender, System.EventArgs e)
		{
			stop();
		}
		public virtual String getParameter(System.String paramName)
		{
			return null;
		}
		#region Windows Form Designer generated code
		private void  InitializeComponent()
		{
			this.SuspendLayout();
			this.BackColor = Color.LightGray;
			this.ResumeLayout(false);
		}
		#endregion
	}
}