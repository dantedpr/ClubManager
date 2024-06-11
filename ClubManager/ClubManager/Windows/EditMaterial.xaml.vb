Imports System.Data
Imports CefSharp.DevTools
Imports Microsoft.Win32

Public Class EditMaterial

    Public Property showPass = False
    Public Property _material As Material
    Private isMaximized As Boolean = False
    Private previousWindowState As WindowState
    Public Sub New()

        ' Esta llamada es exigida por el diseñador.
        InitializeComponent()
        _material = New Material()
        Load()
        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().

    End Sub

    Public Sub New(myMat As Material)

        ' Esta llamada es exigida por el diseñador.
        InitializeComponent()
        _material = myMat
        Load()
        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().

    End Sub

    Public Sub Load()

        AddHandler btnClose.Click, AddressOf CloseApp
        AddHandler btnMinimize.Click, AddressOf Minimize

        AddHandler BUT_Aceptar.Click, AddressOf SaveMaterial
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

        matCategory.Items.Clear()
        matCategory.Items.Add("")
        matCategory.Items.Add("Petos")
        matCategory.Items.Add("Conos")
        matCategory.Items.Add("Balones")
        matCategory.Items.Add("Porterias")
        matCategory.Items.Add("Escaleras")
        matCategory.Items.Add("Campo")
        matCategory.Items.Add("Instalacion")
        matCategory.Items.Add("Vestuario")
        matCategory.Items.Add("Otros")
        matCategory.SelectedIndex = 0

        If _material.ID <> -1 Then
            matName.Text = _material.Name
            matQty.Text = _material.Quantity
            For Each cat In matCategory.Items
                If cat.ToString() = _material.Category Then
                    matCategory.SelectedItem = cat
                    UpdateIcon2()
                End If
            Next
        End If


        AddHandler matCategory.SelectionChanged, AddressOf UpdateIcon

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
    Private Sub UpdateIcon(sender As Object, e As RoutedEventArgs)
        UpdateIcon2()
        e.Handled = True
    End Sub

    Private Sub UpdateIcon2()
        Petos.Visibility = Visibility.Collapsed
        Conos.Visibility = Visibility.Collapsed
        Balones.Visibility = Visibility.Collapsed
        Porterias.Visibility = Visibility.Collapsed
        Escaleras.Visibility = Visibility.Collapsed
        Campo.Visibility = Visibility.Collapsed
        Instalacion.Visibility = Visibility.Collapsed
        Vestuario.Visibility = Visibility.Collapsed
        Otros.Visibility = Visibility.Collapsed

        If matCategory.SelectedItem.ToString() = "Petos" Then
            Petos.Visibility = Visibility.Visible
        ElseIf matCategory.SelectedItem.ToString() = "Conos" Then
            Conos.Visibility = Visibility.Visible
        ElseIf matCategory.SelectedItem.ToString() = "Balones" Then
            Balones.Visibility = Visibility.Visible
        ElseIf matCategory.SelectedItem.ToString() = "Porterias" Then
            Porterias.Visibility = Visibility.Visible
        ElseIf matCategory.SelectedItem.ToString() = "Escaleras" Then
            Escaleras.Visibility = Visibility.Visible
        ElseIf matCategory.SelectedItem.ToString() = "Campo" Then
            Campo.Visibility = Visibility.Visible
        ElseIf matCategory.SelectedItem.ToString() = "Instalacion" Then
            Instalacion.Visibility = Visibility.Visible
        ElseIf matCategory.SelectedItem.ToString() = "Vestuario" Then
            Vestuario.Visibility = Visibility.Visible
        ElseIf matCategory.SelectedItem.ToString() = "Otros" Then
            Otros.Visibility = Visibility.Visible
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

    Private Sub SaveMaterial(sender As Object, e As RoutedEventArgs)

        If matName.Text = "" Or matCategory.SelectedItem.ToString() = "" Or matQty.Text = "" Then
            MessageBox.Show("¡Faltán datos por introducir! Por favor revisa los datos introducidos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            Return
        Else
            If _material.ID <> -1 Then
                _material.UpdateMaterial(matName.Text, matCategory.SelectedItem.ToString(), CInt(matQty.Text))
            Else
                Dim t1 = New Material(Club.ID, matName.Text, matCategory.SelectedItem.ToString(), CInt(matQty.Text))
                t1.SaveMaterial()
            End If
            MessageBox.Show("Se han guardado los datos del material.", "Material", MessageBoxButton.OK, MessageBoxImage.Information)
        End If

        e.Handled = True
        Me.Close()

    End Sub


End Class
