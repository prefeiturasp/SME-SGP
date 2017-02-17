<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCAlunoDeficiente.ascx.cs"
    Inherits="GestaoEscolar.WebControls.AlunoDeficiente.UCAlunoDeficiente" %>
<div style="float: left; clear: none; width: 200px; margin: 10px;">
    <fieldset id="fsEquipamentos" runat="server" visible="false">
        <legend>Equipamentos / Suporte pedagógico</legend>
        <div id="_Equipamentos" class="Equipamentos" runat="server">
            <asp:CheckBoxList ID="chlEquipamentos" runat="server" DataTextField="ted_nome" DataValueField="ted_id">
            </asp:CheckBoxList>
        </div>
    </fieldset>
</div>
