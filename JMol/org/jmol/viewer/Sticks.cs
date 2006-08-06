/* $RCSfile$
* $Author: hansonr $
* $Date: 2006-04-11 13:29:47 +0200 (mar., 11 avr. 2006) $
* $Revision: 4952 $
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
//UPGRADE_TODO: The type 'javax.vecmath.Point3f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Point3f = javax.vecmath.Point3f;
using Graphics3D = org.jmol.g3d.Graphics3D;
namespace org.jmol.viewer
{
	
	class Sticks:Shape
	{
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'connectDistances '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal float[] connectDistances = new float[2];
		internal int connectDistanceCount;
		//UPGRADE_NOTE: Final was removed from the declaration of 'connectSets '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal System.Collections.BitArray[] connectSets = new System.Collections.BitArray[2];
		internal int connectSetCount;
		private const short NULL_BOND_ORDER = - 1;
		// initialized to -1;
		// for delete this means 'delete all'
		// for connect this gets turned into 'single'
		internal short connectBondOrder;
		internal int connectOperation;
		
		private const float DEFAULT_MAX_CONNECT_DISTANCE = 100000000f;
		private const float DEFAULT_MIN_CONNECT_DISTANCE = 0.1f;
		
		internal override void  setSize(int size, System.Collections.BitArray bsSelected)
		{
			short mad = (short) size;
			setMadBond(mad, JmolConstants.BOND_COVALENT_MASK, bsSelected);
		}
		
		internal override void  setProperty(System.String propertyName, System.Object value_Renamed, System.Collections.BitArray bsSelected)
		{
			//System.out.println(propertyName+" "+value+" "+bsSelected);
			if ((System.Object) "color" == (System.Object) propertyName)
			{
				short colix = Graphics3D.getColix(value_Renamed);
				setColixBond(colix, ((colix != Graphics3D.UNRECOGNIZED)?null:(System.String) value_Renamed), JmolConstants.BOND_COVALENT_MASK, bsSelected);
				return ;
			}
			if ((System.Object) "translucency" == (System.Object) propertyName)
			{
				setTranslucencyBond(value_Renamed == (System.Object) "translucent", JmolConstants.BOND_COVALENT_MASK, bsSelected);
				return ;
			}
			if ((System.Object) "resetConnectParameters" == (System.Object) propertyName)
			{
				connectDistanceCount = 0;
				connectSetCount = 0;
				connectBondOrder = NULL_BOND_ORDER;
				connectOperation = MODIFY_OR_CREATE;
				return ;
			}
			if ((System.Object) "connectDistance" == (System.Object) propertyName)
			{
				if (connectDistanceCount < connectDistances.Length)
				{
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					connectDistances[connectDistanceCount++] = (float) ((System.Single) value_Renamed);
				}
				else
					System.Console.Out.WriteLine("too many connect distances specified");
				return ;
			}
			if ((System.Object) "connectSet" == (System.Object) propertyName)
			{
				if (connectSetCount < connectSets.Length)
					connectSets[connectSetCount++] = (System.Collections.BitArray) value_Renamed;
				else
					System.Console.Out.WriteLine("too many connect sets specified");
				return ;
			}
			if ((System.Object) "connectBondOrder" == (System.Object) propertyName)
			{
				connectBondOrder = bondOrderFromString((System.String) value_Renamed);
				return ;
			}
			if ((System.Object) "connectOperation" == (System.Object) propertyName)
			{
				connectOperation = connectOperationFromString((System.String) value_Renamed);
				return ;
			}
			if ((System.Object) "applyConnectParameters" == (System.Object) propertyName)
			{
				if (connectDistanceCount < 2)
				{
					if (connectDistanceCount == 0)
						connectDistances[0] = DEFAULT_MAX_CONNECT_DISTANCE;
					connectDistances[1] = connectDistances[0];
					connectDistances[0] = DEFAULT_MIN_CONNECT_DISTANCE;
				}
				if (connectSetCount < 2)
				{
					if (connectSetCount == 0)
						connectSets[0] = bsSelected;
					connectSets[1] = connectSets[0];
					connectSets[0] = bsSelected;
				}
				if (connectOperation >= 0)
					makeConnections(connectDistances[0], connectDistances[1], connectBondOrder, connectOperation, connectSets[0], connectSets[1]);
				return ;
			}
			if ((System.Object) "rasmolCompatibleConnect" == (System.Object) propertyName)
			{
				// miguel 2006 04 02
				// use of 'connect', 'connect on', 'connect off' is deprecated
				// I suggest that support be dropped at some point in the near future
				frame.deleteAllBonds();
				// go ahead and test out the autoBond(null, null) code a bit
				frame.autoBond(null, null);
				return ;
			}
			base.setProperty(propertyName, value_Renamed, bsSelected);
		}
		
		internal virtual void  setMadBond(short mad, short bondTypeMask, System.Collections.BitArray bs)
		{
			BondIterator iter = frame.getBondIterator(bondTypeMask, bs);
			while (iter.hasNext())
				iter.next().Mad = mad;
		}
		
		internal virtual void  setColixBond(short colix, System.String palette, short bondTypeMask, System.Collections.BitArray bs)
		{
			if (colix != Graphics3D.UNRECOGNIZED)
			{
				BondIterator iter = frame.getBondIterator(bondTypeMask, bs);
				while (iter.hasNext())
					iter.next().Colix = colix;
			}
			else
			{
				System.Console.Out.WriteLine("setColixBond called with palette:" + palette);
			}
		}
		
		internal virtual void  setTranslucencyBond(bool isTranslucent, short bondTypeMask, System.Collections.BitArray bs)
		{
			System.Console.Out.WriteLine("setTranslucencyBond " + isTranslucent);
			BondIterator iter = frame.getBondIterator(bondTypeMask, bs);
			while (iter.hasNext())
				iter.next().Translucent = isTranslucent;
		}
		
		private const int DELETE_BONDS = 0;
		private const int MODIFY_ONLY = 1;
		private const int CREATE_ONLY = 2;
		private const int MODIFY_OR_CREATE = 3;
		private const int AUTO_BOND = 4;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'connectOperationStrings'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private static readonly System.String[] connectOperationStrings = new System.String[]{"delete", "modify", "create", "modifyOrCreate", "auto"};
		
		internal virtual void  makeConnections(float minDistance, float maxDistance, short order, int connectOperation, System.Collections.BitArray bsA, System.Collections.BitArray bsB)
		{
			
			//UPGRADE_TODO: The equivalent in .NET for method 'java.util.BitSet.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			System.Console.Out.WriteLine("makeConnections(" + minDistance + "," + maxDistance + "," + order + "," + connectOperation + "," + SupportClass.BitArraySupport.ToString(bsA) + "," + SupportClass.BitArraySupport.ToString(bsB) + ")");
			
			int atomCount = frame.atomCount;
			Atom[] atoms = frame.atoms;
			if (connectOperation == DELETE_BONDS)
			{
				deleteConnections(minDistance, maxDistance, order, bsA, bsB);
				return ;
			}
			if (connectOperation == AUTO_BOND)
			{
				autoBond(order, bsA, bsB);
				return ;
			}
			if (order == NULL_BOND_ORDER)
				order = JmolConstants.BOND_COVALENT_SINGLE; // default 
			float minDistanceSquared = minDistance * minDistance;
			float maxDistanceSquared = maxDistance * maxDistance;
			for (int iA = atomCount; --iA >= 0; )
			{
				if (!bsA.Get(iA))
					continue;
				Atom atomA = atoms[iA];
				Point3f pointA = atomA.point3f;
				for (int iB = atomCount; --iB >= 0; )
				{
					if (iB == iA)
						continue;
					if (!bsB.Get(iB))
						continue;
					Atom atomB = atoms[iB];
					if (atomA.modelIndex != atomB.modelIndex)
						continue;
					Bond bondAB = atomA.getBond(atomB);
					if (MODIFY_ONLY == connectOperation && bondAB == null)
						continue;
					if (CREATE_ONLY == connectOperation && bondAB != null)
						continue;
					float distanceSquared = pointA.distanceSquared(atomB.point3f);
					if (distanceSquared < minDistanceSquared || distanceSquared > maxDistanceSquared)
						continue;
					if (bondAB != null)
						bondAB.Order = order;
					else
						frame.bondAtoms(atomA, atomB, order);
				}
			}
		}
		
		internal virtual void  deleteConnections(float minDistance, float maxDistance, short order, System.Collections.BitArray bsA, System.Collections.BitArray bsB)
		{
			int bondCount = frame.bondCount;
			Bond[] bonds = frame.bonds;
			System.Collections.BitArray bsDelete = new System.Collections.BitArray(64);
			float minDistanceSquared = minDistance * minDistance;
			float maxDistanceSquared = maxDistance * maxDistance;
			if (order != NULL_BOND_ORDER && (order & JmolConstants.BOND_HYDROGEN_MASK) != 0)
				order = JmolConstants.BOND_HYDROGEN_MASK;
			for (int i = bondCount; --i >= 0; )
			{
				Bond bond = bonds[i];
				Atom atom1 = bond.atom1;
				Atom atom2 = bond.atom2;
				if (bsA.Get(atom1.atomIndex) && bsB.Get(atom2.atomIndex) || bsA.Get(atom2.atomIndex) && bsB.Get(atom1.atomIndex))
				{
					if (bond.atom1.isBonded(bond.atom2))
					{
						float distanceSquared = atom1.point3f.distanceSquared(atom2.point3f);
						if (distanceSquared >= minDistanceSquared && distanceSquared <= maxDistanceSquared)
							if (order == NULL_BOND_ORDER || order == (bond.order & ~ JmolConstants.BOND_SULFUR_MASK) || order == (bond.order & ~ JmolConstants.BOND_PARTIAL01) || order == (bond.order & ~ JmolConstants.BOND_PARTIAL12) || order == (bond.order & ~ JmolConstants.BOND_SULFUR_MASK) || (order & bond.order & JmolConstants.BOND_HYDROGEN_MASK) != 0)
								SupportClass.BitArraySupport.Set(bsDelete, i);
					}
				}
			}
			frame.deleteBonds(bsDelete);
		}
		
		internal virtual short bondOrderFromString(System.String bondOrderString)
		{
			for (int i = JmolConstants.bondOrderNames.Length; --i >= 0; )
			{
				if (JmolConstants.bondOrderNames[i].ToUpper().Equals(bondOrderString.ToUpper()))
					return JmolConstants.bondOrderValues[i];
			}
			return NULL_BOND_ORDER;
		}
		
		internal virtual int connectOperationFromString(System.String connectOperationString)
		{
			int i;
			for (i = connectOperationStrings.Length; --i >= 0; )
				if (connectOperationStrings[i].ToUpper().Equals(connectOperationString.ToUpper()))
					break;
			return i;
		}
		
		internal virtual void  autoBond(short order, System.Collections.BitArray bsA, System.Collections.BitArray bsB)
		{
			if (order == NULL_BOND_ORDER)
				frame.autoBond(bsA, bsB);
			else if (order == JmolConstants.BOND_H_REGULAR)
				frame.autoHbond(bsA, bsB);
			else
				System.Console.Out.WriteLine("Sticks.autoBond() unknown order: " + order);
		}
	}
}