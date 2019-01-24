using System.Collections.Generic;

namespace ConsoleApp1
{
    public class Empresa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public virtual ICollection<Cliente> Clientes { get; set; }
    }
}
