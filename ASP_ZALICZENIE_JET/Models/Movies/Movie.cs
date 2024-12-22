using System;
using System.Collections.Generic;

namespace ASP_ZALICZENIE_JET.Models.Movies;

public partial class Movie
{
    public int MovieId { get; set; }

    public string? Title { get; set; }

    public int? Budget { get; set; }

    public string? Homepage { get; set; }

    public string? Overview { get; set; }

    public double? Popularity { get; set; }

    public DateOnly? ReleaseDate { get; set; }

    public long? Revenue { get; set; }

    public int? Runtime { get; set; }

    public string? MovieStatus { get; set; }

    public string? Tagline { get; set; }

    public double? VoteAverage { get; set; }

    public int? VoteCount { get; set; }
    
    // dodanie danych dotyczacych aktorow
    public ICollection<MovieCast> MovieCasts { get; set; } // Powiązani aktorzy
    public ICollection<Person> Persons { get; set; }     // Powiązane osoby
}
