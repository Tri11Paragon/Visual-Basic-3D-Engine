
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
Imports System.Drawing.Imaging

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

        Maths.setWidth(Me.Width)
        Maths.setHeight(Me.Height)
    End Sub

    Protected Overrides Sub OnClosed(e As EventArgs)
        MyBase.OnClosed(e)

    End Sub

    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        MyBase.OnResize(e)

        Maths.setWidth(Me.Width)
        Maths.setHeight(Me.Height)

        GL.Viewport(0, 0, Me.Width, Me.Height)

        Dim aspect As Single = CSng(Me.Width) / Me.Height
        Dim projMat As Matrix4d = Matrix4d.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspect, 0.1, 100.0)

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

    Public Sub loadTransformationMatrix(matrix As Matrix4d)
        MyBase.loadMatrix(location_transformationMatrix, matrix)
    End Sub

    Public Sub loadProjectionMatrix(matrix As Matrix4d)
        MyBase.loadMatrix(location_projectionMatrix, matrix)
    End Sub

    Public Sub loadViewMatrix(matrix As Matrix4d)
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

    Protected Sub loadVector(location As Integer, value As Vector3d)
        GL.Uniform3(location, value.X, value.Y, value.Z)
    End Sub

    Protected Sub loadBoolean(location As Integer, value As Boolean)
        Dim id As Integer = 0
        If value Then
            id = 1
        End If

        GL.Uniform1(location, id)
    End Sub

    Protected Sub loadMatrix(location As Integer, maxtrix As Matrix4d)
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


Public Class EntityRenderer
    Dim shader As New StaticShader

    Public Sub New(shader As StaticShader, projectionMatrix As Matrix4d)
        Me.shader = shader
        shader.start()
        shader.loadProjectionMatrix(projectionMatrix)
        shader.stop_()
    End Sub

    Public Sub render(entitys As List(Of Entity))
        For Each entity As Entity In entitys
            prepareTexturedModel(entity.getModel())
            prepareInstance(entity)
            GL.DrawElements(BeginMode.Triangles, entity.getModel.getRawModel.getVertexCount(), DrawElementsType.UnsignedInt, 0)
            unbindTexturedModel()
        Next

    End Sub

    Private Sub prepareTexturedModel(ByVal model As TexturedModel)
        Dim rawModel As RawModel = model.getRawModel()
        GL.BindVertexArray(rawModel.getVaoID())
        GL.EnableVertexAttribArray(0)
        GL.EnableVertexAttribArray(1)
        GL.EnableVertexAttribArray(2)
        Dim texture As ModelTexture = model.getTexture()
        'shader.loadShineVariables(texture.getShineDamper(), texture.getReflectivity())
        GL.ActiveTexture(TextureUnit.Texture0)
        GL.BindTexture(TextureTarget.Texture2D, model.getTexture.getTextureID())
    End Sub

    Private Sub unbindTexturedModel()
        GL.DisableVertexAttribArray(0)
        GL.DisableVertexAttribArray(1)
        GL.DisableVertexAttribArray(2)
        GL.BindVertexArray(0)
    End Sub

    Private Sub prepareInstance(ByVal entity As Entity)
        Dim transformationMatrix As Matrix4d = Maths.createTransformationMatrix(entity.getPosition(), entity.getRotX(), entity.getRotY(), entity.getRotZ(), entity.getScale())
        shader.loadTransformationMatrix(transformationMatrix)
    End Sub

End Class

Public Class MasterRenderer

    Private Shared FOV As Single = 70
    Private Shared NEAR_PLANE As Single = 0.1F
    Private Shared FAR_PLANE As Single = 875

    Private projectionMatrix As Matrix4d
    Private shader As StaticShader = New StaticShader()
    Private renderer As EntityRenderer
    Private entities As List(Of Entity)

    Public Sub New(ByVal loader As Loader)
        GL.Enable(EnableCap.CullFace)
        GL.CullFace(CullFaceMode.Back)
        createProjectionMatrix()
        renderer = New EntityRenderer(shader, projectionMatrix)
    End Sub

    Public Sub render(ByVal player As Player)
        prepare()
        shader.start()
        shader.loadViewMatrix(Maths.createViewMatrix(player.getPosition.X, player.getPosition.Y, player.getPosition.Z))
        renderer.render(entities)
        shader.stop_()
        entities.clear()
    End Sub

    Public Sub processEntity(ByVal entity As Entity)
        entities.Add(entity)
    End Sub

    Public Sub cleanUp()
        shader.cleanup()
    End Sub

    Public Sub prepare()
        GL.Enable(EnableCap.DepthTest)
        GL.Clear(ClearBuffer.Color Or ClearBuffer.Depth)
        GL.ClearColor(0, 0, 0, 1)
    End Sub

    Private Sub createProjectionMatrix()
        Dim aspectRatio As Single = Maths.getWidth() / Maths.getHeight()
        Dim y_scale As Single = CSng(((1.0F / Math.Tan(Maths.DegreesToRadians(FOV / 2.0F))) * aspectRatio))
        Dim x_scale As Single = y_scale / aspectRatio
        Dim frustum_length As Single = FAR_PLANE - NEAR_PLANE
        projectionMatrix = New Matrix4d()
        projectionMatrix.Row0.X = x_scale
        projectionMatrix.M11 = y_scale
        projectionMatrix.M22 = -((FAR_PLANE + NEAR_PLANE) / frustum_length)
        projectionMatrix.M23 = -1
        projectionMatrix.M32 = -((2 * NEAR_PLANE * FAR_PLANE) / frustum_length)
        projectionMatrix.M33 = 0
    End Sub
End Class


Public Class Loader

    Dim vaos As ArrayList = New ArrayList()
    Dim vbos As ArrayList = New ArrayList()
    Dim textures(100) As Integer

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

    Private Function storeDataInFloatBuffer(data As Single()) As BufferArray(Of Single)
        Return New BufferArray(Of Single)(data)
    End Function

    Private Function storeDataInIntBuffer(data As Integer()) As BufferArray(Of Integer)
        Return New BufferArray(Of Integer)(data)
    End Function

    Private Sub bindIndicesBuffer(data As Integer())
        Dim vaoID As Int32 = GL.GenBuffer()
        vaos.Add(vaoID)
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, vaoID)
        GL.BufferData(BufferTarget.ElementArrayBuffer, data.Length, data, BufferUsageHint.StaticDraw)
    End Sub

    Private Sub unbindVAO()
        GL.BindVertexArray(0)
    End Sub

    Private Sub storeDataInAttributeList(attributeNumber As Int32, data As Single())
        Dim vboID As Integer = GL.GenBuffer()
        vbos.Add(vboID)
        GL.BindBuffer(BufferTarget.ArrayBuffer, vboID)
        GL.BufferData(BufferTarget.ArrayBuffer, data.Length, data, BufferUsageHint.StaticDraw)
        GL.VertexAttribPointer(attributeNumber, 3, VertexAttribPointerType.Float, False, 0, 0)
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0)
    End Sub

    Private Sub storeDataInAttributeList(attributeNumber As Int32, coordinateSize As Integer, data As Single())
        Dim vboID As Integer = GL.GenBuffer()
        vbos.Add(vboID)
        GL.BindBuffer(BufferTarget.ArrayBuffer, vboID)
        GL.BufferData(BufferTarget.ArrayBuffer, data.Length, data, BufferUsageHint.StaticDraw)
        GL.VertexAttribPointer(attributeNumber, coordinateSize, VertexAttribPointerType.Float, False, 0, 0)
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0)
    End Sub

    Private Function createVAO() As Integer
        Dim vaoID = GL.GenVertexArray()
        vaos.Add(vaoID)
        GL.BindVertexArray(vaoID)
        Return vaoID
    End Function

    Public Sub cleanUp()
        For num As Integer = 0 To vaos.Count
            GL.DeleteVertexArray(num)
        Next
        For num As Integer = 0 To vbos.Count
            GL.DeleteBuffer(num)
        Next
        For num As Integer = 0 To textures.Count
            GL.DeleteTexture(num)
        Next
        vaos = Nothing
        vbos = Nothing
        textures = Nothing
    End Sub

    Protected Sub LoadTexture(ByVal textureId As Integer, ByVal filename As String)
        Dim bmp As New Bitmap(filename)

        Dim data As BitmapData = bmp.LockBits(New Rectangle(0, 0, bmp.Width, bmp.Height),
                                                System.Drawing.Imaging.ImageLockMode.ReadOnly,
                                                System.Drawing.Imaging.PixelFormat.Format32bppArgb)

        GL.BindTexture(TextureTarget.Texture2D, textureId)
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                      bmp.Width, bmp.Height, 0, OpenGL.PixelFormat.Bgra,
                      PixelType.UnsignedByte, data.Scan0)

        bmp.UnlockBits(data)
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, TextureMinFilter.Linear)
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, TextureMagFilter.Linear)
    End Sub

    Public Function loadToVAO(positions As Single()) As RawModel
        Dim vaoID As Integer = createVAO()
        Me.storeDataInAttributeList(0, positions)
        unbindVAO()
        Return New RawModel(vaoID, positions.Length / 3)
    End Function

    Public Function loadToVAO(positions As Single(), modlename As String) As RawModel
        Dim vaoID As Integer = createVAO()
        Me.storeDataInAttributeList(0, 2, positions)
        unbindVAO()
        Return New RawModel(vaoID, positions.Length / 2, modlename)
    End Function

    Public Function loadToVAO(positions As Single(), textureCoords As Single(), normals As Single(), indices As Integer()) As RawModel
        Dim vaoID As Integer = createVAO()
        bindIndicesBuffer(indices)
        Me.storeDataInAttributeList(0, 3, positions)
        Me.storeDataInAttributeList(1, 2, textureCoords)
        Me.storeDataInAttributeList(2, 3, normals)
        unbindVAO()
        Return New RawModel(vaoID, indices.Length)
    End Function

    Public Function loadToVAO(positions As Single(), textureCoords As Single(), normals As Single(), indices As Integer(), modelName As String) As RawModel
        Dim vaoID As Integer = createVAO()
        bindIndicesBuffer(indices)
        Me.storeDataInAttributeList(0, 3, positions)
        Me.storeDataInAttributeList(1, 2, textureCoords)
        Me.storeDataInAttributeList(2, 3, normals)
        unbindVAO()
        Return New RawModel(vaoID, indices.Length, modelName)
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
