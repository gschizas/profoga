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
	
	abstract class MeshCollection:SelectionIndependentShape
	{
		
		internal int meshCount;
		internal Mesh[] meshes = new Mesh[4];
		internal Mesh currentMesh;
		
		internal override void  initShape()
		{
			colix = Graphics3D.ORANGE;
		}
		
		internal override void  setProperty(System.String propertyName, System.Object value_Renamed, System.Collections.BitArray bs)
		{
			
			/*
			System.out.println("setProperty(" + propertyName + "," + value + ")");
			System.out.println("meshCount=" + meshCount +
			" currentMesh=" + currentMesh);
			for (int i = 0; i < meshCount; ++i) {
			Mesh mesh = meshes[i];
			System.out.println("i=" + i +
			" mesh.meshID=" + mesh.meshID +
			" mesh.visible=" + mesh.visible +
			" mesh.translucent=" + mesh.translucent +
			" mesh.colix=" + mesh.meshColix);
			}
			*/
			if ((System.Object) "meshID" == (System.Object) propertyName)
			{
				System.String meshID = (System.String) value_Renamed;
				//      System.out.println("meshID=" + meshID);
				if (meshID == null)
				{
					currentMesh = null;
					return ;
				}
				for (int i = meshCount; --i >= 0; )
				{
					currentMesh = meshes[i];
					if (meshID.Equals(currentMesh.meshID))
						return ;
				}
				allocMesh(meshID);
				return ;
			}
			if ((System.Object) "on" == (System.Object) propertyName)
			{
				if (currentMesh != null)
					currentMesh.visible = true;
				else
				{
					for (int i = meshCount; --i >= 0; )
						meshes[i].visible = true;
				}
				return ;
			}
			if ((System.Object) "off" == (System.Object) propertyName)
			{
				if (currentMesh != null)
					currentMesh.visible = false;
				else
				{
					for (int i = meshCount; --i >= 0; )
						meshes[i].visible = false;
				}
				return ;
			}
			if ((System.Object) "color" == (System.Object) propertyName)
			{
				if (value_Renamed != null)
				{
					colix = Graphics3D.getColix(value_Renamed);
					if (currentMesh != null)
					{
						currentMesh.colix = colix;
						currentMesh.vertexColixes = null;
					}
					else
					{
						for (int i = meshCount; --i >= 0; )
						{
							Mesh mesh = meshes[i];
							mesh.colix = colix;
							mesh.vertexColixes = null;
						}
					}
				}
				return ;
			}
			if ((System.Object) "translucency" == (System.Object) propertyName)
			{
				bool isTranslucent = ((System.Object) "translucent" == value_Renamed);
				if (currentMesh != null)
					currentMesh.Translucent = isTranslucent;
				else
				{
					for (int i = meshCount; --i >= 0; )
						meshes[i].Translucent = isTranslucent;
				}
			}
			if ((System.Object) "dots" == (System.Object) propertyName)
			{
				bool showDots = value_Renamed == (System.Object) true;
				if (currentMesh != null)
					currentMesh.showPoints = showDots;
				else
				{
					for (int i = meshCount; --i >= 0; )
						meshes[i].showPoints = showDots;
				}
				return ;
			}
			if ((System.Object) "mesh" == (System.Object) propertyName)
			{
				bool showMesh = value_Renamed == (System.Object) true;
				if (currentMesh != null)
					currentMesh.drawTriangles = showMesh;
				else
				{
					for (int i = meshCount; --i >= 0; )
						meshes[i].drawTriangles = showMesh;
				}
				return ;
			}
			if ((System.Object) "fill" == (System.Object) propertyName)
			{
				bool showFill = value_Renamed == (System.Object) true;
				if (currentMesh != null)
					currentMesh.fillTriangles = showFill;
				else
				{
					for (int i = meshCount; --i >= 0; )
						meshes[i].fillTriangles = showFill;
				}
				return ;
			}
			if ((System.Object) "delete" == (System.Object) propertyName)
			{
				if (currentMesh != null)
				{
					int iCurrent;
					for (iCurrent = meshCount; meshes[--iCurrent] != currentMesh; )
					{
					}
					for (int j = iCurrent + 1; j < meshCount; ++j)
						meshes[j - 1] = meshes[j];
					meshes[--meshCount] = null;
					currentMesh = null;
				}
				else
				{
					for (int i = meshCount; --i >= 0; )
						meshes[i] = null;
					meshCount = 0;
				}
				return ;
			}
		}
		
		internal virtual void  allocMesh(System.String meshID)
		{
			meshes = (Mesh[]) Util.ensureLength(meshes, meshCount + 1);
			currentMesh = meshes[meshCount++] = new Mesh(viewer, meshID, g3d, colix);
		}
		
		internal int ichNextParse;
		
		internal virtual float parseFloat(System.String str)
		{
			return parseFloatChecked(str, 0, str.Length);
		}
		
		internal virtual float parseFloat(System.String str, int ich)
		{
			int cch = str.Length;
			if (ich >= cch)
				return System.Single.NaN;
			return parseFloatChecked(str, ich, cch);
		}
		
		internal virtual float parseFloat(System.String str, int ichStart, int ichMax)
		{
			int cch = str.Length;
			if (ichMax > cch)
				ichMax = cch;
			if (ichStart >= ichMax)
				return System.Single.NaN;
			return parseFloatChecked(str, ichStart, ichMax);
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'decimalScale'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly float[] decimalScale = new float[]{0.1f, 0.01f, 0.001f, 0.0001f, 0.00001f, 0.000001f, 0.0000001f, 0.00000001f};
		//UPGRADE_NOTE: Final was removed from the declaration of 'tensScale'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly float[] tensScale = new float[]{10, 100, 1000, 10000, 100000, 1000000};
		
		internal virtual float parseFloatChecked(System.String str, int ichStart, int ichMax)
		{
			bool digitSeen = false;
			float value_Renamed = 0;
			int ich = ichStart;
			char ch;
			while (ich < ichMax && ((ch = str[ich]) == ' ' || ch == '\t'))
				++ich;
			bool negative = false;
			if (ich < ichMax && str[ich] == '-')
			{
				++ich;
				negative = true;
			}
			ch = (char) (0);
			while (ich < ichMax && (ch = str[ich]) >= '0' && ch <= '9')
			{
				value_Renamed = value_Renamed * 10 + (ch - '0');
				++ich;
				digitSeen = true;
			}
			if (ch == '.')
			{
				int iscale = 0;
				while (++ich < ichMax && (ch = str[ich]) >= '0' && ch <= '9')
				{
					if (iscale < decimalScale.Length)
						value_Renamed += (ch - '0') * decimalScale[iscale];
					++iscale;
					digitSeen = true;
				}
			}
			if (!digitSeen)
				value_Renamed = System.Single.NaN;
			else if (negative)
				value_Renamed = - value_Renamed;
			if (ich < ichMax && (ch == 'E' || ch == 'e'))
			{
				if (++ich >= ichMax)
					return System.Single.NaN;
				ch = str[ich];
				if ((ch == '+') && (++ich >= ichMax))
					return System.Single.NaN;
				int exponent = parseIntChecked(str, ich, ichMax);
				if (exponent == System.Int32.MinValue)
					return System.Single.NaN;
				if (exponent > 0)
					value_Renamed = (float) (value_Renamed * ((exponent < tensScale.Length)?tensScale[exponent - 1]:System.Math.Pow(10, exponent)));
				else if (exponent < 0)
					value_Renamed = (float) (value_Renamed * ((- exponent < decimalScale.Length)?decimalScale[- exponent - 1]:System.Math.Pow(10, exponent)));
			}
			else
			{
				ichNextParse = ich; // the exponent code finds its own ichNextParse
			}
			//    System.out.println("parseFloat(" + str + "," + ichStart + "," +
			//                       ichMax + ") -> " + value);
			return value_Renamed;
		}
		
		internal virtual int parseInt(System.String str)
		{
			return parseIntChecked(str, 0, str.Length);
		}
		
		internal virtual int parseInt(System.String str, int ich)
		{
			int cch = str.Length;
			if (ich >= cch)
				return System.Int32.MinValue;
			return parseIntChecked(str, ich, cch);
		}
		
		internal virtual int parseInt(System.String str, int ichStart, int ichMax)
		{
			int cch = str.Length;
			if (ichMax > cch)
				ichMax = cch;
			if (ichStart >= ichMax)
				return System.Int32.MinValue;
			return parseIntChecked(str, ichStart, ichMax);
		}
		
		internal virtual int parseIntChecked(System.String str, int ichStart, int ichMax)
		{
			bool digitSeen = false;
			int value_Renamed = 0;
			int ich = ichStart;
			char ch;
			while (ich < ichMax && ((ch = str[ich]) == ' ' || ch == '\t'))
				++ich;
			bool negative = false;
			if (ich < ichMax && str[ich] == '-')
			{
				negative = true;
				++ich;
			}
			while (ich < ichMax && (ch = str[ich]) >= '0' && ch <= '9')
			{
				value_Renamed = value_Renamed * 10 + (ch - '0');
				digitSeen = true;
				++ich;
			}
			if (!digitSeen)
				value_Renamed = System.Int32.MinValue;
			else if (negative)
				value_Renamed = - value_Renamed;
			//    System.out.println("parseInt(" + str + "," + ichStart + "," +
			//                       ichMax + ") -> " + value);
			ichNextParse = ich;
			return value_Renamed;
		}
	}
}