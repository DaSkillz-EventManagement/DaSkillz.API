namespace Domain.DTOs.Coupons
{
    public class CouponDto
    {
        public long ExpiredDate { get; set; }
        public int NOAttempts { get; set; }
        public string DiscountType { get; set; }
        public decimal Value { get; set; }
    }
}
