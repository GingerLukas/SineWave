using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SineWave
{
    public partial class Form1 : Form
    {
        private float[] _r = new[] {75f, 50f, 30f, 20f,10f,5f};
        private float _x = 100f;
        private float _y = 100f;
        private float _graphOffset = 100f;
        private float _pointSize = 4f;
        
        private Stack<float> _data = new Stack<float>();
        private float[] _angles = new float[6];
        private float[] _increments = new[] {0.5f, -0.5f, 0.5f, -0.5f, 0.5f, -0.5f};
        public Form1()
        {
            InitializeComponent();

            DoubleBuffered = true;
            
            _tmrMain.Start();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;



            PointF center = new PointF(_x+_r[0], _y+_r[0]);
            PointF loc = center;
            float offset = center.X + _graphOffset;


            g.DrawLine(Pens.Black, offset, center.Y, Width - 50, center.Y);

            for (int i = 0; i < _r.Length; i++)
            {
                g.DrawEllipse(Pens.Red, center.X - _r[i], center.Y - _r[i], _r[i] * 2, _r[i] * 2);
                loc = GetPointByAngle(_angles[i], center.X, center.Y, (int)_r[i]);
                g.DrawLine(Pens.Blue, center, loc);
                center = loc;
            }

            
            _data.Push(loc.Y);
            g.DrawLine(Pens.Blue, loc, new PointF(offset, loc.Y));



            
            foreach (float y in _data)
            {
                g.FillEllipse(Brushes.Red, offset - _pointSize / 2, y - _pointSize / 2, _pointSize, _pointSize);
                offset += .5f;
            }
        }

        private void _tmrMain_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < _angles.Length; i++)
            {
                _angles[i] += _increments[i];
            }
            Invalidate();
        }

        private PointF GetPointByAngle(float angle, float x, float y, int size)
        {
            double rad = angle * (Math.PI / 180);

            double xx = x + size * Math.Cos(rad);
            double yy = y + size * Math.Sin(rad);

            return new PointF((float) xx, (float) yy);
        }
    }
}
