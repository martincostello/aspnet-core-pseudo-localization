// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using Humanizer;
using TodoApp.Data;
using TodoApp.Models;

namespace TodoApp.Services;

/// <summary>
/// A class representing the class for managing TODO items. This class cannot be inherited.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="TodoService"/> class.
/// </remarks>
/// <param name="repository">The <see cref="ITodoRepository"/> to use.</param>
public sealed class TodoService(ITodoRepository repository) : ITodoService
{
    /// <inheritdoc />
    public Task AddItemAsync(string text, CancellationToken cancellationToken)
    {
        return repository.AddItemAsync(text, cancellationToken);
    }

    /// <inheritdoc />
    public Task<bool?> CompleteItemAsync(string id, CancellationToken cancellationToken)
    {
        return repository.CompleteItemAsync(id, cancellationToken);
    }

    /// <inheritdoc />
    public Task<bool> DeleteItemAsync(string id, CancellationToken cancellationToken)
    {
        return repository.DeleteItemAsync(id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TodoListViewModel> GetListAsync(CancellationToken cancellationToken)
    {
        var items = await repository.GetItemsAsync(cancellationToken);

        var result = new TodoListViewModel();

        foreach (var todo in items)
        {
            result.Items.Add(MapItem(todo));
        }

        return result;
    }

    private static TodoItemModel MapItem(TodoItem item)
    {
        return new TodoItemModel()
        {
            Id = item.Id,
            IsCompleted = item.CompletedAt.HasValue,
            LastUpdated = (item.CompletedAt ?? item.CreatedAt).Humanize(),
            Text = item.Text,
        };
    }
}
