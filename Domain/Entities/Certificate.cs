using System;

namespace Domain.Entities;

public class Certificate
{
    public int CertificateID { get; set; } 
    public Guid UserId { get; set; }  
    public Guid EventId { get; set; }  
    public DateTime IssueDate { get; set; }
    public virtual Participant? Participant { get; set; }  
    public virtual Event? Event { get; set; } 
}
