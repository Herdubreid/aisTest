using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Celin.AIS;
using Microsoft.Extensions.Logging;

namespace aisTest
{
    class Program
    {
        async static Task Main(string[] args)
        {
            // Initialise the Logger
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .SetMinimumLevel(LogLevel.Debug)
                    .AddConsole();
            });
            ILogger logger = loggerFactory.CreateLogger<Program>();
            // Initalise E1 Server
            var e1 = new Server("http://demo.steltix.com/jderest/v2/", logger);
            // Set the authentication parameters
            e1.AuthRequest.deviceName = "aisTest";
            e1.AuthRequest.username = "DEMO";
            e1.AuthRequest.password = "DEMO";

            try
            {
                // Authenticate
                await e1.AuthenticateAsync();
                Console.WriteLine("{0} Logged on {1}!", e1.AuthResponse.userInfo.alphaName, e1.AuthResponse.environment);

                // Create an AB Form Request
                var ab = new FormRequest()
                {
                    formName = "P01012_W01012B",
                    version = "ZJDE0001",
                    formServiceAction = "R",
                    maxPageSize = "10",
                    // Create Form Actions
                    formActions = new List<Celin.AIS.Action>
                    {
                        // Set the Search Type to "C"
                        new FormAction() { controlID = "54", command = FormAction.SetControlValue, value = "C" },
                       // Press the Find Button
                        new FormAction() { controlID = "15", command = FormAction.DoAction }
                    }
                };

                // Submit the Form Request with a Generic Response Object
                var genRsp =  await e1.RequestAsync<object>(ab);
                // Request successful, dumpt the output to the Console
                Console.WriteLine(genRsp);

                // Limit the response to Grid Columns Number and Name
                ab.returnControlIDs = "1[19,20]";

                // Submit the form Request with our AB class definition
                AddressBookForm abRsp = await e1.RequestAsync<AddressBookForm>(ab);
                // Print the Grid Items to the Console
                foreach (var r in abRsp.fs_P01012_W01012B.data.gridData.rowset)
                {
                    Console.WriteLine("{0, 12} {1}", r.z_AN8_19, r.z_ALPH_20);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed!\n{0}", e.Message);
            }
        }
    }
}
