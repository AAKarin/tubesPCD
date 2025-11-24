using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Drawing.Drawing2D; // Wajib untuk Matrix & GraphicsPath
using System.Windows.Forms; // Wajib untuk Dialog UI

namespace MiniPhotoshop.Logic.Geometry
{
    public class Rotation
    {
        // ==================================================================
        // BAGIAN 1: LOGIKA UI (DIALOG INPUT)
        // Digunakan khusus untuk menu "Free"
        // ==================================================================
        public Bitmap RequestRotation(Bitmap sourceImage)
        {
            using (Form inputForm = new Form())
            {
                inputForm.Text = "Rotasi Bebas";
                inputForm.Size = new Size(300, 160);
                inputForm.StartPosition = FormStartPosition.CenterParent;
                inputForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                inputForm.MinimizeBox = false;
                inputForm.MaximizeBox = false;

                Label lblAngle = new Label() { Left = 20, Top = 20, Text = "Sudut (Derajat):" };
                TextBox txtAngle = new TextBox() { Left = 120, Top = 20, Width = 100, Text = "45" };
                Label lblInfo = new Label()
                {
                    Left = 20,
                    Top = 50,
                    Text = "(Positif = Searah jarum jam)",
                    ForeColor = Color.Gray,
                    Width = 200
                };
                Button btnOk = new Button() { Text = "Putar", Left = 100, Width = 80, Top = 80, DialogResult = DialogResult.OK };
                inputForm.Controls.Add(lblAngle);
                inputForm.Controls.Add(txtAngle);
                inputForm.Controls.Add(lblInfo);
                inputForm.Controls.Add(btnOk);
                inputForm.AcceptButton = btnOk;
                if (inputForm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        float angle = float.Parse(txtAngle.Text);
                        return ProcessRotation(sourceImage, angle);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Input harus angka! Error: " + ex.Message);
                    }
                }
            }
            return null;
        }

        // ==================================================================
        // BAGIAN 2: LOGIKA INTI (MATEMATIKA)
        // Digunakan oleh semua menu (45, 90, 180, 270, Free)
        // ==================================================================
        public Bitmap ProcessRotation(Bitmap sourceImage, float angle)
        {
            // 1. Siapkan Matrix Rotasi di titik pusat gambar
            Matrix matrix = new Matrix();
            matrix.RotateAt(angle, new PointF(sourceImage.Width / 2, sourceImage.Height / 2));

            // 2. Hitung Ukuran Baru (Bounding Box)
            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(new RectangleF(0f, 0f, sourceImage.Width, sourceImage.Height));
            path.Transform(matrix);
            RectangleF bounds = path.GetBounds();

            // 3. Buat Bitmap Baru
            Bitmap newImage = new Bitmap((int)bounds.Width, (int)bounds.Height);
            using (Graphics g = Graphics.FromImage(newImage))
            {
                g.Clear(Color.Black); // Background Hitam
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                // 4. Transformasi Koordinat (Geser -> Putar -> Geser Balik)
                Matrix m = new Matrix();
                m.Translate(newImage.Width / 2.0f, newImage.Height / 2.0f);
                m.Rotate(angle);
                m.Translate(-sourceImage.Width / 2.0f, -sourceImage.Height / 2.0f);
                g.Transform = m;
                g.DrawImage(sourceImage, 0, 0);
            }
            return newImage;
        }
    }
}

