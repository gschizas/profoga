/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-03-27 18:50:20 +0200 (lun., 27 mars 2006) $
* $Revision: 4784 $
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
using org.jmol.api;
using JmolConstants = org.jmol.viewer.JmolConstants;
//UPGRADE_TODO: The type 'javax.vecmath.Point3d' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Point3d = javax.vecmath.Point3d;
//UPGRADE_TODO: The type 'javax.vecmath.Point3f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Point3f = javax.vecmath.Point3f;
//UPGRADE_TODO: The type 'javax.vecmath.Matrix4f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Matrix4f = javax.vecmath.Matrix4f;
namespace org.openscience.jmol.app
{
	
	public class PovraySaver
	{
		
		internal System.IO.StreamWriter bw;
		internal JmolViewer viewer;
		internal bool allModels;
		internal int screenWidth;
		internal int screenHeight;
		
		internal Matrix4f transformMatrix;
		
		public PovraySaver(JmolViewer viewer, System.IO.Stream out_Renamed, bool allModels, int width, int height)
		{
			//UPGRADE_WARNING: At least one expression was used more than once in the target code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1181'"
			this.bw = new System.IO.StreamWriter(new System.IO.StreamWriter(out_Renamed, System.Text.Encoding.Default).BaseStream, new System.IO.StreamWriter(out_Renamed, System.Text.Encoding.Default).Encoding, 8192);
			this.viewer = viewer;
			this.allModels = allModels || (viewer.getDisplayModelIndex() == - 1);
			this.screenWidth = width;
			this.screenHeight = height;
		}
		
		internal virtual void  out_Renamed(System.String str)
		{
			bw.Write(str);
		}
		
		public virtual void  writeFrame()
		{
			float zoom = viewer.RotationRadius * 2;
			zoom *= 1.1f; // for some reason I need a little more margin
			zoom /= viewer.ZoomPercent / 100f;
			
			transformMatrix = viewer.UnscaledTransformMatrix;
			if ((screenWidth <= 0) || (screenHeight <= 0))
			{
				screenWidth = viewer.ScreenWidth;
				screenHeight = viewer.ScreenHeight;
			}
			int minScreenDimension = screenWidth < screenHeight?screenWidth:screenHeight;
			
			System.DateTime now = System.DateTime.Now;
			//UPGRADE_ISSUE: Class 'java.text.SimpleDateFormat' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javatextSimpleDateFormat'"
			//UPGRADE_ISSUE: Constructor 'java.text.SimpleDateFormat.SimpleDateFormat' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javatextSimpleDateFormat'"
			SimpleDateFormat sdf = new SimpleDateFormat("EEE, MMMM dd, yyyy 'at' h:mm aaa");
			
			System.String now_st = SupportClass.FormatDateTime(sdf, now);
			
			out_Renamed("//******************************************************\n");
			out_Renamed("// Jmol generated povray script.\n");
			out_Renamed("//\n");
			out_Renamed("// This script was generated on :\n");
			out_Renamed("// " + now_st + "\n");
			out_Renamed("//******************************************************\n");
			out_Renamed("\n");
			out_Renamed("\n");
			out_Renamed("//******************************************************\n");
			out_Renamed("// Declare the resolution, camera, and light sources.\n");
			out_Renamed("//******************************************************\n");
			out_Renamed("\n");
			out_Renamed("// NOTE: if you plan to render at a different resolution,\n");
			out_Renamed("// be sure to update the following two lines to maintain\n");
			out_Renamed("// the correct aspect ratio.\n" + "\n");
			out_Renamed("#declare Width = " + screenWidth + ";\n");
			out_Renamed("#declare Height = " + screenHeight + ";\n");
			out_Renamed("#declare minScreenDimension = " + minScreenDimension + ";\n");
			out_Renamed("#declare Ratio = Width / Height;\n");
			out_Renamed("#declare zoom = " + zoom + ";\n");
			//    out("#declare wireRadius = 1 / minScreenDimension * zoom;\n");
			out_Renamed("#declare showAtoms = true;\n");
			out_Renamed("#declare showBonds = true;\n");
			out_Renamed("#declare showPolymers = false;\n");
			out_Renamed("camera{\n");
			out_Renamed("  location < 0, 0, zoom>\n" + "\n");
			out_Renamed("  // Ratio is negative to switch povray to\n");
			out_Renamed("  // a right hand coordinate system.\n");
			out_Renamed("\n");
			out_Renamed("  right < -Ratio , 0, 0>\n");
			out_Renamed("  look_at < 0, 0, 0 >\n");
			out_Renamed("}\n");
			out_Renamed("\n");
			
			out_Renamed("background { color " + povrayColor(viewer.getBackgroundArgb()) + " }\n");
			out_Renamed("\n");
			
			out_Renamed("light_source { < 0, 0, zoom> " + " rgb <1.0,1.0,1.0> }\n");
			out_Renamed("light_source { < -zoom, zoom, zoom> " + " rgb <1.0,1.0,1.0> }\n");
			out_Renamed("\n");
			out_Renamed("\n");
			
			out_Renamed("//***********************************************\n");
			out_Renamed("// macros for common shapes\n");
			out_Renamed("//***********************************************\n");
			out_Renamed("\n");
			
			writeMacros();
			
			out_Renamed("//***********************************************\n");
			out_Renamed("// List of all of the atoms\n");
			out_Renamed("//***********************************************\n");
			out_Renamed("\n");
			
			out_Renamed("#if (showAtoms)\n");
			if (allModels)
			{
				out_Renamed("#switch (clock)\n");
				for (int m = 0; m < viewer.ModelCount; m++)
				{
					out_Renamed("#range (" + (m + 0.9) + "," + (m + 1.1) + ")\n");
					for (int i = 0; i < viewer.AtomCount; i++)
					{
						writeAtom(m, i);
					}
					out_Renamed("#break\n");
				}
				out_Renamed("#end\n");
			}
			else
			{
				for (int i = 0; i < viewer.AtomCount; i++)
					writeAtom(viewer.getDisplayModelIndex(), i);
			}
			out_Renamed("#end\n");
			
			out_Renamed("\n");
			out_Renamed("//***********************************************\n");
			out_Renamed("// The list of bonds\n");
			out_Renamed("//***********************************************\n");
			out_Renamed("\n");
			
			out_Renamed("#if (showBonds)\n");
			if (allModels)
			{
				out_Renamed("#switch (clock)\n");
				for (int m = 0; m < viewer.ModelCount; m++)
				{
					out_Renamed("#range (" + (m + 0.9) + "," + (m + 1.1) + ")\n");
					for (int i = 0; i < viewer.BondCount; i++)
					{
						writeBond(m, i);
					}
					out_Renamed("#break\n");
				}
				out_Renamed("#end\n");
			}
			else
			{
				for (int i = 0; i < viewer.BondCount; ++i)
					writeBond(viewer.getDisplayModelIndex(), i);
			}
			out_Renamed("#end\n");
			
			out_Renamed("\n");
			out_Renamed("//***********************************************\n");
			out_Renamed("// The list of polymers\n");
			out_Renamed("//***********************************************\n");
			out_Renamed("\n");
			
			out_Renamed("#if (showPolymers)\n");
			if (allModels)
			{
				out_Renamed("#switch (clock)\n");
				for (int m = 0; m < viewer.ModelCount; m++)
				{
					out_Renamed("#range (" + (m + 0.9) + "," + (m + 1.1) + ")\n");
					for (int i = 0; i < viewer.getPolymerCountInModel(m); i++)
					{
						writePolymer(m, i);
					}
					out_Renamed("#break\n");
				}
				out_Renamed("#end\n");
			}
			else
			{
				for (int i = 0; i < viewer.getPolymerCountInModel(viewer.getDisplayModelIndex()); i++)
				{
					writePolymer(viewer.getDisplayModelIndex(), i);
				}
			}
			out_Renamed("#end\n");
		}
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'writeFile'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public virtual void  writeFile()
		{
			lock (this)
			{
				
				try
				{
					writeFrame();
					bw.Close();
				}
				catch (System.IO.IOException e)
				{
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					System.Console.Out.WriteLine("Got IOException " + e + " trying to write frame.");
				}
			}
		}
		
		/// <summary> Takes a java colour and returns a String representing the
		/// colour in povray eg 'rgb<1.0,0.0,0.0>'
		/// 
		/// </summary>
		/// <param name="argb">The color to convert
		/// 
		/// </param>
		/// <returns> A string representaion of the color in povray rgb format.
		/// </returns>
		protected internal virtual System.String povrayColor(int argb)
		{
			return "rgb<" + getRed(argb) + "," + getGrn(argb) + "," + getBlu(argb) + ">";
		}
		
		internal virtual void  writeMacros()
		{
			out_Renamed("#default { finish {\n" + " ambient .2 diffuse .6 specular 1 roughness .001 metallic}}\n\n");
			writeMacrosAtom();
			//writeMacrosRing();
			//writeMacrosWire();
			//writeMacrosDoubleWire();
			//writeMacrosTripleWire();
			writeMacrosBond();
			writeMacrosDoubleBond();
			writeMacrosTripleBond();
			writeMacrosHydrogenBond();
			writeMacrosAromaticBond();
		}
		internal virtual void  writeMacrosAtom()
		{
			out_Renamed("#macro atom(X,Y,Z,RADIUS,R,G,B)\n" + " sphere{<X,Y,Z>,RADIUS\n" + "  pigment{rgb<R,G,B>}}\n" + "#end\n\n");
		}
		internal virtual void  writeMacrosRing()
		{
			out_Renamed("#macro ring(X,Y,Z,RADIUS,R,G,B)\n" + " torus{RADIUS,wireRadius pigment{rgb<R,G,B>}" + " translate<X,Z,-Y> rotate<90,0,0>}\n" + "#end\n\n");
		}
		internal virtual void  writeMacrosBond()
		{
			out_Renamed("#macro bond1(X1,Y1,Z1,X2,Y2,Z2,RADIUS,R,G,B)\n" + " cylinder{<X1,Y1,Z1>,<X2,Y2,Z2>,RADIUS\n" + "  pigment{rgb<R,G,B>}}\n" + " sphere{<X1,Y1,Z1>,RADIUS\n" + "  pigment{rgb<R,G,B>}}\n" + " sphere{<X2,Y2,Z2>,RADIUS\n" + "  pigment{rgb<R,G,B>}}\n" + "#end\n\n");
			out_Renamed("#macro bond2(X1,Y1,Z1,X2,Y2,Z2,RADIUS,R1,G1,B1,R2,G2,B2)\n" + "#local xc = (X1 + X2) / 2;\n" + "#local yc = (Y1 + Y2) / 2;\n" + "#local zc = (Z1 + Z2) / 2;\n" + " cylinder{<X1,Y1,Z1>,<xc,yc,zc>,RADIUS\n" + "  pigment{rgb<R1,G1,B1>}}\n" + " cylinder{<xc,yc,zc>,<X2,Y2,Z2>,RADIUS\n" + "  pigment{rgb<R2,G2,B2>}}\n" + " sphere{<X1,Y1,Z1>,RADIUS\n" + "  pigment{rgb<R1,G1,B1>}}\n" + " sphere{<X2,Y2,Z2>,RADIUS\n" + "  pigment{rgb<R2,G2,B2>}}\n" + "#end\n\n");
		}
		internal virtual void  writeMacrosDoubleBond()
		{
			out_Renamed("#macro dblbond1(X1,Y1,Z1,X2,Y2,Z2,RADIUS,R,G,B)\n" + "#local dx = X2 - X1;\n" + "#local dy = Y2 - Y1;\n" + "#local mag2d = sqrt(dx*dx + dy*dy);\n" + "#local separation = 3/2 * RADIUS;\n" + "#if (dx + dy)\n" + " #local offX = separation * dy / mag2d;\n" + " #local offY = separation * -dx / mag2d;\n" + "#else\n" + " #local offX = 0;\n" + " #local offY = separation;\n" + "#end\n" + "bond1(X1+offX,Y1+offY,Z1,X2+offX,Y2+offY,Z2,RADIUS,R,G,B)\n" + "bond1(X1-offX,Y1-offY,Z1,X2-offX,Y2-offY,Z2,RADIUS,R,G,B)\n" + "#end\n\n");
			out_Renamed("#macro dblbond2(X1,Y1,Z1,X2,Y2,Z2,RADIUS,R1,G1,B1,R2,G2,B2)\n" + "#local dx = X2 - X1;\n" + "#local dy = Y2 - Y1;\n" + "#local mag2d = sqrt(dx*dx + dy*dy);\n" + "#local separation = 3/2 * RADIUS;\n" + "#if (dx + dy)\n" + " #local offX = separation * dy / mag2d;\n" + " #local offY = separation * -dx / mag2d;\n" + "#else\n" + " #local offX = 0;\n" + " #local offY = separation;\n" + "#end\n" + "bond2(X1+offX,Y1+offY,Z1,X2+offX,Y2+offY,Z2,\n" + "      RADIUS,R1,G1,B1,R2,G2,B2)\n" + "bond2(X1-offX,Y1-offY,Z1,X2-offX,Y2-offY,Z2,\n" + "      RADIUS,R1,G1,B1,R2,G2,B2)\n" + "#end\n\n");
		}
		internal virtual void  writeMacrosTripleBond()
		{
			out_Renamed("#macro trpbond1(X1,Y1,Z1,X2,Y2,Z2,RADIUS,R,G,B)\n" + "#local dx = X2 - X1;\n" + "#local dy = Y2 - Y1;\n" + "#local mag2d = sqrt(dx*dx + dy*dy);\n" + "#local separation = 5/2 * RADIUS;\n" + "#if (dx + dy)\n" + " #local offX = separation * dy / mag2d;\n" + " #local offY = separation * -dx / mag2d;\n" + "#else\n" + " #local offX = 0;\n" + " #local offY = separation;\n" + "#end\n" + "bond1(X1+offX,Y1+offY,Z1,X2+offX,Y2+offY,Z2,RADIUS,R,G,B)\n" + "bond1(X1     ,Y1     ,Z1,X2     ,Y2     ,Z2,RADIUS,R,G,B)\n" + "bond1(X1-offX,Y1-offY,Z1,X2-offX,Y2-offY,Z2,RADIUS,R,G,B)\n" + "#end\n\n");
			out_Renamed("#macro trpbond2(X1,Y1,Z1,X2,Y2,Z2,RADIUS,R1,G1,B1,R2,G2,B2)\n" + "#local dx = X2 - X1;\n" + "#local dy = Y2 - Y1;\n" + "#local mag2d = sqrt(dx*dx + dy*dy);\n" + "#local separation = 5/2 * RADIUS;\n" + "#if (dx + dy)\n" + " #local offX = separation * dy / mag2d;\n" + " #local offY = separation * -dx / mag2d;\n" + "#else\n" + " #local offX = 0;\n" + " #local offY = separation;\n" + "#end\n" + "bond2(X1+offX,Y1+offY,Z1,X2+offX,Y2+offY,Z2,\n" + "      RADIUS,R1,G1,B1,R2,G2,B2)\n" + "bond2(X1     ,Y1     ,Z1,X2     ,Y2     ,Z2,\n" + "      RADIUS,R1,G1,B1,R2,G2,B2)\n" + "bond2(X1-offX,Y1-offY,Z1,X2-offX,Y2-offY,Z2,\n" + "      RADIUS,R1,G1,B1,R2,G2,B2)\n" + "#end\n\n");
		}
		internal virtual void  writeMacrosHydrogenBond()
		{
			out_Renamed("#macro hbond1(X1,Y1,Z1,X2,Y2,Z2,RADIUS,R,G,B)\n" + "#local dx = (X2 - X1) / 10;\n" + "#local dy = (Y2 - Y1) / 10;\n" + "#local dz = (Z2 - Z1) / 10;\n" + " cylinder{<X1+dx  ,Y1+dy  ,Z1+dz  >,<X1+3*dx,Y1+3*dy,Z1+3*dz>,RADIUS\n" + "  pigment{rgb<R,G,B>}}\n" + " cylinder{<X1+4*dx,Y1+4*dy,Z1+4*dz>,<X2-4*dx,Y2-4*dy,Z2-4*dz>,RADIUS\n" + "  pigment{rgb<R,G,B>}}\n" + " cylinder{<X2-3*dx,Y2-3*dy,Z2-3*dz>,<X2-dx  ,Y2-dy  ,Z2-dz  >,RADIUS\n" + "  pigment{rgb<R,G,B>}}\n" + "#end\n\n");
			out_Renamed("#macro hbond2(X1,Y1,Z1,X2,Y2,Z2,RADIUS,R1,G1,B1,R2,G2,B2)\n" + "#local dx = (X2 - X1) / 10;\n" + "#local dy = (Y2 - Y1) / 10;\n" + "#local dz = (Z2 - Z1) / 10;\n" + "#local xc = (X1 + X2) / 2;\n" + "#local yc = (Y1 + Y2) / 2;\n" + "#local zc = (Z1 + Z2) / 2;\n" + " cylinder{<X1+dx  ,Y1+dy  ,Z1+dz  >,<X1+3*dx,Y1+3*dy,Z1+3*dz>,RADIUS\n" + "  pigment{rgb<R1,G1,B1>}}\n" + " cylinder{<X1+4*dx,Y1+4*dy,Z1+4*dz>,<xc     ,yc     ,zc     >,RADIUS\n" + "  pigment{rgb<R1,G1,B1>}}\n" + " cylinder{<xc     ,yc     ,zc     >,<X2-4*dx,Y2-4*dy,Z2-4*dz>,RADIUS\n" + "  pigment{rgb<R2,G2,B2>}}\n" + " cylinder{<X2-3*dx,Y2-3*dy,Z2-3*dz>,<X2-dx  ,Y2-dy  ,Z2-dz  >,RADIUS\n" + "  pigment{rgb<R2,G2,B2>}}\n" + "#end\n\n");
		}
		internal virtual void  writeMacrosAromaticBond()
		{
			out_Renamed("#macro abond1(X1,Y1,Z1,X2,Y2,Z2,RADIUS,R,G,B)\n" + "#local dx = (X2 - X1) / 12;\n" + "#local dy = (Y2 - Y1) / 12;\n" + "#local dz = (Z2 - Z1) / 12;\n" + "#local mag2d = sqrt(dx*dx + dy*dy);\n" + "#local separation = 3/2 * RADIUS;\n" + "#if (dx + dy)\n" + " #local offX = separation * dy / mag2d;\n" + " #local offY = separation * -dx / mag2d;\n" + "#else\n" + " #local offX = 0;\n" + " #local offY = separation;\n" + "#end\n" + " bond1(X1+offX,Y1+offY,Z1,X2+offX,Y2+offY,Z2,RADIUS,R,G,B)\n" + " cylinder{<X1-offX+2*dx,Y1-offY+2*dy,Z1+2*dz>,<X1-offX+5*dx,Y1-offY+5*dy,Z1+5*dz>,RADIUS\n" + "  pigment{rgb<R,G,B>}}" + " cylinder{<X2-offX-2*dx,Y2-offY-2*dy,Z2-2*dz>,<X2-offX-5*dx,Y2-offY-5*dy,Z2-5*dz>,RADIUS\n" + "  pigment{rgb<R,G,B>}}" + "#end\n\n");
			out_Renamed("#macro abond2(X1,Y1,Z1,X2,Y2,Z2,RADIUS,R1,G1,B1,R2,G2,B2)\n" + "#local dx = (X2 - X1) / 12;\n" + "#local dy = (Y2 - Y1) / 12;\n" + "#local dz = (Z2 - Z1) / 12;\n" + "#local mag2d = sqrt(dx*dx + dy*dy);\n" + "#local separation = 3/2 * RADIUS;\n" + "#if (dx + dy)\n" + " #local offX = separation * dy / mag2d;\n" + " #local offY = separation * -dx / mag2d;\n" + "#else\n" + " #local offX = 0;\n" + " #local offY = separation;\n" + "#end\n" + " bond2(X1+offX,Y1+offY,Z1,X2+offX,Y2+offY,Z2,RADIUS,R1,G1,B1,R2,G2,B2)\n" + " cylinder{<X1-offX+2*dx,Y1-offY+2*dy,Z1+2*dz>,<X1-offX+3.5*dx,Y1-offY+3.5*dy,Z1+3.5*dz>,RADIUS\n" + "  pigment{rgb<R1,G1,B1>}}" + " cylinder{<X1-offX+5*dx,Y1-offY+5*dy,Z1+5*dz>,<X1-offX+3.5*dx,Y1-offY+3.5*dy,Z1+3.5*dz>,RADIUS\n" + "  pigment{rgb<R2,G2,B2>}}" + " cylinder{<X2-offX-5*dx,Y2-offY-5*dy,Z2-5*dz>,<X2-offX-3.5*dx,Y2-offY-3.5*dy,Z2-3.5*dz>,RADIUS\n" + "  pigment{rgb<R1,G1,B1>}}" + " cylinder{<X2-offX-2*dx,Y2-offY-2*dy,Z2-2*dz>,<X2-offX-3.5*dx,Y2-offY-3.5*dy,Z2-3.5*dz>,RADIUS\n" + "  pigment{rgb<R2,G2,B2>}}" + "#end\n\n");
		}
		internal virtual void  writeMacrosWire()
		{
			out_Renamed("#macro wire1(X1,Y1,Z1,X2,Y2,Z2,RADIUS,R,G,B)\n" + " cylinder{<X1,Y1,Z1>,<X2,Y2,Z2>,wireRadius\n" + "  pigment{rgb<R,G,B>}}\n" + "#end\n\n");
			out_Renamed("#macro wire2(X1,Y1,Z1,X2,Y2,Z2,RADIUS,R1,G1,B1,R2,G2,B2)\n" + "#local xc = (X1 + X2) / 2;\n" + "#local yc = (Y1 + Y2) / 2;\n" + "#local zc = (Z1 + Z2) / 2;\n" + " cylinder{<X1,Y1,Z1>,<xc,yc,zc>,wireRadius\n" + "  pigment{rgb<R1,G1,B1>}}\n" + " cylinder{<xc,yc,zc>,<X2,Y2,Z2>,wireRadius\n" + "  pigment{rgb<R2,G2,B2>}}\n" + "#end\n\n");
		}
		internal virtual void  writeMacrosDoubleWire()
		{
			out_Renamed("#macro dblwire1(X1,Y1,Z1,X2,Y2,Z2,RADIUS,R,G,B)\n" + "#local dx = X2 - X1;\n" + "#local dy = Y2 - Y1;\n" + "#local mag2d = sqrt(dx*dx + dy*dy);\n" + "#local separation = 3/2 * RADIUS;\n" + "#if (dx + dy)\n" + " #local offX = separation * dy / mag2d;\n" + " #local offY = separation * -dx / mag2d;\n" + "#else\n" + " #local offX = 0;\n" + " #local offY = separation;\n" + "#end\n" + "wire1(X1+offX,Y1+offY,Z1,X2+offX,Y2+offY,Z2,RADIUS,R,G,B)\n" + "wire1(X1-offX,Y1-offY,Z1,X2-offX,Y2-offY,Z2,RADIUS,R,G,B)\n" + "#end\n\n");
			out_Renamed("#macro dblwire2(X1,Y1,Z1,X2,Y2,Z2," + "RADIUS,R1,G1,B1,R2,G2,B2)\n" + "#local dx = X2 - X1;\n" + "#local dy = Y2 - Y1;\n" + "#local mag2d = sqrt(dx*dx + dy*dy);\n" + "#local separation = 3/2 * RADIUS;\n" + "#if (dx + dy)\n" + " #local offX = separation * dy / mag2d;\n" + " #local offY = separation * -dx / mag2d;\n" + "#else\n" + " #local offX = 0;\n" + " #local offY = separation;\n" + "#end\n" + "wire2(X1+offX,Y1+offY,Z1,X2+offX,Y2+offY,Z2,\n" + "      RADIUS,R1,G1,B1,R2,G2,B2)\n" + "wire2(X1-offX,Y1-offY,Z1,X2-offX,Y2-offY,Z2,\n" + "      RADIUS,R1,G1,B1,R2,G2,B2)\n" + "#end\n\n");
		}
		internal virtual void  writeMacrosTripleWire()
		{
			out_Renamed("#macro trpwire1(X1,Y1,Z1,X2,Y2,Z2,RADIUS,R,G,B)\n" + "#local dx = X2 - X1;\n" + "#local dy = Y2 - Y1;\n" + "#local mag2d = sqrt(dx*dx + dy*dy);\n" + "#local separation = 5/2 * RADIUS;\n" + "#if (dx + dy)\n" + " #local offX = separation * dy / mag2d;\n" + " #local offY = separation * -dx / mag2d;\n" + "#else\n" + " #local offX = 0;\n" + " #local offY = separation;\n" + "#end\n" + "wire1(X1+offX,Y1+offY,Z1,X2+offX,Y2+offY,Z2,RADIUS,R,G,B)\n" + "wire1(X1     ,Y1     ,Z1,X2     ,Y2     ,Z2,RADIUS,R,G,B)\n" + "wire1(X1-offX,Y1-offY,Z1,X2-offX,Y2-offY,Z2,RADIUS,R,G,B)\n" + "#end\n\n");
			out_Renamed("#macro trpwire2(X1,Y1,Z1,X2,Y2,Z2," + "RADIUS,R1,G1,B1,R2,G2,B2)\n" + "#local dx = X2 - X1;\n" + "#local dy = Y2 - Y1;\n" + "#local mag2d = sqrt(dx*dx + dy*dy);\n" + "#local separation = 5/2 * RADIUS;\n" + "#if (dx + dy)\n" + " #local offX = separation * dy / mag2d;\n" + " #local offY = separation * -dx / mag2d;\n" + "#else\n" + " #local offX = 0;\n" + " #local offY = separation;\n" + "#end\n" + "wire2(X1+offX,Y1+offY,Z1,X2+offX,Y2+offY,Z2,\n" + "      RADIUS,R1,G1,B1,R2,G2,B2)\n" + "wire2(X1     ,Y1     ,Z1,X2     ,Y2     ,Z2,\n" + "      RADIUS,R1,G1,B1,R2,G2,B2)\n" + "wire2(X1-offX,Y1-offY,Z1,X2-offX,Y2-offY,Z2,\n" + "      RADIUS,R1,G1,B1,R2,G2,B2)\n" + "#end\n\n");
		}
		
		internal Point3f point1 = new Point3f();
		internal Point3f point2 = new Point3f();
		internal Point3f pointC = new Point3f();
		
		internal virtual void  writeAtom(int modelIndex, int i)
		{
			int model = viewer.getAtomModelIndex(i);
			if (model != modelIndex)
			{
				return ;
			}
			float radius = (float) viewer.getAtomRadius(i);
			if (radius == 0)
				return ;
			transformMatrix.transform(viewer.getAtomPoint3f(i), point1);
			float x = (float) point1.x;
			float y = (float) point1.y;
			float z = (float) point1.z;
			int argb = viewer.getAtomArgb(i);
			float r = getRed(argb);
			float g = getGrn(argb);
			float b = getBlu(argb);
			out_Renamed("atom(" + x + "," + y + "," + z + "," + radius + "," + r + "," + g + "," + b + ")\n");
		}
		
		internal virtual void  writeBond(int modelIndex, int i)
		{
			int model = viewer.getBondModelIndex(i);
			if (model != modelIndex)
			{
				return ;
			}
			float radius = (float) viewer.getBondRadius(i);
			if (radius == 0)
				return ;
			transformMatrix.transform(viewer.getBondPoint3f1(i), point1);
			float x1 = (float) point1.x;
			float y1 = (float) point1.y;
			float z1 = (float) point1.z;
			transformMatrix.transform(viewer.getBondPoint3f2(i), point2);
			float x2 = (float) point2.x;
			float y2 = (float) point2.y;
			float z2 = (float) point2.z;
			int argb1 = viewer.getBondArgb1(i);
			int argb2 = viewer.getBondArgb2(i);
			float r1 = getRed(argb1);
			float g1 = getGrn(argb1);
			float b1 = getBlu(argb1);
			int order = viewer.getBondOrder(i);
			
			switch (order)
			{
				
				case 1: 
					out_Renamed("bond");
					break;
				
				
				case 2: 
					out_Renamed("dblbond");
					break;
				
				
				case 3: 
					out_Renamed("trpbond");
					break;
				
				
				case JmolConstants.BOND_AROMATIC: 
					//out("bond");
					//TODO: Render aromatic bond as in Jmol : a full cylinder and a dashed cylinder
					// The problem is to place correctly the two cylinders !
					out_Renamed("abond");
					break;
				
				
				default: 
					if ((order & JmolConstants.BOND_HYDROGEN_MASK) != 0)
					{
						out_Renamed("hbond");
					}
					else
					{
						return ;
					}
					break;
				
			}
			
			out_Renamed(argb1 == argb2?"1":"2");
			out_Renamed("(");
			out_Renamed(x1 + "," + y1 + "," + z1 + ",");
			out_Renamed(x2 + "," + y2 + "," + z2 + ",");
			out_Renamed(radius + ",");
			out_Renamed(r1 + "," + g1 + "," + b1);
			if (argb1 != argb2)
			{
				float r2 = getRed(argb2);
				float g2 = getGrn(argb2);
				float b2 = getBlu(argb2);
				out_Renamed("," + r2 + "," + g2 + "," + b2);
			}
			out_Renamed(")\n");
		}
		
		internal virtual void  writePolymer(int modelIndex, int i)
		{
			Point3f[] points = viewer.getPolymerLeadMidPoints(modelIndex, i);
			Point3f[] controls = computeControlPoints(points);
			if (controls != null)
			{
				out_Renamed("sphere_sweep {\n");
				out_Renamed(" b_spline\n");
				out_Renamed(" " + controls.length + "\n");
				for (int j = 0; j < controls.length; j++)
				{
					Point3f point = controls[j];
					transformMatrix.transform(point, point1);
					double d = 0.2; //TODO
					out_Renamed(" <" + point1.x + "," + point1.y + "," + point1.z + ">," + d + "\n");
				}
				System.Drawing.Color color = Color.BLUE; //TODO
				//UPGRADE_TODO: The equivalent in .NET for method 'java.awt.Color.getRed' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				float r = (int) color.R / 255f;
				//UPGRADE_TODO: The equivalent in .NET for method 'java.awt.Color.getGreen' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				float g = (int) color.G / 255f;
				//UPGRADE_TODO: The equivalent in .NET for method 'java.awt.Color.getBlue' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				float b = (int) color.B / 255f;
				out_Renamed(" pigment{rgb<" + r + "," + g + "," + b + ">}\n");
				out_Renamed("}\n");
			}
		}
		
		/// <summary> Computes the control points of a b-spline that goes through a set of points
		/// 
		/// </summary>
		/// <param name="path">Set of n points to go through
		/// </param>
		/// <returns> Set of n+2 control points
		/// </returns>
		internal virtual Point3f[] computeControlPoints(Point3f[] path)
		{
			// NOTE :
			// I digged this code out from a program I wrote more than ten years ago
			// The program was in C++ and with not many comments
			// I don't remember well how it is working
			// Hence the lack of explanations in the code :-)
			Point3f[] controls = null;
			if ((path != null) && (path.length >= 2))
			{
				
				// Determine if it is a closed loop
				int length = path.length;
				bool loop = (path[0].x == path[length - 1].x) && (path[0].y == path[length - 1].y) && (path[0].z == path[length - 1].z);
				
				if (!loop)
				{
					if (length > 2)
					{
						
						// Create vectors for computations
						Point3d[] values = new Point3d[length + 2];
						Point3d[] results1 = new Point3d[length + 2];
						Point3d[] results2 = new Point3d[length + 2];
						
						// Initialize vectors for computations
						values[0] = new Point3d(path[0]);
						for (int i = 0; i < length; i++)
						{
							values[i + 1] = new Point3d(6 * path[i].x, 6 * path[i].y, 6 * path[i].z);
						}
						values[length + 1] = new Point3d(values[length].x / 6.0, values[length].y / 6.0, values[length].z / 6.0);
						
						for (int i = 0; i < length + 2; i++)
						{
							results1[i] = new Point3d(0, 0, 0);
							results2[i] = new Point3d(0, 0, 0);
						}
						results2[0].set_Renamed(1, 1, 1);
						results1[1].set_Renamed(values[0]);
						
						// Computation of control points
						for (int i = 2; i < length + 2; i++)
						{
							results1[i].x = values[i - 1].x - 4 * results1[i - 1].x - results1[i - 2].x;
							results1[i].y = values[i - 1].y - 4 * results1[i - 1].y - results1[i - 2].y;
							results1[i].z = values[i - 1].z - 4 * results1[i - 1].z - results1[i - 2].z;
							results2[i].x = (- 4) * results2[i - 1].x - results2[i - 2].x;
							results2[i].y = (- 4) * results2[i - 1].y - results2[i - 2].y;
							results2[i].z = (- 4) * results2[i - 1].z - results2[i - 2].z;
						}
						double xA = (values[length + 1].x - results1[length].x) / results2[length].x;
						double yA = (values[length + 1].y - results1[length].y) / results2[length].y;
						double zA = (values[length + 1].z - results1[length].z) / results2[length].z;
						
						// Creation of the control points array
						Point3f[] points = new Point3f[length + 2];
						for (int i = 0; i < length + 2; i++)
						{
							points[i] = new Point3f((float) (results1[i].x + xA * results2[i].x), (float) (results1[i].y + yA * results2[i].y), (float) (results1[i].z + zA * results2[i].z));
						}
						controls = points;
					}
					else
					{
						Point3f[] points = new Point3f[length + 2];
						points[0] = new Point3f(2 * path[0].x - path[1].x, 2 * path[0].y - path[1].y, 2 * path[0].z - path[1].z);
						points[1] = new Point3f(path[0]);
						points[2] = new Point3f(path[1]);
						points[3] = new Point3f(2 * path[1].x - path[0].x, 2 * path[1].y - path[0].y, 2 * path[1].z - path[0].z);
						controls = points;
					}
				}
				else
				{
					if (length > 3)
					{
						
						// Create vectors for computations
						Point3d[] values = new Point3d[length + 2];
						Point3d[] results1 = new Point3d[length + 2];
						Point3d[] results2 = new Point3d[length + 2];
						Point3d[] results3 = new Point3d[length + 2];
						Point3f[] points = new Point3f[length + 2];
						
						// Initialize vectors for computations
						for (int i = 0; i < length - 1; i++)
						{
							values[i] = new Point3d(6 * path[i].x, 6 * path[i].y, 6 * path[i].z);
							points[i] = new Point3f(0, 0, 0);
							results1[i] = new Point3d(0, 0, 0);
							results2[i] = new Point3d(0, 0, 0);
							results3[i] = new Point3d(0, 0, 0);
						}
						results2[0].set_Renamed(1, 1, 1);
						results3[0].set_Renamed(1, 1, 1);
						
						// Computation of control points
						for (int i = 2; i < length - 1; i++)
						{
							results1[i].x = values[i - 2].x - results1[i - 2].x - 4 * results1[i - 1].x;
							results1[i].y = values[i - 2].y - results1[i - 2].y - 4 * results1[i - 1].y;
							results1[i].z = values[i - 2].z - results1[i - 2].z - 4 * results1[i - 1].z;
							results2[i].x = - results2[i - 2].x - 4 * results2[i - 1].x;
							results2[i].y = - results2[i - 2].y - 4 * results2[i - 1].y;
							results2[i].z = - results2[i - 2].z - 4 * results2[i - 1].z;
							results3[i].x = - results3[i - 2].x - 4 * results3[i - 1].x;
							results3[i].y = - results3[i - 2].y - 4 * results3[i - 1].y;
							results3[i].z = - results3[i - 2].z - 4 * results3[i - 1].z;
						}
						double ax1 = 1 + results2[length - 3].x + 4 * results2[length - 2].x;
						double ay1 = 1 + results2[length - 3].y + 4 * results2[length - 2].y;
						double az1 = 1 + results2[length - 3].z + 4 * results2[length - 2].z;
						double ax2 = 4 + results2[length - 2].x;
						double ay2 = 4 + results2[length - 2].y;
						double az2 = 4 + results2[length - 2].z;
						double bx1 = 1 + results3[length - 3].x + 4 * results3[length - 2].x;
						double by1 = 1 + results3[length - 3].y + 4 * results3[length - 2].y;
						double bz1 = 1 + results3[length - 3].z + 4 * results3[length - 2].z;
						double bx2 = 1 + results3[length - 2].x;
						double by2 = 1 + results3[length - 2].y;
						double bz2 = 1 + results3[length - 2].z;
						double cx1 = values[length - 3].x - results1[length - 3].x - 4 * results1[length - 2].x;
						double cy1 = values[length - 3].y - results1[length - 3].y - 4 * results1[length - 2].y;
						double cz1 = values[length - 3].z - results1[length - 3].z - 4 * results1[length - 2].z;
						double cx2 = values[length - 2].x - results1[length - 2].x;
						double cy2 = values[length - 2].y - results1[length - 2].y;
						double cz2 = values[length - 2].z - results1[length - 2].z;
						//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
						points[0].set_Renamed((float) ((cx1 * bx2 - cx2 * bx1) / (ax1 * bx2 - ax2 * bx1)), (float) ((cy1 * by2 - cy2 * by1) / (ay1 * by2 - ay2 * by1)), (float) ((cz1 * bz2 - cz2 * bz1) / (az1 * bz2 - az2 * bz1)));
						//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
						points[1].set_Renamed((float) ((cx1 * ax2 - cx2 * ax1) / (ax2 * bx1 - ax1 * bx2)), (float) ((cy1 * ay2 - cy2 * ay1) / (ay2 * by1 - ay1 * by2)), (float) ((cz1 * az2 - cz2 * az1) / (az2 * bz1 - az1 * bz2)));
						for (int i = 2; i < length - 1; i++)
						{
							points[i].set_Renamed((float) (results1[i].x + results2[i].x * points[0].x + results3[i].x * points[1].x), (float) (results1[i].y + results2[i].y * points[0].y + results3[i].y * points[1].y), (float) (results1[i].z + results2[i].z * points[0].z + results3[i].z * points[1].z));
						}
						points[length - 1] = new Point3f(points[0]);
						points[length] = new Point3f(points[1]);
						points[length + 1] = new Point3f(points[2]);
						controls = points;
					}
					else
					{
						Point3f[] points = new Point3f[4];
						points[0] = new Point3f(2 * path[0].x - path[1].x, 2 * path[0].y - path[1].y, 2 * path[0].z - path[1].z);
						points[1] = new Point3f(path[0]);
						points[2] = new Point3f(path[1]);
						points[3] = new Point3f(2 * path[1].x - path[0].x, 2 * path[1].y - path[0].y, 2 * path[1].z - path[0].z);
						controls = points;
					}
				}
			}
			return controls;
		}
		
		internal virtual float getRed(int argb)
		{
			return ((argb >> 16) & 0xFF) / 255f;
		}
		
		internal virtual float getGrn(int argb)
		{
			return ((argb >> 8) & 0xFF) / 255f;
		}
		
		internal virtual float getBlu(int argb)
		{
			return (argb & 0xFF) / 255f;
		}
	}
}