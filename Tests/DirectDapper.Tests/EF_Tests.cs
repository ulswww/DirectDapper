using System.Threading.Tasks;
using Abp.Domain.Uow;
using DirectDapper.Providers;
using DirectDapper.Tests.Domain;
using Xunit;

namespace DirectDapper.Tests
{
    public class EF_Tests : TestBase
    {
        [Fact]
        public async Task GetSql_test()
        {
            var unitOfWork = Resolve<IUnitOfWorkManager>();
            using (var uow = unitOfWork.Begin())
            {
                var s = await Resolve<IDirectDapperQueryProvider>()
                                               .GetSimpleQueryAdapter("Sqls.Hello.GetWorld2.s")
                                               .SetQueryObj(new { })
                                               .GetResultsAsync<Ticket>();

            }
        }
    }
}