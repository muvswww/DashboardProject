Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports Dashboard.ConnectDB

Public Class BindDropdownlist
    Public Shared Sub BindDDL(SQLRN As String, strConnString As String, DB As String, DDL_NAME As DropDownList, DDL_ID As String, DDL_TEXT As String, select_option As String)

        Dim dr As SqlDataReader

        dr = QueryDataReader2(SQLRN, strConnString, DB, Nothing)

        If dr.HasRows Then
            DDL_NAME.Items.Clear()
            DDL_NAME.DataSource = dr
            DDL_NAME.DataValueField = DDL_ID
            DDL_NAME.DataTextField = DDL_TEXT
            DDL_NAME.DataBind()

            If Not String.IsNullOrEmpty(select_option) Then
                DDL_NAME.Items.Insert(0, New ListItem(select_option, "0"))
            End If
        End If

        dr.Close()
        Close_objConn()
    End Sub
End Class
