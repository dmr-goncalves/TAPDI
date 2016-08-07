namespace SS_OpenCV {
	partial class Entropy_Results {
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
			this.Red = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.Green = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.Blue = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.Gray = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// Red
			// 
			this.Red.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.Red.Location = new System.Drawing.Point(153, 12);
			this.Red.Name = "Red";
			this.Red.ReadOnly = true;
			this.Red.Size = new System.Drawing.Size(69, 20);
			this.Red.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(14, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(108, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Entropy Red Channel";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(14, 41);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(117, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Entropy Green Channel";
			// 
			// Green
			// 
			this.Green.Location = new System.Drawing.Point(153, 38);
			this.Green.Name = "Green";
			this.Green.ReadOnly = true;
			this.Green.Size = new System.Drawing.Size(69, 20);
			this.Green.TabIndex = 2;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(14, 67);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(109, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "Entropy Blue Channel";
			// 
			// Blue
			// 
			this.Blue.Location = new System.Drawing.Point(153, 64);
			this.Blue.Name = "Blue";
			this.Blue.ReadOnly = true;
			this.Blue.Size = new System.Drawing.Size(69, 20);
			this.Blue.TabIndex = 4;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(14, 93);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(127, 13);
			this.label4.TabIndex = 7;
			this.label4.Text = "Entropy Intensity Channel";
			// 
			// Gray
			// 
			this.Gray.Location = new System.Drawing.Point(153, 90);
			this.Gray.Name = "Gray";
			this.Gray.ReadOnly = true;
			this.Gray.Size = new System.Drawing.Size(69, 20);
			this.Gray.TabIndex = 6;
			// 
			// Entropy_Results
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(238, 120);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.Gray);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.Blue);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.Green);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.Red);
			this.Name = "Entropy_Results";
			this.Text = "Entropy Results";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.TextBox Red;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		public System.Windows.Forms.TextBox Green;
		private System.Windows.Forms.Label label3;
		public System.Windows.Forms.TextBox Blue;
		private System.Windows.Forms.Label label4;
		public System.Windows.Forms.TextBox Gray;
	}
}