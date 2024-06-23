Imports System.Data
Imports System.Diagnostics.Eventing
Imports Microsoft.Office.Interop.Excel

Class HomeWindow

    Public Property username As String
    Private Property pass As String

    Public Sub New()

        ' Esta llamada es exigida por el diseñador.
        InitializeComponent()
        Load()
        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().

    End Sub

    Public Sub Load()

        '1 Players
        BUT_Player.GetBackground = New SolidColorBrush(Colors.White)
        BUT_Player.ImageName = "soccer-player.png"
        BUT_Player.ButText = "Jugadores"
        BUT_Player.ButtonImage.Width = 50
        BUT_Player.ButtonImage.Height = 50
        BUT_Player.Border = 0
        BUT_Player.st1.HorizontalAlignment = HorizontalAlignment.Left
        BUT_Player.LBL_Text.FontSize = 20
        BUT_Player.LBL_Text.Margin = New Thickness(15, 1, 0, 0)
        '2 Team
        BUT_Team.GetBackground = New SolidColorBrush(Colors.White)
        BUT_Team.ImageName = "team.png"
        BUT_Team.ButText = "Plantillas"
        BUT_Team.ButtonImage.Width = 50
        BUT_Team.ButtonImage.Height = 50
        BUT_Team.Border = 0
        BUT_Team.st1.HorizontalAlignment = HorizontalAlignment.Left
        BUT_Team.LBL_Text.FontSize = 20
        BUT_Team.LBL_Text.Margin = New Thickness(15, 1, 0, 0)
        '3 Material
        BUT_Material.GetBackground = New SolidColorBrush(Colors.White)
        BUT_Material.ImageName = "football.png"
        BUT_Material.ButText = "Material"
        BUT_Material.ButtonImage.Width = 50
        BUT_Material.ButtonImage.Height = 50
        BUT_Material.Border = 0
        BUT_Material.st1.HorizontalAlignment = HorizontalAlignment.Left
        BUT_Material.LBL_Text.FontSize = 20
        BUT_Material.LBL_Text.Margin = New Thickness(15, 1, 0, 0)
        '4 Training
        BUT_Training.GetBackground = New SolidColorBrush(Colors.White)
        BUT_Training.ImageName = "formation.png"
        BUT_Training.ButText = "Entramientos"
        BUT_Training.ButtonImage.Width = 50
        BUT_Training.ButtonImage.Height = 50
        BUT_Training.Border = 0
        BUT_Training.st1.HorizontalAlignment = HorizontalAlignment.Left
        BUT_Training.LBL_Text.FontSize = 20
        BUT_Training.LBL_Text.Margin = New Thickness(15, 1, 0, 0)
        '5 MyClub
        BUT_Club.GetBackground = New SolidColorBrush(Colors.White)
        BUT_Club.ImageName = "football-club.png"
        BUT_Club.ButText = Club.Name
        BUT_Club.ButtonImage.Width = 50
        BUT_Club.ButtonImage.Height = 50
        BUT_Club.Border = 0
        BUT_Club.st1.HorizontalAlignment = HorizontalAlignment.Left
        BUT_Club.LBL_Text.FontSize = 20
        BUT_Club.LBL_Text.Margin = New Thickness(15, 1, 0, 0)

        AddHandler BUT_Club.Click, AddressOf EditMyClub
        AddHandler BUT_Player.Click, AddressOf Players
        AddHandler BUT_Team.Click, AddressOf Teams
        AddHandler BUT_Material.Click, AddressOf Materials
        AddHandler BUT_Training.Click, AddressOf Trainings


        Dim installations = Club.GetInstallations()

        clubInstallation.Items.Clear()
        clubInstallation.Items.Add("")
        clubInstallation.SelectedIndex = 0

        For Each ins In installations
            clubInstallation.Items.Add(ins)
            clubInstallation.SelectedIndex = 1
        Next

        AddHandler clubInstallation.SelectionChanged, AddressOf SearchTrainings
        UpdateGrid()

    End Sub

    Public Sub EditMyClub()

        Dim w = FrameWindow.Instance
        w.CleanWindow()
        Dim w1 As New EditClub
        w.Content.Children.Add(w1)

        w1.Load()
    End Sub
    Private Sub SearchTrainings(sender As Object, e As RoutedEventArgs)
        UpdateGrid()
        e.Handled = True
    End Sub
    Public Sub Players()

        Dim w = FrameWindow.Instance
        w.CleanWindow()
        Dim w1 As New PlayersWindow
        w.Content.Children.Add(w1)

        w1.Load()

    End Sub
    Public Sub Teams()

        Dim w = FrameWindow.Instance
        w.CleanWindow()
        Dim w1 As New TeamsWindow
        w.Content.Children.Add(w1)

        w1.Load()

    End Sub

    Public Sub Materials()

        Dim w = FrameWindow.Instance
        w.CleanWindow()
        Dim w1 As New MaterialsWindow
        w.Content.Children.Add(w1)

        w1.Load()

    End Sub

    Public Sub Trainings()

        Dim w = FrameWindow.Instance
        w.CleanWindow()
        Dim w1 As New TrainingsWindow
        w.Content.Children.Add(w1)

        w1.Load()

    End Sub


    Public Sub UpdateGrid()


        LabelVersion1.Content = ""
        LabelVersion2.Content = ""
        LabelVersion3.Content = ""
        LabelVersion4.Content = ""

        If clubInstallation.SelectedItem.ToString() <> "" Then
            Dim dt = Club.GetNextTrainings(clubInstallation.SelectedItem.ToString())

            Dim xquery = From a In dt.AsEnumerable
                         Select New With {.ID = a.Item("ID"), .TEAM = a.Item("TEAM"), .DATE = a.Item("DATE"), .HOUR = a.Item("HOUR"), .STADIUM = a.Item("STADIUM"), .GROUND = a.Item("GROUND"), .OBSERVATIONS = a.Item("OBSERVATIONS")
            }
            For Each training In xquery

                Dim timeValue As TimeSpan = training.HOUR
                ' Format the time value as HH:mm string
                Dim Hour = timeValue.ToString("hh\:mm")

                If training.GROUND = "Campo 1" Then
                    LabelVersion1.Content = training.TEAM & " - " & training.DATE & " " & Hour
                End If
                If training.GROUND = "Campo 2" Then
                    LabelVersion2.Content = training.TEAM & " - " & training.DATE & " " & Hour
                End If
                If training.GROUND = "Campo 3" Then
                    LabelVersion3.Content = training.TEAM & " - " & training.DATE & " " & Hour
                End If
                If training.GROUND = "Campo 4" Then
                    LabelVersion4.Content = training.TEAM & " - " & training.DATE & " " & Hour
                End If

            Next
        End If

    End Sub

End Class
