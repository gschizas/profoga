/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-04-02 16:06:20 +0200 (dim., 02 avr. 2006) $
* $Revision: 4871 $
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
//UPGRADE_TODO: The type 'javax.vecmath.Matrix4f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Matrix4f = javax.vecmath.Matrix4f;
//UPGRADE_TODO: The type 'javax.vecmath.Point3f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Point3f = javax.vecmath.Point3f;
using Viewer = org.jmol.viewer.Viewer;
namespace org.jmol.api
{
	
	/// <summary> This is the high-level API for the JmolViewer for simple access.
	/// <p>
	/// We will implement a low-level API at some point
	/// 
	/// </summary>
	
	abstract public class JmolViewer:JmolSimpleViewer
	{
		abstract public JmolStatusListener JmolStatusListener{set;}
		abstract public bool Jvm12orGreater{get;}
		abstract public System.String OperatingSystemName{get;}
		abstract public System.String JavaVersion{get;}
		abstract public System.String JavaVendor{get;}
		abstract public System.Drawing.Size ScreenDimension{set;}
		abstract public int ScreenWidth{get;}
		abstract public int ScreenHeight{get;}
		abstract public System.Drawing.Image ScreenImage{get;}
		abstract public int MotionEventNumber{get;}
		abstract public int MeasurementCount{get;}
		abstract public System.Windows.Forms.Control AwtComponent{get;}
		abstract public System.Collections.BitArray ElementsPresentBitSet{get;}
		abstract public int AnimationFps{get;set;}
		abstract public bool ScriptExecuting{get;}
		abstract public float VibrationPeriod{set;}
		abstract public System.String ModelSetName{get;}
		abstract public System.String ModelSetFileName{get;}
		abstract public System.String ModelSetPathName{get;}
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		abstract public System.Collections.Specialized.NameValueCollection ModelSetProperties{get;}
		abstract public int ModelCount{get;}
		abstract public int AtomCount{get;}
		abstract public int BondCount{get;}
		abstract public int GroupCount{get;}
		abstract public int ChainCount{get;}
		abstract public int PolymerCount{get;}
		abstract public int ModeMouse{set;}
		abstract public bool ShowHydrogens{get;set;}
		abstract public bool ShowMeasurements{get;set;}
		abstract public System.Collections.BitArray SelectionSet{get;set;}
		abstract public System.Collections.BitArray GroupsPresentBitSet{get;}
		abstract public bool PerspectiveDepth{get;set;}
		abstract public bool ShowAxes{get;set;}
		abstract public bool ShowBbcage{get;set;}
		abstract public float RotationRadius{get;}
		abstract public int ZoomPercent{get;}
		abstract public Matrix4f UnscaledTransformMatrix{get;}
		abstract public System.String ColorBackground{set;}
		abstract public bool AxesOrientationRasmol{get;set;}
		abstract public int PercentVdwAtom{get;set;}
		abstract public bool AutoBond{get;set;}
		abstract public short MadBond{get;}
		abstract public short MarBond{set;}
		
		static public JmolViewer allocateViewer(System.Windows.Forms.Control awtComponent, JmolAdapter jmolAdapter)
		{
			return Viewer.allocateViewer(awtComponent, jmolAdapter);
		}
		
		abstract public void  setAppletContext(System.Uri documentBase, System.Uri codeBase, System.String appletProxy);
		
		abstract public void  haltScriptExecution();
		
		abstract public bool haveFrame();
		
		abstract public void  pushHoldRepaint();
		abstract public void  popHoldRepaint();
		
		abstract public void  setJmolDefaults();
		abstract public void  setRasmolDefaults();
		abstract public void  setDebugScript(bool debugScript);
		
		abstract public void  setFrankOn(bool frankOn);
		abstract public void  releaseScreenImage();
		
		
		abstract public void  notifyRepainted();
		
		//UPGRADE_ISSUE: Class 'java.awt.Event' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
		abstract public bool handleOldJvm10Event(Event e);
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		abstract public void  openReader(System.String fullPathName, System.String name, System.IO.StreamReader reader);
		abstract public void  openClientFile(System.String fullPathName, System.String fileName, System.Object clientFile);
		
		abstract public void  showUrl(System.String urlString);
		
		abstract public void  deleteMeasurement(int i);
		abstract public void  clearMeasurements();
		abstract public System.String getMeasurementStringValue(int i);
		abstract public int[] getMeasurementCountPlusIndices(int i);
		
		abstract public System.String evalStringQuiet(System.String script);
		
		abstract public void  setVectorScale(float vectorScaleValue);
		abstract public void  setVibrationScale(float vibrationScaleValue);
		abstract public int getModelNumber(int atomSetIndex);
		abstract public System.String getModelName(int atomSetIndex);
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		abstract public System.Collections.Specialized.NameValueCollection getModelProperties(int atomSetIndex);
		abstract public System.String getModelProperty(int atomSetIndex, System.String propertyName);
		abstract public bool modelHasVibrationVectors(int atomSetIndex);
		abstract public int getDisplayModelIndex();
		abstract public int getPolymerCountInModel(int modelIndex);
		abstract public void  setSelectionHaloEnabled(bool haloEnabled);
		
		abstract public void  selectAll();
		abstract public void  clearSelection();
		
		abstract public void  addSelectionListener(JmolSelectionListener listener);
		abstract public void  removeSelectionListener(JmolSelectionListener listener);
		
		abstract public void  homePosition();
		abstract public void  rotateFront();
		abstract public void  rotateToX(int degrees);
		abstract public void  rotateToY(int degrees);
		
		abstract public void  rotateToX(float radians);
		abstract public void  rotateToY(float radians);
		abstract public void  rotateToZ(float radians);
		
		abstract public void  setCenterSelected();
		
		abstract public int getAtomNumber(int atomIndex);
		abstract public System.String getAtomName(int atomIndex);
		
		abstract public int getBackgroundArgb();
		
		abstract public float getAtomRadius(int atomIndex);
		abstract public Point3f getAtomPoint3f(int atomIndex);
		abstract public int getAtomArgb(int atomIndex);
		abstract public int getAtomModelIndex(int atomIndex);
		
		abstract public float getBondRadius(int bondIndex);
		abstract public Point3f getBondPoint3f1(int bondIndex);
		abstract public Point3f getBondPoint3f2(int bondIndex);
		abstract public int getBondArgb1(int bondIndex);
		abstract public int getBondArgb2(int bondIndex);
		abstract public short getBondOrder(int bondIndex);
		abstract public int getBondModelIndex(int bondIndex);
		
		abstract public Point3f[] getPolymerLeadMidPoints(int modelIndex, int polymerIndex);
		
		abstract public void  refresh();
		
		abstract public bool getBooleanProperty(System.String propertyName);
		abstract public void  setBooleanProperty(System.String propertyName, bool value_Renamed);
		
		abstract public bool showModelSetDownload();
	}
}