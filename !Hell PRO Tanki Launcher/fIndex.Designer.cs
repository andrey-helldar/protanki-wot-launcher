namespace _Hell_PRO_Tanki_Launcher
{
    partial class fIndex
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
            this.bPlay = new System.Windows.Forms.Button();
            this.bLauncher = new System.Windows.Forms.Button();
            this.bUpdate = new System.Windows.Forms.Button();
            this.bVideo = new System.Windows.Forms.Button();
            this.bExit = new System.Windows.Forms.Button();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.llVersion = new System.Windows.Forms.LinkLabel();
            this.bwUpdater = new System.ComponentModel.BackgroundWorker();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsShow = new System.Windows.Forms.ToolStripMenuItem();
            this.tsVideo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsCheckUpdates = new System.Windows.Forms.ToolStripMenuItem();
            this.tsSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsExit = new System.Windows.Forms.ToolStripMenuItem();
            this.bOptimizePC = new System.Windows.Forms.Button();
            this.llTitle = new System.Windows.Forms.LinkLabel();
            this.bSettings = new System.Windows.Forms.Button();
            this.bwVideo = new System.ComponentModel.BackgroundWorker();
            this.llActually = new System.Windows.Forms.LinkLabel();
            this.pbDownload = new System.Windows.Forms.ProgressBar();
            this.bShowVideo = new System.Windows.Forms.Button();
            this.bShowNews = new System.Windows.Forms.Button();
            this.llBlockCaption = new System.Windows.Forms.LinkLabel();
            this.bwNews = new System.ComponentModel.BackgroundWorker();
            this.pVideo = new System.Windows.Forms.Panel();
            this.llLoadingVideoData = new System.Windows.Forms.LinkLabel();
            this.pNews = new System.Windows.Forms.Panel();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.pbWarning = new System.Windows.Forms.PictureBox();
            this.llVideoAll = new System.Windows.Forms.LinkLabel();
            this.contextMenu.SuspendLayout();
            this.pVideo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbWarning)).BeginInit();
            this.SuspendLayout();
            // 
            // bPlay
            // 
            this.bPlay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.bPlay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bPlay.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.bPlay.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.bPlay.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.bPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bPlay.Font = new System.Drawing.Font("Sochi2014", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bPlay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.bPlay.Location = new System.Drawing.Point(644, 39);
            this.bPlay.Margin = new System.Windows.Forms.Padding(0);
            this.bPlay.Name = "bPlay";
            this.bPlay.Size = new System.Drawing.Size(200, 50);
            this.bPlay.TabIndex = 1;
            this.bPlay.Text = "Играть";
            this.bPlay.UseVisualStyleBackColor = false;
            this.bPlay.Click += new System.EventHandler(this.bPlay_Click);
            // 
            // bLauncher
            // 
            this.bLauncher.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.bLauncher.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bLauncher.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.bLauncher.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.bLauncher.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.bLauncher.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bLauncher.Font = new System.Drawing.Font("Sochi2014", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bLauncher.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.bLauncher.Location = new System.Drawing.Point(644, 107);
            this.bLauncher.Margin = new System.Windows.Forms.Padding(0);
            this.bLauncher.Name = "bLauncher";
            this.bLauncher.Size = new System.Drawing.Size(200, 50);
            this.bLauncher.TabIndex = 2;
            this.bLauncher.Text = "Лаунчер";
            this.bLauncher.UseVisualStyleBackColor = false;
            this.bLauncher.Click += new System.EventHandler(this.bLauncher_Click);
            // 
            // bUpdate
            // 
            this.bUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.bUpdate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bUpdate.Enabled = false;
            this.bUpdate.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.bUpdate.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.bUpdate.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.bUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bUpdate.Font = new System.Drawing.Font("Sochi2014", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bUpdate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.bUpdate.Location = new System.Drawing.Point(644, 175);
            this.bUpdate.Margin = new System.Windows.Forms.Padding(0);
            this.bUpdate.Name = "bUpdate";
            this.bUpdate.Size = new System.Drawing.Size(200, 50);
            this.bUpdate.TabIndex = 3;
            this.bUpdate.Text = "Обновить";
            this.bUpdate.UseVisualStyleBackColor = false;
            this.bUpdate.Click += new System.EventHandler(this.bUpdate_Click);
            // 
            // bVideo
            // 
            this.bVideo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.bVideo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bVideo.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.bVideo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.bVideo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.bVideo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bVideo.Font = new System.Drawing.Font("Sochi2014", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bVideo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.bVideo.Location = new System.Drawing.Point(644, 243);
            this.bVideo.Margin = new System.Windows.Forms.Padding(0);
            this.bVideo.Name = "bVideo";
            this.bVideo.Size = new System.Drawing.Size(200, 50);
            this.bVideo.TabIndex = 4;
            this.bVideo.Text = "Видео";
            this.bVideo.UseVisualStyleBackColor = false;
            this.bVideo.Click += new System.EventHandler(this.bVideo_Click);
            // 
            // bExit
            // 
            this.bExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.bExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bExit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.bExit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.bExit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.bExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bExit.Font = new System.Drawing.Font("Sochi2014", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.bExit.Location = new System.Drawing.Point(644, 379);
            this.bExit.Margin = new System.Windows.Forms.Padding(0);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(200, 50);
            this.bExit.TabIndex = 6;
            this.bExit.Text = "Выход";
            this.bExit.UseVisualStyleBackColor = false;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.ActiveLinkColor = System.Drawing.Color.Red;
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.linkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabel1.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.linkLabel1.Location = new System.Drawing.Point(660, 436);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(164, 15);
            this.linkLabel1.TabIndex = 0;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Developed by Andrey Helldar";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // llVersion
            // 
            this.llVersion.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.llVersion.AutoSize = true;
            this.llVersion.BackColor = System.Drawing.Color.Transparent;
            this.llVersion.Font = new System.Drawing.Font("Sochi2014", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.llVersion.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.llVersion.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.llVersion.Location = new System.Drawing.Point(9, 4);
            this.llVersion.Name = "llVersion";
            this.llVersion.Size = new System.Drawing.Size(54, 19);
            this.llVersion.TabIndex = 0;
            this.llVersion.TabStop = true;
            this.llVersion.Text = "0.0.0.0";
            this.llVersion.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llVersion_LinkClicked);
            // 
            // bwUpdater
            // 
            this.bwUpdater.WorkerReportsProgress = true;
            this.bwUpdater.WorkerSupportsCancellation = true;
            this.bwUpdater.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwUpdater_DoWork);
            this.bwUpdater.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwUpdater_RunWorkerCompleted);
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsShow,
            this.tsVideo,
            this.toolStripMenuItem2,
            this.tsCheckUpdates,
            this.tsSettings,
            this.toolStripMenuItem3,
            this.tsExit});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(205, 126);
            // 
            // tsShow
            // 
            this.tsShow.Name = "tsShow";
            this.tsShow.Size = new System.Drawing.Size(204, 22);
            this.tsShow.Text = "Главное окно";
            this.tsShow.Click += new System.EventHandler(this.tsShow_Click);
            // 
            // tsVideo
            // 
            this.tsVideo.Name = "tsVideo";
            this.tsVideo.Size = new System.Drawing.Size(204, 22);
            this.tsVideo.Text = "Видео";
            this.tsVideo.Click += new System.EventHandler(this.tsVideo_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(201, 6);
            // 
            // tsCheckUpdates
            // 
            this.tsCheckUpdates.Name = "tsCheckUpdates";
            this.tsCheckUpdates.Size = new System.Drawing.Size(204, 22);
            this.tsCheckUpdates.Text = "Проверить обновления";
            this.tsCheckUpdates.Click += new System.EventHandler(this.tsCheckUpdates_Click);
            // 
            // tsSettings
            // 
            this.tsSettings.Name = "tsSettings";
            this.tsSettings.Size = new System.Drawing.Size(204, 22);
            this.tsSettings.Text = "Настройки";
            this.tsSettings.Click += new System.EventHandler(this.tsSettings_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(201, 6);
            // 
            // tsExit
            // 
            this.tsExit.Name = "tsExit";
            this.tsExit.Size = new System.Drawing.Size(204, 22);
            this.tsExit.Text = "Выход";
            this.tsExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // bOptimizePC
            // 
            this.bOptimizePC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.bOptimizePC.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bOptimizePC.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.bOptimizePC.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.bOptimizePC.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.bOptimizePC.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bOptimizePC.Font = new System.Drawing.Font("Sochi2014", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bOptimizePC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.bOptimizePC.Location = new System.Drawing.Point(644, 311);
            this.bOptimizePC.Margin = new System.Windows.Forms.Padding(0);
            this.bOptimizePC.Name = "bOptimizePC";
            this.bOptimizePC.Size = new System.Drawing.Size(200, 50);
            this.bOptimizePC.TabIndex = 5;
            this.bOptimizePC.Text = "Оптимизировать";
            this.bOptimizePC.UseVisualStyleBackColor = false;
            this.bOptimizePC.Click += new System.EventHandler(this.bOptimizePC_Click);
            // 
            // llTitle
            // 
            this.llTitle.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.llTitle.AutoSize = true;
            this.llTitle.BackColor = System.Drawing.Color.Transparent;
            this.llTitle.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.llTitle.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.llTitle.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.llTitle.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.llTitle.Location = new System.Drawing.Point(189, 6);
            this.llTitle.Name = "llTitle";
            this.llTitle.Size = new System.Drawing.Size(79, 17);
            this.llTitle.TabIndex = 0;
            this.llTitle.TabStop = true;
            this.llTitle.Text = "Загрузка...";
            this.llTitle.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.llTitle.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llTitle_LinkClicked);
            // 
            // bSettings
            // 
            this.bSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.bSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bSettings.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.bSettings.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.bSettings.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.bSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bSettings.Font = new System.Drawing.Font("Sochi2014", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bSettings.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.bSettings.Location = new System.Drawing.Point(12, 421);
            this.bSettings.Name = "bSettings";
            this.bSettings.Size = new System.Drawing.Size(131, 30);
            this.bSettings.TabIndex = 7;
            this.bSettings.Text = "Настройки";
            this.bSettings.UseVisualStyleBackColor = true;
            this.bSettings.Click += new System.EventHandler(this.bSettings_Click);
            // 
            // bwVideo
            // 
            this.bwVideo.WorkerReportsProgress = true;
            this.bwVideo.WorkerSupportsCancellation = true;
            this.bwVideo.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwVideo_DoWork);
            this.bwVideo.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bwVideo_ProgressChanged);
            this.bwVideo.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwVideo_RunWorkerCompleted);
            // 
            // llActually
            // 
            this.llActually.ActiveLinkColor = System.Drawing.Color.Lime;
            this.llActually.BackColor = System.Drawing.Color.Transparent;
            this.llActually.Font = new System.Drawing.Font("Sochi2014", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.llActually.ForeColor = System.Drawing.Color.Lime;
            this.llActually.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.llActually.LinkColor = System.Drawing.Color.Lime;
            this.llActually.Location = new System.Drawing.Point(225, 33);
            this.llActually.Name = "llActually";
            this.llActually.Size = new System.Drawing.Size(408, 30);
            this.llActually.TabIndex = 11;
            this.llActually.TabStop = true;
            this.llActually.Text = "Loading...";
            this.llActually.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.llActually.VisitedLinkColor = System.Drawing.Color.Lime;
            this.llActually.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llActually_LinkClicked);
            // 
            // pbDownload
            // 
            this.pbDownload.Location = new System.Drawing.Point(12, 405);
            this.pbDownload.Name = "pbDownload";
            this.pbDownload.Size = new System.Drawing.Size(629, 10);
            this.pbDownload.TabIndex = 26;
            this.pbDownload.Visible = false;
            // 
            // bShowVideo
            // 
            this.bShowVideo.BackColor = System.Drawing.Color.Black;
            this.bShowVideo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bShowVideo.Enabled = false;
            this.bShowVideo.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(55)))), ((int)(((byte)(0)))));
            this.bShowVideo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.bShowVideo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.bShowVideo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bShowVideo.Font = new System.Drawing.Font("Sochi2014", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bShowVideo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.bShowVideo.Location = new System.Drawing.Point(13, 33);
            this.bShowVideo.Name = "bShowVideo";
            this.bShowVideo.Size = new System.Drawing.Size(100, 30);
            this.bShowVideo.TabIndex = 9;
            this.bShowVideo.Text = "Видео";
            this.bShowVideo.UseVisualStyleBackColor = false;
            this.bShowVideo.Click += new System.EventHandler(this.bShowVideo_Click);
            // 
            // bShowNews
            // 
            this.bShowNews.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.bShowNews.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bShowNews.Enabled = false;
            this.bShowNews.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.bShowNews.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.bShowNews.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.bShowNews.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bShowNews.Font = new System.Drawing.Font("Sochi2014", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bShowNews.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.bShowNews.Location = new System.Drawing.Point(119, 33);
            this.bShowNews.Name = "bShowNews";
            this.bShowNews.Size = new System.Drawing.Size(100, 30);
            this.bShowNews.TabIndex = 10;
            this.bShowNews.Text = "Новости";
            this.bShowNews.UseVisualStyleBackColor = true;
            this.bShowNews.Click += new System.EventHandler(this.bShowNews_Click);
            // 
            // llBlockCaption
            // 
            this.llBlockCaption.ActiveLinkColor = System.Drawing.Color.WhiteSmoke;
            this.llBlockCaption.AutoSize = true;
            this.llBlockCaption.BackColor = System.Drawing.Color.Transparent;
            this.llBlockCaption.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.llBlockCaption.DisabledLinkColor = System.Drawing.Color.DarkGray;
            this.llBlockCaption.Font = new System.Drawing.Font("Sochi2014", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.llBlockCaption.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.llBlockCaption.LinkColor = System.Drawing.Color.WhiteSmoke;
            this.llBlockCaption.Location = new System.Drawing.Point(28, 74);
            this.llBlockCaption.Name = "llBlockCaption";
            this.llBlockCaption.Size = new System.Drawing.Size(63, 24);
            this.llBlockCaption.TabIndex = 29;
            this.llBlockCaption.TabStop = true;
            this.llBlockCaption.Text = "Видео:";
            this.llBlockCaption.VisitedLinkColor = System.Drawing.Color.WhiteSmoke;
            // 
            // bwNews
            // 
            this.bwNews.WorkerReportsProgress = true;
            this.bwNews.WorkerSupportsCancellation = true;
            this.bwNews.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwNews_DoWork);
            this.bwNews.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bwNews_ProgressChanged);
            this.bwNews.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwNews_RunWorkerCompleted);
            // 
            // pVideo
            // 
            this.pVideo.BackColor = System.Drawing.Color.Transparent;
            this.pVideo.Controls.Add(this.llLoadingVideoData);
            this.pVideo.Location = new System.Drawing.Point(13, 109);
            this.pVideo.Name = "pVideo";
            this.pVideo.Size = new System.Drawing.Size(620, 290);
            this.pVideo.TabIndex = 12;
            // 
            // llLoadingVideoData
            // 
            this.llLoadingVideoData.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.llLoadingVideoData.AutoSize = true;
            this.llLoadingVideoData.BackColor = System.Drawing.Color.Transparent;
            this.llLoadingVideoData.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.llLoadingVideoData.Font = new System.Drawing.Font("Sochi2014", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.llLoadingVideoData.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.llLoadingVideoData.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.llLoadingVideoData.Location = new System.Drawing.Point(222, 140);
            this.llLoadingVideoData.Name = "llLoadingVideoData";
            this.llLoadingVideoData.Size = new System.Drawing.Size(182, 19);
            this.llLoadingVideoData.TabIndex = 0;
            this.llLoadingVideoData.TabStop = true;
            this.llLoadingVideoData.Text = "Подождите, идет загрузка...";
            this.llLoadingVideoData.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            // 
            // pNews
            // 
            this.pNews.BackColor = System.Drawing.Color.Transparent;
            this.pNews.Location = new System.Drawing.Point(407, 88);
            this.pNews.Name = "pNews";
            this.pNews.Size = new System.Drawing.Size(89, 46);
            this.pNews.TabIndex = 13;
            this.pNews.Visible = false;
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.contextMenu;
            this.notifyIcon.Text = "notifyIcon";
            this.notifyIcon.Visible = true;
            this.notifyIcon.BalloonTipClicked += new System.EventHandler(this.notifyIcon_BalloonTipClicked);
            this.notifyIcon.Click += new System.EventHandler(this.notifyIcon_Click);
            // 
            // pbWarning
            // 
            this.pbWarning.BackColor = System.Drawing.Color.Transparent;
            this.pbWarning.BackgroundImage = global::_Hell_PRO_Tanki_Launcher.Properties.Resources.Warning;
            this.pbWarning.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbWarning.Location = new System.Drawing.Point(149, 419);
            this.pbWarning.Name = "pbWarning";
            this.pbWarning.Size = new System.Drawing.Size(32, 32);
            this.pbWarning.TabIndex = 32;
            this.pbWarning.TabStop = false;
            this.pbWarning.Click += new System.EventHandler(this.pbWarning_Click);
            // 
            // llVideoAll
            // 
            this.llVideoAll.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.llVideoAll.AutoSize = true;
            this.llVideoAll.BackColor = System.Drawing.Color.Transparent;
            this.llVideoAll.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.llVideoAll.Font = new System.Drawing.Font("Sochi2014", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.llVideoAll.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.llVideoAll.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.llVideoAll.Location = new System.Drawing.Point(187, 426);
            this.llVideoAll.Name = "llVideoAll";
            this.llVideoAll.Size = new System.Drawing.Size(67, 20);
            this.llVideoAll.TabIndex = 8;
            this.llVideoAll.TabStop = true;
            this.llVideoAll.Text = "Loading...";
            this.llVideoAll.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.llVideoAll.Click += new System.EventHandler(this.label_Click);
            // 
            // fIndex
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = global::_Hell_PRO_Tanki_Launcher.Properties.Resources.back_6;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(860, 460);
            this.Controls.Add(this.llVideoAll);
            this.Controls.Add(this.pbWarning);
            this.Controls.Add(this.pNews);
            this.Controls.Add(this.pVideo);
            this.Controls.Add(this.llBlockCaption);
            this.Controls.Add(this.bShowNews);
            this.Controls.Add(this.bShowVideo);
            this.Controls.Add(this.pbDownload);
            this.Controls.Add(this.llActually);
            this.Controls.Add(this.bSettings);
            this.Controls.Add(this.llTitle);
            this.Controls.Add(this.bOptimizePC);
            this.Controls.Add(this.llVersion);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.bExit);
            this.Controls.Add(this.bVideo);
            this.Controls.Add(this.bUpdate);
            this.Controls.Add(this.bLauncher);
            this.Controls.Add(this.bPlay);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fIndex";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "fIndex";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.fIndex_FormClosing);
            this.Load += new System.EventHandler(this.fIndex_Load);
            this.contextMenu.ResumeLayout(false);
            this.pVideo.ResumeLayout(false);
            this.pVideo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbWarning)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bPlay;
        private System.Windows.Forms.Button bLauncher;
        private System.Windows.Forms.Button bUpdate;
        private System.Windows.Forms.Button bVideo;
        private System.Windows.Forms.Button bExit;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.LinkLabel llVersion;
        private System.ComponentModel.BackgroundWorker bwUpdater;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem tsExit;
        private System.Windows.Forms.ToolStripMenuItem tsCheckUpdates;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem tsVideo;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem tsShow;
        private System.Windows.Forms.Button bOptimizePC;
        private System.Windows.Forms.ToolStripMenuItem tsSettings;
        private System.Windows.Forms.LinkLabel llTitle;
        private System.Windows.Forms.Button bSettings;
        private System.ComponentModel.BackgroundWorker bwVideo;
        private System.Windows.Forms.LinkLabel llActually;
        private System.Windows.Forms.Button bShowVideo;
        private System.Windows.Forms.Button bShowNews;
        private System.Windows.Forms.LinkLabel llBlockCaption;
        private System.ComponentModel.BackgroundWorker bwNews;
        private System.Windows.Forms.Panel pVideo;
        private System.Windows.Forms.Panel pNews;
        private System.Windows.Forms.LinkLabel llLoadingVideoData;
        public System.Windows.Forms.ProgressBar pbDownload;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.PictureBox pbWarning;
        private System.Windows.Forms.LinkLabel llVideoAll;
    }
}

