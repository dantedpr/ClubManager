﻿Imports System.Data
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports Microsoft.Win32

Public Class EditPlayer

    Public Property showPass = False
    Public Property _player As Player
    Private isMaximized As Boolean = False
    Private previousWindowState As WindowState
    Private imagenClub As BitmapImage
    Public Sub New()

        ' Esta llamada es exigida por el diseñador.
        InitializeComponent()
        _player = New Player()
        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().

    End Sub

    Public Sub New(myPlayer As Player)

        ' Esta llamada es exigida por el diseñador.
        InitializeComponent()
        _player = myPlayer
        Load()
        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().

    End Sub

    Public Sub Load()

        AddHandler btnClose.Click, AddressOf CloseApp
        AddHandler btnMinimize.Click, AddressOf Minimize

        AddHandler BUT_Aceptar.Click, AddressOf SavePlayer
        BUT_Aceptar.Background = New SolidColorBrush(Colors.White)
        'BUT_Aceptar.GetBackground = New SolidColorBrush(System.Windows.Media.Color.FromRgb(210, 213, 214))
        BUT_Aceptar.GetBackground = New SolidColorBrush(Colors.LightGreen)
        BUT_Aceptar.ImageName = "accept.png"
        BUT_Aceptar.ButText = "Aceptar"

        AddHandler BUT_Cancel.Click, AddressOf CancelCreation
        BUT_Cancel.Background = New SolidColorBrush(Colors.White)
        'BUT_Cancel.GetBackground = New SolidColorBrush(System.Windows.Media.Color.FromRgb(210, 213, 214))
        BUT_Cancel.GetBackground = New SolidColorBrush(Colors.LightSalmon)
        BUT_Cancel.ImageName = "cancel.png"
        BUT_Cancel.ButText = "Cancelar"

        BUT_Image.Background = New SolidColorBrush(Colors.White)
        BUT_Image.GetBackground = New SolidColorBrush(Colors.LightBlue)
        BUT_Image.ImageName = "upload.png"
        BUT_Image.ButText = "Seleccionar escudo"

        Dim li = Club.GetTeamsName()
        playerTeam.Items.Clear()
        playerTeam.Items.Add("")
        For Each x In li
            playerTeam.Items.Add(x)
        Next
        playerTeam.SelectedIndex = 0


        If _player.ID <> -1 Then
            playerName.Text = _player.Name
            playerLast.Text = _player.LastName
            playerLast2.Text = _player.LastName2
            playerAddress.Text = _player.Address
            playerPhone.Text = _player.Phone
            playerMail.Text = _player.Mail
            playerAge.SelectedDate = _player.BirthDate



            For Each cat In playerTeam.Items
                If cat.ToString() = _player.TeamName Then
                    playerTeam.SelectedItem = cat
                End If
            Next
        End If

    End Sub

    Public Overloads Sub CloseApp()
        System.Windows.Application.Current.Shutdown()
    End Sub

    Public Overloads Sub Minimize()
        WindowState = WindowState.Minimized
    End Sub
    Private Sub SeleccionarImagen_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Dim openFileDialog As New OpenFileDialog()
        openFileDialog.Filter = "Imágenes PNG (*.png)|*.png"
        openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)

        If openFileDialog.ShowDialog() = True Then
            Try
                Dim rutaImagen As String = openFileDialog.FileName
                Dim imagen As New BitmapImage(New Uri(rutaImagen))
                imagenClub = imagen
                imagenSeleccionada.Source = imagen
            Catch ex As Exception
                MessageBox.Show($"Error al cargar la imagen: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            End Try
        End If
    End Sub

    Public Sub CancelCreation()
        Me.Close()
    End Sub
    Public Sub AllowMovement(sender As Object, e As MouseEventArgs)
        If e.LeftButton = MouseButtonState.Pressed Then
            DragMove()
        End If
    End Sub

    Private Sub Expand(sender As Object, e As RoutedEventArgs)
        If isMaximized Then
            ' Restaurar el estado previo de la ventana
            Me.WindowState = previousWindowState
            isMaximized = False
        Else
            ' Maximizar la ventana y guardar el estado previo
            previousWindowState = Me.WindowState
            Me.WindowState = WindowState.Maximized
            isMaximized = True
        End If
    End Sub


    Private Sub SavePlayer(sender As Object, e As RoutedEventArgs)

        If playerName.Text = "" Or playerLast.Text = "" Or playerPhone.Text = "" Or playerAddress.Text = "" Or playerAge.SelectedDate.ToString() = "" Then
            MessageBox.Show("¡Faltán datos por introducir! Por favor revisa los datos introducidos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            Return
        Else
            'clubID As String, nameP As String, LastNameP As String, LastName2P As String, mailP As String,
            '       phoneP As String, addressP As String, birthdateP As Date, teamID As String, statusP As Boolean)

            Dim teamID = -1
            If playerTeam.SelectedItem.ToString() <> "" Then
                teamID = Club.GetTeamID(playerTeam.SelectedItem.ToString())
            End If
            If _player.ID <> -1 Then
                _player.UpdatePlayer(playerName.Text, playerLast.Text, playerLast2.Text, playerMail.Text, playerPhone.Text, playerAddress.Text,
                                    playerAge.SelectedDate.Value, teamID)
            Else
                Dim t1 = New Player(Club.ID, playerName.Text, playerLast.Text, playerLast2.Text, playerMail.Text, playerPhone.Text, playerAddress.Text,
                                    playerAge.SelectedDate.Value, teamID, True)
                t1.SavePlayer()
            End If
            MessageBox.Show("Se han guardado los datos del jugador.", "Jugador", MessageBoxButton.OK, MessageBoxImage.Information)
        End If

        e.Handled = True
        Me.Close()

    End Sub


End Class