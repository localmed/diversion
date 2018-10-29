using AutoFixture.Xunit2;
using AutoFixture.AutoMoq;
using AutoFixture;

namespace Diversion.Test
{
    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute()
            : base(() => new Fixture().Customize(new AutoMoqCustomization()))
        {
        }
    }

}
