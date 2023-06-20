using OpenTK.Graphics.OpenGL4;

namespace OpenTKStuff
{
    public static class GLHelper
    {
        /// <summary>
        /// Create a VOA (Vertex Array Object)
        /// </summary>
        /// <returns></returns>
        public static int CreateVertexArray()
        {
            // Generates an unique identifier for the VAO
            GL.GenVertexArrays(1, out int vao);

            GL.BindVertexArray(vao);
            return vao;
        }

        /// <summary>
        /// Create a VBO (Vertex Buffer Object)
        /// </summary>
        /// <returns></returns>
        public static int CreateVertexBuffer()
        {
            // Generates an unique identifier for the VBO
            GL.GenBuffers(1, out int vbo);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

            // Allocates memory for the buffer. No initial data is given
            GL.BufferData(BufferTarget.ArrayBuffer, IntPtr.Zero, IntPtr.Zero, BufferUsageHint.StaticDraw);
            
            return vbo;
        }

        public static void SetVertexAttribute(int shaderProgram, string attributeName, int size, VertexAttribPointerType type, bool normalized, int stride, int offset)
        {
            // Retrieve attribute location within the shader program
            int attributeLocation = GL.GetAttribLocation(shaderProgram, attributeName);
            
            // Specify the layout and properties of the vertex attribute
            GL.VertexAttribPointer(attributeLocation, size, type, normalized, stride, offset);
            
            // Enable the attribute
            GL.EnableVertexAttribArray(attributeLocation);
        }
    }
}
