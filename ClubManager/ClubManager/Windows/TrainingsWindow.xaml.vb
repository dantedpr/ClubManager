Imports System.Data
Imports System.Windows.Forms

Class TrainingsWindow

    Public Property username As String
    Private Property pass As String

    Public Sub New()

        ' Esta llamada es exigida por el diseñador.
        InitializeComponent()
        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().

    End Sub

    Public Sub Load()

        AddHandler BUT_NewTeam.Click, AddressOf CreateTraining
        BUT_NewTeam.Background = New SolidColorBrush(Colors.White)
        'BUT_Aceptar.GetBackground = New SolidColorBrush(System.Windows.Media.Color.FromRgb(210, 213, 214))
        BUT_NewTeam.GetBackground = New SolidColorBrush(Colors.White)
        BUT_NewTeam.ImageName = "plus.png"
        BUT_NewTeam.ButText = "Crear entrenamiento"

        AddHandler BUT_Delete.Click, AddressOf DeleteTraining
        BUT_Delete.Background = New SolidColorBrush(Colors.White)
        BUT_Delete.GetBackground = New SolidColorBrush(Colors.WhiteSmoke)
        BUT_Delete.ImageName = "delete.png"
        BUT_Delete.ButText = "Cancelar entrenamiento"

        AddHandler BUT_Search.Click, AddressOf Seach
        BUT_Search.Background = New SolidColorBrush(Colors.White)
        BUT_Search.GetBackground = New SolidColorBrush(Colors.White)
        BUT_Search.ImageName = "transparency.png"
        BUT_Search.ButText = "Buscar"

        BUT_Clean.ToolTip = "Reestablecer fechas"

        iniDate.SelectedDate = Date.Today
        finDate.SelectedDate = GetLastDayOfWeek(DateTime.Now)
        AddHandler Me.Info_Grid.DG.MouseDoubleClick, AddressOf EditTraining
        AddHandler BUT_Clean.Click, AddressOf CleanDates

        LoadTrainings()

    End Sub

    Private Function GetLastDayOfWeek(currentDate As Date) As Date
        Dim daysUntilEndOfWeek As Integer = DayOfWeek.Sunday - currentDate.DayOfWeek
        If daysUntilEndOfWeek < 0 Then
            daysUntilEndOfWeek += 7
        End If
        Dim lastDayOfWeek As Date = currentDate.AddDays(daysUntilEndOfWeek)
        Return lastDayOfWeek
    End Function

    Private Sub LoadTrainings()


        Dim dt As New DataTable()

        dt = Club.GetTrainings(iniDate.SelectedDate, finDate.SelectedDate)

        Dim xquery = From a In dt.AsEnumerable
                     Select New With {.ID = a.Item("ID"), .TEAM = a.Item("TEAM"), .DATE = a.Item("DATE"), .HOUR = a.Item("HOUR"), .STADIUM = a.Item("STADIUM"), .GROUND = a.Item("GROUND"), .OBSERVATIONS = a.Item("OBSERVATIONS")
        }

        Info_Grid.DG.ItemsSource = xquery
        Info_Grid.DG.Columns.Clear()
        Info_Grid.AddColumn("ID", "ID", 30, True, System.Windows.HorizontalAlignment.Left, "INTEGER")
        Info_Grid.AddColumn("Equipo", "TEAM", 120, True, System.Windows.HorizontalAlignment.Left, "TEXT")
        Info_Grid.AddColumn("Fecha", "DATE", 120, True, System.Windows.HorizontalAlignment.Left, "DATE")
        Info_Grid.AddColumn("Hora", "HOUR", 120, True, System.Windows.HorizontalAlignment.Left, "TEXT")
        Info_Grid.AddColumn("Instalacion", "STADIUM", 120, True, System.Windows.HorizontalAlignment.Left, "TEXT")
        Info_Grid.AddColumn("Campo", "GROUND", 120, True, System.Windows.HorizontalAlignment.Left, "TEXT")
        Info_Grid.AddColumn("Observaciones", "OBSERVATIONS", 220, True, System.Windows.HorizontalAlignment.Left, "TEXT")
        Info_Grid.GridCounter()

    End Sub

    Private Sub CreateTraining(sender As Object, e As RoutedEventArgs)
        Dim w As New EditTraining

        w.ShowDialog()
        e.Handled = True

        LoadTrainings()
    End Sub

    Private Sub CleanDates(sender As Object, e As RoutedEventArgs)

        iniDate.SelectedDate = Date.Today
        finDate.SelectedDate = GetLastDayOfWeek(DateTime.Now)
        e.Handled = True

    End Sub

    Private Sub Seach(sender As Object, e As RoutedEventArgs)

        If iniDate.SelectedDate > finDate.SelectedDate Or finDate.SelectedDate < iniDate.SelectedDate Then
            MessageBox.Show("La fecha final ha de ser posterior a la inicial.", "Eliminar material", CType(MessageBoxButton.OK, MessageBoxButtons), CType(MessageBoxImage.Information, MessageBoxIcon))
            Return
        End If

        LoadTrainings()
        e.Handled = True
    End Sub

    Private Sub Seach2(sender As Object, e As Input.KeyEventArgs)
        If e.Key = Key.Return Then
            LoadTrainings()
        End If
    End Sub

    Private Sub EditTraining(sender As Object, e As RoutedEventArgs)

        Dim dg = Me.Info_Grid.DG

        If dg.SelectedItems.Count > 0 Then
            Dim drv = Me.Info_Grid.DG.SelectedItem
            Dim mat As New Training()
            mat.LoadTraining(drv.ID)
            Dim w As New EditTraining(mat)

            w.ShowDialog()
            e.Handled = True

        End If

        LoadTrainings()
    End Sub

    Private Sub DeleteTraining(sender As Object, e As RoutedEventArgs)
        Dim dg = Me.Info_Grid.DG

        If dg.SelectedItems.Count > 0 Then
            If MessageBox.Show("¿Está seguro que desea cancelar el entrenamiento?", "Cancelar entrenamiento", CType(MessageBoxButton.YesNo, MessageBoxButtons), CType(MessageBoxImage.Information, MessageBoxIcon)) = MessageBoxResult.Yes Then
                Dim drv = Me.Info_Grid.DG.SelectedItem
                Dim mat As New Training()
                mat.LoadTraining(drv.ID)
                mat.DeleteTraining()
                e.Handled = True

                LoadTrainings()
            Else

                e.Handled = True
                Exit Sub
            End If

            e.Handled = True
        Else
            MessageBox.Show("Debe seleccionar primero un material de la tabla.", "Cancelar entrenamiento", CType(MessageBoxButton.OK, MessageBoxButtons), CType(MessageBoxImage.Information, MessageBoxIcon))
        End If

        e.Handled = True
    End Sub

End Class
