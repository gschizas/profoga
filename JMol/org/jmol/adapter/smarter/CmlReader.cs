/* $RCSfile$
* $Author: hansonr $
* $Date: 2006-04-11 13:29:47 +0200 (mar., 11 avr. 2006) $
* $Revision: 4952 $
*
* Copyright (C) 2003-2005  Miguel, Jmol Development, www.jmol.org
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
using JmolAdapter = org.jmol.api.JmolAdapter;
namespace org.jmol.adapter.smarter
{
	
	/// <summary> A CML2 Reader - 
	/// If passed a bufferedReader (from a file or inline string), we
	/// generate a SAX parser and use callbacks to construct an
	/// AtomSetCollection.
	/// If passed a JSObject (from LiveConnect) we treat it as a JS DOM
	/// tree, and walk the tree, (using the same processing as the SAX
	/// parser) to construct the AtomSetCollection.
	/// </summary>
	
	/// <summary> NB Note that the processing routines are shared between SAX 
	/// and DOM processors. This means that attributes must be
	/// transformed from either Attributes (SAX) or JSObjects (DOM)
	/// to HashMap name:value pairs. 
	/// This is done in startElement (SAX) or walkDOMTree (DOM)
	/// </summary>
	
	class CmlReader:AtomSetCollectionReader
	{
		virtual internal XmlSAXDocumentManager XmlReader
		{
			get
			{
				XmlSAXDocumentManager xmlr = null;
				// JAXP is preferred (comes with Sun JVM 1.4.0 and higher)
				//UPGRADE_ISSUE: Method 'java.lang.System.getProperty' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangSystem'"
				if (xmlr == null && String.CompareOrdinal(System_Renamed.getProperty("java.version"), "1.4") >= 0)
					xmlr = allocateXmlReader14();
				// Aelfred is the first alternative.
				if (xmlr == null)
					xmlr = allocateXmlReaderAelfred2();
				return xmlr;
			}
			
		}
		
		internal override AtomSetCollection readAtomSetCollection(System.IO.StreamReader reader)
		{
			logger.log("readAtomSetCollection");
			return readAtomSetCollectionSax(reader, (new CmlHandler(this)), "cml");
		}
		
		internal virtual AtomSetCollection readAtomSetCollectionSax(System.IO.StreamReader reader, System.Object handler, System.String name)
		{
			atomSetCollection = new AtomSetCollection(name);
			
			logger.log("readAtomSetCollectionSax");
			
			XmlSAXDocumentManager xmlr = XmlReader;
			if (xmlr == null)
			{
				logger.log("No XML reader found");
				atomSetCollection.errorMessage = "No XML reader found";
				return atomSetCollection;
			}
			// logger.log("opening InputSource");
			XmlSourceSupport is_Renamed = new XmlSourceSupport(reader);
			is_Renamed.Uri = "foo";
			// logger.log("setting features");
			xmlr.setFeature("http://xml.org/sax/features/validation", false);
			xmlr.setFeature("http://xml.org/sax/features/namespaces", true);
			xmlr.setEntityResolver((CmlHandler) handler);
			xmlr.setContentHandler((CmlHandler) handler);
			//UPGRADE_TODO: Method 'org.xml.sax.XMLReader.setErrorHandler' was converted to 'XmlSAXDocumentManager.SetErrorHandler' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_orgxmlsaxXMLReadersetErrorHandler_orgxmlsaxErrorHandler'"
			xmlr.setErrorHandler((CmlHandler) handler);
			
			xmlr.parse(is_Renamed);
			
			if (atomSetCollection.atomCount == 0)
			{
				atomSetCollection.errorMessage = "No atoms in file";
			}
			return atomSetCollection;
		}
		
		
		internal override AtomSetCollection readAtomSetCollectionFromDOM(System.Object Node)
		{
			//UPGRADE_TODO: Class 'netscape.javascript.JSObject' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
			JSObject DOMNode = (JSObject) Node;
			atomSetCollection = new AtomSetCollection("cml");
			logger.log("CmlReader.readAtomSetCollectionfromDOM\n");
			
			checkFirstNode(DOMNode);
			
			walkDOMTree(DOMNode);
			
			// logger.log("atom count:");
			// logger.log(atomSetCollection.atomCount);
			
			if (atomSetCollection.atomCount == 0)
			{
				atomSetCollection.errorMessage = "No atoms in file";
			}
			return atomSetCollection;
		}
		
		internal virtual XmlSAXDocumentManager allocateXmlReader14()
		{
			XmlSAXDocumentManager xmlr = null;
			try
			{
				XmlSAXDocumentManager spf = XmlSAXDocumentManager.NewInstance();
				spf.NamespaceAllowed = true;
				XmlSAXDocumentManager saxParser = XmlSAXDocumentManager.CloneInstance(spf);
				//UPGRADE_TODO: Method 'javax.xml.parsers.SAXParser.getXMLReader' was converted to 'SupportClass.XmlSAXDocumentManager' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
				xmlr = saxParser;
				logger.log("Using JAXP/SAX XML parser.");
			}
			catch (System.Exception e)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				logger.log("Could not instantiate JAXP/SAX XML reader: " + e.Message);
			}
			return xmlr;
		}
		
		internal virtual XmlSAXDocumentManager allocateXmlReaderAelfred2()
		{
			XmlSAXDocumentManager xmlr = null;
			try
			{
				//UPGRADE_TODO: Method 'java.lang.Class.newInstance' was converted to 'System.Activator.CreateInstance' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangClassnewInstance'"
				//UPGRADE_ISSUE: Method 'java.lang.ClassLoader.loadClass' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassLoader'"
				//UPGRADE_ISSUE: Method 'java.lang.Class.getClassLoader' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassgetClassLoader'"
				xmlr = (XmlSAXDocumentManager) System.Activator.CreateInstance(this.GetType().getClassLoader().loadClass("gnu.xml.aelfred2.XmlReader"));
				logger.log("Using Aelfred2 XML parser.");
			}
			catch (System.Exception e)
			{
				logger.log("Could not instantiate Aelfred2 XML reader!");
			}
			return xmlr;
		}
		
		////////////////////////////////////////////////////////////////
		// Main body of class; variablaes & functiopns shared by DOM & SAX alike.
		
		internal Atom atom;
		internal float[] notionalUnitcell;
		
		// the same atom array gets reused
		// it will grow to the maximum length;
		// atomCount holds the current number of atoms
		internal int atomCount;
		internal Atom[] atomArray = new Atom[100];
		
		internal int bondCount;
		internal Bond[] bondArray = new Bond[100];
		
		// the same string array gets reused
		// tokenCount holds the current number of tokens
		// see breakOutTokens
		internal int tokenCount;
		internal System.String[] tokens = new System.String[16];
		
		// this param is used to keep track of the parent element type
		internal int elementContext;
		internal const int UNSET = 0;
		internal const int CRYSTAL = 1;
		internal const int ATOM = 2;
		
		// this param is used to signal that chars should be kept
		internal bool keepChars;
		internal System.String chars;
		
		// do some bookkeeping of attrib value across the element
		internal System.String dictRef;
		internal System.String title;
		
		// this routine breaks out all the tokens in a string
		// results are placed into the tokens array
		internal virtual void  breakOutTokens(System.String str)
		{
			SupportClass.Tokenizer st = new SupportClass.Tokenizer(str);
			tokenCount = st.Count;
			if (tokenCount > tokens.Length)
				tokens = new System.String[tokenCount];
			for (int i = 0; i < tokenCount; ++i)
			{
				try
				{
					tokens[i] = st.NextToken();
				}
				catch (System.ArgumentOutOfRangeException nsee)
				{
					tokens[i] = null;
				}
			}
		}
		
		internal virtual int parseBondToken(System.String str)
		{
			if (str.Length == 1)
			{
				switch (str[0])
				{
					
					case 'S': 
						return 1;
					
					case 'D': 
						return 2;
					
					case 'T': 
						return 3;
					
					case 'A': 
						return JmolAdapter.ORDER_AROMATIC;
					}
				return parseInt(str);
			}
			if (str.Equals("partial01"))
				return JmolAdapter.ORDER_PARTIAL01;
			if (str.Equals("partial12"))
				return JmolAdapter.ORDER_PARTIAL12;
			float floatOrder = parseFloat(str);
			if (floatOrder == 1.5)
				return JmolAdapter.ORDER_AROMATIC;
			if (floatOrder == 2)
				return 2;
			if (floatOrder == 3)
				return 3;
			return 1;
		}
		
		internal virtual void  breakOutAtomTokens(System.String str)
		{
			breakOutTokens(str);
			checkAtomArrayLength(tokenCount);
		}
		
		internal virtual void  checkAtomArrayLength(int newAtomCount)
		{
			if (atomCount == 0)
			{
				if (newAtomCount > atomArray.Length)
					atomArray = new Atom[newAtomCount];
				for (int i = newAtomCount; --i >= 0; )
					atomArray[i] = new Atom();
				atomCount = newAtomCount;
			}
			else if (newAtomCount != atomCount)
			{
				throw new System.IndexOutOfRangeException("bad atom attribute length");
			}
		}
		
		internal virtual void  breakOutBondTokens(System.String str)
		{
			breakOutTokens(str);
			checkBondArrayLength(tokenCount);
		}
		
		internal virtual void  checkBondArrayLength(int newBondCount)
		{
			if (bondCount == 0)
			{
				if (newBondCount > bondArray.Length)
					bondArray = new Bond[newBondCount];
				for (int i = newBondCount; --i >= 0; )
					bondArray[i] = new Bond();
				bondCount = newBondCount;
			}
			else if (newBondCount != bondCount)
			{
				throw new System.IndexOutOfRangeException("bad bond attribute length");
			}
		}
		
		////////////////////////////////////////////////////////////////
		
		//UPGRADE_TODO: Class 'netscape.javascript.JSObject' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
		internal virtual void  checkFirstNode(JSObject DOMNode)
		{
			// System.out.println("checkFirstNode");
			if (DOMNode == null)
				throw new System.SystemException("Not a node");
			//UPGRADE_TODO: Method 'netscape.javascript.JSObject.getMember' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
			System.String namespaceURI = (System.String) DOMNode.getMember("namespaceURI");
			//UPGRADE_TODO: Method 'netscape.javascript.JSObject.getMember' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
			System.String localName = (System.String) DOMNode.getMember("localName");
			
			if (((System.Object) "http://www.xml-cml.org/schema/cml2/core" != (System.Object) namespaceURI) || ((System.Object) "cml" != (System.Object) localName))
				new System.SystemException("Not a cml:cml node");
		}
		
		
		// walk dom tree given by JSObject. For every element, call
		// startElement with the appropriate strings etc., and then
		// endElement when the element is closed.
		
		//UPGRADE_TODO: Class 'netscape.javascript.JSObject' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
		internal virtual void  walkDOMTree(JSObject DOMNode)
		{
			
			//UPGRADE_TODO: Method 'netscape.javascript.JSObject.getMember' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
			System.String namespaceURI = (System.String) DOMNode.getMember("namespaceURI");
			//UPGRADE_TODO: Method 'netscape.javascript.JSObject.getMember' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
			System.String localName = (System.String) DOMNode.getMember("localName");
			//UPGRADE_TODO: Method 'netscape.javascript.JSObject.getMember' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
			System.String qName = (System.String) DOMNode.getMember("nodeName");
			//UPGRADE_TODO: Class 'netscape.javascript.JSObject' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
			//UPGRADE_TODO: Method 'netscape.javascript.JSObject.getMember' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
			JSObject attributes = (JSObject) DOMNode.getMember("attributes");
			
			//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
			System.Collections.Hashtable atts;
			if (attributes != null)
			// in case this is a text or other weird sort of node.
				atts = attributesToHashMap(attributes);
			else
			{
				//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
				atts = new System.Collections.Hashtable(0);
			}
			
			// put the attributes all into a name:value HashMap for processing.
			// This should be as easy as the code snippet below.
			// Unfortunately, Opera 8.5 doesn't work properly with that, so
			// we have to use attributesToHashMap.
			//HashMap atts = new HashMap(numAtts);
			//for (int i = 0; i < numAtts; i++) {
			//  String attLocalName = (String) ((JSObject) attributes.getSlot(i)).getMember("localName");
			//  String attValue = (String) ((JSObject) attributes.getSlot(i)).getMember("value");
			//  atts.put(attLocalName, attValue);
			//}
			
			
			//if ("http://www.xml-cml.org/schema/cml2/core"!=namespaceURI)
			//    return;
			
			processStartElement(namespaceURI, localName, qName, atts);
			//UPGRADE_TODO: Method 'netscape.javascript.JSObject.call' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
			if (((System.Boolean) DOMNode.call("hasChildNodes", (System.Object[]) null)))
			{
				//UPGRADE_TODO: Class 'netscape.javascript.JSObject' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
				//UPGRADE_TODO: Method 'netscape.javascript.JSObject.getMember' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
				for (JSObject nextNode = (JSObject) DOMNode.getMember("firstChild"); nextNode != (JSObject) null; nextNode = (JSObject) nextNode.getMember("nextSibling"))
					walkDOMTree(nextNode);
			}
			processEndElement(namespaceURI, localName, qName);
		}
		
		//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
		//UPGRADE_TODO: Class 'netscape.javascript.JSObject' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
		internal virtual System.Collections.Hashtable attributesToHashMap(JSObject attributes)
		{
			// list of all attributes we might be interested in:
			System.Object[] interestingAtts = new System.Object[]{"title", "id", "x3", "y3", "z3", "x2", "y2", "elementType", "formalCharge", "atomId", "atomRefs2", "order", "atomRef1", "atomRef2", "dictRef"};
			
			//UPGRADE_TODO: Method 'netscape.javascript.JSObject.getMember' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
			int numAtts = System.Convert.ToInt32(((System.ValueType) attributes.getMember("length")));
			//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
			System.Collections.Hashtable atts = new System.Collections.Hashtable(numAtts);
			for (int i = interestingAtts.Length; --i >= 0; )
			{
				System.Object[] attArgs = new System.Object[]{interestingAtts[i]};
				//UPGRADE_TODO: Class 'netscape.javascript.JSObject' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
				//UPGRADE_TODO: Method 'netscape.javascript.JSObject.call' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
				JSObject attNode = (JSObject) attributes.call("getNamedItem", attArgs);
				if (attNode != null)
				{
					//UPGRADE_TODO: Method 'netscape.javascript.JSObject.getMember' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
					System.String attLocalName = (System.String) attNode.getMember("name");
					//UPGRADE_TODO: Method 'netscape.javascript.JSObject.getMember' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
					System.String attValue = (System.String) attNode.getMember("value");
					atts[attLocalName] = attValue;
				}
			}
			return atts;
		}
		
		internal int moleculeNesting = 0;
		
		//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
		internal virtual void  processStartElement(System.String namespaceURI, System.String localName, System.String qName, System.Collections.Hashtable atts)
		{
			
			if ("molecule".Equals(localName))
			{
				//  logger.log("found molecule");
				if (++moleculeNesting > 1)
					return ;
				atomSetCollection.newAtomSet();
				System.String collectionName = null;
				if (atts.ContainsKey("title"))
				{
					//UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
					collectionName = ((System.String) atts["title"]);
				}
				else if (atts.ContainsKey("id"))
				{
					//UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
					collectionName = ((System.String) atts["id"]);
				}
				if (collectionName != null)
				{
					atomSetCollection.setAtomSetName(collectionName);
				}
				return ;
			}
			if ("atom".Equals(localName))
			{
				//logger.log("found atom");
				elementContext = ATOM;
				atom = new Atom();
				bool coords3D = false;
				//UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
				atom.atomName = ((System.String) atts["id"]);
				if (atts.ContainsKey("x3"))
				{
					coords3D = true;
					//UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
					atom.x = parseFloat((System.String) atts["x3"]);
					//UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
					atom.y = parseFloat((System.String) atts["y3"]);
					//UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
					atom.z = parseFloat((System.String) atts["z3"]);
				}
				if (atts.ContainsKey("x2"))
				{
					if (System.Single.IsNaN(atom.x))
					{
						//UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
						atom.x = parseFloat((System.String) atts["x2"]);
					}
				}
				if (atts.ContainsKey("y2"))
				{
					if (System.Single.IsNaN(atom.y))
					{
						//UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
						atom.y = parseFloat((System.String) atts["y2"]);
					}
				}
				if (atts.ContainsKey("elementType"))
				{
					//UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
					atom.elementSymbol = ((System.String) atts["elementType"]);
				}
				if (atts.ContainsKey("formalCharge"))
				{
					//UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
					atom.formalCharge = parseInt((System.String) atts["formalCharge"]);
				}
				if (!coords3D)
				{
					atom.z = 0;
				}
				return ;
			}
			if ("atomArray".Equals(localName))
			{
				//  logger.log("found atomArray");
				atomCount = 0;
				bool coords3D = false;
				if (atts.ContainsKey("atomID"))
				{
					//UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
					breakOutAtomTokens((System.String) atts["atomID"]);
					for (int i = tokenCount; --i >= 0; )
						atomArray[i].atomName = tokens[i];
				}
				if (atts.ContainsKey("x3"))
				{
					coords3D = true;
					//UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
					breakOutAtomTokens((System.String) atts["x3"]);
					for (int i = tokenCount; --i >= 0; )
						atomArray[i].x = parseFloat(tokens[i]);
				}
				if (atts.ContainsKey("y3"))
				{
					//UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
					breakOutAtomTokens((System.String) atts["y3"]);
					for (int i = tokenCount; --i >= 0; )
						atomArray[i].y = parseFloat(tokens[i]);
				}
				if (atts.ContainsKey("z3"))
				{
					//UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
					breakOutAtomTokens((System.String) atts["z3"]);
					for (int i = tokenCount; --i >= 0; )
						atomArray[i].z = parseFloat(tokens[i]);
				}
				if (atts.ContainsKey("x2"))
				{
					//UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
					breakOutAtomTokens((System.String) atts["x2"]);
					for (int i = tokenCount; --i >= 0; )
						atomArray[i].x = parseFloat(tokens[i]);
				}
				if (atts.ContainsKey("y2"))
				{
					//UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
					breakOutAtomTokens((System.String) atts["y2"]);
					for (int i = tokenCount; --i >= 0; )
						atomArray[i].y = parseFloat(tokens[i]);
				}
				if (atts.ContainsKey("elementType"))
				{
					//UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
					breakOutAtomTokens((System.String) atts["elementType"]);
					for (int i = tokenCount; --i >= 0; )
						atomArray[i].elementSymbol = tokens[i];
				}
				for (int i = atomCount; --i >= 0; )
				{
					Atom atom = atomArray[i];
					if (!coords3D)
						atom.z = 0;
				}
				return ;
			}
			if ("bond".Equals(localName))
			{
				//  <bond atomRefs2="a20 a21" id="b41" order="2"/>
				int order = - 1;
				if (atts.ContainsKey("atomRefs2"))
				{
					//UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
					breakOutTokens((System.String) atts["atomRefs2"]);
				}
				if (atts.ContainsKey("order"))
				{
					//UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
					order = parseBondToken((System.String) atts["order"]);
				}
				if (tokenCount == 2 && order > 0)
				{
					atomSetCollection.addNewBond(tokens[0], tokens[1], order);
				}
				return ;
			}
			if ("bondArray".Equals(localName))
			{
				bondCount = 0;
				if (atts.ContainsKey("order"))
				{
					//UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
					breakOutBondTokens((System.String) atts["order"]);
					for (int i = tokenCount; --i >= 0; )
						bondArray[i].order = parseBondToken(tokens[i]);
				}
				if (atts.ContainsKey("atomRef1"))
				{
					//UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
					breakOutBondTokens((System.String) atts["atomRef1"]);
					for (int i = tokenCount; --i >= 0; )
						bondArray[i].atomIndex1 = atomSetCollection.getAtomNameIndex(tokens[i]);
				}
				if (atts.ContainsKey("atomRef2"))
				{
					//UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
					breakOutBondTokens((System.String) atts["atomRef2"]);
					for (int i = tokenCount; --i >= 0; )
						bondArray[i].atomIndex2 = atomSetCollection.getAtomNameIndex(tokens[i]);
				}
				return ;
			}
			if ("crystal".Equals(localName))
			{
				elementContext = CRYSTAL;
				notionalUnitcell = new float[6];
				for (int i = 6; --i >= 0; )
					notionalUnitcell[i] = System.Single.NaN;
				return ;
			}
			if ("scalar".Equals(localName))
			{
				//UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
				title = ((System.String) atts["title"]);
				//UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
				dictRef = ((System.String) atts["dictRef"]);
				keepChars = true;
				return ;
			}
		}
		
		internal virtual void  processEndElement(System.String uri, System.String localName, System.String qName)
		{
			if ("molecule".Equals(localName))
			{
				--moleculeNesting;
				return ;
			}
			if ("atom".Equals(localName))
			{
				if (atom.elementSymbol != null && !System.Single.IsNaN(atom.z))
				{
					atomSetCollection.addAtomWithMappedName(atom);
					
					/*  logger.log(" I just added an atom of type "
					+ atom.elementSymbol +
					" @ " + atom.x + "," + atom.y + "," + atom.z); */
				}
				atom = null;
				elementContext = UNSET;
				return ;
			}
			if ("crystal".Equals(localName))
			{
				elementContext = UNSET;
				for (int i = 6; --i >= 0; )
					if (System.Single.IsNaN(notionalUnitcell[i]))
					{
						logger.log("incomplete/unrecognized unitcell");
						return ;
					}
				atomSetCollection.notionalUnitcell = notionalUnitcell;
				return ;
			}
			if ("scalar".Equals(localName))
			{
				if (elementContext == CRYSTAL)
				{
					//          logger.log("CRYSTAL atts.title: " + title);
					if (title != null)
					{
						int i = 6;
						while (--i >= 0 && !title.Equals(AtomSetCollection.notionalUnitcellTags[i]))
						{
						}
						if (i >= 0)
							notionalUnitcell[i] = parseFloat(chars);
					}
					//          logger.log("CRYSTAL atts.dictRef: " + dictRef);
					if (dictRef != null)
					{
						int i = 6;
						while (--i >= 0 && !dictRef.Equals("cif:" + CifReader.cellParamNames[i]))
						{
						}
						if (i >= 0)
							notionalUnitcell[i] = parseFloat(chars);
					}
					return ;
				}
				if (elementContext == ATOM)
				{
					if ("jmol:charge".Equals(dictRef))
					{
						atom.partialCharge = parseFloat(chars);
						//logger.log("jmol.partialCharge=" + atom.partialCharge);
					}
				}
				return ;
			}
			if ("atomArray".Equals(localName))
			{
				//    logger.log("adding atomArray:" + atomCount);
				for (int i = 0; i < atomCount; ++i)
				{
					Atom atom = atomArray[i];
					if (atom.elementSymbol != null && !System.Single.IsNaN(atom.z))
						atomSetCollection.addAtomWithMappedName(atom);
				}
				return ;
			}
			if ("bondArray".Equals(localName))
			{
				//        logger.log("adding bondArray:" + bondCount);
				for (int i = 0; i < bondCount; ++i)
					atomSetCollection.addBond(bondArray[i]);
				return ;
			}
			
			keepChars = false;
			title = null;
			dictRef = null;
			chars = null;
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'CmlHandler' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		//UPGRADE_NOTE: The access modifier for this class or class field has been changed in order to prevent compilation errors due to the visibility level. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1296'"
		internal class CmlHandler:XmlSaxDefaultHandler, XmlSaxErrorHandler
		{
			public CmlHandler(CmlReader enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(CmlReader enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private CmlReader enclosingInstance;
			public CmlReader Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			public override void  startDocument()
			{
			}
			
			public override void  startElement(System.String namespaceURI, System.String localName, System.String qName, SaxAttributesSupport attributes)
			{
				
				/* logger.log("startElement(" + namespaceURI + "," + localName +
				"," + qName + ")"); */
				//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
				System.Collections.Hashtable atts = new System.Collections.Hashtable(attributes.GetLength());
				for (int i = attributes.GetLength(); --i >= 0; )
					atts[attributes.GetLocalName(i)] = attributes.GetValue(i);
				
				Enclosing_Instance.processStartElement(namespaceURI, localName, qName, atts);
			}
			
			public override void  endElement(System.String uri, System.String localName, System.String qName)
			{
				
				/* logger.log("endElement(" + uri + "," + localName +
				"," + qName + ")"); */
				Enclosing_Instance.processEndElement(uri, localName, qName);
				Enclosing_Instance.keepChars = false;
				Enclosing_Instance.title = null;
				Enclosing_Instance.dictRef = null;
				Enclosing_Instance.chars = null;
			}
			
			public override void  characters(System.Char[] ch, int start, int length)
			{
				//logger.log("End chars: " + new String(ch, start, length));
				if (Enclosing_Instance.keepChars)
				{
					if (Enclosing_Instance.chars == null)
					{
						Enclosing_Instance.chars = new System.String(ch, start, length);
					}
					else
					{
						Enclosing_Instance.chars += new System.String(ch, start, length);
					}
				}
			}
			
			// Methods for entity resolving, e.g. getting a DTD resolved
			
			public virtual XmlSourceSupport resolveEntity(System.String name, System.String publicId, System.String baseURI, System.String systemId)
			{
				Enclosing_Instance.logger.log("Not resolving this:");
				Enclosing_Instance.logger.log("      name: " + name);
				Enclosing_Instance.logger.log("  systemID: " + systemId);
				Enclosing_Instance.logger.log("  publicID: " + publicId);
				Enclosing_Instance.logger.log("   baseURI: " + baseURI);
				return null;
			}
			
			public override XmlSourceSupport resolveEntity(System.String publicId, System.String systemId)
			{
				Enclosing_Instance.logger.log("Not resolving this:");
				Enclosing_Instance.logger.log("  publicID: " + publicId);
				Enclosing_Instance.logger.log("  systemID: " + systemId);
				return null;
			}
			
			//UPGRADE_TODO: Class 'org.xml.sax.SAXParseException' was converted to 'System.xml.XmlException' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
			public override void  error(System.Xml.XmlException exception)
			{
				Enclosing_Instance.logger.log("SAX ERROR:" + exception.Message);
			}
			
			//UPGRADE_TODO: Class 'org.xml.sax.SAXParseException' was converted to 'System.xml.XmlException' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
			public override void  fatalError(System.Xml.XmlException exception)
			{
				Enclosing_Instance.logger.log("SAX FATAL:" + exception.Message);
			}
			
			//UPGRADE_TODO: Class 'org.xml.sax.SAXParseException' was converted to 'System.xml.XmlException' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
			public override void  warning(System.Xml.XmlException exception)
			{
				Enclosing_Instance.logger.log("SAX WARNING:" + exception.Message);
			}
		}
	}
}