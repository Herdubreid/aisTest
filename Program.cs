using System;
using System.Collections.Generic;
using Celin.AIS;
using Newtonsoft.Json.Linq;

namespace aisTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initalise E1 Server
            var e1 = new Server("http://e1.celin.io:9300/jderest/");
            // Set the authentication parameters
            e1.AuthRequest.deviceName = "aisTest";
            e1.AuthRequest.username = "demo";
            e1.AuthRequest.password = "testing";

            // Authenticate
            if (e1.Authenticate())
            {
                Console.WriteLine("{0} Logged on {1}!", e1.AuthResponse.userInfo.alphaName, e1.AuthResponse.environment);

                // Create an AB Form Request
                var ab = new FormRequest()
                {
                    formName = "P01012_W01012B",
                    version = "ZJDE0001",
                    formServiceAction = "R",
                    maxPageSize = "10",
                    formActions = new List<Celin.AIS.Action>()
                };
                // Set the Search Type to "C"
                ab.formActions.Add(new FormAction() { controlID = "54", command = "SetControlValue", value = "C" });
                // Press the Find Button
                ab.formActions.Add(new FormAction() { controlID = "15", command = "DoAction" });

                // Submit the Form Request with a Generic Response Object
                var genRsp = e1.Request<JObject>(ab);
                if (genRsp.Item1)
                {
                    // Request successful, dumpt the output to the Console
                    Console.WriteLine(genRsp.Item2);
                }

                // Limit the response to Grid Columns Number and Name
                ab.returnControlIDs = "1[19,20]";

                // Submit the form Request with our AB class definition
                var abRsp = e1.Request<AddressBookForm>(ab);
                if (abRsp.Item1)
                {
                    // Print the Grid Items to the Console
                    foreach (var r in abRsp.Item2.fs_P01012_W01012B.data.gridData.rowset)
                    {
                        Console.WriteLine("{0, 12} {1}", r.mnAddressNumber_19.value, r.sAlphaName_20.value);
                    }
                }
            }
            else
            {
                Console.WriteLine("Authentication failed!");
            }
        }
    }
}
