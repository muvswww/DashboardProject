Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports Dashboard.ConnectDB
Imports Dashboard.Encrypt
Public Class Up_research
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Me.IsPostBack Then
            BindgridPub()
            LoadYearPub()
            LoadYearFilterPub()
        End If

    End Sub
    Private Sub LoadYearPub()
        Dim SQLRN As String = "SELECT DISTINCT Strategy_Year FROM MasterProject ORDER BY Strategy_Year DESC"
        Dim dt As DataTable = QueryDataTable2(SQLRN, dbConn, "Dashboard", Nothing)

        ddlYearPub2.Items.Clear()
        ddlYearPub2.Items.Add(New ListItem("-- เลือกปี --", ""))

        For Each dr As DataRow In dt.Rows
            ddlYearPub2.Items.Add(New ListItem(dr("Strategy_Year").ToString(), dr("Strategy_Year").ToString()))
        Next
    End Sub
    Protected Sub ddlYearPub2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlYearPub2.SelectedIndexChanged
        LoadProjectsPub()
    End Sub


    Private Sub LoadProjectsPub()
        If String.IsNullOrEmpty(ddlYearPub2.SelectedValue) Then
            cblProject.Items.Clear()
            Exit Sub
        End If

        Dim selectedYear As Integer = Convert.ToInt32(ddlYearPub2.SelectedValue)


        Dim SQLRN As String = "SELECT Strategy_id, Project_no,Project_id, ProjectName " &
                        "FROM MasterProject " &
                        "WHERE Strategy_Year = @year AND Strategy_id = 1"

        Dim parameters As SqlParameter() = {
                    New SqlParameter("@year", selectedYear)
                                                   }
        Dim dt As DataTable = QueryDataTable2(SQLRN, dbConn, "Dashboard", parameters)
        If dt.Rows.Count > 0 Then
            cblProject.Items.Clear()

            For Each dr As DataRow In dt.Rows
                Dim li As New ListItem()
                li.Text = dr("ProjectName").ToString()
                li.Value = dr("Project_no").ToString()
                cblProject.Items.Add(li)
            Next
        End If

    End Sub
    Private Sub LoadYearFilterPub()
        Dim SQLRN As String = "SELECT DISTINCT year FROM Research_Pub ORDER BY year DESC"
        Dim dt As DataTable = QueryDataTable2(SQLRN, dbConn, "Dashboard", Nothing)

        ddlYearPub1.Items.Clear()
        ddlYearPub1.Items.Add(New ListItem("ทุกปี", "")) ' ค่าว่าง = แสดงทั้งหมด

        For Each dr As DataRow In dt.Rows
            ddlYearPub1.Items.Add(New ListItem(dr("year").ToString(), dr("year").ToString()))
        Next
    End Sub
    Protected Sub ddlYearPub1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlYearPub1.SelectedIndexChanged
        BindgridPub()
    End Sub
    Private Sub BindgridPub()
        Dim SQLRN As String = "SELECT        no, year, CASE WHEN type = 1 THEN 'ผลงานวิจัยระดับชาติ' WHEN type = 2 THEN 'ผลงานวิจัยระดับนานาชาติ' END AS type, title
FROM            Research_Pub "


        If Not String.IsNullOrEmpty(ddlYearPub1.SelectedValue) Then
            SQLRN &= " WHERE year = @year "

        End If

        SQLRN &= " ORDER BY no DESC "
        Dim parameters As SqlParameter() = {
                    New SqlParameter("@year", ddlYearPub1.SelectedValue)
                                                   }
        Dim dt As DataTable = QueryDataTable2(SQLRN, dbConn, "Dashboard", parameters)
        If dt.Rows.Count > 0 Then
            GridViewPub.DataSource = dt
            GridViewPub.DataBind()

            panelUpPub.Visible = False
        End If
    End Sub
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        GridViewPub.PageIndex = e.NewPageIndex
    End Sub
    Protected Sub Add_Click(sender As Object, e As EventArgs) Handles addPub.Click
        txtTitle.Text = ""
        txtTitle.Attributes("placeholder") = "ระบุชื่อของข้อมูล"

        txtAuthors.Text = ""
        txtAuthors.Attributes("placeholder") = "ระบุชื่อผู้แต่ง"

        txtScopus.Text = ""
        txtScopus.Attributes("placeholder") = "ระบุ Scopus"

        txtTCI.Text = ""
        txtTCI.Attributes("placeholder") = "ระบุ TCI"

        txtVolume.Text = ""
        txtVolume.Attributes("placeholder") = "ระบุ Volume"

        txtIssue.Text = ""
        txtIssue.Attributes("placeholder") = "ระบุ Issue"

        txtPages.Text = ""
        txtPages.Attributes("placeholder") = "ระบุ Pages"

        txtDOI.Text = ""
        txtDOI.Attributes("placeholder") = "ระบุ DOI"

        ddlYearPub2.SelectedIndex = 0
        ddlType.SelectedIndex = 0

        cblProject.Items.Clear()

        panelPub.Visible = False
        panelUpPub.Visible = True
        btnsubmitPub.Visible = True
        btnupdatePub.Visible = False
    End Sub
    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim btnEdit As LinkButton = TryCast(sender, LinkButton)
        Dim rowIndex As Integer = Convert.ToInt32(btnEdit.CommandArgument)
        Dim NO As Integer = Convert.ToInt32(GridViewPub.DataKeys(rowIndex).Value)
        hfPubNo.Value = NO.ToString()
        LoadYearPub()
        'hfNewId.Value = newId.ToString()
        'Labelpub.Text = NO.ToString()

        Dim SQLRN As String = "SELECT       no, year, month, type, title, authors, scopus_source, TCI, Volume, Issue, Pages, DOI, KPI, inputDate
FROM            Research_Pub
WHERE (no = " & NO & ")"
        Dim dt As DataTable = QueryDataTable2(SQLRN, dbConn, "Dashboard", Nothing)

        If dt.Rows.Count > 0 Then
            Dim selectedYear As String = dt.Rows(0)("year").ToString()
            If ddlYearPub2.Items.FindByValue(selectedYear) IsNot Nothing Then
                ddlYearPub2.SelectedValue = selectedYear
            End If
            Dim selectedValue As String = dt.Rows(0)("type").ToString()
            ddlType.SelectedValue = selectedValue
            txtTitle.Text = dt.Rows(0)("title")
            txtAuthors.Text = dt.Rows(0)("authors")
            If dt.Rows(0)("scopus_source") Is DBNull.Value Then
                txtScopus.Text = "" ' กำหนดให้เป็นข้อความว่าง
            Else
                txtScopus.Text = dt.Rows(0)("scopus_source")
            End If
            If dt.Rows(0)("TCI") Is DBNull.Value Then
                txtTCI.Text = "" ' กำหนดให้เป็นข้อความว่าง
            Else
                txtTCI.Text = dt.Rows(0)("TCI")
            End If
            If dt.Rows(0)("Volume") Is DBNull.Value Then
                txtVolume.Text = "" ' กำหนดให้เป็นข้อความว่าง
            Else
                txtVolume.Text = dt.Rows(0)("Volume")
            End If
            If dt.Rows(0)("Issue") Is DBNull.Value Then
                txtIssue.Text = "" ' กำหนดให้เป็นข้อความว่าง
            Else
                txtIssue.Text = dt.Rows(0)("Issue")
            End If
            If dt.Rows(0)("Pages") Is DBNull.Value Then
                txtPages.Text = "" ' กำหนดให้เป็นข้อความว่าง
            Else
                txtPages.Text = dt.Rows(0)("Pages")
            End If
            If dt.Rows(0)("DOI") Is DBNull.Value Then
                txtDOI.Text = "" ' กำหนดให้เป็นข้อความว่าง
            Else
                txtDOI.Text = dt.Rows(0)("DOI")
            End If
            LoadProjectsPub()

            SetCheckedKPI(dt.Rows(0)("KPI").ToString())
        End If

        panelPub.Visible = False
        panelUpPub.Visible = True
        btnupdatePub.Visible = True
        btnsubmitPub.Visible = False
    End Sub
    Private Sub SetCheckedKPI(kpiString As String)

        If String.IsNullOrEmpty(kpiString) Then Exit Sub

        Dim selectedKPI As String() = kpiString.Split(","c)

        For Each item As ListItem In cblProject.Items
            If selectedKPI.Contains(item.Value.Trim()) Then
                item.Selected = True
            End If
        Next

    End Sub

    Protected Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        panelPub.Visible = True
        BindgridPub()
    End Sub
    Protected Sub btnsubmitPub_Click(sender As Object, e As EventArgs) Handles btnsubmitPub.Click

        Dim year As Integer = Convert.ToInt32(ddlYearPub2.SelectedValue)
        Dim pubType As Integer = Convert.ToInt32(ddlType.SelectedValue)
        Dim title As String = txtTitle.Text.Trim()
        Dim authors As String = txtAuthors.Text.Trim()
        Dim scopus As String = txtScopus.Text.Trim()
        Dim tci As String = txtTCI.Text.Trim()
        Dim volume As String = txtVolume.Text.Trim()
        Dim issue As String = txtIssue.Text.Trim()
        Dim pages As String = txtPages.Text.Trim()
        Dim doi As String = txtDOI.Text.Trim()

        '==== รวม KPI ที่เลือก ====
        Dim kpiList As New List(Of String)
        For Each item As ListItem In cblProject.Items
            If item.Selected Then
                kpiList.Add(item.Value)
            End If
        Next
        Dim kpiString As String = String.Join(",", kpiList)


        Dim constr As String = WebConfigurationManager.ConnectionStrings("dbConn_Dashboard").ConnectionString
        constr = Replace(constr, "password", Decrypt2("2fxKF+rsggSR+BM25c3IJLWBqS1Pu4Y5"))

        Dim SQLRN As String = "SELECT  ISNULL(MAX(no), 0) AS newNo FROM Research_Pub"
        Dim dt As DataTable = QueryDataTable2(SQLRN, dbConn, "Dashboard", Nothing)
        If dt.Rows.Count > 0 Then
            Dim newNo As Integer = Convert.ToInt32(dt.Rows(0)("newNo")) + 1
            Dim insertSql As String = "INSERT INTO Research_Pub
        (no, year, month, type, title, authors, scopus_source, TCI, Volume, Issue, Pages, DOI, KPI, inputDate)
        VALUES
        (@no, @year, MONTH(GETDATE()), @type, @title, @authors, @scopus, @tci, @volume, @issue, @pages, @doi, @kpi, GETDATE())"

            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand(insertSql, con)

                    cmd.Parameters.AddWithValue("@no", newNo)
                    cmd.Parameters.AddWithValue("@year", year)
                    cmd.Parameters.AddWithValue("@type", pubType)
                    cmd.Parameters.AddWithValue("@title", title)
                    cmd.Parameters.AddWithValue("@authors", authors)
                    cmd.Parameters.AddWithValue("@scopus", If(String.IsNullOrEmpty(scopus), DBNull.Value, scopus))
                    cmd.Parameters.AddWithValue("@tci", If(String.IsNullOrEmpty(tci), DBNull.Value, tci))
                    cmd.Parameters.AddWithValue("@volume", If(String.IsNullOrEmpty(volume), DBNull.Value, volume))
                    cmd.Parameters.AddWithValue("@issue", If(String.IsNullOrEmpty(issue), DBNull.Value, issue))
                    cmd.Parameters.AddWithValue("@pages", If(String.IsNullOrEmpty(pages), DBNull.Value, pages))
                    cmd.Parameters.AddWithValue("@doi", If(String.IsNullOrEmpty(doi), DBNull.Value, doi))
                    cmd.Parameters.AddWithValue("@kpi", If(String.IsNullOrEmpty(kpiString), DBNull.Value, kpiString))

                    con.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End If



        UpdatePub_MasterProject(year)

        panelPub.Visible = True
        BindgridPub()
    End Sub

    Protected Sub BtnupdatePub_Click(sender As Object, e As EventArgs) Handles btnupdatePub.Click

        If String.IsNullOrEmpty(hfPubNo.Value) Then Exit Sub

        Dim NO As Integer = Convert.ToInt32(hfPubNo.Value)
        Dim year As Integer = Convert.ToInt32(ddlYearPub2.SelectedValue)
        Dim type As Integer = Convert.ToInt32(ddlType.SelectedValue)

        ' รวม KPI ที่เลือกเป็น 1,2,3
        Dim kpiList As New List(Of String)
        For Each item As ListItem In cblProject.Items
            If item.Selected Then
                kpiList.Add(item.Value)
            End If
        Next
        Dim kpiString As String = String.Join(",", kpiList)

        Dim constr As String = WebConfigurationManager.ConnectionStrings("dbConn_Dashboard").ConnectionString
        constr = Replace(constr, "password", Decrypt2("2fxKF+rsggSR+BM25c3IJLWBqS1Pu4Y5"))

        Dim query As String = "UPDATE Research_Pub SET 
        year=@year,
        type=@type,
        title=@title,
        authors=@authors,
        scopus_source=@scopus,
        TCI=@tci,
        Volume=@vol,
        Issue=@issue,
        Pages=@pages,
        DOI=@doi,
        KPI=@kpi,
        editDate = GETDATE()
        WHERE no=@no"

        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand(query, con)

                cmd.Parameters.AddWithValue("@no", NO)
                cmd.Parameters.AddWithValue("@year", year)
                cmd.Parameters.AddWithValue("@type", type)
                cmd.Parameters.AddWithValue("@title", txtTitle.Text.Trim())
                cmd.Parameters.AddWithValue("@authors", txtAuthors.Text.Trim())

                cmd.Parameters.AddWithValue("@scopus", If(String.IsNullOrWhiteSpace(txtScopus.Text), DBNull.Value, txtScopus.Text))
                cmd.Parameters.AddWithValue("@tci", If(String.IsNullOrWhiteSpace(txtTCI.Text), DBNull.Value, txtTCI.Text))
                cmd.Parameters.AddWithValue("@vol", If(String.IsNullOrWhiteSpace(txtVolume.Text), DBNull.Value, txtVolume.Text))
                cmd.Parameters.AddWithValue("@issue", If(String.IsNullOrWhiteSpace(txtIssue.Text), DBNull.Value, txtIssue.Text))
                cmd.Parameters.AddWithValue("@pages", If(String.IsNullOrWhiteSpace(txtPages.Text), DBNull.Value, txtPages.Text))
                cmd.Parameters.AddWithValue("@doi", If(String.IsNullOrWhiteSpace(txtDOI.Text), DBNull.Value, txtDOI.Text))
                cmd.Parameters.AddWithValue("@kpi", If(String.IsNullOrEmpty(kpiString), DBNull.Value, kpiString))

                con.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
        UpdatePub_MasterProject(year)
        ' รีเฟรชหน้าหลัก
        panelUpPub.Visible = False
        panelPub.Visible = True
        BindgridPub()

    End Sub
    Private Sub UpdatePub_MasterProject(strategyYear As Integer)

        Dim constr As String = WebConfigurationManager.ConnectionStrings("dbConn_Dashboard").ConnectionString
        constr = Replace(constr, "password", Decrypt2("2fxKF+rsggSR+BM25c3IJLWBqS1Pu4Y5"))

        Dim currentMonth As Integer = DateTime.Now.Month

        Using con As New SqlConnection(constr)
            con.Open()

            Dim dtProject As New DataTable()
            Dim sqlProject As String = "SELECT Project_no FROM MasterProject WHERE Strategy_Year = @year AND Strategy_id = 1"

            Using da As New SqlDataAdapter(sqlProject, con)
                da.SelectCommand.Parameters.AddWithValue("@year", strategyYear)
                da.Fill(dtProject)
            End Using

            For Each row As DataRow In dtProject.Rows
                Dim projectNo As String = row("Project_no").ToString()

                ' นับรายไตรมาส
                Dim sqlCount As String = "
            SELECT 
                SUM(CASE WHEN month IN (10,11,12) THEN 1 ELSE 0 END) AS Q1,
                SUM(CASE WHEN month BETWEEN 1 AND 3 THEN 1 ELSE 0 END) AS Q2,
                SUM(CASE WHEN month BETWEEN 4 AND 6 THEN 1 ELSE 0 END) AS Q3,
                SUM(CASE WHEN month BETWEEN 7 AND 9 THEN 1 ELSE 0 END) AS Q4
            FROM Research_Pub
            WHERE year = @year
            AND ',' + KPI + ',' LIKE '%,' + @projectNo + ',%'"

                Dim q1 As Integer = 0, q2 As Integer = 0, q3 As Integer = 0, q4 As Integer = 0

                Using cmdCount As New SqlCommand(sqlCount, con)
                    cmdCount.Parameters.AddWithValue("@year", strategyYear)
                    cmdCount.Parameters.AddWithValue("@projectNo", projectNo)

                    Using rd = cmdCount.ExecuteReader()
                        If rd.Read() Then
                            q1 = If(IsDBNull(rd("Q1")), 0, Convert.ToInt32(rd("Q1")))
                            q2 = If(IsDBNull(rd("Q2")), 0, Convert.ToInt32(rd("Q2")))
                            q3 = If(IsDBNull(rd("Q3")), 0, Convert.ToInt32(rd("Q3")))
                            q4 = If(IsDBNull(rd("Q4")), 0, Convert.ToInt32(rd("Q4")))
                        End If
                    End Using
                End Using

                ' ทำยอดสะสม
                Dim cQ1 As Integer = q1
                Dim cQ2 As Integer = q1 + q2
                Dim cQ3 As Integer = q1 + q2 + q3
                Dim cQ4 As Integer = q1 + q2 + q3 + q4

                ' แปลงเป็น DBNull ถ้ายังไม่ถึงไตรมาสนั้น
                Dim vQ1 As Object = If(currentMonth >= 10 Or currentMonth <= 12, CType(cQ1, Object), DBNull.Value)
                Dim vQ2 As Object = If(currentMonth >= 1, CType(cQ2, Object), DBNull.Value)
                Dim vQ3 As Object = If(currentMonth >= 4, CType(cQ3, Object), DBNull.Value)
                Dim vQ4 As Object = If(currentMonth >= 7, CType(cQ4, Object), DBNull.Value)

                Dim sqlUpdate As String = "
                UPDATE MasterProject
                SET Quarter1 = @Q1,
                    Quarter2 = @Q2,
                    Quarter3 = @Q3,
                    Quarter4 = @Q4
                WHERE Strategy_Year = @year
                AND Strategy_id = 1
                AND Project_no = @projectNo"

                Using cmdUpdate As New SqlCommand(sqlUpdate, con)
                    cmdUpdate.Parameters.AddWithValue("@Q1", vQ1)
                    cmdUpdate.Parameters.AddWithValue("@Q2", vQ2)
                    cmdUpdate.Parameters.AddWithValue("@Q3", vQ3)
                    cmdUpdate.Parameters.AddWithValue("@Q4", vQ4)
                    cmdUpdate.Parameters.AddWithValue("@year", strategyYear)
                    cmdUpdate.Parameters.AddWithValue("@projectNo", projectNo)
                    cmdUpdate.ExecuteNonQuery()
                End Using

            Next

            con.Close()
        End Using

    End Sub



End Class
