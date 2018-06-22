namespace KatlaSport.Services.Tests.HiveManagement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoFixture;
    using AutoFixture.Xunit2;

    using FluentAssertions;

    using KatlaSport.DataAccess.ProductStoreHive;
    using KatlaSport.Services.HiveManagement;

    using Moq;

    using Xunit;

    public class HiveManagementServiceTests
    {
        public HiveManagementServiceTests()
        {
            var x = MapperInitialize.Initialize();
        }

        [Theory]
        [AutoMoqData]
        public async Task GetHives_SetFiveElement_FiveElementReturned(
            [Frozen] Mock<IProductStoreHiveContext> context,
            HiveService service,
            IFixture fixture)
        {
            var colection = fixture.CreateMany<StoreHive>(5).ToArray();
            context.Setup(x => x.Hives).ReturnsEntitySet(colection);
            context.Setup(x => x.Sections).ReturnsEntitySet(colection[0].Sections.ToList());
            var hives = await service.GetHivesAsync();
            hives.Count.Should().Be(colection.Length);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetHive_ById_SetFiveElement_OneReturned(
            [Frozen] Mock<IProductStoreHiveContext> context,
            HiveService service,
            IFixture fixture)
        {
            var collection = fixture.CreateMany<StoreHive>(5).ToArray();
            context.Setup(x => x.Hives).ReturnsEntitySet(collection);
            var hive = await service.GetHiveAsync(collection[0].Id);
            hive.Id.Should().Be(collection[0].Id);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetHive_RequestedResourceNotFoundException(
            [Frozen] Mock<IProductStoreHiveContext> context,
            HiveService service)
        {
            context.Setup(x => x.Hives).ReturnsEntitySet(new List<StoreHive>());
            Func<Task> act = async () => await service.GetHiveAsync(0);
            act.Should().Throw<RequestedResourceNotFoundException>();
        }

        [Theory]
        [AutoMoqData]
        public async Task CreateHive_SetOne_ReturnedOne(
            [Frozen] Mock<IProductStoreHiveContext> context,
            HiveService service,
            IFixture fixture)
        {
            context.Setup(x => x.Hives).ReturnsEntitySet(new List<StoreHive>());
            context.Setup(x => x.Sections).ReturnsEntitySet(new List<StoreHiveSection>());
            var hive = fixture.Create<UpdateHiveRequest>();
            await service.CreateHiveAsync(hive);
            var hives = await service.GetHivesAsync();
            hives.Count.Should().Be(1);
        }

        [Theory]
        [AutoMoqData]
        public async Task CreateHive_RequestedResourceHasConflictException(
            [Frozen] Mock<IProductStoreHiveContext> context,
            HiveService service,
            IFixture fixture)
        {
            var collection = fixture.CreateMany<StoreHive>(5).ToArray();
            var hive = new UpdateHiveRequest { Code = collection[0].Code };
            context.Setup(x => x.Hives).ReturnsEntitySet(collection);
            Func<Task> act = async () => await service.CreateHiveAsync(hive);

            act.Should().Throw<RequestedResourceHasConflictException>();
        }

        [Theory]
        [AutoMoqData]
        public async Task UpdateHive_SetFive_OneUpdate(
            [Frozen] Mock<IProductStoreHiveContext> context,
            HiveService service,
            IFixture fixture)
        {
            var collection = fixture.CreateMany<StoreHive>(5).ToArray();
            context.Setup(x => x.Hives).ReturnsEntitySet(collection);
            context.Setup(x => x.Sections).ReturnsEntitySet(new List<StoreHiveSection>());
            var address = "New Address";
            var updateHive = new UpdateHiveRequest { Address = address };
            var result = await service.UpdateHiveAsync(collection[0].Id, updateHive);

            result.Id.Should().Be(collection[0].Id);
            result.Address.Should().Be(address);
        }

        [Theory]
        [AutoMoqData]
        public async Task UpdateHive_RequestedResourceHasConflictException(
            [Frozen] Mock<IProductStoreHiveContext> contex,
            HiveService service,
            IFixture fixture)
        {
            var collection = fixture.CreateMany<StoreHive>(5).ToArray();
            contex.Setup(x => x.Hives).ReturnsEntitySet(collection);
            var hive = new UpdateHiveRequest { Code = collection[0].Code };

            Func<Task> act = async () => await service.UpdateHiveAsync(1, hive);

            act.Should().Throw<RequestedResourceHasConflictException>();
        }

        [Theory]
        [AutoMoqData]
        public async Task UpdateHive_RequestedResourceNotFoundException(
            [Frozen] Mock<IProductStoreHiveContext> contex,
            HiveService service,
            IFixture fixture)
        {
            var collection = fixture.CreateMany<StoreHive>(5).ToArray();
            contex.Setup(x => x.Hives).ReturnsEntitySet(collection);
            var hive = fixture.Create<UpdateHiveRequest>();

            Func<Task> act = async () => await service.UpdateHiveAsync(1, hive);

            act.Should().Throw<RequestedResourceNotFoundException>();
        }

        [Theory]
        [AutoMoqData]
        public async Task DeleteHive_SetFiveElement_DeleteOne_FourReturned(
            [Frozen] Mock<IProductStoreHiveContext> contex,
            HiveService service,
            IFixture fixture)
        {
            var collection = fixture.CreateMany<StoreHive>(5).ToList();
            contex.Setup(x => x.Hives).ReturnsEntitySet(collection);
            contex.Setup(x => x.Sections).ReturnsEntitySet(new List<StoreHiveSection>());
            await service.SetStatusAsync(collection[0].Id, true);
            await service.DeleteHiveAsync(collection[0].Id);

            var result = await service.GetHivesAsync();

            result.Count.Should().Be(4);
        }

        [Theory]
        [AutoMoqData]
        public async Task DeleteHive_RequestedResourceNotFoundException(
            [Frozen] Mock<IProductStoreHiveContext> contex,
            HiveService service,
            IFixture fixture)
        {
            var collection = fixture.CreateMany<StoreHive>(5).ToList();
            contex.Setup(x => x.Hives).ReturnsEntitySet(collection);
            contex.Setup(x => x.Sections).ReturnsEntitySet(new List<StoreHiveSection>());

            Func<Task> act = async () => await service.DeleteHiveAsync(0);

            act.Should().Throw<RequestedResourceNotFoundException>();
        }

        [Theory]
        [AutoMoqData]
        public async Task DeleteHive_RequestedResourceHasConflictException(
            [Frozen] Mock<IProductStoreHiveContext> contex,
            HiveService service,
            IFixture fixture)
        {
            var collection = fixture.CreateMany<StoreHive>(5).ToList();
            contex.Setup(x => x.Hives).ReturnsEntitySet(collection);
            contex.Setup(x => x.Sections).ReturnsEntitySet(new List<StoreHiveSection>());
            await service.SetStatusAsync(collection[0].Id, false);

            Func<Task> act = async () => await service.DeleteHiveAsync(collection[0].Id);

            act.Should().Throw<RequestedResourceHasConflictException>();
        }

        [Theory]
        [AutoMoqData]
        public async Task SetStatus_SetTrue(
            [Frozen] Mock<IProductStoreHiveContext> contex,
            HiveService service,
            IFixture fixture)
        {
            var collection = fixture.CreateMany<StoreHive>(5).ToArray();
            contex.Setup(x => x.Hives).ReturnsEntitySet(collection);

            await service.SetStatusAsync(collection[0].Id, true);

            var result = await service.GetHiveAsync(collection[0].Id);

            result.IsDeleted.Should().BeTrue();
        }

        [Theory]
        [AutoMoqData]
        public async Task SetStatus_SetFalse(
            [Frozen] Mock<IProductStoreHiveContext> contex,
            HiveService service,
            IFixture fixture)
        {
            var collection = fixture.CreateMany<StoreHive>(5).ToArray();
            contex.Setup(x => x.Hives).ReturnsEntitySet(collection);

            await service.SetStatusAsync(collection[0].Id, false);

            var result = await service.GetHiveAsync(collection[0].Id);

            result.IsDeleted.Should().BeFalse();
        }

        [Theory]
        [AutoMoqData]
        public async Task SetStatus_RequestedResourceNotFoundException(
            [Frozen] Mock<IProductStoreHiveContext> contex,
            HiveService service,
            IFixture fixture)
        {
            var collection = fixture.CreateMany<StoreHive>(5).ToArray();
            contex.Setup(x => x.Hives).ReturnsEntitySet(collection);

            Func<Task> act = async () => await service.SetStatusAsync(0, true);

            act.Should().Throw<RequestedResourceNotFoundException>();
        }
    }
}
