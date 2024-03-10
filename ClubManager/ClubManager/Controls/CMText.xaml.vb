Imports System.Windows.Controls
Imports System.Windows.Threading

Namespace Controls
    Public Class CMText

        Private _onlyNumeric As Boolean = False
        Private _hasWarning As Boolean = False

        Public Property AllowDecimals As Boolean = False
        Public Property DecimalsMark As String = ","
        Public Property AllowNegatives As Boolean = False
        Public Property AllowEmpty As Boolean = True
        Public Property SelectAllOnFocus As Boolean = False


        Public Property OnlyNumeric As Boolean
            Get
                Return _onlyNumeric
            End Get
            Set(value As Boolean)
                _onlyNumeric = value
                If value Then
                    Me.TextControl.TextAlignment = TextAlignment.Right
                End If
            End Set
        End Property

        Public Property TextControl As TextBox
            Get
                Return MiTextBox
            End Get
            Set(value As TextBox)
                MiTextBox = value
            End Set
        End Property

        Public Sub New()

            ' Esta llamada es exigida por el diseñador.
            InitializeComponent()

            AddHandler Me.TextControl.GotFocus, AddressOf SelectAllText

        End Sub

        Public Sub SelectAllText()
            If Me.SelectAllOnFocus Then
                Dispatcher.BeginInvoke(DispatcherPriority.ContextIdle, New Action(Sub() Me.TextControl.SelectAll()))
            End If
        End Sub

        Public Property IsReadOnly As Boolean
            Get
                Return MiTextBox.IsReadOnly
            End Get
            Set(value As Boolean)
                MiTextBox.IsReadOnly = value
                If value = False Then
                    Me.BorderThickness = New Thickness(1)
                    MiTextBox.Background = New SolidColorBrush(Colors.White)
                Else
                    Me.BorderThickness = New Thickness(0)
                    MiTextBox.Background = New SolidColorBrush(Color.FromRgb(210, 213, 214)) 'GRAY
                End If
            End Set
        End Property


        Public Property HasWarning As Boolean
            Get
                Return _hasWarning
            End Get
            Set(value As Boolean)
                _hasWarning = value
                If value = False Then
                    Me.BorderThickness = New Thickness(1)
                    MiTextBox.Background = New SolidColorBrush(Colors.White)
                Else
                    Me.BorderThickness = New Thickness(0)
                    MiTextBox.Background = New SolidColorBrush(Color.FromRgb(27, 126, 159))
                End If
            End Set
        End Property

        Private Sub FieldGotFocus(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Me.GotFocus
            Me.HasWarning = False
        End Sub

        Public Property MaxLength As Integer
            Get
                Return MiTextBox.MaxLength
            End Get
            Set(value As Integer)
                MiTextBox.MaxLength = value
            End Set
        End Property

        Private _TextBrush As Brush = Nothing
        Private _Texto As String = ""
        Private _TextIfEmpty As String
        Private _BrushIfEmpty As Brush = Brushes.Gray
        Private _FontStyle As FontStyle = Nothing

        Public Property BrushIfEmpty As Brush
            Get
                Return _BrushIfEmpty
            End Get
            Set(value As Brush)
                _BrushIfEmpty = value
            End Set
        End Property

        Public Property TextIfEmpty As String
            Get
                Return _TextIfEmpty
            End Get
            Set(value As String)
                _TextBrush = MiTextBox.Foreground
                _FontStyle = MiTextBox.FontStyle
                _TextIfEmpty = False
                If _Texto = "" Then
                    MiTextBox.Text = value
                    MiTextBox.Foreground = _BrushIfEmpty
                    MiTextBox.FontStyle = FontStyles.Italic
                End If
            End Set
        End Property

        Public Property Text As String
            Get
                Return _Texto
            End Get
            Set(value As String)
                If MiCheck(value) Then
                    _Texto = value
                    MiTextBox.Text = value
                    If _Texto = "" And Me.TextIfEmpty <> "" Then
                        MiTextBox.Text = TextIfEmpty
                        MiTextBox.Foreground = _BrushIfEmpty
                        MiTextBox.FontStyle = FontStyles.Italic
                    ElseIf _Texto <> "" And Me.TextIfEmpty <> "" Then
                        MiTextBox.Foreground = _TextBrush
                        MiTextBox.FontStyle = _FontStyle
                        MiTextBox.Text = _Texto
                    End If
                End If

            End Set
        End Property

        Private Function MiCheck(s As String) As Boolean
            If s = "" And AllowEmpty Then
                Return True
            ElseIf s = "" And Not AllowEmpty Then
                Return False
            End If
            If Not OnlyNumeric Then
                Return True
            End If
            If Not IsNumeric(s) Then
                Return False
            End If
            If (DecimalsMark = "," And InStr(s, ".") > 0) Then
                Replace(s, ".", ",")
            End If
            If (DecimalsMark = "." And InStr(s, ",") > 0) Then
                Replace(s, ",", ".")
            End If
            If Not AllowDecimals And InStr(s, DecimalsMark) > 0 Then
                Return False
            End If
            If (DecimalsMark = "," And InStr(s, ".") > 0) Or (DecimalsMark = "." And InStr(s, ",") > 0) Then
                Return False
            End If

            If Not AllowDecimals Then
                Try
                    Dim n As Long = CLng(s)
                Catch ex As Exception
                    Return False
                End Try
            Else
                Try
                    Dim n As Long = CDec(s)
                Catch ex As Exception
                    Return False
                End Try
            End If

            If Not AllowNegatives And Left(s, 1) = "-" Then
                Return False
            End If
            Return True
        End Function

        Public Property AcceptsReturn As Boolean
            Get
                Return MiTextBox.AcceptsReturn
            End Get
            Set(value As Boolean)
                If value = True Then
                    MiTextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Auto
                    MiTextBox.TextWrapping = TextWrapping.Wrap
                    MiTextBox.AcceptsReturn = True
                    MiTextBox.VerticalContentAlignment = VerticalAlignment.Top
                Else
                    MiTextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Auto
                    MiTextBox.TextWrapping = TextWrapping.NoWrap
                    MiTextBox.AcceptsReturn = False
                    MiTextBox.VerticalContentAlignment = VerticalAlignment.Center
                End If
            End Set
        End Property

        Private Sub MiTextBox_PreviewGotKeyboardFocus(sender As Object, e As KeyboardFocusChangedEventArgs) Handles MiTextBox.PreviewGotKeyboardFocus
            If Me.TextIfEmpty <> "" Then
                MiTextBox.Text = _Texto
                If _TextBrush IsNot Nothing Then
                    MiTextBox.Foreground = _TextBrush
                End If
                MiTextBox.FontStyle = _FontStyle
            End If
        End Sub

        Private Sub MiTextBox_PreviewLoseKeyboardFocus(sender As Object, e As KeyboardFocusChangedEventArgs) Handles MiTextBox.PreviewLostKeyboardFocus
            If Me.TextIfEmpty <> "" And _Texto = "" Then
                MiTextBox.Text = TextIfEmpty
                MiTextBox.Foreground = _BrushIfEmpty
                MiTextBox.FontStyle = FontStyles.Italic
            ElseIf _Texto <> "" And Me.TextIfEmpty <> "" Then
                _TextBrush = MiTextBox.Foreground
                _FontStyle = MiTextBox.FontStyle
                MiTextBox.Text = _Texto
            End If

            If Not MiCheck(CType(e.Source, TextBox).Text) Then
                CType(e.Source, TextBox).Focus()
                e.Handled = True
            End If
        End Sub

        Public Event textchanged(sender As Object, e As KeyEventArgs)

        Private Sub MitextBox_pulsar2(sender As Object, e As KeyEventArgs) Handles MiTextBox.KeyUp
            _Texto = MiTextBox.Text
            RaiseEvent textchanged(sender, e)
        End Sub

        Private Sub MitextBox_pulsar(sender As Object, e As KeyEventArgs) Handles MiTextBox.KeyDown
            e.Handled = False
            _Texto = MiTextBox.Text
            If OnlyNumeric Then
                If Not ((e.Key >= Key.D0 And e.Key <= Key.D9) Or (e.Key >= Key.NumPad0 And e.Key <= Key.NumPad9) Or e.Key = Key.Decimal Or e.Key = Key.OemPeriod Or e.Key = Key.OemComma Or e.Key = 88 Or e.Key = Key.Tab Or e.Key = 143 Or e.Key = Key.Subtract Or e.Key = Key.Back) Then
                    e.Handled = True
                ElseIf e.Key = Key.Decimal Or e.Key = Key.OemPeriod Or e.Key = Key.OemComma Then
                    MiTextBox.Text = MiTextBox.Text & Me.DecimalsMark
                    _Texto = MiTextBox.Text
                    MiTextBox.CaretIndex = MiTextBox.Text.Length
                    e.Handled = True
                End If
            End If
        End Sub

    End Class

End Namespace
