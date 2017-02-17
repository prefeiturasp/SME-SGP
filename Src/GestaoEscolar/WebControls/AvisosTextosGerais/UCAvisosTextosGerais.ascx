<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCAvisosTextosGerais.ascx.cs" Inherits="GestaoEscolar.WebControls.AvisosTextosGerais.UCAvisosTextosGerais" %>

<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagName="UCComboUAEscola"
    TagPrefix="uc2" %>
<%@ Register Src="../Combos/Novos/UCCCursoCurriculo.ascx" TagName="UCComboCursoCurriculo"
    TagPrefix="uc3" %>
<%@ Register Src="../Combos/UCComboAvisoTextoGeralCampoAuxiliar.ascx" TagName="UCComboCampoAuxiliar"
    TagPrefix="uc4" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<%--<script type="text/javascript">
    $(function () {
        $("input[id$='chkAnotacoes']").click(function () {
            if ($(this).attr('checked')) {
                $("input[id$='txtDescricao']").val = CKEDITOR.instances.ctl00_ContentPlaceHolder1_UCAvisosTextosGerais1_txtDescricao.insertText("[Anotacoes]");
            }
            else {
                editor = CKEDITOR.instances.ctl00_ContentPlaceHolder1_UCAvisosTextosGerais1_txtDescricao;

                var edata = editor.getData();

                var replaced_text = edata.replace("[Anotacoes]", "");

                editor.setData(replaced_text);
            }
        });
    });

</script>--%>

<%--<script src="../../Includes/jquery-2.0.3.min.js"></script>
<script src="../../Includes/redactor/redactor.js"></script>
<link rel="stylesheet" href="../../Includes/redactor/redactor.css" />--%>

<asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Avisos" />
    </ContentTemplate>
</asp:UpdatePanel>
<fieldset>
    <legend>
        <asp:Label runat="server" ID="lblFdsMain">Cadastro de avisos e textos gerais</asp:Label></legend><%--Cadastro do cabeçalho de avisos e textos gerais--%>
    <asp:UpdatePanel ID="updFiltroAviso" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset runat="server" id="fdsFiltros">
                <legend>Parâmetros de avisos e textos gerais</legend>
                <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server" />
                <uc2:UCComboUAEscola ID="UCComboUAEscola1" runat="server" CarregarEscolaAutomatico="true"
                    ObrigatorioEscola="true"
                    MostrarMessageSelecioneEscola="true"
                    MostrarMessageSelecioneUA="true" ValidationGroup="Avisos" OnIndexChangedUA="UCComboUAEscola1_IndexChangedUA"
                    OnIndexChangedUnidadeEscola="UCComboUAEscola1_IndexChangedUnidadeEscola" />
                <uc3:UCComboCursoCurriculo ID="UCComboCursoCurriculo" MostrarMensagemSelecione="true"
                    runat="server" Obrigatorio="true" ValidationGroup="Avisos" />
                <br />
                <div style="display: inline-block">
                    <asp:Label ID="lblTitulo" runat="server" Text="Titulo do texto *" AssociatedControlID="txtTitulo"></asp:Label>
                    <asp:TextBox ID="txtTitulo" runat="server" MaxLength="200" SkinID="text60C"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtTitulo"
                        Display="Dynamic" ErrorMessage="Titulo do texto é obrigatório." ValidationGroup="Avisos">*</asp:RequiredFieldValidator>
                </div>
                <div style="display: inline-block">
                    <asp:Label ID="lblSituacao" runat="server" Text="Situação" AssociatedControlID="cmbSituacao"></asp:Label>
                    <asp:DropDownList ID="cmbSituacao" runat="server">
                        <asp:ListItem Value="1" Selected="True">Ativo</asp:ListItem>
                        <asp:ListItem Value="4">Inativo</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="updDeclaracao" runat="server" UpdateMode="Conditional" Visible="false">
        <ContentTemplate>
            <fieldset runat="server" id="fdsDeclaracao">
                <legend>Parâmetros da declaração</legend>
                <div style="display: inline-block">
                    <asp:Label ID="lblTitDeclaracao" runat="server" Text="Declaração" AssociatedControlID="txtTitDeclaracao"></asp:Label>
                    <asp:TextBox ID="txtTitDeclaracao" runat="server" SkinID="text60C"></asp:TextBox>
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
    <fieldset style="width: 70%">
        <legend>Descrição do texto</legend>
        <asp:UpdatePanel ID="updFiltroTipoAux" runat="server">
            <ContentTemplate>
                <div id="divCampoAuxiliar" runat="server">
                    <div style="display: inline-block">
                        <uc4:UCComboCampoAuxiliar ID="UCComboCampoAuxiliar1" runat="server" ObrigatorioTipo="true"
                            ValidationGroupTipo="Avisos" />
                    </div>
                    <div style="display: inline-block" runat="server" id="divChk">
                        <asp:CheckBox ID="chkTimbre" runat="server" Text="Utilizar cabeçalho padrão" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <CKEditor:CKEditorControl ID="txtDescricao" BasePath="/includes/ckeditor/" runat="server"></CKEditor:CKEditorControl>
        <asp:RequiredFieldValidator runat="server" ID="rfvDescricao" ValidationGroup="Avisos" ControlToValidate="txtDescricao"
            Display="Dynamic" ErrorMessage="Descrição é obrigatório.">*</asp:RequiredFieldValidator>
        <%--<textarea id="redactor_content" name="content" style="height: 300px;" runat="server" class="redactor"></textarea>--%>
    </fieldset>
    <div class="right">
        <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click"
            ValidationGroup="Avisos" />
        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click"
            CausesValidation="false" />
    </div>
</fieldset>

