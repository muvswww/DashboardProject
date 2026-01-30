Imports System.Data.SqlClient
Imports System.IO
Imports iTextSharp.text.pdf
Imports Dashboard.ConnectDB
Imports System.Drawing
Imports System.Drawing.Imaging

Public Class WebForm1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim user_id As Integer = 157
        ' --- ดึงข้อมูลจาก Database ลง DataTable ---
        Dim dt As New DataTable()
        Using con As New SqlConnection(dbConn_itjobs)
            Dim SQLRN As String
            '            SQLRN = "SELECT        [user].user_id, CASE WHEN title_technical.title_technicalName IS NULL THEN title.title_name ELSE title_technical.title_technicalName END + [user].fname + SPACE(2) + [user].lname AS fname, 
            '                         CASE WHEN [user].special1 = 1 AND [user].special2 = 1 THEN [user].position + ' (เชี่ยวชาญพิเศษ)' WHEN [user].special1 = 1 THEN [user].position + ' (ชำนาญการพิเศษ)' ELSE [user].position END AS position, [user].phone, 
            '                         dbo.division.division_name, user_Manager1.user_id AS Manager1_user_id, CASE WHEN title_technical_Manager1.title_technicalName IS NULL 
            '                         THEN title_Manager1.title_name ELSE title_technical_Manager1.title_technicalName END + user_Manager1.fname + SPACE(2) + user_Manager1.lname AS Manager1, Manager1.manager_position AS position_Manager1, 
            '                         dbo.department.dept_name, Manager2.user_id AS Manager2_user_id, CASE WHEN title_technical_Manager2.title_technicalName IS NULL 
            '                         THEN title_Manager2.title_name ELSE title_technical_Manager2.title_technicalName END + user_Manager2.fname + SPACE(2) + user_Manager2.lname AS Manager2, Manager2.manager_position AS position_Manager2
            'FROM            dbo.title AS title_Manager1 RIGHT OUTER JOIN
            '                         dbo.[user] AS user_Manager1 INNER JOIN
            '                         dbo.division ON user_Manager1.user_id = dbo.division.manager_id INNER JOIN
            '                         dbo.manager AS Manager1 ON user_Manager1.user_id = Manager1.user_id LEFT OUTER JOIN
            '                         dbo.title_technical AS title_technical_Manager1 ON user_Manager1.title_technicalID = title_technical_Manager1.title_technicalID ON title_Manager1.title_id = user_Manager1.title_id RIGHT OUTER JOIN
            '                         dbo.[user] AS user_Manager2 INNER JOIN
            '                         dbo.manager AS Manager2 ON user_Manager2.user_id = Manager2.user_id INNER JOIN
            '                         dbo.title AS title_Manager2 ON user_Manager2.title_id = title_Manager2.title_id INNER JOIN
            '                         dbo.title_technical AS title_technical_Manager2 ON user_Manager2.title_technicalID = title_technical_Manager2.title_technicalID RIGHT OUTER JOIN
            '                         dbo.department INNER JOIN
            '                         dbo.[user] AS [user] ON dbo.department.dept_id = [user].dept_id ON Manager2.manager_id = dbo.department.manager_id ON dbo.division.division_id = [user].division_id LEFT OUTER JOIN
            '                         dbo.title AS title ON [user].title_id = title.title_id LEFT OUTER JOIN
            '                         dbo.title_technical AS title_technical ON [user].title_technicalID = title_technical.title_technicalID
            'WHERE        ([user].isActive = 1) AND ([user].user_id = " & user_id & ")"

            SQLRN = "SELECT          [user].user_id, CASE WHEN title_technical.title_technicalName IS NULL THEN title.title_name ELSE title_technical.title_technicalName END + [user].fname + SPACE(2) + [user].lname AS fname, CASE WHEN [user].special1 = 1 AND 
                                     [user].special2 = 1 THEN [user].position + ' (เชี่ยวชาญพิเศษ)' WHEN [user].special1 = 1 THEN [user].position + ' (ชำนาญการพิเศษ)' ELSE [user].position END AS position, [user].phone, dbo.division.division_name, 
                                     user_1.fname AS Expr1, user_2.fname AS Expr2
            FROM            dbo.[user] AS [user] INNER JOIN
                                     dbo.department ON [user].dept_id = dbo.department.dept_id INNER JOIN
                                     dbo.manager ON dbo.department.manager_id = dbo.manager.manager_id INNER JOIN
                                     dbo.[user] AS user_2 ON dbo.manager.user_id = user_2.user_id LEFT OUTER JOIN
                                     dbo.[user] AS user_1 INNER JOIN
                                     dbo.division ON user_1.user_id = dbo.division.manager_id ON [user].division_id = dbo.division.division_id LEFT OUTER JOIN
                                     dbo.title ON [user].title_id = dbo.title.title_id LEFT OUTER JOIN
                                     dbo.title_technical ON [user].title_technicalID = dbo.title_technical.title_technicalID
            WHERE        (dbo.manager.session = 0) AND ([user].isActive = 1)"

            Using cmd As New SqlCommand(SQLRN, con)
                Using sda As New SqlDataAdapter(cmd)
                    sda.Fill(dt)
                End Using
            End Using
        End Using

        If dt.Rows.Count > 0 Then

            For Each row As DataRow In dt.Rows
                user_id = CInt(row("user_id"))

                'CropJPG(user_id)
                CorpJPGConvertPNG(user_id)
            Next
        End If

        Exit Sub

        If dt.Rows.Count > 0 Then

            ' --- PDF Template ---
            Dim templatePath As String = "Z:\Desktop\แจ้งซ่อมพลาธิการ\67\PalaForm.pdf"

            ' --- สร้าง MemoryStream แทน FileStream ---
            Using ms As New MemoryStream()
                Using reader As New PdfReader(templatePath)
                    Using stamper As New PdfStamper(reader, ms)
                        Dim formFields As AcroFields = stamper.AcroFields

                        ' ใส่ค่าใน field "Name"
                        formFields.SetField("Name", dt.Rows(0)("fname").ToString())
                        formFields.SetField("Position", dt.Rows(0)("position").ToString())
                        formFields.SetField("Phone", dt.Rows(0)("phone").ToString())

                        ' ให้ iTextSharp สร้าง Appearance ใหม่
                        'formFields.GenerateAppearances = False ' ปิดอัตโนมัติ
                        ' ใส่เครื่องหมายถูก
                        'formFields.SetField("CheckBox1", "Yes")  ' ✅ Checked

                        formFields.SetField("Chk1", "P")


                        formFields.SetField("detail", "1.ปิดช่องแอร์เดิม
2.แก้ไขกุญแจประตูกระจกเดิมให้ใช้งานได้
3.เดินสายแลน (ใส่ flex) จากหลังห้องตรวจ ไปยังห้อง server ชั้นใต้ดิน
4.เพิ่มปลั๊กไฟห้อง server ชั้นใต้ดิน แบบ emergency เพื่อรองรับการทำงานเครื่องปั่นไฟ")

                        formFields.SetField("location", "รพส.ประศุอาทร")

                        formFields.SetField("remark", "จะเริ่มใช้งาน 15 พ.ย. 2564")

                        formFields.SetField("Manager1", dt.Rows(0)("Manager1").ToString())

                        formFields.SetField("Manager2", dt.Rows(0)("Manager2").ToString())

                        'CropJPG(user_id)

                        Dim fieldPositions = formFields.GetFieldPositions("request_sign")
                        If fieldPositions IsNot Nothing AndAlso fieldPositions.Count > 0 Then
                            Dim rect As iTextSharp.text.Rectangle = fieldPositions(0).position

                            ' 🔹 โหลดรูป (ลายเซ็น)
                            Dim signImg As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance("\\10.33.1.104\vswebsite$\Website\LeaveOnline\SignatureFiles\p\" & user_id & ".png")

                            ' ปรับขนาดให้พอดีกับช่องฟิลด์
                            'signImg.ScaleToFit(rect.Width, rect.Height)

                            ' กำหนดขนาดภาพ (ปรับให้พอดีกับตำแหน่ง)
                            signImg.ScaleAbsolute(120, 50) ' width=120, height=50

                            ' 🔹 คำนวณตำแหน่งตรงกลางฟิลด์
                            Dim posX As Single = rect.Left + (rect.Width - signImg.ScaledWidth) / 2
                            Dim posY As Single = rect.Bottom + (rect.Height - signImg.ScaledHeight) / 2

                            ' วางรูปตรงกลาง
                            signImg.SetAbsolutePosition(posX, posY)

                            ' เพิ่มรูปไปที่หน้าเดียวกับฟิลด์
                            Dim pdfContent = stamper.GetOverContent(fieldPositions(0).page)
                            pdfContent.AddImage(signImg)
                        End If

                        Dim Manager1_user_id As Integer

                        If dt.Rows(0)("Manager1") IsNot DBNull.Value Then
                            Manager1_user_id = dt.Rows(0)("Manager1_user_id").ToString()

                        Else
                            formFields.SetField("Manager1", dt.Rows(0)("Manager2").ToString())

                            Manager1_user_id = dt.Rows(0)("Manager2_user_id").ToString()
                        End If

                        Dim Manager1_sign = formFields.GetFieldPositions("Manager1_sign")
                        If Manager1_sign IsNot Nothing AndAlso Manager1_sign.Count > 0 Then
                            Dim rect As iTextSharp.text.Rectangle = Manager1_sign(0).position

                            ' 🔹 โหลดรูป (ลายเซ็น)
                            Dim signImg As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance("\\10.33.1.104\vswebsite$\Website\LeaveOnline\SignatureFiles\p\" & Manager1_user_id & ".png")

                            ' ปรับขนาดให้พอดีกับช่องฟิลด์
                            'signImg.ScaleToFit(rect.Width, rect.Height)

                            ' กำหนดขนาดภาพ (ปรับให้พอดีกับตำแหน่ง)
                            signImg.ScaleAbsolute(120, 50) ' width=120, height=50

                            ' 🔹 คำนวณตำแหน่งตรงกลางฟิลด์
                            Dim posX As Single = rect.Left + (rect.Width - signImg.ScaledWidth) / 2
                            Dim posY As Single = rect.Bottom + (rect.Height - signImg.ScaledHeight) / 2

                            ' วางรูปตรงกลาง
                            signImg.SetAbsolutePosition(posX, posY)

                            ' เพิ่มรูปไปที่หน้าเดียวกับฟิลด์
                            Dim pdfContent = stamper.GetOverContent(Manager1_sign(0).page)
                            pdfContent.AddImage(signImg)
                        End If


                        ' ทำให้ฟิลด์ถูกล็อก
                        stamper.FormFlattening = True
                    End Using
                End Using

                ' --- ส่ง PDF ไป WebBrowser / Browser ---
                Response.Clear()
                Response.ContentType = "application/pdf"
                Response.AddHeader("Content-Disposition", "inline; filename=PalaForm.pdf")
                Response.OutputStream.Write(ms.ToArray(), 0, ms.ToArray().Length)
                Response.Flush()
                Response.End()
            End Using

        End If


    End Sub

    Public Sub CorpJPGConvertPNG(user_id As Integer)
        Dim inputPath As String = Server.MapPath("SignatureFiles\" & user_id & ".jpg")
        Dim outputPath As String = Server.MapPath("SignatureFiles\p\" & user_id & ".png")

        ' 🔹 ถ้าไม่มี JPG ไม่ต้องทำ
        If Not File.Exists(inputPath) Then
            Console.WriteLine("✔ ไม่มีไฟล์ JPG : " & inputPath)
            Return
        End If

        ' โหลดภาพต้นฉบับ
        Using bmp As New Bitmap(inputPath)
            ' สร้าง Bitmap โปร่งใส
            Dim transparentBmp As New Bitmap(bmp.Width, bmp.Height, PixelFormat.Format32bppArgb)

            ' 1) ลบพื้นหลังสีขาว โดยใช้ค่าความสว่าง (Luminance) เป็นเกณฑ์
            Dim threshold As Double = 0.9 ' กำหนดค่า Threshold (0.0 ถึง 1.0), ลองปรับค่านี้เพื่อความเหมาะสม
            For y As Integer = 0 To bmp.Height - 1
                For x As Integer = 0 To bmp.Width - 1
                    Dim pixel As Color = bmp.GetPixel(x, y)
                    Dim luminance As Double = (0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B) / 255.0
                    If luminance > threshold Then
                        transparentBmp.SetPixel(x, y, Color.FromArgb(0, 0, 0, 0)) ' ทำให้โปร่งใส
                    Else
                        transparentBmp.SetPixel(x, y, pixel) ' เก็บสีเดิม
                    End If
                Next
            Next

            ' 2) หา bounding box (พื้นที่ที่ไม่โปร่งใส)
            Dim minX As Integer = bmp.Width
            Dim minY As Integer = bmp.Height
            Dim maxX As Integer = -1
            Dim maxY As Integer = -1

            For y As Integer = 0 To transparentBmp.Height - 1
                For x As Integer = 0 To transparentBmp.Width - 1
                    Dim px As Color = transparentBmp.GetPixel(x, y)
                    If px.A > 0 Then ' ถ้ามีหมึก (ไม่ใช่พิกเซลโปร่งใส)
                        If x < minX Then minX = x
                        If y < minY Then minY = y
                        If x > maxX Then maxX = x
                        If y > maxY Then maxY = y
                    End If
                Next
            Next

            ' ป้องกันกรณีไม่มีลายเซ็น
            If maxX < 0 OrElse maxY < 0 Then
                Console.WriteLine("❌ ไม่พบลายเซ็นในภาพ")
                Return
            End If

            ' 3) ครอปภาพลายเซ็นจริง ๆ
            Dim croppedWidth As Integer = maxX - minX + 1
            Dim croppedHeight As Integer = maxY - minY + 1
            Dim sourceRect As New Rectangle(minX, minY, croppedWidth, croppedHeight)
            Using croppedBmp As New Bitmap(croppedWidth, croppedHeight, PixelFormat.Format32bppArgb)
                Using gCrop As Graphics = Graphics.FromImage(croppedBmp)
                    gCrop.DrawImage(transparentBmp, New Rectangle(0, 0, croppedWidth, croppedHeight), sourceRect, GraphicsUnit.Pixel)
                End Using

                ' 4) สร้างภาพสุดท้ายขนาด 658x318 และวางลายเซ็นที่ครอปแล้วลงตรงกลาง
                Dim finalWidth As Integer = 658
                Dim finalHeight As Integer = 318
                Dim padLeft As Integer = 80
                Dim padRight As Integer = 80
                Dim padBottom As Integer = 60 ' กำหนด padding ด้านล่างที่คุณต้องการ

                Using finalBmp As New Bitmap(finalWidth, finalHeight, PixelFormat.Format32bppArgb)
                    Using g As Graphics = Graphics.FromImage(finalBmp)
                        g.Clear(Color.FromArgb(0, 0, 0, 0)) ' ทำให้พื้นหลังโปร่งใส

                        Dim newWidth As Integer
                        Dim newHeight As Integer

                        ' กำหนดขนาดสูงสุดของลายเซ็นที่ต้องการ
                        Dim maxSignatureDimension As Integer = 250

                        ' คำนวณอัตราส่วนการปรับขนาด
                        Dim ratio As Double = Math.Min(CDbl(maxSignatureDimension) / croppedBmp.Width, CDbl(maxSignatureDimension) / croppedBmp.Height)
                        If croppedBmp.Width > 0 AndAlso croppedBmp.Height > 0 Then
                            newWidth = CInt(croppedBmp.Width * ratio)
                            newHeight = CInt(croppedBmp.Height * ratio)

                            ' ตรวจสอบไม่ให้ขนาดเกินพื้นที่ Content
                            Dim maxContentWidth As Integer = finalWidth - (padLeft + padRight)
                            Dim maxContentHeight As Integer = finalHeight - (padBottom)
                            If newWidth > maxContentWidth OrElse newHeight > maxContentHeight Then
                                Dim contentRatio As Double = Math.Min(CDbl(maxContentWidth) / croppedBmp.Width, CDbl(maxContentHeight) / croppedBmp.Height)
                                newWidth = CInt(croppedBmp.Width * contentRatio)
                                newHeight = CInt(croppedBmp.Height * contentRatio)
                            End If

                            If newWidth < 1 Then newWidth = 1
                            If newHeight < 1 Then newHeight = 1
                        Else
                            newWidth = 1
                            newHeight = 1
                        End If

                        ' คำนวณตำแหน่งที่จะวาดลายเซ็น (กึ่งกลางแนวนอน, ชิดขอบล่างแนวตั้ง)
                        Dim drawX As Integer = CInt((finalWidth - newWidth) / 2)
                        Dim drawY As Integer = finalHeight - newHeight - padBottom
                        'Dim drawY As Integer = CInt((finalHeight - newHeight) / 2)

                        g.DrawImage(croppedBmp, New Rectangle(drawX, drawY, newWidth, newHeight), New Rectangle(0, 0, croppedBmp.Width, croppedBmp.Height), GraphicsUnit.Pixel)
                    End Using

                    ' 5) เซฟเป็น PNG
                    finalBmp.Save(outputPath, ImageFormat.Png)
                End Using
            End Using
        End Using
        Console.WriteLine("✔ แปลงเรียบร้อย: " & outputPath)
    End Sub
    Public Sub CorpJPGConvertPNG_old(user_id As Integer)
        Dim inputPath As String = Server.MapPath("SignatureFiles\" & user_id & ".jpg")
        Dim outputPath As String = Server.MapPath("SignatureFiles\p\" & user_id & ".png")

        'Dim inputPath As String = "\\10.33.1.104\vswebsite$\Website\LeaveOnline\SignatureFiles\" & user_id & ".jpg"
        'Dim outputPath As String = "\\10.33.1.104\vswebsite$\Website\LeaveOnline\SignatureFiles\p\" & user_id & ".png"

        ' 🔹 ถ้าไม่มี JPG ไม่ต้องทำ
        If Not File.Exists(inputPath) Then
            Console.WriteLine("✔ ไม่มีไฟล์ JPG : " & outputPath)
            Return
        End If

        ' 🔹 ถ้ามี PNG อยู่แล้ว ไม่ต้องทำซ้ำ
        If File.Exists(outputPath) Then
            Console.WriteLine("✔ ไฟล์ PNG มีอยู่แล้ว: " & outputPath)
            Return
        End If

        ' โหลดภาพต้นฉบับ
        Using bmp As New Bitmap(inputPath)
            ' สร้าง Bitmap โปร่งใส
            Dim newBmp As New Bitmap(bmp.Width, bmp.Height, PixelFormat.Format32bppArgb)

            ' 1) ลบพื้นหลังสีขาว โดยใช้ค่าความสว่าง (Luminance) เป็นเกณฑ์
            Dim threshold As Double = 0.9 ' กำหนดค่า Threshold (0.0 ถึง 1.0), ลองปรับค่านี้เพื่อความเหมาะสม

            For y As Integer = 0 To bmp.Height - 1
                For x As Integer = 0 To bmp.Width - 1
                    Dim pixel As Color = bmp.GetPixel(x, y)

                    ' คำนวณค่าความสว่าง (Luminance) ของพิกเซล
                    ' สูตร: 0.299*R + 0.587*G + 0.114*B (มาตรฐาน sRGB)
                    Dim luminance As Double = (0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B) / 255.0

                    If luminance > threshold Then
                        ' ถ้าความสว่างสูงกว่าเกณฑ์ -> โปร่งใส
                        newBmp.SetPixel(x, y, Color.FromArgb(0, 0, 0, 0))
                    Else
                        ' ถ้าความสว่างต่ำกว่าเกณฑ์ -> เก็บสีเดิม
                        newBmp.SetPixel(x, y, pixel)
                    End If
                Next
            Next

            ' 2) หา bounding box (พื้นที่ที่ไม่โปร่งใส)
            Dim minX As Integer = bmp.Width - 1
            Dim minY As Integer = bmp.Height - 1
            Dim maxX As Integer = 0
            Dim maxY As Integer = 0

            For y As Integer = 0 To newBmp.Height - 1
                For x As Integer = 0 To newBmp.Width - 1
                    Dim px As Color = newBmp.GetPixel(x, y)
                    If px.A > 0 Then ' ถ้ามีหมึก
                        If x < minX Then minX = x
                        If y < minY Then minY = y
                        If x > maxX Then maxX = x
                        If y > maxY Then maxY = y
                    End If
                Next
            Next

            ' ป้องกันกรณีไม่มีลายเซ็น
            If minX >= maxX OrElse minY >= maxY Then
                Console.WriteLine("❌ ไม่พบลายเซ็นในภาพ")
                Return
            End If

            ' 3) สร้างภาพเปล่าขนาด 658x138
            Dim finalWidth As Integer = 658
            Dim finalHeight As Integer = 318
            Dim desiredPadding As Integer = 80 ' กำหนด padding ที่ต้องการ (ลองปรับค่านี้)
            ' ถ้าอยากให้มีระยะห่างรอบ ๆ ชัดเจนขึ้น ให้เพิ่มค่านี้ เช่น 5, 10, 20

            Using finalBmp As New Bitmap(finalWidth, finalHeight, PixelFormat.Format32bppArgb)
                Using g As Graphics = Graphics.FromImage(finalBmp)
                    ' ทำให้พื้นหลังของภาพสุดท้ายเป็นโปร่งใส
                    g.Clear(Color.FromArgb(0, 0, 0, 0))

                    ' กำหนด Rectange ของส่วนลายเซ็นที่จะดึงมาจาก newBmp
                    Dim sourceRect As New Rectangle(minX, minY, (maxX - minX + 1), (maxY - minY + 1))

                    ' คำนวณพื้นที่ที่เหลือสำหรับลายเซ็นหลังจากหัก padding ออกจากขนาดสุดท้าย
                    Dim maxContentWidth As Integer = finalWidth - (2 * desiredPadding)
                    Dim maxContentHeight As Integer = finalHeight - (2 * desiredPadding)

                    ' ตรวจสอบไม่ให้ขนาดเนื้อหาน้อยกว่า 1
                    If maxContentWidth < 1 Then maxContentWidth = 1
                    If maxContentHeight < 1 Then maxContentHeight = 1

                    ' คำนวณขนาดใหม่ของลายเซ็นโดยรักษาสัดส่วนและพอดีกับ maxContentSize
                    Dim newWidth As Integer
                    Dim newHeight As Integer
                    Dim ratioX As Double = CDbl(maxContentWidth) / sourceRect.Width
                    Dim ratioY As Double = CDbl(maxContentHeight) / sourceRect.Height
                    Dim ratio As Double = Math.Min(ratioX, ratioY)

                    newWidth = CInt(sourceRect.Width * ratio)
                    newHeight = CInt(sourceRect.Height * ratio)

                    ' คำนวณตำแหน่งที่จะวาดลายเซ็นเพื่อให้กึ่งกลางในพื้นที่ที่เหลือ (รวม padding แล้ว)
                    Dim drawX As Integer = CInt((finalWidth - newWidth) / 2)
                    Dim drawY As Integer = CInt((finalHeight - newHeight) / 2)

                    ' วาดลายเซ็นที่ถูกปรับขนาดลงบนภาพสุดท้าย
                    g.DrawImage(newBmp, New Rectangle(drawX, drawY, newWidth, newHeight), sourceRect, GraphicsUnit.Pixel)
                End Using

                ' 4) เซฟเป็น PNG
                finalBmp.Save(outputPath, ImageFormat.Png)


            End Using
        End Using

        Console.WriteLine("✔ แปลงเรียบร้อย: " & outputPath)
    End Sub

    Public Sub CropJPG(user_id As Integer)
        Dim inputPath As String = Server.MapPath("SignatureFiles\" & user_id & ".jpg")
        Dim outputPath As String = Server.MapPath("SignatureFiles\j\" & user_id & ".jpg")

        'Dim inputPath As String = "\\10.33.1.104\vswebsite$\Website\LeaveOnline\SignatureFiles\" & user_id & ".jpg"
        'Dim outputPath As String = "\\10.33.1.104\vswebsite$\Website\LeaveOnline\SignatureFiles\j\" & user_id & ".jpg"

        ' 🔹 ถ้าไม่มี JPG ไม่ต้องทำ
        If Not File.Exists(inputPath) Then
            Console.WriteLine("✔ ไม่มีไฟล์ JPG : " & inputPath)
            Return
        End If

        ' 🔹 ถ้ามี JPG อยู่แล้ว ไม่ต้องทำซ้ำ
        'If File.Exists(outputPath) Then
        '    Console.WriteLine("✔ ไฟล์ JPG มีอยู่แล้ว: " & outputPath)
        '    Return
        'End If

        ' โหลดภาพต้นฉบับ
        Using bmp As New Bitmap(inputPath)
            ' 1) หา bounding box (พื้นที่ที่ไม่ใช่สีขาว)
            Dim minX As Integer = bmp.Width
            Dim minY As Integer = bmp.Height
            Dim maxX As Integer = -1
            Dim maxY As Integer = -1
            Dim whiteThreshold As Integer = 254

            For y As Integer = 0 To bmp.Height - 1
                For x As Integer = 0 To bmp.Width - 1
                    Dim pixel As Color = bmp.GetPixel(x, y)
                    If Not (pixel.R >= whiteThreshold AndAlso pixel.G >= whiteThreshold AndAlso pixel.B >= whiteThreshold) Then
                        If x < minX Then minX = x
                        If y < minY Then minY = y
                        If x > maxX Then maxX = x
                        If y > maxY Then maxY = y
                    End If
                Next
            Next

            ' 2) ถ้าไม่พบลายเซ็น ให้บันทึกภาพเปล่าแล้วจบการทำงาน
            If maxX < 0 OrElse maxY < 0 Then
                Console.WriteLine("❌ ไม่พบลายเซ็นในภาพ หรือภาพเป็นสีขาวทั้งหมด")
                Return
            End If

            ' 3) สร้างภาพใหม่ที่ถูกครอปอย่างแม่นยำ
            Dim croppedWidth As Integer = maxX - minX + 1
            Dim croppedHeight As Integer = maxY - minY + 1
            Dim sourceRect As New Rectangle(minX, minY, croppedWidth, croppedHeight)

            Using croppedBmp As New Bitmap(croppedWidth, croppedHeight)
                Using gCrop As Graphics = Graphics.FromImage(croppedBmp)
                    gCrop.DrawImage(bmp, New Rectangle(0, 0, croppedWidth, croppedHeight), sourceRect, GraphicsUnit.Pixel)
                End Using

                ' 4) สร้างภาพสุดท้ายและวางลายเซ็น
                Dim finalWidth As Integer = 658
                Dim finalHeight As Integer = 318
                Dim padLeft As Integer = 80
                Dim padRight As Integer = 80
                Dim padBottom As Integer = 60 ' กำหนด padding ด้านล่างที่คุณต้องการ

                Using finalBmp As New Bitmap(finalWidth, finalHeight, PixelFormat.Format24bppRgb)
                    Using gFinal As Graphics = Graphics.FromImage(finalBmp)
                        gFinal.Clear(Color.White)
                        Dim newWidth As Integer
                        Dim newHeight As Integer

                        ' กำหนดขนาดสูงสุดของลายเซ็นที่ต้องการ (เช่น 200x200 พิกเซล)
                        Dim maxSignatureDimension As Integer = 250

                        ' คำนวณอัตราส่วนการปรับขนาด
                        Dim ratioWidth As Double = CDbl(maxSignatureDimension) / croppedBmp.Width
                        Dim ratioHeight As Double = CDbl(maxSignatureDimension) / croppedBmp.Height
                        Dim ratio As Double = Math.Min(ratioWidth, ratioHeight)

                        If croppedBmp.Width > 0 AndAlso croppedBmp.Height > 0 Then
                            newWidth = CInt(croppedBmp.Width * ratio)
                            newHeight = CInt(croppedBmp.Height * ratio)

                            Dim maxContentWidth As Integer = finalWidth - (padLeft + padRight)

                            If newWidth > maxContentWidth Then
                                Dim contentRatio As Double = CDbl(maxContentWidth) / croppedBmp.Width
                                newWidth = CInt(croppedBmp.Width * contentRatio)
                                newHeight = CInt(croppedBmp.Height * contentRatio)
                            End If

                            If newWidth < 1 Then newWidth = 1
                            If newHeight < 1 Then newHeight = 1
                        Else
                            newWidth = 1
                            newHeight = 1
                        End If

                        ' 🔹 ส่วนที่แก้ไข: การคำนวณตำแหน่งวาด
                        ' คำนวณตำแหน่งแนวนอน (กึ่งกลาง)
                        Dim drawX As Integer = CInt((finalWidth - newWidth) / 2)
                        ' คำนวณตำแหน่งแนวตั้ง (ชิดขอบล่างตามค่า padding ที่กำหนด)
                        'Dim drawY As Integer = finalHeight - newHeight - padBottom

                        Dim drawY As Integer = CInt((finalHeight - newHeight) / 2)

                        gFinal.DrawImage(croppedBmp, New Rectangle(drawX, drawY, newWidth, newHeight), New Rectangle(0, 0, croppedBmp.Width, croppedBmp.Height), GraphicsUnit.Pixel)
                    End Using

                    ' 5) Save the final JPG
                    finalBmp.Save(outputPath, ImageFormat.Jpeg)
                End Using
            End Using
        End Using

        Console.WriteLine("✔ แปลงเรียบร้อย: " & outputPath)
    End Sub
End Class