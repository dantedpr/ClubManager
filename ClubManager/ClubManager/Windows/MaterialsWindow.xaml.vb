Imports System.Data
Imports System.Windows.Forms

Class MaterialsWindow

    Public Property username As String
    Private Property pass As String

    Public Sub New()

        ' Esta llamada es exigida por el diseñador.
        InitializeComponent()
        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().

    End Sub

    Public Sub Load()

        AddHandler BUT_NewTeam.Click, AddressOf CreateMaterial
        BUT_NewTeam.Background = New SolidColorBrush(Colors.White)
        'BUT_Aceptar.GetBackground = New SolidColorBrush(System.Windows.Media.Color.FromRgb(210, 213, 214))
        BUT_NewTeam.GetBackground = New SolidColorBrush(Colors.White)
        BUT_NewTeam.ImageName = "plus.png"
        BUT_NewTeam.ButText = "Añadir material"

        AddHandler BUT_Delete.Click, AddressOf DeleteMaterial
        BUT_Delete.Background = New SolidColorBrush(Colors.White)
        BUT_Delete.GetBackground = New SolidColorBrush(Colors.WhiteSmoke)
        BUT_Delete.ImageName = "delete.png"
        BUT_Delete.ButText = "Eliminar material"

        AddHandler BUT_Search.Click, AddressOf Seach
        BUT_Search.Background = New SolidColorBrush(Colors.White)
        BUT_Search.GetBackground = New SolidColorBrush(Colors.White)
        BUT_Search.ImageName = "transparency.png"
        BUT_Search.ButText = "Buscar"

        AddHandler Me.Info_Grid.DG.MouseDoubleClick, AddressOf EditMaterial

        matType.Items.Clear()
        matType.Items.Add("")
        matType.Items.Add("Petos")
        matType.Items.Add("Conos")
        matType.Items.Add("Balones")
        matType.Items.Add("Porterias")
        matType.Items.Add("Escaleras")
        matType.Items.Add("Campo")
        matType.Items.Add("Instalacion")
        matType.Items.Add("Vestuario")
        matType.Items.Add("Otros")
        matType.SelectedIndex = 0
        AddHandler matType.SelectionChanged, AddressOf Seach
        AddHandler matName.PreviewKeyDown, AddressOf Seach2
        LoadMaterials()

    End Sub

    Private Sub LoadMaterials()


        Dim dt As New DataTable()

        'dt = Club.GetAllMaterials(matName.Text, matType.SelectedItem.ToString())

        Dim xquery = From a In dt.AsEnumerable
                     Select New With {.ID = a.Item("ID"), .NAME = a.Item("NAME"), .CATEGORY = a.Item("CATEGORY"), .QUANTITY = a.Item("QUANTITY")
        }

        Info_Grid.DG.ItemsSource = xquery
        Info_Grid.DG.Columns.Clear()
        Info_Grid.AddColumn("ID", "ID", 50, True, System.Windows.HorizontalAlignment.Left, "INTEGER")
        Info_Grid.AddColumn("Nombre", "NAME", 200, True, System.Windows.HorizontalAlignment.Left, "TEXT")
        Info_Grid.AddColumn("Categoría", "CATEGORY", 200, True, System.Windows.HorizontalAlignment.Left, "TEXT")
        Info_Grid.AddColumn("Cantidad", "QUANTITY", 200, True, System.Windows.HorizontalAlignment.Left, "TEXT")
        Info_Grid.GridCounter()

    End Sub

    Private Sub CreateMaterial(sender As Object, e As RoutedEventArgs)
        Dim w As New EditMaterial

        w.ShowDialog()
        e.Handled = True

        LoadMaterials()
    End Sub

    Private Sub Seach(sender As Object, e As RoutedEventArgs)
        LoadMaterials()
        e.Handled = True
    End Sub

    Private Sub Seach2(sender As Object, e As Input.KeyEventArgs)
        If e.Key = Key.Return Then
            LoadMaterials()
        End If
    End Sub

    Private Sub EditMaterial(sender As Object, e As RoutedEventArgs)

        Dim dg = Me.Info_Grid.DG

        If dg.SelectedItems.Count > 0 Then
            Dim drv = Me.Info_Grid.DG.SelectedItem
            Dim team As New Team()
            team.LoadTeam(drv.ID)
            Dim w As New EditTeam(team)

            w.ShowDialog()
            e.Handled = True

        End If

        LoadMaterials()
    End Sub

    Private Sub DeleteMaterial(sender As Object, e As RoutedEventArgs)
        Dim dg = Me.Info_Grid.DG

        If dg.SelectedItems.Count > 0 Then
            If MessageBox.Show("¿Está seguro que desea eliminar el material?", "Eliminar material", CType(MessageBoxButton.YesNo, MessageBoxButtons), CType(MessageBoxImage.Information, MessageBoxIcon)) = MessageBoxResult.Yes Then
                Dim drv = Me.Info_Grid.DG.SelectedItem
                Dim team As New Team()
                team.LoadTeam(drv.ID)
                team.DeleteTeam()
                e.Handled = True

                LoadMaterials()
            Else

                e.Handled = True
                Exit Sub
            End If

            e.Handled = True
        Else
            MessageBox.Show("Debe seleccionar primero un material de la tabla.", "Eliminar material", CType(MessageBoxButton.OK, MessageBoxButtons), CType(MessageBoxImage.Information, MessageBoxIcon))
        End If

        e.Handled = True
    End Sub

End Class
