Imports Dashboard.ConnectDB
Imports Dashboard.Encrypt
Imports System.Web.Script.Serialization
Imports System.Data.SqlClient

Public Class testRES
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim MinibleBody As HtmlGenericControl = CType((Me.Master).FindControl("MinibleBody"), HtmlGenericControl)
        MinibleBody.Attributes.Remove("data-layout")
        MinibleBody.Attributes.Remove("data-layout-size")

        If Not IsPostBack Then
            LoadYearDropdown()
            SumProject_PA()
            'BindData()
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
        Dim selectedYear As String = Request.QueryString("year")
        Dim selectedKPIs As List(Of String) = TryCast(ViewState("SelectedKPIs"), List(Of String))
        Dim SQLRN As String = "
        SELECT no, year, month, type, title, authors,
               scopus_source, TCI, Volume, Issue, Pages, DOI, KPI
        FROM Research_Pub
        WHERE 1=1
    "

        If selectedType <> "" Then
            SQLRN &= " AND type = " & selectedType
        End If

        If Not String.IsNullOrEmpty(selectedYear) AndAlso selectedYear <> "all" Then
            SQLRN &= " AND year = " & selectedYear
        End If

        '================ KPI FILTER (ใหม่) =================
        If selectedKPIs IsNot Nothing AndAlso selectedKPIs.Count > 0 Then

            ' ดึง KPI ทั้งหมดจากระบบก่อน เพื่อเช็คว่าเลือกครบไหม
            Dim allKPIList As New List(Of String)

            Dim sqlAllKPI As String = "SELECT DISTINCT KPI FROM Research_Pub WHERE KPI IS NOT NULL"
            Dim dtAll As DataTable = QueryDataTable2(sqlAllKPI, dbConn, "Dashboard", Nothing)

            For Each r As DataRow In dtAll.Rows
                For Each k In r("KPI").ToString().Split(","c)
                    Dim clean = k.Trim().Replace(".", "_")
                    If clean <> "" AndAlso Not allKPIList.Contains(clean) Then
                        allKPIList.Add(clean)
                    End If
                Next
            Next

            ' ถ้าเลือกไม่ครบทุก KPI → ค่อย WHERE
            If selectedKPIs.Count < allKPIList.Count Then
                SQLRN &= " AND ("

                For i As Integer = 0 To selectedKPIs.Count - 1
                    Dim kpi As String = selectedKPIs(i).Replace("_", ".")
                    SQLRN &= " KPI LIKE '%" & kpi & "%' "

                    If i < selectedKPIs.Count - 1 Then
                        SQLRN &= " OR "
                    End If
                Next

                SQLRN &= ")"
            End If
        End If
        '====================================================
        SQLRN &= " ORDER BY no"

        Dim dt As DataTable = QueryDataTable2(SQLRN, dbConn, "Dashboard", Nothing)

        '================ KPI LIST =================
        Dim dtKPI As New DataTable()
        dtKPI.Columns.Add("KPI")

        Dim kpiSet As New HashSet(Of String)

        For Each row As DataRow In dt.Rows
            If Not IsDBNull(row("KPI")) Then
                For Each k In row("KPI").ToString().Split(","c)
                    Dim clean = k.Trim()
                    If clean <> "" Then kpiSet.Add(clean)
                Next
            End If
        Next

        For Each k In kpiSet.OrderBy(Function(x) x)
            dtKPI.Rows.Add(k)
        Next

        '================ ADD KPI COLUMNS TO DT =================
        For Each r As DataRow In dtKPI.Rows
            Dim colName As String = "KPI_" & r("KPI").ToString().Replace(".", "_")
            If Not dt.Columns.Contains(colName) Then
                dt.Columns.Add(colName, GetType(Integer))
            End If
        Next

        For Each row As DataRow In dt.Rows
            If Not IsDBNull(row("KPI")) Then
                For Each k In row("KPI").ToString().Split(","c)
                    Dim colName = "KPI_" & k.Trim().Replace(".", "_")
                    If dt.Columns.Contains(colName) Then row(colName) = 1
                Next
            End If
        Next

        ' ⭐ ต้องสร้าง 3 อย่างนี้ตามลำดับ
        BuildKPICheckBoxes(dtKPI)
        BuildGridColumns(dtKPI)

        data.DataSource = dt
        data.DataBind()

        ApplyColumnVisibility()   ' ค่อยซ่อน/แสดงหลัง bind
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

        '    Dim selected As New List(Of String)

        '    For Each ctrl As Control In phKPI.Controls
        '        Dim chk As CheckBox = TryCast(ctrl.FindControl(ctrl.Controls(0).ID), CheckBox)
        '        If chk IsNot Nothing AndAlso chk.Checked Then
        '            selected.Add(chk.ID.Replace("chkKPI_", ""))
        '        End If
        '    Next

        '    ViewState("SelectedKPIs") = selected
        '    Dim allChecked As Boolean =
        'chkTitle.Checked AndAlso
        'chkAuthors.Checked AndAlso
        'chkSource.Checked AndAlso
        'chkVolume.Checked AndAlso
        'chkIssue.Checked AndAlso
        'chkPages.Checked AndAlso
        'chkDOI.Checked

        '    ' เช็ค KPI ด้วย
        '    For Each ctrl As Control In phKPI.Controls
        '        If TypeOf ctrl Is CheckBox Then
        '            If Not CType(ctrl, CheckBox).Checked Then
        '                allChecked = False
        '                Exit For
        '            End If
        '        End If
        '    Next

        '    chkAllColumns.Checked = allChecked
        ViewState("SelectedKPIs") = GetSelectedKPIsFromUI()
        BindData()

    End Sub


    'Private Sub ApplyColumnVisibility()

    '    ' ===== คอลัมน์พื้นฐาน =====
    '    Dim baseChecks As CheckBox() = {chkTitle, chkAuthors, chkSource, chkVolume, chkIssue, chkPages, chkDOI}
    '    Dim anyBaseChecked = baseChecks.Any(Function(c) c.Checked)

    '    For i As Integer = 0 To 6
    '        data.Columns(i).Visible = Not anyBaseChecked OrElse baseChecks(i).Checked
    '    Next

    '    ' ===== KPI =====
    '    Dim selected As List(Of String) = TryCast(ViewState("SelectedKPIs"), List(Of String))

    '    For i As Integer = 7 To data.Columns.Count - 1
    '        Dim header = data.Columns(i).HeaderText.Replace("[", "").Replace("]", "").Replace(".", "_")

    '        If selected Is Nothing Then
    '            data.Columns(i).Visible = True ' โหลดครั้งแรกแสดงหมด
    '        Else
    '            data.Columns(i).Visible = selected.Contains(header)
    '        End If
    '    Next
    '    ' ---------------- KPI Columns ----------------
    '    Dim selectedKPIs As List(Of String) = Nothing
    '    If ViewState("SelectedKPIs") IsNot Nothing Then
    '        selectedKPIs = CType(ViewState("SelectedKPIs"), List(Of String))
    '    End If

    '    Dim anyKPIChecked As Boolean = (selectedKPIs IsNot Nothing AndAlso selectedKPIs.Count > 0)

    '    For i As Integer = 7 To data.Columns.Count - 1

    '        Dim header As String = data.Columns(i).HeaderText ' [1.1]
    '        Dim clean As String = header.Replace("[", "").Replace("]", "").Replace(".", "_")

    '        If anyKPIChecked Then
    '            ' มีการเลือก → แสดงเฉพาะที่เลือก
    '            data.Columns(i).Visible = selectedKPIs.Contains(clean)
    '        Else
    '            ' ไม่ได้เลือกเลย → แสดงทุก KPI
    '            data.Columns(i).Visible = True
    '        End If

    '    Next

    'End Sub


    Private Sub ApplyColumnVisibility()

        Dim selectedKPIs As List(Of String) = TryCast(ViewState("SelectedKPIs"), List(Of String))

        Dim baseChecked As Boolean =
        chkTitle.Checked Or chkAuthors.Checked Or chkSource.Checked Or
        chkVolume.Checked Or chkIssue.Checked Or chkPages.Checked Or chkDOI.Checked

        Dim kpiChecked As Boolean = (selectedKPIs IsNot Nothing AndAlso selectedKPIs.Count > 0)

        ' ✅ ถ้าไม่ติ๊กอะไรเลย → แสดงทั้งหมด
        If Not baseChecked AndAlso Not kpiChecked Then
            For Each col As DataControlField In data.Columns
                col.Visible = True
            Next
            Exit Sub
        End If

        ' ===== คอลัมน์พื้นฐาน =====
        data.Columns(0).Visible = chkTitle.Checked
        data.Columns(1).Visible = chkAuthors.Checked
        data.Columns(2).Visible = chkSource.Checked
        data.Columns(3).Visible = chkVolume.Checked
        data.Columns(4).Visible = chkIssue.Checked
        data.Columns(5).Visible = chkPages.Checked
        data.Columns(6).Visible = chkDOI.Checked

        ' ===== KPI Columns =====
        For i As Integer = 7 To data.Columns.Count - 1
            Dim header = data.Columns(i).HeaderText.Replace(".", "_")

            If kpiChecked AndAlso selectedKPIs.Contains(header) Then
                data.Columns(i).Visible = True
            Else
                data.Columns(i).Visible = False
            End If
        Next

    End Sub

    Private Sub BuildGridColumns(dtKPI As DataTable)

        data.Columns.Clear()

        ' ===== คอลัมน์พื้นฐาน =====
        data.Columns.Add(New BoundField With {.HeaderText = "ชื่อบทความ", .DataField = "title"})
        data.Columns.Add(New BoundField With {.HeaderText = "ผู้แต่ง", .DataField = "authors"})
        data.Columns.Add(New BoundField With {.HeaderText = "แหล่งตีพิมพ์", .DataField = "scopus_source"})
        data.Columns.Add(New BoundField With {.HeaderText = "Volume", .DataField = "Volume"})
        data.Columns.Add(New BoundField With {.HeaderText = "Issue", .DataField = "Issue"})
        data.Columns.Add(New BoundField With {.HeaderText = "Pages", .DataField = "Pages"})
        data.Columns.Add(New BoundField With {.HeaderText = "DOI", .DataField = "DOI"})

        ' ===== KPI Columns (Dynamic) =====
        For Each r As DataRow In dtKPI.Rows
            Dim kpi As String = r("KPI").ToString()
            Dim colName As String = "KPI_" & kpi.Replace(".", "_")

            Dim tf As New TemplateField()
            tf.HeaderText = kpi
            tf.ItemStyle.HorizontalAlign = HorizontalAlign.Center
            tf.ItemTemplate = New KPIIconTemplate(colName)

            data.Columns.Add(tf)
        Next

    End Sub

    Private Function AnyKPIChecked() As Boolean
        For Each ctrl As Control In phKPI.Controls
            If TypeOf ctrl Is HtmlGenericControl Then
                For Each c As Control In ctrl.Controls
                    If TypeOf c Is CheckBox AndAlso CType(c, CheckBox).Checked Then
                        Return True
                    End If
                Next
            End If
        Next
        Return False
    End Function

    Public Class KPIIconTemplate
        Implements ITemplate

        Private _colName As String

        Public Sub New(colName As String)
            _colName = colName
        End Sub

        Public Sub InstantiateIn(container As Control) Implements ITemplate.InstantiateIn
            Dim lit As New Literal()

            AddHandler lit.DataBinding, Sub(sender, e)
                                            Dim l As Literal = CType(sender, Literal)
                                            Dim row As GridViewRow = CType(l.NamingContainer, GridViewRow)
                                            Dim val = DataBinder.Eval(row.DataItem, _colName)

                                            If val IsNot DBNull.Value AndAlso Not IsNothing(val) Then
                                                l.Text = "<i class='mdi mdi-check' style='color:#1f3a5f;font-size:18px;'></i>"
                                            End If
                                        End Sub

            container.Controls.Add(lit)
        End Sub
    End Class
    Private Sub BuildKPICheckBoxes(dtKPI As DataTable)

        phKPI.Controls.Clear()

        Dim selected As List(Of String) = TryCast(ViewState("SelectedKPIs"), List(Of String))

        For Each r As DataRow In dtKPI.Rows
            Dim kpi As String = r("KPI").ToString()
            Dim clean As String = kpi.Replace(".", "_")

            Dim chk As New CheckBox()
            chk.ID = "chkKPI_" & clean
            chk.CssClass = "form-check-input"

            ' ⭐ ติ๊กกลับถ้าเคยเลือก
            If selected IsNot Nothing AndAlso selected.Contains(clean) Then
                chk.Checked = True
            End If

            Dim lbl As New Label()
            lbl.Text = " " & kpi
            lbl.AssociatedControlID = chk.ID
            lbl.CssClass = "form-check-label"

            Dim div As New HtmlGenericControl("div")
            div.Attributes("class") = "form-check"
            div.Controls.Add(chk)
            div.Controls.Add(lbl)

            phKPI.Controls.Add(div)
        Next

    End Sub




    Protected Sub chkAllColumns_CheckedChanged(sender As Object, e As EventArgs)

        Dim isChecked As Boolean = chkAllColumns.Checked

        chkTitle.Checked = isChecked
        chkAuthors.Checked = isChecked
        chkSource.Checked = isChecked
        chkVolume.Checked = isChecked
        chkIssue.Checked = isChecked
        chkPages.Checked = isChecked
        chkDOI.Checked = isChecked

        For Each ctrl As Control In phKPI.Controls
            If TypeOf ctrl Is HtmlGenericControl Then
                For Each c As Control In ctrl.Controls
                    If TypeOf c Is CheckBox Then
                        CType(c, CheckBox).Checked = isChecked
                    End If
                Next
            End If
        Next

        ViewState("SelectedKPIs") = GetSelectedKPIsFromUI()
        BindData()
    End Sub

    Private Function GetSelectedKPIsFromUI() As List(Of String)

        Dim list As New List(Of String)

        For Each ctrl As Control In phKPI.Controls
            If TypeOf ctrl Is HtmlGenericControl Then
                For Each c As Control In ctrl.Controls
                    If TypeOf c Is CheckBox Then
                        Dim chk As CheckBox = CType(c, CheckBox)
                        If chk.Checked Then
                            list.Add(chk.ID.Replace("chkKPI_", ""))
                        End If
                    End If
                Next
            End If
        Next

        Return list

    End Function




End Class