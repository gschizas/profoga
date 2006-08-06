/* $RCSfile$
* $Author: egonw $
* $Date: 2005-11-10 16:52:44 +0100 (jeu., 10 nov. 2005) $
* $Revision: 4255 $
*
* Copyright (C) 2002-2005  The Jmol Development Team
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
	
	/// <summary> Provides access to resources (for example, strings and images). This class is
	/// a singleton which is retrieved by the getInstance method.
	/// 
	/// </summary>
	/// <author>  Bradley A. Smith (bradley@baysmith.com)
	/// </author>
	class JmolResourceHandler
	{
		public static JmolResourceHandler Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new JmolResourceHandler();
				}
				return instance;
			}
			
		}
		
		private static JmolResourceHandler instance;
		private System.Resources.ResourceManager stringsResourceBundle;
		private System.Resources.ResourceManager generalResourceBundle;
		
		private JmolResourceHandler()
		{
			System.String language = "en";
			System.String country = "";
			//UPGRADE_TODO: Method 'java.lang.System.getProperty' was converted to 'System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangSystemgetProperty_javalangString'"
			System.String localeString = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
			if (localeString != null)
			{
				SupportClass.Tokenizer st = new SupportClass.Tokenizer(localeString, "_");
				if (st.HasMoreTokens())
				{
					language = st.NextToken();
				}
				if (st.HasMoreTokens())
				{
					country = st.NextToken();
				}
			}
			//UPGRADE_WARNING: Constructor 'java.util.Locale.Locale' was converted to 'System.Globalization.CultureInfo' which may throw an exception. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1101'"
			System.Globalization.CultureInfo locale = new System.Globalization.CultureInfo(language + "-" + country);
			//UPGRADE_TODO: Make sure that resources used in this class are valid resource files. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1078'"
			System.Threading.Thread.CurrentThread.CurrentUICulture = locale;
			stringsResourceBundle = System.Resources.ResourceManager.CreateFileBasedResourceManager("org.openscience.jmol.Properties.Jmol", "", null);
			
			try
			{
				System.String t = "/org/openscience/jmol/Properties/Jmol-resources.properties";
				//UPGRADE_ISSUE: Constructor 'java.util.PropertyResourceBundle.PropertyResourceBundle' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilPropertyResourceBundlePropertyResourceBundle_javaioInputStream'"
				//UPGRADE_ISSUE: Method 'java.lang.Class.getResourceAsStream' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassgetResourceAsStream_javalangString'"
				generalResourceBundle = new PropertyResourceBundle(GetType().getResourceAsStream(t));
			}
			catch (System.IO.IOException ex)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				throw new System.SystemException(ex.ToString());
			}
		}
		
		public static System.String getStringX(System.String key)
		{
			return Instance.getString(key);
		}
		
		//UPGRADE_TODO: Class 'javax.swing.ImageIcon' was converted to 'System.Drawing.Image' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingImageIcon'"
		public static System.Drawing.Image getIconX(System.String key)
		{
			return Instance.getIcon(key);
		}
		
		//UPGRADE_TODO: Class 'javax.swing.ImageIcon' was converted to 'System.Drawing.Image' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingImageIcon'"
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'getIcon'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		private System.Drawing.Image getIcon(System.String key)
		{
			lock (this)
			{
				
				System.String resourceName = null;
				try
				{
					resourceName = getString(key);
				}
				catch (System.Resources.MissingManifestResourceException e)
				{
				}
				
				if (resourceName != null)
				{
					System.String imageName = "org/openscience/jmol/images/" + resourceName;
					//UPGRADE_ISSUE: Method 'java.lang.ClassLoader.getResource' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassLoader'"
					//UPGRADE_ISSUE: Method 'java.lang.Class.getClassLoader' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassgetClassLoader'"
					System.Uri imageUrl = this.GetType().getClassLoader().getResource(imageName);
					if (imageUrl != null)
					{
						//UPGRADE_ISSUE: Constructor 'javax.swing.ImageIcon.ImageIcon' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingImageIconImageIcon_javanetURL'"
						return new ImageIcon(imageUrl);
					}
					/*
					System.err.println("Warning: unable to load " + resourceName
					+ " for icon " + key + ".");
					*/
				}
				return null;
			}
		}
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'getString'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		private System.String getString(System.String key)
		{
			lock (this)
			{
				
				System.String result = null;
				try
				{
					//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
					result = stringsResourceBundle.GetString(key);
				}
				catch (System.Resources.MissingManifestResourceException e)
				{
				}
				if (result == null)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						result = generalResourceBundle.GetString(key);
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
					}
				}
				return result != null?result:key;
			}
		}
		
		/// <summary> A wrapper for easy detection which strings in the
		/// source code are localized.
		/// </summary>
		/// <param name="text">Text to translate
		/// </param>
		/// <returns> Translated text
		/// </returns>
		/*private synchronized String translate(String text) {
		StringTokenizer st = new StringTokenizer(text, " ");
		StringBuffer key = new StringBuffer();
		while (st.hasMoreTokens()) {
		key.append(st.nextToken());
		if (st.hasMoreTokens()) {
		key.append("_");
		}
		}
		String translatedText = getString(key.toString());
		return (translatedText != null) ? translatedText : text;
		}*/
	}
}