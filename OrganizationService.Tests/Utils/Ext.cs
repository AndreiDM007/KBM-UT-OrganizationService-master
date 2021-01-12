using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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
<<<<<<< Updated upstream

=======
>>>>>>> Stashed changes
        public static Mock<DbSet<T>> MockDbSet<T>(List<T> list) where T : class
        {
            var mock = new Mock<DbSet<T>>();
            mock.SetupIQueryable(list.AsQueryable());
            return mock;
        }
<<<<<<< Updated upstream
=======

>>>>>>> Stashed changes
    }
}