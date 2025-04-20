using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Compute;
using Azure.ResourceManager.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitoBot
{
    internal class AzureGameService
    {
        static private readonly string RG_NAME = Environment.GetEnvironmentVariable("RG_NAME");
        static private readonly string VM_NAME = Environment.GetEnvironmentVariable("VM_NAME");

        VirtualMachineResource _vmMinecraft;

        public AzureGameService()
        {
            Console.WriteLine("Azure Game Service initialized.");
            Console.WriteLine("Connecting to Azure...");

            ArmClient client = new ArmClient(new DefaultAzureCredential());
            ResourceGroupResource resourceGroup = client.GetDefaultSubscription().GetResourceGroup(RG_NAME);

            _vmMinecraft = resourceGroup.GetVirtualMachine(VM_NAME).Value;

            Console.WriteLine($"Connected to Minecraft VM: {_vmMinecraft.Data.Name}");
        }

        public async Task StartMinecraftServer()
        {
            Console.WriteLine("Starting Minecraft server...");

            _vmMinecraft.PowerOnAsync(Azure.WaitUntil.Started);

            Console.WriteLine("Minecraft server started.");
        }

        public async Task StopMinecraftServer()
        {
            Console.WriteLine("Stopping Minecraft server...");

            _vmMinecraft.PowerOffAsync(Azure.WaitUntil.Started);

            Console.WriteLine("Minecraft server stopped.");
        }
    }
}