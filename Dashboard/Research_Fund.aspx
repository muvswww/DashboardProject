<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MinibleV2.Master" CodeBehind="Research_Fund.aspx.vb" Inherits="Dashboard.Research_Fund" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Quark/quark_fontface.css" rel="stylesheet" />
        <style>
        .gridview-scroll {
    max-height: 500px;   /* ปรับความสูงตามต้องการ */
    overflow-y: auto;
}

.fixed-header th {
    position: sticky;
    top: 0;
    background: #ffffff;   /* สำคัญมาก */
    z-index: 10;
}

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
       <div class="row">
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

        <div class="col-md-12 col-xl-6">
            <div class="card">
                <div class="card-body">
                    <h4><b>ทุนวิจัย</b></h4>
                    <div class="row container-fluid mt-8 mb-8">
                        <div class="col-6">
                            <div class="text-center">
                                <p class="text-muted mb-0 "><i class="bx bx-wallet font-size-22 mx-2 text-danger"></i><b class="text-danger">จำนวนเงินที่ได้รับ</b></p>
                                <h2 class="mb-3 mt-4" id="amount1" runat="server"></h2>
                                <p class="mb-3 mt-4 text-danger">บาท</p>

                            </div>
                        </div>

                        <div class="col-6">
                            <div class="text-center">
                                <p class="text-muted mb-0"><i class="bx bx-news font-size-22  mx-2 text-primary"></i><b class="text-primary">จำนวนโครงการ</b></p>
                                <h2 class="mb-3 mt-4" id="Project1" runat="server"></h2>
                                <p class="mb-3 mt-4 text-primary">โครงการ</p>
                            </div>
                        </div>

                    </div>

                </div>
            </div>
        </div>
        <div class="col-md-12 col-xl-6">
            <div class="card">
                <div class="card-body">
                    <h4><b>บริการวิชาการ</b></h4>
                    <div class="row container-fluid mt-8 mb-8">
                        <div class="col-6">
                            <div class="text-center">
                                <p class="text-muted mb-0 "><i class="bx bx-wallet font-size-22 mx-2 text-danger"></i><b class="text-danger">จำนวนเงินที่ได้รับ</b></p>
                                <h2 class="mb-3 mt-4" id="amount2" runat="server"></h2>
                                <p class="mb-3 mt-4 text-danger">บาท</p>

                            </div>
                        </div>

                        <div class="col-6">
                            <div class="text-center">
                                <p class="text-muted mb-0"><i class="bx bx-news font-size-22  mx-2 text-primary"></i><b class="text-primary">จำนวนโครงการ</b></p>
                                <h2 class="mb-3 mt-4" id="Project2" runat="server"></h2>
                                <p class="mb-3 mt-4 text-primary">โครงการ</p>
                            </div>
                        </div>

                    </div>

                </div>
            </div>
        </div>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <div class="row">
        <div class="col-xl-8">
            
            <div class="card">
                <div class="card-body">
                    <div class="container-fluid mt-3">
                        <div class="row mb-3">
    <div class="col-md-12">
        <label class="form-label">รายละเอียด :</label>
        <div class="btn-group w-100" role="group">
            
            <asp:LinkButton ID="btnAll" runat="server" OnClick="Filter_Click" CommandArgument="" 
                CssClass="btn btn-soft-primary">
                <i class="mdi mdi-check-circle-outline me-1"></i>ทั้งหมด
            </asp:LinkButton>

            <asp:LinkButton ID="btnType1" runat="server" OnClick="Filter_Click" CommandArgument="1" 
                CssClass="btn btn-outline-primary">
                <i class="mdi mdi-circle-outline me-1"></i>ทุนวิจัย
            </asp:LinkButton>

            <asp:LinkButton ID="btnType2" runat="server" OnClick="Filter_Click" CommandArgument="2" 
                CssClass="btn btn-outline-primary">
                <i class="mdi mdi-circle-outline me-1"></i>บริการวิชาการ
            </asp:LinkButton>

        </div>
    </div>
</div>

 <div data-simplebar style="max-height: 400px;">
<div class="table-responsive gridview-scroll">
    <asp:GridView ID="data" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover fixed-header" EmptyDataText="ไม่พบข้อมูล">
        <Columns>
            <asp:BoundField DataField="fullname" HeaderText="หัวหน้าโครงการ" />
            <asp:BoundField DataField="title" HeaderText="ชื่อโครงการ" />
            <asp:BoundField DataField="Fund_source" HeaderText="แหล่งทุน" />
            <asp:BoundField DataField="type_name" HeaderText="ประเภท" />
            <asp:BoundField DataField="amount" HeaderText="จำนวนเงินที่ได้รับตามสัญญารับทุน (บาท)" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
            <asp:TemplateField HeaderText="ระยะเวลาตามสัญญารับทุน">
            <ItemTemplate>
                <%# GetProjectStatus(Eval("fnDate"), Eval("GrantExtDate")) %>
                <%# DisplayDuration(Eval("stDate"), Eval("fnDate"), Eval("GrantExtDate")) %>
            </ItemTemplate>
        </asp:TemplateField>
        </Columns>
    </asp:GridView>
</div>
</div>
                    </div>
                </div>
            </div>
            <div class="card">
    <div class="card-body">
        <div class="container-fluid mt-3">
            <div id="chart_Fund" class="apex-charts" dir="ltr"></div>
        </div>
    </div>
</div>
        </div>
        <div class="col-xl-4">
            <div class="card">
    <div class="card-body">
        <h4>แหล่งทุน</h4>
        <div id="pie_chart_per" class="apex-charts" dir="ltr"></div>
         <div data-simplebar style="max-height: 520px;">
     <div class="table-responsive">
         <table class="table table-borderless table-centered" id="fundTable">

    <thead>
        <tr>
            <th>สี</th>
            <th>ชื่อแหล่งทุน</th>
            <th>จำนวนโครงการ</th>
        </tr>
    </thead>
    <tbody></tbody>
</table>

        </div>
        </div>
        </div>
        </div>
        </div>
        

    </div>
 </ContentTemplate>
</asp:UpdatePanel>
    <!-- apexcharts -->
    <script src="minible/layouts/assets/libs/apexcharts/apexcharts.min.js"></script>
    <script src="minible/layouts/assets/js/pages/apexcharts.init.js"></script>
    <script src="minible/layouts/assets/js/pages/dashboard.init.js"></script>

    <script src="minible/layouts/assets/js/app.js"></script>
</asp:Content>
