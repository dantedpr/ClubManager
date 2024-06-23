Imports System.Data
Imports CefSharp.DevTools
Imports Microsoft.Win32
Imports System.Net
Imports System.Net.Mail

Public Class RecoverPass

    Public Property showPass = False
    Private isMaximized As Boolean = False
    Private previousWindowState As WindowState
    Private verificationCode As String = ""
    Private clubCode
    Public Sub New(code As String, c_code As String)

        InitializeComponent()
        verificationCode = code
        clubCode = c_code
        Club.LoadClub(clubCode)
        Dim cuerpo As String = "Hola," & vbCrLf & vbCrLf &
                           "Has solicitado recuperar tu contraseña. " &
                           "Tu código de recuperación es: " & verificationCode & vbCrLf & vbCrLf &
                           "Por favor, usa este código para restablecer tu contraseña." & vbCrLf & vbCrLf &
                           "Saludos," & vbCrLf &
                           "ClubManager Support Team"
        EnviarCorreo(Club.Mail, "Código de verificación para restablecer su contraseña", cuerpo)
        MessageBox.Show("Debe haber recibido en la bandeja de entrada del mail asociado a la cuenta un código para restablecer su clave.", "Recuperar contraseña", MessageBoxButton.OK, MessageBoxImage.Information)

        Load()

    End Sub

    Private Sub EnviarCorreo(destinatario As String, asunto As String, cuerpo As String)
        Try
            Dim smtpCliente As New SmtpClient("smtp.gmail.com")
            smtpCliente.Credentials = New NetworkCredential("clubmanager2002@gmail.com", "sdky kcjd lypx kmpg")
            smtpCliente.EnableSsl = True

            Dim mensaje As New MailMessage()
            mensaje.From = New MailAddress("clubmanager02@gmail.com")
            mensaje.To.Add(destinatario)
            mensaje.Subject = asunto
            mensaje.Body = cuerpo
            mensaje.IsBodyHtml = False

            smtpCliente.Send(mensaje)

            Console.WriteLine("Correo enviado exitosamente.")
        Catch ex As Exception
            Console.WriteLine("Error al enviar el correo: " & ex.Message)
        End Try
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


        AddHandler btnPassword.Click, AddressOf showPassword


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

    Private Sub SaveMaterial(sender As Object, e As RoutedEventArgs)

        If verCode.Text <> verificationCode Then
            MessageBox.Show("El código de verificación es incorrecto.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            Return
        End If

        Dim pass = ""
        If clubPassUnmask.Visibility = Visibility.Visible AndAlso clubPassUnmask.Text <> "" Then
            pass = clubPassUnmask.Text
        Else
            If clubPassword.Password <> "" Then
                pass = clubPassword.Password
            End If
        End If

        If pass <> "" And pass.Length > 6 Then
            MessageBox.Show("La contraseña debe tener almenos 6 carácteres.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        Else
            Club.UpdatePass(pass)
            MessageBox.Show("Contraseña cambiada con exito.", "Recuperar clave", MessageBoxButton.OK, MessageBoxImage.Information)
        End If

        e.Handled = True
        Me.Close()

    End Sub

    Public Sub showPassword(sender As Object, e As RoutedEventArgs)

        If clubPassword.Visibility = Visibility.Visible Then
            clubPassUnmask.Text = clubPassword.Password
            clubPassword.Visibility = Visibility.Collapsed
            clubPassUnmask.Visibility = Visibility.Visible
        Else
            clubPassword.Password = clubPassUnmask.Text
            clubPassword.Visibility = Visibility.Visible
            clubPassUnmask.Visibility = Visibility.Collapsed
        End If
        e.Handled = True
    End Sub



End Class
