/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-03-30 20:00:29 +0200 (jeu., 30 mars 2006) $
* $Revision: 4849 $
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
namespace org.jmol.viewer
{
	
	abstract class MouseManager
	{
		virtual internal System.Drawing.Rectangle RubberBand
		{
			get
			{
				if (!rubberbandSelectionMode)
					return System.Drawing.Rectangle.Empty;
				return rectRubber;
			}
			
		}
		virtual internal int AttractiveMeasurementTarget
		{
			// the attractive target may be -1
			
			set
			{
				if (measurementCountPlusIndices[0] == measurementCount + 1 && measurementCountPlusIndices[measurementCount + 1] == value)
				{
					viewer.refresh();
					return ;
				}
				for (int i = measurementCount; i > 0; --i)
					if (measurementCountPlusIndices[i] == value)
					{
						viewer.refresh();
						return ;
					}
				int attractiveCount = measurementCount + 1;
				measurementCountPlusIndices[0] = attractiveCount;
				measurementCountPlusIndices[attractiveCount] = value;
				viewer.PendingMeasurement = measurementCountPlusIndices;
			}
			
		}
		
		internal const int HOVER_TIME = 1000;
		
		internal System.Windows.Forms.Control component;
		internal Viewer viewer;
		
		internal SupportClass.ThreadClass hoverWatcherThread;
		
		internal int previousDragX, previousDragY;
		internal int xCurrent, yCurrent;
		internal long timeCurrent;
		
		internal int modifiersWhenPressed;
		internal bool wasDragged;
		
		internal bool measurementMode = false;
		internal bool hoverActive = false;
		
		internal bool rubberbandSelectionMode = false;
		internal int xAnchor, yAnchor;
		//UPGRADE_NOTE: Final was removed from the declaration of 'rectRubber '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.Drawing.Rectangle rectRubber = new System.Drawing.Rectangle();
		
		private const bool logMouseEvents = false;
		
		internal MouseManager(System.Windows.Forms.Control component, Viewer viewer)
		{
			this.component = component;
			this.viewer = viewer;
			hoverWatcherThread = new SupportClass.ThreadClass(new System.Threading.ThreadStart(new HoverWatcher(this).Run));
			hoverWatcherThread.Start();
		}
		
		internal virtual void  calcRectRubberBand()
		{
			if (xCurrent < xAnchor)
			{
				rectRubber.X = xCurrent;
				rectRubber.Width = xAnchor - xCurrent;
			}
			else
			{
				rectRubber.X = xAnchor;
				rectRubber.Width = xCurrent - xAnchor;
			}
			if (yCurrent < yAnchor)
			{
				rectRubber.Y = yCurrent;
				rectRubber.Height = yAnchor - yCurrent;
			}
			else
			{
				rectRubber.Y = yAnchor;
				rectRubber.Height = yCurrent - yAnchor;
			}
		}
		
		internal const long MAX_DOUBLE_CLICK_MILLIS = 700;
		
		internal const int LEFT = 16;
		//UPGRADE_NOTE: Final was removed from the declaration of 'MIDDLE '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_TODO: The equivalent in .NET for field 'java.awt.Event.ALT_MASK' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
		internal static readonly int MIDDLE = (int) System.Windows.Forms.Keys.Alt; // 8 note that MIDDLE
		//UPGRADE_NOTE: Final was removed from the declaration of 'ALT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_TODO: The equivalent in .NET for field 'java.awt.Event.ALT_MASK' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
		internal static readonly int ALT = (int) System.Windows.Forms.Keys.Alt; // 8 and ALT are the same
		//UPGRADE_NOTE: Final was removed from the declaration of 'RIGHT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_ISSUE: Field 'java.awt.Event.META_MASK' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
		internal static readonly int RIGHT = Event.META_MASK; // 4
		//UPGRADE_NOTE: Final was removed from the declaration of 'CTRL '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_TODO: The equivalent in .NET for field 'java.awt.Event.CTRL_MASK' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
		internal static readonly int CTRL = (int) System.Windows.Forms.Keys.ControlKey; // 2
		//UPGRADE_NOTE: Final was removed from the declaration of 'SHIFT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_TODO: The equivalent in .NET for field 'java.awt.Event.SHIFT_MASK' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
		internal static readonly int SHIFT = (int) System.Windows.Forms.Keys.ShiftKey; // 1
		//UPGRADE_NOTE: Final was removed from the declaration of 'MIDDLE_RIGHT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int MIDDLE_RIGHT = MIDDLE | RIGHT;
		//UPGRADE_NOTE: Final was removed from the declaration of 'LEFT_MIDDLE_RIGHT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int LEFT_MIDDLE_RIGHT = LEFT | MIDDLE | RIGHT;
		//UPGRADE_NOTE: Final was removed from the declaration of 'CTRL_SHIFT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int CTRL_SHIFT = CTRL | SHIFT;
		//UPGRADE_NOTE: Final was removed from the declaration of 'CTRL_LEFT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int CTRL_LEFT = CTRL | LEFT;
		//UPGRADE_NOTE: Final was removed from the declaration of 'CTRL_RIGHT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int CTRL_RIGHT = CTRL | RIGHT;
		//UPGRADE_NOTE: Final was removed from the declaration of 'CTRL_MIDDLE '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int CTRL_MIDDLE = CTRL | MIDDLE;
		//UPGRADE_NOTE: Final was removed from the declaration of 'CTRL_ALT_LEFT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int CTRL_ALT_LEFT = CTRL | ALT | LEFT;
		//UPGRADE_NOTE: Final was removed from the declaration of 'ALT_LEFT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int ALT_LEFT = ALT | LEFT;
		//UPGRADE_NOTE: Final was removed from the declaration of 'ALT_SHIFT_LEFT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int ALT_SHIFT_LEFT = ALT | SHIFT | LEFT;
		//UPGRADE_NOTE: Final was removed from the declaration of 'SHIFT_LEFT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int SHIFT_LEFT = SHIFT | LEFT;
		//UPGRADE_NOTE: Final was removed from the declaration of 'CTRL_SHIFT_LEFT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int CTRL_SHIFT_LEFT = CTRL | SHIFT | LEFT;
		//UPGRADE_NOTE: Final was removed from the declaration of 'CTRL_ALT_SHIFT_LEFT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int CTRL_ALT_SHIFT_LEFT = CTRL | ALT | SHIFT | LEFT;
		//UPGRADE_NOTE: Final was removed from the declaration of 'SHIFT_MIDDLE '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int SHIFT_MIDDLE = SHIFT | MIDDLE;
		//UPGRADE_NOTE: Final was removed from the declaration of 'CTRL_SHIFT_MIDDLE '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int CTRL_SHIFT_MIDDLE = CTRL | SHIFT | MIDDLE;
		//UPGRADE_NOTE: Final was removed from the declaration of 'SHIFT_RIGHT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int SHIFT_RIGHT = SHIFT | RIGHT;
		//UPGRADE_NOTE: Final was removed from the declaration of 'CTRL_SHIFT_RIGHT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int CTRL_SHIFT_RIGHT = CTRL | SHIFT | RIGHT;
		//UPGRADE_NOTE: Final was removed from the declaration of 'CTRL_ALT_SHIFT_RIGHT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int CTRL_ALT_SHIFT_RIGHT = CTRL | ALT | SHIFT | RIGHT;
		//UPGRADE_NOTE: Final was removed from the declaration of 'BUTTON_MODIFIER_MASK '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int BUTTON_MODIFIER_MASK = CTRL | ALT | SHIFT | LEFT | MIDDLE | RIGHT;
		
		internal int previousPressedX, previousPressedY;
		internal int previousPressedModifiers, previousPressedCount;
		internal long previousPressedTime;
		internal int pressedCount;
		
		internal virtual void  mousePressed(long time, int x, int y, int modifiers, bool isPopupTrigger)
		{
			if (previousPressedX == x && previousPressedY == y && previousPressedModifiers == modifiers && (time - previousPressedTime) < MAX_DOUBLE_CLICK_MILLIS)
			{
				++pressedCount;
			}
			else
			{
				pressedCount = 1;
			}
			
			hoverOff();
			previousPressedX = previousDragX = xCurrent = x;
			previousPressedY = previousDragY = yCurrent = y;
			previousPressedModifiers = modifiers;
			previousPressedTime = timeCurrent = time;
			
			if (logMouseEvents)
				System.Console.Out.WriteLine("mousePressed(" + x + "," + y + "," + modifiers + " isPopupTrigger=" + isPopupTrigger + ")");
			
			modifiersWhenPressed = modifiers;
			wasDragged = false;
			
			switch (modifiers & BUTTON_MODIFIER_MASK)
			{
				
				/****************************************************************
				* mth 2004 03 17
				* this isPopupTrigger stuff just doesn't work reliably for me
				* and I don't have a Mac to test out CTRL-CLICK behavior
				* Therefore ... we are going to implement both gestures
				* to bring up the popup menu
				* The fact that we are using CTRL_LEFT may 
				* interfere with other platforms if/when we
				* need to support multiple selections, but we will
				* cross that bridge when we come to it
				****************************************************************/
				case CTRL_LEFT: 
				// on MacOSX this brings up popup
				case RIGHT:  // with multi-button mice, this will too
					viewer.popupMenu(x, y);
					return ;
				}
		}
		
		internal virtual void  mouseEntered(long time, int x, int y)
		{
			if (logMouseEvents)
				System.Console.Out.WriteLine("mouseEntered(" + x + "," + y + ")");
			hoverOff();
			timeCurrent = time;
			xCurrent = x; yCurrent = y;
		}
		
		internal virtual void  mouseExited(long time, int x, int y)
		{
			if (logMouseEvents)
				System.Console.Out.WriteLine("mouseExited(" + x + "," + y + ")");
			hoverOff();
			timeCurrent = time;
			xCurrent = x; yCurrent = y;
			exitMeasurementMode();
		}
		
		internal virtual void  mouseReleased(long time, int x, int y, int modifiers)
		{
			hoverOff();
			timeCurrent = time;
			xCurrent = x; yCurrent = y;
			if (logMouseEvents)
				System.Console.Out.WriteLine("mouseReleased(" + x + "," + y + "," + modifiers + ")");
			viewer.InMotion = false;
		}
		
		internal int previousClickX, previousClickY;
		internal int previousClickModifiers, previousClickCount;
		internal long previousClickTime;
		
		internal virtual void  clearClickCount()
		{
			previousClickX = - 1;
		}
		
		internal virtual void  mouseClicked(long time, int x, int y, int modifiers, int clickCount)
		{
			// clickCount is not reliable on some platforms
			// so we will just deal with it ourselves
			clickCount = 1;
			if (previousClickX == x && previousClickY == y && previousClickModifiers == modifiers && (time - previousClickTime) < MAX_DOUBLE_CLICK_MILLIS)
			{
				clickCount = previousClickCount + 1;
			}
			hoverOff();
			xCurrent = previousClickX = x; yCurrent = previousClickY = y;
			previousClickModifiers = modifiers;
			previousClickCount = clickCount;
			timeCurrent = previousClickTime = time;
			
			if (logMouseEvents)
				System.Console.Out.WriteLine("mouseClicked(" + x + "," + y + "," + modifiers + ",clickCount=" + clickCount + ",time=" + (time - previousClickTime) + ")");
			if (!viewer.haveFrame())
				return ;
			
			int nearestAtomIndex = viewer.findNearestAtomIndex(x, y);
			if (clickCount == 1)
				mouseSingleClick(x, y, modifiers, nearestAtomIndex);
			else if (clickCount == 2)
				mouseDoubleClick(x, y, modifiers, nearestAtomIndex);
		}
		
		internal virtual void  mouseSingleClick(int x, int y, int modifiers, int nearestAtomIndex)
		{
			if (logMouseEvents)
				System.Console.Out.WriteLine("mouseSingleClick(" + x + "," + y + "," + modifiers + " nearestAtom=" + nearestAtomIndex);
			switch (modifiers & BUTTON_MODIFIER_MASK)
			{
				
				case LEFT: 
					if (viewer.frankClicked(x, y))
					{
						viewer.popupMenu(x, y);
						return ;
					}
					viewer.atomPicked(nearestAtomIndex, false);
					if (measurementMode)
					{
						addToMeasurement(nearestAtomIndex, false);
					}
					break;
				
				case SHIFT_LEFT: 
					viewer.atomPicked(nearestAtomIndex, true);
					break;
				}
		}
		
		internal virtual void  mouseDoubleClick(int x, int y, int modifiers, int nearestAtomIndex)
		{
			switch (modifiers & BUTTON_MODIFIER_MASK)
			{
				
				case LEFT: 
					if (measurementMode)
					{
						addToMeasurement(nearestAtomIndex, true);
						toggleMeasurement();
					}
					else
					{
						enterMeasurementMode();
						addToMeasurement(nearestAtomIndex, true);
					}
					break;
				
				case ALT_LEFT: 
				case MIDDLE: 
				case SHIFT_LEFT: 
					if (nearestAtomIndex < 0)
						viewer.homePosition();
					break;
				}
		}
		
		internal virtual void  mouseDragged(long time, int x, int y, int modifiers)
		{
			if (logMouseEvents)
				System.Console.Out.WriteLine("mouseDragged(" + x + "," + y + "," + modifiers + ")");
			int deltaX = x - previousDragX;
			int deltaY = y - previousDragY;
			hoverOff();
			timeCurrent = time;
			xCurrent = previousDragX = x; yCurrent = previousDragY = y;
			wasDragged = true;
			viewer.InMotion = true;
			if (pressedCount == 1)
				mouseSinglePressDrag(deltaX, deltaY, modifiers);
			else if (pressedCount == 2)
				mouseDoublePressDrag(deltaX, deltaY, modifiers);
		}
		
		internal virtual void  mouseSinglePressDrag(int deltaX, int deltaY, int modifiers)
		{
			switch (modifiers & BUTTON_MODIFIER_MASK)
			{
				
				case LEFT: 
					viewer.rotateXYBy(deltaX, deltaY);
					break;
				
				case SHIFT_LEFT: 
				case ALT_LEFT: 
				case MIDDLE: 
					viewer.zoomBy(deltaY);
					// fall into
					goto case SHIFT_RIGHT;
				
				case SHIFT_RIGHT:  // the one-button Mac folks won't get this gesture
					viewer.rotateZBy(- deltaX);
					break;
				
				case CTRL_ALT_LEFT: 
				/*
				* miguel 2004 11 23
				* CTRL_ALT_LEFT *should* work on the mac
				* however, Apple has a bug in that mouseDragged events
				* do not get passed through if the CTL button is held down
				*
				* I submitted a bug to apple
				*/
				case CTRL_RIGHT: 
					viewer.translateXYBy(deltaX, deltaY);
					break;
				
				case CTRL_SHIFT_LEFT: 
					if (viewer.SlabEnabled)
						viewer.slabByPixels(deltaY);
					break;
				
				case CTRL_ALT_SHIFT_LEFT: 
					if (viewer.SlabEnabled)
						viewer.slabDepthByPixels(deltaY);
					break;
				}
		}
		
		internal virtual void  mouseDoublePressDrag(int deltaX, int deltaY, int modifiers)
		{
			switch (modifiers & BUTTON_MODIFIER_MASK)
			{
				
				case SHIFT_LEFT: 
				case ALT_LEFT: 
				case MIDDLE: 
					viewer.translateXYBy(deltaX, deltaY);
					break;
				
				case CTRL_SHIFT_LEFT: 
					if (viewer.SlabEnabled)
						viewer.depthByPixels(deltaY);
					break;
				}
		}
		
		
		/*
		int getMode(int modifiers) {
		if (modeMouse != JmolConstants.MOUSE_ROTATE)
		return modeMouse;
		/* RASMOL
		// mth - I think that right click should be reserved for a popup menu
		if ((modifiers & CTRL_LEFT) == CTRL_LEFT)
		return SLAB_PLANE;
		if ((modifiers & SHIFT_LEFT) == SHIFT_LEFT)
		return ZOOM;
		if ((modifiers & SHIFT_RIGHT) == SHIFT_RIGHT)
		return ROTATE_Z;
		if ((modifiers & RIGHT) == RIGHT)
		return XLATE;
		if ((modifiers & LEFT) == LEFT)
		mol is a collaboratively developed visualization an    return ROTATE;
		/
		if ((modifiers & SHIFT_RIGHT) == SHIFT_RIGHT)
		return JmolConstants.MOUSE_ROTATE_Z;
		if ((modifiers & CTRL_RIGHT) == CTRL_RIGHT)
		return JmolConstants.MOUSE_XLATE;
		if ((modifiers & RIGHT) == RIGHT)
		return JmolConstants.MOUSE_POPUP_MENU;
		if ((modifiers & SHIFT_LEFT) == SHIFT_LEFT)
		return JmolConstants.MOUSE_ZOOM;
		if ((modifiers & CTRL_LEFT) == CTRL_LEFT)
		return JmolConstants.MOUSE_SLAB_PLANE;
		if ((modifiers & LEFT) == LEFT)
		return JmolConstants.MOUSE_ROTATE;
		return modeMouse;
		}
		
		void mouseDragged(int x, int y, int modifiers) {
		xCurrent = x; yCurrent = y;
		wasDragged = true;
		viewer.setInMotion(true);
		switch (getMode(modifiers)) {
		case JmolConstants.MOUSE_MEASURE:
		case JmolConstants.MOUSE_ROTATE:
		//if (logMouseEvents)
		//System.out.println("mouseDragged Rotate("+x+","+y+","+modifiers+")");
		viewer.rotateXYBy(xCurrent - xPrevious, yCurrent - yPrevious);
		break;
		case JmolConstants.MOUSE_ROTATE_Z:
		//if (logMouseEvents)
		//System.out.println("mouseDragged RotateZ("+x+","+y+","+modifiers+")");
		viewer.rotateZBy(xPrevious - xCurrent);
		break;
		case JmolConstants.MOUSE_XLATE:
		//if (logMouseEvents)
		//System.out.println("mouseDragged Translate("+x+","+y+","+modifiers+")");
		viewer.translateXYBy(xCurrent - xPrevious, yCurrent - yPrevious);
		break;
		case JmolConstants.MOUSE_ZOOM:
		//if (logMouseEvents)
		//System.out.println("mouseDragged Zoom("+x+","+y+","+modifiers+")");
		viewer.zoomBy(yCurrent - yPrevious);
		break;
		case JmolConstants.MOUSE_SLAB_PLANE:
		viewer.slabBy(yCurrent - yPrevious);
		break;
		case JmolConstants.MOUSE_PICK:
		if (viewer.haveFrame()) {
		calcRectRubberBand();
		BitSet selectedAtoms = viewer.findAtomsInRectangle(rectRubber);
		if ((modifiers & SHIFT) != 0) {
		viewer.addSelection(selectedAtoms);
		} else {
		viewer.setSelectionSet(selectedAtoms);
		}
		}
		break;
		case JmolConstants.MOUSE_POPUP_MENU:
		break;
		}
		xPrevious = xCurrent;
		yPrevious = yCurrent;
		}
		*/
		
		internal int mouseMovedX, mouseMovedY;
		internal long mouseMovedTime;
		
		internal virtual void  mouseMoved(long time, int x, int y, int modifiers)
		{
			/*
			if (logMouseEvents)
			System.out.println("mouseMoved("+x+","+y+","+modifiers"+)");
			*/
			hoverOff();
			timeCurrent = mouseMovedTime = time;
			mouseMovedX = xCurrent = x; mouseMovedY = yCurrent = y;
			if (measurementMode | hoverActive)
			{
				int atomIndex = viewer.findNearestAtomIndex(x, y);
				if (measurementMode)
					AttractiveMeasurementTarget = atomIndex;
			}
		}
		
		internal const float wheelClickFractionUp = 1.25f;
		//UPGRADE_NOTE: Final was removed from the declaration of 'wheelClickFractionDown '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly float wheelClickFractionDown = 1 / wheelClickFractionUp;
		
		internal virtual void  mouseWheel(long time, int rotation, int modifiers)
		{
			hoverOff();
			timeCurrent = time;
			if (rotation == 0)
				return ;
			if ((modifiers & BUTTON_MODIFIER_MASK) == 0)
			{
				float zoomLevel = viewer.ZoomPercentSetting / 100f;
				if (rotation > 0)
				{
					while (--rotation >= 0)
						zoomLevel *= wheelClickFractionUp;
				}
				else
				{
					while (++rotation <= 0)
						zoomLevel *= wheelClickFractionDown;
				}
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				viewer.zoomToPercent((int) (zoomLevel * 100 + 0.5f));
			}
		}
		
		
		//UPGRADE_ISSUE: Class 'java.awt.Event' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
		internal abstract bool handleOldJvm10Event(Event e);
		
		// note that these two may *not* be consistent
		// this term refers to the count of what has actually been selected
		internal int measurementCount = 0;
		// measurementCountPlusIndices[0] may be one higher if there is
		// an attractive measurement target
		// ie. the cursor is hovering near an atom
		internal int[] measurementCountPlusIndices = new int[5];
		
		internal virtual void  addToMeasurement(int atomIndex, bool dblClick)
		{
			if (atomIndex == - 1)
			{
				exitMeasurementMode();
				return ;
			}
			for (int i = measurementCount; --i >= 0; )
				if (measurementCountPlusIndices[i + 1] == atomIndex)
				{
					//        exitMeasurementMode();
					return ;
				}
			if (measurementCount == 3 && !dblClick)
				return ;
			measurementCountPlusIndices[++measurementCount] = atomIndex;
			measurementCountPlusIndices[0] = measurementCount;
			if (measurementCount == 4)
				toggleMeasurement();
			else
				viewer.PendingMeasurement = measurementCountPlusIndices;
		}
		
		internal virtual void  exitMeasurementMode()
		{
			if (measurementMode)
			{
				viewer.PendingMeasurement = null;
				measurementMode = false;
				measurementCount = 0;
				//UPGRADE_ISSUE: Member 'java.awt.Cursor.getDefaultCursor' was converted to 'System.Windows.Forms.Cursors.Default' which cannot be assigned to an int. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1086'"
				viewer.AwtComponent.Cursor = System.Windows.Forms.Cursors.Default;
			}
		}
		
		internal virtual void  enterMeasurementMode()
		{
			//UPGRADE_ISSUE: Member 'java.awt.Cursor.getPredefinedCursor' was converted to 'System.Windows.Forms.Cursor' which cannot be assigned to an int. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1086'"
			//UPGRADE_ISSUE: Member 'java.awt.Cursor.CROSSHAIR_CURSOR' was converted to 'System.Windows.Forms.Cursors.Cross' which cannot be assigned to an int. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1086'"
			viewer.AwtComponent.Cursor = System.Windows.Forms.Cursors.Cross;
			measurementCount = 0;
			measurementMode = true;
		}
		
		internal virtual void  toggleMeasurement()
		{
			if (measurementCount >= 2 && measurementCount <= 4)
			{
				measurementCountPlusIndices[0] = measurementCount;
				viewer.toggleMeasurement(measurementCountPlusIndices);
			}
			exitMeasurementMode();
		}
		
		internal virtual void  hoverOn(int atomIndex)
		{
			viewer.hoverOn(atomIndex);
		}
		
		internal virtual void  hoverOff()
		{
			viewer.hoverOff();
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'HoverWatcher' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		internal class HoverWatcher : IThreadRunnable
		{
			public HoverWatcher(MouseManager enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(MouseManager enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private MouseManager enclosingInstance;
			public MouseManager Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public virtual void  Run()
			{
				//UPGRADE_TODO: The differences in the type  of parameters for method 'java.lang.Thread.setPriority'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
				SupportClass.ThreadClass.Current().Priority = (System.Threading.ThreadPriority) System.Threading.ThreadPriority.Lowest;
				while (true)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.lang.Thread.sleep' was converted to 'System.Threading.Thread.Sleep' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangThreadsleep_long'"
						System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64) 10000 * 1000));
						if (Enclosing_Instance.xCurrent == Enclosing_Instance.mouseMovedX && Enclosing_Instance.yCurrent == Enclosing_Instance.mouseMovedY && Enclosing_Instance.timeCurrent == Enclosing_Instance.mouseMovedTime)
						{
							// the last event was mouse move
							long currentTime = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
							int howLong = (int) (currentTime - Enclosing_Instance.mouseMovedTime);
							if (howLong > org.jmol.viewer.MouseManager.HOVER_TIME)
							{
								int atomIndex = Enclosing_Instance.viewer.findNearestAtomIndex(Enclosing_Instance.xCurrent, Enclosing_Instance.yCurrent);
								if (atomIndex != - 1)
									Enclosing_Instance.hoverOn(atomIndex);
							}
						}
					}
					catch (System.Threading.ThreadInterruptedException ie)
					{
						System.Console.Out.WriteLine("InterruptedException!");
						return ;
					}
				}
			}
		}
	}
}