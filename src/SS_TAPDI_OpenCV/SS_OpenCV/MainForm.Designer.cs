namespace SS_OpenCV
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.imageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.histogramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.fFTToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.autoZoomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.templateMatchingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.colorToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.convertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.grayScaleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.binarizationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.otsuBinarizationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.negativeToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.emguDirectivesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.directAccessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.imageEnhancementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.transformsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.translationToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.rotationToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.resizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.zoomToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.houghToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.rAWDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.horizontalLinesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.verticalLinesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.circlesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.watershedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.filtersToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.colorToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.redToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.greenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.blueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.averageMethodAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.matrix3x3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.medianToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.differentiationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.robertsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.sobelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.x3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.x5ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.fFTToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.highPassToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.lowPassToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.gaussianLowPassToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.meanGaussianToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.motionBlurToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.wienerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.addNoiseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.compressionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.entropyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.quantificationMatrixToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toJPEGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openCLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.array2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.multiplyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.negativeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.directAccessToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.coalescedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.uncoalescedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.dSamplerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.morphologyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.erodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.dilateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.blockSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.videoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.detectFacesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.detectEyesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.fFTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.signDetectionToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.autoresToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.signDetectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.redSignToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.blueSignToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.panel1 = new System.Windows.Forms.Panel();
			this.ImageViewer = new Emgu.CV.UI.ImageBox();
			this.menuStrip1.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ImageViewer)).BeginInit();
			this.SuspendLayout();
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.Filter = "Images (*.png, *.bmp, *.jpg)|*.png;*.bmp;*.jpg";
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.imageToolStripMenuItem,
            this.colorToolStripMenuItem1,
            this.transformsToolStripMenuItem1,
            this.filtersToolStripMenuItem1,
            this.compressionToolStripMenuItem,
            this.openCLToolStripMenuItem,
            this.videoToolStripMenuItem,
            this.autoresToolStripMenuItem,
            this.signDetectionToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(898, 24);
			this.menuStrip1.TabIndex = 2;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.openToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
			this.openToolStripMenuItem.Text = "Open...";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
			this.saveToolStripMenuItem.Text = "Save As...";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(160, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem});
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
			this.editToolStripMenuItem.Text = "Edit";
			// 
			// undoToolStripMenuItem
			// 
			this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
			this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
			this.undoToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
			this.undoToolStripMenuItem.Text = "Undo";
			this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
			// 
			// imageToolStripMenuItem
			// 
			this.imageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.histogramToolStripMenuItem,
            this.fFTToolStripMenuItem1,
            this.autoZoomToolStripMenuItem,
            this.templateMatchingToolStripMenuItem});
			this.imageToolStripMenuItem.Name = "imageToolStripMenuItem";
			this.imageToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
			this.imageToolStripMenuItem.Text = "Image";
			// 
			// histogramToolStripMenuItem
			// 
			this.histogramToolStripMenuItem.Name = "histogramToolStripMenuItem";
			this.histogramToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
			this.histogramToolStripMenuItem.Text = "Histogram";
			this.histogramToolStripMenuItem.Click += new System.EventHandler(this.histogramToolStripMenuItem_Click);
			// 
			// fFTToolStripMenuItem1
			// 
			this.fFTToolStripMenuItem1.Name = "fFTToolStripMenuItem1";
			this.fFTToolStripMenuItem1.Size = new System.Drawing.Size(178, 22);
			this.fFTToolStripMenuItem1.Text = "FFT";
			this.fFTToolStripMenuItem1.Click += new System.EventHandler(this.fFTToolStripMenuItem1_Click);
			// 
			// autoZoomToolStripMenuItem
			// 
			this.autoZoomToolStripMenuItem.CheckOnClick = true;
			this.autoZoomToolStripMenuItem.Name = "autoZoomToolStripMenuItem";
			this.autoZoomToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
			this.autoZoomToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
			this.autoZoomToolStripMenuItem.Text = "Auto Zoom";
			this.autoZoomToolStripMenuItem.Click += new System.EventHandler(this.autoZoomToolStripMenuItem_Click);
			// 
			// templateMatchingToolStripMenuItem
			// 
			this.templateMatchingToolStripMenuItem.Name = "templateMatchingToolStripMenuItem";
			this.templateMatchingToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
			this.templateMatchingToolStripMenuItem.Text = "Template Matching";
			this.templateMatchingToolStripMenuItem.Click += new System.EventHandler(this.templateMatchingToolStripMenuItem_Click);
			// 
			// colorToolStripMenuItem1
			// 
			this.colorToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.convertToolStripMenuItem,
            this.negativeToolStripMenuItem1,
            this.imageEnhancementToolStripMenuItem});
			this.colorToolStripMenuItem1.Name = "colorToolStripMenuItem1";
			this.colorToolStripMenuItem1.Size = new System.Drawing.Size(48, 20);
			this.colorToolStripMenuItem1.Text = "Color";
			// 
			// convertToolStripMenuItem
			// 
			this.convertToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.grayScaleToolStripMenuItem,
            this.binarizationToolStripMenuItem,
            this.otsuBinarizationToolStripMenuItem});
			this.convertToolStripMenuItem.Name = "convertToolStripMenuItem";
			this.convertToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
			this.convertToolStripMenuItem.Text = "Convert";
			// 
			// grayScaleToolStripMenuItem
			// 
			this.grayScaleToolStripMenuItem.Name = "grayScaleToolStripMenuItem";
			this.grayScaleToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
			this.grayScaleToolStripMenuItem.Text = "Gray Scale";
			this.grayScaleToolStripMenuItem.Click += new System.EventHandler(this.grayScaleToolStripMenuItem_Click);
			// 
			// binarizationToolStripMenuItem
			// 
			this.binarizationToolStripMenuItem.Name = "binarizationToolStripMenuItem";
			this.binarizationToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
			this.binarizationToolStripMenuItem.Text = "Binarization";
			this.binarizationToolStripMenuItem.Click += new System.EventHandler(this.binarizationToolStripMenuItem_Click);
			// 
			// otsuBinarizationToolStripMenuItem
			// 
			this.otsuBinarizationToolStripMenuItem.Name = "otsuBinarizationToolStripMenuItem";
			this.otsuBinarizationToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
			this.otsuBinarizationToolStripMenuItem.Text = "Otsu Binarization";
			this.otsuBinarizationToolStripMenuItem.Click += new System.EventHandler(this.otsuBinarizationToolStripMenuItem_Click);
			// 
			// negativeToolStripMenuItem1
			// 
			this.negativeToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.emguDirectivesToolStripMenuItem,
            this.directAccessToolStripMenuItem});
			this.negativeToolStripMenuItem1.Name = "negativeToolStripMenuItem1";
			this.negativeToolStripMenuItem1.Size = new System.Drawing.Size(183, 22);
			this.negativeToolStripMenuItem1.Text = "Negative";
			// 
			// emguDirectivesToolStripMenuItem
			// 
			this.emguDirectivesToolStripMenuItem.Name = "emguDirectivesToolStripMenuItem";
			this.emguDirectivesToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
			this.emguDirectivesToolStripMenuItem.Text = "Emgu Directives";
			this.emguDirectivesToolStripMenuItem.Click += new System.EventHandler(this.emguDirectivesToolStripMenuItem_Click);
			// 
			// directAccessToolStripMenuItem
			// 
			this.directAccessToolStripMenuItem.Name = "directAccessToolStripMenuItem";
			this.directAccessToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
			this.directAccessToolStripMenuItem.Text = "Direct Access";
			this.directAccessToolStripMenuItem.Click += new System.EventHandler(this.directAccessToolStripMenuItem_Click);
			// 
			// imageEnhancementToolStripMenuItem
			// 
			this.imageEnhancementToolStripMenuItem.Name = "imageEnhancementToolStripMenuItem";
			this.imageEnhancementToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
			this.imageEnhancementToolStripMenuItem.Text = "Image Enhancement";
			this.imageEnhancementToolStripMenuItem.Click += new System.EventHandler(this.imageEnhancementToolStripMenuItem_Click);
			// 
			// transformsToolStripMenuItem1
			// 
			this.transformsToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.translationToolStripMenuItem1,
            this.rotationToolStripMenuItem1,
            this.resizeToolStripMenuItem,
            this.zoomToolStripMenuItem1,
            this.houghToolStripMenuItem,
            this.watershedToolStripMenuItem});
			this.transformsToolStripMenuItem1.Name = "transformsToolStripMenuItem1";
			this.transformsToolStripMenuItem1.Size = new System.Drawing.Size(79, 20);
			this.transformsToolStripMenuItem1.Text = "Transforms";
			// 
			// translationToolStripMenuItem1
			// 
			this.translationToolStripMenuItem1.Name = "translationToolStripMenuItem1";
			this.translationToolStripMenuItem1.Size = new System.Drawing.Size(160, 22);
			this.translationToolStripMenuItem1.Text = "Translation";
			this.translationToolStripMenuItem1.Click += new System.EventHandler(this.translationToolStripMenuItem1_Click);
			// 
			// rotationToolStripMenuItem1
			// 
			this.rotationToolStripMenuItem1.Name = "rotationToolStripMenuItem1";
			this.rotationToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
			this.rotationToolStripMenuItem1.Size = new System.Drawing.Size(160, 22);
			this.rotationToolStripMenuItem1.Text = "Rotation";
			this.rotationToolStripMenuItem1.Click += new System.EventHandler(this.rotationToolStripMenuItem1_Click);
			// 
			// resizeToolStripMenuItem
			// 
			this.resizeToolStripMenuItem.Name = "resizeToolStripMenuItem";
			this.resizeToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
			this.resizeToolStripMenuItem.Text = "Resize";
			this.resizeToolStripMenuItem.Click += new System.EventHandler(this.resizeToolStripMenuItem_Click);
			// 
			// zoomToolStripMenuItem1
			// 
			this.zoomToolStripMenuItem1.Name = "zoomToolStripMenuItem1";
			this.zoomToolStripMenuItem1.Size = new System.Drawing.Size(160, 22);
			this.zoomToolStripMenuItem1.Text = "Zoom";
			this.zoomToolStripMenuItem1.Click += new System.EventHandler(this.zoomToolStripMenuItem1_Click);
			// 
			// houghToolStripMenuItem
			// 
			this.houghToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rAWDataToolStripMenuItem,
            this.horizontalLinesToolStripMenuItem,
            this.verticalLinesToolStripMenuItem,
            this.circlesToolStripMenuItem});
			this.houghToolStripMenuItem.Name = "houghToolStripMenuItem";
			this.houghToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
			this.houghToolStripMenuItem.Text = "Hough";
			this.houghToolStripMenuItem.Click += new System.EventHandler(this.houghToolStripMenuItem_Click);
			// 
			// rAWDataToolStripMenuItem
			// 
			this.rAWDataToolStripMenuItem.Name = "rAWDataToolStripMenuItem";
			this.rAWDataToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
			this.rAWDataToolStripMenuItem.Text = "RAW Data";
			this.rAWDataToolStripMenuItem.Click += new System.EventHandler(this.rAWDataToolStripMenuItem_Click);
			// 
			// horizontalLinesToolStripMenuItem
			// 
			this.horizontalLinesToolStripMenuItem.Name = "horizontalLinesToolStripMenuItem";
			this.horizontalLinesToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
			this.horizontalLinesToolStripMenuItem.Text = "Horizontal Lines";
			this.horizontalLinesToolStripMenuItem.Click += new System.EventHandler(this.horizontalLinesToolStripMenuItem_Click);
			// 
			// verticalLinesToolStripMenuItem
			// 
			this.verticalLinesToolStripMenuItem.Name = "verticalLinesToolStripMenuItem";
			this.verticalLinesToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
			this.verticalLinesToolStripMenuItem.Text = "Vertical Lines";
			this.verticalLinesToolStripMenuItem.Click += new System.EventHandler(this.verticalLinesToolStripMenuItem_Click);
			// 
			// circlesToolStripMenuItem
			// 
			this.circlesToolStripMenuItem.Name = "circlesToolStripMenuItem";
			this.circlesToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
			this.circlesToolStripMenuItem.Text = "Circles";
			this.circlesToolStripMenuItem.Click += new System.EventHandler(this.circlesToolStripMenuItem_Click);
			// 
			// watershedToolStripMenuItem
			// 
			this.watershedToolStripMenuItem.Name = "watershedToolStripMenuItem";
			this.watershedToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
			this.watershedToolStripMenuItem.Text = "Watershed";
			this.watershedToolStripMenuItem.Click += new System.EventHandler(this.watershedToolStripMenuItem_Click);
			// 
			// filtersToolStripMenuItem1
			// 
			this.filtersToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.colorToolStripMenuItem2,
            this.averageMethodAToolStripMenuItem,
            this.matrix3x3ToolStripMenuItem,
            this.medianToolStripMenuItem,
            this.differentiationToolStripMenuItem,
            this.robertsToolStripMenuItem,
            this.sobelToolStripMenuItem,
            this.fFTToolStripMenuItem2,
            this.meanGaussianToolStripMenuItem,
            this.motionBlurToolStripMenuItem,
            this.wienerToolStripMenuItem,
            this.addNoiseToolStripMenuItem});
			this.filtersToolStripMenuItem1.Name = "filtersToolStripMenuItem1";
			this.filtersToolStripMenuItem1.Size = new System.Drawing.Size(50, 20);
			this.filtersToolStripMenuItem1.Text = "Filters";
			// 
			// colorToolStripMenuItem2
			// 
			this.colorToolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.redToolStripMenuItem,
            this.greenToolStripMenuItem,
            this.blueToolStripMenuItem});
			this.colorToolStripMenuItem2.Name = "colorToolStripMenuItem2";
			this.colorToolStripMenuItem2.Size = new System.Drawing.Size(181, 22);
			this.colorToolStripMenuItem2.Text = "Color";
			// 
			// redToolStripMenuItem
			// 
			this.redToolStripMenuItem.Name = "redToolStripMenuItem";
			this.redToolStripMenuItem.Size = new System.Drawing.Size(105, 22);
			this.redToolStripMenuItem.Text = "Red";
			this.redToolStripMenuItem.Click += new System.EventHandler(this.redToolStripMenuItem_Click);
			// 
			// greenToolStripMenuItem
			// 
			this.greenToolStripMenuItem.Name = "greenToolStripMenuItem";
			this.greenToolStripMenuItem.Size = new System.Drawing.Size(105, 22);
			this.greenToolStripMenuItem.Text = "Green";
			this.greenToolStripMenuItem.Click += new System.EventHandler(this.greenToolStripMenuItem_Click);
			// 
			// blueToolStripMenuItem
			// 
			this.blueToolStripMenuItem.Name = "blueToolStripMenuItem";
			this.blueToolStripMenuItem.Size = new System.Drawing.Size(105, 22);
			this.blueToolStripMenuItem.Text = "Blue";
			this.blueToolStripMenuItem.Click += new System.EventHandler(this.blueToolStripMenuItem_Click);
			// 
			// averageMethodAToolStripMenuItem
			// 
			this.averageMethodAToolStripMenuItem.Name = "averageMethodAToolStripMenuItem";
			this.averageMethodAToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
			this.averageMethodAToolStripMenuItem.Text = "Average (Method A)";
			this.averageMethodAToolStripMenuItem.Click += new System.EventHandler(this.averageMethodAToolStripMenuItem_Click);
			// 
			// matrix3x3ToolStripMenuItem
			// 
			this.matrix3x3ToolStripMenuItem.Name = "matrix3x3ToolStripMenuItem";
			this.matrix3x3ToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
			this.matrix3x3ToolStripMenuItem.Text = "Matrix 3x3";
			this.matrix3x3ToolStripMenuItem.Click += new System.EventHandler(this.matrix3x3ToolStripMenuItem_Click);
			// 
			// medianToolStripMenuItem
			// 
			this.medianToolStripMenuItem.Name = "medianToolStripMenuItem";
			this.medianToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
			this.medianToolStripMenuItem.Text = "Median";
			this.medianToolStripMenuItem.Click += new System.EventHandler(this.medianToolStripMenuItem_Click);
			// 
			// differentiationToolStripMenuItem
			// 
			this.differentiationToolStripMenuItem.Name = "differentiationToolStripMenuItem";
			this.differentiationToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
			this.differentiationToolStripMenuItem.Text = "Differentiation";
			this.differentiationToolStripMenuItem.Click += new System.EventHandler(this.differentiationToolStripMenuItem_Click);
			// 
			// robertsToolStripMenuItem
			// 
			this.robertsToolStripMenuItem.Name = "robertsToolStripMenuItem";
			this.robertsToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
			this.robertsToolStripMenuItem.Text = "Roberts";
			this.robertsToolStripMenuItem.Click += new System.EventHandler(this.robertsToolStripMenuItem_Click);
			// 
			// sobelToolStripMenuItem
			// 
			this.sobelToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.x3ToolStripMenuItem,
            this.x5ToolStripMenuItem});
			this.sobelToolStripMenuItem.Name = "sobelToolStripMenuItem";
			this.sobelToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
			this.sobelToolStripMenuItem.Text = "Sobel";
			// 
			// x3ToolStripMenuItem
			// 
			this.x3ToolStripMenuItem.Name = "x3ToolStripMenuItem";
			this.x3ToolStripMenuItem.Size = new System.Drawing.Size(91, 22);
			this.x3ToolStripMenuItem.Text = "3x3";
			this.x3ToolStripMenuItem.Click += new System.EventHandler(this.x3ToolStripMenuItem_Click);
			// 
			// x5ToolStripMenuItem
			// 
			this.x5ToolStripMenuItem.Name = "x5ToolStripMenuItem";
			this.x5ToolStripMenuItem.Size = new System.Drawing.Size(91, 22);
			this.x5ToolStripMenuItem.Text = "5x5";
			this.x5ToolStripMenuItem.Click += new System.EventHandler(this.x5ToolStripMenuItem_Click);
			// 
			// fFTToolStripMenuItem2
			// 
			this.fFTToolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.highPassToolStripMenuItem,
            this.lowPassToolStripMenuItem,
            this.gaussianLowPassToolStripMenuItem});
			this.fFTToolStripMenuItem2.Name = "fFTToolStripMenuItem2";
			this.fFTToolStripMenuItem2.Size = new System.Drawing.Size(181, 22);
			this.fFTToolStripMenuItem2.Text = "FFT";
			// 
			// highPassToolStripMenuItem
			// 
			this.highPassToolStripMenuItem.Name = "highPassToolStripMenuItem";
			this.highPassToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
			this.highPassToolStripMenuItem.Text = "High Pass";
			this.highPassToolStripMenuItem.Click += new System.EventHandler(this.highPassToolStripMenuItem_Click);
			// 
			// lowPassToolStripMenuItem
			// 
			this.lowPassToolStripMenuItem.Name = "lowPassToolStripMenuItem";
			this.lowPassToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
			this.lowPassToolStripMenuItem.Text = "Low Pass";
			this.lowPassToolStripMenuItem.Click += new System.EventHandler(this.lowPassToolStripMenuItem_Click);
			// 
			// gaussianLowPassToolStripMenuItem
			// 
			this.gaussianLowPassToolStripMenuItem.Name = "gaussianLowPassToolStripMenuItem";
			this.gaussianLowPassToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
			this.gaussianLowPassToolStripMenuItem.Text = "Gaussian Low Pass";
			this.gaussianLowPassToolStripMenuItem.Click += new System.EventHandler(this.gaussianLowPassToolStripMenuItem_Click);
			// 
			// meanGaussianToolStripMenuItem
			// 
			this.meanGaussianToolStripMenuItem.Name = "meanGaussianToolStripMenuItem";
			this.meanGaussianToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
			this.meanGaussianToolStripMenuItem.Text = "Mean Gaussian";
			this.meanGaussianToolStripMenuItem.Click += new System.EventHandler(this.meanGaussianToolStripMenuItem_Click);
			// 
			// motionBlurToolStripMenuItem
			// 
			this.motionBlurToolStripMenuItem.Name = "motionBlurToolStripMenuItem";
			this.motionBlurToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
			this.motionBlurToolStripMenuItem.Text = "Motion Blur";
			this.motionBlurToolStripMenuItem.Click += new System.EventHandler(this.motionBlurToolStripMenuItem_Click);
			// 
			// wienerToolStripMenuItem
			// 
			this.wienerToolStripMenuItem.Name = "wienerToolStripMenuItem";
			this.wienerToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
			this.wienerToolStripMenuItem.Text = "Wiener";
			this.wienerToolStripMenuItem.Click += new System.EventHandler(this.wienerToolStripMenuItem_Click);
			// 
			// addNoiseToolStripMenuItem
			// 
			this.addNoiseToolStripMenuItem.Name = "addNoiseToolStripMenuItem";
			this.addNoiseToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
			this.addNoiseToolStripMenuItem.Text = "Add Noise";
			this.addNoiseToolStripMenuItem.Click += new System.EventHandler(this.addNoiseToolStripMenuItem_Click);
			// 
			// compressionToolStripMenuItem
			// 
			this.compressionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tableToolStripMenuItem,
            this.entropyToolStripMenuItem,
            this.quantificationMatrixToolStripMenuItem,
            this.toJPEGToolStripMenuItem});
			this.compressionToolStripMenuItem.Name = "compressionToolStripMenuItem";
			this.compressionToolStripMenuItem.Size = new System.Drawing.Size(89, 20);
			this.compressionToolStripMenuItem.Text = "Compression";
			// 
			// tableToolStripMenuItem
			// 
			this.tableToolStripMenuItem.Name = "tableToolStripMenuItem";
			this.tableToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
			this.tableToolStripMenuItem.Text = "Table";
			this.tableToolStripMenuItem.Click += new System.EventHandler(this.tableToolStripMenuItem_Click);
			// 
			// entropyToolStripMenuItem
			// 
			this.entropyToolStripMenuItem.Name = "entropyToolStripMenuItem";
			this.entropyToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
			this.entropyToolStripMenuItem.Text = "Entropy";
			this.entropyToolStripMenuItem.Click += new System.EventHandler(this.entropyToolStripMenuItem_Click);
			// 
			// quantificationMatrixToolStripMenuItem
			// 
			this.quantificationMatrixToolStripMenuItem.Name = "quantificationMatrixToolStripMenuItem";
			this.quantificationMatrixToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
			this.quantificationMatrixToolStripMenuItem.Text = "Quantification Matrix";
			this.quantificationMatrixToolStripMenuItem.Click += new System.EventHandler(this.quantificationMatrixToolStripMenuItem_Click);
			// 
			// toJPEGToolStripMenuItem
			// 
			this.toJPEGToolStripMenuItem.Name = "toJPEGToolStripMenuItem";
			this.toJPEGToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
			this.toJPEGToolStripMenuItem.Text = "To JPEG";
			this.toJPEGToolStripMenuItem.Click += new System.EventHandler(this.toJPEGToolStripMenuItem_Click);
			// 
			// openCLToolStripMenuItem
			// 
			this.openCLToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.array2ToolStripMenuItem,
            this.multiplyToolStripMenuItem,
            this.negativeToolStripMenuItem,
            this.morphologyToolStripMenuItem,
            this.blockSizeToolStripMenuItem,
            this.testToolStripMenuItem});
			this.openCLToolStripMenuItem.Name = "openCLToolStripMenuItem";
			this.openCLToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
			this.openCLToolStripMenuItem.Text = "OpenCL";
			// 
			// array2ToolStripMenuItem
			// 
			this.array2ToolStripMenuItem.Name = "array2ToolStripMenuItem";
			this.array2ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.array2ToolStripMenuItem.Text = "array^2";
			this.array2ToolStripMenuItem.Click += new System.EventHandler(this.array2ToolStripMenuItem_Click);
			// 
			// multiplyToolStripMenuItem
			// 
			this.multiplyToolStripMenuItem.Name = "multiplyToolStripMenuItem";
			this.multiplyToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.multiplyToolStripMenuItem.Text = "Multiply";
			this.multiplyToolStripMenuItem.Click += new System.EventHandler(this.multiplyToolStripMenuItem_Click);
			// 
			// negativeToolStripMenuItem
			// 
			this.negativeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.directAccessToolStripMenuItem1,
            this.dSamplerToolStripMenuItem});
			this.negativeToolStripMenuItem.Name = "negativeToolStripMenuItem";
			this.negativeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.negativeToolStripMenuItem.Text = "Negative";
			// 
			// directAccessToolStripMenuItem1
			// 
			this.directAccessToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.coalescedToolStripMenuItem,
            this.uncoalescedToolStripMenuItem});
			this.directAccessToolStripMenuItem1.Name = "directAccessToolStripMenuItem1";
			this.directAccessToolStripMenuItem1.Size = new System.Drawing.Size(144, 22);
			this.directAccessToolStripMenuItem1.Text = "Direct Access";
			// 
			// coalescedToolStripMenuItem
			// 
			this.coalescedToolStripMenuItem.Name = "coalescedToolStripMenuItem";
			this.coalescedToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
			this.coalescedToolStripMenuItem.Text = "Coalesced";
			this.coalescedToolStripMenuItem.Click += new System.EventHandler(this.coalescedToolStripMenuItem_Click_1);
			// 
			// uncoalescedToolStripMenuItem
			// 
			this.uncoalescedToolStripMenuItem.Name = "uncoalescedToolStripMenuItem";
			this.uncoalescedToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
			this.uncoalescedToolStripMenuItem.Text = "Uncoalesced";
			this.uncoalescedToolStripMenuItem.Click += new System.EventHandler(this.uncoalescedToolStripMenuItem_Click_1);
			// 
			// dSamplerToolStripMenuItem
			// 
			this.dSamplerToolStripMenuItem.Name = "dSamplerToolStripMenuItem";
			this.dSamplerToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
			this.dSamplerToolStripMenuItem.Text = "2D Sampler";
			this.dSamplerToolStripMenuItem.Click += new System.EventHandler(this.dSamplerToolStripMenuItem_Click);
			// 
			// morphologyToolStripMenuItem
			// 
			this.morphologyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.erodeToolStripMenuItem,
            this.dilateToolStripMenuItem});
			this.morphologyToolStripMenuItem.Name = "morphologyToolStripMenuItem";
			this.morphologyToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.morphologyToolStripMenuItem.Text = "Morphology";
			// 
			// erodeToolStripMenuItem
			// 
			this.erodeToolStripMenuItem.Name = "erodeToolStripMenuItem";
			this.erodeToolStripMenuItem.Size = new System.Drawing.Size(104, 22);
			this.erodeToolStripMenuItem.Text = "Erode";
			this.erodeToolStripMenuItem.Click += new System.EventHandler(this.erodeToolStripMenuItem_Click);
			// 
			// dilateToolStripMenuItem
			// 
			this.dilateToolStripMenuItem.Name = "dilateToolStripMenuItem";
			this.dilateToolStripMenuItem.Size = new System.Drawing.Size(104, 22);
			this.dilateToolStripMenuItem.Text = "Dilate";
			this.dilateToolStripMenuItem.Click += new System.EventHandler(this.dilateToolStripMenuItem_Click);
			// 
			// blockSizeToolStripMenuItem
			// 
			this.blockSizeToolStripMenuItem.Name = "blockSizeToolStripMenuItem";
			this.blockSizeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.blockSizeToolStripMenuItem.Text = "Block Size";
			this.blockSizeToolStripMenuItem.Click += new System.EventHandler(this.blockSizeToolStripMenuItem_Click);
			// 
			// testToolStripMenuItem
			// 
			this.testToolStripMenuItem.Name = "testToolStripMenuItem";
			this.testToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.testToolStripMenuItem.Text = "Test";
			this.testToolStripMenuItem.Click += new System.EventHandler(this.testToolStripMenuItem_Click);
			// 
			// videoToolStripMenuItem
			// 
			this.videoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.detectFacesToolStripMenuItem,
            this.detectEyesToolStripMenuItem,
            this.fFTToolStripMenuItem,
            this.signDetectionToolStripMenuItem1});
			this.videoToolStripMenuItem.Name = "videoToolStripMenuItem";
			this.videoToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
			this.videoToolStripMenuItem.Text = "Video";
			// 
			// detectFacesToolStripMenuItem
			// 
			this.detectFacesToolStripMenuItem.Name = "detectFacesToolStripMenuItem";
			this.detectFacesToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
			this.detectFacesToolStripMenuItem.Text = "Detect Faces";
			this.detectFacesToolStripMenuItem.Click += new System.EventHandler(this.detectFacesToolStripMenuItem_Click);
			// 
			// detectEyesToolStripMenuItem
			// 
			this.detectEyesToolStripMenuItem.Name = "detectEyesToolStripMenuItem";
			this.detectEyesToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
			this.detectEyesToolStripMenuItem.Text = "Detect Eyes";
			this.detectEyesToolStripMenuItem.Click += new System.EventHandler(this.detectEyesToolStripMenuItem_Click);
			// 
			// fFTToolStripMenuItem
			// 
			this.fFTToolStripMenuItem.Name = "fFTToolStripMenuItem";
			this.fFTToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
			this.fFTToolStripMenuItem.Text = "FFT";
			this.fFTToolStripMenuItem.Click += new System.EventHandler(this.fFTToolStripMenuItem_Click);
			// 
			// signDetectionToolStripMenuItem1
			// 
			this.signDetectionToolStripMenuItem1.Name = "signDetectionToolStripMenuItem1";
			this.signDetectionToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
			this.signDetectionToolStripMenuItem1.Size = new System.Drawing.Size(192, 22);
			this.signDetectionToolStripMenuItem1.Text = "Sign Detection";
			this.signDetectionToolStripMenuItem1.Click += new System.EventHandler(this.signDetectionToolStripMenuItem1_Click);
			// 
			// autoresToolStripMenuItem
			// 
			this.autoresToolStripMenuItem.Name = "autoresToolStripMenuItem";
			this.autoresToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
			this.autoresToolStripMenuItem.Text = "Authors";
			this.autoresToolStripMenuItem.Click += new System.EventHandler(this.autoresToolStripMenuItem_Click);
			// 
			// signDetectionToolStripMenuItem
			// 
			this.signDetectionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.redSignToolStripMenuItem,
            this.blueSignToolStripMenuItem});
			this.signDetectionToolStripMenuItem.Name = "signDetectionToolStripMenuItem";
			this.signDetectionToolStripMenuItem.Size = new System.Drawing.Size(96, 20);
			this.signDetectionToolStripMenuItem.Text = "Sign Detection";
			this.signDetectionToolStripMenuItem.Visible = false;
			// 
			// redSignToolStripMenuItem
			// 
			this.redSignToolStripMenuItem.Name = "redSignToolStripMenuItem";
			this.redSignToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
			this.redSignToolStripMenuItem.Text = "Red Sign";
			this.redSignToolStripMenuItem.Click += new System.EventHandler(this.redSignToolStripMenuItem_Click);
			// 
			// blueSignToolStripMenuItem
			// 
			this.blueSignToolStripMenuItem.Name = "blueSignToolStripMenuItem";
			this.blueSignToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
			this.blueSignToolStripMenuItem.Text = "Blue Sign";
			this.blueSignToolStripMenuItem.Click += new System.EventHandler(this.blueSignToolStripMenuItem_Click);
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.Controls.Add(this.ImageViewer);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 24);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(898, 357);
			this.panel1.TabIndex = 6;
			// 
			// ImageViewer
			// 
			this.ImageViewer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ImageViewer.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
			this.ImageViewer.Location = new System.Drawing.Point(0, 0);
			this.ImageViewer.Name = "ImageViewer";
			this.ImageViewer.Size = new System.Drawing.Size(898, 357);
			this.ImageViewer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.ImageViewer.TabIndex = 6;
			this.ImageViewer.TabStop = false;
			this.ImageViewer.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ImageViewer_MouseClick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(898, 381);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "MainForm";
			this.Text = "Sistemas Sensoriais 2015/2016 - Image processing";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.ImageViewer)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoresToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoZoomToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ToolStripMenuItem colorToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem convertToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem grayScaleToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem binarizationToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem otsuBinarizationToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem negativeToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem emguDirectivesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem directAccessToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem transformsToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem translationToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem rotationToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem zoomToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem filtersToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem colorToolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem redToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem greenToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem blueToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem averageMethodAToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem matrix3x3ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem sobelToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem x3ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem x5ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem differentiationToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem robertsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem medianToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem histogramToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem signDetectionToolStripMenuItem;
		//public System.Windows.Forms.PictureBox ImageViewer;
		public Emgu.CV.UI.ImageBox ImageViewer;
		private System.Windows.Forms.ToolStripMenuItem redSignToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem blueSignToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem compressionToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem tableToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem entropyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem quantificationMatrixToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toJPEGToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem houghToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem horizontalLinesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem verticalLinesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem circlesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openCLToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem array2ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem multiplyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem negativeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem directAccessToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem coalescedToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem uncoalescedToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem dSamplerToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem videoToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem detectFacesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem detectEyesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem fFTToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem fFTToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem fFTToolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem highPassToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem lowPassToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem gaussianLowPassToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem rAWDataToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem meanGaussianToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem motionBlurToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem wienerToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem addNoiseToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem resizeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem signDetectionToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem watershedToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem morphologyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem erodeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem dilateToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem templateMatchingToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem blockSizeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem imageEnhancementToolStripMenuItem;
	}
}

