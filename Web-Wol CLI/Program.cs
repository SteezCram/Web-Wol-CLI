using CommandLine;
using Web_Wol;
using Web_Wol.Commands;




return await Parser.Default.ParseArguments<Options>(args)
        .MapResult(async (Options opts) =>
        {
            try
            {
                switch (opts.Type)
                {
                    case "device":
                        return await Task.FromResult(new DeviceCommand
                        {
                            DatabasePath = opts.DatabasePath,
                            IsAdd = opts.Mode.ToLower() == "a",
                            IsDelete = opts.Mode.ToLower() == "d"
                        }.Run());

                    case "user":
                        return await Task.FromResult(new UserCommand
                        {
                            DatabasePath = opts.DatabasePath, 
                            IsAdd = opts.Mode.ToLower() == "a", 
                            IsDelete = opts.Mode.ToLower() == "d" 
                        }.Run());

                    default:
                        Console.Error.WriteLine("Invalid operation type.");
                        return await Task.FromResult(1);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return 1; // Unhandled error
            }
        },
        errs => Task.FromResult(1)); // Invalid arguments