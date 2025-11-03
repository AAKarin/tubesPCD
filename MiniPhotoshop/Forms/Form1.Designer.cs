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
            aritmatikaToolStripMenuItem = new ToolStripMenuItem();
            tambahToolStripMenuItem = new ToolStripMenuItem();
            kurangToolStripMenuItem = new ToolStripMenuItem();
            kaliToolStripMenuItem = new ToolStripMenuItem();
            bagiToolStripMenuItem = new ToolStripMenuItem();
            binerToolStripMenuItem = new ToolStripMenuItem();
            aNDToolStripMenuItem = new ToolStripMenuItem();
            oRToolStripMenuItem = new ToolStripMenuItem();
            nEGATIONToolStripMenuItem = new ToolStripMenuItem();
            xORToolStripMenuItem = new ToolStripMenuItem();
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
            thumbPictureBox4 = new PictureBox();
            thumbPictureBox3 = new PictureBox();
            thumbPictureBox2 = new PictureBox();
            thumbPictureBox1 = new PictureBox();
            pictureBoxHistogram = new PictureBox();
            pictureBox1 = new PictureBox();
            radioButtonThum1 = new RadioButton();
            radioButtonThum2 = new RadioButton();
            radioButtonThum3 = new RadioButton();
            radioButtonThum4 = new RadioButton();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBarBrightness).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBarBlackWhite).BeginInit();
            ((System.ComponentModel.ISupportInitialize)thumbPictureBox4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)thumbPictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)thumbPictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)thumbPictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxHistogram).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = SystemColors.ButtonHighlight;
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, aritmatikaToolStripMenuItem, binerToolStripMenuItem });
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
            // aritmatikaToolStripMenuItem
            // 
            aritmatikaToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { tambahToolStripMenuItem, kurangToolStripMenuItem, kaliToolStripMenuItem, bagiToolStripMenuItem });
            aritmatikaToolStripMenuItem.Name = "aritmatikaToolStripMenuItem";
            aritmatikaToolStripMenuItem.Size = new Size(92, 24);
            aritmatikaToolStripMenuItem.Text = "Aritmatika";
            // 
            // tambahToolStripMenuItem
            // 
            tambahToolStripMenuItem.Name = "tambahToolStripMenuItem";
            tambahToolStripMenuItem.Size = new Size(144, 26);
            tambahToolStripMenuItem.Text = "Tambah";
            tambahToolStripMenuItem.Click += tambahToolStripMenuItem_Click;
            // 
            // kurangToolStripMenuItem
            // 
            kurangToolStripMenuItem.Name = "kurangToolStripMenuItem";
            kurangToolStripMenuItem.Size = new Size(144, 26);
            kurangToolStripMenuItem.Text = "Kurang";
            kurangToolStripMenuItem.Click += kurangToolStripMenuItem_Click;
            // 
            // kaliToolStripMenuItem
            // 
            kaliToolStripMenuItem.Name = "kaliToolStripMenuItem";
            kaliToolStripMenuItem.Size = new Size(144, 26);
            kaliToolStripMenuItem.Text = "Kali";
            kaliToolStripMenuItem.Click += kaliToolStripMenuItem_Click;
            // 
            // bagiToolStripMenuItem
            // 
            bagiToolStripMenuItem.Name = "bagiToolStripMenuItem";
            bagiToolStripMenuItem.Size = new Size(144, 26);
            bagiToolStripMenuItem.Text = "Bagi";
            bagiToolStripMenuItem.Click += bagiToolStripMenuItem_Click;
            // 
            // binerToolStripMenuItem
            // 
            binerToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aNDToolStripMenuItem, oRToolStripMenuItem, nEGATIONToolStripMenuItem, xORToolStripMenuItem });
            binerToolStripMenuItem.Name = "binerToolStripMenuItem";
            binerToolStripMenuItem.Size = new Size(57, 24);
            binerToolStripMenuItem.Text = "Biner";
            // 
            // aNDToolStripMenuItem
            // 
            aNDToolStripMenuItem.Name = "aNDToolStripMenuItem";
            aNDToolStripMenuItem.Size = new Size(224, 26);
            aNDToolStripMenuItem.Text = "AND";
            aNDToolStripMenuItem.Click += aNDToolStripMenuItem_Click;
            // 
            // oRToolStripMenuItem
            // 
            oRToolStripMenuItem.Name = "oRToolStripMenuItem";
            oRToolStripMenuItem.Size = new Size(224, 26);
            oRToolStripMenuItem.Text = "OR";
            oRToolStripMenuItem.Click += oRToolStripMenuItem_Click;
            // 
            // nEGATIONToolStripMenuItem
            // 
            nEGATIONToolStripMenuItem.Name = "nEGATIONToolStripMenuItem";
            nEGATIONToolStripMenuItem.Size = new Size(224, 26);
            nEGATIONToolStripMenuItem.Text = "NEGATION";
            nEGATIONToolStripMenuItem.Click += nEGATIONToolStripMenuItem_Click;
            // 
            // xORToolStripMenuItem
            // 
            xORToolStripMenuItem.Name = "xORToolStripMenuItem";
            xORToolStripMenuItem.Size = new Size(224, 26);
            xORToolStripMenuItem.Text = "XOR";
            xORToolStripMenuItem.Click += xORToolStripMenuItem_Click;
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
            splitContainer1.Panel2.Controls.Add(radioButtonThum4);
            splitContainer1.Panel2.Controls.Add(radioButtonThum3);
            splitContainer1.Panel2.Controls.Add(radioButtonThum2);
            splitContainer1.Panel2.Controls.Add(radioButtonThum1);
            splitContainer1.Panel2.Controls.Add(thumbPictureBox4);
            splitContainer1.Panel2.Controls.Add(thumbPictureBox3);
            splitContainer1.Panel2.Controls.Add(thumbPictureBox2);
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
            // thumbPictureBox4
            // 
            thumbPictureBox4.AllowDrop = true;
            thumbPictureBox4.BorderStyle = BorderStyle.FixedSingle;
            thumbPictureBox4.Location = new Point(761, 509);
            thumbPictureBox4.Name = "thumbPictureBox4";
            thumbPictureBox4.Size = new Size(226, 145);
            thumbPictureBox4.SizeMode = PictureBoxSizeMode.Zoom;
            thumbPictureBox4.TabIndex = 5;
            thumbPictureBox4.TabStop = false;
            // 
            // thumbPictureBox3
            // 
            thumbPictureBox3.AllowDrop = true;
            thumbPictureBox3.BorderStyle = BorderStyle.FixedSingle;
            thumbPictureBox3.Location = new Point(515, 509);
            thumbPictureBox3.Name = "thumbPictureBox3";
            thumbPictureBox3.Size = new Size(226, 145);
            thumbPictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            thumbPictureBox3.TabIndex = 4;
            thumbPictureBox3.TabStop = false;
            // 
            // thumbPictureBox2
            // 
            thumbPictureBox2.AllowDrop = true;
            thumbPictureBox2.BorderStyle = BorderStyle.FixedSingle;
            thumbPictureBox2.Location = new Point(268, 509);
            thumbPictureBox2.Name = "thumbPictureBox2";
            thumbPictureBox2.Size = new Size(226, 145);
            thumbPictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            thumbPictureBox2.TabIndex = 3;
            thumbPictureBox2.TabStop = false;
            // 
            // thumbPictureBox1
            // 
            thumbPictureBox1.AllowDrop = true;
            thumbPictureBox1.BorderStyle = BorderStyle.FixedSingle;
            thumbPictureBox1.Location = new Point(17, 509);
            thumbPictureBox1.Name = "thumbPictureBox1";
            thumbPictureBox1.Size = new Size(226, 145);
            thumbPictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            thumbPictureBox1.TabIndex = 2;
            thumbPictureBox1.TabStop = false;
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
            pictureBox1.AllowDrop = true;
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            pictureBox1.Location = new Point(17, 20);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(477, 375);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // radioButtonThum1
            // 
            radioButtonThum1.AutoSize = true;
            radioButtonThum1.Location = new Point(17, 479);
            radioButtonThum1.Name = "radioButtonThum1";
            radioButtonThum1.Size = new Size(95, 24);
            radioButtonThum1.TabIndex = 9;
            radioButtonThum1.TabStop = true;
            radioButtonThum1.Text = "Gambar 1";
            radioButtonThum1.UseVisualStyleBackColor = true;
            // 
            // radioButtonThum2
            // 
            radioButtonThum2.AutoSize = true;
            radioButtonThum2.Location = new Point(268, 479);
            radioButtonThum2.Name = "radioButtonThum2";
            radioButtonThum2.Size = new Size(95, 24);
            radioButtonThum2.TabIndex = 10;
            radioButtonThum2.TabStop = true;
            radioButtonThum2.Text = "Gambar 2";
            radioButtonThum2.UseVisualStyleBackColor = true;
            // 
            // radioButtonThum3
            // 
            radioButtonThum3.AutoSize = true;
            radioButtonThum3.Location = new Point(515, 479);
            radioButtonThum3.Name = "radioButtonThum3";
            radioButtonThum3.Size = new Size(95, 24);
            radioButtonThum3.TabIndex = 11;
            radioButtonThum3.TabStop = true;
            radioButtonThum3.Text = "Gambar 3";
            radioButtonThum3.UseVisualStyleBackColor = true;
            // 
            // radioButtonThum4
            // 
            radioButtonThum4.AutoSize = true;
            radioButtonThum4.Location = new Point(761, 479);
            radioButtonThum4.Name = "radioButtonThum4";
            radioButtonThum4.Size = new Size(95, 24);
            radioButtonThum4.TabIndex = 12;
            radioButtonThum4.TabStop = true;
            radioButtonThum4.Text = "Gambar 4";
            radioButtonThum4.UseVisualStyleBackColor = true;
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
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)trackBarBrightness).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBarBlackWhite).EndInit();
            ((System.ComponentModel.ISupportInitialize)thumbPictureBox4).EndInit();
            ((System.ComponentModel.ISupportInitialize)thumbPictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)thumbPictureBox2).EndInit();
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
        private PictureBox thumbPictureBox4;
        private PictureBox thumbPictureBox3;
        private PictureBox thumbPictureBox2;
        private PictureBox thumbPictureBox1;
        private ToolStripMenuItem aritmatikaToolStripMenuItem;
        private ToolStripMenuItem tambahToolStripMenuItem;
        private ToolStripMenuItem kurangToolStripMenuItem;
        private ToolStripMenuItem kaliToolStripMenuItem;
        private ToolStripMenuItem bagiToolStripMenuItem;
        private ToolStripMenuItem binerToolStripMenuItem;
        private ToolStripMenuItem aNDToolStripMenuItem;
        private ToolStripMenuItem oRToolStripMenuItem;
        private ToolStripMenuItem nEGATIONToolStripMenuItem;
        private ToolStripMenuItem xORToolStripMenuItem;
        private RadioButton radioButtonThum1;
        private RadioButton radioButtonThum4;
        private RadioButton radioButtonThum3;
        private RadioButton radioButtonThum2;
    }
}
