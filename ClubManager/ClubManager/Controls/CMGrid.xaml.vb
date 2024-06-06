Imports System.ComponentModel
Imports System.Windows.Controls
Imports System.Windows.Media
Imports System.Windows.Media.Imaging
Imports System.Windows
Imports System.Windows.Threading
Imports System.Drawing
Imports System.Data
Imports System.Windows.Controls.Primitives
Imports Utils_CS
Imports Microsoft.Win32
Imports Excel = Microsoft.Office.Interop.Excel
Imports System.Windows.Forms

Namespace Controls

    Public Class CMGrid

        Public Property DG As Windows.Controls.DataGrid
            Get
                DG = Me.MyDataGrid
            End Get
            Set(value As Windows.Controls.DataGrid)
                Me.MyDataGrid = value
            End Set
        End Property

        Public Property ExportToExcel As Boolean
            Get
                If ExcelBtn.Visibility = System.Windows.Visibility.Visible Then
                    Return True
                Else
                    Return False
                End If
            End Get
            Set(value As Boolean)
                If value = True Then
                    ExcelBtn.Visibility = System.Windows.Visibility.Visible
                Else
                    ExcelBtn.Visibility = System.Windows.Visibility.Collapsed
                End If
            End Set
        End Property

        Public ReadOnly Property ExcelButton As CMButton
            Get
                Return ExcelBtn
            End Get
        End Property

        Public ReadOnly Property Rows As System.Data.DataRowCollection
            Get
                Dim row, col As Integer
                Dim cols As Integer = Me.MyDataGrid.Columns.Count - 1
                Dim obj As Object()
                Dim cell As Windows.Controls.DataGridCell
                Dim rowcol As New DataTable
                Dim newrow As DataRow
                ReDim obj(0 To cols)

                For Each x As DataGridColumn In Me.MyDataGrid.Columns
                    Dim dCol As DataColumn = New DataColumn()
                    dCol.ColumnName = x.Header.ToString()
                    rowcol.Columns.Add(dCol)
                Next
                For row = 0 To Me.MyDataGrid.Items.Count - 1
                    newrow = rowcol.NewRow
                    For col = 0 To cols
                        cell = Me.GetCell(Me.MyDataGrid, row, col)
                        If Not IsNothing(cell) Then
                            obj(col) = cell.Content.text
                        End If
                    Next
                    newrow.ItemArray() = obj
                    rowcol.Rows.Add(newrow)
                Next
                Return rowcol.Rows
            End Get
        End Property

        Public Function GetCell(dataGrid As Windows.Controls.DataGrid, row As Integer, col As Integer) As Windows.Controls.DataGridCell
            Dim row2 As DataGridRow = Me.GetRow(dataGrid, row)
            Return Me.GetCell2(dataGrid, row2, col)
        End Function

        Public Function GetRow(dataGrid As Windows.Controls.DataGrid, i As Integer) As DataGridRow
            Dim row As DataGridRow = DirectCast(dataGrid.ItemContainerGenerator.ContainerFromIndex(i), DataGridRow)
            If row Is Nothing Then
                dataGrid.UpdateLayout()
                dataGrid.ScrollIntoView(dataGrid.Items(i))
                row = DirectCast(dataGrid.ItemContainerGenerator.ContainerFromIndex(i), DataGridRow)
            End If
            Return row
        End Function

        Public Function GetCell2(dataGrid As Windows.Controls.DataGrid, row As DataGridRow, col As Integer) As Windows.Controls.DataGridCell
            If row Is Nothing Then
                Dim pres As DataGridCellsPresenter = GetVisualChild(Of DataGridCellsPresenter)(row)
                If pres Is Nothing Then
                    dataGrid.ScrollIntoView(row, dataGrid.Columns(col))
                    pres = GetVisualChild(Of DataGridCellsPresenter)(row)
                End If

                Dim cel As Windows.Controls.DataGridCell = DirectCast(pres.ItemContainerGenerator.ContainerFromIndex(col), Windows.Controls.DataGridCell)
                Return cel
            End If
            Return Nothing
        End Function

        Public Overloads Function GetVisualChild(Of T As Visual)(dad As Visual) As T
            Dim child As T = Nothing
            Dim nVis As Integer = VisualTreeHelper.GetChildrenCount(dad)
            For i As Integer = 0 To nVis - 1
                Dim vis As Visual = DirectCast(VisualTreeHelper.GetChild(dad, i), Visual)
                child = TryCast(vis, T)
                If child Is Nothing Then
                    child = GetVisualChild(Of T)(vis)
                End If
                If child IsNot Nothing Then
                    Exit For
                End If
            Next
            Return child
        End Function

        Public Sub New()

            ' Esta llamada es exigida por el diseñador.
            InitializeComponent()

            ' Agregue cualquier inicialización después de la llamada a InitializeComponent().
            Me.ExportToExcel = True
            Me.DG.IsReadOnly = True
            Me.DG.CanUserAddRows = False
            Me.DG.CanUserDeleteRows = False
            Me.DG.CanUserResizeRows = False
            Me.DG.CanUserSortColumns = True
            Me.DG.CanUserResizeColumns = True

            Me.ExcelBtn.ImageName = "excel.png"
            Me.ExcelBtn.ToolTip = "Exportar a excel"
            If Not System.ComponentModel.DesignerProperties.GetIsInDesignMode(Me) Then
                AddHandler Me.ExcelBtn.Click, AddressOf ExportFunction
                GridCounter()
            End If
        End Sub

        Public Sub ExportFunction()
            If MyDataGrid.Items.Count = 0 Then Return

            Utils_CS.Excel.OfficeTools.ExportExcel(Me.MyDataGrid)
        End Sub

        Public Function AddColumn(header As String, biding As String, width As Integer, cansort As Boolean, aligment As Windows.HorizontalAlignment, Optional type As String = "TEXT", Optional bindingmode As BindingMode = BindingMode.OneWay, Optional backgroundbrush As Media.Brush = Nothing, Optional fontbrush As Media.Brush = Nothing, Optional membersort As String = "") As DataGridBoundColumn
            Dim col1 As DataGridBoundColumn

            Try
                Dim padL As New Thickness(2, 0, 0, 0)
                Dim padR As New Thickness(0, 0, 2, 0)
                Dim padC As New Thickness(0, 0, 0, 0)
                Dim x As New Windows.Controls.Label
                x.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch

                Select Case type
                    Case "CHECK"
                        Dim col0 As DataGridCheckBoxColumn = New DataGridCheckBoxColumn
                        col0.IsThreeState = False
                        col1 = col0
                    Case Else
                        col1 = New DataGridTextColumn
                End Select

                If Not biding = "" Then
                    Dim v = New Data.Binding(biding)
                    v.Mode = bindingmode

                    Select Case type
                        Case "DATE"
                            v.StringFormat = "dd/MM/yyyy"
                            v.TargetNullValue = ""
                        Case "DATETIME"
                            v.StringFormat = "dd/MM/yyyy HH:mm:ss"
                            v.TargetNullValue = ""
                        Case "DECIMAL"
                            v.StringFormat = "{0:0.00}"
                        Case "INTEGER"
                            v.StringFormat = "{0:0}"
                        Case "INTEGER2"
                            v.StringFormat = "{0:#,##0}"
                    End Select

                    col1.Binding = v
                End If

                col1.Width = width
                If col1.Width = New System.Windows.Controls.DataGridLength(0).Value Then
                    col1.Visibility = Visibility.Collapsed
                End If

                col1.CanUserSort = cansort
                If membersort <> "" Then
                    col1.SortMemberPath = membersort
                End If
                col1.CanUserResize = True

                Dim st As Style = Nothing

                Select Case aligment
                    Case System.Windows.HorizontalAlignment.Left
                        st = New Style(GetType(System.Windows.Controls.DataGridCell), Application.Current.TryFindResource("LeftCell"))
                        col1.HeaderStyle = Application.Current.TryFindResource("LeftCellHead")
                        x.Padding = padL
                        x.Content = header
                    Case System.Windows.HorizontalAlignment.Right
                        st = New Style(GetType(System.Windows.Controls.DataGridCell), Application.Current.TryFindResource("LeftCell"))
                        col1.HeaderStyle = Application.Current.TryFindResource("LeftCellHead")
                        x.Padding = padR
                        x.Content = header
                    Case System.Windows.HorizontalAlignment.Center
                        st = New Style(GetType(System.Windows.Controls.DataGridCell), Application.Current.TryFindResource("LeftCell"))
                        col1.HeaderStyle = Application.Current.TryFindResource("LeftCellHead")
                        x.Padding = padC
                        x.Content = header
                End Select

                col1.Header = x

                If Not IsNothing(backgroundbrush) Then
                    Dim setBack = New Setter()
                    setBack.Property = Windows.Controls.DataGridCell.BackgroundProperty
                    setBack.Value = backgroundbrush
                    st.Setters.Add(setBack)
                End If

                If Not IsNothing(fontbrush) Then
                    Dim setText = New Setter()
                    setText.Property = Windows.Controls.DataGridCell.ForegroundProperty
                    setText.Value = fontbrush
                    st.Setters.Add(setText)
                End If

                col1.CellStyle = st
                DG.Columns.Add(col1)
                Return col1
            Catch ex As Exception

            End Try
        End Function
        Public Sub GridCounter(Optional n As Integer = -1)
            If n > -1 Then
                Me.Counter.Content = 0
            Else
                Me.Counter.Content = MyDataGrid.Items.Count
            End If

        End Sub

        'Private Sub ExportToExcel2(ByVal dataGridView As Forms.DataGridView, ByVal filePath As String)
        '    ' Crear una instancia de Excel y crear un nuevo libro de trabajo
        '    Dim excelApp As New Excel.Application()
        '    Dim workbook As Excel.Workbook = excelApp.Workbooks.Add()
        '    Dim worksheet As Excel.Worksheet = workbook.Sheets(1)

        '    ' Copiar los datos del DataGridView al libro de trabajo de Excel
        '    For i As Integer = 0 To dataGridView.Rows.Count - 2
        '        For j As Integer = 0 To dataGridView.Columns.Count - 1
        '            worksheet.Cells(i + 1, j + 1) = dataGridView(j, i).Value.ToString()
        '        Next
        '    Next

        '    ' Guardar el libro de trabajo de Excel en el archivo especificado
        '    workbook.SaveAs(filePath)

        '    ' Cerrar Excel y liberar recursos
        '    workbook.Close()
        '    excelApp.Quit()
        '    releaseObject(worksheet)
        '    releaseObject(workbook)
        '    releaseObject(excelApp)

        '    Windows.MessageBox.Show("Exportación completada.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information)
        'End Sub

        '' Método para liberar recursos COM
        'Private Sub releaseObject(ByVal obj As Object)
        '    Try
        '        System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
        '        obj = Nothing
        '    Catch ex As Exception
        '        obj = Nothing
        '    Finally
        '        GC.Collect()
        '    End Try
        'End Sub

        '' Evento de clic en el botón de exportación
        'Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles ExcelBtn.Click
        '    ' Abrir el cuadro de diálogo para seleccionar la ubicación del archivo
        '    Dim saveFileDialog As New Forms.SaveFileDialog()
        '    saveFileDialog.Filter = "Archivo de Excel (*.xlsx)|*.xlsx"
        '    saveFileDialog.Title = "Guardar archivo de Excel"
        '    saveFileDialog.FileName = "DatosExportados.xlsx"

        '    If saveFileDialog.ShowDialog() = DialogResult.OK Then
        '        ' Llamar al método de exportación pasando el DataGridView y la ubicación del archivo
        '        ' Definir las columnas del DataGridView
        '        Dim DataGridView1 As New DataGridView
        '        Dim colNombre As New DataGridViewTextBoxColumn()
        '        colNombre.HeaderText = "Nombre"
        '        colNombre.DataPropertyName = "Nombre"

        '        Dim colEdad As New DataGridViewTextBoxColumn()
        '        colEdad.HeaderText = "Edad"
        '        colEdad.DataPropertyName = "Edad"

        '        Dim colPosicion As New DataGridViewTextBoxColumn()
        '        colPosicion.HeaderText = "Posición"
        '        colPosicion.DataPropertyName = "Posicion"

        '        ' Agregar las columnas al DataGridView
        '        DataGridView1.Columns.Add(colNombre)
        '        DataGridView1.Columns.Add(colEdad)
        '        DataGridView1.Columns.Add(colPosicion)

        '        ' Agregar datos al DataGridView
        '        DataGridView1.Rows.Add("Lionel Messi", 34, "Delantero")
        '        DataGridView1.Rows.Add("Cristiano Ronaldo", 36, "Delantero")
        '        DataGridView1.Rows.Add("Neymar Jr", 29, "Delantero")
        '        DataGridView1.Rows.Add("Kevin De Bruyne", 30, "Centrocampista")
        '        DataGridView1.Rows.Add("Virgil van Dijk", 30, "Defensa")
        '        ExportToExcel2(DataGridView1, saveFileDialog.FileName)
        '    End If
        'End Sub



    End Class


End Namespace
