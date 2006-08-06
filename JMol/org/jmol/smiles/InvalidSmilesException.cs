/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-03-27 17:13:59 +0200 (lun., 27 mars 2006) $
* $Revision: 4771 $
*
* Copyright (C) 2005  The Jmol Development Team
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
namespace org.jmol.smiles
{
	
	/// <summary> Exception thrown for invalid SMILES String</summary>
	[Serializable]
	public class InvalidSmilesException:System.Exception
	{
		
		/// <summary> Constructs a <code>InvalideSmilesException</code> without any detail.</summary>
		public InvalidSmilesException():base()
		{
		}
		
		/// <summary> Constructs a <code>InvalidSmilesException</code> with a detail message.
		/// 
		/// </summary>
		/// <param name="message">The detail message.
		/// </param>
		public InvalidSmilesException(System.String message):base(message)
		{
		}
		
		/// <summary> Contructs a <code>InvalidSmilesException</code> with the specified cause and
		/// a detail message of <tt>(cause == null ? null : cause.toString())</tt>
		/// (which typically contains the class and detail message of <tt>cause</tt>).
		/// 
		/// </summary>
		/// <param name="cause">The cause.
		/// </param>
		//UPGRADE_NOTE: Exception 'java.lang.Throwable' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
		public InvalidSmilesException(System.Exception cause):base(cause)
		{
		}
		
		/// <summary> Construcst a <code>InvalidSmilesException</code> with the specified detail
		/// message and cause.
		/// 
		/// </summary>
		/// <param name="message">The detail message.
		/// </param>
		/// <param name="cause">The cause.
		/// </param>
		//UPGRADE_NOTE: Exception 'java.lang.Throwable' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
		public InvalidSmilesException(System.String message, System.Exception cause):base(message, cause)
		{
		}
	}
}