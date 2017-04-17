USE [GestaoPedagogica]
GO

BEGIN TRANSACTION 
SET XACT_ABORT ON

	UPDATE RES_ChaveResource
	SET rcr_valor = 'Objetos de conhecimento'
	WHERE rcr_chave = 'UCPlanejamentoProjetos.litObjetoAprendizagem.Text'

	UPDATE RES_ChaveResource
	SET rcr_valor = 'Objetos de conhecimento'
	WHERE rcr_chave = 'ControleTurma.DiarioClasse.lblLgdObjAprendizagem.Text'

	UPDATE RES_ChaveResource
	SET rcr_valor = 'Ciclos que possuem turmas com registros ligados ao objeto de conhecimento não podem ser removidos.'
	WHERE rcr_chave = 'ObjetoAprendizagem.Cadastro.lblMessageCiclos.Text'

	UPDATE RES_ChaveResource
	SET rcr_valor = '<p>Prezado(a) professor(a),</p><br><p align="justify">A Secretaria Municipal de Educa&ccedil;&atilde;o (SME) realizar&aacute;, na segunda quinzena de junho, a primeira avalia&ccedil;&atilde;o semestral em nossa rede. Nela estar&atilde;o contempladas as disciplinas: Arte, Ci&ecirc;ncias, Educa&ccedil;&atilde;o F&iacute;sica, Geografia, Hist&oacute;ria, L&iacute;ngua Inglesa, L&iacute;ngua Portuguesa e Matem&aacute;tica.</p><br><p align="justify">A fim de proporcionar uma avalia&ccedil;&atilde;o que colabore com o trabalho docente, convidamos todos os professores para participar de uma pesquisa dispon&iacute;vel aqui no SGP que indicar&aacute; quais objetos de conhecimento s&atilde;o trabalhados nos bimestres de cada ano de escolariza&ccedil;&atilde;o.</p><br><p align="justify">Essas informa&ccedil;&otilde;es s&atilde;o muito importantes para que sejam elaboradas avalia&ccedil;&otilde;es significativas para nossos estudantes e para voc&ecirc; professor.</p><br><p align="justify"><br /> </p><br><p align="justify">Agradecemos a sua participa&ccedil;&atilde;o.</p>'
	WHERE rcr_chave = 'UCPlanejamentoProjetos.lblAvisoObjetosAprendizagem.Text'

	UPDATE RES_ChaveResource
	SET rcr_valor = 'Informações sobre os objetos de conhecimento.'
	WHERE rcr_chave = 'UCPlanejamentoProjetos.btnAjudaObjetos.ToolTip'

	UPDATE RES_ChaveResource
	SET rcr_valor = 'Objetos de conhecimento'
	WHERE rcr_chave = 'ObjetoAprendizagem.Busca.lblLegend.Text'

	UPDATE RES_ChaveResource
	SET rcr_valor = 'Não há ciclos ligados a objetos de conhecimento para os filtros selecionados.'
	WHERE rcr_chave = 'ObjetoAprendizagem.Busca.lblMessageCiclo.Text'

-- Fechar transação     
SET XACT_ABORT OFF 
COMMIT TRANSACTION
