Imports System.Timers
Imports System.Windows.Threading

Public Class FrameWindow

    Inherits Window

    Public Property username As String
    Private Property pass As String

    Dim WithEvents Timer1 As DispatcherTimer

    Private previousState As WindowState

    Private Shared _instance As FrameWindow

    ' Método público para obtener la instancia
    Public Shared ReadOnly Property Instance As FrameWindow
        Get
            If _instance Is Nothing Then
                _instance = New FrameWindow()
            End If
            Return _instance
        End Get
    End Property

    Public Sub New()

        ' Esta llamada es exigida por el diseñador.
        InitializeComponent()
        Timer1 = New DispatcherTimer()
        Timer1.Interval = New TimeSpan(0, 0, 1)
        Dim dateHour = Date.Now.ToString("dddd  dd MMM yyyy HH:mm")
        Dim uppercased As String = dateHour.Substring(0, 1).ToUpper() + dateHour.Substring(1)
        LabelTime.Content = uppercased
        Timer1.Start()
        Load()

        'BUT_Home.GetBackground = Nothing
        'BUT_Home.ImageName = "home2.png"
        'BUT_Home.ButText = ""
        'BUT_Home.ButtonImage.Width = 60
        'BUT_Home.ButtonImage.Height = 60
        'BUT_Home.Border = 0
        'BUT_Home.st1.HorizontalAlignment = HorizontalAlignment.Left
        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().

    End Sub
    Public Sub Load()

        AddHandler btnClose.Click, AddressOf CloseApp
        LabelClub.Content = Club.Name

        'Dim imageBytes As Byte() = ImageManager.Base64StringToByteArray(Club.logoClub)

        '' Convertir el byte array a un BitmapImage
        'logoClub.Source = ImageManager.ByteArrayToImage(imageBytes)
    End Sub


    Public Overloads Sub CloseApp()
        System.Windows.Application.Current.Shutdown()
    End Sub

    Public Overloads Sub Expand()

        If Me.WindowState = WindowState.Maximized Then
            ' If window is maximized, restore to the previous state
            Me.WindowState = previousState
        Else
            ' If window is not maximized, maximize it and store the previous state
            previousState = Me.WindowState
            Me.WindowState = WindowState.Maximized
        End If

    End Sub

    Public Overloads Sub GoHome()
        Dim w = FrameWindow.Instance
        w.CleanWindow()
        Dim w1 As New HomeWindow
        w.Content.Children.Add(w1)
        w1.Load()
    End Sub

    Public Overloads Sub Minimize()
        ' Minimize the window
        Me.WindowState = WindowState.Minimized
    End Sub
    Public Sub AllowMovement(sender As Object, e As MouseEventArgs)
        If e.LeftButton = MouseButtonState.Pressed Then
            DragMove()
        End If
    End Sub

    Public Sub RegisterAccount()

        Dim w As New ClubRegistration

        w.Show()

        Me.Close()

    End Sub

    Private Sub Timer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Dim dateHour = Date.Now.ToString("dddd  dd MMM yyyy HH:mm")
        Dim uppercased As String = dateHour.Substring(0, 1).ToUpper() + dateHour.Substring(1)
        LabelTime.Content = uppercased
    End Sub

    Public Sub CleanWindow()
        Dim li As New List(Of Object)
        For Each obj In Me.Content.Children
            li.Add(obj)
        Next

        Me.Content.Children.Clear()
        For i = 1 To li.Count
            Dim ob = li(0)
            li.RemoveAt(0)
            Try
                TryCast(ob, IDisposable).Dispose()
            Catch ex As Exception

            End Try
        Next

        GC.Collect()
        GC.WaitForPendingFinalizers()
        GC.Collect()
    End Sub

End Class
