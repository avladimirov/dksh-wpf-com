using Com0Com.CSharp;
using System;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;

namespace DKSH.AuditionApp.Tests.VirtualSerialPort
{
    class Program
    {
        private static readonly Com0ComSetupCFacade setupCFacade = new Com0ComSetupCFacade();
        private static CrossoverPortPair virtualPair;
        private static string virtualPortName = "COM-A";


        static void Main(string[] args)
        {
            // Create virtual COM pair if necessary
            Task.Run(CreatePairAsync).Wait();

            var vPort = SetupPort(virtualPortName);

            Console.WriteLine("Press key to exit and delete virtual ports");
            Console.ReadKey();

            // cleanup virtual COM ports
            Task.Run(DeletePairAsync).Wait();
        }

        private static async Task CreatePairAsync()
        {
            Console.WriteLine("Pre-existing virtual crossover port pairs:");
            var preExistingPortPairs = await setupCFacade.GetCrossoverPortPairsAsync();
            foreach (var pp in preExistingPortPairs)
            {
                Console.WriteLine($"Virtual Port Pair: {pp.PairNumber}({pp.PortNameA}) <-> {pp.PairNumber}({pp.PortNameB})");
            }
            Console.WriteLine();

            // Create some new virtual com port pairs
            virtualPair = await setupCFacade.CreatePortPairAsync(virtualPortName, "COM-B");

            Console.WriteLine("Virtual crossover port pairs after creation:");
            var portPairsAfterCreation = await setupCFacade.GetCrossoverPortPairsAsync();
            foreach (var pp in portPairsAfterCreation)
            {
                Console.WriteLine($"Virtual Port Pair: {pp.PairNumber}({pp.PortNameA}) <-> {pp.PairNumber}({pp.PortNameB})");
            }
            Console.WriteLine();
        }

        private static async Task DeletePairAsync()
        {
            if (virtualPair == null) return;
            //Remove the virtual com port pairs that we created
            await setupCFacade.DeletePortPairAsync(virtualPair.PairNumber);

            Console.WriteLine("Virtual crossover port pairs after removal:");
            var portPairsAfterDelete = await setupCFacade.GetCrossoverPortPairsAsync();
            foreach (var pp in portPairsAfterDelete)
            {
                Console.WriteLine($"Virtual Port Pair: {pp.PairNumber}({pp.PortNameA}) <-> {pp.PairNumber}({pp.PortNameB})");
            }
        }

        static SerialPort SetupPort(string portName)
        {
            var port = new SerialPort(portName);
            port.DataReceived += (s, args) =>
            {
                var strData = port.ReadExisting();
                if (strData == null) return;

                // process by simulating specification
                if (strData.Length == 1 && strData.First() == 'i')
                    port.Write("a");

            };
            port.Open();
            return port;
        }

        static void p_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Console.WriteLine((sender as SerialPort).ReadExisting());
        }
    }
}
