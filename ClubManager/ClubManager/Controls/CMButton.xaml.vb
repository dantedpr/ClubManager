Imports System.ComponentModel
Imports System.Windows.Controls
Imports System.Windows.Media
Imports System.Windows.Media.Imaging
Imports System.Windows
Imports System.Windows.Threading
Imports System.Drawing

Namespace Controls

    <System.Drawing.ToolboxBitmap(GetType(Button))>
    <ComponentModel.DefaultEvent("Click")>
    Public Class CMButton

        Private v_ImageName As String = ""
        Private v_Image As ImageSource

        Public Shared ReadOnly ClickEvent As RoutedEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, GetType(RoutedEventHandler), GetType(CMButton))

        Public Custom Event Click As RoutedEventHandler
            AddHandler(value As RoutedEventHandler)
                Me.AddHandler(ClickEvent, value)
            End AddHandler
            RemoveHandler(value As RoutedEventHandler)
                Me.RemoveHandler(ClickEvent, value)
            End RemoveHandler
            RaiseEvent(sender As Object, e As RoutedEventArgs)
                Me.RaiseEvent(e)
            End RaiseEvent
        End Event

        Private Sub CMButton_MouseUp(sender As Object, e As MouseEventArgs) Handles Me.MouseUp
            Try
                Dim newEv As New RoutedEventArgs(CMButton.ClickEvent)
                MyBase.RaiseEvent(newEv)
            Catch ex As Exception

            End Try
        End Sub

        Public Property Image As ImageSource
            Get
                Return v_Image
            End Get
            Set(value As ImageSource)
                v_Image = value
                ButtonImage.Source = value
            End Set
        End Property

        Public ReadOnly Property ButtonImageControl As Windows.Controls.Image
            Get
                Return ButtonImage
            End Get
        End Property

        Private _ShowImage As Boolean = True
        Public Property ShowImage As Boolean
            Get
                Return _ShowImage
            End Get
            Set(value As Boolean)
                _ShowImage = value
                If _ShowImage Then
                    Me.ButtonImage.Width = Me.Width
                    Me.ButtonImage.Height = Me.Height
                Else
                    Me.ButtonImage.Width = 0
                    Me.ButtonImage.Height = 0
                End If
            End Set
        End Property

        Public Property Border As Integer
            Get
                Return Me.MiBorde.BorderThickness.Bottom
            End Get
            Set(value As Integer)
                Me.MiBorde.BorderThickness = New Thickness(value)
            End Set
        End Property

        Public Property ImageName As String
            Get
                Return v_ImageName
            End Get
            Set(value As String)
                v_ImageName = value

                If v_ImageName <> "" Then
                    ' Cargar la imagen desde la carpeta "Images" del proyecto
                    Dim imagePath As String = "/Images/" & v_ImageName ' Asegúrate de ajustar la ruta según la estructura de tu proyecto
                    Dim uri As New Uri(imagePath, UriKind.RelativeOrAbsolute)
                    Dim bitmap As New BitmapImage(uri)
                    ButtonImage.Source = bitmap
                    ButtonImage.Visibility = Visibility.Visible
                Else
                    ButtonImage.Visibility = Visibility.Collapsed
                End If

            End Set
        End Property

        Public Property ButTextForeground As System.Windows.Media.Brush
            Get
                Return Me.LBL_Text.Foreground
            End Get
            Set(value As Media.Brush)
                Me.LBL_Text.Foreground = value
            End Set
        End Property

        Public Property ButText As String
            Get
                Return Me.LBL_Text.Content
            End Get
            Set(value As String)
                If value <> "" Then
                    Me.LBL_Text.Visibility = Visibility.Visible
                    Me.LBL_Text.Content = value
                    If ShowImage Then
                        Me.ButtonImage.Margin = New Thickness(3, 2, 2, 0)
                        Me.ButtonImage.Width = 20
                        Me.ButtonImage.Height = 20
                        Me.ButtonImage.Stretch = Stretch.UniformToFill
                    Else
                        Me.ButtonImage.Margin = New Thickness(0)
                        Me.ButtonImage.Width = 0
                        Me.ButtonImage.Height = 0
                        Me.ButtonImage.Stretch = Stretch.UniformToFill
                    End If
                    Me.MiBorde.Background = _GetBackground
                    Me.Border = 1
                Else
                    Me.LBL_Text.Visibility = Visibility.Collapsed
                    Me.Border = 0
                End If
            End Set
        End Property

        Public Sub New()

            InitializeComponent()

            If _ShowImage Then
                Me.ButtonImage.Width = Me.Width
                Me.ButtonImage.Height = Me.Height
            Else
                Me.ButtonImage.Width = 0
                Me.ButtonImage.Height = 0
            End If

            Me.MiBorde.Width = Me.Width
            Me.MiBorde.Height = Me.Height
            Me.Focusable = True
            Me.Border = 0

            If Not ComponentModel.DesignerProperties.GetIsInDesignMode(Me) Then
                If v_ImageName <> "" AndAlso ShowImage Then
                    ' Cargar la imagen desde la carpeta "Images" del proyecto
                    Dim imagePath As String = "/Images/" & v_ImageName ' Asegúrate de ajustar la ruta según la estructura de tu proyecto
                    Dim uri As New Uri(imagePath, UriKind.RelativeOrAbsolute)
                    Dim bitmap As New BitmapImage(uri)
                    ButtonImage.Source = bitmap

                End If
                If _GetText <> "" Then
                    Me.ButText = _GetText
                End If
                If _GetTipText <> "" Then
                    Me.ToolTip = _GetTipText
                End If
            End If


        End Sub

        Private Sub CMButton_IsEnabledChanged(sender As Object, e As DependencyPropertyChangedEventArgs) Handles Me.IsEnabledChanged
            If e.NewValue = True Then
                Me.ButtonImage.Source = v_Image
            Else
                Dim newBitSource As New FormatConvertedBitmap()
                newBitSource.BeginInit()

                If Not v_Image Is Nothing Then
                    newBitSource.Source = v_Image
                    newBitSource.DestinationFormat = PixelFormats.Gray32Float
                    newBitSource.EndInit()
                End If

                Me.ButtonImage.Source = newBitSource
            End If
        End Sub

        Private Sub KeyTouch(sender As Object, e As KeyEventArgs) Handles MyBase.PreviewKeyUp
            If e.Key = Key.Space OrElse e.Key = Key.Return Then
                Dim x As New RoutedEventArgs
                x.RoutedEvent = ClickEvent
                RaiseEvent Click(sender, x)
            End If
        End Sub

        Private _GetText As String

        Public Property GetText As String
            Get
                Return _GetText
            End Get
            Set(value As String)
                _GetText = value
                If Not ComponentModel.DesignerProperties.GetIsInDesignMode(Me) Then
                    If _GetText <> "" Then
                        Me.ButText = _GetText
                    End If
                End If
            End Set
        End Property

        Public Property _GetTipText As String
        Public Property GetTipText As String
            Get
                Return _GetTipText
            End Get
            Set(value As String)
                _GetTipText = value
                If Not ComponentModel.DesignerProperties.GetIsInDesignMode(Me) Then
                    If _GetTipText <> "" Then
                        Me.ToolTip = _GetTipText
                    End If
                End If
            End Set
        End Property

        Private _GetBackground As Media.Brush

        Public Property GetBackground As Media.Brush
            Get
                Return _GetBackground
            End Get
            Set(value As Media.Brush)
                _GetBackground = value
                MiBorde.Background = _GetBackground
            End Set
        End Property

        Private Sub ChangeStatus(sender As Object, e As MouseEventArgs) Handles MyBase.MouseEnter
            Me.MiBorde.Background = New SolidColorBrush(Colors.AliceBlue)
        End Sub

        Private Sub ChangeStatus2(sender As Object, e As MouseEventArgs) Handles MyBase.MouseLeave
            Me.MiBorde.Background = _GetBackground
        End Sub
    End Class

End Namespace
