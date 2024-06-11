Imports System.Data.SqlClient
Imports CefSharp.DevTools.Autofill
Imports System.Net
Imports Microsoft.Office.Interop.Excel

Public Class Material

    Public Property ID As Integer = -1
    Public Property Name As String
    Public Property Category As String
    Public Property Quantity As Integer
    Public Property Club_ID As String
    Public Sub New()

    End Sub

    Public Sub New(clubID As String, nameM As String, categoryM As String, quantityM As Integer)
        Name = nameM
        Category = categoryM
        Quantity = quantityM
        Club_ID = clubID
    End Sub

    Public Function SaveMaterial() As Integer

        Dim query As String = "INSERT INTO Material ([NAME], [CATEGORY], [QUANTITY], [CLUB_ID]) VALUES (@p1, @p2, @p3, @p4); SELECT SCOPE_IDENTITY();"
        Dim teamId As Integer = -1

        Dim dr As New DatabaseManager

        Try
            Using connection As New SqlConnection(dr.connectionString)
                connection.Open()

                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@p1", Name)
                    command.Parameters.AddWithValue("@p2", Category)
                    command.Parameters.AddWithValue("@p3", Quantity)
                    command.Parameters.AddWithValue("@p4", Club_ID)

                    ' Execute the insert query and get the inserted ID
                    teamId = Convert.ToInt32(command.ExecuteScalar())
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine("Error al insertar el material: " & ex.Message)
        End Try

        ID = teamId

        Return teamId

    End Function

    Public Sub UpdateMaterial(nameM As String, categoryM As String, quantityM As Integer)

        Try
            Dim query As String = "UPDATE Material SET [NAME] = @p1, [CATEGORY] = @p2, [QUANTITY] = @p3 WHERE ID = " & ID & ""

            Dim db As New DatabaseManager

            Using connection As New SqlConnection(db.connectionString)
                connection.Open()

                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@p1", nameM)
                    command.Parameters.AddWithValue("@p2", categoryM)
                    command.Parameters.AddWithValue("@p3", quantityM)
                    command.ExecuteScalar()
                End Using
            End Using

        Catch ex As Exception
            Console.WriteLine("Error al actualizar el material: " & ex.Message)
        End Try

    End Sub

    Public Sub LoadMaterial(materialID As Integer)

        Try

            Dim db As New DatabaseManager

            Dim s = "SELECT * FROM Material WHERE ID = " & materialID & ""

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
        Quantity = CInt(reader("QUANTITY"))
        Club_ID = reader("Club_ID").ToString()
    End Sub

    Public Sub DeleteMaterial()

        Try

            Dim query As String = "DELETE [Material] WHERE ID = " & ID & ""

            Dim db As New DatabaseManager

            Using connection As New SqlConnection(db.connectionString)
                connection.Open()

                Using command As New SqlCommand(query, connection)
                    command.ExecuteScalar()
                End Using
            End Using

        Catch ex As Exception
            Console.WriteLine("Error al eliminar el material: " & ex.Message)
        End Try

    End Sub

End Class
