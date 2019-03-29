Imports Test3DConsole.module1

Public Class RawModel

    Dim vaoID As Integer
    Dim vertexCount As Integer
    Dim modelName As String

    Public Sub New(vaoID As Integer, vertexCount As Integer, modelName As String)
        Me.vaoID = vaoID
        Me.vertexCount = vertexCount
        Me.modelName = modelName
    End Sub

    Public Sub New(vaoId As Integer, vertexCount As Integer)
        Me.vaoID = vaoId
        Me.vertexCount = vertexCount
        Me.modelName = Nothing
    End Sub

    Public Function getVaoID() As Integer
        Return vaoID
    End Function

    Public Function getVertexCount() As Integer
        Return vertexCount
    End Function

    Public Function getName() As String
        Return modelName
    End Function

End Class

Public Class ModelTexture

    Dim textureID As Integer
    Dim textureName As String

    Public Sub New(texture As Integer, name As String)
        Me.textureID = texture
        Me.textureName = name
    End Sub

    Public Sub New(texture As Integer)
        Me.textureID = texture
        Me.textureName = Nothing
    End Sub

    Public Function getTextureID() As Integer
        Return textureID
    End Function

    Public Function getName() As String
        Return textureName
    End Function

End Class

Public Class TextureData

    Dim width As Integer
    Dim height As Integer
    Dim buffer As BufferArray(Of Byte)

    Public Sub New(buffer As BufferArray(Of Byte), width As Integer, height As Integer)
        Me.width = width
        Me.height = height
        Me.buffer = buffer
    End Sub

    Public Function getWidth() As Integer
        Return width
    End Function

    Public Function getHeight() As Integer
        Return height
    End Function

    Public Function getBuffer() As BufferArray(Of Byte)
        Return buffer
    End Function

End Class

Public Class TexturedModel

    Dim rawModel As RawModel
    Dim texture As ModelTexture

    Public Sub New(rawModel As RawModel, modelTexutre As ModelTexture)
        Me.rawModel = rawModel
        Me.texture = modelTexutre
    End Sub

    Public Function getRawModel() As RawModel
        Return rawModel
    End Function

    Public Function getTexture() As ModelTexture
        Return texture
    End Function

End Class

Public Class ModelData

    Dim vertices As Double()
    Dim textureCoords As Double()
    Dim normals As Double()
    Dim indices As Integer()
    Dim furthestPoint As Double
    Dim modelName As String

    Public Sub New(vertices As Double(), textureCoords As Double(), normals As Double(), indices As Integer(),
                   furthestPoint As Double, modelName As String)
        Me.vertices = vertices
        Me.textureCoords = textureCoords
        Me.normals = normals
        Me.indices = indices
        Me.furthestPoint = furthestPoint
        Me.modelName = modelName
    End Sub

    Public Function getVertices() As Double()
        Return vertices
    End Function

    Public Function getTextureCoords() As Double()
        Return textureCoords
    End Function

    Public Function getNormals() As Double()
        Return normals
    End Function

    Public Function getIndices() As Integer()
        Return indices
    End Function

End Class
