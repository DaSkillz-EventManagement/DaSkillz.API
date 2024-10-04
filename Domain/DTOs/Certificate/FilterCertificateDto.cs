namespace Domain.DTOs.Certificate
{
    public class FilterCertificateDto
    {
        public Guid? userId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Status { get; set; }
        public CertificateResponseDto? Certificate { get; set; }
    }
}
