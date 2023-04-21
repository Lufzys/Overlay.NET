using SharpGL;
using SharpGL.Version;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Overlay.NET
{
    public class Overlay : Form
    {
        #region OpenGL Constans
        public RenderContextType RenderContextType { get; } = RenderContextType.NativeWindow;
        public OpenGLVersion OpenGLVersion { get; } = OpenGLVersion.OpenGL4_4;
        #endregion

        #region Variables
        public Drawings Drawings;
        private OpenGL GL;
        private Graphics gfx;
        private readonly Timer UpdateFrame = new Timer();
        private readonly Stopwatch sw = new Stopwatch();
        public double FrameTime;
        private float _fps = 60;
        private bool DrawFPS { get; set; } = true;
        public float FPS
        {
            get
            {
                return _fps;
            }
            set
            {
                if(value > 0 && value <= 1000)
                {
                    _fps = value;
                    UpdateFrame.Interval = (int)(1000.0 / _fps);
                }
            }
        }
        #endregion

        #region Events
        public delegate void OpenGLHandler(object sender, OpenGL GL);

        public OpenGLHandler OpenGL_Initalized;
        public OpenGLHandler OpenGL_Render;
        public OpenGLHandler OpenGL_Resized;
        #endregion

        #region Constructor
        public Overlay()
        {
            // Set graphics
            GL = new OpenGL();
            gfx = this.CreateGraphics();
            Drawings = new Drawings(GL);

            this.TopMost = true;
            //this.ShowInTaskbar = false;
            this.ShowIcon = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.Black;
            this.Left = 1; 
            this.Top = 1;
            this.Width = Screen.PrimaryScreen.Bounds.Width - 2;
            this.Height = Screen.PrimaryScreen.Bounds.Height - 2;
            this.SetTransparent();

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            object parameter = null;

            //  Native render context providers need a little bit more attention.
            if (RenderContextType == RenderContextType.NativeWindow)
            {
                SetStyle(ControlStyles.OptimizedDoubleBuffer, false);
                parameter = Handle;
            }

            GL.Initalize(OpenGLVersion, RenderContextType, parameter, this.Width, this.Height, true);
            OpenGL_Initalized?.Invoke(this, GL);

            UpdateFrame.Interval = (int)(1000.0 / FPS);
            UpdateFrame.Tick += new EventHandler(UpdateFrame_Tick);
            UpdateFrame.Enabled = true;
        }

        private void UpdateFrame_Tick(object sender, EventArgs e)
        {
            sw.Restart();

            // Set Context Target
            GL.MakeCurrent();

            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            GL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            GL.MatrixMode(OpenGL.GL_PROJECTION);
            GL.LoadIdentity();
            GL.Ortho2D(0, this.Width - 1, this.Height - 1, 0);
            GL.Disable(OpenGL.GL_DEPTH_TEST);
            GL.Enable(OpenGL.GL_BLEND);
            GL.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);

            OpenGL_Render?.Invoke(this, GL);
            Drawings.Render();

            // Draw FPS
            if(DrawFPS)
            {
                GL.DrawText(10, this.Height - 20, 1f, 1f, 1f, "Arial", 12f, $"fps {(int)(1000.0 / FrameTime)}");
                GL.Flush();
            }

            // Set HDC
            var handleDeviceContext = gfx.GetHdc();
            GL.Blit(handleDeviceContext);
            gfx.ReleaseHdc(handleDeviceContext);

            sw.Stop();
            FrameTime = sw.Elapsed.TotalMilliseconds;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            if (GL.RenderContextProvider == null)
                return;

            GL.SetDimensions(this.Width, this.Height);
            GL.Viewport(0, 0, this.Width, this.Height);

            GL.MatrixMode(OpenGL.GL_PROJECTION);
            GL.LoadIdentity();

            // Calculate The Aspect Ratio Of The Window
            GL.Perspective(45.0f, (float)Width / (float)Height, 0.1f, 100.0f);

            GL.MatrixMode(OpenGL.GL_MODELVIEW);
            GL.LoadIdentity();

            OpenGL_Resized?.Invoke(this, GL);
        }
        #endregion
    }
}
