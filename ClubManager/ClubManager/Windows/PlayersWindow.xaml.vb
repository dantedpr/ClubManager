Imports System.Data
Imports System.Windows.Forms

Class PlayersWindow

    Public Property username As String
    Private Property pass As String

    Public Sub New()

        ' Esta llamada es exigida por el diseñador.
        InitializeComponent()
        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().

    End Sub

    Public Sub Load()

        AddHandler BUT_NewPlayer.Click, AddressOf CreatePlayer
        BUT_NewPlayer.Background = New SolidColorBrush(Colors.White)
        'BUT_Aceptar.GetBackground = New SolidColorBrush(System.Windows.Media.Color.FromRgb(210, 213, 214))
        BUT_NewPlayer.GetBackground = New SolidColorBrush(Colors.White)
        BUT_NewPlayer.ImageName = "plus.png"
        BUT_NewPlayer.ButText = "Añadir jugador"

        AddHandler BUT_Delete.Click, AddressOf DeletePlayer
        BUT_Delete.Background = New SolidColorBrush(Colors.White)
        BUT_Delete.GetBackground = New SolidColorBrush(Colors.WhiteSmoke)
        BUT_Delete.ImageName = "delete.png"
        BUT_Delete.ButText = "Eliminar jugador"

        AddHandler BUT_Search.Click, AddressOf Search
        BUT_Search.Background = New SolidColorBrush(Colors.White)
        BUT_Search.GetBackground = New SolidColorBrush(Colors.White)
        BUT_Search.ImageName = "transparency.png"
        BUT_Search.ButText = "Buscar"

        AddHandler Me.Info_Grid.DG.MouseDoubleClick, AddressOf EditPlayer


        playerCategory.Items.Clear()
        playerCategory.Items.Add("")
        playerCategory.Items.Add("Promesas")
        playerCategory.Items.Add("Prebenjamín")
        playerCategory.Items.Add("Benjamín")
        playerCategory.Items.Add("Alevín")
        playerCategory.Items.Add("Infantil")
        playerCategory.Items.Add("Cadete")
        playerCategory.Items.Add("Juvenil")
        playerCategory.Items.Add("Amateur")
        playerCategory.SelectedIndex = 0
        AddHandler playerCategory.SelectionChanged, AddressOf Search
        AddHandler playerName.PreviewKeyDown, AddressOf Search2
        AddHandler playerAge.PreviewKeyDown, AddressOf Search2
        AddHandler playerStatus.Click, AddressOf Search


        Dim li = Club.GetTeamsName()
        playerTeam.Items.Clear()
        playerTeam.Items.Add("")
        For Each x In li
            playerTeam.Items.Add(x)
        Next
        playerTeam.SelectedIndex = 0

        AddHandler playerTeam.SelectionChanged, AddressOf Search
        LoadPlayersClub()

    End Sub

    Public Sub LoadPlayersClub()


        Dim dt As New DataTable()

        dt = Club.GetAllPlayers(playerAge.Text, playerName.Text, playerTeam.SelectedItem.ToString(), playerCategory.SelectedItem.ToString(),
                                playerStatus.IsChecked)

        Dim xquery = From a In dt.AsEnumerable
                     Select New With {.ID = a.Item("ID"), .NAME = a.Item("NAME"), .FULLNAME = a.Item("FULLNAME"), .AGE = a.Item("AGE"), .MAIL = a.Item("MAIL"), .PHONE = a.Item("PHONE"),
                                        .ADDRESS = a.Item("ADDRESS"), .TEAMNAME = a.Item("TEAMNAME"), .TEAMCATEGORY = a.Item("TEAMCATEGORY"), .STATUS = a.Item("STATUS")
        }


        Info_Grid.DG.ItemsSource = xquery
        Info_Grid.DG.Columns.Clear()
        Info_Grid.AddColumn("ID", "ID", 50, True, System.Windows.HorizontalAlignment.Left, "INTEGER")
        Info_Grid.AddColumn("Nombre", "NAME", 100, True, System.Windows.HorizontalAlignment.Left, "TEXT")
        Info_Grid.AddColumn("Apellidos", "FULLNAME", 100, True, System.Windows.HorizontalAlignment.Left, "TEXT")
        Info_Grid.AddColumn("Edad", "AGE", 50, True, System.Windows.HorizontalAlignment.Left, "INTEGER")
        Info_Grid.AddColumn("Mail", "MAIL", 150, True, System.Windows.HorizontalAlignment.Left, "TEXT")
        Info_Grid.AddColumn("Teléfono", "PHONE", 100, True, System.Windows.HorizontalAlignment.Left, "TEXT")
        Info_Grid.AddColumn("Dirección", "ADDRESS", 100, True, System.Windows.HorizontalAlignment.Left, "TEXT")
        Info_Grid.AddColumn("Equipo", "TEAMNAME", 100, True, System.Windows.HorizontalAlignment.Left, "TEXT")
        Info_Grid.AddColumn("Categoría", "TEAMCATEGORY", 100, True, System.Windows.HorizontalAlignment.Left, "TEXT")
        Info_Grid.AddColumn("Estado", "STATUS", 100, True, System.Windows.HorizontalAlignment.Left, "TEXT")
        Info_Grid.GridCounter()

    End Sub

    Private Sub CreatePlayer(sender As Object, e As RoutedEventArgs)
        Dim w As New EditPlayer
        w.Load()
        w.ShowDialog()

        e.Handled = True

        LoadPlayersClub()
    End Sub

    Private Sub Search(sender As Object, e As RoutedEventArgs)
        LoadPlayersClub()
        e.Handled = True
    End Sub

    Private Sub Search2(sender As Object, e As Input.KeyEventArgs)
        If e.Key = Key.Return Then
            LoadPlayersClub()
        End If
    End Sub

    Private Sub EditPlayer(sender As Object, e As RoutedEventArgs)

        Dim dg = Me.Info_Grid.DG

        If dg.SelectedItems.Count > 0 Then
            Dim drv = Me.Info_Grid.DG.SelectedItem
            Dim myplayer As New Player()
            myplayer.LoadPlayer(drv.ID)
            Dim w As New EditPlayer(myplayer)

            w.ShowDialog()
            e.Handled = True

        End If

        LoadPlayersClub()
    End Sub

    Private Sub DeletePlayer(sender As Object, e As RoutedEventArgs)
        Dim dg = Me.Info_Grid.DG

        If dg.SelectedItems.Count > 0 Then
            If MessageBox.Show("¿Está seguro que desea eliminar el jugador seleccionado?", "Eliminar jugador", CType(MessageBoxButton.YesNo, MessageBoxButtons), CType(MessageBoxImage.Information, MessageBoxIcon)) = MessageBoxResult.Yes Then
                Dim drv = Me.Info_Grid.DG.SelectedItem
                Dim player As New Player()
                player.LoadPlayer(drv.ID)
                player.DeletePlayer()
                e.Handled = True

                LoadPlayersClub()
            Else

                e.Handled = True
                Exit Sub
            End If

            e.Handled = True
        Else
            MessageBox.Show("Debe seleccionar primero un jugador de la tabla.", "Eliminar jugador", CType(MessageBoxButton.OK, MessageBoxButtons), CType(MessageBoxImage.Information, MessageBoxIcon))
        End If

        e.Handled = True
    End Sub

End Class
