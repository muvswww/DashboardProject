Imports System.Web.Configuration
Imports Dashboard.ConnectDB
Imports Dashboard.Encrypt
Imports System.Data.SqlClient

Public Class Up_researchInnov
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Me.IsPostBack Then
            BindInnov()
        End If

    End Sub
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)

        GridViewInnov.PageIndex = e.NewPageIndex
        BindInnov()
    End Sub

    Private Sub BindInnov()

        Dim SQLRN As String = "SELECT        TOP (200) dbo.Research_Innovation.no, dbo.Research_Innovation.title, dbo.Research_Innovation.user_id, dbo.Research_Innovation.type, dbo.Research_Innovation.request_date, dbo.Research_Innovation.request_number, 
                         dbo.Research_Innovation.inputDate, dbo.Research_Innovation.editDate, CASE WHEN itjobs.dbo.title_technical.title_technicalName IS NULL THEN itjobs.dbo.title.title_name + itjobs.dbo.[user].fname + SPACE(2) 
                         + itjobs.dbo.[user].lname ELSE itjobs.dbo.title_technical.title_technicalName + itjobs.dbo.[user].fname + SPACE(2) + itjobs.dbo.[user].lname END AS fullname, 
                         CASE WHEN Research_Innovation.type = 1 THEN 'สิทธิบัตร' WHEN Research_Innovation.type = 2 THEN 'อนุสิทธิบัตร' WHEN Research_Innovation.type = 3 THEN 'ลิขสิทธิ์' WHEN Research_Innovation.type = 4 THEN 'ขอสิทธิบัตร' WHEN
                          Research_Innovation.type = 5 THEN 'ขออนุสิทธิบัตร' ELSE '-' END AS type_name
FROM            dbo.Research_Innovation INNER JOIN
                         itjobs.dbo.[user] ON dbo.Research_Innovation.user_id = itjobs.dbo.[user].user_id LEFT OUTER JOIN
                         itjobs.dbo.title ON itjobs.dbo.[user].title_id = itjobs.dbo.title.title_id LEFT OUTER JOIN
                         itjobs.dbo.title_technical ON itjobs.dbo.[user].title_technicalID = itjobs.dbo.title_technical.title_technicalID
ORDER BY dbo.Research_Innovation.type"

        Dim dt As DataTable = QueryDataTable2(SQLRN, dbConn, "Dashboard", Nothing)
        If dt.Rows.Count > 0 Then

            GridViewInnov.DataSource = dt
            GridViewInnov.DataBind()
        End If
    End Sub
    Private Sub LoadUserDropdown(ddl As DropDownList, Optional selectedValue As String = "0")

        Dim SQLRN As String = "SELECT dbo.[user].user_id,
        CASE WHEN dbo.title_technical.title_technicalName IS NULL 
        THEN dbo.title.title_name + dbo.[user].fname + SPACE(2) + dbo.[user].lname
        ELSE dbo.title_technical.title_technicalName + dbo.[user].fname + SPACE(2) + dbo.[user].lname 
        END AS fullname
        FROM dbo.[user]
        INNER JOIN dbo.title ON dbo.[user].title_id = dbo.title.title_id
        LEFT JOIN dbo.title_technical ON dbo.[user].title_technicalID = dbo.title_technical.title_technicalID
        WHERE dbo.[user].isActive = 1
        ORDER BY dbo.[user].fname"

        Dim dt As DataTable = QueryDataTable2(SQLRN, dbConn, "itjobs", Nothing)

        ddl.DataSource = dt
        ddl.DataTextField = "fullname"
        ddl.DataValueField = "user_id"
        ddl.DataBind()

        ddl.Items.Insert(0, New ListItem("-- เลือกชื่อเจ้าของผลงาน --", "0"))

        If selectedValue <> "0" Then
            ddl.SelectedValue = selectedValue
        End If

    End Sub
    Protected Sub GridViewInnov_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridViewInnov.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then

            ' EDIT MODE
            If (e.Row.RowState And DataControlRowState.Edit) > 0 Then
                Dim ddlEditUser As DropDownList = CType(e.Row.FindControl("ddlEditUser"), DropDownList)
                Dim userId As String = DataBinder.Eval(e.Row.DataItem, "user_id").ToString()
                LoadUserDropdown(ddlEditUser, userId)
            End If

        End If

        ' FOOTER
        If e.Row.RowType = DataControlRowType.Footer Then
            Dim ddlNewUser As DropDownList = CType(e.Row.FindControl("ddlNewUser"), DropDownList)
            LoadUserDropdown(ddlNewUser)
        End If
    End Sub

    Protected Sub GridViewInnov_RowCommand(sender As Object, e As GridViewCommandEventArgs)

        If e.CommandName = "AddNew" Then

            Dim footer As GridViewRow = GridViewInnov.FooterRow
            Dim ddlType As DropDownList = CType(footer.FindControl("ddlNewType"), DropDownList)
            Dim ddlUser As DropDownList = CType(footer.FindControl("ddlNewUser"), DropDownList)
            Dim txtTitle As TextBox = CType(footer.FindControl("txttitle"), TextBox)
            Dim txtNumber As TextBox = CType(footer.FindControl("txtNumber"), TextBox)
            Dim txtDate As TextBox = CType(footer.FindControl("txtNewDate"), TextBox)
            Dim reqDate As Object = DBNull.Value
            If Not String.IsNullOrWhiteSpace(txtDate.Text) Then
                reqDate = DateTime.Parse(txtDate.Text)
            End If
            Dim constr As String = WebConfigurationManager.ConnectionStrings("dbConn_Dashboard").ConnectionString
            constr = Replace(constr, "password", Decrypt2("2fxKF+rsggSR+BM25c3IJLWBqS1Pu4Y5"))

            Dim SQLRN As String = "SELECT  ISNULL(MAX(no), 0) AS newNo FROM Research_Innovation"
            Dim dt As DataTable = QueryDataTable2(SQLRN, dbConn, "Dashboard", Nothing)

            Dim newNo As Integer = Convert.ToInt32(dt.Rows(0)("newNo")) + 1
                Using conn As New SqlConnection(constr)
                    Using cmd As New SqlCommand("INSERT INTO Research_Innovation (no,title,user_id,type,request_date,request_number,inputDate)
                                         VALUES (@no,@title,@user,@type,@date,@number,GETDATE())", conn)

                        cmd.Parameters.AddWithValue("@no", newNo)
                        cmd.Parameters.AddWithValue("@title", txtTitle.Text.Trim())
                        cmd.Parameters.AddWithValue("@user", If(ddlUser.SelectedValue = "0", DBNull.Value, ddlUser.SelectedValue))
                        cmd.Parameters.AddWithValue("@type", ddlType.SelectedValue)
                        cmd.Parameters.AddWithValue("@number", txtNumber.Text.Trim())
                        cmd.Parameters.AddWithValue("@date", reqDate)

                        conn.Open()
                        cmd.ExecuteNonQuery()
                    End Using
                End Using
                BindInnov()

        End If

    End Sub

    Protected Sub GridViewInnov_RowEditing(sender As Object, e As GridViewEditEventArgs)
        GridViewInnov.EditIndex = e.NewEditIndex
        BindInnov()
    End Sub
    Protected Sub GridViewInnov_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs)
        GridViewInnov.EditIndex = -1
        BindInnov()
    End Sub
    Protected Sub GridViewInnov_RowUpdating(sender As Object, e As GridViewUpdateEventArgs)

        Dim no As Integer = Convert.ToInt32(GridViewInnov.DataKeys(e.RowIndex).Value)
        Dim row As GridViewRow = GridViewInnov.Rows(e.RowIndex)

        Dim ddlType As DropDownList = CType(row.FindControl("ddlEditType"), DropDownList)
        Dim ddlUser As DropDownList = CType(row.FindControl("ddlEditUser"), DropDownList)
        Dim txtTitle As TextBox = CType(row.FindControl("txttitle"), TextBox)
        Dim txtNumber As TextBox = CType(row.FindControl("txtNumber"), TextBox)
        Dim txtDate As TextBox = CType(row.FindControl("txtEditDate"), TextBox)
        Dim reqDate As Object = DBNull.Value
        If Not String.IsNullOrWhiteSpace(txtDate.Text) Then
            reqDate = DateTime.Parse(txtDate.Text)
        End If
        Dim constr As String = WebConfigurationManager.ConnectionStrings("dbConn_Dashboard").ConnectionString
        constr = Replace(constr, "password", Decrypt2("2fxKF+rsggSR+BM25c3IJLWBqS1Pu4Y5"))

        Using conn As New SqlConnection(constr)
            Using cmd As New SqlCommand("UPDATE Research_Innovation
                                     SET title=@title,user_id=@user,type=@type,request_date=@date,request_number=@number,editDate=GETDATE()
                                     WHERE no=@no", conn)

                cmd.Parameters.AddWithValue("@title", txtTitle.Text.Trim())
                cmd.Parameters.AddWithValue("@user", If(ddlUser.SelectedValue = "0", DBNull.Value, ddlUser.SelectedValue))
                cmd.Parameters.AddWithValue("@type", ddlType.SelectedValue)
                cmd.Parameters.AddWithValue("@number", txtNumber.Text.Trim())
                cmd.Parameters.AddWithValue("@date", reqDate)
                cmd.Parameters.AddWithValue("@no", no)

                conn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using

        GridViewInnov.EditIndex = -1
        BindInnov()

    End Sub


    Protected Sub btnCancelsource_Click(sender As Object, e As EventArgs) Handles btnCancelsource.Click
        Response.Redirect("portal.aspx", False)
    End Sub
End Class
