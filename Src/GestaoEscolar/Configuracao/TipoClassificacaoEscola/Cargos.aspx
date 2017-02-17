<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Cargos.aspx.cs" Inherits="Configuracao_TipoClassificacaoEscola_Cargos" %>

<%@ PreviousPageType VirtualPath="~/Configuracao/TipoClassificacaoEscola/Busca.aspx" %>

<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios" TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoVinculo.ascx" TagName="UCComboTipoVinculo" TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/UCComboCargo.ascx" TagName="UCComboCargo" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
    <asp:ValidationSummary ID="vsCargos" runat="server" ValidationGroup="Cargos" />
    <fieldset>
        <legend>Cargos para atribuição esporádica</legend>
        <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios" runat="server" />
        <span>
            <b>
                <asp:Label ID="lblTipoClassificacaoEscola" runat="server" style="font-size:15px" Text="<%$ Resources:Configuracao, TipoClassificacaoEscola.Cargos.lblTipoClassificacaoEscola.Text %>"></asp:Label>
                <asp:Label ID="lblTipoClassificacaoEscolaNome" style="font-size:15px" runat="server" />
            </b></span>        
        <asp:CheckBox ID="ckbPermitirQualquerCargoEscola" runat="server" Text="Permitir qualquer cargo dentro da escola" Enabled="false" />
        <fieldset>
            <asp:GridView ID="gdvCargos" runat="server" AutoGenerateColumns="False" DataKeyNames="tce_id, tcc_id, crg_id"
                PageSize="100" OnRowDataBound="gdvCargos_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="tvi_nome" HeaderText="<%$ Resources:Configuracao, TipoClassificacaoEscola.Cargos.gdvCargos.tvi_nome.HeaderText %>" HeaderStyle-HorizontalAlign="Center"
                        ItemStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                        HeaderText="<%$ Resources:Configuracao, TipoClassificacaoEscola.Cargos.gdvCargos.lblCargo.HeaderText %>">
                        <ItemTemplate>
                            <asp:Label ID="lblCargo" runat="server" Text='<%# Bind("crg_nome")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="tcc_vigenciaInicial" HeaderText="<%$ Resources:Configuracao, TipoClassificacaoEscola.Cargos.gdvCargos.tcc_vigenciaInicial.HeaderText %>" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="center"
                        ItemStyle-HorizontalAlign="Center" DataFormatString="{0:d}">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                     <asp:TemplateField HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="center"
                        HeaderText="<%$ Resources:Configuracao, TipoClassificacaoEscola.Cargos.gdvCargos.lblVigenciaFinal.HeaderText %>">
                        <ItemTemplate>
                            <asp:Label ID="lblVigenciaFinal" runat="server" Text='<%# Bind("tcc_vigenciaFinal")%>' DataFormatString="{0:d}" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </fieldset>
        <div class="right">
            <asp:Button ID="btnCancelar" runat="server" Text="Voltar" CausesValidation="false"
                OnClick="btnCancelar_Click" />
        </div>
    </fieldset>
</asp:Content>
