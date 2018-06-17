using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace KatlaSport.Services.Tests
{
    using System.Linq;

    /// <summary>
    /// Represents a data attribute for theory-based tests that uses Moq for creating fake objects.
    /// </summary>
    public sealed class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute()
            : base(
                () =>
                    {
                        var fixture = new Fixture().Customize(new AutoMoqCustomization());
                        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                            .ForEach(b => fixture.Behaviors.Remove(b));
                        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
                        return fixture;
                    })
        {
        }
    }
}
