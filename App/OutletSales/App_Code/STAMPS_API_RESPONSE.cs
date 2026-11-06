using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for STAMPS_API_RESPONSE
/// </summary>
public class STAMPS_API_RESPONSE
{
    /*
         *  Class Response digunakan untuk melakukan `DeserializeObject` dari hasil response JSON.
         *  Silahkan menambahkan atau mengurangi `Response` atas method-method lain.
         *  Perlu diperhatikan, tipe data harus sesuai dengan response JSON.
         *  
         *  Cara penggunaan : 
         *  1. Silahkan panggil `using Newtonsoft.Json;`
         *  2. Hasil dari return JSON akan dilakukan `DeserializeObject`.
         *     sebagai contoh : return json disimpan dalam variable `result`
         *     
         *     var obj = JsonConvert.DeserializeObject<ResponseMembership>(result.ToString());
         *     
         *     Notes :  1. `obj` sudah dalam bentuk object, bukan JSON string.
         *              2. Cara menggunakan, sebagai contoh : `obj.membership.status`
         */

    #region "Response Existing Transaction ID"
    public class ResponseExistingTransactionID
    {
        public Int64 existing_transaction_id;
    }
    #endregion

    #region "Response Membership"
    public class ResponseMembership
    {
        public ResponseMembership.Membership membership;
        public ResponseMembership.User user;

        public class Membership
        {
            public string[] tags;
            public Int32? status = 0;
            public string status_text;
            public Int32? stamps = 0;
            public Int32? balance = 0;
            public bool is_blocked;
            public string referral_code;
            public string start_date;
            public string created;
        }

        public class User
        {
            public string id;
            public string name;
            public string gender;
            public string address;
            public bool is_active;
            public string email;
            public string picture_url;
            public string birthday;
            public string phone;
            public bool protected_redemption;
            public bool has_incorrect_email;
            public string[] member_ids;
            public string primary_member_id;
        }
    }
    #endregion

    #region "Response Add Transaction"
    public class ResponseAddTransaction
    {
        public ResponseAddTransaction.Transaction transaction;
        public ResponseAddTransaction.Customer customer;

        public class Transaction
        {
            public Int32 id;
            public double? value = 0;
            public Int32? stamps_earned = 0;
            public int? number_of_people = 0;
            public double? discount = 0;
        }

        public class Customer
        {
            public Int32 id;
            public string mobile_phone;
            public Int32? stamps_remaining = 0;
            public string status;
            public Int32? balance = 0;
        }
    }
    #endregion

    #region "Response Cancel Transaction"
    public class ResponseCancelTransaction
    {
        public ResponseCancelTransaction.Transaction transaction;
        public ResponseCancelTransaction.Customer customer;

        public class Transaction
        {
            public Int32? stamps_earned = 0;
            public Int32 id;
            public double? value = 0;
            public string status;
        }

        public class Customer
        {
            public string status;
            public Int32 id;
            public string mobile_number;
            public Int32? stamps_remaining = 0;
        }
    }
    #endregion

    #region "Response Modify Transaction"
    public class ResponseModifyTransaction
    {
        public ResponseModifyTransaction.Transaction transaction;
        public ResponseModifyTransaction.Customer customer;

        public class Transaction
        {
            public Int32 id;
            public double? value = 0;
            public Int32? stamps_earned = 0;
            public int? number_of_people = 0;
        }

        public class Customer
        {
            public Int32 id;
            public string mobile_phone;
            public Int32? stamps_remaining = 0;
            public string status;
            public Int32? balance = 0;
        }
    }
    #endregion

    #region "Response Member Suggestions"
    public class ResponseMemberSuggestions
    {
        public ResponseMemberSuggestions.Suggestions[] suggestions;

        public class Suggestions
        {
            public Int32 id;
            public string member_id;
            public string[] member_ids;
            public string name;
            public string birthday;
            public string address;
            public int? gender = null;
            public string email;
            public string phone;
            public Int32? stamps = 0;
            public string membership;
            public Int32? balance = 0;
            public bool is_active;
            public string identifier;
        }
    }
    #endregion

    #region "Response Validate Voucher Code"
    public class ResponseValidateVoucherCode
    {
        public ResponseValidateVoucherCode.Voucher voucher;
        public ResponseValidateVoucherCode.User user;
        public bool is_redeemable;

        public class Voucher
        {
            public Int32 id;
            public string name;
            public dynamic extra_data;
            public string validity;
            public string start_date;
            public string end_date;
        }

        public class User
        {
            public string name;
            public string email;
            public string phone;
        }
    }
    #endregion

    #region "Response Redeem Voucher Code"
    public class ResponseRedeemVoucherCode
    {
        public ResponseRedeemVoucherCode.Redemption redemption;
        public ResponseRedeemVoucherCode.Voucher voucher;

        public class Redemption
        {
            public Int32 id;
        }

        public class Voucher
        {
            public Int32 id;
            public string name;
            public dynamic extra_data;
        }
    }
    #endregion

	#region Response Add Transaction With Redeem Multiple Voucher Code

	public class ResponseAddTransactionWithRedeemMultipleVoucherCode
	{
		public Membership membership { get; set; }
		public Transaction transaction { get; set; }
		public List<Redemptions> redemptions { get; set; }

		public class Membership
		{
			public List<string> tags;
			public int? status = 0;
			public string status_text;
			public int? stamps = 0;
			public int? balance = 0;
			public bool is_blocked;
			public string referral_code;
			public DateTime start_date;
			public DateTime created;
		}

		public class Transaction
		{
			public int id;
			public int? stamps_earned = 0;
			public double? value = 0;
			public int? number_of_people = 0;
			public double? discount = 0;
		}

		public class Redemptions
		{
			public int id;
			public string code = "";
			public Voucher voucher;
		}

		public class Voucher
		{
			public int id;
			public string code = "";
		}
	}

	#endregion

    #region "Response Cancel Redemptions"
    public class ResponseCancelRedemptions
    {
        public ResponseCancelRedemptions.Redemption redemption;
        public ResponseCancelRedemptions.Customer customer;

        public class Redemption
        {
            public Int32 id;
            public string status;
        }

        public class Customer
        {
            public string status;
            public Int32 id;
            public Int32? stamps_remaining = 0;
        }
    }
    #endregion
    #region "Response Get Membership by Code"
    public class ResponseGetMembershipByCode
    {
        public ResponseGetMembershipByCode.Membership membership;
        public ResponseGetMembershipByCode.User user;

        public class Membership
        {
            public string[] tags;
            public Int32? status = 0;
            public string status_text;
            public Int32? stamps = 0;
            public Int32? balance = 0;
            public bool is_blocked;
            public string referral_code;
            public string start_date;
            public string created;
        }

        public class User
        {
            public string id;
            public string name;
            public string gender;
            public string address;
            public bool is_active;
            public string email;
            public string picture_url;
            public string birthday;
            public string phone;
            public bool protected_redemption;
            public bool has_incorrect_email;
            public string[] member_ids;
        }
    }
     #endregion

    #region "Response Error"
    public class responseError
    {
        public string detail;
        public string error_message;
    }
    #endregion
}