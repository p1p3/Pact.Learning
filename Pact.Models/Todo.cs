﻿using System;

namespace Pact.Models
{
    public class Todo
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public bool Completed { get; set; }
    }

}
