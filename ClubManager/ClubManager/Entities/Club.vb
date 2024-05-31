Imports System.Data.SqlClient

Public Class Club

    Public Property ID As Integer
    Public Property Code As String
    Public Property Name As String
    Public Property Address As String
    Public Property Mail As String
    Public Property Phone As String
    Private Property Password As String

    Public Sub New()

    End Sub

    Public Sub New(ByVal code As String, ByVal name As String, ByVal address As String, ByVal mail As String, ByVal phone As String, ByVal password As String)
        Me.Code = code
        Me.Name = name
        Me.Address = address
        Me.Mail = mail
        Me.Phone = phone
        Me.Password = password
    End Sub

    Public Shared Function CreateClub(ByVal code As String, ByVal name As String, ByVal address As String, ByVal mail As String, ByVal phone As String, ByVal password As String) As Club
        Return New Club(code, name, address, mail, phone, password)
    End Function

    Public Sub Map(ByVal reader As SqlDataReader)
        Me.ID = CInt(reader("ID"))
        Me.Code = reader("CODE").ToString()
        Me.Name = reader("NAME").ToString()
        Me.Address = reader("ADDRESS").ToString()
        Me.Mail = reader("MAIL").ToString()
        Me.Phone = reader("PHONE").ToString()
    End Sub

    Public Function SaveCheck(code As String, name As String, address As String, mail As String, phone As String) As Integer

        Dim query As String = "INSERT INTO Club ([CODE], [NAME], [ADDRESS], [MAIL], [PHONE]) VALUES (@Code, @Name, @Address, @Mail, @Phone); SELECT SCOPE_IDENTITY();"
        Dim clubId As Integer = -1

        Dim dr As New DatabaseManager

        Try
            Using connection As New SqlConnection(dr.connectionString)
                connection.Open()

                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@Code", code)
                    command.Parameters.AddWithValue("@Name", name)
                    command.Parameters.AddWithValue("@Address", address)
                    command.Parameters.AddWithValue("@Mail", mail)
                    command.Parameters.AddWithValue("@Phone", phone)

                    ' Execute the insert query and get the inserted ID
                    clubId = Convert.ToInt32(command.ExecuteScalar())
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine("Error al insertar el club: " & ex.Message)
        End Try

        Me.Code = code

        Return clubId

    End Function

    Public Sub SetPassword(pass As String)

        Try
            Dim query As String = "INSERT INTO ClubPass ([CLUB_CODE], [CLUB_PASS]) VALUES (@Code, @Pass); SELECT SCOPE_IDENTITY();"

            Dim db As New DatabaseManager

            Using connection As New SqlConnection(db.connectionString)
                connection.Open()

                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@Code", Me.Code)
                    command.Parameters.AddWithValue("@Pass", pass)
                    command.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine("Error al insertar el password: " & ex.Message)
        End Try

    End Sub

    Public Sub LoadClub(code As String)

        Try

            Dim db As New DatabaseManager

            Dim s = "SELECT * FROM Club WHERE CODE = '" & code & "'"

            Using connection As New SqlConnection(db.connectionString)
                ' Open connection
                connection.Open()

                Using command As New SqlCommand(s, connection)

                    Dim dr As SqlDataReader = command.ExecuteReader()

                    If dr.Read Then
                        Map(dr)
                    End If

                    dr.Close()
                End Using

                connection.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error executing query: " & ex.Message)
        End Try


    End Sub
End Class
