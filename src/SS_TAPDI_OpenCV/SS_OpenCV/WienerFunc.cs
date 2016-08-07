using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Drawing;
using Emgu.CV.VideoSurveillance;
using System.Windows.Forms;

namespace SS_OpenCV {
	public class WienerFunc {

		/// <summary>
		/// Filter the image using Motion Blur or low pass filter
		/// </summary>
		/// <param name="img"></param>
		/// <param name="motion"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		public static Image<Bgr, byte> FilterConv(Image<Bgr, byte> img, bool motion, float size) {
			Image<Gray, float> filter = null;
			Image<Bgr, float> imgF = null;
			if(motion) {
				filter = MotionBlurTF();
				try {

					ConvolutionKernelF kernel = new ConvolutionKernelF(filter.Width, filter.Height);
					kernel.Bytes = filter.Bytes;
					kernel.Center = new Point(filter.Width / 2, filter.Height / 2);
					imgF = img.Convolution(kernel);
					imgF = imgF.Mul(1 / kernel.Sum);
				} catch(Exception ex) {

					MessageBox.Show(ex.ToString());
				}
			} else {
				filter = FFT.GenerateGaussianMask(new Size(45, 45), size);
				try {

					ConvolutionKernelF kernel = new ConvolutionKernelF(filter.Width, filter.Height);
					kernel.Bytes = filter.Bytes;
					kernel.Center = new Point(filter.Width / 2, filter.Height / 2);
					imgF = img.Convolution(kernel);
					imgF = imgF.Mul(1 / kernel.Sum);
				} catch(Exception ex) {

					MessageBox.Show(ex.ToString());
				}

				//    Image<Gray, float> fft_Amp1;
				//    Image<Gray, float> fft_Phase1;
				//    ImageClass.GetFFTAmpAndPhase(img, out fft_Amp1, out fft_Phase1);
				//    ShowIMG.ShowIMGStatic(fft_Amp1.Log(), fft_Phase1 * 20);

				//    Image<Gray, float> mask = FFT.GenerateGaussian(fft_Amp1.Size, false, Convert.ToInt32(value));

				//    Image<Gray, byte> img_iFFT1 = ImageClass.GetFFT_InverseAmpAndPhase(fft_Amp1.Mul(mask), fft_Phase1);
			}

			return imgF.Convert<Bgr, byte>();

		}

		/// <summary>
		/// generate a sharp filter
		/// </summary>
		/// <param name="size"></param>
		/// <param name="isHighPass"></param>
		/// <param name="width"></param>
		/// <returns></returns>
		private static Image<Gray, float> GenerateFilterMask(Size size, bool isHighPass, int width) {
			Image<Gray, float> mask = new Image<Gray, float>(size);
			if(isHighPass) {
				mask.SetValue(1);
				mask.Draw(new CircleF(new PointF(size.Width / 2, size.Height / 2), width), new Gray(0), 0);
			} else {
				mask.SetZero();
				mask.Draw(new CircleF(new PointF(size.Width / 2, size.Height / 2), width), new Gray(1), 0);
			}
			return mask;
		}

		/// <summary>
		/// generate a sharp filter
		/// </summary>
		/// <param name="size"></param>
		/// <param name="isHighPass"></param>
		/// <param name="width"></param>
		/// <returns></returns>
		private static Image<Gray, float> GenerateButterworth(Size size, double width) {
			Image<Gray, float> mask = new Image<Gray, float>(size);
			double order = 1;
			double uu, vv;
			for(int u = 0; u < size.Height; u++) {
				for(int v = 0; v < size.Width; v++) {
					uu = u - size.Height / 2.0;
					vv = v - size.Width / 2.0;
					mask[u, v] = new Gray(1.0 / (1 + Math.Pow((uu * uu + vv * vv) / (double)(width * width), order)));
				}
			}
			//TableForm.ShowTable(mask,"BW");
			return mask;
		}

		/// <summary>
		/// Motion Blur Transfer function (taken from Matlab)
		/// </summary>
		/// <returns></returns>
		private static Image<Gray, float> MotionBlurTF() {
			Image<Gray, float> TF = new Image<Gray, float>(15, 15);

			float[] matrix ={0,0,0,0,0,0,0,0,0,0,0,0,0,0.013074F,0.026806F,0,0,0,0,0,0,0,0,0,0,0,0,0.013074F,0.044639F,0.013074F,
				0,0,0,0,0,0,0,0,0,0,0,0.013074F,0.044639F,0.013074F,0,0,0,0,0,0,0,0,0,0,0,0.013074F,0.044639F,0.013074F,
				0,0,0,0,0,0,0,0,0,0,0,0.013074F,0.044639F,0.013074F,0,0,0,0,0,0,0,0,0,0,0,0.013074F,0.044639F,0.013074F,
				0,0,0,0,0,0,0,0,0,0,0,0.013074F,0.044639F,0.013074F,0,0,0,0,0,0,0,0,0,0,0,0.013074F,0.044639F,0.013074F,
				0,0,0,0,0,0,0,0,0,0,0,0.013074F,0.044639F,0.013074F,0,0,0,0,0,0,0,0,0,0,0,0.013074F,0.044639F,0.013074F,
				0,0,0,0,0,0,0,0,0,0,0,0.013074F,0.044639F,0.013074F,0,0,0,0,0,0,0,0,0,0,0,0.013074F,0.044639F,0.013074F,
				0,0,0,0,0,0,0,0,0,0,0,0.013074F,0.044639F,0.013074F,0,0,0,0,0,0,0,0,0,0,0,0.013074F,0.044639F,0.013074F,
				0,0,0,0,0,0,0,0,0,0,0,0,0.026806F,0.013074F,0,0,0,0,0,0,0,0,0,0,0,0,0};

			int idx = 0;
			for(int y = 0; y < 15; y++) {
				for(int x = 0; x < 15; x++) {
					TF[y, x] = new Gray(matrix[idx++] * 1 / 0.044639F);
				}
			}
			return TF;
		}

		/// <summary>
		/// Wiener filter
		/// </summary>
		/// <param name="img"></param>
		/// <param name="snr"></param>
		/// <param name="size"></param>
		/// <param name="motion"></param>
		/// <returns></returns>
		public static Image<Bgr, byte> WienerFilter(Image<Bgr, byte> img, float snr, float size, bool motion) {
			//IMG FFT
			IntPtr imageFFT;
			Size s = WienerFunc.GetFFTReAndIm(img, out imageFFT);

			Image<Gray, float> imgFFT_ReNum = new Image<Gray, float>(s);
			Image<Gray, float> imgFFT_ImNum = new Image<Gray, float>(s);
			CvInvoke.cvSplit(imageFFT, imgFFT_ReNum, imgFFT_ImNum, System.IntPtr.Zero, System.IntPtr.Zero);


			//Filter FFT
			Image<Gray, float> filter = null;
			if(motion) {
				filter = MotionBlurTF();
				filter = PadWithZeros(filter, s);
			} else {
				filter = FFT.GenerateGaussianMask(new Size(45, 45), size); //filter = FFT.GenerateGaussianMask(s, size);// GenerateFilterMask(new Size(size, size), false, size / 2);
				filter = PadWithZeros(filter, s);
			}

			IntPtr filterFFT;
			//FFT2shift(filter);
			s = WienerFunc.GetFFTReAndIm(filter, out filterFFT);

			//H(u,v)*.G(u,v)
			IntPtr imgAuxNum = CvInvoke.cvCreateImage(s, Emgu.CV.CvEnum.IPL_DEPTH.IPL_DEPTH_32F, 2);
			CvInvoke.cvMulSpectrums(imageFFT, filterFFT, imgAuxNum, Emgu.CV.CvEnum.MUL_SPECTRUMS_TYPE.CV_DXT_MUL_CONJ);

			//|H(u,v)|^2
			IntPtr imgAuxDen = CvInvoke.cvCreateImage(s, Emgu.CV.CvEnum.IPL_DEPTH.IPL_DEPTH_32F, 2);
			CvInvoke.cvMulSpectrums(filterFFT, filterFFT, imgAuxDen, Emgu.CV.CvEnum.MUL_SPECTRUMS_TYPE.CV_DXT_MUL_CONJ);

			//|H(u,v)|^2 + SNR
			imgAuxDen = AddScalarFFT(imgAuxDen, snr, s);

			//H(u,v)*.G(u,v)/(|H(u,v)|^2 + K)
			IntPtr imgOut = DivideComplexMatrix(imgAuxNum, imgAuxDen, s);
			//get fft inverse

			Image<Gray, byte> imageOut = FFT.FFT2shift(WienerFunc.GetFFT_InverseReAndIm(imgOut, s), true).Convert<Gray, byte>();

			return imageOut.Convert<Bgr, byte>();
		}

		/// <summary>
		/// Calculate image FFT and return real and imaginary 
		/// </summary>
		/// <param name="img"></param>
		/// <param name="fft_Re"></param>
		/// <param name="fft_Im"></param>
		public static Size GetFFTReAndIm(Image<Bgr, byte> img, out IntPtr imageFFT) {
			return GetFFTReAndIm(img.Convert<Gray, float>(), out imageFFT);
		}

		/// <summary>
		/// Calculate Inverse FFT from complex image
		/// </summary>
		/// <param name="imageFFT"></param>
		/// <returns></returns>
		private static Image<Gray, float> GetFFT_InverseReAndIm(IntPtr imageFFT, Size s) {
			// calculate DFT 
			CvInvoke.cvDFT(imageFFT, imageFFT, Emgu.CV.CvEnum.CV_DXT.CV_DXT_INVERSE, 0);

			// get Real and Imaginary channels
			Image<Gray, float> fft_Re = new Image<Gray, float>(s);
			Image<Gray, float> fft_Im = new Image<Gray, float>(s);

			CvInvoke.cvSplit(imageFFT, fft_Re, fft_Im, System.IntPtr.Zero, System.IntPtr.Zero);

			fft_Re = fft_Re.AbsDiff(new Gray(0));
			// Image<Gray, byte> result = fft_Re.Convert<Gray, byte>();

			return fft_Re;
		}

		/// <summary>
		/// Inverse Reconstruction
		/// </summary>
		/// <param name="img"></param>
		/// <param name="motion"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		public static Image<Gray, float> InverseReconstruction(Image<Bgr, byte> img, bool motion, float size) {
			//IMG FFT
			IntPtr imageFFT;
			Size s = WienerFunc.GetFFTReAndIm(img, out imageFFT);

			//Filter FFT
			Image<Gray, float> filter = null;
			if(motion) {
				filter = MotionBlurTF();
				filter = PadWithZeros(filter, s);
			} else {
				filter = FFT.GenerateGaussianMask(new Size(45, 45), size); //filter = FFT.GenerateGaussianMask(s, size);// GenerateFilterMask(new Size(size, size), false, size / 2);
				filter = PadWithZeros(filter, s);
			}

			IntPtr filterFFT;
			s = WienerFunc.GetFFTReAndIm(filter, out filterFFT);

			//G(u,v)/H(u,v)
			IntPtr imgOut = DivideComplexMatrix(imageFFT, filterFFT, s);

			//get fft inverse

			Image<Gray, float> imageOut = FFT.FFT2shift(WienerFunc.GetFFT_InverseReAndIm(imgOut, s), true);

			ShowSingleIMG.ShowIMGStatic(imageOut);


			return imageOut;
		}

		/// <summary>
		/// inverse reconstruction with Butterworth
		/// </summary>
		/// <param name="img"></param>
		/// <param name="motion"></param>
		/// <param name="size"></param>
		/// <param name="sizeBW"></param>
		/// <returns></returns>
		public static Image<Gray, float> InverseReconstructionButterworth(Image<Bgr, byte> img, bool motion, float size, float sizeBW) {
			//IMG FFT
			IntPtr imageFFT;
			Size s = WienerFunc.GetFFTReAndIm(img, out imageFFT);

			//Filter FFT
			Image<Gray, float> filter = null;
			if(motion) {
				filter = MotionBlurTF();
				filter = PadWithZeros(filter, s);
			} else {
				filter = FFT.GenerateGaussianMask(new Size(45, 45), size); //filter = FFT.GenerateGaussianMask(s, size);// GenerateFilterMask(new Size(size, size), false, size / 2);
				filter = PadWithZeros(filter, s);
			}



			IntPtr filterFFT;
			s = WienerFunc.GetFFTReAndIm(filter, out filterFFT);

			// butterworth
			Image<Gray, float> filterBWRe = GenerateButterworth(s, sizeBW);
			Image<Gray, float> filterBWIm = filterBWRe.CopyBlank();
			IntPtr filterBWFFT = CvInvoke.cvCreateImage(s, Emgu.CV.CvEnum.IPL_DEPTH.IPL_DEPTH_32F, 2);
			ShowSingleIMG.ShowIMGStatic(filterBWRe);
			//s = WienerFunc.GetFFTReAndIm(filterBW, out filterBWFFT);
			CvInvoke.cvMerge(filterBWRe, filterBWIm, System.IntPtr.Zero, System.IntPtr.Zero, filterBWFFT);

			// T(u,v) = B(u,v) / H(u,v)
			filterFFT = DivideComplexMatrix(filterBWFFT, filterFFT, s);

			//G(u,v) * T(u,v)
			IntPtr imgOut = CvInvoke.cvCreateImage(s, Emgu.CV.CvEnum.IPL_DEPTH.IPL_DEPTH_32F, 2);
			CvInvoke.cvMulSpectrums(imageFFT, filterFFT, imgOut, Emgu.CV.CvEnum.MUL_SPECTRUMS_TYPE.DEFAULT);

			//get fft inverse
			Image<Gray, float> imageOut = WienerFunc.GetFFT_InverseReAndIm(imgOut, s);

			//release
			CvInvoke.cvReleaseImage(ref imageFFT);
			CvInvoke.cvReleaseImage(ref filterFFT);
			CvInvoke.cvReleaseImage(ref filterBWFFT);
			CvInvoke.cvReleaseImage(ref imgOut);

			// ShowSingleIMG.ShowIMGStatic(imageOut);
			return imageOut;
		}

		/// <summary>
		/// Calculate image FFT and return real and imaginary 
		/// </summary>
		/// <param name="img"></param>
		/// <param name="fft_Re"></param>
		/// <param name="fft_Im"></param>
		public static Size GetFFTReAndIm(Image<Gray, float> img, out IntPtr imageFFT) {
			// First create image with optimal size and copy the content

			//get optimal size
			int wOptim = CvInvoke.cvGetOptimalDFTSize(img.Width);
			int hOptim = CvInvoke.cvGetOptimalDFTSize(img.Height);

			//create empty image
			Image<Gray, float> src1 = new Image<Gray, float>(wOptim, hOptim);
			src1.SetZero();

			// copy original to src
			src1.ROI = new Rectangle(0, 0, img.Width, img.Height);
			img.Copy(src1, null);
			src1.ROI = Rectangle.Empty;

			// prepare image with 2 channels for DFT
			Image<Gray, float> imgFFT_Re = src1;
			Image<Gray, float> imgFFT_Im = src1.Copy();
			imgFFT_Im.SetZero();

			//merge the 2 channels into one image 
			imageFFT = CvInvoke.cvCreateImage(src1.Size, Emgu.CV.CvEnum.IPL_DEPTH.IPL_DEPTH_32F, 2);
			CvInvoke.cvMerge(imgFFT_Re, imgFFT_Im, System.IntPtr.Zero, System.IntPtr.Zero, imageFFT);

			// calculate DFT 
			CvInvoke.cvDFT(imageFFT, imageFFT, Emgu.CV.CvEnum.CV_DXT.CV_DXT_FORWARD, 0);

			//CvInvoke.cvSplit(imageFFT, imgFFT_Re, imgFFT_Im, System.IntPtr.Zero, System.IntPtr.Zero);
			//ShowIMG.ShowIMGStatic(imgFFT_Re, imgFFT_Im);

			return new Size(wOptim, hOptim);
		}

		/// <summary>
		/// divide two complex images = num x den* / ||den||
		/// </summary>
		/// <param name="imgAuxNum">Numerator</param>
		/// <param name="imgAuxDen">Denominator</param>
		/// <returns></returns>
		private static IntPtr DivideComplexMatrix(IntPtr imgAuxNum, IntPtr imgAuxDen, Size s) {
			Image<Gray, float> imgFFT_ReNum = new Image<Gray, float>(s);
			Image<Gray, float> imgFFT_ImNum = new Image<Gray, float>(s);
			Image<Gray, float> imgFFT_ReDen = new Image<Gray, float>(s);
			Image<Gray, float> imgFFT_ImDen = new Image<Gray, float>(s);
			Image<Gray, float> imgFFT_ReOut = new Image<Gray, float>(s);
			Image<Gray, float> imgFFT_ImOut = new Image<Gray, float>(s);

			//num.den*
			IntPtr imgAux1 = CvInvoke.cvCreateImage(s, Emgu.CV.CvEnum.IPL_DEPTH.IPL_DEPTH_32F, 2);
			CvInvoke.cvMulSpectrums(imgAuxNum, imgAuxDen, imgAux1, Emgu.CV.CvEnum.MUL_SPECTRUMS_TYPE.CV_DXT_MUL_CONJ);

			// den.den*
			IntPtr imgAux2 = CvInvoke.cvCreateImage(s, Emgu.CV.CvEnum.IPL_DEPTH.IPL_DEPTH_32F, 2);
			CvInvoke.cvMulSpectrums(imgAuxDen, imgAuxDen, imgAux2, Emgu.CV.CvEnum.MUL_SPECTRUMS_TYPE.CV_DXT_MUL_CONJ);

			// get Real and Imaginary channels - SPLIT
			CvInvoke.cvSplit(imgAux1, imgFFT_ReNum, imgFFT_ImNum, System.IntPtr.Zero, System.IntPtr.Zero);
			CvInvoke.cvSplit(imgAux2, imgFFT_ReDen, imgFFT_ImDen, System.IntPtr.Zero, System.IntPtr.Zero);

			//num.den* / den.den*
			CvInvoke.cvDiv(imgFFT_ReNum, imgFFT_ReDen, imgFFT_ReOut, 1);
			CvInvoke.cvDiv(imgFFT_ImNum, imgFFT_ReDen, imgFFT_ImOut, 1);

			//prepare output
			IntPtr imageOutFFT = CvInvoke.cvCreateImage(s, Emgu.CV.CvEnum.IPL_DEPTH.IPL_DEPTH_32F, 2);
			CvInvoke.cvMerge(imgFFT_ReOut, imgFFT_ImOut, System.IntPtr.Zero, System.IntPtr.Zero, imageOutFFT);

			//release
			CvInvoke.cvReleaseImage(ref imgAux1);
			CvInvoke.cvReleaseImage(ref imgAux2);

			return imageOutFFT;
		}

		/// <summary>
		/// Add scalar to FFT amplitude
		/// </summary>
		/// <param name="imgFFT"></param>
		/// <param name="snr"></param>
		/// <returns></returns>
		private static IntPtr AddScalarFFT(IntPtr imgFFT, float snr, Size s) {
			Image<Gray, float> imgFFT_Re = new Image<Gray, float>(s);
			Image<Gray, float> imgFFT_Im = new Image<Gray, float>(s);

			// get Real and Imaginary channels
			CvInvoke.cvSplit(imgFFT, imgFFT_Re, imgFFT_Im, System.IntPtr.Zero, System.IntPtr.Zero);
			////   ShowIMG.ShowIMGStatic(imgFFT_Re, imgFFT_Im);
			//   // Get amplitude and Phase Channels
			//   Image<Gray, float> imgFFT_Amp = imgFFT_Re.CopyBlank();
			//   Image<Gray, float> imgFFT_Phase = imgFFT_Re.CopyBlank();
			//   CvInvoke.cvCartToPolar(imgFFT_Re, imgFFT_Im, imgFFT_Amp, imgFFT_Phase, false);
			//   //Add
			imgFFT_Re = imgFFT_Re + snr;

			//// recover image in 2channel format
			//CvInvoke.cvPolarToCart(imgFFT_Amp, imgFFT_Phase, imgFFT_Re, imgFFT_Im, false);

			IntPtr imageOutFFT = CvInvoke.cvCreateImage(s, Emgu.CV.CvEnum.IPL_DEPTH.IPL_DEPTH_32F, 2);
			CvInvoke.cvMerge(imgFFT_Re, imgFFT_Im, System.IntPtr.Zero, System.IntPtr.Zero, imageOutFFT);

			return imageOutFFT;
		}

		/// <summary>
		/// Padding Zeros. It will return the input image resized to "size" padded with zeros.
		/// </summary>
		/// <param name="filter">inout image</param>
		/// <param name="size">new size</param>
		/// <returns></returns>
		private static Image<Gray, float> PadWithZeros(Image<Gray, float> filter, Size size) {
			Image<Gray, float> imgOut = new Image<Gray, float>(size);

			imgOut.ROI = new Rectangle(size.Width / 2 - filter.Width / 2, size.Height / 2 - filter.Height / 2, filter.Width, filter.Height);
			filter.ROI = new Rectangle(0, 0, filter.Width, filter.Height);
			filter.Copy(imgOut, null);

			imgOut.ROI = Rectangle.Empty;
			filter.ROI = Rectangle.Empty;

			return imgOut;
		}

		/// <summary>
		/// Add uniform random noise
		/// </summary>
		/// <param name="img"></param>
		/// <param name="noiseAmp"></param>
		/// <returns></returns>
		public static Image<Bgr, byte> AddNoise(Image<Bgr, byte> img, int noiseAmp) {
			Image<Gray, float> imgaux = img.Clone().Convert<Gray, float>();
			imgaux.SetRandUniform(new MCvScalar(0), new MCvScalar(noiseAmp));
			img = img.Add(imgaux.Convert<Bgr, byte>());
			return img;

		}
	}
}