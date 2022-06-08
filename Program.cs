using Celin.AIS;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Celin
{
    class Program
    {
        static AIS.Server E1 { get; set; }
        async static Task Test1()
        {
            // Get PO's
            var zjde0001 = await E1.RequestAsync<AIS.PoResponse<T01012>>(new AIS.PoRequest
            {
                applicationName = "P01012",
                version = "ZJDE0002"
            });
            Console.WriteLine(zjde0001);

            // Create an AB Form Request
            var ab = new AIS.FormRequest()
            {
                outputType = AIS.Request.GRID_DATA,
                formName = "P01012_W01012B",
                version = "ZJDE0002",
                formServiceAction = "R",
                maxPageSize = "500",
                // Create Form Actions
                formActions = new List<AIS.Action>
                    {
                        // Set the Search Type to "C"
                        new AIS.FormAction() { controlID = "54", command = AIS.FormAction.SetControlValue, value = "C" },
                       // Press the Find Button
                        new AIS.FormAction { controlID = "15", command = AIS.FormAction.DoAction }
                    }
            };

            // Create a 500ms Cancel Object
            var cancel = new CancellationTokenSource(50000);
            // Submit the Form Request with a Generic Response Object
            var genRsp = await E1.RequestAsync<object>(ab, cancel.Token);
            // Request successful, dumpt the output to the Console
            Console.WriteLine(genRsp);

            // Limit the response to Grid Columns Number and Name
            ab.returnControlIDs = "1[19,20]";

            // Submit the form Request with our AB class definition
            AddressBookForm abRsp = await E1.RequestAsync<AddressBookForm>(ab);
            // Print the Grid Items to the Console
            foreach (var r in abRsp.fs_P01012_W01012B.data.gridData.rowset)
            {
                Console.WriteLine("{0, 12} {1}", r.z_AN8_19, r.z_ALPH_20);
            }
        }
        static async Task Test2()
        {
            var rs = await E1.RequestAsync(new AIS.DiscoverUBERequest
            {
                reportName = "R43500",
                reportVersion = "XJDE0001"
            });
            foreach (var c in rs.dataSelectionColumns)
            {
                Console.WriteLine("{0} {1}.{2}", c.view, c.table, c.dictItem);
            }
            foreach (var p in rs.poPrompt.tabPages)
            {
                Console.WriteLine("Page {0} - {1}", p.pageNumber, p.title);
                foreach (var c in p.controls)
                {
                    var po = rs.poValues.SingleOrDefault(p => p.id == c.idObject);
                    Console.WriteLine("{0, -60} {1, -30}", c.title, po.value);
                }
            }
        }
        static async Task Test3()
        {
            var rs = await E1.RequestAsync(new AIS.LaunchUBERequest
            {
                reportName = "R01401",
                reportVersion = "ZJDE0001",
                dataSelection = new AIS.DataSelection
                {
                    criteria = new[]
                    {
                            new AIS.Criteria
                            {
                                // Select with the F0101.AN8 field
                                subject = new AIS.Subject
                                {
                                    view = "V0101C",
                                    table = "F0101",
                                    dictItem = "AN8"
                                },
                                comparisonType = AIS.Condition.EQUAL,
                                // Where Equals 6002
                                predicate = new AIS.Predicate
                                {
                                    literalType = AIS.Predicate.SINGLE,
                                    values = new []
                                    {
                                        "6002"
                                    }
                                }
                            }
                        }
                }
            });
            Console.WriteLine("Job {0} Completed Successfully", rs.jobNumber);
        }
        static async Task Test4()
        {
            var rs = await E1.RequestAsync<JsonElement>(new FormRequest
            {
                formName = "P0901_W0901H",
                returnControlIDs = "1[39]",
                formActions = new[]
                {
                    new FormAction
                    {
                        controlID = "45",
                        command = FormAction.DoAction
                     }
                }
            });
            var rows = rs.GetProperty("fs_P0901_W0901H").GetProperty("data").GetProperty("gridData").GetProperty("rowset").EnumerateArray();
            Console.WriteLine($"Returned {rows.Count()} rows!");
        }
        static async Task Test5()
		{
            var rs = await E1.TableRequestAsync<JsonElement>("f0101", new[] { "f0101.an8", "f0101.alph" }, new[] { ("f0101.at1", "eq", "C") });
		}
        static async Task Test6()
        {
            var rs = await E1.RequestAsync<JsonElement>(new AIS.FormRequest
            {
                formName = "P564111_W564111A",
                formServiceDemo = "TRUE",
                showActionControls = true
            });
        }
        async static Task Main(string[] args)
        {
            // Initialise the Logger
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .SetMinimumLevel(LogLevel.Trace)
                    .AddConsole();
            });
            ILogger logger = loggerFactory.CreateLogger<Program>();
            // Initalise E1 Server
            // E1 = new AIS.Server("https://jdedv.steltix.com/jderest/v2/", logger);
            E1 = new AIS.Server("https://presentation2.steltix.com/jderest/v2/", logger);
            // E1 = new AIS.Server("https://ua920-orch.brickworks.com.au/jderest/v2/", logger);
            // Set the authentication parameters
            E1.AuthRequest.deviceName = "aisTest";
            E1.AuthRequest.username = "bragasonf";
            E1.AuthRequest.password = "bragasonf";
            // E1.AuthRequest.username = "SANDBOX";
            // E1.AuthRequest.password = "steltix19c";

            //e1.SetBasicAuthentication("demo", "demo");

            try
            {
                // Authenticate
                //await e1.AuthenticateBasicAsync("demo", "demo");
                //await E1.AuthenticateAsync();
                //Console.WriteLine("{0} Logged on {1}!", E1.AuthResponse.userInfo.alphaName, E1.AuthResponse.environment);

                await Test2();

                Console.ReadKey();
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine("Cancelled: {0}", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed!\n{0}", e.Message);
            }
            await E1.LogoutAsync();
        }
    }
}
