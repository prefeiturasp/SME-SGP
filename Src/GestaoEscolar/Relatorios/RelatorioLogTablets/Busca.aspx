<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    Inherits="Busca" Codebehind="Busca.aspx.cs" %>
<%@ Register Src="~/WebControls/Mensagens/UCLoader.ascx" TagName="UCLoader" 
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagName="UCComboUAEscola" 
    TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios" 
    TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao" 
    TagPrefix="uc4" %>
<%@ Register Src="~/WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros" 
    TagPrefix="uc5" %>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:UCLoader ID="UCLoader1" runat="server" />
    <asp:Label ID="lblMensagemErro" runat="server" EnableViewState="False"></asp:Label>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Group1" />

    <asp:UpdatePanel ID="updPequisa" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset class="msgInfo">
                <legend>Consulta da versão do aplicativo SGP tablet</legend>
                <uc3:UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server" EnableViewState="False" />
                <uc2:UCComboUAEscola ID="uccUaEscola" runat="server" CarregarEscolaAutomatico="true"
                MostrarMessageSelecioneEscola="true" AsteriscoObg="true"
                ObrigatorioUA="true" MostrarMessageSelecioneUA="true" ValidationGroup="Group1" />
                <br />
                <div class="right">
                    <asp:Button ID="btn_pesquisar" runat="server" Text="Pesquisar"
                        OnClick="btnPesquisar_Click" ValidationGroup="Group1"/>
                </div>
            </fieldset>
            <fieldset id="fdsResultados" runat="server" visible="false">
                <legend>Resultados</legend>
                <uc4:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
                <asp:GridView ID="grvConsultaLogTablets" runat="server" AutoGenerateColumns="False"
                    DataSourceID="odsConsultaLogTablets" AllowPaging="True" BorderStyle="None" AllowSorting="true"
                    EmptyDataText="A pesquisa não encontrou resultados." OnDataBound="grvConsultaLogTablets_DataBound">
                    <Columns>                        
                        <asp:BoundField DataField="diretoria" HeaderText="Diretoria regional de ensino" />
                        <asp:BoundField DataField="esc_nome" HeaderText="Escola" SortExpression="esc_nome" />                        
                        <asp:BoundField DataField="equ_identificador" HeaderText="Nº Serie" SortExpression="equ_identificador" 
                           ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="center" />
                        <asp:BoundField DataField="equ_dataAlteracao" HeaderText="Data de envio" SortExpression="equ_dataAlteracao" 
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-CssClass="center" />
                        <asp:BoundField DataField="equ_appVersion" HeaderText="Versão APP" SortExpression="equ_appVersion" 
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-CssClass="center" />
                        <asp:BoundField DataField="equ_soVersion" HeaderText="Versão SO" SortExpression="equ_soVersion" 
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-CssClass="center" />
                    </Columns>
                </asp:GridView>
                <uc5:uctotalregistros id="UCTotalRegistros1" runat="server" associatedgridviewid="grvConsultaLogTablets" />
                <asp:ObjectDataSource ID="odsConsultaLogTablets" runat="server"
                    SelectMethod="SelectLogTabletEquipamento"
                    TypeName="MSTech.GestaoEscolar.BLL.SYS_EquipamentoBO"></asp:ObjectDataSource>
                <div class="right divBtnExportar">
                    <asp:Button ID="_btnExportar" runat="server" Text="Exportar Excel" OnClick="_btnExportar_Click" />
                </div>
            </fieldset>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="_btnExportar"/>
        </Triggers>
    </asp:UpdatePanel>
                    
</asp:Content>



