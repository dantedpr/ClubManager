﻿Class MainWindow

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
        WindowState = WindowState.Minimized
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

        txtUser.Text = "PRATAE"
        txtPass.Password = "DanteTest123"

        If txtUser.Text <> "" And txtPass.Password <> "" Then

            Dim db As New DatabaseManager
            If db.CheckUser(txtUser.Text, txtPass.Password) Then
                Dim Club As New Club

                Club.LoadClub(txtUser.Text)


                Dim w As New FrameWindow
                w.Show()

                Dim w1 As New HomeWindow
                w.CleanWindow()
                w.Content.Children.Add(w1)
                w1.Load()

                Me.Close()

            Else
                '''NO EXISTE USUARIO 
            End If


        End If

    End Sub
End Class
