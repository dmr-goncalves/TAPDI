using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SS_OpenCV {
	public partial class SingleImageFrom :Form {

		public bool keep = true;
		public SingleImageFrom() {
			InitializeComponent();
			keep = true;
		}

		private void button1_Click(object sender, EventArgs e) {
			keep = false;
		}
	}
}
