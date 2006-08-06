/* $RCSfile$
* $Author: migueljmol $
* $Date: 2005-11-27 22:23:02 +0100 (dim., 27 nov. 2005) $
* $Revision: 4284 $
*
* Copyright (C) 2005  Miguel, Jmol Development, www.jmol.org
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
namespace org.jmol.util
{
	
	public class Int2IntHash
	{
		internal int entryCount;
		internal Entry[] entries;
		
		
		public Int2IntHash(int initialCapacity)
		{
			entries = new Entry[initialCapacity];
		}
		
		public Int2IntHash():this(256)
		{
		}
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'get'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public virtual int get_Renamed(int key)
		{
			lock (this)
			{
				Entry[] entries = this.entries;
				int hash = (key & 0x7FFFFFFF) % entries.Length;
				for (Entry e = entries[hash]; e != null; e = e.next)
					if (e.key == key)
						return e.value_Renamed;
				return System.Int32.MinValue;
			}
		}
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'put'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public virtual void  put(int key, int value_Renamed)
		{
			lock (this)
			{
				Entry[] entries = this.entries;
				int hash = (key & 0x7FFFFFFF) % entries.Length;
				for (Entry e = entries[hash]; e != null; e = e.next)
					if (e.key == key)
					{
						e.value_Renamed = value_Renamed;
						return ;
					}
				if (entryCount > entries.Length)
					rehash();
				entries = this.entries;
				hash = (key & 0x7FFFFFFF) % entries.Length;
				entries[hash] = new Entry(key, value_Renamed, entries[hash]);
				++entryCount;
			}
		}
		
		private void  rehash()
		{
			Entry[] oldEntries = entries;
			int oldSize = oldEntries.Length;
			int newSize = oldSize * 2 + 1;
			Entry[] newEntries = new Entry[newSize];
			
			for (int i = oldSize; --i >= 0; )
			{
				for (Entry e = oldEntries[i]; e != null; )
				{
					Entry t = e;
					e = e.next;
					
					int hash = (t.key & 0x7FFFFFFF) % newSize;
					t.next = newEntries[hash];
					newEntries[hash] = t;
				}
			}
			entries = newEntries;
		}
		
		internal class Entry
		{
			internal int key;
			internal int value_Renamed;
			internal Entry next;
			
			internal Entry(int key, int value_Renamed, Entry next)
			{
				this.key = key;
				this.value_Renamed = value_Renamed;
				this.next = next;
			}
		}
	}
}