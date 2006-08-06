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
	
	public class JmolPopupSwing:JmolPopup
	{
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'javax.swing.JPopupMenu' and 'System.Windows.Forms.ContextMenu' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		internal System.Windows.Forms.ContextMenu swingPopup;
		internal CheckboxMenuItemListener cmil;
		internal System.Windows.Forms.MenuItem elementComputedMenu;
		
		public JmolPopupSwing(JmolViewer viewer):base(viewer)
		{
			//UPGRADE_ISSUE: Class hierarchy differences between 'javax.swing.JPopupMenu' and 'System.Windows.Forms.ContextMenu' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			swingPopup = new System.Windows.Forms.ContextMenu();
			cmil = new CheckboxMenuItemListener(this);
			build(swingPopup);
		}
		
		public override void  show(int x, int y)
		{
			//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
			for (System.Collections.IEnumerator keys = htCheckbox.Keys.GetEnumerator(); keys.MoveNext(); )
			{
				//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
				System.String key = (System.String) keys.Current;
				System.Windows.Forms.MenuItem jcbmi = (System.Windows.Forms.MenuItem) htCheckbox[key];
				bool b = viewer.getBooleanProperty(key);
				//System.out.println("found:" + key + " & it is:" + b);
				jcbmi.Checked = b;
			}
			//UPGRADE_TODO: Method 'javax.swing.JPopupMenu.show' was converted to 'System.Windows.Forms.ContextMenu.Show' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJPopupMenushow_javaawtComponent_int_int'"
			swingPopup.Show(jmolComponent, new System.Drawing.Point(x, y));
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'CheckboxMenuItemListener' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		internal class CheckboxMenuItemListener
		{
			public CheckboxMenuItemListener(JmolPopupSwing enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(JmolPopupSwing enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private JmolPopupSwing enclosingInstance;
			public JmolPopupSwing Enclosing_Instance
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
				//System.out.println("CheckboxMenuItemListener() " + e.getSource());
				System.Windows.Forms.MenuItem jcmi = (System.Windows.Forms.MenuItem) event_sender;
				Enclosing_Instance.viewer.setBooleanProperty(SupportClass.CommandManager.GetCommand(jcmi), jcmi.Checked);
			}
		}
		
		internal virtual void  addToMenu(System.Object menu, System.Windows.Forms.Control item)
		{
			//UPGRADE_ISSUE: Class hierarchy differences between 'javax.swing.JPopupMenu' and 'System.Windows.Forms.ContextMenu' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			if (menu is System.Windows.Forms.ContextMenu)
			{
				//UPGRADE_ISSUE: Class hierarchy differences between 'javax.swing.JPopupMenu' and 'System.Windows.Forms.ContextMenu' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
				//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
				((System.Windows.Forms.ContextMenu) menu).Controls.Add(item);
			}
			else if (menu is System.Windows.Forms.MenuItem)
			{
				//UPGRADE_ISSUE: Method 'javax.swing.JMenu.add' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJMenuadd_javaawtComponent'"
				((System.Windows.Forms.MenuItem) menu).add(item);
			}
			else
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				System.Console.Out.WriteLine("cannot add object to menu:" + menu);
			}
		}
		
		////////////////////////////////////////////////////////////////
		
		internal override void  addMenuSeparator(System.Object menu)
		{
			//UPGRADE_ISSUE: Class hierarchy differences between 'javax.swing.JPopupMenu' and 'System.Windows.Forms.ContextMenu' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			if (menu is System.Windows.Forms.ContextMenu)
			{
				//UPGRADE_ISSUE: Class hierarchy differences between 'javax.swing.JPopupMenu' and 'System.Windows.Forms.ContextMenu' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
				((System.Windows.Forms.ContextMenu) menu).MenuItems.Add("-");
			}
			else
				((System.Windows.Forms.MenuItem) menu).MenuItems.Add(new System.Windows.Forms.MenuItem("-"));
		}
		
		internal override System.Object addMenuItem(System.Object menu, System.String entry, System.String script)
		{
			System.Windows.Forms.MenuItem jmi = new System.Windows.Forms.MenuItem(entry);
			updateMenuItem(jmi, entry, script);
			jmi.Click += new System.EventHandler(mil.actionPerformed);
			SupportClass.CommandManager.CheckCommand(jmi);
			addToMenu(menu, jmi);
			return jmi;
		}
		
		internal override void  updateMenuItem(System.Object menuItem, System.String entry, System.String script)
		{
			System.Windows.Forms.MenuItem jmi = (System.Windows.Forms.MenuItem) menuItem;
			jmi.Text = entry;
			SupportClass.CommandManager.SetCommand(jmi, script);
			// miguel 2004 12 03
			// greyed out menu entries are too hard to read
			//    jmi.setEnabled(script != null);
		}
		
		internal override void  addCheckboxMenuItem(System.Object menu, System.String entry, System.String basename)
		{
			System.Windows.Forms.MenuItem jcmi = new System.Windows.Forms.MenuItem(entry);
			jcmi.Click += new System.EventHandler(cmil.itemStateChanged);
			SupportClass.CommandManager.SetCommand(jcmi, basename);
			addToMenu(menu, jcmi);
			rememberCheckbox(basename, jcmi);
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
				elementComputedMenu = new System.Windows.Forms.MenuItem();
				elementComputedMenu.Text = word;
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