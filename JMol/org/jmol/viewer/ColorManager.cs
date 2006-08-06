/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-03-28 03:21:06 +0200 (mar., 28 mars 2006) $
* $Revision: 4808 $
*
* Copyright (C) 2003-2006  Miguel, Jmol Development, www.jmol.org
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
using org.jmol.g3d;
namespace org.jmol.viewer
{
	
	class ColorManager
	{
		private void  InitBlock()
		{
			colixRubberband = Graphics3D.HOTPINK;
		}
		virtual internal System.String DefaultColors
		{
			set
			{
				System.Console.Out.WriteLine("setting color scheme to:" + value);
				if (value.Equals("jmol"))
				{
					argbsCpk = JmolConstants.argbsCpk;
					viewer.ColorBackground = "black";
					viewer.setShapeColorProperty(JmolConstants.SHAPE_DOTS, 0);
				}
				else if (value.Equals("rasmol"))
				{
					copyArgbsCpk();
					int argb = JmolConstants.argbsCpkRasmol[0] | unchecked((int) 0xFF000000);
					for (int i = argbsCpk.Length; --i >= 0; )
						argbsCpk[i] = argb;
					for (int i = JmolConstants.argbsCpkRasmol.Length; --i >= 0; )
					{
						argb = JmolConstants.argbsCpkRasmol[i];
						int atomNo = argb >> 24;
						argb |= unchecked((int) 0xFF000000);
						argbsCpk[atomNo] = argb;
					}
					viewer.ColorBackground = "black";
					viewer.setShapeColorProperty(JmolConstants.SHAPE_DOTS, 0);
				}
				else
				{
					System.Console.Out.WriteLine("unrecognized color scheme");
					return ;
				}
				for (int i = JmolConstants.argbsCpk.Length; --i >= 0; )
					g3d.changeColixArgb((short) i, argbsCpk[i]);
			}
			
		}
		virtual internal int SelectionArgb
		{
			set
			{
				colixSelection = (value == 0?colixSelectionDefault:Graphics3D.getColix(value));
			}
			
		}
		virtual internal short ColixSelection
		{
			get
			{
				return colixSelection;
			}
			
		}
		virtual internal int RubberbandArgb
		{
			set
			{
				colixRubberband = (value == 0?0:Graphics3D.getColix(value));
			}
			
		}
		virtual internal int BackgroundArgb
		{
			set
			{
				argbBackground = value;
				g3d.BackgroundArgb = value;
				colixBackgroundContrast = ((Graphics3D.calcGreyscaleRgbFromRgb(value) & 0xFF) < 128?Graphics3D.WHITE:Graphics3D.BLACK);
			}
			
		}
		virtual internal System.String ColorBackground
		{
			set
			{
				if (value != null && value.Length > 0)
					BackgroundArgb = Graphics3D.getArgbFromString(value);
			}
			
		}
		/// <summary> black or white, whichever contrasts more with the current background
		/// 
		/// </summary>
		/// <returns> black or white colix value
		/// </returns>
		virtual public short ColixBackgroundContrast
		{
			get
			{
				return colixBackgroundContrast;
			}
			
		}
		virtual internal bool Specular
		{
			get
			{
				return g3d.Specular;
			}
			
			set
			{
				g3d.Specular = value;
				flushCaches();
			}
			
		}
		virtual internal int SpecularPower
		{
			set
			{
				g3d.SpecularPower = value;
				flushCaches();
			}
			
		}
		virtual internal int AmbientPercent
		{
			set
			{
				g3d.AmbientPercent = value;
				flushCaches();
			}
			
		}
		virtual internal int DiffusePercent
		{
			set
			{
				g3d.DiffusePercent = value;
				flushCaches();
			}
			
		}
		virtual internal int SpecularPercent
		{
			set
			{
				g3d.SpecularPercent = value;
				flushCaches();
			}
			
		}
		virtual internal float LightsourceZ
		{
			set
			{
				g3d.LightsourceZ = value;
				flushCaches();
			}
			
		}
		
		internal Viewer viewer;
		internal Graphics3D g3d;
		
		internal int[] argbsCpk;
		
		internal ColorManager(Viewer viewer, Graphics3D g3d)
		{
			InitBlock();
			this.viewer = viewer;
			this.g3d = g3d;
			argbsCpk = JmolConstants.argbsCpk;
		}
		
		internal virtual void  copyArgbsCpk()
		{
			argbsCpk = new int[JmolConstants.argbsCpk.Length];
			for (int i = JmolConstants.argbsCpk.Length; --i >= 0; )
				argbsCpk[i] = JmolConstants.argbsCpk[i];
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'colixSelectionDefault '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_NOTE: The initialization of  'colixSelectionDefault' was moved to static method 'org.jmol.viewer.ColorManager'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		internal static readonly short colixSelectionDefault;
		
		internal short colixSelection = colixSelectionDefault;
		
		//UPGRADE_NOTE: The initialization of  'colixRubberband' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		internal short colixRubberband;
		
		internal int argbBackground;
		internal short colixBackgroundContrast;
		
		internal virtual short getColixAtom(Atom atom)
		{
			return getColixAtomPalette(atom, "cpk");
		}
		
		internal virtual short getColixAtomPalette(Atom atom, System.String palette)
		{
			int argb = 0;
			int index;
			if ((System.Object) "cpk" == (System.Object) palette)
			{
				// Note that CPK colors can be changed based upon user preference
				// therefore, a changable colix is allocated in this case
				short id = atom.ElementNumber;
				return g3d.getChangableColix(id, argbsCpk[id]);
			}
			if ((System.Object) "partialcharge" == (System.Object) palette)
			{
				// This code assumes that the range of partial charges is [-1, 1].
				index = quantize(atom.PartialCharge, - 1, 1, JmolConstants.PARTIAL_CHARGE_RANGE_SIZE);
				return g3d.getChangableColix((short) (JmolConstants.PARTIAL_CHARGE_COLIX_RED + index), JmolConstants.argbsRwbScale[index]);
			}
			else if ((System.Object) "formalcharge" == (System.Object) palette)
			{
				index = atom.FormalCharge - JmolConstants.FORMAL_CHARGE_MIN;
				return g3d.getChangableColix((short) (JmolConstants.FORMAL_CHARGE_COLIX_RED + index), JmolConstants.argbsFormalCharge[index]);
			}
			else if ((System.Object) "temperature" == (System.Object) palette || (System.Object) "fixedtemperature" == (System.Object) palette)
			{
				float lo, hi;
				if ((System.Object) "temperature" == (System.Object) palette)
				{
					Frame frame = viewer.Frame;
					lo = frame.Bfactor100Lo;
					hi = frame.Bfactor100Hi;
				}
				else
				{
					lo = 0;
					hi = 100 * 100; // scaled by 100
				}
				index = quantize(atom.Bfactor100, lo, hi, JmolConstants.argbsRwbScale.Length);
				index = JmolConstants.argbsRwbScale.Length - 1 - index;
				argb = JmolConstants.argbsRwbScale[index];
			}
			else if ((System.Object) "structure" == (System.Object) palette)
			{
				argb = JmolConstants.argbsStructure[atom.ProteinStructureType];
			}
			else if ((System.Object) "amino" == (System.Object) palette)
			{
				index = atom.GroupID;
				if (index >= JmolConstants.GROUPID_AMINO_MAX)
					index = 0;
				argb = JmolConstants.argbsAmino[index];
			}
			else if ((System.Object) "shapely" == (System.Object) palette)
			{
				index = atom.GroupID;
				if (index >= JmolConstants.GROUPID_SHAPELY_MAX)
					index = 0;
				argb = JmolConstants.argbsShapely[index];
			}
			else if ((System.Object) "chain" == (System.Object) palette)
			{
				int chain = atom.ChainID & 0x1F;
				if (chain >= JmolConstants.argbsChainAtom.Length)
					chain = chain % JmolConstants.argbsChainAtom.Length;
				argb = (atom.Hetero?JmolConstants.argbsChainHetero:JmolConstants.argbsChainAtom)[chain];
			}
			else if ((System.Object) "group" == (System.Object) palette)
			{
				// viewer.calcSelectedGroupsCount() must be called first ...
				// before we call getSelectedGroupCountWithinChain()
				// or getSelectedGropuIndexWithinChain
				// however, do not call it here because it will get recalculated
				// for each atom
				// therefore, we call it in Eval.colorObject();
				index = quantize(atom.SelectedGroupIndexWithinChain, 0, atom.SelectedGroupCountWithinChain - 1, JmolConstants.argbsRoygbScale.Length);
				index = JmolConstants.argbsRoygbScale.Length - 1 - index;
				argb = JmolConstants.argbsRoygbScale[index];
			}
			else if ((System.Object) "monomer" == (System.Object) palette)
			{
				// viewer.calcSelectedMonomersCount() must be called first ...
				index = quantize(atom.SelectedMonomerIndexWithinPolymer, 0, atom.SelectedMonomerCountWithinPolymer - 1, JmolConstants.argbsRoygbScale.Length);
				index = JmolConstants.argbsRoygbScale.Length - 1 - index;
				argb = JmolConstants.argbsRoygbScale[index];
			}
			else
			{
				System.Console.Out.WriteLine("ColorManager.getColixAtomPalette:" + " unrecognized color palette:" + palette);
				return Graphics3D.HOTPINK;
			}
			// FIXME I think that we should assert that argb != 0 here
			if (argb == 0)
				return Graphics3D.HOTPINK;
			return Graphics3D.getColix(argb);
		}
		
		internal virtual int quantize(float val, float lo, float hi, int segmentCount)
		{
			float range = hi - lo;
			if (range <= 0 || System.Single.IsNaN(val))
				return segmentCount / 2;
			float t = val - lo;
			if (t <= 0)
				return 0;
			float quanta = range / segmentCount;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			int q = (int) (t / quanta + 0.5f);
			if (q >= segmentCount)
				q = segmentCount - 1;
			return q;
		}
		
		internal virtual short getColixFromPalette(float val, float lo, float hi, System.String palette)
		{
			if ((System.Object) palette == (System.Object) "rwb")
			{
				int index = quantize(val, lo, hi, JmolConstants.argbsRwbScale.Length);
				return Graphics3D.getColix(JmolConstants.argbsRwbScale[index]);
			}
			if ((System.Object) palette == (System.Object) "roygb")
			{
				int index = quantize(val, lo, hi, JmolConstants.argbsRoygbScale.Length);
				return Graphics3D.getColix(JmolConstants.argbsRoygbScale[index]);
			}
			return Graphics3D.HOTPINK;
		}
		
		internal virtual short getColixHbondType(short order)
		{
			int argbIndex = ((order & JmolConstants.BOND_HYDROGEN_MASK) >> JmolConstants.BOND_HBOND_SHIFT);
			return Graphics3D.getColix(JmolConstants.argbsHbondType[argbIndex]);
		}
		
		internal virtual void  flushCachedColors()
		{
		}
		
		private void  flushCaches()
		{
			g3d.flushShadesAndImageCaches();
			viewer.refresh();
		}
		
		internal virtual void  setElementArgb(int elementNumber, int argb)
		{
			if (argb == 0)
			{
				if (argbsCpk == JmolConstants.argbsCpk)
					return ;
				argb = JmolConstants.argbsCpk[elementNumber];
			}
			else
				argb |= unchecked((int) 0xFF000000);
			if (argbsCpk == JmolConstants.argbsCpk)
				copyArgbsCpk();
			argbsCpk[elementNumber] = argb;
			g3d.changeColixArgb((short) elementNumber, argb);
		}
		static ColorManager()
		{
			colixSelectionDefault = Graphics3D.GOLD;
		}
	}
}