﻿Imports System.Data
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


        AddHandler BUT_Delete.Click, AddressOf DeleteClub
        BUT_Delete.Background = New SolidColorBrush(Colors.White)
        BUT_Delete.GetBackground = New SolidColorBrush(Colors.WhiteSmoke)
        BUT_Delete.ImageName = "delete.png"
        BUT_Delete.ButText = "Dar de baja suscripción"

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

        imagenSeleccionada.Source = ImageManager.LoadImageFromDatabase("CLUB", Club.ID, Club.ID)
        AddHandler btnPassword.Click, AddressOf showPassword
        AddHandler Me.Info_Grid.DG.MouseDoubleClick, AddressOf EditTeam

        LoadTeams()
    End Sub


    Private Sub LoadTeams()


        Dim dt As New DataTable()

        dt = Club.GetAllTeams("", "")

        Dim xquery = From a In dt.AsEnumerable
                     Select New With {.ID = a.Item("ID"), .NAME = a.Item("NAME"), .CATEGORY = a.Item("CATEGORY"), .DIVISION = a.Item("DIVISION"), .LETTER = a.Item("LETTER")
        }

        Info_Grid.DG.ItemsSource = xquery
        Info_Grid.DG.Columns.Clear()
        Info_Grid.AddColumn("ID", "ID", 50, True, System.Windows.HorizontalAlignment.Left, "INTEGER")
        Info_Grid.AddColumn("Nombre", "NAME", 200, True, System.Windows.HorizontalAlignment.Left, "TEXT")
        Info_Grid.AddColumn("Categoría", "CATEGORY", 200, True, System.Windows.HorizontalAlignment.Left, "TEXT")
        Info_Grid.AddColumn("División", "DIVISION", 200, True, System.Windows.HorizontalAlignment.Left, "TEXT")
        Info_Grid.AddColumn("Letra", "LETTER", 100, True, System.Windows.HorizontalAlignment.Left, "TEXT")
        Info_Grid.GridCounter()

    End Sub
    Private Sub EditTeam(sender As Object, e As RoutedEventArgs)

        Dim dg = Me.Info_Grid.DG

        If dg.SelectedItems.Count > 0 Then
            Dim drv = Me.Info_Grid.DG.SelectedItem
            Dim team As New Team()
            team.LoadTeam(drv.ID)
            Dim w As New EditTeam(team)

            w.ShowDialog()
            e.Handled = True

        End If

        LoadTeams()
    End Sub

    Private Sub CancelEdit(sender As Object, e As RoutedEventArgs)
        Dim w = FrameWindow.Instance
        w.CleanWindow()
        Dim w1 As New HomeWindow
        w.Content.Children.Add(w1)
        w1.Load()

    End Sub

    Private Sub DeleteClub(sender As Object, e As RoutedEventArgs)
        If MessageBox.Show("¿Está seguro que desea cancelar la suscripción a la apliación?", "Eliminar club", MessageBoxButton.YesNo, MessageBoxImage.Information) = MessageBoxResult.Yes Then
            Club.Delete()

            MessageBox.Show("Información del club actualizada con éxito.", "Club", MessageBoxButton.OK, MessageBoxImage.Information)

            System.Windows.Application.Current.Shutdown()
        Else

            e.Handled = True
            Exit Sub
        End If

        e.Handled = True
    End Sub

    Private Sub SaveClub(sender As Object, e As RoutedEventArgs)
        If MessageBox.Show("¿Está seguro que desea actualizar la información del club?", "Editar club", MessageBoxButton.YesNo, MessageBoxImage.Information) = MessageBoxResult.Yes Then
            Club.UpdateClub(clubName.Text, clubAddress.Text, clubMail.Text, clubPhone.Text)
            If clubPassUnmask.Visibility = Visibility.Visible AndAlso clubPassUnmask.Text <> "" Then
                Club.UpdatePass(clubPassUnmask.Text)
            Else
                If clubPassword.Password <> "" Then
                    Club.UpdatePass(clubPassword.Password)
                End If
            End If
            Club.LoadClub(Club.Code)
            FrameWindow.Instance.LabelClub.Content = Club.Name
            If imagenClub IsNot Nothing Then
                ImageManager.UpdateImage("CLUB", Club.ID, imagenClub, Club.ID)
            End If
            MessageBox.Show("Información del club actualizada con éxito.", "Club", MessageBoxButton.OK, MessageBoxImage.Information)
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
