using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace MiniPhotoshop.Logic.Helpers
{
    public class PseudoColorHelper
    {
        // ===================================================================
        // 1. BAGIAN UNTUK MENU PEWARNAAN SEMU (Hot, Jet, Gold)
        // ===================================================================
        public enum ColorMapType
        {
            Hot,        // Hitam -> Merah -> Kuning -> Putih
            Cool,       // Cyan -> Magenta
            Jet,        // Biru -> Hijau -> Merah (Pelangi/Thermal)
            Gold        // Sepia/Emas
        }

        public Bitmap ApplyPseudoColor(Bitmap source, ColorMapType type)
        {
            if (source == null) return null;

            int w = source.Width;
            int h = source.Height;
            Bitmap dst = new Bitmap(w, h, PixelFormat.Format32bppArgb);

            BitmapData srcData = source.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            BitmapData dstData = dst.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            int bytes = Math.Abs(srcData.Stride) * h;
            byte[] srcBuffer = new byte[bytes];
            byte[] dstBuffer = new byte[bytes];

            Marshal.Copy(srcData.Scan0, srcBuffer, 0, bytes);

            byte[][] palette = CreatePalette(type); // Bikin Palette

            Parallel.For(0, bytes / 4, i =>
            {
                int idx = i * 4;
                byte b = srcBuffer[idx];
                byte g = srcBuffer[idx + 1];
                byte r = srcBuffer[idx + 2];
                byte gray = (byte)((r + g + b) / 3);

                dstBuffer[idx] = palette[gray][0];
                dstBuffer[idx + 1] = palette[gray][1];
                dstBuffer[idx + 2] = palette[gray][2];
                dstBuffer[idx + 3] = 255;
            });

            Marshal.Copy(dstBuffer, 0, dstData.Scan0, bytes);
            source.UnlockBits(srcData);
            dst.UnlockBits(dstData);

            return dst;
        }

        private byte[][] CreatePalette(ColorMapType type)
        {
            byte[][] palette = new byte[256][];
            for (int i = 0; i < 256; i++)
            {
                palette[i] = new byte[3];
                double val = i / 255.0;

                switch (type)
                {
                    case ColorMapType.Hot:
                        palette[i][2] = (byte)(Math.Min(1, val * 3) * 255);       // R
                        palette[i][1] = (byte)(Math.Min(1, Math.Max(0, (val - 0.33) * 3)) * 255); // G
                        palette[i][0] = (byte)(Math.Min(1, Math.Max(0, (val - 0.66) * 3)) * 255); // B
                        break;
                    case ColorMapType.Jet:
                        byte red = 0, green = 0, blue = 0;
                        if (i < 64) { blue = 255; green = (byte)(i * 4); }
                        else if (i < 128) { blue = (byte)((127 - i) * 4); green = 255; }
                        else if (i < 192) { green = 255; red = (byte)((i - 128) * 4); }
                        else { green = (byte)((255 - i) * 4); red = 255; }
                        palette[i][0] = blue; palette[i][1] = green; palette[i][2] = red;
                        break;
                    case ColorMapType.Gold:
                        palette[i][2] = (byte)Math.Min(255, i * 1.2);
                        palette[i][1] = (byte)Math.Min(255, i * 1.0);
                        palette[i][0] = (byte)Math.Min(255, i * 0.4);
                        break;
                    default:
                        palette[i][0] = palette[i][1] = palette[i][2] = (byte)i;
                        break;
                }
            }
            return palette;
        }

        // ===================================================================
        // 2. BAGIAN UTAMA UNTUK MOUSE (ISOLASI WARNA)
        // Bagian ini WAJIB ADA untuk fitur Klik & Tahan
        // ===================================================================
        public Bitmap IsolateColor(Bitmap source, Color targetColor, int threshold)
        {
            if (source == null) return null;

            int w = source.Width;
            int h = source.Height;

            // Siapkan hasil gambar
            Bitmap dst = new Bitmap(w, h, PixelFormat.Format32bppArgb);

            // Kunci memori (LockBits) agar proses secepat kilat
            BitmapData srcData = source.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            BitmapData dstData = dst.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            int bytes = Math.Abs(srcData.Stride) * h;
            byte[] srcBuffer = new byte[bytes];
            byte[] dstBuffer = new byte[bytes]; // Buffer hasil

            // Kita butuh buffer tambahan untuk menyimpan status "Apakah pixel ini terpilih?"
            // Agar nanti kita bisa cek tetangganya.
            bool[] maskBuffer = new bool[w * h];

            // Salin data gambar ke buffer
            Marshal.Copy(srcData.Scan0, srcBuffer, 0, bytes);

            int tR = targetColor.R;
            int tG = targetColor.G;
            int tB = targetColor.B;
            int thresholdSq = threshold * threshold;
            int stride = srcData.Stride;

            // TAHAP 1: BUAT MASKING (Tentukan mana yang dipilih, mana yang tidak)
            // Kita tidak pakai Parallel disini biar gampang akses array bool-nya
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int idx = (y * stride) + (x * 4);

                    byte b = srcBuffer[idx];
                    byte g = srcBuffer[idx + 1];
                    byte r = srcBuffer[idx + 2];

                    int distR = r - tR;
                    int distG = g - tG;
                    int distB = b - tB;
                    int distanceSq = (distR * distR) + (distG * distG) + (distB * distB);

                    // Simpan status di array mask (True = Terpilih, False = Tidak)
                    maskBuffer[y * w + x] = (distanceSq < thresholdSq);
                }
            }

            // TAHAP 2: WARNAI GAMBAR & GAMBAR GARIS BATAS
            Parallel.For(0, h, y =>
            {
                for (int x = 0; x < w; x++)
                {
                    int idx = (y * stride) + (x * 4);
                    int maskIdx = y * w + x;
                    bool isSelected = maskBuffer[maskIdx];

                    // --- DETEKSI TEPI (BORDER CHECK) ---
                    // Cek pixel tetangga (Atas, Bawah, Kiri, Kanan)
                    // Jika pixel ini terpilih, tapi tetangganya TIDAK terpilih -> Berarti ini BATAS.
                    bool isBorder = false;

                    if (isSelected)
                    {
                        // Cek tetangga (pastikan tidak keluar batas gambar)
                        bool top = (y > 0) ? maskBuffer[maskIdx - w] : false;
                        bool bottom = (y < h - 1) ? maskBuffer[maskIdx + w] : false;
                        bool left = (x > 0) ? maskBuffer[maskIdx - 1] : false;
                        bool right = (x < w - 1) ? maskBuffer[maskIdx + 1] : false;

                        // Jika salah satu tetangga bernilai 'false' (bukan area terpilih), maka ini adalah pinggir
                        if (!top || !bottom || !left || !right)
                        {
                            isBorder = true;
                        }
                    }

                    if (isBorder)
                    {
                        // GAMBAR GARIS PUTUS-PUTUS (Static Ants)
                        // Pola: Tiap 8 pixel, ganti warna (Hitam/Putih)
                        if ((x + y) % 8 < 4)
                        {
                            // Putih
                            dstBuffer[idx] = 255; dstBuffer[idx + 1] = 255; dstBuffer[idx + 2] = 255;
                        }
                        else
                        {
                            // Hitam
                            dstBuffer[idx] = 0; dstBuffer[idx + 1] = 0; dstBuffer[idx + 2] = 0;
                        }
                        dstBuffer[idx + 3] = 255; // Alpha
                    }
                    else if (isSelected)
                    {
                        // AREA TERPILIH -> Warna Asli
                        dstBuffer[idx] = srcBuffer[idx];         // Blue
                        dstBuffer[idx + 1] = srcBuffer[idx + 1]; // Green
                        dstBuffer[idx + 2] = srcBuffer[idx + 2]; // Red
                        dstBuffer[idx + 3] = 255;
                    }
                    else
                    {
                        // BACKGROUND -> Abu-abu
                        byte b = srcBuffer[idx];
                        byte g = srcBuffer[idx + 1];
                        byte r = srcBuffer[idx + 2];
                        byte gray = (byte)((r + g + b) / 3);

                        dstBuffer[idx] = gray;
                        dstBuffer[idx + 1] = gray;
                        dstBuffer[idx + 2] = gray;
                        dstBuffer[idx + 3] = 255;
                    }
                }
            });

            // Salin balik ke Bitmap
            Marshal.Copy(dstBuffer, 0, dstData.Scan0, bytes);
            source.UnlockBits(srcData);
            dst.UnlockBits(dstData);

            return dst;
        }
    }
}