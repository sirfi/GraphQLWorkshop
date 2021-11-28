﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.EntityFramework;
using GraphQLWorkshop.Data.Contexts;
using GraphQLWorkshop.Data.Entities;

namespace GraphQLWorkshop.GraphQL.Types
{
    [GraphQLMetadata(nameof(Question))]
    public class QuestionGraphType : EfObjectGraphType<AppDbContext, Question>
    {
        public QuestionGraphType(IEfGraphQLService<AppDbContext> graphQlService) : base(graphQlService)
        {
            Name = nameof(Question);
            this.AuthorizeWith("Question.Read");
            AutoMap();
        }
    }
}
