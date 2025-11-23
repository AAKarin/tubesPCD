using System;
using System.Drawing;
using System.Drawing.Drawing2D; 
using System.Windows.Forms;     

namespace MiniPhotoshop.Logic.Geometry
{
    public class Distortion
    {
        public Bitmap RequestDistortion(Bitmap sourceImage)
        {
            using (Form inputForm = new Form())
            {
                inputForm.Text = "Distorsi (Shearing)";
                inputForm.Size = new Size(300, 220);
                inputForm.StartPosition = FormStartPosition.CenterParent;
                inputForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                inputForm.MinimizeBox = false;
                inputForm.MaximizeBox = false;

                // Info untuk user agar tidak memasukkan angka aneh
                Label lblInfo = new Label()
                {
                    Left = 20,
                    Top = 10,
                    Width = 250,
                    Height = 30,
                    Text = "Masukkan nilai kecil (cth: 0.5, -0.2, 1.5)\n0 = Tidak ada distorsi.",
                    ForeColor = Color.Gray
                };

                Label lblX = new Label() { Left = 20, Top = 50, Text = "Shear X (Hor):" };
                TextBox txtX = new TextBox() { Left = 120, Top = 50, Width = 100, Text = "0.5" };

                Label lblY = new Label() { Left = 20, Top = 90, Text = "Shear Y (Ver):" };
                TextBox txtY = new TextBox() { Left = 120, Top = 90, Width = 100, Text = "0" };

                Button btnOk = new Button() { Text = "Distorsi", Left = 100, Width = 80, Top = 130, DialogResult = DialogResult.OK };

                inputForm.Controls.Add(lblInfo);
                inputForm.Controls.Add(lblX);
                inputForm.Controls.Add(txtX);
                inputForm.Controls.Add(lblY);
                inputForm.Controls.Add(txtY);
                inputForm.Controls.Add(btnOk);
                inputForm.AcceptButton = btnOk;

                if (inputForm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Pakai float karena butuh desimal (koma)
                        float shearX = float.Parse(txtX.Text);
                        float shearY = float.Parse(txtY.Text);

                        return ProcessShear(sourceImage, shearX, shearY);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Input harus angka (boleh desimal)! Error: " + ex.Message);
                    }
                }
            }
            return null;
        }

        public Bitmap ProcessShear(Bitmap sourceImage, float shearX, float shearY)
        {
            // 1. Siapkan Matrix Shearing
            Matrix matrix = new Matrix();
            matrix.Shear(shearX, shearY);

            // 2. Hitung Ukuran Baru (Bounding Box)
            // Karena ditarik miring, ukuran gambar pasti melebar/meninggi
            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(new RectangleF(0f, 0f, sourceImage.Width, sourceImage.Height));
            path.Transform(matrix); // Terapkan matrix ke kotak bayangan

            RectangleF bounds = path.GetBounds(); // Ambil ukuran kotak baru

            // 3. Buat Bitmap Baru
            Bitmap newImage = new Bitmap((int)bounds.Width, (int)bounds.Height);

            using (Graphics g = Graphics.FromImage(newImage))
            {
                g.Clear(Color.Black);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                // 4. Geser Sistem Koordinat
                // Saat di-shear, gambar bisa lari ke koordinat negatif.
                // Kita harus geser balik (Translate) agar pas di tengah kanvas baru.
                Matrix m = new Matrix();
                m.Translate(-bounds.Left, -bounds.Top);

                // Gabungkan dengan efek shearing tadi
                m.Shear(shearX, shearY);

                g.Transform = m;

                g.DrawImage(sourceImage, 0, 0);
            }

            return newImage;
        }
    }
}