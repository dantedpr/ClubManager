Imports System.Data
Imports Microsoft.Win32

Public Class EditTeam

    Public Property showPass = False
    Public Property _team As Team
    Private isMaximized As Boolean = False
    Private previousWindowState As WindowState
    Private imagenClub As BitmapImage
    Public Sub New()

        ' Esta llamada es exigida por el diseñador.
        InitializeComponent()
        _team = New Team()
        Load()
        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().

    End Sub

    Public Sub New(myTeam As Team)

        ' Esta llamada es exigida por el diseñador.
        InitializeComponent()
        _team = myTeam
        Load()
        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().

    End Sub

    Public Sub Load()

        AddHandler btnClose.Click, AddressOf CloseApp
        AddHandler btnMinimize.Click, AddressOf Minimize

        AddHandler BUT_Aceptar.Click, AddressOf SaveTeam
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


        AddHandler Me.Info_Grid.DG.MouseDoubleClick, AddressOf EditPlayer
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

        If _team.ID <> -1 Then
            teamName.Text = _team.Name
            teamDivision.Text = _team.Division
            teamLetter.Text = _team.Letter
            For Each cat In teamCategory.Items
                If cat.ToString() = _team.Category Then
                    teamCategory.SelectedItem = cat
                End If
            Next
        End If

        LoadPlayersClub()

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

    Public Sub LoadPlayersClub()


        Dim dt As New DataTable()

        dt = Club.GetAllPlayers("", "", _team.Name, "", True)

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

    Private Sub SaveTeam(sender As Object, e As RoutedEventArgs)

        If teamName.Text = "" Or teamCategory.SelectedItem.ToString() = "" Or teamDivision.Text = "" Or teamLetter.Text = "" Then
            MessageBox.Show("¡Faltán datos por introducir! Por favor revisa los datos introducidos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            Return
        Else
            If _team.ID <> -1 Then
                _team.UpdateTeam(teamName.Text, teamCategory.SelectedItem.ToString(), teamDivision.Text, teamLetter.Text)
            Else
                Dim t1 = New Team(Club.ID, teamName.Text, teamCategory.SelectedItem.ToString(), teamDivision.Text, teamLetter.Text)
                t1.SaveTeam()
            End If
            MessageBox.Show("Se han guardado los datos de la plantilla.", "Plantilla", MessageBoxButton.OK, MessageBoxImage.Information)
        End If

        e.Handled = True
        Me.Close()

    End Sub


End Class
