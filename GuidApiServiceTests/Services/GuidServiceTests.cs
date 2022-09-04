using Xunit;
using GuidApiService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Microsoft.EntityFrameworkCore;
using GuidApiService.Models;
using AutoFixture;

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
                GuidService sut = new GuidService(context);
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

                GuidService sut = new GuidService(context);
                var output = await sut.Get(guid.ToString());

                Assert.Equal(guid.ToString("N").ToUpper(), output.Guid);
                Assert.Equal(user, output.User);

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

                GuidService sut = new GuidService(context);
                var output = await sut.Get(guid.ToString());

                Assert.Equal(guid.ToString("N").ToUpper(), output.Guid);
                Assert.Equal(user, output.User);
            }
        }


        [Fact()]
        public async void SaveTest_WithUser_SavesWithOutput()
        {
            var user = _fixture.Create<string>();
            var options = GetDbContextOptions();

            using (var context = new GuidServiceContext(options))
            {
                GuidService sut = new GuidService(context);
                var output = await sut.Save(user);

                Assert.Equal(user, output.User);
                Assert.NotNull(output.Expire);
                Assert.NotEqual(0, output.Expire);
                Assert.NotEmpty(output.Guid);
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

                GuidService sut = new GuidService(context);
                var output = await sut.Update(guid.ToString(), 0, updatedUser);

                Assert.Equal(updatedUser, output.User);
            }
        }

        [Fact()]
        public void IsExpiryInPastTest()
        {
            // TODO
        }

        [Fact()]
        public void IsExpiryValidTest()
        {
            // TODO
        }

        [Fact()]
        public void IsValidGuidTest()
        {
            // TODO
        }

        [Fact()]
        public void IsValidUserTest()
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