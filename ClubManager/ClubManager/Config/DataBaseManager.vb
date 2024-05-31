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


End Class

