using System.Linq;
using Moq;

namespace OrganizationService.Tests.Utils
{
    public static class Ext
    {
        public static void SetupIQueryable<T>(this Mock<T> mock, IQueryable queryable)
            where T : class, IQueryable
        {
            mock.Setup(r => r.GetEnumerator()).Returns(queryable.GetEnumerator());
            mock.Setup(r => r.Provider).Returns(queryable.Provider);
            mock.Setup(r => r.ElementType).Returns(queryable.ElementType);
            mock.Setup(r => r.Expression).Returns(queryable.Expression);
        }
    }
}