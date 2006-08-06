/* $RCSfile$
* $Author: egonw $
* $Date: 2005-11-10 16:52:44 +0100 (jeu., 10 nov. 2005) $
* $Revision: 4255 $
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
*  Lesser General Public License for more details.
*
*  You should have received a copy of the GNU Lesser General Public
*  License along with this library; if not, write to the Free Software
*  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*/
using System;
using Graphics3D = org.jmol.g3d.Graphics3D;
namespace org.jmol.viewer
{
	
	abstract class ShapeRenderer
	{
		
		internal Viewer viewer;
		internal FrameRenderer frameRenderer;
		
		internal void  setViewerFrameRenderer(Viewer viewer, FrameRenderer frameRenderer, Graphics3D g3d)
		{
			this.viewer = viewer;
			this.frameRenderer = frameRenderer;
			this.g3d = g3d;
			initRenderer();
		}
		
		internal virtual void  initRenderer()
		{
		}
		
		internal Graphics3D g3d;
		internal System.Drawing.Rectangle rectClip;
		internal Frame frame;
		internal int displayModelIndex;
		internal Shape shape;
		
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		internal virtual void  render(Graphics3D g3d, ref System.Drawing.Rectangle rectClip, Frame frame, int displayModelIndex, Shape shape)
		{
			this.g3d = g3d;
			this.rectClip = rectClip;
			this.frame = frame;
			this.displayModelIndex = displayModelIndex;
			this.shape = shape;
			render();
		}
		
		internal abstract void  render();
	}
}