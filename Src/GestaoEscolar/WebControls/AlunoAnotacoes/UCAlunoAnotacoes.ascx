<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCAlunoAnotacoes.ascx.cs" Inherits="GestaoEscolar.WebControls.AlunoAnotacoes.UCAlunoAnotacoes" %>

<style type="text/css">
        .alinharHeader { text-align:left !important;}
</style>

<asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
<fieldset id="fdsConsulta" runat="server" class="divInformacao">
    <legend>Dados do aluno</legend>
    <asp:Label ID="lblNome" runat="server" Text="<b>Nome: </b>" CssClass="wrap600px"></asp:Label>
    <asp:Label ID="lblDataNascimento" runat="server" Text="<b>Data de nascimento: </b>"></asp:Label>
    <asp:Label ID="lblNomeMae" runat="server" Text="<b>Nome da mãe: </b>"></asp:Label>
    <asp:Label ID="lblSituacao" runat="server" Text="<b>Situação: </b>"></asp:Label>
    <asp:Label ID="lblRA" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="lblDataCadastro" runat="server" Text="<b>Data de cadastro: </b>"></asp:Label>
    <asp:Label ID="lblDataAlteracao" runat="server" Text="<b>Data da última alteração: </b>"></asp:Label>
    <asp:Label ID="lblEscola" runat="server" Text="<b>Escola: </b>" CssClass="wrap600px"></asp:Label>
    <asp:Label ID="lblCurso" runat="server" CssClass="wrap600px"></asp:Label>
    <asp:Label ID="lblPeriodo" runat="server"></asp:Label>
    <asp:Label ID="lblTurma" runat="server" Text="<b>Turma: </b>" CssClass="wrap600px"></asp:Label>
    <asp:Label ID="lblAvaliacao" runat="server" Text="<b>Avaliação: </b>" CssClass="wrap600px"></asp:Label>
    <asp:Label ID="lblNChamada" runat="server" Text="<b>Nº Chamada: </b>"></asp:Label>
</fieldset>
<fieldset id="fdsAnotacoes" runat="server">
    <legend>Anotações sobre o aluno</legend>
    <asp:UpdatePanel ID="updAnotacoes" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="divAnoCalendario" runat="server">
                <asp:Label ID="lblAnoCalendario" runat="server" Text="Ano letivo: " AssociatedControlID="ddlAnoCalendario"></asp:Label>
                <asp:DropDownList ID="ddlAnoCalendario" runat="server" OnSelectedIndexChanged="ddlAnoCalendario_IndexChanged"
                    DataSourceID="odsAnoCalendario" DataValueField="cal_id" DataTextField="cal_ano" AutoPostBack="true">
                </asp:DropDownList>
                <br /><br />
            </div>
            <asp:ObjectDataSource ID="odsAnoCalendario" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.MTR_MatriculaTurma"
                DeleteMethod="Delete" OldValuesParameterFormatString="original_{0}" SelectMethod="GetSelectAnoMatricula"
                TypeName="MSTech.GestaoEscolar.BLL.MTR_MatriculaTurmaBO" UpdateMethod="Save" OnSelecting="odsAnoCalendario_Selecting"></asp:ObjectDataSource>
            <fieldset>
                <legend><asp:Label ID="lblLgdAnotacoesAulas" runat="server" Text="<%$ Resources:UserControl, UCAlunoAnotacoes.lblLgdAnotacoesAulas.Text %>"></asp:Label></legend>
                <asp:GridView ID="grvAnotacoes" runat="server" AutoGenerateColumns="False" 
                    EmptyDataText="<%$ Resources:UserControl, UCAlunoAnotacoes.grvAnotacoes.EmptyDataText %>"
                    DataSourceID="odsAnotacoes" SkinID="GridResponsive">
                    <Columns>
                        <asp:TemplateField HeaderText="Escola">
                            <ItemTemplate>
                                <asp:Label ID="lblEscola" runat="server" Text='<%# Bind("esc_nome") %>' CssClass="wrap250px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="17%" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="tur_codigo" HeaderText="Turma" ItemStyle-Width="5%" />
                        <asp:BoundField DataField="tud_nome" HeaderText="<%$ Resources:Mensagens, MSG_DISCIPLINA %>" ItemStyle-Width="13%" />
                        <asp:BoundField DataField="doc_id" HeaderText="Docente" ItemStyle-Width="16%" />
                        <asp:BoundField DataField="tau_data" HeaderText="Data da aula" DataFormatString="{0: dd/MM/yyyy}" ItemStyle-Width="80px" />
                        <asp:TemplateField HeaderText="Anotações sobre o aluno" HeaderStyle-CssClass="alinharHeader">
                            <ItemTemplate>
                                <asp:Label ID="lblAnotacoes" runat="server" Text='<%# Bind("taa_anotacao") %>' CssClass="wrap600px"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsAnotacoes" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.CLS_TurmaAulaAluno"
                    OldValuesParameterFormatString="original_{0}" SelectMethod="SelecionaAnotacaoPorAlunoCalendario"
                    TypeName="MSTech.GestaoEscolar.BLL.CLS_TurmaAulaAlunoBO" UpdateMethod="Save"
                    OnSelecting="odsAnotacoes_Selecting"></asp:ObjectDataSource>
            </fieldset>
            <div id="divAnotacoesGerais" runat="server">
                <fieldset class="area-form">
                    <legend><asp:Label ID="lblLgdAnotacoesGerais" runat="server" Text="<%$ Resources:UserControl, UCAlunoAnotacoes.lblLgdAnotacoesGerais.Text %>"></asp:Label></legend>
                    <asp:GridView ID="grvAnotacoesGerais" runat="server" AutoGenerateColumns="False" 
                        EmptyDataText="<%$ Resources:UserControl, UCAlunoAnotacoes.grvAnotacoesGerais.EmptyDataText %>" DataKeyNames="ano_id" 
                        DataSourceID="odsAnotacoesGerais" SkinID="GridResponsive"
                        OnRowDataBound="grvAnotacoesGerais_RowDataBound" OnRowCommand="grvAnotacoesGerais_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="ano_dataAnotacao" HeaderText="Data da anotação" DataFormatString="{0: dd/MM/yyyy}" ItemStyle-Width="10%" />
                            <asp:BoundField DataField="gru_nome" HeaderText="Função" ItemStyle-Width="30%" />
                            <asp:TemplateField HeaderText="Anotações" HeaderStyle-CssClass="alinharHeader">
                                <ItemTemplate>
                                    <asp:Label ID="lblAnotacoes" runat="server" Text='<%# Bind("ano_anotacao") %>' CssClass="wrap600px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Editar" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="70px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnEditar" SkinID="btEditar" runat="server" CommandName="Editar" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Excluir" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="70px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnExcluir" SkinID="btExcluir" runat="server" CommandName="Deletar" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <div class="right">
                        <asp:Button ID="btnAddAnotacao" runat="server" Text="<%$ Resources:UserControl, UCAlunoAnotacoes.btnAddAnotacao.Text %>" CausesValidation="False"
                            OnClick="btnAddAnotacao_Click" />
                    </div>
                    <asp:ObjectDataSource ID="odsAnotacoesGerais" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_AlunoAnotacao"
                        OldValuesParameterFormatString="original_{0}" SelectMethod="SelecionaAnotacoesAluno"
                        TypeName="MSTech.GestaoEscolar.BLL.ACA_AlunoAnotacaoBO" UpdateMethod="Save"
                        OnSelecting="odsAnotacoesGerais_Selecting"></asp:ObjectDataSource>
                </fieldset>
            </div>
            <div class="right area-botoes-bottom">
                <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" CausesValidation="False"
                    OnClick="btnImprimir_Click" />
                <asp:Button ID="_btnCancelar" runat="server" Text="Voltar" CausesValidation="False"
                    OnClick="_btnCancelar_Click" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</fieldset>
<div id="divCadastroAnotacao" title="Anotação" class="hide">
    <asp:UpdatePanel ID="updCadastro" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblMessageCadastro" runat="server" EnableViewState="false"></asp:Label>
            <asp:ValidationSummary runat="server" ID="vsAnotacao" ValidationGroup="Anotacao" />
            <asp:Label ID="lblDataAnotacao" runat="server" Text="<%$ Resources:UserControl, UCAlunoAnotacoes.lblDataAnotacao.Text %>" 
                AssociatedControlID="txtDataAnotacao"></asp:Label>
            <asp:TextBox ID="txtDataAnotacao" runat="server" CssClass="maskData" SkinID="Data"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvDataAnotacao" runat="server" ErrorMessage="Data da anotação é obrigatória."
                ControlToValidate="txtDataAnotacao" ValidationGroup="Anotacao" Display="Dynamic">*</asp:RequiredFieldValidator>
            <asp:CustomValidator ID="cvDataAnotacao" runat="server" ControlToValidate="txtDataAnotacao" 
                ValidationGroup="Anotacao" Display="Dynamic" ErrorMessage="Data da anotação não está no formato dd/mm/aaaa." 
                OnServerValidate="ValidarData_ServerValidate">*</asp:CustomValidator>
            <asp:Label ID="lblAnotacao" runat="server" Text="<%$ Resources:UserControl, UCAlunoAnotacoes.lblAnotacao.Text %>"
                AssociatedControlID="txtAnotacao"></asp:Label>
            <%--skin text60C é o mesmo tamanho que o limite4000 mas não tem os eventos onkeypress e onkeyup usados no contador de caractere--%>
            <asp:TextBox ID="txtAnotacao" runat="server" TextMode="MultiLine" MaxLength="4000" SkinID="text60c"
                Text="" CssClass="wrap250px" onkeypress="LimitarCaracter(this,'contadesc3','4000');"
                onkeyup="LimitarCaracter(this,'contadesc3','4000');"></asp:TextBox>
            <span id="contadesc3" style="display: inline; font-size: 85%; position: relative; top: -8px;">0/4000</span>
            <asp:RequiredFieldValidator ID="rfvAnotacao" runat="server" ErrorMessage="Anotação é obrigatória."
                ControlToValidate="txtAnotacao" ValidationGroup="Anotacao" Display="Dynamic">*</asp:RequiredFieldValidator>
            <div class="right">
                <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click" 
                    ValidationGroup="Anotacao" />
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CausesValidation="False"
                    OnClick="btnCancelar_Click1" OnClientClick="var exibirMensagemConfirmacao=true;$('#divCadastroAnotacao').dialog('close'); return false;" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>