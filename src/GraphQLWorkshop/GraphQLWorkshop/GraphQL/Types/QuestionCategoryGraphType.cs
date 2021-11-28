using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.EntityFramework;
using GraphQLWorkshop.Data.Contexts;
using GraphQLWorkshop.Data.Entities;

namespace GraphQLWorkshop.GraphQL.Types
{
    [GraphQLMetadata(nameof(QuestionCategory))]
    public class QuestionCategoryGraphType : EfObjectGraphType<AppDbContext, QuestionCategory>
    {
        public QuestionCategoryGraphType(IEfGraphQLService<AppDbContext> graphQlService) : base(graphQlService)
        {
            Name = nameof(QuestionCategory);
            this.AuthorizeWith("QuestionCategory.Read");
            AutoMap();
        }
    }
}
