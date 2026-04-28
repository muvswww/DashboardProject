<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MinibleV2.Master" CodeBehind="Research_EdPEx.aspx.vb" Inherits="Dashboard.Research_EdPEx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>.accordion-button{
    background:#6b5f4a;
    color:white;
    font-weight:600;
}

.accordion-button:not(.collapsed){
    background:#6b5f4a;
    color:white;
}</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card">
        <div class="card-body">
            <h4>ตัวชี้วัด EdPEx ฝ่ายวิจัย</h4>
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
      <%--<asp:Repeater ID="rptSection" runat="server" OnItemDataBound="rptSection_ItemDataBound">
<ItemTemplate>

    <h3><%# Eval("sectionName") %></h3>

    <!-- KPI ที่ไม่มี SubSection -->
    <asp:Repeater ID="rptKPINoSub" runat="server">
        <ItemTemplate>
            <div style="margin-left:20px">
                <%# Eval("KPI_no") %> - <%# Eval("KPT_name") %>
                Target: <%# Eval("target") %>
                Result: <%# Eval("ResultValue") %>
            </div>
        </ItemTemplate>
    </asp:Repeater>

    <!-- SubSection -->
    <asp:Repeater ID="rptSubSection" runat="server" OnItemDataBound="rptSubSection_ItemDataBound">
        <ItemTemplate>

            <h4 style="margin-left:20px">
                <%# Eval("subSectionName") %>
            </h4>

            <asp:Repeater ID="rptKPI" runat="server">
                <ItemTemplate>
                    <div style="margin-left:40px">
                        <%# Eval("KPT_name") %>
                        Target: <%# Eval("target") %>
                        Result: <%# Eval("ResultValue") %>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

        </ItemTemplate>
    </asp:Repeater>

</ItemTemplate>
</asp:Repeater>--%>
          <div class="accordion" id="accordionEdpex">

<asp:Repeater ID="rptSection" runat="server" OnItemDataBound="rptSection_ItemDataBound">
<ItemTemplate>

<div class="accordion-item">

<h2 class="accordion-header">

<button class="accordion-button <%# If(Container.ItemIndex = 0, "", "collapsed") %>"
        type="button"
        data-bs-toggle="collapse"
        data-bs-target="#section<%# Eval("section") %>">

Section <%# Eval("section") %> : <%# Eval("sectionName") %>

</button>

</h2>

<div id="section<%# Eval("section") %>"
     class="accordion-collapse collapse <%# If(Container.ItemIndex = 0, "show", "") %>"
     data-bs-parent="#accordionEdpex">

<div class="accordion-body">


<!-- KPI ที่ไม่มี SubSection -->

<asp:Repeater ID="rptKPINoSub" runat="server">

<HeaderTemplate>

<table class="table table-bordered table-sm">

<thead>
<tr>
<th style="width:60%">KPI</th>
<th style="width:20%; text-align:center">เป้าหมาย</th>
<th style="width:20%; text-align:center">ผลการดำเนินงาน</th>
</tr>
</thead>

<tbody>

</HeaderTemplate>

<ItemTemplate>

<tr>

<td>
<%# Eval("KPT_name") %>
</td>

<td style="text-align:center">
<%# IIf(IsDBNull(Eval("target")), "-", Eval("target")) %>
</td>

<td style="text-align:center">
<%# IIf(IsDBNull(Eval("ResultValue")), "-", Eval("ResultValue")) %>
</td>

</tr>

</ItemTemplate>

<FooterTemplate>

</tbody>
</table>

</FooterTemplate>

</asp:Repeater>


<!-- SubSection -->

<asp:Repeater ID="rptSubSection" runat="server" OnItemDataBound="rptSubSection_ItemDataBound">

<ItemTemplate>

<h5 class="mt-3 text-primary">
<b>▸ <%# Eval("subSectionName") %></b>
</h5>

<asp:Repeater ID="rptKPI" runat="server">

<HeaderTemplate>

<table class="table table-bordered table-sm">

<thead>
<tr>
<th style="width:60%">KPI</th>
<th style="width:20%; text-align:center">เป้าหมาย</th>
<th style="width:20%; text-align:center">ผลการดำเนินงาน</th>
</tr>
</thead>

<tbody>

</HeaderTemplate>

<ItemTemplate>

<tr>

<td>
<%# Eval("KPT_name") %>
</td>

<td style="text-align:center">
<%# IIf(IsDBNull(Eval("target")), "-", Eval("target")) %>
</td>

<td style="text-align:center">
<%# IIf(IsDBNull(Eval("ResultValue")), "-", Eval("ResultValue")) %>
</td>

</tr>

</ItemTemplate>

<FooterTemplate>

</tbody>
</table>

</FooterTemplate>

</asp:Repeater>

</ItemTemplate>

</asp:Repeater>


</div>
</div>
</div>

</ItemTemplate>
</asp:Repeater>

</div>
        </div>
    </div>
</asp:Content>
