Option Explicit On
Option Strict On

Imports System.Net.Mail
Imports System.Net

Imports Dashboard.Encrypt
Imports Dashboard.ConnectDB

'send mail
Imports System.Net.Mime
Imports System.IO
Imports System.ComponentModel
Imports System.Threading
Imports System.Web.Hosting

Public Class useGmail

    Public Shared Sub SendGmail(ByVal EmailTo As String, ByVal Subject As String, ByVal CC As String, BodyHtml As Boolean, ByVal msg As String)

        ServicePointManager.SecurityProtocol = DirectCast(3072, SecurityProtocolType)

        Try
            'Using mail As New MailMessage("aekkapol.sri@mahidol.edu", mailto) 'ส่งไปหาหัวหน้าชั้นต้น และผู้อนุมัติ
            Using mail As New MailMessage("muvs.service@gmail.com", EmailTo) 'ส่งไปหาหัวหน้าชั้นต้น และผู้อนุมัติ
                'mail.Subject = "ขออนุญาตลา" & dt.Rows(0)("leave_name").ToString
                mail.Subject = Subject

                mail.Body = msg

                'If fuAttachment.HasFile Then
                '    Dim FileName As String = Path.GetFileName(fuAttachment.PostedFile.FileName)
                '    mm.Attachments.Add(New Attachment(fuAttachment.PostedFile.InputStream, FileName))
                'End If

                '******  Attachment  '******
                'add our attachment
                'Dim imgAtt As New Attachment(HttpContext.Current.Server.MapPath("~/images/LOGOth7.png"))
                'imgAtt.ContentId = "LOGOth7.png"
                'mail.Attachments.Add(imgAtt)
                '******  Attachment  '******

                mail.IsBodyHtml = BodyHtml

                If BodyHtml = True Then
                    'mail.Body = "<br/><img src=cid:contentID>" & msg

                    '**********  embedded-images-inline-Attachments  **********
                    'http://stackoverflow.com/questions/2317012/attaching-image-in-the-body-of-mail-in-c-sharp

                    Dim attachmentPath As String = HttpContext.Current.Server.MapPath("~/images/LOGOth7.png")
                    Dim inline As New Attachment(attachmentPath)
                    inline.ContentDisposition.Inline = True
                    inline.ContentDisposition.DispositionType = DispositionTypeNames.Inline
                    inline.ContentId = "contentID"
                    inline.ContentType.MediaType = "image/png"
                    inline.ContentType.Name = Path.GetFileName(attachmentPath)

                    mail.Attachments.Add(inline)

                    '**********  embedded-images-inline-Attachments  **********
                End If


                If CC.Length > 0 Then
                    'Dim CC As New MailAddress("aekkapol.sri@mahidol.ac.th,yupin.kum@mahidol.ac.th")
                    mail.CC.Add(CC)
                End If

                Dim smtp As New SmtpClient()
                smtp.Host = "smtp.gmail.com"
                smtp.EnableSsl = True

                Dim NetworkCred As New NetworkCredential("muvs.service@gmail.com", Decrypt2("22w1AXKDaQp6WaylBPIjkkiyGuvZVdTl"))

                '1.Turn on 2-Step Verification in your google account. This step is required as Google only allows generating passwords for apps on accounts that have 2-Step Verification enabled.
                '2.Go to generate apps password (https://myaccount.google.com/apppasswords) and generate a password for your app.

                'app password
                'vs app
                '22w1AXKDaQp6WaylBPIjkkiyGuvZVdTl|rjdbofxxiybigbwp

                'muvs.service
                '7SGJynIQlrj48aUtKsEgOpGA94Q7px/w|yzfuarjyrlxkxmyx

                'https://stackoverflow.com/questions/72547853/unable-to-send-email-in-c-sharp-less-secure-app-access-not-longer-available/72553362#72553362

                

                'backup email aekkapol.s@gmail.com pw:6i
                'https://www.vbforums.com/showthread.php?895413-Sending-Email-via-Gmail-changing-for-May-30
                smtp.UseDefaultCredentials = True
                smtp.Credentials = NetworkCred
                smtp.Port = 587
                smtp.Send(mail)

                'ClientScriptManager.RegisterStartupScript(Me.GetType, "alert", "alert('Email Sent.');", True)
            End Using

        Catch ex As Exception
            ' Dim message As String = String.Format("Message: {0}\n\n", ex.Message)
            ' message &= String.Format("StackTrace: {0}\n\n", ex.StackTrace.Replace(Environment.NewLine, String.Empty))
            'message &= String.Format("Source: {0}\n\n", ex.Source.Replace(Environment.NewLine, String.Empty))
            'message &= String.Format("TargetSite: {0}", ex.TargetSite.ToString().Replace(Environment.NewLine, String.Empty))
            'ClientScriptManager.RegisterStartupScript(this.GetType(), "alert", "alert(""" & message & """);", True)
            HttpContext.Current.Response.Write("<script>alert(""" & ex.ToString & """)</script>")

        End Try

    End Sub

    Public Shared Sub SendAsyncEmail(ByVal EmailTo As String, ByVal Subject As String, ByVal CC As String, BodyHtml As Boolean, ByVal msg As String)
        Try
            Dim attachmentPath As String = HttpContext.Current.Server.MapPath("~/images/LOGOth7.png")
            Dim emailThread As New Thread(Sub() SendGmail2(EmailTo, Subject, CC, BodyHtml, msg, attachmentPath))
            emailThread.IsBackground = True
            emailThread.Start()

            'HttpContext.Current.Response.Write("<script>alert('The email was sent successfully.')</script>")

        Catch ex As Exception
            HttpContext.Current.Response.Write("<script>alert(""" & ex.ToString & """)</script>")
        End Try

    End Sub

    Private Shared Sub SendGmail2(ByVal EmailTo As String, ByVal Subject As String, ByVal CC As String, BodyHtml As Boolean, ByVal msg As String, attachmentPath As String)
        'Private Shared Sub SendEmail(ByVal emailTo As String, ByVal subject As String, ByVal body As String, ByVal postedFile As HttpPostedFile)
        Try
            Using mm As New MailMessage("muvs.service@gmail.com", EmailTo)
                mm.Subject = Subject
                mm.Body = msg
                'If postedFile IsNot Nothing AndAlso postedFile.ContentLength > 0 Then
                '    Dim fileName As String = Path.GetFileName(postedFile.FileName)
                '    mm.Attachments.Add(New Attachment(postedFile.InputStream, fileName))
                'End If

                mm.IsBodyHtml = BodyHtml

                If BodyHtml = True Then
                    'mail.Body = "<br/><img src=cid:contentID>" & msg

                    '**********  embedded-images-inline-Attachments  **********
                    'http://stackoverflow.com/questions/2317012/attaching-image-in-the-body-of-mail-in-c-sharp

                    'Dim attachmentPath As String = HttpContext.Current.Server.MapPath("~/images/LOGOth7.png")

                    'Dim attachmentPath As String = HostingEnvironment.MapPath("~/images/LOGOth7.png")

                    Dim inline As New Attachment(attachmentPath)
                    inline.ContentDisposition.Inline = True
                    inline.ContentDisposition.DispositionType = DispositionTypeNames.Inline
                    inline.ContentId = "contentID"
                    inline.ContentType.MediaType = "image/png"
                    inline.ContentType.Name = Path.GetFileName(attachmentPath)

                    mm.Attachments.Add(inline)

                    '**********  embedded-images-inline-Attachments  **********
                End If

                If CC.Length > 0 Then
                    'Dim CC As New MailAddress("aekkapol.sri@mahidol.ac.th,yupin.kum@mahidol.ac.th")
                    mm.CC.Add(CC)
                End If

                Dim smtp As New SmtpClient()
                smtp.Host = "smtp.gmail.com"
                smtp.EnableSsl = True
                smtp.Port = 587
                smtp.UseDefaultCredentials = False
                'smtp.Credentials = New NetworkCredential(emailFrom, password)
                smtp.Credentials = New NetworkCredential("muvs.service@gmail.com", Decrypt2("22w1AXKDaQp6WaylBPIjkkiyGuvZVdTl"))
                smtp.Send(mm)
            End Using

        Catch ex As Exception
            HttpContext.Current.Response.Write("<script>alert(""" & ex.ToString & """)</script>")
        End Try
    End Sub



End Class
