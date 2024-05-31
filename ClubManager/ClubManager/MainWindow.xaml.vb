Class MainWindow

    Public Property username As String
    Private Property pass As String

    Public Sub New()

        ' Esta llamada es exigida por el diseñador.
        InitializeComponent()
        Load()
        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().

    End Sub

    Public Sub Load()

        AddHandler btnClose.Click, AddressOf CloseApp
        AddHandler btnRegister.Click, AddressOf RegisterAccount
        AddHandler btnLogin.Click, AddressOf Login


    End Sub


    Public Overloads Sub CloseApp()
        System.Windows.Application.Current.Shutdown()
    End Sub

    Public Overloads Sub Minimize()
        WindowState = WindowState.Maximized
    End Sub

    Public Sub AllowMovement(sender As Object, e As MouseEventArgs)
        If e.LeftButton = MouseButtonState.Pressed Then
            DragMove()
        End If
    End Sub

    Public Sub RegisterAccount()

        Dim w As New ClubRegistration

        w.Show()

        Me.Close()

    End Sub

    Public Sub Login()

        If txtUser.Text <> "" And txtPass.Password <> "" Then

            Dim db As New DatabaseManager
            If db.CheckUser(txtUser.Text, txtPass.Password) Then
                Dim Club As New Club

                Club.LoadClub(txtUser.Text)
            Else
                '''NO EXISTE USUARIO 
            End If


        End If

    End Sub
End Class
