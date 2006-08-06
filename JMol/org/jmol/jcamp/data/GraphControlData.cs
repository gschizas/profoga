using System;
namespace org.jmol.jcamp.data
{
	
	public class GraphControlData
	{
		internal double realFistX;
		internal double realLastX;
		internal double lastRealFirstX;
		internal double lastRealLastX;
		internal double lastFirstX;
		internal double lastLastX;
		
		internal double savFirstX;
		internal double savLastX;
		
		internal bool containsClickablePeaks; // Variable to indicate the presence of clickable peaks in the data file provided
		internal int numberOfClickablePeaks; // Number of clickable peaks present in the file
		
		internal double[] peakStart; //Starting points of ranges of clickable peaks
		internal double[] peakStop; //Ending points of ranges of clickable peaks
		internal double[] peakHtml;
	}
}