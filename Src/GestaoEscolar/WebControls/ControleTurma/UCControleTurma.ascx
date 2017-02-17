<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCControleTurma.ascx.cs" Inherits="GestaoEscolar.WebControls.ControleTurma.UCControleTurma" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCComboGenerico.ascx" TagName="UCComboGenerico" TagPrefix="uc10" %>
<script type="text/javascript">
    function MostraComboAlterarTurma()
    {
        $('#spTrocarTurma').css('display', 'inline-block');
        $('#spTrocarTurma').find('select').focus();
        $('#spTituloTurma').css('display', 'none');
    }
</script>
<legend>
    <span id="spTituloTurma" style="display: inline-block;">
        <asp:Label ID="lblTurma" runat="server"></asp:Label>
        <asp:LinkButton ID="lbkAlterarTurma" runat="server" Text="(Alterar turma)"
            OnClientClick="MostraComboAlterarTurma(); return false;"
            style="margin-left: 3px;">
        </asp:LinkButton>
    </span>
    <span id="spTrocarTurma" style="display: none">
    <uc10:uccombogenerico id="uccTurmaDisciplina" runat="server"
        mostrarmensagemselecione="false" obrigatorio="false"
        valoritemvazio="-1;-1;-1;-1" ConfirmExit="true"></uc10:uccombogenerico>
    <asp:ImageButton ID="btnCancelarAlterarTurma" ToolTip="Cancelar ação" runat="server"
        SkinID="btDesfazer" OnClientClick="$('#spTrocarTurma').css('display','none');$('#spTituloTurma').css('display','inline-block');return false;" />
    </span>
</legend>
<div class="planEscopoCompartilhada" style="clear:none;">
<asp:Panel ID="pnlDisciplinaCompartilhada" runat="server" Visible="false" >
    <br />
    <span id="spTituloDisciplinaCompartilhada" style="display: inline-block;"> 
        <asp:Label ID="lblTituloDisciplinaCompartilhada" runat="server" Text="<%$ Resources:UserControl, ControleTurma.UCControleTurma.lblTituloDisciplinaCompartilhada.Text %>"/>
        <asp:Label ID="lblDisciplinaCompartilhada" runat="server"/>
        <asp:LinkButton ID="lbkAlterarDisciplinaCompartilhada" runat="server" Text="<%$ Resources:UserControl, ControleTurma.UCControleTurma.lbkAlterarDisciplinaCompartilhada.Text %>"
            OnClientClick="$('#spTrocaDisciplinaCompartilhada').css('display','inline-block');$('#spTrocaDisciplinaCompartilhada').find('select').focus();$('#spTituloDisciplinaCompartilhada').css('display','none');return false;"
            style="margin-left: 3px;">
        </asp:LinkButton>
    </span>
    <span id="spTrocaDisciplinaCompartilhada" style="display: none">
        <uc10:uccombogenerico id="uccDisciplinaCompartilhada" runat="server"
            mostrarmensagemselecione="false" obrigatorio="false"
            valoritemvazio="-1;-1" ConfirmExit="true" SkinID_Combo="text30C"></uc10:uccombogenerico>
        <asp:ImageButton ID="btnCancelarAlterarDisciplinaCompartilhada" ToolTip="Cancelar ação" runat="server"
            SkinID="btDesfazer" OnClientClick="$('#spTrocaDisciplinaCompartilhada').css('display','none');$('#spTituloDisciplinaCompartilhada').css('display','inline-block');return false;" />
    </span>
</asp:Panel>
<asp:Panel ID="pnlTurmasMultisseriada" runat="server" Visible="false">
    <asp:Label ID="lblTurmasNormais" runat="server" Font-Bold="true" Text="<%$ Resources:UserControl, ControleTurma.UCControleTurma.lblTurmasNormais.Text %>"></asp:Label>
    <asp:CheckBoxList ID="chkTurmasNormaisMultisseriadas" runat="server" DataTextField="tur_codigo" DataValueField="tur_id_tud_id"
        AutoPostBack="true" OnSelectedIndexChanged="chkTurmasNormaisMultisseriadas_SelectedIndexChanged" RepeatDirection="Horizontal">
    </asp:CheckBoxList>
</asp:Panel>
</div>