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
        AddHandler btnRecover.Click, AddressOf RecoverPass

    End Sub


    Public Overloads Sub CloseApp()
        System.Windows.Application.Current.Shutdown()
    End Sub

    Public Overloads Sub Minimize()
        WindowState = WindowState.Minimized
    End Sub

    Public Sub AllowMovement(sender As Object, e As MouseEventArgs)
        If e.LeftButton = MouseButtonState.Pressed Then
            DragMove()
        End If
    End Sub

    Public Sub RecoverPass()

        If txtUser.Text <> "" Then
            Dim db As New DatabaseManager
            Dim id = db.RecoverUser(txtUser.Text)

            If id <> -1 Then
                Dim code = GenerarCodigoAleatorio()
                Dim w As New RecoverPass(code, txtUser.Text)

                w.ShowDialog()
            Else
                MessageBox.Show("No existe ningún club con el código indicado.", "Recuperar contraseña", MessageBoxButton.OK, MessageBoxImage.Information)
            End If

        Else
            MessageBox.Show("Introduce el código del club para poder restablecer la contraseña.", "Recuperar contraseña", MessageBoxButton.OK, MessageBoxImage.Information)
        End If

    End Sub

    Public Sub RegisterAccount()

        Dim w As New ClubRegistration

        w.Show()

        Me.Close()

    End Sub

    Public Function GenerarCodigoAleatorio() As String
        Dim caracteres As String
        caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"
        Dim codigo As String
        codigo = ""
        Dim rnd As New Random()

        For i As Integer = 1 To 6
            Dim indice As Integer = rnd.Next(0, caracteres.Length)
            codigo &= caracteres(indice)
        Next i

        GenerarCodigoAleatorio = codigo
    End Function

    Public Sub Login()

        txtUser.Text = "PRATAE"
        txtPass.Password = "FIB"

        If txtUser.Text <> "" And txtPass.Password <> "" Then

            Dim db As New DatabaseManager
            If db.CheckUser(txtUser.Text, txtPass.Password) Then
                Dim Club As New Club

                Club.LoadClub(txtUser.Text)


                FrameWindow.Instance.Show()

                Dim w1 As New HomeWindow
                FrameWindow.Instance.CleanWindow()
                FrameWindow.Instance.Content.Children.Add(w1)

                Me.Close()

            Else
                MessageBox.Show("Código y contraseña incorrectos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            End If


        End If

    End Sub
End Class
