Imports System.Data.SqlClient
Imports System.Web.Script.Serialization
Imports Dashboard.ConnectDB
Imports Dashboard.Encrypt
Imports iTextSharp.text

Public Class Research_Pub
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim MinibleBody As HtmlGenericControl = CType((Me.Master).FindControl("MinibleBody"), HtmlGenericControl)
        MinibleBody.Attributes.Remove("data-layout")
        MinibleBody.Attributes.Remove("data-layout-size")

        If Not IsPostBack Then
            LoadYearDropdown()
            SumProject_PA()
        End If
        BindData()

    End Sub
    Private Sub LoadYearDropdown()
        Dim SQLRN As String
        Dim dt As DataTable

        SQLRN = "SELECT DISTINCT TOP (100) PERCENT year
FROM            dbo.Research_Pub
ORDER BY year DESC"
        dt = QueryDataTable2(SQLRN, dbConn, "Dashboard", Nothing)

        ' เคลียร์ dropdown ก่อนเพิ่มใหม่
        yearDropdown.InnerHtml = ""
        ' เพิ่มตัวเลือก "ทั้งหมด"
        yearDropdown.InnerHtml &= "<a class='dropdown-item' href='?year=all'>ทั้งหมด</a>"

        ' เพิ่มปีจากฐานข้อมูล
        For Each row As DataRow In dt.Rows
            Dim y As String = row("year").ToString()
            yearDropdown.InnerHtml &= $"<a class='dropdown-item' href='?year={y}'>{y}</a>"
        Next


        Dim selectedYear As String = Request.QueryString("year")
        If String.IsNullOrEmpty(selectedYear) Then
            ' ❗ ไม่มี querystring → ใช้ปีล่าสุดจาก SQL
            lblSelectedYear.InnerText = dt.Rows(0)("year").ToString()

        ElseIf selectedYear = "all" Then
            lblSelectedYear.InnerText = "ทั้งหมด"
        Else
            lblSelectedYear.InnerText = (CInt(selectedYear)).ToString()
        End If

    End Sub
    Private Sub SumProject_PA()
        Dim selectedYear As String = Request.QueryString("year")
        Dim SQLRN As String = ""
        Dim parameters As New List(Of SqlParameter)
        Dim dt As DataTable
        '🟡 กรณีไม่ได้เลือกปี → ใช้ปีล่าสุดจาก MasterProject
        If String.IsNullOrEmpty(selectedYear) Then
            SQLRN = "SELECT DISTINCT TOP (100) PERCENT year
    FROM            dbo.Research_Pub
    ORDER BY year DESC"
            dt = QueryDataTable2(SQLRN, dbConn, "Dashboard", Nothing)
            If dt.Rows.Count > 0 Then
                selectedYear = dt.Rows(0)("year").ToString()
            End If

        End If

   

        SQLRN = "SELECT
        SUM(CASE WHEN type = 1 THEN 1 ELSE 0 END) AS Type1_Count,
        SUM(CASE WHEN type = 2 THEN 1 ELSE 0 END) AS Type2_Count
    FROM Research_Pub
    "
        If selectedYear <> "all" Then
            SQLRN &= " WHERE [year] = '" & selectedYear & "'"
        End If
        dt = QueryDataTable2(SQLRN, dbConn, "Dashboard", Nothing)
        If dt.Rows.Count > 0 Then
            sumType1.InnerText = If(IsDBNull(dt.Rows(0)("Type1_Count")), "0", dt.Rows(0)("Type1_Count").ToString())
            sumType2.InnerText = If(IsDBNull(dt.Rows(0)("Type2_Count")), "0", dt.Rows(0)("Type2_Count").ToString())
        End If
    End Sub
    Protected Sub Filter_Click(sender As Object, e As EventArgs)
        Dim btn As LinkButton = CType(sender, LinkButton)
        Dim selectedVal As String = btn.CommandArgument

        ' เก็บค่าที่เลือกลง ViewState (ตัวช่วยจำค่าของ ASP.NET)
        ViewState("SelectedType") = selectedVal

        ' ปรับสีปุ่ม ให้รู้ว่าเลือกอันไหนอยู่ (UX)
        UpdateButtonStyles(selectedVal)

        ' โหลดข้อมูลใหม่
        BindData()
    End Sub
    Private Sub UpdateButtonStyles(val As String)
        ' รีเซ็ตปุ่มทั้งหมดเป็นแบบโปร่ง (outline)
        btnAll.CssClass = "btn btn-outline-primary"
        btnAll.Text = "<i class='mdi mdi-circle-outline me-1'></i>ทั้งหมด"

        btnType1.CssClass = "btn btn-outline-primary"
        btnType1.Text = "<i class='mdi mdi-circle-outline me-1'></i>ระดับชาติ"

        btnType2.CssClass = "btn btn-outline-primary"
        btnType2.Text = "<i class='mdi mdi-circle-outline me-1'></i>ระดับนานาชาติ"

        ' เช็คว่าอันไหนถูกเลือก ให้เปลี่ยนเป็นสีทึบ และเปลี่ยนไอคอน
        Select Case val
            Case "1"
                btnType1.CssClass = "btn btn-soft-primary"
                btnType1.Text = "<i class='mdi mdi-check-circle-outline me-1'></i>ระดับชาติ"
            Case "2"
                btnType2.CssClass = "btn btn-soft-primary"
                btnType2.Text = "<i class='mdi mdi-check-circle-outline me-1'></i>ระดับนานาชาติ"
            Case Else ' ทั้งหมด
                btnAll.CssClass = "btn btn-soft-primary"
                btnAll.Text = "<i class='mdi mdi-check-circle-outline me-1'></i>ทั้งหมด"
        End Select
    End Sub

    Private Sub BindData()
        Dim selectedYear As String = Request.QueryString("year")
        Dim selectedType As String = If(ViewState("SelectedType") Is Nothing, "", ViewState("SelectedType").ToString())

        ' 👉 ใช้ function เดียว
        Dim cols As List(Of String) = GetSelectedColumns()

        Dim colString As String
        If cols.Count = 0 Then
            colString = "year, month, type, title, authors, scopus_source, Volume, Issue, Pages, DOI, InterCollab, academicCollab, SDG, cluster, Q1, Q2, Q3, Q4, Top1, Top10, TCI_G1, TCI_G2, TCI_G3"
        Else
            colString = String.Join(",", cols)
        End If

        Dim sql As String = "SELECT " & colString & " FROM Research_Pub WHERE 1=1 "

        If selectedType <> "" Then
            sql &= " AND type = " & selectedType
        End If
        If String.IsNullOrEmpty(selectedYear) OrElse selectedYear = "all" Then

            Dim dtYear As DataTable = QueryDataTable2("
        SELECT TOP 1 year 
        FROM Research_Pub 
        ORDER BY year DESC", dbConn, "Dashboard", Nothing)

            If dtYear.Rows.Count > 0 Then
                selectedYear = dtYear.Rows(0)("year").ToString()
            End If

        End If
        sql &= " AND year = " & selectedYear

        Dim dt As DataTable = QueryDataTable2(sql, dbConn, "Dashboard", Nothing)

        ' 🔥 clear column
        data.Columns.Clear()

        ' 🔥 generate column
        For Each col As DataColumn In dt.Columns

            If IsBitColumn(col.ColumnName) Then

                Dim tf As New TemplateField()
                tf.HeaderText = col.ColumnName
                tf.ItemTemplate = New CheckTemplate(col.ColumnName)
                data.Columns.Add(tf)

            ElseIf col.ColumnName = "month" Then

                Dim tf As New TemplateField()
                tf.HeaderText = "Month"
                tf.ItemTemplate = New MonthTemplate()
                data.Columns.Add(tf)

            Else
                Dim bf As New BoundField()
                bf.DataField = col.ColumnName
                bf.HeaderText = col.ColumnName
                data.Columns.Add(bf)
            End If

        Next

        data.DataSource = dt
        data.DataBind()

    End Sub
    'Private Function ShowCheck(val As Object) As String
    '    If val Is Nothing OrElse IsDBNull(val) Then
    '        Return ""
    '    End If

    '    Dim str As String = val.ToString().Trim().ToLower()

    '    If str = "1" OrElse str = "true" Then
    '        Return "✓"
    '    End If

    '    Return ""
    'End Function
    Protected Sub btnLoad_Click(sender As Object, e As EventArgs)
        BindData()
    End Sub
    Private Function IsBitColumn(colName As String) As Boolean
        Dim bitCols As String() = {
            "cluster", "Q1", "Q2", "Q3", "Q4",
            "Top1", "Top10", "TCI_G1", "TCI_G2", "TCI_G3"
        }

        Return bitCols.Contains(colName)
    End Function
    Public Class CheckTemplate
        Implements ITemplate

        Private colName As String

        Public Sub New(columnName As String)
            colName = columnName
        End Sub

        Public Sub InstantiateIn(container As Control) Implements ITemplate.InstantiateIn

            Dim lbl As New Label()

            AddHandler lbl.DataBinding, Sub(sender As Object, e As EventArgs)

                                            Dim l As Label = CType(sender, Label)
                                            Dim row As GridViewRow = CType(l.NamingContainer, GridViewRow)
                                            Dim val = DataBinder.Eval(row.DataItem, colName)

                                            If val IsNot DBNull.Value AndAlso Convert.ToBoolean(val) Then
                                                l.Text = "✓"
                                            Else
                                                l.Text = ""
                                            End If

                                        End Sub

            container.Controls.Add(lbl)

        End Sub
    End Class
    Public Class MonthTemplate
        Implements ITemplate

        Public Sub InstantiateIn(container As Control) Implements ITemplate.InstantiateIn

            Dim lbl As New Label()

            AddHandler lbl.DataBinding, Sub(sender As Object, e As EventArgs)

                                            Dim l As Label = CType(sender, Label)
                                            Dim row As GridViewRow = CType(l.NamingContainer, GridViewRow)
                                            Dim val = DataBinder.Eval(row.DataItem, "month")

                                            If val IsNot DBNull.Value Then
                                                l.Text = MonthName(Convert.ToInt32(val))
                                            End If

                                        End Sub

            container.Controls.Add(lbl)

        End Sub
    End Class
    Private Function GetSelectedColumns() As List(Of String)

        Dim cols As New List(Of String)

        ' 🔹 ข้อมูลหลัก
        If chkYear.Checked Then cols.Add("year")
        If chkMonth.Checked Then cols.Add("month")
        If chkType.Checked Then cols.Add("type")
        If chkTitle.Checked Then cols.Add("title")
        If chkAuthors.Checked Then cols.Add("authors")
        If chkSource.Checked Then cols.Add("scopus_source")

        ' 🔹 บทความ
        If chkVolume.Checked Then cols.Add("Volume")
        If chkIssue.Checked Then cols.Add("Issue")
        If chkPages.Checked Then cols.Add("Pages")
        If chkDOI.Checked Then cols.Add("DOI")

        If chkInterCollab.Checked Then cols.Add("InterCollab")
        If chkAcademicCollab.Checked Then cols.Add("academicCollab")
        If chkSDG.Checked Then cols.Add("SDG")

        ' 🔹 ranking
        If chkCluster.Checked Then cols.Add("cluster")

        If chkQ1.Checked Then cols.Add("Q1")
        If chkQ2.Checked Then cols.Add("Q2")
        If chkQ3.Checked Then cols.Add("Q3")
        If chkQ4.Checked Then cols.Add("Q4")

        If chkTop1.Checked Then cols.Add("Top1")
        If chkTop10.Checked Then cols.Add("Top10")

        If chkTCI1.Checked Then cols.Add("TCI_G1")
        If chkTCI2.Checked Then cols.Add("TCI_G2")
        If chkTCI3.Checked Then cols.Add("TCI_G3")

        Return cols

    End Function
    Protected Sub chkAllColumns_CheckedChanged(sender As Object, e As EventArgs)

        Dim isChecked As Boolean = chkAllColumns.Checked

        chkYear.Checked = isChecked
        chkMonth.Checked = isChecked
        chkType.Checked = isChecked
        chkTitle.Checked = isChecked
        chkAuthors.Checked = isChecked
        chkSource.Checked = isChecked

        chkVolume.Checked = isChecked
        chkIssue.Checked = isChecked
        chkPages.Checked = isChecked
        chkDOI.Checked = isChecked

        chkInterCollab.Checked = isChecked
        chkAcademicCollab.Checked = isChecked
        chkSDG.Checked = isChecked

        chkCluster.Checked = isChecked

        chkQ1.Checked = isChecked
        chkQ2.Checked = isChecked
        chkQ3.Checked = isChecked
        chkQ4.Checked = isChecked

        chkTop1.Checked = isChecked
        chkTop10.Checked = isChecked

        chkTCI1.Checked = isChecked
        chkTCI2.Checked = isChecked
        chkTCI3.Checked = isChecked

        BindData()

    End Sub
End Class