<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MinibleV2.Master" CodeBehind="Research_Innovation.aspx.vb" Inherits="Dashboard.Research_Innovation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Quark/quark_fontface.css" rel="stylesheet" />
    <link href="minible/layouts/assets/css/Style.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card">
        <div class="card-body">
            <h3><b>ผลงานวิจัยที่ได้รับการจดทะเบียนทรัพย์สินทางปัญญา</b></h3>
            <div class="row">
                <div class="col-md-12 col-xl-5">
                    <div class="row container-fluid mt-8 mb-8">
                        <div class="col-6 col-xl-5">
                            <div class="text-center">
                                <img src="minible/layouts/assets/images/Res1.png" class="H-img" />
                                <h3 class="mb-3 mt-4" id="total1" runat="server"><b></b></h3>
                                <p class="text-muted mb-0"><i class="bx bx-food-menu"></i>สิทธิบัตร</p>
                            </div>
                        </div>

                        <div class="col-6 col-xl-5">
                            <div class="text-center">
                                <img src="minible/layouts/assets/images/Res2.png" class="H-img" />
                                <h3 class="mb-3 mt-4" id="total2" runat="server"><b></b></h3>
                                <p class="text-muted mb-0"><i class="bx bx-food-menu"></i>อนุสิทธิบัตร</p>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-12 col-xl-7 custom-divider">
                    <div class="mt-3">
                        <div id="sales-analytics-chart" class="apex-charts" dir="ltr"></div>
                    </div>
                </div>
            </div>
            <div class="container-fluid">
                <h4>รายละเอียดผลงานที่ได้รับการจดทะเบียนทรัพย์สินทางปัญญา</h4>
                <div class="row m-4">
                    <div class="col-12">
                        <div class="d-flex align-items-center flex-wrap gap-4">

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

                            <div class="d-flex align-items-center flex-wrap m-3">
                                <p class="fw-semibold mb-0 me-3 font-size-18">ประเภท</p>

                                <div class="form-check form-check-inline mb-0 me-4">
                                    <input class="form-check-input" type="radio" name="formRadios" id="radioAll" value="all" checked>
                                    <label class="form-check-label" for="radioAll">ทั้งหมด</label>
                                </div>

                                <div class="form-check form-check-inline mb-0 me-4">
                                    <input class="form-check-input" type="radio" name="formRadios" id="radioPatent" value="patent">
                                    <label class="form-check-label" for="radioPatent">สิทธิบัตร</label>
                                </div>

                                <div class="form-check form-check-inline mb-0">
                                    <input class="form-check-input" type="radio" name="formRadios" id="radioUtility" value="utility">
                                    <label class="form-check-label" for="radioUtility">อนุสิทธิบัตร</label>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
                                                       <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                                           DataKeyNames="no" AllowPaging="True" OnPageIndexChanging="OnPaging"
                                           EmptyDataText="No records has been added."
                                           CssClass="table" CellPadding="4" ForeColor="#333333" GridLines="None" PageSize="200">
                                                           <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                           <Columns>
                                               <asp:TemplateField HeaderStyle-CssClass="align-content-center" HeaderText="ชื่อเรื่อง">
                                                   <ItemTemplate>
                                                       <asp:Label ID="title" runat="server" CssClass="table-warning" Text='<%# Eval("title") %>'></asp:Label>
                                                   </ItemTemplate>
                                                   <HeaderStyle CssClass="align-content-center" />
                                               </asp:TemplateField>
                                               <asp:TemplateField HeaderStyle-CssClass="align-content-center" HeaderText="เจ้าของผลงาน">
                                                   <ItemTemplate>
                                                       <asp:Label ID="lblUser" runat="server" CssClass="table-warning" Text='<%# Eval("fullname") %>'></asp:Label>
                                                   </ItemTemplate>
                                                   <HeaderStyle CssClass="align-content-center" />
                                               </asp:TemplateField>
                                               <asp:TemplateField HeaderStyle-CssClass="align-content-center" HeaderText="ประเภท">
                                                   <ItemTemplate>
                                                       <asp:Label ID="lbltype" runat="server" Text='<%# Eval("type_name") %>'></asp:Label>
                                                   </ItemTemplate>
                                                   <HeaderStyle CssClass="align-content-center" />
                                               </asp:TemplateField>
                                               <asp:TemplateField HeaderStyle-CssClass="align-content-center" HeaderText="วันที่ยื่นคำขอ">
                                                   <ItemTemplate>
                                                       <asp:Label ID="lblDate" runat="server" Text='<%# Eval("request_date") %>'></asp:Label>
                                                   </ItemTemplate>
                                                   <HeaderStyle CssClass="align-content-center" />
                                               </asp:TemplateField>
                                               <asp:TemplateField HeaderStyle-CssClass="align-content-center" HeaderText="เลขที่คำขอ">
                                                   <ItemTemplate>
                                                       <asp:Label ID="lblnumber" runat="server" Text='<%# Eval("request_number") %>'></asp:Label>
                                                   </ItemTemplate>
                                                   <HeaderStyle CssClass="align-content-center" />
                                               </asp:TemplateField>
                                           </Columns>
                                                           <EditRowStyle BackColor="#999999" />
                                           <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
                                           <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                           <PagerStyle ForeColor="White" HorizontalAlign="Center" BackColor="#284775" />
                                           <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                           <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" Font-Bold="True" />
                                           <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                           <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                           <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                           <SortedDescendingHeaderStyle BackColor="#6F8DAE" />

                                       </asp:GridView>

            </div>
        </div>

    </div>

    <!-- apexcharts -->
    <%--<script src="minible/layouts/assets/libs/apexcharts/apexcharts.min.js"></script>

    <script src="minible/layouts/assets/js/pages/dashboard.init.js"></script>

    <script src="minible/layouts/assets/js/app.js"></script>--%>

   <script>
       document.addEventListener("DOMContentLoaded", function () {
           // ดึงค่าจาก URL เช่น ?year=2567&type=patent
           const urlParams = new URLSearchParams(window.location.search);
           const selectedType = urlParams.get("type") || "all"; // ถ้าไม่มีค่า ให้เป็น all

           // ตั้งค่า checked ตามที่เลือก
           document.querySelectorAll("input[name='formRadios']").forEach(r => {
               if (r.value === selectedType) {
                   r.checked = true;
               }
           });

           // เมื่อคลิก radio ให้เปลี่ยน URL แล้ว reload หน้า
           document.querySelectorAll("input[name='formRadios']").forEach(r => {
               r.addEventListener("change", function () {
                   const currentUrl = new URL(window.location.href);
                   currentUrl.searchParams.set("type", this.value);
                   window.location.href = currentUrl.toString();
               });
           });
       });
   </script>

</asp:Content>
