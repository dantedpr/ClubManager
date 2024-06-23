Imports System.Data.SqlClient
Imports CefSharp.DevTools.Autofill
Imports System.Net
Imports Microsoft.Office.Interop.Excel
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox
Imports System.Data

Public Class Training

    Public Property ID As Integer = -1
    Public Property Team As String
    Public Property trainingDate As Date
    Public Property Hour As String
    Public Property Club_ID As String
    Public Property Stadium As String
    Public Property Ground As String
    Public Property Observations As String
    Public Sub New()

    End Sub

    Public Sub New(clubID As String, dateT As Date, hourT As String, stadiumT As String, groundT As String, observationsT As String, teamT As String)
        Team = teamT
        trainingDate = dateT
        Hour = hourT
        Stadium = stadiumT
        Ground = groundT
        Observations = observationsT
        Club_ID = clubID
    End Sub

    Public Function SaveTraining() As Integer

        Dim query As String = "INSERT INTO Trainings ([TEAM],[DATE],[CLUB_ID],[HOUR],[STADIUM],[GROUND],[OBSERVATIONS]) VALUES (@p1, @p2, @p3, @p4, @p5, @p6, @p7); SELECT SCOPE_IDENTITY();"
        Dim teamId As Integer = -1

        Dim dr As New DatabaseManager
        Dim timeString As String = Hour
        Dim timeValue As DateTime = DateTime.Parse(timeString)
        Try
            Using connection As New SqlConnection(dr.connectionString)
                connection.Open()

                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@p1", Team)
                    command.Parameters.AddWithValue("@p2", trainingDate)
                    command.Parameters.AddWithValue("@p3", Club_ID)
                    command.Parameters.AddWithValue("@p4", timeValue)
                    command.Parameters.AddWithValue("@p5", Stadium)
                    command.Parameters.AddWithValue("@p6", Ground)
                    command.Parameters.AddWithValue("@p7", Observations)

                    ' Execute the insert query and get the inserted ID
                    teamId = Convert.ToInt32(command.ExecuteScalar())
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine("Error al insertar el entrenamiento: " & ex.Message)
        End Try

        ID = teamId

        Return teamId

    End Function

    Public Sub UpdateTraining(dateT As Date, hourT As String, stadiumT As String, groundT As String, observationsT As String, teamT As String)

        Try
            Dim query As String = "UPDATE Trainings SET [TEAM] = @p1, [DATE] = @p2, [HOUR] = @p3, [STADIUM] = @p4, [GROUND] = @p5, [OBSERVATIONS] = @p6 WHERE ID = " & ID & ""

            Dim db As New DatabaseManager

            Using connection As New SqlConnection(db.connectionString)
                connection.Open()

                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@p1", teamT)
                    command.Parameters.AddWithValue("@p2", dateT)
                    command.Parameters.AddWithValue("@p3", hourT)
                    command.Parameters.AddWithValue("@p4", stadiumT)
                    command.Parameters.AddWithValue("@p5", groundT)
                    command.Parameters.AddWithValue("@p6", observationsT)
                    command.ExecuteScalar()
                End Using
            End Using

        Catch ex As Exception
            Console.WriteLine("Error al actualizar el material: " & ex.Message)
        End Try

    End Sub

    Public Sub LoadTraining(trainingID As Integer)

        Try

            Dim db As New DatabaseManager

            Dim s = "SELECT * FROM Trainings WHERE ID = " & trainingID & ""

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
        Team = reader("TEAM").ToString()
        Dim dateT = reader("DATE").ToString()
        Date.TryParse(dateT, trainingDate)
        Dim timeValue As TimeSpan = reader.GetTimeSpan(reader.GetOrdinal("HOUR"))
        ' Format the time value as HH:mm string
        Hour = timeValue.ToString("hh\:mm")
        Stadium = reader("STADIUM").ToString()
        Ground = reader("GROUND").ToString()
        Observations = reader("OBSERVATIONS").ToString()
        Club_ID = reader("CLUB_ID").ToString()
    End Sub

    Public Sub DeleteTraining()

        Try

            Dim query As String = "DELETE [Trainings] WHERE ID = " & ID & ""

            Dim db As New DatabaseManager

            Using connection As New SqlConnection(db.connectionString)
                connection.Open()

                Using command As New SqlCommand(query, connection)
                    command.ExecuteScalar()
                End Using
            End Using

        Catch ex As Exception
            Console.WriteLine("Error al eliminar el entrenamiento: " & ex.Message)
        End Try

    End Sub

    Public Function GetMaterials(trainingID) As System.Data.DataTable

        Try

            Dim db As New DatabaseManager
            Dim dt As New System.Data.DataTable()
            Dim s = "  SELECT mt.ID, m.Name, mt.Quantity FROM [ClubManager].[dbo].[MaterialTrainings] mt LEFT JOIN Material m ON m.ID = mt.Material_ID WHERE mt.Training_ID = " & trainingID & ""

            Using connection As New SqlConnection(db.connectionString)
                ' Open connection
                connection.Open()

                Using command As New SqlCommand(s, connection)

                    Dim dr As SqlDataReader = command.ExecuteReader()
                    dt.Load(dr)
                    dr.Close()
                End Using

                connection.Close()
            End Using

            Return dt
        Catch ex As Exception
            MessageBox.Show("Error executing query: " & ex.Message)
            Return Nothing
        End Try

    End Function

    Public Sub DeleteMaterial(matID As Integer)

        Try

            Dim query As String = "DELETE [MaterialTrainings] WHERE ID = " & matID & ""

            Dim db As New DatabaseManager

            Using connection As New SqlConnection(db.connectionString)
                connection.Open()

                Using command As New SqlCommand(query, connection)
                    command.ExecuteScalar()
                End Using
            End Using

        Catch ex As Exception
            Console.WriteLine("Error al eliminar el entrenamiento: " & ex.Message)
        End Try

    End Sub

    Public Function AddMaterial(matName As String, quantity As Integer) As Integer

        Dim matID = Club.GetMaterialID(matName)

        Dim query As String = "INSERT INTO MaterialTrainings ([TRAINING_ID],[MATERIAL_ID],[QUANTITY]) VALUES (@p1, @p2, @p3); SELECT SCOPE_IDENTITY();"
        Dim matTraining As Integer = -1

        Dim dr As New DatabaseManager
        Dim timeString As String = Hour
        Dim timeValue As DateTime = DateTime.Parse(timeString)
        Try
            Using connection As New SqlConnection(dr.connectionString)
                connection.Open()

                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@p1", ID)
                    command.Parameters.AddWithValue("@p2", matID)
                    command.Parameters.AddWithValue("@p3", quantity)

                    ' Execute the insert query and get the inserted ID
                    matTraining = Convert.ToInt32(command.ExecuteScalar())
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine("Error al insertar el material en el entrenamiento: " & ex.Message)
        End Try


        Return matTraining

    End Function

End Class
