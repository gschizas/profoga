/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-04-04 21:56:22 +0200 (mar., 04 avr. 2006) $
* $Revision: 4909 $
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
using Graphics3D = org.jmol.g3d.Graphics3D;
namespace org.jmol.viewer
{
	
	class Compiler
	{
		virtual internal short[] LineNumbers
		{
			get
			{
				return lineNumbers;
			}
			
		}
		virtual internal short[] LineIndices
		{
			get
			{
				return lineIndices;
			}
			
		}
		virtual internal Token[][] AatokenCompiled
		{
			get
			{
				return aatokenCompiled;
			}
			
		}
		virtual internal System.String ErrorMessage
		{
			get
			{
				System.String strError = errorMessage;
				strError += (" : " + errorLine + "\n");
				if (filename != null)
					strError += filename;
				strError += (" line#" + lineCurrent);
				return strError;
			}
			
		}
		virtual internal System.String UnescapedStringLiteral
		{
			get
			{
				System.Text.StringBuilder sb = new System.Text.StringBuilder(cchToken - 2);
				int ichMax = ichToken + cchToken - 1;
				int ich = ichToken + 1;
				while (ich < ichMax)
				{
					char ch = script[ich++];
					if (ch == '\\' && ich < ichMax)
					{
						ch = script[ich++];
						switch (ch)
						{
							
							case 'b': 
								ch = '\b';
								break;
							
							case 'n': 
								ch = '\n';
								break;
							
							case 't': 
								ch = '\t';
								break;
							
							case 'r': 
								ch = '\r';
								// fall into
								goto case '"';
							
							case '"': 
							case '\\': 
							case '\'': 
								break;
							
							case 'x': 
							case 'u': 
								int digitCount = ch == 'x'?2:4;
								if (ich < ichMax)
								{
									int unicode = 0;
									for (int k = digitCount; --k >= 0 && ich < ichMax; )
									{
										char chT = script[ich];
										int hexit = getHexitValue(chT);
										if (hexit < 0)
											break;
										unicode <<= 4;
										unicode += hexit;
										++ich;
									}
									ch = (char) unicode;
								}
								break;
							}
					}
					sb.Append(ch);
				}
				return "" + sb;
			}
			
		}
		
		internal System.String filename;
		internal System.String script;
		
		internal short[] lineNumbers;
		internal short[] lineIndices;
		internal Token[][] aatokenCompiled;
		
		internal bool error;
		internal System.String errorMessage;
		internal System.String errorLine;
		
		internal const bool logMessages = false;
		
		private void  log(System.String message)
		{
			if (logMessages)
				System.Console.Out.WriteLine(message);
		}
		
		internal virtual bool compile(System.String filename, System.String script)
		{
			this.filename = filename;
			this.script = script;
			lineNumbers = lineIndices = null;
			aatokenCompiled = null;
			errorMessage = errorLine = null;
			if (compile0())
				return true;
			int icharEnd;
			//UPGRADE_WARNING: Method 'java.lang.String.indexOf' was converted to 'System.String.IndexOf' which may throw an exception. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1101'"
			if ((icharEnd = script.IndexOf('\r', ichCurrentCommand)) == - 1 && (icharEnd = script.IndexOf('\n', ichCurrentCommand)) == - 1)
				icharEnd = script.Length;
			errorLine = script.Substring(ichCurrentCommand, (icharEnd) - (ichCurrentCommand));
			return false;
		}
		
		internal int cchScript;
		internal short lineCurrent;
		
		internal int ichToken;
		internal int cchToken;
		internal Token[] atokenCommand;
		
		internal int ichCurrentCommand;
		
		internal virtual bool compile0()
		{
			cchScript = script.Length;
			ichToken = 0;
			lineCurrent = 1;
			int lnLength = 8;
			lineNumbers = new short[lnLength];
			lineIndices = new short[lnLength];
			error = false;
			
			System.Collections.ArrayList lltoken = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			System.Collections.ArrayList ltoken = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			//Token tokenCommand = null;
			int tokCommand = Token.nada;
			
			for (; true; ichToken += cchToken)
			{
				if (lookingAtLeadingWhitespace())
					continue;
				if (lookingAtComment())
					continue;
				bool endOfLine = lookingAtEndOfLine();
				if (endOfLine || lookingAtEndOfStatement())
				{
					if (tokCommand != Token.nada)
					{
						if (!compileCommand(ltoken))
							return false;
						lltoken.Add(atokenCommand);
						int iCommand = lltoken.Count;
						if (iCommand == lnLength)
						{
							short[] lnT = new short[lnLength * 2];
							Array.Copy(lineNumbers, 0, lnT, 0, lnLength);
							lineNumbers = lnT;
							lnT = new short[lnLength * 2];
							Array.Copy(lineIndices, 0, lnT, 0, lnLength);
							lineIndices = lnT;
							lnLength *= 2;
						}
						lineNumbers[iCommand] = lineCurrent;
						lineIndices[iCommand] = (short) ichCurrentCommand;
						SupportClass.SetCapacity(ltoken, 0);
						tokCommand = Token.nada;
					}
					if (ichToken < cchScript)
					{
						if (endOfLine)
							++lineCurrent;
						continue;
					}
					break;
				}
				if (tokCommand != Token.nada)
				{
					if (lookingAtString())
					{
						System.String str = UnescapedStringLiteral;
						ltoken.Add(new Token(Token.string_Renamed, str));
						continue;
					}
					if (tokCommand == Token.load && lookingAtLoadFormat())
					{
						System.String strFormat = script.Substring(ichToken, (ichToken + cchToken) - (ichToken));
						strFormat = strFormat.ToLower();
						ltoken.Add(new Token(Token.identifier, strFormat));
						continue;
					}
					if ((tokCommand & Token.specialstring) != 0 && lookingAtSpecialString())
					{
						System.String str = script.Substring(ichToken, (ichToken + cchToken) - (ichToken));
						ltoken.Add(new Token(Token.string_Renamed, str));
						continue;
					}
					if (lookingAtDecimal((tokCommand & Token.negnums) != 0))
					{
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						float value_Renamed = (float) System.Single.Parse(script.Substring(ichToken, (ichToken + cchToken) - (ichToken)));
						ltoken.Add(new Token(Token.decimal_Renamed, (System.Object) value_Renamed));
						continue;
					}
					if (lookingAtSeqcode())
					{
						int seqNum = System.Int32.Parse(script.Substring(ichToken, (ichToken + cchToken - 2) - (ichToken)));
						char insertionCode = script[ichToken + cchToken - 1];
						int seqcode = Group.getSeqcode(seqNum, insertionCode);
						ltoken.Add(new Token(Token.seqcode, seqcode, "seqcode"));
						continue;
					}
					if (lookingAtInteger((tokCommand & Token.negnums) != 0))
					{
						System.String intString = script.Substring(ichToken, (ichToken + cchToken) - (ichToken));
						int val = System.Int32.Parse(intString);
						ltoken.Add(new Token(Token.integer, val, intString));
						continue;
					}
				}
				if (lookingAtLookupToken())
				{
					System.String ident = script.Substring(ichToken, (ichToken + cchToken) - (ichToken));
					Token token;
					// hack to support case sensitive alternate locations and chains
					// if an identifier is a single character long, then
					// allocate a new Token with the original character preserved
					if (ident.Length == 1)
					{
						token = (Token) Token.map[ident];
						if (token == null)
						{
							System.String lowerCaseIdent = ident.ToLower();
							token = (Token) Token.map[lowerCaseIdent];
							if (token != null)
								token = new Token(token.tok, token.intValue, ident);
						}
					}
					else
					{
						ident = ident.ToLower();
						token = (Token) Token.map[ident];
					}
					if (token == null)
						token = new Token(Token.identifier, ident);
					int tok = token.tok;
					switch (tokCommand)
					{
						
						case Token.nada: 
							ichCurrentCommand = ichToken;
							//tokenCommand = token;
							tokCommand = tok;
							if ((tokCommand & Token.command) == 0)
								return commandExpected();
							break;
						
						case Token.set_Renamed: 
							if (ltoken.Count == 1)
							{
								if ((tok & Token.setspecial) != 0)
								{
									//tokenCommand = token;
									tokCommand = tok;
									ltoken.Clear();
									break;
								}
								if ((tok & Token.setparam) == 0 && tok != Token.identifier)
									return cannotSet(ident);
							}
							break;
						
						case Token.show: 
							if ((tok & Token.showparam) == 0)
								return cannotShow(ident);
							break;
						
						case Token.define: 
							if (ltoken.Count == 1)
							{
								// we are looking at the variable name
								if (tok != Token.identifier && (tok & Token.predefinedset) != Token.predefinedset)
									return invalidExpressionToken(ident);
							}
							else
							{
								// we are looking at the expression
								if (tok != Token.identifier && tok != Token.set_Renamed && (tok & (Token.expression | Token.predefinedset)) == 0)
									return invalidExpressionToken(ident);
							}
							break;
						
						case Token.center: 
						case Token.restrict: 
						case Token.select: 
							if (tok != Token.identifier && (tok & Token.expression) == 0)
								return invalidExpressionToken(ident);
							break;
						}
					ltoken.Add(token);
					continue;
				}
				if (ltoken.Count == 0)
					return commandExpected();
				return unrecognizedToken();
			}
			aatokenCompiled = new Token[lltoken.Count][];
			lltoken.CopyTo(aatokenCompiled);
			return true;
		}
		
		/*
		mth - 2003 01 05
		initial implementation used java.util.regex.*
		second round used hand-rolled tokenizing to support old browser jvms
		
		the grammar of rasmol scripts is a little messed-up, so this structure
		was the easiest thing for me to come up with that worked
		
		final static Pattern patternLeadingWhiteSpace =
		Pattern.compile("[\\s&&[^\\r\\n]]+");
		final static Pattern patternComment =
		Pattern.compile("#[^\\r\\n]*");
		final static Pattern patternEndOfStatement =
		Pattern.compile(";");
		final static Pattern patternEndOfLine =
		Pattern.compile("\\r?\\n|\\r|$", Pattern.MULTILINE);
		final static Pattern patternDecimal =
		Pattern.compile("-?\\d+\\.(\\d*)?|-?\\.\\d+");
		final static Pattern patternPositiveInteger =
		Pattern.compile("\\d+");
		final static Pattern patternNegativeInteger =
		Pattern.compile("-\\d+");
		final static Pattern patternString =
		Pattern.compile("([\"'`])(.*?)\\1");
		final static Pattern patternSpecialString =
		Pattern.compile("[^\\r\\n]+");
		final static Pattern patternLookup =
		Pattern.compile("\\(|\\)|," +
		"|<=|<|>=|>|==|=|!=|<>|/=" +
		"|&|\\||!" +
		"|\\*" +                      // select *
		"|-" +                        // range
		"|\\[|\\]" +                  // color [##,##,##]
		"|\\+" +                      // bond
		"|\\?" +                      // help command
		"|[a-zA-Z_][a-zA-Z_0-9]*"
		);
		
		boolean lookingAt(Pattern pattern, String description) {
		Matcher m = pattern.matcher(script.subSequence(ichToken, cchScript));
		boolean lookingAt = m.lookingAt();
		if (lookingAt) {
		strToken = m.group();
		cchToken = m.end();
		} else {
		cchToken = 0;
		}
		return lookingAt;
		}
		*/
		
		private static bool isSpaceOrTab(char ch)
		{
			return ch == ' ' || ch == '\t';
		}
		
		internal virtual bool lookingAtLeadingWhitespace()
		{
			log("lookingAtLeadingWhitespace");
			int ichT = ichToken;
			while (ichT < cchScript && isSpaceOrTab(script[ichT]))
				++ichT;
			cchToken = ichT - ichToken;
			log("leadingWhitespace cchScript=" + cchScript + " cchToken=" + cchToken);
			return cchToken > 0;
		}
		
		internal virtual bool lookingAtComment()
		{
			log("lookingAtComment ichToken=" + ichToken + " cchToken=" + cchToken);
			// first, find the end of the statement and scan for # (sharp) signs
			char ch;
			int ichEnd = ichToken;
			int ichFirstSharp = - 1;
			while (ichEnd < cchScript && (ch = script[ichEnd]) != ';' && ch != '\r' && ch != '\n')
			{
				if (ch == '#' && ichFirstSharp == - 1)
				{
					ichFirstSharp = ichEnd;
					//System.out.println("I see a first sharp @ " + ichFirstSharp);
				}
				++ichEnd;
			}
			if (ichFirstSharp == - 1)
			// there were no sharps found
				return false;
			
			/****************************************************************
			* check for #jc comment
			* if it occurs anywhere in the statement, then the statement is
			* not executed.
			* This allows statements which are executed in RasMol but are
			* comments in Jmol
			****************************************************************/
			
			/*
			System.out.println("looking for #jc comment");
			System.out.println("count left=" + (cchScript - ichFirstSharp) + '\n' +
			script.charAt(ichFirstSharp + 1) +
			script.charAt(ichFirstSharp + 2));
			*/
			
			
			if (cchScript - ichFirstSharp >= 3 && script[ichFirstSharp + 1] == 'j' && script[ichFirstSharp + 2] == 'c')
			{
				// statement contains a #jc before then end ... strip it all
				cchToken = ichEnd - ichToken;
				return true;
			}
			
			// if the sharp was not the first character then it isn't a comment
			if (ichFirstSharp != ichToken)
				return false;
			
			/****************************************************************
			* check for leading #jx <space> or <tab>
			* if you see it, then only strip those 4 characters
			* if they put in #jx <newline> then they are not going to
			* execute anything, and the regular code will take care of it
			****************************************************************/
			if (cchScript > ichToken + 3 && script[ichToken + 1] == 'j' && script[ichToken + 2] == 'x' && isSpaceOrTab(script[ichToken + 3]))
			{
				cchToken = 4; // #jx[\s\t]
				return true;
			}
			
			// first character was a sharp, but was not #jx ... strip it all
			cchToken = ichEnd - ichToken;
			return true;
		}
		
		internal virtual bool lookingAtEndOfLine()
		{
			log("lookingAtEndOfLine");
			if (ichToken == cchScript)
				return true;
			int ichT = ichToken;
			char ch = script[ichT];
			if (ch == '\r')
			{
				++ichT;
				if (ichT < cchScript && script[ichT] == '\n')
					++ichT;
			}
			else if (ch == '\n')
			{
				++ichT;
			}
			else
			{
				return false;
			}
			cchToken = ichT - ichToken;
			return true;
		}
		
		internal virtual bool lookingAtEndOfStatement()
		{
			if (ichToken == cchScript || script[ichToken] != ';')
				return false;
			cchToken = 1;
			return true;
		}
		
		internal virtual bool lookingAtString()
		{
			if (ichToken == cchScript)
				return false;
			if (script[ichToken] != '"')
				return false;
			// remove support for single quote
			// in order to use it in atom expressions
			//    char chFirst = script.charAt(ichToken);
			//    if (chFirst != '"' && chFirst != '\'')
			//      return false;
			int ichT = ichToken + 1;
			//    while (ichT < cchScript && script.charAt(ichT++) != chFirst)
			char ch;
			bool previousCharBackslash = false;
			while (ichT < cchScript)
			{
				ch = script[ichT++];
				if (ch == '"' && !previousCharBackslash)
					break;
				previousCharBackslash = ch == '\\'?!previousCharBackslash:false;
			}
			cchToken = ichT - ichToken;
			return true;
		}
		
		internal static int getHexitValue(char ch)
		{
			if (ch >= '0' && ch <= '9')
				return ch - '0';
			else if (ch >= 'a' && ch <= 'f')
				return 10 + ch - 'a';
			else if (ch >= 'A' && ch <= 'F')
				return 10 + ch - 'A';
			else
				return - 1;
		}
		
		// note that these formats include a space character
		internal System.String[] loadFormats = new System.String[]{"alchemy ", "mol2 ", "mopac ", "nmrpdb ", "charmm ", "xyz ", "mdl ", "pdb "};
		
		internal virtual bool lookingAtLoadFormat()
		{
			for (int i = loadFormats.Length; --i >= 0; )
			{
				System.String strFormat = loadFormats[i];
				int cchFormat = strFormat.Length;
				if (String.Compare(script, ichToken, strFormat, 0, cchFormat, true) == 0)
				{
					cchToken = cchFormat - 1; // subtract off the space character
					return true;
				}
			}
			return false;
		}
		
		internal virtual bool lookingAtSpecialString()
		{
			int ichT = ichToken;
			char ch;
			while (ichT < cchScript && (ch = script[ichT]) != ';' && ch != '\r' && ch != '\n')
				++ichT;
			cchToken = ichT - ichToken;
			log("lookingAtSpecialString cchToken=" + cchToken);
			return cchToken > 0;
		}
		
		internal virtual bool lookingAtDecimal(bool allowNegative)
		{
			if (ichToken == cchScript)
				return false;
			int ichT = ichToken;
			if (script[ichT] == '-')
				++ichT;
			bool digitSeen = false;
			char ch = 'X';
			while (ichT < cchScript && isDigit(ch = script[ichT]))
			{
				++ichT;
				digitSeen = true;
			}
			if (ichT == cchScript || ch != '.')
				return false;
			// to support 1.ca, let's check the character after the dot
			// to determine if it is an alpha
			if (ch == '.' && (ichT + 1 < cchScript) && isAlphabetic(script[ichT + 1]))
				return false;
			++ichT;
			while (ichT < cchScript && isDigit(script[ichT]))
			{
				++ichT;
				digitSeen = true;
			}
			cchToken = ichT - ichToken;
			return digitSeen;
		}
		
		internal static bool isAlphabetic(char ch)
		{
			return (ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z');
		}
		
		internal static bool isDigit(char ch)
		{
			return ch >= '0' && ch <= '9';
		}
		
		internal virtual bool lookingAtSeqcode()
		{
			int ichT = ichToken;
			char ch = ' ';
			while (ichT < cchScript && isDigit(ch = script[ichT]))
				++ichT;
			if (ichT == ichToken || ichT + 2 > cchScript || ch != '^')
				return false;
			ch = script[++ichT];
			if (!isAlphabetic(ch))
				return false;
			++ichT;
			cchToken = ichT - ichToken;
			return true;
		}
		
		internal virtual bool lookingAtInteger(bool allowNegative)
		{
			if (ichToken == cchScript)
				return false;
			int ichT = ichToken;
			if (allowNegative && script[ichToken] == '-')
				++ichT;
			int ichBeginDigits = ichT;
			while (ichT < cchScript && isDigit(script[ichT]))
				++ichT;
			if (ichBeginDigits == ichT)
				return false;
			cchToken = ichT - ichToken;
			return true;
		}
		
		internal virtual bool lookingAtLookupToken()
		{
			if (ichToken == cchScript)
				return false;
			int ichT = ichToken;
			char ch;
			switch (ch = script[ichT++])
			{
				
				case '(': 
				case ')': 
				case ',': 
				case '*': 
				case '-': 
				case '[': 
				case ']': 
				case '+': 
				case ':': 
				case '@': 
				case '.': 
				case '%': 
					break;
				
				case '&': 
				case '|': 
					if (ichT < cchScript && script[ichT] == ch)
						++ichT;
					break;
				
				case '<': 
				case '=': 
				case '>': 
					if (ichT < cchScript && ((ch = script[ichT]) == '<' || ch == '=' || ch == '>'))
						++ichT;
					break;
				
				case '/': 
				case '!': 
					if (ichT < cchScript && script[ichT] == '=')
						++ichT;
					break;
				
				default: 
					if ((ch < 'a' || ch > 'z') && (ch < 'A' && ch > 'Z') && ch != '_')
						return false;
					goto case '?';
				
				
				case '?':  // include question marks in identifier for atom expressions
					while (ichT < cchScript && (isAlphabetic(ch = script[ichT]) || isDigit(ch) || ch == '_' || ch == '?') || (ch == '^' && ichT > ichToken && isDigit(script[ichT - 1])))
						++ichT;
					break;
				}
			cchToken = ichT - ichToken;
			return true;
		}
		
		private bool commandExpected()
		{
			return compileError("command expected");
		}
		private bool cannotSet(System.String ident)
		{
			return compileError("cannot SET:" + ident);
		}
		private bool cannotShow(System.String ident)
		{
			return compileError("cannot SHOW:" + ident);
		}
		private bool invalidExpressionToken(System.String ident)
		{
			return compileError("invalid expression token:" + ident);
		}
		private bool unrecognizedToken()
		{
			return compileError("unrecognized token");
		}
		private bool badArgumentCount()
		{
			return compileError("bad argument count");
		}
		private bool expressionExpected()
		{
			return compileError("expression expected");
		}
		private bool endOfExpressionExpected()
		{
			return compileError("end of expression expected");
		}
		private bool leftParenthesisExpected()
		{
			return compileError("left parenthesis expected");
		}
		private bool rightParenthesisExpected()
		{
			return compileError("right parenthesis expected");
		}
		private bool commaExpected()
		{
			return compileError("comma expected");
		}
		private bool commaOrCloseExpected()
		{
			return compileError("comma or right parenthesis expected");
		}
		private bool stringExpected()
		{
			return compileError("string expected");
		}
		private bool unrecognizedExpressionToken()
		{
			//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			return compileError("unrecognized expression token:" + valuePeek());
		}
		/*
		private boolean integerExpectedAfterHyphen() {
		return compileError("integer expected after hyphen");
		}
		*/
		private bool comparisonOperatorExpected()
		{
			return compileError("comparison operator expected");
		}
		private bool integerExpected()
		{
			return compileError("integer expected");
		}
		
		private bool nonnegativeIntegerExpected()
		{
			return compileError("nonnegative integer expected");
		}
		
		/*
		private boolean numberExpected() {
		return compileError("number expected");
		}
		*/
		private bool numberOrKeywordExpected()
		{
			return compileError("number or keyword expected");
		}
		private bool badRGBColor()
		{
			return compileError("bad [R,G,B] color");
		}
		private bool identifierOrResidueSpecificationExpected()
		{
			return compileError("identifier or residue specification expected");
		}
		private bool residueSpecificationExpected()
		{
			return compileError("3 letter residue specification expected");
		}
		/*
		private boolean resnumSpecificationExpected() {
		return compileError("residue number specification expected");
		}
		private boolean invalidResidueNameSpecification(String strResName) {
		return compileError("invalid residue name specification:" + strResName);
		}
		*/
		private bool invalidChainSpecification()
		{
			return compileError("invalid chain specification");
		}
		private bool invalidModelSpecification()
		{
			return compileError("invalid model specification");
		}
		private bool invalidAtomSpecification()
		{
			return compileError("invalid atom specification");
		}
		
		private bool compileError(System.String errorMessage)
		{
			System.Console.Out.WriteLine("compileError(" + errorMessage + ")");
			error = true;
			this.errorMessage = errorMessage;
			return false;
		}
		
		private bool compileCommand(System.Collections.ArrayList ltoken)
		{
			Token tokenCommand = (Token) ltoken[0];
			int tokCommand = tokenCommand.tok;
			if ((tokenCommand.intValue & Token.onDefault1) == Token.onDefault1 && ltoken.Count == 1)
				ltoken.Add(Token.tokenOn);
			if (tokCommand == Token.set_Renamed)
			{
				int size = ltoken.Count;
				if (size < 2)
					return badArgumentCount();
				/*
				* miguel 2005 01 01 - setDefaultOn is not used
				if (size == 2 &&
				(((Token)ltoken.elementAt(1)).tok & Token.setDefaultOn) != 0)
				ltoken.addElement(Token.tokenOn);
				*/
			}
			atokenCommand = new Token[ltoken.Count];
			ltoken.CopyTo(atokenCommand);
			if ((tokCommand & Token.expressionCommand) != 0)
			{
				if (!compileExpression())
					return false;
			}
			else if ((tokCommand & Token.embeddedExpressions) != 0)
			{
				int embeddedExpressionCount = compileEmbeddedExpressions();
				if (embeddedExpressionCount < 0)
					return false;
			}
			if ((tokCommand & Token.colorparam) != 0 && !compileColorParam())
				return false;
			if ((tokenCommand.intValue & Token.varArgCount) == 0 && (tokenCommand.intValue & 0x0F) + 1 != atokenCommand.Length)
				return badArgumentCount();
			return true;
		}
		
		/*
		mth -- I think I am going to be sick
		the grammer is not context-free
		what does the string cys120 mean?
		if you have previously defined a variable, as in
		define cys120 carbon
		then when you use cys120 it refers to the previous definition.
		however, if cys120 was *not* previously defined, then it refers to
		the residue of type cys at number 120.
		what a disaster.
		
		expression       :: = clauseOr
		
		clauseOr         ::= clauseAnd {OR clauseAnd}*
		
		clauseAnd        ::= clauseNot {AND clauseNot}*
		
		clauseNot        ::= NOT clauseNot | clausePrimitive
		
		clausePrimitive  ::= clauseComparator |
		clauseWithin |
		clauseConnected |  // RMH 3/06
		clauseResidueSpec |
		none | all |
		( clauseOr )
		
		clauseComparator ::= atomproperty comparatorop integer
		
		clauseWithin     ::= WITHIN ( clauseDistance , expression )
		
		clauseDistance   ::= integer | decimal
		
		clauseConnected  ::= CONNECTED ( integer , integer , expression ) |
		CONNECTED ( integer , expression ) |
		CONNECTED ( integer , integer ) |
		CONNECTED ( expression ) |
		CONNECTED ( integer ) |
		CONNECTED ( )
		
		clauseResidueSpec::= { clauseResNameSpec }
		{ clauseResNumSpec }
		{ clauseChainSpec }
		{ clauseAtomSpec }
		{ clauseAlternateSpec }
		{ clauseModelSpec }
		
		clauseResNameSpec::= * | [ resNamePattern ] | resNamePattern
		
		// question marks are part of identifiers
		// they get split up and dealt with as wildcards at runtime
		// and the integers which are residue number chains get bundled
		// in with the identifier and also split out at runtime
		// iff a variable of that name does not exist
		
		resNamePattern   ::= up to 3 alphanumeric chars with * and ?
		
		clauseResNumSpec ::= * | clauseSequenceRange
		
		clauseSequenceRange ::= clauseSequenceCode { - clauseSequenceCode }
		
		clauseSequenceCode ::= seqcode | {-} integer
		
		clauseChainSpec  ::= {:} * | identifier | integer
		
		clauseAtomSpec   ::= . * | . identifier {*}
		// note that in atom spec context a * is *not* a wildcard
		// rather, it denotes a 'prime'
		
		clauseAlternateSpec ::= {%} identifier | integer
		
		clauseModelSpec  ::= {:|/} * | integer
		
		*/
		
		internal virtual bool compileExpression()
		{
			int itokenStart = atokenCommand[0].tok == Token.define?2:1;
			int itokenEnd = compileExpression(itokenStart);
			if (itokenEnd < 0)
				return false;
			if (itokenEnd != atokenCommand.Length)
				return endOfExpressionExpected();
			return true;
		}
		
		// -1 == error, >= 0 is count of embedded expressions
		internal virtual int compileEmbeddedExpressions()
		{
			int expressionCount = 0;
			int i = 1;
			while (i < atokenCommand.Length)
			{
				if (atokenCommand[i].tok != Token.leftparen)
				{
					++i;
				}
				else
				{
					i = compileExpression(i);
					if (i < 0)
					// error
						return - 1;
					++expressionCount;
				}
			}
			return expressionCount;
		}
		
		internal System.Collections.ArrayList ltokenPostfix = null;
		internal int itokenInfix;
		
		internal virtual bool addTokenToPostfix(Token token)
		{
			ltokenPostfix.Add(token);
			return true;
		}
		
		// -1 == error, + == next token index;
		internal virtual int compileExpression(int itoken)
		{
			ltokenPostfix = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
			if (itoken >= atokenCommand.Length)
			{
				expressionExpected();
				return - 1;
			}
			for (int i = 0; i < itoken; ++i)
				addTokenToPostfix(atokenCommand[i]);
			itokenInfix = itoken;
			
			addTokenToPostfix(Token.tokenExpressionBegin);
			if (!clauseOr())
				return - 1;
			addTokenToPostfix(Token.tokenExpressionEnd);
			int returnIndex = ltokenPostfix.Count;
			// preserve any tokens after this compiled expression
			for (Token nextToken = null; (nextToken = tokenNext()) != null; )
				addTokenToPostfix(nextToken);
			atokenCommand = new Token[ltokenPostfix.Count];
			ltokenPostfix.CopyTo(atokenCommand);
			ltokenPostfix = null;
			return returnIndex;
		}
		
		internal virtual Token tokenNext()
		{
			if (itokenInfix == atokenCommand.Length)
				return null;
			return atokenCommand[itokenInfix++];
		}
		
		internal virtual bool isNextToken(int tok)
		{
			Token token = tokenNext();
			return (token != null && token.tok == tok);
		}
		
		internal virtual System.Object valuePeek()
		{
			if (itokenInfix == atokenCommand.Length)
				return null;
			return atokenCommand[itokenInfix].value_Renamed;
		}
		
		internal virtual int tokPeek()
		{
			if (itokenInfix == atokenCommand.Length)
				return 0;
			return atokenCommand[itokenInfix].tok;
		}
		
		internal virtual bool clauseOr()
		{
			if (!clauseAnd())
				return false;
			while (tokPeek() == Token.opOr)
			{
				Token tokenOr = tokenNext();
				if (!clauseAnd())
					return false;
				addTokenToPostfix(tokenOr);
			}
			return true;
		}
		
		internal virtual bool clauseAnd()
		{
			if (!clauseNot())
				return false;
			while (tokPeek() == Token.opAnd)
			{
				Token tokenAnd = tokenNext();
				if (!clauseNot())
					return false;
				addTokenToPostfix(tokenAnd);
			}
			return true;
		}
		
		internal virtual bool clauseNot()
		{
			if (tokPeek() == Token.opNot)
			{
				Token tokenNot = tokenNext();
				if (!clauseNot())
					return false;
				return addTokenToPostfix(tokenNot);
			}
			return clausePrimitive();
		}
		
		internal virtual bool clausePrimitive()
		{
			int tok = tokPeek();
			switch (tok)
			{
				
				case Token.within: 
					return clauseWithin();
				
				case Token.connected: 
					return clauseConnected();
				
				case Token.substructure: 
					return clauseSubstructure();
				
				case Token.hyphen: 
				// selecting a negative residue spec
				case Token.integer: 
				case Token.seqcode: 
				case Token.asterisk: 
				case Token.leftsquare: 
				case Token.identifier: 
				case Token.x: 
				case Token.y: 
				case Token.z: 
				case Token.colon: 
					return clauseResidueSpec();
				
				default: 
					if ((tok & Token.atomproperty) == Token.atomproperty)
						return clauseComparator();
					if ((tok & Token.predefinedset) != Token.predefinedset)
						break;
					// fall into the code and below and just add the token
					goto case Token.all;
				
				
				case Token.all: 
				case Token.none: 
					return addTokenToPostfix(tokenNext());
				
				case Token.leftparen: 
					tokenNext();
					if (!clauseOr())
						return false;
					if (!isNextToken(Token.rightparen))
						return rightParenthesisExpected();
					return true;
				}
			return unrecognizedExpressionToken();
		}
		
		internal virtual bool clauseComparator()
		{
			Token tokenAtomProperty = tokenNext();
			Token tokenComparator = tokenNext();
			if ((tokenComparator.tok & Token.comparator) == 0)
				return comparisonOperatorExpected();
			Token tokenValue = tokenNext();
			if (tokenValue.tok != Token.integer)
				return integerExpected();
			int val = tokenValue.intValue;
			// note that a comparator instruction is a complicated instruction
			// int intValue is the tok of the property you are comparing
			// the value against which you are comparing is stored as an Integer
			// in the object value
			return addTokenToPostfix(new Token(tokenComparator.tok, tokenAtomProperty.tok, (System.Object) val));
		}
		
		internal virtual bool clauseWithin()
		{
			tokenNext(); // WITHIN
			if (!isNextToken(Token.leftparen))
			// (
				return leftParenthesisExpected();
			System.Object distance;
			Token tokenDistance = tokenNext(); // distance
			if (tokenDistance == null)
				return numberOrKeywordExpected();
			switch (tokenDistance.tok)
			{
				
				case Token.integer: 
					distance = (float) ((tokenDistance.intValue * 4) / 1000f);
					break;
				
				case Token.decimal_Renamed: 
				case Token.group: 
				case Token.chain: 
				case Token.model: 
					distance = tokenDistance.value_Renamed;
					break;
				
				default: 
					return numberOrKeywordExpected();
				
			}
			if (!isNextToken(Token.opOr))
			// ,
				return commaExpected();
			if (!clauseOr())
			// *expression*
				return false;
			if (!isNextToken(Token.rightparen))
			// )T
				return rightParenthesisExpected();
			return addTokenToPostfix(new Token(Token.within, distance));
		}
		
		internal virtual bool clauseConnected()
		{
			int min = 1;
			int max = System.Int32.MaxValue;
			int tok;
			bool iHaveExpression = false;
			Token token;
			tokenNext(); // Connected
			if (tokPeek() != Token.leftparen)
				return leftParenthesisExpected();
			tokenNext(); // (
			tok = tokPeek();
			if (tok == Token.integer)
			{
				// minimum or required # of bonds(optional)
				token = tokenNext();
				if (token.intValue < 0)
					return nonnegativeIntegerExpected();
				min = max = token.intValue;
				tok = tokPeek();
				if (tok != Token.rightparen && tok != Token.opOr)
					return commaOrCloseExpected();
				if (tok == Token.opOr)
				{
					// ,
					tokenNext();
					tok = tokPeek();
				}
			}
			if (tok == Token.integer)
			{
				token = tokenNext(); // maximum # of bonds (optional)
				if (token.intValue < 0)
					nonnegativeIntegerExpected();
				max = token.intValue;
				tok = tokPeek();
				if (tok != Token.rightparen && tok != Token.opOr)
					return commaOrCloseExpected();
				if (tok == Token.opOr)
				{
					// ,
					tokenNext();
					tok = tokPeek();
				}
			}
			if (tok != Token.rightparen)
			{
				if (!clauseOr())
				// *expression*
					return false;
				iHaveExpression = true;
				tok = tokPeek();
			}
			if (!isNextToken(Token.rightparen))
			// )T
				return rightParenthesisExpected();
			if (!iHaveExpression)
				addTokenToPostfix(new Token(Token.all));
			return addTokenToPostfix(new Token(Token.connected, min, (System.Object) max));
		}
		
		internal virtual bool clauseSubstructure()
		{
			tokenNext(); // substructure
			if (!isNextToken(Token.leftparen))
			// (
				return leftParenthesisExpected();
			Token tokenSmiles = tokenNext(); // "smiles"
			if (tokenSmiles == null || tokenSmiles.tok != Token.string_Renamed)
				return stringExpected();
			if (!isNextToken(Token.rightparen))
			// )
				return rightParenthesisExpected();
			return addTokenToPostfix(new Token(Token.substructure, tokenSmiles.value_Renamed));
		}
		
		internal bool residueSpecCodeGenerated;
		
		internal virtual bool generateResidueSpecCode(Token token)
		{
			addTokenToPostfix(token);
			if (residueSpecCodeGenerated)
				addTokenToPostfix(Token.tokenAnd);
			residueSpecCodeGenerated = true;
			return true;
		}
		
		internal virtual bool clauseResidueSpec()
		{
			bool specSeen = false;
			residueSpecCodeGenerated = false;
			int tok = tokPeek();
			if (tok == Token.asterisk || tok == Token.leftsquare || tok == Token.identifier || tok == Token.set_Renamed || tok == Token.x || tok == Token.y || tok == Token.z)
			{
				log("I see a residue name");
				if (!clauseResNameSpec())
					return false;
				specSeen = true;
				tok = tokPeek();
			}
			if (tok == Token.asterisk || tok == Token.hyphen || tok == Token.integer || tok == Token.seqcode)
			{
				log("I see a residue number");
				if (!clauseResNumSpec())
					return false;
				specSeen = true;
				tok = tokPeek();
			}
			if (tok == Token.colon || tok == Token.asterisk || tok == Token.identifier || tok == Token.x || tok == Token.y || tok == Token.z || tok == Token.integer)
			{
				if (!clauseChainSpec())
					return false;
				specSeen = true;
				tok = tokPeek();
			}
			if (tok == Token.dot)
			{
				if (!clauseAtomSpec())
					return false;
				specSeen = true;
				tok = tokPeek();
			}
			if (tok == Token.percent)
			{
				if (!clauseAlternateSpec())
					return false;
				specSeen = true;
				tok = tokPeek();
			}
			if (tok == Token.colon || tok == Token.slash)
			{
				if (!clauseModelSpec())
					return false;
				specSeen = true;
				tok = tokPeek();
			}
			if (!specSeen)
				return residueSpecificationExpected();
			if (!residueSpecCodeGenerated)
			{
				// nobody generated any code, so everybody was a * (or equivalent)
				addTokenToPostfix(Token.tokenAll);
			}
			return true;
		}
		
		internal virtual bool clauseResNameSpec()
		{
			int tokPeek = tokPeek();
			if (tokPeek == Token.asterisk)
			{
				tokenNext();
				return true;
			}
			Token tokenT = tokenNext();
			if (tokenT == null)
				return false;
			if (tokenT.tok == Token.leftsquare)
			{
				log("I see a left square bracket");
				// FIXME mth -- maybe need to deal with asterisks here too
				tokenT = tokenNext();
				if (tokenT == null)
					return false;
				System.String strSpec = "";
				if (tokenT.tok == Token.plus)
				{
					strSpec = "+";
					tokenT = tokenNext();
					if (tokenT == null)
						return false;
				}
				// what a hack :-(
				int tok = tokenT.tok;
				if (tok == Token.integer)
				{
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					strSpec += tokenT.value_Renamed;
					tokenT = tokenNext();
					if (tokenT == null)
						return false;
					tok = tokenT.tok;
				}
				if (tok == Token.identifier || tok == Token.set_Renamed || tok == Token.x || tok == Token.y || tok == Token.z)
				{
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					strSpec += tokenT.value_Renamed;
					tokenT = tokenNext();
					if (tokenT == null)
						return false;
					tok = tokenT.tok;
				}
				if ((System.Object) strSpec == (System.Object) "")
					return residueSpecificationExpected();
				strSpec = strSpec.ToUpper();
				int groupID = Group.lookupGroupID(strSpec);
				if (groupID != - 1)
					generateResidueSpecCode(new Token(Token.spec_resid, groupID, strSpec));
				else
					generateResidueSpecCode(new Token(Token.spec_name_pattern, strSpec));
				return tok == Token.rightsquare;
			}
			return processIdentifier(tokenT);
		}
		
		internal virtual bool processIdentifier(Token tokenIdent)
		{
			// OK, the kludge here is that in the general case, it is not
			// possible to distinguish between an identifier and an atom expression
			if (tokenIdent.tok != Token.identifier)
				return identifierOrResidueSpecificationExpected();
			System.String strToken = (System.String) tokenIdent.value_Renamed;
			log("processing identifier:" + strToken);
			int cchToken = strToken.Length;
			
			// too short to be an atom specification? 
			if (cchToken < 3)
				return generateResidueSpecCode(tokenIdent);
			// has characters where there should be digits?
			// but don't look at last character because it could be chain spec
			for (int i = 3; i < cchToken - 1; ++i)
				if (!System.Char.IsDigit(strToken[i]))
					return generateResidueSpecCode(tokenIdent);
			log("still here looking at:" + strToken);
			
			// still might be an identifier ... so be careful
			int seqcode = - 1;
			char chain = '?';
			if (cchToken > 3)
			{
				// let's take a look at the last character
				System.String strResno;
				char chLast = strToken[cchToken - 1];
				log("the last character is:" + chLast);
				if (System.Char.IsDigit(chLast))
				{
					strResno = strToken.Substring(3);
					log("strResNo=" + strResno);
				}
				else
				{
					chain = chLast;
					strResno = strToken.Substring(3, (cchToken - 1) - (3));
				}
				try
				{
					int sequenceNum = System.Int32.Parse(strResno);
					log("I parsed sequenceNum=" + sequenceNum);
					seqcode = Group.getSeqcode(sequenceNum, ' ');
				}
				catch (System.FormatException e)
				{
					return generateResidueSpecCode(tokenIdent);
				}
			}
			System.String strUpper3 = strToken.Substring(0, (3) - (0)).ToUpper();
			int groupID;
			if (strUpper3[0] == '?' || strUpper3[1] == '?' || strUpper3[2] == '?')
			{
				generateResidueSpecCode(new Token(Token.spec_name_pattern, strUpper3));
			}
			else if ((groupID = Group.lookupGroupID(strUpper3)) != - 1)
			{
				generateResidueSpecCode(new Token(Token.spec_resid, groupID, strUpper3));
			}
			else
			{
				return generateResidueSpecCode(tokenIdent);
			}
			log(" I see a residue name:" + strUpper3 + " seqcode=" + seqcode + " chain=" + chain);
			
			if (seqcode != - 1)
				generateResidueSpecCode(new Token(Token.spec_seqcode, seqcode, "spec_seqcode"));
			if (chain != '?')
				generateResidueSpecCode(new Token(Token.spec_chain, chain, "spec_chain"));
			return true;
		}
		
		internal virtual bool clauseResNumSpec()
		{
			log("clauseResNumSpec()");
			if (tokPeek() == Token.asterisk)
			{
				tokenNext();
				return true;
			}
			return clauseSequenceRange();
		}
		
		internal virtual bool clauseSequenceRange()
		{
			if (!clauseSequenceCode())
				return false;
			if (tokPeek() == Token.hyphen)
			{
				tokenNext();
				int seqcodeA = seqcode;
				if (!clauseSequenceCode())
					return false;
				return generateResidueSpecCode(new Token(Token.spec_seqcode_range, seqcodeA, (System.Object) seqcode));
			}
			return generateResidueSpecCode(new Token(Token.spec_seqcode, seqcode, "seqcode"));
		}
		
		internal int seqcode;
		
		internal virtual bool clauseSequenceCode()
		{
			bool negative = false;
			int tokPeek = tokPeek();
			if (tokPeek == Token.hyphen)
			{
				tokenNext();
				negative = true;
				tokPeek = tokPeek();
			}
			if (tokPeek == Token.seqcode)
				seqcode = tokenNext().intValue;
			else if (tokPeek == Token.integer)
				seqcode = Group.getSeqcode(tokenNext().intValue, ' ');
			else
				return false;
			if (negative)
				seqcode = - seqcode;
			return true;
		}
		
		internal virtual bool clauseChainSpec()
		{
			if (tokPeek() == Token.colon)
				tokenNext();
			if (tokPeek() == Token.asterisk)
			{
				tokenNext();
				return true;
			}
			if (tokPeek() == Token.colon)
			// null chain followed by model spec    
				return true;
			Token tokenChain;
			char chain;
			switch (tokPeek())
			{
				
				case Token.colon: 
				case Token.nada: 
				case Token.dot: 
					chain = '\x0000';
					break;
				
				case Token.integer: 
					tokenChain = tokenNext();
					if (tokenChain.intValue < 0 || tokenChain.intValue > 9)
						return invalidChainSpecification();
					chain = (char) ('0' + tokenChain.intValue);
					break;
				
				case Token.identifier: 
				case Token.x: 
				case Token.y: 
				case Token.z: 
					tokenChain = tokenNext();
					System.String strChain = (System.String) tokenChain.value_Renamed;
					if (strChain.Length != 1)
						return invalidChainSpecification();
					chain = strChain[0];
					if (chain == '?')
						return true;
					break;
				
				default: 
					return invalidChainSpecification();
				
			}
			return generateResidueSpecCode(new Token(Token.spec_chain, chain, "spec_chain"));
		}
		
		internal virtual bool clauseAlternateSpec()
		{
			int tok = tokPeek();
			if (tok == Token.percent)
				tokenNext();
			if (tokPeek() == Token.asterisk)
			{
				tokenNext();
				return true;
			}
			Token tokenAlternate = tokenNext();
			switch (tokenAlternate.tok)
			{
				
				case Token.string_Renamed: 
				case Token.integer: 
				case Token.identifier: 
				case Token.x: 
				case Token.y: 
				case Token.z: 
					break;
				
				default: 
					return invalidModelSpecification();
				
			}
			System.String alternate = (System.String) tokenAlternate.value_Renamed;
			System.Console.Out.WriteLine("alternate specification seen:" + alternate);
			return generateResidueSpecCode(new Token(Token.spec_alternate, alternate));
		}
		
		internal virtual bool clauseModelSpec()
		{
			int tok = tokPeek();
			if (tok == Token.colon || tok == Token.slash)
				tokenNext();
			if (tokPeek() == Token.asterisk)
			{
				tokenNext();
				return true;
			}
			Token tokenModel = tokenNext();
			if (tokenModel == null)
				return invalidModelSpecification();
			switch (tokenModel.tok)
			{
				
				case Token.string_Renamed: 
				case Token.integer: 
				case Token.identifier: 
				case Token.x: 
				case Token.y: 
				case Token.z: 
					break;
				
				default: 
					return invalidModelSpecification();
				
			}
			return generateResidueSpecCode(new Token(Token.spec_model, (System.String) tokenModel.value_Renamed));
		}
		
		internal virtual bool clauseAtomSpec()
		{
			if (!isNextToken(Token.dot))
				return invalidAtomSpecification();
			Token tokenAtomSpec = tokenNext();
			if (tokenAtomSpec == null)
				return true;
			switch (tokenAtomSpec.tok)
			{
				
				case Token.asterisk: 
					return true;
				
				case Token.x: 
				case Token.y: 
				case Token.z: 
				case Token.identifier: 
					break;
				
				default: 
					return invalidAtomSpecification();
				
			}
			System.String atomSpec = (System.String) tokenAtomSpec.value_Renamed;
			if (tokPeek() == Token.asterisk)
			{
				tokenNext();
				// this one is a '*' as a prime, not a wildcard
				atomSpec += "*";
			}
			return generateResidueSpecCode(new Token(Token.spec_atom, atomSpec));
		}
		
		internal virtual bool compileColorParam()
		{
			for (int i = 1; i < atokenCommand.Length; ++i)
			{
				Token token = atokenCommand[i];
				if (token.tok == Token.leftsquare)
				{
					Token[] atokenNew = new Token[i + 1];
					Array.Copy(atokenCommand, 0, atokenNew, 0, i);
					if (!compileRGB(atokenCommand, i, atokenNew))
						return false;
					atokenCommand = atokenNew;
					break;
				}
				else if (token.tok == Token.identifier)
				{
					System.String id = (System.String) token.value_Renamed;
					int argb = Graphics3D.getArgbFromString(id);
					if (argb != 0)
					{
						token.tok = Token.colorRGB;
						token.intValue = argb;
					}
				}
			}
			return true;
		}
		
		internal virtual bool compileRGB(Token[] atoken, int i, Token[] atokenNew)
		{
			if (atoken.Length == i + 7 && atoken[i].tok == Token.leftsquare && atoken[i + 1].tok == Token.integer && atoken[i + 2].tok == Token.opOr && atoken[i + 3].tok == Token.integer && atoken[i + 4].tok == Token.opOr && atoken[i + 5].tok == Token.integer && atoken[i + 6].tok == Token.rightsquare)
			{
				int argb = (unchecked((int) 0xFF000000) | atoken[i + 1].intValue << 16 | atoken[i + 3].intValue << 8 | atoken[i + 5].intValue);
				atokenNew[i] = new Token(Token.colorRGB, argb, "[R,G,B]");
				return true;
			}
			// chime also accepts [xRRGGBB]
			if (atoken.Length == i + 3 && atoken[i].tok == Token.leftsquare && atoken[i + 1].tok == Token.identifier && atoken[i + 2].tok == Token.rightsquare)
			{
				System.String hex = (System.String) atoken[i + 1].value_Renamed;
				if (hex.Length == 7 && hex[0] == 'x')
				{
					try
					{
						//UPGRADE_TODO: Method 'java.lang.Integer.parseInt' was converted to 'System.Convert.ToInt32' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
						int argb = unchecked((int) 0xFF000000) | System.Convert.ToInt32(hex.Substring(1), 16);
						atokenNew[i] = new Token(Token.colorRGB, argb, "[xRRGGBB]");
						return true;
					}
					catch (System.FormatException e)
					{
					}
				}
			}
			return badRGBColor();
		}
	}
}