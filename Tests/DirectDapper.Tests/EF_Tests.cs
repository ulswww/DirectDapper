using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using DirectDapper.AbpEfcore;
using DirectDapper.AbpEfcore.SqlQueries;
using DirectDapper.Providers;
using DirectDapper.Tests.Domain;
using DirectDapper.Tests.EF;
using Shouldly;
using Xunit;

namespace DirectDapper.Tests
{
    public class EF_Tests : TestBase
    {
        [Fact]
        public async Task GetSql_test()
        {
            var unitOfWork = Resolve<IUnitOfWorkManager>();
            var ticketRep = Resolve<IRepository<Ticket>>();
            using (var uow = unitOfWork.Begin())
            {
                await ticketRep.InsertAsync(new Ticket());
                await ticketRep.InsertAsync(new Ticket());

                await  unitOfWork.Current.SaveChangesAsync();

        
                var s = await Resolve<IDirectDapperQueryProvider>()
                                                .SetAbpEfConnectionProvider("1")
                                            //    .SetAbpEfConnectionProvider<SupportDbContext>()
                                               .GetSimpleQueryAdapter("Sqls.Hello.GetWorld2.s")
                                               .SetQueryObj(new { })
                                               .GetResultsAsync<Ticket>();
                s.Count.ShouldBe(2);
            }
        }
    }
}