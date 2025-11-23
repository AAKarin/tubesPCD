using System;
using System.Drawing;
using System.Windows.Forms;

namespace MiniPhotoshop.Logic.Geometry
{
    public class Translation
    {
        public Bitmap RequestTranslation(Bitmap sourceImage)
        {
            using (Form inputForm = new Form())
            {
                inputForm.Text = "Atur Pergeseran (Translasi)";
                inputForm.Size = new Size(300, 200);
                inputForm.StartPosition = FormStartPosition.CenterParent;
                inputForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                inputForm.MinimizeBox = false;
                inputForm.MaximizeBox = false;

                // Label dan Input untuk X
                Label lblX = new Label() { Left = 20, Top = 20, Text = "Geser X (px):" };
                TextBox txtX = new TextBox() { Left = 120, Top = 20, Width = 100, Text = "50" };

                // Label dan Input untuk Y
                Label lblY = new Label() { Left = 20, Top = 60, Text = "Geser Y (px):" };
                TextBox txtY = new TextBox() { Left = 120, Top = 60, Width = 100, Text = "50" };

                // Tombol OK
                Button btnOk = new Button() { Text = "Terapkan", Left = 100, Width = 80, Top = 100, DialogResult = DialogResult.OK };

                inputForm.Controls.Add(lblX);
                inputForm.Controls.Add(txtX);
                inputForm.Controls.Add(lblY);
                inputForm.Controls.Add(txtY);
                inputForm.Controls.Add(btnOk);
                inputForm.AcceptButton = btnOk;

                // Tampilkan Dialog
                if (inputForm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        int xVal = int.Parse(txtX.Text);
                        int yVal = int.Parse(txtY.Text);

                        // Panggil proses inti
                        return ProcessTranslation(sourceImage, xVal, yVal);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Input harus angka bulat! Error: " + ex.Message);
                    }
                }
            }
            return null; // Jika cancel atau error
        }

        public Bitmap ProcessTranslation(Bitmap sourceImage, int xOffset, int yOffset)
        {
            Bitmap newImage = new Bitmap(sourceImage.Width, sourceImage.Height);

            using (Graphics g = Graphics.FromImage(newImage))
            {
                // Isi background dengan hitam (sesuai request sebelumnya)
                g.Clear(Color.Black);

                // Gambar ulang citra asli di posisi baru
                g.DrawImage(sourceImage, xOffset, yOffset);
            }

            return newImage;
        }
    }
}