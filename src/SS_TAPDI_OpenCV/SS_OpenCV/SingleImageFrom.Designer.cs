namespace SS_OpenCV {
	partial class SingleImageFrom {
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
			this.components = new System.ComponentModel.Container();
			this.ImageBox = new Emgu.CV.UI.ImageBox();
			this.button1 = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.ImageBox)).BeginInit();
			this.SuspendLayout();
			// 
			// ImageBox
			// 
			this.ImageBox.Cursor = System.Windows.Forms.Cursors.Cross;
			this.ImageBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.ImageBox.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
			this.ImageBox.Location = new System.Drawing.Point(0, 0);
			this.ImageBox.Name = "ImageBox";
			this.ImageBox.Size = new System.Drawing.Size(351, 261);
			this.ImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.ImageBox.TabIndex = 4;
			this.ImageBox.TabStop = false;
			// 
			// button1
			// 
			this.button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.button1.Location = new System.Drawing.Point(138, 287);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 5;
			this.button1.Text = "OK";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// SingleImageFrom
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(351, 324);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.ImageBox);
			this.Name = "SingleImageFrom";
			this.Text = "Image";
			((System.ComponentModel.ISupportInitialize)(this.ImageBox)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		public Emgu.CV.UI.ImageBox ImageBox;
		public System.Windows.Forms.Button button1;
	}
}