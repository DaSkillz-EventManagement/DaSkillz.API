namespace Domain.Enum.Payment
{
    public enum TransactionStatus
    {
        SUCCESS = 1,        // Thành công (Successful)
        FAIL = 2,           // Thất bại (Error)
        PROCESSING = 3      // Đơn hàng chưa thanh toán hoặc giao dịch đang xử lý (Pending)
    }

}
