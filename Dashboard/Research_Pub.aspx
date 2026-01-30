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

                                <asp:LinkButton
                                    ID="card11"
                                    runat="server"
                                    CssClass="pub-item accent-gold text-decoration-none d-block"
                                    CommandArgument="C11"
                                    OnClick="Metric_Click">

                                    <div class="pub-code">1.1</div>
                                    <div class="pub-code">Total International Publications</div>
                                    <div class="pub-value">
                                        <asp:Label ID="lbl11" runat="server" />
                                    </div>

                                </asp:LinkButton>
                                <asp:LinkButton
                                    ID="card12"
                                    runat="server"
                                    CssClass="pub-item accent-gold text-decoration-none d-block"
                                    CommandArgument="C12"
                                    OnClick="Metric_Click">

                                    <div class="pub-code">1.2</div>
                                    <div class="pub-code">International Publications in Q1 Journal</div>
                                    <div class="pub-value">
                                        <asp:Label ID="lbl12" runat="server" />
                                    </div>

                                </asp:LinkButton>
                                <asp:LinkButton
                                    ID="card13"
                                    runat="server"
                                    CssClass="pub-item accent-gold text-decoration-none d-block"
                                    CommandArgument="C13"
                                    OnClick="Metric_Click">

                                    <div class="pub-code">1.3</div>
                                    <div class="pub-code">International Publications in Top 10 Journal</div>
                                    <div class="pub-value">
                                        <asp:Label ID="lbl13" runat="server" />
                                    </div>

                                </asp:LinkButton>

                                <asp:LinkButton
                                    ID="card19"
                                    runat="server"
                                    CssClass="pub-item accent-gold text-decoration-none d-block"
                                    CommandArgument="C19"
                                    OnClick="Metric_Click">

                                    <div class="pub-code">1.9</div>
                                    <div class="pub-code">International Publications in Top 1 Journal</div>
                                    <div class="pub-value">
                                        <asp:Label ID="lbl19" runat="server" />
                                    </div>

                                </asp:LinkButton>



                                <%-- <div class="pub-item accent-gold">
                                    <div class="pub-code">1.9</div>
                                    <div class="pub-code">International Publications in Top 1 Journal</div>
                                    <div class="pub-value">
                                        <asp:Label ID="lbl19" runat="server" />
                                    </div>
                                </div>--%>
                            </div>

                        </div>
                    </div>

                    <div class="container-fluid mt-3">
                        <div class="row mb-3">
                            <div class="col-md-12">
                                
                                                              <!-- ===== Dropdown เลือกคอลัมน์ ===== -->
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
     style="min-width: 260px;">

    <div class="fw-bold text-secondary mb-2">ข้อมูลบทความ</div>

    <div class="form-check">
        <asp:CheckBox ID="chkTitle" type="checkbox" runat="server" />
        <label class="form-check-label" for="<%= chkTitle.ClientID %>">ชื่อบทความ</label>
    </div>

    <div class="form-check">
        <asp:CheckBox ID="chkAuthors" runat="server"  />
        <label class="form-check-label" for="<%= chkAuthors.ClientID %>">ผู้แต่ง</label>
    </div>

    <div class="form-check">
        <asp:CheckBox ID="chkSource" runat="server"/>
        <label class="form-check-label" for="<%= chkSource.ClientID %>">แหล่งตีพิมพ์</label>
    </div>

    <div class="form-check">
        <asp:CheckBox ID="chkVolume" runat="server" />
        <label class="form-check-label" for="<%= chkVolume.ClientID %>">Volume</label>
    </div>

    <div class="form-check">
        <asp:CheckBox ID="chkIssue" runat="server" />
        <label class="form-check-label" for="<%= chkIssue.ClientID %>">Issue</label>
    </div>

    <div class="form-check">
        <asp:CheckBox ID="chkPages" runat="server" />
        <label class="form-check-label" for="<%= chkPages.ClientID %>">Pages</label>
    </div>

    <div class="form-check mb-2">
        <asp:CheckBox ID="chkDOI" runat="server"  />
        <label class="form-check-label" for="<%= chkDOI.ClientID %>">DOI</label>
    </div>

    <hr />

    <div class="fw-bold text-secondary mb-2">ตัวชี้วัด</div>

    <div class="form-check">
        <asp:CheckBox ID="chkC11" runat="server" />
        <label class="form-check-label" for="<%= chkC11.ClientID %>">[1.1]</label>
    </div>

    <div class="form-check">
        <asp:CheckBox ID="chkC12" runat="server"  />
        <label class="form-check-label" for="<%= chkC12.ClientID %>">[1.2]</label>
    </div>

    <div class="form-check">
        <asp:CheckBox ID="chkC13" runat="server"  />
        <label class="form-check-label" for="<%= chkC13.ClientID %>">[1.3]</label>
    </div>

    <div class="form-check">
        <asp:CheckBox ID="chkC19" runat="server" />
        <label class="form-check-label" for="<%= chkC19.ClientID %>">[1.9]</label>
    </div>

</div>

</div>

<!-- ===== ปุ่มแสดงผล ===== -->
<asp:Button ID="btnApply" runat="server"
    Text="แสดงผล"
    CssClass="btn btn-primary mb-3"
    OnClick="btnApply_Click" />
                                    </div>
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
                                <%--<asp:GridView ID="data" runat="server"
                                    AutoGenerateColumns="False"
                                    CssClass="table table-bordered table-hover fixed-header"
                                    EmptyDataText="ไม่พบข้อมูล">

                                    <Columns>

                                        <asp:BoundField DataField="title" HeaderText="ชื่อบทความ" />
                                        <asp:BoundField DataField="authors" HeaderText="ผู้แต่ง" />
                                        <asp:BoundField DataField="scopus_source" HeaderText="แหล่งตีพิมพ์" />
                                        <asp:BoundField DataField="Volume" HeaderText="Volume" />
                                        <asp:BoundField DataField="Issue" HeaderText="Issue" />
                                        <asp:BoundField DataField="Pages" HeaderText="Pages" />
                                        <asp:BoundField DataField="DOI" HeaderText="DOI" />

                                        <asp:TemplateField HeaderText="[1.1]">
                                            <ItemTemplate>
                                                <%# ShowCheckIcon(Eval("C11")) %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="[1.2]">
                                            <ItemTemplate>
                                                <%# ShowCheckIcon(Eval("C12")) %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="[1.3]">
                                            <ItemTemplate>
                                                <%# ShowCheckIcon(Eval("C13")) %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="[1.9]">
                                            <ItemTemplate>
                                                <%# ShowCheckIcon(Eval("C19")) %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                    </Columns>

                                </asp:GridView>--%>
                              

<!-- ===== GridView ===== -->
<asp:GridView ID="data" runat="server"
    AutoGenerateColumns="False"
    CssClass="table table-bordered table-hover fixed-header"
    EmptyDataText="ไม่พบข้อมูล">

    <Columns>
        <asp:BoundField DataField="title" HeaderText="ชื่อบทความ" />
        <asp:BoundField DataField="authors" HeaderText="ผู้แต่ง" />
        <asp:BoundField DataField="scopus_source" HeaderText="แหล่งตีพิมพ์" />
        <asp:BoundField DataField="Volume" HeaderText="Volume" />
        <asp:BoundField DataField="Issue" HeaderText="Issue" />
        <asp:BoundField DataField="Pages" HeaderText="Pages" />
        <asp:BoundField DataField="DOI" HeaderText="DOI" />

        <asp:TemplateField HeaderText="[1.1]">
            <ItemTemplate><%# ShowCheckIcon(Eval("C11")) %></ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>

        <asp:TemplateField HeaderText="[1.2]">
            <ItemTemplate><%# ShowCheckIcon(Eval("C12")) %></ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>

        <asp:TemplateField HeaderText="[1.3]">
            <ItemTemplate><%# ShowCheckIcon(Eval("C13")) %></ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>

        <asp:TemplateField HeaderText="[1.9]">
            <ItemTemplate><%# ShowCheckIcon(Eval("C19")) %></ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
    </Columns>
</asp:GridView>


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

