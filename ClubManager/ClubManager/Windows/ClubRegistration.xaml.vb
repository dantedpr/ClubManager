Imports System.Data
Imports Microsoft.Win32

Public Class ClubRegistration

    Public Property showPass = False
    Private isMaximized As Boolean = False
    Private previousWindowState As WindowState
    Private imagenClub As BitmapImage
    Public Sub New()

        ' Esta llamada es exigida por el diseñador.
        InitializeComponent()
        Load()
        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().

    End Sub

    Public Sub Load()

        AddHandler btnClose.Click, AddressOf CloseApp
        AddHandler btnMinimize.Click, AddressOf Minimize

        AddHandler BUT_Aceptar.Click, AddressOf SaveClub
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

        clubPhone.MaxLength = 9
        clubPhone.AllowNegatives = False

        AddHandler btnPassword.Click, AddressOf showPassword

    End Sub

    Public Overloads Sub CloseApp()
        System.Windows.Application.Current.Shutdown()
    End Sub

    Public Overloads Sub Minimize()
        WindowState = WindowState.Minimized
    End Sub

    Public Sub CancelCreation()
        Dim w As New MainWindow
        w.Show()
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

    Public Sub SaveClub()


        'Dim w As New FrameWindow
        'w.Show()

        'Dim w1 As New HomeWindow
        'w.CleanWindow()
        'w.Content.Children.Add(w1)
        'w1.Load()


        Dim pass = ""
        If clubPassUnmask.Visibility = Visibility.Visible AndAlso clubPassUnmask.Text <> "" Then
            pass = clubPassUnmask.Text
        Else
            If clubPassword.Password <> "" Then
                pass = clubPassword.Password
            End If
        End If
        If clubCode.Text = "" Or clubName.Text = "" Or clubAddress.Text = "" Or clubMail.Text = "" Or clubPhone.Text = "" Or clubPhone.Text.Length < 9 Or pass = "" Then

            MessageBox.Show("¡Faltán datos por introducir! Por favor revisa los datos introducidos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            Return
        Else

            Dim club As New Club
            Dim idClub = Club.SaveCheck(clubCode.Text, clubName.Text, clubAddress.Text, clubMail.Text, clubPhone.Text)
            If idClub > -1 Then
                Club.SetPassword(pass)
            Else
                MessageBox.Show("¡Ya existe un club con el mismo código o mail, prueba a cambiarlos!", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
                Return
            End If

            Dim imageBytes As Byte() = ImageManager.ImageToByteArray(imagenClub)

            ' Convertir el byte array a una cadena base64
            Dim base64String As String = ImageManager.ByteArrayToBase64String(imageBytes)
            ImageManager.SaveImageToDatabase("CLUB", idClub, imagenClub, idClub)
            Club.logoClub = base64String

            Dim db As New DatabaseManager
            If db.CheckUser(clubCode.Text, pass) Then

                Club.LoadClub(clubCode.Text)


                FrameWindow.Instance.Show()

                Dim w1 As New HomeWindow
                FrameWindow.Instance.CleanWindow()
                FrameWindow.Instance.Content.Children.Add(w1)
                w1.Load()

                Me.Close()

            Else
                MessageBox.Show("Código y contraseña incorrectos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            End If

        End If

    End Sub

    Public Sub showPassword()

        If showPass Then
            showPass = False
        Else
            showPass = True
        End If

        If showPass Then
            clubPassword.Visibility = Visibility.Hidden
            clubPassUnmask.Visibility = Visibility.Visible
            clubPassUnmask.Text = clubPassword.Password
        Else
            clubPassword.Visibility = Visibility.Visible
            clubPassUnmask.Visibility = Visibility.Hidden
            clubPassword.Password = clubPassUnmask.Text
        End If

    End Sub

End Class
