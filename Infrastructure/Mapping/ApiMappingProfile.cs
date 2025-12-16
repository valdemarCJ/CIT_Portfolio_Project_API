using AutoMapper;
using CIT_Portfolio_Project_API.Application.DTOs;
using CIT_Portfolio_Project_API.Infrastructure.Persistence.PgSqlFunctionResults;
using CIT_Portfolio_Project_API.Models.Entities;

namespace CIT_Portfolio_Project_API.Infrastructure.Mapping;

public class ApiMappingProfile : Profile
{
    public ApiMappingProfile()
    {
        CreateMap<Movie, MovieDto>();
        CreateMap<MovieDetail, MovieDetailDto>();
        CreateMap<MovieGenre, MovieGenreDto>();
        CreateMap<MoviePerson, MoviePersonDto>();
        CreateMap<Person, PersonDto>();
        CreateMap<PersonKnownFor, PersonKnownForDto>();
        CreateMap<Rating, ImdbRatingDto>();
        CreateMap<User, UserDto>();
        CreateMap<WordIndex, WordIndexDto>();

        CreateMap<SearchRow, SearchDto>();
        CreateMap<BookmarkRow, BookmarkDto>();
    }
}
