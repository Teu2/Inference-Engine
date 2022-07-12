using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    public class HornClause // note - use head and body data structure talked about in week 7 online lecture
    {
        private List<string> _body;
        private string _entailedSymbol; // head

        public HornClause(List<string> body, string head)
        {
            _body = body;
            _entailedSymbol = head;
        }

        public List<string> Body
        {
            get { return _body; }
        }

        public string Head 
        {
            get { return _entailedSymbol; } 
        }
    }
}
