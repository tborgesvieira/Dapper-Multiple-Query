using Dapper;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        public static string ConnectionString => @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\projetos\git\Dapper-Multiple-Query\ConsoleApp1\ConsoleApp1\App_Data\Database.mdf;Integrated Security=True";

        protected SqlConnection _connection;
        protected SqlConnection connection => _connection ?? (_connection = GetOpenConnection());

        public static SqlConnection GetOpenConnection()
        {            
            var cs = ConnectionString;

            var connection = new SqlConnection(cs);
            connection.Open();
            return connection;
        }

        static void Main(string[] args)
        {            
            var sql = @"select * from Empresa where id = @id
                        select * from Cliente where empresa = @id";

            var cn = GetOpenConnection();

            for (int i = 1; i <= 5; i++)
                using (var multi = cn.QueryMultiple(sql, new { id = i }))
                {
                    var empresa = multi.Read<Empresa>().SingleOrDefault();

                    if (empresa != null)
                        empresa.Clientes = multi.Read<Cliente>().ToList();

                    Console.WriteLine("=================");
                    Console.WriteLine($"Empresa: {empresa.Id} - {empresa.Nome}");
                    Console.WriteLine("Cliente(s):");
                    empresa.Clientes.ToList().ForEach(c => Console.WriteLine($"  {c.Id} - {c.Nome}"));
                }

            Console.ReadKey();
        }
    }
}
