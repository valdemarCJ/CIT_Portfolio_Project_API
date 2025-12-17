using CIT_Portfolio_Project_API.Models.Entities;
using CIT_Portfolio_Project_API.Infrastructure.Persistence.PgSqlFunctionResults;
using Microsoft.EntityFrameworkCore;

namespace CIT_Portfolio_Project_API.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // IMDB read-only
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Rating> Ratings => Set<Rating>();
    public DbSet<MovieDetail> MovieDetails => Set<MovieDetail>();
    public DbSet<MovieGenre> MovieGenres => Set<MovieGenre>();
    public DbSet<MoviePerson> MoviePeople => Set<MoviePerson>();
    public DbSet<Person> Persons => Set<Person>();
    public DbSet<PersonProfession> PersonProfessions => Set<PersonProfession>();
    public DbSet<PersonKnownFor> PersonKnownFor => Set<PersonKnownFor>();
    public DbSet<WordIndex> WordIndex => Set<WordIndex>();

    // Framework CRUD
    public DbSet<User> Users => Set<User>();
    public DbSet<UserBookmark> UserBookmarks => Set<UserBookmark>();
    public DbSet<UserRating> UserRatings => Set<UserRating>();
    public DbSet<UserSearchHistory> UserSearchHistory => Set<UserSearchHistory>();
    public DbSet<UserPersonBookmark> UserPersonBookmarks => Set<UserPersonBookmark>();
    public DbSet<UserPersonRating> UserPersonRatings => Set<UserPersonRating>();

    // Function result sets (keyless)
    public DbSet<SearchRow> SearchRows => Set<SearchRow>();
    public DbSet<StructuredSearchRow> StructuredSearchRows => Set<StructuredSearchRow>();
    public DbSet<BookmarkRow> BookmarkRows => Set<BookmarkRow>();
    public DbSet<PopularActorRow> PopularActorRows => Set<PopularActorRow>();
    public DbSet<SimilarTitleRow> SimilarTitleRows => Set<SimilarTitleRow>();
    public DbSet<PersonWordRow> PersonWordRows => Set<PersonWordRow>();
    public DbSet<ExactMatchRow> ExactMatchRows => Set<ExactMatchRow>();
    public DbSet<BestMatchRow> BestMatchRows => Set<BestMatchRow>();
    public DbSet<CoPlayerRow> CoPlayerRows => Set<CoPlayerRow>();
    public DbSet<UserRatingHistoryRow> UserRatingHistoryRows => Set<UserRatingHistoryRow>();
    public DbSet<UserSearchHistoryRow> UserSearchHistoryRows => Set<UserSearchHistoryRow>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        
        modelBuilder.Entity<Movie>().HasKey(x => x.Tconst);
        modelBuilder.Entity<Rating>().HasKey(x => x.Tconst);
        modelBuilder.Entity<MovieDetail>().HasKey(x => x.Tconst);
        modelBuilder.Entity<MovieGenre>().HasKey(x => new { x.Tconst, x.Genre });
        modelBuilder.Entity<MoviePerson>().HasKey(x => new { x.Tconst, x.Nconst, x.Role, x.Ordering });
        modelBuilder.Entity<Person>().HasKey(x => x.Nconst);
        modelBuilder.Entity<PersonProfession>().HasKey(x => new { x.Nconst, x.Profession });
        modelBuilder.Entity<PersonKnownFor>().HasKey(x => new { x.Nconst, x.Tconst });
        modelBuilder.Entity<WordIndex>().HasKey(x => new { x.Tconst, x.Word, x.Field });

        modelBuilder.Entity<User>().HasKey(x => x.Id);
        modelBuilder.Entity<UserBookmark>().HasKey(x => new { x.UserId, x.Tconst });
        modelBuilder.Entity<UserRating>().HasKey(x => x.Id);
        modelBuilder.Entity<UserSearchHistory>().HasKey(x => x.Id);
        modelBuilder.Entity<UserPersonBookmark>().HasKey(x => new { x.UserId, x.Nconst });
        modelBuilder.Entity<UserPersonRating>().HasKey(x => new { x.UserId, x.Nconst });

        // Keyless types for function results (read-only projections from DB functions/views)
        modelBuilder.Entity<SearchRow>().HasNoKey();
        modelBuilder.Entity<StructuredSearchRow>().HasNoKey();
        modelBuilder.Entity<BookmarkRow>().HasNoKey();
        modelBuilder.Entity<PopularActorRow>().HasNoKey();
        modelBuilder.Entity<SimilarTitleRow>().HasNoKey();
        modelBuilder.Entity<PersonWordRow>().HasNoKey();
        modelBuilder.Entity<ExactMatchRow>().HasNoKey();
        modelBuilder.Entity<BestMatchRow>().HasNoKey();
        modelBuilder.Entity<CoPlayerRow>().HasNoKey();
        modelBuilder.Entity<UserRatingHistoryRow>().HasNoKey();
        modelBuilder.Entity<UserSearchHistoryRow>().HasNoKey();

        // Map entities to actual table/column names from the SQL scripts (keeps DB naming, idiomatic C# in entities)
        modelBuilder.Entity<Movie>(entity =>
        {
            entity.ToTable("movies");
            entity.Property(e => e.Tconst).HasColumnName("tconst");
            entity.Property(e => e.TitleType).HasColumnName("titletype");
            entity.Property(e => e.PrimaryTitle).HasColumnName("primarytitle");
            entity.Property(e => e.OriginalTitle).HasColumnName("originaltitle");
            entity.Property(e => e.IsAdult).HasColumnName("isadult");
            entity.Property(e => e.StartYear).HasColumnName("startyear");
            entity.Property(e => e.EndYear).HasColumnName("endyear");
            entity.Property(e => e.RuntimeMinutes).HasColumnName("runtimeminutes");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.ToTable("ratings");
            entity.Property(e => e.Tconst).HasColumnName("tconst");
            entity.Property(e => e.AverageRating).HasColumnName("averagerating");
            entity.Property(e => e.NumVotes).HasColumnName("numvotes");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.ToTable("persons");
            entity.Property(e => e.Nconst).HasColumnName("nconst");
            entity.Property(e => e.Name).HasColumnName("primaryname");
            entity.Property(e => e.BirthYear).HasColumnName("birthyear");
            entity.Property(e => e.DeathYear).HasColumnName("deathyear");
            entity.Property(e => e.NameRating).HasColumnName("namerating").HasColumnType("numeric(4,2)");
        });

        modelBuilder.Entity<PersonKnownFor>(entity =>
        {
            entity.ToTable("person_knownfor");
            entity.Property(e => e.Nconst).HasColumnName("nconst");
            entity.Property(e => e.Tconst).HasColumnName("tconst");
        });

        modelBuilder.Entity<MoviePerson>(entity =>
        {
            entity.ToTable("movie_people");
            entity.Property(e => e.Tconst).HasColumnName("tconst");
            entity.Property(e => e.Nconst).HasColumnName("nconst");
            entity.Property(e => e.Role).HasColumnName("role");
            entity.Property(e => e.Ordering).HasColumnName("ordering");
            entity.Property(e => e.Category).HasColumnName("category");
            entity.Property(e => e.Job).HasColumnName("job");
            entity.Property(e => e.Characters).HasColumnName("characters");
        });

        modelBuilder.Entity<MovieGenre>(entity =>
        {
            entity.ToTable("movie_genres");
            entity.Property(e => e.Tconst).HasColumnName("tconst");
            entity.Property(e => e.Genre).HasColumnName("genre");
        });

        modelBuilder.Entity<MovieDetail>(entity =>
        {
            entity.ToTable("movie_details");
            entity.Property(e => e.Tconst).HasColumnName("tconst");
            entity.Property(e => e.Awards).HasColumnName("awards");
            entity.Property(e => e.Plot).HasColumnName("plot");
            entity.Property(e => e.Rated).HasColumnName("rated");
            entity.Property(e => e.Poster).HasColumnName("poster");
            entity.Property(e => e.Boxoffice).HasColumnName("boxoffice");
            entity.Property(e => e.AkaTitles).HasColumnName("aka_titles");
            entity.Property(e => e.ParentTconst).HasColumnName("parenttconst");
            entity.Property(e => e.SeasonNumber).HasColumnName("seasonnumber");
            entity.Property(e => e.EpisodeNumber).HasColumnName("episodenumber");
        });

        modelBuilder.Entity<WordIndex>(entity =>
        {
            entity.ToTable("word_index");
            entity.Property(e => e.Tconst).HasColumnName("tconst");
            entity.Property(e => e.Word).HasColumnName("word");
            entity.Property(e => e.Field).HasColumnName("field");
            entity.Property(e => e.Lexeme).HasColumnName("lexeme");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.Property(e => e.Id).HasColumnName("user_id").ValueGeneratedOnAdd();
            entity.Property(e => e.Username).HasColumnName("username");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
            // DB requires email (NOT NULL). Registration pipeline ensures email is provided before insert.
        });

        modelBuilder.Entity<UserBookmark>(entity =>
        {
            entity.ToTable("user_bookmarks");
            entity.Ignore(e => e.Id);  // Table uses composite key (user_id, tconst), not auto-increment id
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Tconst).HasColumnName("tconst");
            entity.Property(e => e.Note).HasColumnName("note");
        });

        modelBuilder.Entity<UserPersonBookmark>(entity =>
        {
            entity.ToTable("user_person_bookmarks");
            entity.Ignore(e => e.Id);  // Table uses composite key (user_id, nconst), not auto-increment id
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Nconst).HasColumnName("nconst");
            entity.Property(e => e.Note).HasColumnName("note");
        });

        modelBuilder.Entity<UserPersonRating>(entity =>
        {
            entity.ToTable("user_person_ratings");
            entity.Ignore(e => e.Id);  // Table uses composite key (user_id, nconst), not auto-increment id
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Nconst).HasColumnName("nconst");
            entity.Property(e => e.Value).HasColumnName("rating");
        });
    }

    // Function-call helpers (using FromSqlInterpolated/ExecuteSqlInterpolated for DB functions)
    public IQueryable<SearchRow> CallStringSearch(int userId, string text)
        => SearchRows.FromSqlInterpolated($"select * from string_search({userId}, {text})");

    public IQueryable<StructuredSearchRow> CallStructuredStringSearch(int userId, string? title, string? plot, string? characters, string? person)
        => StructuredSearchRows.FromSqlInterpolated($"select * from structured_string_search({userId}, {title}, {plot}, {characters}, {person})");

    public async Task<int> ExecuteRateAsync(int userId, string tconst, int value, CancellationToken ct = default)
        => await Database.ExecuteSqlInterpolatedAsync($"select rate({userId}, {tconst}, {value})", ct);

    public IQueryable<BookmarkRow> CallGetBookmarks(int userId)
        => BookmarkRows.FromSqlInterpolated($"select * from get_bookmarks({userId})");

    public async Task<int> ExecuteAddBookmarkAsync(int userId, string tconst, string? note, CancellationToken ct = default)
        => await Database.ExecuteSqlInterpolatedAsync($"select add_bookmark({userId}, {tconst}, {note})", ct);

    // Analytics examples (read-only)
    public IQueryable<PopularActorRow> CallPopularActorsInMovie(string tconst)
        => PopularActorRows.FromSqlInterpolated($"select * from popular_actors_in_movie({tconst})");

    public IQueryable<SimilarTitleRow> CallSimilarMovies(string tconst)
        => SimilarTitleRows.FromSqlInterpolated($"select * from similar_movies({tconst})");

    public IQueryable<PersonWordRow> CallPersonWords(string name)
        => PersonWordRows.FromSqlInterpolated($"select * from person_words({name})");

    public IQueryable<CoPlayerRow> CallCoPlayers(string actor)
        => CoPlayerRows.FromSqlInterpolated($"select * from co_players({actor})");

    public IQueryable<ExactMatchRow> CallExactMatch(string? w1, string? w2 = null, string? w3 = null)
        => ExactMatchRows.FromSqlInterpolated($"select * from exact_match_query({w1}, {w2}, {w3})");

    public IQueryable<BestMatchRow> CallBestMatch(string? w1, string? w2 = null, string? w3 = null)
        => BestMatchRows.FromSqlInterpolated($"select * from best_match_query({w1}, {w2}, {w3})");

    // User activity (history)
    public IQueryable<UserRatingHistoryRow> CallUserRatingHistory(int userId)
        => UserRatingHistoryRows.FromSqlInterpolated($"select * from get_user_rating_history({userId})");

    public IQueryable<UserSearchHistoryRow> CallUserSearchHistory(int userId)
        => UserSearchHistoryRows.FromSqlInterpolated($"select * from get_user_search_history({userId})");

    // Framework user creation (delegates to DB function from Part 1)
    public async Task<int> ExecuteCreateUserAsync(string username, string email, string passwordHash, CancellationToken ct = default)
        => await Database.ExecuteSqlInterpolatedAsync($"select create_user({username}, {email}, {passwordHash})", ct);
}
