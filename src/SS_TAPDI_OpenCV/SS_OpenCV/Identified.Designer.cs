namespace SS_OpenCV {
	partial class Identified {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.NameSign = new System.Windows.Forms.TextBox();
			this.IdentifiedViewer = new System.Windows.Forms.PictureBox();
			this.Probability = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.IdentifiedViewer)).BeginInit();
			this.SuspendLayout();
			// 
			// NameSign
			// 
			this.NameSign.Location = new System.Drawing.Point(7, 161);
			this.NameSign.Multiline = true;
			this.NameSign.Name = "NameSign";
			this.NameSign.ReadOnly = true;
			this.NameSign.Size = new System.Drawing.Size(170, 40);
			this.NameSign.TabIndex = 1;
			this.NameSign.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// IdentifiedViewer
			// 
			this.IdentifiedViewer.Location = new System.Drawing.Point(17, 5);
			this.IdentifiedViewer.Name = "IdentifiedViewer";
			this.IdentifiedViewer.Size = new System.Drawing.Size(150, 150);
			this.IdentifiedViewer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.IdentifiedViewer.TabIndex = 0;
			this.IdentifiedViewer.TabStop = false;
			// 
			// Probability
			// 
			this.Probability.Location = new System.Drawing.Point(67, 207);
			this.Probability.Name = "Probability";
			this.Probability.ReadOnly = true;
			this.Probability.Size = new System.Drawing.Size(50, 20);
			this.Probability.TabIndex = 2;
			this.Probability.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// Identified
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(184, 231);
			this.Controls.Add(this.Probability);
			this.Controls.Add(this.NameSign);
			this.Controls.Add(this.IdentifiedViewer);
			this.Name = "Identified";
			this.Text = "Identified";
			((System.ComponentModel.ISupportInitialize)(this.IdentifiedViewer)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		public System.Windows.Forms.TextBox NameSign;
		public System.Windows.Forms.TextBox Probability;
		public System.Windows.Forms.PictureBox IdentifiedViewer;
	}
}