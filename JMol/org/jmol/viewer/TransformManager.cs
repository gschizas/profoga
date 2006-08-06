/* $RCSfile$
* $Author: hansonr $
* $Date: 2006-04-11 13:30:26 +0200 (mar., 11 avr. 2006) $
* $Revision: 4953 $
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
//UPGRADE_TODO: The type 'javax.vecmath.Point3f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Point3f = javax.vecmath.Point3f;
//UPGRADE_TODO: The type 'javax.vecmath.Point3i' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Point3i = javax.vecmath.Point3i;
//UPGRADE_TODO: The type 'javax.vecmath.Vector3f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Vector3f = javax.vecmath.Vector3f;
//UPGRADE_TODO: The type 'javax.vecmath.Matrix4f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Matrix4f = javax.vecmath.Matrix4f;
//UPGRADE_TODO: The type 'javax.vecmath.Matrix3f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Matrix3f = javax.vecmath.Matrix3f;
//UPGRADE_TODO: The type 'javax.vecmath.AxisAngle4f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using AxisAngle4f = javax.vecmath.AxisAngle4f;
namespace org.jmol.viewer
{
	
	class TransformManager
	{
		virtual internal float TranslationXPercent
		{
			get
			{
				return (xTranslation - width / 2) * 100 / width;
			}
			
		}
		virtual internal float TranslationYPercent
		{
			get
			{
				return (yTranslation - height / 2) * 100 / height;
			}
			
		}
		virtual internal float TranslationZPercent
		{
			get
			{
				return 0;
			}
			
		}
		virtual internal System.String OrientationText
		{
			get
			{
				return MoveToText + "\nOR\n" + RotateZyzText;
			}
			
		}
		virtual internal System.String MoveToText
		{
			get
			{
				axisangleT.set_Renamed(matrixRotate);
				float degrees = axisangleT.angle * degreesPerRadian;
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				sb.Append("moveto 1");
				if (degrees < 0.01f)
				{
					sb.Append(" 0 0 0 0");
				}
				else
				{
					vectorT.set_Renamed(axisangleT.x, axisangleT.y, axisangleT.z);
					vectorT.normalize();
					vectorT.scale(1000);
					truncate0(sb, vectorT.x);
					truncate0(sb, vectorT.y);
					truncate0(sb, vectorT.z);
					truncate1(sb, degrees);
				}
				int zoom = ZoomPercent;
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				int tX = (int) TranslationXPercent;
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				int tY = (int) TranslationYPercent;
				if (zoom != 100 || tX != 0 || tY != 0)
				{
					sb.Append(" ");
					sb.Append(zoom);
					if (tX != 0 || tY != 0)
					{
						sb.Append(" ");
						sb.Append(tX);
						sb.Append(" ");
						sb.Append(tY);
					}
				}
				return "" + sb + ";";
			}
			
		}
		virtual internal System.String RotateZyzText
		{
			/*
			String getRotateXyzText() {
			StringBuffer sb = new StringBuffer();
			float m20 = matrixRotate.m20;
			float rY = -(float)Math.asin(m20) * degreesPerRadian;
			float rX, rZ;
			if (m20 > .999f || m20 < -.999f) {
			rX = -(float)Math.atan2(matrixRotate.m12, matrixRotate.m11) *
			degreesPerRadian;
			rZ = 0;
			} else {
			rX = (float)Math.atan2(matrixRotate.m21, matrixRotate.m22) *
			degreesPerRadian;
			rZ = (float)Math.atan2(matrixRotate.m10, matrixRotate.m00) *
			degreesPerRadian;
			}
			sb.append("reset");
			if (rX != 0) {
			sb.append("; rotate x");
			truncate1(sb, rX);
			}
			if (rY != 0) {
			sb.append("; rotate y");
			truncate1(sb, rY);
			}
			if (rZ != 0) {
			sb.append("; rotate z");
			truncate1(sb, rZ);
			}
			sb.append(";");
			int zoom = getZoomPercent();
			if (zoom != 100) {
			sb.append(" zoom ");
			sb.append(zoom);
			sb.append(";");
			}
			int tX = getTranslationXPercent();
			if (tX != 0) {
			sb.append(" translate x ");
			sb.append(tX);
			sb.append(";");
			}
			int tY = getTranslationYPercent();
			if (tY != 0) {
			sb.append(" translate y ");
			sb.append(tY);
			sb.append(";");
			}
			return "" + sb;
			}
			*/
			
			
			get
			{
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				float m22 = matrixRotate.m22;
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				float rY = (float) System.Math.Acos(m22) * degreesPerRadian;
				float rZ1, rZ2;
				if (m22 > .999f || m22 < - .999f)
				{
					rZ1 = (float) Math.atan2(matrixRotate.m10, matrixRotate.m11) * degreesPerRadian;
					rZ2 = 0;
				}
				else
				{
					rZ1 = (float) Math.atan2(matrixRotate.m21, - matrixRotate.m20) * degreesPerRadian;
					rZ2 = (float) Math.atan2(matrixRotate.m12, matrixRotate.m02) * degreesPerRadian;
				}
				if (rZ1 != 0 && rY != 0 && rZ2 != 0)
					sb.Append("#Follows Z-Y-Z convention for Euler angles\n");
				sb.Append("reset");
				if (rZ1 != 0)
				{
					sb.Append("; rotate z");
					truncate1(sb, rZ1);
				}
				if (rY != 0)
				{
					sb.Append("; rotate y");
					truncate1(sb, rY);
				}
				if (rZ2 != 0)
				{
					sb.Append("; rotate z");
					truncate1(sb, rZ2);
				}
				int zoom = ZoomPercent;
				if (zoom != 100)
				{
					sb.Append("; zoom ");
					sb.Append(zoom);
				}
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				int tX = (int) TranslationXPercent;
				if (tX != 0)
				{
					sb.Append("; translate x ");
					sb.Append(tX);
				}
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				int tY = (int) TranslationYPercent;
				if (tY != 0)
				{
					sb.Append("; translate y ");
					sb.Append(tY);
				}
				sb.Append(';');
				return "" + sb;
			}
			
		}
		virtual internal System.String TransformText
		{
			get
			{
				return "matrixRotate=\n" + matrixRotate;
			}
			
		}
		virtual internal int ZoomPercent
		{
			get
			{
				return zoomPercent;
			}
			
		}
		virtual internal int ZoomPercentSetting
		{
			get
			{
				return zoomPercentSetting;
			}
			
		}
		virtual internal bool ZoomEnabled
		{
			set
			{
				if (this.zoomEnabled != value)
				{
					this.zoomEnabled = value;
					calcZoom();
				}
			}
			
		}
		virtual internal float ScaleAngstromsPerInch
		{
			set
			{
				scalePixelsPerAngstrom = scaleDefaultPixelsPerAngstrom = 72 / value;
			}
			
		}
		virtual internal bool SlabEnabled
		{
			get
			{
				return slabEnabled;
			}
			
			set
			{
				this.slabEnabled = value;
			}
			
		}
		virtual internal int SlabPercentSetting
		{
			get
			{
				return slabPercentSetting;
			}
			
		}
		virtual internal int ModeSlab
		{
			get
			{
				return modeSlab;
			}
			
			// miguel 24 sep 2004 - as I recall, this slab mode stuff is not implemented
			
			set
			{
				this.modeSlab = value;
			}
			
		}
		virtual internal bool PerspectiveDepth
		{
			get
			{
				return perspectiveDepth;
			}
			
			set
			{
				this.perspectiveDepth = value;
				//scaleFitToScreen();
			}
			
		}
		virtual internal float CameraDepth
		{
			get
			{
				return cameraDepth;
			}
			
			set
			{
				cameraDepth = value;
			}
			
		}
		virtual internal bool Oversample
		{
			set
			{
				if (this.tOversample == value)
					return ;
				this.tOversample = value;
				if (value)
				{
					width = width4;
					height = height4;
				}
				else
				{
					width = width1;
					height = height1;
				}
				scaleFitToScreen();
			}
			
		}
		virtual internal bool AxesOrientationRasmol
		{
			set
			{
				this.axesOrientationRasmol = value;
			}
			
		}
		virtual internal float RotationRadiusIncrease
		{
			get
			{
				//System.out.println("TransformManager.getRotationRadiusIncrease()");
				//System.out.println("minimumZ=" + minimumZ);
				// add one more pixel just for good luck;
				int backupDistance = cameraDistance - minimumZ + 1;
				float angstromsIncrease = backupDistance / scalePixelsPerAngstrom;
				//System.out.println("angstromsIncrease=" + angstromsIncrease);
				return angstromsIncrease;
			}
			
		}
		virtual internal Matrix4f UnscaledTransformMatrix
		{
			get
			{
				// for PovRay only, via Viewer
				Matrix4f unscaled = new Matrix4f();
				unscaled.setIdentity();
				vectorTemp.set_Renamed(viewer.RotationCenter);
				matrixTemp.setZero();
				matrixTemp.setTranslation(vectorTemp);
				unscaled.sub(matrixTemp);
				matrixTemp.set_Renamed(matrixRotate);
				unscaled.mul(matrixTemp, unscaled);
				return unscaled;
			}
			
		}
		virtual internal float VibrationPeriod
		{
			set
			{
				if (value <= 0)
				{
					this.vibrationPeriod = 0;
					this.vibrationPeriodMs = 0;
					clearVibration();
				}
				else
				{
					this.vibrationPeriod = value;
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					this.vibrationPeriodMs = (int) (value * 1000);
					VibrationOn = true;
				}
			}
			
		}
		virtual internal float VibrationT
		{
			set
			{
				vibrationRadians = value * twoPI;
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				vibrationAmplitude = (float) System.Math.Cos(vibrationRadians) * vibrationScale;
			}
			
		}
		virtual internal float VectorScale
		{
			set
			{
				if (value >= - 10 && value <= 10)
					vectorScale = value;
			}
			
		}
		virtual internal float VibrationScale
		{
			set
			{
				if (value >= - 10 && value <= 10)
					vibrationScale = value;
			}
			
		}
		virtual internal int SpinX
		{
			set
			{
				spinX = value;
				//    System.out.println("spinX=" + spinX);
			}
			
		}
		virtual internal int SpinY
		{
			set
			{
				spinY = value;
				//    System.out.println("spinY=" + spinY);
			}
			
		}
		virtual internal int SpinZ
		{
			set
			{
				spinZ = value;
				//    System.out.println("spinZ=" + spinZ);
			}
			
		}
		virtual internal int SpinFps
		{
			set
			{
				if (value <= 0)
					value = 1;
				else if (value > 50)
					value = 50;
				spinFps = value;
				//    System.out.println("spinFps=" + spinFps);
			}
			
		}
		virtual internal bool SpinOn
		{
			set
			{
				if (value)
				{
					if (spinThread == null)
					{
						spinThread = new SpinThread(this);
						spinThread.Start();
					}
				}
				else
				{
					if (spinThread != null)
					{
						spinThread.Interrupt();
						spinThread = null;
					}
				}
				this.spinOn = value;
				//    System.out.println("spinOn=" + spinOn);
			}
			
		}
		private bool VibrationOn
		{
			set
			{
				if (!value || !viewer.haveFrame())
				{
					if (vibrationThread != null)
					{
						vibrationThread.Interrupt();
						vibrationThread = null;
					}
					this.vibrationOn = false;
					return ;
				}
				if (viewer.ModelCount < 1)
				{
					this.vibrationOn = false;
					return ;
				}
				if (vibrationThread == null)
				{
					vibrationThread = new VibrationThread(this);
					vibrationThread.Start();
				}
				this.vibrationOn = true;
			}
			
		}
		virtual internal int StereoMode
		{
			set
			{
				this.stereoMode = value;
				viewer.GreyscaleRendering = value >= JmolConstants.STEREO_REDCYAN;
			}
			
		}
		virtual internal float StereoDegrees
		{
			set
			{
				this.stereoDegrees = value;
				this.stereoRadians = value * radiansPerDegree;
			}
			
		}
		
		internal Viewer viewer;
		
		internal TransformManager(Viewer viewer)
		{
			this.viewer = viewer;
		}
		
		internal virtual void  homePosition()
		{
			matrixRotate.setIdentity(); // no rotations
			//    setSlabEnabled(false);              // no slabbing
			//    slabToPercent(100);
			ZoomEnabled = true;
			zoomToPercent(100);
			scaleFitToScreen();
		}
		
		/// <summary>*************************************************************
		/// ROTATIONS
		/// **************************************************************
		/// </summary>
		
		// this matrix only holds rotations ... no translations
		// however, it cannot be a Matrix3f because we need to multiply it by
		// a matrix4f which contains translations
		//UPGRADE_NOTE: Final was removed from the declaration of 'matrixRotate '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private Matrix3f matrixRotate = new Matrix3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'matrixTemp3 '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private Matrix3f matrixTemp3 = new Matrix3f();
		
		internal virtual void  rotateXYBy(int xDelta, int yDelta)
		{
			rotateXRadians(yDelta * radiansPerDegree);
			rotateYRadians(xDelta * radiansPerDegree);
		}
		
		internal virtual void  rotateZBy(int zDelta)
		{
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			rotateZRadians((float) System.Math.PI * zDelta / 180);
		}
		
		internal virtual void  rotateFront()
		{
			matrixRotate.setIdentity();
		}
		
		internal virtual void  rotateToX(float angleRadians)
		{
			matrixRotate.rotX(angleRadians);
		}
		internal virtual void  rotateToY(float angleRadians)
		{
			matrixRotate.rotY(angleRadians);
		}
		internal virtual void  rotateToZ(float angleRadians)
		{
			matrixRotate.rotZ(angleRadians);
		}
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'rotateXRadians'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		internal virtual void  rotateXRadians(float angleRadians)
		{
			lock (this)
			{
				matrixTemp3.rotX(angleRadians);
				matrixRotate.mul(matrixTemp3, matrixRotate);
				//    System.out.println("rotateXRadius matrixRotate=\n" + matrixRotate);
			}
		}
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'rotateYRadians'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		internal virtual void  rotateYRadians(float angleRadians)
		{
			lock (this)
			{
				if (axesOrientationRasmol)
					angleRadians = - angleRadians;
				matrixTemp3.rotY(angleRadians);
				matrixRotate.mul(matrixTemp3, matrixRotate);
			}
		}
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'rotateZRadians'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		internal virtual void  rotateZRadians(float angleRadians)
		{
			lock (this)
			{
				if (axesOrientationRasmol)
					angleRadians = - angleRadians;
				matrixTemp3.rotZ(angleRadians);
				matrixRotate.mul(matrixTemp3, matrixRotate);
			}
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'radiansPerDegree '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
		internal static readonly float radiansPerDegree = (float) (2 * System.Math.PI / 360);
		//UPGRADE_NOTE: Final was removed from the declaration of 'degreesPerRadian '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
		internal static readonly float degreesPerRadian = (float) (360 / (2 * System.Math.PI));
		
		internal virtual void  rotateZRadiansScript(float angleRadians)
		{
			matrixTemp3.rotZ(angleRadians);
			matrixRotate.mul(matrixTemp3, matrixRotate);
		}
		
		internal virtual void  rotate(AxisAngle4f axisAngle)
		{
			matrixTemp3.setIdentity();
			matrixTemp3.set_Renamed(axisAngle);
			matrixRotate.mul(matrixTemp3, matrixRotate);
		}
		
		internal virtual void  rotateAxisAngle(float x, float y, float z, float degrees)
		{
			axisangleT.set_Renamed(x, y, z, degrees * radiansPerDegree);
			rotate(axisangleT);
		}
		
		internal virtual void  rotateTo(float x, float y, float z, float degrees)
		{
			if (degrees < .01 && degrees > - .01)
			{
				matrixRotate.setIdentity();
			}
			else
			{
				axisangleT.set_Renamed(x, y, z, degrees * radiansPerDegree);
				matrixRotate.set_Renamed(axisangleT);
			}
		}
		
		internal virtual void  rotateTo(AxisAngle4f axisAngle)
		{
			if (axisAngle.angle < .01 && axisAngle.angle > - .01)
				matrixRotate.setIdentity();
			else
				matrixRotate.set_Renamed(axisAngle);
		}
		
		/// <summary>*************************************************************
		/// TRANSLATIONS
		/// **************************************************************
		/// </summary>
		internal float xTranslation;
		internal float yTranslation;
		
		internal virtual void  translateXYBy(int xDelta, int yDelta)
		{
			xTranslation += xDelta;
			yTranslation += yDelta;
		}
		
		internal virtual void  translateToXPercent(float percent)
		{
			// FIXME -- what is the proper RasMol interpretation of this with zooming?
			xTranslation = (width / 2) + width * percent / 100;
		}
		
		internal virtual void  translateToYPercent(float percent)
		{
			yTranslation = (height / 2) + height * percent / 100;
		}
		
		internal virtual void  translateToZPercent(float percent)
		{
			// FIXME who knows what this should be? some type of zoom?
		}
		
		internal virtual void  translateCenterTo(int x, int y)
		{
			xTranslation = x;
			yTranslation = y;
			//System.out.println("translateCenterTo: "+x+" "+y);
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'axisangleT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal AxisAngle4f axisangleT = new AxisAngle4f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'vectorT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Vector3f vectorT = new Vector3f();
		
		internal static void  truncate0(System.Text.StringBuilder sb, float val)
		{
			sb.Append(' ');
			//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangMathround_float'"
			sb.Append((int) System.Math.Round((double) val));
		}
		
		internal static void  truncate1(System.Text.StringBuilder sb, float val)
		{
			sb.Append(' ');
			//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangMathround_float'"
			sb.Append((int) System.Math.Round((double) (val * 10)) / 10f);
		}
		
		/*
		static void truncate2(StringBuffer sb, float val) {
		sb.append(" ");
		sb.append(Math.round(val * 100) / 100f);
		}
		
		static void truncate3(StringBuffer sb, float val) {
		sb.append(" ");
		sb.append(Math.round(val * 1000) / 1000f);
		}
		*/
		
		internal virtual void  getAxisAngle(AxisAngle4f axisAngle)
		{
			axisAngle.set_Renamed(matrixRotate);
		}
		
		internal virtual void  setRotation(Matrix3f matrixRotation)
		{
			this.matrixRotate.set_Renamed(matrixRotation);
		}
		
		internal virtual void  getRotation(Matrix3f matrixRotation)
		{
			// hmm ... I suppose that there could be a race condiditon here
			// if matrixRotate is being modified while this is called
			matrixRotation.set_Renamed(this.matrixRotate);
		}
		
		/// <summary>*************************************************************
		/// ZOOM
		/// **************************************************************
		/// </summary>
		internal bool zoomEnabled = true;
		// zoomPercent is the current displayed zoom value
		internal int zoomPercent = 100;
		// zoomPercentSetting is the current setting of zoom
		// if zoom is not enabled then the two values will be different
		internal int zoomPercentSetting = 100;
		
		internal virtual void  zoomBy(int pixels)
		{
			if (pixels > 20)
				pixels = 20;
			else if (pixels < - 20)
				pixels = - 20;
			int deltaPercent = pixels * zoomPercentSetting / 50;
			if (deltaPercent == 0)
				deltaPercent = (pixels > 0?1:(deltaPercent < 0?- 1:0));
			int percent = deltaPercent + zoomPercentSetting;
			zoomToPercent(percent);
		}
		
		internal virtual void  zoomToPercent(int percentZoom)
		{
			zoomPercentSetting = percentZoom;
			calcZoom();
		}
		
		internal virtual void  zoomByPercent(int percentZoom)
		{
			int delta = percentZoom * zoomPercentSetting / 100;
			if (delta == 0)
				delta = (percentZoom < 0)?- 1:1;
			zoomPercentSetting += delta;
			calcZoom();
		}
		
		private void  calcZoom()
		{
			if (zoomPercentSetting < 5)
				zoomPercentSetting = 5;
			if (zoomPercentSetting > Viewer.MAXIMUM_ZOOM_PERCENTAGE)
				zoomPercentSetting = Viewer.MAXIMUM_ZOOM_PERCENTAGE;
			zoomPercent = (zoomEnabled)?zoomPercentSetting:100;
			scalePixelsPerAngstrom = scaleDefaultPixelsPerAngstrom * zoomPercent / 100;
		}
		
		/// <summary>*************************************************************
		/// SLAB
		/// **************************************************************
		/// </summary>
		/*
		slab is a term defined and used in rasmol.
		it is a z-axis clipping plane. only atoms behind the slab get rendered.
		100% means:
		- the slab is set to z==0
		- 100% of the molecule will be shown
		50% means:
		- the slab is set to the center of rotation of the molecule
		- only the atoms behind the center of rotation are shown
		0% means:
		- the slab is set behind the molecule
		- 0% (nothing, nada, nil, null) gets shown
		*/
		
		/*
		final static int SLABREJECT = 0;
		final static int SLABHALF = 1;
		final static int SLABHOLLOW = 2;
		final static int SLABSOLID = 3;
		final static int SLABSECTION = 4;
		*/
		
		internal bool slabEnabled = false;
		internal int modeSlab;
		internal int slabPercentSetting = 100;
		internal int depthPercentSetting = 0;
		
		private int slabValue;
		private int depthValue;
		
		internal virtual void  slabByPercentagePoints(int percentage)
		{
			slabPercentSetting += percentage;
			if (slabPercentSetting < 1)
				slabPercentSetting = 1;
			else if (slabPercentSetting > 100)
				slabPercentSetting = 100;
			if (depthPercentSetting >= slabPercentSetting)
				depthPercentSetting = slabPercentSetting - 1;
		}
		
		internal virtual void  depthByPercentagePoints(int percentage)
		{
			depthPercentSetting += percentage;
			if (depthPercentSetting < 0)
				depthPercentSetting = 0;
			else if (depthPercentSetting > 99)
				depthPercentSetting = 99;
			if (slabPercentSetting <= depthPercentSetting)
				slabPercentSetting = depthPercentSetting + 1;
		}
		
		internal virtual void  slabDepthByPercentagePoints(int percentage)
		{
			if (percentage > 0)
			{
				if (slabPercentSetting + percentage > 100)
					percentage = 100 - slabPercentSetting;
			}
			else
			{
				if (depthPercentSetting + percentage < 0)
					percentage = 0 - depthPercentSetting;
			}
			slabPercentSetting += percentage;
			depthPercentSetting += percentage;
		}
		
		internal virtual void  slabToPercent(int percentSlab)
		{
			slabPercentSetting = percentSlab < 1?1:(percentSlab > 100?100:percentSlab);
			if (depthPercentSetting >= slabPercentSetting)
				depthPercentSetting = slabPercentSetting - 1;
		}
		
		// depth is an extension added by OpenRasMol
		// it represents the 'back' of the slab plane
		internal virtual void  depthToPercent(int percentDepth)
		{
			depthPercentSetting = percentDepth < 0?0:(percentDepth > 99?99:percentDepth);
			if (slabPercentSetting <= depthPercentSetting)
				slabPercentSetting = depthPercentSetting + 1;
		}
		
		internal virtual void  calcSlabAndDepthValues()
		{
			slabValue = 0;
			depthValue = System.Int32.MaxValue;
			if (slabEnabled)
			{
				// miguel 24 sep 2004 -- the comment below does not seem right to me
				// I don't think that all transformed z coordinates are negative
				// any more
				//
				// all transformed z coordinates are negative
				// a slab percentage of 100 should map to zero
				// a slab percentage of 0 should map to -diameter
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				int radius = (int) (viewer.RotationRadius * scalePixelsPerAngstrom);
				slabValue = ((100 - slabPercentSetting) * 2 * radius / 100) + cameraDistance;
				depthValue = ((100 - depthPercentSetting) * 2 * radius / 100) + cameraDistance;
			}
		}
		
		/// <summary>*************************************************************
		/// PERSPECTIVE
		/// **************************************************************
		/// </summary>
		internal bool perspectiveDepth = true;
		internal float cameraDepth = 3;
		internal int cameraDistance = 1000; // prevent divide by zero on startup
		internal float cameraDistanceFloat = 1000; // prevent divide by zero on startup
		
		/// <summary>*************************************************************
		/// SCREEN SCALING
		/// **************************************************************
		/// </summary>
		internal bool tOversample;
		internal int width, height;
		internal int width1, height1, width4, height4;
		internal int screenPixelCount;
		internal float scalePixelsPerAngstrom;
		internal float scaleDefaultPixelsPerAngstrom;
		
		internal virtual void  setScreenDimension(int width, int height)
		{
			this.width1 = this.width = width;
			this.width4 = width + width;
			this.height1 = this.height = height;
			this.height4 = height + height;
		}
		
		internal virtual void  scaleFitToScreen()
		{
			if (width == 0 || height == 0 || !viewer.haveFrame())
				return ;
			// translate to the middle of the screen
			xTranslation = width / 2;
			yTranslation = height / 2;
			// 2005 02 22
			// switch to finding larger screen dimension
			// find smaller screen dimension
			screenPixelCount = width;
			if (height > screenPixelCount)
				screenPixelCount = height;
			// ensure that rotations don't leave some atoms off the screen
			// note that this radius is to the furthest outside edge of an atom
			// given the current VDW radius setting. it is currently *not*
			// recalculated when the vdw radius settings are changed
			// leave a very small margin - only 1 on top and 1 on bottom
			if (screenPixelCount > 2)
				screenPixelCount -= 2;
			scaleDefaultPixelsPerAngstrom = screenPixelCount / 2 / viewer.RotationRadius;
			if (perspectiveDepth)
			{
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				cameraDistance = (int) (cameraDepth * screenPixelCount);
				cameraDistanceFloat = cameraDistance;
				float scaleFactor = (cameraDistance + screenPixelCount / 2) / cameraDistanceFloat;
				// mth - for some reason, I can make the scaleFactor bigger in this
				// case. I do not know why, but there is extra space around the edges.
				// I have looked at it three times and still cannot figure it out
				// so just bump it up a bit.
				scaleFactor = (float) (scaleFactor + 0.02);
				scaleDefaultPixelsPerAngstrom *= scaleFactor;
			}
			calcZoom();
		}
		
		/*==============================================================*
		* scalings
		*==============================================================*/
		
		internal virtual float scaleToScreen(int z, float sizeAngstroms)
		{
			// all z's are >= 0
			// so the more positive z is, the smaller the screen scale
			float pixelSize = sizeAngstroms * scalePixelsPerAngstrom;
			if (perspectiveDepth)
				pixelSize = (pixelSize * cameraDistance) / z;
			return pixelSize;
		}
		
		internal virtual float scaleToPerspective(int z, float sizeAngstroms)
		{
			return (perspectiveDepth?(sizeAngstroms * cameraDistance) / (+ z):sizeAngstroms);
		}
		
		internal virtual short scaleToScreen(int z, int milliAngstroms)
		{
			if (milliAngstroms == 0)
				return 0;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			int pixelSize = (int) (milliAngstroms * scalePixelsPerAngstrom / 1000);
			if (perspectiveDepth)
				pixelSize = (pixelSize * cameraDistance) / z;
			if (pixelSize == 0)
				return 1;
			return (short) pixelSize;
		}
		
		/// <summary>*************************************************************
		/// TRANSFORMATIONS
		/// **************************************************************
		/// </summary>
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'matrixTransform '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Matrix4f matrixTransform = new Matrix4f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'point3fVibrationTemp '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private Point3f point3fVibrationTemp = new Point3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'point3fScreenTemp '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private Point3f point3fScreenTemp = new Point3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'point3iScreenTemp '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private Point3i point3iScreenTemp = new Point3i();
		//UPGRADE_NOTE: Final was removed from the declaration of 'matrixTemp '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private Matrix4f matrixTemp = new Matrix4f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'vectorTemp '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private Vector3f vectorTemp = new Vector3f();
		
		
		/// <summary>*************************************************************
		/// RasMol has the +Y axis pointing down
		/// And rotations about the y axis are left-handed
		/// setting this flag makes Jmol mimic this behavior
		/// **************************************************************
		/// </summary>
		internal bool axesOrientationRasmol = false;
		
		internal Point3i defaultRotationCenterTransformed = new Point3i(0, 0, 0);
		internal virtual void  calcTransformMatrices()
		{
			//entry point from FrameRenderer, via Viewer
			calcTransformMatrix();
			calcSlabAndDepthValues();
			viewer.setSlabAndDepthValues(slabValue, depthValue);
			increaseRotationRadius = false;
			minimumZ = System.Int32.MaxValue;
			setPerspectiveOffset();
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'pointT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Point3f pointT = new Point3f();
		//UPGRADE_NOTE: Final was removed from the declaration of 'pointT2 '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal Point3f pointT2 = new Point3f();
		
		internal Vector3f perspectiveOffset = new Vector3f(0, 0, 0);
		internal virtual void  setPerspectiveOffset()
		{
			// lock in the perspective so that when you change
			// centers there is no jump
			if (!viewer.WindowCentered)
			{
				matrixTransform.transform(viewer.DefaultRotationCenter, pointT);
				matrixTransform.transform(viewer.RotationCenter, pointT2);
				perspectiveOffset.sub(pointT, pointT2);
			}
			perspectiveOffset.x = xTranslation;
			perspectiveOffset.y = yTranslation;
			
			perspectiveOffset.z = 0;
			/*
			* The above line disables this method's action. Perhaps a simple idea for
			* post-10.1 consideration would be to leave it at least as an option. 
			* I hope we can return to this after 10.1, but for now the consensus 
			* is to leave it as the 10.00 status quo (no additional z offset).
			* 
			* Note that the effect of this modification was restricted to the 
			* (undocumented) specialized circumstances when both
			* 
			* (a) the "Frieda" switch is on (formerly "set frieda on")
			* 
			*  AND
			*
			* (b) the center has been changed to something other than the default
			* rotation center, either using "set picking center" followed by a user
			* click of an atom, or by a scripted "center (atom expression)".
			* 
			* My understanding was that this change would only affect one user, 
			* because only Frieda Reichsman knows of the Frieda switch, presumably. 
			* It is not otherwise documented.
			* 
			* The proposed change here disabled had no effect whatsoever on 
			* general use.
			*  
			* The current "perspectiveOffset.z = 0" line returns us to 10.00 
			* behavior, with all the issues of molecule distortion and walking into
			* the camera unresolved. (The short-->int fix did get rid of much
			* of the problem.)
			* 
			* I've left adjustedTemporaryScreenPoint(Point3f) in, because it is only
			* a code clarifaction measure -- making it clear that transforming points,
			* whether there are vibration vectors or not, fundamentally must involve
			* the exact same action. Previously, these two transformPoint() variants
			* carried out the exact same action (after adjusting for vibration), but
			* were delivering insignificantly different System.out messages when the
			* molecule walked into the camera, which is something only the "frieda"
			* swich enabled. (Although it was an undiscovered issue as well for pmesh
			* and sasurfaces, but that is another story.) 
			* 
			* In my opinion, the "frieda" switch is currently broken, and the
			* unequal action of "center (atomset)" and "set picking center" is
			* a bug.
			* 
			* These notes are meant solely as a guide to development and should be
			* removed when the issues relating to them are resolved.
			* 
			* Bob Hanson 4/06
			*  
			*/
			//System.out.println("\nsetPerspectiveOffset "+perspectiveOffset);
		}
		
		internal bool increaseRotationRadius;
		internal int minimumZ;
		
		private void  calcTransformMatrix()
		{
			// you absolutely *must* watch the order of these operations
			matrixTransform.setIdentity();
			// first, translate the coordinates back to the center
			vectorTemp.set_Renamed(viewer.RotationCenter);
			
			matrixTemp.setZero();
			matrixTemp.setTranslation(vectorTemp);
			matrixTransform.sub(matrixTemp);
			// now, multiply by angular rotations
			// this is *not* the same as  matrixTransform.mul(matrixRotate);
			matrixTemp.set_Renamed(stereoFrame?matrixStereo:matrixRotate);
			matrixTransform.mul(matrixTemp, matrixTransform);
			//    matrixTransform.mul(matrixRotate, matrixTransform);
			// we want all z coordinates >= 0, with larger coordinates further away
			// this is important for scaling, and is the way our zbuffer works
			// so first, translate an make all z coordinates negative
			vectorTemp.x = 0;
			vectorTemp.y = 0;
			vectorTemp.z = viewer.RotationRadius + cameraDistanceFloat / scalePixelsPerAngstrom;
			matrixTemp.setZero();
			matrixTemp.setTranslation(vectorTemp);
			if (axesOrientationRasmol)
				matrixTransform.add(matrixTemp);
			// make all z positive
			else
				matrixTransform.sub(matrixTemp); // make all z negative
			
			// now scale to screen coordinates
			matrixTemp.setZero();
			matrixTemp.set_Renamed(scalePixelsPerAngstrom);
			if (!axesOrientationRasmol)
			{
				// negate y (for screen) and z (for zbuf)
				matrixTemp.m11 = matrixTemp.m22 = - scalePixelsPerAngstrom;
			}
			matrixTransform.mul(matrixTemp, matrixTransform);
			// note that the image is still centered at 0, 0 in the xy plane
			// all z coordinates are (should be) >= 0
			// translations come later (to deal with perspective)
		}
		
		internal virtual void  transformPoints(int count, Point3f[] angstroms, Point3i[] screens)
		{
			for (int i = count; --i >= 0; )
				screens[i].set_Renamed(transformPoint(angstroms[i]));
		}
		
		internal virtual void  transformPoint(Point3f pointAngstroms, Point3i pointScreen)
		{
			pointScreen.set_Renamed(transformPoint(pointAngstroms));
		}
		
		internal virtual Point3i transformPoint(Point3f pointAngstroms)
		{
			matrixTransform.transform(pointAngstroms, point3fScreenTemp);
			return adjustedTemporaryScreenPoint(pointAngstroms);
		}
		
		internal virtual Point3i adjustedTemporaryScreenPoint(Point3f pointAngstroms)
		{
			int z = (int) point3fScreenTemp.z;
			z -= perspectiveOffset.z;
			
			if (z < cameraDistance)
			{
				if (Float.isNaN(point3fScreenTemp.z))
				{
					System.Console.Out.WriteLine("NaN seen in TransformPoint");
					z = 1;
				}
				else
				{
					System.Console.Out.WriteLine("need to back up the camera");
					System.Console.Out.WriteLine("point3fScreenTemp.z=" + point3fScreenTemp.z + " -> z=" + z);
					increaseRotationRadius = true;
					if (z < minimumZ)
						minimumZ = z;
					if (z <= 0)
					{
						System.Console.Out.WriteLine("WARNING! DANGER! z <= 0! transformPoint()" + " point=" + pointAngstroms + "  ->  " + point3fScreenTemp);
						z = 1;
					}
				}
			}
			point3iScreenTemp.z = z;
			if (perspectiveDepth)
			{
				float perspectiveFactor = cameraDistanceFloat / z;
				point3fScreenTemp.x *= perspectiveFactor;
				point3fScreenTemp.y *= perspectiveFactor;
			}
			point3fScreenTemp.x += perspectiveOffset.x;
			point3fScreenTemp.y += perspectiveOffset.y;
			point3iScreenTemp.x = (int) (point3fScreenTemp.x);
			point3iScreenTemp.y = (int) (point3fScreenTemp.y);
			
			//System.out.println("adjustTempPoint:"+pointAngstroms + " " + point3iScreenTemp);
			return point3iScreenTemp;
		}
		
		internal virtual void  transformPoint(Point3f pointAngstroms, Point3f screen)
		{
			
			//used solely by RocketsRenderer
			//needs consolidation
			
			matrixTransform.transform(pointAngstroms, screen);
			
			float z = screen.z;
			z -= perspectiveOffset.z;
			if (z < cameraDistance)
			{
				System.Console.Out.WriteLine("need to back up the camera");
				increaseRotationRadius = true;
				if (z < minimumZ)
				{
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					minimumZ = (int) z;
				}
				if (z <= 0)
				{
					System.Console.Out.WriteLine("WARNING! DANGER! z <= 0! transformPoint()");
					z = 1;
				}
			}
			screen.z = z;
			if (perspectiveDepth)
			{
				float perspectiveFactor = cameraDistanceFloat / z;
				screen.x = screen.x * perspectiveFactor;
				screen.y = screen.y * perspectiveFactor;
			}
			screen.x += perspectiveOffset.x;
			screen.y += perspectiveOffset.y;
		}
		
		internal virtual Point3i transformPoint(Point3f pointAngstroms, Vector3f vibrationVector)
		{
			
			if (!vibrationOn || vibrationVector == null)
				matrixTransform.transform(pointAngstroms, point3fScreenTemp);
			else
			{
				point3fVibrationTemp.scaleAdd(vibrationAmplitude, vibrationVector, pointAngstroms);
				matrixTransform.transform(point3fVibrationTemp, point3fScreenTemp);
			}
			return adjustedTemporaryScreenPoint(pointAngstroms);
		}
		
		internal virtual void  transformPoint(Point3f pointAngstroms, Vector3f vibrationVector, Point3i pointScreen)
		{
			pointScreen.set_Renamed(transformPoint(pointAngstroms, vibrationVector));
		}
		
		internal virtual void  transformVector(Vector3f vectorAngstroms, Vector3f vectorTransformed)
		{
			matrixTransform.transform(vectorAngstroms, vectorTransformed);
		}
		
		////////////////////////////////////////////////////////////////
		
		internal bool vibrationOn;
		internal float vibrationPeriod;
		internal int vibrationPeriodMs;
		internal float vibrationAmplitude;
		internal float vibrationRadians;
		
		internal float vectorScale = 1f;
		
		internal float vibrationScale = 1f;
		
		internal int spinX, spinY = 30, spinZ, spinFps = 30;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'twoPI '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
		internal static readonly float twoPI = (float) (2 * System.Math.PI);
		internal bool spinOn;
		internal SpinThread spinThread;
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'SpinThread' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		internal class SpinThread:SupportClass.ThreadClass, IThreadRunnable
		{
			public SpinThread(TransformManager enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(TransformManager enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private TransformManager enclosingInstance;
			public TransformManager Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			override public void  Run()
			{
				int myFps = Enclosing_Instance.spinFps;
				int i = 0;
				long timeBegin = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
				//UPGRADE_ISSUE: Method 'java.lang.Thread.isInterrupted' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangThreadisInterrupted'"
				while (!isInterrupted())
				{
					if (myFps != Enclosing_Instance.spinFps)
					{
						myFps = Enclosing_Instance.spinFps;
						i = 0;
						timeBegin = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
					}
					bool refreshNeeded = false;
					if (Enclosing_Instance.spinX != 0)
					{
						Enclosing_Instance.rotateXRadians(Enclosing_Instance.spinX * org.jmol.viewer.TransformManager.radiansPerDegree / myFps);
						refreshNeeded = true;
					}
					if (Enclosing_Instance.spinY != 0)
					{
						Enclosing_Instance.rotateYRadians(Enclosing_Instance.spinY * org.jmol.viewer.TransformManager.radiansPerDegree / myFps);
						refreshNeeded = true;
					}
					if (Enclosing_Instance.spinZ != 0)
					{
						Enclosing_Instance.rotateZRadians(Enclosing_Instance.spinZ * org.jmol.viewer.TransformManager.radiansPerDegree / myFps);
						refreshNeeded = true;
					}
					++i;
					int targetTime = i * 1000 / myFps;
					int currentTime = (int) ((System.DateTime.Now.Ticks - 621355968000000000) / 10000 - timeBegin);
					int sleepTime = targetTime - currentTime;
					if (sleepTime > 0)
					{
						if (refreshNeeded)
							Enclosing_Instance.viewer.refresh();
						try
						{
							//UPGRADE_TODO: Method 'java.lang.Thread.sleep' was converted to 'System.Threading.Thread.Sleep' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangThreadsleep_long'"
							System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64) 10000 * sleepTime));
						}
						catch (System.Threading.ThreadInterruptedException e)
						{
							//            System.out.println("interrupt caught!");
							break;
						}
					}
				}
			}
		}
		
		/// <summary>*************************************************************
		/// Vibration support
		/// **************************************************************
		/// </summary>
		
		internal virtual void  clearVibration()
		{
			VibrationOn = false;
		}
		
		internal VibrationThread vibrationThread;
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'VibrationThread' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		internal class VibrationThread:SupportClass.ThreadClass, IThreadRunnable
		{
			public VibrationThread(TransformManager enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(TransformManager enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private TransformManager enclosingInstance;
			public TransformManager Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			override public void  Run()
			{
				long startTime = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
				long lastRepaintTime = startTime;
				try
				{
					//UPGRADE_ISSUE: Method 'java.lang.Thread.isInterrupted' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangThreadisInterrupted'"
					do 
					{
						long currentTime = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
						int elapsed = (int) (currentTime - lastRepaintTime);
						int sleepTime = 33 - elapsed;
						if (sleepTime > 0)
						{
							//UPGRADE_TODO: Method 'java.lang.Thread.sleep' was converted to 'System.Threading.Thread.Sleep' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangThreadsleep_long'"
							System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64) 10000 * sleepTime));
						}
						//
						lastRepaintTime = currentTime = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
						elapsed = (int) (currentTime - startTime);
						//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
						float t = (float) (elapsed % Enclosing_Instance.vibrationPeriodMs) / Enclosing_Instance.vibrationPeriodMs;
						Enclosing_Instance.VibrationT = t;
						Enclosing_Instance.viewer.refresh();
					}
					while (!isInterrupted());
				}
				catch (System.Threading.ThreadInterruptedException ie)
				{
				}
			}
		}
		
		////////////////////////////////////////////////////////////////
		// stereo support
		////////////////////////////////////////////////////////////////
		
		internal int stereoMode;
		
		internal float stereoDegrees = 5;
		internal float stereoRadians = 5 * radiansPerDegree;
		
		internal bool stereoFrame;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'matrixStereo '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private Matrix3f matrixStereo = new Matrix3f();
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'getStereoRotationMatrix'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		internal virtual Matrix3f getStereoRotationMatrix(bool stereoFrame)
		{
			lock (this)
			{
				this.stereoFrame = stereoFrame;
				if (stereoFrame)
				{
					matrixTemp3.rotY(axesOrientationRasmol?stereoRadians:- stereoRadians);
					matrixStereo.mul(matrixTemp3, matrixRotate);
				}
				else
				{
					matrixStereo.set_Renamed(matrixRotate);
				}
				return matrixStereo;
			}
		}
	}
}