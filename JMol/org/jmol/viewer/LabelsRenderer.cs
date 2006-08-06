/* $RCSfile$
* $Author: egonw $
* $Date: 2005-11-10 16:52:44 +0100 (jeu., 10 nov. 2005) $
* $Revision: 4255 $
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
using org.jmol.g3d;
namespace org.jmol.viewer
{
	
	class LabelsRenderer:ShapeRenderer
	{
		
		// offsets are from the font baseline
		internal sbyte fidPrevious;
		internal Font3D font3d;
		internal System.Drawing.Font fontMetrics;
		internal int ascent;
		internal int descent;
		internal int msgHeight;
		internal int msgWidth;
		
		internal override void  render()
		{
			fidPrevious = 0;
			
			Labels labels = (Labels) shape;
			System.String[] labelStrings = labels.strings;
			short[] colixes = labels.colixes;
			short[] bgcolixes = labels.bgcolixes;
			sbyte[] fids = labels.fids;
			short[] offsets = labels.offsets;
			if (labelStrings == null)
				return ;
			Atom[] atoms = frame.atoms;
			int displayModelIndex = this.displayModelIndex;
			for (int i = labelStrings.Length; --i >= 0; )
			{
				System.String label = labelStrings[i];
				if (label == null)
					continue;
				Atom atom = atoms[i];
				if (displayModelIndex >= 0 && displayModelIndex != atom.modelIndex)
					continue;
				short colix = (colixes == null || i >= colixes.Length)?0:colixes[i];
				short bgcolix = (bgcolixes == null || i >= bgcolixes.Length)?0:bgcolixes[i];
				sbyte fid = ((fids == null || i >= fids.Length || fids[i] == 0)?labels.defaultFont3D.fid:fids[i]);
				if (fid != fidPrevious)
				{
					g3d.setFont(fid);
					fidPrevious = fid;
					font3d = g3d.Font3DCurrent;
					fontMetrics = font3d.fontMetrics;
					//UPGRADE_TODO: The equivalent in .NET for method 'java.awt.FontMetrics.getAscent' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					ascent = SupportClass.GetAscent(fontMetrics);
					//UPGRADE_TODO: The equivalent in .NET for method 'java.awt.FontMetrics.getDescent' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					descent = SupportClass.GetDescent(fontMetrics);
					msgHeight = ascent + descent;
				}
				short offset = offsets == null || i >= offsets.Length?0:offsets[i];
				int xOffset, yOffset;
				if (offset == 0)
				{
					xOffset = JmolConstants.LABEL_DEFAULT_X_OFFSET;
					yOffset = JmolConstants.LABEL_DEFAULT_Y_OFFSET;
				}
				else if (offset == System.Int16.MinValue)
				{
					xOffset = yOffset = 0;
				}
				else
				{
					xOffset = offset >> 8;
					yOffset = (sbyte) (offset & 0xFF);
				}
				renderLabel(atom, label, colix, bgcolix, xOffset, yOffset);
			}
		}
		
		internal virtual void  renderLabel(Atom atom, System.String strLabel, short colix, short bgcolix, int labelOffsetX, int labelOffsetY)
		{
			//UPGRADE_ISSUE: Method 'java.awt.FontMetrics.stringWidth' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtFontMetricsstringWidth_javalangString'"
			int msgWidth = fontMetrics.stringWidth(strLabel);
			int boxWidth = msgWidth + 8;
			int boxHeight = msgHeight + 8;
			
			int xBoxOffset, yBoxOffset, zBox;
			zBox = atom.ScreenZ - atom.ScreenD / 2 - 2;
			if (zBox < 1)
				zBox = 1;
			
			if (labelOffsetX > 0)
			{
				xBoxOffset = labelOffsetX;
			}
			else
			{
				xBoxOffset = - boxWidth;
				if (labelOffsetX == 0)
					xBoxOffset /= 2;
				else
					xBoxOffset += labelOffsetX;
			}
			
			if (labelOffsetY < 0)
			{
				yBoxOffset = labelOffsetY;
			}
			else
			{
				if (labelOffsetY == 0)
					yBoxOffset = boxHeight / 2 + 2;
				else
					yBoxOffset = boxHeight + labelOffsetY;
			}
			int xBox = atom.ScreenX + xBoxOffset;
			int yBox = atom.ScreenY - yBoxOffset;
			colix = Graphics3D.inheritColix(colix, atom.colixAtom);
			if (bgcolix != 0)
			{
				g3d.fillRect(bgcolix, xBox, yBox, zBox, boxWidth, boxHeight);
				g3d.drawRect(colix, xBox + 1, yBox + 1, zBox - 1, boxWidth - 2, boxHeight - 2);
			}
			int msgX = xBox + 4;
			int msgYBaseline = yBox + 4 + ascent;
			g3d.drawString(strLabel, font3d, colix, msgX, msgYBaseline, zBox - 1);
		}
	}
}