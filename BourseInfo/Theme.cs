using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BourseInfo
{
    public class Theme
    {
        private Color _negative;

        public Color Negative => _negative;

        public Color Positive => _positive;

        public Color Neutral => _neutral;

        public Color NotificationBackground => _notificationBackground;

        public Color TextBox => _textBox;

        public ThemeName Name => _name;

        public double NotificationOpacity => _notificationOpacity;

        private Color _positive;
        private Color _neutral;
        private Color _notificationBackground;
        private Color _textBox;
        private ThemeName _name;
        private double _notificationOpacity;

        private Theme()
        {
        }

        public Theme(ThemeName name)
        {
            _name = name;
            _negative = Color.Tomato;
            _positive = Color.LimeGreen;
            _textBox = Color.Gray;

            switch (name)
            {
                case ThemeName.Dark:
                    _neutral = Color.LightGray;
                    _notificationBackground = Color.DarkSlateGray;
                    _notificationOpacity = 0.8;
                    break;

                case ThemeName.Light:
                    _neutral = Color.DarkSlateGray;
                    _notificationBackground = Color.Snow;
                    _notificationOpacity = 0.2;
                    break;
            }
        }

        public enum ThemeName
        {
            Dark,
            Light
        }
    }
}
