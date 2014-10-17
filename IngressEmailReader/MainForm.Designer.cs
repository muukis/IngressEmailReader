namespace IngressEmailReader
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.ilMain = new System.Windows.Forms.ImageList(this.components);
            this.pbSettings = new System.Windows.Forms.PictureBox();
            this.lvIngressEmails = new System.Windows.Forms.ListView();
            this.chType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chReceived = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chResponseTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.pMain = new System.Windows.Forms.Panel();
            this.btnShowAll = new System.Windows.Forms.Button();
            this.lvSummary = new System.Windows.Forms.ListView();
            this.chSummaryType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chSummaryValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnTypeHierarchy = new System.Windows.Forms.Button();
            this.pbMain = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.pbSettings)).BeginInit();
            this.pMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // ilMain
            // 
            this.ilMain.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilMain.ImageStream")));
            this.ilMain.TransparentColor = System.Drawing.Color.Transparent;
            this.ilMain.Images.SetKeyName(0, "1413591318_accept.png");
            this.ilMain.Images.SetKeyName(1, "1413591335_16189.ico");
            this.ilMain.Images.SetKeyName(2, "1413591429_Time.png");
            // 
            // pbSettings
            // 
            this.pbSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbSettings.Image = global::IngressEmailReader.Properties.Resources.settings;
            this.pbSettings.Location = new System.Drawing.Point(551, 0);
            this.pbSettings.Name = "pbSettings";
            this.pbSettings.Size = new System.Drawing.Size(24, 24);
            this.pbSettings.TabIndex = 0;
            this.pbSettings.TabStop = false;
            this.pbSettings.Click += new System.EventHandler(this.pbSettings_Click);
            // 
            // lvIngressEmails
            // 
            this.lvIngressEmails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvIngressEmails.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chType,
            this.chName,
            this.chReceived,
            this.chResponseTime});
            this.lvIngressEmails.FullRowSelect = true;
            this.lvIngressEmails.Location = new System.Drawing.Point(0, 30);
            this.lvIngressEmails.MultiSelect = false;
            this.lvIngressEmails.Name = "lvIngressEmails";
            this.lvIngressEmails.Size = new System.Drawing.Size(575, 305);
            this.lvIngressEmails.SmallImageList = this.ilMain;
            this.lvIngressEmails.TabIndex = 1;
            this.lvIngressEmails.UseCompatibleStateImageBehavior = false;
            this.lvIngressEmails.View = System.Windows.Forms.View.Details;
            this.lvIngressEmails.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvIngressEmails_ColumnClick);
            // 
            // chType
            // 
            this.chType.Text = "Type";
            this.chType.Width = 152;
            // 
            // chName
            // 
            this.chName.Text = "Name";
            this.chName.Width = 99;
            // 
            // chReceived
            // 
            this.chReceived.Text = "Received";
            this.chReceived.Width = 93;
            // 
            // chResponseTime
            // 
            this.chResponseTime.Text = "Response Time";
            this.chResponseTime.Width = 103;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(0, 0);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(93, 24);
            this.btnLoad.TabIndex = 2;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(99, 0);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(93, 24);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // pMain
            // 
            this.pMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pMain.Controls.Add(this.btnShowAll);
            this.pMain.Controls.Add(this.lvSummary);
            this.pMain.Controls.Add(this.btnTypeHierarchy);
            this.pMain.Controls.Add(this.btnLoad);
            this.pMain.Controls.Add(this.btnClear);
            this.pMain.Controls.Add(this.pbSettings);
            this.pMain.Controls.Add(this.lvIngressEmails);
            this.pMain.Location = new System.Drawing.Point(12, 12);
            this.pMain.Name = "pMain";
            this.pMain.Size = new System.Drawing.Size(575, 490);
            this.pMain.TabIndex = 4;
            // 
            // btnShowAll
            // 
            this.btnShowAll.Location = new System.Drawing.Point(297, 0);
            this.btnShowAll.Name = "btnShowAll";
            this.btnShowAll.Size = new System.Drawing.Size(93, 24);
            this.btnShowAll.TabIndex = 6;
            this.btnShowAll.Text = "Show All";
            this.btnShowAll.UseVisualStyleBackColor = true;
            this.btnShowAll.Click += new System.EventHandler(this.btnShowAll_Click);
            // 
            // lvSummary
            // 
            this.lvSummary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvSummary.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chSummaryType,
            this.chSummaryValue});
            this.lvSummary.FullRowSelect = true;
            this.lvSummary.Location = new System.Drawing.Point(0, 341);
            this.lvSummary.Name = "lvSummary";
            this.lvSummary.Size = new System.Drawing.Size(575, 149);
            this.lvSummary.TabIndex = 5;
            this.lvSummary.UseCompatibleStateImageBehavior = false;
            this.lvSummary.View = System.Windows.Forms.View.Details;
            this.lvSummary.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvSummary_ColumnClick);
            // 
            // chSummaryType
            // 
            this.chSummaryType.Text = "Summary Type";
            this.chSummaryType.Width = 146;
            // 
            // chSummaryValue
            // 
            this.chSummaryValue.Text = "Value";
            this.chSummaryValue.Width = 81;
            // 
            // btnTypeHierarchy
            // 
            this.btnTypeHierarchy.Location = new System.Drawing.Point(198, 0);
            this.btnTypeHierarchy.Name = "btnTypeHierarchy";
            this.btnTypeHierarchy.Size = new System.Drawing.Size(93, 24);
            this.btnTypeHierarchy.TabIndex = 4;
            this.btnTypeHierarchy.Text = "Type Hierarchy";
            this.btnTypeHierarchy.UseVisualStyleBackColor = true;
            this.btnTypeHierarchy.Click += new System.EventHandler(this.btnTypeHierarchy_Click);
            // 
            // pbMain
            // 
            this.pbMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbMain.Location = new System.Drawing.Point(12, 508);
            this.pbMain.Name = "pbMain";
            this.pbMain.Size = new System.Drawing.Size(575, 13);
            this.pbMain.Step = 1;
            this.pbMain.TabIndex = 5;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(599, 533);
            this.Controls.Add(this.pbMain);
            this.Controls.Add(this.pMain);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.MinimumSize = new System.Drawing.Size(510, 431);
            this.Name = "MainForm";
            this.Text = "Ingress Email Reader";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbSettings)).EndInit();
            this.pMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList ilMain;
        private System.Windows.Forms.PictureBox pbSettings;
        private System.Windows.Forms.ListView lvIngressEmails;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.ColumnHeader chType;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.ColumnHeader chReceived;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Panel pMain;
        private System.Windows.Forms.ProgressBar pbMain;
        private System.Windows.Forms.ColumnHeader chResponseTime;
        private System.Windows.Forms.ListView lvSummary;
        private System.Windows.Forms.Button btnTypeHierarchy;
        private System.Windows.Forms.Button btnShowAll;
        private System.Windows.Forms.ColumnHeader chSummaryType;
        private System.Windows.Forms.ColumnHeader chSummaryValue;
    }
}

