/* $RCSfile$
* $Author: hansonr $
* $Date: 2006-04-06 13:21:56 +0200 (jeu., 06 avr. 2006) $
* $Revision: 4923 $
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
namespace org.jmol.viewer
{
	
	class PickingManager
	{
		private void  InitBlock()
		{
			pickingMode = JmolConstants.PICKING_IDENT;
		}
		virtual internal int PickingMode
		{
			set
			{
				this.pickingMode = value;
				queuedAtomCount = 0;
				System.Console.Out.WriteLine("setPickingMode(" + value + ":" + JmolConstants.pickingModeNames[value] + ")");
			}
			
		}
		
		internal Viewer viewer;
		
		//UPGRADE_NOTE: The initialization of  'pickingMode' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		internal int pickingMode;
		
		internal bool chimeStylePicking = true;
		
		internal int queuedAtomCount = 0;
		internal int[] queuedAtomIndexes = new int[4];
		
		internal int[] countPlusIndexes = new int[5];
		
		internal PickingManager(Viewer viewer)
		{
			InitBlock();
			this.viewer = viewer;
		}
		
		internal virtual void  atomPicked(int atomIndex, bool shiftKey)
		{
			if (atomIndex == - 1)
				return ;
			Frame frame = viewer.Frame;
			switch (pickingMode)
			{
				
				case JmolConstants.PICKING_OFF: 
					break;
				
				case JmolConstants.PICKING_IDENT: 
					viewer.notifyAtomPicked(atomIndex);
					break;
				
				case JmolConstants.PICKING_DISTANCE: 
					if (queuedAtomCount >= 2)
						queuedAtomCount = 0;
					queueAtom(atomIndex);
					if (queuedAtomCount < 2)
						break;
					float distance = frame.getDistance(queuedAtomIndexes[0], atomIndex);
					viewer.scriptStatus("Distance " + viewer.getAtomInfo(queuedAtomIndexes[0]) + " - " + viewer.getAtomInfo(queuedAtomIndexes[1]) + " : " + distance);
					break;
				
				case JmolConstants.PICKING_ANGLE: 
					if (queuedAtomCount >= 3)
						queuedAtomCount = 0;
					queueAtom(atomIndex);
					if (queuedAtomCount < 3)
						break;
					float angle = frame.getAngle(queuedAtomIndexes[0], queuedAtomIndexes[1], atomIndex);
					viewer.scriptStatus("Angle " + viewer.getAtomInfo(queuedAtomIndexes[0]) + " - " + viewer.getAtomInfo(queuedAtomIndexes[1]) + " - " + viewer.getAtomInfo(queuedAtomIndexes[2]) + " : " + angle);
					break;
				
				case JmolConstants.PICKING_TORSION: 
					if (queuedAtomCount >= 4)
						queuedAtomCount = 0;
					queueAtom(atomIndex);
					if (queuedAtomCount < 4)
						break;
					float torsion = frame.getTorsion(queuedAtomIndexes[0], queuedAtomIndexes[1], queuedAtomIndexes[2], atomIndex);
					viewer.scriptStatus("Torsion " + viewer.getAtomInfo(queuedAtomIndexes[0]) + " - " + viewer.getAtomInfo(queuedAtomIndexes[1]) + " - " + viewer.getAtomInfo(queuedAtomIndexes[2]) + " - " + viewer.getAtomInfo(queuedAtomIndexes[3]) + " : " + torsion);
					break;
				
				case JmolConstants.PICKING_MONITOR: 
					if (queuedAtomCount >= 2)
						queuedAtomCount = 0;
					queueAtom(atomIndex);
					if (queuedAtomCount < 2)
						break;
					countPlusIndexes[0] = 2;
					countPlusIndexes[1] = queuedAtomIndexes[0];
					countPlusIndexes[2] = queuedAtomIndexes[1];
					viewer.toggleMeasurement(countPlusIndexes);
					break;
				
				case JmolConstants.PICKING_LABEL: 
					viewer.togglePickingLabel(atomIndex);
					break;
				
				case JmolConstants.PICKING_CENTER: 
					viewer.CenterPicked = atomIndex;
					break;
				
				case JmolConstants.PICKING_SELECT_ATOM: 
					if (shiftKey | chimeStylePicking)
						viewer.toggleSelection(atomIndex);
					else
						viewer.Selection = atomIndex;
					reportSelection();
					break;
				
				case JmolConstants.PICKING_SELECT_GROUP: 
					System.Collections.BitArray bsGroup = frame.getGroupBitSet(atomIndex);
					if (shiftKey | chimeStylePicking)
						viewer.toggleSelectionSet(bsGroup);
					else
						viewer.SelectionSet = bsGroup;
					viewer.clearClickCount();
					reportSelection();
					break;
				
				case JmolConstants.PICKING_SELECT_CHAIN: 
					System.Collections.BitArray bsChain = frame.getChainBitSet(atomIndex);
					if (shiftKey | chimeStylePicking)
						viewer.toggleSelectionSet(bsChain);
					else
						viewer.SelectionSet = bsChain;
					viewer.clearClickCount();
					reportSelection();
					break;
				}
		}
		
		internal virtual void  reportSelection()
		{
			viewer.scriptStatus("" + viewer.SelectionCount + " atoms selected");
		}
		
		internal virtual void  queueAtom(int atomIndex)
		{
			queuedAtomIndexes[queuedAtomCount++] = atomIndex;
			viewer.scriptStatus("Atom #" + queuedAtomCount + ":" + viewer.getAtomInfo(atomIndex));
		}
	}
}