Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Web.Configuration
Imports Dashboard.ConnectDB
Imports Dashboard.Encrypt
Imports iTextSharp.text.pdf
Imports Org.BouncyCastle.Crypto

Public Class WebForm1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Me.IsPostBack Then
            BindgridFund()
            'LoadYearFund()
            LoadYearFilterFund()
            ddlNameuserFund()
        End If

    End Sub
    Private Sub ddlNameuserFund()
        Dim SQLRN As String = "SELECT        TOP (100) PERCENT dbo.[user].user_id, dbo.[user].isActive, CASE WHEN dbo.title_technical.title_technicalName IS NULL THEN dbo.title.title_name + dbo.[user].fname + SPACE(2) 
                         + .dbo.[user].lname ELSE dbo.title_technical.title_technicalName + dbo.[user].fname + SPACE(2) + dbo.[user].lname END AS fullname
FROM            dbo.[user] INNER JOIN
                         dbo.title ON dbo.[user].title_id = dbo.title.title_id LEFT OUTER JOIN
                         dbo.title_technical ON dbo.[user].title_technicalID = dbo.title_technical.title_technicalID
WHERE        (dbo.[user].isActive = 1)
ORDER BY dbo.[user].fname"
        Dim dt As DataTable = QueryDataTable2(SQLRN, dbConn, "itjobs", Nothing)
        If dt.Rows.Count > 0 Then
            ddlUserFund.DataSource = dt
            ddlUserFund.DataValueField = "user_id"
            ddlUserFund.DataTextField = "fullname"

            ddlUserFund.DataBind()

        End If
        ddlUserFund.Items.Insert(0, New ListItem("-- เลือกหัวหน้าโครงการ --", "0"))


        SQLRN = "SELECT     Fund_ID, Fund_source, Fund_type
FROM            FundType
ORDER BY Fund_ID"
        dt = QueryDataTable2(SQLRN, dbConn, "Dashboard", Nothing)
        If dt.Rows.Count > 0 Then
            ddlSource.DataSource = dt
            ddlSource.DataValueField = "Fund_ID"
            ddlSource.DataTextField = "Fund_source"

            ddlSource.DataBind()
        End If
        ddlSource.Items.Insert(0, New ListItem("-- เลือกแหล่งทุน --", "0"))


    End Sub
    Protected Sub ddlUserFund_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlUserFund.SelectedIndexChanged
        If ddlUserFund.SelectedValue <> "0" Then
            LoadDeptFund(ddlUserFund.SelectedValue)
        Else
            ddlDept.Items.Clear()
            ddlDept.Items.Add(New ListItem("-- เลือกหน่วยงาน --", "0"))
            ddlDept.Enabled = False
        End If
    End Sub

    Private Sub LoadDeptFund(userId As String)

        Dim SQLRN As String = "SELECT d.dept_id, d.dept_name
                           FROM dbo.[user] u
                           INNER JOIN dbo.department d ON u.dept_id = d.dept_id
                           WHERE u.user_id = @userId"

        Dim parameters As SqlParameter() = {
        New SqlParameter("@userId", userId)
    }

        Dim dt As DataTable = QueryDataTable2(SQLRN, dbConn, "itjobs", parameters)

        ddlDept.Items.Clear()

        If dt.Rows.Count > 0 Then
            ' เพิ่มรายการเดียว แล้วเลือกให้เลย
            Dim deptId As String = dt.Rows(0)("dept_id").ToString()
            Dim deptName As String = dt.Rows(0)("dept_name").ToString()

            ddlDept.Items.Add(New ListItem(deptName, deptId))
            ddlDept.SelectedValue = deptId   ' เลือกค่าให้อัตโนมัติ
        Else
            ddlDept.Items.Add(New ListItem("ไม่พบหน่วยงาน", "0"))
        End If

        ddlDept.Enabled = False   ' 🔒 ล็อกไม่ให้ผู้ใช้เปลี่ยน
    End Sub



    'Private Sub LoadYearFund()
    '    Dim SQLRN As String = "SELECT DISTINCT Strategy_Year FROM MasterProject ORDER BY Strategy_Year DESC"
    '    Dim dt As DataTable = QueryDataTable2(SQLRN, dbConn, "Dashboard", Nothing)

    '    ddlYearFund2.Items.Clear()
    '    ddlYearFund2.Items.Add(New ListItem("-- เลือกปี --", ""))

    '    For Each dr As DataRow In dt.Rows
    '        ddlYearFund2.Items.Add(New ListItem(dr("Strategy_Year").ToString(), dr("Strategy_Year").ToString()))
    '    Next
    'End Sub
    'Protected Sub ddlYearFund2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlYearFund2.SelectedIndexChanged
    '    LoadProjectsFund()
    'End Sub


    'Private Sub LoadProjectsFund()
    '    If String.IsNullOrEmpty(ddlYearFund2.SelectedValue) Then
    '        cblProject.Items.Clear()
    '        Exit Sub
    '    End If

    '    Dim selectedYear As Integer = Convert.ToInt32(ddlYearFund2.SelectedValue)


    '    Dim SQLRN As String = "SELECT Strategy_id, Project_no,Project_id, ProjectName " &
    '                    "FROM MasterProject " &
    '                    "WHERE Strategy_Year = @year AND Strategy_id = 1"

    '    Dim parameters As SqlParameter() = {
    '                New SqlParameter("@year", selectedYear)
    '                                               }
    '    Dim dt As DataTable = QueryDataTable2(SQLRN, dbConn, "Dashboard", parameters)
    '    If dt.Rows.Count > 0 Then
    '        cblProject.Items.Clear()

    '        For Each dr As DataRow In dt.Rows
    '            Dim li As New ListItem()
    '            li.Text = dr("ProjectName").ToString()
    '            li.Value = dr("Project_no").ToString()
    '            cblProject.Items.Add(li)
    '        Next
    '    End If

    'End Sub
    Private Sub LoadYearFilterFund()
        Dim SQLRN As String = "SELECT DISTINCT year FROM Research_Fund ORDER BY year DESC"
        Dim dt As DataTable = QueryDataTable2(SQLRN, dbConn, "Dashboard", Nothing)

        ddlYearFund1.Items.Clear()
        ddlYearFund1.Items.Add(New ListItem("ทุกปี", "")) ' ค่าว่าง = แสดงทั้งหมด

        For Each dr As DataRow In dt.Rows
            ddlYearFund1.Items.Add(New ListItem(dr("year").ToString(), dr("year").ToString()))
        Next
    End Sub
    Protected Sub ddlYearFund1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlYearFund1.SelectedIndexChanged
        BindgridFund()
    End Sub
    Private Sub BindgridFund()
        Dim SQLRN As String = "SELECT        dbo.Research_Fund.no, dbo.Research_Fund.year, dbo.Research_Fund.user_id, dbo.Research_Fund.title, dbo.Research_Fund.type, CASE WHEN itjobs.dbo.title_technical.title_technicalName IS NULL 
                         THEN itjobs.dbo.title.title_name + itjobs.dbo.[user].fname + SPACE(2) + itjobs.dbo.[user].lname ELSE itjobs.dbo.title_technical.title_technicalName + itjobs.dbo.[user].fname + SPACE(2) + itjobs.dbo.[user].lname END AS fullname, 
                         CASE WHEN dbo.Research_Fund.type = 1 THEN 'ทุนวิจัย' WHEN dbo.Research_Fund.type = 2 THEN 'บริการวิชาการ' ELSE '-' END AS type_name
FROM            dbo.Research_Fund INNER JOIN
                         itjobs.dbo.[user] ON dbo.Research_Fund.user_id = itjobs.dbo.[user].user_id INNER JOIN
                         itjobs.dbo.title_technical ON itjobs.dbo.[user].title_technicalID = itjobs.dbo.title_technical.title_technicalID INNER JOIN
                         itjobs.dbo.title ON itjobs.dbo.[user].title_id = itjobs.dbo.title.title_id"


        If Not String.IsNullOrEmpty(ddlYearFund1.SelectedValue) Then
            SQLRN &= " WHERE        Research_Fund.year = @year "

        End If

        SQLRN &= " ORDER BY Research_Fund.no DESC "
        Dim parameters As SqlParameter() = {
                    New SqlParameter("@year", ddlYearFund1.SelectedValue)
                                                   }
        Dim dt As DataTable = QueryDataTable2(SQLRN, dbConn, "Dashboard", parameters)
        If dt.Rows.Count > 0 Then
            GridViewFund.DataSource = dt
            GridViewFund.DataBind()

            panelUpFund.Visible = False
        End If
    End Sub
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        GridViewFund.PageIndex = e.NewPageIndex
    End Sub
    Protected Sub Add_Click(sender As Object, e As EventArgs) Handles addFund.Click
        'txtTitle.Text = ""
        'txtTitle.Attributes("placeholder") = "ระบุชื่อของข้อมูล"

        'txtAuthors.Text = ""
        'txtAuthors.Attributes("placeholder") = "ระบุชื่อผู้แต่ง"

        'txtScopus.Text = ""
        'txtScopus.Attributes("placeholder") = "ระบุ Scopus"

        'txtTCI.Text = ""
        'txtTCI.Attributes("placeholder") = "ระบุ TCI"

        'txtVolume.Text = ""
        'txtVolume.Attributes("placeholder") = "ระบุ Volume"

        'txtIssue.Text = ""
        'txtIssue.Attributes("placeholder") = "ระบุ Issue"

        'txtPages.Text = ""
        'txtPages.Attributes("placeholder") = "ระบุ Pages"

        'txtDOI.Text = ""
        'txtDOI.Attributes("placeholder") = "ระบุ DOI"

        'ddlYearFund2.SelectedIndex = 0
        'ddlType.SelectedIndex = 0

        'cblProject.Items.Clear()

        panelFund.Visible = False
        panelUpFund.Visible = True
        btnsubmitFund.Visible = True
        btnupdateFund.Visible = False
    End Sub
    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim btnEdit As LinkButton = TryCast(sender, LinkButton)
        Dim rowIndex As Integer = Convert.ToInt32(btnEdit.CommandArgument)
        Dim NO As Integer = Convert.ToInt32(GridViewFund.DataKeys(rowIndex).Value)
        hfFundNo.Value = NO.ToString()
        LabelFund.Text = NO.ToString()
        'LoadYearFund()

        Dim SQLRN As String = "SELECT        no, year, user_id, title, Fund_ID, type, dept_id, stDate, fnDate, GrantExtDate, amount
    FROM            Research_Fund
    WHERE (no = " & NO & ")"
        Dim dt As DataTable = QueryDataTable2(SQLRN, dbConn, "Dashboard", Nothing)

        If dt.Rows.Count > 0 Then

            txtyear.Text = dt.Rows(0)("year")
            Dim selectedtype As String = dt.Rows(0)("type").ToString()
            If ddlType.Items.FindByValue(selectedtype) IsNot Nothing Then
                ddlType.SelectedValue = selectedtype
            End If
            Dim selectedUser As String = dt.Rows(0)("user_id").ToString()
            If ddlUserFund.Items.FindByValue(selectedUser) IsNot Nothing Then
                ddlUserFund.SelectedValue = selectedUser
                LoadDeptFund(selectedUser)
            End If
            'Dim selectedDept As String = dt.Rows(0)("dept_id").ToString()
            'If ddlDept.Items.FindByValue(selectedDept) IsNot Nothing Then
            '    ddlDept.SelectedValue = selectedDept
            'End If
            Dim selectedddlSource As String = dt.Rows(0)("Fund_ID").ToString()
            If ddlSource.Items.FindByValue(selectedddlSource) IsNot Nothing Then
                ddlSource.SelectedValue = selectedddlSource
            End If
            txtTitle.Text = dt.Rows(0)("title")
            If Not IsDBNull(dt.Rows(0)("stDate")) Then
                txtStartDate.Value = Convert.ToDateTime(dt.Rows(0)("stDate")).ToString("dd MMM, yyyy")
            End If

            If Not IsDBNull(dt.Rows(0)("fnDate")) Then
                txtEndDate.Value = Convert.ToDateTime(dt.Rows(0)("fnDate")).ToString("dd MMM, yyyy")
            End If

            If Not IsDBNull(dt.Rows(0)("GrantExtDate")) Then
                txtExtendDate.Value = Convert.ToDateTime(dt.Rows(0)("GrantExtDate")).ToString("dd MMM, yyyy")
            End If
            If Not IsDBNull(dt.Rows(0)("amount")) Then
                Dim amt As Decimal = Convert.ToDecimal(dt.Rows(0)("amount"))
                txtamount.Text = amt.ToString("#,##0.00")
            End If


        End If

        panelFund.Visible = False
        panelUpFund.Visible = True
        btnupdateFund.Visible = True
        btnsubmitFund.Visible = False
    End Sub
    'Private Sub SetCheckedKPI(kpiString As String)

    '    If String.IsNullOrEmpty(kpiString) Then Exit Sub

    '    Dim selectedKPI As String() = kpiString.Split(","c)

    '    For Each item As ListItem In cblProject.Items
    '        If selectedKPI.Contains(item.Value.Trim()) Then
    '            item.Selected = True
    '        End If
    '    Next

    'End Sub

    Protected Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        panelFund.Visible = True
        BindgridFund()
    End Sub
    'Protected Sub btnsubmitFund_Click(sender As Object, e As EventArgs) Handles btnsubmitFund.Click

    '    Dim year As Integer = Convert.ToInt32(ddlYearFund2.SelectedValue)
    '    Dim FundType As Integer = Convert.ToInt32(ddlType.SelectedValue)
    '    Dim title As String = txtTitle.Text.Trim()
    '    Dim authors As String = txtAuthors.Text.Trim()
    '    Dim scopus As String = txtScopus.Text.Trim()
    '    Dim tci As String = txtTCI.Text.Trim()
    '    Dim volume As String = txtVolume.Text.Trim()
    '    Dim issue As String = txtIssue.Text.Trim()
    '    Dim pages As String = txtPages.Text.Trim()
    '    Dim doi As String = txtDOI.Text.Trim()

    '    '==== รวม KPI ที่เลือก ====
    '    Dim kpiList As New List(Of String)
    '    For Each item As ListItem In cblProject.Items
    '        If item.Selected Then
    '            kpiList.Add(item.Value)
    '        End If
    '    Next
    '    Dim kpiString As String = String.Join(",", kpiList)


    '    Dim constr As String = WebConfigurationManager.ConnectionStrings("dbConn_Dashboard").ConnectionString
    '    constr = Replace(constr, "password", Decrypt2("2fxKF+rsggSR+BM25c3IJLWBqS1Pu4Y5"))

    '    Dim SQLRN As String = "SELECT  ISNULL(MAX(no), 0) AS newNo FROM Research_Fund"
    '    Dim dt As DataTable = QueryDataTable2(SQLRN, dbConn, "Dashboard", Nothing)
    '    If dt.Rows.Count > 0 Then
    '        Dim newNo As Integer = Convert.ToInt32(dt.Rows(0)("newNo")) + 1
    '        Dim insertSql As String = "INSERT INTO Research_Fund
    '    (no, year, month, type, title, authors, scopus_source, TCI, Volume, Issue, Pages, DOI, KPI, inputDate)
    '    VALUES
    '    (@no, @year, MONTH(GETDATE()), @type, @title, @authors, @scopus, @tci, @volume, @issue, @pages, @doi, @kpi, GETDATE())"

    '        Using con As New SqlConnection(constr)
    '            Using cmd As New SqlCommand(insertSql, con)

    '                cmd.Parameters.AddWithValue("@no", newNo)
    '                cmd.Parameters.AddWithValue("@year", year)
    '                cmd.Parameters.AddWithValue("@type", FundType)
    '                cmd.Parameters.AddWithValue("@title", title)
    '                cmd.Parameters.AddWithValue("@authors", authors)
    '                cmd.Parameters.AddWithValue("@scopus", If(String.IsNullOrEmpty(scopus), DBNull.Value, scopus))
    '                cmd.Parameters.AddWithValue("@tci", If(String.IsNullOrEmpty(tci), DBNull.Value, tci))
    '                cmd.Parameters.AddWithValue("@volume", If(String.IsNullOrEmpty(volume), DBNull.Value, volume))
    '                cmd.Parameters.AddWithValue("@issue", If(String.IsNullOrEmpty(issue), DBNull.Value, issue))
    '                cmd.Parameters.AddWithValue("@pages", If(String.IsNullOrEmpty(pages), DBNull.Value, pages))
    '                cmd.Parameters.AddWithValue("@doi", If(String.IsNullOrEmpty(doi), DBNull.Value, doi))
    '                cmd.Parameters.AddWithValue("@kpi", If(String.IsNullOrEmpty(kpiString), DBNull.Value, kpiString))

    '                con.Open()
    '                cmd.ExecuteNonQuery()
    '            End Using
    '        End Using
    '    End If
    '    panelFund.Visible = True
    '    BindgridFund()
    'End Sub

    'Protected Sub BtnupdateFund_Click(sender As Object, e As EventArgs) Handles btnupdateFund.Click

    '    If String.IsNullOrEmpty(hfFundNo.Value) Then Exit Sub

    '    Dim NO As Integer = Convert.ToInt32(hfFundNo.Value)
    '    Dim year As Integer = Convert.ToInt32(ddlYearFund2.SelectedValue)
    '    Dim type As Integer = Convert.ToInt32(ddlType.SelectedValue)

    '    ' รวม KPI ที่เลือกเป็น 1,2,3
    '    Dim kpiList As New List(Of String)
    '    For Each item As ListItem In cblProject.Items
    '        If item.Selected Then
    '            kpiList.Add(item.Value)
    '        End If
    '    Next
    '    Dim kpiString As String = String.Join(",", kpiList)

    '    Dim constr As String = WebConfigurationManager.ConnectionStrings("dbConn_Dashboard").ConnectionString
    '    constr = Replace(constr, "password", Decrypt2("2fxKF+rsggSR+BM25c3IJLWBqS1Pu4Y5"))

    '    Dim query As String = "UPDATE Research_Fund SET 
    '    year=@year,
    '    type=@type,
    '    title=@title,
    '    authors=@authors,
    '    scopus_source=@scopus,
    '    TCI=@tci,
    '    Volume=@vol,
    '    Issue=@issue,
    '    Pages=@pages,
    '    DOI=@doi,
    '    KPI=@kpi,
    '    editDate = GETDATE()
    '    WHERE no=@no"

    '    Using con As New SqlConnection(constr)
    '        Using cmd As New SqlCommand(query, con)

    '            cmd.Parameters.AddWithValue("@no", NO)
    '            cmd.Parameters.AddWithValue("@year", year)
    '            cmd.Parameters.AddWithValue("@type", type)
    '            cmd.Parameters.AddWithValue("@title", txtTitle.Text.Trim())
    '            cmd.Parameters.AddWithValue("@authors", txtAuthors.Text.Trim())

    '            cmd.Parameters.AddWithValue("@scopus", If(String.IsNullOrWhiteSpace(txtScopus.Text), DBNull.Value, txtScopus.Text))
    '            cmd.Parameters.AddWithValue("@tci", If(String.IsNullOrWhiteSpace(txtTCI.Text), DBNull.Value, txtTCI.Text))
    '            cmd.Parameters.AddWithValue("@vol", If(String.IsNullOrWhiteSpace(txtVolume.Text), DBNull.Value, txtVolume.Text))
    '            cmd.Parameters.AddWithValue("@issue", If(String.IsNullOrWhiteSpace(txtIssue.Text), DBNull.Value, txtIssue.Text))
    '            cmd.Parameters.AddWithValue("@pages", If(String.IsNullOrWhiteSpace(txtPages.Text), DBNull.Value, txtPages.Text))
    '            cmd.Parameters.AddWithValue("@doi", If(String.IsNullOrWhiteSpace(txtDOI.Text), DBNull.Value, txtDOI.Text))
    '            cmd.Parameters.AddWithValue("@kpi", If(String.IsNullOrEmpty(kpiString), DBNull.Value, kpiString))

    '            con.Open()
    '            cmd.ExecuteNonQuery()
    '        End Using
    '    End Using

    '    ' รีเฟรชหน้าหลัก
    '    panelUpFund.Visible = False
    '    panelFund.Visible = True
    '    BindgridFund()

    'End Sub

End Class
