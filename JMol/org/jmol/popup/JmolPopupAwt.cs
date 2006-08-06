/* $RCSfile$
* $Author: migueljmol $
* $Date: 2005-11-27 22:18:57 +0100 (dim., 27 nov. 2005) $
* $Revision: 4282 $
*
* Copyright (C) 2000-2005  The Jmol Development Team
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
namespace org.jmol.popup
{
	
	// mth 2003 05 27
	// This class is built with awt instead of swing so that it will
	// operate as an applet with old JVMs
	
	public class JmolPopupAwt:JmolPopup
	{
		
		internal System.Windows.Forms.ContextMenu awtPopup;
		internal CheckboxMenuItemListener cmil;
		internal System.Windows.Forms.MenuItem elementComputedMenu;
		
		public JmolPopupAwt(JmolViewer viewer):base(viewer)
		{
			awtPopup = new System.Windows.Forms.ContextMenu();
			mil = new MenuItemListener(this);
			cmil = new CheckboxMenuItemListener(this);
			jmolComponent.ContextMenu = awtPopup;
			build(awtPopup);
		}
		
		public override void  show(int x, int y)
		{
			//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
			for (System.Collections.IEnumerator keys = htCheckbox.Keys.GetEnumerator(); keys.MoveNext(); )
			{
				//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
				System.String key = (System.String) keys.Current;
				System.Windows.Forms.MenuItem cbmi = (System.Windows.Forms.MenuItem) htCheckbox[key];
				bool b = viewer.getBooleanProperty(key);
				cbmi.Checked = b;
			}
			//UPGRADE_TODO: Method 'java.awt.PopupMenu.show' was converted to 'System.Windows.Forms.ContextMenu.Show' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtPopupMenushow_javaawtComponent_int_int'"
			awtPopup.Show(jmolComponent, new System.Drawing.Point(x, y));
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'CheckboxMenuItemListener' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		internal class CheckboxMenuItemListener
		{
			public CheckboxMenuItemListener(JmolPopupAwt enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(JmolPopupAwt enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private JmolPopupAwt enclosingInstance;
			public JmolPopupAwt Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public virtual void  itemStateChanged(System.Object event_sender, System.EventArgs e)
			{
				if (event_sender is System.Windows.Forms.MenuItem)
					((System.Windows.Forms.MenuItem) event_sender).Checked = !((System.Windows.Forms.MenuItem) event_sender).Checked;
				System.Windows.Forms.MenuItem cmi = (System.Windows.Forms.MenuItem) event_sender;
				Enclosing_Instance.viewer.setBooleanProperty(SupportClass.CommandManager.GetCommand(cmi), cmi.Checked);
			}
		}
		
		/*
		void addMenuItems(String key, Menu menu) {
		String value = getValue(key);
		if (value == null) {
		MenuItem mi = new MenuItem("#" + key);
		menu.add(mi);
		return;
		}
		StringTokenizer st = new StringTokenizer(getValue(key));
		while (st.hasMoreTokens()) {
		String item = st.nextToken();
		if (item.endsWith("Menu")) {
		String word = getWord(item);
		Menu subMenu = new Menu(word);
		addMenuItems(item, subMenu);
		menu.add(subMenu);
		} else if (item.equals("-")) {
		menu.addSeparator();
		} else {
		String word = getWord(item);
		MenuItem mi;
		if (item.endsWith("Checkbox")) {
		CheckboxMenuItem cmi = new CheckboxMenuItem(word);
		String basename = item.substring(0, item.length() - 8);
		cmi.addItemListener(cmil);
		rememberCheckbox(basename, cmi);
		cmi.setActionCommand(basename);
		mi = cmi;
		} else {
		mi = new MenuItem(word);
		getValue(item);
		mi.addActionListener(mil);
		mi.setActionCommand(item);
		}
		menu.add(mi);
		}
		}
		}
		
		void addVersionAndDate() {
		addSeparator();
		MenuItem mi = new MenuItem("Jmol " + JmolConstants.version);
		add(mi);
		mi = new MenuItem(JmolConstants.date);
		add(mi);
		}
		*/
		
		internal virtual void  addToMenu(System.Object menu, System.Windows.Forms.MenuItem item)
		{
			((System.Windows.Forms.MenuItem) menu).MenuItems.Add(item);
		}
		
		////////////////////////////////////////////////////////////////
		
		internal override void  addMenuSeparator(System.Object menu)
		{
			((System.Windows.Forms.MenuItem) menu).MenuItems.Add(new System.Windows.Forms.MenuItem("-"));
		}
		
		internal override System.Object addMenuItem(System.Object menu, System.String entry, System.String script)
		{
			System.Windows.Forms.MenuItem mi = new System.Windows.Forms.MenuItem(entry);
			updateMenuItem(mi, entry, script);
			mi.Click += new System.EventHandler(mil.actionPerformed);
			SupportClass.CommandManager.CheckCommand(mi);
			addToMenu(menu, mi);
			return mi;
		}
		
		internal override void  updateMenuItem(System.Object menuItem, System.String entry, System.String script)
		{
			System.Windows.Forms.MenuItem mi = (System.Windows.Forms.MenuItem) menuItem;
			mi.Text = entry;
			SupportClass.CommandManager.SetCommand(mi, script);
			// miguel 2004 12 03
			// greyed out menu entries are too hard to read
			//    mi.setEnabled(script != null);
		}
		
		internal override void  addCheckboxMenuItem(System.Object menu, System.String entry, System.String basename)
		{
			System.Windows.Forms.MenuItem cmi = new System.Windows.Forms.MenuItem(entry);
			cmi.Click += new System.EventHandler(cmil.itemStateChanged);
			SupportClass.CommandManager.SetCommand(cmi, basename);
			addToMenu(menu, cmi);
			rememberCheckbox(basename, cmi);
		}
		
		internal override void  addMenuSubMenu(System.Object menu, System.Object subMenu)
		{
			addToMenu(menu, (System.Windows.Forms.MenuItem) subMenu);
		}
		
		internal override System.Object newMenu(System.String menuName)
		{
			return new System.Windows.Forms.MenuItem(menuName);
		}
		
		internal override void  renameMenu(System.Object menu, System.String newMenuName)
		{
			((System.Windows.Forms.MenuItem) menu).Text = newMenuName;
		}
		
		internal virtual System.Object newComputedMenu(System.String key, System.String word)
		{
			if ("elementComputedMenu".Equals(key))
			{
				elementComputedMenu = new System.Windows.Forms.MenuItem(word);
				return elementComputedMenu;
			}
			return new System.Windows.Forms.MenuItem("unrecognized ComputedMenu:" + key);
		}
		
		internal override void  removeAll(System.Object menu)
		{
			((System.Windows.Forms.MenuItem) menu).MenuItems.Clear();
		}
		
		internal override void  enableMenu(System.Object menu, bool enable)
		{
			((System.Windows.Forms.MenuItem) menu).Enabled = enable;
		}
	}
}