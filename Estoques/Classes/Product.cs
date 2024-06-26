using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace Estoques
{
    internal class Product : IStock
    {
        /* NOTE:
         * DADOS DO PRODUTOS SENDO ELES, NOME DO PRODUTO,
         * QUANTIDADE, NOME DO FORNECEDOR , QUANTIDADE MÍNIMA RESTANTE.
         * FAZENDO A CAPTURA DOS MESMOS ATRAVES DO MÉTODO CONSTRUTOR POR GET E SET.
         */

        /*
         * 
         * 
         */
        private string _ProdutoNome { get; set; }
        private int _QuantidadeProduto { get; set; }
        private string _NomeFornecedor { get; set; }
        private int _QuantidadeMinimaRestantes { get; set; }

        //MÉTODO CONSTRUTOR
        public Product(string ProdutoNome, int QuantidadeProduto, string NomeFornecedor, int QuantidadeMinima)
        {
            _ProdutoNome = ProdutoNome;
            _QuantidadeProduto = QuantidadeProduto;
            _NomeFornecedor = NomeFornecedor;
            _QuantidadeMinimaRestantes = QuantidadeMinima;
        }

        //DADOS PARA ACESSAR O DB LOCAL SERVIDOR,DB,SUPERUSUARIO,PASSWORD. (WHIT PROTECTED)
        protected string DataForAccessDataBase()
        {
            string Passaport = "Server=DESKTOP-E2TAI3D\\SQLEXPRESS; DataBase=Estoque; User=sa; Password=admin04;";
            return Passaport;
        }


        /*NOTE:
         * FUNÇÃO DE ADICIONAR PRODUTO NO DB 
         * 
         * CONEXÃO  ABERTA 
         *  CRIAÇÃO DE COMANDO
         * 
         * CMD.PARAMETRO.LIMPAR -> LIMPA OS PARAMETROS DO DB ANTES DE EXECUTAR O CMD.
         */
        public void AddProduto()
        {
            using (SqlConnection connect = new SqlConnection(DataForAccessDataBase()))
            {
                connect.Open();

                using (SqlCommand cmd = connect.CreateCommand())
                {
                    cmd.CommandText = "SELECT (1) FROM Stock WHERE NomeProduto = @ProdutoNome";
                    cmd.Parameters.AddWithValue("@ProdutoNome", _ProdutoNome);
                    object resultado = cmd.ExecuteScalar();
                    Console.WriteLine("Verificação feita com sucesso");

                    if (Convert.ToInt32(resultado) > 0)
                    {
                        Console.WriteLine($"Não é possível adicionar. Já existe um produto de nome {_ProdutoNome}");
                    }
                    else
                    {

                        Console.WriteLine("Preparando para adicionar o produto.");

                        try
                        {
                            cmd.Parameters.Clear();

                            cmd.CommandText = $"INSERT INTO Stock (NomeProduto, QuantidadeProduto, FornecedorProduto, QuantidadeMinima) VALUES(@NomeProduto, @QuantidadeProduto, @FornecedorProduto, @QuantidadeMinima);";
                            cmd.Parameters.Add("@NomeProduto", _ProdutoNome);
                            cmd.Parameters.Add("@QuantidadeProduto", _QuantidadeProduto);
                            cmd.Parameters.Add("@FornecedorProduto", _NomeFornecedor);
                            cmd.Parameters.Add("@QuantidadeMinima", _QuantidadeMinimaRestantes);
                            cmd.ExecuteNonQuery();

                            Console.WriteLine("Produto adicionado.");

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[ERRO] Tente novamente mais tarde.{ex.Message}");
                        }

                    }
                }
            }
        }

        /*NOTE:
         * REMOÇÃO DE PRODUTO DO DATABASE;
         * 
         * VARIÁVEL RESPOSTA REFERE-SE A PERGUNTA DE REMOÇÃO.
         * 
         * LIMPEZA DE CONSOLE REALIZADA.
         */
        public void RemoverProduto()
        {
            Console.WriteLine("Digite um nome do produto:");
            string NomeProduto = Console.ReadLine();

            Console.WriteLine("Você Realmente deseja remover o produto.[S/N]");
            string resposta = Console.ReadLine().ToUpper();

            
            if(resposta == "S")
            {
                try
                {
                    Console.Clear();

                    using (SqlConnection connect = new SqlConnection(DataForAccessDataBase()))
                    {
                        connect.Open();

                        using (SqlCommand cmd = new SqlCommand("DELETE FROM Stock WHERE NomeProduto = @NomeProduto;", connect))
                        {
                            cmd.Parameters.Add("@NomeProduto",NomeProduto);
                            cmd.ExecuteNonQuery();

                            Console.WriteLine("O produto foi removido.");
                        }

                    }
                }catch(Exception e)
                {
                    Console.WriteLine($"[ERRO] Tente Novamente Mais tarde. {e.Message}"); 
                }
            }
            else
            {
                Console.WriteLine("O produto não será removido.");
                Console.Clear();

            }

            
        }

        //NOTE: CONTAGEM TOTAL DE UNIDADES DE UM DETEMIDADO PRODUTO.
        public void ContagemTotal()
        {
            using (SqlConnection connect = new SqlConnection(DataForAccessDataBase()))
            {
                using (SqlCommand cmd = connect.CreateCommand())
                {
                    connect.Open();

                    try
                    {
                        cmd.CommandText = $"SELECT COUNT(NomeProduto) FROM Stock;";
                        object ProdutosTotais = cmd.ExecuteScalar();
                        /*
                         * REALIZANDO UMA CONVERSÃO DE STRING PARA O INT 32 B.
                         * ToInt32
                         */
                        Console.WriteLine($"Número de Produtos Totais: {Convert.ToInt32(ProdutosTotais)}");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(value:$"[ERRO] Tente novamente mais tarde.{e.Message}");
                    }
                    
                }
            }
        }

        public void StatusProduto()
        {

            /*
             * NOTE:
             * 
             * Retornar:
             * NOME: NOME DO PRODUTO;
             * NOME: FORNECEDOR
             * QUANTIDADE: X 
             * 
             */

            Console.WriteLine("Digite o nome do produto");
            string WNomeProduto = Console.ReadLine();

            using (SqlConnection connect = new SqlConnection(DataForAccessDataBase()))
            {
                connect.Open();

                using (SqlCommand cmd = connect.CreateCommand())
                {
                    try{
                        cmd.CommandText = $"SELECT COUNT(NomeProduto) FROM Stock WHERE NomeProduto = @ProdutoNome";
                        cmd.Parameters.AddWithValue("@ProdutoNome",WNomeProduto);
                        object resultado = cmd.ExecuteScalar();

                        if(Convert.ToInt32(resultado) > 0)
                        {
                            cmd.CommandText = $"SELECT * FROM Stock WHERE NomeProduto = @ProdutoNome";
                            cmd.Parameters.AddWithValue("@Produto",WNomeProduto);

                            SqlDataReader rd = cmd.ExecuteReader();
                            while (rd.Read())
                            {
                                Console.WriteLine($"NOME PRODUTO:{rd["NomeProduto"]}");
                                Console.WriteLine($"NOME FORNECEDOR:{rd["FornecedorProduto"]}");
                                Console.WriteLine($"QUANTIDADE:{rd["QuantidadeProduto"]}");

                                //TODO: LIGADO COM O SISTEMA DE USO E REPOSIÇÃO.
                                string searchQuantidade = $"{rd["QuantidadeProduto"]}";
                                int finalQuantidade = int.Parse(searchQuantidade);
                                if (finalQuantidade <  _QuantidadeMinimaRestantes)
                                {
                                    Console.WriteLine($"[AVISO] A QUANTIDADE ESTÁ ABAIXO DO LIMITE MÍNIMO -> {finalQuantidade} UNIDADES.");
                                }
                                else
                                {
                                    Console.WriteLine("");
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"[ERRO] Tente novamente mais tarde.{e.Message}");
                    }
                }
            }
        }

        public void UtilizarProduto()
        {
            /*
             * NOTE: 
             * 
             * UTILIZAR PRODUTO É USADO QUANDO VOCÊ DESEJA REMOVER/FAZER USO UMA QUANTIDADE X DO PRODUTO;
             * ESTANDO REFERENTEMENTE LIGADO AO AVISO DE QUANTIDADE MÍNIMA 
             */
            Console.WriteLine("Digite o nome do produto:");
            string ProdutoNome = Console.ReadLine();
            
            Console.WriteLine("Digite a Quantidade de unidades que você ira utilizar:");
            string stringQuantidadeUsada = Console.ReadLine();
            int quantidadeUsada = int.Parse(stringQuantidadeUsada); 
            
            if(quantidadeUsada < 1)
            {
                throw new Exception("Digite uma quantidade válida");
            }
            else
            {
                try
                {
                    using (SqlConnection connect = new SqlConnection(DataForAccessDataBase()))
                    {
                        connect.Open();

                        using (SqlCommand cmd = new SqlCommand($"SELECT QuantidadeProduto FROM Stock WHERE NomeProduto = @NomeProduto",connect))
                        {
                            cmd.Parameters.AddWithValue("@NomeProduto",ProdutoNome);
                            object resultado = cmd.ExecuteScalar();

                            if(Convert.ToInt32(resultado) < quantidadeUsada)
                            {
                                Console.WriteLine("[ERRO] A quantidade de produtos em estoque está abaixo.");
                            }
                            else
                            {
                                cmd.Parameters.Clear();
                                int QuantidadeAtualizada = Convert.ToInt32(resultado) - quantidadeUsada;

                                cmd.CommandText = "UPDATE Stock SET QuantidadeProduto = @QuantidadeProduto WHERE NomeProduto = @NomeProduto";
                                cmd.Parameters.Add("@QuantidadeProduto",QuantidadeAtualizada);
                                cmd.Parameters.Add("@NomeProduto",ProdutoNome);
                                cmd.ExecuteNonQuery();

                                Console.WriteLine("Estoque Atualizado");
                            }
                        }
                    }
                }catch(Exception ex)
                {
                    Console.WriteLine($"[ERRO] Tente novamente mais tarde.{ex.Message}");
                }
            }
        }


        /*
         * NOTE: REABASTECE O PRODUTO COM BASE NA UNIDADE DIGITADA 
         */
        public void ReabastecerProduto()
        {
            Console.WriteLine("Digite o nome do produto:");
            string ProdutoNome = Console.ReadLine();

            Console.WriteLine("Digite a Quantidade de unidades que você ira reabastecer:");
            string stringQuantidadeReabastecer = Console.ReadLine();
            int quantidadeReabastecer = int.Parse(stringQuantidadeReabastecer);

            if (quantidadeReabastecer < 1)
            {
                throw new Exception("Digite uma quantidade válida");
            }
            else
            {
                try
                {
                    using (SqlConnection connect = new SqlConnection(DataForAccessDataBase()))
                    {
                        connect.Open();

                        using (SqlCommand cmd = new SqlCommand($"SELECT QuantidadeProduto FROM Stock WHERE NomeProduto = @NomeProduto", connect))
                        {
                            cmd.Parameters.AddWithValue("@NomeProduto", ProdutoNome);
                            object resultado = cmd.ExecuteScalar();

                            if (Convert.ToBoolean(resultado))
                            {
                                cmd.Parameters.Clear();
                                int QuantidadeAtualizada = Convert.ToInt32(resultado) + quantidadeReabastecer;

                                cmd.CommandText = "UPDATE Stock SET QuantidadeProduto = @QuantidadeProduto WHERE NomeProduto = @NomeProduto";
                                cmd.Parameters.Add("@QuantidadeProduto", QuantidadeAtualizada);
                                cmd.Parameters.Add("@NomeProduto", ProdutoNome);
                                cmd.ExecuteNonQuery();

                                Console.WriteLine("Estoque Atualizado");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERRO] Tente novamente mais tarde.{ex.Message}");
                }
            }
        }

    }
}

