using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTKStuff
{
    public class Window : GameWindow
    {
        private int _vao;
        private int _vbo;
        private int _shaderProgram;
        private List<Vector2> _pointPositions;

        public Window(int width, int height, string title) 
            : base(GameWindowSettings.Default, 
                  new NativeWindowSettings { Size = new Vector2i(width, height), Title = title })
        {
        }

        protected override void OnLoad()
        {
            // Start with black background color
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            SwapBuffers();

            _pointPositions = new List<Vector2>();

            // Load the shaders from .glsl files and compile into a program
            _shaderProgram = new Shader("./shaders/vertex.glsl", "./shaders/fragment.glsl").GetProgramId();

            _vao = GLHelper.CreateVertexArray();
            _vbo = GLHelper.CreateVertexBuffer();

            GLHelper.SetVertexAttribute(_shaderProgram, "aPosition", 2, VertexAttribPointerType.Float, false, 0, 0);

            GL.PointSize(10.0f);

            // Register mouse button event
            MouseUp += OnWindowMouseUp;
        }

        private void OnWindowMouseUp(MouseButtonEventArgs args)
        {
            if (args.Button == MouseButton.Left)
            {
                // Convert mouse position to normalized device coordinates (NDC)
                // NDC is a coordinate system that ranges from -1 to 1 (-1, -1 bottom left corner, 1, 1 top right corner)
                float normalizedX = 2.0f * MouseState.X / Size.X - 1.0f;
                float normalizedY = 2.0f * (Size.Y - MouseState.Y) / Size.Y - 1.0f;

                // Add the clicked position to the list
                _pointPositions.Add(new Vector2(normalizedX, normalizedY));

                // Bind VAO and VBO
                GL.BindVertexArray(_vao);
                GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
                GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * 2 * _pointPositions.Count, _pointPositions.ToArray(), BufferUsageHint.StaticDraw);

                // Use the shader program
                GL.UseProgram(_shaderProgram);

                // Draw all the points based on vertex data in the VBO
                GL.DrawArrays(PrimitiveType.Points, 0, _pointPositions.Count);

                // Unbind VAO and VBO
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                GL.BindVertexArray(0);

                // Present rendered frame to screen
                SwapBuffers();

                Console.WriteLine($"Point Count: {_pointPositions.Count}");
            }
        }

        protected override void OnUnload()
        {
            // Delete VAO and VBO
            GL.DeleteBuffers(1, ref _vbo);
            GL.DeleteVertexArrays(1, ref _vao);

            // Delete shader program
            GL.DeleteProgram(_shaderProgram);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            // Defines the dimensions and location of the viewport
            // Its the region of the rendering window where an image is displayed
            GL.Viewport(0, 0, Size.X, Size.Y);
        }
    }
}
