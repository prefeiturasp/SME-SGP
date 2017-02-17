<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCCadastroAvaliacao.ascx.cs" Inherits="GestaoEscolar.WebControls.LancamentoAvaliacao.UCCadastroAvaliacao" %>

<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios" TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoAtividadeAvaliativa.ascx" TagName="UCComboTipoAtividadeAvaliativa" TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Habilidades/UCHabilidades.ascx" TagName="UCHabilidades" TagPrefix="uc4" %>

<asp:UpdatePanel ID="updMessageAtividade" runat="server">
    <ContentTemplate>
        <asp:Label ID="lblMessageAtividade" runat="server" EnableViewState="False"></asp:Label>
        <asp:ValidationSummary ID="vsAtividade" runat="server" ValidationGroup="Atividade" />
    </ContentTemplate>
</asp:UpdatePanel> 
<asp:UpdatePanel ID="updAtividade" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel ID="pnlAtividadeAvaliativa" runat="server">
            <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios2" runat="server" />
        
            <asp:Label ID="lblData" runat="server" Text="<%$ Resources:UserControl, LancamentoAvaliacao.UCCadastroAvaliacao.lblData.Text %>" AssociatedControlID="txtData"></asp:Label>
            <asp:TextBox ID="txtData" runat="server" SkinID="Data" autofocus></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvData" runat="server" ErrorMessage="Data é obrigatória."
                ControlToValidate="txtData" Display="Dynamic" ValidationGroup="Atividade">*</asp:RequiredFieldValidator>
            <asp:CustomValidator ID="cvData" runat="server" ControlToValidate="txtData"
                ValidationGroup="Atividade" Display="Dynamic" ErrorMessage="" OnServerValidate="ValidarData_ServerValidate">*</asp:CustomValidator>

            <asp:Label ID="lblComponenteAtAvaliativa" runat="server" Text="<%$ Resources:Mensagens, MSG_DISCIPLINA %>" AssociatedControlID="ddlComponenteAtAvaliativa"></asp:Label>
            <asp:DropDownList ID="ddlComponenteAtAvaliativa" runat="server" AppendDataBoundItems="True"
                AutoPostBack="true" DataTextField="tur_tud_nome" DataValueField="tur_tud_id" SkinID="text20C" OnSelectedIndexChanged="ddlComponenteAtAvaliativa_SelectedIndexChanged">
            </asp:DropDownList>
                
            <uc3:UCComboTipoAtividadeAvaliativa ID="UCComboTipoAtividadeAvaliativa" runat="server"
                ValidationGroup="Atividade" Obrigatorio="true" Validator_IsValid="true" MostrarMessageOutros="true" />
                            
            <asp:Label ID="lblNomeAtividade" runat="server" Text="<%$ Resources:UserControl, LancamentoAvaliacao.UCCadastroAvaliacao.lblNomeAtividade.Text %>" AssociatedControlID="txtNomeAtividade"></asp:Label>
            <asp:TextBox ID="txtNomeAtividade" runat="server" SkinID="text60C"></asp:TextBox>
                    
            <asp:Label ID="lblConteudoAtividade" runat="server" Text="<%$ Resources:UserControl, LancamentoAvaliacao.UCCadastroAvaliacao.lblConteudoAtividade.Text %>" AssociatedControlID="txtConteudoAtividade">
            </asp:Label>
            <asp:TextBox ID="txtConteudoAtividade" runat="server" TextMode="MultiLine" SkinID="limite4000">
            </asp:TextBox>
                    
            <fieldset runat="server" id="fdsHabilidadesRelacionadas">
                <legend><asp:Literal ID="litHabilidadesRelacionadas" runat="server" Text="<%$ Resources:UserControl, LancamentoAvaliacao.UCCadastroAvaliacao.litHabilidadesRelacionadas.Text %>"></asp:Literal> </legend>
                <div></div>
                <uc4:UCHabilidades runat="server" ID="UCHabilidades" TituloFildSet="Expectativa de aprendizagem" LegendaCheck="Planejada" bHabilidaEdicao="True" />
            </fieldset>

            <asp:CheckBox ID="chkAtividadeExclusiva" runat="server" Text="<%$ Resources:UserControl, LancamentoAvaliacao.UCCadastroAvaliacao.chkAtividadeExclusiva.Text %>" />

            <div class="right" id="divBotaoAcaoAtividade" runat="server">
                <asp:Button ID="btnSalvarAtividade" runat="server" Text="Salvar" OnClick="btnSalvarAtividade_Click" ValidationGroup="Atividade" Visible="false" />
                <asp:Button ID="btnCancelarAtividade" runat="server" Text="Cancelar" OnClick="btnCancelarAtividade_Click" CausesValidation="false" />
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>