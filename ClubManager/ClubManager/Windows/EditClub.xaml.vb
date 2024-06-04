Imports System.Data
Imports Microsoft.Win32

Class EditClub

    Public Property username As String
    Private Property pass As String
    Public Property showPass = False
    Public Property count As Integer = 0
    Private imagenClub As BitmapImage
    Public Sub New()

        ' Esta llamada es exigida por el diseñador.
        InitializeComponent()
        Load()
        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().

    End Sub

    Public Sub Load()
        AddHandler BUT_Aceptar.Click, AddressOf SaveClub
        BUT_Aceptar.Background = New SolidColorBrush(Colors.White)
        'BUT_Aceptar.GetBackground = New SolidColorBrush(System.Windows.Media.Color.FromRgb(210, 213, 214))
        BUT_Aceptar.GetBackground = New SolidColorBrush(Colors.LightGreen)
        BUT_Aceptar.ImageName = "accept.png"
        BUT_Aceptar.ButText = "Aceptar"

        AddHandler BUT_Cancel.Click, AddressOf CancelEdit
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

        clubCode.Text = Club.Code
        clubCode.IsEnabled = False
        clubAddress.Text = Club.Address
        clubPhone.Text = Club.Phone
        clubMail.Text = Club.Mail
        clubName.Text = Club.Name

        AddHandler btnPassword.Click, AddressOf showPassword
    End Sub

    Private Sub CancelEdit(sender As Object, e As RoutedEventArgs)
        Dim w = FrameWindow.Instance
        w.CleanWindow()
        Dim w1 As New HomeWindow
        w.Content.Children.Add(w1)
        w1.Load()

    End Sub

    Private Sub SaveClub(sender As Object, e As RoutedEventArgs)
        If MessageBox.Show("¿Está seguro que desea actualizar la información del club?", "Editar club", MessageBoxButton.YesNo, MessageBoxImage.Information) = MessageBoxResult.Yes Then
            Club.UpdateClub(clubName.Text, clubAddress.Text, clubMail.Text, clubPhone.Text)
        Else

            e.Handled = True
            Exit Sub
        End If

        e.Handled = True
    End Sub

    Public Sub showPassword(sender As Object, e As RoutedEventArgs)

        If clubPassword.Visibility = Visibility.Visible AndAlso count = 0 Then
            count += 1
            clubPassUnmask.Text = clubPassword.Password
            clubPassword.Visibility = Visibility.Collapsed
            clubPassUnmask.Visibility = Visibility.Visible
        ElseIf count = 1 Then
            count = 0
        Else
            count += 1
            clubPassword.Password = clubPassUnmask.Text
            clubPassword.Visibility = Visibility.Visible
            clubPassUnmask.Visibility = Visibility.Collapsed
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

End Class
