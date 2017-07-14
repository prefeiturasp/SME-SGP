<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Configuracao.Alertas.Cadastro" %>

<%@ PreviousPageType VirtualPath="~/Configuracao/Alertas/Busca.aspx" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios" TagPrefix="uc1" %>
<%@ Register src="~/WebControls/FrequenciaServico/UCFrequenciaServico.ascx" tagname="UCFrequenciaServico" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updServico" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <asp:ValidationSummary ID="vsAlerta" runat="server" ValidationGroup="Alerta" /> 
            <fieldset id="fdsAlertas" runat="server">
                <legend><asp:Literal ID="litTitulo" runat="server" Text='<%$ Resources:GestaoEscolar.Configuracao.Alertas.Cadastro, litTitulo.Text %>'></asp:Literal></legend>
                <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server" />

                <asp:Label ID="lblNome" runat="server" Text="<%$ Resources:GestaoEscolar.Configuracao.Alertas.Cadastro, lblNome.Text %>" AssociatedControlID="txtNome"></asp:Label>
                <asp:TextBox ID="txtNome" runat="server" MaxLength="200" SkinID="text60C"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvNome" runat="server" ControlToValidate="txtNome" Display="Dynamic" ValidationGroup="Alerta" ErrorMessage="<%$ Resources:GestaoEscolar.Configuracao.Alertas.Cadastro, rfvNome.ErrorMessage %>">*</asp:RequiredFieldValidator>

                <asp:Label ID="lblPeriodoAnalise" runat="server" Text="<%$ Resources:GestaoEscolar.Configuracao.Alertas.Cadastro, lblPeriodoAnalise.Text %>" AssociatedControlID="txtPeriodoAnalise"></asp:Label>
                <asp:TextBox ID="txtPeriodoAnalise" runat="server" MaxLength="3" SkinID="Numerico"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPeriodoAnalise" runat="server" ControlToValidate="txtPeriodoAnalise" Display="Dynamic" ValidationGroup="Alerta" ErrorMessage="<%$ Resources:GestaoEscolar.Configuracao.Alertas.Cadastro, rfvPeriodoAnalise.ErrorMessage %>">*</asp:RequiredFieldValidator>

                <asp:Label ID="lblPeriodoValidade" runat="server" Text="<%$ Resources:GestaoEscolar.Configuracao.Alertas.Cadastro, lblPeriodoValidade.Text %>" AssociatedControlID="txtPeriodoValidade"></asp:Label>
                <asp:TextBox ID="txtPeriodoValidade" runat="server" MaxLength="3" SkinID="Numerico"></asp:TextBox>
    
                <asp:Label ID="lblAssunto" runat="server" Text="<%$ Resources:GestaoEscolar.Configuracao.Alertas.Cadastro, lblAssunto.Text %>" AssociatedControlID="txtAssunto"></asp:Label>
                <asp:TextBox ID="txtAssunto" runat="server" TextMode="MultiLine" Height="100"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvAssunto" runat="server" ControlToValidate="txtAssunto" Display="Dynamic" ValidationGroup="Alerta" ErrorMessage="<%$ Resources:GestaoEscolar.Configuracao.Alertas.Cadastro, rfvAssunto.ErrorMessage %>">*</asp:RequiredFieldValidator>

                <br /><br />
                <fieldset>
                    <legend><asp:Literal ID="litGrupos" runat="server" Text='<%$ Resources:GestaoEscolar.Configuracao.Alertas.Cadastro, litGrupos.Text %>'></asp:Literal></legend>
                    <asp:GridView ID="grvGrupos" runat="server" AutoGenerateColumns="false" AllowPaging="false" AllowSorting="false"
                        EmptyDataText="<%$ Resources:GestaoEscolar.Configuracao.Alertas.Cadastro, grvGrupos.EmptyDataText %>" DataKeyNames="gru_id">
                        <Columns>
                            <asp:BoundField HeaderText="<%$ Resources:GestaoEscolar.Configuracao.Alertas.Cadastro, grvGrupos.ColunaNome %>" DataField="gru_nome" />
                            <asp:TemplateField HeaderText="<%$ Resources:GestaoEscolar.Configuracao.Alertas.Cadastro, grvGrupos.ColunaEnvioNotificacao %>" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="150">
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="chkEnvio" Checked='<%# Eval("enviarNotificacao") %>' />
                                </ItemTemplate>
                                <HeaderStyle CssClass="center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </fieldset>

                <fieldset>
                    <legend><asp:Literal ID="litAgendamento" runat="server" Text='<%$ Resources:GestaoEscolar.Configuracao.Alertas.Cadastro, litAgendamento.Text %>'></asp:Literal></legend>
                    <asp:CheckBox ID="chkDesativar" runat="server" Text='<%$ Resources:GestaoEscolar.Configuracao.Alertas.Cadastro, chkDesativar.Text %>' AutoPostBack="true" OnCheckedChanged="chkDesativar_CheckedChanged"/>
                    <uc1:UCFrequenciaServico ID="UCFrequenciaServico1" runat="server" ValidationGroupUCFrequenciaServico="Alerta"/>
                </fieldset>

                <div class="right">
                    <asp:Button ID="btnSalvar" runat="server" Text="<%$ Resources:Padrao, Padrao.Salvar.Text %>" CausesValidation="true" OnClick="btnSalvar_Click" ValidationGroup="Alerta" />
                    <asp:Button ID="btnCancelar" runat="server" Text="<%$ Resources:Padrao, Padrao.Cancelar.Text %>" CausesValidation="false" OnClick="btnCancelar_Click" />
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
