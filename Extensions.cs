using SharpGL;
using SharpGL.Version;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Overlay.NET
{
    public static class Extensions
    {
        public static void SetTransparent(this Form frm)
        {
            frm.BackColor = Color.Black;

            IntPtr hWnd = frm.Handle;

            int exStyle = Win32.GetWindowLong(hWnd, Win32.GWL_EXSTYLE);
            Win32.SetWindowLong(hWnd, -20, exStyle | Win32.WS_EX_LAYERED | 0x20);
            Win32.SetLayeredWindowAttributes(hWnd, 0, 0, Win32.LWA_COLORKEY);
        }

        public static Vector2 GetDisplaySize(this OpenGL GL)
        {
            int[] viewport = new int[4];
            GL.GetInteger(OpenGL.GL_VIEWPORT, viewport);
            int width = viewport[2];
            int height = viewport[3];
            return new Vector2(width, height);
        }


        public static Vector2 MeasureText(string text, string fontName, float fontSize)
        {
            var size = TextRenderer.MeasureText(text, new Font(fontName, fontSize - 3.5F));
            return new Vector2(size.Width, size.Height);
        }

        public static bool Initalize(this OpenGL gl, OpenGLVersion OpenGLVersion, RenderContextType RenderContextType, object parameter, int Width, int Height, bool antiAlias = false)
        {
            //  Create the render context.
            bool isCreated = gl.Create(OpenGLVersion, RenderContextType, Width, Height, 32, parameter);

            //  Set the most basic OpenGL styles.
            gl.ShadeModel(OpenGL.GL_SMOOTH);
            gl.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
            gl.ClearDepth(1.0f);
            gl.Enable(OpenGL.GL_DEPTH_TEST);
            gl.DepthFunc(OpenGL.GL_LEQUAL);
            gl.Hint(OpenGL.GL_PERSPECTIVE_CORRECTION_HINT, OpenGL.GL_NICEST);
            if(antiAlias)
            {
                gl.Enable(OpenGL.GL_LINE_SMOOTH);
                gl.Enable(OpenGL.GL_BLEND);
                gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
                gl.Hint(OpenGL.GL_LINE_SMOOTH_HINT, OpenGL.GL_NICEST);
            }
            return isCreated;
        }
    }
}
