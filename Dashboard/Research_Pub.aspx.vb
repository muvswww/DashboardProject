Imports Dashboard.ConnectDB
Imports Dashboard.Encrypt
Imports System.Web.Script.Serialization
Imports System.Data.SqlClient

Public Class Research_Pub
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim MinibleBody As HtmlGenericControl = CType((Me.Master).FindControl("MinibleBody"), HtmlGenericControl)
        MinibleBody.Attributes.Remove("data-layout")
        MinibleBody.Attributes.Remove("data-layout-size")
        If Not IsPostBack Then
            LoadYearDropdown()
            SumProject_PA()
            BindData()
        End If
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

        ' ตรวจสอบว่ามีการเลือกปีไหม (ผ่าน querystring)
        'Dim selectedYear As String = Request.QueryString("year") + 543
        'If String.IsNullOrEmpty(selectedYear) OrElse selectedYear = "all" Then
        '    lblSelectedYear.InnerText = "ทั้งหมด"
        'Else
        '    lblSelectedYear.InnerText = selectedYear
        'End If
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
        Dim SQLRN As String
        Dim dt As DataTable
        Dim selectedYear As String = Request.QueryString("year")

        If String.IsNullOrEmpty(selectedYear) Then
            Dim dtYear As DataTable
            Dim sqlYear As String = "
            SELECT DISTINCT TOP (1) year
            FROM Research_Pub
            ORDER BY year DESC"
            dtYear = QueryDataTable2(sqlYear, dbConn, "Dashboard", Nothing)

            If dtYear.Rows.Count > 0 Then
                selectedYear = dtYear.Rows(0)("year").ToString()
            End If
        End If

        SQLRN = "SELECT
            SUM(CASE WHEN [1.1] = 1 THEN 1 ELSE 0 END) AS C11,
            SUM(CASE WHEN [1.2] = 1 THEN 1 ELSE 0 END) AS C12,
            SUM(CASE WHEN [1.3] = 1 THEN 1 ELSE 0 END) AS C13,
            SUM(CASE WHEN [1.9] = 1 THEN 1 ELSE 0 END) AS C19
        FROM Research_Pub"

        If selectedYear <> "all" Then
            SQLRN &= " WHERE [year] = '" & selectedYear & "'"
        End If

        dt = QueryDataTable2(SQLRN, dbConn, "Dashboard", Nothing)
        If dt.Rows.Count > 0 Then
            lbl11.Text = If(IsDBNull(dt.Rows(0)("C11")), "0", dt.Rows(0)("C11").ToString())
            lbl12.Text = If(IsDBNull(dt.Rows(0)("C12")), "0", dt.Rows(0)("C12").ToString())
            lbl13.Text = If(IsDBNull(dt.Rows(0)("C13")), "0", dt.Rows(0)("C13").ToString())
            lbl19.Text = If(IsDBNull(dt.Rows(0)("C19")), "0", dt.Rows(0)("C19").ToString())
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
        Dim selectedType As String = If(ViewState("SelectedType") Is Nothing, "", ViewState("SelectedType").ToString())
        Dim selectedMetric As String = If(ViewState("SelectedMetric") Is Nothing, "", ViewState("SelectedMetric").ToString())
        Dim SQLRN As String = "
        SELECT
            no, year, month, type, title, authors,
            scopus_source, TCI, Volume, Issue, Pages, DOI,
             [1.1] AS C11, [1.2] AS C12, [1.3] AS C13, [1.9] AS C19
        FROM Research_Pub
        WHERE 1 = 1
    "

        If Not String.IsNullOrEmpty(selectedType) Then
            SQLRN &= " AND type = " & selectedType
        End If

        Dim selectedYear As String = Request.QueryString("year")
        If Not String.IsNullOrEmpty(selectedYear) AndAlso selectedYear <> "all" Then
            SQLRN &= " AND year = '" & selectedYear & "'"
        End If

        Select Case selectedMetric
            Case "C11"
                SQLRN &= " AND [1.1] = 1"
            Case "C12"
                SQLRN &= " AND [1.2] = 1"
            Case "C13"
                SQLRN &= " AND [1.3] = 1"
            Case "C19"
                SQLRN &= " AND [1.9] = 1"
        End Select
        SQLRN &= " ORDER BY no"

        Dim dt As DataTable = QueryDataTable2(SQLRN, dbConn, "Dashboard", Nothing)

        data.DataSource = dt
        data.DataBind()

    End Sub
    Public Function ShowCheckIcon(value As Object) As String
        If value IsNot DBNull.Value AndAlso Convert.ToBoolean(value) Then
            Return "<i class='mdi mdi-check' style='color:#1f3a5f;font-size:18px;'></i>"
        End If
        Return ""
    End Function
    Protected Sub Metric_Click(sender As Object, e As EventArgs)
        Dim btn As LinkButton = CType(sender, LinkButton)

        ViewState("SelectedMetric") = btn.CommandArgument

        BindData()
    End Sub
    Protected Sub btnApply_Click(sender As Object, e As EventArgs)
        BindData()
        ApplyColumnVisibility()
    End Sub
    Private Sub ApplyColumnVisibility()

        ' ถ้าไม่เลือกอะไร → แสดงทั้งหมด
        Dim anyChecked As Boolean =
        chkTitle.Checked Or chkAuthors.Checked Or chkSource.Checked Or
        chkVolume.Checked Or chkIssue.Checked Or chkPages.Checked Or chkDOI.Checked Or
        chkC11.Checked Or chkC12.Checked Or chkC13.Checked Or chkC19.Checked

        For Each col As DataControlField In data.Columns
            col.Visible = Not anyChecked
        Next

        If Not anyChecked Then Exit Sub

        ' ตามลำดับ Columns ใน GridView
        data.Columns(0).Visible = chkTitle.Checked
        data.Columns(1).Visible = chkAuthors.Checked
        data.Columns(2).Visible = chkSource.Checked
        data.Columns(3).Visible = chkVolume.Checked
        data.Columns(4).Visible = chkIssue.Checked
        data.Columns(5).Visible = chkPages.Checked
        data.Columns(6).Visible = chkDOI.Checked
        data.Columns(7).Visible = chkC11.Checked
        data.Columns(8).Visible = chkC12.Checked
        data.Columns(9).Visible = chkC13.Checked
        data.Columns(10).Visible = chkC19.Checked

    End Sub



End Class