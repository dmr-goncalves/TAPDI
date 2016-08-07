using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Emgu.CV.Structure;
using Emgu.CV;


namespace SS_OpenCV {
	public partial class ShowIMG :Form {
		public bool keep = true;
		public ShowIMG() {// IImage img1, IImage img2) {
			InitializeComponent();
			//imageBox1.Image = img1;
			//imageBox2.Image = img2;
		}

		public static void ShowIMGStatic(IImage img1, IImage img2) {
			//new ShowIMG(img1, img2).ShowDialog();
			ShowIMG viewer = new ShowIMG();
			viewer.imageBox1.Image = img1;
			viewer.imageBox2.Image = img2;
			viewer.ShowDialog();
		}

		private void button1_Click(object sender, EventArgs e) {
			keep = false;
			Close();
		}

	}
}