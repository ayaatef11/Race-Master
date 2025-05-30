﻿using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using RunGroop.Data.Models.Data;
using RunGroop.Repository.Interfaces;

namespace RunGroopWebApp.Services
{
    public class ClubType : ObjectGraphType<Club>
    {
        public ClubType()
        {
            Field(x => x.Id);
            Field(x => x.Title);
            Field(x => x.Description);
            Field(x => x.Image);
        }
    }
    public class ClubQuery : ObjectGraphType
    {
        public ClubQuery(IClubRepository productProvider)
        {

            Field<ListGraphType<ClubType>>(Name = "Clubs", resolve: x => productProvider.GetAllStates());
            Field<ClubType>(Name = "Clubs",
                      arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
                      resolve: x => productProvider.GetAllStates().Result);
        }
    }
    public class ClubSchema : Schema
    {

        public ClubSchema(IServiceProvider serviceProvider) : base(serviceProvider)
        {

            Query = serviceProvider.GetRequiredService<ClubQuery>();
        }
    }
}

