Imports System.Web.Configuration
Imports Dashboard.ConnectDB
Imports Dashboard.Encrypt
Imports System.Data.SqlClient

Public Class Up_researchFund
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Me.IsPostBack Then
            BindgridFund()
            'LoadYearFund()
            LoadYearFilterFund()
            ddlNameuserFund()
            BindFundSource()
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
        Dim SQLRN As String = "SELECT  dbo.Research_Fund.no, dbo.Research_Fund.year, dbo.Research_Fund.user_id, dbo.Research_Fund.title, dbo.Research_Fund.type, CASE WHEN itjobs.dbo.title_technical.title_technicalName IS NULL 
                         THEN itjobs.dbo.title.title_name + itjobs.dbo.[user].fname + SPACE(2) + itjobs.dbo.[user].lname ELSE itjobs.dbo.title_technical.title_technicalName + itjobs.dbo.[user].fname + SPACE(2) + itjobs.dbo.[user].lname END AS fullname, 
                         CASE WHEN dbo.Research_Fund.type = 1 THEN 'ทุนวิจัย' WHEN dbo.Research_Fund.type = 2 THEN 'บริการวิชาการ' ELSE '-' END AS type_name
FROM            dbo.Research_Fund INNER JOIN
                         itjobs.dbo.[user] ON dbo.Research_Fund.user_id = itjobs.dbo.[user].user_id LEFT OUTER JOIN
                         itjobs.dbo.title ON itjobs.dbo.[user].title_id = itjobs.dbo.title.title_id LEFT OUTER JOIN
                         itjobs.dbo.title_technical ON itjobs.dbo.[user].title_technicalID = itjobs.dbo.title_technical.title_technicalID"


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
            panelUpsource.Visible = False
        End If
    End Sub
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        GridViewFund.PageIndex = e.NewPageIndex

        GridViewsource.PageIndex = e.NewPageIndex
        BindFundSource()
    End Sub
    Protected Sub Add_Click(sender As Object, e As EventArgs) Handles addFund.Click
        txtTitle.Text = ""
        txtTitle.Attributes("placeholder") = "ระบุชื่อโครงการ"

        txtyear.Text = ""
        txtyear.Attributes("placeholder") = "ปีงบประมาณ"

        ddlType.SelectedIndex = 0
        ddlUserFund.SelectedIndex = 0
        LoadDeptFund(0)
        ddlSource.SelectedIndex = 0

        txtStartDate.Value = ""
        txtStartDate.Attributes("placeholder") = "Start Date"

        txtEndDate.Value = ""
        txtEndDate.Attributes("placeholder") = "End Date"

        txtExtendDate.Value = ""
        txtExtendDate.Attributes("placeholder") = "Extend Date"

        txtamount.Text = ""
        txtamount.Attributes("placeholder") = "ระบุจำนวนเงินทุน"


        panelFund.Visible = False
        panelUpFund.Visible = True
        btnsubmitFund.Visible = True
        btnupdateFund.Visible = False
        panelUpsource.Visible = False
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
        panelUpsource.Visible = False
    End Sub

    Protected Sub BtnCancelFundl_Click(sender As Object, e As EventArgs) Handles btnCancelFund.Click
        panelFund.Visible = True
        panelUpsource.Visible = False
        panelUpFund.Visible = False
        BindgridFund()
    End Sub

    Protected Sub btnsubmitFund_Click(sender As Object, e As EventArgs) Handles btnsubmitFund.Click


        Dim constr As String = WebConfigurationManager.ConnectionStrings("dbConn_Dashboard").ConnectionString
        constr = Replace(constr, "password", Decrypt2("2fxKF+rsggSR+BM25c3IJLWBqS1Pu4Y5"))

        Dim SQLRN As String = "SELECT  ISNULL(MAX(no), 0) AS newNo FROM Research_Fund"
        Dim dt As DataTable = QueryDataTable2(SQLRN, dbConn, "Dashboard", Nothing)
        If dt.Rows.Count > 0 Then
            Dim newNo As Integer = Convert.ToInt32(dt.Rows(0)("newNo")) + 1
            Dim query As String = "INSERT INTO Research_Fund
        (no, year, user_id, title, Fund_ID, type, dept_id, stDate, fnDate, GrantExtDate, amount, inputDate)
        VALUES
        (@no, @year, @user_id, @title, @Fund_ID, @type, @dept_id, @stDate, @fnDate, @GrantExtDate, @amount, GETDATE())"

            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand(query, con)
                    cmd.Parameters.AddWithValue("@no", newNo)
                    cmd.Parameters.AddWithValue("@year", txtyear.Text.Trim())
                    cmd.Parameters.AddWithValue("@user_id", ddlUserFund.SelectedValue)
                    cmd.Parameters.AddWithValue("@title", txtTitle.Text.Trim())
                    cmd.Parameters.AddWithValue("@Fund_ID", ddlSource.SelectedValue)
                    cmd.Parameters.AddWithValue("@type", ddlType.SelectedValue)
                    cmd.Parameters.AddWithValue("@dept_id", ddlDept.SelectedValue)

                    If String.IsNullOrWhiteSpace(txtStartDate.Value) Then
                        cmd.Parameters.AddWithValue("@stDate", DBNull.Value)
                    Else
                        cmd.Parameters.AddWithValue("@stDate", DateTime.Parse(txtStartDate.Value))
                    End If

                    If String.IsNullOrWhiteSpace(txtEndDate.Value) Then
                        cmd.Parameters.AddWithValue("@fnDate", DBNull.Value)
                    Else
                        cmd.Parameters.AddWithValue("@fnDate", DateTime.Parse(txtEndDate.Value))
                    End If

                    If String.IsNullOrWhiteSpace(txtExtendDate.Value) Then
                        cmd.Parameters.AddWithValue("@GrantExtDate", DBNull.Value)
                    Else
                        cmd.Parameters.AddWithValue("@GrantExtDate", DateTime.Parse(txtExtendDate.Value))
                    End If

                    Dim amt As Decimal = 0
                    Decimal.TryParse(txtamount.Text.Replace(",", ""), amt)
                    cmd.Parameters.AddWithValue("@amount", amt)

                    con.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using




        End If
        ' กลับหน้า Grid
        panelUpFund.Visible = False
        panelUpsource.Visible = False
        panelFund.Visible = True
        BindgridFund()
    End Sub

    Protected Sub BtnupdateFund_Click(sender As Object, e As EventArgs) Handles btnupdateFund.Click

        If String.IsNullOrEmpty(hfFundNo.Value) Then Exit Sub

        Dim NO As Integer = Convert.ToInt32(hfFundNo.Value)
        Dim type As Integer = Convert.ToInt32(ddlType.SelectedValue)


        Dim constr As String = WebConfigurationManager.ConnectionStrings("dbConn_Dashboard").ConnectionString
        constr = Replace(constr, "password", Decrypt2("2fxKF+rsggSR+BM25c3IJLWBqS1Pu4Y5"))


        Dim query As String = "UPDATE Research_Fund SET 
        year = @year,
        user_id = @user_id,
        title = @title,
        Fund_ID = @Fund_ID,
        type = @type,
        dept_id = @dept_id,
        stDate = @stDate,
        fnDate = @fnDate,
        GrantExtDate = @GrantExtDate,
        amount = @amount,
        editDate = GETDATE()
        WHERE no = @no"

        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand(query, con)

                ' 🔢 year
                cmd.Parameters.AddWithValue("@year", txtyear.Text.Trim())

                ' 👤 หัวหน้าโครงการ
                cmd.Parameters.AddWithValue("@user_id", ddlUserFund.SelectedValue)

                ' 🏷 ชื่อโครงการ
                cmd.Parameters.AddWithValue("@title", txtTitle.Text.Trim())

                ' 💰 แหล่งทุน
                cmd.Parameters.AddWithValue("@Fund_ID", ddlSource.SelectedValue)

                ' 📂 ประเภทงาน
                cmd.Parameters.AddWithValue("@type", ddlType.SelectedValue)

                ' 🏢 หน่วยงาน
                cmd.Parameters.AddWithValue("@dept_id", ddlDept.SelectedValue)

                ' 📅 วันที่เริ่ม
                If String.IsNullOrWhiteSpace(txtStartDate.Value) Then
                    cmd.Parameters.AddWithValue("@stDate", DBNull.Value)
                Else
                    cmd.Parameters.AddWithValue("@stDate", DateTime.Parse(txtStartDate.Value))
                End If

                ' 📅 วันที่สิ้นสุด
                If String.IsNullOrWhiteSpace(txtEndDate.Value) Then
                    cmd.Parameters.AddWithValue("@fnDate", DBNull.Value)
                Else
                    cmd.Parameters.AddWithValue("@fnDate", DateTime.Parse(txtEndDate.Value))
                End If

                ' 📅 ขยายเวลา
                If String.IsNullOrWhiteSpace(txtExtendDate.Value) Then
                    cmd.Parameters.AddWithValue("@GrantExtDate", DBNull.Value)
                Else
                    cmd.Parameters.AddWithValue("@GrantExtDate", DateTime.Parse(txtExtendDate.Value))
                End If

                ' 💵 จำนวนเงิน (ตัด comma ออกก่อน)
                Dim amt As Decimal = 0
                Decimal.TryParse(txtamount.Text.Replace(",", ""), amt)
                cmd.Parameters.AddWithValue("@amount", amt)

                ' 🔑 primary key
                cmd.Parameters.AddWithValue("@no", NO)

                con.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
        ' รีเฟรชหน้าหลัก
        panelUpFund.Visible = False
        panelUpsource.Visible = False
        panelFund.Visible = True
        BindgridFund()

    End Sub

    Protected Sub addFundsource_Click(sender As Object, e As EventArgs) Handles addFundsource.Click
        panelFund.Visible = False
        panelUpFund.Visible = False
        panelUpsource.Visible = True
        BindFundSource()
    End Sub
    Protected Sub rblFundType_SelectedIndexChanged(sender As Object, e As EventArgs)
        GridViewsource.PageIndex = 0 ' รีเซ็ตกลับหน้าแรก
        BindFundSource()
    End Sub
    Private Sub BindFundSource()

        Dim SQLRN As String = "SELECT Fund_ID as no, Fund_source, Fund_type,
        CASE WHEN Fund_type = 1 THEN 'ภายใน'
             WHEN Fund_type = 2 THEN 'ภายนอก'
             ELSE '-' END AS type_name
        FROM FundType WHERE 1=1 "

        If rblFundType.SelectedValue <> "0" Then
            SQLRN &= " AND Fund_type = @type"

        End If
        SQLRN &= " ORDER BY no DESC"
        Dim parameters As SqlParameter() = {
                    New SqlParameter("@type", rblFundType.SelectedValue)
                                                   }
        Dim dt As DataTable = QueryDataTable2(SQLRN, dbConn, "Dashboard", parameters)


        If dt.Rows.Count > 0 Then

            GridViewsource.DataSource = dt
            GridViewsource.DataBind()
        End If





    End Sub
    Protected Sub GridViewsource_RowCommand(sender As Object, e As GridViewCommandEventArgs)

        If e.CommandName = "AddNew" Then

            Dim txtSource As TextBox = CType(GridViewsource.FooterRow.FindControl("txtNewSource"), TextBox)
            Dim ddlType As DropDownList = CType(GridViewsource.FooterRow.FindControl("ddlNewType"), DropDownList)


            Dim constr As String = WebConfigurationManager.ConnectionStrings("dbConn_Dashboard").ConnectionString
            constr = Replace(constr, "password", Decrypt2("2fxKF+rsggSR+BM25c3IJLWBqS1Pu4Y5"))

            Dim SQLRN As String = "SELECT  ISNULL(MAX(Fund_ID), 0) AS newNo FROM FundType"
            Dim dt As DataTable = QueryDataTable2(SQLRN, dbConn, "Dashboard", Nothing)
            If dt.Rows.Count > 0 Then
                Dim newNo As Integer = Convert.ToInt32(dt.Rows(0)("newNo")) + 1
                Using conn As New SqlConnection(constr)
                    Using cmd As New SqlCommand("INSERT INTO FundType (Fund_ID, Fund_source, Fund_type) VALUES (@fund_ID, @source, @type)", conn)
                        cmd.Parameters.AddWithValue("@fund_ID", newNo)
                        cmd.Parameters.AddWithValue("@source", txtSource.Text.Trim())
                        cmd.Parameters.AddWithValue("@type", ddlType.SelectedValue)
                        conn.Open()
                        cmd.ExecuteNonQuery()
                    End Using
                End Using

                BindFundSource()
            End If
        End If

    End Sub
    Protected Sub GridViewsource_RowEditing(sender As Object, e As GridViewEditEventArgs)
        GridViewsource.EditIndex = e.NewEditIndex
        BindFundSource()
    End Sub
    Protected Sub GridViewsource_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs)
        GridViewsource.EditIndex = -1
        BindFundSource()
    End Sub
    Protected Sub GridViewsource_RowUpdating(sender As Object, e As GridViewUpdateEventArgs)

        Dim no As Integer = Convert.ToInt32(GridViewsource.DataKeys(e.RowIndex).Value)

        Dim row As GridViewRow = GridViewsource.Rows(e.RowIndex)
        Dim txtSource As TextBox = CType(row.FindControl("txtEditSource"), TextBox)
        Dim ddlType As DropDownList = CType(row.FindControl("ddlEditType"), DropDownList)


        Dim constr As String = WebConfigurationManager.ConnectionStrings("dbConn_Dashboard").ConnectionString
        constr = Replace(constr, "password", Decrypt2("2fxKF+rsggSR+BM25c3IJLWBqS1Pu4Y5"))

        Using conn As New SqlConnection(constr)
            Using cmd As New SqlCommand("UPDATE FundType SET Fund_source=@source, Fund_type=@type WHERE Fund_ID=@no", conn)
                cmd.Parameters.AddWithValue("@source", txtSource.Text.Trim())
                cmd.Parameters.AddWithValue("@type", ddlType.SelectedValue)
                cmd.Parameters.AddWithValue("@no", no)
                conn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using

        GridViewsource.EditIndex = -1
        BindFundSource()

    End Sub

    Protected Sub btnCancelsource_Click(sender As Object, e As EventArgs) Handles btnCancelsource.Click
        panelFund.Visible = True
        panelUpsource.Visible = False
        panelUpFund.Visible = False
        BindgridFund()
    End Sub
End Class
