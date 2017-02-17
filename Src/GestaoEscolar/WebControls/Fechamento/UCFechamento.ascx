<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCFechamento.ascx.cs" Inherits="GestaoEscolar.WebControls.Fechamento.UCFechamento" %>

<%@ Register Src="~/WebControls/AlunoEfetivacaoObservacao/UCAlunoEfetivacaoObservacaoGeral.ascx" TagName="UCAlunoEfetivacaoObservacaoGeral" TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/BoletimCompletoAluno/UCBoletimAngular.ascx" TagName="UCBoletim" TagPrefix="uc" %>
<%@ Register Src="~/WebControls/Fechamento/UCFechamentoPadrao.ascx" TagName="UCFechamentoPadrao" TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Fechamento/UCFechamentoFinal.ascx" TagName="UCFechamentoFinal" TagPrefix="uc1" %>

<asp:UpdatePanel ID="upnMensagem" runat="server">
    <ContentTemplate>
        <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
        <asp:Label ID="lblMessage2" runat="server" EnableViewState="False"></asp:Label>
        <asp:Label ID="lblMessage3" runat="server" EnableViewState="False"></asp:Label>
        <asp:ValidationSummary ID="vsEfetivacaoNotas" runat="server" Style="display: none;" />
        <asp:Label ID="lblFixMessage" runat="server" EnableViewState="True"></asp:Label>
    </ContentTemplate>
</asp:UpdatePanel>

<div class="right divSalvarFechamento" style="display: none;">
    <asp:UpdatePanel ID="upnBotoes2" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Button ID="btnSalvar2" runat="server" Text="Salvar" OnClick="btnSalvar_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<br />

<asp:UpdatePanel ID="upnStatusFechamento" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div style="float: left; border: 1px solid #DDD; padding: 10px;" runat="server" id="divStatusFechamento">
            <asp:Image ID="imgStatusFechamento" runat="server" Width="18" Style="vertical-align: middle;" />
            <asp:Label ID="lblStatusFechamento" runat="server" Font-Bold="true" Style="margin-left: 5px; vertical-align: middle; display: inline-block;"></asp:Label>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<asp:Panel ID="pnlAlunos" runat="server" DefaultButton="btnSalvar">
    <div id="divLoading" runat="server" visible="false" style="clear: both; border: 1px solid #DDD; padding: 50px 10px; text-align: center;">
        <asp:Image runat="server" ID="imgLoading" />
        <asp:Label ID="lblLoading" runat="server" Font-Bold="true" Style="margin-left: 5px; vertical-align: 10px; font-size: 1.4em;"></asp:Label>
    </div>    
    <uc1:UCFechamentoPadrao ID="UCFechamentoPadrao" runat="server" Visible="false" />
    <uc1:UCFechamentoFinal ID="UCFechamentoFinal" runat="server" Visible="false" />
    <asp:Timer ID="tmrLoad" OnTick="tmrLoad_Tick" runat="server" Enabled="false"></asp:Timer>
    <asp:HiddenField ID="hdnTentativas" runat="server" Value="0" />
</asp:Panel>

<div class="right divSalvarFechamento" style="display: none;">
    <asp:UpdatePanel ID="upnBotoes" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click" />
            <asp:HiddenField ID="hdnVisibleBotaoSalvar" runat="server" Value="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<asp:HiddenField ID="hdnTurmaDisciplina" runat="server" />
<asp:HiddenField ID="hdnPosicaoDocente" runat="server" />
<asp:HiddenField ID="hdnTudSituacao" runat="server" />
<asp:HiddenField ID="hdnVariacaoFrequencia" runat="server" />

<!-- Relatorio do aluno -->
<div id="divRelatorio" title="Relatório de avaliação" class="hide">
    <fieldset id="fdsRelatorio" runat="server">
        <asp:UpdatePanel ID="_updRelatorio" runat="server">
            <ContentTemplate>
                <asp:Label ID="lblDadosRelatorio" runat="server"></asp:Label>
                <br />
                <br />
                <asp:Label ID="lblMessageRelatorio" runat="server" EnableViewState="False"></asp:Label>
                <asp:TextBox ID="txtArea" runat="server" TextMode="MultiLine" SkinID="limite4000"></asp:TextBox>
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
            <asp:Button ID="btnSalvarRelatorio" runat="server" Text="Salvar" OnClick="btnSalvarRelatorio_Click" CausesValidation="false" />
            <asp:Button ID="btnCancelarRelatorio" runat="server" Text="Cancelar" CausesValidation="false"
                OnClientClick="$('#divRelatorio').dialog('close'); return false;" />
        </div>
    </fieldset>
</div>

<!-- Cadastro de observacao geral para o aluno -->
<div id="divCadastroObservacaoGeral" title="Observação">
    <fieldset class="fieldset-fechamento-gestor">
        <asp:UpdatePanel ID="updObservacaoGeral" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:HiddenField ID="hdnIndiceObservacaoGeral" runat="server" />

                    <uc1:UCAlunoEfetivacaoObservacaoGeral ID="UCAlunoEfetivacaoObservacaoGeral" runat="server" TipoFechamento="2" AnotacoesAlunoVisible="false" />

            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSalvar" />
                <asp:PostBackTrigger ControlID="btnSalvar2" />
            </Triggers>
        </asp:UpdatePanel>
        </fieldset>
</div>

<!-- Boletim completo do aluno -->
<div id="divBoletimCompleto" title="Boletim completo do aluno" class="hide">
    <asp:UpdatePanel ID="updBoletimCompleto" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset>
                <uc:UCBoletim ID="UCBoletim" runat="server" />
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>