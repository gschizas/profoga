/// <summary> Copyright (c) 2005, Shravan Sadasivan & Richard Spinney 
/// Department of Chemisty - The Ohio State University
/// </summary>
using System;
namespace org.jmol.jcamp.utils
{
	
	public class StringDataUtils
	{
		
		/// <summary> Returns the sub-string of the string provided</summary>
		/// <param name="str">
		/// </param>
		/// <param name="i">
		/// </param>
		/// <param name="j">
		/// </param>
		/// <returns> String
		/// </returns>
		public static System.String jcampSubString(System.String str, int i, int j)
		{
			if (str.Length < j)
				return str;
			return str.Substring(i, (j) - (i));
		}
		
		/// <summary> Truncates blank spaces at the end of the string provided</summary>
		/// <param name="str">
		/// </param>
		/// <returns> String
		/// </returns>
		public static System.String truncateEndBlanks(System.String str)
		{
			while (str[str.Length - 1] == ' ')
				str = str.Substring(0, (str.Length - 1) - (0));
			return str;
		}
		
		/// <summary> Compares two strings provided and returns an expected integer value based on the comparison</summary>
		/// <param name="str1">
		/// </param>
		/// <param name="str2">
		/// </param>
		/// <returns> int
		/// </returns>
		public static int compareStrings(System.String str1, System.String str2)
		{
			if (str1 == null)
				return - 1;
			if (str2 == null)
				return - 1;
			
			if (str1.Length != str2.Length)
				return - 1;
			
			return String.CompareOrdinal(str1, str2);
		}
		
		/// <summary> Reduces the precision of numerical data strings provided</summary>
		/// <param name="data">
		/// </param>
		/// <returns> String
		/// </returns>
		public static System.String reduceDataPrecision(System.String data)
		{
			// trop de precision sur Communicator!
			if (data.Length > 10)
			{
				if (data.IndexOf('E') != - 1)
					data = data.Substring(0, (data.IndexOf('E') - 1) - (0)) + "e" + data.Substring(data.IndexOf('E') + 1);
				if (data.IndexOf('e') == - 1)
					data = data.Substring(0, (9) - (0));
				else
					data = System.Convert.ToString(System.Math.Pow(10, System.Double.Parse(data.Substring(data.IndexOf('e') + 1))) * System.Double.Parse(data.Substring(0, (System.Math.Min(9, data.IndexOf('e') - 1)) - (0))));
			}
			return data;
		}
	}
}