/* $RCSfile$
* $Author: migueljmol $
* $Date: 2005-11-27 22:39:31 +0100 (dim., 27 nov. 2005) $
* $Revision: 4285 $
*
* Copyright (C) 2005  Miguel, Jmol Development, www.jmol.org
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
namespace org.jmol.i18n
{
	
	public class GT
	{
		
		private static GT getTextWrapper = new GT();
		private System.Resources.ResourceManager translationResources;
		private System.Resources.ResourceManager appletTranslationResources;
		
		private GT()
		{
			//UPGRADE_TODO: Method 'java.util.Locale.getDefault' was converted to 'System.Threading.Thread.CurrentThread.CurrentCulture' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
			System.Globalization.CultureInfo locale = System.Threading.Thread.CurrentThread.CurrentCulture;
			//UPGRADE_TODO: Method 'java.util.Locale.getLanguage' was converted to 'System.Globalization.CultureInfo.TwoLetterISOLanguageName' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilLocalegetLanguage'"
			if ((locale != null) && (locale.TwoLetterISOLanguageName != null))
			{
				//UPGRADE_TODO: Method 'java.util.Locale.getLanguage' was converted to 'System.Globalization.CultureInfo.TwoLetterISOLanguageName' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilLocalegetLanguage'"
				System.String language = locale.TwoLetterISOLanguageName;
				//UPGRADE_TODO: Method 'java.util.Locale.getLanguage' was converted to 'System.Globalization.CultureInfo.TwoLetterISOLanguageName' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilLocalegetLanguage'"
				if ((language.Equals("")) || (language.Equals(new System.Globalization.CultureInfo("en").TwoLetterISOLanguageName)))
				{
					System.Console.Out.WriteLine("English: no need for gettext wrapper");
					return ;
				}
			}
			System.Console.Out.WriteLine("Instantiating gettext wrapper...");
			try
			{
				//UPGRADE_TODO: Make sure that resources used in this class are valid resource files. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1078'"
				translationResources = System.Resources.ResourceManager.CreateFileBasedResourceManager("org.jmol.translation.Jmol.Messages", "", null);
			}
			catch (System.Resources.MissingManifestResourceException mre)
			{
				System.Console.Out.WriteLine("Translations do not seem to have been installed!");
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				System.Console.Out.WriteLine(mre.Message);
				translationResources = null;
			}
			catch (System.Exception exception)
			{
				System.Console.Out.WriteLine("Some exception occured!");
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				System.Console.Out.WriteLine(exception.Message);
				SupportClass.WriteStackTrace(exception, Console.Error);
				translationResources = null;
			}
			try
			{
				//UPGRADE_TODO: Make sure that resources used in this class are valid resource files. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1078'"
				appletTranslationResources = System.Resources.ResourceManager.CreateFileBasedResourceManager("org.jmol.translation.JmolApplet.Messages", "", null);
			}
			catch (System.Resources.MissingManifestResourceException mre)
			{
				System.Console.Out.WriteLine("Applet translations do not seem to have been installed!");
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				System.Console.Out.WriteLine(mre.Message);
				appletTranslationResources = null;
			}
			catch (System.Exception exception)
			{
				System.Console.Out.WriteLine("Some exception occured!");
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				System.Console.Out.WriteLine(exception.Message);
				SupportClass.WriteStackTrace(exception, Console.Error);
				appletTranslationResources = null;
			}
		}
		
		public static System.String _(System.String string_Renamed)
		{
			return getTextWrapper.getString(string_Renamed);
		}
		
		public static System.String _(System.String string_Renamed, System.Object[] objects)
		{
			return getTextWrapper.getString(string_Renamed, objects);
		}
		
		private System.String getString(System.String string_Renamed)
		{
			if (translationResources != null)
			{
				try
				{
					//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
					System.String trans = translationResources.GetString(string_Renamed);
					//System.out.println("trans: " + string  + " ->" + trans);
					return trans;
				}
				catch (System.Resources.MissingManifestResourceException mre)
				{
					//System.out.println("No trans, using default: " + string);
					//return string;
				}
			}
			if (appletTranslationResources != null)
			{
				try
				{
					//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
					System.String trans = appletTranslationResources.GetString(string_Renamed);
					//System.out.println("trans: " + string  + " ->" + trans);
					return trans;
				}
				catch (System.Resources.MissingManifestResourceException mre)
				{
					//System.out.println("No trans, using default: " + string);
					//return string;
				}
			}
			if ((translationResources != null) || (appletTranslationResources != null))
			{
				System.Console.Out.WriteLine("No trans, using default: " + string_Renamed);
			}
			return string_Renamed;
		}
		
		private System.String getString(System.String string_Renamed, System.Object[] objects)
		{
			System.String trans = string_Renamed;
			if (translationResources != null)
			{
				try
				{
					//UPGRADE_TODO: Method 'java.text.MessageFormat.format' was converted to 'System.String.Format' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
					//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
					trans = System.String.Format(translationResources.GetString(string_Renamed), objects);
					//System.out.println("trans: " + string  + " ->" + trans);
					return trans;
				}
				catch (System.Resources.MissingManifestResourceException mre)
				{
					//trans = MessageFormat.format(string, objects);
					//System.out.println("No trans, using default: " + trans);
				}
			}
			if (appletTranslationResources != null)
			{
				try
				{
					//UPGRADE_TODO: Method 'java.text.MessageFormat.format' was converted to 'System.String.Format' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
					//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
					trans = System.String.Format(appletTranslationResources.GetString(string_Renamed), objects);
					//System.out.println("trans: " + string  + " ->" + trans);
					return trans;
				}
				catch (System.Resources.MissingManifestResourceException mre)
				{
					//trans = MessageFormat.format(string, objects);
					//System.out.println("No trans, using default: " + trans);
				}
			}
			//UPGRADE_TODO: Method 'java.text.MessageFormat.format' was converted to 'System.String.Format' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
			trans = System.String.Format(string_Renamed, objects);
			if ((translationResources != null) || (appletTranslationResources != null))
			{
				System.Console.Out.WriteLine("No trans, using default: " + trans);
			}
			return trans;
		}
	}
}