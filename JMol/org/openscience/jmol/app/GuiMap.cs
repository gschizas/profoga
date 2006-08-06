/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-03-28 14:30:10 +0200 (mar., 28 mars 2006) $
* $Revision: 4828 $
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
using GT = org.jmol.i18n.GT;
namespace org.openscience.jmol.app
{
	
	//UPGRADE_NOTE: The access modifier for this class or class field has been changed in order to prevent compilation errors due to the visibility level. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1296'"
	public class GuiMap
	{
		
		internal System.Collections.Hashtable map = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
		
		internal System.Collections.Hashtable labels = null;
		
		private System.Collections.Hashtable setupLabels()
		{
			System.Collections.Hashtable labels = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
			labels["file"] = GT._("&File");
			labels["newwin"] = GT._("New");
			labels["open"] = GT._("&Open");
			labels["openurl"] = GT._("Open &URL");
			labels["script"] = GT._("Scrip&t...");
			labels["atomsetchooser"] = GT._("AtomSetChooser...");
			labels["saveas"] = GT._("&Save As...");
			labels["exportMenu"] = GT._("&Export");
			labels["export"] = GT._("Export Image...");
			labels["povray"] = GT._("Render in pov-ray...");
			labels["pdf"] = GT._("Export PDF...");
			labels["print"] = GT._("&Print...");
			labels["close"] = GT._("Close");
			labels["exit"] = GT._("E&xit");
			labels["recentFiles"] = GT._("Recent Files...");
			labels["edit"] = GT._("&Edit");
			labels["makecrystal"] = GT._("Make crystal...");
			labels["selectall"] = GT._("Select All");
			labels["deselectall"] = GT._("Deselect All");
			labels["copyImage"] = GT._("Copy Image");
			labels["prefs"] = GT._("&Preferences...");
			labels["editSelectAllScript"] = GT._("Select All");
			labels["selectMenu"] = GT._("Select");
			labels["selectAllScript"] = GT._("All");
			labels["selectNoneScript"] = GT._("None");
			labels["selectHydrogenScript"] = GT._("Hydrogen");
			labels["selectCarbonScript"] = GT._("Carbon");
			labels["selectNitrogenScript"] = GT._("Nitrogen");
			labels["selectOxygenScript"] = GT._("Oxygen");
			labels["selectPhosphorusScript"] = GT._("Phosphorus");
			labels["selectSulphurScript"] = GT._("Sulphur");
			labels["selectAminoScript"] = GT._("Amino");
			labels["selectNucleicScript"] = GT._("Nucleic");
			labels["selectWaterScript"] = GT._("Water");
			labels["selectHeteroScript"] = GT._("Hetero");
			labels["display"] = GT._("&Display");
			labels["atomMenu"] = GT._("Atom");
			labels["atomNoneScript"] = GT._("None");
			labels["atom15Script"] = GT._("{0}% vanderWaals", new System.Object[]{"15"});
			labels["atom20Script"] = GT._("{0}% vanderWaals", new System.Object[]{"20"});
			labels["atom25Script"] = GT._("{0}% vanderWaals", new System.Object[]{"25"});
			labels["atom100Script"] = GT._("{0}% vanderWaals", new System.Object[]{"100"});
			labels["bondMenu"] = GT._("Bond");
			labels["bondNoneScript"] = GT._("None");
			labels["bondWireframeScript"] = GT._("Wireframe");
			labels["bond100Script"] = GT._("{0} \u00C5", new System.Object[]{"0.10"});
			labels["bond150Script"] = GT._("{0} \u00C5", new System.Object[]{"0.15"});
			labels["bond200Script"] = GT._("{0} \u00C5", new System.Object[]{"0.20"});
			labels["labelMenu"] = GT._("Label");
			labels["labelNoneScript"] = GT._("None");
			labels["labelSymbolScript"] = GT._("Symbol");
			labels["labelNameScript"] = GT._("Name");
			labels["labelNumberScript"] = GT._("Number");
			labels["labelCenteredScript"] = GT._("Centered");
			labels["labelUpperRightScript"] = GT._("Upper right");
			labels["vectorMenu"] = GT._("Vector");
			labels["vectorOffScript"] = GT._("None");
			labels["vectorOnScript"] = GT._("On");
			labels["vector3Script"] = GT._("{0} pixels", new System.Object[]{"3"});
			labels["vector005Script"] = GT._("{0} \u00C5", new System.Object[]{"0.05"});
			labels["vector01Script"] = GT._("{0} \u00C5", new System.Object[]{"0.1"});
			labels["vectorScale02Script"] = GT._("Scale {0}", new System.Object[]{"0.2"});
			labels["vectorScale05Script"] = GT._("Scale {0}", new System.Object[]{"0.5"});
			labels["vectorScale1Script"] = GT._("Scale {0}", new System.Object[]{"1"});
			labels["vectorScale2Script"] = GT._("Scale {0}", new System.Object[]{"2"});
			labels["vectorScale5Script"] = GT._("Scale {0}", new System.Object[]{"5"});
			labels["zoomMenu"] = GT._("Zoom");
			labels["zoom100Script"] = GT._("{0}%", new System.Object[]{"100"});
			labels["zoom150Script"] = GT._("{0}%", new System.Object[]{"150"});
			labels["zoom200Script"] = GT._("{0}%", new System.Object[]{"200"});
			labels["zoom400Script"] = GT._("{0}%", new System.Object[]{"400"});
			labels["zoom800Script"] = GT._("{0}%", new System.Object[]{"800"});
			labels["perspectiveCheck"] = GT._("Perspective Depth");
			labels["axesCheck"] = GT._("Axes");
			labels["boundboxCheck"] = GT._("Bounding Box");
			labels["hydrogensCheck"] = GT._("&Hydrogens");
			labels["vectorsCheck"] = GT._("&Vectors");
			labels["measurementsCheck"] = GT._("&Measurements");
			labels["view"] = GT._("&View");
			labels["front"] = GT._("Front");
			labels["top"] = GT._("Top");
			labels["bottom"] = GT._("Bottom");
			labels["right"] = GT._("Right");
			labels["left"] = GT._("Left");
			labels["transform"] = GT._("Transform...");
			labels["definecenter"] = GT._("Define Center");
			labels["tools"] = GT._("&Tools");
			labels["viewMeasurementTable"] = GT._("Measurements...");
			labels["distanceUnitsMenu"] = GT._("Distance Units");
			labels["distanceNanometersScript"] = GT._("Nanometers 1E-9");
			labels["distanceAngstromsScript"] = GT._("Angstroms 1E-10");
			labels["distancePicometersScript"] = GT._("Picometers 1E-12");
			labels["animateMenu"] = GT._("Animate...");
			labels["vibrateMenu"] = GT._("Vibrate...");
			labels["graph"] = GT._("&Graph...");
			labels["chemicalShifts"] = GT._("Calculate chemical &shifts...");
			labels["crystprop"] = GT._("&Crystal Properties");
			labels["animateOnceScript"] = GT._("Once");
			labels["animateLoopScript"] = GT._("Loop");
			labels["animatePalindromeScript"] = GT._("Palindrome");
			labels["animateStopScript"] = GT._("Stop animation");
			labels["animateRewindScript"] = GT._("Rewind to first frame");
			labels["animateNextScript"] = GT._("Go to next frame");
			labels["animatePrevScript"] = GT._("Go to previous frame");
			labels["vibrateStartScript"] = GT._("Start vibration");
			labels["vibrateStopScript"] = GT._("Stop vibration");
			labels["vibrateRewindScript"] = GT._("First frequency");
			labels["vibrateNextScript"] = GT._("Next frequency");
			labels["vibratePrevScript"] = GT._("Previous frequency");
			labels["help"] = GT._("&Help");
			labels["about"] = GT._("About Jmol");
			labels["uguide"] = GT._("User Guide");
			labels["whatsnew"] = GT._("What's New");
			labels["console"] = GT._("Jmol Console");
			labels["Prefs.showHydrogens"] = GT._("Hydrogens");
			labels["Prefs.showMeasurements"] = GT._("Measurements");
			labels["Prefs.perspectiveDepth"] = GT._("Perspective Depth");
			labels["Prefs.showAxes"] = GT._("Axes");
			labels["Prefs.showBoundingBox"] = GT._("Bounding Box");
			labels["Prefs.axesOrientationRasmol"] = GT._("RasMol/Chime compatible axes orientation/rotations");
			labels["Prefs.openFilePreview"] = GT._("File Preview (needs restarting Jmol)");
			labels["Prefs.isLabelAtomColor"] = GT._("Use Atom Color");
			labels["Prefs.isBondAtomColor"] = GT._("Use Atom Color");
			labels["openTip"] = GT._("Open a file.");
			labels["exportTip"] = GT._("Export view to an image file.");
			labels["povrayTip"] = GT._("Render in pov-ray.");
			labels["printTip"] = GT._("Print view.");
			labels["rotateTip"] = GT._("Rotate molecule.");
			labels["pickTip"] = GT._("Select an atom or region.");
			labels["viewMeasurementTableTip"] = GT._("View measurement table.");
			labels["homeTip"] = GT._("Return molecule to home position.");
			labels["animateRewindScriptTip"] = GT._("Rewind to first frame");
			labels["animateNextScriptTip"] = GT._("Go to next frame");
			labels["animatePrevScriptTip"] = GT._("Go to previous frame");
			
			return labels;
		}
		
		internal virtual System.String getLabel(System.String key)
		{
			if (labels == null)
			{
				labels = setupLabels();
			}
			System.String label = (System.String) labels[key];
			if (label == null)
			// Use the previous system as backup
				if (label == null)
				{
					System.Console.Out.WriteLine("Missing i18n menu resource, trying old scheme for: " + key);
					JmolResourceHandler.getStringX(key + "Label");
				}
			return label;
		}
		
		internal virtual System.Windows.Forms.MenuItem newJMenu(System.String key)
		{
			System.String label = getLabel(key);
			return new KeyJMenu(this, key, getLabelWithoutMnemonic(label), getMnemonic(label));
		}
		
		internal virtual System.Windows.Forms.MenuItem newJMenuItem(System.String key)
		{
			System.String label = getLabel(key);
			return new KeyJMenuItem(this, key, getLabelWithoutMnemonic(label), getMnemonic(label));
		}
		internal virtual System.Windows.Forms.MenuItem newJCheckBoxMenuItem(System.String key, bool isChecked)
		{
			System.String label = getLabel(key);
			return new KeyJCheckBoxMenuItem(this, key, getLabelWithoutMnemonic(label), getMnemonic(label), isChecked);
		}
		internal virtual System.Windows.Forms.MenuItem newJRadioButtonMenuItem(System.String key)
		{
			System.String label = getLabel(key);
			return new KeyJRadioButtonMenuItem(this, key, getLabelWithoutMnemonic(label), getMnemonic(label));
		}
		internal virtual System.Windows.Forms.CheckBox newJCheckBox(System.String key, bool isChecked)
		{
			System.String label = getLabel(key);
			return new KeyJCheckBox(this, key, getLabelWithoutMnemonic(label), getMnemonic(label), isChecked);
		}
		
		internal virtual System.Object get_Renamed(System.String key)
		{
			return map[key];
		}
		
		internal virtual System.String getKey(System.Object obj)
		{
			return (((GuiMap.GetKey) obj).Key);
		}
		
		public virtual System.String getLabelWithoutMnemonic(System.String label)
		{
			if (label == null)
			{
				return null;
			}
			int index = label.IndexOf('&');
			if (index == - 1)
			{
				return label;
			}
			return label.Substring(0, (index) - (0)) + ((index < label.Length - 1)?label.Substring(index + 1):"");
		}
		
		public virtual char getMnemonic(System.String label)
		{
			if (label == null)
			{
				return ' ';
			}
			int index = label.IndexOf('&');
			if ((index == - 1) || (index == label.Length - 1))
			{
				return ' ';
			}
			return label[index + 1];
		}
		
		internal virtual void  setSelected(System.String key, bool b)
		{
			//UPGRADE_ISSUE: Method 'javax.swing.AbstractButton.setSelected' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractButtonsetSelected_boolean'"
			((System.Windows.Forms.ButtonBase) get_Renamed(key)).setSelected(b);
		}
		
		internal virtual bool isSelected(System.String key)
		{
			System.Windows.Forms.ButtonBase generatedAux = ((System.Windows.Forms.ButtonBase) get_Renamed(key));
			return false;
		}
		
		
		internal interface GetKey
		{
			System.String Key
			{
				get;
				
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'KeyJMenu' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class KeyJMenu:System.Windows.Forms.MenuItem, GuiMap.GetKey
		{
			private void  InitBlock(GuiMap enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private GuiMap enclosingInstance;
			virtual public System.String Key
			{
				get
				{
					return key;
				}
				
			}
			public GuiMap Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			internal System.String key;
			internal KeyJMenu(GuiMap enclosingInstance, System.String key, System.String label, char mnemonic):base(label)
			{
				InitBlock(enclosingInstance);
				if (mnemonic != ' ')
				{
					System.Int32 tempInt;
					tempInt = Text.ToLower().IndexOf(char.ToLower(mnemonic));
					Text = tempInt >= 0?Text.Insert(tempInt, "&"):Text;
				}
				this.key = key;
				Enclosing_Instance.map[key] = this;
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'KeyJMenuItem' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class KeyJMenuItem:System.Windows.Forms.MenuItem, GuiMap.GetKey
		{
			private void  InitBlock(GuiMap enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private GuiMap enclosingInstance;
			virtual public System.String Key
			{
				get
				{
					return key;
				}
				
			}
			public GuiMap Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			internal System.String key;
			internal KeyJMenuItem(GuiMap enclosingInstance, System.String key, System.String label, char mnemonic):base(label)
			{
				InitBlock(enclosingInstance);
				if (mnemonic != ' ')
				{
					System.Int32 tempInt;
					tempInt = Text.ToLower().IndexOf(char.ToLower(mnemonic));
					Text = tempInt >= 0?Text.Insert(tempInt, "&"):Text;
				}
				this.key = key;
				Enclosing_Instance.map[key] = this;
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'KeyJCheckBoxMenuItem' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class KeyJCheckBoxMenuItem:System.Windows.Forms.MenuItem, GuiMap.GetKey
		{
			private void  InitBlock(GuiMap enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private GuiMap enclosingInstance;
			virtual public System.String Key
			{
				get
				{
					return key;
				}
				
			}
			public GuiMap Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			internal System.String key;
			internal KeyJCheckBoxMenuItem(GuiMap enclosingInstance, System.String key, System.String label, char mnemonic, bool isChecked):base()
			{
				InitBlock(enclosingInstance);
				SupportClass.CheckBoxMenuItemSupport.SetCheckBoxMenu(this, label, isChecked);
				if (mnemonic != ' ')
				{
					System.Int32 tempInt;
					tempInt = Text.ToLower().IndexOf(char.ToLower(mnemonic));
					Text = tempInt >= 0?Text.Insert(tempInt, "&"):Text;
				}
				this.key = key;
				Enclosing_Instance.map[key] = this;
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'KeyJRadioButtonMenuItem' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class KeyJRadioButtonMenuItem:System.Windows.Forms.MenuItem, GuiMap.GetKey
		{
			private void  InitBlock(GuiMap enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private GuiMap enclosingInstance;
			virtual public System.String Key
			{
				get
				{
					return key;
				}
				
			}
			public GuiMap Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			internal System.String key;
			internal KeyJRadioButtonMenuItem(GuiMap enclosingInstance, System.String key, System.String label, char mnemonic):base()
			{
				InitBlock(enclosingInstance);
				//UPGRADE_TODO: Constructor 'javax.swing.JRadioButtonMenuItem.JRadioButtonMenuItem' was converted to 'System.Windows.Forms.MenuItem' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJRadioButtonMenuItemJRadioButtonMenuItem_javalangString'"
				this.RadioCheck = true;
				this.Text = label;
				if (mnemonic != ' ')
				{
					System.Int32 tempInt;
					tempInt = Text.ToLower().IndexOf(char.ToLower(mnemonic));
					Text = tempInt >= 0?Text.Insert(tempInt, "&"):Text;
				}
				this.key = key;
				Enclosing_Instance.map[key] = this;
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'KeyJCheckBox' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		[Serializable]
		internal class KeyJCheckBox:System.Windows.Forms.CheckBox, GuiMap.GetKey
		{
			private void  InitBlock(GuiMap enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private GuiMap enclosingInstance;
			virtual public System.String Key
			{
				get
				{
					return key;
				}
				
			}
			public GuiMap Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			internal System.String key;
			internal KeyJCheckBox(GuiMap enclosingInstance, System.String key, System.String label, char mnemonic, bool isChecked):base()
			{
				InitBlock(enclosingInstance);
				SupportClass.CheckBoxSupport.SetCheckBox(this, label, isChecked);
				if (mnemonic != ' ')
				{
					System.Int32 tempInt;
					tempInt = Text.ToLower().IndexOf(char.ToLower(mnemonic));
					Text = tempInt >= 0?Text.Insert(tempInt, "&"):Text;
				}
				this.key = key;
				Enclosing_Instance.map[key] = this;
			}
		}
	}
}