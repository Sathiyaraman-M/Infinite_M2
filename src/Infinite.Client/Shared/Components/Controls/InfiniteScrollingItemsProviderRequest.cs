namespace Infinite.Client.Shared.Components.Controls;

public sealed class InfiniteScrollingItemsProviderRequest(int startIndex, CancellationToken cancellationToken)
{
    public int StartIndex { get; } = startIndex;
    public CancellationToken CancellationToken { get; } = cancellationToken;
}

public delegate Task<IEnumerable<T>> InfiniteScrollingItemsProviderRequestDelegate<T>(InfiniteScrollingItemsProviderRequest context);