<%@ Control Language="C#" AutoEventWireup="true" Inherits="WebControls_Funcao_UCCadastroFuncao"
    CodeBehind="UCCadastroFuncao.ascx.cs" %>
<%@ Register Src="../Combos/UCComboFuncao.ascx" TagName="UCComboFuncao" TagPrefix="uc1" %>
<%@ Register Src="../Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc3" %>
<%@ Register Src="../Mensagens/UCLoader.ascx" TagName="UCLoader" TagPrefix="uc1" %>
<asp:UpdatePanel ID="updCadastroFuncoes" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <uc1:UCLoader ID="UCLoader1" runat="server" AssociatedUpdatePanelID="updCadastroFuncoes" />
        <div id="divCadastro" runat="server" visible="false">
            <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Funcao" />
            <fieldset>
                <uc3:UCCamposObrigatorios ID="UCCamposObrigatorios2" runat="server" />
                <uc1:UCComboFuncao ID="UCComboFuncao1" runat="server" />
                <asp:Label ID="LabelMatricula" runat="server" Text="Matrícula" AssociatedControlID="txtMatricula"></asp:Label>
                <asp:TextBox ID="txtMatricula" runat="server" MaxLength="50" SkinID="text20C"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvMatricula" runat="server" ErrorMessage="Matrícula é obrigatório."
                    Display="Dynamic" ControlToValidate="txtMatricula" ValidationGroup="Funcao" Visible="False">*</asp:RequiredFieldValidator>
                <asp:Label ID="LabelUA" runat="server" Text="Unidade administrativa *" AssociatedControlID="txtUA"></asp:Label>
                <asp:TextBox ID="txtUA" runat="server" Enabled="False" MaxLength="200" SkinID="text30C"></asp:TextBox>
                <asp:ImageButton ID="btnPesquisarUA" runat="server" CausesValidation="False" SkinID="btPesquisar"
                    OnClick="btnPesquisarUA_Click" />
                <asp:RequiredFieldValidator ID="rfvUA" ControlToValidate="txtUA" ValidationGroup="Funcao"
                    runat="server" ErrorMessage="Unidade administrativa é obrigatório.">*</asp:RequiredFieldValidator>
                <asp:CheckBox ID="chkResponsavelUA" Text="Responsável UA" runat="server" />
                <asp:Label ID="LabelVigenciaIni" runat="server" Text="Vigência inicial *" AssociatedControlID="txtVigenciaIni"></asp:Label>
                <asp:TextBox ID="txtVigenciaIni" runat="server" MaxLength="10" SkinID="Data"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvVigenciaInicial" ControlToValidate="txtVigenciaIni"
                    ValidationGroup="Funcao" runat="server" ErrorMessage="Vigência inicial é obrigatório.">*</asp:RequiredFieldValidator>
                <asp:CustomValidator ID="cvVigenciaIni" runat="server" ControlToValidate="txtVigenciaIni"
                    ValidationGroup="Funcao" Display="Dynamic" ErrorMessage="" OnServerValidate="ValidarData_ServerValidate">* </asp:CustomValidator>
                <asp:Label ID="LabelVigenciaFinal" runat="server" Text="Vigência final" AssociatedControlID="txtVigenciaFim"></asp:Label>
                <asp:TextBox ID="txtVigenciaFim" runat="server" MaxLength="10" SkinID="Data"></asp:TextBox>
                <asp:CustomValidator ID="cvVigenciaFim" runat="server" ControlToValidate="txtVigenciaFim"
                    ValidationGroup="Funcao" Display="Dynamic" ErrorMessage="" OnServerValidate="ValidarData_ServerValidate">* </asp:CustomValidator>
                <asp:Label ID="LabelFuncaoSituacao" runat="server" Text="Situação *" AssociatedControlID="ddlFuncaoSituacao"></asp:Label>
                <asp:DropDownList ID="ddlFuncaoSituacao" runat="server" AppendDataBoundItems="True"
                    SkinID="text30C">
                    <asp:ListItem Value="-1">-- Selecione uma situação --</asp:ListItem>
                    <asp:ListItem Value="1">Ativo</asp:ListItem>
                    <asp:ListItem Value="4">Designado</asp:ListItem>
                    <asp:ListItem Value="5">Afastado</asp:ListItem>
                    <asp:ListItem Value="6">Desativado</asp:ListItem>
                </asp:DropDownList>
                <asp:CompareValidator ID="cpvFuncaoSituacao" runat="server" ErrorMessage="Situação é obrigatório."
                    ControlToValidate="ddlFuncaoSituacao" Operator="GreaterThan" ValueToCompare="0"
                    Display="Dynamic" ValidationGroup="Funcao">*</asp:CompareValidator>
                <asp:Label ID="LabelObservacao" runat="server" Text="Observação" AssociatedControlID="txtObservacao"></asp:Label>
                <asp:TextBox ID="txtObservacao" runat="server" TextMode="MultiLine" SkinID="limite4000"></asp:TextBox>
                <div class="right">
                    <asp:Button ID="_btnIncluir" runat="server" Text="Incluir" ValidationGroup="Funcao"
                        OnClick="_btnIncluir_Click" />
                    <asp:Button ID="_btnCancelar" runat="server" Text="Cancelar" CausesValidation="False"
                        OnClientClick="$('#divFuncoes').dialog('close');" />
                </div>
            </fieldset>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
