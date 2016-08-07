using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;

namespace SS_OpenCV {
	public partial class Histogram :Form {
		public Histogram(int[] arrayGray, int[] arrayB, int[] arrayG, int[] arrayR, int axisLenght) {

			InitializeComponent();

			// get a reference to the GraphPane
			GraphPane myPane = zedGraphControl1.GraphPane;

			// Set the Titles
			myPane.Title.Text = "Histogram";
			myPane.XAxis.Title.Text = "Intensity";
			myPane.YAxis.Title.Text = "Count";

			//list points
			PointPairList listGray = new PointPairList();
			PointPairList listB = new PointPairList();
			PointPairList listG = new PointPairList();
			PointPairList listR = new PointPairList();

			for(int i = 0; i < arrayGray.Length; i++) {
				listGray.Add(i, arrayGray[i]);
				listB.Add(i, arrayB[i]);
				listG.Add(i, arrayG[i]);
				listR.Add(i, arrayR[i]);
			}

			//add bar series
			LineItem Intensity = myPane.AddCurve("Intensity", listGray, Color.Black, SymbolType.None);
			LineItem Blue = myPane.AddCurve("Blue", listB, Color.Blue, SymbolType.None);
			LineItem Green = myPane.AddCurve("Green", listG, Color.Green, SymbolType.None);
			LineItem Red = myPane.AddCurve("Red", listR, Color.Red, SymbolType.None);

			Intensity.Line.Width = 2.0F;
			Blue.Line.Width = 1.0F;
			Green.Line.Width = 1.0F;
			Red.Line.Width = 1.0F;

			myPane.XAxis.Scale.Min = 0;
			myPane.XAxis.Scale.Max = axisLenght;

			zedGraphControl1.AxisChange();
		}
	}
}
