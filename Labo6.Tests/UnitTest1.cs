using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Labo0911;
using System.Threading.Tasks;

namespace Labo6.Tests
{
    [TestClass]
    public class UnitTest1
    {
        CompanyContext _context;
        [TestInitialize]
        public void AvantChaqueTest()
        {
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder();
            DbContextOptions options = builder.UseSqlServer(@"Data Source=vm-sql2.iesn.be\Stu3ig;Initial Catalog=1819_etu35884_Labo6; User Id=1819_etu35884; Password=K1iD0cH4n").Options;

            _context = new CompanyContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            _context.Customers.Add(new Customer(){
                    AddressLine1 = "Rue J. Calozet, 19",
                    PostCode = "5000",
                    City = "Namur",
                    AccountBalance = 12,
                    Name = "Doe",
                    Country = "Belgique",
                    EMail = "info@doe.com",
                    Remark = "Client suspect"
            });
            _context.SaveChanges();
        }

        [TestMethod]
        public async Task TestMethod()
        {
            Customer client = await _context.Customers.FirstAsync();
            Assert.AreEqual("5000", client.PostCode);
        }

        [TestMethod]
        public async Task AccesConcurrent()
        {
            Customer client = await _context.Customers.FirstAsync();
            var firstCall = Acces(client, 50);
            await Task.Delay(1000);
            var secondCall = Acces(client, 60);

            await Task.WhenAll(firstCall, secondCall);

        }

        private async Task Acces(Customer client, int accountBalance)
        {
            client.AccountBalance = accountBalance;
            await Task.Delay(3000);
            Assert.AreEqual(accountBalance, client.AccountBalance);
        }
    }
}
