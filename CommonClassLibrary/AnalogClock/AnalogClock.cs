﻿namespace AnalogClock
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Text;
    using System.Globalization;
    using System.Windows.Forms;

    public class AnalogClock : UserControl
    {
        private Calendar _calendar = new GregorianCalendar();
        private CalendarTypes _calendartype;
        private string _caption = "SiRFaware";
        private PointF _center;
        private ClockStyles _clockstyle;
        private DateStyles _datestyle;
        private Color _handcolor = Color.Black;
        private HandStyles _handstyle;
        private Color _innercolor = Color.SkyBlue;
        private NumberStyles _numberstyle;
        private Color _outercolor = Color.SteelBlue;
        private float _radius;
        private Color _secondhandcolor = Color.Red;
        private Color _textcolor = Color.Black;
        private Color _tickcolor = Color.Black;
        private TickStyles _tickstyle;
        private IContainer components;
        private BufferedGraphicsContext CurrentContext = BufferedGraphicsManager.Current;
        public bool ShowHourMinute = true;
        public int StartSecond = DateTime.Now.Second;
        private Timer tmrAnalogClock;

        public AnalogClock()
        {
            this.InitializeComponent();
            this._radius = (base.Width * 0.9f) / 2f;
            this._center = new PointF((float) (base.Width / 2), (float) (base.Height / 2));
        }

        private void AnalogClock_Resize(object sender, EventArgs e)
        {
            if (base.Width < base.Height)
            {
                this._radius = (base.Width * 0.9f) / 2f;
            }
            else
            {
                this._radius = (base.Height * 0.9f) / 2f;
            }
            this._center = new PointF(((float) base.Width) / 2f, ((float) base.Height) / 2f);
            this.Refresh();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DrawBackground(Graphics g)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse((float) (this._center.X - this._radius), (float) (this._center.Y - this._radius), (float) (this._radius * 2f), (float) (this._radius * 2f));
            PathGradientBrush brush = new PathGradientBrush(path);
            brush.CenterColor = this._innercolor;
            brush.SurroundColors = new Color[] { this._outercolor };
            g.FillEllipse(brush, (float) (this._center.X - this._radius), (float) (this._center.Y - this._radius), (float) (this._radius * 2f), (float) (this._radius * 2f));
            g.DrawEllipse(new Pen(this._tickcolor, this._radius * 0.01f), (float) (this._center.X - this._radius), (float) (this._center.Y - this._radius), (float) (this._radius * 2f), (float) (this._radius * 2f));
        }

        private void DrawCaption(Graphics g)
        {
            if (this._caption.Trim() != string.Empty)
            {
                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                g.DrawString(this._caption, new Font("Tahoma", this._radius * 0.1f, FontStyle.Bold), new SolidBrush(this._textcolor), this._center.X, this._center.Y - (this._radius * 0.4f), format);
            }
        }

        private void DrawDate(Graphics g)
        {
            if (this._datestyle != DateStyles.None)
            {
                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                g.FillRectangle(Brushes.White, (float) (this._center.X + (this._radius * 0.7f)), (float) (this._center.Y - (this._radius * 0.06f)), (float) (this._radius * 0.16f), (float) (this._radius * 0.12f));
                g.DrawRectangle(new Pen(Color.Black, this._radius * 0.01f), (float) (this._center.X + (this._radius * 0.7f)), (float) (this._center.Y - (this._radius * 0.06f)), (float) (this._radius * 0.16f), (float) (this._radius * 0.12f));
                g.DrawString(this._calendar.GetDayOfMonth(DateTime.Now).ToString(), new Font("Lucida Console", this._radius * 0.08f, FontStyle.Bold), Brushes.Black, this._center.X + (this._radius * 0.785f), this._center.Y - (this._radius * 0.04f), format);
                if (this._datestyle == DateStyles.Full)
                {
                    g.FillRectangle(Brushes.White, (float) (this._center.X + (this._radius * 0.46f)), (float) (this._center.Y - (this._radius * 0.06f)), (float) (this._radius * 0.24f), (float) (this._radius * 0.12f));
                    g.DrawRectangle(new Pen(Color.Black, this._radius * 0.01f), (float) (this._center.X + (this._radius * 0.46f)), (float) (this._center.Y - (this._radius * 0.06f)), (float) (this._radius * 0.24f), (float) (this._radius * 0.12f));
                    g.DrawString(DateTime.Now.DayOfWeek.ToString().Substring(0, 3).ToUpper(), new Font("Lucida Console", this._radius * 0.09f, FontStyle.Bold), Brushes.Black, this._center.X + (this._radius * 0.585f), this._center.Y - (this._radius * 0.05f), format);
                }
            }
        }

        private void DrawHands(Graphics g)
        {
            int num = DateTime.Now.Hour % 12;
            int minute = DateTime.Now.Minute;
            int second = DateTime.Now.Second - this.StartSecond;
            if (this.ShowHourMinute)
            {
                second = DateTime.Now.Second;
            }
            if (this._clockstyle == ClockStyles.Classic)
            {
                Color color = Color.FromArgb((this._innercolor.R + this._outercolor.R) / 2, (this._innercolor.G + this._outercolor.G) / 2, (this._innercolor.B + this._outercolor.B) / 2);
                g.FillEllipse(new SolidBrush(color), (float) (this._center.X - (this._radius * 0.25f)), (float) (this._center.Y + (this._radius * 0.25f)), (float) (this._radius * 0.5f), (float) (this._radius * 0.5f));
                Pen pen = new Pen(this._tickcolor);
                pen.DashStyle = DashStyle.Custom;
                pen.DashPattern = new float[] { 1f, 3f };
                g.DrawEllipse(pen, (float) (this._center.X - (this._radius * 0.25f)), (float) (this._center.Y + (this._radius * 0.25f)), (float) (this._radius * 0.5f), (float) (this._radius * 0.5f));
                g.DrawLine(new Pen(this._secondhandcolor, this._radius * 0.01f), (float) (this._center.X - ((((float) Math.Sin(((second * 6) * 3.1415926535897931) / 180.0)) * this._radius) * 0.05f)), (float) ((this._center.Y + (this._radius * 0.5f)) - ((((float) -Math.Cos(((second * 6) * 3.1415926535897931) / 180.0)) * this._radius) * 0.05f)), (float) (((((float) Math.Sin(((second * 6) * 3.1415926535897931) / 180.0)) * this._radius) * 0.25f) + this._center.X), (float) ((((((float) -Math.Cos(((second * 6) * 3.1415926535897931) / 180.0)) * this._radius) * 0.25f) + this._center.Y) + (this._radius * 0.5f)));
                g.FillEllipse(new SolidBrush(this._secondhandcolor), (float) (this._center.X - (this._radius * 0.02f)), (float) (this._center.Y + (this._radius * 0.48f)), (float) (this._radius * 0.04f), (float) (this._radius * 0.04f));
            }
            if (this._handstyle == HandStyles.Uniform)
            {
                if (this.ShowHourMinute)
                {
                    g.DrawLine(new Pen(this._handcolor, this._radius * 0.05f), (float) (this._center.X - ((((float) Math.Sin((((num * 30) + ((minute / 12) * 6)) * 3.1415926535897931) / 180.0)) * this._radius) * 0.1f)), (float) (this._center.Y - ((((float) -Math.Cos((((num * 30) + ((minute / 12) * 6)) * 3.1415926535897931) / 180.0)) * this._radius) * 0.1f)), (float) (((((float) Math.Sin((((num * 30) + ((minute / 12) * 6)) * 3.1415926535897931) / 180.0)) * this._radius) * 0.5f) + this._center.X), (float) (((((float) -Math.Cos((((num * 30) + ((minute / 12) * 6)) * 3.1415926535897931) / 180.0)) * this._radius) * 0.5f) + this._center.Y));
                    g.DrawLine(new Pen(this._handcolor, this._radius * 0.03f), (float) (this._center.X - ((((float) Math.Sin(((minute * 6) * 3.1415926535897931) / 180.0)) * this._radius) * 0.1f)), (float) (this._center.Y - ((((float) -Math.Cos(((minute * 6) * 3.1415926535897931) / 180.0)) * this._radius) * 0.1f)), (float) (((((float) Math.Sin(((minute * 6) * 3.1415926535897931) / 180.0)) * this._radius) * 0.7f) + this._center.X), (float) (((((float) -Math.Cos(((minute * 6) * 3.1415926535897931) / 180.0)) * this._radius) * 0.7f) + this._center.Y));
                }
                g.FillEllipse(new SolidBrush(this._handcolor), (float) (this._center.X - (this._radius * 0.05f)), (float) (this._center.Y - (this._radius * 0.05f)), (float) (this._radius * 0.1f), (float) (this._radius * 0.1f));
            }
            else
            {
                PointF[] points = new PointF[] { new PointF(this._center.X - ((((float) Math.Sin((((num * 30) + ((minute / 12) * 6)) * 3.1415926535897931) / 180.0)) * this._radius) * 0.1f), this._center.Y - ((((float) -Math.Cos((((num * 30) + ((minute / 12) * 6)) * 3.1415926535897931) / 180.0)) * this._radius) * 0.1f)), new PointF(this._center.X - ((((float) Math.Sin(((((num * 30) + ((minute / 12) * 6)) + 90) * 3.1415926535897931) / 180.0)) * this._radius) * 0.05f), this._center.Y - ((((float) -Math.Cos(((((num * 30) + ((minute / 12) * 6)) + 90) * 3.1415926535897931) / 180.0)) * this._radius) * 0.05f)), new PointF(((((float) Math.Sin((((num * 30) + ((minute / 12) * 6)) * 3.1415926535897931) / 180.0)) * this._radius) * 0.5f) + this._center.X, ((((float) -Math.Cos((((num * 30) + ((minute / 12) * 6)) * 3.1415926535897931) / 180.0)) * this._radius) * 0.5f) + this._center.Y), new PointF(this._center.X - ((((float) Math.Sin(((((num * 30) + ((minute / 12) * 6)) - 90) * 3.1415926535897931) / 180.0)) * this._radius) * 0.05f), this._center.Y - ((((float) -Math.Cos(((((num * 30) + ((minute / 12) * 6)) - 90) * 3.1415926535897931) / 180.0)) * this._radius) * 0.05f)) };
                g.FillPolygon(new SolidBrush(this._handcolor), points);
                PointF[] tfArray2 = new PointF[] { new PointF(this._center.X - ((((float) Math.Sin(((minute * 6) * 3.1415926535897931) / 180.0)) * this._radius) * 0.1f), this._center.Y - ((((float) -Math.Cos(((minute * 6) * 3.1415926535897931) / 180.0)) * this._radius) * 0.1f)), new PointF(this._center.X - ((((float) Math.Sin((((minute * 6) + 90) * 3.1415926535897931) / 180.0)) * this._radius) * 0.05f), this._center.Y - ((((float) -Math.Cos((((minute * 6) + 90) * 3.1415926535897931) / 180.0)) * this._radius) * 0.05f)), new PointF(((((float) Math.Sin(((minute * 6) * 3.1415926535897931) / 180.0)) * this._radius) * 0.7f) + this._center.X, ((((float) -Math.Cos(((minute * 6) * 3.1415926535897931) / 180.0)) * this._radius) * 0.7f) + this._center.Y), new PointF(this._center.X - ((((float) Math.Sin((((minute * 6) - 90) * 3.1415926535897931) / 180.0)) * this._radius) * 0.05f), this._center.Y - ((((float) -Math.Cos((((minute * 6) - 90) * 3.1415926535897931) / 180.0)) * this._radius) * 0.05f)) };
                g.FillPolygon(new SolidBrush(this._handcolor), tfArray2);
            }
            if (this._clockstyle == ClockStyles.Standard)
            {
                g.DrawLine(new Pen(this._secondhandcolor, this._radius * 0.01f), (float) (this._center.X - ((((float) Math.Sin(((second * 6) * 3.1415926535897931) / 180.0)) * this._radius) * 0.2f)), (float) (this._center.Y - ((((float) -Math.Cos(((second * 6) * 3.1415926535897931) / 180.0)) * this._radius) * 0.2f)), (float) (((((float) Math.Sin(((second * 6) * 3.1415926535897931) / 180.0)) * this._radius) * 0.9f) + this._center.X), (float) (((((float) -Math.Cos(((second * 6) * 3.1415926535897931) / 180.0)) * this._radius) * 0.9f) + this._center.Y));
                g.FillEllipse(new SolidBrush(this._secondhandcolor), (float) (this._center.X - (this._radius * 0.03f)), (float) (this._center.Y - (this._radius * 0.03f)), (float) (this._radius * 0.06f), (float) (this._radius * 0.06f));
            }
        }

        private void DrawNumbers(Graphics g)
        {
            if (this._numberstyle != NumberStyles.None)
            {
                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                int num = 12;
                for (int i = 1; i <= num; i++)
                {
                    int num3 = i;
                    if (!this.ShowHourMinute)
                    {
                        num3 = i * 5;
                    }
                    switch (i)
                    {
                        case 10:
                            if (this._numberstyle == NumberStyles.All)
                            {
                                g.DrawString(num3.ToString(), new Font("Tahoma", this._radius * 0.15f, FontStyle.Bold), new SolidBrush(this._textcolor), ((((float) Math.Sin(((i * 30) * 3.1415926535897931) / 180.0)) * this._radius) * 0.72f) + this._center.X, ((((float) -Math.Cos(((i * 30) * 3.1415926535897931) / 180.0)) * this._radius) * 0.8f) + this._center.Y, format);
                            }
                            break;

                        case 11:
                            if (this._numberstyle == NumberStyles.All)
                            {
                                g.DrawString(num3.ToString(), new Font("Tahoma", this._radius * 0.15f, FontStyle.Bold), new SolidBrush(this._textcolor), ((((float) Math.Sin(((i * 30) * 3.1415926535897931) / 180.0)) * this._radius) * 0.64f) + this._center.X, ((((float) -Math.Cos(((i * 30) * 3.1415926535897931) / 180.0)) * this._radius) * 0.8f) + this._center.Y, format);
                            }
                            break;

                        default:
                            if ((this._numberstyle != NumberStyles.Quadric) || ((i % 3) == 0))
                            {
                                if ((i == 3) && (this._datestyle != DateStyles.None))
                                {
                                    g.DrawString(num3.ToString(), new Font("Tahoma", this._radius * 0.15f, FontStyle.Bold), new SolidBrush(this._textcolor), ((((float) Math.Sin(((i * 30) * 3.1415926535897931) / 180.0)) * this._radius) * 0.8f) + this._center.X, ((((float) -Math.Cos(((i * 30) * 3.1415926535897931) / 180.0)) * this._radius) * 0.8f) + this._center.Y, format);
                                }
                                else if ((i != 6) || (this._clockstyle != ClockStyles.Classic))
                                {
                                    g.DrawString(num3.ToString(), new Font("Tahoma", this._radius * 0.15f, FontStyle.Bold), new SolidBrush(this._textcolor), ((((float) Math.Sin(((i * 30) * 3.1415926535897931) / 180.0)) * this._radius) * 0.8f) + this._center.X, ((((float) -Math.Cos(((i * 30) * 3.1415926535897931) / 180.0)) * this._radius) * 0.8f) + this._center.Y, format);
                                }
                            }
                            break;
                    }
                }
            }
        }

        private void DrawTicks(Graphics g)
        {
            if (this._tickstyle != TickStyles.None)
            {
                for (int i = 0; i < 4; i++)
                {
                    g.DrawLine(new Pen(this._tickcolor, this._radius * 0.05f), (float) (((((float) Math.Cos(((i * 90) * 3.1415926535897931) / 180.0)) * this._radius) * 0.9f) + this._center.X), (float) (((((float) Math.Sin(((i * 90) * 3.1415926535897931) / 180.0)) * this._radius) * 0.9f) + this._center.Y), (float) ((((float) Math.Cos(((i * 90) * 3.1415926535897931) / 180.0)) * this._radius) + this._center.X), (float) ((((float) Math.Sin(((i * 90) * 3.1415926535897931) / 180.0)) * this._radius) + this._center.Y));
                }
                if ((this._tickstyle == TickStyles.All) || (this._tickstyle == TickStyles.Twelve))
                {
                    for (int j = 0; j < 12; j++)
                    {
                        if ((j % 3) != 0)
                        {
                            g.DrawLine(new Pen(this._tickcolor, this._radius * 0.03f), (float) (((((float) Math.Cos(((j * 30) * 3.1415926535897931) / 180.0)) * this._radius) * 0.9f) + this._center.X), (float) (((((float) Math.Sin(((j * 30) * 3.1415926535897931) / 180.0)) * this._radius) * 0.9f) + this._center.Y), (float) ((((float) Math.Cos(((j * 30) * 3.1415926535897931) / 180.0)) * this._radius) + this._center.X), (float) ((((float) Math.Sin(((j * 30) * 3.1415926535897931) / 180.0)) * this._radius) + this._center.Y));
                        }
                    }
                }
                if (this._tickstyle == TickStyles.All)
                {
                    for (int k = 0; k < 60; k++)
                    {
                        if ((k % 5) != 0)
                        {
                            g.DrawLine(new Pen(this._tickcolor, this._radius * 0.01f), (float) (((((float) Math.Cos(((k * 6) * 3.1415926535897931) / 180.0)) * this._radius) * 0.95f) + this._center.X), (float) (((((float) Math.Sin(((k * 6) * 3.1415926535897931) / 180.0)) * this._radius) * 0.95f) + this._center.Y), (float) ((((float) Math.Cos(((k * 6) * 3.1415926535897931) / 180.0)) * this._radius) + this._center.X), (float) ((((float) Math.Sin(((k * 6) * 3.1415926535897931) / 180.0)) * this._radius) + this._center.Y));
                        }
                    }
                }
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.tmrAnalogClock = new Timer(this.components);
            base.SuspendLayout();
            this.tmrAnalogClock.Interval = 0x3e8;
            this.tmrAnalogClock.Tick += new EventHandler(this.tmrAnalogClock_Tick);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.MinimumSize = new Size(50, 50);
            base.Name = "AnalogClock";
            base.Resize += new EventHandler(this.AnalogClock_Resize);
            base.ResumeLayout(false);
        }

        public void Start()
        {
            this.tmrAnalogClock.Enabled = true;
        }

        public void Stop()
        {
            this.tmrAnalogClock.Enabled = false;
        }

        private void tmrAnalogClock_Tick(object sender, EventArgs e)
        {
            BufferedGraphics graphics = this.CurrentContext.Allocate(base.CreateGraphics(), base.ClientRectangle);
            graphics.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            graphics.Graphics.Clear(this.BackColor);
            this.DrawBackground(graphics.Graphics);
            this.DrawCaption(graphics.Graphics);
            this.DrawTicks(graphics.Graphics);
            this.DrawNumbers(graphics.Graphics);
            this.DrawHands(graphics.Graphics);
            graphics.Render();
            graphics.Dispose();
        }

        public CalendarTypes CalendarType
        {
            get
            {
                return this._calendartype;
            }
            set
            {
                if (value == CalendarTypes.Gregorian)
                {
                    this._calendar = new GregorianCalendar();
                }
                else
                {
                    this._calendar = new PersianCalendar();
                }
                this._calendartype = value;
            }
        }

        public string Caption
        {
            get
            {
                return this._caption;
            }
            set
            {
                this._caption = value;
            }
        }

        public ClockStyles ClockStyle
        {
            get
            {
                return this._clockstyle;
            }
            set
            {
                this._clockstyle = value;
            }
        }

        public DateStyles DateStyle
        {
            get
            {
                return this._datestyle;
            }
            set
            {
                this._datestyle = value;
            }
        }

        public bool Enabled
        {
            get
            {
                return this.tmrAnalogClock.Enabled;
            }
            set
            {
                this.tmrAnalogClock.Enabled = value;
            }
        }

        public Color HandColor
        {
            get
            {
                return this._handcolor;
            }
            set
            {
                this._handcolor = value;
            }
        }

        public HandStyles HandStyle
        {
            get
            {
                return this._handstyle;
            }
            set
            {
                this._handstyle = value;
            }
        }

        public Color InnerColor
        {
            get
            {
                return this._innercolor;
            }
            set
            {
                this._innercolor = value;
            }
        }

        public NumberStyles NumberStyle
        {
            get
            {
                return this._numberstyle;
            }
            set
            {
                this._numberstyle = value;
            }
        }

        public Color OuterColor
        {
            get
            {
                return this._outercolor;
            }
            set
            {
                this._outercolor = value;
            }
        }

        public Color SecondHandColor
        {
            get
            {
                return this._secondhandcolor;
            }
            set
            {
                this._secondhandcolor = value;
            }
        }

        public Color TextColor
        {
            get
            {
                return this._textcolor;
            }
            set
            {
                this._textcolor = value;
            }
        }

        public Color TickColor
        {
            get
            {
                return this._tickcolor;
            }
            set
            {
                this._tickcolor = value;
            }
        }

        public TickStyles TickStyle
        {
            get
            {
                return this._tickstyle;
            }
            set
            {
                this._tickstyle = value;
            }
        }

        public enum CalendarTypes
        {
            Gregorian,
            Persian
        }

        public enum ClockStyles
        {
            Standard,
            Classic
        }

        public enum DateStyles
        {
            Full,
            DayOfMonth,
            None
        }

        public enum HandStyles
        {
            Uniform,
            Sharp
        }

        public enum NumberStyles
        {
            All,
            Quadric,
            None
        }

        public enum TickStyles
        {
            All,
            Twelve,
            Quadric,
            None
        }
    }
}

