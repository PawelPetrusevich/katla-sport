namespace KatlaSport.Services.Tests.ProductManagement
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoFixture;
    using AutoFixture.Xunit2;

    using FluentAssertions;

    using KatlaSport.DataAccess.ProductCatalogue;
    using KatlaSport.Services.ProductManagement;
    using KatlaSport.Services.ProductManagement.DTO;

    using Moq;
    using Xunit;
    using DbProductCategory = KatlaSport.DataAccess.ProductCatalogue.ProductCategory;

    public class ProductCategoryServiceTests
    {
        public ProductCategoryServiceTests()
        {
            MapperInitialize.Initialize();
        }

        [Theory]
        [AutoMoqData]
        public async Task GetCategoriesAsync_CreqteFiveElement_ReturnFiveElements(
            [Frozen] Mock<IProductCatalogueContext> context,
            ProductCategoryService service,
            IFixture fixture)
        {
            var categories = fixture.CreateMany<DbProductCategory>(5).ToList();
            var products = fixture.CreateMany<CatalogueProduct>(5).ToList();
            context.Setup(x => x.Products).ReturnsEntitySet(products);
            context.Setup(x => x.Categories).ReturnsEntitySet(categories);
            var result = await service.GetCategoriesAsync(0, 5);

            result.Count.Should().Be(5);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetCategoryAsync_CreqteFiveElement_ReturnOneFiveElement(
            [Frozen] Mock<IProductCatalogueContext> context,
            ProductCategoryService service,
            IFixture fixture)
        {
            var categories = fixture.CreateMany<DbProductCategory>(5).ToList();
            var products = fixture.CreateMany<CatalogueProduct>(5).ToList();
            context.Setup(x => x.Products).ReturnsEntitySet(products);
            context.Setup(x => x.Categories).ReturnsEntitySet(categories);
            var result = await service.GetCategoryAsync(categories[0].Id);

            result.Id.Should().Be(categories[0].Id);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetCategoryAsync_RequestedResourceNotFoundException(
            [Frozen] Mock<IProductCatalogueContext> context,
            ProductCategoryService service,
            IFixture fixture)
        {
            var categories = fixture.CreateMany<DbProductCategory>(5).ToList();
            var products = fixture.CreateMany<CatalogueProduct>(5).ToList();
            context.Setup(x => x.Products).ReturnsEntitySet(products);
            context.Setup(x => x.Categories).ReturnsEntitySet(categories);
            Func<Task> act = async () => await service.GetCategoryAsync(0);

            act.Should().Throw<RequestedResourceNotFoundException>();
        }

        [Theory]
        [AutoMoqData]
        public async Task CreateCategoryAsync_AddOneElement(
            [Frozen] Mock<IProductCatalogueContext> context,
            ProductCategoryService service,
            IFixture fixture)
        {
            var categories = fixture.CreateMany<DbProductCategory>(5).ToList();
            var products = fixture.CreateMany<CatalogueProduct>(5).ToList();
            var updateCategory = fixture.Create<UpdateProductCategoryRequest>();
            context.Setup(x => x.Products).ReturnsEntitySet(products);
            context.Setup(x => x.Categories).ReturnsEntitySet(categories);

            await service.CreateCategoryAsync(updateCategory);

            var result = await service.GetCategoriesAsync(0, 6);

            result.Count.Should().Be(6);
        }

        [Theory]
        [AutoMoqData]
        public async Task CreateCategoryAsync_RequestedResourceHasConflictException(
            [Frozen] Mock<IProductCatalogueContext> context,
            ProductCategoryService service,
            IFixture fixture)
        {
            var categories = fixture.CreateMany<DbProductCategory>(5).ToList();
            var products = fixture.CreateMany<CatalogueProduct>(5).ToList();
            var updateCategory = new UpdateProductCategoryRequest { Code = categories[0].Code };
            context.Setup(x => x.Products).ReturnsEntitySet(products);
            context.Setup(x => x.Categories).ReturnsEntitySet(categories);

            Func<Task> act = async () => await service.CreateCategoryAsync(updateCategory);

            act.Should().Throw<RequestedResourceHasConflictException>();
        }

        [Theory]
        [AutoMoqData]
        public async Task UpdateCategoryAsync_UpdateOneElement(
            [Frozen] Mock<IProductCatalogueContext> context,
            ProductCategoryService service,
            IFixture fixture)
        {
            var categories = fixture.CreateMany<DbProductCategory>(5).ToList();
            var products = fixture.CreateMany<CatalogueProduct>(5).ToList();
            var updateCategory = fixture.Create<UpdateProductCategoryRequest>();
            context.Setup(x => x.Products).ReturnsEntitySet(products);
            context.Setup(x => x.Categories).ReturnsEntitySet(categories);

            await service.UpdateCategoryAsync(categories[0].Id, updateCategory);

            var result = await service.GetCategoryAsync(categories[0].Id);

            result.Code.Should().Be(updateCategory.Code);

            //Func<Task> act = async () => await service.CreateCategoryAsync(updateCategory);

            //act.Should().Throw<RequestedResourceHasConflictException>();
        }

        [Theory]
        [AutoMoqData]
        public async Task UpdateCategoryAsync_RequestedResourceNotFoundException(
            [Frozen] Mock<IProductCatalogueContext> context,
            ProductCategoryService service,
            IFixture fixture)
        {
            var categories = fixture.CreateMany<DbProductCategory>(5).ToList();
            var products = fixture.CreateMany<CatalogueProduct>(5).ToList();
            var updateCategory = fixture.Create<UpdateProductCategoryRequest>();
            context.Setup(x => x.Products).ReturnsEntitySet(products);
            context.Setup(x => x.Categories).ReturnsEntitySet(categories);

            Func<Task> act = async () => await service.UpdateCategoryAsync(0, updateCategory);

            act.Should().Throw<RequestedResourceNotFoundException>();
        }

        [Theory]
        [AutoMoqData]
        public async Task UpdateCategoryAsync_RequestedResourceHasConflictException(
            [Frozen] Mock<IProductCatalogueContext> context,
            ProductCategoryService service,
            IFixture fixture)
        {
            var categories = fixture.CreateMany<DbProductCategory>(5).ToList();
            var products = fixture.CreateMany<CatalogueProduct>(5).ToList();
            var updateCategory = new UpdateProductCategoryRequest { Code = categories[1].Code };
            context.Setup(x => x.Products).ReturnsEntitySet(products);
            context.Setup(x => x.Categories).ReturnsEntitySet(categories);

            Func<Task> act = async () => await service.UpdateCategoryAsync(categories[0].Id, updateCategory);

            act.Should().Throw<RequestedResourceHasConflictException>();
        }

        [Theory]
        [AutoMoqData]
        public async Task DeleteCategoryAsync_OneElementDelete(
            [Frozen] Mock<IProductCatalogueContext> context,
            ProductCategoryService service,
            IFixture fixture)
        {
            var categories = fixture.CreateMany<DbProductCategory>(5).ToList();
            var products = fixture.CreateMany<CatalogueProduct>(5).ToList();
            categories[0].IsDeleted = true;
            context.Setup(x => x.Products).ReturnsEntitySet(products);
            context.Setup(x => x.Categories).ReturnsEntitySet(categories);

            await service.DeleteCategoryAsync(categories[0].Id);

            var result = await service.GetCategoriesAsync(0, 6);
            result.Count.Should().Be(4);
        }

        [Theory]
        [AutoMoqData]
        public async Task DeleteCategoryAsync_RequestedResourceHasConflictException(
            [Frozen] Mock<IProductCatalogueContext> context,
            ProductCategoryService service,
            IFixture fixture)
        {
            var categories = fixture.CreateMany<DbProductCategory>(5).ToList();
            var products = fixture.CreateMany<CatalogueProduct>(5).ToList();
            categories[0].IsDeleted = false;
            context.Setup(x => x.Products).ReturnsEntitySet(products);
            context.Setup(x => x.Categories).ReturnsEntitySet(categories);

            Func<Task> act = async () => await service.DeleteCategoryAsync(categories[0].Id);

            act.Should().Throw<RequestedResourceHasConflictException>();
        }

        [Theory]
        [AutoMoqData]
        public async Task DeleteCategoryAsync_RequestedResourceNotFoundException(
            [Frozen] Mock<IProductCatalogueContext> context,
            ProductCategoryService service,
            IFixture fixture)
        {
            var categories = fixture.CreateMany<DbProductCategory>(5).ToList();
            var products = fixture.CreateMany<CatalogueProduct>(5).ToList();
            categories[0].IsDeleted = true;
            context.Setup(x => x.Products).ReturnsEntitySet(products);
            context.Setup(x => x.Categories).ReturnsEntitySet(categories);

            Func<Task> act = async () => await service.DeleteCategoryAsync(0);

            act.Should().Throw<RequestedResourceNotFoundException>();
        }

        [Theory]
        [AutoMoqData]
        public async Task SetStatusAsync_RequestedResourceNotFoundException(
            [Frozen] Mock<IProductCatalogueContext> context,
            ProductCategoryService service,
            IFixture fixture)
        {
            var categories = fixture.CreateMany<DbProductCategory>(5).ToList();
            var products = fixture.CreateMany<CatalogueProduct>(5).ToList();
            context.Setup(x => x.Products).ReturnsEntitySet(products);
            context.Setup(x => x.Categories).ReturnsEntitySet(categories);

            Func<Task> act = async () => await service.SetStatusAsync(0, true);

            act.Should().Throw<RequestedResourceNotFoundException>();
        }

        [Theory]
        [AutoMoqData]
        public async Task SetStatusAsync_SetTrueStatus(
            [Frozen] Mock<IProductCatalogueContext> context,
            ProductCategoryService service,
            IFixture fixture)
        {
            var categories = fixture.CreateMany<DbProductCategory>(5).ToList();
            var products = fixture.CreateMany<CatalogueProduct>(5).ToList();
            context.Setup(x => x.Products).ReturnsEntitySet(products);
            context.Setup(x => x.Categories).ReturnsEntitySet(categories);

            await service.SetStatusAsync(categories[0].Id, true);

            var result = await service.GetCategoryAsync(categories[0].Id);

            result.IsDeleted.Should().BeTrue();
        }
    }
}
