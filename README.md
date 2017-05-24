# Repositório de Histórico no GitLab:
* https://gitlab.com/smecotic/sgp

# Utilização

* [Roteiro de uso](https://gitlab.com/smecotic/sgp/wikis/roteiro-de-uso)

# Configuração

**Requisitos de instalação**

* Microsoft .NET Framework 4.5 SDK (4.5.1, 4.5.2)
* Microsoft SQL Server 2008 R2 ou superior
* Microsoft SQL Server Reporting Services 2008R2, 2012 ou 2014
* Microsoft Visual Studio 2013 ou superior
 
**Bibliotecass utlizadas**
* .NET 4.5
* AspNet MVC 4.0
* AspNet WebApi
* Newtonsoft.Json 8.0.2
* Quartz 2.4.1
* jQuery 1.8.2
* DevExpress 16.1


**Clonar o projeto através do repositório do github.com e dar checkout no branch develop**
* git clone https://gitlab.com/smecotic/sgp.git
* cd sgp
* git pull

**Restaurar os pacotes NuGets da solução**
* Clicar com o direito na solução e selecionar Restore NuGet Packages
* Verificar se o build está executando com sucesso

**Criar a base de dados GestaoPedagogica e usuários através dos scripts da pasta \sgp\scripts\install\GestaoPedagogica**
* 01-GestaoPedagogica_Cria_Database.sql
* 02-GestaoPedagogica_Cria_usuários_SQL.sql

**Dar permissão para os usuários**
* script sgp\scripts\install\UserPermission\GestaoPedagogica_Grant.sql (remover EXEC dos bancos não utilizados) 

**Baixar aplicativo gerenciador de conexões dos bancos de dados MSTech.Config.exe**
Abrir a pasta sgp\scripts\install\configs, copiar os arquivos para as pastas:
* sgp\Src\GestaoEscolar\bin
* sgp\Src\GestaoAcademica.WebApi\bin
* sgp\Src\AreaAluno\bin

Abrir o executável MSTech.Config.exe das pastas "bin" acima e configurar os campos conforme tabela:

	Connection name (nome da conexão - seguir tabela acima)
	Server name (nome da instância)
	Database name (nome do banco)
	Tipo de autenticação
	User name (usuário do banco de dados)
	Password (senha do usuário do banco de dados)

<table>
<tr>
	<th>Connection Names</th>
	<th>Usuário do banco de dados</th>
</tr>
<tr>
	<td>CoreSSO</td>
	<td>user_coresso</td>
</tr>
<tr>
	<td>MSTech.Log</td>
	<td>user_coresso</td>
</tr>
<tr>
	<td>GestaoPedagogica</td>
	<td>user_gestaoescolar</td>
</tr>
</table>

Clicar em testar conexão e salvar


