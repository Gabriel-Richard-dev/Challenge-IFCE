using System;
using System.Collections.Generic;

namespace ToDo.Core.Exceptions;

public class ToDoException : Exception
{

    public ToDoException() {  }

    public ToDoException(string message) : base(message)
    {
            
    }

}