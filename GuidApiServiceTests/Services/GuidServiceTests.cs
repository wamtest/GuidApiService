using AutoFixture;
using GuidApiService.DataProviders;
using GuidApiService.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GuidApiService.Services.Tests
{
    public class GuidServiceTests
    {
        private readonly Fixture _fixture;

        public GuidServiceTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void BuildGuid_Withguid_ReturnsGuidInfo()
        {
            string guid = Guid.NewGuid().ToString();
            var guidInput = _fixture.Create<GuidInput>();
            var options = GetDbContextOptions();

            using (var context = new GuidServiceContext(options))
            {
                var repo = new GuidApiRepository(context);
                var sut = new GuidService(repo);
                var guidInfo = sut.BuildGuid(guid, guidInput);
                Assert.Equal(guid, guidInfo.GuidInfoId.ToString());
            }
        }

        [Fact()]
        public async void DeleteTest_OnExistingGuid_DeletesItem()
        {
            var guid = Guid.NewGuid();
            var user = _fixture.Create<string>();
            var expire = DateTime.Now.AddDays(1);
            var options = GetDbContextOptions();

            using (var context = new GuidServiceContext(options))
            {
                context.GuidInfoSet.Add(new GuidInfo()
                {
                    GuidInfoId = guid,
                    GuidInput = new GuidInput() { MetaData = user, Expire = expire }
                });
                context.SaveChanges();

                var repo = new GuidApiRepository(context);
                var sut = new GuidService(repo);
                var output = await sut.Get(guid.ToString());

                Assert.Equal(guid.ToString("N").ToUpper(), output?.Guid);
                Assert.Equal(user, output?.User);

                await sut.Delete(guid.ToString());
                output = await sut.Get(guid.ToString());

                Assert.Null(output);
            }
        }

        [Fact]
        public async void GetTest_WithExistingGuid_ReturnsResult()
        {
            var guid = Guid.NewGuid();
            var user = _fixture.Create<string>();
            var expire = DateTime.Now.AddDays(1);
            var options = GetDbContextOptions();

            using (var context = new GuidServiceContext(options))
            {
                context.GuidInfoSet.Add(new GuidInfo()
                {
                    GuidInfoId = guid,
                    GuidInput = new GuidInput() { MetaData = user, Expire = expire }
                });
                context.SaveChanges();

                var repo = new GuidApiRepository(context);
                var sut = new GuidService(repo);
                var output = await sut.Get(guid.ToString());

                Assert.Equal(guid.ToString("N").ToUpper(), output?.Guid);
                Assert.Equal(user, output?.User);
            }
        }


        [Fact()]
        public async void SaveTest_WithUser_SavesWithOutput()
        {
            var user = _fixture.Create<string>();
            var options = GetDbContextOptions();

            using (var context = new GuidServiceContext(options))
            {

                var repo = new GuidApiRepository(context);
                var sut = new GuidService(repo);

                var output = await sut.Create(user);

                Assert.Equal(user, output?.User);
                Assert.NotNull(output?.Expire);
                Assert.NotEqual(0, output?.Expire);
                Assert.NotEmpty(output?.Guid);
            }
        }

        [Fact]
        public async void UpdateTest_WithExistingGuid_UpdatesWithOutput()
        {
            var guid = Guid.NewGuid();
            var user = _fixture.Create<string>();
            var updatedUser = _fixture.Create<string>();
            var expire = DateTime.Now.AddDays(1);
            var options = GetDbContextOptions();

            using (var context = new GuidServiceContext(options))
            {
                context.GuidInfoSet.Add(new GuidInfo()
                {
                    GuidInfoId = guid,
                    GuidInput = new GuidInput() { MetaData = user, Expire = expire }
                });
                context.SaveChanges();

                var repo = new GuidApiRepository(context);
                var sut = new GuidService(repo);
                var output = await sut.Update(guid.ToString(), 0, updatedUser);

                Assert.Equal(updatedUser, output.User);
            }
        }

        [Fact()]
        public void IsExpiryInPastTest_TODO()
        {
            // TODO
        }

        [Fact()]
        public void IsExpiryValidTest_TODO()
        {
            // TODO
        }

        [Fact()]
        public void IsValidGuidTest()
        {
            // TODO
        }

        [Fact()]
        public void IsValidUserTest_TODO()
        {
            // TODO
        }

        private DbContextOptions<GuidServiceContext> GetDbContextOptions()
        {
            return new DbContextOptionsBuilder<GuidServiceContext>()
                            .UseInMemoryDatabase("GuidInfoSet")
                            .Options;
        }


    }
}