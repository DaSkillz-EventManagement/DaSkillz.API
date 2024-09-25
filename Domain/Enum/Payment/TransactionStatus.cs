namespace Domain.Enum.Payment
{
    /// <summary>
    /// Trạng thái giao dịch (Transaction Status)
    /// </summary>
    public enum TransactionStatus
    {
        /// <summary>
        /// Giao dịch thành công
        /// </summary>
        SUCCESS = 1, // Thành công

        /// <summary>
        /// Giao dịch thất bại
        /// </summary>
        FAIL = 2, // Thất bại

        /// <summary>
        /// Giao dịch đang xử lý
        /// </summary>
        PROCESSING = 3 // Đang xử lý
    }
}