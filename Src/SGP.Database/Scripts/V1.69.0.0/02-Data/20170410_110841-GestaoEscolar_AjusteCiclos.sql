
	DECLARE @tci_id1Serie INT = (SELECT TOP 1 tci_id FROM ACA_TipoCiclo WITH(NOLOCK) WHERE tci_nome = '1ª série')
	DECLARE @tci_id2Serie INT = (SELECT TOP 1 tci_id FROM ACA_TipoCiclo WITH(NOLOCK) WHERE tci_nome = '2ª série')
	DECLARE @tci_id3Serie INT = (SELECT TOP 1 tci_id FROM ACA_TipoCiclo WITH(NOLOCK) WHERE tci_nome = '3ª série')

	UPDATE ACA_TipoCiclo
	SET tci_situacao = 3
	WHERE tci_id IN (@tci_id1Serie, @tci_id2Serie)

	UPDATE ACA_TipoCiclo
	SET tci_nome = 'Ensino Médio'
	WHERE tci_id = @tci_id3Serie

	UPDATE ACA_CurriculoPeriodo
	SET tci_id = @tci_id3Serie
	WHERE tci_id IN (@tci_id1Serie, @tci_id2Serie)