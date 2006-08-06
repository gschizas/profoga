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
	
	class Measures:Shape
	{
		
		internal const int measurementGrowthIncrement = 16;
		internal int measurementCount = 0;
		internal Measurement[] measurements = new Measurement[measurementGrowthIncrement];
		internal PendingMeasurement pendingMeasurement;
		
		internal short mad = (short) (- 1);
		internal short colix; // default to none in order to contrast with background
		internal bool showMeasurementNumbers = true;
		internal Font3D font3d;
		
		internal override void  initShape()
		{
			pendingMeasurement = new PendingMeasurement(frame);
			font3d = g3d.getFont3D(JmolConstants.MEASURE_DEFAULT_FONTSIZE);
		}
		
		internal virtual void  clear()
		{
			int countT = measurementCount;
			measurementCount = 0;
			for (int i = countT; --i >= 0; )
				measurements[i] = null;
		}
		
		internal virtual bool isDefined(int[] atomCountPlusIndices)
		{
			for (int i = measurementCount; --i >= 0; )
			{
				if (measurements[i].sameAs(atomCountPlusIndices))
					return true;
			}
			return false;
		}
		
		internal virtual void  define(int[] atomCountPlusIndices)
		{
			if (isDefined(atomCountPlusIndices))
				return ;
			Measurement measureNew = new Measurement(frame, atomCountPlusIndices);
			if (measurementCount == measurements.Length)
				measurements = (Measurement[]) Util.setLength(measurements, measurementCount + measurementGrowthIncrement);
			measurements[measurementCount++] = measureNew;
		}
		
		internal virtual bool delete(System.Object value_Renamed)
		{
			if (value_Renamed is int[])
				return delete((int[]) value_Renamed);
			if (value_Renamed is System.Int32)
				return delete(((System.Int32) value_Renamed));
			return false;
		}
		
		internal virtual bool delete(int[] atomCountPlusIndices)
		{
			for (int i = measurementCount; --i >= 0; )
			{
				if (measurements[i].sameAs(atomCountPlusIndices))
					return delete(i);
			}
			return false;
		}
		
		internal virtual bool delete(int i)
		{
			if (i < measurementCount)
			{
				Array.Copy(measurements, i + 1, measurements, i, measurementCount - i - 1);
				--measurementCount;
				measurements[measurementCount] = null;
				return true;
			}
			return false;
		}
		
		internal virtual void  toggle(int[] atomCountPlusIndices)
		{
			if (isDefined(atomCountPlusIndices))
				delete(atomCountPlusIndices);
			else
				define(atomCountPlusIndices);
		}
		
		internal virtual void  pending(int[] countPlusIndices)
		{
			pendingMeasurement.CountPlusIndices = countPlusIndices;
		}
		
		internal override void  setSize(int size, System.Collections.BitArray bsSelected)
		{
			mad = (short) size;
			System.Console.Out.WriteLine("Measures.setSize(" + size + ")");
			//    throw new NullPointerException();
		}
		
		internal override void  setProperty(System.String propertyName, System.Object value_Renamed, System.Collections.BitArray bsSelected)
		{
			if ("color".Equals(propertyName))
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				System.Console.Out.WriteLine("Measures.color set to:" + value_Renamed);
				colix = value_Renamed == null?0:Graphics3D.getColix(value_Renamed); return ;
			}
			else if ("font".Equals(propertyName))
			{
				font3d = (Font3D) value_Renamed; return ;
			}
			else if ("define".Equals(propertyName))
			{
				define((int[]) value_Renamed);
			}
			else if ("delete".Equals(propertyName))
			{
				delete(value_Renamed);
			}
			else if ("toggle".Equals(propertyName))
			{
				toggle((int[]) value_Renamed);
			}
			else if ("pending".Equals(propertyName))
			{
				pending((int[]) value_Renamed);
			}
			else if ("clear".Equals(propertyName))
			{
				clear();
			}
			else if ("showMeasurementNumbers".Equals(propertyName))
			{
				showMeasurementNumbers = ((System.Boolean) value_Renamed);
			}
			else if ("reformatDistances".Equals(propertyName))
			{
				reformatDistances();
			}
			else
				return ;
			viewer.notifyMeasurementsChanged();
		}
		
		internal override System.Object getProperty(System.String property, int index)
		{
			//    System.out.println("Measures.getProperty(" +property + "," + index +")");
			//String propertyString = (String)property;
			if ("count".Equals(property))
			{
				return (System.Int32) measurementCount;
			}
			if ("countPlusIndices".Equals(property))
			{
				return index < measurementCount?measurements[index].countPlusIndices:null;
			}
			if ("stringValue".Equals(property))
			{
				return index < measurementCount?measurements[index].strMeasurement:null;
			}
			return null;
		}
		
		internal virtual void  reformatDistances()
		{
			for (int i = measurementCount; --i >= 0; )
				measurements[i].reformatDistanceIfSelected();
		}
	}
}