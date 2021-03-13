using System;
using System.Collections.Generic;
using System.Text;

namespace TranslateVis.DTO
{
    public class OutPut<T> where T : new()
    {

        public bool Succeeded { get; set; }

        public T Data { get; set; }

        public List<ErrorModel> Errors { get; set; }
    }

    public class ErrorModel
    {
        public string Description { get; set; }
    }
}
