Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports System.Web.Configuration
Imports Dashboard.ConnectDB
Imports Dashboard.Encrypt
Public Class MinibleV2
    Inherits System.Web.UI.MasterPage

    'Dim ck As New HttpCookie("_Dvjwek43")

    ' Public path As String
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

    End Sub
    'ใช้ทั้งหมด ในการ login 
    '    Public Sub SetAvatar()
    '        Dim ck As New HttpCookie("_Dvjwek43")
    '        ck = Request.Cookies("_Dvjwek43")
    '        path = "img_avatar/" & ck("user_id") & ".jpg"

    '        Dim g As Guid = Guid.NewGuid()
    '        Dim GuidString As String = Convert.ToBase64String(g.ToByteArray())
    '        GuidString = GuidString.Replace("=", "")
    '        GuidString = GuidString.Replace("+", "")

    '        'Dim isFile As Boolean = New FileInfo(Server.MapPath(path)).Exists
    '        Dim isFile As Boolean = False

    '        'On Error Resume Next
    '        Try
    '            Dim HttpRequest As HttpWebRequest = CType(System.Net.WebRequest.Create("https://vs.mahidol.ac.th/portal/img_avatar/" & ck("user_id") & ".jpg"), HttpWebRequest)

    '            'Dim response = CType(HttpRequest.GetResponse(), HttpWebResponse)
    '            Using response As HttpWebResponse = CType(HttpRequest.GetResponse(), HttpWebResponse)
    '                isFile = response.StatusCode = HttpStatusCode.OK
    '            End Using
    '        Catch ex As Exception
    '            'isFile = Response.StatusCode = HttpStatusCode.NotFound
    '        End Try

    '        If isFile = False Then
    '            path = "https://vs.mahidol.ac.th/portal/img_avatar/blank.jpg"

    '        Else
    '            path = "https://vs.mahidol.ac.th/portal/img_avatar/" & ck("user_id") & ".jpg"

    '            'avatar.ImageUrl = path & "?r=" & GuidString
    '        End If

    '        path = GetImageAsBase64(path)

    '        'path = path & "?r=" & GuidString

    '    End Sub

    '    Private Function GetImageAsBase64(ByVal url As String) As String
    '        Dim encodedUrl As String = Convert.ToBase64String(Encoding.[Default].GetBytes(url))

    '        Using client = New WebClient()
    '            Dim dataBytes As Byte() = client.DownloadData(New Uri(url))
    '            Dim encodedFileAsBase64 As String = Convert.ToBase64String(dataBytes)
    '            Return "data:image/jpg;base64," & encodedFileAsBase64
    '        End Using
    '    End Function

    '    Public Shared Function ImageToBase64(ByVal _imagePath As String) As String
    '        Using _image As System.Drawing.Image = System.Drawing.Image.FromFile(_imagePath)

    '            Using _mStream As MemoryStream = New MemoryStream()
    '                _image.Save(_mStream, _image.RawFormat)
    '                Dim _imageBytes As Byte() = _mStream.ToArray()
    '                Dim _base64String As String = Convert.ToBase64String(_imageBytes)
    '                Return "data:image/jpg;base64," & _base64String
    '            End Using
    '        End Using
    '    End Function

    '    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
    '        'session หมดอายุ ให้ไปหน้า login
    '        'https://www.aspsnippets.com/Articles/Reload-Refresh-and-Redirect-Pages-using-Meta-Tags-in-ASP.Net.aspx
    '        Response.AppendHeader("Refresh", Convert.ToString(Session.Timeout * 60) _
    '                              & ";url=Login.aspx")

    '        If ck Is Nothing Then
    '            ck = New HttpCookie("_Dvjwek43")
    '            ck("loginstate") = CStr(0)
    '            ck.Expires.AddMonths(1)
    '            Response.Cookies.Add(ck)
    '        End If

    '        ck = Request.Cookies("_Dvjwek43")

    '        If Session("fname") Is Nothing Then 'ถ้า session หมดอายุแล้ว
    '            If ck("loginstate") = "1" Then 'cookie ยังอยู่
    '                ck("loginstate") = "0" 'เขียน loginstate ใน cookie ให้เป็น 0
    '                Response.Cookies.Add(ck)
    '                'MsgBox(ck("loginstate"))
    '                'Response.Redirect("login.aspx?redirect=booking") 'redirect ไปหน้าแรก
    '                Response.Redirect("login.aspx") 'redirect ไปหน้าแรก
    '            End If
    '        End If

    '        SetAvatar()

    '        If Not Page.IsPostBack Then

    '            Dim lang As String = "en-US" ' ค่า default

    '            ' อ่านค่าภาษาใน Cookie
    '            If Not Request.Cookies("_Rvj2dHb37") Is Nothing Then
    '                Dim langCookie As HttpCookie = Request.Cookies("_Rvj2dHb37")
    '                If langCookie("language") IsNot Nothing Then
    '                    lang = langCookie("language")
    '                End If
    '            End If

    '            ' เปลี่ยนรูปธงและข้อความภาษาตาม cookie
    '            If lang = "th-TH" Then
    '                Image1.ImageUrl = "minible/layouts/assets/images/flags/thai.jpg"
    '                spLang.InnerText = "TH"
    '            ElseIf lang = "en-US" Then
    '                Image1.ImageUrl = "minible/layouts/assets/images/flags/us.jpg"
    '                spLang.InnerText = "EN"
    '            End If

    '            'If Page.Request.Url.AbsolutePath.ToLower().Contains("dashboard.aspx") Then
    '            '    MinibleBody.Attributes.Remove("data-layout")
    '            '    MinibleBody.Attributes.Remove("data-topbar")
    '            'End If

    '            If ck("loginstate") = "1" Then
    '                BindMessage()
    '            End If

    '            'If ddlLanguages.Items.FindByValue(CultureInfo.CurrentCulture.Name) IsNot Nothing Then
    '            '    ddlLanguages.Items.FindByValue(CultureInfo.CurrentCulture.Name).Selected = True
    '            'End If

    '            If ck("loginstate") = "1" Then
    '                sPanName.InnerText = Session("username")

    '                Exit Sub

    '                Dim SQLRN As String
    '                Dim dt As DataTable

    '                Dim str As String = Request.QueryString("arrStr")

    '                'If String.IsNullOrEmpty(HttpUtility.UrlDecode(Request.QueryString("arrStr"))) Then
    '                '    If Not String.IsNullOrEmpty(Session("arrStr")) Then
    '                '        str = HttpUtility.UrlDecode(Session("arrStr"))
    '                '        Response.Redirect("Default.aspx?arrStr=" & str, False)
    '                '        Exit Sub

    '                '    Else
    '                '        GoTo Clear_arrStr
    '                '    End If

    '                'End If

    '                str = Decrypt1(str.Split(":")(0))

    '                Dim returnStr As New ArrayList()
    '                returnStr.AddRange(str.Split(New Char() {","c}))

    '                SQLRN = "SELECT username, pw, last_login FROM dbo.[user] WHERE (user_id = " & CInt(returnStr(1)) & ")"

    '                dt = QueryDataTable(SQLRN, dbConn_itjobs, "itjobs")

    '                If dt.Rows.Count > 0 Then

    '                    Dim _cultureEnInfo As New System.Globalization.CultureInfo("en-US")

    '                    Dim dateLogin As DateTime = Convert.ToDateTime(dt.Rows(0)("last_login"), _cultureEnInfo)
    '                    Dim formattedDateLogin As String = dateLogin.ToString("yyyy-MM-dd HH:mm:00", _cultureEnInfo)

    '                    'MsgBox(formattedDateLogin & " = " & CStr(returnStr(2)))

    '                    If returnStr.Count > 2 AndAlso formattedDateLogin = CStr(returnStr(2)) Then
    '                        'MsgBox(dateLogin)

    '                    Else 'ไม่ตรงกัน
    'Clear_arrStr:
    '                        ck = Request.Cookies("_Dvjwek43")

    '                        ck = New HttpCookie("_Dvjwek43")
    '                        ck("loginstate") = CStr(0)
    '                        Response.Cookies.Add(ck)

    '                        Session.Clear()

    '                        Response.Redirect("Login.aspx")
    '                    End If
    '                End If



    '            Else
    '                ck = Request.Cookies("_Dvjwek43")

    '                ck = New HttpCookie("_Dvjwek43")
    '                ck("loginstate") = CStr(0)
    '                Response.Cookies.Add(ck)

    '                Session.Clear()

    '                Response.Redirect("login.aspx")
    '            End If

    '        End If
    '    End Sub

    '    Protected Sub LbSignOut_Click(sender As Object, e As System.EventArgs) Handles LbSignOut.Click

    '        If Page.IsPostBack Then
    '            'Dim cks As String() = Request.Cookies.AllKeys

    '            'For Each ck As String In cks
    '            'Response.Cookies(ck).Expires = DateTime.Now.AddHours(-1) 'ลบ cookie ทิ้งแล้วจะทักทายยังไง

    '            'Session.Clear()
    '            'Next

    '            ck = Request.Cookies("_Dvjwek43")

    '            ck = New HttpCookie("_Dvjwek43")
    '            ck("loginstate") = CStr(0)
    '            Response.Cookies.Add(ck)

    '            Session.Clear()

    '            'Dim ckAccess = New HttpCookie("_dfWxdjh35")

    '            'ckAccess = Request.Cookies("_dfWxdjh35")

    '            'MsgBox(ckAccess("ckUser") & " -- " & ckAccess("ckPwd"))

    '            Response.Redirect("Login.aspx")
    '            'ScriptManager.RegisterStartupScript(Me, [GetType](), "showalert", "alert('คุณได้ออกจากระบบแล้ว');location.href='default.aspx';", True)

    '        End If
    '    End Sub

    '    Protected Sub lbEN_Click(sender As Object, e As System.EventArgs) Handles lbEN.Click
    '        Dim SiteLanguage As New HttpCookie("_Rvj2dHb37")
    '        SiteLanguage("language") = "en-US"
    '        SiteLanguage.Expires = DateTime.Now.AddYears(1) ' ให้คุกกี้อยู่ได้นาน

    '        ' ✅ เพิ่มความปลอดภัย
    '        SiteLanguage.HttpOnly = True
    '        SiteLanguage.Secure = Request.IsSecureConnection

    '        Response.Cookies.Add(SiteLanguage)

    '        Response.Redirect(Request.RawUrl) ' โหลดหน้าใหม่เพื่อให้ culture ทำงาน

    '    End Sub

    '    Protected Sub lbTH_Click(sender As Object, e As System.EventArgs) Handles lbTH.Click
    '        Dim SiteLanguage As New HttpCookie("_Rvj2dHb37")
    '        SiteLanguage("language") = "th-TH"
    '        SiteLanguage.Expires = DateTime.Now.AddYears(1)

    '        ' ✅ เพิ่มความปลอดภัย
    '        SiteLanguage.HttpOnly = True
    '        SiteLanguage.Secure = Request.IsSecureConnection

    '        Response.Cookies.Add(SiteLanguage)

    '        Response.Redirect(Request.RawUrl)

    '    End Sub

    '    Protected Sub rptNotification_OnItemBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs)
    '        'bind repeater ตัวที่ 2 ให้เงื่อนไขเท่ากับแถวที่โหลด ParentMenuId =" & (CType((e.Item.DataItem), System.Data.DataRowView)).Row(0))
    '        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
    '            'Dim rptSubMenu As Repeater = TryCast(e.Item.FindControl("rptChildMenu"), Repeater)
    '            'rptSubMenu.DataSource = GetData("SELECT ParentMenuId, Title, Url FROM Menus WHERE ParentMenuId =" & (CType((e.Item.DataItem), System.Data.DataRowView)).Row(0))
    '            'rptSubMenu.DataBind()
    '            'MsgBox(CType(e.Item.DataItem, System.Data.DataRowView).Row(0))
    '        End If
    '    End Sub

    '    Private Sub BindMessage()

    '        If Request.Cookies("user_id") Is Nothing Then
    '            Dim dtNotification As DataTable = GetData("SELECT * FROM message WHERE (status = 1) AND user_id= " & ck("user_id") & " ")

    '            rptNotification.DataSource = dtNotification
    '            rptNotification.DataBind()

    '            If dtNotification.Rows.Count > 0 Then
    '                'divNotification.Attributes.Add("style", "display:block")
    '                sMsgNumber.Attributes.Add("style", "display:block")
    '                sMsgNumber.InnerText = dtNotification.Rows.Count.ToString()

    '            Else
    '                'divNotification.Attributes.Add("style", "display:none")
    '                sMsgNumber.Attributes.Add("style", "display:none")
    '            End If
    '        End If

    '    End Sub

    '    Private Shared Function GetData(query As String) As DataTable
    '        Dim strConnString As String = WebConfigurationManager.ConnectionStrings("dbConn_leave").ConnectionString
    '        strConnString = Replace(strConnString, "password", Decrypt2("2fxKF+rsggSR+BM25c3IJLWBqS1Pu4Y5"))

    '        Using con As New SqlConnection(strConnString)
    '            Using cmd As New SqlCommand()
    '                cmd.CommandText = query
    '                Using sda As New SqlDataAdapter()
    '                    cmd.Connection = con
    '                    sda.SelectCommand = cmd
    '                    Using ds As New DataSet()
    '                        Dim dt As New DataTable()
    '                        sda.Fill(dt)
    '                        Return dt
    '                    End Using
    '                End Using
    '            End Using
    '        End Using
    '    End Function

    '    Public Function msg_time(ByVal Dob As DateTime) As String
    '        Dim Now As DateTime = DateTime.Now
    '        Dim Years As Integer = New DateTime(DateTime.Now.Subtract(Dob).Ticks).Year - 1
    '        Dim PastYearDate As DateTime = Dob.AddYears(Years)
    '        Dim Months As Integer = 0
    '        For i As Integer = 1 To 12
    '            If PastYearDate.AddMonths(i) = Now Then
    '                Months = i
    '                Exit For
    '            ElseIf PastYearDate.AddMonths(i) >= Now Then
    '                Months = i - 1
    '                Exit For
    '            End If
    '        Next
    '        Dim Days As Integer = Now.Subtract(PastYearDate.AddMonths(Months)).Days
    '        Dim Hours As Integer = Now.Subtract(PastYearDate).Hours
    '        Dim Minutes As Integer = Now.Subtract(PastYearDate).Minutes
    '        Dim Seconds As Integer = Now.Subtract(PastYearDate).Seconds

    '        'Return [String].Format("Age: {0} Year(s) {1} Month(s) {2} Day(s) {3} Hour(s) {4} Second(s)", Years, Months, Days, Hours, Seconds)
    '        If Days > 0 Then
    '            Return [String].Format("{0} Days {1} Hours {2} Mins ago", Days, Hours, Minutes)

    '        Else
    '            If Hours > 0 Then
    '                Return [String].Format("{0} Hours {1} Mins ago", Hours, Minutes)

    '            Else
    '                Return [String].Format("{0} Mins ago", Minutes)
    '            End If
    '        End If




    '    End Function
End Class

