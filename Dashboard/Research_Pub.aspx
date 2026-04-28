<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MinibleV2.Master" CodeBehind="Research_Pub.aspx.vb" Inherits="Dashboard.Research_Pub" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <link href="Quark/quark_fontface.css" rel="stylesheet" />
    <link href="minible/layouts/assets/css/Style.css" rel="stylesheet" />

    <style>
        .pub-summary.luxury {
            display: grid;
            grid-template-columns: repeat(4, 1fr);
            background: #ffffff;
            border-radius: 14px;
            overflow: hidden;
            box-shadow: 0 10px 30px rgba(0,0,0,0.06);
            border: 1px solid #e6e6e6;
        }

        .pub-item {
            padding: 26px 18px 30px;
            text-align: center;
            position: relative;
        }

            .pub-item:not(:last-child) {
                border-right: 1px solid #f0f0f0;
            }

            /* accent line */
            .pub-item::before {
                content: "";
                position: absolute;
                top: 0;
                left: 22%;
                right: 22%;
                height: 3px;
                border-radius: 2px;
            }

        /* luxury colors */
        .accent-gold::before {
            background: #b38686;
        }

        .pub-code {
            font-size: 16px;
            font-weight: 600;
            color: #6b7280;
            letter-spacing: 0.8px;
        }

        .pub-value {
            font-size: 40px;
            margin: 8px 0 6px;
            color: #111827; /* สีหลักเดียว */
        }



        /* subtle hover */
        .pub-item:hover {
            background: #fafafa;
        }

        /* responsive */
        @media (max-width: 768px) {
            .pub-summary.luxury {
                grid-template-columns: 1fr 1fr;
            }

            .pub-item:nth-child(2) {
                border-right: none;
            }
        }

        .gridview-scroll {
            max-height: 500px; /* ปรับความสูงตามต้องการ */
            overflow-y: auto;
        }

        .fixed-header th {
            position: sticky;
            top: 0;
            background: #ffffff; /* สำคัญมาก */
            z-index: 10;
        }
    </style>



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server" />--%>
    <div class="card">
        <div class="card-body">
            <h3>เทียบแต่ละปี</h3>
            <div id="line_chart_datalabel" class="apex-charts" dir="ltr"></div>
        </div>
    </div>
    <asp:UpdatePanel ID="updDashboard" runat="server">
        <ContentTemplate>
            <div class="card">
                <div class="card-body">
                    <div class="dropdown m-3">
                        <a class="dropdown-toggle text-reset" href="#" id="dropdownMenuButton1"
                            data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                            <span class="fw-semibold font-size-18">ปี พ.ศ.</span>
                            <span id="lblSelectedYear" runat="server" class="text-muted">ทั้งหมด</span>
                            <i class="mdi mdi-chevron-down ms-1"></i>
                        </a>
                        <div class="dropdown-menu" aria-labelledby="dropdownMenuButton1" id="yearDropdown" runat="server">
                        </div>
                    </div>
                    <div class="text-center">
                        <div class="row container-fluid m-5">
                            <div class="col-6">
                                <div class="row">
                                    <div class="col-12 col-md-2">
                                        <img src="minible/layouts/assets/images/3.2.png" class="H-img" />
                                    </div>
                                    <div class="col-12 col-md-5">
                                        <h2><b id="sumType1" runat="server"></b></h2>
                                        <h4>ผลงานวิจัยระดับชาติ</h4>
                                    </div>
                                </div>
                            </div>
                            <div class="col-6">
                                <div class="row">
                                    <div class="col-12 col-md-2">
                                        <img src="minible/layouts/assets/images/4.2.png" class="H-img" />
                                    </div>
                                    <div class="col-12 col-md-5">
                                        <h2><b id="sumType2" runat="server"></b></h2>
                                        <h4>ผลงานวิจัยระดับนานาชาติ</h4>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="container-fluid mt-5">

                            <div class="pub-summary luxury mt-5">

                         
                            </div>

                        </div>
                    </div>

                    <div class="container-fluid mt-3">
                        <div class="row mb-3">
                            <div class="col-md-12">
                                
                                                            
<div class="d-flex justify-content-end align-items-start gap-2 mb-3">
<div class="dropdown mb-3">

    <button class="btn btn-outline-secondary dropdown-toggle"
        type="button"
        id="colDropdown"
        data-bs-toggle="dropdown"
        aria-expanded="false">
        เลือกคอลัมน์ที่แสดง
    </button>

    <div class="dropdown-menu p-3"
         aria-labelledby="colDropdown"
         onclick="event.stopPropagation();"
         style="min-width: 300px; max-height: 400px; overflow:auto;">

        <!-- 🔥 เลือกทั้งหมด -->
        <div class="form-check mb-2">
            <input type="checkbox" id="chkAllColumns" runat="server"
                   class="form-check-input"
                   onserverchange="chkAllColumns_CheckedChanged" />
            <label class="form-check-label fw-bold text-primary" for="chkAllColumns">
                แสดงทั้งหมด
            </label>
        </div>

        <hr />

        <!-- ข้อมูลบทความ -->
<div class="fw-bold text-secondary mb-2">ข้อมูลบทความ</div>
<div class="form-check"><input type="checkbox" id="chkYear" runat="server" class="form-check-input"  /><label>Year</label></div>
<div class="form-check"><input type="checkbox" id="chkMonth" runat="server" class="form-check-input" /><label>Month</label></div>
<div class="form-check"><input type="checkbox" id="chkType" runat="server" class="form-check-input"/><label>Type</label></div>
<div class="form-check"><input type="checkbox" id="chkTitle" runat="server" class="form-check-input"/><label>Title</label></div>
<div class="form-check"><input type="checkbox" id="chkAuthors" runat="server" class="form-check-input" /><label>Authors</label></div>
<div class="form-check"><input type="checkbox" id="chkSource" runat="server" class="form-check-input" /><label>Scopus</label></div>

<div class="form-check"><input type="checkbox" id="chkVolume" runat="server" class="form-check-input" /><label>Volume</label></div>
<div class="form-check"><input type="checkbox" id="chkIssue" runat="server" class="form-check-input" /><label>Issue</label></div>
<div class="form-check"><input type="checkbox" id="chkPages" runat="server" class="form-check-input" /><label>Pages</label></div>
<div class="form-check"><input type="checkbox" id="chkDOI" runat="server" class="form-check-input" /><label>DOI</label></div>

<div class="form-check"><input type="checkbox" id="chkInterCollab" runat="server" class="form-check-input" /><label>InterCollab</label></div>
<div class="form-check"><input type="checkbox" id="chkAcademicCollab" runat="server" class="form-check-input" /><label>AcademicCollab</label></div>
<div class="form-check"><input type="checkbox" id="chkSDG" runat="server" class="form-check-input" /><label>SDG</label></div>


<div class="form-check"><input type="checkbox" id="chkCluster" runat="server" class="form-check-input" /><label>Cluster</label></div>

<div class="form-check"><input type="checkbox" id="chkQ1" runat="server" class="form-check-input" /><label>Q1</label></div>
<div class="form-check"><input type="checkbox" id="chkQ2" runat="server" class="form-check-input" /><label>Q2</label></div>
<div class="form-check"><input type="checkbox" id="chkQ3" runat="server" class="form-check-input" /><label>Q3</label></div>
<div class="form-check"><input type="checkbox" id="chkQ4" runat="server" class="form-check-input" /><label>Q4</label></div>

<div class="form-check"><input type="checkbox" id="chkTop1" runat="server" class="form-check-input" /><label>Top1</label></div>
<div class="form-check"><input type="checkbox" id="chkTop10" runat="server" class="form-check-input" /><label>Top10</label></div>

<div class="form-check"><input type="checkbox" id="chkTCI1" runat="server" class="form-check-input" /><label>TCI G1</label></div>
<div class="form-check"><input type="checkbox" id="chkTCI2" runat="server" class="form-check-input" /><label>TCI G2</label></div>
<div class="form-check"><input type="checkbox" id="chkTCI3" runat="server" class="form-check-input" /><label>TCI G3</label></div>
    </div>
</div>
<asp:Button ID="btnLoad" runat="server"
    Text="แสดงข้อมูล"
    CssClass="btn btn-primary mb-3"
    OnClick="btnLoad_Click" />
</div>

<!-- ===== ปุ่มแสดงผล ===== -->
                                <label class="form-label">รายละเอียด :</label>
                                <div class="btn-group w-100" role="group">

                                    <asp:LinkButton ID="btnAll" runat="server" OnClick="Filter_Click" CommandArgument=""
                                        CssClass="btn btn-soft-primary">
                <i class="mdi mdi-check-circle-outline me-1"></i>ทั้งหมด
                                    </asp:LinkButton>

                                    <asp:LinkButton ID="btnType1" runat="server" OnClick="Filter_Click" CommandArgument="1"
                                        CssClass="btn btn-outline-primary">
                <i class="mdi mdi-circle-outline me-1"></i>ระดับชาติ
                                    </asp:LinkButton>

                                    <asp:LinkButton ID="btnType2" runat="server" OnClick="Filter_Click" CommandArgument="2"
                                        CssClass="btn btn-outline-primary">
                <i class="mdi mdi-circle-outline me-1"></i>ระดับนานาชาติ
                                    </asp:LinkButton>

                                </div>
                            </div>
                        </div>



                        <div data-simplebar style="max-height: 500px;">
                            <div class="table-responsive gridview-scroll">
<!-- ===== GridView ===== -->
<asp:GridView ID="data" runat="server"
    AutoGenerateColumns="False"
    CssClass="table table-bordered table-hover fixed-header"
    EmptyDataText="ไม่พบข้อมูล">

</asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    
  <%--  <script>
    document.getElementById('<%= chkAllColumns.ClientID %>')
        .addEventListener('change', function () {
            __doPostBack('<%= chkAllColumns.UniqueID %>', '');
        });
    </script>--%>

    <!-- apexcharts -->
    <script src="minible/layouts/assets/libs/apexcharts/apexcharts.min.js"></script>
    <script src="minible/layouts/assets/js/pages/apexcharts.init.js"></script>
    <script src="minible/layouts/assets/js/pages/dashboard.init.js"></script>

    <script src="minible/layouts/assets/js/app.js"></script>
</asp:Content>

