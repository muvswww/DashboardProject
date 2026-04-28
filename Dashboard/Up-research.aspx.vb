Imports System.Data.SqlClient
Imports System.Drawing.Drawing2D
Imports System.Web.Configuration
Imports Dashboard.ConnectDB
Imports Dashboard.Encrypt
Imports Org.BouncyCastle.Bcpg
Public Class Up_research
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Me.IsPostBack Then
            BindgridPub()
            LoadYearPub()
            LoadYearFilterPub()
            LoadCountries()
            LoadCountries2()
            LoadMonth()
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

    Private Sub LoadMonth()
        ddlMonth.Items.Clear()
        ddlMonth.Items.Add(New ListItem("-- เลือกเดือนที่ลงข้อมูล --", ""))

        ddlMonth.Items.Add(New ListItem("มกราคม (January)", "1"))
        ddlMonth.Items.Add(New ListItem("กุมภาพันธ์ (February)", "2"))
        ddlMonth.Items.Add(New ListItem("มีนาคม (March)", "3"))
        ddlMonth.Items.Add(New ListItem("เมษายน (April)", "4"))
        ddlMonth.Items.Add(New ListItem("พฤษภาคม (May)", "5"))
        ddlMonth.Items.Add(New ListItem("มิถุนายน (June)", "6"))
        ddlMonth.Items.Add(New ListItem("กรกฎาคม (July)", "7"))
        ddlMonth.Items.Add(New ListItem("สิงหาคม (August)", "8"))
        ddlMonth.Items.Add(New ListItem("กันยายน (September)", "9"))
        ddlMonth.Items.Add(New ListItem("ตุลาคม (October)", "10"))
        ddlMonth.Items.Add(New ListItem("พฤศจิกายน (November)", "11"))
        ddlMonth.Items.Add(New ListItem("ธันวาคม (December)", "12"))
    End Sub
    Private Sub LoadProjectsPub()
        'If String.IsNullOrEmpty(ddlYearPub2.SelectedValue) Then
        '    cblProject.Items.Clear()
        '    Exit Sub
        'End If

        Dim selectedYear As Integer = Convert.ToInt32(ddlYearPub2.SelectedValue)


        Dim SQLRN As String = "SELECT Strategy_id, Project_no,Project_id, ProjectName " &
                        "FROM MasterProject " &
                        "WHERE Strategy_Year = @year AND Strategy_id = 1"

        Dim parameters As SqlParameter() = {
                    New SqlParameter("@year", selectedYear)
                                                   }
        Dim dt As DataTable = QueryDataTable2(SQLRN, dbConn, "Dashboard", parameters)
        If dt.Rows.Count > 0 Then
            'cblProject.Items.Clear()

            For Each dr As DataRow In dt.Rows
                Dim li As New ListItem()
                li.Text = dr("ProjectName").ToString()
                li.Value = dr("Project_no").ToString()
                'cblProject.Items.Add(li)
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

        ClearCheckBoxList(cblQ)
        ClearCheckBoxList(cblTCI)
        ClearCheckBoxList(cblSDG)


        panelPub.Visible = False
        panelUpPub.Visible = True
        btnsubmitPub.Visible = True
        btnupdatePub.Visible = False
    End Sub
    Private Sub ClearCheckBoxList(cbl As CheckBoxList)
        For Each item As ListItem In cbl.Items
            item.Selected = False
        Next
    End Sub
    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim btnEdit As LinkButton = TryCast(sender, LinkButton)
        Dim rowIndex As Integer = Convert.ToInt32(btnEdit.CommandArgument)
        Dim NO As Integer = Convert.ToInt32(GridViewPub.DataKeys(rowIndex).Value)
        hfPubNo.Value = NO.ToString()
        LoadYearPub()
        'hfNewId.Value = newId.ToString()
        'Labelpub.Text = NO.ToString()

        ClearCheckBoxList(cblQ)
        ClearCheckBoxList(cblTCI)
        ClearCheckBoxList(cblSDG)

        Dim SQLRN As String = "SELECT no, year, month, type, title, authors, scopus_source,
       Volume, Issue, Pages, DOI,
       SDG, Q1, Q2, Q3, Q4, Top1, Top10,
       TCI_G1, TCI_G2, TCI_G3
FROM Research_Pub
WHERE (no = " & NO & ")"
        Dim dt As DataTable = QueryDataTable2(SQLRN, dbConn, "Dashboard", Nothing)

        If dt.Rows.Count > 0 Then
            Dim dr As DataRow = dt.Rows(0)

            ' ===== ปี =====
            If ddlYearPub2.Items.FindByValue(dr("year").ToString()) IsNot Nothing Then
                ddlYearPub2.SelectedValue = dr("year").ToString()
            End If

            ' ===== เดือน =====
            If Not IsDBNull(dr("month")) Then
                ddlMonth.SelectedValue = dr("month").ToString()
            End If

            ' ===== type =====
            ddlType.SelectedValue = dr("type").ToString()

            txtTitle.Text = dr("title").ToString()
            txtAuthors.Text = dr("authors").ToString()
            txtScopus.Text = If(IsDBNull(dr("scopus_source")), "", dr("scopus_source").ToString())
            txtVolume.Text = If(IsDBNull(dr("Volume")), "", dr("Volume").ToString())
            txtIssue.Text = If(IsDBNull(dr("Issue")), "", dr("Issue").ToString())
            txtPages.Text = If(IsDBNull(dr("Pages")), "", dr("Pages").ToString())
            txtDOI.Text = If(IsDBNull(dr("DOI")), "", dr("DOI").ToString())

            ' ===== SDG (1,3,5) =====
            If Not IsDBNull(dr("SDG")) Then
                Dim sdgArr = dr("SDG").ToString().Split(","c)

                For Each item As ListItem In cblSDG.Items
                    item.Selected = sdgArr.Contains(item.Value)
                Next
            End If

            ' ===== Q =====
            SetCheckBox(cblQ, "Q1", dr("Q1"))
            SetCheckBox(cblQ, "Q2", dr("Q2"))
            SetCheckBox(cblQ, "Q3", dr("Q3"))
            SetCheckBox(cblQ, "Q4", dr("Q4"))
            SetCheckBox(cblQ, "Top1", dr("Top1"))
            SetCheckBox(cblQ, "Top10", dr("Top10"))

            ' ===== TCI =====
            SetCheckBox(cblTCI, "TCI_G1", dr("TCI_G1"))
            SetCheckBox(cblTCI, "TCI_G2", dr("TCI_G2"))
            SetCheckBox(cblTCI, "TCI_G3", dr("TCI_G3"))

        End If

        panelPub.Visible = False
        panelUpPub.Visible = True
        btnupdatePub.Visible = True
        btnsubmitPub.Visible = False
    End Sub
    Private Sub SetCheckBox(cbl As CheckBoxList, value As String, dbValue As Object)
        Dim item As ListItem = cbl.Items.FindByValue(value)

        If item IsNot Nothing Then
            If IsDBNull(dbValue) Then
                item.Selected = False
            Else
                item.Selected = Convert.ToBoolean(dbValue)
            End If
        End If
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
        panelPub.Visible = True
        BindgridPub()
    End Sub
    Protected Sub btnsubmitPub_Click(sender As Object, e As EventArgs) Handles btnsubmitPub.Click

        Dim year As Integer = Convert.ToInt32(ddlYearPub2.SelectedValue)
        Dim month As Integer = Convert.ToInt32(ddlMonth.SelectedValue)
        Dim pubType As Integer = Convert.ToInt32(ddlType.SelectedValue)

        Dim title As String = txtTitle.Text.Trim()
        Dim authors As String = txtAuthors.Text.Trim()
        Dim scopus As String = txtScopus.Text.Trim()
        Dim volume As String = txtVolume.Text.Trim()
        Dim issue As String = txtIssue.Text.Trim()
        Dim pages As String = txtPages.Text.Trim()
        Dim doi As String = txtDOI.Text.Trim()

        ' ===== SDG =====
        Dim sdgString As String = GetSelectedValues(cblSDG)

        ' ===== Q =====
        Dim Q1 As Integer = If(cblQ.Items.FindByValue("Q1").Selected, 1, 0)
        Dim Q2 As Integer = If(cblQ.Items.FindByValue("Q2").Selected, 1, 0)
        Dim Q3 As Integer = If(cblQ.Items.FindByValue("Q3").Selected, 1, 0)
        Dim Q4 As Integer = If(cblQ.Items.FindByValue("Q4").Selected, 1, 0)
        Dim Top1 As Integer = If(cblQ.Items.FindByValue("Top1").Selected, 1, 0)
        Dim Top10 As Integer = If(cblQ.Items.FindByValue("Top10").Selected, 1, 0)

        ' ===== TCI =====
        Dim TCI_G1 As Integer = If(cblTCI.Items.FindByValue("TCI_G1").Selected, 1, 0)
        Dim TCI_G2 As Integer = If(cblTCI.Items.FindByValue("TCI_G2").Selected, 1, 0)
        Dim TCI_G3 As Integer = If(cblTCI.Items.FindByValue("TCI_G3").Selected, 1, 0)



        Dim constr As String = WebConfigurationManager.ConnectionStrings("dbConn_Dashboard").ConnectionString
        constr = Replace(constr, "password", Decrypt2("2fxKF+rsggR/rV4zqRqEFgaWM7ITZryKK1haDXSOUV4="))

        Dim SQLRN As String = "SELECT  ISNULL(MAX(no), 0) AS newNo FROM Research_Pub"
        Dim dt As DataTable = QueryDataTable2(SQLRN, dbConn, "Dashboard", Nothing)
        If dt.Rows.Count > 0 Then
            Dim newNo As Integer = Convert.ToInt32(dt.Rows(0)("newNo")) + 1
            Dim insertSql As String = "
    INSERT INTO Research_Pub
    (year, month, type, title, authors, scopus_source,
     Volume, Issue, Pages, DOI,
     SDG, Q1, Q2, Q3, Q4, Top1, Top10,
     TCI_G1, TCI_G2, TCI_G3, inputDate)
    VALUES
    (@year, @month, @type, @title, @authors, @scopus,
     @volume, @issue, @pages, @doi,
     @SDG, @Q1, @Q2, @Q3, @Q4, @Top1, @Top10,
     @TCI_G1, @TCI_G2, @TCI_G3, GETDATE());"

            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand(insertSql, con)

                    cmd.Parameters.AddWithValue("@year", year)
                    cmd.Parameters.AddWithValue("@month", month)
                    cmd.Parameters.AddWithValue("@type", pubType)
                    cmd.Parameters.AddWithValue("@title", title)
                    cmd.Parameters.AddWithValue("@authors", authors)

                    cmd.Parameters.AddWithValue("@scopus", If(scopus = "", DBNull.Value, scopus))
                    cmd.Parameters.AddWithValue("@volume", If(volume = "", DBNull.Value, volume))
                    cmd.Parameters.AddWithValue("@issue", If(issue = "", DBNull.Value, issue))
                    cmd.Parameters.AddWithValue("@pages", If(pages = "", DBNull.Value, pages))
                    cmd.Parameters.AddWithValue("@doi", If(doi = "", DBNull.Value, doi))

                    cmd.Parameters.AddWithValue("@SDG", If(sdgString = "", DBNull.Value, sdgString))

                    cmd.Parameters.AddWithValue("@Q1", Q1)
                    cmd.Parameters.AddWithValue("@Q2", Q2)
                    cmd.Parameters.AddWithValue("@Q3", Q3)
                    cmd.Parameters.AddWithValue("@Q4", Q4)
                    cmd.Parameters.AddWithValue("@Top1", Top1)
                    cmd.Parameters.AddWithValue("@Top10", Top10)

                    cmd.Parameters.AddWithValue("@TCI_G1", TCI_G1)
                    cmd.Parameters.AddWithValue("@TCI_G2", TCI_G2)
                    cmd.Parameters.AddWithValue("@TCI_G3", TCI_G3)

                    con.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End If



        panelPub.Visible = True
        BindgridPub()
    End Sub

    Protected Sub BtnupdatePub_Click(sender As Object, e As EventArgs) Handles btnupdatePub.Click


        If String.IsNullOrEmpty(hfPubNo.Value) Then Exit Sub

        Dim NO As Integer = Convert.ToInt32(hfPubNo.Value)

        Dim year As Integer = Convert.ToInt32(ddlYearPub2.SelectedValue)
        Dim month As Integer = Convert.ToInt32(ddlMonth.SelectedValue)
        Dim type As Integer = Convert.ToInt32(ddlType.SelectedValue)

        Dim title As String = txtTitle.Text.Trim()
        Dim authors As String = txtAuthors.Text.Trim()
        Dim scopus As String = txtScopus.Text.Trim()
        Dim volume As String = txtVolume.Text.Trim()
        Dim issue As String = txtIssue.Text.Trim()
        Dim pages As String = txtPages.Text.Trim()
        Dim doi As String = txtDOI.Text.Trim()

        Dim sdgString As String = GetSelectedValues(cblSDG)

        ' ===== Q =====
        Dim Q1 As Integer = If(cblQ.Items.FindByValue("Q1").Selected, 1, 0)
        Dim Q2 As Integer = If(cblQ.Items.FindByValue("Q2").Selected, 1, 0)
        Dim Q3 As Integer = If(cblQ.Items.FindByValue("Q3").Selected, 1, 0)
        Dim Q4 As Integer = If(cblQ.Items.FindByValue("Q4").Selected, 1, 0)
        Dim Top1 As Integer = If(cblQ.Items.FindByValue("Top1").Selected, 1, 0)
        Dim Top10 As Integer = If(cblQ.Items.FindByValue("Top10").Selected, 1, 0)

        ' ===== TCI =====
        Dim TCI_G1 As Integer = If(cblTCI.Items.FindByValue("TCI_G1").Selected, 1, 0)
        Dim TCI_G2 As Integer = If(cblTCI.Items.FindByValue("TCI_G2").Selected, 1, 0)
        Dim TCI_G3 As Integer = If(cblTCI.Items.FindByValue("TCI_G3").Selected, 1, 0)

        Dim constr As String = WebConfigurationManager.ConnectionStrings("dbConn_Dashboard").ConnectionString
        constr = Replace(constr, "password", Decrypt2("2fxKF+rsggR/rV4zqRqEFgaWM7ITZryKK1haDXSOUV4="))

        Dim query As String = "
        UPDATE Research_Pub SET
        year =@year,
        month =@month,
        type =@type,
        title =@title,
        authors =@authors,
        scopus_source =@scopus,
        volume =@volume,
        issue =@issue,
        pages =@pages,
        doi =@doi,
        SDG =@SDG,
        Q1 =@Q1,
        Q2 =@Q2,
        Q3 =@Q3,
        Q4 =@Q4,
        Top1 =@Top1,
        Top10 =@Top10,
        TCI_G1 =@TCI_G1,
        TCI_G2 =@TCI_G2,
        TCI_G3 =@TCI_G3,
        editDate = GETDATE()
        WHERE NO =@NO"


        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand(query, con)
                cmd.Parameters.AddWithValue("@NO", NO)
                cmd.Parameters.AddWithValue("@year", year)
                cmd.Parameters.AddWithValue("@month", month)
                cmd.Parameters.AddWithValue("@type", type)
                cmd.Parameters.AddWithValue("@title", title)
                cmd.Parameters.AddWithValue("@authors", authors)

                cmd.Parameters.AddWithValue("@scopus", If(scopus = "", DBNull.Value, scopus))
                cmd.Parameters.AddWithValue("@volume", If(volume = "", DBNull.Value, volume))
                cmd.Parameters.AddWithValue("@issue", If(issue = "", DBNull.Value, issue))
                cmd.Parameters.AddWithValue("@pages", If(pages = "", DBNull.Value, pages))
                cmd.Parameters.AddWithValue("@doi", If(doi = "", DBNull.Value, doi))

                cmd.Parameters.AddWithValue("@SDG", If(sdgString = "", DBNull.Value, sdgString))

                cmd.Parameters.AddWithValue("@Q1", Q1)
                cmd.Parameters.AddWithValue("@Q2", Q2)
                cmd.Parameters.AddWithValue("@Q3", Q3)
                cmd.Parameters.AddWithValue("@Q4", Q4)
                cmd.Parameters.AddWithValue("@Top1", Top1)
                cmd.Parameters.AddWithValue("@Top10", Top10)

                cmd.Parameters.AddWithValue("@TCI_G1", TCI_G1)
                cmd.Parameters.AddWithValue("@TCI_G2", TCI_G2)
                cmd.Parameters.AddWithValue("@TCI_G3", TCI_G3)

                con.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
        ' รีเฟรชหน้าหลัก
        panelUpPub.Visible = False
        panelPub.Visible = True
        BindgridPub()

    End Sub
    'Private Sub UpdatePub_MasterProject(strategyYear As Integer)

    '    Dim constr As String = WebConfigurationManager.ConnectionStrings("dbConn_Dashboard").ConnectionString
    '    constr = Replace(constr, "password", Decrypt2("2fxKF+rsggR/rV4zqRqEFgaWM7ITZryKK1haDXSOUV4="))

    '    Dim currentMonth As Integer = DateTime.Now.Month

    '    Using con As New SqlConnection(constr)
    '        con.Open()

    '        Dim dtProject As New DataTable()
    '        Dim sqlProject As String = "SELECT Project_no FROM MasterProject WHERE Strategy_Year = @year And Strategy_id = 1"

    '        Using da As New SqlDataAdapter(sqlProject, con)
    '            da.SelectCommand.Parameters.AddWithValue("@year", strategyYear)
    '            da.Fill(dtProject)
    '        End Using

    '        For Each row As DataRow In dtProject.Rows
    '            Dim projectNo As String = row("Project_no").ToString()

    '            ' นับรายไตรมาส
    '            Dim sqlCount As String = "
    '        SELECT 
    '            SUM(CASE WHEN month IN (10,11,12) THEN 1 ELSE 0 END) AS Q1,
    '            SUM(CASE WHEN month BETWEEN 1 AND 3 THEN 1 ELSE 0 END) AS Q2,
    '            SUM(CASE WHEN month BETWEEN 4 AND 6 THEN 1 ELSE 0 END) AS Q3,
    '            SUM(CASE WHEN month BETWEEN 7 AND 9 THEN 1 ELSE 0 END) AS Q4
    '        FROM Research_Pub
    '        WHERE year = @year
    '        AND ',' + KPI + ',' LIKE '%,' + @projectNo + ',%'"

    '            Dim q1 As Integer = 0, q2 As Integer = 0, q3 As Integer = 0, q4 As Integer = 0

    '            Using cmdCount As New SqlCommand(sqlCount, con)
    '                cmdCount.Parameters.AddWithValue("@year", strategyYear)
    '                cmdCount.Parameters.AddWithValue("@projectNo", projectNo)

    '                Using rd = cmdCount.ExecuteReader()
    '                    If rd.Read() Then
    '                        q1 = If(IsDBNull(rd("Q1")), 0, Convert.ToInt32(rd("Q1")))
    '                        q2 = If(IsDBNull(rd("Q2")), 0, Convert.ToInt32(rd("Q2")))
    '                        q3 = If(IsDBNull(rd("Q3")), 0, Convert.ToInt32(rd("Q3")))
    '                        q4 = If(IsDBNull(rd("Q4")), 0, Convert.ToInt32(rd("Q4")))
    '                    End If
    '                End Using
    '            End Using

    '            ' ทำยอดสะสม
    '            Dim cQ1 As Integer = q1
    '            Dim cQ2 As Integer = q1 + q2
    '            Dim cQ3 As Integer = q1 + q2 + q3
    '            Dim cQ4 As Integer = q1 + q2 + q3 + q4

    '            ' แปลงเป็น DBNull ถ้ายังไม่ถึงไตรมาสนั้น
    '            Dim vQ1 As Object = If(currentMonth >= 10 Or currentMonth <= 12, CType(cQ1, Object), DBNull.Value)
    '            Dim vQ2 As Object = If(currentMonth >= 1, CType(cQ2, Object), DBNull.Value)
    '            Dim vQ3 As Object = If(currentMonth >= 4, CType(cQ3, Object), DBNull.Value)
    '            Dim vQ4 As Object = If(currentMonth >= 7, CType(cQ4, Object), DBNull.Value)

    '            Dim sqlUpdate As String = "
    '            UPDATE MasterProject
    '            SET Quarter1 = @Q1,
    '                Quarter2 = @Q2,
    '                Quarter3 = @Q3,
    '                Quarter4 = @Q4
    '            WHERE Strategy_Year = @year
    '            AND Strategy_id = 1
    '            AND Project_no = @projectNo"

    '            Using cmdUpdate As New SqlCommand(sqlUpdate, con)
    '                cmdUpdate.Parameters.AddWithValue("@Q1", vQ1)
    '                cmdUpdate.Parameters.AddWithValue("@Q2", vQ2)
    '                cmdUpdate.Parameters.AddWithValue("@Q3", vQ3)
    '                cmdUpdate.Parameters.AddWithValue("@Q4", vQ4)
    '                cmdUpdate.Parameters.AddWithValue("@year", strategyYear)
    '                cmdUpdate.Parameters.AddWithValue("@projectNo", projectNo)
    '                cmdUpdate.ExecuteNonQuery()
    '            End Using

    '        Next

    '        con.Close()
    '    End Using

    'End Sub
    Private Property InterList As List(Of String)
        Get
            If ViewState("InterList") Is Nothing Then
                ViewState("InterList") = New List(Of String)
            End If
            Return CType(ViewState("InterList"), List(Of String))
        End Get
        Set(value As List(Of String))
            ViewState("InterList") = value
        End Set
    End Property

    Private Property AcademicList As List(Of String)
        Get
            If ViewState("AcademicList") Is Nothing Then
                ViewState("AcademicList") = New List(Of String)
            End If
            Return CType(ViewState("AcademicList"), List(Of String))
        End Get
        Set(value As List(Of String))
            ViewState("AcademicList") = value
        End Set
    End Property

    Private Sub LoadCountries()

        ddlCountryInter.Items.Clear()
        ddlCountryInter.Items.Add(New ListItem("-- เลือกประเทศ --", ""))

        Dim countries = System.Globalization.CultureInfo.GetCultures(Globalization.CultureTypes.SpecificCultures) _
    .Select(Function(c) New Globalization.RegionInfo(c.LCID).EnglishName) _
    .Distinct() _
    .OrderBy(Function(c) c)

        For Each c In countries
            ddlCountryInter.Items.Add(New ListItem(c, c))
        Next
    End Sub
    Private Sub LoadCountries2()

        ddlCountryAcademic.Items.Clear()
        ddlCountryAcademic.Items.Add(New ListItem("-- เลือกประเทศ --", ""))

        Dim countries = System.Globalization.CultureInfo.GetCultures(Globalization.CultureTypes.SpecificCultures) _
    .Select(Function(c) New Globalization.RegionInfo(c.LCID).EnglishName) _
    .Distinct() _
    .OrderBy(Function(c) c)

        For Each c In countries
            ddlCountryAcademic.Items.Add(New ListItem(c, c))
        Next
    End Sub
    Protected Sub chkInter_CheckedChanged(sender As Object, e As EventArgs)
        pnlInter.Visible = chkInter.Checked
    End Sub

    Protected Sub chkAcademic_CheckedChanged(sender As Object, e As EventArgs)
        pnlAcademic.Visible = chkAcademic.Checked
    End Sub
    Protected Sub btnAddInter_Click(sender As Object, e As EventArgs)

        Dim uni = txtInterCollab.Text.Trim()

        If uni = "" Then Exit Sub

        Dim selectedCountries = ddlCountryInter.Items.Cast(Of ListItem)().
            Where(Function(i) i.Selected).
            Select(Function(i) i.Value)

        Dim countryStr = String.Join(", ", selectedCountries)

        Dim item = uni & " (" & countryStr & ")"

        If Not InterList.Contains(item) Then
            InterList.Add(item)
        End If

        lblInter.Text = String.Join(", ", InterList)

        txtInterCollab.Text = ""
    End Sub
    Protected Sub btnAddAcademic_Click(sender As Object, e As EventArgs)

        Dim uni = txtacademicCollab.Text.Trim()

        If uni = "" Then Exit Sub

        Dim selectedCountries = ddlCountryAcademic.Items.Cast(Of ListItem)().
            Where(Function(i) i.Selected).
            Select(Function(i) i.Value)

        Dim countryStr = String.Join(", ", selectedCountries)

        Dim item = uni & " (" & countryStr & ")"

        If Not AcademicList.Contains(item) Then
            AcademicList.Add(item)
        End If

        lblAcademic.Text = String.Join(", ", AcademicList)

        txtacademicCollab.Text = ""
    End Sub
    Private Function GetSelectedValues(cbl As CheckBoxList) As String
        Dim selected As New List(Of String)

        For Each item As ListItem In cbl.Items
            If item.Selected Then
                selected.Add(item.Value)
            End If
        Next

        Return String.Join(",", selected)
    End Function
End Class
