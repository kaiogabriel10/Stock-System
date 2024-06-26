using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estoques
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Product p1 = new Product("Achocolatado",10,"Nestle",5);
            p1.StatusProduto();
        }
    }
}
