using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseMVC.Domain;
using NHibernate;
using NHibernate.Linq;
using BaseMVC.ViewModels;
using NHibernate.Transform;
using NHibernate.Criterion;

namespace BaseMVC.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(int pageSize, ISession session)
            : base(pageSize, session)
        {
        }
    }
}
