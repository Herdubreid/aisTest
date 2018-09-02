using Celin.AIS;
namespace aisTest
{
    public class AddressBookRow : Row
    {
        public Number mnAddressNumber_19 { get; set; }
        public String sAlphaName_20 { get; set; }
    }
    public class AddressBookForm : FormResponse
    {
        public Form<FormData<AddressBookRow>> fs_P01012_W01012B;
    }
}
