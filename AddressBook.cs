using Celin.AIS;
namespace Celin
{
    public class AddressBookRow
    {
        public int      z_AN8_19 { get; set; }
        public string   z_ALPH_20 { get; set; }
    }
    public class AddressBookForm : FormResponse
    {
        public Form<FormData<AddressBookRow>> fs_P01012_W01012B { get; set; }
    }
}
