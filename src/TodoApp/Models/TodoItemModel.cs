﻿// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace TodoApp.Models;

public class TodoItemModel
{
    public string Id { get; set; }

    public string Text { get; set; }

    public bool IsCompleted { get; set; }

    public string LastUpdated { get; set; }
}
