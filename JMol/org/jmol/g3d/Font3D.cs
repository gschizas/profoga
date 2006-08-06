/* $RCSfile$
* $Author: egonw $
* $Date: 2005-11-10 10:52:44 -0500 (Thu, 10 Nov 2005) $
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
namespace org.jmol.g3d
{
	
	/// <summary><p>
	/// Provides font support using a byte fid
	/// (<strong>F</strong>ont <strong>ID</strong>) as an index into font table.
	/// </p>
	/// <p>
	/// Supports standard font faces, font styles, and font sizes.
	/// </p>
	/// 
	/// </summary>
	/// <author>  Miguel, miguel@jmol.org
	/// </author>
	sealed public class Font3D
	{
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'fid '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public sbyte fid;
		//UPGRADE_NOTE: Final was removed from the declaration of 'fontFace '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public System.String fontFace;
		//UPGRADE_NOTE: Final was removed from the declaration of 'fontStyle '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public System.String fontStyle;
		//UPGRADE_NOTE: Final was removed from the declaration of 'fontSize '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public short fontSize;
		//UPGRADE_NOTE: Final was removed from the declaration of 'font '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public System.Drawing.Font font;
		//UPGRADE_NOTE: Final was removed from the declaration of 'fontMetrics '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public System.Drawing.Font fontMetrics;
		
		private Font3D(sbyte fid, int idFontFace, int idFontStyle, int fontSize, System.Drawing.Font font, System.Drawing.Font fontMetrics)
		{
			this.fid = fid;
			this.fontFace = fontFaces[idFontFace];
			this.fontStyle = fontStyles[idFontStyle];
			this.fontSize = (short) fontSize;
			this.font = font;
			this.fontMetrics = fontMetrics;
		}
		
		////////////////////////////////////////////////////////////////
		
		internal static System.Drawing.Graphics graphicsOffscreen;
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'initialize'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		internal static void  initialize(Platform3D platform)
		{
			lock (typeof(org.jmol.g3d.Font3D))
			{
				if (graphicsOffscreen == null)
					graphicsOffscreen = System.Drawing.Graphics.FromImage(platform.allocateOffscreenImage(1, 1));
			}
		}
		
		private const int FONT_ALLOCATION_UNIT = 8;
		private static int fontkeyCount = 1;
		private static short[] fontkeys = new short[FONT_ALLOCATION_UNIT];
		private static Font3D[] font3ds = new Font3D[FONT_ALLOCATION_UNIT];
		
		internal static Font3D getFont3D(int fontface, int fontstyle, int fontsize, Platform3D platform)
		{
			if (graphicsOffscreen == null)
				initialize(platform);
			/*
			System.out.println("Font3D.getFont3D("  + fontFaces[fontface] + "," +
			fontStyles[fontstyle] + "," + fontsize + ")");
			*/
			if (fontsize > 63)
				fontsize = 63;
			short fontkey = (short) (((fontface & 3) << 8) | ((fontstyle & 3) << 6) | fontsize);
			// watch out for race condition here!
			for (int i = fontkeyCount; --i > 0; )
				if (fontkey == fontkeys[i])
					return font3ds[i];
			return allocFont3D(fontkey, fontface, fontstyle, fontsize);
		}
		
		/*
		FontMetrics getFontMetrics(Font font) {
		if (gOffscreen == null)
		checkOffscreenSize(16, 64);
		return gOffscreen.getFontMetrics(font);
		}
		*/
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'allocFont3D'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public static Font3D allocFont3D(short fontkey, int fontface, int fontstyle, int fontsize)
		{
			lock (typeof(org.jmol.g3d.Font3D))
			{
				// recheck in case another process just allocated one
				for (int i = fontkeyCount; --i > 0; )
					if (fontkey == fontkeys[i])
						return font3ds[i];
				int fontIndexNext = fontkeyCount++;
				if (fontIndexNext == fontkeys.Length)
				{
					short[] t0 = new short[fontIndexNext + FONT_ALLOCATION_UNIT];
					Array.Copy(fontkeys, 0, t0, 0, fontIndexNext);
					fontkeys = t0;
					
					Font3D[] t1 = new Font3D[fontIndexNext + FONT_ALLOCATION_UNIT];
					Array.Copy(font3ds, 0, t1, 0, fontIndexNext);
					font3ds = t1;
				}
				//UPGRADE_NOTE: If the given Font Name does not exist, a default Font instance is created. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1075'"
				System.Drawing.Font font = new System.Drawing.Font(fontFaces[fontface], fontsize, (System.Drawing.FontStyle) fontstyle);
				if (graphicsOffscreen == null)
					System.Console.Out.WriteLine("Font3D.graphicsOffscreen not initialized");
				System.Drawing.Font fontMetrics = font;
				Font3D font3d = new Font3D((sbyte) fontIndexNext, fontface, fontstyle, fontsize, font, fontMetrics);
				// you must set the font3d before setting the fontkey in order
				// to prevent a race condition with getFont3D
				font3ds[fontIndexNext] = font3d;
				fontkeys[fontIndexNext] = fontkey;
				return font3d;
			}
		}
		
		public const int FONT_FACE_SANS = 0;
		public const int FONT_FACE_SERIF = 1;
		public const int FONT_FACE_MONO = 2;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'fontFaces'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly System.String[] fontFaces = new System.String[]{"SansSerif", "Serif", "Monospaced", ""};
		
		public const int FONT_STYLE_PLAIN = 0;
		public const int FONT_STYLE_BOLD = 1;
		public const int FONT_STYLE_ITALIC = 2;
		public const int FONT_STYLE_BOLDITALIC = 3;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'fontStyles'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly System.String[] fontStyles = new System.String[]{"Plain", "Bold", "Italic", "BoldItalic"};
		
		public static int getFontFaceID(System.String fontface)
		{
			if ("Monospaced".ToUpper().Equals(fontface.ToUpper()))
				return FONT_FACE_MONO;
			if ("Serif".ToUpper().Equals(fontface.ToUpper()))
				return FONT_FACE_SERIF;
			return FONT_FACE_SANS;
		}
		
		public static int getFontStyleID(System.String fontstyle)
		{
			int i = 4;
			while (--i > 0)
				if (fontStyles[i].ToUpper().Equals(fontstyle.ToUpper()))
					break;
			return i;
		}
		
		public static Font3D getFont3D(sbyte fontID)
		{
			return font3ds[fontID & 0xFF];
		}
	}
}