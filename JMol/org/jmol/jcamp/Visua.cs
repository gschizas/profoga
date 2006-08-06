/*
* ##############     class Visua v2.0     ################
*
* Copyright (c) Guillaume Cottenceau, 1998
*
* JCAMP and SPC viewer for HTML page
*
* Last modification : 24.4.98
*
* Modified to write JCAMP files on client (APT : 05-01-99)
*
*/
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using org.jmol.jcamp.utils;
namespace org.jmol.jcamp
{
	//UPGRADE_TODO: Class 'java.applet.Applet' was converted to 'System.Windows.Forms.UserControl' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaappletApplet'"
	[Serializable]
	public class Visua:System.Windows.Forms.UserControl, IThreadRunnable
	{
		public Visua()
		{
			InitBlock();
		}
		private void  InitBlock()
		{
			this.Load += new System.EventHandler(this.Visua_StartEventHandler);
			this.Disposed += new System.EventHandler(this.Visua_StopEventHandler);
		}
		public bool isActiveVar = true;
		internal ZoneVisu My_ZoneVisu;
		internal SupportClass.ThreadClass My_Thread;
		private GraphCharacteristics _graphDataUtils; // All data pertaining to the graphs' features
		public System.Collections.ArrayList texte; // variable qui sont utilisees par des sous classes
		public int nbLignes;
		internal int shitty_starting_constant = 66666; //what is this? figure it out
		public double Firstx;
		public double Lastx;
		public double YFactor;
		public int Nbpoints;
		public double nmr_observe_frequency;
		public System.String TexteTitre;
		public System.String x_units;
		public System.String y_units;
		public System.String Datatype;
		// TODO - Shravan Change 2
		internal System.Windows.Forms.Button Load_File;
		internal System.Windows.Forms.Button Zoom_In;
		internal System.Windows.Forms.Button Zoom_Back;
		internal System.Windows.Forms.Button Zoom_Out;
		internal System.Windows.Forms.CheckBox Reverse;
		internal System.Windows.Forms.CheckBox Grid;
		internal System.Windows.Forms.CheckBox Integrate;
		// END - Shravan Change 2
		internal System.Windows.Forms.Button Find_Peak;
		internal System.Windows.Forms.Button JCamp;
		//UPGRADE_TODO: Class 'java.awt.Frame' was converted to 'System.Windows.Forms.Form' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtFrame'"
		internal System.Windows.Forms.Form Fenetre_Load_File;
		internal SaisieDlg Dialogue_Load_File;
		//UPGRADE_ISSUE: Interface 'java.applet.AppletContext' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaappletAppletContext'"
		internal AppletContext contexte;
		internal System.String clickable_peaks_frame_name;
		internal bool Flag_Zoomin = false;
		internal bool Flag_Zoomback = false;
		internal bool Flag_Zoomout = false;
		internal bool Flag_Reverse = false;
		internal bool Flag_Integrate = false;
		internal bool Flag_Dialogue_File_Enabled = false;
		internal bool Flag_Redraw = false;
		internal bool Flag_Find_Peak = false;
		internal bool Flag_Write_JCamp = false;
		internal bool isActive = false, hasPrivilege = false;
		internal bool inNavigator = false, inExplorer = false;
		internal System.Windows.Forms.FileDialog openDialog;
		internal System.Windows.Forms.FileDialog saveDialog;
		internal System.String Current_Error = "";
		/* Applet initialization */
		//UPGRADE_TODO: Commented code was moved to the 'InitializeComponent' method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1228'"
		public void  init()
		{
			InitializeComponent();
			//UPGRADE_ISSUE: Method 'java.applet.Applet.showStatus' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaappletAppletshowStatus_javalangString'"
			showStatus("Initializing jcamp/spc visualizer, please wait...");
			System.Console.Out.WriteLine("jcamp/spc visualizer v2.0.0 (c) G. Cottenceau 1998");
			//
			//		Netscape security for the applet RMI network comms
			//		    This will launch a Java Security window
			//
			/* Shravan Sadasivan - Commented during developement */
			// TODO - Uncomment during implementation
			/*	System.out.println(System.getProperty("java.vendor"));
			System.out.println(System.getProperty("browser") + " v" +
			System.getProperty("browser.version"));
			if ((System.getProperty("java.vendor").toLowerCase().indexOf("netscape")) != -1) {
			inNavigator = true;
			} else if ((System.getProperty("java.vendor").toLowerCase().indexOf("microsoft")) != -1) {
			inExplorer = true;
			} else {
			System.out.println("\nUnknown browser vendor");
			} */
			//		MSIE4.0 will give a security exception :
			//		com.ms.security.SecurityExceptionEx : FileDialog creation denied
			if (inNavigator)
			{
				//		FileDialog needs a parent Frame
				
				System.Windows.Forms.Control c = this.Parent;
				//UPGRADE_TODO: Class 'java.awt.Frame' was converted to 'System.Windows.Forms.Form' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtFrame'"
				while (c != null && !(c is System.Windows.Forms.Form))
					c = c.Parent;
				//	     openDialog = new FileDialog( (Frame)c, "Open PDB", FileDialog.LOAD);
				saveDialog = new System.Windows.Forms.SaveFileDialog();
				saveDialog.Title = "Save JCAMP";
			}
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtBorderLayout'"
			/*
			setLayout(new BorderLayout());*/
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.awt.Panel' and 'System.Windows.Forms.Panel' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			System.Windows.Forms.Panel Mes_Boutons = new System.Windows.Forms.Panel();
			//UPGRADE_TODO: Constructor 'java.awt.GridLayout.GridLayout' was converted to 'System.Drawing.Rectangle.Rectangle' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGridLayoutGridLayout_int_int'"
			//UPGRADE_TODO: Class 'java.awt.GridLayout' was converted to 'System.Drawing.Rectangle' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGridLayout'"
			Mes_Boutons.Tag = new System.Drawing.Rectangle(1, 1, 0, 0);
			Mes_Boutons.Layout += new System.Windows.Forms.LayoutEventHandler(this.Mes_Boutons_setLayout);
			if (getParameter("INTEGRATE") != null && String.CompareOrdinal(getParameter("INTEGRATE"), "TRUE") == 0)
			{
				Integrate = SupportClass.CheckBoxSupport.CreateCheckBox("Integrate");
				//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
				Mes_Boutons.Controls.Add(Integrate);
			}
			
			if (getParameter("LOAD_FILE") != null && String.CompareOrdinal(getParameter("LOAD_FILE"), "SHOW") == 0)
			{
				//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
				Mes_Boutons.Controls.Add(Load_File);
			}
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javalangString_javaawtComponent'"
			//UPGRADE_TODO: The following statement was not moved to InitializeComponent. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1307'"
			Controls.Add(Mes_Boutons);
			
			
			/*
			My_ZoneVisu = new ZoneVisu();*/
			initGraphParameters();
			
			/*
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javalangString_javaawtComponent'"
			Controls.Add(My_ZoneVisu);*/
			PerformLayout();
			//UPGRADE_ISSUE: Method 'java.applet.Applet.getAppletContext' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaappletAppletgetAppletContext'"
			contexte = getAppletContext();
			clickable_peaks_frame_name = getParameter("CLICKABLE_PEAKS_FRAME_NAME");
			My_ZoneVisu.Y_Values = getParameter("Y_VALUES");
			My_ZoneVisu.ShowTitle = getParameter("TITLE");
			My_ZoneVisu.Flag_Clickable_Peaks = Load_Clickable_Peaks_Source_File(getParameter("CLICKABLE_PEAKS_SOURCE_FILE"));
			My_ZoneVisu.init();
			My_ZoneVisu.GraphDataUtils = this._graphDataUtils;
			Really_Load_File(getParameter("SOURCE_FILE"));
			if (getParameter("CLICKABLE_PEAKS_FIRST_FRAME") != null)
			{
				My_ZoneVisu.Flag_Load_Now_Html = true;
				My_ZoneVisu.Name_Load_Now_Html = getParameter("CLICKABLE_PEAKS_FIRST_FRAME");
			}
		}
		
		/// <summary> Method to initialize the GraphCharacteristics object in this class</summary>
		private void  initGraphParameters()
		{
			_graphDataUtils = new GraphCharacteristics();
			if (getParameter("GRID") != null && String.CompareOrdinal(getParameter("GRID"), "TRUE") == 0)
			{
				_graphDataUtils.Grid = true;
				My_ZoneVisu.Flag_Grid = true;
			}
			else
			{
				_graphDataUtils.Grid = false;
			}
			
			if (getParameter("INTEGRATION_VALUES") != null)
			{
				_graphDataUtils.UnsortedIntegrationValues = getParameter("INTEGRATION_VALUES");
			}
			
			
			if (getParameter("REVERSE") != null && String.CompareOrdinal(getParameter("REVERSE"), "TRUE") == 0)
			{
				_graphDataUtils.Reverse = true;
				Flag_Reverse = true;
				My_ZoneVisu.reverse = true;
			}
			
			if (getParameter("AXIS_COLOR") != null)
			{
				_graphDataUtils.AxisColor = getParameter("AXIS_COLOR");
			}
			
			if (getParameter("AXIS_TEXT_COLOR") != null)
			{
				_graphDataUtils.AxisTextColor = getParameter("AXIS_TEXT_COLOR");
			}
			
			if (getParameter("INTEGRATE_CURVE_COLOR") != null)
			{
				_graphDataUtils.IntegrateCurveColor = getParameter("INTEGRATE_CURVE_COLOR");
			}
			
			if (getParameter("INTEGRATE_TEXT_COLOR") != null)
			{
				_graphDataUtils.IntegrateTextColor = getParameter("INTEGRATE_TEXT_COLOR");
			}
			
			if (getParameter("GRAPH_CURVE_COLOR") != null)
			{
				_graphDataUtils.GraphCurveColor = getParameter("GRAPH_CURVE_COLOR");
			}
			
			if (getParameter("TEXT_COLOR") != null)
			{
				_graphDataUtils.TextColor = getParameter("TEXT_COLOR");
			}
		}
		public virtual void  start()
		{
			isActiveVar = true;
			if (My_Thread == null)
			{
				My_Thread = new SupportClass.ThreadClass(new System.Threading.ThreadStart(this.Run));
				My_Thread.Start();
			}
			//UPGRADE_ISSUE: Method 'java.applet.Applet.showStatus' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaappletAppletshowStatus_javalangString'"
			showStatus("Ready");
		}
		public virtual void  stop()
		{
			if (My_Thread != null)
			{
				My_Thread.Abort();
				My_Thread = null;
			}
			isActiveVar = false;
		}
		// thread pour que le "please wait" ait le temps de s'afficher et d'autres trucs... "timer"
		public virtual void  Run()
		{
			while (true)
			{
				try
				{
					//UPGRADE_TODO: Method 'java.lang.Thread.sleep' was converted to 'System.Threading.Thread.Sleep' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangThreadsleep_long'"
					System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64) 10000 * 100));
				}
				catch (System.Exception e)
				{
				}
				// TO-DO: Shravan Change 1 - Zoom in and zoom out functionality
				// To be changed for removal of buttons
				if (Flag_Zoomin)
				{
					System.Console.Out.WriteLine("Zooming in!");
					My_ZoneVisu.Zoomin();
					Flag_Zoomin = false;
				}
				if (Flag_Zoomback)
				{
					My_ZoneVisu.Zoomback();
					Flag_Zoomback = false;
				}
				if (Flag_Zoomout)
				{
					My_ZoneVisu.Zoomout();
					Flag_Zoomout = false;
				}
				// END: Shravan Change 1
				/* START: Shravan - No changes required in this section */
				if (Flag_Reverse)
				{
					My_ZoneVisu.Reverse();
					Flag_Reverse = false;
				}
				if (Flag_Integrate)
				{
					My_ZoneVisu.Flag_Integrate = true;
					My_ZoneVisu.Redraw();
					Flag_Integrate = false;
				}
				/* END - no changes*/
				if (Flag_Find_Peak)
				{
					My_ZoneVisu.Find_Peak();
					Flag_Find_Peak = false;
				}
				if (Flag_Write_JCamp && getParameter("SOURCE_FILE").ToLower().EndsWith("dx"))
				{
					Write_JCamp();
					Flag_Write_JCamp = false;
				}
				if (Flag_Redraw)
				{
					My_ZoneVisu.Redraw();
					Flag_Redraw = false;
				}
				if (My_ZoneVisu.Flag_Load_Now_Html)
				{
					My_ZoneVisu.Flag_Load_Now_Html = false;
					try
					{
						//UPGRADE_ISSUE: Method 'java.applet.AppletContext.showDocument' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaappletAppletContext'"
						//UPGRADE_TODO: Class 'java.net.URL' was converted to a 'System.Uri' which does not throw an exception if a URL specifies an unknown protocol. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1132'"
						//UPGRADE_TODO: Method 'java.applet.Applet.getDocumentBase' was converted to 'DocumentBase' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaappletAppletgetDocumentBase'"
						contexte.showDocument(new System.Uri(DocumentBase, My_ZoneVisu.Name_Load_Now_Html), clickable_peaks_frame_name);
					}
					catch (System.UriFormatException e)
					{
					}
				}
				if (Flag_Dialogue_File_Enabled && Dialogue_Load_File.fin)
				{
					Flag_Dialogue_File_Enabled = false;
					Dialogue_Load_File.fin = false;
					if (Dialogue_Load_File.OkStatus)
						Really_Load_File(Dialogue_Load_File.lisSaisie());
				}
			}
		}
		public virtual bool Load_Clickable_Peaks_Source_File(System.String tam)
		{
			if (tam == null)
				return false;
			try
			{
				//UPGRADE_TODO: Class 'java.net.URL' was converted to a 'System.Uri' which does not throw an exception if a URL specifies an unknown protocol. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1132'"
				//UPGRADE_TODO: Method 'java.applet.Applet.getDocumentBase' was converted to 'DocumentBase' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaappletAppletgetDocumentBase'"
				System.Uri url = new System.Uri(DocumentBase, tam);
				System.IO.Stream stream = System.Net.WebRequest.Create(url).GetResponse().GetResponseStream();
				//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
				System.IO.BinaryReader fichier = new System.IO.BinaryReader(stream);
				texte = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
				System.String s;
				//UPGRADE_TODO: Method 'java.io.DataInputStream.readLine' was converted to 'System.IO.BinaryReader.ReadString' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStreamreadLine'"
				while ((s = fichier.ReadString()) != null)
				{
					texte.Add(s);
				}
				My_ZoneVisu.Nb_Clickable_Peaks = texte.Count;
			}
			catch (System.Exception e)
			{
				return false;
			}
			My_ZoneVisu.Peak_Start = new double[My_ZoneVisu.Nb_Clickable_Peaks];
			My_ZoneVisu.Peak_Stop = new double[My_ZoneVisu.Nb_Clickable_Peaks];
			My_ZoneVisu.Peak_Html = new System.String[My_ZoneVisu.Nb_Clickable_Peaks];
			int cpt_tokens = 0;
			int i = 0;
			SupportClass.Tokenizer mon_token;
			while (cpt_tokens < My_ZoneVisu.Nb_Clickable_Peaks)
			{
				do 
				{
					System.String mysub = (System.String) texte[cpt_tokens];
					mon_token = new SupportClass.Tokenizer(mysub, " ");
					cpt_tokens++;
				}
				while (cpt_tokens < My_ZoneVisu.Nb_Clickable_Peaks && mon_token.HasMoreTokens() == false);
				if (mon_token.HasMoreTokens() == true)
				{
					My_ZoneVisu.Peak_Start[i] = System.Double.Parse(mon_token.NextToken());
					My_ZoneVisu.Peak_Stop[i] = System.Double.Parse(mon_token.NextToken());
					if (My_ZoneVisu.Peak_Start[i] > My_ZoneVisu.Peak_Stop[i])
					{
						double temp = My_ZoneVisu.Peak_Start[i];
						My_ZoneVisu.Peak_Start[i] = My_ZoneVisu.Peak_Stop[i];
						My_ZoneVisu.Peak_Stop[i] = temp;
					}
					My_ZoneVisu.Peak_Html[i] = ((System.String) mon_token.NextToken());
				}
				i++;
			}
			return true;
		}
		
		public virtual System.String Move_Points_To_Tableau()
		{
			int indice = 0;
			int nbp = Nbpoints;
			if (String.CompareOrdinal(Datatype, "XYDATA") == 0)
			{
				// se place sur la premiere ligne de donnees
				while (String.CompareOrdinal(StringDataUtils.jcampSubString(((System.String) texte[indice]), 0, 8), "##XYDATA") != 0)
					indice++;
				indice++;
				int indicetableau = 0;
				System.String Un_Nombre;
				My_ZoneVisu.tableau_points = new double[Nbpoints];
				double[] tmp_tab = new double[Nbpoints];
				while (nbp > 0 && indice < nbLignes)
				{
					SupportClass.Tokenizer mon_token;
					do 
					{
						System.String mysub = (System.String) texte[indice];
						indice++;
						mon_token = new SupportClass.Tokenizer(mysub, " ");
					}
					while (indice < nbLignes && mon_token.HasMoreTokens() == false);
					if (mon_token.HasMoreTokens())
					{
						Un_Nombre = mon_token.NextToken();
						while (nbp > 0 && (mon_token.HasMoreTokens() == true || Un_Nombre.IndexOf('-') > 1))
						{
							if (Un_Nombre.IndexOf('-') <= 1)
								Un_Nombre = mon_token.NextToken();
							else
								Un_Nombre = Un_Nombre.Substring(Un_Nombre.IndexOf('-') + 1);
							// dans certains fichiers certains nombres sont separes par un '-'
							while (Un_Nombre.IndexOf('-') > 1)
							{
								nbp--;
								try
								{
									My_ZoneVisu.tableau_points[indicetableau] = System.Double.Parse(Un_Nombre.Substring(0, (Un_Nombre.IndexOf('-')) - (0)));
								}
								catch (System.Exception e)
								{
									return ", wrong number format";
								}
								indicetableau++;
								Un_Nombre = Un_Nombre.Substring(Un_Nombre.IndexOf('-') + 1);
							}
							nbp--;
							try
							{
								My_ZoneVisu.tableau_points[indicetableau] = System.Double.Parse(Un_Nombre);
							}
							catch (System.Exception e)
							{
								return ", wrong number format";
							}
							indicetableau++;
						}
					}
				}
				
				if (Firstx > Lastx)
				{
					for (int i = 0; i < Nbpoints; i++)
						tmp_tab[i] = My_ZoneVisu.tableau_points[i];
					for (int i = 0; i < Nbpoints; i++)
						My_ZoneVisu.tableau_points[i] = tmp_tab[Nbpoints - i - 1];
					double tmp = Firstx;
					Firstx = Lastx;
					Lastx = tmp;
				}
			}
			if (String.CompareOrdinal(Datatype, "PEAK TABLE") == 0)
			{
				// se place sur la premiere ligne de donnees
				while (String.CompareOrdinal(((System.String) texte[indice]).Substring(0, (6) - (0)), "##PEAK") != 0)
					indice++;
				indice++;
				int indicetableau = 0;
				System.String Un_Nombre;
				My_ZoneVisu.tableau_points = new double[Nbpoints * 2];
				double[] tmp_tab = new double[Nbpoints * 2];
				while (nbp > 0 && indice < nbLignes)
				{
					SupportClass.Tokenizer mon_token;
					do 
					{
						System.String mysub = (System.String) texte[indice];
						indice++;
						mon_token = new SupportClass.Tokenizer(mysub, " ");
					}
					while (indice < nbLignes && mon_token.HasMoreTokens() == false);
					if (mon_token.HasMoreTokens())
					{
						while (nbp > 0 && mon_token.HasMoreTokens() == true)
						{
							Un_Nombre = mon_token.NextToken();
							nbp--;
							My_ZoneVisu.tableau_points[indicetableau] = System.Double.Parse(Un_Nombre.Substring(0, (Un_Nombre.IndexOf(',')) - (0)));
							indicetableau++;
							if (Un_Nombre.IndexOf(',') == Un_Nombre.Length - 1)
							// cas de x, y
								My_ZoneVisu.tableau_points[indicetableau] = System.Double.Parse(mon_token.NextToken());
							// cas x,y
							else
								My_ZoneVisu.tableau_points[indicetableau] = System.Double.Parse(Un_Nombre.Substring(Un_Nombre.IndexOf(',') + 1));
							indicetableau++;
						}
					}
				}
				// Dans certains fichiers "Peak Table" il n'y a pas Firstx/Lastx
				if (Firstx == shitty_starting_constant)
					Firstx = My_ZoneVisu.tableau_points[0];
				if (Lastx == shitty_starting_constant)
					Lastx = My_ZoneVisu.tableau_points[(Nbpoints - 1) * 2];
				
				if (Firstx > Lastx)
				{
					for (int i = 0; i < Nbpoints; i++)
						tmp_tab[i * 2] = My_ZoneVisu.tableau_points[i * 2];
					for (int i = 0; i < Nbpoints; i++)
						My_ZoneVisu.tableau_points[i * 2] = tmp_tab[Nbpoints * 2 - i * 2 - 1];
					for (int i = 0; i < Nbpoints; i++)
						tmp_tab[i * 2 + 1] = My_ZoneVisu.tableau_points[i * 2 + 1];
					for (int i = 0; i < Nbpoints; i++)
						My_ZoneVisu.tableau_points[i * 2 + 1] = tmp_tab[Nbpoints * 2 - i * 2];
					double tmp = Firstx;
					Firstx = Lastx;
					Lastx = tmp;
				}
			}
			if (nbp > 0)
				return ", file corrupted or unsupported file format";
			return "OK";
		}
		public virtual bool initFile(System.String filename)
		{
			//UPGRADE_ISSUE: Method 'java.applet.Applet.showStatus' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaappletAppletshowStatus_javalangString'"
			showStatus("Loading the file, please wait...");
			x_units = "?";
			y_units = "ARBITRARY";
			Datatype = "UNKNOWN";
			if (filename.ToLower().EndsWith(".spc"))
			{
				// Il s'agit d'un fichier SPC
				try
				{
					//UPGRADE_TODO: Class 'java.net.URL' was converted to a 'System.Uri' which does not throw an exception if a URL specifies an unknown protocol. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1132'"
					//UPGRADE_TODO: Method 'java.applet.Applet.getDocumentBase' was converted to 'DocumentBase' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaappletAppletgetDocumentBase'"
					System.Uri url = new System.Uri(DocumentBase, filename);
					System.IO.Stream stream = System.Net.WebRequest.Create(url).GetResponse().GetResponseStream();
					//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
					System.IO.BinaryReader fichier = new System.IO.BinaryReader(stream);
					// Lecture de SPCHDR (512 octets)
					sbyte ftflgs = (sbyte) fichier.ReadByte(); // Flag Bits
					sbyte fversn = (sbyte) fichier.ReadByte(); // Version
					if (((ftflgs != 0) && (ftflgs != 0x20)) || (fversn != 0x4B))
					{
						Current_Error = ", support only Evenly Spaced new version 4B"; return false;
					}
					
					//Commented by Shravan Sadasivan - Variable not used
					// byte fexper = fichier.readByte();          // Instrument technique code (on s'en fout)
					sbyte fexp = (sbyte) fichier.ReadByte(); // Fraction scaling exponent integer
					if (fexp != 0x80)
						YFactor = System.Math.Pow(2, (byte) fexp) / System.Math.Pow(2, 32);
					Nbpoints = NumericDataUtils.convToIntelInt(fichier.ReadInt32());
					if (Firstx == shitty_starting_constant)
					{
						Firstx = NumericDataUtils.convToIntelDouble(fichier.ReadInt64());
						Lastx = NumericDataUtils.convToIntelDouble(fichier.ReadInt64());
					}
					//Commented by Shravan Sadasivan - Variable not used
					// int fnsub = fichier.readInt();                // Integer Number of Subfiles (on s'en fout)
					sbyte fxtype = (sbyte) fichier.ReadByte();
					switch (fxtype)
					{
						
						case 0:  x_units = "Arbitrary"; break;
						
						case 1:  x_units = "Wavenumber (cm -1)"; break;
						
						case 2:  x_units = "Micrometers"; break;
						
						case 3:  x_units = "Nanometers"; break;
						
						case 4:  x_units = "Seconds"; break;
						
						case 5:  x_units = "Minuts"; break;
						
						case 6:  x_units = "Hertz"; break;
						
						case 7:  x_units = "Kilohertz"; break;
						
						case 8:  x_units = "Megahertz"; break;
						
						case 9:  x_units = "Mass (M/z)"; break;
						
						case 10:  x_units = "Parts per million"; break;
						
						case 11:  x_units = "Days"; break;
						
						case 12:  x_units = "Years"; break;
						
						case 13:  x_units = "Raman Shift (cm -1)"; break;
						
						case 14:  x_units = "Electron Volt (eV)"; break;
						
						case 16:  x_units = "Diode Number"; break;
						
						case 17:  x_units = "Channel"; break;
						
						case 18:  x_units = "Degrees"; break;
						
						case 19:  x_units = "Temperature (F)"; break;
						
						case 20:  x_units = "Temperature (C)"; break;
						
						case 21:  x_units = "Temperature (K)"; break;
						
						case 22:  x_units = "Data Points"; break;
						
						case 23:  x_units = "Milliseconds (mSec)"; break;
						
						case 24:  x_units = "Microseconds (uSec)"; break;
						
						case 25:  x_units = "Nanoseconds (nSec)"; break;
						
						case 26:  x_units = "Gigahertz (GHz)"; break;
						
						case 27:  x_units = "Centimeters (cm)"; break;
						
						case 28:  x_units = "Meters (m)"; break;
						
						case 29:  x_units = "Millimeters (mm)"; break;
						
						case 30:  x_units = "Hours"; break;
						
						case - 1:  x_units = "(double interferogram)"; break;
						}
					sbyte fytype = (sbyte) fichier.ReadByte();
					switch (fytype)
					{
						
						case 0:  y_units = "Arbitrary Intensity"; break;
						
						case 1:  y_units = "Interfeogram"; break;
						
						case 2:  y_units = "Absorbance"; break;
						
						case 3:  y_units = "Kubelka-Munk"; break;
						
						case 4:  y_units = "Counts"; break;
						
						case 5:  y_units = "Volts"; break;
						
						case 6:  y_units = "Degrees"; break;
						
						case 7:  y_units = "Milliamps"; break;
						
						case 8:  y_units = "Millimeters"; break;
						
						case 9:  y_units = "Millivolts"; break;
						
						case 10:  y_units = "Log (1/R)"; break;
						
						case 11:  y_units = "Percent"; break;
						
						case 12:  y_units = "Intensity"; break;
						
						case 13:  y_units = "Relative Intensity"; break;
						
						case 14:  y_units = "Energy"; break;
						
						case 16:  y_units = "Decibel"; break;
						
						case 19:  y_units = "Temperature (F)"; break;
						
						case 20:  y_units = "Temperature (C)"; break;
						
						case 21:  y_units = "Temperature (K)"; break;
						
						case 22:  y_units = "Index of Refraction [N]"; break;
						
						case 23:  y_units = "Extinction Coeff. [K]"; break;
						
						case 24:  y_units = "Real"; break;
						
						case 25:  y_units = "Imaginary"; break;
						
						case 26:  y_units = "Complex"; break;
						
						case - 128:  y_units = "Transmission"; break;
						
						case - 127:  y_units = "Reflectance"; break;
						
						case - 126:  y_units = "Arbitrary or Single Beam with Valley Peaks"; break;
						
						case - 125:  y_units = "Emission"; break;
						}
					// lecture du reste du SPCHDR
					// Cas general
					if (ftflgs == 0)
					{
						System.IO.BinaryReader temp_BinaryReader;
						System.Int64 temp_Int64;
						temp_BinaryReader = fichier;
						temp_Int64 = temp_BinaryReader.BaseStream.Position;
						temp_Int64 = temp_BinaryReader.BaseStream.Seek(512 - 30, System.IO.SeekOrigin.Current) - temp_Int64;
						int generatedAux3 = temp_Int64;
					}
					else
					{
						// Cas ou les labels des axes sont donnes textuellement
						System.IO.BinaryReader temp_BinaryReader2;
						System.Int64 temp_Int65;
						temp_BinaryReader2 = fichier;
						temp_Int65 = temp_BinaryReader2.BaseStream.Position;
						temp_Int65 = temp_BinaryReader2.BaseStream.Seek(188, System.IO.SeekOrigin.Current) - temp_Int65;
						int generatedAux4 = temp_Int65;
						sbyte b;
						int i = 0; x_units = "";
						do 
						{
							b = (sbyte) fichier.ReadByte(); x_units += (char) b; i++;
						}
						while (b != 0);
						int j = 0; y_units = "";
						do 
						{
							b = (sbyte) fichier.ReadByte(); y_units += (char) b; j++;
						}
						while (b != 0);
						System.IO.BinaryReader temp_BinaryReader3;
						System.Int64 temp_Int66;
						temp_BinaryReader3 = fichier;
						temp_Int66 = temp_BinaryReader3.BaseStream.Position;
						temp_Int66 = temp_BinaryReader3.BaseStream.Seek(512 - 30 - 188 - i - j, System.IO.SeekOrigin.Current) - temp_Int66;
						int generatedAux5 = temp_Int66;
					}
					// Lecture de SUBHDR (512 octets)
					System.IO.BinaryReader temp_BinaryReader4;
					System.Int64 temp_Int67;
					temp_BinaryReader4 = fichier;
					temp_Int67 = temp_BinaryReader4.BaseStream.Position;
					temp_Int67 = temp_BinaryReader4.BaseStream.Seek(32, System.IO.SeekOrigin.Current) - temp_Int67;
					int generatedAux6 = temp_Int67; // (on s'en fout)
					// Lecture des donnees
					My_ZoneVisu.tableau_points = new double[Nbpoints];
					if (fexp == 0x80)
					{
						for (int i = 0; i < Nbpoints; i++)
						{
							My_ZoneVisu.tableau_points[i] = NumericDataUtils.convToIntelFloat(fichier.ReadInt32());
						}
					}
					else
					{
						for (int i = 0; i < Nbpoints; i++)
						{
							My_ZoneVisu.tableau_points[i] = NumericDataUtils.convToIntelInt(fichier.ReadInt32());
						}
					}
				}
				catch (System.Exception e)
				{
					Current_Error = "SPC file corrupted"; return false;
				}
				Datatype = "XYDATA";
				return true;
			}
			// Il s'agit d'un fichier JCAMP
			// On met tout le fichier dans la variable globale "Vector texte"
			try
			{
				//UPGRADE_TODO: Class 'java.net.URL' was converted to a 'System.Uri' which does not throw an exception if a URL specifies an unknown protocol. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1132'"
				//UPGRADE_TODO: Method 'java.applet.Applet.getDocumentBase' was converted to 'DocumentBase' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaappletAppletgetDocumentBase'"
				System.Uri url = new System.Uri(DocumentBase, filename);
				System.IO.Stream stream = System.Net.WebRequest.Create(url).GetResponse().GetResponseStream();
				//      DataInputStream fichier = new DataInputStream(stream);
				//UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
				//UPGRADE_WARNING: At least one expression was used more than once in the target code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1181'"
				System.IO.StreamReader fichier = new System.IO.StreamReader(new System.IO.StreamReader(stream, System.Text.Encoding.Default).BaseStream, new System.IO.StreamReader(stream, System.Text.Encoding.Default).CurrentEncoding);
				texte = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
				System.String s;
				while ((s = fichier.ReadLine()) != null)
				{
					texte.Add(s);
				}
				nbLignes = texte.Count;
			}
			catch (System.Exception e)
			{
				return false;
			}
			int My_Counter = 0;
			System.String uneligne = "";
			while (My_Counter < nbLignes)
			{
				try
				{
					SupportClass.Tokenizer mon_token;
					do 
					{
						uneligne = ((System.String) texte[My_Counter]);
						My_Counter++;
						mon_token = new SupportClass.Tokenizer(uneligne, " ");
					}
					while (My_Counter < nbLignes && mon_token.HasMoreTokens() == false);
					if (mon_token.HasMoreTokens() == true)
					{
						System.String keyword = mon_token.NextToken();
						if (StringDataUtils.compareStrings(keyword, "##TITLE=") == 0)
							TexteTitre = uneligne.Substring(9);
						if (StringDataUtils.compareStrings(keyword, "##FIRSTX=") == 0)
							Firstx = System.Double.Parse(mon_token.NextToken());
						if (StringDataUtils.compareStrings(keyword, "##LASTX=") == 0)
							Lastx = System.Double.Parse(mon_token.NextToken());
						if (StringDataUtils.compareStrings(keyword, "##YFACTOR=") == 0)
							YFactor = System.Double.Parse(mon_token.NextToken());
						if (StringDataUtils.compareStrings(keyword, "##NPOINTS=") == 0)
							Nbpoints = System.Int32.Parse(mon_token.NextToken());
						if (StringDataUtils.compareStrings(keyword, "##XUNITS=") == 0)
							x_units = uneligne.Substring(10);
						if (StringDataUtils.compareStrings(keyword, "##YUNITS=") == 0)
							y_units = uneligne.Substring(10);
						if (StringDataUtils.compareStrings(keyword, "##.OBSERVE") == 0 && StringDataUtils.compareStrings(mon_token.NextToken(), "FREQUENCY=") == 0)
							nmr_observe_frequency = System.Double.Parse(mon_token.NextToken());
						if (StringDataUtils.compareStrings(keyword, "##XYDATA=") == 0 && StringDataUtils.compareStrings(mon_token.NextToken(), "(X++(Y..Y))") == 0)
							Datatype = "XYDATA";
						if (StringDataUtils.compareStrings(keyword, "##XYDATA=(X++(Y..Y))") == 0)
							Datatype = "XYDATA";
						if (StringDataUtils.compareStrings(keyword, "##PEAK") == 0 && StringDataUtils.compareStrings(mon_token.NextToken(), "TABLE=") == 0 && StringDataUtils.compareStrings(mon_token.NextToken(), "(XY..XY)") == 0)
							Datatype = "PEAK TABLE";
						if (StringDataUtils.compareStrings(keyword, "##PEAK") == 0 && StringDataUtils.compareStrings(mon_token.NextToken(), "TABLE=(XY..XY)") == 0)
							Datatype = "PEAK TABLE";
					}
				}
				catch (System.Exception e)
				{
				}
			}
			/*if(getParameter("XUPPERLIMIT") != null && getParameter("XLOWERLIMIT") != null){
			Firstx = Double.valueOf(getParameter("XLOWERLIMIT"));
			Lastx = Double.valueOf(getParameter("XUPPERLIMIT"));
			}*/
			if (String.CompareOrdinal(Datatype, "UNKNOWN") == 0)
				return false;
			if (String.CompareOrdinal(Datatype, "PEAK TABLE") == 0 && String.CompareOrdinal(x_units, "?") == 0)
				x_units = "M/Z";
			// conversion Hz --> PPM
			if (String.CompareOrdinal(StringDataUtils.truncateEndBlanks(x_units), "HZ") == 0 && nmr_observe_frequency != shitty_starting_constant)
			{
				Firstx /= nmr_observe_frequency;
				Lastx /= nmr_observe_frequency;
				x_units = "PPM.";
			}
			System.String resultat_move_points = Move_Points_To_Tableau();
			if (String.CompareOrdinal(resultat_move_points, "OK") != 0)
			{
				Current_Error = resultat_move_points;
				return false;
			}
			return true;
		}
		public virtual void  Really_Load_File(System.String chaine_a_lire)
		{
			/*if(getParameter("XUPPERLIMIT") != null && getParameter("XLOWERLIMIT") != null){
			System.out.println("In here!");
			Firstx = Double.valueOf(getParameter("XLOWERLIMIT"));
			Lastx = Double.valueOf(getParameter("XUPPERLIMIT"));
			}else{*/
			Firstx = shitty_starting_constant;
			Lastx = shitty_starting_constant;
			/*}*/
			Nbpoints = shitty_starting_constant;
			nmr_observe_frequency = shitty_starting_constant;
			TexteTitre = "";
			YFactor = 1;
			My_ZoneVisu.Draw_Texte("Drawing graphics, please wait...");
			if (initFile(chaine_a_lire))
			{
				My_ZoneVisu.Init_File();
				//if (Reverse.getState()) { My_ZoneVisu.Flag_Reverse=true; Flag_Reverse=true; } else Flag_Redraw=true;
				if (Flag_Reverse)
				{
					My_ZoneVisu.Flag_Reverse = true;
				}
				else
					Flag_Redraw = true;
			}
			else
				My_ZoneVisu.Draw_Texte("Bad file or filename" + Current_Error);
		}
		public virtual void  Write_JCamp()
		{
			//
			//		Write JCAMP-DX records to new file on client
			//
			// Commented by Shravan Sadasivan for testing
			/* if (inNavigator) {
			if (hasPrivilege == false) {
			System.out.println("\n\nNecessary write privilege has not been granted.");
			return;
			}
			PrivilegeManager.enablePrivilege("UniversalFileAccess");
			PrivilegeManager.enablePrivilege("UniversalPropertyRead");
			} */
			System.String dirName, filName2, newLine;
			System.IO.StreamWriter pw;
			filName2 = getParameter("SOURCE_FILE").ToLower();
			int ipos = filName2.LastIndexOf("/") + 1;
			if (ipos == - 1)
				ipos = 0;
			filName2 = filName2.Substring(ipos, (filName2.Length) - (ipos));
			System.Console.Out.WriteLine("FileName : " + filName2);
			try
			{
				if (inNavigator)
				{
					saveDialog.FileName = filName2;
					saveDialog.ShowDialog();
					dirName = saveDialog.InitialDirectory;
					//UPGRADE_TODO: Method 'java.awt.FileDialog.getFile' was converted to 'System.Windows.Forms.FileDialog.FileName' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtFileDialoggetFile'"
					filName2 = dirName + saveDialog.FileName;
				}
				//UPGRADE_WARNING: At least one expression was used more than once in the target code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1181'"
				//UPGRADE_TODO: Constructor 'java.io.FileWriter.FileWriter' was converted to 'System.IO.StreamWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileWriterFileWriter_javalangString'"
				//UPGRADE_TODO: Class 'java.io.FileWriter' was converted to 'System.IO.StreamWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileWriter'"
				pw = new System.IO.StreamWriter(new System.IO.StreamWriter(filName2, false, System.Text.Encoding.Default).BaseStream, new System.IO.StreamWriter(filName2, false, System.Text.Encoding.Default).Encoding);
				for (int ii = 0; ii < texte.Count; ii++)
				{
					newLine = ((System.String) texte[ii]);
					//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
					pw.WriteLine(newLine);
				}
				//UPGRADE_NOTE: Exceptions thrown by the equivalent in .NET of method 'java.io.PrintWriter.close' may be different. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1099'"
				pw.Close();
			}
			catch (System.Exception e)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				System.Console.Out.WriteLine("\nClient: Unable to write local JCAMP-DX file. " + e);
			}
		}
		public virtual void  Do_Zoomin()
		{
			My_ZoneVisu.Draw_Texte("Zooming in, please wait...");
			Flag_Zoomin = true;
		}
		public virtual void  Do_Zoomback()
		{
			My_ZoneVisu.Draw_Texte("Zooming back, please wait...");
			Flag_Zoomback = true;
		}
		public virtual void  Do_Zoomout()
		{
			My_ZoneVisu.Draw_Texte("Drawing whole graphics, please wait...");
			Flag_Zoomout = true;
		}
		/*
		Shravan Sadasivan - Changes need to be made to this section
		at the time of implementation of the parameterization of the Grid option
		requirement.
		*/
		public virtual void  Do_Grid()
		{
			My_ZoneVisu.Draw_Texte("Redrawing with grid, please wait...");
			//My_ZoneVisu.Flag_Grid=Grid.getState();
			Flag_Redraw = true;
		}
		public virtual void  Do_Reverse()
		{
			My_ZoneVisu.Draw_Texte("Reversing graphics, please wait...");
			My_ZoneVisu.Flag_Reverse = Reverse.Checked;
			Flag_Reverse = true;
		}
		/*
		Shravan Sadasivan - Changes need to be made to this section
		at the time of implementation of the parameterization of the Integrate option
		requirement.
		*/
		public virtual void  Do_Integrate()
		{
			if (Integrate.Checked)
			{
				My_ZoneVisu.Draw_Texte("Integrating peaks, please wait...");
				Flag_Integrate = true;
			}
			else
			{
				My_ZoneVisu.Flag_Integrate = false;
				My_ZoneVisu.Draw_Texte("Redrawing graphics, please wait...");
				Flag_Redraw = true;
			}
		}
		public virtual void  Do_Find_Peak()
		{
			My_ZoneVisu.Draw_Texte("Finding peak, please wait...");
			Flag_Find_Peak = true;
		}
		public virtual void  Do_Write_JCamp()
		{
			Flag_Write_JCamp = true;
		}
		public virtual void  Do_Load_File()
		{
			//UPGRADE_TODO: Class 'java.awt.Frame' was converted to 'System.Windows.Forms.Form' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtFrame'"
			System.Windows.Forms.Form Frame_Load_File = (System.Windows.Forms.Form) Parent;
			Dialogue_Load_File = new SaisieDlg(Frame_Load_File, "Load file...", "Enter the filename :");
			//UPGRADE_TODO: Method 'java.awt.Dialog.show' was converted to 'System.Windows.Forms.Form.ShowDialog' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtDialogshow'"
			Dialogue_Load_File.ShowDialog();
			Flag_Dialogue_File_Enabled = true;
		}
		//UPGRADE_NOTE: The equivalent of method 'java.awt.Component.handleEvent' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		//UPGRADE_ISSUE: Class 'java.awt.Event' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
		public bool handleEvent(Event evt)
		{
			//UPGRADE_ISSUE: Field 'java.awt.Event.target' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
			//UPGRADE_ISSUE: Field 'java.awt.Event.id' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
			//UPGRADE_ISSUE: Field 'java.awt.Event.ACTION_EVENT' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
			if (evt.target == Load_File && evt.id == Event.ACTION_EVENT)
			{
				Do_Load_File();
				return true;
			}
			//UPGRADE_ISSUE: Field 'java.awt.Event.target' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
			//UPGRADE_ISSUE: Field 'java.awt.Event.id' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
			//UPGRADE_ISSUE: Field 'java.awt.Event.ACTION_EVENT' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
			if (evt.target == Zoom_In && evt.id == Event.ACTION_EVENT)
			{
				Do_Zoomin();
				return true;
			}
			//UPGRADE_ISSUE: Field 'java.awt.Event.target' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
			//UPGRADE_ISSUE: Field 'java.awt.Event.id' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
			//UPGRADE_ISSUE: Field 'java.awt.Event.ACTION_EVENT' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
			if (evt.target == Zoom_Back && evt.id == Event.ACTION_EVENT)
			{
				Do_Zoomback();
				return true;
			}
			//UPGRADE_ISSUE: Field 'java.awt.Event.target' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
			//UPGRADE_ISSUE: Field 'java.awt.Event.id' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
			//UPGRADE_ISSUE: Field 'java.awt.Event.ACTION_EVENT' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
			if (evt.target == Zoom_Out && evt.id == Event.ACTION_EVENT)
			{
				Do_Zoomout();
				return true;
			}
			//UPGRADE_ISSUE: Field 'java.awt.Event.target' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
			//UPGRADE_ISSUE: Field 'java.awt.Event.id' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
			//UPGRADE_ISSUE: Field 'java.awt.Event.ACTION_EVENT' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
			if (evt.target == Reverse && evt.id == Event.ACTION_EVENT)
			{
				Do_Reverse();
				return true;
			}
			//UPGRADE_ISSUE: Field 'java.awt.Event.target' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
			//UPGRADE_ISSUE: Field 'java.awt.Event.id' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
			//UPGRADE_ISSUE: Field 'java.awt.Event.ACTION_EVENT' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
			if (evt.target == Grid && evt.id == Event.ACTION_EVENT)
			{
				Do_Grid();
				return true;
			}
			//UPGRADE_ISSUE: Field 'java.awt.Event.target' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
			//UPGRADE_ISSUE: Field 'java.awt.Event.id' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
			//UPGRADE_ISSUE: Field 'java.awt.Event.ACTION_EVENT' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
			if (evt.target == Integrate && evt.id == Event.ACTION_EVENT)
			{
				Do_Integrate();
				return true;
			}
			//UPGRADE_ISSUE: Field 'java.awt.Event.target' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
			//UPGRADE_ISSUE: Field 'java.awt.Event.id' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
			//UPGRADE_ISSUE: Field 'java.awt.Event.ACTION_EVENT' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
			if (evt.target == Find_Peak && evt.id == Event.ACTION_EVENT)
			{
				Do_Find_Peak();
				return true;
			}
			//UPGRADE_ISSUE: Field 'java.awt.Event.target' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
			//UPGRADE_ISSUE: Field 'java.awt.Event.id' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
			//UPGRADE_ISSUE: Field 'java.awt.Event.ACTION_EVENT' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
			if (evt.target == JCamp && evt.id == Event.ACTION_EVENT)
			{
				Do_Write_JCamp();
				return true;
			}
			//UPGRADE_ISSUE: Method 'java.awt.Component.handleEvent' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtComponenthandleEvent_javaawtEvent'"
			return base.handleEvent(evt);
		}
		public void  ResizeControl(System.Drawing.Size p)
		{
			this.Width = p.Width;
			this.Height = p.Height;
		}
		public void  ResizeControl(int p2, int p3)
		{
			this.Width = p2;
			this.Height = p3;
		}
		public System.String GetUserControlInfo()
		{
			return null;
		}
		public System.String[][] GetParameterInfo()
		{
			return null;
		}
		public System.String  TempDocumentBaseVar = "";
		public virtual System.Uri DocumentBase
		{
			get
			{
				if (TempDocumentBaseVar == "")
					return new System.Uri("http://127.0.0.1");
				else
					return new System.Uri(TempDocumentBaseVar);
			}
			
		}
		public System.Drawing.Image getImage(System.Uri p4)
		{
			Bitmap TemporalyBitmap = new Bitmap(p4.AbsolutePath);
			return (Image) TemporalyBitmap;
		}
		public System.Drawing.Image getImage(System.Uri p5, System.String p6)
		{
			Bitmap TemporalyBitmap = new Bitmap(p5.AbsolutePath + p6);
			return (Image) TemporalyBitmap;
		}
		public virtual System.Boolean isActive()
		{
			return isActiveVar;
		}
		private void  Visua_StartEventHandler(System.Object sender, System.EventArgs e)
		{
			init();
			start();
		}
		private void  Visua_StopEventHandler(System.Object sender, System.EventArgs e)
		{
			stop();
		}
		public virtual String getParameter(System.String paramName)
		{
			return null;
		}
		#region Windows Form Designer generated code
		private void  InitializeComponent()
		{
			this.SuspendLayout();
			this.BackColor = Color.LightGray;
			My_ZoneVisu = new ZoneVisu();
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javalangString_javaawtComponent'"
			Controls.Add(My_ZoneVisu);
			this.ResumeLayout(false);
		}
		#endregion
		public void  Mes_Boutons_setLayout(System.Object event_sender, System.Windows.Forms.LayoutEventArgs e)
		{
			SupportClass.GridLayoutResize(event_sender, e);
		}
	}
}