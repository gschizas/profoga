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
namespace org.jmol.api
{
	
	public interface JmolStatusListener
	{
		System.String StatusMessage
		{
			set;
			
		}
		void  notifyFileLoaded(System.String fullPathName, System.String fileName, System.String modelName, System.Object clientFile, System.String errorMessage);
		
		void  scriptEcho(System.String strEcho);
		
		void  scriptStatus(System.String strStatus);
		
		void  notifyScriptTermination(System.String statusMessage, int msWalltime);
		
		void  handlePopupMenu(int x, int y);
		
		void  notifyMeasurementsChanged();
		
		void  notifyFrameChanged(int frameNo);
		
		void  notifyAtomPicked(int atomIndex, System.String strInfo);
		
		void  showUrl(System.String url);
		
		void  showConsole(bool showConsole);
	}
}