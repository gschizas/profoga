/* $RCSfile$
* $Author: egonw $
* $Date: 2005-11-10 16:52:44 +0100 (jeu., 10 nov. 2005) $
* $Revision: 4255 $
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
	
	class JmeReader:AtomSetCollectionReader
	{
		
		internal System.String line;
		internal SupportClass.Tokenizer tokenizer;
		
		internal override AtomSetCollection readAtomSetCollection(System.IO.StreamReader reader)
		{
			atomSetCollection = new AtomSetCollection("jme");
			
			try
			{
				line = reader.ReadLine();
				tokenizer = new SupportClass.Tokenizer(line, "\t ");
				int atomCount = parseInt(tokenizer.NextToken());
				System.Console.Out.WriteLine("atomCount=" + atomCount);
				int bondCount = parseInt(tokenizer.NextToken());
				atomSetCollection.CollectionName = "JME";
				readAtoms(atomCount);
				readBonds(bondCount);
			}
			catch (System.Exception ex)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				atomSetCollection.errorMessage = "Could not read file:" + ex;
				logger.log(atomSetCollection.errorMessage);
			}
			return atomSetCollection;
		}
		
		internal virtual void  readAtoms(int atomCount)
		{
			for (int i = 0; i < atomCount; ++i)
			{
				System.String strAtom = tokenizer.NextToken();
				//      System.out.println("strAtom=" + strAtom);
				int indexColon = strAtom.IndexOf(':');
				System.String elementSymbol = String.Intern((indexColon > 0?strAtom.Substring(0, (indexColon) - (0)):strAtom));
				float x = parseFloat(tokenizer.NextToken());
				float y = parseFloat(tokenizer.NextToken());
				float z = 0;
				Atom atom = atomSetCollection.addNewAtom();
				atom.elementSymbol = elementSymbol;
				atom.x = x; atom.y = y; atom.z = z;
			}
		}
		
		internal virtual void  readBonds(int bondCount)
		{
			for (int i = 0; i < bondCount; ++i)
			{
				int atomIndex1 = parseInt(tokenizer.NextToken());
				int atomIndex2 = parseInt(tokenizer.NextToken());
				int order = parseInt(tokenizer.NextToken());
				//      System.out.println("bond "+atomIndex1+"->"+atomIndex2+" "+order);
				if (order < 1)
				{
					//        System.out.println("Stereo found:" + order);
					order = ((order == - 1)?JmolAdapter.ORDER_STEREO_NEAR:JmolAdapter.ORDER_STEREO_FAR);
				}
				atomSetCollection.addBond(new Bond(atomIndex1 - 1, atomIndex2 - 1, order));
			}
		}
	}
}