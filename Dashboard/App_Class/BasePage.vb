Imports System.Globalization
Imports System.Threading

Public Class BasePage
    Inherits System.Web.UI.Page
    Protected Overrides Sub InitializeCulture()
        Dim language As String = "en-US" ' ค่าเริ่มต้น

        ' อ่านค่าจากคุกกี้ที่ชื่อ "_Rvj2dHb37"
        If Not Request.Cookies("_Rvj2dHb37") Is Nothing Then
            Dim langCookie As HttpCookie = Request.Cookies("_Rvj2dHb37")
            If langCookie("language") IsNot Nothing Then
                language = langCookie("language")
            End If
        End If

        ' ✅ ตรวจสอบค่าภาษาที่รองรับ (ป้องกันการถูกแก้ไข cookie)
        If language <> "en-US" AndAlso language <> "th-TH" Then
            language = "en-US"
        End If

        ' ตั้งค่าวัฒนธรรม
        Thread.CurrentThread.CurrentCulture = New CultureInfo(language)
        Thread.CurrentThread.CurrentUICulture = New CultureInfo(language)

    End Sub
End Class

