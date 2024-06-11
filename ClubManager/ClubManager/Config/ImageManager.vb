Imports System
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.IO
Imports System.Net.Mime.MediaTypeNames

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
    Public Shared Sub SaveImageToDatabase(type As String, id_type As Integer, image As BitmapImage, club_id As Integer)

        Dim dr As New DatabaseManager
        ' Convertir la imagen a un byte array
        Dim imageBytes As Byte() = ImageToByteArray(image)

        ' Convertir el byte array a una cadena base64
        Dim base64String As String = ByteArrayToBase64String(imageBytes)
        Try
            ' Guardar la cadena base64 en la base de datos
            Using connection As New SqlConnection(dr.connectionString)
                connection.Open()
                Dim query As String = "INSERT INTO Imagenes ([TYPE],[ID_TYPE],[IMAGEN],[CLUB_ID],[EXTENSION]) VALUES (@p1, @p2, @p3, @p4,@p5)"
                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@p1", type)
                    command.Parameters.AddWithValue("@p2", id_type)
                    command.Parameters.AddWithValue("@p3", base64String)
                    command.Parameters.AddWithValue("@p4", club_id)
                    command.Parameters.AddWithValue("@p5", ".png")
                    command.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine("Error al insertar el club: " & ex.Message)
        End Try

    End Sub

    Public Shared Sub UpdateImage(type As String, id_type As Integer, image As BitmapImage, club_id As Integer)

        Dim base64String As String = String.Empty
        Dim dr As New DatabaseManager
        ' Recuperar la cadena base64 de la base de datos
        Using connection As New SqlConnection(dr.connectionString)
            connection.Open()
            Dim query As String = "SELECT IMAGEN FROM Imagenes WHERE type = @p1 and id_type = @p2 and club_id = @p3"
            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@p1", type)
                command.Parameters.AddWithValue("@p2", id_type)
                command.Parameters.AddWithValue("@p3", club_id)
                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        base64String = reader("IMAGEN").ToString()
                    End If
                End Using
            End Using
        End Using

        If base64String <> "" Then

            ' Convertir la imagen a un byte array
            Dim imageBytes As Byte() = ImageToByteArray(image)

            ' Convertir el byte array a una cadena base64
            base64String = ByteArrayToBase64String(imageBytes)
            Try
                ' Guardar la cadena base64 en la base de datos
                Using connection As New SqlConnection(dr.connectionString)
                    connection.Open()
                    Dim query As String = "UPDATE IMAGENES SET IMAGEN = @p1 WHERE ID_TYPE = @p2 AND CLUB_ID = @p3 AND TYPE = @p4"
                    Using command As New SqlCommand(query, connection)
                        command.Parameters.AddWithValue("@p1", base64String)
                        command.Parameters.AddWithValue("@p2", id_type)
                        command.Parameters.AddWithValue("@p3", club_id)
                        command.Parameters.AddWithValue("@p4", type)
                        command.ExecuteNonQuery()
                    End Using
                End Using
            Catch ex As Exception
                Console.WriteLine("Error al insertar el club: " & ex.Message)
            End Try
        Else
            SaveImageToDatabase(type, id_type, image, club_id)
        End If
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
    Public Shared Function LoadImageFromDatabase(type As String, id As Integer, clubId As Integer) As BitmapImage
        Dim base64String As String = String.Empty
        Dim dr As New DatabaseManager
        ' Recuperar la cadena base64 de la base de datos
        Using connection As New SqlConnection(dr.connectionString)
            connection.Open()
            Dim query As String = "SELECT IMAGEN FROM Imagenes WHERE type = @p1 and id_type = @p2 and club_id = @p3"
            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@p1", type)
                command.Parameters.AddWithValue("@p2", id)
                command.Parameters.AddWithValue("@p3", clubId)
                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        base64String = reader("IMAGEN").ToString()
                    End If
                End Using
            End Using
        End Using

        If base64String = "" Then
            Using connection As New SqlConnection(dr.connectionString)
                connection.Open()
                Dim query As String = "SELECT IMAGEN FROM Imagenes WHERE type = 'CLUB_DEF'"
                Using command As New SqlCommand(query, connection)
                    Using reader As SqlDataReader = command.ExecuteReader()
                        If reader.Read() Then
                            base64String = reader("IMAGEN").ToString()
                        End If
                    End Using
                End Using
            End Using
        End If

        ' Convertir la cadena base64 a un byte array
        Dim imageBytes As Byte() = Base64StringToByteArray(base64String)

        ' Convertir el byte array a un BitmapImage
        Return ByteArrayToImage(imageBytes)
    End Function

End Class
