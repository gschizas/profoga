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
*  Lesser General Public License for more details.
*
*  You should have received a copy of the GNU Lesser General Public
*  License along with this library; if not, write to the Free Software
*  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*/
using System;
namespace org.jmol.viewer
{
	
	class StyleManager
	{
		private void  InitBlock()
		{
			modeMultipleBond = JmolConstants.MULTIBOND_SMALL;
			pointsLabelFontSize = JmolConstants.LABEL_DEFAULT_FONTSIZE;
			labelOffsetX = JmolConstants.LABEL_DEFAULT_X_OFFSET;
			labelOffsetY = JmolConstants.LABEL_DEFAULT_Y_OFFSET;
		}
		virtual internal int PercentVdwAtom
		{
			set
			{
				this.percentVdwAtom = value;
			}
			
		}
		virtual internal short MarBond
		{
			set
			{
				this.marBond = value;
			}
			
		}
		virtual internal sbyte ModeMultipleBond
		{
			set
			{
				this.modeMultipleBond = value;
			}
			
		}
		virtual internal bool ShowMultipleBonds
		{
			set
			{
				this.showMultipleBonds = value;
			}
			
		}
		virtual internal bool ShowAtoms
		{
			set
			{
				this.showAtoms = value;
			}
			
		}
		virtual internal bool ShowBonds
		{
			set
			{
				this.showBonds = value;
			}
			
		}
		virtual internal bool ShowHydrogens
		{
			set
			{
				this.showHydrogens = value;
			}
			
		}
		virtual internal bool ShowVectors
		{
			set
			{
				this.showVectors = value;
			}
			
		}
		virtual internal bool ShowMeasurements
		{
			set
			{
				this.showMeasurements = value;
			}
			
		}
		virtual internal bool ShowMeasurementLabels
		{
			set
			{
				this.showMeasurementLabels = value;
			}
			
		}
		virtual internal System.String PropertyStyleString
		{
			set
			{
				propertyStyleString = value;
			}
			
		}
		virtual internal bool ZeroBasedXyzRasmol
		{
			set
			{
				this.zeroBasedXyzRasmol = value;
			}
			
		}
		virtual internal bool FrankOn
		{
			set
			{
				this.frankOn = value;
			}
			
		}
		virtual internal bool SsbondsBackbone
		{
			set
			{
				this.ssbondsBackbone = value;
			}
			
		}
		virtual internal bool HbondsBackbone
		{
			set
			{
				this.hbondsBackbone = value;
			}
			
		}
		virtual internal bool HbondsSolid
		{
			set
			{
				this.hbondsSolid = value;
			}
			
		}
		virtual internal int LabelFontSize
		{
			set
			{
				this.pointsLabelFontSize = value <= 0?JmolConstants.LABEL_DEFAULT_FONTSIZE:value;
			}
			
		}
		
		internal Viewer viewer;
		
		internal StyleManager(Viewer viewer)
		{
			InitBlock();
			this.viewer = viewer;
		}
		
		internal int percentVdwAtom = 20;
		
		internal short marBond = 100;
		
		//UPGRADE_NOTE: The initialization of  'modeMultipleBond' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		internal sbyte modeMultipleBond;
		
		internal bool showMultipleBonds = true;
		
		internal bool showAtoms = true;
		
		internal bool showBonds = true;
		
		internal bool showHydrogens = true;
		
		internal bool showVectors = true;
		
		internal bool showMeasurements = true;
		
		internal bool showMeasurementLabels = true;
		
		internal System.String measureDistanceUnits = "nanometers";
		internal virtual bool setMeasureDistanceUnits(System.String units)
		{
			if (units.ToUpper().Equals("angstroms".ToUpper()))
				measureDistanceUnits = "angstroms";
			else if (units.ToUpper().Equals("nanometers".ToUpper()))
				measureDistanceUnits = "nanometers";
			else if (units.ToUpper().Equals("picometers".ToUpper()))
				measureDistanceUnits = "picometers";
			else
				return false;
			return true;
		}
		
		internal System.String propertyStyleString = "";
		
		internal bool zeroBasedXyzRasmol = false;
		
		internal virtual void  setCommonDefaults()
		{
			viewer.zoomToPercent(100);
			viewer.PercentVdwAtom = 20;
			viewer.PerspectiveDepth = true;
			viewer.BondTolerance = 0.45f;
			viewer.MinBondDistance = 0.40f;
			viewer.MarBond = (short) 150;
		}
		
		internal virtual void  setJmolDefaults()
		{
			setCommonDefaults();
			viewer.DefaultColors = "jmol";
			viewer.AxesOrientationRasmol = false;
			ZeroBasedXyzRasmol = false;
		}
		
		internal virtual void  setRasmolDefaults()
		{
			setCommonDefaults();
			viewer.DefaultColors = "rasmol";
			viewer.AxesOrientationRasmol = true;
			ZeroBasedXyzRasmol = true;
			viewer.PercentVdwAtom = 0;
			viewer.MarBond = (short) 1;
		}
		
		internal bool frankOn;
		
		internal bool ssbondsBackbone;
		
		internal bool hbondsBackbone;
		
		internal bool hbondsSolid;
		
		/// <summary>*************************************************************
		/// label related
		/// **************************************************************
		/// </summary>
		
		//UPGRADE_NOTE: The initialization of  'pointsLabelFontSize' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		internal int pointsLabelFontSize;
		
		//UPGRADE_NOTE: The initialization of  'labelOffsetX' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		internal int labelOffsetX;
		//UPGRADE_NOTE: The initialization of  'labelOffsetY' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		internal int labelOffsetY;
		internal virtual void  setLabelOffset(int offsetX, int offsetY)
		{
			labelOffsetX = offsetX;
			labelOffsetY = offsetY;
		}
		
		internal static System.String[] formattingStrings = new System.String[]{"0", "0.0", "0.00", "0.000", "0.0000", "0.00000", "0.000000", "0.0000000", "0.00000000", "0.000000000"};
		
		//UPGRADE_ISSUE: Class 'java.text.DecimalFormat' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javatextDecimalFormat'"
		internal DecimalFormat[] formatters;
		
		internal virtual System.String formatDecimal(float value_Renamed, int decimalDigits)
		{
			if (decimalDigits < 0)
				return "" + value_Renamed;
			if (formatters == null)
			{
				//UPGRADE_ISSUE: Class 'java.text.DecimalFormat' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javatextDecimalFormat'"
				formatters = new DecimalFormat[formattingStrings.Length];
			}
			if (decimalDigits >= formattingStrings.Length)
				decimalDigits = formattingStrings.Length - 1;
			//UPGRADE_ISSUE: Class 'java.text.DecimalFormat' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javatextDecimalFormat'"
			DecimalFormat formatter = formatters[decimalDigits];
			if (formatter == null)
			{
				//UPGRADE_ISSUE: Constructor 'java.text.DecimalFormat.DecimalFormat' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javatextDecimalFormat'"
				formatter = formatters[decimalDigits] = new DecimalFormat(formattingStrings[decimalDigits]);
			}
			return formatter.FormatDouble(value_Renamed);
		}
	}
}