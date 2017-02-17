<%@ Page Language="C#" MasterPageFile="~/MasterPageAluno.Master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="AreaAluno.Cadastro.CompromissoEstudo.Busca" %>

<%@ Register Src="~/WebControls/Combos/UCComboAnosLetivos.ascx" TagPrefix="uc1" TagName="UCComboAnosLetivos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 

    <uc1:UCComboAnosLetivos runat="server" id="UCComboAnosLetivos" />

    <asp:Label ID="lblMensagem" runat="server" EnableViewState="False"></asp:Label>
    <fieldset id="fdsCompromissoEstudo" runat="server">
        <legend>
            <asp:Label ID="lblLegend" text="<%$ Resources:AreaAluno, Cadastro.CompromissoEstudo.Busca.lblLegend.Text %>" runat="server"/>
        </legend>
        <br />           
        <asp:GridView ID="grvCompromissoEstudo" runat="server" EmptyDataText="A pesquisa não encontrou resultados." 
            AutoGenerateColumns="false" BorderStyle="None" OnRowDataBound="grvCompromissoEstudo_RowDataBound"
            OnRowCommand="grvCompromissoEstudo_RowCommand" DataKeyNames="cpe_id" OnRowEditing="grvCompromissoEstudo_RowEditing">
            <Columns>
                <asp:TemplateField HeaderText="Bimestre" SortExpression="tpc_nome">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnAlterar" runat="server" CommandName="Edit" PostBackUrl="~/Cadastro/CompromissoEstudo/Cadastro.aspx"
                            Text='<%# Bind("tpc_nome") %>' CssClass="wrap200px"></asp:LinkButton>
                        <asp:Label ID="lblBimestre" runat="server" Text='<%# Bind("tpc_nome") %>' CssClass="wrap200px"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="O que tenho feito?" DataField="cpe_atividadeFeita" />
                <asp:BoundField HeaderText="O que pretendo fazer?" DataField="cpe_atividadePretendeFazer" />
                <asp:TemplateField HeaderText="Excluir">                   
                    <ItemTemplate>
                        <asp:ImageButton ID="btnExcluir" runat="server" CausesValidation="False" CommandName="Deletar"
                            SkinID="btExcluir" />
                    </ItemTemplate>
                    <HeaderStyle CssClass="center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <br />
        <div class="right">
            <asp:Button ID="btnNovo" runat="server" Text="<%$ Resources:AreaAluno, Cadastro.CompromissoEstudo.Busca.btnNovo.Text %>" OnClick="btnNovo_Click" />            
        </div>
    </fieldset>
</asp:Content>