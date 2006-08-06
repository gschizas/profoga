/*
* File Name   : ZoneVisu.java
* Description : All visual graphing elements and functionality is localized in this class
* Author      : Shravan Sadasivan
* Organization: Department Of Chemistry,
*               The Ohio State University
*/
using System;
using org.jmol.jcamp.utils;
namespace org.jmol.jcamp
{
	[Serializable]
	public class ZoneVisu:System.Windows.Forms.Control
	{
		virtual public GraphCharacteristics GraphDataUtils
		{
			set
			{
				this._graphDataUtils = value;
			}
			
		}
		/// <summary> Method to set the color of the graphics output</summary>
		private System.String GraphicsColor
		{
			set
			{
				if (value.ToUpper().Equals("RED".ToUpper()))
				{
					SupportClass.GraphicsManager.manager.SetColor(BufGra, System.Drawing.Color.Red);
				}
				else if (value.ToUpper().Equals("ORANGE".ToUpper()))
				{
					SupportClass.GraphicsManager.manager.SetColor(BufGra, System.Drawing.Color.Orange);
				}
				else if (value.ToUpper().Equals("BLACK".ToUpper()))
				{
					SupportClass.GraphicsManager.manager.SetColor(BufGra, System.Drawing.Color.Black);
				}
				else if (value.ToUpper().Equals("BLUE".ToUpper()))
				{
					SupportClass.GraphicsManager.manager.SetColor(BufGra, System.Drawing.Color.Blue);
				}
				else if (value.ToUpper().Equals("GREEN".ToUpper()))
				{
					SupportClass.GraphicsManager.manager.SetColor(BufGra, System.Drawing.Color.Green);
				}
				else if (value.ToUpper().Equals("DARKGREEN".ToUpper()))
				{
					SupportClass.GraphicsManager.manager.SetColor(BufGra, System.Drawing.Color.FromArgb(0, 100, 0));
				}
				else if (value.ToUpper().Equals("LIME".ToUpper()))
				{
					SupportClass.GraphicsManager.manager.SetColor(BufGra, System.Drawing.Color.FromArgb(0, 255, 0));
				}
				else if (value.ToUpper().Equals("NAVY".ToUpper()))
				{
					SupportClass.GraphicsManager.manager.SetColor(BufGra, System.Drawing.Color.FromArgb(0, 0, 102));
				}
				else if (value.ToUpper().Equals("DARKRED".ToUpper()))
				{
					SupportClass.GraphicsManager.manager.SetColor(BufGra, System.Drawing.Color.FromArgb(128, 0, 0));
				}
				else if (value.ToUpper().Equals("MAGENTA".ToUpper()))
				{
					SupportClass.GraphicsManager.manager.SetColor(BufGra, System.Drawing.Color.FromArgb(255, 0, 255));
				}
				else if (value.ToUpper().Equals("PURPLE".ToUpper()))
				{
					SupportClass.GraphicsManager.manager.SetColor(BufGra, System.Drawing.Color.FromArgb(128, 0, 128));
				}
				else if (value.ToUpper().Equals("YELLOW".ToUpper()))
				{
					SupportClass.GraphicsManager.manager.SetColor(BufGra, System.Drawing.Color.FromArgb(255, 255, 0));
				}
				else
				{
					SupportClass.GraphicsManager.manager.SetColor(BufGra, System.Drawing.Color.Black);
				}
			}
			
		}
		private GraphCharacteristics _graphDataUtils;
		// -Variables to fill in values from the dx files
		// -Each variable in this section has a corresponding value in the *.dx file being loaded
		public double Firstx;
		public double Lastx;
		public double Miny;
		public double Maxy;
		public double YFactor;
		public int xLowerLimit = 0;
		public int xUpperLimit = 0;
		public int nbLignes;
		public int Nbpoints;
		public System.String TexteTitre;
		public System.String x_units;
		public System.String y_units;
		public int typedata;
		public bool reverse = false;
		// -Values of the variables converted to real values
		internal double RealFirstx;
		internal double RealLastx;
		internal double Last_RealFirstx;
		internal double Last_RealLastx;
		internal double Last_Firstx;
		internal double Last_Lastx;
		internal double Sav_Firstx;
		internal double Sav_Lastx;
		internal bool Flag_Clickable_Peaks = false;
		internal int Nb_Clickable_Peaks;
		internal double[] Peak_Start;
		internal double[] Peak_Stop;
		internal System.String[] Peak_Html;
		internal bool Flag_Load_Now_Html = false;
		internal System.String Name_Load_Now_Html;
		internal int Sav_Nbpoints_a_tracer;
		internal int Nbpoints_a_tracer;
		internal int prempoint;
		internal System.String Y_Values;
		internal System.String ShowTitle;
		internal int shitty_starting_constant;
		internal double[] tableau_points;
		internal double[] tableau_integrate;
		internal double Incrx; // Only used for the XYDATAS
		internal double Multx; // Only used for the PEAK TABLES and FIND PEAK
		internal double Multy;
		internal double Maxintegrate; // Only used for Integrate
		internal double x_Renamed_Field;
		internal double y;
		internal double ax;
		internal double ay;
		internal double xd;
		internal double xf;
		internal int largeur; //Width of the ZoneVisua component
		internal int hauteur; //Height of the ZoneVisua component
		internal int largeur_gfx; // Constant based on the value of the width of the canvas
		internal int hauteur_gfx; // Constant based on the value of the height of the canvas
		internal int decalx_gfx; // Border width
		internal int decaly_gfx; // Border Height
		internal int hauteur_bandeau;
		internal bool Flag_Grid = false;
		internal bool Flag_Reverse = false;
		internal bool Flag_Integrate = false;
		internal int location_textetitre;
		internal System.Drawing.Image BufImg;
		internal System.Drawing.Graphics BufGra;
		internal System.Drawing.Image SavBufImg;
		internal System.Drawing.Graphics SavBufGra;
		internal System.Drawing.Image ZoomBufImg;
		internal System.Drawing.Graphics ZoomBufGra;
		internal int x1_zoom;
		internal int x2_zoom;
		internal int indice;
		internal System.String Un_Nombre;
		public virtual void  init()
		{
			largeur = Size.Width; //Width of the component
			hauteur = Size.Height; //Height of the component
			BufImg = new System.Drawing.Bitmap(largeur, hauteur); //Create an image of the width and height of the ZoneVisua component
			BufGra = System.Drawing.Graphics.FromImage(BufImg);
			// Filling the color to the drawing area
			SupportClass.GraphicsManager.manager.SetColor(BufGra, System.Drawing.Color.White);
			BufGra.FillRectangle(SupportClass.GraphicsManager.manager.GetPaint(BufGra), 1, 1, largeur - 2, hauteur - 2);
			// Filling the color to the border area of thickness 2X2
			SupportClass.GraphicsManager.manager.SetColor(BufGra, System.Drawing.Color.Black);
			BufGra.DrawRectangle(SupportClass.GraphicsManager.manager.GetPen(BufGra), 0, 0, largeur - 1, hauteur - 1);
			//UPGRADE_NOTE: If the given Font Name does not exist, a default Font instance is created. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1075'"
			//UPGRADE_TODO: Field 'java.awt.Font.PLAIN' was converted to 'System.Drawing.FontStyle.Regular' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtFontPLAIN_f'"
			SupportClass.GraphicsManager.manager.SetFont(BufGra, new System.Drawing.Font("TimesRoman", 12, System.Drawing.FontStyle.Regular));
			hauteur_gfx = (hauteur * 3) / 4; // Some random calculation for a constant based on the height of the canvas
			/*
			Shravan Sadasivan - Code present to check if the Y Values should be displayed on the screen
			Modification required
			*/
			if (Y_Values != null && String.CompareOrdinal(Y_Values, "HIDE") == 0)
			{
				decalx_gfx = 5;
				largeur_gfx = largeur - decalx_gfx - 10;
			}
			else
			{
				decalx_gfx = 60;
				largeur_gfx = largeur - decalx_gfx - 15;
			}
			decaly_gfx = hauteur / 8;
			location_textetitre = 125 + decalx_gfx / 2;
			hauteur_bandeau = hauteur - hauteur_gfx - decaly_gfx; //Height of the on screen text elements
			x1_zoom = decalx_gfx;
			x2_zoom = decalx_gfx + largeur_gfx;
			//x1_zoom = xLowerLimit;
			//x2_zoom = xUpperLimit;
			SavBufImg = new System.Drawing.Bitmap(largeur, hauteur);
			SavBufGra = System.Drawing.Graphics.FromImage(SavBufImg);
			ZoomBufImg = new System.Drawing.Bitmap(largeur, hauteur); //Variables required for zooming into the graph
			ZoomBufGra = System.Drawing.Graphics.FromImage(ZoomBufImg); //Variables required for zooming into the graph
		}
		/// <summary> </summary>
		public virtual void  Draw_Texte(System.String tam)
		{
			SupportClass.GraphicsManager.manager.SetColor(BufGra, System.Drawing.Color.White);
			BufGra.FillRectangle(SupportClass.GraphicsManager.manager.GetPaint(BufGra), 20, hauteur - decaly_gfx / 6 - 10, (largeur * 3) / 4, 14);
			if (_graphDataUtils.AxisTextColor == null)
			{
				SupportClass.GraphicsManager.manager.SetColor(BufGra, System.Drawing.Color.FromArgb(255, 60, 120));
			}
			else
			{
				GraphicsColor = _graphDataUtils.AxisTextColor;
			}
			//UPGRADE_TODO: Method 'java.awt.Graphics.drawString' was converted to 'System.Drawing.Graphics.DrawString' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphicsdrawString_javalangString_int_int'"
			BufGra.DrawString(tam, SupportClass.GraphicsManager.manager.GetFont(BufGra), SupportClass.GraphicsManager.manager.GetBrush(BufGra), 20, hauteur - decaly_gfx / 6 - SupportClass.GraphicsManager.manager.GetFont(BufGra).GetHeight());
			//UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
			Refresh();
		}
		public virtual void  Init_File()
		{
			nbLignes = ((Visua) Parent).nbLignes;
			Firstx = ((Visua) Parent).Firstx;
			Lastx = ((Visua) Parent).Lastx;
			YFactor = ((Visua) Parent).YFactor;
			Nbpoints = ((Visua) Parent).Nbpoints;
			TexteTitre = ((Visua) Parent).TexteTitre;
			x_units = ((Visua) Parent).x_units;
			y_units = ((Visua) Parent).y_units;
			if (String.CompareOrdinal(((Visua) Parent).Datatype, "XYDATA") == 0)
				typedata = 0;
			if (String.CompareOrdinal(((Visua) Parent).Datatype, "PEAK TABLE") == 0)
				typedata = 1;
			shitty_starting_constant = ((Visua) Parent).shitty_starting_constant;
			Flag_Integrate = false;
			RealFirstx = Firstx;
			RealLastx = Lastx;
			Last_RealFirstx = Firstx;
			Last_RealLastx = Lastx;
			Sav_Firstx = Firstx;
			Sav_Lastx = Lastx;
			Determine_Extrem_y();
			Integrate();
		}
		//Shravan Sadasivan
		//Apply changes to this section of the code for zooming in on a portion of the graph
		/// <summary> </summary>
		public virtual void  Determine_Extrem_y()
		{
			
			if (typedata == 0)
			{
				// XYDATA
				Miny = tableau_points[0];
				Maxy = tableau_points[0];
				for (int i = 0; i < Nbpoints; i++)
				{
					if (tableau_points[i] < Miny)
						Miny = tableau_points[i];
					if (tableau_points[i] > Maxy)
						Maxy = tableau_points[i];
				}
				Miny = Miny * YFactor;
				Maxy = Maxy * YFactor;
			}
			
			if (typedata == 1)
			{
				// PEAK TABLE
				Miny = 0;
				Maxy = tableau_points[1];
				for (int i = 1; i < Nbpoints; i++)
				{
					if (tableau_points[i * 2 + 1] > Maxy)
						Maxy = tableau_points[i * 2 + 1];
				}
			}
		}
		/// <summary> </summary>
		/// <param name="tam">
		/// </param>
		/// <returns> String
		/// </returns>
		public virtual System.String Reduce_String_EndBlanks(System.String tam)
		{
			while (tam[tam.Length - 1] == ' ')
				tam = tam.Substring(0, (tam.Length - 1) - (0));
			return tam;
		}
		/// <summary> 
		/// 
		/// </summary>
		public virtual void  drawText()
		{
			
			if (!(ShowTitle != null && String.CompareOrdinal(ShowTitle, "HIDE") == 0))
			{
				//Displaying the title of the graph
				if (_graphDataUtils.TextColor == null)
				{
					SupportClass.GraphicsManager.manager.SetColor(BufGra, System.Drawing.Color.FromArgb(250, 60, 250));
				}
				else
				{
					GraphicsColor = _graphDataUtils.TextColor;
				}
				//UPGRADE_TODO: Method 'java.awt.Graphics.drawString' was converted to 'System.Drawing.Graphics.DrawString' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphicsdrawString_javalangString_int_int'"
				BufGra.DrawString(TexteTitre, SupportClass.GraphicsManager.manager.GetFont(BufGra), SupportClass.GraphicsManager.manager.GetBrush(BufGra), location_textetitre, 12 - SupportClass.GraphicsManager.manager.GetFont(BufGra).GetHeight());
			}
			//Drawing the labels of the units on the X and Y axis of the graph
			SupportClass.GraphicsManager.manager.SetColor(BufGra, System.Drawing.Color.FromArgb(174, 0, 226));
			//UPGRADE_TODO: Method 'java.awt.Graphics.drawString' was converted to 'System.Drawing.Graphics.DrawString' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphicsdrawString_javalangString_int_int'"
			BufGra.DrawString(x_units, SupportClass.GraphicsManager.manager.GetFont(BufGra), SupportClass.GraphicsManager.manager.GetBrush(BufGra), largeur - Reduce_String_EndBlanks(x_units).Length * 5 - 250, hauteur - decaly_gfx / 8 - SupportClass.GraphicsManager.manager.GetFont(BufGra).GetHeight());
			//UPGRADE_TODO: Method 'java.awt.Graphics.drawString' was converted to 'System.Drawing.Graphics.DrawString' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphicsdrawString_javalangString_int_int'"
			BufGra.DrawString(y_units, SupportClass.GraphicsManager.manager.GetFont(BufGra), SupportClass.GraphicsManager.manager.GetBrush(BufGra), 5, hauteur - hauteur_gfx - decaly_gfx - (hauteur_bandeau * 11) / 20 - SupportClass.GraphicsManager.manager.GetFont(BufGra).GetHeight());
		}
		/// <summary> </summary>
		/// <param name="tam">
		/// </param>
		/// <returns> String
		/// </returns>
		public virtual System.String Reduce_String_0(System.String tam)
		{
			double sav = System.Double.Parse(tam);
			while (tam.Length > 0 && tam[tam.Length - 1] == '0')
				tam = tam.Substring(0, (tam.Length - 1) - (0));
			if (tam.Length == 0 || String.CompareOrdinal(tam, "-") == 0)
				return "0";
			if (tam[tam.Length - 1] == '.')
				tam = tam.Substring(0, (tam.Length - 1) - (0));
			if (tam.Length == 0)
				return "0";
			if (sav != System.Double.Parse(tam))
				return System.Convert.ToString(sav);
			return tam;
		}
		private void  drawXAxis()
		{
			
			
		}
		
		public virtual void  drawAxis()
		{
			int[] xpoints = new int[20]; // The grid in numbers
			int[] ypoints = new int[20];
			int indicex = 0;
			int indicey = 0;
			if (_graphDataUtils.AxisColor == null)
			{
				SupportClass.GraphicsManager.manager.SetColor(BufGra, System.Drawing.Color.Blue);
			}
			else
			{
				GraphicsColor = _graphDataUtils.AxisColor;
			}
			BufGra.DrawLine(SupportClass.GraphicsManager.manager.GetPen(BufGra), (decalx_gfx * 7) / 8, hauteur - (decaly_gfx * 15) / 16, largeur, hauteur - (decaly_gfx * 15) / 16);
			BufGra.DrawLine(SupportClass.GraphicsManager.manager.GetPen(BufGra), (decalx_gfx * 15) / 16, hauteur - (decaly_gfx * 7) / 8, (decalx_gfx * 15) / 16, hauteur_bandeau / 2);
			if (_graphDataUtils.AxisColor == null)
			{
				System.Drawing.Color temp_Color2;
				temp_Color2 = System.Drawing.Color.Blue;
				System.Drawing.Color temp_Color;
				temp_Color = System.Drawing.Color.FromArgb(System.Convert.ToInt32(temp_Color2.R / 0.7f), System.Convert.ToInt32(temp_Color2.G / 0.7f), System.Convert.ToInt32(temp_Color2.B / 0.7f));
				SupportClass.GraphicsManager.manager.SetColor(BufGra, System.Drawing.Color.FromArgb(System.Convert.ToInt32(temp_Color.R / 0.7f), System.Convert.ToInt32(temp_Color.G / 0.7f), System.Convert.ToInt32(temp_Color.B / 0.7f)));
			}
			else
			{
				GraphicsColor = _graphDataUtils.AxisTextColor;
			}
			// X Axis
			double dixiemegap = System.Math.Abs((RealLastx - RealFirstx) / 15);
			double lmytest = 5e20;
			double mytest = 1e20;
			while (!(lmytest > dixiemegap && mytest <= dixiemegap) && lmytest > 1e-15)
			{
				lmytest /= 5;
				mytest /= 2;
				if (!(lmytest > dixiemegap && mytest <= dixiemegap))
				{
					lmytest /= 2;
					mytest /= 5;
				}
			}
			double Start_With;
			double Multiply_Factor_CommunicatorPC;
			if (Lastx > Firstx)
			{
				mytest = lmytest;
				Multiply_Factor_CommunicatorPC = 1.0000000001;
			}
			else
			{
				mytest = - lmytest;
				Multiply_Factor_CommunicatorPC = 0.9999999999;
			}
			Start_With = System.Math.Ceiling((RealFirstx + ((RealFirstx - RealLastx) * decalx_gfx) / (16 * largeur - decalx_gfx)) / mytest) * mytest;
			// draw the small graduations
			for (double i = Start_With - mytest; ((i - RealFirstx) * largeur_gfx) / (RealLastx - RealFirstx) < largeur - decalx_gfx; i += mytest / 5)
			{
				if (((i - RealFirstx) * largeur_gfx) / (RealLastx - RealFirstx) > 0)
				{
					//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangMathround_double'"
					BufGra.DrawLine(SupportClass.GraphicsManager.manager.GetPen(BufGra), (int) System.Math.Round(decalx_gfx + ((i - RealFirstx) * largeur_gfx) / (RealLastx - RealFirstx)), hauteur - (decaly_gfx * 11) / 12, (int) System.Math.Round(decalx_gfx + ((i - RealFirstx) * largeur_gfx) / (RealLastx - RealFirstx)), hauteur - (decaly_gfx * 15) / 16);
				}
			}
			// draw the large graduations and values
			for (double i = Start_With; ((i - RealFirstx) * largeur_gfx) / (RealLastx - RealFirstx) < largeur - decalx_gfx; i += mytest * Multiply_Factor_CommunicatorPC)
			{
				//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangMathround_double'"
				xpoints[indicex] = (int) System.Math.Round(decalx_gfx + ((i - RealFirstx) * largeur_gfx) / (RealLastx - RealFirstx));
				BufGra.DrawLine(SupportClass.GraphicsManager.manager.GetPen(BufGra), xpoints[indicex], hauteur - (decaly_gfx * 7) / 8, xpoints[indicex], hauteur - (decaly_gfx * 15) / 16);
				indicex++;
				// trop de precision sur Communicator... plein de 0 et des merdes...
				System.String Une_Solution = System.Convert.ToString(i);
				if (Une_Solution.Length > 10)
				{
					if (Une_Solution.IndexOf('E') != - 1)
						Une_Solution = Une_Solution.Substring(0, (Une_Solution.IndexOf('E') - 1) - (0)) + "e" + Une_Solution.Substring(Une_Solution.IndexOf('E') + 1);
					if (Une_Solution.IndexOf('e') == - 1)
						Une_Solution = Une_Solution.Substring(0, (9) - (0));
					else
						Une_Solution = System.Convert.ToString(System.Math.Pow(10, System.Double.Parse(Une_Solution.Substring(Une_Solution.IndexOf('e') + 1))) * System.Double.Parse(Une_Solution.Substring(0, (System.Math.Min(9, Une_Solution.IndexOf('e') - 1)) - (0))));
				}
				Une_Solution = Reduce_String_0(Une_Solution);
				if (System.Math.Abs(System.Double.Parse(Une_Solution)) < 1e-4 && System.Math.Abs(mytest) > 1e-3)
					Une_Solution = "0";
				//UPGRADE_TODO: Method 'java.awt.Graphics.drawString' was converted to 'System.Drawing.Graphics.DrawString' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphicsdrawString_javalangString_int_int'"
				//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangMathround_double'"
				BufGra.DrawString(Une_Solution, SupportClass.GraphicsManager.manager.GetFont(BufGra), SupportClass.GraphicsManager.manager.GetBrush(BufGra), (int) System.Math.Round(decalx_gfx + ((i - RealFirstx) * largeur_gfx) / (RealLastx - RealFirstx)) - Une_Solution.Length * 2, hauteur - decaly_gfx / 2 - SupportClass.GraphicsManager.manager.GetFont(BufGra).GetHeight());
			}
			// Y Axis
			dixiemegap = (Maxy - Miny) / 15;
			lmytest = 5e20;
			mytest = 1e20;
			while (!(lmytest > dixiemegap && mytest <= dixiemegap) && lmytest > 1e-20)
			{
				lmytest /= 5;
				mytest /= 2;
				if (!(lmytest > dixiemegap && mytest <= dixiemegap))
				{
					lmytest /= 2;
					mytest /= 5;
				}
			}
			mytest = lmytest;
			// draw the small graduations
			for (double i = (System.Math.Ceiling(Miny / mytest)) * mytest - mytest; ((i - Miny) * hauteur_gfx) / (Maxy - Miny) < hauteur - decaly_gfx - hauteur_bandeau / 2; i += mytest / 5)
			{
				if (((i - Miny) * hauteur_gfx) / (Maxy - Miny) > 0)
				{
					//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangMathround_double'"
					BufGra.DrawLine(SupportClass.GraphicsManager.manager.GetPen(BufGra), (decalx_gfx * 11) / 12, hauteur - decaly_gfx - (int) System.Math.Round(((i - Miny) * hauteur_gfx) / (Maxy - Miny)), (decalx_gfx * 15) / 16, hauteur - decaly_gfx - (int) System.Math.Round(((i - Miny) * hauteur_gfx) / (Maxy - Miny)));
				}
			}
			// draw the large graduations and values
			for (double i = (System.Math.Ceiling(Miny / mytest)) * mytest; ((i - Miny) * hauteur_gfx) / (Maxy - Miny) < hauteur - decaly_gfx - hauteur_bandeau / 2; i += mytest * 1.0000000001)
			{
				//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangMathround_double'"
				ypoints[indicey] = hauteur - decaly_gfx - (int) System.Math.Round(((i - Miny) * hauteur_gfx) / (Maxy - Miny));
				BufGra.DrawLine(SupportClass.GraphicsManager.manager.GetPen(BufGra), (decalx_gfx * 7) / 8, ypoints[indicey], (decalx_gfx * 15) / 16, ypoints[indicey]);
				indicey++;
				// trop de precision sur Communicator... plein de 0 et des merdes...
				System.String Une_Solution = System.Convert.ToString(i);
				if (Une_Solution.Length > 10)
				{
					if (Une_Solution.IndexOf('E') != - 1)
						Une_Solution = Une_Solution.Substring(0, (Une_Solution.IndexOf('E') - 1) - (0)) + "e" + Une_Solution.Substring(Une_Solution.IndexOf('E') + 1);
					if (Une_Solution.IndexOf('e') == - 1)
						Une_Solution = Une_Solution.Substring(0, (9) - (0));
					else
						Une_Solution = System.Convert.ToString(System.Math.Pow(10, System.Double.Parse(Une_Solution.Substring(Une_Solution.IndexOf('e') + 1))) * System.Double.Parse(Une_Solution.Substring(0, (System.Math.Min(9, Une_Solution.IndexOf('e') - 1)) - (0))));
				}
				Une_Solution = Reduce_String_0(Une_Solution);
				if (System.Math.Abs(System.Double.Parse(Une_Solution)) < 1e-4 && System.Math.Abs(mytest) > 1e-3)
					Une_Solution = "0";
				/*
				Shravan Sadasivan - Changes to be made to this section in order to rotate the Y Axis labels by 90 Degrees
				Changes to be made to this section for optional display of Y Axis labels based on the
				type of spectral data being used.
				*/
				if (!(Y_Values != null && String.CompareOrdinal(Y_Values, "HIDE") == 0))
					if (y_units.ToUpper().Equals("ABSORBANCE".ToUpper()) || y_units.ToUpper().Equals("%T".ToUpper()) || y_units.ToUpper().Equals("TRANSMITTANCE".ToUpper()) || y_units.ToUpper().Equals("RELATIVE ABUNDANCE".ToUpper()))
					{
						//UPGRADE_TODO: Method 'java.awt.Graphics.drawString' was converted to 'System.Drawing.Graphics.DrawString' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphicsdrawString_javalangString_int_int'"
						//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangMathround_double'"
						BufGra.DrawString(Une_Solution, SupportClass.GraphicsManager.manager.GetFont(BufGra), SupportClass.GraphicsManager.manager.GetBrush(BufGra), (decalx_gfx * 3) / 4 - Une_Solution.Length * 5, hauteur - decaly_gfx + 4 - (int) System.Math.Round(((i - Miny) * hauteur_gfx) / (Maxy - Miny)) - SupportClass.GraphicsManager.manager.GetFont(BufGra).GetHeight());
					}
			}
			if (Flag_Grid)
			{
				SupportClass.GraphicsManager.manager.SetColor(BufGra, System.Drawing.Color.FromArgb(150, 100, 255));
				for (int i = 0; i < indicex; i++)
					for (int j = hauteur - (decaly_gfx * 15) / 16; j > hauteur_bandeau / 2; j -= (hauteur - (decaly_gfx * 15) / 16 - hauteur_bandeau / 2) / 32)
						BufGra.DrawLine(SupportClass.GraphicsManager.manager.GetPen(BufGra), xpoints[i], j, xpoints[i], j - (hauteur - (decaly_gfx * 15) / 16 - hauteur_bandeau / 2) / 64);
				for (int i = 0; i < indicey; i++)
					for (int j = (decalx_gfx * 15) / 16; j < largeur; j += (largeur - (decalx_gfx * 15) / 16) / 32)
						BufGra.DrawLine(SupportClass.GraphicsManager.manager.GetPen(BufGra), j, ypoints[i], j + (largeur - (decalx_gfx * 15) / 16) / 64, ypoints[i]);
			}
		}
		
		public virtual void  Trace_PEAK_TABLE()
		{
			SupportClass.GraphicsManager.manager.SetColor(BufGra, System.Drawing.Color.Black);
			double rxd;
			double rxf;
			if (Multx > 0)
			{
				rxd = xd; rxf = xf;
			}
			else
			{
				rxd = xf; rxf = xd;
			}
			for (int i = 0; i < Nbpoints; i++)
				if (tableau_points[i * 2] >= rxd && tableau_points[i * 2] <= rxf)
				{
					//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangMathround_double'"
					BufGra.DrawLine(SupportClass.GraphicsManager.manager.GetPen(BufGra), decalx_gfx + (int) System.Math.Round((tableau_points[i * 2] - RealFirstx) * Multx), (int) System.Math.Round(hauteur - tableau_points[i * 2 + 1] * YFactor * Multy) - decaly_gfx, decalx_gfx + (int) System.Math.Round((tableau_points[i * 2] - RealFirstx) * Multx), hauteur - decaly_gfx);
				}
		}
		/*
		Shravan Sadasivan - Changes should be made to this section in order to print the data provided for the peaks,
		on top of the peaks. This is the method that traces the yellow line on top of the Integrated graph
		*/
		public virtual void  Trace_Integrate()
		{
			System.String point = null;
			System.String integrateValue = null;
			System.String trueXValue = null;
			int xIntegrateValue = 0;
			int yIntegrateValue = 120;
			System.String lastPoint = null;
			// NumberFormat form = NumberFormat.getInstance();
			//UPGRADE_ISSUE: Class 'java.text.DecimalFormat' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javatextDecimalFormat'"
			//UPGRADE_ISSUE: Constructor 'java.text.DecimalFormat.DecimalFormat' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javatextDecimalFormat'"
			DecimalFormat decForm = new DecimalFormat("#####");
			if (Incrx > 0)
				x_Renamed_Field = 0;
			else
				x_Renamed_Field = largeur_gfx;
			ax = x_Renamed_Field;
			if (Incrx > 0)
			{
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				prempoint = (int) System.Math.Ceiling(((xd - Firstx) * Nbpoints) / (Lastx - Firstx));
			}
			else
			{
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				prempoint = (int) System.Math.Ceiling(((Lastx - xf) * Nbpoints) / (Lastx - Firstx));
			}
			double Multintegrate = hauteur_gfx / Maxintegrate;
			Nbpoints_a_tracer = Sav_Nbpoints_a_tracer;
			int indicetableau = 0;
			if (!Flag_Reverse)
				ay = tableau_integrate[indicetableau] * Multintegrate;
			else
				ay = (Maxintegrate - tableau_integrate[indicetableau]) * Multintegrate;
			while (Nbpoints_a_tracer > 0 && indicetableau < Nbpoints)
			{
				/* Sets the color of the intergration curve */
				if (_graphDataUtils.IntegrateCurveColor == null)
				{
					SupportClass.GraphicsManager.manager.SetColor(BufGra, System.Drawing.Color.Orange);
				}
				else
				{
					GraphicsColor = _graphDataUtils.IntegrateCurveColor;
				}
				if (prempoint > 1)
				{
					prempoint--;
				}
				else if (prempoint == 1)
				{
					if (!Flag_Reverse)
						ay = tableau_integrate[indicetableau] * Multintegrate;
					else
						ay = (Maxintegrate - tableau_integrate[indicetableau]) * Multintegrate;
					ax = x_Renamed_Field;
					x_Renamed_Field += Incrx;
					prempoint--;
				}
				else
				{
					//	Control enters this loop when prempoint is 0
					if (!Flag_Reverse)
						y = tableau_integrate[indicetableau] * Multintegrate;
					else
						y = (Maxintegrate - tableau_integrate[indicetableau]) * Multintegrate;
					if (y != 0 && ay != 0)
					{
						//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangMathround_double'"
						BufGra.DrawLine(SupportClass.GraphicsManager.manager.GetPen(BufGra), (int) System.Math.Round(ax) + decalx_gfx, (int) System.Math.Round(hauteur - ay) - decaly_gfx, (int) System.Math.Round(x_Renamed_Field) + decalx_gfx, (int) System.Math.Round(hauteur - y) - decaly_gfx);
						trueXValue = StringDataUtils.reduceDataPrecision(System.Convert.ToString(x(RealFirstx + ((x_Renamed_Field - decalx_gfx) * (RealLastx - RealFirstx)) / largeur_gfx)));
						System.Double tempAux = System.Double.Parse(trueXValue);
						//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
						point = _graphDataUtils.getIntegrationCurveAreaValue(ref tempAux);
						if (point != null)
						{
							if (_graphDataUtils.IntegrateTextColor == null)
							{
								SupportClass.GraphicsManager.manager.SetColor(BufGra, System.Drawing.Color.Black);
							}
							else
							{
								GraphicsColor = _graphDataUtils.IntegrateTextColor;
							}
							System.Console.Out.WriteLine(point);
							/* xIntegrateValue = (int)Math.round(Double.valueOf(decForm.format(x)));
							yIntegrateValue = (int)Math.round(hauteur-y)-decaly_gfx-10; */
							//UPGRADE_TODO: Method 'java.awt.Graphics.drawString' was converted to 'System.Drawing.Graphics.DrawString' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphicsdrawString_javalangString_int_int'"
							BufGra.DrawString(point, SupportClass.GraphicsManager.manager.GetFont(BufGra), SupportClass.GraphicsManager.manager.GetBrush(BufGra), xIntegrateValue, yIntegrateValue - SupportClass.GraphicsManager.manager.GetFont(BufGra).GetHeight());
							point = null;
						}
					}
					ax = x_Renamed_Field; ay = y;
					x_Renamed_Field += Incrx;
					Nbpoints_a_tracer--;
				}
				indicetableau++;
			}
		}
		public virtual void  Trace_XYDATA()
		{
			if (Flag_Integrate)
				Trace_Integrate();
			if (_graphDataUtils.GraphCurveColor == null)
			{
				SupportClass.GraphicsManager.manager.SetColor(BufGra, System.Drawing.Color.Black);
			}
			else
			{
				GraphicsColor = _graphDataUtils.GraphCurveColor;
			}
			if (Incrx > 0)
				x_Renamed_Field = 0;
			else
				x_Renamed_Field = largeur_gfx;
			ax = x_Renamed_Field;
			if (Incrx > 0)
			{
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				prempoint = (int) System.Math.Ceiling(((xd - Firstx) * Nbpoints) / (Lastx - Firstx));
			}
			else
			{
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				prempoint = (int) System.Math.Ceiling(((Lastx - xf) * Nbpoints) / (Lastx - Firstx));
			}
			Nbpoints_a_tracer = Sav_Nbpoints_a_tracer;
			int indicetableau = 0;
			ay = (tableau_points[0] * YFactor - Miny) * Multy;
			while (Nbpoints_a_tracer >= 0 && indicetableau < Nbpoints)
			{
				if (prempoint > 1)
				{
					prempoint--;
				}
				else if (prempoint == 1)
				{
					ay = (tableau_points[indicetableau] * YFactor - Miny) * Multy;
					ax = x_Renamed_Field;
					x_Renamed_Field += Incrx;
					prempoint--;
				}
				else
				{
					y = (tableau_points[indicetableau] * YFactor - Miny) * Multy;
					//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangMathround_double'"
					BufGra.DrawLine(SupportClass.GraphicsManager.manager.GetPen(BufGra), (int) System.Math.Round(ax) + decalx_gfx, (int) System.Math.Round(hauteur - ay) - decaly_gfx, (int) System.Math.Round(x_Renamed_Field) + decalx_gfx, (int) System.Math.Round(hauteur - y) - decaly_gfx);
					ax = x_Renamed_Field; ay = y;
					x_Renamed_Field += Incrx;
					Nbpoints_a_tracer--;
				}
				indicetableau++;
			}
		}
		public virtual bool drawSpectra()
		{
			if (Firstx == shitty_starting_constant || Lastx == shitty_starting_constant || Miny == shitty_starting_constant || Maxy == shitty_starting_constant || YFactor == shitty_starting_constant || Nbpoints == shitty_starting_constant || Firstx == Lastx || Miny == Maxy)
				return false;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			Sav_Nbpoints_a_tracer = (int) System.Math.Floor((Nbpoints * (xf - xd)) / (Lastx - Firstx)) + 1;
			Incrx = largeur_gfx / (double) Sav_Nbpoints_a_tracer;
			if (Firstx > Lastx)
				Incrx = - Incrx;
			Multx = largeur_gfx / (RealLastx - RealFirstx);
			Multy = hauteur_gfx / (Maxy - Miny);
			if (typedata == 0)
				Trace_XYDATA();
			if (typedata == 1)
				Trace_PEAK_TABLE();
			return true;
		}
		public virtual void  Draw_Graphics(double xdeb, double xfin)
		{
			xd = xdeb;
			xf = xfin;
			SupportClass.GraphicsManager.manager.SetColor(BufGra, System.Drawing.Color.White);
			BufGra.FillRectangle(SupportClass.GraphicsManager.manager.GetPaint(BufGra), 1, 1, largeur - 2, hauteur - 2);
			SupportClass.GraphicsManager.manager.SetColor(BufGra, System.Drawing.Color.Black);
			drawAxis();
			if (!drawSpectra())
				Draw_Texte("File corrupted/unregnognized file format/datas");
			drawText();
			//UPGRADE_WARNING: Method 'java.awt.Graphics.drawImage' was converted to 'System.Drawing.Graphics.drawImage' which may throw an exception. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1101'"
			SavBufGra.DrawImage(BufImg, 0, 0);
			//UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
			Refresh();
		}
		public virtual double antecedent(double x)
		{
			return RealFirstx + (x * (RealLastx - RealFirstx)) / largeur_gfx;
		}
		public virtual void  Find_Peak()
		{
			if (typedata == 1)
				Draw_Texte("Unavailable with Peak Data files");
			if (typedata == 0)
			{
				if (x2_zoom < x1_zoom)
				{
					int tmp = x1_zoom; x1_zoom = x2_zoom; x2_zoom = tmp;
				}
				double Seuil1;
				double Seuil2;
				if (Incrx > 0)
				{
					Seuil1 = RealFirstx + ((x1_zoom - decalx_gfx) * (RealLastx - RealFirstx)) / largeur_gfx;
					Seuil2 = RealFirstx + ((x2_zoom - decalx_gfx) * (RealLastx - RealFirstx)) / largeur_gfx;
				}
				else
				{
					Seuil2 = RealFirstx + ((x1_zoom - decalx_gfx) * (RealLastx - RealFirstx)) / largeur_gfx;
					Seuil1 = RealFirstx + ((x2_zoom - decalx_gfx) * (RealLastx - RealFirstx)) / largeur_gfx;
				}
				// -Shravan Sadasivan
				// -Hard coded values?
				double xpeakmax = 666667;
				double ypeakmax = 666667;
				double xpeakmin = 666667;
				double ypeakmin = 666667;
				double currmax = - 666667e66;
				double currmin = 666667e66;
				// -
				if (Incrx > 0)
					x_Renamed_Field = 0;
				else
					x_Renamed_Field = largeur_gfx;
				if (Incrx > 0)
				{
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					prempoint = (int) System.Math.Floor(((xd - Firstx) * Nbpoints) / (Lastx - Firstx));
				}
				else
				{
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					prempoint = (int) System.Math.Floor(((Lastx - xf) * Nbpoints) / (Lastx - Firstx));
				}
				int indicetableau = 0;
				Nbpoints_a_tracer = Sav_Nbpoints_a_tracer - 1;
				while (Nbpoints_a_tracer > 0 && indicetableau < Nbpoints)
				{
					if (prempoint > 0)
					{
						prempoint--;
					}
					else
					{
						if (antecedent(x_Renamed_Field) >= Seuil1 && antecedent(x_Renamed_Field) <= Seuil2)
						{
							y = tableau_points[indicetableau] * YFactor;
							if (y > currmax)
							{
								xpeakmax = x_Renamed_Field; ypeakmax = y; currmax = y;
							}
							if (y < currmin)
							{
								xpeakmin = x_Renamed_Field; ypeakmin = y; currmin = y;
							}
						}
						x_Renamed_Field += Incrx;
						Nbpoints_a_tracer--;
					}
					indicetableau++;
				}
				double valeur_teste = (f(Seuil1) + f(Seuil2)) / 2;
				double xpeak = 666667;
				double ypeak = 666667;
				if (System.Math.Abs(ypeakmax - valeur_teste) > System.Math.Abs(ypeakmin - valeur_teste))
				{
					xpeak = xpeakmax; ypeak = ypeakmax;
				}
				else
				{
					xpeak = xpeakmin; ypeak = ypeakmin;
				}
				Draw_Texte("Peak found at X=" + StringDataUtils.reduceDataPrecision(System.Convert.ToString(antecedent(xpeak))) + " " + Reduce_String_EndBlanks(x_units) + " (Y=" + StringDataUtils.reduceDataPrecision(System.Convert.ToString(ypeak)) + ")");
				SupportClass.GraphicsManager.manager.SetColor(BufGra, System.Drawing.Color.Green);
				//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangMathround_double'"
				BufGra.DrawLine(SupportClass.GraphicsManager.manager.GetPen(BufGra), decalx_gfx + (int) System.Math.Round(xpeak), (int) System.Math.Round(hauteur - ypeak * Multy) - decaly_gfx, decalx_gfx + (int) System.Math.Round(xpeak), hauteur - decaly_gfx);
				//UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
				Refresh();
			}
		}
		public virtual void  Integrate()
		{
			tableau_integrate = new double[Nbpoints];
			double current_value = 0;
			double seuil = Maxy / YFactor / 1000;
			for (int i = 0; i < Nbpoints; i++)
			{
				if (tableau_points[i] > seuil)
					current_value += tableau_points[i];
				tableau_integrate[i] = current_value;
			}
			Maxintegrate = 0;
			for (int i = 0; i < Nbpoints; i++)
				if (tableau_integrate[i] > Maxintegrate)
					Maxintegrate = tableau_integrate[i];
		}
		/*
		Shravan Sadasivan - This code can be removed since this functionality is being phased out for V1.0
		*/
		/* BEGIN - REMOVE */
		public virtual void  Zoomin()
		{
			Last_RealFirstx = RealFirstx; // pour Zoomback
			Last_RealLastx = RealLastx;
			Last_Firstx = Firstx;
			Last_Lastx = Lastx;
			if (x2_zoom < x1_zoom)
			{
				int tmp = x1_zoom; x1_zoom = x2_zoom; x2_zoom = tmp;
			}
			double tmp2 = RealFirstx + ((x1_zoom - decalx_gfx) * (RealLastx - RealFirstx)) / largeur_gfx;
			RealLastx = RealFirstx + ((x2_zoom - decalx_gfx) * (RealLastx - RealFirstx)) / largeur_gfx;
			//RealFirstx = x1_zoom;
			//RealLastx = x2_zoom;
			RealFirstx = tmp2;
			
			Draw_Graphics(RealFirstx, RealLastx);
		}
		/// <summary> 
		/// 
		/// </summary>
		public virtual void  Zoomback()
		{
			double tmp1 = RealFirstx;
			double tmp2 = RealLastx;
			double tmp3 = Firstx;
			double tmp4 = Lastx;
			RealFirstx = Last_RealFirstx;
			RealLastx = Last_RealLastx;
			Firstx = Last_Firstx;
			Lastx = Last_Lastx;
			Last_RealFirstx = tmp1;
			Last_RealLastx = tmp2;
			Last_Firstx = tmp3;
			Last_Lastx = tmp4;
			Draw_Graphics(RealFirstx, RealLastx);
		}
		public virtual void  Zoomout()
		{
			RealFirstx = Firstx;
			RealLastx = Lastx;
			Draw_Graphics(RealFirstx, RealLastx);
		}
		/* END - REMOVE */
		public virtual void  Redraw()
		{
			Draw_Graphics(RealFirstx, RealLastx);
		}
		public virtual void  Reverse()
		{
			Last_RealFirstx = RealFirstx; // pour Zoomback
			Last_RealLastx = RealLastx;
			Last_Firstx = Firstx;
			Last_Lastx = Lastx;
			double tmp1 = RealFirstx;
			double tmp2 = Firstx;
			RealFirstx = RealLastx;
			Firstx = Lastx;
			RealLastx = tmp1;
			Lastx = tmp2;
			Draw_Graphics(RealFirstx, RealLastx);
		}
		public virtual double f(double tam)
		{
			if (tam < Sav_Firstx || tam > Sav_Lastx)
				return 666;
			if (typedata == 0)
			// XYDATA
			{
				//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangMathround_double'"
				return tableau_points[(int) System.Math.Round(((tam - Sav_Firstx) * Nbpoints) / (Sav_Lastx - Sav_Firstx))] * YFactor;
			}
			if (typedata == 1)
			{
				// PEAK TABLE
				double distance = 666667e66;
				double meilleure_approx = 666667;
				for (int i = 0; i < Nbpoints; i++)
					if (System.Math.Abs(tableau_points[i * 2] - tam) < distance)
					{
						meilleure_approx = tableau_points[i * 2 + 1]; distance = System.Math.Abs(tableau_points[i * 2] - tam);
					}
				return meilleure_approx * YFactor;
			}
			return 666667;
		}
		public virtual double x(double tam)
		{
			if (typedata == 1)
			{
				//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangMathround_double'"
				return (long) System.Math.Round(tam);
			}
			else
				return tam; // peak table, valeurs entieres seulement
		}
		/// <summary> </summary>
		/// <param name="tam">
		/// </param>
		/// <returns>
		/// </returns>
		public virtual System.String trouve_f(double tam)
		{
			if (tam < Sav_Firstx || tam > Sav_Lastx)
				return "(outside spectra)";
			if (typedata == 0)
			// XYDATA
			{
				//UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangMathround_double'"
				return "Y=" + StringDataUtils.reduceDataPrecision(System.Convert.ToString(tableau_points[(int) System.Math.Round(((tam - Sav_Firstx) * Nbpoints) / (Sav_Lastx - Sav_Firstx))] * YFactor));
			}
			if (typedata == 1)
			{
				// PEAK TABLE
				double distance = 666667e66;
				double meilleure_approx_x = 666667;
				double meilleure_approx_y = 666667;
				for (int i = 0; i < Nbpoints; i++)
					if (System.Math.Abs(tableau_points[i * 2] - tam) < distance)
					{
						meilleure_approx_x = tableau_points[i * 2]; meilleure_approx_y = tableau_points[i * 2 + 1]; distance = System.Math.Abs(tableau_points[i * 2] - tam);
					}
				return "Y(" + StringDataUtils.reduceDataPrecision(System.Convert.ToString(meilleure_approx_x)) + ")=" + StringDataUtils.reduceDataPrecision(System.Convert.ToString(meilleure_approx_y * YFactor));
			}
			return "666667";
		}
		public virtual void  Do_Clickable_Peaks(double xvalue)
		{
			for (int i = 0; i < Nb_Clickable_Peaks; i++)
				if (xvalue > Peak_Start[i] && xvalue < Peak_Stop[i])
				{
					Flag_Load_Now_Html = true;
					Name_Load_Now_Html = Peak_Html[i];
				}
		}
		//UPGRADE_NOTE: The equivalent of method 'java.awt.Component.mouseDown' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		//UPGRADE_ISSUE: Class 'java.awt.Event' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
		public bool mouseDown(Event evt, int x, int y)
		{
			x1_zoom = x;
			//UPGRADE_WARNING: Method 'java.awt.Graphics.drawImage' was converted to 'System.Drawing.Graphics.drawImage' which may throw an exception. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1101'"
			BufGra.DrawImage(SavBufImg, 0, 0);
			SupportClass.GraphicsManager.manager.SetColor(BufGra, System.Drawing.Color.Red);
			BufGra.DrawLine(SupportClass.GraphicsManager.manager.GetPen(BufGra), x, hauteur_bandeau, x, hauteur - (decaly_gfx * 15) / 16 - 1);
			
			//Centralize this set of formulae - Shravan
			Draw_Texte("X=" + StringDataUtils.reduceDataPrecision(System.Convert.ToString(x(RealFirstx + ((x - decalx_gfx) * (RealLastx - RealFirstx)) / largeur_gfx))) + " ; " + trouve_f(RealFirstx + ((x - decalx_gfx) * (RealLastx - RealFirstx)) / largeur_gfx));
			if (Flag_Clickable_Peaks)
				Do_Clickable_Peaks(RealFirstx + ((x - decalx_gfx) * (RealLastx - RealFirstx)) / largeur_gfx);
			//UPGRADE_WARNING: Method 'java.awt.Graphics.drawImage' was converted to 'System.Drawing.Graphics.drawImage' which may throw an exception. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1101'"
			ZoomBufGra.DrawImage(BufImg, 0, 0);
			return true;
		}
		//UPGRADE_NOTE: The equivalent of method 'java.awt.Component.mouseDrag' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		//UPGRADE_ISSUE: Class 'java.awt.Event' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
		public bool mouseDrag(Event evt, int x, int y)
		{
			x2_zoom = x;
			//UPGRADE_WARNING: Method 'java.awt.Graphics.drawImage' was converted to 'System.Drawing.Graphics.drawImage' which may throw an exception. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1101'"
			BufGra.DrawImage(ZoomBufImg, 0, 0);
			SupportClass.GraphicsManager.manager.SetColor(BufGra, System.Drawing.Color.Red);
			BufGra.DrawLine(SupportClass.GraphicsManager.manager.GetPen(BufGra), x, hauteur_bandeau, x, hauteur - (decaly_gfx * 15) / 16 - 1);
			
			Draw_Texte("X=" + StringDataUtils.reduceDataPrecision(System.Convert.ToString(x(RealFirstx + ((x1_zoom - decalx_gfx) * (RealLastx - RealFirstx)) / largeur_gfx))) + " ; " + trouve_f(RealFirstx + ((x1_zoom - decalx_gfx) * (RealLastx - RealFirstx)) / largeur_gfx) + "  /  X=" + StringDataUtils.reduceDataPrecision(System.Convert.ToString(x(RealFirstx + ((x - decalx_gfx) * (RealLastx - RealFirstx)) / largeur_gfx))) + " ; " + trouve_f(RealFirstx + ((x - decalx_gfx) * (RealLastx - RealFirstx)) / largeur_gfx));
			return true;
		}
		protected override void  OnPaint(System.Windows.Forms.PaintEventArgs g_EventArg)
		{
			System.Drawing.Graphics g = null;
			if (g_EventArg != null)
				g = g_EventArg.Graphics;
			//UPGRADE_WARNING: Method 'java.awt.Graphics.drawImage' was converted to 'System.Drawing.Graphics.drawImage' which may throw an exception. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1101'"
			g.DrawImage(BufImg, 0, 0);
		}
		//UPGRADE_NOTE: The equivalent of method 'java.awt.Component.update' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		public void  update(System.Drawing.Graphics g)
		{
			OnPaint(new System.Windows.Forms.PaintEventArgs(g, Bounds));
		}
	}
}