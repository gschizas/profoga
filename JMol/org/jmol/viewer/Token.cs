/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-04-04 18:50:58 +0200 (mar., 04 avr. 2006) $
* $Revision: 4905 $
*
* Copyright (C) 2003-2005  The Jmol Development Team
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
namespace org.jmol.viewer
{
	
	class Token
	{
		
		internal int tok;
		internal System.Object value_Renamed;
		internal int intValue = System.Int32.MaxValue;
		
		internal Token(int tok, int intValue, System.Object value_Renamed)
		{
			this.tok = tok;
			this.intValue = intValue;
			this.value_Renamed = value_Renamed;
		}
		
		internal Token(int tok, int intValue)
		{
			this.tok = tok;
			this.intValue = intValue;
		}
		
		internal Token(int tok)
		{
			this.tok = tok;
		}
		
		internal Token(int tok, System.Object value_Renamed)
		{
			this.tok = tok;
			this.value_Renamed = value_Renamed;
		}
		
		internal const int nada = 0;
		internal const int identifier = 1;
		internal const int integer = 2;
		internal const int decimal_Renamed = 3;
		internal const int string_Renamed = 4;
		internal const int seqcode = 5;
		internal const int unknown = 6;
		internal const int keyword = 7;
		internal const int whitespace = 8;
		internal const int comment = 9;
		internal const int endofline = 10;
		internal const int endofstatement = 11;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'astrType'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] astrType = new System.String[]{"nada", "identifier", "integer", "decimal", "string", "seqcode", "unknown", "keyword"};
		
		internal const int command = (1 << 8);
		internal const int expressionCommand = (1 << 9); // expression command
		internal const int embeddedExpressions = (1 << 10); // embedded expressions
		internal const int setparam = (1 << 11); // parameter to set command
		internal const int showparam = (1 << 12); // parameter to show command
		internal const int bool_Renamed = (1 << 13);
		internal const int misc = (1 << 14); // misc parameter
		internal const int expression = (1 << 15); /// expression term
		// every property is also valid in an expression context
		//UPGRADE_NOTE: Final was removed from the declaration of 'atomproperty '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int atomproperty = (1 << 16) | expression;
		// every predefined is also valid in an expression context
		//UPGRADE_NOTE: Final was removed from the declaration of 'comparator '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int comparator = (1 << 17) | expression;
		//UPGRADE_NOTE: Final was removed from the declaration of 'predefinedset '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int predefinedset = (1 << 18) | expression;
		internal const int colorparam = (1 << 19);
		internal const int specialstring = (1 << 20); // load, echo, label
		// generally, the minus sign is used to denote atom ranges
		// this property is used for the few commands which allow negative integers
		internal const int negnums = (1 << 21);
		// for some commands the 'set' is optional
		// so, just delete the set command from the token list
		// but not for hbonds nor ssbonds
		internal const int setspecial = (1 << 22);
		
		// These are unrelated
		internal const int varArgCount = (1 << 4);
		internal const int onDefault1 = (1 << 5) | 1;
		/// <summary> miguel 2005 01 01 not used
		/// final static int setDefaultOn      = (1 << 25);
		/// </summary>
		
		// rasmol commands
		//UPGRADE_NOTE: Final was removed from the declaration of 'backbone '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int backbone = command | 0 | bool_Renamed | predefinedset;
		//UPGRADE_NOTE: Final was removed from the declaration of 'background '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int background = command | 1 | colorparam | setspecial;
		//UPGRADE_NOTE: Final was removed from the declaration of 'bond '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int bond = command | 2 | setparam | bool_Renamed;
		//UPGRADE_NOTE: Final was removed from the declaration of 'cartoon '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int cartoon = command | 3 | setparam;
		//UPGRADE_NOTE: Final was removed from the declaration of 'center '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int center = command | 4 | showparam | expressionCommand;
		//UPGRADE_NOTE: Final was removed from the declaration of 'clipboard '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int clipboard = command | 5;
		//UPGRADE_NOTE: Final was removed from the declaration of 'color '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int color = command | 6 | colorparam | setparam;
		//UPGRADE_NOTE: Final was removed from the declaration of 'connect '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int connect = command | 7 | embeddedExpressions;
		//UPGRADE_NOTE: Final was removed from the declaration of 'define '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int define = command | 9 | expressionCommand;
		//UPGRADE_NOTE: Final was removed from the declaration of 'dots '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int dots = command | 10 | bool_Renamed;
		//UPGRADE_NOTE: Final was removed from the declaration of 'echo '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int echo = command | 11 | setparam | specialstring;
		//UPGRADE_NOTE: Final was removed from the declaration of 'exit '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int exit = command | 12;
		//UPGRADE_NOTE: Final was removed from the declaration of 'hbond '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int hbond = command | 13 | setparam | bool_Renamed;
		//UPGRADE_NOTE: Final was removed from the declaration of 'help '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int help = command | 14;
		//UPGRADE_NOTE: Final was removed from the declaration of 'label '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int label = command | 15 | specialstring;
		//UPGRADE_NOTE: Final was removed from the declaration of 'load '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int load = command | 16 | specialstring;
		//UPGRADE_NOTE: Final was removed from the declaration of 'molecule '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int molecule = command | 17;
		//UPGRADE_NOTE: Final was removed from the declaration of 'monitor '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int monitor = command | 18 | setparam | bool_Renamed;
		//UPGRADE_NOTE: Final was removed from the declaration of 'pause '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int pause = command | 19;
		//UPGRADE_NOTE: Final was removed from the declaration of 'print '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int print = command | 20;
		//UPGRADE_NOTE: Final was removed from the declaration of 'quit '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int quit = command | 21;
		//UPGRADE_NOTE: Final was removed from the declaration of 'refresh '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int refresh = command | 22;
		//UPGRADE_NOTE: Final was removed from the declaration of 'renumber '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int renumber = command | 23 | negnums;
		//UPGRADE_NOTE: Final was removed from the declaration of 'reset '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int reset = command | 24;
		//UPGRADE_NOTE: Final was removed from the declaration of 'restrict '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int restrict = command | 25 | expressionCommand;
		//UPGRADE_NOTE: Final was removed from the declaration of 'ribbon '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int ribbon = command | 26 | bool_Renamed;
		//UPGRADE_NOTE: Final was removed from the declaration of 'rotate '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int rotate = command | 27 | bool_Renamed | negnums;
		//UPGRADE_NOTE: Final was removed from the declaration of 'save '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int save = command | 28;
		//UPGRADE_NOTE: Final was removed from the declaration of 'script '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int script = command | 29 | specialstring;
		//UPGRADE_NOTE: Final was removed from the declaration of 'select '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int select = command | 30 | expressionCommand;
		//UPGRADE_NOTE: Final was removed from the declaration of 'set '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int set_Renamed = command | 31 | bool_Renamed | negnums;
		//UPGRADE_NOTE: Final was removed from the declaration of 'show '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int show = command | 32;
		//UPGRADE_NOTE: Final was removed from the declaration of 'slab '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int slab = command | 33 | bool_Renamed;
		//UPGRADE_NOTE: Final was removed from the declaration of 'cpk '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int cpk = command | 35 | setparam | bool_Renamed | negnums;
		//UPGRADE_NOTE: Final was removed from the declaration of 'ssbond '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int ssbond = command | 36 | setparam | bool_Renamed;
		//UPGRADE_NOTE: Final was removed from the declaration of 'stereo '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int stereo = command | 38 | setspecial | bool_Renamed | negnums;
		//UPGRADE_NOTE: Final was removed from the declaration of 'strands '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int strands = command | 39 | setparam | bool_Renamed;
		//UPGRADE_NOTE: Final was removed from the declaration of 'structure '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int structure = command | 40;
		//UPGRADE_NOTE: Final was removed from the declaration of 'trace '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int trace = command | 41 | bool_Renamed;
		//UPGRADE_NOTE: Final was removed from the declaration of 'translate '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int translate = command | 42 | negnums;
		//UPGRADE_NOTE: Final was removed from the declaration of 'unbond '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int unbond = command | 43;
		//UPGRADE_NOTE: Final was removed from the declaration of 'wireframe '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int wireframe = command | 44 | bool_Renamed;
		//UPGRADE_NOTE: Final was removed from the declaration of 'write '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int write = command | 45 | setparam;
		//UPGRADE_NOTE: Final was removed from the declaration of 'zap '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int zap = command | 46;
		//UPGRADE_NOTE: Final was removed from the declaration of 'zoom '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int zoom = command | 47 | showparam | bool_Renamed;
		// openrasmol commands
		//UPGRADE_NOTE: Final was removed from the declaration of 'depth '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int depth = command | 50;
		//UPGRADE_NOTE: Final was removed from the declaration of 'star '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int star = command | 51;
		// chime commands
		//UPGRADE_NOTE: Final was removed from the declaration of 'delay '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int delay = command | 60;
		//UPGRADE_NOTE: Final was removed from the declaration of 'loop '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int loop = command | 61;
		//UPGRADE_NOTE: Final was removed from the declaration of 'move '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int move = command | 62 | negnums;
		//UPGRADE_NOTE: Final was removed from the declaration of 'view '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int view = command | 63;
		//UPGRADE_NOTE: Final was removed from the declaration of 'spin '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int spin = command | 64 | setparam | showparam | bool_Renamed;
		//UPGRADE_NOTE: Final was removed from the declaration of 'list '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int list = command | 65 | showparam;
		//UPGRADE_NOTE: Final was removed from the declaration of 'display3d '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int display3d = command | 66;
		//UPGRADE_NOTE: Final was removed from the declaration of 'animation '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int animation = command | 67;
		//UPGRADE_NOTE: Final was removed from the declaration of 'frame '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int frame = command | 68;
		// jmol commands
		//UPGRADE_NOTE: Final was removed from the declaration of 'font '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int font = command | 80;
		//UPGRADE_NOTE: Final was removed from the declaration of 'hover '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int hover = command | 81 | specialstring;
		//UPGRADE_NOTE: Final was removed from the declaration of 'vibration '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int vibration = command | 82;
		//UPGRADE_NOTE: Final was removed from the declaration of 'vector '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int vector = command | 83;
		//UPGRADE_NOTE: Final was removed from the declaration of 'meshRibbon '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int meshRibbon = command | 84;
		//UPGRADE_NOTE: Final was removed from the declaration of 'prueba '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int prueba = command | 85;
		//UPGRADE_NOTE: Final was removed from the declaration of 'rocket '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int rocket = command | 86;
		//UPGRADE_NOTE: Final was removed from the declaration of 'sasurface '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int sasurface = command | 87;
		//UPGRADE_NOTE: Final was removed from the declaration of 'moveto '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int moveto = command | 88 | negnums;
		//UPGRADE_NOTE: Final was removed from the declaration of 'console '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int console = command | 89;
		//UPGRADE_NOTE: Final was removed from the declaration of 'pmesh '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int pmesh = command | 90;
		//UPGRADE_NOTE: Final was removed from the declaration of 'polyhedra '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int polyhedra = command | 91 | embeddedExpressions;
		//UPGRADE_NOTE: Final was removed from the declaration of 'centerAt '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int centerAt = command | 92;
		//UPGRADE_NOTE: Final was removed from the declaration of 'isosurface '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int isosurface = command | 93;
		
		// parameters
		//UPGRADE_NOTE: Final was removed from the declaration of 'ambient '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int ambient = setparam | 0;
		//UPGRADE_NOTE: Final was removed from the declaration of 'axes '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int axes = setparam | 1;
		// background
		//UPGRADE_NOTE: Final was removed from the declaration of 'backfade '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int backfade = setparam | 2;
		//UPGRADE_NOTE: Final was removed from the declaration of 'bondmode '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int bondmode = setparam | 3;
		//UPGRADE_NOTE: Final was removed from the declaration of 'bonds '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int bonds = setparam | 4;
		//UPGRADE_NOTE: Final was removed from the declaration of 'boundbox '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int boundbox = setparam | 5 | showparam;
		// cartoon
		//UPGRADE_NOTE: Final was removed from the declaration of 'cisangle '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int cisangle = setparam | 6;
		//UPGRADE_NOTE: Final was removed from the declaration of 'display '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int display = setparam | 7;
		//UPGRADE_NOTE: Final was removed from the declaration of 'fontsize '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int fontsize = setparam | 8;
		//UPGRADE_NOTE: Final was removed from the declaration of 'fontstroke '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int fontstroke = setparam | 9;
		// hbonds
		// hetero
		//UPGRADE_NOTE: Final was removed from the declaration of 'hourglass '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int hourglass = setparam | 10;
		// hydrogen
		//UPGRADE_NOTE: Final was removed from the declaration of 'kinemage '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int kinemage = setparam | 11;
		//UPGRADE_NOTE: Final was removed from the declaration of 'menus '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int menus = setparam | 12;
		// monitor
		//UPGRADE_NOTE: Final was removed from the declaration of 'mouse '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int mouse = setparam | 13;
		//UPGRADE_NOTE: Final was removed from the declaration of 'picking '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int picking = setparam | 14;
		//  final static int radius       = setparam | 15 | atomproperty;
		//UPGRADE_NOTE: Final was removed from the declaration of 'shadow '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int shadow = setparam | 15;
		//UPGRADE_NOTE: Final was removed from the declaration of 'slabmode '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int slabmode = setparam | 16;
		// solvent
		//UPGRADE_NOTE: Final was removed from the declaration of 'specular '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int specular = setparam | 17;
		//UPGRADE_NOTE: Final was removed from the declaration of 'specpower '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int specpower = setparam | 18;
		// ssbonds
		// stereo
		// strands
		//UPGRADE_NOTE: Final was removed from the declaration of 'transparent '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int transparent = setparam | 19;
		//UPGRADE_NOTE: Final was removed from the declaration of 'unitcell '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int unitcell = setparam | 20;
		//UPGRADE_NOTE: Final was removed from the declaration of 'vectps '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int vectps = setparam | 21;
		// write
		
		// chime set parameters
		//UPGRADE_NOTE: Final was removed from the declaration of 'clear '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int clear = setparam | 22;
		//UPGRADE_NOTE: Final was removed from the declaration of 'gaussian '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int gaussian = setparam | 23;
		// load
		//UPGRADE_NOTE: Final was removed from the declaration of 'mep '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int mep = setparam | 24;
		//UPGRADE_NOTE: Final was removed from the declaration of 'mlp '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int mlp = setparam | 25 | showparam;
		//UPGRADE_NOTE: Final was removed from the declaration of 'molsurface '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int molsurface = setparam | 26;
		//UPGRADE_NOTE: Final was removed from the declaration of 'debugscript '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int debugscript = setparam | 27;
		//UPGRADE_NOTE: Final was removed from the declaration of 'scale3d '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int scale3d = setparam | 28;
		// jmol extensions
		//UPGRADE_NOTE: Final was removed from the declaration of 'property '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int property = setparam | 29;
		//UPGRADE_NOTE: Final was removed from the declaration of 'diffuse '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int diffuse = setparam | 30;
		//UPGRADE_NOTE: Final was removed from the declaration of 'labeloffset '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int labeloffset = setparam | 31;
		//UPGRADE_NOTE: Final was removed from the declaration of 'frank '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int frank = setparam | 32;
		//UPGRADE_NOTE: Final was removed from the declaration of 'formalCharge '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int formalCharge = setparam | 33;
		//UPGRADE_NOTE: Final was removed from the declaration of 'partialCharge '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int partialCharge = setparam | 34;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'information '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int information = showparam | 0;
		//UPGRADE_NOTE: Final was removed from the declaration of 'phipsi '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int phipsi = showparam | 1;
		// center centre
		//UPGRADE_NOTE: Final was removed from the declaration of 'ramprint '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int ramprint = showparam | 2;
		//UPGRADE_NOTE: Final was removed from the declaration of 'rotation '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int rotation = showparam | 3;
		// selected
		//UPGRADE_NOTE: Final was removed from the declaration of 'group '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int group = showparam | 4 | expression;
		//UPGRADE_NOTE: Final was removed from the declaration of 'chain '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int chain = showparam | 5 | expression;
		//UPGRADE_NOTE: Final was removed from the declaration of 'atom '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int atom = showparam | 6;
		//UPGRADE_NOTE: Final was removed from the declaration of 'sequence '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int sequence = showparam | 7;
		//UPGRADE_NOTE: Final was removed from the declaration of 'symmetry '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int symmetry = showparam | 8;
		//UPGRADE_NOTE: Final was removed from the declaration of 'translation '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int translation = showparam | 9;
		// zoom
		// chime show parameters
		//UPGRADE_NOTE: Final was removed from the declaration of 'residue '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int residue = showparam | 10;
		// model
		// mlp
		// list
		// spin
		//UPGRADE_NOTE: Final was removed from the declaration of 'all '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int all = showparam | 11 | expression;
		//UPGRADE_NOTE: Final was removed from the declaration of 'pdbheader '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int pdbheader = showparam | 12 | expression;
		//UPGRADE_NOTE: Final was removed from the declaration of 'axisangle '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int axisangle = showparam | 13;
		//UPGRADE_NOTE: Final was removed from the declaration of 'transform '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int transform = showparam | 14;
		//UPGRADE_NOTE: Final was removed from the declaration of 'orientation '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int orientation = showparam | 15;
		//UPGRADE_NOTE: Final was removed from the declaration of 'file '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int file = showparam | 16;
		
		// atom expression operators
		//UPGRADE_NOTE: Final was removed from the declaration of 'leftparen '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int leftparen = expression | 0;
		//UPGRADE_NOTE: Final was removed from the declaration of 'rightparen '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int rightparen = expression | 1;
		//UPGRADE_NOTE: Final was removed from the declaration of 'hyphen '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int hyphen = expression | 2;
		//UPGRADE_NOTE: Final was removed from the declaration of 'opAnd '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int opAnd = expression | 3;
		//UPGRADE_NOTE: Final was removed from the declaration of 'opOr '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int opOr = expression | 4;
		//UPGRADE_NOTE: Final was removed from the declaration of 'opNot '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int opNot = expression | 5;
		//UPGRADE_NOTE: Final was removed from the declaration of 'within '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int within = expression | 6;
		//UPGRADE_NOTE: Final was removed from the declaration of 'plus '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int plus = expression | 7;
		//UPGRADE_NOTE: Final was removed from the declaration of 'pick '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int pick = expression | 8;
		//UPGRADE_NOTE: Final was removed from the declaration of 'asterisk '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int asterisk = expression | 9;
		//UPGRADE_NOTE: Final was removed from the declaration of 'dot '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int dot = expression | 11;
		//UPGRADE_NOTE: Final was removed from the declaration of 'leftsquare '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int leftsquare = expression | 12;
		//UPGRADE_NOTE: Final was removed from the declaration of 'rightsquare '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int rightsquare = expression | 13;
		//UPGRADE_NOTE: Final was removed from the declaration of 'colon '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int colon = expression | 14;
		//UPGRADE_NOTE: Final was removed from the declaration of 'slash '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int slash = expression | 15;
		//UPGRADE_NOTE: Final was removed from the declaration of 'substructure '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int substructure = expression | 16;
		//UPGRADE_NOTE: Final was removed from the declaration of 'connected '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int connected = expression | 17;
		
		// miguel 2005 01 01
		// these are used to demark the beginning and end of expressions
		// they do not exist in the source code, but are emitted by the
		// expression compiler
		//UPGRADE_NOTE: Final was removed from the declaration of 'expressionBegin '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int expressionBegin = expression | 100;
		//UPGRADE_NOTE: Final was removed from the declaration of 'expressionEnd '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int expressionEnd = expression | 101;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'atomno '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int atomno = atomproperty | 0;
		//UPGRADE_NOTE: Final was removed from the declaration of 'elemno '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int elemno = atomproperty | 1;
		//UPGRADE_NOTE: Final was removed from the declaration of 'resno '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int resno = atomproperty | 2;
		//UPGRADE_NOTE: Final was removed from the declaration of 'radius '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int radius = atomproperty | 3 | setparam;
		//UPGRADE_NOTE: Final was removed from the declaration of 'temperature '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int temperature = atomproperty | 4;
		//UPGRADE_NOTE: Final was removed from the declaration of 'model '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int model = atomproperty | 5 | showparam | expression | command;
		//UPGRADE_NOTE: Final was removed from the declaration of 'bondcount '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int bondcount = atomproperty | 6;
		//UPGRADE_NOTE: Final was removed from the declaration of '_groupID '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int _groupID = atomproperty | 7;
		//UPGRADE_NOTE: Final was removed from the declaration of '_atomID '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int _atomID = atomproperty | 8;
		//UPGRADE_NOTE: Final was removed from the declaration of '_structure '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int _structure = atomproperty | 9;
		//UPGRADE_NOTE: Final was removed from the declaration of 'occupancy '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int occupancy = atomproperty | 10;
		//UPGRADE_NOTE: Final was removed from the declaration of 'polymerLength '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int polymerLength = atomproperty | 11;
		//UPGRADE_NOTE: Final was removed from the declaration of 'hbondcount '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int hbondcount = atomproperty | 12;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'opGT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int opGT = comparator | 0;
		//UPGRADE_NOTE: Final was removed from the declaration of 'opGE '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int opGE = comparator | 1;
		//UPGRADE_NOTE: Final was removed from the declaration of 'opLE '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int opLE = comparator | 2;
		//UPGRADE_NOTE: Final was removed from the declaration of 'opLT '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int opLT = comparator | 3;
		//UPGRADE_NOTE: Final was removed from the declaration of 'opEQ '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int opEQ = comparator | 4;
		//UPGRADE_NOTE: Final was removed from the declaration of 'opNE '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int opNE = comparator | 5;
		
		// misc
		//UPGRADE_NOTE: Final was removed from the declaration of 'off '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int off = bool_Renamed | 0;
		//UPGRADE_NOTE: Final was removed from the declaration of 'on '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int on = bool_Renamed | 1;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'dash '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int dash = misc | 0; //backbone
		//UPGRADE_NOTE: Final was removed from the declaration of 'user '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int user = misc | 1; //cpk & star
		//UPGRADE_NOTE: Final was removed from the declaration of 'x '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int x = misc | 2 | expression;
		//UPGRADE_NOTE: Final was removed from the declaration of 'y '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int y = misc | 3 | expression | predefinedset;
		//UPGRADE_NOTE: Final was removed from the declaration of 'z '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int z = misc | 4 | expression;
		//UPGRADE_NOTE: Final was removed from the declaration of 'none '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int none = misc | 5 | expression;
		//UPGRADE_NOTE: Final was removed from the declaration of 'normal '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int normal = misc | 7;
		//UPGRADE_NOTE: Final was removed from the declaration of 'rasmol '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int rasmol = misc | 8;
		//UPGRADE_NOTE: Final was removed from the declaration of 'insight '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int insight = misc | 9;
		//UPGRADE_NOTE: Final was removed from the declaration of 'quanta '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int quanta = misc | 10;
		//UPGRADE_NOTE: Final was removed from the declaration of 'ident '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int ident = misc | 11;
		//UPGRADE_NOTE: Final was removed from the declaration of 'distance '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int distance = misc | 12;
		//UPGRADE_NOTE: Final was removed from the declaration of 'angle '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int angle = misc | 13;
		//UPGRADE_NOTE: Final was removed from the declaration of 'torsion '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int torsion = misc | 14;
		//UPGRADE_NOTE: Final was removed from the declaration of 'coord '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int coord = misc | 15;
		//UPGRADE_NOTE: Final was removed from the declaration of 'shapely '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int shapely = misc | 18;
		//UPGRADE_NOTE: Final was removed from the declaration of 'restore '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int restore = misc | 19; // chime extended
		//UPGRADE_NOTE: Final was removed from the declaration of 'colorRGB '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int colorRGB = misc | 20 | colorparam;
		//UPGRADE_NOTE: Final was removed from the declaration of 'spec_resid '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int spec_resid = misc | 21;
		//UPGRADE_NOTE: Final was removed from the declaration of 'spec_name_pattern '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int spec_name_pattern = misc | 22;
		//UPGRADE_NOTE: Final was removed from the declaration of 'spec_seqcode '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int spec_seqcode = misc | 23;
		//UPGRADE_NOTE: Final was removed from the declaration of 'spec_seqcode_range '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int spec_seqcode_range = misc | 24;
		//UPGRADE_NOTE: Final was removed from the declaration of 'spec_chain '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int spec_chain = misc | 25;
		//UPGRADE_NOTE: Final was removed from the declaration of 'spec_alternate '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int spec_alternate = misc | 26;
		//UPGRADE_NOTE: Final was removed from the declaration of 'spec_model '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int spec_model = misc | 27;
		//UPGRADE_NOTE: Final was removed from the declaration of 'spec_atom '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int spec_atom = misc | 28;
		//UPGRADE_NOTE: Final was removed from the declaration of 'percent '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int percent = misc | expression | 29;
		//UPGRADE_NOTE: Final was removed from the declaration of 'dotted '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int dotted = misc | 30;
		//UPGRADE_NOTE: Final was removed from the declaration of 'mode '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int mode = misc | 31;
		//UPGRADE_NOTE: Final was removed from the declaration of 'direction '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int direction = misc | 32;
		//UPGRADE_NOTE: Final was removed from the declaration of 'fps '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int fps = misc | 33;
		//UPGRADE_NOTE: Final was removed from the declaration of 'displacement '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int displacement = misc | 34;
		//UPGRADE_NOTE: Final was removed from the declaration of 'type '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int type = misc | 35;
		//UPGRADE_NOTE: Final was removed from the declaration of 'fixedtemp '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int fixedtemp = misc | 36;
		//UPGRADE_NOTE: Final was removed from the declaration of 'rubberband '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int rubberband = misc | 37;
		//UPGRADE_NOTE: Final was removed from the declaration of 'monomer '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int monomer = misc | 38;
		//UPGRADE_NOTE: Final was removed from the declaration of 'defaultColors '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int defaultColors = misc | 39 | setparam;
		//UPGRADE_NOTE: Final was removed from the declaration of 'opaque '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int opaque = misc | 40;
		//UPGRADE_NOTE: Final was removed from the declaration of 'translucent '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int translucent = misc | 41;
		//UPGRADE_NOTE: Final was removed from the declaration of 'delete '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int delete = misc | 42;
		//UPGRADE_NOTE: Final was removed from the declaration of 'edges '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int edges = misc | 43;
		//UPGRADE_NOTE: Final was removed from the declaration of 'noedges '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int noedges = misc | 44;
		//UPGRADE_NOTE: Final was removed from the declaration of 'frontedges '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int frontedges = misc | 45;
		//UPGRADE_NOTE: Final was removed from the declaration of 'solid '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int solid = misc | 45;
		//UPGRADE_NOTE: Final was removed from the declaration of 'jmol '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int jmol = misc | 46;
		//UPGRADE_NOTE: Final was removed from the declaration of 'absolute '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int absolute = misc | 47;
		//UPGRADE_NOTE: Final was removed from the declaration of 'average '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int average = misc | 48;
		//UPGRADE_NOTE: Final was removed from the declaration of 'nodots '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int nodots = misc | 49;
		//UPGRADE_NOTE: Final was removed from the declaration of 'mesh '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int mesh = misc | 50;
		//UPGRADE_NOTE: Final was removed from the declaration of 'nomesh '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int nomesh = misc | 51;
		//UPGRADE_NOTE: Final was removed from the declaration of 'fill '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int fill = misc | 52;
		//UPGRADE_NOTE: Final was removed from the declaration of 'nofill '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int nofill = misc | 53;
		//UPGRADE_NOTE: Final was removed from the declaration of 'vanderwaals '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int vanderwaals = misc | 54;
		//UPGRADE_NOTE: Final was removed from the declaration of 'ionic '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int ionic = misc | 55;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'amino '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int amino = predefinedset | 0;
		//UPGRADE_NOTE: Final was removed from the declaration of 'hetero '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int hetero = predefinedset | 1 | setparam;
		//UPGRADE_NOTE: Final was removed from the declaration of 'hydrogen '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int hydrogen = predefinedset | 2 | setparam;
		//UPGRADE_NOTE: Final was removed from the declaration of 'selected '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int selected = predefinedset | 3 | showparam;
		//UPGRADE_NOTE: Final was removed from the declaration of 'solvent '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int solvent = predefinedset | 4 | setparam;
		//UPGRADE_NOTE: Final was removed from the declaration of 'sidechain '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int sidechain = predefinedset | 5;
		//UPGRADE_NOTE: Final was removed from the declaration of 'protein '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int protein = predefinedset | 6;
		//UPGRADE_NOTE: Final was removed from the declaration of 'nucleic '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int nucleic = predefinedset | 7;
		//UPGRADE_NOTE: Final was removed from the declaration of 'dna '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int dna = predefinedset | 8;
		//UPGRADE_NOTE: Final was removed from the declaration of 'rna '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int rna = predefinedset | 9;
		//UPGRADE_NOTE: Final was removed from the declaration of 'purine '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int purine = predefinedset | 10;
		//UPGRADE_NOTE: Final was removed from the declaration of 'pyrimidine '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int pyrimidine = predefinedset | 11;
		//UPGRADE_NOTE: Final was removed from the declaration of 'surface '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int surface = predefinedset | 12;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'tokenOn '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly Token tokenOn = new Token(on, 1, "on");
		//UPGRADE_NOTE: Final was removed from the declaration of 'tokenAll '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly Token tokenAll = new Token(all, "all");
		//UPGRADE_NOTE: Final was removed from the declaration of 'tokenAnd '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly Token tokenAnd = new Token(opAnd, "and");
		//UPGRADE_NOTE: Final was removed from the declaration of 'tokenElemno '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly Token tokenElemno = new Token(elemno, "elemno");
		//UPGRADE_NOTE: Final was removed from the declaration of 'tokenExpressionBegin '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly Token tokenExpressionBegin = new Token(expressionBegin, "expressionBegin");
		//UPGRADE_NOTE: Final was removed from the declaration of 'tokenExpressionEnd '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly Token tokenExpressionEnd = new Token(expressionEnd, "expressionEnd");
		
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'comparatorNames'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] comparatorNames = new System.String[]{">", ">=", "<=", "<", "=", "!="};
		//UPGRADE_NOTE: Final was removed from the declaration of 'atomPropertyNames'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] atomPropertyNames = new System.String[]{"atomno", "elemno", "resno", "radius", "temperature", "model", "bondcount", "_groupID", "_atomID", "_structure"};
		
		/*
		Note that the RasMol scripting language is case-insensitive.
		So, the compiler turns all identifiers to lower-case before
		looking up in the hash table. 
		Therefore, the left column of this array *must* be lower-case
		*/
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'arrayPairs '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.Object[] arrayPairs = new System.Object[]{"backbone", new Token(backbone, onDefault1, "backbone"), "background", new Token(background, varArgCount, "background"), "bond", new Token(bond, varArgCount, "bond"), "cartoon", new Token(cartoon, onDefault1, "cartoon"), "cartoons", null, "center", new Token(center, varArgCount, "center"), "centre", null, "clipboard", new Token(clipboard, 0, "clipboard"), "color", new Token(color, varArgCount, "color"), "colour", null, "connect", new Token(connect, varArgCount, "connect"), "define", new Token(define, varArgCount, "define"), "@", null, "dots", new Token(dots, onDefault1, "dots"), "echo", new Token(echo, varArgCount, "echo"), "exit", new Token(exit, 0, "exit"), "hbond", new Token(hbond, onDefault1, "hbond"), "hbonds", null, "help", new Token(help, varArgCount, "help"), "label", new Token(label, onDefault1, "label"), "labels", null, "load", new Token(load, varArgCount, "load"), "molecule", new Token(molecule, 1, "molecule"), "monitor", new Token(monitor, varArgCount, "monitor"), "monitors", null, "measure", null, "measures", null, "measurement", null, "measurements", null, "pause", new Token(pause, 0, "pause"), "wait", null, "print", new Token(print, 0, "print"), "quit", new Token(quit, 0, "quit"), "refresh", new Token(refresh, 0, "refresh"), "renumber", new Token(renumber, onDefault1, "renumber"), "reset", new Token(reset, 0, "reset"), "restrict", new Token(restrict, varArgCount, "restrict"), "ribbon", new Token(ribbon, onDefault1, "ribbon"), "ribbons", null, "rotate", new Token(rotate, varArgCount, "rotate"), "save", new Token(save, varArgCount, "save"), "script", new Token(script, 1, "script"), "source", null, "select", new Token(select, varArgCount, "select"), "set", new Token(set_Renamed, varArgCount, "set"), "show", new Token(show, varArgCount, "show"), "slab", new Token(slab, onDefault1, "slab"), "cpk", new Token(cpk, varArgCount, "cpk"), "spacefill", null, "ssbond", new Token(ssbond, onDefault1, "ssbond"), "ssbonds", null, 
			"stereo", new Token(stereo, varArgCount, "stereo"), "strands", new Token(strands, onDefault1, "strands"), "structure", new Token(structure, 0, "structure"), "trace", new Token(trace, onDefault1, "trace"), "translate", new Token(translate, varArgCount, "translate"), "unbond", new Token(unbond, varArgCount, "unbond"), "wireframe", new Token(wireframe, onDefault1, "wireframe"), "write", new Token(write, varArgCount, "write"), "zap", new Token(zap, 0, "zap"), "zoom", new Token(zoom, onDefault1, "zoom"), "depth", new Token(depth, 1, "depth"), "star", new Token(star, varArgCount, "star"), "stars", null, "delay", new Token(delay, onDefault1, "delay"), "loop", new Token(loop, onDefault1, "loop"), "move", new Token(move, varArgCount, "move"), "view", new Token(view, varArgCount, "view"), "spin", new Token(spin, onDefault1, "spin"), "list", new Token(list, varArgCount, "list"), "display3d", new Token(display3d, "display3d"), "animation", new Token(animation, "animation"), "anim", null, "frame", new Token(frame, "frame"), "font", new Token(font, "font"), "hover", new Token(hover, "hover"), "vibration", new Token(vibration, "vibration"), "vector", new Token(vector, varArgCount, "vector"), "vectors", null, "meshribbon", new Token(meshRibbon, onDefault1, "meshribbon"), "meshribbons", null, "prueba", new Token(prueba, onDefault1, "prueba"), "rocket", new Token(rocket, onDefault1, "rocket"), "rockets", null, "sasurface", new Token(sasurface, varArgCount, "sasurface"), "moveto", new Token(moveto, varArgCount, "moveto"), "console", new Token(console, onDefault1, "console"), "pmesh", new Token(pmesh, varArgCount, "pmesh"), "polyhedra", new Token(polyhedra, varArgCount, "polyhedra"), "centerat", new Token(centerAt, varArgCount, "centerat"), "isosurface", new Token(isosurface, varArgCount, "isosurface"), "ambient", new Token(ambient, "ambient"), "axes", new Token(axes, "axes"), "backfade", new Token(backfade, "backfade"), "bondmode", new Token(bondmode, "bondmode"), "bonds", new Token(bonds, "bonds"), "boundbox", 
			new Token(boundbox, "boundbox"), "cisangle", new Token(cisangle, "cisangle"), "display", new Token(display, "display"), "fontsize", new Token(fontsize, "fontsize"), "fontstroke", new Token(fontstroke, "fontstroke"), "hourglass", new Token(hourglass, "hourglass"), "kinemage", new Token(kinemage, "kinemage"), "menus", new Token(menus, "menus"), "mouse", new Token(mouse, "mouse"), "picking", new Token(picking, "picking"), "radius", new Token(radius, "radius"), "shadow", new Token(shadow, "shadow"), "slabmode", new Token(slabmode, "slabmode"), "specular", new Token(specular, "specular"), "specpower", new Token(specpower, "specpower"), "transparent", new Token(transparent, "transparent"), "unitcell", new Token(unitcell, "unitcell"), "vectps", new Token(vectps, "vectps"), "clear", new Token(clear, "clear"), "gaussian", new Token(gaussian, "gaussian"), "mep", new Token(mep, "mep"), "mlp", new Token(mlp, "mlp"), "molsurface", new Token(molsurface, "molsurface"), "debugscript", new Token(debugscript, "debugscript"), "fps", new Token(fps, "fps"), "scale3d", new Token(scale3d, "scale3d"), "property", new Token(property, "property"), "diffuse", new Token(diffuse, "diffuse"), "labeloffset", new Token(labeloffset, "labeloffset"), "frank", new Token(frank, "frank"), "formalcharge", new Token(formalCharge, "formalcharge"), "charge", null, "partialcharge", new Token(partialCharge, "partialcharge"), "information", new Token(information, "information"), "info", null, "phipsi", new Token(phipsi, "phipsi"), "ramprint", new Token(ramprint, "ramprint"), "rotation", new Token(rotation, "rotation"), "group", new Token(group, "group"), "chain", new Token(chain, "chain"), "atom", new Token(atom, "atom"), "atoms", null, "sequence", new Token(sequence, "sequence"), "symmetry", new Token(symmetry, "symmetry"), "translation", new Token(translation, "translation"), "residue", new Token(residue, "residue"), "model", new Token(model, "model"), "models", null, "pdbheader", new Token(pdbheader, "pdbheader"), "axisangle", new Token(
			axisangle, "axisangle"), "transform", new Token(transform, "transform"), "orientation", new Token(orientation, "orientation"), "file", new Token(file, "file"), "(", new Token(leftparen, "("), ")", new Token(rightparen, ")"), "-", new Token(hyphen, "-"), "and", tokenAnd, "&", null, "&&", null, "or", new Token(opOr, "or"), ",", null, "|", null, "||", null, "not", new Token(opNot, "not"), "!", null, "<", new Token(opLT, "<"), "<=", new Token(opLE, "<="), ">=", new Token(opGE, ">="), ">", new Token(opGT, ">="), "==", new Token(opEQ, "=="), "=", null, "!=", new Token(opNE, "!="), "<>", null, "/=", null, "within", new Token(within, "within"), "+", new Token(plus, "+"), "pick", new Token(pick, "pick"), ".", new Token(dot, "."), "[", new Token(leftsquare, "["), "]", new Token(rightsquare, "]"), ":", new Token(colon, ":"), "/", new Token(slash, "/"), "substructure", new Token(substructure, "substructure"), "connected", new Token(connected, "connected"), "atomno", new Token(atomno, "atomno"), "elemno", tokenElemno, "_e", tokenElemno, "resno", new Token(resno, "resno"), "temperature", new Token(temperature, "temperature"), "relativetemperature", null, "bondcount", new Token(bondcount, "bondcount"), "hbondcount", new Token(hbondcount, "hbondcount"), "_groupID", new Token(_groupID, "_groupID"), "_g", null, "_atomID", new Token(_atomID, "_atomID"), "_a", null, "_structure", new Token(_structure, "_structure"), "occupancy", new Token(occupancy, "occupancy"), "polymerlength", new Token(polymerLength, "polymerlength"), "off", new Token(off, 0, "off"), "false", null, "on", tokenOn, "true", null, "dash", new Token(dash, "dash"), "user", new Token(user, "user"), "x", new Token(x, "x"), "y", new Token(y, "y"), "z", new Token(z, "z"), "*", new Token(asterisk, "*"), "all", tokenAll, "none", new Token(none, "none"), "null", null, "inherit", null, "normal", new Token(normal, "normal"), "rasmol", new Token(rasmol, "rasmol"), "insight", new Token(insight, "insight"), "quanta", new Token(quanta, "quanta"), "ident", new 
			Token(ident, "ident"), "distance", new Token(distance, "distance"), "angle", new Token(angle, "angle"), "torsion", new Token(torsion, "torsion"), "coord", new Token(coord, "coord"), "shapely", new Token(shapely, "shapely"), "restore", new Token(restore, "restore"), "amino", new Token(amino, "amino"), "hetero", new Token(hetero, "hetero"), "hydrogen", new Token(hydrogen, "hydrogen"), "hydrogens", null, "selected", new Token(selected, "selected"), "solvent", new Token(solvent, "solvent"), "%", new Token(percent, "%"), "dotted", new Token(dotted, "dotted"), "sidechain", new Token(sidechain, "sidechain"), "protein", new Token(protein, "protein"), "nucleic", new Token(nucleic, "nucleic"), "dna", new Token(dna, "dna"), "rna", new Token(rna, "rna"), "purine", new Token(purine, "purine"), "pyrimidine", new Token(pyrimidine, "pyrimidine"), "surface", new Token(surface, "surface"), "mode", new Token(mode, "mode"), "direction", new Token(direction, "direction"), "jmol", new Token(jmol, "jmol"), "displacement", new Token(displacement, "displacement"), "type", new Token(type, "type"), "fixedtemperature", new Token(fixedtemp, "fixedtemperature"), "rubberband", new Token(rubberband, "rubberband"), "monomer", new Token(monomer, "monomer"), "defaultcolors", new Token(defaultColors, "defaultColors"), "opaque", new Token(opaque, "opaque"), "translucent", new Token(translucent, "translucent"), "delete", new Token(delete, "delete"), "edges", new Token(edges, "edges"), "noedges", new Token(noedges, "noedges"), "frontedges", new Token(frontedges, "frontedges"), "solid", new Token(solid, "solid"), "absolute", new Token(absolute, "absolute"), "average", new Token(average, "average"), "nodots", new Token(nodots, "nodots"), "mesh", new Token(mesh, "mesh"), "nomesh", new Token(nomesh, "nomesh"), "fill", new Token(fill, "fill"), "nofill", new Token(nofill, "nofill"), "vanderwaals", new Token(vanderwaals, "vanderwaals"), "ionic", new Token(ionic, "ionic")};
		
		internal static System.Collections.Hashtable map = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
		
		public override System.String ToString()
		{
			//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			return "Token[" + astrType[tok <= keyword?tok:keyword] + "-" + tok + ((intValue == System.Int32.MaxValue)?"":":" + intValue) + ((value_Renamed == null)?"":":" + value_Renamed) + "]";
		}
		static Token()
		{
			{
				Token tokenLast = null;
				System.String stringThis;
				Token tokenThis;
				for (int i = 0; i + 1 < arrayPairs.Length; i += 2)
				{
					stringThis = ((System.String) arrayPairs[i]);
					tokenThis = (Token) arrayPairs[i + 1];
					if (tokenThis == null)
						tokenThis = tokenLast;
					if (map[stringThis] != null)
						System.Console.Out.WriteLine("duplicate token definition:" + stringThis);
					map[stringThis] = tokenThis;
					tokenLast = tokenThis;
				}
			}
		}
	}
}