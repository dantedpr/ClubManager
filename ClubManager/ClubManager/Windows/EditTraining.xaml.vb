Imports System.Data
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports Microsoft.Win32

Public Class EditTraining

    Public Property showPass = False
    Public Property _training As Training
    Private isMaximized As Boolean = False
    Private previousWindowState As WindowState
    Private imagenPlayer As BitmapImage
    Private mats As List(Of String)
    Public Sub New()

        ' Esta llamada es exigida por el diseñador.
        InitializeComponent()
        _training = New Training()
        Load()
        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().

    End Sub

    Public Sub New(myTraining As Training)

        ' Esta llamada es exigida por el diseñador.
        InitializeComponent()
        _training = myTraining
        Load()
        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().

    End Sub

    Public Sub Load()

        AddHandler btnClose.Click, AddressOf CloseApp
        AddHandler btnMinimize.Click, AddressOf Minimize

        AddHandler BUT_Aceptar.Click, AddressOf SaveTraining
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

        newMat.GetBackground = New SolidColorBrush(Colors.White)
        newMat.ImageName = "plus.png"
        newMat.ButText = ""
        newMat.ToolTip = "Añadir material"
        newMat.Border = 0
        newMat.st1.HorizontalAlignment = HorizontalAlignment.Left
        newMat.LBL_Text.FontSize = 8
        newMat.LBL_Text.Margin = New Thickness(15, 1, 0, 0)
        AddHandler newMat.Click, AddressOf AddMaterial

        delMat.GetBackground = New SolidColorBrush(Colors.White)
        delMat.ImageName = "delete.png"
        delMat.ButText = ""
        delMat.ToolTip = "Eliminar material"
        delMat.Border = 0
        delMat.st1.HorizontalAlignment = HorizontalAlignment.Left
        delMat.LBL_Text.FontSize = 8
        delMat.LBL_Text.Margin = New Thickness(15, 1, 0, 0)
        AddHandler delMat.Click, AddressOf DeleteMaterial
        Dim li = Club.GetTeamsName()
        trainingTeam.Items.Clear()
        trainingTeam.Items.Add("")
        For Each x In li
            trainingTeam.Items.Add(x)
        Next
        trainingTeam.SelectedIndex = 0
        trainingDate.SelectedDate = Today
        trainingDate.SelectedDateFormat = DatePickerFormat.Short
        trainingDate.FirstDayOfWeek = DayOfWeek.Monday

        Dim startTime As DateTime = DateTime.Parse("08:30")
        Dim endTime As DateTime = DateTime.Parse("22:30")

        While startTime <= endTime
            trainingHour.Items.Add(startTime.ToString("HH:mm"))
            startTime = startTime.AddMinutes(30)
        End While

        li = Club.GetInstallations()
        trainingGround.Items.Clear()
        trainingGround.Items.Add("")
        For Each x In li
            trainingGround.Items.Add(x)
        Next
        trainingGround.SelectedIndex = 0

        li = Club.GetPitches()
        trainingPitch.Items.Clear()
        trainingPitch.Items.Add("")
        For Each x In li
            trainingPitch.Items.Add(x)
        Next
        trainingPitch.SelectedIndex = 0

        If _training.ID <> -1 Then

            trainingTeam.IsEnabled = False

            trainingDate.SelectedDate = _training.trainingDate
            For Each cat In trainingTeam.Items
                If cat.ToString() = _training.Team Then
                    trainingTeam.SelectedItem = cat
                    Exit For
                End If
            Next
            For Each cat In trainingHour.Items
                If cat.ToString() = _training.Hour Then
                    trainingHour.SelectedItem = cat
                End If
            Next
            For Each cat In trainingGround.Items
                If cat.ToString() = _training.Stadium Then
                    trainingGround.SelectedItem = cat
                    Exit For
                End If
            Next
            For Each cat In trainingPitch.Items
                If cat.ToString() = _training.Ground Then
                    trainingPitch.SelectedItem = cat
                    Exit For
                End If
            Next

            observations.Text = _training.Observations
        End If

        LoadMaterialTraining()

    End Sub

    Public Overloads Sub CloseApp()
        System.Windows.Application.Current.Shutdown()
    End Sub

    Public Overloads Sub Minimize()
        WindowState = WindowState.Minimized
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

    Public Sub LoadMaterialTraining()


        Dim dt As New DataTable()

        dt = _training.GetMaterials(_training.ID)



        Dim xquery = From a In dt.AsEnumerable
                     Select New With {.ID = a.Item("ID"), .NAME = a.Item("NAME"), .QUANTITY = a.Item("QUANTITY")
        }


        Info_Grid.DG.ItemsSource = xquery
        Info_Grid.DG.Columns.Clear()
        Info_Grid.AddColumn("ID", "ID", 50, True, System.Windows.HorizontalAlignment.Left, "INTEGER")
        Info_Grid.AddColumn("Nombre", "NAME", 100, True, System.Windows.HorizontalAlignment.Left, "TEXT")
        Info_Grid.AddColumn("Cantidad", "QUANTITY", 100, True, System.Windows.HorizontalAlignment.Left, "INTEGER")
        Info_Grid.GridCounter()

        mats = New List(Of String)

        For Each x In xquery
            mats.Add(x.NAME)
        Next

        Dim liMat = Club.GetMaterialsNames()

        materialCombo.Items.Clear()
        materialCombo.Items.Add("")
        materialCombo.SelectedIndex = 0
        matQuantity.Text = ""
        For Each mat In liMat
            If Not mats.Contains(mat) Then
                materialCombo.Items.Add(mat)
            End If
        Next

    End Sub

    Private Sub SaveTraining(sender As Object, e As RoutedEventArgs)

        If trainingTeam.SelectedItem.ToString() = "" Or trainingGround.SelectedItem.ToString() = "" Or trainingPitch.SelectedItem.ToString() = "" Or trainingHour.SelectedItem.ToString() = "" Or trainingDate.SelectedDate < Date.Now Then
            MessageBox.Show("¡Faltán datos por seleccionar o la fecha es posterior a la actual!", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            Return
        Else

            Dim trainingID = -1
            If _training.ID <> -1 Then
                If Not Club.CheckTraining(trainingDate.SelectedDate, trainingHour.SelectedItem.ToString(), trainingGround.SelectedItem.ToString(), trainingPitch.SelectedItem.ToString(), _training.ID) Then
                    _training.UpdateTraining(trainingDate.SelectedDate, trainingHour.SelectedItem.ToString(), trainingGround.SelectedItem.ToString(), trainingPitch.SelectedItem.ToString(), observations.Text, trainingTeam.SelectedItem.ToString())
                Else
                    MessageBox.Show("Existe un entrenamiento ya para la fecha, hora en la instalación y campo indicado.", "Entrenamiento", MessageBoxButton.OK, MessageBoxImage.Information)
                    Return
                End If

            Else

                If Not Club.CheckTraining(trainingDate.SelectedDate, trainingHour.SelectedItem.ToString(), trainingGround.SelectedItem.ToString(), trainingPitch.SelectedItem.ToString()) Then
                    Dim t1 = New Training(Club.ID, trainingDate.SelectedDate, trainingHour.SelectedItem.ToString(), trainingGround.SelectedItem.ToString(), trainingPitch.SelectedItem.ToString(), observations.Text, trainingTeam.SelectedItem.ToString())
                    trainingID = t1.SaveTraining()
                Else
                    MessageBox.Show("Existe un entrenamiento ya para la fecha, hora en la instalación y campo indicado.", "Entrenamiento", MessageBoxButton.OK, MessageBoxImage.Information)
                    Return
                End If

            End If

            MessageBox.Show("Se han guardado los datos del entrenamiento.", "Entrenamiento", MessageBoxButton.OK, MessageBoxImage.Information)

        End If

        e.Handled = True
        Me.Close()

    End Sub

    Private Sub AddMaterial(sender As Object, e As RoutedEventArgs)

        If materialCombo.SelectedItem.ToString() <> "" And matQuantity.Text <> "" AndAlso CInt(matQuantity.Text) > 0 Then
            Dim quantity = Club.CheckQuantity(materialCombo.SelectedItem.ToString())

            If quantity >= CInt(matQuantity.Text) Then
                _training.AddMaterial(materialCombo.SelectedItem.ToString(), CInt(matQuantity.Text))
            Else
                MessageBox.Show("La cantidad no puede ser superior a " & quantity & ", ya que es la cantidad disponible.", "Entrenamiento", MessageBoxButton.OK, MessageBoxImage.Information)
                Return
            End If
        Else
            MessageBox.Show("No ha seleccionado un material o la cantidad no es superior a 0.", "Entrenamiento", MessageBoxButton.OK, MessageBoxImage.Information)
            Return
        End If

        LoadMaterialTraining()

        e.Handled = True

    End Sub

    Private Sub DeleteMaterial(sender As Object, e As RoutedEventArgs)
        Dim dg = Me.Info_Grid.DG

        If dg.SelectedItems.Count > 0 Then
            If MessageBox.Show("¿Está seguro que desea eliminar el material de la sesión?", "Eliminar material de la sesión", MessageBoxButton.YesNo, MessageBoxImage.Information) = MessageBoxResult.Yes Then
                Dim drv = Me.Info_Grid.DG.SelectedItem
                _training.DeleteMaterial(drv.ID)
                e.Handled = True

                LoadMaterialTraining()
            Else

                e.Handled = True
                Exit Sub
            End If

            e.Handled = True
        Else
            MessageBox.Show("Debe seleccionar primero un material de la tabla.", "Eliminar material de la sesión", MessageBoxButton.OK, MessageBoxImage.Information)
        End If

        e.Handled = True

    End Sub
End Class
