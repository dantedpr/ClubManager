Imports System.Data.SqlClient

Public Class DatabaseManager

    Private connectionString As String

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
End Class

