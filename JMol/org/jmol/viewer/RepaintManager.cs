/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-03-28 14:30:10 +0200 (mar., 28 mars 2006) $
* $Revision: 4828 $
*
* Copyright (C) 2003-2005  The Jmol Development Team
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
*  Lesser General License for more details.
*
*  You should have received a copy of the GNU Lesser General Public
*  License along with this library; if not, write to the Free Software
*  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*/
using System;
using org.jmol.g3d;
namespace org.jmol.viewer
{
	
	class RepaintManager
	{
		virtual internal int AnimationDirection
		{
			set
			{
				if (value == 1 || value == - 1)
				{
					this.animationDirection = currentDirection = value;
				}
				else
					System.Console.Out.WriteLine("invalid animationDirection:" + value);
			}
			
		}
		virtual internal int AnimationFps
		{
			set
			{
				if (value >= 1 && value <= 50)
					this.animationFps = value;
				else
					System.Console.Out.WriteLine("invalid animationFps:" + value);
			}
			
		}
		virtual internal bool InMotion
		{
			set
			{
				this.inMotion = value;
				if (!value)
					refresh();
			}
			
		}
		virtual internal bool Oversample
		{
			set
			{
				this.tOversample = value;
			}
			
		}
		
		internal Viewer viewer;
		internal FrameRenderer frameRenderer;
		
		internal RepaintManager(Viewer viewer)
		{
			this.viewer = viewer;
			frameRenderer = new FrameRenderer(viewer);
		}
		
		internal int displayModelIndex = 0;
		
		internal virtual bool setDisplayModelIndex(int modelIndex)
		{
			Frame frame = viewer.Frame;
			if (frame == null || modelIndex < 0 || modelIndex >= frame.ModelCount)
				displayModelIndex = - 1;
			else
				displayModelIndex = modelIndex;
			this.displayModelIndex = modelIndex;
			viewer.notifyFrameChanged(modelIndex);
			return true;
		}
		
		internal int animationDirection = 1;
		internal int currentDirection = 1;
		
		internal int animationFps = 10;
		
		// 0 = once
		// 1 = loop
		// 2 = palindrome
		internal int animationReplayMode = 0;
		internal float firstFrameDelay, lastFrameDelay;
		internal int firstFrameDelayMs, lastFrameDelayMs;
		
		internal virtual void  setAnimationReplayMode(int animationReplayMode, float firstFrameDelay, float lastFrameDelay)
		{
			//System.out.println("animationReplayMode=" + animationReplayMode);
			this.firstFrameDelay = firstFrameDelay > 0?firstFrameDelay:0;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			firstFrameDelayMs = (int) (this.firstFrameDelay * 1000);
			this.lastFrameDelay = lastFrameDelay > 0?lastFrameDelay:0;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			lastFrameDelayMs = (int) (this.lastFrameDelay * 1000);
			if (animationReplayMode >= 0 && animationReplayMode <= 2)
				this.animationReplayMode = animationReplayMode;
			else
				System.Console.Out.WriteLine("invalid animationReplayMode:" + animationReplayMode);
		}
		
		internal virtual bool setAnimationRelative(int direction)
		{
			if (displayModelIndex < 0)
				return false;
			int modelIndexNext = displayModelIndex + (direction * currentDirection);
			int modelCount = viewer.ModelCount;
			
			/*
			System.out.println("setAnimationRelative: displayModelID=" +
			displayModelID +
			" displayModelIndex=" + displayModelIndex +
			" currentDirection=" + currentDirection +
			" direction=" + direction +
			" modelIndexNext=" + modelIndexNext +
			" modelCount=" + modelCount +
			" animationReplayMode=" + animationReplayMode +
			" animationDirection=" + animationDirection);
			*/
			
			if (modelIndexNext == modelCount)
			{
				switch (animationReplayMode)
				{
					
					case 0: 
						return false;
					
					case 1: 
						modelIndexNext = 0;
						break;
					
					case 2: 
						currentDirection = - 1;
						modelIndexNext = modelCount - 2;
						break;
					}
			}
			else if (modelIndexNext < 0)
			{
				switch (animationReplayMode)
				{
					
					case 0: 
						return false;
					
					case 1: 
						modelIndexNext = modelCount - 1;
						break;
					
					case 2: 
						currentDirection = 1;
						modelIndexNext = 1;
						break;
					}
			}
			setDisplayModelIndex(modelIndexNext);
			return true;
		}
		
		internal virtual bool setAnimationNext()
		{
			return setAnimationRelative(animationDirection);
		}
		
		internal virtual bool setAnimationPrevious()
		{
			return setAnimationRelative(- animationDirection);
		}
		
		internal bool inMotion = false;
		
		internal virtual System.Drawing.Image takeSnapshot()
		{
			return null;
			//return awtComponent.takeSnapshot();
		}
		
		internal int holdRepaint = 0;
		internal bool repaintPending;
		
		internal virtual void  pushHoldRepaint()
		{
			++holdRepaint;
			//    System.out.println("pushHoldRepaint:" + holdRepaint);
		}
		
		internal virtual void  popHoldRepaint()
		{
			--holdRepaint;
			//    System.out.println("popHoldRepaint:" + holdRepaint);
			if (holdRepaint <= 0)
			{
				holdRepaint = 0;
				repaintPending = true;
				// System.out.println("popHoldRepaint called awtComponent.repaint()");
				//UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
				viewer.awtComponent.Refresh();
			}
		}
		
		internal virtual void  forceRefresh()
		{
			repaintPending = true;
			//UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
			viewer.awtComponent.Refresh();
		}
		
		internal virtual void  refresh()
		{
			if (repaintPending)
				return ;
			repaintPending = true;
			if (holdRepaint == 0)
			{
				//UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
				viewer.awtComponent.Refresh();
			}
		}
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'requestRepaintAndWait'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		internal virtual void  requestRepaintAndWait()
		{
			lock (this)
			{
				//UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
				viewer.awtComponent.Refresh();
				try
				{
					System.Threading.Monitor.Wait(this);
				}
				catch (System.Threading.ThreadInterruptedException e)
				{
				}
			}
		}
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'notifyRepainted'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		internal virtual void  notifyRepainted()
		{
			lock (this)
			{
				repaintPending = false;
				System.Threading.Monitor.Pulse(this);
			}
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'rectOversample '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal System.Drawing.Rectangle rectOversample = new System.Drawing.Rectangle();
		internal bool tOversample;
		
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		internal virtual void  render(Graphics3D g3d, ref System.Drawing.Rectangle rectClip, Frame frame, int displayModelID)
		{
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
			frameRenderer.render(g3d, ref rectClip, frame, displayModelID);
			viewer.checkCameraDistance();
			System.Drawing.Rectangle band = viewer.RubberBandSelection;
			if (!band.IsEmpty)
				g3d.drawRect(viewer.ColixRubberband, band.X, band.Y, 0, band.Width, band.Height);
		}
		
		/// <summary>*************************************************************
		/// Animation support
		/// **************************************************************
		/// </summary>
		
		internal virtual void  clearAnimation()
		{
			setAnimationOn(false);
			setDisplayModelIndex(0);
			AnimationDirection = 1;
			AnimationFps = 10;
			setAnimationReplayMode(0, 0, 0);
		}
		
		internal bool animationOn = false;
		internal AnimationThread animationThread;
		
		internal virtual void  setAnimationOn(bool animationOn)
		{
			setAnimationOn(animationOn, - 1);
		}
		internal virtual void  setAnimationOn(bool animationOn, int framePointer)
		{
			if (!animationOn || !viewer.haveFrame())
			{
				if (animationThread != null)
				{
					animationThread.Interrupt();
					animationThread = null;
				}
				this.animationOn = false;
				return ;
			}
			int modelCount = viewer.ModelCount;
			if (modelCount <= 1)
			{
				this.animationOn = false;
				return ;
			}
			currentDirection = animationDirection;
			setDisplayModelIndex(framePointer >= 0?framePointer:(animationDirection == 1?0:modelCount - 1));
			if (animationThread == null)
			{
				animationThread = new AnimationThread(this, modelCount);
				animationThread.Start();
			}
			this.animationOn = true;
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnimationThread' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		internal class AnimationThread:SupportClass.ThreadClass, IThreadRunnable
		{
			private void  InitBlock(RepaintManager enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private RepaintManager enclosingInstance;
			public RepaintManager Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			//UPGRADE_NOTE: Final was removed from the declaration of 'modelCount '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
			internal int modelCount;
			//UPGRADE_NOTE: Final was removed from the declaration of 'lastModelIndex '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
			internal int lastModelIndex;
			internal AnimationThread(RepaintManager enclosingInstance, int modelCount)
			{
				InitBlock(enclosingInstance);
				this.modelCount = modelCount;
				lastModelIndex = modelCount - 1;
			}
			
			override public void  Run()
			{
				long timeBegin = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
				int targetTime = 0;
				int sleepTime;
				Enclosing_Instance.requestRepaintAndWait();
				try
				{
					sleepTime = targetTime - (int) ((System.DateTime.Now.Ticks - 621355968000000000) / 10000 - timeBegin);
					if (sleepTime > 0)
					{
						//UPGRADE_TODO: Method 'java.lang.Thread.sleep' was converted to 'System.Threading.Thread.Sleep' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangThreadsleep_long'"
						System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64) 10000 * sleepTime));
					}
					//UPGRADE_ISSUE: Method 'java.lang.Thread.isInterrupted' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangThreadisInterrupted'"
					while (!isInterrupted())
					{
						if (Enclosing_Instance.displayModelIndex == 0)
						{
							targetTime += Enclosing_Instance.firstFrameDelayMs;
							sleepTime = targetTime - (int) ((System.DateTime.Now.Ticks - 621355968000000000) / 10000 - timeBegin);
							if (sleepTime > 0)
							{
								//UPGRADE_TODO: Method 'java.lang.Thread.sleep' was converted to 'System.Threading.Thread.Sleep' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangThreadsleep_long'"
								System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64) 10000 * sleepTime));
							}
						}
						if (Enclosing_Instance.displayModelIndex == lastModelIndex)
						{
							targetTime += Enclosing_Instance.lastFrameDelayMs;
							sleepTime = targetTime - (int) ((System.DateTime.Now.Ticks - 621355968000000000) / 10000 - timeBegin);
							if (sleepTime > 0)
							{
								//UPGRADE_TODO: Method 'java.lang.Thread.sleep' was converted to 'System.Threading.Thread.Sleep' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangThreadsleep_long'"
								System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64) 10000 * sleepTime));
							}
						}
						if (!Enclosing_Instance.setAnimationNext())
						{
							Enclosing_Instance.setAnimationOn(false);
							return ;
						}
						targetTime += (1000 / Enclosing_Instance.animationFps);
						sleepTime = targetTime - (int) ((System.DateTime.Now.Ticks - 621355968000000000) / 10000 - timeBegin);
						if (sleepTime < 0)
							continue;
						Enclosing_Instance.refresh();
						sleepTime = targetTime - (int) ((System.DateTime.Now.Ticks - 621355968000000000) / 10000 - timeBegin);
						if (sleepTime > 0)
						{
							//UPGRADE_TODO: Method 'java.lang.Thread.sleep' was converted to 'System.Threading.Thread.Sleep' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangThreadsleep_long'"
							System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64) 10000 * sleepTime));
						}
					}
				}
				catch (System.Threading.ThreadInterruptedException ie)
				{
					System.Console.Out.WriteLine("animation interrupted!");
				}
			}
		}
	}
}