﻿using System.Runtime.CompilerServices;
using bottlenoselabs.C2CS.Runtime;
using Dojo;
using dojo_bindings;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using System.Text.Json;



internal class Example
{
    private static void OnEntityStateUpdate()
    {
        // React
        Console.WriteLine("Entity state updated");
    }

    unsafe static void Main()
    {
        // Use root directory to load the native library
        Environment.CurrentDirectory = "../";

        // Initialize world data
        var world = "0x5010c31f127114c6198df8a5239e2b7a5151e1156fb43791e37e7385faa8138";
        var player = "0x517ececd29116499f4a1b64b094da79ba08dfd54a3edaa316134c41f8160973";
        // Initialize entities
        var entities = new dojo.Keys[] { new dojo.Keys { model = "Position", keys = new string[] {
            player
        } } };

        // Throws an exception if the client cannot be created
        ToriiClient client = new ToriiClient("http://localhost:8080", "http://localhost:5050", world, entities);

        // Get the world metadata
        dojo.WorldMetadata worldMetadata = client.WorldMetadata();

        dojo.Ty entity = client.Entity(entities[0]);
        Console.WriteLine(entity.ty_struct.children[0].name);

        // Start the subscription
        client.StartSubscription();

        // Add our entities to the sync list
        client.AddEntitiesToSync(entities);

        // Listen for updatest
        dojo.FnPtr_Void.@delegate callback = OnEntityStateUpdate;
        client.OnEntityStateUpdate(entities[0], new dojo.FnPtr_Void(callback));
        // while (true)
        // {
        //     // client.Update();
        // }

        client.RemoveEntitiesToSync(entities);
    }
}