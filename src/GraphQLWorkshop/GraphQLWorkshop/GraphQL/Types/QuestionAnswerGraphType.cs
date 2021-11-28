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
    [GraphQLMetadata(nameof(QuestionAnswer))]
    public class QuestionAnswerGraphType : EfObjectGraphType<AppDbContext, QuestionAnswer>
    {
        public QuestionAnswerGraphType(IEfGraphQLService<AppDbContext> graphQlService) : base(graphQlService)
        {
            Name = nameof(QuestionAnswer);
            this.AuthorizeWith("QuestionAnswer.Read");
            AutoMap();
        }
    }
}
