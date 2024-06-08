Imports System.Data.SqlClient
Imports CefSharp.DevTools.Autofill
Imports System.Net
Imports Microsoft.Office.Interop.Excel
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class Player

    Public Property ID As Integer = -1
    Public Property Name As String
    Public Property LastName As String
    Public Property LastName2 As String
    Public Property Mail As String
    Public Property Phone As String
    Public Property Address As String
    Public Property Age As Integer
    Public Property BirthDate As Date
    Public Property Club_ID As Integer
    Public Property Team_ID As Integer
    Public Property TeamName As String
    Public Property Status As Boolean

    Public Sub New()

    End Sub

    Public Sub New(clubID As String, nameP As String, LastNameP As String, LastName2P As String, mailP As String,
                   phoneP As String, addressP As String, birthdateP As Date, teamID As String, statusP As Boolean)
        Name = nameP
        LastName = LastNameP
        LastName2 = LastName2P
        Mail = mailP
        Phone = phoneP
        Address = addressP
        BirthDate = birthdateP
        Club_ID = clubID
        Team_ID = teamID
        Status = statusP
        Age = CalculateAge(birthdateP)
    End Sub
    Function CalculateAge(birthDate As Date) As Integer
        Dim currentDate As Date
        Dim age As Integer

        ' Get the current date
        currentDate = Date.Now

        ' Calculate the difference in years
        age = DateDiff("yyyy", birthDate, currentDate)

        ' Adjust if the birthdate has not occurred yet this year
        If currentDate < DateSerial(Year(currentDate), Month(birthDate), Day(birthDate)) Then
            age = age - 1
        End If

        CalculateAge = age
    End Function
    Public Function SavePlayer() As Integer

        Dim query As String = "INSERT INTO Players ([NAME],[LASTNAME1],[LASTNAME2],[MAIL],[PHONE],[ADDRESS],[AGE],[BIRTHDATE],[CLUB_ID],[TEAM_ID],[STATUS]) 
                                VALUES (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11); SELECT SCOPE_IDENTITY();"
        Dim playerID As Integer = -1

        Dim dr As New DatabaseManager

        Try
            Using connection As New SqlConnection(dr.connectionString)
                connection.Open()

                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@p1", Name)
                    command.Parameters.AddWithValue("@p2", LastName)
                    command.Parameters.AddWithValue("@p3", LastName2)
                    command.Parameters.AddWithValue("@p4", Mail)
                    command.Parameters.AddWithValue("@p5", Phone)
                    command.Parameters.AddWithValue("@p6", Address)
                    command.Parameters.AddWithValue("@p7", Age)
                    command.Parameters.AddWithValue("@p8", BirthDate)
                    command.Parameters.AddWithValue("@p9", Club_ID)
                    command.Parameters.AddWithValue("@p10", Team_ID)
                    If Status Then
                        command.Parameters.AddWithValue("@p11", 1)
                    Else
                        command.Parameters.AddWithValue("@p11", 0)
                    End If

                    playerID = Convert.ToInt32(command.ExecuteScalar())
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine("Error al insertar el jugador: " & ex.Message)
        End Try

        ID = playerID

        Return playerID

    End Function

    Public Sub UpdatePlayer(nameP As String, LastNameP As String, LastName2P As String, mailP As String,
                   phoneP As String, addressP As String, birthdateP As Date, teamID As Integer)

        Try
            Dim query As String = "UPDATE Players SET [NAME] = @p1
                                ,[LASTNAME1] = @p2
                                ,[LASTNAME2] = @p3
                                ,[MAIL] = @p4
                                ,[PHONE] = @p5
                                ,[ADDRESS] = @p6
                                ,[AGE] = @p7
                                ,[BIRTHDATE] = @p8
                                ,[CLUB_ID] = @p9
                                ,[TEAM_ID] = @p10
                                ,[STATUS] = @p11 WHERE ID = " & ID & ""

            Dim db As New DatabaseManager

            Using connection As New SqlConnection(db.connectionString)
                connection.Open()

                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@p1", nameP)
                    command.Parameters.AddWithValue("@p2", LastNameP)
                    command.Parameters.AddWithValue("@p3", LastName2P)
                    command.Parameters.AddWithValue("@p4", mailP)
                    command.Parameters.AddWithValue("@p5", phoneP)
                    command.Parameters.AddWithValue("@p6", addressP)
                    If BirthDate <> birthdateP Then
                        Age = CalculateAge(birthdateP)
                    End If
                    command.Parameters.AddWithValue("@p7", Age)
                    command.Parameters.AddWithValue("@p8", birthdateP)
                    command.Parameters.AddWithValue("@p9", Club_ID)
                    command.Parameters.AddWithValue("@p10", teamID)
                    If Status Then
                        command.Parameters.AddWithValue("@p11", 1)
                    Else
                        command.Parameters.AddWithValue("@p11", 0)
                    End If
                    command.ExecuteScalar()
                End Using
            End Using

        Catch ex As Exception
            Console.WriteLine("Error al actualizar el jugador: " & ex.Message)
        End Try

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

    Public Sub LoadPlayer(playerID As Integer)

        Try

            Dim db As New DatabaseManager

            Dim s = "SELECT p.*, t.Name AS TEAMNAME FROM Players p LEFT JOIN Teams t ON t.ID = p.TEAM_ID WHERE p.ID = " & playerID & " AND p.CLUB_ID = " & Club.ID

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
        LastName = reader("LASTNAME1").ToString()
        LastName2 = reader("LASTNAME2").ToString()
        Mail = reader("MAIL").ToString()
        Phone = reader("PHONE").ToString()
        Address = reader("ADDRESS").ToString()
        Age = CInt(reader("AGE"))
        Dim birth = reader("BIRTHDATE").ToString()
        Date.TryParse(birth, BirthDate)
        Club_ID = CInt(reader("CLUB_ID"))
        Team_ID = CInt(reader("TEAM_ID"))
        TeamName = reader("TEAMNAME").ToString()
        Status = reader("STATUS")
    End Sub


End Class
