/* $RCSfile$
* $Author: hansonr $
* $Date: 2006-04-13 13:28:49 +0200 (jeu., 13 avr. 2006) $
* $Revision: 4964 $
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
using JmolConstants = org.jmol.viewer.JmolConstants;
namespace org.jmol.adapter.smarter
{
	
	abstract class AtomSetCollectionReader
	{
		virtual internal JmolAdapter.Logger Logger
		{
			set
			{
				this.logger = value;
			}
			
		}
		internal AtomSetCollection atomSetCollection;
		internal JmolAdapter.Logger logger;
		
		internal const float ANGSTROMS_PER_BOHR = 0.5291772f;
		
		internal virtual void  initialize()
		{
		}
		
		internal abstract AtomSetCollection readAtomSetCollection(System.IO.StreamReader reader);
		
		internal virtual AtomSetCollection readAtomSetCollectionFromDOM(System.Object DOMNode)
		{
			return null;
		}
		
		internal int ichNextParse;
		
		internal virtual float parseFloat(System.String str)
		{
			return parseFloatChecked(str, 0, str.Length);
		}
		
		internal virtual float parseFloat(System.String str, int ich)
		{
			int cch = str.Length;
			if (ich >= cch)
				return System.Single.NaN;
			return parseFloatChecked(str, ich, cch);
		}
		
		internal virtual float parseFloat(System.String str, int ichStart, int ichMax)
		{
			int cch = str.Length;
			if (ichMax > cch)
				ichMax = cch;
			if (ichStart >= ichMax)
				return System.Single.NaN;
			return parseFloatChecked(str, ichStart, ichMax);
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'decimalScale'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly float[] decimalScale = new float[]{0.1f, 0.01f, 0.001f, 0.0001f, 0.00001f, 0.000001f, 0.0000001f, 0.00000001f};
		//UPGRADE_NOTE: Final was removed from the declaration of 'tensScale'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly float[] tensScale = new float[]{10, 100, 1000, 10000, 100000, 1000000};
		
		internal virtual float parseFloatChecked(System.String str, int ichStart, int ichMax)
		{
			bool digitSeen = false;
			float value_Renamed = 0;
			int ich = ichStart;
			char ch;
			while (ich < ichMax && ((ch = str[ich]) == ' ' || ch == '\t'))
				++ich;
			bool negative = false;
			if (ich < ichMax && str[ich] == '-')
			{
				++ich;
				negative = true;
			}
			ch = (char) (0);
			while (ich < ichMax && (ch = str[ich]) >= '0' && ch <= '9')
			{
				value_Renamed = value_Renamed * 10 + (ch - '0');
				++ich;
				digitSeen = true;
			}
			if (ch == '.')
			{
				int iscale = 0;
				while (++ich < ichMax && (ch = str[ich]) >= '0' && ch <= '9')
				{
					if (iscale < decimalScale.Length)
						value_Renamed += (ch - '0') * decimalScale[iscale];
					++iscale;
					digitSeen = true;
				}
			}
			if (!digitSeen)
				value_Renamed = System.Single.NaN;
			else if (negative)
				value_Renamed = - value_Renamed;
			if (ich < ichMax && (ch == 'E' || ch == 'e'))
			{
				if (++ich >= ichMax)
					return System.Single.NaN;
				ch = str[ich];
				if ((ch == '+') && (++ich >= ichMax))
					return System.Single.NaN;
				int exponent = parseIntChecked(str, ich, ichMax);
				if (exponent == System.Int32.MinValue)
					return System.Single.NaN;
				if (exponent > 0)
					value_Renamed = (float) (value_Renamed * ((exponent < tensScale.Length)?tensScale[exponent - 1]:System.Math.Pow(10, exponent)));
				else if (exponent < 0)
					value_Renamed = (float) (value_Renamed * ((- exponent < decimalScale.Length)?decimalScale[- exponent - 1]:System.Math.Pow(10, exponent)));
			}
			else
			{
				ichNextParse = ich; // the exponent code finds its own ichNextParse
			}
			//    System.out.println("parseFloat(" + str + "," + ichStart + "," +
			//                       ichMax + ") -> " + value);
			return value_Renamed;
		}
		
		internal virtual int parseInt(System.String str)
		{
			return parseIntChecked(str, 0, str.Length);
		}
		
		internal virtual int parseInt(System.String str, int ich)
		{
			int cch = str.Length;
			if (ich >= cch)
				return System.Int32.MinValue;
			return parseIntChecked(str, ich, cch);
		}
		
		internal virtual int parseInt(System.String str, int ichStart, int ichMax)
		{
			int cch = str.Length;
			if (ichMax > cch)
				ichMax = cch;
			if (ichStart >= ichMax)
				return System.Int32.MinValue;
			return parseIntChecked(str, ichStart, ichMax);
		}
		
		internal virtual int parseIntChecked(System.String str, int ichStart, int ichMax)
		{
			bool digitSeen = false;
			int value_Renamed = 0;
			int ich = ichStart;
			char ch;
			while (ich < ichMax && ((ch = str[ich]) == ' ' || ch == '\t'))
				++ich;
			bool negative = false;
			if (ich < ichMax && str[ich] == '-')
			{
				negative = true;
				++ich;
			}
			while (ich < ichMax && (ch = str[ich]) >= '0' && ch <= '9')
			{
				value_Renamed = value_Renamed * 10 + (ch - '0');
				digitSeen = true;
				++ich;
			}
			if (!digitSeen)
				value_Renamed = System.Int32.MinValue;
			else if (negative)
				value_Renamed = - value_Renamed;
			//    System.out.println("parseInt(" + str + "," + ichStart + "," +
			//                       ichMax + ") -> " + value);
			ichNextParse = ich;
			return value_Renamed;
		}
		
		internal virtual System.String[] getTokens(System.String line)
		{
			return getTokens(line, 0);
		}
		
		internal virtual System.String[] getTokens(System.String line, int ich)
		{
			if (line == null)
				return null;
			int cchLine = line.Length;
			if (ich > cchLine)
				return null;
			int tokenCount = countTokens(line, ich);
			System.String[] tokens = new System.String[tokenCount];
			ichNextParse = ich;
			for (int i = 0; i < tokenCount; ++i)
				tokens[i] = parseTokenChecked(line, ichNextParse, cchLine);
			/*
			System.out.println("-----------\nline:" + line);
			for (int i = 0; i < tokenCount; ++i) 
			System.out.println("token[" + i + "]=" + tokens[i]);
			*/
			return tokens;
		}
		
		internal virtual int countTokens(System.String line, int ich)
		{
			int tokenCount = 0;
			if (line != null)
			{
				int ichMax = line.Length;
				char ch;
				while (true)
				{
					while (ich < ichMax && ((ch = line[ich]) == ' ' || ch == '\t'))
						++ich;
					if (ich == ichMax)
						break;
					++tokenCount;
					do 
					{
						++ich;
					}
					while (ich < ichMax && ((ch = line[ich]) != ' ' && ch != '\t'));
				}
			}
			return tokenCount;
		}
		
		internal virtual System.String parseToken(System.String str)
		{
			return parseTokenChecked(str, 0, str.Length);
		}
		
		internal virtual System.String parseToken(System.String str, int ich)
		{
			int cch = str.Length;
			if (ich >= cch)
				return null;
			return parseTokenChecked(str, ich, cch);
		}
		
		internal virtual System.String parseToken(System.String str, int ichStart, int ichMax)
		{
			int cch = str.Length;
			if (ichMax > cch)
				ichMax = cch;
			if (ichStart >= ichMax)
				return null;
			return parseTokenChecked(str, ichStart, ichMax);
		}
		
		internal virtual System.String parseTokenChecked(System.String str, int ichStart, int ichMax)
		{
			int ich = ichStart;
			char ch;
			while (ich < ichMax && ((ch = str[ich]) == ' ' || ch == '\t'))
				++ich;
			int ichNonWhite = ich;
			while (ich < ichMax && ((ch = str[ich]) != ' ' && ch != '\t'))
				++ich;
			ichNextParse = ich;
			if (ichNonWhite == ich)
				return null;
			return str.Substring(ichNonWhite, (ich) - (ichNonWhite));
		}
		
		internal virtual System.String parseTrimmed(System.String str)
		{
			return parseTrimmedChecked(str, 0, str.Length);
		}
		
		internal virtual System.String parseTrimmed(System.String str, int ich)
		{
			int cch = str.Length;
			if (ich >= cch)
				return null;
			return parseTrimmedChecked(str, ich, cch);
		}
		
		internal virtual System.String parseTrimmed(System.String str, int ichStart, int ichMax)
		{
			int cch = str.Length;
			if (ichMax > cch)
				ichMax = cch;
			if (ichStart >= ichMax)
				return "";
			return parseTrimmedChecked(str, ichStart, ichMax);
		}
		
		internal virtual System.String parseTrimmedChecked(System.String str, int ichStart, int ichMax)
		{
			int ich = ichStart;
			char ch;
			while (ich < ichMax && ((ch = str[ich]) == ' ' || ch == '\t'))
				++ich;
			int ichLast = ichMax - 1;
			while (ichLast >= ich && ((ch = str[ichLast]) == ' ' || ch == '\t'))
				--ichLast;
			if (ichLast < ich)
				return "";
			ichNextParse = ichLast + 1;
			return str.Substring(ich, (ichLast + 1) - (ich));
		}
		
		internal static int[] doubleLength(int[] array)
		{
			return setLength(array, array.Length * 2);
		}
		
		internal static System.String[] doubleLength(System.String[] array)
		{
			return setLength(array, array.Length * 2);
		}
		
		internal static System.Object doubleLength(System.Object[] array)
		{
			return setLength(array, array.Length * 2);
		}
		
		internal static System.Object setLength(System.Object array, int newLength)
		{
			System.Object t = System.Array.CreateInstance(array.GetType().GetElementType(), newLength);
			int oldLength = ((System.Array) array).Length;
			Array.Copy(array, 0, t, 0, oldLength < newLength?oldLength:newLength);
			return t;
		}
		
		internal static System.String[] setLength(System.String[] array, int newLength)
		{
			System.String[] t = new System.String[newLength];
			if (array != null)
			{
				int oldLength = array.Length;
				Array.Copy(array, 0, t, 0, oldLength < newLength?oldLength:newLength);
			}
			return t;
		}
		
		internal static int[] setLength(int[] array, int newLength)
		{
			int oldLength = array.Length;
			int[] t = new int[newLength];
			Array.Copy(array, 0, t, 0, oldLength < newLength?oldLength:newLength);
			return t;
		}
		
		internal static float[] setLength(float[] array, int newLength)
		{
			int oldLength = array.Length;
			float[] t = new float[newLength];
			Array.Copy(array, 0, t, 0, oldLength < newLength?oldLength:newLength);
			return t;
		}
		
		internal virtual void  discardLines(System.IO.StreamReader reader, int nLines)
		{
			for (int i = nLines; --i >= 0; )
				reader.ReadLine();
		}
		
		internal virtual System.String discardLinesUntilStartsWith(System.IO.StreamReader reader, System.String startsWith)
		{
			System.String line;
			while ((line = reader.ReadLine()) != null && !line.StartsWith(startsWith))
			{
			}
			return line;
		}
		
		internal virtual System.String discardLinesUntilContains(System.IO.StreamReader reader, System.String containsMatch)
		{
			System.String line;
			while ((line = reader.ReadLine()) != null && line.IndexOf(containsMatch) < 0)
			{
			}
			return line;
		}
		
		internal virtual void  discardLinesUntilBlank(System.IO.StreamReader reader)
		{
			System.String line;
			while ((line = reader.ReadLine()) != null && line.Length != 0)
			{
			}
		}
		
		internal virtual System.String discardLinesUntilNonBlank(System.IO.StreamReader reader)
		{
			System.String line;
			while ((line = reader.ReadLine()) != null && line.Length == 0)
			{
			}
			return line;
		}
		
		internal static System.String getElementSymbol(int elementNumber)
		{
			if (elementNumber < 0 || elementNumber >= JmolConstants.elementSymbols.Length)
				elementNumber = 0;
			return JmolConstants.elementSymbols[elementNumber];
		}
		
		/*
		* miguel 2006 04 02
		* included in r4624+r4625, but not referenced by SpartanSmolReader
		* and not reviewed yet
		* 
		String checkLineForScript(String line) {
		int pt = line.indexOf("#jmolscript:");
		if (pt >= 0) {
		String script = line.substring(pt + 12, line.length());
		if (script.indexOf("#") >= 0) {
		script = script.substring(0, script.indexOf("#"));
		}
		String previousScript = atomSetCollection
		.getAtomSetCollectionProperty("jmolscript");
		if (previousScript == null)
		previousScript = "";
		else
		previousScript += ";";
		atomSetCollection.setAtomSetCollectionProperty("jmolscript",
		previousScript + script);
		line = line.substring(0, pt).trim();
		}
		return line;
		}
		
		String concatTokens(String[] tokens, int iFirst, int iEnd) {
		String str = "";
		String sep = "";
		for (int i = iFirst; i < iEnd; i++) {
		if (i < tokens.length) {
		str += sep + tokens[i];
		sep = " ";
		}
		}
		return str;
		}
		*/
		
		internal virtual System.String getString(System.String line, System.String strQuote)
		{
			int i = line.IndexOf(strQuote);
			int j = line.LastIndexOf(strQuote);
			return (j == i?"":line.Substring(i + 1, (j) - (i + 1)));
		}
	}
}