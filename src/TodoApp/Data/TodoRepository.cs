// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace TodoApp.Data
{
    /// <summary>
    /// A class representing a repository of TODO items. This class cannot be inherited.
    /// </summary>
    public sealed class TodoRepository : ITodoRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TodoRepository"/> class.
        /// </summary>
        /// <param name="clock">The <see cref="IClock"/> to use.</param>
        /// <param name="context">The <see cref="TodoContext"/> to use.</param>
        public TodoRepository(IClock clock, TodoContext context)
        {
            Clock = clock;
            Context = context;
        }

        private IClock Clock { get; }

        private TodoContext Context { get; }

        /// <inheritdoc />
        public async Task<TodoItem> AddItemAsync(string text, CancellationToken cancellationToken = default)
        {
            var item = new TodoItem()
            {
                CreatedAt = Now(),
                Text = text,
            };

            Context.Add(item);

            await Context.SaveChangesAsync(cancellationToken);

            return item;
        }

        /// <inheritdoc />
        public async Task<bool?> CompleteItemAsync(string id, CancellationToken cancellationToken = default)
        {
            var item = await Context.Items.FindAsync(new[] { id }, cancellationToken);

            if (item == null)
            {
                return null;
            }

            if (item.CompletedAt.HasValue)
            {
                return false;
            }

            item.CompletedAt = Now();

            Context.Items.Update(item);

            await Context.SaveChangesAsync(cancellationToken);

            return true;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteItemAsync(string id, CancellationToken cancellationToken = default)
        {
            var item = await Context.Items.FindAsync(new[] { id }, cancellationToken);

            if (item == null)
            {
                return false;
            }

            Context.Items.Remove(item);

            await Context.SaveChangesAsync(cancellationToken);

            return true;
        }

        /// <inheritdoc />
        public async Task<IList<TodoItem>> GetItemsAsync(CancellationToken cancellationToken = default)
        {
            return await Context.Items
                .OrderBy((p) => p.CompletedAt.HasValue)
                .ThenBy((p) => p.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Returns the current date and time.
        /// </summary>
        /// <returns>
        /// The <see cref="DateTimeOffset"/> for the current date and time.
        /// </returns>
        private DateTimeOffset Now() => Clock.GetCurrentInstant().ToDateTimeOffset();
    }
}
