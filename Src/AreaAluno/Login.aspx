<%@ Page Language="C#" MasterPageFile="~/MasterPageAreaAberta.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="AreaAluno.Login" %>
<%@ Register Src="WebControls/Combos/UCComboEntidade.ascx" TagName="UCComboEntidade" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div >
        <fieldset id="fdsLogin" runat="server">
            <div id="msgLogin">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </fieldset>
    </div>
    <style>
        #acessibilidade {
            display: none;
        }
    </style>
</asp:Content>

