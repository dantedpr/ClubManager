﻿Imports System.Data

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

        Dim dt As New DataTable()

        dt = Club.GetAllClubs()

        Dim xquery = From a In dt.AsEnumerable
                     Select New With {.ID = a.Item("ID"), .NAME = a.Item("NAME")}


        Info_Grid.MyDataGrid.ItemsSource = xquery
        Info_Grid.MyDataGrid.Columns.Clear()
        Info_Grid.AddColumn("ID", "ID", 70, True, System.Windows.HorizontalAlignment.Left)
        Info_Grid.AddColumn("NAME", "NAME", 70, True, System.Windows.HorizontalAlignment.Left)
        Info_Grid.GridCounter()

    End Sub


    Public Sub RegisterAccount()


    End Sub

End Class