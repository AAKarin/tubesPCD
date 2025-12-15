using System.Drawing;
using MiniPhotoshop.Logic.Histogram; // PENTING: Panggil folder Logic/Histogram

namespace MiniPhotoshop.Logic.Helpers
{
    public class HistogramManager
    {
        // Constructor
        public HistogramManager()
        {
        }

        // ------------------------------------------
        // TUGAS 1: ADAPTIVE EQUALIZATION (Milik Anda)
        // ------------------------------------------
        public Bitmap ProcessAdaptiveEqualization(Bitmap source)
        {
            if (source == null) return null;

            // Kita pakai grid 8x8 (Standar)
            AdaptiveHistogram algo = new AdaptiveHistogram(8, 8);
            return algo.Apply(source);
        }

        // Nanti method teman-teman Anda ditaruh di bawah sini...


        // Global Equalization
        public Bitmap ProcessGlobalEqualization(Bitmap source)
        {
            if (source == null) return null;
            GlobalHistogram algo = new GlobalHistogram();
            return algo.Apply(source);
        }
    }
}