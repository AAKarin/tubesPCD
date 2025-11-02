using System.Collections.Generic;
using System.Windows.Forms;

namespace MiniPhotoshop.Helpers
{
    public class UIManager
    {
        private readonly List<Control> _managedControls;
        private readonly TrackBar _trackBarBrightness;
        private readonly Label _lblBrightnessValue;
        private readonly TrackBar _trackBarBlackWhite;

        public UIManager(List<Control> controlsToManage,
                         TrackBar trackBarBrightness,
                         Label lblBrightnessValue,
                         TrackBar trackBarBlackWhite)
        {
            _managedControls = controlsToManage;
            _trackBarBrightness = trackBarBrightness;
            _lblBrightnessValue = lblBrightnessValue;
            _trackBarBlackWhite = trackBarBlackWhite;
        }

        public void EnableTools()
        {
            SetControlsEnabled(true);
        }

        public void DisableTools()
        {
            SetControlsEnabled(false);
        }

        public void ResetControls()
        {
            _trackBarBrightness.Value = 0;
            _lblBrightnessValue.Text = "Brightness: 0";
            _trackBarBlackWhite.Value = 2;
        }

        private void SetControlsEnabled(bool isEnabled)
        {
            foreach (var control in _managedControls)
            {
                control.Enabled = isEnabled;
            }
        }
    }
}