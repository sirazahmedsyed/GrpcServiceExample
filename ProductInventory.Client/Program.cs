namespace ProductInventory.Client;

public class Program
{
    public static async Task Main(string[] args)
    {
        await ExampleUsage();
    }

    private static async Task ExampleUsage()
    {
        try
        {
            using var client = new InventoryClient("https://localhost:7274");

            Console.WriteLine("Creating a product...");
            var product = await client.CreateProductAsync(
                name: "Test Product",
                description: "A test product",
                price: 99.99,
                stock: 100
            );
            Console.WriteLine($"Created product with ID: {product.ProductId}");

            Console.WriteLine("\nGetting the product...");
            var retrievedProduct = await client.GetProductAsync(product.ProductId);

            Console.WriteLine($"Retrieved product details \n" +
                  $"Name: {retrievedProduct.Name}\n" +
                  $"Description: {retrievedProduct.Description}\n" +
                  $"Price: ${retrievedProduct.Price}\n" +
                  $"Stock: {retrievedProduct.Stock}");

            Console.WriteLine("\nUpdating the product...");
            var updatedProduct = await client.UpdateProductAsync(
                productId: product.ProductId,
                name: "Updated Test Product",
                description: "Updated test product",
                price: 149.99,
                stockAdjustment: 100
            );

            Console.WriteLine($"Updated product:\n" +
                    $"Name: {updatedProduct.Name}\n" +
                    $"Description: {updatedProduct.Description}\n" +
                    $"Price: ${updatedProduct.Price}\n" +
                    $"Stock: {updatedProduct.Stock}");
            
            Console.WriteLine("\nDeleting the product...");
            var deleteResponse = await client.DeleteProductAsync(product.ProductId);
            Console.WriteLine($"Delete response: {deleteResponse.Message}");

            Console.WriteLine("\nGetting the product...");
            var productExists = await client.GetProductAsync(product.ProductId);
            
            if (productExists == null)
                Console.WriteLine($"Retrieved product: {retrievedProduct.Name}, Price: ${retrievedProduct.Price}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}



















//using ProductInventory.Server.Services;

//public class InventoryClient
//{
//    private readonly InventoryService.InventoryServiceClient _client;

//    public InventoryClient(string serverUrl)
//    {
//        var channel = GrpcChannel.ForAddress(serverUrl);
//        _client = new InventoryService.InventoryServiceClient(channel);
//    }

//    // Create a new product (Unary)
//    public async Task<ProductResponse> CreateProductAsync(
//        string name,
//        string description,
//        double price,
//        int initialStock)
//    {
//        return await _client.CreateProductAsync(new CreateProductRequest
//        {
//            Name = name,
//            Description = description,
//            Price = price,
//            InitialStock = initialStock
//        });
//    }

//    // Monitor stock changes (Server Streaming)
//    public async Task MonitorProductStockAsync(
//        string productId,
//        Action<StockUpdateResponse> onStockUpdate,
//        CancellationToken cancellationToken = default)
//    {
//        var request = new ProductStockRequest { ProductId = productId };
//        using var call = _client.MonitorStock(request);

//        try
//        {
//            await foreach (var update in call.ResponseStream.ReadAllAsync(cancellationToken))
//            {
//                onStockUpdate(update);
//            }
//        }
//        catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
//        {
//            // Handle cancellation
//        }
//    }

//    // Batch update products (Client Streaming)
//    public async Task<BatchUpdateResponse> BatchUpdateProductsAsync(
//        IEnumerable<UpdateProductRequest> updates)
//    {
//        using var call = _client.BatchUpdateProducts();

//        foreach (var update in updates)
//        {
//            await call.RequestStream.WriteAsync(update);
//        }

//        await call.RequestStream.CompleteAsync();
//        return await call.ResponseAsync;
//    }

//    // Sync inventory (Bidirectional Streaming)
//    public async Task SyncInventoryAsync(
//        Action<InventorySync> onInventoryUpdate,
//        CancellationToken cancellationToken = default)
//    {
//        using var call = _client.SyncInventory();

//        // Read responses
//        _ = Task.Run(async () =>
//        {
//            try
//            {
//                await foreach (var update in call.ResponseStream.ReadAllAsync(cancellationToken))
//                {
//                    onInventoryUpdate(update);
//                }
//            }
//            catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
//            {
//                // Handle cancellation
//            }
//        }, cancellationToken);

//        // Send local updates
//        while (!cancellationToken.IsCancellationRequested)
//        {
//            // Simulate local inventory changes
//            await call.RequestStream.WriteAsync(new InventorySync
//            {
//                ProductId = "test-product",
//                Stock = Random.Shared.Next(1, 100),
//                Timestamp = DateTime.UtcNow.ToString("o")
//            });

//            await Task.Delay(2000, cancellationToken);
//        }

//        await call.RequestStream.CompleteAsync();
//    }


//    static async Task Main(string[] args)
//    {
//        var client = new InventoryClient("https://localhost:5001");

//        // Create a product
//        var product = await client.CreateProductAsync(
//            "Test Product",
//            "Description",
//            29.99,
//            100);

//        Console.WriteLine($"Created product: {product.ProductId}");

//        // Monitor stock
//        var cts = new CancellationTokenSource();
//        _ = client.MonitorProductStockAsync(
//            product.ProductId,
//            update => Console.WriteLine($"Stock update: {update.CurrentStock}"),
//            cts.Token);

//        // Batch update products
//        var updates = new List<UpdateProductRequest>
//    {
//        new() { ProductId = product.ProductId, Price = 34.99 },
//        new() { ProductId = product.ProductId, StockAdjustment = -5 }
//    };

//        var batchResult = await client.BatchUpdateProductsAsync(updates);
//        Console.WriteLine($"Updated {batchResult.ProductsUpdated} products");

//        // Start inventory sync
//        await client.SyncInventoryAsync(
//            update => Console.WriteLine($"Sync update: Product {update.ProductId} - Stock {update.Stock}"),
//            cts.Token);

//        Console.WriteLine("Press any key to exit");
//        Console.ReadKey();
//        cts.Cancel();
//    }

//}

