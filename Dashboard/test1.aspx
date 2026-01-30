<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MinibleV2.Master" CodeBehind="test1.aspx.vb" Inherits="Dashboard.test1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>แก้ไขในเว็บ github</h1>
       <asp:Panel ID="Panel1" runat="server">
        <div class="mb-3 d-flex justify-content-end align-items-center">
            <label for="ddlYear " class="me-2">ประจำปีงบประมาณ</label>
            <asp:DropDownList ID="ddlYear" runat="server"
                CssClass="form-select w-auto" AutoPostBack="True"
                OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
            </asp:DropDownList>
        </div>

        <h4 class="mb-4 text-center" id="FiscalYear" runat="server"></h4>
        <div class="row">
            <asp:Repeater ID="RptStrategy" runat="server" OnItemDataBound="RptStrategy_OnItemDataBound" OnItemCommand="RptStrategy_ItemCommand">
                <ItemTemplate>
                    <div class="col-md-12 col-xl-6">
                        <div class="card">
                            <div class="card-body text-center">
                                <%--<h4 class="card-title mb-4">ยุทธศาสตร์ที่ <%# Eval("Strategy_id") %> <%# Eval("StrategyName") %></h4>--%>
                                <h4 id="lblStrategyName" runat="server" class="card-title mb-4">
                                    <%# "ยุทธศาสตร์ที่ " & Eval("Strategy_id") & " " & Eval("StrategyName") %>
                                </h4>

                                <h5 class="card-title mb-4"></h5>
                                <div id='<%# "pie_chart_per_" & Eval("Strategy_id") %>' class="apex-charts" dir="ltr"></div>
                                <%--<asp:LinkButton type="button" ID="LinkButton1" runat="server" class="btn btn-outline-primary waves-effect waves-light mt-3">รายละเอียด</asp:LinkButton>--%>
                                <%--<asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn btn-outline-primary waves-effect waves-light mt-3" OnClick="LinkButton1_Click">รายละเอียด</asp:LinkButton>--%>
                                <asp:LinkButton ID="lnkDetail" runat="server" CssClass="btn btn-outline-primary waves-effect waves-light mt-3" CommandName="ShowDetail" CommandArgument='<%# Eval("Strategy_id") %>'>
                            รายละเอียด
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </asp:Panel>
    <asp:Panel ID="Panel2" runat="server">
        <div class="row">
        <div class="col-md-12">
            <div class="card">
                <div class="card-body text-center">
                    <div class="text-start">
                        <asp:LinkButton ID="btnBack" runat="server" OnClick="btnBack_Click" CssClass="btn btn-outline-primary btn-rounded waves-effect waves-light">< ย้อนกลับ</asp:LinkButton>
                  
                    </div>
                    <h4 class="card-title mb-4" id="lblStrategyTitle" runat="server"></h4>
                    <%--<h5 class="card-title mb-4" id="lblStrategySubTitle" runat="server"></h5>--%>
                    
    <div class="mb-3 d-flex justify-content-end align-items-center">
        <asp:DropDownList ID="ddlChartType" runat="server" AutoPostBack="True"
            CssClass="form-select w-auto" OnSelectedIndexChanged="ddlChartType_SelectedIndexChanged">
            <asp:ListItem Value="bar" Text="Bar Chart"></asp:ListItem>
            <asp:ListItem Value="column" Text="Column Chart"></asp:ListItem>
        </asp:DropDownList>
    </div>
                    <p class="text-center fs-5 fw-semibold mt-4">
  รายงานผลตามตัวชี้วัดแผนยุทธศาสตร์ ที่ 
  <span class="badge bg-gradient-info">มี</span> 
  ค่าเป้าหมายของส่วนงาน
</p>

<div id="column_chart1" class="apex-charts" dir="ltr"></div>
                    <div id="bar_chart1" class="apex-charts" dir="ltr"></div>
                     <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red"></asp:Label>


                    
                </div>  
                <div class="card mt-4">
                     <div class="card-body">
                         <p class="text-center fs-5 fw-semibold mt-4">
  รายงานผลตามตัวชี้วัดแผนยุทธศาสตร์ ที่ 
  <span class="badge bg-gradient-danger">ไม่มี</span> 
  ค่าเป้าหมายของส่วนงาน
</p>
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
    DataKeyNames="Strategy_Year,Strategy_id,Project_id" AllowPaging="True" OnPageIndexChanging="OnPaging"
    EmptyDataText="No records has been added."
    CssClass="table" CellPadding="4" ForeColor="#333333" GridLines="None" PageSize="200">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="align-content-center" HeaderText="No">
                            <ItemTemplate>
                                <asp:Label ID="lblProject_no" runat="server" CssClass="table-warning" Text='<%# Eval("Project_no") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="align-content-center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-CssClass="align-content-center" HeaderText="Project_id">
                            <ItemTemplate>
                                <asp:Label ID="lblProject_id" runat="server" CssClass="table-warning" Text='<%# Eval("Project_id") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="align-content-center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-CssClass=" text-start" HeaderText="ProjectName">
                            <ItemTemplate>
                                <asp:Label ID="lblProjectName" runat="server" Text='<%# Eval("ProjectName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="align-content-center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-CssClass="align-content-center" HeaderText="ผู้รับผิดชอบ">
                            <ItemTemplate>
                                <asp:Label ID="lblResponsible" runat="server" Text='<%# Eval("Responsible") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="align-content-center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-CssClass="align-content-center" HeaderText="ค่าที่ได้จากไตรมาสล่าสุด">
                            <ItemTemplate>
                                <div style="text-align:center;">
                                <asp:Label ID="lblLastQuarter" runat="server" CssClass="text-center" Text='<%# Eval("LastQuarter") %>'></asp:Label>
                                    </div>
                            </ItemTemplate>
                            <HeaderStyle CssClass="align-content-center" />
                        </asp:TemplateField>
                        
                    </Columns>
                    <EditRowStyle BackColor="#999999" />
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />

</asp:GridView>
                    </div>
                    </div>
            </div>
        </div>
        </div>
    </asp:Panel>

</asp:Content>
