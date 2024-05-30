Imports System
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.IO

Public Class ImageManager
    ' Función para convertir una imagen a un byte array
    Public Shared Function ImageToByteArray(image As BitmapImage) As Byte()
        Dim encoder As New PngBitmapEncoder()
        encoder.Frames.Add(BitmapFrame.Create(image))
        Using ms As New MemoryStream()
            encoder.Save(ms)
            Return ms.ToArray()
        End Using
    End Function

    ' Función para convertir un byte array a una cadena base64
    Public Shared Function ByteArrayToBase64String(byteArray As Byte()) As String
        Return Convert.ToBase64String(byteArray)
    End Function

    ' Función para guardar una imagen en la base de datos
    Public Shared Sub SaveImageToDatabase(image As BitmapImage, connectionString As String)
        ' Convertir la imagen a un byte array
        Dim imageBytes As Byte() = ImageToByteArray(image)

        ' Convertir el byte array a una cadena base64
        Dim base64String As String = ByteArrayToBase64String(imageBytes)

        ' Guardar la cadena base64 en la base de datos
        Using connection As New SqlConnection(connectionString)
            connection.Open()
            Dim query As String = "INSERT INTO Images (ImageData) VALUES (@ImageData)"
            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@ImageData", base64String)
                command.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Shared Function Base64StringToByteArray(base64String As String) As Byte()
        Return Convert.FromBase64String(base64String)
    End Function

    ' Función para convertir un byte array a una imagen BitmapImage
    Public Shared Function ByteArrayToImage(byteArray As Byte()) As BitmapImage
        Using ms As New MemoryStream(byteArray)
            Dim image As New BitmapImage()
            image.BeginInit()
            image.StreamSource = ms
            image.CacheOption = BitmapCacheOption.OnLoad
            image.EndInit()
            image.Freeze() ' Para hacer que el BitmapImage sea seguro para el acceso multi-hilo
            Return image
        End Using
    End Function

    ' Función para recuperar la imagen de la base de datos y convertirla a BitmapImage
    Public Shared Function LoadImageFromDatabase(connectionString As String, imageId As Integer) As BitmapImage
        Dim base64String As String = String.Empty

        ' Recuperar la cadena base64 de la base de datos
        Using connection As New SqlConnection(connectionString)
            connection.Open()
            Dim query As String = "SELECT ImageData FROM Images WHERE ImageID = @ImageID"
            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@ImageID", imageId)
                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        base64String = reader("ImageData").ToString()
                    End If
                End Using
            End Using
        End Using

        ' Convertir la cadena base64 a un byte array
        Dim imageBytes As Byte() = Base64StringToByteArray(base64String)

        ' Convertir el byte array a un BitmapImage
        Return ByteArrayToImage(imageBytes)
    End Function

End Class
