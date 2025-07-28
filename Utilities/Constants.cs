namespace BookStoreEcommerce.Utilities
{
    public static class Constants
    {
        // Session Keys
        public const string SessionKeyUser = "CurrentUser";
        public const string SessionKeyCart = "ShoppingCart";

        // Default Values
        public const int DefaultPageSize = 10;
        public const int LowStockThreshold = 10;

        // File Upload
        public const long MaxFileSize = 5 * 1024 * 1024; // 5MB
        public static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png", ".gif" };

        // Order Status
        public const string OrderStatusPending = "Pending";
        public const string OrderStatusProcessing = "Processing";
        public const string OrderStatusShipped = "Shipped";
        public const string OrderStatusDelivered = "Delivered";
        public const string OrderStatusCancelled = "Cancelled";

        // Payment Status
        public const string PaymentStatusPending = "Pending";
        public const string PaymentStatusPaid = "Paid";
        public const string PaymentStatusFailed = "Failed";
        public const string PaymentStatusRefunded = "Refunded";

        // User Types
        public const string UserTypeAdmin = "Admin";
        public const string UserTypeCustomer = "Customer";

        // Error Messages
        public const string ErrorGeneral = "An error occurred. Please try again.";
        public const string ErrorNotFound = "The requested item was not found.";
        public const string ErrorUnauthorized = "You are not authorized to perform this action.";
        public const string ErrorInvalidInput = "Please check your input and try again.";

        // Success Messages
        public const string SuccessCreate = "Item created successfully.";
        public const string SuccessUpdate = "Item updated successfully.";
        public const string SuccessDelete = "Item deleted successfully.";
    }
}