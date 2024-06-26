# Stock System

## Sistema de Estoque construido com C#.
Este é um sistema de Estoque construido em CSharp, o mesmo possui as seguintes funções:
  - Adicionar Produto.
  - Remover Produto.
  - Contagem total de produtos.
  - Status do Produto.
  - Incremento e Decremento do Produto.(Unidade)

## Banco de Dados e ADO.NET
Todos as funções do sistema estão ligadas com um banco de dados local, que realiza ações de CRUD o mesmo utiliza o ADO.NET para fazer a conexão e ações no DB.

## Medidas Contra a Injeção de SQL
  - Uso do command.Parametros.AddWithValues
    A medida de proteção contra a injeção de SQL faz com que os parâmetros sejam tratados como dados.
    
## Funções do Sistema
  - Adicionar o Produto: Realiza a adição do produto no DB com base nos dados passados no método construtor sendo eles, NomeProduto, QuantidadeProduto, NomeFornecedor, QuantidadeMinima, fazendo antes uma verificação de existência do produto.

  - Remover Produto: Realiza a remoção do produto com base em seu nome.

  - Contagem Total: Realiza uma contagem total dos produtos em estoque.

  - Status do Produto: Realiza um apanhado geral de informações de um determinado produto, mostrando NOME,NOME FORNECEDO, QUANTIDADE, ademais o mesmo também da um aviso caso
    a Quantidade de Unidades do produto esteja perto ou abaixo do seu limite.

  - Incremento e Decremento: Realizam uma subtração ou adição a unidades de um produto X.
