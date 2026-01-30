Option Explicit On
Option Strict On
Imports System.Threading

Imports Dashboard.Encrypt
Imports Dashboard.ConnectDB
Imports Dashboard.GenPwd
Imports System.IO 'sms
Imports System.Net 'sms
Imports System.Xml 'sms

Imports Newtonsoft.Json.Linq
Partial Class login
    Inherits System.Web.UI.Page

    Dim ck As New HttpCookie("_Dvjwek43")
    Dim SQLRN As String

Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            Dim startTime As Date = Now
            '******************* Mobile ***********************
            'Dim SQLRN As String
            Dim dt As DataTable

            If Request.QueryString("arrStr") IsNot Nothing Then

                If Request.QueryString("arrStr") = "***" Then Exit Sub

                'Dim str As String = HttpUtility.UrlDecode(Request.QueryString("arrStr"))
                Dim str As String = Request.QueryString("arrStr")
                str = str.Split(CChar(":"))(0)

                str = Decrypt1(str)

                Dim returnStr As New ArrayList()
                returnStr.AddRange(str.Split(New Char() {","c}))

                SQLRN = "SELECT username, pw, last_login FROM dbo.[user] WHERE (user_id = " & CInt(returnStr(1)) & ")"

                dt = QueryDataTable(SQLRN, dbConn_itjobs, "itjobs")

                If dt.Rows.Count > 0 Then

                    Dim _cultureEnInfo As New System.Globalization.CultureInfo("en-US")

                    Dim dateLogin As DateTime = Convert.ToDateTime(dt.Rows(0)("last_login"), _cultureEnInfo)
                    Dim formattedDateLogin As String = dateLogin.ToString("yyyy-MM-dd HH:mm:00", _cultureEnInfo)

                    'formattedDateLogin = dateLogin.ToString("yyyy-MM-dd", _cultureEnInfo)


                    ' Check if returnStr array has enough elements before accessing returnStr(2)
                    If returnStr.Count > 2 AndAlso formattedDateLogin = CStr(returnStr(2)) Then
                        CallSubmit(dt.Rows(0)("username").ToString, dt.Rows(0)("pw").ToString)

                    Else
                        Response.Redirect("https://vs.mahidol.ac.th/portal", True)
                        Exit Sub
                    End If




                End If

                'Dim url As String = Request.Url.AbsolutePath '& "?" + nvc.ToString()
                'Response.Redirect("defaul.aspx")

            Else
                Response.Redirect("https://vs.mahidol.ac.th/portal", True)

            End If

        End If


        Close_objConn()
    End Sub



    Private Sub CallSubmit(username As String, pwd As String)

        If Not String.IsNullOrEmpty(username) OrElse String.IsNullOrEmpty(pwd) Then
            GoTo StartLoginProcess
        End If

StartLoginProcess:
        Dim dt As DataTable


        SQLRN = "SELECT  username, tmpwd, isActive FROM dbo.[user] WHERE  (username = '" & Trim(username) & "')" ' AND (NOT (tmpwd IS NULL)) "

        dt = QueryDataTable(SQLRN, dbConn_itjobs, "itjobs")

        SQLRN = "SELECT dbo.[user].user_id, dbo.title_technical.title_technicalName AS title_technical, dbo.[user].fname, dbo.[user].lname, dbo.[user].last_login, dbo.[user].permiss " &
               "FROM         dbo.[user] LEFT OUTER JOIN  dbo.title_technical ON dbo.[user].title_technicalID = dbo.title_technical.title_technicalID " &
                "WHERE     (dbo.[user].username = '" & Trim(username) & "') AND (NOT (dbo.[user].permiss IS NULL)) " &
                "GROUP BY dbo.[user].user_id, dbo.title_technical.title_technicalName, dbo.[user].fname, dbo.[user].lname, dbo.[user].pw, dbo.[user].tmpwd, dbo.[user].last_login, dbo.[user].permiss " &
                "HAVING (dbo.[user].pw = '" & Trim(pwd) & "') "

        SQLRN = "SELECT        dbo.[user].user_id, dbo.title_technical.title_technicalName AS title_technical, dbo.[user].fname, dbo.[user].lname, dbo.[user].last_login, dbo.[user].permiss, dbo.department.Check_id " &
"FROM            dbo.[user] INNER JOIN " &
                         "dbo.department ON dbo.[user].dept_id = dbo.department.dept_id LEFT OUTER JOIN " &
                         "dbo.title_technical ON dbo.[user].title_technicalID = dbo.title_technical.title_technicalID " &
"WHERE        (dbo.[user].username = '" & Trim(username) & "') AND (NOT (dbo.[user].permiss IS NULL)) " &
"GROUP BY dbo.[user].user_id, dbo.title_technical.title_technicalName, dbo.[user].fname, dbo.[user].lname, dbo.[user].pw, dbo.[user].tmpwd, dbo.[user].last_login, dbo.[user].permiss, dbo.department.Check_id " &
"HAVING        (dbo.[user].pw = '" & Trim(pwd) & "')"


        If dt.Rows.Count > 0 Then 'มี username

            If CInt(dt.Rows(0)("isActive")) = 0 Then
                ScriptManager.RegisterStartupScript(Me, [GetType](), "showalert", "alert('ชื่อผู้ใช้ " & username & " สิ้นสภาพการเป็นพนักงาน');", True)
                Exit Sub
            End If

            If dt.Rows(0)("tmpwd") IsNot DBNull.Value Then 'และมี(tmpwd)
                'SQLRN = SQLRN & "HAVING (dbo.[user].pw = '" & EncryptMD5(Trim(txtPassword.Text)) & "') OR (dbo.[user].tmpwd = '" & EncryptMD5(Trim(txtPassword.Text)) & "')"
                SQLRN = SQLRN & "OR (dbo.[user].tmpwd = '" & Trim(pwd) & "')"

                'Else 'tmpwd = NULL
                '    SQLRN = SQLRN & "HAVING ([user].pw = '" & EncryptMD5(Trim(txtPassword.Text)) & "')"
            End If

        End If

        'MsgBox(SQLRN)

        'Close_objConn()

        dt = QueryDataTable(SQLRN, dbConn_itjobs, "itjobs")

        'ตรวจสอบ และสร้าง cookie
        'If ck Is Nothing Then
        If Request.Cookies("ck") Is Nothing Then

            ck = New HttpCookie("_Dvjwek43")
            ck("user_id") = CStr(0)
            ck("loginstate") = CStr(0)
            ck.Expires.AddMonths(1)
            Response.Cookies.Add(ck)

        Else
            ck = Request.Cookies("_Dvjwek43")

        End If

        If dt.Rows.Count > 0 Then
            'MsgBox(dt.Rows(0)("user_id").ToString)
            Session("fname") = dt.Rows(0)("title_technical").ToString & dt.Rows(0)("fname").ToString & " " & dt.Rows(0)("lname").ToString

            'เก็บข้อมูลวันที่ login
            ck("user_id") = dt.Rows(0)("user_id").ToString

            ck("loginstate") = CStr(1)
            Session("loginstate") = 1

            If IsDBNull(dt.Rows(0)("last_login")) Then
                ck("lastvisit") = CStr(DateTime.Now)

            Else
                ck("lastvisit") = CStr(dt.Rows(0)("last_login"))
            End If

            Session("permiss") = dt.Rows(0)("permiss")

            ck.Expires.AddMonths(1)
            Response.Cookies.Add(ck)

            If dt.Rows(0)("user_id").ToString = dt.Rows(0)("Check_id").ToString Then
                Session.Timeout = 240

            Else
                Session.Timeout = 60 'Default = 20 min
            End If


            Try

                Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")

                SQLRN = "INSERT INTO pwHistory(dStamp, pwF, user_id, activities) " &
                "VALUES('" & DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") & "', " &
                " '" & Encrypt2(pwd) & "', " & ck("user_id") & " , '" & "LeaveOnline" & "') "

                Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("th-TH")

                QueryExecuteNonQuery(SQLRN, dbConn_itjobs)

                Close_objConn()


                Response.Redirect("Default.aspx", False)

            Catch ex As Exception
                'MsgBox(ex.Message)
                ScriptManager.RegisterStartupScript(Me, [GetType](), "showalert", "alert('" & ex.Message & "');", True)
            End Try
            'End With

        Else
            ScriptManager.RegisterStartupScript(Me, [GetType](), "showalert", "alert('ไม่มีชื่อผู้ใช้ " & username & " หรือ Password ไม่ถูกต้อง');", True)

        End If

        Close_objConn()

        Exit Sub

    End Sub

End Class
