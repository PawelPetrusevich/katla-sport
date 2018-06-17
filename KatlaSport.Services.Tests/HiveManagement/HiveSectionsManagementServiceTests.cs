namespace KatlaSport.Services.Tests.HiveManagement
{
    using System;
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
    }
}
