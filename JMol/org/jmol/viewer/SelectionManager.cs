/* $RCSfile$
* $Author: egonw $
* $Date: 2005-11-10 16:52:44 +0100 (jeu., 10 nov. 2005) $
* $Revision: 4255 $
*
* Copyright (C) 2003-2005  Miguel, Jmol Development
*
* Contact: miguel@jmol.org, jmol-developers@lists.sf.net
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
using JmolSelectionListener = org.jmol.api.JmolSelectionListener;
namespace org.jmol.viewer
{
	
	class SelectionManager
	{
		virtual internal bool Empty
		{
			get
			{
				if (empty != UNKNOWN)
					return empty == TRUE;
				for (int i = viewer.AtomCount; --i >= 0; )
					if (bsSelection.Get(i))
					{
						empty = FALSE;
						return false;
					}
				empty = TRUE;
				return true;
			}
			
		}
		virtual internal int Selection
		{
			set
			{
				//UPGRADE_NOTE: In .NET BitArrays must be of the same size to allow the 'System.Collections.BitArray.And' operation. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1083'"
				bsSelection.And(bsNull);
				SupportClass.BitArraySupport.Set(bsSelection, value);
				empty = FALSE;
				selectionChanged();
			}
			
		}
		virtual internal System.Collections.BitArray SelectionSet
		{
			set
			{
				//UPGRADE_NOTE: In .NET BitArrays must be of the same size to allow the 'System.Collections.BitArray.And' operation. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1083'"
				bsSelection.And(bsNull);
				//UPGRADE_NOTE: In .NET BitArrays must be of the same size to allow the 'System.Collections.BitArray.Or' operation. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1083'"
				bsSelection.Or(value);
				empty = UNKNOWN;
				selectionChanged();
			}
			
		}
		virtual internal int SelectionCount
		{
			get
			{
				// FIXME mth 2003 11 16
				// very inefficient ... but works for now
				// need to implement our own bitset that keeps track of the count
				// maybe one that takes 'model' into account as well
				if (empty == TRUE)
					return 0;
				int count = 0;
				empty = TRUE;
				for (int i = viewer.AtomCount; --i >= 0; )
					if (bsSelection.Get(i))
						++count;
				if (count > 0)
					empty = FALSE;
				return count;
			}
			
		}
		
		internal Viewer viewer;
		
		internal JmolSelectionListener[] listeners = new JmolSelectionListener[4];
		
		internal SelectionManager(Viewer viewer)
		{
			this.viewer = viewer;
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'bsNull '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private System.Collections.BitArray bsNull = new System.Collections.BitArray(64);
		//UPGRADE_NOTE: Final was removed from the declaration of 'bsSelection '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal System.Collections.BitArray bsSelection = new System.Collections.BitArray(64);
		// this is a tri-state. the value -1 means unknown
		internal const int TRUE = 1;
		internal const int FALSE = 0;
		internal const int UNKNOWN = - 1;
		internal int empty = TRUE;
		
		
		internal virtual void  addSelection(int atomIndex)
		{
			if (!bsSelection.Get(atomIndex))
			{
				SupportClass.BitArraySupport.Set(bsSelection, atomIndex);
				empty = FALSE;
				selectionChanged();
			}
		}
		
		internal virtual void  addSelection(System.Collections.BitArray set_Renamed)
		{
			//UPGRADE_NOTE: In .NET BitArrays must be of the same size to allow the 'System.Collections.BitArray.Or' operation. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1083'"
			bsSelection.Or(set_Renamed);
			if (empty == TRUE)
				empty = UNKNOWN;
			selectionChanged();
		}
		
		internal virtual void  toggleSelection(int atomIndex)
		{
			if (bsSelection.Get(atomIndex))
				bsSelection.Set(atomIndex, false);
			else
				SupportClass.BitArraySupport.Set(bsSelection, atomIndex);
			empty = (empty == TRUE)?FALSE:UNKNOWN;
			selectionChanged();
		}
		
		internal virtual bool isSelected(int atomIndex)
		{
			return bsSelection.Get(atomIndex);
		}
		
		internal virtual void  selectAll()
		{
			int count = viewer.AtomCount;
			empty = (count == 0)?TRUE:FALSE;
			for (int i = count; --i >= 0; )
				SupportClass.BitArraySupport.Set(bsSelection, i);
			selectionChanged();
		}
		
		internal virtual void  clearSelection()
		{
			//UPGRADE_NOTE: In .NET BitArrays must be of the same size to allow the 'System.Collections.BitArray.And' operation. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1083'"
			bsSelection.And(bsNull);
			empty = TRUE;
			selectionChanged();
		}
		
		internal virtual void  toggleSelectionSet(System.Collections.BitArray bs)
		{
			/*
			toggle each one independently
			for (int i = viewer.getAtomCount(); --i >= 0; )
			if (bs.get(i))
			toggleSelection(i);
			*/
			int atomCount = viewer.AtomCount;
			int i = atomCount;
			while (--i >= 0)
				if (bs.Get(i) && !bsSelection.Get(i))
					break;
			if (i < 0)
			{
				// all were selected
				for (i = atomCount; --i >= 0; )
					if (bs.Get(i))
						bsSelection.Set(i, false);
				empty = UNKNOWN;
			}
			else
			{
				// at least one was not selected
				do 
				{
					if (bs.Get(i))
					{
						SupportClass.BitArraySupport.Set(bsSelection, i);
						empty = FALSE;
					}
				}
				while (--i >= 0);
			}
			selectionChanged();
		}
		
		internal virtual void  invertSelection()
		{
			empty = TRUE;
			for (int i = viewer.AtomCount; --i >= 0; )
				if (bsSelection.Get(i))
				{
					bsSelection.Set(i, false);
				}
				else
				{
					SupportClass.BitArraySupport.Set(bsSelection, i);
					empty = FALSE;
				}
			selectionChanged();
		}
		
		internal virtual void  excludeSelectionSet(System.Collections.BitArray setExclude)
		{
			if (empty == TRUE)
				return ;
			for (int i = viewer.AtomCount; --i >= 0; )
				if (setExclude.Get(i))
					bsSelection.Set(i, false);
			empty = UNKNOWN;
			selectionChanged();
		}
		
		internal virtual void  addListener(JmolSelectionListener listener)
		{
			removeListener(listener);
			int len = listeners.Length;
			for (int i = len; --i >= 0; )
			{
				if (listeners[i] == null)
				{
					listeners[i] = listener;
					return ;
				}
			}
			listeners = (JmolSelectionListener[]) Util.doubleLength(listeners);
			listeners[len] = listener;
		}
		
		internal virtual void  removeListener(JmolSelectionListener listener)
		{
			for (int i = listeners.Length; --i >= 0; )
				if (listeners[i] == listener)
				{
					listeners[i] = null;
					return ;
				}
		}
		
		private void  selectionChanged()
		{
			for (int i = listeners.Length; --i >= 0; )
			{
				JmolSelectionListener listener = listeners[i];
				if (listener != null)
					listeners[i].selectionChanged(bsSelection);
			}
		}
	}
}