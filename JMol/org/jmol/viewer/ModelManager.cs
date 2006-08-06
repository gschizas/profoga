/* $RCSfile$
* $Author: hansonr $
* $Date: 2006-04-06 13:46:05 +0200 (jeu., 06 avr. 2006) $
* $Revision: 4925 $
*
* Copyright (C) 2003-2005  Miguel, Jmol Development, www.jmol.org
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
using JmolAdapter = org.jmol.api.JmolAdapter;
//UPGRADE_TODO: The type 'javax.vecmath.Point3f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Point3f = javax.vecmath.Point3f;
//UPGRADE_TODO: The type 'javax.vecmath.Vector3f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Vector3f = javax.vecmath.Vector3f;
//UPGRADE_TODO: The type 'javax.vecmath.Point3i' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Point3i = javax.vecmath.Point3i;
namespace org.jmol.viewer
{
	
	class ModelManager
	{
		private void  InitBlock()
		{
			shapeSizes = new int[JmolConstants.SHAPE_MAX];
			shapeProperties = new System.Collections.Hashtable[JmolConstants.SHAPE_MAX];
		}
		virtual internal Frame Frame
		{
			get
			{
				return frame;
			}
			
		}
		virtual internal JmolAdapter ExportJmolAdapter
		{
			get
			{
				return (frame == null)?null:frame.ExportJmolAdapter;
			}
			
		}
		virtual internal System.String ModelSetName
		{
			get
			{
				return modelSetName;
			}
			
		}
		virtual internal System.String ModelSetFileName
		{
			get
			{
				return fileName;
			}
			
		}
		virtual internal System.String ModelSetPathName
		{
			get
			{
				return fullPathName;
			}
			
		}
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		virtual internal System.Collections.Specialized.NameValueCollection ModelSetProperties
		{
			get
			{
				return frame == null?null:frame.ModelSetProperties;
			}
			
		}
		virtual internal System.String ModelSetTypeName
		{
			get
			{
				return frame == null?null:frame.ModelSetTypeName;
			}
			
		}
		virtual internal int ModelCount
		{
			get
			{
				return (frame == null)?0:frame.ModelCount;
			}
			
		}
		virtual internal float RotationRadius
		{
			get
			{
				return (frame == null)?1:frame.RotationRadius;
			}
			
		}
		virtual internal Point3f BoundBoxCenter
		{
			get
			{
				return (frame == null)?null:frame.BoundBoxCenter;
			}
			
		}
		virtual internal Vector3f BoundBoxCornerVector
		{
			get
			{
				return (frame == null)?null:frame.BoundBoxCornerVector;
			}
			
		}
		virtual internal int ChainCount
		{
			get
			{
				return (frame == null)?0:frame.ChainCount;
			}
			
		}
		virtual internal int GroupCount
		{
			get
			{
				return (frame == null)?0:frame.GroupCount;
			}
			
		}
		virtual internal int PolymerCount
		{
			get
			{
				return (frame == null)?0:frame.PolymerCount;
			}
			
		}
		virtual internal int AtomCount
		{
			get
			{
				return (frame == null)?0:frame.AtomCount;
			}
			
		}
		virtual internal int BondCount
		{
			get
			{
				return (frame == null)?0:frame.BondCount;
			}
			
		}
		virtual internal System.Collections.BitArray CenterBitSet
		{
			set
			{
				if (frame == null)
					return ;
				Point3f center = null;
				if (value != null)
				{
					int countSelected = 0;
					center = pointT;
					center.set_Renamed(0, 0, 0);
					for (int i = AtomCount; --i >= 0; )
					{
						if (!value.Get(i))
							continue;
						++countSelected;
						center.add(frame.getAtomPoint3f(i));
					}
					if (countSelected > 0)
						center.scale(1.0f / countSelected);
					// just divide by the quantity
					else
						center = null;
				}
				if (!viewer.WindowCentered)
				{
					if (center == null)
						center = frame.RotationCenterDefault;
					Point3i newCenterScreen = viewer.transformPoint(center);
					viewer.translateCenterTo(newCenterScreen.x, newCenterScreen.y);
				}
				frame.setRotationCenter(center);
			}
			
		}
		virtual internal Point3f DefaultRotationCenter
		{
			get
			{
				return (frame == null?null:frame.DefaultRotationCenter);
			}
			
		}
		virtual internal bool AutoBond
		{
			set
			{
				autoBond = value;
			}
			
		}
		virtual internal float BondTolerance
		{
			set
			{
				this.bondTolerance = value;
			}
			
		}
		virtual internal float MinBondDistance
		{
			set
			{
				this.minBondDistance = value;
			}
			
		}
		virtual internal float SolventProbeRadius
		{
			set
			{
				this.solventProbeRadius = value;
			}
			
		}
		virtual internal bool SolventOn
		{
			set
			{
				this.solventOn = value;
			}
			
		}
		virtual internal System.Collections.BitArray ElementsPresentBitSet
		{
			get
			{
				return (frame == null)?null:frame.ElementsPresentBitSet;
			}
			
		}
		virtual internal System.Collections.BitArray GroupsPresentBitSet
		{
			get
			{
				return (frame == null)?null:frame.GroupsPresentBitSet;
			}
			
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'viewer '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Viewer viewer;
		//UPGRADE_NOTE: Final was removed from the declaration of 'adapter '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal JmolAdapter adapter;
		
		internal ModelManager(Viewer viewer, JmolAdapter adapter)
		{
			InitBlock();
			this.viewer = viewer;
			this.adapter = adapter;
		}
		
		internal System.String fullPathName;
		internal System.String fileName;
		internal System.String modelSetName;
		//  int frameCount = 0;
		internal bool haveFile = false;
		//  int currentFrameNumber;
		internal Frame frame;
		//  Frame[] frames;
		
		internal virtual void  setClientFile(System.String fullPathName, System.String fileName, System.Object clientFile)
		{
			if (clientFile == null)
			{
				fullPathName = fileName = modelSetName = null;
				frame = null;
				haveFile = false;
			}
			else
			{
				this.fullPathName = fullPathName;
				this.fileName = fileName;
				modelSetName = adapter.getAtomSetCollectionName(clientFile);
				if (modelSetName != null)
				{
					modelSetName = modelSetName.Trim();
					if (modelSetName.Length == 0)
						modelSetName = null;
				}
				if (modelSetName == null)
					modelSetName = reduceFilename(fileName);
				frame = new Frame(viewer, adapter, clientFile);
				haveFile = true;
			}
		}
		
		internal virtual System.String reduceFilename(System.String fileName)
		{
			if (fileName == null)
				return null;
			int ichDot = fileName.IndexOf('.');
			if (ichDot > 0)
				fileName = fileName.Substring(0, (ichDot) - (0));
			if (fileName.Length > 24)
				fileName = fileName.Substring(0, (20) - (0)) + " ...";
			return fileName;
		}
		
		internal virtual System.String getClientAtomStringProperty(System.Object clientAtom, System.String propertyName)
		{
			return adapter.getClientAtomStringProperty(clientAtom, propertyName);
		}
		
		internal virtual System.String getModelSetProperty(System.String propertyName)
		{
			return frame == null?null:frame.getModelSetProperty(propertyName);
		}
		
		internal virtual bool modelSetHasVibrationVectors()
		{
			return frame == null?false:frame.modelSetHasVibrationVectors();
		}
		
		internal virtual bool modelHasVibrationVectors(int modelIndex)
		{
			return frame == null?false:frame.modelHasVibrationVectors(modelIndex);
		}
		
		internal virtual System.String getModelName(int modelIndex)
		{
			return (frame == null)?null:frame.getModelName(modelIndex);
		}
		
		internal virtual int getModelNumber(int modelIndex)
		{
			return (frame == null)?- 1:frame.getModelNumber(modelIndex);
		}
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		internal virtual System.Collections.Specialized.NameValueCollection getModelProperties(int modelIndex)
		{
			return frame == null?null:frame.getModelProperties(modelIndex);
		}
		
		internal virtual System.String getModelProperty(int modelIndex, System.String propertyName)
		{
			return frame == null?null:frame.getModelProperty(modelIndex, propertyName);
		}
		
		internal virtual int getModelNumberIndex(int modelNumber)
		{
			return (frame == null)?- 1:frame.getModelNumberIndex(modelNumber);
		}
		
		internal virtual bool hasVibrationVectors()
		{
			return frame.hasVibrationVectors();
		}
		
		internal virtual void  increaseRotationRadius(float increaseInAngstroms)
		{
			if (frame != null)
				frame.increaseRotationRadius(increaseInAngstroms);
		}
		
		internal virtual int getPolymerCountInModel(int modelIndex)
		{
			return (frame == null)?0:frame.getPolymerCountInModel(modelIndex);
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'pointT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private Point3f pointT = new Point3f();
		
		internal virtual void  setRotationCenter(Point3f center)
		{
			if (frame != null)
				frame.setRotationCenter(center);
		}
		
		internal virtual Point3f getRotationCenter()
		{
			return (frame == null?null:frame.RotationCenter);
		}
		
		internal virtual void  setRotationCenter(System.String relativeTo, float x, float y, float z)
		{
			if (frame == null)
				return ;
			pointT.set_Renamed(x, y, z);
			if ((System.Object) relativeTo == (System.Object) "average")
				pointT.add(frame.AverageAtomPoint);
			else if ((System.Object) relativeTo == (System.Object) "boundbox")
				pointT.add(frame.BoundBoxCenter);
			else if ((System.Object) relativeTo != (System.Object) "absolute")
				pointT.set_Renamed(frame.RotationCenterDefault);
			frame.setRotationCenter(pointT);
		}
		
		internal bool autoBond = true;
		
		// angstroms of slop ... from OpenBabel ... mth 2003 05 26
		internal float bondTolerance = 0.45f;
		
		// minimum acceptable bonding distance ... from OpenBabel ... mth 2003 05 26
		internal float minBondDistance = 0.4f;
		
		/*
		void deleteAtom(int atomIndex) {
		frame.deleteAtom(atomIndex);
		}
		*/
		
		internal virtual bool frankClicked(int x, int y)
		{
			return (getShapeSize(JmolConstants.SHAPE_FRANK) != 0 && frame.frankClicked(x, y));
		}
		
		internal virtual int findNearestAtomIndex(int x, int y)
		{
			return (frame == null)?- 1:frame.findNearestAtomIndex(x, y);
		}
		
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		internal virtual System.Collections.BitArray findAtomsInRectangle(ref System.Drawing.Rectangle rectRubber)
		{
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
			return frame.findAtomsInRectangle(ref rectRubber);
		}
		
		// FIXME mth 2004 02 23 -- this does *not* belong here
		internal float solventProbeRadius = 1.2f;
		
		internal bool solventOn = false;
		
		/// <summary>*************************************************************
		/// shape support
		/// **************************************************************
		/// </summary>
		
		//UPGRADE_NOTE: The initialization of  'shapeSizes' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		internal int[] shapeSizes;
		//UPGRADE_NOTE: The initialization of  'shapeProperties' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		internal System.Collections.Hashtable[] shapeProperties;
		
		internal virtual void  loadShape(int shapeID)
		{
			if (frame != null)
				frame.loadShape(shapeID);
		}
		
		internal virtual void  setShapeSize(int shapeType, int size, System.Collections.BitArray bsSelected)
		{
			shapeSizes[shapeType] = size;
			if (frame != null)
				frame.setShapeSize(shapeType, size, bsSelected);
		}
		
		internal virtual int getShapeSize(int shapeType)
		{
			return shapeSizes[shapeType];
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'NULL_SURROGATE '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private static readonly System.Object NULL_SURROGATE = new System.Object();
		
		internal virtual void  setShapeProperty(int shapeType, System.String propertyName, System.Object value_Renamed, System.Collections.BitArray bsSelected)
		{
			System.Collections.Hashtable props = shapeProperties[shapeType];
			if (props == null)
				props = shapeProperties[shapeType] = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
			
			// be sure to intern all propertyNames!
			propertyName = String.Intern(propertyName);
			/*
			System.out.println("propertyName=" + propertyName + "\n" +
			"value=" + value);
			*/
			
			// Hashtables cannot store null values :-(
			props[propertyName] = value_Renamed != null?value_Renamed:NULL_SURROGATE;
			if (frame != null)
				frame.setShapeProperty(shapeType, propertyName, value_Renamed, bsSelected);
		}
		
		internal virtual System.Object getShapeProperty(int shapeType, System.String propertyName, int index)
		{
			System.Object value_Renamed = null;
			if (frame != null)
				value_Renamed = frame.getShapeProperty(shapeType, propertyName, index);
			if (value_Renamed == null)
			{
				System.Collections.Hashtable props = shapeProperties[shapeType];
				if (props != null)
				{
					value_Renamed = props[propertyName];
					if (value_Renamed == NULL_SURROGATE)
						return value_Renamed = null;
				}
			}
			return value_Renamed;
		}
		
		internal virtual int getAtomIndexFromAtomNumber(int atomNumber)
		{
			return (frame == null)?- 1:frame.getAtomIndexFromAtomNumber(atomNumber);
		}
		
		internal virtual void  calcSelectedGroupsCount(System.Collections.BitArray bsSelected)
		{
			if (frame != null)
				frame.calcSelectedGroupsCount(bsSelected);
		}
		
		internal virtual void  calcSelectedMonomersCount(System.Collections.BitArray bsSelected)
		{
			if (frame != null)
				frame.calcSelectedMonomersCount(bsSelected);
		}
		
		////////////////////////////////////////////////////////////////
		// Access to atom properties for clients
		////////////////////////////////////////////////////////////////
		
		internal virtual System.String getAtomInfo(int i)
		{
			return frame.getAtomAt(i).Info;
		}
		
		/*
		String getAtomInfoChime(int i) {
		Atom atom = frame.atoms[i];
		PdbAtom pdbAtom = atom.pdbAtom;
		if (pdbAtom == null)
		return "Atom: " + atom.getAtomicSymbol() + " " + atom.getAtomno();
		return "Atom: " + pdbAtom.getAtomName() + " " + pdbAtom.getAtomSerial() +
		" " + pdbAtom.getGroup3() + " " + pdbAtom.getSeqcodeString() +
		" Chain:" + pdbAtom.getChainID();
		}*/
		
		internal virtual System.String getElementSymbol(int i)
		{
			return frame.getAtomAt(i).ElementSymbol;
		}
		
		internal virtual int getElementNumber(int i)
		{
			return frame.getAtomAt(i).ElementNumber;
		}
		
		internal virtual System.String getAtomName(int i)
		{
			return frame.getAtomAt(i).AtomName;
		}
		
		internal virtual int getAtomNumber(int i)
		{
			return frame.getAtomAt(i).AtomNumber;
		}
		
		internal virtual float getAtomX(int i)
		{
			return frame.getAtomAt(i).AtomX;
		}
		
		internal virtual float getAtomY(int i)
		{
			return frame.getAtomAt(i).AtomY;
		}
		
		internal virtual float getAtomZ(int i)
		{
			return frame.getAtomAt(i).AtomZ;
		}
		
		internal virtual Point3f getAtomPoint3f(int i)
		{
			return frame.getAtomAt(i).Point3f;
		}
		
		internal virtual float getAtomRadius(int i)
		{
			return frame.getAtomAt(i).Radius;
		}
		
		internal virtual short getAtomColix(int i)
		{
			return frame.getAtomAt(i).Colix;
		}
		
		internal virtual System.String getAtomChain(int i)
		{
			return "" + frame.getAtomAt(i).ChainID;
		}
		
		internal virtual System.String getAtomSequenceCode(int i)
		{
			return frame.getAtomAt(i).SeqcodeString;
		}
		
		internal virtual int getAtomModelIndex(int i)
		{
			return frame.getAtomAt(i).ModelIndex;
		}
		
		internal virtual Point3f getBondPoint3f1(int i)
		{
			return frame.getBondAt(i).Atom1.Point3f;
		}
		
		internal virtual Point3f getBondPoint3f2(int i)
		{
			return frame.getBondAt(i).Atom2.Point3f;
		}
		
		internal virtual float getBondRadius(int i)
		{
			return frame.getBondAt(i).Radius;
		}
		
		internal virtual short getBondOrder(int i)
		{
			return frame.getBondAt(i).Order;
		}
		
		internal virtual short getBondColix1(int i)
		{
			return frame.getBondAt(i).Colix1;
		}
		
		internal virtual short getBondColix2(int i)
		{
			return frame.getBondAt(i).Colix2;
		}
		
		internal virtual int getBondModelIndex(int i)
		{
			Atom atom = frame.getBondAt(i).Atom1;
			if (atom != null)
			{
				return atom.ModelIndex;
			}
			atom = frame.getBondAt(i).Atom2;
			if (atom != null)
			{
				return atom.ModelIndex;
			}
			return 0;
		}
		
		public virtual Point3f[] getPolymerLeadMidPoints(int modelIndex, int polymerIndex)
		{
			Polymer polymer = frame.getPolymerAt(modelIndex, polymerIndex);
			return polymer.LeadMidpoints;
		}
	}
}