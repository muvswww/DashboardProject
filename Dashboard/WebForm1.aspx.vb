Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Web.Configuration
Imports Dashboard.ConnectDB
Imports Dashboard.Encrypt
Imports iTextSharp.text.pdf

Public Class WebForm1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            Bindgrid()
        End If
    End Sub

    Private Sub Bindgrid()
        Dim SQLRN As String = "SELECT        year, title, section, SubSec, linkData, ID
FROM            OIT
ORDER BY year DESC, title"
        Dim dt As DataTable
        dt = QueryDataTable2(SQLRN, dbConn, "VSAPP", Nothing)
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
        txtyear.Text = ""
        txtyear.Attributes("placeholder") = "ระบุปี พ.ศ."

        txtSec.Text = ""
        txtSec.Attributes("placeholder") = "ระบุชื่อของข้อมูล"

        txtSub.Text = ""
        txtSub.Attributes("placeholder") = "ระบุชื่อย่อยของข้อมูล"

        txtLink.Text = ""
        txtLink.Attributes("placeholder") = "ระบุLinkของข้อมูล"


        panelPub.Visible = False
        panelUpPub.Visible = True
    End Sub
    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim btnEdit As LinkButton = TryCast(sender, LinkButton)
        Dim rowIndex As Integer = Convert.ToInt32(btnEdit.CommandArgument)
        Dim newId As Integer = Convert.ToInt32(GridViewPub.DataKeys(rowIndex).Value)
        'hfNewId.Value = newId.ToString()
        Labelpub.Text = newId.ToString()

        Dim SQLRN As String = "SELECT       year, title, section, SubSec, linkData, ID
FROM            OIT 
WHERE (ID = " & newId & ")"
        Dim dt As DataTable = QueryDataTable2(SQLRN, dbConn, "VSAPP", Nothing)

        If dt.Rows.Count > 0 Then
            txtyear.Text = dt.Rows(0)("year")
            Dim selectedValue As String = dt.Rows(0)("title").ToString()
            ddlTitle.SelectedValue = selectedValue
            txtSec.Text = dt.Rows(0)("section")
            If dt.Rows(0)("SubSec") Is DBNull.Value Then
                txtSub.Text = "" ' กำหนดให้เป็นข้อความว่าง
            Else
                txtSub.Text = dt.Rows(0)("SubSec")
            End If
            txtLink.Text = dt.Rows(0)("linkData")

        End If

        panelPub.Visible = False
        panelUpPub.Visible = True
    End Sub

    Protected Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        panelPub.Visible = True
        Bindgrid()
    End Sub
    Protected Sub Btnsubmit_Click(sender As Object, e As EventArgs) Handles btnsubmitPub.Click
        Dim year As String = txtyear.Text
        Dim Sec As String = txtSec.Text
        Dim SubSec As String = txtSub.Text
        Dim Link As String = txtLink.Text
        Dim selectedType As Integer = Convert.ToInt32(ddlTitle.SelectedValue)

        Dim constr As String = WebConfigurationManager.ConnectionStrings("dbConn_VSAPP").ConnectionString
        constr = Replace(constr, "password", Decrypt2("2fxKF+rsggSR+BM25c3IJLWBqS1Pu4Y5"))
        Dim SQLRN As String = "SELECT TOP (1) ID FROM OIT ORDER BY ID DESC"
        Dim dt As DataTable = QueryDataTable2(SQLRN, dbConn, "VSAPP", Nothing)
        If dt.Rows.Count > 0 Then
            Dim maxId As Integer = Convert.ToInt32(dt.Rows(0)("ID")) + 1
            Dim query As String = "INSERT INTO OIT (year, title, section, SubSec, linkData, ID ) VALUES (@year, @title, @section, @SubSec, @linkData, @ID)"
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand(query, con)
                    cmd.Parameters.AddWithValue("@ID", maxId)
                    cmd.Parameters.AddWithValue("@year", year)
                    cmd.Parameters.AddWithValue("@title", selectedType)
                    cmd.Parameters.AddWithValue("@section", Sec)
                    cmd.Parameters.AddWithValue("@linkData", Link)
                    If String.IsNullOrEmpty(SubSec) Then
                        cmd.Parameters.AddWithValue("@SubSec", DBNull.Value)
                    Else
                        cmd.Parameters.AddWithValue("@SubSec", SubSec)
                    End If

                    con.Open()
                    cmd.ExecuteNonQuery()

                End Using
            End Using
        End If
        panelPub.Visible = True
        Bindgrid()
    End Sub
    Protected Sub Btnupdate_Click(sender As Object, e As EventArgs) Handles btnupdatePub.Click
        Dim year As String = txtyear.Text
        Dim Sec As String = txtSec.Text
        Dim SubSec As String = txtSub.Text
        Dim Link As String = txtLink.Text
        Dim selectedType As Integer = Convert.ToInt32(ddlTitle.SelectedValue)
        Dim ID As String = Labelpub.Text
        Dim constr As String = WebConfigurationManager.ConnectionStrings("dbConn_VSAPP").ConnectionString
        constr = Replace(constr, "password", Decrypt2("2fxKF+rsggSR+BM25c3IJLWBqS1Pu4Y5"))
        Dim query As String = "UPDATE OIT SET year=@year, title=@title, section=@section, SubSec=@SubSec, linkData=@linkData WHERE ID=@ID"
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand(query, con)
                cmd.Parameters.AddWithValue("@ID", ID)
                cmd.Parameters.AddWithValue("@year", year)
                cmd.Parameters.AddWithValue("@title", selectedType)
                cmd.Parameters.AddWithValue("@section", Sec)
                cmd.Parameters.AddWithValue("@linkData", Link)
                If String.IsNullOrEmpty(SubSec) Then
                    cmd.Parameters.AddWithValue("@SubSec", DBNull.Value)
                Else
                    cmd.Parameters.AddWithValue("@SubSec", SubSec)
                End If

                con.Open()
                cmd.ExecuteNonQuery()

            End Using
        End Using
        panelPub.Visible = True
        Bindgrid()



    End Sub
End Class
