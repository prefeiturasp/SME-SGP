<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComboAnosLetivos.ascx.cs" Inherits="AreaAluno.WebControls.Combos.UCComboAnosLetivos" %>

<div id="divAnosLetivos" runat="server" visible="false" class="divComboAno notprint">
    <asp:Label ID="lblAnosLetivos" runat="server" Text="Ano letivo - " />
    <asp:DropDownList ID="ddlAnosLetivos" runat="server"
        DataTextField="Ano" DataValueField="mtu_id"
        OnSelectedIndexChanged="ddlAnosLetivos_SelectedIndexChanged"
        AutoPostBack="true">
    </asp:DropDownList>
</div>
