Imports System.Web.Script.Serialization
Imports System.Data.SqlClient
Imports Dashboard.ConnectDB
Imports Dashboard.Encrypt
Imports System.Reflection.Emit
Public Class Research_Innovation
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim MinibleBody As HtmlGenericControl = CType((Me.Master).FindControl("MinibleBody"), HtmlGenericControl)
        MinibleBody.Attributes.Remove("data-layout")
        MinibleBody.Attributes.Remove("data-layout-size")

        If Not IsPostBack Then
            LoadYearDropdown()
            LoadData()
            LoadTotalCount()
        End If

    End Sub
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        GridView1.PageIndex = e.NewPageIndex
    End Sub
    Private Sub LoadYearDropdown()
        Dim SQLRN As String
        Dim dt As DataTable

        SQLRN = "SELECT DISTINCT YEAR(request_date) + 543 AS display_year FROM Research_Innovation WHERE request_date IS NOT NULL ORDER BY display_year DESC"
        dt = QueryDataTable2(SQLRN, dbConn, "Dashboard", Nothing)

        ' เคลียร์ dropdown ก่อนเพิ่มใหม่
        yearDropdown.InnerHtml = ""
        ' เพิ่มตัวเลือก "ทั้งหมด"
        yearDropdown.InnerHtml &= "<a class='dropdown-item' href='?year=all'>ทั้งหมด</a>"

        ' เพิ่มปีจากฐานข้อมูล
        For Each row As DataRow In dt.Rows
            Dim y As String = row("display_year").ToString()
            yearDropdown.InnerHtml &= $"<a class='dropdown-item' href='?year={y}'>{y}</a>"
        Next

        ' ตรวจสอบว่ามีการเลือกปีไหม (ผ่าน querystring)
        Dim selectedYear As String = Request.QueryString("year")
        If String.IsNullOrEmpty(selectedYear) OrElse selectedYear = "all" Then
            lblSelectedYear.InnerText = "ทั้งหมด"
        Else
            lblSelectedYear.InnerText = selectedYear
        End If
    End Sub
    Private Sub LoadTotalCount()
        Dim selectedYear As String = Request.QueryString("year")
        Dim SQLRN As String

        SQLRN = "SELECT 
              SUM(CASE WHEN type = 1 THEN 1 ELSE 0 END) AS patent_count,
              SUM(CASE WHEN type = 2 THEN 1 ELSE 0 END) AS utility_count
           FROM Research_Innovation
           WHERE request_date IS NOT NULL"

        If Not String.IsNullOrEmpty(selectedYear) AndAlso selectedYear <> "all" Then
            Dim yearAD As Integer = CInt(selectedYear) - 543
            SQLRN &= $" AND YEAR(request_date) = {yearAD}"
        End If

        Dim dt As DataTable = QueryDataTable2(SQLRN, dbConn, "Dashboard", Nothing)

        If dt.Rows.Count > 0 Then
            total1.InnerHtml = $"<b>{dt.Rows(0)("patent_count")}</b>"
            total2.InnerHtml = $"<b>{dt.Rows(0)("utility_count")}</b>"
        Else
            total1.InnerHtml = "<b>0</b>"
            total2.InnerHtml = "<b>0</b>"
        End If
    End Sub

    Private Sub LoadData()
        Dim selectedYear As String = Request.QueryString("year")
        Dim selectedType As String = Request.QueryString("type") ' all / patent / utility
        Dim SQL As String = "SELECT        dbo.Research_Innovation.no, dbo.Research_Innovation.title, dbo.Research_Innovation.user_id, dbo.Research_Innovation.type, 
                         CASE WHEN dbo.Research_Innovation.type = 1 THEN N'สิทธิบัตร' WHEN dbo.Research_Innovation.type = 2 THEN N'อนุสิทธิบัตร' WHEN dbo.Research_Innovation.type = 4 THEN N'ขอสิทธิบัตร' WHEN dbo.Research_Innovation.type = 5 THEN N'ขออนุสิทธิบัตร'   ELSE N'-' END AS type_name,FORMAT(dbo.Research_Innovation.request_date, 'dd/MM/yyyy', 'th-TH') AS request_date, 
                         dbo.Research_Innovation.request_number, CASE WHEN itjobs.dbo.title_technical.title_technicalName IS NULL THEN itjobs.dbo.title.title_name + itjobs.dbo.[user].fname + SPACE(2) 
                         + itjobs.dbo.[user].lname ELSE itjobs.dbo.title_technical.title_technicalName + + itjobs.dbo.[user].fname + SPACE(2) + itjobs.dbo.[user].lname END AS fullname
FROM            dbo.Research_Innovation INNER JOIN
                         itjobs.dbo.[user] ON dbo.Research_Innovation.user_id = itjobs.dbo.[user].user_id INNER JOIN
                         itjobs.dbo.title ON itjobs.dbo.[user].title_id = itjobs.dbo.title.title_id INNER JOIN
                         itjobs.dbo.title_technical ON itjobs.dbo.[user].title_technicalID = itjobs.dbo.title_technical.title_technicalID
WHERE        (dbo.Research_Innovation.request_date IS NOT NULL)"
        Dim filter As String = ""

        ' ===== กรองตามปี =====
        If Not String.IsNullOrEmpty(selectedYear) AndAlso selectedYear <> "all" Then
            Dim yearAD As Integer = CInt(selectedYear) - 543
            filter &= $" AND YEAR(request_date) = {yearAD}"
        End If

        ' ===== กรองตามประเภท =====
        If Not String.IsNullOrEmpty(selectedType) AndAlso selectedType <> "all" Then
            If selectedType = "patent" Then
                filter &= " AND type = 1"
            ElseIf selectedType = "utility" Then
                filter &= " AND type = 2"
            End If
        End If

        SQL &= filter & " ORDER BY dbo.Research_Innovation.type, dbo.Research_Innovation.request_date DESC"
        'SQL &= filter & " ORDER BY type, request_date DESC"

        ' ===== ดึงข้อมูลหลัก =====
        Dim dt As DataTable = QueryDataTable2(SQL, dbConn, "Dashboard", Nothing)

        GridView1.DataSource = dt
        GridView1.DataBind()

    End Sub
End Class