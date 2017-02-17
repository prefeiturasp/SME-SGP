<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Classe.JustificativaAbonoFalta.Cadastro" %>

<%@ PreviousPageType VirtualPath="~/Classe/JustificativaAbonoFalta/Busca.aspx" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagPrefix="uc1" TagName="UCCamposObrigatorios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <fieldset id="fdsConsulta" runat="server" class="divInformacao">
        <legend><asp:Label ID="lblLgdDadosAluno" runat="server" Text="<%$ Resources:Classe, JustificativaAbonoFalta.lblLgdDadosAluno.Text %>"></asp:Label></legend>
        <asp:Label ID="lblDisciplina" runat="server" Text="<b>Componente curricular: </b>" CssClass="wrap600px"></asp:Label>
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
        <asp:Label ID="lblNChamada" runat="server" Text="<b>Nº Chamada: </b>"></asp:Label>
    </fieldset>
    <asp:UpdatePanel ID="updJustificativaFalta" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset id="fdsJustificativaFalta" runat="server">
                <legend><asp:Label ID="lblLgdJustificativaFalta" runat="server" Text="<%$ Resources:Classe, JustificativaAbonoFalta.lblLgdJustificativaFalta.Text %>"></asp:Label></legend>
                <div class="area-botoes-bottom">
                    <asp:Button ID="btnAddJustificativaFalta" runat="server" Text="<%$ Resources:Classe, JustificativaAbonoFalta.btnAddJustificativaFalta.Text %>" CausesValidation="False"
                        OnClick="btnAddJustificativaFalta_Click" />
                </div>
                <div class="area-form">
                    <asp:GridView ID="grvJustificativaFalta" runat="server" AutoGenerateColumns="False" 
                        EmptyDataText="<%$ Resources:Classe, JustificativaAbonoFalta.grvJustificativaFalta.EmptyDataText %>" DataKeyNames="ajf_id" 
                        DataSourceID="odsJustificativaFalta" 
                        OnRowDataBound="grvJustificativaFalta_RowDataBound" OnRowCommand="grvJustificativaFalta_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="ajf_dataInicio" HeaderText="<%$ Resources:Classe, JustificativaAbonoFalta.grvJustificativaFalta.ajf_dataInicio.HeaderText %>" DataFormatString="{0: dd/MM/yyyy}" ItemStyle-Width="10%" />
                            <asp:BoundField DataField="ajf_dataFim" HeaderText="<%$ Resources:Classe, JustificativaAbonoFalta.grvJustificativaFalta.ajf_dataFim.HeaderText %>" DataFormatString="{0: dd/MM/yyyy}" ItemStyle-Width="10%" />
                            <asp:TemplateField HeaderText="<%$ Resources:Classe, JustificativaAbonoFalta.grvJustificativaFalta.ajf_observacao.HeaderText %>" HeaderStyle-CssClass="alinharHeader">
                                <ItemTemplate>
                                    <asp:Label ID="lblObservacao" runat="server" Text='<%# Bind("ajf_observacao") %>' CssClass="wrap600px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Classe, JustificativaAbonoFalta.grvJustificativaFalta.Editar.HeaderText %>" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="70px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnEditar" SkinID="btEditar" runat="server" CommandName="Editar" ToolTip="<%$ Resources:Classe, JustificativaAbonoFalta.grvJustificativaFalta.Editar.HeaderText %>" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Classe, JustificativaAbonoFalta.grvJustificativaFalta.Excluir.HeaderText %>" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="70px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnExcluir" SkinID="btExcluir" runat="server" CommandName="Deletar" ToolTip="<%$ Resources:Classe, JustificativaAbonoFalta.grvJustificativaFalta.Excluir.HeaderText %>" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <div class="right">
                        <asp:Button ID="btnVoltar" runat="server" Text="<%$ Resources:Classe, JustificativaAbonoFalta.btnVoltar.Text %>" CausesValidation="False"
                            OnClick="btnVoltar_Click" />
                    </div>
                    <asp:ObjectDataSource ID="odsJustificativaFalta" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_AlunoJustificativaAbonoFalta"
                        OldValuesParameterFormatString="original_{0}" SelectMethod="SelecionarPorAlunoETurmaDisciplina"
                        TypeName="MSTech.GestaoEscolar.BLL.ACA_AlunoJustificativaAbonoFaltaBO" UpdateMethod="Save">
                        <SelectParameters>
                            <asp:Parameter DbType="Int64" Name="alu_id" />
                            <asp:Parameter DbType="Int64" Name="tud_id" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="divCadastroJustificativaFalta" title="Justificativa de abono de falta" class="hide">
        <asp:UpdatePanel ID="updCadastro" runat="server">
            <ContentTemplate>
                <fieldset>
                    <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios" runat="server" />
                    <asp:Label ID="lblMessageCadastro" runat="server" EnableViewState="false"></asp:Label>
                    <asp:ValidationSummary runat="server" ID="vsJustificativaFalta" ValidationGroup="JustificativaFalta" />
                    <asp:Label ID="lblDataInicio" runat="server" Text="<%$ Resources:Classe, JustificativaAbonoFalta.lblDataInicio.Text %>" 
                        AssociatedControlID="txtDataInicio"></asp:Label>
                    <asp:TextBox ID="txtDataInicio" runat="server" CssClass="maskData" SkinID="Data"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvDataInicio" runat="server" ErrorMessage="<%$ Resources:Classe, JustificativaAbonoFalta.rfvDataInicio.ErrorMessage %>"
                        ControlToValidate="txtDataInicio" ValidationGroup="JustificativaFalta" Display="Dynamic">*</asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="cvDataInicio" runat="server" ControlToValidate="txtDataInicio" 
                        ValidationGroup="JustificativaFalta" Display="Dynamic" ErrorMessage="<%$ Resources:Classe, JustificativaAbonoFalta.cvDataInicio.ErrorMessage %>" 
                        OnServerValidate="ValidarData_ServerValidate">*</asp:CustomValidator>
                    <asp:Label ID="lblDataFim" runat="server" Text="<%$ Resources:Classe, JustificativaAbonoFalta.lblDataFim.Text %>" 
                        AssociatedControlID="txtDataFim"></asp:Label>
                    <asp:TextBox ID="txtDataFim" runat="server" CssClass="maskData" SkinID="Data"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvDataFim" runat="server" ErrorMessage="<%$ Resources:Classe, JustificativaAbonoFalta.rfvDataFim.ErrorMessage %>"
                        ControlToValidate="txtDataFim" ValidationGroup="JustificativaFalta" Display="Dynamic">*</asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="cvDataFim" runat="server" ControlToValidate="txtDataFim" 
                        ValidationGroup="JustificativaFalta" Display="Dynamic" ErrorMessage="<%$ Resources:Classe, JustificativaAbonoFalta.cvDataFim.ErrorMessage %>" 
                        OnServerValidate="ValidarData_ServerValidate">*</asp:CustomValidator>
                    <asp:Label ID="lblObservacao" runat="server" Text="<%$ Resources:Classe, JustificativaAbonoFalta.lblObservacao.Text %>"
                        AssociatedControlID="txtObservacao"></asp:Label>
                    <asp:TextBox ID="txtObservacao" runat="server" TextMode="MultiLine" MaxLength="4000" SkinID="text60c"
                        Text="" CssClass="wrap250px" onkeypress="LimitarCaracter(this,'contadesc3','4000');"
                        onkeyup="LimitarCaracter(this,'contadesc3','4000');"></asp:TextBox>
                    <span id="contadesc3" style="display: inline; font-size: 85%; position: relative; top: -8px;">0/4000</span>
                    <asp:RequiredFieldValidator ID="rfvObservacao" runat="server" ErrorMessage="<%$ Resources:Classe, JustificativaAbonoFalta.rfvObservacao.ErrorMessage %>"
                        ControlToValidate="txtObservacao" ValidationGroup="JustificativaFalta" Display="Dynamic">*</asp:RequiredFieldValidator>
                    <div class="right">
                        <asp:Button ID="btnSalvar" runat="server" Text="<%$ Resources:Classe, JustificativaAbonoFalta.btnSalvar.Text %>" OnClick="btnSalvar_Click" 
                            ValidationGroup="JustificativaFalta" />
                        <asp:Button ID="btnCancelar" runat="server" Text="<%$ Resources:Classe, JustificativaAbonoFalta.btnCancelar.Text %>" CausesValidation="False"
                            OnClick="btnCancelar_Click" OnClientClick="$('#divCadastroJustificativaFalta').dialog('close'); return false;" />
                    </div>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
