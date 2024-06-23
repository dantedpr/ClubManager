Imports System.Data.SqlClient

Public Class FileManager

    Public Sub SaveFileToDatabase(fileName As String, fileData As Byte(), id As Integer)
        Try

            Dim dr As New DatabaseManager()

            Using connection As New SqlConnection(dr.connectionString)
                connection.Open()
                Dim query As String = "INSERT INTO files ([TYPE],[ITEM_ID],[FILENAME],[FILEDATA]) VALUES (@p1, @p2, @p3, @p4)"
                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@p1", "PLAYER")
                    command.Parameters.AddWithValue("@p2", id)
                    command.Parameters.AddWithValue("@p3", fileName)
                    command.Parameters.AddWithValue("@p4", fileData)
                    command.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine("Error al insertar el archivo: " & ex.Message)
        End Try
    End Sub

    Public Sub UpdateFileToDatabase(fileName As String, fileData As Byte(), id As Integer)

        Try
            Dim query As String = "UPDATE files SET [FILENAME] = @p1, [FILEDATA] = @p2 WHERE ID = " & id & ""

            Dim db As New DatabaseManager

            Using connection As New SqlConnection(db.connectionString)
                connection.Open()

                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@p1", fileName)
                    command.Parameters.AddWithValue("@p2", fileData)
                    command.ExecuteScalar()
                End Using
            End Using

        Catch ex As Exception
            Console.WriteLine("Error al actualizar el material: " & ex.Message)
        End Try

    End Sub

    Public Sub LoadFileFromDatabaseAndOpen(fileId As Integer)
        Try

            Dim dr As New DatabaseManager()

            Using connection As New SqlConnection(dr.connectionString)
                connection.Open()
                Dim query As String = "SELECT FileName, FileData FROM files WHERE Id = @fileId"
                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@fileId", fileId)
                    Using reader As SqlDataReader = command.ExecuteReader()
                        If reader.Read() Then
                            Dim fileName As String = reader("FileName").ToString()
                            Dim fileData As Byte() = CType(reader("FileData"), Byte())

                            ' Guardar el archivo temporalmente
                            Dim tempFilePath As String = IO.Path.Combine(IO.Path.GetTempPath(), fileName)
                            IO.File.WriteAllBytes(tempFilePath, fileData)

                            ' Abrir el archivo
                            Process.Start(tempFilePath)
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine("Error al cargar el archivo: " & ex.Message)
        End Try
    End Sub

End Class
