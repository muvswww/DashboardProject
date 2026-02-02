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
            BindgridPub()

        End If
        LoadYear()
    End Sub
    Private Sub LoadYear()
        Dim SQLRN As String = "SELECT DISTINCT Strategy_Year FROM MasterProject ORDER BY Strategy_Year DESC"
        Dim dt As DataTable = QueryDataTable2(SQLRN, dbConn, "Dashboard", Nothing)

        ddlYear.Items.Clear()
        ddlYear.Items.Add(New ListItem("-- เลือกปี --", ""))

        For Each dr As DataRow In dt.Rows
            ddlYear.Items.Add(New ListItem(dr("Strategy_Year").ToString(), dr("Strategy_Year").ToString()))
        Next
    End Sub
    Protected Sub ddlYear_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlYear.SelectedIndexChanged
    LoadProjects()
End Sub


    Private Sub LoadProjects()
        If ddlYear.SelectedValue = "" Then
            cblProject.Items.Clear()
            Exit Sub
        End If
        Dim selectedYear As Integer = Convert.ToInt32(ddlYear.SelectedValue)


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

    Private Sub BindgridPub()
        Dim SQLRN As String = "SELECT        no, year, CASE WHEN type = 1 THEN 'ผลงานวิจัยระดับชาติ' WHEN type = 2 THEN 'ผลงานวิจัยระดับนานาชาติ' END AS type, title
FROM            Research_Pub
ORDER BY no DESC"
        Dim dt As DataTable
        dt = QueryDataTable2(SQLRN, dbConn, "Dashboard", Nothing)
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

        txtDOI.Text = ""
        txtDOI.Attributes("placeholder") = "ระบุ DOI"


        panelPub.Visible = False
        panelUpPub.Visible = True
    End Sub
    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim btnEdit As LinkButton = TryCast(sender, LinkButton)
        Dim rowIndex As Integer = Convert.ToInt32(btnEdit.CommandArgument)
        Dim NO As Integer = Convert.ToInt32(GridViewPub.DataKeys(rowIndex).Value)
        LoadYear()
        'hfNewId.Value = newId.ToString()
        'Labelpub.Text = NO.ToString()

        Dim SQLRN As String = "SELECT       no, year, month, type, title, authors, scopus_source, TCI, Volume, Issue, Pages, DOI, KPI, inputDate
FROM            Research_Pub
WHERE (no = " & NO & ")"
        Dim dt As DataTable = QueryDataTable2(SQLRN, dbConn, "Dashboard", Nothing)

        If dt.Rows.Count > 0 Then
            Dim selectedYear As String = dt.Rows(0)("year").ToString()
            If ddlYear.Items.FindByValue(selectedYear) IsNot Nothing Then
                ddlYear.SelectedValue = selectedYear
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
            LoadProjects()

            SetCheckedKPI(dt.Rows(0)("KPI").ToString())
        End If

        panelPub.Visible = False
        panelUpPub.Visible = True
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
    'Protected Sub Btnsubmit_Click(sender As Object, e As EventArgs) Handles btnsubmitPub.Click
    '    'Dim year As String = txtyear.Text
    '    'Dim Sec As String = txtSec.Text
    '    'Dim SubSec As String = txtSub.Text
    '    'Dim Link As String = txtLink.Text
    '    'Dim selectedType As Integer = Convert.ToInt32(ddlYear.SelectedValue)

    '    Dim constr As String = WebConfigurationManager.ConnectionStrings("dbConn_VSAPP").ConnectionString
    '    constr = Replace(constr, "password", Decrypt2("2fxKF+rsggSR+BM25c3IJLWBqS1Pu4Y5"))
    '    Dim SQLRN As String = "SELECT TOP (1) ID FROM OIT ORDER BY ID DESC"
    '    Dim dt As DataTable = QueryDataTable2(SQLRN, dbConn, "VSAPP", Nothing)
    '    If dt.Rows.Count > 0 Then
    '        Dim maxId As Integer = Convert.ToInt32(dt.Rows(0)("ID")) + 1
    '        Dim query As String = "INSERT INTO OIT (year, title, section, SubSec, linkData, ID ) VALUES (@year, @title, @section, @SubSec, @linkData, @ID)"
    '        Using con As New SqlConnection(constr)
    '            Using cmd As New SqlCommand(query, con)
    '                cmd.Parameters.AddWithValue("@ID", maxId)
    '                'cmd.Parameters.AddWithValue("@year", year)
    '                cmd.Parameters.AddWithValue("@title", selectedType)
    '                cmd.Parameters.AddWithValue("@section", Sec)
    '                cmd.Parameters.AddWithValue("@linkData", Link)
    '                If String.IsNullOrEmpty(SubSec) Then
    '                    cmd.Parameters.AddWithValue("@SubSec", DBNull.Value)
    '                Else
    '                    cmd.Parameters.AddWithValue("@SubSec", SubSec)
    '                End If

    '                con.Open()
    '                cmd.ExecuteNonQuery()

    '            End Using
    '        End Using
    '    End If
    '    panelPub.Visible = True
    '    BindgridPub()
    'End Sub
    'Protected Sub Btnupdate_Click(sender As Object, e As EventArgs) Handles btnupdatePub.Click
    '    'Dim year As String = txtyear.Text
    '    Dim Sec As String = txtSec.Text
    '    Dim SubSec As String = txtSub.Text
    '    Dim Link As String = txtLink.Text
    '    Dim selectedType As Integer = Convert.ToInt32(ddlYear.SelectedValue)
    '    Dim ID As String = Labelpub.Text
    '    Dim constr As String = WebConfigurationManager.ConnectionStrings("dbConn_VSAPP").ConnectionString
    '    constr = Replace(constr, "password", Decrypt2("2fxKF+rsggSR+BM25c3IJLWBqS1Pu4Y5"))
    '    Dim query As String = "UPDATE OIT SET year=@year, title=@title, section=@section, SubSec=@SubSec, linkData=@linkData WHERE ID=@ID"
    '    Using con As New SqlConnection(constr)
    '        Using cmd As New SqlCommand(query, con)
    '            cmd.Parameters.AddWithValue("@ID", ID)
    '            'cmd.Parameters.AddWithValue("@year", year)
    '            cmd.Parameters.AddWithValue("@title", selectedType)
    '            cmd.Parameters.AddWithValue("@section", Sec)
    '            cmd.Parameters.AddWithValue("@linkData", Link)
    '            If String.IsNullOrEmpty(SubSec) Then
    '                cmd.Parameters.AddWithValue("@SubSec", DBNull.Value)
    '            Else
    '                cmd.Parameters.AddWithValue("@SubSec", SubSec)
    '            End If

    '            con.Open()
    '            cmd.ExecuteNonQuery()

    '        End Using
    '    End Using
    '    panelPub.Visible = True
    '    BindgridPub()



    'End Sub
End Class
