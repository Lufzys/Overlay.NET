using SharpGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpGL.RenderContextProviders;

namespace Overlay.NET
{
    public class Drawings
    {
        private OpenGL GL;
        private DrawManager drawManager;
        public Drawings(OpenGL gl) 
        {
            GL = gl;
            drawManager = new DrawManager(gl);
        }

        public void AddLine(Color color, Vector2 src, Vector2 dst, float width = 1f, bool outline = false)
        {
            DrawManager.DrawItem item = new DrawManager.DrawItem();
            item.type = DrawManager.DrawType.Line;
            item.color = color;
            item.src = src;
            item.dst = dst;
            item.width = width;
            item.outline = outline;

            drawManager.DrawList.Add(item);
        }

        public void AddRectangle(Color color, Vector2 src, Vector2 size, float width = 1f, bool filled = false, bool outline = false)
        {
            DrawManager.DrawItem item = new DrawManager.DrawItem();
            item.type = DrawManager.DrawType.Rectangle;
            item.color = color;
            item.src = src;
            item.dst = size;
            item.width = width;
            item.filled = filled;
            item.outline = outline;

            drawManager.DrawList.Add(item);
        }

        public void AddEllipse(Color color, Vector2 src, Vector2 size, int numSegments = 32, float width = 1f, bool filled = false, bool outline = false)
        {
            DrawManager.DrawItem item = new DrawManager.DrawItem();
            item.type = DrawManager.DrawType.Ellipse;
            item.color = color;
            item.src = src;
            item.dst = size;
            item.width = width;
            item.filled = filled;
            item.segments = numSegments;
            item.outline = outline;

            drawManager.DrawList.Add(item);
        }

        public void AddEllipse(Color color, Vector2 src, float radius, int numSegments = 32, float width = 1f, bool filled = false, bool outline = false)
        {
            AddEllipse(color, src, new Vector2(radius, radius), numSegments, width, filled, outline);
        }

        public void AddText(Color color, Vector2 src, string text, string fontName = "Arial", float fontSize = 12f)
        {
            DrawManager.DrawItem item = new DrawManager.DrawItem();
            item.type = DrawManager.DrawType.Text;
            item.color = color;
            item.src = src;
            item.text = text;
            item.fontName = fontName;
            item.fontSize = fontSize;

            drawManager.DrawList.Add(item);
        }

        public void Render()
        {
            drawManager.Render();
        }
    }
}
