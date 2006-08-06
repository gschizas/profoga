/*
* File Name   : GraphCharacteristics.java
* Description : This class contains and modifies a list of spectrometer plot controlling factors. Ideally,
*               all manipulation or setting of values controlling the look and feel of the graphs should
*               flow through the data elements in this class.
* Author      : Shravan Sadasivan
* Organization: Department Of Chemistry,
*				 The Ohio State University
*/
using System;
namespace org.jmol.jcamp
{
	
	public class GraphCharacteristics
	{
		/// <summary> Method to split and create an <code>ArrayList</code> of Integration relationship values</summary>
		virtual public System.String UnsortedIntegrationValues
		{
			set
			{
				System.String[] _temp = value.split(INTEGRATION_DELIM);
				System.Collections.ArrayList _tempList = new System.Collections.ArrayList();
				
				for (int i = 0; i < _temp.Length; i++)
				{
					_tempList.Add(_temp[i]);
				}
				
				this._unsortedIntegrationValues = _tempList;
				setIntegrationValues(_tempList);
			}
			
		}
		/// <summary> Setter and Getter methods for the 'Zoom In' option</summary>
		virtual public bool ZoomIn
		{
			get
			{
				return this._zoomIn;
			}
			
			set
			{
				this._zoomIn = value;
			}
			
		}
		/// <summary> Setter and Getter methods for the 'Grid' option</summary>
		virtual public bool Grid
		{
			get
			{
				return this._grid;
			}
			
			set
			{
				this._grid = value;
			}
			
		}
		/// <summary> Setter and Getter methods for the 'Integrate' option</summary>
		virtual public bool Integrate
		{
			get
			{
				return this._integrate;
			}
			
			set
			{
				this._integrate = value;
			}
			
		}
		/// <summary> Setter and Getter methods for the 'Reverse' option</summary>
		virtual public bool Reverse
		{
			get
			{
				return this._reverse;
			}
			
			set
			{
				this._reverse = value;
			}
			
		}
		/// <summary> Setter and Getter methods for the 'Axis Color' option</summary>
		virtual public System.String AxisColor
		{
			get
			{
				return this._axisColor;
			}
			
			set
			{
				this._axisColor = value;
			}
			
		}
		/// <summary> Setter and Getter methods for the 'Axis Text Color' option</summary>
		virtual public System.String AxisTextColor
		{
			get
			{
				return this._axisTextColor;
			}
			
			set
			{
				this._axisTextColor = value;
			}
			
		}
		/// <summary> Setter and Getter methods for the 'Integrate Curve Color' option</summary>
		virtual public System.String IntegrateCurveColor
		{
			get
			{
				return this._integrateCurveColor;
			}
			
			set
			{
				this._integrateCurveColor = value;
			}
			
		}
		/// <summary> Setter and Getter methods for the 'Graph Curve Color' option</summary>
		virtual public System.String GraphCurveColor
		{
			get
			{
				return this._graphCurveColor;
			}
			
			set
			{
				this._graphCurveColor = value;
			}
			
		}
		/// <summary> Setter and Getter methods for the 'Graph Text Color' option</summary>
		virtual public System.String TextColor
		{
			get
			{
				return this._textColor;
			}
			
			set
			{
				this._textColor = value;
			}
			
		}
		virtual public System.String IntegrateTextColor
		{
			get
			{
				return _integrateTextColor;
			}
			
			set
			{
				this._integrateTextColor = value;
			}
			
		}
		private const System.String INTEGRATION_VALUES_DELIM = ":";
		private const System.String INTEGRATION_DELIM = ",";
		private const int MAX_DECIMALS = 2;
		
		private bool _zoomIn; /* Parameter to indicate if "Zoom In" is desired on the graph */
		private bool _integrate; /* Parameter to indicate if "Integration" is desired on the graph */
		private bool _grid; /* Parameter to indicate if "Grid" is desired on the graph */
		private bool _reverse; /* Parameter to indicate if "Reverse" is desired on the graph */
		private System.String _allIntegrationValues = null;
		private System.Collections.Hashtable _integrationValues = null; /* Sorted mapping of integration curve values to be printed */
		private System.Collections.ArrayList _unsortedIntegrationValues = null; /* Unsorted integration curve value strings */
		
		//Variables to control the various colors in the elements of the graph
		private System.String _textColor = null;
		private System.String _axisColor = null;
		private System.String _axisTextColor = null;
		private System.String _integrateCurveColor = null;
		private System.String _integrateTextColor = null;
		private System.String _graphCurveColor = null;
		//UPGRADE_ISSUE: Class 'java.text.DecimalFormat' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javatextDecimalFormat'"
		private DecimalFormat _decForm = null;
		private System.String _lastPoint = null;
		
		public GraphCharacteristics()
		{
			this._zoomIn = false;
			this._integrate = false;
			this._grid = false;
			this._reverse = false;
			this._allIntegrationValues = new System.Text.StringBuilder().ToString();
			this._integrationValues = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
			this._unsortedIntegrationValues = new System.Collections.ArrayList();
			this._decForm = null;
		}
		
		public GraphCharacteristics(bool zoomIn, bool integrate, bool grid, bool reverse, System.String allIntegrationValues, System.String axisColor, System.String axisTextColor, System.String integrateCurveColor, System.String graphCurveColor, System.String textColor, System.String integrateTextColor)
		{
			this._zoomIn = zoomIn;
			this._integrate = integrate;
			this._grid = grid;
			this._reverse = reverse;
			this._allIntegrationValues = allIntegrationValues;
			setIntegrationValues(this._unsortedIntegrationValues);
			this._axisColor = axisColor;
			this._axisTextColor = axisTextColor;
			this._integrateCurveColor = integrateCurveColor;
			this._integrateTextColor = integrateTextColor;
			this._graphCurveColor = graphCurveColor;
			this._textColor = textColor;
			this._decForm = null;
		}
		
		/// <summary> Method to create a <code>HashTable</code> of integration curve area relationship
		/// to the concerned points
		/// </summary>
		public virtual void  setIntegrationValues(System.Collections.ArrayList unsortedIntegrationValues)
		{
			System.String[] _temp = null; // Output of the String split operation
			System.String _tempString = new System.Text.StringBuilder().ToString();
			System.Collections.Hashtable _tempTable = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable()); // Temp Storage for values extracted from the integration values string
			
			for (int i = 0; i < unsortedIntegrationValues.Count; i++)
			{
				_tempString = ((System.String) unsortedIntegrationValues[i]);
				_temp = _tempString.split(INTEGRATION_VALUES_DELIM);
				_tempTable[_temp[0]] = _temp[1];
			}
			
			this._integrationValues = _tempTable;
		}
		
		/// <summary> </summary>
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		private System.String isIntegrationCurvePoint(ref System.Double point)
		{
			System.String[] formats = new System.String[]{"####.00", "####.0", "####"};
			for (int i = 0; i <= MAX_DECIMALS; i++)
			{
				//UPGRADE_ISSUE: Constructor 'java.text.DecimalFormat.DecimalFormat' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javatextDecimalFormat'"
				_decForm = new DecimalFormat(formats[i]);
				//UPGRADE_TODO: Method 'java.text.Format.format' was converted to 'System.String.Format' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javatextFormatformat_javalangObject'"
				if (this._integrationValues.ContainsKey(System.String.Format(_decForm, point)))
				{
					//UPGRADE_TODO: Method 'java.text.Format.format' was converted to 'System.String.Format' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javatextFormatformat_javalangObject'"
					if (_lastPoint == null || !_lastPoint.ToUpper().Equals(System.String.Format(_decForm, point).ToUpper()))
					{
						//UPGRADE_TODO: Method 'java.text.Format.format' was converted to 'System.String.Format' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javatextFormatformat_javalangObject'"
						_lastPoint = System.String.Format(_decForm, point);
						//UPGRADE_TODO: Method 'java.text.Format.format' was converted to 'System.String.Format' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javatextFormatformat_javalangObject'"
						return System.String.Format(_decForm, point);
					}
					else
					{
						return null;
					}
				}
			}
			return null;
		}
		
		/// <summary> </summary>
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		public virtual System.String getIntegrationCurveAreaValue(ref System.Double point)
		{
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
			System.String integratePeakValue = isIntegrationCurvePoint(ref point);
			
			if (integratePeakValue != null)
			{
				return (System.String) this._integrationValues[integratePeakValue];
			}
			return null;
		}
		
		/// <summary> Setter and Getter methods for the 'Integration Values' option</summary>
		public virtual void  setIntegrationValues()
		{
			
		}
	}
}