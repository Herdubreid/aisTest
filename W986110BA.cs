using System;

namespace Celin
{
    // Namespace for the Submitted Jobs Form
    namespace W986110BA
    {
        // The Grid Defintion
        public class Row
		{
			// Grid Columns
			public string z_JOBPTY_8 { get; set; }
			public string z_RPDSBTYP_71 { get; set; }
			public string z_PRINTW_98 { get; set; }
			public string z_JD_79 { get; set; }
			public string z_JOBQUE_6 { get; set; }
			public DateTime z_ACTDATE_19 { get; set; }
			public string z_PRTQ_58 { get; set; }
			public string z_VIEWOUTW_96 { get; set; }
			public string z_JOBSTS_7 { get; set; }
			public string z_FNDFUF2_22 { get; set; }
			public string z_ENHV_14 { get; set; }
			public DateTime z_SBMDATE_17 { get; set; }
			public string z_EXEHOST_11 { get; set; }
			public string z_TMLAAT_53 { get; set; }
			public string z_ORGHOST_15 { get; set; }
			public string z_JOBTYPE_61 { get; set; }
			public string z_FUNO_9 { get; set; }
			public string z_VERS_74 { get; set; }
			public string z_OBNM_73 { get; set; }
			public string z_FNDFUF1_103 { get; set; }
			public string z_DL01_47 { get; set; }
			public string z_MD_78 { get; set; }
			public decimal z_PROCESSID_16 { get; set; }
			public decimal z_JOBNBR_13 { get; set; }
			public string z_TMJBSB_52 { get; set; }
			public string z_USER_10 { get; set; }
		}
		// The Form Defintion
		public class FormData : AIS.FormData<Row>
		{
			// Form Field "User"
			public AIS.FormField<string> z_USER_29 { get; set; }
		}
		// Form Response
		public class Response : AIS.FormResponse
		{
			// Form Parameter
			public AIS.Form<FormData> fs_P986110B_W986110BA { get; set; }
			public AIS.Form<FormData> Form => fs_P986110B_W986110BA;
		}
		// Form Request
		public class Request : AIS.FormRequest
		{
			public Request()
			{
				// Form name to call
				formName = "P986110B_W986110BA";
				// Version
				version = "ZJDE0001";
				// Read-only
				formServiceAction = "R";
				// Return Maximum 10 rows
				maxPageSize = "10";
			}
		}
	}
}
