/* $RCSfile$
* $Author: egonw $
* $Date: 2005-11-10 16:52:44 +0100 (jeu., 10 nov. 2005) $
* $Revision: 4255 $
*
* Copyright (C) 2003-2005  Miguel, Jmol Development Team
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
using JmolAdapter = org.jmol.api.JmolAdapter;
namespace org.jmol.viewer
{
	/// <summary>*************************************************************
	/// will not work with applet
	/// import java.net.URI;
	/// import java.net.URISyntaxException;
	/// import java.util.Enumeration;
	/// import org.openscience.jmol.io.ChemFileReader;
	/// import org.openscience.jmol.io.ReaderFactory;
	/// </summary>
	
	class FileManager
	{
		virtual internal System.String FullPathName
		{
			get
			{
				return fullPathName != null?fullPathName:nameAsGiven;
			}
			
		}
		virtual internal System.String FileName
		{
			get
			{
				return fileName != null?fileName:nameAsGiven;
			}
			
		}
		
		internal Viewer viewer;
		internal JmolAdapter modelAdapter;
		private System.String openErrorMessage;
		
		// for applet proxy
		internal System.Uri appletDocumentBase = null;
		internal System.Uri appletCodeBase = null;
		internal System.String appletProxy = null;
		
		// for expanding names into full path names
		//private boolean isURL;
		private System.String nameAsGiven;
		private System.String fullPathName;
		internal System.String fileName;
		private System.IO.FileInfo file;
		
		private FileOpenThread fileOpenThread;
		private FilesOpenThread filesOpenThread;
		private DOMOpenThread aDOMOpenThread;
		
		
		internal FileManager(Viewer viewer, JmolAdapter modelAdapter)
		{
			this.viewer = viewer;
			this.modelAdapter = modelAdapter;
		}
		
		internal virtual void  openFile(System.String name)
		{
			System.Console.Out.WriteLine("FileManager.openFile(" + name + ")");
			nameAsGiven = name;
			openErrorMessage = fullPathName = fileName = null;
			classifyName(name);
			if (openErrorMessage != null)
			{
				System.Console.Out.WriteLine("openErrorMessage=" + openErrorMessage);
				return ;
			}
			fileOpenThread = new FileOpenThread(this, fullPathName, name);
			fileOpenThread.Run();
		}
		
		internal virtual void  openFiles(System.String modelName, System.String[] names)
		{
			System.String[] fullPathNames = new System.String[names.Length];
			for (int i = 0; i < names.Length; i++)
			{
				nameAsGiven = names[i];
				openErrorMessage = fullPathName = fileName = null;
				classifyName(names[i]);
				if (openErrorMessage != null)
				{
					System.Console.Out.WriteLine("openErrorMessage=" + openErrorMessage);
					return ;
				}
				fullPathNames[i] = fullPathName;
			}
			fullPathName = fileName = nameAsGiven = modelName;
			filesOpenThread = new FilesOpenThread(this, fullPathNames, names);
			filesOpenThread.Run();
		}
		
		internal virtual void  openStringInline(System.String strModel)
		{
			openErrorMessage = null;
			fullPathName = fileName = "string";
			fileOpenThread = new FileOpenThread(this, fullPathName, new System.IO.StringReader(strModel));
			fileOpenThread.Run();
		}
		
		internal virtual void  openDOM(System.Object DOMNode)
		{
			openErrorMessage = null;
			fullPathName = fileName = "JSNode";
			aDOMOpenThread = new DOMOpenThread(this, DOMNode);
			aDOMOpenThread.Run();
		}
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		internal virtual void  openReader(System.String fullPathName, System.String name, System.IO.StreamReader reader)
		{
			openErrorMessage = null;
			this.fullPathName = fullPathName;
			fileName = name;
			fileOpenThread = new FileOpenThread(this, fullPathName, reader);
			fileOpenThread.Run();
		}
		
		internal virtual System.String getFileAsString(System.String name)
		{
			System.Console.Out.WriteLine("FileManager.getFileAsString(" + name + ")");
			System.Object t = getInputStreamOrErrorMessageFromName(name);
			sbyte[] abMagic = new sbyte[4];
			if (t is System.String)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				return "Error:" + t;
			}
			try
			{
				System.IO.BufferedStream bis = new System.IO.BufferedStream((System.IO.Stream) t, 8192);
				System.IO.Stream is_Renamed = bis;
				SupportClass.BufferedStreamManager.manager.MarkPosition(5, bis);
				int countRead = 0;
				countRead = SupportClass.ReadInput(bis, abMagic, 0, 4);
				bis.Position = SupportClass.BufferedStreamManager.manager.ResetMark(bis);
				if (countRead == 4 && abMagic[0] == (sbyte) 0x1F && abMagic[1] == (sbyte) SupportClass.Identity(0x8B))
				{
					//UPGRADE_ISSUE: Constructor 'java.util.zip.GZIPInputStream.GZIPInputStream' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilzipGZIPInputStream'"
					is_Renamed = new GZIPInputStream(bis);
				}
				//UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
				//UPGRADE_WARNING: At least one expression was used more than once in the target code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1181'"
				System.IO.StreamReader br = new System.IO.StreamReader(new System.IO.StreamReader(is_Renamed, System.Text.Encoding.Default).BaseStream, new System.IO.StreamReader(is_Renamed, System.Text.Encoding.Default).CurrentEncoding);
				System.Text.StringBuilder sb = new System.Text.StringBuilder(8192);
				System.String line;
				while ((line = br.ReadLine()) != null)
				{
					sb.Append(line);
					sb.Append('\n');
				}
				return "" + sb;
			}
			catch (System.IO.IOException ioe)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				return ioe.Message;
			}
		}
		
		internal virtual System.Object waitForClientFileOrErrorMessage()
		{
			System.Object clientFile = null;
			if (fileOpenThread != null)
			{
				clientFile = fileOpenThread.clientFile;
				if (fileOpenThread.errorMessage != null)
					openErrorMessage = fileOpenThread.errorMessage;
				else if (clientFile == null)
					openErrorMessage = "Client file is null loading:" + nameAsGiven;
				fileOpenThread = null;
			}
			else if (filesOpenThread != null)
			{
				clientFile = filesOpenThread.clientFile;
				if (filesOpenThread.errorMessage != null)
					openErrorMessage = filesOpenThread.errorMessage;
				else if (clientFile == null)
					openErrorMessage = "Client file is null loading:" + nameAsGiven;
			}
			else if (aDOMOpenThread != null)
			{
				clientFile = aDOMOpenThread.clientFile;
				if (aDOMOpenThread.errorMessage != null)
					openErrorMessage = aDOMOpenThread.errorMessage;
				else if (clientFile == null)
					openErrorMessage = "Client file is null loading:" + nameAsGiven;
				aDOMOpenThread = null;
			}
			if (openErrorMessage != null)
				return openErrorMessage;
			return clientFile;
		}
		
		internal virtual void  setAppletContext(System.Uri documentBase, System.Uri codeBase, System.String jmolAppletProxy)
		{
			appletDocumentBase = documentBase;
			System.Console.Out.WriteLine("appletDocumentBase=" + documentBase);
			//    dumpDocumentBase("" + documentBase);
			appletCodeBase = codeBase;
			appletProxy = jmolAppletProxy;
		}
		
		internal virtual void  dumpDocumentBase(System.String documentBase)
		{
			System.Console.Out.WriteLine("dumpDocumentBase:" + documentBase);
			System.Object inputStreamOrError = getInputStreamOrErrorMessageFromName(documentBase);
			if (inputStreamOrError == null)
			{
				System.Console.Out.WriteLine("?Que? ?null?");
			}
			else if (inputStreamOrError is System.String)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				System.Console.Out.WriteLine("Error:" + inputStreamOrError);
			}
			else
			{
				//UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
				//UPGRADE_WARNING: At least one expression was used more than once in the target code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1181'"
				System.IO.StreamReader br = new System.IO.StreamReader(new System.IO.StreamReader((System.IO.Stream) inputStreamOrError, System.Text.Encoding.Default).BaseStream, new System.IO.StreamReader((System.IO.Stream) inputStreamOrError, System.Text.Encoding.Default).CurrentEncoding);
				System.String line;
				try
				{
					while ((line = br.ReadLine()) != null)
						System.Console.Out.WriteLine(line);
					br.Close();
				}
				catch (System.Exception ex)
				{
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					System.Console.Out.WriteLine("exception caught:" + ex);
				}
			}
		}
		
		// mth jan 2003 -- there must be a better way for me to do this!?
		//UPGRADE_NOTE: Final was removed from the declaration of 'urlPrefixes '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal System.String[] urlPrefixes = new System.String[]{"http:", "https:", "ftp:", "file:"};
		
		private void  classifyName(System.String name)
		{
			//isURL = false;
			if (name == null)
				return ;
			if (appletDocumentBase != null)
			{
				// This code is only for the applet
				//isURL = true;
				try
				{
					//UPGRADE_TODO: Class 'java.net.URL' was converted to a 'System.Uri' which does not throw an exception if a URL specifies an unknown protocol. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1132'"
					System.Uri url = new System.Uri(appletDocumentBase, name);
					fullPathName = url.ToString();
					// we add one to lastIndexOf(), so don't worry about -1 return value
					fileName = fullPathName.Substring(fullPathName.LastIndexOf('/') + 1, (fullPathName.Length) - (fullPathName.LastIndexOf('/') + 1));
				}
				catch (System.UriFormatException e)
				{
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					openErrorMessage = e.Message;
				}
				return ;
			}
			// This code is for the app
			for (int i = 0; i < urlPrefixes.Length; ++i)
			{
				if (name.StartsWith(urlPrefixes[i]))
				{
					//isURL = true;
					try
					{
						//UPGRADE_TODO: Class 'java.net.URL' was converted to a 'System.Uri' which does not throw an exception if a URL specifies an unknown protocol. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1132'"
						System.Uri url = new System.Uri(name);
						fullPathName = url.ToString();
						fileName = fullPathName.Substring(fullPathName.LastIndexOf('/') + 1, (fullPathName.Length) - (fullPathName.LastIndexOf('/') + 1));
					}
					catch (System.UriFormatException e)
					{
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						openErrorMessage = e.Message;
					}
					return ;
				}
			}
			//isURL = false;
			file = new System.IO.FileInfo(name);
			fullPathName = file.FullName;
			fileName = file.Name;
		}
		
		internal virtual System.Object getInputStreamOrErrorMessageFromName(System.String name)
		{
			System.String errorMessage = null;
			int iurlPrefix;
			for (iurlPrefix = urlPrefixes.Length; --iurlPrefix >= 0; )
				if (name.StartsWith(urlPrefixes[iurlPrefix]))
					break;
			try
			{
				System.IO.Stream in_Renamed;
				int length;
				if (appletDocumentBase == null)
				{
					if (iurlPrefix >= 0)
					{
						//UPGRADE_TODO: Class 'java.net.URL' was converted to a 'System.Uri' which does not throw an exception if a URL specifies an unknown protocol. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1132'"
						System.Uri url = new System.Uri(name);
						System.Net.HttpWebRequest conn = (System.Net.HttpWebRequest) System.Net.WebRequest.Create(url);
						int ContentLength;
						try
						{
							ContentLength = System.Int32.Parse(conn.GetResponse().Headers.Get("Content-Length"));
						}
						catch (System.IO.IOException e)
						{
							ContentLength = -1;
						}
						length = ContentLength;
						in_Renamed = conn.GetResponse().GetResponseStream();
					}
					else
					{
						System.IO.FileInfo file = new System.IO.FileInfo(name);
						length = (int) SupportClass.FileLength(file);
						//UPGRADE_TODO: Constructor 'java.io.FileInputStream.FileInputStream' was converted to 'System.IO.FileStream.FileStream' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileInputStreamFileInputStream_javaioFile'"
						in_Renamed = new System.IO.FileStream(file.FullName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
					}
				}
				else
				{
					if (iurlPrefix >= 0 && appletProxy != null)
						name = appletProxy + "?url=" + URLEncoder.encode(name, "utf-8");
					//UPGRADE_TODO: Class 'java.net.URL' was converted to a 'System.Uri' which does not throw an exception if a URL specifies an unknown protocol. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1132'"
					System.Uri url = new System.Uri(appletDocumentBase, name);
					System.Net.HttpWebRequest conn = (System.Net.HttpWebRequest) System.Net.WebRequest.Create(url);
					int ContentLength2;
					try
					{
						ContentLength2 = System.Int32.Parse(conn.GetResponse().Headers.Get("Content-Length"));
					}
					catch (System.IO.IOException e)
					{
						ContentLength2 = -1;
					}
					length = ContentLength2;
					in_Renamed = conn.GetResponse().GetResponseStream();
				}
				return new MonitorInputStream(in_Renamed, length);
			}
			catch (System.Exception e)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				errorMessage = "" + e;
			}
			return errorMessage;
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'abMagic '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal sbyte[] abMagic = new sbyte[4];
		
		internal virtual System.Object getUnzippedBufferedReaderOrErrorMessageFromName(System.String name)
		{
			System.Object t = getInputStreamOrErrorMessageFromName(name);
			if (t is System.String)
				return t;
			try
			{
				System.IO.BufferedStream bis = new System.IO.BufferedStream((System.IO.Stream) t, 8192);
				System.IO.Stream is_Renamed = bis;
				SupportClass.BufferedStreamManager.manager.MarkPosition(5, bis);
				int countRead = 0;
				countRead = SupportClass.ReadInput(bis, abMagic, 0, 4);
				bis.Position = SupportClass.BufferedStreamManager.manager.ResetMark(bis);
				if (countRead == 4 && abMagic[0] == (sbyte) 0x1F && abMagic[1] == (sbyte) SupportClass.Identity(0x8B))
				{
					//UPGRADE_ISSUE: Constructor 'java.util.zip.GZIPInputStream.GZIPInputStream' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilzipGZIPInputStream'"
					is_Renamed = new GZIPInputStream(bis);
				}
				//UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
				//UPGRADE_WARNING: At least one expression was used more than once in the target code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1181'"
				return new System.IO.StreamReader(new System.IO.StreamReader(is_Renamed, System.Text.Encoding.Default).BaseStream, new System.IO.StreamReader(is_Renamed, System.Text.Encoding.Default).CurrentEncoding);
			}
			catch (System.IO.IOException ioe)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				return ioe.Message;
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'DOMOpenThread' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		internal class DOMOpenThread : IThreadRunnable
		{
			private void  InitBlock(FileManager enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private FileManager enclosingInstance;
			public FileManager Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			internal bool terminated;
			internal System.String errorMessage;
			internal System.Object aDOMNode;
			internal System.Object clientFile;
			
			internal DOMOpenThread(FileManager enclosingInstance, System.Object DOMNode)
			{
				InitBlock(enclosingInstance);
				this.aDOMNode = DOMNode;
			}
			
			public virtual void  Run()
			{
				clientFile = Enclosing_Instance.modelAdapter.openDOMReader(aDOMNode);
				errorMessage = null;
				terminated = true;
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'FileOpenThread' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		internal class FileOpenThread : IThreadRunnable
		{
			private void  InitBlock(FileManager enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private FileManager enclosingInstance;
			public FileManager Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			internal bool terminated;
			internal System.String errorMessage;
			internal System.String fullPathNameInThread;
			internal System.String nameAsGivenInThread;
			internal System.Object clientFile;
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			internal System.IO.StreamReader reader;
			
			internal FileOpenThread(FileManager enclosingInstance, System.String fullPathName, System.String nameAsGiven)
			{
				InitBlock(enclosingInstance);
				this.fullPathNameInThread = fullPathName;
				this.nameAsGivenInThread = nameAsGiven;
			}
			
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			internal FileOpenThread(FileManager enclosingInstance, System.String name, System.IO.StreamReader reader)
			{
				InitBlock(enclosingInstance);
				nameAsGivenInThread = fullPathNameInThread = name;
				this.reader = reader;
			}
			
			public virtual void  Run()
			{
				if (reader != null)
				{
					openReader(reader);
				}
				else
				{
					System.Object t = Enclosing_Instance.getInputStreamOrErrorMessageFromName(nameAsGivenInThread);
					if (!(t is System.IO.Stream))
					{
						errorMessage = (t == null?"error opening:" + nameAsGivenInThread:(System.String) t);
					}
					else
					{
						openInputStream(fullPathNameInThread, Enclosing_Instance.fileName, (System.IO.Stream) t);
					}
				}
				if (errorMessage != null)
					System.Console.Out.WriteLine("error opening " + fullPathNameInThread + "\n" + errorMessage);
				terminated = true;
			}
			
			internal sbyte[] abMagicF = new sbyte[4];
			private void  openInputStream(System.String fullPathName, System.String fileName, System.IO.Stream istream)
			{
				System.IO.BufferedStream bistream = new System.IO.BufferedStream(istream, 8192);
				System.IO.Stream istreamToRead = bistream;
				SupportClass.BufferedStreamManager.manager.MarkPosition(5, bistream);
				int countRead = 0;
				try
				{
					countRead = SupportClass.ReadInput(bistream, abMagicF, 0, 4);
					bistream.Position = SupportClass.BufferedStreamManager.manager.ResetMark(bistream);
					if (countRead == 4)
					{
						if (abMagicF[0] == (sbyte) 0x1F && abMagicF[1] == (sbyte) SupportClass.Identity(0x8B))
						{
							//UPGRADE_ISSUE: Constructor 'java.util.zip.GZIPInputStream.GZIPInputStream' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilzipGZIPInputStream'"
							istreamToRead = new GZIPInputStream(bistream);
						}
					}
					openReader(new System.IO.StreamReader(istreamToRead, System.Text.Encoding.Default));
				}
				catch (System.IO.IOException ioe)
				{
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					errorMessage = ioe.Message;
				}
			}
			
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			private void  openReader(System.IO.StreamReader reader)
			{
				//UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
				System.Object clientFile = Enclosing_Instance.modelAdapter.openBufferedReader(fullPathNameInThread, new System.IO.StreamReader(reader.BaseStream, reader.CurrentEncoding));
				if (clientFile is System.String)
					errorMessage = ((System.String) clientFile);
				else
					this.clientFile = clientFile;
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'FilesOpenThread' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		internal class FilesOpenThread : IThreadRunnable
		{
			private void  InitBlock(FileManager enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private FileManager enclosingInstance;
			public FileManager Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			internal bool terminated;
			internal System.String errorMessage;
			internal System.String[] fullPathNameInThread;
			internal System.String[] nameAsGivenInThread;
			internal System.Object clientFile;
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			internal System.IO.StreamReader[] reader;
			
			internal FilesOpenThread(FileManager enclosingInstance, System.String[] fullPathName, System.String[] nameAsGiven)
			{
				InitBlock(enclosingInstance);
				this.fullPathNameInThread = fullPathName;
				this.nameAsGivenInThread = nameAsGiven;
			}
			
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			internal FilesOpenThread(FileManager enclosingInstance, System.String[] name, System.IO.StreamReader[] reader)
			{
				InitBlock(enclosingInstance);
				nameAsGivenInThread = fullPathNameInThread = name;
				this.reader = reader;
			}
			
			public virtual void  Run()
			{
				if (reader != null)
				{
					openReader(reader);
				}
				else
				{
					System.IO.Stream[] istream = new System.IO.Stream[nameAsGivenInThread.Length];
					for (int i = 0; i < nameAsGivenInThread.Length; i++)
					{
						System.Object t = Enclosing_Instance.getInputStreamOrErrorMessageFromName(nameAsGivenInThread[i]);
						if (!(t is System.IO.Stream))
						{
							//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
							errorMessage = (t == null?"error opening:" + nameAsGivenInThread:(System.String) t);
							terminated = true;
							return ;
						}
						istream[i] = (System.IO.Stream) t;
					}
					openInputStream(fullPathNameInThread, istream);
				}
				if (errorMessage != null)
				{
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					System.Console.Out.WriteLine("error opening " + fullPathNameInThread + "\n" + errorMessage);
				}
				terminated = true;
			}
			
			internal sbyte[] abMagicF = new sbyte[4];
			private void  openInputStream(System.String[] fullPathName, System.IO.Stream[] istream)
			{
				System.IO.StreamReader[] zistream = new System.IO.StreamReader[istream.Length];
				for (int i = 0; i < istream.Length; i++)
				{
					System.IO.BufferedStream bistream = new System.IO.BufferedStream(istream[i], 8192);
					System.IO.Stream istreamToRead = bistream;
					SupportClass.BufferedStreamManager.manager.MarkPosition(5, bistream);
					int countRead = 0;
					try
					{
						countRead = SupportClass.ReadInput(bistream, abMagicF, 0, 4);
						bistream.Position = SupportClass.BufferedStreamManager.manager.ResetMark(bistream);
						if (countRead == 4)
						{
							if (abMagicF[0] == (sbyte) 0x1F && abMagicF[1] == (sbyte) SupportClass.Identity(0x8B))
							{
								//UPGRADE_ISSUE: Constructor 'java.util.zip.GZIPInputStream.GZIPInputStream' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilzipGZIPInputStream'"
								istreamToRead = new GZIPInputStream(bistream);
							}
						}
						zistream[i] = new System.IO.StreamReader(istreamToRead, System.Text.Encoding.Default);
					}
					catch (System.IO.IOException ioe)
					{
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						errorMessage = ioe.Message;
						return ;
					}
				}
				openReader(zistream);
			}
			
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			private void  openReader(System.IO.StreamReader[] reader)
			{
				System.IO.StreamReader[] buffered = new System.IO.StreamReader[reader.Length];
				for (int i = 0; i < reader.Length; i++)
				{
					//UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
					buffered[i] = new System.IO.StreamReader(reader[i].BaseStream, reader[i].CurrentEncoding);
				}
				System.Object clientFile = Enclosing_Instance.modelAdapter.openBufferedReaders(fullPathNameInThread, buffered);
				if (clientFile is System.String)
					errorMessage = ((System.String) clientFile);
				else
					this.clientFile = clientFile;
			}
		}
	}
	
	class MonitorInputStream:System.IO.BinaryReader
	{
		virtual internal int Position
		{
			get
			{
				return position;
			}
			
		}
		virtual internal int Length
		{
			get
			{
				return length;
			}
			
		}
		virtual internal int PercentageRead
		{
			get
			{
				return position * 100 / length;
			}
			
		}
		virtual internal int ReadingTimeMillis
		{
			get
			{
				return (int) ((System.DateTime.Now.Ticks - 621355968000000000) / 10000 - timeBegin);
			}
			
		}
		internal int length;
		internal int position;
		internal int markPosition;
		internal int readEventCount;
		internal long timeBegin;
		
		internal MonitorInputStream(System.IO.Stream in_Renamed, int length):base(in_Renamed)
		{
			this.length = length;
			this.position = 0;
			timeBegin = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
		}
		
		public  override int Read()
		{
			++readEventCount;
			int nextByte = base.Read();
			if (nextByte >= 0)
				++position;
			return nextByte;
		}
		
		//UPGRADE_NOTE: The equivalent of method 'java.io.FilterInputStream.read' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		public int read(sbyte[] b)
		{
			++readEventCount;
			int cb = SupportClass.ReadInput(base.BaseStream, b, 0, b.Length);
			if (cb > 0)
				position += cb;
			return cb;
		}
		
		//UPGRADE_NOTE: The equivalent of method 'java.io.FilterInputStream.read' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		public int read(sbyte[] b, int off, int len)
		{
			++readEventCount;
			int cb = SupportClass.ReadInput(base.BaseStream, b, off, len);
			if (cb > 0)
				position += cb;
			/*
			System.out.println("" + getPercentageRead() + "% " +
			getPosition() + " of " + getLength() + " in " +
			getReadingTimeMillis());
			*/
			return cb;
		}
		
		//UPGRADE_NOTE: The equivalent of method 'java.io.FilterInputStream.skip' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		public long skip(long n)
		{
			System.IO.BinaryReader temp_BinaryReader;
			System.Int64 temp_Int64;
			temp_BinaryReader = (System.IO.BinaryReader) this;
			temp_Int64 = temp_BinaryReader.BaseStream.Position;
			temp_Int64 = temp_BinaryReader.BaseStream.Seek(n, System.IO.SeekOrigin.Current) - temp_Int64;
			long cb = temp_Int64;
			// this will only work in relatively small files ... 2Gb
			position = (int) (position + cb);
			return cb;
		}
		
		//UPGRADE_NOTE: The equivalent of method 'java.io.FilterInputStream.mark' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		public void  mark(int readlimit)
		{
			//UPGRADE_ISSUE: Method 'java.io.FilterInputStream.mark' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaioFilterInputStreammark_int'"
			base.mark(readlimit);
			markPosition = position;
		}
		
		//UPGRADE_NOTE: The equivalent of method 'java.io.FilterInputStream.reset' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		public void  reset()
		{
			position = markPosition;
			//UPGRADE_ISSUE: Method 'java.io.FilterInputStream.reset' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaioFilterInputStreamreset'"
			base.reset();
		}
	}
}