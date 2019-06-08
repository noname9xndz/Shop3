using Shop3.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Shop3.UtilitiesTest.Dtos
{
    public class GenericResultTest
    {
        [Fact]
        public void Contructor_CreateObjectNotNull_NoParam()
        {
            var genericResult = new GenericResult();
            Assert.NotNull(genericResult);
        }
        [Fact]
        public void Contructor_SuccessIsTrue_OneParam()
        {
            var genericResult = new GenericResult(true);
            Assert.True(genericResult.Success);
        }

        [Fact]
        public void Contructor_SuccessAndDataHasValue_TwoParam1()
        {
            var genericResult = new GenericResult(true,"test");
            Assert.Equal("test",genericResult.Message);
        }

        [Fact]
        public void Contructor_SuccessAndDataHasValue_TwoParam2()
        {
            var genericResult = new GenericResult(true,new object());
            Assert.NotNull(genericResult.Data);
        }
    }
}
