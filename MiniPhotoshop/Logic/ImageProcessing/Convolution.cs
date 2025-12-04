using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiniPhotoshop.Logic.Helpers;

namespace MiniPhotoshop.Logic.ImageProcessing
{
    public class Convolution
    {
        /// <summary>
        /// Menampilkan UI Dialog untuk konfigurasi konvolusi, lalu menjalankan proses jika user menekan OK.
        /// </summary>
        public Bitmap RequestConvolution(Bitmap sourceImage)
        {
            using (Form inputForm = new Form())
            {
                // Konfigurasi Form Dasar
                inputForm.Text = "Konvolusi Lengkap";
                inputForm.Size = new Size(500, 720);
                inputForm.StartPosition = FormStartPosition.CenterParent;
                inputForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                inputForm.MaximizeBox = false;

                int yPos = 20, labelX = 20, controlX = 150;

                // ---------------------------------------------------------
                // 1. UI: Ukuran Kernel
                // ---------------------------------------------------------
                Label lblSize = new Label()
                {
                    Text = "Ukuran Kernel:",
                    Left = labelX,
                    Top = yPos,
                    AutoSize = true
                };

                ComboBox cmbSize = new ComboBox()
                {
                    Left = controlX,
                    Top = yPos,
                    Width = 120,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };

                cmbSize.Items.AddRange(new object[] { "3 x 3", "5 x 5", "7 x 7", "Free" });

                NumericUpDown numSize = new NumericUpDown()
                {
                    Left = controlX + 130,
                    Top = yPos,
                    Width = 60,
                    Minimum = 3,
                    Maximum = 21,
                    Increment = 2,
                    Visible = false,
                    Value = 3
                };

                inputForm.Controls.Add(lblSize);
                inputForm.Controls.Add(cmbSize);
                inputForm.Controls.Add(numSize);
                yPos += 40;

                // ---------------------------------------------------------
                // 2. UI: Preset Kernel (Integrasi KernelDatabase)
                // ---------------------------------------------------------
                Label lblIsi = new Label()
                {
                    Text = "Isi Kernel:",
                    Left = labelX,
                    Top = yPos,
                    AutoSize = true
                };

                ComboBox cmbType = new ComboBox()
                {
                    Left = controlX,
                    Top = yPos,
                    Width = 200,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };

                cmbType.Items.AddRange(new object[]
                {
                    "Custom",
                    "Blur (Box)",
                    "Blur (Gaussian)",
                    "Sharpen",
                    "Edge (Simple)",
                    "Edge (Laplacian)",
                    "Edge (Sobel Horizontal)",
                    "Edge (Sobel Vertical)",
                    "Emboss",
                    "Random"
                });

                cmbType.SelectedIndex = 0;

                inputForm.Controls.Add(lblIsi);
                inputForm.Controls.Add(cmbType);
                yPos += 40;

                // ---------------------------------------------------------
                // 3. UI: Padding Options
                // ---------------------------------------------------------
                Label lblPad = new Label()
                {
                    Text = "Padding:",
                    Left = labelX,
                    Top = yPos,
                    AutoSize = true
                };

                ComboBox cmbPad = new ComboBox()
                {
                    Left = controlX,
                    Top = yPos,
                    Width = 200,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };

                cmbPad.Items.AddRange(new object[]
                {
                    "0 (Hitam)",
                    "255 (Putih)",
                    "Replicate",
                    "Random Color"
                });

                cmbPad.SelectedIndex = 2;

                inputForm.Controls.Add(lblPad);
                inputForm.Controls.Add(cmbPad);
                yPos += 40;

                // ---------------------------------------------------------
                // 4. UI: Grid Matrix Editor
                // ---------------------------------------------------------
                Label lblGrid = new Label()
                {
                    Text = "Matriks:",
                    Left = labelX,
                    Top = yPos,
                    AutoSize = true
                };

                DataGridView grid = new DataGridView()
                {
                    Left = labelX,
                    Top = yPos + 25,
                    Width = 440,
                    Height = 300,
                    AllowUserToAddRows = false,
                    AllowUserToDeleteRows = false,
                    ColumnHeadersVisible = false,
                    RowHeadersVisible = false
                };

                inputForm.Controls.Add(lblGrid);
                inputForm.Controls.Add(grid);
                yPos += 340;

                // ---------------------------------------------------------
                // 5. UI: Input Divisor & Offset
                // ---------------------------------------------------------
                Label lblDiv = new Label()
                {
                    Text = "Pembagi:",
                    Left = 20,
                    Top = yPos,
                    AutoSize = true
                };

                NumericUpDown numDivisor = new NumericUpDown()
                {
                    Left = 100,
                    Top = yPos - 3,
                    Width = 80,
                    DecimalPlaces = 2,
                    Minimum = -9999,
                    Maximum = 9999,
                    Value = 1
                };

                Label lblOff = new Label()
                {
                    Text = "Offset:",
                    Left = 260,
                    Top = yPos,
                    AutoSize = true
                };

                NumericUpDown numOffset = new NumericUpDown()
                {
                    Left = 320,
                    Top = yPos - 3,
                    Width = 80,
                    Minimum = -255,
                    Maximum = 255,
                    Value = 0
                };

                inputForm.Controls.Add(lblDiv);
                inputForm.Controls.Add(numDivisor);
                inputForm.Controls.Add(lblOff);
                inputForm.Controls.Add(numOffset);
                yPos += 50;

                // ---------------------------------------------------------
                // 6. Tombol Aksi
                // ---------------------------------------------------------
                Button btnProses = new Button()
                {
                    Text = "Proses",
                    Left = 220,
                    Top = yPos,
                    Width = 120,
                    Height = 40,
                    DialogResult = DialogResult.OK,
                    Font = new Font(Control.DefaultFont, FontStyle.Bold)
                };

                Button btnBatal = new Button()
                {
                    Text = "Batal",
                    Left = 350,
                    Top = yPos,
                    Width = 120,
                    Height = 40,
                    DialogResult = DialogResult.Cancel
                };

                inputForm.Controls.Add(btnProses);
                inputForm.Controls.Add(btnBatal);
                inputForm.AcceptButton = btnProses;

                // ---------------------------------------------------------
                // Event: Setup Grid
                // ---------------------------------------------------------
                Action<int> SetupGrid = (n) =>
                {
                    grid.Rows.Clear();
                    grid.Columns.Clear();

                    int colW = Math.Max(30, grid.Width / n);

                    for (int i = 0; i < n; i++)
                    {
                        grid.Columns.Add("", "");
                        grid.Columns[i].Width = colW;
                    }

                    grid.Rows.Add(n);
                };

                // Event: Preset Kernel
                cmbType.SelectedIndexChanged += (s, e) =>
                {
                    if (cmbType.Text == "Custom") return;

                    int n = grid.RowCount;

                    KernelData data = KernelDatabase.GetKernel(cmbType.Text, n);

                    for (int r = 0; r < n; r++)
                        for (int c = 0; c < n; c++)
                            grid.Rows[r].Cells[c].Value = data.Matrix[r, c];

                    numDivisor.Value = (decimal)data.Divisor;
                    numOffset.Value = data.Offset;
                };

                // Event: Resize Kernel
                cmbSize.SelectedIndexChanged += (s, e) =>
                {
                    if (cmbSize.Text == "Free")
                    {
                        numSize.Visible = true;
                        SetupGrid((int)numSize.Value);
                    }
                    else
                    {
                        numSize.Visible = false;
                        SetupGrid(int.Parse(cmbSize.Text.Substring(0, 1)));
                    }
                };

                numSize.ValueChanged += (s, e) => SetupGrid((int)numSize.Value);

                cmbSize.SelectedIndex = 0;

                // ---------------------------------------------------------
                // Eksekusi proses jika user menekan OK
                // ---------------------------------------------------------
                if (inputForm.ShowDialog() == DialogResult.OK)
                {
                    int n = grid.RowCount;
                    double[,] kernel = new double[n, n];

                    for (int r = 0; r < n; r++)
                        for (int c = 0; c < n; c++)
                            if (grid.Rows[r].Cells[c].Value != null)
                                double.TryParse(grid.Rows[r].Cells[c].Value.ToString(), out kernel[r, c]);

                    double divisor = (double)numDivisor.Value;
                    int offset = (int)numOffset.Value;

                    PaddingType padding = PaddingType.Replicate;

                    if (cmbPad.SelectedIndex == 0) padding = PaddingType.Zero;
                    else if (cmbPad.SelectedIndex == 1) padding = PaddingType.White;
                    else if (cmbPad.SelectedIndex == 3) padding = PaddingType.Random;

                    return ProcessConvolutionFast(sourceImage, kernel, padding, divisor, offset);
                }
            }

            return null;
        }

        /// <summary>
        /// Mesin utama pemrosesan konvolusi menggunakan Parallel Loop dan Direct Memory Access.
        /// </summary>
        public Bitmap ProcessConvolutionFast(Bitmap source, double[,] kernel, PaddingType paddingType, double divisor, int offset)
        {
            if (divisor == 0) divisor = 1;

            Bitmap srcBmp = new Bitmap(source);
            int width = srcBmp.Width;
            int height = srcBmp.Height;

            BitmapData srcData = srcBmp.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb
            );

            int bytes = Math.Abs(srcData.Stride) * height;
            byte[] rgbValues = new byte[bytes];
            byte[] resultValues = new byte[bytes];

            Marshal.Copy(srcData.Scan0, rgbValues, 0, bytes);

            int kH = kernel.GetLength(0);
            int kW = kernel.GetLength(1);
            int radY = kH / 2;
            int radX = kW / 2;
            int stride = srcData.Stride;

            Parallel.For(0, height,
                () => new Random(Guid.NewGuid().GetHashCode()),
                (y, loopState, rng) =>
                {
                    for (int x = 0; x < width; x++)
                    {
                        double sumR = 0, sumG = 0, sumB = 0;

                        for (int ky = -radY; ky <= radY; ky++)
                        {
                            int currentY = y + ky;
                            bool isOutY = false;

                            if (currentY < 0)
                            {
                                currentY = (paddingType == PaddingType.Replicate) ? 0 : -1;
                                isOutY = true;
                            }
                            else if (currentY >= height)
                            {
                                currentY = (paddingType == PaddingType.Replicate) ? height - 1 : -1;
                                isOutY = true;
                            }

                            for (int kx = -radX; kx <= radX; kx++)
                            {
                                int currentX = x + kx;
                                bool isOutX = false;

                                if (currentX < 0)
                                {
                                    currentX = (paddingType == PaddingType.Replicate) ? 0 : -1;
                                    isOutX = true;
                                }
                                else if (currentX >= width)
                                {
                                    currentX = (paddingType == PaddingType.Replicate) ? width - 1 : -1;
                                    isOutX = true;
                                }

                                double r = 0, g = 0, b = 0;

                                if (!isOutY && !isOutX)
                                {
                                    int index = (currentY * stride) + (currentX * 4);
                                    b = rgbValues[index];
                                    g = rgbValues[index + 1];
                                    r = rgbValues[index + 2];
                                }
                                else
                                {
                                    if (paddingType == PaddingType.White)
                                    {
                                        r = 255; g = 255; b = 255;
                                    }
                                    else if (paddingType == PaddingType.Random)
                                    {
                                        r = rng.Next(256);
                                        g = rng.Next(256);
                                        b = rng.Next(256);
                                    }
                                }

                                double w = kernel[ky + radY, kx + radX];
                                sumR += r * w;
                                sumG += g * w;
                                sumB += b * w;
                            }
                        }

                        int finalR = (int)((sumR / divisor) + offset);
                        int finalG = (int)((sumG / divisor) + offset);
                        int finalB = (int)((sumB / divisor) + offset);

                        int finalIdx = (y * stride) + (x * 4);

                        resultValues[finalIdx] = (byte)Math.Min(255, Math.Max(0, finalB));
                        resultValues[finalIdx + 1] = (byte)Math.Min(255, Math.Max(0, finalG));
                        resultValues[finalIdx + 2] = (byte)Math.Min(255, Math.Max(0, finalR));
                        resultValues[finalIdx + 3] = 255;
                    }

                    return rng;
                },
                _ => { }
            );

            srcBmp.UnlockBits(srcData);

            Bitmap resultBmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            BitmapData resData = resultBmp.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly,
                PixelFormat.Format32bppArgb
            );

            Marshal.Copy(resultValues, 0, resData.Scan0, bytes);
            resultBmp.UnlockBits(resData);

            return resultBmp;
        }
    }
}