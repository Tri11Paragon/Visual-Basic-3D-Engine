
Imports OpenTK
Imports OpenTK.Graphics
Imports OpenTK.Graphics.OpenGL
Imports System
Imports System.Text
Imports System.Runtime.InteropServices
Imports System.IO
Imports Test3DConsole.models
Imports System.Media
Imports System.Windows
Imports System.Drawing

' https://github.com/lefam/opengl_dot_net_experiments/tree/master/GLTexturedCube

Module module1

    Public Sub Main()
        Dim app As New GlShadedCube
        app.Run(60, 60)
    End Sub

End Module

Public Class GlShadedCube
    Inherits GameWindow

    Protected angle As Single

    Public Sub New()
        MyBase.New(320, 240, GraphicsMode.Default, "First touch with OpenTK")
    End Sub

    Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
        MyBase.OnLoad(e)
        GL.ClearColor(0.274, 0.874, 0.945, 0)
        GL.Enable(EnableCap.DepthTest)
    End Sub

    Protected Overrides Sub OnClosed(e As EventArgs)
        MyBase.OnClosed(e)

    End Sub

    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        MyBase.OnResize(e)

        GL.Viewport(0, 0, Me.Width, Me.Height)

        Dim aspect As Single = CSng(Me.Width) / Me.Height
        Dim projMat As Matrix4 = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspect, 0.1, 100.0)

        GL.MatrixMode(MatrixMode.Projection)
        GL.LoadMatrix(projMat)

        GL.MatrixMode(MatrixMode.Modelview)
        GL.LoadIdentity()
    End Sub

    Protected Overrides Sub OnRenderFrame(ByVal e As OpenTK.FrameEventArgs)
        MyBase.OnRenderFrame(e)
        GL.Clear(ClearBufferMask.ColorBufferBit Or ClearBufferMask.DepthBufferBit)

        GL.LoadIdentity()
        GL.Translate(0, 0, -5)
        GL.Rotate(angle, 0, 1, 1)
        'GL.Rotate(angle, 0, 1, 0)
        GL.Rotate(angle, 1, 0, 0)
        GL.Rotate(angle, 0, 1, 0)
        GL.Rotate(angle, 0, 0, 1)

        angle += 1

        GL.Begin(BeginMode.Quads)

        ' Front face 
        GL.Color3(1.0, 0.0, 0.0)
        GL.Vertex3(-1, 1, 1)
        GL.Color3(0.0, 1.0, 0.0)
        GL.Vertex3(1, 1, 1)
        GL.Color3(0.0, 0.0, 0.7)
        GL.Vertex3(1, -1, 1)
        GL.Color3(0.0, 1.0, 0.0)
        GL.Vertex3(-1, -1, 1)

        ' Back face
        GL.Color3(1.0, 0.0, 0.0)
        GL.Vertex3(1, 1, -1)
        GL.Color3(0.0, 1.0, 0.0)
        GL.Vertex3(-1, 1, -1)
        GL.Color3(0.0, 0.0, 0.7)
        GL.Vertex3(-1, -1, -1)
        GL.Color3(0.0, 1.0, 0.0)
        GL.Vertex3(1, -1, -1)

        ' Right face
        GL.Color3(1.0, 0.0, 0.0)
        GL.Vertex3(1, 1, -1)
        GL.Color3(0.0, 1.0, 0.0)
        GL.Vertex3(1, 1, 1)
        GL.Color3(0.0, 0.0, 0.7)
        GL.Vertex3(1, -1, 1)
        GL.Color3(0.0, 1.0, 0.0)
        GL.Vertex3(1, -1, -1)

        ' Left face
        GL.Color3(1.0, 0.0, 0.0)
        GL.Vertex3(-1, 1, -1)
        GL.Color3(0.0, 1.0, 0.0)
        GL.Vertex3(-1, 1, 1)
        GL.Color3(0.0, 0.0, 0.7)
        GL.Vertex3(-1, -1, 1)
        GL.Color3(0.0, 1.0, 0.0)
        GL.Vertex3(-1, -1, -1)

        ' Top face
        GL.Color3(1.0, 0.0, 0.0)
        GL.Vertex3(-1, 1, 1)
        GL.Color3(0.0, 1.0, 0.0)
        GL.Vertex3(-1, 1, -1)
        GL.Color3(0.0, 0.0, 0.7)
        GL.Vertex3(1, 1, -1)
        GL.Color3(0.0, 1.0, 0.0)
        GL.Vertex3(1, 1, 1)

        ' Bottom face
        GL.Color3(1.0, 0.0, 0.0)
        GL.Vertex3(-1, -1, 1)
        GL.Color3(0.0, 1.0, 0.0)
        GL.Vertex3(1, -1, 1)
        GL.Color3(0.0, 0.0, 0.7)
        GL.Vertex3(1, -1, -1)
        GL.Color3(0.0, 1.0, 0.0)
        GL.Vertex3(-1, -1, -1)

        GL.End()

        SwapBuffers()
    End Sub

End Class

Public Class StaticShader
    Inherits shaderProgram

    Dim location_transformationMatrix As Integer
    Dim location_projectionMatrix As Integer
    Dim location_viewMatrix As Integer

    Private Const VERTEX_FILE As String = "shader/vertexShader.txt"
    Private Const FRAGMENT_FILE As String = "shader/fragmentShader.txt"

    Public Sub New()
        MyBase.New(VERTEX_FILE, FRAGMENT_FILE)
    End Sub

    Protected Overrides Sub bindAttributes()
        MyBase.bindAttribute(0, "position")
        MyBase.bindAttribute(1, "textureCoordinates")
        MyBase.bindAttribute(2, "normal")
    End Sub

    Protected Overrides Sub getAllUniformLocations()
        location_transformationMatrix = MyBase.getUniformLocation("transformationMatrix")
        location_projectionMatrix = MyBase.getUniformLocation("projectionMatrix")
        location_viewMatrix = MyBase.getUniformLocation("viewMatrix")
    End Sub

    Public Sub loadTransformationMatrix(matrix As Matrix4)
        MyBase.loadMatrix(location_transformationMatrix, matrix)
    End Sub

    Public Sub loadProjectionMatrix(matrix As Matrix4)
        MyBase.loadMatrix(location_projectionMatrix, matrix)
    End Sub

    Public Sub loadViewMatrix(matrix As Matrix4)
        MyBase.loadMatrix(location_viewMatrix, matrix)
    End Sub

End Class

Public Class shaderProgram

    Dim programID As Integer
    Dim vertexShaderID As Integer
    Dim fragmentShaderID As Integer

    Private maxtrixBuffer As BufferArray(Of System.Single) = New BufferArray(Of Single)(16)

    Public Sub New(vertexFile As String, fragmentFile As String)
        vertexShaderID = loadShader(vertexFile, ShaderType.VertexShader)
        fragmentShaderID = loadShader(fragmentFile, ShaderType.FragmentShader)
        programID = GL.CreateProgram()
        GL.AttachShader(programID, vertexShaderID)
        GL.AttachShader(programID, fragmentShaderID)
        bindAttributes()
        GL.LinkProgram(programID)
        GL.ValidateProgram(programID)
        getAllUniformLocations()
    End Sub

    Protected Overridable Sub bindAttributes()

    End Sub

    Protected Overridable Sub getAllUniformLocations()

    End Sub

    Public Sub cleanup()
        stop_()
        GL.DetachShader(programID, fragmentShaderID)
        GL.DetachShader(programID, vertexShaderID)
        GL.DeleteShader(fragmentShaderID)
        GL.DeleteShader(vertexShaderID)
        GL.DeleteProgram(programID)
    End Sub

    Public Function getUniformLocation(uniform As String) As Integer
        Return GL.GetUniformLocation(programID, uniform)
    End Function

    Public Sub start()
        GL.UseProgram(programID)
    End Sub

    Public Sub stop_()
        GL.UseProgram(0)
    End Sub

    Protected Sub bindAttribute(attribute As Integer, variableName As String)
        GL.BindAttribLocation(programID, attribute, variableName)
    End Sub

    Protected Sub loadFlaot(location As Integer, value As Double)
        GL.Uniform1(location, value)
    End Sub

    Protected Sub loadInt(location As Integer, value As Integer)
        GL.Uniform1(location, value)
    End Sub

    Protected Sub loadVector(location As Integer, value As Vector3)
        GL.Uniform3(location, value.X, value.Y, value.Z)
    End Sub

    Protected Sub loadBoolean(location As Integer, value As Boolean)
        Dim id As Integer = 0
        If value Then
            id = 1
        End If

        GL.Uniform1(location, id)
    End Sub

    Protected Sub loadMatrix(location As Integer, maxtrix As Matrix4)
        GL.UniformMatrix4(location, False, maxtrix)
    End Sub

    Private Function loadShader(path As String, type As ShaderType) As Integer
        Dim shaderSource As StringBuilder = New StringBuilder()

        Try
            Dim reader As BufferedStream = New BufferedStream(New FileStream(path, FileMode.Open))
            Dim SR As StreamReader = File.OpenText(path)
            Dim line As String = ""

            line = SR.ReadLine()
            shaderSource.Append(line).Append("//\n")

            Do While line IsNot Nothing
                line = SR.ReadLine()
                shaderSource.Append(line).Append("//\n")
                Console.WriteLine("Loading Shaders :: " + line)
            Loop

            reader.Close()

        Catch ex As IO.IOException

        End Try

        Dim shaderID = GL.CreateShader(type)
        GL.ShaderSource(shaderID, shaderSource.ToString)
        GL.CompileShader(shaderID)

        Dim params As Integer
        GL.GetShader(shaderID, ShaderParameter.CompileStatus, params)
        Console.WriteLine("Shader Compile Code: " + params)

        Return shaderID

    End Function

End Class


Public Class Renderer

    Public Sub render(model As RawModel)
        GL.BindVertexArray(model.getVaoID)
        GL.EnableVertexAttribArray(0)
        GL.DrawArrays(PrimitiveType.Triangles, 0, model.getVertexCount())
        GL.DisableVertexAttribArray(0)
        GL.BindVertexArray(0)
    End Sub

End Class

Public Class Loader


    Private Function decodeTextureFile(path As String) As TextureData
        Dim width As Integer = 0
        Dim height As Integer = 0
        Dim buffer As BufferArray(Of Byte)
        Try
            Dim img As Image = Image.FromFile(path)
            width = img.Width
            height = img.Height
            buffer = New BufferArray(Of Byte)(imageUtils.imgToByteArray(img))

        Catch ex As Exception

        End Try

        Return New TextureData(buffer, width, height)

    End Function

End Class

'source http://net-informations.com/q/faq/imgtobyte.html
Public Class imageUtils

    Public Shared Function imgToByteArray(ByVal img As Image) As Byte()
        Using mStream As New MemoryStream()
            img.Save(mStream, img.RawFormat)
            Return mStream.ToArray()
        End Using
    End Function
    'convert bytearray to image
    Public Shared Function byteArrayToImage(ByVal byteArrayIn As Byte()) As Image
        Using mStream As New MemoryStream(byteArrayIn)
            Return Image.FromStream(mStream)
        End Using
    End Function
    'another easy way to convert image to bytearray
    Public Shared Function imgToByteConverter(ByVal inImg As Image) As Byte()
        Dim imgCon As New ImageConverter()
        Return DirectCast(imgCon.ConvertTo(inImg, GetType(Byte())), Byte())
    End Function

End Class

' credit to https://stackoverflow.com/questions/29402216/c-sharp-equivalent-to-javas-floatbuffer-shortbuffer
' for the buffer
Public Class BufferArray(Of T)
    Protected _array As T()

    Public Sub New(ByVal array As T())
        _array = New T(array.Length - 1) {}
        array.Copy(array, 0, _array, 0, _array.Length)
    End Sub

    Public Sub New(ByVal capacity As Integer)
        _array = New T(capacity - 1) {}

        For i As Integer = 0 To _array.Length - 1
            _array(i) = Nothing
        Next
    End Sub

    Public ReadOnly Property First As T
        Get
            Return _array(0)
        End Get
    End Property

    Public ReadOnly Property Last As T
        Get
            Return _array(_array.Length - 1)
        End Get
    End Property

    Public ReadOnly Property Length As Integer
        Get
            Return _array.Length
        End Get
    End Property

    Public ReadOnly Property Array As T()
        Get
            Return _array
        End Get
    End Property

    Public Function [Get](ByVal index As Integer, ByVal length As Integer) As T()
        Dim array = New T(length - 1) {}
        array.Copy(_array, index, array, 0, length)
        Return array
    End Function

    Public Function [Get]() As T()
        Dim array = New T(_array.Length - 1) {}
        array.Copy(_array, 0, array, 0, array.Length)
        Return array
    End Function

    Public Function GetValue(ByVal index As Integer) As T
        Return _array(index)
    End Function

    Public Sub Put(ByVal value As T)
        Dim array = New T(_array.Length + 1 - 1) {}
        array.Copy(_array, 0, array, 1, _array.Length)
        array(0) = value
        _array = array
    End Sub

    Public Sub Put(ByVal value As T, ByVal index As Integer)
        If index = 0 Then
            Put(value)
        ElseIf index = _array.Length - 1 Then
            Append(value)
        Else
            Dim array = New T(_array.Length + 1 - 1) {}
            array(index) = value
            array.Copy(_array, 0, array, 0, index)
            array.Copy(_array, index, array, index + 1, _array.Length - index)
            _array = array
        End If
    End Sub

    Public Sub Insert(ByVal value As T, ByVal index As Integer)
        _array(index) = value
    End Sub

    Public Sub SetA(ByVal value As T())
        _array = value
    End Sub

    Public Sub Append(ByVal value As T)
        Dim array = New T(_array.Length + 1 - 1) {}
        array.Copy(_array, 0, array, 0, _array.Length)
        array(array.Length - 1) = value
        _array = array
    End Sub

    Public Sub Put(ByVal value As T())
        Dim array = New T(_array.Length + value.Length - 1) {}
        array.Copy(value, 0, array, 0, value.Length)
        array.Copy(_array, 0, array, value.Length, _array.Length)
        _array = array
    End Sub

    Public Sub Put(ByVal value As T(), ByVal index As Integer)
        If index = 0 Then
            Put(value)
        ElseIf index = _array.Length - 1 Then
            Append(value)
        Else
            Dim array = New T(_array.Length + value.Length - 1) {}
            array.Copy(value, 0, array, index, value.Length)
            array.Copy(_array, 0, array, 0, index)
            array.Copy(_array, index, array, index + value.Length, _array.Length - index)
            _array = array
        End If
    End Sub

    Public Sub Append(ByVal value As T())
        Dim array = New T(_array.Length + value.Length - 1) {}
        array.Copy(_array, 0, array, 0, _array.Length)
        array.Copy(value, 0, array, _array.Length, value.Length)
        _array = array
    End Sub

    Public Sub Put(ByVal value As BufferArray(Of T))
        Put(value._array)
    End Sub

    Public Sub Put(ByVal value As BufferArray(Of T), ByVal index As Integer)
        Put(value._array, index)
    End Sub

    Public Sub Append(ByVal value As BufferArray(Of T))
        Append(value._array)
    End Sub

    Public Function GetBytes() As Byte()
        Dim buff = New Byte(_array.Length * Marshal.SizeOf(GetType(T)) - 1) {}
        System.Buffer.BlockCopy(_array, 0, buff, 0, buff.Length)
        Return buff
    End Function
End Class
