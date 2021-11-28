using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;
using GraphQL.EntityFramework;
using GraphQLWorkshop.Data.Contexts;

namespace GraphQLWorkshop.GraphQL.Queries
{
    public class QuestionQuery : QueryGraphType<AppDbContext>
    {
        public QuestionQuery(IEfGraphQLService<AppDbContext> efGraphQlService) :
            base(efGraphQlService)
        {
            AddQueryField(
                name: "questions",
                resolve: context => context.DbContext.Questions);
            AddQueryConnectionField(
                name: "questionsConnection",
                resolve: context => context.DbContext.Questions);
            AddSingleField(
                resolve: context => context.DbContext.Questions,
                name: "question");
            AddQueryField(
                name: "questionCategories",
                resolve: context => context.DbContext.QuestionCategories);
            AddQueryConnectionField(
                name: "questionCategoriesConnection",
                resolve: context => context.DbContext.QuestionCategories);
            AddSingleField(
                resolve: context => context.DbContext.QuestionCategories,
                name: "questionCategory");
            AddQueryField(
                name: "questionAnswers",
                resolve: context => context.DbContext.QuestionAnswers);
            AddQueryConnectionField(
                name: "questionAnswersConnection",
                resolve: context => context.DbContext.QuestionAnswers);
            AddSingleField(
                resolve: context => context.DbContext.QuestionAnswers,
                name: "questionAnswer");
        }
    }
}
