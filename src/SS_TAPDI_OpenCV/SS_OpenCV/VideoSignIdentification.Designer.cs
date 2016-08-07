namespace SS_OpenCV {
	partial class VideoSignIdentification {
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
			this.label1 = new System.Windows.Forms.Label();
			this.RedDetection = new Emgu.CV.UI.ImageBox();
			this.BlueDetection = new Emgu.CV.UI.ImageBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.Webcam = new System.Windows.Forms.RadioButton();
			this.Video = new System.Windows.Forms.RadioButton();
			this.button1 = new System.Windows.Forms.Button();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.ImageBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.RedDetection)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.BlueDetection)).BeginInit();
			this.SuspendLayout();
			// 
			// ImageBox
			// 
			this.ImageBox.BackColor = System.Drawing.Color.White;
			this.ImageBox.Cursor = System.Windows.Forms.Cursors.Cross;
			this.ImageBox.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
			this.ImageBox.Location = new System.Drawing.Point(12, 29);
			this.ImageBox.Name = "ImageBox";
			this.ImageBox.Size = new System.Drawing.Size(678, 462);
			this.ImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.ImageBox.TabIndex = 5;
			this.ImageBox.TabStop = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(309, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 17);
			this.label1.TabIndex = 6;
			this.label1.Text = "Video Feed";
			// 
			// RedDetection
			// 
			this.RedDetection.BackColor = System.Drawing.Color.White;
			this.RedDetection.Cursor = System.Windows.Forms.Cursors.Cross;
			this.RedDetection.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
			this.RedDetection.Location = new System.Drawing.Point(714, 56);
			this.RedDetection.Name = "RedDetection";
			this.RedDetection.Size = new System.Drawing.Size(132, 109);
			this.RedDetection.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.RedDetection.TabIndex = 7;
			this.RedDetection.TabStop = false;
			// 
			// BlueDetection
			// 
			this.BlueDetection.BackColor = System.Drawing.Color.White;
			this.BlueDetection.Cursor = System.Windows.Forms.Cursors.Cross;
			this.BlueDetection.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
			this.BlueDetection.Location = new System.Drawing.Point(714, 212);
			this.BlueDetection.Name = "BlueDetection";
			this.BlueDetection.Size = new System.Drawing.Size(132, 109);
			this.BlueDetection.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.BlueDetection.TabIndex = 13;
			this.BlueDetection.TabStop = false;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(724, 36);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(112, 17);
			this.label2.TabIndex = 14;
			this.label2.Text = "Red Binarization";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(724, 192);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(114, 17);
			this.label3.TabIndex = 15;
			this.label3.Text = "Blue Binarization";
			// 
			// comboBox1
			// 
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(714, 443);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(132, 21);
			this.comboBox1.TabIndex = 16;
			this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
			// 
			// Webcam
			// 
			this.Webcam.AutoSize = true;
			this.Webcam.Location = new System.Drawing.Point(717, 401);
			this.Webcam.Name = "Webcam";
			this.Webcam.Size = new System.Drawing.Size(68, 17);
			this.Webcam.TabIndex = 17;
			this.Webcam.TabStop = true;
			this.Webcam.Text = "Webcam";
			this.Webcam.UseVisualStyleBackColor = true;
			this.Webcam.CheckedChanged += new System.EventHandler(this.Webcam_CheckedChanged);
			// 
			// Video
			// 
			this.Video.AutoSize = true;
			this.Video.Location = new System.Drawing.Point(717, 424);
			this.Video.Name = "Video";
			this.Video.Size = new System.Drawing.Size(52, 17);
			this.Video.TabIndex = 18;
			this.Video.TabStop = true;
			this.Video.Text = "Video";
			this.Video.UseVisualStyleBackColor = true;
			this.Video.CheckedChanged += new System.EventHandler(this.Video_CheckedChanged);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(744, 470);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(72, 22);
			this.button1.TabIndex = 19;
			this.button1.Text = "Play";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new System.Drawing.Point(792, 415);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(49, 17);
			this.checkBox1.TabIndex = 20;
			this.checkBox1.Text = "GPU";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(714, 364);
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.Size = new System.Drawing.Size(132, 20);
			this.textBox1.TabIndex = 21;
			this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(722, 348);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(117, 13);
			this.label4.TabIndex = 22;
			this.label4.Text = "Frame Processing Time";
			// 
			// VideoSignIdentification
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(859, 502);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.Video);
			this.Controls.Add(this.Webcam);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.BlueDetection);
			this.Controls.Add(this.RedDetection);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.ImageBox);
			this.MaximizeBox = false;
			this.Name = "VideoSignIdentification";
			this.Text = "Video Sign Identification";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VideoSignIdentification_FormClosing);
			((System.ComponentModel.ISupportInitialize)(this.ImageBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.RedDetection)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.BlueDetection)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public Emgu.CV.UI.ImageBox ImageBox;
		private System.Windows.Forms.Label label1;
		public Emgu.CV.UI.ImageBox RedDetection;
		public Emgu.CV.UI.ImageBox BlueDetection;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		public System.Windows.Forms.RadioButton Webcam;
		public System.Windows.Forms.RadioButton Video;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button button1;
	}
}