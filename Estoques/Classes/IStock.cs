using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estoques
{
    internal interface IStock
    {
        void AddProduto();
        void RemoverProduto();
        void ContagemTotal();
        void StatusProduto();
        void UtilizarProduto();
        void ReabastecerProduto(); 
    }
}
