using System;
using System.Linq;
using System.Data;

public class Email 
{

    public static bool valid_Email(string email)
    {
        using (Map_AC ac = new Map_AC())
        {

            int count = ac.UserAccount.Count(p => p.Email == Str.CapLower(email));

            bool x = count == 0;

            return x;
            }
    }
    public static bool valid_Email2(string email)
    {
        bool x = false;
        if (Str.CapLower(email).Contains("@centralmegakencana.com")
			|| Str.CapLower(email).Contains("@centralmegakencana.co.id")
            || Str.CapLower(email).Contains("@frankncojewellery.com")
			|| Str.CapLower(email).Contains("@frankncojewellery.co.id")
            || Str.CapLower(email).Contains("@mondialjeweler.com")
			|| Str.CapLower(email).Contains("@mondialjeweler.co.id")
            || Str.CapLower(email).Contains("@palacejeweler.com")
			|| Str.CapLower(email).Contains("@palacejeweler.co.id")
            || Str.CapLower(email).Contains("@miss-mondial.com")
			|| Str.CapLower(email).Contains("@miss-mondial.co.id")
			|| Str.CapLower(email).Contains("@lakuemas.com")
			|| Str.CapLower(email).Contains("@frankandcojewellery.com")
            )
        {
            x = true;
        }

        return x;
    }
}