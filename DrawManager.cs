using SharpGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;

namespace Overlay.NET
{
    internal class DrawManager
    {
        public List<DrawItem> DrawList = new List<DrawItem>();
        private OpenGL GL;
        public DrawManager(OpenGL gl)
        {
            GL = gl;
        }

        private void DrawLine(DrawItem drawItem)
        {
            GL.LineWidth(drawItem.width);
            GL.Color(drawItem.color.R, drawItem.color.G, drawItem.color.B);
            GL.Begin(OpenGL.GL_LINES);
            GL.Vertex(drawItem.src.X, drawItem.src.Y);
            GL.Vertex(drawItem.dst.X, drawItem.dst.Y);
            GL.End();
        }

        private void DrawRectangle(DrawItem drawItem)
        {
            GL.LineWidth(drawItem.width);
            GL.Color(drawItem.color.R, drawItem.color.G, drawItem.color.B);
            GL.Begin(drawItem.filled ? OpenGL.GL_QUADS : OpenGL.GL_LINE_LOOP);
            GL.Vertex(drawItem.src.X, drawItem.src.Y);
            GL.Vertex(drawItem.src.X + drawItem.dst.X, drawItem.src.Y);
            GL.Vertex(drawItem.src.X + drawItem.dst.X, drawItem.src.Y + drawItem.dst.Y);
            GL.Vertex(drawItem.src.X, drawItem.src.Y + drawItem.dst.Y);
            GL.End();
        }

        private void DrawEllipse(DrawItem drawItem)
        {
            GL.LineWidth(drawItem.width);
            GL.Color(drawItem.color.R, drawItem.color.G, drawItem.color.B);
            GL.Begin(drawItem.filled ? OpenGL.GL_TRIANGLE_FAN : OpenGL.GL_LINE_LOOP);

            for (int i = 0; i < drawItem.segments; i++)
            {
                float angle = (float)i * (360.0f / drawItem.segments);
                float rad = angle * (float)Math.PI / 180.0f;
                float px = drawItem.src.X + drawItem.dst.X * (float)Math.Cos(rad);
                float py = drawItem.src.Y + drawItem.dst.Y * (float)Math.Sin(rad);
                GL.Vertex(px, py);
            }

            GL.End();
        }

        private void DrawText(DrawItem drawItem)
        {
            var size = Extensions.MeasureText(drawItem.text, drawItem.fontName, drawItem.fontSize);
            GL.DrawText((int)(drawItem.src.X), (int)(GL.GetDisplaySize().Y - size.Y / 1.25 - drawItem.src.Y), drawItem.color.R, drawItem.color.G, drawItem.color.B, drawItem.fontName, drawItem.fontSize, drawItem.text);
        }

        public void Render()
        {
            foreach(var drawItem in DrawList)
            {
                if(drawItem.type == DrawType.Line)
                {
                    if(drawItem.outline)
                    {
                        DrawItem drawItem1 = drawItem.Clone();
                        drawItem1.color = Color.FromArgb(2, 2, 2);
                        drawItem1.width = drawItem.width + 2f;
                        DrawLine(drawItem1);
                    }
                    DrawLine(drawItem);
                }
                else if (drawItem.type == DrawType.Rectangle)
                {
                    if (drawItem.outline)
                    {
                        DrawItem drawItem1 = drawItem.Clone(); ;
                        drawItem1.color = Color.FromArgb(2, 2, 2);
                        drawItem1.width = drawItem.width + 2f;
                        DrawLine(drawItem1);
                    }
                    DrawRectangle(drawItem);
                }
                else if(drawItem.type == DrawType.Ellipse)
                {
                    if (drawItem.outline)
                    {
                        DrawItem drawItem1 = drawItem.Clone(); ;
                        drawItem1.color = Color.FromArgb(2, 2, 2);
                        drawItem1.width = drawItem.width + 2f;
                        DrawLine(drawItem1);
                    }
                    DrawEllipse(drawItem);
                }
                else if (drawItem.type == DrawType.Text)
                {
                    DrawText(drawItem);
                }
            }
            DrawList.Clear();
        }

        public enum DrawType
        {
            Line,
            Rectangle,
            Ellipse,
            Text
        }

        public struct DrawItem : ICloneable
        {
            public DrawType type;
            public Color color;
            public Vector2 src;
            public Vector2 dst;
            public float width;
            public bool filled;
            public int segments;
            public string text;
            public string fontName;
            public float fontSize;
            public bool outline;

            public DrawItem Clone()
            {
                return this;
            }
            object ICloneable.Clone()
            {
                return Clone();
            }
        }
    }
}
