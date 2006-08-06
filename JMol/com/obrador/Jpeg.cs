// Copyright (C) 1998, James R. Weeks and BioElectroMech.
// Visit BioElectroMech at www.obrador.com.  Email James@obrador.com.

// This software is based in part on the work of the Independent JPEG Group.
// See license.txt for details about the allowed used of this software.
// See IJGreadme.txt for details about the Independent JPEG Group's license.
using System;
namespace com.obrador
{
	
	
	public class Jpeg
	{
		/// <summary>*********** Main Method ***************</summary>
		/// <summary> 
		/// Jpeg("Imagefile", Quality, "OutFileName")
		/// According to JAVA virtual machine, the files which can be read are jpeg, tiff and gif files
		/// *
		/// </summary>
		
		public static void  StandardUsage()
		{
			System.Console.Out.WriteLine("JpegEncoder for Java(tm) Version 0.9");
			System.Console.Out.WriteLine("");
			System.Console.Out.WriteLine("Program usage: java Jpeg \"InputImage\".\"ext\" Quality [\"OutputFile\"[.jpg]]");
			System.Console.Out.WriteLine("");
			System.Console.Out.WriteLine("Where \"InputImage\" is the name of an existing image in the current directory.");
			System.Console.Out.WriteLine("  (\"InputImage may specify a directory, too.) \"ext\" must be .tif, .gif,");
			System.Console.Out.WriteLine("  or .jpg.");
			System.Console.Out.WriteLine("Quality is an integer (0 to 100) that specifies how similar the compressed");
			System.Console.Out.WriteLine("  image is to \"InputImage.\"  100 is almost exactly like \"InputImage\" and 0 is");
			System.Console.Out.WriteLine("  most dissimilar.  In most cases, 70 - 80 gives very good results.");
			System.Console.Out.WriteLine("\"OutputFile\" is an optional argument.  If \"OutputFile\" isn't specified, then");
			System.Console.Out.WriteLine("  the input file name is adopted.  This program will NOT write over an existing");
			System.Console.Out.WriteLine("  file.  If a directory is specified for the input image, then \"OutputFile\"");
			System.Console.Out.WriteLine("  will be written in that directory.  The extension \".jpg\" may automatically be");
			System.Console.Out.WriteLine("  added.");
			System.Console.Out.WriteLine("");
			System.Console.Out.WriteLine("Copyright 1998 BioElectroMech and James R. Weeks.  Portions copyright IJG and");
			System.Console.Out.WriteLine("  Florian Raemy, LCAV.  See license.txt for details.");
			System.Console.Out.WriteLine("Visit BioElectroMech at www.obrador.com.  Email James@obrador.com.");
			System.Environment.Exit(0);
		}
		
		[STAThread]
		public static void  Main(System.String[] args)
		{
			System.Drawing.Image image = null;
			System.IO.FileStream dataOut = null;
			System.IO.FileInfo file, outFile;
			JpegEncoder jpg;
			System.String string_Renamed = new System.Text.StringBuilder().ToString();
			int i, Quality = 80;
			// Check to see if the input file name has one of the extensions:
			//     .tif, .gif, .jpg
			// If not, print the standard use info.
			if (args.Length < 2)
				StandardUsage();
			if (!args[0].EndsWith(".jpg") && !args[0].EndsWith(".tif") && !args[0].EndsWith(".gif"))
				StandardUsage();
			// First check to see if there is an OutputFile argument.  If there isn't
			// then name the file "InputFile".jpg
			// Second check to see if the .jpg extension is on the OutputFile argument.
			// If there isn't one, add it.
			// Need to check for the existence of the output file.  If it exists already,
			// rename the file with a # after the file name, then the .jpg extension.
			if (args.Length < 3)
			{
				string_Renamed = args[0].Substring(0, (args[0].LastIndexOf(".")) - (0)) + ".jpg";
			}
			else
			{
				string_Renamed = args[2];
				if (string_Renamed.EndsWith(".tif") || string_Renamed.EndsWith(".gif"))
					string_Renamed = string_Renamed.Substring(0, (string_Renamed.LastIndexOf(".")) - (0));
				if (!string_Renamed.EndsWith(".jpg"))
					string_Renamed = System.String.Concat(string_Renamed, ".jpg");
			}
			outFile = new System.IO.FileInfo(string_Renamed);
			i = 1;
			bool tmpBool;
			if (System.IO.File.Exists(outFile.FullName))
				tmpBool = true;
			else
				tmpBool = System.IO.Directory.Exists(outFile.FullName);
			while (tmpBool)
			{
				outFile = new System.IO.FileInfo(string_Renamed.Substring(0, (string_Renamed.LastIndexOf(".")) - (0)) + (i++) + ".jpg");
				if (i > 100)
					System.Environment.Exit(0);
				if (System.IO.File.Exists(outFile.FullName))
					tmpBool = true;
				else
					tmpBool = System.IO.Directory.Exists(outFile.FullName);
			}
			file = new System.IO.FileInfo(args[0]);
			bool tmpBool2;
			if (System.IO.File.Exists(file.FullName))
				tmpBool2 = true;
			else
				tmpBool2 = System.IO.Directory.Exists(file.FullName);
			if (tmpBool2)
			{
				try
				{
					//UPGRADE_TODO: Constructor 'java.io.FileOutputStream.FileOutputStream' was converted to 'System.IO.FileStream.FileStream' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileOutputStreamFileOutputStream_javaioFile'"
					dataOut = new System.IO.FileStream(outFile.FullName, System.IO.FileMode.Create);
				}
				catch (System.IO.IOException e)
				{
				}
				try
				{
					Quality = System.Int32.Parse(args[1]);
				}
				catch (System.FormatException e)
				{
					StandardUsage();
				}
				//UPGRADE_ISSUE: Method 'java.awt.Toolkit.getDefaultToolkit' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtToolkit'"
				Toolkit.getDefaultToolkit();
				image = System.Drawing.Image.FromFile(args[0]);
				jpg = new JpegEncoder(image, Quality, dataOut);
				jpg.Compress();
				try
				{
					dataOut.Close();
				}
				catch (System.IO.IOException e)
				{
				}
			}
			else
			{
				System.Console.Out.WriteLine("I couldn't find " + args[0] + ". Is it in another directory?");
			}
			System.Environment.Exit(0);
		}
	}
}