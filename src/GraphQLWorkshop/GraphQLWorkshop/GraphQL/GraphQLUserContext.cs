using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GraphQL.Authorization;

namespace GraphQLWorkshop.GraphQL
{
    public class GraphQLUserContext : Dictionary<string, object>, IProvideClaimsPrincipal
    {
        public ClaimsPrincipal User { get; set; }
    }
}
