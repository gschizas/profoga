/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-03-27 16:57:12 +0200 (lun., 27 mars 2006) $
* $Revision: 4769 $
*
* Copyright (C) 2004-2005  The Jmol Development Team
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
using org.jmol.api;
using org.jmol.appletwrapper;
using SmarterJmolAdapter = org.jmol.adapter.smarter.SmarterJmolAdapter;
//import org.openscience.jmol.adapters.CdkJmolAdapter;
using JmolPopup = org.jmol.popup.JmolPopup;
using GT = org.jmol.i18n.GT;
namespace org.jmol.applet
{
	
	/*
	these are *required*
	
	[param name="progressbar" value="true" /]
	[param name="progresscolor" value="blue" /]
	[param name="boxmessage" value="your-favorite-message" /]
	[param name="boxbgcolor" value="#112233" /]
	[param name="boxfgcolor" value="#778899" /]  
	
	[param name="loadInline" value="
	| do
	| it
	| this
	| way
	" /]
	
	[param name="script"             value="your-script" /]
	
	// this one flips the orientation and uses RasMol/Chime colors
	[param name="emulate"    value="chime" /]
	
	// this is *required* if you want the applet to be able to
	// call your callbacks
	
	mayscript="true" is required as an applet tag
	
	[param name="AnimFrameCallback"  value="yourJavaScriptMethodName" /]
	[param name="LoadStructCallback" value="yourJavaScriptMethodName" /]
	[param name="MessageCallback"    value="yourJavaScriptMethodName" /]
	[param name="PauseCallback"      value="yourJavaScriptMethodName" /]
	[param name="PickCallback"       value="yourJavaScriptMethodName" /]
	*/
	
	public class Jmol : WrappedApplet, JmolAppletInterface
	{
		virtual public System.String AppletInfo
		{
			get
			{
				return appletInfo;
			}
			
		}
		virtual public AppletWrapper AppletWrapper
		{
			set
			{
				this.appletWrapper = value;
			}
			
		}
		
		internal JmolViewer viewer;
		internal bool jvm12orGreater;
		internal System.String emulate;
		internal Jvm12 jvm12;
		internal JmolPopup jmolpopup;
		internal System.String htmlName;
		internal JmolAppletRegistry appletRegistry;
		
		internal MyStatusListener myStatusListener;
		
		internal AppletWrapper appletWrapper;
		
		/*
		* miguel 2004 11 29
		*
		* WARNING! DANGER!
		*
		* I have discovered that if you call JSObject.getWindow().toString()
		* on Safari v125.1 / Java 1.4.2_03 then it breaks or kills Safari
		* I filed Apple bug report #3897879
		*
		* Therefore, do *not* call System.out.println("" + jsoWindow);
		*/
		//UPGRADE_TODO: Class 'netscape.javascript.JSObject' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
		internal JSObject jsoWindow;
		//UPGRADE_TODO: Class 'netscape.javascript.JSObject' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
		internal JSObject jsoDocument;
		
		internal bool mayScript;
		internal System.String animFrameCallback;
		internal System.String loadStructCallback;
		internal System.String messageCallback;
		internal System.String pauseCallback;
		internal System.String pickCallback;
		
		internal const bool REQUIRE_PROGRESSBAR = true;
		internal bool hasProgressBar;
		internal int paintCounter;
		
		//UPGRADE_NOTE: The initialization of  'appletInfo' was moved to static method 'org.jmol.applet.Jmol'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		internal static System.String appletInfo;
		
		public virtual void  init()
		{
			htmlName = getParameter("name");
			System.String ms = getParameter("mayscript");
			mayScript = (ms != null) && (!ms.ToUpper().Equals("false".ToUpper()));
			appletRegistry = new JmolAppletRegistry(htmlName, mayScript, appletWrapper);
			
			initWindows();
			initApplication();
		}
		
		internal virtual System.String getParameter(System.String paramName)
		{
			return appletWrapper.getParameter(paramName);
		}
		
		public virtual void  initWindows()
		{
			
			// to enable CDK
			//    viewer = new JmolViewer(this, new CdkJmolAdapter(null));
			viewer = JmolViewer.allocateViewer(appletWrapper, new SmarterJmolAdapter(null));
			myStatusListener = new MyStatusListener(this);
			viewer.JmolStatusListener = myStatusListener;
			
			//UPGRADE_TODO: Method 'java.applet.Applet.getDocumentBase' was converted to 'DocumentBase' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaappletAppletgetDocumentBase'"
			//UPGRADE_TODO: The equivalent in .NET for method 'java.applet.Applet.getCodeBase' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			viewer.setAppletContext(appletWrapper.DocumentBase, new System.Uri(System.IO.Directory.GetCurrentDirectory()), getValue("JmolAppletProxy", null));
			
			jvm12orGreater = viewer.Jvm12orGreater;
			if (jvm12orGreater)
				jvm12 = new Jvm12(appletWrapper, viewer);
			
			if (mayScript)
			{
				try
				{
					//UPGRADE_TODO: Method 'netscape.javascript.JSObject.getWindow' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
					jsoWindow = JSObject.getWindow(appletWrapper);
					if (jsoWindow == null)
						System.Console.Out.WriteLine("jsoWindow returned null ... no JavaScript callbacks :-(");
					//UPGRADE_TODO: Method 'netscape.javascript.JSObject.getMember' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
					//UPGRADE_TODO: Class 'netscape.javascript.JSObject' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
					jsoDocument = (JSObject) jsoWindow.getMember("document");
					if (jsoDocument == null)
						System.Console.Out.WriteLine("jsoDocument returned null ... no DOM manipulations :-(");
				}
				catch (System.Exception e)
				{
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					System.Console.Out.WriteLine("" + e);
				}
			}
		}
		
		/*
		PropertyResourceBundle appletProperties = null;
		
		private void loadProperties() {
		URL codeBase = getCodeBase();
		try {
		URL urlProperties = new URL(codeBase, "JmolApplet.properties");
		appletProperties =
		new PropertyResourceBundle(urlProperties.openStream());
		} catch (Exception ex) {
		System.out.println("JmolApplet.loadProperties() -> " + ex);
		}
		}
		*/
		
		internal virtual bool getBooleanValue(System.String propertyName, bool defaultValue)
		{
			System.String value_Renamed = getValue(propertyName, defaultValue?"true":"");
			return (value_Renamed.ToUpper().Equals("true".ToUpper()) || value_Renamed.ToUpper().Equals("on".ToUpper()) || value_Renamed.ToUpper().Equals("yes".ToUpper()));
		}
		
		internal virtual System.String getValue(System.String propertyName, System.String defaultValue)
		{
			System.String stringValue = getParameter(propertyName);
			if (stringValue != null)
				return stringValue;
			/*
			if (appletProperties != null) {
			try {
			stringValue = appletProperties.getString(propertyName);
			return stringValue;
			} catch (MissingResourceException ex) {
			}
			}
			*/
			return defaultValue;
		}
		/*
		private int getValue(String propertyName, int defaultValue) {
		String stringValue = getValue(propertyName, null);
		if (stringValue != null)
		try {
		return Integer.parseInt(stringValue);
		} catch (NumberFormatException ex) {
		System.out.println(propertyName + ":" +
		stringValue + " is not an integer");
		}
		return defaultValue;
		}
		
		private double getValue(String propertyName, double defaultValue) {
		String stringValue = getValue(propertyName, null);
		if (stringValue != null)
		try {
		return (new Double(stringValue)).doubleValue();
		} catch (NumberFormatException ex) {
		System.out.println(propertyName + ":" +
		stringValue + " is not a double");
		}
		return defaultValue;
		}
		*/
		internal virtual System.String getValueLowerCase(System.String paramName, System.String defaultValue)
		{
			System.String value_Renamed = getValue(paramName, defaultValue);
			if (value_Renamed != null)
			{
				value_Renamed = value_Renamed.Trim().ToLower();
				if (value_Renamed.Length == 0)
					value_Renamed = null;
			}
			return value_Renamed;
		}
		
		public virtual void  initApplication()
		{
			viewer.pushHoldRepaint();
			{
				// REQUIRE that the progressbar be shown
				hasProgressBar = getBooleanValue("progressbar", false);
				
				// should the popupMenu be loaded ?
				bool popupMenu = getBooleanValue("popupMenu", true);
				if (popupMenu)
					loadPopupMenuAsBackgroundTask();
				
				emulate = getValueLowerCase("emulate", "jmol");
				if (emulate.Equals("chime"))
				{
					viewer.setRasmolDefaults();
				}
				else
				{
					viewer.setJmolDefaults();
				}
				System.String bgcolor = getValue("boxbgcolor", "black");
				bgcolor = getValue("bgcolor", bgcolor);
				viewer.ColorBackground = bgcolor;
				
				loadInline(getValue("loadInline", null));
				loadNodeId(getValue("loadNodeId", null));
				
				viewer.setFrankOn(true);
				
				animFrameCallback = getValue("AnimFrameCallback", null);
				loadStructCallback = getValue("LoadStructCallback", null);
				messageCallback = getValue("MessageCallback", null);
				pauseCallback = getValue("PauseCallback", null);
				pickCallback = getValue("PickCallback", null);
				if (!mayScript && (animFrameCallback != null || loadStructCallback != null || messageCallback != null || pauseCallback != null || pickCallback != null))
					System.Console.Out.WriteLine("WARNING!! MAYSCRIPT not found");
				
				System.String scriptParam = getValue("script", "");
				System.String loadParam = getValue("load", null);
				if (loadParam != null)
					scriptParam = "load " + loadParam + ";" + scriptParam;
				script(scriptParam);
			}
			viewer.popHoldRepaint();
		}
		
		internal virtual void  showStatusAndConsole(System.String message)
		{
			//UPGRADE_ISSUE: Method 'java.applet.Applet.showStatus' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaappletAppletshowStatus_javalangString'"
			appletWrapper.showStatus(message);
			consoleMessage(message);
		}
		
		internal virtual void  consoleMessage(System.String message)
		{
			if (jvm12 != null)
				jvm12.consoleMessage(message);
		}
		
		public virtual void  update(System.Drawing.Graphics g)
		{
			//    System.out.println("update called");
			if (viewer == null)
			// it seems that this can happen at startup sometimes
				return ;
			if (showPaintTime)
				startPaintClock();
			System.Drawing.Size size = jvm12orGreater?jvm12.Size:appletWrapper.Size;
			viewer.ScreenDimension = size;
			//UPGRADE_TODO: The equivalent in .NET for method 'java.awt.Graphics.getClipRect' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			System.Drawing.Rectangle rectClip = jvm12orGreater?jvm12.getClipBounds(g):System.Drawing.Rectangle.Truncate(g.ClipBounds);
			++paintCounter;
			if (REQUIRE_PROGRESSBAR && !hasProgressBar && paintCounter < 30 && (paintCounter & 1) == 0)
			{
				printProgressbarMessage(g);
				viewer.notifyRepainted();
			}
			else
			{
				//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
				viewer.renderScreenImage(g, ref size, ref rectClip);
			}
			
			if (showPaintTime)
			{
				stopPaintClock();
				showTimes(10, 10, g);
			}
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'progressbarMsgs'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] progressbarMsgs = new System.String[]{"Jmol developer alert!", "", "progressbar is REQUIRED ... otherwise users", "will have no indicate that the applet is loading", "", "<applet code='JmolApplet' ... >", "  <param name='progressbar' value='true' />", "  <param name='progresscolor' value='blue' />", "  <param name='boxmessage' value='your-favorite-message' />", "  <param name='boxbgcolor' value='#112233' />", "  <param name='boxfgcolor' value='#778899' />", "   ...", "</applet>"};
		
		internal virtual void  printProgressbarMessage(System.Drawing.Graphics g)
		{
			SupportClass.GraphicsManager.manager.SetColor(g, System.Drawing.Color.Yellow);
			g.FillRectangle(SupportClass.GraphicsManager.manager.GetPaint(g), 0, 0, 10000, 10000);
			SupportClass.GraphicsManager.manager.SetColor(g, System.Drawing.Color.Black);
			for (int i = 0, y = 13; i < progressbarMsgs.Length; ++i, y += 13)
			{
				//UPGRADE_TODO: Method 'java.awt.Graphics.drawString' was converted to 'System.Drawing.Graphics.DrawString' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphicsdrawString_javalangString_int_int'"
				g.DrawString(progressbarMsgs[i], SupportClass.GraphicsManager.manager.GetFont(g), SupportClass.GraphicsManager.manager.GetBrush(g), 10, y - SupportClass.GraphicsManager.manager.GetFont(g).GetHeight());
			}
		}
		
		public bool showPaintTime = false;
		
		public virtual void  paint(System.Drawing.Graphics g)
		{
			//    System.out.println("paint called");
			update(g);
		}
		
		//UPGRADE_ISSUE: Class 'java.awt.Event' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEvent'"
		public virtual bool handleEvent(Event e)
		{
			if (viewer == null)
				return false;
			return viewer.handleOldJvm10Event(e);
		}
		
		// code to record last and average times
		// last and average of all the previous times are shown in the status window
		
		internal static int timeLast = 0;
		internal static int timeCount;
		internal static int timeTotal;
		
		internal virtual void  resetTimes()
		{
			timeCount = timeTotal = 0;
			timeLast = - 1;
		}
		
		internal virtual void  recordTime(int time)
		{
			if (timeLast != - 1)
			{
				timeTotal += timeLast;
				++timeCount;
			}
			timeLast = time;
		}
		
		internal long timeBegin;
		internal int lastMotionEventNumber;
		
		internal virtual void  startPaintClock()
		{
			timeBegin = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
			int motionEventNumber = viewer.MotionEventNumber;
			if (lastMotionEventNumber != motionEventNumber)
			{
				lastMotionEventNumber = motionEventNumber;
				resetTimes();
			}
		}
		
		internal virtual void  stopPaintClock()
		{
			int time = (int) ((System.DateTime.Now.Ticks - 621355968000000000) / 10000 - timeBegin);
			recordTime(time);
		}
		
		internal virtual System.String fmt(int num)
		{
			if (num < 0)
				return "---";
			if (num < 10)
				return "  " + num;
			if (num < 100)
				return " " + num;
			return "" + num;
		}
		
		internal virtual void  showTimes(int x, int y, System.Drawing.Graphics g)
		{
			int timeAverage = (timeCount == 0)?- 1:(timeTotal + timeCount / 2) / timeCount; // round, don't truncate
			SupportClass.GraphicsManager.manager.SetColor(g, System.Drawing.Color.Green);
			//UPGRADE_TODO: Method 'java.awt.Graphics.drawString' was converted to 'System.Drawing.Graphics.DrawString' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphicsdrawString_javalangString_int_int'"
			g.DrawString(fmt(timeLast) + "ms : " + fmt(timeAverage) + "ms", SupportClass.GraphicsManager.manager.GetFont(g), SupportClass.GraphicsManager.manager.GetBrush(g), x, y - SupportClass.GraphicsManager.manager.GetFont(g).GetHeight());
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'buttonCallbackBefore '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal System.Object[] buttonCallbackBefore = new System.Object[]{null, false};
		//UPGRADE_NOTE: Final was removed from the declaration of 'buttonCallbackAfter '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal System.Object[] buttonCallbackAfter = new System.Object[]{null, true};
		
		internal bool buttonCallbackNotificationPending;
		internal System.String buttonCallback;
		internal System.String buttonName;
		//UPGRADE_TODO: Class 'netscape.javascript.JSObject' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
		internal JSObject buttonWindow;
		
		//UPGRADE_TODO: Class 'netscape.javascript.JSObject' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
		public virtual void  scriptButton(JSObject buttonWindow, System.String buttonName, System.String script, System.String buttonCallback)
		{
			//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			System.Console.Out.WriteLine(htmlName + " JmolApplet.scriptButton(" + buttonWindow + "," + buttonName + "," + script + "," + buttonCallback);
			if (buttonWindow != null && buttonCallback != null)
			{
				System.Console.Out.WriteLine("!!!! calling back " + buttonCallback);
				buttonCallbackBefore[0] = buttonName;
				System.Console.Out.WriteLine("trying...");
				//UPGRADE_TODO: Method 'netscape.javascript.JSObject.call' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
				buttonWindow.call(buttonCallback, buttonCallbackBefore);
				System.Console.Out.WriteLine("made it");
				
				buttonCallbackNotificationPending = true;
				this.buttonCallback = buttonCallback;
				this.buttonWindow = buttonWindow;
				this.buttonName = buttonName;
			}
			else
			{
				buttonCallbackNotificationPending = false;
			}
			this.script(script);
		}
		
		public virtual void  script(System.String script)
		{
			System.String strError = viewer.evalString(script);
			if (strError == null)
				strError = GT._("Jmol executing script ...");
			myStatusListener.StatusMessage = strError;
		}
		
		internal char inlineNewlineChar = '|';
		
		public virtual void  loadInline(System.String strModel)
		{
			if (strModel != null)
			{
				if (inlineNewlineChar != 0)
				{
					int len = strModel.Length;
					int i;
					for (i = 0; i < len && strModel[0] == ' '; ++i)
					{
					}
					if (i < len && strModel[i] == inlineNewlineChar)
						strModel = strModel.Substring(i + 1);
					strModel = strModel.Replace(inlineNewlineChar, '\n');
				}
				viewer.openStringInline(strModel);
				myStatusListener.StatusMessage = viewer.OpenFileError;
			}
		}
		
		//UPGRADE_TODO: Class 'netscape.javascript.JSObject' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
		public virtual void  loadDOMNode(JSObject DOMNode)
		{
			// This should provide a route to pass in a browser DOM node
			// directly as a JSObject. Unfortunately does not seem to work with
			// current browsers
			viewer.openDOM(DOMNode);
			myStatusListener.StatusMessage = viewer.OpenFileError;
		}
		
		public virtual void  loadNodeId(System.String nodeId)
		{
			if (nodeId != null)
			{
				// Retrieve Node ...
				// First try to find by ID
				System.Object[] idArgs = new System.Object[]{nodeId};
				//UPGRADE_TODO: Class 'netscape.javascript.JSObject' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
				//UPGRADE_TODO: Method 'netscape.javascript.JSObject.call' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
				JSObject tryNode = (JSObject) jsoDocument.call("getElementById", idArgs);
				
				// But that relies on a well-formed CML DTD specifying ID search.
				// Otherwise, search all cml:cml nodes.
				if (tryNode == null)
				{
					System.Object[] searchArgs = new System.Object[]{"http://www.xml-cml.org/schema/cml2/core", "cml"};
					//UPGRADE_TODO: Class 'netscape.javascript.JSObject' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
					//UPGRADE_TODO: Method 'netscape.javascript.JSObject.call' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
					JSObject tryNodeList = (JSObject) jsoDocument.call("getElementsByTagNameNS", searchArgs);
					if (tryNodeList != null)
					{
						//UPGRADE_TODO: Method 'netscape.javascript.JSObject.getMember' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
						for (int i = 0; i < System.Convert.ToInt32(((System.ValueType) tryNodeList.getMember("length"))); i++)
						{
							//UPGRADE_TODO: Method 'netscape.javascript.JSObject.getSlot' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
							//UPGRADE_TODO: Class 'netscape.javascript.JSObject' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
							tryNode = (JSObject) tryNodeList.getSlot(i);
							System.Object[] idArg = new System.Object[]{"id"};
							//UPGRADE_TODO: Method 'netscape.javascript.JSObject.call' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
							System.String idValue = (System.String) tryNode.call("getAttribute", idArg);
							if (nodeId.Equals(idValue))
								break;
						}
					}
				}
				if (tryNode != null)
				{
					viewer.openDOM(tryNode);
					myStatusListener.StatusMessage = viewer.OpenFileError;
				}
			}
		}
		
		
		internal virtual void  loadPopupMenuAsBackgroundTask()
		{
			// no popup on MacOS 9 NetScape
			if (viewer.OperatingSystemName.Equals("Mac OS") && viewer.JavaVersion.Equals("1.1.5"))
				return ;
			new SupportClass.ThreadClass(new System.Threading.ThreadStart(new LoadPopupThread(this).Run)).Start();
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'LoadPopupThread' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		internal class LoadPopupThread : IThreadRunnable
		{
			public LoadPopupThread(Jmol enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(Jmol enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Jmol enclosingInstance;
			public Jmol Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			public virtual void  Run()
			{
				//UPGRADE_TODO: The differences in the type  of parameters for method 'java.lang.Thread.setPriority'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
				SupportClass.ThreadClass.Current().Priority = (System.Threading.ThreadPriority) System.Threading.ThreadPriority.Lowest;
				// long beginTime = System.currentTimeMillis();
				// System.out.println("LoadPopupThread starting ");
				// this is a background task
				JmolPopup popup;
				try
				{
					popup = JmolPopup.newJmolPopup(Enclosing_Instance.viewer);
				}
				catch (System.Exception e)
				{
					System.Console.Out.WriteLine("JmolPopup not loaded");
					return ;
				}
				if (Enclosing_Instance.viewer.haveFrame())
					popup.updateComputedMenus();
				Enclosing_Instance.jmolpopup = popup;
				// long runTime = System.currentTimeMillis() - beginTime;
				// System.out.println("LoadPopupThread finished " + runTime + " ms");
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'MyStatusListener' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		internal class MyStatusListener : JmolStatusListener
		{
			public MyStatusListener(Jmol enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(Jmol enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Jmol enclosingInstance;
			virtual public System.String StatusMessage
			{
				set
				{
					if (value == null)
						return ;
					if (Enclosing_Instance.messageCallback != null && Enclosing_Instance.jsoWindow != null)
					{
						//UPGRADE_TODO: Method 'netscape.javascript.JSObject.call' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
						Enclosing_Instance.jsoWindow.call(Enclosing_Instance.messageCallback, new System.Object[]{Enclosing_Instance.htmlName, value});
					}
					Enclosing_Instance.showStatusAndConsole(value);
				}
				
			}
			public Jmol Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public virtual void  notifyFileLoaded(System.String fullPathName, System.String fileName, System.String modelName, System.Object clientFile, System.String errorMsg)
			{
				if (errorMsg != null)
				{
					Enclosing_Instance.showStatusAndConsole(GT._("File Error:") + errorMsg);
					return ;
				}
				if (fullPathName != null)
					if (Enclosing_Instance.loadStructCallback != null && Enclosing_Instance.jsoWindow != null)
					{
						//UPGRADE_TODO: Method 'netscape.javascript.JSObject.call' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
						Enclosing_Instance.jsoWindow.call(Enclosing_Instance.loadStructCallback, new System.Object[]{Enclosing_Instance.htmlName});
					}
				if (Enclosing_Instance.jmolpopup != null)
					Enclosing_Instance.jmolpopup.updateComputedMenus();
			}
			
			public virtual void  scriptEcho(System.String strEcho)
			{
				scriptStatus(strEcho);
			}
			
			public virtual void  scriptStatus(System.String strStatus)
			{
				if (strStatus != null && Enclosing_Instance.messageCallback != null && Enclosing_Instance.jsoWindow != null)
				{
					//UPGRADE_TODO: Method 'netscape.javascript.JSObject.call' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
					Enclosing_Instance.jsoWindow.call(Enclosing_Instance.messageCallback, new System.Object[]{Enclosing_Instance.htmlName, strStatus});
				}
				Enclosing_Instance.consoleMessage(strStatus);
			}
			
			public virtual void  notifyScriptTermination(System.String errorMessage, int msWalltime)
			{
				Enclosing_Instance.showStatusAndConsole(GT._("Jmol script completed"));
				if (Enclosing_Instance.buttonCallbackNotificationPending)
				{
					System.Console.Out.WriteLine("!!!! calling back " + Enclosing_Instance.buttonCallback);
					Enclosing_Instance.buttonCallbackAfter[0] = Enclosing_Instance.buttonName;
					//UPGRADE_TODO: Method 'netscape.javascript.JSObject.call' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
					Enclosing_Instance.buttonWindow.call(Enclosing_Instance.buttonCallback, Enclosing_Instance.buttonCallbackAfter);
				}
			}
			
			public virtual void  handlePopupMenu(int x, int y)
			{
				if (Enclosing_Instance.jmolpopup != null)
					Enclosing_Instance.jmolpopup.show(x, y);
			}
			
			public virtual void  measureSelection(int atomIndex)
			{
			}
			
			public virtual void  notifyMeasurementsChanged()
			{
			}
			
			public virtual void  notifyFrameChanged(int frameNo)
			{
				//System.out.println("notifyFrameChanged(" + frameNo +")");
				if (Enclosing_Instance.animFrameCallback != null && Enclosing_Instance.jsoWindow != null)
				{
					//UPGRADE_TODO: Method 'netscape.javascript.JSObject.call' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
					Enclosing_Instance.jsoWindow.call(Enclosing_Instance.animFrameCallback, new System.Object[]{Enclosing_Instance.htmlName, (System.Int32) frameNo});
				}
			}
			
			public virtual void  notifyAtomPicked(int atomIndex, System.String strInfo)
			{
				//System.out.println("notifyAtomPicked(" + atomIndex + "," + strInfo +")");
				Enclosing_Instance.showStatusAndConsole(strInfo);
				if (Enclosing_Instance.pickCallback != null && Enclosing_Instance.jsoWindow != null)
				{
					//UPGRADE_TODO: Method 'netscape.javascript.JSObject.call' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1095'"
					Enclosing_Instance.jsoWindow.call(Enclosing_Instance.pickCallback, new System.Object[]{Enclosing_Instance.htmlName, strInfo, (System.Int32) atomIndex});
				}
			}
			
			public virtual void  showUrl(System.String urlString)
			{
				System.Console.Out.WriteLine("showUrl(" + urlString + ")");
				if (urlString != null && urlString.Length > 0)
				{
					try
					{
						//UPGRADE_TODO: Class 'java.net.URL' was converted to a 'System.Uri' which does not throw an exception if a URL specifies an unknown protocol. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1132'"
						System.Uri url = new System.Uri(urlString);
						//UPGRADE_ISSUE: Method 'java.applet.AppletContext.showDocument' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaappletAppletContext'"
						//UPGRADE_ISSUE: Method 'java.applet.Applet.getAppletContext' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaappletAppletgetAppletContext'"
						Enclosing_Instance.appletWrapper.getAppletContext().showDocument(url, "_blank");
					}
					catch (System.UriFormatException mue)
					{
						Enclosing_Instance.showStatusAndConsole("Malformed URL:" + urlString);
					}
				}
			}
			
			public virtual void  showConsole(bool showConsole)
			{
				System.Console.Out.WriteLine("JmolApplet.showConsole(" + showConsole + ")");
				if (Enclosing_Instance.jvm12 != null)
					Enclosing_Instance.jvm12.showConsole(showConsole);
			}
		}
		static Jmol()
		{
			appletInfo = GT._("Jmol Molecular Visualization http://www.jmol.org");
		}
	}
}