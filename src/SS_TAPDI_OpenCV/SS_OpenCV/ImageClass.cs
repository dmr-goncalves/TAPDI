using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Emgu.CV.Structure;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.UI;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using MathNet.Numerics;

namespace SS_OpenCV {
	class ImageClass {

		/// <summary>
		/// Negativo da Imagem
		/// Manipulação Imagem - Primitivas EmguCV
		/// Algoritmo de manipulação de imagem mais lento 
		/// </summary>
		/// <param name="img">Imagem</param>
		internal static void Negative(Image<Bgr, byte> img) {
			Bgr aux;
			for(int y = 0; y < img.Height; y++) {
				for(int x = 0; x < img.Width; x++) {
					// acesso directo : mais lento 
					aux = img[y, x];
					img[y, x] = new Bgr(255 - aux.Blue, 255 - aux.Green, 255 - aux.Red);
				}
			}
		}

		/// <summary>
		/// Negativo da Imagem
		/// Manipulação Imagem - Primitivas EmguCV
		/// Algoritmo de inversão de cores da imagem rápido
		/// </summary>
		/// <param name="img">Imagem</param>
		internal static void NegativeFast(Image<Bgr, byte> img) {
			unsafe
			{
				// acesso directo à memoria da imagem (sequencial)
				// direcção top left -> bottom right

				MIplImage m = img.MIplImage;
				byte* dataPtr = (byte*)m.imageData.ToPointer(); // obter apontador do inicio da imagem

				int width = img.Width;
				int height = img.Height;
				int nChan = m.nChannels; // numero de canais 3
				int padding = m.widthStep - m.nChannels * m.width; // alinhamento (padding)
				int x, y;

				if(nChan == 3) { // imagem em RGB
					for(y = 0; y < height; y++) {
						for(x = 0; x < width; x++) {

							// guarda na imagem
							dataPtr[0] = (byte)(255 - dataPtr[0]); //blue
							dataPtr[1] = (byte)(255 - dataPtr[1]); //green
							dataPtr[2] = (byte)(255 - dataPtr[2]); //red

							// avança apontador para próximo pixel
							dataPtr += nChan;
						}

						//no fim da linha avança alinhamento (padding)
						dataPtr += padding;
					}
				}
			}
		}

		/// <summary>
		/// Conversão para Cinzento
		/// Manipulação Imagem - Acesso directo à memoria
		/// </summary>
		/// <param name="img">imagem</param>
		internal static void ConvertToGray(Image<Bgr, byte> img) {
			unsafe
			{
				// acesso directo à memoria da imagem (sequencial)
				// direcção top left -> bottom right

				MIplImage m = img.MIplImage;
				byte* dataPtr = (byte*)m.imageData.ToPointer(); // obter apontador do inicio da imagem
				byte gray;

				int width = img.Width;
				int height = img.Height;
				int nChan = m.nChannels; // numero de canais 3
				int padding = m.widthStep - m.nChannels * m.width; // alinhamento (padding)
				int x, y;

				if(nChan == 3) { // imagem em RGB
					for(y = 0; y < height; y++) {
						for(x = 0; x < width; x++) {

							// converte para cinza
							gray = (byte)((dataPtr[0]/*blue*/ + dataPtr[1]/*green*/ + dataPtr[2]/*red*/) / 3);

							// guarda na imagem
							dataPtr[0] = gray;
							dataPtr[1] = gray;
							dataPtr[2] = gray;

							// avança apontador para próximo pixel
							dataPtr += nChan;
						}

						//no fim da linha avança alinhamento (padding)
						dataPtr += padding;
					}
				}
			}
		}

		/// <summary>
		/// Conversão para Preto e Branco
		/// Manipulação Imagem - Acesso directo à memoria
		/// </summary>
		/// <param name="img">imagem</param>
		/// <param name="threshold">threshold</param>
		internal static void Binarization(Image<Bgr, byte> img, int threshold) {
			unsafe
			{
				// acesso directo à memoria da imagem (sequencial)
				// direcção top left -> bottom right

				MIplImage m = img.MIplImage;
				byte* dataPtr = (byte*)m.imageData.ToPointer(); // obter apontador do inicio da imagem
				byte gray;

				int width = img.Width;
				int height = img.Height;
				int nChan = m.nChannels; // numero de canais 3
				int padding = m.widthStep - m.nChannels * m.width; // alinhamento (padding)
				int x, y;

				if(nChan == 3) { // imagem em RGB
					for(y = 0; y < height; y++) {
						for(x = 0; x < width; x++) {

							// converte para cinza
							gray = (byte)((dataPtr[0]/*blue*/ + dataPtr[1]/*green*/ + dataPtr[2]/*red*/) / 3);

							if(gray <= threshold) {
								// guarda na imagem
								dataPtr[0] = 0;
								dataPtr[1] = 0;
								dataPtr[2] = 0;
							} else {
								// guarda na imagem
								dataPtr[0] = 255;
								dataPtr[1] = 255;
								dataPtr[2] = 255;
							}

							// avança apontador para próximo pixel
							dataPtr += nChan;
						}

						//no fim da linha avança alinhamento (padding)
						dataPtr += padding;
					}
				}
			}
		}

		/// <summary>
		/// Filtragem de Cor
		/// Manipulação Imagem - Primitivas EmguCV
		/// Algoritmo de isolamento de cor
		/// </summary>
		/// <param name="img">Imagem</param>
		/// <param name="color">Cor</param>
		internal static void ColorFilter(Image<Bgr, byte> img, string color) {
			unsafe
			{
				// acesso directo à memoria da imagem (sequencial)
				// direcção top left -> bottom right

				MIplImage m = img.MIplImage;
				byte* dataPtr = (byte*)m.imageData.ToPointer(); // obter apontador do inicio da imagem

				int width = img.Width;
				int height = img.Height;
				int nChan = m.nChannels; // numero de canais 3
				int padding = m.widthStep - m.nChannels * m.width; // alinhamento (padding)
				int x, y;

				switch(color) {
					case "red":
						if(nChan == 3) { // imagem em RGB
							for(y = 0; y < height; y++) {
								for(x = 0; x < width; x++) {

									// guarda na imagem
									dataPtr[0] = dataPtr[2]; //red
									dataPtr[1] = dataPtr[2]; //red
									dataPtr[2] = dataPtr[2]; //red

									// avança apontador para próximo pixel
									dataPtr += nChan;
								}

								//no fim da linha avança alinhamento (padding)
								dataPtr += padding;
							}
						}; break;
					case "green":
						if(nChan == 3) { // imagem em RGB
							for(y = 0; y < height; y++) {
								for(x = 0; x < width; x++) {

									// guarda na imagem
									dataPtr[0] = dataPtr[1]; //green
									dataPtr[1] = dataPtr[1]; //green
									dataPtr[2] = dataPtr[1]; //green

									// avança apontador para próximo pixel
									dataPtr += nChan;
								}

								//no fim da linha avança alinhamento (padding)
								dataPtr += padding;
							}
						}; break;
					case "blue":
						if(nChan == 3) { // imagem em RGB
							for(y = 0; y < height; y++) {
								for(x = 0; x < width; x++) {

									// guarda na imagem
									dataPtr[0] = dataPtr[0]; //blue
									dataPtr[1] = dataPtr[0]; //blue
									dataPtr[2] = dataPtr[0]; //blue

									// avança apontador para próximo pixel
									dataPtr += nChan;
								}

								//no fim da linha avança alinhamento (padding)
								dataPtr += padding;
							}
						}; break;
				}
			}
		}

		/// <summary>
		/// Tranlação
		/// Manipulação Imagem - Acesso directo à memoria
		/// </summary>
		/// <param name="img">imagem</param>
		/// <param name="Dx">pixeis de translação no eixo x</param>
		/// <param name="Dy">pixeis de translação no eixo y</param>
		internal static void Translation(Image<Bgr, byte> img, double Dx, double Dy) {
			unsafe
			{
				// acesso directo à memoria da imagem (sequencial)
				// direcção top left -> bottom right

				Image<Bgr, byte> imgSource = null;
				imgSource = img.Copy();

				MIplImage source = imgSource.MIplImage;
				byte* sourceDataPtr = (byte*)source.imageData.ToPointer(); // obter apontador do inicio da imagem

				MIplImage destination = img.MIplImage;
				byte* destinationDataPtr = (byte*)destination.imageData.ToPointer(); // obter apontador do inicio da imagem

				int width = img.Width;
				int height = img.Height;
				int nChan = source.nChannels; // numero de canais 3
				int padding = source.widthStep - source.nChannels * source.width; // alinhamento (padding)
				int x, y, x_origin, y_origin, sum_x, sum_y;
				double R1_b, R1_g, R1_r, R2_b, R2_g, R2_r, point_b, point_g, point_r;

				byte* auxPtr11, auxPtr12, auxPtr21, auxPtr22;

				double offsetX = ((Dx - (int)Dx) < 0 ? -(Dx - (int)Dx) : (Dx - (int)Dx));
				double offsetY = ((Dy - (int)Dy) < 0 ? -(Dy - (int)Dy) : (Dy - (int)Dy));

				if(nChan == 3) { // imagem em RGB
					for(y = 0; y < height; y++) {
						for(x = 0; x < width; x++) {

							x_origin = x - (int)Dx;
							y_origin = y - (int)Dy;

							if(x_origin < 0 || x_origin >= width || y_origin < 0 || y_origin >= height) {
								destinationDataPtr[0] = 255; //blue
								destinationDataPtr[1] = 255; //green
								destinationDataPtr[2] = 255; //red
							} else {
								sum_y = (y_origin + 1 > (height - 1) ? y_origin : y_origin + 1);
								sum_x = (x_origin + 1 > (width - 1) ? x_origin : x_origin + 1);

								auxPtr11 = (sourceDataPtr + y_origin * destination.widthStep + x_origin * nChan);
								auxPtr12 = (sourceDataPtr + sum_y * destination.widthStep + x_origin * nChan);
								auxPtr21 = (sourceDataPtr + y_origin * destination.widthStep + sum_x * nChan);
								auxPtr22 = (sourceDataPtr + sum_y * destination.widthStep + sum_x * nChan);

								//bilinear
								R1_b = (1 - offsetX) * auxPtr11[0] + offsetX * auxPtr21[0];
								R1_g = (1 - offsetX) * auxPtr11[1] + offsetX * auxPtr21[1];
								R1_r = (1 - offsetX) * auxPtr11[2] + offsetX * auxPtr21[2];

								R2_b = (1 - offsetX) * auxPtr12[0] + offsetX * auxPtr22[0];
								R2_g = (1 - offsetX) * auxPtr12[1] + offsetX * auxPtr22[1];
								R2_r = (1 - offsetX) * auxPtr12[2] + offsetX * auxPtr22[2];

								point_b = (1 - offsetY) * R1_b + offsetY * R2_b;
								point_g = (1 - offsetY) * R1_g + offsetY * R2_g;
								point_r = (1 - offsetY) * R1_r + offsetY * R2_r;

								// guarda na imagem
								destinationDataPtr[0] = (byte)point_b; //blue
								destinationDataPtr[1] = (byte)point_g; //green
								destinationDataPtr[2] = (byte)point_r; //red
							}
							// avança apontador para próximo pixel
							destinationDataPtr += nChan;
						}

						//no fim da linha avança alinhamento (padding)
						destinationDataPtr += padding;
					}
				}
			}
		}

		/// <summary>
		/// Rotação
		/// Manipulação Imagem - Acesso directo à memoria
		/// </summary>
		/// <param name="img">imagem</param>
		/// <param name="Ang">ângulo de rotação</param>
		internal static void Rotation(Image<Bgr, byte> img, double Ang) {
			unsafe
			{
				// acesso directo à memoria da imagem (sequencial)
				// direcção top left -> bottom right

				Image<Bgr, byte> imgSource = null;
				imgSource = img.Copy();

				MIplImage source = imgSource.MIplImage;
				byte* sourceDataPtr = (byte*)source.imageData.ToPointer(); // obter apontador do inicio da imagem

				MIplImage destination = img.MIplImage;
				byte* destinationDataPtr = (byte*)destination.imageData.ToPointer(); // obter apontador do inicio da imagem

				int width = img.Width;
				int height = img.Height;
				int nChan = source.nChannels; // numero de canais 3
				int padding = source.widthStep - source.nChannels * source.width; // alinhamento (padding)
				int x, y, sum_x, sum_y, x_origin_int, y_origin_int;
				double R1_b, R1_g, R1_r, R2_b, R2_g, R2_r;
				int point_b, point_g, point_r;
				double offsetX, offsetY, x_origin, y_origin;

				byte* auxPtr11, auxPtr12, auxPtr21, auxPtr22;

				double radians = (Ang * Math.PI) / 180;
				double cos = Math.Cos(radians);
				double sen = Math.Sin(radians);

				if(nChan == 3) { // imagem em RGB
					for(y = 0; y < height; y++) {
						for(x = 0; x < width; x++) {

							x_origin = (x - (width / 2)) * cos - ((height / 2) - y) * sen + (width / 2);
							y_origin = (height / 2) - ((height / 2) - y) * cos - (x - (width / 2)) * sen;

							offsetX = ((x_origin - (int)x_origin) < 0 ? -(x_origin - (int)x_origin) : (x_origin - (int)x_origin));
							offsetY = ((y_origin - (int)y_origin) < 0 ? -(y_origin - (int)y_origin) : (y_origin - (int)y_origin));

							if(x_origin < 0 || x_origin >= width || y_origin < 0 || y_origin >= height) {

								destinationDataPtr[0] = 255; //blue
								destinationDataPtr[1] = 255; //green
								destinationDataPtr[2] = 255; //red
							} else {

								x_origin_int = (int)x_origin;
								y_origin_int = (int)y_origin;

								sum_y = (y_origin_int + 1 > (height - 1) ? y_origin_int : y_origin_int + 1);
								sum_x = (x_origin_int + 1 > (width - 1) ? x_origin_int : x_origin_int + 1);

								auxPtr11 = (sourceDataPtr + y_origin_int * destination.widthStep + x_origin_int * nChan);
								auxPtr12 = (sourceDataPtr + sum_y * destination.widthStep + x_origin_int * nChan);
								auxPtr21 = (sourceDataPtr + y_origin_int * destination.widthStep + sum_x * nChan);
								auxPtr22 = (sourceDataPtr + sum_y * destination.widthStep + sum_x * nChan);

								//bilinear
								R1_b = (1 - offsetX) * auxPtr11[0] + offsetX * auxPtr21[0];
								R1_g = (1 - offsetX) * auxPtr11[1] + offsetX * auxPtr21[1];
								R1_r = (1 - offsetX) * auxPtr11[2] + offsetX * auxPtr21[2];

								R2_b = (1 - offsetX) * auxPtr12[0] + offsetX * auxPtr22[0];
								R2_g = (1 - offsetX) * auxPtr12[1] + offsetX * auxPtr22[1];
								R2_r = (1 - offsetX) * auxPtr12[2] + offsetX * auxPtr22[2];

								point_b = (int)((1 - offsetY) * R1_b + offsetY * R2_b);
								point_g = (int)((1 - offsetY) * R1_g + offsetY * R2_g);
								point_r = (int)((1 - offsetY) * R1_r + offsetY * R2_r);

								// guarda na imagem
								destinationDataPtr[0] = (byte)point_b; //blue
								destinationDataPtr[1] = (byte)point_g; //green
								destinationDataPtr[2] = (byte)point_r; //red
							}
							// avança apontador para próximo pixel
							destinationDataPtr += nChan;
						}

						//no fim da linha avança alinhamento (padding)
						destinationDataPtr += padding;
					}
				}
			}
		}

		/// <summary>
		/// Zoom
		/// Manipulação Imagem - Acesso directo à memoria
		/// </summary>
		/// <param name="img">imagem</param>
		/// <param name="zoom">factor de ampliação</param>
		/// <param name="x_image">coordenada x do centro</param>
		/// <param name="y_image">coordenada y do centro</param>
		internal static void Zoom(Image<Bgr, byte> img, double zoom, int x_image, int y_image) {
			unsafe
			{
				// acesso directo à memoria da imagem (sequencial)
				// direcção top left -> bottom right

				Image<Bgr, byte> imgSource = null;
				imgSource = img.Copy();

				MIplImage source = imgSource.MIplImage;
				byte* sourceDataPtr = (byte*)source.imageData.ToPointer(); // obter apontador do inicio da imagem

				MIplImage destination = img.MIplImage;
				byte* destinationDataPtr = (byte*)destination.imageData.ToPointer(); // obter apontador do inicio da imagem

				int width = img.Width;
				int height = img.Height;
				int nChan = source.nChannels; // numero de canais 3
				int padding = source.widthStep - source.nChannels * source.width; // alinhamento (padding)
				int x, y, x_origin, y_origin;

				byte* auxPtr;

				if(nChan == 3) { // imagem em RGB
					for(y = 0; y < height; y++) {
						for(x = 0; x < width; x++) {

							x_origin = (int)Math.Round(x / zoom + x_image - x_image / zoom);
							y_origin = (int)Math.Round(y / zoom + y_image - y_image / zoom);

							if(x_origin < 0 || x_origin >= width || y_origin < 0 || y_origin >= height) {
								destinationDataPtr[0] = 255; //blue
								destinationDataPtr[1] = 255; //green
								destinationDataPtr[2] = 255; //red
							} else {
								// guarda na imagem
								auxPtr = (sourceDataPtr + y_origin * destination.widthStep + x_origin * nChan);

								destinationDataPtr[0] = auxPtr[0]; //blue
								destinationDataPtr[1] = auxPtr[1]; //green
								destinationDataPtr[2] = auxPtr[2]; //red
							}
							// avança apontador para próximo pixel
							destinationDataPtr += nChan;
						}

						//no fim da linha avança alinhamento (padding)
						destinationDataPtr += padding;
					}
				}
			}
		}

		/// <summary>
		/// Redimensão
		/// Manipulação Imagem - EmguCV
		/// </summary>
		/// <param name="img">imagem</param>
		/// <param name="width">Largura desejada da imagem</param>
		/// <param name="height">Altura desejada da imagem</param>
		internal static Image<Bgr, byte> mResize(Image<Bgr, byte> img, int width, int height) {
			img = img.Resize(width, height, INTER.CV_INTER_CUBIC);
			return img;
		}

		/// <summary>
		/// Aplicação de filtro de média pelo método A
		/// Manipulação Imagem - Acesso directo à memoria
		/// </summary>
		/// <param name="img">imagem</param>
		/// <param name="size">tamanho da matriz</param>
		internal static void FilterA(Image<Bgr, byte> img, int size) {
			unsafe
			{
				// acesso directo à memoria da imagem (sequencial)
				// direcção top left -> bottom right

				Image<Bgr, byte> imgSource = null;
				imgSource = img.Copy();

				MIplImage source = imgSource.MIplImage;
				byte* sourceDataPtr = (byte*)source.imageData.ToPointer(); // obter apontador do inicio da imagem

				MIplImage destination = img.MIplImage;
				byte* destinationDataPtr = (byte*)destination.imageData.ToPointer(); // obter apontador do inicio da imagem

				int width = img.Width;
				int height = img.Height;
				int nChan = source.nChannels; // numero de canais 3
				int padding = source.widthStep - source.nChannels * source.width; // alinhamento (padding)
				int x, y, x_f, y_f, sum_x, sum_y;

				byte* auxPtr;

				int margin = (int)(size / 2);
				int area = size * size;
				int blue = 0, green = 0, red = 0;

				try {
					if(nChan == 3) { // imagem em RGB
						for(y = 0; y < height; y++) {
							for(x = 0; x < width; x++) {

								for(x_f = -1 * margin; x_f <= margin; x_f++) {
									for(y_f = -1 * margin; y_f <= margin; y_f++) {

										sum_y = (y + y_f < 0 ? -(y + y_f) : ((y + y_f) > (height - 1) ? (y - y_f) : (y + y_f)));
										sum_x = (x + x_f < 0 ? -(x + x_f) : ((x + x_f) > (width - 1) ? (x - x_f) : (x + x_f)));

										auxPtr = (sourceDataPtr + sum_y * destination.widthStep + sum_x * nChan);

										blue += auxPtr[0]; //blue
										green += auxPtr[1]; //green
										red += auxPtr[2]; //red
									}
								}

								// guarda na imagem
								destinationDataPtr[0] = (byte)(blue / area); //blue
								destinationDataPtr[1] = (byte)(green / area); //green
								destinationDataPtr[2] = (byte)(red / area); //red
								blue = 0;
								green = 0;
								red = 0;

								// avança apontador para próximo pixel
								destinationDataPtr += nChan;
							}

							//no fim da linha avança alinhamento (padding)
							destinationDataPtr += padding;
						}
					}
				} catch(Exception ex) {
					Console.WriteLine(ex.ToString());
				}
			}
		}

		/// <summary>
		/// Aplicação de filtro 3x3 com valores expecificados pelo método A
		/// Manipulação Imagem - Acesso directo à memoria
		/// </summary>
		/// <param name="img">imagem</param>
		/// <param name="v_coeficientes">coeficientes especificados</param>
		internal static void FilterMatrix(Image<Bgr, byte> img, int[] v_coeficientes) {
			unsafe
			{
				// acesso directo à memoria da imagem (sequencial)
				// direcção top left -> bottom right

				Image<Bgr, byte> imgSource = null;
				imgSource = img.Copy();

				MIplImage source = imgSource.MIplImage;
				byte* sourceDataPtr = (byte*)source.imageData.ToPointer(); // obter apontador do inicio da imagem

				MIplImage destination = img.MIplImage;
				byte* destinationDataPtr = (byte*)destination.imageData.ToPointer(); // obter apontador do inicio da imagem

				int width = img.Width;
				int height = img.Height;
				int nChan = source.nChannels; // numero de canais 3
				int padding = source.widthStep - source.nChannels * source.width; // alinhamento (padding)
				int x, y, x_f, y_f;

				int margin = 1;
				int weight = v_coeficientes[9];
				int blue = 0, green = 0, red = 0, blue_total = 0, green_total = 0, red_total = 0;
				int i = 0;
				int sum_x, sum_y;

				byte* auxPtr;

				if(nChan == 3) { // imagem em RGB
					for(y = 0; y < height; y++) {
						for(x = 0; x < width; x++) {

							for(y_f = -1 * margin; y_f <= margin; y_f++) {
								for(x_f = -1 * margin; x_f <= margin; x_f++) {

									sum_y = (y + y_f < 0 ? -(y + y_f) : ((y + y_f) > (height - 1) ? (y - y_f) : (y + y_f)));
									sum_x = (x + x_f < 0 ? -(x + x_f) : ((x + x_f) > (width - 1) ? (x - x_f) : (x + x_f)));

									auxPtr = (sourceDataPtr + sum_y * destination.widthStep + sum_x * nChan);

									blue_total += v_coeficientes[i] * auxPtr[0]; //blue
									green_total += v_coeficientes[i] * auxPtr[1]; //green
									red_total += v_coeficientes[i] * auxPtr[2]; //red
									i++;
								}
							}

							blue = (blue_total < 0 ? -blue_total : blue_total) / weight;
							green = (green_total < 0 ? -green_total : green_total) / weight;
							red = (red_total < 0 ? -red_total : red_total) / weight;

							if(blue > 255)
								blue = 255;

							if(green > 255)
								green = 255;

							if(red > 255)
								red = 255;

							// guarda na imagem
							destinationDataPtr[0] = (byte)blue; //blue
							destinationDataPtr[1] = (byte)green; //green
							destinationDataPtr[2] = (byte)red; //red
							blue_total = 0;
							green_total = 0;
							red_total = 0;
							i = 0;

							// avança apontador para próximo pixel
							destinationDataPtr += nChan;
						}

						//no fim da linha avança alinhamento (padding)
						destinationDataPtr += padding;
					}
				}
			}
		}

		/// <summary>
		/// Aplicação de filtro de Sobel 5x5 pelo método A
		/// Manipulação Imagem - Acesso directo à memoria
		/// </summary>
		/// <param name="img">imagem</param>
		/// <param name="v_coeficientes1">coeficientes verticais</param>
		/// <param name="v_coeficientes2">coeficientes horizontais</param>
		internal static void Sobel3x3(Image<Bgr, byte> img, int[] v_coeficientes1, int[] v_coeficientes2) {
			unsafe
			{
				// acesso directo à memoria da imagem (sequencial)
				// direcção top left -> bottom right

				Image<Bgr, byte> imgSource = null;
				imgSource = img.Copy();

				MIplImage source = imgSource.MIplImage;
				byte* sourceDataPtr = (byte*)source.imageData.ToPointer(); // obter apontador do inicio da imagem

				MIplImage destination = img.MIplImage;
				byte* destinationDataPtr = (byte*)destination.imageData.ToPointer(); // obter apontador do inicio da imagem

				int width = img.Width;
				int height = img.Height;
				int nChan = source.nChannels; // numero de canais 3
				int padding = source.widthStep - source.nChannels * source.width; // alinhamento (padding)
				int x, y, x_f, y_f, sum_x, sum_y;

				int margin = 1;
				int blue = 0, green = 0, red = 0, blue_Sx = 0, blue_Sy = 0, green_Sx = 0, green_Sy = 0, red_Sx = 0, red_Sy = 0;
				int i = 0;

				byte* auxPtr;

				if(nChan == 3) { // imagem em RGB
					for(y = 0; y < height; y++) {
						for(x = 0; x < width; x++) {

							for(y_f = -1 * margin; y_f <= margin; y_f++) {
								for(x_f = -1 * margin; x_f <= margin; x_f++) {

									sum_y = (y + y_f < 0 ? -(y + y_f) : ((y + y_f) > (height - 1) ? (y - y_f) : (y + y_f)));
									sum_x = (x + x_f < 0 ? -(x + x_f) : ((x + x_f) > (width - 1) ? (x - x_f) : (x + x_f)));

									auxPtr = (sourceDataPtr + sum_y * destination.widthStep + sum_x * nChan);

									blue_Sx += v_coeficientes1[i] * auxPtr[0]; //blue
									green_Sx += v_coeficientes1[i] * auxPtr[1]; //green
									red_Sx += v_coeficientes1[i] * auxPtr[2]; //red

									blue_Sy += v_coeficientes2[i] * auxPtr[0]; //blue
									green_Sy += v_coeficientes2[i] * auxPtr[1]; //green
									red_Sy += v_coeficientes2[i] * auxPtr[2]; //red
									i++;
								}
							}

							blue = (blue_Sx < 0 ? -blue_Sx : blue_Sx) + (blue_Sy < 0 ? -blue_Sy : blue_Sy);
							green = (green_Sx < 0 ? -green_Sx : green_Sx) + (green_Sy < 0 ? -green_Sy : green_Sy);
							red = (red_Sx < 0 ? -red_Sx : red_Sx) + (red_Sy < 0 ? -red_Sy : red_Sy);

							if(blue > 255)
								blue = 255;

							if(green > 255)
								green = 255;

							if(red > 255)
								red = 255;

							// guarda na imagem
							destinationDataPtr[0] = (byte)blue; //blue
							destinationDataPtr[1] = (byte)green; //green
							destinationDataPtr[2] = (byte)red; //red
							blue_Sx = 0;
							blue_Sy = 0;
							green_Sx = 0;
							green_Sy = 0;
							red_Sx = 0;
							red_Sy = 0; ;
							i = 0;

							// avança apontador para próximo pixel
							destinationDataPtr += nChan;
						}

						//no fim da linha avança alinhamento (padding)
						destinationDataPtr += padding;
					}
				}
			}
		}

		/// <summary>
		/// Aplicação de filtro de Sobel 5x5 pelo método A
		/// Manipulação Imagem - Acesso directo à memoria
		/// </summary>
		/// <param name="img">imagem</param>
		/// <param name="v_coeficientes1">coeficientes verticais</param>
		/// <param name="v_coeficientes2">coeficientes horizontais</param>
		internal static void Sobel5x5(Image<Bgr, byte> img, int[] v_coeficientes1, int[] v_coeficientes2) {
			unsafe
			{
				// acesso directo à memoria da imagem (sequencial)
				// direcção top left -> bottom right

				Image<Bgr, byte> imgSource = null;
				imgSource = img.Copy();

				MIplImage source = imgSource.MIplImage;
				byte* sourceDataPtr = (byte*)source.imageData.ToPointer(); // obter apontador do inicio da imagem

				MIplImage destination = img.MIplImage;
				byte* destinationDataPtr = (byte*)destination.imageData.ToPointer(); // obter apontador do inicio da imagem

				int width = img.Width;
				int height = img.Height;
				int nChan = source.nChannels; // numero de canais 3
				int padding = source.widthStep - source.nChannels * source.width; // alinhamento (padding)
				int x, y, x_f, y_f, sum_x, sum_y;

				int margin = 2;
				int blue = 0, green = 0, red = 0, blue_Sx = 0, blue_Sy = 0, green_Sx = 0, green_Sy = 0, red_Sx = 0, red_Sy = 0;
				int i = 0;

				byte* auxPtr;

				if(nChan == 3) { // imagem em RGB
					for(y = 0; y < height; y++) {
						for(x = 0; x < width; x++) {

							for(y_f = -1 * margin; y_f <= margin; y_f++) {
								for(x_f = -1 * margin; x_f <= margin; x_f++) {

									sum_y = (y + y_f < 0 ? -(y + y_f) : ((y + y_f) > (height - 1) ? (y - y_f) : (y + y_f)));
									sum_x = (x + x_f < 0 ? -(x + x_f) : ((x + x_f) > (width - 1) ? (x - x_f) : (x + x_f)));

									auxPtr = (sourceDataPtr + sum_y * destination.widthStep + sum_x * nChan);

									blue_Sx += v_coeficientes1[i] * auxPtr[0]; //blue
									green_Sx += v_coeficientes1[i] * auxPtr[1]; //green
									red_Sx += v_coeficientes1[i] * auxPtr[2]; //red

									blue_Sy += v_coeficientes2[i] * auxPtr[0]; //blue
									green_Sy += v_coeficientes2[i] * auxPtr[1]; //green
									red_Sy += v_coeficientes2[i] * auxPtr[2]; //red

									i++;
								}
							}

							blue = (int)Math.Sqrt(blue_Sx * blue_Sx + blue_Sy * blue_Sy);
							green = (int)Math.Sqrt(green_Sx * green_Sx + green_Sy * green_Sy);
							red = (int)Math.Sqrt(red_Sx * red_Sx + red_Sy * red_Sy);

							if(blue > 255)
								blue = 255;

							if(green > 255)
								green = 255;

							if(red > 255)
								red = 255;

							// guarda na imagem
							destinationDataPtr[0] = (byte)blue; //blue
							destinationDataPtr[1] = (byte)green; //green
							destinationDataPtr[2] = (byte)red; //red
							blue_Sx = 0;
							blue_Sy = 0;
							green_Sx = 0;
							green_Sy = 0;
							red_Sx = 0;
							red_Sy = 0; ;
							i = 0;

							// avança apontador para próximo pixel
							destinationDataPtr += nChan;
						}

						//no fim da linha avança alinhamento (padding)
						destinationDataPtr += padding;
					}
				}
			}
		}

		/// <summary>
		/// Aplicação de filtro de Diferenciação
		/// Manipulação Imagem - Acesso directo à memoria
		/// </summary>
		/// <param name="img">imagem</param>
		internal static void Differentiation(Image<Bgr, byte> img) {
			unsafe
			{
				// acesso directo à memoria da imagem (sequencial)
				// direcção top left -> bottom right

				Image<Bgr, byte> imgSource = null;
				imgSource = img.Copy();

				MIplImage source = imgSource.MIplImage;
				byte* sourceDataPtr = (byte*)source.imageData.ToPointer(); // obter apontador do inicio da imagem

				MIplImage destination = img.MIplImage;
				byte* destinationDataPtr = (byte*)destination.imageData.ToPointer(); // obter apontador do inicio da imagem

				int width = img.Width;
				int height = img.Height;
				int nChan = source.nChannels; // numero de canais 3
				int padding = source.widthStep - source.nChannels * source.width; // alinhamento (padding)
				int x, y, sum_x, sum_y;

				int blue = 0, green = 0, red = 0, blue_right = 0, blue_down = 0, green_right = 0, green_down = 0, red_right = 0, red_down = 0;

				byte* auxPtr, auxPtrRight, auxPtrDown;

				if(nChan == 3) { // imagem em RGB
					for(y = 0; y < height; y++) {
						for(x = 0; x < width; x++) {

							sum_y = ((y + 1) > (height - 1) ? (y - 1) : (y + 1));
							sum_x = ((x + 1) > (width - 1) ? (x - 1) : (x + 1));

							auxPtr = (sourceDataPtr + y * destination.widthStep + x * nChan);
							auxPtrRight = (sourceDataPtr + (sum_y - 1) * destination.widthStep + sum_x * nChan);
							auxPtrDown = (sourceDataPtr + sum_y * destination.widthStep + (sum_x - 1) * nChan);

							blue_right = auxPtr[0] - auxPtrRight[0]; //blue
							green_right = auxPtr[1] - auxPtrRight[1]; //green
							red_right = auxPtr[2] - auxPtrRight[2]; //red

							blue_down = auxPtr[0] - auxPtrDown[0]; //blue
							green_down = auxPtr[1] - auxPtrDown[1]; //green
							red_down = auxPtr[2] - auxPtrDown[2]; //red

							blue = (blue_right < 0 ? -blue_right : blue_right) + (blue_down < 0 ? -blue_down : blue_down);
							green = (green_right < 0 ? -green_right : green_right) + (green_down < 0 ? -green_down : green_down);
							red = (red_right < 0 ? -red_right : red_right) + (red_down < 0 ? -red_down : red_down);

							if(blue > 255)
								blue = 255;

							if(green > 255)
								green = 255;

							if(red > 255)
								red = 255;

							// guarda na imagem
							destinationDataPtr[0] = (byte)blue; //blue
							destinationDataPtr[1] = (byte)green; //green
							destinationDataPtr[2] = (byte)red; //red

							// avança apontador para próximo pixel
							destinationDataPtr += nChan;
						}

						//no fim da linha avança alinhamento (padding)
						destinationDataPtr += padding;
					}
				}
			}
		}

		/// <summary>
		/// Aplicação de filtro de Roberts
		/// Manipulação Imagem - Acesso directo à memoria
		/// </summary>
		/// <param name="img">imagem</param>
		internal static void Roberts(Image<Bgr, byte> img) {
			unsafe
			{
				// acesso directo à memoria da imagem (sequencial)
				// direcção top left -> bottom right

				Image<Bgr, byte> imgSource = null;
				imgSource = img.Copy();

				MIplImage source = imgSource.MIplImage;
				byte* sourceDataPtr = (byte*)source.imageData.ToPointer(); // obter apontador do inicio da imagem

				MIplImage destination = img.MIplImage;
				byte* destinationDataPtr = (byte*)destination.imageData.ToPointer(); // obter apontador do inicio da imagem

				int width = img.Width;
				int height = img.Height;
				int nChan = source.nChannels; // numero de canais 3
				int padding = source.widthStep - source.nChannels * source.width; // alinhamento (padding)
				int x, y, sum_x, sum_y;

				int blue = 0, green = 0, red = 0;
				int blue_diag_principal = 0, green_diag_principal = 0, red_diag_principal = 0;
				int blue_diag_secundary = 0, green_diag_secundary = 0, red_diag_secundary = 0;

				byte* auxPtr, auxPtrRight, auxPtrDown, auxPtrDiag;

				if(nChan == 3) { // imagem em RGB
					for(y = 0; y < height; y++) {
						for(x = 0; x < width; x++) {

							sum_y = ((y + 1) > (height - 1) ? (y - 1) : (y + 1));
							sum_x = ((x + 1) > (width - 1) ? (x - 1) : (x + 1));

							auxPtr = (sourceDataPtr + y * destination.widthStep + x * nChan);
							auxPtrRight = (sourceDataPtr + (sum_y - 1) * destination.widthStep + sum_x * nChan);
							auxPtrDown = (sourceDataPtr + sum_y * destination.widthStep + (sum_x - 1) * nChan);
							auxPtrDiag = (sourceDataPtr + sum_y * destination.widthStep + sum_x * nChan);

							blue_diag_principal = auxPtr[0] - auxPtrDiag[0]; //blue
							green_diag_principal = auxPtr[1] - auxPtrDiag[1]; //green
							red_diag_principal = auxPtr[2] - auxPtrDiag[2]; //red

							blue_diag_secundary = auxPtrRight[0] - auxPtrDown[0]; //blue
							green_diag_secundary = auxPtrRight[1] - auxPtrDown[1]; //green
							red_diag_secundary = auxPtrRight[2] - auxPtrDown[2]; //red

							blue = (blue_diag_principal < 0 ? -blue_diag_principal : blue_diag_principal) + (blue_diag_secundary < 0 ? -blue_diag_secundary : blue_diag_secundary);
							green = (green_diag_principal < 0 ? -green_diag_principal : green_diag_principal) + (green_diag_secundary < 0 ? -green_diag_secundary : green_diag_secundary);
							red = (red_diag_principal < 0 ? -red_diag_principal : red_diag_principal) + (red_diag_secundary < 0 ? -red_diag_secundary : red_diag_secundary);

							if(blue > 255)
								blue = 255;

							if(green > 255)
								green = 255;

							if(red > 255)
								red = 255;

							// guarda na imagem
							destinationDataPtr[0] = (byte)blue; //blue
							destinationDataPtr[1] = (byte)green; //green
							destinationDataPtr[2] = (byte)red; //red

							// avança apontador para próximo pixel
							destinationDataPtr += nChan;
						}

						//no fim da linha avança alinhamento (padding)
						destinationDataPtr += padding;
					}
				}
			}
		}

		/// <summary>
		/// Aplicação de filtro de Média
		/// Manipulação Imagem - Acesso directo à memoria
		/// </summary>
		/// <param name="img">imagem</param>
		internal static void Median(Image<Bgr, byte> img) {
			unsafe
			{
				// acesso directo à memoria da imagem (sequencial)
				// direcção top left -> bottom right

				Image<Bgr, byte> imgSource = null;
				imgSource = img.Copy();

				MIplImage source = imgSource.MIplImage;
				byte* sourceDataPtr = (byte*)source.imageData.ToPointer(); // obter apontador do inicio da imagem

				MIplImage destination = img.MIplImage;
				byte* destinationDataPtr = (byte*)destination.imageData.ToPointer(); // obter apontador do inicio da imagem

				int width = img.Width;
				int height = img.Height;
				int nChan = source.nChannels; // numero de canais 3
				int padding = source.widthStep - source.nChannels * source.width; // alinhamento (padding)
				int x, y, sum_xp, sum_xm, sum_yp, sum_ym, i, j, minIndex, min;

				int[] dist_points = new int[9];

				int[] blue = new int[9];
				int[] green = new int[9];
				int[] red = new int[9];
				int[] delta = new int[9];

				byte* auxPtr11, auxPtr12, auxPtr13, auxPtr21, auxPtr22, auxPtr23, auxPtr31, auxPtr32, auxPtr33;

				if(nChan == 3) { // imagem em RGB
					for(y = 0; y < height; y++) {
						for(x = 0; x < width; x++) {

							sum_ym = (y - 1 < 0 ? -(y - 1) : y - 1);
							sum_yp = ((y + 1) > (height - 1) ? y - 1 : y + 1);
							sum_xm = (x - 1 < 0 ? -(x - 1) : x - 1);
							sum_xp = ((x + 1) > (width - 1) ? x - 1 : x + 1);

							auxPtr11 = (sourceDataPtr + (sum_ym) * destination.widthStep + (sum_xm) * nChan);
							blue[0] = auxPtr11[0]; //blue
							green[0] = auxPtr11[1]; //green
							red[0] = auxPtr11[2]; //red

							auxPtr12 = (sourceDataPtr + (sum_ym) * destination.widthStep + (x) * nChan);
							blue[1] = auxPtr12[0]; //blue
							green[1] = auxPtr12[1]; //green
							red[1] = auxPtr12[2]; //red

							auxPtr13 = (sourceDataPtr + (sum_ym) * destination.widthStep + (sum_xp) * nChan);
							blue[2] = auxPtr13[0]; //blue
							green[2] = auxPtr13[1]; //green
							red[2] = auxPtr13[2]; //red

							auxPtr21 = (sourceDataPtr + (y) * destination.widthStep + (sum_xm) * nChan);
							blue[3] = auxPtr21[0]; //blue
							green[3] = auxPtr21[1]; //green
							red[3] = auxPtr21[2]; //red

							auxPtr22 = (sourceDataPtr + (y) * destination.widthStep + (x) * nChan);
							blue[4] = auxPtr22[0]; //blue
							green[4] = auxPtr22[1]; //green
							red[4] = auxPtr22[2]; //red

							auxPtr23 = (sourceDataPtr + (y) * destination.widthStep + (sum_xp) * nChan);
							blue[5] = auxPtr23[0]; //blue
							green[5] = auxPtr23[1]; //green
							red[5] = auxPtr23[2]; //red

							auxPtr31 = (sourceDataPtr + (sum_yp) * destination.widthStep + (sum_xm) * nChan);
							blue[6] = auxPtr31[0]; //blue
							green[6] = auxPtr31[1]; //green
							red[6] = auxPtr31[2]; //red

							auxPtr32 = (sourceDataPtr + (sum_yp) * destination.widthStep + (x) * nChan);
							blue[7] = auxPtr32[0]; //blue
							green[7] = auxPtr32[1]; //green
							red[7] = auxPtr32[2]; //red

							auxPtr33 = (sourceDataPtr + (sum_yp) * destination.widthStep + (sum_xp) * nChan);
							blue[8] = auxPtr33[0]; //blue
							green[8] = auxPtr33[1]; //green
							red[8] = auxPtr33[2]; //red

							for(i = 0; i < 9; i++) {
								for(j = 0; j < 9; j++) {
									if(i != j) {
										delta[i] += ((blue[i] - blue[j]) < 0 ? -(blue[i] - blue[j]) : (blue[i] - blue[j])) + ((green[i] - green[j]) < 0 ? -(green[i] - green[j]) : (green[i] - green[j])) + ((red[i] - red[j]) < 0 ? -(red[i] - red[j]) : (red[i] - red[j]));
									}
								}
							}

							min = delta.Min();
							minIndex = delta.ToList().IndexOf(min);

							// guarda na imagem
							destinationDataPtr[0] = (byte)blue[minIndex]; //blue
							destinationDataPtr[1] = (byte)green[minIndex]; //green
							destinationDataPtr[2] = (byte)red[minIndex]; //red

							delta[0] = 0;
							delta[1] = 0;
							delta[2] = 0;
							delta[3] = 0;
							delta[4] = 0;
							delta[5] = 0;
							delta[6] = 0;
							delta[7] = 0;
							delta[8] = 0;

							// avança apontador para próximo pixel
							destinationDataPtr += nChan;
						}

						//no fim da linha avança alinhamento (padding)
						destinationDataPtr += padding;
					}
				}
			}
		}

		/// <summary>
		/// Binarização por Método de Otsu
		/// Manipulação Imagem - Acesso directo à memoria
		/// </summary>
		/// <param name="img">imagem</param>
		internal static void Otsu(Image<Bgr, byte> img) {
			unsafe
			{
				// acesso directo à memoria da imagem (sequencial)
				// direcção top left -> bottom right

				MIplImage m = img.MIplImage;
				byte* dataPtr = (byte*)m.imageData.ToPointer(); // obter apontador do inicio da imagem

				int width = img.Width;
				int height = img.Height;
				int nChan = m.nChannels; // numero de canais 3
				int padding = m.widthStep - m.nChannels * m.width; // alinhamento (padding)
				int x, y, gray, i, n;
				float q1 = 0, q2 = 0, u1 = 0, u2 = 0, difference;

				float max = 0;
				int thres = 0;

				//histogram array
				int[] arrayGray = new int[256];
				//probability array
				float[] arrayProb = new float[256];
				//sigma array
				float[] arraySigma = new float[256];

				//amount of pixels
				int pixel_area = width * height;

				//make a histogram
				if(nChan == 3) { // imagem em RGB
					for(y = 0; y < height; y++) {
						for(x = 0; x < width; x++) {

							gray = (dataPtr[0] + dataPtr[1] + dataPtr[2]) / 3;
							arrayGray[gray]++;

							// avança apontador para próximo pixel
							dataPtr += nChan;
						}

						//no fim da linha avança alinhamento (padding)
						dataPtr += padding;
					}
				}

				//producing array of probabilities
				for(i = 0; i < 256; i++) {
					arrayProb[i] = (float)arrayGray[i] / pixel_area;
				}

				//algoritm
				for(n = 0; n < 256; n++) {

					//sum of probabilities left
					for(i = 0; i <= n; i++) { //q1
						q1 += arrayProb[i];
					}

					//sum of probabilities right
					q2 = 1 - q1; //q2

					for(i = 0; i <= n; i++) { //u1
						u1 += i * arrayProb[i];
					}

					for(i = n + 1; i < 256; i++) { //u2
						u2 += i * arrayProb[i];
					}

					//check if some of the sum of probabilities is 0 for division by 0
					if(q1 * q2 != 0) {
						difference = (u1 / q1) - (u2 / q2);
						arraySigma[n] = q1 * q2 * (difference * difference);
					} else {
						arraySigma[n] = 0;
					}

					//reinicialize variables
					q1 = 0;
					q2 = 0;
					u1 = 0;
					u2 = 0;

					//check for max
					if(max < arraySigma[n]) {
						max = arraySigma[n];
						thres = n;
					}
				}
				//apply threshold
				Binarization(img, thres);
			}
		}

		/// <summary>
		/// Histograma
		/// Manipulação Imagem - Acesso directo à memoria
		/// </summary>
		/// <param name="img">imagem</param>
		internal static List<int[]> Histogram(Image<Bgr, byte> img) {
			unsafe
			{
				// acesso directo à memoria da imagem (sequencial)
				// direcção top left -> bottom right

				MIplImage m = img.MIplImage;
				byte* dataPtr = (byte*)m.imageData.ToPointer(); // obter apontador do inicio da imagem

				int width = img.Width;
				int height = img.Height;
				int nChan = m.nChannels; // numero de canais 3
				int padding = m.widthStep - m.nChannels * m.width; // alinhamento (padding)
				int x, y;
				int gray;

				int[] arrayGray = new int[256];
				int[] arrayB = new int[256];
				int[] arrayG = new int[256];
				int[] arrayR = new int[256];

				int[] arrayGBGR = new int[4];

				if(nChan == 3) { // imagem em RGB
					for(y = 0; y < height; y++) {
						for(x = 0; x < width; x++) {

							gray = (dataPtr[0] + dataPtr[1] + dataPtr[2]) / 3;

							arrayGray[gray]++;
							arrayB[dataPtr[0]]++;
							arrayG[dataPtr[1]]++;
							arrayR[dataPtr[2]]++;

							// avança apontador para próximo pixel
							dataPtr += nChan;
						}

						//no fim da linha avança alinhamento (padding)
						dataPtr += padding;
					}
				}
				List<int[]> listGBGR = new List<int[]>() { arrayGray, arrayB, arrayG, arrayR };

				return listGBGR;
			}
		}

		/// <summary>
		/// Histograma só de um dos eixos
		/// Manipulação Imagem - Acesso directo à memoria
		/// </summary>
		/// <param name="img">imagem</param>
		/// <param name="axis">eixo</param>
		internal static int[] HistogramXY(Image<Bgr, byte> img, char axis) {
			unsafe
			{
				// acesso directo à memoria da imagem (sequencial)
				// direcção top left -> bottom right

				MIplImage m = img.MIplImage;
				byte* dataPtr = (byte*)m.imageData.ToPointer(); // obter apontador do inicio da imagem

				int width = img.Width;
				int height = img.Height;
				int nChan = m.nChannels; // numero de canais 3
				int padding = m.widthStep - m.nChannels * m.width; // alinhamento (padding)
				int x, y;

				//amount of pixels
				int pixel_area = width * height;
				int[] array;

				if(axis == 'x') {
					array = new int[width];
				} else {
					array = new int[height];
				}

				if(axis == 'x') {
					if(nChan == 3) { // imagem em RGB
						for(y = 0; y < height; y++) {
							for(x = 0; x < width; x++) {

								if((dataPtr[0] == 255) && (dataPtr[1] == 255) && (dataPtr[2] == 255)) {
									array[x]++;
								}

								// avança apontador para próximo pixel
								dataPtr += nChan;
							}

							//no fim da linha avança alinhamento (padding)
							dataPtr += padding;
						}
					}
				} else {
					if(nChan == 3) { // imagem em RGB
						for(y = 0; y < height; y++) {
							for(x = 0; x < width; x++) {

								if((dataPtr[0] == 255) && (dataPtr[1] == 255) && (dataPtr[2] == 255)) {
									array[y]++;
								}

								// avança apontador para próximo pixel
								dataPtr += nChan;
							}

							//no fim da linha avança alinhamento (padding)
							dataPtr += padding;
						}
					}
				}
				//Histogram formX = new Histogram(array, array, array, array, array.Length);
				//formX.ShowDialog();
				return array;
			}
		}

		/// <summary>
		/// Histograma de um dos eixos em cooredenadas especificas
		/// Manipulação Imagem - Acesso directo à memoria
		/// </summary>
		/// <param name="img">imagem</param>
		/// <param name="axis">eixo</param>
		/// <param name="coordenate1">coordenada de início</param>
		/// <param name="coordenate2">coordenada de fim</param>
		internal static int[] HistogramCustom(Image<Bgr, byte> img, char axis, int coordenate1, int coordenate2) {
			unsafe
			{
				// acesso directo à memoria da imagem (sequencial)
				// direcção top left -> bottom right

				MIplImage m = img.MIplImage;
				byte* dataPtr = (byte*)m.imageData.ToPointer(); // obter apontador do inicio da imagem

				int width = img.Width;
				int height = img.Height;
				int nChan = m.nChannels; // numero de canais 3
				int padding = m.widthStep - m.nChannels * m.width; // alinhamento (padding)
				int x, y;

				//amount of pixels
				int pixel_area = width * height;
				int[] array;

				if(axis == 'x') {
					array = new int[width];
				} else {
					array = new int[height];
				}

				if(axis == 'x') {
					if(nChan == 3) { // imagem em RGB
						for(y = 0; y < height; y++) {
							for(x = 0; x < width; x++) {

								if((y >= coordenate1 && y <= coordenate2) && (dataPtr[0] == 255) && (dataPtr[1] == 255) && (dataPtr[2] == 255)) {
									array[x]++;
								}

								// avança apontador para próximo pixel
								dataPtr += nChan;
							}

							//no fim da linha avança alinhamento (padding)
							dataPtr += padding;
						}
					}
				} else {
					if(nChan == 3) { // imagem em RGB
						for(y = 0; y < height; y++) {
							for(x = 0; x < width; x++) {

								if((x >= coordenate1 && x <= coordenate2) && (dataPtr[0] == 255) && (dataPtr[1] == 255) && (dataPtr[2] == 255)) {
									array[y]++;
								}

								// avança apontador para próximo pixel
								dataPtr += nChan;
							}

							//no fim da linha avança alinhamento (padding)
							dataPtr += padding;
						}
					}
				}
				//Histogram formX = new Histogram(array, array, array, array, array.Length);
				//formX.ShowDialog();
				return array;
			}
		}

		/// <summary>
		/// Detecção de Vermelho
		/// Manipulação Imagem - Acesso directo à memoria
		/// </summary>
		/// <param name="img">imagem</param>
		/// <param name="MF">MainForm</param>
		internal static void RedDetectionFailed(Image<Bgr, byte> imgOriginal, MainForm MF) {
			unsafe
			{
				// acesso directo à memoria da imagem (sequencial)
				// direcção top left -> bottom right

				MIplImage m = imgOriginal.MIplImage;
				byte* dataPtrOriginal = (byte*)m.imageData.ToPointer(); // obter apontador do inicio da imagem

				Image<Bgr, byte> img = null;
				img = imgOriginal.Copy();

				MIplImage source = img.MIplImage;
				byte* dataPtr = (byte*)source.imageData.ToPointer(); // obter apontador do inicio da imagem

				int width = img.Width;
				int height = img.Height;
				int nChan = m.nChannels; // numero de canais 3
				int padding = m.widthStep - m.nChannels * m.width; // alinhamento (padding)
				int x, y;

				if(nChan == 3) { // imagem em RGB
					for(y = 0; y < height; y++) {
						for(x = 0; x < width; x++) {

							if(dataPtr[2] > 120 && dataPtr[1] < 100 && dataPtr[0] < 100) {

								// guarda na imagem
								dataPtr[0] = 255; //blue
								dataPtr[1] = 255; //green
								dataPtr[2] = 255; //red
							} else {
								// guarda na imagem
								dataPtr[0] = 0; //blue
								dataPtr[1] = 0; //green
								dataPtr[2] = 0; //red
							}

							// avança apontador para próximo pixel
							dataPtr += nChan;
						}

						//no fim da linha avança alinhamento (padding)
						dataPtr += padding;
					}
				}
				FilterA(img, 9);
				Otsu(img);

				int[] arrayX = HistogramXY(img, 'x');
				int[] arrayY = HistogramXY(img, 'y');
				int i, max = 0, index = 0, ptrLeft = 0, ptrRight = 0, dist = 1, area = 0, last_area = 0;

				for(i = 0; i < width; i++) {
					index = (max < arrayX[i] ? i : index);
					max = (max < arrayX[i] ? arrayX[i] : max);
				}

				/*AREA OPTIMIZATION WITH OTSU*/

				float[] arrayProbX = new float[width];
				float[] arrayProbY = new float[height];
				float[] arraySigmaX, arraySigmaY;

				int n, thresX = 0, thresY = 0;
				float q1 = 0, q2 = 0, u1 = 0, u2 = 0, difference;
				float maxX = 0, maxY = 0;

				//amount of pixels
				int pixel_area = width * height;

				//producing array of probabilities for X
				for(i = 0; i < width; i++) {
					arrayProbX[i] = (float)arrayX[i] / pixel_area;
				}

				//producing array of probabilities for Y
				for(i = 0; i < height; i++) {
					arrayProbY[i] = (float)arrayY[i] / pixel_area;
				}

				arraySigmaX = new float[width];

				//algoritm for X
				for(n = 0; n < width; n++) {

					//sum of probabilities left
					for(i = 0; i <= n; i++) { //q1
						q1 += arrayProbX[i];
					}

					//sum of probabilities right
					for(i = n + 1; i < width; i++) { //q2
						q2 += arrayProbX[i];
					}

					for(i = 0; i <= n; i++) { //u1
						u1 += i * arrayProbX[i];
					}

					for(i = n + 1; i < width; i++) { //u2
						u2 += i * arrayProbX[i];
					}

					//check if some of the sum of probabilities is 0 for division by 0
					if(q1 * q2 != 0) {
						difference = (u1 / q1) - (u2 / q2);
						arraySigmaX[n] = q1 * q2 * (difference * difference);
					} else {
						arraySigmaX[n] = 0;
					}

					//reinicialize variables
					q1 = 0;
					q2 = 0;
					u1 = 0;
					u2 = 0;

					//check for max
					if(maxX < arraySigmaX[n]) {
						maxX = arraySigmaX[n];
						thresX = n;
					}
				}

				arraySigmaY = new float[height];

				//algoritm for Y
				for(n = 0; n < height; n++) {

					//sum of probabilities left
					for(i = 0; i <= n; i++) { //q1
						q1 += arrayProbY[i];
					}

					//sum of probabilities right
					for(i = n + 1; i < height; i++) { //q2
						q2 += arrayProbY[i];
					}

					for(i = 0; i <= n; i++) { //u1
						u1 += i * arrayProbY[i];
					}

					for(i = n + 1; i < height; i++) { //u2
						u2 += i * arrayProbY[i];
					}

					//check if some of the sum of probabilities is 0 for division by 0
					if(q1 * q2 != 0) {
						difference = (u1 / q1) - (u2 / q2);
						arraySigmaY[n] = q1 * q2 * (difference * difference);
					} else {
						arraySigmaY[n] = 0;
					}

					//reinicialize variables
					q1 = 0;
					q2 = 0;
					u1 = 0;
					u2 = 0;

					//check for max
					if(maxY < arraySigmaY[n]) {
						maxY = arraySigmaY[n];
						thresY = n;
					}
				}

				/*AREA OPTIMIZATION WITH OTSU*/

				/*GETTING THE OBJECT*/

				int leftDelta = 0, rightDelta = 0;
				int upDelta = 0, downDelta = 0;
				int ptrUp = 0, ptrDown = 0;

				area = arrayX[thresX];
				last_area = area;
				ptrRight = thresX + dist;
				area = area + arrayX[ptrRight];
				rightDelta = area - last_area;

				while(rightDelta > 10 && ptrRight < width) {
					area = area + arrayX[ptrRight];
					rightDelta = area - last_area;
					last_area = area;

					dist++;

					ptrRight = thresX + dist;
				}

				dist = 1; //reset of the variable
				area = arrayX[thresX];
				last_area = area;
				ptrLeft = thresX - dist;
				area = area + arrayX[ptrLeft];
				leftDelta = area - last_area;

				while(leftDelta > 10 && ptrLeft > 0) {
					area = area + arrayX[ptrLeft];
					leftDelta = area - last_area;
					last_area = area;

					dist++;

					ptrLeft = thresX - dist;
				}

				dist = 1; //reset of the variable
				area = arrayY[thresY];
				last_area = area;
				ptrUp = thresY + dist;
				area = area + arrayY[ptrUp];
				upDelta = area - last_area;

				while(upDelta > 20 && ptrUp < height) {
					area = area + arrayY[ptrUp];
					upDelta = area - last_area;
					last_area = area;

					dist++;

					ptrUp = thresY + dist;
				}

				dist = 1; //reset of the variable
				area = arrayY[thresY];
				last_area = area;
				ptrDown = thresY - dist;
				area = area + arrayY[ptrDown];
				downDelta = area - last_area;

				while(downDelta > 20 && ptrDown > 0) {
					area = area + arrayY[ptrDown];
					downDelta = area - last_area;
					last_area = area;

					dist++;

					ptrDown = thresY - dist;
				}

				/*GETTING THE OBJECT*/

				m = img.MIplImage;
				dataPtr = (byte*)m.imageData.ToPointer(); // obter apontador do inicio da imagem

				//Image<Bgr, byte> img2 = img.Copy(new System.Drawing.Rectangle(ptrLeft, ptrDown, (ptrRight - ptrLeft), (ptrUp - ptrDown)));

				if(nChan == 3) { // imagem em RGB
					for(y = 0; y < height; y++) {
						for(x = 0; x < width; x++) {

							if((x >= ptrLeft && y == ptrDown && x <= ptrRight) || (x == ptrRight && y <= ptrUp && y >= ptrDown) || (y == ptrUp && x >= ptrLeft && x <= ptrRight) || (x == ptrLeft && y <= ptrUp && y >= ptrDown)) {

								// guarda na imagem
								dataPtrOriginal[0] = 255; //blue
								dataPtrOriginal[1] = 255; //green
								dataPtrOriginal[2] = 0; //red

								// guarda na imagem
								dataPtr[0] = 255; //blue
								dataPtr[1] = 255; //green
								dataPtr[2] = 0; //red
							}

							// avança apontador para próximo pixel
							dataPtrOriginal += nChan;
							dataPtr += nChan;
						}

						//no fim da linha avança alinhamento (padding)
						dataPtrOriginal += padding;
						dataPtr += padding;
					}
				}
			}
		}

		/// <summary>
		/// Detecção de Vermelho
		/// Manipulação Imagem - Acesso directo à memoria
		/// </summary>
		/// <param name="imgGet">imagem</param>
		/// <param name="MF">MainFrom</param>
		internal static int[] RedDetection(Image<Bgr, byte> imgGet, MainForm MF) {
			unsafe
			{
				// acesso directo à memoria da imagem (sequencial)
				// direcção top left -> bottom right

				//vai buscar e guarda uma copia da imagem original
				Image<Bgr, byte> imgOriginal = null;
				imgOriginal = imgGet.Copy();

				MIplImage m = imgOriginal.MIplImage;
				byte* dataPtrOriginal = (byte*)m.imageData.ToPointer(); // obter apontador do inicio da imagem

				//vai buscar e guarda uma copia da imagem original para ser alterada e analizada
				Image<Bgr, byte> img = null;
				img = imgGet.Copy();

				MIplImage source = img.MIplImage;
				byte* dataPtr = (byte*)source.imageData.ToPointer(); // obter apontador do inicio da imagem

				int width = img.Width;
				int height = img.Height;
				int nChan = m.nChannels; // numero de canais 3
				int padding = m.widthStep - m.nChannels * m.width; // alinhamento (padding)
				int x, y, i;
				int thickness = 2;

				//diferença entre uma componente de cor e outra
				float diff = 0.15f; //%

				if(nChan == 3) { // imagem em RGB
					for(y = 0; y < height; y++) {
						for(x = 0; x < width; x++) {

							//verifica se é vermelho (desejado)
							if((((float)dataPtr[1] / 255 <= (float)dataPtr[2] / 255 - diff) && (((float)dataPtr[0] / 255 <= (float)dataPtr[2] / 255 - diff)))) {

								// guarda na imagem
								dataPtr[0] = 255; //blue
								dataPtr[1] = 255; //green
								dataPtr[2] = 255; //red
							} else {
								// guarda na imagem
								dataPtr[0] = 0; //blue
								dataPtr[1] = 0; //green
								dataPtr[2] = 0; //red
							}

							// avança apontador para próximo pixel
							dataPtr += nChan;
						}

						//no fim da linha avança alinhamento (padding)
						dataPtr += padding;
					}
				}

				//histograma das colunas
				int[] arrayX = HistogramXY(img, 'x');

				List<int> objectsX1X2 = new List<int>();
				List<int> objectsY1Y2 = new List<int>();
				List<int> areaX = new List<int>();
				int same_object = 0;
				int area = 0, x1 = 0, x2 = 0, y1 = 0, y2 = 0;

				//1º NIVEL DE SEGMENTAÇÃO

				//procura tudo o que podemos considerar objecto
				for(i = 0; i < width; i++) {
					if(arrayX[i] != 0 && same_object == 0) {
						objectsX1X2.Add(i); //inicial
						same_object = 1;
					} else if(arrayX[i] == 0 && same_object == 1) {
						objectsX1X2.Add(i - 1); //final
						same_object = 0;
					}
				}

				int[] arrayArea = objectsX1X2.ToArray();

				//procura o maior objecto
				for(i = 0; i < arrayArea.Length - 1; i = i + 2) {
					if(area < arrayArea[i + 1] - arrayArea[i]) {
						area = arrayArea[i + 1] - arrayArea[i];
						x1 = arrayArea[i];
						x2 = arrayArea[i + 1];
					}
				}

				//histograma das linhas entre limitadas verticalmente por x1 e x2
				int[] arrayY = HistogramCustom(img, 'y', x1, x2);
				same_object = 0;

				//procura tudo o que podemos considerar objecto
				for(i = 0; i < arrayY.Length; i++) {
					if(arrayY[i] != 0 && same_object == 0) {
						objectsY1Y2.Add(i); //inicial
						same_object = 1;
					} else if(arrayY[i] == 0 && same_object == 1) {
						objectsY1Y2.Add(i - 1); //final
						same_object = 0;
					}
				}

				arrayArea = objectsY1Y2.ToArray();
				area = 0;

				//procura o maior objecto
				for(i = 0; i < arrayArea.Length - 1; i = i + 2) {
					if(area < arrayArea[i + 1] - arrayArea[i]) {
						area = arrayArea[i + 1] - arrayArea[i];
						y1 = arrayArea[i];
						y2 = arrayArea[i + 1];
					}
				}

				//2º NIVEL DE SEGMENTAÇÃO

				objectsX1X2 = new List<int>();
				same_object = 0;

				//histograma das colunas entre limitadas horizontalmente por y1 e y2
				arrayX = HistogramCustom(img, 'x', y1, y2);

				//procura tudo o que podemos considerar objecto
				for(i = 0; i < arrayX.Length; i++) {
					if(arrayX[i] != 0 && same_object == 0) {
						objectsX1X2.Add(i); //inicial
						same_object = 1;
					} else if(arrayX[i] == 0 && same_object == 1) {
						objectsX1X2.Add(i - 1); //final
						same_object = 0;
					}
				}

				arrayArea = objectsX1X2.ToArray();
				area = 0;

				//procura o maior objecto
				for(i = 0; i < arrayArea.Length - 1; i = i + 2) {
					if(area < arrayArea[i + 1] - arrayArea[i]) {
						area = arrayArea[i + 1] - arrayArea[i];
						x2 = arrayArea[i + 1];
						x1 = arrayArea[i];
					}
				}

				//histograma das linhas entre limitadas verticalmente por x1 e x2
				arrayY = HistogramCustom(img, 'y', x1, x2);
				same_object = 0;

				//procura tudo o que podemos considerar objecto
				for(i = 0; i < arrayY.Length; i++) {
					if(arrayY[i] != 0 && same_object == 0) {
						objectsY1Y2.Add(i); //inicial
						same_object = 1;
					} else if(arrayY[i] == 0 && same_object == 1) {
						objectsY1Y2.Add(i - 1); //final
						same_object = 0;
					}
				}

				arrayArea = objectsY1Y2.ToArray();
				area = 0;

				//procura o maior objecto
				for(i = 0; i < arrayArea.Length - 1; i = i + 2) {
					if(area < arrayArea[i + 1] - arrayArea[i]) {
						area = arrayArea[i + 1] - arrayArea[i];
						y1 = arrayArea[i];
						y2 = arrayArea[i + 1];
					}
				}

				//desenho do quadrado à volta do sinal
				if(nChan == 3) { // imagem em RGB
					for(y = 0; y < height; y++) {
						for(x = 0; x < width; x++) {

							if((x >= x1 - thickness && y >= y2 && y <= y2 + thickness && x <= x2 + thickness) ||
								(x >= x2 && x <= x2 + thickness && y <= y2 && y >= y1) ||
								(y >= y1 - thickness && y <= y1 && x >= x1 - thickness && x <= x2 + thickness) ||
								(x >= x1 - thickness && x <= x1 && y <= y2 && y >= y1)) {

								// guarda na imagem
								dataPtrOriginal[0] = 255; //blue
								dataPtrOriginal[1] = 255; //green
								dataPtrOriginal[2] = 0; //red
							}

							// avança apontador para próximo pixel
							dataPtrOriginal += nChan;
						}

						//no fim da linha avança alinhamento (padding)
						dataPtrOriginal += padding;
					}
				}
				MF.ImageViewer.Image = imgOriginal;
				int[] image = new int[] { x1, x2, y1, y2 };
				return image;
			}
		}

		/// <summary>
		/// Detecção de sinal de trânsito vermelhos
		/// Manipulação Imagem - Acesso directo à memoria
		/// </summary>
		/// <param name="img">imagem</param>
		/// <param name="MF">MainFrom</param>
		/// <param name="DataBase">Base de dados de imagens</param>
		/// <param name="NamesDataBase">Nomes referentes aos sinais de trânsito</param>
		internal static void RedSignDetection(Image<Bgr, byte> img, MainForm MF, List<Image<Bgr, byte>> DataBase, List<string> NamesDataBase) {
			unsafe
			{
				// acesso directo à memoria da imagem (sequencial)
				// direcção top left -> bottom right

				//definição de um tamanho standard
				int width = 111;
				int height = 111;

				int x, y, i = 0;
				int nChan;
				int padding;

				//coordenadas do sinal
				int[] coordenates = RedDetection(img, MF);

				MIplImage a = img.MIplImage;

				//diferença entre uma componente de cor e outra
				float diff = 0.15f; //%
									//factor de escala do sinal em relação à imagem
				int factor = 20;

				//verificação se alguma coisa foi encontrada
				if((coordenates[0] != 0 || coordenates[1] != 0 || coordenates[2] != 0 || coordenates[3] != 0) && (coordenates[1] - coordenates[0]) > a.width / factor && (coordenates[3] - coordenates[2]) > a.height / factor) {

					//copiar só o sinal da imagem original
					Image<Bgr, byte> imgSelected = img.Copy(new System.Drawing.Rectangle(coordenates[0], coordenates[2], (coordenates[1] - coordenates[0]), (coordenates[3] - coordenates[2])));

					//redimensionar para o tamanho standard
					Image<Bgr, byte> imgResized = imgSelected.Resize(width, height, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

					//apontador para a nova imagem redimensionada
					MIplImage m = imgResized.MIplImage;
					byte* dataPtr = (byte*)m.imageData.ToPointer(); // obter apontador do inicio da imagem

					nChan = m.nChannels; // numero de canais 3
					padding = m.widthStep - m.nChannels * m.width; // alinhamento (padding)

					//tudo o que estiver fora do sinal, é retirado (do lado esquerdo)
					if(nChan == 3) { // imagem em RGB
						for(y = 0; y < height; y++) {
							for(x = 0; x < width; x++) {

								//se vermelho, avança para a linha seguinte
								if((((float)dataPtr[1] / 255 <= (float)dataPtr[2] / 255 - diff) && (((float)dataPtr[0] / 255 <= (float)dataPtr[2] / 255 - diff)))) {
									dataPtr = dataPtr + (width - x) * nChan;
									break;
								}

								dataPtr[2] = 255;
								dataPtr[1] = 255;
								dataPtr[0] = 255;

								// avança apontador para próximo pixel
								dataPtr += nChan;
							}
							//no fim da linha avança alinhamento (padding)
							dataPtr += padding;
						}
					}

					//reinicializar o apontador
					dataPtr = (byte*)m.imageData.ToPointer(); // obter apontador do inicio da imagem

					byte* auxPtr;

					//tudo o que estiver fora do sinal, é retirado (do lado direito)
					for(y = 0; y < height; y++) {
						for(x = width - 1; x >= 0; x--) {

							auxPtr = (dataPtr + y * m.widthStep + x * nChan);

							//se vermelho, avança para a linha seguinte
							if((((float)auxPtr[1] / 255 <= (float)auxPtr[2] / 255 - diff) && (((float)auxPtr[0] / 255 <= (float)auxPtr[2] / 255 - diff)))) {
								break;
							}

							auxPtr[2] = 255;
							auxPtr[1] = 255;
							auxPtr[0] = 255;
						}
					}

					//converte qualquer vermelho para vermelho puro e qualquer "preto" para preto puro
					if(nChan == 3) { // imagem em RGB
						for(y = 0; y < height; y++) {
							for(x = 0; x < width; x++) {

								//se for vermelho
								if((((float)dataPtr[1] / 255 <= (float)dataPtr[2] / 255 - diff) && (((float)dataPtr[0] / 255 <= (float)dataPtr[2] / 255 - diff)))) {
									dataPtr[2] = 255;
									dataPtr[1] = 0;
									dataPtr[0] = 0;
								} else {
									//se for preto
									if((float)dataPtr[0] / 255 <= 0.35 && (float)dataPtr[1] / 255 <= 0.35 && (float)dataPtr[2] / 255 <= 0.40) {
										dataPtr[2] = 0;
										dataPtr[1] = 0;
										dataPtr[0] = 0;
									} else {
										dataPtr[2] = 255;
										dataPtr[1] = 255;
										dataPtr[0] = 255;
									}
								}

								// avança apontador para próximo pixel
								dataPtr += nChan;
							}
							//no fim da linha avança alinhamento (padding)
							dataPtr += padding;
						}
					}

					int total = 0;
					int nameIndex = 0;
					float total_percentage;
					float percentage = 0;
					Image<Bgr, byte> imgShow = null;
					Image<Bgr, byte> temp = null;

					//amount of pixels
					int pixel_area = width * height;

					//por cada imagem na base de dados
					foreach(Image<Bgr, byte> imgDBGet in DataBase) {

						//redimensionar a imagem para o tamanho standard
						Image<Bgr, byte> imgDB = imgDBGet.Resize(width, height, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

						//obter uma copia da imagem para depois ser mostrada
						temp = imgDBGet.Copy();

						//criar apontador para a imagem
						MIplImage n = imgDB.MIplImage;
						byte* dataPtrDB = (byte*)n.imageData.ToPointer(); // obter apontador do inicio da imagem

						//reinicializar o apontador para a imagem a analisar
						m = imgResized.MIplImage;
						dataPtr = (byte*)m.imageData.ToPointer(); // obter apontador do inicio da imagem

						nChan = n.nChannels; // numero de canais 3
						padding = n.widthStep - n.nChannels * n.width; // alinhamento (padding)

						//converte qualquer vermelho para vermelho puro e qualquer "preto" para preto puro
						if(nChan == 3) { // imagem em RGB
							for(y = 0; y < height; y++) {
								for(x = 0; x < width; x++) {

									//se for vermelho
									if((((float)dataPtrDB[1] / 255 <= (float)dataPtrDB[2] / 255 - diff) && (((float)dataPtrDB[0] / 255 <= (float)dataPtrDB[2] / 255 - diff)))) {
										dataPtrDB[2] = 255;
										dataPtrDB[1] = 0;
										dataPtrDB[0] = 0;
									} else {
										//se for preto
										if((float)dataPtrDB[0] / 255 <= 0.15 && (float)dataPtrDB[1] / 255 <= 0.15 && (float)dataPtrDB[2] / 255 <= 0.15) {
											dataPtrDB[2] = 0;
											dataPtrDB[1] = 0;
											dataPtrDB[0] = 0;
										} else {
											dataPtrDB[2] = 255;
											dataPtrDB[1] = 255;
											dataPtrDB[0] = 255;
										}
									}

									// avança apontador para próximo pixel
									dataPtrDB += nChan;
								}
								//no fim da linha avança alinhamento (padding)
								dataPtrDB += padding;
							}
						}

						//reinicializar o apontador da imagem da base de dados
						dataPtrDB = (byte*)n.imageData.ToPointer(); // obter apontador do inicio da imagem

						//compara pixel a pixel para determinar a "igualdade" das imagens
						if(nChan == 3) { // imagem em RGB
							for(y = 0; y < height; y++) {
								for(x = 0; x < width; x++) {

									total = ((dataPtr[0] == dataPtrDB[0] && dataPtr[1] == dataPtrDB[1] && dataPtr[2] == dataPtrDB[2]) ? total + 1 : total);

									// avança apontador para próximo pixel
									dataPtr += nChan;
									dataPtrDB += nChan;
								}

								//no fim da linha avança alinhamento (padding)
								dataPtr += padding;
								dataPtrDB += padding;
							}
						}

						total_percentage = ((float)total / pixel_area);
						total = 0;

						//guarda o sinal com maior percentagem
						if(total_percentage > percentage) {
							percentage = total_percentage;
							nameIndex = i;
							imgShow = temp.Resize(width, height, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
						}
						i++;
					}

					//apresenta o caso mais provavel do sinal
					Identified form = new Identified();
					form.Probability.Text = percentage.ToString("p2");
					form.NameSign.Text = NamesDataBase[nameIndex].ToString();
					form.IdentifiedViewer.Image = imgShow.Bitmap;
					form.ShowDialog();
				} else {
					ImageNotFound form = new ImageNotFound();
					form.ShowDialog();
				}
			}
		}

		/// <summary>
		/// Detecção de Azul
		/// Manipulação Imagem - Acesso directo à memoria
		/// </summary>
		/// <param name="imgGet">imagem</param>
		/// <param name="MF">MainFrom</param>
		internal static int[] BlueDetection(Image<Bgr, byte> imgGet, MainForm MF) {
			unsafe
			{
				// acesso directo à memoria da imagem (sequencial)
				// direcção top left -> bottom right

				//vai buscar e guarda uma copia da imagem original
				Image<Bgr, byte> imgOriginal = null;
				imgOriginal = imgGet.Copy();

				MIplImage m = imgOriginal.MIplImage;
				byte* dataPtrOriginal = (byte*)m.imageData.ToPointer(); // obter apontador do inicio da imagem

				//vai buscar e guarda uma copia da imagem original para ser alterada e analizada
				Image<Bgr, byte> img = null;
				img = imgGet.Copy();

				MIplImage source = img.MIplImage;
				byte* dataPtr = (byte*)source.imageData.ToPointer(); // obter apontador do inicio da imagem

				int width = img.Width;
				int height = img.Height;
				int nChan = m.nChannels; // numero de canais 3
				int padding = m.widthStep - m.nChannels * m.width; // alinhamento (padding)
				int x, y, i;
				int thickness = 2;

				//diferença entre azul e verde
				float diff = 0.22f; //%
									//diferença entre verde e vermelho
				float diff2 = 0.1f; //%

				if(nChan == 3) { // imagem em RGB
					for(y = 0; y < height; y++) {
						for(x = 0; x < width; x++) {

							//verifica se é azul (desejado)
							if((((float)dataPtr[1] / 255 <= (float)dataPtr[0] / 255 - diff) && (((float)dataPtr[2] / 255 <= (float)dataPtr[1] / 255 - diff2)))) {

								// guarda na imagem
								dataPtr[0] = 255; //blue
								dataPtr[1] = 255; //green
								dataPtr[2] = 255; //red
							} else {
								// guarda na imagem
								dataPtr[0] = 0; //blue
								dataPtr[1] = 0; //green
								dataPtr[2] = 0; //red
							}

							// avança apontador para próximo pixel
							dataPtr += nChan;
						}

						//no fim da linha avança alinhamento (padding)
						dataPtr += padding;
					}
				}

				//histograma das colunas
				int[] arrayX = HistogramXY(img, 'x');

				List<int> objectsX1X2 = new List<int>();
				List<int> objectsY1Y2 = new List<int>();
				List<int> areaX = new List<int>();
				int same_object = 0;
				int area = 0, x1 = 0, x2 = 0, y1 = 0, y2 = 0;

				//1º NIVEL DE SEGMENTAÇÃO

				//procura tudo o que podemos considerar objecto
				for(i = 0; i < width; i++) {
					if(arrayX[i] != 0 && same_object == 0) {
						objectsX1X2.Add(i); //inicial
						same_object = 1;
					} else if(arrayX[i] == 0 && same_object == 1) {
						objectsX1X2.Add(i - 1); //final
						same_object = 0;
					}
				}

				int[] arrayArea = objectsX1X2.ToArray();

				//procura o maior objecto
				for(i = 0; i < arrayArea.Length - 1; i = i + 2) {
					if(area < arrayArea[i + 1] - arrayArea[i]) {
						area = arrayArea[i + 1] - arrayArea[i];
						x1 = arrayArea[i];
						x2 = arrayArea[i + 1];
					}
				}

				//histograma das linhas entre limitadas verticalmente por x1 e x2
				int[] arrayY = HistogramCustom(img, 'y', x1, x2);
				same_object = 0;

				//procura tudo o que podemos considerar objecto
				for(i = 0; i < arrayY.Length; i++) {
					if(arrayY[i] != 0 && same_object == 0) {
						objectsY1Y2.Add(i); //inicial
						same_object = 1;
					} else if(arrayY[i] == 0 && same_object == 1) {
						objectsY1Y2.Add(i - 1); //final
						same_object = 0;
					}
				}

				arrayArea = objectsY1Y2.ToArray();
				area = 0;

				//procura o maior objecto
				for(i = 0; i < arrayArea.Length - 1; i = i + 2) {
					if(area < arrayArea[i + 1] - arrayArea[i]) {
						area = arrayArea[i + 1] - arrayArea[i];
						y1 = arrayArea[i];
						y2 = arrayArea[i + 1];
					}
				}

				//2º NIVEL DE SEGMENTAÇÃO

				objectsX1X2 = new List<int>();
				same_object = 0;

				//histograma das colunas entre limitadas horizontalmente por y1 e y2
				arrayX = HistogramCustom(img, 'x', y1, y2);

				//procura tudo o que podemos considerar objecto
				for(i = 0; i < arrayX.Length; i++) {
					if(arrayX[i] != 0 && same_object == 0) {
						objectsX1X2.Add(i); //inicial
						same_object = 1;
					} else if(arrayX[i] == 0 && same_object == 1) {
						objectsX1X2.Add(i - 1); //final
						same_object = 0;
					}
				}

				arrayArea = objectsX1X2.ToArray();
				area = 0;

				//procura o maior objecto
				for(i = 0; i < arrayArea.Length - 1; i = i + 2) {
					if(area < arrayArea[i + 1] - arrayArea[i]) {
						area = arrayArea[i + 1] - arrayArea[i];
						x2 = arrayArea[i + 1];
						x1 = arrayArea[i];
					}
				}

				//histograma das linhas entre limitadas verticalmente por x1 e x2
				arrayY = HistogramCustom(img, 'y', x1, x2);
				same_object = 0;

				//procura tudo o que podemos considerar objecto
				for(i = 0; i < arrayY.Length; i++) {
					if(arrayY[i] != 0 && same_object == 0) {
						objectsY1Y2.Add(i); //inicial
						same_object = 1;
					} else if(arrayY[i] == 0 && same_object == 1) {
						objectsY1Y2.Add(i - 1); //final
						same_object = 0;
					}
				}

				arrayArea = objectsY1Y2.ToArray();
				area = 0;

				//procura o maior objecto
				for(i = 0; i < arrayArea.Length - 1; i = i + 2) {
					if(area < arrayArea[i + 1] - arrayArea[i]) {
						area = arrayArea[i + 1] - arrayArea[i];
						y1 = arrayArea[i];
						y2 = arrayArea[i + 1];
					}
				}

				//desenho do quadrado à volta do sinal
				if(nChan == 3) { // imagem em RGB
					for(y = 0; y < height; y++) {
						for(x = 0; x < width; x++) {

							if((x >= x1 - thickness && y >= y2 && y <= y2 + thickness && x <= x2 + thickness) ||
								(x >= x2 && x <= x2 + thickness && y <= y2 && y >= y1) ||
								(y >= y1 - thickness && y <= y1 && x >= x1 - thickness && x <= x2 + thickness) ||
								(x >= x1 - thickness && x <= x1 && y <= y2 && y >= y1)) {

								// guarda na imagem
								dataPtrOriginal[0] = 255; //blue
								dataPtrOriginal[1] = 255; //green
								dataPtrOriginal[2] = 0; //red
							}

							// avança apontador para próximo pixel
							dataPtrOriginal += nChan;
						}

						//no fim da linha avança alinhamento (padding)
						dataPtrOriginal += padding;
					}
				}
				MF.ImageViewer.Image = imgOriginal;
				int[] image = new int[] { x1, x2, y1, y2 };
				return image;
			}
		}

		/// <summary>
		/// Detecção de sinal de trânsito azuis
		/// Manipulação Imagem - Acesso directo à memoria
		/// </summary>
		/// <param name="img">imagem</param>
		/// <param name="MF">MainFrom</param>
		/// <param name="DataBase">Base de dados de imagens</param>
		/// <param name="NamesDataBase">Nomes referentes aos sinais de trânsito</param>
		internal static void BlueSignDetection(Image<Bgr, byte> img, MainForm MF, List<Image<Bgr, byte>> DataBase, List<string> NamesDataBase) {
			unsafe
			{
				// acesso directo à memoria da imagem (sequencial)
				// direcção top left -> bottom right

				//definição de um tamanho standard
				int width = 111;
				int height = 111;

				int x, y, i = 0;
				int nChan;
				int padding;

				//coordenadas do sinal
				int[] coordenates = BlueDetection(img, MF);

				MIplImage a = img.MIplImage;

				//diferença entre azul e verde
				float diff = 0.2f;
				//diferença entre verde e vermelho
				float diff2 = 0.1f;
				//factor de escala do sinal em relação à imagem
				int factor = 20;

				//verificação se alguma coisa foi encontrada
				if((coordenates[0] != 0 || coordenates[1] != 0 || coordenates[2] != 0 || coordenates[3] != 0) && (coordenates[1] - coordenates[0]) > a.width / factor && (coordenates[3] - coordenates[2]) > a.height / factor) {

					//copiar só o sinal da imagem original
					Image<Bgr, byte> imgSelected = img.Copy(new System.Drawing.Rectangle(coordenates[0], coordenates[2], (coordenates[1] - coordenates[0]), (coordenates[3] - coordenates[2])));

					//redimensionar para o tamanho standard
					Image<Bgr, byte> imgResized = imgSelected.Resize(width, height, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

					//apontador para a nova imagem redimensionada
					MIplImage m = imgResized.MIplImage;
					byte* dataPtr = (byte*)m.imageData.ToPointer(); // obter apontador do inicio da imagem

					nChan = m.nChannels; // numero de canais 3
					padding = m.widthStep - m.nChannels * m.width; // alinhamento (padding)

					//tudo o que estiver fora do sinal, é retirado (do lado esquerdo)
					if(nChan == 3) { // imagem em RGB
						for(y = 0; y < height; y++) {
							for(x = 0; x < width; x++) {

								//se azul, avança para a linha seguinte
								if((((float)dataPtr[1] / 255 <= (float)dataPtr[0] / 255 - diff) && (((float)dataPtr[2] / 255 <= (float)dataPtr[1] / 255 - diff2)))) {
									dataPtr = dataPtr + (width - x) * nChan;
									break;
								}

								dataPtr[2] = 255;
								dataPtr[1] = 255;
								dataPtr[0] = 255;

								// avança apontador para próximo pixel
								dataPtr += nChan;
							}
							//no fim da linha avança alinhamento (padding)
							dataPtr += padding;
						}
					}

					//reinicializar o apontador
					dataPtr = (byte*)m.imageData.ToPointer(); // obter apontador do inicio da imagem

					byte* auxPtr;

					//tudo o que estiver fora do sinal, é retirado (do lado direito)
					for(y = 0; y < height; y++) {
						for(x = width - 1; x >= 0; x--) {

							auxPtr = (dataPtr + y * m.widthStep + x * nChan);

							if((((float)auxPtr[1] / 255 <= (float)auxPtr[0] / 255 - diff) && (((float)auxPtr[2] / 255 <= (float)auxPtr[1] / 255 - diff2)))) {
								break;
							}

							auxPtr[2] = 255;
							auxPtr[1] = 255;
							auxPtr[0] = 255;
						}
					}

					//converte qualquer azul para azul puro e qualquer "preto" para preto puro
					if(nChan == 3) { // imagem em RGB
						for(y = 0; y < height; y++) {
							for(x = 0; x < width; x++) {

								if((((float)dataPtr[1] / 255 <= (float)dataPtr[0] / 255 - diff) && (((float)dataPtr[2] / 255 <= (float)dataPtr[1] / 255 - diff2)))) {
									dataPtr[2] = 0;
									dataPtr[1] = 0;
									dataPtr[0] = 255;
								} else {
									if((float)dataPtr[0] / 255 <= 0.15 && (float)dataPtr[1] / 255 <= 0.15 && (float)dataPtr[2] / 255 <= 0.15) {
										dataPtr[2] = 0;
										dataPtr[1] = 0;
										dataPtr[0] = 0;
									} else {
										dataPtr[2] = 255;
										dataPtr[1] = 255;
										dataPtr[0] = 255;
									}
								}

								// avança apontador para próximo pixel
								dataPtr += nChan;
							}
							//no fim da linha avança alinhamento (padding)
							dataPtr += padding;
						}
					}

					int total = 0;
					int nameIndex = 0;
					float total_percentage;
					float percentage = 0;
					Image<Bgr, byte> imgShow = null;
					Image<Bgr, byte> temp = null;

					//amount of pixels
					int pixel_area = width * height;

					//por cada imagem na base de dados
					foreach(Image<Bgr, byte> imgDBGet in DataBase) {

						//redimensionar a imagem para o tamanho standard
						Image<Bgr, byte> imgDB = imgDBGet.Resize(width, height, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

						//obter uma copia da imagem para depois ser mostrada
						temp = imgDBGet.Copy();

						//criar apontador para a imagem
						MIplImage n = imgDB.MIplImage;
						byte* dataPtrDB = (byte*)n.imageData.ToPointer(); // obter apontador do inicio da imagem

						//reinicializar o apontador para a imagem a analisar
						m = imgResized.MIplImage;
						dataPtr = (byte*)m.imageData.ToPointer(); // obter apontador do inicio da imagem

						nChan = n.nChannels; // numero de canais 3
						padding = n.widthStep - n.nChannels * n.width; // alinhamento (padding)

						//converte qualquer azul para azul puro e qualquer "preto" para preto puro
						if(nChan == 3) { // imagem em RGB
							for(y = 0; y < height; y++) {
								for(x = 0; x < width; x++) {

									//se for azul
									if((((float)dataPtrDB[1] / 255 <= (float)dataPtrDB[0] / 255 - diff) && (((float)dataPtrDB[2] / 255 <= (float)dataPtrDB[1] / 255 - diff2)))) {
										dataPtrDB[2] = 0;
										dataPtrDB[1] = 0;
										dataPtrDB[0] = 255;
									} else {
										//se for preto
										if((float)dataPtrDB[0] / 255 <= 0.1 && (float)dataPtrDB[1] / 255 <= 0.1 && (float)dataPtrDB[2] / 255 <= 0.1) {
											dataPtrDB[2] = 0;
											dataPtrDB[1] = 0;
											dataPtrDB[0] = 0;
										} else {
											dataPtrDB[2] = 255;
											dataPtrDB[1] = 255;
											dataPtrDB[0] = 255;
										}
									}

									// avança apontador para próximo pixel
									dataPtrDB += nChan;
								}
								//no fim da linha avança alinhamento (padding)
								dataPtrDB += padding;
							}
						}

						//reinicializar o apontador da imagem da base de dados
						dataPtrDB = (byte*)n.imageData.ToPointer(); // obter apontador do inicio da imagem

						//compara pixel a pixel para determinar a "igualdade" das imagens
						if(nChan == 3) { // imagem em RGB
							for(y = 0; y < height; y++) {
								for(x = 0; x < width; x++) {

									total = ((dataPtr[0] == dataPtrDB[0] && dataPtr[1] == dataPtrDB[1] && dataPtr[2] == dataPtrDB[2]) ? total + 1 : total);

									// avança apontador para próximo pixel
									dataPtr += nChan;
									dataPtrDB += nChan;
								}

								//no fim da linha avança alinhamento (padding)
								dataPtr += padding;
								dataPtrDB += padding;
							}
						}
						total_percentage = ((float)total / pixel_area);
						total = 0;

						//guarda o sinal com maior percentagem
						if(total_percentage > percentage) {
							percentage = total_percentage;
							nameIndex = i;
							imgShow = temp.Resize(width, height, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
						}
						i++;
					}

					//apresenta o caso mais provavel do sinal
					Identified form = new Identified();
					form.Probability.Text = percentage.ToString("p2");
					form.NameSign.Text = NamesDataBase[nameIndex].ToString();
					form.IdentifiedViewer.Image = imgShow.Bitmap;
					form.ShowDialog();
				} else {
					ImageNotFound form = new ImageNotFound();
					form.ShowDialog();
				}
			}
		}

		/// <summary>
		/// Detecta a entropia da imagem
		/// Manipulação Imagem - Acesso directo à memoria
		/// </summary>
		/// <param name="img">imagem</param>
		internal static double[] Entropy(Image<Bgr, byte> img) {
			unsafe
			{
				// acesso directo à memoria da imagem (sequencial)
				// direcção top left -> bottom right

				MIplImage m = img.MIplImage;
				byte* dataPtr = (byte*)m.imageData.ToPointer(); // obter apontador do inicio da imagem

				int width = img.Width;
				int height = img.Height;
				int nChan = m.nChannels; // numero de canais 3
				int padding = m.widthStep - m.nChannels * m.width; // alinhamento (padding)
				int x, y, gray, i, n;

				//probability array
				float[] arrayProbGray = new float[256];
				float[] arrayProbBlue = new float[256];
				float[] arrayProbGreen = new float[256];
				float[] arrayProbRed = new float[256];

				//amount of pixels
				int pixel_area = width * height;

				int[] arrayGray = new int[256];
				int[] arrayB = new int[256];
				int[] arrayG = new int[256];
				int[] arrayR = new int[256];

				double[] entropy = new double[4];

				if(nChan == 3) { // imagem em RGB
					for(y = 0; y < height; y++) {
						for(x = 0; x < width; x++) {

							gray = (dataPtr[0] + dataPtr[1] + dataPtr[2]) / 3;

							arrayGray[gray]++;
							arrayB[dataPtr[0]]++;
							arrayG[dataPtr[1]]++;
							arrayR[dataPtr[2]]++;

							// avança apontador para próximo pixel
							dataPtr += nChan;
						}

						//no fim da linha avança alinhamento (padding)
						dataPtr += padding;
					}
				}

				//producing array of probabilities
				for(i = 0; i < 256; i++) {
					arrayProbGray[i] = (float)arrayGray[i] / pixel_area;
					arrayProbBlue[i] = (float)arrayB[i] / pixel_area;
					arrayProbGreen[i] = (float)arrayG[i] / pixel_area;
					arrayProbRed[i] = (float)arrayR[i] / pixel_area;
				}

				//algorithm
				for(n = 0; n < 256; n++) {
					entropy[0] += (arrayProbGray[n] != 0 ? arrayProbGray[n] * (Math.Log(arrayProbGray[n]) / Math.Log(2)) : 0);
					entropy[1] += (arrayProbBlue[n] != 0 ? arrayProbBlue[n] * (Math.Log(arrayProbBlue[n]) / Math.Log(2)) : 0);
					entropy[2] += (arrayProbGreen[n] != 0 ? arrayProbGreen[n] * (Math.Log(arrayProbGreen[n]) / Math.Log(2)) : 0);
					entropy[3] += (arrayProbRed[n] != 0 ? arrayProbRed[n] * (Math.Log(arrayProbRed[n]) / Math.Log(2)) : 0);
				}

				entropy[0] = -1 * entropy[0];
				entropy[1] = -1 * entropy[1];
				entropy[2] = -1 * entropy[2];
				entropy[3] = -1 * entropy[3];

				return entropy;
			}
		}

		/// <summary>
		/// Arredonda o bloco da imagem inserido
		/// Manipulação Imagem - Acesso directo à memoria
		/// </summary>
		/// <param name="img">imagem</param>
		internal static void RoundBlock(Image<Gray, float> img) {
			unsafe
			{
				// acesso directo à memoria da imagem (sequencial)
				// top left to bottom right

				MIplImage m = img.MIplImage;
				int start = m.imageData.ToInt32();
				float* dataPtr = (float*)start;
				int h = img.Height;
				int w = img.Width;
				int x, y;
				int nC = m.nChannels;
				int wStep = m.widthStep - m.nChannels * m.width * sizeof(float);

				for(y = 0; y < h; y++) {
					for(x = 0; x < w; x++) {
						// converte BGR para cinza 
						*dataPtr = (float)Math.Round((double)*dataPtr);
						// avança apontador 
						dataPtr++;
					}
					//no fim da linha avança alinhamento
					dataPtr += wStep;
				}
			}
		}

		/// <summary>
		/// Obtem a matriz de quantização da imagem
		/// Manipulação Imagem - Acesso directo à memoria
		/// </summary>
		/// <param name="LuminanceOrChrominance">Luminance or Chrominance</param>
		/// <param name="compfactor">Compression Factor</param>
		internal static Image<Gray, float> GetQuantificationMatrix(bool LuminanceOrChrominance, int compfactor) {

			float[] LumQuant = {   16,11,10,16,24,40,51,61,
								12,12,14,19,26,58,60,55,
								14,13,16,24,40,57,69,56,
								14,17,22,29,51,87,80,62,
								18,22,37,56,68,109,103,77,
								24,35,55,64,81,104,113,92,
								49,64,78,87,103,121,120,101,
								72,92,95,98,112,100,103,99};

			float[] ChrQuant = {17,18,24,47,99,99,99,99,
								18,21,26,66,99,99,99,99,
								24,26,56,99,99,99,99,99,
								47,66,99,99,99,99,99,99,
								99,99,99,99,99,99,99,99,
								99,99,99,99,99,99,99,99,
								99,99,99,99,99,99,99,99,
								99,99,99,99,99,99,99,99
								};



			Image<Gray, float> matrix = new Image<Gray, float>(8, 8);

			int idx = 0;
			float[] Quant = (LuminanceOrChrominance) ? LumQuant : ChrQuant;

			for(int y = 0; y < 8; y++) {
				for(int x = 0; x < 8; x++) {
					matrix[y, x] = new Gray(Quant[idx++] * 100 / compfactor);
				}
			}

			return matrix;
		}

		/// <summary>
		/// Realiza a compressão da imagem para jpeg
		/// Manipulação Imagem - Acesso directo à memoria
		/// </summary>
		/// <param name="img">imagem</param>
		/// <param name="CompressionFactor">Compression Factor</param>
		internal static Image<Bgr, byte> CompressionToJPEG(Image<Bgr, byte> img, int CompressionFactor) {
			unsafe
			{
				// acesso directo à memoria da imagem (sequencial)
				// direcção top left -> bottom right

				MIplImage m = img.MIplImage;
				byte* dataPtr = (byte*)m.imageData.ToPointer(); // obter apontador do inicio da imagem

				int width = img.Width;
				int height = img.Height;
				int nChan = m.nChannels; // numero de canais 3
				int padding = m.widthStep - m.nChannels * m.width; // alinhamento (padding)
				int x, y;

				//conversão para YCbCr
				Image<Ycc, float> YCbCr_img = img.Convert<Ycc, float>();

				Image<Ycc, byte> img8x8_b = new Image<Ycc, byte>(8, 8);

				Image<Ycc, float> blockYCC = new Image<Ycc, float>(8, 8);

				for(y = 0; y < height; y += 8) {
					for(x = 0; x < width; x += 8) {

						//criar bloco de 8x8
						Image<Gray, float> img8x8_Y = YCbCr_img[0].Copy(new System.Drawing.Rectangle(x, y, 8, 8));
						Image<Gray, float> img8x8_Cb = YCbCr_img[1].Copy(new System.Drawing.Rectangle(x, y, 8, 8));
						Image<Gray, float> img8x8_Cr = YCbCr_img[2].Copy(new System.Drawing.Rectangle(x, y, 8, 8));

						//calculo da transformada de cossenos
						CvInvoke.cvDCT(img8x8_Y, img8x8_Y, Emgu.CV.CvEnum.CV_DCT_TYPE.CV_DXT_FORWARD);
						CvInvoke.cvDCT(img8x8_Cb, img8x8_Cb, Emgu.CV.CvEnum.CV_DCT_TYPE.CV_DXT_FORWARD);
						CvInvoke.cvDCT(img8x8_Cr, img8x8_Cr, Emgu.CV.CvEnum.CV_DCT_TYPE.CV_DXT_FORWARD);

						//quantificação dos coeficientes
						CvInvoke.cvDiv(img8x8_Y, GetQuantificationMatrix(true, CompressionFactor), img8x8_Y, 1);
						CvInvoke.cvDiv(img8x8_Cb, GetQuantificationMatrix(false, CompressionFactor), img8x8_Cb, 1);
						CvInvoke.cvDiv(img8x8_Cr, GetQuantificationMatrix(false, CompressionFactor), img8x8_Cr, 1);

						//arredontamento dos queficientes
						RoundBlock(img8x8_Y);
						RoundBlock(img8x8_Cb);
						RoundBlock(img8x8_Cr);

						//recuperaçao dos coeficientes
						CvInvoke.cvMul(img8x8_Y, GetQuantificationMatrix(true, CompressionFactor), img8x8_Y, 1);
						CvInvoke.cvMul(img8x8_Cb, GetQuantificationMatrix(false, CompressionFactor), img8x8_Cb, 1);
						CvInvoke.cvMul(img8x8_Cr, GetQuantificationMatrix(false, CompressionFactor), img8x8_Cr, 1);

						//calculo da transformada inversa de cossenos
						CvInvoke.cvDCT(img8x8_Y, img8x8_Y, Emgu.CV.CvEnum.CV_DCT_TYPE.CV_DXT_INVERSE);
						CvInvoke.cvDCT(img8x8_Cb, img8x8_Cb, Emgu.CV.CvEnum.CV_DCT_TYPE.CV_DXT_INVERSE);
						CvInvoke.cvDCT(img8x8_Cr, img8x8_Cr, Emgu.CV.CvEnum.CV_DCT_TYPE.CV_DXT_INVERSE);

						//copy the processed block to the image
						YCbCr_img.ROI = new Rectangle(x, y, 8, 8);

						// merge individual channels into one single Ycc image
						CvInvoke.cvMerge(img8x8_Y, img8x8_Cb, img8x8_Cr, IntPtr.Zero, blockYCC);

						// copy to final image
						blockYCC.CopyTo(YCbCr_img);
						YCbCr_img.ROI = Rectangle.Empty;
					}
				}
				//Convert image to BGR,byte (Convert< , > )
				Image<Bgr, byte> imgRGB = YCbCr_img.Convert<Bgr, float>().Convert<Bgr, byte>();

				return imgRGB;
			}
		}

		/// <summary>
		/// Compute image connected components 
		/// </summary>
		/// <param name="img"></param>
		/// <returns></returns>
		internal static Image<Gray, int> GetConnectedComponents(Image<Bgr, byte> img) {
			Image<Gray, byte> imgThresh = img.Convert<Gray, byte>();
			CvInvoke.cvThreshold(imgThresh, imgThresh, 0, 255, Emgu.CV.CvEnum.THRESH.CV_THRESH_BINARY | Emgu.CV.CvEnum.THRESH.CV_THRESH_OTSU);

			ShowSingleIMG.ShowIMGStatic(imgThresh);

			Contour<Point> contours = imgThresh.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_NONE, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_CCOMP);
			Image<Gray, int> labelsImg = new Image<Gray, int>(imgThresh.Size);
			int count = 1;

			while(contours != null) {
				labelsImg.Draw(contours, new Gray(count), -1);
				labelsImg.Draw(contours, new Gray(-10), 1);
				contours = contours.HNext;
				count++;
			}

			return labelsImg;
		}

		/// <summary>
		/// Watershed with labels (Meyer)
		/// </summary>
		/// <param name="img"></param>
		/// <param name="labels"></param>
		/// <returns></returns>
		internal static Image<Gray, int> GetWatershedFromLabels(Image<Bgr, byte> img, Image<Gray, byte> labels) {
			Image<Gray, int> watershedAux = labels.Convert<Gray, int>();

			CvInvoke.cvWatershed(img, watershedAux);

			return watershedAux;
		}

		/// <summary>
		/// Watershed By Immersion (Vincent and Soille)
		/// </summary>
		/// <param name="img"></param>
		/// <returns></returns>
		internal static Image<Gray, int> GetWatershedByImmersion(Image<Bgr, byte> img) {
			WatershedGrayscale wt = new WatershedGrayscale();
			Image<Gray, byte> wtImg = img.Convert<Gray, byte>();
			wt.ProcessFilter(wtImg.Not());
			wtImg.SetZero();
			Image<Gray, int> wtOutimg = new Image<Gray, int>(img.Size);
			wt.DrawWatershedLines(wtOutimg);

			return wtOutimg;
		}

		/// <summary>
		/// Get Gradient Path Labelling (GPL) segmnentation 
		/// </summary>
		/// <param name="img">image</param>
		/// <returns></returns>
		internal static Image<Bgr, byte> GetGPL(Image<Bgr, byte> img) {
			GPL_lib.GPL_lib gpl = new GPL_lib.GPL_lib(img, false);

			gpl.ShowConfigForm();

			return gpl.GetImage();
		}

		/// <summary>
		/// Calculate Hough Transform Plane
		/// </summary>
		/// <param name="img"></param>
		/// <param name="angleSpacing">Radians</param>
		/// <param name="minAngle">Radians</param>
		/// <param name="maxAngle">Radians</param>
		/// <returns></returns>
		internal static Image<Gray, float> HoughTransform(Image<Gray, byte> img, float angleSpacing, float minAngle, float maxAngle) {
			int numberAngles = (int)((maxAngle - minAngle) / angleSpacing);
			float angle = 0;
			Image<Gray, byte> workImg = img.Clone();
			Matrix<float> imgHough = null;

			for(float col = 0; col < numberAngles; col++) {
				workImg = img.Rotate(angle, new Gray(0), true);
				angle += angleSpacing;

				Matrix<float> imgMatH = new Matrix<float>(workImg.Height, 1, 1);

				workImg.Reduce<float>(imgMatH, Emgu.CV.CvEnum.REDUCE_DIMENSION.SINGLE_COL, Emgu.CV.CvEnum.REDUCE_TYPE.CV_REDUCE_SUM);
				if(imgHough == null)
					imgHough = imgMatH;
				else
					imgHough = imgHough.ConcateHorizontal(imgMatH);

			}
			Image<Gray, float> houghImg = new Image<Gray, float>(numberAngles, img.Height);
			CvInvoke.cvConvert(imgHough, houghImg);

			return houghImg;
		}

		/// <summary>
		/// Calculates Hough Transform major lines using EmguCV function
		/// </summary>
		/// <param name="imgPreprocessed"></param>
		/// <param name="imgOriginal"></param>
		/// <returns></returns>
		internal static Image<Bgr, byte> ShowHoughLines(Image<Bgr, byte> img, Image<Bgr, byte> imgOriginal, int threshold) {

			// convert to Gray
			Image<Gray, byte> imgGray = img.Convert<Gray, byte>();

			MemStorage SS = new MemStorage();
			IntPtr segmentsPtr = CvInvoke.cvHoughLines2(imgGray, SS.Ptr, Emgu.CV.CvEnum.HOUGH_TYPE.CV_HOUGH_STANDARD, 1, Math.PI / 180, threshold, 0, 0);

			// draw lines
			Seq<PointF> segments = new Seq<PointF>(segmentsPtr, SS);
			PointF[] lines = segments.ToArray();

			Image<Bgr, byte> lineImage = imgOriginal.Copy();

			foreach(PointF line in lines) {
				float rho = line.X;
				float theta = line.Y;
				Point pt1 = new Point(), pt2 = new Point();

				double a = Math.Cos(theta), b = Math.Sin(theta);
				double x0 = a * rho, y0 = b * rho;
				pt1.X = (int)Math.Round(x0 + 1000 * (-b));
				pt1.Y = (int)Math.Round(y0 + 1000 * (a));
				pt2.X = (int)Math.Round(x0 - 1000 * (-b));
				pt2.Y = (int)Math.Round(y0 - 1000 * (a));

				lineImage.Draw(new LineSegment2D(pt1, pt2), new Bgr(255, 0, 0), 1);

			}

			//CvInvoke.cvReleaseImage(ref segmentsPtr);

			return lineImage;
		}

		/// <summary>
		/// Calculates Hough Transform circles using EmguCV function
		/// </summary>
		/// <param name="imgPreprocessed"></param>
		/// <param name="imgOriginal"></param>
		/// /// <param name="imgOriginal"></param>
		/// <returns>Image<Bgr, byte></returns>
		internal static Image<Bgr, byte> ShowHoughCircles(Image<Bgr, byte> img, Image<Bgr, byte> imgOriginal) {

			// convert to Gray
			Image<Gray, byte> imgGray = img.Convert<Gray, byte>();

			MemStorage SS = new MemStorage();
			//IntPtr segmentsPtr = CvInvoke.cvHoughCircles(imgGray, SS.Ptr, Emgu.CV.CvEnum.HOUGH_TYPE.CV_HOUGH_GRADIENT, 1, 60, 200, 100, 10, 500);//imgGray.Height/8, 200, 100, 0, imgGray.Height/2);//5, 0, 50, 3, 60);
			IntPtr segmentsPtr = CvInvoke.cvHoughCircles(imgGray, SS.Ptr, Emgu.CV.CvEnum.HOUGH_TYPE.CV_HOUGH_GRADIENT, 1, 30, 20, 10, 0, 0);

			// draw lines
			Seq<CircleF> segments = new Seq<CircleF>(segmentsPtr, SS);
			CircleF[] circles = segments.ToArray();

			Image<Bgr, byte> circleImage = imgOriginal.Copy();

			foreach(CircleF circle in circles) {
				float x = circle.Center.X;
				float y = circle.Center.Y;
				float r = circle.Radius;

				Point pt = new Point((int)x, (int)y);

				circleImage.Draw(new CircleF(pt, r), new Bgr(255, 0, 0), 1);
			}

			//CvInvoke.cvReleaseImage(ref segmentsPtr);

			return circleImage;
		}

		/// <summary>
		/// Detects faces using Haar cascade
		/// </summary>
		internal static void DetectFaces() {
			SingleImageFrom viewer = new SingleImageFrom();

			Capture capture = new Capture(); //create a camera captue

			EventHandler evnth = new EventHandler(delegate (object sender, EventArgs e) {  //run this until application closed (close button click on image viewer)
				HaarCascade haar = new HaarCascade("haarcascade_frontalface_default.xml"); //Detect Eyes
				Image<Bgr, byte> img = capture.QueryFrame().Clone();
				img = img.Flip(Emgu.CV.CvEnum.FLIP.HORIZONTAL);
				Image<Gray, byte> grayframe = img.Convert<Gray, byte>();
				var faces = haar.Detect(grayframe, 1.2, 3, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_ROUGH_SEARCH, new Size(40, 40), new Size(img.Width / 5, img.Height / 5));
				foreach(var face in faces) {
					img.Draw(face.rect, new Bgr(0, double.MaxValue, 0), 3);
				}
				viewer.ImageBox.Image = img;
				GC.Collect();
			});

			Application.Idle += evnth;

			viewer.ShowDialog();
			while(viewer.keep) {
				Application.DoEvents();
			}

			Application.Idle -= evnth;
			viewer.Dispose();
			capture.Dispose();
		}

		/// <summary>
		/// Detects eyes using Haar cascade
		/// </summary>
		internal static void DetectEyes() {
			SingleImageFrom viewer = new SingleImageFrom();

			Capture capture = new Capture(); //create a camera captue

			EventHandler evnth = new EventHandler(delegate (object sender, EventArgs e) {  //run this until application closed (close button click on image viewer)
				HaarCascade haar = new HaarCascade("haarcascade_eye.xml"); //Detect Eyes
				Image<Bgr, byte> img = capture.QueryFrame().Clone();
				img = img.Flip(Emgu.CV.CvEnum.FLIP.HORIZONTAL);
				Image<Gray, byte> grayframe = img.Convert<Gray, byte>();
				var faces = haar.Detect(grayframe, 1.2, 4, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_ROUGH_SEARCH, new Size(40, 40), new Size(img.Width / 3, img.Height / 3));
				foreach(var face in faces) {
					img.Draw(face.rect, new Bgr(0, double.MaxValue, 0), 3);
				}
				viewer.ImageBox.Image = img;
				GC.Collect();
			});

			Application.Idle += evnth;

			viewer.ShowDialog();
			while(viewer.keep) {
				Application.DoEvents();
			}

			Application.Idle -= evnth;
			viewer.Dispose();
			capture.Dispose();
		}

		/// <summary>
		/// Visualizes Amplitude and Phase from Webcam
		/// </summary>
		internal static void VideoSpectrumVisualization() {
			Image<Gray, float> amplitude = null;
			Image<Gray, float> phase = null;
			Image<Bgr, byte> img;

			ShowIMG viewer = new ShowIMG();

			Capture capture = new Capture(); //create a camera captue
			EventHandler evnth = new EventHandler(delegate (object sender, EventArgs e) {  //run this until application closed (close button click on image viewer)
				img = capture.QueryFrame().Clone();
				img = img.Flip(Emgu.CV.CvEnum.FLIP.HORIZONTAL);
				FFT.GetFFTAmpAndPhase(img, out amplitude, out phase);
				amplitude = FFT.PrepareForVizualization(amplitude, true);
				phase = FFT.PrepareForVizualization(phase, false);

				viewer.imageBox1.Image = amplitude;
				viewer.imageBox2.Image = phase;
			});

			Application.Idle += evnth;

			viewer.ShowDialog();
			while(viewer.keep) {
				Application.DoEvents();
				GC.Collect();
			}

			Application.Idle -= evnth;
			capture.Dispose();
			viewer.Dispose();

		}

		/// <summary>
		/// Visualizes Amplitude and Phase from image
		/// </summary>
		/// <param name="img">Imagem</param>
		/// <param name="amp">Imagem FFT de amplitude</param>
		/// <param name="pha">Imagem FFT de fase</param>
		internal static void ImageSpectrumVisualization(Image<Bgr, byte> img, out Image<Gray, float> amp, out Image<Gray, float> pha) {
			Image<Gray, float> amplitude = null;
			Image<Gray, float> phase = null;

			FFT.GetFFTAmpAndPhase(img, out amplitude, out phase);
			amp = FFT.PrepareForVizualization(amplitude, true);
			pha = FFT.PrepareForVizualization(phase, false);
		}

		/// <summary>
		/// Mixes Amplitude and Phase from 2 images
		/// </summary>
		/// <param name="img1">Imagem 1</param>
		/// <param name="img2">Imagem 2</param>
		/// <param name="img1mix">Imagem misturada 1</param>
		/// <param name="img2mix">Imagem misturada 2</param>
		internal static void MixAmpAndPhase(Image<Bgr, byte> img1, Image<Bgr, byte> img2, out Image<Gray, byte> img1mix, out Image<Gray, byte> img2mix) {
			Image<Gray, float> amp1;
			Image<Gray, float> phase1;

			Image<Gray, float> amp2;
			Image<Gray, float> phase2;

			FFT.GetFFTAmpAndPhase(img1, out amp1, out phase1);
			FFT.GetFFTAmpAndPhase(img2, out amp2, out phase2);

			img1mix = FFT.GetFFT_InverseAmpAndPhase(amp1, phase2);
			img2mix = FFT.GetFFT_InverseAmpAndPhase(amp2, phase1);
		}

		/// <summary>
		/// Aplica um filtro passa alto ideal no dominio da frequencia
		/// </summary>
		/// <param name="img">Imagem</param>
		/// <param name="size">Tamanho do filtro em pixeis</param>
		/// <param name="img_out">Imagem apos aplicacao do filtro</param>
		internal static void IdealHighPassFilter(Image<Bgr, byte> img, int size, out Image<Gray, byte> img_out) {
			Image<Gray, float> amp;
			Image<Gray, float> phase;

			Image<Gray, float> filterMask;

			FFT.GetFFTAmpAndPhase(img, out amp, out phase);
			filterMask = FFT.GenerateFilterMask(amp.Size, true, size);

			amp._Mul(filterMask);

			img_out = FFT.GetFFT_InverseAmpAndPhase(amp, phase);
		}

		/// <summary>
		/// Aplica um filtro passa baixo ideal no dominio da frequencia
		/// </summary>
		/// <param name="img">Imagem</param>
		/// <param name="size">Tamanho do filtro em pixeis</param>
		/// <param name="img_out">Imagem apos aplicacao do filtro</param>
		internal static void IdealLowPassFilter(Image<Bgr, byte> img, int size, out Image<Gray, byte> img_out) {
			Image<Gray, float> amp;
			Image<Gray, float> phase;

			Image<Gray, float> mask;

			FFT.GetFFTAmpAndPhase(img, out amp, out phase);
			mask = FFT.GenerateFilterMask(amp.Size, false, size);

			amp._Mul(mask);

			img_out = FFT.GetFFT_InverseAmpAndPhase(amp, phase);
		}

		/// <summary>
		/// Aplica um filtro passa baixo gaussiano no dominio da frequencia
		/// </summary>
		/// <param name="img">Imagem</param>
		/// <param name="size">Tamanho do filtro em pixeis</param>
		/// <param name="img_out">Imagem apos aplicacao do filtro</param>
		internal static void GaussianLowPassFilter(Image<Bgr, byte> img, int size, out Image<Gray, byte> img_out) {
			Image<Gray, float> amp;
			Image<Gray, float> phase;

			Image<Gray, float> mask;

			FFT.GetFFTAmpAndPhase(img, out amp, out phase);
			mask = FFT.GenerateGaussianMask(amp.Size, size);

			amp._Mul(mask);
			img_out = FFT.GetFFT_InverseAmpAndPhase(amp, phase);
		}

		internal static void VideoSignDetection(MainForm MF) {
			//SingleImageFrom viewer = new SingleImageFrom();
			//ShowIMG viewer = new ShowIMG();

			//Image<Gray, float> imagem = new Image<Gray, float>(@"C:\Users\Dimo\Documents\Universidade\TAPDI\4 Questionario\Imagem.png");
			//Image<Gray, float> mascara = new Image<Gray, float>(@"C:\Users\Dimo\Documents\Universidade\TAPDI\4 Questionario\mascara.png");

			////Image<Gray, float> imagem = new Image<Gray, float>(@"C:\Users\Dimo\Documents\Universidade\TAPDI\4 Questionario\imagem_antonio.png");
			////Image<Gray, float> mascara = new Image<Gray, float>(@"C:\Users\Dimo\Documents\Universidade\TAPDI\4 Questionario\mascara_antonio.png");

			//Image<Gray, float> img_out = new Image<Gray, float>(8 - 4 + 1, 8 - 4 + 1);

			//CvInvoke.cvMatchTemplate(imagem, mascara, img_out, TM_TYPE.CV_TM_CCOEFF);

			////ShowSingleIMG.ShowIMGStatic(img_out);
			//MF.ImageViewer.Image = img_out;
		}

		/// <summary>
		/// Realiza a conversão para HSV e faz a binarização para tons de vermelho
		/// </summary>
		/// <param name="img">Imagem</param>
		internal static Image<Bgr, byte> RedBinarization(Image<Bgr, byte> img) {
			unsafe
			{
				Image<Hsv, byte> img_hsv = img.Convert<Hsv, byte>();
				MIplImage source = img_hsv.MIplImage;
				byte* dataPtr = (byte*)source.imageData.ToPointer();

				int width = img_hsv.Width;
				int height = img_hsv.Height;
				int nChan = source.nChannels;
				int padding = source.widthStep - source.nChannels * source.width;
				int x, y;

				for(y = 0; y < height; y++) {
					for(x = 0; x < width; x++) {
						//if RED, then pixel is white
						if((dataPtr[0] >= 160 || dataPtr[0] <= 10) && (dataPtr[1] >= 100) && (dataPtr[2] >= 30)) {
							dataPtr[1] = 0;
							dataPtr[2] = 255;
							//else, pixel is black
						} else {
							dataPtr[2] = 0;
						}
						dataPtr += nChan;
					}
					dataPtr += padding;
				}
				GC.Collect();
				return (img_hsv.Convert<Bgr, byte>());
			}
		}

		/// <summary>
		/// Realiza a conversão para HSV e faz a binarização para tons de azul
		/// </summary>
		/// <param name="img">Imagem</param>
		internal static Image<Bgr, byte> BlueBinarization(Image<Bgr, byte> img) {
			unsafe
			{
				Image<Hsv, byte> img_hsv = img.Convert<Hsv, byte>();
				MIplImage source = img_hsv.MIplImage;
				byte* dataPtr = (byte*)source.imageData.ToPointer();

				int width = img_hsv.Width;
				int height = img_hsv.Height;
				int nChan = source.nChannels;
				int padding = source.widthStep - source.nChannels * source.width;
				int x, y;

				for(y = 0; y < height; y++) {
					for(x = 0; x < width; x++) {
						//if BLUE, then pixel is white
						if((dataPtr[0] >= 103 && dataPtr[0] <= 160) && (dataPtr[1] >= 30) && (dataPtr[2] >= 50)) {
							dataPtr[1] = 0;
							dataPtr[2] = 255;
							//else, pixel is black
						} else {
							dataPtr[2] = 0;
						}
						dataPtr += nChan;
					}
					dataPtr += padding;
				}
				GC.Collect();
				return (img_hsv.Convert<Bgr, byte>());
			}
		}

		/// <summary>
		/// Realiza a segmentação da imagem binarizada para uma determinada cor
		/// </summary>
		/// <param name="img">Imagem binarizada</param>
		/// <param name="color">Cor para a qual foi realizada a binarização</param>
		internal static int[] Segmentation(Image<Bgr, byte> img, char color) {
			unsafe
			{

				MIplImage source = img.MIplImage;
				byte* dataPtr = (byte*)source.imageData.ToPointer();

				int width = img.Width;
				int height = img.Height;
				int nChan = source.nChannels;
				int padding = source.widthStep - source.nChannels * source.width;
				int x, y, i, j, o, p;

				int min_size = img.Width / 15;

				img._Erode(2);
				img._Dilate(2);

				//vertical histogram
				int[] arrayXhist = HistogramXY(img, 'x');

				//lists for keeping detected objects
				List<int> objectsX1X2 = new List<int>();
				List<int> objectsY1Y2 = new List<int>();

				//BEGIN 1ST LEVEL OF SEGMENTATION	/*--------------- - -----------------*/	/*--------------- - -----------------*/

				int same_object = 0;
				int object_area = 0;
				int last_coordenate = 0;

				for(i = 0; i < width; i++) {
					if(arrayXhist[i] != 0 && same_object == 0) {
						object_area = 0;
						objectsX1X2.Add(i); //inicial
						same_object = 1;
						object_area += 1;
						last_coordenate = i;

					} else if(arrayXhist[i] == 0 && same_object == 1) {
						if(object_area > min_size)
							objectsX1X2.Add(i); //final
						else
							objectsX1X2.Remove(last_coordenate);

						same_object = 0;
						object_area = 0;
					} else if(arrayXhist[i] != 0 && same_object == 1 && i == width - 1) {
						if(object_area > min_size)
							objectsX1X2.Add(i); //final
						else
							objectsX1X2.Remove(last_coordenate);

						same_object = 0;
						object_area = 0;
					}
					if(object_area > 0)
						object_area += 1;
				}

				//convert list to array
				int[] arrayX = objectsX1X2.ToArray();

				//horizontal histogram
				int[] arrayYhist = HistogramXY(img, 'y');

				same_object = 0;

				for(i = 0; i < height; i++) {
					if(arrayYhist[i] != 0 && same_object == 0) {
						object_area = 0;
						objectsY1Y2.Add(i); //inicial
						same_object = 1;
						object_area += 1;
						last_coordenate = i;

					} else if(arrayYhist[i] == 0 && same_object == 1) {
						if(object_area > min_size)
							objectsY1Y2.Add(i); //final
						else
							objectsY1Y2.Remove(last_coordenate);

						same_object = 0;
						object_area = 0;

					} else if(arrayYhist[i] != 0 && same_object == 1 && i == height - 1) {
						if(object_area > min_size)
							objectsY1Y2.Add(i); //final
						else
							objectsY1Y2.Remove(last_coordenate);

						same_object = 0;
						object_area = 0;
					}
					if(object_area > 0)
						object_area += 1;
				}

				//convert list to array
				int[] arrayY = objectsY1Y2.ToArray();

				List<int> firstSegmentationObjectList = new List<int>();

				//makes an list with configuration {x1, x2, y1, y2}
				for(i = 0; i < arrayX.Length; i += 2) {
					for(j = 0; j < arrayY.Length; j += 2) {
						firstSegmentationObjectList.Add(arrayX[i]);
						firstSegmentationObjectList.Add(arrayX[i + 1]);

						firstSegmentationObjectList.Add(arrayY[j]);
						firstSegmentationObjectList.Add(arrayY[j + 1]);
					}
				}

				//END 1ST LEVEL OF SEGMENTATION	/*--------------- - -----------------*/	/*--------------- - -----------------*/

				//convert to array
				int[] objects = firstSegmentationObjectList.ToArray();

				//BEGIN 2ND LEVEL OF SEGMENTATION	/*--------------- - -----------------*/	/*--------------- - -----------------*/

				List<int> secondSegmentationObjectList = new List<int>();
				Image<Bgr, byte> imgROI = null;

				for(i = 0; i < objects.Length; i += 4) {

					objectsX1X2.Clear();
					objectsY1Y2.Clear();

					//gets ROI of image and does the same algorithm as before
					imgROI = img.Copy(new System.Drawing.Rectangle(objects[i], objects[i + 2], (objects[i + 1] - objects[i]), (objects[i + 3] - objects[i + 2])));

					//vertical histogram
					arrayXhist = HistogramXY(imgROI, 'x');
					same_object = 0;

					for(j = 0; j < arrayXhist.Length; j++) {
						if(arrayXhist[j] != 0 && same_object == 0) {
							object_area = 0;
							objectsX1X2.Add(j); //inicial
							same_object = 1;
							object_area = 1;
							last_coordenate = j;

						} else if(arrayXhist[j] == 0 && same_object == 1) {
							if(object_area > min_size)
								objectsX1X2.Add(j); //final
							else
								objectsX1X2.Remove(last_coordenate);

							same_object = 0;
							object_area = 0;
						} else if(arrayXhist[j] != 0 && same_object == 1 && j == arrayXhist.Length - 1) {
							if(object_area > min_size)
								objectsX1X2.Add(j); //final
							else
								objectsX1X2.Remove(last_coordenate);

							same_object = 0;
							object_area = 0;
						}
						if(object_area > 0)
							object_area += 1;
					}

					arrayX = objectsX1X2.ToArray();

					//horizontal histogram
					arrayYhist = HistogramXY(imgROI, 'y');
					same_object = 0;

					for(j = 0; j < arrayYhist.Length; j++) {
						if(arrayYhist[j] != 0 && same_object == 0) {
							object_area = 0;
							objectsY1Y2.Add(j); //inicial
							same_object = 1;
							object_area += 1;
							last_coordenate = j;

						} else if(arrayYhist[j] == 0 && same_object == 1) {
							if(object_area > min_size)
								objectsY1Y2.Add(j); //final
							else
								objectsY1Y2.Remove(last_coordenate);

							same_object = 0;
							object_area = 0;
						} else if(arrayYhist[j] != 0 && same_object == 1 && j == arrayYhist.Length - 1) {
							if(object_area > min_size)
								objectsY1Y2.Add(j); //final
							else
								objectsY1Y2.Remove(last_coordenate);

							same_object = 0;
							object_area = 0;
						}
						if(object_area > 0)
							object_area += 1;
					}

					arrayY = objectsY1Y2.ToArray();

					//makes an list with configuration {x1, x2, y1, y2}
					for(o = 0; o < arrayX.Length; o += 2) {
						for(p = 0; p < arrayY.Length; p += 2) {
							secondSegmentationObjectList.Add(arrayX[o] + objects[i]);
							secondSegmentationObjectList.Add(arrayX[o + 1] + objects[i]);

							secondSegmentationObjectList.Add(arrayY[p] + objects[i + 2]);
							secondSegmentationObjectList.Add(arrayY[p + 1] + objects[i + 2]);
						}
					}
				}

				objectsX1X2.Clear();
				objectsY1Y2.Clear();
				GC.Collect();

				//END 2ND LEVEL OF SEGMENTATION	/*--------------- - -----------------*/	/*--------------- - -----------------*/

				int[] ratioObjects = secondSegmentationObjectList.ToArray();

				/*---REMOVE OBJECTS THAT AREN'T SQUARES---*/

				List<int> ratioCleanList = new List<int>();

				for(i = 0; i < ratioObjects.Length; i += 4) {
					float ratio = ((float)ratioObjects[i + 1] - ratioObjects[i]) / ((float)ratioObjects[i + 3] - ratioObjects[i + 2]);
					if(ratio < 1.2 && ratio > 0.8) {
						ratioCleanList.Add(ratioObjects[i]);
						ratioCleanList.Add(ratioObjects[i + 1]);
						ratioCleanList.Add(ratioObjects[i + 2]);
						ratioCleanList.Add(ratioObjects[i + 3]);
					}
				}

				/*---REMOVE OBJECTS THAT AREN'T SQUARES---*/

				int[] finalObjects = ratioCleanList.ToArray();

				/*---IDENTIFIES WHAT SHAPE IS THE SIGN---*/

				List<int> finalShapeIdentifiedList = new List<int>();

				double[] coeff = new double[3];
				double[] hist_x;
				double[] hist_y;

				for(i = 0; i < finalObjects.Length; i += 4) {
					imgROI = img.Copy(new System.Drawing.Rectangle(finalObjects[i], finalObjects[i + 2], (finalObjects[i + 1] - finalObjects[i]), (finalObjects[i + 3] - finalObjects[i + 2])));
					imgROI = imgROI.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC, true);

					int[] hist = new int[imgROI.Rows];

					MIplImage m_temp = imgROI.MIplImage;
					dataPtr = (byte*)m_temp.imageData.ToPointer();
					int nChan_temp = m_temp.nChannels;
					int width_temp = imgROI.Width;
					int height_temp = imgROI.Height;
					int padding_temp = m_temp.widthStep - m_temp.nChannels * m_temp.width;

					//gets distance of white pixels from image edge
					for(y = 0; y < height_temp; y++) {
						for(x = 0; x < width_temp; x++) {

							if((dataPtr[0] > 128) && (dataPtr[1] > 128) && (dataPtr[2] > 128)) {
								dataPtr = dataPtr + (width_temp - x) * nChan_temp;
								break;
							}
							hist[y] += 1;

							// avança apontador para próximo pixel
							dataPtr += nChan_temp;
						}
						//no fim da linha avança alinhamento (padding)
						dataPtr += padding_temp;
					}

					hist_x = new double[hist.Length];
					hist_y = new double[hist.Length];

					for(j = 0; j < hist.Length; j++) {
						hist_x[j] = j;
						hist_y[j] = hist[j];
					}

					//curve fit to get polinomial coefficients
					coeff = Fit.Polynomial(hist_x, hist_y, 2, MathNet.Numerics.LinearRegression.DirectRegressionMethod.QR);

					//applies module to coefficients
					coeff[0] = coeff[0] < 0 ? -coeff[0] : coeff[0];
					coeff[1] = coeff[1] < 0 ? -coeff[1] : coeff[1];
					coeff[2] = coeff[2] < 0 ? -coeff[2] : coeff[2];

					switch(color) {
						case 'r':
							finalShapeIdentifiedList.Add(finalObjects[i]);
							finalShapeIdentifiedList.Add(finalObjects[i + 1]);
							finalShapeIdentifiedList.Add(finalObjects[i + 2]);
							finalShapeIdentifiedList.Add(finalObjects[i + 3]);

							if(coeff[2] < 0.006 && coeff[1] < 1 && coeff[0] < 50) {
								finalShapeIdentifiedList.Add(0); //RED
								finalShapeIdentifiedList.Add(3); //TRI-ANGLE
							} else if(coeff[2] < 0.020 && coeff[1] > 1 && coeff[0] < 40) {
								finalShapeIdentifiedList.Add(0); //RED
								finalShapeIdentifiedList.Add(0); //CIRCLE
							} else {
								finalShapeIdentifiedList.Add(0); //RED
								finalShapeIdentifiedList.Add(1); //DEFAULT
							}
							break;

						case 'b':
							finalShapeIdentifiedList.Add(finalObjects[i]);
							finalShapeIdentifiedList.Add(finalObjects[i + 1]);
							finalShapeIdentifiedList.Add(finalObjects[i + 2]);
							finalShapeIdentifiedList.Add(finalObjects[i + 3]);

							if(coeff[2] < 0.020 && coeff[1] > 1 && coeff[0] < 40) {
								finalShapeIdentifiedList.Add(1); //BLUE
								finalShapeIdentifiedList.Add(0); //CIRCLE
							} else if(coeff[2] < 0.010 && coeff[1] < 1.0 && coeff[0] < 30) {
								finalShapeIdentifiedList.Add(1); //BLUE
								finalShapeIdentifiedList.Add(4); //SQUARE
							} else {
								finalShapeIdentifiedList.Add(1); //BLUE
								finalShapeIdentifiedList.Add(1); //DEFAULT
							}
							break;

						default:
							break;
					}
				}

				GC.Collect();

				/*---IDENTIFIES WHAT SHAPE IS THE SIGN---*/

				return finalShapeIdentifiedList.ToArray();
			}
		}

		/// <summary>
		/// Realiza um template matching para determinar o melhor candidato para o sinal em análise
		/// </summary>
		/// <param name="img">Imagem</param>
		/// <param name="segmentation">Segmentação da imagem</param>
		/// <param name="DataBase">Base de Dados de imagens</param>
		internal static void TemplateMatching(Image<Bgr, byte> img, int[] segmentation, List<List<Image<Bgr, byte>>> DataBase) {

			int[] objects = segmentation;
			int i, color, shape;
			Image<Bgr, byte> imgROI;
			List<Image<Bgr, byte>> blueSquareDataBase = new List<Image<Bgr, byte>>();
			List<Image<Bgr, byte>> blueCircleDataBase = new List<Image<Bgr, byte>>();
			List<Image<Bgr, byte>> redTriangleDataBase = new List<Image<Bgr, byte>>();
			List<Image<Bgr, byte>> redCircleDataBase = new List<Image<Bgr, byte>>();
			double value = 0;
			double best_match_value = 0;
			MCvFont f = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 2.0, 3.0);

			//objects come at formation {x1, x2, y1, y2, color, shape}
			for(i = 0; i < objects.Length - 1; i += 6) {
				imgROI = img.Copy(new System.Drawing.Rectangle(objects[i], objects[i + 2], (objects[i + 1] - objects[i]), (objects[i + 3] - objects[i + 2])));
				imgROI = imgROI.Resize(100, 100, INTER.CV_INTER_CUBIC);

				best_match_value = 0;
				Image<Bgr, byte> best_match = imgROI.Sub(new Bgr(255,255,255));

				color = objects[i + 4];
				shape = objects[i + 5];

				if(color == 0) {
					imgROI = ImageEnhancement(imgROI, 'r');
				} else if(color == 1) {
					imgROI = ImageEnhancement(imgROI, 'b');
				}

				redCircleDataBase = DataBase.ElementAt(0); //color: 0 shape 0
				redTriangleDataBase = DataBase.ElementAt(1); //color: 0 shape 3
				blueCircleDataBase = DataBase.ElementAt(2); //color: 1 shape 0
				blueSquareDataBase = DataBase.ElementAt(3); //color: 1 shape 4

				switch(color) {
					case 0: //RED
						switch(shape) {
							case 0:
								foreach(Image<Bgr, byte> imgDB in redCircleDataBase) {
									//value = MatchTemplate(imgROI.Convert<Gray, byte>(), imgDB.Convert<Gray, byte>());
									value = MatchTemplate(imgROI, imgDB);

									if(value > best_match_value) {
										best_match_value = value;
										best_match = imgDB;
									}
								}
								break;

							case 3:
								foreach(Image<Bgr, byte> imgDB in redTriangleDataBase) {
									//value = MatchTemplate(imgROI.Convert<Gray, byte>(), imgDB.Convert<Gray, byte>());
									value = MatchTemplate(imgROI, imgDB);

									if(value > best_match_value) {
										best_match_value = value;
										best_match = imgDB;
									}
								}
								break;

							default:

								foreach(Image<Bgr, byte> imgDB in redCircleDataBase) {
									//value = MatchTemplate(imgROI.Convert<Gray, byte>(), imgDB.Convert<Gray, byte>());
									value = MatchTemplate(imgROI, imgDB);

									if(value > best_match_value) {
										best_match_value = value;
										best_match = imgDB;
									}
								}

								foreach(Image<Bgr, byte> imgDB in redTriangleDataBase) {
									//value = MatchTemplate(imgROI.Convert<Gray, byte>(), imgDB.Convert<Gray, byte>());
									value = MatchTemplate(imgROI, imgDB);

									if(value > best_match_value) {
										best_match_value = value;
										best_match = imgDB;
									}
								}

								break;
						}
						break;

					case 1: //BLUE
						switch(shape) {
							case 0:
								foreach(Image<Bgr, byte> imgDB in blueCircleDataBase) {
									//value = MatchTemplate(imgROI.Convert<Gray, byte>(), imgDB.Convert<Gray, byte>());
									value = MatchTemplate(imgROI, imgDB);

									if(value > best_match_value) {
										best_match_value = value;
										best_match = imgDB;
									}
								}
								break;

							case 4:
								foreach(Image<Bgr, byte> imgDB in blueSquareDataBase) {
									//value = MatchTemplate(imgROI.Convert<Gray, byte>(), imgDB.Convert<Gray, byte>());
									value = MatchTemplate(imgROI, imgDB);

									if(value > best_match_value) {
										best_match_value = value;
										best_match = imgDB;
									}
								}
								break;

							default:
								foreach(Image<Bgr, byte> imgDB in blueCircleDataBase) {
									//value = MatchTemplate(imgROI.Convert<Gray, byte>(), imgDB.Convert<Gray, byte>());
									value = MatchTemplate(imgROI, imgDB);

									if(value > best_match_value) {
										best_match_value = value;
										best_match = imgDB;
									}
								}

								foreach(Image<Bgr, byte> imgDB in blueSquareDataBase) {
									//value = MatchTemplate(imgROI.Convert<Gray, byte>(), imgDB.Convert<Gray, byte>());
									value = MatchTemplate(imgROI, imgDB);

									if(value > best_match_value) {
										best_match_value = value;
										best_match = imgDB;
									}
								}

								break;
						}
						break;

					default:

						foreach(Image<Bgr, byte> imgDB in redCircleDataBase) {
							//value = MatchTemplate(imgROI.Convert<Gray, byte>(), imgDB.Convert<Gray, byte>());
							value = MatchTemplate(imgROI, imgDB);

							if(value > best_match_value) {
								best_match_value = value;
								best_match = imgDB;
							}
						}

						foreach(Image<Bgr, byte> imgDB in redTriangleDataBase) {
							//value = MatchTemplate(imgROI.Convert<Gray, byte>(), imgDB.Convert<Gray, byte>());
							value = MatchTemplate(imgROI, imgDB);

							if(value > best_match_value) {
								best_match_value = value;
								best_match = imgDB;
							}
						}

						foreach(Image<Bgr, byte> imgDB in blueCircleDataBase) {
							//value = MatchTemplate(imgROI.Convert<Gray, byte>(), imgDB.Convert<Gray, byte>());
							value = MatchTemplate(imgROI, imgDB);

							if(value > best_match_value) {
								best_match_value = value;
								best_match = imgDB;
							}
						}

						foreach(Image<Bgr, byte> imgDB in blueSquareDataBase) {
							//value = MatchTemplate(imgROI.Convert<Gray, byte>(), imgDB.Convert<Gray, byte>());
							value = MatchTemplate(imgROI, imgDB);

							if(value > best_match_value) {
								best_match_value = value;
								best_match = imgDB;
							}
						}

						break;
				}

				if(best_match_value >= 0.1) {
					unsafe {
						MIplImage m = img.MIplImage;
						byte* imgPtr = (byte*)m.imageData.ToPointer(); // obter apontador do inicio da imagem
						byte* auxPtr;

						best_match = best_match.Resize(objects[i + 1] - objects[i], objects[i + 3] - objects[i + 2], INTER.CV_INTER_CUBIC);
						MIplImage n = best_match.MIplImage;
						byte* imgMatchPtr = (byte*)n.imageData.ToPointer(); // obter apontador do inicio da imagem
						byte* auxMatchPtr;

						int width = img.Width;
						int height = img.Height;
						int nChan = m.nChannels; // numero de canais 3
						int padding = m.widthStep - m.nChannels * m.width; // alinhamento (padding)
						int x, y;

						for(y = 0; y < height; y++) {
							for(x = 0; x < width; x++) {
								auxPtr = (imgPtr + y * m.widthStep + x * nChan);

								if(x >= objects[i] && x < objects[i + 1] && y >= objects[i + 2] && y < objects[i + 3]) {

									auxMatchPtr = (imgMatchPtr + (y - objects[i + 2]) * n.widthStep + (x - objects[i]) * nChan);

									auxPtr[0] = auxMatchPtr[0];
									auxPtr[1] = auxMatchPtr[1];
									auxPtr[2] = auxMatchPtr[2];
								}
							}
						}
						CvInvoke.cvPutText(img, value.ToString("p2"), new Point(objects[i+1], objects[i + 2] - 4), ref f, new Bgr(255, 255, 0).MCvScalar);
					}
				}
			}
		}

		/// <summary>
		/// Compara duas imagens com coeficiente de correlação normalizado
		/// </summary>
		/// <param name="img">Imagem</param>
		/// <param name="tmp">Template</param>
		internal static double MatchTemplate(Image<Bgr, byte> img, Image<Bgr, byte> tmp) {
			unsafe {
				Image<Bgr, byte> diff = img.Convert<Bgr, byte>();

				MIplImage m = img.MIplImage;
				byte* imgPtr = (byte*)m.imageData.ToPointer(); // obter apontador do inicio da imagem
				MIplImage n = tmp.MIplImage;
				byte* tmpPtr = (byte*)n.imageData.ToPointer(); // obter apontador do inicio da imagem
				MIplImage o = tmp.MIplImage;
				byte* diffPtr = (byte*)o.imageData.ToPointer(); // obter apontador do inicio da imagem

				int width_img = img.Width;
				int height_img = img.Height;
				int nChan_img = m.nChannels; // numero de canais 3
				int padding_img = m.widthStep - m.nChannels * m.width; // alinhamento (padding)

				int width_tmp = tmp.Width;
				int height_tmp = tmp.Height;
				int nChan_tmp = n.nChannels; // numero de canais 3
				int padding_tmp = n.widthStep - n.nChannels * n.width; // alinhamento (padding)

				int width_diff = diff.Width;
				int height_diff = diff.Height;
				int nChan_diff = o.nChannels; // numero de canais 3
				int padding_diff = o.widthStep - o.nChannels * o.width; // alinhamento (padding)

				int x, y;
				byte* auxPtr_img, auxPtr_tmp;

				float factor_img = 1f / (width_img * height_img);
				float factor_tmp = 1f / (width_tmp * height_tmp);

				double R = 0;
				float sum_T = 0;
				float sum_I = 0;
				float sum_T_2 = 0;
				float sum_I_2 = 0;
				float temp_nom = 0;

				for(y = 0; y < height_tmp; y++) { //OBTER SOMATORIO DE T E I
					for(x = 0; x < width_tmp; x++) {
						auxPtr_img = (imgPtr + y * m.widthStep + x * nChan_img);
						auxPtr_tmp = (tmpPtr + y * n.widthStep + x * nChan_tmp);

						sum_T += ((float)auxPtr_tmp[0]+ auxPtr_tmp[1]+ auxPtr_tmp[2])/3;
						sum_I += ((float)auxPtr_img[0]+ auxPtr_img[1]+ auxPtr_img[2])/3;
					}
				}

				for(y = 0; y < height_tmp; y++) {
					for(x = 0; x < width_tmp; x++) {
						auxPtr_img = (imgPtr + y * m.widthStep + x * nChan_img);
						auxPtr_tmp = (tmpPtr + y * n.widthStep + x * nChan_tmp);

						temp_nom += ((((float)auxPtr_tmp[0] + auxPtr_tmp[1] + auxPtr_tmp[2]) / 3) - factor_tmp * sum_T) * ((((float)auxPtr_img[0] + auxPtr_img[1] + auxPtr_img[2]) / 3) - factor_img * sum_I);
						sum_T_2 += ((((float)auxPtr_tmp[0] + auxPtr_tmp[1] + auxPtr_tmp[2]) / 3) - factor_tmp * sum_T) * ((((float)auxPtr_tmp[0] + auxPtr_tmp[1] + auxPtr_tmp[2]) / 3) - factor_tmp * sum_T);
						sum_I_2 += ((((float)auxPtr_img[0] + auxPtr_img[1] + auxPtr_img[2]) / 3) - factor_img * sum_I) * ((((float)auxPtr_img[0] + auxPtr_img[1] + auxPtr_img[2]) / 3) - factor_img * sum_I);
					}
				}

				R = temp_nom / (Math.Sqrt(sum_T_2 * sum_I_2));

				return R;
			}
		}

		/// <summary>
		/// Realiza um melhoramento da imagem que contem um sinal
		/// </summary>
		/// <param name="img">Imagem</param>
		/// <param name="color">Cor do sinal</param>
		internal static Image<Bgr, byte> ImageEnhancement(Image<Bgr, byte> img, char color) {
			unsafe
			{
				Image<Hsv, byte> img_hsv = img.Convert<Hsv, byte>();
				MIplImage m = img_hsv.MIplImage;
				byte* dataPtr = (byte*)m.imageData.ToPointer(); // obter apontador do inicio da imagem

				int width = img.Width;
				int height = img.Height;
				int nChan = m.nChannels; // numero de canais 3
				int padding = m.widthStep - m.nChannels * m.width; // alinhamento (padding)
				int x, y;

				byte* auxPtr;

				switch(color) {
					case 'r':
						//tudo o que estiver fora do sinal, é retirado (do lado esquerdo)
						for(y = 0; y < height; y++) {
							for(x = 0; x < width; x++) {
								auxPtr = (dataPtr + y * m.widthStep + x * nChan);
								//se vermelho, avança para a linha seguinte
								if((auxPtr[0] >= 160 || auxPtr[0] <= 10) && (auxPtr[1] >= 100) && (auxPtr[2] >= 30)) {
									auxPtr = auxPtr + (width - x) * nChan;
									break;
								}
								auxPtr[2] = 255;
								auxPtr[1] = 0;
								auxPtr[0] = 180;
							}
						}

						//tudo o que estiver fora do sinal, é retirado (do lado direito)
						for(y = 0; y < height; y++) {
							for(x = width - 1; x >= 0; x--) {
								auxPtr = (dataPtr + y * m.widthStep + x * nChan);
								//se vermelho, avança para a linha seguinte
								if((auxPtr[0] >= 160 || auxPtr[0] <= 10) && (auxPtr[1] >= 100) && (auxPtr[2] >= 30)) {
									break;
								}
								auxPtr[2] = 255;
								auxPtr[1] = 0;
								auxPtr[0] = 180;
							}
						}

						//converte qualquer vermelho para vermelho puro e qualquer "preto" para preto puro
						for(y = 0; y < height; y++) {
							for(x = 0; x < width; x++) {
								auxPtr = (dataPtr + y * m.widthStep + x * nChan);
								//se for vermelho
								if((auxPtr[0] >= 160 || auxPtr[0] <= 10) && (auxPtr[1] >= 100) && (auxPtr[2] >= 30)) {
									auxPtr[2] = 255;
									auxPtr[1] = 255;
									auxPtr[0] = 0;
								} else {
									//se for preto
									if((auxPtr[2] <= 100)) {
										auxPtr[2] = 0;
										auxPtr[1] = 0;
										auxPtr[0] = 0;
									} else {
										auxPtr[2] = 255;
										auxPtr[1] = 0;
										auxPtr[0] = 180;
									}
								}
							}
						}
						break;

					case 'b':
						//tudo o que estiver fora do sinal, é retirado (do lado esquerdo)
						for(y = 0; y < height; y++) {
							for(x = 0; x < width; x++) {
								auxPtr = (dataPtr + y * m.widthStep + x * nChan);
								//se azul, avança para a linha seguinte
								if((auxPtr[0] >= 100 && auxPtr[0] <= 160) && (auxPtr[1] >= 30) && (auxPtr[2] >= 50)) {
									auxPtr = auxPtr + (width - x) * nChan;
									break;
								}
								auxPtr[2] = 255;
								auxPtr[1] = 0;
								auxPtr[0] = 180;
							}
						}

						//tudo o que estiver fora do sinal, é retirado (do lado direito)
						for(y = 0; y < height; y++) {
							for(x = width - 1; x >= 0; x--) {

								auxPtr = (dataPtr + y * m.widthStep + x * nChan);

								//se azul, avança para a linha seguinte
								if((auxPtr[0] >= 100 && auxPtr[0] <= 160) && (auxPtr[1] >= 30) && (auxPtr[2] >= 50)) {
									break;
								}

								auxPtr[2] = 255;
								auxPtr[1] = 0;
								auxPtr[0] = 180;
							}
						}

						//converte qualquer azul para azul puro e qualquer "preto" para preto puro
						for(y = 0; y < height; y++) {
							for(x = 0; x < width; x++) {
								auxPtr = (dataPtr + y * m.widthStep + x * nChan);
								//se for azul
								if((auxPtr[0] >= 100 && auxPtr[0] <= 160) && (auxPtr[1] >= 30) && (auxPtr[2] >= 50)) {
									auxPtr[2] = 255;
									auxPtr[1] = 255;
									auxPtr[0] = 120;
								} else {
									//se for preto
									if((auxPtr[1] <= 100)&&(auxPtr[2] <= 100)) {
										auxPtr[2] = 0;
										auxPtr[1] = 0;
										auxPtr[0] = 0;
									} else {
										auxPtr[2] = 255;
										auxPtr[1] = 0;
										auxPtr[0] = 180;
									}
								}
							}
						}
						break;
					default:
						break;
				}

				return img_hsv.Convert<Bgr, byte>();

			}
		}

	}
}
