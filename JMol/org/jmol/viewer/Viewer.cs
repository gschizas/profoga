/* $RCSfile$
* $Author: hansonr $
* $Date: 2006-04-11 04:43:39 +0200 (mar., 11 avr. 2006) $
* $Revision: 4951 $
*
* Copyright (C) 2002-2006  Miguel, Jmol Development, www.jmol.org
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
using org.jmol.api;
using org.jmol.g3d;
//UPGRADE_TODO: The type 'javax.vecmath.Point3f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Point3f = javax.vecmath.Point3f;
//UPGRADE_TODO: The type 'javax.vecmath.Vector3f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Vector3f = javax.vecmath.Vector3f;
//UPGRADE_TODO: The type 'javax.vecmath.Point3i' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Point3i = javax.vecmath.Point3i;
//UPGRADE_TODO: The type 'javax.vecmath.Matrix4f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Matrix4f = javax.vecmath.Matrix4f;
//UPGRADE_TODO: The type 'javax.vecmath.Matrix3f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Matrix3f = javax.vecmath.Matrix3f;
//UPGRADE_TODO: The type 'javax.vecmath.AxisAngle4f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using AxisAngle4f = javax.vecmath.AxisAngle4f;
namespace org.jmol.viewer
{
	
	/*
	* The JmolViewer can be used to render client molecules. Clients
	* implement the JmolAdapter. JmolViewer uses this interface
	* to extract information from the client data structures and
	* render the molecule to the supplied java.awt.Component
	*
	* The JmolViewer runs on Java 1.1 virtual machines.
	* The 3d graphics rendering package is a software implementation
	* of a z-buffer. It does not use Java3D and does not use Graphics2D
	* from Java 1.2. Therefore, it is well suited to building web browser
	* applets that will run on a wide variety of system configurations.
	*/
	
	sealed public class Viewer:JmolViewer
	{
		private void  InitBlock()
		{
			nullAtomIterator = new NullAtomIterator();
			nullBondIterator = new NullBondIterator();
		}
		override public System.Windows.Forms.Control AwtComponent
		{
			get
			{
				return awtComponent;
			}
			
		}
		internal float TranslationXPercent
		{
			get
			{
				return transformManager.TranslationXPercent;
			}
			
		}
		internal float TranslationYPercent
		{
			get
			{
				return transformManager.TranslationYPercent;
			}
			
		}
		internal float TranslationZPercent
		{
			get
			{
				return transformManager.TranslationZPercent;
			}
			
		}
		override public int ZoomPercent
		{
			get
			{
				return transformManager.zoomPercent;
			}
			
		}
		internal int ZoomPercentSetting
		{
			get
			{
				return transformManager.zoomPercentSetting;
			}
			
		}
		internal bool ZoomEnabled
		{
			get
			{
				return transformManager.zoomEnabled;
			}
			
			set
			{
				transformManager.ZoomEnabled = value;
				refresh();
			}
			
		}
		internal bool SlabEnabled
		{
			get
			{
				return transformManager.slabEnabled;
			}
			
			set
			{
				transformManager.SlabEnabled = value;
				refresh();
			}
			
		}
		internal int SlabPercentSetting
		{
			get
			{
				return transformManager.slabPercentSetting;
			}
			
		}
		internal int ModeSlab
		{
			get
			{
				return transformManager.modeSlab;
			}
			
			set
			{
				transformManager.ModeSlab = value;
				refresh();
			}
			
		}
		override public Matrix4f UnscaledTransformMatrix
		{
			get
			{
				return transformManager.UnscaledTransformMatrix;
			}
			
		}
		internal float ScalePixelsPerAngstrom
		{
			get
			{
				return transformManager.scalePixelsPerAngstrom;
			}
			
		}
		override public bool PerspectiveDepth
		{
			get
			{
				return transformManager.perspectiveDepth;
			}
			
			set
			{
				transformManager.PerspectiveDepth = value;
				refresh();
			}
			
		}
		override public bool AxesOrientationRasmol
		{
			get
			{
				return transformManager.axesOrientationRasmol;
			}
			
			set
			{
				transformManager.AxesOrientationRasmol = value;
				refresh();
			}
			
		}
		internal float CameraDepth
		{
			get
			{
				return transformManager.cameraDepth;
			}
			
			set
			{
				transformManager.CameraDepth = value;
			}
			
		}
		override public System.Drawing.Size ScreenDimension
		{
			set
			{
				// There is a bug in Netscape 4.7*+MacOS 9 when comparing dimension objects
				// so don't try dim1.equals(dim2)
				int height = value.Height;
				int width = value.Width;
				if (StereoMode == JmolConstants.STEREO_DOUBLE)
					width = (width + 1) / 2;
				if (dimScreen.Width == width && dimScreen.Height == height)
					return ;
				dimScreen.Width = width;
				dimScreen.Height = height;
				transformManager.setScreenDimension(width, height);
				transformManager.scaleFitToScreen();
				g3d.setWindowSize(width, height, enableFullSceneAntialiasing);
			}
			
		}
		override public int ScreenWidth
		{
			get
			{
				return dimScreen.Width;
			}
			
		}
		override public int ScreenHeight
		{
			get
			{
				return dimScreen.Height;
			}
			
		}
		internal System.Drawing.Rectangle RectClip
		{
			set
			{
				if (value.IsEmpty)
				{
					rectClip.X = rectClip.Y = 0;
					rectClip.Size = dimScreen;
				}
				else
				{
					SupportClass.RectangleSupport.SetBoundsRectangle(ref rectClip, value);
					// on Linux platform with Sun 1.4.2_02 I am getting a clipping rectangle
					// that is wider than the current window during window resize
					if (rectClip.X < 0)
						rectClip.X = 0;
					if (rectClip.Y < 0)
						rectClip.Y = 0;
					if (rectClip.X + rectClip.Width > dimScreen.Width)
						rectClip.Width = dimScreen.Width - rectClip.X;
					if (rectClip.Y + rectClip.Height > dimScreen.Height)
						rectClip.Height = dimScreen.Height - rectClip.Y;
				}
			}
			
		}
		internal float ScaleAngstromsPerInch
		{
			set
			{
				transformManager.ScaleAngstromsPerInch = value;
			}
			
		}
		override public float VibrationPeriod
		{
			set
			{
				transformManager.VibrationPeriod = value;
			}
			
		}
		internal float VibrationT
		{
			set
			{
				transformManager.VibrationT = value;
			}
			
		}
		internal float VibrationRadians
		{
			get
			{
				return transformManager.vibrationRadians;
			}
			
		}
		internal int SpinX
		{
			get
			{
				return transformManager.spinX;
			}
			
			set
			{
				transformManager.SpinX = value;
			}
			
		}
		internal int SpinY
		{
			get
			{
				return transformManager.spinY;
			}
			
			set
			{
				transformManager.SpinY = value;
			}
			
		}
		internal int SpinZ
		{
			get
			{
				return transformManager.spinZ;
			}
			
			set
			{
				transformManager.SpinZ = value;
			}
			
		}
		internal int SpinFps
		{
			get
			{
				return transformManager.spinFps;
			}
			
			set
			{
				transformManager.SpinFps = value;
			}
			
		}
		internal bool SpinOn
		{
			get
			{
				return transformManager.spinOn;
			}
			
			set
			{
				transformManager.SpinOn = value;
			}
			
		}
		internal System.String OrientationText
		{
			get
			{
				return transformManager.OrientationText;
			}
			
		}
		internal System.String TransformText
		{
			get
			{
				return transformManager.TransformText;
			}
			
		}
		internal System.String DefaultColors
		{
			/////////////////////////////////////////////////////////////////
			// delegated to ColorManager
			/////////////////////////////////////////////////////////////////
			
			
			set
			{
				colorManager.DefaultColors = value;
			}
			
		}
		public int SelectionArgb
		{
			set
			{
				colorManager.SelectionArgb = value;
				refresh();
			}
			
		}
		internal short ColixSelection
		{
			get
			{
				return colorManager.ColixSelection;
			}
			
		}
		internal int RubberbandArgb
		{
			set
			{
				colorManager.RubberbandArgb = value;
			}
			
		}
		internal short ColixRubberband
		{
			get
			{
				return colorManager.colixRubberband;
			}
			
		}
		override public System.String ColorBackground
		{
			set
			{
				colorManager.ColorBackground = value;
				refresh();
			}
			
		}
		internal short ColixBackgroundContrast
		{
			get
			{
				return colorManager.colixBackgroundContrast;
			}
			
		}
		internal bool Specular
		{
			get
			{
				return colorManager.Specular;
			}
			
			set
			{
				colorManager.Specular = value;
			}
			
		}
		internal int SpecularPower
		{
			set
			{
				colorManager.SpecularPower = value;
			}
			
		}
		internal int AmbientPercent
		{
			set
			{
				colorManager.AmbientPercent = value;
			}
			
		}
		internal int DiffusePercent
		{
			set
			{
				colorManager.DiffusePercent = value;
			}
			
		}
		internal int SpecularPercent
		{
			set
			{
				colorManager.SpecularPercent = value;
			}
			
		}
		internal float LightsourceZ
		{
			// x & y light source coordinates are fixed at -1,-1
			// z should be in the range 0, +/- 3 ?
			
			set
			{
				colorManager.LightsourceZ = value;
			}
			
		}
		internal int Selection
		{
			set
			{
				selectionManager.Selection = value;
				refresh();
			}
			
		}
		internal bool BondSelectionModeOr
		{
			get
			{
				return bondSelectionModeOr;
			}
			
			set
			{
				this.bondSelectionModeOr = value;
				refresh();
			}
			
		}
		override public System.Collections.BitArray SelectionSet
		{
			get
			{
				return selectionManager.bsSelection;
			}
			
			set
			{
				selectionManager.SelectionSet = value;
				refresh();
			}
			
		}
		internal int SelectionCount
		{
			get
			{
				return selectionManager.SelectionCount;
			}
			
		}
		override public int ModeMouse
		{
			/////////////////////////////////////////////////////////////////
			// delegated to MouseManager
			/////////////////////////////////////////////////////////////////
			
			
			set
			{
				// deprecated
			}
			
		}
		internal System.Drawing.Rectangle RubberBandSelection
		{
			get
			{
				return mouseManager.RubberBand;
			}
			
		}
		internal int CursorX
		{
			get
			{
				return mouseManager.xCurrent;
			}
			
		}
		internal int CursorY
		{
			get
			{
				return mouseManager.yCurrent;
			}
			
		}
		override public System.String OpenFileError
		{
			get
			{
				System.String errorMsg = OpenFileError1;
				return errorMsg;
			}
			
		}
		internal System.String OpenFileError1
		{
			get
			{
				System.String fullPathName = fileManager.FullPathName;
				System.String fileName = fileManager.FileName;
				System.Object clientFile = fileManager.waitForClientFileOrErrorMessage();
				if (clientFile is System.String || clientFile == null)
				{
					System.String errorMsg = (System.String) clientFile;
					notifyFileNotLoaded(fullPathName, errorMsg);
					return errorMsg;
				}
				openClientFile(fullPathName, fileName, clientFile);
				notifyFileLoaded(fullPathName, fileName, modelManager.ModelSetName, clientFile);
				return null;
			}
			
		}
		internal System.String CurrentFileAsString
		{
			get
			{
				System.String pathName = modelManager.ModelSetPathName;
				if (pathName == null)
					return null;
				return fileManager.getFileAsString(pathName);
			}
			
		}
		override public System.String ModelSetName
		{
			get
			{
				return modelManager.ModelSetName;
			}
			
		}
		override public System.String ModelSetFileName
		{
			get
			{
				return modelManager.ModelSetFileName;
			}
			
		}
		override public System.String ModelSetPathName
		{
			get
			{
				return modelManager.ModelSetPathName;
			}
			
		}
		internal System.String ModelSetTypeName
		{
			get
			{
				return modelManager.ModelSetTypeName;
			}
			
		}
		internal System.Object ClientFile
		{
			get
			{
				// DEPRECATED - use getExportJmolAdapter()
				return null;
			}
			
		}
		/// <summary>*************************************************************
		/// This is the method that should be used to extract the model
		/// data from Jmol.
		/// Note that the API provided by JmolAdapter is used to
		/// import data into Jmol and to export data out of Jmol.
		/// 
		/// When exporting, a few of the methods in JmolAdapter do
		/// not make sense.
		/// openBufferedReader(...)
		/// Others may be implemented in the future, but are not currently
		/// all pdb specific things
		/// Just pass in null for the methods that want a clientFile.
		/// The main methods to use are
		/// getFrameCount(null) -> currently always returns 1
		/// getAtomCount(null, 0)
		/// getAtomIterator(null, 0)
		/// getBondIterator(null, 0)
		/// 
		/// The AtomIterator and BondIterator return Objects as unique IDs
		/// to identify the atoms.
		/// atomIterator.getAtomUid()
		/// bondIterator.getAtomUid1() & bondIterator.getAtomUid2()
		/// The ExportJmolAdapter will return the 0-based atom index as
		/// a boxed Integer. That means that you can cast the results to get
		/// a zero-based atom index
		/// int atomIndex = ((Integer)atomIterator.getAtomUid()).intValue();
		/// ...
		/// int bondedAtom1 = ((Integer)bondIterator.getAtomUid1()).intValue();
		/// int bondedAtom2 = ((Integer)bondIterator.getAtomUid2()).intValue();
		/// 
		/// post questions to jmol-developers@lists.sf.net
		/// </summary>
		/// <returns> A JmolAdapter
		/// **************************************************************
		/// </returns>
		internal JmolAdapter ExportJmolAdapter
		{
			
			
			get
			{
				return modelManager.ExportJmolAdapter;
			}
			
		}
		internal Frame Frame
		{
			get
			{
				return modelManager.Frame;
			}
			
		}
		override public float RotationRadius
		{
			get
			{
				return modelManager.RotationRadius;
			}
			
		}
		internal Point3f RotationCenter
		{
			get
			{
				return modelManager.getRotationCenter();
			}
			
		}
		internal Point3f DefaultRotationCenter
		{
			get
			{
				return modelManager.DefaultRotationCenter;
			}
			
		}
		internal Point3f BoundBoxCenter
		{
			get
			{
				return modelManager.BoundBoxCenter;
			}
			
		}
		internal Vector3f BoundBoxCornerVector
		{
			get
			{
				return modelManager.BoundBoxCornerVector;
			}
			
		}
		internal int BoundBoxCenterX
		{
			get
			{
				// FIXME mth 2003 05 31
				// used by the labelRenderer for rendering labels away from the center
				// for now this is returning the center of the screen
				// need to transform the center of the bounding box and return that point
				return dimScreen.Width / 2;
			}
			
		}
		internal int BoundBoxCenterY
		{
			get
			{
				return dimScreen.Height / 2;
			}
			
		}
		override public int ModelCount
		{
			get
			{
				return modelManager.ModelCount;
			}
			
		}
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		override public System.Collections.Specialized.NameValueCollection ModelSetProperties
		{
			get
			{
				return modelManager.ModelSetProperties;
			}
			
		}
		override public int ChainCount
		{
			get
			{
				return modelManager.ChainCount;
			}
			
		}
		override public int GroupCount
		{
			get
			{
				return modelManager.GroupCount;
			}
			
		}
		override public int PolymerCount
		{
			get
			{
				return modelManager.PolymerCount;
			}
			
		}
		override public int AtomCount
		{
			get
			{
				return modelManager.AtomCount;
			}
			
		}
		override public int BondCount
		{
			get
			{
				return modelManager.BondCount;
			}
			
		}
		internal System.Collections.BitArray CenterBitSet
		{
			set
			{
				modelManager.CenterBitSet = value;
				if (windowCenteredFlag)
					scaleFitToScreen();
				refresh();
			}
			
		}
		public int CenterPicked
		{
			set
			{
				setCenter(modelManager.getAtomPoint3f(value));
				
				/*
				* This method is called exclusively by PickingManager when the user
				* clicks on an atom and we have "set picking center"
				* 
				* Formerly, PickingManager went directly to Viewer.setCenter; the
				* inclusion of setCenterPicked() allows for more flexibility in future
				* development. 
				* 
				* In Bob's opinion, the above is a bug. We have two different results for
				* 
				* set picking center
				* [user clicks on an atom]
				* 
				* and 
				* 
				* center (atom expression)
				* 
				* In the clicking case, we are going through setCenter(Point3f) 
				* and disregarding any setting of the "frieda/windowCentered switch" -- 
				* whether the clicked atom jumps to the window center 
				* (set windowCentered ON) or remains in place (set windowCentered OFF).
				* 
				* In the scripted case, we are going through setCenterBitset(), which
				* considers the windowCentered state and possibly rescales.
				* 
				* Basically, as it currently stands, "set picking center" and
				* "set windowCentered OFF" (the Frieda switch) are incompatible. 
				* This should not be the case. 
				*  
				* A further undesirable programming aspect is that windowCenteredFlag
				* is being checked first in Viewer.setCenterBitSet() and then again in
				* ModelManager.setCenterBitSet(). This seems inappropriate to Bob. In
				* Bob's opinion, all checking of the windowCenteredFlag should be in one 
				* method, namely ModelManager.setCenterBitSet().   
				* 
				* The issue is somewhat complicated in that Viewer.setCenterBitSet()
				* is also called indirectly by DefineCenterAction events in the App.
				* 
				* To be correct, Bob thinks, Viewer.setCenter() should ONLY be passed
				* through by a call to Viewer.homePosition(), which implements the 
				* equivalent of a scripted "reset" in various contexts.
				* 
				* To fix the Frieda/windowCentered incompatibility issue, one would
				* disable the above line and substitute the three lines below, so that
				* all user-directed and scripted centering is going through 
				* Viewer.setCenterBitSet().
				* 
				* These notes are meant solely as a guide to development and should be
				* removed when the issues relating to them are resolved.
				* 
				*  Bob Hanson 4/06
				*  
				BitSet bsCenter = new BitSet();
				bsCenter.set(atomIndex);
				setCenterBitSet(bsCenter);
				*/
			}
			
		}
		internal bool WindowCentered
		{
			get
			{
				return windowCenteredFlag;
			}
			
			set
			{
				windowCenteredFlag = value;
			}
			
		}
		internal float BondTolerance
		{
			get
			{
				return modelManager.bondTolerance;
			}
			
			set
			{
				modelManager.BondTolerance = value;
				refresh();
			}
			
		}
		internal float MinBondDistance
		{
			get
			{
				return modelManager.minBondDistance;
			}
			
			set
			{
				modelManager.MinBondDistance = value;
				refresh();
			}
			
		}
		override public bool AutoBond
		{
			get
			{
				return modelManager.autoBond;
			}
			
			set
			{
				modelManager.AutoBond = value;
				refresh();
			}
			
		}
		internal float SolventProbeRadius
		{
			get
			{
				return modelManager.solventProbeRadius;
			}
			
			set
			{
				modelManager.SolventProbeRadius = value;
			}
			
		}
		internal float CurrentSolventProbeRadius
		{
			get
			{
				return modelManager.solventOn?modelManager.solventProbeRadius:0;
			}
			
		}
		internal bool SolventOn
		{
			get
			{
				return modelManager.solventOn;
			}
			
			set
			{
				modelManager.SolventOn = value;
			}
			
		}
		override public System.Collections.BitArray ElementsPresentBitSet
		{
			get
			{
				return modelManager.ElementsPresentBitSet;
			}
			
		}
		override public System.Collections.BitArray GroupsPresentBitSet
		{
			get
			{
				return modelManager.GroupsPresentBitSet;
			}
			
		}
		override public int MeasurementCount
		{
			get
			{
				int count = getShapePropertyAsInt(JmolConstants.SHAPE_MEASURES, "count");
				return count <= 0?0:count;
			}
			
		}
		internal int[] PendingMeasurement
		{
			set
			{
				setShapeProperty(JmolConstants.SHAPE_MEASURES, "pending", value);
			}
			
		}
		internal int AnimationDirection
		{
			get
			{
				return repaintManager.animationDirection;
			}
			
			/////////////////////////////////////////////////////////////////
			// delegated to RepaintManager
			/////////////////////////////////////////////////////////////////
			
			
			set
			{
				// 1 or -1
				repaintManager.AnimationDirection = value;
			}
			
		}
		override public int AnimationFps
		{
			get
			{
				return repaintManager.animationFps;
			}
			
			set
			{
				repaintManager.AnimationFps = value;
			}
			
		}
		internal FrameRenderer FrameRenderer
		{
			get
			{
				return repaintManager.frameRenderer;
			}
			
		}
		override public int MotionEventNumber
		{
			get
			{
				return motionEventNumber;
			}
			
		}
		internal bool InMotion
		{
			get
			{
				return repaintManager.inMotion;
			}
			
			set
			{
				//System.out.println("viewer.setInMotion("+inMotion+")");
				if (wasInMotion ^ value)
				{
					if (value)
						++motionEventNumber;
					repaintManager.InMotion = value;
					wasInMotion = value;
				}
			}
			
		}
		override public System.Drawing.Image ScreenImage
		{
			get
			{
				bool antialiasThisFrame = true;
				System.Drawing.Rectangle tempAux = System.Drawing.Rectangle.Empty;
				RectClip = tempAux;
				g3d.beginRendering(rectClip.X, rectClip.Y, rectClip.Width, rectClip.Height, transformManager.getStereoRotationMatrix(false), antialiasThisFrame);
				//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
				repaintManager.render(g3d, ref rectClip, modelManager.Frame, repaintManager.displayModelIndex);
				g3d.endRendering();
				return g3d.ScreenImage;
			}
			
		}
		internal Eval Eval
		{
			/////////////////////////////////////////////////////////////////
			// routines for script support
			/////////////////////////////////////////////////////////////////
			
			
			get
			{
				if (eval == null)
					eval = new Eval(this);
				return eval;
			}
			
		}
		override public bool ScriptExecuting
		{
			get
			{
				return eval.ScriptExecuting;
			}
			
		}
		internal bool ChainCaseSensitive
		{
			get
			{
				return chainCaseSensitive;
			}
			
			set
			{
				this.chainCaseSensitive = value;
			}
			
		}
		internal bool RibbonBorder
		{
			get
			{
				return ribbonBorder;
			}
			
			set
			{
				this.ribbonBorder = value;
			}
			
		}
		internal bool HideNameInPopup
		{
			get
			{
				return hideNameInPopup;
			}
			
			set
			{
				this.hideNameInPopup = value;
			}
			
		}
		internal bool SsbondsBackbone
		{
			get
			{
				return styleManager.ssbondsBackbone;
			}
			
			set
			{
				styleManager.SsbondsBackbone = value;
			}
			
		}
		internal bool HbondsBackbone
		{
			get
			{
				return styleManager.hbondsBackbone;
			}
			
			set
			{
				styleManager.HbondsBackbone = value;
			}
			
		}
		internal bool HbondsSolid
		{
			get
			{
				return styleManager.hbondsSolid;
			}
			
			set
			{
				styleManager.HbondsSolid = value;
			}
			
		}
		override public short MarBond
		{
			set
			{
				styleManager.MarBond = value;
				setShapeSize(JmolConstants.SHAPE_STICKS, value * 2);
			}
			
		}
		internal System.String Label
		{
			set
			{
				if (value != null)
				// force the class to load and display
					setShapeSize(JmolConstants.SHAPE_LABELS, styleManager.pointsLabelFontSize);
				setShapeProperty(JmolConstants.SHAPE_LABELS, "label", value);
			}
			
		}
		internal System.Collections.BitArray BitSetSelection
		{
			get
			{
				return selectionManager.bsSelection;
			}
			
		}
		internal bool RasmolHydrogenSetting
		{
			get
			{
				return rasmolHydrogenSetting;
			}
			
			set
			{
				rasmolHydrogenSetting = value;
			}
			
		}
		internal bool RasmolHeteroSetting
		{
			get
			{
				return rasmolHeteroSetting;
			}
			
			set
			{
				rasmolHeteroSetting = value;
			}
			
		}
		override public JmolStatusListener JmolStatusListener
		{
			set
			{
				this.jmolStatusListener = value;
			}
			
		}
		internal int PickingMode
		{
			set
			{
				pickingManager.PickingMode = value;
			}
			
		}
		internal bool TestFlag1
		{
			get
			{
				return testFlag1;
			}
			
			set
			{
				testFlag1 = value;
			}
			
		}
		internal bool TestFlag2
		{
			get
			{
				return testFlag2;
			}
			
			set
			{
				testFlag2 = value;
			}
			
		}
		internal bool TestFlag3
		{
			get
			{
				return testFlag3;
			}
			
			set
			{
				testFlag3 = value;
			}
			
		}
		internal bool TestFlag4
		{
			get
			{
				return testFlag4;
			}
			
			set
			{
				testFlag4 = value;
			}
			
		}
		internal bool GreyscaleRendering
		{
			get
			{
				return greyscaleRendering;
			}
			
			set
			{
				this.greyscaleRendering = value;
				g3d.GreyscaleMode = value;
				refresh();
			}
			
		}
		internal bool DisablePopupMenu
		{
			get
			{
				return disablePopupMenu;
			}
			
			set
			{
				this.disablePopupMenu = value;
			}
			
		}
		override public int PercentVdwAtom
		{
			get
			{
				return styleManager.percentVdwAtom;
			}
			
			/*
			* for rasmol compatibility with continued menu operation:
			*  - if it is from the menu & nothing selected
			*    * set the setting
			*    * apply to all
			*  - if it is from the menu and something is selected
			*    * apply to selection
			*  - if it is from a script
			*    * apply to selection
			*    * possibly set the setting for some things
			*/
			
			
			set
			{
				styleManager.PercentVdwAtom = value;
				setShapeSize(JmolConstants.SHAPE_BALLS, - value);
			}
			
		}
		internal short MadAtom
		{
			get
			{
				return (short) (- styleManager.percentVdwAtom);
			}
			
		}
		override public short MadBond
		{
			get
			{
				return (short) (styleManager.marBond * 2);
			}
			
		}
		internal sbyte ModeMultipleBond
		{
			get
			{
				return styleManager.modeMultipleBond;
			}
			
			set
			{
				styleManager.ModeMultipleBond = value;
				refresh();
			}
			
		}
		internal bool ShowMultipleBonds
		{
			get
			{
				return styleManager.showMultipleBonds;
			}
			
			set
			{
				styleManager.ShowMultipleBonds = value;
				refresh();
			}
			
		}
		override public bool ShowHydrogens
		{
			get
			{
				return styleManager.showHydrogens;
			}
			
			set
			{
				styleManager.ShowHydrogens = value;
				refresh();
			}
			
		}
		override public bool ShowBbcage
		{
			get
			{
				return getShapeShow(JmolConstants.SHAPE_BBCAGE);
			}
			
			set
			{
				setShapeShow(JmolConstants.SHAPE_BBCAGE, value);
			}
			
		}
		override public bool ShowAxes
		{
			get
			{
				return getShapeShow(JmolConstants.SHAPE_AXES);
			}
			
			set
			{
				setShapeShow(JmolConstants.SHAPE_AXES, value);
			}
			
		}
		override public bool ShowMeasurements
		{
			get
			{
				return styleManager.showMeasurements;
			}
			
			set
			{
				styleManager.ShowMeasurements = value;
				refresh();
			}
			
		}
		internal bool ShowMeasurementLabels
		{
			get
			{
				return styleManager.showMeasurementLabels;
			}
			
			set
			{
				styleManager.ShowMeasurementLabels = value;
				refresh();
			}
			
		}
		internal bool ZeroBasedXyzRasmol
		{
			get
			{
				return styleManager.zeroBasedXyzRasmol;
			}
			
			set
			{
				styleManager.ZeroBasedXyzRasmol = value;
			}
			
		}
		internal int LabelFontSize
		{
			set
			{
				styleManager.LabelFontSize = value;
				refresh();
			}
			
		}
		internal int LabelOffsetX
		{
			get
			{
				return styleManager.labelOffsetX;
			}
			
		}
		internal int LabelOffsetY
		{
			get
			{
				return styleManager.labelOffsetY;
			}
			
		}
		internal int StereoMode
		{
			get
			{
				return transformManager.stereoMode;
			}
			
			////////////////////////////////////////////////////////////////
			// stereo support
			////////////////////////////////////////////////////////////////
			
			
			set
			{
				transformManager.StereoMode = value;
			}
			
		}
		internal float StereoDegrees
		{
			get
			{
				return transformManager.stereoDegrees;
			}
			
			set
			{
				transformManager.StereoDegrees = value;
			}
			
		}
		override public bool Jvm12orGreater
		{
			////////////////////////////////////////////////////////////////
			//
			////////////////////////////////////////////////////////////////
			
			
			get
			{
				return jvm12orGreater;
			}
			
		}
		override public System.String OperatingSystemName
		{
			get
			{
				return strOSName;
			}
			
		}
		override public System.String JavaVendor
		{
			get
			{
				return strJavaVendor;
			}
			
		}
		override public System.String JavaVersion
		{
			get
			{
				return strJavaVersion;
			}
			
		}
		internal Graphics3D Graphics3D
		{
			get
			{
				return g3d;
			}
			
		}
		
		internal System.Windows.Forms.Control awtComponent;
		internal ColorManager colorManager;
		internal TransformManager transformManager;
		internal SelectionManager selectionManager;
		internal MouseManager mouseManager;
		internal FileManager fileManager;
		internal ModelManager modelManager;
		internal RepaintManager repaintManager;
		internal StyleManager styleManager;
		internal TempManager tempManager;
		internal PickingManager pickingManager;
		internal Eval eval;
		internal Graphics3D g3d;
		
		internal JmolAdapter modelAdapter;
		
		internal System.String strJavaVendor;
		internal System.String strJavaVersion;
		internal System.String strOSName;
		internal bool jvm11orGreater = false;
		internal bool jvm12orGreater = false;
		internal bool jvm14orGreater = false;
		
		internal JmolStatusListener jmolStatusListener;
		
		internal Viewer(System.Windows.Forms.Control awtComponent, JmolAdapter modelAdapter)
		{
			InitBlock();
			
			this.awtComponent = awtComponent;
			this.modelAdapter = modelAdapter;
			
			//UPGRADE_ISSUE: Method 'java.lang.System.getProperty' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangSystem'"
			strJavaVendor = System_Renamed.getProperty("java.vendor");
			//UPGRADE_TODO: Method 'java.lang.System.getProperty' was converted to 'System.Environment.GetEnvironmentVariable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangSystemgetProperty_javalangString'"
			strOSName = System.Environment.GetEnvironmentVariable("OS");
			//UPGRADE_ISSUE: Method 'java.lang.System.getProperty' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangSystem'"
			strJavaVersion = System_Renamed.getProperty("java.version");
			jvm11orGreater = (String.CompareOrdinal(strJavaVersion, "1.1") >= 0 && !(strJavaVendor.StartsWith("Netscape") && String.CompareOrdinal(strJavaVersion, "1.1.5") <= 0 && "Mac OS".Equals(strOSName)));
			jvm12orGreater = (String.CompareOrdinal(strJavaVersion, "1.2") >= 0);
			jvm14orGreater = (String.CompareOrdinal(strJavaVersion, "1.4") >= 0);
			
			System.Console.Out.WriteLine(JmolConstants.copyright + "\nJmol Version " + JmolConstants.version + "  " + JmolConstants.date + "\njava.vendor:" + strJavaVendor + "\njava.version:" + strJavaVersion + "\nos.name:" + strOSName);
			
			g3d = new Graphics3D(awtComponent);
			colorManager = new ColorManager(this, g3d);
			transformManager = new TransformManager(this);
			selectionManager = new SelectionManager(this);
			if (jvm14orGreater)
				mouseManager = MouseWrapper14.alloc(awtComponent, this);
			else if (jvm11orGreater)
				mouseManager = MouseWrapper11.alloc(awtComponent, this);
			else
				mouseManager = new MouseManager10(awtComponent, this);
			fileManager = new FileManager(this, modelAdapter);
			repaintManager = new RepaintManager(this);
			modelManager = new ModelManager(this, modelAdapter);
			styleManager = new StyleManager(this);
			tempManager = new TempManager(this);
			pickingManager = new PickingManager(this);
		}
		
		public static new JmolViewer allocateViewer(System.Windows.Forms.Control awtComponent, JmolAdapter modelAdapter)
		{
			return new Viewer(awtComponent, modelAdapter);
		}
		
		//UPGRADE_ISSUE: Class 'java.awt.Event' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
		public override bool handleOldJvm10Event(Event e)
		{
			return mouseManager.handleOldJvm10Event(e);
		}
		
		public override void  homePosition()
		{
			setCenter(null);
			transformManager.homePosition();
			refresh();
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'imageCache '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal System.Collections.Hashtable imageCache = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
		internal void  flushCachedImages()
		{
			imageCache.Clear();
			colorManager.flushCachedColors();
		}
		
		internal void  logError(System.String strMsg)
		{
			System.Console.Out.WriteLine(strMsg);
		}
		
		/////////////////////////////////////////////////////////////////
		// delegated to TransformManager
		/////////////////////////////////////////////////////////////////
		
		internal void  rotateXYBy(int xDelta, int yDelta)
		{
			transformManager.rotateXYBy(xDelta, yDelta);
			refresh();
		}
		
		internal void  rotateZBy(int zDelta)
		{
			transformManager.rotateZBy(zDelta);
			refresh();
		}
		
		public override void  rotateFront()
		{
			transformManager.rotateFront();
			refresh();
		}
		
		public override void  rotateToX(float angleRadians)
		{
			transformManager.rotateToX(angleRadians);
			refresh();
		}
		public override void  rotateToY(float angleRadians)
		{
			transformManager.rotateToY(angleRadians);
			refresh();
		}
		public override void  rotateToZ(float angleRadians)
		{
			transformManager.rotateToZ(angleRadians);
			refresh();
		}
		
		public override void  rotateToX(int angleDegrees)
		{
			rotateToX(angleDegrees * radiansPerDegree);
		}
		public override void  rotateToY(int angleDegrees)
		{
			rotateToY(angleDegrees * radiansPerDegree);
		}
		//UPGRADE_NOTE: Access modifiers of method 'rotateToZ' were changed to 'public'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1204'"
		public void  rotateToZ(int angleDegrees)
		{
			rotateToZ(angleDegrees * radiansPerDegree);
		}
		
		internal void  rotateXRadians(float angleRadians)
		{
			transformManager.rotateXRadians(angleRadians);
			refresh();
		}
		internal void  rotateYRadians(float angleRadians)
		{
			transformManager.rotateYRadians(angleRadians);
			refresh();
		}
		internal void  rotateZRadians(float angleRadians)
		{
			transformManager.rotateZRadians(angleRadians);
			refresh();
		}
		internal void  rotateXDegrees(float angleDegrees)
		{
			rotateXRadians(angleDegrees * radiansPerDegree);
		}
		internal void  rotateYDegrees(float angleDegrees)
		{
			rotateYRadians(angleDegrees * radiansPerDegree);
		}
		internal void  rotateZDegrees(float angleDegrees)
		{
			rotateZRadians(angleDegrees * radiansPerDegree);
		}
		internal void  rotateZDegreesScript(float angleDegrees)
		{
			transformManager.rotateZRadiansScript(angleDegrees * radiansPerDegree);
			refresh();
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'radiansPerDegree '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
		internal static readonly float radiansPerDegree = (float) (2 * System.Math.PI / 360);
		//UPGRADE_NOTE: Final was removed from the declaration of 'degreesPerRadian '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
		internal static readonly float degreesPerRadian = (float) (360 / (2 * System.Math.PI));
		
		internal void  rotate(AxisAngle4f axisAngle)
		{
			transformManager.rotate(axisAngle);
			refresh();
		}
		
		internal void  rotateAxisAngle(float x, float y, float z, float degrees)
		{
			transformManager.rotateAxisAngle(x, y, z, degrees);
		}
		
		internal void  rotateTo(float xAxis, float yAxis, float zAxis, float degrees)
		{
			transformManager.rotateTo(xAxis, yAxis, zAxis, degrees);
		}
		
		internal void  rotateTo(AxisAngle4f axisAngle)
		{
			transformManager.rotateTo(axisAngle);
		}
		
		internal void  translateXYBy(int xDelta, int yDelta)
		{
			transformManager.translateXYBy(xDelta, yDelta);
			refresh();
		}
		
		internal void  translateToXPercent(float percent)
		{
			transformManager.translateToXPercent(percent);
			refresh();
		}
		
		internal void  translateToYPercent(float percent)
		{
			transformManager.translateToYPercent(percent);
			refresh();
		}
		
		internal void  translateToZPercent(float percent)
		{
			transformManager.translateToZPercent(percent);
			refresh();
		}
		
		internal void  translateByXPercent(float percent)
		{
			translateToXPercent(TranslationXPercent + percent);
		}
		
		internal void  translateByYPercent(float percent)
		{
			translateToYPercent(TranslationYPercent + percent);
		}
		
		internal void  translateByZPercent(float percent)
		{
			translateToZPercent(TranslationZPercent + percent);
		}
		
		internal void  translateCenterTo(int x, int y)
		{
			transformManager.translateCenterTo(x, y);
		}
		
		internal void  zoomBy(int pixels)
		{
			transformManager.zoomBy(pixels);
			refresh();
		}
		
		public const int MAXIMUM_ZOOM_PERCENTAGE = 20000;
		/*
		* OK, I give. We have a real limitation with perspective depth.
		* Zoom is back to where it was.  
		* When it is on and we go very far past this in zoom, we can see some 
		* nasty rendering issues. I believe this is because we are hitting
		* a point where z*z > int.MAX_VALUE, but I can't be sure. I believe
		* that means that the real limit for z is a short.   
		* 
		* These notes are meant solely as a guide to development and should be
		* removed when the issues relating to them are resolved.
		* 
		*  Bob Hanson 4/06
		*  
		*/
		
		internal void  zoomToPercent(int percent)
		{
			transformManager.zoomToPercent(percent);
			refresh();
		}
		
		internal void  zoomByPercent(int percent)
		{
			transformManager.zoomByPercent(percent);
			refresh();
		}
		
		internal void  slabByPixels(int pixels)
		{
			transformManager.slabByPercentagePoints(pixels);
			refresh();
		}
		
		internal void  depthByPixels(int pixels)
		{
			transformManager.depthByPercentagePoints(pixels);
			refresh();
		}
		
		internal void  slabDepthByPixels(int pixels)
		{
			transformManager.slabDepthByPercentagePoints(pixels);
			refresh();
		}
		
		internal void  slabToPercent(int percentSlab)
		{
			transformManager.slabToPercent(percentSlab);
			refresh();
		}
		
		internal void  depthToPercent(int percentDepth)
		{
			transformManager.depthToPercent(percentDepth);
			refresh();
		}
		
		internal void  calcTransformMatrices()
		{
			transformManager.calcTransformMatrices();
		}
		
		internal Point3i transformPoint(Point3f pointAngstroms)
		{
			return transformManager.transformPoint(pointAngstroms);
		}
		
		internal Point3i transformPoint(Point3f pointAngstroms, Vector3f vibrationVector)
		{
			return transformManager.transformPoint(pointAngstroms, vibrationVector);
		}
		
		internal void  transformPoint(Point3f pointAngstroms, Vector3f vibrationVector, Point3i pointScreen)
		{
			transformManager.transformPoint(pointAngstroms, vibrationVector, pointScreen);
		}
		
		internal void  transformPoint(Point3f pointAngstroms, Point3i pointScreen)
		{
			transformManager.transformPoint(pointAngstroms, pointScreen);
		}
		
		internal void  transformPoint(Point3f pointAngstroms, Point3f pointScreen)
		{
			transformManager.transformPoint(pointAngstroms, pointScreen);
		}
		
		internal void  transformPoints(Point3f[] pointsAngstroms, Point3i[] pointsScreens)
		{
			transformManager.transformPoints(pointsAngstroms.length, pointsAngstroms, pointsScreens);
		}
		
		internal void  transformVector(Vector3f vectorAngstroms, Vector3f vectorTransformed)
		{
			transformManager.transformVector(vectorAngstroms, vectorTransformed);
		}
		
		internal float scaleToScreen(int z, float sizeAngstroms)
		{
			return transformManager.scaleToScreen(z, sizeAngstroms);
		}
		
		internal short scaleToScreen(int z, int milliAngstroms)
		{
			return transformManager.scaleToScreen(z, milliAngstroms);
		}
		
		internal float scaleToPerspective(int z, float sizeAngstroms)
		{
			return transformManager.scaleToPerspective(z, sizeAngstroms);
		}
		
		internal void  scaleFitToScreen()
		{
			transformManager.scaleFitToScreen();
		}
		
		internal void  checkCameraDistance()
		{
			if (transformManager.increaseRotationRadius)
				modelManager.increaseRotationRadius(transformManager.RotationRadiusIncrease);
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'dimScreen '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal System.Drawing.Size dimScreen = new System.Drawing.Size(0, 0);
		//UPGRADE_NOTE: Final was removed from the declaration of 'rectClip '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal System.Drawing.Rectangle rectClip = new System.Drawing.Rectangle();
		
		internal bool enableFullSceneAntialiasing = false;
		
		internal void  setSlabAndDepthValues(int slabValue, int depthValue)
		{
			g3d.setSlabAndDepthValues(slabValue, depthValue);
		}
		
		internal void  getAxisAngle(AxisAngle4f axisAngle)
		{
			transformManager.getAxisAngle(axisAngle);
		}
		
		internal void  setRotation(Matrix3f matrixRotation)
		{
			transformManager.setRotation(matrixRotation);
		}
		
		internal void  getRotation(Matrix3f matrixRotation)
		{
			transformManager.getRotation(matrixRotation);
		}
		
		internal int getColixArgb(short colix)
		{
			return g3d.getColixArgb(colix);
		}
		
		internal void  setElementArgb(int elementNumber, int argb)
		{
			colorManager.setElementArgb(elementNumber, argb);
		}
		
		internal float getVectorScale()
		{
			return transformManager.vectorScale;
		}
		
		public override void  setVectorScale(float scale)
		{
			transformManager.VectorScale = scale;
		}
		
		public override void  setVibrationScale(float scale)
		{
			transformManager.VibrationScale = scale;
		}
		
		internal float getVibrationScale()
		{
			return transformManager.vibrationScale;
		}
		
		public void  setBackgroundArgb(int argb)
		{
			colorManager.BackgroundArgb = argb;
			refresh();
		}
		
		public override int getBackgroundArgb()
		{
			return colorManager.argbBackground;
		}
		
		internal int getArgbFromString(System.String colorName)
		{
			return Graphics3D.getArgbFromString(colorName);
		}
		
		internal short getColixAtom(Atom atom)
		{
			return colorManager.getColixAtom(atom);
		}
		
		internal short getColixAtomPalette(Atom atom, System.String palette)
		{
			return colorManager.getColixAtomPalette(atom, palette);
		}
		
		internal short getColixHbondType(short order)
		{
			return colorManager.getColixHbondType(order);
		}
		
		internal short getColixFromPalette(float val, float rangeMin, float rangeMax, System.String palette)
		{
			return colorManager.getColixFromPalette(val, rangeMin, rangeMax, palette);
		}
		
		/////////////////////////////////////////////////////////////////
		// delegated to SelectionManager
		/////////////////////////////////////////////////////////////////
		
		internal void  addSelection(int atomIndex)
		{
			selectionManager.addSelection(atomIndex);
			refresh();
		}
		
		internal void  addSelection(System.Collections.BitArray set_Renamed)
		{
			selectionManager.addSelection(set_Renamed);
			refresh();
		}
		
		internal void  toggleSelection(int atomIndex)
		{
			selectionManager.toggleSelection(atomIndex);
			refresh();
		}
		
		internal bool isSelected(int atomIndex)
		{
			return selectionManager.isSelected(atomIndex);
		}
		
		internal bool hasSelectionHalo(int atomIndex)
		{
			return selectionHaloEnabled && selectionManager.isSelected(atomIndex);
		}
		
		internal bool selectionHaloEnabled = false;
		public override void  setSelectionHaloEnabled(bool selectionHaloEnabled)
		{
			if (this.selectionHaloEnabled != selectionHaloEnabled)
			{
				this.selectionHaloEnabled = selectionHaloEnabled;
				refresh();
			}
		}
		
		internal bool getSelectionHaloEnabled()
		{
			return selectionHaloEnabled;
		}
		
		private bool bondSelectionModeOr;
		
		public override void  selectAll()
		{
			selectionManager.selectAll();
			refresh();
		}
		
		public override void  clearSelection()
		{
			selectionManager.clearSelection();
			refresh();
		}
		
		internal void  toggleSelectionSet(System.Collections.BitArray set_Renamed)
		{
			selectionManager.toggleSelectionSet(set_Renamed);
			refresh();
		}
		
		internal void  invertSelection()
		{
			selectionManager.invertSelection();
			// only used from a script, so I do not think a refresh() is necessary
		}
		
		internal void  excludeSelectionSet(System.Collections.BitArray set_Renamed)
		{
			selectionManager.excludeSelectionSet(set_Renamed);
			// only used from a script, so I do not think a refresh() is necessary
		}
		
		public override void  addSelectionListener(JmolSelectionListener listener)
		{
			selectionManager.addListener(listener);
		}
		
		public override void  removeSelectionListener(JmolSelectionListener listener)
		{
			selectionManager.addListener(listener);
		}
		
		internal void  popupMenu(int x, int y)
		{
			if (!disablePopupMenu && jmolStatusListener != null)
				jmolStatusListener.handlePopupMenu(x, y);
		}
		
		/////////////////////////////////////////////////////////////////
		// delegated to FileManager
		/////////////////////////////////////////////////////////////////
		
		public override void  setAppletContext(System.Uri documentBase, System.Uri codeBase, System.String appletProxy)
		{
			fileManager.setAppletContext(documentBase, codeBase, appletProxy);
		}
		
		internal System.Object getInputStreamOrErrorMessageFromName(System.String name)
		{
			return fileManager.getInputStreamOrErrorMessageFromName(name);
		}
		
		internal System.Object getUnzippedBufferedReaderOrErrorMessageFromName(System.String name)
		{
			return fileManager.getUnzippedBufferedReaderOrErrorMessageFromName(name);
		}
		
		public override void  openFile(System.String name)
		{
			/*
			System.out.println("openFile(" + name + ") thread:" + Thread.currentThread() +
			" priority:" + Thread.currentThread().getPriority());
			*/
			clear();
			// keep old screen image while new file is being loaded
			//    forceRefresh();
			long timeBegin = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
			fileManager.openFile(name);
			long ms = (System.DateTime.Now.Ticks - 621355968000000000) / 10000 - timeBegin;
			System.Console.Out.WriteLine("openFile(" + name + ") " + ms + " ms");
		}
		
		public void  openFiles(System.String modelName, System.String[] names)
		{
			clear();
			// keep old screen image while new file is being loaded
			//    forceRefresh();
			long timeBegin = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
			fileManager.openFiles(modelName, names);
			long ms = (System.DateTime.Now.Ticks - 621355968000000000) / 10000 - timeBegin;
			System.Console.Out.WriteLine("openFiles() " + ms + " ms");
		}
		
		public override void  openStringInline(System.String strModel)
		{
			clear();
			fileManager.openStringInline(strModel);
			System.String generatedAux = OpenFileError;
		}
		
		public override void  openDOM(System.Object DOMNode)
		{
			clear();
			long timeBegin = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
			fileManager.openDOM(DOMNode);
			long ms = (System.DateTime.Now.Ticks - 621355968000000000) / 10000 - timeBegin;
			System.Console.Out.WriteLine("openDOM " + ms + " ms");
			System.String generatedAux4 = OpenFileError;
		}
		
		/// <summary> Opens the file, given the reader.
		/// 
		/// name is a text name of the file ... to be displayed in the window
		/// no need to pass a BufferedReader ...
		/// ... the FileManager will wrap a buffer around it
		/// </summary>
		/// <param name="fullPathName">
		/// </param>
		/// <param name="name">
		/// </param>
		/// <param name="reader">
		/// </param>
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		public override void  openReader(System.String fullPathName, System.String name, System.IO.StreamReader reader)
		{
			clear();
			fileManager.openReader(fullPathName, name, reader);
			System.String generatedAux = OpenFileError;
			System.GC.Collect();
		}
		
		internal System.String getFileAsString(System.String pathName)
		{
			return fileManager.getFileAsString(pathName);
		}
		
		/////////////////////////////////////////////////////////////////
		// delegated to ModelManager
		/////////////////////////////////////////////////////////////////
		
		public override void  openClientFile(System.String fullPathName, System.String fileName, System.Object clientFile)
		{
			// maybe there needs to be a call to clear()
			// or something like that here
			// for when CdkEditBus calls this directly
			pushHoldRepaint();
			modelManager.setClientFile(fullPathName, fileName, clientFile);
			homePosition();
			selectAll();
			if (eval != null)
				eval.clearDefinitionsAndLoadPredefined();
			// there probably needs to be a better startup mechanism for shapes
			if (modelManager.hasVibrationVectors())
				setShapeSize(JmolConstants.SHAPE_VECTORS, 1);
			setFrankOn(styleManager.frankOn);
			
			popHoldRepaint();
		}
		
		internal void  clear()
		{
			repaintManager.clearAnimation();
			transformManager.clearVibration();
			modelManager.setClientFile(null, null, (System.Object) null);
			selectionManager.clearSelection();
			clearMeasurements();
			WindowCentered = true;
			notifyFileLoaded(null, null, null, (System.Object) null);
			refresh();
		}
		
		public override bool haveFrame()
		{
			return modelManager.frame != null;
		}
		
		internal System.String getClientAtomStringProperty(System.Object clientAtomReference, System.String propertyName)
		{
			return modelManager.getClientAtomStringProperty(clientAtomReference, propertyName);
		}
		
		public override int getModelNumber(int modelIndex)
		{
			return modelManager.getModelNumber(modelIndex);
		}
		
		public override System.String getModelName(int modelIndex)
		{
			return modelManager.getModelName(modelIndex);
		}
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		public override System.Collections.Specialized.NameValueCollection getModelProperties(int modelIndex)
		{
			return modelManager.getModelProperties(modelIndex);
		}
		
		public override System.String getModelProperty(int modelIndex, System.String propertyName)
		{
			return modelManager.getModelProperty(modelIndex, propertyName);
		}
		
		internal int getModelNumberIndex(int modelNumber)
		{
			return modelManager.getModelNumberIndex(modelNumber);
		}
		
		internal bool modelSetHasVibrationVectors()
		{
			return modelManager.modelSetHasVibrationVectors();
		}
		
		public override bool modelHasVibrationVectors(int modelIndex)
		{
			return modelManager.modelHasVibrationVectors(modelIndex);
		}
		
		public override int getPolymerCountInModel(int modelIndex)
		{
			return modelManager.getPolymerCountInModel(modelIndex);
		}
		
		internal bool frankClicked(int x, int y)
		{
			return modelManager.frankClicked(x, y);
		}
		
		internal int findNearestAtomIndex(int x, int y)
		{
			return modelManager.findNearestAtomIndex(x, y);
		}
		
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		internal System.Collections.BitArray findAtomsInRectangle(ref System.Drawing.Rectangle rectRubberBand)
		{
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
			return modelManager.findAtomsInRectangle(ref rectRubberBand);
		}
		
		internal void  setCenter(Point3f center)
		{
			modelManager.RotationCenter = center;
			refresh();
		}
		
		internal Point3f getCenter()
		{
			return modelManager.getRotationCenter();
		}
		
		internal void  setCenter(System.String relativeTo, float x, float y, float z)
		{
			modelManager.setRotationCenter(relativeTo, x, y, z);
			scaleFitToScreen();
		}
		
		public override void  setCenterSelected()
		{
			CenterBitSet = selectionManager.bsSelection;
		}
		
		internal bool windowCenteredFlag = true;
		
		internal int getAtomIndexFromAtomNumber(int atomNumber)
		{
			return modelManager.getAtomIndexFromAtomNumber(atomNumber);
		}
		
		internal void  calcSelectedGroupsCount()
		{
			modelManager.calcSelectedGroupsCount(selectionManager.bsSelection);
		}
		
		internal void  calcSelectedMonomersCount()
		{
			modelManager.calcSelectedMonomersCount(selectionManager.bsSelection);
		}
		
		/// <summary>*************************************************************
		/// delegated to MeasurementManager
		/// **************************************************************
		/// </summary>
		
		public override void  clearMeasurements()
		{
			setShapeProperty(JmolConstants.SHAPE_MEASURES, "clear", (System.Object) null);
			refresh();
		}
		
		public override System.String getMeasurementStringValue(int i)
		{
			//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			return "" + getShapeProperty(JmolConstants.SHAPE_MEASURES, "stringValue", i);
		}
		
		public override int[] getMeasurementCountPlusIndices(int i)
		{
			return (int[]) getShapeProperty(JmolConstants.SHAPE_MEASURES, "countPlusIndices", i);
		}
		
		internal void  defineMeasurement(int[] atomCountPlusIndices)
		{
			setShapeProperty(JmolConstants.SHAPE_MEASURES, "define", atomCountPlusIndices);
		}
		
		public override void  deleteMeasurement(int i)
		{
			setShapeProperty(JmolConstants.SHAPE_MEASURES, "delete", (System.Object) i);
		}
		
		internal void  deleteMeasurement(int[] atomCountPlusIndices)
		{
			setShapeProperty(JmolConstants.SHAPE_MEASURES, "delete", atomCountPlusIndices);
		}
		
		internal void  toggleMeasurement(int[] atomCountPlusIndices)
		{
			setShapeProperty(JmolConstants.SHAPE_MEASURES, "toggle", atomCountPlusIndices);
		}
		
		internal void  clearAllMeasurements()
		{
			setShapeProperty(JmolConstants.SHAPE_MEASURES, "clear", (System.Object) null);
		}
		
		internal void  setAnimationReplayMode(int replay, float firstFrameDelay, float lastFrameDelay)
		{
			// 0 means once
			// 1 means loop
			// 2 means palindrome
			repaintManager.setAnimationReplayMode(replay, firstFrameDelay, lastFrameDelay);
		}
		internal int getAnimationReplayMode()
		{
			return repaintManager.animationReplayMode;
		}
		
		internal void  setAnimationOn(bool animationOn)
		{
			bool wasAnimating = repaintManager.animationOn;
			repaintManager.setAnimationOn(animationOn);
			if (animationOn != wasAnimating)
				refresh();
		}
		
		internal void  setAnimationOn(bool animationOn, int framePointer)
		{
			bool wasAnimating = repaintManager.animationOn;
			System.Console.Out.WriteLine(" setAnimationOn " + wasAnimating + " " + animationOn + " " + framePointer);
			repaintManager.setAnimationOn(animationOn, framePointer);
			if (animationOn != wasAnimating)
				refresh();
		}
		
		internal bool isAnimationOn()
		{
			return repaintManager.animationOn;
		}
		
		internal void  setAnimationNext()
		{
			if (repaintManager.setAnimationNext())
				refresh();
		}
		
		internal void  setAnimationPrevious()
		{
			if (repaintManager.setAnimationPrevious())
				refresh();
		}
		
		internal bool setDisplayModelIndex(int modelIndex)
		{
			return repaintManager.setDisplayModelIndex(modelIndex);
		}
		
		public override int getDisplayModelIndex()
		{
			return repaintManager.displayModelIndex;
		}
		
		internal int motionEventNumber;
		
		internal bool wasInMotion = false;
		
		internal System.Drawing.Image takeSnapshot()
		{
			return repaintManager.takeSnapshot();
		}
		
		public override void  pushHoldRepaint()
		{
			repaintManager.pushHoldRepaint();
		}
		
		public override void  popHoldRepaint()
		{
			repaintManager.popHoldRepaint();
		}
		
		internal void  forceRefresh()
		{
			repaintManager.forceRefresh();
		}
		
		public override void  refresh()
		{
			repaintManager.refresh();
		}
		
		internal void  requestRepaintAndWait()
		{
			repaintManager.requestRepaintAndWait();
		}
		
		public override void  notifyRepainted()
		{
			repaintManager.notifyRepainted();
		}
		
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		public override void  renderScreenImage(System.Drawing.Graphics g, ref System.Drawing.Size size, ref System.Drawing.Rectangle clip)
		{
			manageScriptTermination();
			if (!size.IsEmpty)
				ScreenDimension = size;
			int stereoMode = StereoMode;
			switch (stereoMode)
			{
				
				case JmolConstants.STEREO_NONE: 
					RectClip = clip;
					render1(g, transformManager.getStereoRotationMatrix(false), false, 0, 0);
					break;
				
				case JmolConstants.STEREO_DOUBLE: 
					System.Drawing.Rectangle tempAux = System.Drawing.Rectangle.Empty;
					RectClip = tempAux;
					render1(g, transformManager.getStereoRotationMatrix(false), false, 0, 0);
					render1(g, transformManager.getStereoRotationMatrix(true), false, dimScreen.Width, 0);
					break;
				
				case JmolConstants.STEREO_REDCYAN: 
				case JmolConstants.STEREO_REDBLUE: 
				case JmolConstants.STEREO_REDGREEN: 
					System.Drawing.Rectangle tempAux2 = System.Drawing.Rectangle.Empty;
					RectClip = tempAux2;
					g3d.beginRendering(rectClip.X, rectClip.Y, rectClip.Width, rectClip.Height, transformManager.getStereoRotationMatrix(true), false);
					//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
					repaintManager.render(g3d, ref rectClip, modelManager.Frame, repaintManager.displayModelIndex);
					g3d.endRendering();
					g3d.snapshotAnaglyphChannelBytes();
					g3d.beginRendering(rectClip.X, rectClip.Y, rectClip.Width, rectClip.Height, transformManager.getStereoRotationMatrix(false), false);
					//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
					repaintManager.render(g3d, ref rectClip, modelManager.Frame, repaintManager.displayModelIndex);
					g3d.endRendering();
					if (stereoMode == JmolConstants.STEREO_REDCYAN)
						g3d.applyCyanAnaglyph();
					else
						g3d.applyBlueOrGreenAnaglyph(stereoMode == JmolConstants.STEREO_REDBLUE);
					System.Drawing.Image img = g3d.ScreenImage;
					try
					{
						//UPGRADE_WARNING: Method 'java.awt.Graphics.drawImage' was converted to 'System.Drawing.Graphics.drawImage' which may throw an exception. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1101'"
						g.DrawImage(img, 0, 0);
					}
					catch (System.NullReferenceException npe)
					{
						System.Console.Out.WriteLine("Sun!! ... fix graphics your bugs!");
					}
					g3d.releaseScreenImage();
					break;
				}
			notifyRepainted();
		}
		
		internal void  render1(System.Drawing.Graphics g, Matrix3f matrixRotate, bool antialias, int x, int y)
		{
			g3d.beginRendering(rectClip.X, rectClip.Y, rectClip.Width, rectClip.Height, matrixRotate, antialias);
			//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
			repaintManager.render(g3d, ref rectClip, modelManager.Frame, repaintManager.displayModelIndex);
			// mth 2003-01-09 Linux Sun JVM 1.4.2_02
			// Sun is throwing a NullPointerExceptions inside graphics routines
			// while the window is resized. 
			g3d.endRendering();
			System.Drawing.Image img = g3d.ScreenImage;
			try
			{
				//UPGRADE_WARNING: Method 'java.awt.Graphics.drawImage' was converted to 'System.Drawing.Graphics.drawImage' which may throw an exception. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1101'"
				g.DrawImage(img, x, y);
			}
			catch (System.NullReferenceException npe)
			{
				System.Console.Out.WriteLine("Sun!! ... fix graphics your bugs!");
			}
			g3d.releaseScreenImage();
		}
		
		public override void  releaseScreenImage()
		{
			g3d.releaseScreenImage();
		}
		
		public override System.String evalFile(System.String strFilename)
		{
			if (strFilename != null)
			{
				if (!Eval.loadScriptFile(strFilename, false))
					return eval.ErrorMessage;
				eval.start();
			}
			return null;
		}
		
		public override System.String evalString(System.String strScript)
		{
			if (strScript != null)
			{
				if (!Eval.loadScriptString(strScript, false))
					return eval.ErrorMessage;
				eval.start();
			}
			return null;
		}
		
		public override System.String evalStringQuiet(System.String strScript)
		{
			if (strScript != null)
			{
				if (!Eval.loadScriptString(strScript, true))
					return eval.ErrorMessage;
				eval.start();
			}
			return null;
		}
		
		public override void  haltScriptExecution()
		{
			if (eval != null)
				eval.haltExecution();
		}
		
		internal bool chainCaseSensitive = false;
		
		internal bool ribbonBorder = false;
		
		internal bool hideNameInPopup = false;
		
		internal int hoverAtomIndex = - 1;
		internal void  hoverOn(int atomIndex)
		{
			if ((eval == null || !eval.Active) && atomIndex != hoverAtomIndex)
			{
				loadShape(JmolConstants.SHAPE_HOVER);
				setShapeProperty(JmolConstants.SHAPE_HOVER, "target", (System.Object) atomIndex);
				hoverAtomIndex = atomIndex;
			}
		}
		
		internal void  hoverOff()
		{
			if (hoverAtomIndex >= 0)
			{
				setShapeProperty(JmolConstants.SHAPE_HOVER, "target", (System.Object) null);
				hoverAtomIndex = - 1;
			}
		}
		
		internal void  togglePickingLabel(int atomIndex)
		{
			if (atomIndex != - 1)
			{
				// hack to force it to load
				setShapeSize(JmolConstants.SHAPE_LABELS, styleManager.pointsLabelFontSize);
				modelManager.setShapeProperty(JmolConstants.SHAPE_LABELS, "pickingLabel", (System.Object) atomIndex, null);
				refresh();
			}
		}
		
		internal void  setShapeShow(int shapeID, bool show)
		{
			setShapeSize(shapeID, show?- 1:0);
		}
		
		internal bool getShapeShow(int shapeID)
		{
			return getShapeSize(shapeID) != 0;
		}
		
		internal void  loadShape(int shapeID)
		{
			modelManager.loadShape(shapeID);
		}
		
		internal void  setShapeSize(int shapeID, int size)
		{
			modelManager.setShapeSize(shapeID, size, selectionManager.bsSelection);
			refresh();
		}
		
		internal int getShapeSize(int shapeID)
		{
			return modelManager.getShapeSize(shapeID);
		}
		
		internal void  setShapeProperty(int shapeID, System.String propertyName, System.Object value_Renamed)
		{
			
			/*
			System.out.println("JmolViewer.setShapeProperty("+
			JmolConstants.shapeClassBases[shapeID]+
			"," + propertyName + "," + value + ")");
			*/
			modelManager.setShapeProperty(shapeID, propertyName, value_Renamed, selectionManager.bsSelection);
			refresh();
		}
		
		internal void  setShapeProperty(int shapeID, System.String propertyName, int value_Renamed)
		{
			setShapeProperty(shapeID, propertyName, (System.Object) value_Renamed);
		}
		
		internal void  setShapePropertyArgb(int shapeID, System.String propertyName, int argb)
		{
			//UPGRADE_TODO: The 'System.Int32' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
			setShapeProperty(shapeID, propertyName, (System.Object) (argb == 0?null:(System.Int32) (argb | unchecked((int) 0xFF000000))));
		}
		
		internal void  setShapeColorProperty(int shapeType, int argb)
		{
			setShapePropertyArgb(shapeType, "color", argb);
		}
		
		internal System.Object getShapeProperty(int shapeType, System.String propertyName)
		{
			return modelManager.getShapeProperty(shapeType, propertyName, System.Int32.MinValue);
		}
		
		internal System.Object getShapeProperty(int shapeType, System.String propertyName, int index)
		{
			return modelManager.getShapeProperty(shapeType, propertyName, index);
		}
		
		internal int getShapePropertyAsInt(int shapeID, System.String propertyName)
		{
			System.Object value_Renamed = getShapeProperty(shapeID, propertyName);
			return value_Renamed == null || !(value_Renamed is System.Int32)?System.Int32.MinValue:((System.Int32) value_Renamed);
		}
		
		internal int getShapeID(System.String shapeName)
		{
			for (int i = JmolConstants.SHAPE_MAX; --i >= 0; )
				if (JmolConstants.shapeClassBases[i].Equals(shapeName))
					return i;
			System.String msg = "Unrecognized shape name:" + shapeName;
			System.Console.Out.WriteLine(msg);
			throw new System.NullReferenceException(msg);
		}
		
		internal short getColix(System.Object object_Renamed)
		{
			return Graphics3D.getColix(object_Renamed);
		}
		
		internal bool rasmolHydrogenSetting = true;
		
		internal bool rasmolHeteroSetting = true;
		
		internal void  notifyFrameChanged(int frameNo)
		{
			if (jmolStatusListener != null)
				jmolStatusListener.notifyFrameChanged(frameNo);
		}
		
		internal void  notifyFileLoaded(System.String fullPathName, System.String fileName, System.String modelName, System.Object clientFile)
		{
			if (jmolStatusListener != null)
				jmolStatusListener.notifyFileLoaded(fullPathName, fileName, modelName, clientFile, null);
		}
		
		internal void  notifyFileNotLoaded(System.String fullPathName, System.String errorMsg)
		{
			if (jmolStatusListener != null)
				jmolStatusListener.notifyFileLoaded(fullPathName, null, null, (System.Object) null, errorMsg);
		}
		
		private void  manageScriptTermination()
		{
			if (eval != null && eval.hasTerminationNotification())
			{
				System.String strErrorMessage = eval.ErrorMessage;
				int msWalltime = eval.ExecutionWalltime;
				eval.resetTerminationNotification();
				if (jmolStatusListener != null)
					jmolStatusListener.notifyScriptTermination(strErrorMessage, msWalltime);
			}
		}
		
		internal void  scriptEcho(System.String strEcho)
		{
			if (jmolStatusListener != null)
				jmolStatusListener.scriptEcho(strEcho);
		}
		
		internal bool debugScript = false;
		internal bool getDebugScript()
		{
			return debugScript;
		}
		public override void  setDebugScript(bool debugScript)
		{
			this.debugScript = debugScript;
		}
		
		internal void  scriptStatus(System.String strStatus)
		{
			if (jmolStatusListener != null)
				jmolStatusListener.scriptStatus(strStatus);
		}
		
		/*
		void measureSelection(int iatom) {
		if (jmolStatusListener != null)
		jmolStatusListener.measureSelection(iatom);
		}
		*/
		
		internal void  notifyMeasurementsChanged()
		{
			if (jmolStatusListener != null)
				jmolStatusListener.notifyMeasurementsChanged();
		}
		
		internal void  atomPicked(int atomIndex, bool shiftKey)
		{
			pickingManager.atomPicked(atomIndex, shiftKey);
		}
		
		internal void  clearClickCount()
		{
			mouseManager.clearClickCount();
		}
		
		internal void  notifyAtomPicked(int atomIndex)
		{
			if (atomIndex != - 1 && jmolStatusListener != null)
				jmolStatusListener.notifyAtomPicked(atomIndex, modelManager.getAtomInfo(atomIndex));
		}
		
		public override void  showUrl(System.String urlString)
		{
			if (jmolStatusListener != null)
				jmolStatusListener.showUrl(urlString);
		}
		
		public void  showConsole(bool showConsole)
		{
			if (jmolStatusListener != null)
				jmolStatusListener.showConsole(showConsole);
		}
		
		internal System.String getAtomInfo(int atomIndex)
		{
			return modelManager.getAtomInfo(atomIndex);
		}
		
		/// <summary>*************************************************************
		/// mth 2003 05 31 - needs more work
		/// this should be implemented using properties
		/// or as a hashtable using boxed/wrapped values so that the
		/// values could be shared
		/// </summary>
		/// <param name="key">
		/// </param>
		/// <returns> the boolean property
		/// mth 2005 06 24
		/// and/or these property names should be interned strings
		/// so that we can just do == comparisions between strings
		/// **************************************************************
		/// </returns>
		
		public override bool getBooleanProperty(System.String key)
		{
			if (key.ToUpper().Equals("perspectiveDepth".ToUpper()))
				return PerspectiveDepth;
			if (key.ToUpper().Equals("showAxes".ToUpper()))
				return getShapeShow(JmolConstants.SHAPE_AXES);
			if (key.ToUpper().Equals("showBoundBox".ToUpper()))
				return getShapeShow(JmolConstants.SHAPE_BBCAGE);
			if (key.ToUpper().Equals("showUnitcell".ToUpper()))
				return getShapeShow(JmolConstants.SHAPE_UCCAGE);
			if (key.ToUpper().Equals("showHydrogens".ToUpper()))
				return ShowHydrogens;
			if (key.ToUpper().Equals("showMeasurements".ToUpper()))
				return ShowMeasurements;
			if (key.ToUpper().Equals("showSelections".ToUpper()))
				return getSelectionHaloEnabled();
			if (key.ToUpper().Equals("axesOrientationRasmol".ToUpper()))
				return AxesOrientationRasmol;
			if (key.ToUpper().Equals("windowCentered".ToUpper()))
				return WindowCentered;
			if (key.ToUpper().Equals("zeroBasedXyzRasmol".ToUpper()))
				return ZeroBasedXyzRasmol;
			if (key.ToUpper().Equals("testFlag1".ToUpper()))
				return TestFlag1;
			if (key.ToUpper().Equals("testFlag2".ToUpper()))
				return TestFlag2;
			if (key.ToUpper().Equals("testFlag3".ToUpper()))
				return TestFlag3;
			if (key.ToUpper().Equals("testFlag4".ToUpper()))
				return TestFlag4;
			if (key.ToUpper().Equals("chainCaseSensitive".ToUpper()))
				return ChainCaseSensitive;
			if (key.ToUpper().Equals("hideNameInPopup".ToUpper()))
				return HideNameInPopup;
			if (key.ToUpper().Equals("autobond".ToUpper()))
				return AutoBond;
			if (key.ToUpper().Equals("greyscaleRendering".ToUpper()))
				return GreyscaleRendering;
			if (key.ToUpper().Equals("disablePopupMenu".ToUpper()))
				return DisablePopupMenu;
			System.Console.Out.WriteLine("viewer.getBooleanProperty(" + key + ") - unrecognized");
			return false;
		}
		
		public override void  setBooleanProperty(System.String key, bool value_Renamed)
		{
			refresh();
			if (key.ToUpper().Equals("perspectiveDepth".ToUpper()))
			{
				PerspectiveDepth = value_Renamed; return ;
			}
			if (key.ToUpper().Equals("showAxes".ToUpper()))
			{
				setShapeShow(JmolConstants.SHAPE_AXES, value_Renamed); return ;
			}
			if (key.ToUpper().Equals("showBoundBox".ToUpper()))
			{
				setShapeShow(JmolConstants.SHAPE_BBCAGE, value_Renamed); return ;
			}
			if (key.ToUpper().Equals("showUnitcell".ToUpper()))
			{
				setShapeShow(JmolConstants.SHAPE_UCCAGE, value_Renamed); return ;
			}
			if (key.ToUpper().Equals("showHydrogens".ToUpper()))
			{
				ShowHydrogens = value_Renamed; return ;
			}
			if (key.ToUpper().Equals("showHydrogens".ToUpper()))
			{
				ShowHydrogens = value_Renamed; return ;
			}
			if (key.ToUpper().Equals("showMeasurements".ToUpper()))
			{
				ShowMeasurements = value_Renamed; return ;
			}
			if (key.ToUpper().Equals("showSelections".ToUpper()))
			{
				setSelectionHaloEnabled(value_Renamed); return ;
			}
			if (key.ToUpper().Equals("axesOrientationRasmol".ToUpper()))
			{
				AxesOrientationRasmol = value_Renamed; return ;
			}
			if (key.ToUpper().Equals("windowCentered".ToUpper()))
			{
				WindowCentered = value_Renamed; return ;
			}
			if (key.ToUpper().Equals("zeroBasedXyzRasmol".ToUpper()))
			{
				ZeroBasedXyzRasmol = value_Renamed; return ;
			}
			if (key.ToUpper().Equals("frieda".ToUpper()))
			//deprecated
			{
				WindowCentered = !value_Renamed; return ;
			}
			if (key.ToUpper().Equals("testFlag1".ToUpper()))
			{
				TestFlag1 = value_Renamed; return ;
			}
			if (key.ToUpper().Equals("testFlag2".ToUpper()))
			{
				TestFlag2 = value_Renamed; return ;
			}
			if (key.ToUpper().Equals("testFlag3".ToUpper()))
			{
				TestFlag3 = value_Renamed; return ;
			}
			if (key.ToUpper().Equals("testFlag4".ToUpper()))
			{
				TestFlag4 = value_Renamed; return ;
			}
			if (key.ToUpper().Equals("chainCaseSensitive".ToUpper()))
			{
				ChainCaseSensitive = value_Renamed; return ;
			}
			if (key.ToUpper().Equals("ribbonBorder".ToUpper()))
			{
				RibbonBorder = value_Renamed; return ;
			}
			if (key.ToUpper().Equals("hideNameInPopup".ToUpper()))
			{
				HideNameInPopup = value_Renamed; return ;
			}
			if (key.ToUpper().Equals("autobond".ToUpper()))
			{
				AutoBond = value_Renamed; return ;
			}
			if (key.ToUpper().Equals("greyscaleRendering".ToUpper()))
			{
				GreyscaleRendering = value_Renamed; return ;
			}
			if (key.ToUpper().Equals("disablePopupMenu".ToUpper()))
			{
				DisablePopupMenu = value_Renamed; return ;
			}
			System.Console.Out.WriteLine("viewer.setBooleanProperty(" + key + "," + value_Renamed + ") - unrecognized");
		}
		
		internal bool testFlag1;
		internal bool testFlag2;
		internal bool testFlag3;
		internal bool testFlag4;
		
		/// <summary>*************************************************************
		/// Graphics3D
		/// **************************************************************
		/// </summary>
		
		internal bool greyscaleRendering;
		
		internal bool disablePopupMenu;
		
		/////////////////////////////////////////////////////////////////
		// Frame
		/////////////////////////////////////////////////////////////////
		/*
		private BondIterator bondIteratorSelected(byte bondType) {
		return
		getFrame().getBondIterator(bondType, selectionManager.bsSelection);
		}
		*/
		//UPGRADE_NOTE: Final was removed from the declaration of 'nullAtomIterator '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_NOTE: The initialization of  'nullAtomIterator' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		internal AtomIterator nullAtomIterator;
		
		internal class NullAtomIterator : AtomIterator
		{
			public bool hasNext()
			{
				return false;
			}
			public Atom next()
			{
				return null;
			}
			public void  release()
			{
			}
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'nullBondIterator '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_NOTE: The initialization of  'nullBondIterator' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		internal BondIterator nullBondIterator;
		
		internal class NullBondIterator : BondIterator
		{
			public bool hasNext()
			{
				return false;
			}
			public int nextIndex()
			{
				return - 1;
			}
			public Bond next()
			{
				return null;
			}
		}
		
		/////////////////////////////////////////////////////////////////
		// delegated to StyleManager
		/////////////////////////////////////////////////////////////////
		
		public override void  setFrankOn(bool frankOn)
		{
			styleManager.FrankOn = frankOn;
			setShapeSize(JmolConstants.SHAPE_FRANK, frankOn?- 1:0);
		}
		
		internal bool getFrankOn()
		{
			return styleManager.frankOn;
		}
		
		/*
		short getMeasurementMad() {
		return styleManager.measurementMad;
		}
		*/
		
		internal bool setMeasureDistanceUnits(System.String units)
		{
			if (!styleManager.setMeasureDistanceUnits(units))
				return false;
			setShapeProperty(JmolConstants.SHAPE_MEASURES, "reformatDistances", (System.Object) null);
			return true;
		}
		
		internal System.String getMeasureDistanceUnits()
		{
			return styleManager.measureDistanceUnits;
		}
		
		public override void  setJmolDefaults()
		{
			styleManager.setJmolDefaults();
		}
		
		public override void  setRasmolDefaults()
		{
			styleManager.setRasmolDefaults();
		}
		
		internal void  setLabelOffset(int xOffset, int yOffset)
		{
			styleManager.setLabelOffset(xOffset, yOffset);
			refresh();
		}
		
		////////////////////////////////////////////////////////////////
		// temp manager
		////////////////////////////////////////////////////////////////
		
		internal Point3f[] allocTempPoints(int size)
		{
			return tempManager.allocTempPoints(size);
		}
		
		internal void  freeTempPoints(Point3f[] tempPoints)
		{
			tempManager.freeTempPoints(tempPoints);
		}
		
		internal Point3i[] allocTempScreens(int size)
		{
			return tempManager.allocTempScreens(size);
		}
		
		internal void  freeTempScreens(Point3i[] tempScreens)
		{
			tempManager.freeTempScreens(tempScreens);
		}
		
		internal bool[] allocTempBooleans(int size)
		{
			return tempManager.allocTempBooleans(size);
		}
		
		internal void  freeTempBooleans(bool[] tempBooleans)
		{
			tempManager.freeTempBooleans(tempBooleans);
		}
		
		////////////////////////////////////////////////////////////////
		// font stuff
		////////////////////////////////////////////////////////////////
		internal Font3D getFont3D(int fontSize)
		{
			return g3d.getFont3D(JmolConstants.DEFAULT_FONTFACE, JmolConstants.DEFAULT_FONTSTYLE, fontSize);
		}
		
		internal Font3D getFont3D(System.String fontFace, System.String fontStyle, int fontSize)
		{
			return g3d.getFont3D(fontFace, fontStyle, fontSize);
		}
		
		////////////////////////////////////////////////////////////////
		// Access to atom properties for clients
		////////////////////////////////////////////////////////////////
		
		internal System.String getElementSymbol(int i)
		{
			return modelManager.getElementSymbol(i);
		}
		
		internal int getElementNumber(int i)
		{
			return modelManager.getElementNumber(i);
		}
		
		public override System.String getAtomName(int i)
		{
			return modelManager.getAtomName(i);
		}
		
		public override int getAtomNumber(int i)
		{
			return modelManager.getAtomNumber(i);
		}
		
		internal float getAtomX(int i)
		{
			return modelManager.getAtomX(i);
		}
		
		internal float getAtomY(int i)
		{
			return modelManager.getAtomY(i);
		}
		
		internal float getAtomZ(int i)
		{
			return modelManager.getAtomZ(i);
		}
		
		public override Point3f getAtomPoint3f(int i)
		{
			return modelManager.getAtomPoint3f(i);
		}
		
		public override float getAtomRadius(int i)
		{
			return modelManager.getAtomRadius(i);
		}
		
		public override int getAtomArgb(int i)
		{
			return g3d.getColixArgb(modelManager.getAtomColix(i));
		}
		
		internal System.String getAtomChain(int i)
		{
			return modelManager.getAtomChain(i);
		}
		
		public override int getAtomModelIndex(int i)
		{
			return modelManager.getAtomModelIndex(i);
		}
		
		internal System.String getAtomSequenceCode(int i)
		{
			return modelManager.getAtomSequenceCode(i);
		}
		
		public override Point3f getBondPoint3f1(int i)
		{
			return modelManager.getBondPoint3f1(i);
		}
		
		public override Point3f getBondPoint3f2(int i)
		{
			return modelManager.getBondPoint3f2(i);
		}
		
		public override float getBondRadius(int i)
		{
			return modelManager.getBondRadius(i);
		}
		
		public override short getBondOrder(int i)
		{
			return modelManager.getBondOrder(i);
		}
		
		public override int getBondArgb1(int i)
		{
			return g3d.getColixArgb(modelManager.getBondColix1(i));
		}
		
		public override int getBondModelIndex(int i)
		{
			return modelManager.getBondModelIndex(i);
		}
		
		public override int getBondArgb2(int i)
		{
			return g3d.getColixArgb(modelManager.getBondColix2(i));
		}
		
		public override Point3f[] getPolymerLeadMidPoints(int modelIndex, int polymerIndex)
		{
			return modelManager.getPolymerLeadMidPoints(modelIndex, polymerIndex);
		}
		
		public override bool showModelSetDownload()
		{
			return true;
		}
		
		internal System.String formatDecimal(float value_Renamed, int decimalDigits)
		{
			return styleManager.formatDecimal(value_Renamed, decimalDigits);
		}
	}
}