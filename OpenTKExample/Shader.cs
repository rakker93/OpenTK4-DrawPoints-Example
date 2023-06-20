using OpenTK.Graphics.OpenGL4;

namespace OpenTKStuff
{
    public class Shader
    {
        private readonly int _shaderProgram;

        public Shader(string vertexShaderPath, string fragmentShaderPath)
        {
            // Load the shader source code from the provided paths
            string vertexShaderSource = LoadShaderSource(vertexShaderPath);
            string fragmentShaderSource = LoadShaderSource(fragmentShaderPath);

            // Compile and link the shaders
            _shaderProgram = CreateShaderProgram(vertexShaderSource, fragmentShaderSource);
        }

        public int GetProgramId()
        {
            return _shaderProgram;
        }

        private static string LoadShaderSource(string filePath)
        {
            using StreamReader reader = new StreamReader(filePath);
            return reader.ReadToEnd();
        }

        private int CreateShaderProgram(string vertexShaderSource, string fragmentShaderSource)
        {
            int vertexShaderId = CompileShader(ShaderType.VertexShader, vertexShaderSource);
            int fragmentShaderId = CompileShader(ShaderType.FragmentShader, fragmentShaderSource);

            int shaderProgramId = GL.CreateProgram();
            GL.AttachShader(shaderProgramId, vertexShaderId);
            GL.AttachShader(shaderProgramId, fragmentShaderId);
            GL.LinkProgram(shaderProgramId);

            // Check for linking errors
            string programLog = GL.GetProgramInfoLog(shaderProgramId);
            
            if (!string.IsNullOrEmpty(programLog))
            {
                Console.WriteLine($"Shader program linking failed: {programLog}");
                throw new Exception($"Shader program linking failed: {programLog}");
            }

            // Detach and delete the individual shaders
            GL.DetachShader(shaderProgramId, vertexShaderId);
            GL.DetachShader(shaderProgramId, fragmentShaderId);
            GL.DeleteShader(vertexShaderId);
            GL.DeleteShader(fragmentShaderId);

            return shaderProgramId;
        }

        private int CompileShader(ShaderType type, string source)
        {
            int shaderId = GL.CreateShader(type);
            GL.ShaderSource(shaderId, source);
            GL.CompileShader(shaderId);

            // Check for compilation errors
            GL.GetShader(shaderId, ShaderParameter.CompileStatus, out int compileStatus);
            
            if (compileStatus != 1)
            {
                string shaderLog = GL.GetShaderInfoLog(shaderId);
                Console.WriteLine($"Shader compilation failed: {shaderLog}");
                throw new Exception($"Shader compilation failed: {shaderLog}");
            }

            return shaderId;
        }

        public void Dispose()
        {
            GL.DeleteProgram(_shaderProgram);
        }
    }
}
