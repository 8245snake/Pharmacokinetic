using System;
using System.Collections.Generic;

namespace Simulator.Factories
{
    public class ParseErrorException : Exception
    {
        public List<string> ExceptParams { get; set; }

        public ParseErrorException(List<string> exceptionsList)
        : base($"{exceptionsList[0]}などの解析に失敗しました。")
        {
            ExceptParams = exceptionsList;
        }
    }
}