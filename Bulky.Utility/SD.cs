using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Utility
{
    public static class SD
    {
        public const string ROLE_CUSTOMER = "Customer";
        public const string ROLE_COMPANY = "Company";
        public const string ROLE_ADMIN = "Admin";
        public const string ROLE_EMPLOYEE = "Employee";

        public const string ORDER_STATUS_PENDING = "Pending";
        public const string ORDER_STATUS_APPROVED = "Approved";
        public const string ORDER_STATUS_PROCESSING = "Processing";
        public const string ORDER_STATUS_SHIPPED = "Shipped";
        public const string ORDER_STATUS_CANCELLED = "Cancelled";
        public const string ORDER_STATUS_REFUNDED = "Refunded";

        public const string PAYMENT_STATUS_PENDING = "Pending";
        public const string PAYMENT_STATUS_APPROVED = "Approved";
        public const string PAYMENT_STATUS_DELAYED_PAYMENT = "ApprovedForDelayedPayment";
        public const string PAYMENT_STATUS_REJECTED = "Rejected";


        // this status represent the status return by stripe,
        // refer: https://docs.stripe.com/api/checkout/sessions/object#checkout_session_object-payment_status
        public const string STRIPE_PAID = "paid";
        public const string STRIPE_NO_PAYMENT_REQUIRED = "no_payment_required";
        public const string STRIPE_UNPAID = "unpaid";

        public const string SESSION_CART = "SessionShoppingCart";
    }
}
