﻿namespace ProductApi.DTOs
{
    public class ProductQueryParameters
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 100;
    }
}
