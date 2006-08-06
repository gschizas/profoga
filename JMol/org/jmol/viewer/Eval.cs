/* $RCSfile$
* $Author: hansonr $
* $Date: 2006-04-12 22:09:52 +0200 (mer., 12 avr. 2006) $
* $Revision: 4963 $
*
* Copyright (C) 2003-2006  Miguel, Jmol Development, www.jmol.org
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
using Font3D = org.jmol.g3d.Font3D;
using InvalidSmilesException = org.jmol.smiles.InvalidSmilesException;
//UPGRADE_TODO: The type 'javax.vecmath.AxisAngle4f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using AxisAngle4f = javax.vecmath.AxisAngle4f;
//UPGRADE_TODO: The type 'javax.vecmath.Matrix3f' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Matrix3f = javax.vecmath.Matrix3f;
namespace org.jmol.viewer
{
	
	class Context
	{
		internal System.String filename;
		internal System.String script;
		internal short[] linenumbers;
		internal short[] lineIndices;
		internal Token[][] aatoken;
		internal int pc;
	}
	
	class Eval : IThreadRunnable
	{
		virtual internal bool ScriptExecuting
		{
			get
			{
				return myThread != null;
			}
			
		}
		virtual internal bool Active
		{
			get
			{
				return myThread != null;
			}
			
		}
		virtual internal System.String ErrorMessage
		{
			get
			{
				return errorMessage;
			}
			
		}
		virtual internal int ExecutionWalltime
		{
			get
			{
				return (int) (timeEndExecution - timeBeginExecution);
			}
			
		}
		virtual internal int Linenumber
		{
			get
			{
				return linenumbers[pc];
			}
			
		}
		virtual internal System.String Line
		{
			get
			{
				int ichBegin = lineIndices[pc];
				int ichEnd;
				//UPGRADE_WARNING: Method 'java.lang.String.indexOf' was converted to 'System.String.IndexOf' which may throw an exception. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1101'"
				if ((ichEnd = script_Renamed_Field.IndexOf('\r', ichBegin)) == - 1 && (ichEnd = script_Renamed_Field.IndexOf('\n', ichBegin)) == - 1)
					ichEnd = script_Renamed_Field.Length;
				return script_Renamed_Field.Substring(ichBegin, (ichEnd) - (ichBegin));
			}
			
		}
		virtual internal float SetAngstroms
		{
			get
			{
				checkLength3();
				Token token = statement[2];
				switch (token.tok)
				{
					
					case Token.integer: 
						return token.intValue / 250f;
					
					case Token.decimal_Renamed: 
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						return (float) ((System.Single) token.value_Renamed);
					
					default: 
						numberExpected();
						break;
					
				}
				return - 1;
			}
			
		}
		virtual internal bool SetBoolean
		{
			get
			{
				checkLength3();
				switch (statement[2].tok)
				{
					
					case Token.on: 
						return true;
					
					case Token.off: 
						return false;
					
					default: 
						booleanExpected();
						break;
					
				}
				return false;
			}
			
		}
		virtual internal short SetAxesTypeMad
		{
			get
			{
				checkLength3();
				int tok = statement[2].tok;
				short mad = 0;
				switch (tok)
				{
					
					case Token.on: 
						mad = 1;
						goto case Token.off;
					
					case Token.off: 
						break;
					
					case Token.integer: 
						int diameterPixels = statement[2].intValue;
						if (diameterPixels < 0 || diameterPixels >= 20)
							numberOutOfRange(0, 19);
						mad = (short) diameterPixels;
						break;
					
					case Token.decimal_Renamed: 
						float angstroms = floatParameter(2);
						if (angstroms < 0 || angstroms >= 2)
							numberOutOfRange(0f, 1.99999f);
						//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
						mad = (short) (angstroms * 1000 * 2);
						break;
					
					case Token.dotted: 
						mad = - 1;
						break;
					
					default: 
						booleanOrNumberExpected();
						break;
					
				}
				return mad;
			}
			
		}
		virtual internal int SetInteger
		{
			get
			{
				checkLength3();
				return intParameter(2);
			}
			
		}
		virtual internal System.Collections.BitArray HeteroSet
		{
			get
			{
				Frame frame = viewer.Frame;
				System.Collections.BitArray bsHetero = new System.Collections.BitArray(64);
				for (int i = viewer.AtomCount; --i >= 0; )
					if (frame.getAtomAt(i).Hetero)
						SupportClass.BitArraySupport.Set(bsHetero, i);
				return bsHetero;
			}
			
		}
		virtual internal System.Collections.BitArray HydrogenSet
		{
			get
			{
				if (logMessages)
					viewer.scriptStatus("getHydrogenSet()");
				Frame frame = viewer.Frame;
				System.Collections.BitArray bsHydrogen = new System.Collections.BitArray(64);
				for (int i = viewer.AtomCount; --i >= 0; )
				{
					Atom atom = frame.getAtomAt(i);
					if (atom.ElementNumber == 1)
						SupportClass.BitArraySupport.Set(bsHydrogen, i);
				}
				return bsHydrogen;
			}
			
		}
		virtual internal System.Collections.BitArray ProteinSet
		{
			get
			{
				Frame frame = viewer.Frame;
				System.Collections.BitArray bsProtein = new System.Collections.BitArray(64);
				for (int i = viewer.AtomCount; --i >= 0; )
					if (frame.getAtomAt(i).Protein)
						SupportClass.BitArraySupport.Set(bsProtein, i);
				return bsProtein;
			}
			
		}
		virtual internal System.Collections.BitArray NucleicSet
		{
			get
			{
				Frame frame = viewer.Frame;
				System.Collections.BitArray bsNucleic = new System.Collections.BitArray(64);
				for (int i = viewer.AtomCount; --i >= 0; )
					if (frame.getAtomAt(i).Nucleic)
						SupportClass.BitArraySupport.Set(bsNucleic, i);
				return bsNucleic;
			}
			
		}
		virtual internal System.Collections.BitArray DnaSet
		{
			get
			{
				Frame frame = viewer.Frame;
				System.Collections.BitArray bsDna = new System.Collections.BitArray(64);
				for (int i = viewer.AtomCount; --i >= 0; )
					if (frame.getAtomAt(i).Dna)
						SupportClass.BitArraySupport.Set(bsDna, i);
				return bsDna;
			}
			
		}
		virtual internal System.Collections.BitArray RnaSet
		{
			get
			{
				Frame frame = viewer.Frame;
				System.Collections.BitArray bsRna = new System.Collections.BitArray(64);
				for (int i = viewer.AtomCount; --i >= 0; )
					if (frame.getAtomAt(i).Rna)
						SupportClass.BitArraySupport.Set(bsRna, i);
				return bsRna;
			}
			
		}
		virtual internal System.Collections.BitArray PurineSet
		{
			get
			{
				Frame frame = viewer.Frame;
				System.Collections.BitArray bsPurine = new System.Collections.BitArray(64);
				for (int i = viewer.AtomCount; --i >= 0; )
					if (frame.getAtomAt(i).Purine)
						SupportClass.BitArraySupport.Set(bsPurine, i);
				return bsPurine;
			}
			
		}
		virtual internal System.Collections.BitArray PyrimidineSet
		{
			get
			{
				Frame frame = viewer.Frame;
				System.Collections.BitArray bsPyrimidine = new System.Collections.BitArray(64);
				for (int i = viewer.AtomCount; --i >= 0; )
					if (frame.getAtomAt(i).Pyrimidine)
						SupportClass.BitArraySupport.Set(bsPyrimidine, i);
				return bsPyrimidine;
			}
			
		}
		virtual internal short MadParameter
		{
			get
			{
				//for wireframe, ssbond, hbond
				int tok = statement[1].tok;
				short mad = 1;
				switch (tok)
				{
					
					case Token.on: 
						break;
					
					case Token.off: 
						mad = 0;
						break;
					
					case Token.integer: 
						int radiusRasMol = statement[1].intValue;
						if (radiusRasMol < 0 || radiusRasMol > 750)
							numberOutOfRange(0, 750);
						mad = (short) (radiusRasMol * 4 * 2);
						break;
					
					case Token.decimal_Renamed: 
						float angstroms = floatParameter(1);
						if (angstroms < 0 || angstroms > 3)
							numberOutOfRange(0f, 3f);
						//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
						mad = (short) (angstroms * 1000 * 2);
						break;
					
					default: 
						booleanOrNumberExpected();
						break;
					
				}
				return mad;
			}
			
		}
		
		internal Compiler compiler;
		
		internal const int scriptLevelMax = 10;
		internal int scriptLevel;
		
		internal Context[] stack = new Context[scriptLevelMax];
		
		internal System.String filename;
		internal System.String script_Renamed_Field;
		internal short[] linenumbers;
		internal short[] lineIndices;
		internal Token[][] aatoken;
		internal int pc; // program counter
		
		internal long timeBeginExecution;
		internal long timeEndExecution;
		internal bool error;
		internal System.String errorMessage;
		
		internal Token[] statement;
		internal int statementLength;
		internal Viewer viewer;
		internal SupportClass.ThreadClass myThread;
		internal bool terminationNotification;
		internal bool interruptExecution;
		internal bool tQuiet;
		
		internal const bool logMessages = false;
		
		internal Eval(Viewer viewer)
		{
			compiler = new Compiler();
			this.viewer = viewer;
			clearDefinitionsAndLoadPredefined();
		}
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'start'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		internal virtual void  start()
		{
			lock (this)
			{
				if (myThread == null)
				{
					myThread = new SupportClass.ThreadClass(new System.Threading.ThreadStart(this.Run));
					interruptExecution = false;
					myThread.Start();
				}
			}
		}
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'haltExecution'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		internal virtual void  haltExecution()
		{
			lock (this)
			{
				if (myThread != null)
				{
					interruptExecution = true;
					myThread.Interrupt();
				}
			}
		}
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'clearMyThread'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		internal virtual void  clearMyThread()
		{
			lock (this)
			{
				myThread = null;
			}
		}
		
		internal virtual bool hasTerminationNotification()
		{
			return terminationNotification;
		}
		
		internal virtual void  resetTerminationNotification()
		{
			terminationNotification = false;
		}
		
		internal virtual bool hadRuntimeError()
		{
			return error;
		}
		
		internal virtual bool loadScript(System.String filename, System.String script)
		{
			this.filename = filename;
			this.script_Renamed_Field = script;
			if (!compiler.compile(filename, script))
			{
				error = true;
				errorMessage = compiler.ErrorMessage;
				viewer.scriptStatus(errorMessage);
				return false;
			}
			pc = 0;
			aatoken = compiler.AatokenCompiled;
			linenumbers = compiler.LineNumbers;
			lineIndices = compiler.LineIndices;
			return true;
		}
		
		internal virtual void  clearState(bool tQuiet)
		{
			for (int i = scriptLevelMax; --i >= 0; )
				stack[i] = null;
			scriptLevel = 0;
			error = false;
			errorMessage = null;
			terminationNotification = false;
			interruptExecution = false;
			this.tQuiet = tQuiet;
		}
		
		internal virtual bool loadScriptString(System.String script, bool tQuiet)
		{
			clearState(tQuiet);
			return loadScript(null, script);
		}
		
		internal virtual bool loadScriptFile(System.String filename, bool tQuiet)
		{
			clearState(tQuiet);
			return loadScriptFileInternal(filename);
		}
		
		internal virtual bool loadScriptFileInternal(System.String filename)
		{
			System.Object t = viewer.getInputStreamOrErrorMessageFromName(filename);
			if (!(t is System.IO.Stream))
				return loadError((System.String) t);
			//UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
			//UPGRADE_WARNING: At least one expression was used more than once in the target code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1181'"
			System.IO.StreamReader reader = new System.IO.StreamReader(new System.IO.StreamReader((System.IO.Stream) t, System.Text.Encoding.Default).BaseStream, new System.IO.StreamReader((System.IO.Stream) t, System.Text.Encoding.Default).CurrentEncoding);
			System.Text.StringBuilder script = new System.Text.StringBuilder();
			try
			{
				while (true)
				{
					System.String command = reader.ReadLine();
					if (command == null)
						break;
					script.Append(command);
					script.Append("\n");
				}
			}
			catch (System.IO.IOException e)
			{
				try
				{
					reader.Close();
				}
				catch (System.IO.IOException ioe)
				{
				}
				return ioError(filename);
			}
			try
			{
				reader.Close();
			}
			catch (System.IO.IOException ioe)
			{
			}
			return loadScript(filename, script.ToString());
		}
		
		internal virtual bool loadError(System.String msg)
		{
			error = true;
			errorMessage = msg;
			return false;
		}
		
		internal virtual bool fileNotFound(System.String filename)
		{
			return loadError("file not found:" + filename);
		}
		
		internal virtual bool ioError(System.String filename)
		{
			return loadError("io error reading:" + filename);
		}
		
		public override System.String ToString()
		{
			System.Text.StringBuilder str = new System.Text.StringBuilder();
			str.Append("Eval\n pc:");
			str.Append(pc); str.Append("\n");
			str.Append(aatoken.Length); str.Append(" statements\n");
			for (int i = 0; i < aatoken.Length; ++i)
			{
				str.Append(" |");
				Token[] atoken = aatoken[i];
				for (int j = 0; j < atoken.Length; ++j)
				{
					str.Append(' ');
					str.Append(atoken[j]);
				}
				str.Append("\n");
			}
			str.Append("END\n");
			return str.ToString();
		}
		
		internal virtual void  clearDefinitionsAndLoadPredefined()
		{
			variables.Clear();
			
			int cPredef = JmolConstants.predefinedSets.Length;
			for (int iPredef = 0; iPredef < cPredef; iPredef++)
				predefine(JmolConstants.predefinedSets[iPredef]);
			// Now, define all the elements as predefined sets
			// hydrogen is handled specially, so don't define it
			for (int i = JmolConstants.elementNames.Length; --i > 1; )
			{
				System.String definition = "@" + JmolConstants.elementNames[i] + " _e=" + i;
				predefine(definition);
			}
			for (int j = JmolConstants.alternateElementNumbers.Length; --j >= 0; )
			{
				System.String definition = "@" + JmolConstants.alternateElementNames[j] + " _e=" + JmolConstants.alternateElementNumbers[j];
				predefine(definition);
			}
		}
		
		internal virtual void  predefineElements()
		{
			// the name 'hydrogen' handled specially
			for (int i = JmolConstants.elementNames.Length; --i > 1; )
			{
				System.String definition = "@" + JmolConstants.elementNames[i] + " _e=" + i;
				if (!compiler.compile("#element", definition))
				{
					System.Console.Out.WriteLine("element definition error:" + definition);
					continue;
				}
				Token[][] aatoken = compiler.AatokenCompiled;
				Token[] statement = aatoken[0];
				//int tok = statement[1].tok;
				System.String variable = (System.String) statement[1].value_Renamed;
				variables[variable] = statement;
			}
		}
		
		internal virtual void  predefine(System.String script)
		{
			if (compiler.compile("#predefine", script))
			{
				Token[][] aatoken = compiler.AatokenCompiled;
				if (aatoken.Length != 1)
				{
					viewer.scriptStatus("predefinition does not have exactly 1 command:" + script);
					return ;
				}
				Token[] statement = aatoken[0];
				if (statement.Length > 2)
				{
					int tok = statement[1].tok;
					if (tok == Token.identifier || (tok & Token.predefinedset) == Token.predefinedset)
					{
						System.String variable = (System.String) statement[1].value_Renamed;
						variables[variable] = statement;
					}
					else
					{
						viewer.scriptStatus("invalid variable name:" + script);
					}
				}
				else
				{
					viewer.scriptStatus("bad predefinition length:" + script);
				}
			}
			else
			{
				viewer.scriptStatus("predefined set compile error:" + script + "\ncompile error:" + compiler.ErrorMessage);
			}
		}
		
		public virtual void  Run()
		{
			// this refresh is here to ensure that the screen has been painted ...
			// since it could be a problem when an applet is loaded with a script
			// ready to run. 
			refresh();
			timeBeginExecution = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
			viewer.pushHoldRepaint();
			try
			{
				instructionDispatchLoop();
			}
			catch (ScriptException e)
			{
				error = true;
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				errorMessage = "" + e;
				viewer.scriptStatus(errorMessage);
			}
			timeEndExecution = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
			if (errorMessage == null && interruptExecution)
				errorMessage = "execution interrupted";
			if (errorMessage != null)
				viewer.scriptStatus(errorMessage);
			else if (!tQuiet)
				viewer.scriptStatus("Script completed");
			clearMyThread();
			terminationNotification = true;
			viewer.popHoldRepaint();
		}
		
		internal virtual void  instructionDispatchLoop()
		{
			long timeBegin = 0;
			if (logMessages)
			{
				timeBegin = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
				viewer.scriptStatus("Eval.instructionDispatchLoop():" + timeBegin);
				viewer.scriptStatus(ToString());
			}
			while (!interruptExecution && pc < aatoken.Length)
			{
				statement = aatoken[pc++];
				statementLength = statement.Length;
				if (viewer.getDebugScript())
					logDebugScript();
				Token token = statement[0];
				switch (token.tok)
				{
					
					case Token.backbone: 
						proteinShape(JmolConstants.SHAPE_BACKBONE);
						break;
					
					case Token.background: 
						background();
						break;
					
					case Token.center: 
						center();
						break;
					
					case Token.color: 
						color();
						break;
					
					case Token.define: 
						define();
						break;
					
					case Token.echo: 
						echo();
						break;
					
					case Token.exit: 
					case Token.quit:  // in rasmol quit actually exits the program
						return ;
					
					case Token.label: 
						label();
						break;
					
					case Token.hover: 
						hover();
						break;
					
					case Token.load: 
						load();
						break;
					
					case Token.monitor: 
						monitor();
						break;
					
					case Token.refresh: 
						refresh();
						break;
					
					case Token.reset: 
						reset();
						break;
					
					case Token.rotate: 
						rotate();
						break;
					
					case Token.script: 
						script();
						break;
					
					case Token.select: 
						select();
						break;
					
					case Token.translate: 
						translate();
						break;
					
					case Token.zap: 
						zap();
						break;
					
					case Token.zoom: 
						zoom();
						break;
					
					case Token.delay: 
						delay();
						break;
					
					case Token.loop: 
						delay(); // a loop is just a delay followed by ...
						pc = 0; // ... resetting the program counter
						break;
					
					case Token.move: 
						move();
						break;
					
					case Token.restrict: 
						restrict();
						break;
					
					case Token.set_Renamed: 
						set_Renamed();
						break;
					
					case Token.slab: 
						slab();
						break;
					
					case Token.depth: 
						depth();
						break;
					
					case Token.star: 
						star();
						break;
					
					case Token.cpk: 
						cpk();
						break;
					
					case Token.wireframe: 
						wireframe();
						break;
					
					case Token.vector: 
						vector();
						break;
					
					case Token.animation: 
						animation();
						break;
					
					case Token.vibration: 
						vibration();
						break;
					
					case Token.dots: 
						dots();
						break;
					
					case Token.strands: 
						proteinShape(JmolConstants.SHAPE_STRANDS);
						break;
					
					case Token.meshRibbon: 
						proteinShape(JmolConstants.SHAPE_MESHRIBBON);
						break;
					
					case Token.ribbon: 
						proteinShape(JmolConstants.SHAPE_RIBBONS);
						break;
					
					case Token.prueba: 
						proteinShape(JmolConstants.SHAPE_PRUEBA);
						break;
					
					case Token.trace: 
						proteinShape(JmolConstants.SHAPE_TRACE);
						break;
					
					case Token.cartoon: 
						proteinShape(JmolConstants.SHAPE_CARTOON);
						break;
					
					case Token.rocket: 
						proteinShape(JmolConstants.SHAPE_ROCKETS);
						break;
					
					case Token.spin: 
						spin();
						break;
					
					case Token.ssbond: 
						ssbond();
						break;
					
					case Token.hbond: 
						hbond();
						break;
					
					case Token.show: 
						show();
						break;
					
					case Token.frame: 
					case Token.model: 
						frame();
						break;
					
					case Token.font: 
						font();
						break;
					
					case Token.moveto: 
						moveto();
						break;
					
					case Token.console: 
						console();
						break;
					
					case Token.pmesh: 
						pmesh();
						break;
					
					case Token.polyhedra: 
						polyhedra();
						break;
					
					case Token.sasurface: 
						sasurface();
						break;
					
					case Token.centerAt: 
						centerAt();
						break;
					
					case Token.isosurface: 
						isosurface();
						break;
					
					case Token.stereo: 
						stereo();
						break;
					
					case Token.connect: 
						connect();
						break;
						
						// not implemented
					
					case Token.bond: 
					case Token.clipboard: 
					case Token.help: 
					case Token.molecule: 
					case Token.pause: 
					case Token.print: 
					case Token.renumber: 
					case Token.save: 
					case Token.structure: 
					case Token.unbond: 
					case Token.write: 
					// chime extended commands
					case Token.view: 
					case Token.list: 
					case Token.display3d: 
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						viewer.scriptStatus("Script command not implemented:" + token.value_Renamed);
						break;
					
					default: 
						unrecognizedCommand(token);
						return ;
					
				}
			}
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'strbufLog '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal System.Text.StringBuilder strbufLog = new System.Text.StringBuilder(80);
		internal virtual void  logDebugScript()
		{
			strbufLog.Length = 0;
			//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			strbufLog.Append(statement[0].value_Renamed.ToString());
			for (int i = 1; i < statementLength; ++i)
			{
				strbufLog.Append(' ');
				Token token = statement[i];
				switch (token.tok)
				{
					
					case Token.integer: 
						strbufLog.Append(token.intValue);
						continue;
					
					case Token.spec_seqcode: 
						strbufLog.Append(Group.getSeqcodeString(token.intValue));
						continue;
					
					case Token.spec_chain: 
						strbufLog.Append(':');
						strbufLog.Append((char) token.intValue);
						continue;
					
					case Token.spec_alternate: 
						strbufLog.Append("%");
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						strbufLog.Append("" + token.value_Renamed);
						break;
					
					case Token.spec_model: 
						strbufLog.Append("/");
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						strbufLog.Append("" + token.value_Renamed);
						break;
					
					case Token.spec_resid: 
						strbufLog.Append('[');
						strbufLog.Append(Group.getGroup3((short) token.intValue));
						strbufLog.Append(']');
						continue;
					
					case Token.spec_name_pattern: 
						strbufLog.Append('[');
						strbufLog.Append(token.value_Renamed);
						strbufLog.Append(']');
						continue;
					
					case Token.spec_atom: 
						strbufLog.Append('.');
						break;
					
					case Token.spec_seqcode_range: 
						strbufLog.Append(Group.getSeqcodeString(token.intValue));
						strbufLog.Append('-');
						strbufLog.Append(Group.getSeqcodeString(((System.Int32) token.value_Renamed)));
						break;
					
					case Token.within: 
						strbufLog.Append("within ");
						break;
					
					case Token.connected: 
						strbufLog.Append("connected ");
						break;
					
					case Token.substructure: 
						strbufLog.Append("substructure ");
						break;
					
					case Token.opEQ: 
					case Token.opNE: 
					case Token.opGT: 
					case Token.opGE: 
					case Token.opLT: 
					case Token.opLE: 
						strbufLog.Append(Token.atomPropertyNames[token.intValue & 0x0F]);
						strbufLog.Append(Token.comparatorNames[token.tok & 0x0F]);
						break;
					}
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				strbufLog.Append("" + token.value_Renamed);
			}
			viewer.scriptStatus(strbufLog.ToString());
		}
		
		internal virtual void  evalError(System.String message)
		{
			throw new ScriptException(message, Line, filename, Linenumber);
		}
		
		internal virtual void  unrecognizedCommand(Token token)
		{
			//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			evalError("unrecognized command:" + token.value_Renamed);
		}
		
		internal virtual void  unrecognizedAtomProperty(int propnum)
		{
			evalError("unrecognized atom property:" + propnum);
		}
		
		internal virtual void  filenameExpected()
		{
			evalError("filename expected");
		}
		
		internal virtual void  booleanExpected()
		{
			evalError("boolean expected");
		}
		
		internal virtual void  booleanOrPercentExpected()
		{
			evalError("boolean or percent expected");
		}
		
		internal virtual void  booleanOrNumberExpected()
		{
			evalError("boolean or number expected");
		}
		
		internal virtual void  integerExpected()
		{
			evalError("integer expected");
		}
		
		internal virtual void  numberExpected()
		{
			evalError("number expected");
		}
		
		internal virtual void  propertyNameExpected()
		{
			evalError("property name expected");
		}
		
		internal virtual void  axisExpected()
		{
			evalError("x y z axis expected");
		}
		
		internal virtual void  colorExpected()
		{
			evalError("color expected");
		}
		
		internal virtual void  keywordExpected()
		{
			evalError("keyword expected");
		}
		
		internal virtual void  unrecognizedColorObject()
		{
			evalError("unrecognized color object");
		}
		
		internal virtual void  unrecognizedExpression()
		{
			evalError("runtime unrecognized expression");
		}
		
		internal virtual void  undefinedVariable()
		{
			evalError("variable undefined");
		}
		
		internal virtual void  badArgumentCount()
		{
			evalError("bad argument count");
		}
		
		internal virtual void  invalidArgument()
		{
			evalError("invalid argument");
		}
		
		internal virtual void  incompatibleArguments()
		{
			evalError("incompatible arguments");
		}
		
		internal virtual void  insufficientArguments()
		{
			evalError("insufficient arguments");
		}
		
		internal virtual void  unrecognizedSetParameter()
		{
			evalError("unrecognized SET parameter");
		}
		
		internal virtual void  unrecognizedSubcommand()
		{
			evalError("unrecognized subcommand");
		}
		
		internal virtual void  invalidParameterOrder()
		{
			evalError("invalid parameter order");
		}
		
		internal virtual void  subcommandExpected()
		{
			evalError("subcommand expected");
		}
		
		internal virtual void  setspecialShouldNotBeHere()
		{
			evalError("interpreter error - setspecial should not be here");
		}
		
		internal virtual void  numberOutOfRange()
		{
			evalError("number out of range");
		}
		
		internal virtual void  numberOutOfRange(int min, int max)
		{
			evalError("integer out of range (" + min + " - " + max + ")");
		}
		
		internal virtual void  numberOutOfRange(float min, float max)
		{
			evalError("decimal number out of range (" + min + " - " + max + ")");
		}
		
		internal virtual void  numberMustBe(int a, int b)
		{
			evalError("number must be (" + a + " or " + b + ")");
		}
		
		internal virtual void  badAtomNumber()
		{
			evalError("bad atom number");
		}
		
		internal virtual void  errorLoadingScript(System.String msg)
		{
			evalError("error loading script -> " + msg);
		}
		
		internal virtual void  fileNotFoundException(System.String filename)
		{
			evalError("file not found : " + filename);
		}
		
		internal virtual void  notImplemented(int itoken)
		{
			notImplemented(statement[itoken]);
		}
		
		internal virtual void  notImplemented(Token token)
		{
			//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			viewer.scriptStatus("" + token.value_Renamed + " not implemented in command:" + statement[0].value_Renamed);
		}
		
		// gets a boolean value from the 2nd parameter to the command
		// as in set foo <boolean>
		
		internal virtual void  checkStatementLength(int length)
		{
			if (statementLength != length)
				badArgumentCount();
		}
		
		internal virtual void  checkLength34()
		{
			if (statementLength < 3 || statementLength > 4)
				badArgumentCount();
		}
		
		internal virtual void  checkLength2()
		{
			checkStatementLength(2);
		}
		
		internal virtual void  checkLength3()
		{
			checkStatementLength(3);
		}
		
		internal virtual void  checkLength4()
		{
			checkStatementLength(4);
		}
		
		internal virtual int intParameter(int index)
		{
			if (statement[index].tok != Token.integer)
				integerExpected();
			return statement[index].intValue;
		}
		
		internal virtual float floatParameter(int index)
		{
			if (index >= statementLength)
				badArgumentCount();
			float floatValue = 0;
			switch (statement[index].tok)
			{
				
				case Token.integer: 
					floatValue = statement[index].intValue;
					break;
				
				case Token.decimal_Renamed: 
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					floatValue = (float) ((System.Single) statement[index].value_Renamed);
					break;
				
				default: 
					numberExpected();
					break;
				
			}
			return floatValue;
		}
		
		internal virtual System.Collections.BitArray copyBitSet(System.Collections.BitArray bitSet)
		{
			System.Collections.BitArray copy = new System.Collections.BitArray(64);
			//UPGRADE_NOTE: In .NET BitArrays must be of the same size to allow the 'System.Collections.BitArray.Or' operation. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1083'"
			copy.Or(bitSet);
			return copy;
		}
		
		internal int pcLastExpressionInstruction;
		internal virtual System.Collections.BitArray expression(Token[] code, int pcStart)
		{
			int numberOfAtoms = viewer.AtomCount;
			System.Collections.BitArray bs;
			System.Collections.BitArray[] stack = new System.Collections.BitArray[10];
			int sp = 0;
			if (logMessages)
				viewer.scriptStatus("start to evaluate expression");
			for (int pc = pcStart; ; ++pc)
			{
				Token instruction = code[pc];
				if (logMessages)
				{
					viewer.scriptStatus("instruction=" + instruction);
				}
				switch (instruction.tok)
				{
					
					case Token.expressionBegin: 
						break;
					
					case Token.expressionEnd: 
						pcLastExpressionInstruction = pc;
						//UPGRADE_NOTE: Labeled break statement was changed to a goto statement. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1012'"
						goto expression_loop_brk;
					
					case Token.all: 
						bs = stack[sp++] = new System.Collections.BitArray((numberOfAtoms % 64 == 0?numberOfAtoms / 64:numberOfAtoms / 64 + 1) * 64);
						for (int i = numberOfAtoms; --i >= 0; )
							SupportClass.BitArraySupport.Set(bs, i);
						break;
					
					case Token.none: 
						stack[sp++] = new System.Collections.BitArray(64);
						break;
					
					case Token.opOr: 
						bs = stack[--sp];
						//UPGRADE_NOTE: In .NET BitArrays must be of the same size to allow the 'System.Collections.BitArray.Or' operation. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1083'"
						stack[sp - 1].Or(bs);
						break;
					
					case Token.opAnd: 
						bs = stack[--sp];
						//UPGRADE_NOTE: In .NET BitArrays must be of the same size to allow the 'System.Collections.BitArray.And' operation. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1083'"
						stack[sp - 1].And(bs);
						break;
					
					case Token.opNot: 
						bs = stack[sp - 1];
						notSet(bs);
						break;
					
					case Token.within: 
						bs = stack[sp - 1];
						stack[sp - 1] = new System.Collections.BitArray(64);
						withinInstruction(instruction, bs, stack[sp - 1]);
						break;
					
					case Token.connected: 
						bs = stack[sp - 1];
						stack[sp - 1] = connected(instruction, bs);
						break;
					
					case Token.substructure: 
						stack[sp++] = getSubstructureSet((System.String) instruction.value_Renamed);
						break;
					
					case Token.selected: 
						stack[sp++] = copyBitSet(viewer.SelectionSet);
						break;
					
					case Token.hetero: 
						stack[sp++] = HeteroSet;
						break;
					
					case Token.hydrogen: 
						stack[sp++] = HydrogenSet;
						break;
					
					case Token.spec_name_pattern: 
						stack[sp++] = getSpecName((System.String) instruction.value_Renamed);
						break;
					
					case Token.spec_resid: 
						stack[sp++] = getSpecResid(instruction.intValue);
						break;
					
					case Token.spec_seqcode: 
						stack[sp++] = getSpecSeqcode(instruction.intValue);
						break;
					
					case Token.spec_seqcode_range: 
						int seqcodeA = instruction.intValue;
						int seqcodeB = ((System.Int32) instruction.value_Renamed);
						stack[sp++] = getSpecSeqcodeRange(seqcodeA, seqcodeB);
						break;
					
					case Token.spec_chain: 
						stack[sp++] = getSpecChain((char) instruction.intValue);
						break;
					
					case Token.spec_atom: 
						stack[sp++] = getSpecAtom((System.String) instruction.value_Renamed);
						break;
					
					case Token.spec_alternate: 
						stack[sp++] = getSpecAlternate((System.String) instruction.value_Renamed);
						break;
					
					case Token.spec_model: 
						stack[sp++] = getSpecModel((System.String) instruction.value_Renamed);
						break;
					
					case Token.protein: 
						stack[sp++] = ProteinSet;
						break;
					
					case Token.nucleic: 
						stack[sp++] = NucleicSet;
						break;
					
					case Token.dna: 
						stack[sp++] = DnaSet;
						break;
					
					case Token.rna: 
						stack[sp++] = RnaSet;
						break;
					
					case Token.purine: 
						stack[sp++] = PurineSet;
						break;
					
					case Token.pyrimidine: 
						stack[sp++] = PyrimidineSet;
						break;
					
					case Token.y: 
					case Token.amino: 
					case Token.backbone: 
					case Token.solvent: 
					case Token.identifier: 
					case Token.sidechain: 
					case Token.surface: 
						stack[sp++] = lookupIdentifierValue((System.String) instruction.value_Renamed);
						break;
					
					case Token.opLT: 
					case Token.opLE: 
					case Token.opGE: 
					case Token.opGT: 
					case Token.opEQ: 
					case Token.opNE: 
						bs = stack[sp++] = new System.Collections.BitArray(64);
						comparatorInstruction(instruction, bs);
						break;
					
					default: 
						unrecognizedExpression();
						break;
					
				}
			}
			//UPGRADE_NOTE: Label 'expression_loop_brk' was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1011'"
expression_loop_brk: ;
			
			if (sp != 1)
				evalError("atom expression compiler error - stack over/underflow");
			return stack[0];
		}
		
		internal virtual System.Collections.BitArray lookupIdentifierValue(System.String identifier)
		{
			// identifiers must be handled as a hack
			// the expression 'select c1a' might be [c]1:a or might be a 'define c1a ...'
			System.Collections.BitArray bsDefinedSet = lookupValue(identifier, false);
			if (bsDefinedSet != null)
				return copyBitSet(bsDefinedSet); // identifier had been previously defined
			//    System.out.println("undefined & trying specname with:" + identifier);
			// determine number of leading alpha characters
			int alphaLen = 0;
			int len = identifier.Length;
			while (alphaLen < len && Compiler.isAlphabetic(identifier[alphaLen]))
				++alphaLen;
			if (alphaLen > 3)
				undefinedVariable();
			System.String potentialGroupName = identifier.Substring(0, (alphaLen) - (0));
			//    System.out.println("potentialGroupName=" + potentialGroupName);
			//          undefinedVariable();
			System.Collections.BitArray bsName = lookupPotentialGroupName(potentialGroupName);
			if (bsName == null)
				undefinedVariable();
			if (alphaLen == len)
				return bsName;
			//
			// look for a sequence code
			// for now, only support a sequence number
			//
			int seqcodeEnd = alphaLen;
			while (seqcodeEnd < len && Compiler.isDigit(identifier[seqcodeEnd]))
				++seqcodeEnd;
			int seqNumber = 0;
			try
			{
				seqNumber = System.Int32.Parse(identifier.Substring(alphaLen, (seqcodeEnd) - (alphaLen)));
			}
			catch (System.FormatException nfe)
			{
				evalError("identifier parser error #373");
			}
			char insertionCode = ' ';
			if (seqcodeEnd < len && identifier[seqcodeEnd] == '^')
			{
				++seqcodeEnd;
				if (seqcodeEnd == len)
					evalError("invalid insertion code");
				insertionCode = identifier[seqcodeEnd++];
			}
			//    System.out.println("sequence number=" + seqNumber +
			//                       " insertionCode=" + insertionCode);
			int seqcode = Group.getSeqcode(seqNumber, insertionCode);
			//    System.out.println("seqcode=" + seqcode);
			System.Collections.BitArray bsSequence = getSpecSeqcode(seqcode);
			System.Collections.BitArray bsNameSequence = bsName;
			//UPGRADE_NOTE: In .NET BitArrays must be of the same size to allow the 'System.Collections.BitArray.And' operation. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1083'"
			bsNameSequence.And(bsSequence);
			if (seqcodeEnd == len)
				return bsNameSequence;
			//
			// look for a chain spec ... also alpha & part of an identifier ... :-(
			//
			char chainID = identifier[seqcodeEnd];
			if (++seqcodeEnd != len)
				undefinedVariable();
			//    System.out.println("chainID=" + chainID);
			System.Collections.BitArray bsChain = getSpecChain(chainID);
			System.Collections.BitArray bsNameSequenceChain = bsNameSequence;
			//UPGRADE_NOTE: In .NET BitArrays must be of the same size to allow the 'System.Collections.BitArray.And' operation. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1083'"
			bsNameSequenceChain.And(bsChain);
			return bsNameSequenceChain;
		}
		
		internal virtual System.Collections.BitArray lookupPotentialGroupName(System.String potentialGroupName)
		{
			System.Collections.BitArray bsResult = null;
			//    System.out.println("lookupPotentialGroupName:" + potentialGroupName);
			Frame frame = viewer.Frame;
			for (int i = viewer.AtomCount; --i >= 0; )
			{
				Atom atom = frame.getAtomAt(i);
				if (atom.isGroup3(potentialGroupName))
				{
					if (bsResult == null)
						bsResult = new System.Collections.BitArray(((i + 1) % 64 == 0?(i + 1) / 64:(i + 1) / 64 + 1) * 64);
					SupportClass.BitArraySupport.Set(bsResult, i);
				}
			}
			return bsResult;
		}
		
		internal virtual void  notSet(System.Collections.BitArray bs)
		{
			for (int i = viewer.AtomCount; --i >= 0; )
			{
				if (bs.Get(i))
					bs.Set(i, false);
				else
					SupportClass.BitArraySupport.Set(bs, i);
			}
		}
		
		/*
		BitSet getResidueSet(String strResidue) {
		Frame frame = viewer.getFrame();
		BitSet bsResidue = new BitSet();
		for (int i = viewer.getAtomCount(); --i >= 0; ) {
		PdbAtom pdbatom = frame.getAtomAt(i).getPdbAtom();
		if (pdbatom != null && pdbatom.isResidue(strResidue))
		bsResidue.set(i);
		}
		return bsResidue;
		}
		*/
		
		internal virtual System.Collections.BitArray getSpecName(System.String resNameSpec)
		{
			System.Collections.BitArray bsRes = new System.Collections.BitArray(64);
			//    System.out.println("getSpecName:" + resNameSpec);
			Frame frame = viewer.Frame;
			for (int i = viewer.AtomCount; --i >= 0; )
			{
				Atom atom = frame.getAtomAt(i);
				if (atom.isGroup3Match(resNameSpec))
					SupportClass.BitArraySupport.Set(bsRes, i);
			}
			return bsRes;
		}
		
		internal virtual System.Collections.BitArray getSpecResid(int resid)
		{
			System.Collections.BitArray bsRes = new System.Collections.BitArray(64);
			Frame frame = viewer.Frame;
			for (int i = viewer.AtomCount; --i >= 0; )
			{
				Atom atom = frame.getAtomAt(i);
				if (atom.GroupID == resid)
					SupportClass.BitArraySupport.Set(bsRes, i);
			}
			return bsRes;
		}
		
		internal virtual System.Collections.BitArray getSpecSeqcode(int seqcode)
		{
			Frame frame = viewer.Frame;
			System.Collections.BitArray bsResno = new System.Collections.BitArray(64);
			for (int i = viewer.AtomCount; --i >= 0; )
			{
				Atom atom = frame.getAtomAt(i);
				if (seqcode == atom.Seqcode)
					SupportClass.BitArraySupport.Set(bsResno, i);
			}
			return bsResno;
		}
		
		internal virtual System.Collections.BitArray getSpecSeqcodeRange(int seqcodeA, int seqcodeB)
		{
			Frame frame = viewer.Frame;
			System.Collections.BitArray bsResidue = new System.Collections.BitArray(64);
			frame.selectSeqcodeRange(seqcodeA, seqcodeB, bsResidue);
			return bsResidue;
		}
		
		internal virtual System.Collections.BitArray getSpecChain(char chain)
		{
			bool caseSensitive = viewer.ChainCaseSensitive;
			if (!caseSensitive)
				chain = System.Char.ToUpper(chain);
			Frame frame = viewer.Frame;
			System.Collections.BitArray bsChain = new System.Collections.BitArray(64);
			for (int i = viewer.AtomCount; --i >= 0; )
			{
				char ch = frame.getAtomAt(i).ChainID;
				if (!caseSensitive)
					ch = System.Char.ToUpper(ch);
				if (chain == ch)
					SupportClass.BitArraySupport.Set(bsChain, i);
			}
			return bsChain;
		}
		
		internal virtual System.Collections.BitArray getSpecAtom(System.String atomSpec)
		{
			Frame frame = viewer.Frame;
			System.Collections.BitArray bsAtom = new System.Collections.BitArray(64);
			for (int i = viewer.AtomCount; --i >= 0; )
			{
				Atom atom = frame.getAtomAt(i);
				if (atom.isAtomNameMatch(atomSpec))
					SupportClass.BitArraySupport.Set(bsAtom, i);
			}
			return bsAtom;
		}
		
		internal virtual System.Collections.BitArray getResidueWildcard(System.String strWildcard)
		{
			Frame frame = viewer.Frame;
			System.Collections.BitArray bsResidue = new System.Collections.BitArray(64);
			//    System.out.println("getResidueWildcard:" + strWildcard);
			for (int i = viewer.AtomCount; --i >= 0; )
			{
				Atom atom = frame.getAtomAt(i);
				if (atom.isGroup3Match(strWildcard))
					SupportClass.BitArraySupport.Set(bsResidue, i);
			}
			return bsResidue;
		}
		
		internal virtual System.Collections.BitArray lookupValue(System.String variable, bool plurals)
		{
			if (logMessages)
				viewer.scriptStatus("lookupValue(" + variable + ")");
			System.Object value_Renamed = variables[variable];
			if (value_Renamed != null)
			{
				if (value_Renamed is Token[])
				{
					value_Renamed = expression((Token[]) value_Renamed, 2);
					variables[variable] = value_Renamed;
				}
				return (System.Collections.BitArray) value_Renamed;
			}
			if (plurals)
				return null;
			int len = variable.Length;
			if (len < 5)
			// iron is the shortest
				return null;
			if (variable[len - 1] != 's')
				return null;
			if (variable.EndsWith("ies"))
				variable = variable.Substring(0, (len - 3) - (0)) + 'y';
			else
				variable = variable.Substring(0, (len - 1) - (0));
			return lookupValue(variable, true);
		}
		
		internal virtual void  selectModelIndexAtoms(int modelIndex, System.Collections.BitArray bsResult)
		{
			Frame frame = viewer.Frame;
			for (int i = viewer.AtomCount; --i >= 0; )
				if (frame.getAtomAt(i).ModelIndex == modelIndex)
					SupportClass.BitArraySupport.Set(bsResult, i);
		}
		
		internal virtual System.Collections.BitArray getSpecAlternate(System.String alternateSpec)
		{
			System.Console.Out.WriteLine("getSpecAlternate(" + alternateSpec + ")");
			Frame frame = viewer.Frame;
			System.Collections.BitArray bs = new System.Collections.BitArray(64);
			for (int i = viewer.AtomCount; --i >= 0; )
			{
				Atom atom = frame.getAtomAt(i);
				if (atom.isAlternateLocationMatch(alternateSpec))
					SupportClass.BitArraySupport.Set(bs, i);
			}
			return bs;
		}
		
		internal virtual System.Collections.BitArray getSpecModel(System.String modelTag)
		{
			int modelNumber = - 1;
			try
			{
				modelNumber = System.Int32.Parse(modelTag);
			}
			catch (System.FormatException nfe)
			{
			}
			
			System.Collections.BitArray bsModel = new System.Collections.BitArray((viewer.AtomCount % 64 == 0?viewer.AtomCount / 64:viewer.AtomCount / 64 + 1) * 64);
			selectModelIndexAtoms(viewer.getModelNumberIndex(modelNumber), bsModel);
			return bsModel;
		}
		
		internal virtual void  comparatorInstruction(Token instruction, System.Collections.BitArray bs)
		{
			int comparator = instruction.tok;
			int property = instruction.intValue;
			float propertyValue = 0; // just for temperature
			int comparisonValue = ((System.Int32) instruction.value_Renamed);
			int numberOfAtoms = viewer.AtomCount;
			Frame frame = viewer.Frame;
			for (int i = 0; i < numberOfAtoms; ++i)
			{
				Atom atom = frame.getAtomAt(i);
				switch (property)
				{
					
					case Token.atomno: 
						propertyValue = atom.AtomNumber;
						break;
					
					case Token.elemno: 
						propertyValue = atom.ElementNumber;
						break;
					
					case Token.temperature: 
						propertyValue = atom.Bfactor100;
						if (propertyValue < 0)
							continue;
						propertyValue /= 100;
						break;
					
					case Token.occupancy: 
						propertyValue = atom.Occupancy;
						break;
					
					case Token.polymerLength: 
						propertyValue = atom.PolymerLength;
						break;
					
					case Token.resno: 
						propertyValue = atom.Resno;
						if (propertyValue == - 1)
							continue;
						break;
					
					case Token._groupID: 
						propertyValue = atom.GroupID;
						if (propertyValue < 0)
							continue;
						break;
					
					case Token._atomID: 
						propertyValue = atom.SpecialAtomID;
						if (propertyValue < 0)
							continue;
						break;
					
					case Token._structure: 
						propertyValue = getProteinStructureType(atom);
						if (propertyValue == - 1)
							continue;
						break;
					
					case Token.radius: 
						propertyValue = atom.RasMolRadius;
						break;
					
					case Token.bondcount: 
						propertyValue = atom.CovalentBondCount;
						break;
					
					case Token.hbondcount: 
						propertyValue = atom.HbondCount;
						break;
					
					case Token.model: 
						propertyValue = atom.ModelTagNumber;
						break;
					
					default: 
						unrecognizedAtomProperty(property);
						break;
					
				}
				bool match = false;
				switch (comparator)
				{
					
					case Token.opLT: 
						match = propertyValue < comparisonValue;
						break;
					
					case Token.opLE: 
						match = propertyValue <= comparisonValue;
						break;
					
					case Token.opGE: 
						match = propertyValue >= comparisonValue;
						break;
					
					case Token.opGT: 
						match = propertyValue > comparisonValue;
						break;
					
					case Token.opEQ: 
						match = propertyValue == comparisonValue;
						break;
					
					case Token.opNE: 
						match = propertyValue != comparisonValue;
						break;
					}
				if (match)
					SupportClass.BitArraySupport.Set(bs, i);
			}
		}
		
		
		internal virtual void  withinInstruction(Token instruction, System.Collections.BitArray bs, System.Collections.BitArray bsResult)
		{
			System.Object withinSpec = instruction.value_Renamed;
			if (withinSpec is System.Single)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				withinDistance((float) ((System.Single) withinSpec), bs, bsResult);
				return ;
			}
			if (withinSpec is System.String)
			{
				System.String withinStr = (System.String) withinSpec;
				if (withinStr.Equals("group"))
				{
					withinGroup(bs, bsResult);
					return ;
				}
				if (withinStr.Equals("chain"))
				{
					withinChain(bs, bsResult);
					return ;
				}
				if (withinStr.Equals("model"))
				{
					withinModel(bs, bsResult);
					return ;
				}
			}
			//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			evalError("Unrecognized within parameter:" + withinSpec);
		}
		
		internal virtual void  withinDistance(float distance, System.Collections.BitArray bs, System.Collections.BitArray bsResult)
		{
			Frame frame = viewer.Frame;
			for (int i = frame.AtomCount; --i >= 0; )
			{
				if (bs.Get(i))
				{
					Atom atom = frame.getAtomAt(i);
					AtomIterator iterWithin = frame.getWithinAnyModelIterator(atom, distance);
					while (iterWithin.hasNext())
						SupportClass.BitArraySupport.Set(bsResult, iterWithin.next().AtomIndex);
				}
			}
		}
		
		internal virtual void  withinGroup(System.Collections.BitArray bs, System.Collections.BitArray bsResult)
		{
			//    System.out.println("withinGroup");
			Frame frame = viewer.Frame;
			Group groupLast = null;
			for (int i = viewer.AtomCount; --i >= 0; )
			{
				if (!bs.Get(i))
					continue;
				Atom atom = frame.getAtomAt(i);
				Group group = atom.Group;
				if (group != groupLast)
				{
					group.selectAtoms(bsResult);
					groupLast = group;
				}
			}
		}
		
		internal virtual void  withinChain(System.Collections.BitArray bs, System.Collections.BitArray bsResult)
		{
			Frame frame = viewer.Frame;
			Chain chainLast = null;
			for (int i = viewer.AtomCount; --i >= 0; )
			{
				if (!bs.Get(i))
					continue;
				Atom atom = frame.getAtomAt(i);
				Chain chain = atom.Chain;
				if (chain != chainLast)
				{
					chain.selectAtoms(bsResult);
					chainLast = chain;
				}
			}
		}
		
		internal virtual void  withinModel(System.Collections.BitArray bs, System.Collections.BitArray bsResult)
		{
			Frame frame = viewer.Frame;
			int modelIndexLast = - 1;
			for (int i = viewer.AtomCount; --i >= 0; )
			{
				if (bs.Get(i))
				{
					int modelIndex = frame.getAtomAt(i).ModelIndex;
					if (modelIndex != modelIndexLast)
					{
						selectModelIndexAtoms(modelIndex, bsResult);
						modelIndexLast = modelIndex;
					}
				}
			}
		}
		
		internal virtual System.Collections.BitArray connected(Token instruction, System.Collections.BitArray bs)
		{
			int min = instruction.intValue;
			int max = ((System.Int32) instruction.value_Renamed);
			//UPGRADE_TODO: The equivalent in .NET for method 'java.util.BitSet.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			System.Console.Out.WriteLine("connected(" + min + "," + max + "," + SupportClass.BitArraySupport.ToString(bs) + ")");
			Frame frame = viewer.Frame;
			System.Collections.BitArray bsResult = new System.Collections.BitArray(64);
			for (int i = viewer.AtomCount; --i >= 0; )
			{
				int connectedCount = frame.getAtomAt(i).getConnectedCount(bs);
				if (connectedCount >= min && connectedCount <= max)
					SupportClass.BitArraySupport.Set(bsResult, i);
			}
			return bsResult;
		}
		
		internal virtual System.Collections.BitArray getSubstructureSet(System.String smiles)
		{
			PatternMatcher matcher = new PatternMatcher(viewer);
			try
			{
				return matcher.getSubstructureSet(smiles);
			}
			catch (InvalidSmilesException e)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				evalError(e.Message);
			}
			return null;
		}
		
		internal virtual int getProteinStructureType(Atom atom)
		{
			return atom.ProteinStructureType;
		}
		
		
		internal virtual int getArgbParam(int itoken)
		{
			if (itoken >= statementLength)
				colorExpected();
			if (statement[itoken].tok != Token.colorRGB)
				colorExpected();
			return statement[itoken].intValue;
		}
		
		internal virtual int getArgbOrNoneParam(int itoken)
		{
			if (itoken >= statementLength)
				colorExpected();
			if (statement[itoken].tok == Token.colorRGB)
				return statement[itoken].intValue;
			if (statement[itoken].tok != Token.none)
				colorExpected();
			return 0;
		}
		
		internal virtual void  background()
		{
			if (statementLength < 2 || statementLength > 3)
				badArgumentCount();
			int tok = statement[1].tok;
			if (tok == Token.colorRGB || tok == Token.none)
				viewer.setBackgroundArgb(getArgbOrNoneParam(1));
			else
				viewer.setShapePropertyArgb(getShapeType(tok), "bgcolor", getArgbOrNoneParam(2));
		}
		
		// mth - 2003 01
		// the doc for RasMol says that they use the center of gravity
		// this is currently only using the geometric center
		// but someplace in the rasmol doc it makes reference to the geometric
		// center as the default for rotations. who knows. 
		internal virtual void  center()
		{
			viewer.CenterBitSet = statementLength == 1?null:expression(statement, 1);
		}
		
		internal virtual void  color()
		{
			if (statementLength > 5 || statementLength < 2)
				badArgumentCount();
			int tok = statement[1].tok;
			switch (tok)
			{
				
				case Token.colorRGB: 
				case Token.none: 
				case Token.cpk: 
				case Token.amino: 
				case Token.chain: 
				case Token.group: 
				case Token.shapely: 
				case Token.structure: 
				case Token.temperature: 
				case Token.fixedtemp: 
				case Token.formalCharge: 
				case Token.partialCharge: 
				case Token.user: 
				case Token.monomer: 
				case Token.translucent: 
				case Token.opaque: 
					colorObject(Token.atom, 1);
					break;
				
				case Token.rubberband: 
					viewer.RubberbandArgb = getArgbParam(2);
					break;
				
				case Token.background: 
					viewer.setBackgroundArgb(getArgbOrNoneParam(2));
					break;
				
				case Token.identifier: 
				case Token.hydrogen: 
					System.String str = (System.String) statement[1].value_Renamed;
					int argb = getArgbOrNoneParam(2);
					if (str.ToUpper().Equals("dotsConvex".ToUpper()))
					{
						viewer.setShapePropertyArgb(JmolConstants.SHAPE_DOTS, "colorConvex", argb);
						return ;
					}
					if (str.ToUpper().Equals("dotsConcave".ToUpper()))
					{
						viewer.setShapePropertyArgb(JmolConstants.SHAPE_DOTS, "colorConcave", argb);
						return ;
					}
					if (str.ToUpper().Equals("dotsSaddle".ToUpper()))
					{
						viewer.setShapePropertyArgb(JmolConstants.SHAPE_DOTS, "colorSaddle", argb);
						return ;
					}
					if (str.ToUpper().Equals("selectionHalo".ToUpper()))
					{
						viewer.SelectionArgb = argb;
						return ;
					}
					for (int i = JmolConstants.elementNames.Length; --i >= 0; )
					{
						if (str.ToUpper().Equals(JmolConstants.elementNames[i].ToUpper()))
						{
							viewer.setElementArgb(i, getArgbParam(2));
							return ;
						}
					}
					for (int i = JmolConstants.alternateElementNames.Length; --i >= 0; )
					{
						if (str.ToUpper().Equals(JmolConstants.alternateElementNames[i].ToUpper()))
						{
							viewer.setElementArgb(JmolConstants.alternateElementNumbers[i], getArgbParam(2));
							return ;
						}
					}
					invalidArgument();
					goto default;
				
				
				default: 
					if (tok == Token.bond)
					// special hack for bond/bonds confusion
						tok = Token.bonds;
					for (int i = 0; i < shapeToks.Length; ++i)
						if (tok == shapeToks[i])
						{
							colorObject(tok, 2);
							//UPGRADE_NOTE: Labeled break statement was changed to a goto statement. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1012'"
							goto outer_brk;
						}
					invalidArgument();
					break;
				
			}
			//UPGRADE_NOTE: Label 'outer_brk' was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1011'"
outer_brk: ;
			
		}
		
		internal virtual void  colorObject(int tokObject, int itoken)
		{
			if (itoken >= statementLength)
				badArgumentCount();
			System.String translucentOrOpaque = null;
			System.Object colorvalue = null;
			int shapeType = getShapeType(tokObject);
			System.String colorOrBgcolor = "color";
			int tok = statement[itoken].tok;
			if (tok == Token.background)
			{
				colorOrBgcolor = "bgcolor";
				++itoken;
				tok = statement[itoken].tok;
			}
			if (tok == Token.translucent || tok == Token.opaque)
			{
				translucentOrOpaque = ((System.String) (statement[itoken].value_Renamed));
				++itoken;
			}
			if (itoken < statementLength)
			{
				colorvalue = statement[itoken].value_Renamed;
				tok = statement[itoken].tok;
				switch (tok)
				{
					
					case Token.none: 
					case Token.cpk: 
					case Token.formalCharge: 
					case Token.partialCharge: 
					case Token.structure: 
					case Token.amino: 
					case Token.shapely: 
					case Token.chain: 
					case Token.type: 
					case Token.temperature: 
					case Token.fixedtemp: 
						break;
					
					case Token.group: 
						viewer.calcSelectedGroupsCount();
						break;
					
					case Token.monomer: 
						viewer.calcSelectedMonomersCount();
						break;
					
					case Token.user: 
						notImplemented(itoken);
						return ;
					
					case Token.colorRGB: 
						int argb = getArgbParam(itoken);
						//UPGRADE_TODO: The 'System.Int32' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
						colorvalue = argb == 0?null:(System.Int32) argb;
						break;
					
					default: 
						invalidArgument();
						break;
					
				}
				viewer.loadShape(shapeType);
				viewer.setShapeProperty(shapeType, colorOrBgcolor, colorvalue);
			}
			if (translucentOrOpaque != null)
				viewer.setShapeProperty(shapeType, "translucency", translucentOrOpaque);
		}
		
		internal System.Collections.Hashtable variables = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
		internal virtual void  define()
		{
			System.String variable = (System.String) statement[1].value_Renamed;
			variables[variable] = (expression(statement, 2));
		}
		
		internal virtual void  predefine(Token[] statement)
		{
			System.String variable = (System.String) statement[1].value_Renamed;
			variables[variable] = statement;
		}
		
		internal bool echoShapeActive = false;
		
		internal virtual void  echo()
		{
			System.String text = "";
			if (statementLength == 2 && statement[1].tok == Token.string_Renamed)
				text = ((System.String) statement[1].value_Renamed);
			if (echoShapeActive)
				viewer.setShapeProperty(JmolConstants.SHAPE_ECHO, "echo", text);
			viewer.scriptEcho(text);
		}
		
		internal virtual void  label()
		{
			System.String strLabel = (System.String) statement[1].value_Renamed;
			if (strLabel.ToUpper().Equals("on".ToUpper()))
			{
				// from the RasMol 2.6b2 manual: RasMol uses the label
				// "%n%r:%c.%a" if the molecule contains more than one chain:
				// "%e%i" if the molecule has only a single residue (a small molecule) and
				// "%n%r.%a" otherwise.
				if (viewer.ModelCount > 1)
					strLabel = "[%n]%r:%c.%a/%M";
				else if (viewer.ChainCount > 1)
					strLabel = "[%n]%r:%c.%a";
				else if (viewer.GroupCount <= 1)
					strLabel = "%e%i";
				else
					strLabel = "[%n]%r.%a";
			}
			else if (strLabel.ToUpper().Equals("off".ToUpper()))
				strLabel = null;
			viewer.loadShape(JmolConstants.SHAPE_LABELS);
			viewer.Label = strLabel;
		}
		
		internal virtual void  hover()
		{
			System.String strLabel = (System.String) statement[1].value_Renamed;
			if (strLabel.ToUpper().Equals("on".ToUpper()))
				strLabel = "%U";
			else if (strLabel.ToUpper().Equals("off".ToUpper()))
				strLabel = null;
			viewer.loadShape(JmolConstants.SHAPE_HOVER);
			viewer.setShapeProperty(JmolConstants.SHAPE_HOVER, "label", strLabel);
		}
		
		internal virtual void  load()
		{
			int i = 1;
			// ignore optional file format
			if (statement[i].tok == Token.identifier)
				++i;
			if (statement[i].tok != Token.string_Renamed)
				filenameExpected();
			//long timeBegin = System.currentTimeMillis();
			if (statementLength == i + 1)
			{
				System.String filename = (System.String) statement[i].value_Renamed;
				viewer.openFile(filename);
			}
			else
			{
				System.String modelName = (System.String) statement[i].value_Renamed;
				i++;
				System.String[] filenames = new System.String[statementLength - i];
				while (i < statementLength)
				{
					filenames[filenames.Length - statementLength + i] = ((System.String) statement[i].value_Renamed);
					i++;
				}
				viewer.openFiles(modelName, filenames);
			}
			System.String errMsg = viewer.OpenFileError;
			//int millis = (int)(System.currentTimeMillis() - timeBegin);
			//    System.out.println("!!!!!!!!! took " + millis + " ms");
			if (errMsg != null)
				evalError(errMsg);
			if (logMessages)
				viewer.scriptStatus("Successfully loaded:" + filename);
		}
		
		internal int[] monitorArgs = new int[5];
		
		internal virtual void  monitor()
		{
			if (statementLength == 1)
			{
				viewer.ShowMeasurements = true;
				return ;
			}
			if (statementLength == 2)
			{
				if (statement[1].tok == Token.on)
					viewer.ShowMeasurements = true;
				else if (statement[1].tok == Token.off)
					viewer.clearMeasurements();
				else
					booleanExpected();
				return ;
			}
			if (statementLength < 3 || statementLength > 5)
				badArgumentCount();
			for (int i = 1; i < statementLength; ++i)
			{
				if (statement[i].tok != Token.integer)
					integerExpected();
			}
			int argCount = monitorArgs[0] = statementLength - 1;
			//int numAtoms = viewer.getAtomCount();
			for (int i = 0; i < argCount; ++i)
			{
				Token token = statement[i + 1];
				if (token.tok != Token.integer)
					integerExpected();
				int atomNumber = token.intValue;
				int atomIndex = viewer.getAtomIndexFromAtomNumber(atomNumber);
				if (atomIndex == - 1)
					badAtomNumber();
				monitorArgs[i + 1] = atomIndex;
			}
			viewer.toggleMeasurement(monitorArgs);
		}
		
		internal virtual void  refresh()
		{
			viewer.requestRepaintAndWait();
		}
		
		internal virtual void  reset()
		{
			viewer.homePosition();
		}
		
		internal virtual void  restrict()
		{
			select();
			viewer.invertSelection();
			bool bondmode = viewer.BondSelectionModeOr;
			viewer.BondSelectionModeOr = true;
			viewer.setShapeSize(JmolConstants.SHAPE_STICKS, 0);
			
			// also need to turn off backbones, ribbons, strands, cartoons
			for (int shapeType = JmolConstants.SHAPE_MIN_SELECTION_INDEPENDENT; --shapeType >= 0; )
				viewer.setShapeSize(shapeType, 0);
			
			viewer.Label = null;
			
			viewer.BondSelectionModeOr = bondmode;
			viewer.invertSelection();
		}
		
		internal virtual void  rotate()
		{
			if (statement.Length > 3 && statement[1].tok == Token.axisangle)
			{
				checkStatementLength(6);
				viewer.rotateAxisAngle(floatParameter(2), floatParameter(3), floatParameter(4), floatParameter(5));
				return ;
			}
			checkLength3();
			float degrees = floatParameter(2);
			switch (statement[1].tok)
			{
				
				case Token.x: 
					viewer.rotateXDegrees(degrees);
					break;
				
				case Token.y: 
					viewer.rotateYDegrees(degrees);
					break;
				
				case Token.z: 
					viewer.rotateZDegreesScript(degrees);
					break;
				
				default: 
					axisExpected();
					break;
				
			}
		}
		
		internal virtual void  pushContext()
		{
			if (scriptLevel == scriptLevelMax)
				evalError("too many script levels");
			Context context = new Context();
			context.filename = filename;
			context.script = script_Renamed_Field;
			context.linenumbers = linenumbers;
			context.lineIndices = lineIndices;
			context.aatoken = aatoken;
			context.pc = pc;
			stack[scriptLevel++] = context;
		}
		
		internal virtual void  popContext()
		{
			if (scriptLevel == 0)
				evalError("RasMol virtual machine error - stack underflow");
			Context context = stack[--scriptLevel];
			stack[scriptLevel] = null;
			filename = context.filename;
			script_Renamed_Field = context.script;
			linenumbers = context.linenumbers;
			lineIndices = context.lineIndices;
			aatoken = context.aatoken;
			pc = context.pc;
		}
		internal virtual void  script()
		{
			// token allows for only 1 parameter
			pushContext();
			System.String filename = (System.String) statement[1].value_Renamed;
			if (!loadScriptFileInternal(filename))
				errorLoadingScript(errorMessage);
			instructionDispatchLoop();
			popContext();
		}
		
		internal virtual void  select()
		{
			// NOTE this is called by restrict()
			if (statementLength == 1)
			{
				viewer.selectAll();
				if (!viewer.RasmolHydrogenSetting)
					viewer.excludeSelectionSet(HydrogenSet);
				if (!viewer.RasmolHeteroSetting)
					viewer.excludeSelectionSet(HeteroSet);
			}
			else
			{
				viewer.SelectionSet = expression(statement, 1);
			}
			viewer.scriptStatus("" + viewer.SelectionCount + " atoms selected");
		}
		
		internal virtual void  translate()
		{
			if (statementLength < 3)
				badArgumentCount();
			if (statement[2].tok != Token.integer)
				integerExpected();
			int percent = statement[2].intValue;
			if (percent > 100 || percent < - 100)
				numberOutOfRange(- 100, 100);
			switch (statement[1].tok)
			{
				
				case Token.x: 
					viewer.translateToXPercent(percent);
					break;
				
				case Token.y: 
					viewer.translateToYPercent(percent);
					break;
				
				case Token.z: 
					viewer.translateToZPercent(percent);
					break;
				
				default: 
					axisExpected();
					break;
				
			}
		}
		
		internal virtual void  zap()
		{
			viewer.clear();
		}
		
		internal virtual void  zoom()
		{
			// token has ondefault1
			if (statement[1].tok == Token.integer)
			{
				int percent = statement[1].intValue;
				if (percent < 5 || percent > Viewer.MAXIMUM_ZOOM_PERCENTAGE)
					numberOutOfRange(5, Viewer.MAXIMUM_ZOOM_PERCENTAGE);
				viewer.zoomToPercent(percent);
				return ;
			}
			switch (statement[1].tok)
			{
				
				case Token.on: 
					viewer.ZoomEnabled = true;
					break;
				
				case Token.off: 
					viewer.ZoomEnabled = false;
					break;
				
				default: 
					booleanOrPercentExpected();
					break;
				
			}
		}
		
		internal virtual void  delay()
		{
			long timeBegin = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
			long millis = 0;
			//token has ondefault1
			Token token = statement[1];
			switch (token.tok)
			{
				
				case Token.integer: 
				case Token.on:  // this is auto-provided as a default
					millis = token.intValue * 1000;
					break;
				
				case Token.decimal_Renamed: 
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					millis = (long) ((float) ((System.Single) token.value_Renamed) * 1000);
					break;
				
				default: 
					numberExpected();
					break;
				
			}
			viewer.requestRepaintAndWait();
			millis -= ((System.DateTime.Now.Ticks - 621355968000000000) / 10000 - timeBegin);
			if (millis > 0)
			{
				viewer.popHoldRepaint();
				try
				{
					//UPGRADE_TODO: Method 'java.lang.Thread.sleep' was converted to 'System.Threading.Thread.Sleep' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangThreadsleep_long'"
					System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64) 10000 * millis));
				}
				catch (System.Threading.ThreadInterruptedException e)
				{
				}
				viewer.pushHoldRepaint();
			}
		}
		
		internal virtual void  move()
		{
			if (statementLength < 10 || statementLength > 12)
				badArgumentCount();
			float dRotX = floatParameter(1);
			float dRotY = floatParameter(2);
			float dRotZ = floatParameter(3);
			int dZoom = intParameter(4);
			int dTransX = intParameter(5);
			int dTransY = intParameter(6);
			int dTransZ = intParameter(7);
			int dSlab = intParameter(8);
			float floatSecondsTotal = floatParameter(9);
			int fps = 30;
			if (statementLength > 10)
			{
				fps = statement[10].intValue;
				if (statementLength > 11)
				{
					//maxAccel = statement[11].intValue;
				}
			}
			
			int zoom = viewer.ZoomPercent;
			int slab = viewer.SlabPercentSetting;
			float transX = viewer.TranslationXPercent;
			float transY = viewer.TranslationYPercent;
			float transZ = viewer.TranslationZPercent;
			
			long timeBegin = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
			int timePerStep = 1000 / fps;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			int totalSteps = (int) (fps * floatSecondsTotal);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			float radiansPerDegreePerStep = (float) System.Math.PI / 180 / totalSteps;
			float radiansXStep = radiansPerDegreePerStep * dRotX;
			float radiansYStep = radiansPerDegreePerStep * dRotY;
			float radiansZStep = radiansPerDegreePerStep * dRotZ;
			viewer.InMotion = true;
			if (totalSteps == 0)
				totalSteps = 1; // to catch a zero secondsTotal parameter
			for (int i = 1; i <= totalSteps && !interruptExecution; ++i)
			{
				if (dRotX != 0)
					viewer.rotateXRadians(radiansXStep);
				if (dRotY != 0)
					viewer.rotateYRadians(radiansYStep);
				if (dRotZ != 0)
					viewer.rotateZRadians(radiansZStep);
				if (dZoom != 0)
					viewer.zoomToPercent(zoom + dZoom * i / totalSteps);
				if (dTransX != 0)
					viewer.translateToXPercent(transX + dTransX * i / totalSteps);
				if (dTransY != 0)
					viewer.translateToYPercent(transY + dTransY * i / totalSteps);
				if (dTransZ != 0)
					viewer.translateToZPercent(transZ + dTransZ * i / totalSteps);
				if (dSlab != 0)
					viewer.slabToPercent(slab + dSlab * i / totalSteps);
				int timeSpent = (int) ((System.DateTime.Now.Ticks - 621355968000000000) / 10000 - timeBegin);
				int timeAllowed = i * timePerStep;
				if (timeSpent < timeAllowed)
				{
					viewer.requestRepaintAndWait();
					timeSpent = (int) ((System.DateTime.Now.Ticks - 621355968000000000) / 10000 - timeBegin);
					int timeToSleep = timeAllowed - timeSpent;
					if (timeToSleep > 0)
					{
						try
						{
							//UPGRADE_TODO: Method 'java.lang.Thread.sleep' was converted to 'System.Threading.Thread.Sleep' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangThreadsleep_long'"
							System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64) 10000 * timeToSleep));
						}
						catch (System.Threading.ThreadInterruptedException e)
						{
						}
					}
				}
			}
			viewer.InMotion = false;
		}
		
		internal virtual void  slab()
		{
			//token has ondefault1
			if (statement[1].tok == Token.integer)
			{
				int percent = statement[1].intValue;
				if (percent < 0 || percent > 100)
					numberOutOfRange(0, 100);
				viewer.slabToPercent(percent);
				return ;
			}
			switch (statement[1].tok)
			{
				
				case Token.on: 
					viewer.SlabEnabled = true;
					break;
				
				case Token.off: 
					viewer.SlabEnabled = false;
					break;
				
				default: 
					booleanOrPercentExpected();
					break;
				
			}
		}
		
		internal virtual void  depth()
		{
			viewer.depthToPercent(intParameter(1));
		}
		
		internal virtual void  star()
		{
			short mad = 0;
			int tok = Token.on;
			if (statementLength > 1)
			{
				tok = statement[1].tok;
				if (!((statementLength == 2) || (statementLength == 3 && tok == Token.integer && statement[2].tok == Token.percent)))
				{
					badArgumentCount();
				}
			}
			switch (tok)
			{
				
				case Token.on: 
				case Token.vanderwaals: 
					mad = - 100; // cpk with no args goes to 100%
					break;
				
				case Token.off: 
					break;
				
				case Token.integer: 
					int radiusRasMol = statement[1].intValue;
					if (statementLength == 2)
					{
						if (radiusRasMol >= 750 || radiusRasMol < - 100)
							numberOutOfRange(- 100, 749);
						mad = (short) radiusRasMol;
						if (radiusRasMol > 0)
							mad = (short) (mad * 4 * 2);
					}
					else
					{
						if (radiusRasMol < 0 || radiusRasMol > 100)
							numberOutOfRange(0, 100);
						mad = (short) (- radiusRasMol); // use a negative number to specify %vdw
					}
					break;
				
				case Token.decimal_Renamed: 
					float angstroms = floatParameter(1);
					if (angstroms < 0 || angstroms > 3)
						numberOutOfRange(0f, 3f);
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					mad = (short) (angstroms * 1000 * 2);
					break;
				
				case Token.temperature: 
					mad = - 1000;
					break;
				
				case Token.ionic: 
					mad = - 1001;
					break;
				
				default: 
					booleanOrNumberExpected();
					break;
				
			}
			viewer.setShapeSize(JmolConstants.SHAPE_STARS, mad);
		}
		
		internal virtual void  cpk()
		{
			short mad = 0;
			int tok = Token.on;
			if (statementLength > 1)
			{
				tok = statement[1].tok;
				if (!((statementLength == 2) || (statementLength == 3 && tok == Token.integer && statement[2].tok == Token.percent)))
				{
					badArgumentCount();
				}
			}
			switch (tok)
			{
				
				case Token.on: 
				case Token.vanderwaals: 
					mad = - 100; // cpk with no args goes to 100%
					break;
				
				case Token.off: 
					break;
				
				case Token.integer: 
					int radiusRasMol = statement[1].intValue;
					if (statementLength == 2)
					{
						if (radiusRasMol >= 750 || radiusRasMol < - 200)
							numberOutOfRange(- 200, 749);
						mad = (short) radiusRasMol;
						if (radiusRasMol > 0)
							mad = (short) (mad * 4 * 2);
					}
					else
					{
						if (radiusRasMol < 0 || radiusRasMol > 200)
							numberOutOfRange(0, 200);
						mad = (short) (- radiusRasMol); // use a negative number to specify %vdw
					}
					break;
				
				case Token.decimal_Renamed: 
					float angstroms = floatParameter(1);
					if (angstroms < 0 || angstroms > 3)
						numberOutOfRange(0f, 3f);
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					mad = (short) (angstroms * 1000 * 2);
					break;
				
				case Token.temperature: 
					mad = - 1000;
					break;
				
				case Token.ionic: 
					mad = - 1001;
					break;
				
				default: 
					booleanOrNumberExpected();
					break;
				
			}
			viewer.setShapeSize(JmolConstants.SHAPE_BALLS, mad);
		}
		
		internal virtual void  wireframe()
		{
			viewer.setShapeSize(JmolConstants.SHAPE_STICKS, MadParameter);
		}
		
		internal virtual void  ssbond()
		{
			viewer.loadShape(JmolConstants.SHAPE_SSSTICKS);
			viewer.setShapeSize(JmolConstants.SHAPE_SSSTICKS, MadParameter);
		}
		
		internal virtual void  hbond()
		{
			viewer.loadShape(JmolConstants.SHAPE_HSTICKS);
			if (statementLength == 2 && statement[1].tok == Token.identifier && ((System.String) statement[1].value_Renamed).ToUpper().Equals("calculate".ToUpper()))
			{
				System.Collections.BitArray bs = viewer.SelectionSet;
				viewer.Frame.autoHbond(bs, bs);
				return ;
			}
			viewer.setShapeSize(JmolConstants.SHAPE_HSTICKS, MadParameter);
		}
		
		internal virtual void  vector()
		{
			short mad = 1;
			if (statementLength > 1)
			{
				switch (statement[1].tok)
				{
					
					case Token.on: 
						break;
					
					case Token.off: 
						mad = 0;
						break;
					
					case Token.integer: 
						int diameterPixels = statement[1].intValue;
						if (diameterPixels < 0 || diameterPixels >= 20)
							numberOutOfRange(0, 19);
						mad = (short) diameterPixels;
						break;
					
					case Token.decimal_Renamed: 
						float angstroms = floatParameter(1);
						if (angstroms < 0 || angstroms > 3)
							numberOutOfRange(0f, 3f);
						//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
						mad = (short) (angstroms * 1000 * 2);
						break;
					
					case Token.identifier: 
						System.String cmd = (System.String) statement[1].value_Renamed;
						if (!cmd.ToUpper().Equals("scale".ToUpper()))
							unrecognizedSubcommand();
						vectorScale();
						return ;
					
					default: 
						booleanOrNumberExpected();
						break;
					
				}
				checkLength2();
			}
			viewer.setShapeSize(JmolConstants.SHAPE_VECTORS, mad);
		}
		
		internal virtual void  vectorScale()
		{
			checkLength3();
			float scale = floatParameter(2);
			if (scale < - 10 || scale > 10)
				numberOutOfRange(- 10f, 10f);
			viewer.setVectorScale(scale);
		}
		
		internal virtual void  animation()
		{
			if (statementLength < 2)
				subcommandExpected();
			int tok = statement[1].tok;
			bool animate = false;
			switch (tok)
			{
				
				case Token.on: 
					animate = true;
					goto case Token.off;
				
				case Token.off: 
					viewer.setAnimationOn(animate);
					break;
				
				case Token.information: 
					showAnimation();
					break;
				
				case Token.frame: 
					frame(2);
					break;
				
				case Token.mode: 
					animationMode();
					break;
				
				case Token.direction: 
					animationDirection();
					break;
				
				case Token.fps: 
					viewer.AnimationFps = SetInteger;
					break;
				
				default: 
					unrecognizedSubcommand();
					break;
				
			}
		}
		
		internal virtual void  animationMode()
		{
			float startDelay = 1, endDelay = 1;
			if (statementLength < 3 || statementLength > 5)
				badArgumentCount();
			int animationMode = 0;
			switch (statement[2].tok)
			{
				
				case Token.loop: 
					++animationMode;
					break;
				
				case Token.identifier: 
					System.String cmd = (System.String) statement[2].value_Renamed;
					if (cmd.ToUpper().Equals("once".ToUpper()))
					{
						startDelay = endDelay = 0;
						break;
					}
					if (cmd.ToUpper().Equals("palindrome".ToUpper()))
					{
						animationMode = 2;
						break;
					}
					unrecognizedSubcommand();
					break;
				}
			if (statementLength >= 4)
			{
				startDelay = endDelay = floatParameter(3);
				if (statementLength == 5)
					endDelay = floatParameter(4);
			}
			viewer.setAnimationReplayMode(animationMode, startDelay, endDelay);
		}
		
		internal virtual void  vibration()
		{
			if (statementLength < 2)
				subcommandExpected();
			Token token = statement[1];
			float period = 0;
			switch (token.tok)
			{
				
				case Token.off: 
				case Token.on: 
				case Token.integer: 
					period = token.intValue;
					break;
				
				case Token.decimal_Renamed: 
					period = floatParameter(1);
					break;
				
				case Token.identifier: 
					System.String cmd = (System.String) statement[1].value_Renamed;
					if (cmd.ToUpper().Equals("scale".ToUpper()))
					{
						vibrationScale();
						return ;
					}
					goto default;
				
				default: 
					unrecognizedSubcommand();
					break;
				
			}
			viewer.VibrationPeriod = period;
		}
		
		internal virtual void  vibrationScale()
		{
			checkLength3();
			float scale = floatParameter(2);
			if (scale < - 10 || scale > 10)
				numberOutOfRange(- 10f, 10f);
			viewer.setVibrationScale(scale);
		}
		
		internal virtual void  animationDirection()
		{
			checkStatementLength(4);
			bool negative = false;
			if (statement[2].tok == Token.hyphen)
				negative = true;
			else if (statement[2].tok != Token.plus)
				invalidArgument();
			
			if (statement[3].tok != Token.integer)
				invalidArgument();
			int direction = statement[3].intValue;
			if (direction != 1)
				numberMustBe(1, - 1);
			if (negative)
				direction = - direction;
			viewer.AnimationDirection = direction;
		}
		
		
		/*
		void animate() throws ScriptException {
		if (statement.length < 2 || statement[1].tok != Token.identifier)
		unrecognizedSubcommand();
		String cmd = (String)statement[1].value;
		if (cmd.equalsIgnoreCase("frame")) {
		if (statement.length != 3 || statement[2].tok != Token.integer)
		integerExpected();
		int frame = statement[2].intValue;
		if (frame < 0 || frame >= viewer.getNumberOfFrames()) 
		numberOutOfRange();
		viewer.setFrame(frame);
		} else if (cmd.equalsIgnoreCase("next")) {
		int frame = viewer.getCurrentFrameNumber() + 1;
		if (frame < viewer.getNumberOfFrames())
		viewer.setFrame(frame);
		} else if (cmd.equalsIgnoreCase("prev")) {
		int frame = viewer.getCurrentFrameNumber() - 1;
		if (frame >= 0)
		viewer.setFrame(frame);
		} else if (cmd.equalsIgnoreCase("nextwrap")) {
		int frame = viewer.getCurrentFrameNumber() + 1;
		if (frame >= viewer.getNumberOfFrames())
		frame = 0;
		viewer.setFrame(frame);
		} else if (cmd.equalsIgnoreCase("prevwrap")) {
		int frame = viewer.getCurrentFrameNumber() - 1;
		if (frame < 0)
		frame = viewer.getNumberOfFrames() - 1;
		viewer.setFrame(frame);
		} else if (cmd.equalsIgnoreCase("play")) {
		animatePlay(true);
		} else if (cmd.equalsIgnoreCase("revplay")) {
		animatePlay(false);
		} else if (cmd.equalsIgnoreCase("rewind")) {
		viewer.setFrame(0);
		} else {
		unrecognizedSubcommand();
		}
		}
		
		void animatePlay(boolean forward) {
		int nframes = viewer.getNumberOfFrames();
		long timeBegin = System.currentTimeMillis();
		long targetTime = timeBegin;
		int frameTimeMillis = 100;
		int frameBegin, frameEnd, frameDelta;
		if (forward) {
		frameBegin = 0;
		frameEnd = nframes;
		frameDelta = 1;
		} else {
		frameBegin = nframes - 1;
		frameEnd = -1;
		frameDelta = -1;
		}
		viewer.setInMotion(true);
		for (int frame = frameBegin; frame != frameEnd; frame += frameDelta) {
		viewer.setFrame(frame);
		refresh();
		targetTime += frameTimeMillis;
		long sleepTime = targetTime - System.currentTimeMillis();
		if (sleepTime > 0) {
		try {
		Thread.sleep(sleepTime);
		} catch (InterruptedException ie) {
		}
		}
		}
		viewer.setInMotion(false);
		}
		*/
		
		internal virtual void  dots()
		{
			// token has onDefault1
			short mad = 0;
			switch (statement[1].tok)
			{
				
				case Token.on: 
				case Token.vanderwaals: 
					mad = 1;
					break;
				
				case Token.ionic: 
					mad = - 1;
					break;
				
				case Token.off: 
					break;
				
				case Token.integer: 
					int dotsParam = statement[1].intValue;
					if (dotsParam < 0 || dotsParam > 1000)
						numberOutOfRange(0, 1000);
					// I don't know what to do with this thing yet
					mad = (short) dotsParam;
					break;
				
				default: 
					booleanOrNumberExpected();
					break;
				
			}
			viewer.setShapeSize(JmolConstants.SHAPE_DOTS, mad);
		}
		
		internal virtual void  proteinShape(int shapeType)
		{
			short mad = 0;
			//token has ondefault1
			int tok = statement[1].tok;
			switch (tok)
			{
				
				case Token.on: 
					mad = - 1; // means take default
					break;
				
				case Token.off: 
					break;
				
				case Token.structure: 
					mad = - 2;
					break;
				
				case Token.temperature: 
				// MTH 2004 03 15
				// Let temperature return the mean positional displacement
				// see what people think
				//      mad = -3;
				//      break;
				case Token.displacement: 
					mad = - 4;
					break;
				
				case Token.integer: 
					int radiusRasMol = statement[1].intValue;
					//currently not possible to get here with < 0, but that may change
					//this redundancy is safer
					if (radiusRasMol < 0 || radiusRasMol >= 500)
						numberOutOfRange(0, 499);
					mad = (short) (radiusRasMol * 4 * 2);
					break;
				
				case Token.decimal_Renamed: 
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					float angstroms = (float) ((System.Single) statement[1].value_Renamed);
					if (angstroms < 0 || angstroms > 4)
						numberOutOfRange(0f, 4f);
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					mad = (short) (angstroms * 1000 * 2);
					break;
				
				default: 
					booleanOrNumberExpected();
					break;
				
			}
			viewer.setShapeSize(shapeType, mad);
		}
		
		internal virtual void  spin()
		{
			bool spinOn = false;
			switch (statement[1].tok)
			{
				
				case Token.on: 
					spinOn = true;
					goto case Token.off;
				
				case Token.off: 
					break;
				
				default: 
					booleanExpected();
					break;
				
			}
			viewer.SpinOn = spinOn;
		}
		
		internal virtual void  frame()
		{
			frame(1);
		}
		
		internal virtual void  frame(int offset)
		{
			if (statementLength <= offset)
				badArgumentCount();
			if (statement[offset].tok == Token.hyphen)
			{
				++offset;
				checkStatementLength(offset + 1);
				if (statement[offset].tok != Token.integer || statement[offset].intValue != 1)
					invalidArgument();
				viewer.setAnimationPrevious();
				return ;
			}
			if (statementLength != offset + 1)
				badArgumentCount();
			int modelNumber = - 1;
			switch (statement[offset].tok)
			{
				
				case Token.all: 
				case Token.asterisk: 
					break;
				
				case Token.none: 
					break;
				
				case Token.integer: 
					modelNumber = statement[offset].intValue;
					break;
				
				case Token.identifier: 
					System.String ident = (System.String) statement[offset].value_Renamed;
					if (ident.ToUpper().Equals("next".ToUpper()))
					{
						viewer.setAnimationNext();
						return ;
					}
					if (ident.ToUpper().Equals("prev".ToUpper()))
					{
						viewer.setAnimationPrevious();
						return ;
					}
					if (ident.ToUpper().Equals("play".ToUpper()))
					{
						viewer.setAnimationOn(true, viewer.getDisplayModelIndex());
						return ;
					}
					break;
				
				default: 
					invalidArgument();
					break;
				
			}
			int modelIndex = viewer.getModelNumberIndex(modelNumber);
			viewer.setDisplayModelIndex(modelIndex);
		}
		
		// note that this array *MUST* be in the same sequence as the
		// SHAPE_* constants in JmolConstants
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'shapeToks '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_NOTE: The initialization of  'shapeToks' was moved to static method 'org.jmol.viewer.Eval'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private static readonly int[] shapeToks;
		
		internal virtual int getShapeType(int tok)
		{
			for (int i = shapeToks.Length; --i >= 0; )
				if (tok == shapeToks[i])
					return i;
			unrecognizedColorObject();
			return - 1;
		}
		
		internal virtual void  font()
		{
			int shapeType = 0;
			int fontsize = 0;
			System.String fontface = "SansSerif";
			System.String fontstyle = "Plain";
			switch (statementLength)
			{
				
				case 5: 
					if (statement[4].tok != Token.identifier)
						keywordExpected();
					fontstyle = ((System.String) statement[4].value_Renamed);
					goto case 4;
				
				case 4: 
					if (statement[3].tok != Token.identifier)
						keywordExpected();
					fontface = ((System.String) statement[3].value_Renamed);
					goto case 3;
				
				case 3: 
					if (statement[2].tok != Token.integer)
						integerExpected();
					fontsize = statement[2].intValue;
					shapeType = getShapeType(statement[1].tok);
					break;
				
				default: 
					badArgumentCount();
					break;
				
			}
			/*
			System.out.println("font <obj> fontsize=" + fontsize);
			System.out.println("fontface=" + fontface + " fontstyle=" + fontstyle);
			*/
			Font3D font3d = viewer.getFont3D(fontface, fontstyle, fontsize);
			viewer.setShapeProperty(shapeType, "font", font3d);
		}
		
		/*==============================================================*
		* SET implementations
		*==============================================================*/
		
		internal virtual void  set_Renamed()
		{
			switch (statement[1].tok)
			{
				
				case Token.axes: 
					setAxes();
					break;
				
				case Token.bondmode: 
					setBondmode();
					break;
				
				case Token.bonds: 
					setBonds();
					break;
				
				case Token.boundbox: 
					setBoundbox();
					break;
				
				case Token.color: 
					System.Console.Out.WriteLine("WARNING! use 'set defaultColors' not 'set color'");
					// fall into
					goto case Token.defaultColors;
				
				case Token.defaultColors: 
					setDefaultColors();
					break;
				
				case Token.debugscript: 
					setDebugScript();
					break;
				
				case Token.display: 
					setDisplay();
					break;
				
				case Token.echo: 
					setEcho();
					break;
				
				case Token.fontsize: 
					setFontsize();
					break;
				
				case Token.frank: 
					setFrank();
					break;
				
				case Token.hetero: 
					setHetero();
					break;
				
				case Token.hydrogen: 
					setHydrogen();
					break;
				
				case Token.labeloffset: 
					setLabelOffset();
					break;
				
				case Token.monitor: 
					setMonitor();
					break;
				
				case Token.property: 
					setProperty();
					break;
				
				case Token.solvent: 
					setSolvent();
					break;
				
				case Token.radius: 
					setRadius();
					break;
				
				case Token.strands: 
					setStrands();
					break;
				
				case Token.specular: 
					setSpecular();
					break;
				
				case Token.specpower: 
					setSpecPower();
					break;
				
				case Token.ambient: 
					setAmbient();
					break;
				
				case Token.diffuse: 
					setDiffuse();
					break;
				
				case Token.spin: 
					setSpin();
					break;
				
				case Token.ssbond: 
					setSsbond();
					break;
				
				case Token.hbond: 
					setHbond();
					break;
				
				case Token.scale3d: 
					setScale3d();
					break;
				
				case Token.unitcell: 
					setUnitcell();
					break;
				
				case Token.picking: 
					setPicking();
					break;
					// not implemented
				
				case Token.backfade: 
				case Token.cartoon: 
				case Token.hourglass: 
				case Token.kinemage: 
				case Token.menus: 
				case Token.mouse: 
				case Token.shadow: 
				case Token.slabmode: 
				case Token.transparent: 
				case Token.vectps: 
				case Token.write: 
				case Token.formalCharge:  // set charge in Chime
					notImplemented(1);
					break;
				
				case Token.identifier: 
					viewer.setBooleanProperty((System.String) statement[1].value_Renamed, SetBoolean);
					break;
				
				case Token.background: 
				case Token.stereo: 
					setspecialShouldNotBeHere();
					goto default;
				
				default: 
					unrecognizedSetParameter();
					break;
				
			}
		}
		
		internal virtual void  setAxes()
		{
			viewer.setShapeSize(JmolConstants.SHAPE_AXES, SetAxesTypeMad);
		}
		
		internal virtual void  setBoundbox()
		{
			viewer.setShapeSize(JmolConstants.SHAPE_BBCAGE, SetAxesTypeMad);
		}
		
		internal virtual void  setUnitcell()
		{
			viewer.setShapeSize(JmolConstants.SHAPE_UCCAGE, SetAxesTypeMad);
		}
		
		internal virtual void  setFrank()
		{
			viewer.setShapeSize(JmolConstants.SHAPE_FRANK, SetAxesTypeMad);
		}
		
		internal virtual void  setDefaultColors()
		{
			checkLength3();
			switch (statement[2].tok)
			{
				
				case Token.rasmol: 
				case Token.jmol: 
					viewer.DefaultColors = ((System.String) statement[2].value_Renamed);
					break;
				
				default: 
					invalidArgument();
					break;
				
			}
		}
		
		internal virtual void  setBondmode()
		{
			checkLength3();
			bool bondmodeOr = false;
			switch (statement[2].tok)
			{
				
				case Token.opAnd: 
					break;
				
				case Token.opOr: 
					bondmodeOr = true;
					break;
				
				default: 
					invalidArgument();
					break;
				
			}
			viewer.BondSelectionModeOr = bondmodeOr;
		}
		
		internal virtual void  setBonds()
		{
			viewer.ShowMultipleBonds = SetBoolean;
		}
		
		internal virtual void  setDisplay()
		{
			bool showHalo = false;
			checkLength3();
			switch (statement[2].tok)
			{
				
				case Token.selected: 
					showHalo = true;
					goto case Token.normal;
				
				case Token.normal: 
					viewer.setSelectionHaloEnabled(showHalo);
					break;
				
				default: 
					keywordExpected();
					break;
				
			}
		}
		
		internal virtual void  setEcho()
		{
			System.String propertyName = "target";
			System.String propertyValue = null;
			checkLength34();
			echoShapeActive = true;
			switch (statement[2].tok)
			{
				
				case Token.off: 
					echoShapeActive = false;
					propertyName = "off";
					break;
				
				case Token.none: 
					echoShapeActive = false;
					goto case Token.identifier;
				
				case Token.identifier: 
					propertyValue = ((System.String) statement[2].value_Renamed);
					break;
				
				default: 
					keywordExpected();
					break;
				
			}
			viewer.loadShape(JmolConstants.SHAPE_ECHO);
			viewer.setShapeProperty(JmolConstants.SHAPE_ECHO, propertyName, propertyValue);
			if (statementLength == 4)
			{
				int tok = statement[3].tok;
				if (tok != Token.identifier && tok != Token.center)
					keywordExpected();
				viewer.setShapeProperty(JmolConstants.SHAPE_ECHO, "align", (System.String) statement[3].value_Renamed);
			}
		}
		
		internal virtual void  setFontsize()
		{
			int rasmolSize = 8;
			if (statementLength == 3)
			{
				rasmolSize = SetInteger;
				// this is a kludge/hack to be somewhat compatible with RasMol
				rasmolSize += 5;
				
				if (rasmolSize < JmolConstants.LABEL_MINIMUM_FONTSIZE || rasmolSize > JmolConstants.LABEL_MAXIMUM_FONTSIZE)
					numberOutOfRange(JmolConstants.LABEL_MINIMUM_FONTSIZE, JmolConstants.LABEL_MINIMUM_FONTSIZE);
			}
			viewer.loadShape(JmolConstants.SHAPE_LABELS);
			viewer.setShapeProperty(JmolConstants.SHAPE_LABELS, "fontsize", (System.Object) rasmolSize);
		}
		
		internal virtual void  setLabelOffset()
		{
			checkLength4();
			int xOffset = intParameter(2);
			int yOffset = intParameter(3);
			int offset = ((xOffset & 0xFF) << 8) | (yOffset & 0xFF);
			viewer.loadShape(JmolConstants.SHAPE_LABELS);
			viewer.setShapeProperty(JmolConstants.SHAPE_LABELS, "offset", (System.Object) offset);
		}
		
		internal virtual void  setHetero()
		{
			viewer.RasmolHeteroSetting = SetBoolean;
		}
		
		internal virtual void  setHydrogen()
		{
			viewer.RasmolHydrogenSetting = SetBoolean;
		}
		
		internal virtual void  setMonitor()
		{
			bool showMeasurementNumbers = false;
			checkLength3();
			switch (statement[2].tok)
			{
				
				case Token.on: 
					showMeasurementNumbers = true;
					goto case Token.off;
				
				case Token.off: 
					viewer.setShapeProperty(JmolConstants.SHAPE_MEASURES, "showMeasurementNumbers", (System.Object) (showMeasurementNumbers?true:false));
					return ;
				
				case Token.identifier: 
					if (!viewer.setMeasureDistanceUnits((System.String) statement[2].value_Renamed))
						unrecognizedSetParameter();
					return ;
				}
			viewer.setShapeSize(JmolConstants.SHAPE_MEASURES, SetAxesTypeMad);
		}
		
		internal virtual void  setDebugScript()
		{
			viewer.setDebugScript(SetBoolean);
		}
		
		internal virtual void  setProperty()
		{
			checkLength4();
			if (statement[2].tok != Token.identifier)
				propertyNameExpected();
			System.String propertyName = (System.String) statement[2].value_Renamed;
			switch (statement[3].tok)
			{
				
				case Token.on: 
					viewer.setBooleanProperty(propertyName, true);
					break;
				
				case Token.off: 
					viewer.setBooleanProperty(propertyName, false);
					break;
				
				case Token.integer: 
				case Token.decimal_Renamed: 
				case Token.string_Renamed: 
					notImplemented(3);
					goto default;
				
				default: 
					unrecognizedSetParameter();
					break;
				
			}
		}
		
		internal virtual void  setSolvent()
		{
			viewer.SolventOn = SetBoolean;
		}
		
		internal virtual void  setRadius()
		{
			viewer.SolventProbeRadius = SetAngstroms;
		}
		
		internal virtual void  setStrands()
		{
			int strandCount = 5;
			if (statementLength == 3)
			{
				if (statement[2].tok != Token.integer)
					integerExpected();
				strandCount = statement[2].intValue;
				if (strandCount < 0 || strandCount > 20)
					numberOutOfRange(0, 20);
			}
			viewer.setShapeProperty(JmolConstants.SHAPE_STRANDS, "strandCount", (System.Object) strandCount);
		}
		
		internal virtual void  setSpecular()
		{
			checkLength3();
			if (statement[2].tok == Token.integer)
				viewer.SpecularPercent = SetInteger;
			else
				viewer.Specular = SetBoolean;
		}
		
		internal virtual void  setSpecPower()
		{
			viewer.SpecularPower = SetInteger;
		}
		
		internal virtual void  setAmbient()
		{
			viewer.AmbientPercent = SetInteger;
		}
		
		internal virtual void  setDiffuse()
		{
			viewer.DiffusePercent = SetInteger;
		}
		
		internal virtual void  setSpin()
		{
			checkLength4();
			int value_Renamed = intParameter(3);
			switch (statement[2].tok)
			{
				
				case Token.x: 
					viewer.SpinX = value_Renamed;
					break;
				
				case Token.y: 
					viewer.SpinY = value_Renamed;
					break;
				
				case Token.z: 
					viewer.SpinZ = value_Renamed;
					break;
				
				case Token.fps: 
					viewer.SpinFps = value_Renamed;
					break;
				
				default: 
					unrecognizedSetParameter();
					break;
				
			}
		}
		
		internal virtual void  setSsbond()
		{
			checkLength3();
			bool ssbondsBackbone = false;
			viewer.loadShape(JmolConstants.SHAPE_SSSTICKS);
			switch (statement[2].tok)
			{
				
				case Token.backbone: 
					ssbondsBackbone = true;
					break;
				
				case Token.sidechain: 
					break;
				
				default: 
					invalidArgument();
					break;
				
			}
			viewer.SsbondsBackbone = ssbondsBackbone;
		}
		
		internal virtual void  setHbond()
		{
			checkLength3();
			bool bool_Renamed = false;
			switch (statement[2].tok)
			{
				
				case Token.backbone: 
					bool_Renamed = true;
					// fall into
					goto case Token.sidechain;
				
				case Token.sidechain: 
					viewer.HbondsBackbone = bool_Renamed;
					break;
				
				case Token.solid: 
					bool_Renamed = true;
					// falll into
					goto case Token.dotted;
				
				case Token.dotted: 
					viewer.HbondsSolid = bool_Renamed;
					break;
				
				default: 
					invalidArgument();
					break;
				
			}
		}
		
		internal virtual void  setScale3d()
		{
			checkLength3();
			float angstromsPerInch = 0;
			switch (statement[2].tok)
			{
				
				case Token.decimal_Renamed: 
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Float.floatValue' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					angstromsPerInch = (float) ((System.Single) statement[2].value_Renamed);
					break;
				
				case Token.integer: 
					angstromsPerInch = statement[2].intValue;
					break;
				
				default: 
					numberExpected();
					break;
				
			}
			viewer.ScaleAngstromsPerInch = angstromsPerInch;
		}
		
		internal virtual void  setPicking()
		{
			int pickingMode = JmolConstants.PICKING_IDENT;
			if (statementLength >= 3)
			{
				switch (statement[2].tok)
				{
					
					case Token.none: 
					case Token.off: 
						pickingMode = JmolConstants.PICKING_OFF;
						//fall into
						goto case Token.on;
					
					case Token.on: 
						break;
					
					case Token.ident: 
						pickingMode = JmolConstants.PICKING_IDENT;
						break;
					
					case Token.distance: 
						pickingMode = JmolConstants.PICKING_DISTANCE;
						break;
					
					case Token.monitor: 
						pickingMode = JmolConstants.PICKING_MONITOR;
						break;
					
					case Token.angle: 
						pickingMode = JmolConstants.PICKING_ANGLE;
						break;
					
					case Token.torsion: 
						pickingMode = JmolConstants.PICKING_TORSION;
						break;
					
					case Token.label: 
						pickingMode = JmolConstants.PICKING_LABEL;
						break;
					
					case Token.center: 
						pickingMode = JmolConstants.PICKING_CENTER;
						break;
					
					case Token.coord: 
						pickingMode = JmolConstants.PICKING_COORD;
						break;
					
					case Token.bond: 
						pickingMode = JmolConstants.PICKING_BOND;
						break;
					
					case Token.atom: 
						pickingMode = JmolConstants.PICKING_SELECT_ATOM;
						break;
					
					case Token.group: 
						pickingMode = JmolConstants.PICKING_SELECT_GROUP;
						break;
					
					case Token.chain: 
						pickingMode = JmolConstants.PICKING_SELECT_CHAIN;
						break;
					
					case Token.select: 
						pickingMode = JmolConstants.PICKING_SELECT_ATOM;
						if (statementLength == 4)
						{
							switch (statement[3].tok)
							{
								
								case Token.chain: 
									pickingMode = JmolConstants.PICKING_SELECT_CHAIN;
									// fall into
									goto case Token.atom;
								
								case Token.atom: 
									break;
								
								case Token.group: 
									pickingMode = JmolConstants.PICKING_SELECT_GROUP;
									break;
								
								default: 
									invalidArgument();
									break;
								
							}
						}
						break;
					
					default: 
						invalidArgument();
						break;
					
				}
			}
			viewer.PickingMode = pickingMode;
		}
		
		/*==============================================================*
		* SHOW implementations
		*==============================================================*/
		
		internal virtual void  show()
		{
			switch (statement[1].tok)
			{
				
				case Token.pdbheader: 
					showPdbHeader();
					break;
				
				case Token.model: 
					showModel();
					break;
				
				case Token.animation: 
					showAnimation();
					break;
				
				case Token.orientation: 
					showOrientation();
					break;
				
				case Token.transform: 
					showTransform();
					break;
				
				case Token.center: 
					showCenter();
					break;
				
				case Token.file: 
					showFile();
					break;
				
				case Token.boundbox: 
					showBoundbox();
					break;
				
				case Token.zoom: 
					showZoom();
					break;
					
					// not implemented
				
				case Token.spin: 
				case Token.list: 
				case Token.mlp: 
				case Token.information: 
				case Token.phipsi: 
				case Token.ramprint: 
				case Token.rotation: 
				case Token.group: 
				case Token.chain: 
				case Token.atom: 
				case Token.sequence: 
				case Token.symmetry: 
				case Token.translation: 
				case Token.residue: 
				case Token.all: 
				case Token.selected: 
					notImplemented(1);
					break;
				
				
				default: 
					evalError("unrecognized SHOW parameter");
					break;
				
			}
		}
		
		internal virtual void  showString(System.String str)
		{
			System.Console.Out.WriteLine("show:" + str);
			viewer.scriptStatus(str);
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'pdbRecords'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] pdbRecords = new System.String[]{"ATOM  ", "HELIX ", "SHEET ", "TURN  ", "MODEL ", "SCALE", "HETATM", "SEQRES", "DBREF "};
		
		internal virtual void  showPdbHeader()
		{
			if ((System.Object) "pdb" != (System.Object) viewer.ModelSetTypeName)
			{
				showString("!Not a pdb file!");
				return ;
			}
			System.String modelFile = viewer.CurrentFileAsString;
			int ichMin = modelFile.Length;
			for (int i = pdbRecords.Length; --i >= 0; )
			{
				int ichFound = - 1;
				System.String strRecord = pdbRecords[i];
				if (modelFile.StartsWith(strRecord))
					ichFound = 0;
				else
				{
					System.String strSearch = "\n" + strRecord;
					ichFound = modelFile.IndexOf(strSearch);
					if (ichFound >= 0)
						++ichFound;
				}
				if (ichFound >= 0 && ichFound < ichMin)
					ichMin = ichFound;
			}
			showString(modelFile.Substring(0, (ichMin) - (0)));
		}
		
		internal virtual void  showModel()
		{
			int modelCount = viewer.ModelCount;
			showString("model count = " + modelCount + "\nmodelSetHasVibrationVectors:" + viewer.modelSetHasVibrationVectors());
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			System.Collections.Specialized.NameValueCollection props = viewer.ModelSetProperties;
			printProperties(props);
			for (int i = 0; i < modelCount; ++i)
			{
				showString("" + i + ":" + viewer.getModelNumber(i) + ":" + viewer.getModelName(i) + "\nmodelHasVibrationVectors:" + viewer.modelHasVibrationVectors(i));
				printProperties(viewer.getModelProperties(i));
			}
		}
		
		internal virtual void  showFile()
		{
			System.Console.Out.WriteLine("showFile && statementLength=" + statementLength);
			if (statementLength == 2)
			{
				showString(viewer.CurrentFileAsString);
				return ;
			}
			if (statementLength == 3 && statement[2].tok == Token.string_Renamed)
			{
				System.String fileName = (System.String) statement[2].value_Renamed;
				System.Console.Out.WriteLine("fileName=" + fileName);
				showString(viewer.getFileAsString(fileName));
				return ;
			}
			invalidArgument();
		}
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		internal virtual void  printProperties(System.Collections.Specialized.NameValueCollection props)
		{
			if (props == null)
			{
				showString("Properties: null");
			}
			else
			{
				System.Collections.IEnumerator e = props.Keys.GetEnumerator();
				showString("Properties:");
				//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
				while (e.MoveNext())
				{
					//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
					System.String propertyName = (System.String) e.Current;
					showString(" " + propertyName + "=" + props.Get(propertyName));
				}
			}
			System.Console.Out.WriteLine("");
		}
		
		internal virtual void  showAnimation()
		{
			showString("show animation information goes here");
		}
		
		internal virtual void  showOrientation()
		{
			showString(viewer.OrientationText);
		}
		
		internal virtual void  showTransform()
		{
			showString("transform:\n" + viewer.TransformText);
		}
		
		internal virtual void  showCenter()
		{
			showString("center: " + viewer.getCenter());
		}
		
		internal virtual void  showZoom()
		{
			showString("zoom " + (viewer.ZoomEnabled?("" + viewer.ZoomPercentSetting):"off"));
		}
		
		internal virtual void  showBoundbox()
		{
			showString("boundbox: " + viewer.BoundBoxCenter + " " + viewer.BoundBoxCornerVector);
		}
		
		internal AxisAngle4f aaMoveTo;
		internal AxisAngle4f aaStep;
		internal AxisAngle4f aaTotal;
		internal Matrix3f matrixStart;
		internal Matrix3f matrixInverse;
		internal Matrix3f matrixStep;
		internal Matrix3f matrixEnd;
		
		internal virtual void  moveto()
		{
			if (statementLength < 6 || statementLength > 9)
				badArgumentCount();
			float floatSecondsTotal = floatParameter(1);
			float axisX = floatParameter(2);
			float axisY = floatParameter(3);
			float axisZ = floatParameter(4);
			float degrees = floatParameter(5);
			int zoom = statementLength >= 7?intParameter(6):100;
			int xTrans = statementLength >= 8?intParameter(7):0;
			int yTrans = statementLength >= 9?intParameter(8):0;
			
			if (aaMoveTo == null)
			{
				aaMoveTo = new AxisAngle4f();
				aaStep = new AxisAngle4f();
				aaTotal = new AxisAngle4f();
				matrixStart = new Matrix3f();
				matrixEnd = new Matrix3f();
				matrixStep = new Matrix3f();
				matrixInverse = new Matrix3f();
			}
			if (degrees < 0.01f && degrees > - 0.01f)
			{
				matrixEnd.setIdentity();
			}
			else
			{
				if (axisX == 0 && axisY == 0 && axisZ == 0)
				{
					// invalid ... no rotation
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					int sleepTime = (int) (floatSecondsTotal * 1000) - 30;
					if (sleepTime > 0)
					{
						try
						{
							//UPGRADE_TODO: Method 'java.lang.Thread.sleep' was converted to 'System.Threading.Thread.Sleep' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangThreadsleep_long'"
							System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64) 10000 * sleepTime));
						}
						catch (System.Threading.ThreadInterruptedException ie)
						{
						}
					}
					return ;
				}
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				aaMoveTo.set_Renamed(axisX, axisY, axisZ, degrees * (float) System.Math.PI / 180);
				matrixEnd.set_Renamed(aaMoveTo);
			}
			viewer.getRotation(matrixStart);
			matrixInverse.invert(matrixStart);
			
			matrixStep.mul(matrixEnd, matrixInverse);
			aaTotal.set_Renamed(matrixStep);
			
			/*
			System.out.println("\nmatrixStart=\n" + matrixStart +
			"\nmatrixInverse=\n" + matrixInverse +
			"\nmatrixStep=\n" + matrixStep +
			"\naaStep=\n" + aaStep);
			*/
			
			int fps = 30;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			int totalSteps = (int) (floatSecondsTotal * fps);
			if (totalSteps > 1)
			{
				aaStep.angle /= totalSteps;
				int frameTimeMillis = 1000 / fps;
				long targetTime = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
				int zoomStart = viewer.ZoomPercent;
				int zoomDelta = zoom - zoomStart;
				float xTransStart = viewer.TranslationXPercent;
				float xTransDelta = xTrans - xTransStart;
				float yTransStart = viewer.TranslationYPercent;
				float yTransDelta = yTrans - yTransStart;
				for (int i = 1; i < totalSteps; ++i)
				{
					
					viewer.getRotation(matrixStart);
					matrixInverse.invert(matrixStart);
					matrixStep.mul(matrixEnd, matrixInverse);
					aaTotal.set_Renamed(matrixStep);
					
					aaStep.set_Renamed(aaTotal);
					aaStep.angle /= (totalSteps - i + 1);
					if (aaStep.angle == 0)
						matrixStep.setIdentity();
					else
						matrixStep.set_Renamed(aaStep);
					matrixStep.mul(matrixStart);
					viewer.zoomToPercent(zoomStart + (zoomDelta * i / totalSteps));
					viewer.translateToXPercent(xTransStart + (xTransDelta * i / totalSteps));
					viewer.translateToYPercent(yTransStart + (yTransDelta * i / totalSteps));
					viewer.setRotation(matrixStep);
					targetTime += frameTimeMillis;
					if ((System.DateTime.Now.Ticks - 621355968000000000) / 10000 < targetTime)
					{
						viewer.requestRepaintAndWait();
						int sleepTime = (int) (targetTime - (System.DateTime.Now.Ticks - 621355968000000000) / 10000);
						if (sleepTime > 0)
						{
							try
							{
								//UPGRADE_TODO: Method 'java.lang.Thread.sleep' was converted to 'System.Threading.Thread.Sleep' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangThreadsleep_long'"
								System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64) 10000 * sleepTime));
							}
							catch (System.Threading.ThreadInterruptedException ie)
							{
							}
						}
					}
				}
			}
			else
			{
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				int sleepTime = (int) (floatSecondsTotal * 1000) - 30;
				if (sleepTime > 0)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.lang.Thread.sleep' was converted to 'System.Threading.Thread.Sleep' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangThreadsleep_long'"
						System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64) 10000 * sleepTime));
					}
					catch (System.Threading.ThreadInterruptedException ie)
					{
					}
				}
			}
			viewer.zoomToPercent(zoom);
			viewer.translateToXPercent(xTrans);
			viewer.translateToYPercent(yTrans);
			viewer.setRotation(matrixEnd);
		}
		
		internal virtual void  console()
		{
			viewer.showConsole(statement[1].tok == Token.on);
		}
		
		internal virtual void  pmesh()
		{
			viewer.loadShape(JmolConstants.SHAPE_PMESH);
			viewer.setShapeProperty(JmolConstants.SHAPE_PMESH, "meshID", (System.Object) null);
			for (int i = 1; i < statementLength; ++i)
			{
				System.String propertyName = null;
				System.Object propertyValue = null;
				switch (statement[i].tok)
				{
					
					case Token.identifier: 
						propertyName = "meshID";
						propertyValue = statement[i].value_Renamed;
						break;
					
					case Token.string_Renamed: 
						System.String filename = (System.String) statement[i].value_Renamed;
						System.Object t = viewer.getUnzippedBufferedReaderOrErrorMessageFromName(filename);
						if (t is System.String)
						{
							//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
							fileNotFoundException(filename + ":" + t);
						}
						propertyName = "bufferedreader";
						propertyValue = t;
						break;
					
					case Token.dots: 
						propertyValue = true;
						goto case Token.nodots;
					
					case Token.nodots: 
						propertyName = "dots";
						break;
					
					case Token.mesh: 
						propertyValue = true;
						goto case Token.nomesh;
					
					case Token.nomesh: 
						propertyName = "mesh";
						break;
					
					case Token.fill: 
						propertyValue = true;
						goto case Token.nofill;
					
					case Token.nofill: 
						propertyName = "fill";
						break;
					
					case Token.on: 
					case Token.off: 
						propertyName = ((System.String) statement[i].value_Renamed);
						break;
					
					case Token.delete: 
						propertyName = "delete";
						break;
					
					default: 
						invalidArgument();
						break;
					
				}
				viewer.setShapeProperty(JmolConstants.SHAPE_PMESH, propertyName, propertyValue);
			}
		}
		
		internal virtual void  polyhedra()
		{
			/*
			* needsGenerating:
			* 
			* polyhedra [number of vertices and/or basis] [at most two selection sets] 
			*   [optional type and/or edge] [optional design parameters]
			*   
			* OR else:
			* 
			* polyhedra [at most one selection set] [type-and/or-edge or on/off/delete]
			* 
			*/
			bool needsGenerating = false;
			bool onOffDelete = false;
			bool typeSeen = false;
			bool edgeParameterSeen = false;
			bool isDesignParameter = false;
			int nAtomSets = 0;
			viewer.loadShape(JmolConstants.SHAPE_POLYHEDRA);
			viewer.setShapeProperty(JmolConstants.SHAPE_POLYHEDRA, "init", (System.Object) null);
			System.String setPropertyName = "potentialCenterSet";
			System.String decimalPropertyName = "radius_";
			for (int i = 1; i < statementLength; ++i)
			{
				System.String propertyName = null;
				System.Object propertyValue = null;
				Token token = statement[i];
				switch (token.tok)
				{
					
					case Token.opEQ: 
					case Token.opOr: 
						continue;
					
					case Token.bonds: 
						if (nAtomSets > 0)
							invalidParameterOrder();
						needsGenerating = true;
						propertyName = "bonds";
						break;
					
					case Token.radius: 
						decimalPropertyName = "radius";
						continue;
					
					case Token.identifier: 
						System.String str = (System.String) token.value_Renamed;
						if ("collapsed".ToUpper().Equals(str.ToUpper()))
						{
							propertyName = "collapsed";
							propertyValue = true;
							if (typeSeen)
								incompatibleArguments();
							typeSeen = true;
							break;
						}
						if ("flat".ToUpper().Equals(str.ToUpper()))
						{
							propertyName = "collapsed";
							propertyValue = false;
							if (typeSeen)
								incompatibleArguments();
							typeSeen = true;
							break;
						}
						if (!needsGenerating)
							insufficientArguments();
						if ("to".ToUpper().Equals(str.ToUpper()))
						{
							if (nAtomSets > 1)
								invalidParameterOrder();
							setPropertyName = "potentialVertexSet";
							continue;
						}
						if ("centerAngleMax".ToUpper().Equals(str.ToUpper()))
						{
							decimalPropertyName = "centerAngleMax";
							isDesignParameter = true;
							continue;
						}
						if ("faceNormalMax".ToUpper().Equals(str.ToUpper()))
						{
							decimalPropertyName = "faceNormalMax";
							isDesignParameter = true;
							continue;
						}
						if ("faceCenterOffset".ToUpper().Equals(str.ToUpper()))
						{
							decimalPropertyName = "faceCenterOffset";
							isDesignParameter = true;
							continue;
						}
						unrecognizedSubcommand();
						goto case Token.integer;
					
					case Token.integer: 
						if (nAtomSets > 0 && !isDesignParameter)
							invalidParameterOrder();
						// no reason not to allow integers when explicit
						if ((System.Object) decimalPropertyName == (System.Object) "radius_")
						{
							propertyName = "vertexCount";
							propertyValue = (System.Int32) token.intValue;
							needsGenerating = true;
							break;
						}
						goto case Token.decimal_Renamed;
					
					case Token.decimal_Renamed: 
						if (nAtomSets > 0 && !isDesignParameter)
							invalidParameterOrder();
						propertyName = ((System.Object) decimalPropertyName == (System.Object) "radius_"?"radius":decimalPropertyName);
						propertyValue = (float) floatParameter(i);
						decimalPropertyName = "radius_";
						isDesignParameter = false;
						needsGenerating = true;
						break;
					
					case Token.delete: 
					case Token.on: 
					case Token.off: 
						if (++i != statementLength || needsGenerating || nAtomSets > 1 || nAtomSets == 0 && (System.Object) setPropertyName == (System.Object) "potentialVertexSet")
							incompatibleArguments();
						propertyName = ((System.String) token.value_Renamed);
						onOffDelete = true;
						break;
					
					case Token.edges: 
					case Token.noedges: 
					case Token.frontedges: 
						if (edgeParameterSeen)
							incompatibleArguments();
						propertyName = ((System.String) token.value_Renamed);
						edgeParameterSeen = true;
						break;
					
					case Token.expressionBegin: 
						if (typeSeen)
							invalidParameterOrder();
						if (++nAtomSets > 2)
							badArgumentCount();
						if ((System.Object) setPropertyName == (System.Object) "potentialVertexSet")
							needsGenerating = true;
						propertyName = setPropertyName;
						setPropertyName = "potentialVertexSet";
						propertyValue = expression(statement, ++i);
						i = pcLastExpressionInstruction; // the for loop will increment i
						break;
					
					default: 
						invalidArgument();
						break;
					
				}
				viewer.setShapeProperty(JmolConstants.SHAPE_POLYHEDRA, propertyName, propertyValue);
				if (onOffDelete)
					return ;
			}
			if (!needsGenerating && !typeSeen && !edgeParameterSeen)
				insufficientArguments();
			if (needsGenerating)
				viewer.setShapeProperty(JmolConstants.SHAPE_POLYHEDRA, "generate", (System.Object) null);
		}
		
		internal virtual void  sasurface()
		{
			viewer.loadShape(JmolConstants.SHAPE_SASURFACE);
			viewer.setShapeProperty(JmolConstants.SHAPE_SASURFACE, "surfaceID", (System.Object) null);
			for (int i = 1; i < statementLength; ++i)
			{
				System.String propertyName = null;
				System.Object propertyValue = null;
				switch (statement[i].tok)
				{
					
					case Token.identifier: 
						propertyValue = statement[i].value_Renamed;
						// fall into
						goto case Token.all;
					
					case Token.all: 
						propertyName = "surfaceID";
						break;
					
					case Token.on: 
					case Token.off: 
						propertyName = ((System.String) statement[i].value_Renamed);
						break;
					
					case Token.delete: 
						propertyName = "delete";
						break;
					
					default: 
						invalidArgument();
						break;
					
				}
				viewer.setShapeProperty(JmolConstants.SHAPE_SASURFACE, propertyName, propertyValue);
			}
		}
		
		internal virtual void  centerAt()
		{
			if (statementLength != 2 && statementLength != 5)
				badArgumentCount();
			System.String relativeTo = null;
			switch (statement[1].tok)
			{
				
				case Token.absolute: 
					relativeTo = "absolute";
					break;
				
				case Token.average: 
					relativeTo = "average";
					break;
				
				case Token.boundbox: 
					relativeTo = "boundbox";
					break;
				
				default: 
					unrecognizedSubcommand();
					break;
				
			}
			float x, y, z;
			if (statementLength == 2)
			{
				x = y = z = 0;
			}
			else
			{
				x = floatParameter(2);
				y = floatParameter(3);
				z = floatParameter(4);
			}
			viewer.setCenter(relativeTo, x, y, z);
		}
		
		internal virtual void  isosurface()
		{
			viewer.loadShape(JmolConstants.SHAPE_ISOSURFACE);
			viewer.setShapeProperty(JmolConstants.SHAPE_ISOSURFACE, "meshID", (System.Object) null);
			bool colorSeen = false;
			int colorRangeStage = 0;
			for (int i = 1; i < statementLength; ++i)
			{
				System.String propertyName = null;
				System.Object propertyValue = null;
				switch (statement[i].tok)
				{
					
					case Token.identifier: 
						propertyValue = statement[i].value_Renamed;
						// fall into
						goto case Token.all;
					
					case Token.all: 
						propertyName = "meshID";
						break;
					
					case Token.string_Renamed: 
						System.String filename = (System.String) statement[i].value_Renamed;
						System.Object t = viewer.getUnzippedBufferedReaderOrErrorMessageFromName(filename);
						if (t is System.String)
						{
							//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
							fileNotFoundException(filename + ":" + t);
						}
						propertyName = colorSeen?"colorreader":"bufferedreader";
						propertyValue = t;
						if (colorSeen && colorRangeStage != 0 && colorRangeStage != 3)
							invalidArgument();
						break;
					
					case Token.decimal_Renamed: 
						if (colorRangeStage == 0)
						{
							propertyName = "cutoff";
							propertyValue = statement[i].value_Renamed;
							break;
						}
						// fall into
						goto case Token.integer;
					
					case Token.integer: 
						if (colorRangeStage == 0 || colorRangeStage >= 3)
							invalidArgument();
						propertyName = colorRangeStage == 1?"rangeMin":"rangeMax";
						propertyValue = (float) floatParameter(i);
						++colorRangeStage;
						break;
					
					case Token.dots: 
						propertyValue = true;
						goto case Token.nodots;
					
					case Token.nodots: 
						propertyName = "dots";
						break;
					
					case Token.mesh: 
						propertyValue = true;
						goto case Token.nomesh;
					
					case Token.nomesh: 
						propertyName = "mesh";
						break;
					
					case Token.fill: 
						propertyValue = true;
						goto case Token.nofill;
					
					case Token.nofill: 
						propertyName = "fill";
						break;
					
					case Token.on: 
					case Token.off: 
						propertyName = ((System.String) statement[i].value_Renamed);
						break;
					
					case Token.delete: 
						propertyName = "delete";
						break;
					
					case Token.color: 
						colorSeen = true;
						propertyName = "removeRange";
						break;
					
					case Token.absolute: 
						colorRangeStage = 1;
						break;
					
					default: 
						invalidArgument();
						break;
					
				}
				if (propertyName != null)
					viewer.setShapeProperty(JmolConstants.SHAPE_ISOSURFACE, propertyName, propertyValue);
			}
		}
		
		internal virtual void  stereo()
		{
			int stereoMode = JmolConstants.STEREO_DOUBLE;
			// see www.usm.maine.edu/~rhodes/0Help/StereoViewing.html
			float degrees = - 5;
			bool degreesSeen = false;
			for (int i = 1; i < statementLength; ++i)
			{
				switch (statement[i].tok)
				{
					
					case Token.on: 
						stereoMode = JmolConstants.STEREO_DOUBLE;
						break;
					
					case Token.off: 
						stereoMode = JmolConstants.STEREO_NONE;
						break;
					
					case Token.integer: 
					case Token.decimal_Renamed: 
						degrees = floatParameter(i);
						degreesSeen = true;
						break;
					
					case Token.identifier: 
						System.String id = (System.String) statement[i].value_Renamed;
						if (!degreesSeen)
							degrees = 3;
						if (id.ToUpper().Equals("redblue".ToUpper()))
						{
							stereoMode = JmolConstants.STEREO_REDBLUE;
							break;
						}
						if (id.ToUpper().Equals("redcyan".ToUpper()))
						{
							stereoMode = JmolConstants.STEREO_REDCYAN;
							break;
						}
						if (id.ToUpper().Equals("redgreen".ToUpper()))
						{
							stereoMode = JmolConstants.STEREO_REDGREEN;
							break;
						}
						// fall into
						goto default;
					
					default: 
						booleanOrNumberExpected();
						break;
					
				}
			}
			viewer.StereoDegrees = degrees;
			viewer.StereoMode = stereoMode;
		}
		
		internal virtual void  connect()
		{
			bool haveType = false;
			int nAtomSets = 0;
			int nDistances = 0;
			/*
			* connect [<=2 distance parameters] [<=2 atom sets] 
			*             [<=1 bond type] [<=1 operation]
			* 
			*/
			
			viewer.setShapeProperty(JmolConstants.SHAPE_STICKS, "resetConnectParameters", (System.Object) null);
			if (statementLength == 1)
			{
				viewer.setShapeProperty(JmolConstants.SHAPE_STICKS, "rasmolCompatibleConnect", (System.Object) null);
				return ;
			}
			for (int i = 1; i < statementLength; ++i)
			{
				System.String propertyName = null;
				System.Object propertyValue = null;
				switch (statement[i].tok)
				{
					
					case Token.on: 
					case Token.off: 
						if (statementLength != 2)
							badArgumentCount();
						viewer.setShapeProperty(JmolConstants.SHAPE_STICKS, "rasmolCompatibleConnect", (System.Object) null);
						return ;
					
					case Token.integer: 
					case Token.decimal_Renamed: 
						if (++nDistances > 2)
							badArgumentCount();
						if (nAtomSets > 0 || haveType)
							invalidParameterOrder();
						propertyName = "connectDistance";
						propertyValue = (float) floatParameter(i);
						break;
					
					case Token.expressionBegin: 
						if (++nAtomSets > 2)
							badArgumentCount();
						if (haveType)
							invalidParameterOrder();
						propertyName = "connectSet";
						propertyValue = expression(statement, i);
						i = pcLastExpressionInstruction; // the for loop will increment i
						break;
					
					case Token.identifier: 
					case Token.hbond: 
						System.String cmd = (System.String) statement[i].value_Renamed;
						for (int j = JmolConstants.bondOrderNames.Length; --j >= 0; )
						{
							if (cmd.ToUpper().Equals(JmolConstants.bondOrderNames[j].ToUpper()))
							{
								if (haveType)
									incompatibleArguments();
								cmd = JmolConstants.bondOrderNames[j];
								propertyName = "connectBondOrder";
								propertyValue = cmd;
								haveType = true;
								//UPGRADE_NOTE: Labeled break statement was changed to a goto statement. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1012'"
								goto switch_tag_brk;
							}
						}
						if (++i != statementLength)
							invalidParameterOrder();
						if ("modify".ToUpper().Equals(cmd.ToUpper()))
							propertyValue = "modify";
						else if ("create".ToUpper().Equals(cmd.ToUpper()))
							propertyValue = "create";
						else if ("modifyOrCreate".ToUpper().Equals(cmd.ToUpper()))
							propertyValue = "modifyOrCreate";
						else if ("auto".ToUpper().Equals(cmd.ToUpper()))
							propertyValue = "auto";
						else
							unrecognizedSubcommand();
						propertyName = "connectOperation";
						break;
					
					case Token.none: 
					case Token.delete: 
						if (++i != statementLength)
							invalidParameterOrder();
						propertyName = "connectOperation";
						propertyValue = "delete";
						break;
					
					default: 
						invalidArgument();
						break;
					
				}
				//UPGRADE_NOTE: Label 'switch_tag_brk' was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1011'"
switch_tag_brk: ;
				
				viewer.setShapeProperty(JmolConstants.SHAPE_STICKS, propertyName, propertyValue);
			}
			viewer.setShapeProperty(JmolConstants.SHAPE_STICKS, "applyConnectParameters", (System.Object) null);
		}
		static Eval()
		{
			shapeToks = new int[]{Token.atom, Token.bonds, Token.hbond, Token.ssbond, Token.label, Token.vector, Token.monitor, Token.dots, Token.backbone, Token.trace, Token.cartoon, Token.strands, Token.meshRibbon, Token.ribbon, Token.rocket, Token.star, Token.axes, Token.boundbox, Token.unitcell, Token.frank, Token.echo, Token.hover, Token.pmesh, Token.polyhedra, Token.sasurface, Token.isosurface, Token.prueba};
			{
				if (shapeToks.Length != JmolConstants.SHAPE_MAX)
				{
					System.Console.Out.WriteLine("shapeToks mismatch");
					throw new System.NullReferenceException();
				}
			}
		}
	}
}