# Overlay.NET
Basic OpenGL Overlay
- Packages -> SharpGL

Basic Usage
```csharp
      internal class Program
      {
          private static Overlay.NET.Overlay overlay;
          static void Main(string[] args)
          {
              overlay = new Overlay.NET.Overlay();
              overlay.FPS = 1000;
              overlay.OpenGL_Render += new Overlay.NET.Overlay.OpenGLHandler(Overlay_Render);
              overlay.ShowDialog();
          }

          static int numSegments = 32;
          private static void Overlay_Render(object sender, SharpGL.OpenGL GL)
          {
              var cursorPos = overlay.PointToClient(Cursor.Position);
              overlay.Drawings.AddRectangle(Color.Blue, new System.Numerics.Vector2(cursorPos.X, cursorPos.Y), new System.Numerics.Vector2(60, 90));
              overlay.Drawings.AddLine(Color.Red, new System.Numerics.Vector2(10, 10), new System.Numerics.Vector2(cursorPos.X, cursorPos.Y));
              overlay.Drawings.AddEllipse(Color.Red, new System.Numerics.Vector2(cursorPos.X, cursorPos.Y), 25, numSegments: numSegments);
              overlay.Drawings.AddRectangle(Color.FromArgb(2, 2, 2), new System.Numerics.Vector2(cursorPos.X, cursorPos.Y), Extensions.MeasureText("Hello World", "Arial", 12), filled:true);
              overlay.Drawings.AddText(Color.White, new System.Numerics.Vector2(cursorPos.X, cursorPos.Y), "Hello World");
          }
      }
```
