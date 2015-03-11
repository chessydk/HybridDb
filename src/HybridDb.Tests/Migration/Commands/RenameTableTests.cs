using HybridDb.Config;
using HybridDb.Migration.Commands;
using Shouldly;
using Xunit;
using Xunit.Extensions;

namespace HybridDb.Tests.Migration.Commands
{
    public class RenameTableTests : HybridDbTests
    {
        [Theory]
        [InlineData(TableMode.UseTempTables)]
        [InlineData(TableMode.UseRealTables)]
        public void RenamesTable(TableMode mode)
        {
            Use(mode);
            new CreateTable(new Table("Entities", new Column("col1", typeof(int)))).Execute(database);

            new RenameTable("Entities", "OtherEntities").Execute(database);

            database.QuerySchema().ShouldNotContainKey("Entities");
            database.QuerySchema().ShouldContainKey("OtherEntities");
        }


        [Fact]
        public void DoesNotRequireReprojection()
        {
            new RenameTable("Entities", "OtherEntities").RequiresReprojectionOf.ShouldBe(null);
        }

        [Fact]
        public void IsSafe()
        {
            new RenameTable("Entities", "OtherEntities").Unsafe.ShouldBe(false);
        }
    }
}