using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.UI;
using Emgu.CV.Structure;

namespace SS_OpenCV {
	public partial class MainForm :Form {
		Image<Bgr, Byte> img = null; // imagem corrente
		Image<Bgr, Byte> imgUndo = null; // imagem backup - UNDO
		string title_bak = "";

		int x_image = -1;
		int y_image = -1;
		bool wait = true;

		public static int blockSize;

		//string directory = @".\Base Dados\";
		string blueSquareDirectory = @".\Base Dados\Azul\Quadrado";
		string blueCircleDirectory = @".\Base Dados\Azul\Circulo";
		string redTriangleDirectory = @".\Base Dados\Vermelho\Triangulo";
		string redCircleDirectory = @".\Base Dados\Vermelho\Circulo";
		//string temp;

		//List<Image<Bgr, byte>> DataBase = new List<Image<Bgr, byte>>();
		//List<string> NamesDataBase = new List<string>();

		public static List<Image<Bgr, byte>> blueSquareDataBase = new List<Image<Bgr, byte>>();
		//List<string> NamesDataBase = new List<string>();
		public static List<Image<Bgr, byte>> blueCircleDataBase = new List<Image<Bgr, byte>>();
		//List<string> NamesDataBase = new List<string>();
		public static List<Image<Bgr, byte>> redTriangleDataBase = new List<Image<Bgr, byte>>();
		//List<string> NamesDataBase = new List<string>();
		public static List<Image<Bgr, byte>> redCircleDataBase = new List<Image<Bgr, byte>>();
		//List<string> NamesDataBase = new List<string>();

		public static List<List<Image<Bgr, byte>>> DataBase = new List<List<Image<Bgr, byte>>>();

		public MainForm() {
			InitializeComponent();
			title_bak = Text;
			//this.WindowState = FormWindowState.Maximized;
			this.Width = 1000;
			this.Height = 600;

			img = new Image<Bgr, byte>(@"D:\Documentos\FCT\4o Ano\2o Semestre\TAPDI\P1\Imagens\IMG_7254.jpg");
			Text = title_bak + " [" + openFileDialog1.FileName.Substring(openFileDialog1.FileName.LastIndexOf("\\") + 1) + "]";
			imgUndo = img.Copy();
			ImageViewer.Image = img;
			ImageViewer.Refresh();

			blockSize = 32;

			//OpenCL_Class.GetInfo();
			//OpenCL_Class.GetDeviceProperties(0);
			//OpenCL_Class.GetDeviceProperties(1);

			ImageViewer.SizeMode = PictureBoxSizeMode.Zoom;
			
			//foreach(string image in Directory.GetFiles(directory, "*.png")) {
			//	DataBase.Add(new Image<Bgr, byte>(image));
			//	temp = image.Substring(image.LastIndexOf("\\") + 1);
			//	temp = temp.Substring(0, temp.Length - 4);

			//	NamesDataBase.Add(temp);
			//}

			foreach(string image in Directory.GetFiles(blueSquareDirectory, "*.png")) {
				blueSquareDataBase.Add(new Image<Bgr, byte>(image));
				//temp = image.Substring(image.LastIndexOf("\\") + 1);
				//temp = temp.Substring(0, temp.Length - 4);
				//NamesDataBase.Add(temp);
			}

			foreach(string image in Directory.GetFiles(blueCircleDirectory, "*.png")) {
				blueCircleDataBase.Add(new Image<Bgr, byte>(image));
				//temp = image.Substring(image.LastIndexOf("\\") + 1);
				//temp = temp.Substring(0, temp.Length - 4);
				//NamesDataBase.Add(temp);
			}

			foreach(string image in Directory.GetFiles(redTriangleDirectory, "*.png")) {
				redTriangleDataBase.Add(new Image<Bgr, byte>(image));
				//temp = image.Substring(image.LastIndexOf("\\") + 1);
				//temp = temp.Substring(0, temp.Length - 4);
				//NamesDataBase.Add(temp);
			}

			foreach(string image in Directory.GetFiles(redCircleDirectory, "*.png")) {
				redCircleDataBase.Add(new Image<Bgr, byte>(image));
				//temp = image.Substring(image.LastIndexOf("\\") + 1);
				//temp = temp.Substring(0, temp.Length - 4);
				//NamesDataBase.Add(temp);
			}

			OpenCL_Class.Setup();

			DataBase.Add(redCircleDataBase);
			DataBase.Add(redTriangleDataBase);
			DataBase.Add(blueCircleDataBase);
			DataBase.Add(blueSquareDataBase);
		}

		/// <summary>
		/// Abrir uma nova imagem
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void openToolStripMenuItem_Click(object sender, EventArgs e) {
			if(openFileDialog1.ShowDialog() == DialogResult.OK) {
				img = new Image<Bgr, byte>(openFileDialog1.FileName);
				Text = title_bak + " [" +
						openFileDialog1.FileName.Substring(openFileDialog1.FileName.LastIndexOf("\\") + 1) +
						"]";
				imgUndo = img.Copy();
				ImageViewer.Image = img;
				ImageViewer.Refresh();
			}
		}

		/// <summary>
		/// Guardar a imagem com novo nome
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
			if(saveFileDialog1.ShowDialog() == DialogResult.OK) {
				ImageViewer.Image.Save(saveFileDialog1.FileName);
			}
		}

		/// <summary>
		/// Fecha a aplicação
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
			Close();
		}

		/// <summary>
		/// repoe a ultima copia da imagem
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void undoToolStripMenuItem_Click(object sender, EventArgs e) {
			if(imgUndo == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;
			Cursor = Cursors.WaitCursor;
			img = imgUndo.Copy();
			ImageViewer.Image = img;
			ImageViewer.Refresh();
			Cursor = Cursors.Default;
		}

		/// <summary>
		/// Altera o modo de vizualização
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void autoZoomToolStripMenuItem_Click(object sender, EventArgs e) {
			// zoom
			if(autoZoomToolStripMenuItem.Checked) {
				ImageViewer.SizeMode = PictureBoxSizeMode.Zoom;
				ImageViewer.Dock = DockStyle.Fill;
			} else {// com scroll bars
				ImageViewer.Dock = DockStyle.None;
				ImageViewer.SizeMode = PictureBoxSizeMode.AutoSize;
			}
		}

		/// <summary>
		/// Mostra a janela Autores
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void autoresToolStripMenuItem_Click(object sender, EventArgs e) {
			AuthorsForm form = new AuthorsForm();
			form.ShowDialog();
		}

		/// <summary>
		/// Efectua o negativo da imagem
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void emguDirectivesToolStripMenuItem_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;
			Cursor = Cursors.WaitCursor; // cursor relogio

			//copy Undo Image
			imgUndo = img.Copy();

			ImageClass.Negative(img);

			ImageViewer.Refresh(); // atualiza imagem no ecrã

			Cursor = Cursors.Default; // cursor normal
		}

		/// <summary>
		/// Efectua o negativo da imagem
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void directAccessToolStripMenuItem_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;
			Cursor = Cursors.WaitCursor; // cursor relogio

			//copy Undo Image
			imgUndo = img.Copy();

			ImageClass.NegativeFast(img);

			ImageViewer.Refresh(); // atualiza imagem no ecrã

			Cursor = Cursors.Default; // cursor normal
		}

		/// <summary>
		/// Converte a imagem para tons de cinzento
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void grayScaleToolStripMenuItem_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;
			Cursor = Cursors.WaitCursor; // cursor relogio

			//copy Undo Image
			imgUndo = img.Copy();

			ImageClass.ConvertToGray(img);

			ImageViewer.Refresh(); // atualiza imagem no ecrã

			Cursor = Cursors.Default; // cursor normal
		}

		/// <summary>
		/// Converte a imagem para preto e branco
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void binarizationToolStripMenuItem_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;

			InputBox formX = new InputBox("Threshold");
			formX.ShowDialog();

			if(formX.ValueTextBox.Text != "") {
				int threshold = Convert.ToInt32(formX.ValueTextBox.Text);

				//copy Undo Image
				imgUndo = img.Copy();

				Cursor = Cursors.WaitCursor; // cursor relogio

				ImageClass.Binarization(img, threshold);

				ImageViewer.Refresh(); // atualiza imagem no ecrã

				Cursor = Cursors.Default; // cursor normal
			}
		}

		/// <summary>
		/// Converte a imagem para preto e branco pelo utilizando o método de Otsu
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void otsuBinarizationToolStripMenuItem_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;

			//copy Undo Image
			imgUndo = img.Copy();

			Cursor = Cursors.WaitCursor; // cursor relogio

			ImageClass.Otsu(img);

			ImageViewer.Refresh(); // atualiza imagem no ecrã

			Cursor = Cursors.Default; // cursor normal
		}

		/// <summary>
		/// Converte a imagem para tons de cinzento na componente vermelha
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void redToolStripMenuItem_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;
			Cursor = Cursors.WaitCursor; // cursor relogio

			//copy Undo Image
			imgUndo = img.Copy();

			ImageClass.ColorFilter(img, "red");

			ImageViewer.Refresh(); // atualiza imagem no ecrã

			Cursor = Cursors.Default; // cursor normal
		}

		/// <summary>
		/// Converte a imagem para tons de cinzento na componente verde
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void greenToolStripMenuItem_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;
			Cursor = Cursors.WaitCursor; // cursor relogio

			//copy Undo Image
			imgUndo = img.Copy();

			ImageClass.ColorFilter(img, "green");

			ImageViewer.Refresh(); // atualiza imagem no ecrã

			Cursor = Cursors.Default; // cursor normal
		}

		/// <summary>
		/// Converte a imagem para tons de cinzento na componente azul
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void blueToolStripMenuItem_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;
			Cursor = Cursors.WaitCursor; // cursor relogio

			//copy Undo Image
			imgUndo = img.Copy();

			ImageClass.ColorFilter(img, "blue");

			ImageViewer.Refresh(); // atualiza imagem no ecrã

			Cursor = Cursors.Default; // cursor normal
		}

		/// <summary>
		/// Realiza uma translação na imagem
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void translationToolStripMenuItem1_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;

			//copy Undo Image
			imgUndo = img.Copy();

			InputBox formX = new InputBox("Deslocamento em X?");
			formX.ShowDialog();

			if(formX.ValueTextBox.Text != "") {

				//int Dx = Convert.ToInt32(formX.ValueTextBox.Text);
				double Dx = Convert.ToDouble(formX.ValueTextBox.Text);

				InputBox formY = new InputBox("Deslocamento em Y?");
				formY.ShowDialog();

				//int Dy = Convert.ToInt32(formY.ValueTextBox.Text);
				double Dy = Convert.ToDouble(formY.ValueTextBox.Text);

				Cursor = Cursors.WaitCursor; // cursor relogio

				ImageClass.Translation(img, Dx, Dy);

				ImageViewer.Refresh(); // atualiza imagem no ecrã

				Cursor = Cursors.Default; // cursor normal
			}
		}

		/// <summary>
		/// Realiza uma rotação na imagem
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void rotationToolStripMenuItem1_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;

			//copy Undo Image
			imgUndo = img.Copy();

			InputBox formX = new InputBox("Rotação em Graus");
			formX.ShowDialog();

			if(formX.ValueTextBox.Text != "") {
				double Ang = Convert.ToDouble(formX.ValueTextBox.Text);

				Cursor = Cursors.WaitCursor; // cursor relogio

				ImageClass.Rotation(img, Ang);

				ImageViewer.Refresh(); // atualiza imagem no ecrã

				Cursor = Cursors.Default; // cursor normal
			}
		}

		/// <summary>
		/// Realiza uma ampliação na imagem
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void zoomToolStripMenuItem1_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;

			//copy Undo Image
			imgUndo = img.Copy();

			InputBox formX = new InputBox("Zoom");
			formX.ShowDialog();
			if(formX.ValueTextBox.Text != "") {
				double zoom = Convert.ToDouble(formX.ValueTextBox.Text);

				wait = true;
				while(wait) {
					Application.DoEvents();
				}

				Cursor = Cursors.WaitCursor; // cursor relogio

				ImageClass.Zoom(img, zoom, x_image, y_image);

				ImageViewer.Refresh(); // atualiza imagem no ecrã

				Cursor = Cursors.Default; // cursor normal
			}
		}

		/// <summary>
		/// Captura a posição do rato na altura do click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ImageViewer_MouseClick(object sender, MouseEventArgs e) {
			x_image = e.X;
			y_image = e.Y;
			wait = false;
		}

		/// <summary>
		/// Aplica um filtro de média na imagem
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void averageMethodAToolStripMenuItem_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;

			//copy Undo Image
			imgUndo = img.Copy();

			InputBox formX = new InputBox("Tamanho do Filtro");
			formX.ShowDialog();

			if(formX.ValueTextBox.Text != "") {
				int Size = Convert.ToInt32(formX.ValueTextBox.Text);

				Cursor = Cursors.WaitCursor; // cursor relogio

				ImageClass.FilterA(img, Size);

				ImageViewer.Refresh(); // atualiza imagem no ecrã

				Cursor = Cursors.Default; // cursor normal
			}
		}

		/// <summary>
		/// Aplica um filtro não-uniforme na imagem
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void matrix3x3ToolStripMenuItem_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;

			//copy Undo Image
			imgUndo = img.Copy();

			MatrixInputBox formX = new MatrixInputBox();
			formX.ShowDialog();

			if(formX.numericUpDown10.Text != "0") {
				int[] v_coeficientes = new int[10];

				v_coeficientes[0] = Convert.ToInt32(formX.numericUpDown1.Text);
				v_coeficientes[1] = Convert.ToInt32(formX.numericUpDown2.Text);
				v_coeficientes[2] = Convert.ToInt32(formX.numericUpDown3.Text);
				v_coeficientes[3] = Convert.ToInt32(formX.numericUpDown4.Text);
				v_coeficientes[4] = Convert.ToInt32(formX.numericUpDown5.Text);
				v_coeficientes[5] = Convert.ToInt32(formX.numericUpDown6.Text);
				v_coeficientes[6] = Convert.ToInt32(formX.numericUpDown7.Text);
				v_coeficientes[7] = Convert.ToInt32(formX.numericUpDown8.Text);
				v_coeficientes[8] = Convert.ToInt32(formX.numericUpDown9.Text);
				v_coeficientes[9] = Convert.ToInt32(formX.numericUpDown10.Text);

				Cursor = Cursors.WaitCursor; // cursor relogio

				ImageClass.FilterMatrix(img, v_coeficientes);

				ImageViewer.Refresh(); // atualiza imagem no ecrã

				Cursor = Cursors.Default; // cursor normal
			}
		}

		/// <summary>
		/// Aplica um filtro de Sobel de 3x3 na imagem
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void x3ToolStripMenuItem_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;

			//copy Undo Image
			imgUndo = img.Copy();

			int[] v_coeficientes1 = new int[9];
			int[] v_coeficientes2 = new int[9];

			v_coeficientes1[0] = 1;
			v_coeficientes1[1] = 0;
			v_coeficientes1[2] = -1;
			v_coeficientes1[3] = 2;
			v_coeficientes1[4] = 0;
			v_coeficientes1[5] = -2;
			v_coeficientes1[6] = 1;
			v_coeficientes1[7] = 0;
			v_coeficientes1[8] = -1;

			v_coeficientes2[0] = -1;
			v_coeficientes2[1] = -2;
			v_coeficientes2[2] = -1;
			v_coeficientes2[3] = 0;
			v_coeficientes2[4] = 0;
			v_coeficientes2[5] = 0;
			v_coeficientes2[6] = 1;
			v_coeficientes2[7] = 2;
			v_coeficientes2[8] = 1;

			Cursor = Cursors.WaitCursor; // cursor relogio

			ImageClass.Sobel3x3(img, v_coeficientes1, v_coeficientes2);

			ImageViewer.Refresh(); // atualiza imagem no ecrã

			Cursor = Cursors.Default; // cursor normal
		}

		/// <summary>
		/// Aplica um filtro de Sobel de 5x5 na imagem
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void x5ToolStripMenuItem_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;

			//copy Undo Image
			imgUndo = img.Copy();

			int[] v_coeficientes1 = new int[25];
			int[] v_coeficientes2 = new int[25];

			v_coeficientes1[0] = 1;
			v_coeficientes1[1] = 2;
			v_coeficientes1[2] = 0;
			v_coeficientes1[3] = -2;
			v_coeficientes1[4] = -1;
			v_coeficientes1[5] = 4;
			v_coeficientes1[6] = 8;
			v_coeficientes1[7] = 0;
			v_coeficientes1[8] = -8;
			v_coeficientes1[9] = -4;
			v_coeficientes1[10] = 6;
			v_coeficientes1[11] = 12;
			v_coeficientes1[12] = 0;
			v_coeficientes1[13] = -12;
			v_coeficientes1[14] = -6;
			v_coeficientes1[15] = 4;
			v_coeficientes1[16] = 8;
			v_coeficientes1[17] = 0;
			v_coeficientes1[18] = -8;
			v_coeficientes1[19] = -4;
			v_coeficientes1[20] = 1;
			v_coeficientes1[21] = 2;
			v_coeficientes1[22] = 0;
			v_coeficientes1[23] = -2;
			v_coeficientes1[24] = -1;

			v_coeficientes2[0] = -1;
			v_coeficientes2[1] = -4;
			v_coeficientes2[2] = -6;
			v_coeficientes2[3] = -4;
			v_coeficientes2[4] = -1;
			v_coeficientes2[5] = -2;
			v_coeficientes2[6] = -8;
			v_coeficientes2[7] = -12;
			v_coeficientes2[8] = -8;
			v_coeficientes2[9] = -2;
			v_coeficientes2[10] = 0;
			v_coeficientes2[11] = 0;
			v_coeficientes2[12] = 0;
			v_coeficientes2[13] = 0;
			v_coeficientes2[14] = 0;
			v_coeficientes2[15] = 2;
			v_coeficientes2[16] = 8;
			v_coeficientes2[17] = 12;
			v_coeficientes2[18] = 8;
			v_coeficientes2[19] = 2;
			v_coeficientes2[20] = 1;
			v_coeficientes2[21] = 4;
			v_coeficientes2[22] = 6;
			v_coeficientes2[23] = 4;
			v_coeficientes2[24] = 1;

			Cursor = Cursors.WaitCursor; // cursor relogio

			ImageClass.Sobel5x5(img, v_coeficientes1, v_coeficientes2);

			ImageViewer.Refresh(); // atualiza imagem no ecrã

			Cursor = Cursors.Default; // cursor normal
		}

		/// <summary>
		/// Aplica um filtro de diferenciação na imagem
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void differentiationToolStripMenuItem_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;

			//copy Undo Image
			imgUndo = img.Copy();

			Cursor = Cursors.WaitCursor; // cursor relogio

			ImageClass.Differentiation(img);

			ImageViewer.Refresh(); // atualiza imagem no ecrã

			Cursor = Cursors.Default; // cursor normal
		}

		/// <summary>
		/// Aplica um filtro de Roberts na imagem
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void robertsToolStripMenuItem_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;

			//copy Undo Image
			imgUndo = img.Copy();

			Cursor = Cursors.WaitCursor; // cursor relogio

			ImageClass.Roberts(img);

			ImageViewer.Refresh(); // atualiza imagem no ecrã

			Cursor = Cursors.Default; // cursor normal
		}

		/// <summary>
		/// Aplica um filtro de mediana na imagem
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void medianToolStripMenuItem_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;

			//copy Undo Image
			imgUndo = img.Copy();

			Cursor = Cursors.WaitCursor; // cursor relogio

			ImageClass.Median(img);

			ImageViewer.Refresh(); // atualiza imagem no ecrã

			Cursor = Cursors.Default; // cursor normal
		}

		/// <summary>
		/// Realiza o histrograma da imagem
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void histogramToolStripMenuItem_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;

			Cursor = Cursors.WaitCursor; // cursor relogio

			List<int[]> arrays = new List<int[]>();
			arrays = ImageClass.Histogram(img);

			int[] arrayGray = new int[256];
			int[] arrayB = new int[256];
			int[] arrayG = new int[256];
			int[] arrayR = new int[256];

			arrayGray = arrays[0];
			arrayB = arrays[1];
			arrayG = arrays[2];
			arrayR = arrays[3];

			Histogram formX = new Histogram(arrayGray, arrayB, arrayG, arrayR, arrayGray.Length);
			formX.ShowDialog();

			ImageViewer.Refresh(); // atualiza imagem no ecrã

			Cursor = Cursors.Default; // cursor normal
		}

		/// <summary>
		/// Realiza a detecção do sinal vermelho
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void redSignToolStripMenuItem_Click(object sender, EventArgs e) {
			//if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
			//	return;

			//Cursor = Cursors.WaitCursor; // cursor relogio

			//ImageClass.RedSignDetection(img, this, DataBase, NamesDataBase);

			//ImageViewer.Refresh(); // atualiza imagem no ecrã

			//Cursor = Cursors.Default; // cursor normal
		}

		/// <summary>
		/// Realiza a detecção do sinal azul
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void blueSignToolStripMenuItem_Click(object sender, EventArgs e) {
			//if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
			//	return;

			//Cursor = Cursors.WaitCursor; // cursor relogio

			//ImageClass.BlueSignDetection(img, this, DataBase, NamesDataBase);

			//ImageViewer.Refresh(); // atualiza imagem no ecrã

			//Cursor = Cursors.Default; // cursor normal
		}

		/// <summary>
		/// Realiza a tabela de codificação
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void tableToolStripMenuItem_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;

			Cursor = Cursors.WaitCursor; // cursor relogio

			int[] hist = new int[256];
			List<int[]> arrays = new List<int[]>();
			arrays = ImageClass.Histogram(img);
			hist = arrays[0];

			CompressionTableForm X = new CompressionTableForm(hist, img);
			X.ShowDialog();

			Cursor = Cursors.Default; // cursor normal
		}

		/// <summary>
		/// Realiza a entropia da imagem
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void entropyToolStripMenuItem_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;

			Cursor = Cursors.WaitCursor; // cursor relogio

			double[] entropy = new double[4];
			entropy = ImageClass.Entropy(img);

			Entropy_Results X = new Entropy_Results();

			X.Gray.Text = entropy[0].ToString("n8");
			X.Blue.Text = entropy[1].ToString("n8");
			X.Green.Text = entropy[2].ToString("n8");
			X.Red.Text = entropy[3].ToString("n8");

			X.ShowDialog();

			Cursor = Cursors.Default; // cursor normal
		}

		private void quantificationMatrixToolStripMenuItem_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;

			Cursor = Cursors.WaitCursor; // cursor relogio

			InputBox formX = new InputBox("Compression Factor");
			formX.ShowDialog();

			int compFactor = Convert.ToInt32(formX.ValueTextBox.Text);

			Image<Gray, float> img_quant = ImageClass.GetQuantificationMatrix(true, compFactor);

			TableForm X = new TableForm();
			TableForm.ShowTableStatic(img_quant, "Luminance");

			img_quant = ImageClass.GetQuantificationMatrix(false, compFactor);

			TableForm Y = new TableForm();
			TableForm.ShowTableStatic(img_quant, "Chrominance");

			Cursor = Cursors.Default; // cursor normal
		}

		private void toJPEGToolStripMenuItem_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;

			Cursor = Cursors.WaitCursor; // cursor relogio

			InputBox formX = new InputBox("Compression Factor");
			formX.ShowDialog();

			int _CompressionFactor = Convert.ToInt32(formX.ValueTextBox.Text);

			Image<Bgr, byte> _imgRGB = ImageClass.CompressionToJPEG(img, _CompressionFactor);

			ShowIMG.ShowIMGStatic(img, _imgRGB);

			Cursor = Cursors.Default; // cursor normal
		}

		private void houghToolStripMenuItem_Click(object sender, EventArgs e) {

		}

		private void rAWDataToolStripMenuItem_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;

			Cursor = Cursors.WaitCursor; // cursor relogio

			Image<Gray, Byte> imgGray = img.Convert<Gray, byte>();

			Image<Gray, float> _img = ImageClass.HoughTransform(imgGray, 1.0f, 0, 180);

			ShowIMG.ShowIMGStatic(img, _img);

			Cursor = Cursors.Default; // cursor normal
		}

		private void horizontalLinesToolStripMenuItem_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;

			Cursor = Cursors.WaitCursor; // cursor relogio

			Image<Bgr, byte> _imgC = img.Copy();

			int[] coeff = { -1, -2, -1, 0, 0, 0, 1, 2, 1, 1 };

			ImageClass.FilterMatrix(_imgC, coeff);
			ImageClass.Otsu(_imgC);

			Image<Gray, byte> imgGray = _imgC.Convert<Gray, byte>();

			Image<Gray, float> _img = ImageClass.HoughTransform(imgGray, 1.0f, 0, 180.0f); //PODE FAZER-SE UMA JANELA A PEDIR OS PARAMETROS
																						   //PARA ENCONTRAR LINHAS VERTICAIS OU HORIZONTAIS, FAZER O SOBEL VERTICAL OU HORIZONTAL (TEMOS DE FAZER AS FUNCÇOES ADICIONAIS)

			InputBox formX = new InputBox("Threshold");
			formX.ShowDialog();

			int _thres = Convert.ToInt32(formX.ValueTextBox.Text);

			_imgC = ImageClass.ShowHoughLines(_imgC, img, _thres);

			ShowIMG.ShowIMGStatic(img, _imgC);

			Cursor = Cursors.Default; // cursor normal
		}

		private void verticalLinesToolStripMenuItem_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;

			Cursor = Cursors.WaitCursor; // cursor relogio

			Image<Bgr, byte> _imgC = img.Copy();

			int[] coeff = { 1, 0, -1, 2, 0, -2, 1, 0, -1, 1 };

			ImageClass.FilterMatrix(_imgC, coeff);
			ImageClass.Otsu(_imgC);

			Image<Gray, byte> imgGray = _imgC.Convert<Gray, byte>();

			Image<Gray, float> _img = ImageClass.HoughTransform(imgGray, 0.5f, 0, 180.0f); //PODE FAZER-SE UMA JANELA A PEDIR OS PARAMETROS
																						   //PARA ENCONTRAR LINHAS VERTICAIS OU HORIZONTAIS, FAZER O SOBEL VERTICAL OU HORIZONTAL (TEMOS DE FAZER AS FUNCÇOES ADICIONAIS)

			InputBox formX = new InputBox("Threshold");
			formX.ShowDialog();

			int _thres = Convert.ToInt32(formX.ValueTextBox.Text);

			_imgC = ImageClass.ShowHoughLines(_imgC, img, _thres);

			ShowIMG.ShowIMGStatic(img, _imgC);

			Cursor = Cursors.Default; // cursor normal
		}

		private void circlesToolStripMenuItem_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;

			Cursor = Cursors.WaitCursor; // cursor relogio

			int[] coeff1 = { 1, 0, -1, 2, 0, -2, 1, 0, -1 };
			int[] coeff2 = { -1, -2, -1, 0, 0, 0, 1, 2, 1 };

			Image<Bgr, byte> _imgC = img.Copy();
			ImageClass.Sobel3x3(_imgC, coeff1, coeff2);
			ImageClass.Otsu(_imgC);

			Image<Gray, byte> imgGray = _imgC.Convert<Gray, byte>();

			Image<Gray, float> _img = ImageClass.HoughTransform(imgGray, 1f, 0, 180.0f); //PODE FAZER-SE UMA JANELA A PEDIR OS PARAMETROS
																						 //PARA ENCONTRAR LINHAS VERTICAIS OU HORIZONTAIS, FAZER O SOBEL VERTICAL OU HORIZONTAL (TEMOS DE FAZER AS FUNCÇOES ADICIONAIS)

			_imgC = ImageClass.ShowHoughCircles(_imgC, img);

			ShowIMG.ShowIMGStatic(img, _imgC);

			Cursor = Cursors.Default; // cursor normal
		}

		private void array2ToolStripMenuItem_Click(object sender, EventArgs e) {
			Cursor = Cursors.WaitCursor; // cursor relogio

			float[] arr = new float[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

			OpenCL_Class.Square(arr);

			Cursor = Cursors.Default; // cursor normal
		}

		private void multiplyToolStripMenuItem_Click(object sender, EventArgs e) {
			Cursor = Cursors.WaitCursor; // cursor relogio
			float[] arr = new float[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

			InputBox formX = new InputBox("Multiplier");
			formX.ShowDialog();

			int _mult = Convert.ToInt32(formX.ValueTextBox.Text);

			OpenCL_Class.Multiply(arr, _mult);

			Cursor = Cursors.Default; // cursor normal
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
			Cursor = Cursors.WaitCursor; // cursor relogio

			OpenCL_Class.Release();

			Cursor = Cursors.Default; // cursor normal
		}

		private void coalescedToolStripMenuItem_Click_1(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;
			Cursor = Cursors.WaitCursor; // cursor relogio

			//copy Undo Image
			imgUndo = img.Copy();

			OpenCL_Class.NegativeDirectMemoryAccessCoalesced(img);

			ImageViewer.Refresh(); // atualiza imagem no ecrã

			Cursor = Cursors.Default; // cursor normal
		}

		private void uncoalescedToolStripMenuItem_Click_1(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;
			Cursor = Cursors.WaitCursor; // cursor relogio

			//copy Undo Image
			imgUndo = img.Copy();

			OpenCL_Class.NegativeDirectMemoryAccessUncoalesced(img);

			ImageViewer.Refresh(); // atualiza imagem no ecrã

			Cursor = Cursors.Default; // cursor normal
		}

		private void dSamplerToolStripMenuItem_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;
			Cursor = Cursors.WaitCursor; // cursor relogio

			//copy Undo Image
			imgUndo = img.Copy();

			OpenCL_Class.Negative2D(img);

			ImageViewer.Refresh(); // atualiza imagem no ecrã

			Cursor = Cursors.Default; // cursor normal
		}

		private void detectFacesToolStripMenuItem_Click(object sender, EventArgs e) {
			ImageClass.DetectFaces();
		}

		private void detectEyesToolStripMenuItem_Click(object sender, EventArgs e) {
			ImageClass.DetectEyes();
		}

		private void fFTToolStripMenuItem_Click(object sender, EventArgs e) {
			ImageClass.VideoSpectrumVisualization();
		}

		private void fFTToolStripMenuItem1_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;
			Cursor = Cursors.WaitCursor; // cursor relogio

			//copy Undo Image
			imgUndo = img.Copy();

			Image<Gray, float> amplitude = null;
			Image<Gray, float> phase = null;

			ImageClass.ImageSpectrumVisualization(img, out amplitude, out phase);

			ShowIMG.ShowIMGStatic(amplitude, phase);

			Cursor = Cursors.Default; // cursor normal
		}

		private void highPassToolStripMenuItem_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;
			Cursor = Cursors.WaitCursor; // cursor relogio

			//copy Undo Image
			imgUndo = img.Copy();

			InputBox formX = new InputBox("Filter size (pixels)");
			formX.ShowDialog();

			int size = Convert.ToInt32(formX.ValueTextBox.Text);

			Image<Gray, byte> img_out;

			ImageClass.IdealHighPassFilter(img, size, out img_out);

			ImageViewer.Image = img_out;

			Cursor = Cursors.Default; // cursor normal
		}

		private void lowPassToolStripMenuItem_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;
			Cursor = Cursors.WaitCursor; // cursor relogio

			//copy Undo Image
			imgUndo = img.Copy();

			InputBox formX = new InputBox("Filter size (pixels)");
			formX.ShowDialog();

			int size = Convert.ToInt32(formX.ValueTextBox.Text);

			Image<Gray, byte> img_out;

			ImageClass.IdealLowPassFilter(img, size, out img_out);

			ImageViewer.Image = img_out;

			Cursor = Cursors.Default; // cursor normal
		}

		private void gaussianLowPassToolStripMenuItem_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;
			Cursor = Cursors.WaitCursor; // cursor relogio

			//copy Undo Image
			imgUndo = img.Copy();

			InputBox formX = new InputBox("Filter size (pixels)");
			formX.ShowDialog();

			int size = Convert.ToInt32(formX.ValueTextBox.Text);

			Image<Gray, byte> img_out;

			ImageClass.GaussianLowPassFilter(img, size, out img_out);

			ImageViewer.Image = img_out;

			Cursor = Cursors.Default; // cursor normal
		}

		private void meanGaussianToolStripMenuItem_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;
			Cursor = Cursors.WaitCursor; // cursor relogio

			//copy Undo Image
			imgUndo = img.Copy();

			InputBox formX = new InputBox("Filter size (pixels)");
			formX.ShowDialog();

			int _size = Convert.ToInt32(formX.ValueTextBox.Text);

			Image<Bgr, byte> _img = WienerFunc.FilterConv(img, false, _size);

			ImageViewer.Image = _img;

			Cursor = Cursors.Default; // cursor normal
		}

		private void motionBlurToolStripMenuItem_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;
			Cursor = Cursors.WaitCursor; // cursor relogio

			//copy Undo Image
			imgUndo = img.Copy();

			img = WienerFunc.FilterConv(img, true, 0);

			ImageViewer.Image = img;

			Cursor = Cursors.Default; // cursor normal
		}

		private void wienerToolStripMenuItem_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;
			Cursor = Cursors.WaitCursor; // cursor relogio

			//copy Undo Image
			imgUndo = img.Copy();

			InputBox formX = new InputBox("Filter size (pixels)");
			formX.ShowDialog();
			formX.Dispose();
			int _size = Convert.ToInt32(formX.ValueTextBox.Text);

			formX = new InputBox("SNR");
			formX.ShowDialog();
			formX.Dispose();
			float _snr = Convert.ToSingle(formX.ValueTextBox.Text);

			formX = new InputBox("Motion?");
			formX.ShowDialog();
			
			char _motion = Convert.ToChar(formX.ValueTextBox.Text);
			formX.Dispose();

			if(_motion == 'f') {
				img = WienerFunc.WienerFilter(img, _snr, _size, false);
			} else {
				img = WienerFunc.WienerFilter(img, _snr, _size, true);
			}

			ImageViewer.Image = img;

			Cursor = Cursors.Default; // cursor normal
		}

		private void addNoiseToolStripMenuItem_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;
			Cursor = Cursors.WaitCursor; // cursor relogio

			//copy Undo Image
			imgUndo = img.Copy();

			img = WienerFunc.AddNoise(img, 10);

			ImageViewer.Image = img;

			Cursor = Cursors.Default; // cursor normal
		}

		private void resizeToolStripMenuItem_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;
			Cursor = Cursors.WaitCursor; // cursor relogio

			//copy Undo Image
			imgUndo = img.Copy();

			img = ImageClass.mResize(img, 100, 100);

			ImageViewer.Image = img;

			Cursor = Cursors.Default; // cursor normal
		}

		private void watershedToolStripMenuItem_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;
			Cursor = Cursors.WaitCursor; // cursor relogio

			//copy Undo Image
			imgUndo = img.Copy();

			Image<Gray, int> _img = ImageClass.GetWatershedByImmersion(img);

			ImageViewer.Image = _img;

			Cursor = Cursors.Default; // cursor normal
		}

		private void erodeToolStripMenuItem_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;
			Cursor = Cursors.WaitCursor; // cursor relogio

			//copy Undo Image
			imgUndo = img.Copy();

			img = OpenCL_Class.Erode(img, 5);

			ImageViewer.Image = img;

			Cursor = Cursors.Default; // cursor normal
		}

		private void dilateToolStripMenuItem_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;
			Cursor = Cursors.WaitCursor; // cursor relogio

			//copy Undo Image
			imgUndo = img.Copy();

			img = OpenCL_Class.Dilate(img, 5);

			ImageViewer.Image = img;

			Cursor = Cursors.Default; // cursor normal
		}

		private void templateMatchingToolStripMenuItem_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;
			Cursor = Cursors.WaitCursor; // cursor relogio

			//copy Undo Image
			imgUndo = img.Copy();

			Image<Bgr, byte> _img = new Image<Bgr, byte>(@"D:\Documentos\FCT\4o Ano\2o Semestre\TAPDI\P1\SS_TAPDI_OpenCV\SS_OpenCV\bin\Debug\Base Dados\Vermelho\Circulo\Proibição de exceder a velocidade máxima de 60 Km 3.png");
			Image<Bgr, byte> _tmp = new Image<Bgr, byte>(@"D:\Documentos\FCT\4o Ano\2o Semestre\TAPDI\P1\SS_TAPDI_OpenCV\SS_OpenCV\bin\Debug\Base Dados\Vermelho\Circulo\Proibição de exceder a velocidade máxima de 60 Km 3.png");

            //Image<Bgr, byte> _img = new Image<Bgr, byte>(@"C:\Users\Dimo\Documents\Universidade\TAPDI\4 Questionario\imagem_antonio.png");
            //Image<Bgr, byte> _tmp = new Image<Bgr, byte>(@"C:\Users\Dimo\Documents\Universidade\TAPDI\4 Questionario\mascara_antonio.png");

            //double value1 = ImageClass.MatchTemplate(_img.Convert<Gray, byte>(), _tmp.Convert<Gray, byte>());

            Image<Gray, float> result = _img.Convert<Gray, byte>().MatchTemplate(_tmp.Convert<Gray, byte>(), Emgu.CV.CvEnum.TM_TYPE.CV_TM_CCOEFF_NORMED);
			double value2 = result[0, 0].Intensity;

			Entropy_Results form = new Entropy_Results();
			//form.Red.Text = value1.ToString();
			form.Gray.Text = value2.ToString();
			form.ShowDialog();

			ImageViewer.Image = img;

			Cursor = Cursors.Default; // cursor normal
		}

		private void signDetectionToolStripMenuItem1_Click(object sender, EventArgs e) {
			VideoSignIdentification viewer = new VideoSignIdentification();
			viewer.ShowDialog();
		}

		private void testToolStripMenuItem_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;
			Cursor = Cursors.WaitCursor; // cursor relogio

			//copy Undo Image
			imgUndo = img.Copy();

			Image<Bgr, byte> img_red;
			Image<Bgr, byte> img_blue;
			int[] segmentation;

			double total_time = 0;

			for(int i = 0; i < 10; i++) {
				DateTime t1 = DateTime.Now;

				//OpenCL_Class.ColorConvertion_Binarization_Projections_TemplateMatching(img, out img_red, out img_blue);

				img_blue = ImageClass.BlueBinarization(img);
				segmentation = ImageClass.Segmentation(img_blue, 'b');
				ImageClass.TemplateMatching(img, segmentation, MainForm.DataBase);

				img_red = ImageClass.RedBinarization(img);
				segmentation = ImageClass.Segmentation(img_red, 'r');
				ImageClass.TemplateMatching(img, segmentation, MainForm.DataBase);

				DateTime t2 = DateTime.Now;
				total_time += (t2 - t1).TotalMilliseconds;
				
			}

			MessageBox.Show((total_time/10).ToString("n3"));

			ImageViewer.Image = img;

			Cursor = Cursors.Default; // cursor normal
		}

		private void blockSizeToolStripMenuItem_Click(object sender, EventArgs e) {
			InputBox formX = new InputBox("Block Size");
			formX.ShowDialog();

			int _size = Convert.ToInt32(formX.ValueTextBox.Text);

			blockSize = _size;
		}

		private void imageEnhancementToolStripMenuItem_Click(object sender, EventArgs e) {
			if(img == null) // protege de executar a função sem ainda ter aberto a imagem 
				return;
			Cursor = Cursors.WaitCursor; // cursor relogio

			//copy Undo Image
			imgUndo = img.Copy();

			img = ImageClass.ImageEnhancement(img, 'r');

			ImageViewer.Image = img;

			Cursor = Cursors.Default; // cursor normal
		}

	}
}