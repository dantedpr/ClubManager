Imports System.Data
Imports System.Data.SqlClient

Public Class DatabaseManager

    Public connectionString As String

    Public Sub New()

        connectionString = "Data Source=DANTEDPR\SQLEXPRESS;Initial Catalog=ClubManager;Integrated Security=True"
    End Sub

    Public Sub ExecuteSelectQuery(query As String)
        Try

            Using connection As New SqlConnection(connectionString)
                ' Open connection
                connection.Open()

                Using command As New SqlCommand(query, connection)

                    Dim dr As SqlDataReader = command.ExecuteReader()
                    While dr.Read()

                        'Implement select

                    End While
                    dr.Close()
                End Using

                connection.Close()
            End Using
        Catch ex As Exception

            MessageBox.Show("Error executing query: " & ex.Message)
        End Try
    End Sub

    Public Function CheckUser(user As String, pass As String) As Boolean
        Try

            Dim query = "SELECT ID FROM ClubPass WHERE CLUB_CODE = '" & user & "' AND CLUB_PASS = '" & pass & "'"
            Dim exists = False
            Using connection As New SqlConnection(connectionString)
                ' Open connection
                connection.Open()

                Using command As New SqlCommand(query, connection)

                    Dim dr As SqlDataReader = command.ExecuteReader()
                    While dr.Read()

                        If dr("ID") > 0 Then
                            exists = True
                        End If

                    End While
                    dr.Close()
                End Using

                connection.Close()
            End Using

            Return exists
        Catch ex As Exception
            MessageBox.Show("Error executing query: " & ex.Message)
            Return False
        End Try
    End Function

    Public Function RecoverUser(user As String) As Integer
        Try

            Dim query = "SELECT ID FROM ClubPass WHERE CLUB_CODE = '" & user & "'"
            Dim id = -1
            Using connection As New SqlConnection(connectionString)
                ' Open connection
                connection.Open()

                Using command As New SqlCommand(query, connection)

                    Dim dr As SqlDataReader = command.ExecuteReader()
                    While dr.Read()

                        If dr("ID") > 0 Then
                            id = CInt(dr("ID"))
                        End If

                    End While
                    dr.Close()
                End Using

                connection.Close()
            End Using

            Return id
        Catch ex As Exception
            MessageBox.Show("Error executing query: " & ex.Message)
            Return False
        End Try
    End Function

    Public Sub SaveFileToDatabase(fileName As String, fileData As Byte())
        Try
            Using connection As New SqlConnection(connectionString)
                connection.Open()
                Dim query As String = "INSERT INTO files (FileName, FileData) VALUES (@fileName, @fileData)"
                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@fileName", fileName)
                    command.Parameters.AddWithValue("@fileData", fileData)
                    command.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine("Error al insertar el archivo: " & ex.Message)
        End Try
    End Sub

    Public Sub LoadFileFromDatabaseAndOpen(fileId As Integer)
        Try
            Using connection As New SqlConnection(connectionString)
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

