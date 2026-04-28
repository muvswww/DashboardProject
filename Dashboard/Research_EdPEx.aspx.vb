Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports Dashboard.ConnectDB
Imports Dashboard.Encrypt
Public Class Research_EdPEx
    Inherits System.Web.UI.Page
    Dim dtAll As DataTable
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim MinibleBody As HtmlGenericControl = CType((Me.Master).FindControl("MinibleBody"), HtmlGenericControl)
        MinibleBody.Attributes.Remove("data-layout")
        MinibleBody.Attributes.Remove("data-layout-size")
        If Not IsPostBack Then
            LoadSection()
        End If
        LoadYearDropdown()

    End Sub
    Private Sub LoadYearDropdown()
        Dim SQLRN As String
        Dim dt As DataTable

        SQLRN = "SELECT DISTINCT TOP (100) PERCENT CASE WHEN MONTH(datetime) >= 10 THEN YEAR(datetime) ELSE YEAR(datetime) END AS YEAR
FROM            dbo.Research_EdPEx_Result
ORDER BY YEAR DESC"
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
    Sub LoadSection()

        Dim sql As String = "SELECT DISTINCT TOP (100) PERCENT section, CASE WHEN section = 1 THEN 'Publication' WHEN section = 2 THEN 'Fund' WHEN section = 3 THEN 'Other' END AS sectionName
FROM            dbo.Research_EdPEx
ORDER BY section"

        Dim dt = QueryDataTable2(sql, dbConn, "Dashboard", Nothing)

        rptSection.DataSource = dt
        rptSection.DataBind()

    End Sub

    'Protected Sub rptSection_ItemDataBound(sender As Object, e As RepeaterItemEventArgs)

    '    If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

    '        ' กัน dtAll เป็น Nothing
    '        If dtAll Is Nothing Then Exit Sub

    '        Dim section As String = DataBinder.Eval(e.Item.DataItem, "section").ToString()

    '        Dim dv As New DataView(dtAll)

    '        ' ถ้า section เป็นตัวเลขใช้แบบนี้
    '        dv.RowFilter = "section = " & section

    '        ' ถ้า section เป็น text ให้ใช้แบบนี้แทน
    '        ' dv.RowFilter = "section = '" & section & "'"

    '        Dim dtSub As DataTable = dv.ToTable(True, "subSection")

    '        Dim rptSub As Repeater = CType(e.Item.FindControl("rptSubSection"), Repeater)

    '        rptSub.DataSource = dtSub
    '        rptSub.DataBind()

    '    End If

    'End Sub
    Protected Sub rptSection_ItemDataBound(sender As Object, e As RepeaterItemEventArgs)

        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim section As Integer = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "section"))

            ' ---------- KPI ที่ไม่มี SubSection ----------

            Dim sqlKPI As String = "
SELECT *
FROM Research_EdPEx
LEFT JOIN Research_EdPEx_Result
ON Research_EdPEx.KPI_id = Research_EdPEx_Result.KPI_id
WHERE section = " & section & "
AND subSection IS NULL
ORDER BY KPI_no
"

            Dim dtKPI = QueryDataTable2(sqlKPI, dbConn, "Dashboard", Nothing)

            CalculateResult(dtKPI)

            Dim rptKPINoSub As Repeater = CType(e.Item.FindControl("rptKPINoSub"), Repeater)

            If dtKPI.Rows.Count > 0 Then

                rptKPINoSub.DataSource = dtKPI
                rptKPINoSub.DataBind()

            Else

                rptKPINoSub.Visible = False

            End If
            ' ---------- SubSection ----------
            Dim sqlSub As String = "
        SELECT DISTINCT 
    subSection,
    CASE 
        WHEN section = 1 AND subSection = 1 THEN 'Output'
        WHEN section = 1 AND subSection = 2 THEN 'Support'

        WHEN section = 3 AND subSection = 1 THEN 'Satisfaction'
        WHEN section = 3 AND subSection = 2 THEN 'Training'
        WHEN section = 3 AND subSection = 3 THEN 'IACUC'
    END AS subSectionName
FROM Research_EdPEx
WHERE section = " & section & "
AND subSection IS NOT NULL
ORDER BY subSection
        "

            Dim dtSub = QueryDataTable2(sqlSub, dbConn, "Dashboard", Nothing)

            Dim rptSub As Repeater = CType(e.Item.FindControl("rptSubSection"), Repeater)
            rptSub.DataSource = dtSub
            rptSub.DataBind()

        End If

    End Sub
    'Sub CalculateResult(dt As DataTable)

    '    If Not dt.Columns.Contains("ResultValue") Then
    '        dt.Columns.Add("ResultValue")
    '    End If

    '    For Each r As DataRow In dt.Rows

    '        If Not IsDBNull(r("resultSQL")) AndAlso r("resultSQL").ToString() <> "" Then

    '            Dim sql As String = r("resultSQL").ToString()

    '            Dim value = QueryExecuteScalar2(sql, dbConn)

    '            r("ResultValue") = value

    '        Else
    '            r("ResultValue") = r("result")
    '        End If

    '    Next

    'End Sub
    'Protected Sub rptSubSection_ItemDataBound(sender As Object, e As RepeaterItemEventArgs)

    '    If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

    '        'Dim subSection As Integer = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "subSection"))


    '        'Dim sql As String = "
    '        'SELECT *
    '        'FROM Research_EdPEx
    '        'LEFT JOIN Research_EdPEx_Result
    '        'ON Research_EdPEx.KPI_id = Research_EdPEx_Result.KPI_id
    '        'WHERE subSection = " & subSection & "
    '        'ORDER BY KPI_no
    '        '"

    '        Dim dt = QueryDataTable2(sql, dbConn, "Dashboard", Nothing)

    '        CalculateResult(dt)

    '        Dim rptKPI As Repeater = CType(e.Item.FindControl("rptKPI"), Repeater)
    '        rptKPI.DataSource = dt
    '        rptKPI.DataBind()

    '    End If

    'End Sub
    Protected Sub rptSubSection_ItemDataBound(sender As Object, e As RepeaterItemEventArgs)

        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

            ' ดึงค่า subSection
            Dim subSection As Integer = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "subSection"))

            ' ดึง Repeater ของ Section
            Dim parentItem As RepeaterItem = CType(e.Item.NamingContainer.NamingContainer, RepeaterItem)

            ' ดึงค่า section จาก Repeater บน
            Dim section As Integer = Convert.ToInt32(DataBinder.Eval(parentItem.DataItem, "section"))

            Dim sql As String = "
        SELECT *
        FROM Research_EdPEx
        LEFT JOIN Research_EdPEx_Result
        ON Research_EdPEx.KPI_id = Research_EdPEx_Result.KPI_id
        WHERE section = " & section & "
        AND subSection = " & subSection & "
        ORDER BY KPI_no
        "

            Dim dt = QueryDataTable2(sql, dbConn, "Dashboard", Nothing)

            CalculateResult(dt)

            Dim rptKPI As Repeater = CType(e.Item.FindControl("rptKPI"), Repeater)
            rptKPI.DataSource = dt
            rptKPI.DataBind()

        End If

    End Sub
    Sub CalculateResult(dt As DataTable)

        ' เพิ่ม column ถ้ายังไม่มี
        If Not dt.Columns.Contains("ResultValue") Then
            dt.Columns.Add("ResultValue")
        End If

        ' ปีที่เลือก
        Dim selectedYear As String = Request.QueryString("year")

        If String.IsNullOrEmpty(selectedYear) Or selectedYear = "all" Then
            selectedYear = "2567"   ' default ปี
        End If

        For Each r As DataRow In dt.Rows

            If Not IsDBNull(r("resultSQL")) AndAlso r("resultSQL").ToString().Trim() <> "" Then

                Dim sql As String = r("resultSQL").ToString()

                ' แทนค่า @year
                If sql.Contains("@year") Then
                    sql = sql.Replace("@year", selectedYear)
                End If

                ' debug ดู SQL จริง
                'Response.Write("<br>" & sql & "<br>")

                Dim dtValue As DataTable = QueryDataTable2(sql, dbConn, "Dashboard", Nothing)

                If dtValue.Rows.Count > 0 Then
                    'Response.Write("<br>VALUE = " & dtValue.Rows(0)(0) & "<br>")
                    r("ResultValue") = If(IsDBNull(dtValue.Rows(0)(0)), 0, dtValue.Rows(0)(0))
                Else
                    r("ResultValue") = 0
                End If

            Else
                r("ResultValue") = r("result")
            End If

        Next

    End Sub
    Function GetSelectedYear() As String

        Dim selectedYear As String = Request.QueryString("year")

        If String.IsNullOrEmpty(selectedYear) Then

            Dim sqlYear As String = "
        SELECT TOP 1 
        CASE 
            WHEN MONTH(datetime) >= 10 THEN YEAR(datetime)
            ELSE YEAR(datetime)
        END AS year
        FROM dbo.Research_EdPEx_Result
        ORDER BY year DESC
        "

            Dim dtYear = QueryDataTable2(sqlYear, dbConn, "Dashboard", Nothing)

            If dtYear.Rows.Count > 0 Then
                selectedYear = dtYear.Rows(0)("year").ToString()
            End If

        End If

        Return selectedYear

    End Function
End Class