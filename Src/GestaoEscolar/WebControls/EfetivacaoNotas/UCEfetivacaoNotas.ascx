<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCEfetivacaoNotas.ascx.cs" Inherits="GestaoEscolar.WebControls.EfetivacaoNotas.UCEfetivacaoNotas" %>

<%@ Register Src="~/WebControls/AlunoEfetivacaoObservacao/UCAlunoEfetivacaoObservacao.ascx" TagName="UCAlunoEfetivacaoObservacao" TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/EfetivacaoNotas/UCEfetivacaoNotasPadrao.ascx" TagName="UCEfetivacaoNotasPadrao" TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/EfetivacaoNotas/UCEfetivacaoNotasFinal.ascx" TagName="UCEfetivacaoNotasFinal" TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/BoletimCompletoAluno/UCBoletimCompletoAluno.ascx" TagName="UCBoletimCompletoAluno" TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/NavegacaoLancamentoClasse/UCNavegacaoLancamentoClasse.ascx" TagName="UCNavegacaoLancamentoClasse" TagPrefix="uc4" %>
<%@ Register Src="~/WebControls/Combos/UCComboAvaliacao.ascx" TagName="UCComboAvaliacao" TagPrefix="uc5" %>

<asp:UpdatePanel ID="upnMensagem" runat="server">
    <ContentTemplate>
        <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
        <asp:Label ID="_lblMessage2" runat="server" EnableViewState="False"></asp:Label>
        <asp:Label ID="_lblMessage3" runat="server" EnableViewState="False"></asp:Label>
        <asp:ValidationSummary ID="vsEfetivacaoNotas" runat="server" style="display:none;"/>
    </ContentTemplate>
</asp:UpdatePanel>
<fieldset style="padding-top: 0; overflow: hidden; width: auto; clear: both;">
    <legend>Dados da turma</legend>
    <div>
        <uc4:UCNavegacaoLancamentoClasse ID="UCNavegacaoLancamentoClasse1" runat="server" />
        <br />
        <asp:Label ID="lblTurmaDisciplina" runat="server" Text="<%$ Resources:UserControl, EfetivacaoNotas.UCEfetivacaoNotas.lblTurmaDisciplina.Text %>" AssociatedControlID="ddlTurmaDisciplina"></asp:Label>
        <asp:DropDownList ID="ddlTurmaDisciplina" runat="server" AppendDataBoundItems="True"
            AutoPostBack="true" DataTextField="tur_tud_nome" DataValueField="tur_tud_id"
            SkinID="text60C" OnSelectedIndexChanged="ddlTurmaDisciplina_SelectedIndexChanged">
        </asp:DropDownList>
        <div style="float: right; border: 1px solid #DDD; padding: 10px;" runat="server" id="divStatusFechamento">
            <asp:Image ID="imgStatusFechamento" runat="server" />
            <asp:Label ID="lblStatusFechamento" runat="server" Font-Bold="true" style="margin-left:5px; vertical-align:2px;" ></asp:Label>
        </div>
        <uc5:UCComboAvaliacao ID="UCComboAvaliacao1" runat="server" />
        <br />
        <div class="right divBtnCadastro">
            <asp:UpdatePanel ID="upnBotoes2" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <asp:Button ID="_btnSalvar2" runat="server" Text="Salvar" OnClick="_btnSalvar_Click" />
                    <asp:Button ID="_btnCancelar2" runat="server" Text="Cancelar" CausesValidation="False" OnClick="_btnCancelar_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</fieldset>
<asp:Panel ID="_pnlAlunos" runat="server" GroupingText="Efetivação de notas" DefaultButton="_btnSalvar">
    <uc2:UCEfetivacaoNotasPadrao ID="UCEfetivacaoNotasPadrao" runat="server" Visible="false" />
    <uc2:UCEfetivacaoNotasFinal ID="UCEfetivacaoNotasFinal" runat="server" Visible="false" />
</asp:Panel>
<fieldset>
    <div class="right">
        <asp:UpdatePanel ID="upnBotoes" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <asp:Button ID="_btnSalvar" runat="server" Text="Salvar" OnClick="_btnSalvar_Click" />
                <asp:Button ID="_btnCancelar" runat="server" Text="Cancelar" CausesValidation="False" OnClick="_btnCancelar_Click" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</fieldset>
<asp:HiddenField ID="hdnPosicaoDocente" runat="server" />
<asp:HiddenField ID="hdnTudSituacao" runat="server" />
<asp:HiddenField ID="hdnVariacaoFrequencia" runat="server" />
<div id="divRelatorio" title="Relatório de avaliação" class="hide">
    <fieldset id="fdsRelatorio" runat="server">
        <asp:UpdatePanel ID="_updRelatorio" runat="server">
            <ContentTemplate>
                <asp:Label ID="lblDadosRelatorio" runat="server"></asp:Label>
                <br />
                <br />
                <asp:Label ID="_lblMessageRelatorio" runat="server" EnableViewState="False"></asp:Label>
                <asp:TextBox ID="_txtArea" runat="server" TextMode="MultiLine" SkinID="limite4000"></asp:TextBox>
                <asp:Button ID="btnImportarAnotacoesAluno" runat="server" Text="Importar anotações do aluno"
                    OnClick="btnImportarAnotacoesAluno_Click" CausesValidation="false" />
                <br />
                <br />
                <asp:HyperLink ID="hplAnexoRelatorio" runat="server" Visible="False"></asp:HyperLink>
                <asp:ImageButton ID="btnExcluirAnexoRelatorio" runat="server" CausesValidation="False"
                    SkinID="btExcluirPlanejamento" Visible="false" OnClick="btnExcluirAnexoRelatorio_Click" />
                <br />
                <br />
                <asp:HiddenField ID="hdnIndices" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Label ID="LabelAnexoRelatorio" runat="server" Text="Anexar documento" AssociatedControlID="fupAnexoRelatorio"></asp:Label>
        <asp:FileUpload ID="fupAnexoRelatorio" runat="server" ToolTip="Procurar documento"
            CssClass="text60C" Style="width: 96%" />
        <div class="right">
            <asp:Button ID="btnImprimirRelatorio" runat="server" Text="Imprimir" OnClick="btnImprimirRelatorio_Click" CausesValidation="false" />
            <asp:Button ID="btnSalvarRelatorio" runat="server" Text="Salvar" OnClick="btnSalvarRelatorio_Click" CausesValidation="false" />
            <asp:Button ID="btnCancelarRelatorio" runat="server" Text="Cancelar" CausesValidation="false"
                OnClientClick="$('#divRelatorio').dialog('close'); return false;" />
        </div>
    </fieldset>
</div>
<div id="divRegistroClasse" title="Registro de Classe - Informação sobre o aluno"
    class="hide">
    <asp:HyperLink ID="lkbImprimir" runat="server" onclick="$('#divRegistroClasse').dialog('close');"
        Text="Visualizar relatório" ToolTip="Visualizar relatório gerado" Target="_blank">
    </asp:HyperLink>
    <div class="right">
        <asp:Button ID="btnCancelarRegistroClasse" runat="server" Text="Cancelar" CausesValidation="false"
            OnClientClick="$('#divRegistroClasse').dialog('close'); return false;" />
    </div>
</div>
<div id="divCadastroObservacao" title="Observação">
    <asp:UpdatePanel ID="updObservacao" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="hdnIndiceObservacao" runat="server" />
            <uc1:UCAlunoEfetivacaoObservacao ID="UCAlunoEfetivacaoObservacao" runat="server" TipoFechamento="1" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="_btnSalvar" />
            <asp:PostBackTrigger ControlID="_btnSalvar2" />
        </Triggers>
    </asp:UpdatePanel>
</div>
<!-- Boletim completo do aluno -->
<div id="divBoletimCompleto" title="Boletim completo do aluno" class="hide">
    <asp:UpdatePanel ID="updBoletimCompleto" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset>
                <uc3:UCBoletimCompletoAluno ID="UCBoletimCompletoAluno" runat="server" ImprimirVisible="true" />
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<!-- Justificativa de nota pos-conselho e de nota final -->
<div id="divJustificativa" runat="server" title='Justificativa' class="hide divJustificativa">
    <asp:UpdatePanel ID="updJustificativa" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset>
                <asp:Label ID="lblDadosJustificativa" runat="server"></asp:Label>
                <br /><br />
                <asp:Label ID="lblMsgJustificativa" runat="server" EnableViewState="False"></asp:Label>
                <asp:Label ID="lblJustificativa" runat="server"  AssociatedControlID="txtJustificativa"></asp:Label>
                <asp:TextBox ID="txtJustificativa" runat="server" TextMode="MultiLine" MaxLength="4000" SkinID="limite4000"></asp:TextBox>
                <div class="right">
                    <asp:Button ID="btnSalvarJustificativa" runat="server" CausesValidation="false" Text="Salvar" OnClick="btnSalvarJustificativa_Click" />
                    <asp:Button ID="btnCancelarJustificativa" runat="server" CausesValidation="false" OnClientClick='$(".divJustificativa").dialog("close"); return false;' Text="Cancelar" />
                </div>
                <asp:HiddenField ID="hdnIndiceJustificativa" runat="server" />
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>