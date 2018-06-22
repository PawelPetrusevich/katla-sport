using System.Linq;
using System.Threading.Tasks;

namespace KatlaSport.Services.Tests.ProductManagement
{
    using System;

    using AutoFixture;
    using AutoFixture.Xunit2;

    using FluentAssertions;

    using KatlaSport.DataAccess.ProductCatalogue;
    using KatlaSport.Services.ProductManagement;

    using Moq;

    using Xunit;

    public class ProductCatalogueServiceTests
    {
        public ProductCatalogueServiceTests()
        {
            MapperInitialize.Initialize();
        }

        [Theory]
        [AutoMoqData]
        public async Task GetProductsAsync_ReturnTwoElement(
            [Frozen] Mock<IProductCatalogueContext> context,
            ProductCatalogueService service,
            IFixture fixture)
        {
            var start = 1;
            var amount = 2;
            var products = fixture.CreateMany<CatalogueProduct>(5).ToList();
            context.Setup(x => x.Products).ReturnsEntitySet(products);
            var result = await service.GetProductsAsync(1, 2);

            result.Count.Should().Be(2);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetProductAsync_ById_ReturnOneElement(
            [Frozen] Mock<IProductCatalogueContext> context,
            ProductCatalogueService service,
            IFixture fixture)
        {
            var products = fixture.CreateMany<CatalogueProduct>(5).ToList();
            context.Setup(x => x.Products).ReturnsEntitySet(products);
            var result = await service.GetProductAsync(products[0].Id);

            result.Id.Should().Be(products[0].Id);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetProductAsync_ById_RequestedResourceNotFoundException(
            [Frozen] Mock<IProductCatalogueContext> context,
            ProductCatalogueService service,
            IFixture fixture)
        {
            var products = fixture.CreateMany<CatalogueProduct>(5).ToList();
            context.Setup(x => x.Products).ReturnsEntitySet(products);
            Func<Task> act = async () => await service.GetProductAsync(0);

            act.Should().Throw<RequestedResourceNotFoundException>();
        }

        [Theory]
        [AutoMoqData]
        public async Task ProductCreateAsync_AddOneElement_ColectionCountSix(
            [Frozen] Mock<IProductCatalogueContext> context,
            ProductCatalogueService service,
            IFixture fixture)
        {
            var products = fixture.CreateMany<CatalogueProduct>(5).ToList();
            var product = fixture.Create<UpdateProductRequest>();
            context.Setup(x => x.Products).ReturnsEntitySet(products);

            await service.CreateProductAsync(product);

            var result = await service.GetProductsAsync(0, 8);

            result.Count.Should().Be(6);
        }

        [Theory]
        [AutoMoqData]
        public async Task ProductCreateAsync_RequestedResourceHasConflictException(
            [Frozen] Mock<IProductCatalogueContext> context,
            ProductCatalogueService service,
            IFixture fixture)
        {
            var products = fixture.CreateMany<CatalogueProduct>(5).ToList();
            var product = new UpdateProductRequest { Code = products[0].Code };
            context.Setup(x => x.Products).ReturnsEntitySet(products);

            Func<Task> act = async () => await service.CreateProductAsync(product);

            act.Should().Throw<RequestedResourceHasConflictException>();
        }

        [Theory]
        [AutoMoqData]
        public async Task ProductUpdateAsync_UpdateOneItem_ReturnOneItem(
            [Frozen] Mock<IProductCatalogueContext> context,
            ProductCatalogueService service,
            IFixture fixture)
        {
            var products = fixture.CreateMany<CatalogueProduct>(5).ToList();
            var updateProduct = fixture.Create<UpdateProductRequest>();
            context.Setup(x => x.Products).ReturnsEntitySet(products);
            var result = await service.UpdateProductAsync(products[0].Id, updateProduct);
            result.Code.Should().Be(service.GetProductAsync(products[0].Id).Result.Code);
        }

        [Theory]
        [AutoMoqData]
        public async Task ProductUpdateAsync_RequestedResourceHasConflictException(
            [Frozen] Mock<IProductCatalogueContext> context,
            ProductCatalogueService service,
            IFixture fixture)
        {
            var products = fixture.CreateMany<CatalogueProduct>(5).ToList();
            var updateProduct = fixture.Create<UpdateProductRequest>();
            updateProduct.Code = products[1].Code;
            context.Setup(x => x.Products).ReturnsEntitySet(products);
            Func<Task> act = async () => await service.UpdateProductAsync(products[0].Id, updateProduct);

            act.Should().Throw<RequestedResourceHasConflictException>();
        }

        [Theory]
        [AutoMoqData]
        public async Task ProductUpdateAsync_RequestedResourceNotFoundException(
            [Frozen] Mock<IProductCatalogueContext> context,
            ProductCatalogueService service,
            IFixture fixture)
        {
            var products = fixture.CreateMany<CatalogueProduct>(5).ToList();
            var updateProduct = fixture.Create<UpdateProductRequest>();
            context.Setup(x => x.Products).ReturnsEntitySet(products);
            Func<Task> act = async () => await service.UpdateProductAsync(0, updateProduct);

            act.Should().Throw<RequestedResourceNotFoundException>();
        }

        [Theory]
        [AutoMoqData]
        public async Task DeleteProductAsync_DeleteOneElement(
            [Frozen] Mock<IProductCatalogueContext> context,
            ProductCatalogueService service,
            IFixture fixture)
        {
            var products = fixture.CreateMany<CatalogueProduct>(5).ToList();
            products[0].IsDeleted = true;
            var id = products[0].Id;
            context.Setup(x => x.Products).ReturnsEntitySet(products);

            await service.DeleteProductAsync(id);

            Func<Task> act = async () => await service.GetProductAsync(id);
            act.Should().Throw<RequestedResourceNotFoundException>();
        }

        [Theory]
        [AutoMoqData]
        public async Task DeleteProductAsync_RequestedResourceNotFoundException(
            [Frozen] Mock<IProductCatalogueContext> context,
            ProductCatalogueService service,
            IFixture fixture)
        {
            var products = fixture.CreateMany<CatalogueProduct>(5).ToList();
            products[0].IsDeleted = true;
            context.Setup(x => x.Products).ReturnsEntitySet(products);

            Func<Task> act = async () => await service.DeleteProductAsync(0);
            act.Should().Throw<RequestedResourceNotFoundException>();
        }

        [Theory]
        [AutoMoqData]
        public async Task DeleteProductAsync_RequestedResourceHasConflictException(
            [Frozen] Mock<IProductCatalogueContext> context,
            ProductCatalogueService service,
            IFixture fixture)
        {
            var products = fixture.CreateMany<CatalogueProduct>(5).ToList();
            products[0].IsDeleted = false;
            context.Setup(x => x.Products).ReturnsEntitySet(products);

            Func<Task> act = async () => await service.DeleteProductAsync(products[0].Id);
            act.Should().Throw<RequestedResourceHasConflictException>();
        }

        [Theory]
        [AutoMoqData]
        public async Task SetStatusProductAsync_SetDeleteStatus(
            [Frozen] Mock<IProductCatalogueContext> context,
            ProductCatalogueService service,
            IFixture fixture)
        {
            var products = fixture.CreateMany<CatalogueProduct>(5).ToList();
            context.Setup(x => x.Products).ReturnsEntitySet(products);
            await service.SetStatusAsync(products[0].Id, true);

            Func<Task> act = async () => await service.DeleteProductAsync(products[0].Id);

            act.Should().NotThrow();
        }

        [Theory]
        [AutoMoqData]
        public async Task SetStatusProductAsync_RequestedResourceNotFoundException(
            [Frozen] Mock<IProductCatalogueContext> context,
            ProductCatalogueService service,
            IFixture fixture)
        {
            var products = fixture.CreateMany<CatalogueProduct>(5).ToList();
            context.Setup(x => x.Products).ReturnsEntitySet(products);
            Func<Task> act = async () => await service.SetStatusAsync(0, true);

            act.Should().Throw<RequestedResourceNotFoundException>();
        }
    }
}
