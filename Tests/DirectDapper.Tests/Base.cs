using Abp.Dependency;
using Abp.EntityFrameworkCore.Tests;
using Abp.TestBase;

namespace DirectDapper.Tests
{
    public class TestBase : AbpIntegratedTestBase<EntityFrameworkCoreTestModule>
    {
        public TestBase(bool initializeAbp = true, IIocManager localIocManager = null) : base(initializeAbp, localIocManager)
        {
        }
    }
}