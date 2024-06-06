Imports System.Data.SqlClient
Imports CefSharp.DevTools.Autofill
Imports System.Net
Imports Microsoft.Office.Interop.Excel

Public Class Team

    Public Property ID As Integer = -1
    Public Property Name As String
    Public Property Category As String
    Public Property Division As String
    Public Property Club_ID As String
    Public Property Letter As String

    Public Sub New()

    End Sub

    Public Sub New(clubID As String, nameT As String, categoryT As String, divisionT As String, letterT As String)
        Name = nameT
        Category = categoryT
        Division = divisionT
        Letter = letterT
        Club_ID = clubID
    End Sub

    Public Function SaveTeam() As Integer

        Dim query As String = "INSERT INTO Teams ([NAME], [CATEGORY], [DIVISION], [CLUB_ID], [LETTER]) VALUES (@p1, @p2, @p3, @p4, @p5); SELECT SCOPE_IDENTITY();"
        Dim teamId As Integer = -1

        Dim dr As New DatabaseManager

        Try
            Using connection As New SqlConnection(dr.connectionString)
                connection.Open()

                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@p1", Name)
                    command.Parameters.AddWithValue("@p2", Category)
                    command.Parameters.AddWithValue("@p3", Division)
                    command.Parameters.AddWithValue("@p4", Club_ID)
                    command.Parameters.AddWithValue("@p5", Letter)

                    ' Execute the insert query and get the inserted ID
                    teamId = Convert.ToInt32(command.ExecuteScalar())
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine("Error al insertar el club: " & ex.Message)
        End Try

        ID = teamId

        Return teamId

    End Function

    Public Sub UpdateTeam(nameT As String, categoryT As String, divisionT As String, letterT As String)

        Try
            Dim query As String = "UPDATE Teams SET [NAME] = @p1, [CATEGORY] = @p2, [DIVISION] = @p3, [LETTER] = @p4 WHERE ID = " & ID & ""

            Dim db As New DatabaseManager

            Using connection As New SqlConnection(db.connectionString)
                connection.Open()

                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@p1", nameT)
                    command.Parameters.AddWithValue("@p2", categoryT)
                    command.Parameters.AddWithValue("@p3", divisionT)
                    command.Parameters.AddWithValue("@p4", letterT)
                    command.ExecuteScalar()
                End Using
            End Using

        Catch ex As Exception
            Console.WriteLine("Error al actualizar la plantilla: " & ex.Message)
        End Try

    End Sub

    Public Sub LoadTeam(teamId As Integer)

        Try

            Dim db As New DatabaseManager

            Dim s = "SELECT * FROM Teams WHERE ID = " & teamId & ""

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

    Private Sub Map(ByVal reader As SqlDataReader)
        ID = CInt(reader("ID"))
        Name = reader("NAME").ToString()
        Category = reader("CATEGORY").ToString()
        Division = reader("Division").ToString()
        Club_ID = reader("Club_ID").ToString()
        Letter = reader("Letter").ToString()
    End Sub

    Public Sub DeleteTeam()

        Try

            UpdatePlayers()

            Dim query As String = "DELETE [Teams] WHERE ID = " & ID & ""

            Dim db As New DatabaseManager

            Using connection As New SqlConnection(db.connectionString)
                connection.Open()

                Using command As New SqlCommand(query, connection)
                    command.ExecuteScalar()
                End Using
            End Using

        Catch ex As Exception
            Console.WriteLine("Error al eliminar la plantilla: " & ex.Message)
        End Try

    End Sub

    Public Sub UpdatePlayers()

        Try
            Dim query As String = "UPDATE Players SET [TEAM_ID] = @p1 WHERE TEAM_ID = " & ID & ""

            Dim db As New DatabaseManager

            Using connection As New SqlConnection(db.connectionString)
                connection.Open()

                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@p1", DBNull.Value)
                    command.ExecuteScalar()
                End Using
            End Using

        Catch ex As Exception
            Console.WriteLine("Error al actualizar la plantilla: " & ex.Message)
        End Try

    End Sub

End Class
