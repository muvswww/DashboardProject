Imports System.Web.Script.Serialization
Imports System.Data.SqlClient
Imports Dashboard.ConnectDB
Imports Dashboard.Encrypt
Imports System.Reflection.Emit
Public Class test1
    'Inherits System.Web.UI.Page
    Inherits BasePage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim MinibleBody As HtmlGenericControl = CType((Me.Master).FindControl("MinibleBody"), HtmlGenericControl)
        MinibleBody.Attributes.Remove("data-layout")
        MinibleBody.Attributes.Remove("data-layout-size")

        If Not IsPostBack Then
            Panel1.Visible = True
            Panel2.Visible = False

            Dim SQLRN As String = "SELECT DISTINCT FiscalYear
FROM            MasterStrategy
ORDER BY FiscalYear DESC"
            Dim dt As DataTable = QueryDataTable2(SQLRN, dbConn, "Dashboard", Nothing)


            ' ตรวจสอบข้อมูล
            If dt.Rows.Count > 0 Then
                Dim maxYear As Integer = 0
                ddlYear.Items.Clear()

                ddlYear.DataSource = dt
                ddlYear.DataValueField = "FiscalYear"
                ddlYear.DataTextField = "FiscalYear"
                ddlYear.DataBind()

                ' หา Value ที่มากที่สุด
                For Each row As DataRow In dt.Rows
                    Dim fiscalYear As Integer = CInt(row("FiscalYear"))
                    If fiscalYear > maxYear Then
                        maxYear = fiscalYear
                    End If
                Next

                ' ตั้งค่า SelectedItem เป็น Value ที่มากที่สุด
                If ddlYear.Items.FindByValue(maxYear.ToString()) IsNot Nothing Then
                    ddlYear.SelectedValue = maxYear.ToString()
                End If

                ' เรียก SearchData ด้วยค่าปัจจุบัน
                SearchData(ddlYear.SelectedValue)
            End If
        End If



        'column_chart1()
        'line_chart_datalabel()
        'mixed_chart1()
        'bar_chart1()

    End Sub
    Protected Sub ddlYear_SelectedIndexChanged(sender As Object, e As EventArgs)
        ' ตรวจสอบค่าที่ผู้ใช้เลือก
        Dim selectedYear As String = ddlYear.SelectedValue

        SearchData(selectedYear)
    End Sub
    Private Sub SearchData(selectedYear As String)
        Dim SQLRN As String = "SELECT        Strategy_id, FiscalYear, StrategyName
FROM            MasterStrategy
WHERE        (FiscalYear = '" & selectedYear & "')
ORDER BY Strategy_id"
        Dim dt As DataTable = QueryDataTable2(SQLRN, dbConn, "Dashboard", Nothing)
        If dt.Rows.Count > 0 Then
            RptStrategy.DataSource = dt
            RptStrategy.DataBind()

            FiscalYear.InnerText = "ผลการดำเนินงานตามตัวชี้วัดแผนยุทธศาสตร์มหาวิทยาลัย ประจำปีงบประมาณ พ.ศ." & dt.Rows(0)("FiscalYear").ToString()

        End If
    End Sub
    Protected Sub RptStrategy_OnItemDataBound(sender As Object, e As RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim strategyId As Integer = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "Strategy_id"))
            Dim divId As String = "pie_chart_per_" & strategyId
            pie_chart_per(strategyId, divId)
        End If

    End Sub
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        GridView1.PageIndex = e.NewPageIndex
    End Sub
    'Protected Sub RptStrategy_ItemCommand(source As Object, e As RepeaterCommandEventArgs) _
    'Handles RptStrategy.ItemCommand

    '    If e.CommandName = "ShowDetail" Then
    '        Panel1.Visible = False
    '        Panel2.Visible = True

    '        Dim strategyId As Integer = Convert.ToInt32(e.CommandArgument)

    '        Dim lblName As HtmlGenericControl = CType(e.Item.FindControl("lblStrategyName"), HtmlGenericControl)
    '        Dim strategyName As String = lblName.InnerText


    '        lblStrategyTitle.InnerText = strategyName
    '        lblStrategySubTitle.InnerText = "รายละเอียดความก้าวหน้า"
    '        Dim selectedYear As String = ddlYear.SelectedValue
    '        ' query data สำหรับ chart
    '        '            Dim SQLRN As String = "
    '        '            SELECT
    '        '    Project_id,
    '        '    ProjectName,
    '        '    Target,
    '        '    CAST(
    '        '        CASE 
    '        '            WHEN Quarter4 IS NOT NULL THEN Quarter4
    '        '            WHEN Quarter3 IS NOT NULL THEN Quarter3
    '        '            WHEN Quarter2 IS NOT NULL THEN Quarter2
    '        '            WHEN Quarter1 IS NOT NULL THEN Quarter1
    '        '            ELSE '0' 
    '        '        END AS FLOAT
    '        '    ) AS LastQuarterValue,
    '        '    ROUND(
    '        '        CASE 
    '        '            WHEN Target IS NOT NULL AND ISNUMERIC(Target) = 1 AND CAST(Target AS FLOAT) <> 0 THEN 
    '        '                CASE 
    '        '                    WHEN (CAST(
    '        '                            CASE 
    '        '                                WHEN Quarter4 IS NOT NULL THEN Quarter4
    '        '                                WHEN Quarter3 IS NOT NULL THEN Quarter3
    '        '                                WHEN Quarter2 IS NOT NULL THEN Quarter2
    '        '                                WHEN Quarter1 IS NOT NULL THEN Quarter1
    '        '                                ELSE '0' 
    '        '                            END AS FLOAT
    '        '                        ) / CAST(Target AS FLOAT) * 100) > 100 THEN 100
    '        '                    ELSE (CAST(
    '        '                            CASE 
    '        '                                WHEN Quarter4 IS NOT NULL THEN Quarter4
    '        '                                WHEN Quarter3 IS NOT NULL THEN Quarter3
    '        '                                WHEN Quarter2 IS NOT NULL THEN Quarter2
    '        '                                WHEN Quarter1 IS NOT NULL THEN Quarter1
    '        '                                ELSE '0' 
    '        '                            END AS FLOAT
    '        '                        ) / CAST(Target AS FLOAT) * 100)
    '        '                END
    '        '            ELSE 0
    '        '        END
    '        '    ,2) AS PercentProgress
    '        'FROM MasterProject
    '        'WHERE (Target IS NOT NULL) 
    '        '  AND (Strategy_id = @Strategy_id) 
    '        'AND (Strategy_Year = @Strategy_Year)
    '        '  AND (ISNUMERIC(Target) = 1)"
    '         Dim SQLRN As String = "
    '        WITH ProjectData AS (SELECT        Strategy_id, no, SUM(CAST(Target AS FLOAT)) AS TotalTarget, SUM(CAST(CASE WHEN Quarter4 IS NOT NULL THEN Quarter4 WHEN Quarter3 IS NOT NULL THEN Quarter3 WHEN Quarter2 IS NOT NULL 
    '                                                                     THEN Quarter2 WHEN Quarter1 IS NOT NULL THEN Quarter1 ELSE '0' END AS FLOAT)) AS TotalProgress
    '                                            FROM            dbo.MasterProject
    '                                            WHERE        (Strategy_id = @Strategy_id) AND (Strategy_Year = @Strategy_Year) AND (Target IS NOT NULL) AND (ISNUMERIC(Target) = 1)
    '                                            GROUP BY Strategy_id, no)
    'SELECT        TOP (100) PERCENT pd.no, pd.Strategy_id, 1 AS title, mp.Project_id, ISNULL(mp.Project_no, '') + ' ' + ISNULL(mp.ProjectName, '') AS ProjectName, pd.TotalTarget, pd.TotalProgress, 
    '                          CASE WHEN pd.TotalTarget = 0 THEN 0 WHEN (pd.TotalProgress / pd.TotalTarget) * 100 > 100 THEN 100 ELSE (pd.TotalProgress / pd.TotalTarget) * 100 END AS PercentProgress
    ' FROM            ProjectData AS pd INNER JOIN
    '                          dbo.MasterProject AS mp ON pd.Strategy_id = mp.Strategy_id AND pd.no = mp.no
    ' WHERE        (mp.title = 1) AND (mp.Strategy_id = @Strategy_id) AND (mp.Strategy_Year = @Strategy_Year)
    ' ORDER BY pd.no"


    '        Dim parameters As SqlParameter() = {
    '            New SqlParameter("@Strategy_id", strategyId),
    '            New SqlParameter("@Strategy_Year", selectedYear)}
    '        Dim dt As DataTable = QueryDataTable2(SQLRN, dbConn, "Dashboard", parameters)
    '        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
    '            ' เรียกกราฟ
    '            Dim chartType As String = ddlChartType.SelectedValue
    '            If String.IsNullOrEmpty(chartType) OrElse chartType = "bar" Then
    '                bar_chart1(dt, strategyId)
    '            ElseIf chartType = "column" Then
    '                column_chart1(dt, strategyId)
    '            End If
    '        Else
    '            ' แสดงข้อความว่าไม่มีข้อมูล
    '            lblMessage.Text = "ไม่พบข้อมูลสำหรับ Strategy นี้"
    '        End If
    '        '' ===== เก็บค่าไว้ใช้ตอนเปลี่ยน dropdown =====
    '        'CurrentStrategyId = strategyId
    '        'CurrentData = dt

    '        '' ===== ค่า default ให้แสดง Bar Chart ตอนแรก =====
    '        'bar_chart1(dt, strategyId)
    '        'ddlChartType.SelectedValue = "bar"


    '        'column_chart1(dt, strategyId)
    '        'bar_chart1(dt, strategyId)

    '    End If
    'End Sub

    Protected Sub RptStrategy_ItemCommand(source As Object, e As RepeaterCommandEventArgs) _
    Handles RptStrategy.ItemCommand

        If e.CommandName = "ShowDetail" Then
            Panel1.Visible = False
            Panel2.Visible = True

            Dim strategyId As Integer = Convert.ToInt32(e.CommandArgument)
            Dim lblName As HtmlGenericControl = CType(e.Item.FindControl("lblStrategyName"), HtmlGenericControl)
            Dim strategyName As String = lblName.InnerText

            lblStrategyTitle.InnerText = strategyName
            'lblStrategySubTitle.InnerText = "รายละเอียดความก้าวหน้า"

            Dim selectedYear As String = ddlYear.SelectedValue

            Dim SQLRN As String = "
            WITH ProjectData AS (SELECT        Strategy_id, no, SUM(CAST(Target AS FLOAT)) AS TotalTarget, SUM(CAST(CASE WHEN Quarter4 IS NOT NULL THEN Quarter4 WHEN Quarter3 IS NOT NULL THEN Quarter3 WHEN Quarter2 IS NOT NULL 
                                                                         THEN Quarter2 WHEN Quarter1 IS NOT NULL THEN Quarter1 ELSE '0' END AS FLOAT)) AS TotalProgress
                                                FROM            dbo.MasterProject
                                                WHERE        (Strategy_id = @Strategy_id) AND (Strategy_Year = @Strategy_Year) AND (Target IS NOT NULL) AND (ISNUMERIC(Target) = 1)
                                                GROUP BY Strategy_id, no)
    SELECT        TOP (100) PERCENT pd.no, pd.Strategy_id, 1 AS title, mp.Project_id, ISNULL(mp.Project_no, '') + ' ' + ISNULL(mp.ProjectName, '') AS ProjectName, pd.TotalTarget, pd.TotalProgress, 
                              CASE WHEN pd.TotalTarget = 0 THEN 0 WHEN (pd.TotalProgress / pd.TotalTarget) * 100 > 100 THEN 100 ELSE (pd.TotalProgress / pd.TotalTarget) * 100 END AS PercentProgress
     FROM            ProjectData AS pd INNER JOIN
                              dbo.MasterProject AS mp ON pd.Strategy_id = mp.Strategy_id AND pd.no = mp.no
     WHERE        (mp.title = 1) AND (mp.Strategy_id = @Strategy_id) AND (mp.Strategy_Year = @Strategy_Year)
     ORDER BY pd.no"


            Dim parameters As SqlParameter() = {
                New SqlParameter("@Strategy_id", strategyId),
                New SqlParameter("@Strategy_Year", selectedYear)}
            Dim dt As DataTable = QueryDataTable2(SQLRN, dbConn, "Dashboard", parameters)

            ' ===== เก็บค่าไว้ใช้ตอนเปลี่ยน DropDown =====
            CurrentStrategyId = strategyId
            CurrentData = dt

            Dim chartType As String = ddlChartType.SelectedValue
            If String.IsNullOrEmpty(chartType) OrElse chartType = "bar" Then
                bar_chart1(dt, strategyId)
            Else
                column_chart1(dt, strategyId)
            End If

            SQLRN = "SELECT   Strategy_Year, Strategy_id, no, title, Project_no, Project_id, ProjectName, Unit, typeYear, Year, Responsible, division_id, Target, FORMAT(
           CASE 
               WHEN ISNUMERIC(Quarter4) = 1 THEN CAST(Quarter4 AS INT)
               WHEN ISNUMERIC(Quarter3) = 1 THEN CAST(Quarter3 AS INT)
               WHEN ISNUMERIC(Quarter2) = 1 THEN CAST(Quarter2 AS INT)
               WHEN ISNUMERIC(Quarter1) = 1 THEN CAST(Quarter1 AS INT)
               ELSE 0 
           END, 
           'N0'
       ) AS LastQuarter
FROM            dbo.MasterProject
WHERE        (Strategy_Year = @Strategy_Year) AND (Target IS NULL) AND (Strategy_id = @Strategy_id)"
            parameters = {
                New SqlParameter("@Strategy_id", strategyId),
                New SqlParameter("@Strategy_Year", selectedYear)}
            dt = QueryDataTable2(SQLRN, dbConn, "Dashboard", parameters)
            If dt.Rows.Count > 0 Then
                GridView1.DataSource = dt
                GridView1.DataBind()


            End If


        End If
    End Sub

    Protected Sub ddlChartType_SelectedIndexChanged(sender As Object, e As EventArgs)
        If CurrentData IsNot Nothing AndAlso CurrentStrategyId > 0 Then
            Dim chartType As String = ddlChartType.SelectedValue
            If String.IsNullOrEmpty(chartType) OrElse chartType = "bar" Then
                bar_chart1(CurrentData, CurrentStrategyId)
            ElseIf chartType = "column" Then
                column_chart1(CurrentData, CurrentStrategyId)
            End If
        End If
    End Sub
    ' Page level properties
    Private Property CurrentData As DataTable
        Get
            Return TryCast(ViewState("CurrentData"), DataTable)
        End Get
        Set(value As DataTable)
            ViewState("CurrentData") = value
        End Set
    End Property

    Private Property CurrentStrategyId As Integer
        Get
            Dim v = ViewState("CurrentStrategyId")
            If v IsNot Nothing Then
                Return Convert.ToInt32(v)
            End If
            Return 0
        End Get
        Set(value As Integer)
            ViewState("CurrentStrategyId") = value
        End Set
    End Property

    Private Sub pie_chart_per(strategyId As Integer, divId As String)
        Dim SQLRN As String = "WITH ProjectData AS (SELECT        Strategy_id, no, SUM(CAST(Target AS FLOAT)) AS TotalTarget, SUM(CAST(CASE WHEN Quarter4 IS NOT NULL THEN Quarter4 WHEN Quarter3 IS NOT NULL THEN Quarter3 WHEN Quarter2 IS NOT NULL 
                                                                         THEN Quarter2 WHEN Quarter1 IS NOT NULL THEN Quarter1 ELSE '0' END AS FLOAT)) AS TotalProgress
                                                FROM            dbo.MasterProject
                                                WHERE        (Strategy_id = @Strategy_id) AND (Strategy_Year = @Strategy_Year) AND (Target IS NOT NULL) AND (ISNUMERIC(Target) = 1)
                                                GROUP BY Strategy_id, no), ProjectWithPercent AS
    (SELECT        pd.no, pd.Strategy_id, CASE WHEN pd.TotalTarget = 0 THEN 0 WHEN (pd.TotalProgress / pd.TotalTarget) * 100 > 100 THEN 100 ELSE (pd.TotalProgress / pd.TotalTarget) * 100 END AS PercentProgress
      FROM            ProjectData AS pd INNER JOIN
                                dbo.MasterProject AS mp ON pd.Strategy_id = mp.Strategy_id AND pd.no = mp.no
      WHERE        (mp.title = 1) AND (mp.Strategy_id = @Strategy_id) AND (mp.Strategy_Year = @Strategy_Year))
    SELECT        AVG(PercentProgress) AS AvgPercentProgress
     FROM            ProjectWithPercent AS ProjectWithPercent_1"
        'หา % แต่ละข้อใหญ่ก่อน เช่น 2.1 2.2 2.3 2.19 แล้วเอาเปอร์เซ็นที่ได้ มาหาค่าเฉลี่ย 

        'Dim SQLRN As String = "SELECT 
        '    CASE 
        '        WHEN SUM(
        '            CAST(
        '                CASE 
        '                    WHEN Quarter4 IS NOT NULL THEN Quarter4 
        '                    WHEN Quarter3 IS NOT NULL THEN Quarter3 
        '                    WHEN Quarter2 IS NOT NULL THEN Quarter2 
        '                    WHEN Quarter1 IS NOT NULL THEN Quarter1 
        '                    ELSE '0' 
        '                END AS FLOAT
        '            )
        '        ) / NULLIF(SUM(CAST(Target AS FLOAT)), 0) * 100 > 100 
        '        THEN 100 
        '        ELSE SUM(
        '            CAST(
        '                CASE 
        '                    WHEN Quarter4 IS NOT NULL THEN Quarter4 
        '                    WHEN Quarter3 IS NOT NULL THEN Quarter3 
        '                    WHEN Quarter2 IS NOT NULL THEN Quarter2 
        '                    WHEN Quarter1 IS NOT NULL THEN Quarter1 
        '                    ELSE '0' 
        '                END AS FLOAT
        '            )
        '        ) / NULLIF(SUM(CAST(Target AS FLOAT)), 0) * 100 
        '    END AS TotalPercentProgress
        'FROM dbo.MasterProject
        'WHERE (Strategy_id = @Strategy_id) 
        '  AND (Strategy_Year = @Strategy_Year)
        '  AND (Target IS NOT NULL) 
        '  AND (ISNUMERIC(Target) = 1)"

        Dim selectedYear As String = ddlYear.SelectedValue

        Dim parameters As SqlParameter() = {
            New SqlParameter("@Strategy_id", strategyId),
            New SqlParameter("@Strategy_Year", selectedYear)}
        ' ดึงข้อมูลจากฐานข้อมูล
        Dim dt As DataTable = QueryDataTable2(SQLRN, dbConn, "Dashboard", parameters)

        ' ตรวจสอบค่าความคืบหน้า
        Dim progressPercent As Double
        Dim hasData As Boolean = False

        If dt.Rows.Count > 0 AndAlso Not IsDBNull(dt.Rows(0)("AvgPercentProgress")) Then
            Double.TryParse(dt.Rows(0)("AvgPercentProgress").ToString(), progressPercent)
            hasData = True
        End If

        ' เตรียมข้อมูลสำหรับ JavaScript
        Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Dim seriesData As String
        Dim labelsData As String
        Dim colorsData As String

        If hasData Then
            Dim remainingPercent As Double = 100 - progressPercent
            seriesData = serializer.Serialize(New List(Of Double) From {progressPercent, remainingPercent})
            labelsData = serializer.Serialize(New List(Of String) From {"ความก้าวหน้า", "ที่เหลือ"})
            colorsData = serializer.Serialize(New List(Of String) From {"#50a5f1", "#50a5f182"})
            ' เลือกสีตามระดับความคืบหน้า
            'If progressPercent >= 90 Then
            '    ' เขียว
            '    colorsData = serializer.Serialize(New List(Of String) From {"#34c38f", "#34c38f82"})
            'ElseIf progressPercent >= 70 Then
            '    ' เหลือง 
            '    colorsData = serializer.Serialize(New List(Of String) From {"#f1b44c", "#f1b44c70"})
            'Else
            '    ' แดง 
            '    colorsData = serializer.Serialize(New List(Of String) From {"#f46a6a", "#f46a6a94"})
            'End If
        Else
            ' ไม่มีข้อมูล
            seriesData = serializer.Serialize(New List(Of Double) From {100})
            labelsData = serializer.Serialize(New List(Of String) From {"ตัวชี้วัดไม่จัดทำ PA"})
            colorsData = serializer.Serialize(New List(Of String) From {"#50a5f1", "#50a5f182"}) ' เทา
        End If


        ' สร้าง Script สำหรับ Pie Chart ด้วย ApexCharts
        Dim script As String = $"
<script type='text/javascript'>
    $(document).ready(function () {{
        var options = {{
            chart: {{
                height: 380,
                type: 'pie'
            }},
            series: {seriesData},
            labels: {labelsData},
            colors: {colorsData},
            dataLabels: {{
                style: {{
                    fontSize: '18px'
                }}
            }},
            legend: {{
                show: true,
                position: 'bottom',
                fontSize: '14px'
            }},
            tooltip: {{
                y: {{
                    formatter: function (val) {{
                        return val.toFixed(2) + '%';
                    }}
                }}
            }},
            responsive: [{{
                breakpoint: 600,
                options: {{
                    chart: {{ height: 240 }},
                    legend: {{ show: false }}
                }}
            }}]
        }};

        var chart = new ApexCharts(document.querySelector('#{divId}'), options);
        chart.render();
    }});
</script>"

        ' แสดง Script ในหน้าเว็บ
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "PieChartScript_" & divId, script, False)
    End Sub

    Private Sub column_chart1(dt As DataTable, strategyId As Integer)
        Dim labels As New List(Of String)
        Dim values As New List(Of Decimal)

        ' อ่านข้อมูลและแปลง PercentProgress เป็น Decimal
        For Each row As DataRow In dt.Rows
            Dim progressObj = row("PercentProgress")
            Dim progressPercent As Decimal = 0
            If Decimal.TryParse(progressObj.ToString(), progressPercent) = False Then
                progressPercent = 0
            End If

            labels.Add(row("ProjectName").ToString())
            values.Add(progressPercent)
        Next

        ' Serialize ข้อมูลเป็น JSON
        Dim serializer As New JavaScriptSerializer()
        Dim jsonLabels As String = serializer.Serialize(labels)
        Dim jsonValues As String = serializer.Serialize(values)

        ' สร้าง JavaScript สำหรับ ApexCharts
        ' ใช้ plotOptions.bar.colors.ranges เพื่อกำหนดสีแต่ละแท่งตามค่า
        Dim script As String = "<script type='text/javascript'>" &
        "var options = {" &
        "   chart: { height: 400, type: 'bar' }," &
        "   series: [{ name: 'ความก้าวหน้า (%)', data: " & jsonValues & " }]," &
        "   xaxis: { categories: " & jsonLabels & " }," &
        "   yaxis: { max: 100 }," &
        "   dataLabels: {" &
"       enabled: true," &
"       offsetY: -50," &
"       style: { colors: ['#000'], fontSize: '15px'}," &   ' สีตัวหนังสือดำ
"       formatter: function (val) { return val.toFixed(1) + '%'; }" & ' แสดง % และปัดทศนิยม 1 ตำแหน่ง
"   }," &
        "   plotOptions: {" &
        "       bar: {" &
        "           dataLabels: { position: 'top' }," &
        "           colors: {" &
        "               ranges: [" &
        "                   { from: 0, to: 69.9999, color: '#f46a6a' }," &   ' แดง
        "                   { from: 70, to: 89.9999, color: '#ffda3b' }," & ' เหลือง
        "                   { from: 90, to: 100, color: '#34c38f' }" &       ' เขียว
        "               ]" &
        "           }" &
        "       }" &
        "   }" &
        "};" &
        "var chart = new ApexCharts(document.querySelector('#column_chart1'), options);" &
        "chart.render();" &
        "</script>"

        ' ลงทะเบียน Script ให้ทำงานบนหน้า
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "chart_" & strategyId, script, False)
    End Sub
    Private Sub bar_chart1(dt As DataTable, strategyId As Integer)
        Dim categories As New List(Of String)()
        Dim values As New List(Of Decimal)()

        For Each row As DataRow In dt.Rows
            categories.Add(row("ProjectName").ToString())
            values.Add(Convert.ToDecimal(row("PercentProgress")))
        Next

        Dim serializer As New JavaScriptSerializer()
        Dim jsonCategories As String = serializer.Serialize(categories)
        Dim jsonValues As String = serializer.Serialize(values)

        Dim divId As String = "bar_chart1"

        Dim script As String = $"
<script type='text/javascript'>
    $(document).ready(function () {{
        var options = {{
            chart: {{
                height: 380,
                type: 'bar',
                toolbar: {{ show: false }}
            }},
            plotOptions: {{
                bar: {{
                    horizontal: true,
                    borderRadius: 4,
                    barHeight: '60%',
                    dataLabels: {{ position: 'right' }},
                    colors: {{
                        ranges: [
                            {{ from: 0, to: 69.9999, color: '#f46a6a' }},
                            {{ from: 70, to: 89.9999, color: '#ffda3b' }},
                            {{ from: 90, to: 100, color: '#34c38f' }}
                        ]
                    }}
                }}
            }},
            series: [{{
                name: 'ความก้าวหน้า (%)',
                data: {jsonValues}
            }}],
            xaxis: {{
                categories: {jsonCategories},
                max: 110,
                labels: {{ style: {{ fontSize: '16px' }} }}
            }},
            dataLabels: {{
                enabled: true,
                formatter: function (val) {{
                    return val.toFixed(2) + '%';
                }},
                style: {{
                    fontSize: '18px',
                    colors: ['#000']
                }},
                offsetX: 10,
                textAnchor: 'start',
                position: 'right'
            }},
            tooltip: {{
                y: {{
                    formatter: function (val) {{
                        return val.toFixed(2) + '%';
                    }}
                }}
            }},
            grid: {{ borderColor: '#f1f1f1' }}
        }};

        var chart = new ApexCharts(document.querySelector('#{divId}'), options);
        chart.render();
    }});
</script>"


        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "barChart" & strategyId, script, False)
    End Sub



    Protected Sub btnBack_Click(sender As Object, e As EventArgs)
        Panel1.Visible = True
        Panel2.Visible = False

        Dim selectedYear As String = ddlYear.SelectedValue
        SearchData(selectedYear)

    End Sub






    '    Private Sub Bindgrid()
    '        Dim SQLRN As String = "SELECT        CourseKM.Course_id, CourseKM.user_id, CourseKM.dept_id, CourseKM.title, CASE WHEN status = 0 THEN 'รออนุมัติ' WHEN status = 1 THEN 'อนุมัติ' WHEN status = 2 THEN 'ไม่อนุมัติ' WHEN status = 3 THEN 'แก้ไข/ปรับปรุง' ELSE '' END AS status, CASE WHEN itjobs.dbo.title_technical.title_technicalName IS NULL 
    '                         THEN itjobs.dbo.title.title_name + itjobs.dbo.[user].fname + SPACE(2) + itjobs.dbo.[user].lname ELSE itjobs.dbo.title_technical.title_technicalName + itjobs.dbo.[user].fname + SPACE(2) 
    '                         + itjobs.dbo.[user].lname END AS Fullname
    'FROM            CourseKM INNER JOIN
    '                         itjobs.dbo.[user] ON CourseKM.user_id = itjobs.dbo.[user].user_id INNER JOIN
    '                         itjobs.dbo.title ON itjobs.dbo.[user].title_id = itjobs.dbo.title.title_id LEFT OUTER JOIN
    '                         itjobs.dbo.title_technical ON itjobs.dbo.[user].title_technicalID = itjobs.dbo.title_technical.title_technicalID 
    'WHERE        (dbo.CourseKM.adder_id = " & ck("user_id") & ") AND (dbo.CourseKM.Active = 1)
    'ORDER BY CourseKM.Course_id DESC" '" & ck("user_id") & "

    '        Dim dt As DataTable
    '        dt = QueryDataTable2(SQLRN, dbConn, "VSAPP", Nothing)
    '        If dt.Rows.Count > 0 Then
    '            GridView1.DataSource = dt
    '            GridView1.DataBind()

    '            'btnsubmit.Visible = False
    '            'btnsubmit2.Visible = True
    '            'btnupdate.Visible = False
    '            'btnCancel.Visible = True
    '        End If
End Class