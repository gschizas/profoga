/* $RCSfile$
* $Author: egonw $
* $Date: 2005-11-10 16:52:44 +0100 (jeu., 10 nov. 2005) $
* $Revision: 4255 $
*
* Copyright (C) 2003-2005  Miguel, Jmol Development, www.jmol.org
* Copyright (C) 2005  Peter Knowles
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
namespace org.jmol.adapter.smarter
{
	
	/// <summary> A Molpro 2005 reader</summary>
	class MolproReader:CmlReader
	{
		
		internal override AtomSetCollection readAtomSetCollection(System.IO.StreamReader reader)
		{
			return readAtomSetCollectionSax(reader, (new MolproHandler(this)), "molpro");
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'MolproHandler' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		internal class MolproHandler:CmlHandler
		{
			public MolproHandler(MolproReader enclosingInstance):base(enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(MolproReader enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private MolproReader enclosingInstance;
			public new MolproReader Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			// tag names in http://www.molpro.net/schema/molpro2005
			private System.String normalCoordinateTag = "normalCoordinate";
			private System.String vibrationsTag = "vibrations";
			
			public override void  startElement(System.String namespaceURI, System.String localName, System.String qName, SaxAttributesSupport atts)
			{
				//logger.log("startElement(" + namespaceURI + "," + localName +
				//"," + qName + "," + atts +  ")");
				// the CML stuff
				
				//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
				System.Collections.Hashtable hashAtts = new System.Collections.Hashtable(atts.GetLength());
				for (int i = atts.GetLength(); --i >= 0; )
					hashAtts[atts.GetLocalName(i)] = atts.GetValue(i);
				
				Enclosing_Instance.processStartElement(namespaceURI, localName, qName, hashAtts);
				// the extra Molpro stuff
				molproStartElement(namespaceURI, localName, qName, atts);
			}
			
			internal int frequencyCount;
			
			public virtual void  molproStartElement(System.String namespaceURI, System.String localName, System.String qName, SaxAttributesSupport atts)
			{
				//logger.log("molproStartElement(" + namespaceURI + "," + localName +
				//"," + qName + "," + atts +  ")");
				if (normalCoordinateTag.Equals(localName))
				{
					//int atomCount = atomSetCollection.getLastAtomSetAtomCount();
					System.String wavenumber = "";
					//String units = "";
					//String tokens[];
					Enclosing_Instance.atomSetCollection.cloneLastAtomSet();
					frequencyCount++;
					for (int i = atts.GetLength(); --i >= 0; )
					{
						System.String attLocalName = atts.GetLocalName(i);
						System.String attValue = atts.GetValue(i);
						if ("wavenumber".Equals(attLocalName))
						{
							wavenumber = attValue;
						}
						else if ("units".Equals(attLocalName))
						{
							//units = attValue;
						}
					}
					Enclosing_Instance.atomSetCollection.setAtomSetProperty("Frequency", wavenumber + " cm**-1");
					//logger.log("new normal mode " + wavenumber + " " + units);
					Enclosing_Instance.keepChars = true;
					return ;
				}
				
				if (vibrationsTag.Equals(localName))
				{
					frequencyCount = 0;
					return ;
				}
			}
			
			public override void  endElement(System.String uri, System.String localName, System.String qName)
			{
				/*
				System.out.println("endElement(" + uri + "," + localName +
				"," + qName + ")");
				/* */
				Enclosing_Instance.processEndElement(uri, localName, qName);
				molproProcessEndElement(uri, localName, qName);
				Enclosing_Instance.keepChars = false;
				Enclosing_Instance.title = null;
				Enclosing_Instance.dictRef = null;
				Enclosing_Instance.chars = null;
			}
			
			public virtual void  molproProcessEndElement(System.String uri, System.String localName, System.String qName)
			{
				if (normalCoordinateTag.Equals(localName))
				{
					int atomCount = Enclosing_Instance.atomSetCollection.LastAtomSetAtomCount;
					Enclosing_Instance.tokens = Enclosing_Instance.getTokens(Enclosing_Instance.chars);
					for (int offset = Enclosing_Instance.tokens.Length - atomCount * 3, i = 0; i < atomCount; i++)
					{
						Atom atom = Enclosing_Instance.atomSetCollection.atoms[i + Enclosing_Instance.atomSetCollection.currentAtomSetIndex * atomCount];
						atom.vectorX = Enclosing_Instance.parseFloat(Enclosing_Instance.tokens[offset++]);
						atom.vectorY = Enclosing_Instance.parseFloat(Enclosing_Instance.tokens[offset++]);
						atom.vectorZ = Enclosing_Instance.parseFloat(Enclosing_Instance.tokens[offset++]);
					}
				}
			}
		}
	}
}