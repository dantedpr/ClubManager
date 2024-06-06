Imports System.Data
Imports System.Windows.Forms

Class TeamsWindow

    Public Property username As String
    Private Property pass As String

    Public Sub New()

        ' Esta llamada es exigida por el diseñador.
        InitializeComponent()
        Load()
        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().

    End Sub

    Public Sub Load()

        AddHandler BUT_NewTeam.Click, AddressOf CreateTeam
        BUT_NewTeam.Background = New SolidColorBrush(Colors.White)
        'BUT_Aceptar.GetBackground = New SolidColorBrush(System.Windows.Media.Color.FromRgb(210, 213, 214))
        BUT_NewTeam.GetBackground = New SolidColorBrush(Colors.White)
        BUT_NewTeam.ImageName = "plus.png"
        BUT_NewTeam.ButText = "Añadir plantilla"

        AddHandler Me.Info_Grid.DG.MouseDoubleClick, AddressOf EditTeam

        LoadTeams()

    End Sub

    Public Sub LoadTeams()


        Dim dt As New DataTable()

        dt = Club.GetAllTeams(0, "", "")

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

End Class
