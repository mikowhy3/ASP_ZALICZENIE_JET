using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP_ZALICZENIE_JET.Models.Movies;

public partial class Person
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PersonId { get; set; }

    public string? PersonName { get; set; }
    
    // Kolekcja związana z relacją z MovieCast
    public ICollection<MovieCast> MovieCasts { get; set; } = new List<MovieCast>();

    // Kolekcja związana z relacją z Movie (pośrednio przez MovieCast)
    public ICollection<Movie> Movies { get; set; } = new List<Movie>();
}