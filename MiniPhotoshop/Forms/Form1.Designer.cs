namespace MiniPhotoshop
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            histogramRToolStripMenuItem = new ToolStripMenuItem();
            histogramGToolStripMenuItem = new ToolStripMenuItem();
            histogramBToolStripMenuItem = new ToolStripMenuItem();
            histogramGrToolStripMenuItem = new ToolStripMenuItem();
            splitContainer1 = new SplitContainer();
            lblBrightnessValue = new Label();
            trackBarBrightness = new TrackBar();
            label1 = new Label();
            trackBarBlackWhite = new TrackBar();
            button10 = new Button();
            button8 = new Button();
            button7 = new Button();
            button6 = new Button();
            button5 = new Button();
            button4 = new Button();
            button3 = new Button();
            button2 = new Button();
            button1 = new Button();
            pictureBox5 = new PictureBox();
            pictureBox4 = new PictureBox();
            pictureBox3 = new PictureBox();
            thumbPictureBox1 = new PictureBox();
            pictureBoxHistogram = new PictureBox();
            pictureBox1 = new PictureBox();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBarBrightness).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBarBlackWhite).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)thumbPictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxHistogram).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = SystemColors.ButtonHighlight;
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1280, 28);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { histogramRToolStripMenuItem, histogramGToolStripMenuItem, histogramBToolStripMenuItem, histogramGrToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(55, 24);
            fileToolStripMenuItem.Text = "View";
            // 
            // histogramRToolStripMenuItem
            // 
            histogramRToolStripMenuItem.Name = "histogramRToolStripMenuItem";
            histogramRToolStripMenuItem.Size = new Size(181, 26);
            histogramRToolStripMenuItem.Text = "Histogram R";
            // 
            // histogramGToolStripMenuItem
            // 
            histogramGToolStripMenuItem.Name = "histogramGToolStripMenuItem";
            histogramGToolStripMenuItem.Size = new Size(181, 26);
            histogramGToolStripMenuItem.Text = "Histogram G";
            // 
            // histogramBToolStripMenuItem
            // 
            histogramBToolStripMenuItem.Name = "histogramBToolStripMenuItem";
            histogramBToolStripMenuItem.Size = new Size(181, 26);
            histogramBToolStripMenuItem.Text = "Histogram B";
            // 
            // histogramGrToolStripMenuItem
            // 
            histogramGrToolStripMenuItem.Name = "histogramGrToolStripMenuItem";
            histogramGrToolStripMenuItem.Size = new Size(181, 26);
            histogramGrToolStripMenuItem.Text = "Histogram Gr";
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 28);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(lblBrightnessValue);
            splitContainer1.Panel1.Controls.Add(trackBarBrightness);
            splitContainer1.Panel1.Controls.Add(label1);
            splitContainer1.Panel1.Controls.Add(trackBarBlackWhite);
            splitContainer1.Panel1.Controls.Add(button10);
            splitContainer1.Panel1.Controls.Add(button8);
            splitContainer1.Panel1.Controls.Add(button7);
            splitContainer1.Panel1.Controls.Add(button6);
            splitContainer1.Panel1.Controls.Add(button5);
            splitContainer1.Panel1.Controls.Add(button4);
            splitContainer1.Panel1.Controls.Add(button3);
            splitContainer1.Panel1.Controls.Add(button2);
            splitContainer1.Panel1.Controls.Add(button1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(pictureBox5);
            splitContainer1.Panel2.Controls.Add(pictureBox4);
            splitContainer1.Panel2.Controls.Add(pictureBox3);
            splitContainer1.Panel2.Controls.Add(thumbPictureBox1);
            splitContainer1.Panel2.Controls.Add(pictureBoxHistogram);
            splitContainer1.Panel2.Controls.Add(pictureBox1);
            splitContainer1.Size = new Size(1280, 692);
            splitContainer1.SplitterDistance = 267;
            splitContainer1.TabIndex = 1;
            // 
            // lblBrightnessValue
            // 
            lblBrightnessValue.AutoSize = true;
            lblBrightnessValue.Location = new Point(83, 436);
            lblBrightnessValue.Name = "lblBrightnessValue";
            lblBrightnessValue.Size = new Size(77, 20);
            lblBrightnessValue.TabIndex = 12;
            lblBrightnessValue.Text = "Brightness";
            // 
            // trackBarBrightness
            // 
            trackBarBrightness.Location = new Point(22, 460);
            trackBarBrightness.Maximum = 255;
            trackBarBrightness.Minimum = -255;
            trackBarBrightness.Name = "trackBarBrightness";
            trackBarBrightness.Size = new Size(205, 56);
            trackBarBrightness.TabIndex = 11;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(83, 354);
            label1.Name = "label1";
            label1.Size = new Size(91, 20);
            label1.TabIndex = 2;
            label1.Text = "Black & White";
            // 
            // trackBarBlackWhite
            // 
            trackBarBlackWhite.Location = new Point(22, 377);
            trackBarBlackWhite.Maximum = 4;
            trackBarBlackWhite.Name = "trackBarBlackWhite";
            trackBarBlackWhite.Size = new Size(205, 56);
            trackBarBlackWhite.TabIndex = 10;
            trackBarBlackWhite.Value = 2;
            trackBarBlackWhite.Scroll += trackBarBlackWhite_Scroll;
            // 
            // button10
            // 
            button10.Location = new Point(22, 301);
            button10.Name = "button10";
            button10.Size = new Size(111, 29);
            button10.TabIndex = 9;
            button10.Text = "Negation";
            button10.UseVisualStyleBackColor = true;
            // 
            // button8
            // 
            button8.Location = new Point(22, 266);
            button8.Name = "button8";
            button8.Size = new Size(111, 29);
            button8.TabIndex = 7;
            button8.Text = "Selection Tool";
            button8.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            button7.Location = new Point(22, 161);
            button7.Name = "button7";
            button7.Size = new Size(111, 29);
            button7.TabIndex = 6;
            button7.Text = "Show Blue";
            button7.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            button6.Location = new Point(22, 196);
            button6.Name = "button6";
            button6.Size = new Size(111, 29);
            button6.TabIndex = 5;
            button6.Text = "Show Green";
            button6.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            button5.Location = new Point(22, 126);
            button5.Name = "button5";
            button5.Size = new Size(111, 29);
            button5.TabIndex = 4;
            button5.Text = "Show Red";
            button5.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            button4.Location = new Point(22, 231);
            button4.Name = "button4";
            button4.Size = new Size(111, 29);
            button4.TabIndex = 3;
            button4.Text = "Grayscale";
            button4.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.Location = new Point(22, 91);
            button3.Name = "button3";
            button3.Size = new Size(111, 29);
            button3.TabIndex = 2;
            button3.Text = "Restore Color";
            button3.UseVisualStyleBackColor = true;
            button3.Click += buttonRestore_Click;
            // 
            // button2
            // 
            button2.Location = new Point(63, 560);
            button2.Name = "button2";
            button2.Size = new Size(111, 29);
            button2.TabIndex = 1;
            button2.Text = "Save Gambar";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.Location = new Point(22, 28);
            button1.Name = "button1";
            button1.Size = new Size(220, 29);
            button1.TabIndex = 0;
            button1.Text = "Tambah Gambar";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // pictureBox5
            // 
            pictureBox5.BorderStyle = BorderStyle.FixedSingle;
            pictureBox5.Location = new Point(761, 509);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new Size(226, 145);
            pictureBox5.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox5.TabIndex = 5;
            pictureBox5.TabStop = false;
            // 
            // pictureBox4
            // 
            pictureBox4.BorderStyle = BorderStyle.FixedSingle;
            pictureBox4.Location = new Point(515, 509);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(226, 145);
            pictureBox4.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox4.TabIndex = 4;
            pictureBox4.TabStop = false;
            // 
            // pictureBox3
            // 
            pictureBox3.BorderStyle = BorderStyle.FixedSingle;
            pictureBox3.Location = new Point(268, 509);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(226, 145);
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 3;
            pictureBox3.TabStop = false;
            // 
            // thumbPictureBox1
            // 
            this.thumbPictureBox1.AllowDrop = true; // <-- TAMBAHKAN INI (WAJIB UNTUK DROP)
            this.thumbPictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.thumbPictureBox1.Location = new System.Drawing.Point(17, 509);
            this.thumbPictureBox1.Name = "thumbPictureBox1"; // (Pastikan nama ini konsisten dengan Form1.cs)
            this.thumbPictureBox1.Size = new System.Drawing.Size(226, 145);
            this.thumbPictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom; // <-- TAMBAHKAN INI (Untuk ukuran)
            this.thumbPictureBox1.TabIndex = 2;
            this.thumbPictureBox1.TabStop = false;
            // 
            // pictureBoxHistogram
            // 
            pictureBoxHistogram.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxHistogram.Location = new Point(510, 20);
            pictureBoxHistogram.Name = "pictureBoxHistogram";
            pictureBoxHistogram.Size = new Size(477, 375);
            pictureBoxHistogram.TabIndex = 1;
            pictureBoxHistogram.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.AllowDrop = true;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(17, 20);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(477, 375);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1280, 720);
            Controls.Add(splitContainer1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "Form1";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)trackBarBrightness).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBarBlackWhite).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)thumbPictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxHistogram).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem histogramRToolStripMenuItem;
        private ToolStripMenuItem histogramGToolStripMenuItem;
        private SplitContainer splitContainer1;
        private Button button1;
        private PictureBox pictureBox1;
        private Button button2;
        private Button button4;
        private Button button3;
        private Button button7;
        private Button button6;
        private Button button5;
        private ToolStripMenuItem histogramBToolStripMenuItem;
        private ToolStripMenuItem histogramGrToolStripMenuItem;
        private PictureBox pictureBoxHistogram;
        private Button button8;
        private Button button10;
        private TrackBar trackBarBlackWhite;
        private Label label1;
        private TrackBar trackBarBrightness;
        private Label lblBrightnessValue;
        private PictureBox pictureBox5;
        private PictureBox pictureBox4;
        private PictureBox pictureBox3;
        private PictureBox thumbPictureBox1;
    }
}
