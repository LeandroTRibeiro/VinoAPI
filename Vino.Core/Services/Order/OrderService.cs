using BetterThanYou.Core.DTOs.Order;
using BetterThanYou.Core.Entities;
using BetterThanYou.Core.Interfaces.Order;
using BetterThanYou.Core.Interfaces.Product;
using BetterThanYou.SharedKernel.Enums;

namespace BetterThanYou.Core.Services.Order;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public OrderService(IOrderRepository orderRepository, IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

public async Task<OrderDto> CreateAsync(Guid clienteId, List<OrderItemCreateDto> itens,
    string? observacoes, string? enderecoEntrega, DateTime? dataEntregaPrevista, string criadoPor)
{
    if (itens == null || !itens.Any())
        throw new ArgumentException("O pedido deve ter pelo menos um item", nameof(itens));

    var numeroOrder = await _orderRepository.GenerateNextOrderNumberAsync();

    var order = new Entities.Order
    {
        Id = Guid.NewGuid(),
        NumeroOrder = numeroOrder,
        ClienteId = clienteId,
        DataPedido = DateTime.UtcNow,
        Status = OrderStatus.Pendente,
        Observacoes = observacoes,
        EnderecoEntrega = enderecoEntrega,
        DataEntregaPrevista = dataEntregaPrevista,
        CriadoPor = criadoPor,
        DataCriacao = DateTime.UtcNow,
        Ativo = true,
        Itens = new List<OrderItem>()
    };

    decimal valorTotal = 0;

    foreach (var itemDto in itens)
    {
        var produto = await _productRepository.GetByIdAsync(itemDto.ProdutoId);
        if (produto == null)
            throw new KeyNotFoundException($"Produto {itemDto.ProdutoId} não encontrado");

        if (itemDto.Quantidade <= 0)
            throw new ArgumentException("Quantidade deve ser maior que zero");

        if (produto.QuantidadeEstoque < itemDto.Quantidade)
            throw new InvalidOperationException(
                $"Estoque insuficiente para o produto '{produto.Nome}'. " +
                $"Disponível: {produto.QuantidadeEstoque}, Solicitado: {itemDto.Quantidade}");

        var subtotal = produto.PrecoVenda * itemDto.Quantidade;
        valorTotal += subtotal;

        var orderItem = new OrderItem
        {
            Id = Guid.NewGuid(),
            OrderId = order.Id,
            ProdutoId = produto.Id,
            Quantidade = itemDto.Quantidade,
            PrecoUnitario = produto.PrecoVenda,
            Subtotal = subtotal
        };

        order.Itens.Add(orderItem);

        produto.QuantidadeEstoque -= itemDto.Quantidade;
        await _productRepository.UpdateAsync(produto);
    }

    order.ValorTotal = valorTotal;

    await _orderRepository.CreateAsync(order);

    return await MapToDto(order);
}

    public async Task<List<OrderDto>> GetAllAsync()
    {
        var orders = await _orderRepository.GetAllAsync();
        var dtos = new List<OrderDto>();

        foreach (var order in orders)
        {
            dtos.Add(await MapToDto(order));
        }

        return dtos;
    }

    public async Task<OrderDto> GetByIdAsync(Guid id)
    {
        var order = await _orderRepository.GetByIdAsync(id);

        if (order == null)
            throw new KeyNotFoundException("Pedido não encontrado");

        return await MapToDto(order);
    }
    
    public async Task<OrderDto> UpdateAsync(Guid id, Guid clienteId, List<OrderItemCreateDto> itens,
        string? observacoes, string? enderecoEntrega, DateTime? dataEntregaPrevista, string modificadoPor)
    {
        var order = await _orderRepository.GetByIdAsync(id);

        if (order == null)
            throw new KeyNotFoundException("Pedido não encontrado");

        if (order.Status != OrderStatus.Pendente)
            throw new InvalidOperationException("Só é possível editar pedidos com status Pendente");

        if (itens == null || !itens.Any())
            throw new ArgumentException("O pedido deve ter pelo menos um item", nameof(itens));

        // ← DEVOLVER ESTOQUE DOS ITENS ANTIGOS
        foreach (var itemAntigo in order.Itens)
        {
            var produto = await _productRepository.GetByIdAsync(itemAntigo.ProdutoId);
            if (produto != null)
            {
                produto.QuantidadeEstoque += itemAntigo.Quantidade;
                await _productRepository.UpdateAsync(produto);
            }
        }

        order.ClienteId = clienteId;
        order.Observacoes = observacoes;
        order.EnderecoEntrega = enderecoEntrega;
        order.DataEntregaPrevista = dataEntregaPrevista;
        order.ModificadoPor = modificadoPor;
        order.DataModificacao = DateTime.UtcNow;

        decimal valorTotal = 0;

        // Limpar itens antigos
        order.Itens.Clear();

        // ← ADICIONAR NOVOS ITENS E DECREMENTAR ESTOQUE
        foreach (var itemDto in itens)
        {
            var produto = await _productRepository.GetByIdAsync(itemDto.ProdutoId);
            if (produto == null)
                throw new KeyNotFoundException($"Produto {itemDto.ProdutoId} não encontrado");

            if (itemDto.Quantidade <= 0)
                throw new ArgumentException("Quantidade deve ser maior que zero");

            // Validar estoque disponível
            if (produto.QuantidadeEstoque < itemDto.Quantidade)
                throw new InvalidOperationException(
                    $"Estoque insuficiente para o produto '{produto.Nome}'. " +
                    $"Disponível: {produto.QuantidadeEstoque}, Solicitado: {itemDto.Quantidade}");

            var subtotal = produto.PrecoVenda * itemDto.Quantidade;
            valorTotal += subtotal;

            var orderItem = new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                ProdutoId = produto.Id,
                Quantidade = itemDto.Quantidade,
                PrecoUnitario = produto.PrecoVenda,
                Subtotal = subtotal
            };

            order.Itens.Add(orderItem);

            // ← DECREMENTAR ESTOQUE DOS NOVOS ITENS
            produto.QuantidadeEstoque -= itemDto.Quantidade;
            await _productRepository.UpdateAsync(produto);
        }

        order.ValorTotal = valorTotal;

        var updated = await _orderRepository.UpdateWithItemsAsync(order, order.Itens.ToList());

        return await MapToDto(updated);
    }
    
    public async Task<OrderDto> UpdateStatusAsync(Guid id, OrderStatus status, string modificadoPor)
    {
        var order = await _orderRepository.GetByIdAsync(id);

        if (order == null)
            throw new KeyNotFoundException("Pedido não encontrado");

        var statusAnterior = order.Status;
        
        if (status == OrderStatus.Cancelado && statusAnterior != OrderStatus.Cancelado)
        {
            foreach (var item in order.Itens)
            {
                var produto = await _productRepository.GetByIdAsync(item.ProdutoId);
                if (produto == null)
                    throw new KeyNotFoundException($"Produto {item.ProdutoId} não encontrado");

                produto.QuantidadeEstoque += item.Quantidade;
                await _productRepository.UpdateAsync(produto);
            }
        }
        
        if (statusAnterior == OrderStatus.Cancelado && status != OrderStatus.Cancelado)
        {
            foreach (var item in order.Itens)
            {
                var produto = await _productRepository.GetByIdAsync(item.ProdutoId);
                if (produto == null)
                    throw new KeyNotFoundException($"Produto {item.ProdutoId} não encontrado");

                if (produto.QuantidadeEstoque < item.Quantidade)
                    throw new InvalidOperationException(
                        $"Estoque insuficiente para o produto '{produto.Nome}'. " +
                        $"Disponível: {produto.QuantidadeEstoque}, Necessário: {item.Quantidade}");

                produto.QuantidadeEstoque -= item.Quantidade;
                await _productRepository.UpdateAsync(produto);
            }
        }

        order.Status = status;
        order.ModificadoPor = modificadoPor;
        order.DataModificacao = DateTime.UtcNow;

        if (status == OrderStatus.Entregue && !order.DataEntregaRealizada.HasValue)
        {
            order.DataEntregaRealizada = DateTime.UtcNow;
        }

        var updated = await _orderRepository.UpdateStatusAsync(order);

        return await MapToDto(updated);
    }

    public async Task DeleteAsync(Guid id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
    
        if (order == null)
            throw new KeyNotFoundException("Pedido não encontrado");

        if (order.Status != OrderStatus.Cancelado)
        {
            foreach (var item in order.Itens)
            {
                var produto = await _productRepository.GetByIdAsync(item.ProdutoId);
                if (produto != null)
                {
                    produto.QuantidadeEstoque += item.Quantidade;
                    await _productRepository.UpdateAsync(produto);
                }
            }
        }

        await _orderRepository.DeleteAsync(order);
    }

    private async Task<OrderDto> MapToDto(Entities.Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            NumeroOrder = order.NumeroOrder,
            ClienteId = order.ClienteId,
            ClienteNome = order.Cliente?.NomeFantasia ?? "",
            DataPedido = order.DataPedido,
            Status = order.Status,
            ValorTotal = order.ValorTotal,
            Observacoes = order.Observacoes,
            EnderecoEntrega = order.EnderecoEntrega,
            DataEntregaPrevista = order.DataEntregaPrevista,
            DataEntregaRealizada = order.DataEntregaRealizada,
            CriadoPor = order.CriadoPor,
            DataCriacao = order.DataCriacao,
            ModificadoPor = order.ModificadoPor,
            DataModificacao = order.DataModificacao,
            Ativo = order.Ativo,
            Itens = order.Itens.Select(i => new OrderItemDto
            {
                Id = i.Id,
                OrderId = i.OrderId,
                ProdutoId = i.ProdutoId,
                ProdutoNome = i.Produto?.Nome ?? "",
                Quantidade = i.Quantidade,
                PrecoUnitario = i.PrecoUnitario,
                Subtotal = i.Subtotal
            }).ToList()
        };
    }
}