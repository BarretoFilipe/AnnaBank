﻿using System.Net;

namespace AnnaBank.Abstractions
{
    public sealed record Error(HttpStatusCode StatusCode, string? Message);
}