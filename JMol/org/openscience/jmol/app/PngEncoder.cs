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
	
	/// <summary> PngEncoder takes a Java Image object and creates a byte string which can be saved as a PNG file.
	/// The Image is presumed to use the DirectColorModel.
	/// 
	/// Thanks to Jay Denny at KeyPoint Software
	/// http://www.keypoint.com/
	/// who let me develop this code on company time.
	/// 
	/// You may contact me with (probably very-much-needed) improvements,
	/// comments, and bug fixes at:
	/// 
	/// david@catcode.com
	/// 
	/// </summary>
	/// <author>  J. David Eisenberg
	/// </author>
	/// <author>  http://catcode.com/pngencoder/
	/// </author>
	/// <author>  Christian Ribeaud (christian.ribeaud@genedata.com)
	/// </author>
	/// <version>  1.4, 31 March 2000
	/// </version>
	public class PngEncoder:System.Object
	{
		/// <summary> Set the image to be encoded
		/// 
		/// </summary>
		/// <param name="image">A Java Image object which uses the DirectColorModel
		/// </param>
		/// <seealso cref="java.awt.Image">
		/// </seealso>
		/// <seealso cref="java.awt.image.DirectColorModel">
		/// </seealso>
		virtual public System.Drawing.Image Image
		{
			set
			{
				this.image = value;
				pngBytes = null;
			}
			
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Retrieve alpha encoding status.
		/// 
		/// </summary>
		/// <returns> boolean false=no, true=yes
		/// </returns>
		/// <summary> Set the alpha encoding on or off.
		/// 
		/// </summary>
		/// <param name="encodeAlpha"> false=no, true=yes
		/// </param>
		virtual public bool EncodeAlpha
		{
			get
			{
				return encodeAlpha;
			}
			
			set
			{
				this.encodeAlpha = value;
			}
			
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Retrieve filtering scheme
		/// 
		/// </summary>
		/// <returns> int (see constant list)
		/// </returns>
		/// <summary> Set the filter to use
		/// 
		/// </summary>
		/// <param name="whichFilter">from constant list
		/// </param>
		virtual public int Filter
		{
			get
			{
				return filter;
			}
			
			set
			{
				this.filter = FILTER_NONE;
				if (value <= FILTER_LAST)
				{
					this.filter = value;
				}
			}
			
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Retrieve compression level
		/// 
		/// </summary>
		/// <returns> int in range 0-9
		/// </returns>
		/// <summary> Set the compression level to use
		/// 
		/// </summary>
		/// <param name="level">0 through 9
		/// </param>
		virtual public int CompressionLevel
		{
			get
			{
				return compressionLevel;
			}
			
			set
			{
				if ((value >= 0) && (value <= 9))
				{
					this.compressionLevel = value;
				}
			}
			
		}
		
		/// <summary>Constant specifying that alpha channel should be encoded. </summary>
		public const bool ENCODE_ALPHA = true;
		
		/// <summary>Constant specifying that alpha channel should not be encoded. </summary>
		public const bool NO_ALPHA = false;
		
		/// <summary>Constants for filters </summary>
		public const int FILTER_NONE = 0;
		public const int FILTER_SUB = 1;
		public const int FILTER_UP = 2;
		public const int FILTER_LAST = 2;
		
		protected internal sbyte[] pngBytes;
		protected internal sbyte[] priorRow;
		protected internal sbyte[] leftBytes;
		protected internal System.Drawing.Image image;
		protected internal int width, height;
		protected internal int bytePos, maxPos;
		protected internal int hdrPos, dataPos, endPos;
		//UPGRADE_ISSUE: Class 'java.util.zip.CRC32' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilzipCRC32'"
		//UPGRADE_ISSUE: Constructor 'java.util.zip.CRC32.CRC32' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilzipCRC32'"
		protected internal CRC32 crc = new CRC32();
		protected internal long crcValue;
		protected internal bool encodeAlpha;
		protected internal int filter;
		protected internal int bytesPerPixel;
		protected internal int compressionLevel;
		
		/// <summary> Class constructor
		/// 
		/// </summary>
		public PngEncoder():this(null, false, FILTER_NONE, 0)
		{
		}
		
		/// <summary> Class constructor specifying Image to encode, with no alpha channel encoding.
		/// 
		/// </summary>
		/// <param name="image">A Java Image object which uses the DirectColorModel
		/// </param>
		/// <seealso cref="java.awt.Image">
		/// </seealso>
		public PngEncoder(System.Drawing.Image image):this(image, false, FILTER_NONE, 0)
		{
		}
		
		/// <summary> Class constructor specifying Image to encode, and whether to encode alpha.
		/// 
		/// </summary>
		/// <param name="image">A Java Image object which uses the DirectColorModel
		/// </param>
		/// <param name="encodeAlpha">Encode the alpha channel? false=no; true=yes
		/// </param>
		/// <seealso cref="java.awt.Image">
		/// </seealso>
		public PngEncoder(System.Drawing.Image image, bool encodeAlpha):this(image, encodeAlpha, FILTER_NONE, 0)
		{
		}
		
		/// <summary> Class constructor specifying Image to encode, whether to encode alpha, and filter to use.
		/// 
		/// </summary>
		/// <param name="image">A Java Image object which uses the DirectColorModel
		/// </param>
		/// <param name="encodeAlpha">Encode the alpha channel? false=no; true=yes
		/// </param>
		/// <param name="whichFilter">0=none, 1=sub, 2=up
		/// </param>
		/// <seealso cref="java.awt.Image">
		/// </seealso>
		public PngEncoder(System.Drawing.Image image, bool encodeAlpha, int whichFilter):this(image, encodeAlpha, whichFilter, 0)
		{
		}
		
		
		/// <summary> Class constructor specifying Image source to encode, whether to encode alpha, filter to use, and compression level.
		/// 
		/// </summary>
		/// <param name="image">A Java Image object
		/// </param>
		/// <param name="encodeAlpha">Encode the alpha channel? false=no; true=yes
		/// </param>
		/// <param name="whichFilter">0=none, 1=sub, 2=up
		/// </param>
		/// <param name="compLevel">0..9
		/// </param>
		/// <seealso cref="java.awt.Image">
		/// </seealso>
		public PngEncoder(System.Drawing.Image image, bool encodeAlpha, int whichFilter, int compLevel)
		{
			
			this.image = image;
			this.encodeAlpha = encodeAlpha;
			Filter = whichFilter;
			if ((compLevel >= 0) && (compLevel <= 9))
			{
				this.compressionLevel = compLevel;
			}
		}
		
		/// <summary> Creates an array of bytes that is the PNG equivalent of the current image, specifying whether to encode alpha or not.
		/// 
		/// </summary>
		/// <param name="encodeAlpha">boolean false=no alpha, true=encode alpha
		/// </param>
		/// <returns> an array of bytes, or null if there was a problem
		/// </returns>
		public virtual sbyte[] pngEncode(bool encodeAlpha)
		{
			
			sbyte[] pngIdBytes = new sbyte[]{- 119, 80, 78, 71, 13, 10, 26, 10};
			
			if (image == null)
			{
				return null;
			}
			width = image.Width;
			height = image.Height;
			//this.image = image;
			
			/*
			* start with an array that is big enough to hold all the pixels
			* (plus filter bytes), and an extra 200 bytes for header info
			*/
			pngBytes = new sbyte[((width + 1) * height * 3) + 200];
			
			/*
			* keep track of largest byte written to the array
			*/
			maxPos = 0;
			
			bytePos = writeBytes(pngIdBytes, 0);
			hdrPos = bytePos;
			writeHeader();
			dataPos = bytePos;
			if (writeImageData())
			{
				writeEnd();
				pngBytes = resizeByteArray(pngBytes, maxPos);
			}
			else
			{
				pngBytes = null;
			}
			return pngBytes;
		}
		
		/// <summary> Creates an array of bytes that is the PNG equivalent of the current image.
		/// Alpha encoding is determined by its setting in the constructor.
		/// 
		/// </summary>
		/// <returns> an array of bytes, or null if there was a problem
		/// </returns>
		public virtual sbyte[] pngEncode()
		{
			return pngEncode(encodeAlpha);
		}
		
		/// <summary> Increase or decrease the length of a byte array.
		/// 
		/// </summary>
		/// <param name="array">The original array.
		/// </param>
		/// <param name="newLength">The length you wish the new array to have.
		/// </param>
		/// <returns> Array of newly desired length. If shorter than the
		/// original, the trailing elements are truncated.
		/// </returns>
		protected internal virtual sbyte[] resizeByteArray(sbyte[] array, int newLength)
		{
			
			sbyte[] newArray = new sbyte[newLength];
			int oldLength = array.Length;
			
			Array.Copy(array, 0, newArray, 0, System.Math.Min(oldLength, newLength));
			return newArray;
		}
		
		/// <summary> Write an array of bytes into the pngBytes array.
		/// Note: This routine has the side effect of updating
		/// maxPos, the largest element written in the array.
		/// The array is resized by 1000 bytes or the length
		/// of the data to be written, whichever is larger.
		/// 
		/// </summary>
		/// <param name="data">The data to be written into pngBytes.
		/// </param>
		/// <param name="offset">The starting point to write to.
		/// </param>
		/// <returns> The next place to be written to in the pngBytes array.
		/// </returns>
		protected internal virtual int writeBytes(sbyte[] data, int offset)
		{
			
			maxPos = System.Math.Max(maxPos, offset + data.Length);
			if (data.Length + offset > pngBytes.Length)
			{
				pngBytes = resizeByteArray(pngBytes, pngBytes.Length + System.Math.Max(1000, data.Length));
			}
			Array.Copy(data, 0, pngBytes, offset, data.Length);
			return offset + data.Length;
		}
		
		/// <summary> Write an array of bytes into the pngBytes array, specifying number of bytes to write.
		/// Note: This routine has the side effect of updating
		/// maxPos, the largest element written in the array.
		/// The array is resized by 1000 bytes or the length
		/// of the data to be written, whichever is larger.
		/// 
		/// </summary>
		/// <param name="data">The data to be written into pngBytes.
		/// </param>
		/// <param name="nBytes">The number of bytes to be written.
		/// </param>
		/// <param name="offset">The starting point to write to.
		/// </param>
		/// <returns> The next place to be written to in the pngBytes array.
		/// </returns>
		protected internal virtual int writeBytes(sbyte[] data, int nBytes, int offset)
		{
			
			maxPos = System.Math.Max(maxPos, offset + nBytes);
			if (nBytes + offset > pngBytes.Length)
			{
				pngBytes = resizeByteArray(pngBytes, pngBytes.Length + System.Math.Max(1000, nBytes));
			}
			Array.Copy(data, 0, pngBytes, offset, nBytes);
			return offset + nBytes;
		}
		
		/// <summary> Write a two-byte integer into the pngBytes array at a given position.
		/// 
		/// </summary>
		/// <param name="n">The integer to be written into pngBytes.
		/// </param>
		/// <param name="offset">The starting point to write to.
		/// </param>
		/// <returns> The next place to be written to in the pngBytes array.
		/// </returns>
		protected internal virtual int writeInt2(int n, int offset)
		{
			sbyte[] temp = new sbyte[]{(sbyte) ((n >> 8) & 0xff), (sbyte) (n & 0xff)};
			return writeBytes(temp, offset);
		}
		
		/// <summary> Write a four-byte integer into the pngBytes array at a given position.
		/// 
		/// </summary>
		/// <param name="n">The integer to be written into pngBytes.
		/// </param>
		/// <param name="offset">The starting point to write to.
		/// </param>
		/// <returns> The next place to be written to in the pngBytes array.
		/// </returns>
		protected internal virtual int writeInt4(int n, int offset)
		{
			
			sbyte[] temp = new sbyte[]{(sbyte) ((n >> 24) & 0xff), (sbyte) ((n >> 16) & 0xff), (sbyte) ((n >> 8) & 0xff), (sbyte) (n & 0xff)};
			return writeBytes(temp, offset);
		}
		
		/// <summary> Write a single byte into the pngBytes array at a given position.
		/// 
		/// </summary>
		/// <param name="b">The byte to be written into pngBytes.
		/// </param>
		/// <param name="offset">The starting point to write to.
		/// </param>
		/// <returns> The next place to be written to in the pngBytes array.
		/// </returns>
		protected internal virtual int writeByte(int b, int offset)
		{
			sbyte[] temp = new sbyte[]{(sbyte) b};
			return writeBytes(temp, offset);
		}
		
		/// <summary> Write a string into the pngBytes array at a given position.
		/// This uses the getBytes method, so the encoding used will
		/// be its default.
		/// 
		/// </summary>
		/// <param name="s">The string to be written into pngBytes.
		/// </param>
		/// <param name="offset">The starting point to write to.
		/// </param>
		/// <returns> The next place to be written to in the pngBytes array.
		/// </returns>
		/// <seealso cref="java.lang.String.getBytes()">
		/// </seealso>
		protected internal virtual int writeString(System.String s, int offset)
		{
			return writeBytes(SupportClass.ToSByteArray(SupportClass.ToByteArray(s)), offset);
		}
		
		/// <summary> Write a PNG "IHDR" chunk into the pngBytes array.</summary>
		protected internal virtual void  writeHeader()
		{
			
			int startPos;
			
			startPos = bytePos = writeInt4(13, bytePos);
			bytePos = writeString("IHDR", bytePos);
			width = image.Width;
			height = image.Height;
			bytePos = writeInt4(width, bytePos);
			bytePos = writeInt4(height, bytePos);
			bytePos = writeByte(8, bytePos); // bit depth
			bytePos = writeByte((encodeAlpha)?6:2, bytePos); // direct model
			bytePos = writeByte(0, bytePos); // compression method
			bytePos = writeByte(0, bytePos); // filter method
			bytePos = writeByte(0, bytePos); // no interlace
			//UPGRADE_ISSUE: Method 'java.util.zip.CRC32.reset' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilzipCRC32'"
			crc.reset();
			//UPGRADE_ISSUE: Method 'java.util.zip.CRC32.update' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilzipCRC32'"
			crc.update(pngBytes, startPos, bytePos - startPos);
			//UPGRADE_ISSUE: Method 'java.util.zip.CRC32.getValue' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilzipCRC32'"
			crcValue = crc.getValue();
			bytePos = writeInt4((int) crcValue, bytePos);
		}
		
		/// <summary> Perform "sub" filtering on the given row.
		/// Uses temporary array leftBytes to store the original values
		/// of the previous pixels.  The array is 16 bytes long, which
		/// will easily hold two-byte samples plus two-byte alpha.
		/// 
		/// </summary>
		/// <param name="pixels">The array holding the scan lines being built
		/// </param>
		/// <param name="startPos">Starting position within pixels of bytes to be filtered.
		/// </param>
		/// <param name="width">Width of a scanline in pixels.
		/// </param>
		protected internal virtual void  filterSub(sbyte[] pixels, int startPos, int width)
		{
			
			int i;
			int offset = bytesPerPixel;
			int actualStart = startPos + offset;
			int nBytes = width * bytesPerPixel;
			int leftInsert = offset;
			int leftExtract = 0;
			//byte current_byte;
			
			for (i = actualStart; i < startPos + nBytes; i++)
			{
				leftBytes[leftInsert] = pixels[i];
				pixels[i] = (sbyte) ((pixels[i] - leftBytes[leftExtract]) % 256);
				leftInsert = (leftInsert + 1) % 0x0f;
				leftExtract = (leftExtract + 1) % 0x0f;
			}
		}
		
		/// <summary> Perform "up" filtering on the given row.
		/// Side effect: refills the prior row with current row
		/// 
		/// </summary>
		/// <param name="pixels">The array holding the scan lines being built
		/// </param>
		/// <param name="startPos">Starting position within pixels of bytes to be filtered.
		/// </param>
		/// <param name="width">Width of a scanline in pixels.
		/// </param>
		protected internal virtual void  filterUp(sbyte[] pixels, int startPos, int width)
		{
			
			int i, nBytes;
			sbyte current_byte;
			
			nBytes = width * bytesPerPixel;
			
			for (i = 0; i < nBytes; i++)
			{
				current_byte = pixels[startPos + i];
				pixels[startPos + i] = (sbyte) ((pixels[startPos + i] - priorRow[i]) % 256);
				priorRow[i] = current_byte;
			}
		}
		
		/// <summary> Write the image data into the pngBytes array.
		/// This will write one or more PNG "IDAT" chunks. In order
		/// to conserve memory, this method grabs as many rows as will
		/// fit into 32K bytes, or the whole image; whichever is less.
		/// 
		/// 
		/// </summary>
		/// <returns> true if no errors; false if error grabbing pixels
		/// </returns>
		protected internal virtual bool writeImageData()
		{
			
			int rowsLeft = height; // number of rows remaining to write
			int startRow = 0; // starting row to process this time through
			int nRows; // how many rows to grab at a time
			
			sbyte[] scanLines; // the scan lines to be compressed
			int scanPos; // where we are in the scan lines
			int startPos; // where this line's actual pixels start (used for filtering)
			
			sbyte[] compressedLines; // the resultant compressed lines
			int nCompressed; // how big is the compressed area?
			
			//int depth;                 // color depth ( handle only 8 or 32 )
			
			SupportClass.PixelCapturer pg;
			
			bytesPerPixel = (encodeAlpha)?4:3;
			
			//UPGRADE_ISSUE: Class 'java.util.zip.Deflater' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilzipDeflater'"
			//UPGRADE_ISSUE: Constructor 'java.util.zip.Deflater.Deflater' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilzipDeflater'"
			Deflater scrunch = new Deflater(compressionLevel);
			System.IO.MemoryStream outBytes = new System.IO.MemoryStream(1024);
			
			//UPGRADE_ISSUE: Class 'java.util.zip.DeflaterOutputStream' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilzipDeflaterOutputStream'"
			//UPGRADE_ISSUE: Constructor 'java.util.zip.DeflaterOutputStream.DeflaterOutputStream' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilzipDeflaterOutputStream'"
			DeflaterOutputStream compBytes = new DeflaterOutputStream(outBytes, scrunch);
			try
			{
				while (rowsLeft > 0)
				{
					nRows = System.Math.Min(32767 / (width * (bytesPerPixel + 1)), rowsLeft);
					
					// nRows = rowsLeft;
					
					int[] pixels = new int[width * nRows];
					
					pg = new SupportClass.PixelCapturer(image, 0, startRow, width, nRows, pixels, 0, width);
					try
					{
						pg.CapturePixels();
					}
					catch (System.Exception e)
					{
						System.Console.Error.WriteLine("interrupted waiting for pixels!");
						return false;
					}
					//UPGRADE_ISSUE: Method 'java.awt.image.PixelGrabber.getStatus' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtimagePixelGrabbergetStatus'"
					//UPGRADE_ISSUE: Field 'java.awt.image.ImageObserver.ABORT' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtimageImageObserver'"
					if ((pg.getStatus() & ImageObserver.ABORT) != 0)
					{
						System.Console.Error.WriteLine("image fetch aborted or errored");
						return false;
					}
					
					/*
					* Create a data chunk. scanLines adds "nRows" for
					* the filter bytes.
					*/
					scanLines = new sbyte[width * nRows * bytesPerPixel + nRows];
					
					if (filter == FILTER_SUB)
					{
						leftBytes = new sbyte[16];
					}
					if (filter == FILTER_UP)
					{
						priorRow = new sbyte[width * bytesPerPixel];
					}
					
					scanPos = 0;
					startPos = 1;
					for (int i = 0; i < width * nRows; i++)
					{
						if (i % width == 0)
						{
							scanLines[scanPos++] = (sbyte) filter;
							startPos = scanPos;
						}
						scanLines[scanPos++] = (sbyte) ((pixels[i] >> 16) & 0xff);
						scanLines[scanPos++] = (sbyte) ((pixels[i] >> 8) & 0xff);
						scanLines[scanPos++] = (sbyte) ((pixels[i]) & 0xff);
						if (encodeAlpha)
						{
							scanLines[scanPos++] = (sbyte) ((pixels[i] >> 24) & 0xff);
						}
						if ((i % width == width - 1) && (filter != FILTER_NONE))
						{
							if (filter == FILTER_SUB)
							{
								filterSub(scanLines, startPos, width);
							}
							if (filter == FILTER_UP)
							{
								filterUp(scanLines, startPos, width);
							}
						}
					}
					
					/*
					* Write these lines to the output area
					*/
					//UPGRADE_ISSUE: Method 'java.util.zip.DeflaterOutputStream.write' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilzipDeflaterOutputStream'"
					compBytes.write(scanLines, 0, scanPos);
					
					
					startRow += nRows;
					rowsLeft -= nRows;
				}
				//UPGRADE_ISSUE: Method 'java.util.zip.DeflaterOutputStream.close' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilzipDeflaterOutputStream'"
				compBytes.close();
				
				/*
				* Write the compressed bytes
				*/
				compressedLines = SupportClass.ToSByteArray(outBytes.ToArray());
				nCompressed = compressedLines.Length;
				
				//UPGRADE_ISSUE: Method 'java.util.zip.CRC32.reset' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilzipCRC32'"
				crc.reset();
				bytePos = writeInt4(nCompressed, bytePos);
				bytePos = writeString("IDAT", bytePos);
				//UPGRADE_ISSUE: Method 'java.util.zip.CRC32.update' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilzipCRC32'"
				crc.update(SupportClass.ToSByteArray(SupportClass.ToByteArray("IDAT")));
				bytePos = writeBytes(compressedLines, nCompressed, bytePos);
				//UPGRADE_ISSUE: Method 'java.util.zip.CRC32.update' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilzipCRC32'"
				crc.update(compressedLines, 0, nCompressed);
				
				//UPGRADE_ISSUE: Method 'java.util.zip.CRC32.getValue' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilzipCRC32'"
				crcValue = crc.getValue();
				bytePos = writeInt4((int) crcValue, bytePos);
				//UPGRADE_ISSUE: Method 'java.util.zip.Deflater.finish' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilzipDeflater'"
				scrunch.finish();
				return true;
			}
			catch (System.IO.IOException e)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				System.Console.Error.WriteLine(e.ToString());
				return false;
			}
		}
		
		/// <summary> Write a PNG "IEND" chunk into the pngBytes array.</summary>
		protected internal virtual void  writeEnd()
		{
			
			bytePos = writeInt4(0, bytePos);
			bytePos = writeString("IEND", bytePos);
			//UPGRADE_ISSUE: Method 'java.util.zip.CRC32.reset' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilzipCRC32'"
			crc.reset();
			//UPGRADE_ISSUE: Method 'java.util.zip.CRC32.update' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilzipCRC32'"
			crc.update(SupportClass.ToSByteArray(SupportClass.ToByteArray("IEND")));
			//UPGRADE_ISSUE: Method 'java.util.zip.CRC32.getValue' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilzipCRC32'"
			crcValue = crc.getValue();
			bytePos = writeInt4((int) crcValue, bytePos);
		}
	}
}