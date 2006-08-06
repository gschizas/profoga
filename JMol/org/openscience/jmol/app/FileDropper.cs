/* $RCSfile$
* $Author: egonw $
* $Date: 2005-11-10 16:52:44 +0100 (jeu., 10 nov. 2005) $
* $Revision: 4255 $
*
* Copyright (C) 2004-2005  The Jmol Development Team
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
namespace org.openscience.jmol.app
{
	
	/// <summary> A simple Dropping class to allow files to be dragged onto a target.
	/// It supports drag-and-drop of files from file browsers, and CML text
	/// from editors, e.g. jEdit.
	/// 
	/// <p>Note that multiple drops are not thread safe.
	/// 
	/// </summary>
	/// <author>  Billy <simon.tyrrell@virgin.net>
	/// </author>
	public class FileDropper
	{
		[System.ComponentModel.Browsable(true)]
		public  event SupportClass.PropertyChangeEventHandler PropertyChange;
		private System.String fd_oldFileName;
		//UPGRADE_ISSUE: Class 'java.beans.PropertyChangeSupport' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javabeansPropertyChangeSupport'"
		private PropertyChangeSupport fd_propSupport;
		
		public const System.String FD_PROPERTY_FILENAME = "filename";
		public const System.String FD_PROPERTY_INLINE = "inline";
		
		public FileDropper()
		{
			fd_oldFileName = "";
			//UPGRADE_ISSUE: Constructor 'java.beans.PropertyChangeSupport.PropertyChangeSupport' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javabeansPropertyChangeSupport'"
			fd_propSupport = new PropertyChangeSupport(this);
		}
		
		//UPGRADE_TODO: Interface 'java.beans.PropertyChangeListener' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'addPropertyChangeListener'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public virtual void  addPropertyChangeListener(PropertyChangeListener l)
		{
			lock (this)
			{
				//UPGRADE_ISSUE: Method 'java.beans.PropertyChangeSupport.addPropertyChangeListener' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javabeansPropertyChangeSupport'"
				fd_propSupport.addPropertyChangeListener(l);
			}
		}
		
		//UPGRADE_TODO: Interface 'java.beans.PropertyChangeListener' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'removePropertyChangeListener'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public virtual void  removePropertyChangeListener(PropertyChangeListener l)
		{
			lock (this)
			{
				//UPGRADE_ISSUE: Method 'java.beans.PropertyChangeSupport.removePropertyChangeListener' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javabeansPropertyChangeSupport'"
				fd_propSupport.removePropertyChangeListener(l);
			}
		}
		
		public virtual void  dragOver_renamed(System.Object event_sender, System.Windows.Forms.DragEventArgs dtde)
		{
			System.Console.Out.WriteLine("DropOver detected...");
		}
		
		public virtual void  dragEnter_renamed(System.Object event_sender, System.Windows.Forms.DragEventArgs dtde)
		{
			System.Console.Out.WriteLine("DropEnter detected...");
			dtde.Effect = (System.Windows.Forms.DragDropEffects) (System.Windows.Forms.DragDropEffects.Copy | System.Windows.Forms.DragDropEffects.Move);
		}
		
		//UPGRADE_ISSUE: Class 'java.awt.dnd.DropTargetEvent' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtdndDropTargetEvent'"
		public virtual void  dragExit_renamed(System.Object event_sender, System.EventArgs dtde)
		{
			System.Console.Out.WriteLine("DropExit detected...");
		}
		
		public virtual void  dropActionChanged(System.Object event_sender, System.Windows.Forms.DragEventArgs dtde)
		{
		}
		
		public virtual void  drop_renamed(System.Object event_sender, System.Windows.Forms.DragEventArgs dtde)
		{
			System.Console.Out.WriteLine("Drop detected...");
			System.Windows.Forms.IDataObject t = dtde.Data;
			//UPGRADE_TODO: Field 'java.awt.datatransfer.DataFlavor.javaFileListFlavor' was converted to 'System.Windows.Forms.DataFormats.FileDrop' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtdatatransferDataFlavorjavaFileListFlavor_f'"
			if (SupportClass.IsDataFormatSupported(t, System.Windows.Forms.DataFormats.GetFormat(System.Windows.Forms.DataFormats.FileDrop)))
			{
				dtde.Effect = (System.Windows.Forms.DragDropEffects) (System.Windows.Forms.DragDropEffects.Copy | System.Windows.Forms.DragDropEffects.Move);
				System.Object o = null;
				
				try
				{
					//UPGRADE_TODO: Field 'java.awt.datatransfer.DataFlavor.javaFileListFlavor' was converted to 'System.Windows.Forms.DataFormats.FileDrop' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtdatatransferDataFlavorjavaFileListFlavor_f'"
					o = t.GetData(System.Windows.Forms.DataFormats.GetFormat(System.Windows.Forms.DataFormats.FileDrop).Name);
				}
				catch (System.Exception ufe)
				{
					SupportClass.WriteStackTrace(ufe, Console.Error);
				}
				catch (System.IO.IOException ioe)
				{
					SupportClass.WriteStackTrace(ioe, Console.Error);
				}
				
				// if o is still null we had an exception
				if ((o != null) && (o is System.Collections.IList))
				{
					System.Collections.IList fileList = (System.Collections.IList) o;
					//UPGRADE_NOTE: Final was removed from the declaration of 'length '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
					int length = fileList.Count;
					
					for (int i = 0; i < length; ++i)
					{
						System.IO.FileInfo f = (System.IO.FileInfo) fileList[i];
						//UPGRADE_ISSUE: Constructor 'java.beans.PropertyChangeEvent.PropertyChangeEvent' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javabeansPropertyChangeEventPropertyChangeEvent_javalangObject_javalangString_javalangObject_javalangObject'"
						SupportClass.PropertyChangingEventArgs pce = new PropertyChangeEvent(this, FD_PROPERTY_FILENAME, fd_oldFileName, f.FullName);
						//UPGRADE_ISSUE: Method 'java.beans.PropertyChangeSupport.firePropertyChange' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javabeansPropertyChangeSupport'"
						fd_propSupport.firePropertyChange(pce);
					}
					
					//UPGRADE_ISSUE: Method 'java.awt.dnd.DropTargetContext.dropComplete' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtdndDropTargetContext'"
					//UPGRADE_ISSUE: Method 'java.awt.dnd.DropTargetEvent.getDropTargetContext' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtdndDropTargetEvent'"
					dtde.getDropTargetContext().dropComplete(true);
				}
			}
			else
			{
				System.Console.Out.WriteLine("browsing supported flavours to find something useful...");
				//UPGRADE_TODO: Method 'java.awt.datatransfer.Transferable.getTransferDataFlavors' was converted to 'System.Windows.Forms.IDataObject.GetFormats' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtdatatransferTransferablegetTransferDataFlavors'"
				System.Windows.Forms.DataFormats.Format[] df = t.GetFormats();
				
				if ((df != null) && (df.Length > 0))
				{
					for (int i = 0; i < df.Length; ++i)
					{
						
						System.Windows.Forms.DataFormats.Format flavor = df[i];
						System.Console.Out.WriteLine("df " + i + " flavor " + flavor);
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						//UPGRADE_ISSUE: Method 'java.awt.datatransfer.DataFlavor.getRepresentationClass' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtdatatransferDataFlavorgetRepresentationClass'"
						System.Console.Out.WriteLine("  class: " + flavor.getRepresentationClass().FullName);
						//UPGRADE_TODO: Method 'java.awt.datatransfer.DataFlavor.getMimeType' was converted to 'System.Windows.Forms.DataFormats.Format.Name' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtdatatransferDataFlavorgetMimeType'"
						System.Console.Out.WriteLine("  mime : " + flavor.Name);
						
						//UPGRADE_TODO: Method 'java.awt.datatransfer.DataFlavor.getMimeType' was converted to 'System.Windows.Forms.DataFormats.Format.Name' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtdatatransferDataFlavorgetMimeType'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						//UPGRADE_ISSUE: Method 'java.awt.datatransfer.DataFlavor.getRepresentationClass' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtdatatransferDataFlavorgetRepresentationClass'"
						if (flavor.Name.StartsWith("text/uri-list") && flavor.getRepresentationClass().FullName.Equals("java.lang.String"))
						{
							
							/* This is one of the (many) flavors that KDE provides:
							
							df 2 flavour java.awt.datatransfer.DataFlavor[mimetype=text/uri-list;representationclass=java.lang.String]
							java.lang.String
							String: file:/home/egonw/data/Projects/SourceForge/Jmol/Jmol-HEAD/samples/cml/methanol2.cml
							
							A later KDE version gave me the following. Note the mime!! hence the startsWith above
							
							df 3 flavor java.awt.datatransfer.DataFlavor[mimetype=text/uri-list;representationclass=java.lang.String]
							class: java.lang.String
							mime : text/uri-list; class=java.lang.String; charset=Unicode
							*/
							
							dtde.Effect = (System.Windows.Forms.DragDropEffects) (System.Windows.Forms.DragDropEffects.Copy | System.Windows.Forms.DragDropEffects.Move);
							System.Object o = null;
							
							try
							{
								o = t.GetData(flavor.Name);
							}
							catch (System.Exception ufe)
							{
								SupportClass.WriteStackTrace(ufe, Console.Error);
							}
							catch (System.IO.IOException ioe)
							{
								SupportClass.WriteStackTrace(ioe, Console.Error);
							}
							
							if ((o != null) && (o is System.String))
							{
								//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
								System.Console.Out.WriteLine("  String: " + o.ToString());
								
								//UPGRADE_ISSUE: Constructor 'java.beans.PropertyChangeEvent.PropertyChangeEvent' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javabeansPropertyChangeEventPropertyChangeEvent_javalangObject_javalangString_javalangObject_javalangObject'"
								//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
								SupportClass.PropertyChangingEventArgs pce = new PropertyChangeEvent(this, FD_PROPERTY_FILENAME, fd_oldFileName, o.ToString());
								//UPGRADE_ISSUE: Method 'java.beans.PropertyChangeSupport.firePropertyChange' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javabeansPropertyChangeSupport'"
								fd_propSupport.firePropertyChange(pce);
								//UPGRADE_ISSUE: Method 'java.awt.dnd.DropTargetContext.dropComplete' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtdndDropTargetContext'"
								//UPGRADE_ISSUE: Method 'java.awt.dnd.DropTargetEvent.getDropTargetContext' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtdndDropTargetEvent'"
								dtde.getDropTargetContext().dropComplete(true);
							}
							return ;
						}
						else
						{
							//UPGRADE_TODO: Method 'java.awt.datatransfer.DataFlavor.getMimeType' was converted to 'System.Windows.Forms.DataFormats.Format.Name' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtdatatransferDataFlavorgetMimeType'"
							if (flavor.Name.Equals("application/x-java-serialized-object; class=java.lang.String"))
							{
								
								/* This is one of the flavors that jEdit provides:
								
								df 0 flavor java.awt.datatransfer.DataFlavor[mimetype=application/x-java-serialized-object;representationclass=java.lang.String]
								class: java.lang.String
								mime : application/x-java-serialized-object; class=java.lang.String
								String: <molecule title="benzene.mol" xmlns="http://www.xml-cml.org/schema/cml2/core"
								
								But KDE also provides:
								
								df 24 flavor java.awt.datatransfer.DataFlavor[mimetype=application/x-java-serialized-object;representationclass=java.lang.String]
								class: java.lang.String
								mime : application/x-java-serialized-object; class=java.lang.String
								String: file:/home/egonw/Desktop/1PN8.pdb
								*/
								
								dtde.Effect = (System.Windows.Forms.DragDropEffects) (System.Windows.Forms.DragDropEffects.Copy | System.Windows.Forms.DragDropEffects.Move);
								System.Object o = null;
								
								try
								{
									o = t.GetData(df[i].Name);
								}
								catch (System.Exception ufe)
								{
									SupportClass.WriteStackTrace(ufe, Console.Error);
								}
								catch (System.IO.IOException ioe)
								{
									SupportClass.WriteStackTrace(ioe, Console.Error);
								}
								
								if ((o != null) && (o is System.String))
								{
									System.String content = (System.String) o;
									System.Console.Out.WriteLine("  String: " + content);
									if (content.StartsWith("file:/"))
									{
										//UPGRADE_ISSUE: Constructor 'java.beans.PropertyChangeEvent.PropertyChangeEvent' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javabeansPropertyChangeEventPropertyChangeEvent_javalangObject_javalangString_javalangObject_javalangObject'"
										SupportClass.PropertyChangingEventArgs pce = new PropertyChangeEvent(this, FD_PROPERTY_FILENAME, fd_oldFileName, content);
										//UPGRADE_ISSUE: Method 'java.beans.PropertyChangeSupport.firePropertyChange' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javabeansPropertyChangeSupport'"
										fd_propSupport.firePropertyChange(pce);
									}
									else
									{
										//UPGRADE_ISSUE: Constructor 'java.beans.PropertyChangeEvent.PropertyChangeEvent' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javabeansPropertyChangeEventPropertyChangeEvent_javalangObject_javalangString_javalangObject_javalangObject'"
										SupportClass.PropertyChangingEventArgs pce = new PropertyChangeEvent(this, FD_PROPERTY_INLINE, fd_oldFileName, content);
										//UPGRADE_ISSUE: Method 'java.beans.PropertyChangeSupport.firePropertyChange' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javabeansPropertyChangeSupport'"
										fd_propSupport.firePropertyChange(pce);
									}
									//UPGRADE_ISSUE: Method 'java.awt.dnd.DropTargetContext.dropComplete' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtdndDropTargetContext'"
									//UPGRADE_ISSUE: Method 'java.awt.dnd.DropTargetEvent.getDropTargetContext' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtdndDropTargetEvent'"
									dtde.getDropTargetContext().dropComplete(true);
								}
								return ;
							}
						}
					}
				}
				
				//UPGRADE_ISSUE: Method 'java.awt.dnd.DropTargetDropEvent.rejectDrop' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtdndDropTargetDropEventrejectDrop'"
				dtde.rejectDrop();
			}
		}
	}
}