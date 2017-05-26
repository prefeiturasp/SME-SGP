USE [GestaoPedagogica]
GO

DECLARE @cal_ano INT = 2017
DECLARE @tds_nome VARCHAR(100) = 'Matemática'
DECLARE @nivelEnsino VARCHAR(100) = NULL

DECLARE @tbObjetos TABLE
(
	ordemEixo INT
	, ordemSubEixo INT
	, ordemObjeto INT
	, descricaoEixo VARCHAR(500)
	, descricaoSubEixo VARCHAR(500)
	, descricaoObjeto VARCHAR(500)
)

DECLARE @ordemEixo INT = 1
DECLARE @ordemSubEixo INT = 1
DECLARE @ordemObjeto INT = 0
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Estratégias de cálculo mental')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Arredondamento e aproximação')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Cálculo aproximado e estimativa')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Divisores de um número')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'História da Matemática')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Juros simples')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Máximo divisor comum de dois ou mais números')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Mínimo múltiplo comum de dois ou mais números')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Múltiplos de um número')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Números inteiros na reta numérica')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Números inteiros: conceito')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Números inteiros: operações básicas')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Números irracionais na reta numérica')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Números irracionais: conceito')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Números naturais: conceito')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Números naturais: operações básicas')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Números naturais: reta numérica')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Números racionais expressos na forma decimal: operações básicas')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Números racionais expressos na forma fracionária: operações básicas')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Números racionais na reta numérica')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Números racionais: conceito')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Números reais na reta numérica')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Números reais: conceito')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Números reais: operações básicas')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Porcentagens básicas (10%; 25%; 50%; 75%; 100%) utilizando estratégias não-convencionais')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Porcentagens diversas por meio de cálculo convencional')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Potenciação envolvendo números inteiros')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Potenciação envolvendo números racionais')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Potenciação envolvendo números reais')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Potenciação envolvendo números naturais')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Potenciação: conceito')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Potenciação: propriedades')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Proporcionalidade: Cálculo de proporção fazendo uso da "regra de três"')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Proporcionalidade: Cálculo de proporção sem fazer uso da "regra de três"')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Proporcionalidade: escala')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Proporcionalidade: grandezas diretamente proporcionais')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Proporcionalidade: grandezas inversamente proporcionais ')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Proporcionalidade: Razão')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Racionalização')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Radiciação envolvendo números inteiros')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Radiciação envolvendo números naturais')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Radiciação envolvendo números racionais')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Radiciação envolvendo números reais')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Radiciação: conceito')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Radiciação: propriedades')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Sequência numérica crescente e decrescente')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Sistema de numeração decimal: arredondamento e estimativas')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Sistema de numeração decimal: classes e ordens')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Sistema de numeração decimal: composição e decomposição')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Sistema de numeração decimal: contagem')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Sistema de numeração decimal: escrita por extenso')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Sistema de numeração decimal: notação científica')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Sistema de numeração decimal: subunidades decimais')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Sistema de numeração decimal: valor posicional')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Sistema monetário brasileiro')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Sistema romano de numeração')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Princípio multiplicativo')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Números e operações', NULL, 'Leitura e produção de gêneros matemáticos (enunciado de problema, resolução de problema, etc)')

SET @ordemEixo = @ordemEixo + 1;
SET @ordemSubEixo = 1;
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Álgebra', NULL, 'Equação biquadrada')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Álgebra', NULL, 'Equação do 1° grau')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Álgebra', NULL, 'Equação do 2° grau completa')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Álgebra', NULL, 'Equação do 2° grau incompleta')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Álgebra', NULL, 'Expressões algébricas: fatoração')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Álgebra', NULL, 'Expressões algébricas: interpretação e produção')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Álgebra', NULL, 'Expressões algébricas: operações com polinômios')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Álgebra', NULL, 'Expressões algébricas: termo algébrico')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Álgebra', NULL, 'Expressões algébricas: valor numérico')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Álgebra', NULL, 'Função do primeiro grau: conceito')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Álgebra', NULL, 'Função do primeiro grau: gráfico')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Álgebra', NULL, 'Inequações do primeiro grau')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Álgebra', NULL, 'Produtos notáveis: produto da soma pela diferença de dois termos')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Álgebra', NULL, 'Produtos notáveis: quadrado da diferença de dois termos')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Álgebra', NULL, 'Produtos notáveis: quadrado da soma de dois termos')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Álgebra', NULL, 'Sistema de equações do do primeiro grau: resolução pela adição')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Álgebra', NULL, 'Sistema de equações do primeiro grau: representação geométrica e analítica')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Álgebra', NULL, 'Generalização de Padrões')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Álgebra', NULL, 'Sistema de equações do primeiro grau: resolução por substituição')

SET @ordemEixo = @ordemEixo + 1;
SET @ordemSubEixo = 1;
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geometria', NULL, 'Ângulo: conceito')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geometria', NULL, 'Ângulos complementares')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geometria', NULL, 'Ângulos em feixes de retas paralelas cortadas por retas transversais')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geometria', NULL, 'Ângulos opostos pelo vértice')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geometria', NULL, 'Ângulos suplementares')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geometria', NULL, 'Figuras não planas: propriedades geométricas (relações diversas)')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geometria', NULL, 'Figuras planas: ampliação e redução')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geometria', NULL, 'Figuras planas: congruência')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geometria', NULL, 'Figuras planas: propriedades (paralelismo, eixos de simetria, relações entre lados e ângulos)')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geometria', NULL, 'Figuras planas: semelhança')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geometria', NULL, 'Figuras tridimensionais (cilindro, cone, esfera, prisma, pirâmide)')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geometria', NULL, 'Figuras tridimensionais: planificação')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geometria', NULL, 'Figuras tridimensionais (vistas: lateral direita e esquerda, frontal e superior)')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geometria', NULL, 'Localização em croquis')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geometria', NULL, 'Localização em mapas')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geometria', NULL, 'Localização utilizando o princípio cartesiano')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geometria', NULL, 'Localização utilizando pontos de referência')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geometria', NULL, 'Lugar geométrico: mediatriz')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geometria', NULL, 'Lugar geométrico: bissetriz')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geometria', NULL, 'Plano cartesiano: pares ordenados')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geometria', NULL, 'Plano cartesiano: pares ordenados em todos os quadrantes')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geometria', NULL, 'Poliedros: definição')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geometria', NULL, 'Poliedros: face, vértice e aresta')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geometria', NULL, 'Polígonos: classificação de quadriláteros')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geometria', NULL, 'Polígonos: classificação de triângulos')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geometria', NULL, 'Polígonos: definição')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geometria', NULL, 'Polígonos: diagonais')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geometria', NULL, 'Polígonos: soma das medidas dos ângulos internos de um polígono')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geometria', NULL, 'Teorema de Pitágoras')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geometria', NULL, 'Teorema de Tales: aplicação')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geometria', NULL, 'Teorema de Tales: conceito')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geometria', NULL, 'Transformação geométrica: reflexão')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geometria', NULL, 'Transformação geométrica: rotação')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geometria', NULL, 'Transformação geométrica: translação')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geometria', NULL, 'Triângulo retângulo: Razões trigonométricas')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geometria', NULL, 'Triângulo retângulo: Relações métricas')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Geometria', NULL, 'Geometria não euclidiana')

SET @ordemEixo = @ordemEixo + 1;
SET @ordemSubEixo = 1;
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Grandezas e medidas', NULL, 'Área de figuras poligonais')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Grandezas e medidas', NULL, 'Área do círculo')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Grandezas e medidas', NULL, 'Área: conceito')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Grandezas e medidas', NULL, 'Comprimento de circunferência')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Grandezas e medidas', NULL, 'História da Matemática')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Grandezas e medidas', NULL, 'Instrumentos de medida: leitura em balança eletrônica ou mecânica')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Grandezas e medidas', NULL, 'Instrumentos de medida: leitura em régua')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Grandezas e medidas', NULL, 'Instrumentos de medida: leitura em relógio analógico')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Grandezas e medidas', NULL, 'Instrumentos de medida: leitura em relógio digital')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Grandezas e medidas', NULL, 'Instrumentos de medida: leitura em transferidor')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Grandezas e medidas', NULL, 'Medida de comprimento: medidas convencionais e não convencionais')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Grandezas e medidas', NULL, 'Medida de temperatura')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Grandezas e medidas', NULL, 'Medidas de capacidade: medidas convencionais e não convencionais')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Grandezas e medidas', NULL, 'Medidas de massa: medidas convencionais')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Grandezas e medidas', NULL, 'Medidas de tempo: duração de eventos')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Grandezas e medidas', NULL, 'Medidas de tempo: leitura em calendário')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Grandezas e medidas', NULL, 'Medidas de tempo: medidas convencionais')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Grandezas e medidas', NULL, 'Medidas: conversão')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Grandezas e medidas', NULL, 'Perímetro de figuras poligonais')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Grandezas e medidas', NULL, 'Perímetro: conceito')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Grandezas e medidas', NULL, 'Volume de paralelepípedo')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Grandezas e medidas', NULL, 'Volume: conceito')

SET @ordemEixo = @ordemEixo + 1;
SET @ordemSubEixo = 1;
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Probabilidade e estatística', NULL, 'Frequência')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Probabilidade e estatística', NULL, 'Gráfico de Barras')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Probabilidade e estatística', NULL, 'Gráfico de Colunas')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Probabilidade e estatística', NULL, 'Gráfico de Linhas')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Probabilidade e estatística', NULL, 'Gráfico de Setores')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Probabilidade e estatística', NULL, 'Média aritmética ponderada')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Probabilidade e estatística', NULL, 'Média aritmética simples')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Probabilidade e estatística', NULL, 'Mediana')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Probabilidade e estatística', NULL, 'Moda')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Probabilidade e estatística', NULL, 'Probabilidade')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Probabilidade e estatística', NULL, 'Tabela de dupla entrada')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Probabilidade e estatística', NULL, 'Tabela simples')
SET @ordemObjeto = @ordemObjeto + 1; INSERT INTO @tbObjetos VALUES (@ordemEixo, @ordemSubEixo, @ordemObjeto, 'Probabilidade e estatística', NULL, 'Não neutralidade da Matemática')

-- Encontra a disciplina
DECLARE @tbDisciplina TABLE
(
	tds_id INT
)
IF (@nivelEnsino IS NULL)
BEGIN
	INSERT INTO @tbDisciplina
	SELECT tds.tds_id
	FROM ACA_TipoDisciplina tds WITH(NOLOCK)
	WHERE tds.tds_nome = @tds_nome AND tds.tds_situacao <> 3
END
ELSE 
BEGIN
	INSERT INTO @tbDisciplina
	SELECT tds.tds_id
	FROM ACA_TipoDisciplina tds WITH(NOLOCK)
	INNER JOIN ACA_TipoNivelEnsino tne WITH(NOLOCK)
		ON tne.tne_id = tds.tne_id
		AND tne.tne_nome = @nivelEnsino
		AND tne.tne_situacao <> 3
	WHERE tds.tds_nome = @tds_nome AND tds.tds_situacao <> 3
END

DECLARE @tds_id INT
WHILE (EXISTS (SELECT TOP 1 1 FROM @tbDisciplina))
BEGIN
	
	SELECT TOP 1 @tds_id = tds_id
	FROM @tbDisciplina

	DECLARE @dataAtual DATETIME = GETDATE();

	/*
	-- Apaga os registros existentes
	UPDATE ACA_ObjetoAprendizagem
	SET oap_situacao = 3, oap_dataAlteracao = @dataAtual
	WHERE tds_id = @tds_id AND cal_ano = @cal_ano AND oap_situacao <> 3

	UPDATE ACA_ObjetoAprendizagemEixo
	SET oae_situacao = 3, oae_dataAlteracao = @dataAtual
	WHERE tds_id = @tds_id AND cal_ano = @cal_ano AND oae_situacao <> 3
	--
	*/
	
	-- Verifica se os objetos de aprendizagem ainda não foram cadastrados
	-- para a disciplina e ano.
	IF (NOT EXISTS (
		SELECT TOP 1 1 
		FROM ACA_ObjetoAprendizagem WITH(NOLOCK) 
		WHERE tds_id = @tds_id AND cal_ano = @cal_ano AND oap_situacao <> 3
		))
	BEGIN
		--Iniciar transação
		BEGIN TRANSACTION
		SET XACT_ABORT ON

			-- Cadastra os eixos
			INSERT INTO ACA_ObjetoAprendizagemEixo (tds_id, oae_descricao, oae_ordem, oae_situacao, oae_dataCriacao, oae_dataAlteracao, cal_ano)
			SELECT 
				@tds_id
				, descricaoEixo
				, ordemEixo
				, 1
				, @dataAtual
				, @dataAtual
				, @cal_ano
			FROM @tbObjetos
			WHERE descricaoEixo IS NOT NULL
			GROUP BY ordemEixo, descricaoEixo
			ORDER BY ordemEixo

			-- Cadastra os subeixos
			;WITH Subeixos AS
			(
				SELECT ordemEixo, descricaoEixo, ordemSubEixo, descricaoSubEixo
				FROM @tbObjetos
				WHERE descricaoSubEixo IS NOT NULL
				GROUP BY ordemEixo, descricaoEixo, ordemSubEixo, descricaoSubEixo
			)
			INSERT INTO ACA_ObjetoAprendizagemEixo (tds_id, oae_descricao, oae_ordem, oae_idPai, oae_situacao, oae_dataCriacao, oae_dataAlteracao, cal_ano)
			SELECT 
				@tds_id
				, descricaoSubEixo
				, ordemSubEixo
				, CASE WHEN descricaoEixo IS NOT NULL THEN
					(
						SELECT TOP 1 oae_id
						FROM ACA_ObjetoAprendizagemEixo
						WHERE 
						tds_id = @tds_id 
						AND cal_ano = @cal_ano
						AND oae_descricao = descricaoEixo
						AND oae_ordem = ordemEixo
						AND oae_situacao = 1
					)
					ELSE NULL END
				, 1
				, @dataAtual
				, @dataAtual
				, @cal_ano
			FROM Subeixos
			ORDER BY ordemEixo, ordemSubEixo

			-- Cadastra os objetos
			INSERT INTO ACA_ObjetoAprendizagem (tds_id, oap_descricao, oap_situacao, oap_dataCriacao, oap_dataAlteracao, cal_ano, oae_id)
			SELECT 
				@tds_id
				, descricaoObjeto
				, 1
				, @dataAtual
				, @dataAtual
				, @cal_ano
				, CASE WHEN descricaoSubEixo IS NOT NULL THEN
					(
						SELECT TOP 1 oae_id
						FROM ACA_ObjetoAprendizagemEixo
						WHERE 
						tds_id = @tds_id 
						AND cal_ano = @cal_ano
						AND oae_descricao = descricaoSubEixo
						AND oae_ordem = ordemSubEixo
						AND oae_situacao = 1
					)
					ELSE
						CASE WHEN descricaoEixo IS NOT NULL THEN
						(
							SELECT TOP 1 oae_id
							FROM ACA_ObjetoAprendizagemEixo
							WHERE 
							tds_id = @tds_id 
							AND cal_ano = @cal_ano
							AND oae_descricao = descricaoEixo
							AND oae_ordem = ordemEixo
							AND oae_situacao = 1
						)
						ELSE NULL END 			
					END
			FROM @tbObjetos
			WHERE descricaoObjeto IS NOT NULL
			ORDER BY ordemObjeto

			-- Cadastra os tipos de ciclo dos objetos
			INSERT INTO ACA_ObjetoAprendizagemTipoCiclo (oap_id, tci_id)
			SELECT oap.oap_id, tci.tci_id
			FROM @tbObjetos obj
			INNER JOIN ACA_ObjetoAprendizagem oap
				ON oap.tds_id = @tds_id 
				AND oap.cal_ano = @cal_ano
				AND oap.oap_descricao = obj.descricaoObjeto
				AND oap.oap_situacao = 1
				AND 
				(
					(oap.oae_id IS NULL AND obj.descricaoSubEixo IS NULL AND obj.descricaoEixo IS NULL)
					OR 
					(
						obj.descricaoSubEixo IS NOT NULL
						AND oap.oae_id = 
						(
							SELECT TOP 1 oae_id
							FROM ACA_ObjetoAprendizagemEixo
							WHERE 
							tds_id = @tds_id 
							AND cal_ano = @cal_ano
							AND oae_descricao = obj.descricaoSubEixo
							AND oae_ordem = obj.ordemSubEixo
							AND oae_situacao = 1 
						)
					)
					OR 
					(
						obj.descricaoEixo IS NOT NULL
						AND oap.oae_id = 
						(
							SELECT TOP 1 oae_id
							FROM ACA_ObjetoAprendizagemEixo
							WHERE 
							tds_id = @tds_id 
							AND cal_ano = @cal_ano
							AND oae_descricao = obj.descricaoEixo
							AND oae_ordem = obj.ordemEixo
							AND oae_situacao = 1 
						)
					)
				)
			LEFT JOIN ACA_TipoCiclo tci WITH(NOLOCK) 
				ON tci.tci_situacao = 1

		-- Fechar transação	
		SET XACT_ABORT OFF
		COMMIT TRANSACTION
	END	
	ELSE
	BEGIN
		PRINT 'Já existem objetos de conhecimento cadastrados para a disciplina.'
	END 
	
	DELETE TOP(1)
	FROM @tbDisciplina
END