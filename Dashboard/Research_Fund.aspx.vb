Imports System.Web.Script.Serialization
Imports System.Data.SqlClient
Imports Dashboard.ConnectDB
Imports Dashboard.Encrypt
Imports System.Reflection.Emit
Public Class Research_Fund
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim MinibleBody As HtmlGenericControl = CType((Me.Master).FindControl("MinibleBody"), HtmlGenericControl)
        MinibleBody.Attributes.Remove("data-layout")
        MinibleBody.Attributes.Remove("data-layout-size")

        If Not IsPostBack Then
            LoadYearDropdown()
            LoadTotalCount()
            pie_chart_per()
            chart_Fund()
            BindData()
        End If
    End Sub
    Private Sub LoadYearDropdown()
        Dim SQLRN As String
        Dim dt As DataTable

        SQLRN = "SELECT DISTINCT TOP (100) PERCENT year
FROM            dbo.Research_Fund
ORDER BY year DESC"
        dt = QueryDataTable2(SQLRN, dbConn, "Dashboard", Nothing)

        ' เคลียร์ dropdown ก่อนเพิ่มใหม่
        yearDropdown.InnerHtml = ""
        ' เพิ่มตัวเลือก "ทั้งหมด"
        yearDropdown.InnerHtml &= "<a class='dropdown-item' href='?year=all'>ทั้งหมด</a>"

        ' เพิ่มปีจากฐานข้อมูล
        For Each row As DataRow In dt.Rows
            Dim y As String = row("year").ToString()
            yearDropdown.InnerHtml &= $"<a class='dropdown-item' href='?year={y}'>{y}</a>"
        Next

        '' ตรวจสอบว่ามีการเลือกปีไหม (ผ่าน querystring)
        'Dim selectedYear As String = Request.QueryString("year")
        'If String.IsNullOrEmpty(selectedYear) OrElse selectedYear = "all" Then
        '    lblSelectedYear.InnerText = "ทั้งหมด"
        'Else
        '    lblSelectedYear.InnerText = selectedYear
        'End If
        Dim selectedYear As String = Request.QueryString("year")
        If String.IsNullOrEmpty(selectedYear) Then
            ' ❗ ไม่มี querystring → ใช้ปีล่าสุดจาก SQL
            lblSelectedYear.InnerText = dt.Rows(0)("year").ToString()

        ElseIf selectedYear = "all" Then
            lblSelectedYear.InnerText = "ทั้งหมด"
        Else
            lblSelectedYear.InnerText = (CInt(selectedYear)).ToString()
        End If
    End Sub
    Private Sub LoadTotalCount()
        Dim selectedYear As String = Request.QueryString("year")

        If String.IsNullOrEmpty(selectedYear) Then
            Dim dtYear As DataTable
            Dim sqlYear As String = "
            SELECT DISTINCT TOP (1) year FROM Research_Fund ORDER BY year DESC"
            dtYear = QueryDataTable2(sqlYear, dbConn, "Dashboard", Nothing)

            If dtYear.Rows.Count > 0 Then
                selectedYear = dtYear.Rows(0)("year").ToString()
            End If
        End If
        'Dim yearParam As Object = If(String.IsNullOrEmpty(selectedYear), DBNull.Value, selectedYear)
        Dim SQLRN As String

        SQLRN = "SELECT        SUM(CASE WHEN type = 1 THEN amount ELSE 0 END) AS amount1, COUNT(CASE WHEN type = 1 THEN 1 END) AS Project1, SUM(CASE WHEN type = 2 THEN amount ELSE 0 END) AS amount2, 
                         COUNT(CASE WHEN type = 2 THEN 1 END) AS Project2
FROM            dbo.Research_Fund
"

        'If Not String.IsNullOrEmpty(selectedYear) AndAlso selectedYear <> "all" Then
        If selectedYear <> "all" Then
            SQLRN &= $"WHERE        (year = " & selectedYear & ")"
        End If

        Dim dt As DataTable = QueryDataTable2(SQLRN, dbConn, "Dashboard", Nothing)

        If dt.Rows.Count > 0 Then
            Dim amount01 = If(IsDBNull(dt.Rows(0)("amount1")), 0, dt.Rows(0)("amount1"))
            Dim project01 = If(IsDBNull(dt.Rows(0)("Project1")), 0, dt.Rows(0)("Project1"))

            Dim amount02 = If(IsDBNull(dt.Rows(0)("amount2")), 0, dt.Rows(0)("amount2"))
            Dim project02 = If(IsDBNull(dt.Rows(0)("Project2")), 0, dt.Rows(0)("Project2"))

            amount1.InnerHtml = $"<b class=""text-dark"">{Convert.ToDecimal(amount01).ToString("N0")}</b>"
            Project1.InnerHtml = $"<b class=""text-dark"">{project01}</b>"

            amount2.InnerHtml = $"<b class=""text-dark"">{Convert.ToDecimal(amount02).ToString("N0")}</b>"
            Project2.InnerHtml = $"<b class=""text-dark"">{project02}</b>"

        End If
    End Sub

    Protected Sub Filter_Click(sender As Object, e As EventArgs)
        Dim btn As LinkButton = CType(sender, LinkButton)
        Dim selectedVal As String = btn.CommandArgument

        ' เก็บค่าที่เลือกลง ViewState (ตัวช่วยจำค่าของ ASP.NET)
        ViewState("SelectedType") = selectedVal

        ' ปรับสีปุ่ม ให้รู้ว่าเลือกอันไหนอยู่ (UX)
        UpdateButtonStyles(selectedVal)

        ' โหลดข้อมูลใหม่
        BindData()
    End Sub
    Private Sub UpdateButtonStyles(val As String)
        ' รีเซ็ตปุ่มทั้งหมดเป็นแบบโปร่ง (outline)
        btnAll.CssClass = "btn btn-outline-primary"
        btnAll.Text = "<i class='mdi mdi-circle-outline me-1'></i>ทั้งหมด"

        btnType1.CssClass = "btn btn-outline-primary"
        btnType1.Text = "<i class='mdi mdi-circle-outline me-1'></i>ทุนวิจัย"

        btnType2.CssClass = "btn btn-outline-primary"
        btnType2.Text = "<i class='mdi mdi-circle-outline me-1'></i>บริการวิชาการ"

        ' เช็คว่าอันไหนถูกเลือก ให้เปลี่ยนเป็นสีทึบ และเปลี่ยนไอคอน
        Select Case val
            Case "1"
                btnType1.CssClass = "btn btn-soft-primary"
                btnType1.Text = "<i class='mdi mdi-check-circle-outline me-1'></i>ทุนวิจัย"
            Case "2"
                btnType2.CssClass = "btn btn-soft-primary"
                btnType2.Text = "<i class='mdi mdi-check-circle-outline me-1'></i>บริการวิชาการ"
            Case Else ' ทั้งหมด
                btnAll.CssClass = "btn btn-soft-primary"
                btnAll.Text = "<i class='mdi mdi-check-circle-outline me-1'></i>ทั้งหมด"
        End Select
    End Sub
    Private Sub BindData()
        Dim selectedType As String = If(ViewState("SelectedType") Is Nothing, "", ViewState("SelectedType").ToString())
        Dim SQLRN As String = "SELECT TOP (100) PERCENT dbo.FundType.Fund_source, " &
                        "CASE WHEN itjobs.dbo.title_technical.title_technicalName IS NULL " &
                        "THEN itjobs.dbo.title.title_name + itjobs.dbo.[user].fname + SPACE(2) + itjobs.dbo.[user].lname " &
                        "ELSE itjobs.dbo.title_technical.title_technicalName + itjobs.dbo.[user].fname + SPACE(2) + itjobs.dbo.[user].lname " &
                        "END AS fullname, " &
                        "dbo.Research_Fund.year, dbo.Research_Fund.title, " &
                        "CASE WHEN dbo.Research_Fund.type = 1 THEN 'ทุนวิจัย' " &
              "     WHEN dbo.Research_Fund.type = 2 THEN 'บริการวิชาการ' " &
              "     ELSE '-' END AS type_name, " &
        "dbo.Research_Fund.stDate, " &
                        "dbo.Research_Fund.fnDate, dbo.Research_Fund.GrantExtDate, dbo.Research_Fund.amount " &
                        "FROM dbo.Research_Fund " &
                        "INNER JOIN itjobs.dbo.[user] On dbo.Research_Fund.user_id = itjobs.dbo.[user].user_id " &
                        "INNER JOIN dbo.FundType On dbo.Research_Fund.Fund_ID = dbo.FundType.Fund_ID " &
                        "INNER JOIN itjobs.dbo.title_technical On itjobs.dbo.[user].title_technicalID = itjobs.dbo.title_technical.title_technicalID " &
                        "INNER JOIN itjobs.dbo.title On itjobs.dbo.[user].title_id = itjobs.dbo.title.title_id " &
                        "WHERE 1=1"

        If Not String.IsNullOrEmpty(selectedType) Then
            SQLRN &= " AND dbo.Research_Fund.type = " & selectedType & " "
        End If

        Dim selectedYear As String = Request.QueryString("year")
        If String.IsNullOrEmpty(selectedYear) Then
            Dim dtYear As DataTable
            Dim sqlYear As String = "
            SELECT DISTINCT TOP (1) year FROM Research_Fund ORDER BY year DESC"
            dtYear = QueryDataTable2(sqlYear, dbConn, "Dashboard", Nothing)

            If dtYear.Rows.Count > 0 Then
                selectedYear = dtYear.Rows(0)("year").ToString()
            End If
        End If
        If selectedYear <> "all" Then
            SQLRN &= " AND dbo.Research_Fund.year = " & selectedYear & " "
        End If

        SQLRN &= " ORDER BY dbo.Research_Fund.fnDate DESC"

        Dim dt As DataTable = QueryDataTable2(SQLRN, dbConn, "Dashboard", Nothing)
        If dt.Rows.Count > 0 Then
            data.DataSource = dt
            data.DataBind()

            LoadYearDropdown()
            LoadTotalCount()
            pie_chart_per()
            chart_Fund()
        End If
    End Sub
    Protected Function DisplayDuration(ByVal stDate As Object, ByVal fnDate As Object, ByVal extDate As Object) As String
        ' 1. แปลง Object เป็น DateTime (รองรับค่า NULL)
        Dim startD As DateTime? = Nothing
        Dim endD As DateTime? = Nothing
        Dim extD As DateTime? = Nothing

        If Not IsDBNull(stDate) Then startD = Convert.ToDateTime(stDate)
        If Not IsDBNull(fnDate) Then endD = Convert.ToDateTime(fnDate)
        If Not IsDBNull(extDate) Then extD = Convert.ToDateTime(extDate)

        ' กำหนดรูปแบบวันที่ (เช่น d/M/yyyy)
        Dim format As String = "dd/MM/yyyy"

        ' 2. ตรวจสอบเงื่อนไข 3 ข้อ

        ' กรณีที่ 1: มีทั้ง 3 ค่า (วันเริ่ม, วันจบ, วันขยาย)
        If startD.HasValue AndAlso endD.HasValue AndAlso extD.HasValue Then
            Return String.Format("{0} - {1} (ขยายระยะเวลาถึงวันที่ {2})",
                             startD.Value.ToString(format),
                             endD.Value.ToString(format),
                             extD.Value.ToString(format))

            ' กรณีที่ 2: ไม่มี GrantExtDate (แต่มีวันเริ่ม-จบ)
        ElseIf startD.HasValue AndAlso endD.HasValue AndAlso (Not extD.HasValue) Then
            Return String.Format("{0} - {1}",
                             startD.Value.ToString(format),
                             endD.Value.ToString(format))

            ' กรณีที่ 3: ไม่มี stDate-fnDate (แต่มีวันขยาย)
        ElseIf (Not startD.HasValue) AndAlso extD.HasValue Then
            Return String.Format("ขยายระยะเวลาถึงวันที่ {0}",
                             extD.Value.ToString(format))

            ' กรณีแถม: ถ้าไม่มีข้อมูลเลย
        Else
            Return "-"
        End If
    End Function
    ' ฟังก์ชันตรวจสอบสถานะโครงการเทียบกับวันที่ปัจจุบัน
    Protected Function GetProjectStatus(ByVal fnDateObj As Object, ByVal extDateObj As Object) As String
        Dim fnDate As DateTime? = Nothing
        Dim extDate As DateTime? = Nothing
        Dim today As DateTime = DateTime.Now.Date ' วันที่ปัจจุบัน (ตัดเวลาออกเพื่อเทียบแค่วัน)

        ' 1. แปลงค่าจาก Database (รองรับค่า NULL)
        If Not IsDBNull(fnDateObj) Then fnDate = Convert.ToDateTime(fnDateObj)
        If Not IsDBNull(extDateObj) Then extDate = Convert.ToDateTime(extDateObj)

        ' --- ตรวจสอบเงื่อนไขตามลำดับ ---

        ' กรณี 1: มีการขยายเวลา (GrantExtDate) และ ยังไม่หมดอายุ (>= วันนี้)
        ' แสดง: ขยายเวลา (สีเหลือง, ลูกศรขึ้น)
        If extDate.HasValue AndAlso extDate.Value >= today Then
            Return "<div class='mb-1 badge bg-soft-warning '>ขยายเวลา</div>"

            ' กรณี 2: ไม่มีวันขยาย (หรือวันขยายหมดไปแล้ว) แต่ยังอยู่ในสัญญาปกติ (fnDate >= วันนี้)
            ' แสดง: อยู่ในสัญญา (สีเขียว, ลูกศรขึ้น)
        ElseIf fnDate.HasValue AndAlso fnDate.Value >= today Then
            Return "<div class='mb-1 badge bg-soft-success'>อยู่ในสัญญา</div>"

            ' กรณี 3: เลยกำหนดทุกอย่างแล้ว (GrantExtDate หรือ fnDate ผ่านไปแล้ว)
            ' แสดง: หมดสัญญา (สีแดง/ส้ม, ลูกศรลง)
        Else
            ' ผมใส่สี text-danger (แดง) ให้เพื่อความชัดเจนว่าหมดสัญญา แต่ถ้าต้องการ text-warning ตามโจทย์ก็แก้ได้ครับ
            Return "<div class='mb-1 badge bg-danger'>หมดสัญญา</div>"
        End If

    End Function

    '        SQLRN = "Select        SUM(Case When type = 1 And GrantExtDate Is NULL And GETDATE() <= fnDate Then 1 Else 0 End) As Active1, SUM(Case When type = 1 And GrantExtDate Is Not NULL And GETDATE() 
    '                         <= GrantExtDate THEN 1 ELSE 0 END) AS Extended1, SUM(CASE WHEN type = 1 AND ((GrantExtDate IS NULL AND GETDATE() > fnDate) OR
    '                         (GrantExtDate IS NOT NULL AND GETDATE() > GrantExtDate)) THEN 1 ELSE 0 END) AS Ended1, SUM(CASE WHEN type = 2 AND GrantExtDate IS NULL AND GETDATE() <= fnDate THEN 1 ELSE 0 END) AS Active2, 
    '                         SUM(CASE WHEN type = 2 AND GrantExtDate IS NOT NULL AND GETDATE() <= GrantExtDate THEN 1 ELSE 0 END) AS Extended2, SUM(CASE WHEN type = 2 AND ((GrantExtDate IS NULL AND GETDATE() > fnDate) OR
    '                         (GrantExtDate IS NOT NULL AND GETDATE() > GrantExtDate)) THEN 1 ELSE 0 END) AS Ended2
    'FROM            dbo.Research_Fund
    'WHERE        (1 = 1)"
    '        If Not String.IsNullOrEmpty(selectedYear) AndAlso selectedYear <> "all" Then
    '            SQLRN &= $"WHERE        (year = " & selectedYear & ")"
    '        End If
    '        dt = QueryDataTable2(SQLRN, dbConn, "Dashboard", Nothing)
    '        If dt.Rows.Count > 0 Then
    '            ' 1. ทุนวิจัย (Type 1)
    '            Dim valActive1 = If(IsDBNull(dt.Rows(0)("Active1")), 0, dt.Rows(0)("Active1"))
    '            Dim valExtended1 = If(IsDBNull(dt.Rows(0)("Extended1")), 0, dt.Rows(0)("Extended1"))
    '            Dim valEnded1 = If(IsDBNull(dt.Rows(0)("Ended1")), 0, dt.Rows(0)("Ended1"))

    '            ' Active1: สีเขียว Icon up
    '            Active1.InnerHtml = $"<i class=""icon-xs icon me-2 text-success"" data-feather=""trending-up""></i>{valActive1}"

    '            ' Extended1: สีแดง Icon down (ตาม HTML ต้นฉบับ)
    '            Extended1.InnerHtml = $"<i class=""icon-xs icon me-2 text-warning"" data-feather=""trending-down""></i>{valExtended1}"

    '            ' Ended1: สีเขียว Icon up (ตาม HTML ต้นฉบับ)
    '            Ended1.InnerHtml = $"<i class=""icon-xs icon me-2 text-danger"" data-feather=""trending-up""></i>{valEnded1}"


    '            ' 2. บริการวิชาการ (Type 2)
    '            Dim valActive2 = If(IsDBNull(dt.Rows(0)("Active2")), 0, dt.Rows(0)("Active2"))
    '            Dim valExtended2 = If(IsDBNull(dt.Rows(0)("Extended2")), 0, dt.Rows(0)("Extended2"))
    '            Dim valEnded2 = If(IsDBNull(dt.Rows(0)("Ended2")), 0, dt.Rows(0)("Ended2"))

    '            ' Active2
    '            Active2.InnerHtml = $"<i class=""icon-xs icon me-2 text-success"" data-feather=""trending-up""></i>{valActive2} "

    '            ' Extended2
    '            Extended2.InnerHtml = $"<i class=""icon-xs icon me-2 text-warning"" data-feather=""trending-down""></i>{valExtended2}"

    '            ' Ended2
    '            Ended2.InnerHtml = $"<i class=""icon-xs icon me-2 text-danger"" data-feather=""trending-up""></i>{valEnded2}"
    '        End If

    Private Sub pie_chart_per()
        Dim selectedYear As String = Request.QueryString("year")
        If String.IsNullOrEmpty(selectedYear) Then
            Dim dtYear As DataTable
            Dim sqlYear As String = "
            SELECT DISTINCT TOP (1) year FROM Research_Fund ORDER BY year DESC"
            dtYear = QueryDataTable2(sqlYear, dbConn, "Dashboard", Nothing)

            If dtYear.Rows.Count > 0 Then
                selectedYear = dtYear.Rows(0)("year").ToString()
            End If
        End If
        Dim yearParam As Object = If(String.IsNullOrEmpty(selectedYear), DBNull.Value, selectedYear)
        Dim selectedType As String = If(ViewState("SelectedType") Is Nothing, "", ViewState("SelectedType").ToString())
        Dim SQLRN As String = "SELECT dbo.FundType.Fund_source, COUNT(dbo.Research_Fund.Fund_ID) AS Total_Count " &
                      "FROM dbo.FundType " &
                      "INNER JOIN dbo.Research_Fund ON dbo.FundType.Fund_ID = dbo.Research_Fund.Fund_ID " &
        "WHERE 1=1 "

        If Not String.IsNullOrEmpty(selectedType) Then
            SQLRN &= " AND dbo.Research_Fund.type = " & selectedType & " "
        End If

        If selectedYear <> "all" Then
            SQLRN &= " AND dbo.Research_Fund.year = " & selectedYear & " "
        End If

        SQLRN &= "GROUP BY dbo.FundType.Fund_ID, dbo.FundType.Fund_source "
        SQLRN &= "ORDER BY Total_Count DESC"

        Dim dt As DataTable = QueryDataTable2(SQLRN, dbConn, "Dashboard", Nothing)

        Dim listLabels As New List(Of String)
        Dim listSeries As New List(Of Integer)
        Dim listColors As New List(Of String)

        Dim colorPalette As String() = {
    "#b71c1c", "#c62828", "#d32f2f", "#e53935", "#f44336",
    "#ef5350", "#e57373", "#f06292", "#ec407a", "#d81b60",
    "#c2185b", "#ad1457", "#9c27b0", "#ab47bc", "#ba68c8",
    "#ce93d8", "#7b1fa2", "#6a1b9a", "#4a148c", "#283593",
    "#303f9f", "#3949ab", "#1e88e5", "#1976d2", "#1565c0",
    "#0d47a1", "#039be5", "#29b6f6", "#4fc3f7", "#81d4fa",
    "#00695c", "#00796b", "#00897b", "#26a69a", "#4db6ac",
    "#1b5e20", "#2e7d32", "#388e3c", "#43a047", "#66bb6a",
    "#8bc34a", "#9ccc65", "#aed581", "#c5e1a5", "#dcedc8",
    "#f9a825", "#fbc02d", "#fdd835", "#ffee58", "#fff176",
    "#ffeb3b", "#ffc107", "#ffb300", "#ffa000", "#ff8f00",
    "#ff6f00", "#ff9800", "#fb8c00", "#f57c00", "#ef6c00",
    "#e65100", "#ff7043", "#ff8a65", "#ffab91", "#d84315",
    "#8d6e63", "#a1887f", "#bcaaa4", "#d7ccc8", "#efebe9",
    "#616161", "#757575", "#9e9e9e", "#bdbdbd", "#e0e0e0",
    "#fafafa", "#263238", "#37474f", "#455a64", "#607d8b"
    }

        ' ตัวนับลำดับ เพื่อใช้หยิบสี
        Dim i As Integer = 0

        If dt.Rows.Count > 0 Then
            For Each row As DataRow In dt.Rows
                listLabels.Add(If(IsDBNull(row("Fund_source")), "-", row("Fund_source").ToString()))
                listSeries.Add(Convert.ToInt32(row("Total_Count")))

                ' 2. หยิบสีจาก Palette ตามลำดับ
                ' ใช้ Mod (Modulo) เพื่อป้องกัน error กรณีข้อมูลเกิน 30 แถว (มันจะวนกลับไปใช้สีแรกใหม่)
                Dim colorIndex As Integer = i Mod colorPalette.Length
                listColors.Add(colorPalette(colorIndex))

                ' บวกตัวนับเพิ่ม
                i += 1
            Next
        End If


        ' 4. แปลงข้อมูลเป็น JSON
        Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Dim jsonLabels As String = serializer.Serialize(listLabels)
        Dim jsonSeries As String = serializer.Serialize(listSeries)
        Dim jsonColors As String = serializer.Serialize(listColors) ' <--- (ใหม่) แปลงสีเป็น JSON

        ' 5. สร้าง Script กราฟ
        Dim script As String = "
    <script type='text/javascript'>
        $(document).ready(function () {
            var chartLabels = " & jsonLabels & ";
            var chartSeries = " & jsonSeries & ";
            var chartColors = " & jsonColors & "; // <--- (ใหม่) รับค่าสีจาก VB

            var chartOptions = {
                chart: {
                    height: 380,
                    type: 'pie'
                },
                series: chartSeries,
                labels: chartLabels,
                colors: chartColors, // <--- (ใหม่) เปลี่ยนจากค่า Hardcode เป็นตัวแปรนี้
                legend: {
                    show: false
                },
                tooltip: {
                    y: {
                        formatter: function (val) {
                            return val + ' โครงการ'; 
                        }
                    }
                },
                dataLabels: {
                    enabled: true,
                    formatter: function (val, opts) {
                        return opts.w.config.series[opts.seriesIndex];
                    }
                },
                responsive: [{
                    breakpoint: 600,
                    options: {
                        chart: { height: 240 },
                        legend: { show: false }
                    }
                }]
            };

            var chart = new ApexCharts(document.querySelector('#pie_chart_per'), chartOptions);
            chart.render();
// --- ส่วนที่ 2: สร้างตาราง (Table Generation) ---
        // ใช้ข้อมูลชุดเดียวกับกราฟ วนลูปสร้างแถวตาราง
        var tableBody = $('#fundTable tbody');
        tableBody.empty(); // เคลียร์ข้อมูลเก่า (ถ้ามี)

        var htmlRows = '';
        for (var i = 0; i < chartLabels.length; i++) {
            // สร้างกล่องสีสี่เหลี่ยม
            var colorBox = '<span style=""display:inline-block; width:20px; height:20px; background-color:' + chartColors[i] + '; border:1px solid #ccc;""></span>';
            
            htmlRows += '<tr>';
            htmlRows += '<td class=""text-center"">' + colorBox + '</td>'; // คอลัมน์สี
            htmlRows += '<td>' + chartLabels[i] + '</td>';                 // คอลัมน์ชื่อแหล่งทุน
            htmlRows += '<td class=""text-right"">' + chartSeries[i] + '</td>'; // คอลัมน์จำนวน
            htmlRows += '</tr>';
        }
        
        // เอา HTML ที่สร้างเสร็จไปใส่ในตารางทีเดียว
        tableBody.append(htmlRows);
    });
</script>
    "

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "PieChartScript", script, False)


    End Sub
    Private Sub chart_Fund()

        ' ---------- SQL ----------
        Dim sql As String = "
        SELECT year, type, COUNT(*) AS total_count
        FROM dbo.Research_Fund
        WHERE year IS NOT NULL
        GROUP BY year, type
        ORDER BY year
    "

        Dim dt As DataTable = QueryDataTable2(sql, dbConn, "Dashboard", Nothing)
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then Exit Sub

        ' ---------- ปี (X-axis) ----------
        Dim years As List(Of Integer) = dt.AsEnumerable() _
        .Select(Function(r) CInt(r("year"))) _
        .Distinct() _
        .OrderBy(Function(y) y) _
        .ToList()

        ' ---------- type = 1 (area) ----------
        Dim areaData As New List(Of Integer)
        For Each y In years
            Dim rows = dt.Select("year = " & y & " AND type = 1")
            areaData.Add(If(rows.Length > 0, CInt(rows(0)("total_count")), 0))
        Next

        ' ---------- type = 2 (line) ----------
        Dim lineData As New List(Of Integer)
        For Each y In years
            Dim rows = dt.Select("year = " & y & " AND type = 2")
            lineData.Add(If(rows.Length > 0, CInt(rows(0)("total_count")), 0))
        Next

        ' ---------- Series (เหมือนตัวอย่าง) ----------
        Dim series As New List(Of Object) From {
        New With {
            .name = "ทุนวิจัย",
            .type = "area",
            .data = areaData
        },
        New With {
            .name = "บริการวิชาการ",
            .type = "line",
            .data = lineData
        }
    }

        Dim js As New JavaScriptSerializer()
        Dim jsonSeries As String = js.Serialize(series)
        Dim jsonYears As String = js.Serialize(years)

        ' ---------- JavaScript (โครงเดียวกับที่คุณให้มา) ----------
        Dim script As String = "
<script>
document.addEventListener('DOMContentLoaded', function () {

    var options = {
        chart: {
            height: 350,
            type: 'line',
            stacked: false,
            toolbar: { show: false }
        },
        stroke: {
            width: [0, 3],
            curve: 'smooth'
        },
        colors: ['#94b3d1', '#f1b44c'],
        series: " & jsonSeries & ",
        fill: {
            opacity: [0.35, 1],
            gradient: {
                shade: 'light',
                type: 'vertical',
                opacityFrom: 0.85,
                opacityTo: 0.55,
                stops: [0, 100]
            }
        },
        labels: " & jsonYears & ",
        markers: {
            size: 0
        },
        xaxis: {
            title: { text: 'ปี พ.ศ.' }
        },
        yaxis: {
            title: { text: 'จำนวน' }
        },
        tooltip: {
            shared: true,
            intersect: false
        },
        grid: {
            borderColor: '#f1f1f1'
        }
    };

    var el = document.querySelector('#chart_Fund');
    if (el) {
        var chart = new ApexCharts(el, options);
        chart.render();
    }

});
</script>
"

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ChartFundMixed", script, False)

    End Sub
End Class