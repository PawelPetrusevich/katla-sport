using System;
using KatlaSport.DataAccess.ProductCatalogue;
using KatlaSport.Services.CatalogueManagement;
using Moq;
using Xunit;

namespace KatlaSport.Services.Tests.CatalogueManagement
{
    using AutoMapper;

    public class CatalogueManagementServiceTests
    {
        public CatalogueManagementServiceTests()
        {
            Mapper.Reset();
        }

        [Fact]
        public void CreateCatalogueManagementServiceWithNullAsParameterTest()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new CatalogueManagementService(null));

            Assert.Equal(typeof(ArgumentNullException), ex.GetType());
        }

        [Fact]
        public void GetProductCategories_EmptyCollection_Test()
        {
            var context = new Mock<IProductCatalogueContext>();
            context.Setup(c => c.Categories).ReturnsEntitySet(new ProductCategory[0]);

            var service = new CatalogueManagementService(context.Object);

            var categories = service.GetProductCategories();

            Assert.Equal(0, categories.Count);
        }

        [Fact]
        public void AddProductCategoryTest()
        {
            var context = new Mock<IProductCatalogueContext>();
            var service = new CatalogueManagementService(context.Object);

            service.AddProductCategory();
        }
    }
}
