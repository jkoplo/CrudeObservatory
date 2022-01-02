using CrudeObservatory.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CrudeObservatory.Triggers.Manual
{
    internal class ManualTrigger : ITrigger
    {
        const int STD_INPUT_HANDLE = -10;

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool CancelIoEx(IntPtr handle, IntPtr lpOverlapped);


        public Task InitializeAsync(CancellationToken stoppingToken) => Task.CompletedTask;
        public Task ShutdownAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }

        public Task WaitForTriggerAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Press any key to fire trigger");

            //Adapted from: https://stackoverflow.com/a/58475263
            using (stoppingToken.Register(() => CancelConsoleReadyKey()))
            {
                return Task.Run(() => ConsoleReadKeyCancellable());
            }
        }

        private void CancelConsoleReadyKey()
        {
            var handle = GetStdHandle(STD_INPUT_HANDLE);
            CancelIoEx(handle, IntPtr.Zero);
        }

        private static void ConsoleReadKeyCancellable()
        {
            try
            {
                // Start reading from the console
                var key = Console.ReadKey();
                Console.WriteLine("Key read");
            }
            // Handle the exception when the operation is canceled
            catch (InvalidOperationException)
            {
                Console.WriteLine("Operation canceled");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Operation canceled");
            }
        }
    }
}
