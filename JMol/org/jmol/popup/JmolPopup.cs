/* $RCSfile$
* $Author: migueljmol $
* $Date: 2005-12-19 01:43:55 +0100 $
* $Revision: 4377 $
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
using JmolConstants = org.jmol.viewer.JmolConstants;
namespace org.jmol.popup
{
	
	abstract public class JmolPopup
	{
		private const bool forceAwt = false;
		
		internal JmolViewer viewer;
		internal System.Windows.Forms.Control jmolComponent;
		internal MenuItemListener mil;
		
		internal System.Object elementsComputedMenu;
		internal System.Object aaresiduesComputedMenu;
		internal System.Object aboutMenu;
		internal System.Object consoleMenu;
		internal System.Object modelSetInfoMenu;
		internal System.String nullModelSetName;
		internal System.String hiddenModelSetName;
		
		internal JmolPopup(JmolViewer viewer)
		{
			this.viewer = viewer;
			jmolComponent = viewer.AwtComponent;
			mil = new MenuItemListener(this);
		}
		
		static public JmolPopup newJmolPopup(JmolViewer viewer)
		{
			if (!viewer.Jvm12orGreater || forceAwt)
				return new JmolPopupAwt(viewer);
			return new JmolPopupSwing(viewer);
		}
		
		
		internal virtual void  build(System.Object popupMenu)
		{
			addMenuItems("popupMenu", popupMenu, new PopupResourceBundle());
			addVersionAndDate(popupMenu);
			if (!viewer.Jvm12orGreater && (consoleMenu != null))
				enableMenu(consoleMenu, false);
		}
		
		public virtual void  updateComputedMenus()
		{
			updateElementsComputedMenu(viewer.ElementsPresentBitSet);
			updateAaresiduesComputedMenu(viewer.GroupsPresentBitSet);
			updateModelSetInfoMenu();
		}
		
		internal virtual void  updateElementsComputedMenu(System.Collections.BitArray elementsPresentBitSet)
		{
			if (elementsComputedMenu == null || elementsPresentBitSet == null)
				return ;
			removeAll(elementsComputedMenu);
			for (int i = 0; i < JmolConstants.elementNames.Length; ++i)
			{
				if (elementsPresentBitSet.Get(i))
				{
					System.String elementName = JmolConstants.elementNames[i];
					System.String elementSymbol = JmolConstants.elementSymbols[i];
					System.String entryName = elementSymbol + " - " + elementName;
					System.String script = "select " + elementName;
					addMenuItem(elementsComputedMenu, entryName, script);
				}
			}
		}
		
		internal virtual void  updateAaresiduesComputedMenu(System.Collections.BitArray groupsPresentBitSet)
		{
			if (aaresiduesComputedMenu == null || groupsPresentBitSet == null)
				return ;
			removeAll(aaresiduesComputedMenu);
			for (int i = 1; i < JmolConstants.GROUPID_AMINO_MAX; ++i)
			{
				if (groupsPresentBitSet.Get(i))
				{
					System.String aaresidueName = JmolConstants.predefinedGroup3Names[i];
					System.String script = "select " + aaresidueName;
					addMenuItem(aaresiduesComputedMenu, aaresidueName, script);
				}
			}
		}
		
		internal virtual void  updateModelSetInfoMenu()
		{
			if (modelSetInfoMenu == null)
				return ;
			removeAll(modelSetInfoMenu);
			renameMenu(modelSetInfoMenu, nullModelSetName);
			enableMenu(modelSetInfoMenu, false);
			System.String modelSetName = viewer.ModelSetName;
			if (modelSetName == null)
				return ;
			renameMenu(modelSetInfoMenu, viewer.getBooleanProperty("hideNameInPopup")?hiddenModelSetName:modelSetName);
			enableMenu(modelSetInfoMenu, true);
			addMenuItem(modelSetInfoMenu, "atoms:" + viewer.AtomCount);
			addMenuItem(modelSetInfoMenu, "bonds:" + viewer.BondCount);
			addMenuSeparator(modelSetInfoMenu);
			addMenuItem(modelSetInfoMenu, "groups:" + viewer.GroupCount);
			addMenuItem(modelSetInfoMenu, "chains:" + viewer.ChainCount);
			addMenuItem(modelSetInfoMenu, "polymers:" + viewer.PolymerCount);
			addMenuItem(modelSetInfoMenu, "models:" + viewer.ModelCount);
			if (viewer.showModelSetDownload() && !viewer.getBooleanProperty("hideNameInPopup"))
			{
				addMenuSeparator(modelSetInfoMenu);
				addMenuItem(modelSetInfoMenu, viewer.ModelSetFileName, viewer.ModelSetPathName);
			}
		}
		
		private void  addVersionAndDate(System.Object popupMenu)
		{
			if (aboutMenu != null)
			{
				addMenuSeparator(aboutMenu);
				addMenuItem(aboutMenu, "Jmol " + JmolConstants.version);
				addMenuItem(aboutMenu, JmolConstants.date);
				addMenuItem(aboutMenu, viewer.OperatingSystemName);
				addMenuItem(aboutMenu, viewer.JavaVendor);
				addMenuItem(aboutMenu, viewer.JavaVersion);
			}
		}
		
		private void  addMenuItems(System.String key, System.Object menu, PopupResourceBundle popupResourceBundle)
		{
			System.String value_Renamed = popupResourceBundle.getStructure(key);
			if (value_Renamed == null)
			{
				addMenuItem(menu, "#" + key);
				return ;
			}
			SupportClass.Tokenizer st = new SupportClass.Tokenizer(value_Renamed);
			while (st.HasMoreTokens())
			{
				System.String item = st.NextToken();
				System.String word = popupResourceBundle.getWord(item);
				if (item.EndsWith("Menu"))
				{
					System.Object subMenu = newMenu(word);
					if ("elementsComputedMenu".Equals(item))
						elementsComputedMenu = subMenu;
					else if ("aaresiduesComputedMenu".Equals(item))
						aaresiduesComputedMenu = subMenu;
					else
						addMenuItems(item, subMenu, popupResourceBundle);
					if ("aboutMenu".Equals(item))
						aboutMenu = subMenu;
					else if ("consoleMenu".Equals(item))
						consoleMenu = subMenu;
					else if ("modelSetInfoMenu".Equals(item))
					{
						nullModelSetName = word;
						hiddenModelSetName = popupResourceBundle.getWord("hiddenModelSetName");
						modelSetInfoMenu = subMenu;
						enableMenu(modelSetInfoMenu, false);
					}
					addMenuSubMenu(menu, subMenu);
				}
				else if ("-".Equals(item))
				{
					addMenuSeparator(menu);
				}
				else if (item.EndsWith("Checkbox"))
				{
					System.String basename = item.Substring(0, (item.Length - 8) - (0));
					addCheckboxMenuItem(menu, word, basename);
				}
				else
				{
					addMenuItem(menu, word, popupResourceBundle.getStructure(item));
				}
			}
		}
		
		internal System.Collections.Hashtable htCheckbox = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
		
		internal virtual void  rememberCheckbox(System.String key, System.Object checkboxMenuItem)
		{
			htCheckbox[key] = checkboxMenuItem;
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'MenuItemListener' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		internal class MenuItemListener
		{
			public MenuItemListener(JmolPopup enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(JmolPopup enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private JmolPopup enclosingInstance;
			public JmolPopup Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public virtual void  actionPerformed(System.Object event_sender, System.EventArgs e)
			{
				System.String script = SupportClass.CommandManager.GetCommand(event_sender);
				if (script == null || script.Length == 0)
					return ;
				if (script.StartsWith("http:") || script.StartsWith("file:") || script.StartsWith("/"))
				{
					Enclosing_Instance.viewer.showUrl(script);
					return ;
				}
				Enclosing_Instance.viewer.evalStringQuiet(script);
			}
		}
		
		internal virtual System.Object addMenuItem(System.Object menuItem, System.String entry)
		{
			return addMenuItem(menuItem, entry, null);
		}
		
		////////////////////////////////////////////////////////////////
		
		abstract public void  show(int x, int y);
		
		internal abstract void  addMenuSeparator(System.Object menu);
		
		internal abstract System.Object addMenuItem(System.Object menu, System.String entry, System.String script);
		
		internal abstract void  updateMenuItem(System.Object menuItem, System.String entry, System.String script);
		
		internal abstract void  addCheckboxMenuItem(System.Object menu, System.String entry, System.String basename);
		
		internal abstract void  addMenuSubMenu(System.Object menu, System.Object subMenu);
		
		internal abstract System.Object newMenu(System.String menuName);
		
		internal abstract void  enableMenu(System.Object menu, bool enable);
		
		internal abstract void  renameMenu(System.Object menu, System.String menuName);
		
		internal abstract void  removeAll(System.Object menu);
	}
}