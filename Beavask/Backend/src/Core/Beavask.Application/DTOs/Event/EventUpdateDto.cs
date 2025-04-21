using System;
using System.ComponentModel.DataAnnotations;

namespace Beavask.Application.DTOs.Event;

public class EventUpdateDto
{
    [Required]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    public string Description { get; set; } = string.Empty;
    
    [Required]
    public DateTime StartDate { get; set; }
    
    [Required]
    public DateTime EndDate { get; set; }
    
    [Required]
    public string Location { get; set; } = string.Empty;
    
    [Required]
    public int TeamId { get; set; }
}
