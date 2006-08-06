/* $RCSfile$
* $Author: migueljmol $
* $Date: 2005-11-27 22:39:31 +0100 (dim., 27 nov. 2005) $
* $Revision: 4285 $
*
* Copyright (C) 2002-2005  The Jmol Development Team
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
using GT = org.jmol.i18n.GT;
namespace org.openscience.jmol.app
{
	
	[Serializable]
	public class Splash:System.Windows.Forms.Form
	{
		private void  InitBlock()
		{
			status = GT._("Loading...");
		}
		
		private System.Drawing.Image splashImage;
		private int imgWidth, imgHeight;
		private const int BORDERSIZE = 10;
		//UPGRADE_NOTE: Final was removed from the declaration of 'BORDERCOLOR '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private static readonly System.Drawing.Color BORDERCOLOR
		{
			get
			{
				return BORDERCOLOR_Renamed;
			}
			
			set
			{
				BORDERCOLOR_Renamed = value;
			}
			
		}
		private static readonly System.Drawing.Color BORDERCOLOR_Renamed = System.Drawing.Color.Blue;
		//UPGRADE_NOTE: The initialization of  'status' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private System.String status;
		private int textY;
		private int statusTop;
		private const int STATUSSIZE = 10;
		//UPGRADE_NOTE: Final was removed from the declaration of 'TEXTCOLOR '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private static readonly System.Drawing.Color TEXTCOLOR
		{
			get
			{
				return TEXTCOLOR_Renamed;
			}
			
			set
			{
				TEXTCOLOR_Renamed = value;
			}
			
		}
		private static readonly System.Drawing.Color TEXTCOLOR_Renamed = System.Drawing.Color.White;
		
		//UPGRADE_TODO: Class 'java.awt.Frame' was converted to 'System.Windows.Forms.Form' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtFrame'"
		//UPGRADE_TODO: Class 'javax.swing.ImageIcon' was converted to 'System.Drawing.Image' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingImageIcon'"
		public Splash(System.Windows.Forms.Form parent, System.Drawing.Image ii):base()
		{
			InitBlock();
			//UPGRADE_TODO: Class 'java.awt.Frame' was converted to 'System.Windows.Forms.Form' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtFrame'"
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Owner = new System.Windows.Forms.Form();
			this.ShowInTaskbar = false;
			splashImage = ii;
			imgWidth = splashImage.Width;
			imgHeight = splashImage.Height;
			showSplashScreen();
			//UPGRADE_NOTE: Some methods of the 'java.awt.event.WindowListener' class are not used in the .NET Framework. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1308'"
			parent.Activated += new System.EventHandler(new WindowListener(this).windowActivated);
		}
		
		public virtual void  showSplashScreen()
		{
			
			//UPGRADE_ISSUE: Class 'java.awt.Toolkit' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtToolkit'"
			//UPGRADE_ISSUE: Method 'java.awt.Toolkit.getDefaultToolkit' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtToolkit'"
			Toolkit tk = Toolkit.getDefaultToolkit();
			System.Drawing.Size screenSize = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size;
			BackColor = BORDERCOLOR;
			int w = imgWidth + (BORDERSIZE * 2);
			int h = imgHeight + (BORDERSIZE * 2) + STATUSSIZE;
			int x = (screenSize.Width - w) / 2;
			int y = (screenSize.Height - h) / 2;
			SetBounds(x, y, w, h);
			statusTop = BORDERSIZE + imgHeight;
			textY = BORDERSIZE + STATUSSIZE + imgHeight + 1;
			//UPGRADE_TODO: 'System.Windows.Forms.Application.Run' must be called to start a main form. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1135'"
			Show();
		}
		
		protected override void  OnPaint(System.Windows.Forms.PaintEventArgs g_EventArg)
		{
			System.Drawing.Graphics g = null;
			if (g_EventArg != null)
				g = g_EventArg.Graphics;
			
			//UPGRADE_WARNING: Method 'java.awt.Graphics.drawImage' was converted to 'System.Drawing.Graphics.drawImage' which may throw an exception. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1101'"
			g.DrawImage(splashImage, BORDERSIZE, BORDERSIZE, imgWidth, imgHeight);
			SupportClass.GraphicsManager.manager.SetColor(g, BORDERCOLOR);
			g.FillRectangle(SupportClass.GraphicsManager.manager.GetPaint(g), BORDERSIZE, statusTop, imgWidth, textY);
			SupportClass.GraphicsManager.manager.SetColor(g, TEXTCOLOR);
			//UPGRADE_TODO: Method 'java.awt.Graphics.drawString' was converted to 'System.Drawing.Graphics.DrawString' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphicsdrawString_javalangString_int_int'"
			g.DrawString(status, SupportClass.GraphicsManager.manager.GetFont(g), SupportClass.GraphicsManager.manager.GetBrush(g), BORDERSIZE, textY - SupportClass.GraphicsManager.manager.GetFont(g).GetHeight());
		}
		
		public virtual void  showStatus(System.String message)
		{
			
			if (message != null)
			{
				status = message;
				System.Drawing.Graphics g = SupportClass.GraphicsManager.manager.GetGraphics(this);
				if (g == null)
				{
					return ;
				}
				SupportClass.GraphicsManager.manager.SetColor(g, BORDERCOLOR);
				g.FillRectangle(SupportClass.GraphicsManager.manager.GetPaint(g), BORDERSIZE, statusTop, imgWidth + BORDERSIZE, textY);
				SupportClass.GraphicsManager.manager.SetColor(g, TEXTCOLOR);
				//UPGRADE_TODO: Method 'java.awt.Graphics.drawString' was converted to 'System.Drawing.Graphics.DrawString' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphicsdrawString_javalangString_int_int'"
				g.DrawString(status, SupportClass.GraphicsManager.manager.GetFont(g), SupportClass.GraphicsManager.manager.GetBrush(g), BORDERSIZE, textY - SupportClass.GraphicsManager.manager.GetFont(g).GetHeight());
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'WindowListener' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		internal class WindowListener
		{
			public WindowListener(Splash enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(Splash enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Splash enclosingInstance;
			public Splash Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			public void  windowActivated(System.Object event_sender, System.EventArgs we)
			{
				Enclosing_Instance.Visible = false;
				Enclosing_Instance.Dispose();
			}
		}
	}
}