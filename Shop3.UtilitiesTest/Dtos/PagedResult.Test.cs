﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Shop3.Utilities.Dtos;
using Xunit;

namespace Shop3.UtilitiesTest.Dtos
{
    // bắt càng nhiều trường hợp càng tránh được các rủi ro
    public class PagedResultTest
    {
        [Fact]
        public void Contructor_CreateObject_NotNullObject()
        {
            var pagedResult = new PagedResult<Array>();
            Assert.NotNull(pagedResult);
        }
        [Fact]
        public void Contructor_CreateObject_WithResultNotNull()
        {
            var pagedResult = new PagedResult<Array>();
            Assert.NotNull(pagedResult.Results);
        }
    }
}
