using Grpc.Core;
using Grpc.Net.Client;
using ProductInventory.Server;
using Grpc.Reflection.V1Alpha;

namespace ProductInventory.Client;

public class InventoryClient : IDisposable
{
    private readonly InventoryService.InventoryServiceClient _client;
    private readonly GrpcChannel _channel;

    public InventoryClient(string serverUrl)
    {
        _channel = GrpcChannel.ForAddress(serverUrl);
        _client = new InventoryService.InventoryServiceClient(_channel);
    }
    public async Task<ProductResponse> CreateProductAsync(string name, string description, double price, int stock)
    {
        var request = new CreateProductRequest
        {
            Name = name,
            Description = description,
            Price = price,
            Stock = stock
        };

        return await _client.CreateProductAsync(request);
    }

    public async Task<ProductResponse> GetProductAsync(string productId)
    {
        var request = new GetProductRequest { ProductId = productId };
        return await _client.GetProductAsync(request);
    }

    public async Task<ProductResponse> UpdateProductAsync(string productId,
        string name = null, string description = null, double price = 0, int stockAdjustment = 0)
    {
        var request = new UpdateProductRequest
        {
            ProductId = productId,
            Name = name,
            Description = description,
            Price = price,
            StockAdjustment = stockAdjustment
        };

        return await _client.UpdateProductAsync(request);
    }

    public async Task<DeleteProductResponse> DeleteProductAsync(string productId)
    {
        var request = new DeleteProductRequest { ProductId = productId };
        return await _client.DeleteProductAsync(request);
    }

    public void Dispose()
    {
        _channel.Dispose();
    }
}
