Imports System.Data
Imports System.Data.SqlClient
Imports System.Windows.Forms

Public Class Club

    Public Shared Property ID As Integer
    Public Shared Property Code As String
    Public Shared Property Name As String
    Public Shared Property Address As String
    Public Shared Property Mail As String
    Public Shared Property Phone As String
    Public Shared Property logoClub As String
    Private Shared Property Password As String


    Public Sub New()

    End Sub

    Public Sub New(ByVal code1 As String, ByVal name1 As String, ByVal address1 As String, ByVal mail1 As String, ByVal phone1 As String, ByVal password1 As String)
        Code = code1
        Name = name1
        Address = address1
        Mail = mail1
        Phone = phone1
        Password = password1
    End Sub

    Public Shared Function CreateClub(ByVal code As String, ByVal name As String, ByVal address As String, ByVal mail As String, ByVal phone As String, ByVal password As String) As Club
        Return New Club(code, name, address, mail, phone, password)
    End Function

    Private Shared Sub Map(ByVal reader As SqlDataReader)
        ID = CInt(reader("ID"))
        Code = reader("CODE").ToString()
        Name = reader("NAME").ToString()
        Address = reader("ADDRESS").ToString()
        Mail = reader("MAIL").ToString()
        Phone = reader("PHONE").ToString()
    End Sub

    Public Shared Function SaveCheck(code1 As String, name As String, address As String, mail As String, phone As String) As Integer

        Dim query As String = "INSERT INTO Club ([CODE], [NAME], [ADDRESS], [MAIL], [PHONE]) VALUES (@Code, @Name, @Address, @Mail, @Phone); SELECT SCOPE_IDENTITY();"
        Dim clubId As Integer = -1

        Dim dr As New DatabaseManager

        Try
            Using connection As New SqlConnection(dr.connectionString)
                connection.Open()

                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@Code", code1)
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

        Code = code1

        Return clubId

    End Function

    Public Shared Sub SetPassword(pass As String)

        Try
            Dim query As String = "INSERT INTO ClubPass ([CLUB_CODE], [CLUB_PASS]) VALUES (@Code, @Pass); SELECT SCOPE_IDENTITY();"

            Dim db As New DatabaseManager

            Using connection As New SqlConnection(db.connectionString)
                connection.Open()

                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@Code", Code)
                    command.Parameters.AddWithValue("@Pass", pass)
                    command.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine("Error al insertar el password: " & ex.Message)
        End Try

    End Sub

    Public Shared Sub UpdatePass(pass As String)

        Try
            Dim query As String = "UPDATE ClubPass SET [CLUB_PASS] = @p1 WHERE CLUB_CODE = '" & Club.Code & "'"

            Dim db As New DatabaseManager

            Using connection As New SqlConnection(db.connectionString)
                connection.Open()

                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@p1", pass)
                    command.ExecuteScalar()
                End Using
            End Using

        Catch ex As Exception
            Console.WriteLine("Error al actualizar el club: " & ex.Message)
        End Try

    End Sub

    Public Shared Sub Delete()

        Try

            DeleteTeams()
            DeletePlayers()
            DeleteMaterial()
            DeleteClubPass()

            Dim query As String = "DELETE [CLUB] WHERE CODE = '" & Club.Code & "'"

            Dim db As New DatabaseManager

            Using connection As New SqlConnection(db.connectionString)
                connection.Open()

                Using command As New SqlCommand(query, connection)
                    command.ExecuteScalar()
                End Using
            End Using

        Catch ex As Exception
            Console.WriteLine("Error al eliminar el club: " & ex.Message)
        End Try

    End Sub

    Private Shared Sub DeleteTeams()
        Try

            Dim query As String = "DELETE [Teams] WHERE CLUB_ID = " & Club.ID & ""

            Dim db As New DatabaseManager

            Using connection As New SqlConnection(db.connectionString)
                connection.Open()

                Using command As New SqlCommand(query, connection)
                    command.ExecuteScalar()
                End Using
            End Using

        Catch ex As Exception
            Console.WriteLine("Error al eliminar el club: " & ex.Message)
        End Try
    End Sub

    Private Shared Sub DeletePlayers()
        Try

            Dim query As String = "DELETE [Players] WHERE CLUB_ID = " & Club.ID & ""

            Dim db As New DatabaseManager

            Using connection As New SqlConnection(db.connectionString)
                connection.Open()

                Using command As New SqlCommand(query, connection)
                    command.ExecuteScalar()
                End Using
            End Using

        Catch ex As Exception
            Console.WriteLine("Error al eliminar el club: " & ex.Message)
        End Try
    End Sub

    Private Shared Sub DeleteMaterial()
        Try

            Dim query As String = "DELETE [Material] WHERE CLUB_ID = " & Club.ID & ""

            Dim db As New DatabaseManager

            Using connection As New SqlConnection(db.connectionString)
                connection.Open()

                Using command As New SqlCommand(query, connection)
                    command.ExecuteScalar()
                End Using
            End Using

        Catch ex As Exception
            Console.WriteLine("Error al eliminar el club: " & ex.Message)
        End Try
    End Sub

    Private Shared Sub DeleteClubPass()
        Try

            Dim query As String = "DELETE [ClubPass] WHERE CLUB_CODE = '" & Club.Code & "'"

            Dim db As New DatabaseManager

            Using connection As New SqlConnection(db.connectionString)
                connection.Open()

                Using command As New SqlCommand(query, connection)
                    command.ExecuteScalar()
                End Using
            End Using

        Catch ex As Exception
            Console.WriteLine("Error al eliminar el club: " & ex.Message)
        End Try
    End Sub

    Public Shared Sub UpdateClub(name As String, address As String, mail As String, phone As String)

        Try
            Dim query As String = "UPDATE Club SET [NAME] = @p1, [ADDRESS] = @p2, [MAIL] = @p3, [PHONE] = @p4 WHERE ID = " & Club.ID & ""

            Dim db As New DatabaseManager

            Using connection As New SqlConnection(db.connectionString)
                connection.Open()

                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@p1", name)
                    command.Parameters.AddWithValue("@p2", address)
                    command.Parameters.AddWithValue("@p3", mail)
                    command.Parameters.AddWithValue("@p4", phone)
                    command.ExecuteScalar()
                End Using
            End Using

        Catch ex As Exception
            Console.WriteLine("Error al actualizar el club: " & ex.Message)
        End Try

    End Sub

    Public Shared Sub LoadClub(code As String)

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

    Public Shared Function GetAllClubs() As DataTable
        Try

            Dim query = "SELECT ID, NAME FROM Club"
            Dim exists = False
            Dim db As New DatabaseManager
            Dim dataTable As New DataTable()

            ' Agregar columnas al DataTable (por ejemplo)
            dataTable.Columns.Add("ID", GetType(String))
            dataTable.Columns.Add("NAME", GetType(String))
            Using connection As New SqlConnection(db.connectionString)
                ' Open connection
                connection.Open()

                Using command As New SqlCommand(query, connection)

                    Dim dr As SqlDataReader = command.ExecuteReader()
                    While dr.Read()

                        Dim name11 = dr("NAME").ToString()

                        dataTable.Rows.Add(CInt(dr("ID")), dr("NAME").ToString())
                    End While
                    dr.Close()
                End Using

                connection.Close()
            End Using

            Return dataTable
        Catch ex As Exception
            MessageBox.Show("Error executing query: " & ex.Message)
            Return Nothing
        End Try
    End Function

    Public Shared Function GetAllPlayers(age As String, name As String, team As String, category As String, Optional status As Boolean = False) As DataTable
        Try

            Dim query = "SELECT p.ID, p.[NAME] ,p.[LASTNAME1] + ' ' + p.[LASTNAME2] AS FULLNAME ,p.[MAIL] ,p.[PHONE] ,p.[ADDRESS] ,p.[AGE] ,p.[BIRTHDATE] ,p.[CLUB_ID] ,p.[TEAM_ID]
                        ,CASE  WHEN p.[STATUS] = 1 THEN 'ACTIVO' WHEN p.[STATUS] = 0 THEN 'INACTIVO' ELSE 'UNKNOWN' END AS STATUS
                        ,t.Name AS TEAMNAME, t.Category AS TEAMCATEGORY FROM Players p LEFT JOIN Teams t ON t.ID = p.TEAM_ID WHERE p.CLUB_ID = " & Club.ID

            If age <> "" AndAlso IsNumeric(age) Then
                query = query & " AND p.Age = " & CInt(age)
            End If

            If name <> "" Then
                query = query & " AND p.Name LIKE '" & name & "%'"
            End If

            If team <> "" Then
                query = query & " AND t.Name = '" & team & "'"
            End If

            If category <> "" Then
                query = query & " AND t.Category = '" & category & "'"
            End If

            If Not status Then
                query = query & " AND p.Status = " & 1
            End If
            query = query & " ORDER BY p.ID DESC"

            Dim db As New DatabaseManager
            Dim dataTable As New DataTable()

            Using connection As New SqlConnection(db.connectionString)
                ' Open connection
                connection.Open()

                Using command As New SqlCommand(query, connection)

                    Dim dr As SqlDataReader = command.ExecuteReader()
                    dataTable.Load(dr)
                    dr.Close()
                End Using

                connection.Close()
            End Using

            Return dataTable
        Catch ex As Exception
            MessageBox.Show("Error executing query: " & ex.Message)
            Return Nothing
        End Try
    End Function

    Public Shared Function GetAllTeams(name As String, cat As String) As DataTable
        Try

            Dim query = "SELECT * FROM Teams WHERE CLUB_ID = " & Club.ID

            If name <> "" Then
                query = query & " AND NAME LIKE '" & name & "%' "
            End If

            If cat <> "" Then
                query = query & " AND CATEGORY = '" & cat & "' "
            End If

            Dim exists = False
            Dim db As New DatabaseManager
            Dim dataTable As New DataTable()

            Using connection As New SqlConnection(db.connectionString)
                ' Open connection
                connection.Open()

                Using command As New SqlCommand(query, connection)

                    Dim dr As SqlDataReader = command.ExecuteReader()
                    dataTable.Load(dr)
                    dr.Close()
                End Using

                connection.Close()
            End Using

            Return dataTable
        Catch ex As Exception
            MessageBox.Show("Error executing query: " & ex.Message)
            Return Nothing
        End Try
    End Function

    Public Shared Function GetTeamsName() As List(Of String)
        Try

            Dim query = "SELECT * FROM Teams WHERE CLUB_ID = " & Club.ID


            Dim db As New DatabaseManager
            Dim li As New List(Of String)

            Using connection As New SqlConnection(db.connectionString)
                ' Open connection
                connection.Open()

                Using command As New SqlCommand(query, connection)

                    Dim dr As SqlDataReader = command.ExecuteReader()

                    While dr.Read()
                        li.Add(dr("NAME").ToString())
                    End While
                    dr.Close()
                End Using

                connection.Close()
            End Using

            Return li
        Catch ex As Exception
            MessageBox.Show("Error executing query: " & ex.Message)
            Return Nothing
        End Try
    End Function

    Public Shared Function GetTeamID(name As String) As Integer
        Try

            Dim query = "SELECT ID FROM Teams WHERE CLUB_ID = " & Club.ID & " AND NAME = '" & name & "'"


            Dim db As New DatabaseManager
            Dim teamID As Integer = -1

            Using connection As New SqlConnection(db.connectionString)
                ' Open connection
                connection.Open()

                Using command As New SqlCommand(query, connection)

                    Dim dr As SqlDataReader = command.ExecuteReader()

                    If dr.Read() Then
                        teamID = CInt(dr("ID"))
                    End If

                    dr.Close()
                End Using

                connection.Close()
            End Using

            Return teamID
        Catch ex As Exception
            MessageBox.Show("Error executing query: " & ex.Message)
            Return Nothing
        End Try
    End Function

End Class
