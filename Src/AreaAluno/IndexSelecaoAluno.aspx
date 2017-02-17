<%@ Page Language="C#" MasterPageFile="~/MasterPagePaginaInicial.Master" AutoEventWireup="true" CodeBehind="IndexSelecaoAluno.aspx.cs" Inherits="AreaAluno.IndexSelecaoAluno" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="margin-top:10px;"></div>
    <asp:Label ID="lblMensagem" runat="server" EnableViewState="False"></asp:Label>
    <fieldset id="Fieldset1" runat="server" >
        <legend></legend>
        <div style="border-width:0px; width:100%; margin-top:30px;">
            <asp:GridView ID="grvAluno" runat="server" DataKeyNames="alu_id, pes_idAluno" AutoGenerateColumns="false"
                OnRowDataBound="grvAluno_RowDataBound" OnRowCommand="grvAluno_RowCommand">
                <Columns>
                    <asp:TemplateField HeaderText="Seleção de aluno">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnAluno" runat="server" CommandName="Encaminhar" Text='<%# Bind("pes_nome") %>'></asp:LinkButton>
                        </ItemTemplate>
                        <HeaderStyle CssClass="center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </fieldset>
</asp:Content>