using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;
using GraphQLWorkshop.Data.Entities;
using GraphQLWorkshop.GraphQL.Mutations;
using GraphQLWorkshop.GraphQL.Queries;
using GraphQLWorkshop.GraphQL.Types;

namespace GraphQLWorkshop.GraphQL.Schemas
{
    public class QuestionSchema : Schema
    {
        public QuestionSchema(IServiceProvider serviceProvider, QuestionQuery query, QuestionMutation mutation) : base(serviceProvider)
        {
            RegisterTypeMapping(typeof(Question), typeof(QuestionGraphType));
            RegisterTypeMapping(typeof(QuestionCategory), typeof(QuestionCategoryGraphType));
            RegisterTypeMapping(typeof(QuestionAnswer), typeof(QuestionAnswerGraphType));
            RegisterTypeMapping(typeof(QuestionApprovalStatus), typeof(QuestionApprovalStatusGraphType));
            Query = query;
            //Mutation = mutation;
        }
    }
}
