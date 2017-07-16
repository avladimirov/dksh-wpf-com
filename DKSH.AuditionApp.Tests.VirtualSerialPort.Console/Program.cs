using Com0Com.CSharp;
using System;
using System.Threading.Tasks;

namespace DKSH.AuditionApp.Tests.VirtualSerialPort
{
    class Program
    {
        private static readonly Com0ComSetupCFacade setupCFacade = new Com0ComSetupCFacade();
        private static CrossoverPortPair virtualPair;
        private static string virtualPortName = "COM-B";

        [STAThread]
        static void Main(string[] args)
        {
            // Create virtual COM pair if necessary
            Task.Run(CreatePairAsync).Wait();

            // open virtual port to simulate a responder
            PortSimulator.SetupPort(virtualPortName);

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
            virtualPair = await setupCFacade.CreatePortPairAsync(virtualPortName, "COM-A");

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
        
    }
}
