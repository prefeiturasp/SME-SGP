<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Classe.LancamentoSondagem.Busca" %>

<%@ Register Src="~/WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros" TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao" TagPrefix="uc2" %>  

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <fieldset id="fdsSondagem" runat="server">
        <legend><asp:Label runat="server" ID="lblLegend" Text="<%$ Resources:Classe, LancamentoSondagem.Busca.lblLegend.Text %>" /></legend>
        <div id="divPesquisa" runat="server">            
            <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:Classe, LancamentoSondagem.Busca.lblTitulo.Text %>" AssociatedControlID="txtTitulo"></asp:Label>
            <asp:TextBox ID="txtTitulo" runat="server" SkinID="text60C" MaxLength="200"></asp:TextBox>
        
            <asp:Label runat="server" ID="lblDataInicio" Text="<%$ Resources:Classe, LancamentoSondagem.Busca.lblDataInicio.Text %>" AssociatedControlID="txtDataInicio" />
            <asp:TextBox runat="server" ID="txtDataInicio" SkinID="Data" />
            <asp:CustomValidator ID="cvDataInicio" runat="server" ControlToValidate="txtDataInicio"
                ValidationGroup="periodo" Display="Dynamic" ErrorMessage="<%$ Resources:Classe, LancamentoSondagem.Busca.cvDataInicio.ErrorMessage %>" 
                OnServerValidate="ValidarData_ServerValidate">*</asp:CustomValidator>

            <asp:Label runat="server" ID="lblDataFim" Text="<%$ Resources:Classe, LancamentoSondagem.Busca.lblDataFim.Text %>" AssociatedControlID="txtDataFim" />
            <asp:TextBox runat="server" ID="txtDataFim" SkinID="Data" />
            <asp:CustomValidator ID="cvDataFim" runat="server" ControlToValidate="txtDataFim"
                ValidationGroup="periodo" Display="Dynamic" ErrorMessage="<%$ Resources:Classe, LancamentoSondagem.Busca.cvDataFim.ErrorMessage %>"
                OnServerValidate="ValidarData_ServerValidate">*</asp:CustomValidator>
            
            <asp:Label runat="server" ID="lblSituacao" Text="<%$ Resources:Classe, LancamentoSondagem.Busca.lblSituacao.Text %>" AssociatedControlID="ddlSituacao" />
            <asp:DropDownList ID="ddlSituacao" runat="server">
                <asp:ListItem Text="<%$ Resources:Classe, LancamentoSondagem.Busca.ddlSituacao.Selecione %>" Value="0"></asp:ListItem>
                <asp:ListItem Text="<%$ Resources:Classe, LancamentoSondagem.Busca.ddlSituacao.Vigente %>" Value="1" ></asp:ListItem>
                <asp:ListItem Text="<%$ Resources:Classe, LancamentoSondagem.Busca.ddlSituacao.VigenteComLançamento %>" Value="2" ></asp:ListItem>
                <asp:ListItem Text="<%$ Resources:Classe, LancamentoSondagem.Busca.ddlSituacao.VigenteSemLançamento %>" Value="3" ></asp:ListItem>
                <asp:ListItem Text="<%$ Resources:Classe, LancamentoSondagem.Busca.ddlSituacao.VigenciaEncerrada %>" Value="4" ></asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="right">
            <asp:Button ID="btnPesquisar" runat="server" Text="<%$ Resources:Classe, LancamentoSondagem.Busca.btnPesquisar.Text %>" OnClick="btnPesquisar_Click" />
            <asp:Button ID="btnLimparPesquisa" runat="server" Text="<%$ Resources:Classe, LancamentoSondagem.Busca.btnLimparPesquisa.Text %>" OnClick="btnLimparPesquisa_Click" />
        </div>
    </fieldset>
    <div class="area-form">
        <fieldset id="fdsResultados" runat="server">
            <legend><asp:Label runat="server" ID="lblLegendResultados" Text="<%$ Resources:Classe, LancamentoSondagem.Busca.lblLegendResultados.Text %>" /></legend>
            <uc2:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
            <asp:GridView ID="dgvSondagem" runat="server" DataKeyNames="snd_id,sda_id,situacao" AutoGenerateColumns="False"
                DataSourceID="odsSondagem" AllowPaging="True" OnRowDataBound="dgvSondagem_RowDataBound"
                EmptyDataText="<%$ Resources:Classe, LancamentoSondagem.Busca.dgvSondagem.EmptyDataText %>"
                OnDataBound="dgvSondagem_DataBound" AllowSorting="true" SkinID="GridResponsive">
                <Columns>
                    <asp:TemplateField HeaderText="<%$ Resources:Classe, LancamentoSondagem.Busca.dgvSondagem.HeaderTitulo %>" SortExpression="snd_titulo">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnRespostas" runat="server" CommandName="Select" Text='<%# Bind("snd_titulo") %>'
                                PostBackUrl="~/Classe/LancamentoSondagem/Cadastro.aspx" CssClass="wrap600px"></asp:LinkButton>
                            <asp:Label ID="lblRespostas" runat="server" Text='<%# Bind("snd_titulo") %>' CssClass="wrap600px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="situacaoText" HeaderText="<%$ Resources:Classe, LancamentoSondagem.Busca.dgvSondagem.HeaderSituacao %>" SortExpression="situacaoText" />
                    <asp:TemplateField HeaderText="<%$ Resources:Classe, LancamentoSondagem.Busca.dgvSondagem.HeaderDataInicio %>" SortExpression="sda_dataInicio">
                        <ItemTemplate>
                            <asp:Label ID="lblDataInicio" runat="server" Text='<%# Convert.ToDateTime(Eval("sda_dataInicio")).ToString("dd/MM/yyyy") %>' CssClass="wrap600px"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle CssClass="center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:Classe, LancamentoSondagem.Busca.dgvSondagem.HeaderDataFim %>" SortExpression="sda_dataFim">
                        <ItemTemplate>
                            <asp:Label ID="lblDataFim" runat="server" Text='<%# Convert.ToDateTime(Eval("sda_dataFim")).ToString("dd/MM/yyyy") %>' CssClass="wrap600px"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle CssClass="center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:Classe, LancamentoSondagem.Busca.dgvSondagem.HeaderResponder %>">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnResponder" runat="server" SkinID="btFormulario" 
                                PostBackUrl="~/Classe/LancamentoSondagem/Cadastro.aspx" CommandName="Edit" 
                                ToolTip="<%$ Resources:Classe, LancamentoSondagem.Busca.dgvSondagem.btnResponder.ToolTip %>" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle HorizontalAlign="Center" />
            </asp:GridView>
            <uc1:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="dgvSondagem" />
        </fieldset>
        <asp:ObjectDataSource ID="odsSondagem" runat="server" EnablePaging="True"
            MaximumRowsParameterName="pageSize" SelectCountMethod="GetTotalRecords" StartRowIndexParameterName="currentPage"
            DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_Sondagem" SelectMethod="SelecionaSondagensLancamentoPaginado"
            TypeName="MSTech.GestaoEscolar.BLL.ACA_SondagemBO" OnSelecting="odsSondagem_Selecting">
        </asp:ObjectDataSource>
    </div>
</asp:Content>
