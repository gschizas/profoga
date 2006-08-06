/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-03-27 19:40:31 +0200 (lun., 27 mars 2006) $
* $Revision: 4787 $
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
using org.jmol.api;
using JmolConstants = org.jmol.viewer.JmolConstants;
using GT = org.jmol.i18n.GT;
namespace org.openscience.jmol.app
{
	
	//UPGRADE_ISSUE: Interface 'java.awt.print.Printable' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtprintPrintable'"
	[Serializable]
	public class DisplayPanel:System.Windows.Forms.Panel, Printable
	{
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassMenuListener' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		internal class AnonymousClassMenuListener
		{
			public AnonymousClassMenuListener(DisplayPanel enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(DisplayPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private DisplayPanel enclosingInstance;
			public DisplayPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public virtual void  menuSelected(System.Object event_sender, System.EventArgs e)
			{
				//UPGRADE_TODO: The method 'javax.swing.event.MenuEvent.getSource' needs to be in a event handling method in order to be properly converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1171'"
				System.String menuKey = Enclosing_Instance.guimap.getKey(e.getSource());
				if (menuKey.Equals("display"))
				{
					Enclosing_Instance.setDisplayMenuState();
				}
			}
			public virtual void  menuDeselected(System.Object event_sender, System.EventArgs e)
			{
			}
			public virtual void  menuCanceled(System.Object event_sender, System.EventArgs e)
			{
			}
		}
		private void  InitBlock()
		{
			deleteAction = new DeleteAction(this);
			pickAction = new PickAction(this);
			rotateAction = new RotateAction(this);
			zoomAction = new ZoomAction(this);
			xlateAction = new XlateAction(this);
			homeAction = new HomeAction(this);
			frontAction = new FrontAction(this);
			topAction = new TopAction(this);
			bottomAction = new BottomAction(this);
			rightAction = new RightAction(this);
			leftAction = new LeftAction(this);
			defineCenterAction = new DefineCenterAction(this);
			hydrogensAction = new HydrogensAction(this);
			measurementsAction = new MeasurementsAction(this);
			selectallAction = new SelectallAction(this);
			deselectallAction = new DeselectallAction(this);
			perspectiveAction = new PerspectiveAction(this);
			axesAction = new AxesAction(this);
			boundboxAction = new BoundboxAction(this);
			menuListener = new AnonymousClassMenuListener(this);
		}
		virtual public JmolViewer Viewer
		{
			set
			{
				this.viewer = value;
				//UPGRADE_TODO: Method 'javax.swing.JComponent.getSize' was converted to 'System.Drawing.Size' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJComponentgetSize_javaawtDimension'"
				dimSize = Size;
				value.ScreenDimension = Size;
			}
			
		}
		//UPGRADE_TODO: Interface 'javax.swing.event.MenuListener' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
		virtual public MenuListener MenuListener
		{
			get
			{
				return menuListener;
			}
			
		}
		virtual public SupportClass.ActionSupport[] Actions
		{
			get
			{
				
				SupportClass.ActionSupport[] defaultActions = new SupportClass.ActionSupport[]{deleteAction, pickAction, rotateAction, zoomAction, xlateAction, frontAction, topAction, bottomAction, rightAction, leftAction, defineCenterAction, hydrogensAction, measurementsAction, selectallAction, deselectallAction, homeAction, perspectiveAction, axesAction, boundboxAction};
				return defaultActions;
			}
			
		}
		internal StatusBar status;
		internal GuiMap guimap;
		internal JmolViewer viewer;
		
		private System.String displaySpeed;
		
		public DisplayPanel(StatusBar status, GuiMap guimap)
		{
			InitBlock();
			this.status = status;
			this.guimap = guimap;
			//UPGRADE_ISSUE: Method 'java.lang.System.getProperty' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangSystem'"
			if (System_Renamed.getProperty("painttime", "false").Equals("true"))
				showPaintTime = true;
			//UPGRADE_ISSUE: Method 'java.lang.System.getProperty' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangSystem'"
			displaySpeed = System_Renamed.getProperty("display.speed");
			if (displaySpeed == null)
			{
				displaySpeed = "ms";
			}
			//UPGRADE_ISSUE: Method 'javax.swing.JComponent.setDoubleBuffered' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJComponentsetDoubleBuffered_boolean'"
			setDoubleBuffered(false);
		}
		
		// for now, default to true
		private bool showPaintTime = true;
		
		// current dimensions of the display screen
		//UPGRADE_NOTE: Final was removed from the declaration of 'dimSize '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private System.Drawing.Size dimSize
		{
			get
			{
				return dimSize_Renamed;
			}
			
			set
			{
				dimSize_Renamed = value;
			}
			
		}
		private System.Drawing.Size dimSize_Renamed = new System.Drawing.Size(0, 0);
		//UPGRADE_NOTE: Final was removed from the declaration of 'rectClip '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private System.Drawing.Rectangle rectClip
		{
			get
			{
				return rectClip_Renamed;
			}
			
			set
			{
				rectClip_Renamed = value;
			}
			
		}
		private System.Drawing.Rectangle rectClip_Renamed = new System.Drawing.Rectangle();
		
		public virtual void  start()
		{
			//UPGRADE_WARNING: Extra logic should be included into componentHidden to know if the Component is hidden. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1144'"
			VisibleChanged += new System.EventHandler(this.componentHidden);
			Move += new System.EventHandler(this.componentMoved);
			Resize += new System.EventHandler(this.componentResized);
			//UPGRADE_WARNING: Extra logic should be included into componentShown to know if the Component is visible. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1145'"
			VisibleChanged += new System.EventHandler(this.componentShown);
		}
		
		internal virtual void  setRotateMode()
		{
			Jmol.setRotateButton();
			viewer.ModeMouse = JmolConstants.MOUSE_ROTATE;
			viewer.setSelectionHaloEnabled(false);
		}
		
		public virtual void  componentHidden(System.Object event_sender, System.EventArgs e)
		{
		}
		
		public virtual void  componentMoved(System.Object event_sender, System.EventArgs e)
		{
		}
		
		public virtual void  componentResized(System.Object event_sender, System.EventArgs e)
		{
			updateSize();
		}
		
		public virtual void  componentShown(System.Object event_sender, System.EventArgs e)
		{
			updateSize();
		}
		
		private void  updateSize()
		{
			//UPGRADE_TODO: Method 'javax.swing.JComponent.getSize' was converted to 'System.Drawing.Size' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJComponentgetSize_javaawtDimension'"
			dimSize = Size;
			viewer.ScreenDimension = Size;
			setRotateMode();
		}
		
		protected override void  OnPaint(System.Windows.Forms.PaintEventArgs g_EventArg)
		{
			System.Drawing.Graphics g = null;
			if (g_EventArg != null)
				g = g_EventArg.Graphics;
			if (showPaintTime)
				startPaintClock();
			rectClip = System.Drawing.Rectangle.Truncate(g.ClipBounds);
			System.Drawing.Rectangle generatedAux = rectClip;
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
			viewer.renderScreenImage(g, ref dimSize, ref rectClip);
			if (showPaintTime)
				stopPaintClock();
		}
		
		public virtual int print(System.Drawing.Graphics g, System.Drawing.Printing.PageSettings pf, int pageIndex)
		{
			System.Drawing.Graphics g2 = (System.Drawing.Graphics) g;
			if (pageIndex > 0)
			{
				//UPGRADE_ISSUE: Field 'java.awt.print.Printable.NO_SUCH_PAGE' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtprintPrintable'"
				return Printable.NO_SUCH_PAGE;
			}
			rectClip.X = rectClip.Y = 0;
			int screenWidth = rectClip.Width = viewer.ScreenWidth;
			int screenHeight = rectClip.Height = viewer.ScreenHeight;
			System.Drawing.Image image = viewer.ScreenImage;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			int pageX = (int) pf.Bounds.X;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			int pageY = (int) pf.Bounds.Y;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			int pageWidth = (int) (((pf.Bounds.Width * 72) / 100));
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			int pageHeight = (int) (((pf.Bounds.Height * 72) / 100));
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			float scaleWidth = pageWidth / (float) screenWidth;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			float scaleHeight = pageHeight / (float) screenHeight;
			float scale = (scaleWidth < scaleHeight?scaleWidth:scaleHeight);
			if (scale < 1)
			{
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				int width = (int) (screenWidth * scale);
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				int height = (int) (screenHeight * scale);
				//UPGRADE_ISSUE: Method 'java.awt.Graphics2D.setRenderingHint' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGraphics2DsetRenderingHint_javaawtRenderingHintsKey_javalangObject'"
				//UPGRADE_ISSUE: Field 'java.awt.RenderingHints.KEY_RENDERING' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtRenderingHintsKEY_RENDERING_f'"
				g2.setRenderingHint(RenderingHints.KEY_RENDERING, (System.Object) System.Drawing.Drawing2D.CompositingQuality.HighQuality);
				//UPGRADE_ISSUE: Method 'java.awt.Graphics2D.setRenderingHint' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGraphics2DsetRenderingHint_javaawtRenderingHintsKey_javalangObject'"
				//UPGRADE_ISSUE: Field 'java.awt.RenderingHints.KEY_INTERPOLATION' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtRenderingHintsKEY_INTERPOLATION_f'"
				g2.setRenderingHint(RenderingHints.KEY_INTERPOLATION, (System.Object) System.Drawing.Drawing2D.InterpolationMode.Bicubic);
				//UPGRADE_WARNING: Method 'java.awt.Graphics.drawImage' was converted to 'System.Drawing.Graphics.drawImage' which may throw an exception. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1101'"
				g2.DrawImage(image, pageX, pageY, width, height);
			}
			else
			{
				//UPGRADE_WARNING: Method 'java.awt.Graphics.drawImage' was converted to 'System.Drawing.Graphics.drawImage' which may throw an exception. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1101'"
				g2.DrawImage(image, pageX, pageY);
			}
			viewer.releaseScreenImage();
			//UPGRADE_ISSUE: Field 'java.awt.print.Printable.PAGE_EXISTS' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtprintPrintable'"
			return Printable.PAGE_EXISTS;
		}
		
		// The actions:
		
		//UPGRADE_NOTE: The initialization of  'deleteAction' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private DeleteAction deleteAction;
		//UPGRADE_NOTE: The initialization of  'pickAction' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private PickAction pickAction;
		//UPGRADE_NOTE: The initialization of  'rotateAction' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private RotateAction rotateAction;
		//UPGRADE_NOTE: The initialization of  'zoomAction' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private ZoomAction zoomAction;
		//UPGRADE_NOTE: The initialization of  'xlateAction' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private XlateAction xlateAction;
		//UPGRADE_NOTE: The initialization of  'homeAction' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private HomeAction homeAction;
		//UPGRADE_NOTE: The initialization of  'frontAction' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private FrontAction frontAction;
		//UPGRADE_NOTE: The initialization of  'topAction' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private TopAction topAction;
		//UPGRADE_NOTE: The initialization of  'bottomAction' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private BottomAction bottomAction;
		//UPGRADE_NOTE: The initialization of  'rightAction' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private RightAction rightAction;
		//UPGRADE_NOTE: The initialization of  'leftAction' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private LeftAction leftAction;
		//UPGRADE_NOTE: The initialization of  'defineCenterAction' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private DefineCenterAction defineCenterAction;
		//UPGRADE_NOTE: The initialization of  'hydrogensAction' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private HydrogensAction hydrogensAction;
		//UPGRADE_NOTE: The initialization of  'measurementsAction' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private MeasurementsAction measurementsAction;
		//UPGRADE_NOTE: The initialization of  'selectallAction' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private SelectallAction selectallAction;
		//UPGRADE_NOTE: The initialization of  'deselectallAction' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private DeselectallAction deselectallAction;
		//UPGRADE_NOTE: The initialization of  'perspectiveAction' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private PerspectiveAction perspectiveAction;
		//UPGRADE_NOTE: The initialization of  'axesAction' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private AxesAction axesAction;
		//UPGRADE_NOTE: The initialization of  'boundboxAction' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private BoundboxAction boundboxAction;
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'HydrogensAction' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class HydrogensAction:SupportClass.ActionSupport
		{
			private void  InitBlock(DisplayPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private DisplayPanel enclosingInstance;
			public DisplayPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			//UPGRADE_TODO: Constructor 'javax.swing.AbstractAction.AbstractAction' was converted to 'ActionSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingAbstractActionAbstractAction_javalangString'"
			public HydrogensAction(DisplayPanel enclosingInstance):base("hydrogensCheck")
			{
				InitBlock(enclosingInstance);
				//UPGRADE_ISSUE: Method 'javax.swing.AbstractAction.setEnabled' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractActionsetEnabled_boolean'"
				this.setEnabled(true);
			}
			
			public override void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				System.Windows.Forms.MenuItem cbmi = (System.Windows.Forms.MenuItem) event_sender;
				Enclosing_Instance.viewer.ShowHydrogens = cbmi.Checked;
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'MeasurementsAction' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class MeasurementsAction:SupportClass.ActionSupport
		{
			private void  InitBlock(DisplayPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private DisplayPanel enclosingInstance;
			public DisplayPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			//UPGRADE_TODO: Constructor 'javax.swing.AbstractAction.AbstractAction' was converted to 'ActionSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingAbstractActionAbstractAction_javalangString'"
			public MeasurementsAction(DisplayPanel enclosingInstance):base("measurementsCheck")
			{
				InitBlock(enclosingInstance);
				//UPGRADE_ISSUE: Method 'javax.swing.AbstractAction.setEnabled' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractActionsetEnabled_boolean'"
				this.setEnabled(true);
			}
			
			public override void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				System.Windows.Forms.MenuItem cbmi = (System.Windows.Forms.MenuItem) event_sender;
				Enclosing_Instance.viewer.ShowMeasurements = cbmi.Checked;
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'SelectallAction' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class SelectallAction:SupportClass.ActionSupport
		{
			private void  InitBlock(DisplayPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private DisplayPanel enclosingInstance;
			public DisplayPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			//UPGRADE_TODO: Constructor 'javax.swing.AbstractAction.AbstractAction' was converted to 'ActionSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingAbstractActionAbstractAction_javalangString'"
			public SelectallAction(DisplayPanel enclosingInstance):base("selectall")
			{
				InitBlock(enclosingInstance);
				//UPGRADE_ISSUE: Method 'javax.swing.AbstractAction.setEnabled' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractActionsetEnabled_boolean'"
				this.setEnabled(true);
			}
			
			public override void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				
				if (Enclosing_Instance.viewer.haveFrame())
				{
					Enclosing_Instance.viewer.selectAll();
				}
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'DeselectallAction' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class DeselectallAction:SupportClass.ActionSupport
		{
			private void  InitBlock(DisplayPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private DisplayPanel enclosingInstance;
			public DisplayPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			//UPGRADE_TODO: Constructor 'javax.swing.AbstractAction.AbstractAction' was converted to 'ActionSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingAbstractActionAbstractAction_javalangString'"
			public DeselectallAction(DisplayPanel enclosingInstance):base("deselectall")
			{
				InitBlock(enclosingInstance);
				//UPGRADE_ISSUE: Method 'javax.swing.AbstractAction.setEnabled' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractActionsetEnabled_boolean'"
				this.setEnabled(true);
			}
			
			public override void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				
				if (Enclosing_Instance.viewer.haveFrame())
				{
					Enclosing_Instance.viewer.clearSelection();
				}
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'PickAction' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class PickAction:SupportClass.ActionSupport
		{
			private void  InitBlock(DisplayPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private DisplayPanel enclosingInstance;
			public DisplayPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			//UPGRADE_TODO: Constructor 'javax.swing.AbstractAction.AbstractAction' was converted to 'ActionSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingAbstractActionAbstractAction_javalangString'"
			public PickAction(DisplayPanel enclosingInstance):base("pick")
			{
				InitBlock(enclosingInstance);
				//UPGRADE_ISSUE: Method 'javax.swing.AbstractAction.setEnabled' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractActionsetEnabled_boolean'"
				this.setEnabled(true);
			}
			
			public override void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				Enclosing_Instance.viewer.ModeMouse = JmolConstants.MOUSE_PICK;
				Enclosing_Instance.viewer.setSelectionHaloEnabled(true);
				Enclosing_Instance.status.setStatus(1, GT._("Select Atoms"));
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'DeleteAction' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class DeleteAction:SupportClass.ActionSupport
		{
			private void  InitBlock(DisplayPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private DisplayPanel enclosingInstance;
			public DisplayPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			//UPGRADE_TODO: Constructor 'javax.swing.AbstractAction.AbstractAction' was converted to 'ActionSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingAbstractActionAbstractAction_javalangString'"
			public DeleteAction(DisplayPanel enclosingInstance):base("delete")
			{
				InitBlock(enclosingInstance);
				//UPGRADE_ISSUE: Method 'javax.swing.AbstractAction.setEnabled' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractActionsetEnabled_boolean'"
				this.setEnabled(true);
			}
			
			public override void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				Enclosing_Instance.viewer.ModeMouse = JmolConstants.MOUSE_DELETE;
				Enclosing_Instance.viewer.setSelectionHaloEnabled(false);
				Enclosing_Instance.status.setStatus(1, GT._("Delete Atoms"));
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'RotateAction' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class RotateAction:SupportClass.ActionSupport
		{
			private void  InitBlock(DisplayPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private DisplayPanel enclosingInstance;
			public DisplayPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			//UPGRADE_TODO: Constructor 'javax.swing.AbstractAction.AbstractAction' was converted to 'ActionSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingAbstractActionAbstractAction_javalangString'"
			public RotateAction(DisplayPanel enclosingInstance):base("rotate")
			{
				InitBlock(enclosingInstance);
				//UPGRADE_ISSUE: Method 'javax.swing.AbstractAction.setEnabled' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractActionsetEnabled_boolean'"
				this.setEnabled(true);
			}
			
			public override void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				Enclosing_Instance.viewer.ModeMouse = JmolConstants.MOUSE_ROTATE;
				Enclosing_Instance.viewer.setSelectionHaloEnabled(false);
				Enclosing_Instance.status.setStatus(1, SupportClass.ToolTipSupport.getToolTipText(((System.Windows.Forms.Control) event_sender)));
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'ZoomAction' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class ZoomAction:SupportClass.ActionSupport
		{
			private void  InitBlock(DisplayPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private DisplayPanel enclosingInstance;
			public DisplayPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			//UPGRADE_TODO: Constructor 'javax.swing.AbstractAction.AbstractAction' was converted to 'ActionSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingAbstractActionAbstractAction_javalangString'"
			public ZoomAction(DisplayPanel enclosingInstance):base("zoom")
			{
				InitBlock(enclosingInstance);
				//UPGRADE_ISSUE: Method 'javax.swing.AbstractAction.setEnabled' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractActionsetEnabled_boolean'"
				this.setEnabled(true);
			}
			
			public override void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				Enclosing_Instance.viewer.ModeMouse = JmolConstants.MOUSE_ZOOM;
				Enclosing_Instance.viewer.setSelectionHaloEnabled(false);
				Enclosing_Instance.status.setStatus(1, SupportClass.ToolTipSupport.getToolTipText(((System.Windows.Forms.Control) event_sender)));
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'XlateAction' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class XlateAction:SupportClass.ActionSupport
		{
			private void  InitBlock(DisplayPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private DisplayPanel enclosingInstance;
			public DisplayPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			//UPGRADE_TODO: Constructor 'javax.swing.AbstractAction.AbstractAction' was converted to 'ActionSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingAbstractActionAbstractAction_javalangString'"
			public XlateAction(DisplayPanel enclosingInstance):base("xlate")
			{
				InitBlock(enclosingInstance);
				//UPGRADE_ISSUE: Method 'javax.swing.AbstractAction.setEnabled' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractActionsetEnabled_boolean'"
				this.setEnabled(true);
			}
			
			public override void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				Enclosing_Instance.viewer.ModeMouse = JmolConstants.MOUSE_XLATE;
				Enclosing_Instance.viewer.setSelectionHaloEnabled(false);
				Enclosing_Instance.status.setStatus(1, SupportClass.ToolTipSupport.getToolTipText(((System.Windows.Forms.Control) event_sender)));
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'FrontAction' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class FrontAction:SupportClass.ActionSupport
		{
			private void  InitBlock(DisplayPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private DisplayPanel enclosingInstance;
			public DisplayPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			//UPGRADE_TODO: Constructor 'javax.swing.AbstractAction.AbstractAction' was converted to 'ActionSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingAbstractActionAbstractAction_javalangString'"
			public FrontAction(DisplayPanel enclosingInstance):base("front")
			{
				InitBlock(enclosingInstance);
				//UPGRADE_ISSUE: Method 'javax.swing.AbstractAction.setEnabled' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractActionsetEnabled_boolean'"
				this.setEnabled(true);
			}
			
			public override void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				Enclosing_Instance.viewer.rotateFront();
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'TopAction' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class TopAction:SupportClass.ActionSupport
		{
			private void  InitBlock(DisplayPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private DisplayPanel enclosingInstance;
			public DisplayPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			//UPGRADE_TODO: Constructor 'javax.swing.AbstractAction.AbstractAction' was converted to 'ActionSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingAbstractActionAbstractAction_javalangString'"
			public TopAction(DisplayPanel enclosingInstance):base("top")
			{
				InitBlock(enclosingInstance);
				//UPGRADE_ISSUE: Method 'javax.swing.AbstractAction.setEnabled' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractActionsetEnabled_boolean'"
				this.setEnabled(true);
			}
			
			public override void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				Enclosing_Instance.viewer.rotateToX(90);
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'BottomAction' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class BottomAction:SupportClass.ActionSupport
		{
			private void  InitBlock(DisplayPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private DisplayPanel enclosingInstance;
			public DisplayPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			//UPGRADE_TODO: Constructor 'javax.swing.AbstractAction.AbstractAction' was converted to 'ActionSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingAbstractActionAbstractAction_javalangString'"
			public BottomAction(DisplayPanel enclosingInstance):base("bottom")
			{
				InitBlock(enclosingInstance);
				//UPGRADE_ISSUE: Method 'javax.swing.AbstractAction.setEnabled' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractActionsetEnabled_boolean'"
				this.setEnabled(true);
			}
			
			public override void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				Enclosing_Instance.viewer.rotateToX(- 90);
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'RightAction' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class RightAction:SupportClass.ActionSupport
		{
			private void  InitBlock(DisplayPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private DisplayPanel enclosingInstance;
			public DisplayPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			//UPGRADE_TODO: Constructor 'javax.swing.AbstractAction.AbstractAction' was converted to 'ActionSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingAbstractActionAbstractAction_javalangString'"
			public RightAction(DisplayPanel enclosingInstance):base("right")
			{
				InitBlock(enclosingInstance);
				//UPGRADE_ISSUE: Method 'javax.swing.AbstractAction.setEnabled' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractActionsetEnabled_boolean'"
				this.setEnabled(true);
			}
			
			public override void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				Enclosing_Instance.viewer.rotateToY(90);
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'LeftAction' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class LeftAction:SupportClass.ActionSupport
		{
			private void  InitBlock(DisplayPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private DisplayPanel enclosingInstance;
			public DisplayPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			//UPGRADE_TODO: Constructor 'javax.swing.AbstractAction.AbstractAction' was converted to 'ActionSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingAbstractActionAbstractAction_javalangString'"
			public LeftAction(DisplayPanel enclosingInstance):base("left")
			{
				InitBlock(enclosingInstance);
				//UPGRADE_ISSUE: Method 'javax.swing.AbstractAction.setEnabled' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractActionsetEnabled_boolean'"
				this.setEnabled(true);
			}
			
			public override void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				Enclosing_Instance.viewer.rotateToY(- 90);
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'DefineCenterAction' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class DefineCenterAction:SupportClass.ActionSupport
		{
			private void  InitBlock(DisplayPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private DisplayPanel enclosingInstance;
			public DisplayPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			//UPGRADE_TODO: Constructor 'javax.swing.AbstractAction.AbstractAction' was converted to 'ActionSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingAbstractActionAbstractAction_javalangString'"
			public DefineCenterAction(DisplayPanel enclosingInstance):base("definecenter")
			{
				InitBlock(enclosingInstance);
				//UPGRADE_ISSUE: Method 'javax.swing.AbstractAction.setEnabled' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractActionsetEnabled_boolean'"
				this.setEnabled(true);
			}
			
			public override void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				Enclosing_Instance.viewer.setCenterSelected();
				Enclosing_Instance.setRotateMode();
				Enclosing_Instance.viewer.setSelectionHaloEnabled(false);
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'HomeAction' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class HomeAction:SupportClass.ActionSupport
		{
			private void  InitBlock(DisplayPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private DisplayPanel enclosingInstance;
			public DisplayPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			//UPGRADE_TODO: Constructor 'javax.swing.AbstractAction.AbstractAction' was converted to 'ActionSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingAbstractActionAbstractAction_javalangString'"
			public HomeAction(DisplayPanel enclosingInstance):base("home")
			{
				InitBlock(enclosingInstance);
				//UPGRADE_ISSUE: Method 'javax.swing.AbstractAction.setEnabled' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractActionsetEnabled_boolean'"
				this.setEnabled(true);
			}
			
			public override void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				Enclosing_Instance.viewer.homePosition();
				Enclosing_Instance.setRotateMode();
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'PerspectiveAction' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class PerspectiveAction:SupportClass.ActionSupport
		{
			private void  InitBlock(DisplayPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private DisplayPanel enclosingInstance;
			public DisplayPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			//UPGRADE_TODO: Constructor 'javax.swing.AbstractAction.AbstractAction' was converted to 'ActionSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingAbstractActionAbstractAction_javalangString'"
			public PerspectiveAction(DisplayPanel enclosingInstance):base("perspectiveCheck")
			{
				InitBlock(enclosingInstance);
				//UPGRADE_ISSUE: Method 'javax.swing.AbstractAction.setEnabled' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractActionsetEnabled_boolean'"
				this.setEnabled(true);
			}
			
			public override void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				System.Windows.Forms.MenuItem cbmi = (System.Windows.Forms.MenuItem) event_sender;
				Enclosing_Instance.viewer.PerspectiveDepth = cbmi.Checked;
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AxesAction' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class AxesAction:SupportClass.ActionSupport
		{
			private void  InitBlock(DisplayPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private DisplayPanel enclosingInstance;
			public DisplayPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			//UPGRADE_TODO: Constructor 'javax.swing.AbstractAction.AbstractAction' was converted to 'ActionSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingAbstractActionAbstractAction_javalangString'"
			public AxesAction(DisplayPanel enclosingInstance):base("axesCheck")
			{
				InitBlock(enclosingInstance);
				//UPGRADE_ISSUE: Method 'javax.swing.AbstractAction.setEnabled' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractActionsetEnabled_boolean'"
				this.setEnabled(true);
			}
			
			public override void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				System.Windows.Forms.MenuItem cbmi = (System.Windows.Forms.MenuItem) event_sender;
				Enclosing_Instance.viewer.ShowAxes = cbmi.Checked;
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'BoundboxAction' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class BoundboxAction:SupportClass.ActionSupport
		{
			private void  InitBlock(DisplayPanel enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private DisplayPanel enclosingInstance;
			public DisplayPanel Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			//UPGRADE_TODO: Constructor 'javax.swing.AbstractAction.AbstractAction' was converted to 'ActionSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingAbstractActionAbstractAction_javalangString'"
			public BoundboxAction(DisplayPanel enclosingInstance):base("boundboxCheck")
			{
				InitBlock(enclosingInstance);
				//UPGRADE_ISSUE: Method 'javax.swing.AbstractAction.setEnabled' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractActionsetEnabled_boolean'"
				this.setEnabled(true);
			}
			
			public override void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				System.Windows.Forms.MenuItem cbmi = (System.Windows.Forms.MenuItem) event_sender;
				Enclosing_Instance.viewer.ShowBbcage = cbmi.Checked;
			}
		}
		
		//UPGRADE_TODO: Interface 'javax.swing.event.MenuListener' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
		//UPGRADE_NOTE: The initialization of  'menuListener' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		internal MenuListener menuListener;
		
		internal virtual void  setDisplayMenuState()
		{
			guimap.setSelected("perspectiveCheck", viewer.PerspectiveDepth);
			guimap.setSelected("hydrogensCheck", viewer.ShowHydrogens);
			guimap.setSelected("measurementsCheck", viewer.ShowMeasurements);
			guimap.setSelected("axesCheck", viewer.ShowAxes);
			guimap.setSelected("boundboxCheck", viewer.ShowBbcage);
		}
		
		// code to record last and average times
		// last and average of all the previous times are shown in the status window
		
		private static int timeLast = 0;
		private static int timeCount;
		private static int timeTotal;
		
		private void  resetTimes()
		{
			timeCount = timeTotal = 0;
			timeLast = - 1;
		}
		
		private void  recordTime(int time)
		{
			if (timeLast != - 1)
			{
				timeTotal += timeLast;
				++timeCount;
			}
			timeLast = time;
		}
		
		private long timeBegin;
		private int lastMotionEventNumber;
		
		private void  startPaintClock()
		{
			timeBegin = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
			int motionEventNumber = viewer.MotionEventNumber;
			if (lastMotionEventNumber != motionEventNumber)
			{
				lastMotionEventNumber = motionEventNumber;
				resetTimes();
			}
		}
		
		private void  stopPaintClock()
		{
			int time = (int) ((System.DateTime.Now.Ticks - 621355968000000000) / 10000 - timeBegin);
			recordTime(time);
			showTimes();
		}
		
		private System.String fmt(int num)
		{
			if (num < 0)
				return "---";
			if (num < 10)
				return "  " + num;
			if (num < 100)
				return " " + num;
			return "" + num;
		}
		
		private void  showTimes()
		{
			int timeAverage = (timeCount == 0)?- 1:(timeTotal + timeCount / 2) / timeCount; // round, don't truncate
			if (displaySpeed.ToUpper().Equals("fps".ToUpper()))
			{
				status.setStatus(3, fmt(1000 / timeLast) + "FPS : " + fmt(1000 / timeAverage) + "FPS");
			}
			else
			{
				status.setStatus(3, fmt(timeLast) + "ms : " + fmt(timeAverage) + "ms");
			}
		}
		
		public const int X_AXIS = 1;
		public const int Y_AXIS = 2;
		public const int Z_AXIS = 3;
		
		public virtual void  rotate(int axis, double angle)
		{
			if (axis == X_AXIS)
			{
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				viewer.rotateToX((float) SupportClass.DegreesToRadians(angle));
			}
			else if (axis == Y_AXIS)
			{
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				viewer.rotateToY((float) SupportClass.DegreesToRadians(angle));
			}
			else if (axis == Z_AXIS)
			{
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				viewer.rotateToZ((float) SupportClass.DegreesToRadians(angle));
			}
		}
	}
}