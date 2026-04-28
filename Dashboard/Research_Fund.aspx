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
    .grid-clean {
        border-collapse: collapse;
        width: 100%;
        font-size: 14px;
    }

    /* เส้นตารางชัดเจน */
    .grid-clean th,
    .grid-clean td {
        border: 1px solid #bfbfbf;
        padding: 6px 8px;
    }

    /* หัวตารางมีสี */
    .grid-clean th {
        background-color: #d8dadd;
        font-weight: 600;
        text-align: center;
    }

    /* แถวสลับสีอ่อนมาก */
    .grid-clean tr:nth-child(even) {
        /*background-color: #f8f9fa;*/
        background-color: #ebf2fb;
    }

    /* Hover เบา ๆ */
    .grid-clean tr:hover {
        background-color: #eef5ff;
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
                                <p class="text-muted mb-0 d-flex align-items-center justify-content-center"><i class="bx bx-wallet font-size-22  mx-2 text-danger"></i><b class="text-danger">จำนวนเงินที่ได้รับ</b></p>
                                <h2 class="mb-3 mt-4" id="amount1" runat="server"></h2>
                                <p class="mb-3 mt-4 text-danger">บาท</p>

                            </div>
                        </div>

                        <div class="col-6">
                            <div class="text-center">
                                <p class="text-muted mb-0 d-flex align-items-center justify-content-center"><i class="bx bx-news font-size-22  mx-2 text-primary"></i><b class="text-primary">จำนวนโครงการ</b></p>
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
                                <p class="text-muted mb-0 d-flex align-items-center justify-content-center"><i class="bx bx-wallet font-size-22 mx-2 text-danger"></i><b class="text-danger">จำนวนเงินที่ได้รับ</b></p>
                                <h2 class="mb-3 mt-4" id="amount2" runat="server"></h2>
                                <p class="mb-3 mt-4 text-danger">บาท</p>

                            </div>
                        </div>

                        <div class="col-6">
                            <div class="text-center">
                                <p class="text-muted mb-0 d-flex align-items-center justify-content-center"><i class="bx bx-news font-size-22 mx-2 text-primary"></i><b class="text-primary">จำนวนโครงการ</b></p>
                                <h2 class="mb-3 mt-4" id="Project2" runat="server"></h2>
                                <p class="mb-3 mt-4 text-primary">โครงการ</p>
                            </div>
                        </div>

                    </div>

                </div>
            </div>
        </div>
    </div>
    <asp:Panel ID="PanelUser" runat="server">
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
            <asp:BoundField DataField="year" HeaderText="ปีงบประมาณ" />
            <asp:BoundField DataField="type_name" HeaderText="ประเภท" />
            <asp:BoundField DataField="fullname" HeaderText="หัวหน้าโครงการ" />
            <asp:BoundField DataField="Fund_type" HeaderText="แหล่งทุน" />
            <%--<asp:BoundField DataField="period" HeaderText="ระยะเวลาสัญญา" />--%>
                <asp:TemplateField HeaderText="ระยะเวลาสัญญา">
    <ItemTemplate ><%# Eval("period") %>
        <%# GetProjectStatus(Eval("fnDate"), Eval("GrantExtDate")) %>
        
        
    </ItemTemplate>
</asp:TemplateField>
            <asp:BoundField DataField="amount" HeaderText="จำนวนเงินที่ได้รับตามสัญญารับทุน (บาท)" DataFormatString="{0:N2}" />
            
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
    </asp:Panel>
    <asp:Panel ID="PanelAdmin" runat="server">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
    <ContentTemplate>
    <div class="row">
        <div class="col-xl-12">
            
            <div class="card">
                <div class="card-body">
                    <div class="container-fluid mt-3">
                        <div class="row mb-3">
    <div class="col-md-12">
       <%-- <label class="form-label">รายละเอียด :</label>--%>
        <div style="display:flex; align-items:center;" class="m-2">

    <label class="form-label">รายละเอียด :</label>
         
    
<asp:LinkButton 
    ID="lbtExcel" 
    runat="server" 
    OnClick="lbtExcel_Click"
    CssClass="btn btn-success waves-effect waves-light mb-0 d-flex align-items-center justify-content-center text-white"
    Style="margin-left:auto; padding: 0.2rem;">
    
    <i class="mdi mdi-microsoft-excel font-size-22  mx-2"></i><span class=" mx-2">Export</span>
</asp:LinkButton>

          <%--<div class="d-flex justify-content-center">
    <asp:LinkButton 
        ID="lbtExcel" 
        runat="server" 
        OnClick="lbtExcel_Click"
        CssClass="btn btn-success" Font-Size="15px" Style="margin-left:auto; text-decoration:none;">
      
        <i class="mdi mdi-microsoft-excel"></i>  <span>Export</span>
    </asp:LinkButton>
</div>--%>
   <%-- <asp:LinkButton ID="lbtExcel"
        runat="server"
        OnClick="lbtExcel_Click"
        Style="margin-left:auto; color:#339933; text-decoration:none;">
        Export
        <i class="mdi mdi-microsoft-excel me-2"
           style="color:#339933; font-size:28px;"></i>

    </asp:LinkButton>--%>

</div>
        <div class="btn-group w-100" role="group">
            
            <asp:LinkButton ID="btnAllAdmin" runat="server" OnClick="Filter_Click" CommandArgument="" 
                CssClass="btn btn-soft-primary">
                <i class="mdi mdi-check-circle-outline me-1"></i>ทั้งหมด
            </asp:LinkButton>

            <asp:LinkButton ID="btnType11" runat="server" OnClick="Filter_Click" CommandArgument="1" 
                CssClass="btn btn-outline-primary">
                <i class="mdi mdi-circle-outline me-1"></i>ทุนวิจัย
            </asp:LinkButton>

            <asp:LinkButton ID="btnType22" runat="server" OnClick="Filter_Click" CommandArgument="2" 
                CssClass="btn btn-outline-primary">
                <i class="mdi mdi-circle-outline me-1"></i>บริการวิชาการ
            </asp:LinkButton>
        </div>
    </div>
</div>
  <%--                   
<div class="table-responsive">
    <div style="max-height:400px; overflow:auto;">
    <asp:GridView ID="dataForadmin" runat="server"
        AutoGenerateColumns="False"
        CssClass="table table-bordered table-hover"
        style="min-width:1400px;">

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
</div>--%>
     <div class="table-responsive">
    <div style="max-height:500px; overflow:auto;">

        <asp:GridView ID="dataForadmin" runat="server"
            CssClass="table table-bordered table-hover fixed-header grid-clean"
            EmptyDataText="ไม่พบข้อมูล">
        </asp:GridView>

    </div>
</div>

                    </div>
                </div>
            </div>
            
        </div>
        
        

    </div>
 </ContentTemplate>
</asp:UpdatePanel>
    </asp:Panel>
    <!-- apexcharts -->
    <script src="minible/layouts/assets/libs/apexcharts/apexcharts.min.js"></script>
    <script src="minible/layouts/assets/js/pages/apexcharts.init.js"></script>
    <script src="minible/layouts/assets/js/pages/dashboard.init.js"></script>

    <script src="minible/layouts/assets/js/app.js"></script>
</asp:Content>
