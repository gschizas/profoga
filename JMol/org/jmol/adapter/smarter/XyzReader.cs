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
namespace org.jmol.adapter.smarter
{
	
	/// <summary> Have not been able to find any good description/reference of this
	/// file format. Suggestions appreciated
	/// </summary>
	
	class XyzReader:AtomSetCollectionReader
	{
		
		internal override AtomSetCollection readAtomSetCollection(System.IO.StreamReader reader)
		{
			
			atomSetCollection = new AtomSetCollection("xyz");
			
			try
			{
				int modelAtomCount;
				while ((modelAtomCount = readAtomCount(reader)) > 0)
				{
					atomSetCollection.newAtomSet();
					readAtomSetName(reader);
					readAtoms(reader, modelAtomCount);
				}
			}
			catch (System.Exception ex)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				atomSetCollection.errorMessage = "Could not read file:" + ex;
			}
			return atomSetCollection;
		}
		
		internal virtual int readAtomCount(System.IO.StreamReader reader)
		{
			System.String line = reader.ReadLine();
			if (line != null)
			{
				int atomCount = parseInt(line);
				if (atomCount > 0)
					return atomCount;
			}
			return 0;
		}
		
		internal virtual void  readAtomSetName(System.IO.StreamReader reader)
		{
			System.String name = reader.ReadLine().Trim();
			if (name.EndsWith("#noautobond"))
			{
				name = name.Substring(0, (name.LastIndexOf('#')) - (0)).Trim();
				atomSetCollection.setAtomSetCollectionProperty("noautobond", "true");
			}
			atomSetCollection.setAtomSetName(name);
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'chargeAndOrVector '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal float[] chargeAndOrVector = new float[4];
		//UPGRADE_NOTE: Final was removed from the declaration of 'isNaN '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal bool[] isNaN = new bool[4];
		
		internal virtual void  readAtoms(System.IO.StreamReader reader, int modelAtomCount)
		{
			for (int i = 0; i < modelAtomCount; ++i)
			{
				System.String line = reader.ReadLine();
				Atom atom = atomSetCollection.addNewAtom();
				atom.elementSymbol = parseToken(line);
				atom.x = parseFloat(line, ichNextParse);
				atom.y = parseFloat(line, ichNextParse);
				atom.z = parseFloat(line, ichNextParse);
				for (int j = 0; j < 4; ++j)
					isNaN[j] = System.Single.IsNaN(chargeAndOrVector[j] = parseFloat(line, ichNextParse));
				if (isNaN[0])
					continue;
				if (isNaN[1])
				{
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					atom.formalCharge = (int) chargeAndOrVector[0];
					continue;
				}
				if (isNaN[3])
				{
					atom.vectorX = chargeAndOrVector[0];
					atom.vectorY = chargeAndOrVector[1];
					atom.vectorZ = chargeAndOrVector[2];
					continue;
				}
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				atom.formalCharge = (int) chargeAndOrVector[0];
				atom.vectorX = chargeAndOrVector[1];
				atom.vectorY = chargeAndOrVector[2];
				atom.vectorZ = chargeAndOrVector[3];
			}
		}
	}
}