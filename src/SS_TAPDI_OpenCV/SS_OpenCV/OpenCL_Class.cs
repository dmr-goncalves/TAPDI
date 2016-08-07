using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CASS.OpenCL;
using CASS.Types;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.IO;
using MathNet.Numerics;

namespace SS_OpenCV {
	class OpenCL_Class {

		static CLContext ctx;
		static CLCommandQueue comQ;
		static CLError err;
		static CLDeviceID[] devices;

		static CLKernel kernel_square;
		static CLKernel kernel_mult;
		static CLKernel kernel_negative_direct_memory_accessUncoalesced;
		static CLKernel kernel_negative_direct_memory_accessCoalesced;
		static CLKernel kernel_negative2D;
		static CLKernel kernelRedDetection;
		static CLKernel kernelBlueDetection;
		static CLKernel kernelErode;
		static CLKernel kernelDilate;
		static CLKernel kernelHistogram;
		static CLKernel kernelMatchTemplate;
		static CLKernel kernelSumImgPixels;
		static CLKernel kernelSum2ImgPixels;

		static List<CLMem> ListRedTriangleCLMem = new List<CLMem>();
		static List<CLMem> ListRedCircleCLMem = new List<CLMem>();
		static List<CLMem> ListBlueSquareCLMem = new List<CLMem>();
		static List<CLMem> ListBlueCircleCLMem = new List<CLMem>();

		static int blockSize;

		/// <summary>
		/// Get platform and device info
		/// </summary>
		internal static void GetInfo() {
			try {
				uint num_entries = 0;

				// get device
				CLPlatformID[] platforms = new CLPlatformID[5];

				CLError err = OpenCLDriver.clGetPlatformIDs(5, platforms, ref num_entries);
				if(err != CLError.Success) throw new Exception(err.ToString());
				if(num_entries == 0) throw new Exception("No Platform Entries found!");

				// get platform properties
				byte[] buffer = new byte[1000];
				GCHandle bufferGC = GCHandle.Alloc(buffer, GCHandleType.Pinned);

				for(int i = 0; i < num_entries; i++) {
					SizeT buffSize = new CASS.Types.SizeT(1000);
					err = OpenCLDriver.clGetPlatformInfo(platforms[i], CLPlatformInfo.Name, buffSize, bufferGC.AddrOfPinnedObject(), ref buffSize);
					if(err != CLError.Success) throw new Exception(err.ToString());

					MessageBox.Show("Platform: " + i + "\n" + System.Text.Encoding.ASCII.GetString(buffer));
				}

			} catch(Exception exc) {
				MessageBox.Show(exc.ToString());
			}
		}

		/// <summary>
		/// Get device properties
		/// </summary>
		internal static void GetDeviceProperties(int ID) {
			try {
				uint num_entries = 0;

				// get device
				CLPlatformID[] platforms = new CLPlatformID[5];
				CLError err = OpenCLDriver.clGetPlatformIDs(5, platforms, ref num_entries);
				if(err != CLError.Success) throw new Exception(err.ToString());
				if(num_entries == 0) throw new Exception("No Platform Entries found!");

				//get device ID
				CLDeviceID[] devices = new CLDeviceID[1];
				err = OpenCLDriver.clGetDeviceIDs(platforms[ID], CLDeviceType.All, 1, devices, ref num_entries);
				if(err != CLError.Success) throw new Exception(err.ToString());

				//Get Device Properties
				string result = "";
				byte[] buffer = new byte[1000];

				GCHandle bufferGC = GCHandle.Alloc(buffer, GCHandleType.Pinned);
				SizeT buffSize = new CASS.Types.SizeT(1000), buffSizeOut = new SizeT();

				err = OpenCLDriver.clGetDeviceInfo(devices[0], CLDeviceInfo.Vendor, buffSize, bufferGC.AddrOfPinnedObject(), ref buffSizeOut);
				if(err != CLError.Success) throw new Exception(err.ToString());

				result += CLDeviceInfo.Vendor.ToString() + ": " + Encoding.ASCII.GetString(buffer, 0, buffSizeOut - 1) + "\n";
				buffSize = new CASS.Types.SizeT(1000);
				err = OpenCLDriver.clGetDeviceInfo(devices[0], CLDeviceInfo.Name, buffSize, bufferGC.AddrOfPinnedObject(), ref buffSizeOut);
				if(err != CLError.Success) throw new Exception(err.ToString());
				result += CLDeviceInfo.Name.ToString() + ": " + Encoding.ASCII.GetString(buffer, 0, buffSizeOut - 1) + "\n";

				int[] workDim = new int[1];

				bufferGC = GCHandle.Alloc(workDim, GCHandleType.Pinned);
				buffSize = new CASS.Types.SizeT(sizeof(int));
				err = OpenCLDriver.clGetDeviceInfo(devices[0], CLDeviceInfo.MaxComputeUnits, buffSize, bufferGC.AddrOfPinnedObject(), ref buffSizeOut);
				if(err != CLError.Success) throw new Exception(err.ToString());
				result += CLDeviceInfo.MaxComputeUnits.ToString() + ": " + workDim[0] + "\n";
				err = OpenCLDriver.clGetDeviceInfo(devices[0], CLDeviceInfo.MaxWorkItemDimensions, workDim.Length * sizeof(int), bufferGC.AddrOfPinnedObject(), ref buffSizeOut);
				if(err != CLError.Success) throw new Exception(err.ToString());
				result += CLDeviceInfo.MaxWorkItemDimensions.ToString() + ": " + workDim[0] + "\n";
				SizeT[] sizeWI = new SizeT[workDim[0]];
				bufferGC = GCHandle.Alloc(sizeWI, GCHandleType.Pinned);
				err = OpenCLDriver.clGetDeviceInfo(devices[0], CLDeviceInfo.MaxWorkItemSizes, sizeWI.Length * sizeof(int), bufferGC.AddrOfPinnedObject(), ref buffSizeOut);
				if(err != CLError.Success) throw new Exception(err.ToString());
				result += CLDeviceInfo.MaxWorkItemSizes.ToString() + ": " + sizeWI[0] + "x" + sizeWI[1] + "x" + sizeWI[2] + "\n";
				MessageBox.Show(result, "Device");

			} catch(Exception exc) {
				MessageBox.Show(exc.ToString());
			}
		}

		/// <summary>
		/// Setup OpenCL
		/// </summary>
		internal static void Setup() {
			try {

				uint num_entries = 0;// get platform
				CLPlatformID[] platforms = new CLPlatformID[5];
				err = OpenCLDriver.clGetPlatformIDs(5, platforms, ref num_entries);
				if(err != CLError.Success) throw new Exception(err.ToString());
				if(num_entries == 0) throw new Exception("No Platform Entries found!");

				//get device ID
				devices = new CLDeviceID[1];
				err = OpenCLDriver.clGetDeviceIDs(platforms[1], CLDeviceType.All, 1, devices, ref num_entries);
				if(err != CLError.Success) throw new Exception(err.ToString());

				// create context
				ctx = OpenCLDriver.clCreateContext(null, 1, devices, null, IntPtr.Zero, ref err);

				// create command queue
				comQ = OpenCLDriver.clCreateCommandQueue(ctx, devices[0], 0, ref err);

				string[] progString = new string[1];
				progString[0] = File.ReadAllText(@".\kernels.cl");

				CLProgram program = OpenCLDriver.clCreateProgramWithSource(ctx, 1, progString, null, ref err);
				err = OpenCLDriver.clBuildProgram(program, 1, devices, "", null, IntPtr.Zero);
				if(err != CLError.Success) throw new Exception(err.ToString());

				//create kernel
				kernel_square = OpenCLDriver.clCreateKernel(program, "square", ref err);
				if(err != CLError.Success) throw new Exception(err.ToString());
				kernel_mult = OpenCLDriver.clCreateKernel(program, "multiply", ref err);
				if(err != CLError.Success) throw new Exception(err.ToString());
				kernel_negative_direct_memory_accessUncoalesced = OpenCLDriver.clCreateKernel(program, "negative_uncoalesced", ref err);
				if(err != CLError.Success) throw new Exception(err.ToString());
				kernel_negative_direct_memory_accessCoalesced = OpenCLDriver.clCreateKernel(program, "negative_coalesced", ref err);
				if(err != CLError.Success) throw new Exception(err.ToString());
				kernel_negative2D = OpenCLDriver.clCreateKernel(program, "negative_image2D", ref err);
				if(err != CLError.Success) throw new Exception(err.ToString());
				kernelRedDetection = OpenCLDriver.clCreateKernel(program, "RedDetection", ref err);
				if(err != CLError.Success) throw new Exception(err.ToString());
				kernelBlueDetection = OpenCLDriver.clCreateKernel(program, "BlueDetection", ref err);
				if(err != CLError.Success) throw new Exception(err.ToString());
				kernelErode = OpenCLDriver.clCreateKernel(program, "erode", ref err);
				if(err != CLError.Success) throw new Exception(err.ToString());
				kernelDilate = OpenCLDriver.clCreateKernel(program, "dilate", ref err);
				if(err != CLError.Success) throw new Exception(err.ToString());
				kernelHistogram = OpenCLDriver.clCreateKernel(program, "histogram", ref err);
				if(err != CLError.Success) throw new Exception(err.ToString());
				kernelMatchTemplate = OpenCLDriver.clCreateKernel(program, "matchTemplate", ref err);
				if(err != CLError.Success) throw new Exception(err.ToString());
				kernelSumImgPixels = OpenCLDriver.clCreateKernel(program, "sumImgPixels", ref err);
				if(err != CLError.Success) throw new Exception(err.ToString());
				kernelSum2ImgPixels = OpenCLDriver.clCreateKernel(program, "sum2ImgPixels", ref err);
				if(err != CLError.Success) throw new Exception(err.ToString());

				CLEvent[] eventObjs = new CLEvent[1];
				CLEvent eventObj = new CLEvent();

				foreach(Image<Bgr, byte> imgDB in MainForm.redTriangleDataBase) {
					CLMem temp_buffer = OpenCLDriver.clCreateBuffer(ctx, CLMemFlags.ReadWrite | CLMemFlags.CopyHostPtr, imgDB.MIplImage.imageSize, imgDB.MIplImage.imageData, ref err);
					if(err != CLError.Success) throw new Exception(err.ToString());

					ListRedTriangleCLMem.Add(temp_buffer);

					//err = OpenCLDriver.clEnqueueWriteBuffer(comQ, temp_buffer, CLBool.True, 0, imgDB.MIplImage.imageSize, imgDB.MIplImage.imageData, 0, null, ref eventObj);
					//if(err != CLError.Success) throw new Exception(err.ToString());
				}

				foreach(Image<Bgr, byte> imgDB in MainForm.redCircleDataBase) {
					CLMem temp_buffer = OpenCLDriver.clCreateBuffer(ctx, CLMemFlags.ReadWrite | CLMemFlags.CopyHostPtr, imgDB.MIplImage.imageSize, /*IntPtr.Zero*/imgDB.MIplImage.imageData, ref err);
					if(err != CLError.Success) throw new Exception(err.ToString());

					ListRedCircleCLMem.Add(temp_buffer);

					//err = OpenCLDriver.clEnqueueWriteBuffer(comQ, temp_buffer, CLBool.True, 0, imgDB.MIplImage.imageSize, imgDB.MIplImage.imageData, 0, null, ref eventObj);
					//if(err != CLError.Success) throw new Exception(err.ToString());
				}

				foreach(Image<Bgr, byte> imgDB in MainForm.blueSquareDataBase) {
					CLMem temp_buffer = OpenCLDriver.clCreateBuffer(ctx, CLMemFlags.ReadWrite | CLMemFlags.CopyHostPtr, imgDB.MIplImage.imageSize, imgDB.MIplImage.imageData, ref err);
					if(err != CLError.Success) throw new Exception(err.ToString());

					ListBlueSquareCLMem.Add(temp_buffer);

					//err = OpenCLDriver.clEnqueueWriteBuffer(comQ, temp_buffer, CLBool.True, 0, imgDB.MIplImage.imageSize, imgDB.MIplImage.imageData, 0, null, ref eventObj);
					//if(err != CLError.Success) throw new Exception(err.ToString());
				}

				foreach(Image<Bgr, byte> imgDB in MainForm.blueCircleDataBase) {
					CLMem temp_buffer = OpenCLDriver.clCreateBuffer(ctx, CLMemFlags.ReadWrite | CLMemFlags.CopyHostPtr, imgDB.MIplImage.imageSize, imgDB.MIplImage.imageData, ref err);
					if(err != CLError.Success) throw new Exception(err.ToString());

					ListBlueCircleCLMem.Add(temp_buffer);

					//err = OpenCLDriver.clEnqueueWriteBuffer(comQ, temp_buffer, CLBool.True, 0, imgDB.MIplImage.imageSize, imgDB.MIplImage.imageData, 0, null, ref eventObj);
					//if(err != CLError.Success) throw new Exception(err.ToString());
				}

			} catch(Exception exc) {
				MessageBox.Show(exc.ToString());
			}
		}

		/// <summary>
		/// Release OpenCL objects (final)
		/// </summary>
		internal static void Release() {
			try {
				err = OpenCLDriver.clReleaseCommandQueue(comQ);
				if(err != CLError.Success) throw new Exception(err.ToString());

				err = OpenCLDriver.clReleaseKernel(kernel_square);
				if(err != CLError.Success) throw new Exception(err.ToString());
				err = OpenCLDriver.clReleaseKernel(kernel_mult);
				if(err != CLError.Success) throw new Exception(err.ToString());
				err = OpenCLDriver.clReleaseKernel(kernel_negative_direct_memory_accessUncoalesced);
				if(err != CLError.Success) throw new Exception(err.ToString());
				err = OpenCLDriver.clReleaseKernel(kernel_negative_direct_memory_accessCoalesced);
				if(err != CLError.Success) throw new Exception(err.ToString());
				err = OpenCLDriver.clReleaseKernel(kernel_negative2D);
				if(err != CLError.Success) throw new Exception(err.ToString());
				err = OpenCLDriver.clReleaseKernel(kernelRedDetection);
				if(err != CLError.Success) throw new Exception(err.ToString());
				err = OpenCLDriver.clReleaseKernel(kernelBlueDetection);
				if(err != CLError.Success) throw new Exception(err.ToString());
				err = OpenCLDriver.clReleaseKernel(kernelErode);
				if(err != CLError.Success) throw new Exception(err.ToString());
				err = OpenCLDriver.clReleaseKernel(kernelDilate);
				if(err != CLError.Success) throw new Exception(err.ToString());
				err = OpenCLDriver.clReleaseKernel(kernelHistogram);
				if(err != CLError.Success) throw new Exception(err.ToString());
				err = OpenCLDriver.clReleaseKernel(kernelMatchTemplate);
				if(err != CLError.Success) throw new Exception(err.ToString());
				err = OpenCLDriver.clReleaseKernel(kernelSumImgPixels);
				if(err != CLError.Success) throw new Exception(err.ToString());
				err = OpenCLDriver.clReleaseKernel(kernelSum2ImgPixels);
				if(err != CLError.Success) throw new Exception(err.ToString());

				ListRedCircleCLMem.Clear();
				ListRedTriangleCLMem.Clear();
				ListBlueCircleCLMem.Clear();
				ListBlueSquareCLMem.Clear();

				GC.Collect();

				err = OpenCLDriver.clReleaseContext(ctx);
				if(err != CLError.Success) throw new Exception(err.ToString());
				err = OpenCLDriver.clUnloadCompiler();
				if(err != CLError.Success) throw new Exception(err.ToString());

			} catch(Exception exc) {
				MessageBox.Show(exc.ToString());
			}
		}

		/// <summary>
		/// Square an array
		/// </summary>
		internal static void Square(float[] arr) {
			try {

				GCHandle arrGC = GCHandle.Alloc(arr, GCHandleType.Pinned);

				CLMem bufferFilter = OpenCLDriver.clCreateBuffer(ctx, CLMemFlags.ReadWrite | CLMemFlags.CopyHostPtr, new SizeT(arr.Length * sizeof(float)), arrGC.AddrOfPinnedObject(), ref err);
				if(err != CLError.Success) throw new Exception(err.ToString());

				unsafe
				{
					OpenCLDriver.clSetKernelArg(kernel_square, 0, new SizeT(sizeof(CLMem)), ref bufferFilter);
				}

				//Define grid & execute
				err = OpenCLDriver.clFinish(comQ);
				if(err != CLError.Success) throw new Exception(err.ToString());

				SizeT[] localws = { 5, 1 };
				SizeT[] globalws = { 10, 1 };

				CLEvent eventObj = new CLEvent();
				err = OpenCLDriver.clEnqueueNDRangeKernel(comQ, kernel_square, 2, null, globalws, localws, 0, null, ref eventObj);
				if(err != CLError.Success) throw new Exception(err.ToString());

				CLEvent[] eventObjs = new CLEvent[1];
				eventObjs[0] = eventObj;
				OpenCLDriver.clWaitForEvents(1, eventObjs);

				// read buffer
				err = OpenCLDriver.clEnqueueReadBuffer(comQ, bufferFilter, CLBool.True, 0, 10 * sizeof(float), arrGC.AddrOfPinnedObject(), 0, null, ref eventObj);
				if(err != CLError.Success) throw new Exception(err.ToString());

				err = OpenCLDriver.clReleaseMemObject(bufferFilter);
				if(err != CLError.Success) throw new Exception(err.ToString());
				err = OpenCLDriver.clReleaseEvent(eventObj);
				if(err != CLError.Success) throw new Exception(err.ToString());

				TableForm.ShowTableStatic(arr, "Array");

			} catch(Exception exc) {
				MessageBox.Show(exc.ToString());
			}
		}

		/// <summary>
		/// Multiply an array by a constant value
		/// </summary>
		internal static void Multiply(float[] array, float multiplier) {
			try {

				GCHandle arrGC = GCHandle.Alloc(array, GCHandleType.Pinned);

				CLMem bufferFilter = OpenCLDriver.clCreateBuffer(ctx, CLMemFlags.ReadWrite | CLMemFlags.CopyHostPtr, new SizeT(array.Length * sizeof(float)), arrGC.AddrOfPinnedObject(), ref err);
				if(err != CLError.Success) throw new Exception(err.ToString());

				unsafe
				{
					OpenCLDriver.clSetKernelArg(kernel_mult, 0, new SizeT(sizeof(CLMem)), ref bufferFilter);
					OpenCLDriver.clSetKernelArg(kernel_mult, 1, sizeof(float), ref multiplier);
				}

				//Define grid & execute
				err = OpenCLDriver.clFinish(comQ);
				if(err != CLError.Success) throw new Exception(err.ToString());

				SizeT[] localws = { 5, 1 };
				SizeT[] globalws = { 10, 1 };

				CLEvent eventObj = new CLEvent();
				err = OpenCLDriver.clEnqueueNDRangeKernel(comQ, kernel_mult, 2, null, globalws, localws, 0, null, ref eventObj);
				if(err != CLError.Success) throw new Exception(err.ToString());

				CLEvent[] eventObjs = new CLEvent[1];
				eventObjs[0] = eventObj;
				OpenCLDriver.clWaitForEvents(1, eventObjs);

				// read buffer
				err = OpenCLDriver.clEnqueueReadBuffer(comQ, bufferFilter, CLBool.True, 0, 10 * sizeof(float), arrGC.AddrOfPinnedObject(), 0, null, ref eventObj);
				if(err != CLError.Success) throw new Exception(err.ToString());

				err = OpenCLDriver.clReleaseMemObject(bufferFilter);
				if(err != CLError.Success) throw new Exception(err.ToString());
				err = OpenCLDriver.clReleaseEvent(eventObj);
				if(err != CLError.Success) throw new Exception(err.ToString());

				TableForm.ShowTableStatic(array, "Array");

			} catch(Exception exc) {
				MessageBox.Show(exc.ToString());
			}
		}

		/// <summary>
		/// Negative Coalesced
		/// </summary>
		internal static void NegativeDirectMemoryAccessCoalesced(Image<Bgr, byte> img) {
			try {
				unsafe
				{
					MIplImage m = img.MIplImage;
					byte* dataPtr = (byte*)m.imageData.ToPointer(); // obter apontador do inicio da imagem

					int width = img.Width;
					int height = img.Height;
					int nChan = m.nChannels; // numero de canais 3
					int padding = m.widthStep - m.nChannels * m.width; // alinhamento (padding)

					Image<Bgra, byte> img_bgra = img.Convert<Bgra, byte>();
					MIplImage n = img_bgra.MIplImage;
					padding = n.widthStep - n.nChannels * n.width;

					CLMem bufferFilterIn = OpenCLDriver.clCreateBuffer(ctx, CLMemFlags.ReadOnly | CLMemFlags.CopyHostPtr, img_bgra.MIplImage.imageSize, img_bgra.MIplImage.imageData, ref err);
					if(err != CLError.Success) throw new Exception(err.ToString());
					CLMem bufferFilterOut = OpenCLDriver.clCreateBuffer(ctx, CLMemFlags.ReadWrite | CLMemFlags.UseHostPtr, img_bgra.MIplImage.imageSize, img_bgra.MIplImage.imageData, ref err);
					if(err != CLError.Success) throw new Exception(err.ToString());

					unsafe
					{
						OpenCLDriver.clSetKernelArg(kernel_negative_direct_memory_accessCoalesced, 0, new SizeT(sizeof(CLMem)), ref bufferFilterIn);
						OpenCLDriver.clSetKernelArg(kernel_negative_direct_memory_accessCoalesced, 1, sizeof(int), ref width);
						OpenCLDriver.clSetKernelArg(kernel_negative_direct_memory_accessCoalesced, 2, sizeof(int), ref height);
						OpenCLDriver.clSetKernelArg(kernel_negative_direct_memory_accessCoalesced, 3, sizeof(int), ref padding);
						OpenCLDriver.clSetKernelArg(kernel_negative_direct_memory_accessCoalesced, 4, new SizeT(sizeof(CLMem)), ref bufferFilterOut);
					}

					//Define grid & execute
					err = OpenCLDriver.clFinish(comQ);
					if(err != CLError.Success) throw new Exception(err.ToString());

					blockSize = MainForm.blockSize;

					SizeT[] localws = { blockSize, blockSize };
					SizeT[] globalws = { (int)Math.Ceiling((double)width / blockSize) * blockSize, (int)Math.Ceiling(height / (double)blockSize) * blockSize };

					CLEvent eventObj = new CLEvent();
					err = OpenCLDriver.clEnqueueNDRangeKernel(comQ, kernel_negative_direct_memory_accessCoalesced, 2, null, globalws, localws, 0, null, ref eventObj);
					if(err != CLError.Success) throw new Exception(err.ToString());

					CLEvent[] eventObjs = new CLEvent[1];
					eventObjs[0] = eventObj;
					OpenCLDriver.clWaitForEvents(1, eventObjs);

					// read buffer
					err = OpenCLDriver.clEnqueueReadBuffer(comQ, bufferFilterOut, CLBool.True, 0, img_bgra.MIplImage.imageSize, img_bgra.MIplImage.imageData, 0, null, ref eventObj);
					if(err != CLError.Success) throw new Exception(err.ToString());

					err = OpenCLDriver.clReleaseMemObject(bufferFilterIn);
					if(err != CLError.Success) throw new Exception(err.ToString());
					err = OpenCLDriver.clReleaseMemObject(bufferFilterOut);
					if(err != CLError.Success) throw new Exception(err.ToString());
					err = OpenCLDriver.clReleaseEvent(eventObj);
					if(err != CLError.Success) throw new Exception(err.ToString());

					img = img_bgra.Convert<Bgr, byte>();
				}
			} catch(Exception exc) {
				MessageBox.Show(exc.ToString());
			}
		}

		/// <summary>
		/// Negative Uncoalesced
		/// </summary>
		internal static void NegativeDirectMemoryAccessUncoalesced(Image<Bgr, byte> img) {
			try {
				unsafe
				{
					MIplImage m = img.MIplImage;
					byte* dataPtr = (byte*)m.imageData.ToPointer(); // obter apontador do inicio da imagem

					int width = img.Width;
					int height = img.Height;
					int nChan = m.nChannels; // numero de canais 3
					int padding = m.widthStep - m.nChannels * m.width; // alinhamento (padding)

					CLMem bufferFilterIn = OpenCLDriver.clCreateBuffer(ctx, CLMemFlags.ReadOnly | CLMemFlags.CopyHostPtr, img.MIplImage.imageSize, img.MIplImage.imageData, ref err);
					if(err != CLError.Success) throw new Exception(err.ToString());
					CLMem bufferFilterOut = OpenCLDriver.clCreateBuffer(ctx, CLMemFlags.ReadWrite | CLMemFlags.UseHostPtr, img.MIplImage.imageSize, img.MIplImage.imageData, ref err);
					if(err != CLError.Success) throw new Exception(err.ToString());

					unsafe
					{
						OpenCLDriver.clSetKernelArg(kernel_negative_direct_memory_accessUncoalesced, 0, new SizeT(sizeof(CLMem)), ref bufferFilterIn);
						OpenCLDriver.clSetKernelArg(kernel_negative_direct_memory_accessUncoalesced, 1, sizeof(int), ref width);
						OpenCLDriver.clSetKernelArg(kernel_negative_direct_memory_accessUncoalesced, 2, sizeof(int), ref height);
						OpenCLDriver.clSetKernelArg(kernel_negative_direct_memory_accessUncoalesced, 3, sizeof(int), ref padding);
						OpenCLDriver.clSetKernelArg(kernel_negative_direct_memory_accessUncoalesced, 4, new SizeT(sizeof(CLMem)), ref bufferFilterOut);
					}

					//Define grid & execute
					err = OpenCLDriver.clFinish(comQ);
					if(err != CLError.Success) throw new Exception(err.ToString());

					blockSize = MainForm.blockSize;

					SizeT[] localws = { blockSize, blockSize };
					SizeT[] globalws = { (int)Math.Ceiling((double)width / blockSize) * blockSize, (int)Math.Ceiling(height / (double)blockSize) * blockSize };

					CLEvent eventObj = new CLEvent();
					err = OpenCLDriver.clEnqueueNDRangeKernel(comQ, kernel_negative_direct_memory_accessUncoalesced, 2, null, globalws, localws, 0, null, ref eventObj);
					if(err != CLError.Success) throw new Exception(err.ToString());

					CLEvent[] eventObjs = new CLEvent[1];
					eventObjs[0] = eventObj;
					OpenCLDriver.clWaitForEvents(1, eventObjs);

					// read buffer
					//err = OpenCLDriver.clEnqueueReadBuffer(comQ, bufferFilter, CLBool.True, 0, size * sizeof(byte), arrGC.AddrOfPinnedObject(), 0, null, ref eventObj);
					err = OpenCLDriver.clEnqueueReadBuffer(comQ, bufferFilterOut, CLBool.True, 0, img.MIplImage.imageSize, img.MIplImage.imageData, 0, null, ref eventObj);
					if(err != CLError.Success) throw new Exception(err.ToString());

					err = OpenCLDriver.clReleaseMemObject(bufferFilterIn);
					if(err != CLError.Success) throw new Exception(err.ToString());
					err = OpenCLDriver.clReleaseMemObject(bufferFilterOut);
					if(err != CLError.Success) throw new Exception(err.ToString());
					err = OpenCLDriver.clReleaseEvent(eventObj);
					if(err != CLError.Success) throw new Exception(err.ToString());

				}
			} catch(Exception exc) {
				MessageBox.Show(exc.ToString());
			}
		}

		/// <summary>
		/// Negative Sampler
		/// </summary>
		internal static void Negative2D(Image<Bgr, byte> img) {
			try {
				int width = img.Width;
				int height = img.Height;

				//Define Image Format
				CLImageFormat imgFormat;
				imgFormat.image_channel_data_type = CLChannelType.UnSignedInt8;
				imgFormat.image_channel_order = CLChannelOrder.RGBA;

				Image<Bgra, byte> img_bgra = img.Convert<Bgra, byte>();

				CLMem bufferFilterIn = OpenCLDriver.clCreateImage2D(ctx, CLMemFlags.ReadOnly | CLMemFlags.CopyHostPtr, ref imgFormat, img_bgra.Width, img_bgra.Height, img_bgra.MIplImage.widthStep, img_bgra.MIplImage.imageData, ref err);
				if(err != CLError.Success) throw new Exception(err.ToString());
				CLMem bufferFilterOut = OpenCLDriver.clCreateImage2D(ctx, CLMemFlags.ReadWrite | CLMemFlags.UseHostPtr, ref imgFormat, img_bgra.Width, img_bgra.Height, img_bgra.MIplImage.widthStep, img_bgra.MIplImage.imageData, ref err);
				if(err != CLError.Success) throw new Exception(err.ToString());

				unsafe
				{
					OpenCLDriver.clSetKernelArg(kernel_negative2D, 0, new SizeT(sizeof(CLMem)), ref bufferFilterIn);
					OpenCLDriver.clSetKernelArg(kernel_negative2D, 1, sizeof(CLMem), ref bufferFilterOut);
					OpenCLDriver.clSetKernelArg(kernel_negative2D, 2, sizeof(int), ref width);
					OpenCLDriver.clSetKernelArg(kernel_negative2D, 3, sizeof(int), ref height);
				}

				//Define grid & execute
				err = OpenCLDriver.clFinish(comQ);
				if(err != CLError.Success) throw new Exception(err.ToString());

				blockSize = MainForm.blockSize;

				SizeT[] localws = { blockSize, blockSize };
				SizeT[] globalws = { (int)Math.Ceiling((double)width / blockSize) * blockSize, (int)Math.Ceiling(height / (double)blockSize) * blockSize };

				CLEvent eventObj = new CLEvent();
				err = OpenCLDriver.clEnqueueNDRangeKernel(comQ, kernel_negative2D, 2, null, globalws, localws, 0, null, ref eventObj);
				if(err != CLError.Success) throw new Exception(err.ToString());

				CLEvent[] eventObjs = new CLEvent[1];
				eventObjs[0] = eventObj;
				OpenCLDriver.clWaitForEvents(1, eventObjs);

				SizeT[] origin = { 0, 0, 0 };
				SizeT[] region = { width, height, 1 };
				// read buffer
				err = OpenCLDriver.clEnqueueReadImage(comQ, bufferFilterOut, CLBool.True, origin, region, img_bgra.MIplImage.widthStep, 0, img_bgra.MIplImage.imageData, 0, null, ref eventObj);
				if(err != CLError.Success) throw new Exception(err.ToString());

				err = OpenCLDriver.clReleaseMemObject(bufferFilterIn);
				if(err != CLError.Success) throw new Exception(err.ToString());
				err = OpenCLDriver.clReleaseMemObject(bufferFilterOut);
				if(err != CLError.Success) throw new Exception(err.ToString());
				err = OpenCLDriver.clReleaseEvent(eventObj);
				if(err != CLError.Success) throw new Exception(err.ToString());

				img = img_bgra.Convert<Bgr, byte>();

				//ShowIMG.ShowIMGStatic(img, img);
			} catch(Exception exc) {
				MessageBox.Show(exc.ToString());
			}
		}

		/// <summary>
		/// Bgr to Hsv and Red Binarization
		/// </summary>
		internal static Image<Bgr, byte> RedDetection(Image<Bgr, byte> img) {
			Image<Bgr, byte> imgF = img.Copy();

			try {
				unsafe
				{
					MIplImage m = img.MIplImage;
					byte* dataPtr = (byte*)m.imageData.ToPointer(); // obter apontador do inicio da imagem

					int width = img.Width;
					int height = img.Height;
					int nChan = m.nChannels; // numero de canais 3
					int padding = m.widthStep - m.nChannels * m.width; // alinhamento (padding)
					int size = width * height * 4;

					CLMem bufferFilterIn = OpenCLDriver.clCreateBuffer(ctx, CLMemFlags.ReadOnly | CLMemFlags.CopyHostPtr, img.MIplImage.imageSize, img.MIplImage.imageData, ref err);
					if(err != CLError.Success) throw new Exception(err.ToString());
					CLMem bufferFilterOut = OpenCLDriver.clCreateBuffer(ctx, CLMemFlags.ReadWrite | CLMemFlags.UseHostPtr, imgF.MIplImage.imageSize, imgF.MIplImage.imageData, ref err);
					if(err != CLError.Success) throw new Exception(err.ToString());

					unsafe
					{
						OpenCLDriver.clSetKernelArg(kernelRedDetection, 0, new SizeT(sizeof(CLMem)), ref bufferFilterIn);
						OpenCLDriver.clSetKernelArg(kernelRedDetection, 1, sizeof(int), ref width);
						OpenCLDriver.clSetKernelArg(kernelRedDetection, 2, sizeof(int), ref height);
						OpenCLDriver.clSetKernelArg(kernelRedDetection, 3, sizeof(int), ref padding);
						OpenCLDriver.clSetKernelArg(kernelRedDetection, 4, new SizeT(sizeof(CLMem)), ref bufferFilterOut);
					}

					//Define grid & execute
					err = OpenCLDriver.clFinish(comQ);
					if(err != CLError.Success) throw new Exception(err.ToString());

					blockSize = MainForm.blockSize;

					SizeT[] localws = { blockSize, blockSize };
					SizeT[] globalws = { (int)Math.Ceiling((double)width / blockSize) * blockSize, (int)Math.Ceiling(height / (double)blockSize) * blockSize };

					CLEvent eventObj = new CLEvent();
					err = OpenCLDriver.clEnqueueNDRangeKernel(comQ, kernelRedDetection, 2, null, globalws, localws, 0, null, ref eventObj);
					if(err != CLError.Success) throw new Exception(err.ToString());

					CLEvent[] eventObjs = new CLEvent[1];
					eventObjs[0] = eventObj;
					OpenCLDriver.clWaitForEvents(1, eventObjs);

					// read buffer
					err = OpenCLDriver.clEnqueueReadBuffer(comQ, bufferFilterOut, CLBool.True, 0, imgF.MIplImage.imageSize, imgF.MIplImage.imageData, 0, null, ref eventObj);
					if(err != CLError.Success) throw new Exception(err.ToString());

					err = OpenCLDriver.clReleaseMemObject(bufferFilterIn);
					if(err != CLError.Success) throw new Exception(err.ToString());
					err = OpenCLDriver.clReleaseMemObject(bufferFilterOut);
					if(err != CLError.Success) throw new Exception(err.ToString());
					err = OpenCLDriver.clReleaseEvent(eventObj);
					if(err != CLError.Success) throw new Exception(err.ToString());
				}
			} catch(Exception exc) {
				MessageBox.Show(exc.ToString());
			}
			return imgF;
		}

		/// <summary>
		/// Bgr to Hsv and Blue Binarization
		/// </summary>
		internal static Image<Bgr, byte> BlueDetection(Image<Bgr, byte> img) {
			Image<Bgr, byte> imgF = img.Copy();

			try {
				unsafe
				{
					MIplImage m = img.MIplImage;
					byte* dataPtr = (byte*)m.imageData.ToPointer(); // obter apontador do inicio da imagem

					int width = img.Width;
					int height = img.Height;
					int nChan = m.nChannels; // numero de canais 3
					int padding = m.widthStep - m.nChannels * m.width; // alinhamento (padding)
					int size = width * height * 4;

					CLMem bufferFilterIn = OpenCLDriver.clCreateBuffer(ctx, CLMemFlags.ReadOnly | CLMemFlags.CopyHostPtr, img.MIplImage.imageSize, img.MIplImage.imageData, ref err);
					if(err != CLError.Success) throw new Exception(err.ToString());
					CLMem bufferFilterOut = OpenCLDriver.clCreateBuffer(ctx, CLMemFlags.ReadWrite | CLMemFlags.UseHostPtr, imgF.MIplImage.imageSize, imgF.MIplImage.imageData, ref err);
					if(err != CLError.Success) throw new Exception(err.ToString());

					unsafe
					{
						OpenCLDriver.clSetKernelArg(kernelBlueDetection, 0, new SizeT(sizeof(CLMem)), ref bufferFilterIn);
						OpenCLDriver.clSetKernelArg(kernelBlueDetection, 1, sizeof(int), ref width);
						OpenCLDriver.clSetKernelArg(kernelBlueDetection, 2, sizeof(int), ref height);
						OpenCLDriver.clSetKernelArg(kernelBlueDetection, 3, sizeof(int), ref padding);
						OpenCLDriver.clSetKernelArg(kernelBlueDetection, 4, new SizeT(sizeof(CLMem)), ref bufferFilterOut);
					}

					//Define grid & execute
					err = OpenCLDriver.clFinish(comQ);
					if(err != CLError.Success) throw new Exception(err.ToString());

					blockSize = MainForm.blockSize;

					SizeT[] localws = { blockSize, blockSize };
					SizeT[] globalws = { (int)Math.Ceiling((double)width / blockSize) * blockSize, (int)Math.Ceiling(height / (double)blockSize) * blockSize };

					CLEvent eventObj = new CLEvent();
					err = OpenCLDriver.clEnqueueNDRangeKernel(comQ, kernelBlueDetection, 2, null, globalws, localws, 0, null, ref eventObj);
					if(err != CLError.Success) throw new Exception(err.ToString());

					CLEvent[] eventObjs = new CLEvent[1];
					eventObjs[0] = eventObj;
					OpenCLDriver.clWaitForEvents(1, eventObjs);

					// read buffer
					err = OpenCLDriver.clEnqueueReadBuffer(comQ, bufferFilterOut, CLBool.True, 0, imgF.MIplImage.imageSize, imgF.MIplImage.imageData, 0, null, ref eventObj);
					if(err != CLError.Success) throw new Exception(err.ToString());

					err = OpenCLDriver.clReleaseMemObject(bufferFilterIn);
					if(err != CLError.Success) throw new Exception(err.ToString());
					err = OpenCLDriver.clReleaseMemObject(bufferFilterOut);
					if(err != CLError.Success) throw new Exception(err.ToString());
					err = OpenCLDriver.clReleaseEvent(eventObj);
					if(err != CLError.Success) throw new Exception(err.ToString());
				}
			} catch(Exception exc) {
				MessageBox.Show(exc.ToString());
			}
			return imgF;
		}

		/// <summary>
		/// Erosion
		/// </summary>
		internal static Image<Bgr, byte> Erode(Image<Bgr, byte> img, int iterations) {
			Image<Bgr, byte> imgF = img.Copy();
			try {
				unsafe
				{
					MIplImage m = img.MIplImage;
					byte* dataPtr = (byte*)m.imageData.ToPointer(); // obter apontador do inicio da imagem

					int width = img.Width;
					int height = img.Height;
					int nChan = m.nChannels; // numero de canais 3
					int padding = m.widthStep - m.nChannels * m.width; // alinhamento (padding)
					int size = width * height * 4;

					CLMem bufferFilterIn = OpenCLDriver.clCreateBuffer(ctx, CLMemFlags.ReadWrite | CLMemFlags.UseHostPtr, img.MIplImage.imageSize, img.MIplImage.imageData, ref err);
					if(err != CLError.Success) throw new Exception(err.ToString());
					CLMem bufferFilterOut = OpenCLDriver.clCreateBuffer(ctx, CLMemFlags.ReadWrite | CLMemFlags.UseHostPtr, imgF.MIplImage.imageSize, imgF.MIplImage.imageData, ref err);
					if(err != CLError.Success) throw new Exception(err.ToString());

					CLEvent[] eventObjs = new CLEvent[1];
					CLEvent eventObj = new CLEvent();

					for(int i = 0; i < iterations; i++) {
						unsafe
						{
							OpenCLDriver.clSetKernelArg(kernelErode, 0, new SizeT(sizeof(CLMem)), ref bufferFilterIn);
							OpenCLDriver.clSetKernelArg(kernelErode, 1, sizeof(int), ref width);
							OpenCLDriver.clSetKernelArg(kernelErode, 2, sizeof(int), ref height);
							OpenCLDriver.clSetKernelArg(kernelErode, 3, sizeof(int), ref padding);
							OpenCLDriver.clSetKernelArg(kernelErode, 4, new SizeT(sizeof(CLMem)), ref bufferFilterOut);
						}

						//Define grid & execute
						err = OpenCLDriver.clFinish(comQ);
						if(err != CLError.Success) throw new Exception(err.ToString());

						blockSize = MainForm.blockSize;

						SizeT[] localws = { blockSize, blockSize };
						SizeT[] globalws = { (int)Math.Ceiling((double)width / blockSize) * blockSize, (int)Math.Ceiling(height / (double)blockSize) * blockSize };

						err = OpenCLDriver.clEnqueueNDRangeKernel(comQ, kernelErode, 2, null, globalws, localws, 0, null, ref eventObj);
						if(err != CLError.Success) throw new Exception(err.ToString());

						eventObjs[0] = eventObj;
						OpenCLDriver.clWaitForEvents(1, eventObjs);

						CLMem aux = bufferFilterIn;

						bufferFilterIn = bufferFilterOut;
						bufferFilterOut = aux;
					}

					// read buffer
					err = OpenCLDriver.clEnqueueReadBuffer(comQ, bufferFilterOut, CLBool.True, 0, imgF.MIplImage.imageSize, imgF.MIplImage.imageData, 0, null, ref eventObj);
					if(err != CLError.Success) throw new Exception(err.ToString());

					err = OpenCLDriver.clReleaseMemObject(bufferFilterIn);
					if(err != CLError.Success) throw new Exception(err.ToString());
					err = OpenCLDriver.clReleaseMemObject(bufferFilterOut);
					if(err != CLError.Success) throw new Exception(err.ToString());
					err = OpenCLDriver.clReleaseEvent(eventObj);
					if(err != CLError.Success) throw new Exception(err.ToString());

					//ShowIMG.ShowIMGStatic(img, imgF);
				}
			} catch(Exception exc) {
				MessageBox.Show(exc.ToString());
			}

			return imgF;

		}

		/// <summary>
		/// Dilation
		/// </summary>
		internal static Image<Bgr, byte> Dilate(Image<Bgr, byte> img, int iterations) {
			Image<Bgr, byte> imgF = img.Copy();
			try {
				unsafe
				{
					MIplImage m = img.MIplImage;
					byte* dataPtr = (byte*)m.imageData.ToPointer(); // obter apontador do inicio da imagem

					int width = img.Width;
					int height = img.Height;
					int nChan = m.nChannels; // numero de canais 3
					int padding = m.widthStep - m.nChannels * m.width; // alinhamento (padding)
					int size = width * height * 4;

					CLMem bufferFilterIn = OpenCLDriver.clCreateBuffer(ctx, CLMemFlags.ReadOnly | CLMemFlags.CopyHostPtr, img.MIplImage.imageSize, img.MIplImage.imageData, ref err);
					if(err != CLError.Success) throw new Exception(err.ToString());
					CLMem bufferFilterOut = OpenCLDriver.clCreateBuffer(ctx, CLMemFlags.ReadWrite | CLMemFlags.UseHostPtr, imgF.MIplImage.imageSize, imgF.MIplImage.imageData, ref err);
					if(err != CLError.Success) throw new Exception(err.ToString());

					CLEvent eventObj = new CLEvent();
					CLEvent[] eventObjs = new CLEvent[1];

					for(int i = 0; i < iterations; i++) {
						unsafe
						{
							OpenCLDriver.clSetKernelArg(kernelDilate, 0, new SizeT(sizeof(CLMem)), ref bufferFilterIn);
							OpenCLDriver.clSetKernelArg(kernelDilate, 1, sizeof(int), ref width);
							OpenCLDriver.clSetKernelArg(kernelDilate, 2, sizeof(int), ref height);
							OpenCLDriver.clSetKernelArg(kernelDilate, 3, sizeof(int), ref padding);
							OpenCLDriver.clSetKernelArg(kernelDilate, 4, new SizeT(sizeof(CLMem)), ref bufferFilterOut);
						}

						//Define grid & execute
						err = OpenCLDriver.clFinish(comQ);
						if(err != CLError.Success) throw new Exception(err.ToString());

						blockSize = MainForm.blockSize;

						SizeT[] localws = { blockSize, blockSize };
						SizeT[] globalws = { (int)Math.Ceiling((double)width / blockSize) * blockSize, (int)Math.Ceiling(height / (double)blockSize) * blockSize };

						err = OpenCLDriver.clEnqueueNDRangeKernel(comQ, kernelDilate, 2, null, globalws, localws, 0, null, ref eventObj);
						if(err != CLError.Success) throw new Exception(err.ToString());

						OpenCLDriver.clWaitForEvents(1, eventObjs);

						eventObjs[0] = eventObj;
						CLMem aux = bufferFilterIn;

						bufferFilterIn = bufferFilterOut;
						bufferFilterOut = aux;
					}

					// read buffer
					err = OpenCLDriver.clEnqueueReadBuffer(comQ, bufferFilterOut, CLBool.True, 0, imgF.MIplImage.imageSize, imgF.MIplImage.imageData, 0, null, ref eventObj);
					if(err != CLError.Success) throw new Exception(err.ToString());

					err = OpenCLDriver.clReleaseMemObject(bufferFilterIn);
					if(err != CLError.Success) throw new Exception(err.ToString());
					err = OpenCLDriver.clReleaseMemObject(bufferFilterOut);
					if(err != CLError.Success) throw new Exception(err.ToString());
					err = OpenCLDriver.clReleaseEvent(eventObj);
					if(err != CLError.Success) throw new Exception(err.ToString());

					//ShowIMG.ShowIMGStatic(img, imgF);
				}
			} catch(Exception exc) {
				MessageBox.Show(exc.ToString());
			}

			return imgF;

		}

		/// <summary>
		/// Histogram
		/// </summary>
		internal static int[] Histogram(Image<Bgr, byte> img, bool vertical) {
			int[] array = null;

			try {
				unsafe
				{
					MIplImage m = img.MIplImage;
					byte* dataPtr = (byte*)m.imageData.ToPointer(); // obter apontador do inicio da imagem
					int v;

					if(!vertical) {
						array = new int[img.Height];
						v = 0;
					} else {
						array = new int[img.Width];
						v = 1;
					}

					int width = img.Width;
					int height = img.Height;
					int nChan = m.nChannels; // numero de canais 3
					int padding = m.widthStep - m.nChannels * m.width; // alinhamento (padding)
					int size = width * height * 4;
					GCHandle arrGC = GCHandle.Alloc(array, GCHandleType.Pinned);

					CLMem bufferFilterIn = OpenCLDriver.clCreateBuffer(ctx, CLMemFlags.ReadOnly | CLMemFlags.CopyHostPtr, img.MIplImage.imageSize, img.MIplImage.imageData, ref err);
					if(err != CLError.Success) throw new Exception(err.ToString());
					CLMem bufferFilter = OpenCLDriver.clCreateBuffer(ctx, CLMemFlags.ReadWrite | CLMemFlags.CopyHostPtr, new SizeT(array.Length * sizeof(float)), arrGC.AddrOfPinnedObject(), ref err);
					if(err != CLError.Success) throw new Exception(err.ToString());

					unsafe
					{
						OpenCLDriver.clSetKernelArg(kernelHistogram, 0, new SizeT(sizeof(CLMem)), ref bufferFilterIn);
						OpenCLDriver.clSetKernelArg(kernelHistogram, 1, sizeof(int), ref width);
						OpenCLDriver.clSetKernelArg(kernelHistogram, 2, sizeof(int), ref height);
						OpenCLDriver.clSetKernelArg(kernelHistogram, 3, sizeof(int), ref padding);
						OpenCLDriver.clSetKernelArg(kernelHistogram, 4, new SizeT(sizeof(CLMem)), ref bufferFilter);
						OpenCLDriver.clSetKernelArg(kernelHistogram, 5, sizeof(int), ref v);

					}

					//Define grid & execute
					err = OpenCLDriver.clFinish(comQ);
					if(err != CLError.Success) throw new Exception(err.ToString());

					blockSize = MainForm.blockSize;

					SizeT[] localws = { blockSize, blockSize };
					SizeT[] globalws = { (int)Math.Ceiling((double)width / blockSize) * blockSize, (int)Math.Ceiling(height / (double)blockSize) * blockSize };

					CLEvent eventObj = new CLEvent();
					err = OpenCLDriver.clEnqueueNDRangeKernel(comQ, kernelHistogram, 2, null, globalws, localws, 0, null, ref eventObj);
					if(err != CLError.Success) throw new Exception(err.ToString());

					CLEvent[] eventObjs = new CLEvent[1];
					eventObjs[0] = eventObj;
					OpenCLDriver.clWaitForEvents(1, eventObjs);

					// read buffer
					err = OpenCLDriver.clEnqueueReadBuffer(comQ, bufferFilter, CLBool.True, 0, array.Length * sizeof(int), arrGC.AddrOfPinnedObject(), 0, null, ref eventObj);
					if(err != CLError.Success) throw new Exception(err.ToString());

					err = OpenCLDriver.clReleaseMemObject(bufferFilterIn);
					if(err != CLError.Success) throw new Exception(err.ToString());
					err = OpenCLDriver.clReleaseMemObject(bufferFilter);
					if(err != CLError.Success) throw new Exception(err.ToString());
					err = OpenCLDriver.clReleaseEvent(eventObj);
					if(err != CLError.Success) throw new Exception(err.ToString());

					//TableForm.ShowTableStatic(array, "Array");

					return array;
				}
			} catch(Exception exc) {
				MessageBox.Show(exc.ToString());
			}

			return array;
		}

		/// <summary>
		/// TemplateMatching For testing
		/// </summary>
		internal static int[] TemplateMatching(Image<Bgr, byte> img, char color, int shape) {
			int[] image_coeff = new int[2];
			Image<Bgr, byte> imgF = img.Copy();
			try {
				unsafe
				{
					if(color == 'r') {
						img = ImageClass.ImageEnhancement(img, 'r');
					} else if(color == 'b') {
						img = ImageClass.ImageEnhancement(img, 'b');
					}

					MIplImage m = img.MIplImage;
					byte* dataPtr = (byte*)m.imageData.ToPointer(); // obter apontador do inicio da imagem

					int width = img.Width;
					int height = img.Height;
					int nChan = m.nChannels; // numero de canais 3
					int padding = m.widthStep - m.nChannels * m.width; // alinhamento (padding)
					int size = width * height * 4;

					CLMem bufferFilterIn = OpenCLDriver.clCreateBuffer(ctx, CLMemFlags.ReadWrite | CLMemFlags.CopyHostPtr, img.MIplImage.imageSize, img.MIplImage.imageData, ref err);
					if(err != CLError.Success) throw new Exception(err.ToString());

					CLMem bufferImgF = OpenCLDriver.clCreateBuffer(ctx, CLMemFlags.ReadWrite | CLMemFlags.CopyHostPtr, imgF.MIplImage.imageSize, imgF.MIplImage.imageData, ref err);
					if(err != CLError.Success) throw new Exception(err.ToString());

					blockSize = MainForm.blockSize;

					SizeT[] localws = { blockSize, blockSize };
					SizeT[] globalws = { (int)Math.Ceiling((double)width / blockSize) * blockSize, (int)Math.Ceiling(height / (double)blockSize) * blockSize };

					CLEvent eventObj = new CLEvent();
					CLEvent[] eventObjs = new CLEvent[1];

					int[] nominator = new int[1];
					int[] sum_T_2 = new int[1];
					int[] sum_I_2 = new int[1];
					int[] sum_T = new int[1];
					int sum_I = 0;

					GCHandle nomGC = GCHandle.Alloc(nominator, GCHandleType.Pinned);
					GCHandle sumT2GC = GCHandle.Alloc(sum_T_2, GCHandleType.Pinned);
					GCHandle sumI2GC = GCHandle.Alloc(sum_I_2, GCHandleType.Pinned);
					GCHandle sumTGC = GCHandle.Alloc(sum_T, GCHandleType.Pinned);

					CLMem nomBuffer = OpenCLDriver.clCreateBuffer(ctx, CLMemFlags.ReadWrite | CLMemFlags.CopyHostPtr, new SizeT(nominator.Length * sizeof(int)), nomGC.AddrOfPinnedObject(), ref err);
					if(err != CLError.Success) throw new Exception(err.ToString());
					CLMem sumTBuffer = OpenCLDriver.clCreateBuffer(ctx, CLMemFlags.ReadWrite | CLMemFlags.CopyHostPtr, new SizeT(sum_T.Length * sizeof(int)), sumTGC.AddrOfPinnedObject(), ref err);
					if(err != CLError.Success) throw new Exception(err.ToString());
					CLMem sumT2Buffer = OpenCLDriver.clCreateBuffer(ctx, CLMemFlags.ReadWrite | CLMemFlags.CopyHostPtr, new SizeT(sum_T_2.Length * sizeof(int)), sumT2GC.AddrOfPinnedObject(), ref err);
					if(err != CLError.Success) throw new Exception(err.ToString());
					CLMem sumI2Buffer = OpenCLDriver.clCreateBuffer(ctx, CLMemFlags.ReadWrite | CLMemFlags.CopyHostPtr, new SizeT(sum_I_2.Length * sizeof(int)), sumI2GC.AddrOfPinnedObject(), ref err);
					if(err != CLError.Success) throw new Exception(err.ToString());

					float factor = 1.0f / (width * height);

					int x, y, i;
					int best_i = 0;

					byte* auxPtr;

					for(y = 0; y < height; y++) { //OBTER SOMATORIO DE T E I
						for(x = 0; x < width; x++) {
							auxPtr = (dataPtr + y * m.widthStep + x * nChan);

							sum_I += (int)((float)auxPtr[0] + auxPtr[1] + auxPtr[2])/3;
						}
					}

					switch(color) {
						case 'r':
							switch(shape) {
								case 0:
									for(i = 0; i < ListRedCircleCLMem.Count; i++) {
										CLMem aux = ListRedCircleCLMem[i];

										unsafe
										{
											OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 0, new SizeT(sizeof(CLMem)), ref bufferFilterIn);
											OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 1, sizeof(int), ref width);
											OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 2, sizeof(int), ref height);
											OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 3, sizeof(int), ref padding);
											OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 4, new SizeT(sizeof(CLMem)), ref sumTBuffer);
										}

										//Define grid & execute
										err = OpenCLDriver.clFinish(comQ);
										if(err != CLError.Success) throw new Exception(err.ToString());

										err = OpenCLDriver.clEnqueueNDRangeKernel(comQ, kernelSumImgPixels, 2, null, globalws, localws, 0, null, ref eventObj);
										if(err != CLError.Success) throw new Exception(err.ToString());

										OpenCLDriver.clWaitForEvents(1, eventObjs);

										eventObjs[0] = eventObj;

										err = OpenCLDriver.clEnqueueReadBuffer(comQ, sumTBuffer, CLBool.True, 0, sum_T.Length * sizeof(int), sumTGC.AddrOfPinnedObject(), 0, null, ref eventObj);
										if(err != CLError.Success) throw new Exception(err.ToString());

										unsafe
										{
											OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 0, new SizeT(sizeof(CLMem)), ref bufferFilterIn);
											OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 1, new SizeT(sizeof(CLMem)), ref aux);
											OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 2, sizeof(int), ref width);
											OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 3, sizeof(int), ref height);
											OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 4, sizeof(int), ref padding);
											OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 5, sizeof(float), ref factor);
											OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 6, sizeof(float), ref factor);
											OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 7, sizeof(int), ref sum_I);
											OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 8, sizeof(int), ref sum_T[0]);
											OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 9, new SizeT(sizeof(CLMem)), ref nomBuffer);
											OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 10, new SizeT(sizeof(CLMem)), ref sumT2Buffer);
											OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 11, new SizeT(sizeof(CLMem)), ref sumI2Buffer);
										}

										//Define grid & execute
										err = OpenCLDriver.clFinish(comQ);
										if(err != CLError.Success) throw new Exception(err.ToString());

										err = OpenCLDriver.clEnqueueNDRangeKernel(comQ, kernelMatchTemplate, 2, null, globalws, localws, 0, null, ref eventObj);
										if(err != CLError.Success) throw new Exception(err.ToString());

										OpenCLDriver.clWaitForEvents(1, eventObjs);

										eventObjs[0] = eventObj;

										err = OpenCLDriver.clEnqueueReadBuffer(comQ, nomBuffer, CLBool.True, 0, sizeof(int), nomGC.AddrOfPinnedObject(), 0, null, ref eventObj);
										if(err != CLError.Success) throw new Exception(err.ToString());
										err = OpenCLDriver.clEnqueueReadBuffer(comQ, sumT2Buffer, CLBool.True, 0, sizeof(int), sumT2GC.AddrOfPinnedObject(), 0, null, ref eventObj);
										if(err != CLError.Success) throw new Exception(err.ToString());
										err = OpenCLDriver.clEnqueueReadBuffer(comQ, sumI2Buffer, CLBool.True, 0, sizeof(int), sumI2GC.AddrOfPinnedObject(), 0, null, ref eventObj);
										if(err != CLError.Success) throw new Exception(err.ToString());

										double big_mul = (double)sum_T_2[0] * sum_I_2[0];

										double R = nominator[0] / (Math.Sqrt(big_mul));
									}

									break;
								default:
									break;
							}
							break;

						default:
							break;
					}

					nomGC.Free();
					sumTGC.Free();
					sumT2GC.Free();
					sumI2GC.Free();

					err = OpenCLDriver.clReleaseMemObject(bufferFilterIn);
					if(err != CLError.Success) throw new Exception(err.ToString());
					err = OpenCLDriver.clReleaseMemObject(sumTBuffer);
					if(err != CLError.Success) throw new Exception(err.ToString());
					err = OpenCLDriver.clReleaseEvent(eventObj);
					if(err != CLError.Success) throw new Exception(err.ToString());

					//ShowIMG.ShowIMGStatic(img, imgF);
				}
			} catch(Exception exc) {
				MessageBox.Show(exc.ToString());
			}

			return image_coeff;

		}

		/// <summary>
		/// Color Conversion, Binarization, Projections and Template Matching
		/// </summary>
		internal static void ColorConvertion_Binarization_Projections_TemplateMatching(Image<Bgr, byte> img, out Image<Bgr, byte> red_bin, out Image<Bgr, byte> blue_bin) {

			List<int[]> projections = new List<int[]>();
			Image<Bgr, byte> imgI = img.Copy();
			Image<Bgr, byte> imgF = img.Copy();
			Image<Bgr, byte> img_test = img.Copy();
			int[] array_vert_red = new int[img.Width];
			int[] array_horz_red = new int[img.Height];
			int[] array_vert_blue = new int[img.Width];
			int[] array_horz_blue = new int[img.Height];
			List<int> finalShapeIdentifiedList = new List<int>();
			MCvFont f = new MCvFont(FONT.CV_FONT_HERSHEY_DUPLEX, 3.0, 4.0);

			int min_size = img.Width / 15;

			red_bin = img.Copy();
			blue_bin = img.Copy();

			try {
				unsafe
				{
					MIplImage m = img.MIplImage;
					byte* dataPtr = (byte*)m.imageData.ToPointer(); // obter apontador do inicio da imagem

					int width = img.Width;
					int height = img.Height;
					int nChan = m.nChannels; // numero de canais 3
					int padding = m.widthStep - m.nChannels * m.width; // alinhamento (padding)
					int size = width * height * 4;
					int i, j, o, p, x, y;

					GCHandle arrGC1 = GCHandle.Alloc(array_vert_red, GCHandleType.Pinned);
					GCHandle arrGC2 = GCHandle.Alloc(array_horz_red, GCHandleType.Pinned);

					GCHandle arrGC3 = GCHandle.Alloc(array_vert_blue, GCHandleType.Pinned);
					GCHandle arrGC4 = GCHandle.Alloc(array_horz_blue, GCHandleType.Pinned);

					CLMem bufferFilterIn = OpenCLDriver.clCreateBuffer(ctx, CLMemFlags.ReadWrite | CLMemFlags.CopyHostPtr, imgI.MIplImage.imageSize, imgI.MIplImage.imageData, ref err);
					if(err != CLError.Success) throw new Exception(err.ToString());

					CLMem bufferFilterOut = OpenCLDriver.clCreateBuffer(ctx, CLMemFlags.ReadWrite | CLMemFlags.CopyHostPtr, imgF.MIplImage.imageSize, imgF.MIplImage.imageData, ref err);
					if(err != CLError.Success) throw new Exception(err.ToString());

					CLMem bufferArrayVertRed = OpenCLDriver.clCreateBuffer(ctx, CLMemFlags.ReadWrite | CLMemFlags.CopyHostPtr, new SizeT(array_vert_red.Length * sizeof(int)), arrGC1.AddrOfPinnedObject(), ref err);
					if(err != CLError.Success) throw new Exception(err.ToString());
					CLMem bufferArrayHorzRed = OpenCLDriver.clCreateBuffer(ctx, CLMemFlags.ReadWrite | CLMemFlags.CopyHostPtr, new SizeT(array_horz_red.Length * sizeof(int)), arrGC2.AddrOfPinnedObject(), ref err);
					if(err != CLError.Success) throw new Exception(err.ToString());

					CLMem bufferArrayVertBlue = OpenCLDriver.clCreateBuffer(ctx, CLMemFlags.ReadWrite | CLMemFlags.CopyHostPtr, new SizeT(array_vert_blue.Length * sizeof(int)), arrGC3.AddrOfPinnedObject(), ref err);
					if(err != CLError.Success) throw new Exception(err.ToString());
					CLMem bufferArrayHorzBlue = OpenCLDriver.clCreateBuffer(ctx, CLMemFlags.ReadWrite | CLMemFlags.CopyHostPtr, new SizeT(array_horz_blue.Length * sizeof(int)), arrGC4.AddrOfPinnedObject(), ref err);
					if(err != CLError.Success) throw new Exception(err.ToString());

					CLEvent eventObj = new CLEvent();
					CLEvent[] eventObjs = new CLEvent[1];

					blockSize = MainForm.blockSize;

					SizeT[] localws = { blockSize, blockSize };
					SizeT[] globalws = { (int)Math.Ceiling((double)width / blockSize) * blockSize, (int)Math.Ceiling(height / (double)blockSize) * blockSize };

					///--------- RED ---------///*********************************************************************************************************************************

					/*--- RED DETECTION ---*/ /*----------------------------------------*/
					unsafe
					{
						OpenCLDriver.clSetKernelArg(kernelRedDetection, 0, new SizeT(sizeof(CLMem)), ref bufferFilterIn);
						OpenCLDriver.clSetKernelArg(kernelRedDetection, 1, sizeof(int), ref width);
						OpenCLDriver.clSetKernelArg(kernelRedDetection, 2, sizeof(int), ref height);
						OpenCLDriver.clSetKernelArg(kernelRedDetection, 3, sizeof(int), ref padding);
						OpenCLDriver.clSetKernelArg(kernelRedDetection, 4, new SizeT(sizeof(CLMem)), ref bufferFilterOut);
					}

					//Define grid & execute
					err = OpenCLDriver.clFinish(comQ);
					if(err != CLError.Success) throw new Exception(err.ToString());

					err = OpenCLDriver.clEnqueueNDRangeKernel(comQ, kernelRedDetection, 2, null, globalws, localws, 0, null, ref eventObj);
					if(err != CLError.Success) throw new Exception(err.ToString());

					eventObjs[0] = eventObj;
					OpenCLDriver.clWaitForEvents(1, eventObjs);

					/*--- RED DETECTION ---*/ /*----------------------------------------*/

					/*--- ERODE ---*/ /*----------------------------------------*/

					for(i = 0; i < 5; i++) {
						unsafe
						{
							OpenCLDriver.clSetKernelArg(kernelErode, 0, new SizeT(sizeof(CLMem)), ref bufferFilterOut);
							OpenCLDriver.clSetKernelArg(kernelErode, 1, sizeof(int), ref width);
							OpenCLDriver.clSetKernelArg(kernelErode, 2, sizeof(int), ref height);
							OpenCLDriver.clSetKernelArg(kernelErode, 3, sizeof(int), ref padding);
							OpenCLDriver.clSetKernelArg(kernelErode, 4, new SizeT(sizeof(CLMem)), ref bufferFilterIn);
						}

						//Define grid & execute
						err = OpenCLDriver.clFinish(comQ);
						if(err != CLError.Success) throw new Exception(err.ToString());

						err = OpenCLDriver.clEnqueueNDRangeKernel(comQ, kernelErode, 2, null, globalws, localws, 0, null, ref eventObj);
						if(err != CLError.Success) throw new Exception(err.ToString());

						eventObjs[0] = eventObj;
						OpenCLDriver.clWaitForEvents(1, eventObjs);

						CLMem aux = bufferFilterIn;

						bufferFilterIn = bufferFilterOut;
						bufferFilterOut = aux;
					}

					/*--- ERODE ---*/ /*----------------------------------------*/

					/*--- DILATE ---*/ /*----------------------------------------*/

					for(i = 0; i < 5; i++) {
						unsafe
						{
							OpenCLDriver.clSetKernelArg(kernelDilate, 0, new SizeT(sizeof(CLMem)), ref bufferFilterOut);
							OpenCLDriver.clSetKernelArg(kernelDilate, 1, sizeof(int), ref width);
							OpenCLDriver.clSetKernelArg(kernelDilate, 2, sizeof(int), ref height);
							OpenCLDriver.clSetKernelArg(kernelDilate, 3, sizeof(int), ref padding);
							OpenCLDriver.clSetKernelArg(kernelDilate, 4, new SizeT(sizeof(CLMem)), ref bufferFilterIn);
						}

						//Define grid & execute
						err = OpenCLDriver.clFinish(comQ);
						if(err != CLError.Success) throw new Exception(err.ToString());

						err = OpenCLDriver.clEnqueueNDRangeKernel(comQ, kernelDilate, 2, null, globalws, localws, 0, null, ref eventObj);
						if(err != CLError.Success) throw new Exception(err.ToString());

						OpenCLDriver.clWaitForEvents(1, eventObjs);

						eventObjs[0] = eventObj;

						CLMem aux = bufferFilterIn;

						bufferFilterIn = bufferFilterOut;
						bufferFilterOut = aux;
					}

					/*--- DILATE ---*/ /*----------------------------------------*/

					/*--- VERTICAL HISTOGRAM ---*/ /*----------------------------------------*/

					int v = 1;
					unsafe
					{
						OpenCLDriver.clSetKernelArg(kernelHistogram, 0, new SizeT(sizeof(CLMem)), ref bufferFilterOut);
						OpenCLDriver.clSetKernelArg(kernelHistogram, 1, sizeof(int), ref width);
						OpenCLDriver.clSetKernelArg(kernelHistogram, 2, sizeof(int), ref height);
						OpenCLDriver.clSetKernelArg(kernelHistogram, 3, sizeof(int), ref padding);
						OpenCLDriver.clSetKernelArg(kernelHistogram, 4, new SizeT(sizeof(CLMem)), ref bufferArrayVertRed);
						OpenCLDriver.clSetKernelArg(kernelHistogram, 5, sizeof(int), ref v);

					}

					//Define grid & execute
					err = OpenCLDriver.clFinish(comQ);
					if(err != CLError.Success) throw new Exception(err.ToString());

					err = OpenCLDriver.clEnqueueNDRangeKernel(comQ, kernelHistogram, 2, null, globalws, localws, 0, null, ref eventObj);
					if(err != CLError.Success) throw new Exception(err.ToString());

					eventObjs[0] = eventObj;
					OpenCLDriver.clWaitForEvents(1, eventObjs);

					err = OpenCLDriver.clEnqueueReadBuffer(comQ, bufferArrayVertRed, CLBool.True, 0, array_vert_red.Length * sizeof(int), arrGC1.AddrOfPinnedObject(), 0, null, ref eventObj);
					if(err != CLError.Success) throw new Exception(err.ToString());

					/*--- VERTICAL HISTOGRAM ---*/ /*----------------------------------------*/

					/*--- HORIZONTAL HISTOGRAM ---*/ /*----------------------------------------*/

					v = 0;
					unsafe
					{
						OpenCLDriver.clSetKernelArg(kernelHistogram, 0, new SizeT(sizeof(CLMem)), ref bufferFilterOut);
						OpenCLDriver.clSetKernelArg(kernelHistogram, 1, sizeof(int), ref width);
						OpenCLDriver.clSetKernelArg(kernelHistogram, 2, sizeof(int), ref height);
						OpenCLDriver.clSetKernelArg(kernelHistogram, 3, sizeof(int), ref padding);
						OpenCLDriver.clSetKernelArg(kernelHistogram, 4, new SizeT(sizeof(CLMem)), ref bufferArrayHorzRed);
						OpenCLDriver.clSetKernelArg(kernelHistogram, 5, sizeof(int), ref v);
					}

					//Define grid & execute
					err = OpenCLDriver.clFinish(comQ);
					if(err != CLError.Success) throw new Exception(err.ToString());

					err = OpenCLDriver.clEnqueueNDRangeKernel(comQ, kernelHistogram, 2, null, globalws, localws, 0, null, ref eventObj);
					if(err != CLError.Success) throw new Exception(err.ToString());

					eventObjs[0] = eventObj;
					OpenCLDriver.clWaitForEvents(1, eventObjs);

					err = OpenCLDriver.clEnqueueReadBuffer(comQ, bufferArrayHorzRed, CLBool.True, 0, array_horz_red.Length * sizeof(int), arrGC2.AddrOfPinnedObject(), 0, null, ref eventObj);
					if(err != CLError.Success) throw new Exception(err.ToString());

					/*--- HORIZONTAL HISTOGRAM ---*/ /*----------------------------------------*/

					err = OpenCLDriver.clEnqueueReadBuffer(comQ, bufferFilterOut, CLBool.True, 0, imgF.MIplImage.imageSize, imgF.MIplImage.imageData, 0, null, ref eventObj);
					if(err != CLError.Success) throw new Exception(err.ToString());

					err = OpenCLDriver.clEnqueueReadBuffer(comQ, bufferFilterOut, CLBool.True, 0, red_bin.MIplImage.imageSize, red_bin.MIplImage.imageData, 0, null, ref eventObj);
					if(err != CLError.Success) throw new Exception(err.ToString());

					////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

					int[] arrayXhist = array_vert_red;

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
					int[] arrayYhist = array_horz_red;

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
						imgROI = imgF.Copy(new System.Drawing.Rectangle(objects[i], objects[i + 2], (objects[i + 1] - objects[i]), (objects[i + 3] - objects[i + 2])));

						//vertical histogram
						arrayXhist = ImageClass.HistogramXY(imgROI, 'x');
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
						arrayYhist = ImageClass.HistogramXY(imgROI, 'y');
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

					double[] coeff = new double[3];
					double[] hist_x;
					double[] hist_y;

					for(i = 0; i < finalObjects.Length; i += 4) {
						imgROI = imgF.Copy(new System.Drawing.Rectangle(finalObjects[i], finalObjects[i + 2], (finalObjects[i + 1] - finalObjects[i]), (finalObjects[i + 3] - finalObjects[i + 2])));
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
					}

					/*---IDENTIFIES WHAT SHAPE IS THE SIGN---*/

					///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

					///--------- RED ---------///*********************************************************************************************************************************

					imgI = img.Copy();
					imgF = img.Copy();

					err = OpenCLDriver.clReleaseMemObject(bufferFilterIn);
					if(err != CLError.Success) throw new Exception(err.ToString());
					err = OpenCLDriver.clReleaseMemObject(bufferFilterOut);
					if(err != CLError.Success) throw new Exception(err.ToString());

					bufferFilterIn = OpenCLDriver.clCreateBuffer(ctx, CLMemFlags.ReadWrite | CLMemFlags.CopyHostPtr, imgI.MIplImage.imageSize, imgI.MIplImage.imageData, ref err);
					if(err != CLError.Success) throw new Exception(err.ToString());

					bufferFilterOut = OpenCLDriver.clCreateBuffer(ctx, CLMemFlags.ReadWrite | CLMemFlags.CopyHostPtr, imgF.MIplImage.imageSize, imgF.MIplImage.imageData, ref err);
					if(err != CLError.Success) throw new Exception(err.ToString());

					///--------- BLUE ---------///*********************************************************************************************************************************

					/*--- BLUE DETECTION ---*/ /*----------------------------------------*/
					unsafe
					{
						OpenCLDriver.clSetKernelArg(kernelBlueDetection, 0, new SizeT(sizeof(CLMem)), ref bufferFilterIn);
						OpenCLDriver.clSetKernelArg(kernelBlueDetection, 1, sizeof(int), ref width);
						OpenCLDriver.clSetKernelArg(kernelBlueDetection, 2, sizeof(int), ref height);
						OpenCLDriver.clSetKernelArg(kernelBlueDetection, 3, sizeof(int), ref padding);
						OpenCLDriver.clSetKernelArg(kernelBlueDetection, 4, new SizeT(sizeof(CLMem)), ref bufferFilterOut);
					}

					//Define grid & execute
					err = OpenCLDriver.clFinish(comQ);
					if(err != CLError.Success) throw new Exception(err.ToString());

					err = OpenCLDriver.clEnqueueNDRangeKernel(comQ, kernelBlueDetection, 2, null, globalws, localws, 0, null, ref eventObj);
					if(err != CLError.Success) throw new Exception(err.ToString());

					eventObjs[0] = eventObj;
					OpenCLDriver.clWaitForEvents(1, eventObjs);

					/*--- BLUE DETECTION ---*/ /*----------------------------------------*/

					/*--- ERODE ---*/ /*----------------------------------------*/

					for(i = 0; i < 5; i++) {
						unsafe
						{
							OpenCLDriver.clSetKernelArg(kernelErode, 0, new SizeT(sizeof(CLMem)), ref bufferFilterOut);
							OpenCLDriver.clSetKernelArg(kernelErode, 1, sizeof(int), ref width);
							OpenCLDriver.clSetKernelArg(kernelErode, 2, sizeof(int), ref height);
							OpenCLDriver.clSetKernelArg(kernelErode, 3, sizeof(int), ref padding);
							OpenCLDriver.clSetKernelArg(kernelErode, 4, new SizeT(sizeof(CLMem)), ref bufferFilterIn);
						}

						//Define grid & execute
						err = OpenCLDriver.clFinish(comQ);
						if(err != CLError.Success) throw new Exception(err.ToString());

						err = OpenCLDriver.clEnqueueNDRangeKernel(comQ, kernelErode, 2, null, globalws, localws, 0, null, ref eventObj);
						if(err != CLError.Success) throw new Exception(err.ToString());

						eventObjs[0] = eventObj;
						OpenCLDriver.clWaitForEvents(1, eventObjs);

						CLMem aux = bufferFilterIn;

						bufferFilterIn = bufferFilterOut;
						bufferFilterOut = aux;
					}

					/*--- ERODE ---*/ /*----------------------------------------*/

					/*--- DILATE ---*/ /*----------------------------------------*/

					for(i = 0; i < 5; i++) {
						unsafe
						{
							OpenCLDriver.clSetKernelArg(kernelDilate, 0, new SizeT(sizeof(CLMem)), ref bufferFilterOut);
							OpenCLDriver.clSetKernelArg(kernelDilate, 1, sizeof(int), ref width);
							OpenCLDriver.clSetKernelArg(kernelDilate, 2, sizeof(int), ref height);
							OpenCLDriver.clSetKernelArg(kernelDilate, 3, sizeof(int), ref padding);
							OpenCLDriver.clSetKernelArg(kernelDilate, 4, new SizeT(sizeof(CLMem)), ref bufferFilterIn);
						}

						//Define grid & execute
						err = OpenCLDriver.clFinish(comQ);
						if(err != CLError.Success) throw new Exception(err.ToString());

						err = OpenCLDriver.clEnqueueNDRangeKernel(comQ, kernelDilate, 2, null, globalws, localws, 0, null, ref eventObj);
						if(err != CLError.Success) throw new Exception(err.ToString());

						OpenCLDriver.clWaitForEvents(1, eventObjs);

						eventObjs[0] = eventObj;

						CLMem aux = bufferFilterIn;

						bufferFilterIn = bufferFilterOut;
						bufferFilterOut = aux;
					}

					/*--- DILATE ---*/ /*----------------------------------------*/

					/*--- VERTICAL HISTOGRAM ---*/ /*----------------------------------------*/

					v = 1;
					unsafe
					{
						OpenCLDriver.clSetKernelArg(kernelHistogram, 0, new SizeT(sizeof(CLMem)), ref bufferFilterOut);
						OpenCLDriver.clSetKernelArg(kernelHistogram, 1, sizeof(int), ref width);
						OpenCLDriver.clSetKernelArg(kernelHistogram, 2, sizeof(int), ref height);
						OpenCLDriver.clSetKernelArg(kernelHistogram, 3, sizeof(int), ref padding);
						OpenCLDriver.clSetKernelArg(kernelHistogram, 4, new SizeT(sizeof(CLMem)), ref bufferArrayVertBlue);
						OpenCLDriver.clSetKernelArg(kernelHistogram, 5, sizeof(int), ref v);

					}

					//Define grid & execute
					err = OpenCLDriver.clFinish(comQ);
					if(err != CLError.Success) throw new Exception(err.ToString());

					err = OpenCLDriver.clEnqueueNDRangeKernel(comQ, kernelHistogram, 2, null, globalws, localws, 0, null, ref eventObj);
					if(err != CLError.Success) throw new Exception(err.ToString());

					eventObjs[0] = eventObj;
					OpenCLDriver.clWaitForEvents(1, eventObjs);

					err = OpenCLDriver.clEnqueueReadBuffer(comQ, bufferArrayVertBlue, CLBool.True, 0, array_vert_blue.Length * sizeof(int), arrGC3.AddrOfPinnedObject(), 0, null, ref eventObj);
					if(err != CLError.Success) throw new Exception(err.ToString());

					/*--- VERTICAL HISTOGRAM ---*/ /*----------------------------------------*/

					/*--- HORIZONTAL HISTOGRAM ---*/ /*----------------------------------------*/

					v = 0;
					unsafe
					{
						OpenCLDriver.clSetKernelArg(kernelHistogram, 0, new SizeT(sizeof(CLMem)), ref bufferFilterOut);
						OpenCLDriver.clSetKernelArg(kernelHistogram, 1, sizeof(int), ref width);
						OpenCLDriver.clSetKernelArg(kernelHistogram, 2, sizeof(int), ref height);
						OpenCLDriver.clSetKernelArg(kernelHistogram, 3, sizeof(int), ref padding);
						OpenCLDriver.clSetKernelArg(kernelHistogram, 4, new SizeT(sizeof(CLMem)), ref bufferArrayHorzBlue);
						OpenCLDriver.clSetKernelArg(kernelHistogram, 5, sizeof(int), ref v);
					}

					//Define grid & execute
					err = OpenCLDriver.clFinish(comQ);
					if(err != CLError.Success) throw new Exception(err.ToString());

					err = OpenCLDriver.clEnqueueNDRangeKernel(comQ, kernelHistogram, 2, null, globalws, localws, 0, null, ref eventObj);
					if(err != CLError.Success) throw new Exception(err.ToString());

					eventObjs[0] = eventObj;
					OpenCLDriver.clWaitForEvents(1, eventObjs);

					err = OpenCLDriver.clEnqueueReadBuffer(comQ, bufferArrayHorzBlue, CLBool.True, 0, array_horz_blue.Length * sizeof(int), arrGC4.AddrOfPinnedObject(), 0, null, ref eventObj);
					if(err != CLError.Success) throw new Exception(err.ToString());

					/*--- HORIZONTAL HISTOGRAM ---*/ /*----------------------------------------*/

					err = OpenCLDriver.clEnqueueReadBuffer(comQ, bufferFilterOut, CLBool.True, 0, imgF.MIplImage.imageSize, imgF.MIplImage.imageData, 0, null, ref eventObj);
					if(err != CLError.Success) throw new Exception(err.ToString());

					err = OpenCLDriver.clEnqueueReadBuffer(comQ, bufferFilterOut, CLBool.True, 0, blue_bin.MIplImage.imageSize, blue_bin.MIplImage.imageData, 0, null, ref eventObj);
					if(err != CLError.Success) throw new Exception(err.ToString());

					////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

					arrayXhist = array_vert_blue;

					//lists for keeping detected objects
					objectsX1X2 = new List<int>();
					objectsY1Y2 = new List<int>();

					//BEGIN 1ST LEVEL OF SEGMENTATION	/*--------------- - -----------------*/	/*--------------- - -----------------*/

					same_object = 0;
					object_area = 0;
					last_coordenate = 0;

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
					arrayX = objectsX1X2.ToArray();

					//horizontal histogram
					arrayYhist = array_horz_blue;

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
					arrayY = objectsY1Y2.ToArray();

					firstSegmentationObjectList = new List<int>();

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
					objects = firstSegmentationObjectList.ToArray();

					//BEGIN 2ND LEVEL OF SEGMENTATION	/*--------------- - -----------------*/	/*--------------- - -----------------*/

					secondSegmentationObjectList = new List<int>();
					imgROI = null;

					for(i = 0; i < objects.Length; i += 4) {

						objectsX1X2.Clear();
						objectsY1Y2.Clear();

						//gets ROI of image and does the same algorithm as before
						imgROI = imgF.Copy(new System.Drawing.Rectangle(objects[i], objects[i + 2], (objects[i + 1] - objects[i]), (objects[i + 3] - objects[i + 2])));

						//vertical histogram
						arrayXhist = ImageClass.HistogramXY(imgROI, 'x');
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
						arrayYhist = ImageClass.HistogramXY(imgROI, 'y');
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

					//END 2ND LEVEL OF SEGMENTATION	/*--------------- - -----------------*/	/*--------------- - -----------------*/

					ratioObjects = secondSegmentationObjectList.ToArray();

					/*---REMOVE OBJECTS THAT AREN'T SQUARES---*/

					ratioCleanList = new List<int>();

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

					finalObjects = ratioCleanList.ToArray();

					/*---IDENTIFIES WHAT SHAPE IS THE SIGN---*/
					
					coeff = new double[3];

					for(i = 0; i < finalObjects.Length; i += 4) {
						imgROI = imgF.Copy(new System.Drawing.Rectangle(finalObjects[i], finalObjects[i + 2], (finalObjects[i + 1] - finalObjects[i]), (finalObjects[i + 3] - finalObjects[i + 2])));
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
					}

					int[] finalShape = finalShapeIdentifiedList.ToArray();

					/*---IDENTIFIES WHAT SHAPE IS THE SIGN---*/

					///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

					///--------- BLUE ---------///*********************************************************************************************************************************

					///--------- TEMPLATE MATCHING ---------///*********************************************************************************************************************************
					int color, shape, h;
					MIplImage z;
					int[] nominator = new int[1];
					int[] sum_T_2 = new int[1];
					int[] sum_I_2 = new int[1];
					int[] sum_T = new int[1];
					int sum_I = 0, bestIndex = 0, bestList = 0;
					double big_mul, R, bestR = 0;
					byte* auxPtr;
					float factor;

					Image<Bgr, byte> match;

					for(i = 0; i < finalShape.Length; i += 6) {

						imgROI = img.Copy(new System.Drawing.Rectangle(finalShape[i], finalShape[i + 2], (finalShape[i + 1] - finalShape[i]), (finalShape[i + 3] - finalShape[i + 2])));
						imgROI = imgROI.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

						match = imgROI;

						color = finalShape[i + 4];
						shape = finalShape[i + 5];

						if(color == 0) {
							imgROI = ImageClass.ImageEnhancement(imgROI, 'r');
						} else if(color == 1) {
							imgROI = ImageClass.ImageEnhancement(imgROI, 'b');
						}

						z = imgROI.MIplImage;
						dataPtr = (byte*)z.imageData.ToPointer(); // obter apontador do inicio da imagem

						width = imgROI.Width;
						height = imgROI.Height;
						nChan = z.nChannels; // numero de canais 3
						padding = z.widthStep - z.nChannels * z.width; // alinhamento (padding)

						err = OpenCLDriver.clReleaseMemObject(bufferFilterIn);
						if(err != CLError.Success) throw new Exception(err.ToString());

						bufferFilterIn = OpenCLDriver.clCreateBuffer(ctx, CLMemFlags.ReadWrite | CLMemFlags.CopyHostPtr, imgROI.MIplImage.imageSize, imgROI.MIplImage.imageData, ref err);
						if(err != CLError.Success) throw new Exception(err.ToString());

						bestR = 0;
						sum_I = 0;
						nominator[0] = 0;
						sum_T_2[0] = 0;
						sum_I_2[0] = 0;
						sum_T[0] = 0;

						GCHandle nomGC = GCHandle.Alloc(nominator, GCHandleType.Pinned);
						GCHandle sumT2GC = GCHandle.Alloc(sum_T_2, GCHandleType.Pinned);
						GCHandle sumI2GC = GCHandle.Alloc(sum_I_2, GCHandleType.Pinned);
						GCHandle sumTGC = GCHandle.Alloc(sum_T, GCHandleType.Pinned);

						CLMem nomBuffer = OpenCLDriver.clCreateBuffer(ctx, CLMemFlags.ReadWrite | CLMemFlags.CopyHostPtr, new SizeT(nominator.Length * sizeof(int)), nomGC.AddrOfPinnedObject(), ref err);
						if(err != CLError.Success) throw new Exception(err.ToString());
						CLMem sumTBuffer = OpenCLDriver.clCreateBuffer(ctx, CLMemFlags.ReadWrite | CLMemFlags.CopyHostPtr, new SizeT(sum_T.Length * sizeof(int)), sumTGC.AddrOfPinnedObject(), ref err);
						if(err != CLError.Success) throw new Exception(err.ToString());
						CLMem sumT2Buffer = OpenCLDriver.clCreateBuffer(ctx, CLMemFlags.ReadWrite | CLMemFlags.CopyHostPtr, new SizeT(sum_T_2.Length * sizeof(int)), sumT2GC.AddrOfPinnedObject(), ref err);
						if(err != CLError.Success) throw new Exception(err.ToString());
						CLMem sumI2Buffer = OpenCLDriver.clCreateBuffer(ctx, CLMemFlags.ReadWrite | CLMemFlags.CopyHostPtr, new SizeT(sum_I_2.Length * sizeof(int)), sumI2GC.AddrOfPinnedObject(), ref err);
						if(err != CLError.Success) throw new Exception(err.ToString());

						factor = 1.0f / (width * height);

						for(y = 0; y < height; y++) { //OBTER SOMATORIO DE T E I
							for(x = 0; x < width; x++) {
								auxPtr = (dataPtr + y * z.widthStep + x * nChan);

								sum_I += (int)((float)auxPtr[0] + auxPtr[1] + auxPtr[2]) / 3;
							}
						}

						switch(color) {
							case 0: //RED
								switch(shape) {
									case 0:
										for(h = 0; h < ListRedCircleCLMem.Count; h++) {
											CLMem aux = ListRedCircleCLMem[h];

											nominator[0] = 0;
											sum_T_2[0] = 0;
											sum_I_2[0] = 0;
											sum_T[0] = 0;

											err = OpenCLDriver.clEnqueueWriteBuffer(comQ, sumI2Buffer, CLBool.True, 0, sum_I_2.Length * sizeof(int), sumI2GC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());
											err = OpenCLDriver.clEnqueueWriteBuffer(comQ, sumT2Buffer, CLBool.True, 0, sum_T_2.Length * sizeof(int), sumT2GC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());
											err = OpenCLDriver.clEnqueueWriteBuffer(comQ, sumTBuffer, CLBool.True, 0, sum_T.Length * sizeof(int), sumTGC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());
											err = OpenCLDriver.clEnqueueWriteBuffer(comQ, nomBuffer, CLBool.True, 0, nominator.Length * sizeof(int), nomGC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());

											unsafe
											{
												OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 0, new SizeT(sizeof(CLMem)), ref aux);
												OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 1, sizeof(int), ref width);
												OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 2, sizeof(int), ref height);
												OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 3, sizeof(int), ref padding);
												OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 4, new SizeT(sizeof(CLMem)), ref sumTBuffer);
											}

											//Define grid & execute
											err = OpenCLDriver.clFinish(comQ);
											if(err != CLError.Success) throw new Exception(err.ToString());

											err = OpenCLDriver.clEnqueueNDRangeKernel(comQ, kernelSumImgPixels, 2, null, globalws, localws, 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());

											OpenCLDriver.clWaitForEvents(1, eventObjs);

											eventObjs[0] = eventObj;

											err = OpenCLDriver.clEnqueueReadBuffer(comQ, sumTBuffer, CLBool.True, 0, sum_T.Length * sizeof(int), sumTGC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());

											unsafe
											{
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 0, new SizeT(sizeof(CLMem)), ref bufferFilterIn);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 1, new SizeT(sizeof(CLMem)), ref aux);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 2, sizeof(int), ref width);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 3, sizeof(int), ref height);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 4, sizeof(int), ref padding);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 5, sizeof(float), ref factor);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 6, sizeof(float), ref factor);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 7, sizeof(int), ref sum_I);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 8, sizeof(int), ref sum_T[0]);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 9, new SizeT(sizeof(CLMem)), ref nomBuffer);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 10, new SizeT(sizeof(CLMem)), ref sumT2Buffer);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 11, new SizeT(sizeof(CLMem)), ref sumI2Buffer);
											}

											//Define grid & execute
											err = OpenCLDriver.clFinish(comQ);
											if(err != CLError.Success) throw new Exception(err.ToString());

											err = OpenCLDriver.clEnqueueNDRangeKernel(comQ, kernelMatchTemplate, 2, null, globalws, localws, 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());

											OpenCLDriver.clWaitForEvents(1, eventObjs);

											eventObjs[0] = eventObj;

											err = OpenCLDriver.clEnqueueReadBuffer(comQ, nomBuffer, CLBool.True, 0, nominator.Length * sizeof(int), nomGC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());
											err = OpenCLDriver.clEnqueueReadBuffer(comQ, sumT2Buffer, CLBool.True, 0, sum_T_2.Length * sizeof(int), sumT2GC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());
											err = OpenCLDriver.clEnqueueReadBuffer(comQ, sumI2Buffer, CLBool.True, 0, sum_I_2.Length * sizeof(int), sumI2GC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());

											big_mul = (double)sum_T_2[0] * sum_I_2[0];

											R = nominator[0] / (Math.Sqrt(big_mul));

											if(R > bestR) {
												bestR = R;
												bestIndex = h;
												bestList = 0;
												//err = OpenCLDriver.clEnqueueReadBuffer(comQ, aux, CLBool.True, 0, match.MIplImage.imageSize, match.MIplImage.imageData, 0, null, ref eventObj);
												//if(err != CLError.Success) throw new Exception(err.ToString());
											}
										}

										break;
									case 3:
										for(h = 0; h < ListRedTriangleCLMem.Count; h++) {
											CLMem aux = ListRedTriangleCLMem[h];

											nominator[0] = 0;
											sum_T_2[0] = 0;
											sum_I_2[0] = 0;
											sum_T[0] = 0;

											err = OpenCLDriver.clEnqueueWriteBuffer(comQ, sumI2Buffer, CLBool.True, 0, sum_I_2.Length * sizeof(int), sumI2GC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());
											err = OpenCLDriver.clEnqueueWriteBuffer(comQ, sumT2Buffer, CLBool.True, 0, sum_T_2.Length * sizeof(int), sumT2GC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());
											err = OpenCLDriver.clEnqueueWriteBuffer(comQ, sumTBuffer, CLBool.True, 0, sum_T.Length * sizeof(int), sumTGC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());
											err = OpenCLDriver.clEnqueueWriteBuffer(comQ, nomBuffer, CLBool.True, 0, nominator.Length * sizeof(int), nomGC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());

											unsafe
											{
												OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 0, new SizeT(sizeof(CLMem)), ref aux);
												OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 1, sizeof(int), ref width);
												OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 2, sizeof(int), ref height);
												OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 3, sizeof(int), ref padding);
												OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 4, new SizeT(sizeof(CLMem)), ref sumTBuffer);
											}

											//Define grid & execute
											err = OpenCLDriver.clFinish(comQ);
											if(err != CLError.Success) throw new Exception(err.ToString());

											err = OpenCLDriver.clEnqueueNDRangeKernel(comQ, kernelSumImgPixels, 2, null, globalws, localws, 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());

											OpenCLDriver.clWaitForEvents(1, eventObjs);

											eventObjs[0] = eventObj;

											err = OpenCLDriver.clEnqueueReadBuffer(comQ, sumTBuffer, CLBool.True, 0, sum_T.Length * sizeof(int), sumTGC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());

											unsafe
											{
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 0, new SizeT(sizeof(CLMem)), ref bufferFilterIn);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 1, new SizeT(sizeof(CLMem)), ref aux);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 2, sizeof(int), ref width);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 3, sizeof(int), ref height);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 4, sizeof(int), ref padding);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 5, sizeof(float), ref factor);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 6, sizeof(float), ref factor);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 7, sizeof(int), ref sum_I);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 8, sizeof(int), ref sum_T[0]);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 9, new SizeT(sizeof(CLMem)), ref nomBuffer);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 10, new SizeT(sizeof(CLMem)), ref sumT2Buffer);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 11, new SizeT(sizeof(CLMem)), ref sumI2Buffer);
											}

											//Define grid & execute
											err = OpenCLDriver.clFinish(comQ);
											if(err != CLError.Success) throw new Exception(err.ToString());

											err = OpenCLDriver.clEnqueueNDRangeKernel(comQ, kernelMatchTemplate, 2, null, globalws, localws, 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());

											OpenCLDriver.clWaitForEvents(1, eventObjs);

											eventObjs[0] = eventObj;

											err = OpenCLDriver.clEnqueueReadBuffer(comQ, nomBuffer, CLBool.True, 0, nominator.Length * sizeof(int), nomGC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());
											err = OpenCLDriver.clEnqueueReadBuffer(comQ, sumT2Buffer, CLBool.True, 0, sum_T_2.Length * sizeof(int), sumT2GC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());
											err = OpenCLDriver.clEnqueueReadBuffer(comQ, sumI2Buffer, CLBool.True, 0, sum_I_2.Length * sizeof(int), sumI2GC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());

											big_mul = (double)sum_T_2[0] * sum_I_2[0];

											R = nominator[0] / (Math.Sqrt(big_mul));

											if(R > bestR) {
												bestR = R;
												bestIndex = h;
												bestList = 1;
												//err = OpenCLDriver.clEnqueueReadBuffer(comQ, aux, CLBool.True, 0, match.MIplImage.imageSize, match.MIplImage.imageData, 0, null, ref eventObj);
												//if(err != CLError.Success) throw new Exception(err.ToString());
											}
										}
										break;
									default:
										for(h = 0; h < ListRedCircleCLMem.Count; h++) {
											CLMem aux = ListRedCircleCLMem[h];

											nominator[0] = 0;
											sum_T_2[0] = 0;
											sum_I_2[0] = 0;
											sum_T[0] = 0;

											err = OpenCLDriver.clEnqueueWriteBuffer(comQ, sumI2Buffer, CLBool.True, 0, sum_I_2.Length * sizeof(int), sumI2GC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());
											err = OpenCLDriver.clEnqueueWriteBuffer(comQ, sumT2Buffer, CLBool.True, 0, sum_T_2.Length * sizeof(int), sumT2GC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());
											err = OpenCLDriver.clEnqueueWriteBuffer(comQ, sumTBuffer, CLBool.True, 0, sum_T.Length * sizeof(int), sumTGC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());
											err = OpenCLDriver.clEnqueueWriteBuffer(comQ, nomBuffer, CLBool.True, 0, nominator.Length * sizeof(int), nomGC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());

											unsafe
											{
												OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 0, new SizeT(sizeof(CLMem)), ref aux);
												OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 1, sizeof(int), ref width);
												OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 2, sizeof(int), ref height);
												OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 3, sizeof(int), ref padding);
												OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 4, new SizeT(sizeof(CLMem)), ref sumTBuffer);
											}

											//Define grid & execute
											err = OpenCLDriver.clFinish(comQ);
											if(err != CLError.Success) throw new Exception(err.ToString());

											err = OpenCLDriver.clEnqueueNDRangeKernel(comQ, kernelSumImgPixels, 2, null, globalws, localws, 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());

											OpenCLDriver.clWaitForEvents(1, eventObjs);

											eventObjs[0] = eventObj;

											err = OpenCLDriver.clEnqueueReadBuffer(comQ, sumTBuffer, CLBool.True, 0, sum_T.Length * sizeof(int), sumTGC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());

											unsafe
											{
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 0, new SizeT(sizeof(CLMem)), ref bufferFilterIn);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 1, new SizeT(sizeof(CLMem)), ref aux);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 2, sizeof(int), ref width);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 3, sizeof(int), ref height);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 4, sizeof(int), ref padding);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 5, sizeof(float), ref factor);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 6, sizeof(float), ref factor);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 7, sizeof(int), ref sum_I);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 8, sizeof(int), ref sum_T[0]);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 9, new SizeT(sizeof(CLMem)), ref nomBuffer);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 10, new SizeT(sizeof(CLMem)), ref sumT2Buffer);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 11, new SizeT(sizeof(CLMem)), ref sumI2Buffer);
											}

											//Define grid & execute
											err = OpenCLDriver.clFinish(comQ);
											if(err != CLError.Success) throw new Exception(err.ToString());

											err = OpenCLDriver.clEnqueueNDRangeKernel(comQ, kernelMatchTemplate, 2, null, globalws, localws, 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());

											OpenCLDriver.clWaitForEvents(1, eventObjs);

											eventObjs[0] = eventObj;

											err = OpenCLDriver.clEnqueueReadBuffer(comQ, nomBuffer, CLBool.True, 0, nominator.Length * sizeof(int), nomGC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());
											err = OpenCLDriver.clEnqueueReadBuffer(comQ, sumT2Buffer, CLBool.True, 0, sum_T_2.Length * sizeof(int), sumT2GC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());
											err = OpenCLDriver.clEnqueueReadBuffer(comQ, sumI2Buffer, CLBool.True, 0, sum_I_2.Length * sizeof(int), sumI2GC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());

											big_mul = (double)sum_T_2[0] * sum_I_2[0];

											R = nominator[0] / (Math.Sqrt(big_mul));

											if(R > bestR) {
												bestR = R;
												bestIndex = h;
												bestList = 0;
												//err = OpenCLDriver.clEnqueueReadBuffer(comQ, aux, CLBool.True, 0, match.MIplImage.imageSize, match.MIplImage.imageData, 0, null, ref eventObj);
												//if(err != CLError.Success) throw new Exception(err.ToString());
											}
										}

										for(h = 0; h < ListRedTriangleCLMem.Count; h++) {
											CLMem aux = ListRedTriangleCLMem[h];

											nominator[0] = 0;
											sum_T_2[0] = 0;
											sum_I_2[0] = 0;
											sum_T[0] = 0;

											err = OpenCLDriver.clEnqueueWriteBuffer(comQ, sumI2Buffer, CLBool.True, 0, sum_I_2.Length * sizeof(int), sumI2GC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());
											err = OpenCLDriver.clEnqueueWriteBuffer(comQ, sumT2Buffer, CLBool.True, 0, sum_T_2.Length * sizeof(int), sumT2GC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());
											err = OpenCLDriver.clEnqueueWriteBuffer(comQ, sumTBuffer, CLBool.True, 0, sum_T.Length * sizeof(int), sumTGC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());
											err = OpenCLDriver.clEnqueueWriteBuffer(comQ, nomBuffer, CLBool.True, 0, nominator.Length * sizeof(int), nomGC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());

											unsafe
											{
												OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 0, new SizeT(sizeof(CLMem)), ref aux);
												OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 1, sizeof(int), ref width);
												OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 2, sizeof(int), ref height);
												OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 3, sizeof(int), ref padding);
												OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 4, new SizeT(sizeof(CLMem)), ref sumTBuffer);
											}

											//Define grid & execute
											err = OpenCLDriver.clFinish(comQ);
											if(err != CLError.Success) throw new Exception(err.ToString());

											err = OpenCLDriver.clEnqueueNDRangeKernel(comQ, kernelSumImgPixels, 2, null, globalws, localws, 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());

											OpenCLDriver.clWaitForEvents(1, eventObjs);

											eventObjs[0] = eventObj;

											err = OpenCLDriver.clEnqueueReadBuffer(comQ, sumTBuffer, CLBool.True, 0, sum_T.Length * sizeof(int), sumTGC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());

											unsafe
											{
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 0, new SizeT(sizeof(CLMem)), ref bufferFilterIn);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 1, new SizeT(sizeof(CLMem)), ref aux);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 2, sizeof(int), ref width);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 3, sizeof(int), ref height);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 4, sizeof(int), ref padding);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 5, sizeof(float), ref factor);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 6, sizeof(float), ref factor);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 7, sizeof(int), ref sum_I);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 8, sizeof(int), ref sum_T[0]);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 9, new SizeT(sizeof(CLMem)), ref nomBuffer);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 10, new SizeT(sizeof(CLMem)), ref sumT2Buffer);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 11, new SizeT(sizeof(CLMem)), ref sumI2Buffer);
											}

											//Define grid & execute
											err = OpenCLDriver.clFinish(comQ);
											if(err != CLError.Success) throw new Exception(err.ToString());

											err = OpenCLDriver.clEnqueueNDRangeKernel(comQ, kernelMatchTemplate, 2, null, globalws, localws, 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());

											OpenCLDriver.clWaitForEvents(1, eventObjs);

											eventObjs[0] = eventObj;

											err = OpenCLDriver.clEnqueueReadBuffer(comQ, nomBuffer, CLBool.True, 0, nominator.Length * sizeof(int), nomGC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());
											err = OpenCLDriver.clEnqueueReadBuffer(comQ, sumT2Buffer, CLBool.True, 0, sum_T_2.Length * sizeof(int), sumT2GC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());
											err = OpenCLDriver.clEnqueueReadBuffer(comQ, sumI2Buffer, CLBool.True, 0, sum_I_2.Length * sizeof(int), sumI2GC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());

											big_mul = (double)sum_T_2[0] * sum_I_2[0];

											R = nominator[0] / (Math.Sqrt(big_mul));

											if(R > bestR) {
												bestR = R;
												bestIndex = h;
												bestList = 1;
												//err = OpenCLDriver.clEnqueueReadBuffer(comQ, aux, CLBool.True, 0, match.MIplImage.imageSize, match.MIplImage.imageData, 0, null, ref eventObj);
												//if(err != CLError.Success) throw new Exception(err.ToString());
											}
										}

										break;
								}
								break;
							case 1: //BLUE
								switch(shape) {
									case 0:
										for(h = 0; h < ListBlueCircleCLMem.Count; h++) {
											CLMem aux = ListBlueCircleCLMem[h];

											nominator[0] = 0;
											sum_T_2[0] = 0;
											sum_I_2[0] = 0;
											sum_T[0] = 0;

											err = OpenCLDriver.clEnqueueWriteBuffer(comQ, sumI2Buffer, CLBool.True, 0, sum_I_2.Length * sizeof(int), sumI2GC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());
											err = OpenCLDriver.clEnqueueWriteBuffer(comQ, sumT2Buffer, CLBool.True, 0, sum_T_2.Length * sizeof(int), sumT2GC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());
											err = OpenCLDriver.clEnqueueWriteBuffer(comQ, sumTBuffer, CLBool.True, 0, sum_T.Length * sizeof(int), sumTGC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());
											err = OpenCLDriver.clEnqueueWriteBuffer(comQ, nomBuffer, CLBool.True, 0, nominator.Length * sizeof(int), nomGC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());

											unsafe
											{
												OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 0, new SizeT(sizeof(CLMem)), ref aux);
												OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 1, sizeof(int), ref width);
												OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 2, sizeof(int), ref height);
												OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 3, sizeof(int), ref padding);
												OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 4, new SizeT(sizeof(CLMem)), ref sumTBuffer);
											}

											//Define grid & execute
											err = OpenCLDriver.clFinish(comQ);
											if(err != CLError.Success) throw new Exception(err.ToString());

											err = OpenCLDriver.clEnqueueNDRangeKernel(comQ, kernelSumImgPixels, 2, null, globalws, localws, 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());

											OpenCLDriver.clWaitForEvents(1, eventObjs);

											eventObjs[0] = eventObj;

											err = OpenCLDriver.clEnqueueReadBuffer(comQ, sumTBuffer, CLBool.True, 0, sum_T.Length * sizeof(int), sumTGC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());

											unsafe
											{
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 0, new SizeT(sizeof(CLMem)), ref bufferFilterIn);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 1, new SizeT(sizeof(CLMem)), ref aux);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 2, sizeof(int), ref width);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 3, sizeof(int), ref height);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 4, sizeof(int), ref padding);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 5, sizeof(float), ref factor);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 6, sizeof(float), ref factor);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 7, sizeof(int), ref sum_I);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 8, sizeof(int), ref sum_T[0]);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 9, new SizeT(sizeof(CLMem)), ref nomBuffer);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 10, new SizeT(sizeof(CLMem)), ref sumT2Buffer);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 11, new SizeT(sizeof(CLMem)), ref sumI2Buffer);
											}

											//Define grid & execute
											err = OpenCLDriver.clFinish(comQ);
											if(err != CLError.Success) throw new Exception(err.ToString());

											err = OpenCLDriver.clEnqueueNDRangeKernel(comQ, kernelMatchTemplate, 2, null, globalws, localws, 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());

											OpenCLDriver.clWaitForEvents(1, eventObjs);

											eventObjs[0] = eventObj;

											err = OpenCLDriver.clEnqueueReadBuffer(comQ, nomBuffer, CLBool.True, 0, nominator.Length * sizeof(int), nomGC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());
											err = OpenCLDriver.clEnqueueReadBuffer(comQ, sumT2Buffer, CLBool.True, 0, sum_T_2.Length * sizeof(int), sumT2GC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());
											err = OpenCLDriver.clEnqueueReadBuffer(comQ, sumI2Buffer, CLBool.True, 0, sum_I_2.Length * sizeof(int), sumI2GC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());

											big_mul = (double)sum_T_2[0] * sum_I_2[0];

											R = nominator[0] / (Math.Sqrt(big_mul));

											if(R > bestR) {
												bestR = R;
												bestIndex = h;
												bestList = 2;
												//err = OpenCLDriver.clEnqueueReadBuffer(comQ, aux, CLBool.True, 0, match.MIplImage.imageSize, match.MIplImage.imageData, 0, null, ref eventObj);
												//if(err != CLError.Success) throw new Exception(err.ToString());
											}
										}

										break;
									case 4:
										for(h = 0; h < ListBlueSquareCLMem.Count; h++) {
											CLMem aux = ListBlueSquareCLMem[h];

											nominator[0] = 0;
											sum_T_2[0] = 0;
											sum_I_2[0] = 0;
											sum_T[0] = 0;

											err = OpenCLDriver.clEnqueueWriteBuffer(comQ, sumI2Buffer, CLBool.True, 0, sum_I_2.Length * sizeof(int), sumI2GC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());
											err = OpenCLDriver.clEnqueueWriteBuffer(comQ, sumT2Buffer, CLBool.True, 0, sum_T_2.Length * sizeof(int), sumT2GC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());
											err = OpenCLDriver.clEnqueueWriteBuffer(comQ, sumTBuffer, CLBool.True, 0, sum_T.Length * sizeof(int), sumTGC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());
											err = OpenCLDriver.clEnqueueWriteBuffer(comQ, nomBuffer, CLBool.True, 0, nominator.Length * sizeof(int), nomGC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());

											unsafe
											{
												OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 0, new SizeT(sizeof(CLMem)), ref aux);
												OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 1, sizeof(int), ref width);
												OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 2, sizeof(int), ref height);
												OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 3, sizeof(int), ref padding);
												OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 4, new SizeT(sizeof(CLMem)), ref sumTBuffer);
											}

											//Define grid & execute
											err = OpenCLDriver.clFinish(comQ);
											if(err != CLError.Success) throw new Exception(err.ToString());

											err = OpenCLDriver.clEnqueueNDRangeKernel(comQ, kernelSumImgPixels, 2, null, globalws, localws, 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());

											OpenCLDriver.clWaitForEvents(1, eventObjs);

											eventObjs[0] = eventObj;

											err = OpenCLDriver.clEnqueueReadBuffer(comQ, sumTBuffer, CLBool.True, 0, sum_T.Length * sizeof(int), sumTGC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());

											unsafe
											{
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 0, new SizeT(sizeof(CLMem)), ref bufferFilterIn);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 1, new SizeT(sizeof(CLMem)), ref aux);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 2, sizeof(int), ref width);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 3, sizeof(int), ref height);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 4, sizeof(int), ref padding);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 5, sizeof(float), ref factor);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 6, sizeof(float), ref factor);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 7, sizeof(int), ref sum_I);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 8, sizeof(int), ref sum_T[0]);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 9, new SizeT(sizeof(CLMem)), ref nomBuffer);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 10, new SizeT(sizeof(CLMem)), ref sumT2Buffer);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 11, new SizeT(sizeof(CLMem)), ref sumI2Buffer);
											}

											//Define grid & execute
											err = OpenCLDriver.clFinish(comQ);
											if(err != CLError.Success) throw new Exception(err.ToString());

											err = OpenCLDriver.clEnqueueNDRangeKernel(comQ, kernelMatchTemplate, 2, null, globalws, localws, 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());

											OpenCLDriver.clWaitForEvents(1, eventObjs);

											eventObjs[0] = eventObj;

											err = OpenCLDriver.clEnqueueReadBuffer(comQ, nomBuffer, CLBool.True, 0, nominator.Length * sizeof(int), nomGC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());
											err = OpenCLDriver.clEnqueueReadBuffer(comQ, sumT2Buffer, CLBool.True, 0, sum_T_2.Length * sizeof(int), sumT2GC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());
											err = OpenCLDriver.clEnqueueReadBuffer(comQ, sumI2Buffer, CLBool.True, 0, sum_I_2.Length * sizeof(int), sumI2GC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());

											big_mul = (double)sum_T_2[0] * sum_I_2[0];

											R = nominator[0] / (Math.Sqrt(big_mul));

											if(R > bestR) {
												bestR = R;
												bestIndex = h;
												bestList = 3;
												//err = OpenCLDriver.clEnqueueReadBuffer(comQ, aux, CLBool.True, 0, match.MIplImage.imageSize, match.MIplImage.imageData, 0, null, ref eventObj);
												//if(err != CLError.Success) throw new Exception(err.ToString());
											}
										}
										break;
									default:
										for(h = 0; h < ListBlueCircleCLMem.Count; h++) {
											CLMem aux = ListBlueCircleCLMem[h];

											nominator[0] = 0;
											sum_T_2[0] = 0;
											sum_I_2[0] = 0;
											sum_T[0] = 0;

											err = OpenCLDriver.clEnqueueWriteBuffer(comQ, sumI2Buffer, CLBool.True, 0, sum_I_2.Length * sizeof(int), sumI2GC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());
											err = OpenCLDriver.clEnqueueWriteBuffer(comQ, sumT2Buffer, CLBool.True, 0, sum_T_2.Length * sizeof(int), sumT2GC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());
											err = OpenCLDriver.clEnqueueWriteBuffer(comQ, sumTBuffer, CLBool.True, 0, sum_T.Length * sizeof(int), sumTGC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());
											err = OpenCLDriver.clEnqueueWriteBuffer(comQ, nomBuffer, CLBool.True, 0, nominator.Length * sizeof(int), nomGC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());

											unsafe
											{
												OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 0, new SizeT(sizeof(CLMem)), ref aux);
												OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 1, sizeof(int), ref width);
												OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 2, sizeof(int), ref height);
												OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 3, sizeof(int), ref padding);
												OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 4, new SizeT(sizeof(CLMem)), ref sumTBuffer);
											}

											//Define grid & execute
											err = OpenCLDriver.clFinish(comQ);
											if(err != CLError.Success) throw new Exception(err.ToString());

											err = OpenCLDriver.clEnqueueNDRangeKernel(comQ, kernelSumImgPixels, 2, null, globalws, localws, 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());

											OpenCLDriver.clWaitForEvents(1, eventObjs);

											eventObjs[0] = eventObj;

											err = OpenCLDriver.clEnqueueReadBuffer(comQ, sumTBuffer, CLBool.True, 0, sum_T.Length * sizeof(int), sumTGC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());

											unsafe
											{
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 0, new SizeT(sizeof(CLMem)), ref bufferFilterIn);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 1, new SizeT(sizeof(CLMem)), ref aux);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 2, sizeof(int), ref width);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 3, sizeof(int), ref height);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 4, sizeof(int), ref padding);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 5, sizeof(float), ref factor);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 6, sizeof(float), ref factor);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 7, sizeof(int), ref sum_I);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 8, sizeof(int), ref sum_T[0]);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 9, new SizeT(sizeof(CLMem)), ref nomBuffer);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 10, new SizeT(sizeof(CLMem)), ref sumT2Buffer);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 11, new SizeT(sizeof(CLMem)), ref sumI2Buffer);
											}

											//Define grid & execute
											err = OpenCLDriver.clFinish(comQ);
											if(err != CLError.Success) throw new Exception(err.ToString());

											err = OpenCLDriver.clEnqueueNDRangeKernel(comQ, kernelMatchTemplate, 2, null, globalws, localws, 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());

											OpenCLDriver.clWaitForEvents(1, eventObjs);

											eventObjs[0] = eventObj;

											err = OpenCLDriver.clEnqueueReadBuffer(comQ, nomBuffer, CLBool.True, 0, nominator.Length * sizeof(int), nomGC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());
											err = OpenCLDriver.clEnqueueReadBuffer(comQ, sumT2Buffer, CLBool.True, 0, sum_T_2.Length * sizeof(int), sumT2GC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());
											err = OpenCLDriver.clEnqueueReadBuffer(comQ, sumI2Buffer, CLBool.True, 0, sum_I_2.Length * sizeof(int), sumI2GC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());

											big_mul = (double)sum_T_2[0] * sum_I_2[0];

											R = nominator[0] / (Math.Sqrt(big_mul));

											if(R > bestR) {
												bestR = R;
												bestIndex = h;
												bestList = 2;
												//err = OpenCLDriver.clEnqueueReadBuffer(comQ, aux, CLBool.True, 0, match.MIplImage.imageSize, match.MIplImage.imageData, 0, null, ref eventObj);
												//if(err != CLError.Success) throw new Exception(err.ToString());
											}
										}

										for(h = 0; h < ListBlueSquareCLMem.Count; h++) {
											CLMem aux = ListBlueSquareCLMem[h];

											nominator[0] = 0;
											sum_T_2[0] = 0;
											sum_I_2[0] = 0;
											sum_T[0] = 0;

											err = OpenCLDriver.clEnqueueWriteBuffer(comQ, sumI2Buffer, CLBool.True, 0, sum_I_2.Length * sizeof(int), sumI2GC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());
											err = OpenCLDriver.clEnqueueWriteBuffer(comQ, sumT2Buffer, CLBool.True, 0, sum_T_2.Length * sizeof(int), sumT2GC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());
											err = OpenCLDriver.clEnqueueWriteBuffer(comQ, sumTBuffer, CLBool.True, 0, sum_T.Length * sizeof(int), sumTGC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());
											err = OpenCLDriver.clEnqueueWriteBuffer(comQ, nomBuffer, CLBool.True, 0, nominator.Length * sizeof(int), nomGC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());

											unsafe
											{
												OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 0, new SizeT(sizeof(CLMem)), ref aux);
												OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 1, sizeof(int), ref width);
												OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 2, sizeof(int), ref height);
												OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 3, sizeof(int), ref padding);
												OpenCLDriver.clSetKernelArg(kernelSumImgPixels, 4, new SizeT(sizeof(CLMem)), ref sumTBuffer);
											}

											//Define grid & execute
											err = OpenCLDriver.clFinish(comQ);
											if(err != CLError.Success) throw new Exception(err.ToString());

											err = OpenCLDriver.clEnqueueNDRangeKernel(comQ, kernelSumImgPixels, 2, null, globalws, localws, 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());

											OpenCLDriver.clWaitForEvents(1, eventObjs);

											eventObjs[0] = eventObj;

											err = OpenCLDriver.clEnqueueReadBuffer(comQ, sumTBuffer, CLBool.True, 0, sum_T.Length * sizeof(int), sumTGC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());

											unsafe
											{
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 0, new SizeT(sizeof(CLMem)), ref bufferFilterIn);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 1, new SizeT(sizeof(CLMem)), ref aux);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 2, sizeof(int), ref width);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 3, sizeof(int), ref height);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 4, sizeof(int), ref padding);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 5, sizeof(float), ref factor);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 6, sizeof(float), ref factor);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 7, sizeof(int), ref sum_I);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 8, sizeof(int), ref sum_T[0]);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 9, new SizeT(sizeof(CLMem)), ref nomBuffer);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 10, new SizeT(sizeof(CLMem)), ref sumT2Buffer);
												OpenCLDriver.clSetKernelArg(kernelMatchTemplate, 11, new SizeT(sizeof(CLMem)), ref sumI2Buffer);
											}

											//Define grid & execute
											err = OpenCLDriver.clFinish(comQ);
											if(err != CLError.Success) throw new Exception(err.ToString());

											err = OpenCLDriver.clEnqueueNDRangeKernel(comQ, kernelMatchTemplate, 2, null, globalws, localws, 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());

											OpenCLDriver.clWaitForEvents(1, eventObjs);

											eventObjs[0] = eventObj;

											err = OpenCLDriver.clEnqueueReadBuffer(comQ, nomBuffer, CLBool.True, 0, nominator.Length * sizeof(int), nomGC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());
											err = OpenCLDriver.clEnqueueReadBuffer(comQ, sumT2Buffer, CLBool.True, 0, sum_T_2.Length * sizeof(int), sumT2GC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());
											err = OpenCLDriver.clEnqueueReadBuffer(comQ, sumI2Buffer, CLBool.True, 0, sum_I_2.Length * sizeof(int), sumI2GC.AddrOfPinnedObject(), 0, null, ref eventObj);
											if(err != CLError.Success) throw new Exception(err.ToString());

											big_mul = (double)sum_T_2[0] * sum_I_2[0];

											R = nominator[0] / (Math.Sqrt(big_mul));

											if(R > bestR) {
												bestR = R;
												bestIndex = h;
												bestList = 3;
												//err = OpenCLDriver.clEnqueueReadBuffer(comQ, aux, CLBool.True, 0, match.MIplImage.imageSize, match.MIplImage.imageData, 0, null, ref eventObj);
												//if(err != CLError.Success) throw new Exception(err.ToString());
											}
										}
										break;
								}
								break;
							default:
								bestList = 4;
								break;
						}

						nomGC.Free();
						sumTGC.Free();
						sumT2GC.Free();
						sumI2GC.Free();

						err = OpenCLDriver.clReleaseMemObject(sumTBuffer);
						if(err != CLError.Success) throw new Exception(err.ToString());
						err = OpenCLDriver.clReleaseMemObject(nomBuffer);
						if(err != CLError.Success) throw new Exception(err.ToString());
						err = OpenCLDriver.clReleaseMemObject(sumT2Buffer);
						if(err != CLError.Success) throw new Exception(err.ToString());
						err = OpenCLDriver.clReleaseMemObject(sumI2Buffer);
						if(err != CLError.Success) throw new Exception(err.ToString());

						CLMem aux1 = new CLMem();

						switch(bestList) {
							case 0:
								aux1 = ListRedCircleCLMem[bestIndex];
								err = OpenCLDriver.clEnqueueReadBuffer(comQ, aux1, CLBool.True, 0, match.MIplImage.imageSize, match.MIplImage.imageData, 0, null, ref eventObj);
								if(err != CLError.Success) throw new Exception(err.ToString());
								break;
							case 1:
								aux1 = ListRedTriangleCLMem[bestIndex];
								err = OpenCLDriver.clEnqueueReadBuffer(comQ, aux1, CLBool.True, 0, match.MIplImage.imageSize, match.MIplImage.imageData, 0, null, ref eventObj);
								if(err != CLError.Success) throw new Exception(err.ToString());
								break;
							case 2:
								aux1 = ListBlueCircleCLMem[bestIndex];
								err = OpenCLDriver.clEnqueueReadBuffer(comQ, aux1, CLBool.True, 0, match.MIplImage.imageSize, match.MIplImage.imageData, 0, null, ref eventObj);
								if(err != CLError.Success) throw new Exception(err.ToString());
								break;
							case 3:
								aux1 = ListBlueSquareCLMem[bestIndex];
								err = OpenCLDriver.clEnqueueReadBuffer(comQ, aux1, CLBool.True, 0, match.MIplImage.imageSize, match.MIplImage.imageData, 0, null, ref eventObj);
								if(err != CLError.Success) throw new Exception(err.ToString());
								break;
							default:
								break;
						}

						if(bestR >= 0.1) {
							unsafe
							{
								dataPtr = (byte*)m.imageData.ToPointer(); // obter apontador do inicio da imagem

								match = match.Resize(finalShape[i + 1] - finalShape[i], finalShape[i + 3] - finalShape[i + 2], INTER.CV_INTER_CUBIC);
								MIplImage n = match.MIplImage;
								byte* imgMatchPtr = (byte*)n.imageData.ToPointer(); // obter apontador do inicio da imagem
								byte* auxMatchPtr;

								width = img.Width;
								height = img.Height;
								nChan = m.nChannels; // numero de canais 3
								padding = m.widthStep - m.nChannels * m.width; // alinhamento (padding)

								for(y = 0; y < height; y++) {
									for(x = 0; x < width; x++) {
										auxPtr = (dataPtr + y * m.widthStep + x * nChan);

										if(x >= finalShape[i] && x < finalShape[i + 1] && y >= finalShape[i + 2] && y < finalShape[i + 3]) {

											auxMatchPtr = (imgMatchPtr + (y - finalShape[i + 2]) * n.widthStep + (x - finalShape[i]) * nChan);

											auxPtr[0] = auxMatchPtr[0];
											auxPtr[1] = auxMatchPtr[1];
											auxPtr[2] = auxMatchPtr[2];
										}
									}
								}
								CvInvoke.cvPutText(img, bestR.ToString("p2"), new Point(finalShape[i + 1], finalShape[i + 2] - 4), ref f, new Bgr(255, 255, 0).MCvScalar);
							}
						}
					}

					///--------- TEMPLATE MATCHING ---------///*********************************************************************************************************************************

					arrGC1.Free();
					arrGC2.Free();
					arrGC3.Free();
					arrGC4.Free();

					err = OpenCLDriver.clReleaseMemObject(bufferArrayVertRed);
					if(err != CLError.Success) throw new Exception(err.ToString());
					err = OpenCLDriver.clReleaseMemObject(bufferArrayHorzRed);
					if(err != CLError.Success) throw new Exception(err.ToString());

					err = OpenCLDriver.clReleaseMemObject(bufferArrayVertBlue);
					if(err != CLError.Success) throw new Exception(err.ToString());
					err = OpenCLDriver.clReleaseMemObject(bufferArrayHorzBlue);
					if(err != CLError.Success) throw new Exception(err.ToString());

					err = OpenCLDriver.clReleaseMemObject(bufferFilterIn);
					if(err != CLError.Success) throw new Exception(err.ToString());
					err = OpenCLDriver.clReleaseMemObject(bufferFilterOut);
					if(err != CLError.Success) throw new Exception(err.ToString());

					err = OpenCLDriver.clReleaseEvent(eventObj);
					if(err != CLError.Success) throw new Exception(err.ToString());
				}
			} catch(Exception exc) {
				MessageBox.Show(exc.ToString());
			}
			//return finalShapeIdentifiedList.ToArray();
		}

	}
}
