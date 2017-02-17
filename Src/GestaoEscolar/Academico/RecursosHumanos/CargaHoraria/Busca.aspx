<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Academico_RecursosHumanos_CargaHoraria_Busca" Title="Untitled Page" CodeBehind="Busca.aspx.cs" %>

<%@ Register Src="../../../WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc2" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <fieldset id="fdsCargaHoraria" runat="server">
        <legend>Consulta de carga horária</legend>
        <div id="_divPesquisa" runat="server">
            <asp:Label ID="_lblDescricaoCargaHoraria" runat="server" Text="Descrição da carga horária" AssociatedControlID="_txtDescricaoCargaHoraria"></asp:Label>
            <asp:TextBox ID="_txtDescricaoCargaHoraria" runat="server" SkinID="text60C" 
                MaxLength="200"></asp:TextBox>
        </div>
        <div class="right">
            <asp:Button ID="_btnPesquisar" runat="server" Text="Pesquisar" OnClick="_btnPesquisar_Click" />
            <asp:Button ID="_btnLimparPesquisa" runat="server" Text="Limpar pesquisa" OnClick="_btnLimparPesquisa_Click" />
    </fieldset>
    <fieldset id="fdsResultados" runat="server">
        <legend>Resultados</legend>
        <uc2:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
        <asp:GridView ID="_dgvCargaHoraria" runat="server" AutoGenerateColumns="False" AllowPaging="True"
            DataKeyNames="chr_id" DataSourceID="_odsCargaHoraria" EmptyDataText="A pesquisa não encontrou resultados."
            OnDataBound="_dgvCargaHoraria_DataBound" AllowSorting="true" >
            <Columns>    
                <asp:BoundField DataField="chr_descricao" HeaderText="Descrição da carga horária" SortExpression="chr_descricao" />                          
                <asp:BoundField DataField="chr_especialista" HeaderText="Tipo do cargo" SortExpression="chr_especialista" />                
                <asp:BoundField DataField="crg_descricao" HeaderText="Cargo" SortExpression="crg_descricao" />                
                <asp:BoundField DataField="chr_cargaHorariaSemanal" HeaderText="Carga horária semanal" SortExpression="chr_cargaHorariaSemanal" />     
                <asp:TemplateField HeaderText="Visualizar" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="130px">
                    <ItemTemplate>   
                        <asp:ImageButton ID="_btnAlterar" runat="server" CommandName="Edit" SkinID="btDetalhar"
                          PostBackUrl="~/Academico/RecursosHumanos/CargaHoraria/Cadastro.aspx" ToolTip="Visualizar" ></asp:ImageButton>                       
                    </ItemTemplate>
                    <HeaderStyle CssClass="center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>                           
            </Columns>
        </asp:GridView>
        <uc1:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="_dgvCargaHoraria" />
    </fieldset>
    <asp:ObjectDataSource ID="_odsCargaHoraria" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.RHU_CargaHoraria"
         SelectMethod="SelecionaCargaHoraria" TypeName="MSTech.GestaoEscolar.BLL.RHU_CargaHorariaBO"></asp:ObjectDataSource>
</asp:Content>
