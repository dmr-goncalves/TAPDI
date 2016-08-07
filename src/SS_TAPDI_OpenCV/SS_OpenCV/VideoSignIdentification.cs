using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.UI;
using Emgu.CV.Structure;

namespace SS_OpenCV {
	public partial class VideoSignIdentification :Form {
		public bool keep = false;
		public string directory;
		bool first, GPU;

		Capture capture;
		Image<Bgr, byte> img;
		Image<Bgr, byte> img_red;
		Image<Bgr, byte> img_blue;
		int[] segmentation;
		double total_frames = 0;
		double frame = 0;

		public VideoSignIdentification() {
			InitializeComponent();
			first = true;
			GPU = true;

			comboBox1.Items.Add("video 1");
			comboBox1.Items.Add("video 2");
			comboBox1.Items.Add("video 3");
			comboBox1.Items.Add("video 4");
			comboBox1.Items.Add("video 5");
			comboBox1.Items.Add("video 6");
			comboBox1.Items.Add("video 7");
			comboBox1.Items.Add("video 8");
			comboBox1.Items.Add("video 9");

			comboBox1.SelectedItem = "video 1";

			Video.Select();
			checkBox1.Checked = true;
			
			textBox1.Text = "0.00 ms" + " / " + "0.00 FPS";
		}

		private void VideoSignIdentification_FormClosing(object sender, FormClosingEventArgs e) {
			if(!first){
				if(GPU) {
					if(keep) {
						Application.Idle -= GPUFrameProcessing;
						capture.Stop();
						capture.Dispose();
					} else {
						capture.Stop();
						capture.Dispose();
					}
				} else {
					if(keep) {
						Application.Idle -= CPUFrameProcessing;
						capture.Stop();
						capture.Dispose();
					} else {
						capture.Stop();
						capture.Dispose();
					}
				}
			}
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
			if(comboBox1.SelectedItem.ToString() == "video 1") {
				directory = @".\Videos\video1.mp4";
			} else if(comboBox1.SelectedItem.ToString() == "video 2") {
				directory = @".\Videos\video2.mp4";
			} else if(comboBox1.SelectedItem.ToString() == "video 3") {
				directory = @".\Videos\video3.mp4";
			} else if(comboBox1.SelectedItem.ToString() == "video 4") {
				directory = @".\Videos\video4.mp4";
			} else if(comboBox1.SelectedItem.ToString() == "video 5") {
				directory = @".\Videos\video5.mp4";
			} else if(comboBox1.SelectedItem.ToString() == "video 6") {
				directory = @".\Videos\video6.mp4";
			} else if(comboBox1.SelectedItem.ToString() == "video 7") {
				directory = @".\Videos\video7.mp4";
			} else if(comboBox1.SelectedItem.ToString() == "video 8") {
				directory = @".\Videos\video8.mp4";
			} else if(comboBox1.SelectedItem.ToString() == "video 9") {
				directory = @".\Videos\video9.mp4";
			}
		}

		private void GPUFrameProcessing(object sender, EventArgs e) {
			
			img = capture.QueryFrame().Clone();
			img = capture.QueryFrame().Clone();
			img = capture.QueryFrame().Clone();
			img = capture.QueryFrame().Clone();

			frame += 4;
			if(frame <= total_frames - 4) {
				Image<Bgr, byte> _img = img.Copy();

				DateTime t1 = DateTime.Now;
				OpenCL_Class.ColorConvertion_Binarization_Projections_TemplateMatching(_img, out img_red, out img_blue);
				DateTime t2 = DateTime.Now;

				textBox1.Text = (t2 - t1).TotalMilliseconds.ToString("n2") + " ms" + " / " + (1 / (t2-t1).TotalSeconds).ToString("n2") + " FPS";

				this.RedDetection.Image = img_red;
				this.BlueDetection.Image = img_blue;
				this.ImageBox.Image = _img;
			} else {
				stopVideo();
			}

			GC.Collect();
		}

		private void CPUFrameProcessing(object sender, EventArgs e) {
			
			img = capture.QueryFrame().Clone();
			img = capture.QueryFrame().Clone();
			img = capture.QueryFrame().Clone();
			img = capture.QueryFrame().Clone();

			frame += 4;
			if(frame <= total_frames - 4) {
				Image<Bgr, byte> _img = img.Copy();

				DateTime t1 = DateTime.Now;

				img_blue = ImageClass.BlueBinarization(img);
				segmentation = ImageClass.Segmentation(img_blue, 'b');
				ImageClass.TemplateMatching(img, segmentation, MainForm.DataBase);

				img_red = ImageClass.RedBinarization(img);
				segmentation = ImageClass.Segmentation(img_red, 'r');
				ImageClass.TemplateMatching(img, segmentation, MainForm.DataBase);

				DateTime t2 = DateTime.Now;

				textBox1.Text = (t2 - t1).TotalMilliseconds.ToString("n2") + " ms" + " / " + (1 / (t2 - t1).TotalSeconds).ToString("n2") + " FPS";

				this.RedDetection.Image = img_red;
				this.BlueDetection.Image = img_blue;
				this.ImageBox.Image = img;
			} else {
				stopVideo();
			}

			GC.Collect();
		}

		private void stopVideo() {
			if(GPU) {
				button1.Text = "Play";
				capture.Stop();

				Application.Idle -= GPUFrameProcessing;
				capture.Dispose();

				keep = false;

				ImageBox.Image = null;
				RedDetection.Image = null;
				BlueDetection.Image = null;

				textBox1.Text = "0.00 ms" + " / " + "0.00 FPS";
			} else {
				button1.Text = "Play";
				capture.Stop();

				Application.Idle -= CPUFrameProcessing;

				capture.Dispose();

				keep = false;

				ImageBox.Image = null;
				RedDetection.Image = null;
				BlueDetection.Image = null;

				textBox1.Text = "0.00 ms" + " / " + "0.00 FPS";
			}
		}

		private void button1_Click(object sender, EventArgs e) {
			first = false;

			if(GPU) {
				if(Webcam.Checked) {
					if(button1.Text == "Start") {
						button1.Text = "Stop";

						capture = new Capture();
						Application.Idle += GPUFrameProcessing;
						keep = true;

					} else if(button1.Text == "Stop") {
						button1.Text = "Start";

						Application.Idle -= GPUFrameProcessing;
						capture.Dispose();
						ImageBox.Image = null;
						RedDetection.Image = null;
						BlueDetection.Image = null;
						keep = false;

						textBox1.Text = "0.00 ms" + " / " + "0.00 FPS";
					}
				} else if(Video.Checked) {
					if(button1.Text == "Play") {
						button1.Text = "Stop";

						frame = 0;

						capture = new Capture(directory);

						total_frames = capture.GetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_COUNT);

						Application.Idle += GPUFrameProcessing;
						keep = true;

					} else if(button1.Text == "Stop") {
						button1.Text = "Play";
						capture.Stop();

						Application.Idle -= GPUFrameProcessing;
						capture.Dispose();
						ImageBox.Image = null;
						RedDetection.Image = null;
						BlueDetection.Image = null;
						keep = false;

						textBox1.Text = "0.00 ms" + " / " + "0.00 FPS";
					}
				}
			} else {
				if(Webcam.Checked) {
					if(button1.Text == "Start") {
						button1.Text = "Stop";

						capture = new Capture();
						Application.Idle += CPUFrameProcessing;
						keep = true;

					} else if(button1.Text == "Stop") {
						button1.Text = "Start";

						Application.Idle -= CPUFrameProcessing;
						capture.Dispose();
						ImageBox.Image = null;
						RedDetection.Image = null;
						BlueDetection.Image = null;
						keep = false;

						textBox1.Text = "0.00 ms" + " / " + "0.00 FPS";
					}
				} else if(Video.Checked) {
					if(button1.Text == "Play") {
						button1.Text = "Stop";

						frame = 0;

						capture = new Capture(directory);
						total_frames = capture.GetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_COUNT);

						Application.Idle += CPUFrameProcessing;
						keep = true;

					} else if(button1.Text == "Stop") {
						button1.Text = "Play";
						capture.Stop();

						Application.Idle -= CPUFrameProcessing;
						capture.Dispose();
						ImageBox.Image = null;
						RedDetection.Image = null;
						BlueDetection.Image = null;
						keep = false;

						textBox1.Text = "0.00 ms" + " / " + "0.00 FPS";
					}
				}
			}

		}

		private void Webcam_CheckedChanged(object sender, EventArgs e) {
			button1.Text = "Start";
		}

		private void Video_CheckedChanged(object sender, EventArgs e) {
			button1.Text = "Play";
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e) {
			if(!checkBox1.Checked) {
				GPU = false;
			} else {
				GPU = true;
			}
		}

	}
}
