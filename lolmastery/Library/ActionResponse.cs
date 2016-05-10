using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lolmastery.Library
{
    public class ActionResponse
    {
        public bool ActionState { get; set; }
        public string ActionResult { get; set; }
        public string ErrorMessage { get; set; }
    }
}