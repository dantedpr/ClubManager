Imports System.Data
Imports System.Windows.Forms

Class TeamsWindow

    Public Property username As String
    Private Property pass As String

    Public Sub New()

        ' Esta llamada es exigida por el diseñador.
        InitializeComponent()
        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().

    End Sub

    Public Sub Load()

        AddHandler BUT_NewTeam.Click, AddressOf CreateTeam
        BUT_NewTeam.Background = New SolidColorBrush(Colors.White)
        'BUT_Aceptar.GetBackground = New SolidColorBrush(System.Windows.Media.Color.FromRgb(210, 213, 214))
        BUT_NewTeam.GetBackground = New SolidColorBrush(Colors.White)
        BUT_NewTeam.ImageName = "plus.png"
        BUT_NewTeam.ButText = "Añadir plantilla"

        AddHandler BUT_Delete.Click, AddressOf DeleteTeam
        BUT_Delete.Background = New SolidColorBrush(Colors.White)
        BUT_Delete.GetBackground = New SolidColorBrush(Colors.WhiteSmoke)
        BUT_Delete.ImageName = "delete.png"
        BUT_Delete.ButText = "Eliminar plantilla"

        AddHandler BUT_Search.Click, AddressOf Seach
        BUT_Search.Background = New SolidColorBrush(Colors.White)
        BUT_Search.GetBackground = New SolidColorBrush(Colors.White)
        BUT_Search.ImageName = "transparency.png"
        BUT_Search.ButText = "Buscar"

        AddHandler Me.Info_Grid.DG.MouseDoubleClick, AddressOf EditTeam

        teamCategory.Items.Clear()
        teamCategory.Items.Add("")
        teamCategory.Items.Add("Promesas")
        teamCategory.Items.Add("Prebenjamín")
        teamCategory.Items.Add("Benjamín")
        teamCategory.Items.Add("Alevín")
        teamCategory.Items.Add("Infantil")
        teamCategory.Items.Add("Cadete")
        teamCategory.Items.Add("Juvenil")
        teamCategory.Items.Add("Amateur")
        teamCategory.SelectedIndex = 0
        AddHandler teamCategory.SelectionChanged, AddressOf Seach
        AddHandler teamName.PreviewKeyDown, AddressOf Seach2
        LoadTeams()

    End Sub

    Private Sub LoadTeams()


        Dim dt As New DataTable()

        dt = Club.GetAllTeams(teamName.Text, teamCategory.SelectedItem.ToString())

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

    Private Sub CreateTeam(sender As Object, e As RoutedEventArgs)
        Dim w As New EditTeam

        w.ShowDialog()
        e.Handled = True

        LoadTeams()
    End Sub

    Private Sub Seach(sender As Object, e As RoutedEventArgs)
        LoadTeams()
        e.Handled = True
    End Sub

    Private Sub Seach2(sender As Object, e As Input.KeyEventArgs)
        If e.Key = Key.Return Then
            LoadTeams()
        End If
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

    Private Sub DeleteTeam(sender As Object, e As RoutedEventArgs)
        Dim dg = Me.Info_Grid.DG

        If dg.SelectedItems.Count > 0 Then
            If MessageBox.Show("¿Está seguro que desea eliminar la plantilla?", "Eliminar plantilla", CType(MessageBoxButton.YesNo, MessageBoxButtons), CType(MessageBoxImage.Information, MessageBoxIcon)) = MessageBoxResult.Yes Then
                Dim drv = Me.Info_Grid.DG.SelectedItem
                Dim team As New Team()
                team.LoadTeam(drv.ID)
                team.DeleteTeam()
                e.Handled = True

                LoadTeams()
            Else

                e.Handled = True
                Exit Sub
            End If

            e.Handled = True
        Else
            MessageBox.Show("Debe seleccionar primero una plantilla de la tabla.", "Eliminar plantilla", CType(MessageBoxButton.OK, MessageBoxButtons), CType(MessageBoxImage.Information, MessageBoxIcon))
        End If

        e.Handled = True
    End Sub

End Class
