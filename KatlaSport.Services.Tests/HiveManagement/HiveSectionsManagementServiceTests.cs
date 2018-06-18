namespace KatlaSport.Services.Tests.HiveManagement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoFixture;
    using AutoFixture.Xunit2;

    using AutoMapper;

    using FluentAssertions;

    using KatlaSport.DataAccess.ProductStoreHive;
    using KatlaSport.Services.HiveManagement;

    using Moq;

    using Xunit;

    public class HiveSectionsManagementServiceTests
    {
        public HiveSectionsManagementServiceTests()
        {
            Mapper.Reset();
            Mapper.Initialize(x => x.AddProfile(new HiveManagementMappingProfile()));
        }

        [Theory]
        [AutoMoqData]
        public async Task SetStatus_SetTrue([Frozen] Mock<IProductStoreHiveContext> context, HiveSectionService service, IFixture fixture)
        {
            var hiveSections = fixture.CreateMany<StoreHiveSection>(5).ToArray();

            context.Setup(x => x.Sections).ReturnsEntitySet(hiveSections);

            await service.SetStatusAsync(hiveSections[2].Id, true);

            var hiveSection = await service.GetHiveSectionAsync(hiveSections[2].Id);

            hiveSection.IsDeleted.Should().BeTrue();
        }

        [Theory]
        [AutoMoqData]
        public async Task SetStatus_SetFalse([Frozen] Mock<IProductStoreHiveContext> context, HiveSectionService service, IFixture fixture)
        {
            var hiveSections = fixture.CreateMany<StoreHiveSection>(5).ToArray();

            context.Setup(x => x.Sections).ReturnsEntitySet(hiveSections);

            await service.SetStatusAsync(hiveSections[2].Id, false);

            var hiveSection = await service.GetHiveSectionAsync(hiveSections[2].Id);

            hiveSection.IsDeleted.Should().BeFalse();
        }

        [Theory]
        [AutoMoqData]
        public async Task SetStatus_RequestedResourceNotFoundException(
            [Frozen] Mock<IProductStoreHiveContext> context,
            HiveSectionService service,
            IFixture fixture)
        {
            var section = fixture.Create<StoreHiveSection>();
            context.Setup(x => x.Sections).ReturnsEntitySet(new StoreHiveSection[] { section });
            Func<Task> act = async () => await service.SetStatusAsync(15, true);
            act.Should().Throw<RequestedResourceNotFoundException>();
        }

        [Theory]
        [AutoMoqData]
        public async Task GetHiveSections_EmptySet_ZeroReturned(
            [Frozen] Mock<IProductStoreHiveContext> context,
            HiveSectionService service)
        {
            context.Setup(x => x.Sections).ReturnsEntitySet(new StoreHiveSection[] { });
            var sections = await service.GetHiveSectionsAsync();
            sections.Should().BeEmpty();
        }

        [Theory]
        [AutoMoqData]
        public async Task GetHiveSections_SetWhithFiveElements_FiveReturned(
            [Frozen] Mock<IProductStoreHiveContext> context,
            HiveSectionService service,
            IFixture fixture)
        {
            var sectionsArray = fixture.CreateMany<StoreHiveSection>(5).ToArray();
            context.Setup(x => x.Sections).ReturnsEntitySet(sectionsArray);
            var sections = await service.GetHiveSectionsAsync();
            sections.Should().HaveCount(sections.Count);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetHiveSection_AnyElementSet_OneReturned(
            [Frozen] Mock<IProductStoreHiveContext> context,
            HiveSectionService service,
            IFixture fixture)
        {
            var section = fixture.Create<StoreHiveSection>();
            context.Setup(x => x.Sections).ReturnsEntitySet(new StoreHiveSection[] { section });
            var sectionResult = await service.GetHiveSectionAsync(section.Id);
            sectionResult.Id.Should().Be(section.Id);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetHiveSection_AnyElementSet_RequestedResourceNotFoundException(
            [Frozen] Mock<IProductStoreHiveContext> context,
            HiveSectionService service,
            IFixture fixture)
        {
            var section = fixture.Create<StoreHiveSection>();
            context.Setup(x => x.Sections).ReturnsEntitySet(new StoreHiveSection[] { section });
            Func<Task> act = async () => await service.GetHiveSectionAsync(10);

            act.Should().Throw<RequestedResourceNotFoundException>();
        }

        [Theory]
        [AutoMoqData]
        public async Task GetHiveSections_ByHiveId_SetFiveElement_OneReturned(
            [Frozen] Mock<IProductStoreHiveContext> context,
            HiveSectionService service,
            IFixture fixture)
        {
            var colections = fixture.CreateMany<StoreHiveSection>(5).ToArray();
            context.Setup(x => x.Sections).ReturnsEntitySet(colections);
            var section = await service.GetHiveSectionsAsync(colections[0].StoreHiveId);
            section[0].Id.Should().Be(colections[0].Id);
        }

        [Theory]
        [AutoMoqData]
        public async Task CreateHiveSectio_SetOneElement_OneReturned(
            [Frozen] Mock<IProductStoreHiveContext> context,
            HiveSectionService service,
            IFixture fixture)
        {
            var hives = fixture.CreateMany<StoreHive>(3).ToArray();
            context.Setup(x => x.Hives).ReturnsEntitySet(hives);
            context.Setup(x => x.Sections).ReturnsEntitySet(new List<StoreHiveSection>());
            var section = fixture.Create<UpdateHiveSectionRequest>();
            await service.CreateHiveSection(section, hives[0].Id);

            var result = await service.GetHiveSectionsAsync();
            result.Count.Should().Be(1);
        }

        [Theory]
        [AutoMoqData]
        public async Task CreateHiveSectio_RequestedResourceHasConflictException(
            [Frozen] Mock<IProductStoreHiveContext> context,
            HiveSectionService service,
            IFixture fixture)
        {
            var hives = fixture.CreateMany<StoreHive>(3).ToArray();
            context.Setup(x => x.Hives).ReturnsEntitySet(hives);
            var sections = fixture.CreateMany<StoreHiveSection>(3).ToList();
            context.Setup(x => x.Sections).ReturnsEntitySet(sections);
            var section = new UpdateHiveSectionRequest { Code = sections[0].Code };
            Func<Task> act = async () => await service.CreateHiveSection(section, hives[0].Id);

            act.Should().Throw<RequestedResourceHasConflictException>();
        }

        [Theory]
        [AutoMoqData]
        public async Task CreateHiveSectio_RequestedResourceNotFoundException(
            [Frozen] Mock<IProductStoreHiveContext> context,
            HiveSectionService service,
            IFixture fixture)
        {
            var hives = fixture.CreateMany<StoreHive>(3).ToArray();
            context.Setup(x => x.Hives).ReturnsEntitySet(hives);
            var sections = fixture.CreateMany<StoreHiveSection>(3).ToList();
            context.Setup(x => x.Sections).ReturnsEntitySet(sections);
            var section = fixture.Create<UpdateHiveSectionRequest>();
            Func<Task> act = async () => await service.CreateHiveSection(section, 20);

            act.Should().Throw<RequestedResourceNotFoundException>();
        }

        [Theory]
        [AutoMoqData]
        public async Task DeleteHiveSection_SetFiveElement_DeleteOne_FourReturned(
            [Frozen] Mock<IProductStoreHiveContext> contex,
            HiveSectionService service,
            IFixture fixture)
        {
            var hiveSections = fixture.CreateMany<StoreHiveSection>(5).ToList();
            var hives = fixture.CreateMany<StoreHive>(3).ToArray();
            contex.Setup(x => x.Hives).ReturnsEntitySet(hives);
            contex.Setup(x => x.Sections).ReturnsEntitySet(hiveSections);
            await service.SetStatusAsync(hiveSections[0].Id, true);
            await service.DeleteHiveSection(hiveSections[0].Id);

            var result = await service.GetHiveSectionsAsync();

            result.Count.Should().Be(4);
        }

        [Theory]
        [AutoMoqData]
        public async Task DeleteHiveSection_RequestedResourceNotFoundException(
            [Frozen] Mock<IProductStoreHiveContext> contex,
            HiveSectionService service,
            IFixture fixture)
        {
            var hiveSections = fixture.CreateMany<StoreHiveSection>(5).ToList();
            var hives = fixture.CreateMany<StoreHive>(3).ToArray();
            contex.Setup(x => x.Hives).ReturnsEntitySet(hives);
            contex.Setup(x => x.Sections).ReturnsEntitySet(hiveSections);
            await service.SetStatusAsync(hiveSections[0].Id, true);
            Func<Task> act = async () => await service.DeleteHiveSection(0);

            act.Should().Throw<RequestedResourceNotFoundException>();
        }

        [Theory]
        [AutoMoqData]
        public async Task DeleteHiveSection_RequestedResourceHasConflictException(
            [Frozen] Mock<IProductStoreHiveContext> contex,
            HiveSectionService service,
            IFixture fixture)
        {
            var hiveSections = fixture.CreateMany<StoreHiveSection>(5).ToList();
            var hives = fixture.CreateMany<StoreHive>(3).ToArray();
            contex.Setup(x => x.Hives).ReturnsEntitySet(hives);
            contex.Setup(x => x.Sections).ReturnsEntitySet(hiveSections);
            await service.SetStatusAsync(hiveSections[0].Id, false);
            Func<Task> act = async () => await service.DeleteHiveSection(hiveSections[0].Id);

            act.Should().Throw<RequestedResourceHasConflictException>();
        }
    }
}
