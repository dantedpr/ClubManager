Imports System.Data

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


        LoadPlayersClub()

    End Sub

    Public Sub LoadPlayersClub()


        Dim dt As New DataTable()

        dt = Club.GetAllTeams(0, "", "")

        Dim xquery = From a In dt.AsEnumerable
                     Select New With {.ID = a.Item("ID"), .NAME = a.Item("NAME"), .LASTNAME1 = a.Item("LASTNAME1"), .LASTNAME2 = a.Item("LASTNAME2"), .MAIL = a.Item("MAIL"), .PHONE = a.Item("PHONE"), .ADDRESS = a.Item("ADDRESS")
        }

        Info_Grid.DG.ItemsSource = xquery
        Info_Grid.DG.Columns.Clear()
        Info_Grid.AddColumn("ID", "ID", 50, True, System.Windows.HorizontalAlignment.Left, "INTEGER")
        Info_Grid.AddColumn("NAME", "NAME", 100, True, System.Windows.HorizontalAlignment.Left, "TEXT")
        Info_Grid.GridCounter()

    End Sub

    Public Sub EditMyClub()

        Dim w = FrameWindow.Instance
        w.CleanWindow()
        Dim w1 As New EditClub
        w.Content.Children.Add(w1)

        w1.Load()
    End Sub

    Public Sub CreateTeam()

    End Sub

    Public Sub RegisterAccount()


    End Sub

End Class
